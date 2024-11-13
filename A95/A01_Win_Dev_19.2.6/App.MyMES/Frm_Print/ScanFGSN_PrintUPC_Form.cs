
using App.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Data.Helpers;

namespace App.MyMES
{
    public partial class ScanFGSN_PrintUPC_Form : FrmBase
    {
        DataTable DT_PrintSn;
        string UPCSNLabelTemplate;
        string CreateUPCSN;
        private List<string> apnList = new List<string>();

        public ScanFGSN_PrintUPC_Form()
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

                // 查询模板
                string S_StationTypeID = List_Login.StationTypeID.ToString();
                string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                string S_PartID = Com_Part.EditValue.ToString();
                string S_ProductionOrderID = Com_PO.EditValue.ToString();
                string S_LoginLineID = List_Login.LineID.ToString();

                UPCSNLabelTemplate = public_.GetLabelName(PartSelectSVC, S_StationTypeID, S_PartFamilyID,
                                                            S_PartID, S_ProductionOrderID, S_LoginLineID);
                if (string.IsNullOrEmpty(UPCSNLabelTemplate))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20097", "NG", List_Login.Language);
                    return;
                }

                DataSet dtSet = PartSelectSVC.GetProductionOrderDetailDef(Com_PO.EditValue.ToString());
                if (dtSet != null && dtSet.Tables.Count > 0)
                {
                    DataTable dtPara = dtSet.Tables[0];
                    DataRow[] drUPCSN = dtPara.Select("Description='IsCreateUPCSN'");

                    if (drUPCSN.Count() > 0)
                    {
                        //查找UPC生成条码格式
                        CreateUPCSN = drUPCSN[0]["Content"].ToString();
                    }
                    DataRow[] drApnRows = dtPara.Select("Description='BoxAPN'");

                    if (drApnRows.Any())
                    {
                        //查找UPC生成条码格式
                        apnList = drApnRows[0]["Content"].ToString().Trim().Split(',').ToList();
                    }
                }



                base.Btn_ConfirmPO_Click(sender, e);

