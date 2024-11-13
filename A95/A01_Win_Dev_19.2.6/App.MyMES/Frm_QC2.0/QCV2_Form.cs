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
    public partial class QCV2_Form : FrmBase
    {
        string S_UnitID = "";
        bool COF = false;

        string S_IsCheckPO = "1";
        string S_IsCheckPN = "1";

        string S_ProductionOrderID = "";
        //string S_PartID = "";
        //string S_PartFamilyID = "";
        //string S_PartFamilyType = "";
        DataSet dsBOM = null;
        DataSet DS_StationTypeDef;
        bool isEnableCOF = false;

        #region howard 20220322 add 
        bool isSampleAlerm = false;
        int sampleCount = 0;
        string sampleTimeContext;
        List<Tuple<DateTime, DateTime, DateTime>> timeSpec = new List<Tuple<DateTime, DateTime, DateTime>>();
        bool isStationType = false;
        #endregion

        string mS_UnitStateID = string.Empty;
        string S_luUnitStateID = "";


        public QCV2_Form()
        {
            InitializeComponent();
        }

        private DataSet GetStationData()
        {
            DataSet dsStationDetail = PartSelectSVC.GetPLCSeting("", List_Login.StationID.ToString());
            DS_StationTypeDef = PartSelectSVC.MESGetStationTypeParameter(List_Login.StationTypeID);

            DataRow[] DR_IsCheckPO = DS_StationTypeDef.Tables[0].Select("Description='IsCheckPO'");
            if (DR_IsCheckPO.Length > 0)
            {
                S_IsCheckPO = DR_IsCheckPO[0]["Content"].ToString();

                if (S_IsCheckPO == "1")
                {
                    DataRow[] DR_Child_IsCheckPO = dsStationDetail.Tables[0].Select("Name='IsCheckPO'");
                    if (DR_Child_IsCheckPO.Length > 0) S_IsCheckPO = DR_Child_IsCheckPO[0]["Value"].ToString(); else S_IsCheckPO = "1";
                }
            }
            else
            {
                S_IsCheckPO = "1";
            }

            DataRow[] DR_IsCheckPN = DS_StationTypeDef.Tables[0].Select("Description='IsCheckPN'");
            if (DR_IsCheckPN.Length > 0)
            {
                S_IsCheckPN = DR_IsCheckPN[0]["Content"].ToString();

                if (S_IsCheckPN == "1")
                {
                    DataRow[] DR_Child_IsCheckPN = dsStationDetail.Tables[0].Select("Name='IsCheckPN'");
                    if (DR_Child_IsCheckPN.Length > 0) S_IsCheckPN = DR_Child_IsCheckPN[0]["Value"].ToString(); else S_IsCheckPN = "1";
                }
            }
            else
            {
                S_IsCheckPN = "1";
            }


            if (S_IsCheckPO == "1")
            {
                lblProductionOrder.Visible = true;
                lblUnitState.Visible = true;
                lblUnitState.Location = new Point(58, 135);

                panel12.Visible = true;
            }
            else
            {
                lblProductionOrder.Visible = false;
                lblUnitState.Visible = true;

                lblUnitState.Location = new Point(58, 102);
                panel12.Visible = false;
            }

            return dsStationDetail;
        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            Edt_SN.Enabled = false;
            Edt_ChildSN.Enabled = false;

            GetStationData();
            if (S_IsCheckPN == "1")
            {
                GrpControlPart.Visible = true;
            }
            else
            {
                GrpControlPart.Visible = false;
                MessageInfo.Add_Info_MSG(Edt_MSG, "NG", "QC station need to setting unit state, so 'IsCheckPN' must be enabled.");
                //dsBOM = PartSelectSVC.MESGetBomPartInfo(Convert.ToInt32(Com_Part.EditValue.ToString()),                                   List_Login.StationTypeID);
            }

        }

        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            try
            {
                base.Btn_ConfirmPO_Click(sender, e);
                base.Btn_Defect.Enabled = true;
                base.Com_luUnitStatus.Enabled = true;
                Edt_SN.Enabled = true;
                Edt_SN.Focus();
                if (isEnableCOF && S_IsCheckPO == "1")
                {
                    string PO = Com_PO.EditValue.ToString();
                    if (!string.IsNullOrEmpty(PO))
                    {
                        DataSet dsCOF = PartSelectSVC.GetluPODetailDef(Convert.ToInt32(PO), "COF");
                        if (dsCOF != null && dsCOF.Tables.Count > 0 && dsCOF.Tables[0].Rows.Count > 0)
                        {
                            COF = dsCOF.Tables[0].Rows[0]["Content"].ToString() == "1";
                        }
                    }
                }
                SetQC();
            }
            catch { }
        }

        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                List<mesUnit> List_mesUnit = new List<mesUnit>();
                List<mesHistory> List_mesHistory = new List<mesHistory>();
                List<mesUnitDefect> List_mesUnitDefect = new List<mesUnitDefect>();

                DateTime dateStart = DateTime.Now;
                string outString1 = string.Empty;
                string mainSN = Edt_SN.Text.Trim();

                if (mainSN == "")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language, new string[] { "SN:" + mainSN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }
                if (!Regex.IsMatch(mainSN, SN_Pattern))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20027", "NG", List_Login.Language, new string[] { "SN:" + mainSN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }
                string mPartId = null;
                string mPoId = null;
                string mPartFamilyId = Com_PartFamily.EditValue.ToString();
                //S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();
                string S_DefectID = base.DefectChar;
                if (S_luUnitStateID == "2" || S_luUnitStateID == "3")
                {
                    if (S_DefectID == "")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20049", "NG", List_Login.Language, new string[] { "SN:" + mainSN });
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }
                }

                if (S_IsCheckPN == "1")
                {
                    mPartId = Com_Part.EditValue.ToString();
                    if (S_IsCheckPO == "1")
                    {
                        mPoId = Com_PO.EditValue.ToString();
                    }
                }

                if (string.IsNullOrEmpty(mPartId) || string.IsNullOrEmpty(mPoId))
                {
                    DataSet dsMainSN;
                    dsMainSN = PartSelectSVC.uspCallProcedure("uspGetBaseData", mainSN,
                           null, null, null, null, null, ref outString1);
                    if (outString1 != "1" || dsMainSN == null || dsMainSN.Tables[0].Rows.Count <= 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20119", "NG", List_Login.Language, new string[] { "SN:" + mainSN });
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }

                    DataTable dt = dsMainSN.Tables[0];
                    mPoId = string.IsNullOrEmpty(mPoId) ? dt.Rows[0]["ProductionOrderID"].ToString() : mPoId;
                    mPartId = string.IsNullOrEmpty(mPartId) ? dt.Rows[0]["PartID"].ToString() : mPartId;
                    mPartFamilyId = dt.Rows[0]["PartFamilyID"].ToString();
                }

                //重测机制暂时不用
                if (isEnableCOF)
                {
                    DataSet dsCOF = PartSelectSVC.GetluPODetailDef(Convert.ToInt32(mPoId), "COF");
                    if (dsCOF != null && dsCOF.Tables.Count > 0 && dsCOF.Tables[0].Rows.Count > 0)
                    {
                        COF = dsCOF.Tables[0].Rows[0]["Content"].ToString() == "1";
                    }
                }

                DataSet unitDataset = PartSelectSVC.GetAndCheckUnitInfo(mainSN, mPoId, mPartId);
                if (unitDataset == null || unitDataset.Tables.Count <= 0 || unitDataset.Tables[0].Rows.Count <= 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20015", "NG", List_Login.Language, new string[] { "SN:" + mainSN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }
                DataTable DT_Unit = unitDataset.Tables[0];
                if (!isEnableCOF || !COF)
                {
                    //根据工艺路线判断状态
                    DataSet DsRout = PartSelectSVC.GetRouteData(List_Login.LineID.ToString(), mPartId,
                                                                "", mPoId);
                    if (DsRout == null || DsRout.Tables.Count == 0 || DsRout.Tables[0].Rows.Count == 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20195", "NG", List_Login.Language, new string[] { "SN:" + mainSN });
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }

                    //string RoutType = DsRout.Tables[0].Rows[0]["RouteType"].ToString();
                    //string StatusID = DT_Unit.Rows[0]["StatusID"].ToString();

                    //if (RoutType != "1" && StatusID != "1")
                    //{
                    //    MessageInfo.Add_Info_MSG(Edt_MSG, "20036", "NG", List_Login.Language, new string[] { "SN:" + mainSN });
                    //    Edt_SN.Focus();
                    //    Edt_SN.Text = "";
                    //    return;
                    //}
                }

                mS_UnitStateID = "";
                DataSet tmpDataSet = PartSelectSVC.GetmesUnitState(mPartId, mPartFamilyId, "", List_Login.LineID.ToString(), List_Login.StationTypeID, mPoId,  S_luUnitStateID);
               
                //MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] 
                //{ "UnitStatusID:"+ S_luUnitStateID+"  ", S_luUnitStateID });

                if (tmpDataSet != null && tmpDataSet.Tables.Count != 0 && tmpDataSet.Tables[0].Rows.Count != 0)
                {
                    mS_UnitStateID = tmpDataSet.Tables[0].Rows[0]["ID"].ToString().Trim();

                    //MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[]
                    //{ "UnitStateID:"+ mS_UnitStateID+"  ", mS_UnitStateID });
                }
                if (string.IsNullOrEmpty(mS_UnitStateID))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + mainSN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }
                //////////////////////////////////////////////////////
                /// //二次检查是否有指定线路  20231214
                var tmpSecondRet = PartSelectSVC.GetmesUnitStateSecond(mPartId, mPartFamilyId, "", List_Login.LineID.ToString(),
                    List_Login.StationTypeID, mPoId, S_luUnitStateID, mainSN);

                if (tmpSecondRet.StartsWith("Error", StringComparison.OrdinalIgnoreCase))
                {
                    //MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + mainSN });
                    MessageInfo.Add_Info_MSG(Edt_MSG,"NG", tmpSecondRet);
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }

                mS_UnitStateID = tmpSecondRet;
                if (string.IsNullOrEmpty(mS_UnitStateID))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + mainSN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }
                /////////////////////////////////////////////////////
                string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, mainSN);
                if (S_RouteCheck == "1")
                {
                    try
                    {
                        //调用通用过程
                        string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + mPoId + "\"> </ProdOrder>";
                        string xmlPart = "<Part PartID=\"" + mPartId + "\"> </Part>";
                        string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                        string outString = string.Empty;
                        PartSelectSVC.uspCallProcedure("uspQCCheck", mainSN, xmlProdOrder, xmlPart, xmlStation, null, S_luUnitStateID, ref outString);
                        if (outString != "1")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { mainSN, ProMsg }, outString);
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                            return;
                        }


                        mesUnit v_mesUnit = new mesUnit();
                        v_mesUnit.ID = Convert.ToInt32(DT_Unit.Rows[0]["ID"].ToString());
                        v_mesUnit.UnitStateID = Convert.ToInt32(mS_UnitStateID);
                        v_mesUnit.StatusID = Convert.ToInt32(S_luUnitStateID);
                        v_mesUnit.StationID = List_Login.StationID;
                        v_mesUnit.EmployeeID = List_Login.EmployeeID;
                        v_mesUnit.LastUpdate = DateTime.Now;
                        v_mesUnit.ProductionOrderID = Convert.ToInt32(mPoId);
                        //修改 Unit
                        List_mesUnit.Add(v_mesUnit);

                        mesHistory v_mesHistory = new mesHistory();
                        v_mesHistory.UnitID = Convert.ToInt32(v_mesUnit.ID);
                        v_mesHistory.UnitStateID = Convert.ToInt32(mS_UnitStateID);
                        v_mesHistory.EmployeeID = List_Login.EmployeeID;
                        v_mesHistory.StationID = List_Login.StationID;
                        #region howard 20220324      
                        //v_mesHistory.EnterTime = DateTime.Now;
                        //v_mesHistory.ExitTime = DateTime.Now;
                        string tmpCurrentDateTime = PartSelectSVC.GetServerDateTime();
                        DateTime currentDateTime;
                        if (!DateTime.TryParse(tmpCurrentDateTime, out currentDateTime))
                            currentDateTime = DateTime.Now;

                        v_mesHistory.EnterTime = currentDateTime;
                        v_mesHistory.ExitTime = currentDateTime;
                        #endregion
                        v_mesHistory.ProductionOrderID = Convert.ToInt32(mPoId);
                        v_mesHistory.PartID = Convert.ToInt32(mPartId);
                        v_mesHistory.LooperCount = 1;
                        v_mesHistory.StatusID = Convert.ToInt32(S_luUnitStateID);

                        List_mesHistory.Add(v_mesHistory);

                        //////////////////////////  Defect ///////////////////////////////////////////
                        //////////////////////////////////////////////////////////////////////////////
                        string[] Array_Defect = S_DefectID.Split(';');
                        if (S_luUnitStateID != "1")
                        {
                            foreach (var item in Array_Defect)
                            {
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

                                        List_mesUnitDefect.Add(v_mesUnitDefect);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                }
                            }
                            //非Pass产品解绑关联的治具
                            string result = string.Empty;
                            string xmlStationStr = "<Part PartID=\"" + List_Login.StationID.ToString() + "\"> </Part>";
                            string xmlPartStr = "<Part PartID=\"" + mPartId + "\"> </Part>";
                            PartSelectSVC.uspCallProcedure("uspQcReleaseMachine", Edt_SN.Text.ToString(),
                                                                    null, xmlPartStr, xmlStationStr, null, null, ref result);
                        }

                        mesUnit[] L_mesUnit = List_mesUnit.ToArray();
                        mesHistory[] L_mesHistory = List_mesHistory.ToArray();
                        mesUnitDefect[] L_mesUnitDefect = List_mesUnitDefect.ToArray();

                        string ReturnValue = DataCommitSVC.SubmitDataUHD(L_mesUnit, L_mesHistory, L_mesUnitDefect);
                        if (ReturnValue != "OK")
                        {
                            ReturnValue = "SN:" + mainSN + "  " + ReturnValue;
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ReturnValue });
                            return;
                        }

                        /////////////////////////////////////////////////////////////////////////
                        #region Howard 20220322 add update unitDetail table
                        if (isSampleAlerm && Com_luUnitStatus.EditValue.ToString() == "1")
                        {
                            UpdateSampleResult(currentDateTime, mainSN, v_mesUnit.ID);
                        }
                        #endregion
                        /////////////////////////////////////////////////////////////////////////
                        //if (S_luUnitStateID == "1")
                        //{
                        //    base.SetOverStiaonQTY(true);
                        //}
                        //else if (S_luUnitStateID == "2")
                        //{
                        //    base.SetOverStiaonQTY(false);
                        //}

                        base.SetOverStiaonQTY_V2(S_luUnitStateID);

                        Edt_SN.Text = "";
                        Edt_SN.Enabled = true;
                        Edt_SN.Focus();

                        Edt_ChildSN.Text = "";
                        Edt_ChildSN.Enabled = false;
                        List_ChildSN.Items.Clear();

                        TimeSpan ts = DateTime.Now - dateStart;
                        double mill = Math.Round(ts.TotalMilliseconds, 0);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { mainSN, mill.ToString() });

                    }
                    catch (Exception ex)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                    }
                }
                else
                {
                    if (S_RouteCheck == "20243")
                    {
                        string nowUnitStateID = PartSelectSVC.GetSerialNumber2(mainSN).Tables[0].Rows[0]["UnitStateID"].ToString();
                        mesUnitState mesunitSateModel = mesUnitStateSVC.Get(Convert.ToInt32(nowUnitStateID));
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20243", "NG", List_Login.Language, new string[] { mainSN, mesunitSateModel.Description });
                    }
                    else
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(S_RouteCheck, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_RouteCheck);
                    }
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                }
            }
        }

        private void Edt_ChildSN_KeyDown(object sender, KeyEventArgs e)
        {

        }

        public override void Com_luUnitStatus_EditValueChanged(object sender, EventArgs e)
        {
            base.Com_luUnitStatus_EditValueChanged(sender, e);
            S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();
            switch (S_luUnitStateID)
            {
                case "1":
                    //lblDefectCode.ForeColor = Color.Green;
                    lblDefectCode.Visible = false;
                    Edt_SN.BackColor = Com_luUnitStatus.ForeColor;
                    break;
                case "2":
                    lblDefectCode.ForeColor = Color.Red;
                    lblDefectCode.Visible = true;
                    Edt_SN.BackColor = Com_luUnitStatus.ForeColor;
                    break;
                case "3":
                    lblDefectCode.ForeColor = Color.Orange;
                    lblDefectCode.Visible = true;
                    Edt_SN.BackColor = Com_luUnitStatus.ForeColor;
                    break;
                default:
                    lblDefectCode.Visible = false;
                    Edt_SN.BackColor = Color.White;
                    break;
            }

            if (lblDefectCode.Visible == false)
            {
                Edt_SN.Focus();
            }
        }

        public override void btnDefectSave_Click(object sender, EventArgs e)
        {
            base.btnDefectSave_Click(sender, e);
            lblDefectCode.Text = "DefectCode:" + DefectCode;

            Edt_SN.Focus();
        }

        private int GetStationParam(string columnName, DataRow[] dr1, DataRow[] dr2, DataRow[] dr3)
        {

            isSampleAlerm = dr1[0][columnName].ToString().Trim() == "1";
            if (isSampleAlerm)
            {
                if (dr2.Length > 0 && int.TryParse(dr2[0][columnName].ToString().Trim(), out int count))
                    sampleCount = count;

                if (sampleCount <= 0)
                    return 70024;

                sampleTimeContext = dr3.Length > 0 ? dr3[0][columnName].ToString().Trim().Replace("，", ",").Replace("：", ":").Replace("；", ";") : "";

                if (string.IsNullOrEmpty(sampleTimeContext))
                    return 70025;

                string[] timeGroup = sampleTimeContext.Split(';');
                if (timeGroup.Length <= 0)
                    return 70026;

                for (int i = 0; i < timeGroup.Length; i++)
                {
                    string[] timeKey = timeGroup[i].Split(',');
                    if (timeKey.Length != 3)
                        return 70026;

                    if (DateTime.TryParse(timeKey[0], out DateTime dateTime) && int.TryParse(timeKey[1], out int LowLimit) && int.TryParse(timeKey[2], out int UpLimit))
                    {
                        if (LowLimit > 0 || LowLimit < -60)
                            return 70027;
                        if (UpLimit < 0 || UpLimit > 60)
                            return 70028;

                        timeSpec.Add(new Tuple<DateTime, DateTime, DateTime>(dateTime, dateTime.AddMinutes(LowLimit), dateTime.AddMinutes(UpLimit)));
                    }
                }
            }
            return 1;
        }
        private void checkStationParams(string columnName, DataRow[] dr1, DataRow[] dr2, DataRow[] dr3)
        {
            int res = GetStationParam(columnName, dr1, dr2, dr3);
            if (res != 1)
            {
                //添加不良信息
                string ProMsg = MessageInfo.GetMsgByCode(res.ToString(), List_Login.Language);
                //条码:{0} 错误信息:{1}.
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, res.ToString());
            }
        }

        private void UpdateSampleResult(DateTime currentTime, string sn, int unitId)
        {
            //查询是否符合时间区间
            var v = timeSpec.Find(x => x.Item2.TimeOfDay.CompareTo(currentTime.TimeOfDay) == -1 && x.Item3.TimeOfDay.CompareTo(currentTime.TimeOfDay) == 1);
            if (v == null)
                return;

            DateTime dateTimeS = DateTime.Parse(currentTime.ToString("yyyy-MM-dd ") + v.Item2.TimeOfDay);
            DateTime dateTimeE = DateTime.Parse(currentTime.ToString("yyyy-MM-dd ") + v.Item3.TimeOfDay);
            string result = PartSelectSVC.GetSampleCount(dateTimeS.ToString("yyyy-MM-dd HH:mm:ss"), dateTimeE.ToString("yyyy-MM-dd HH:mm:ss"), isStationType, List_Login.StationID);

            if (int.TryParse(result, out int res))
            {
                if (res < sampleCount)
                {
                    mesUnitDetail tmpMesUnitDetail = mesUnitDetailSVC.GetUnitDetail(unitId);
                    tmpMesUnitDetail.reserved_04 = currentTime.ToString("yyyy-MM-dd");
                    string tmpRes = mesUnitDetailSVC.Update(tmpMesUnitDetail);
                    if (tmpRes != "OK")
                    {
                        tmpRes = "SN:" + sn + ",  update Failed," + tmpRes;
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { tmpRes });
                        return;
                    }

                    MessageInfo.Add_Info_MSG(Edt_MSG, "70029", "OK", List_Login.Language, new string[] { sn });
                    MessageInfo.ShowMessageBoxLoseFocus("70029", List_Login.Language, MessageBoxIcon.Warning, new string[] { sn });
                }
            }
            else
            {
                LogNet.LogEor(this.Name, "get sample count failed.");
            }
        }

        private void QCV2_Form_Load(object sender, EventArgs e)
        {
            base.FrmBase_Load(sender, e);

            if (S_IsCheckPN == "0")
            {
                GrpControlPart.Visible = false;
                //QC工站不适合设置NoPN，要设置不良代码
                MessageInfo.Add_Info_MSG(Edt_MSG, "NG", "At QC station, 'IsCheckPN' must be enabled.");
            }

            ///////////////////////////////////////////
            #region Howard 20220322 add paramerts
            try
            {
                DataSet stationDs = PartSelectSVC.GetPLCSeting("", List_Login.StationID.ToString());
                DataSet stationTypeDs = PartSelectSVC.MESGetStationTypeParameter(List_Login.StationTypeID);


                if (stationDs != null && stationDs.Tables[0] != null && stationDs.Tables[0].Rows.Count > 0)
                {
                    DataRow[] dr1 = stationDs.Tables[0].Select("Name = 'IsSampleAlarm'");
                    DataRow[] dr2 = stationDs.Tables[0].Select("Name = 'SampleCount'");
                    DataRow[] dr3 = stationDs.Tables[0].Select("Name = 'SampleAlarmTimePoint'");
                    isStationType = false;
                    if (dr1.Length > 0)
                        checkStationParams("Value", dr1, dr2, dr3);
                }
                else
                {
                    //检查站点类型
                    if (stationTypeDs != null && stationTypeDs.Tables[0] != null && stationTypeDs.Tables[0].Rows.Count > 0)
                    {
                        DataRow[] dr1 = stationTypeDs.Tables[0].Select("Description = 'IsSampleAlarm'");
                        DataRow[] dr2 = stationTypeDs.Tables[0].Select("Description = 'SampleCount'");
                        DataRow[] dr3 = stationTypeDs.Tables[0].Select("Description = 'SampleAlarmTimePoint'");
                        isStationType = true;
                        if (dr1.Length > 0)
                            checkStationParams("Content", dr1, dr2, dr3);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }

            #endregion
            ///////////////////////////////////////////
        }
    }
}