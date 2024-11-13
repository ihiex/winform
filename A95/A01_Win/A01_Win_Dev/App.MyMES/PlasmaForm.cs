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
using App.Model;

namespace App.MyMES
{
    public partial class PlasmaForm : Form
    {
        int I_POID = 0;
        int OverStationNumber = 0;
        int NumIn = 1;                                  //扫入序号
        int NumOut = 1;                                 //扫出序号
        int OutGroupID = 1;                             //扫出分组
        int InGroupID = 1;                              //扫入分组
        int TimeOut = 0;                                //Plasma超时时间 /秒
        int GroupNumber = 0;                            //Plasma分组扫描数量
        int MaxGroupNumber=1;                           //Plasma最大分组数
        string S_DefectTypeID = "";
        DataTable DT_ProductionOrder = new DataTable();
        DataTable GridView;

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

        public PlasmaForm()
        {
            InitializeComponent();
        }

        #region 界面加载
        private void PlasmaForm_Load(object sender, EventArgs e)
        {
            LoadControls();
            LoadGridViewData();
            Com_PO.Enabled = true;
        }

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

                List_Login = this.Tag as LoginList;
                public_.AddLine(Com_Line, Grid_Line);

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

                DataSet ds = PartSelectSVC.MESGetStationTypeParameter(List_Login.StationTypeID);
                DataRow[] drTimeOut;
                DataRow[] drGroupNumber;
                DataRow[] drMaxGroup;
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    Public_.Add_Info_MSG(Edt_MSG, "料号没有配置任何参数!", "NG");
                    return;
                }

                drTimeOut = ds.Tables[0].Select("Description='PlasmaTimeOut'");
                drGroupNumber = ds.Tables[0].Select("Description='PlasmaGroupNumber'");
                drMaxGroup = ds.Tables[0].Select("Description='PlasmaMaxGroup'");
                if (drTimeOut.Length == 0)
                {
                    Public_.Add_Info_MSG(Edt_MSG, "料号没有配置Plasma操作超时时间!", "NG");
                    return;
                }

                TimeOut = Convert.ToInt32(drTimeOut[0]["Content"].ToString());
                if (drGroupNumber.Length != 0)
                {
                    GroupNumber = Convert.ToInt32(drGroupNumber[0]["Content"].ToString());
                }
                if (drMaxGroup.Length != 0)
                {
                    MaxGroupNumber = Convert.ToInt32(drMaxGroup[0]["Content"].ToString());
                }

