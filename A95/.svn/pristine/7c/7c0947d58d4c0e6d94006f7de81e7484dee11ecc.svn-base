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
    public partial class JumpStation_Form : FrmBase3
    {
        string JumpFromUnitStateID = string.Empty;
        string JumpToUnitStateID = string.Empty;
        string JumpStatusID = string.Empty;

        public JumpStation_Form()
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
            if(dr1==null || dr1.Length==0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20199", "NG", List_Login.Language,new string[] { "JumpFromUnitStateID" });
                return;
            }
            JumpFromUnitStateID = dr1[0]["Value"].ToString();

            DataRow[] dr2= dtStation.Select("Name='JumpToUnitStateID'");
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
            JumpStatusID = dr3[0]["Value"].ToString();

            base.Btn_ConfirmPO_Click(sender, e);
        }

        private void txtSN_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DateTime dateStart = DateTime.Now;
                    string SN = txtSN.Text.Trim();
                    if (string.IsNullOrEmpty(SN))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                        txtSN.Focus();
                        txtSN.Text = "";
                        return;
                    }

                    DataSet dsUnit = PartSelectSVC.GetSerialNumber2(SN);
                    if (dsUnit == null || dsUnit.Tables.Count == 0 || dsUnit.Tables[0].Rows.Count == 0)
                    {
                        DataSet dsMachine = mesUnitDetailSVC.MesGetBatchIDByBarcodeSN(SN);
                        if (dsMachine == null || dsMachine.Tables.Count == 0 || dsMachine.Tables[0].Rows.Count == 0)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language,new string[] { SN });
                            txtSN.Focus();
                            txtSN.Text = "";
                            return;
                        }
                        string UnitID = dsMachine.Tables[0].Rows[0]["dsMachine"].ToString();
                        SN = PartSelectSVC.GetmesSerialNumberByUnitID(UnitID).Tables[0].Rows[0]["Value"].ToString();
                        dsUnit = PartSelectSVC.GetSerialNumber2(SN);
                    }

                    DataTable dtUnit = dsUnit.Tables[0];
                    string UnitStateID = dtUnit.Rows[0]["UnitStateID"].ToString();
                    string StatusID = dtUnit.Rows[0]["StatusID"].ToString();
                    string ProductionOrderID = dtUnit.Rows[0]["ProductionOrderID"].ToString();

                    if (ProductionOrderID != Com_PO.EditValue.ToString())
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20050", "NG", List_Login.Language);
                        txtSN.Focus();
                        txtSN.Text = "";
                        return; 
                    }

                    if (UnitStateID != JumpFromUnitStateID)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20201", "NG", List_Login.Language);
                        txtSN.Focus();
                        txtSN.Text = "";
                        return;
                    }
                    else if (StatusID != JumpStatusID)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20045", "NG", List_Login.Language);
                        txtSN.Focus();
                        txtSN.Text = "";
                        return;
                    }

                    mesUnit v_mesUnit = new mesUnit();
                    v_mesUnit.ID = Convert.ToInt32(dtUnit.Rows[0]["UnitID"].ToString());
                    v_mesUnit.UnitStateID = Convert.ToInt32(JumpToUnitStateID);
                    v_mesUnit.StatusID = 1;
                    v_mesUnit.StationID = List_Login.StationID;
                    v_mesUnit.EmployeeID = List_Login.EmployeeID;
                    v_mesUnit.LastUpdate = DateTime.Now;
                    v_mesUnit.ProductionOrderID = Convert.ToInt32(dtUnit.Rows[0]["ProductionOrderID"].ToString());
                    string S_UpdateUnit = mesUnitSVC.Update(v_mesUnit);
                    if (S_UpdateUnit.Substring(0, 1) == "E")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_UpdateUnit });
                        return;
                    }

                    mesHistory v_mesHistory = new mesHistory();

                    v_mesHistory.UnitID = Convert.ToInt32(v_mesUnit.ID);
                    v_mesHistory.UnitStateID = Convert.ToInt32(JumpToUnitStateID);
                    v_mesHistory.EmployeeID = List_Login.EmployeeID;
                    v_mesHistory.StationID = List_Login.StationID;
                    v_mesHistory.EnterTime = DateTime.Now;
                    v_mesHistory.ExitTime = DateTime.Now;
                    v_mesHistory.ProductionOrderID = Convert.ToInt32(dtUnit.Rows[0]["ProductionOrderID"].ToString());
                    v_mesHistory.PartID = Convert.ToInt32(dtUnit.Rows[0]["PartID"].ToString());
                    v_mesHistory.LooperCount = 1;
                    mesHistorySVC.Insert(v_mesHistory);

                    TimeSpan ts = DateTime.Now - dateStart;
                    double mill = Math.Round(ts.TotalMilliseconds, 0);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { txtSN.Text.Trim(), mill.ToString() });

                    txtSN.Focus();
                    txtSN.Text = "";
                } 
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }
    }
}
