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
using App.MyMES.mesMachineService;
using System.Threading;

namespace App.MyMES
{
    public partial class Lamination_Form : Form
    {
        Public_ public_ = new Public_();
        DataTable DT_ProductionOrder = new DataTable();
        DataTable DT_ProductStructure = new DataTable();
        DataTable DT_UnitDetail = new DataTable(); 

        LoginList List_Login = new LoginList();
         
        int I_RouteSequence = -9;  //当前工序顺序 
        int I_POID = 0;
        string S_DefectTypeID = "";
        string S_Box_SN = "";
        string S_BatchHB_SN = "";
        int ScanOKQuantity = 0;
        int ScanNGQuantity = 0;

        PartSelectSVCClient PartSelectSVC;
        ImesUnitSVCClient mesUnitSVC ;
        ImesHistorySVCClient mesHistorySVC;
        ImesUnitDefectSVCClient mesUnitDefectSVC ;
        ImesUnitComponentSVCClient mesUnitComponentSVC;
        ImesProductionOrderSVCClient mesProductionOrderSVC;
        ImesMachineSVCClient mesMachineSVC;

        public Lamination_Form()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            try
            {
                PartSelectSVC.Close();
                mesUnitSVC.Close();
                mesHistorySVC.Close();
                mesUnitDefectSVC.Close();
                mesUnitComponentSVC.Close();
                mesProductionOrderSVC.Close();
                mesMachineSVC.Close(); 
            }
            catch
            { }

            PartSelectSVC = PartSelectFactory.CreateServerClient();
            mesUnitSVC = ImesUnitFactory.CreateServerClient();
            mesHistorySVC = ImesHistoryFactory.CreateServerClient();
            mesUnitDefectSVC = ImesUnitDefectFactory.CreateServerClient();
            mesUnitComponentSVC = ImesUnitComponentFactory.CreateServerClient();
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
           
            Edt_BoxSN.Text = "";
            Edt_BoxSN.Enabled = true;
            Edt_BoxSN.Focus();
            
            //Edt_MSG.Text = "";
            Edt_SN.Text = "";
            Edt_SN.Enabled = false;

            List_ChildSN.Items.Clear();
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

                Edt_BoxSN.Enabled = false;
                Edt_SN.Enabled = false;  
            }
            else
            {
                Com_PartFamilyType.BackColor = Color.Yellow;
                Com_PartFamily.BackColor = Color.Yellow;
                Com_Part.BackColor = Color.Yellow;
                Com_PO.BackColor = Color.Yellow;

                Edt_BoxSN.Enabled = true;
                Edt_SN.Enabled = false;

                Edt_BoxSN.Focus(); 
            }

            Edt_BoxSN.Text = "";
            Edt_SN.Text = ""; 
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

        private void Btn_Unlock_Click(object sender, EventArgs e)
        {
            Edt_Batch.Text = "";
            PartEdtStatus(true);
        }

        private void Lamination_Form_Load(object sender, EventArgs e)
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

