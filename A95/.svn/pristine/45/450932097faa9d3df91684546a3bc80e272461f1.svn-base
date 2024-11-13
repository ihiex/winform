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

namespace App.MyMES
{
    public partial class LinkBatch_Form : Form
    {
        Public_ public_ = new Public_();
        DataTable DT_ProductionOrder = new DataTable();
        DataTable DT_ProductStructure = new DataTable();
        LoginList List_Login = new LoginList();
        string Batch_Pattern = string.Empty;

        int I_RouteSequence = -9;  //当前工序顺序 
        int ScanOKQuantity = 0;
        int ScanNGQuantity = 0;
        int I_POID = 0;
        string S_DefectTypeID = "";

        PartSelectSVCClient PartSelectSVC ;
        ImesUnitSVCClient mesUnitSVC ;
        ImesUnitDetailSVCClient mesUnitDetailSVC ;
        ImesHistorySVCClient mesHistorySVC ;
        ImesUnitDefectSVCClient mesUnitDefectSVC ;


        public LinkBatch_Form()
        {
            InitializeComponent();
        }

        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            try
            {
                PartSelectSVC.Close();
                mesUnitSVC.Close();
                mesUnitDetailSVC.Close();
                mesHistorySVC.Close();
                mesUnitDefectSVC.Close();
            }
            catch
            { }

            PartSelectSVC = PartSelectFactory.CreateServerClient();
            mesUnitSVC = ImesUnitFactory.CreateServerClient();
            mesUnitDetailSVC = ImesUnitDetailFactory.CreateServerClient();
            mesHistorySVC = ImesHistoryFactory.CreateServerClient();
            mesUnitDefectSVC = ImesUnitDefectFactory.CreateServerClient();

            List_Login = this.Tag as LoginList;
            ///////////////////////////
            public_.AddStationType(Com_StationType,Grid_StationType);
            public_.AddLine(Com_Line,Grid_Line);

            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_LineID = List_Login.LineID.ToString();
            public_.AddStation(Com_Station, S_LineID,Grid_Station);

            Com_StationType.Text = S_StationTypeID;
            Com_Line.Text = S_LineID;
            Com_Station.Text = List_Login.StationID.ToString();

            public_.AddPartFamilyType(Com_PartFamilyType,Grid_PartFamilyType);
            public_.AddluUnitStatus(Com_luUnitStatus,Grid_luUnitStatus);

            //DataRowView DRV = Com_Part.SelectedItem as DataRowView;
            string S_PartID = Com_Part.EditValue.ToString();   //DRV["ID"].ToString();
            public_.AddPOAll(Com_PO, S_PartID, List_Login.LineID.ToString(),Grid_PO);

            Edt_SN.Focus();
            Edt_SN.SelectAll();

            //Edt_MSG.Text = "";
            Edt_SN.Text = "";
            Edt_SN.Enabled = true;

            Edt_Batch.Text = ""; 

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

                Edt_Batch.Enabled = false;  
                Edt_SN.Enabled = false;
            }
            else
            {
                Com_PartFamilyType.BackColor = Color.Yellow;
                Com_PartFamily.BackColor = Color.Yellow;
                Com_Part.BackColor = Color.Yellow;
                Com_PO.BackColor = Color.Yellow;

                Edt_Batch.Enabled = true; 
                Edt_SN.Enabled = false;
            }

            Edt_SN.Focus();
            Edt_SN.SelectAll();

            Edt_Batch.Text = "";
            Edt_SN.Text = "";  
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


