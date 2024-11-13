using App.Model;
using App.MyMES.PartSelectService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.MyMES
{
    public partial class Select_Form : DevExpress.XtraEditors.XtraForm
    {
        PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
        LoginList List_Login = new LoginList();
        Public_ public_ = new Public_();
        string treeViewSN = string.Empty;

        public Select_Form()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            tViewDetail.Nodes.Clear();
            treeViewSN = string.Empty;
            string xmlExtraData = "<ExtraData StartTime=\"" + dateTimeStart.Text.ToString() + " 00:00:00:000" + "\" " +
                                             "EndTime =\"" + dateTimeEnd.Text.ToString() + " 23:59:59:000" + "\" " +
                                             "UnitSN =\"" + txtUnitSN.Text.Trim() + "\" " +
                                             "Status =\"" + (comBoxStatus.EditValue==null ? "" : comBoxStatus.EditValue.ToString()) + "\" " +
                                             "PartFamilyTypeID =\"" + (Com_PartFamilyType.EditValue == null ? "" : Com_PartFamilyType.EditValue.ToString()) + "\" " +
                                             "PartFamilyID =\"" + (Com_PartFamily.EditValue == null ? "" : Com_PartFamily.EditValue.ToString()) + "\" " +
                                             "PartNumber =\"" + (Com_Part.EditValue == null ? "" : Com_Part.EditValue.ToString()) + "\" " +
                                             "ProductionOrder =\"" + (Com_PO.EditValue == null ? "" : Com_PO.EditValue.ToString()) + "\"> </ExtraData>";
            string strOutput = string.Empty;
            DataSet ds = PartSelectSVC.Get_SearchData(null, null, null, null, xmlExtraData, "Search", ref strOutput);
            if(strOutput!="1")
            {
                MessageBox.Show(strOutput, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                tViewDetail.DataSource = ds.Tables[0];
                tViewDetail.ExpandAll();

                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    //将所有父节点加到treeview上
                //    TreeNode nodeParent = new TreeNode();
                //    nodeParent.Text = dt.Rows[i]["SN"].ToString();
                //    tViewDetail.Nodes.Add(nodeParent);
                //}
                lblInfomation.Text = string.Empty;
            }
            else
            {
                tbControlDetail_SelectedPageChanged(null, null);
                lblInfomation.Text = "No Data";
            }
        }

        private void Select_Form_Load(object sender, EventArgs e)
        {
            dateTimeStart.DateTime = GetDateNow(DateTime.Now.AddDays(-7));
            dateTimeEnd.DateTime = GetDateNow(DateTime.Now);
            public_.AddPartFamilyType(Com_PartFamilyType, Grid_PartFamilyType);
            Com_PartFamilyType.EditValue = null;
            DataSet dataSet = PartSelectSVC.GetluUnitStatus();
            if(dataSet!=null && dataSet.Tables.Count>0)
            {
                comBoxStatus.Properties.DataSource = dataSet.Tables[0];
                comBoxStatus.Properties.DisplayMember = "Description";
                comBoxStatus.Properties.ValueMember = "ID";
            }
           
        }

        private void Com_PartFamilyType_EditValueChanged(object sender, EventArgs e)
        {
            if (Com_PartFamilyType.EditValue == null)
            {
                public_.AddPartFamily(Com_PartFamily, "", Grid_PartFamily);
            }
            else
            {
                string S_PartFamilyTypeID = Com_PartFamilyType.EditValue.ToString();
                public_.AddPartFamily(Com_PartFamily, S_PartFamilyTypeID, Grid_PartFamily);
            }
            Com_PartFamily.EditValue = null;
        }

        private void Com_PartFamily_EditValueChanged(object sender, EventArgs e)
        {
            if (Com_PartFamily.EditValue == null)
            {
                public_.AddPart(Com_Part, "", Grid_Part);
            }
            else
            {
                string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                public_.AddPart(Com_Part, S_PartFamilyID, Grid_Part);
            }
            Com_Part.EditValue = null;
        }

        private void Com_Part_EditValueChanged(object sender, EventArgs e)
        {
            if (Com_Part.EditValue == null)
            {
                public_.AddPOAll(Com_PO, "", "", Grid_PO);
            }
            else
            {
                string S_PartID = Com_Part.EditValue.ToString();
                public_.AddPOAll(Com_PO, S_PartID, "", Grid_PO);
            }
            Com_PO.EditValue = null;
        }


        private DateTime GetDateNow(DateTime dtime)
        {
            int year = dtime.Year;
            int month = dtime.Month;
            int day = dtime.Day;
            return new DateTime(year, month, day);
        }

        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if(e.Node==null || e.Node.ParentNode==null)
            {
                return;
            }
            string SN = e.Node["ChildUnitID"].ToString();

            string ParentSN = e.Node.ParentNode["SN"].ToString();
            string strOutput = string.Empty;
            string xmlExtraData = "<ExtraData ParentSN=\"" + ParentSN + "\"> </ExtraData>";
            DataSet ds = PartSelectSVC.Get_SearchData(SN, null, null, null, xmlExtraData, "TraceabilityDetail", ref strOutput);
            if (ds != null && ds.Tables.Count > 0)
            {
                dtGridTraceability.DataSource = ds.Tables[0];
            }
        }

        private void treeList2_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (e.Node == null)
            {
                return;
            }
            string SN = e.Node["SN"].ToString();
            string strOutput = string.Empty;
            DataSet ds = PartSelectSVC.Get_SearchData(SN, null, null, null, null, "PackageDetail", ref strOutput);
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables.Count > 0)
                {
                    dtGridPackageDetail.DataSource = ds.Tables[0];
                }
                if (ds.Tables.Count > 1)
                {
                    dtGridPackageHistory.DataSource = ds.Tables[1];
                }
            }
        }

        private void tViewDetail_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (e.Node == null)
                return;
            treeViewSN = e.Node.GetValue("SN").ToString();
            tbControlDetail_SelectedPageChanged(null, null);
        }

        private void tViewRoute_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (e.Node == null)
                return;
            string ID = e.Node.GetValue("ID").ToString();
            string strOutput = string.Empty;
            DataSet ds = PartSelectSVC.Get_SearchData(ID, null, null, null, null, "RouteInfoDetail", ref strOutput);
            if (ds != null && ds.Tables.Count > 0)
            {
                dtGridRouteDetail.DataSource = ds.Tables[0];
            }
        }

        private void tViewPart_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (e.Node == null)
                return;
            string UnitID = e.Node.GetValue("ChildUnitID").ToString();
            string strOutput = string.Empty;
            DataSet ds = PartSelectSVC.Get_SearchData(UnitID, null, null, null, null, "PartDetail", ref strOutput);
            if (ds != null && ds.Tables.Count > 0)
            {
                dtGridPart.DataSource = ds.Tables[0];
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            dateTimeStart.DateTime = GetDateNow(DateTime.Now.AddDays(-7));
            dateTimeEnd.DateTime = GetDateNow(DateTime.Now);
            txtUnitSN.Text = string.Empty;
            comBoxStatus.Text = string.Empty;
            Com_PartFamilyType.EditValue = string.Empty;
            Com_PartFamily.EditValue = string.Empty;
            Com_Part.EditValue = string.Empty;
            Com_PO.EditValue = string.Empty;
        }

        private void tbControlDetail_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                if (tbContolDetail.SelectedTabPage.Name == "tabPageUnitInfor")
                {
                    string strOutput = string.Empty;
                    DataSet ds = PartSelectSVC.Get_SearchData(treeViewSN, null, null, null, null, "UnitInfor", ref strOutput);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        //加载 General Information
                        if (ds.Tables.Count > 0)
                        {
                            DataTable DtGeneralInfo = ds.Tables[0];
                            dtGridGeneral.DataSource = DtGeneralInfo;
                        }

                        //加载 Detail Information
                        if (ds.Tables.Count > 1)
                        {
                            DataTable DtDetailInfo = ds.Tables[1];
                            dtGridDetail.DataSource = DtDetailInfo;
                        }

                        //加载 History
                        if (ds.Tables.Count > 2)
                        {
                            var tmpTab = ds.Tables[2];
                            tmpTab.Columns.Remove("ID");
                            dtViewHistory.DataSource = ds.Tables[2];
                        }

                        //加载 Defect
                        if (ds.Tables.Count > 3)
                        {
                            dtViewDefect.DataSource = ds.Tables[3];
                        }
                    }
                    else
                    {
                        dtGridGeneral.DataSource = null;
                        dtGridDetail.DataSource = null;
                        dtViewHistory.DataSource = null;
                        dtViewDefect.DataSource = null;
                    }

                }
                else if (tbContolDetail.SelectedTabPage.Name == "tabPageTraceability")
                {
                    string strOutput = string.Empty;
                    DataSet ds = PartSelectSVC.Get_SearchData(treeViewSN, null, null, null, null, "TraceabilityView", ref strOutput);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        //treeList1.DataSource = ds.Tables[0];
                        DataTable dts = new DataTable();
                        dts.Columns.Add("UnitID", typeof(string));
                        dts.Columns.Add("ChildUnitID", typeof(string));
                        dts.Columns.Add("SN", typeof(string));

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow drs = dts.NewRow();
                            drs["UnitID"] = dr["UnitID"].ToString();
                            drs["ChildUnitID"] = dr["ChildUnitID"].ToString();
                            drs["SN"] = dr["SN"].ToString();
                            dts.Rows.Add(drs);
                        }

                        treeList1.DataSource = dts;
                        treeList1.KeyFieldName = "ChildUnitID";
                        treeList1.ParentFieldName = "UnitID";
                        treeList1.Columns["SN"].Caption = "SN";
                        treeList1.ExpandAll();
                    }
                    else
                    {
                        treeList1.Nodes.Clear();
                        dtGridTraceability.DataSource = null;
                    }
                }
                else if (tbContolDetail.SelectedTabPage.Name == "tabPagePackage")
                {
                    string strOutput = string.Empty;
                    DataSet ds = PartSelectSVC.Get_SearchData(treeViewSN, null, null, null, null, "PackageView", ref strOutput);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        treeList2.DataSource = ds.Tables[0];
                        treeList2.KeyFieldName = "ChildUnitID";
                        treeList2.ParentFieldName = "UnitID";
                        treeList2.Columns["SN"].Caption = "SN";
                        treeList2.ExpandAll();

                        textBox1.Text = "";
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            textBox1.Text += ds.Tables[0].Rows[i]["SN"].ToString() + "\r\n";
                        }
                    }
                    else
                    {
                        treeList2.Nodes.Clear();
                    }
                }
                else if (tbContolDetail.SelectedTabPage.Name == "tabPageTestRecord")
                {
                    string strOutput = string.Empty;
                    DataSet ds = PartSelectSVC.Get_SearchData(treeViewSN, null, null, null, null, "TestRecord", ref strOutput);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        dtGridTestRecord.DataSource = ds.Tables[0];
                    }
                    else
                    {
                        dtGridTestRecord.DataSource = null;
                    }
                }
                else if (tbContolDetail.SelectedTabPage.Name == "tabPagePart")
                {
                    string strOutput = string.Empty;
                    DataSet ds = PartSelectSVC.Get_SearchData(treeViewSN, null, null, null, null, "PartView", ref strOutput);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        tViewPart.DataSource = ds.Tables[0];
                        tViewPart.KeyFieldName = "ChildUnitID";
                        tViewPart.ParentFieldName = "UnitID";
                        tViewPart.Columns["SN"].Caption = "SN";
                        tViewPart.ExpandAll();
                    }
                    else
                    {
                        tViewPart.Nodes.Clear();
                    }
                }
                else if (tbContolDetail.SelectedTabPage.Name == "tabPageProductionOrder")
                {
                    string strOutput = string.Empty;
                    DataSet ds = PartSelectSVC.Get_SearchData(treeViewSN, null, null, null, null, "ProductionOrder", ref strOutput);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            dtGridProductionOrder.DataSource = ds.Tables[0];
                        }
                        if (ds.Tables.Count > 1)
                        {
                            dtGridProductionOrderDetail.DataSource = ds.Tables[1];
                        }
                        if (ds.Tables.Count > 2)
                        {
                            dtGridLineOder.DataSource = ds.Tables[2];
                        }
                    }
                    else
                    {
                        dtGridProductionOrder.DataSource = null;
                        dtGridProductionOrderDetail.DataSource = null;
                        dtGridLineOder.DataSource = null;
                    }
                }
                else if (tbContolDetail.SelectedTabPage.Name == "tabPageRouteInfo")
                {
                    string strOutput = string.Empty;
                    DataSet ds = PartSelectSVC.Get_SearchData(treeViewSN, null, null, null, null, "RouteInfoView", ref strOutput);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        tViewRoute.DataSource = ds.Tables[0];
                        tViewRoute.Columns["ID"].Visible = false;
                    }
                    else
                    {
                        tViewRoute.Nodes.Clear();
                    }
                }
                else if (tbContolDetail.SelectedTabPage.Name == "tabPageMaterialUnit")
                {
                    string strOutput = string.Empty;
                    DataSet ds = PartSelectSVC.Get_SearchData(treeViewSN, null, null, null, null, "MaterialUnit", ref strOutput);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dtGridMaterialUnit.DataSource = ds.Tables[0];
                    }
                    else
                    {
                        dtGridTestRecord.DataSource = null;
                    }
                }
                else if (tbContolDetail.SelectedTabPage.Name == "tabPageMachine")
                {
                    string strOutput = string.Empty;
                    DataSet ds = PartSelectSVC.Get_SearchData(treeViewSN, null, null, null, null, "Machine", ref strOutput);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dtGridMachine.DataSource = ds.Tables[0];
                    }
                    else
                    {
                        dtGridTestRecord.DataSource = null;
                    }
                }
                else if (tbContolDetail.SelectedTabPage.Name == "tabPageShipment")
                {
                    string strOutput = string.Empty;
                    DataSet ds = PartSelectSVC.Get_SearchData(txtUnitSN.Text.Trim(), null, null, null, null, "Shipment", ref strOutput);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dtGridShipment.DataSource = ds.Tables[0];
                    }
                    else
                    {
                        dtGridShipment.DataSource = null;
                        dtGridMultipack.DataSource = null;
                    }
                }
            }
            catch (Exception ex) { }
        }

        private void gridViewShipment_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                int index = e.FocusedRowHandle;

                if (dtGridShipment.DataSource == null || index < 0)
                {
                    dtGridMultipack.DataSource = null;
                    return;
                }
                string ShipmentDetailID = gridViewShipment.GetRowCellValue(index, "FDetailID").ToString();
                string strOutput = string.Empty;
                DataSet ds = PartSelectSVC.Get_SearchData(ShipmentDetailID, null, null, null, null, "MultipackDetail", ref strOutput);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dtGridMultipack.DataSource = ds.Tables[0];
                }
                else
                {
                    dtGridMultipack.DataSource = null;
                }
            }
            catch(Exception ex) { }
        }

        private void txtUnitSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btnSelect_Click(sender, e);
                txtUnitSN.Text = "";
                txtUnitSN.Focus(); 
            }
        }


        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    gridView1.ExportToXlsx(saveFileDialog1.FileName);
                }
                catch
                {
                }
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    dtGridPackageHistory.ExportToXlsx(saveFileDialog1.FileName);
                }
                catch
                {
                }
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    dtGridMultipack.ExportToXlsx(saveFileDialog1.FileName);
                }
                catch
                {
                }
            }
        }
    }
}
