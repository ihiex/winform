using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.MyMES.mesUnitComponentService;
using App.Model;

namespace App.MyMES
{
    public partial class IQCV2_Form : FrmBase
    {
        string InnerSN_Pattern = string.Empty;

        public IQCV2_Form()
        {
            InitializeComponent();
        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            Edt_SN.Enabled = false;
            Edt_SN.Text = string.Empty;
            txtChildSN.Enabled = false;
            txtChildSN.Text = string.Empty;
        }

        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            base.Btn_ConfirmPO_Click(sender, e);
            DataSet dataSet;
            //批次正则
            int I_PartID = Convert.ToInt32(Com_Part.EditValue.ToString());
            dataSet = PartSelectSVC.GetmesPartDetail(I_PartID, "InnerSN_Pattern");
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                InnerSN_Pattern = dataSet.Tables[0].Rows[0]["Content"].ToString();
                lblInnerSN.Visible = true;
                txtChildSN.Visible = true;
            }
            else
            {
                lblInnerSN.Visible = false;
                txtChildSN.Visible = false;
            }

            lblMainSN.Text = Com_Part.Text.ToString();
            base.Btn_Defect.Enabled = true;
            base.Com_luUnitStatus.Enabled = true;
            Edt_SN.Enabled = true;
            txtChildSN.Enabled = false;
            Edt_SN.Focus();

