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
using System.Threading;

namespace App.MyMES
{
    public partial class Interlock_Form : FrmBase
    {
        string S_UnitID = "";
        string S_PartID = "";
        string S_UnitStateID = "";
        string S_luUnitStateID = "";
        string S_DefectID = "";
        string S_POPartID = "";
        DateTime dateStart;

        Boolean IsComExist = false;      //串口是否存在
        string S_PLCControl = "";        //是否控制PLC 
        string S_PCLAllowStart = "";     //PLC启动
        string S_PCLRuning = "";         //PLC运行中
        string S_PLCRunEnd = "";         //PLC完成 
        string S_PLCOnLineStatus = "";   //PLC在线状态  
        int I_PLCScanQTY = 0;            //扫描数量(数量达到发送启动命令)
        string S_PLCCom = "COM1";        //COM 端口
        int I_PLCBaudRate = 9600;        //PLC波特率
        Parity PLCParity = Parity.Odd;   //None = 0,Odd = 1,Even = 2,Mark = 3,Space = 4   
        int I_PLCDataBits = 7;            //PLC数据位
        StopBits PLCSTopBits = StopBits.One; //PLC停止位 
        int I_PLCTimeout = 10;            //PLC超时(秒)  


        public Interlock_Form()
        {
            InitializeComponent();
        }

        protected Boolean GetPLCSeting()
        {
            Boolean B_Result = true;
            try
            {
                DataSet DS_PLCSeting = PartSelectSVC.GetPLCSeting("", List_Login.StationID.ToString());
                S_PLCControl = (from c in DS_PLCSeting.Tables[0].AsEnumerable()
                                where c.Field<string>("Name") == "PLCControl"
                                select c.Field<String>("Value")
                                ).FirstOrDefault().ToString();

                S_PCLAllowStart = (from c in DS_PLCSeting.Tables[0].AsEnumerable()
                                   where c.Field<string>("Name") == "PCLAllowStart"
                                   select c.Field<String>("Value")
                                 ).FirstOrDefault().ToString();

                S_PCLRuning = (from c in DS_PLCSeting.Tables[0].AsEnumerable()
                               where c.Field<string>("Name") == "PCLRuning"
                               select c.Field<String>("Value")
                               ).FirstOrDefault().ToString();

                S_PLCRunEnd = (from c in DS_PLCSeting.Tables[0].AsEnumerable()
                               where c.Field<string>("Name") == "PLCRunEnd"
                               select c.Field<String>("Value")
                               ).FirstOrDefault().ToString();

                S_PLCOnLineStatus = (from c in DS_PLCSeting.Tables[0].AsEnumerable()
                                     where c.Field<string>("Name") == "PLCOnLineStatus"
                                     select c.Field<String>("Value")
                                     ).FirstOrDefault().ToString();

                I_PLCScanQTY = Convert.ToInt32((from c in DS_PLCSeting.Tables[0].AsEnumerable()
                                                where c.Field<string>("Name") == "PLCScanQTY"
                                                select c.Field<String>("Value")
                                ).FirstOrDefault().ToString());


                S_PLCCom = (from c in DS_PLCSeting.Tables[0].AsEnumerable()
                            where c.Field<string>("Name") == "PLCCom"
                            select c.Field<String>("Value")
                               ).FirstOrDefault().ToString();

                I_PLCBaudRate = Convert.ToInt32((from c in DS_PLCSeting.Tables[0].AsEnumerable()
                                                 where c.Field<string>("Name") == "PLCBaudRate"
                                                 select c.Field<String>("Value")
                                ).FirstOrDefault().ToString());

                I_PLCDataBits = Convert.ToInt32((from c in DS_PLCSeting.Tables[0].AsEnumerable()
                                                 where c.Field<string>("Name") == "PLCDataBits"
                                                 select c.Field<String>("Value")
                                ).FirstOrDefault().ToString());

                I_PLCTimeout = Convert.ToInt32((from c in DS_PLCSeting.Tables[0].AsEnumerable()
                                                where c.Field<string>("Name") == "PLCTimeout"
                                                select c.Field<String>("Value")
                                ).FirstOrDefault().ToString());

                string S_PLCStopBits = (from c in DS_PLCSeting.Tables[0].AsEnumerable()
                                        where c.Field<string>("Name") == "PLCStopBits"
                                        select c.Field<String>("Value")
                               ).FirstOrDefault().ToString();

                if (S_PLCStopBits == "None")
                {
                    PLCSTopBits = StopBits.None;
                }
                else if (S_PLCStopBits == "One")
                {
                    PLCSTopBits = StopBits.One;
                }
                else if (S_PLCStopBits == "Two")
                {
                    PLCSTopBits = StopBits.Two;
                }
                else if (S_PLCStopBits == "OnePointFive")
                {
                    PLCSTopBits = StopBits.OnePointFive;
                }


                string S_PLCParity = (from c in DS_PLCSeting.Tables[0].AsEnumerable()
                                      where c.Field<string>("Name") == "PLCParity"
                                      select c.Field<String>("Value")
                               ).FirstOrDefault().ToString();

                if (S_PLCParity == "None")
                {
                    PLCParity = Parity.None;
                }
                else if (S_PLCParity == "Odd")
                {
                    PLCParity = Parity.Odd;
                }
                else if (S_PLCParity == "Even")
                {
                    PLCParity = Parity.Even;
                }
                else if (S_PLCParity == "Mark")
                {
                    PLCParity = Parity.Mark;
                }
                else if (S_PLCParity == "Space")
                {
                    PLCParity = Parity.Space;
                }

                if (S_PLCControl == "" || S_PCLAllowStart == "" || S_PCLRuning == "" ||
                    S_PLCRunEnd == "" || S_PLCOnLineStatus == "" || I_PLCScanQTY == 0 ||
                    S_PLCCom == "" || PLCParity.ToString() == "" || I_PLCBaudRate == 0 ||
                    I_PLCDataBits == 0 || PLCSTopBits.ToString() == "" || I_PLCTimeout == 0)
                {
                    B_Result = false;
                }
            }
            catch (Exception ex)
            {
                B_Result = false;
            }
            return B_Result;
        }

