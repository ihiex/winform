using App.MyMES.PartSelectService;
using App.MyMES.SimensService;
using DevExpress.XtraEditors;
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
    public partial class WH_Edit_Detail_Old_Form : DevExpress.XtraEditors.XtraForm
    {
        public WH_Edit_Detail_Old_Form()
        {
            InitializeComponent();
        }

        public delegate void De_UpdateMain();
        De_UpdateMain F_De_UpdateMain;
        string F_Type, F_FInterID, F_FDetailID;
        //PartSelectSVCClient F_PartSelectSVC;
        SiemensSVCClient F_SiemensSVC;
        string F_StationTypeID;

        public void Show_WH_Edit_Detail(WH_Edit_Detail_Old_Form F_WH_Edit_Detail, string S_Type, string S_FInterID, string S_FDetailID,
            string S_StationTypeID, SiemensSVCClient SiemensSVC, De_UpdateMain v_UpdateMain)
        {
            F_Type = S_Type;
            F_FInterID = S_FInterID;
            F_FDetailID = S_FDetailID;
            F_SiemensSVC = SiemensSVC;
            F_StationTypeID = S_StationTypeID;
            F_De_UpdateMain = v_UpdateMain;
            F_WH_Edit_Detail.ShowDialog();
        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            string S_FEntryID = Edt_NO.Text.Trim();
            string S_FCTN = Edt_CTN.Text.Trim();
            string S_FKPONO = Edt_PO.Text.Trim();
            string S_FMPNNO = Edt_MPN.Text.Trim();

            if (S_FEntryID == "")
            {
                MessageBox.Show("NO. Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_NO.Focus();
                return;
            }

            if (S_FCTN == "")
            {
                MessageBox.Show("FCTN Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_CTN.Focus();
                return;
            }
            if (S_FKPONO == "")
            {
                MessageBox.Show("PO Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_PO.Focus();
                return;
            }

            if (S_FMPNNO == "")
            {
                MessageBox.Show("MPN Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_MPN.Focus();
                return;
            }
 
            string S_Result = "";
            try
            {
                  S_Result = F_SiemensSVC.Edit_CO_WH_ShipmentEntry
                    (
                        F_FDetailID,
                        S_FEntryID,
                        F_FInterID,

                        S_FKPONO,
                        S_FMPNNO,
                        S_FCTN,                      
                        "0",
                        F_Type,

                        F_StationTypeID
                    );
            }
            catch(Exception ex)
            {
                S_Result = ex.ToString();
            }

            if (S_Result == "1")
            {
                MessageBox.Show("Save succeed!", "MSG", MessageBoxButtons.OK, MessageBoxIcon.Information);
                F_De_UpdateMain();
            }
            else
            {
                MessageBox.Show("Save Fail!  " + S_Result, "MSG", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (F_Type == "Mod")
            {
                this.Close();
            }
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WH_Edit_Detail_Form_Shown(object sender, EventArgs e)
        {
            if (F_Type == "Mod")
            {
                DataTable DT = F_SiemensSVC.GetShipmentEntry_One(F_FDetailID,F_StationTypeID).Tables[0];
                Edt_NO.Text = DT.Rows[0]["FEntryID"].ToString();  
                Edt_PO.Text= DT.Rows[0]["FKPONO"].ToString();
                Edt_MPN.Text = DT.Rows[0]["FMPNNO"].ToString();
                Edt_CTN.Text = DT.Rows[0]["FCTN"].ToString();

                Lab_Title.Text = "Modify";
            }
            else
            {
                Lab_Title.Text = "New";

                foreach (object obj in this.Controls)
                {
                    if (obj is TextEdit)
                    {
                        (obj as TextEdit).Text = "";
                    }
                }
            }

            Lab_Title.Location = new Point(Convert.ToInt32((Panel_Title.Width - Lab_Title.Width) / 2), 8);
        }

        

    }
}
