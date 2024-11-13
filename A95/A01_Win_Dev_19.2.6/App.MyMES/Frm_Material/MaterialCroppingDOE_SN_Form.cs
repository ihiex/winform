using System;
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
using App.MyMES.mesMaterialUnitService;
using App.MyMES.mesHistoryService;
using System.Threading;
using System.Configuration;
using System.Text.RegularExpressions;

namespace App.MyMES
{
    public partial class MaterialCroppingDOE_SN_Form : FrmBase2
    {
        
        int ParentID;
        int BatchQTY = 0;
        int UsageQTY = 0;
        string S_LabelName = "";
        DataTable dtBatchData;
        ImesMaterialUnitSVCClient mesMaterialSVC;
        ImesHistorySVCClient mesHistorySVC;
        DataTable DT_PrintSn = new DataTable();
        LabelManager2.Application LabSN;
        string S_LabelPath = string.Empty;
        int CCCLength = 4;

        public MaterialCroppingDOE_SN_Form()
        {
            InitializeComponent();
        }

        public override void FrmBase_Load(object sender, EventArgs e)
        {
            base.FrmBase_Load(sender, e);
            mesMaterialSVC = ImesMaterialUnitFactory.CreateServerClient();
            mesHistorySVC = ImesHistoryFactory.CreateServerClient();
            ProBarControl.Properties.Minimum = 0;
            ProBarControl.Properties.Maximum = 100;
            ProBarControl.Properties.Step = 1;
            ProBarControl.Properties.ProgressViewStyle = DevExpress.XtraEditors.Controls.ProgressViewStyle.Solid;
            ProBarControl.Position = 0;
            ProBarControl.Properties.ShowTitle = true;
            ProBarControl.Properties.PercentView = true;
            public_.OpenBartender(List_Login.StationID.ToString());

        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            Com_PO.Enabled = true;
            Com_LineGroup.Enabled = true;
        }

        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
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
                if (Com_PO.EditValue == null || string.IsNullOrEmpty(Com_PO.Text.Trim()))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20114", "NG", List_Login.Language);
                    return;
                }

