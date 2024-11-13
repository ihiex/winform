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
using System.Text.RegularExpressions;
using System.Globalization;

namespace App.MyMES
{
    public partial class MaterialRePrint_Form : FrmBase3
    {
        LabelManager2.Application LabSN;
        ImesMaterialUnitSVCClient mesMaterialSVC;

        DataTable dtVendor;
        string MaterialLable = "0";

        public MaterialRePrint_Form()
        {
            InitializeComponent();
        }

        public override void FrmBase_Load(object sender, EventArgs e)
        {
            mesMaterialSVC = ImesMaterialUnitFactory.CreateServerClient();
            base.FrmBase_Load(sender, e);
            public_.OpenBartender(List_Login.StationID.ToString());

            btnReprint.Visible = true;
        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            ResetControl();
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
                if (Com_PO.EditValue == null || string.IsNullOrEmpty(Com_PO.Text.ToString()))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20118", "NG", List_Login.Language);
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


                //是否打印条码
                dataSet = PartSelectSVC.GetmesPartDetail(I_PartID, "MaterialLable");
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    MaterialLable = dataSet.Tables[0].Rows[0]["Content"].ToString();
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

                DataSet ds = PartSelectSVC.GetVendor(Com_Part.EditValue.ToString());
                if (ds != null && ds.Tables.Count > 0)
                {
                    dtVendor = ds.Tables[0];
                }
                VendorID.Properties.DataSource = dtVendor;
                VendorID.Properties.DisplayMember = "Description";
                VendorID.Properties.ValueMember = "ID";

                Com_PartFamilyType.Enabled = false;
                Com_PartFamily.Enabled = false;
                Com_Part.Enabled = false;
                Com_PO.Enabled = false;
                base.GrpControlInputData.Enabled = true;
                base.GetProductionCount();
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }

        }



        private void ResetControl()
        {

            LotCode.Text = string.Empty;

            txtRollCode.Text = string.Empty;

        }

        private void btnReprint_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(LotCode.Text.Trim()))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20144", "NG", List_Login.Language);
                return;
            }
            string S_BatchSN = LotCode.Text.Trim().ToUpper();
            string S_PartID = Com_Part.EditValue.ToString();
            DataSet dsBatch = mesMaterialSVC.GetMesMaterialUnitByLotCode(S_PartID, S_BatchSN);
            if(dsBatch==null || dsBatch.Tables.Count==0 || dsBatch.Tables[0].Rows.Count==0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20028", "NG", List_Login.Language, new string[] { S_BatchSN });
                return;
            }

            string SN = dsBatch.Tables[0].Rows[0]["SerialNumber"].ToString();

            try
            {
                string S_LabelName = public_.GetLabelName(PartSelectSVC, List_Login.StationTypeID.ToString(), Com_PartFamily.EditValue.ToString(),
                                Com_Part.EditValue.ToString(), "", List_Login.LineID.ToString());
                if(string.IsNullOrEmpty(S_LabelName))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20104", "NG", List_Login.Language);
                    return; 
                }

                DataTable DT_PrintSn = new DataTable();
                DT_PrintSn.Columns.Add("SN");
                DT_PrintSn.Columns.Add("CreateTime");
                DT_PrintSn.Columns.Add("PrintTime");
                DT_PrintSn.Columns.Add("TmpPath");
                DT_PrintSn.Columns.Add("PartID");
                DataRow dr = DT_PrintSn.NewRow();
                dr["SN"] = SN;
                dr["PartID"] = Com_Part.EditValue.ToString();
                dr["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DT_PrintSn.Rows.Add(dr);

                string S_PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, S_LabelName, DT_PrintSn, Convert.ToInt32(Com_Part.EditValue.ToString()));
                if (S_PrintResult != "OK")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(S_PrintResult.Replace("ERROR", ""), List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20111", "NG", List_Login.Language, new string[] { ProMsg }, ProMsg);
                    return;
                }
                else
                {
                    DataSet dsSetPr = PartSelectSVC.GetmesSerialNumber(SN);
                    string unit = dsSetPr.Tables[0].Rows[0]["UnitID"].ToString();
                    mesHistory v_mesHistory = new mesHistory();
                    v_mesHistory.UnitID = Convert.ToInt32(unit);
                    v_mesHistory.UnitStateID = -300;
                    v_mesHistory.EmployeeID = List_Login.EmployeeID;
                    v_mesHistory.StationID = List_Login.StationID;
                    v_mesHistory.EnterTime = DateTime.Now;
                    v_mesHistory.ExitTime = DateTime.Now;
                    v_mesHistory.ProductionOrderID = Convert.ToInt32(Com_PO.EditValue.ToString());
                    v_mesHistory.PartID = Convert.ToInt32(Com_Part.EditValue.ToString());
                    v_mesHistory.LooperCount = 1;
                    mesHistorySVC.Insert(v_mesHistory);

                    MessageInfo.Add_Info_MSG(Edt_MSG, "10032", "OK", List_Login.Language, new string[] { SN });
                }
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20111", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                return;
            }
        }


    }
}