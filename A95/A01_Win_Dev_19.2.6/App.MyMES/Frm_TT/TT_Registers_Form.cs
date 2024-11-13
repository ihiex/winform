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
    public partial class TT_Registers_Form : FrmBase
    {
        public TT_Registers_Form()
        {
            InitializeComponent();
        }

        private string strPort = "COM1";
        string IsAuto = "0";
        string PassCmd = "PASS";
        string FailCmd = "FAIL";
        string ReadCmd = "READ";

        string IsFlashScreen = "0";
        int I_TimerInterval = 500;

        private void txtSN_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DateTime dateStart = DateTime.Now;

                    string S_SN = txtSN.Text.Trim();
                    if (string.IsNullOrEmpty(S_SN))
                    {
                        //20007 条码不能为空.
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language,null, null, this.Panel_Data, IsFlashScreen);
                        txtSN.Focus();
                        txtSN.Text = "";
                        SendFail();
                        return;
                    }

                    if (!Regex.IsMatch(S_SN, SN_Pattern))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20027", "NG", List_Login.Language, new string[] { S_SN}, null, this.Panel_Data, IsFlashScreen);
                        txtSN.Focus();
                        txtSN.Text = "";
                        SendFail();
                        return;
                    }

                    string xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                    string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>";
                    string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                    string outString = string.Empty;

                    //通用方法校验
                    PartSelectSVC.uspCallProcedure("uspTTCheck", S_SN, xmlProdOrder, xmlPart, xmlStation, "", "", ref outString);
                    if (outString != "1")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, outString, 
                            this.Panel_Data, IsFlashScreen);
                        txtSN.Focus();
                        txtSN.Text = "";
                        SendFail();
                        return;
                    }

                    DataSet dsUnit = PartSelectSVC.GetSerialNumber2(S_SN);
                    string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                    string S_PartID = Com_Part.EditValue.ToString();
                    string S_ProductionOrderID = Com_PO.EditValue.ToString();
                    string S_LoginLineID = List_Login.LineID.ToString();
                    string PartFamilyID = Com_PartFamily.EditValue.ToString();
                    if (dsUnit == null || dsUnit.Tables.Count == 0 || dsUnit.Tables[0].Rows.Count == 0)
                    {
                        PartSelectSVC.uspCallProcedure("uspPONumberCheck", Com_PO.EditValue.ToString(),
                            null, null, null, null, "1", ref outString);
                        if (outString != "1")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[]
                               { Com_PO.EditValue.ToString(), ProMsg }, outString,  this.Panel_Data, IsFlashScreen);
                            txtSN.Focus();
                            txtSN.Text = "";
                            SendFail();
                            return;
                        }

                        string S_StationTypeID = List_Login.StationTypeID.ToString();
                        string Status = string.IsNullOrEmpty(Com_luUnitStatus.EditValue.ToString()) ? "1" : Com_luUnitStatus.EditValue.ToString();
                        string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID, 
                            List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), Status);
                        if (string.IsNullOrEmpty(S_UnitStateID))
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, null,null, this.Panel_Data, IsFlashScreen);
                            txtSN.Focus();
                            txtSN.Text = "";
                            SendFail();
                            return;
                        }
                        mesUnit v_mesUnit_New = new mesUnit();
                        v_mesUnit_New.UnitStateID = Convert.ToInt32(S_UnitStateID);
                        v_mesUnit_New.StatusID = 1;
                        v_mesUnit_New.StationID = List_Login.StationID;
                        v_mesUnit_New.EmployeeID = List_Login.EmployeeID;
                        v_mesUnit_New.CreationTime = DateTime.Now;
                        v_mesUnit_New.LastUpdate = DateTime.Now;
                        v_mesUnit_New.PanelID = 0;
                        v_mesUnit_New.LineID = List_Login.LineID; 

                        v_mesUnit_New.ProductionOrderID = Convert.ToInt32(S_ProductionOrderID);
                        v_mesUnit_New.RMAID = 0;
                        v_mesUnit_New.PartID = Convert.ToInt32(S_PartID);
                        v_mesUnit_New.LooperCount = 1;
                        v_mesUnit_New.PartFamilyID = Convert.ToInt32(PartFamilyID);
                        v_mesUnit_New.SerialNumberType = 10;  //TT SerialNumber

                        string S_UnitID = mesUnitSVC.Insert(v_mesUnit_New);
                        if (string.IsNullOrEmpty(S_UnitID) || S_UnitID.Substring(0, 1) == "E")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(S_UnitID, List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20130", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, S_UnitID,
                                 this.Panel_Data, IsFlashScreen);
                            txtSN.Focus();
                            txtSN.Text = "";
                            SendFail();
                            return;
                        }

                        //写入mesSerialNumber
                        mesSerialNumber v_mesSerialNumber = new mesSerialNumber();
                        v_mesSerialNumber.UnitID = Convert.ToInt32(S_UnitID);
                        v_mesSerialNumber.SerialNumberTypeID = 0;
                        v_mesSerialNumber.Value = S_SN;
                        mesSerialNumberSVC.Insert(v_mesSerialNumber);

                        //写入UnitDetail表
                        mesUnitDetail msDetail = new mesUnitDetail();
                        msDetail.UnitID = Convert.ToInt32(S_UnitID);
                        msDetail.reserved_01 = "";
                        msDetail.reserved_02 = "";
                        msDetail.reserved_03 = "";
                        msDetail.reserved_04 = "";
                        msDetail.reserved_05 = "";
                        mesUnitDetailSVC.Insert(msDetail);

                        mesHistory v_mesHistory = new mesHistory();
                        v_mesHistory.UnitID = Convert.ToInt32(S_UnitID);
                        v_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                        v_mesHistory.EmployeeID = List_Login.EmployeeID;
                        v_mesHistory.StationID = List_Login.StationID;
                        v_mesHistory.EnterTime = DateTime.Now;
                        v_mesHistory.ExitTime = DateTime.Now;
                        v_mesHistory.ProductionOrderID = Convert.ToInt32(S_ProductionOrderID);
                        v_mesHistory.PartID = Convert.ToInt32(S_PartID);
                        v_mesHistory.LooperCount = 1;
                        mesHistorySVC.Insert(v_mesHistory);


                        TimeSpan ts = DateTime.Now - dateStart;
                        double mill = Math.Round(ts.TotalMilliseconds, 0);

                        //10010  SN:{0}操作完成,用时{1}毫秒.
                        MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { S_SN, mill.ToString() }, 
                            null, this.Panel_Data, IsFlashScreen);
                        txtSN.Focus();
                        txtSN.Text = "";
                        SendPass();

                        if (Status == "1")
                        {
                            base.SetOverStiaonQTY(true);
                        }
                        else
                        {
                            base.SetOverStiaonQTY(false);
                        }
                        base.GetProductionCount();
                    }
                    else
                    {
                        string ScanType = "1";         //扫码类型 1:条码SN 2:BOX 3:MachineBox (未配置默认1)
                        string SN = string.Empty;
                        string S_UnitID = string.Empty;
                        DataTable DT_Unit = null;

                        DataSet dataTTScanType = PartSelectSVC.GetPLCSeting("TTScanType", List_Login.StationID.ToString());
                        if (dataTTScanType != null && dataTTScanType.Tables.Count > 0 && dataTTScanType.Tables[0].Rows.Count > 0)
                        {
                            ScanType = dataTTScanType.Tables[0].Rows[0]["Value"].ToString();
                        }

                        if (ScanType == "3")
                        {
                            DataSet ds = mesUnitDetailSVC.MesGetBatchIDByBarcodeSN(S_SN);
                            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20030", "NG", List_Login.Language, new string[] { S_SN }, null, this.Panel_Data, IsFlashScreen);
                                txtSN.Focus();
                                txtSN.Text = "";
                                SendFail();
                                return;
                            }
                            S_UnitID = ds.Tables[0].Rows[0]["UnitID"].ToString();
                            SN = ds.Tables[0].Rows[0]["reserved_02"].ToString();
                            DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0];
                            string Result = string.Empty;
                            xmlPart = "<Part PartID=\"" + DT_Unit.Rows[0]["PartID"].ToString() + "\"> </Part>";
                            PartSelectSVC.uspCallProcedure("uspMachineToolingCheck", S_SN,
                                                             null, xmlPart, null, "", List_Login.StationTypeID.ToString(), ref Result);
                            if (Result != "1")
                            {
                                string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, Result, 
                                    this.Panel_Data, IsFlashScreen);
                                txtSN.Text = "";
                                SendFail();
                                return;
                            }
                        }
                        else
                        {
                            dsUnit = PartSelectSVC.GetSerialNumber2(S_SN);
                            if (dsUnit == null || dsUnit.Tables.Count == 0 || dsUnit.Tables[0].Rows.Count == 0)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language, new string[] { S_SN },
                                    null, this.Panel_Data, IsFlashScreen);
                                txtSN.Focus();
                                txtSN.Text = "";
                                SendFail();
                                return;
                            }
                            S_UnitID = dsUnit.Tables[0].Rows[0]["UnitID"].ToString();
                            DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0];
                            SN = S_SN;
                        }

                        //校验料号/工单
                        DataSet dsUnitSN = PartSelectSVC.GetSerialNumber2(SN);
                        if(dsUnitSN == null || dsUnitSN.Tables.Count==0 || dsUnitSN.Tables[0].Rows.Count==0)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language, new string[] { SN }, 
                                null, this.Panel_Data, IsFlashScreen);
                            txtSN.Focus();
                            txtSN.Text = "";
                            SendFail();
                            return;
                        }
                        string UnitPartID = dsUnitSN.Tables[0].Rows[0]["PartID"].ToString();
                        string UnitProductionOrderID = dsUnitSN.Tables[0].Rows[0]["ProductionOrderID"].ToString();
                        if (S_ProductionOrderID != UnitProductionOrderID)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20050", "NG", List_Login.Language, new string[] { S_SN }, 
                                null, this.Panel_Data, IsFlashScreen);
                            txtSN.Focus();
                            txtSN.Text = "";
                            SendFail();
                            return;
                        }

                        //校验工序是否正确
                        string UnitStateID = string.Empty;

                        string result = PartSelectSVC.GetRouteCheck_Diagram(List_Login.StationTypeID, List_Login.LineID.ToString(), DT_Unit, "", out UnitStateID);
                        if (result != "1")
                        {
                            string nowUnitStateID = PartSelectSVC.GetSerialNumber2(S_SN).Tables[0].Rows[0]["UnitStateID"].ToString();
                            mesUnitState mesunitSateModel = mesUnitStateSVC.Get(Convert.ToInt32(nowUnitStateID));
                            if (result == "20243")
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20243", "NG", List_Login.Language, new string[] 
                                    { S_SN, mesunitSateModel.Description }, null, this.Panel_Data, IsFlashScreen);
                                SendFail();
                            }
                            else
                            {
                                string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, result, 
                                     this.Panel_Data, IsFlashScreen);
                                SendFail();
                            }
                            txtSN.Focus();
                            txtSN.Text = "";
                            return;
                        }

                        UnitStateID = public_.GetluUnitStatusID(UnitPartID, PartFamilyID, List_Login.StationTypeID,
                            List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), "1");

                        if (string.IsNullOrEmpty(UnitStateID))
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, null,null, this.Panel_Data, IsFlashScreen);
                            txtSN.Focus();
                            txtSN.Text = "";
                            SendFail();
                            return;
                        }

                        //过站逻辑
                        string xmlExtraData = "<ExtraData UnitStateID =\"" + UnitStateID + "\" " +
                                                 "LineID =\"" + List_Login.LineID.ToString() + "\" " +
                                                 "EmployeeID =\"" + List_Login.EmployeeID.ToString() + "\"> </ExtraData>";
                        PartSelectSVC.uspCallProcedure("uspTTSetData", SN, null, null, xmlStation, xmlExtraData, ScanType, ref outString);
                        if (outString != "1")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, outString, 
                                 this.Panel_Data, IsFlashScreen);
                            txtSN.Focus();
                            txtSN.Text = "";
                            SendFail();
                            return;
                        }

                        TimeSpan ts = DateTime.Now - dateStart;
                        double mill = Math.Round(ts.TotalMilliseconds, 0);


                        if (ScanType == "3")
                        {
                            mesMachineSVC.MesModMachineBySNStationTypeID(S_SN, List_Login.StationTypeID);
                            
                        }

                        base.SetOverStiaonQTY(true);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { S_SN, mill.ToString() },
                            null, this.Panel_Data, IsFlashScreen);
                        txtSN.Focus();
                        txtSN.Text = "";
                        SendPass();
                    }
                }
            }
            catch(Exception ex)
            {
                // 20009 错误信息:{0}.
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() }, 
                    null, this.Panel_Data, IsFlashScreen);
                SendFail();
            }
        }

        private void SendPass()
        {
            if (IsAuto == "1")
            {
                try
                {
                    port1.WriteLine(PassCmd);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "60101", "OK", List_Login.Language,null, null, this.Panel_Data, IsFlashScreen);
                }
                catch (Exception ex)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "60105", "NG", List_Login.Language, new string[] { ex.Message.ToString() }, 
                        null, this.Panel_Data, IsFlashScreen);
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
                    MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "NG", List_Login.Language, null,null, this.Panel_Data, IsFlashScreen);
                }
                catch (Exception ex)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() },
                        null, this.Panel_Data, IsFlashScreen);
                }
            }
        }

        private void TT_Registers_Form_Load(object sender, EventArgs e)
        {
            base.FrmBase_Load(sender, e);

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
            if (drTimerInterval.Length > 0) I_TimerInterval = Convert.ToInt32(drTimerInterval[0]["Value"].ToString()); else I_TimerInterval = 500;

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
                    MessageInfo.Add_Info_MSG(Edt_MSG, "60104", "NG", List_Login.Language, new string[] { ex.Message.ToString() },
                        null, this.Panel_Data, IsFlashScreen);
                    return;
                }
            }
        }

        private void TT_Registers_Form_FormClosed(object sender, FormClosedEventArgs e)
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
