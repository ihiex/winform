using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using App.Model;

namespace App.MyMES
{
    public delegate void FGSNCheckEventHandler(string Result, string FGSN);
    public delegate void UPCSNSubmitEventHandler(string UPCSN, string FGSN);

    public partial class SNlinkUPC_Form : FrmBase
    {
        public event FGSNCheckEventHandler FGSNCompletedCheck;
        public event UPCSNSubmitEventHandler UPCSNCompletedSubmit;

        bool IsScanUPC = false;
        bool IsScanJAN = false;

        public SNlinkUPC_Form()
        {
            InitializeComponent();
            FGSNCompletedCheck += this.FGSNCheckCompleted;
            UPCSNCompletedSubmit += this.UPCSNSubmitCompleted;
        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            IsScanUPC = false;
            IsScanJAN = false;
        }

        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            
            try
            {
                DataSet dtSet = PartSelectSVC.GetProductionOrderDetailDef(Com_PO.EditValue.ToString());
                if (dtSet == null || dtSet.Tables.Count == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20070", "NG", List_Login.Language);
                    return;
                }

                DataTable dtPara = dtSet.Tables[0];
                DataRow[] drUpc = dtPara.Select("Description='IsScanUPC'");
                if (drUpc.Count() > 0)
                {
                    IsScanUPC = drUpc[0]["Content"].ToString() == "1";
                }

                if (IsScanUPC)
                {
                    DataRow[] drUpcCode = dtPara.Select("Description='UPC'");
                    if (drUpcCode.Count() == 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20071", "NG", List_Login.Language);
                        return;
                    }
                    lblUPCCode.Text = drUpcCode[0]["Content"].ToString();
                    lblUPC.Visible = true;
                    txtUPC.Visible = true;
                    lblUPCCode.Visible = true;
                }
                else
                {
                    lblUPC.Visible = false;
                    txtUPC.Visible = false;
                    lblUPCCode.Visible = false;
                }

                DataRow[] drJan = dtPara.Select("Description='IsScanJAN'");
                if (drJan.Count() > 0)
                {
                    IsScanJAN = drJan[0]["Content"].ToString() == "1";
                }

                if (IsScanJAN)
                {
                    DataRow[] drJanCode = dtPara.Select("Description='Jan'");
                    if (drJanCode.Count() == 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20198", "NG", List_Login.Language, new string[] { "Jan" });
                        return;
                    }
                    lblJANCode.Text = drJanCode[0]["Content"].ToString();
                    lblJAN.Visible = true;
                    txtJan.Visible = true;
                    lblJANCode.Visible = true;
                }
                else
                {
                    lblJAN.Visible = false;
                    txtJan.Visible = false;
                    lblJANCode.Visible = false;
                }

                base.Btn_ConfirmPO_Click(sender, e);
                Edt_SN.Text = string.Empty;
                Edt_UPCSN.Text = string.Empty;
                Edt_SN.Enabled = true;
                Edt_UPCSN.Enabled = false;
                txtUPC.Text = string.Empty;
                txtUPC.Enabled = false;
                txtJan.Text = string.Empty;
                txtJan.Enabled = false;
                Edt_SN.Focus();
            }
            catch(Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                return;
            }
        }

        private void FGSNCheckEvent(string Result, string FGSN)
        {
            FGSNCompletedCheck?.Invoke(Result, FGSN);
        }

