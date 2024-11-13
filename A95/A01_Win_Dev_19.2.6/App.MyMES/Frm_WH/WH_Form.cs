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
using System.Diagnostics;
using App.MyMES.Report;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using DevExpress.XtraEditors;

namespace App.MyMES
{
    public partial class WH_Form : DevExpress.XtraEditors.XtraForm
    {
        public WH_Form()
        {
            InitializeComponent();
        }

        string S_FInterID = "";
        string S_FDetailID = "";
        string S_FBillNO = "";
        PartSelectSVCClient PartSelectSVC;
        WH_Edit_Form v_WH_Edit_Form = new WH_Edit_Form();
        WH_Edit_Detail_Form v_WH_Edit_Detail_Form = new WH_Edit_Detail_Form();
        Public_ public_ = new Public_();

        public LoginList List_Login = new LoginList();
        private bool IsShowShipBtn = false;
        private void WH_Form_Load(object sender, EventArgs e)
        {
            Date_Start.DateTime = DateTime.Now.AddDays(-7);
            Date_End.DateTime = DateTime.Now.AddDays(+1);

            PartSelectSVC = PartSelectFactory.CreateServerClient();
            public_.AddWHType(Com_Type, Grid_Type);

            List_Login = this.Tag as LoginList;
            string tmpRes = PartSelectSVC.GetMesStationAndStationTypeDetail(List_Login.StationID, "IsShowShipBtn", "", out string IsShowShipBtnStr);
            if (tmpRes == "1" && IsShowShipBtnStr == "1")
            {
                IsShowShipBtn = true;
            }
        }

        private void MySearch()
        {
            string S_Start = Date_Start.DateTime.ToString("yyyy-MM-dd");
            string S_End = Date_End.DateTime.ToString("yyyy-MM-dd");
            string S_TypeID = Com_Type.EditValue.ToString();   //Grid_Type.GetRowCellValue(Grid_Type.GetSelectedRows()[0], "ID").ToString();

            DataTable DT_Main = PartSelectSVC.GetShipmentNew(S_Start, S_End, S_TypeID).Tables[0];
            Grid_Main.DataSource = DT_Main;

            //Btn_Modify.Enabled = false;
            //Btn_Del.Enabled = false;

            //Btn_Modify2.Enabled = false;
            //Btn_Del2.Enabled = false;

            try
            {
                string S_FStatus = Grid_ViewMain.GetRowCellValue(Grid_ViewMain.GetSelectedRows()[0], "FStatus").ToString();
                if (S_FStatus == "0")
                {
                    Btn_New2.Enabled = true;
                }
                else
                {
                    Btn_New2.Enabled = false;
                }
            }
            catch
            {
                Btn_New2.Enabled = false;
            }
        }

        private void MySearchDetail()
        {
            try
            {
                int I_SelectRowID = Grid_ViewMain.GetFocusedDataSourceRowIndex();

                //S_FInterID = Grid_ViewMain.GetRowCellValue(Grid_ViewMain.GetSelectedRows()[0], "FInterID").ToString();
                //S_FBillNO = Grid_ViewMain.GetRowCellValue(Grid_ViewMain.GetSelectedRows()[0], "FBillNO").ToString();

                S_FInterID = Grid_ViewMain.GetRowCellValue(I_SelectRowID, "FInterID").ToString();
                S_FBillNO = Grid_ViewMain.GetRowCellValue(I_SelectRowID, "FBillNO").ToString();

                DataTable DT_Detail = PartSelectSVC.GetShipmentEntryNew(S_FInterID).Tables[0];
                Grid_Detail.DataSource = DT_Detail;

                //Btn_Modify.Enabled = true;
                //Btn_Del.Enabled = true;

                try
                {
                    //string S_FStatus = Grid_ViewMain.GetRowCellValue(Grid_ViewMain.GetSelectedRows()[0], "FStatus").ToString();
                    //if (S_FStatus == "0")
                    //{
                    //    Btn_Modify.Enabled = true;
                    //    Btn_Del.Enabled = true;
                    //    Btn_Audit.Enabled = true;

                    //    Btn_New2.Enabled = true;
                    //}
                    //else
                    //{
                    //    Btn_Modify.Enabled = false;
                    //    Btn_Del.Enabled = false;
                    //    Btn_Audit.Enabled = false;

                    //    Btn_New2.Enabled = false;
                    //}

                    string S_TypeID = Com_Type.EditValue.ToString();
                    if (S_TypeID == "0")
                    {
                        Btn_New2.Enabled = true;
                        Btn_Modify2.Enabled = true;
                        Btn_Del2.Enabled = true;
                    }
                    else
                    {
                        Btn_New2.Enabled = false;
                        Btn_Modify2.Enabled = false;
                        Btn_Del2.Enabled = false;
                    }

                }
                catch
                {
                    Btn_New2.Enabled = false;
                }
            }
            catch
            { }
        }

