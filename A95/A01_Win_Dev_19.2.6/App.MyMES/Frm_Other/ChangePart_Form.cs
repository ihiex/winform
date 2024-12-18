﻿using System;
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
    public partial class ChangePart_Form : FrmBase
    {
        string S_UnitID = "";
        bool COF = false;
        string S_IsChangePO = "";
        string S_IsChangePN = "";
        string S_ChangedUnitStateID = "";

        public ChangePart_Form()
        {
            InitializeComponent();
        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            Edt_SN.Enabled = false;
            base.Btn_Defect.Visible = false;
            Edt_SN.Enabled = false;           
        }

        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            base.Btn_ConfirmPO_Click(sender, e);
            Edt_SN.Enabled = false;
            Edt_SN.Focus();

            Panel_Target.Enabled = true;

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

                if (S_IsChangePN != "1" && S_IsChangePO != "1")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG,"NG", "please setup 'S_IsChangePN' or 'S_IsChangePO' in the station config.");
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }


                string outString = "";
                PartSelectSVC.uspCallProcedure("uspPONumberCheck", Com_POTarget.EditValue.ToString(),
                                null, null, null, null, "1", ref outString);
                if (outString != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Edt_SN.Text.Trim(), ProMsg }, outString);
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }

                DateTime dateStart = DateTime.Now;

                string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                string PartFamilyID = Com_PartFamily.EditValue.ToString();
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
                    string S_StationID = List_Login.StationID.ToString();

                    if (!COF)
                    {
                        if (DT_Unit.Rows[0]["StatusID"].ToString() != "1")
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20036", "NG", List_Login.Language);
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                            return;
                        }
                    }

                    //string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, S_SN);
                    //if (S_RouteCheck == "1")
                    {
                        string S_SelectPartID = DT_Unit.Rows[0]["PartID"].ToString();
                        string S_UnitStateID= DT_Unit.Rows[0]["UnitStateID"].ToString();
                        string S_StationID_Old=DT_Unit.Rows[0]["StationID"].ToString();
                        string S_StatusID=DT_Unit.Rows[0]["StatusID"].ToString();

                        if (S_POPartID == S_SelectPartID)
                        {
                            try
                            {
                                //调用通用过程
                                //string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>";
                                //string xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                                //string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                                //string outString = string.Empty;
                                //PartSelectSVC.uspCallProcedure("uspQCCheck", S_SN,
                                //                                                        xmlProdOrder, xmlPart, xmlStation, null, null, ref outString);
                                //if (outString != "1")
                                //{
                                //    string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                                //    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, outString);
                                //    Edt_SN.Focus();
                                //    Edt_SN.Text = "";
                                //    return;
                                //}

                                //string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID,
                                //    List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());
                                //if (string.IsNullOrEmpty(S_UnitStateID))
                                //{
                                //    MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
                                //    Edt_SN.Focus();
                                //    Edt_SN.Text = "";
                                //    return;
                                //}

                                string S_PartIDTarget = Com_PartTarget.EditValue.ToString();
                                string S_POIDTarget = Com_POTarget.EditValue.ToString();  

                                DataTable DT_UPUnitID = PartSelectSVC.Get_UnitID(S_SN).Tables[0];

                                mesUnit v_mesUnit = new mesUnit();
                                v_mesUnit.ID = Convert.ToInt32(DT_UPUnitID.Rows[0]["UnitID"].ToString());
                                v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                                v_mesUnit.StatusID = Convert.ToInt32(S_StatusID);;
                                v_mesUnit.StationID = Convert.ToInt32(S_StationID_Old);    //List_Login.StationID;
                                v_mesUnit.EmployeeID = List_Login.EmployeeID;
                                v_mesUnit.LastUpdate = DateTime.Now;

                                v_mesUnit.ProductionOrderID = Convert.ToInt32(S_POID);
                                if (S_IsChangePO == "1")
                                {
                                    v_mesUnit.ProductionOrderID = Convert.ToInt32(S_POIDTarget);
                                }

                                v_mesUnit.PartID = Convert.ToInt32(S_PartID);
                                if (S_IsChangePN == "1")
                                {
                                    v_mesUnit.ProductionOrderID = Convert.ToInt32(S_POIDTarget);
                                    v_mesUnit.PartID = Convert.ToInt32(S_PartIDTarget); 
                                }

                                //修改 Unit
                                string S_UpdateUnit = mesUnitSVC.UpdatePart(v_mesUnit);
                                if (S_UpdateUnit.Substring(0, 1) == "E")
                                {
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_UpdateUnit });
                                    return;
                                }

                                if (S_UpdateUnit.Substring(0, 1) != "E")
                                {
                                    mesHistory v_mesHistory = new mesHistory();

                                    v_mesHistory.UnitID = Convert.ToInt32(v_mesUnit.ID);
                                    v_mesHistory.UnitStateID = Convert.ToInt32(S_ChangedUnitStateID);
                                    v_mesHistory.EmployeeID = List_Login.EmployeeID;
                                    v_mesHistory.StationID = List_Login.StationID;
                                    v_mesHistory.EnterTime = DateTime.Now;
                                    v_mesHistory.ExitTime = DateTime.Now;

                                    v_mesHistory.ProductionOrderID = v_mesUnit.ProductionOrderID; 
                                    v_mesHistory.PartID = Convert.ToInt32(S_PartIDTarget);


                                    v_mesHistory.LooperCount = 1;
                                    int I_InsertHistory = mesHistorySVC.Insert(v_mesHistory);
                                    base.SetOverStiaonQTY(true);

                                    Edt_SN.Text = "";
                                    Edt_SN.Enabled = true;
                                    Edt_SN.Focus();

                                    TimeSpan ts = DateTime.Now - dateStart;
                                    double mill = Math.Round(ts.TotalMilliseconds, 0);
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { S_SN, mill.ToString() });
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                            }
                        }
                        else
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20050", "NG", List_Login.Language);
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                        }
                    }
                    //else
                    //{
                    //    if (S_RouteCheck == "20243")
                    //    {
                    //        string nowUnitStateID = PartSelectSVC.GetSerialNumber2(S_SN).Tables[0].Rows[0]["UnitStateID"].ToString();
                    //        mesUnitState mesunitSateModel = mesUnitStateSVC.Get(Convert.ToInt32(nowUnitStateID));
                    //        MessageInfo.Add_Info_MSG(Edt_MSG, "20243", "NG", List_Login.Language, new string[] { S_SN, mesunitSateModel.Description });
                    //    }
                    //    else
                    //    {
                    //        string ProMsg = MessageInfo.GetMsgByCode(S_RouteCheck, List_Login.Language);
                    //        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_RouteCheck);
                    //        //MessageInfo.Add_Info_MSG(Edt_MSG, "20243", "NG", List_Login.Language, new string[] { S_SN, mesunitSateModel.Description + " [PartName]:" + dsMainSN.Tables[0].Rows[0]["PartNumber"].ToString() + " [LineName]:" + dsMainSN.Tables[0].Rows[0]["LineName"].ToString() });
                    //    }
                    //    Edt_SN.Focus();
                    //    Edt_SN.Text = "";
                    //}
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language, new string[] { S_SN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                }
            }
        }

        public override void FrmBase_Load(object sender, EventArgs e)
        {
            base.FrmBase_Load(sender, e);

            public_.AddPartFamilyType(Com_PartFamilyTypeTarget, Grid_PartFamilyTypeTarget);
        }

        private void Com_PartFamilyTypeTarget_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartFamilyTypeID = Com_PartFamilyTypeTarget.EditValue.ToString();
                public_.AddPartFamily(Com_PartFamilyTarget, S_PartFamilyTypeID, Grid_PartFamilyTarget);
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void Com_PartFamilyTarget_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartFamilyID = Com_PartFamilyTarget.EditValue.ToString();
                public_.AddPart(Com_PartTarget, S_PartFamilyID, Grid_PartTarget);
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void Com_PartTarget_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartID = Com_PartTarget.EditValue.ToString();
                public_.AddPOAll(Com_POTarget, S_PartID, List_Login.LineID.ToString(), Grid_POTarget);
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void Btn_ConfirmPOTarget_Click(object sender, EventArgs e)
        {
            DataSet DS=  PartSelectSVC.GetPLCSeting("", List_Login.StationID.ToString());
            DataTable DT = DS.Tables[0];

            if (DT.Rows.Count > 1)
            {
                DataRow[] DR = DT.Select("Name = 'IsChangePO'");                
                S_IsChangePO =DR[0]["Value"].ToString();

                DR = DT.Select("Name = 'IsChangePN'");
                S_IsChangePN = DR[0]["Value"].ToString();

                DR = DT.Select("Name = 'ChangedUnitStateID'");
                S_ChangedUnitStateID = DR[0]["Value"].ToString();
            }


            if (Com_PartFamilyTypeTarget.EditValue == null || string.IsNullOrEmpty(Com_PartFamilyTypeTarget.Text.ToString()))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20115", "NG", List_Login.Language);
                return;
            }
            if (Com_PartFamilyTarget.EditValue == null || string.IsNullOrEmpty(Com_PartFamilyTarget.Text.ToString()))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20116", "NG", List_Login.Language);
                return;
            }
            if (Com_PartTarget.EditValue == null || string.IsNullOrEmpty(Com_PartTarget.Text.ToString()))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20117", "NG", List_Login.Language);
                return;
            }
            if ((Com_POTarget.EditValue == null || string.IsNullOrEmpty(Com_POTarget.Text.ToString())) && S_IsChangePO=="1")
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20118", "NG", List_Login.Language);
                return;
            }

            Edt_SN.Enabled = true;
            Btn_ConfirmPOTarget.Enabled = false;  
            Panel_Target.Enabled = false;  
        }

        private void Btn_RefreshTarget_Click(object sender, EventArgs e)
        {
            Panel_Target.Enabled = true;
            Btn_ConfirmPOTarget.Enabled = true;            
        }
    }
}