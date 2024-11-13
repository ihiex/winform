using App.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.MyMES.PartSelectService;
using App.MyMES.mesUnitService;
using App.MyMES.mesSerialNumberService;
using App.MyMES.mesHistoryService;
using App.MyMES.mesProductionOrderService;
using App.MyMES.mesProductStructureService;
using App.MyMES.mesUnitComponentService;
using App.MyMES.mesUnitDefectService;
using App.MyMES.mesUnitDetailService;
using System.Text.RegularExpressions;
using App.MyMES.mesPartService;
using App.MyMES.mesMachineService;

namespace App.MyMES
{
    public partial class UnitReplaceTool_Form : Form
    {
        Public_ public_ = new Public_();
        DataTable DT_ProductionOrder = new DataTable();
        DataTable DT_ProductStructure = new DataTable();
        LoginList List_Login = new LoginList();
        string Batch_Pattern = string.Empty;

        int I_RouteSequence = -9;  //当前工序顺序 
        int I_POID = 0;
        string S_DefectTypeID = "";

        PartSelectSVCClient PartSelectSVC;
        ImesUnitSVCClient mesUnitSVC;
        ImesSerialNumberSVCClient mesSerialNumberSVC;
        ImesHistorySVCClient mesHistorySVC;
        ImesUnitComponentSVCClient mesUnitComponentSVC;
        ImesUnitDefectSVCClient mesUnitDefectSVC;
        ImesPartSVCClient mesPartSVC;
        ImesProductionOrderSVCClient mesProductionOrderSVC;
        ImesUnitDetailSVCClient mesUnitDetailSVC;
        ImesMachineSVCClient mesMachineSVC;



        public UnitReplaceTool_Form()
        {
            InitializeComponent();
        }

        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            try
            {
                PartSelectSVC.Close();
                mesUnitSVC.Close();
                mesSerialNumberSVC.Close();
                mesHistorySVC.Close();
                mesUnitComponentSVC.Close();
                mesUnitDefectSVC.Close();
                mesPartSVC.Close();
                mesProductionOrderSVC.Close();
                mesUnitDetailSVC.Close();
                mesMachineSVC.Close(); 
            }
            catch
            { }

            PartSelectSVC = PartSelectFactory.CreateServerClient();
            mesUnitSVC = ImesUnitFactory.CreateServerClient();
            mesSerialNumberSVC = ImesSerialNumberFactory.CreateServerClient();
            mesHistorySVC = ImesHistoryFactory.CreateServerClient();
            mesUnitComponentSVC = ImesUnitComponentFactory.CreateServerClient();
            mesUnitDefectSVC = ImesUnitDefectFactory.CreateServerClient();
            mesPartSVC = ImesPartFactory.CreateServerClient();
            mesProductionOrderSVC = ImesProductionOrderFactory.CreateServerClient();
            mesUnitDetailSVC = ImesUnitDetailFactory.CreateServerClient();
            mesMachineSVC = ImesMachineFactory.CreateServerClient();


            List_Login = this.Tag as LoginList;
            ///////////////////////////
            public_.AddStationType(Com_StationType, Grid_StationType);
            public_.AddLine(Com_Line, Grid_Line);

            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_LineID = List_Login.LineID.ToString();
            public_.AddStation(Com_Station, S_LineID, Grid_Station);

            Com_StationType.Text = S_StationTypeID;
            Com_Line.Text = S_LineID;
            Com_Station.Text = List_Login.StationID.ToString();

            public_.AddPartFamilyType(Com_PartFamilyType, Grid_PartFamilyType);
            public_.AddluUnitStatus(Com_luUnitStatus, Grid_luUnitStatus);

            //DataRowView DRV = Com_Part.SelectedItem as DataRowView;
            string S_PartID = Com_Part.EditValue.ToString();   //DRV["ID"].ToString();
            public_.AddPOAll(Com_PO, S_PartID, List_Login.LineID.ToString(), Grid_PO);

            Edt_BuckSN.Focus();
            Edt_BuckSN.SelectAll();

            Edt_MSG.Text = "";
            Edt_BuckSN.Text = "";
            Edt_BuckSN.Enabled = true;

            Edt_SN.Text = "";

