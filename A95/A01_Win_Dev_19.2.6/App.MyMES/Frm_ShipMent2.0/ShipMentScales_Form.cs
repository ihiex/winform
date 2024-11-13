using App.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.MyMES.PartSelectService;
using App.MyMES.mesPackageService;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using System.Runtime.InteropServices;
using App.MyMES.DataCommitService;
using DevExpress.DataProcessing;

namespace App.MyMES
{
    public partial class ShipMentScales_Form : DevExpress.XtraEditors.XtraForm
    {
        public Public_ public_ = new Public_();
        public LoginList List_Login = new LoginList();
        public PartSelectSVCClient PartSelectSVC;
        public DataCommitSVCClient DataCommitSVC;
        public ImesPackageSVCClient mesPackageSVC;

        Dictionary<WeightConfig, string> keyValuePairsWeight = new Dictionary<WeightConfig, string>();

        string weightUnit = string.Empty;
        double weightBase = 0;
        double weightBaseOffsetU = 0;
        double weightBaseOffsetL = 0;
        double mUpperLimit = 0;
        double mLowerLimit = 0;

        private bool isAutoStation = false;
        private string ComPort = "COM1";
        private int ComBaudRate = 9600;
        private string mWeight;
        private SerialPort port1;


        public ShipMentScales_Form()
        {
            InitializeComponent();
        }






        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShipMent_Form_Load(object sender, EventArgs e)
        {
            try
            {
                
                Btn_Refresh_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }



        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            txtSSCC18.Text = string.Empty;
            txtSSCC18.Enabled = true;
            txtWeight.Text = string.Empty;
            txtWeight.Enabled = false;
            SwitchSerialPort(false);

        }

        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            List_Login = this.Tag as LoginList;
            PartSelectSVC = PartSelectFactory.CreateServerClient();
            mesPackageSVC = ImesPackageFactory.CreateServerClient();
            DataCommitSVC = DataCommitFactory.CreateServerClient();

            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_LineID = List_Login.LineID.ToString();
            public_.AddPartFamilyType(Com_PartFamilyType, Grid_PartFamilyType);
            panelCtrMain.Enabled = false;
            txtSSCC18.Enabled = true;
            txtSSCC18.Text = string.Empty;
            txtWeight.Text = string.Empty;
            txtWeight.Enabled = false;
            this.Com_PartFamily.Enabled = true;
            this.Com_PartFamilyType.Enabled = true;
            this.Com_Part.Enabled = true;
            this.Com_PO.Enabled = true;
            this.Btn_ConfirmPO.Enabled = true;
            this.Btn_Refresh.Enabled = false;


            btnReset_Click(null, null);


        }

