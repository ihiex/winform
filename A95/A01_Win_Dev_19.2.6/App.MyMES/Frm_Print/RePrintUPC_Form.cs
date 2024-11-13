using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.Model;

namespace App.MyMES
{
    public partial class RePrintUPC_Form : FrmBase
    {
        DataTable DT_PrintSn;
        string[] RePrintStationID;
        string UPCSNLabelTemplate;
        string PrintUnitStateID;
        string PrintStationTypeID;

        string S_IsCheckPO = "1";
        string S_IsCheckPN = "1";
        public RePrintUPC_Form()
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
            DataTable dtStationDetail = public_.GetmesStationConfig("PrintUPCUnitStateID", List_Login.StationID.ToString());
            if(dtStationDetail == null || dtStationDetail.Rows.Count==0)
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

            DataTable dtPrintStationTypeID = public_.GetmesStationConfig("PrintStationTypeID", List_Login.StationID.ToString());
            if (dtPrintStationTypeID == null || dtPrintStationTypeID.Rows.Count == 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20199", "NG", List_Login.Language, new string[] { "PrintStationTypeID" });
                return;
            }
            PrintStationTypeID = dtPrintStationTypeID.Rows[0]["Value"].ToString();


            DataTable dtIsCheckPO = public_.GetmesStationConfig("IsCheckPO", List_Login.StationID.ToString());
            if (dtIsCheckPO != null && dtIsCheckPO.Rows.Count > 0)
            {
                S_IsCheckPO = dtIsCheckPO.Rows[0]["Value"].ToString();
            }

            DataTable dtIsCheckPN = public_.GetmesStationConfig("IsCheckPN", List_Login.StationID.ToString());
            if (dtIsCheckPN != null && dtIsCheckPN.Rows.Count > 0)
            {
                S_IsCheckPN = dtIsCheckPN.Rows[0]["Value"].ToString();
            }

            // 查询模板
            string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
            string S_PartID = Com_Part.EditValue.ToString();
            string S_ProductionOrderID = Com_PO.EditValue.ToString();
            string S_LoginLineID = List_Login.LineID.ToString();

            UPCSNLabelTemplate = public_.GetLabelName(PartSelectSVC, PrintStationTypeID, S_PartFamilyID,
                                                        S_PartID, S_ProductionOrderID, S_LoginLineID);

