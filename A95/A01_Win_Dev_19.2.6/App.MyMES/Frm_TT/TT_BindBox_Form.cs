using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.MyMES.mesUnitComponentService;
using App.Model;
using System.IO.Ports;

namespace App.MyMES
{
    public partial class TT_BindBox_Form : FrmBase
    {
        string SNFormat = string.Empty;           //条码生成格式
        string LabelPath = string.Empty;          //打印文件路径
        string CaseQTY = string.Empty;            //整箱数量
        int CurrQTY = 0;                          //当前装箱数量
        string PanelUnitID = string.Empty;        //父ID
        bool IsTTRegistSN = false;                //是否注册SN
        string BoxType = "1";            //1:只注册 2:注册且系统生成SN 3:系统Machine表存在  
        string UnitStatusID = string.Empty;
        //string S_PanelSN = "";
        string S_MachineSN = "";
        string S_TTScanType = "";   // 本站扫描类型不使用
        string S_BoxSN = "";


        string xmlStation = string.Empty;
        string xmlPart = string.Empty;
        string xmlProdOrder = string.Empty;
        
        DataTable DT_PrintSn;

        private string strPort = "COM1";
        string IsAuto = "0";
        string PassCmd = "PASS";
        string FailCmd = "FAIL";
        string ReadCmd = "READ";

        string IsFlashScreen = "0";
        int I_TimerInterval = 500;

        string S_IsCheckPO = "1";
        string S_IsCheckPN = "1";

        public TT_BindBox_Form()
        {
            InitializeComponent();
        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            Edt_SN.Text = string.Empty;
            txtChildSN.Text = string.Empty;
            txtCaseQTY.Text = string.Empty;
            txtShipmentQTY.Text = string.Empty;
        }

        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            DataSet DS_StationType = PartSelectSVC.MESGetStationTypeParameter(List_Login.StationTypeID);
            DataSet dsStationDetail = PartSelectSVC.GetPLCSeting("", List_Login.StationID.ToString());
            Memo_ChildSN.Text = "";


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

