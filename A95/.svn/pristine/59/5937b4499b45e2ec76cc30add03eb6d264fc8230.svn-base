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
    public partial class JumpStationQCV2_Form : FrmBase
    {
        string S_UnitID = "";

        public JumpStationQCV2_Form()
        {
            InitializeComponent();
        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            Edt_SN.Enabled = false;
            Edt_ChildSN.Enabled = false;
        }

        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            base.Btn_ConfirmPO_Click(sender, e);
            base.Btn_Defect.Enabled = true;
            base.Com_luUnitStatus.Enabled = true;
            Edt_SN.Enabled = true;
            Edt_SN.Focus();

            SetQC();
        }

        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                List<mesUnit> List_mesUnit = new List<mesUnit>();
                List<mesHistory> List_mesHistory = new List<mesHistory>();
                List<mesUnitDefect> List_mesUnitDefect = new List<mesUnitDefect>();

                DateTime dateStart = DateTime.Now;

                string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                string PartFamilyID = Com_PartFamily.EditValue.ToString();
                string S_StationID = List_Login.StationID.ToString();
                string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();

                string S_UnitStateID2 = "";
                string S_StationID2 = "";
                
                if (S_luUnitStateID == "1")
                {
                    DataTable DT = public_.GetmesStationConfig("FQC1_OK_UnitStateID", List_Login.StationID.ToString());
                    S_UnitStateID2 = DT.Rows[0]["Value"].ToString();

                    DT = public_.GetmesStationConfig("FQC1_OK_StationID", List_Login.StationID.ToString());
                    S_StationID2 = DT.Rows[0]["Value"].ToString();
                }

                string S_DefectID = base.DefectChar;
                string S_SN = Edt_SN.Text.Trim();
                //工单  PartID
                string S_POPartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();

                if (S_SN == "")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }

                if (S_luUnitStateID == "2" || S_luUnitStateID == "3")
                {
                    if (S_DefectID == "")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20049", "NG", List_Login.Language);
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }
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
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20014", "NG", List_Login.Language, new string[] { S_SN });
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
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20015", "NG", List_Login.Language, new string[] { S_SN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }

                DataTable DT_SN = PartSelectSVC.GetmesSerialNumber(S_SN).Tables[0]; 
                if (DT_SN.Rows.Count > 0)
                {
                    S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                    DataTable DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0];

                    if (DT_Unit.Rows[0]["StatusID"].ToString() != "1")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20036", "NG", List_Login.Language);
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }

                    string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, S_SN);

                    if (S_RouteCheck == "1")
                    {
                        string S_SelectPartID = DT_Unit.Rows[0]["PartID"].ToString();
                        if (S_POPartID == S_SelectPartID)
                        {
                            string S_StationTypeID = List_Login.StationTypeID.ToString();
                            DT_ProductStructure = PartSelectSVC.GetmesProductStructure2(S_POPartID, S_StationTypeID).Tables[0];   //public_.P_DataSet(S_Sql).Tables[0];
                            if (DT_ProductStructure.Rows.Count > 0)
                            {
                                Edt_SN.Enabled = false;
                                Edt_ChildSN.Enabled = true;
                                Edt_ChildSN.Focus();
                            }
                            else
                            {
                                try
                                {

                                    //调用通用过程
                                    string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>";
                                    string xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                                    string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                                    string outString = string.Empty;
                                    PartSelectSVC.uspCallProcedure("uspQCCheck", S_SN, xmlProdOrder, xmlPart, xmlStation, null, null, ref outString);
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
                                        MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
                                        return;
                                    }

                                    //////////////////////////////////////////////////////
                                    /// //二次检查是否有指定线路  20231214
                                    var tmpSecondRet = PartSelectSVC.GetmesUnitStateSecond(S_PartID, PartFamilyID, "", List_Login.LineID.ToString(), List_Login.StationTypeID,
                                         Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString(), S_SN);

                                    if (tmpSecondRet.StartsWith("Error", StringComparison.OrdinalIgnoreCase))
                                    {
                                        //MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + mainSN });
                                        MessageInfo.Add_Info_MSG(Edt_MSG, "NG", tmpSecondRet);
                                        Edt_SN.Focus();
                                        Edt_SN.Text = "";
                                        return;
                                    }

                                    S_UnitStateID = tmpSecondRet;
                                    if (string.IsNullOrEmpty(S_UnitStateID))
                                    {
                                        MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                                        Edt_SN.Focus();
                                        Edt_SN.Text = "";
                                        return;
                                    }
                                    /////////////////////////////////////////////////////


                                    S_POID = DT_ProductionOrder.Rows[0]["ID"].ToString();
                                    DataTable DT_UPUnitID = PartSelectSVC.Get_UnitID(S_SN).Tables[0];

                                    mesUnit v_mesUnit = new mesUnit();
                                    v_mesUnit.ID = Convert.ToInt32(DT_UPUnitID.Rows[0]["UnitID"].ToString());
                                    v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                                    v_mesUnit.StatusID = Convert.ToInt32(S_luUnitStateID);
                                    v_mesUnit.StationID = Convert.ToInt32(S_StationID);//List_Login.StationID;
                                    v_mesUnit.EmployeeID = List_Login.EmployeeID;
                                    v_mesUnit.LastUpdate = DateTime.Now;
                                    v_mesUnit.ProductionOrderID = Convert.ToInt32(S_POID);
                                    //修改 Unit
                                    List_mesUnit.Add(v_mesUnit);
                                    //string S_UpdateUnit = mesUnitSVC.Update(v_mesUnit);
                                    //if (S_UpdateUnit.Substring(0, 1) == "E")
                                    //{
                                    //    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_UpdateUnit });
                                    //    return;
                                    //}

                                    //if (S_UpdateUnit.Substring(0, 1) != "E")
                                    {
                                        mesHistory v_mesHistory = new mesHistory();

                                        v_mesHistory.UnitID = Convert.ToInt32(v_mesUnit.ID);
                                        v_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                                        v_mesHistory.EmployeeID = List_Login.EmployeeID;
                                        v_mesHistory.StationID = Convert.ToInt32(S_StationID); //List_Login.StationID;
                                        v_mesHistory.EnterTime = DateTime.Now;
                                        v_mesHistory.ExitTime = DateTime.Now;
                                        v_mesHistory.ProductionOrderID = Convert.ToInt32(S_POID);
                                        v_mesHistory.PartID = Convert.ToInt32(S_POPartID);
                                        v_mesHistory.LooperCount = 1;
                                        v_mesHistory.StatusID = Convert.ToInt32(S_luUnitStateID);
                                        //int I_InsertHistory = mesHistorySVC.Insert(v_mesHistory);
                                        List_mesHistory.Add(v_mesHistory);

                                        if (S_luUnitStateID == "1")  //OK 跳站到 FQ2
                                        {
                                            v_mesUnit = new mesUnit();
                                            v_mesUnit.ID = Convert.ToInt32(DT_UPUnitID.Rows[0]["UnitID"].ToString());
                                            v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID2);
                                            v_mesUnit.StatusID = Convert.ToInt32(S_luUnitStateID);
                                            v_mesUnit.StationID = Convert.ToInt32(S_StationID2);//List_Login.StationID;
                                            v_mesUnit.EmployeeID = List_Login.EmployeeID;
                                            v_mesUnit.LastUpdate = DateTime.Now;
                                            v_mesUnit.ProductionOrderID = Convert.ToInt32(S_POID);
                                            //修改 Unit
                                            //S_UpdateUnit = mesUnitSVC.Update(v_mesUnit);
                                            List_mesUnit.Add(v_mesUnit); 

                                        }

                                        //////////////////////////  Defect ///////////////////////////////////////////
                                        //////////////////////////////////////////////////////////////////////////////
                                        string[] Array_Defect = S_DefectID.Split(';');
                                        if (S_luUnitStateID != "1")
                                        {
                                            foreach (var item in Array_Defect)
                                            {
                                                //string S_mesUnitDefectInsert = "";
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
                                                        //S_mesUnitDefectInsert = mesUnitDefectSVC.Insert(v_mesUnitDefect);
                                                    }
                                                }
                                                catch(Exception ex)
                                                {
                                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                                }
                                            }
                                            //非Pass产品解绑关联的治具
                                            string result = string.Empty;
                                            string xmlStationStr = "<Part PartID=\"" + List_Login.StationID.ToString() + "\"> </Part>";
                                            string xmlPartStr = "<Part PartID=\"" + Com_Part.EditValue.ToString() + "\"> </Part>";
                                            PartSelectSVC.uspCallProcedure("uspQcReleaseMachine", Edt_SN.Text.ToString(),
                                                                                    null, xmlPartStr, xmlStationStr, null, null, ref result);
                                        }

                                        mesUnit[] L_mesUnit = List_mesUnit.ToArray();
                                        mesHistory[] L_mesHistory = List_mesHistory.ToArray();
                                        mesUnitDefect[] L_mesUnitDefect = List_mesUnitDefect.ToArray();

                                        string ReturnValue = DataCommitSVC.SubmitDataUHD(L_mesUnit, L_mesHistory, L_mesUnitDefect);
                                        if (ReturnValue != "OK")
                                        {
                                            ReturnValue = "SN:" + S_SN + "  " + ReturnValue;
                                            MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ReturnValue });
                                            return;
                                        }


                                        /////////////////////////////////////////////////////////////////////////
                                        /////////////////////////////////////////////////////////////////////////
                                        //if (S_luUnitStateID == "1")
                                        //{
                                        //    base.SetOverStiaonQTY(true);
                                        //}
                                        //else if(S_luUnitStateID == "2")
                                        //{
                                        //    base.SetOverStiaonQTY(false);
                                        //}

                                        base.SetOverStiaonQTY_V2(S_luUnitStateID);

                                        Edt_SN.Text = "";
                                        Edt_SN.Enabled = true;
                                        Edt_SN.Focus();

                                        Edt_ChildSN.Text = "";
                                        Edt_ChildSN.Enabled = false;
                                        List_ChildSN.Items.Clear();

                                        TimeSpan ts = DateTime.Now - dateStart;
                                        double mill = Math.Round(ts.TotalMilliseconds, 0);
                                        MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { S_SN, mill.ToString() });
                                    }
                                }
                                catch(Exception ex)
                                {
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                }
                            }
                        }
                        else
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20050", "NG", List_Login.Language);
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
                        }
                       
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                    }
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language, new string[] { S_SN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                }
            }
        }

        private void Edt_ChildSN_KeyDown(object sender, KeyEventArgs e)
        {

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
                    Edt_SN.BackColor = Com_luUnitStatus.ForeColor;
                    break;
                case "2":
                    lblDefectCode.ForeColor = Color.Red;
                    lblDefectCode.Visible = true;
                    Edt_SN.BackColor = Com_luUnitStatus.ForeColor;
                    break;
                case "3":
                    lblDefectCode.ForeColor = Color.Orange;
                    lblDefectCode.Visible = true;
                    Edt_SN.BackColor = Com_luUnitStatus.ForeColor;
                    break;
                default:
                    lblDefectCode.Visible = false;
                    Edt_SN.BackColor = Com_luUnitStatus.ForeColor;
                    break;
            }

            if (lblDefectCode.Visible == false)
            {
                Edt_SN.Focus();
            }
        }

        public override void btnDefectSave_Click(object sender, EventArgs e)
        {
            base.btnDefectSave_Click(sender, e);
            lblDefectCode.Text = "DefectCode:" + DefectCode;

            Edt_SN.Focus();
        }
    }
}