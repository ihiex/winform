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

namespace App.MyMES
{
    public partial class RMAChange_Form : FrmBase
    {
        string[] JumpFromUnitStateIDs;
        string JumpToUnitStateID = string.Empty;
        string JumpFromStatusID = string.Empty;
        string JumpToStateID = string.Empty;
        //private string[] OldFGUnitStateIDs;

        private string PartID = string.Empty;
        private string POID = string.Empty;
        private string StateID = string.Empty;
        private string OldFGUnitID = string.Empty;
        public RMAChange_Form()
        {
            InitializeComponent();
        }

        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            DataSet dtsStation = PartSelectSVC.GetPLCSeting("", List_Login.StationID.ToString());
            if (dtsStation == null || dtsStation.Tables.Count == 0 || dtsStation.Tables[0].Rows.Count == 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20200", "NG", List_Login.Language);
                return;
            }
            DataTable dtStation = dtsStation.Tables[0];

            DataRow[] dr1 = dtStation.Select("Name='JumpFromUnitStateID'");
            if (dr1 == null || dr1.Length == 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20199", "NG", List_Login.Language, new string[] { "JumpFromUnitStateID" });
                return;
            }
            JumpFromUnitStateIDs = dr1[0]["Value"].ToString().Split(',');
            
            DataRow[] dr2 = dtStation.Select("Name='JumpToUnitStateID'");
            if (dr2 == null || dr2.Length == 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20199", "NG", List_Login.Language, new string[] { "JumpToUnitStateID" });
                return;
            }
            JumpToUnitStateID = dr2[0]["Value"].ToString();

            DataRow[] dr3 = dtStation.Select("Name='JumpStatusID'");
            if (dr3 == null || dr3.Length == 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20199", "NG", List_Login.Language, new string[] { "JumpStatusID" });
                return;
            }
            JumpFromStatusID = dr3[0]["Value"].ToString();


            DataRow[] dr4 = dtStation.Select("Name='JumpUnitStateID'");
            if (dr4 == null || dr4.Length == 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20199", "NG", List_Login.Language, new string[] { "JumpUnitStateID" });
                return;
            }
            JumpToStateID = dr4[0]["Value"].ToString();

            base.Btn_ConfirmPO_Click(sender, e);
            base.Btn_Defect.Enabled = true;
            base.Com_luUnitStatus.Enabled = true;
            PartID = Com_Part.EditValue.ToString();
            POID = Com_PO.EditValue.ToString();

            txtOldSN.Enabled = true;

        }

        private void txtOldSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Enter)
                return;

            DateTime dateStart = DateTime.Now;
            string tmpOldSN = txtOldSN.Text.Trim();
            if (string.IsNullOrEmpty(tmpOldSN))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                txtOldSN.Focus();
                return;
            }

            var OldSNDataset = PartSelectSVC.GetmesSerialNumber(tmpOldSN);
            if (OldSNDataset == null || OldSNDataset.Tables.Count <= 0 || OldSNDataset.Tables[0].Rows.Count <= 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language, new string[] { tmpOldSN });
                txtOldSN.Focus();
                txtOldSN.SelectAll();
                return;
            }

            string NumberType = OldSNDataset.Tables[0]?.Rows[0]["SerialNumberTypeID"].ToString();
            if (NumberType != "6" && NumberType != "5")
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "60202", "NG", List_Login.Language, new string[] { tmpOldSN });
                txtOldSN.Focus();
                txtOldSN.SelectAll();
                return;
            }

            string FGSN = string.Empty;
            FGSN = NumberType == "6" ? mesUnitDetailSVC.MesGetFGSNByUPCSN(tmpOldSN) : tmpOldSN;
            if (FGSN.Contains(","))
                LogNet.LogEor("format error", $"input SN : {tmpOldSN}, get FG SN : {FGSN}....");

            var OldFGUnit = PartSelectSVC.GetSNToUnit(FGSN);
            if (OldFGUnit == null || OldFGUnit.Tables.Count <= 0 || OldFGUnit.Tables[0].Rows.Count <= 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20189", "NG", List_Login.Language, new string[] { tmpOldSN });
                txtOldSN.Focus();
                txtOldSN.SelectAll();
                return;
            }

            string cUnitStateID = OldFGUnit.Tables[0].Rows[0]["UnitStateID"].ToString();
            string cStateID = OldFGUnit.Tables[0].Rows[0]["StatusID"].ToString();
            OldFGUnitID = OldFGUnit.Tables[0].Rows[0]["UnitID"].ToString();

            if (string.IsNullOrEmpty(cUnitStateID) || !JumpFromUnitStateIDs.Contains(cUnitStateID))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20201", "NG", List_Login.Language);
                txtOldSN.Focus();
                txtOldSN.SelectAll();
                return;
            }

            if (JumpFromStatusID != cStateID)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20045", "NG", List_Login.Language);
                txtOldSN.Focus();
                txtOldSN.SelectAll();
                return;
            }

            if (string.IsNullOrEmpty(JumpToStateID) || Com_luUnitStatus.EditValue.ToString() != JumpToStateID)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "60600", "NG", List_Login.Language);
                txtOldSN.Focus();
                txtOldSN.SelectAll();
                return;
            }




            string xmlUnitDefect = "";
            //////////////////////////  Defect ///////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////
            string S_DefectID = base.DefectChar;
            string[] Array_Defect = S_DefectID.Split(';');
            if (Com_luUnitStatus.EditValue.ToString() != "1")
            {
                int i = 1;
                foreach (var item in Array_Defect)
                {
                    try
                    {
                        if (item.Trim() != "")
                        {
                            int I_DefectID = Convert.ToInt32(item);

                            xmlUnitDefect = xmlUnitDefect + "&DefectID" + i + "=" + I_DefectID + "";
                            i++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                        return;
                    }
                }
            }


            string xmlExtraData = "UnitID=" + OldFGUnitID + "&UnitStateID=" + Convert.ToInt32(JumpToUnitStateID)
                                  + "&EmployeeID=" + List_Login.EmployeeID + "&StatusID=" + JumpToStateID;

            xmlExtraData = xmlExtraData + xmlUnitDefect;
            string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + Convert.ToInt32(Com_PO.EditValue.ToString()) + "\"> </ProdOrder>";
            string xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
            string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";

            string outString = string.Empty;
            PartSelectSVC.uspCallProcedure("uspRMAChange", FGSN, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, null, ref outString);
            if (outString != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { tmpOldSN, ProMsg }, outString);

                txtOldSN.Focus();
                txtOldSN.SelectAll();
                return;
            }

            TimeSpan ts = DateTime.Now - dateStart;
            double mill = Math.Round(ts.TotalMilliseconds, 0);
            MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { txtOldSN.Text.Trim(), mill.ToString() });
            txtOldSN.Text = "";
            txtOldSN.Focus();
        }


    }
}
