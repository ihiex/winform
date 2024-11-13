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
using App.Model;
using App.MyMES.mesUnitDetailService;

namespace App.MyMES
{
    public partial class LinkUPC_Form : Form
    {
        Public_ public_ = new Public_();
        DataTable DT_ProductionOrder = new DataTable();
        DataTable DT_ProductStructure = new DataTable();

        LoginList List_Login = new LoginList();

        PartSelectSVCClient PartSelectSVC ;
        ImesUnitSVCClient mesUnitSVC;
        ImesHistorySVCClient mesHistorySVC;
        ImesUnitDefectSVCClient mesUnitDefectSVC ;
        ImesUnitDetailSVCClient mesUnitDetailSVC ;

        int I_RouteSequence = -9;  //当前工序顺序 
        int I_POID = 0;
        string S_DefectTypeID = "";

        public LinkUPC_Form()
        {
            InitializeComponent();
        }

        private void LinkUPC_Form_Load(object sender, EventArgs e)
        {
            Btn_Refresh_Click(sender, e);
            Com_PO.Enabled = true;
        }

        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            try
            {
                PartSelectSVC.Close();
                mesUnitSVC.Close();
                mesUnitDetailSVC.Close();
                mesHistorySVC.Close();
                mesUnitDefectSVC.Close();
            }
            catch
            { }

            PartSelectSVC = PartSelectFactory.CreateServerClient();
            mesUnitSVC = ImesUnitFactory.CreateServerClient();
            mesUnitDetailSVC = ImesUnitDetailFactory.CreateServerClient();
            mesHistorySVC = ImesHistoryFactory.CreateServerClient();
            mesUnitDefectSVC = ImesUnitDefectFactory.CreateServerClient();


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


            Edt_SN.Focus();
            Edt_SN.SelectAll();

            //Edt_MSG.Text = "";
            Edt_SN.Text = "";
            Edt_SN.Enabled = true;

            Edt_UPCSN.Text = "";
            Edt_UPCSN.Enabled = false;

            PartEdtStatus(true);
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
            string S_Sql = "select *  from mesProductionOrder where ID='" + S_POID + "'";
            DT_ProductionOrder = public_.P_DataSet(S_Sql).Tables[0];

            Edt_OrderQuantity.Text = DT_ProductionOrder.Rows[0]["OrderQuantity"].ToString();
            GetScanQuantity();
        }



        private void GetScanQuantity()
        {
            string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
            string S_Sql = "select COUNT(ID) CountUnit  from  mesUnit  where PartID=" + S_PartID +
                            "  and StationID=" + List_Login.StationID +
                            "  and ProductionOrderID=" + I_POID;
            DataTable DT_CountUnit = public_.P_DataSet(S_Sql).Tables[0];

            Edt_ScanQuantity.Text = DT_CountUnit.Rows[0]["CountUnit"].ToString();
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

        string S_UnitID = "";
        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                public_.Add_Info_MSG(Edt_MSG, "", "Start");
                //Edt_MSG.Text = "";
                string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, List_Login.StationTypeID, List_Login.LineID.ToString());

                string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();     
                string S_DefectID = Edt_DefectID.Text.Trim();
                string S_SN = Edt_SN.Text.Trim();
                //工单  PartID
                string S_POPartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();

                if (S_luUnitStateID != "1")
                {
                    if (S_DefectID == "")
                    {
                        public_.Add_Info_MSG(Edt_MSG, S_SN + " 确认此物料NG,请设置NG原因!", "NG");
                        Edt_SN.Text = "";
                        Edt_SN.Focus();                        
                        return;
                    }
                }

                int I_FirstScanSequence = FirstScanSequence(S_POPartID, List_Login.StationTypeID);

                List<string> List_PO = public_.SnToPOID(S_SN);
                string S_POID = List_PO[0];                

                if (S_POID.Length > 4)
                {
                    if (S_POID.Substring(0, 5) == "ERROR")
                    {
                        public_.Add_Info_MSG(Edt_MSG, S_SN + " " + S_POID, "NG");
                        Edt_SN.Text = "";
                        Edt_SN.Focus();
                        return;
                    }
                }

