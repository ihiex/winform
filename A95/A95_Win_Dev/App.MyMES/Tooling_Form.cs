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
using App.Model;
using System.Threading;
using App.MyMES.mesUnitDefectService;

namespace App.MyMES
{
    public partial class Tooling_Form : Form
    {
        Public_ public_ = new Public_();
        DataTable DT_ProductionOrder = new DataTable();
        DataTable DT_ProductStructure = new DataTable();

        LoginList List_Login = new LoginList();
        PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();

        int I_RouteSequence = -9;  //当前工序顺序 
        int I_POID = 0;

        public Tooling_Form()
        {
            InitializeComponent();
        }

        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            List_Login = this.Tag as LoginList;

            public_.AddStationType(Com_StationType);
            public_.AddLine(Com_Line);
          //  public_.AddmesUnitState(Com_UnitState);

            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_LineID = List_Login.LineID.ToString();
            public_.AddStation(Com_Station,S_LineID);

            Com_StationType.Text = S_StationTypeID;
            Com_Line.Text = S_LineID;
            Com_Station.Text = List_Login.StationID.ToString();

            //DataRowView DRV = Com_Part.SelectedItem as DataRowView;
            //string S_PartID= DRV["ID"].ToString();

            public_.AddPOAll(Com_PO,"",S_LineID);
            //Com_PO.Text = List_Login.POID.ToString();

            Edt_SN.Text = "";
            Edt_SN.Enabled = true;
            //Edt_MSG.Text = "";  

            Edt_ToolSN.Text = "";
            Edt_ToolSN.Enabled = false;
            List_ToolSN.Items.Clear();
            //Com_UnitState.Enabled = true;

            timer_GridColor.Enabled = true;            
        }

        private void Tooling_Form_Load(object sender, EventArgs e)
        {
            Btn_Refresh_Click(sender, e);

            string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
            I_RouteSequence = public_.RouteSequence(S_PartID, List_Login.StationID, List_Login.LineID.ToString());

            Com_PO.Enabled = true;
            if (I_RouteSequence > 1)
            {               
                Com_PO.Enabled = false;
                Com_PO.BackColor = Color.Yellow;
            }
        }

        //private void Grid_NGColor(DataGridView v_DataGridView)
        //{
        //    for (int i = 0; i < v_DataGridView.Rows.Count; i++)
        //    {
        //        string S_UnitState = v_DataGridView.Rows[i].Cells["UnitState"].Value.ToString();
        //        if (S_UnitState == "NG")
        //        {
        //            v_DataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
        //        }
        //    }
        //}

        private void GetScanQuantity()
        {
            string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
            string S_Sql = "select COUNT(ID) CountUnit  from  mesUnit  where PartID=" + S_PartID +
                            "  and StationID=" + List_Login.StationID +
                            "  and ProductionOrderID=" + I_POID;
            DataTable DT_CountUnit = public_.P_DataSet(S_Sql).Tables[0];

            Edt_ScanQuantity.Text = DT_CountUnit.Rows[0]["CountUnit"].ToString();
        }

        private void AddUnit()
        {
            //string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
            //DataTable DT = PartSelectSVC.GetUnit(S_PartID).Tables[0];
            //Grid_mesUnit.DataSource = DT;


            string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
            string S_StationID = List_Login.StationID.ToString();
            DataTable DT = PartSelectSVC.GetUnit2(S_PartID, S_StationID, I_POID.ToString()).Tables[0];
            Grid_mesUnit.DataSource = DT;

            //for (int i = 0; i < this.Grid_mesUnit.Columns.Count; i++)
            //{
            //    this.Grid_mesUnit.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //}
        }

        private void AddMachineHistory(string S_UnitID)
        {
            string S_Sql = @"select A.*,B.SN from
                                (select *  from mesMachineHistory)A
	                            JOIN (select *  from mesMachine)B ON A.MachineID=B.ID
                            where A.UnitID='"+ S_UnitID + "'";

            DataTable DT_Tool = public_.P_DataSet(S_Sql).Tables[0];

            Grid_Tool.DataSource = DT_Tool; 
        }

        //private Boolean GetIsRoute(int Scan_StationID, DataTable DT_Unit)
        //{
        //    Boolean B_Result = true;