        public override void Refresh()
        {
            base.Refresh();
            string S_PartID = Com_Part.EditValue.ToString();
            public_.AddPOAll(Com_PO, S_PartID, List_Login.LineID.ToString(),Grid_PO);
            List_SN.Items.Clear();

            Btn_Start.BackColor = Color.Gray;
            Btn_Run.BackColor = Color.Gray;
            Btn_End.BackColor = Color.Gray;

            SP_Main.Close();
            SP_Main.PortName = S_PLCCom;
            SP_Main.BaudRate = I_PLCBaudRate;
            SP_Main.Parity = PLCParity;
            SP_Main.DataBits = I_PLCDataBits;
            SP_Main.StopBits = PLCSTopBits;

            Lab_COM.Text = S_PLCCom;

            public_.AddComProt(Com_ComProt, Grid_Com);
            DataTable DT_Com = Com_ComProt.Properties.DataSource as DataTable;
            if (DT_Com.Rows.Count > 0)
            {
                for (int i = 0; i < DT_Com.Rows.Count; i++)
                {
                    if (S_PLCCom == DT_Com.Rows[i]["Description"].ToString())
                    {
                        Com_ComProt.Text = i.ToString();
                    }
                }
                IsComExist = true;
            }
            else
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20057", "NG", List_Login.Language);
                return;
            }

            if (GetPLCSeting() == false)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20056", "NG", List_Login.Language);
                return;
            }


