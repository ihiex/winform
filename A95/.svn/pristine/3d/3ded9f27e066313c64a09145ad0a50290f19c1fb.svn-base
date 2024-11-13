using App.Model;
using App.MyMES.PartSelectService;
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
    public partial class ORTReplaceSN : DevExpress.XtraEditors.XtraForm
    {
        public PartSelectSVCClient PartSelectSVC;
        LoginList List_Login = new LoginList();
        string DetailID = string.Empty;

        public ORTReplaceSN()
        {
            InitializeComponent();
        }

        public ORTReplaceSN(LoginList login, PartSelectSVCClient PartSelect)
        {
            this.PartSelectSVC = PartSelect;
            this.List_Login = login;
            InitializeComponent();
        }

        private void txtSN_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string OldSN = txtSN.Text.Trim();
                    if (string.IsNullOrEmpty(OldSN))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                        txtSN.Text = string.Empty;
                        return;
                    }

                    string result = string.Empty;
                    DataSet dsResult = PartSelectSVC.uspCallProcedure("uspORTGetData", OldSN, "", "",
                                                                    "", "", "4", ref result);
                    if (dsResult == null || dsResult.Tables.Count == 0 || dsResult.Tables[0].Rows.Count == 0)
                    {
                        DetailID = string.Empty;
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language,new string[] { OldSN });
                        return;
                    }
                    DataTable dtOld = dsResult.Tables[0];
                    txtPartFamilyType.Text = dtOld.Rows[0]["PartFamilyType"].ToString();
                    txtPo.Text = dtOld.Rows[0]["ProductionOrder"].ToString();
                    txtORTStatus.Text = dtOld.Rows[0]["ORTState"].ToString();
                    txtLine.Text = dtOld.Rows[0]["Line"].ToString();
                    txtYear.Text = dtOld.Rows[0]["YearID"].ToString();
                    txtWeek.Text = dtOld.Rows[0]["WeekID"].ToString();
                    txtFQCTime.Text = dtOld.Rows[0]["FQCTime"].ToString();
                    txtBatch.Text = dtOld.Rows[0]["OrderNo"].ToString();
                    txtTestType.Text = dtOld.Rows[0]["TestTypeID"].ToString();
                    DetailID = dtOld.Rows[0]["DetailID"].ToString();
                    btnUpdate.Enabled = true;
                    txtSN.ReadOnly = true;
                    if (!checkUpdateStaus.Checked)
                    {
                        txtNewSN.ReadOnly = false;
                    }
                }
            }
            catch(Exception ex)
            {
                string result = string.Empty;
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void checkUpdateStaus_CheckedChanged(object sender, EventArgs e)
        {
            if(checkUpdateStaus.Checked)
            {
                btnUpdate.Visible = true;
                txtNewSN.Text = string.Empty;
                txtNewPartFamilyType.Text = string.Empty;
                txtNewPO.Text = string.Empty;
                txtNewSNstate.Text = string.Empty;
                txtNewLine.Text = string.Empty;
                txtNewFQCTime.Text = string.Empty;
                txtNewSN.ReadOnly = true;
            }
            else
            {
                btnUpdate.Visible = false;
                if(!string.IsNullOrEmpty(DetailID))
                {
                    txtNewSN.ReadOnly = false;
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string Status = comboxStatus.Text.ToString();
            if (Status==txtORTStatus.Text.ToString())
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20236", "NG", List_Login.Language);
                return;
            }

            string result = string.Empty;
            string StrStatus = Status == "Passed" ? "2" : Status == "Red" ? "3" : "4";
            PartSelectSVC.uspCallProcedure("uspORTUpdateStatus", txtSN.Text.Trim(), "", "",
                                                                "", "", StrStatus, ref result);
            if (result != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, result);
            }
            else
            {
                txtSN.Text = string.Empty;
                txtSN.ReadOnly = false;
                txtPartFamilyType.Text = string.Empty;
                txtPo.Text = string.Empty;
                txtTestType.Text = string.Empty;
                txtLine.Text = string.Empty;
                txtYear.Text = string.Empty;
                txtWeek.Text = string.Empty;
                txtORTStatus.Text = string.Empty;
                txtFQCTime.Text = string.Empty;
                txtBatch.Text = string.Empty;
                MessageInfo.Add_Info_MSG(Edt_MSG, "10045", "OK", List_Login.Language, new string[] { txtSN.Text.Trim(), Status }, result);
            }
            btnUpdate.Enabled = false;
            txtSN.ReadOnly = false;
        }

        private void txtNewSN_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                string NewSN = txtNewSN.Text.Trim();
                if(string.IsNullOrEmpty(NewSN))
                {
                    return;
                }

                string Status = comboxStatus.Text.ToString();
                string StrStatus = Status == "Passed" ? "2" : Status == "Red" ? "3" : "4";
                string xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";
                string xmlExtraData = "<ExtraData EmployeeId =\"" + List_Login.EmployeeID + "\" " +
                                             "ORTStatus =\"" + StrStatus + "\"> </ExtraData>";
                string result = string.Empty;
                DataSet dsSNResult = PartSelectSVC.uspCallProcedure("uspORTReplaceDetailSN", NewSN, "", "",
                                                                xmlStation, xmlExtraData, txtSN.Text.Trim(), ref result);


                if (dsSNResult != null && dsSNResult.Tables.Count > 0 && dsSNResult.Tables[0].Rows.Count > 0)
                {
                    txtNewPartFamilyType.Text = dsSNResult.Tables[0].Rows[0]["PartFamilyType"].ToString();
                    txtNewPO.Text = dsSNResult.Tables[0].Rows[0]["ProductionOrder"].ToString();
                    txtNewSNstate.Text = dsSNResult.Tables[0].Rows[0]["Status"].ToString();
                    txtNewLine.Text = dsSNResult.Tables[0].Rows[0]["Line"].ToString();
                    txtNewFQCTime.Text = dsSNResult.Tables[0].Rows[0]["FQCDate"].ToString();
                }

                if (result != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, result);
                    txtNewSN.SelectAll();
                    return;
                }
                else
                {
                    txtSN.Text = string.Empty;
                    txtSN.ReadOnly = false;
                    txtPartFamilyType.Text = string.Empty;
                    txtPo.Text = string.Empty;
                    txtTestType.Text = string.Empty;
                    txtLine.Text = string.Empty;
                    txtYear.Text = string.Empty;
                    txtWeek.Text = string.Empty;
                    txtORTStatus.Text = string.Empty;
                    txtFQCTime.Text = string.Empty;
                    txtBatch.Text = string.Empty;
                    txtNewSN.ReadOnly = true;
                    txtNewSN.Text = string.Empty;
                    txtNewPartFamilyType.Text = string.Empty;
                    txtNewPO.Text = string.Empty;
                    txtNewSNstate.Text = string.Empty;
                    txtNewLine.Text = string.Empty;
                    txtNewFQCTime.Text = string.Empty;
                    txtSN.Focus();
                }

                MessageInfo.Add_Info_MSG(Edt_MSG, "10045", "OK", List_Login.Language, new string[] { NewSN ,txtSN.Text.Trim() }, result);
            }
        }

        private void ORTReplaceSN_Load(object sender, EventArgs e)
        {
            comboxStatus.SelectedIndex = 0;
            btnUpdate.Visible = false;
            DataSet dsFrmBase = PartSelectSVC.GetLanguage(this.Name, "FRM");
            if (dsFrmBase != null && dsFrmBase.Tables.Count > 0 && dsFrmBase.Tables[0].Rows.Count > 0)
            {
                DataTable dtFrmBase = dsFrmBase.Tables[0];
                foreach (Control ctrl in this.Controls)
                {
                   MessageInfo.LanguageForLableControl(ctrl, dtFrmBase, List_Login.Language);
                }
            }
        }
    }
}