        //    try
        //    {
        //        string S_PartID = DT_Unit.Rows[0]["PartID"].ToString();
        //        //最后扫描工序
        //        string S_StationID = DT_Unit.Rows[0]["StationID"].ToString();
        //        //获取此料工序路径
        //        DataTable DT_Route = PartSelectSVC.GetRoute(S_PartID).Tables[0];
        //        //当前扫描信息
        //        string S_Sql = "select *  from mesStation where ID='" + Scan_StationID + "'";
        //        DataTable DT_Scan_Station = public_.P_DataSet(S_Sql).Tables[0];
        //        int I_StationTypeID_Scan = Convert.ToInt32(DT_Scan_Station.Rows[0]["StationTypeID"].ToString());

        //        var v_Route_Sacn = from c in DT_Route.AsEnumerable()
        //                           where c.Field<int>("StationTypeID") == I_StationTypeID_Scan
        //                           select c;
        //        if (v_Route_Sacn.ToList().Count() > 0)
        //        {
        //            int I_Sequence_Scan = v_Route_Sacn.ToList()[0].Field<int>("Sequence");
        //            if (I_Sequence_Scan > 1)
        //            {
        //                //最后扫描信息
        //                S_Sql = "select *  from mesStation where ID='" + S_StationID + "'";
        //                DataTable DT_Station = public_.P_DataSet(S_Sql).Tables[0];
        //                int I_StationTypeID = Convert.ToInt32(DT_Station.Rows[0]["StationTypeID"].ToString());

        //                try
        //                {
        //                    var v_Route = from c in DT_Route.AsEnumerable()
        //                                  where c.Field<int>("StationTypeID") == I_StationTypeID
        //                                  select c;
        //                    int I_Sequence = v_Route.ToList()[0].Field<int>("Sequence");

        //                    if (I_Sequence >= I_Sequence_Scan)
        //                    {
        //                        public_.Add_Info_MSG(Edt_MSG, "此条码已过站！", "NG");
        //                        Edt_SN.SelectAll();
        //                        B_Result = false;
        //                    }
        //                    else
        //                    {
        //                        if (I_Sequence_Scan - 1 != I_Sequence)
        //                        {
        //                            public_.Add_Info_MSG(Edt_MSG, "上一站未扫描！", "NG");
        //                            Edt_SN.SelectAll();
        //                            B_Result = false;
        //                        }
        //                    }
        //                }
        //                catch
        //                {
        //                    public_.Add_Info_MSG(Edt_MSG, "上一站未扫描！", "NG");
        //                    Edt_SN.SelectAll();
        //                    B_Result = false;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            //MessageBox.Show("无此工序！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            public_.Add_Info_MSG(Edt_MSG, "无此工序！", "NG");
        //            Edt_SN.SelectAll();
        //            B_Result = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        public_.Add_Info_MSG(Edt_MSG, ex.ToString(), "NG");
        //        Edt_SN.SelectAll();
        //        B_Result = false;
        //    }
        //    return B_Result;
        //}

