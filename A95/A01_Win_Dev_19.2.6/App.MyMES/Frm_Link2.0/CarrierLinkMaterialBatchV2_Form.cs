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
using System.Text.RegularExpressions;

namespace App.MyMES
{
    public partial class CarrierLinkMaterialBatchV2_Form : FrmBase
    {
        public CarrierLinkMaterialBatchV2_Form()
        {
            InitializeComponent();
        }

        private void Edt_Box_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                string Result = string.Empty;
                string xmlPartStr = "<Part PartID=\"" + base.Com_Part.EditValue.ToString() + "\"> </Part>";

                PartSelectSVC.uspCallProcedure("uspMachineToolingCheck", Edt_Box.Text.Trim(),
                                                                        null, xmlPartStr, null, null, List_Login.StationTypeID.ToString(), ref Result);
                if (Result != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Edt_Box.Text.Trim(), ProMsg }, Result);
                    Edt_Box.SelectAll();
                    Edt_Box.Text = "";
                    Edt_Box.Focus();
                }
                else
                {
                    Edt_Box.Enabled = false;
                    Edt_BactchSN.Enabled = true;
                    Edt_BactchSN.Focus();
                    Edt_BactchSN.SelectAll();
                }
            }
        }

        private void Edt_BactchSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                List<mesUnit> List_mesUnit = new List<mesUnit>();
                List<mesHistory> List_mesHistory = new List<mesHistory>();
                List<mesUnitDetail> List_mesUnitDetail = new List<mesUnitDetail>();
                List<mesSerialNumber> List_mesSerialNumber = new List<mesSerialNumber>();
                List<mesUnitDefect> List_mesUnitDefect = new List<mesUnitDefect>(); 

                List<mesMachine> List_mesMachine = new List<mesMachine>(); 

                string S_BatchSN = Edt_BactchSN.Text.Trim();
                if (string.IsNullOrEmpty(Batch_Pattern))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20025", "NG", List_Login.Language, new string[] { Com_Part.EditValue.ToString() });
                    Edt_BactchSN.SelectAll();
                    return;
                }

                if (!Regex.IsMatch(Edt_BactchSN.Text.Trim(), Batch_Pattern))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20047", "NG", List_Login.Language, new string[] { S_BatchSN });
                    Edt_BactchSN.SelectAll();
                    return;
                }

                try
                {
                    string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                    string PartFamilyID = Com_PartFamily.EditValue.ToString();
                    string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID, 
                        List_Login.LineID.ToString(), Com_PO.EditValue.ToString(),Com_luUnitStatus.EditValue.ToString());
                    if (string.IsNullOrEmpty(S_UnitStateID))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
                        Edt_BactchSN.Focus();
                        Edt_BactchSN.Text = "";
                        return;
                    }
                    string S_DefectID = base.DefectChar;
                    string S_CardSN = Edt_Box.Text.Trim();
                    string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                    string S_ProductionOrderID = Com_PO.EditValue.ToString();
                    //string S_xmlPart = "'<BoxSN SN=" + "\"" + S_CardSN + "\"" + "> </BoxSN>'";
                    string S_FormatSN = PartSelectSVC.mesGetSNFormatIDByList(S_PartID, S_PartFamilyID,
                        List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), List_Login.StationTypeID.ToString());
                    //DataSet DS=PartSelectSVC.uspSNRGetNext(S_FormatSN, 0, null, S_xmlPart, null, null, null);

                    string S_PartFamilyType = "";
                    string xmlProdOrder = "'<ProdOrder ProductionOrder=" + "\"" + S_ProductionOrderID + "\"" + "> </ProdOrder>'";
                    string xmlStation = "'<Station StationID=" + "\"" + List_Login.StationID.ToString() + "\"" + "> </Station>'";
                    string xmlPart = "'<Part PartID=" + "\"" + S_PartID + "\"" + "> </Part>'";
                    string xmlExtraData = "'<ExtraData BoxSN=" + "\"" + S_CardSN + "\"" +
                                                   " LineID = " + "\"" + List_Login.LineID.ToString() + "\"" +
                                                   " PartFamilyTypeID=" + "\"" + S_PartFamilyType + "\"" +
                                                   " LineType=" + "\"" + "M" + "\"" + "> </ExtraData>'";
                    DataSet DS = PartSelectSVC.uspSNRGetNext(S_FormatSN, 0, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, null);

                    DataTable DT = DS.Tables[1];
                    string S_SN = DT.Rows[0][0].ToString(); //S_CardSN + "_" + DateTime.Now.ToString("yyyyMMddHHmm");

                    if (S_CardSN == S_BatchSN)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20048", "NG", List_Login.Language);
                        return;
                    }

                    //工单  PartID
                    string S_POPartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                    //选择的  PartID
                    string S_SelectPartID = Com_Part.EditValue.ToString();
                    // luUnitStatus  ID
                    string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();   

                    if (S_luUnitStateID == "2" || S_luUnitStateID == "3")
                    {
                        if (S_DefectID == "")
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20049", "NG", List_Login.Language);
                            Edt_BactchSN.Focus();
                            Edt_BactchSN.Text = "";
                            return;
                        }
                    }

                    DataTable DT_Card= PartSelectSVC.GetmesMachine(S_CardSN).Tables[0];
                    if (DT_Card.Rows.Count == 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language,new string[] { S_CardSN });
                        Edt_Box.SelectAll();
                    }
                    else
                    {
                        string S_StatusID = DT_Card.Rows[0]["StatusID"].ToString();
                        if (S_StatusID == "1")  //Box 文本框已做状态判断
                        {
                            if (S_POPartID == S_SelectPartID)
                            {
                                //批次验证
                                string xmlPartBatch = "<Part PartID=\"" + Com_Part.EditValue.ToString() + "\"> </Part>";
                                string OutputStr = string.Empty;
                                PartSelectSVC.uspCallProcedure("uspBatchDataCheck", Edt_BactchSN.Text.Trim(),
                                                                                        null, xmlPartBatch, null, null, "1", ref OutputStr);
                                if (OutputStr != "1")
                                {
                                    string ProMsg = MessageInfo.GetMsgByCode(OutputStr, List_Login.Language);
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_BatchSN, ProMsg }, OutputStr);
                                    return;
                                }

                                // 本工位不判断 子料                                                               
                                string S_POID = DT_ProductionOrder.Rows[0]["ID"].ToString();
 
                                mesUnit v_mesUnit = new mesUnit();
                                v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                                v_mesUnit.StatusID = Convert.ToInt32(S_luUnitStateID);
                                v_mesUnit.StationID = List_Login.StationID;
                                v_mesUnit.EmployeeID = List_Login.EmployeeID;
                                v_mesUnit.CreationTime = DateTime.Now;
                                v_mesUnit.LastUpdate = DateTime.Now;

                                v_mesUnit.PanelID = 0;
                                v_mesUnit.LineID = List_Login.LineID;
                                v_mesUnit.ProductionOrderID = Convert.ToInt32(S_POID);

                                v_mesUnit.RMAID = 0;
                                v_mesUnit.PartID = Convert.ToInt32(S_PartID);
                                v_mesUnit.LooperCount = 1;

                                List_mesUnit.Add(v_mesUnit);
                               
                                try
                                {
                                    mesSerialNumber v_mesSerialNumber = new mesSerialNumber();

                                    //v_mesSerialNumber.UnitID = Convert.ToInt32(S_InsertUnit);
                                    v_mesSerialNumber.SerialNumberTypeID = 8;
                                    v_mesSerialNumber.Value = S_SN;

                                    List_mesSerialNumber.Add(v_mesSerialNumber);

                                    //////////////////////////////////////  mesUnitDetail /////////////////////////////////////////////////////                                    
                                    mesUnitDetail v_mesUnitDetail = new mesUnitDetail();
                                    //v_mesUnitDetail.UnitID = Convert.ToInt32(v_mesUnit.ID);
                                    v_mesUnitDetail.reserved_01 = S_CardSN;
                                    v_mesUnitDetail.reserved_02 = S_BatchSN;
                                    v_mesUnitDetail.reserved_03 = "1";        //开料 1  备胶  2          
                                    v_mesUnitDetail.reserved_04 = "";
                                    v_mesUnitDetail.reserved_05 = "";

                                    List_mesUnitDetail.Add(v_mesUnitDetail);
                                }
                                catch (Exception ex)
                                {
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                }

                                try
                                {
                                    //////////////////////////////////////  mesHistory /////////////////////////////////////////////////////                                    
                                    mesHistory v_mesHistory = new mesHistory();
                                    v_mesHistory.UnitID = Convert.ToInt32(v_mesUnit.ID);
                                    v_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                                    v_mesHistory.EmployeeID = List_Login.EmployeeID;
                                    v_mesHistory.StationID = List_Login.StationID;
                                    v_mesHistory.EnterTime = DateTime.Now;
                                    v_mesHistory.ExitTime = DateTime.Now;
                                    v_mesHistory.ProductionOrderID = Convert.ToInt32(S_POID);
                                    v_mesHistory.PartID = Convert.ToInt32(S_POPartID);
                                    v_mesHistory.LooperCount = 1;
                                    v_mesHistory.StatusID = 1;

                                    List_mesHistory.Add(v_mesHistory);
                                }
                                catch (Exception ex)
                                {
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                }

                                //////////////////////////  Defect ///////////////////////////////////////////
                                //////////////////////////////////////////////////////////////////////////////
                                string[] Array_Defect = S_DefectID.Split(';');
                                if (S_luUnitStateID == "2")
                                {
                                    foreach (var item in Array_Defect)
                                    {
                                        try
                                        {
                                            if (item.Trim() != "")
                                            {
                                                int I_DefectID = Convert.ToInt32(item);
                                                mesUnitDefect v_mesUnitDefect = new mesUnitDefect();

                                                //v_mesUnitDefect.UnitID = v_mesUnit.ID;
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
                                    base.SetOverStiaonQTY(false);
                                }
                                if (S_luUnitStateID == "1")
                                    base.SetOverStiaonQTY(true);
                                ///////////////////////////////////////// mesMachine  //////////////////////////////////////////////////
                                string S_MachineID = DT_Card.Rows[0]["ID"].ToString();;
                                //mesMachineSVC.MesModMachineBySNStationTypeID(Edt_Box.Text.Trim(), List_Login.StationTypeID);
                                mesMachine v_mesMachine = new mesMachine();
                                v_mesMachine.SN = Edt_Box.Text.Trim();

                                List_mesMachine.Add(v_mesMachine);
                                /////////////////////////////////////////////////////////////////////////
                                /////////////////////////////////////////////////////////////////////////

                                mesUnit[] L_mesUnit = List_mesUnit.ToArray();
                                mesUnitDetail[] L_mesUnitDetail = List_mesUnitDetail.ToArray();
                                mesHistory[] L_mesHistory = List_mesHistory.ToArray();
                                mesSerialNumber[] L_mesSerialNumber = List_mesSerialNumber.ToArray();                                    
                                mesUnitDefect[] L_mesUnitDefect = List_mesUnitDefect.ToArray();
                                mesMachine[] L_mesMachine = List_mesMachine.ToArray();

                                string ReturnValue = DataCommitSVC.InsertALL(L_mesUnit, L_mesUnitDetail, L_mesHistory,
                                    L_mesSerialNumber, null, L_mesUnitDefect, L_mesMachine, List_Login,null);
                                if (ReturnValue != "OK")
                                {
                                    ReturnValue = "MachineSN:" + S_CardSN + "  Batch:" + S_BatchSN + ReturnValue;
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ReturnValue });
                                    return;
                                }

                                MessageInfo.Add_Info_MSG(Edt_MSG, "10006", "OK", List_Login.Language, new string[] { S_CardSN, S_BatchSN });

                                Edt_BactchSN.Text = "";
                                Edt_BactchSN.Enabled = false;

                                Edt_Box.Text = "";
                                Edt_Box.Enabled = true;
                                Edt_Box.Focus();
                                
                            }
                            else
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20050", "NG", List_Login.Language);
                                Edt_BactchSN.Focus();
                                Edt_BactchSN.Text = "";
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                }
            }
        }

        private void Btn_Refresh_Click_1(object sender, EventArgs e)
        {
            Edt_Box.Text = "";
            Edt_Box.Enabled = true;
            Edt_BactchSN.Text = "";
            Edt_BactchSN.Enabled = false;
        }
    }
}