using App.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.MyMES.PartSelectService;
using App.MyMES.mesPackageService;
using App.MyMES.SimensService;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;

namespace App.MyMES
{
    public partial class WH_ShipmentOut_Old_Form : DevExpress.XtraEditors.XtraForm
    {
        public Public_ public_ = new Public_();
        SiemensSVCClient SiemensSVC;
        LoginList List_Login = new LoginList();

        public PartSelectSVCClient PartSelectSVC;
        private bool IsScan2DSN = false;
        private string MultiSN_2D = string.Empty;
        private string SN_Pattern = string.Empty;
        public WH_ShipmentOut_Old_Form()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShipMent_Form_Load(object sender, EventArgs e)
        {
            try
            {
                Btn_Refresh_Click(null, null);
                SiemensSVC = ISiemensFactory.CreateServerClient();
                PartSelectSVC = PartSelectFactory.CreateServerClient();
                List_Login = this.Tag as LoginList;

                var repositoryItemImageComboBoxStatus = new RepositoryItemImageComboBox();
                repositoryItemImageComboBoxStatus.Items.AddRange(new[]
                {
                    new ImageComboBoxItem("",0,0),
                    new ImageComboBoxItem("",1,1)
                });

                repositoryItemImageComboBoxStatus.Name = "repositoryItemImageComboBoxStatus";
                repositoryItemImageComboBoxStatus.SmallImages = imageList1;
                gridColumn3.ColumnEdit = repositoryItemImageComboBoxStatus;
                gridColumn7.ColumnEdit = repositoryItemImageComboBoxStatus;
            }
            catch(Exception ex)
            {
                //未查询到出货关联数据,请确认.
               // MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }


        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            //Edt_MPN.Text = "";
            txtBillNo.Text = "";
            txtMultipackSN.Text = "";

            txtBillNo.Enabled = true;
            txtMultipackSN.Enabled = false;
            Grid_Main.DataSource = null;
            Grid_Detail.DataSource = null;

            txtMultipack2DSN.Text = string.Empty;

            if (IsScan2DSN && !Check_NotOut.Checked)
            {
                lbl_Mult2D.Show();
                txtMultipack2DSN.Show();
                txtMultipack2DSN.Enabled = false;
            }
            else
            {
                lbl_Mult2D.Hide();
                txtMultipack2DSN.Hide();
            }
        }


        private void ShipMent_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            PartSelectSVC.Close();
            SiemensSVC.Close();             
        }

         
        private void Edt_BillNo_KeyDown(object sender, KeyEventArgs e)
        {            
            if (e.KeyCode == Keys.Enter)
            {                
                string S_BillNo = txtBillNo.Text.Trim();
                
                if (S_BillNo == "")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                    txtBillNo.Focus();
                    txtBillNo.SelectAll(); 
                    return;
                }
                    
                string S_CheckBillNo = "1";
                DataSet DS= SiemensSVC.CheckBillNo(S_BillNo, List_Login.StationTypeID.ToString(),out S_CheckBillNo);
                if (S_CheckBillNo != "1")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, S_CheckBillNo, "NG", List_Login.Language);
                    txtBillNo.Focus();
                    txtBillNo.SelectAll();
                    return;
                }

