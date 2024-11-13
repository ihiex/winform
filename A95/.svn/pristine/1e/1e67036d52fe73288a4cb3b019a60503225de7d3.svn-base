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
    public partial class MaterialInitial_Form : FrmBase3
    {
        LabelManager2.Application LabSN;
        ImesMaterialUnitSVCClient mesMaterialSVC;
        int MaterialBatchQTY;
        string MaterialCodeRules;
        DataTable dtVendor;
        string MaterialAuto = "0";
        string MaterialLable = "0";
        int ExpiresTime = 0;
        int UnitConversionPCS = 0;

        public MaterialInitial_Form()
        {
            InitializeComponent();
        }

        public override void FrmBase_Load(object sender, EventArgs e)
        {
            mesMaterialSVC = ImesMaterialUnitFactory.CreateServerClient();
            base.FrmBase_Load(sender, e);
            public_.OpenBartender(List_Login.StationID.ToString());

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
                //批次数量
                dataSet = PartSelectSVC.GetmesPartDetail(I_PartID, "MaterialBatchQTY");
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    MaterialBatchQTY = Convert.ToInt32(dataSet.Tables[0].Rows[0]["Content"].ToString());
                    Quantity.Text = MaterialBatchQTY.ToString();
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20148", "NG", List_Login.Language);
                    return;
                }

                //是否打印条码
                dataSet = PartSelectSVC.GetmesPartDetail(I_PartID, "MaterialLable");
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    MaterialLable = dataSet.Tables[0].Rows[0]["Content"].ToString();
                }

                //收料单位转换
                dataSet = PartSelectSVC.GetmesPartDetail(I_PartID, "M_UnitConversion_PCS");
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    UnitConversionPCS = Convert.ToInt32(dataSet.Tables[0].Rows[0]["Content"].ToString());
                    this.txtUnit.SelectedIndex = 1;
                }
                else
                {
                    this.txtUnit.SelectedIndex = 0;
                }

                //是否自动识别客户条码
                dataSet = PartSelectSVC.GetmesPartDetail(I_PartID, "MaterialAuto");
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    MaterialAuto = dataSet.Tables[0].Rows[0]["Content"].ToString();
                    if(MaterialAuto=="1")
                    {
                        //条码规则
                        dataSet = PartSelectSVC.GetmesPartDetail(I_PartID, "MaterialCodeRules");
                        if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            MaterialCodeRules = dataSet.Tables[0].Rows[0]["Content"].ToString();
                            base.GrpControlInputData.Enabled = true;
                            TranceCode.Enabled = true;
                            TranceCode.Focus();
                        }
                        else
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20149", "NG", List_Login.Language);
                            return;
                        }

                        dataSet = PartSelectSVC.GetmesPartDetail(I_PartID, "Expires_Time");
                        if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            ExpiresTime = Convert.ToInt32(dataSet.Tables[0].Rows[0]["Content"].ToString());
                        }
                        LotCode.Enabled = false;
                    }
                    else
                    {
                        TranceCode.Enabled = false;
                        LotCode.Enabled = true;
                    }
                }

                ExpiringTime.Enabled = MaterialAuto != "1";

                //加载分配物料信息
                string Result = string.Empty;
                string xmlPartStr = "<Part PartID=\"" + Com_Part.EditValue.ToString() + "\"> </Part>";
                dataSet = PartSelectSVC.uspCallProcedure("uspGetMaterailBomData", null,null, xmlPartStr, 
                    null, null, List_Login.StationTypeID.ToString(), ref Result);
                if (Result == "1" && dataSet != null)
                {
                    ckComBoxCtlBom.Properties.DataSource = dataSet.Tables[0];
                    ckComBoxCtlBom.Properties.ValueMember = "PartID";
                    ckComBoxCtlBom.Properties.DisplayMember = "Description";
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20173", "NG", List_Login.Language);
                    return;
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

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(VendorID.Text))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20143", "NG", List_Login.Language);
                return;
            }
            if (string.IsNullOrEmpty(LotCode.Text))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20144", "NG", List_Login.Language);
                return;
            }
            if (string.IsNullOrEmpty(txtUnit.Text.Trim()))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20193", "NG", List_Login.Language);
                return;
            }
            else
            {
                if(txtUnit.Text.Trim()=="m" && UnitConversionPCS==0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20194", "NG", List_Login.Language);
                    return;
                }
            }
            if (string.IsNullOrEmpty(Quantity.Text))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20145", "NG", List_Login.Language);
                return;
            }
            if (Convert.ToInt32(Quantity.Text.ToString()) <= 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20146", "NG", List_Login.Language);
                return;
            }
            if (string.IsNullOrEmpty(ExpiringTime.Text.ToString()) || Convert.ToDateTime(ExpiringTime.Text.ToString())<DateTime.Now)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20147", "NG", List_Login.Language);
                return;
            }
            if (!radBtnClose.Checked && string.IsNullOrEmpty(ckComBoxCtlBom.Text.Trim()))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20174", "NG", List_Login.Language);
                return;
            }

            string S_BatchSN = LotCode.Text.Trim().ToUpper();
            int BatchQTY = Convert.ToInt32(Quantity.Text.ToString());
            if (!Regex.IsMatch(S_BatchSN, Batch_Pattern))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20047", "NG", List_Login.Language, new string[] { S_BatchSN });
                LotCode.SelectAll();
                return;
            }
            if (BatchQTY > MaterialBatchQTY)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20150", "NG", List_Login.Language, new string[] { MaterialBatchQTY.ToString() });
                Quantity.SelectAll();
                return;
            }

            //调用通用过程
            string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>";
            string xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
            string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
            string xmlExtraData = "<ExtraData RollCode=\"" + txtRollCode.Text.ToUpper() + "\" " +
                                            "Unit =\"" + txtUnit.Text.ToString() + "\" " +
                                            "MaterialDate =\"" + MaterialDate.Text.Trim() + "\" " +
                                            "MPN =\"" + MPN.Text.Trim() + "\" " +
                                            "DateCode =\"" + DateCode.Text.Trim() + "\" " +
                                            "ExpiringTime =\"" + ExpiringTime.Text.Trim() + "\" " +
                                            "TranceCode =\"" + TranceCode.Text.Trim() + "\"> </ExtraData>";
            string outString = string.Empty;
            PartSelectSVC.uspCallProcedure("uspMaterialUintCheck", LotCode.Text.ToUpper(),
                                                                    xmlProdOrder, xmlPart, xmlStation, xmlExtraData, null, ref outString);
            if (outString != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, outString, "NG", List_Login.Language, null, outString);
                return;
            }

            string S_PartID = Com_Part.EditValue.ToString();
            string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
            string S_FormatSN = PartSelectSVC.mesGetSNFormatIDByList(S_PartID, S_PartFamilyID,
                List_Login.LineID.ToString(), null, List_Login.StationTypeID.ToString());

            string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, S_PartFamilyID, List_Login.StationTypeID, 
                List_Login.LineID.ToString(), Com_PO.EditValue.ToString(),"1");
            if (string.IsNullOrEmpty(S_UnitStateID))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
                return;
            }

            if (!string.IsNullOrEmpty(S_FormatSN))
            {
                string vendorCode = dtVendor.Select("ID='" + VendorID.EditValue.ToString() + "'")[0]["Code"].ToString();
                xmlExtraData = "'<ExtraData BatchNo=" + "\"" + LotCode.Text.Trim().ToUpper() + "\" " +
                     "VendorCode =\"" + vendorCode + "\" "+
                    "RollCode =\"" + txtRollCode.Text.Trim().ToUpper() + "\" " + "> </ExtraData>'";

                DataSet DS = PartSelectSVC.uspSNRGetNext(S_FormatSN, 0, null, null, null, xmlExtraData, null);
                if (DS == null || DS.Tables[0].Rows.Count == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20087", "NG", List_Login.Language);
                    return;
                }
                S_BatchSN = DS.Tables[1].Rows[0][0].ToString();
            }
            else
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20132", "NG", List_Login.Language);
                return;
            }

            //校验批次是否重复
            DataSet dsBatch = mesMaterialSVC.GetMesMaterialUnitByLotCode(S_PartID, S_BatchSN);
            if (dsBatch != null && dsBatch.Tables.Count > 0 && dsBatch.Tables[0].Rows.Count > 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20151", "NG", List_Login.Language);
                LotCode.SelectAll();
                return;
            }

            MesMaterialUnit mesMaterial = new MesMaterialUnit();
            if (TranceCode.Enabled && !string.IsNullOrEmpty(TranceCode.Text.Trim()))
            {
                mesMaterial.SerialNumber = TranceCode.Text.ToString();
            }
            else
            {
                mesMaterial.SerialNumber = S_BatchSN;
            }

            //判断条码是否重复
            DataSet DsMaterial = mesMaterialSVC.GetMesMaterialUnitData(mesMaterial.SerialNumber);
            if (DsMaterial != null && DsMaterial.Tables.Count > 0 && DsMaterial.Tables[0].Rows.Count > 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20029", "NG", List_Login.Language, new string[] { mesMaterial.SerialNumber });
                return;
            }
            mesMaterial.VendorID = Convert.ToInt32(VendorID.EditValue.ToString());
            mesMaterial.PartID= Convert.ToInt32(Com_Part.EditValue.ToString());
            mesMaterial.StatusID = 1;
            mesMaterial.EmployeeID = List_Login.EmployeeID;
            mesMaterial.LotCode= LotCode.Text.ToString().ToUpper();
            mesMaterial.DateCode = DateCode.Text.ToString();
            mesMaterial.TraceCode = TranceCode.Text.ToString();
            mesMaterial.MPN = MPN.Text.ToString();
            if (txtUnit.Text.Trim() == "pcs")
            {
                mesMaterial.Quantity = Convert.ToInt32(Quantity.Text.ToString());
            }
            else
            {
                mesMaterial.Quantity = Convert.ToInt32(Quantity.Text.ToString()) * UnitConversionPCS;
            }
            mesMaterial.RemainQTY = Convert.ToInt32(Quantity.Text.ToString());
            mesMaterial.LineID = List_Login.LineID;
            mesMaterial.RollCode = txtRollCode.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(ExpiringTime.Text))
            {
                mesMaterial.ExpiringTime = Convert.ToDateTime("2099-12-31");
            }
            else
            {
                mesMaterial.ExpiringTime = Convert.ToDateTime(ExpiringTime.Text.ToString());
            }
            mesMaterial.MaterialTypeID = 1;
            mesMaterial.StationID = List_Login.StationID;
            mesMaterial.MaterialDateCode = MaterialDate.Text.Trim();
            int MaterialUnitID = mesMaterialSVC.Insert(mesMaterial);


            MesMaterialUnitDetail mesMaterialDetail = new MesMaterialUnitDetail();
            mesMaterialDetail.MaterialUnitID = MaterialUnitID;
            mesMaterialDetail.LooperCount = 1;
            mesMaterialDetail.Reserved_02 = "";
            mesMaterialDetail.Reserved_03 = "";
            mesMaterialDetail.Reserved_04 = "";
            mesMaterialDetail.Reserved_05 = "";
            if (!radBtnClose.Checked)
            {
                //1:指定项目 2:排除项目
                if(radBtnAssign.Checked)
                {
                    mesMaterialDetail.Reserved_02 = "1";
                }
                else if(radBtnExclude.Checked)
                {
                    mesMaterialDetail.Reserved_02 = "2";
                }

                mesMaterialDetail.Reserved_01 = ckComBoxCtlBom.EditValue.ToString();
            }
            else
            {
                mesMaterialDetail.Reserved_01 = "";
            }
            mesMaterialSVC.InserDetail(mesMaterialDetail);

            mesUnit v_mesUnit = new mesUnit();
            v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
            v_mesUnit.StatusID = 1;
            v_mesUnit.StationID = List_Login.StationID;
            v_mesUnit.EmployeeID = List_Login.EmployeeID;
            v_mesUnit.CreationTime = DateTime.Now;
            v_mesUnit.LastUpdate = DateTime.Now;
            v_mesUnit.PanelID = 0;
            v_mesUnit.LineID = List_Login.LineID;
            v_mesUnit.ProductionOrderID = 0;
            v_mesUnit.RMAID = 0;
            v_mesUnit.PartID = Convert.ToInt32(S_PartID);
            v_mesUnit.LooperCount = 1;
            string S_InsertUnit = mesUnitSVC.Insert(v_mesUnit);

            mesUnitDetail v_mesUnitDetail = new mesUnitDetail();
            v_mesUnitDetail.UnitID = Convert.ToInt32(S_InsertUnit);
            v_mesUnitDetail.reserved_01 = mesMaterial.SerialNumber;
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
            v_mesSerialNumber.Value = mesMaterial.SerialNumber; 
            mesSerialNumberSVC.Insert(v_mesSerialNumber);

            //条码打印
            if (MaterialLable == "1")
            {
                try
                {
                    string S_LabelName = public_.GetLabelName(PartSelectSVC, List_Login.StationTypeID.ToString(), Com_PartFamily.EditValue.ToString(),
                                    Com_Part.EditValue.ToString(), "", List_Login.LineID.ToString());
                    if (string.IsNullOrEmpty(S_LabelName))
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
                    dr["SN"] = mesMaterial.SerialNumber;
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
                }
                catch(Exception ex)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20111", "NG", List_Login.Language,new string[] { ex.Message.ToString()});
                    return;
                }
            }
            MessageInfo.Add_Info_MSG(Edt_MSG, MaterialLable == "1" ? "10025" : "10024", "OK", List_Login.Language, new string[] { S_BatchSN });
            ResetControl();
            base.GetProductionCount();
        }

        private void ResetControl()
        {
            //VendorID.Text = string.Empty;
            LotCode.Text = string.Empty;
            MPN.Text = string.Empty;
            DateCode.Text = string.Empty;
            ExpiringTime.Text = string.Empty;
            TranceCode.Text = string.Empty;
            txtRollCode.Text = string.Empty;
            //txtUnit.Text = string.Empty;
            MaterialDate.Text = string.Empty;
            if (MaterialAuto == "1")
            {
                TranceCode.Enabled = true;
                TranceCode.Focus();
            }
            else
            {
                TranceCode.Enabled = false;
            }
        }

        private void TranceCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (string.IsNullOrEmpty(MaterialCodeRules))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20149", "NG", List_Login.Language);
                        return;
                    }

                    string StrTrance = TranceCode.Text.ToString();
                    string[] strList = MaterialCodeRules.Split(';');
                    foreach (string str in strList)
                    {
                        string ControlName = str.Split(':')[0].ToString();
                        string[] strListIndex = str.Split(':')[1].Split(',');
                        int StartIndex = Convert.ToInt32(strListIndex[0].ToString());
                        int Strlength = Convert.ToInt32(strListIndex[1].ToString());

                        foreach (Control ctr in GrpControlInputData.Controls)
                        {
                            if (ctr.Name.ToString() == ControlName)
                            {
                                string Controlvalue = StrTrance.Substring(StartIndex, Strlength);
                                if (ctr is SearchLookUpEdit && ControlName == "VendorID")
                                {
                                    DataRow[] drVendor = dtVendor.Select("Code='" + Controlvalue + "'");
                                    if (drVendor != null && drVendor.Length > 0)
                                    {
                                        (ctr as SearchLookUpEdit).EditValue = dtVendor.Select("Code='" + Controlvalue + "'")[0]["ID"].ToString();
                                    }
                                    break;
                                }
                                if (ctr is DateEdit)
                                {
                                    if (Controlvalue.Length == 6)
                                    {
                                        Controlvalue = "20" + Controlvalue;
                                    }
                                    (ctr as DateEdit).DateTime = DateTime.ParseExact(Controlvalue, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                                    break;
                                }

                                if (ctr is TextEdit)
                                {
                                    ctr.Text = Controlvalue;
                                    break;
                                }
                            }
                        }
                    }

                    //计算过期日期
                    if (!string.IsNullOrEmpty(MaterialDate.Text.Trim())&& ExpiresTime>0)
                    {
                        DateTime dateTime = DateTime.ParseExact(MaterialDate.Text.Trim(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                        string Expiring = dateTime.AddMonths(ExpiresTime).ToString("yyyyMMdd");
                        ExpiringTime.DateTime = DateTime.ParseExact(Expiring, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    }
                    ExpiringTime.Enabled = MaterialAuto != "1";
                }
            }
            catch(Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20078", "NG", List_Login.Language);
                return;
            }
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

        private void radBtnClose_CheckedChanged(object sender, EventArgs e)
        {
            if (!radBtnClose.Checked)
            {
                ckComBoxCtlBom.EditValue = string.Empty;
                ckComBoxCtlBom.Text = string.Empty;
                ckComBoxCtlBom.Properties.RefreshDataSource();
                ckComBoxCtlBom.Visible = true;

            }
            else
            {
                ckComBoxCtlBom.Visible = false;
            }
        }
    }
}