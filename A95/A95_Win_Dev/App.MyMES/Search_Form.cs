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
    public partial class Search_Form : Form
    {
        public Search_Form()
        {
            InitializeComponent();
        }

        Public_ public_ = new Public_();
        LoginList List_Login = new LoginList();

        public void Show_Search_Form(Search_Form v_Search_Form, LoginList v_LoginList)
        {
            List_Login = v_LoginList;
            v_Search_Form.Show();
        }

        private void Search_Form_Load(object sender, EventArgs e)
        {
            Date_Start.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            Date_End.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            Btn_Refresh_Click(sender, e);
        }

        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            //List_Login = this.Tag as LoginList;
            ///////////////////////////
            public_.AddPartFamilyType(Com_PartFamilyType,Grid_PartFamilyType);
            public_.AddluUnitStatus(Com_luUnitStatus, Grid_luUnitStatus);
            public_.AddluSerialNumberType(Com_SNType, Grid_SNType);

        }

        private void Com_PartFamilyType_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
 
        }

        private void Com_PartFamily_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {

        }
        private void Com_Part_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {

        }

        private void AddUnit(string S_Where)
        {
            try
            {
                string S_DateStart = Date_Start.Text.Trim();
                string S_DateEnd = Date_End.Text.Trim();

                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                DataTable DT = PartSelectSVC.GetUnit_Search(S_DateStart, S_DateEnd, S_Where).Tables[0] ;
                Grid_mesUnit.DataSource = DT;
                PartSelectSVC.Close();
            }
            catch
            {               
            }
        }


        private void Btn_Search1_Click(object sender, EventArgs e)
        {
            //DataRowView DRV = Com_Part.SelectedItem as DataRowView;
            string S_PartID = Com_Part.EditValue.ToString();
            string S_SnTypeID = Com_SNType.EditValue.ToString();

            if (S_PartID == "")
            {
                public_.Add_Info_MSG(Edt_MSG, "料号不能为空", "NG");
                return;
            }
            string S_Where = " and A.PartID=" + S_PartID;
            if (S_SnTypeID != "0")
            {
                 S_Where +=" and SN_TYPE.ID=" + S_SnTypeID;
            }


            AddUnit(S_Where);

            //Edt_MSG.Text = "";
            Edt_MSG.BackColor = Color.White;

            Grid_mesHistory.DataSource = null;
            Grid_Tool.DataSource = null;
            Grid_Defect.DataSource = null;  
        }

        private void Btn_Search2_Click(object sender, EventArgs e)
        {
            string S_Where = "";

            if (Check_PO.Checked==true)
            {
                //DataRowView DRV_PO = Com_PO.SelectedItem as DataRowView;
                string S_POID = Com_PO.EditValue.ToString();
                if (S_POID == "")
                {
                    public_.Add_Info_MSG(Edt_MSG, "请设置工单", "NG");
                    return;
                }

                S_Where += " and A.ProductionOrderID=" + S_POID;
            }

            if (Check_UnitState.Checked == true)
            {
                //DataRowView DRV_luUnitStatus = Com_luUnitStatus.SelectedItem as DataRowView;
                string S_Status = Com_luUnitStatus.EditValue.ToString();

                if (S_Status == "")
                {
                    public_.Add_Info_MSG(Edt_MSG, "请设置单元状态", "NG");
                    return;
                }

                S_Where += " and A.Status=" + S_Status;
            }

            if (Check_Batch.Checked == true)
            {               
                string S_BatchSN = Edt_Batch.Text.Trim();
                if (S_BatchSN == "")
                {
                    public_.Add_Info_MSG(Edt_MSG, "请设置批次号码", "NG");
                    return;
                }

                S_Where += " and A_Detail.reserved_02 like '%" + S_BatchSN+"%' ";
            }

            if (Check_SN.Checked == true)
            {
                string S_SN = Edt_SN.Text.Trim();

                if (S_SN == "")
                {
                    public_.Add_Info_MSG(Edt_MSG, "请设置SN号码", "NG");
                    return;
                }

                S_Where += " and SN.[Value] like '%" + S_SN+"%' ";
            }

            if (S_Where == "")
            {
                public_.Add_Info_MSG(Edt_MSG, "请设置查询条件", "NG");
                return;
            }

            AddUnit(S_Where);
            //Edt_MSG.Text = "";
            Edt_MSG.BackColor = Color.White;

            Grid_mesHistory.DataSource = null;
            Grid_UnitComponent.DataSource = null;
            Grid_Tool.DataSource = null;
            Grid_Defect.DataSource = null;
        }

        private void Grid_mesUnit_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string S_ID = Grid_mesUnit.CurrentRow.Cells["ID"].Value.ToString();
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            DataTable DT = PartSelectSVC.GetHistory(S_ID).Tables[0];

            Grid_mesHistory.DataSource = DT;
            public_.Grid_NGColor(Grid_mesHistory);
            AddMachineHistory(S_ID);
            AddUnitComponent(S_ID);
            AddmesUnitDetail(S_ID);

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

        private void AddMachineHistory(string S_UnitID)
        {
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            DataTable DT_Tool = PartSelectSVC.GetMachineHistory(S_UnitID).Tables[0];
            PartSelectSVC.Close();

            Grid_Tool.DataSource = DT_Tool;
        }

        private void AddUnitComponent(string S_UnitID)
        {
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            DataTable DT = PartSelectSVC.GetUnitComponent(S_UnitID).Tables[0];
            PartSelectSVC.Close();

            Grid_UnitComponent.DataSource = DT;

            for (int i = 0; i < this.Grid_UnitComponent.Columns.Count; i++)
            {
                this.Grid_UnitComponent.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
        }

        private void AddmesUnitDetail(string S_UnitID)
        {
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            DataTable DT = PartSelectSVC.GetmesUnitDetail(S_UnitID).Tables[0];
            PartSelectSVC.Close();

            Grid_UnitDetail.DataSource = DT;

            for (int i = 0; i < this.Grid_UnitDetail.Columns.Count; i++)
            {
                this.Grid_UnitDetail.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
        }


        private void M_Unit_Click(object sender, EventArgs e)
        {
            string S_Result= public_.GridToExcel(Grid_mesUnit);

            if (S_Result != "OK")
            {
                public_.Add_Info_MSG(Edt_MSG, S_Result, "NG");
            }
        }

        private void M_Component_Click(object sender, EventArgs e)
        {
            string S_Result = public_.GridToExcel(Grid_UnitComponent);

            if (S_Result != "OK")
            {
                public_.Add_Info_MSG(Edt_MSG, S_Result, "NG");
            }
        }

        private void M_Tool_Click(object sender, EventArgs e)
        {
            string S_Result = public_.GridToExcel(Grid_Tool);

            if (S_Result != "OK")
            {
                public_.Add_Info_MSG(Edt_MSG, S_Result, "NG");
            }
        }

        private void M_ScanList_Click(object sender, EventArgs e)
        {
            string S_Result = public_.GridToExcel(Grid_mesHistory);

            if (S_Result != "OK")
            {
                public_.Add_Info_MSG(Edt_MSG, S_Result, "NG");
            }
        }

        private void M_Defact_Click(object sender, EventArgs e)
        {
            string S_Result = public_.GridToExcel(Grid_Defect);

            if (S_Result != "OK")
            {
                public_.Add_Info_MSG(Edt_MSG, S_Result, "NG");
            }
        }

        private void Com_PartFamilyType_EditValueChanged(object sender, EventArgs e)
        {
            string S_ID = Com_PartFamilyType.EditValue.ToString();  
            public_.AddPartFamily(Com_PartFamily, S_ID, Grid_PartFamily);
        }

        private void Com_PartFamily_EditValueChanged(object sender, EventArgs e)
        {
            string S_ID = Com_PartFamily.EditValue.ToString();
            public_.AddPart(Com_Part, S_ID, Grid_Part);
        }

        private void Com_Part_EditValueChanged(object sender, EventArgs e)
        {
            string S_ID = Com_Part.EditValue.ToString();
            public_.AddPOAll(Com_PO, S_ID, "1", Grid_PO);
        }

        private void Btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