                Grid_Detail.DataSource = DS.Tables[0]; 
                txtBillNo.Enabled = false;
                if (IsScan2DSN && !Check_NotOut.Checked)
                {
                    txtMultipack2DSN.Enabled = true;
                    txtMultipack2DSN.Focus();
                }
                else
                {
                    txtMultipackSN.Enabled = true;
                    txtMultipackSN.Focus();
                }

            }
        }

        private void Edt_MultipackSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DateTime dateStart = DateTime.Now;
                string S_BillNo = txtBillNo.Text.Trim();
                string S_MultipackSN = txtMultipackSN.Text.Trim();

                string S_Type = "0";
                if (Check_NotOut.Checked == true) { S_Type = "1"; }

                if (!Check_NotOut.Checked && IsScan2DSN)
                {
                    if (S_MultipackSN != MultiSN_2D)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "NG", "Error: 2D barcode and multi-pack SN mismatch.");
                        txtMultipackSN.Enabled = true;
                        txtMultipackSN.Text = string.Empty;
                        txtMultipackSN.Focus();
                        return;
                    }
                }



                string S_WHout = SiemensSVC.WHOut("", S_BillNo, S_MultipackSN, S_Type, List_Login.StationTypeID.ToString());
                if (S_WHout == "OK")
                {
                    txtMultipackSN.Text = "";
                    if (IsScan2DSN && !Check_NotOut.Checked)
                    {
                        txtMultipackSN.Enabled = false;
                        txtMultipack2DSN.Enabled = true;
                        txtMultipack2DSN.Text = "";
                        txtMultipack2DSN.Focus();

                    }
                    else
                    {
                        txtMultipackSN.Focus();
                    }



                    TimeSpan ts = DateTime.Now - dateStart;
                    double mill = Math.Round(ts.TotalMilliseconds, 0);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { S_MultipackSN, mill.ToString() });

                   // txtOverStationQTY.Text = (Convert.ToInt32(txtOverStationQTY.Text) + 1).ToString();

                    string S_CheckBillNo = "";
                    DataSet DS_MPN = SiemensSVC.CheckBillNo(S_BillNo, List_Login.StationTypeID.ToString(), out S_CheckBillNo);
                    Grid_Detail.DataSource = DS_MPN.Tables[0];
                }
                else
                {
                    txtMultipackSN.Focus();
                    txtMultipackSN.SelectAll();
                    string ProMsg = MessageInfo.GetMsgByCode(S_WHout, List_Login.Language);
                    string S_GetMsgByFormName = MessageInfo.GetMsgByFormName(S_WHout, List_Login.Language);

                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_MultipackSN, ProMsg, S_GetMsgByFormName }, S_WHout);

                }

                DataSet DS = SiemensSVC.WHOut_DT(List_Login.StationTypeID.ToString(), S_MultipackSN);
                if (DS.Tables.Count == 3)
                {
                    Grid_Main.DataSource = DS.Tables[0];
                    string S_LCount = DS.Tables[1].Rows[0]["LCount"].ToString();
                    string S_ShippingQty = DS.Tables[2].Rows[0]["LCountS"].ToString();

                    Lab_TotalBox.Text = "Total Box:" + S_LCount;
                    Lab_ShippingQty.Text = "ShippingQty:" + S_ShippingQty;
                    Lab_Remain.Text = "Remain:" + (Convert.ToInt32(S_LCount) - Convert.ToInt32(S_ShippingQty)).ToString();
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //txtOverStationQTY.Text = "0";
        }

        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            List_Login = this.Tag as LoginList;

            PartSelectSVC = PartSelectFactory.CreateServerClient();

            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_LineID = List_Login.LineID.ToString();
            //panelCtrMain.Enabled = false;
            txtBillNo.Enabled = true;
            txtBillNo.Text = string.Empty;
            txtMultipackSN.Text = string.Empty;
            txtMultipackSN.Enabled = false;

            DataSet DS_StationTypeDef = PartSelectSVC.MESGetStationTypeParameter(List_Login.StationTypeID);
            DataRow[] drIsScan2D = DS_StationTypeDef.Tables[0].Select("Description='IsScan2DSN'");
            if (drIsScan2D.Length > 0)
            {
                string tmpIsScan2DSN = drIsScan2D[0]["Content"].ToString();

                IsScan2DSN = string.IsNullOrEmpty(tmpIsScan2DSN) || tmpIsScan2DSN == "1";
            }
            else
            {
                IsScan2DSN = true;
            }
            txtMultipack2DSN.Text = "";
            txtMultipack2DSN.Enabled = false;
            if (IsScan2DSN)
            {
                DataRow[] drDataRows = DS_StationTypeDef.Tables[0].Select("Description='Pattern_2DSN'");
                if (drDataRows != null && drDataRows.Length > 0 )
                {
                    SN_Pattern = drDataRows[0]["Content"].ToString();
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20199", "NG", List_Login.Language, new string[] { "Pattern_2DSN" });
                    return;
                }
                txtMultipack2DSN.Show();
                lbl_Mult2D.Show();
            }
            else
            {
                txtMultipack2DSN.Hide();
                lbl_Mult2D.Hide();
            }


            btnReset_Click(null, null);
        }

        
        private void txtMultipack2DSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            string mult2DSN = txtMultipack2DSN.Text.Trim();
            if (string.IsNullOrEmpty(mult2DSN))
            {
                // 条码不能为空.
                MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                txtMultipack2DSN.Focus();
                return;
            }

            if (string.IsNullOrEmpty(SN_Pattern))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20199", "NG", List_Login.Language, new string[] { "Pattern_2DSN" });
                txtMultipack2DSN.Text = string.Empty;
                txtMultipack2DSN.Focus();
                return;
            }

            if (!Regex.IsMatch(mult2DSN, SN_Pattern))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20027", "NG", List_Login.Language);
                txtMultipack2DSN.Text = string.Empty;
                txtMultipack2DSN.Focus();
                return;
            }


            string Result = "1";
            string xmlProdOrder = ("<ProdOrder ProdOrderID=\"0\"> </ProdOrder>");
            string xmlPart = "<Part PartID=\"0\"> </Part>";
            string xmlExtraData = "<ExtraData EmployeeId=\"" + List_Login.EmployeeID.ToString() + "\"> </ExtraData>";
            string xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";

            DataSet multBoxSN = PartSelectSVC.uspCallProcedure("uspShipping2DBoxCheck", mult2DSN,
                xmlProdOrder, xmlPart, xmlStation, xmlExtraData, txtBillNo.Text.Trim(), ref Result);
            if (Result != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { mult2DSN, ProMsg }, Result);
                txtMultipack2DSN.Text = string.Empty;
                txtMultipack2DSN.Focus();
                return;
            }

            if (multBoxSN != null && multBoxSN.Tables.Count == 1 && multBoxSN.Tables[0].Rows.Count == 1)
            {
                MultiSN_2D = multBoxSN.Tables[0].Rows[0][0].ToString();
            }

            txtMultipack2DSN.Enabled = false;
            txtMultipackSN.Enabled = true;
            txtMultipackSN.Text = string.Empty;
            txtMultipackSN.Focus();
        }

        private void Check_NotOut_CheckedChanged(object sender, EventArgs e)
        {
            if (!Check_NotOut.Checked && IsScan2DSN)
            {
                lbl_Mult2D.Show();
                txtMultipack2DSN.Show();
                if (!string.IsNullOrEmpty(txtBillNo.Text) && !txtBillNo.Enabled)
                {
                    txtMultipack2DSN.Enabled = true;
                    txtMultipackSN.Enabled = false;
                    txtMultipack2DSN.Focus();
                }
                else
                {
                    txtMultipack2DSN.Enabled = false;
                    txtBillNo.Focus();
                }
            }
            else
            {
                lbl_Mult2D.Hide();
                txtMultipack2DSN.Hide();
                txtMultipack2DSN.Enabled = false;
                if (!string.IsNullOrEmpty(txtBillNo.Text) && !txtBillNo.Enabled)
                {
                    txtMultipackSN.Text = "";
                    txtMultipackSN.Enabled = true;
                    txtMultipackSN.Focus();
                }
                else
                {
                    txtMultipackSN.Enabled = false;
                    txtBillNo.Focus();
                }
            }
        }
    }
}