            //获取料号满箱数量
            int I_PartID = Convert.ToInt32(Com_Part.EditValue.ToString());
            DataSet dataSet = PartSelectSVC.GetmesPartDetail(I_PartID, "FullNumber");
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                CaseQTY = dataSet.Tables[0].Rows[0]["Content"].ToString();
                txtCaseQTY.Text = CaseQTY;
            }
            else
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20198", "NG", List_Login.Language, new string[] { "FullNumber" }, null, this.Panel_Data, IsFlashScreen);
                return;
            }

            DataRow[] DR_TTRegistSN = DS_StationType.Tables[0].Select("Description='TTRegistSN'");
            if (DR_TTRegistSN.Length > 0)
            {
                IsTTRegistSN= DR_TTRegistSN[0]["Content"].ToString()=="1";

                DataSet DS_Seting = PartSelectSVC.GetPLCSeting("TTRegistSN", List_Login.StationID.ToString());
                if (DS_Seting != null && DS_Seting.Tables.Count != 0 && DS_Seting.Tables[0].Rows.Count != 0)
                {
                    IsTTRegistSN = DS_Seting.Tables[0].Rows[0]["Value"].ToString() == "1";
                }
            }

            DataRow[] DR_TTScanType = DS_StationType.Tables[0].Select("Description='TTScanType'");
            if (DR_TTScanType.Length > 0)
            {
                S_TTScanType = DR_TTScanType[0]["Content"].ToString();

                DataSet DS_Seting = PartSelectSVC.GetPLCSeting("TTScanType", List_Login.StationID.ToString());
                if (DS_Seting != null && DS_Seting.Tables.Count != 0 && DS_Seting.Tables[0].Rows.Count != 0)
                {
                    S_TTScanType = DS_Seting.Tables[0].Rows[0]["Value"].ToString();
                }
            }

            DataRow[] DR_IsCheckPO = DS_StationType.Tables[0].Select("Description='IsCheckPO'");
            if (DR_IsCheckPO.Length > 0)
            {
                S_IsCheckPO = DR_IsCheckPO[0]["Content"].ToString();

                DataSet DS_Seting = PartSelectSVC.GetPLCSeting("IsCheckPO", List_Login.StationID.ToString());
                if (DS_Seting != null && DS_Seting.Tables.Count != 0 && DS_Seting.Tables[0].Rows.Count != 0)
                {
                    S_IsCheckPO = DS_Seting.Tables[0].Rows[0]["Value"].ToString();
                }
            }

            DataRow[] DR_IsCheckPN = DS_StationType.Tables[0].Select("Description='IsCheckPN'");
            if (DR_IsCheckPN.Length > 0)
            {
                S_IsCheckPN = DR_IsCheckPN[0]["Content"].ToString();

                DataSet DS_Seting = PartSelectSVC.GetPLCSeting("IsCheckPN", List_Login.StationID.ToString());
                if (DS_Seting != null && DS_Seting.Tables.Count != 0 && DS_Seting.Tables[0].Rows.Count != 0)
                {
                    S_IsCheckPN = DS_Seting.Tables[0].Rows[0]["Value"].ToString();
                }
            }


            //是否自动生成Panel条码
            DataRow[] DR_TTBoxType = DS_StationType.Tables[0].Select("Description='TTBoxType'");
            if (DR_TTBoxType.Length > 0)
            {
                BoxType = DR_TTBoxType[0]["Content"].ToString();

                DataSet dataSetTTLable = PartSelectSVC.GetPLCSeting("TTBoxType", List_Login.StationID.ToString());
                if (dataSetTTLable != null && dataSetTTLable.Tables.Count > 0 && dataSetTTLable.Tables[0].Rows.Count > 0)
                {
                    BoxType = dataSetTTLable.Tables[0].Rows[0]["Value"].ToString();
                }

                if (BoxType == "2" || BoxType == "3")
                {
                    // 获取条码生成格式
                    string S_StationTypeID = List_Login.StationTypeID.ToString();
                    string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                    string S_PartID = Com_Part.EditValue.ToString();
                    string S_ProductionOrderID = Com_PO.EditValue.ToString();
                    string S_LoginLineID = List_Login.LineID.ToString();

                    SNFormat = PartSelectSVC.mesGetSNFormatIDByList(S_PartID, S_PartFamilyID, S_LoginLineID,
                                Com_PO.EditValue.ToString(), List_Login.StationTypeID.ToString());
                    if (!string.IsNullOrEmpty(SNFormat))
                    {
                        LabelPath = public_.GetLabelName(PartSelectSVC, S_StationTypeID, S_PartFamilyID,
                                                   S_PartID, S_ProductionOrderID, S_LoginLineID);

                        if (string.IsNullOrEmpty(LabelPath))
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20098", "NG", List_Login.Language, null, null, this.Panel_Data, IsFlashScreen);
                            return;
                        }
                    }
                    else
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20132", "NG", List_Login.Language, null, null, this.Panel_Data, IsFlashScreen);
                        return;
                    }
                }
            }



            base.Btn_ConfirmPO_Click(sender, e);
            xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";
            xmlProdOrder = "<ProdOrder ProdOrderID=\"" + Com_PO.EditValue.ToString() + "\"> </ProdOrder>";
            xmlPart = "<Part PartID=\"" + Com_Part.EditValue.ToString() + "\"> </Part>";
            if (BoxType == "2")
            {
                Edt_SN.Enabled = false;
                txtChildSN.Enabled = true;
                txtChildSN.Focus();
            }
            else
            {
                Edt_SN.Enabled = true;
                txtChildSN.Enabled = false;
                Edt_SN.Focus();
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
                    MessageInfo.Add_Info_MSG(Edt_MSG, "60104", "NG", List_Login.Language, new string[] { ex.Message.ToString() },
                        null, this.Panel_Data, IsFlashScreen);
                    return;
                }
            }

        }

        public override void FrmBase_Load(object sender, EventArgs e)
        {
            base.FrmBase_Load(sender, e);
            mesUnitComponentSVC = ImesUnitComponentFactory.CreateServerClient();
            public_.OpenBartender(List_Login.StationID.ToString());
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
                    MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "NG", List_Login.Language,null, null, this.Panel_Data, IsFlashScreen);
                }
                catch (Exception ex)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() },
                        null, this.Panel_Data, IsFlashScreen);
                }
            }
        }



        public bool LoadMainSNData(string PanelSN)
        {
            try
            {               
                DataSet DS_TTBindBoxUnit = PartSelectSVC.GetmesUnitTTBox(PanelSN);
                DataTable DT_TTBindBoxUnit = DS_TTBindBoxUnit.Tables[0];
                string S_TTBindBoxUnitID = DT_TTBindBoxUnit.Rows[0]["ID"].ToString();
                string S_FullNumber = DT_TTBindBoxUnit.Rows[0]["FullNumber"].ToString() == "" ? "-1" : DT_TTBindBoxUnit.Rows[0]["FullNumber"].ToString();
                string S_ChildCount = DT_TTBindBoxUnit.Rows[0]["ChildCount"].ToString() == "" ? "0" : DT_TTBindBoxUnit.Rows[0]["ChildCount"].ToString();
                string S_BoxSNStatus = DT_TTBindBoxUnit.Rows[0]["BoxSNStatus"].ToString() == "" ? "1" : DT_TTBindBoxUnit.Rows[0]["BoxSNStatus"].ToString();

                int I_TTBindBoxUnitID=Convert.ToInt32(S_TTBindBoxUnitID);
                int I_FullNumber =Convert.ToInt32(S_FullNumber);
                int I_ChildCount= Convert.ToInt32(S_ChildCount);
                int I_BoxSNStatus = Convert.ToInt32(S_BoxSNStatus);

                if (I_BoxSNStatus == 2)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20202", "NG", List_Login.Language, null, null, this.Panel_Data, IsFlashScreen);
                    SendFail();
                    return false;
                }
                else
                {
                    //判断是否重复
                    DataSet dsSN = PartSelectSVC.GetmesSerialNumber(PanelSN);
                    if (dsSN != null && dsSN.Tables.Count > 0 && dsSN.Tables[0].Rows.Count > 0)
                    {
                        if (I_ChildCount >= I_FullNumber)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20202", "NG", List_Login.Language, null, null, this.Panel_Data, IsFlashScreen);
                            SendFail();
                            return false;
                        }
                    }
                }

                string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                string PartFamilyID = Com_PartFamily.EditValue.ToString();
                string ProductionID = Com_PO.EditValue.ToString();
                string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID, 
                    List_Login.LineID.ToString(), Com_PO.EditValue.ToString(),"1");

                if (string.IsNullOrEmpty(S_UnitStateID))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language,null, null, this.Panel_Data, IsFlashScreen);
                    SendFail();
                    return false;
                }
                if (string.IsNullOrEmpty(S_UnitStateID))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20131", "NG", List_Login.Language,null, null, this.Panel_Data, IsFlashScreen);
                    SendFail();
                    return false;
                }

                if (I_ChildCount < I_FullNumber)
                {
                    if (BoxType == "1" || BoxType == "3")
                    {
                        if (I_TTBindBoxUnitID > 0)
                        {
                            PanelUnitID = S_TTBindBoxUnitID;
                            UnitStatusID = S_UnitStateID;

                            return true;
                        } 
                    }
                    else if (BoxType == "2" && I_ChildCount>0)
                    {
                        PanelUnitID = S_TTBindBoxUnitID;
                        UnitStatusID = S_UnitStateID;

                        if (DT_PrintSn != null)
                        {
                            DT_PrintSn.Columns.Clear();
                            DT_PrintSn.Rows.Clear();
                        }
                        else
                        {
                            DT_PrintSn = new DataTable();
                        }
                        DT_PrintSn.Columns.Add("SN");
                        DT_PrintSn.Columns.Add("CreateTime");
                        DT_PrintSn.Columns.Add("PrintTime");
                        DataRow DR = DT_PrintSn.NewRow();
                        DR["SN"] = PanelSN;
                        DR["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        DT_PrintSn.Rows.Add(DR);

                        //打印条码
                        string S_PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, LabelPath, DT_PrintSn, Convert.ToInt32(Com_Part.EditValue.ToString()));
                        if (S_PrintResult != "OK")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(S_PrintResult.Replace("ERROR", ""), List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20111", "NG", List_Login.Language, new string[] { ProMsg }, ProMsg,
                                this.Panel_Data, IsFlashScreen);
                            SendFail();
                            return false;
                        }
                        else
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "10021", "OK", List_Login.Language, null, null, this.Panel_Data, IsFlashScreen);
                            SendPass();
                            return true;
                        }                        
                    }                    
                }


                mesUnit v_mesUnit = new mesUnit();
                v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                v_mesUnit.StatusID = 1;
                v_mesUnit.StationID = List_Login.StationID;
                v_mesUnit.EmployeeID = List_Login.EmployeeID;
                v_mesUnit.LineID = List_Login.LineID;
                v_mesUnit.ProductionOrderID = Convert.ToInt32(ProductionID);
                v_mesUnit.PartID = Convert.ToInt32(S_PartID);
                v_mesUnit.CreationTime = DateTime.Now;
                v_mesUnit.LastUpdate = DateTime.Now;
                v_mesUnit.PanelID = 0;
                v_mesUnit.RMAID = 0;
                v_mesUnit.LooperCount = 1;
                string UnitID = mesUnitSVC.Insert(v_mesUnit);
                PanelUnitID = UnitID;
                UnitStatusID = S_UnitStateID;

                //写入UnitDetail表
                mesUnitDetail msDetail = new mesUnitDetail();
                msDetail.UnitID = Convert.ToInt32(UnitID);
                if (BoxType == "1")
                {
                    msDetail.reserved_01 = "";
                    msDetail.reserved_02 = PanelSN;
                    msDetail.reserved_03 = "1";
                }
                else if (BoxType == "2")
                {
                    msDetail.reserved_01 = "";
                    msDetail.reserved_02 = PanelSN;
                    msDetail.reserved_03 = "1";
                }
                else if (BoxType=="3")
                {
                    msDetail.reserved_01 = S_MachineSN;
                    msDetail.reserved_02 = PanelSN;
                    msDetail.reserved_03 = "1";
                }
                //else
                //{
                //    msDetail.reserved_01 = "";
                //    msDetail.reserved_02 = "";
                //    msDetail.reserved_03 = "";
                //}

                msDetail.reserved_04 = "1";
                msDetail.reserved_05 = "";
                mesUnitDetailSVC.Insert(msDetail);

                mesSerialNumber v_mesSerialNumber = new mesSerialNumber();
                v_mesSerialNumber.UnitID = Convert.ToInt32(UnitID);
                if (BoxType == "3")
                {
                    v_mesSerialNumber.SerialNumberTypeID = 9;
                }
                else if (BoxType == "2")
                {
                    v_mesSerialNumber.SerialNumberTypeID = 8;
                }
                else if (BoxType == "1")
                {
                    v_mesSerialNumber.SerialNumberTypeID = 10;
                }

                v_mesSerialNumber.Value = PanelSN;
                mesSerialNumberSVC.Insert(v_mesSerialNumber);

                mesHistory v_mesHistory = new mesHistory();
                v_mesHistory.UnitID = Convert.ToInt32(UnitID);
                v_mesHistory.UnitStateID = v_mesUnit.UnitStateID;
                v_mesHistory.EmployeeID = v_mesUnit.EmployeeID;
                v_mesHistory.StationID = v_mesUnit.StationID;
                v_mesHistory.EnterTime = DateTime.Now;
                v_mesHistory.ExitTime = DateTime.Now;
                v_mesHistory.ProductionOrderID = v_mesUnit.ProductionOrderID;
                v_mesHistory.PartID = Convert.ToInt32(v_mesUnit.PartID);
                v_mesHistory.LooperCount = 1;
                v_mesHistory.StatusID = 1;
                mesHistorySVC.Insert(v_mesHistory);

                base.SetOverStiaonQTY(true);

                //if (BoxType == "2")
                //{
                //    if (DT_PrintSn != null)
                //    {
                //        DT_PrintSn.Columns.Clear();
                //        DT_PrintSn.Rows.Clear();
                //    }
                //    else
                //    {
                //        DT_PrintSn = new DataTable();
                //    }
                //    DT_PrintSn.Columns.Add("SN");
                //    DT_PrintSn.Columns.Add("CreateTime");
                //    DT_PrintSn.Columns.Add("PrintTime");
                //    DataRow DR = DT_PrintSn.NewRow();
                //    DR["SN"] = PanelSN;
                //    DR["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //    DT_PrintSn.Rows.Add(DR);

                //    //打印条码
                //    string S_PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, LabelPath, DT_PrintSn, Convert.ToInt32(Com_Part.EditValue.ToString()));
                //    if (S_PrintResult != "OK")
                //    {
                //        string ProMsg = MessageInfo.GetMsgByCode(S_PrintResult.Replace("ERROR", ""), List_Login.Language);
                //        MessageInfo.Add_Info_MSG(Edt_MSG, "20111", "NG", List_Login.Language, new string[] { ProMsg }, ProMsg, 
                //            this.Panel_Data, IsFlashScreen);
                //        SendFail();
                //        return false;
                //    }
                //    else
                //    {
                //        MessageInfo.Add_Info_MSG(Edt_MSG, "10021", "OK", List_Login.Language,null, null, this.Panel_Data, IsFlashScreen);
                //        SendPass();
                //        return true;
                //    }
                //}

                return true;
            }
            catch(Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() }, 
                    null, this.Panel_Data, IsFlashScreen);
                SendFail();
                return false;
            }
        }

        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {                   
                    string PanelSN = Edt_SN.Text.Trim();
                    PanelUnitID = string.Empty;
                    UnitStatusID = string.Empty;
                    string SN = string.Empty;
                    if (string.IsNullOrEmpty(PanelSN))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language, new string[] { PanelSN},
                            null, this.Panel_Data, IsFlashScreen);
                        Edt_SN.Text = string.Empty;
                        Edt_SN.Focus();
                        SendFail();
                        return;
                    }

                    if (BoxType == "3")
                    {
                        S_MachineSN= Edt_SN.Text.Trim();
                        string Result = string.Empty;
                        PartSelectSVC.uspCallProcedure("uspMachineToolingCheck", PanelSN,
                                                         null, xmlPart, null, "", List_Login.StationTypeID.ToString(), ref Result);
                        if (Result != "1")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { PanelSN, ProMsg }, Result, 
                                this.Panel_Data, IsFlashScreen);
                            Edt_SN.Text = "";
                            SendFail();
                            return;
                        }

                        if (string.IsNullOrEmpty(SNFormat))
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20132", "NG", List_Login.Language, new string[] { SNFormat }, null,
                                this.Panel_Data, IsFlashScreen);
                            txtChildSN.Text = string.Empty;
                            SendFail();
                            return;
                        }
                        else
                        {
                            string v_Sql = "select reserved_01 ValStr1,reserved_02 ValStr2,reserved_04 ValStr3 from mesUnitDetail where reserved_01='" +
                                S_MachineSN + "' and reserved_04='1'";
                            DataSet DS_Query = public_.P_DataSet(v_Sql);

                            if (DS_Query.Tables[0].Rows.Count > 0)
                            {
                                SN = DS_Query.Tables[0].Rows[0]["ValStr2"].ToString();
                            }
                            else
                            {
                                string S_xmlPart = "'<BoxSN SN=" + "\"" + PanelSN + "\"" + "> </BoxSN>'";
                                DataSet dsSN = PartSelectSVC.uspSNRGetNext(SNFormat, 0, null, S_xmlPart, null, null, null);
                                if (dsSN == null || dsSN.Tables.Count == 0 || dsSN.Tables[0].Rows.Count == 0)
                                {
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20087", "NG", List_Login.Language, null, null, this.Panel_Data, IsFlashScreen);
                                    txtChildSN.Text = string.Empty;
                                    SendFail();
                                    return;
                                }
                                SN = dsSN.Tables[1].Rows[0][0].ToString();
                            }

                            /////////////////////////////////////////////
                            /////////////////////////////////////////////
                            PanelSN = SN;                           
                        }
                    }
                    else
                    {
                        SN = PanelSN;
                    }

                    string xmlExtraData = "<ExtraData LineID =\"" + List_Login.LineID.ToString() + "\" " +
                                             "EmployeeID =\"" + List_Login.EmployeeID.ToString() + "\"> </ExtraData>";
                    string outString = string.Empty;
                    //通用方法校验
                    PartSelectSVC.uspCallProcedure("uspTTCheck", SN, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, "", ref outString);
                    if (outString != "1")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { SN, ProMsg }, outString, 
                            this.Panel_Data, IsFlashScreen);
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        SendFail();
                        return;
                    }

                    S_BoxSN = SN;


                    if (LoadMainSNData(SN))
                    {
                        if (BoxType == "3")
                        {
                            mesMachineSVC.MesModMachineBySNStationTypeID(Edt_SN.Text.Trim(), List_Login.StationTypeID);
                        }
                        Edt_SN.Enabled = false;
                        txtChildSN.Enabled = true;
                        txtChildSN.Focus();
                        ////////////////////////////////////////////////////////////
                        Edt_SN.Text = PanelSN;
                    }
                    else
                    {
                        Edt_SN.Text = string.Empty;
                        Edt_SN.Focus();
                    }

                    SetChildSN(PanelSN);
                }
                catch (Exception ex)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() }, 
                        null, this.Panel_Data, IsFlashScreen);
                    SendFail();
                }
            }
        }

        private void SetChildSN(string S_BoxSN)
        {
            string S_Result = public_.GetTTBindBoxChildSN(S_BoxSN, PartSelectSVC);
            Memo_ChildSN.Text = S_Result.Trim();

            txtShipmentQTY.Text = Memo_ChildSN.Lines.Count().ToString();
        }

        private void PrintSN(string S_PanelSN)
        {
            if (BoxType == "2")
            {
                if (DT_PrintSn != null)
                {
                    DT_PrintSn.Columns.Clear();
                    DT_PrintSn.Rows.Clear();
                }
                else
                {
                    DT_PrintSn = new DataTable();
                }
                DT_PrintSn.Columns.Add("SN");
                DT_PrintSn.Columns.Add("CreateTime");
                DT_PrintSn.Columns.Add("PrintTime");
                DataRow DR = DT_PrintSn.NewRow();
                DR["SN"] = S_PanelSN;
                DR["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DT_PrintSn.Rows.Add(DR);

                //打印条码
                string S_PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, LabelPath, DT_PrintSn, Convert.ToInt32(Com_Part.EditValue.ToString()));
                if (S_PrintResult != "OK")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(S_PrintResult.Replace("ERROR", ""), List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20111", "NG", List_Login.Language, new string[] { ProMsg }, ProMsg,
                        this.Panel_Data, IsFlashScreen);
                    SendFail();                   
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10021", "OK", List_Login.Language, null, null, this.Panel_Data, IsFlashScreen);
                    SendPass();                   
                }
            }
        }

        private void txtChildSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string ChildSN = txtChildSN.Text.Trim();
                string PanleSN = Edt_SN.Text.Trim();
                DataSet DS_TTBindBoxUnit = null;
                string S_RouteCheck = "1";

                string S_PartID = Com_Part.EditValue.ToString();
                string S_POID = Com_PO.EditValue.ToString();
                string S_PartFamilyID = Com_PartFamily.EditValue.ToString();

                if (string.IsNullOrEmpty(ChildSN))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language, new string[] { ChildSN }, null, 
                        this.Panel_Data, IsFlashScreen);
                    txtChildSN.Text = string.Empty;
                    txtChildSN.Focus();
                    SendFail();
                    return;
                }

                //if (!Regex.IsMatch(ChildSN, SN_Pattern))
                //{
                //    MessageInfo.Add_Info_MSG(Edt_MSG, "20027", "NG", List_Login.Language, new string[] { ChildSN }, null, this.Panel_Data, IsFlashScreen);
                //    txtChildSN.Focus();
                //    txtChildSN.Text = "";
                //    SendFail();
                //    return;
                //}

                DataSet DS_GetSN= PartSelectSVC.GetmesSerialNumber(ChildSN);
                if (DS_GetSN.Tables[0].Rows.Count == 0)
                {
                    //条码不存在或者状态不符.  20119
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20119", "NG", List_Login.Language, new string[] { ChildSN }, null,
                        this.Panel_Data, IsFlashScreen);
                    txtChildSN.Text = string.Empty;
                    txtChildSN.Focus();
                    SendFail();
                }

                string outString1 = string.Empty;
                if (S_IsCheckPO == "0")
                {
                    DataSet dsMainSN;
                    dsMainSN = PartSelectSVC.uspCallProcedure("uspGetBaseData", ChildSN,
                           null, null, null, null, null, ref outString1);
                    if (outString1 != "1" || dsMainSN == null || dsMainSN.Tables[0].Rows.Count <= 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20119", "NG", List_Login.Language, new string[] { "SN:" + ChildSN });
                        txtChildSN.Text = string.Empty;
                        txtChildSN.Focus();
                        return;
                    }

                    DataTable dt = dsMainSN.Tables[0];
                    S_POID = string.IsNullOrEmpty(S_POID) ? dt.Rows[0]["ProductionOrderID"].ToString() : S_POID;
                }

                if (S_IsCheckPN == "0")
                {
                    DataSet dsMainSN;
                    dsMainSN = PartSelectSVC.uspCallProcedure("uspGetBaseData", ChildSN,
                           null, null, null, null, null, ref outString1);
                    if (outString1 != "1" || dsMainSN == null || dsMainSN.Tables[0].Rows.Count <= 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20119", "NG", List_Login.Language, new string[] { "SN:" + ChildSN });
                        txtChildSN.Text = string.Empty;
                        txtChildSN.Focus();
                        return;
                    }

                    DataTable dt = dsMainSN.Tables[0];
                    S_POID = string.IsNullOrEmpty(S_POID) ? dt.Rows[0]["ProductionOrderID"].ToString() : S_POID;
                    S_PartID = string.IsNullOrEmpty(S_PartID) ? dt.Rows[0]["PartID"].ToString() : S_PartID;
                    S_PartFamilyID=string.IsNullOrEmpty(S_PartFamilyID) ? dt.Rows[0]["PartFamilyID"].ToString() : S_PartFamilyID;
                }

                DataSet unitDataset = PartSelectSVC.GetAndCheckUnitInfo(ChildSN, S_POID, S_PartID);
                if (unitDataset == null || unitDataset.Tables.Count <= 0 || unitDataset.Tables[0].Rows.Count <= 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20015", "NG", List_Login.Language, new string[] { "SN:" + ChildSN });
                    txtChildSN.Text = string.Empty;
                    txtChildSN.Focus();
                    return;
                }

                //校验工序是否正确
                DataSet dsUnit_Check = PartSelectSVC.GetSerialNumber2(ChildSN);
                string S_UnitID = dsUnit_Check.Tables[0].Rows[0]["UnitID"].ToString();
                DataTable DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0];
                string UnitStateID = string.Empty;
                S_RouteCheck = PartSelectSVC.GetRouteCheck_Diagram(List_Login.StationTypeID, List_Login.LineID.ToString(), DT_Unit, "", out UnitStateID);

                //正则值
                if (!Regex.IsMatch(ChildSN, SN_Pattern))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20027", "NG", List_Login.Language, new string[] { ChildSN }, null, this.Panel_Data, IsFlashScreen);
                    txtChildSN.Focus();
                    txtChildSN.Text = "";
                    SendFail();
                    return;
                }

                string xmlExtraData = "<ExtraData LineID =\"" + List_Login.LineID.ToString() + "\" " +
                        "EmployeeID =\"" + List_Login.EmployeeID.ToString() + "\"> </ExtraData>";
                string outString = string.Empty;
                //通用方法校验
                PartSelectSVC.uspCallProcedure("uspTTCheck", ChildSN, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, "", ref outString);
                if (outString != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { ChildSN, ProMsg }, outString,
                         this.Panel_Data, IsFlashScreen);
                    txtChildSN.Focus();
                    txtChildSN.Text = "";
                    SendFail();
                    return;
                }


                DS_TTBindBoxUnit = PartSelectSVC.GetmesUnitTTBox(PanleSN);
                DataTable DT_TTBindBoxUnit = DS_TTBindBoxUnit.Tables[0];

                string S_TTBindBoxUnitID = DT_TTBindBoxUnit.Rows[0]["ID"].ToString();
                string S_FullNumber = DT_TTBindBoxUnit.Rows[0]["FullNumber"].ToString() == "-1" ? "10" : DT_TTBindBoxUnit.Rows[0]["FullNumber"].ToString();
                string S_ChildCount = DT_TTBindBoxUnit.Rows[0]["ChildCount"].ToString() == "" ? "0" : DT_TTBindBoxUnit.Rows[0]["ChildCount"].ToString();
                string S_BoxSNStatus = DT_TTBindBoxUnit.Rows[0]["BoxSNStatus"].ToString() == "" ? "1" : DT_TTBindBoxUnit.Rows[0]["BoxSNStatus"].ToString();

                int I_FullNumber = Convert.ToInt32(S_FullNumber);
                int I_ChildCount = Convert.ToInt32(S_ChildCount);
                int I_BoxSNStatus = Convert.ToInt32(S_BoxSNStatus);

                if (I_ChildCount >= I_FullNumber)
                {
                    //TT箱子已经扫描结束       
                    MessageInfo.Add_Info_MSG(Edt_MSG, "60201", "NG", List_Login.Language, null, null, this.Panel_Data, IsFlashScreen);
                    return; 
                }   


                //判断 BoxSN
                //string S_PartID = Com_Part.EditValue.ToString();
                // string PartFamilyID = Com_PartFamily.EditValue.ToString();

                if (string.IsNullOrEmpty(PanleSN) && BoxType != "2")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language, new string[] { PanleSN }, 
                        null, this.Panel_Data, IsFlashScreen);
                    Edt_SN.Text = string.Empty;
                    txtChildSN.Text = string.Empty;
                    Edt_SN.Enabled = true;
                    txtChildSN.Enabled = false;
                    Edt_SN.Focus();
                    SendFail();
                    return;
                }


                if (PanleSN == "" &&  BoxType== "2")
                {                    
                    if (dsUnit_Check == null || dsUnit_Check.Tables.Count == 0 || dsUnit_Check.Tables[0].Rows.Count == 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language, new string[] { ChildSN }, null, this.Panel_Data, IsFlashScreen);
                        txtChildSN.Text = string.Empty;
                        txtChildSN.Focus();
                        SendFail();
                        return;
                    }

                    if (S_RouteCheck != "1")
                    {
                        DataSet DS_TTBindBoxStatus = public_.GetTTBindBoxStatus(S_PartID,S_POID, BoxType, S_IsCheckPO,List_Login, PartSelectSVC);
                        DataTable DT_TTBindBoxStatus = DS_TTBindBoxStatus.Tables[0];
                        if (DT_TTBindBoxStatus.Rows.Count > 0)
                        {
                            PanelUnitID = DT_TTBindBoxStatus.Rows[0]["ID"].ToString();
                            PanleSN = DT_TTBindBoxStatus.Rows[0]["PanleSN"].ToString();

                            Edt_SN.Text = PanleSN;

                            DataSet DS_PanleSN = PartSelectSVC.GetmesUnitTTBox(PanleSN);
                            txtShipmentQTY.Text = DS_PanleSN.Tables[0].Rows[0]["ChildCount"].ToString();

                            return;
                        }
                        else
                        {
                            //工艺流程校验失败,请确认.
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20241", "NG", List_Login.Language, new string[] { ChildSN }, null, this.Panel_Data, IsFlashScreen);
                            txtChildSN.Text = string.Empty;
                            txtChildSN.Focus();
                            SendFail();
                            return;
                        }
                    }
                    else
                    {
                        DataSet DS_TTBindBoxStatus = public_.GetTTBindBoxStatus(S_PartID, S_POID, BoxType, S_IsCheckPO, List_Login, PartSelectSVC);
                        DataTable DT_TTBindBoxStatus = DS_TTBindBoxStatus.Tables[0];
                        if (DT_TTBindBoxStatus.Rows.Count > 0)
                        {
                            PanelUnitID = DT_TTBindBoxStatus.Rows[0]["ID"].ToString();
                            PanleSN = DT_TTBindBoxStatus.Rows[0]["PanleSN"].ToString();

                            Edt_SN.Text = PanleSN;

                            DataSet DS_PanleSN = PartSelectSVC.GetmesUnitTTBox(PanleSN);
                            txtShipmentQTY.Text = DS_PanleSN.Tables[0].Rows[0]["ChildCount"].ToString();                            
                        }
                    }
                }

                if (PanleSN != "" && BoxType == "2")
                {
                    if (S_RouteCheck != "1")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(S_RouteCheck, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { ChildSN, ProMsg }, S_RouteCheck,
                            this.Panel_Data, IsFlashScreen);
                        txtChildSN.Focus();
                        txtChildSN.Text = "";
                        SendFail();
                        return;
                    }
                }

                if (BoxType == "3")
                {
                    if (PanelUnitID == "")
                    {
                        DataSet DS_BoxSN = PartSelectSVC.GetmesSerialNumber(S_BoxSN);
                        PanelUnitID = DS_BoxSN.Tables[0].Rows[0]["UnitID"].ToString(); 
                    }
                }


                    //if (BoxType == "2" && string.IsNullOrEmpty(PanelUnitID))
                    //{
                    //    DataSet DS_TTBindBoxStatus = public_.GetTTBindBoxStatus(Com_PO.EditValue.ToString(), PartSelectSVC);
                    //    if (DS_TTBindBoxStatus != null && DS_TTBindBoxStatus.Tables.Count > 0)
                    //    {
                    //        DataTable DT_TTBindBoxStatus = DS_TTBindBoxStatus.Tables[0];
                    //        if (DT_TTBindBoxStatus.Rows.Count > 0)
                    //        {
                    //            PanelUnitID = DT_TTBindBoxStatus.Rows[0]["ID"].ToString();
                    //            PanleSN=DT_TTBindBoxStatus.Rows[0]["PanleSN"].ToString();

                    //            Edt_SN.Text = PanleSN;
                    //        }
                    //    }
                    //}

                if (BoxType == "2" && string.IsNullOrEmpty(PanelUnitID))
                {
                    if (string.IsNullOrEmpty(SNFormat))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20132", "NG", List_Login.Language, new string[] { SNFormat }, null, this.Panel_Data, IsFlashScreen);
                        txtChildSN.Text = string.Empty;
                        SendFail();
                        return;
                    }
                    else
                    {
                         dsUnit_Check = PartSelectSVC.GetSerialNumber2(ChildSN);
                        if (dsUnit_Check == null || dsUnit_Check.Tables.Count == 0 || dsUnit_Check.Tables[0].Rows.Count == 0)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language, new string[] { ChildSN }, null, this.Panel_Data, IsFlashScreen);
                            txtChildSN.Text = string.Empty;
                            txtChildSN.Focus();
                            SendFail();
                            return;
                        }
                        //校验工序是否正确
                         S_UnitID = dsUnit_Check.Tables[0].Rows[0]["UnitID"].ToString();
                         DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0];
                         UnitStateID = string.Empty;
                        S_RouteCheck = PartSelectSVC.GetRouteCheck_Diagram(List_Login.StationTypeID, List_Login.LineID.ToString(), DT_Unit, "", out UnitStateID);
                        if (S_RouteCheck != "1")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(S_RouteCheck, List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { ChildSN, ProMsg }, S_RouteCheck,
                                this.Panel_Data, IsFlashScreen);
                            txtChildSN.Focus();
                            txtChildSN.Text = "";
                            SendFail();
                            return;
                        }


                        //////////////////////////////////////
                        ////////////////////////////////////

                        DataSet dsSN = PartSelectSVC.uspSNRGetNext(SNFormat, 0, "'" + xmlProdOrder + "'", "'" + xmlPart + "'", "'" + xmlStation + "'", "''", "''");
                        if (dsSN == null || dsSN.Tables.Count == 0 || dsSN.Tables[0].Rows.Count == 0)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20087", "NG", List_Login.Language, null,null, this.Panel_Data, IsFlashScreen);
                            txtChildSN.Text = string.Empty;
                            SendFail();
                            return;
                        }
                        string SN = dsSN.Tables[1].Rows[0][0].ToString();
                        if (!LoadMainSNData(SN))
                        {
                            txtChildSN.Text = string.Empty;
                            return;
                        }
                        else
                        {                            
                            Edt_SN.Text = SN;
                            PanleSN = Edt_SN.Text.Trim();

                            DataSet DS_TTNewSN = PartSelectSVC.GetSerialNumber2(PanleSN);
                            PanelUnitID = DS_TTNewSN.Tables[0].Rows[0]["ID"].ToString();
                        }
                    }
                }

                string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, S_PartFamilyID, List_Login.StationTypeID,
                    List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), "1");

                if (string.IsNullOrEmpty(S_UnitStateID))
                {
                    //当前工站类型在配置的工艺流程中未配置,请检查工艺流程.
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { ChildSN }, null, this.Panel_Data, IsFlashScreen);
                    txtChildSN.Text = string.Empty;
                    txtChildSN.Focus();
                    SendFail();
                    return;
                }

                DataSet dsUnit = PartSelectSVC.GetSerialNumber2(ChildSN);
                if (PanelUnitID == "")
                {
                    if (dsUnit != null && dsUnit.Tables.Count != 0 && dsUnit.Tables[0].Rows.Count != 0)
                    {
                        PanelUnitID = dsUnit.Tables[0].Rows[0]["UnitID"].ToString();
                    }
                }

                if (IsTTRegistSN)
                {
                    if (dsUnit != null && dsUnit.Tables.Count != 0 && dsUnit.Tables[0].Rows.Count != 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20113", "NG", List_Login.Language, new string[] { ChildSN }, null, this.Panel_Data, IsFlashScreen);
                        txtChildSN.Text = string.Empty;
                        txtChildSN.Focus();
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
                    v_mesUnit_New.PanelID = Convert.ToInt32(PanelUnitID);
                    v_mesUnit_New.LineID = List_Login.LineID;
                    v_mesUnit_New.ProductionOrderID = Convert.ToInt32(Com_PO.EditValue.ToString());
                    v_mesUnit_New.RMAID = 0;
                    v_mesUnit_New.PartID = Convert.ToInt32(S_PartID);
                    v_mesUnit_New.LooperCount = 1;
                    v_mesUnit_New.PartFamilyID = Convert.ToInt32(Com_PartFamily.EditValue);
                    v_mesUnit_New.SerialNumberType = 10;
                    string S_UnitId = mesUnitSVC.Insert(v_mesUnit_New);

                    if (string.IsNullOrEmpty(S_UnitId) || S_UnitId.Substring(0, 1) == "E")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(S_UnitId, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20130", "NG", List_Login.Language, new string[] { ChildSN, ProMsg }, S_UnitId, 
                             this.Panel_Data, IsFlashScreen);
                        txtChildSN.Focus();
                        txtChildSN.Text = "";
                        SendFail();
                        return;
                    }

                    //写入mesSerialNumber
                    mesSerialNumber v_mesSerialNumber = new mesSerialNumber();
                    v_mesSerialNumber.UnitID = Convert.ToInt32(S_UnitId);
                    v_mesSerialNumber.SerialNumberTypeID = 0;
                    v_mesSerialNumber.Value = ChildSN;
                    mesSerialNumberSVC.Insert(v_mesSerialNumber);


                    //写入UnitDetail表
                    mesUnitDetail msDetail = new mesUnitDetail();
                    msDetail.UnitID = Convert.ToInt32(S_UnitId);
                    msDetail.reserved_01 = "";
                    msDetail.reserved_02 = "";
                    msDetail.reserved_03 = "";
                    msDetail.reserved_04 = "";
                    msDetail.reserved_05 = "";
                    mesUnitDetailSVC.Insert(msDetail);

                    mesHistory v_mesHistory = new mesHistory();
                    v_mesHistory.UnitID = Convert.ToInt32(S_UnitId);
                    v_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                    v_mesHistory.EmployeeID = List_Login.EmployeeID;
                    v_mesHistory.StationID = List_Login.StationID;
                    v_mesHistory.EnterTime = DateTime.Now;
                    v_mesHistory.ExitTime = DateTime.Now;
                    v_mesHistory.ProductionOrderID = Convert.ToInt32(Com_PO.EditValue.ToString());
                    v_mesHistory.PartID = Convert.ToInt32(S_PartID);
                    v_mesHistory.LooperCount = 1;
                    mesHistorySVC.Insert(v_mesHistory);
                }
                else
                {
                    if (dsUnit == null || dsUnit.Tables.Count == 0 || dsUnit.Tables[0].Rows.Count == 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language, new string[] { ChildSN }, null, this.Panel_Data, IsFlashScreen);
                        txtChildSN.Text = string.Empty;
                        txtChildSN.Focus();
                        SendFail();
                        return;
                    }
                    //校验工序是否正确
                     S_UnitID = dsUnit.Tables[0].Rows[0]["UnitID"].ToString();
                     DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0];
                     UnitStateID = string.Empty;
                    //string result = PartSelectSVC.GetRouteCheck_Diagram(List_Login.StationTypeID, List_Login.LineID.ToString(), DT_Unit, "", out UnitStateID);
                    if (S_RouteCheck != "1")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(S_RouteCheck, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { ChildSN, ProMsg }, S_RouteCheck,
                            this.Panel_Data, IsFlashScreen);
                        txtChildSN.Focus();
                        txtChildSN.Text = "";
                        SendFail();
                        return;
                    }

                    mesUnit v_mesUnit = new mesUnit();
                    v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                    v_mesUnit.StationID = List_Login.StationID;
                    v_mesUnit.StatusID = 1;
                    v_mesUnit.LineID = List_Login.LineID;
                    v_mesUnit.EmployeeID = List_Login.EmployeeID;
                    v_mesUnit.PanelID = Convert.ToInt32(PanelUnitID);
                    v_mesUnit.PartID = Convert.ToInt32(S_PartID);
                    v_mesUnit.PartFamilyID = Convert.ToInt32(S_PartFamilyID);
                    mesUnitSVC.UpdateTTUnitStateID(v_mesUnit, S_UnitID);

                    mesHistory v_mesHistory = new mesHistory();
                    v_mesHistory.UnitID = Convert.ToInt32(S_UnitID);
                    v_mesHistory.UnitStateID = v_mesUnit.UnitStateID;
                    v_mesHistory.EmployeeID = v_mesUnit.EmployeeID;
                    v_mesHistory.StationID = v_mesUnit.StationID;
                    v_mesHistory.EnterTime = DateTime.Now;
                    v_mesHistory.ExitTime = DateTime.Now;
                    v_mesHistory.ProductionOrderID = Convert.ToInt32(S_POID);
                    v_mesHistory.PartID = Convert.ToInt32(v_mesUnit.PartID);
                    v_mesHistory.LooperCount = 1;
                    v_mesHistory.StatusID = 1;
                    mesHistorySVC.Insert(v_mesHistory);
                }

                CurrQTY++;
                MessageInfo.Add_Info_MSG(Edt_MSG, "10009", "OK", List_Login.Language, new string[]
                   { CurrQTY.ToString() + " ", ChildSN, "", Edt_SN.Text.Trim() }, null, this.Panel_Data, IsFlashScreen);
                SendPass();
                /////////////////////////////////////////////////////////////////
                DS_TTBindBoxUnit = PartSelectSVC.GetmesUnitTTBox(PanleSN);
                 DT_TTBindBoxUnit = DS_TTBindBoxUnit.Tables[0];
                 S_TTBindBoxUnitID = DT_TTBindBoxUnit.Rows[0]["ID"].ToString();
                 S_FullNumber = DT_TTBindBoxUnit.Rows[0]["FullNumber"].ToString()==""? "-1": DT_TTBindBoxUnit.Rows[0]["FullNumber"].ToString();
                 S_ChildCount = DT_TTBindBoxUnit.Rows[0]["ChildCount"].ToString() ==""? "0": DT_TTBindBoxUnit.Rows[0]["ChildCount"].ToString();
                 S_BoxSNStatus = DT_TTBindBoxUnit.Rows[0]["BoxSNStatus"].ToString() ==""? "1": DT_TTBindBoxUnit.Rows[0]["BoxSNStatus"].ToString();

                 I_FullNumber = Convert.ToInt32(S_FullNumber);
                 I_ChildCount = Convert.ToInt32(S_ChildCount);
                 I_BoxSNStatus = Convert.ToInt32(S_BoxSNStatus);

                ///////////////////////////////////////////////////////////////////

                SetChildSN(PanleSN);

                txtShipmentQTY.Text = S_ChildCount; //CurrQTY.ToString();
                PanleSN = Edt_SN.Text.Trim();
                txtChildSN.Text = string.Empty;
                //if (CurrQTY == Convert.ToInt32(CaseQTY))
                if (I_ChildCount>= I_FullNumber)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10014", "OK", List_Login.Language, new string[] { PanleSN },
                        null, this.Panel_Data, IsFlashScreen);
                    SendPass();
                    Memo_ChildSN.Text = "";

                    CurrQTY = 0;
                    Edt_SN.Text = string.Empty;
                    txtShipmentQTY.Text = "0";
                    PanelUnitID = string.Empty;
                    if (BoxType != "2")
                    {
                        Edt_SN.Enabled = true;
                        txtChildSN.Enabled = false;
                        Edt_SN.Focus();
                    }
                    //////////////////
                    public_.SetmesUnitDetail(S_TTBindBoxUnitID, PartSelectSVC);

                    PrintSN(PanleSN);
                }                
            }
        }

        public override void Com_luUnitStatus_EditValueChanged(object sender, EventArgs e)
        {
            base.Com_luUnitStatus_EditValueChanged(sender, e);
            string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();
            switch (S_luUnitStateID)
            {
                case "2":
                    lblDefectCode.ForeColor = Color.Red;
                    lblDefectCode.Visible = true;
                    break;
                case "3":
                    lblDefectCode.ForeColor = Color.Orange;
                    lblDefectCode.Visible = true;
                    break;
                default:
                    lblDefectCode.Visible = false;
                    break;
            }
        }

        public override void btnDefectSave_Click(object sender, EventArgs e)
        {
            base.btnDefectSave_Click(sender, e);
            lblDefectCode.Text = "DefectCode:" + DefectCode;
        }

        private void btnLastBox_Click(object sender, EventArgs e)
        {
            if (Edt_SN.Text.Trim() == "")
            {
                return;
            }

            DataSet DS_TTBindBoxUnit = PartSelectSVC.GetmesUnitTTBox(Edt_SN.Text);
            int I_ChildCount = Convert.ToInt32(DS_TTBindBoxUnit.Tables[0].Rows[0]["ChildCount"].ToString());

            if (I_ChildCount<1)
            {
                //这个箱子没有绑定任何数据
                MessageInfo.Add_Info_MSG(Edt_MSG, "60200", "NG", List_Login.Language, null, null, this.Panel_Data, IsFlashScreen);
                
                return;
            }
            DataTable DT_TTBindBoxUnit = DS_TTBindBoxUnit.Tables[0];
            string S_TTBindBoxUnitID = DT_TTBindBoxUnit.Rows[0]["ID"].ToString();
            public_.SetmesUnitDetail(S_TTBindBoxUnitID, PartSelectSVC);
            
            MessageInfo.Add_Info_MSG(Edt_MSG, "10014", "OK", List_Login.Language, new string[] { Edt_SN.Text.ToString() },
                null, this.Panel_Data, IsFlashScreen);
            SendPass();
            Memo_ChildSN.Text = "";

            PrintSN(Edt_SN.Text.Trim());

            CurrQTY = 0;
            Edt_SN.Text = string.Empty;
            txtShipmentQTY.Text = "0";
            PanelUnitID = string.Empty;
            if (BoxType != "2")
            {
                Edt_SN.Enabled = true;
                txtChildSN.Enabled = false;
                Edt_SN.Focus();
            }
            else
            {
                txtChildSN.Text = string.Empty;
                txtChildSN.Focus();
            }

        }

        private void TT_BindBox_Form_FormClosed(object sender, FormClosedEventArgs e)
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
