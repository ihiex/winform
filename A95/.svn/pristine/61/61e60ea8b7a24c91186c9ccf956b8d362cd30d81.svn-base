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
using App.MyMES.mesMachineService;
using System.Configuration;
using System.Threading;
using Microsoft.VisualBasic.Devices;

namespace App.MyMES
{
    public partial class ToolingQCPrint : DevExpress.XtraEditors.XtraForm
    {
        Public_ public_ = new Public_();
        DataTable DT_ProductionOrder = new DataTable();
        DataTable DT_ProductStructure = new DataTable();
        DataTable DT_PrintSn = new DataTable();
        LabelManager2.Application LabSN;

        LoginList List_Login = new LoginList();

        PartSelectSVCClient PartSelectSVC;
        ImesUnitSVCClient mesUnitSVC;
        ImesSerialNumberSVCClient mesSerialNumberSVC;
        ImesHistorySVCClient mesHistorySVC;
        ImesUnitComponentSVCClient mesUnitComponentSVC;
        ImesUnitDefectSVCClient mesUnitDefectSVC;
        ImesPartSVCClient mesPartSVC;
        ImesProductionOrderSVCClient mesProductionOrderSVC;
        ImesMachineSVCClient mesMachineSVC;

        int I_RouteSequence = -9;  //当前工序顺序 
        int ScanOKQuantity = 0;
        int ScanNGQuantity = 0;
        int I_POID = 0;
        string S_DefectTypeID = "";
        string S_LabelName = "";
        string S_Print_TemplateType = "1";    //1 :CodeSoft  2：BarTender

        public ToolingQCPrint()
        {
            InitializeComponent();
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
                mesMachineSVC.Close();
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
            mesMachineSVC = ImesMachineFactory.CreateServerClient();

            List_Login = this.Tag as LoginList;
            ///////////////////////////
            public_.AddStationType(Com_StationType, Grid_StationType);
            public_.AddLine(Com_Line, Grid_Line);
            //public_.AddmesUnitState(Com_UnitState);

            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_LineID = List_Login.LineID.ToString();
            public_.AddStation(Com_Station, S_LineID, Grid_Station);

            Com_StationType.Text = S_StationTypeID;
            Com_Line.Text = S_LineID;
            Com_Station.Text = List_Login.StationID.ToString();

            public_.AddPartFamilyType(Com_PartFamilyType, Grid_PartFamilyType);
            public_.AddluUnitStatus(Com_luUnitStatus, Grid_luUnitStatus);

            //DataRowView DRV = Com_Part.SelectedItem as DataRowView;
            string S_PartID = Com_Part.EditValue.ToString();   //DRV["ID"].ToString();
            public_.AddPOAll(Com_PO, S_PartID, List_Login.LineID.ToString(), Grid_PO);
            Com_PO.Text = "";

            Edt_SN.Focus();
            Edt_SN.SelectAll();

            //Edt_MSG.Text = ""; 
            Edt_SN.Text = "";
            Edt_SN.Enabled = true;

            Edt_ChildSN.Text = "";
            Edt_ChildSN.Enabled = false;
            List_ChildSN.Items.Clear();

            PartEdtStatus(true);
            Get_TempletPath(true);

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

            if (B_Status == true)
            {
                Com_PartFamilyType.BackColor = Color.White;
                Com_PartFamily.BackColor = Color.White;
                Com_Part.BackColor = Color.White;
                Com_PO.BackColor = Color.White;

                Edt_SN.Enabled = false;
            }
            else
            {
                Com_PartFamilyType.BackColor = Color.Yellow;
                Com_PartFamily.BackColor = Color.Yellow;
                Com_Part.BackColor = Color.Yellow;
                Com_PO.BackColor = Color.Yellow;

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

            //string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
            //I_RouteSequence = public_.RouteSequence(S_PartID, List_Login.StationTypeID);
            //int I_FirstScanSequence = FirstScanSequence(S_PartID, List_Login.StationTypeID);

            //public_.AddmesUnitState(Com_UnitState, S_PartID, I_RouteSequence.ToString());

            Com_PO.Enabled = true;
            //if (I_RouteSequence > I_FirstScanSequence)

            //if (IsFirstSequence()==false)
            {
                //Com_PO.Enabled = false;
                //Com_PO.BackColor = Color.Yellow;

                //Edt_SN.Enabled = true;

                //Btn_Refresh.Enabled = false;
                //Btn_ConfirmPO.Enabled = false;

                //Panel_Part.Visible = false;               
            }
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
            GetScanQuantity();
        }



        private void GetScanQuantity()
        {
            //string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
            //string S_Sql = "select COUNT(ID) CountUnit  from  mesUnit  where PartID=" + S_PartID +
            //                "  and StationID=" + List_Login.StationID +
            //                "  and ProductionOrderID=" + I_POID;
            //DataTable DT_CountUnit = public_.P_DataSet(S_Sql).Tables[0];

            //Edt_ScanQuantity.Text = DT_CountUnit.Rows[0]["CountUnit"].ToString();  
        }

        string S_UnitID = "";
        private void Edt_ScanSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //Public_.Add_Info_MSG(Edt_MSG, "", "Start");
                DateTime dateStart = DateTime.Now;

                string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, List_Login.StationTypeID, List_Login.LineID.ToString());
                // luUnitStatus  ID
                //DataRowView DRV_luUnitStatus = Com_luUnitStatus.SelectedItem as DataRowView;
                string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();     //DRV_luUnitStatus["ID"].ToString().Trim();

