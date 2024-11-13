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

namespace App.MyMES
{
    public partial class BoxLinkBatch_Form : Form
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



        public BoxLinkBatch_Form()
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

            Edt_BactchSN.Focus();
            Edt_BactchSN.SelectAll();

            Edt_MSG.Text = "";
            Edt_BactchSN.Text = "";
            Edt_BactchSN.Enabled = true;

            Edt_Box.Text = "";

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

                Edt_Box.Enabled = false;
                Edt_BactchSN.Enabled = false;
            }
            else
            {
                Com_PartFamilyType.BackColor = Color.Yellow;
                Com_PartFamily.BackColor = Color.Yellow;
                Com_Part.BackColor = Color.Yellow;
                Com_PO.BackColor = Color.Yellow;

                Edt_Box.Enabled = true;
                Edt_BactchSN.Enabled = false;
            }

            Edt_BactchSN.Focus();
            Edt_BactchSN.SelectAll();

            Edt_Box.Text = "";
            Edt_BactchSN.Text = "";
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
            GetScanQuantity();
        }


        private void GetScanQuantity()
        {
            //string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
            //string S_Sql = "select COUNT(ID) CountUnit  from  mesUnit  where PartID=" + S_PartID +
            //                "  and StationID=" + List_Login.StationID +
            //                "  and ProductionOrderID=" + I_POID;
            //DataTable DT_CountUnit = public_.P_DataSet(S_Sql).Tables[0];

            //Edt_ScanQuantity.Text = DT_CountUnit.Rows[0]["CountUnit"].ToString();
        }


        private void Btn_Unlock_Click(object sender, EventArgs e)
        {
            PartEdtStatus(true);
        }

        private void BoxLinkBatch_Form_Load(object sender, EventArgs e)
        {
            Btn_Refresh_Click(sender, e);

            string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
            I_RouteSequence = public_.RouteSequence(S_PartID, List_Login.StationTypeID, List_Login.LineID.ToString());
            int I_FirstScanSequence = FirstScanSequence(S_PartID, List_Login.StationTypeID);

            Com_PO.Enabled = true;
            Edt_Box.Enabled = false;
            if (I_RouteSequence > I_FirstScanSequence)
            {
                Com_PO.Enabled = false;
                Com_PO.BackColor = Color.Yellow;

                Edt_BactchSN.Enabled = true;
                Btn_Unlock.Enabled = false;
                Btn_Refresh.Enabled = false;
                Btn_ConfirmPO.Enabled = false;

                Panel_Part.Visible = false;
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


        private void BoxLinkBatch_Form_Resize(object sender, EventArgs e)
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
                public_.Add_Info_MSG(Edt_MSG, "工单不能为空!", "NG");
            }
            else
            {
                PartEdtStatus(false);

                Edt_Box.Focus();
                Edt_Box.SelectAll();

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
                        public_.Add_Info_MSG(Edt_MSG, " 未配置校验批次正则表达式！", "NG");
                    }
                    //PartSelectSVC.Close();
                }
            }
        }

        private void Edt_Box_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string S_CardSN = Edt_Box.Text.Trim();

                //string S_Sql = "select * from mesMachine where SN='" + S_CardSN + "' ";
                //DataTable DT_Machine = public_.P_DataSet(S_Sql).Tables[0];

                DataTable DT_Machine = PartSelectSVC.GetmesMachine(S_CardSN).Tables[0]; 
                if (DT_Machine.Rows.Count == 0)
                {
                    public_.Add_Info_MSG(Edt_MSG, S_CardSN+" 无此盒子编码SN!", "NG");
                    Edt_Box.SelectAll();
                    Edt_Box.Text = "";
                    Edt_Box.Focus();
                    return;
                }

                string S_MachineID= DT_Machine.Rows[0]["ID"].ToString();
                string S_StatusID = DT_Machine.Rows[0]["StatusID"].ToString();                
                string S_MachineFamilyID= DT_Machine.Rows[0]["MachineFamilyID"].ToString();
                string S_PartID = Com_Part.EditValue.ToString();

                //S_Sql = "select *  from mesRouteMachineMap where MachineID="+ S_MachineID;
                //DataTable DT_MachineMapSN = public_.P_DataSet(S_Sql).Tables[0];
                DataTable DT_MachineMapSN = PartSelectSVC.GetmesRouteMachineMap(S_MachineID, "").Tables[0];

                //S_Sql = "select *  from mesRouteMachineMap where MachineFamilyID=" + S_MachineFamilyID;
                //DataTable DT_MachineMapFamilyID = public_.P_DataSet(S_Sql).Tables[0];
                DataTable DT_MachineMapFamilyID = PartSelectSVC.GetmesRouteMachineMap("", S_MachineFamilyID).Tables[0];

                string S_MapPartID = "";
                if (DT_MachineMapSN.Rows.Count > 0)
                {
                    S_MapPartID = DT_MachineMapSN.Rows[0]["PartID"].ToString();
                }
                else if (DT_MachineMapFamilyID.Rows.Count>0)
                {
                    S_MapPartID = DT_MachineMapFamilyID.Rows[0]["PartID"].ToString();
                }

                if (S_MapPartID != S_PartID)
                {
                    public_.Add_Info_MSG(Edt_MSG, S_CardSN + " 此盒子编码和所选料号不匹配!", "NG");
                    Edt_Box.SelectAll();
                    Edt_Box.Text = "";
                    Edt_Box.Focus();
                    return;
                }

                if (S_StatusID == "0")
                {
                    public_.Add_Info_MSG(Edt_MSG, S_CardSN+" 此盒子编码已停用!", "NG");
                    Edt_Box.SelectAll();
                    Edt_Box.Text = "";
                    Edt_Box.Focus();
                    return;
                }
                else if (S_StatusID == "2")
                {
                    public_.Add_Info_MSG(Edt_MSG, S_CardSN+" 此盒子编码下一站扫描后方可再次扫描!", "NG");
                    Edt_Box.SelectAll();
                    Edt_Box.Text = "";
                    Edt_Box.Focus();
                    return;
                }

                Edt_MSG.Text = ""; 
                Edt_Box.Enabled = false;
                Edt_BactchSN.Enabled = true;
                Edt_BactchSN.Focus();
                Edt_BactchSN.SelectAll();
            }
        }

        //string S_UnitID = "";
        private void Edt_BactchSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string S_BatchSN = Edt_BactchSN.Text.Trim();
                if (string.IsNullOrEmpty(Batch_Pattern))
                {
                    public_.Add_Info_MSG(Edt_MSG, " 未配置校验批次正则表达式！", "NG");
                    Edt_BactchSN.SelectAll();
                    return;
                }

                if (!Regex.IsMatch(Edt_BactchSN.Text.Trim(), Batch_Pattern))
                {
                    public_.Add_Info_MSG(Edt_MSG, S_BatchSN + " 批次号格式不正确！", "NG");
                    Edt_BactchSN.SelectAll();
                    return;
                }

                if (S_BatchSN.Length < 5)
                {
                    public_.Add_Info_MSG(Edt_MSG, S_BatchSN + " 批次号最少5个字符！", "NG");
                    Edt_BactchSN.SelectAll();
                    return;
                }

                //PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                //ImesUnitSVCClient mesUnitSVC = ImesUnitFactory.CreateServerClient();
                //ImesUnitDetailSVCClient mesUnitDetailSVC = ImesUnitDetailFactory.CreateServerClient();
                //ImesHistorySVCClient mesHistorySVC = ImesHistoryFactory.CreateServerClient();
                //ImesUnitDefectSVCClient mesUnitDefectSVC = ImesUnitDefectFactory.CreateServerClient();
                //ImesSerialNumberSVCClient mesSerialNumberSVC = ImesSerialNumberFactory.CreateServerClient();

                try
                {
                    Edt_MSG.Text = "";
                    string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                    string S_UnitStateID = "104";//public_.GetluUnitStatusID(S_PartID, List_Login.StationTypeID, List_Login.LineID.ToString());

                    string S_DefectID = Edt_DefectID.Text.Trim();
                    string S_CardSN = Edt_Box.Text.Trim();

                    string S_xmlPart = "'<BoxSN SN=" + "\"" + S_CardSN + "\"" + "> </BoxSN>'";
                    string S_FormatSN = Com_PartFamilyType.Text.Substring(0, 4) + "_Box";  
                    DataSet DS = PartSelectSVC.uspSNRGetNext(S_FormatSN, 0, null, S_xmlPart, null, null, null);

                    DataTable DT = DS.Tables[2];
                    string S_SN = DT.Rows[0][0].ToString(); //S_CardSN + "_" + DateTime.Now.ToString("yyyyMMddHHmm");

                    if (S_CardSN == S_BatchSN)
                    {
                        public_.Add_Info_MSG(Edt_MSG, S_BatchSN + " " + S_CardSN + " 批次号(BatchSN)和盒子(SN)一样！", "NG");
                        return;
                    }

                    //工单  PartID
                    string S_POPartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                    //选择的  PartID
                    string S_SelectPartID = Com_Part.EditValue.ToString(); //DRV_SelectPart["ID"].ToString().Trim();
                    // luUnitStatus  ID
                    string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();   //DRV_luUnitStatus["ID"].ToString().Trim();

                    if (S_luUnitStateID != "1")
                    {
                        if (S_DefectID == "")
                        {
                            public_.Add_Info_MSG(Edt_MSG, S_BatchSN + " 确认此物料NG,请设置NG原因!", "NG");
                            Edt_BactchSN.Focus();
                            Edt_BactchSN.Text = "";
                            return;
                        }
                    }

                    //string S_Sql = "select * from mesMachine where SN='" + S_CardSN + "'";
                    //DataTable DT_Card = public_.P_DataSet(S_Sql).Tables[0];
                     DataTable DT_Card = PartSelectSVC.GetmesMachine(S_CardSN).Tables[0];  
                    if (DT_Card.Rows.Count == 0)
                    {
                        public_.Add_Info_MSG(Edt_MSG, " 无此编码SN!", "NG");
                        Edt_Box.SelectAll();
                    }
                    else
                    {
                        string S_StatusID = DT_Card.Rows[0]["StatusID"].ToString();
                        if (S_StatusID == "1")  //Box 文本框已做状态判断
                        {
                            if (S_POPartID == S_SelectPartID)
                            {
                                public_.Add_Info_MSG(Edt_MSG, "", "Start");
                                // 本工位不判断 子料                                                               
                                string S_POID = DT_ProductionOrder.Rows[0]["ID"].ToString();
                                //DataTable DT_UPUnitID = PartSelectSVC.Get_UnitID(S_BatchSN).Tables[0];

                                mesUnit v_mesUnit = new mesUnit();
                                //v_mesUnit.ID = Convert.ToInt32(DT_UPUnitID.Rows[0]["UnitID"].ToString());
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

                                //insert Unit                                
                                string S_InsertUnit = mesUnitSVC.Insert(v_mesUnit);
                                v_mesUnit.ID = Convert.ToInt32(S_InsertUnit);

                                if (S_InsertUnit.Substring(0, 1) == "E")
                                {
                                    public_.Add_Info_MSG(Edt_MSG, S_BatchSN + " " + S_InsertUnit, "NG");
                                    return;
                                }

                                if (S_InsertUnit.Substring(0, 1) != "E")
                                {
                                    string S_InsertUnitDetail = "";
                                    try
                                    {
                                        mesSerialNumber v_mesSerialNumber = new mesSerialNumber();

                                        v_mesSerialNumber.UnitID = Convert.ToInt32(S_InsertUnit);
                                        v_mesSerialNumber.SerialNumberTypeID = 8;
                                        v_mesSerialNumber.Value = S_SN;

                                        int I_InsertSN = mesSerialNumberSVC.Insert(v_mesSerialNumber);

                                        //////////////////////////////////////  mesUnitDetail /////////////////////////////////////////////////////                                    
                                        mesUnitDetail v_mesUnitDetail = new mesUnitDetail();
                                        v_mesUnitDetail.UnitID = Convert.ToInt32(v_mesUnit.ID);
                                        v_mesUnitDetail.reserved_01 = S_CardSN;
                                        v_mesUnitDetail.reserved_02 = S_BatchSN;
                                        v_mesUnitDetail.reserved_03 = "1";        //开料 1  备胶  2          
                                        v_mesUnitDetail.reserved_04 = "";
                                        v_mesUnitDetail.reserved_05 = "";

                                        ///// webservice
                                        S_InsertUnitDetail = mesUnitDetailSVC.Insert(v_mesUnitDetail);
                                    }
                                    catch (Exception ex)
                                    {
                                        public_.Add_Info_MSG(Edt_MSG, S_BatchSN + " " + S_CardSN +
                                                                " mesUnitDetail Insert" + S_InsertUnitDetail + " " + ex.Message, "NG");
                                    }

                                    int I_InsertHistory = 0;
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

                                        ///// webservice
                                        I_InsertHistory = mesHistorySVC.Insert(v_mesHistory);
                                    }
                                    catch (Exception ex)
                                    {
                                        public_.Add_Info_MSG(Edt_MSG, S_BatchSN + " mesHistory Insert" + ex.Message, "NG");
                                    }


                                    //修改工单状态
                                    string S_PO_StatusID = DT_ProductionOrder.Rows[0]["StatusID"].ToString();
                                    if (S_PO_StatusID == "1")
                                    {
                                        //S_Sql = "Update mesProductionOrder set StatusID=2,LastUpdate='" + DateTime.Now.ToString() +
                                        //        "' where ID='" + S_POID + "'";
                                        //public_.ExecSql(S_Sql);
                                        //PartSelectSVC.ModPO(S_POID);
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
                                                    ///// webservice
                                                    S_mesUnitDefectInsert = mesUnitDefectSVC.Insert(v_mesUnitDefect);
                                                }
                                            }
                                            catch
                                            {
                                                public_.Add_Info_MSG(Edt_MSG, S_mesUnitDefectInsert, "NG");
                                            }
                                        }
                                    }

                                    ///////////////////////////////////////// mesMachine  //////////////////////////////////////////////////
                                    string S_MachineID = DT_Card.Rows[0]["ID"].ToString();
                                    //S_Sql = "Update mesMachine set StatusID=2 where ID=" + S_MachineID;
                                    //public_.ExecSql(S_Sql);
                                    PartSelectSVC.ModMachine2(S_MachineID, "2");
                                    /////////////////////////////////////////////////////////////////////////
                                    /////////////////////////////////////////////////////////////////////////
                                    public_.Add_Info_MSG(Edt_MSG, S_BatchSN + " " + S_CardSN + " OK！", "OK");

                                    Edt_BactchSN.Text = "";
                                    Edt_BactchSN.Enabled = false;  

                                    Edt_Box.Text = "";
                                    Edt_Box.Enabled = true;
                                    Edt_Box.Focus();

                                    GetScanQuantity();
                                }
                            }
                            else
                            {
                                public_.Add_Info_MSG(Edt_MSG, S_BatchSN + " 此条码和工单不匹配", "NG");
                                Edt_BactchSN.Focus();
                                Edt_BactchSN.Text = "";
                            }
                        }
                    }
                }
                finally
                {
                    //PartSelectSVC.Close();
                    //mesUnitSVC.Close();
                    //mesUnitDetailSVC.Close();
                    //mesHistorySVC.Close();
                    //mesUnitDefectSVC.Close();
                    //mesSerialNumberSVC.Close();
                }
            }
        }

        private void BoxLinkBatch_Form_FormClosed(object sender, FormClosedEventArgs e)
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