            if (S_PCLAllowStart == "1")
            {
                RunPLC(S_PLCOnLineStatus);
                Thread.Sleep(200);

                try
                {
                    string S_ReadValue = SP_Main.ReadExisting();
                    if (S_ReadValue.Length == 5)
                    {
                    }
                    else
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20067", "NG", List_Login.Language);
                    }
                }
                catch (Exception ex)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                }

                if (S_PLCControl == "1")
                {
                    timer_OnLine.Enabled = true;
                }
                else
                {
                    timer_OnLine.Enabled = false;
                }
            }
        }


        /// <summary>
        /// 第一站 扫描顺序号
        /// </summary>
        /// <param name="PartID"></param>
        /// <param name="StationTypeID"></param>
        /// <returns></returns>
        private int FirstScanSequence(string PartID, int StationTypeID)
        {
            int I_Result = 1;
            string PartFamilyID = Com_PartFamily.EditValue.ToString();
            if (public_.IsOneStationPrint(PartID, PartFamilyID, StationTypeID, List_Login.LineID.ToString(), Com_PO.EditValue.ToString()))
            {
                I_Result = 2;
            }
            return I_Result;
        }

        private Boolean RunPLC(string S_CommandPLC)
        {
            Boolean B_Result = false;

            if (IsComExist == true)
            {
                try
                {
                    if (SP_Main.IsOpen == false)
                    {
                        SP_Main.Open();
                    }
                }
                catch (Exception ex)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                    return B_Result;
                }

                try
                {
                    Byte[] data = new Byte[16];
                    data[0] = Convert.ToByte(Convert.ToInt32("5", 16));
                    data[1] = Convert.ToByte(Convert.ToInt32("30", 16));
                    data[2] = Convert.ToByte(Convert.ToInt32("30", 16));
                    data[3] = Convert.ToByte(Convert.ToInt32("46", 16));
                    data[4] = Convert.ToByte(Convert.ToInt32("46", 16));
                    data[5] = Convert.ToByte(Convert.ToInt32("42", 16));
                    data[6] = Convert.ToByte(Convert.ToInt32("57", 16));
                    data[7] = Convert.ToByte(Convert.ToInt32("33", 16));
                    data[8] = Convert.ToByte(Convert.ToInt32("4D", 16));

                    data[9] = Convert.ToByte(public_.ASC(S_CommandPLC.Substring(1, 1)));
                    data[10] = Convert.ToByte(public_.ASC(S_CommandPLC.Substring(2, 1)));
                    data[11] = Convert.ToByte(public_.ASC(S_CommandPLC.Substring(3, 1)));
                    data[12] = Convert.ToByte(public_.ASC(S_CommandPLC.Substring(4, 1)));

                    data[13] = Convert.ToByte(Convert.ToInt32("30", 16));
                    data[14] = Convert.ToByte(Convert.ToInt32("31", 16));
                    data[15] = Convert.ToByte(Convert.ToInt32("31", 16));

                    SP_Main.DiscardOutBuffer();
                    SP_Main.Write(data, 0, 16);

                    B_Result = true;
                }
                catch (Exception ex)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                }
            }
            else
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20057", "NG", List_Login.Language);
                timer_OnLine.Enabled = false;
                Btn_OnLine.BackColor = Color.Black;

            }
            return B_Result;
        }

        private Boolean RunPLC2(string S_CommandPLC)
        {
            Boolean B_Result = false;

            if (IsComExist == true)
            {
                try
                {
                    if (SP_Main.IsOpen == false)
                    {
                        SP_Main.Open();
                    }
                }
                catch (Exception ex)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                    return B_Result;
                }

                try
                {
                    Byte[] data = new Byte[15];
                    data[0] = Convert.ToByte(Convert.ToInt32("5", 16));
                    data[1] = Convert.ToByte(Convert.ToInt32("30", 16));
                    data[2] = Convert.ToByte(Convert.ToInt32("30", 16));
                    data[3] = Convert.ToByte(Convert.ToInt32("46", 16));
                    data[4] = Convert.ToByte(Convert.ToInt32("46", 16));
                    data[5] = Convert.ToByte(Convert.ToInt32("42", 16));
                    data[6] = Convert.ToByte(Convert.ToInt32("52", 16));
                    data[7] = Convert.ToByte(Convert.ToInt32("33", 16));
                    data[8] = Convert.ToByte(Convert.ToInt32("4D", 16));

                    data[9] = Convert.ToByte(public_.ASC(S_CommandPLC.Substring(1, 1)));
                    data[10] = Convert.ToByte(public_.ASC(S_CommandPLC.Substring(2, 1)));
                    data[11] = Convert.ToByte(public_.ASC(S_CommandPLC.Substring(3, 1)));
                    data[12] = Convert.ToByte(public_.ASC(S_CommandPLC.Substring(4, 1)));

                    data[13] = Convert.ToByte(Convert.ToInt32("30", 16));
                    data[14] = Convert.ToByte(Convert.ToInt32("31", 16));

                    SP_Main.DiscardOutBuffer();

                    //for (int i = 1; i <= 1000; i++)
                    //{

                    //}   

                    SP_Main.Write(data, 0, 15);
                    string S_PLCResult = SP_Main.ReadExisting();
                    if (S_PLCResult.Length == 7 && S_PLCResult.Substring(6, 1) == "1")
                    {
                        B_Result = true;

                    }

                    Thread.Sleep(200);

                }
                catch (Exception ex)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                }
            }
            else
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20057", "NG", List_Login.Language);
                timer_OnLine.Enabled = false;
                Btn_OnLine.BackColor = Color.Black;

            }
            return B_Result;
        }

        private void SetFilish()
        {
            try
            {
                for (int i = 0; i < I_PLCScanQTY; i++)
                {
                    string S_SN = List_SN.Items[i].ToString();

                    string S_POID = DT_ProductionOrder.Rows[0]["ID"].ToString();
                    DataTable DT_UPUnitID = PartSelectSVC.Get_UnitID(S_SN).Tables[0];

                    mesUnit v_mesUnit = new mesUnit();
                    v_mesUnit.ID = Convert.ToInt32(DT_UPUnitID.Rows[0]["UnitID"].ToString());
                    v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                    v_mesUnit.StatusID = Convert.ToInt32(S_luUnitStateID);
                    v_mesUnit.StationID = List_Login.StationID;
                    v_mesUnit.EmployeeID = List_Login.EmployeeID;
                    v_mesUnit.LastUpdate = DateTime.Now;
                    v_mesUnit.ProductionOrderID = Convert.ToInt32(S_POID);
                    //修改 Unit
                    string S_UpdateUnit = mesUnitSVC.Update(v_mesUnit);
                    if (S_UpdateUnit.Substring(0, 1) == "E")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_UpdateUnit });
                        return;
                    }

                    if (S_UpdateUnit.Substring(0, 1) != "E")
                    {
                        mesHistory v_mesHistory = new mesHistory();

                        v_mesHistory.UnitID = Convert.ToInt32(v_mesUnit.ID);
                        v_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                        v_mesHistory.EmployeeID = List_Login.EmployeeID;
                        v_mesHistory.StationID = List_Login.StationID;
                        v_mesHistory.EnterTime = DateTime.Now;
                        v_mesHistory.ExitTime = DateTime.Now;
                        v_mesHistory.ProductionOrderID = Convert.ToInt32(S_POID);
                        v_mesHistory.PartID = Convert.ToInt32(S_POPartID);
                        v_mesHistory.LooperCount = 1;
                        int I_InsertHistory = mesHistorySVC.Insert(v_mesHistory);

                        //////////////////////////  Defect ///////////////////////////////////////////
                        //////////////////////////////////////////////////////////////////////////////
                        string[] Array_Defect = S_DefectID.Split(';');
                        if (S_luUnitStateID != "1")
                        {
                            foreach (var item in Array_Defect)
                            {
                                string S_mesUnitDefectInsert = "";
                                try
                                {
                                    if (item.Trim() != "")
                                    {
                                        int I_DefectID = Convert.ToInt32(item);
                                        mesUnitDefect v_mesUnitDefect = new mesUnitDefect();

                                        v_mesUnitDefect.UnitID = v_mesUnit.ID;
                                        v_mesUnitDefect.DefectID = I_DefectID;
                                        v_mesUnitDefect.StationID = List_Login.StationID;
                                        v_mesUnitDefect.EmployeeID = List_Login.EmployeeID;

                                        S_mesUnitDefectInsert = mesUnitDefectSVC.Insert(v_mesUnitDefect);
                                    }
                                }
                                catch(Exception ex)
                                {
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                }
                            }
                        }

                        TimeSpan ts = DateTime.Now - dateStart;
                        double mill = Math.Round(ts.TotalMilliseconds, 0);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { S_SN, mill.ToString() });
                        /////////////////////////////////////////////////////////////////////////
                        /////////////////////////////////////////////////////////////////////////
                        if (S_luUnitStateID == "1")
                        {
                            base.SetOverStiaonQTY(true);
                        }
                        else if (S_luUnitStateID == "2")
                        {
                            base.SetOverStiaonQTY(false);
                        }
                    }
                }
                Edt_SN.Text = "";
                Edt_SN.Enabled = true;
                Edt_SN.Focus();
                List_SN.Items.Clear();

                Btn_Run.BackColor = Color.Gray;
                Btn_Start.BackColor = Color.Gray;
                Btn_End.BackColor = Color.Gray;
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dateStart = DateTime.Now;

                S_PartID = base.Com_Part.EditValue.ToString();
                string PartFamilyID = Com_PartFamily.EditValue.ToString();
                S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID,List_Login.StationTypeID, 
                    List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());
                S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();
                S_DefectID = base.DefectChar;
                string S_SN = Edt_SN.Text.Trim();

                if (string.IsNullOrEmpty(S_UnitStateID))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }

                if (S_luUnitStateID == "2" || S_luUnitStateID == "3")
                {
                    if (S_DefectID == "")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20049", "NG", List_Login.Language);
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }
                }

                S_POPartID = Com_Part.EditValue.ToString();
                int I_FirstScanSequence = FirstScanSequence(S_POPartID, List_Login.StationTypeID);

                List<string> List_PO = public_.SnToPOID(S_SN);
                string S_POID = List_PO[0];

                if (S_POID.Length > 4)
                {
                    if (S_POID.Substring(0, 5) == "ERROR")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(S_POID.Replace("ERROR", ""), List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, S_POID.Replace("ERROR", ""));
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }
                }

                if (S_POID == "" || S_POID == "0")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20014", "NG", List_Login.Language, new string[] { S_SN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                };

                //根据  SN  获取工单  料号
                mesProductionOrder v_mesProductionOrder = mesProductionOrderSVC.Get(Convert.ToInt32(S_POID));
                S_POPartID = v_mesProductionOrder.PartID.ToString(); 
                I_FirstScanSequence = FirstScanSequence(S_POPartID, List_Login.StationTypeID);

                string S_ComPO = Com_PO.EditValue.ToString();
                if (S_POID != S_ComPO)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20015", "NG", List_Login.Language, new string[] { S_SN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }

                string S_Sql = "";
                DataTable DT_SN = PartSelectSVC.GetmesSerialNumber(S_SN).Tables[0];

                if (DT_SN.Rows.Count > 0)
                {
                    S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                    DataTable DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0];
                    string S_StationID = List_Login.StationID.ToString();

                    //string S_SerialNumberType = DT_SN.Rows[0]["SerialNumberTypeID"].ToString();
                    //if (S_SerialNumberType != "5")
                    //{
                    //    MessageInfo.Add_Info_MSG(Edt_MSG, "20035", "NG", List_Login.Language);
                    //    Edt_SN.Focus();
                    //    Edt_SN.Text = "";
                    //    return;
                    //}

                    if (DT_Unit.Rows[0]["StatusID"].ToString() != "1")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20036", "NG", List_Login.Language);
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }
                    string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, S_SN);
                    Boolean B_SN_Exist = false; //是否已扫描此条码
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    if (S_RouteCheck == "1")
                    {
                        string S_TimeCheck = PartSelectSVC.TimeCheck(List_Login.StationID.ToString(), S_SN);
                        if (S_TimeCheck != "1")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(S_TimeCheck, List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, S_TimeCheck);
                            return;
                        }

                        string S_SelectPartID = DT_Unit.Rows[0]["PartID"].ToString();
                        if (S_POPartID == S_SelectPartID)
                        {
                            for (int i = 0; i < List_SN.Items.Count; i++)
                            {
                                string S_ListSN = List_SN.Items[i].ToString().Trim();
                                if (S_SN == S_ListSN)
                                {
                                    B_SN_Exist = true;
                                }
                            }

                            if (B_SN_Exist == true)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20059", "NG", List_Login.Language);
                                return;
                            }
                            List_SN.Items.Add(S_SN);
                            //一组数量完成开始处理
                            if (List_SN.Items.Count == I_PLCScanQTY)
                            {
                                try
                                {
                                    Edt_SN.Enabled = false;
                                    string S_PLCResult = "";
                                    if (S_PLCControl == "1")
                                    {
                                        timer_OnLine.Enabled = false;
                                        if (RunPLC(S_PCLAllowStart) == true)
                                        {
                                            Thread.Sleep(300);
                                            S_PLCResult = SP_Main.ReadExisting();

                                            if (S_PLCResult.Length == 5)//PLC  开机启动成功
                                            {
                                                MessageInfo.Add_Info_MSG(Edt_MSG, "20060", "OK", List_Login.Language);
                                                Btn_Start.BackColor = Color.Lime;

                                                if (RunPLC2(S_PCLRuning) == true)
                                                {
                                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20061", "OK", List_Login.Language);
                                                    Btn_Run.BackColor = Color.Lime;
                                                    timer_Run.Enabled = true;
                                                }
                                                else
                                                {
                                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20062", "NG", List_Login.Language);
                                                    Btn_Run.BackColor = Color.Red;
                                                    timer_Run.Enabled = false;                                                   
                                                    return;
                                                }

                                            }
                                            else
                                            {
                                                MessageInfo.Add_Info_MSG(Edt_MSG, "20063", "NG", List_Login.Language);
                                                Btn_Start.BackColor = Color.Red;
                                                timer_Run.Enabled = false;                                              
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            MessageInfo.Add_Info_MSG(Edt_MSG, "20064", "NG", List_Login.Language);
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        SetFilish();
                                    }

                                }
                                catch(Exception ex)
                                {
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                }
                            }
                        }
                        else
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20050", "NG", List_Login.Language);
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                        }
                    }
                    else
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(S_RouteCheck, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Edt_SN.Text.ToString(), ProMsg }, S_RouteCheck);
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                    }
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language,new string[] { S_SN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                }
            }
        }

        private void timer_OnLine_Tick(object sender, EventArgs e)
        {
            if (S_PLCControl == "0")
            {
                timer_OnLine.Enabled = false;
                return;
            }

            try
            {
                string S_ReadValue = SP_Main.ReadExisting();
                MessageInfo.Add_Info_MSG(Edt_MSG, "10011", "OK", List_Login.Language,new string[] { S_ReadValue + "\r\n" });

            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        int I_RunTime = 0;
        private void timer_Run_Tick(object sender, EventArgs e)
        {
            try
            {
                if (I_RunTime >= I_PLCTimeout)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20065", "NG", List_Login.Language);
                    Btn_Run.BackColor = Color.Red;
                    timer_Run.Enabled = false;////////////////////////////////////    
                    Edt_SN.Text = "";
                    Edt_SN.Enabled = false;
                    List_SN.Items.Clear();
                    I_RunTime = 0;
                    return;
                }
                I_RunTime += 1;

                string S_ReadValue = SP_Main.ReadExisting();
                if (S_ReadValue.Length == 7 && S_ReadValue.Substring(6, 1) == "1") //运行中完成
                {
                    timer_Run.Enabled = false;
                    I_RunTime = 0;

                    if (RunPLC2(S_PLCRunEnd) == true)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "10012", "OK", List_Login.Language);
                        Btn_End.BackColor = Color.Lime;
                        timer_Filish.Enabled = true;/////////////////////////////////// 
                    }
                    else
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20066", "NG", List_Login.Language);
                        Btn_End.BackColor = Color.Red;
                        timer_Filish.Enabled = false;////////////////////////////////////
                        Edt_SN.Enabled = false;
                        return;
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }


        int I_RunEndTime = 0;
        private void timer_Filish_Tick(object sender, EventArgs e)
        {
            try
            {
                if (I_RunEndTime >= I_PLCTimeout)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20065", "NG", List_Login.Language);
                    Btn_End.BackColor = Color.Red;
                    timer_Filish.Enabled = false;////////////////////////////////////   
                    Edt_SN.Text = "";
                    Edt_SN.Enabled = false;
                    List_SN.Items.Clear();
                    I_RunTime = 0;
                    return;
                }
                I_RunEndTime += 1;

                string S_ReadValue = SP_Main.ReadExisting();
                if (S_ReadValue.Length == 7 || S_ReadValue.Substring(6, 1) == "1") //运行完成完毕
                {
                    timer_Filish.Enabled = false;
                    //PLC 运行完毕后
                    //SetFilish();

                    timer_OnLine.Enabled = true;
                    I_RunEndTime = 0;
                    Edt_SN.Enabled = true;
                    Edt_SN.Focus();
                    List_SN.Items.Clear();

                    Btn_Start.BackColor = Color.Gray;
                    Btn_Run.BackColor = Color.Gray;
                    Btn_End.BackColor = Color.Gray;
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                I_RunEndTime = 0;
            }
        }

        public string Update_SFC(string S_SN)
        {
            string S_Result = "1";
            try
            {
                string S_Sql = "select * from mesSerialNumber A join mesUnit B on A.UnitID=B.ID where A.Value='" + S_SN + "'";
                DataTable DT_Unit = new DataTable();
                //DT_Unit = public_.P_DataSet(S_Sql).Tables[0];

                DT_Unit = PartSelectSVC.GetSerialNumber2(S_SN).Tables[0];

                string S_PartID = DT_Unit.Rows[0]["PartID"].ToString();
                string PartFamilyID = Com_PartFamily.EditValue.ToString();
                string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID, 
                    List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());

                if (string.IsNullOrEmpty(S_UnitStateID))
                {
                    return "20203";
                }

                string S_POID = DT_Unit.Rows[0]["ProductionOrderID"].ToString();
                DataTable DT_UPUnitID = PartSelectSVC.Get_UnitID(S_SN).Tables[0];

                int I_UnitID = Convert.ToInt32(DT_UPUnitID.Rows[0]["UnitID"].ToString());
                mesUnit F_mesUnit = mesUnitSVC.Get(I_UnitID);

                mesUnit v_mesUnit = new mesUnit();
                v_mesUnit.ID = I_UnitID;
                v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                v_mesUnit.StatusID = F_mesUnit.StatusID;
                v_mesUnit.StationID = List_Login.StationID;
                v_mesUnit.EmployeeID = List_Login.EmployeeID;
                v_mesUnit.LastUpdate = DateTime.Now;
                v_mesUnit.ProductionOrderID = Convert.ToInt32(S_POID);
                //修改 Unit
                string S_UpdateUnit = mesUnitSVC.Update(v_mesUnit);
                if (S_UpdateUnit.Substring(0, 1) == "E")
                {
                    return S_Result = S_UpdateUnit;
                }

                if (S_UpdateUnit.Substring(0, 1) != "E")
                {
                    mesHistory v_mesHistory = new mesHistory();

                    v_mesHistory.UnitID = Convert.ToInt32(v_mesUnit.ID);
                    v_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                    v_mesHistory.EmployeeID = List_Login.EmployeeID;
                    v_mesHistory.StationID = List_Login.StationID;
                    v_mesHistory.EnterTime = DateTime.Now;
                    v_mesHistory.ExitTime = DateTime.Now;
                    v_mesHistory.ProductionOrderID = Convert.ToInt32(S_POID);
                    v_mesHistory.PartID = Convert.ToInt32(S_PartID);
                    v_mesHistory.LooperCount = 1;
                    int I_InsertHistory = mesHistorySVC.Insert(v_mesHistory);
                }
            }
            catch (Exception ex)
            {
                return S_Result = ex.ToString();
            }
            return S_Result;
        }

        public override void FrmBase_Load(object sender, EventArgs e)
        {
            base.FrmBase_Load(sender, e);
            GetPLCSeting();
        }
    }
}