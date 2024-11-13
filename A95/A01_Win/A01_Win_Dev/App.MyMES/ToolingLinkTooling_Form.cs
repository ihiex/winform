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
using App.MyMES.mesMachineService;
using App.MyMES.mesPartService;
using App.MyMES.mesUnitDetailService;
using App.Model;

namespace App.MyMES
{
    public partial class ToolingLinkTooling_Form : Form
    {
        int I_POID = 0;
        string S_DefectTypeID = "";
        int OverStationNumber = 0;
        DataTable DT_ProductionOrder = new DataTable();
        string S_MachineID = string.Empty;

        Public_ public_ = new Public_();
        LoginList List_Login = new LoginList();

        PartSelectSVCClient PartSelectSVC;
        ImesUnitSVCClient mesUnitSVC;
        ImesHistorySVCClient mesHistorySVC;
        ImesUnitDefectSVCClient mesUnitDefectSVC;
        ImesUnitComponentSVCClient mesUnitComponentSVC;
        ImesProductionOrderSVCClient mesProductionOrderSVC;
        ImesMachineSVCClient mesMachineSVC;
        ImesPartSVCClient mesPartSVC;
        ImesSerialNumberSVCClient mesSerialNumberSVC;
        ImesUnitDetailSVCClient mesUnitDetailSVC;

        public ToolingLinkTooling_Form()
        {
            InitializeComponent();
        }

        private void ToolingLinkTooling_Form_Load(object sender, EventArgs e)
        {
            LoadControls();
            Com_PO.Enabled = true;
        }

        #region 界面加载
        private void LoadControls()
        {
            try
            {
                PartSelectSVC = PartSelectFactory.CreateServerClient();
                mesUnitSVC = ImesUnitFactory.CreateServerClient();
                mesHistorySVC = ImesHistoryFactory.CreateServerClient();
                mesUnitDefectSVC = ImesUnitDefectFactory.CreateServerClient();
                mesUnitComponentSVC = ImesUnitComponentFactory.CreateServerClient();
                mesProductionOrderSVC = ImesProductionOrderFactory.CreateServerClient();
                mesMachineSVC = ImesMachineFactory.CreateServerClient();
                mesPartSVC = ImesPartFactory.CreateServerClient();
                mesSerialNumberSVC = ImesSerialNumberFactory.CreateServerClient();
                mesUnitDetailSVC = ImesUnitDetailFactory.CreateServerClient();

                List_Login = this.Tag as LoginList;
                public_.AddLine(Com_Line, Grid_Line);
                ShowScanToolingName();

                string S_StationTypeID = List_Login.StationTypeID.ToString();
                string S_LineID = List_Login.LineID.ToString();
                public_.AddStation(Com_Station, S_LineID, Grid_Station);

                Com_Line.Text = S_LineID;
                Com_Station.Text = List_Login.StationID.ToString();

                public_.AddPartFamilyType(Com_PartFamilyType, Grid_PartFamilyType);
                string S_PartID = Com_Part.EditValue.ToString();
                public_.AddPOAll(Com_PO, S_PartID, List_Login.LineID.ToString(), Grid_PO);
            }
            catch
            { }

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

        private void Get_DTPO(string S_POID)
        {
            I_POID = Convert.ToInt32(S_POID);
            DT_ProductionOrder = PartSelectSVC.GetProductionOrder(S_POID).Tables[0];

            Edt_OrderQuantity.Text = DT_ProductionOrder.Rows[0]["OrderQuantity"].ToString();
        }

        private void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {

            if (Btn_ConfirmPO.Tag.ToString() == "0")
            {
                if (Com_PartFamilyType.EditValue == null || string.IsNullOrEmpty(Com_PartFamilyType.Text.ToString()))
                {
                    Public_.Add_Info_MSG(Edt_MSG, "未选择料号类别，请确认！", "NG");
                    return;
                }
                if (Com_PartFamily.EditValue == null || string.IsNullOrEmpty(Com_PartFamily.Text.ToString()))
                {
                    Public_.Add_Info_MSG(Edt_MSG, "未选择料号群，请确认！", "NG");
                    return;
                }
                if (Com_Part.EditValue == null || string.IsNullOrEmpty(Com_Part.Text.ToString()))
                {
                    Public_.Add_Info_MSG(Edt_MSG, "未选择料号，请确认！", "NG");
                    return;
                }
                if (Com_PO.EditValue == null || string.IsNullOrEmpty(Com_PO.Text.ToString()))
                {
                    Public_.Add_Info_MSG(Edt_MSG, "未选择工单，请确认！", "NG");
                    return;
                }

                PartEdtStatus(false);
                Btn_ConfirmPO.Text = "Unlock(解锁)";
                Btn_ConfirmPO.Tag = 1;
            }
            else
            {
                Btn_ConfirmPO.Text = "Confirm(确定)";
                Btn_ConfirmPO.Tag = 0;
                PartEdtStatus(true);
            }
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
            txtNewTooling.ReadOnly = B_Status;
            txtNewTooling.Text = string.Empty;
            txtOldTooing.ReadOnly = true;
            txtOldTooing.Text = string.Empty;

            if (B_Status == true)
            {
                Com_PartFamilyType.BackColor = Color.White;
                Com_PartFamily.BackColor = Color.White;
                Com_Part.BackColor = Color.White;
                Com_PO.BackColor = Color.White;
            }
            else
            {
                txtNewTooling.Focus();
                Com_PartFamilyType.BackColor = Color.Yellow;
                Com_PartFamily.BackColor = Color.Yellow;
                Com_Part.BackColor = Color.Yellow;
                Com_PO.BackColor = Color.Yellow;
            }
        }

        /// <summary>
        /// 过站重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRest_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确认要重置当前数量吗?", "提示",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }
            Edt_ScanOKQuantity.Text = "0";
            OverStationNumber = 0;
        }

