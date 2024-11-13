using App.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.MyMES.PartSelectService;
using App.MyMES.mesPackageService;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace App.MyMES
{
    public partial class ShipMentRemove_Form : DevExpress.XtraEditors.XtraForm
    {
        public Public_ public_ = new Public_();
        public LoginList List_Login = new LoginList();
        public PartSelectSVCClient PartSelectSVC;
        public ImesPackageSVCClient mesPackageSVC;

        DataTable dtMupltiPack;
        int number = 1;

        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        public ShipMentRemove_Form()
        {
            InitializeComponent();
        }

        /// <summary>
        /// BillNo扫描
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBillNo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                string BillNo = txtBillNo.Text.Trim();
                if (string.IsNullOrEmpty(BillNo))
                {
                    // 条码不能为空.
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                    grdControlMultipack.DataSource = null;
                    txtBillNo.Focus();
                    return;
                }

                string Result = "1";
                string xmlProdOrder = ("<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>");
                string xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                string xmlExtraData = "<ExtraData EmployeeId=\"" + List_Login.EmployeeID.ToString() + "\"> </ExtraData>";
                string xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";

                DataSet dsDetail = PartSelectSVC.uspCallProcedure("uspGetShipMentDetail", BillNo,
                                                null, null, null, null, null, ref Result);
                if (dsDetail != null && dsDetail.Tables.Count > 0 && dsDetail.Tables[0].Rows.Count > 0)
                {
                    dtMupltiPack = dsDetail.Tables[0];
                    trListMultipack.DataSource = dtMupltiPack;
                    number = dtMupltiPack.Rows.Count + 1;

                    txtBillNo.Enabled = false;
                    txtMultipackSN.Enabled = true;
                    txtMultipackSN.Text = string.Empty;
                    txtMultipackSN.Focus();
                }
                else
                {
                    //未查询到出货关联数据,请确认.
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20197", "NG", List_Login.Language);
                    grdControlMultipack.DataSource = null;
                    txtBillNo.Text = string.Empty;
                    txtBillNo.Focus();
                    return;
                }
            }
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

                this.Com_PartFamily.Enabled = false;
                this.Com_PartFamilyType.Enabled = false;
                this.Com_Part.Enabled = false;
                this.Com_PO.Enabled = false;
                this.Btn_ConfirmPO.Enabled = false;
                this.Btn_Refresh.Enabled = true;
                this.panelCtrMain.Enabled = true;
            }
            catch(Exception ex)
            {
                //未查询到出货关联数据,请确认.
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        /// <summary>
        /// txtMultipackSN扫描
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMultipackSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string Result = string.Empty;
                string MultipackSN = txtMultipackSN.Text.Trim().ToUpper();
                if (string.IsNullOrEmpty(MultipackSN))
                {
                    //条码不能为空.
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                    txtMultipackSN.Focus();
                    return;
                }
                if(string.IsNullOrEmpty(txtBillNo.Text.Trim()))
                {
                    //条码不能为空.
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                    txtBillNo.Enabled = true;
                    txtMultipackSN.Enabled = false;
                    txtMultipackSN.Text = string.Empty;
                    txtBillNo.Focus();
                    return;
                }

                string xmlProdOrder = ("<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>");
                string xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                string xmlExtraData = "<ExtraData EmployeeId=\"" + List_Login.EmployeeID.ToString() + "\"> </ExtraData>";
                string xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";

                //包装移除
                PartSelectSVC.uspCallProcedure("uspMoveShipmentMultipack", MultipackSN, xmlProdOrder, xmlPart,
                                                            xmlStation, xmlExtraData, txtBillNo.Text.Trim(), ref Result);

                if (Result != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                    //条码:{0} 错误信息:{1}.
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { MultipackSN, ProMsg }, Result);
                    txtMultipackSN.SelectAll();
                    return;
                }

                number = dtMupltiPack.Rows.Count - 1;
                DataSet dsDetail = PartSelectSVC.uspCallProcedure("uspGetShipMentDetail", txtBillNo.Text.Trim(),
                                                null, null, null, null, null, ref Result);
                if (dsDetail != null && dsDetail.Tables.Count > 0 && dsDetail.Tables[0].Rows.Count > 0)
                {
                    dtMupltiPack = dsDetail.Tables[0];                    
                    trListMultipack.DataSource = dtMupltiPack;

                    MessageInfo.Add_Info_MSG(Edt_MSG, "10033", "OK", List_Login.Language, new string[] { MultipackSN });
                }



                if (number==0)
                {
                    txtBillNo.Text = string.Empty;
                    txtMultipackSN.Text = string.Empty;
                    txtBillNo.Enabled = true;
                    txtMultipackSN.Enabled = false;
                    grdControlMultipack.DataSource = null;
                    trListMultipack.DataSource = null;
                    dtMupltiPack.Rows.Clear();
                    txtBillNo.Focus();
                    number = 1;
                }
                else
                {
                    txtMultipackSN.Text = string.Empty;
                    txtMultipackSN.Focus();
                }
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            txtBillNo.Text = string.Empty;
            txtBillNo.Enabled = true;
            txtMultipackSN.Text = string.Empty;
            txtMultipackSN.Enabled = false;
            grdControlMultipack.DataSource = null;
            trListMultipack.DataSource = null;
            number = 1;
            if (dtMupltiPack != null && dtMupltiPack.Rows.Count > 0)
            {
                dtMupltiPack.Rows.Clear();
            }

        }

        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            List_Login = this.Tag as LoginList;

            PartSelectSVC = PartSelectFactory.CreateServerClient();
            mesPackageSVC = ImesPackageFactory.CreateServerClient();

            if (dtMupltiPack == null)
            {
                dtMupltiPack = new DataTable();
                dtMupltiPack.Columns.Add("ID", typeof(int));
                dtMupltiPack.Columns.Add("MultiPackSN", typeof(string));
            }

            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_LineID = List_Login.LineID.ToString();
            public_.AddPartFamilyType(Com_PartFamilyType, Grid_PartFamilyType);
            panelCtrMain.Enabled = false;
            txtBillNo.Enabled = true;
            txtBillNo.Text = string.Empty;
            txtMultipackSN.Text = string.Empty;
            txtMultipackSN.Enabled = false;
            this.Com_PartFamily.Enabled = true;
            this.Com_PartFamilyType.Enabled = true;
            this.Com_Part.Enabled = true;
            this.Com_PO.Enabled = true;
            this.Btn_ConfirmPO.Enabled = true;
            this.Btn_Refresh.Enabled = false;
            //this.btnRePrint.Enabled = false;
            btnReset_Click(null, null);

            string S_BarPrint = "";
            string S_WindowsVer = public_.GetWinVer().Substring(0, 2);

            if (S_WindowsVer == "64")
            {
                S_BarPrint = "BartenderPrint.exe";
            }
            else if (S_WindowsVer == "32")
            {
                S_BarPrint = "BartenderPrint_X86.exe";
            }

            Process[] arrayProcess = Process.GetProcessesByName(S_BarPrint.Replace(".exe", ""));
            if (arrayProcess.Length == 0)
            {
                Process p = Process.Start(S_BarPrint);
                Thread.Sleep(500);
            }
            else
            {
                foreach (Process pp in arrayProcess)
                {
                    IntPtr handle = pp.MainWindowHandle;
                    SwitchToThisWindow(handle, true);
                }
            }
        }

        private void Com_PartFamilyType_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartFamilyTypeID = Com_PartFamilyType.EditValue.ToString();
                public_.AddPartFamily(Com_PartFamily, S_PartFamilyTypeID, Grid_PartFamily);
            }
            catch (Exception ex)
            {
                //错误信息:{0}.
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void Com_PartFamily_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                public_.AddPart(Com_Part, S_PartFamilyID, Grid_Part);
            }
            catch (Exception ex)
            {
                //错误信息:{0}.
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void Com_Part_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartID = Com_Part.EditValue.ToString();
                public_.AddPOAll(Com_PO, S_PartID, List_Login.LineID.ToString(), Grid_PO);
            }
            catch (Exception ex)
            {
                //错误信息:{0}.
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            try
            {
                if (Com_PO.Text.Trim() == "")
                {
                    //工单不能为空,请确认.
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20114", "NG", List_Login.Language);
                    return;
                }
                else
                {
                    if (Com_PartFamilyType.EditValue == null || string.IsNullOrEmpty(Com_PartFamilyType.Text.ToString()))
                    {
                        //未选择料号类别,请确认.
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20115", "NG", List_Login.Language);
                        return;
                    }
                    if (Com_PartFamily.EditValue == null || string.IsNullOrEmpty(Com_PartFamily.Text.ToString()))
                    {
                        //未选择料号群,请确认.
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20116", "NG", List_Login.Language);
                        return;
                    }
                    if (Com_Part.EditValue == null || string.IsNullOrEmpty(Com_Part.Text.ToString()))
                    {
                        //未选择料号,请确认.
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20117", "NG", List_Login.Language);
                        return;
                    }
                    if (Com_PO.EditValue == null || string.IsNullOrEmpty(Com_PO.Text.ToString()))
                    {
                        //未选择工单，请确认.
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20118", "NG", List_Login.Language);
                        return;
                    }

                    this.Com_PartFamily.Enabled = false;
                    this.Com_PartFamilyType.Enabled = false;
                    this.Com_Part.Enabled = false;
                    this.Com_PO.Enabled = false;
                    this.Btn_ConfirmPO.Enabled = false;
                    this.Btn_Refresh.Enabled = true;
                    this.panelCtrMain.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                //错误信息:{0}.
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void ShipMent_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            PartSelectSVC.Close();
            mesPackageSVC.Close();
        }



        private void Edt_BillNO_Pallet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string BillNo = Edt_BillNO_Pallet.Text.Trim();
                if (string.IsNullOrEmpty(BillNo))
                {
                    // 条码不能为空.
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                    grdControlMultipack.DataSource = null;
                    txtBillNo.Focus();
                    return;
                }

                string Result = "1";
                string xmlProdOrder = ("<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>");
                string xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                string xmlExtraData = "<ExtraData EmployeeId=\"" + List_Login.EmployeeID.ToString() + "\"> </ExtraData>";
                string xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";

                DataSet dsDetail = PartSelectSVC.uspCallProcedure("uspGetShipMentDetail", BillNo,
                                                null, null, null, null, null, ref Result);
                if (dsDetail != null && dsDetail.Tables.Count > 0 && dsDetail.Tables[0].Rows.Count > 0)
                {
                    dtMupltiPack = dsDetail.Tables[0];
                    trListMultipack.DataSource = dtMupltiPack;
                    number = dtMupltiPack.Rows.Count + 1;

                    Edt_BillNO_Pallet.Enabled = false;
                    Edt_ShipmentPalletSN.Enabled = true;
                    Edt_ShipmentPalletSN.Text = string.Empty;
                    Edt_ShipmentPalletSN.Focus();
                }
                else
                {
                    //未查询到出货关联数据,请确认.
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20197", "NG", List_Login.Language);
                    grdControlMultipack.DataSource = null;
                    Edt_BillNO_Pallet.Text = string.Empty;
                    Edt_BillNO_Pallet.Focus();
                    return;
                }
            }
        }

        private void Edt_ShipmentPalletSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string Result = string.Empty;
                string S_ShipmentPalletSN = Edt_ShipmentPalletSN.Text.Trim().ToUpper();

                if (MessageBox.Show("Are you sure want to unpack this shipment pallet?"+"\r\n"+ S_ShipmentPalletSN,
                    "Unpack Shipment Pallet", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                {
                    Edt_ShipmentPalletSN.Text = "";
                    Edt_ShipmentPalletSN.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(S_ShipmentPalletSN))
                {
                    //条码不能为空.
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                    txtMultipackSN.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(Edt_BillNO_Pallet.Text.Trim()))
                {
                    //条码不能为空.
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                    Edt_BillNO_Pallet.Enabled = true;
                    Edt_ShipmentPalletSN.Enabled = false;
                    Edt_ShipmentPalletSN.Text = string.Empty;
                    Edt_BillNO_Pallet.Focus();
                    return;
                }

                string xmlProdOrder = ("<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>");
                string xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                string xmlExtraData = "<ExtraData EmployeeId=\"" + List_Login.EmployeeID.ToString() + "\"> </ExtraData>";
                string xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";

                //包装移除
                PartSelectSVC.uspCallProcedure("uspUnpackShipmentPallet", S_ShipmentPalletSN, xmlProdOrder, xmlPart,
                                                            xmlStation, xmlExtraData, Edt_BillNO_Pallet.Text.Trim(), ref Result);

                if (Result != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                    //条码:{0} 错误信息:{1}.
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_ShipmentPalletSN, ProMsg }, Result);
                    Edt_ShipmentPalletSN.SelectAll();
                    return;
                }

                number = dtMupltiPack.Rows.Count - 1;
                DataSet dsDetail = PartSelectSVC.uspCallProcedure("uspGetShipMentDetail", Edt_BillNO_Pallet.Text.Trim(),
                                                null, null, null, null, null, ref Result);
                if (dsDetail != null && dsDetail.Tables.Count > 0 && dsDetail.Tables[0].Rows.Count > 0)
                {
                    dtMupltiPack = dsDetail.Tables[0];
                    trListMultipack.DataSource = dtMupltiPack;                    
                }
                else
                {
                    dtMupltiPack = null;
                    trListMultipack.DataSource = dtMupltiPack;

                    Edt_BillNO_Pallet.Text = string.Empty;
                    Edt_ShipmentPalletSN.Text = string.Empty;
                    Edt_BillNO_Pallet.Enabled = true;
                    Edt_ShipmentPalletSN.Enabled = false;

                    MessageInfo.Add_Info_MSG(Edt_MSG, "10033", "OK", List_Login.Language, new string[] { S_ShipmentPalletSN });
                }


                if (number == 0)
                {
                    Edt_BillNO_Pallet.Text = string.Empty;
                    Edt_ShipmentPalletSN.Text = string.Empty;
                    Edt_BillNO_Pallet.Enabled = true;
                    Edt_ShipmentPalletSN.Enabled = false;
                    grdControlMultipack.DataSource = null;
                    trListMultipack.DataSource = null;
                    dtMupltiPack.Rows.Clear();
                    Edt_BillNO_Pallet.Focus();
                    number = 1;
                }
                else
                {
                    Edt_ShipmentPalletSN.Text = string.Empty;
                    Edt_ShipmentPalletSN.Focus();
                }
            }
        }

        private void Btn_Reset_Pallet_Click(object sender, EventArgs e)
        {
            Edt_BillNO_Pallet.Text = string.Empty;
            Edt_BillNO_Pallet.Enabled = true;
            Edt_ShipmentPalletSN.Text = string.Empty;
            Edt_ShipmentPalletSN.Enabled = false;
            grdControlMultipack.DataSource = null;
            trListMultipack.DataSource = null;
            number = 1;
            if (dtMupltiPack != null && dtMupltiPack.Rows.Count > 0)
            {
                dtMupltiPack.Rows.Clear();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {            
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    trListMultipack.ExportToXlsx(saveFileDialog1.FileName);
                }
                catch
                {
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int ii = textBox1.Lines.Count();

                for (int i = 0; i < ii; i++)
                {
                    string S_Sn = textBox1.Lines[i].Trim();

                    txtMultipackSN.Text = S_Sn;
                    txtMultipackSN_KeyDown(sender, e);
                }
            }
        }
    }
}
