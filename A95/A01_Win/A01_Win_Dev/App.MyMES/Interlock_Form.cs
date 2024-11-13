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
using App.MyMES.mesUnitService;
using App.MyMES.mesSerialNumberService;
using App.MyMES.mesHistoryService;
using App.MyMES.mesProductionOrderService;
using App.MyMES.mesProductStructureService;
using App.MyMES.mesUnitComponentService;
using App.MyMES.mesUnitDefectService;
using App.MyMES.mesPartService;
using System.Configuration;
using System.Threading;
using Microsoft.VisualBasic.Devices;
using System.IO.Ports;

namespace App.MyMES
{
    public partial class Interlock_Form : Form
    {
        Public_ public_ = new Public_();
        DataTable DT_ProductionOrder = new DataTable();
        DataTable DT_ProductStructure = new DataTable();
        LoginList List_Login = new LoginList();

        PartSelectSVCClient PartSelectSVC; 
        ImesUnitSVCClient mesUnitSVC ;
        ImesSerialNumberSVCClient mesSerialNumberSVC;
        ImesHistorySVCClient mesHistorySVC ;
        ImesUnitComponentSVCClient mesUnitComponentSVC;
        ImesUnitDefectSVCClient mesUnitDefectSVC;
        ImesPartSVCClient mesPartSVC;
        ImesProductionOrderSVCClient mesProductionOrderSVC;

        int I_RouteSequence = -9;  //当前工序顺序 
        int ScanOKQuantity = 0;
        int ScanNGQuantity = 0;
        int I_POID = 0;
        string S_DefectTypeID = "";
        
        Boolean IsComExist = false;      //串口是否存在
        string S_PLCControl = "";        //是否控制PLC 
        string S_PCLAllowStart = "";     //PLC启动
        string S_PCLRuning = "";         //PLC运行中
        string S_PLCRunEnd = "";         //PLC完成 
        string S_PLCOnLineStatus = "";   //PLC在线状态  
        int I_PLCScanQTY = 0;            //扫描数量(数量达到发送启动命令)

        string S_PLCCom = "COM1";       //COM 端口
        int I_PLCBaudRate = 9600;       //PLC波特率
        Parity PLCParity = Parity.Odd;  //None = 0,Odd = 1,Even = 2,Mark = 3,Space = 4   

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
                    S_PLCRunEnd == "" || S_PLCOnLineStatus == "" || I_PLCScanQTY==0||
                    S_PLCCom==""  || PLCParity.ToString()=="" ||    I_PLCBaudRate ==0 ||
                    I_PLCDataBits==0 || PLCSTopBits .ToString() == "" || I_PLCTimeout==0) 
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


        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            try
            {
                PartSelectSVC.Close(); 
                mesUnitSVC.Close();
                mesSerialNumberSVC.Close();
                mesHistorySVC.Close();
                mesUnitComponentSVC.Close();
                mesUnitDefectSVC.Close();
                mesPartSVC.Close();
                mesProductionOrderSVC.Close();
            }
            catch
            { }

            PartSelectSVC = PartSelectFactory.CreateServerClient();
            mesUnitSVC = ImesUnitFactory.CreateServerClient();
            mesSerialNumberSVC = ImesSerialNumberFactory.CreateServerClient();
            mesHistorySVC = ImesHistoryFactory.CreateServerClient();
            mesUnitComponentSVC = ImesUnitComponentFactory.CreateServerClient();
            mesUnitDefectSVC = ImesUnitDefectFactory.CreateServerClient();
            mesPartSVC = ImesPartFactory.CreateServerClient();
            mesProductionOrderSVC = ImesProductionOrderFactory.CreateServerClient();

            List_Login = this.Tag as LoginList;
            ///////////////////////////
            public_.AddStationType(Com_StationType, Grid_StationType);
            public_.AddLine(Com_Line,Grid_Line);
            //public_.AddmesUnitState(Com_UnitState);

            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_LineID = List_Login.LineID.ToString();
            public_.AddStation(Com_Station,S_LineID,Grid_Station);

            Com_StationType.Text = S_StationTypeID;
            Com_Line.Text = S_LineID;
            Com_Station.Text = List_Login.StationID.ToString();