        private void OverStation_Form_Load(object sender, EventArgs e)
        {
            Btn_Refresh_Click(sender, e);

            string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
            I_RouteSequence = public_.RouteSequence(S_PartID, List_Login.StationTypeID, List_Login.LineID.ToString());
            int I_FirstScanSequence = FirstScanSequence(S_PartID, List_Login.StationTypeID);

            Com_PO.Enabled = true;
            Edt_Batch.Enabled = false;  
            if (I_RouteSequence > I_FirstScanSequence)
            {
                Com_PO.Enabled = false;
                Com_PO.BackColor = Color.Yellow;

                Edt_SN.Enabled = true;
                Btn_Unlock.Enabled = false;
                Btn_Refresh.Enabled = false;
                Btn_ConfirmPO.Enabled = false;

                Panel_Part.Visible = false;
            }
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

        private void Edt_Batch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string S_Batch = Edt_Batch.Text.Trim();
                if(string.IsNullOrEmpty(Batch_Pattern))
                {
                    public_.Add_Info_MSG(Edt_MSG," 未配置校验批次正则表达式！", "NG");
                    Edt_Batch.SelectAll();
                    return;
                }

                if (!Regex.IsMatch(Edt_Batch.Text.Trim(), Batch_Pattern.Replace("\\\\", "\\")))
                {
                    public_.Add_Info_MSG(Edt_MSG, S_Batch + " 批次号格式不正确！", "NG");
                    Edt_Batch.SelectAll();
                    return;
                }

                Edt_Batch.Enabled = false;
                Edt_SN.Enabled = true;
                Edt_SN.Focus();
                Edt_SN.SelectAll();  
            }
        }


        string S_UnitID = "";
        private void Edt_ScanSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DateTime dateStart = DateTime.Now;
                string ScanSN = Edt_SN.Text.ToString();
                try
                {
                    public_.Add_Info_MSG(Edt_MSG, "", "Start");
                    //Edt_MSG.Text = "";
                    string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                    string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, List_Login.StationTypeID, List_Login.LineID.ToString());

                    string S_DefectID = Edt_DefectID.Text.Trim();
                    string S_BatchSN = Edt_Batch.Text.Trim();
                    string S_SN = Edt_SN.Text.Trim();

                    if (S_SN == "")
                    {
                        public_.Add_Info_MSG(Edt_MSG, S_SN + " SN不能为空!", "NG");
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }


                    if (S_BatchSN == S_SN)
                    {
                        public_.Add_Info_MSG(Edt_MSG, S_SN +" "+ S_BatchSN+ " 批次号(BatchSN)和序列号(SN)一样！", "NG");
                        return;
                    }

                    //工单  PartID
                    string S_POPartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                    //选择的  PartID
                    //DataRowView DRV_SelectPart = Com_Part.SelectedItem as DataRowView;
                    string S_SelectPartID = Com_Part.EditValue.ToString(); //DRV_SelectPart["ID"].ToString().Trim();
                    // luUnitStatus  ID
                    //DataRowView DRV_luUnitStatus = Com_luUnitStatus.SelectedItem as DataRowView;
                    string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();   //DRV_luUnitStatus["ID"].ToString().Trim();

                    if (S_luUnitStateID != "1")
                    {
                        if (S_DefectID == "")
                        {
                            public_.Add_Info_MSG(Edt_MSG, S_SN + " 确认此物料NG,请设置NG原因!", "NG");
                            Edt_SN.Focus();
                            Edt_SN.Text = ""; 
                            return;
                        }
                    }

                    int I_FirstScanSequence = FirstScanSequence(S_POPartID, List_Login.StationTypeID);
                    if (I_RouteSequence > I_FirstScanSequence)  //不是第一个工序时根据  SN  获取工单
                    {
                        List<string> List_PO = public_.SnToPOID(S_SN);
                        string S_POID = List_PO[0];

                        if (S_POID.Length > 4)
                        {
                            if (S_POID.Substring(0, 5) == "ERROR")
                            {
                                public_.Add_Info_MSG(Edt_MSG, S_SN + " "+ S_POID, "NG");
                                Edt_SN.Focus();
                                Edt_SN.Text = "";
                                return;
                            }
                        }

                        if (S_POID == "" || S_POID == "0")
                        {
                            public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码没有第一次过站记录！", "NG");
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                            return;
                        }
                        
                        Get_DTPO(S_POID);
                        Com_PO.Text = S_POID;
                    }


                    //string S_Sql = "select * from mesSerialNumber where Value ='" + S_SN + "'";
                    //DataTable DT_SN = public_.P_DataSet(S_Sql).Tables[0];
                    string S_Sql = "";
                    DataTable DT_SN = PartSelectSVC.GetmesSerialNumber(S_SN).Tables[0] ;

                    if (DT_SN.Rows.Count > 0)
                    {
                        S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                        //S_Sql = "select * from mesUnit where ID='" + S_UnitID + "'";
                        //DataTable DT_Unit = public_.P_DataSet(S_Sql).Tables[0];

                        DataTable DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0] ;
                        string S_StationID = List_Login.StationID.ToString();    //DT_Unit.Rows[0]["StationID"].ToString();

                        string S_SerialNumberType = DT_SN.Rows[0]["SerialNumberTypeID"].ToString();
                        if (S_SerialNumberType != "5")
                        {
                            public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码类别不匹配！", "NG");
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                            return;
                        }

                        if (DT_Unit.Rows[0]["UnitStateID"].ToString() == "2")
                        {
                            public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码已NG！", "NG");
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                            return;
                        }

                        if (S_POPartID != DT_Unit.Rows[0]["PartID"].ToString())
                        {
                            public_.Add_Info_MSG(Edt_MSG, S_SN + " SN和选择的工单不匹配！", "NG");
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                            return;
                        }


                        /////////////////////////////// GetRouteCheck  /////////////////////////////////////
                        //Boolean B_IsRoute = GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, DT_Unit,S_SN);                    
                        string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, S_SN);                       

                        if (S_RouteCheck == "1")
                        {
                            //string S_PartID = DT_Unit.Rows[0]["PartID"].ToString();
                            if (S_POPartID == S_SelectPartID)
                            {
                                // 本工位不判断 子料,不需要判断BOM
                                //string S_StationTypeID = List_Login.StationTypeID.ToString();
                                //S_Sql = "select * from mesProductStructure where ParentPartID='" + S_PartID + "' and StationTypeID='" + S_StationTypeID + "'";
                                ////S_Sql = "select * from mesProductStructure where ParentPartID='" + S_PartID + "' and StationID='" + S_StationID + "'";
                                //DT_ProductStructure = public_.P_DataSet(S_Sql).Tables[0];

                                //if (DT_ProductStructure.Rows.Count > 0)
                                //{
                                //    Edt_SN.Enabled = false; 
                                //}
                                //else

                                {
                                    // 本工位不判断 子料                                
                                    //ImesSerialNumberSVCClient mesSerialNumberSVC = ImesSerialNumberFactory.CreateServerClient();                                
                                    string S_POID = DT_ProductionOrder.Rows[0]["ID"].ToString();
                                    DataTable DT_UPUnitID = PartSelectSVC.Get_UnitID(S_SN).Tables[0];

                                    mesUnit v_mesUnit = new mesUnit();
                                    v_mesUnit.ID = Convert.ToInt32(DT_UPUnitID.Rows[0]["UnitID"].ToString());
                                    v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                                    v_mesUnit.StatusID=Convert.ToInt32(S_luUnitStateID);
                                    v_mesUnit.StationID = List_Login.StationID;
                                    v_mesUnit.EmployeeID = List_Login.EmployeeID;
                                    v_mesUnit.LastUpdate = DateTime.Now;
                                    v_mesUnit.ProductionOrderID = Convert.ToInt32(S_POID);
                                    //修改 Unit                                
                                    string S_UpdateUnit = mesUnitSVC.Update(v_mesUnit);                                   

                                    if (S_UpdateUnit.Substring(0, 1) == "E")
                                    {
                                        public_.Add_Info_MSG(Edt_MSG, S_SN + " " + S_UpdateUnit, "NG");
                                        return;
                                    }

                                    if (S_UpdateUnit.Substring(0, 1) != "E")
                                    {
                                        string S_InsertUnitDetail = "";
                                        try
                                        {
                                            //////////////////////////////////////  mesUnitDetail /////////////////////////////////////////////////////                                    
                                            mesUnitDetail v_mesUnitDetail = new mesUnitDetail();
                                            v_mesUnitDetail.UnitID = Convert.ToInt32(v_mesUnit.ID);
                                            v_mesUnitDetail.reserved_01 = S_SN;
                                            v_mesUnitDetail.reserved_02 = S_BatchSN;
                                            v_mesUnitDetail.reserved_03 = "";
                                            v_mesUnitDetail.reserved_04 = "";
                                            v_mesUnitDetail.reserved_05 = "";

                                            ///// webservice
                                            S_InsertUnitDetail = mesUnitDetailSVC.Insert(v_mesUnitDetail);
                                        }
                                        catch (Exception ex)
                                        {
                                            public_.Add_Info_MSG(Edt_MSG, S_SN+" " +S_BatchSN + " mesUnitDetail Insert" + S_InsertUnitDetail+" "+ex.Message, "NG");
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
                                            public_.Add_Info_MSG(Edt_MSG, S_SN + " mesHistory Insert" + ex.Message, "NG");
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
                                        /////////////////////////////////////////////////////////////////////////
                                        /////////////////////////////////////////////////////////////////////////
                                        if (S_luUnitStateID == "1")
                                        {
                                            ScanOKQuantity++;
                                            Edt_ScanOKQuantity.Text = ScanOKQuantity.ToString();
                                        }
                                        else
                                        {
                                            ScanNGQuantity++;
                                            Edt_ScanNGQuantity.Text = ScanNGQuantity.ToString();
                                        }

                                        TimeSpan ts = DateTime.Now - dateStart;
                                        double mill = Math.Round(ts.TotalMilliseconds, 0);
                                        public_.Add_Info_MSG(Edt_MSG, S_SN + " " + S_BatchSN + " "+ mill+"毫秒 OK！", "OK");

                                        Edt_SN.Text = "";                                       
                                        Edt_SN.Focus();
                                        //Edt_DefectID.Text = "";
                                        GetScanQuantity();                                        
                                        //CloseDefect();
                                    }
                                }
                            }
                            else
                            {
                                public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码和工单不匹配", "NG");
                                Edt_SN.Focus();
                                Edt_SN.Text = "";
                            }
                        }
                        else
                        {
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                            public_.Add_Info_MSG(Edt_MSG, S_RouteCheck, "NG");
                        }
                    }
                    else
                    {
                        public_.Add_Info_MSG(Edt_MSG, S_SN + " 条码不存在!", "NG");
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                    }
                }
                finally
                {
                    string OpenLogFile = System.Configuration.ConfigurationManager.AppSettings["OpenLogFile"];
                    if (OpenLogFile == "Y")
                    {
                        TimeSpan ts = DateTime.Now - dateStart;
                        double mill = ts.TotalMilliseconds;
                        if (Edt_MSG.ForeColor == Color.Green)
                        {
                            LogNet.LogDug(this.Name, "工位:" + List_Login.StationType + ",批次号:" + Edt_Batch.Text.ToString() + ",SN:" +
                                                    ScanSN + ",用时：" + mill.ToString() + "毫秒");
                        }
                        else
                        {
                            LogNet.LogEor(this.Name, "工位:" + List_Login.StationType + ",批次号:" + Edt_Batch.Text.ToString() + ",SN:" +
                                                    ScanSN + ",用时：" + mill.ToString() + "毫秒");
                        }
                    }
                    //PartSelectSVC.Close(); 
                    //mesUnitSVC.Close();
                    //mesUnitDetailSVC.Close();
                    //mesHistorySVC.Close();
                    //mesUnitDefectSVC.Close();
                }
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
            v_DefectForm.Show_DefectFormToPanel(v_DefectForm, Edt_DefectID,Convert.ToInt32(S_DefectTypeID));

            v_DefectForm.FormBorderStyle = FormBorderStyle.None;
            v_DefectForm.Width = Panel_Defact.Width;
            v_DefectForm.Height = Panel_Defact.Height;
            v_DefectForm.TopLevel = false;
            v_DefectForm.Parent = Panel_Defact;
            v_DefectForm.BringToFront();
            // v_DefectForm.Tag = F_LoginList;
            v_DefectForm.Show();
        }

        private void Com_PartFamilyType_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {

        }

        private void Com_PartFamily_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {

        }

        private void Com_luUnitStatus_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {

        }

        private void OverStation_Form_Resize(object sender, EventArgs e)
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

        private void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            if (Com_PO.Text.Trim() == "")
            {
                public_.Add_Info_MSG(Edt_MSG, "工单不能为空!", "NG");
            }
            else
            {
                PartEdtStatus(false);

                Edt_Batch.Focus();
                Edt_Batch.SelectAll();
                if (!string.IsNullOrEmpty(Com_Part.EditValue.ToString()))
                {
                    int I_PartID = Convert.ToInt32(Com_Part.EditValue.ToString());
                  //  PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                    DataSet dataSet = PartSelectSVC.GetmesPartDetail(I_PartID, "Batch_Pattern");
                    if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        Batch_Pattern = dataSet.Tables[0].Rows[0]["Content"].ToString();
                    }
                    else
                    {
                        public_.Add_Info_MSG(Edt_MSG, " 未配置校验批次正则表达式！", "NG");
                    }
                   // PartSelectSVC.Close();
                }
            }
        }

        private void Btn_Unlock_Click(object sender, EventArgs e)
        {
            PartEdtStatus(true);
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

        private void LinkBatch_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                PartSelectSVC.Close();
                mesUnitSVC.Close();
                mesUnitDetailSVC.Close();
                mesHistorySVC.Close();
                mesUnitDefectSVC.Close();
            }
            catch
            { }
        }

        private void btnRest_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要重置当前计数吗?", "提示",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }
            Edt_ScanOKQuantity.Text = "0";
            Edt_ScanNGQuantity.Text = "0";
            ScanOKQuantity = 0;
            ScanNGQuantity = 0;
        }
    }

}
