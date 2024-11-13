using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
using System.IO;
using DevExpress.Data.Helpers;
using Newtonsoft.Json;

namespace App.MyMES
{
    public partial class MaterialImport_Form : FrmBase3
    {
        ImesMaterialUnitSVCClient mesMaterialSVC;
        DataTable dtVendor;
        string SN_Pattern = string.Empty;
        private string BinID = string.Empty;

        public MaterialImport_Form()
        {
            InitializeComponent();
        }

        public override void FrmBase_Load(object sender, EventArgs e)
        {
            mesMaterialSVC = ImesMaterialUnitFactory.CreateServerClient();
            base.FrmBase_Load(sender, e);
            public_.OpenBartender(List_Login.StationID.ToString());
            var isFileUpload = PartSelectSVC.GetMesStationAndStationTypeDetail(List_Login.StationID, "isFileUpload", "", out string result);
            if (result == "1" && isFileUpload == "1")
            {
                tabNavigationPageFile.PageVisible = true;
                tabNavigationPageScan.PageVisible = false;
                tabPane.SelectedPageIndex = 0;
                
            }
            else
            {
                tabNavigationPageFile.PageVisible = false;
                tabNavigationPageScan.PageVisible = true;
                tabPane.SelectedPageIndex = 1;

            }
        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);

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
                
                //查询SN正则表达式规则
                 dataSet = PartSelectSVC.GetmesPartDetail(I_PartID, "SN_Pattern");
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    SN_Pattern = dataSet.Tables[0].Rows[0]["Content"].ToString();
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


                Com_PartFamilyType.Enabled = false;
                Com_PartFamily.Enabled = false;
                Com_Part.Enabled = false;
                Com_PO.Enabled = false;
                base.GrpControlInputData.Enabled = true;
                base.GetProductionCount();