        private void FGSNCheckCompleted(string Result, string FGSN)
        {
            if (Result != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { FGSN, ProMsg }, Result);
                Edt_SN.Text = string.Empty;
                Edt_SN.Focus();
                return;
            }
            else
            {
                Edt_SN.Enabled = false;
                if (!IsScanUPC && !IsScanJAN)
                {
                    Edt_UPCSN.Text = "";
                    Edt_UPCSN.Enabled = true;
                    Edt_UPCSN.Focus();
                    Edt_UPCSN.SelectAll();
                }
                else if (IsScanUPC)
                {
                    txtUPC.Text = string.Empty;
                    txtUPC.Enabled = true;
                    txtUPC.Focus();
                }
                else
                {
                    txtJan.Text = string.Empty;
                    txtJan.Enabled = true;
                    txtJan.Focus();
                }
            }
        }

        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string FGSN = Edt_SN.Text.Trim();
                string xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";
                string xmlPart = "<Part PartID=\"" + Com_Part.EditValue.ToString() + "\"> </Part>";
                string result = string.Empty;
                string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + Com_PO.EditValue.ToString() + "\"> </ProdOrder>";
                PartSelectSVC.uspCallProcedure("uspFGLinkUPCFGSNCheck", FGSN, xmlProdOrder, xmlPart,
                                                            xmlStation, "", null, ref result);
                FGSNCheckEvent(result, FGSN);
            }
        }


        private void UPCSNSubmitEvent(string UPCSN, string FGSN)
        {
            UPCSNCompletedSubmit?.Invoke(UPCSN, FGSN);
        }

        private void UPCSNSubmitCompleted(string UPCSN, string FGSN)
        {
            MessageInfo.Add_Info_MSG(Edt_MSG, "10008", "OK", List_Login.Language, new string[] { FGSN, UPCSN });

            Edt_SN.Text = "";
            Edt_SN.Enabled = true;
            Edt_SN.Focus();
            txtUPC.Text = string.Empty;
            txtJan.Text = string.Empty;

            Edt_UPCSN.Text = "";
            Edt_UPCSN.Enabled = false;
        }

        private void Edt_UPCSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    string UPCSN = Edt_UPCSN.Text.Trim();
                    string S_SN = Edt_SN.Text.Trim();

                    string xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";
                    string xmlPart = "<Part PartID=\"" + Com_Part.EditValue.ToString() + "\"> </Part>";
                    string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + Com_PO.EditValue.ToString() + "\"> </ProdOrder>";
                    string result = string.Empty;
                    PartSelectSVC.uspCallProcedure("uspFGLinkUPCUPCSNCheck", UPCSN, xmlProdOrder, xmlPart,
                                                                xmlStation, "", null, ref result);
                    if (result != "1")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { UPCSN, ProMsg }, result);
                        Edt_UPCSN.Text = string.Empty;
                        Edt_UPCSN.Focus();
                        return;
                    }
                    else
                    {
                        string S_StationTypeID = List_Login.StationTypeID.ToString();
                        string S_PartID = Com_Part.EditValue.ToString();
                        string PartFamilyID = Com_PartFamily.EditValue.ToString();
                        string S_POID = Com_PO.EditValue.ToString();
                        string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID, 
                            List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());
                        if (string.IsNullOrEmpty(S_UnitStateID))
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
                            Edt_UPCSN.Text = string.Empty;
                            Edt_UPCSN.Focus();
                            return;
                        }
                        DataTable DT_FGUnitID = PartSelectSVC.Get_UnitID(S_SN).Tables[0];
                        DataTable DT_UPCUnitID = PartSelectSVC.Get_UnitID(UPCSN).Tables[0];
                        string FGUnitID = DT_FGUnitID.Rows[0]["UnitID"].ToString();
                        string UPCUitID = DT_UPCUnitID.Rows[0]["UnitID"].ToString();

                        mesUnit v_mesUnit = new mesUnit();
                        v_mesUnit.ID = Convert.ToInt32(FGUnitID);
                        v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                        v_mesUnit.StatusID = 1;
                        v_mesUnit.StationID = List_Login.StationID;
                        v_mesUnit.EmployeeID = List_Login.EmployeeID;
                        v_mesUnit.LastUpdate = DateTime.Now;
                        v_mesUnit.ProductionOrderID = Convert.ToInt32(S_POID);
                        //修改 Unit
                        mesUnitSVC.Update(v_mesUnit);
                        v_mesUnit.ID = Convert.ToInt32(UPCUitID);
                        mesUnitSVC.Update(v_mesUnit);

                        mesHistory v_mesHistory = new mesHistory();
                        v_mesHistory.UnitID = Convert.ToInt32(FGUnitID);
                        v_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                        v_mesHistory.EmployeeID = List_Login.EmployeeID;
                        v_mesHistory.StationID = List_Login.StationID;
                        v_mesHistory.EnterTime = DateTime.Now;
                        v_mesHistory.ExitTime = DateTime.Now;
                        v_mesHistory.ProductionOrderID = Convert.ToInt32(S_POID);
                        v_mesHistory.PartID = Convert.ToInt32(S_PartID);
                        v_mesHistory.LooperCount = 1;
                        mesHistorySVC.Insert(v_mesHistory);
                        v_mesHistory.UnitID = Convert.ToInt32(UPCUitID);
                        mesHistorySVC.Insert(v_mesHistory);

                        PartSelectSVC.MESModifyUnitDetail(Convert.ToInt32(FGUnitID), "KitSerialNumber", UPCSN);
                        UPCSNSubmitEvent(UPCSN, S_SN);

                    }
                }
                catch (Exception ex)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Edt_SN.Text = string.Empty;
            Edt_SN.Enabled = true;
            Edt_UPCSN.Text = string.Empty;
            Edt_UPCSN.Enabled = false;
            txtUPC.Enabled = false;
            txtJan.Enabled = false;
            txtUPC.Text = string.Empty;
            txtJan.Text = string.Empty;
            Edt_SN.Focus();
        }

        private void txtUPC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string EnterCode = txtUPC.Text.Trim();
                if (EnterCode == lblUPCCode.Text.ToString())
                {
                    txtUPC.Enabled = false;
                    if (IsScanJAN && string.IsNullOrEmpty(txtJan.Text.Trim()))
                    {
                        txtJan.Enabled = true;
                        txtJan.Text = string.Empty;
                        txtJan.Focus();
                    }
                    else
                    {
                        Edt_UPCSN.Enabled = true;
                        Edt_UPCSN.Text = string.Empty;
                        Edt_UPCSN.Focus();
                    }
                }
                else if (string.IsNullOrEmpty(txtJan.Text.Trim()) && IsScanJAN && EnterCode == lblJANCode.Text.ToString())
                {
                    txtJan.Text = EnterCode;
                    txtJan.Enabled = false;
                    txtUPC.Text = string.Empty;
                    txtUPC.Focus();
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20050", "NG", List_Login.Language);
                    txtUPC.Focus();
                    txtUPC.Text = "";
                }
            }
        }

        private void txtJan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string EnterCode = txtJan.Text.Trim();
                if (EnterCode == lblJANCode.Text.ToString())
                {
                    txtJan.Enabled = false;
                    if (IsScanUPC && string.IsNullOrEmpty(txtUPC.Text.Trim()))
                    {
                        txtUPC.Enabled = true;
                        txtUPC.Text = string.Empty;
                        txtUPC.Focus();
                    }
                    else
                    {
                        Edt_UPCSN.Enabled = true;
                        Edt_UPCSN.Text = string.Empty;
                        Edt_UPCSN.Focus();
                    }
                }
                else if (string.IsNullOrEmpty(txtUPC.Text.Trim()) && IsScanUPC && EnterCode == txtUPC.Text.ToString())
                {
                    txtUPC.Text = EnterCode;
                    txtUPC.Enabled = false;
                    txtJan.Text = string.Empty;
                    txtJan.Focus();
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20050", "NG", List_Login.Language);
                    txtJan.Focus();
                    txtJan.Text = "";
                }
            }
        }
    }
}