                DataSet dataSet;
                //批次正则
                int I_PartID = Convert.ToInt32(Com_Part.EditValue.ToString());
                dataSet = PartSelectSVC.GetmesPartDetail(I_PartID, "Batch_Pattern");
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    Batch_Pattern = dataSet.Tables[0].Rows[0]["Content"].ToString();
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20025", "NG", List_Login.Language);
                    return;
                }

                dataSet = PartSelectSVC.GetmesPartDetail(I_PartID, "SplitBatchQTY");
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    txtSpecQTY.Text = dataSet.Tables[0].Rows[0]["Content"].ToString();
                }

                dataSet = PartSelectSVC.GetmesPartDetail(I_PartID, "IsForceSplit");
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    txtSpecQTY.Enabled = dataSet.Tables[0].Rows[0]["Content"].ToString() != "1";
                }
                else
                {
                    txtSpecQTY.Enabled = true;
                }

                string Result = string.Empty;
                string xmlPartStr = "<Part PartID=\"" + Com_Part.EditValue.ToString() + "\"> </Part>";
                string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                dataSet = PartSelectSVC.uspCallProcedure("uspGetMaterailBatchData", null,
                                                                        null, xmlPartStr, xmlStation, null, null, ref Result);
                if (Result == "1" && dataSet != null)
                {
                    txtParentBatchNo.Properties.DataSource = dataSet.Tables[0];
                    txtParentBatchNo.Properties.DisplayMember = "SerialNumber";
                    txtParentBatchNo.Properties.ValueMember = "ID";

                    if (dataSet.Tables[0].Rows.Count != 0)
                    {
                        dtBatchData = dataSet.Tables[0];
                    }
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
                    treeKeyObj.KeyName2 = "PartDescription";
                    treeKeyObj.KeyValue = "PartID";
                    SetDataSetToTree(TreeModule, null, treeKeyObj, dt);
                }

                DataSet DS_RouteData = PartSelectSVC.GetRouteData(List_Login.LineID.ToString(), Com_Part.EditValue.ToString(),
                        Com_PartFamily.EditValue.ToString(), Com_PO.EditValue.ToString());
                
                if (DS_RouteData == null || DS_RouteData.Tables.Count == 0 || DS_RouteData.Tables[0].Rows.Count == 0)
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
                        Com_Part.EditValue.ToString(), Com_PartFamily.EditValue.ToString(), Com_PO.EditValue.ToString());
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

                string PO = Com_PO.EditValue.ToString();
                DataSet dsBuildName = PartSelectSVC.GetluPODetailDef(Convert.ToInt32(PO), "DOE_BuildName");
                if (dsBuildName != null && dsBuildName.Tables.Count > 0 && dsBuildName.Tables[0].Rows.Count > 0)
                {
                    string BuildName = dsBuildName.Tables[0].Rows[0]["Content"].ToString();
                    if (!string.IsNullOrEmpty(BuildName))
                    {
                        comboxET.Enabled = true;
                        string[] strValue = BuildName.Split(',');
                        foreach (string str in strValue)
                        {
                            comboxET.Properties.Items.Add(str);
                        }
                    }
                    else
                    {
                        comboxET.Enabled = false;
                    }
                }

                DataSet dsCCCC = PartSelectSVC.GetluPODetailDef(Convert.ToInt32(PO), "DOE_CCCCConfig");
                if (dsCCCC != null && dsCCCC.Tables.Count > 0 && dsCCCC.Tables[0].Rows.Count > 0)
                {
                    string Length = dsCCCC.Tables[0].Rows[0]["Content"].ToString();
                    if (Length == "1")
                    {
                        CCCLength = Convert.ToInt32(Length);
                        txtCCCC.Enabled = true;
                    }
                    else
                    {
                        txtCCCC.Enabled = false;
                    }
                }

                radBtnCode.Checked = true;

                //string S_PartFamilyType = Com_PartFamilyType.Text.Trim();
                //if (S_PartFamilyType == "MF")
                //{
                //    radioGroupType.SelectedIndex = 1;
                //}
                //else if (S_PartFamilyType == "HBF")
                //{
                //    radioGroupType.SelectedIndex = 0;
                //}


                Com_PartFamilyType.Enabled = false;
                Com_PartFamily.Enabled = false;
                Com_Part.Enabled = false;
                Com_PO.Enabled = false;
                Btn_ConfirmPO.Enabled = false;
                Com_LineGroup.Enabled = false;
                base.GrpControlInputData.Enabled = true;
                base.GetProductionCount();
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }

        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtParentBatchNo.Text.Trim()))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20144", "NG", List_Login.Language);
                return;
            }
            if (string.IsNullOrEmpty(txtSpecQTY.Text.Trim()))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20145", "NG", List_Login.Language);
                return;
            }

            int SplitQTY = Convert.ToInt32(txtSpecQTY.Text.ToString());
            if (SplitQTY <= 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20146", "NG", List_Login.Language);
                return;
            }
            if (BatchQTY - UsageQTY < SplitQTY)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20172", "NG", List_Login.Language, new string[] { (BatchQTY - UsageQTY).ToString() });
                return;
            }
            if(txtCCCC.Enabled && CCCLength != txtCCCC.Text.Trim().Length)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { "The CCCC Config length should be " + CCCLength.ToString() + "bits" }, "");
                return;
            }
            string S_PartID = Com_Part.EditValue.ToString();
            string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
            string S_FormatSN = PartSelectSVC.mesGetSNFormatIDByList(S_PartID, S_PartFamilyID,
                        List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), List_Login.StationTypeID.ToString());
            string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, S_PartFamilyID, List_Login.StationTypeID, 
                List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), "1");

            if (string.IsNullOrEmpty(S_UnitStateID))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
                return;
            }

            if (string.IsNullOrEmpty(S_FormatSN))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20034", "NG", List_Login.Language, new string[] { S_PartID });
                return;
            }

            string S_PartFamilyTypeID = Com_PartFamilyType.EditValue.ToString();
            string S_LineID = Grid_LineGroup.GetRowCellValue(Grid_LineGroup.GetSelectedRows()[0], "LineID").ToString();
            string S_Production0rder = "'<ProdOrder ProductionOrder=" + "\"" + Com_PO.EditValue.ToString() + "\"" + "> </ProdOrder>'";
            string S_Station = "'<Station StationID=" + "\"" + List_Login.StationID.ToString() + "\"" + "> </Station>'";
            string S_xmlPart = "'<Part PartID=" + "\"" + S_PartID + "\"" + "> </Part>'";
            string xmlExtraData = @"'<ExtraData BatchNo=" + "\"" + txtParentBatchNo.Text.Trim() + "\"" +
                                               " RollCode =\"" + txtRoolNo.Text.Trim() + "\"" +
                                               " PartFamilyTypeID=" + "\"" + S_PartFamilyTypeID + "\"" +
                                               " LineType=" + "\"" + "M" + "\"" +
                                               " LineID = " + "\"" + S_LineID + "\"";
            if (!string.IsNullOrEmpty(comboxET.Text.Trim()))
            {
                xmlExtraData = xmlExtraData + " ET=" + "\"" + comboxET.Text.Trim() + "\"";
            }
            if (!string.IsNullOrEmpty(txtCCCC.Text.Trim()))
            {
                xmlExtraData = xmlExtraData + " SPCA = " + "\"" + txtCCCC.Text.Trim() + "\"";
            }

            xmlExtraData  = xmlExtraData  + "> </ExtraData>'";
            MesMaterialUnit mesMaterial = new MesMaterialUnit();
            mesMaterial.PartID = Convert.ToInt32(S_PartID);
            mesMaterial.StationID = List_Login.StationID;
            mesMaterial.EmployeeID = List_Login.EmployeeID; ;
            mesMaterial.LineID = List_Login.LineID;
            mesMaterial.ParentID = ParentID;
            mesMaterial.RollCode = txtRoolNo.Text.Trim();

            mesUnit v_mesUnit = new mesUnit();
            v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
            v_mesUnit.StatusID = 1;
            v_mesUnit.PartFamilyID = Convert.ToInt32(Com_PartFamily.EditValue.ToString());
            v_mesUnit.StationID = List_Login.StationID;
            v_mesUnit.EmployeeID = List_Login.EmployeeID;
            v_mesUnit.CreationTime = DateTime.Now;
            v_mesUnit.LastUpdate = DateTime.Now;
            v_mesUnit.PanelID = 0;
            v_mesUnit.LineID = List_Login.LineID;
            v_mesUnit.ProductionOrderID = Convert.ToInt32(Com_PO.EditValue.ToString());
            v_mesUnit.RMAID = 0;
            v_mesUnit.PartID = Convert.ToInt32(S_PartID);
            v_mesUnit.LooperCount = 1;
            v_mesUnit.StatusID = 1;
            string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";

            if (radBtnBsmall.Checked)
            {
                DataSet DS = PartSelectSVC.uspSNRGetNext(S_FormatSN, 0, S_Production0rder, S_xmlPart, S_Station, xmlExtraData, null);
                if (DS == null || DS.Tables[0].Rows.Count == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20087", "NG", List_Login.Language);
                    return;
                }

                string New_SN = DS.Tables[1].Rows[0][0].ToString();

                if (!Regex.IsMatch(New_SN, SN_Pattern))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20027", "NG", List_Login.Language,new string[] { New_SN });
                    return;
                }
                mesMaterial.SerialNumber = New_SN;
                mesMaterial.Quantity = SplitQTY;
                mesMaterial.RemainQTY = SplitQTY;
                mesMaterial.MaterialTypeID = 1;
                mesMaterialSVC.InserForParent(mesMaterial);

                string S_InsertUnit = mesUnitSVC.Insert(v_mesUnit);

                mesUnitDetail v_mesUnitDetail = new mesUnitDetail();
                v_mesUnitDetail.UnitID = Convert.ToInt32(S_InsertUnit);
                v_mesUnitDetail.reserved_01 = New_SN;
                v_mesUnitDetail.reserved_02 = "";
                v_mesUnitDetail.reserved_03 = "";
                v_mesUnitDetail.reserved_04 = "";
                v_mesUnitDetail.reserved_05 = "";
                mesUnitDetailSVC.Insert(v_mesUnitDetail);

                mesHistory v_mesHistory = new mesHistory();
                v_mesHistory.UnitID = Convert.ToInt32(S_InsertUnit);
                v_mesHistory.UnitStateID = v_mesUnit.UnitStateID;
                v_mesHistory.EmployeeID = v_mesUnit.EmployeeID;
                v_mesHistory.StationID = v_mesUnit.StationID;
                v_mesHistory.EnterTime = DateTime.Now;
                v_mesHistory.ExitTime = DateTime.Now;
                v_mesHistory.ProductionOrderID = v_mesUnit.ProductionOrderID;
                v_mesHistory.PartID = Convert.ToInt32(v_mesUnit.PartID);
                v_mesHistory.LooperCount = 1;
                mesHistorySVC.Insert(v_mesHistory);

                mesSerialNumber v_mesSerialNumber = new mesSerialNumber();
                v_mesSerialNumber.UnitID = Convert.ToInt32(S_InsertUnit);
                v_mesSerialNumber.SerialNumberTypeID = 2;
                v_mesSerialNumber.Value = New_SN;
                mesSerialNumberSVC.Insert(v_mesSerialNumber);

                string Result = string.Empty;
                
                PartSelectSVC.uspCallProcedure("uspSetMaterailBatchData", txtParentBatchNo.EditValue.ToString(),
                                                                        null, null, xmlStation, null, txtSpecQTY.Text.Trim(), ref Result);

                DT_PrintSn.Columns.Clear();
                DT_PrintSn.Clear();
                DT_PrintSn.Columns.Add("SN", typeof(string));
                DT_PrintSn.Columns.Add("CreateTime", typeof(string));
                DT_PrintSn.Columns.Add("PrintTime", typeof(string));
                DT_PrintSn.Columns.Add("TempPath", typeof(string));

                DataRow DR = DT_PrintSn.NewRow();
                DR["SN"] = New_SN;
                DR["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DR["PrintTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DT_PrintSn.Rows.Add(DR);

                dataGridSN.DataSource = DT_PrintSn;
            }
            else
            {
                int number = Convert.ToInt32(txtSpecQTY.Text.Trim());
                DataSet dsMaterial = null;
                mesMaterial.Quantity = 1;
                mesMaterial.RemainQTY = 0;
                mesMaterial.MaterialTypeID = 2;
                PartSelectSVC.Get_CreateMaterail(S_FormatSN, null, S_xmlPart, null, xmlExtraData,
                    List_Login, v_mesUnit, mesMaterial, number, ref dsMaterial);

                string Result = string.Empty;
                PartSelectSVC.uspCallProcedure("uspSetMaterailBatchData", txtParentBatchNo.EditValue.ToString(),
                                                                        null, null, xmlStation, null, txtSpecQTY.Text.Trim(), ref Result);

                DT_PrintSn = dsMaterial.Tables[0];
                DT_PrintSn.Columns.Add("CreateTime", typeof(string));
                DT_PrintSn.Columns.Add("PrintTime", typeof(string));
                foreach (DataRow dr in DT_PrintSn.Rows)
                {
                    dr["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dr["PrintTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                dataGridSN.DataSource = DT_PrintSn;
                for (int i = 0; i < this.dataGridSN.Columns.Count; i++)
                {
                    this.dataGridSN.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
            }
            UsageQTY = UsageQTY + Convert.ToInt32(txtSpecQTY.Text.Trim());

            string S_LabelName = public_.GetLabelName(PartSelectSVC, List_Login.StationTypeID.ToString(), Com_PartFamily.EditValue.ToString(),
                  Com_Part.EditValue.ToString(), Com_PO.EditValue.ToString(), List_Login.LineID.ToString());

            if (string.IsNullOrEmpty(S_LabelName))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20104", "NG", List_Login.Language);
                return;
            }
            string S_PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, S_LabelName, DT_PrintSn, Convert.ToInt32(Com_Part.EditValue.ToString()));
            ProBarControl.Position = ProBarControl.Position + SplitQTY;
            if (S_PrintResult != "OK")
            {
                string ProMsg = MessageInfo.GetMsgByCode(S_PrintResult.Replace("ERROR", ""), List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20111", "NG", List_Login.Language, new string[] { ProMsg }, ProMsg);
                return;
            }
            else
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "10029", "OK", List_Login.Language, new string[] { txtParentBatchNo.Text.Trim(), SplitQTY.ToString() });
                base.GetProductionCount();
            }
        }


        private void Get_TempletPath(Boolean IsMsg)
        {
            try
            {
                string S_StationTypeID = List_Login.StationTypeID.ToString();
                string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                string S_PartID = Com_Part.EditValue.ToString();
                string S_ProductionOrderID = Com_PO.EditValue.ToString();
                string S_LoginLineID = List_Login.LineID.ToString();

                string labelPath = public_.GetLabelPath(PartSelectSVC, S_StationTypeID, S_PartFamilyID, S_PartID, S_ProductionOrderID, S_LoginLineID);
                if (labelPath == "")
                {
                    if (IsMsg == true)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20097", "NG", List_Login.Language);
                    }
                }
                else
                {
                    S_LabelPath = labelPath;
                    if (S_LabelPath.Substring(0, 5) == "ERROR")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(S_LabelPath.Replace("ERROR", ""), List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_LabelPath.Replace("ERROR", ""));
                        S_LabelPath = "";
                    }
                }
            }
            catch
            { }
        }

        private void txtParentBatchNo_EditValueChanged(object sender, EventArgs e)
        {
            if (dtBatchData != null && dtBatchData.Rows.Count > 0)
            {
                ParentID = Convert.ToInt32(txtParentBatchNo.EditValue.ToString());
                DataRow drBatch = dtBatchData.Select("ID=" + ParentID.ToString()).FirstOrDefault();
                if (drBatch != null && !drBatch.IsNull("ID"))
                {
                    BatchQTY = Convert.ToInt32(drBatch["Quantity"].ToString()) +
                            Convert.ToInt32(drBatch["BalanceQty"].ToString());

                    UsageQTY = mesMaterialSVC.GetMesMaterialUseQTY(txtParentBatchNo.EditValue.ToString());//Convert.ToInt32(drBatch["UsageQTY"].ToString());

                    ProBarControl.Properties.Maximum = BatchQTY;
                    ProBarControl.Position = UsageQTY;

                    string RoolNo = drBatch["RollCode"].ToString();
                    if (!string.IsNullOrEmpty(RoolNo))
                    {
                        txtRoolNo.Text = RoolNo;
                        txtRoolNo.Enabled = false;
                    }
                    else
                    {
                        txtRoolNo.Text = string.Empty;
                        txtRoolNo.Enabled = true;
                    }
                }
            }
        }

        public override void Com_PartFamilyType_EditValueChanged(object sender, EventArgs e)
        {
            base.Com_PartFamilyType_EditValueChanged(sender, e);
            int I_PartFamilyID = Convert.ToInt32(Com_PartFamilyType.EditValue.ToString());
            public_.AddLineGroup(Com_LineGroup, Grid_LineGroup, "M", I_PartFamilyID);
            S_DefectTypeID = public_.GetDefectTypeID(Com_PartFamilyType.Text.Trim());
        }
    }
}