                dataGridView1.DataSource = null;
                BinID = "";
                txtBin.Text = "";
                txtBin.Enabled = true;
                txtBin.Tag = 1;
                btnLock.Text = "Lock(锁定)";
                txtSN.Text = "";
                txtBin.Focus();
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }


        }

        private void btn_selectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Filter = "CSV File(*.csv)|*.csv|All Files(*.*)|*.*";
            openFileDialog.Multiselect = false;
            openFileDialog.CheckFileExists = true;
            openFileDialog.Title = "please Select CSV file.";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txt_FilePath.Text = openFileDialog.FileName;

                var lsSnModels = ReadCsv(txt_FilePath.Text);
                if (lsSnModels != null && lsSnModels.Any())
                    checkData(lsSnModels);

                dataGridView1.DataSource = lsSnModels;
            }

        }


        public  List<SnModel> ReadCsv(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            List<SnModel>  snList = new List<SnModel>();
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                if (lines.Length <= 1)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "NG", "please check file format 1.");
                    return null;
                }

                string columnName = lines[0];
                string[] tmpColumns = columnName.Split(',');

                for (int i = 1; i < lines.Length; i++)
                {
                    var tmpValues = lines[i].Split(',');
                    if (tmpValues.Length != 2)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "NG", "please check file format 2.");
                        return null;
                    }

                    if (!string.IsNullOrEmpty(Batch_Pattern))
                    {
                        if (!Regex.IsMatch(tmpValues[0], Batch_Pattern))
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"{tmpValues[0]} Batch_Pattern check filed (Bin SN format error).");
                            return null;
                        }
                    }

                    if (!string.IsNullOrEmpty(SN_Pattern))
                    {
                        if (!Regex.IsMatch(tmpValues[1], SN_Pattern))
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"{tmpValues[1]} SN_Pattern check filed.");
                            return null;
                        }
                    }

                    snList.Add(new SnModel()
                    {
                        MainSN = tmpValues[0],
                        ChildSN = tmpValues[1]
                    });
                }
            }
            catch (Exception e)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "NG", "Exception : " + e.Message);
                return null;
            }
            return snList;
        }

        private void btn_Upload_Click(object sender, EventArgs e)
        {

            var tmpData = dataGridView1.DataSource as List<SnModel>;
            if (tmpData is null || string.IsNullOrEmpty(txt_FilePath.Text.Trim()))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "NG", "Please select one csv file.");
                return;
            }
            var res = public_.UploadSNList(List_Login.EmployeeID.ToString(), List_Login.StationID.ToString(),
                List_Login.LineID.ToString(), Com_Part.EditValue.ToString(), PartSelectSVC);
            if (res != "1")
            {
                MessageInfo.Add_Info_MSG(Edt_MSG,  "NG", "Exception : " + res);
                return;
            }
            MessageInfo.Add_Info_MSG(Edt_MSG, "10028", "OK", List_Login.Language);
            dataGridView1.DataSource = null;
        }

        private void btnLock_Click(object sender, EventArgs e)
        {
            if (btnLock.Tag.ToString() == "1")
            {
                if (!checkBinInput(txtBin.Text.Trim()))
                    return;

                btnLock.Tag = "0";
                btnLock.Text = "Unlock(解除)";
                txtBin.Enabled = false;

                MessageInfo.Add_Info_MSG(Edt_MSG, "OK", $"{txtBin.Text.Trim()} insert successes.");
                txtSN.SelectAll();
                txtSN.Focus();
            }
            else
            {
                btnLock.Tag = "1";
                btnLock.Text = "Lock(锁定)";
                txtBin.Enabled = true;
                txtBin.Text = "";
                BinID = "";
            }
        }
        private void txtBin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            btnLock_Click(null,null);

        }

        private bool checkBinInput(string binSN)
        {
            bool result = false;
            if (string.IsNullOrEmpty(binSN))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "NG", "Please key down Bin number.");
                txtBin.SelectAll();
                txtBin.Focus();
                return result;
            }

            if (!Regex.IsMatch(binSN, Batch_Pattern))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"{binSN} Batch_Pattern check filed.");
                txtBin.SelectAll();
                txtBin.Focus();
                return result;
            }
            //查询是否存在，不存在则直接插入, 并拿到ID
            var tmpBinID = public_.UploadSingleBin(txtBin.Text.Trim(), PartSelectSVC);
            if (string.IsNullOrEmpty(tmpBinID))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"{txtBin.Text.Trim()} insert failed.");
                txtBin.SelectAll();
                txtBin.Focus();
                return result;
            }

            BinID = tmpBinID;
            return result = true;
        }


        private void txtSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (string.IsNullOrEmpty(txtBin.Text.Trim()) || txtBin.Enabled || string.IsNullOrEmpty(BinID))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"please key down Bin number at first.");
                txtBin.SelectAll();
                txtBin.Focus();
                return;
            }

            string tmpSn = txtSN.Text.Trim();
            if (string.IsNullOrEmpty(tmpSn))
            {
                txtSN.SelectAll();
                txtSN.Focus();
                return;
            }

            if (!Regex.IsMatch(tmpSn, SN_Pattern))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"{tmpSn} SN_Pattern check filed.");
                txtSN.SelectAll();
                txtSN.Focus();
                return;
            }

            var r = public_.UploadSingleSN(tmpSn, BinID, Com_Part.EditValue.ToString(), PartSelectSVC);
            if (r != "1")
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"{tmpSn} insert failed, please check it already exist.");
                txtSN.SelectAll();
                txtSN.Focus();
                return;
            }
            MessageInfo.Add_Info_MSG(Edt_MSG, "OK", $"{tmpSn} insert successes.");
            txtSN.Text = "";
            txtSN.Focus();
        }

        public void checkData(List<SnModel> lsSnModels)
        {
            public_.CheckTabExist(PartSelectSVC);
            PartSelectSVC.InsertBulkData(lsSnModels.ToArray());
            MessageInfo.Add_Info_MSG(Edt_MSG, "OK", $"check file finish.");
        }
    }
}