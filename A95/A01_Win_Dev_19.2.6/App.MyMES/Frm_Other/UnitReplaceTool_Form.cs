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
    public partial class UnitReplaceTool_Form : FrmBase
    {
        string S_UnitID = "";

        public UnitReplaceTool_Form()
        {
            InitializeComponent();
        }

        private void Edt_BuckSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string S_BuckSN = Edt_BuckSN.Text.Trim();
                //调用通用过程               
                string xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                string outString = string.Empty;
                PartSelectSVC.uspCallProcedure("uspMachineToolingCheck", S_BuckSN, null, xmlPart, null, null, List_Login.StationTypeID.ToString(), ref outString);
                if (outString == "1")
                {
                    Edt_MSG.Text = "";
                    Edt_SN.Enabled = true;
                    Edt_BuckSN.Enabled = false;
                    Edt_SN.Focus();
                    Edt_SN.SelectAll();
                }
                else
                {
                    string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_BuckSN, ProMsg }, outString);
                    Edt_BuckSN.SelectAll();
                    Edt_BuckSN.Text = "";
                    Edt_BuckSN.Focus();
                    return;
                }
            }
        }

        /// <summary>
        /// 第一站 扫描顺序号
        /// </summary>
        /// <param name="PartID"></param>
        /// <param name="StationTypeID"></param>
        /// <returns></returns>
        private int FirstScanSequence(string PartID, int StationTypeID)
        {
            int I_Result = 1;
            string PartFamilyID = Com_PartFamily.EditValue.ToString();
            if (public_.IsOneStationPrint(PartID, PartFamilyID, StationTypeID, 
                List_Login.LineID.ToString(), Com_PO.EditValue.ToString()))
            {
                I_Result = 2;
            }
            return I_Result;
        }

        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DateTime dateStart = DateTime.Now;

                string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                string PartFamilyID = Com_PartFamily.EditValue.ToString();
                string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID,
                    List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());
                if (string.IsNullOrEmpty(S_UnitStateID))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
                    return;
                }

                // luUnitStatus  ID
                //DataRowView DRV_luUnitStatus = Com_luUnitStatus.SelectedItem as DataRowView;
                string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();

                string S_DefectID = base.DefectChar;
                string S_SN = Edt_SN.Text.Trim();
                string S_BuckSN = Edt_BuckSN.Text.Trim();
                //工单  PartID
                string S_POPartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();

                if (S_SN == "")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }

                if (S_BuckSN == S_SN)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20069", "NG", List_Login.Language);
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

                int I_FirstScanSequence = FirstScanSequence(S_POPartID, List_Login.StationTypeID);

                {
                    List<string> List_PO = public_.SnToPOID(S_SN);
                    string S_POID = List_PO[0];

                    if (S_POID.Length > 4)
                    {
                        if (S_POID.Substring(0, 5) == "ERROR")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(S_POID.Replace("ERROR", ""), List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, S_POID.Replace("ERROR", ""));
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

                    S_POPartID = v_mesProductionOrder.PartID.ToString();   //DT_PO.Rows[0]["PartID"].ToString();
                    I_FirstScanSequence = FirstScanSequence(S_POPartID, List_Login.StationTypeID);

                    string S_ComPO = Com_PO.EditValue.ToString();
                    if (S_POID != S_ComPO)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20015", "NG", List_Login.Language, new string[] { S_SN });
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }

                }

                DataTable DT_SN = PartSelectSVC.GetmesSerialNumber(S_SN).Tables[0]; //public_.P_DataSet(S_Sql).Tables[0];  

                if (DT_SN.Rows.Count > 0)
                {
                    S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                    DataTable DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0];

                    string S_StationID = List_Login.StationID.ToString();    //DT_Unit.Rows[0]["StationID"].ToString();

                    //string S_SerialNumberType = DT_SN.Rows[0]["SerialNumberTypeID"].ToString();
                    //if (S_SerialNumberType != "5")
                    //{
                    //    MessageInfo.Add_Info_MSG(Edt_MSG, "20035", "NG", List_Login.Language);
                    //    Edt_SN.Focus();
                    //    Edt_SN.Text = "";
                    //    return;
                    //}

                    if (DT_Unit.Rows[0]["StatusID"].ToString() != "1")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20036", "NG", List_Login.Language);
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }

                    ////////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////////
                    string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID,
                                          List_Login.LineID.ToString(), DT_Unit, S_SN);

                    if (S_RouteCheck == "1")
                    {
                        string S_SelectPartID = DT_Unit.Rows[0]["PartID"].ToString();
                        if (S_POPartID == S_SelectPartID)
                        {
                            // 本工位不判断 子料,不需要判断BOM
                            //string S_StationTypeID = List_Login.StationTypeID.ToString();

                            //DT_ProductStructure = PartSelectSVC.GetmesProductStructure2(S_POPartID, S_StationTypeID).Tables[0];   
                            //if (DT_ProductStructure.Rows.Count > 0)
                            //{
                            //    Edt_SN.Enabled = false;
                            //    Edt_BuckSN.Enabled = true;
                            //    Edt_BuckSN.Focus();
                            //}
                            //else
                            {
                                //本工位不判断 子料    
                                try
                                {
                                    string S_POID = DT_ProductionOrder.Rows[0]["ID"].ToString();
                                    DataTable DT_UPUnitID = PartSelectSVC.Get_UnitID(S_SN).Tables[0];

                                    mesUnit v_mesUnit = new mesUnit();
                                    v_mesUnit.ID = Convert.ToInt32(DT_UPUnitID.Rows[0]["UnitID"].ToString());
                                    v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                                    v_mesUnit.StatusID = Convert.ToInt32(S_luUnitStateID);
                                    v_mesUnit.StationID = List_Login.StationID;
                                    v_mesUnit.EmployeeID = List_Login.EmployeeID;
                                    v_mesUnit.LastUpdate = DateTime.Now;
                                    v_mesUnit.ProductionOrderID = Convert.ToInt32(S_POID);
                                    //修改 Unit
                                    string S_UpdateUnit = mesUnitSVC.Update(v_mesUnit);
                                    if (S_UpdateUnit.Substring(0, 1) == "E")
                                    {
                                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_UpdateUnit });
                                        return;
                                    }

                                    if (S_UpdateUnit.Substring(0, 1) != "E")
                                    {
                                        //虚拟  Buck  SN 
                                        string S_xmlPart = "'<BoxSN SN=" + "\"" + S_BuckSN + "\"" + "> </BoxSN>'";
                                        //string S_FormatSN = Com_PartFamilyType.Text.Substring(0, 3) + "_Buck";
                                        string S_FormatSN = PartSelectSVC.mesGetSNFormatIDByList(S_PartID, Com_PartFamily.EditValue.ToString(),
                                            List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), List_Login.StationTypeID.ToString());
                                        if (string.IsNullOrEmpty(S_FormatSN))
                                        {
                                            MessageInfo.Add_Info_MSG(Edt_MSG, "20034", "NG", List_Login.Language, new string[] { S_PartID });
                                            return;
                                        }
                                        DataSet DS = PartSelectSVC.uspSNRGetNext(S_FormatSN, 0, null, S_xmlPart, null, null, null);
                                        DataTable DT = DS.Tables[1];
                                        string S_BuckVirtualSN = DT.Rows[0][0].ToString();
                                        //虚拟  Buck  mesUnit
                                        mesUnit v_mesUnit_Buck = new mesUnit();
                                        v_mesUnit_Buck.UnitStateID = Convert.ToInt32(S_UnitStateID);
                                        v_mesUnit_Buck.StatusID = Convert.ToInt32(S_luUnitStateID);
                                        v_mesUnit_Buck.StationID = List_Login.StationID;
                                        v_mesUnit_Buck.EmployeeID = List_Login.EmployeeID;
                                        v_mesUnit_Buck.CreationTime = DateTime.Now;
                                        v_mesUnit_Buck.LastUpdate = DateTime.Now;

                                        v_mesUnit_Buck.PanelID = 0;
                                        v_mesUnit_Buck.LineID = List_Login.LineID;
                                        v_mesUnit_Buck.ProductionOrderID = Convert.ToInt32(S_POID);

                                        v_mesUnit_Buck.RMAID = 0;
                                        v_mesUnit_Buck.PartID = Convert.ToInt32(S_PartID);
                                        v_mesUnit_Buck.LooperCount = 1;

                                        //insert Buck Unit                                
                                        string S_InsertBuckUnit = mesUnitSVC.Insert(v_mesUnit_Buck);
                                        if (S_InsertBuckUnit.Substring(0, 1) == "E")
                                        {
                                            MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_InsertBuckUnit });
                                            return;
                                        }
                                        v_mesUnit_Buck.ID = Convert.ToInt32(S_InsertBuckUnit);

                                        string S_InsertUnitDetail = "";
                                        try
                                        {
                                            //虚拟 Buck SN mesSerialNumber
                                            mesSerialNumber v_mesSerialNumber_Buck = new mesSerialNumber();
                                            v_mesSerialNumber_Buck.UnitID = Convert.ToInt32(S_InsertBuckUnit);
                                            v_mesSerialNumber_Buck.SerialNumberTypeID = 9;
                                            v_mesSerialNumber_Buck.Value = S_BuckVirtualSN;
                                            int I_InsertSN = mesSerialNumberSVC.Insert(v_mesSerialNumber_Buck);

                                            //////////////////////////////////////  mesUnitDetail ////////////////////////////////    
                                            mesUnitDetail v_mesUnitDetail_Buck = new mesUnitDetail();
                                            v_mesUnitDetail_Buck.UnitID = Convert.ToInt32(v_mesUnit_Buck.ID);
                                            v_mesUnitDetail_Buck.reserved_01 = S_BuckSN;
                                            v_mesUnitDetail_Buck.reserved_02 = "";     //Batch
                                            v_mesUnitDetail_Buck.reserved_03 = "1";
                                            v_mesUnitDetail_Buck.reserved_04 = S_SN;   //产品 SN 
                                            v_mesUnitDetail_Buck.reserved_05 = "";

                                            ///// webservice
                                            S_InsertUnitDetail = mesUnitDetailSVC.Insert(v_mesUnitDetail_Buck);

                                            /////////////////////////////////// 修改设备表  使用次数  /////////////////////                                           
                                            mesMachineSVC.SetMachineRuningQuantity(S_BuckSN);
                                            /////////////////////////////////// 修改设备表  设备状态  /////////////////////
                                            DataTable DT_Machine = PartSelectSVC.GetmesMachine(S_BuckSN).Tables[0];
                                            string S_MachineID = DT_Machine.Rows[0]["Id"].ToString();
                                            PartSelectSVC.ModMachine2(S_MachineID, "2");

                                        }
                                        catch (Exception ex)
                                        {
                                            MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                        }

                                        /////////////////////////////////////////////////////////////////////////
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
                                        int I_InsertHistory = mesHistorySVC.Insert(v_mesHistory);

                                        //修改工单状态
                                        string S_PO_StatusID = DT_ProductionOrder.Rows[0]["StatusID"].ToString();
                                        if (S_PO_StatusID == "1")
                                        {
                                            //S_Sql = "Update mesProductionOrder set StatusID=2,LastUpdate='" + DateTime.Now.ToString() +
                                            //        "' where ID='" + S_POID + "'";
                                            //public_.ExecSql(S_Sql);
                                            //PartSelectSVC.ModPO(S_POID); 
                                        }

                                        //////////////////////////////////////  mesUnitComponent ////////////////////////////////
                                        mesUnitComponent v_mesUnitComponent = new mesUnitComponent();
                                        v_mesUnitComponent.UnitID = v_mesUnit.ID;
                                        v_mesUnitComponent.UnitComponentTypeID = 1;
                                        v_mesUnitComponent.ChildUnitID = Convert.ToInt32(S_InsertBuckUnit);
                                        v_mesUnitComponent.ChildSerialNumber = S_BuckVirtualSN;
                                        v_mesUnitComponent.ChildLotNumber = "";
                                        DataSet dtsParts = PartSelectSVC.GetmesMachine(Edt_BuckSN.Text.Trim());
                                        int partID = Convert.ToInt32(dtsParts.Tables[0].Rows[0]["PartID"].ToString());
                                        mesPart part = mesPartSVC.Get(partID);
                                        v_mesUnitComponent.ChildPartID = partID;
                                        v_mesUnitComponent.ChildPartFamilyID = part.PartFamilyID;
                                        v_mesUnitComponent.Position = "";
                                        v_mesUnitComponent.InsertedEmployeeID = List_Login.EmployeeID;
                                        v_mesUnitComponent.InsertedStationID = List_Login.StationID;
                                        v_mesUnitComponent.StatusID = 1;

                                        string S_UC_Result = mesUnitComponentSVC.Insert(v_mesUnitComponent);
                                        if (S_UC_Result != "OK")
                                        {
                                            MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_UC_Result });
                                            return;
                                        }

                                        //////////////////////////  Defect ///////////////////////////////////////////
                                        //////////////////////////////////////////////////////////////////////////////
                                        string[] Array_Defect = S_DefectID.Split(';');
                                        if (S_luUnitStateID != "1")
                                        {
                                            foreach (var item in Array_Defect)
                                            {
                                                string S_mesUnitDefectInsert = "";
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

                                                        S_mesUnitDefectInsert = mesUnitDefectSVC.Insert(v_mesUnitDefect);
                                                    }
                                                }
                                                catch(Exception ex)
                                                {
                                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                                }
                                            }
                                        }
                                        /////////////////////////////////////////////////////////////////////////
                                        /////////////////////////////////////////////////////////////////////////
                                        Edt_SN.Text = "";
                                        Edt_SN.Enabled = false;

                                        //Edt_DefectID.Text = "";

                                        Edt_BuckSN.Text = "";
                                        Edt_BuckSN.Enabled = true;
                                        Edt_BuckSN.Focus();

                                        TimeSpan ts = DateTime.Now - dateStart;
                                        double mill = Math.Round(ts.TotalMilliseconds, 0);
                                        MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { S_SN, mill.ToString() });
                                    }
                                }
                                finally
                                {
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
                        string ProMsg = MessageInfo.GetMsgByCode(S_RouteCheck, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Edt_SN.Text.ToString(), ProMsg }, S_RouteCheck);
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
    }
}