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
    public partial class WH_Edit_Old_Form : DevExpress.XtraEditors.XtraForm
    {
        public WH_Edit_Old_Form()
        {
            InitializeComponent();
        }

        public delegate void De_UpdateMain();
        De_UpdateMain F_De_UpdateMain;
        string F_Type, F_FInterID;
        //PartSelectSVCClient F_PartSelectSVC;
        SiemensSVCClient F_SiemensSVC;
        string F_StationTypeID;

        public void Show_WH_Edit_Form(WH_Edit_Old_Form F_WH_Edit_Form,string S_Type,string S_FInterID,
            string S_StationTypeID,SiemensSVCClient SiemensSVC, De_UpdateMain v_UpdateMain)
        {
            F_Type = S_Type;
            F_FInterID = S_FInterID;
            F_SiemensSVC = SiemensSVC;
            F_De_UpdateMain = v_UpdateMain;
            F_StationTypeID = S_StationTypeID;

            if (F_Type == "Att")
            {
                Btn_OK.Enabled = false;
            }
            else
            {
                Btn_OK.Enabled = true;
            }

            F_WH_Edit_Form.ShowDialog();
        }

        private void WH_Edit_Form_Load(object sender, EventArgs e)
        {
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WH_Edit_Form_Shown(object sender, EventArgs e)
        {
            if (F_Type == "Mod" || F_Type=="Att")
            {
                DataTable DT = F_SiemensSVC.GetShipment_One(F_FInterID, F_StationTypeID).Tables[0];

                Edt_HAWB.Text = DT.Rows[0]["HAWB"].ToString();
                Edt_PalletCount.Text = DT.Rows[0]["FPalletCount"].ToString();
                Edt_GrossWeight.Text = DT.Rows[0]["FGrossWeight"].ToString();
                Edt_Project.Text = DT.Rows[0]["FProjectNO"].ToString();

                Date_ShipDate.Text =Convert.ToDateTime(DT.Rows[0]["FDate"].ToString()).ToString("yyyy-MM-dd");
                Edt_PalletSeq.Text = DT.Rows[0]["FPalletSeq"].ToString();
                Edt_EmptyCarton.Text = DT.Rows[0]["FEmptyCarton"].ToString();
                Edt_ShipNO.Text = DT.Rows[0]["FShipNO"].ToString();

                Lab_Title.Text = DT.Rows[0]["FBillNO"].ToString();

                Edt_HAWB.Enabled = false;
                Edt_PalletCount.Enabled = false;
                Edt_PalletSeq.Enabled = false;
                Edt_ShipNO.Enabled = false;
            }
            else
            {
                Edt_HAWB.Enabled = true;
                Edt_PalletCount.Enabled = true;
                Edt_PalletSeq.Enabled = true;
                Edt_ShipNO.Enabled = true;

                Lab_Title.Text = "New";

                foreach (object obj in Group_Shipment.Controls)
                {
                    if (obj is TextEdit)
                    {
                        (obj as TextEdit).Text = "";
                    }
                }
                Date_ShipDate.Text = ""; 
            }

            Lab_Title.Location=new Point(Convert.ToInt32((Panel_Title.Width - Lab_Title.Width) / 2),8);      
        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            string S_HAWB                       = Edt_HAWB.Text.Trim();
            string S_PalletCount                = Edt_PalletCount.Text.Trim();
            string S_GrossWeight                = Edt_GrossWeight.Text.Trim();
            string S_Project                    = Edt_Project.Text.Trim();

            string S_ShipDate                   = Date_ShipDate.DateTime.ToString("yyyy-MM-dd HH:mm");
            string S_PalletSeq                  = Edt_PalletSeq.Text.Trim();
            string S_EmptyCarton                = Edt_EmptyCarton.Text.Trim();
            string S_ShipNO                     = Edt_ShipNO.Text.Trim();

            if (S_HAWB == "")
            {
                MessageBox.Show("HAWB Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_HAWB.Focus();
                return;
            }
            if (S_PalletCount == "")
            {
                MessageBox.Show("Pallet Count Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_PalletCount.Focus(); 
                return;
            }
            if (S_GrossWeight == "")
            {
                MessageBox.Show("GrossWeight  Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_GrossWeight.Focus();
                return;
            }
            if (S_Project == "")
            {
                MessageBox.Show("Project  Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_Project.Focus();
                return;
            }
            if (S_ShipDate == "")
            {
                MessageBox.Show("ShipDate  Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Date_ShipDate.Focus();
                return;
            }
            if (S_PalletSeq == "")
            {
                MessageBox.Show("PalletSeq  Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_PalletSeq.Focus();
                return;
            }
            if (S_EmptyCarton == "")
            {
                MessageBox.Show("EmptyCarton  Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_EmptyCarton.Focus();
                return;
            }
            
            string S_Result = F_SiemensSVC.Edit_CO_WH_Shipment
                (
                      F_FInterID,
                      S_ShipDate,
                      S_HAWB,
                      S_PalletSeq,
                      S_PalletCount,

                      S_GrossWeight,
                      S_EmptyCarton,
                      S_ShipNO,
                      S_Project,
                                                                                                            
                      F_Type,
                      F_StationTypeID
                );

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
    }
}