            SetQC();
        }

        public override void FrmBase_Load(object sender, EventArgs e)
        {
            base.FrmBase_Load(sender, e);
            lblMainSN.Text = Com_Part.Text.ToString();
        }

        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                List<mesUnit> List_mesUnit = new List<mesUnit>();                
                List<mesUnitDetail> List_mesUnitDetail = new List<mesUnitDetail>();
                List<mesHistory> List_mesHistory = new List<mesHistory>();
                List<mesSerialNumber> List_mesSerialNumber = new List<mesSerialNumber>();
                List<mesUnitComponent> List_mesUnitComponent = new List<mesUnitComponent>();                
                List<mesUnitDefect> List_mesUnitDefect = new List<mesUnitDefect>(); 

                string MainSN = Edt_SN.Text.Trim();
                if (string.IsNullOrEmpty(MainSN))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language, new string[] { "SN:" + MainSN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }
                if (!Regex.IsMatch(MainSN, SN_Pattern))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20027", "NG", List_Login.Language, new string[] { MainSN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }

                DataSet Dts = PartSelectSVC.GetmesSerialNumber(MainSN);
                if (Dts != null && Dts.Tables.Count > 0 && Dts.Tables[0].Rows.Count > 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20006", "NG", List_Login.Language, new string[] { "SN:" + MainSN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }



                if (string.IsNullOrEmpty(InnerSN_Pattern))
                {
                    //调用通用过程
                    string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>";
                    string xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                    string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                    string outString = string.Empty;
                    PartSelectSVC.uspCallProcedure("uspQCCheck", MainSN,
                                                                            xmlProdOrder, xmlPart, xmlStation, null, null, ref outString);
                    if (outString != "1")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { MainSN, ProMsg }, outString);
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }

                    PartSelectSVC.uspCallProcedure("uspPONumberCheck", Com_PO.EditValue.ToString(),
                                    null, null, null, null, "1", ref outString);
                    if (outString != "1")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { MainSN, ProMsg }, outString);
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }

                    string S_PartID = Com_Part.EditValue.ToString();
                    string PartFamilyID = Com_PartFamily.EditValue.ToString();
                    string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();
                    string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID,
                        List_Login.LineID.ToString(),Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());
                    if (string.IsNullOrEmpty(S_UnitStateID))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + MainSN });
                        return;
                    }
                    //////////////////////////////////////////////////////
                    /// //二次检查是否有指定线路  20231214
                    var tmpSecondRet = PartSelectSVC.GetmesUnitStateSecond(S_PartID, PartFamilyID, "", List_Login.LineID.ToString(), 
                        List_Login.StationTypeID,Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString(), MainSN);

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
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + MainSN });
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }
                    /////////////////////////////////////////////////////


                    mesUnit v_mesUnit = new mesUnit();
                    v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                    v_mesUnit.StatusID = Convert.ToInt32(S_luUnitStateID);
                    v_mesUnit.StationID = List_Login.StationID;
                    v_mesUnit.EmployeeID = List_Login.EmployeeID;
                    v_mesUnit.CreationTime = DateTime.Now;
                    v_mesUnit.LastUpdate = DateTime.Now;
                    v_mesUnit.PanelID = 0;
                    v_mesUnit.LineID = List_Login.LineID;
                    v_mesUnit.ProductionOrderID = Convert.ToInt32(Com_PO.EditValue.ToString());
                    v_mesUnit.RMAID = 0;
                    v_mesUnit.PartID = Convert.ToInt32(S_PartID);
                    v_mesUnit.LooperCount = 1;

                    List_mesUnit.Add(v_mesUnit);

                    mesHistory v_mesHistory = new mesHistory();
                    v_mesHistory.UnitStateID = v_mesUnit.UnitStateID;
                    v_mesHistory.EmployeeID = v_mesUnit.EmployeeID;
                    v_mesHistory.StationID = v_mesUnit.StationID;
                    v_mesHistory.EnterTime = DateTime.Now;
                    v_mesHistory.ExitTime = DateTime.Now;
                    v_mesHistory.ProductionOrderID = v_mesUnit.ProductionOrderID;
                    v_mesHistory.PartID = Convert.ToInt32(v_mesUnit.PartID);
                    v_mesHistory.LooperCount = 1;
                    v_mesHistory.StatusID = Convert.ToInt32(S_luUnitStateID);

                    List_mesHistory.Add(v_mesHistory);

                    mesSerialNumber v_mesSerialNumber = new mesSerialNumber();
                    v_mesSerialNumber.SerialNumberTypeID = 0;
                    v_mesSerialNumber.Value = MainSN;

                    List_mesSerialNumber.Add(v_mesSerialNumber);


                    mesUnitDetail v_mesDetail = new mesUnitDetail();                    
                    v_mesDetail.reserved_01 = "";
                    v_mesDetail.reserved_02 = "";
                    v_mesDetail.reserved_03 = "";
                    v_mesDetail.reserved_04 = "";
                    v_mesDetail.reserved_05 = "";

                    List_mesUnitDetail.Add(v_mesDetail);


                    //////////////////////////  Defect ///////////////////////////////////////////
                    //////////////////////////////////////////////////////////////////////////////
                    string S_DefectID = base.DefectChar;
                    string[] Array_Defect = S_DefectID.Split(';');
                    if (S_luUnitStateID != "1")
                    {                        
                        foreach (var item in Array_Defect)
                        {                           
                            try
                            {
                                if (item.Trim() != "")
                                {
                                    int I_DefectID = Convert.ToInt32(item);

                                    mesUnitDefect v_mesUnitDefect = new mesUnitDefect();
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
                    }

                    mesUnit[] L_mesUnit = List_mesUnit.ToArray();
                    mesUnitDetail[] L_mesUnitDetail = List_mesUnitDetail.ToArray(); 
                    mesHistory[] L_mesHistory = List_mesHistory.ToArray();
                    mesSerialNumber[] L_mesSerialNumber = List_mesSerialNumber.ToArray();
                    mesUnitComponent[] L_mesUnitComponent = List_mesUnitComponent.ToArray(); 
                    mesUnitDefect[] L_mesUnitDefect = List_mesUnitDefect.ToArray();

                    string ReturnValue = DataCommitSVC.InsertALL(L_mesUnit, L_mesUnitDetail, L_mesHistory, 
                        L_mesSerialNumber, L_mesUnitComponent, L_mesUnitDefect,null,null,null);
                    if (ReturnValue != "OK")
                    {
                        for (int i = 0; i < 10 && ReturnValue.Contains("PRIMARY KEY") && ReturnValue.Contains("Unit_PK"); i++)
                        {
                            ReturnValue = DataCommitSVC.InsertALL(L_mesUnit, L_mesUnitDetail, L_mesHistory,
                                L_mesSerialNumber, L_mesUnitComponent, L_mesUnitDefect, null, null, null);
                        }
                        if (ReturnValue != "OK")
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ReturnValue });
                        }
                    }

                    if (S_luUnitStateID == "1")
                    {
                        if (ReturnValue == "OK")
                        {
                            base.SetOverStiaonQTY(true);
                        }
                    }
                    else if (S_luUnitStateID == "2")
                    {
                        base.SetOverStiaonQTY(false);
                    }

                    base.GetProductionCount();

                    MessageInfo.Add_Info_MSG(Edt_MSG, "10030", "OK", List_Login.Language, new string[] { lblMainSN.Text.ToString(),MainSN });
                    Edt_SN.Text = string.Empty;
                }
                else
                {
                    Edt_SN.Enabled = false;
                    txtChildSN.Enabled = true;
                    txtChildSN.Focus();
                }
            }
        }

        private void txtChildSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                List<mesUnit> List_mesUnit = new List<mesUnit>();
                List<mesUnitDetail> List_mesUnitDetail = new List<mesUnitDetail>();
                List<mesHistory> List_mesHistory = new List<mesHistory>();
                List<mesSerialNumber> List_mesSerialNumber = new List<mesSerialNumber>();
                List<mesUnitComponent> List_mesUnitComponent = new List<mesUnitComponent>();
                List<mesUnitDefect> List_mesUnitDefect = new List<mesUnitDefect>();

                string ChildSN = txtChildSN.Text.Trim();
                string MainSN = Edt_SN.Text.Trim();

                if (string.IsNullOrEmpty(ChildSN))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language, new string[] { "SN:" + MainSN });
                    txtChildSN.Focus();
                    txtChildSN.Text = "";
                    return;
                }
                if (!Regex.IsMatch(ChildSN, InnerSN_Pattern))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20027", "NG", List_Login.Language, new string[] { "SN:" + ChildSN });
                    txtChildSN.Focus();
                    txtChildSN.Text = "";
                    return;
                }

                //调用通用过程
                string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>";
                string xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";                
                string outString = string.Empty;
                PartSelectSVC.uspCallProcedure("uspQCCheck", MainSN,
                                                                        xmlProdOrder, xmlPart, xmlStation, null, ChildSN, ref outString);
                if (outString != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { MainSN, ProMsg }, outString);
                    Edt_SN.Enabled = true;
                    Edt_SN.Focus();
                    Edt_SN.Text = "";

                    txtChildSN.Text = "";
                    txtChildSN.Enabled = false; 
                    return;
                }


                outString = string.Empty;
                PartSelectSVC.uspCallProcedure("uspPONumberCheck", Com_PO.EditValue.ToString(),
                                    null, null, null, null, "1", ref outString);
                if (outString != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { MainSN, ProMsg }, outString);
                    Edt_SN.Enabled = true;
                    Edt_SN.Text = "";
                    txtChildSN.Text = "";
                    txtChildSN.Enabled = false;
                    Edt_SN.Focus();
                    return;
                }

                bool Result = mesUnitComponentSVC.MESCheckChildSerialNumber(ChildSN);
                if(!Result)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20006", "NG", List_Login.Language, new string[] { "SN:" + ChildSN });
                    txtChildSN.Focus();
                    txtChildSN.Text = "";
                    return;
                }

                DataSet DtsMain = PartSelectSVC.GetmesSerialNumber(MainSN);
                if (DtsMain != null && DtsMain.Tables.Count > 0 && DtsMain.Tables[0].Rows.Count > 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20202", "NG", List_Login.Language, new string[] { "SN:" + ChildSN });
                    Edt_SN.Text = "";
                    Edt_SN.Enabled = true;
                    Edt_SN.Focus();
                    txtChildSN.Text = "";
                    txtChildSN.Enabled = false;
                    return;
                }

                string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();
                string S_PartID = Com_Part.EditValue.ToString();
                string PartFamilyID = Com_PartFamily.EditValue.ToString();
                //string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID, 
                //    List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());

                string S_UnitStateID = string.Empty;
                DataSet tmpDataSet = PartSelectSVC.GetmesUnitState(S_PartID, PartFamilyID, "", List_Login.LineID.ToString(), List_Login.StationTypeID, Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());
                if (tmpDataSet != null && tmpDataSet.Tables.Count != 0 && tmpDataSet.Tables[0].Rows.Count != 0)
                {
                    S_UnitStateID = tmpDataSet.Tables[0].Rows[0]["ID"].ToString().Trim();
                }
                if (string.IsNullOrEmpty(S_UnitStateID))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + ChildSN });
                    return;
                }
                mesUnit v_mesUnit = new mesUnit();
                v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                v_mesUnit.StatusID = Convert.ToInt32(S_luUnitStateID);
                v_mesUnit.StationID = List_Login.StationID;
                v_mesUnit.EmployeeID = List_Login.EmployeeID;
                v_mesUnit.CreationTime = DateTime.Now;
                v_mesUnit.LastUpdate = DateTime.Now;
                v_mesUnit.PanelID = 0;
                v_mesUnit.LineID = List_Login.LineID;
                v_mesUnit.ProductionOrderID = Convert.ToInt32(Com_PO.EditValue.ToString());
                v_mesUnit.RMAID = 0;
                v_mesUnit.PartID = Convert.ToInt32(S_PartID);
                v_mesUnit.LooperCount = 1;

                List_mesUnit.Add(v_mesUnit);

                //写入UnitDetail表
                mesUnitDetail msDetail = new mesUnitDetail();               
                msDetail.reserved_01 = "";
                msDetail.reserved_02 = "";
                msDetail.reserved_03 = "";
                msDetail.reserved_04 = "";
                msDetail.reserved_05 = "";

                List_mesUnitDetail.Add(msDetail);

                mesHistory v_mesHistory = new mesHistory();                
                v_mesHistory.UnitStateID = v_mesUnit.UnitStateID;
                v_mesHistory.EmployeeID = v_mesUnit.EmployeeID;
                v_mesHistory.StationID = v_mesUnit.StationID;
                v_mesHistory.EnterTime = DateTime.Now;
                v_mesHistory.ExitTime = DateTime.Now;
                v_mesHistory.ProductionOrderID = v_mesUnit.ProductionOrderID;
                v_mesHistory.PartID = Convert.ToInt32(v_mesUnit.PartID);
                v_mesHistory.LooperCount = 1;
                v_mesHistory.StatusID = Convert.ToInt32(S_luUnitStateID);

                List_mesHistory.Add(v_mesHistory);

                mesSerialNumber v_mesSerialNumber = new mesSerialNumber();
                v_mesSerialNumber.SerialNumberTypeID = 0;
                v_mesSerialNumber.Value = MainSN;

                List_mesSerialNumber.Add(v_mesSerialNumber);


                mesUnitComponent v_mesUnitComponent = new mesUnitComponent();
                v_mesUnitComponent.UnitComponentTypeID = 1;
                v_mesUnitComponent.ChildUnitID = 0;
                v_mesUnitComponent.ChildSerialNumber = ChildSN;
                v_mesUnitComponent.ChildLotNumber = "";
                v_mesUnitComponent.ChildPartID = 0;
                v_mesUnitComponent.ChildPartFamilyID = 0;
                v_mesUnitComponent.Position = "";
                v_mesUnitComponent.InsertedEmployeeID = List_Login.EmployeeID;
                v_mesUnitComponent.InsertedStationID = List_Login.StationID;
                v_mesUnitComponent.StatusID = 1;

                List_mesUnitComponent.Add(v_mesUnitComponent); 

                //////////////////////////  Defect ///////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////
                string S_DefectID = base.DefectChar;
                string[] Array_Defect = S_DefectID.Split(';');
                if (S_luUnitStateID != "1")
                {
                    foreach (var item in Array_Defect)
                    {
                        try
                        {
                            if (item.Trim() != "")
                            {
                                int I_DefectID = Convert.ToInt32(item);
                                mesUnitDefect v_mesUnitDefect = new mesUnitDefect();

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
                }

                mesUnit[] L_mesUnit = List_mesUnit.ToArray();
                mesUnitDetail[] L_mesUnitDetail = List_mesUnitDetail.ToArray();
                mesHistory[] L_mesHistory = List_mesHistory.ToArray();
                mesSerialNumber[] L_mesSerialNumber = List_mesSerialNumber.ToArray();
                mesUnitComponent[] L_mesUnitComponent = List_mesUnitComponent.ToArray();
                mesUnitDefect[] L_mesUnitDefect = List_mesUnitDefect.ToArray();

                string ReturnValue = DataCommitSVC.InsertALL(L_mesUnit, L_mesUnitDetail, L_mesHistory,
                    L_mesSerialNumber, L_mesUnitComponent, L_mesUnitDefect, null, null,null);
                if (ReturnValue != "OK")
                {
                    for (int i = 0; i < 10 && ReturnValue.Contains("PRIMARY KEY") && ReturnValue.Contains("Unit_PK"); i++)
                    {
                        ReturnValue = DataCommitSVC.InsertALL(L_mesUnit, L_mesUnitDetail, L_mesHistory,
                            L_mesSerialNumber, L_mesUnitComponent, L_mesUnitDefect, null, null, null);
                    }

                    if (ReturnValue != "OK")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { "SN:" + ChildSN + "  " + ReturnValue });
                    }
                }

                if (S_luUnitStateID == "1")
                {
                    if (ReturnValue == "OK")
                    {
                        base.SetOverStiaonQTY(true);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "10030", "OK", List_Login.Language, new string[] { lblMainSN.Text.ToString(), MainSN });
                    }
                }
                else if (S_luUnitStateID == "2")
                {
                    base.SetOverStiaonQTY(false);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10031", "OK", List_Login.Language, new string[] { lblMainSN.Text.ToString(), MainSN ,Com_luUnitStatus.Text.Trim()});
                }

                base.GetProductionCount();

                txtChildSN.Text = string.Empty;
                txtChildSN.Enabled = false;
                Edt_SN.Text = string.Empty;
                Edt_SN.Enabled = true;
                Edt_SN.Focus();
            }
        }

        public override void Com_luUnitStatus_EditValueChanged(object sender, EventArgs e)
        {
            base.Com_luUnitStatus_EditValueChanged(sender, e);
            string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();
            switch (S_luUnitStateID)
            {
                case "1":
                    lblDefectCode.Visible = false;                    
                    break;
                case "2":
                    lblDefectCode.ForeColor = Color.Red;
                    lblDefectCode.Visible = true;
                    break;
                case "3":
                    lblDefectCode.ForeColor = Color.Orange;
                    lblDefectCode.Visible = true;
                    break;
                default:
                    lblDefectCode.Visible = false;
                    break;
            }

            Edt_SN.BackColor = Com_luUnitStatus.ForeColor;
            txtChildSN.BackColor = Com_luUnitStatus.ForeColor;
        }

        public override void btnDefectSave_Click(object sender, EventArgs e)
        {
            base.btnDefectSave_Click(sender, e);
            lblDefectCode.Text = "DefectCode:" + DefectCode;
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            Edt_SN.Text = string.Empty;
            txtChildSN.Text = string.Empty;
            txtChildSN.Enabled = false;
            Edt_SN.Enabled = true;
            Edt_SN.Focus();
        }
    }
}