            if (string.IsNullOrEmpty(UPCSNLabelTemplate))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20097", "NG", List_Login.Language);
                return;
            }
            base.Btn_ConfirmPO_Click(sender, e);
        }

        private void txtSN_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode != Keys.Enter)
                    return;

                string S_SN = txtSN.Text.Trim();
                string UPCSN = string.Empty;
                string UPCID = string.Empty;
                string UPCProductionOrderID = string.Empty;
                string UPCPartID = string.Empty;
                
                if (string.IsNullOrEmpty(S_SN))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                    txtSN.Focus();
                    return;
                }

                DataSet dts = PartSelectSVC.GetmesSerialNumber(S_SN);
                if (dts == null || dts.Tables.Count == 0 || dts.Tables[0].Rows.Count == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language, new string[] { S_SN });
                    txtSN.Focus();
                    return;
                }

                string SerialNumberType = dts.Tables[0].Rows[0]["SerialNumberTypeID"].ToString();

                //校验是否满足unitstate状态
                if (SerialNumberType == "6")
                {
                    //转换FG条码检测
                    string FGSN = mesUnitDetailSVC.MesGetFGSNByUPCSN(S_SN);
                    if(string.IsNullOrEmpty(FGSN))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20189", "NG", List_Login.Language);
                        txtSN.Text = string.Empty;
                        return;
                    }

                    string UPCUnitID = dts.Tables[0].Rows[0]["UnitID"].ToString();
                    mesUnit UPCmesUnit = mesUnitSVC.Get(Convert.ToInt32(UPCUnitID));
                    dts = PartSelectSVC.GetmesSerialNumber(FGSN);
                    UPCSN = S_SN;
                    UPCID = UPCUnitID;
                    UPCProductionOrderID = UPCmesUnit.ProductionOrderID.ToString();
                    UPCPartID = UPCmesUnit.PartID.ToString();
                }
                else
                {
                    //查询UPC条码
                    string FGUnitID = dts.Tables[0].Rows[0]["UnitID"].ToString();
                    mesUnitDetail mesUnitDetail = mesUnitDetailSVC.GetUnitDetail(Convert.ToInt32(FGUnitID));
                    UPCSN = mesUnitDetail.KitSerialNumber;
                    if (string.IsNullOrEmpty(UPCSN))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20189", "NG", List_Login.Language);
                        txtSN.Text = string.Empty;
                        return;
                    }
                    DataSet dsUPC = PartSelectSVC.GetmesSerialNumber(UPCSN);
                    UPCID = dsUPC.Tables[0].Rows[0]["UnitID"].ToString();
                    mesUnit mesUnitUPC = mesUnitSVC.Get(Convert.ToInt32(UPCID));
                    UPCProductionOrderID = mesUnitUPC.ProductionOrderID.ToString();
                    UPCPartID = mesUnitUPC.PartID.ToString();
                }


                if (S_IsCheckPN == "1")
                {
                    //校验料号是否一致
                    if (UPCPartID != Com_Part.EditValue.ToString())
                    {
                        string ProMsg = MessageInfo.GetMsgByCode("20083", List_Login.Language);
                        ProMsg = "SN:[" + S_SN + "] " + ProMsg;
                        MessageInfo.Add_Info_MSG(Edt_MSG, ProMsg, "NG", List_Login.Language, null, "20083");
                        txtSN.Text = string.Empty;
                        return;
                    }
                }

                if (S_IsCheckPO == "1")
                {
                    //校验工单是否一致
                    if (UPCProductionOrderID != Com_PO.EditValue.ToString())
                    {
                        //max.xie 
                        string ProMsg = MessageInfo.GetMsgByCode("20084", List_Login.Language);
                        ProMsg = "SN:[" + S_SN + "] " + ProMsg;
                        //MessageInfo.Add_Info_MSG(Edt_MSG, "20084", "NG", List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, ProMsg, "NG", List_Login.Language, null, "20084");
                        txtSN.Text = string.Empty;
                        return;
                    }
                }



                string UnitID = dts.Tables[0].Rows[0]["UnitID"].ToString();
                mesUnit mesUnit = mesUnitSVC.Get(Convert.ToInt32(UnitID));
                string unitStateID = mesUnit.UnitStateID.ToString();
                if (!RePrintStationID.Contains(unitStateID))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20045", "NG", List_Login.Language);
                    txtSN.Text = string.Empty;
                    return;
                }

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
                DR["SN"] = UPCSN;
                DR["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DT_PrintSn.Rows.Add(DR);

                string PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, UPCSNLabelTemplate,
                DT_PrintSn, Convert.ToInt32(Com_Part.EditValue.ToString()));
                if (PrintResult != "OK")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(PrintResult, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20111", "NG", List_Login.Language, new string[] { ProMsg }, "SN:["+ S_SN +"] "+ PrintResult);
                    txtSN.Text = string.Empty;
                    return;
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10032", "OK", List_Login.Language, new string[] { S_SN });
                    mesHistory v_mesHistory = new mesHistory();

                    v_mesHistory.UnitID = Convert.ToInt32(UPCID);
                    v_mesHistory.UnitStateID = Convert.ToInt32(PrintUnitStateID);
                    v_mesHistory.EmployeeID = List_Login.EmployeeID;
                    v_mesHistory.StationID = List_Login.StationID;
                    v_mesHistory.EnterTime = DateTime.Now;
                    v_mesHistory.ExitTime = DateTime.Now;
                    v_mesHistory.ProductionOrderID = Convert.ToInt32(UPCProductionOrderID);
                    v_mesHistory.PartID = Convert.ToInt32(UPCPartID);
                    v_mesHistory.LooperCount = 1;
                    v_mesHistory.StatusID = 1;
                    int I_InsertHistory = mesHistorySVC.Insert(v_mesHistory);

                    txtSN.Text = string.Empty;
                }
            }
            catch(Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { "SN:[" + txtSN.Text.Trim() + "] " + ex.Message.ToString() });
            }
        }

        private void RePrintUPC_Form_Load(object sender, EventArgs e)
        {
            public_.OpenBartender(List_Login.StationID.ToString());
        }
    }
}