            PartEdtStatus(true);

        }

        /// <summary>
        /// Part  编辑框状态
        /// </summary>
        /// <param name="B_Status"></param>
        private void PartEdtStatus(Boolean B_Status)
        {
            Com_PartFamilyType.Enabled = B_Status;
            Com_PartFamily.Enabled = B_Status;
            Com_Part.Enabled = B_Status;
            Com_Part.Enabled = B_Status;
            Com_PO.Enabled = B_Status;
            Btn_ConfirmPO.Enabled = B_Status;

            if (B_Status == true)
            {
                Com_PartFamilyType.BackColor = Color.White;
                Com_PartFamily.BackColor = Color.White;
                Com_Part.BackColor = Color.White;
                Com_PO.BackColor = Color.White;

                Edt_SN.Enabled = false;
                Edt_BuckSN.Enabled = false;
            }
            else
            {
                Com_PartFamilyType.BackColor = Color.Yellow;
                Com_PartFamily.BackColor = Color.Yellow;
                Com_Part.BackColor = Color.Yellow;
                Com_PO.BackColor = Color.Yellow;

                Edt_BuckSN.Enabled = true;
                Edt_SN.Enabled = false;
                
            }

            Edt_BuckSN.Focus();
            Edt_BuckSN.SelectAll();

            Edt_SN.Text = "";
            Edt_BuckSN.Text = "";
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
            if (public_.IsOneStationPrint(PartID, StationTypeID, List_Login.LineID.ToString()))
            {
                I_Result = 2;
            }
            return I_Result;
        }

        private void Get_DTPO(string S_POID)
        {
            I_POID = Convert.ToInt32(S_POID);
            //string S_Sql = "select *  from mesProductionOrder where ID='" + S_POID + "'";
            DT_ProductionOrder = PartSelectSVC.GetProductionOrder(S_POID).Tables[0];   //public_.P_DataSet(S_Sql).Tables[0];
            //mesProductionOrder v_mesProductionOrder= mesProductionOrderSVC.Get(Convert.ToInt32(S_POID));

            Edt_OrderQuantity.Text = DT_ProductionOrder.Rows[0]["OrderQuantity"].ToString();
            //GetScanQuantity();
        }

        private void Btn_Unlock_Click(object sender, EventArgs e)
        {
            PartEdtStatus(true);
        }

