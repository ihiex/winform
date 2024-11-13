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
    public partial class Plasma_Form : FrmBase
    {
        int NumIn = 1;                                  //扫入序号
        int NumOut = 1;                                 //扫出序号
        int OutGroupID = 1;                             //扫出分组
        int InGroupID = 1;                              //扫入分组
        int TimeOut = 0;                                //Plasma超时时间 /秒
        int SetTime = 0;                                //规定不能少于当前设置时间
        int GroupNumber = 0;                            //Plasma分组扫描数量
        int MaxGroupNumber = 1;                         //Plasma最大分组数
        DataTable GridView;

        public Plasma_Form()
        {
            InitializeComponent();
        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            if (GridView != null)
                GridView.Clear();
        }

        public override void FrmBase_Load(object sender, EventArgs e)
        {
            base.FrmBase_Load(sender, e);
            LoadGridViewData();
        }

        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            base.Btn_ConfirmPO_Click(sender, e);

            DataSet ds = PartSelectSVC.MESGetStationTypeParameter(List_Login.StationTypeID);
            DataRow[] drTimeOut;
            DataRow[] drGroupNumber;
            DataRow[] drMaxGroup;
            DataRow[] drSetTime;
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20000", "NG", List_Login.Language);
                return;
            }

            drTimeOut = ds.Tables[0].Select("Description='PlasmaTimeOut'");
            drGroupNumber = ds.Tables[0].Select("Description='PlasmaGroupNumber'");
            drMaxGroup = ds.Tables[0].Select("Description='PlasmaMaxGroup'");
            drSetTime = ds.Tables[0].Select("Description='PlasmaSetTime'");
            if (drTimeOut.Length == 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20001", "NG", List_Login.Language);
                return;
            }
            if (drSetTime.Length == 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20152", "NG", List_Login.Language);
                return;
            }

            TimeOut = Convert.ToInt32(drTimeOut[0]["Content"].ToString());
            SetTime = Convert.ToInt32(drSetTime[0]["Content"].ToString());
            if (drGroupNumber.Length != 0)
            {
                GroupNumber = Convert.ToInt32(drGroupNumber[0]["Content"].ToString());
            }
            if (drMaxGroup.Length != 0)
            {
                MaxGroupNumber = Convert.ToInt32(drMaxGroup[0]["Content"].ToString());
            }

            txtScanIn.Focus();
        }

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
                MessageInfo.Add_Info_MSG(Edt_MSG, "20002", "NG", List_Login.Language, new string[] { GroupNumber.ToString() });
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
                MessageInfo.Add_Info_MSG(Edt_MSG, "20004", "NG", List_Login.Language, new string[] { GroupNumber.ToString() });
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

        private void BtnMoveScanIn_Click(object sender, EventArgs e)
        {
            int indexNo = gdViewData.FocusedRowHandle;

            if (indexNo >= 0)
            {
                int nowGroup = Convert.ToInt32(gdViewData.GetFocusedRowCellValue("GroupID").ToString());
                DataRow[] drNew = GridView.Select("GroupID=" + nowGroup);
                if (gdViewData.GetFocusedRowCellValue("Status").ToString() != "ScanIn"
                    || nowGroup != InGroupID || (drNew.Length == GroupNumber) && GroupNumber > 1)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20003", "NG", List_Login.Language);
                    return;
                }
                if (!MessageInfo.Add_Info_MessageBox("10002", List_Login.Language))
                {
                    return;
                }

                gdViewData.DeleteRow(indexNo);                

                if (GroupNumber > 1)
                {
                    //重新排序
                    for (int i = 1; i <= drNew.Length; i++)
                    {
                        drNew[i - 1]["NO"] = i;
                    }
                    NumIn = GridView.Select("GroupID=" + nowGroup).Length + 1;
                }
            }
        }

        private void txtScanIn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string S_SN = txtScanIn.Text.Trim();

                if (GroupNumber > 1 && GridView.Rows.Count >= GroupNumber * MaxGroupNumber)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20005", "NG", List_Login.Language, new string[] { MaxGroupNumber.ToString() });
                    txtScanIn.Focus();
                    txtScanIn.Text = "";
                    return;
                }

                if (S_SN == "")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG",List_Login.Language);
                    return;
                }

                //校验是否已经扫入
                if (GridView != null && GridView.Rows.Count > 0)
                {
                    int len = GridView.Select("SerialNumber='" + S_SN + "'").Length;
                    if (len > 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20006", "NG", List_Login.Language);
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
                    dataRow["Status"] = "ScanIn";
                    GridView.Rows.Add(dataRow);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10004", "OK", List_Login.Language, new string[] { S_SN.ToString() });
                    this.gdViewData.FocusedRowHandle = this.gdViewData.DataRowCount - 1;

                    txtScanIn.Text = "";
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
        private void MesDisposeScanIn(string LastUpdate, string SN)
        {
            string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
            string PartFamilyID = Com_PartFamily.EditValue.ToString();
            string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID,List_Login.StationTypeID, 
                List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());
            if (string.IsNullOrEmpty(S_UnitStateID))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
                return;
            }

            string Result = mesUnitSVC.UpdatePlasma(SN, Convert.ToInt32(S_UnitStateID), LastUpdate, List_Login.LineID);
            if (Result != "OK")
            {
                string[] strList = new string[] { Result.ToString() };
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, strList);
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
            string PartFamilyID = Com_PartFamily.EditValue.ToString();
            string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID, 
                List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());

            if (string.IsNullOrEmpty(S_UnitStateID))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
                return;
            }
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
                string[] strList = new string[] { S_UpdateUnit.ToString() };
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, strList);
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
                MessageInfo.Add_Info_MSG(Edt_MSG, "20010", "NG", List_Login.Language, new string[] { S_SN.ToString() });
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
                        string ProMsg = MessageInfo.GetMsgByCode(S_POID.Replace("ERROR",""),List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN.ToString(), ProMsg }, S_POID.Replace("ERROR", ""));
                        return false;
                    }
                }
                if (S_POID == "" || S_POID == "0")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20014", "NG", List_Login.Language, new string[] { S_SN.ToString() });
                    return false;
                }

                string S_ComPO = Com_PO.EditValue.ToString();
                if (S_POID != S_ComPO)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20015", "NG", List_Login.Language, new string[] { S_SN.ToString() });
                    return false;
                }

                string S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                DataTable DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0];
                string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, S_SN);
                if (S_RouteCheck != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(S_RouteCheck, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_RouteCheck);
                    return false;
                }
            }
            else
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language, new string[] { S_SN });
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
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                    return;
                }

                //校验是否已经扫入
                if (GridView == null || GridView.Rows.Count <= 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20019", "NG", List_Login.Language);
                    txtScanOut.Text = string.Empty;
                    txtScanOut.Focus();
                    return;
                }

                drs = GridView.Select("SerialNumber='" + S_SNOut + "'");
                if (drs.Length <= 0 || drs[0]["Status"].ToString() != "ScanIn")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20020", "NG", List_Login.Language, new string[] { S_SNOut.ToString() });
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
                    //TimeSpan timeSpan = dtTimeOut.Subtract(dtTimeIn);
                    //int seconds = timeSpan.Seconds;
                    System.TimeSpan ts1 = new System.TimeSpan(dtTimeIn.Ticks);
                    System.TimeSpan ts2 = new System.TimeSpan(dtTimeOut.Ticks);
                    System.TimeSpan tsSub = ts1.Subtract(ts2).Duration();
                    int seconds = ts1.Subtract(ts2).Duration().Seconds;
                    int minute = ts1.Subtract(ts2).Duration().Minutes;
                    int hours = ts1.Subtract(ts2).Duration().Hours;
                    int day = ts1.Subtract(ts2).Duration().Days;
                    int count = day * 86400 + hours * 3600 + minute * 60 + seconds;
                    if (count >= TimeOut)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20021", "NG", List_Login.Language, new string[] { S_SNOut.ToString(), count.ToString() });
                        txtScanOut.Text = string.Empty;
                        txtScanOut.Focus();
                        return;
                    }

                    if (count < SetTime)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20153", "NG", List_Login.Language, new string[] { S_SNOut.ToString(), count.ToString() });
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
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20022", "NG", List_Login.Language);
                                txtScanOut.Text = string.Empty;
                                txtScanOut.Focus();
                                return;
                            }
                        }
                    }

                    MesDisposeScanOut(dtTimeIn, dtTimeOut);

                    drs[0]["ScanOutTime"] = dtTimeOut.ToString("yyyy-MM-dd HH:mm:ss");
                    drs[0]["Status"] = "ScanOut";

                    MessageInfo.Add_Info_MSG(Edt_MSG, "10003", "OK", List_Login.Language, new string[] { txtScanOut.Text.ToString() });
                    txtScanOut.Text = string.Empty;
                    txtScanOut.Focus();
                    base.SetOverStiaonQTY(true);
                    if (GroupNumber > 1)
                    {
                        if (GroupNumber == NumOut)
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
                        DataRow drRemove = GridView.Select("SerialNumber='" + S_SNOut + "'")[0];
                        GridView.Rows.Remove(drRemove);
                    }
                }
            }
        }
    }
}