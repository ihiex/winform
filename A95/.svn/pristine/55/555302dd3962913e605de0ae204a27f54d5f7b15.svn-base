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
using App.MyMES.luDefectService;

namespace App.MyMES
{
    public partial class LabelForm : Form
    {
        Public_ public_ = new Public_();
        DataTable DT_ProductionOrder = new DataTable();
        DataTable DT_ProductStructure = new DataTable();

        LoginList List_Login = new LoginList();
        PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();

        int I_RouteSequence = -9;  //当前工序顺序 
        int I_POID = 0;

        public LabelForm()
        {
            InitializeComponent();
        }

        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            List_Login = this.Tag as LoginList;

            public_.AddStationType(Com_StationType);
            public_.AddLine(Com_Line);
           // public_.AddmesUnitState(Com_UnitState);

            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_LineID = List_Login.LineID.ToString();
            public_.AddStation(Com_Station,S_LineID);

            Com_StationType.Text = S_StationTypeID;
            Com_Line.Text = S_LineID;
            Com_Station.Text = List_Login.StationID.ToString();

            public_.AddPOAll(Com_PO,"",S_LineID);
            //Com_PO.Text = List_Login.POID.ToString();


            //Edt_MSG.Text = "";
            Edt_SN.Text = "";
            Edt_SN.Enabled = true;

            //Com_UnitState.Enabled = true;
            timer_GridColor.Enabled = true;
        }

        private void LabelForm_Load(object sender, EventArgs e)
        {
            Btn_Refresh_Click(sender, e);

            string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
            I_RouteSequence = public_.RouteSequence(S_PartID, List_Login.StationID, List_Login.LineID.ToString());

            Com_PO.Enabled = true;
            if (I_RouteSequence > 1)
            {
                Com_PO.BackColor = Color.Yellow;
                Com_PO.Enabled = false;
            }
        }

        private void Com_PO_SelectedIndexChanged(object sender, EventArgs e)
        {
            Edt_SN.Enabled = true;

            DataRowView DRV_PO = Com_PO.SelectedItem as DataRowView;
            string S_POID = DRV_PO["ID"].ToString();

            Get_DTPO(S_POID);
        }

