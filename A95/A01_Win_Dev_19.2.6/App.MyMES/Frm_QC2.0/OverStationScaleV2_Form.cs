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
using App.Model;
using System.IO.Ports;

namespace App.MyMES
{
    public partial class OverStationScaleV2_Form : FrmBase
    {
        string S_UnitID = "";
        bool COF = false;
        string IsAuto = "0";

        string S_IsCheckPO = "1";
        string S_IsCheckPN = "1";


        private string strPort = "COM1";
        string ReadCmd = "READY";
        string TestSN = "TESTSHELL001";

        string S_ProductionOrderID = "";
        DataSet dsBOM = null;
        DataSet DS_StationTypeDef;
        bool isEnableCOF = false;


        string mWeight = string.Empty;

        string weightUnit = string.Empty;
        double weightBase = 0;
        double weightBaseOffsetU = 0;
        double weightBaseOffsetL = 0;

        Dictionary<WeightConfig, string> keyValuePairsWeight = new Dictionary<WeightConfig, string>();
        double mUpperLimit = 0;
        double mLowerLimit = 0;
        private SNDataInfo gSnData;
        private bool isShowNgDialog = false;
        public OverStationScaleV2_Form()
        {
            InitializeComponent();
        }

        private DataSet GetStationData()
        {
            DataSet dsStationDetail = PartSelectSVC.GetPLCSeting("", List_Login.StationID.ToString());
            DS_StationTypeDef = PartSelectSVC.MESGetStationTypeParameter(List_Login.StationTypeID);

            DataRow[] drREAD = dsStationDetail.Tables[0].Select("Name='READY'");
            if (drREAD.Length > 0) ReadCmd = drREAD[0]["Value"].ToString(); else ReadCmd = "READY";

            DataRow[] drPort = dsStationDetail.Tables[0].Select("Name='Port'");
            if (drPort.Length > 0) strPort = drPort[0]["Value"].ToString(); else strPort = "COM1";

            DataRow[] drAutoStation = dsStationDetail.Tables[0].Select("Name='AutoStation'");
            if (drAutoStation.Length > 0) IsAuto = drAutoStation[0]["Value"].ToString(); else IsAuto = "0";


            DataRow[] DR_IsCheckPO = DS_StationTypeDef.Tables[0].Select("Description='IsCheckPO'");
            if (DR_IsCheckPO.Length > 0)
            {
                S_IsCheckPO = DR_IsCheckPO[0]["Content"].ToString();

                if (S_IsCheckPO == "1")
                {
                    DataRow[] DR_Child_IsCheckPO = dsStationDetail.Tables[0].Select("Name='IsCheckPO'");
                    if (DR_Child_IsCheckPO.Length > 0) S_IsCheckPO = DR_Child_IsCheckPO[0]["Value"].ToString(); else S_IsCheckPO = "1";
                }
            }
            else
            {
                S_IsCheckPO = "1";
            }

            DataRow[] DR_IsCheckPN = DS_StationTypeDef.Tables[0].Select("Description='IsCheckPN'");
            if (DR_IsCheckPN.Length > 0)
            {
                S_IsCheckPN = DR_IsCheckPN[0]["Content"].ToString();

                if (S_IsCheckPN == "1")
                {
                    DataRow[] DR_Child_IsCheckPN = dsStationDetail.Tables[0].Select("Name='IsCheckPN'");
                    if (DR_Child_IsCheckPN.Length > 0) S_IsCheckPN = DR_Child_IsCheckPN[0]["Value"].ToString(); else S_IsCheckPN = "1";
                }
            }
            else
            {
                S_IsCheckPN = "1";
            }


            if (S_IsCheckPO == "1")
            {
                lblProductionOrder.Visible = true;
                lblUnitState.Visible = true;
                lblUnitState.Location = new Point(58, 135);

                panel12.Visible = true;
            }
            else
            {
                lblProductionOrder.Visible = false;
                lblUnitState.Visible = true;

                lblUnitState.Location = new Point(58, 102);
                panel12.Visible = false;
            }
            DataRow[] DR_IsShowNgDialog = DS_StationTypeDef.Tables[0].Select("Description='IsShowNgDialog'");
            if (DR_IsShowNgDialog.Length > 0)
            {
                isShowNgDialog = DR_IsShowNgDialog[0]["Content"].ToString().Trim() == "1";
            }

            return dsStationDetail;
        }

