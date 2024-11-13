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
    public partial class TT_OverStation_Form : FrmBase2
    {
        string ScanType = "1";         //扫码类型 1:条码SN 2:BOX 3:MachineBox (未配置默认1)

        private string strPort = "COM1";
        string IsAuto = "0";
        string PassCmd = "PASS";
        string FailCmd = "FAIL";
        string ReadCmd = "READ";

        string IsFlashScreen = "0";
        int I_TimerInterval = 500;

        public TT_OverStation_Form()
        {
            InitializeComponent();
        }

        public override void FrmBase_Load(object sender, EventArgs e)
        {
            base.FrmBase_Load(sender, e);
            base.btnHide_Click(sender, e);

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
            txtSN.Focus();
            DataSet dataTTScanType = PartSelectSVC.GetPLCSeting("TTScanType", List_Login.StationID.ToString());
            if (dataTTScanType != null && dataTTScanType.Tables.Count > 0 && dataTTScanType.Tables[0].Rows.Count > 0)
            {
                ScanType = dataTTScanType.Tables[0].Rows[0]["Value"].ToString();
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
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data,IsFlashScreen);
                        txtSN.Focus();
                        txtSN.Text = "";
                        SendFail();
                        return;
                    }

                    if (ScanType == "3")
                    {
                        DataSet ds = mesUnitDetailSVC.MesGetBatchIDByBarcodeSN(S_SN);
                        if(ds==null || ds.Tables.Count==0 || ds.Tables[0].Rows.Count==0)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20030", "NG", List_Login.Language,  new string[] { "SN:" + S_SN }, null, this.Panel_Data,IsFlashScreen);
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
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, Result, this.Panel_Data,IsFlashScreen);
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
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data,IsFlashScreen);
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
                        if (result== "20243")
                        {
                            string outString1 = string.Empty;
                            DataSet dsMainSN;
                            dsMainSN = PartSelectSVC.uspCallProcedure("uspGetBaseData", S_SN,
                                   null, null, null, null, null, ref outString1);
                            if (outString1 != "1" || dsMainSN == null || dsMainSN.Tables[0].Rows.Count <= 0)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20119", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data,IsFlashScreen);
                                SendFail();
                            }

                            string nowUnitStateID = PartSelectSVC.GetSerialNumber2(S_SN).Tables[0].Rows[0]["UnitStateID"].ToString();
                            mesUnitState mesunitSateModel = mesUnitStateSVC.Get(Convert.ToInt32(nowUnitStateID));
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20243", "NG", List_Login.Language, new string[] 
                            { S_SN, mesunitSateModel.Description+" [PartName]:"+ 
                                dsMainSN.Tables[0].Rows[0]["PartNumber"].ToString() + " [LineName]:" +
                                dsMainSN.Tables[0].Rows[0]["LineName"].ToString() }, null, this.Panel_Data,IsFlashScreen);
                            SendFail();
                        }
                        else
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, result, this.Panel_Data,IsFlashScreen);
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

                    if(string.IsNullOrEmpty(UnitStateID))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + S_SN }, null, this.Panel_Data,IsFlashScreen);
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
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, outCheck,  this.Panel_Data,IsFlashScreen);
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
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, outString,  this.Panel_Data,IsFlashScreen);
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
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { S_SN, mill.ToString() }, null, this.Panel_Data,IsFlashScreen);
                    txtSN.Focus();
                    txtSN.Text = "";
                    SendPass();
                }
            }
            catch(Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString()+"  "+S_SN }, null, this.Panel_Data,IsFlashScreen);
                SendFail();
            }
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
    }
}