        private void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            if (Com_PO.Text.Trim() == "")
            {
                Public_.Add_Info_MSG(Edt_MSG, "工单不能为空!", "NG");
            }
            else
            {
                PartEdtStatus(false);
                Edt_Batch.Enabled = false; 
            }
        }

        private void Lamination_Form_Resize(object sender, EventArgs e)
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

        string S_UnitID = "";
        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //Public_.Add_Info_MSG(Edt_MSG, "", "Start");
                DateTime dateStart = DateTime.Now;
                //Edt_MSG.Text = "";

                string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, List_Login.StationTypeID, List_Login.LineID.ToString());
                // luUnitStatus  ID
                string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();

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

                int I_FirstScanSequence = FirstScanSequence(S_POPartID, List_Login.StationTypeID);

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
                    mesProductionOrder v_mesProductionOrder = mesProductionOrderSVC.Get(Convert.ToInt32(S_POID));
                    S_POPartID = v_mesProductionOrder.PartID.ToString();
                    I_FirstScanSequence = FirstScanSequence(S_POPartID, List_Login.StationTypeID);

                    string S_ComPO = Com_PO.EditValue.ToString();
                    if (S_POID != S_ComPO)
                    {
                        Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码和选择的工单不一致！", "NG");
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }

                }

                string S_Sql = "";
                DataTable DT_SN = PartSelectSVC.GetmesSerialNumber(S_SN).Tables[0]; //public_.P_DataSet(S_Sql).Tables[0]; 

                if (DT_SN.Rows.Count > 0)
                {
                    S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
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

                    ////////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////////
                    //Boolean B_IsRoute = GetIsRoute(List_Login.StationID, DT_Unit);
                    string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, S_SN);

                    if (S_RouteCheck == "1")
                    {
                        //判断托盘数量是否超过最大限制
                        string outString = string.Empty;
                        PartSelectSVC.uspCallProcedure("uspBoxNumberCheck", Edt_BoxSN.Text.ToUpper(), null, null, null, null, null, ref outString);
                        if (outString != "OK")
                        {
                            Public_.Add_Info_MSG(Edt_MSG, outString, "NG");
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                            return;
                        }

                        string S_SelectPartID = DT_Unit.Rows[0]["PartID"].ToString();
                        if (S_POPartID == S_SelectPartID)
                        {
                            string S_StationTypeID = List_Login.StationTypeID.ToString();

                            {
                                //没有子料，这里扫描过站
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
                                            PartSelectSVC.ModPO(S_POID);
                                        }

                                        /////////////////////////////////////////////////////////////////////////
                                        ////////////////////////////////   Component     //////////////////////////

                                        int I_ChildUnitID = Convert.ToInt32(DT_UnitDetail.Rows[0]["UnitID"].ToString());

                                        DataTable DT_ChildUnit = PartSelectSVC.GetComponent(I_ChildUnitID).Tables[0];
                                        string S_ChildSerialNumber = DT_ChildUnit.Rows[0]["Value"].ToString();
                                        int I_ChildPartID = Convert.ToInt32(DT_ChildUnit.Rows[0]["PartID"].ToString());
                                        int I_ChildPartFamilyID = Convert.ToInt32(DT_ChildUnit.Rows[0]["PartFamilyID"].ToString());

                                        mesUnitComponent v_mesUnitComponent = new mesUnitComponent();
                                        v_mesUnitComponent.UnitID = v_mesUnit.ID;
                                        v_mesUnitComponent.UnitComponentTypeID = 1;
                                        v_mesUnitComponent.ChildUnitID = I_ChildUnitID;
                                        v_mesUnitComponent.ChildSerialNumber = S_ChildSerialNumber;

                                        v_mesUnitComponent.ChildLotNumber = S_BatchHB_SN;
                                        v_mesUnitComponent.ChildPartID = I_ChildPartID;
                                        v_mesUnitComponent.ChildPartFamilyID = I_ChildPartFamilyID;
                                        v_mesUnitComponent.Position = "";

                                        v_mesUnitComponent.InsertedTime = DateTime.Now;
                                        v_mesUnitComponent.StatusID = 1;
                                        v_mesUnitComponent.LastUpdate = DateTime.Now;

                                        string S_UC_Result = mesUnitComponentSVC.Insert(v_mesUnitComponent);


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

                                        /////////////////////////////////////////// mesMachine  (2019-11-06 修改按钮执行此代码)//////////////////////////////////////////////////

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

                                        //Edt_BoxSN.Text = "";
                                        //Edt_BoxSN.Enabled = false;
                                        List_ChildSN.Items.Clear();

                                        GetScanQuantity();

                                        TimeSpan ts = DateTime.Now - dateStart;
                                        double mill = Math.Round(ts.TotalMilliseconds, 0);
                                        Public_.Add_Info_MSG(Edt_MSG, S_SN + " " + mill + "毫秒 OK！", "OK");
                                        //Public_.Add_Info_MSG(Edt_MSG, S_SN + " OK！", "OK");
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

        private void Edt_SN_KeyDown____________________(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Thread v_Thread = new Thread(SacnSN0);
                v_Thread.Start();
            }
        }

        private void Edt_SN1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Thread v_Thread1 = new Thread(SacnSN1);
                v_Thread1.Start();
            }
        }

        private void Edt_SN2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Thread v_Thread2 = new Thread(SacnSN2);
                v_Thread2.Start();
            }
        }
        //暂不使用 0  1  2
        private void SacnSN0()
        {
            Edt_SN.Enabled = false;
            Edt_SN1.Enabled = true;
            //Edt_SN1.Focus();
            InvokeFocus(Edt_SN1);
            Edt_SN1.Text = "";


            //Public_.Add_Info_MSG(Edt_MSG, "SN0", "Start");
            DateTime dateStart = DateTime.Now;
            //Edt_MSG.Text = "";

            string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
            string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, List_Login.StationTypeID, List_Login.LineID.ToString());
            // luUnitStatus  ID
            string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();

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

            int I_FirstScanSequence = FirstScanSequence(S_POPartID, List_Login.StationTypeID);

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
                mesProductionOrder v_mesProductionOrder = mesProductionOrderSVC.Get(Convert.ToInt32(S_POID));
                S_POPartID = v_mesProductionOrder.PartID.ToString();
                I_FirstScanSequence = FirstScanSequence(S_POPartID, List_Login.StationTypeID);

                string S_ComPO = Com_PO.EditValue.ToString();
                if (S_POID != S_ComPO)
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码和选择的工单不一致！", "NG");
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }

            }

            string S_Sql = "";
            DataTable DT_SN = PartSelectSVC.GetmesSerialNumber(S_SN).Tables[0]; //public_.P_DataSet(S_Sql).Tables[0]; 

            if (DT_SN.Rows.Count > 0)
            {
                S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
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

                ////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////
                //Boolean B_IsRoute = GetIsRoute(List_Login.StationID, DT_Unit);
                string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, S_SN);

                if (S_RouteCheck == "1")
                {
                    string S_SelectPartID = DT_Unit.Rows[0]["PartID"].ToString();
                    if (S_POPartID == S_SelectPartID)
                    {                       
                        string S_StationTypeID = List_Login.StationTypeID.ToString();

                        {
                            //没有子料，这里扫描过站
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
                                        PartSelectSVC.ModPO(S_POID);
                                    }

                                    /////////////////////////////////////////////////////////////////////////
                                    ////////////////////////////////   Component     //////////////////////////

                                    int I_ChildUnitID = Convert.ToInt32(DT_UnitDetail.Rows[0]["UnitID"].ToString());

                                    DataTable DT_ChildUnit = PartSelectSVC.GetComponent(I_ChildUnitID).Tables[0];
                                    string S_ChildSerialNumber = DT_ChildUnit.Rows[0]["Value"].ToString();
                                    int I_ChildPartID = Convert.ToInt32(DT_ChildUnit.Rows[0]["PartID"].ToString());
                                    int I_ChildPartFamilyID = Convert.ToInt32(DT_ChildUnit.Rows[0]["PartFamilyID"].ToString());

                                    mesUnitComponent v_mesUnitComponent = new mesUnitComponent();
                                    v_mesUnitComponent.UnitID = v_mesUnit.ID;
                                    v_mesUnitComponent.UnitComponentTypeID = 1;
                                    v_mesUnitComponent.ChildUnitID = I_ChildUnitID;
                                    v_mesUnitComponent.ChildSerialNumber = S_ChildSerialNumber;

                                    v_mesUnitComponent.ChildLotNumber = S_BatchHB_SN;
                                    v_mesUnitComponent.ChildPartID = I_ChildPartID;
                                    v_mesUnitComponent.ChildPartFamilyID = I_ChildPartFamilyID;
                                    v_mesUnitComponent.Position = "";

                                    v_mesUnitComponent.InsertedTime = DateTime.Now;
                                    v_mesUnitComponent.StatusID = 1;
                                    v_mesUnitComponent.LastUpdate = DateTime.Now;

                                    string S_UC_Result = mesUnitComponentSVC.Insert(v_mesUnitComponent);


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

                                    /////////////////////////////////////////// mesMachine  (2019-11-06 修改按钮执行此代码)//////////////////////////////////////////////////

                                    /////////////////////////////////////////////////////////////////////////
                                    /////////////////////////////////////////////////////////////////////////

                            //        Edt_SN.Text = "";
                            //        Edt_SN.Enabled = true;
                            //        Edt_SN.Focus();
                                    //Edt_DefectID.Text = "";

                                    //Edt_BoxSN.Text = "";
                                    //Edt_BoxSN.Enabled = false;
                                    List_ChildSN.Items.Clear();

                                    GetScanQuantity();

                                    TimeSpan ts = DateTime.Now - dateStart;
                                    double mill = Math.Round(ts.TotalMilliseconds, 0);
                                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " SN0 " + mill + "毫秒 OK！", "OK");
                                    //Public_.Add_Info_MSG(Edt_MSG, S_SN + " OK！", "OK");
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
        private void SacnSN1()
        {
            Edt_SN1.Enabled = false;
            Edt_SN2.Enabled = true;
            //Edt_SN2.Focus();
            InvokeFocus(Edt_SN2);
            Edt_SN2.Text = "";

            //Public_.Add_Info_MSG(Edt_MSG, "SN1", "Start");
            DateTime dateStart = DateTime.Now;
            //Edt_MSG.Text = "";

            string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
            string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, List_Login.StationTypeID, List_Login.LineID.ToString());
            // luUnitStatus  ID
            string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();

            string S_DefectID = Edt_DefectID.Text.Trim();
            string S_SN = Edt_SN1.Text.Trim();
            //工单  PartID
            string S_POPartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();

            if (S_SN == "")
            {
                Public_.Add_Info_MSG(Edt_MSG, S_SN + " SN不能为空!", "NG");
                Edt_SN1.Focus();
                Edt_SN1.Text = "";
                return;
            }

            if (S_luUnitStateID != "1")
            {
                if (S_DefectID == "")
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " 确认此物料NG,请设置NG原因!", "NG");
                    Edt_SN1.Focus();
                    Edt_SN1.Text = "";
                    return;
                }
            }

            int I_FirstScanSequence = FirstScanSequence(S_POPartID, List_Login.StationTypeID);

            //if (Panel_Part.Visible == false)
            {
                List<string> List_PO = public_.SnToPOID(S_SN);
                string S_POID = List_PO[0];

                if (S_POID.Length > 4)
                {
                    if (S_POID.Substring(0, 5) == "ERROR")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, S_SN + " " + S_POID, "NG");
                        Edt_SN1.Focus();
                        Edt_SN1.Text = "";
                        return;
                    }
                }

                if (S_POID == "" || S_POID == "0")
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码没有第一次过站记录！", "NG");
                    Edt_SN1.Focus();
                    Edt_SN1.Text = "";
                    return;
                };

                //根据  SN  获取工单  料号
                mesProductionOrder v_mesProductionOrder = mesProductionOrderSVC.Get(Convert.ToInt32(S_POID));
                S_POPartID = v_mesProductionOrder.PartID.ToString();
                I_FirstScanSequence = FirstScanSequence(S_POPartID, List_Login.StationTypeID);

                string S_ComPO = Com_PO.EditValue.ToString();
                if (S_POID != S_ComPO)
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码和选择的工单不一致！", "NG");
                    Edt_SN1.Focus();
                    Edt_SN1.Text = "";
                    return;
                }

            }

            string S_Sql = "";
            DataTable DT_SN = PartSelectSVC.GetmesSerialNumber(S_SN).Tables[0]; //public_.P_DataSet(S_Sql).Tables[0]; 

            if (DT_SN.Rows.Count > 0)
            {
                S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                DataTable DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0];
                string S_StationID = List_Login.StationID.ToString();    //DT_Unit.Rows[0]["StationID"].ToString();

                string S_SerialNumberType = DT_SN.Rows[0]["SerialNumberTypeID"].ToString();
                if (S_SerialNumberType != "5")
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码类别不匹配！", "NG");
                    Edt_SN1.Focus();
                    Edt_SN1.Text = "";
                    return;
                }

                if (DT_Unit.Rows[0]["StatusID"].ToString() != "1")
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码已NG！", "NG");
                    Edt_SN1.Focus();
                    Edt_SN1.Text = "";
                    return;
                }

                ////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////
                //Boolean B_IsRoute = GetIsRoute(List_Login.StationID, DT_Unit);
                string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, S_SN);

                if (S_RouteCheck == "1")
                {
                    string S_SelectPartID = DT_Unit.Rows[0]["PartID"].ToString();
                    if (S_POPartID == S_SelectPartID)
                    {                        
                        string S_StationTypeID = List_Login.StationTypeID.ToString();

                        {
                            //没有子料，这里扫描过站
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
                                        PartSelectSVC.ModPO(S_POID);
                                    }

                                    /////////////////////////////////////////////////////////////////////////
                                    ////////////////////////////////   Component     //////////////////////////

                                    int I_ChildUnitID = Convert.ToInt32(DT_UnitDetail.Rows[0]["UnitID"].ToString());

                                    DataTable DT_ChildUnit = PartSelectSVC.GetComponent(I_ChildUnitID).Tables[0];
                                    string S_ChildSerialNumber = DT_ChildUnit.Rows[0]["Value"].ToString();
                                    int I_ChildPartID = Convert.ToInt32(DT_ChildUnit.Rows[0]["PartID"].ToString());
                                    int I_ChildPartFamilyID = Convert.ToInt32(DT_ChildUnit.Rows[0]["PartFamilyID"].ToString());

                                    mesUnitComponent v_mesUnitComponent = new mesUnitComponent();
                                    v_mesUnitComponent.UnitID = v_mesUnit.ID;
                                    v_mesUnitComponent.UnitComponentTypeID = 1;
                                    v_mesUnitComponent.ChildUnitID = I_ChildUnitID;
                                    v_mesUnitComponent.ChildSerialNumber = S_ChildSerialNumber;

                                    v_mesUnitComponent.ChildLotNumber = S_BatchHB_SN;
                                    v_mesUnitComponent.ChildPartID = I_ChildPartID;
                                    v_mesUnitComponent.ChildPartFamilyID = I_ChildPartFamilyID;
                                    v_mesUnitComponent.Position = "";
                                    v_mesUnitComponent.InsertedEmployeeID = List_Login.EmployeeID;
                                    v_mesUnitComponent.InsertedStationID = List_Login.StationID;
                                    v_mesUnitComponent.InsertedTime = DateTime.Now;
                                    v_mesUnitComponent.StatusID = 1;
                                    v_mesUnitComponent.LastUpdate = DateTime.Now;

                                    string S_UC_Result = mesUnitComponentSVC.Insert(v_mesUnitComponent);


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

                                    /////////////////////////////////////////// mesMachine  (2019-11-06 修改按钮执行此代码)//////////////////////////////////////////////////

                                    /////////////////////////////////////////////////////////////////////////
                                    /////////////////////////////////////////////////////////////////////////

                        //            Edt_SN1.Text = "";
                        //            Edt_SN1.Enabled = true;
                        //            Edt_SN1.Focus();
                                   

                                    //Edt_BoxSN.Text = "";
                                    //Edt_BoxSN.Enabled = false;
                                    List_ChildSN.Items.Clear();

                                    GetScanQuantity();

                                    TimeSpan ts = DateTime.Now - dateStart;
                                    double mill = Math.Round(ts.TotalMilliseconds, 0);
                                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " SN1 " + mill + "毫秒 OK！", "OK");
                                    //Public_.Add_Info_MSG(Edt_MSG, S_SN + " OK！", "OK");
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
                        Edt_SN1.Focus();
                        Edt_SN1.Text = "";
                    }
                }
                else
                {
                    Edt_SN1.Focus();
                    Edt_SN1.Text = "";
                    Public_.Add_Info_MSG(Edt_MSG, S_RouteCheck, "NG");
                }
            }
            else
            {
                Public_.Add_Info_MSG(Edt_MSG, S_SN + " 条码不存在!", "NG");
                Edt_SN1.Focus();
                Edt_SN1.Text = "";
            }
        }
        private void SacnSN2()
        {
            Edt_SN2.Enabled = false;
            Edt_SN.Enabled = true;
            //Edt_SN.Focus();
            InvokeFocus(Edt_SN);
            Edt_SN.Text = "";


            //Public_.Add_Info_MSG(Edt_MSG, "SN2", "Start");
            DateTime dateStart = DateTime.Now;
            //Edt_MSG.Text = "";

            string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
            string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, List_Login.StationTypeID, List_Login.LineID.ToString());
            // luUnitStatus  ID
            string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();

            string S_DefectID = Edt_DefectID.Text.Trim();
            string S_SN = Edt_SN2.Text.Trim();
            //工单  PartID
            string S_POPartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();

            if (S_SN == "")
            {
                Public_.Add_Info_MSG(Edt_MSG, S_SN + " SN不能为空!", "NG");
                Edt_SN2.Focus();
                Edt_SN2.Text = "";
                return;
            }

            if (S_luUnitStateID != "1")
            {
                if (S_DefectID == "")
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " 确认此物料NG,请设置NG原因!", "NG");
                    Edt_SN2.Focus();
                    Edt_SN2.Text = "";
                    return;
                }
            }

            int I_FirstScanSequence = FirstScanSequence(S_POPartID, List_Login.StationTypeID);

            //if (Panel_Part.Visible == false)
            {
                List<string> List_PO = public_.SnToPOID(S_SN);
                string S_POID = List_PO[0];

                if (S_POID.Length > 4)
                {
                    if (S_POID.Substring(0, 5) == "ERROR")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, S_SN + " " + S_POID, "NG");
                        Edt_SN2.Focus();
                        Edt_SN2.Text = "";
                        return;
                    }
                }

                if (S_POID == "" || S_POID == "0")
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码没有第一次过站记录！", "NG");
                    Edt_SN2.Focus();
                    Edt_SN2.Text = "";
                    return;
                };

                //根据  SN  获取工单  料号
                mesProductionOrder v_mesProductionOrder = mesProductionOrderSVC.Get(Convert.ToInt32(S_POID));
                S_POPartID = v_mesProductionOrder.PartID.ToString();
                I_FirstScanSequence = FirstScanSequence(S_POPartID, List_Login.StationTypeID);

                string S_ComPO = Com_PO.EditValue.ToString();
                if (S_POID != S_ComPO)
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码和选择的工单不一致！", "NG");
                    Edt_SN2.Focus();
                    Edt_SN2.Text = "";
                    return;
                }

            }

            string S_Sql = "";
            DataTable DT_SN = PartSelectSVC.GetmesSerialNumber(S_SN).Tables[0]; //public_.P_DataSet(S_Sql).Tables[0]; 

            if (DT_SN.Rows.Count > 0)
            {
                S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                DataTable DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0];
                string S_StationID = List_Login.StationID.ToString();    //DT_Unit.Rows[0]["StationID"].ToString();

                string S_SerialNumberType = DT_SN.Rows[0]["SerialNumberTypeID"].ToString();
                if (S_SerialNumberType != "5")
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码类别不匹配！", "NG");
                    Edt_SN2.Focus();
                    Edt_SN2.Text = "";
                    return;
                }

                if (DT_Unit.Rows[0]["StatusID"].ToString() != "1")
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码已NG！", "NG");
                    Edt_SN2.Focus();
                    Edt_SN2.Text = "";
                    return;
                }

                ////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////
                //Boolean B_IsRoute = GetIsRoute(List_Login.StationID, DT_Unit);
                string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, S_SN);

                if (S_RouteCheck == "1")
                {
                    string S_SelectPartID = DT_Unit.Rows[0]["PartID"].ToString();
                    if (S_POPartID == S_SelectPartID)
                    {                     
                        string S_StationTypeID = List_Login.StationTypeID.ToString();

                        {
                            //没有子料，这里扫描过站
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
                                        PartSelectSVC.ModPO(S_POID);
                                    }

                                    /////////////////////////////////////////////////////////////////////////
                                    ////////////////////////////////   Component     //////////////////////////

                                    int I_ChildUnitID = Convert.ToInt32(DT_UnitDetail.Rows[0]["UnitID"].ToString());

                                    DataTable DT_ChildUnit = PartSelectSVC.GetComponent(I_ChildUnitID).Tables[0];
                                    string S_ChildSerialNumber = DT_ChildUnit.Rows[0]["Value"].ToString();
                                    int I_ChildPartID = Convert.ToInt32(DT_ChildUnit.Rows[0]["PartID"].ToString());
                                    int I_ChildPartFamilyID = Convert.ToInt32(DT_ChildUnit.Rows[0]["PartFamilyID"].ToString());

                                    mesUnitComponent v_mesUnitComponent = new mesUnitComponent();
                                    v_mesUnitComponent.UnitID = v_mesUnit.ID;
                                    v_mesUnitComponent.UnitComponentTypeID = 1;
                                    v_mesUnitComponent.ChildUnitID = I_ChildUnitID;
                                    v_mesUnitComponent.ChildSerialNumber = S_ChildSerialNumber;

                                    v_mesUnitComponent.ChildLotNumber = S_BatchHB_SN;
                                    v_mesUnitComponent.ChildPartID = I_ChildPartID;
                                    v_mesUnitComponent.ChildPartFamilyID = I_ChildPartFamilyID;
                                    v_mesUnitComponent.Position = "";
                                    v_mesUnitComponent.InsertedEmployeeID = List_Login.EmployeeID;
                                    v_mesUnitComponent.InsertedStationID = List_Login.StationID;

                                    v_mesUnitComponent.InsertedTime = DateTime.Now;
                                    v_mesUnitComponent.StatusID = 1;
                                    v_mesUnitComponent.LastUpdate = DateTime.Now;

                                    string S_UC_Result = mesUnitComponentSVC.Insert(v_mesUnitComponent);


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

                                    /////////////////////////////////////////// mesMachine  (2019-11-06 修改按钮执行此代码)//////////////////////////////////////////////////

                                    /////////////////////////////////////////////////////////////////////////
                                    /////////////////////////////////////////////////////////////////////////

                        //            Edt_SN2.Text = "";
                        //            Edt_SN2.Enabled = true;
                        //            Edt_SN2.Focus();
                                    //Edt_DefectID.Text = "";

                                    //Edt_BoxSN.Text = "";
                                    //Edt_BoxSN.Enabled = false;
                                    List_ChildSN.Items.Clear();

                                    GetScanQuantity();

                                    TimeSpan ts = DateTime.Now - dateStart;
                                    double mill = Math.Round(ts.TotalMilliseconds, 0);
                                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " " + mill + "毫秒 OK！", "OK");
                                    //Public_.Add_Info_MSG(Edt_MSG, S_SN + " OK！", "OK");
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
                        Edt_SN2.Focus();
                        Edt_SN2.Text = "";
                    }
                }
                else
                {
                    Edt_SN2.Focus();
                    Edt_SN2.Text = "";
                    Public_.Add_Info_MSG(Edt_MSG, S_RouteCheck, "NG");
                }
            }
            else
            {
                Public_.Add_Info_MSG(Edt_MSG, S_SN + " 条码不存在!", "NG");
                Edt_SN2.Focus();
                Edt_SN2.Text = "";
            }
        }

        public void InvokeFocus(Control c)
        {
            if (c.InvokeRequired)
            {
                c.Invoke(new Action<Control>(InvokeFocus), new object[] { c });
            }
            else
            {
                c.Focus();
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

        private void Edt_BoxSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //Public_.Add_Info_MSG(Edt_MSG, "", "Start");
                S_Box_SN = Edt_BoxSN.Text.Trim();

                //string S_Sql = "select * from mesMachine where SN='" + S_Box_SN + "' ";
                //DataTable DT_Machine = public_.P_DataSet(S_Sql).Tables[0];
                DataTable DT_Machine = PartSelectSVC.GetmesMachine(S_Box_SN).Tables[0];   
                if (DT_Machine.Rows.Count == 0)
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_Box_SN + " 无此盒子编码SN!", "NG");
                    Edt_BoxSN.SelectAll();
                    Edt_BoxSN.Text = "";
                    Edt_BoxSN.Focus();
                    return;
                }

                string S_MachineID = DT_Machine.Rows[0]["ID"].ToString();
                string S_StatusID = DT_Machine.Rows[0]["StatusID"].ToString();
                string S_MachineFamilyID = DT_Machine.Rows[0]["MachineFamilyID"].ToString();
                string S_PartID = Com_Part.EditValue.ToString();

                //S_Sql = "select *  from mesRouteMachineMap where MachineID=" + S_MachineID;
                //DataTable DT_MachineMapSN = public_.P_DataSet(S_Sql).Tables[0];
                DataTable DT_MachineMapSN = PartSelectSVC.GetmesRouteMachineMap(S_MachineID, "").Tables[0];

                //S_Sql = "select *  from mesRouteMachineMap where MachineFamilyID=" + S_MachineFamilyID;
                //DataTable DT_MachineMapFamilyID = public_.P_DataSet(S_Sql).Tables[0];
                DataTable DT_MachineMapFamilyID = PartSelectSVC.GetmesRouteMachineMap("", S_MachineFamilyID).Tables[0];

                string S_MapPartID = "";
                if (DT_MachineMapSN.Rows.Count > 0)
                {
                    S_MapPartID = DT_MachineMapSN.Rows[0]["PartID"].ToString();
                }
                else if (DT_MachineMapFamilyID.Rows.Count > 0)
                {
                    S_MapPartID = DT_MachineMapFamilyID.Rows[0]["PartID"].ToString();
                }
                /////////////////////////////////////////////////////////////////////
                //S_Sql = "select *  from  mesProductStructure WHERE ParentPartID="+ S_PartID;
                //DataTable DT_PS = public_.P_DataSet(S_Sql).Tables[0];

                DataTable DT_PS = PartSelectSVC.GetmesProductStructure1(S_PartID).Tables[0]; 
                string S_PartID_PS = DT_PS.Rows[0]["PartID"].ToString();
                if (S_MapPartID != S_PartID_PS)
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_Box_SN + " 盒子和主条码不匹配,请检查BOM设置！", "NG");
                    Edt_BoxSN.SelectAll();
                    Edt_BoxSN.Text = "";
                    Edt_BoxSN.Focus();
                    return;
                }
                /////////////////////////////////////////////////////////////////////////
                S_BatchHB_SN = "";
                DT_UnitDetail = public_.BoxSnToBatch(S_Box_SN, out S_BatchHB_SN);
               
                if (S_BatchHB_SN != "")
                {
                    if (S_BatchHB_SN.Substring(0, 5) == "ERROR")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, S_BatchHB_SN, "NG");
                        Edt_BoxSN.Text = "";
                    }
                    else
                    {
                        //Edt_BoxSN.Text = S_BatchHB_SN;
                        //Edt_MSG.Text = "";
                        Edt_Batch.Text = S_BatchHB_SN;
                        Edt_BoxSN.Enabled = false;

                        Edt_SN.Enabled = true;
                        Edt_SN.Focus();
                        Edt_SN.SelectAll();

                        Btn_Filish.Enabled = true;
                        Public_.Add_Info_MSG(Edt_MSG, "盒子批次扫描成功！", "OK");
                    }
                }
            }
        }

        private void Lamination_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                PartSelectSVC.Close();
                mesUnitSVC.Close();
                mesHistorySVC.Close();
                mesUnitDefectSVC.Close();
                mesUnitComponentSVC.Close();
                mesProductionOrderSVC.Close();
                mesMachineSVC.Close(); 
            }
            catch
            { }
        }

        private void Btn_Filish_Click(object sender, EventArgs e)
        {
            ///////////////////////////////////////// mesMachine  //////////////////////////////////////////////////
            if (S_Box_SN == "")
            {
                Public_.Add_Info_MSG(Edt_MSG, "盒子SN不能为空", "NG");
                return;
            }

            if (MessageBox.Show("你确定此盒物料扫描完成了吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                try
                {
                    PartSelectSVC.ModMachine(S_Box_SN,"1",true); 
                    PartEdtStatus(false);
                    Edt_Batch.Text = "";
                    Btn_Filish.Enabled = false;
                }
                catch (Exception ex)
                {
                    Public_.Add_Info_MSG(Edt_MSG, ex.Message, "NG");
                }
            }
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
    }
}
