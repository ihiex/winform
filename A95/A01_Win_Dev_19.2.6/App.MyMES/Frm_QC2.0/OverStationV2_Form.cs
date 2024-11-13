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
using System.IO.Ports;

namespace App.MyMES
{
    public partial class OverStationV2_Form : FrmBase
    {
        string S_UnitID = "";
        bool COF = false;
        string IsAuto = "0";

        string S_IsCheckPO = "1";
        string S_IsCheckPN = "1";


        private string strPort = "COM1";
        string ReadCmd = "READY";
        string TestSN = "TESTSHELL001";

        string S_ProductionOrderID = "";
        DataSet dsBOM = null;
        DataSet DS_StationTypeDef;
        bool isEnableCOF = false;

        public OverStationV2_Form()
        {
            InitializeComponent();
        }

        private DataSet GetStationData()
        {
            DataSet dsStationDetail = PartSelectSVC.GetPLCSeting("", List_Login.StationID.ToString());
            DS_StationTypeDef = PartSelectSVC.MESGetStationTypeParameter(List_Login.StationTypeID);

            DataRow[] drREAD = dsStationDetail.Tables[0].Select("Name='READY'");
            if (drREAD.Length > 0) ReadCmd = drREAD[0]["Value"].ToString(); else ReadCmd = "READY";

            DataRow[] drPort = dsStationDetail.Tables[0].Select("Name='Port'");
            if (drPort.Length > 0) strPort = drPort[0]["Value"].ToString(); else strPort = "COM1";

            DataRow[] drAutoStation = dsStationDetail.Tables[0].Select("Name='AutoStation'");
            if (drAutoStation.Length > 0) IsAuto = drAutoStation[0]["Value"].ToString(); else IsAuto = "0";


            DataRow[] DR_IsCheckPO = DS_StationTypeDef.Tables[0].Select("Description='IsCheckPO'");
            if (DR_IsCheckPO.Length > 0)
            {
                S_IsCheckPO = DR_IsCheckPO[0]["Content"].ToString();

                if (S_IsCheckPO == "1")
                {
                    DataRow[] DR_Child_IsCheckPO = dsStationDetail.Tables[0].Select("Name='IsCheckPO'");
                    if (DR_Child_IsCheckPO.Length > 0) S_IsCheckPO = DR_Child_IsCheckPO[0]["Value"].ToString(); else S_IsCheckPO = "1";
                }
            }
            else
            {
                S_IsCheckPO = "1";
            }

            DataRow[] DR_IsCheckPN = DS_StationTypeDef.Tables[0].Select("Description='IsCheckPN'");
            if (DR_IsCheckPN.Length > 0)
            {
                S_IsCheckPN = DR_IsCheckPN[0]["Content"].ToString();

                if (S_IsCheckPN == "1")
                {
                    DataRow[] DR_Child_IsCheckPN = dsStationDetail.Tables[0].Select("Name='IsCheckPN'");
                    if (DR_Child_IsCheckPN.Length > 0) S_IsCheckPN = DR_Child_IsCheckPN[0]["Value"].ToString(); else S_IsCheckPN = "1";
                }
            }
            else
            {
                S_IsCheckPN = "1";
            }


            if (S_IsCheckPO == "1")
            {
                lblProductionOrder.Visible = true;
                lblUnitState.Visible = true;
                lblUnitState.Location = new Point(58, 135);

                panel12.Visible = true;
            }
            else
            {
                lblProductionOrder.Visible = false;
                lblUnitState.Visible = true;

                lblUnitState.Location = new Point(58, 102);
                panel12.Visible = false;
            }

            return dsStationDetail;
        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            Edt_SN.Enabled = false;
            base.Btn_Defect.Visible = false;

            GetStationData();

            if (S_IsCheckPN == "1")
            {
                GrpControlPart.Visible = true;
            }
            else
            {
                GrpControlPart.Visible = false;

                //dsBOM = PartSelectSVC.MESGetBomPartInfo(Convert.ToInt32(Com_Part.EditValue.ToString()),
                //                   List_Login.StationTypeID);
            }
            if (S_IsCheckPO == "0" && S_IsCheckPN == "0")
            {
                Edt_SN.Text = "";
                Edt_SN.Enabled = true;
                Edt_SN.Focus();
            }
        }

        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            DataSet dsStationDetail = GetStationData();

