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
    public partial class ToolingLinkTooling_Form : FrmBase
    {
        public ToolingLinkTooling_Form()
        {
            InitializeComponent();
        }

        public override void FrmBase_Load(object sender, EventArgs e)
        {
            base.FrmBase_Load(sender, e);
            ShowScanToolingName();
        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            txtNewTooling.Enabled = true;
            txtOldTooing.Enabled = false;
        }

        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            base.Btn_ConfirmPO_Click(sender, e);
            txtNewTooling.Enabled = true;
            txtOldTooing.Enabled = false;
            txtNewTooling.Focus();
        }

        private void ShowScanToolingName()
        {
            DataSet ds = PartSelectSVC.MESGetStationTypeParameter(List_Login.StationTypeID);
            DataRow[] drToolingFrom;
            DataRow[] drToolingTo;
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20000", "NG", List_Login.Language);
                return;
            }

            drToolingFrom = ds.Tables[0].Select("Description='ToolingFrom'");
            drToolingTo = ds.Tables[0].Select("Description='ToolingTo'");
            if (drToolingFrom.Length > 0)
            {
                lblToolingFrom.Text = drToolingFrom[0]["Content"].ToString();
            }
            if (drToolingTo.Length > 0)
            {
                lblToolingTo.Text = drToolingTo[0]["Content"].ToString();
            }
        }

        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string MachineSN = txtNewTooling.Text.Trim();
                if (string.IsNullOrEmpty(MachineSN))
                {
                    return;
                }

                string Result = string.Empty;
                string xmlPartStr = "<Part PartID=\"" + Com_Part.EditValue.ToString() + "\"> </Part>";

                PartSelectSVC.uspCallProcedure("uspMachineToolingCheck", MachineSN,
                                                                        null, xmlPartStr, null, null, List_Login.StationTypeID.ToString(), ref Result);

                if (Result != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { MachineSN, ProMsg }, Result);
                    txtNewTooling.Text = string.Empty;
                    txtNewTooling.Focus();
                    return;
                }

                txtNewTooling.Enabled = false;
                txtOldTooing.Enabled = true;
                txtOldTooing.Text = string.Empty;
                txtOldTooing.Focus();
            }
        }

        private void txtOldTooing_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    string MachineSN = txtOldTooing.Text.Trim();
                    if (string.IsNullOrEmpty(MachineSN))
                    {
                        return;
                    }

                    //string batch = string.Empty;
                    //DataSet dsPart = PartSelectSVC.BoxSnToBatch(MachineSN, out batch);
                    //if (string.IsNullOrEmpty(batch))
                    //{
                    //    MessageInfo.Add_Info_MSG(Edt_MSG, "20028", "NG", List_Login.Language, new string[] { MachineSN });
                    //    txtOldTooing.Text = string.Empty;
                    //    txtOldTooing.Focus();
                    //    return;
                    //}
                    //else if (batch.Substring(0, 5) == "ERROR")
                    //{
                    //    string ProMsg = MessageInfo.GetMsgByCode(batch.Replace("ERROR", ""), List_Login.Language);
                    //    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { MachineSN, ProMsg });
                    //    txtOldTooing.Text = string.Empty;
                    //    txtOldTooing.Focus();
                    //    return;
                    //}

                    //string OldPartID = dsPart.Tables[0].Rows[0]["PartID"].ToString();
                    //if(OldPartID!=Com_Part.EditValue.ToString())
                    //{
                    //    MessageInfo.Add_Info_MSG(Edt_MSG, "20043", "NG", List_Login.Language);
                    //    txtOldTooing.Text = string.Empty;
                    //    txtOldTooing.Focus();
                    //    return;
                    //}

                    string Result = string.Empty;
                    string xmlPartStr = "<Part PartID=\""+ Com_Part.EditValue.ToString() + "\"> </Part>";

                    PartSelectSVC.uspCallProcedure("uspMachineToolingCheck", MachineSN,
                                                                            null, xmlPartStr, null, null, List_Login.StationTypeID.ToString(), ref Result);

                    if (Result != "1")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { MachineSN, ProMsg }, Result);
                        txtOldTooing.Text = string.Empty;
                        txtOldTooing.Focus();
                        return;
                    }

                    string S_PartID = Com_Part.EditValue.ToString();
                    string S_POID = Com_PO.EditValue.ToString();
                    string PartFamilyID = Com_PartFamily.EditValue.ToString();
                    string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID, 
                        List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());
                    if (string.IsNullOrEmpty(S_UnitStateID))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
                        return;
                    }
                    string S_xmlPart = "'<BoxSN SN=" + "\"" + txtNewTooling.Text.Trim() + "\"" + "> </BoxSN>'";
                    string S_FormatSN = PartSelectSVC.mesGetSNFormatIDByList(S_PartID, Com_PartFamily.EditValue.ToString(),
                        List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), List_Login.StationTypeID.ToString());
                    DataSet DS = PartSelectSVC.uspSNRGetNext(S_FormatSN, 0, null, S_xmlPart, null, null, null);
                    DataTable DT = DS.Tables[1];
                    string S_SN = DT.Rows[0][0].ToString();

                    //数据保存
                    mesUnit v_mesUnit = new mesUnit();
                    v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                    v_mesUnit.StatusID = 1;
                    v_mesUnit.StationID = List_Login.StationID;
                    v_mesUnit.EmployeeID = List_Login.EmployeeID;
                    v_mesUnit.CreationTime = DateTime.Now;
                    v_mesUnit.LastUpdate = DateTime.Now;
                    v_mesUnit.PanelID = 0;
                    v_mesUnit.LineID = List_Login.LineID;
                    v_mesUnit.ProductionOrderID = Convert.ToInt32(S_POID);
                    v_mesUnit.RMAID = 0;
                    v_mesUnit.PartID = Convert.ToInt32(S_PartID);
                    v_mesUnit.LooperCount = 1;
                    string S_InsertUnit = mesUnitSVC.Insert(v_mesUnit);
                    v_mesUnit.ID = Convert.ToInt32(S_InsertUnit);

                    if (S_InsertUnit.Substring(0, 1) == "E")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_InsertUnit });
                        return;
                    }

                    //////////////////////////////////////  mesSerialNumber /////////////////////////////////////////////////////
                    mesSerialNumber v_mesSerialNumber = new mesSerialNumber();
                    v_mesSerialNumber.UnitID = Convert.ToInt32(S_InsertUnit);
                    v_mesSerialNumber.SerialNumberTypeID = 8;
                    v_mesSerialNumber.Value = S_SN;
                    mesSerialNumberSVC.Insert(v_mesSerialNumber);

                    //////////////////////////////////////  mesUnitDetail /////////////////////////////////////////////////////                                    
                    //mesUnitDetail v_mesUnitDetail = new mesUnitDetail();
                    //v_mesUnitDetail.UnitID = Convert.ToInt32(v_mesUnit.ID);
                    //v_mesUnitDetail.reserved_01 = txtNewTooling.Text.Trim();
                    //v_mesUnitDetail.reserved_02 = batch;
                    //v_mesUnitDetail.reserved_03 = "1";
                    //v_mesUnitDetail.reserved_04 = "";
                    //v_mesUnitDetail.reserved_05 = "";
                    //mesUnitDetailSVC.Insert(v_mesUnitDetail);

                    //////////////////////////////////////  mesHistory /////////////////////////////////////////////////////                                    
                    mesHistory v_mesHistory = new mesHistory();
                    v_mesHistory.UnitID = Convert.ToInt32(v_mesUnit.ID);
                    v_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                    v_mesHistory.EmployeeID = List_Login.EmployeeID;
                    v_mesHistory.StationID = List_Login.StationID;
                    v_mesHistory.EnterTime = DateTime.Now;
                    v_mesHistory.ExitTime = DateTime.Now;
                    v_mesHistory.ProductionOrderID = Convert.ToInt32(S_POID);
                    v_mesHistory.PartID = Convert.ToInt32(S_POID);
                    v_mesHistory.LooperCount = 1;
                    mesHistorySVC.Insert(v_mesHistory);

                    bool SetUnitComponent = false;
                    SetUnitComponent = PartSelectSVC.SetToolingLinkTooling(txtNewTooling.Text.Trim(),
                        txtOldTooing.Text.Trim(), v_mesUnit.ID, List_Login);
                    if(!SetUnitComponent)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20169", "NG", List_Login.Language);
                        return;
                    }

                    //更新状态
                    mesMachineSVC.MesModMachineBySNStationTypeID(txtNewTooling.Text.Trim(), List_Login.StationTypeID);
                    mesMachineSVC.MesModMachineBySNStationTypeID(txtOldTooing.Text.Trim(), List_Login.StationTypeID);

                    MessageInfo.Add_Info_MSG(Edt_MSG, "10009", "OK", List_Login.Language, new string[] { lblToolingFrom.Text.ToString(),
                        txtNewTooling.Text.Trim(), lblToolingTo.Text.ToString(), txtOldTooing.Text.Trim() });
                    base.SetOverStiaonQTY(true);
                    txtOldTooing.Enabled = false;
                    txtNewTooling.Enabled = true ;
                    txtNewTooling.Text = string.Empty;
                    txtOldTooing.Text = string.Empty;
                    txtNewTooling.Focus();
                }
                catch (Exception ex)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                }
            }
        }

        private void BtnRelease_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtOldTooing.Text.ToString()) && MessageInfo.Add_Info_MessageBox("10022", List_Login.Language))
            {
                string result = mesMachineSVC.MesToolingReleaseCheck(txtOldTooing.Text.Trim(), List_Login.StationTypeID.ToString());
                if (result != "1")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, result, "NG", List_Login.Language);
                    return;
                }
                PartSelectSVC.ModMachine(txtOldTooing.Text.Trim(), "1", true);
                MessageInfo.Add_Info_MSG(Edt_MSG, "10023", "OK", List_Login.Language);
                txtOldTooing.Text = string.Empty;
                txtOldTooing.Enabled = true;
                txtOldTooing.Focus();
            }
        }
    }
}