                //base.Btn_Defect.Enabled = true;
                //base.Com_luUnitStatus.Enabled = true;
                SetQC();
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                return;
            }
            txtSN.Focus();
        }

        private void txtSN_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DateTime dateStart = DateTime.Now;
                    string S_SN = txtSN.Text.ToUpper().Trim();
                    string PrintSN = string.Empty;

                    if (string.IsNullOrEmpty(S_SN))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                        txtSN.Enabled = true;
                        txtSN.Focus();
                        txtSN.SelectAll();
                        return;
                    }

                    string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();
                    string S_DefectID = base.DefectChar;
                    if (S_luUnitStateID == "2" || S_luUnitStateID == "3")
                    {
                        if (S_DefectID == "")
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20049", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                            txtSN.Focus();
                            txtSN.Text = "";
                            return;
                        }
                    }

                    string xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";
                    string xmlPart = "<Part PartID=\"" + Com_Part.EditValue.ToString() + "\"> </Part>";
                    string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + Com_PO.EditValue.ToString() + "\"> </ProdOrder>";
                    string result1 = string.Empty;
                    PartSelectSVC.uspCallProcedure("uspFGLinkUPCFGSNCheck", S_SN, xmlProdOrder, xmlPart,
                        xmlStation, "", null, ref result1);
                    if (result1 != "1")
                    {

                        if (result1 == "20243")
                        {
                            string nowUnitStateID = PartSelectSVC.GetSerialNumber2(S_SN).Tables[0].Rows[0]["UnitStateID"].ToString();
                            mesUnitState mesunitSateModel = mesUnitStateSVC.Get(Convert.ToInt32(nowUnitStateID));
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20243", "NG", List_Login.Language, new string[] { S_SN, mesunitSateModel.Description });
                        }
                        else
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(result1, List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, result1);
                        }

                        txtSN.Focus();
                        txtSN.Text = "";
                        return;
                    }


                    //DataSet dts = PartSelectSVC.GetmesSerialNumber(S_SN);
                    //if (dts == null || dts.Tables.Count == 0 || dts.Tables[0].Rows.Count == 0)
                    //{
                    //    MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                    //    txtSN.SelectAll();
                    //    return;
                    //}

                    string S_ComPO = Com_PO.EditValue.ToString();
                    string S_PartID = Com_Part.EditValue.ToString();
                    string PartFamilyID = Com_PartFamily.EditValue.ToString();
                    //List<string> List_PO = public_.SnToPOID(S_SN);
                    string[] List_PO = PartSelectSVC.SnToPOID(S_SN);
                    if (List_PO.Length<=0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20014", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                        txtSN.Enabled = true;
                        txtSN.Focus();
                        txtSN.SelectAll();
                        return;
                    }

                    string S_POID = List_PO[0];

                    if (S_POID == "" || S_POID == "0")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20014", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                        txtSN.Enabled = true;
                        txtSN.Focus();
                        txtSN.SelectAll();
                        return;
                    }
                    //if (S_POID != S_ComPO)
                    //{
                    //    MessageInfo.Add_Info_MSG(Edt_MSG, "20015", "NG", List_Login.Language, new string[] { S_SN });
                    //    txtSN.Focus();
                    //    txtSN.Text = "";
                    //    return;
                    //}

                    //根据  SN  获取工单  料号
                    mesProductionOrder v_mesProductionOrder = mesProductionOrderSVC.Get(Convert.ToInt32(S_POID));
                    string S_POPartID = v_mesProductionOrder.PartID.ToString();
                    if (S_POPartID != S_PartID)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20050", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                        txtSN.Enabled = true;
                        txtSN.Focus();
                        txtSN.SelectAll();
                        return;
                    }

                    DataTable DT_SN = PartSelectSVC.GetmesSerialNumber(S_SN).Tables[0];
                    if (DT_SN == null || DT_SN.Rows.Count == 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language, new string[] { S_SN });
                        txtSN.Enabled = true;
                        txtSN.Focus();
                        txtSN.SelectAll();
                        return;
                    }

                    string S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                    DataTable DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0];

                    //校验工艺路线
                    string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, S_SN);
                    if (S_RouteCheck != "1")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(S_RouteCheck, List_Login.Language);

                        mesUnitState mesunitSateModel = mesUnitStateSVC.Get(Convert.ToInt32(DT_Unit.Rows[0]["UnitStateId"]));
                        ProMsg = string.Format(ProMsg, S_SN, mesunitSateModel.Description);

                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_RouteCheck);

                        txtSN.Enabled = true;
                        txtSN.Focus();
                        txtSN.SelectAll();
                        return;
                    }
                    //TestTime("GetRouteCheck", dateStart);
                    //string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID,
                    //    List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());
                    //if (string.IsNullOrEmpty(S_UnitStateID))
                    //{
                    //    MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                    //    txtSN.Focus();
                    //    txtSN.Text = "";
                    //    return;
                    //}

                    DataSet ds = PartSelectSVC.GetmesUnitState(S_PartID, PartFamilyID, "",
                            List_Login.LineID.ToString(), List_Login.StationTypeID, Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());

                    if (ds == null && ds.Tables.Count <= 0 && ds.Tables[0].Rows.Count <= 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                        txtSN.Enabled = true;
                        txtSN.Focus();
                        txtSN.SelectAll();
                        return;
                    }
                    string S_UnitStateID = ds.Tables[0].Rows[0]["ID"].ToString().Trim();
                    if (string.IsNullOrEmpty(S_UnitStateID))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                        txtSN.Enabled = true;
                        txtSN.Focus();
                        txtSN.SelectAll();
                        return;
                    }
                    mesUnit v_mesUnit = new mesUnit();
                    v_mesUnit.ID = Convert.ToInt32(S_UnitID);
                    v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                    v_mesUnit.StatusID = 1;
                    v_mesUnit.StationID = List_Login.StationID;
                    v_mesUnit.EmployeeID = List_Login.EmployeeID;
                    v_mesUnit.LastUpdate = DateTime.Now;
                    v_mesUnit.ProductionOrderID = Convert.ToInt32(Com_PO.EditValue.ToString());
                    v_mesUnit.PanelID = 0;
                    v_mesUnit.LineID = List_Login.LineID;
                    v_mesUnit.ProductionOrderID = Convert.ToInt32(Com_PO.EditValue.ToString());
                    v_mesUnit.RMAID = 0;
                    v_mesUnit.PartID = Convert.ToInt32(Com_Part.EditValue.ToString());
                    v_mesUnit.LooperCount = 1;
                    v_mesUnit.PartFamilyID = Convert.ToInt32(Com_PartFamily.EditValue);
                    v_mesUnit.SNFamilyID = 10;        //UPC
                    v_mesUnit.SerialNumberType = 6;   //UPC SerialNumber
                    v_mesUnit.StatusID = 1;

                    string S_Sql = "";
                    string sqlValue = "";

                    //是否生成UPC条码
                    if (CreateUPCSN == "1")
                    {
                        DataSet dsSN = new DataSet();
                        string S_PartFamilyTypeID = Com_PartFamilyType.EditValue.ToString();
                        string S_xmlPart = "'<Part PartID=" + "\"" + Com_Part.EditValue.ToString() + "\"" + "> </Part>'";
                        string S_Production0rder = "'<ProdOrder ProductionOrder=" + "\"" + Com_PO.EditValue.ToString() + "\"" + "> </ProdOrder>'";
                        string S_Station = "'<Station StationID=" + "\"" + List_Login.StationID.ToString() + "\"" + "> </Station>'";
                        string S_xmlExtraData = "'<ExtraData LineID=" + "\"" + List_Login.Line.ToString() + "\"" +
                                                     " PartFamilyTypeID=" + "\"" + S_PartFamilyTypeID + "\"" +
                                                     " SerialNumber=" + "\"" + txtSN.Text.Trim() + "\"" +
                                                     " LineType=" + "\"" + "K" + "\"" + " > </ExtraData>'";
                        //string result = PartSelectSVC.Get_CreateMesSN(SN_Pattern, List_Login, S_Production0rder, S_xmlPart, S_Station, S_xmlExtraData, v_mesUnit, 1, ref dsSN);
                        string result = PartSelectSVC.Get_CreateMesSN_New(SN_Pattern, List_Login, S_Production0rder, S_xmlPart, S_Station, S_xmlExtraData, v_mesUnit, 1, ref dsSN);
                        if (result != "1" || dsSN == null || dsSN.Tables.Count == 0)
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                            ProMsg = "SN:" + S_SN + "  " + ProMsg;
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20106", "NG", List_Login.Language, new string[] { ProMsg }, result);
                            txtSN.Enabled = true;
                            txtSN.Focus();
                            txtSN.SelectAll();
                            return;
                        }

                        PrintSN = dsSN.Tables[0].Rows[0][0].ToString();
                    }
                    else
                    {
                        PrintSN = S_SN;
                    }

                    //TestTime("Get_CreateMesSN_New", dateStart);
                    sqlValue = " declare @UnitID int select @UnitID=max(ID)+1 from mesUnit" +
                             " INSERT INTO mesUnit(ID,UnitStateID,StatusID,StationID,EmployeeID,CreationTime,LastUpdate,PanelID,LineID," +
                            "ProductionOrderID,RMAID,PartID,LooperCount) " +
                            "VALUES (@UnitID,'" + Convert.ToInt32(S_UnitStateID) + "',1,'" + List_Login.StationID + "','" + List_Login.EmployeeID + "',GETDATE(),GETDATE(),0,'" + List_Login.LineID + "'" +
                            ",'" + Convert.ToInt32(Com_PO.EditValue.ToString()) + "',0,'" + Convert.ToInt32(Com_Part.EditValue.ToString()) + "',1)" + //;SELECT @@identity

                            " INSERT INTO mesSerialNumber(UnitID,SerialNumberTypeID,Value) VALUES (@UnitID,6,'" + PrintSN + "')" +

                             "INSERT INTO mesUnitDetail(UnitID,reserved_01,reserved_02,reserved_03,reserved_04,reserved_05) VALUES " +
                            "(@UnitID,'','','','','')" +

                           " INSERT INTO mesHistory(UnitID,UnitStateID,EmployeeID,StationID,EnterTime,ExitTime,ProductionOrderID,PartID,LooperCount,StatusID)" +
                           " VALUES (@UnitID,'" + Convert.ToInt32(S_UnitStateID) + "','" + List_Login.EmployeeID + "','" + List_Login.StationID + "',GETDATE(),GETDATE(),'" + Convert.ToInt32(Com_PO.EditValue.ToString()) + "','" + Convert.ToInt32(Com_Part.EditValue.ToString()) + "',1,1)" +

                            " UPDATE mesUnit SET UnitStateID='" + Convert.ToInt32(S_UnitStateID) + "',StatusID=1,StationID='" + List_Login.StationID + "'," +
                            "LastUpdate=GETDATE(),ProductionOrderID='" + Convert.ToInt32(Com_PO.EditValue.ToString()) + "' WHERE ID=" + Convert.ToInt32(S_UnitID) +

                            " UPDATE mesUnitDetail SET KitSerialNumber='" + PrintSN + "' WHERE UnitID=" + Convert.ToInt32(S_UnitID) +

                           " INSERT INTO mesHistory(UnitID,UnitStateID,EmployeeID,StationID,EnterTime,ExitTime,ProductionOrderID,PartID,LooperCount,StatusID)" +
                           " VALUES (" + Convert.ToInt32(v_mesUnit.ID) + ",'" + Convert.ToInt32(S_UnitStateID) + "','" + List_Login.EmployeeID + "','" + List_Login.StationID + "',GETDATE(),GETDATE(),'" + Convert.ToInt32(Com_PO.EditValue.ToString()) + "','" + Convert.ToInt32(S_POPartID) + "',1,1)";


                    List<mesUnitDefect> List_mesUnitDefect = null;
                    string[] Array_Defect = S_DefectID.Split(';');
                    if (S_luUnitStateID != "1")
                    {
                        List_mesUnitDefect = new List<mesUnitDefect>();
                        foreach (var item in Array_Defect)
                        {
                            try
                            {
                                if (item.Trim() != "")
                                {
                                    int I_DefectID = Convert.ToInt32(item);
                                    mesUnitDefect v_mesUnitDefect = new mesUnitDefect();

                                    v_mesUnitDefect.UnitID = v_mesUnit.ID;
                                    v_mesUnitDefect.DefectID = I_DefectID;
                                    v_mesUnitDefect.StationID = List_Login.StationID;
                                    v_mesUnitDefect.EmployeeID = List_Login.EmployeeID;

                                    List_mesUnitDefect.Add(v_mesUnitDefect);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                            }
                        }

                        
                        if (List_mesUnitDefect != null)
                        {
                            if (List_mesUnitDefect.Count > 0)
                            {
                                S_Sql += "declare @MaxDefID int" + "\r\n";
                            }
                            int I_Qyt = 1;


                            for (int i = 0; i < List_mesUnitDefect.Count; i++)
                            {
                                mesUnitDefect v_mesUnitDefect = new mesUnitDefect();
                                v_mesUnitDefect = List_mesUnitDefect[i];
                                I_Qyt = i + 1;

                                S_Sql +=
                                        "select @MaxDefID=ISNULL(Max(ID),0)+" + I_Qyt + " from mesUnitDefect " + "\r\n" +
                                        "INSERT INTO mesUnitDefect(ID, UnitID, DefectID, StationID, EmployeeID) Values(" + "\r\n" +
                                        "@MaxDefID," + "\r\n" +
                                        "'" + v_mesUnitDefect.UnitID + "'," + "\r\n" +
                                        "'" + v_mesUnitDefect.DefectID + "'," + "\r\n" +
                                        "'" + v_mesUnitDefect.StationID + "'," + "\r\n" +
                                        "'" + v_mesUnitDefect.EmployeeID + "'" + "\r\n" +
                                        ")" + "\r\n";
                            }
                        }
                    }

                    sqlValue = sqlValue + "\r\n" + S_Sql;
                    string ReturnValue = PartSelectSVC.ExecSql(sqlValue);
                    //TestTime("Insert", dateStart);
                    if (ReturnValue != "OK")
                    {
                        ReturnValue = "SN:" + S_SN + "  " + ReturnValue;
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ReturnValue });
                        txtSN.Enabled = true;
                        txtSN.Focus();
                        txtSN.SelectAll();
                        return;
                    }

                    if (ReturnValue != "OK")
                    {
                        for (int i = 0; i < 10 && ReturnValue.Contains("PRIMARY KEY") && ReturnValue.Contains("Unit_PK"); i++)
                        {
                            ReturnValue = PartSelectSVC.ExecSql(sqlValue);
                        }
                        if (ReturnValue != "OK")
                        {
                            //MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ReturnValue });
                            ReturnValue = "SN:" + S_SN + "  " + ReturnValue;
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ReturnValue });
                            txtSN.Enabled = true;
                            txtSN.Focus();
                            txtSN.SelectAll();
                            return;
                        }
                    }


                    //string S_UpdateUnit = mesUnitSVC.Update(v_mesUnit);
                    //if (S_UpdateUnit.Substring(0, 1) == "E")
                    //{
                    //    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_UpdateUnit });
                    //    return;
                    //}

                    //mesUnitDetailSVC.UpdateKitSerialnumber(Convert.ToInt32(S_UnitID), PrintSN);

                    //mesHistory v_mesHistory = new mesHistory();
                    //v_mesHistory.UnitID = Convert.ToInt32(v_mesUnit.ID);
                    //v_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                    //v_mesHistory.EmployeeID = List_Login.EmployeeID;
                    //v_mesHistory.StationID = List_Login.StationID;
                    //v_mesHistory.EnterTime = DateTime.Now;
                    //v_mesHistory.ExitTime = DateTime.Now;
                    //v_mesHistory.ProductionOrderID = Convert.ToInt32(Com_PO.EditValue.ToString());
                    //v_mesHistory.PartID = Convert.ToInt32(S_POPartID);
                    //v_mesHistory.LooperCount = 1;
                    //int I_InsertHistory = mesHistorySVC.Insert(v_mesHistory);

                    if (!string.IsNullOrEmpty(UPCSNLabelTemplate))
                    {

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


                        DR["SN"] = PrintSN;
                        DR["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        DT_PrintSn.Rows.Add(DR);

                        string PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, UPCSNLabelTemplate,
                        DT_PrintSn, Convert.ToInt32(Com_Part.EditValue.ToString()));
                        if (PrintResult != "OK")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(PrintResult, List_Login.Language);
                            ProMsg = "-> SN:" + S_SN + "  " + ProMsg;

                            MessageInfo.Add_Info_MSG(Edt_MSG, "20111", "NG", List_Login.Language, new string[] { ProMsg }, " -- : "+ PrintResult);
                            txtSN.Enabled = true;
                            txtSN.Focus();
                            txtSN.SelectAll();
                            return;
                        }
                        else
                        {
                            Thread.Sleep(200);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "10021", "OK", List_Login.Language, new string[] { "-- SN:" + S_SN });
                        }
                    }

                    TimeSpan ts = DateTime.Now - dateStart;
                    double mill = Math.Round(ts.TotalMilliseconds, 0);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { S_SN, mill.ToString() });
                    txtSN.Enabled = true;
                    txtSN.Text = string.Empty;
                    txtSN.Focus();
                    txtSN.SelectAll();

                }
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                txtSN.Enabled = true;
                txtSN.Focus();
                txtSN.SelectAll();

            }
        }

        private void ScanFGSN_PrintUPC_Form_Load(object sender, EventArgs e)
        {
            public_.OpenBartender(List_Login.StationID.ToString());
        }


        private void TestTime(string SS, DateTime dateStart)
        {
            TimeSpan ts = DateTime.Now - dateStart;
            double mill = Math.Round(ts.TotalMilliseconds, 0);


            MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { SS, mill.ToString() });
        }



        public override void Com_luUnitStatus_EditValueChanged(object sender, EventArgs e)
        {
            base.Com_luUnitStatus_EditValueChanged(sender, e);
            string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();
            switch (S_luUnitStateID)
            {
                case "1":
                    //lblDefectCode.ForeColor = Color.Green;
                    lblDefectCode.Visible = false;
                    txtSN.BackColor = Com_luUnitStatus.ForeColor;
                    break;
                case "2":
                    lblDefectCode.ForeColor = Color.Red;
                    lblDefectCode.Visible = true;
                    txtSN.BackColor = Com_luUnitStatus.ForeColor;
                    break;
                case "3":
                    lblDefectCode.ForeColor = Color.Orange;
                    lblDefectCode.Visible = true;
                    txtSN.BackColor = Com_luUnitStatus.ForeColor;
                    break;
                default:
                    lblDefectCode.Visible = false;
                    txtSN.BackColor = Color.White;
                    break;
            }
        }


        public override void btnDefectSave_Click(object sender, EventArgs e)
        {
            base.btnDefectSave_Click(sender, e);
            lblDefectCode.Text = "DefectCode:" + DefectCode;
        }
    }
}