        private Boolean GetIsRoute(int Scan_StationID, DataTable DT_Unit)
        {
            Boolean B_Result = true;
            string S_PartID = DT_Unit.Rows[0]["PartID"].ToString();
            //最后扫描工序
            string S_StationID = DT_Unit.Rows[0]["StationID"].ToString();
            //获取此料工序路径
            DataTable DT_Route = PartSelectSVC.GetRoute(S_PartID,"", List_Login.LineID.ToString(), List_Login.StationTypeID).Tables[0];
            //当前扫描信息
            string S_Sql = "select *  from mesStation where ID='" + Scan_StationID + "'";
            DataTable DT_Scan_Station = public_.P_DataSet(S_Sql).Tables[0];
            //int I_StationTypeID_Scan =Convert.ToInt32(DT_Scan_Station.Rows[0]["StationTypeID"].ToString());

            //var v_Route_Sacn = from c in DT_Route.AsEnumerable()
            //              where c.Field<int>("StationTypeID") == I_StationTypeID_Scan
            //              select c;

            //改为按  StationID  来识别
            int I_StationID_Scan = Convert.ToInt32(DT_Scan_Station.Rows[0]["ID"].ToString());

            var v_Route_Sacn = from c in DT_Route.AsEnumerable()
                               where c.Field<int>("StationID") == I_StationID_Scan
                               select c;

            if (v_Route_Sacn.ToList().Count() > 0)
            {
                int I_Sequence_Scan = v_Route_Sacn.ToList()[0].Field<int>("Sequence");
                if (I_Sequence_Scan > 1)
                {
                    //最后扫描信息
                    S_Sql = "select *  from mesStation where ID='" + S_StationID + "'";
                    DataTable DT_Station = public_.P_DataSet(S_Sql).Tables[0];
                    //int I_StationTypeID = Convert.ToInt32(DT_Station.Rows[0]["StationTypeID"].ToString());
                    int I_StationID = Convert.ToInt32(DT_Station.Rows[0]["ID"].ToString());

                    try
                    {
                        //var v_Route = from c in DT_Route.AsEnumerable()
                        //              where c.Field<int>("StationTypeID") == I_StationTypeID
                        //              select c;
                        var v_Route = from c in DT_Route.AsEnumerable()
                                      where c.Field<int>("StationID") == I_StationID
                                      select c;
                        int I_Sequence = v_Route.ToList()[0].Field<int>("Sequence");

                        if (I_Sequence >= I_Sequence_Scan)
                        {
                            public_.Add_Info_MSG(Edt_MSG, "此条码已过站！", "NG");
                            Edt_SN.SelectAll();
                            B_Result = false;
                        }
                        else
                        {
                            if (I_Sequence_Scan - 1 != I_Sequence)
                            {
                                public_.Add_Info_MSG(Edt_MSG, "上一站未扫描！", "NG");
                                Edt_SN.SelectAll();
                                B_Result = false;
                            }
                        }
                    }
                    catch
                    {
                        public_.Add_Info_MSG(Edt_MSG, "上一站未扫描！", "NG");
                        Edt_SN.SelectAll();
                        B_Result = false;
                    }
                }
            }
            else
            {
                //MessageBox.Show("无此工序！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                public_.Add_Info_MSG(Edt_MSG, "无此工序！", "NG");
                Edt_SN.SelectAll();
                B_Result = false;
            }
            return B_Result;
        }


        string S_UnitID = "";
        private void Com_UnitState_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView DRV = Com_UnitState.SelectedItem as DataRowView;
            string S_UnitStateID = DRV["ID"].ToString().Trim();

