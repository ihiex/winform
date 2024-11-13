﻿using System;
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
using System.Diagnostics;
using App.MyMES.mesUnitStateService;
using System.IO.Ports;
using App.MyMES.DataCommitService;
using DevComponents.DotNetBar;

namespace App.MyMES
{
    public partial class FrmBaseV2 : DevExpress.XtraEditors.XtraForm
    {
        public Public_ public_ = new Public_();
        public DataTable DT_ProductionOrder = new DataTable();
        public DataTable DT_ProductStructure = new DataTable();
        public LoginList List_Login = new LoginList();
        public string Batch_Pattern = string.Empty;
        public string SN_Pattern = string.Empty;
        public DataTable DtDefect = new DataTable();
        public string DefectChar = string.Empty;
        public string DefectCode = string.Empty;

        public int I_RouteSequence = -9;  //当前工序顺序 
        public int I_POID = 0;
        public string S_DefectTypeID = "";

        public ImesUnitSVCClient mesUnitSVC;
        public PartSelectSVCClient PartSelectSVC;
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
        public ImesUnitStateSVCClient mesUnitStateSVC;
        public DataCommitSVCClient DataCommitSVC;

        public SerialPort port1;

        public bool COF = false;
        public string IsAuto = "0";
        public string IsNFC = "0";
        public string IsBindAdhesive = "0";
        public string S_IsCheckPO = "1";
        public string S_IsCheckPN = "1";

        public string strPort = "COM1";
        public string ReadCmd = "READY";
        public string TestSN = "TESTSHELL001";
        
        public DataSet DS_StationTypeDef;

        public FrmBaseV2()
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
                mesUnitStateSVC = ImesUnitStateFactory.CreateServerClient();
                DataCommitSVC = DataCommitFactory.CreateServerClient();
            }
            catch (Exception ex)
            {
                //MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }

        }


        public DataSet GetStationData()
        {
            Group_NoPN.Visible = false;

            DataSet dsStationDetail = PartSelectSVC.GetPLCSeting("", List_Login.StationID.ToString());
            DS_StationTypeDef = PartSelectSVC.MESGetStationTypeParameter(List_Login.StationTypeID);

            DataRow[] drREAD = dsStationDetail.Tables[0].Select("Name='READY'");
            if (drREAD.Length > 0) ReadCmd = drREAD[0]["Value"].ToString(); else ReadCmd = "READY";

            DataRow[] drPort = dsStationDetail.Tables[0].Select("Name='Port'");
            if (drPort.Length > 0) strPort = drPort[0]["Value"].ToString(); else strPort = "COM1";

            DataRow[] drAutoStation = dsStationDetail.Tables[0].Select("Name='AutoStation'");
            if (drAutoStation.Length > 0) IsAuto = drAutoStation[0]["Value"].ToString(); else IsAuto = "0";

            DataRow[] drIsBindAdhesive = dsStationDetail.Tables[0].Select("Name='IsBindAdhesive'");
            if (drIsBindAdhesive.Length > 0) IsBindAdhesive = drIsBindAdhesive[0]["Value"].ToString(); else IsBindAdhesive = "0";

            DataRow[] drIsNFC = dsStationDetail.Tables[0].Select("Name='IsBindNFC'");
            if (drIsNFC.Length > 0) IsNFC = drIsNFC[0]["Value"].ToString(); else IsNFC = "0";

            DataRow[] DR_IsCheckPO = DS_StationTypeDef.Tables[0].Select("Description='IsCheckPO'");
            if (DR_IsCheckPO.Length > 0)
            {
                S_IsCheckPO = DR_IsCheckPO[0]["Content"].ToString();

                if (S_IsCheckPO == "1")
                {
                    DataRow[] DR_Child_IsCheckPO = dsStationDetail.Tables[0].Select("Name='IsCheckPO'");
                    if (DR_Child_IsCheckPO.Length > 0) S_IsCheckPO = DR_Child_IsCheckPO[0]["Value"].ToString(); else S_IsCheckPO = "1";
                }
            }
            else
            {
                S_IsCheckPO = "1";
            }

            DataRow[] DR_IsCheckPN = DS_StationTypeDef.Tables[0].Select("Description='IsCheckPN'");
            if (DR_IsCheckPN.Length > 0)
            {
                S_IsCheckPN = DR_IsCheckPN[0]["Content"].ToString();

                if (S_IsCheckPN == "1")
                {
                    DataRow[] DR_Child_IsCheckPN = dsStationDetail.Tables[0].Select("Name='IsCheckPN'");
                    if (DR_Child_IsCheckPN.Length > 0) S_IsCheckPN = DR_Child_IsCheckPN[0]["Value"].ToString(); else S_IsCheckPN = "1";
                }
            }
            else
            {
                S_IsCheckPN = "1";
            }


            if (S_IsCheckPO == "1")
            {
                lblProductionOrder.Visible = true;
                lblUnitState.Visible = true;
                lblUnitState.Location = new Point(58, 135);

                panel12.Visible = true;
            }
            else
            {
                lblProductionOrder.Visible = false;
                lblUnitState.Visible = true;

                lblUnitState.Location = new Point(58, 102);
                panel12.Visible = false;
            }


            ////判断是否胶注册
            //DataRow[] drScanGlue = DS_StationTypeDef.Tables[0].Select("Description='IsScanAdhesive'");
            //if (drScanGlue.Length > 0 && drScanGlue[0]["Content"].ToString() == "1")
            //{
            //    PanelBatchConfirn.Visible = true;
            //}
            //else
            //{
            //    PanelBatchConfirn.Visible = false;
            //}

            if (S_IsCheckPN == "0")
            {
                Group_NoPN.Visible = true;
                Group_NoPN.Enabled = true;
            }

            return dsStationDetail;
        }


