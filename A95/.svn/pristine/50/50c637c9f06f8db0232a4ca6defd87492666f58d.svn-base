using App.MyMES.PartSelectService;
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
    public partial class WH_Edit_Detail_Form : DevExpress.XtraEditors.XtraForm
    {
        public WH_Edit_Detail_Form()
        {
            InitializeComponent();
        }

        public delegate void De_UpdateMain();
        De_UpdateMain F_De_UpdateMain;
        string F_Type, F_FInterID, F_FDetailID;
        PartSelectSVCClient F_PartSelectSVC;

        public void Show_WH_Edit_Detail(WH_Edit_Detail_Form F_WH_Edit_Detail, string S_Type, string S_FInterID, string S_FDetailID,
            PartSelectSVCClient PartSelectSVC, De_UpdateMain v_UpdateMain)
        {
            F_Type = S_Type;
            F_FInterID = S_FInterID;
            F_FDetailID = S_FDetailID;
            F_PartSelectSVC = PartSelectSVC;
            F_De_UpdateMain = v_UpdateMain;
            F_WH_Edit_Detail.ShowDialog();
        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            string S_FEntryID = Edt_NO.Text.Trim();
            string S_FCarrierNo = Edt_CarrierNo.Text.Trim();
            string S_FCommercialInvoice = Edt_CommercialInvoice.Text.Trim();
            string S_FCrossWeight = Edt_GrossWeight.Text.Trim();
            string S_FCTN = Edt_CTN.Text.Trim();
            string S_FKPONO = Edt_PO.Text.Trim();
            string S_FLineItem = Edt_LineItem.Text.Trim();
            string S_FMPNNO = Edt_MPN.Text.Trim();
            string S_FNetWeight = Edt_NetWeight.Text.Trim();

            string S_FPartNumberDesc = Edt_MPNDesc.Text.Trim();
            string S_FQTY = Edt_QTY.Text.Trim();
            string S_FProjectNO = Edt_FProjectNO.Text.Trim();  

            if (S_FEntryID == "")
            {
                MessageBox.Show("NO. Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_NO.Focus();
                return;
            }

            if (S_FCarrierNo == "")
            {
                MessageBox.Show("CarrierNo Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_CarrierNo.Focus();
                return;
            }
            if (S_FCommercialInvoice == "")
            {
                MessageBox.Show("CommercialInvoice Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_CommercialInvoice.Focus();
                return;
            }

            if (S_FCrossWeight == "")
            {
                MessageBox.Show("FCrossWeight Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_GrossWeight.Focus();
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

            if (S_FLineItem == "")
            {
                MessageBox.Show("LineItem Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_LineItem.Focus();
                return;
            }
            if (S_FMPNNO == "")
            {
                MessageBox.Show("MPN Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_MPN.Focus();
                return;
            }
            if (S_FNetWeight == "")
            {
                MessageBox.Show("NetWeight Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Lab_NetWeight.Focus();
                return;
            }
            if (S_FPartNumberDesc == "")
            {
                MessageBox.Show("PartNumberDesc. Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_MPNDesc.Focus();
                return;
            }
            if (S_FQTY == "")
            {
                MessageBox.Show("QTY Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_QTY.Focus();
                return;
            }
            if (S_FProjectNO == "")
            {
                MessageBox.Show("ProjectNO Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_QTY.Focus();
                return;
            }



            string S_Result = "";
            try
            {
                  S_Result = F_PartSelectSVC.Edit_CO_WH_ShipmentEntryNew
                    (
                        F_FDetailID,
                        S_FEntryID,
                        F_FInterID,
                        S_FCarrierNo,
                        S_FCommercialInvoice,
                        S_FCrossWeight,
                        S_FCTN,
                        S_FKPONO,
                        S_FLineItem,
                        S_FMPNNO,
                        S_FNetWeight,
                        "",
                        S_FPartNumberDesc,
                        S_FQTY,
                        "0",
                        S_FProjectNO,
                        F_Type
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
                DataTable DT = F_PartSelectSVC.GetShipmentEntryNew_One(F_FDetailID).Tables[0];
                Edt_NO.Text = DT.Rows[0]["FEntryID"].ToString();  
                Edt_PO.Text= DT.Rows[0]["FKPONO"].ToString();
                Edt_LineItem.Text = DT.Rows[0]["FLineItem"].ToString();
                Edt_MPN.Text = DT.Rows[0]["FMPNNO"].ToString();
                Edt_MPNDesc.Text = DT.Rows[0]["FPartNumberDesc"].ToString();
                Edt_CTN.Text = DT.Rows[0]["FCTN"].ToString();
                Edt_QTY.Text = DT.Rows[0]["FQTY"].ToString();
                Edt_GrossWeight.Text = DT.Rows[0]["FCrossWeight"].ToString();
                Edt_NetWeight.Text = DT.Rows[0]["FNetWeight"].ToString();
                Edt_CarrierNo.Text = DT.Rows[0]["FCarrierNo"].ToString();
                Edt_CommercialInvoice.Text = DT.Rows[0]["FCommercialInvoice"].ToString();
                Edt_FProjectNO.Text=DT.Rows[0]["FProjectNO"].ToString();

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
