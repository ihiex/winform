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
    public partial class Lamination_Form : FrmBase
    {
        string S_Box_SN = "";
        string S_BatchHB_SN = "";
        string S_UnitID = "";
        DataTable DT_UnitDetail = new DataTable();

        public Lamination_Form()
        {
            InitializeComponent();
        }


        private void Edt_BoxSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                S_Box_SN = Edt_BoxSN.Text.Trim();

                DataTable DT_Machine = PartSelectSVC.GetmesMachine(S_Box_SN).Tables[0];
                if (DT_Machine.Rows.Count == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20053", "NG", List_Login.Language);
                    Edt_BoxSN.SelectAll();
                    Edt_BoxSN.Text = "";
                    Edt_BoxSN.Focus();
                    return;
                }

                string S_MachineID = DT_Machine.Rows[0]["ID"].ToString();
                string S_StatusID = DT_Machine.Rows[0]["StatusID"].ToString();
                string S_MachineFamilyID = DT_Machine.Rows[0]["MachineFamilyID"].ToString();
                string S_PartID = Com_Part.EditValue.ToString();

                DataTable DT_MachineMapSN = PartSelectSVC.GetmesRouteMachineMap(S_MachineID, "").Tables[0];

                DataTable DT_MachineMapFamilyID = PartSelectSVC.GetmesRouteMachineMap("", S_MachineFamilyID).Tables[0];

                string S_MapPartID = "";
                if (DT_MachineMapSN.Rows.Count > 0)
                {
                    S_MapPartID = DT_MachineMapSN.Rows[0]["PartID"].ToString();
                }
                else if (DT_MachineMapFamilyID.Rows.Count > 0)
                {
                    S_MapPartID = DT_MachineMapFamilyID.Rows[0]["PartID"].ToString();
                }

                DataTable DT_PS = PartSelectSVC.GetmesProductStructure1(S_PartID).Tables[0];
                string S_PartID_PS = DT_PS.Rows[0]["PartID"].ToString();
                if (S_MapPartID != S_PartID_PS)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20068", "NG", List_Login.Language);
                    Edt_BoxSN.SelectAll();
                    Edt_BoxSN.Text = "";
                    Edt_BoxSN.Focus();
                    return;
                }
                /////////////////////////////////////////////////////////////////////////
                S_BatchHB_SN = "";
                DT_UnitDetail = public_.BoxSnToBatch(S_Box_SN, out S_BatchHB_SN);

                if (S_BatchHB_SN != "")
                {
                    if (S_BatchHB_SN.Substring(0, 5) == "ERROR")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(S_BatchHB_SN.Replace("ERROR", ""), List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_BatchHB_SN, ProMsg }, S_BatchHB_SN.Replace("ERROR", ""));
                        Edt_BoxSN.Text = "";
                    }
                    else
                    {
                        Edt_Batch.Text = S_BatchHB_SN;
                        Edt_BoxSN.Enabled = false;

                        Edt_SN.Enabled = true;
                        Edt_SN.Focus();
                        Edt_SN.SelectAll();

                        MessageInfo.Add_Info_MSG(Edt_MSG, "10013", "OK", List_Login.Language);
                    }
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
            if (public_.IsOneStationPrint(PartID, PartFamilyID,StationTypeID, List_Login.LineID.ToString(), Com_PO.EditValue.ToString()))
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

                string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();

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

                int I_FirstScanSequence = FirstScanSequence(S_POPartID, List_Login.StationTypeID);

                //if (Panel_Part.Visible == false)
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
                    S_POPartID = v_mesProductionOrder.PartID.ToString();
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

                string S_Sql = "";
                DataTable DT_SN = PartSelectSVC.GetmesSerialNumber(S_SN).Tables[0]; 

                if (DT_SN.Rows.Count > 0)
                {
                    S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                    DataTable DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0];
                    string S_StationID = List_Login.StationID.ToString(); 

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
                    string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, S_SN);

                    if (S_RouteCheck == "1")
                    {
                        //判断托盘数量是否超过最大限制
                        string outString = string.Empty;
                        PartSelectSVC.uspCallProcedure("uspBoxNumberCheck", Edt_BoxSN.Text.ToUpper(), null, null, null, null, null, ref outString);
                        if (outString != "OK")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(outString.Replace("ERROR", ""), List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Edt_BoxSN.Text.ToString(), ProMsg }, outString.Replace("ERROR", ""));
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                            return;
                        }

                        string S_SelectPartID = DT_Unit.Rows[0]["PartID"].ToString();
                        if (S_POPartID == S_SelectPartID)
                        {
                            string S_StationTypeID = List_Login.StationTypeID.ToString();
                            {
                                //没有子料，这里扫描过站
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
                                            PartSelectSVC.ModPO(S_POID);
                                        }

                                        /////////////////////////////////////////////////////////////////////////
                                        ////////////////////////////////   Component     //////////////////////////

                                        int I_ChildUnitID = Convert.ToInt32(DT_UnitDetail.Rows[0]["UnitID"].ToString());

                                        DataTable DT_ChildUnit = PartSelectSVC.GetComponent(I_ChildUnitID).Tables[0];
                                        string S_ChildSerialNumber = DT_ChildUnit.Rows[0]["Value"].ToString();
                                        int I_ChildPartID = Convert.ToInt32(DT_ChildUnit.Rows[0]["PartID"].ToString());
                                        int I_ChildPartFamilyID = Convert.ToInt32(DT_ChildUnit.Rows[0]["PartFamilyID"].ToString());

                                        mesUnitComponent v_mesUnitComponent = new mesUnitComponent();
                                        v_mesUnitComponent.UnitID = v_mesUnit.ID;
                                        v_mesUnitComponent.UnitComponentTypeID = 1;
                                        v_mesUnitComponent.ChildUnitID = I_ChildUnitID;
                                        v_mesUnitComponent.ChildSerialNumber = S_ChildSerialNumber;

                                        v_mesUnitComponent.ChildLotNumber = S_BatchHB_SN;
                                        v_mesUnitComponent.ChildPartID = I_ChildPartID;
                                        v_mesUnitComponent.ChildPartFamilyID = I_ChildPartFamilyID;
                                        v_mesUnitComponent.Position = "";

                                        v_mesUnitComponent.InsertedTime = DateTime.Now;
                                        v_mesUnitComponent.StatusID = 1;
                                        v_mesUnitComponent.LastUpdate = DateTime.Now;

                                        string S_UC_Result = mesUnitComponentSVC.Insert(v_mesUnitComponent);


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

                                        /////////////////////////////////////////// mesMachine  (2019-11-06 修改按钮执行此代码)//////////////////////////////////////////////////
                                        if (S_luUnitStateID == "1")
                                        {
                                            base.SetOverStiaonQTY(true);
                                        }
                                        else
                                        {
                                            base.SetOverStiaonQTY(false);
                                        }

                                        Edt_SN.Text = "";
                                        Edt_SN.Enabled = true;
                                        Edt_SN.Focus();

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