            DataRow[] drTestSN = dsStationDetail.Tables[0].Select("Name='TestSN'");
            if (drTestSN.Length > 0) TestSN = drTestSN[0]["Value"].ToString(); else TestSN = "TESTSHELL001";

            if (IsAuto == "1")
            {
                port1 = new SerialPort(strPort);
                port1.BaudRate = 9600;
                port1.DataBits = 8;
                port1.Parity = Parity.None;
                port1.StopBits = StopBits.One;

                try
                {
                    if (!port1.IsOpen)
                    {
                        port1.Open();
                    }
                }
                catch (Exception ex)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "60104", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                    return;
                }
            }


            base.Btn_ConfirmPO_Click(sender, e);
            Edt_SN.Enabled = true;
            Edt_SN.Focus();

            if (isEnableCOF && S_IsCheckPO == "1")
            {
                string PO = Com_PO.EditValue.ToString();
                if (!string.IsNullOrEmpty(PO))
                {
                    DataSet dsCOF = PartSelectSVC.GetluPODetailDef(Convert.ToInt32(PO), "COF");
                    if (dsCOF != null && dsCOF.Tables.Count > 0 && dsCOF.Tables[0].Rows.Count > 0)
                    {
                        COF = dsCOF.Tables[0].Rows[0]["Content"].ToString() == "1";
                    }
                }
            }
        }

        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                List<mesUnit> List_mesUnit = new List<mesUnit>();
                List<mesHistory> List_mesHistory = new List<mesHistory>();

                string S_SN = Edt_SN.Text.Trim();
                DateTime dateStart = DateTime.Now;

                if (S_SN == "")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }

                string mPartId = null;
                string mPoId = null;
                string mPartFamilyId = Com_PartFamily.EditValue.ToString();
                string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();
                string S_DefectID = base.DefectChar;
                if (S_luUnitStateID == "2" || S_luUnitStateID == "3")
                {
                    if (S_DefectID == "")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20049", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }
                }

                if (S_IsCheckPN == "1")
                {
                    mPartId = Com_Part.EditValue.ToString();
                    if (S_IsCheckPO == "1")
                    {
                        mPoId = Com_PO.EditValue.ToString();
                    }
                }
                string outString1 = string.Empty;
                if (string.IsNullOrEmpty(mPartId) || string.IsNullOrEmpty(mPoId))
                {
                    DataSet dsMainSN;
                    dsMainSN = PartSelectSVC.uspCallProcedure("uspGetBaseData", S_SN,
                           null, null, null, null, null, ref outString1);
                    if (outString1 != "1" || dsMainSN == null || dsMainSN.Tables[0].Rows.Count <= 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20119", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }

                    DataTable dt = dsMainSN.Tables[0];
                    mPoId = string.IsNullOrEmpty(mPoId) ? dt.Rows[0]["ProductionOrderID"].ToString() : mPoId;
                    mPartId = string.IsNullOrEmpty(mPartId) ? dt.Rows[0]["PartID"].ToString() : mPartId;
                    mPartFamilyId = dt.Rows[0]["PartFamilyID"].ToString();
                }
                if (isEnableCOF)
                {
                    DataSet dsCOF = PartSelectSVC.GetluPODetailDef(Convert.ToInt32(mPoId), "COF");
                    if (dsCOF != null && dsCOF.Tables.Count > 0 && dsCOF.Tables[0].Rows.Count > 0)
                    {
                        COF = dsCOF.Tables[0].Rows[0]["Content"].ToString() == "1";
                    }
                }
                DataSet unitDataset = PartSelectSVC.GetAndCheckUnitInfo(S_SN, mPoId, mPartId);
                if (unitDataset == null || unitDataset.Tables.Count <= 0 || unitDataset.Tables[0].Rows.Count <= 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20015", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }
                DataTable DT_Unit = unitDataset.Tables[0];
                //if (!isEnableCOF || !COF)
                //{
                //    //根据工艺路线判断状态
                //    string StatusID = DT_Unit.Rows[0]["StatusID"].ToString();
                //    if (StatusID != "1")
                //    {
                //        MessageInfo.Add_Info_MSG(Edt_MSG, "20036", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                //        Edt_SN.Focus();
                //        Edt_SN.Text = "";
                //        return;
                //    }
                //}
                if (IsAuto == "1")
                {
                    try
                    {
                        port1.Write(ReadCmd);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "60108", "OK", List_Login.Language);
                    }
                    catch (Exception ex)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "60109", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                    }

                }
                string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, S_SN);
                if (S_RouteCheck == "1")
                {
                    try
                    {
                        //调用通用过程
                        string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + mPoId + "\"> </ProdOrder>";
                        string xmlPart = "<Part PartID=\"" + mPartId + "\"> </Part>";
                        string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                        string outString = string.Empty;
                        PartSelectSVC.uspCallProcedure("uspQCCheck", S_SN,
                                                                                xmlProdOrder, xmlPart, xmlStation, null, S_luUnitStateID, ref outString);
                        if (outString != "1")
                        {
                            sendSignal(false);
                            string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, outString);
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                            return;
                        }
                        //////////////////////////
                        string S_UnitStateID = string.Empty;
                        DataSet tmpDataSet = PartSelectSVC.GetmesUnitState(mPartId, mPartFamilyId, "", List_Login.LineID.ToString(), List_Login.StationTypeID, mPoId, Com_luUnitStatus.EditValue.ToString());
                        if (tmpDataSet != null && tmpDataSet.Tables.Count != 0 && tmpDataSet.Tables[0].Rows.Count != 0)
                        {
                            S_UnitStateID = tmpDataSet.Tables[0].Rows[0]["ID"].ToString().Trim();
                        }
                        if (string.IsNullOrEmpty(S_UnitStateID))
                        {
                            sendSignal(false);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                            return;
                        }
                        /////////////////////////

                        //DataTable DT_UPUnitID = PartSelectSVC.Get_UnitID(S_SN).Tables[0];

                        mesUnit v_mesUnit = new mesUnit();
                        v_mesUnit.ID = Convert.ToInt32(DT_Unit.Rows[0]["ID"].ToString());
                        v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                        v_mesUnit.StatusID = 1;
                        v_mesUnit.StationID = List_Login.StationID;
                        v_mesUnit.EmployeeID = List_Login.EmployeeID;
                        v_mesUnit.LastUpdate = DateTime.Now;
                        v_mesUnit.ProductionOrderID = Convert.ToInt32(mPoId);
                        //修改 Unit
                        List_mesUnit.Add(v_mesUnit);

                        mesHistory v_mesHistory = new mesHistory();
                        v_mesHistory.UnitID = Convert.ToInt32(v_mesUnit.ID);
                        v_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                        v_mesHistory.EmployeeID = List_Login.EmployeeID;
                        v_mesHistory.StationID = List_Login.StationID;
                        v_mesHistory.ProductionOrderID = Convert.ToInt32(mPoId);
                        v_mesHistory.PartID = Convert.ToInt32(mPartId);
                        v_mesHistory.LooperCount = 1;
                        v_mesHistory.StatusID = 1;
                        //插入 mesHistory
                        List_mesHistory.Add(v_mesHistory);

                        mesUnit[] L_mesUnit = List_mesUnit.ToArray();
                        mesHistory[] L_mesHistory = List_mesHistory.ToArray();

                        string ReturnValue = DataCommitSVC.SubmitDataUH(L_mesUnit, L_mesHistory);
                        if (ReturnValue != "OK")
                        {
                            sendSignal(false);
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
                        sendSignal(true);
                    }
                    catch (Exception ex)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                    }
                }
                else
                {
                    sendSignal(false);
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
        }

        private void sendSignal(bool type)
        {
            if (IsAuto == "1")
            {
                try
                {
                    port1.Write(type?"PASS": "FAIL");
                    MessageInfo.Add_Info_MSG(Edt_MSG, "60108", "OK", List_Login.Language);
                }
                catch (Exception ex)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "60109", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                }

            }
        }

        private void OverStationV2_Form_Load(object sender, EventArgs e)
        {
            base.FrmBase_Load(sender, e);

            if (S_IsCheckPN == "0")
            {
                GrpControlInputData.Enabled = true;

                Edt_SN.Enabled = true; 
            }
        }
    }
}