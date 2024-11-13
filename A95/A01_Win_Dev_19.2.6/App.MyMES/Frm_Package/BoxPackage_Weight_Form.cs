using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;
using App.Model;
using System.IO.Ports;


namespace App.MyMES
{
    public partial class BoxPackage_Weight_Form : FrmBase
    {

        string xmlProdOrder;
        string xmlPart;
        string xmlExtraData;
        string xmlStation;

        string mBoxSN = string.Empty;
        string mWeight = string.Empty;

        string weightUnit = string.Empty;
        double weightBase = 0;
        double weightBaseOffsetU = 0;
        double weightBaseOffsetL = 0;

        Dictionary<WeightConfig, string> keyValuePairsWeight = new Dictionary<WeightConfig, string>();
        double mUpperLimit = 0;
        double mLowerLimit = 0;

        private bool isShowNgDialog = false;
        public BoxPackage_Weight_Form()
        {
            InitializeComponent();            
            
        }

        /// <summary>
        /// 刷新重新
        /// </summary>
        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            SwitchSerialPort(false);
            keyValuePairsWeight.Clear();
            weightBase = 0;
            weightBaseOffsetU = 0;
            weightBaseOffsetL = 0;
            weightUnit = "";

            mUpperLimit = 0;
            mLowerLimit = 0;
            base.Btn_Refresh_Click(sender, e);
        }



        /// <summary>
        /// 重写确认菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet dtSet = base.PartSelectSVC.GetProductionOrderDetailDef(Com_PO.EditValue.ToString());
                if (dtSet == null || dtSet.Tables.Count == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20070", "NG", List_Login.Language);
                    return;
                }
                DataTable dtPara = dtSet.Tables[0];
                DataRow[] drUpc = dtPara.Select("Description='UPC'");

                DataSet DS_StationTypeDef = PartSelectSVC.MESGetStationTypeParameter(List_Login.StationTypeID);
                if (DS_StationTypeDef.Tables.Count > 0)
                {
                    DataRow[] DR_IsShowNgDialog = DS_StationTypeDef.Tables[0].Select("Description='IsShowNgDialog'");
                    if (DR_IsShowNgDialog.Length > 0)
                    {
                        isShowNgDialog = DR_IsShowNgDialog[0]["Content"].ToString().Trim() == "1";
                    }
                }
                else
                {
                    isShowNgDialog = false;
                }



                base.Btn_ConfirmPO_Click(sender, e);


                #region 20220301 定义一个参数
                string configRes = "0";
                DataSet BoxWeightConfig = base.PartSelectSVC.GetMesPartAndPartFamilyDetail(int.Parse(Com_Part.EditValue.ToString()), "PackBoxWeightLimit", "PackBoxWeightLimit", out configRes);