                PartEdtStatus(false);
                txtScanIn.Text = string.Empty;
                txtScanOut.Text = string.Empty;
                Btn_ConfirmPO.Text = "Unlock(解锁)";
                Btn_ConfirmPO.Tag = 1;
                txtScanIn.Focus();
            }
            else
            {
                //参数初始化
                NumIn = 1;
                NumOut = 1;
                OutGroupID = 1;
                InGroupID = 1;
                GridView.Clear();

                PartEdtStatus(true);
                Btn_ConfirmPO.Text = "Confirm(确定)";
                Btn_ConfirmPO.Tag = 0;
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
            btnScanIn.Enabled = false;
            BtnMoveScanIn.Enabled = !B_Status;
            txtScanIn.ReadOnly = B_Status;
            btnScanOut.Enabled = !B_Status;
            txtScanOut.ReadOnly = true;
            //BtnMoveScanOut.Enabled = false;

            if (B_Status == true)
            {
                Com_PartFamilyType.BackColor = Color.White;
                Com_PartFamily.BackColor = Color.White;
                Com_Part.BackColor = Color.White;
                Com_PO.BackColor = Color.White;
            }
            else
            {
                Com_PartFamilyType.BackColor = Color.Yellow;
                Com_PartFamily.BackColor = Color.Yellow;
                Com_Part.BackColor = Color.Yellow;
                Com_PO.BackColor = Color.Yellow;
            }
        }
        #endregion

        private void LoadGridViewData()
        {
            if (GridView == null)
            {
                GridView = new DataTable();
                GridView.Columns.Add("NO", typeof(int));
                GridView.Columns.Add("SerialNumber", typeof(string));
                GridView.Columns.Add("ScanInTime", typeof(string));
                GridView.Columns.Add("ScanOutTime", typeof(string));
                GridView.Columns.Add("GroupID", typeof(int));
                GridView.Columns.Add("Status", typeof(string));
                DataGridViewScan.DataSource = GridView;
            }
        }

        private void btnScanIn_Click(object sender, EventArgs e)
        {
            if (GroupNumber > 1 && NumOut != 1)
            {
                Public_.Add_Info_MSG(Edt_MSG, string.Format("Plasma分组数量为{0},未完成所有扫出不能进行扫入操作",
                    GroupNumber.ToString()), "NG");
                return;
            }

            btnScanIn.Enabled = false;
            //BtnMoveScanOut.Enabled = false;
            BtnMoveScanIn.Enabled = true;
            btnScanOut.Enabled = true;
            txtScanIn.ReadOnly = false;
            txtScanOut.ReadOnly = true;
            txtScanOut.Text = string.Empty;
        }

        private void btnScanOut_Click(object sender, EventArgs e)
        {
            //分组扫描判断是否已经扫完分组数量
            if (GroupNumber > 1 && NumIn != 1)
            {
                Public_.Add_Info_MSG(Edt_MSG, string.Format("Plasma分组数量为{0},未完成所有扫入不能进行扫出操作",
                    GroupNumber.ToString()), "NG");
                return;
            }
            btnScanIn.Enabled = true;
            //BtnMoveScanOut.Enabled = true;
            BtnMoveScanIn.Enabled = false;
            btnScanOut.Enabled = false;
            txtScanOut.ReadOnly = false;
            txtScanIn.ReadOnly = true;
            txtScanIn.Text = string.Empty;
        }

        private void txtScanIn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string S_SN = txtScanIn.Text.Trim();

                if (GroupNumber > 1 && GridView.Rows.Count >= GroupNumber * MaxGroupNumber)
                {
                    Public_.Add_Info_MSG(Edt_MSG, string.Format("最大扫描{0}组Plasma条码，请先扫出！", MaxGroupNumber), "NG");
                    txtScanIn.Focus();
                    txtScanIn.Text = "";
                    return;
                }

                if (S_SN == "")
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " SN不能为空!", "NG");
                    return;
                }

                //校验是否已经扫入
                if (GridView != null && GridView.Rows.Count > 0)
                {
                    int len = GridView.Select("SerialNumber='" + S_SN + "'").Length;
                    if (len > 0)
                    {
                        Public_.Add_Info_MSG(Edt_MSG, S_SN + " SN已经扫进，不能重复操作!", "NG");
                        txtScanIn.Focus();
                        txtScanIn.Text = "";
                        return;
                    }
                }

                //工序校验
                if (!MESCheckScanSN(S_SN))
                {
                    txtScanIn.Focus();
                    txtScanIn.Text = "";
                }
                else
                {
                    string LastUpdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    MesDisposeScanIn(LastUpdate, S_SN);
                   
                    DataRow dataRow = GridView.NewRow();
                    dataRow["NO"] = NumIn;
                    dataRow["SerialNumber"] = txtScanIn.Text.Trim();
                    dataRow["ScanInTime"] = LastUpdate;
                    dataRow["ScanOutTime"] = "";
                    dataRow["GroupID"] = InGroupID;
                    dataRow["Status"] = "已扫进";
                    GridView.Rows.Add(dataRow);
                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " 扫进成功！", "OK");

                    txtScanIn.Text = "";
                    DataGridViewScan.CurrentCell = DataGridViewScan.Rows[GridView.Rows.Count-1].Cells[0];
                    if (GroupNumber > 1 && GroupNumber == NumIn)
                    {
                        NumIn = 1;
                        SetParamScan();
                    }
                    else
                    {
                        NumIn++;
                    }
                }
            }
        }

        /// <summary>
        /// 参数设置
        /// </summary>
        private void SetParamScan()
        {
            for (int i = 1; i <= MaxGroupNumber; i++)
            {
                if (GridView.Select("GroupID=" + i).Length == 0)
                {
                    InGroupID = i;
                    return;
                }
            }
        }

        /// <summary>
        /// 扫进条码处理
        /// </summary>
        private void MesDisposeScanIn(string LastUpdate,string SN)
        {
            string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
            string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, List_Login.StationTypeID, List_Login.LineID.ToString());
            string Result = mesUnitSVC.UpdatePlasma(SN, Convert.ToInt32(S_UnitStateID), LastUpdate,List_Login.LineID);
            if (Result != "OK")
            {
                Public_.Add_Info_MSG(Edt_MSG, Result, "NG");
                return;
            }
        }


        /// <summary>
        /// 扫出条码过站
        /// </summary>
        private void MesDisposeScanOut(DateTime EnterTime, DateTime ExitTime)
        {
            string S_SN = txtScanOut.Text.Trim();
            string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
            string S_POID = DT_ProductionOrder.Rows[0]["ID"].ToString();
            string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, List_Login.StationTypeID, List_Login.LineID.ToString());

            DataTable DT_UPUnitID = PartSelectSVC.Get_UnitID(S_SN).Tables[0];

            mesUnit v_mesUnit = new mesUnit();
            v_mesUnit.ID = Convert.ToInt32(DT_UPUnitID.Rows[0]["UnitID"].ToString());
            v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
            v_mesUnit.StatusID = 1;
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

            mesHistory v_mesHistory = new mesHistory();

            v_mesHistory.UnitID = Convert.ToInt32(v_mesUnit.ID);
            v_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
            v_mesHistory.EmployeeID = List_Login.EmployeeID;
            v_mesHistory.StationID = List_Login.StationID;
            v_mesHistory.EnterTime = EnterTime;
            v_mesHistory.ExitTime = ExitTime;
            v_mesHistory.ProductionOrderID = Convert.ToInt32(S_POID);
            v_mesHistory.PartID = Convert.ToInt32(S_PartID);
            v_mesHistory.LooperCount = 1;
            int result = mesHistorySVC.Insert(v_mesHistory);
            if (result == 0)
            {
                Public_.Add_Info_MSG(Edt_MSG, S_SN + " SN过站履历记录失败！", "NG");
                return;
            }
        }

        /// <summary>
        /// 扫进条码校验
        /// </summary>
        /// <param name="S_SN"></param>
        /// <returns></returns>
        private bool MESCheckScanSN(string S_SN)
        {
            DataTable DT_SN = PartSelectSVC.GetmesSerialNumber(S_SN).Tables[0];
            if (DT_SN.Rows.Count > 0)
            {
                List<string> List_PO = public_.SnToPOID(S_SN);
                string S_POID = List_PO[0];

                if (S_POID.Length > 4)
                {
                    if (S_POID.Substring(0, 5) == "ERROR")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, S_SN + " " + S_POID, "NG");
                        return false;
                    }
                }
                if (S_POID == "" || S_POID == "0")
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码没有第一次过站记录！", "NG");
                    return false;
                }

                string S_ComPO = Com_PO.EditValue.ToString();
                if (S_POID != S_ComPO)
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码和选择的工单不一致！", "NG");
                    return false;
                }

                string S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                DataTable DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0];
                string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, S_SN);
                if (S_RouteCheck != "1")
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_RouteCheck, "NG");
                    return false;
                }
            }
            else
            {
                Public_.Add_Info_MSG(Edt_MSG, S_SN + " 条码不存在!", "NG");
                return false;
            }
            return true;
        }

        private void txtScanOut_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string S_SNOut = txtScanOut.Text.Trim();

                DataRow[] drs;
                if (S_SNOut == "")
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_SNOut + " SN不能为空!", "NG");
                    return;
                }

                //校验是否已经扫入
                if (GridView == null || GridView.Rows.Count <= 0)
                {
                    Public_.Add_Info_MSG(Edt_MSG, "没有任何扫进数据，无法进行扫出操作!", "NG");
                    txtScanOut.Text = string.Empty;
                    txtScanOut.Focus();
                    return;
                }

                drs = GridView.Select("SerialNumber='" + S_SNOut + "'");
                if (drs.Length <= 0 || drs[0]["Status"].ToString() != "已扫进")
                {
                    Public_.Add_Info_MSG(Edt_MSG, S_SNOut + " SN未扫进，不能扫出!", "NG");
                    txtScanOut.Text = string.Empty;
                    txtScanOut.Focus();
                    return;
                }

                //工序校验
                if (!MESCheckScanSN(S_SNOut))
                {
                    txtScanOut.Text = string.Empty;
                    txtScanOut.Focus();
                }
                else
                {
                    //校验是否超时
                    DateTime dtTimeIn = Convert.ToDateTime(drs[0]["ScanInTime"].ToString());
                    DateTime dtTimeOut = DateTime.Now;
                    TimeSpan timeSpan = dtTimeOut.Subtract(dtTimeIn);
                    int seconds = timeSpan.Seconds;
                    if(seconds>=TimeOut)
                    {
                        Public_.Add_Info_MSG(Edt_MSG, string.Format(" SN：{0} Plasma操作超时。用时：{1}秒", S_SNOut, seconds.ToString()), "NG");
                        txtScanOut.Text = string.Empty;
                        txtScanOut.Focus();
                        return;
                    }

                    //记录每组第一次的扫描分组
                    if (GroupNumber > 1)
                    {
                        if (NumOut == 1)
                        {
                            OutGroupID = Convert.ToInt32(GridView.Select("SerialNumber='" + S_SNOut + "'")[0]["GroupID"].ToString());
                        }
                        else
                        {
                            int ScanGroupID = Convert.ToInt32(GridView.Select("SerialNumber='" + S_SNOut + "'")[0]["GroupID"].ToString());
                            if (OutGroupID != ScanGroupID)
                            {
                                Public_.Add_Info_MSG(Edt_MSG, "扫描条码的组别不匹配!", "NG");
                                txtScanOut.Text = string.Empty;
                                txtScanOut.Focus();
                                return;
                            }
                        }
                    }

                    MesDisposeScanOut(dtTimeIn, dtTimeOut);
                    
                    drs[0]["ScanOutTime"] = dtTimeOut.ToString("yyyy-MM-dd HH:mm:ss");
                    drs[0]["Status"] = "已扫出";

                    Public_.Add_Info_MSG(Edt_MSG, txtScanOut.Text.ToString() + " 扫出成功！", "OK");
                    txtScanOut.Text = string.Empty;
                    txtScanOut.Focus();
                    OverStationNumber++;
                    Edt_ScanOKQuantity.Text = OverStationNumber.ToString();
                    if (GroupNumber > 1)
                    {
                        if(GroupNumber == NumOut)
                        {
                            NumOut = 1;
                            DataRow[] ArrDeleteRowNo = GridView.Select("GroupID=" + OutGroupID);

                            //扫描完成清除数据
                            foreach (DataRow dr in ArrDeleteRowNo)
                            {
                                GridView.Rows.Remove(dr);
                            }
                            SetParamScan();
                        }
                        else
                        {
                            NumOut++;
                        }
                    }
                    else
                    {
                        DataRow drRemove = GridView.Select("SerialNumber='" + S_SNOut+"'")[0];
                        GridView.Rows.Remove(drRemove);
                    }
                }
            }
        }

        private void BtnMoveScanIn_Click(object sender, EventArgs e)
        {
            if (DataGridViewScan.CurrentRow == null) return;
            int indexNo = DataGridViewScan.CurrentRow.Index;

            if (indexNo >= 0)
            {
                int nowGroup = Convert.ToInt32(DataGridViewScan.Rows[indexNo].Cells["GroupID"].Value.ToString());
                DataRow[] drNew = GridView.Select("GroupID=" + nowGroup);
                if (DataGridViewScan.Rows[indexNo].Cells["Status"].Value.ToString() != "已扫进"
                    || nowGroup != InGroupID || (drNew.Length == GroupNumber) && GroupNumber > 1)
                {
                    Public_.Add_Info_MSG(Edt_MSG, "当前状态不能移除", "NG");
                    return;
                }
                if (MessageBox.Show("你确认要移除选中的扫进条码?", "提示",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }

                DataGridViewScan.Rows.Remove(DataGridViewScan.Rows[indexNo]);
                if (GroupNumber > 1)
                {
                    //重新排序
                    for (int i = 1; i <= drNew.Length; i++)
                    {
                        drNew[i-1]["NO"] = i;
                    }
                    NumIn = GridView.Select("GroupID=" + nowGroup).Length + 1;
                }
            }
        }

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
    }
}
