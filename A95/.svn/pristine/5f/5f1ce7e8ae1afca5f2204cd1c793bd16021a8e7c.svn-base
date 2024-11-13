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
    public partial class ScanUPC_PrintFGSN_Form : FrmBase
    {
        DataTable DT_PrintSn;
        string FGSNLabelTemplate;
        string PrintUnitStateID;
        string[] RePrintStationID;

        public ScanUPC_PrintFGSN_Form()
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
            try
            {
                DataTable dtStationDetail = public_.GetmesStationConfig("PrintUPCUnitStateID", List_Login.StationID.ToString());
                if (dtStationDetail == null || dtStationDetail.Rows.Count == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20199", "NG", List_Login.Language, new string[] { "PrintUPCUnitStateID" });
                    return;
                }
                RePrintStationID = dtStationDetail.Rows[0]["Value"].ToString().Split(',');

                DataTable dtPrintUnitStateID = public_.GetmesStationConfig("PrintUnitStateID", List_Login.StationID.ToString());
                if (dtPrintUnitStateID == null || dtPrintUnitStateID.Rows.Count == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20199", "NG", List_Login.Language, new string[] { "dtPrintUnitStateID" });
                    return;
                }
                PrintUnitStateID = dtPrintUnitStateID.Rows[0]["Value"].ToString();

                // 查询模板
                string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                string S_PartID = Com_Part.EditValue.ToString();
                string S_ProductionOrderID = Com_PO.EditValue.ToString();
                string S_LoginLineID = List_Login.LineID.ToString();

                FGSNLabelTemplate = public_.GetLabelName(PartSelectSVC, List_Login.StationTypeID.ToString(), S_PartFamilyID,
                                                            S_PartID, S_ProductionOrderID, S_LoginLineID);

                if (string.IsNullOrEmpty(FGSNLabelTemplate))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20097", "NG", List_Login.Language);
                    return;
                }
                base.Btn_ConfirmPO_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                return;
            }
        }

        private void txtSN_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode != Keys.Enter)
                    return;

                string S_SN = txtSN.Text.Trim();
                string FGSN = string.Empty;
                string FGUnitID = string.Empty;
                string FGProductionOrderID = string.Empty;
                string FGPartID = string.Empty;

                if (string.IsNullOrEmpty(S_SN))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                    txtSN.Focus();
                    return;
                }

                DataSet UPCdts = PartSelectSVC.GetmesSerialNumber(S_SN);
                if (UPCdts == null || UPCdts.Tables.Count == 0 || UPCdts.Tables[0].Rows.Count == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language, new string[] { S_SN });
                    txtSN.Focus();
                    return;
                }

                string SerialNumberType = UPCdts.Tables[0].Rows[0]["SerialNumberTypeID"].ToString();

                //校验是否满足unitstate状态
                if (SerialNumberType == "6")
                {
                    string UPCUnitID = UPCdts.Tables[0].Rows[0]["UnitID"].ToString();
                    mesUnit mesUnit = mesUnitSVC.Get(Convert.ToInt32(UPCUnitID));
                    string unitStateID = mesUnit.UnitStateID.ToString();
                    if (!RePrintStationID.Contains(unitStateID))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20045", "NG", List_Login.Language);
                        txtSN.Text = string.Empty;
                        return;
                    }

                    //转换FG条码检测
                    FGSN = mesUnitDetailSVC.MesGetFGSNByUPCSN(S_SN);
                    if (string.IsNullOrEmpty(FGSN))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20189", "NG", List_Login.Language);
                        txtSN.Text = string.Empty;
                        return;
                    }

                    DataSet FGdts = PartSelectSVC.GetmesSerialNumber(FGSN);
                    FGUnitID = FGdts.Tables[0].Rows[0]["UnitID"].ToString();
                    mesUnit FGmesUnit = mesUnitSVC.Get(Convert.ToInt32(FGUnitID));
                    FGProductionOrderID = FGmesUnit.ProductionOrderID.ToString();
                    FGPartID = FGmesUnit.PartID.ToString();
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20035", "NG", List_Login.Language);
                    txtSN.Text = string.Empty;
                    return;
                }

                //打印逻辑
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
                DR["SN"] = FGSN;
                DR["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DT_PrintSn.Rows.Add(DR);

                string PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, FGSNLabelTemplate,
                DT_PrintSn, Convert.ToInt32(Com_Part.EditValue.ToString()));
                if (PrintResult != "OK")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(PrintResult, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20111", "NG", List_Login.Language, new string[] { ProMsg }, PrintResult);
                    txtSN.Text = string.Empty;
                    return;
                }
                else
                {
                    mesHistory v_mesHistory = new mesHistory();

                    v_mesHistory.UnitID = Convert.ToInt32(FGUnitID);
                    v_mesHistory.UnitStateID = Convert.ToInt32(PrintUnitStateID);
                    v_mesHistory.EmployeeID = List_Login.EmployeeID;
                    v_mesHistory.StationID = List_Login.StationID;
                    v_mesHistory.EnterTime = DateTime.Now;
                    v_mesHistory.ExitTime = DateTime.Now;
                    v_mesHistory.ProductionOrderID = Convert.ToInt32(FGProductionOrderID);
                    v_mesHistory.PartID = Convert.ToInt32(FGPartID);
                    v_mesHistory.LooperCount = 1;
                    mesHistorySVC.Insert(v_mesHistory);
                    txtSN.Text = string.Empty;
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10032", "OK", List_Login.Language, new string[] { S_SN });
                }
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void ScanUPC_PrintFGSN_Form_Load(object sender, EventArgs e)
        {
            public_.OpenBartender(List_Login.StationID.ToString());
        }
    }
}
