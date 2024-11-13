﻿using System;
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

namespace App.MyMES
{
    public partial class ToolingOverStationV2_Form : FrmBase
    {
        string S_UnitID = "";

        public ToolingOverStationV2_Form()
        {
            InitializeComponent();
        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            Edt_SN.Enabled = false;
            Edt_ChildSN.Enabled = false;
        }

        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            base.Btn_ConfirmPO_Click(sender, e);
            base.Btn_Defect.Enabled = true;
            base.Com_luUnitStatus.Enabled = true;
            Edt_SN.Enabled = true;
            Edt_SN.Focus();

            SetQC();
        }

        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                List<mesUnit> List_mesUnit = new List<mesUnit>();
                List<mesHistory> List_mesHistory = new List<mesHistory>();
                List<mesUnitDefect> List_mesUnitDefect = new List<mesUnitDefect>();

                DateTime dateStart = DateTime.Now;
                string PartFamilyID = Com_PartFamily.EditValue.ToString();
                string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                //string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID, 
                //    List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());
                string S_UnitStateID = string.Empty;
                DataSet tmpDataSet = PartSelectSVC.GetmesUnitState(S_PartID, PartFamilyID, "", List_Login.LineID.ToString(), List_Login.StationTypeID, Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());
                if (tmpDataSet != null && tmpDataSet.Tables.Count != 0 && tmpDataSet.Tables[0].Rows.Count != 0)
                {
                    S_UnitStateID = tmpDataSet.Tables[0].Rows[0]["ID"].ToString().Trim();
                }
                if (string.IsNullOrEmpty(S_UnitStateID))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
                    return;
                }

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

                // Buck 获取  SN
                string S_FG_SN = PartSelectSVC.BuckToFGSN(S_SN);
                S_SN = S_FG_SN;

                {
                    string[] List_PO = PartSelectSVC.SnToPOID(S_SN);
                    string S_POID = List_PO[0];

                    if (S_POID.Length > 4)
                    {
                        if (S_POID.Substring(0, 5) == "ERROR")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(S_POID.Replace("ERROR", ""), List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_POID.Replace("ERROR", ""));
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
                    string S_ComPO = Com_PO.EditValue.ToString();
                    if (S_POID != S_ComPO)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20015", "NG", List_Login.Language, new string[] { S_SN });
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }

                }

                DataTable DT_SN = PartSelectSVC.GetmesSerialNumber(S_SN).Tables[0]; 
                if (DT_SN.Rows.Count > 0)
                {
                    S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                    DataTable DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0];

                    string S_StationID = List_Login.StationID.ToString(); 

                    if (DT_Unit.Rows[0]["StatusID"].ToString() != "1")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20036", "NG", List_Login.Language);
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }
 
                    string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, S_SN);

                    if (S_RouteCheck == "1")
                    {


                        string S_SelectPartID = DT_Unit.Rows[0]["PartID"].ToString();
                        if (S_POPartID == S_SelectPartID)
                        {
                            //调用通用过程
                            string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + Com_PO.EditValue.ToString() + "\"> </ProdOrder>";
                            string xmlPart = "<Part PartID=\"" + S_POPartID + "\"> </Part>";
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


                            string S_StationTypeID = List_Login.StationTypeID.ToString();

                            DT_ProductStructure = PartSelectSVC.GetmesProductStructure2(S_POPartID, S_StationTypeID).Tables[0];   //public_.P_DataSet(S_Sql).Tables[0];
                            if (DT_ProductStructure.Rows.Count > 0)
                            {
                                Edt_SN.Enabled = false;
                                Edt_ChildSN.Enabled = true;
                                Edt_ChildSN.Focus();
                            }
                            else
                            {

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
                                    List_mesUnit.Add(v_mesUnit);

                                    //string S_UpdateUnit = mesUnitSVC.Update(v_mesUnit);
                                    //if (S_UpdateUnit.Substring(0, 1) == "E")
                                    //{

                                    //    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_UpdateUnit });
                                    //    return;
                                    //}

                                    //if (S_UpdateUnit.Substring(0, 1) != "E")
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
                                        v_mesHistory.StatusID = Convert.ToInt32(S_luUnitStateID);
                                        //int I_InsertHistory = mesHistorySVC.Insert(v_mesHistory);
                                        List_mesHistory.Add(v_mesHistory);

                                        //////////////////////////  Defect ///////////////////////////////////////////
                                        //////////////////////////////////////////////////////////////////////////////
                                        string[] Array_Defect = S_DefectID.Split(';');
                                        if (S_luUnitStateID != "1")
                                        {
                                            foreach (var item in Array_Defect)
                                            {
                                                //string S_mesUnitDefectInsert = "";
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

                                                        List_mesUnitDefect.Add(v_mesUnitDefect);

                                                        //S_mesUnitDefectInsert = mesUnitDefectSVC.Insert(v_mesUnitDefect);
                                                    }
                                                }
                                                catch(Exception ex)
                                                {
                                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                                }
                                            }
                                        }

                                        mesUnit[] L_mesUnit = List_mesUnit.ToArray();
                                        mesHistory[] L_mesHistory = List_mesHistory.ToArray();
                                        mesUnitDefect[] L_mesUnitDefect = List_mesUnitDefect.ToArray();

                                        string ReturnValue = DataCommitSVC.SubmitDataUHD(L_mesUnit, L_mesHistory, L_mesUnitDefect);
                                        if (ReturnValue != "OK")
                                        {
                                            ReturnValue = "SN:" + S_SN + "  " + ReturnValue;
                                            MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ReturnValue });
                                            return;
                                        }

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

                                        Edt_SN.Text = "";
                                        Edt_SN.Enabled = true;
                                        Edt_SN.Focus();

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
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] {  ProMsg }, S_RouteCheck);
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
                List<mesUnit> List_mesUnit = new List<mesUnit>();
                List<mesHistory> List_mesHistory = new List<mesHistory>();
                List<mesUnitDefect> List_mesUnitDefect = new List<mesUnitDefect>();
                List<mesUnitComponent> List_mesUnitComponent = new List<mesUnitComponent>(); 

                DateTime dateStart = DateTime.Now;
                string ScanSN = Edt_SN.Text.ToString();
                string ChildSN = Edt_ChildSN.Text.ToString();
                try
                {
                    string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                    string PartFamilyID = Com_PartFamily.EditValue.ToString();
                    string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID, 
                        List_Login.LineID.ToString(), Com_PO.EditValue.ToString(),Com_luUnitStatus.EditValue.ToString());
                    if (string.IsNullOrEmpty(S_UnitStateID))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
                        return;
                    }
                    // luUnitStatus  ID
                    string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();   //DRV_luUnitStatus["ID"].ToString().Trim();

                    string S_DefectID = base.DefectChar;

                    //工单 PartID
                    string S_POPartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();

                    string S_POID = DT_ProductionOrder.Rows[0]["ID"].ToString();
                    string S_PO_StatusID = DT_ProductionOrder.Rows[0]["StatusID"].ToString();

                    string S_SN = Edt_SN.Text.Trim();

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
                        string S_StationID = List_Login.StationID.ToString();
                        int I_ComponentCount = PartSelectSVC.GetComponentCount(S_POPartID, S_StationTypeID);   //DT_ComponentCount.Rows.Count;

                        if (B_ChildSN_Exist == false)
                        {
                            if (B_PartID_Exist == true)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20039", "NG", List_Login.Language);
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

                                    if (S_StationID == List_Login.StationID.ToString())
                                    {
                                        MessageInfo.Add_Info_MSG(Edt_MSG, "20016", "NG", List_Login.Language);
                                        Edt_ChildSN.Focus();
                                        Edt_ChildSN.Text = "";
                                    }
                                    else
                                    {
                                        DataTable DT_mesUnitComponent = PartSelectSVC.GetmesUnitComponent(S_UnitID, S_ChildUnitID).Tables[0];

                                        if (DT_mesUnitComponent.Rows.Count == 0)
                                        {

                                            S_StationID = List_Login.StationID.ToString();
                                            DT_ProductStructure = PartSelectSVC.GetmesProductStructure(S_POPartID, S_ChildPartID, S_StationTypeID).Tables[0];

                                            if (DT_ProductStructure.Rows.Count > 0)
                                            {

                                                DataTable DT_ChildScanLast = new DataTable();
                                                DT_ChildScanLast = PartSelectSVC.GetChildScanLast(S_ChildSN).Tables[0];

                                                try
                                                {    
                                                    string S_StationTypeID_LastScan = DT_ChildScanLast.Rows[0]["StationTypeID"].ToString();
                                                    string S_StationTypeID_RouteDetail_LastScan = DT_ChildScanLast.Rows[0]["StationTypeID_RouteDetail"].ToString();

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
                                                    List_mesUnit.Add(v_mesUnit);

                                                    //string S_UpdateUnit = mesUnitSVC.Update(v_mesUnit);
                                                    //if (S_UpdateUnit.Substring(0, 1) == "E")
                                                    //{
                                                    //    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_UpdateUnit });
                                                    //    return;
                                                    //}

                                                    //if (S_UpdateUnit.Substring(0, 1) != "E")
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
                                                        v_mesHistory.StatusID = Convert.ToInt32(S_luUnitStateID);
                                                        //int I_InsertHistory = mesHistorySVC.Insert(v_mesHistory);
                                                        List_mesHistory.Add(v_mesHistory);
                                                    }
                                                    //////////////////////////////////// ChildSN /////////////////////////////////////////
                                                    //////////////////////////////////// ChildSN /////////////////////////////////////////
                                                    for (int i = 0; i < List_ChildSN.Items.Count; i++)
                                                    {
                                                        string S_DoChildSN = List_ChildSN.Items[i].ToString();
                                                        DataTable DT_UPUnitID_Child = PartSelectSVC.Get_UnitID(S_ChildSN).Tables[0];
                                                        int I_PartID_Child = Convert.ToInt32(DT_UPUnitID_Child.Rows[0]["PartID"].ToString());

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
                                                        //string S_UpdateUnit_Child = mesUnitSVC.Update(v_mesUnit_Child);
                                                        List_mesUnit.Add(v_mesUnit_Child);

                                                        //if (S_UpdateUnit_Child.Substring(0, 1) != "E")
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
                                                            v_mesHistory.StatusID = Convert.ToInt32(S_luUnitStateID);
                                                            //int I_InsertHistory = mesHistorySVC.Insert(v_mesHistory);
                                                            List_mesHistory.Add(v_mesHistory);

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

                                                            List_mesUnitComponent.Add(v_mesUnitComponent);
                                                            //string S_UC_Result = mesUnitComponentSVC.Insert(v_mesUnitComponent);

                                                            //if (S_UC_Result.Substring(0, 1) == "E")
                                                            //{
                                                            //    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_UC_Result });
                                                            //    return;
                                                            //}
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
                                                            //string S_mesUnitDefectInsert = "";
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

                                                                    List_mesUnitDefect.Add(v_mesUnitDefect);

                                                                    //S_mesUnitDefectInsert = mesUnitDefectSVC.Insert(v_mesUnitDefect);
                                                                }
                                                            }
                                                            catch(Exception  ex)
                                                            {
                                                                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                                            }
                                                        }
                                                    }

                                                    mesUnit[] L_mesUnit = List_mesUnit.ToArray();
                                                    mesHistory[] L_mesHistory = List_mesHistory.ToArray();
                                                    mesUnitComponent[] L_mesUnitComponent = List_mesUnitComponent.ToArray();
                                                    mesUnitDefect[] L_mesUnitDefect = List_mesUnitDefect.ToArray();

                                                    string ReturnValue = DataCommitSVC.SubmitDataUHCD(L_mesUnit, L_mesHistory, L_mesUnitComponent,
                                                        L_mesUnitDefect);
                                                    if (ReturnValue != "OK")
                                                    {
                                                        ReturnValue = "SN:" + S_SN + "  " + ReturnValue;
                                                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ReturnValue });
                                                        return;
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


                                                    if (S_luUnitStateID == "1")
                                                    {
                                                        base.SetOverStiaonQTY(true);
                                                    }
                                                    else if(S_luUnitStateID == "2")
                                                    {
                                                        base.SetOverStiaonQTY(false);
                                                    }

                                                    Edt_SN.Text = "";
                                                    Edt_SN.Enabled = true;
                                                    Edt_SN.Focus();

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
                            LogNet.LogDug(this.Name, "Station:" + List_Login.StationType + ",MainSN:" + ScanSN + ",ChildSN:" +
                                                    ChildSN + ",Time：" + mill.ToString() + "ms");
                        }
                        else
                        {
                            LogNet.LogEor(this.Name, "Station:" + List_Login.StationType + ",MainSN:" + ScanSN + ",ChildSN:" +
                                                    ChildSN + ",Time：" + mill.ToString() + "ms");
                        }
                    }
                }
            }
        }
    }
}