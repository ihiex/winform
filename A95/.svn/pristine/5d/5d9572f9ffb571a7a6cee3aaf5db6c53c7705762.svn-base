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
    public partial class OverStationNew_Form : FrmBase
    {
        string S_UnitID = "";
        bool COF = false;

        public OverStationNew_Form()
        {
            InitializeComponent();
        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            Edt_SN.Enabled = false;
            base.Btn_Defect.Visible = false;
        }

        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            base.Btn_ConfirmPO_Click(sender, e);
            Edt_SN.Enabled = true;
            Edt_SN.Focus();

            string PO = Com_PO.EditValue.ToString();
            DataSet dsCOF = PartSelectSVC.GetluPODetailDef(Convert.ToInt32(PO), "COF");
            if (dsCOF != null && dsCOF.Tables.Count > 0 && dsCOF.Tables[0].Rows.Count > 0)
            {
                COF = dsCOF.Tables[0].Rows[0]["Content"].ToString() == "1";
            }
        }

        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                DateTime dateStart = DateTime.Now;

                string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                string PartFamilyID = Com_PartFamily.EditValue.ToString();
                string S_SN = Edt_SN.Text.Trim();

                //工单  PartID
                string S_POPartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();

                if (S_SN == "")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }

                List<string> List_PO = public_.SnToPOID(S_SN);
                string S_POID = List_PO[0];

                if (S_POID.Length > 4)
                {
                    if (S_POID.Substring(0, 5) == "ERROR")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(S_POID.Replace("ERROR", ""), List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_POID.Replace("ERROR", ""));
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }
                }
                if (S_POID == "" || S_POID == "0")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20014", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                };

                //根据  SN  获取工单  料号
                mesProductionOrder v_mesProductionOrder = mesProductionOrderSVC.Get(Convert.ToInt32(S_POID));
                S_POPartID = v_mesProductionOrder.PartID.ToString();
                string S_ComPO = Com_PO.EditValue.ToString();
                if (S_POID != S_ComPO)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20015", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }

                DataTable DT_SN = PartSelectSVC.GetmesSerialNumber(S_SN).Tables[0]; 
                if (DT_SN.Rows.Count > 0)
                {
                    S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                    DataTable DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0];
                    string S_StationID = List_Login.StationID.ToString();

                    if (!COF)
                    {
                        if (DT_Unit.Rows[0]["StatusID"].ToString() != "1")
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20036", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                            return;
                        }
                    }

                    string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, S_SN);
                    if (S_RouteCheck == "1")
                    {
                        string S_SelectPartID = DT_Unit.Rows[0]["PartID"].ToString();
                        if (S_POPartID == S_SelectPartID)
                        {
                            try
                            {
                                //调用通用过程
                                string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>";
                                string xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                                string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                                string outString = string.Empty;
                                PartSelectSVC.uspCallProcedure("uspQCCheck", S_SN,
                                                                                        xmlProdOrder, xmlPart, xmlStation, null, null, ref outString);
                                if (outString != "1")
                                {
                                    string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, outString);
                                    Edt_SN.Focus();
                                    Edt_SN.Text = "";
                                    return;
                                }

                                string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID,
                                    List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());
                                if (string.IsNullOrEmpty(S_UnitStateID))
                                {
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                                    Edt_SN.Focus();
                                    Edt_SN.Text = "";
                                    return;
                                }
                                DataTable DT_UPUnitID = PartSelectSVC.Get_UnitID(S_SN).Tables[0];

                                //mesUnit v_mesUnit = new mesUnit();
                                //v_mesUnit.ID = Convert.ToInt32(DT_UPUnitID.Rows[0]["UnitID"].ToString());
                                //v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                                //v_mesUnit.StatusID = 1;
                                //v_mesUnit.StationID = List_Login.StationID;
                                //v_mesUnit.EmployeeID = List_Login.EmployeeID;
                                //v_mesUnit.LastUpdate = DateTime.Now;
                                //v_mesUnit.ProductionOrderID = Convert.ToInt32(S_POID);
                                ////修改 Unit
                                //string S_UpdateUnit = mesUnitSVC.Update(v_mesUnit);
                                //if (S_UpdateUnit.Substring(0, 1) == "E")
                                //{
                                //    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_UpdateUnit });
                                //    return;
                                //}

                                string sqlValue = "";
                                sqlValue = sqlValue + " update mesUnit set UnitStateID='" + Convert.ToInt32(S_UnitStateID) +
                                "',StatusID=1,StationID='" + List_Login.StationID + "',EmployeeID='" + List_Login.EmployeeID +
                                 "',ProductionOrderID='" + Convert.ToInt32(S_POID) + "',LastUpdate=getdate() where ID='" + Convert.ToInt32(DT_UPUnitID.Rows[0]["UnitID"].ToString()) + "'" +

                                " insert into mesHistory(UnitID, UnitStateID, EmployeeID, StationID, EnterTime, ExitTime, ProductionOrderID, PartID, LooperCount, StatusID) " +
                                "values('" + Convert.ToInt32(DT_UPUnitID.Rows[0]["UnitID"].ToString()) + "', '" + Convert.ToInt32(S_UnitStateID) + "', '" + List_Login.EmployeeID +
                                "', '" + List_Login.StationID + "', getdate(), GETDATE(), '" + Convert.ToInt32(S_POID) + "', '" + Convert.ToInt32(S_POPartID) + "', '1', 1) ";


                                string ReturnValue = PartSelectSVC.ExecSql(sqlValue);
                                if (ReturnValue != "OK")
                                {
                                    ReturnValue = "SN:" + S_SN + "  " + ReturnValue;
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ReturnValue });
                                    return;
                                }

                                base.SetOverStiaonQTY(true);

                                Edt_SN.Text = "";
                                Edt_SN.Enabled = true;
                                Edt_SN.Focus();

                                TimeSpan ts = DateTime.Now - dateStart;
                                double mill = Math.Round(ts.TotalMilliseconds, 0);
                                MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { S_SN, mill.ToString() });


                       

                                //if (S_UpdateUnit.Substring(0, 1) != "E")
                                //{
                                //    mesHistory v_mesHistory = new mesHistory();

                                //    v_mesHistory.UnitID = Convert.ToInt32(v_mesUnit.ID);
                                //    v_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                                //    v_mesHistory.EmployeeID = List_Login.EmployeeID;
                                //    v_mesHistory.StationID = List_Login.StationID;
                                //    v_mesHistory.EnterTime = DateTime.Now;
                                //    v_mesHistory.ExitTime = DateTime.Now;
                                //    v_mesHistory.ProductionOrderID = Convert.ToInt32(S_POID);
                                //    v_mesHistory.PartID = Convert.ToInt32(S_POPartID);
                                //    v_mesHistory.LooperCount = 1;
                                //    int I_InsertHistory = mesHistorySVC.Insert(v_mesHistory);
                                //    base.SetOverStiaonQTY(true);

                                //    Edt_SN.Text = "";
                                //    Edt_SN.Enabled = true;
                                //    Edt_SN.Focus();

                                //    TimeSpan ts = DateTime.Now - dateStart;
                                //    double mill = Math.Round(ts.TotalMilliseconds, 0);
                                //    MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { S_SN, mill.ToString() });
                                //}
                            }
                            catch (Exception ex)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                            }
                        }
                        else
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20050", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                        }
                    }
                    else
                    {
                        if (S_RouteCheck == "20243")
                        {
                            string nowUnitStateID = PartSelectSVC.GetSerialNumber2(S_SN).Tables[0].Rows[0]["UnitStateID"].ToString();
                            mesUnitState mesunitSateModel = mesUnitStateSVC.Get(Convert.ToInt32(nowUnitStateID));
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20243", "NG", List_Login.Language, new string[] { S_SN, mesunitSateModel.Description });
                        }
                        else
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(S_RouteCheck, List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_RouteCheck);
                            //MessageInfo.Add_Info_MSG(Edt_MSG, "20243", "NG", List_Login.Language, new string[] { S_SN, mesunitSateModel.Description + " [PartName]:" + dsMainSN.Tables[0].Rows[0]["PartNumber"].ToString() + " [LineName]:" + dsMainSN.Tables[0].Rows[0]["LineName"].ToString() });
                        }
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                    }
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                }
            }
        }
    }
}