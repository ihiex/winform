using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using App.Model;
using System.Threading;
using App.MyMES.DataCommitService;
using DevExpress.Utils;

namespace App.MyMES
{
    public partial class PackageOverStation_Form : FrmBase
    {
        string xmlProdOrder = string.Empty;
        string xmlPart = string.Empty;
        string xmlExtraData = string.Empty;
        string xmlStation = string.Empty;

        bool COF = false;
        string IsAuto = "0";
        string IsNFC = "0";
        string IsBindAdhesive = "0";
        //string IsNoPOCheck = "0";
        //string IsNoPNCheck = "0";
        string S_IsCheckPO = "1";
        string S_IsCheckPN = "1";

        string S_ProductionOrderID = "";
        string S_PartID = "";
        string S_PartFamilyID = "";
        string S_PartFamilyType = "";
        DataSet dsBOM = null;
        DataSet DS_StationTypeDef;

        /// <summary>
        /// 1: Carton Box SN
        /// 2: Pallet SN
        /// </summary>
        int IsScanPalletSNOrCartonBoxSN = 1; 
        public PackageOverStation_Form()
        {
            InitializeComponent();
        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            Edt_BoxSN.Enabled = false;
            base.Btn_Defect.Visible = false;
        }

        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            base.Btn_ConfirmPO_Click(sender, e);
            GetStationData();
            Edt_BoxSN.Enabled = true;
            Edt_BoxSN.Focus();
            xmlProdOrder = ("<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>");
            xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
            xmlExtraData = "<ExtraData EmployeeId=\"" + List_Login.EmployeeID.ToString() + "\"> </ExtraData>";
            xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";
            string tmpVal = PartSelectSVC.GetMesStationAndStationTypeDetail(List_Login.StationID, "IsScanPalletSNOrCartonBoxSN", "IsScanPalletSNOrCartonBoxSN", out string result);
            if (result == "1")
            {
                if (!int.TryParse(tmpVal, out IsScanPalletSNOrCartonBoxSN))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "NG", "station type setting error, please check 'IsScanPalletSNOrCartonBoxSN' ");
                }
                if (IsScanPalletSNOrCartonBoxSN != 1 && IsScanPalletSNOrCartonBoxSN != 2)
                {
                    IsScanPalletSNOrCartonBoxSN = 1;
                }
            }

            checkedListBoxControl1.ReadOnly = true;
            checkedListBoxControl1.Items.Clear();
            checkedListBoxControl1.AllowHtmlDraw = DefaultBoolean.True;

            lbl_totalCount.Text = "0";
            lbl_CurrentCount.Text = "0";
            Check_IsCancelInWH.Enabled = true;
            Edt_BoxSN.Enabled = false;
            Edt_BoxSN.Text = string.Empty;
            Edt_PalletSN.Enabled = true;
            Edt_PalletSN.Text = "";
            Edt_PalletSN.Focus();
        }
        private DataSet GetStationData()
        {
            DataSet dsStationDetail = PartSelectSVC.GetPLCSeting("", List_Login.StationID.ToString());
            DS_StationTypeDef = PartSelectSVC.MESGetStationTypeParameter(List_Login.StationTypeID);

            //DataRow[] drREAD = dsStationDetail.Tables[0].Select("Name='READY'");
            //if (drREAD.Length > 0) ReadCmd = drREAD[0]["Value"].ToString(); else ReadCmd = "READY";

            //DataRow[] drPort = dsStationDetail.Tables[0].Select("Name='Port'");
            //if (drPort.Length > 0) strPort = drPort[0]["Value"].ToString(); else strPort = "COM1";

            //DataRow[] drAutoStation = dsStationDetail.Tables[0].Select("Name='AutoStation'");
            //if (drAutoStation.Length > 0) IsAuto = drAutoStation[0]["Value"].ToString(); else IsAuto = "0";

            //DataRow[] drIsBindAdhesive = dsStationDetail.Tables[0].Select("Name='IsBindAdhesive'");
            //if (drIsBindAdhesive.Length > 0) IsBindAdhesive = drIsBindAdhesive[0]["Value"].ToString(); else IsBindAdhesive = "0";

            //DataRow[] drIsNFC = dsStationDetail.Tables[0].Select("Name='IsBindNFC'");
            //if (drIsNFC.Length > 0) IsNFC = drIsNFC[0]["Value"].ToString(); else IsNFC = "0";

            //DataRow[] drIsNoPOCheck = dsStationDetail.Tables[0].Select("Name='IsNoPOCheck'");
            //if (drIsNoPOCheck.Length > 0) IsNoPOCheck = drIsNoPOCheck[0]["Value"].ToString(); else IsNoPOCheck = "0";

            //DataRow[] drIsNoPNCheck = dsStationDetail.Tables[0].Select("Name='IsNoPNCheck'");
            //if (drIsNoPNCheck.Length > 0) IsNoPNCheck = drIsNoPNCheck[0]["Value"].ToString(); else IsNoPNCheck = "0";

            DataRow[] DR_IsCheckPO = DS_StationTypeDef.Tables[0].Select("Description='IsCheckPO'");
            if (DR_IsCheckPO.Length > 0)
            {
                S_IsCheckPO = DR_IsCheckPO[0]["Content"].ToString();

                if (S_IsCheckPO == "1")
                {
                    DataRow[] DR_Child_IsCheckPO = dsStationDetail.Tables[0].Select("Name='IsCheckPO'");
                    if (DR_Child_IsCheckPO.Length > 0) S_IsCheckPO = DR_Child_IsCheckPO[0]["Value"].ToString(); else S_IsCheckPO = "1";
                }
            }
            else
            {
                S_IsCheckPO = "1";
            }

            DataRow[] DR_IsCheckPN = DS_StationTypeDef.Tables[0].Select("Description='IsCheckPN'");
            if (DR_IsCheckPN.Length > 0)
            {
                S_IsCheckPN = DR_IsCheckPN[0]["Content"].ToString();

                if (S_IsCheckPN == "1")
                {
                    DataRow[] DR_Child_IsCheckPN = dsStationDetail.Tables[0].Select("Name='IsCheckPN'");
                    if (DR_Child_IsCheckPN.Length > 0) S_IsCheckPN = DR_Child_IsCheckPN[0]["Value"].ToString(); else S_IsCheckPN = "1";
                }
            }
            else
            {
                S_IsCheckPN = "1";
            }


            if (S_IsCheckPO == "1")
            {
                lblProductionOrder.Visible = true;
                lblUnitState.Visible = true;
                lblUnitState.Location = new Point(58, 135);

                panel12.Visible = true;
            }
            else
            {
                lblProductionOrder.Visible = false;
                lblUnitState.Visible = true;

                lblUnitState.Location = new Point(58, 102);
                panel12.Visible = false;
            }
            return dsStationDetail;
        }

        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DateTime dateStart = DateTime.Now;
                string S_SN = Edt_BoxSN.Text.Trim();

                if(string.IsNullOrEmpty(S_SN))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20075", "NG", List_Login.Language);
                    return;
                }

                //获取配置是扫描栈板条码还是中箱条码
                int actualType = PartSelectSVC.GetBarcodeType(S_SN);
                if (IsScanPalletSNOrCartonBoxSN != actualType)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"Error:Should be {(IsScanPalletSNOrCartonBoxSN== 1?"Carton Barcode":"Pallet Barocde")}, current barcode: {S_SN}");
                    Edt_BoxSN.Focus();
                    Edt_BoxSN.Text = string.Empty;
                    return;
                }
                int? unitCount = 1;
                if (S_IsCheckPN == "1")
                {
                    DataSet dataSet = PartSelectSVC.GetProductDataInfo(S_SN, -1);
                    if (dataSet?.Tables[0].Rows.Count != 1)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"Error:Product info of the current barcode [{S_SN}] no match.");
                        Edt_BoxSN.Focus();
                        Edt_BoxSN.Text = string.Empty;
                        return;
                    }
                    S_PartID = dataSet.Tables[0].Rows[0]["PartID"].ToString();
                    if (S_PartID != Com_Part.EditValue.ToString())
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20043", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                        Edt_BoxSN.Focus();
                        Edt_BoxSN.Text = string.Empty;
                        return;
                    }
                    if (S_IsCheckPO == "1")
                    {
                        S_ProductionOrderID = dataSet.Tables[0].Rows[0]["ProductionOrderID"].ToString();
                        if (S_ProductionOrderID != Com_PO.EditValue.ToString())
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20050", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                            Edt_BoxSN.Focus();
                            Edt_BoxSN.Text = string.Empty;
                            return;
                        }
                    }
                }

                if (Check_IsCancelInWH.Checked == false)
                {
                    string result = string.Empty;
                    //包装Route校验
                    PartSelectSVC.uspCallProcedure("uspPackageRouteCheck", S_SN, xmlProdOrder, xmlPart,
                                                                xmlStation, xmlExtraData, "", ref result);

                    if (result != "1")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, result);
                        Edt_BoxSN.Text = string.Empty;
                        return;
                    }

                    //包装过站逻辑
                    PartSelectSVC.uspCallProcedure("uspSetPackageData", S_SN, xmlProdOrder, xmlPart,
                                                                xmlStation, xmlExtraData, "", ref result);

                    if (result != "1")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, result);
                        Edt_BoxSN.Text = string.Empty;
                        return;
                    }
                    //////////////////////////////////////////////////////////////
                    ///更新数量
                    if (checkedListBoxControl1.Items.Contains(S_SN))
                    {
                        lbl_CurrentCount.Text = (int.Parse(lbl_CurrentCount.Text) + unitCount).ToString();

                        int itemIndex = checkedListBoxControl1.Items.IndexOf(S_SN);
                        checkedListBoxControl1.Items[itemIndex].CheckState = CheckState.Checked;
                        checkedListBoxControl1.SelectedIndex = itemIndex;

                        if (lbl_CurrentCount.Text == lbl_totalCount.Text)
                        {
                            btn_reset_Click(null, null);
                        }
                    }
                    ///////////////////////////////////////////////////////////////
                    
                    TimeSpan ts = DateTime.Now - dateStart;
                    double mill = Math.Round(ts.TotalMilliseconds, 0);
                    Edt_BoxSN.Text = string.Empty;
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { S_SN, mill.ToString() });
                }
                else
                {
                    //反入库
                    string ReturnValue = DataCommitSVC.SetCancelInWHEntry(S_SN, Com_PO.EditValue.ToString().Trim(), Com_Part.EditValue.ToString().Trim(),
                        List_Login.StationID.ToString().Trim(), List_Login.EmployeeID.ToString().Trim(),"","");

                    TimeSpan ts = DateTime.Now - dateStart;
                    double mill = Math.Round(ts.TotalMilliseconds, 0);

                    Edt_BoxSN.Text = string.Empty;
                    if (ReturnValue != "OK")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ReturnValue }, "");
                        return;
                    }
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { S_SN, mill.ToString() });
                }

            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string[] List_Data = textBox2.Text.Split('\r');

                for (int i = 0; i < List_Data.Length; i++)
                {
                    string S_SN = List_Data[i].Replace("\n", "").Trim();

                    Edt_BoxSN.Text = S_SN;

                    Edt_SN_KeyDown(sender, e);

                    Thread.Sleep(500);
                }
            }
        }

        private void Edt_PalletSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Enter)
                return;

            string palletSN = Edt_PalletSN.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(palletSN))
                return;


            DateTime dateStart = DateTime.Now;
            Check_IsCancelInWH.Enabled = false;

            checkedListBoxControl1.Items.Clear();
            lbl_totalCount.Text = "0";
            lbl_CurrentCount.Text = "0";

            string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, Com_PartFamily.EditValue.ToString(),
                List_Login.StationTypeID,
                List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());


            if (IsScanPalletSNOrCartonBoxSN == 1)
            {
                //需要扫描箱号进行过站
                //获取产品数

                DataSet dataSet = public_.Get_PalletProductData(palletSN,PartSelectSVC);
                if (dataSet== null || dataSet.Tables.Count<= 0 || dataSet.Tables[0].Rows.Count <= 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"Error:please input the current barcode, current barcode: {palletSN}");
                    return;
                }
                
                DataTable dataTable1 =
                    dataSet.Tables[0].DefaultView.ToTable(true, new[] { "KITSN", "UnitStateID"});

                DataTable dataTable2 =
                    dataSet.Tables[0].DefaultView.ToTable(true, new[] { "KITSN" });

                if (dataTable2.Rows.Count != dataTable1.Rows.Count)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"Error:please check unit state by pallet sn, current barcode: {palletSN}");
                    return;
                }
                lbl_totalCount.Text = dataTable2.Rows.Count.ToString();
                foreach (DataRow item in dataTable1.Rows)
                {
                    DevExpress.XtraEditors.Controls.CheckedListBoxItem checkedListBoxItem = new DevExpress.XtraEditors.Controls.CheckedListBoxItem();

                    checkedListBoxItem.Value = $"{item["KITSN"].ToString()}";
                    if (item["UnitStateID"].ToString() == S_UnitStateID)
                    {
                        //已经扫描入库完成
                        checkedListBoxItem.CheckState = CheckState.Checked;
                        lbl_CurrentCount.Text = (int.Parse(lbl_CurrentCount.Text) + 1).ToString();
                    }
                    else
                    {
                        checkedListBoxItem.CheckState = CheckState.Unchecked;
                    }
                    checkedListBoxControl1.Items.Add(checkedListBoxItem);
                }

                if (lbl_CurrentCount.Text == lbl_totalCount.Text && lbl_totalCount.Text != "0")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"Error:please check unit state by pallet sn, current barcode: {palletSN}");
                    Edt_PalletSN.SelectAll();
                    return;
                }
                MessageInfo.Add_Info_MSG(Edt_MSG, "OK", $"Scan pallet barcode: {palletSN}");
                Edt_PalletSN.Enabled = false;
                Edt_BoxSN.Enabled = true;
                Edt_BoxSN.Focus();
                return;
            }
            string S_SN = Edt_PalletSN.Text.Trim();
            if (IsScanPalletSNOrCartonBoxSN == 2)
            {
                //只扫描栈板条码即可所有产品过站
                if (!Check_IsCancelInWH.Checked)
                {
                    //正常入库扫描过站
                    //获取配置是扫描栈板条码还是中箱条码
                    int actualType = PartSelectSVC.GetBarcodeType(S_SN);
                    if (IsScanPalletSNOrCartonBoxSN != actualType)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"Error:Should be {(IsScanPalletSNOrCartonBoxSN == 1 ? "Carton Barcode" : "Pallet Barocde")}, current barcode: {S_SN}");
                        Edt_PalletSN.SelectAll();
                        return;
                    }
                    if (S_IsCheckPN == "1")
                    {
                        DataSet dataSet = PartSelectSVC.GetProductDataInfo(S_SN, -1);
                        if (dataSet?.Tables[0].Rows.Count != 1)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"Error:Product info of the current barcode [{S_SN}] no match.");
                            Edt_PalletSN.SelectAll();
                            return;
                        }
                        S_PartID = dataSet.Tables[0].Rows[0]["PartID"].ToString();
                        if (S_PartID != Com_Part.EditValue.ToString())
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20043", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                            Edt_PalletSN.SelectAll();
                            return;
                        }
                        if (S_IsCheckPO == "1")
                        {
                            S_ProductionOrderID = dataSet.Tables[0].Rows[0]["ProductionOrderID"].ToString();
                            if (S_ProductionOrderID != Com_PO.EditValue.ToString())
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20050", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                                Edt_PalletSN.SelectAll();
                                return;
                            }
                        }
                    }

                    string result = string.Empty;
                    //包装Route校验
                    PartSelectSVC.uspCallProcedure("uspPackageRouteCheck", S_SN, xmlProdOrder, xmlPart,
                                                                xmlStation, xmlExtraData, "", ref result);

                    if (result != "1")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, result);
                        Edt_PalletSN.SelectAll();
                        return;
                    }

                    //包装过站逻辑
                    PartSelectSVC.uspCallProcedure("uspSetPackageData", S_SN, xmlProdOrder, xmlPart,
                                                                xmlStation, xmlExtraData, "", ref result);

                    if (result != "1")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, result);
                        Edt_PalletSN.SelectAll();
                        return;
                    }

                    TimeSpan ts = DateTime.Now - dateStart;
                    double mill = Math.Round(ts.TotalMilliseconds, 0);
                    Edt_PalletSN.Text = "";
                    Edt_PalletSN.Focus();
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { S_SN, mill.ToString() });

                }
                else
                {
                    //栈板暂时不允许反入库扫描过站

                    
                }
                return;
            }
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            if (!Check_IsCancelInWH.Checked)
            {
                lbl_totalCount.Text = "0";
                lbl_CurrentCount.Text = "0";
                Check_IsCancelInWH.Enabled = true;
                checkedListBoxControl1.Items.Clear();
                Edt_BoxSN.Enabled = false;
                Edt_BoxSN.Text = string.Empty;
                Edt_PalletSN.Enabled = true;
                Edt_PalletSN.Text = "";
                Edt_PalletSN.Focus();
            }
        }

        private void PackageOverStation_Form_Load(object sender, EventArgs e)
        {

        }


        private void checkedListBoxControl1_DrawItem(object sender, ListBoxDrawItemEventArgs e)
        {
            if ((e.State & DrawItemState.Checked) != 0 )
            {
                //e.Appearance.BackColor = Color.Green;
                e.Appearance.ForeColor = Color.Green;
            }
        }

        private void Check_IsCancelInWH_CheckedChanged(object sender, EventArgs e)
        {
            checkedListBoxControl1.Visible = !Check_IsCancelInWH.Checked;
            lbl_ScanPalletSN.Visible = !Check_IsCancelInWH.Checked;
            Edt_PalletSN.Visible = !Check_IsCancelInWH.Checked;
            tableLayoutPanel2.Visible = !Check_IsCancelInWH.Checked;
            btn_reset.Visible = !Check_IsCancelInWH.Checked;
            if (Check_IsCancelInWH.Checked)
            {
                Edt_BoxSN.Enabled = true;
                Edt_BoxSN.Text = "";
                Edt_BoxSN.Focus();
            }
            else
            {
                Edt_PalletSN.Enabled = true;
                Edt_BoxSN.Enabled = false;
                Edt_PalletSN.Text = "";
                Edt_PalletSN.Focus();
            }
        }
    }
}