        private void Get_DTPO(string S_POID)
        {
            I_POID = Convert.ToInt32(S_POID);  
            string S_Sql = "select *  from mesProductionOrder where ID='" + S_POID + "'";
            DT_ProductionOrder = public_.P_DataSet(S_Sql).Tables[0];

            AddUnit();

            Edt_OrderQuantity.Text = DT_ProductionOrder.Rows[0]["OrderQuantity"].ToString();
            GetScanQuantity();
            public_.Grid_NGColor(Grid_mesUnit);

            Grid_mesHistory.DataSource = null;
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

        private void AddUnit()
        {
            //string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
            //DataTable DT = PartSelectSVC.GetUnit(S_PartID).Tables[0];
            //Grid_mesUnit.DataSource = DT;

            string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
            string S_StationID = List_Login.StationID.ToString();
            DataTable DT = PartSelectSVC.GetUnit2(S_PartID, S_StationID, I_POID.ToString()).Tables[0];
            Grid_mesUnit.DataSource = DT;
        }


        private Boolean GetIsRoute(int Scan_StationID, string S_PartID)
        {
            Boolean B_Result = true;
            
            //获取此料工序路径
            DataTable DT_Route = PartSelectSVC.GetRoute(S_PartID,"", List_Login.LineID.ToString(), Scan_StationID).Tables[0];
            //当前扫描信息
            string S_Sql = "select *  from mesStation where ID='" + Scan_StationID + "'";
            DataTable DT_Scan_Station = public_.P_DataSet(S_Sql).Tables[0];
            //int I_StationTypeID_Scan = Convert.ToInt32(DT_Scan_Station.Rows[0]["StationTypeID"].ToString());
            //var v_Route_Sacn = from c in DT_Route.AsEnumerable()
            //                   where c.Field<int>("StationTypeID") == I_StationTypeID_Scan
            //                   select c;

            int I_StationID_Scan = Convert.ToInt32(DT_Scan_Station.Rows[0]["ID"].ToString());
            var v_Route_Sacn = from c in DT_Route.AsEnumerable()
                               where c.Field<int>("StationID") == I_StationID_Scan
                               select c;

            if (v_Route_Sacn.ToList().Count() > 0)
            {

            }
            else
            {
                public_.Add_Info_MSG(Edt_MSG, "无此工序！", "NG");
                Edt_SN.SelectAll();
                B_Result = false;
            }
            return B_Result;
        }


        private Boolean GetIsRoute(int Scan_StationID, DataTable DT_Unit)
        {
            Boolean B_Result = true;
            string S_PartID = DT_Unit.Rows[0]["PartID"].ToString();
            //最后扫描工序
            string S_StationID = DT_Unit.Rows[0]["StationID"].ToString();
            //获取此料工序路径
            DataTable DT_Route = PartSelectSVC.GetRoute(S_PartID,"", List_Login.LineID.ToString(), Scan_StationID).Tables[0];
            //当前扫描信息
            string S_Sql = "select *  from mesStation where ID='" + Scan_StationID + "'";
            DataTable DT_Scan_Station = public_.P_DataSet(S_Sql).Tables[0];
            //int I_StationTypeID_Scan =Convert.ToInt32(DT_Scan_Station.Rows[0]["StationTypeID"].ToString());

            //var v_Route_Sacn = from c in DT_Route.AsEnumerable()
            //              where c.Field<int>("StationTypeID") == I_StationTypeID_Scan
            //              select c;

            //改为按  StationID  来识别
            int I_StationID_Scan = Convert.ToInt32(DT_Scan_Station.Rows[0]["StationID"].ToString());

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
                    int I_StationID = Convert.ToInt32(DT_Station.Rows[0]["StationID"].ToString());

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



        //string S_UnitID = "";
        private void Edt_ScanSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //Edt_MSG.Text = "";
                string S_UnitState = Com_UnitState.Text.Trim();
                DataRowView DRV = Com_UnitState.SelectedItem as DataRowView;
                string S_UnitStateID = DRV["ID"].ToString().Trim();

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
                    Com_PO.Text = S_POID;
                }


                int I_PartID = Convert.ToInt32(DT_ProductionOrder.Rows[0]["PartID"].ToString());
                int I_POID= Convert.ToInt32(DT_ProductionOrder.Rows[0]["ID"].ToString());
                //string S_SN = Edt_SN.Text.Trim();


                ImesUnitSVCClient mesUnitSVC = ImesUnitFactory.CreateServerClient();
                ImesSerialNumberSVCClient mesSerialNumberSVC = ImesSerialNumberFactory.CreateServerClient();
                ImesHistorySVCClient mesHistorySVC = ImesHistoryFactory.CreateServerClient();

                IEnumerable<mesSerialNumber> List_mesSerialNumber = mesSerialNumberSVC.ListAll(" Where Value='" + S_SN.Trim() + "'");
                Boolean B_NG = true;

                string S_DefectID = Edt_DefectID.Text.Trim();

                if (List_mesSerialNumber.ToList().Count > 0)
                {
                    public_.Add_Info_MSG(Edt_MSG, "条码已过站!", "NG");
                    Edt_SN.SelectAll();
                }
                else
                {
                    Boolean B_IsRoute = false; 

                    //产品第一次贴标签以后的贴标工站，判断前段工序是否完成
                    if (List_Login.StationType == "Labelling2")  //已取消使用
                    {                       
                        //string S_Sql = "select * from mesSerialNumber where Value ='" + S_SN + "'";
                        //DataTable DT_SN = public_.P_DataSet(S_Sql).Tables[0];

                        //if (DT_SN.Rows.Count > 0)
                        //{
                        //    S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                        //    S_Sql = "select * from mesUnit where ID='" + S_UnitID + "'";
                        //    DataTable DT_Unit = public_.P_DataSet(S_Sql).Tables[0];
                        //    string S_StationID = DT_Unit.Rows[0]["StationID"].ToString();

                        //    if (DT_Unit.Rows[0]["UnitStateID"].ToString() == "2")
                        //    {
                        //        //MessageBox.Show("此条码已NG！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //        public_.Add_Info_MSG(Edt_MSG, "此条码已NG！", "NG");
                        //        Edt_SN.SelectAll();
                        //        return;
                        //    }

                        //    B_IsRoute = GetIsRoute(List_Login.StationID, DT_Unit);
                        //}
                        //else
                        //{
                        //    B_IsRoute = GetIsRoute(List_Login.StationID, I_PartID.ToString());
                        //}
                    }
                    else
                    {
                        B_IsRoute = GetIsRoute(List_Login.StationID, I_PartID.ToString());
                    }

                    if (B_IsRoute == true) //判断路径信息
                    {
                        if (S_SN.Trim().Length >= 5)  //具体条码格式以后再处理
                        {
                            if (S_UnitStateID == "2")
                            {
                                if (S_DefectID == "")
                                {
                                    public_.Add_Info_MSG(Edt_MSG, " 确认此物料NG,请设置NG原因!", "NG");
                                    Edt_SN.SelectAll();
                                    B_NG = false;
                                }

                                //if (MessageBox.Show("确认此物料是: NG ？", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                                //{
                                //    B_NG = false;
                                //    Edt_SN.SelectAll();
                                //}
                            }

                            if (B_NG == true)
                            {
                                mesUnit v_mesUnit = new mesUnit();

                                v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                                v_mesUnit.StatusID = 1;
                                v_mesUnit.StationID = List_Login.StationID;
                                v_mesUnit.EmployeeID = List_Login.EmployeeID;
                                v_mesUnit.CreationTime = DateTime.Now;
                                v_mesUnit.LastUpdate = DateTime.Now;
                                v_mesUnit.PanelID = 0;
                                v_mesUnit.LineID = List_Login.LineID;
                                v_mesUnit.ProductionOrderID = I_POID;
                                v_mesUnit.RMAID = 0;
                                v_mesUnit.PartID = I_PartID;
                                v_mesUnit.LooperCount = 1;

                                string S_InsertUnit = mesUnitSVC.Insert(v_mesUnit);
                               
                                if (S_InsertUnit.Substring(0, 1) != "E")
                                {
                                    mesSerialNumber v_mesSerialNumber = new mesSerialNumber();

                                    v_mesSerialNumber.UnitID = Convert.ToInt32(S_InsertUnit);
                                    v_mesSerialNumber.SerialNumberTypeID = 1;
                                    v_mesSerialNumber.Value = S_SN;
                                    int I_InsertSN = mesSerialNumberSVC.Insert(v_mesSerialNumber);
                                    ///////////////////
                                    public_.Grid_NGColor(Grid_mesUnit);

                                    mesHistory v_mesHistory = new mesHistory();

                                    v_mesHistory.UnitID = Convert.ToInt32(S_InsertUnit);
                                    v_mesHistory.UnitStateID = v_mesUnit.UnitStateID;
                                    v_mesHistory.EmployeeID = List_Login.EmployeeID;
                                    v_mesHistory.StationID = List_Login.StationID;
                                    v_mesHistory.EnterTime = DateTime.Now;
                                    v_mesHistory.ExitTime = DateTime.Now;
                                    v_mesHistory.ProductionOrderID = 0;
                                    v_mesHistory.PartID = I_PartID;
                                    v_mesHistory.LooperCount = 1;
                                    int I_InsertHistory = mesHistorySVC.Insert(v_mesHistory);

                                    //////////////////////////  Defect ///////////////////////////////////////////
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

                                                    v_mesUnitDefect.UnitID = Convert.ToInt32(S_InsertUnit);
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

                                    //关闭对象
                                    mesSerialNumberSVC.Close();
                                    mesUnitSVC.Close();
                                    mesHistorySVC.Close();
                                    mesUnitDefectSVC.Close();

                                    Edt_SN.Text = "";
                                    Edt_SN.Enabled = true;
                                    Edt_SN.Focus();
                                    Edt_DefectID.Text = "";

                                    AddUnit();
                                    GetScanQuantity();
                                    public_.Grid_NGColor(Grid_mesUnit);

                                    public_.Add_Info_MSG(Edt_MSG, "OK！", "OK");
                                }
                            }
                        }
                        else
                        {
                            public_.Add_Info_MSG(Edt_MSG, "条码格式错误!", "NG");
                            Edt_SN.SelectAll();
                        }
                    }
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

        private void Grid_mesUnit_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string S_ID = Grid_mesUnit.CurrentRow.Cells["ID"].Value.ToString();
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            DataTable DT = PartSelectSVC.GetHistory(S_ID).Tables[0];

            Grid_mesHistory.DataSource = DT;
            public_.Grid_NGColor(Grid_mesHistory);

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
        }

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