        private void DetailBtnStatus()
        {
            try
            {
                S_FDetailID = Grid_ViewDetail.GetRowCellValue(Grid_ViewDetail.GetSelectedRows()[0], "FDetailID").ToString();
                string S_FStatus = Grid_ViewMain.GetRowCellValue(Grid_ViewMain.GetSelectedRows()[0], "FStatus").ToString();

                if (Convert.ToInt32(S_FDetailID) < 1000 && S_FStatus == "0")
                {
                    Btn_New2.Enabled = true;
                    Btn_Modify2.Enabled = true;
                    Btn_Del2.Enabled = true;
                }
                else
                {
                    Btn_New2.Enabled = false;
                    Btn_Modify2.Enabled = false;
                    Btn_Del2.Enabled = false;
                }
            }
            catch { }
        }

        private void Btn_Confirm_Click(object sender, EventArgs e)
        {
            MySearch();
        }

        private void Grid_ViewMain_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            MySearchDetail();
        }

        private void Grid_Main_Click(object sender, EventArgs e)
        {
            MySearchDetail();
        }
        private void Grid_Detail_Click(object sender, EventArgs e)
        {
            DetailBtnStatus();
        }

        private void Grid_ViewDetail_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DetailBtnStatus();
        }

        private void Grid_ViewMain_MouseDown(object sender, MouseEventArgs e)
        {
           
        }

        private void Grid_ViewMain_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
                 
        }

        private void Btn_Report_Click(object sender, EventArgs e)
        {
            MySearch();

            WH_Report _Report = new WH_Report();
            string S_Start = Date_Start.DateTime.ToString("yyyy-MM-dd");
            string S_End = Date_End.DateTime.ToString("yyyy-MM-dd");

            string S_TypeID = Com_Type.EditValue.ToString();
            DataTable DT_Report = PartSelectSVC.GetShipmentReport(S_Start, S_End, S_TypeID).Tables[0];

            DT_Report.TableName = "WGQuery";            
            _Report.DataSource = DT_Report;
            _Report.DataMember = "WGQuery";
            _Report.PrintingSystem.ContinuousPageNumbering = true;
            ReportPrintTool printTool = new ReportPrintTool(_Report);
            printTool.ShowPreviewDialog();
            _Report.ShowPreview();
        }

        private void Grid_ViewMain_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "MyStatus") //xxxx为需要变色的那一列绑定的字段名
            {
                DataRow dr = Grid_ViewMain.GetDataRow(e.RowHandle);
                string S_FStatus = dr["FStatus"].ToString().Trim();//获取单元格数据
                  
                if (S_FStatus == "0")
                { 
                    e.Appearance.BackColor = Color.Red;
                }
                else if (S_FStatus == "1")
                {
                    e.Appearance.BackColor = Color.MediumSpringGreen;
                }
                else if (S_FStatus == "2")
                {
                    e.Appearance.BackColor = Color.Green;
                }
                else if (S_FStatus == "3")
                {
                    e.Appearance.BackColor = Color.LightGreen;
                }
                else if (S_FStatus == "4")
                {
                    e.Appearance.BackColor = Color.GreenYellow;
                }
                else if (S_FStatus == "5")
                {
                    e.Appearance.BackColor = Color.DarkGreen;
                }
                else if (S_FStatus == "6")
                {
                    e.Appearance.BackColor = Color.Gray;
                }
            }

            //if (e.Column.Caption == "Modify")
            //{
            //    DataRow dr = Grid_ViewMain.GetDataRow(e.RowHandle);
            //    string S_FStatus = dr["FStatus"].ToString().Trim();//获取单元格数据

            //    if (S_FStatus == "0")
            //    {
            //        reBtn_Modify.Buttons[0].Enabled = true;                    
            //    }
            //    else
            //    {
            //        reBtn_Modify.Buttons[0].Enabled = false;                    
            //    }
            //}

            //if (e.Column.Caption == "Delete")
            //{
            //    DataRow dr = Grid_ViewMain.GetDataRow(e.RowHandle);
            //    string S_FStatus = dr["FStatus"].ToString().Trim();//获取单元格数据

            //    if (S_FStatus == "0")
            //    {                    
            //        reBtn_Del.Buttons[0].Enabled = true;
            //    }
            //    else
            //    {                   
            //        reBtn_Del.Buttons[0].Enabled = false;
            //    }
            //}
        }

        private void Btn_Add_Click(object sender, EventArgs e)
        {
            v_WH_Edit_Form.Show_WH_Edit_Form(v_WH_Edit_Form, "New", "", PartSelectSVC, MySearch);
        }

        private void Btn_Modify_Click(object sender, EventArgs e)
        {
            v_WH_Edit_Form.Show_WH_Edit_Form(v_WH_Edit_Form, "Mod", S_FInterID, PartSelectSVC, MySearch);
            //Btn_Modify.Enabled = false;  
        }

        private void Btn_Del_Click(object sender, EventArgs e)
        {
            //Btn_Del.Enabled = false;
            if (MessageBox.Show("Sure to delete "+S_FBillNO +" ?", "MSG", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                string S_Result = PartSelectSVC.DeleteShipmentNew(S_FInterID);
                if (S_Result == "1")
                {
                    MySearch();
                }
                else
                {
                    MessageBox.Show(S_Result, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Btn_Audit_Click(object sender, EventArgs e)
        {
            UpdateFStatus("1");
        }

        private void Btn_Shipped_Click(object sender, EventArgs e)
        {
            UpdateFStatus("3");
        }

        private void UpdateFStatus(string FStatus)
        {
            int[] List_Select = Grid_ViewMain.GetSelectedRows();
            if (List_Select.Count() == 0)
            {
                MessageBox.Show("Please select data ", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string S_FInterIDList = "";
            int i = 1;
            foreach (var item in List_Select)
            {
                string S_ID = Grid_ViewMain.GetRowCellValue(item, "FInterID").ToString();
                string S_FStatus=Grid_ViewMain.GetRowCellValue(item, "FStatus").ToString();
                string S_FBillNO=Grid_ViewMain.GetRowCellValue(item, "FBillNO").ToString();

                string S_PerStatus =(Convert.ToInt32(FStatus) -1).ToString();
                if (S_FStatus == S_PerStatus)
                {
                    if (i == List_Select.Count())
                    {
                        S_FInterIDList += "'" + S_ID + "'";
                    }
                    else
                    {
                        S_FInterIDList += "'" + S_ID + "',";
                    }
                    i += 1;
                }
                else
                {
                    MessageBox.Show(S_FBillNO + " Status Error!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            string S_Result = "No Data!";
            if (S_FInterIDList != "")
            {
                S_Result = PartSelectSVC.UpdateShipmentNew_FStatus(S_FInterIDList, FStatus);
            }

            if (S_Result == "1")
            {
                MessageBox.Show("Save succeed!", "MSG", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MySearch();
            }
            else
            {
                MessageBox.Show(S_Result, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Grid_Main_DoubleClick(object sender, EventArgs e)
        {
            //Btn_Modify_Click(sender, e);
            v_WH_Edit_Form.Show_WH_Edit_Form(v_WH_Edit_Form, "Att", S_FInterID, PartSelectSVC, MySearch);
        }


        private void Btn_New2_Click(object sender, EventArgs e)
        {
            v_WH_Edit_Detail_Form.Show_WH_Edit_Detail(v_WH_Edit_Detail_Form, "New", S_FInterID, S_FDetailID, PartSelectSVC, MySearchDetail);
            Btn_New2.Enabled = false;
        }

        private void Btn_Modify2_Click(object sender, EventArgs e)
        {
            v_WH_Edit_Detail_Form.Show_WH_Edit_Detail(v_WH_Edit_Detail_Form, "Mod", S_FInterID, S_FDetailID, PartSelectSVC, MySearchDetail);
            Btn_Modify2.Enabled = false;
        }

        private void Btn_Delete2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure to delete Detail ?", "MSG", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                string S_Result = PartSelectSVC.Edit_CO_WH_ShipmentEntryNew
                    (S_FDetailID, "0", S_FInterID, "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0","0", "Del");
                if (S_Result == "1")
                {
                    MySearchDetail();
                }
                else
                {
                    MessageBox.Show(S_Result, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            Btn_Del2.Enabled = false;
        }

        private void Grid_ViewDetail_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void Com_Type_EditValueChanged(object sender, EventArgs e)
        {
            string S_TypeID = Com_Type.EditValue.ToString();

            if (S_TypeID == "0")
            {
                Btn_Modify.Enabled = true;
                Btn_Del.Enabled = true;
                Btn_Audit.Enabled = true;                
            }
            else
            {
                Btn_Modify.Enabled = false;
                Btn_Del.Enabled = false;
                Btn_Audit.Enabled = false;
            }

            if (S_TypeID == "2")
            {
                if (IsShowShipBtn)
                {
                    Btn_Shipped.Enabled = true;
                }
            }
            else
            {
                Btn_Shipped.Enabled = false;
            }
            MySearch();

            Grid_Detail.DataSource = null; 
        }

        private void Grid_ViewMain_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //if (e.RepositoryItem.Name == "reBtn_Modify")
            //{

            //}
            //else if (e.RepositoryItem.Name == "reBtn_Del")
            //{

            //}
        }

        private void reBtn_Modify_Click(object sender, EventArgs e)
        {
            int I_SelectRowID = Grid_ViewMain.GetFocusedDataSourceRowIndex();
            string S_FStatus = Grid_ViewMain.GetRowCellValue(I_SelectRowID, "FStatus").ToString();

            if (S_FStatus == "0")
            {
                Btn_Modify_Click(sender, e);
            }
            else
            {
                MessageBox.Show("FStatus mismatch","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void reBtn_Del_Click(object sender, EventArgs e)
        {
            int I_SelectRowID = Grid_ViewMain.GetFocusedDataSourceRowIndex();
            string S_FStatus = Grid_ViewMain.GetRowCellValue(I_SelectRowID, "FStatus").ToString();

            if (S_FStatus == "0")
            {
                Btn_Del_Click(sender, e);
            }
            else
            {
                MessageBox.Show("FStatus mismatch", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }           
        }
    }
}
