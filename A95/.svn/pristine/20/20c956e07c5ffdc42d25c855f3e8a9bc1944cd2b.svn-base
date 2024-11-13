using App.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.MyMES
{
    public partial class ScanFGSN_PrintUPC_Auto_Form : FrmBase
    {
        DataTable DT_PrintSn;
        string UPCSNLabelTemplate;
        string CreateUPCSN;

        #region add by Jim.Zhou,2021.8.31,Memo:串口通信

        private string strPort = "COM1";
        string IsAuto = "0";
        string PassCmd = "PASS";

        string FailCmd = "FAIL";
        string ReadCmd = "READ";
        #endregion

        public ScanFGSN_PrintUPC_Auto_Form()
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
                }

                #region add by Jim.Zhou,2021.8.31,Memo:串口通信
                DataSet dsStationDetail = PartSelectSVC.GetPLCSeting("", List_Login.StationID.ToString());
                DataRow[] drAutoStation = dsStationDetail.Tables[0].Select("Name='AutoStation'");
                if (drAutoStation.Length > 0) IsAuto = drAutoStation[0]["Value"].ToString(); else IsAuto = "0";

                DataRow[] drPASS = dsStationDetail.Tables[0].Select("Name='PASS'");
                if (drPASS.Length > 0) PassCmd = drPASS[0]["Value"].ToString(); else PassCmd = "PASS";

                DataRow[] drFAIL = dsStationDetail.Tables[0].Select("Name='FAIL'");
                if (drFAIL.Length > 0) FailCmd = drFAIL[0]["Value"].ToString(); else FailCmd = "FAIL";

                DataRow[] drREAD = dsStationDetail.Tables[0].Select("Name='READ'");
                if (drREAD.Length > 0) ReadCmd = drREAD[0]["Value"].ToString(); else ReadCmd = "READ";

                DataRow[] drPort = dsStationDetail.Tables[0].Select("Name='Port'");
                if (drPort.Length > 0) strPort = drPort[0]["Value"].ToString(); else strPort = "COM1";
                if (IsAuto == "1")
                {
                    if (port1 == null)
                    {
                        port1 = new SerialPort(strPort);
                        port1.BaudRate = 9600;
                        port1.DataBits = 8;
                        port1.Parity = Parity.Odd;
                        port1.StopBits = StopBits.One;
                    }

                    try
                    {
                        if (port1.IsOpen)
                        {
                            port1.Close();
                        }
                        port1.Open();
                    }
                    catch (Exception ex)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "60104", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                        return;
                    }
                }
                #endregion

                base.Btn_ConfirmPO_Click(sender, e);
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
                    txtSN.Enabled = false;
                    if (string.IsNullOrEmpty(S_SN))
                    {
                        #region add by Jim.Zhou,2021.8.31,Memo:串口通信
                        if (IsAuto == "1")
                        {
                            try
                            {

                                port1.WriteLine(FailCmd);
                                MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
                            }
                            catch (Exception ex)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                //return;
                            }
                        }
                        #endregion

                        MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                        txtSN.Enabled = true;
                        txtSN.Focus();
                        txtSN.SelectAll();
                        return;
                    }

                    DateTime lastDateTime = DateTime.Now;
                    string S_ComPO = Com_PO.EditValue.ToString();
                    string S_PartID = Com_Part.EditValue.ToString();
                    string PartFamilyID = Com_PartFamily.EditValue.ToString();

                    DataSet dataSetInfo = public_.GetAndCheckUnitInfoUPC(S_SN,S_PartID,base.PartSelectSVC);
                    if (dataSetInfo == null || dataSetInfo.Tables.Count <= 0 || dataSetInfo.Tables[0].Rows.Count <= 0)
                    {
                        #region add by Jim.Zhou,2021.8.31,Memo:串口通信
                        if (IsAuto == "1")
                        {
                            try
                            {

                                port1.WriteLine(FailCmd);
                                MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
                            }
                            catch (Exception ex)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                //return;
                            }
                        }
                        #endregion

                        MessageInfo.Add_Info_MSG(Edt_MSG, "20050", "NG", List_Login.Language);
                        txtSN.Enabled = true;
                        txtSN.Focus();
                        txtSN.SelectAll();
                        return;
                    }

                    
                    DataTable DT_Unit = dataSetInfo.Tables[0];
                    string S_UnitID = DT_Unit.Rows[0]["ID"].ToString();
                    public_.ShowTimeSpan("GetAndCheckUnitInfo", ref lastDateTime);
                    //校验工艺路线
                    string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, S_SN);
                    if (S_RouteCheck != "1")
                    {
                        #region add by Jim.Zhou,2021.8.31,Memo:串口通信
                        if (IsAuto == "1")
                        {
                            try
                            {

                                port1.WriteLine(FailCmd);
                                MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
                            }
                            catch (Exception ex)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                //return;
                            }
                        }
                        #endregion

                        string ProMsg = MessageInfo.GetMsgByCode(S_RouteCheck, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_RouteCheck);
                        txtSN.Enabled = true;
                        txtSN.Focus();
                        txtSN.SelectAll();
                        return;
                    }
                    public_.ShowTimeSpan("GetRouteCheck", ref lastDateTime);
                    DataSet ds = PartSelectSVC.GetmesUnitState(S_PartID, PartFamilyID, "",  List_Login.LineID.ToString(), List_Login.StationTypeID, Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());
                    public_.ShowTimeSpan("GetmesUnitState", ref lastDateTime);

                    if (ds == null && ds.Tables.Count <= 0 && ds.Tables[0].Rows.Count <= 0)
                    {
                        #region add by Jim.Zhou,2021.8.31,Memo:串口通信
                        if (IsAuto == "1")
                        {
                            try
                            {

                                port1.WriteLine(FailCmd);
                                MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
                            }
                            catch (Exception ex)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                //return;
                            }
                        }
                        #endregion
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                        txtSN.Enabled = true;
                        txtSN.Focus();
                        txtSN.SelectAll();
                        return;
                    }
                    string S_UnitStateID = ds.Tables[0].Rows[0]["ID"].ToString().Trim();
                    if (string.IsNullOrEmpty(S_UnitStateID))
                    {
                        #region add by Jim.Zhou,2021.8.31,Memo:串口通信
                        if (IsAuto == "1")
                        {
                            try
                            {

                                port1.WriteLine(FailCmd);
                                MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
                            }
                            catch (Exception ex)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                //return;
                            }
                        }
                        #endregion

                        MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
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
                            #region add by Jim.Zhou,2021.8.31,Memo:串口通信
                            if (IsAuto == "1")
                            {
                                try
                                {

                                    port1.WriteLine(FailCmd);
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
                                }
                                catch (Exception ex)
                                {
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                    //return;
                                }
                            }
                            #endregion

                            string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
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
                    public_.ShowTimeSpan("Get_CreateMesSN_New", ref lastDateTime);
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
                           " VALUES (" + Convert.ToInt32(v_mesUnit.ID) + ",'" + Convert.ToInt32(S_UnitStateID) + "','" + List_Login.EmployeeID + "','" + List_Login.StationID + "',GETDATE(),GETDATE(),'" + Convert.ToInt32(Com_PO.EditValue.ToString()) + "','" + Convert.ToInt32(S_PartID) + "',1,1)";

                    string ReturnValue = PartSelectSVC.ExecSql(sqlValue);
                    public_.ShowTimeSpan("end sql", ref lastDateTime);
                    if (ReturnValue != "OK")
                    {
                        #region add by Jim.Zhou,2021.8.31,Memo:串口通信
                        if (IsAuto == "1")
                        {
                            try
                            {

                                port1.WriteLine(FailCmd);
                                MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
                            }
                            catch (Exception ex)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                //return;
                            }
                        }
                        #endregion

                        txtSN.Enabled = true;
                        txtSN.Focus();
                        txtSN.SelectAll();
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ReturnValue });
                        return;
                    }


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
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20111", "NG", List_Login.Language, new string[] { ProMsg }, PrintResult);
                            txtSN.Enabled = true;
                            txtSN.Focus();
                            txtSN.SelectAll();
                            return;
                        }
                        else
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "10021", "OK", List_Login.Language);
                        }
                    }
                    public_.ShowTimeSpan("print label", ref lastDateTime);
                    #region add by Jim.Zhou,2021.8.31,Memo:串口通信
                    if (IsAuto == "1")
                    {
                        try
                        {
                            port1.WriteLine(PassCmd);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60101", "OK", List_Login.Language);
                        }
                        catch (Exception ex)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60105", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                            //return;
                        }
                    }
                    #endregion
                    public_.ShowTimeSpan("send command", ref lastDateTime);

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
                #region add by Jim.Zhou,2021.8.31,Memo:串口通信
                if (IsAuto == "1")
                {
                    try
                    {
                        port1.WriteLine(FailCmd);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "60101", "OK", List_Login.Language);
                    }
                    catch
                    { }
                }
                #endregion
                txtSN.Enabled = true;
                txtSN.Text = string.Empty;
                txtSN.Focus();
                txtSN.SelectAll();
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void ScanFGSN_PrintUPC_Auto_Form_Load(object sender, EventArgs e)
        {
            public_.OpenBartender(List_Login.StationID.ToString());
        }


        private void TestTime(string SS, DateTime dateStart)
        {
            TimeSpan ts = DateTime.Now - dateStart;
            double mill = Math.Round(ts.TotalMilliseconds, 0);


            MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { SS, mill.ToString() });
        }

    }
}
