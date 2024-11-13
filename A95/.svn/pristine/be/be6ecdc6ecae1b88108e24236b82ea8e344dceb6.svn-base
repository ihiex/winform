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

namespace App.MyMES
{
    public partial class DefectForm : DevExpress.XtraEditors.XtraForm
    {
        public DefectForm()
        {
            InitializeComponent();
        }

        TextBox F_Edt_DefectID;
        int F_DefectTypeID;
        public void Show_DefectForm(DefectForm v_Form, TextBox Edt_DefectID)
        {
            F_Edt_DefectID = Edt_DefectID;
            v_Form.ShowDialog();
        }

        public DefectForm Show_DefectFormToPanel(DefectForm v_Form, TextBox Edt_DefectID,int DefectTypeID)
        {
            F_DefectTypeID = DefectTypeID;
            F_Edt_DefectID = Edt_DefectID;
            return v_Form;
        }


        private void DefectForm_Load(object sender, EventArgs e)
        {
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            DataTable DT = PartSelectSVC.GetluDefect(F_DefectTypeID).Tables[0];            
            DT.Columns.Add("Check", typeof(bool));

            Grid_Main.DataSource = DT;

            string[] Array_Defect = F_Edt_DefectID.Text.Split(';');
            foreach (var item in Array_Defect)
            {
                for (int i = 0; i < GridView_Defact.DataRowCount ; i++)
                {
                    DataRow DR = GridView_Defact.GetDataRow(i);
                    string S_ID = DR["ID"].ToString(); 

                    if (S_ID == item)
                    {
                        GridView_Defact.SetRowCellValue(i, "Check", true);
                    }
                }
            }

        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            string S_ID = "";
                        
            for (int i = 0; i < GridView_Defact.DataRowCount; i++)
            {
                DataRow DR = GridView_Defact.GetDataRow(i);
                string S_Check = DR["Check"].ToString(); 
                if (S_Check != "")
                {
                    DR = GridView_Defact.GetDataRow(i);
                    S_Check = DR["Check"].ToString();
                    string v_ID= DR["ID"].ToString();

                    Boolean B_Check = Convert.ToBoolean(S_Check);
                    if (B_Check == true)
                    {
                        if (i == GridView_Defact.DataRowCount - 1)
                        {
                            S_ID += v_ID;
                        }
                        else
                        {
                            S_ID += v_ID + ";";
                        }
                    }
                }
            }

            if (S_ID.Trim() != "")
            {
                F_Edt_DefectID.Text = S_ID;

                Grid_Main.Enabled = false;
                Btn_OK.Enabled = false;

                this.Close(); 
            }
            else
            {
                MessageBox.Show("请至少勾选一个不良代码！", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Grid_Main_Click(object sender, EventArgs e)
        {

        }

        private void GridView_Defact_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            
        }

        private void GridView_Defact_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Btn_OK.Enabled = true;
        }
    }
}