            public_.AddPartFamilyType(Com_PartFamilyType,Grid_PartFamilyType);
            public_.AddluUnitStatus(Com_luUnitStatus,Grid_luUnitStatus);

            //DataRowView DRV = Com_Part.SelectedItem as DataRowView;
            string S_PartID = Com_Part.EditValue.ToString();   //DRV["ID"].ToString();
            public_.AddPOAll(Com_PO, S_PartID, List_Login.LineID.ToString(), Grid_PO);
            Com_PO.Text = ""; 

            Edt_SN.Focus();
            Edt_SN.SelectAll(); 

            //Edt_MSG.Text = ""; 
            Edt_SN.Text = "";
            Edt_SN.Enabled = true;
            PartEdtStatus(true);
            List_SN.Items.Clear();

//            Btn_OnLine.BackColor = Color.Gray;
            Btn_Start.BackColor = Color.Gray;
            Btn_Run.BackColor= Color.Gray;
            Btn_End.BackColor = Color.Gray;

            //if (List_Login.ApplicationTypeID == 26) //Interlock
            {
                SP_Main.Close();
                SP_Main.PortName = S_PLCCom;
                SP_Main.BaudRate = I_PLCBaudRate;
                SP_Main.Parity = PLCParity;
                SP_Main.DataBits = I_PLCDataBits;
                SP_Main.StopBits = PLCSTopBits;

                Lab_COM.Text = S_PLCCom;

                public_.AddComProt(Com_ComProt, Grid_Com);
                DataTable DT_Com = Com_ComProt.Properties.DataSource as DataTable;
                if (DT_Com.Rows.Count>0)
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
                    MessageBox.Show("当前计算机上不存在串口，请检查！", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (GetPLCSeting() == false)
                {
                    Public_.Add_Info_MSG(Edt_MSG, "  PLC 参数配置错误，请确认！", "NG");
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
                            MessageBox.Show("置位在线状态数据写入PLC失败，请检查", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        }

        /// <summary>
        /// Part  编辑框状态
        /// </summary>
        /// <param name="B_Status"></param>
        private void PartEdtStatus(Boolean B_Status)
        {
            Com_PartFamilyType.Enabled = B_Status;
            Com_PartFamily.Enabled = B_Status;
            Com_Part.Enabled = B_Status;
            Com_Part.Enabled = B_Status;
            Com_PO.Enabled = B_Status;
            Btn_ConfirmPO.Enabled = B_Status;
            Com_ComProt.Enabled = B_Status;

            if (B_Status == true)
            {
                Com_PartFamilyType.BackColor = Color.White;
                Com_PartFamily.BackColor = Color.White;
                Com_Part.BackColor = Color.White;
                Com_PO.BackColor = Color.White;
                Com_ComProt.BackColor = Color.White;

                Edt_SN.Enabled = false; 
            }
            else
            {
                Com_PartFamilyType.BackColor = Color.Yellow; 
                Com_PartFamily.BackColor = Color.Yellow; 
                Com_Part.BackColor = Color.Yellow; 
                Com_PO.BackColor = Color.Yellow;
                Com_ComProt.BackColor= Color.Yellow;

                Edt_SN.Enabled = true;
            }

            Edt_SN.Focus();
            Edt_SN.SelectAll();
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
            if (public_.IsOneStationPrint(PartID, StationTypeID, List_Login.LineID.ToString()))
            {
                I_Result = 2;
            }
            return I_Result;
        }


        private void OverStation_Form_Load(object sender, EventArgs e)
        {
            Btn_Refresh_Click(sender, e);
            Com_PO.Enabled = true;          
        }

        private Boolean IsFirstSequence()
        {
            string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
            I_RouteSequence = public_.RouteSequence(S_PartID, List_Login.StationTypeID, List_Login.LineID.ToString());
            int I_FirstScanSequence = FirstScanSequence(S_PartID, List_Login.StationTypeID);

            Boolean B_Result = true;
            if (I_RouteSequence > I_FirstScanSequence)
            {
                B_Result = false; 
            }

            return B_Result;
        }

        private void Get_DTPO(string S_POID)
        {
            I_POID = Convert.ToInt32(S_POID);
            //string S_Sql = "select *  from mesProductionOrder where ID='" + S_POID + "'";
            DT_ProductionOrder = PartSelectSVC.GetProductionOrder(S_POID).Tables[0];   //public_.P_DataSet(S_Sql).Tables[0];
            //mesProductionOrder v_mesProductionOrder= mesProductionOrderSVC.Get(Convert.ToInt32(S_POID));

            Edt_OrderQuantity.Text = DT_ProductionOrder.Rows[0]["OrderQuantity"].ToString();
        }


        string S_UnitID = "";
        string S_PartID = "";
        string S_UnitStateID = "";
        string S_luUnitStateID = "";
        string S_DefectID = "";
        string S_POPartID = "";
        DateTime dateStart;
        private void Edt_ScanSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Public_.Add_Info_MSG(Edt_MSG, "", "Start");
                dateStart = DateTime.Now;  

                 S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                 S_UnitStateID = public_.GetluUnitStatusID(S_PartID, List_Login.StationTypeID, List_Login.LineID.ToString());
                // luUnitStatus  ID
                //DataRowView DRV_luUnitStatus = Com_luUnitStatus.SelectedItem as DataRowView;
                S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();     //DRV_luUnitStatus["ID"].ToString().Trim();

                S_DefectID = Edt_DefectID.Text.Trim();
                string S_SN = Edt_SN.Text.Trim();
                //工单  PartID
                S_POPartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();

                if (S_SN == "")
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " SN不能为空!", "NG");
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }

                if (S_luUnitStateID != "1")
                {
                    if (S_DefectID == "")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, S_SN + " 确认此物料NG,请设置NG原因!", "NG");
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }
                }


                 int I_FirstScanSequence = FirstScanSequence(S_POPartID, List_Login.StationTypeID);

    //TimeSpan ts = DateTime.Now - dateStart;
    //double mill = Math.Round(ts.TotalMilliseconds, 0);
    //Public_.Add_Info_MSG(Edt_MSG, S_SN + " " + mill + "毫秒 1", "OK");
                //if (Panel_Part.Visible == false)
                {
                    List<string> List_PO = public_.SnToPOID(S_SN);
                    string S_POID = List_PO[0];

                    if (S_POID.Length > 4)
                    {
                        if (S_POID.Substring(0, 5) == "ERROR")
                        {
                            Public_.Add_Info_MSG(Edt_MSG, S_SN + " " + S_POID, "NG");
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                            return;
                        }
                    }

                    if (S_POID == "" || S_POID == "0")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码没有第一次过站记录！", "NG");
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    };

                    //根据  SN  获取工单  料号
                    //string S_SqlPO = "select *  from mesProductionOrder where ID='" + S_POID + "'";
                    //DataTable DT_PO = public_.P_DataSet(S_SqlPO).Tables[0];
                    mesProductionOrder v_mesProductionOrder=  mesProductionOrderSVC.Get(Convert.ToInt32(S_POID));

                    S_POPartID = v_mesProductionOrder.PartID.ToString();   //DT_PO.Rows[0]["PartID"].ToString();
                    I_FirstScanSequence = FirstScanSequence(S_POPartID, List_Login.StationTypeID);
                    //public_.AddPOAll(Com_PO, S_POPartID, List_Login.LineID.ToString(),Grid_PO);
                    //Com_PO.Text = S_POID;

                    string S_ComPO = Com_PO.EditValue.ToString();
                    if (S_POID != S_ComPO)
                    {
                        Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码和选择的工单不一致！", "NG");
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }

                }

                //string S_Sql = "select * from mesSerialNumber where Value ='" + S_SN + "'";
                string S_Sql = "";
                DataTable DT_SN = PartSelectSVC.GetmesSerialNumber(S_SN).Tables[0]; //public_.P_DataSet(S_Sql).Tables[0];  

                if (DT_SN.Rows.Count > 0)
                {
                    S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                    //S_Sql = "select * from mesUnit where ID='" + S_UnitID + "'";
                    //DataTable DT_Unit= public_.P_DataSet(S_Sql).Tables[0];
                    DataTable DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0]; 
                   
                    string S_StationID = List_Login.StationID.ToString();    //DT_Unit.Rows[0]["StationID"].ToString();

                    string S_SerialNumberType = DT_SN.Rows[0]["SerialNumberTypeID"].ToString();
                    if (S_SerialNumberType != "5")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码类别不匹配！", "NG");
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }

                    if (DT_Unit.Rows[0]["StatusID"].ToString() != "1")                    
                    {
                        Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码已NG！", "NG");
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }


                    // ts = DateTime.Now - dateStart;
                    // mill = Math.Round(ts.TotalMilliseconds, 0);
                    //Public_.Add_Info_MSG(Edt_MSG, S_SN + " " + mill + "毫秒 2", "OK");
                    ////////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////////
                    //Boolean B_IsRoute = GetIsRoute(List_Login.StationID, DT_Unit);
                    //DateTime dateStartC = DateTime.Now; 
                     string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, S_SN);
                    //string S_RouteCheck = "1";
                    //TimeSpan tsC = DateTime.Now - dateStartC;
                    //double millC = Math.Round(tsC.TotalMilliseconds, 0);
                    //Public_.Add_Info_MSG(Edt_MSG, S_SN + " " + millC + "毫秒 GetRouteCheck", "OK");

                    Boolean B_SN_Exist = false; //是否已扫描此条码
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    if (S_RouteCheck == "1")
                    {
                        string S_TimeCheck=PartSelectSVC.TimeCheck(List_Login.StationTypeID.ToString() , S_SN);
                        if (S_TimeCheck != "1")
                        {
                            Public_.Add_Info_MSG(Edt_MSG, S_TimeCheck, "NG");
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
                                Public_.Add_Info_MSG(Edt_MSG, "此条码已扫描！", "NG");
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
                                        timer_OnLine.Enabled = false;////////////////////////////////////// 
                                        if (RunPLC(S_PCLAllowStart) == true)
                                        {
                                            Thread.Sleep(300);
                                            S_PLCResult = SP_Main.ReadExisting();

                                            if (S_PLCResult.Length == 5)//PLC  开机启动成功
                                            {
                                                Public_.Add_Info_MSG(Edt_MSG, "PLC启动成功", "OK");
                                                Btn_Start.BackColor = Color.Lime;

                                                if (RunPLC2(S_PCLRuning) == true)
                                                {
                                                    Public_.Add_Info_MSG(Edt_MSG, "PLC运行中", "OK");
                                                    Btn_Run.BackColor = Color.Lime;
                                                    timer_Run.Enabled = true;/////////////////////////////////// 
                                                }
                                                else
                                                {
                                                    Public_.Add_Info_MSG(Edt_MSG, "PLC运行中失败", "NG");
                                                    Btn_Run.BackColor = Color.Red;
                                                    timer_Run.Enabled = false;////////////////////////////////////                                                   
                                                    return;
                                                }

                                            }
                                            else
                                            {
                                                Public_.Add_Info_MSG(Edt_MSG, "PLC启动失败", "NG");
                                                Btn_Start.BackColor = Color.Red;
                                                timer_Run.Enabled = false;//////////////////////////////////////                                                
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            Public_.Add_Info_MSG(Edt_MSG, "PLC开始失败", "NG");
                                            return;
                                        }
                                    }
                                    else  //////////////////////////////////////////////////////////////////////////////
                                    {
                                        SetFilish();
                                    }

                                }
                                catch
                                {
                                }
                            }                             
                        }
                        else
                        {
                            Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码和工单不匹配", "NG");
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                        }
                    }
                    else
                    {
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        Public_.Add_Info_MSG(Edt_MSG, S_RouteCheck, "NG");
                    }
                }
                else
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " 条码不存在!", "NG");
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                }
            }
        }

        private void CloseDefect()
        {
            try
            {
                foreach (Form frm in Panel_Defact.Controls)
                {
                    frm.Close();
                }
                Edt_DefectID.Text = "";
            }
            catch { }
        }

        private void Btn_Defect_Click(object sender, EventArgs e)
        {
            try
            {
                CloseDefect();

                DefectForm v_DefectForm = new DefectForm();
                //v_DefectForm.Show_DefectForm(v_DefectForm, Edt_DefectID);
                v_DefectForm.Show_DefectFormToPanel(v_DefectForm, Edt_DefectID, Convert.ToInt32(S_DefectTypeID));

                v_DefectForm.FormBorderStyle = FormBorderStyle.None;
                v_DefectForm.Width = Panel_Defact.Width;
                v_DefectForm.Height = Panel_Defact.Height;
                v_DefectForm.TopLevel = false;
                v_DefectForm.Parent = Panel_Defact;
                v_DefectForm.BringToFront();
                // v_DefectForm.Tag = F_LoginList;
                v_DefectForm.Show();
            }
            catch
            { }
        }

        private void OverStation_Form_Resize(object sender, EventArgs e)
        {
            try
            {
                foreach (Form frm in Panel_Defact.Controls)
                {
                    frm.Width = Panel_Defact.Width;
                    frm.Height = Panel_Defact.Height;
                }
            }
            catch { }
        }

        private void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            if (Com_PO.Text.Trim() == "")
            {
                Public_.Add_Info_MSG(Edt_MSG, "工单不能为空!", "NG");
            }
            else
            {
                if (S_PLCControl == "1")
                {
                    if (Btn_OnLine.BackColor == Color.Gray)
                    {
                        Public_.Add_Info_MSG(Edt_MSG, "PLC离线,连接设备失败!", "NG");
                        return;
                    }
                }

                if (Btn_OnLine.BackColor == Color.Red)
                {
                    Public_.Add_Info_MSG(Edt_MSG, "PLC离线,连接设备失败!", "NG");
                    return;
                }
                PartEdtStatus(false);
            }
        }

        private void Btn_Unlock_Click(object sender, EventArgs e)
        {
            PartEdtStatus(true);
        }

        private void Com_PartFamilyType_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartFamilyTypeID = Com_PartFamilyType.EditValue.ToString();
                public_.AddPartFamily(Com_PartFamily, S_PartFamilyTypeID, Grid_PartFamily);

                S_DefectTypeID = public_.GetDefectTypeID(Com_PartFamilyType.Text.Trim());
            }
            catch
            { }
        }

        private void Com_PartFamily_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                public_.AddPart(Com_Part, S_PartFamilyID, Grid_Part);
            }
            catch
            { }
        }

        private void Com_Part_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartID = Com_Part.EditValue.ToString();
                public_.AddPOAll(Com_PO, S_PartID, List_Login.LineID.ToString(), Grid_PO);

                GetPLCSeting();
            }
            catch
            { }
        }

        private void Com_PO_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_POID = Com_PO.EditValue.ToString();

                Get_DTPO(S_POID);
            }
            catch
            { }
        }

        private void Com_luUnitStatus_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();
                string S_Description = Com_luUnitStatus.Text.Trim();

                if (S_luUnitStateID != "1")
                {
                    Btn_Defect.Enabled = true;

                    Btn_Defect_Click(sender, e);
                    Lab_NG.Visible = true;
                    Lab_NG.Text = "单元状态：" + S_Description;
                }
                else
                {
                    Btn_Defect.Enabled = false;
                    Lab_NG.Visible = false;

                    CloseDefect();
                }
            }
            catch
            { }
        }

        private void OverStation_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                PartSelectSVC.Close();
                mesUnitSVC.Close();
                mesSerialNumberSVC.Close();
                mesHistorySVC.Close();
                mesUnitComponentSVC.Close();
                mesUnitDefectSVC.Close();
                mesPartSVC.Close();
                mesProductionOrderSVC.Close();
            }
            catch
            { }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            double dd= PartSelectSVC.MyTest();
            Public_.Add_Info_MSG(Edt_MSG, " " + dd + "毫秒 GetRouteCheck", "OK");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime dateStartC = DateTime.Now;

            string S_Sql = "exec GetRouteCheck 21,19,11,186,102,19,'CHINA  GG2ZN05WP3MT',''";
            public_.P_DataSet(S_Sql);

            TimeSpan tsC = DateTime.Now - dateStartC;
            double millC = Math.Round(tsC.TotalMilliseconds, 0);
            Public_.Add_Info_MSG(Edt_MSG, " " + millC + "毫秒 GetRouteCheck", "OK");
        }

        private void btnRest_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要重置当前计数吗?", "提示",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }
            Edt_ScanOKQuantity.Text = "0";
            Edt_ScanNGQuantity.Text = "0";
            ScanOKQuantity = 0;
            ScanNGQuantity = 0;
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
                    Public_.Add_Info_MSG(Edt_MSG, ex.ToString(), "NG");
                    //MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    //MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Public_.Add_Info_MSG(Edt_MSG, ex.ToString(), "NG");
                }
            }
            else
            {
                //MessageBox.Show("当前计算机上不存在串口，请检查！", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Public_.Add_Info_MSG(Edt_MSG, "当前计算机上不存在串口，请检查", "NG");
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
                    Public_.Add_Info_MSG(Edt_MSG, ex.ToString(), "NG");
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
                    Public_.Add_Info_MSG(Edt_MSG, ex.ToString(), "NG");
                }
            }
            else
            {
                Public_.Add_Info_MSG(Edt_MSG, "当前计算机上不存在串口，请检查", "NG");
                timer_OnLine.Enabled = false;
                Btn_OnLine.BackColor = Color.Black;

            }
            return B_Result;
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
                //if (RunPLC(S_PLCOnLineStatus) == false)
                //{
                //    timer_OnLine.Enabled = false;
                //}
                //Thread.Sleep(200);

                //string S_ReadValue = SP_Main.ReadExisting();
                //if (S_ReadValue.Length == 5)
                //{
                //    Btn_OnLine.BackColor = Color.Lime; 
                //}
                //else
                //{
                //    Btn_OnLine.BackColor = Color.Red;
                //}

                string S_ReadValue = SP_Main.ReadExisting();
                textBox1.Text += S_ReadValue+"\r\n";

            }
            catch (Exception ex)
            {              
            }
        }

        int I_RunTime = 0;
        private void timer_Run_Tick(object sender, EventArgs e)
        {
            try
            {
                if (I_RunTime >= I_PLCTimeout)
                {
                    Public_.Add_Info_MSG(Edt_MSG, "PLC运行中已超时", "NG");
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
                if (S_ReadValue.Length == 7 && S_ReadValue.Substring(6,1)=="1" ) //运行中完成
                {
                    timer_Run.Enabled = false;
                    I_RunTime = 0;

                    if (RunPLC2(S_PLCRunEnd) == true)
                    {
                        Public_.Add_Info_MSG(Edt_MSG, "PLC运行完成成功", "OK");
                        Btn_End.BackColor = Color.Lime;
                        timer_Filish.Enabled = true;/////////////////////////////////// 
                    }
                    else
                    {
                        Public_.Add_Info_MSG(Edt_MSG, "PLC运行完成失败", "NG");
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

            }
        }


        int I_RunEndTime = 0;
        private void timer_Filish_Tick(object sender, EventArgs e)
        {
            try
            {
                if (I_RunEndTime >= I_PLCTimeout)
                {
                    Public_.Add_Info_MSG(Edt_MSG, "PLC运行结束已超时", "NG");
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
                Public_.Add_Info_MSG(Edt_MSG, " "+ex.ToString(), "NG");
                I_RunEndTime = 0;
            }
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
                        Public_.Add_Info_MSG(Edt_MSG, S_UpdateUnit, "NG");
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

                        //ts = DateTime.Now - dateStart;
                        //mill = Math.Round(ts.TotalMilliseconds, 0);
                        //Public_.Add_Info_MSG(Edt_MSG, S_SN + " " + mill + "毫秒 3", "OK");

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
                                catch
                                {
                                    Public_.Add_Info_MSG(Edt_MSG, S_mesUnitDefectInsert, "NG");
                                }
                            }
                        }

                        TimeSpan ts = DateTime.Now - dateStart;
                        double mill = Math.Round(ts.TotalMilliseconds, 0);
                        Public_.Add_Info_MSG(Edt_MSG, S_SN + " " + mill + "毫秒 OK！", "OK");
                        /////////////////////////////////////////////////////////////////////////
                        /////////////////////////////////////////////////////////////////////////
                        if (S_luUnitStateID == "1")
                        {
                            ScanOKQuantity++;
                            Edt_ScanOKQuantity.Text = ScanOKQuantity.ToString();
                        }
                        else
                        {
                            ScanNGQuantity++;
                            Edt_ScanNGQuantity.Text = ScanNGQuantity.ToString();
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
                Public_.Add_Info_MSG(Edt_MSG, " " + ex.ToString(), "NG");
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
                string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, List_Login.StationTypeID, List_Login.LineID.ToString());

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

    }
}