        private void ToolingLinkTooling_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                PartSelectSVC.Close();
                mesUnitSVC.Close();
                mesHistorySVC.Close();
                mesUnitDefectSVC.Close();
                mesUnitComponentSVC.Close();
                mesProductionOrderSVC.Close();
                mesMachineSVC.Close();
                mesPartSVC.Close();
                mesSerialNumberSVC.Close();
                mesUnitDetailSVC.Close();
            }catch(Exception ex)
            {

            }
        }
        #endregion

        private void ShowScanToolingName()
        {
            DataSet ds = PartSelectSVC.MESGetStationTypeParameter(List_Login.StationTypeID);
            DataRow[] drToolingFrom;
            DataRow[] drToolingTo;
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                Public_.Add_Info_MSG(Edt_MSG, "料号没有配置任何参数!", "NG");
                return;
            }

            drToolingFrom = ds.Tables[0].Select("Description='ToolingFrom'");
            drToolingTo = ds.Tables[0].Select("Description='ToolingTo'");
            if (drToolingFrom.Length > 0)
            {
                lblToolingFrom.Text = drToolingFrom[0]["Content"].ToString();
            }
            if (drToolingTo.Length > 0)
            {
                lblToolingTo.Text = drToolingTo[0]["Content"].ToString();
            }
        }

        private void txtNewTooling_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string MachineSN = txtNewTooling.Text.Trim();
                if (string.IsNullOrEmpty(MachineSN))
                {
                    return;
                }

                string Result = string.Empty;
                string xmlPartStr = "<Part PartID=\"" + Com_Part.EditValue.ToString() + "\"> </Part>";

                PartSelectSVC.uspCallProcedure("uspMachineToolingCheck", MachineSN,
                                                                        null, xmlPartStr, null, null, List_Login.StationTypeID.ToString(), ref Result);

                if(Result!="1")
                {
                    Public_.Add_Info_MSG(Edt_MSG, Result, "NG");
                    txtNewTooling.Text = string.Empty;
                    txtNewTooling.Focus();
                    return;
                }

                //string Result = PartSelectSVC.MESAssembleCheckVirtualSN(MachineSN, Com_Part.EditValue.ToString(), "1");
                //if (Result != "1")
                //{
                //    Public_.Add_Info_MSG(Edt_MSG, Result, "NG");
                //    txtNewTooling.Text = string.Empty;
                //    txtNewTooling.Focus();
                //    return;
                //}

                //DataSet DS_Machine = PartSelectSVC.GetmesMachine(MachineSN);
                //string warningStatus = DS_Machine.Tables[0].Rows[0]["WarningStatus"].ToString();
                //S_MachineID = DS_Machine.Tables[0].Rows[0]["ID"].ToString();
                ////校验使用次数
                //if (warningStatus == "1")
                //{
                //    string outString = string.Empty;
                //    PartSelectSVC.uspCallProcedure("uspMachineToolingCheck", MachineSN, null, null, null, null, null, ref outString);
                //    if (outString != "OK")
                //    {
                //        Public_.Add_Info_MSG(Edt_MSG, outString, "NG");
                //        txtNewTooling.Text = string.Empty;
                //        txtNewTooling.Focus();
                //        return;
                //    }
                //}
                txtNewTooling.ReadOnly = true;
                txtOldTooing.ReadOnly = false;
                txtOldTooing.Text = string.Empty;
                txtOldTooing.Focus();
            }
        }

        private void txtOldTooing_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    string MachineSN = txtOldTooing.Text.Trim();
                    if (string.IsNullOrEmpty(MachineSN))
                    {
                        return;
                    }


                    string Result = string.Empty;
                    string xmlPartStr = "<Part PartID=\"\"> </Part>";

                    PartSelectSVC.uspCallProcedure("uspMachineToolingCheck", MachineSN,
                                                                            null, xmlPartStr, null, null, List_Login.StationTypeID.ToString(), ref Result);

                    if (Result != "1")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, Result, "NG");
                        txtOldTooing.Text = string.Empty;
                        txtOldTooing.Focus();
                        return;
                    }

                    //string Result = PartSelectSVC.MESAssembleCheckVirtualSN(MachineSN, null, "2");
                    //if (Result != "1")
                    //{
                    //    Public_.Add_Info_MSG(Edt_MSG, Result, "NG");
                    //    txtOldTooing.Text = string.Empty;
                    //    txtOldTooing.Focus();
                    //    return;
                    //}

                    //DataSet DS_Machine = PartSelectSVC.GetmesMachine(MachineSN);
                    //string warningStatus = DS_Machine.Tables[0].Rows[0]["WarningStatus"].ToString();
                    ////校验使用次数
                    //if (warningStatus == "1")
                    //{
                    //    string outString = string.Empty;
                    //    PartSelectSVC.uspCallProcedure("uspMachineToolingCheck", MachineSN, null, null, null, null, null, ref outString);
                    //    if (outString != "OK")
                    //    {
                    //        Public_.Add_Info_MSG(Edt_MSG, outString, "NG");
                    //        txtOldTooing.Text = string.Empty;
                    //        txtOldTooing.Focus();
                    //        return;
                    //    }
                    //}

                    string batch = string.Empty;
                    PartSelectSVC.BoxSnToBatch(MachineSN, out batch);
                    if (string.IsNullOrEmpty(batch))
                    {
                        Public_.Add_Info_MSG(Edt_MSG, string.Format("夹具/托盘SN：{0} 未找到绑定的批次!", MachineSN), "OK");
                        txtOldTooing.Text = string.Empty;
                        txtOldTooing.Focus();
                        return;
                    }
                    else if (batch.Substring(0, 5) == "ERROR")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, batch, "NG");
                        txtOldTooing.Text = string.Empty;
                        txtOldTooing.Focus();
                        return;
                    }

                    string S_PartID = Com_Part.EditValue.ToString();
                    string S_POID = Com_PO.EditValue.ToString();
                    string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, List_Login.StationTypeID, List_Login.LineID.ToString());

                    string S_xmlPart = "'<BoxSN SN=" + "\"" + txtNewTooling.Text.Trim() + "\"" + "> </BoxSN>'";
                    string S_FormatSN = PartSelectSVC.mesGetSNFormatIDByList(S_PartID, Com_PartFamily.EditValue.ToString(), null);
                    DataSet DS = PartSelectSVC.uspSNRGetNext(S_FormatSN, 0, null, S_xmlPart, null, null, null);
                    DataTable DT = DS.Tables[2];
                    string S_SN = DT.Rows[0][0].ToString();

                    //数据保存
                    mesUnit v_mesUnit = new mesUnit();
                    v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                    v_mesUnit.StatusID = 1;
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
                    string S_InsertUnit = mesUnitSVC.Insert(v_mesUnit);
                    v_mesUnit.ID = Convert.ToInt32(S_InsertUnit);

                    if (S_InsertUnit.Substring(0, 1) == "E")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, batch + " " + S_InsertUnit, "NG");
                        return;
                    }

                    //////////////////////////////////////  mesSerialNumber /////////////////////////////////////////////////////
                    mesSerialNumber v_mesSerialNumber = new mesSerialNumber();
                    v_mesSerialNumber.UnitID = Convert.ToInt32(S_InsertUnit);
                    v_mesSerialNumber.SerialNumberTypeID = 8;
                    v_mesSerialNumber.Value = S_SN;
                    mesSerialNumberSVC.Insert(v_mesSerialNumber);

                    //////////////////////////////////////  mesUnitDetail /////////////////////////////////////////////////////                                    
                    mesUnitDetail v_mesUnitDetail = new mesUnitDetail();
                    v_mesUnitDetail.UnitID = Convert.ToInt32(v_mesUnit.ID);
                    v_mesUnitDetail.reserved_01 = txtNewTooling.Text.Trim();
                    v_mesUnitDetail.reserved_02 = batch;
                    v_mesUnitDetail.reserved_03 = "1";             
                    v_mesUnitDetail.reserved_04 = "";
                    v_mesUnitDetail.reserved_05 = "";
                    mesUnitDetailSVC.Insert(v_mesUnitDetail);

                    //////////////////////////////////////  mesHistory /////////////////////////////////////////////////////                                    
                    mesHistory v_mesHistory = new mesHistory();
                    v_mesHistory.UnitID = Convert.ToInt32(v_mesUnit.ID);
                    v_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                    v_mesHistory.EmployeeID = List_Login.EmployeeID;
                    v_mesHistory.StationID = List_Login.StationID;
                    v_mesHistory.EnterTime = DateTime.Now;
                    v_mesHistory.ExitTime = DateTime.Now;
                    v_mesHistory.ProductionOrderID = Convert.ToInt32(S_POID);
                    v_mesHistory.PartID = Convert.ToInt32(S_POID);
                    v_mesHistory.LooperCount = 1;
                    mesHistorySVC.Insert(v_mesHistory);

                    //更新状态
                    //PartSelectSVC.ModMachine2(S_MachineID, "2");
                    mesMachineSVC.MesModMachineBySNStationTypeID(txtNewTooling.Text.Trim(),List_Login.StationTypeID);
                    mesMachineSVC.MesModMachineBySNStationTypeID(txtOldTooing.Text.Trim(),List_Login.StationTypeID);

                    Public_.Add_Info_MSG(Edt_MSG, string.Format("{0}SN：{1}绑定{2}SN：{3}成功",lblToolingFrom.Text.ToString(),
                        txtNewTooling.Text.Trim(),lblToolingTo.Text.ToString(),txtOldTooing.Text.Trim()), "OK");
                    OverStationNumber++;
                    Edt_ScanOKQuantity.Text = OverStationNumber.ToString();
                    txtOldTooing.ReadOnly = true;
                    txtNewTooling.ReadOnly = false;
                    txtNewTooling.Text = string.Empty;
                    txtOldTooing.Text = string.Empty;
                    txtNewTooling.Focus();
                }
                catch(Exception ex)
                {
                    Public_.Add_Info_MSG(Edt_MSG, string.Format("程序异常：{0}",ex.Message.ToString()), "NG");
                }
            }
        }
    }
}
