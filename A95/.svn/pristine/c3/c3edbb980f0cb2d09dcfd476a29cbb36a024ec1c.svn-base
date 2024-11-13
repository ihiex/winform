using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.Model;
using App.MyMES.PartSelectService;
using App.MyMES.mesUnitService;
using App.MyMES.mesSerialNumberService;
using App.MyMES.mesHistoryService;
using App.MyMES.mesProductionOrderService;
using App.MyMES.mesProductStructureService;
using App.MyMES.mesUnitComponentService;
using App.MyMES.mesUnitDefectService;
using App.MyMES.mesUnitDetailService;
using System.Text.RegularExpressions;
using App.MyMES.mesPartService;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using App.MyMES.mesRouteDetailService;
using App.MyMES.mesMachineService;
using App.MyMES.mesEmployeeService;

namespace App.MyMES
{
    public partial class ModUnitState_Form : DevExpress.XtraEditors.XtraForm
    {
        public Public_ public_ = new Public_();
        public DataTable DT_ProductionOrder = new DataTable();
        public DataTable DT_ProductStructure = new DataTable();
        public LoginList List_Login = new LoginList();
        public string Batch_Pattern = string.Empty;
        public DataTable DtDefect = new DataTable();
        public string DefectChar = string.Empty;

        public int I_RouteSequence = -9;  //当前工序顺序 
        public int I_POID = 0;
        public string S_DefectTypeID = "";
        string S_StationID = "";
        string S_INIStationTypeID = "";
        string S_INIS_UnitStateID = "";

        public PartSelectSVCClient PartSelectSVC;
        public ImesUnitSVCClient mesUnitSVC;
        public ImesSerialNumberSVCClient mesSerialNumberSVC;
        public ImesHistorySVCClient mesHistorySVC;
        public ImesUnitComponentSVCClient mesUnitComponentSVC;
        public ImesUnitDefectSVCClient mesUnitDefectSVC;
        public ImesPartSVCClient mesPartSVC;
        public ImesProductionOrderSVCClient mesProductionOrderSVC;
        public ImesUnitDetailSVCClient mesUnitDetailSVC;
        public ImesProductStructureSVCClient mesProductStructureSVC;
        public ImesRouteDetailSVCClient mesRouteDetailSVC;
        public ImesMachineSVCClient mesMachineSVC;
        public ImesEmployeeSVCClient mesEmployeeSVC;

        public ModUnitState_Form()
        {
            InitializeComponent();
        }