        private void Com_PartFamilyType_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartFamilyTypeID = Com_PartFamilyType.EditValue.ToString();
                public_.AddPartFamily(Com_PartFamily, S_PartFamilyTypeID, Grid_PartFamily);
            }
            catch (Exception ex)
            {
                //错误信息:{0}.
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void Com_PartFamily_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                public_.AddPart(Com_Part, S_PartFamilyID, Grid_Part);
            }
            catch (Exception ex)
            {
                //错误信息:{0}.
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void Com_Part_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartID = Com_Part.EditValue.ToString();
                public_.AddPOAll(Com_PO, S_PartID, List_Login.LineID.ToString(), Grid_PO);
            }
            catch (Exception ex)
            {
                //错误信息:{0}.
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            try
            {
                if (Com_PO.Text.Trim() == "")
                {
                    //工单不能为空,请确认.
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20114", "NG", List_Login.Language);
                    return;
                }
                else
                {
                    if (Com_PartFamilyType.EditValue == null || string.IsNullOrEmpty(Com_PartFamilyType.Text.ToString()))
                    {
                        //未选择料号类别,请确认.
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20115", "NG", List_Login.Language);
                        return;
                    }
                    if (Com_PartFamily.EditValue == null || string.IsNullOrEmpty(Com_PartFamily.Text.ToString()))
                    {
                        //未选择料号群,请确认.
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20116", "NG", List_Login.Language);
                        return;
                    }
                    if (Com_Part.EditValue == null || string.IsNullOrEmpty(Com_Part.Text.ToString()))
                    {
                        //未选择料号,请确认.
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20117", "NG", List_Login.Language);
                        return;
                    }
                    if (Com_PO.EditValue == null || string.IsNullOrEmpty(Com_PO.Text.ToString()))
                    {
                        //未选择工单，请确认.
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20118", "NG", List_Login.Language);
                        return;
                    }

                    int PartID = Convert.ToInt32(Com_Part.EditValue.ToString());

                    if(!GetSetting())
                        return;

                    // 查询模板
                    string S_StationTypeID = List_Login.StationTypeID.ToString();
                    string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                    string S_PartID = Com_Part.EditValue.ToString();
                    string S_ProductionOrderID = Com_PO.EditValue.ToString();
                    string S_LoginLineID = List_Login.LineID.ToString();

                    this.Com_PartFamily.Enabled = false;
                    this.Com_PartFamilyType.Enabled = false;
                    this.Com_Part.Enabled = false;
                    this.Com_PO.Enabled = false;
                    this.Btn_ConfirmPO.Enabled = false;
                    this.Btn_Refresh.Enabled = true;
                    this.panelCtrMain.Enabled = true;
                    txtSSCC18.Focus();
                }
            }
            catch (Exception ex)
            {
                //错误信息:{0}.
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }
        private bool GetSetting()
        {
            bool result = false;
            
            string tmpRes = PartSelectSVC.GetMesStationAndStationTypeDetail(List_Login.StationID, "AutoStation", "", out string tmpAutoStation);
            if (tmpRes == "1" && tmpAutoStation == "1")
            {
                isAutoStation = true;
                tmpRes = PartSelectSVC.GetMesStationAndStationTypeDetail(List_Login.StationID, "Port", "", out string tmpPort);
                if (tmpRes == "1" && !string.IsNullOrEmpty(tmpPort))
                {
                    ComPort = tmpPort;
                }

                tmpRes = PartSelectSVC.GetMesStationAndStationTypeDetail(List_Login.StationID, "BaudRate", "", out string tmpBaudRate);
                if (tmpRes == "1" && !string.IsNullOrEmpty(tmpBaudRate))
                {
                    ComBaudRate = int.Parse(tmpBaudRate);
                }

                if (!SerialPort.GetPortNames().Contains(ComPort))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"Please check PC serial port, no found [{ComPort}]");
                    return result;
                }


                if (port1 != null)
                {
                    if (port1.IsOpen)
                    {
                        port1.Close();
                    }
                    port1 = null;
                }

                port1 = new System.IO.Ports.SerialPort();
                port1.PortName = ComPort;
                port1.BaudRate = ComBaudRate;
                port1.Parity = System.IO.Ports.Parity.None;
                port1.DataBits = 8;
                port1.StopBits = System.IO.Ports.StopBits.One;
                port1.DataReceived += Port1_DataReceived;

                result = SwitchSerialPort();
                if (!result)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "NG", "Open serial port failed.");
                }
            }
            return result = true;
        }

        private bool SetupWeightConfig(string PartId)
        {
            bool result = false;
            #region 20220301 定义一个参数
            string configRes = "0";
            DataSet BoxWeightConfig = PartSelectSVC.GetMesPartAndPartFamilyDetail(int.Parse(PartId), "ShippingPalletWeightLimit", "ShippingPalletWeightLimit", out configRes);

            if (configRes != "1")
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, configRes, "NG", List_Login.Language, new string[] { "" }, configRes, "");
                txtSSCC18.SelectAll();
                return result;
            }
            keyValuePairsWeight.Clear();
            for (int i = 0; i < BoxWeightConfig.Tables[0].Rows.Count; i++)
            {
                WeightConfig weightConfig;
                string ItemNameData = BoxWeightConfig.Tables[0].Rows[i]["itemsName"].ToString().Trim().ToUpper();
                if (!Enum.TryParse(ItemNameData, out weightConfig))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "70022", "NG", List_Login.Language, new string[] { ItemNameData }, configRes, "");
                    txtSSCC18.SelectAll();
                    return result;

                }

                keyValuePairsWeight.Add(weightConfig, BoxWeightConfig.Tables[0].Rows[i]["itemsValue"].ToString());
            }

            weightBase = double.Parse(keyValuePairsWeight[WeightConfig.BASE]);
            weightBaseOffsetU = double.Parse(keyValuePairsWeight[WeightConfig.UL]);
            weightBaseOffsetL = double.Parse(keyValuePairsWeight[WeightConfig.LL]);
            weightUnit = keyValuePairsWeight[WeightConfig.UNIT];
            #endregion


            mUpperLimit = weightBase + weightBaseOffsetU;
            mLowerLimit = weightBase + weightBaseOffsetL;
            if (mUpperLimit <= mLowerLimit || mUpperLimit < 0 || mLowerLimit < 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "70012", "NG", List_Login.Language);
                txtSSCC18.SelectAll();
                return result;
            }

            te_base_UL.Text = weightBaseOffsetU.ToString();
            te_base_LL.Text = weightBaseOffsetL.ToString();
            te_base.Text = weightBase.ToString();
            te_unit.Text = weightUnit;
            return result = true;
        }
        private bool GetSetting2()
        {
            bool result = false;

            string configRes = "0";
            DataSet BoxWeightConfig = PartSelectSVC.P_DataSet($"SELECT * FROM dbo.CO_WH_ProjectBaseNew WHERE FProjectNO = '{Com_PartFamilyType.Text}'");

            if (BoxWeightConfig == null || BoxWeightConfig.Tables.Count <= 0 || BoxWeightConfig.Tables[0].Rows.Count <= 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "NG", "Query package scales config failed. please check setup.");
                return result;
            }

            if (!BoxWeightConfig.Tables[0].Columns.Contains("ScalesOffset"))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "NG", "please setup field ('ScalesOffset') of the database.");
                return result;
            }


            PartSelectSVC.SetupPackageScalesDesc();


            int FCountByPallet = int.Parse(BoxWeightConfig.Tables[0].Rows[0]["FCountByPallet"].ToString());
            int FCountByCase = int.Parse(BoxWeightConfig.Tables[0].Rows[0]["FCountByPallet"].ToString());
            double FWeightByCase = double.Parse(BoxWeightConfig.Tables[0].Rows[0]["FWeightByCase"].ToString());
            double FWeightByPallet = double.Parse(BoxWeightConfig.Tables[0].Rows[0]["FWeightByPallet"].ToString());
            double FTotalWeight = double.Parse(BoxWeightConfig.Tables[0].Rows[0]["FTotalWeight"].ToString());
            double ScalesOffset = string.IsNullOrEmpty(BoxWeightConfig.Tables[0].Rows[0]["ScalesOffset"].ToString())?0: double.Parse((BoxWeightConfig.Tables[0].Rows[0]["ScalesOffset"] ?? "0").ToString());


            weightBase = FCountByPallet * FWeightByCase + FWeightByPallet;
            weightBaseOffsetU = + ScalesOffset;
            weightBaseOffsetL = - ScalesOffset;
            weightUnit = "KG";

            mUpperLimit = weightBase + ScalesOffset;
            mLowerLimit = weightBase - ScalesOffset;


            te_base_UL.Text = weightBaseOffsetU.ToString();
            te_base_LL.Text = weightBaseOffsetL.ToString();
            te_base.Text = weightBase.ToString();
            te_unit.Text = weightUnit;


            string tmpRes = PartSelectSVC.GetMesStationAndStationTypeDetail(List_Login.StationID,"AutoStation", "", out string tmpAutoStation);
            if (tmpRes == "1" && tmpAutoStation == "1")
            {
                isAutoStation = true;
                tmpRes = PartSelectSVC.GetMesStationAndStationTypeDetail(List_Login.StationID, "Port", "", out string tmpPort);
                if (tmpRes == "1" && !string.IsNullOrEmpty(tmpPort))
                {
                    ComPort = tmpPort;
                }

                tmpRes = PartSelectSVC.GetMesStationAndStationTypeDetail(List_Login.StationID, "BaudRate", "", out string tmpBaudRate);
                if (tmpRes == "1" && !string.IsNullOrEmpty(tmpBaudRate))
                {
                    ComBaudRate = int.Parse(tmpBaudRate);
                }

                if (!SerialPort.GetPortNames().Contains(ComPort))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"Please check PC serial port, no found [{ComPort}]");
                    return result;
                }


                if (port1 != null)
                {
                    if (port1.IsOpen)
                    {
                        port1.Close();
                    }
                    port1 = null;
                }

                port1 = new System.IO.Ports.SerialPort();
                port1.PortName = ComPort;
                port1.BaudRate = ComBaudRate;
                port1.Parity = System.IO.Ports.Parity.None;
                port1.DataBits = 8;
                port1.StopBits = System.IO.Ports.StopBits.One;
                port1.DataReceived += Port1_DataReceived;

                result = SwitchSerialPort();
                if (!result)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "NG", "Open serial port failed.");
                }
            }
            return result = true;
        }


        private void Port1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string tmpData = port1.ReadExisting();
            mWeight = tmpData.ToUpper().Trim();
            this.BeginInvoke(new MethodInvoker(() =>
            {
                double currentWeight = 0;
                mWeight = mWeight.Replace(" ", "").Replace(weightUnit.ToString(), "").Trim();

                if (!double.TryParse(mWeight, out currentWeight))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "70017", "NG", List_Login.Language, new string[] { mWeight });
                    return;
                }
                txtWeight.Text = currentWeight.ToString();
                txtWeight.ForeColor = (currentWeight > mLowerLimit && currentWeight < mUpperLimit) ? Color.Green : Color.Red;

                if (txtSSCC18.Enabled || string.IsNullOrEmpty(txtSSCC18.Text.Trim()) || currentWeight < mLowerLimit || currentWeight > mUpperLimit)
                {
                    return;
                }

                string result = PartSelectSVC.ShipMentScalesCommint( txtSSCC18.Text.Trim(), List_Login, currentWeight.ToString(), Com_PO.EditValue.ToString());
                SwitchSerialPort(false);
                if (result != "1")
                {
                    Console.WriteLine("更新失败");
                    string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { txtSSCC18.Text.ToString(), ProMsg }, result);
                    txtSSCC18.Enabled = true;
                    txtSSCC18.Focus();
                    txtSSCC18.SelectAll();
                    return;
                }
                MessageInfo.Add_Info_MSG(Edt_MSG, "70018", "OK", List_Login.Language, new string[] { txtSSCC18.Text });
                txtSSCC18.Enabled = true;
                txtSSCC18.Text = string.Empty;
                txtSSCC18.Focus();
            }));
        }
        /// <summary>
        /// 开关串口
        /// </summary>
        /// <param name="isOpen"></param>
        /// <returns></returns>
        private bool SwitchSerialPort(bool isOpen = true)
        {
            bool reslut = false;
            if (isOpen)
            {
                try
                {
                    if (port1 != null)
                    {
                        if (!port1.IsOpen)
                        {
                            port1.Open();
                            port1.DiscardInBuffer();
                        }
                        reslut = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("打开串口失败");
                }
            }
            else
            {
                try
                {
                    if (port1 != null)
                    {
                        if (port1.IsOpen)
                        {
                            port1.Close();
                        }
                        reslut = true;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("关闭串口失败");
                }
            }
            return reslut;
        }
        private void ShipMent_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            PartSelectSVC.Close();
            mesPackageSVC.Close();
            DataCommitSVC.Close();
            if (isAutoStation)
            {
                SwitchSerialPort(false);
                port1?.Dispose();
            }
        }

        private void txtSCC18_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Enter)
                return;

            string sscc18 = txtSSCC18.Text.Trim();
            //获取料号
            string UnitPartId = string.Empty;
            string UnitPoId = string.Empty;
            var partAndPo = PartSelectSVC.GetPartIdByShippingPallet(sscc18);
            if (partAndPo == null)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20043", "NG", List_Login.Language);
                txtSSCC18.SelectAll();
                return;
            }

            UnitPartId = partAndPo[0];
            UnitPoId = partAndPo[1];
            if (!SetupWeightConfig(UnitPartId))
            {
                txtSSCC18.SelectAll();
                return;
            }


            //检查箱码状态
            string palletState = PartSelectSVC.CheckShipmentPalletState(sscc18, Com_PO.EditValue.ToString());
            if (palletState != "1")
            {
                MessageInfo.Add_Info_MSG(Edt_MSG , palletState, "NG", List_Login.Language);
                txtSSCC18.SelectAll();
                return;
            }

            string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + UnitPoId + "\"> </ProdOrder>";
            string xmlPart = "<Part PartID=\"" + UnitPartId + "\"> </Part>";
            string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
            string outString = string.Empty;
            PartSelectSVC.uspCallProcedure("uspShippingPalletCheck", sscc18, xmlProdOrder, xmlPart, xmlStation, null, null, ref outString);
            if (outString != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { sscc18, ProMsg }, outString);
                txtSSCC18.SelectAll();
                return;
            }


            txtSSCC18.Enabled = false;
            if (!isAutoStation)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "RE", $"please enter  weight of the pallet is qualified.");
                txtWeight.Enabled = true;
                txtWeight.Focus();
            }
            else
            {
                MessageInfo.Add_Info_MSG(Edt_MSG,"RE", "Please put the delivered product on the electronic scale steadily...");
                SwitchSerialPort();
            }
        }

        private void txtWeight_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Enter)
                return;

            string tmpWeight = txtWeight.Text.Trim();
            if (!double.TryParse(tmpWeight, out double mCurrentWeight))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG,  "NG",$"Please enter the correct value");
                txtWeight.SelectAll();
                return;
            }

            if (mCurrentWeight <= 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"Please enter the correct value");
                txtWeight.SelectAll();
                return;
            }

            if (mCurrentWeight > mUpperLimit || mCurrentWeight < mLowerLimit)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"pallet scales must be >= '{mLowerLimit}' and <= '{mUpperLimit}', please check it.");
                txtWeight.SelectAll();
                return;
            }

            string result = PartSelectSVC.ShipMentScalesCommint(txtSSCC18.Text.Trim(), List_Login, mCurrentWeight.ToString(),Com_PO.EditValue.ToString());
            if (result != "1")
            {
                Console.WriteLine("更新失败");
                string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { txtSSCC18.Text.ToString(), ProMsg }, result);
                txtSSCC18.Enabled = true;
                txtSSCC18.Focus();
                txtSSCC18.SelectAll();
                return;
            }
            MessageInfo.Add_Info_MSG(Edt_MSG, "OK",$"Pallet Number:{txtSSCC18.Text} The weight of the pallet is qualified.");
            txtSSCC18.Enabled = true;
            txtSSCC18.Text = string.Empty;
            txtWeight.Text = string.Empty;
            txtWeight.Enabled = false;
            txtSSCC18.Focus();
        }
    }
    enum WeightConfig
    {
        [Description("箱重标准值")]
        BASE,
        [Description("重量单位")]
        UNIT,
        [Description("箱重标准值上限")]
        UL,
        [Description("箱重标准值下限")]
        LL,
    };
}