                if (S_POID == "" || S_POID == "0")
                {
                    public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码没有第一次过站记录！", "NG");
                    Edt_SN.Text = "";
                    Edt_SN.Focus();
                    return;
                };

                //根据  SN  获取工单  料号
                string S_SqlPO = "select *  from mesProductionOrder where ID='" + S_POID + "'";
                DataTable DT_PO = public_.P_DataSet(S_SqlPO).Tables[0];

                S_POPartID = DT_PO.Rows[0]["PartID"].ToString();
                I_FirstScanSequence = FirstScanSequence(S_POPartID, List_Login.StationTypeID);

                string S_ComPO = Com_PO.EditValue.ToString();
                if ((S_POID != S_ComPO ) &&(S_POPartID!= List_PO[1]))
                {
                    public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码和选择的工单不一致！", "NG");
                    Edt_SN.Text = "";
                    Edt_SN.Focus();
                    return;
                }
               
                string S_Sql = "select * from mesSerialNumber where Value ='" + S_SN + "'";
                DataTable DT_SN = public_.P_DataSet(S_Sql).Tables[0];
                if (DT_SN.Rows.Count > 0)
                {
                    S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                    S_Sql = "select * from mesUnit where ID='" + S_UnitID + "'";
                    DataTable DT_Unit = public_.P_DataSet(S_Sql).Tables[0];
                    string S_StationID = List_Login.StationID.ToString();    //DT_Unit.Rows[0]["StationID"].ToString();

                    string S_SerialNumberType = DT_SN.Rows[0]["SerialNumberTypeID"].ToString();
                    if (S_SerialNumberType != "5")
                    {
                        public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码类别不匹配！", "NG");
                        Edt_SN.Text = "";
                        Edt_SN.Focus();
                        return;
                    }

                    if (DT_Unit.Rows[0]["StatusID"].ToString() != "1")
                    {
                        public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码已NG！", "NG");
                        Edt_SN.Text = "";
                        Edt_SN.Focus();                        
                        return;
                    }

                    ////////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////////

                    string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, S_SN);
                    if (S_RouteCheck == "1")
                    {
                        string S_SelectPartID = DT_Unit.Rows[0]["PartID"].ToString();
                        if (S_POPartID == S_SelectPartID)
                        {
                            Edt_SN.Enabled = false;
                            
                            Edt_UPCSN.Text = "";
                            Edt_UPCSN.Enabled = true;
                            Edt_UPCSN.Focus();
                            Edt_UPCSN.SelectAll();
                        }
                        else
                        {
                            public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码和工单不匹配", "NG");
                            Edt_SN.Text = "";
                            Edt_SN.Focus();                            
                        }
                    }
                    else
                    {
                        Edt_SN.Text = "";
                        Edt_SN.Focus();                       
                        public_.Add_Info_MSG(Edt_MSG, S_RouteCheck, "NG");
                    }
                }
                else
                {
                    public_.Add_Info_MSG(Edt_MSG, S_SN + " 条码不存在!", "NG");
                    Edt_SN.Text = "";
                    Edt_SN.Focus();                    
                }
            }
        }

        private void Edt_UPCSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                public_.Add_Info_MSG(Edt_MSG, "", "Start");
                ////Edt_MSG.Text = "";
                string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, List_Login.StationTypeID, List_Login.LineID.ToString());

                string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();
                string S_DefectID = Edt_DefectID.Text.Trim();
                string S_SN = Edt_SN.Text.Trim();
                string S_UPCSN = Edt_UPCSN.Text.Trim();
                //工单  PartID
                string S_POPartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();

                if (S_luUnitStateID != "1")
                {
                    if (S_DefectID == "")
                    {
                        public_.Add_Info_MSG(Edt_MSG, S_UPCSN + " 确认此物料NG,请设置NG原因!", "NG");
                        Edt_UPCSN.Focus();
                        Edt_UPCSN.Text = "";
                        return;
                    }
                }

                List<string> List_PO = public_.SnToPOID(S_UPCSN);
                string S_POID = List_PO[0];

                if (S_POID.Length > 4)
                {
                    if (S_POID.Substring(0, 5) == "ERROR")
                    {
                        public_.Add_Info_MSG(Edt_MSG, S_UPCSN + " " + S_POID, "NG");
                        Edt_UPCSN.Focus();
                        Edt_UPCSN.Text = "";
                        return;
                    }
                }

                string S_Sql = "select KitSerialNumber from mesUnitDetail where KitSerialNumber='" + S_UPCSN + "'";
                DataTable DT_Kit = public_.P_DataSet(S_Sql).Tables[0];
                if (DT_Kit.Rows.Count > 0)
                {
                    public_.Add_Info_MSG(Edt_MSG, S_UPCSN + " UPC已经和其他产品绑定！" , "NG");
                    Edt_UPCSN.Text = ""; 
                    Edt_UPCSN.Focus();
                    return;
                }

                string S_ComPO = Com_PO.EditValue.ToString();
                if ((S_POID != S_ComPO) || (S_POPartID != List_PO[1]))
                {
                    public_.Add_Info_MSG(Edt_MSG, S_UPCSN + " 此UPC条码和选择的工单不一致！", "NG");
                    Edt_UPCSN.Focus();
                    Edt_UPCSN.Text = "";
                    return;
                }

                S_Sql = "select * from mesSerialNumber where Value ='" + S_UPCSN + "'";
                DataTable DT_SN = public_.P_DataSet(S_Sql).Tables[0];
                if (DT_SN.Rows.Count > 0)
                {
                    S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                    S_Sql = "select * from mesUnit where ID='" + S_UnitID + "'";
                    DataTable DT_Unit = public_.P_DataSet(S_Sql).Tables[0];
                    string S_StationID = List_Login.StationID.ToString();    //DT_Unit.Rows[0]["StationID"].ToString();

                    string S_SerialNumberType = DT_SN.Rows[0]["SerialNumberTypeID"].ToString();
                    if (S_SerialNumberType != "6")
                    {
                        public_.Add_Info_MSG(Edt_MSG, S_UPCSN + " 此条码类别不匹配！", "NG");
                        Edt_UPCSN.Focus();
                        Edt_UPCSN.Text = "";
                        return;
                    }

                    if (DT_Unit.Rows[0]["UnitStateID"].ToString() == "2")
                    {
                        public_.Add_Info_MSG(Edt_MSG, S_UPCSN + " 此条码已NG！", "NG");
                        Edt_UPCSN.Focus();
                        Edt_UPCSN.Text = "";
                        return;
                    }

                    ////////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////////
                    
                    //string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, DT_Unit, S_SN);

                    //if (S_RouteCheck == "1")  UPC  不用检查路径
                    {
                        string S_SelectPartID = DT_Unit.Rows[0]["PartID"].ToString();
                        if (S_POPartID == S_SelectPartID)
                        {
                            string S_StationTypeID = List_Login.StationTypeID.ToString();
                            S_Sql = "select * from mesProductStructure where ParentPartID='" + S_POPartID + "' and StationTypeID='" + S_StationTypeID + "'";

                            DT_ProductStructure = public_.P_DataSet(S_Sql).Tables[0];
                            if (DT_ProductStructure.Rows.Count > 0)
                            {
                                //Edt_SN.Enabled = false;
                                Edt_UPCSN.Enabled = true;
                                Edt_UPCSN.Focus();
                            }
                            else
                            {
                                // UPC绑定过站
                                //ImesUnitSVCClient mesUnitSVC = ImesUnitFactory.CreateServerClient();
                                //ImesHistorySVCClient mesHistorySVC = ImesHistoryFactory.CreateServerClient();
                                //ImesUnitDefectSVCClient mesUnitDefectSVC = ImesUnitDefectFactory.CreateServerClient();
                                //ImesUnitDetailSVCClient mesUnitDetailSVC = ImesUnitDetailFactory.CreateServerClient();

                                try
                                {
                                    S_POID = DT_ProductionOrder.Rows[0]["ID"].ToString();
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
                                        public_.Add_Info_MSG(Edt_MSG, S_UpdateUnit, "NG");
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
                                        //string S_PO_StatusID = DT_ProductionOrder.Rows[0]["StatusID"].ToString();
                                        //if (S_PO_StatusID == "1")
                                        //{
                                        //    S_Sql = "Update mesProductionOrder set StatusID=2,LastUpdate='" + DateTime.Now.ToString() +
                                        //            "' where ID='" + S_POID + "'";
                                        //    public_.ExecSql(S_Sql);
                                        //}

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
                                                    public_.Add_Info_MSG(Edt_MSG, S_mesUnitDefectInsert, "NG");
                                                }
                                            }
                                        }
                                        ///////////////////////////////////////////////////////////////////////
                                        ///////////////////////////////////////////////////////////////////////

                                        string S_InsertUnitDetail = "";
                                        try
                                        {
                                            //////////////////////////////////////  mesUnitDetail /////////////////////////////////////////////////////                                    
                                            S_Sql = "Update mesUnitDetail set KitSerialNumber='" + S_UPCSN + "' where UnitID=" + v_mesUnit.ID;
                                            public_.ExecSql(S_Sql); 
                                        }
                                        catch (Exception ex)
                                        {
                                            public_.Add_Info_MSG(Edt_MSG, S_SN + " " + S_UPCSN + " mesUnitDetail Update" + S_InsertUnitDetail + " " + ex.Message, "NG");
                                        }
                                        ///////////////////////////////////////////////////////////////////////

                                        GetScanQuantity();
                                        public_.Add_Info_MSG(Edt_MSG,S_UPCSN+" "+ S_SN + " OK！", "OK");

                                        Edt_SN.Text = "";
                                        Edt_SN.Enabled = true;
                                        Edt_SN.Focus();

                                        Edt_UPCSN.Text = "";
                                        Edt_UPCSN.Enabled = false;
                                    }
                                }
                                finally
                                {
                                    mesUnitSVC.Close();
                                    mesHistorySVC.Close();
                                    mesUnitDefectSVC.Close();
                                    mesUnitDetailSVC.Close();
                                }
                            }
                        }
                        else
                        {
                            public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码和工单不匹配", "NG");
                            Edt_UPCSN.Focus();
                            Edt_UPCSN.Text = "";
                        }
                    }
                    //else
                    //{
                    //    Edt_SN.Focus();
                    //    Edt_SN.Text = "";
                    //    public_.Add_Info_MSG(Edt_MSG, S_RouteCheck, "NG");
                    //}
                }
                else
                {
                    public_.Add_Info_MSG(Edt_MSG, S_SN + " UPC条码不存在!", "NG");
                    Edt_UPCSN.Focus();
                    Edt_UPCSN.Text = "";
                }
            }
        }

        private void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            if (Com_PO.Text.Trim() == "")
            {
                public_.Add_Info_MSG(Edt_MSG, "工单不能为空!", "NG");
            }
            else
            {
                PartEdtStatus(false);
            }
        }

        private void Btn_Defect_Click(object sender, EventArgs e)
        {
            CloseDefect();

            DefectForm v_DefectForm = new DefectForm();
            //v_DefectForm.Show_DefectForm(v_DefectForm, Edt_DefectID);
            v_DefectForm.Show_DefectFormToPanel(v_DefectForm, Edt_DefectID,Convert.ToInt32(S_DefectTypeID));

            v_DefectForm.FormBorderStyle = FormBorderStyle.None;
            v_DefectForm.Width = Panel_Defact.Width;
            v_DefectForm.Height = Panel_Defact.Height;
            v_DefectForm.TopLevel = false;
            v_DefectForm.Parent = Panel_Defact;
            v_DefectForm.BringToFront();
            // v_DefectForm.Tag = F_LoginList;
            v_DefectForm.Show();
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

        private void LinkUPC_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                PartSelectSVC.Close();
                mesUnitSVC.Close();
                mesUnitDetailSVC.Close();
                mesHistorySVC.Close();
                mesUnitDefectSVC.Close();
            }
            catch
            { }
        }
    }
}