                if (configRes != "1")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, configRes, "NG", List_Login.Language, new string[] { "" }, configRes, "");
                    Btn_Refresh_Click(this, null);
                    return;
                }
                keyValuePairsWeight.Clear();
                for (int i = 0; i < BoxWeightConfig.Tables[0].Rows.Count; i++)
                {
                    WeightConfig weightConfig;
                    string ItemNameData = BoxWeightConfig.Tables[0].Rows[i]["itemsName"].ToString().Trim().ToUpper();
                    if (!Enum.TryParse(ItemNameData, out weightConfig))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "70022", "NG", List_Login.Language, new string[] { ItemNameData }, configRes, "");
                        Btn_Refresh_Click(this, null);
                        return;

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
                    Btn_Refresh_Click(this, null);
                    return;
                }

                te_base_UL.Text = weightBaseOffsetU.ToString();
                te_base_LL.Text = weightBaseOffsetL.ToString();
                te_base.Text = weightBase.ToString();
                te_unit.Text = weightUnit;

                string result = initSerialPort();

                if (result != "1")
                {
                    string strMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { strMsg }, result);
                    Btn_Refresh_Click(this, null);
                    return;
                }
                DataTable tmpDt = base.Grid_RouteData.DataSource as DataTable;
                if (tmpDt?.Rows.Count <= 0 || tmpDt?.Select($"StationTypeID = {List_Login.StationTypeID}").Length <= 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "70019", "NG", List_Login.Language);
                    Btn_Refresh_Click(this, null);
                    return;
                }
                DataRow[] drGeneratePalletSN = dtPara.Select("Description='IsGeneratePalletSN'");
                xmlProdOrder = ("<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>");
                xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                xmlExtraData = "<ExtraData EmployeeId=\"" + List_Login.EmployeeID.ToString() + "\"> </ExtraData>";
                xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";

                SetQC(isShowNgDialog);
                Edt_BoxSN.Enabled = true;
                Edt_BoxSN.Focus();
                Edt_BoxSN.SelectAll();
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private string initSerialPort()
        {
            string tmpPortName = string.Empty;
            DataSet ds = base.PartSelectSVC.GetmesStationConfig("Port", List_Login.StationID.ToString());
            if (ds?.Tables?[0].Rows.Count <= 0)
            {
                //return "70015";
                tmpPortName = "COM1";
            }
            else
            {
                
                DataRow[] portNames = ds.Tables[0].Select("Name = 'Port'");
                tmpPortName = portNames[0]["Value"].ToString();
            }

            int tmpBaudRate = 9600;
            if (!SerialPort.GetPortNames().Contains(tmpPortName))
            {
                return "70016";
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
            port1.PortName = tmpPortName;
            port1.BaudRate = tmpBaudRate;
            port1.Parity = System.IO.Ports.Parity.None;
            port1.DataBits = 8;
            port1.StopBits = System.IO.Ports.StopBits.One;
            port1.DataReceived += Port1_DataReceived;
            return SwitchSerialPort()?"1": "70014";
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


        private void Port1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string tmpData = port1.ReadExisting();
            mWeight = tmpData.ToUpper().Trim();
            this.BeginInvoke(new MethodInvoker(() =>
            {
                double currentWeight = 0;
                mWeight = mWeight.Replace(" ", "").Replace(weightUnit.ToUpper().ToString(), "").Trim();

                if (!double.TryParse(mWeight, out currentWeight))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "70017", "NG", List_Login.Language, new string[] { mWeight });
                    return;
                }
                tb_weight.Text = currentWeight.ToString();

                tb_weight.ForeColor = (currentWeight > mUpperLimit || currentWeight < mLowerLimit) ? Color.Red : Color.Green;

                if (Edt_BoxSN.Enabled || string.IsNullOrEmpty(Edt_BoxSN.Text.Trim()))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"SN : {Edt_BoxSN.Text.ToString()}, Please place the product correctly on the electronic scale again");
                    return;
                }

                if (currentWeight < mLowerLimit || currentWeight > mUpperLimit)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"SN : {Edt_BoxSN.Text.ToString()}, Scale [{currentWeight}] no match,   SPEC : {mLowerLimit} ~ {mUpperLimit}");
                    SwitchSerialPort(false);
                    string tmpNgRes = public_.uspUpdateBoxNGV2(Com_Part.EditValue.ToString(), Com_PO.EditValue.ToString(), Edt_BoxSN.Text.Trim(), List_Login, 
                        currentWeight.ToString(),PartSelectSVC);
                    if (tmpNgRes != "OK")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, tmpNgRes, "NG", List_Login.Language, new string[] { mWeight });
                        LogNet.LogEor("error", "scale update NG  failed.");
                    }
                    Edt_BoxSN.Enabled = true;
                    Edt_BoxSN.Text = string.Empty;
                    Edt_BoxSN.Focus();
                    return;
                }

                string result = PartSelectSVC.uspUpdateBoxV2(Com_Part.EditValue.ToString(), Com_PO.EditValue.ToString(), Edt_BoxSN.Text.Trim(), List_Login, currentWeight.ToString());
                SwitchSerialPort(false);
                if (result != "1")
                {
                    Console.WriteLine("更新失败");
                    string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Edt_BoxSN.Text.ToString(), ProMsg }, result);
                    Edt_BoxSN.Enabled = true;
                    Edt_BoxSN.Focus();
                    Edt_BoxSN.SelectAll();
                    return;
                }
                MessageInfo.Add_Info_MSG(Edt_MSG, "70018", "OK", List_Login.Language, new string[] { Edt_BoxSN.Text });
                Edt_BoxSN.Enabled = true;
                Edt_BoxSN.Text = string.Empty;
                Edt_BoxSN.Focus();
            }));
        }


        private void Edt_BoxSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || string.IsNullOrEmpty(Edt_BoxSN.Text.Trim()))
                return;

            #region 条码校验

            string result = "0";
            result = PartSelectSVC.uspPalletCheck(Edt_BoxSN.Text.Trim(), xmlProdOrder, xmlPart, xmlStation, xmlExtraData, "BOX");
            if (result != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Edt_BoxSN.Text.Trim(), ProMsg }, result);
                Edt_BoxSN.SelectAll();
                return;
            }
            //获取箱中其中一个一个条码
            string S_SN = public_.boxSNSecondCheck(Edt_BoxSN.Text.Trim(), PartSelectSVC);

            /// //二次检查是否有指定线路  20231214
            var tmpSecondRet = PartSelectSVC.GetmesUnitStateSecond(Com_Part.EditValue.ToString(), Com_PartFamily.EditValue.ToString(), "", List_Login.LineID.ToString(),
                List_Login.StationTypeID, Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString(), S_SN);

            if (tmpSecondRet.StartsWith("Error", StringComparison.OrdinalIgnoreCase))
            {
                //MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + mainSN });
                MessageInfo.Add_Info_MSG(Edt_MSG, "NG", tmpSecondRet);
                Edt_BoxSN.Focus();
                Edt_BoxSN.SelectAll();
                return;
            }

            string  S_UnitStateID = tmpSecondRet;
            if (string.IsNullOrEmpty(S_UnitStateID))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                Edt_BoxSN.Focus();
                Edt_BoxSN.SelectAll();
                return;
            }
            /////////////////////////////////////////////////////

            //包装校验
            PartSelectSVC.uspCallProcedure("uspPackageRouteCheck", Edt_BoxSN.Text.Trim(), xmlProdOrder, xmlPart,
                                                        xmlStation, xmlExtraData, "", ref result);

            if (result != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Edt_BoxSN.Text.ToString(), ProMsg }, result);
                Edt_BoxSN.SelectAll();
                return;
            }
            #endregion





            if (!SwitchSerialPort())
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "70014", "NG", List_Login.Language, new string[] {  }, "70014", "");
                return;
            }
            tb_weight.Text = "----";
            tb_weight.ForeColor = Color.Black;
            Edt_BoxSN.Enabled = false;
            MessageInfo.Add_Info_MSG(Edt_MSG, "70020", "NG", List_Login.Language, new string[] { Edt_BoxSN.Text,"", "Error_RE" });
            tb_weight.Focus();
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
        private void btn_reset_Click(object sender, EventArgs e)
        {
            SwitchSerialPort(false);
            Edt_BoxSN.Enabled = true;
            Edt_BoxSN.Focus();
            Edt_BoxSN.SelectAll();
        }
    }

}