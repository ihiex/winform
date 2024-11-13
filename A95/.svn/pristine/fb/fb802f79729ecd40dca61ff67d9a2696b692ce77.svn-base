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
    public partial class ScanToolingPrint : FrmBase
    {
        DataTable DT_PrintSn;
        string BoxLabelTemplate;

        public ScanToolingPrint()
        {
            InitializeComponent();
        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            this.txtSN.Text = string.Empty;
        }

        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            // 查询模板
            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
            string S_PartID = Com_Part.EditValue.ToString();
            string S_ProductionOrderID = Com_PO.EditValue.ToString();
            string S_LoginLineID = List_Login.LineID.ToString();

            BoxLabelTemplate = public_.GetLabelName(PartSelectSVC, S_StationTypeID, S_PartFamilyID,
                                                        S_PartID, S_ProductionOrderID, S_LoginLineID);

            if (string.IsNullOrEmpty(BoxLabelTemplate))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20076", "NG", List_Login.Language);
                return;
            }
            base.Btn_ConfirmPO_Click(sender, e);
        }

        private void txtSN_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                string S_ToolingSN = txtSN.Text.Trim();
                if (string.IsNullOrEmpty(S_ToolingSN))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                    txtSN.Focus();
                    return; 
                }

                DataSet dsDetail = mesUnitDetailSVC.MesGetBatchIDByBarcodeSN(S_ToolingSN);
                if(dsDetail==null || dsDetail.Tables.Count==0 || dsDetail.Tables[0].Rows.Count==0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20054", "NG", List_Login.Language);
                    txtSN.Focus();
                    return; 
                }

                string UnitID = dsDetail.Tables[0].Rows[0]["UnitID"].ToString();
                DataSet dsSN = PartSelectSVC.GetmesSerialNumberByUnitID(UnitID);
                string S_SN = dsSN.Tables[0].Rows[0]["Value"].ToString();

                string S_ComPO = Com_PO.EditValue.ToString();
                string S_PartID = Com_Part.EditValue.ToString();
                string PartFamilyID = Com_PartFamily.EditValue.ToString();
                List<string> List_PO = public_.SnToPOID(S_SN);
                string S_POID = List_PO[0];

                if (S_POID == "" || S_POID == "0")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20014", "NG", List_Login.Language, new string[] { S_SN });
                    txtSN.Focus();
                    txtSN.Text = "";
                    return;
                }

                if (S_POID != S_ComPO)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20015", "NG", List_Login.Language, new string[] { S_SN });
                    txtSN.Focus();
                    txtSN.Text = "";
                    return;
                }

                //根据  SN  获取工单  料号
                mesProductionOrder v_mesProductionOrder = mesProductionOrderSVC.Get(Convert.ToInt32(S_POID));
                string S_POPartID = v_mesProductionOrder.PartID.ToString();
                if (S_POPartID != S_PartID)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20050", "NG", List_Login.Language);
                    txtSN.Focus();
                    txtSN.Text = "";
                    return;
                }

                DataTable DT_SN = PartSelectSVC.GetmesSerialNumber(S_SN).Tables[0];
                if (DT_SN==null || DT_SN.Rows.Count == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language, new string[] { S_SN });
                    txtSN.Focus();
                    txtSN.Text = "";
                    return;
                }
                string S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                DataTable DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0];

                //校验工艺路线
                string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, S_SN);
                if (S_RouteCheck != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(S_RouteCheck, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_RouteCheck);
                    txtSN.Focus();
                    txtSN.Text = "";
                    return;
                }
                string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID, 
                    List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());
                if (string.IsNullOrEmpty(S_UnitStateID))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
                    txtSN.Focus();
                    txtSN.Text = "";
                    return;
                }
                //mesUnit v_mesUnit = new mesUnit();
                //v_mesUnit.ID = Convert.ToInt32(S_UnitID);
                //v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                //v_mesUnit.StatusID = 1;
                //v_mesUnit.StationID = List_Login.StationID;
                //v_mesUnit.EmployeeID = List_Login.EmployeeID;
                //v_mesUnit.LastUpdate = DateTime.Now;
                //v_mesUnit.ProductionOrderID = Convert.ToInt32(S_POID);
                //string S_UpdateUnit = mesUnitSVC.Update(v_mesUnit);
                //if (S_UpdateUnit.Substring(0, 1) == "E")
                //{
                //    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_UpdateUnit });
                //    return;
                //}

                //mesHistory v_mesHistory = new mesHistory();
                //v_mesHistory.UnitID = Convert.ToInt32(v_mesUnit.ID);
                //v_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                //v_mesHistory.EmployeeID = List_Login.EmployeeID;
                //v_mesHistory.StationID = List_Login.StationID;
                //v_mesHistory.EnterTime = DateTime.Now;
                //v_mesHistory.ExitTime = DateTime.Now;
                //v_mesHistory.ProductionOrderID = Convert.ToInt32(S_POID);
                //v_mesHistory.PartID = Convert.ToInt32(S_POPartID);
                //v_mesHistory.LooperCount = 1;
                //int I_InsertHistory = mesHistorySVC.Insert(v_mesHistory);

                string sqlValue = "";
                sqlValue = sqlValue + " update mesUnit set UnitStateID='" + Convert.ToInt32(S_UnitStateID) +
                "',StatusID=1" + ",StationID='" + List_Login.StationID + "',EmployeeID='" + List_Login.EmployeeID +
                "',ProductionOrderID='" + Convert.ToInt32(S_POID) + "',LastUpdate=getdate() where ID='" + Convert.ToInt32(S_UnitID) + "'" +

                " insert into mesHistory(UnitID, UnitStateID, EmployeeID, StationID, EnterTime, ExitTime, ProductionOrderID, PartID, LooperCount, StatusID) " +
                "values('" + Convert.ToInt32(S_UnitID) + "', '" + Convert.ToInt32(S_UnitStateID) + "', '" + List_Login.EmployeeID +
                "', '" + List_Login.StationID + "', getdate(), GETDATE(), '" + Convert.ToInt32(S_POID) + "', '" + Convert.ToInt32(S_POPartID) + "', '1', '1'"  + ") ";

                string S_UpdateDetail = mesMachineSVC.MesModMachineBySNStationTypeID_Sql(S_ToolingSN, List_Login.StationTypeID);

                if (S_UpdateDetail.Substring(0, 5) == "ERROR")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_UpdateDetail });
                    return;
                }
                else
                {
                    sqlValue += "\r\n" + S_UpdateDetail;

                    string ReturnValue = PartSelectSVC.ExecSql(sqlValue);
                    if (ReturnValue != "OK")
                    {
                        ReturnValue = "SN:" + S_SN + "  " + ReturnValue;
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ReturnValue });
                        return;
                    }
                }

                //修改夹具状态及属性
                //mesMachineSVC.MesModMachineBySNStationTypeID(S_ToolingSN, List_Login.StationTypeID);

                if (DT_PrintSn != null)
                {
                    DT_PrintSn.Columns.Clear();
                    DT_PrintSn.Rows.Clear();
                }
                else
                {
                    DT_PrintSn = new DataTable();
                }
                DT_PrintSn.Columns.Add("SN");
                DT_PrintSn.Columns.Add("CreateTime");
                DT_PrintSn.Columns.Add("PrintTime");

                DataRow DR = DT_PrintSn.NewRow();
                DR["SN"] = S_SN;
                DR["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DT_PrintSn.Rows.Add(DR);

                string PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, BoxLabelTemplate,
                    DT_PrintSn, Convert.ToInt32(Com_Part.EditValue.ToString()));
                if (PrintResult != "OK")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(PrintResult, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20111", "NG", List_Login.Language, new string[] { ProMsg }, PrintResult);
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10021", "OK", List_Login.Language);
                    txtSN.Text = string.Empty;
                    txtSN.Focus();
                }
            }
        }

        private void ScanToolingPrint_Load(object sender, EventArgs e)
        {
            public_.OpenBartender(List_Login.StationID.ToString());
        }
    }
}