        private string SetScaleSetting()
        {
            string result = string.Empty;

            #region 20220301 定义一个参数
            string configRes = "0";
            DataSet BoxWeightConfig = base.PartSelectSVC.GetMesPartAndPartFamilyDetail(int.Parse(Com_Part.EditValue.ToString()), "FGWeightLimit", "FGWeightLimit", out configRes);

            if (configRes != "1")
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, configRes, "NG", List_Login.Language, new string[] { "" }, configRes, "");
                Btn_Refresh_Click(this, null);
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
                    Btn_Refresh_Click(this, null);
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
                Btn_Refresh_Click(this, null);
                return result;
            }

            te_base_UL.Text = weightBaseOffsetU.ToString();
            te_base_LL.Text = weightBaseOffsetL.ToString();
            te_base.Text = weightBase.ToString();
            te_unit.Text = weightUnit;

            string reSerialPort = initSerialPort();

            if (reSerialPort != "1")
            {
                string strMsg = MessageInfo.GetMsgByCode(reSerialPort, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { strMsg }, reSerialPort);
                Btn_Refresh_Click(this, null);
                return result;
            }
            DataTable tmpDt = base.Grid_RouteData.DataSource as DataTable;
            if (tmpDt?.Rows.Count <= 0 || tmpDt?.Select($"StationTypeID = {List_Login.StationTypeID}").Length <= 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "70019", "NG", List_Login.Language);
                Btn_Refresh_Click(this, null);
                return result;
            }

            result = "1";
            return result;
        }


        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            Edt_SN.Enabled = false;
            base.Btn_Defect.Visible = false;

            GetStationData();

            if (S_IsCheckPN == "1")
            {
                GrpControlPart.Visible = true;
            }
            else
            {
                GrpControlPart.Visible = false;

                //dsBOM = PartSelectSVC.MESGetBomPartInfo(Convert.ToInt32(Com_Part.EditValue.ToString()),
                //                   List_Login.StationTypeID);
            }
            if (S_IsCheckPO == "0" && S_IsCheckPN == "0")
            {
                Edt_SN.Text = "";
                Edt_SN.Enabled = true;
                Edt_SN.Focus();
            }

            SwitchSerialPort(false);
        }

        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            DataSet dsStationDetail = GetStationData();

            base.Btn_ConfirmPO_Click(sender, e);
            string tmpRes = SetScaleSetting();
            if (tmpRes != "1")
                return;


            Edt_SN.Enabled = true;
            Edt_SN.Focus();

            if (isEnableCOF && S_IsCheckPO == "1")
            {
                string PO = Com_PO.EditValue.ToString();
                if (!string.IsNullOrEmpty(PO))
                {
                    DataSet dsCOF = PartSelectSVC.GetluPODetailDef(Convert.ToInt32(PO), "COF");
                    if (dsCOF != null && dsCOF.Tables.Count > 0 && dsCOF.Tables[0].Rows.Count > 0)
                    {
                        COF = dsCOF.Tables[0].Rows[0]["Content"].ToString() == "1";
                    }
                }
            }

            SetQC(isShowNgDialog);
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
            return SwitchSerialPort() ? "1" : "70014";
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

                tb_weight.ForeColor = (currentWeight > mUpperLimit || currentWeight < mLowerLimit) ?  Color.Red : Color.Green;

                if (Edt_SN.Enabled || string.IsNullOrEmpty(Edt_SN.Text.Trim()) || gSnData is null)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"SN : {Edt_SN.Text.ToString()}, Please place the product correctly on the electronic scale again");
                    return;
                }

                if ( currentWeight < mLowerLimit || currentWeight > mUpperLimit)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"SN : {Edt_SN.Text.ToString()}, Scale [{currentWeight}] no match,   SPEC : {mLowerLimit} ~ {mUpperLimit}");
                    string ngRes = CommitData(currentWeight.ToString(),false);
                    gSnData = null;
                    SwitchSerialPort(false);
                    Edt_SN.Enabled = true;
                    Edt_SN.Text = string.Empty;
                    Edt_SN.Focus();
                    return;
                }
                
                string result = CommitData(currentWeight.ToString());
                gSnData = null;
                SwitchSerialPort(false);
                if (result != "OK")
                {
                    Console.WriteLine("更新失败");
                    string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Edt_SN.Text.ToString(), ProMsg }, result);
                    Edt_SN.Enabled = true;
                    Edt_SN.Focus();
                    Edt_SN.SelectAll();
                    return;
                }
                MessageInfo.Add_Info_MSG(Edt_MSG, "70018", "OK", List_Login.Language, new string[] { Edt_SN.Text });
                base.SetOverStiaonQTY(true);
                Edt_SN.Enabled = true;
                Edt_SN.Text = string.Empty;
                Edt_SN.Focus();
            }));
        }

        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                gSnData = null;
                string S_SN = Edt_SN.Text.Trim();

                if (S_SN == "")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }

                string mPartId = null;
                string mPoId = null;
                string mPartFamilyId = Com_PartFamily.EditValue.ToString();
                string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();
                string S_DefectID = base.DefectChar;
                if (S_luUnitStateID == "2" || S_luUnitStateID == "3")
                {
                    if (S_DefectID == "")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20049", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }
                }

                if (S_IsCheckPN == "1")
                {
                    mPartId = Com_Part.EditValue.ToString();
                    if (S_IsCheckPO == "1")
                    {
                        mPoId = Com_PO.EditValue.ToString();
                    }
                }
                string outString1 = string.Empty;
                if (string.IsNullOrEmpty(mPartId) || string.IsNullOrEmpty(mPoId))
                {
                    DataSet dsMainSN;
                    dsMainSN = PartSelectSVC.uspCallProcedure("uspGetBaseData", S_SN,
                           null, null, null, null, null, ref outString1);
                    if (outString1 != "1" || dsMainSN == null || dsMainSN.Tables[0].Rows.Count <= 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20119", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }

                    DataTable dt = dsMainSN.Tables[0];
                    mPoId = string.IsNullOrEmpty(mPoId) ? dt.Rows[0]["ProductionOrderID"].ToString() : mPoId;
                    mPartId = string.IsNullOrEmpty(mPartId) ? dt.Rows[0]["PartID"].ToString() : mPartId;
                    mPartFamilyId = dt.Rows[0]["PartFamilyID"].ToString();
                }
                if (isEnableCOF)
                {
                    DataSet dsCOF = PartSelectSVC.GetluPODetailDef(Convert.ToInt32(mPoId), "COF");
                    if (dsCOF != null && dsCOF.Tables.Count > 0 && dsCOF.Tables[0].Rows.Count > 0)
                    {
                        COF = dsCOF.Tables[0].Rows[0]["Content"].ToString() == "1";
                    }
                }
                DataSet unitDataset = PartSelectSVC.GetAndCheckUnitInfo(S_SN, mPoId, mPartId);
                if (unitDataset == null || unitDataset.Tables.Count <= 0 || unitDataset.Tables[0].Rows.Count <= 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20015", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }
                DataTable DT_Unit = unitDataset.Tables[0];


                if (IsAuto == "1")
                {
                    try
                    {
                        port1.Write(ReadCmd);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "60108", "OK", List_Login.Language);
                    }
                    catch (Exception ex)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "60109", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                    }

                }

                //////////////////////////
                string S_UnitStateID = string.Empty;
                DataSet tmpDataSet = PartSelectSVC.GetmesUnitState(mPartId, mPartFamilyId, "", List_Login.LineID.ToString(), List_Login.StationTypeID, mPoId, Com_luUnitStatus.EditValue.ToString());
                if (tmpDataSet != null && tmpDataSet.Tables.Count != 0 && tmpDataSet.Tables[0].Rows.Count != 0)
                {
                    S_UnitStateID = tmpDataSet.Tables[0].Rows[0]["ID"].ToString().Trim();
                }
                if (string.IsNullOrEmpty(S_UnitStateID))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }
                /////////////////////////

                //////////////////////////////////////////////////////
                /// //二次检查是否有指定线路  20231214
                var tmpSecondRet = PartSelectSVC.GetmesUnitStateSecond(mPartId, mPartFamilyId, "", List_Login.LineID.ToString(),
                    List_Login.StationTypeID, mPoId, S_luUnitStateID, S_SN);

                if (tmpSecondRet.StartsWith("Error", StringComparison.OrdinalIgnoreCase))
                {
                    //MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + mainSN });
                    MessageInfo.Add_Info_MSG(Edt_MSG, "NG", tmpSecondRet);
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }

                S_UnitStateID = tmpSecondRet;
                if (string.IsNullOrEmpty(S_UnitStateID))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }
                /////////////////////////////////////////////////////

                string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, S_SN);
                if (S_RouteCheck == "1")
                {
                    try
                    {
                        //调用通用过程
                        string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + mPoId + "\"> </ProdOrder>";
                        string xmlPart = "<Part PartID=\"" + mPartId + "\"> </Part>";
                        string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                        string outString = string.Empty;
                        PartSelectSVC.uspCallProcedure("uspQCCheck", S_SN,
                                                                                xmlProdOrder, xmlPart, xmlStation, null, S_luUnitStateID, ref outString);
                        if (outString != "1")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, outString);
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                            return;
                        }


                        //DataTable DT_UPUnitID = PartSelectSVC.Get_UnitID(S_SN).Tables[0];

                        string tmpNGUnitStateID = string.Empty;
                        DataSet tmpDataSetNG = PartSelectSVC.GetmesUnitState(mPartId, mPartFamilyId, "", List_Login.LineID.ToString(), List_Login.StationTypeID, mPoId, "2");
                        if (tmpDataSetNG != null && tmpDataSetNG.Tables.Count != 0 && tmpDataSetNG.Tables[0].Rows.Count != 0)
                        {
                            tmpNGUnitStateID = tmpDataSetNG.Tables[0].Rows[0]["ID"].ToString().Trim();
                        }

                        if (string.IsNullOrEmpty(tmpNGUnitStateID))
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                            return;
                        }

                        gSnData = new SNDataInfo()
                        {
                            UnitID = Convert.ToInt32(DT_Unit.Rows[0]["ID"].ToString()),
                            UnitStateID = Convert.ToInt32(S_UnitStateID),
                            POID = Convert.ToInt32(mPoId),
                            PartID = Convert.ToInt32(mPartId),
                            NGUnitStateID = Convert.ToInt32(tmpNGUnitStateID)
                            
                        };
                        if (!SwitchSerialPort())
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "70014", "NG", List_Login.Language, new string[] { }, "70014", "");
                            return;
                        }

                        tb_weight.Text = "----";
                        tb_weight.ForeColor = Color.Black;
                        Edt_SN.Enabled = false;
                        MessageInfo.Add_Info_MSG(Edt_MSG, "70020", "NG", List_Login.Language, new string[] { Edt_SN.Text, "", "Error_RE" });
                        tb_weight.Focus();

                    }
                    catch (Exception ex)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                    }
                }
                else
                {
                    if (S_RouteCheck == "20243")
                    {
                        string nowUnitStateID = PartSelectSVC.GetSerialNumber2(S_SN).Tables[0].Rows[0]["UnitStateID"].ToString();
                        mesUnitState mesunitSateModel = mesUnitStateSVC.Get(Convert.ToInt32(nowUnitStateID));
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20243", "NG", List_Login.Language, new string[] { S_SN, mesunitSateModel.Description });
                    }
                    else
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(S_RouteCheck, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_RouteCheck);
                        //MessageInfo.Add_Info_MSG(Edt_MSG, "20243", "NG", List_Login.Language, new string[] { S_SN, mesunitSateModel.Description + " [PartName]:" + dsMainSN.Tables[0].Rows[0]["PartNumber"].ToString() + " [LineName]:" + dsMainSN.Tables[0].Rows[0]["LineName"].ToString() });
                    }
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                }                
            }
        }


        private string CommitData(string weight, bool isPass = true)
        {
            if (gSnData is null)
                return "";


            List<mesUnit> List_mesUnit = new List<mesUnit>();
            List<mesHistory> List_mesHistory = new List<mesHistory>();
            List<mesUnitDetail> List_mesUnitDetail = new List<mesUnitDetail>();

            mesUnit v_mesUnit = new mesUnit();
            v_mesUnit.ID = gSnData.UnitID;
            v_mesUnit.UnitStateID = isPass? gSnData.UnitStateID : gSnData.NGUnitStateID;
            v_mesUnit.StatusID = isPass? 1 : 2;
            v_mesUnit.StationID = List_Login.StationID;
            v_mesUnit.EmployeeID = List_Login.EmployeeID;
            v_mesUnit.LastUpdate = DateTime.Now;
            v_mesUnit.ProductionOrderID = gSnData.POID;
            //修改 Unit
            List_mesUnit.Add(v_mesUnit);

            mesHistory v_mesHistory = new mesHistory();
            v_mesHistory.UnitID = Convert.ToInt32(v_mesUnit.ID);
            v_mesHistory.UnitStateID = isPass ? gSnData.UnitStateID : gSnData.NGUnitStateID;
            v_mesHistory.EmployeeID = List_Login.EmployeeID;
            v_mesHistory.StationID = List_Login.StationID;
            v_mesHistory.ProductionOrderID = gSnData.POID;
            v_mesHistory.PartID = gSnData.PartID;
            v_mesHistory.LooperCount = 1;
            v_mesHistory.StatusID = isPass ? 1 : 2;
            //插入 mesHistory
            List_mesHistory.Add(v_mesHistory);

            mesUnitDetail v_mesUnitDetail = new mesUnitDetail();
            v_mesUnitDetail.UnitID = gSnData.UnitID;
            v_mesUnitDetail.reserved_08 = weight;
            List_mesUnitDetail.Add(v_mesUnitDetail);


            mesUnit[] L_mesUnit = List_mesUnit.ToArray();
            mesHistory[] L_mesHistory = List_mesHistory.ToArray();

            return DataCommitSVC.SubmitDataUH_UDetail(L_mesUnit, L_mesHistory, List_mesUnitDetail.ToArray());
        }



        private void OverStationV2_Form_Load(object sender, EventArgs e)
        {
            base.FrmBase_Load(sender, e);

            if (S_IsCheckPN == "0")
            {
                GrpControlInputData.Enabled = true;

                Edt_SN.Enabled = true; 
            }
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            gSnData = null;
            SwitchSerialPort(false);
            Edt_SN.Enabled = true;
            Edt_SN.Focus();
            Edt_SN.SelectAll();
        }
    }
    class SNDataInfo
    {
        public int UnitID { get; set; }
        public int UnitStateID { get; set; }
        public int POID { get; set; }
        public int PartID { get; set; }
        public int NGUnitStateID { get; set; }


    }
}