                string S_DefectID = Edt_DefectID.Text.Trim();
                string S_SN = Edt_SN.Text.Trim();
                //工单  PartID
                string S_POPartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();

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

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////  QCPrint   NG 打印  /////////////////////////////////////////////
                //判断打印模板信息 和 Buck 获取  SN               
                //if (List_Login.ApplicationType== "QCPrint") //QCPrint
                {
                    try
                    {
                        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        S_Print_TemplateType = config.AppSettings.Settings["Print_TemplateType"].Value.Trim();
                    }
                    catch
                    {
                    }

                    if (string.IsNullOrEmpty(Edt_Templet.Text.Trim().Replace(";", "")))
                    {
                        Public_.Add_Info_MSG(Edt_MSG, "未配置打印文件路径！", "NG");
                        return;
                    }


                    string S_TempletPath = Edt_Templet.Text.Trim();
                    string[] List_TempletPath = S_TempletPath.Split(';');

                    string S_TmpType = List_TempletPath[0].Substring(List_TempletPath[0].Length - 3, 3);
                    if (S_Print_TemplateType == "1" && S_TmpType != "lab")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, "CodeSoft 模板配置错误！", "NG");
                        return;
                    }

                    if (S_Print_TemplateType == "2" && S_TmpType != "btw")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, "BarTender 模板配置错误！", "NG");
                        return;
                    }

                    string S_StationTypeID = List_Login.StationTypeID.ToString();
                    string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                    string S_ComPartID = Com_Part.EditValue.ToString();
                    string S_ProductionOrderID = "";
                    string S_LoginLineID = List_Login.LineID.ToString();

                    S_LabelName = public_.GetLabelName(PartSelectSVC, S_StationTypeID, S_PartFamilyID, S_ComPartID, S_ProductionOrderID, S_LoginLineID);
                    if (S_LabelName == "")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, "此工序没有配置打印标签！", "NG");
                        return;
                    }
                    // Buck 获取  SN
                    string S_FG_SN = PartSelectSVC.BuckToFGSN(S_SN);
                    S_SN = S_FG_SN;
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
                    mesProductionOrder v_mesProductionOrder = mesProductionOrderSVC.Get(Convert.ToInt32(S_POID));

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

                //if (I_RouteSequence > I_FirstScanSequence)  //不是第一个工序时根据  SN  获取工单
                //{
                //    string S_POID = public_.SnToPOID(S_SN);
                //    if (S_POID=="" || S_POID=="0" )
                //    {
                //        Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码没有第一次过站记录！", "NG");
                //        Edt_SN.Focus();
                //        Edt_SN.Text = "";
                //        return;
                //    };                   
                //}


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

                    //TimeSpan tsC = DateTime.Now - dateStartC;
                    //double millC = Math.Round(tsC.TotalMilliseconds, 0);
                    //Public_.Add_Info_MSG(Edt_MSG, S_SN + " " + millC + "毫秒 GetRouteCheck", "OK");

                    if (S_RouteCheck == "1")
                    {
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        /////////////////////////////////////////////////   Interlock      /////////////////////////////////////////////
                        if (List_Login.ApplicationTypeID == 26) //Interlock
                        {

                        }

                        string S_SelectPartID = DT_Unit.Rows[0]["PartID"].ToString();
                        if (S_POPartID == S_SelectPartID)
                        {
                            string S_StationTypeID = List_Login.StationTypeID.ToString();
                            //S_Sql = "select * from mesProductStructure where ParentPartID='" + S_POPartID + "' and StationTypeID='" + S_StationTypeID + "'";

                            DT_ProductStructure = PartSelectSVC.GetmesProductStructure2(S_POPartID, S_StationTypeID).Tables[0];   //public_.P_DataSet(S_Sql).Tables[0];
                            if (DT_ProductStructure.Rows.Count > 0)
                            {
                                Edt_SN.Enabled = false;
                                Edt_ChildSN.Enabled = true;
                                Edt_ChildSN.Focus();
                            }
                            else
                            {
                                //没有子料，这里扫描过站
                                //ImesUnitSVCClient mesUnitSVC = ImesUnitFactory.CreateServerClient();
                                //ImesHistorySVCClient mesHistorySVC = ImesHistoryFactory.CreateServerClient();
                                //ImesUnitDefectSVCClient mesUnitDefectSVC = ImesUnitDefectFactory.CreateServerClient();

                                try
                                {
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

                                        //修改工单状态
                                        string S_PO_StatusID = DT_ProductionOrder.Rows[0]["StatusID"].ToString();
                                        if (S_PO_StatusID == "1")
                                        {
                                            //S_Sql = "Update mesProductionOrder set StatusID=2,LastUpdate='" + DateTime.Now.ToString() +
                                            //        "' where ID='" + S_POID + "'";
                                            //public_.ExecSql(S_Sql);
                                            //PartSelectSVC.ModPO(S_POID); 
                                        }

                                        //ts = DateTime.Now - dateStart;
                                        //mill = Math.Round(ts.TotalMilliseconds, 0);
                                        //Public_.Add_Info_MSG(Edt_MSG, S_SN + " " + mill + "毫秒 3", "OK");

                                        //////////////////////////////////////////////////////////////////////////////
                                        /////////////////////////////////   NG  Print   /////////////////////////////
                                        // NG 后打印出此FG 条码
                                        //if (List_Login.ApplicationTypeID == 1) //QCPrint
                                        {
                                            DT_PrintSn.Columns.Clear();
                                            DT_PrintSn.Columns.Add("SN", typeof(string));
                                            DT_PrintSn.Columns.Add("CreateTime", typeof(string));
                                            DT_PrintSn.Columns.Add("PrintTime", typeof(string));

                                            DataRow DR = DT_PrintSn.NewRow();
                                            DR["SN"] = S_SN;
                                            DT_PrintSn.Rows.Add(DR);

                                            string S_PrintResult = PrintCodeSoftSN(S_LabelName);
                                            if (S_PrintResult != "OK")
                                            {
                                                Public_.Add_Info_MSG(Edt_MSG, "条码打印失败," + S_PrintResult, "NG");
                                            }
                                            else
                                            {
                                                Public_.Add_Info_MSG(Edt_MSG, "条码生成成功并发送至打印机！", "OK");
                                            }
                                        }
                                        //修改状态
                                        mesMachineSVC.MesModMachineBySNStationTypeID(Edt_SN.Text.Trim(), List_Login.StationTypeID);

                                        //////////////////////////  Defect ///////////////////////////////////////////
                                        //////////////////////////////////////////////////////////////////////////////
                                        //string[] Array_Defect = S_DefectID.Split(';');
                                        //if (S_luUnitStateID != "1")
                                        //{
                                        //    foreach (var item in Array_Defect)
                                        //    {
                                        //        string S_mesUnitDefectInsert = "";
                                        //        try
                                        //        {
                                        //            if (item.Trim() != "")
                                        //            {
                                        //                int I_DefectID = Convert.ToInt32(item);
                                        //                mesUnitDefect v_mesUnitDefect = new mesUnitDefect();

                                        //                v_mesUnitDefect.UnitID = v_mesUnit.ID;
                                        //                v_mesUnitDefect.DefectID = I_DefectID;
                                        //                v_mesUnitDefect.StationID = List_Login.StationID;
                                        //                v_mesUnitDefect.EmployeeID = List_Login.EmployeeID;

                                        //                S_mesUnitDefectInsert = mesUnitDefectSVC.Insert(v_mesUnitDefect);
                                        //            }
                                        //        }
                                        //        catch
                                        //        {
                                        //            Public_.Add_Info_MSG(Edt_MSG, S_mesUnitDefectInsert, "NG");
                                        //        }
                                        //    }
                                        //}
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

                                        Edt_SN.Text = "";
                                        Edt_SN.Enabled = true;
                                        Edt_SN.Focus();
                                        //Edt_DefectID.Text = "";

                                        Edt_ChildSN.Text = "";
                                        Edt_ChildSN.Enabled = false;
                                        List_ChildSN.Items.Clear();

                                        GetScanQuantity();
                                        //Public_.Add_Info_MSG(Edt_MSG, S_SN + " OK！", "OK");
                                        TimeSpan ts = DateTime.Now - dateStart;
                                        double mill = Math.Round(ts.TotalMilliseconds, 0);
                                        Public_.Add_Info_MSG(Edt_MSG, S_SN + " " + mill + "毫秒 OK！", "OK");
                                    }
                                }
                                finally
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

        private void Edt_ChildSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //Edt_MSG.Text = "";

                //                ImesUnitSVCClient mesUnitSVC = ImesUnitFactory.CreateServerClient();
                //                ImesSerialNumberSVCClient mesSerialNumberSVC = ImesSerialNumberFactory.CreateServerClient();   
                //                ImesHistorySVCClient mesHistorySVC = ImesHistoryFactory.CreateServerClient();
                //                ImesUnitComponentSVCClient mesUnitComponentSVC = ImesUnitComponentFactory.CreateServerClient();

                DateTime dateStart = DateTime.Now;
                string ScanSN = Edt_SN.Text.ToString();
                string ChildSN = Edt_ChildSN.Text.ToString();
                try
                {
                    //Public_.Add_Info_MSG(Edt_MSG, "", "Start");
                    string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                    string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, List_Login.StationTypeID, List_Login.LineID.ToString());
                    // luUnitStatus  ID
                    //DataRowView DRV_luUnitStatus = Com_luUnitStatus.SelectedItem as DataRowView;
                    string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();   //DRV_luUnitStatus["ID"].ToString().Trim();

                    string S_DefectID = Edt_DefectID.Text.Trim();

                    //工单 PartID
                    string S_POPartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();

                    string S_POID = DT_ProductionOrder.Rows[0]["ID"].ToString();
                    string S_PO_StatusID = DT_ProductionOrder.Rows[0]["StatusID"].ToString();

                    string S_SN = Edt_SN.Text.Trim();
                    string S_Sql = "";

                    if (S_SN != "")
                    {
                        string S_ChildSN = Edt_ChildSN.Text.Trim();
                        if (S_ChildSN == "")
                        {
                            Public_.Add_Info_MSG(Edt_MSG, S_ChildSN + " 子SN不能为空!", "NG");
                            Edt_ChildSN.Focus();
                            Edt_ChildSN.Text = "";
                            return;
                        }


                        DataTable DT_ChildSN = PartSelectSVC.Get_UnitID(S_ChildSN).Tables[0];
                        if (DT_ChildSN.Rows.Count == 0)
                        {
                            Public_.Add_Info_MSG(Edt_MSG, S_ChildSN + " 子条码不存在!", "NG");
                            Edt_ChildSN.Focus();
                            Edt_ChildSN.Text = "";
                            return;
                        }

                        if (DT_ChildSN.Rows[0]["StatusID"].ToString() != "1")
                        {
                            Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此子条码已NG！", "NG");
                            Edt_ChildSN.Focus();
                            Edt_ChildSN.Text = "";
                            return;
                        }



                        string S_ChildPartID = DT_ChildSN.Rows[0]["PartID"].ToString();

                        Boolean B_ChildSN_Exist = false; //是否已扫描此条码
                        Boolean B_PartID_Exist = false;  //是否扫描同类条码

                        for (int i = 0; i < List_ChildSN.Items.Count; i++)
                        {
                            string S_ListSN = List_ChildSN.Items[i].ToString().Trim();
                            if (S_ChildSN == S_ListSN)
                            {
                                B_ChildSN_Exist = true;
                            }

                            DataTable DT_List_ChildSN = PartSelectSVC.Get_UnitID(S_SN).Tables[0];
                            string S_List_ChildPartID = DT_List_ChildSN.Rows[0]["PartID"].ToString();
                            if (S_ChildPartID == S_List_ChildPartID)
                            {
                                B_PartID_Exist = true;
                            }
                        }

                        //子料数量
                        string S_StationTypeID = List_Login.StationTypeID.ToString();
                        //S_Sql = "select * from mesProductStructure where ParentPartID='" + S_POPartID + "'" + " and StationTypeID='" + S_StationTypeID + "'";
                        string S_StationID = List_Login.StationID.ToString();
                        //DataTable DT_ComponentCount = public_.P_DataSet(S_Sql).Tables[0];
                        int I_ComponentCount = PartSelectSVC.GetComponentCount(S_POPartID, S_StationTypeID);   //DT_ComponentCount.Rows.Count;

                        if (B_ChildSN_Exist == false)
                        {
                            if (B_PartID_Exist == true)
                            {
                                Public_.Add_Info_MSG(Edt_MSG, S_ChildSN + " 子料已扫描此类别物料!", "NG");
                                Edt_ChildSN.Focus();
                                Edt_ChildSN.Text = "";
                            }
                            else
                            {
                                if (DT_ChildSN.Rows.Count > 0)
                                {
                                    string S_ChildUnitID = DT_ChildSN.Rows[0]["UnitID"].ToString();
                                    mesUnit vT_Unit = mesUnitSVC.Get(Convert.ToInt32(S_ChildUnitID));
                                    S_StationID = vT_Unit.StationID.ToString();

                                    //S_Sql = "select * from mesUnit where ID='" + S_ChildUnitID + "'";
                                    //DataTable DT_Unit = public_.P_DataSet(S_Sql).Tables[0];
                                    //S_StationID = DT_Unit.Rows[0]["StationID"].ToString();

                                    if (S_StationID == List_Login.StationID.ToString())
                                    {
                                        Public_.Add_Info_MSG(Edt_MSG, S_ChildSN + " 子料此条码已过站!", "NG");
                                        Edt_ChildSN.Focus();
                                        Edt_ChildSN.Text = "";
                                    }
                                    else
                                    {

                                        //S_Sql = "select * from mesUnitComponent where UnitID='" + S_UnitID + "' and ChildUnitID='" + S_ChildUnitID + "'";
                                        //DataTable DT_mesUnitComponent = public_.P_DataSet(S_Sql).Tables[0];
                                        DataTable DT_mesUnitComponent = PartSelectSVC.GetmesUnitComponent(S_UnitID, S_ChildUnitID).Tables[0];

                                        if (DT_mesUnitComponent.Rows.Count == 0)
                                        {

                                            S_StationID = List_Login.StationID.ToString();

                                            //S_Sql = "select * from mesProductStructure where ParentPartID='" + S_POPartID + "' and PartID='" + S_ChildPartID + "'" +
                                            // " and StationTypeID = '" + S_StationTypeID + "'";
                                            //DT_ProductStructure = public_.P_DataSet(S_Sql).Tables[0];
                                            DT_ProductStructure = PartSelectSVC.GetmesProductStructure(S_POPartID, S_ChildPartID, S_StationTypeID).Tables[0];

                                            if (DT_ProductStructure.Rows.Count > 0)
                                            {
                                                //判断子料是否有流程扫描过， 如果是判断资料是否扫描完毕                    
                                                //S_Sql = @"select top 1 * from 
                                                //    (
                                                //     select A.*,B.StationID,
                                                //       B.PartID,B.ProductionOrderID,
                                                //       C.Description Station,C.StationTypeID,
                                                //       E.StationTypeID StationTypeID_RouteDetail,
                                                //       E.Sequence           
                                                //     from 
                                                //      (select *  from mesSerialNumber where Value  ='" + S_ChildSN + @"')A   
                                                //      join (select ID,StationID,PartID,ProductionOrderID from  mesUnit) B on A.UnitID=B.ID
                                                //      join (select ID,Description,StationTypeID from mesStation) C on B.StationID=C.ID
                                                //      join mesRouteMap D on D.PartID=B.PartID 
                                                //      join mesRouteDetail E on E.RouteID=D.RouteID

                                                //    )AA order by AA.Sequence desc";

                                                DataTable DT_ChildScanLast = new DataTable();
                                                DT_ChildScanLast = PartSelectSVC.GetChildScanLast(S_ChildSN).Tables[0];

                                                try
                                                {
                                                    //DT_ChildScanLast = public_.P_DataSet(S_Sql).Tables[0];
                                                    string S_StationTypeID_LastScan = DT_ChildScanLast.Rows[0]["StationTypeID"].ToString();
                                                    string S_StationTypeID_RouteDetail_LastScan = DT_ChildScanLast.Rows[0]["StationTypeID_RouteDetail"].ToString();
                                                    //string S_StationID_LastScan = DT_ChildScanLast.Rows[0]["StationID"].ToString();
                                                    //string S_StationID_RouteDetail_LastScan = DT_ChildScanLast.Rows[0]["StationID_RouteDetail"].ToString();

                                                    if (S_StationTypeID_LastScan != S_StationTypeID_RouteDetail_LastScan)
                                                    {
                                                        Public_.Add_Info_MSG(Edt_MSG, S_ChildSN + " 子SN在其他工序没有扫描完成!", "NG");
                                                        Edt_ChildSN.Text = "";
                                                        Edt_ChildSN.Focus();
                                                        return;
                                                    }
                                                }
                                                catch
                                                { }


                                                ///////////////////////////////////////////////////////////////////////////////
                                                ///////////////////////////////////////////////////////////////////////////////
                                                List_ChildSN.Items.Add(S_ChildSN);
                                                Edt_ChildSN.Text = "";
                                                //子料扫完后处理
                                                if (I_ComponentCount == List_ChildSN.Items.Count)
                                                {
                                                    DataTable DT_UPUnitID = PartSelectSVC.Get_UnitID(S_SN).Tables[0];
                                                    //string S_StationID = List_Login.StationID.ToString();

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
                                                        //Edt_ChildSN.Text = "";
                                                        //Edt_ChildSN.Focus();
                                                        return;
                                                    }

                                                    //修改工单状态
                                                    if (S_PO_StatusID == "1")
                                                    {
                                                        //S_Sql = "Update mesProductionOrder set StatusID=2,LastUpdate='" + DateTime.Now.ToString() +
                                                        //        "' where ID='" + S_POID + "'";
                                                        //public_.ExecSql(S_Sql);
                                                        //PartSelectSVC.ModPO(S_POID);
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
                                                    }
                                                    //////////////////////////////////// ChildSN /////////////////////////////////////////
                                                    //////////////////////////////////// ChildSN /////////////////////////////////////////
                                                    for (int i = 0; i < List_ChildSN.Items.Count; i++)
                                                    {
                                                        string S_DoChildSN = List_ChildSN.Items[i].ToString();
                                                        DataTable DT_UPUnitID_Child = PartSelectSVC.Get_UnitID(S_ChildSN).Tables[0];
                                                        int I_PartID_Child = Convert.ToInt32(DT_UPUnitID_Child.Rows[0]["PartID"].ToString());

                                                        //DataTable DT_Part_Child = public_.P_DataSet("select * from mesPart where ID='" +
                                                        //                          I_PartID_Child.ToString() + "'").Tables[0];
                                                        //string S_PartFamilyID = DT_Part_Child.Rows[0]["PartFamilyID"].ToString();
                                                        mesPart v_mesPart = mesPartSVC.Get(I_PartID_Child);
                                                        string S_PartFamilyID = v_mesPart.PartFamilyID.ToString();

                                                        mesUnit v_mesUnit_Child = new mesUnit();
                                                        v_mesUnit_Child.ID = Convert.ToInt32(DT_UPUnitID_Child.Rows[0]["UnitID"].ToString());
                                                        v_mesUnit_Child.UnitStateID = Convert.ToInt32(S_UnitStateID);
                                                        v_mesUnit_Child.StatusID = Convert.ToInt32(S_luUnitStateID);
                                                        v_mesUnit_Child.StationID = List_Login.StationID;
                                                        v_mesUnit_Child.EmployeeID = List_Login.EmployeeID;
                                                        v_mesUnit_Child.LastUpdate = DateTime.Now;
                                                        v_mesUnit_Child.ProductionOrderID = Convert.ToInt32(S_POID);
                                                        string S_UpdateUnit_Child = mesUnitSVC.Update(v_mesUnit_Child);

                                                        if (S_UpdateUnit_Child.Substring(0, 1) != "E")
                                                        {
                                                            mesHistory v_mesHistory = new mesHistory();
                                                            v_mesHistory.UnitID = Convert.ToInt32(v_mesUnit_Child.ID);
                                                            v_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                                                            v_mesHistory.EmployeeID = List_Login.EmployeeID;
                                                            v_mesHistory.StationID = List_Login.StationID;
                                                            v_mesHistory.EnterTime = DateTime.Now;
                                                            v_mesHistory.ExitTime = DateTime.Now;
                                                            v_mesHistory.ProductionOrderID = Convert.ToInt32(S_POID);
                                                            v_mesHistory.PartID = I_PartID_Child;
                                                            v_mesHistory.LooperCount = 1;
                                                            int I_InsertHistory = mesHistorySVC.Insert(v_mesHistory);

                                                            //////////////////////////////////////////////////////////////////////////////////

                                                            mesUnitComponent v_mesUnitComponent = new mesUnitComponent();
                                                            v_mesUnitComponent.UnitID = v_mesUnit.ID;
                                                            v_mesUnitComponent.UnitComponentTypeID = 1;
                                                            v_mesUnitComponent.ChildUnitID = v_mesUnit_Child.ID;
                                                            v_mesUnitComponent.ChildSerialNumber = DT_UPUnitID_Child.Rows[0]["Value"].ToString();

                                                            v_mesUnitComponent.ChildLotNumber = "";
                                                            v_mesUnitComponent.ChildPartID = I_PartID_Child;
                                                            v_mesUnitComponent.ChildPartFamilyID = Convert.ToInt32(S_PartFamilyID);
                                                            v_mesUnitComponent.Position = "";

                                                            v_mesUnitComponent.InsertedTime = DateTime.Now;
                                                            v_mesUnitComponent.StatusID = 1;
                                                            v_mesUnitComponent.LastUpdate = DateTime.Now;

                                                            string S_UC_Result = mesUnitComponentSVC.Insert(v_mesUnitComponent);

                                                            if (S_UC_Result.Substring(0, 1) == "E")
                                                            {
                                                                //MessageBox.Show(S_UC_Result, "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                                                                Public_.Add_Info_MSG(Edt_MSG, S_UC_Result, "NG");
                                                                //Edt_ChildSN.Text = "";
                                                                //Edt_ChildSN.Focus();
                                                                return;
                                                            }
                                                        }
                                                    }

                                                    //////////////////////////  Defect ///////////////////////////////////////////
                                                    //////////////////////////////////////////////////////////////////////////////
                                                    string[] Array_Defect = S_DefectID.Split(';');
                                                    // ImesUnitDefectSVCClient mesUnitDefectSVC = ImesUnitDefectFactory.CreateServerClient();
                                                    if (S_UnitStateID != "0")
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
                                                                Public_.Add_Info_MSG(Edt_MSG, S_ChildSN + " " + S_mesUnitDefectInsert, "NG");
                                                            }
                                                        }
                                                    }
                                                    /////////////////////////////////////////////////////////////////////////
                                                    /////////////////////////////////////////////////////////////////////////
                                                    string S_ListChildSN = "";
                                                    for (int i = 0; i < List_ChildSN.Items.Count; i++)
                                                    {
                                                        S_ListChildSN += List_ChildSN.Items[i].ToString() + "  ";
                                                    }

                                                    TimeSpan ts = DateTime.Now - dateStart;
                                                    double mill = Math.Round(ts.TotalMilliseconds, 0);
                                                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " " + S_ListChildSN + " " + mill + "毫秒 OK！", "OK");


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

                                                    Edt_SN.Text = "";
                                                    Edt_SN.Enabled = true;
                                                    Edt_SN.Focus();
                                                    //Edt_DefectID.Text = "";

                                                    Edt_ChildSN.Text = "";
                                                    Edt_ChildSN.Enabled = false;
                                                    List_ChildSN.Items.Clear();

                                                    GetScanQuantity();
                                                }
                                            }
                                            else
                                            {
                                                Public_.Add_Info_MSG(Edt_MSG, S_ChildSN + " 此条码和工单不匹配!", "NG");
                                                Edt_ChildSN.Text = "";
                                                Edt_ChildSN.Focus();
                                            }
                                        }
                                        else
                                        {
                                            Public_.Add_Info_MSG(Edt_MSG, S_ChildSN + " 条码不存在!", "NG");
                                            Edt_ChildSN.Text = "";
                                            Edt_ChildSN.Focus();
                                        }
                                    }
                                }
                                else
                                {
                                    Public_.Add_Info_MSG(Edt_MSG, S_ChildSN + " 此条码已绑定其他物料!", "NG");
                                    Edt_ChildSN.Text = "";
                                    Edt_ChildSN.Focus();
                                }
                            }
                        }
                        else
                        {
                            Public_.Add_Info_MSG(Edt_MSG, S_ChildSN + " 重复扫描!", "NG");
                            Edt_ChildSN.Text = "";
                            Edt_ChildSN.Focus();
                        }
                    }
                    else
                    {
                        Public_.Add_Info_MSG(Edt_MSG, "主条码不能为空!", "NG");
                    }
                }
                finally
                {
                    string OpenLogFile = System.Configuration.ConfigurationManager.AppSettings["OpenLogFile"];
                    if (OpenLogFile == "Y")
                    {
                        TimeSpan ts = DateTime.Now - dateStart;
                        double mill = ts.TotalMilliseconds;
                        if (Edt_MSG.ForeColor == Color.Green)
                        {
                            LogNet.LogDug(this.Name, "工位:" + List_Login.StationType + ",主SN:" + ScanSN + ",子SN:" +
                                                    ChildSN + ",用时：" + mill.ToString() + "毫秒");
                        }
                        else
                        {
                            LogNet.LogEor(this.Name, "工位:" + List_Login.StationType + ",主SN:" + ScanSN + ",子SN:" +
                                                    ChildSN + ",用时：" + mill.ToString() + "毫秒");
                        }
                    }
                    //mesUnitSVC.Close(); 
                    //mesSerialNumberSVC.Close();
                    //mesHistorySVC.Close();
                    //mesUnitComponentSVC.Close();
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
                PartEdtStatus(false);
            }
        }

        private void Btn_Unlock_Click(object sender, EventArgs e)
        {
            PartEdtStatus(true);
            //if (IsFirstSequence() == true)
            //{
            //    PartEdtStatus(true);
            //}
            //else
            //{
            //    Edt_SN.Enabled = true;
            //    Edt_SN.Text = "";

            //    Edt_ChildSN.Enabled = false;  
            //    Edt_ChildSN.Text = ""; 
            //}
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

                Get_TempletPath(false);
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
                mesMachineSVC.Close();
            }
            catch
            { }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            double dd = PartSelectSVC.MyTest();
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

        private void Get_TempletPath(Boolean IsMsg)
        {
            try
            {
                string S_StationTypeID = List_Login.StationTypeID.ToString();
                string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                string S_PartID = Com_Part.EditValue.ToString();
                string S_ProductionOrderID = "";
                string S_LoginLineID = List_Login.LineID.ToString();

                string S_LabelPath = public_.GetLabelPath(PartSelectSVC, S_StationTypeID, S_PartFamilyID, S_PartID, S_ProductionOrderID, S_LoginLineID);
                if (S_LabelPath == "")
                {
                    if (IsMsg == true)
                    {
                        Public_.Add_Info_MSG(Edt_MSG, "未配置打印模板!", "NG");
                    }
                }
                else
                {
                    Edt_Templet.Text = S_LabelPath;
                    if (S_LabelPath.Substring(0, 5) == "ERROR")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, S_LabelPath, "NG");
                        Edt_Templet.Text = "";
                    }
                }
            }
            catch
            { }
        }


        private string PrintCodeSoftSN(string S_LabelName)
        {
            string S_Result = "OK";
            if (DT_PrintSn == null || DT_PrintSn.Rows.Count == 0)
            {
                S_Result = "没有条码可以打印";
                return S_Result;
            }
            int I_PartID = Convert.ToInt32(Com_Part.EditValue.ToString());


            if (S_Print_TemplateType == "1")
            {
                try
                {
                    if (LabSN == null)
                    {
                        LabSN = new LabelManager2.Application();
                    }

                    string[] List_LabelName = S_LabelName.Split(';');
                    foreach (var item in List_LabelName)
                    {
                        if (item != "")
                        {
                            MESLabel.MESLabel.PrintLabel("", PartSelectSVC, LabSN, DT_PrintSn, Panel_Defact,
                                                        item, false, false, "", "", false, "");
                        }
                    }
                }
                catch (Exception ex)
                {
                    S_Result = ex.Message;
                }
            }
            else if (S_Print_TemplateType == "2")
            {
                DT_PrintSn.Columns.Add("TmpPath");
                DT_PrintSn.Columns.Add("PartID");

                int I_ColCount = DT_PrintSn.Columns.Count;
                DT_PrintSn.Rows[0]["TmpPath"] = Edt_Templet.Text.Trim();
                DT_PrintSn.Rows[0]["PartID"] = I_PartID;

                string[] List_LabelName = S_LabelName.Split(';');
                string[] List_Templet = Edt_Templet.Text.Trim().Split(';');

                for (int i = 0; i < List_LabelName.Length; i++)
                {
                    if (List_LabelName[i] != "")
                    {
                        DT_PrintSn.Rows[0]["TmpPath"] = List_Templet[i];
                        MESLabel.MESLabel.PrintLabel("", PartSelectSVC, LabSN, DT_PrintSn, Panel_Defact,
                                                List_LabelName[i], false, false, "", "", false, "");
                        Thread.Sleep(500);
                    }
                }
            }
            else
            {
                S_Result = "ERROR:打印软件配置错误！";
            }

            return S_Result;
        }

    }
}