        private void BoxLinkBatch_Form_Load(object sender, EventArgs e)
        {
            try
            {
                Btn_Refresh_Click(sender, e);

                string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                I_RouteSequence = public_.RouteSequence(S_PartID, List_Login.StationTypeID, List_Login.LineID.ToString());
                int I_FirstScanSequence = FirstScanSequence(S_PartID, List_Login.StationTypeID);

                Com_PO.Enabled = true;
                Edt_SN.Enabled = false;
                if (I_RouteSequence > I_FirstScanSequence)
                {
                    Com_PO.Enabled = false;
                    Com_PO.BackColor = Color.Yellow;

                    Edt_BuckSN.Enabled = true;
                    Btn_Unlock.Enabled = false;
                    Btn_Refresh.Enabled = false;
                    Btn_ConfirmPO.Enabled = false;

                    Panel_Part.Visible = false;
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void CloseDefect()
        {
            try
            {
                foreach (Form frm in Panel_Defact.Controls)
                {
                    frm.Close();
                }
                Edt_DefectID.Text = "";
            }
            catch { }
        }

        private void Btn_Defect_Click(object sender, EventArgs e)
        {
            CloseDefect();

            DefectForm v_DefectForm = new DefectForm();
            //v_DefectForm.Show_DefectForm(v_DefectForm, Edt_DefectID);
            v_DefectForm.Show_DefectFormToPanel(v_DefectForm, Edt_DefectID, Convert.ToInt32(S_DefectTypeID));

            v_DefectForm.FormBorderStyle = FormBorderStyle.None;
            v_DefectForm.Width = Panel_Defact.Width;
            v_DefectForm.Height = Panel_Defact.Height;
            v_DefectForm.TopLevel = false;
            v_DefectForm.Parent = Panel_Defact;
            v_DefectForm.BringToFront();
            // v_DefectForm.Tag = F_LoginList;
            v_DefectForm.Show();
        }


        private void UnitReplaceTool_Form_Resize(object sender, EventArgs e)
        {
            try
            {
                foreach (Form frm in Panel_Defact.Controls)
                {
                    frm.Width = Panel_Defact.Width;
                    frm.Height = Panel_Defact.Height;
                }
            }
            catch { }
        }

        private void Com_PartFamilyType_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartFamilyTypeID = Com_PartFamilyType.EditValue.ToString();
                public_.AddPartFamily(Com_PartFamily, S_PartFamilyTypeID, Grid_PartFamily);

                S_DefectTypeID = public_.GetDefectTypeID(Com_PartFamilyType.Text.Trim());
            }
            catch
            { }
        }

        private void Com_PartFamily_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                public_.AddPart(Com_Part, S_PartFamilyID, Grid_Part);
            }
            catch
            { }
        }

        private void Com_Part_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartID = Com_Part.EditValue.ToString();
                public_.AddPOAll(Com_PO, S_PartID, List_Login.LineID.ToString(), Grid_PO);
            }
            catch
            { }
        }

        private void Com_PO_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_POID = Com_PO.EditValue.ToString();

                Get_DTPO(S_POID);
            }
            catch
            { }
        }

        private void Com_luUnitStatus_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();
                string S_Description = Com_luUnitStatus.Text.Trim();

                if (S_luUnitStateID != "1")
                {
                    Btn_Defect.Enabled = true;

                    Btn_Defect_Click(sender, e);
                    Lab_NG.Visible = true;
                    Lab_NG.Text = "单元状态：" + S_Description;
                }
                else
                {
                    Btn_Defect.Enabled = false;
                    Lab_NG.Visible = false;

                    CloseDefect();
                }
            }
            catch
            { }
        }

        private void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            if (Com_PO.Text.Trim() == "")
            {
                Public_.Add_Info_MSG(Edt_MSG, "工单不能为空!", "NG");
            }
            else
            {
                PartEdtStatus(false);

                Edt_BuckSN.Focus();
                Edt_BuckSN.SelectAll();

                if (!string.IsNullOrEmpty(Com_Part.EditValue.ToString()))
                {
                    int I_PartID = Convert.ToInt32(Com_Part.EditValue.ToString());
                    //PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                    DataSet dataSet = PartSelectSVC.GetmesPartDetail(I_PartID, "Batch_Pattern");
                    if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        Batch_Pattern = dataSet.Tables[0].Rows[0]["Content"].ToString();
                    }
                    else
                    {
                        Public_.Add_Info_MSG(Edt_MSG, " 未配置校验批次正则表达式！", "NG");
                    }
                    //PartSelectSVC.Close();
                }
            }
        }

        private void Edt_BuckSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string S_BuckSN = Edt_BuckSN.Text.Trim();
                //调用通用过程               
                string xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                string outString = string.Empty;
                PartSelectSVC.uspCallProcedure("uspMachineToolingCheck", S_BuckSN,null, xmlPart,null, null, List_Login.StationTypeID.ToString(), ref outString);
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
                    Public_.Add_Info_MSG(Edt_MSG, outString, "NG");
                    Edt_BuckSN.SelectAll();
                    Edt_BuckSN.Text = "";
                    Edt_BuckSN.Focus();
                    return;
                }
            }

            //if (e.KeyCode == Keys.Enter)
            //{
            //    string S_BuckSN = Edt_BuckSN.Text.Trim();

            //    DataTable DT_Machine = PartSelectSVC.GetmesMachine(S_BuckSN).Tables[0];
            //    if (DT_Machine.Rows.Count == 0)
            //    {
            //        Public_.Add_Info_MSG(Edt_MSG, S_BuckSN + " 无此治具编码SN!", "NG");
            //        Edt_BuckSN.SelectAll();
            //        Edt_BuckSN.Text = "";
            //        Edt_BuckSN.Focus();
            //        return;
            //    }

            //    string S_MachineID = DT_Machine.Rows[0]["ID"].ToString();
            //    string S_StatusID = DT_Machine.Rows[0]["StatusID"].ToString();
            //    string S_MachineFamilyID = DT_Machine.Rows[0]["MachineFamilyID"].ToString();
            //    string S_MaxUseQuantity=DT_Machine.Rows[0]["MaxUseQuantity"].ToString();
            //    string S_RuningQuantity=DT_Machine.Rows[0]["RuningQuantity"].ToString();

            //    int I_MaxUseQuantity=Convert.ToInt32(S_MaxUseQuantity);
            //    int I_RuningQuantity = S_RuningQuantity == "" ? 0 : Convert.ToInt32(S_RuningQuantity);

            //    string S_PartID = Com_Part.EditValue.ToString();

            //    DataTable DT_MachineMapSN = PartSelectSVC.GetmesRouteMachineMap(S_MachineID, "").Tables[0];
            //    DataTable DT_MachineMapFamilyID = PartSelectSVC.GetmesRouteMachineMap("", S_MachineFamilyID).Tables[0];

            //    string S_MapPartID = "";
            //    if (DT_MachineMapSN.Rows.Count > 0)
            //    {
            //        S_MapPartID = DT_MachineMapSN.Rows[0]["PartID"].ToString();
            //    }
            //    else if (DT_MachineMapFamilyID.Rows.Count > 0)
            //    {
            //        S_MapPartID = DT_MachineMapFamilyID.Rows[0]["PartID"].ToString();
            //    }

            //    if (I_RuningQuantity > I_MaxUseQuantity)
            //    {
            //        Public_.Add_Info_MSG(Edt_MSG, S_BuckSN + " 此治具编码已超过最大使用次数!", "NG");
            //        Edt_BuckSN.SelectAll();
            //        Edt_BuckSN.Text = "";
            //        Edt_BuckSN.Focus();
            //        return;
            //    }


            //    if (S_MapPartID != S_PartID)
            //    {
            //        Public_.Add_Info_MSG(Edt_MSG, S_BuckSN + " 此治具编码和所选料号不匹配!", "NG");
            //        Edt_BuckSN.SelectAll();
            //        Edt_BuckSN.Text = "";
            //        Edt_BuckSN.Focus();
            //        return;
            //    }

            //    if (S_StatusID == "0")
            //    {
            //        Public_.Add_Info_MSG(Edt_MSG, S_BuckSN + " 此治具编码已停用!", "NG");
            //        Edt_BuckSN.SelectAll();
            //        Edt_BuckSN.Text = "";
            //        Edt_BuckSN.Focus();
            //        return;
            //    }
            //    else if (S_StatusID == "2")
            //    {
            //        Public_.Add_Info_MSG(Edt_MSG, S_BuckSN + " 此治具编码已和产品绑定，解绑后方可再次扫描!", "NG");
            //        Edt_BuckSN.SelectAll();
            //        Edt_BuckSN.Text = "";
            //        Edt_BuckSN.Focus();
            //        return;
            //    }

            //    Edt_MSG.Text = "";
            //    Edt_SN.Enabled = true;
            //    Edt_BuckSN.Enabled = false;
            //    Edt_SN.Focus();
            //    Edt_SN.SelectAll();
            //}
        }


        string S_UnitID = "";
        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //Public_.Add_Info_MSG(Edt_MSG, "", "Start");
                DateTime dateStart = DateTime.Now;

                string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, List_Login.StationTypeID, List_Login.LineID.ToString());
                // luUnitStatus  ID
                //DataRowView DRV_luUnitStatus = Com_luUnitStatus.SelectedItem as DataRowView;
                string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();     

                string S_DefectID = Edt_DefectID.Text.Trim();
                string S_SN = Edt_SN.Text.Trim();
                string S_BuckSN = Edt_BuckSN.Text.Trim(); 
                //工单  PartID
                string S_POPartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();

                if (S_SN == "")
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " SN不能为空!", "NG");
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }

                if (S_BuckSN == S_SN)
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " " + S_BuckSN + " 治具编号(BuckSN)和序列号(SN)一样！", "NG");
                    return;
                }


                if (S_luUnitStateID != "1")
                {
                    if (S_DefectID == "")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, S_SN + " 确认此物料NG,请设置NG原因!", "NG");
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
                            Public_.Add_Info_MSG(Edt_MSG, S_SN + " " + S_POID, "NG");
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                            return;
                        }
                    }

                    if (S_POID == "" || S_POID == "0")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码没有第一次过站记录！", "NG");
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
                        Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码和选择的工单不一致！", "NG");
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

                    string S_SerialNumberType = DT_SN.Rows[0]["SerialNumberTypeID"].ToString();
                    if (S_SerialNumberType != "5")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码类别不匹配！", "NG");
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }

                    if (DT_Unit.Rows[0]["StatusID"].ToString() != "1")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码已NG！", "NG");
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
                                        Public_.Add_Info_MSG(Edt_MSG, S_UpdateUnit, "NG");
                                        return;
                                    }

                                    if (S_UpdateUnit.Substring(0, 1) != "E")
                                    {
                                        //虚拟  Buck  SN 
                                        string S_xmlPart = "'<BoxSN SN=" + "\"" + S_BuckSN + "\"" + "> </BoxSN>'";
                                        string S_FormatSN = Com_PartFamilyType.Text.Substring(0, 3) + "_Buck";
                                        DataSet DS = PartSelectSVC.uspSNRGetNext(S_FormatSN, 0, null, S_xmlPart, null, null, null);
                                        DataTable DT = DS.Tables[2];
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
                                            Public_.Add_Info_MSG(Edt_MSG, S_BuckSN + " " + S_InsertBuckUnit, "NG");
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
                                            v_mesUnitDetail_Buck.reserved_03 = "";
                                            v_mesUnitDetail_Buck.reserved_04 = S_SN;   //产品 SN 
                                            v_mesUnitDetail_Buck.reserved_05 = "";

                                            ///// webservice
                                            S_InsertUnitDetail = mesUnitDetailSVC.Insert(v_mesUnitDetail_Buck);

                                            /////////////////////////////////// 修改设备表  使用次数  /////////////////////                                           
                                            mesMachineSVC.SetMachineRuningQuantity(S_BuckSN);
                                            /////////////////////////////////// 修改设备表  设备状态  /////////////////////
                                            DataTable DT_Machine = PartSelectSVC.GetmesMachine(S_BuckSN).Tables[0];
                                            string  S_MachineID = DT_Machine.Rows[0]["Id"].ToString();  
                                            PartSelectSVC.ModMachine2(S_MachineID, "2");

                                        }
                                        catch (Exception ex)
                                        {
                                            Public_.Add_Info_MSG(Edt_MSG, S_SN + " " + S_BuckSN +
                                                                    " mesUnitDetail Insert" + S_InsertUnitDetail + " " + ex.Message, "NG");
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
                                        int partID =Convert.ToInt32(dtsParts.Tables[0].Rows[0]["PartID"].ToString());
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
                                            Public_.Add_Info_MSG(Edt_MSG, S_UC_Result, "NG");
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
                                                catch
                                                {
                                                    Public_.Add_Info_MSG(Edt_MSG, S_mesUnitDefectInsert, "NG");
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

                                        //Public_.Add_Info_MSG(Edt_MSG, S_SN + " OK！", "OK");
                                        TimeSpan ts = DateTime.Now - dateStart;
                                        double mill = Math.Round(ts.TotalMilliseconds, 0);
                                        Public_.Add_Info_MSG(Edt_MSG, S_SN + " " + mill + "毫秒 OK！", "OK");
                                    }
                                }
                                finally
                                {
                                }
                            }
                        }
                        else
                        {
                            Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码和工单不匹配", "NG");
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                        }
                    }
                    else
                    {
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        Public_.Add_Info_MSG(Edt_MSG, S_RouteCheck, "NG");
                    }
                }
                else
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " 条码不存在!", "NG");
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                }
            }
        }

        private void UnitReplaceTool_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                PartSelectSVC.Close();
                mesUnitSVC.Close();
                mesSerialNumberSVC.Close();
                mesHistorySVC.Close();
                mesUnitComponentSVC.Close();
                mesUnitDefectSVC.Close();
                mesPartSVC.Close();
                mesProductionOrderSVC.Close();
                mesUnitDetailSVC.Close(); 
            }
            catch
            { }
        }
    }
}
