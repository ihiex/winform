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
    public partial class WH_Edit_Form : DevExpress.XtraEditors.XtraForm
    {
        public WH_Edit_Form()
        {
            InitializeComponent();
        }

        public delegate void De_UpdateMain();
        De_UpdateMain F_De_UpdateMain;
        string F_Type, F_FInterID;
        PartSelectSVCClient F_PartSelectSVC;
        public void Show_WH_Edit_Form(WH_Edit_Form F_WH_Edit_Form,string S_Type,string S_FInterID,
            PartSelectSVCClient PartSelectSVC, De_UpdateMain v_UpdateMain)
        {
            F_Type = S_Type;
            F_FInterID = S_FInterID;
            F_PartSelectSVC = PartSelectSVC;
            F_De_UpdateMain = v_UpdateMain;

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
                DataTable DT = F_PartSelectSVC.GetShipmentNew_One(F_FInterID).Tables[0];

                Edt_HAWB.Text = DT.Rows[0]["HAWB"].ToString();
                Edt_PalletCount.Text = DT.Rows[0]["FPalletCount"].ToString();
                Edt_GrossWeight.Text = DT.Rows[0]["FGrossWeight"].ToString();
                Edt_Project.Text = DT.Rows[0]["FProjectNO"].ToString();

                Date_ShipDate.Text =Convert.ToDateTime(DT.Rows[0]["FDate"].ToString()).ToString("yyyy-MM-dd");
                Edt_PalletSeq.Text = DT.Rows[0]["FPalletSeq"].ToString();
                Edt_EmptyCarton.Text = DT.Rows[0]["FEmptyCarton"].ToString();

                Edt_PalletSN.Text = DT.Rows[0]["PalletSN"].ToString();
                Edt_ShipNO.Text = DT.Rows[0]["FShipNO"].ToString();
                Edt_ShipID.Text = DT.Rows[0]["ShipID"].ToString();

                Edt_Regin.Text = DT.Rows[0]["Region"].ToString();
                Edt_ReferenceNO.Text = DT.Rows[0]["ReferenceNO"].ToString();
                Edt_Country.Text = DT.Rows[0]["Country"].ToString();
                Edt_Carrier.Text = DT.Rows[0]["Carrier"].ToString();
                Edt_HubCode.Text = DT.Rows[0]["HubCode"].ToString();
                Edt_TruckNo.Text = DT.Rows[0]["TruckNo"].ToString();

                Edt_ReturnAddress.Text = DT.Rows[0]["ReturnAddress"].ToString();
                Edt_DeliveryStreetAddress.Text = DT.Rows[0]["DeliveryStreetAddress"].ToString();
                Edt_DeliveryRegion.Text = DT.Rows[0]["DeliveryRegion"].ToString();
                Edt_DeliveryToName.Text = DT.Rows[0]["DeliveryToName"].ToString();
                Edt_DeliveryCityName.Text = DT.Rows[0]["DeliveryCityName"].ToString();
                Edt_DeliveryCountry.Text = DT.Rows[0]["DeliveryCountry"].ToString();
                Edt_AdditionalDeliveryToName.Text = DT.Rows[0]["AdditionalDeliveryToName"].ToString();
                Edt_DeliveryPostalCode.Text = DT.Rows[0]["DeliveryPostalCode"].ToString();
                Edt_TelNo.Text = DT.Rows[0]["TelNo"].ToString();

                Edt_OceanContainerNumber.Text = DT.Rows[0]["MAWB_OceanContainerNumber"].ToString();
                Edt_Origin.Text = DT.Rows[0]["Origin"].ToString();
                Edt_TotalVolume.Text = DT.Rows[0]["TotalVolume"].ToString();
                Edt_POE_COC.Text = DT.Rows[0]["POE_COC"].ToString();
                Edt_TransportMethod.Text = DT.Rows[0]["TransportMethod"].ToString();
                Edt_POE.Text = DT.Rows[0]["POE"].ToString();

                Lab_Title.Text = DT.Rows[0]["FBillNO"].ToString();

                Edt_HAWB.Enabled = false;
                Edt_PalletCount.Enabled = false;
                Edt_PalletSeq.Enabled = false;
                Edt_PalletSN.Enabled = false;
                Edt_ShipNO.Enabled = false;
                Edt_ShipID.Enabled = false;  
            }
            else
            {
                Edt_HAWB.Enabled = true;
                Edt_PalletCount.Enabled = true;
                Edt_PalletSeq.Enabled = true;
                Edt_PalletSN.Enabled = true;
                Edt_ShipNO.Enabled = true;
                Edt_ShipID.Enabled = true;

                Lab_Title.Text = "New";

                foreach (object obj in Group_Shipment.Controls)
                {
                    if (obj is TextEdit)
                    {
                        (obj as TextEdit).Text = "";
                    }
                }
                foreach (object obj in Group_Shipping.Controls)
                {
                    if (obj is TextEdit)
                    {
                        (obj as TextEdit).Text = "";
                    }
                }
                foreach (object obj in Group_Delievery.Controls)
                {
                    if (obj is TextEdit)
                    {
                        (obj as TextEdit).Text = "";
                    }
                }
                foreach (object obj in Group_Other.Controls)
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

            string S_PalletSN                   = Edt_PalletSN.Text.Trim();
            string S_ShipNO                     = Edt_ShipNO.Text.Trim();
            string S_ShipID                     = Edt_ShipID.Text.Trim();

            string S_Regin                      = Edt_Regin.Text.Trim();
            string S_ReferenceNO                = Edt_ReferenceNO.Text.Trim();
            string S_Country                    = Edt_Country.Text.Trim();
            string S_Carrier                    = Edt_Carrier.Text.Trim();
            string S_HubCode                    = Edt_HubCode.Text.Trim();
            string S_TruckNo                    = Edt_TruckNo.Text.Trim();

            string S_ReturnAddress              = Edt_ReturnAddress.Text.Trim();
            string S_DeliveryStreetAddress      = Edt_DeliveryStreetAddress.Text.Trim();
            string S_DeliveryRegion             = Edt_DeliveryRegion.Text.Trim();
            string S_DeliveryToName             = Edt_DeliveryToName.Text.Trim();
            string S_DeliveryCityName           = Edt_DeliveryCityName.Text.Trim();
            string S_DeliveryCountry            = Edt_DeliveryCountry.Text.Trim();
            string S_AdditionalDeliveryToName   = Edt_AdditionalDeliveryToName.Text.Trim();
            string S_DeliveryPostalCode         = Edt_DeliveryPostalCode.Text.Trim();
            string S_TelNo                      = Edt_TelNo.Text.Trim();

            string S_OceanContainerNumber       = Edt_OceanContainerNumber.Text.Trim();
            string S_Origin                     = Edt_Origin.Text.Trim();
            string S_TotalVolume                = Edt_TotalVolume.Text.Trim();
            string S_POE_COC                    = Edt_POE_COC.Text.Trim();
            string S_TransportMethod            = Edt_TransportMethod.Text.Trim();
            string S_POE                        = Edt_POE.Text.Trim();

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
            //if (S_PalletSN == "")
            //{
            //    MessageBox.Show("PalletSN  Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    Edt_PalletSN.Focus();
            //    return;
            //}
            if (S_ShipNO == "")
            {
                MessageBox.Show("ShipNO  Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_ShipNO.Focus();
                return;
            }
            if (S_ShipID == "")
            {
                MessageBox.Show("ShipID   Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_ShipID.Focus();
                return;
            }

            if (S_Regin == "")
            {
                MessageBox.Show("Regin    Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_Regin.Focus();
                return;
            }
            if (S_ReferenceNO == "")
            {
                MessageBox.Show("ReferenceNO Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_ReferenceNO.Focus();
                return;
            }
            if (S_Country == "")
            {
                MessageBox.Show("Country Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_Country.Focus();
                return;
            }
            if (S_Carrier == "")
            {
                MessageBox.Show("Carrier Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_Carrier.Focus();
                return;
            }
            if (S_HubCode == "")
            {
                MessageBox.Show("HubCode Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_HubCode.Focus();
                return;
            }
            if (S_TruckNo == "")
            {
                MessageBox.Show("TruckNo Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_TruckNo.Focus();
                return;
            }

            if (S_OceanContainerNumber == "")
            {
                MessageBox.Show("OceanContainerNumber Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_OceanContainerNumber.Focus();
                return;
            }
            if (S_Origin == "")
            {
                MessageBox.Show("Origin  Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_Origin.Focus();
                return;
            }
            if (S_TotalVolume == "")
            {
                MessageBox.Show("TotalVolume  Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_TotalVolume.Focus();
                return;
            }
            if (S_POE_COC == "")
            {
                MessageBox.Show("POE_COC  Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_POE_COC.Focus();
                return;
            }
            if (S_TransportMethod == "")
            {
                MessageBox.Show("TransportMethod  Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_TransportMethod.Focus();
                return;
            }
            if (S_POE == "")
            {
                MessageBox.Show("POE  Can't be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Edt_POE.Focus();
                return;
            }
            
            string S_Result = F_PartSelectSVC.Edit_CO_WH_ShipmentNew
                (
                      F_FInterID,
                      S_HAWB,
                      S_PalletCount,
                      S_GrossWeight,
                      S_Project,

                      S_ShipDate,
                      S_PalletSeq,
                      S_EmptyCarton,

                      S_PalletSN,
                      S_ShipNO,
                      S_ShipID,

                      S_Regin,
                      S_ReferenceNO,
                      S_Country,
                      S_Carrier,
                      S_HubCode,
                      S_TruckNo,
                      S_ReturnAddress,
                      S_DeliveryStreetAddress,
                      S_DeliveryRegion,
                      S_DeliveryToName,
                      S_DeliveryCityName,
                      S_DeliveryCountry,
                      S_AdditionalDeliveryToName,
                      S_DeliveryPostalCode,
                      S_TelNo,

                      S_OceanContainerNumber,
                      S_Origin,
                      S_TotalVolume,
                      S_POE_COC,
                      S_TransportMethod,
                      S_POE,

                      F_Type                      
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