            if (S_UnitStateID == "2")
            {
                Btn_Defect.Enabled = true;
                DefectForm v_DefectForm = new DefectForm();
                v_DefectForm.Show_DefectForm(v_DefectForm, Edt_DefectID);
                Lab_NG.Visible = true;
            }
            else
            {
                Btn_Defect.Enabled = false;
                Lab_NG.Visible = false;
            }
        }

        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //Edt_MSG.Text = "";
                string S_UnitState = Com_UnitState.Text.Trim();
                DataRowView DRV = Com_UnitState.SelectedItem as DataRowView;
                string S_UnitStateID = DRV["ID"].ToString().Trim();
                string S_DefectID = Edt_DefectID.Text.Trim();

                if (S_UnitStateID == "2")
                {
                    if (S_DefectID == "")
                    {
                        public_.Add_Info_MSG(Edt_MSG, " 确认此物料NG,请设置NG原因!", "NG");
                        Edt_SN.SelectAll();
                        return;
                    }

                    //if (MessageBox.Show("确认是: NG 吗？", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                    //{
                    //    Edt_SN.SelectAll();
                    //    //Edt_SN.Text = ""; 
                    //    return; 
                    //}
                }


                string S_SN = Edt_SN.Text.Trim();
                if (I_RouteSequence > 1)  //不是第一个工序时根据  SN  获取工单
                {
                    List<string> List_PO = public_.SnToPOID(S_SN);
                    string S_POID = List_PO[0];
                    if (S_POID == "" || S_POID == "0" )
                    {
                        public_.Add_Info_MSG(Edt_MSG, "此条码没有第一次过站记录！", "NG");
                        Edt_SN.SelectAll();
                        return;
                    }
                    Get_DTPO(S_POID);                   
                    Com_PO.Text =S_POID;
                }


                int I_PartID = Convert.ToInt32(DT_ProductionOrder.Rows[0]["PartID"].ToString());
               
                string S_Sql = "select * from mesSerialNumber where Value ='" + S_SN + "'";
                DataTable DT_SN = public_.P_DataSet(S_Sql).Tables[0];

                if (DT_SN.Rows.Count > 0)
                {
                    S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                    S_Sql = "select * from mesUnit where ID='" + S_UnitID + "'";
                    DataTable DT_Unit = public_.P_DataSet(S_Sql).Tables[0];
                    string S_StationID = DT_Unit.Rows[0]["StationID"].ToString();

                    if (DT_Unit.Rows[0]["UnitStateID"].ToString() == "2")
                    {
                        //MessageBox.Show("此条码已NG！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        public_.Add_Info_MSG(Edt_MSG, "此条码已NG！", "NG");
                        Edt_SN.SelectAll();
                        return;
                    }

                    ////////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////////
                    Boolean B_IsRoute = GetIsRoute(List_Login.StationID, DT_Unit);

                    if (B_IsRoute == true)
                    {
                        string S_PartID = DT_Unit.Rows[0]["PartID"].ToString();
                        if (S_PartID == I_PartID.ToString())
                        {
                            Edt_SN.Enabled = false;
                            Edt_ToolSN.Enabled = true;
                            Edt_ToolSN.Focus();
                            //Com_UnitState.Enabled = false;                            
                        }
                        else
                        {
                            //MessageBox.Show("此条码和工单不匹配！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            public_.Add_Info_MSG(Edt_MSG, "此条码和工单不匹配！", "NG");
                            Edt_SN.SelectAll();
                        }
                    }
                }
                else
                {
                    //MessageBox.Show("条码不存在！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    public_.Add_Info_MSG(Edt_MSG, "条码不存在！", "NG");
                    Edt_SN.SelectAll();
                }
            }
        }

        private void Edt_ToolSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //Edt_MSG.Text = ""; 

                string S_ToolSN = Edt_ToolSN.Text.Trim();

                string S_UnitState = Com_UnitState.Text.Trim();
                DataRowView DRV = Com_UnitState.SelectedItem as DataRowView;
                string S_UnitStateID = DRV["ID"].ToString().Trim();
                string S_DefectID = Edt_DefectID.Text.Trim();

                //工单 PartID
                string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();//产品料号

                string S_POID = DT_ProductionOrder.Rows[0]["ID"].ToString();
                string S_PO_StatusID = DT_ProductionOrder.Rows[0]["StatusID"].ToString();

                string S_SN = Edt_SN.Text.Trim();
                string S_Sql = "";

                if (S_SN != "")
                {
                    DataTable DT_Machine = public_.P_DataSet("select ID,SN  from  mesMachine  where  SN='" + S_ToolSN + "'").Tables[0];
                    if (DT_Machine.Rows.Count == 0)
                    {
                        public_.Add_Info_MSG(Edt_MSG, "设备条码不存在！", "NG");
                        Edt_SN.SelectAll();
                        return;
                    }

                    //获取此料工序路径
                    DataTable DT_Route = PartSelectSVC.GetRoute(S_PartID,"", List_Login.LineID.ToString(), List_Login.StationTypeID).Tables[0];
                    //var v_Route_Sacn = from c in DT_Route.AsEnumerable()
                    //                   where c.Field<int>("StationTypeID") == List_Login.StationTypeID 
                    //                   select c;
                    string S_RouteID = DT_Route.Rows[0]["ID"].ToString().Trim();
                    string S_StationTypeID = List_Login.StationTypeID.ToString();


                    //获取对应  Machine   数据
                    string S_RouteMap= Get_MachineRouteMap(S_ToolSN, S_PartID, S_RouteID, S_StationTypeID);

                    if (S_RouteMap != "Y")
                    {
                        if (S_RouteMap == "N")
                        {
                            public_.Add_Info_MSG(Edt_MSG, "设备条码不匹配！", "NG");
                            Edt_ToolSN.SelectAll();
                            return;
                        }
                        else if (S_RouteMap.Substring(0, 5) == "Error")
                        {
                            public_.Add_Info_MSG(Edt_MSG, S_RouteMap, "NG");
                            Edt_ToolSN.SelectAll();
                            return;
                        }
                    }
                    ////设备ID
                    //string S_MachineID = DT_Machine.Rows[0]["ID"].ToString().Trim();
                    ////设备料号
                    //string S_Machine_PartID = DT_Machine.Rows[0]["PartID"].ToString().Trim();  
                    ////设备群
                    //string S_MachineFamilyID=DT_Machine.Rows[0]["MachineFamilyID"].ToString().Trim();

                    //int I_RuningQuantity = Convert.ToInt32(DT_Machine.Rows[0]["RuningQuantity"].ToString());
                    //int I_MaxUseQuantity = Convert.ToInt32(DT_Machine.Rows[0]["MaxUseQuantity"].ToString());

                    //// 关联路径
                    //string S_MachineMap_RouteID= DT_MachineMap.Rows[0]["RouteID"].ToString().Trim();
                    ////关联工站
                    //string S_MachineMap_StationID= DT_MachineMap.Rows[0]["StationID"].ToString().Trim();   
                    ////关联工站类别
                    //string S_MachineMap_StationTypeID= DT_MachineMap.Rows[0]["StationTypeID"].ToString().Trim();

                    ////设备 SN
                    //string S_MachineSN= DT_MachineMap.Rows[0]["SN"].ToString().Trim();                   
                    ////关联产品料号
                    //string S_MachineMap_PartID = DT_MachineMap.Rows[0]["PartID"].ToString().Trim();
                    ////关联设备料号
                    //string S_MachineMap_MachinePartID=DT_MachineMap.Rows[0]["MachinePartID"].ToString().Trim();
                    ////关联设备群
                    //string S_MachineMap_MachineFamilyID=DT_MachineMap.Rows[0]["MachineFamilyID"].ToString().Trim();
                   
                    //if (S_PartID == S_MachineMap_PartID && S_RouteID == S_MachineMap_RouteID && S_StationTypeID == S_MachineMap_StationTypeID 
                    //    && (S_ToolSN==S_MachineSN || S_Machine_PartID== S_MachineMap_MachinePartID || S_MachineFamilyID== S_MachineMap_MachineFamilyID)
                    //    )
                    {
                        //过站
                        ImesUnitSVCClient mesUnitSVC = ImesUnitFactory.CreateServerClient();
                        ImesHistorySVCClient mesHistorySVC = ImesHistoryFactory.CreateServerClient();
                        DataTable DT_UPUnitID = Get_UnitID(S_SN);

                        mesUnit v_mesUnit = new mesUnit();
                        v_mesUnit.ID = Convert.ToInt32(DT_UPUnitID.Rows[0]["UnitID"].ToString());
                        v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                        v_mesUnit.StationID = List_Login.StationID;
                        v_mesUnit.EmployeeID = List_Login.EmployeeID;
                        v_mesUnit.LastUpdate = DateTime.Now;
                        v_mesUnit.ProductionOrderID = Convert.ToInt32(S_POID);
                        //修改 Unit
                        string S_UpdateUnit = mesUnitSVC.Update(v_mesUnit);
                        if (S_UpdateUnit.Substring(0, 1) == "E")
                        {
                            MessageBox.Show(S_UpdateUnit, "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
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
                            v_mesHistory.PartID = Convert.ToInt32(S_PartID);
                            v_mesHistory.LooperCount = 1;
                            int I_InsertHistory = mesHistorySVC.Insert(v_mesHistory);

                            ///////////////////////////////////////////////////////////////////////////////
                            ///////////////////////////////////////////////////////////////////////////////
                            S_Sql = "insert into mesMachineHistory(UnitID,MachineID,EnterTime) Values(" +
                                   "'" + v_mesUnit.ID + "'," +
                                   "'" + DT_Machine.Rows[0]["ID"].ToString() + "'," +
                                   "'" + DateTime.Now.ToString() + "'" +
                                   ")";
                            public_.ExecSql(S_Sql);

                            //修改工单状态                           
                            if (S_PO_StatusID == "1")
                            {
                                //S_Sql = "Update mesProductionOrder set StatusID=2,LastUpdate='" + DateTime.Now.ToString() +
                                //        "' where ID='" + S_POID + "'";
                                //public_.ExecSql(S_Sql);
                            }

                            //////////////////////////  Defect ///////////////////////////////////////////
                            //////////////////////////////////////////////////////////////////////////////
                            string[] Array_Defect = S_DefectID.Split(';');
                            ImesUnitDefectSVCClient mesUnitDefectSVC = ImesUnitDefectFactory.CreateServerClient();
                            if (S_UnitStateID == "2")
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
                            /////////////////////////////////////////////////////////////////////////
                            /////////////////////////////////////////////////////////////////////////

                            Edt_SN.Text = "";
                            Edt_SN.Enabled = true;
                            Edt_SN.Focus();
                            Edt_DefectID.Text = ""; 

                            Edt_ToolSN.Text = "";
                            Edt_ToolSN.Enabled = false;
                            List_ToolSN.Items.Clear();

                            AddUnit();
                            GetScanQuantity();
                            public_.Grid_NGColor(Grid_mesUnit);

                            public_.Add_Info_MSG(Edt_MSG, "OK！", "OK");
                        }
                    }
                    //else
                    //{
                    //    //MessageBox.Show("设备不匹配，请确认！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    public_.Add_Info_MSG(Edt_MSG, "设备条码不匹配！", "NG");
                    //    Edt_ToolSN.SelectAll();
                    //    Edt_ToolSN.Focus();
                    //}

                }
                else
                {
                    //MessageBox.Show("主条码不能为空！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    public_.Add_Info_MSG(Edt_MSG, "主条码不能为空！", "NG");
                    Edt_SN.SelectAll();
                    Edt_SN.Focus(); 
                }
            }
        }

        private DataTable Get_UnitID(string S_SN)
        {
            string S_Sql = @"select A.*,B.PartID,B.StationID,B.UnitStateID  from 
	                            (select * from mesSerialNumber) A 
	                            JOIN (select u.ID,u.PartID,u.StationID,u.UnitStateID  from mesUnit u)B on A.UnitID=B.ID
                            where Value = '" + S_SN + "'";
            DataTable DT = public_.P_DataSet(S_Sql).Tables[0];
            return DT;
        }

        private string Get_MachineRouteMap(string S_ToolSN, string S_ProductPartID, string S_RouteID, string S_StationTypeID)
        {
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            string S_Result = PartSelectSVC.Get_MachineRouteMap( S_ToolSN,  S_ProductPartID,  S_RouteID,  S_StationTypeID);
            PartSelectSVC.Close();
            return S_Result;
        }

        private void Grid_mesUnit_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string S_ID = Grid_mesUnit.CurrentRow.Cells["ID"].Value.ToString();
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            DataTable DT = PartSelectSVC.GetHistory(S_ID).Tables[0];

            Grid_mesHistory.DataSource = DT;           
            public_.Grid_NGColor(Grid_mesHistory);
            AddMachineHistory(S_ID);
            
            try
            {
                DataTable DT_Defect = PartSelectSVC.GetmesUnitDefect(S_ID).Tables[0];
                Grid_Defect.DataSource = DT_Defect;
            }
            catch
            {
                Grid_Defect.DataSource = null;
            }

            PartSelectSVC.Close();
            //for (int i = 0; i < this.Grid_mesHistory.Columns.Count; i++)
            //{
            //    this.Grid_mesHistory.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //}
        }

        private void Com_PO_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Edt_SN.Enabled = true;

                DataRowView DRV_PO = Com_PO.SelectedItem as DataRowView;
                string S_POID = DRV_PO["ID"].ToString();

                Get_DTPO(S_POID);
            }
            catch
            { }
        }

        private void Get_DTPO(string S_POID)
        {
            I_POID = Convert.ToInt32(S_POID); 
            string S_Sql = "select *  from mesProductionOrder where ID='" + S_POID + "'";
            DT_ProductionOrder = public_.P_DataSet(S_Sql).Tables[0];

            AddUnit();
            //AddUnitComponent();

            Edt_OrderQuantity.Text = DT_ProductionOrder.Rows[0]["OrderQuantity"].ToString();
            GetScanQuantity();
            public_.Grid_NGColor(Grid_mesUnit);

            Grid_Tool.DataSource = null;
            Grid_mesHistory.DataSource = null;
        }



        /// <summary>
        /// 解决Combobox 第一次无法更新grid  背景颜色问题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_GridColor_Tick(object sender, EventArgs e)
        {
            public_.Grid_NGColor(Grid_mesUnit);
            timer_GridColor.Enabled = false;  
        }

        private void Grid_mesUnit_Sorted(object sender, EventArgs e)
        {
            timer_GridColor.Enabled = true;
        }

        private void Btn_Defect_Click(object sender, EventArgs e)
        {
            DefectForm v_DefectForm = new DefectForm();
            v_DefectForm.Show_DefectForm(v_DefectForm, Edt_DefectID);
        }
    }
}