        #region 界面加载
        public virtual void FrmBase_Load(object sender, EventArgs e)
        {
            LoadControls();
            Btn_Refresh_Click(null, null);
            Com_PO.Enabled = true;

            proBarProductionOrder.Properties.Minimum = 0;
            proBarProductionOrder.Properties.Maximum = 100;
            proBarProductionOrder.Properties.Step = 1;
            proBarProductionOrder.Properties.ProgressViewStyle = DevExpress.XtraEditors.Controls.ProgressViewStyle.Solid;
            proBarProductionOrder.Position = 0;
            proBarProductionOrder.Properties.ShowTitle = true;
            proBarProductionOrder.Properties.PercentView = true;
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

        public virtual void Com_PartFamily_EditValueChanged(object sender, EventArgs e)
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

        private void Com_Part_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartID = Com_Part.EditValue.ToString();
                public_.AddPOAll(Com_PO, S_PartID, List_Login.LineID.ToString(), Grid_PO);
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void Com_PO_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_POID = Com_PO.EditValue.ToString();

                Get_DTPO(S_POID);
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        public void Get_DTPO(string S_POID)
        {
            I_POID = Convert.ToInt32(S_POID);
            DT_ProductionOrder = PartSelectSVC.GetProductionOrder(S_POID).Tables[0];
        }

        public virtual void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            try
            {
                GetStationData();
                if (Com_PO.Text.Trim() == "")
                {
                    if (S_IsCheckPN == "1" && S_IsCheckPO == "1")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20114", "NG", List_Login.Language);
                        return;
                    }
                }
                else
                {
                    if (Com_PartFamilyType.EditValue == null || string.IsNullOrEmpty(Com_PartFamilyType.Text.ToString()))
                    {
                        if (S_IsCheckPN == "1")
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20115", "NG", List_Login.Language);
                            return;
                        }
                    }
                    if (Com_PartFamily.EditValue == null || string.IsNullOrEmpty(Com_PartFamily.Text.ToString()))
                    {
                        if (S_IsCheckPN == "1")
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20116", "NG", List_Login.Language);
                            return;
                        }
                    }
                    if (Com_Part.EditValue == null || string.IsNullOrEmpty(Com_Part.Text.ToString()))
                    {
                        if (S_IsCheckPN == "1")
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20117", "NG", List_Login.Language);
                            return;
                        }
                    }
                    if (Com_PO.EditValue == null || string.IsNullOrEmpty(Com_PO.Text.ToString()))
                    {
                        if (S_IsCheckPN == "1" && S_IsCheckPO == "1")
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20118", "NG", List_Login.Language);
                            return;
                        }
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

                        dataSet = PartSelectSVC.GetmesPartDetail(I_PartID, "SN_Pattern");
                        if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            SN_Pattern = dataSet.Tables[0].Rows[0]["Content"].ToString();
                        }
                        else
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20181", "NG", List_Login.Language, new string[] { Com_Part.Text.ToString() });
                            return;
                        }

                        //显示料号颜色
                        DevExpress.XtraEditors.LabelControl lblColors = new DevExpress.XtraEditors.LabelControl();
                        DataSet dataColor = PartSelectSVC.GetmesPartDetail(I_PartID, "Color");
                        if (dataColor != null && dataColor.Tables.Count > 0 && dataColor.Tables[0].Rows.Count > 0)
                        {
                            lblColors.Text = dataColor.Tables[0].Rows[0]["Content"].ToString();
                            lblColors.Appearance.Font = new System.Drawing.Font("Tahoma", 18F);
                            lblColors.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            lblColors.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                            lblColors.Dock = System.Windows.Forms.DockStyle.Right;
                            lblColors.Size = new System.Drawing.Size(0, 42);
                        }

                        DataSet dataColorValue = PartSelectSVC.GetmesPartDetail(I_PartID, "ColorValue");
                        if (dataColorValue != null && dataColorValue.Tables.Count > 0 && dataColorValue.Tables[0].Rows.Count > 0)
                        {
                            string colorValue = dataColorValue.Tables[0].Rows[0]["Content"].ToString();
                            string[] listColor = colorValue.Split(',');
                            if (listColor.Length == 3 && Public_.IsNumeric(listColor[0].ToString())
                                && Public_.IsNumeric(listColor[1].ToString()) && Public_.IsNumeric(listColor[2].ToString()))
                            {
                                int color1 = Convert.ToInt32(listColor[0].ToString());
                                int color2 = Convert.ToInt32(listColor[1].ToString());
                                int color3 = Convert.ToInt32(listColor[2].ToString());
                                lblColors.Appearance.BackColor = System.Drawing.Color.FromArgb(color1, color2, color3);
                            }
                        }
                        PanleControl.Controls.Add(lblColors);
                    }

                    DataSet dsStructure = mesProductStructureSVC.GetBOMStructure(Com_Part.EditValue.ToString(), "", "");
                    if (dsStructure != null && dsStructure.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = dsStructure.Tables[0];
                        treeListColBom.Caption = dt.Rows[0]["ParentPartDescription"].ToString();
                        TreeKeyObj treeKeyObj = new TreeKeyObj();
                        treeKeyObj.FiterName = "ParentPartID";
                        treeKeyObj.FiterValue = Com_Part.EditValue.ToString();
                        treeKeyObj.KeyName1 = "PartID";
                        treeKeyObj.KeyName2 = "PartNumber";
                        treeKeyObj.KeyValue = "PartID";
                        SetDataSetToTree(TreeModule, null, treeKeyObj, dt);
                    }

                    DataSet DS_RouteData = PartSelectSVC.GetRouteData(List_Login.LineID.ToString(), Com_Part.EditValue.ToString(),
                        Com_PartFamily.EditValue.ToString(), Com_PO.EditValue.ToString());
                    if (DS_RouteData.Tables.Count == 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20195", "NG", List_Login.Language);                        
                        return; 
                    }

                    if (DS_RouteData.Tables[0].Rows.Count==0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20195", "NG", List_Login.Language);
                        return;
                    }


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
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20195", "NG", List_Login.Language, new[] { "uspGetRouteData" });
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
                        DataSet dsRouteDetail = mesRouteDetailSVC.GetRouteDetail(List_Login.LineID.ToString(),
                            Com_Part.EditValue.ToString(), Com_PartFamily.EditValue.ToString(),Com_PO.EditValue.ToString());
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
                    PartEdtStatus(false);
                    GetProductionCount();
                }
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
            Com_Part.Enabled = B_Status;
            Com_PO.Enabled = B_Status;
            Btn_ConfirmPO.Enabled = B_Status;
            //Com_luUnitStatus.Enabled = !B_Status;
            GrpControlInputData.Enabled = !B_Status;
        }
        #endregion

        private void ConsoleDefect()
        {
            if (Com_luUnitStatus.EditValue.ToString() == "2" || Com_luUnitStatus.EditValue.ToString() == "3")
            {
                Btn_Defect.Enabled = true;
                if (!GrpControlDefect.Visible)
                {
                    //if (DtDefect == null || DtDefect.Rows.Count == 0)
                    //{
                    if (string.IsNullOrEmpty(S_DefectTypeID))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20182", "NG", List_Login.Language, new string[] { Com_Part.Text.Trim() });
                        return;
                    }
                    DtDefect = PartSelectSVC.GetluDefect(Convert.ToInt32(S_DefectTypeID)).Tables[0];
                    if (DtDefect == null || DtDefect.Rows.Count == 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20182", "NG", List_Login.Language, new string[] { Com_Part.Text.Trim() });
                        return;
                    }
                    DtDefect.Columns.Add("Check", typeof(bool));
                    Grid_Main.DataSource = DtDefect;
                    //}
                    GrpControlInputData.Visible = false;
                    SplitContolAll.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
                    GrpControlDefect.Dock = DockStyle.Fill;
                    GrpControlDefect.Visible = true;
                }
            }
            else
            {
                Btn_Defect.Enabled = false;
                if (GrpControlDefect.Visible)
                {
                    GrpControlDefect.Dock = DockStyle.None;
                    SplitContolAll.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;
                    GrpControlDefect.Visible = false;
                    GrpControlInputData.Visible = true;

                }
            }
        }

        public virtual void Com_luUnitStatus_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();
                string S_Description = Com_luUnitStatus.Text.Trim();
                ConsoleDefect();
                switch (S_luUnitStateID)
                {
                    case "1":
                        Com_luUnitStatus.ForeColor = Color.Green;
                        break;
                    case "2":
                        Com_luUnitStatus.ForeColor = Color.Red;
                        break;
                    case "3":
                        Com_luUnitStatus.ForeColor = Color.Orange;
                        break;
                    case "4":
                        Com_luUnitStatus.ForeColor = Color.LightGray;
                        break;
                    default:
                        Com_luUnitStatus.ForeColor = Color.Black;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void Btn_Defect_Click(object sender, EventArgs e)
        {
            ConsoleDefect();
        }

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
                mesUnitStateSVC.Close();
                DataCommitSVC.Close();

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
                mesUnitStateSVC = ImesUnitStateFactory.CreateServerClient();
                DataCommitSVC = DataCommitFactory.CreateServerClient();

                List_Login = this.Tag as LoginList;
                string S_StationTypeID = List_Login.StationTypeID.ToString();
                string S_LineID = List_Login.LineID.ToString();
                public_.AddPartFamilyType(Com_PartFamilyType, Grid_PartFamilyType);
                public_.AddluUnitStatus(Com_luUnitStatus, Grid_luUnitStatus);
                string S_PartID = Com_Part.EditValue.ToString();
                public_.AddPOAll(Com_PO, S_PartID, List_Login.LineID.ToString(), Grid_PO);
                foreach (Control ctrl in PanleControl.Controls)
                {
                    PanleControl.Controls.Remove(ctrl);
                }

                PartEdtStatus(true);

                try
                {
                    port1.Close();
                }
                catch { }

                GetStationData();               
            }
            catch (Exception ex)
            {
                //MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        public virtual void btnDefectSave_Click(object sender, EventArgs e)
        {
            bool IsChoose = false;
            DefectChar = string.Empty;
            DefectCode = string.Empty;

            for (int i = 0; i < GridView_Defact.DataRowCount; i++)
            {
                DataRow DR = GridView_Defact.GetDataRow(i);
                string S_Check = DR["Check"].ToString();
                if (S_Check != "" && Convert.ToBoolean(S_Check))
                {
                    DefectChar += DR["ID"].ToString() + ";";
                    DefectCode += DR["不良代码"].ToString() + ";";
                    IsChoose = true;
                }
            }

            if (IsChoose)
            {
                GrpControlDefect.Dock = DockStyle.None;
                GrpControlDefect.Visible = false;
                GrpControlInputData.Visible = true;
                SplitContolAll.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;
            }
            else
            {
                MessageInfo.Show_Info_MessageBox("20049", List_Login.Language, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 影藏
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

        /// <summary>
        /// 计数累加
        /// </summary>
        /// <param name="OvserStaion"></param>
        public void SetOverStiaonQTY(bool OvserStaion)
        {
            if (OvserStaion)
            {
                txtOverStationQTY.Text = (Convert.ToInt32(txtOverStationQTY.Text.ToString()) + 1).ToString();
            }
            else
            {
                txtDefectQTY.Text = (Convert.ToInt32(txtDefectQTY.Text.ToString()) + 1).ToString();
            }
        }

        /// <summary>
        /// 计数重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确认要重置当前计数数量吗?", "提示",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }
            txtOverStationQTY.Text = "0";
            txtDefectQTY.Text = "0";
        }

        private void FrmBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (port1 != null)
                {
                    if (port1.IsOpen) port1.Close();
                    port1.Dispose();
                }

                string S_BarPrint = "";
                string S_WindowsVer = public_.GetWinVer().Substring(0, 2);

                if (S_WindowsVer == "64")
                {
                    S_BarPrint = "BartenderPrint.exe";
                }
                else if (S_WindowsVer == "32")
                {
                    S_BarPrint = "BartenderPrint_X86.exe";
                }

                Process[] arrayProcess = Process.GetProcessesByName(S_BarPrint.Replace(".exe", ""));
                if (arrayProcess.Length > 0)
                {
                    foreach (Process pp in arrayProcess)
                    {
                        pp.Kill();
                    }
                }
            }
            catch { }
        }

        private void Grid_RouteDataView_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            if (Grid_RouteData.DataSource as DataTable != null && e.RowHandle >= 0)
            {
                string S_StationTypeID = view.GetRowCellDisplayText(e.RowHandle, view.Columns["StationTypeID"]);
                if (S_StationTypeID == List_Login.StationTypeID.ToString())
                {
                    e.Appearance.ForeColor = Color.Red;
                }
            }

        }

        private void Grid_RouteDataView_MasterRowExpanded(object sender, DevExpress.XtraGrid.Views.Grid.CustomMasterRowEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView childView = Grid_RouteDataView.GetDetailView(e.RowHandle, e.RelationIndex) as DevExpress.XtraGrid.Views.Grid.GridView;

            if (childView != null)
            {
                childView.Columns["ID"].Visible = false;
                childView.Columns["StationTypeID"].Visible = false;
                childView.Columns["RouteID"].Visible = false;
                childView.Columns["CurrStateID"].Visible = false;
                childView.Columns["OutputStateID"].Visible = false;
                childView.Columns["OutputStateDefID"].Visible = false;
                childView.Columns["RouteType"].Visible = false;
                childView.Columns["ParentID"].Visible = false;
                childView.OptionsView.ShowGroupPanel = false;
                childView.OptionsView.ShowColumnHeaders = false;
            }
        }

        public void GetProductionCount()
        {
            try
            {
                if (string.IsNullOrEmpty(Com_PO.EditValue.ToString()))
                    return;
                string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + Com_PO.EditValue.ToString() + "\"> </ProdOrder>";
                string xmlStation = "<Station StationID=\"" + List_Login.StationID.ToString() + "\"> </Station>";
                string outString = string.Empty;
                DataSet ds = PartSelectSVC.uspCallProcedure("uspGetProductionOrderCount", null, xmlProdOrder, null, xmlStation, null, null, ref outString);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string currentCount = ds.Tables[0].Rows[0]["currentCount"].ToString();
                    string prodCount = ds.Tables[0].Rows[0]["prodCount"].ToString();
                    proBarProductionOrder.Properties.Maximum = Convert.ToInt32(prodCount);
                    proBarProductionOrder.Position = Convert.ToInt32(currentCount);
                    lblProBar.Text = currentCount + "/" + prodCount;
                }
            }
            catch { }
        }
    }
}