        private void LoadControls()
        {
            try
            {
                PartSelectSVC = PartSelectFactory.CreateServerClient();
                mesUnitSVC = ImesUnitFactory.CreateServerClient();
                mesSerialNumberSVC = ImesSerialNumberFactory.CreateServerClient();
                mesHistorySVC = ImesHistoryFactory.CreateServerClient();
                mesUnitComponentSVC = ImesUnitComponentFactory.CreateServerClient();
                mesUnitDefectSVC = ImesUnitDefectFactory.CreateServerClient();
                mesPartSVC = ImesPartFactory.CreateServerClient();
                mesProductionOrderSVC = ImesProductionOrderFactory.CreateServerClient();
                mesUnitDetailSVC = ImesUnitDetailFactory.CreateServerClient();
                mesProductStructureSVC = ImesProductStructureFactory.CreateServerClient();
                mesRouteDetailSVC = ImesRouteDetailFactory.CreateServerClient();
                mesMachineSVC = ImesMachineFactory.CreateServerClient();
                mesEmployeeSVC = ImesEmployeeFactory.CreateServerClient();
                // lblFrmName.Text = "Carrier Link RawMaterial Batch";
                //SplitControlFrm.Panel2.Width = SplitControlFrm.Panel2.Width / 5;
            }
            catch (Exception ex)
            {
                //MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }

        }

        #region 界面加载
        public virtual void FrmBase_Load(object sender, EventArgs e)
        {
            LoadControls();
            Btn_Refresh_Click(null, null);

            mesEmployee V_mesEmployee = mesEmployeeSVC.Get(List_Login.EmployeeID);
            if (V_mesEmployee.PermissionId != 1)
            {
                PartEdtStatus(false);
                Btn_Refresh.Enabled = false;
            }

            string S_Path = Application.StartupPath;
            MyINI myINI = new MyINI(S_Path + "\\MES_Main.ini");

            string S_PartFamilyTypeID = myINI.ReadValue("ModUnitState", "PartFamilyTypeID");
            string S_PartFamilyID = myINI.ReadValue("ModUnitState", "PartFamilyID");
            string S_PartID = myINI.ReadValue("ModUnitState", "PartID");
            string S_Route = myINI.ReadValue("ModUnitState", "Route");
            S_StationID=myINI.ReadValue("ModUnitState", "StationID");
            S_INIStationTypeID=myINI.ReadValue("ModUnitState", "StationTypeID");
            S_INIS_UnitStateID=myINI.ReadValue("ModUnitState", "UnitStateID");

            if (S_PartFamilyTypeID != "") { Com_PartFamilyType.EditValue = S_PartFamilyTypeID; }
            if (S_PartFamilyID != "") { Com_PartFamily.EditValue = S_PartFamilyID; }
            if (S_PartID != "") { Com_Part.EditValue = S_PartID; }
            if (S_Route != "") { Com_Route.EditValue = S_Route; }
            if (S_StationID != "")
            {
                Com_Station.EditValue = S_StationID;
            }
        }

        private void Com_PartFamilyType_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartFamilyTypeID = Com_PartFamilyType.EditValue.ToString();
                public_.AddPartFamily(Com_PartFamily, S_PartFamilyTypeID, Grid_PartFamily);

                S_DefectTypeID = public_.GetDefectTypeID(Com_PartFamilyType.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void Com_PartFamily_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                public_.AddPart(Com_Part, S_PartFamilyID, Grid_Part);
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void ListBoxRouteDetail_DrawItem(object sender, DevExpress.XtraEditors.ListBoxDrawItemEventArgs e)
        {
            throw new NotImplementedException();
        }

        public class TreeKeyObj
        {
            public string KeyValue { set; get; }
            public string KeyName1 { set; get; }
            public string KeyName2 { set; get; }
            public string FiterName { set; get; }
            public string FiterValue { set; get; }
            public int Level { set; get; }
        }

        public void SetDataSetToTree(TreeList treeList, TreeListNode ParentNode2, TreeKeyObj treeKeyObj, DataTable dataTable)
        {
            if (treeKeyObj.Level <= 0)
            {
                treeList.Nodes.Clear();
            }

            List<DataRow> DataRowList = null;
            if (dataTable.Columns.Contains(treeKeyObj.FiterName))
            {
                DataRowList = dataTable.Rows.OfType<DataRow>().Where<DataRow>(P => P[treeKeyObj.FiterName].ToString() == treeKeyObj.FiterValue).ToList();
            }

            if (DataRowList != null)
            {
                foreach (DataRow dataRow in DataRowList)
                {
                    string NodeStr = dataRow[treeKeyObj.KeyName2].ToString();
                    TreeListNode ParentNode = treeList.AppendNode(new object[] { NodeStr }, ParentNode2);

                    ParentNode.Tag = dataRow[treeKeyObj.KeyValue].ToString();
                    ParentNode.TreeList.Tag = "Child";
                    ParentNode.StateImageIndex = treeKeyObj.Level;
                    TreeKeyObj ChildKey = new TreeKeyObj();
                    ChildKey.FiterName = treeKeyObj.FiterName;
                    ChildKey.FiterValue = dataRow[treeKeyObj.KeyValue].ToString();
                    ChildKey.KeyName1 = treeKeyObj.KeyName1;
                    ChildKey.KeyName2 = treeKeyObj.KeyName2;
                    ChildKey.KeyValue = treeKeyObj.KeyValue;
                    ChildKey.Level = treeKeyObj.Level + 1;
                    SetDataSetToTree(treeList, ParentNode, ChildKey, dataTable);
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
            GrpControlInputData.Enabled = !B_Status;
            Btn_ConfirmPO.Enabled = B_Status;
            Com_Route.Enabled = B_Status;
            Com_Station.Enabled = B_Status;
        }
        #endregion


        public virtual void Btn_Refresh_Click(object sender, EventArgs e)
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
                mesUnitDetailSVC.Close();
                mesProductStructureSVC.Close();
                mesRouteDetailSVC.Close();
                mesMachineSVC.Close();
                mesEmployeeSVC.Close();

                PartSelectSVC = PartSelectFactory.CreateServerClient();
                mesUnitSVC = ImesUnitFactory.CreateServerClient();
                mesSerialNumberSVC = ImesSerialNumberFactory.CreateServerClient();
                mesHistorySVC = ImesHistoryFactory.CreateServerClient();
                mesUnitComponentSVC = ImesUnitComponentFactory.CreateServerClient();
                mesUnitDefectSVC = ImesUnitDefectFactory.CreateServerClient();
                mesPartSVC = ImesPartFactory.CreateServerClient();
                mesProductionOrderSVC = ImesProductionOrderFactory.CreateServerClient();
                mesUnitDetailSVC = ImesUnitDetailFactory.CreateServerClient();
                mesProductStructureSVC = ImesProductStructureFactory.CreateServerClient();
                mesRouteDetailSVC = ImesRouteDetailFactory.CreateServerClient();
                mesMachineSVC = ImesMachineFactory.CreateServerClient();
                mesEmployeeSVC = ImesEmployeeFactory.CreateServerClient();

                List_Login = this.Tag as LoginList;
                string S_StationTypeID = List_Login.StationTypeID.ToString();
                string S_LineID = List_Login.LineID.ToString();
                public_.AddPartFamilyType(Com_PartFamilyType, Grid_PartFamilyType);
                //public_.AddluUnitStatus(Com_luUnitStatus, Grid_luUnitStatus);
                string S_PartID = Com_Part.EditValue.ToString();
                //public_.AddPOAll(Com_PO, S_PartID, List_Login.LineID.ToString(), Grid_PO);
                PartEdtStatus(true);
            }
            catch (Exception ex)
            {
                //MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }


        /// <summary>
        /// 隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHide_Click(object sender, EventArgs e)
        {
            this.btnHide.Visible = false;
            this.btnShow.Visible = true;
            SplitControlFrm.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShow_Click(object sender, EventArgs e)
        {
            this.btnShow.Visible = false;
            this.btnHide.Visible = true;
            SplitControlFrm.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;
        }

        public virtual void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            try
            {
                if (Com_PartFamilyType.EditValue == null || string.IsNullOrEmpty(Com_PartFamilyType.Text.ToString()))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20115", "NG", List_Login.Language);
                    return;
                }
                if (Com_PartFamily.EditValue == null || string.IsNullOrEmpty(Com_PartFamily.Text.ToString()))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20116", "NG", List_Login.Language);
                    return;
                }
                if (Com_Part.EditValue == null || string.IsNullOrEmpty(Com_Part.Text.ToString()))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20117", "NG", List_Login.Language);
                    return;
                }
                if (!string.IsNullOrEmpty(Com_Part.EditValue.ToString()))
                {
                    int I_PartID = Convert.ToInt32(Com_Part.EditValue.ToString());
                    DataSet dataSet = PartSelectSVC.GetmesPartDetail(I_PartID, "Batch_Pattern");
                    if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        Batch_Pattern = dataSet.Tables[0].Rows[0]["Content"].ToString();
                    }
                    else
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20025", "NG", List_Login.Language, new string[] { Com_Part.Text.ToString() });
                        return;
                    }
                }

                DataSet dsStructure = mesProductStructureSVC.GetBOMStructure(Com_PartFamily.EditValue.ToString(),
                    Com_Part.EditValue.ToString(), "");
                if (dsStructure != null && dsStructure.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = dsStructure.Tables[0];
                    treeListColBom.Caption = dt.Rows[0]["ParentPartDescription"].ToString();
                    TreeKeyObj treeKeyObj = new TreeKeyObj();
                    treeKeyObj.FiterName = "ParentPartID";
                    treeKeyObj.FiterValue = Com_Part.EditValue.ToString();
                    treeKeyObj.KeyName1 = "PartID";
                    treeKeyObj.KeyName2 = "PartDescription";
                    treeKeyObj.KeyValue = "PartID";
                    SetDataSetToTree(TreeModule, null, treeKeyObj, dt);
                }

                DataSet dsRouteDetail = mesRouteDetailSVC.GetRouteDetail(List_Login.LineID.ToString(),
                                            Com_Part.EditValue.ToString(), Com_PartFamily.EditValue.ToString(),"null");
                if (dsRouteDetail != null && dsRouteDetail.Tables[0].Rows.Count > 0)
                {
                    //listBoxRouteDetail.Items.Clear();
                    //int index = 0;
                    //int selectIndex = 0;
                    //foreach (DataRow dr in dsRouteDetail.Tables[0].Rows)
                    //{
                    //    string StationTypeName = string.Empty;
                    //    StationTypeName = dr["Sequence"].ToString() + "  " + dr["Description"].ToString();
                    //    if (List_Login.StationTypeID.ToString() == dr["StationTypeID"].ToString())
                    //    {
                    //        StationTypeName = "<color=Red>" + StationTypeName + "</color>";
                    //        selectIndex = index;
                    //    }
                    //    index++;
                    //    listBoxRouteDetail.Items.Add(StationTypeName);
                    //    listBoxRouteDetail.SelectedIndex = selectIndex + 3;
                    //    listBoxRouteDetail.SelectedIndex = selectIndex;
                    //}


                    DataSet DS_RouteData = PartSelectSVC.GetRouteData(List_Login.LineID.ToString(), Com_Part.EditValue.ToString(),
                        Com_PartFamily.EditValue.ToString(), "null");
                    string S_RouteType = DS_RouteData.Tables[0].Rows[0]["RouteType"].ToString();


                    listBoxRouteDetail.Items.Clear();
                    int index = 0;
                    int selectIndex = 0;
                    GrpControlRouteDetail.Text = DS_RouteData.Tables[0].Rows[0]["Description"].ToString();

                    if (S_RouteType == "1")
                    {
                        string result = string.Empty;
                        string routID = DS_RouteData.Tables[0].Rows[0]["ID"].ToString();
                        DataSet dsRoute = PartSelectSVC.uspCallProcedure("uspGetRouteData", routID, "", "",
                                                                "", "", List_Login.StationTypeID.ToString(), ref result);

                        if (dsRoute == null || dsRoute.Tables.Count <= 0)
                        {
                            return;
                        }
                        Grid_RouteData.Visible = true;
                        DataColumn keyColumn = dsRoute.Tables[0].Columns["ID"];         //主键
                        DataColumn foreignColumn = dsRoute.Tables[1].Columns["ParentID"];    //外键
                        dsRoute.Relations.Add("", keyColumn, foreignColumn);
                        Grid_RouteData.DataSource = dsRoute.Tables[0];
                        Grid_RouteDataView.OptionsDetail.ShowDetailTabs = false;
                        Grid_RouteData.UseEmbeddedNavigator = false;
                        Grid_RouteDataView.OptionsDetail.AllowExpandEmptyDetails = true;

                        int foundHandle = Grid_RouteDataView.LocateByDisplayText(0, Grid_RouteDataView.Columns["StationTypeID"],
                            List_Login.StationTypeID.ToString());
                        Grid_RouteDataView.FocusedRowHandle = foundHandle;
                        //Grid_RouteDataView.ExpandGroupLevel(0);
                    }
                    else
                    {
                        Grid_RouteData.Visible = false;
                        //DataSet dsRouteDetail = mesRouteDetailSVC.GetRouteDetail(List_Login.LineID.ToString(),
                        //    Com_Part.EditValue.ToString(), Com_PartFamily.EditValue.ToString(), Com_PO.EditValue.ToString());
                        foreach (DataRow dr in dsRouteDetail.Tables[0].Rows)
                        {
                            string StationTypeName = string.Empty;
                            StationTypeName = dr["Sequence"].ToString() + "  " + dr["Description"].ToString();
                            if (List_Login.StationTypeID.ToString() == dr["StationTypeID"].ToString())
                            {
                                StationTypeName = "<color=Red>" + StationTypeName + "</color>";
                                selectIndex = index;
                            }
                            index++;
                            listBoxRouteDetail.Items.Add(StationTypeName);
                            listBoxRouteDetail.SelectedIndex = selectIndex + 3;
                            listBoxRouteDetail.SelectedIndex = selectIndex;
                        }
                    }

                }

                PartEdtStatus(false);
                SaveSeting();
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }


        private void SaveSeting()
        {
            try
            {
                string S_Path = Application.StartupPath;
                MyINI myINI = new MyINI(S_Path + "\\MES_Main.ini");

                string S_PartFamilyTypeID = Com_PartFamilyType.EditValue.ToString();
                string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                string S_PartID = Com_Part.EditValue.ToString();
                string S_Route = Com_Route.EditValue.ToString();
                string S_StationID= Com_Station.EditValue.ToString();

                myINI.WriteValue("ModUnitState", "PartFamilyTypeID", S_PartFamilyTypeID);
                myINI.WriteValue("ModUnitState", "PartFamilyID", S_PartFamilyID);
                myINI.WriteValue("ModUnitState", "PartID", S_PartID);
                myINI.WriteValue("ModUnitState", "Route", S_Route);
                myINI.WriteValue("ModUnitState", "StationID", S_StationID);

                string S_StationTypeID = "";
                int[] pRows = this.Grid_Route.GetSelectedRows();//传递实体类过去 获取选中的行
                if (pRows.GetLength(0) > 0)
                {
                    S_StationTypeID = Grid_Route.GetRowCellValue(pRows[0], "StationTypeID").ToString();
                    myINI.WriteValue("ModUnitState", "StationTypeID", S_StationTypeID);

                    S_INIS_UnitStateID=Grid_Route.GetRowCellValue(pRows[0], "UnitStateID").ToString();
                    myINI.WriteValue("ModUnitState", "UnitStateID", S_INIS_UnitStateID);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Com_Part_EditValueChanged(object sender, EventArgs e)
        {
            public_.AddRouteDetail(Com_Route, Grid_Route, mesRouteDetailSVC, List_Login.LineID.ToString(),
                                        Com_Part.EditValue.ToString(), Com_PartFamily.EditValue.ToString(), "");
        }

        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string S_SN = Edt_SN.Text.Trim();               
                DateTime dateStart = DateTime.Now;
                if (S_SN == "")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }

                DataTable DT_SN = PartSelectSVC.GetSerialNumber2(S_SN).Tables[0];
                if (DT_SN.Rows.Count > 0)
                {
                    if (DT_SN.Rows[0]["StatusID"].ToString() != "1")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20036", "NG", List_Login.Language);
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }

                    if (DT_SN.Rows[0]["PartID"].ToString() != Com_Part.EditValue.ToString())
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20043", "NG", List_Login.Language);
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }


                    try
                    {
                        string S_UnitStateID = "";
                        int[] pRows = this.Grid_Route.GetSelectedRows();//传递实体类过去 获取选中的行
                        if (pRows.GetLength(0) > 0)
                        {
                            S_UnitStateID=Grid_Route.GetRowCellValue(pRows[0], "UnitStateID").ToString();
                        }

                        if (S_UnitStateID == "")
                        {
                            S_UnitStateID = S_INIS_UnitStateID;
                        }

                        int I_StationID = Convert.ToInt32(Com_Station.EditValue.ToString()); 


                        mesUnit v_mesUnit = new mesUnit();
                        v_mesUnit.ID = Convert.ToInt32(DT_SN.Rows[0]["UnitID"].ToString());
                        //////////////////////////
                        v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);

                        v_mesUnit.StatusID = Convert.ToInt32(DT_SN.Rows[0]["StatusID"].ToString());
                        //////////////////////////////////////////////
                        v_mesUnit.StationID = I_StationID;

                        v_mesUnit.EmployeeID = List_Login.EmployeeID;
                        v_mesUnit.LastUpdate = DateTime.Now;
                        v_mesUnit.ProductionOrderID = Convert.ToInt32(DT_SN.Rows[0]["ProductionOrderID"].ToString());
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
                            ///////////////////////////////////////////
                            v_mesHistory.StationID = I_StationID;

                            v_mesHistory.EnterTime = DateTime.Now;
                            v_mesHistory.ExitTime = DateTime.Now;
                            v_mesHistory.ProductionOrderID = Convert.ToInt32(DT_SN.Rows[0]["ProductionOrderID"].ToString());
                            v_mesHistory.PartID = Convert.ToInt32(DT_SN.Rows[0]["PartID"].ToString());
                            v_mesHistory.LooperCount = 1;
                            int I_InsertHistory = mesHistorySVC.Insert(v_mesHistory);

                            Edt_SN.Text = "";
                            Edt_SN.Enabled = true;
                            Edt_SN.Focus();


                            TimeSpan ts = DateTime.Now - dateStart;
                            double mill = Math.Round(ts.TotalMilliseconds, 0);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { S_SN, mill.ToString() });
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
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

        private void Com_Station_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void Com_Route_EditValueChanged(object sender, EventArgs e)
        {
            string S_StationTypeID = "";
            int[] pRows = this.Grid_Route.GetSelectedRows();//传递实体类过去 获取选中的行
            if (pRows.GetLength(0) > 0)
            {
                S_StationTypeID = Grid_Route.GetRowCellValue(pRows[0], "StationTypeID").ToString();
            }

            if (S_StationTypeID == "")
            {
                S_StationTypeID = S_INIStationTypeID;
            }
            public_.AddStation(Com_Station, Grid_Station, S_StationTypeID, "");

            if (S_StationID != "") { Com_Station.EditValue = S_StationID; }
        }
    }

}
