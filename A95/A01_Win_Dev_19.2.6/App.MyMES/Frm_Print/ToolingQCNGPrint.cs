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
    public partial class ToolingQCNGPrint : FrmBase
    {
        DataTable DT_PrintSn = new DataTable();
        LabelManager2.Application LabSN;
        string S_LabelName = "";
        string S_UnitID = "";

        public ToolingQCNGPrint()
        {
            InitializeComponent();
        }

        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            base.Btn_ConfirmPO_Click(sender, e);
            base.Btn_Defect.Enabled = true;
            base.Com_luUnitStatus.Enabled = true;
            Edt_SN.Enabled = true;
            Edt_SN.Focus();
        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            List_ChildSN.Items.Clear();
            Get_TempletPath(true);
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
            if (public_.IsOneStationPrint(PartID, PartFamilyID, StationTypeID, 
                List_Login.LineID.ToString(), Com_PO.EditValue.ToString()))
            {
                I_Result = 2;
            }
            return I_Result;
        }

        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DateTime dateStart = DateTime.Now;

                string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                string PartFamilyID = Com_PartFamily.EditValue.ToString();
                string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID, 
                    List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());
                if (string.IsNullOrEmpty(S_UnitStateID))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }
                // luUnitStatus  ID
                //DataRowView DRV_luUnitStatus = Com_luUnitStatus.SelectedItem as DataRowView;
                string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();     //DRV_luUnitStatus["ID"].ToString().Trim();

                string S_DefectID = base.DefectChar;
                string S_SN = Edt_SN.Text.Trim();
                //工单  PartID
                string S_POPartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();

                if (S_SN == "")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
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

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////  QCPrint   NG 打印  /////////////////////////////////////////////
                //判断打印模板信息 和 Buck 获取  SN               
                //if (List_Login.ApplicationType== "QCPrint") //QCPrint
                {
                    
                    if (string.IsNullOrEmpty(Edt_Templet.Text.Trim().Replace(";", "")))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20098", "NG", List_Login.Language);
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
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20104", "NG", List_Login.Language);
                        return;
                    }
                    // Buck 获取  SN
                    string S_FG_SN = PartSelectSVC.BuckToFGSN(S_SN);
                    S_SN = S_FG_SN;
                }


                int I_FirstScanSequence = FirstScanSequence(S_POPartID, List_Login.StationTypeID);

                {
                    List<string> List_PO = public_.SnToPOID(S_SN);
                    string S_POID = List_PO[0];

                    if (S_POID.Length > 4)
                    {
                        if (S_POID.Substring(0, 5) == "ERROR")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(S_POID.Replace("ERROR", ""), List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, S_POID);
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
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20015", "NG", List_Login.Language,new string[] { S_SN });
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

                    ////////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////////
                    string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, S_SN);

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
                                                catch(Exception ex )
                                                {
                                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                                }
                                            }

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

                                                string S_PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, S_LabelName, 
                                                                        DT_PrintSn, Convert.ToInt32(Com_Part.EditValue.ToString()));
                                                if (S_PrintResult != "OK")
                                                {
                                                    string ProMsg = MessageInfo.GetMsgByCode(S_PrintResult, List_Login.Language);
                                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20111", "NG", List_Login.Language, new string[] { ProMsg }, S_PrintResult);
                                                }
                                                else
                                                {
                                                    MessageInfo.Add_Info_MSG(Edt_MSG, "10021", "OK", List_Login.Language);
                                                }
                                            }

                                            mesMachineSVC.MesModMachineBySN(Edt_SN.Text.Trim());
                                            mesMachineSVC.SetMachineRuningQuantity(Edt_SN.Text.Trim());
                                        }
                                        /////////////////////////////////////////////////////////////////////////
                                        /////////////////////////////////////////////////////////////////////////

                                        Edt_SN.Text = "";
                                        Edt_SN.Enabled = true;
                                        Edt_SN.Focus();
                                        //Edt_DefectID.Text = "";

                                        Edt_ChildSN.Text = "";
                                        Edt_ChildSN.Enabled = false;
                                        List_ChildSN.Items.Clear();
                                       
                                        TimeSpan ts = DateTime.Now - dateStart;
                                        double mill = Math.Round(ts.TotalMilliseconds, 0);
                                        MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { S_SN, mill.ToString() });
                                    }
                                }
                                finally
                                {
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
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_RouteCheck);
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                    }
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language, new string[] { S_SN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                }
            }
        }

        private void Edt_ChildSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DateTime dateStart = DateTime.Now;
                string ScanSN = Edt_SN.Text.ToString();
                string ChildSN = Edt_ChildSN.Text.ToString();
                try
                {
                    string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                    string PartFamilyID = Com_PartFamily.EditValue.ToString();
                    string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID, 
                        List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());
                    if (string.IsNullOrEmpty(S_UnitStateID))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
                        Edt_ChildSN.Focus();
                        Edt_ChildSN.Text = "";
                        return;
                    }
                    // luUnitStatus  ID
                    //DataRowView DRV_luUnitStatus = Com_luUnitStatus.SelectedItem as DataRowView;
                    string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();   //DRV_luUnitStatus["ID"].ToString().Trim();

                    string S_DefectID =base.DefectChar.Trim();

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
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                            Edt_ChildSN.Focus();
                            Edt_ChildSN.Text = "";
                            return;
                        }


                        DataTable DT_ChildSN = PartSelectSVC.Get_UnitID(S_ChildSN).Tables[0];
                        if (DT_ChildSN.Rows.Count == 0)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language, new string[] { S_ChildSN });
                            Edt_ChildSN.Focus();
                            Edt_ChildSN.Text = "";
                            return;
                        }

                        if (DT_ChildSN.Rows[0]["StatusID"].ToString() != "1")
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20036", "NG", List_Login.Language);
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
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20112", "NG", List_Login.Language);
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
                                        MessageInfo.Add_Info_MSG(Edt_MSG, "20016", "NG", List_Login.Language);
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
                                                        MessageInfo.Add_Info_MSG(Edt_MSG, "20017", "NG", List_Login.Language);
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
                                                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_UpdateUnit });
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
                                                                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_UC_Result });
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
                                                            catch(Exception ex)
                                                            {
                                                                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
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
                                                    MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { S_SN, mill.ToString() });


                                                    Edt_SN.Text = "";
                                                    Edt_SN.Enabled = true;
                                                    Edt_SN.Focus();
                                                    //Edt_DefectID.Text = "";

                                                    Edt_ChildSN.Text = "";
                                                    Edt_ChildSN.Enabled = false;
                                                    List_ChildSN.Items.Clear();                                                   
                                                }
                                            }
                                            else
                                            {
                                                MessageInfo.Add_Info_MSG(Edt_MSG, "20050", "NG", List_Login.Language);
                                                Edt_ChildSN.Text = "";
                                                Edt_ChildSN.Focus();
                                            }
                                        }
                                        else
                                        {
                                            MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language, new string[] { ChildSN });
                                            Edt_ChildSN.Text = "";
                                            Edt_ChildSN.Focus();
                                        }
                                    }
                                }
                                else
                                {
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20039", "NG", List_Login.Language);
                                    Edt_ChildSN.Text = "";
                                    Edt_ChildSN.Focus();
                                }
                            }
                        }
                        else
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20113", "NG", List_Login.Language);
                            Edt_ChildSN.Text = "";
                            Edt_ChildSN.Focus();
                        }
                    }
                    else
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
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
                }
            }
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
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20097", "NG", List_Login.Language);
                    }
                }
                else
                {
                    Edt_Templet.Text = S_LabelPath;
                    if (S_LabelPath.Substring(0, 5) == "ERROR")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(S_LabelPath, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_LabelPath);
                        Edt_Templet.Text = "";
                    }
                }
            }
            catch
            { }
        }

        private void ToolingQCNGPrint_FormClosing(object sender, FormClosingEventArgs e)
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

        private void Btn_ConfirmPO_Click_1(object sender, EventArgs e)
        {
            Edt_SN.Enabled = true;
        }

        private void Com_Part_EditValueChanged(object sender, EventArgs e)
        {
            Get_TempletPath(false);
        }

        private void ToolingQCNGPrint_Load(object sender, EventArgs e)
        {
            public_.OpenBartender(List_Login.StationID.ToString());
        }
    }
}
