using App.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.MyMES
{
    public partial class TT_OverStation_V2_Form : FrmBase2
    {
        string ScanType = "1";         //扫码类型 1:条码SN 2:BOX 3:MachineBox (未配置默认1) 4:混合（前3种类型都可以扫描）当输入为1类型时，且条码被绑定在2或3中，这时对SN做解除动作。

        private string strPort = "COM1";
        string IsAuto = "0";
        string PassCmd = "PASS";
        string FailCmd = "FAIL";
        string ReadCmd = "READ";

        string IsFlashScreen = "0";
        int I_TimerInterval = 500;
        bool isV2 = true;

        string S_IsTTBoxUnpack = "0";

        private string CardIDPattern = string.Empty;
        public TT_OverStation_V2_Form()
        {
            InitializeComponent();
        }

        public override void FrmBase_Load(object sender, EventArgs e)
        {
            base.FrmBase_Load(sender, e);
            base.btnHide_Click(sender, e);
            ///////////////////////////
            /// 20230627 add
            lblCardID.Visible = false;
            tbCardID.Visible = false;
            btnLock.Visible = false;
            string output = string.Empty;
            var res = PartSelectSVC.GetMesStationAndStationTypeDetail(List_Login.StationID, "IsCheckCardID", "IsCheckCardID",
                out output);

            if (res == "1" && output == "1")
            {
                lblCardID.Visible = true;
                tbCardID.Visible = true;
                btnLock.Visible = true;

                var CardIDPatten = PartSelectSVC.GetMesStationAndStationTypeDetail(List_Login.StationID, "CardIDPattern", "CardIDPattern",
                    out output);

                CardIDPattern = output == "1" ? CardIDPatten : @"[\\s\\S]*";
            }

            /////////////////////////////////////////////////////////////////////////////////////////

            DataSet DS_StationType = PartSelectSVC.MESGetStationTypeParameter(List_Login.StationTypeID);
            DataSet dsStationDetail = PartSelectSVC.GetPLCSeting("", List_Login.StationID.ToString());

            DataRow[] drAutoStation = dsStationDetail.Tables[0].Select("Name='AutoStation'");
            if (drAutoStation.Length > 0) IsAuto = drAutoStation[0]["Value"].ToString(); else IsAuto = "0";

            DataRow[] drPASS = dsStationDetail.Tables[0].Select("Name='PASS'");
            if (drPASS.Length > 0) PassCmd = drPASS[0]["Value"].ToString(); else PassCmd = "PASS";

            DataRow[] drFAIL = dsStationDetail.Tables[0].Select("Name='FAIL'");
            if (drFAIL.Length > 0) FailCmd = drFAIL[0]["Value"].ToString(); else FailCmd = "FAIL";

            DataRow[] drREAD = dsStationDetail.Tables[0].Select("Name='READ'");
            if (drREAD.Length > 0) ReadCmd = drREAD[0]["Value"].ToString(); else ReadCmd = "READ";

            DataRow[] drPort = dsStationDetail.Tables[0].Select("Name='Port'");
            if (drPort.Length > 0) strPort = drPort[0]["Value"].ToString(); else strPort = "COM1";

            DataRow[] drIsFlashScreen = dsStationDetail.Tables[0].Select("Name='IsFlashScreen'");
            if (drIsFlashScreen.Length > 0) IsFlashScreen = drIsFlashScreen[0]["Value"].ToString(); else IsFlashScreen = "0";

            DataRow[] drTimerInterval = dsStationDetail.Tables[0].Select("Name='TimerInterval'");
            if (drTimerInterval.Length > 0) I_TimerInterval =Convert.ToInt32(drTimerInterval[0]["Value"].ToString()); else I_TimerInterval = 500;

            base.FrmBase_Load(sender, e);
            this.GrpControlInputData.Enabled = true;
            if (tbCardID.Visible)
            {
                tbCardID.Focus();
            }
            else
            {
                txtSN.Focus();
            }
            
            //DataSet dataTTScanType = PartSelectSVC.GetPLCSeting("TTScanType", List_Login.StationID.ToString());
            //if (dataTTScanType != null && dataTTScanType.Tables.Count > 0 && dataTTScanType.Tables[0].Rows.Count > 0)
            //{
            //    ScanType = dataTTScanType.Tables[0].Rows[0]["Value"].ToString();
            //}

            DataRow[] DR_TTScanType = DS_StationType.Tables[0].Select("Description='TTScanType'");
            if (DR_TTScanType.Length > 0)
            {
                ScanType = DR_TTScanType[0]["Content"].ToString();

                DataSet DS_Seting = PartSelectSVC.GetPLCSeting("TTScanType", List_Login.StationID.ToString());
                if (DS_Seting != null && DS_Seting.Tables.Count != 0 && DS_Seting.Tables[0].Rows.Count != 0)
                {
                    ScanType = DS_Seting.Tables[0].Rows[0]["Value"].ToString();
                }
            }


            DataRow[] DR_IsTTBoxUnpack = DS_StationType.Tables[0].Select("Description='IsTTBoxUnpack'");
            if (DR_IsTTBoxUnpack.Length > 0)
            {
                S_IsTTBoxUnpack = DR_IsTTBoxUnpack[0]["Content"].ToString();

                DataSet DS_Seting = PartSelectSVC.GetPLCSeting("IsTTBoxUnpack", List_Login.StationID.ToString());
                if (DS_Seting != null && DS_Seting.Tables.Count != 0 && DS_Seting.Tables[0].Rows.Count != 0)
                {
                    S_IsTTBoxUnpack = DS_Seting.Tables[0].Rows[0]["Value"].ToString();
                }
            }


            if (IsFlashScreen == "1")
            {
                timer1.Interval = I_TimerInterval;
                timer1.Enabled = true;
            }

            if (IsAuto == "1")
            {
                port1 = new SerialPort(strPort);
                port1.BaudRate = 9600;
                port1.DataBits = 8;
                port1.Parity = Parity.Odd;
                port1.StopBits = StopBits.One;

                try
                {
                    if (!port1.IsOpen)
                    {
                        port1.Open();
                    }
                }
                catch (Exception ex)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "60104", "NG", List_Login.Language, new string[] { ex.Message.ToString() },null,this.Panel_Data,IsFlashScreen);
                    return;
                }
            }

        }

        private void SendPass()
        {
            if (IsAuto == "1")
            {
                try
                {
                    port1.WriteLine(PassCmd);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "60101", "OK", List_Login.Language,null, null, this.Panel_Data,IsFlashScreen);
                }
                catch (Exception ex)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "60105", "NG", List_Login.Language, new string[] { ex.Message.ToString() }, null, this.Panel_Data,IsFlashScreen);
                }
            }
        }

        private void SendFail()
        {
            if (IsAuto == "1")
            {
                try
                {

                    port1.WriteLine(FailCmd);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "NG", List_Login.Language,null,null, this.Panel_Data,IsFlashScreen);
                }
                catch (Exception ex)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() }, null, this.Panel_Data,IsFlashScreen);
                }
            }
        }

        private void txtSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }
            if (tbCardID.Visible )
            {
                if (string.IsNullOrEmpty(tbCardID.Text) || tbCardID.Enabled)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "NG", "Please input your Card ID, and lock card ID");
                    tbCardID.Focus();
                    return;
                }
            }
            if (isV2)
            {
                if (e.KeyCode != Keys.Enter || string.IsNullOrEmpty(txtSN.Text.Trim()))
                {
                    return;
                }
                CheckAndOverStation(txtSN.Text.Trim().ToUpper());
            }
            else
            {
                #region old code
                string S_SN = txtSN.Text.Trim();
                try
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        DateTime dateStart = DateTime.Now;


                        string SN = string.Empty;
                        string S_UnitID = string.Empty;
                        DataTable DT_Unit = null;
                        if (string.IsNullOrEmpty(S_SN))
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data, IsFlashScreen);
                            txtSN.Focus();
                            txtSN.Text = "";
                            SendFail();
                            return;
                        }

                        if (ScanType == "3")
                        {
                            DataSet ds = mesUnitDetailSVC.MesGetBatchIDByBarcodeSN(S_SN);
                            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20030", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data, IsFlashScreen);
                                txtSN.Focus();
                                txtSN.Text = "";
                                SendFail();
                                return;
                            }
                            S_UnitID = ds.Tables[0].Rows[0]["UnitID"].ToString();
                            SN = ds.Tables[0].Rows[0]["reserved_02"].ToString();
                            DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0];
                            string xmlParts = "<Part PartID=\"" + DT_Unit.Rows[0]["PartID"].ToString() + "\"> </Part>";
                            string Result = string.Empty;
                            PartSelectSVC.uspCallProcedure("uspMachineToolingCheck", S_SN,
                                                             null, xmlParts, null, "", List_Login.StationTypeID.ToString(), ref Result);
                            if (Result != "1")
                            {
                                string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, Result, this.Panel_Data, IsFlashScreen);
                                txtSN.Text = "";
                                SendFail();
                                return;
                            }
                        }
                        else
                        {
                            DataSet dsUnit = PartSelectSVC.GetSerialNumber2(S_SN);
                            if (dsUnit == null || dsUnit.Tables.Count == 0 || dsUnit.Tables[0].Rows.Count == 0)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data, IsFlashScreen);
                                txtSN.Focus();
                                txtSN.Text = "";
                                SendFail();
                                return;
                            }
                            S_UnitID = dsUnit.Tables[0].Rows[0]["UnitID"].ToString();
                            DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0];
                            SN = S_SN;
                        }

                        //校验工序是否正确
                        string result = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID,
                            List_Login.LineID.ToString(), DT_Unit, S_SN);
                        if (result != "1")
                        {
                            if (result == "20243")
                            {
                                string outString1 = string.Empty;
                                DataSet dsMainSN;
                                dsMainSN = PartSelectSVC.uspCallProcedure("uspGetBaseData", S_SN,
                                       null, null, null, null, null, ref outString1);
                                if (outString1 != "1" || dsMainSN == null || dsMainSN.Tables[0].Rows.Count <= 0)
                                {
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20119", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data, IsFlashScreen);
                                    SendFail();
                                }

                                string nowUnitStateID = PartSelectSVC.GetSerialNumber2(S_SN).Tables[0].Rows[0]["UnitStateID"].ToString();
                                mesUnitState mesunitSateModel = mesUnitStateSVC.Get(Convert.ToInt32(nowUnitStateID));
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20243", "NG", List_Login.Language, new string[]
                                { S_SN, mesunitSateModel.Description+" [PartName]:"+
                                dsMainSN.Tables[0].Rows[0]["PartNumber"].ToString() + " [LineName]:" +
                                dsMainSN.Tables[0].Rows[0]["LineName"].ToString() }, null, this.Panel_Data, IsFlashScreen);
                                SendFail();
                            }
                            else
                            {
                                string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, result, this.Panel_Data, IsFlashScreen);
                                SendFail();
                            }
                            txtSN.Focus();
                            txtSN.Text = "";
                            return;
                        }

                        string S_POID = DT_Unit.Rows[0]["ProductionOrderID"].ToString();
                        string S_PartID = DT_Unit.Rows[0]["PartID"].ToString();
                        string UnitStateID = public_.GetluUnitStatusID(S_PartID, "", List_Login.StationTypeID,
                            List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), "1");

                        if (string.IsNullOrEmpty(UnitStateID))
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data, IsFlashScreen);
                            txtSN.Focus();
                            txtSN.Text = "";
                            SendFail();
                            return;
                        }

                        string xmlPart = "<Part PartID=\"" + DT_Unit.Rows[0]["PartID"].ToString() + "\"> </Part>";
                        string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + DT_Unit.Rows[0]["ProductionOrderID"].ToString() + "\"> </ProdOrder>";
                        string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                        string xmlExtraData = "<ExtraData UnitStateID =\"" + UnitStateID + "\" " +
                                                 "LineID =\"" + List_Login.LineID.ToString() + "\" " +
                                                 "EmployeeID =\"" + List_Login.EmployeeID.ToString() + "\"> </ExtraData>";
                        string outString = string.Empty;
                        string outCheck = string.Empty;

                        //通用方法校验
                        PartSelectSVC.uspCallProcedure("uspTTCheck", SN, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, ScanType, ref outCheck);
                        if (outCheck != "1")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(outCheck, List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, outCheck, this.Panel_Data, IsFlashScreen);
                            txtSN.Focus();
                            txtSN.Text = "";
                            SendFail();
                            return;
                        }

                        //过站逻辑
                        PartSelectSVC.uspCallProcedure("uspTTSetData", SN, null, null, xmlStation, xmlExtraData, ScanType, ref outString);
                        if (outString != "1")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, outString, this.Panel_Data, IsFlashScreen);
                            txtSN.Focus();
                            txtSN.Text = "";
                            SendFail();
                            return;
                        }

                        ////更新UnitStates状态
                        //mesUnit v_mesUnit = new mesUnit();
                        //v_mesUnit.ID = Convert.ToInt32(S_UnitID);
                        //v_mesUnit.UnitStateID =Convert.ToInt32(UnitStateID);
                        //v_mesUnit.LineID = List_Login.LineID;
                        //v_mesUnit.EmployeeID = List_Login.EmployeeID;
                        //v_mesUnit.StationID = List_Login.StationID;
                        //string S_UpdateUnit = mesUnitSVC.UpdateUnitStateID(v_mesUnit);

                        //if (S_UpdateUnit != "OK")
                        //{
                        //    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_UpdateUnit });
                        //    txtSN.Focus();
                        //    txtSN.Text = "";
                        //    return;
                        //}

                        //mesHistory v_mesHistory = new mesHistory();

                        //v_mesHistory.UnitID = Convert.ToInt32(S_UnitID);
                        //v_mesHistory.UnitStateID = Convert.ToInt32(UnitStateID);
                        //v_mesHistory.EmployeeID = List_Login.EmployeeID;
                        //v_mesHistory.StationID = List_Login.StationID;
                        //v_mesHistory.EnterTime = DateTime.Now;
                        //v_mesHistory.ExitTime = DateTime.Now;
                        //v_mesHistory.ProductionOrderID = Convert.ToInt32(DT_Unit.Rows[0]["ProductionOrderID"].ToString());
                        //v_mesHistory.PartID = Convert.ToInt32(DT_Unit.Rows[0]["PartID"].ToString());
                        //v_mesHistory.LooperCount = 1;
                        //mesHistorySVC.Insert(v_mesHistory);

                        TimeSpan ts = DateTime.Now - dateStart;
                        double mill = Math.Round(ts.TotalMilliseconds, 0);


                        if (ScanType == "3")
                            mesMachineSVC.MesModMachineBySNStationTypeID(S_SN, List_Login.StationTypeID);

                        base.SetOverStiaonQTY(true);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { S_SN, mill.ToString() }, null, this.Panel_Data, IsFlashScreen);
                        txtSN.Focus();
                        txtSN.Text = "";
                        SendPass();
                    }
                }
                catch (Exception ex)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() + "  " + S_SN }, null, this.Panel_Data, IsFlashScreen);
                    SendFail();
                }
                #endregion
            }

        }

        private void saveLog(string name,string message)
        {
            if (false)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(name);
                log.Debug(message);
            }
        }
        private void CheckAndOverStation(string S_SN)
        {
            try
            {
                //string _ScanType = CheckBarcodeType(S_SN);
                ////目前类型只有1,2,3。 4为混合    其他则为异常
                //if (!new string[3] { "1", "2", "3" }.Contains(_ScanType))
                //{
                //    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data, IsFlashScreen);
                //    txtSN.Focus();
                //    txtSN.Text = "";
                //    SendFail();
                //    return;
                //}


                DateTime dateStart = DateTime.Now;
                DateTime logDateStart = DateTime.Now;
                string S_TTScanType_SN = checkMixScan(S_SN);
                if (ScanType != "4")
                {
                    if (ScanType != S_TTScanType_SN)
                    {
                        //扫描类型不匹配
                        MessageInfo.Add_Info_MSG(Edt_MSG, "60202", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data, IsFlashScreen);
                        txtSN.Focus();
                        txtSN.Text = "";
                        SendFail();
                        return;
                    }
                }
                saveLog("Test",$"checkMixScan {(DateTime.Now- logDateStart).Milliseconds}");

                logDateStart = DateTime.Now;
                //这个增加判断 成品SN  有没有已经装箱。需要增加一个方法，需要判断 SerialNumberTypeID 0，1，3，5  PanelId！=null
                //  2022-11-02  扫描类型！=4 的
                if (ScanType != "4")
                {
                    string S_Sql =
                        @"SELECT COUNT(1) AS Valint1 FROM mesSerialNumber A JOIN mesUnit B ON A.UnitID=B.ID
                            WHERE B.PanelID>0
		                          AND A.SerialNumberTypeID IN(0,1,3,5) AND A.[Value]='" + S_SN + "'";
                    var List_InBox = PartSelectSVC.P_DataSet(S_Sql);
                   
                    if (List_InBox.Tables[0].Rows.Count > 0)
                    {
                        int I_Valint1 = Convert.ToInt32(List_InBox.Tables[0].Rows[0]["Valint1"].ToString());
                        if (I_Valint1 > 0)
                        {
                            //产品已经装箱
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60203", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data, IsFlashScreen);
                            txtSN.Focus();
                            txtSN.Text = "";
                            SendFail();
                            return;
                        }
                    }
                }
                saveLog("Test", $"P_DataSet {(DateTime.Now- logDateStart).Milliseconds}");
                logDateStart = DateTime.Now;
                string _ScanType = CheckBarcodeType(S_SN);
                //目前类型只有1,2,3。 4为混合    其他则为异常
                if (!new string[3] { "1", "2", "3" }.Contains(_ScanType))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data, IsFlashScreen);
                    txtSN.Focus();
                    txtSN.Text = "";
                    SendFail();
                    return;
                }

                string SN = string.Empty;
                string S_UnitID = string.Empty;
                DataTable DT_Unit = null;
                if (string.IsNullOrEmpty(S_SN))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data, IsFlashScreen);
                    txtSN.Focus();
                    txtSN.Text = "";
                    SendFail();
                    return;
                }
                saveLog("Test", $"CheckBarcodeType {(DateTime.Now- logDateStart).Milliseconds}");
                logDateStart = DateTime.Now;
                if (_ScanType == "2")
                {

                    DataSet DS_TTBindBoxUnit = PartSelectSVC.GetmesUnitTTBox(S_SN);
                    DataTable DT_TTBindBoxUnit = DS_TTBindBoxUnit.Tables[0];
                    saveLog("Test", $"type 2 GetmesUnitTTBox {(DateTime.Now- logDateStart).Milliseconds}");
                    logDateStart = DateTime.Now;
                    if (DT_TTBindBoxUnit.Rows.Count == 0)
                    {
                        //这个箱子没有绑定的数据
                        MessageInfo.Add_Info_MSG(Edt_MSG, "60204", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data, IsFlashScreen);
                        txtSN.Focus();
                        txtSN.Text = "";
                        SendFail();
                        return;
                    }
                    else
                    {
                        if (DT_TTBindBoxUnit.Rows[0]["ChildCount"].ToString() == "0")
                        {
                            //这个箱子没有绑定的数据
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60204", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data, IsFlashScreen);
                            txtSN.Focus();
                            txtSN.Text = "";
                            SendFail();
                            return;
                        }
                    }

                }

                if (_ScanType == "3")
                {
                    DataSet ds = mesUnitDetailSVC.MesGetBatchIDByBarcodeSN(S_SN);
                    saveLog("Test", $"MesGetBatchIDByBarcodeSN {(DateTime.Now- logDateStart).Milliseconds}");
                    logDateStart = DateTime.Now;
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20030", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data, IsFlashScreen);
                        txtSN.Focus();
                        txtSN.Text = "";
                        SendFail();
                        return;
                    }
                    else
                    {
                        DataSet DS_TTBindBoxUnit = PartSelectSVC.GetmesUnitTTBox(ds.Tables[0].Rows[0]["reserved_02"].ToString());
                        saveLog("Test", $"type 3 GetmesUnitTTBox {(DateTime.Now- logDateStart).Milliseconds}");
                        logDateStart = DateTime.Now;
                        DataTable DT_TTBindBoxUnit = DS_TTBindBoxUnit.Tables[0];
                        if (DT_TTBindBoxUnit.Rows.Count == 0)
                        {
                            //这个箱子没有绑定的数据
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60204", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data, IsFlashScreen);
                            txtSN.Focus();
                            txtSN.Text = "";
                            SendFail();
                            return;
                        }
                        else
                        {
                            if (DT_TTBindBoxUnit.Rows[0]["ChildCount"].ToString() == "0")
                            {
                                //这个箱子没有绑定的数据
                                MessageInfo.Add_Info_MSG(Edt_MSG, "60204", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data, IsFlashScreen);
                                txtSN.Focus();
                                txtSN.Text = "";
                                SendFail();
                                return;
                            }
                        }
                    }


                    S_UnitID = ds.Tables[0].Rows[0]["UnitID"].ToString();
                    SN = ds.Tables[0].Rows[0]["reserved_02"].ToString();
                    DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0];
                    saveLog("Test", $"GetmesUnit {(DateTime.Now- logDateStart).Milliseconds}");
                    logDateStart = DateTime.Now;
                    if (DT_Unit.Rows[0]["StatusID"].ToString() != "1")
                    {
                        //此条码已NG.
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20036", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data, IsFlashScreen);
                        txtSN.Focus();
                        txtSN.Text = "";
                        SendFail();
                        return;
                    }

                    string xmlParts = "<Part PartID=\"" + DT_Unit.Rows[0]["PartID"].ToString() + "\"> </Part>";
                    string Result = string.Empty;
                    PartSelectSVC.uspCallProcedure("uspMachineToolingCheck", S_SN,
                                                     null, xmlParts, null, "", List_Login.StationTypeID.ToString(), ref Result);
                    saveLog("Test", $"uspMachineToolingCheck {(DateTime.Now- logDateStart).Milliseconds}");
                    logDateStart = DateTime.Now;
                    if (Result != "1")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, Result, this.Panel_Data, IsFlashScreen);
                        txtSN.Text = "";
                        SendFail();
                        return;
                    }
                }
                else
                {
                    DataSet dsUnit = PartSelectSVC.GetSerialNumber2(S_SN);
                    saveLog("Test", $"GetSerialNumber2 {(DateTime.Now- logDateStart).Milliseconds}");
                    logDateStart = DateTime.Now;

                    if (dsUnit == null || dsUnit.Tables.Count == 0 || dsUnit.Tables[0].Rows.Count == 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data, IsFlashScreen);
                        txtSN.Focus();
                        txtSN.Text = "";
                        SendFail();
                        return;
                    }
                    S_UnitID = dsUnit.Tables[0].Rows[0]["UnitID"].ToString();
                    DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0];
                    saveLog("Test", $"GetmesUnit {(DateTime.Now- logDateStart).Milliseconds}");
                    logDateStart = DateTime.Now;
                    SN = S_SN;
                }

                //校验工序是否正确
                string result = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID,
                    List_Login.LineID.ToString(), DT_Unit, S_SN);
                saveLog("Test", $"GetRouteCheck {(DateTime.Now- logDateStart).Milliseconds}");
                logDateStart = DateTime.Now;

                if (result != "1")
                {
                    if (result == "20243")
                    {
                        string outString1 = string.Empty;
                        DataSet dsMainSN;
                        dsMainSN = PartSelectSVC.uspCallProcedure("uspGetBaseData", S_SN,
                               null, null, null, null, null, ref outString1);
                        if (outString1 != "1" || dsMainSN == null || dsMainSN.Tables[0].Rows.Count <= 0)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20119", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data, IsFlashScreen);
                            SendFail();
                        }

                        string nowUnitStateID = PartSelectSVC.GetSerialNumber2(S_SN).Tables[0].Rows[0]["UnitStateID"].ToString();
                        mesUnitState mesunitSateModel = mesUnitStateSVC.Get(Convert.ToInt32(nowUnitStateID));
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20243", "NG", List_Login.Language, new string[]
                        { S_SN, mesunitSateModel.Description+" [PartName]:"+
                                dsMainSN.Tables[0].Rows[0]["PartNumber"].ToString() + " [LineName]:" +
                                dsMainSN.Tables[0].Rows[0]["LineName"].ToString() }, null, this.Panel_Data, IsFlashScreen);
                        SendFail();
                    }
                    else
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, result, this.Panel_Data, IsFlashScreen);
                        SendFail();
                    }
                    txtSN.Focus();
                    txtSN.Text = "";
                    return;
                }

                string v_Sql = "select reserved_01 ValStr1,reserved_02 ValStr2,reserved_04 ValStr3 from mesUnitDetail where (reserved_01='" +
                    S_SN + "' or reserved_02='" + S_SN + "' )and reserved_04='1'";
                DataSet DS_Query = PartSelectSVC.P_DataSet(v_Sql);
                saveLog("Test", $"2 P_DataSet {(DateTime.Now- logDateStart).Milliseconds}");
                logDateStart = DateTime.Now;

                if (DS_Query.Tables[0].Rows.Count > 0)
                {
                    //箱子没有关闭
                    MessageInfo.Add_Info_MSG(Edt_MSG, "60205", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data, IsFlashScreen);
                    txtSN.Focus();
                    txtSN.Text = "";
                    SendFail();
                    return;
                }



                string S_POID = DT_Unit.Rows[0]["ProductionOrderID"].ToString();
                string S_PartID = DT_Unit.Rows[0]["PartID"].ToString();
                string UnitStateID = string.Empty;
                //string UnitStateID = public_.GetluUnitStatusID(S_PartID, "", List_Login.StationTypeID,
                //    List_Login.LineID.ToString(), S_POID, "1");

                DataSet tmpDataSet = PartSelectSVC.GetmesUnitState(S_PartID, "","",  List_Login.LineID.ToString(), List_Login.StationTypeID, S_POID, "1");
                if (tmpDataSet != null && tmpDataSet.Tables.Count != 0 && tmpDataSet.Tables[0].Rows.Count != 0)
                {
                    UnitStateID = tmpDataSet.Tables[0].Rows[0]["ID"].ToString().Trim();
                }

                saveLog("Test", $"GetluUnitStatusID {(DateTime.Now- logDateStart).Milliseconds}");
                logDateStart = DateTime.Now;
                //20230919 老板提议TT取消工单检查
                //if (S_POID != "0" && S_POID != "")
                //{
                //    var v_LineOrder = public_.GetmesLineOrder(List_Login.LineID.ToString(), S_POID);
                //    if (v_LineOrder.Tables[0].Rows.Count == 0)
                //    {
                //        MessageInfo.Add_Info_MSG(Edt_MSG, "20050", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data, IsFlashScreen);
                //        txtSN.Focus();
                //        txtSN.Text = "";
                //        SendFail();
                //        return;
                //    }
                //}

                if (string.IsNullOrEmpty(UnitStateID))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data, IsFlashScreen);
                    txtSN.Focus();
                    txtSN.Text = "";
                    SendFail();
                    return;
                }

                string xmlPart = "<Part PartID=\"" + DT_Unit.Rows[0]["PartID"].ToString() + "\"> </Part>";
                string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + DT_Unit.Rows[0]["ProductionOrderID"].ToString() + "\"> </ProdOrder>";
                string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                string xmlExtraData = "<ExtraData UnitStateID =\"" + UnitStateID + "\" " +
                                         "LineID =\"" + List_Login.LineID.ToString() + "\" " +
                                         "EmployeeID =\"" + List_Login.EmployeeID.ToString() + "\" " +
                                         "IsTTBoxUnpack =\"" + S_IsTTBoxUnpack + "\" " +
                                         "SNType =\"" + _ScanType + "\" " +
                                         "CardID =\"" + tbCardID.Text.Trim() + "\" " +
                                         "> </ExtraData>";
                string outString = string.Empty;
                string outCheck = string.Empty;

                //通用方法校验
                PartSelectSVC.uspCallProcedure("uspTTCheck", SN, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, _ScanType, ref outCheck);
                saveLog("Test", $"uspTTCheck {(DateTime.Now- logDateStart).Milliseconds}");
                logDateStart = DateTime.Now;
                if (outCheck != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(outCheck, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, outCheck, this.Panel_Data, IsFlashScreen);
                    txtSN.Focus();
                    txtSN.Text = "";
                    SendFail();
                    return;
                }

                //过站逻辑
                PartSelectSVC.uspCallProcedure("uspTTSetData", SN, null, null, xmlStation, xmlExtraData, _ScanType, ref outString);
                saveLog("Test", $"uspTTSetData {(DateTime.Now- logDateStart).Milliseconds}");
                logDateStart = DateTime.Now;
                if (outString != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, outString, this.Panel_Data, IsFlashScreen);
                    txtSN.Focus();
                    txtSN.Text = "";
                    SendFail();
                    return;
                }

                #region -----
                ////更新UnitStates状态
                //mesUnit v_mesUnit = new mesUnit();
                //v_mesUnit.ID = Convert.ToInt32(S_UnitID);
                //v_mesUnit.UnitStateID =Convert.ToInt32(UnitStateID);
                //v_mesUnit.LineID = List_Login.LineID;
                //v_mesUnit.EmployeeID = List_Login.EmployeeID;
                //v_mesUnit.StationID = List_Login.StationID;
                //string S_UpdateUnit = mesUnitSVC.UpdateUnitStateID(v_mesUnit);

                //if (S_UpdateUnit != "OK")
                //{
                //    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_UpdateUnit });
                //    txtSN.Focus();
                //    txtSN.Text = "";
                //    return;
                //}

                //mesHistory v_mesHistory = new mesHistory();

                //v_mesHistory.UnitID = Convert.ToInt32(S_UnitID);
                //v_mesHistory.UnitStateID = Convert.ToInt32(UnitStateID);
                //v_mesHistory.EmployeeID = List_Login.EmployeeID;
                //v_mesHistory.StationID = List_Login.StationID;
                //v_mesHistory.EnterTime = DateTime.Now;
                //v_mesHistory.ExitTime = DateTime.Now;
                //v_mesHistory.ProductionOrderID = Convert.ToInt32(DT_Unit.Rows[0]["ProductionOrderID"].ToString());
                //v_mesHistory.PartID = Convert.ToInt32(DT_Unit.Rows[0]["PartID"].ToString());
                //v_mesHistory.LooperCount = 1;
                //mesHistorySVC.Insert(v_mesHistory);
                #endregion

                TimeSpan ts = DateTime.Now - dateStart;
                double mill = Math.Round(ts.TotalMilliseconds, 0);


                if (_ScanType == "3")
                    mesMachineSVC.MesModMachineBySNStationTypeID(S_SN, List_Login.StationTypeID);

                saveLog("Test", $"MesModMachineBySNStationTypeID {(DateTime.Now- logDateStart).Milliseconds}");

                base.SetOverStiaonQTY(true);
                MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { S_SN, mill.ToString() }, null, this.Panel_Data, IsFlashScreen);
                txtSN.Focus();
                txtSN.Text = "";
                SendPass();
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() + "  " + S_SN }, null, this.Panel_Data, IsFlashScreen);
                SendFail();

            }
            
        }
        private string CheckBarcodeType(string inputSN)
        {
            string tmpType = "0";
            try
            {
                switch (ScanType)
                {
                    case "1":
                    case "2":
                    case "3":
                        return ScanType;
                    case "4":
                        tmpType = checkMixScan(inputSN);
                        break;
                    default:
                        return tmpType;
                }
                string logMessage = string.Empty;
                switch (tmpType)
                {
                    case "1":
                        Console.WriteLine("有绑定，且扫描的是单品条码");
                        break;
                    case "2":
                        Console.WriteLine("");
                        break;
                    case "3":
                        
                        break;
                    default:
                        break;
                }

            }
            catch (Exception)
            {

                throw;
            }
            
            return tmpType;
        }

        private string checkMixScan(string inputSN)
        {
            string tmpScanType = "0";
            DataTable dtMachine = mesUnitDetailSVC.MesGetBatchIDByBarcodeSN(inputSN)?.Tables[0];
            if (dtMachine != null && dtMachine.Rows.Count > 0)
            {
                //过滤设备机器条码
                var v = mesSerialNumberSVC.Get(int.Parse(dtMachine.Rows[0]["UnitID"].ToString().Trim()));                
                tmpScanType = v.SerialNumberTypeID.ToString() == "9" ? "3" : "-2";
            }
            else
            {   
                //查询条码是否有绑定箱号，理论上不论是箱号或者单个产品条码都应该是可以找到记录的。
                DataTable childTable = PartSelectSVC.GetSerialNumber2(inputSN)?.Tables[0];
                if (childTable?.Rows.Count <= 0)
                {
                    Console.WriteLine("类型为0时， 条码不存在");
                    return tmpScanType;
                }

                string panelId = childTable.Rows[0]["PanelID"].ToString().Trim();
                //panelId为空并且条码的类型为8 则扫描类型为2， 条码类型不为8，则是扫描的条码是单品条码，且不存在绑定关系 -1
                //panelId不为空，则扫描的条码为单品条码且与箱号存在绑定关系。
                if (panelId == "0") { panelId = string.Empty; }
                //tmpScanType = string.IsNullOrEmpty(panelId) ? (childTable.Rows[0]["SerialNumberTypeID"].ToString().Trim() == "8" ? "2" : "-1") : "1";

                if (string.IsNullOrEmpty(panelId))
                {
                    if (childTable.Rows[0]["SerialNumberTypeID"].ToString().Trim() == "8" ||
                        childTable.Rows[0]["SerialNumberTypeID"].ToString().Trim() == "10")
                    {
                        tmpScanType = "2";
                    }
                    else if (new string[4] { "0", "1", "3", "5" }.Contains(childTable.Rows[0]["SerialNumberTypeID"].ToString().Trim()))
                    {
                        tmpScanType = "1";
                    }
                    else
                    {
                        tmpScanType = "-1";
                    }
                }
                else
                {
                    tmpScanType = "1";
                }
            }
            return tmpScanType;
        }

        private void TT_OverStation_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (port1 != null)
                {
                    if (port1.IsOpen) port1.Close();
                    port1.Dispose();
                }
            }
            catch
            { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Panel_Data.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            Edt_MSG.BackColor = Color.White;
        }

        private void tbCardID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Enter)
                return;

            if (string.IsNullOrEmpty(tbCardID.Text))
                return;

            if (!Regex.IsMatch(tbCardID.Text, CardIDPattern))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "NG", "Card ID  regular check failed, please confirm.");
                tbCardID.SelectAll();
                return;
            }

            tbCardID.Enabled = false;
            txtSN.Focus();
        }

        private void btnLock_Click(object sender, EventArgs e)
        {
            if (!tbCardID.Visible)
                return;

            if (tbCardID.Enabled)
            {
                if (string.IsNullOrEmpty(tbCardID.Text))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG,"NG","please input card ID.");
                    return;
                }

                if (!Regex.IsMatch(tbCardID.Text, CardIDPattern))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "NG", "Card ID  regular check failed, please confirm.");
                    tbCardID.SelectAll();
                    return;
                }

                btnLock.Text = "Unlock";
                tbCardID.Enabled = false;
                txtSN.Focus();
            }
            else
            {
                tbCardID.Text = string.Empty;
                tbCardID.Enabled = true;
                btnLock.Text = "Lock";
                tbCardID.Focus();
            }

        }
    }
}
