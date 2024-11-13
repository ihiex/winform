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
    public partial class SNLinkBatch_Form : FrmBase
    {
        public SNLinkBatch_Form()
        {
            InitializeComponent();
        }

        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            base.Btn_ConfirmPO_Click(sender, e);
            Edt_Batch.Text = string.Empty;
            Edt_SN.Text = string.Empty;
            Edt_Batch.Enabled = true;
            Edt_SN.Enabled = false;
            Edt_Batch.Focus();
        }

        private void Edt_Batch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string S_Batch = Edt_Batch.Text.Trim();
                if (string.IsNullOrEmpty(Batch_Pattern))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20025", "NG", List_Login.Language, new string[] { Com_Part.EditValue.ToString() });
                    Edt_Batch.SelectAll();
                    return;
                }

                if (!Regex.IsMatch(Edt_Batch.Text.Trim(), Batch_Pattern.Replace("\\\\", "\\")))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20047", "NG", List_Login.Language, new string[] { S_Batch });
                    Edt_Batch.SelectAll();
                    return;
                }

                Edt_Batch.Enabled = false;
                Edt_SN.Enabled = true;
                Edt_SN.Focus();
                Edt_SN.SelectAll();
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
            if (public_.IsOneStationPrint(PartID, PartFamilyID, StationTypeID, List_Login.LineID.ToString(), Com_PO.EditValue.ToString()))
            {
                I_Result = 2;
            }
            return I_Result;
        }

        string S_UnitID = "";
        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DateTime dateStart = DateTime.Now;
                string ScanSN = Edt_SN.Text.ToString();
                try
                {
                    string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                    string PartFamilyID = Com_PartFamily.EditValue.ToString();
                    string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID, 
                        List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());
                    if (string.IsNullOrEmpty(S_UnitStateID))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }
                    string S_DefectID = base.DefectChar;
                    string S_BatchSN = Edt_Batch.Text.Trim();
                    string S_SN = Edt_SN.Text.Trim();

                    if (S_SN == "")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }


                    if (S_BatchSN == S_SN)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20048", "NG", List_Login.Language);
                        return;
                    }

                    //工单  PartID
                    string S_POPartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                    //选择的  PartID
                    string S_SelectPartID = Com_Part.EditValue.ToString(); 
                    // luUnitStatus  ID
                    string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();  

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
                    if (base.I_RouteSequence > I_FirstScanSequence)  //不是第一个工序时根据  SN  获取工单
                    {
                        List<string> List_PO = public_.SnToPOID(S_SN);
                        string S_POID = List_PO[0];

                        if (S_POID.Length > 4)
                        {
                            if (S_POID.Substring(0, 5) == "ERROR")
                            {
                                string ProMsg = MessageInfo.GetMsgByCode(S_POID.Replace("ERROR", ""), List_Login.Language);
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN.ToString(), ProMsg }, S_POID.Replace("ERROR", ""));
                                Edt_SN.Focus();
                                Edt_SN.Text = "";
                                return;
                            }
                        }

                        if (S_POID == "" || S_POID == "0")
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20014", "NG", List_Login.Language, new string[] { S_SN.ToString() });
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                            return;
                        }
                        Com_PO.Text = S_POID;
                    }

                    string S_Sql = "";
                    DataTable DT_SN = PartSelectSVC.GetmesSerialNumber(S_SN).Tables[0];

                    if (DT_SN.Rows.Count > 0)
                    {
                        S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                        DataTable DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0];
                        string S_StationID = List_Login.StationID.ToString(); 

                        string S_SerialNumberType = DT_SN.Rows[0]["SerialNumberTypeID"].ToString();
                        //if (S_SerialNumberType != "5" && S_SerialNumberType != "0")
                        //{
                        //    MessageInfo.Add_Info_MSG(Edt_MSG, "20035", "NG", List_Login.Language);
                        //    Edt_SN.Focus();
                        //    Edt_SN.Text = "";
                        //    return;
                        //}

                        if (DT_Unit.Rows[0]["UnitStateID"].ToString() == "2")
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20036", "NG", List_Login.Language);
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                            return;
                        }

                        if (S_POPartID != DT_Unit.Rows[0]["PartID"].ToString())
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20050", "NG", List_Login.Language);
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
                                // 本工位不判断 子料                                                     
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
                                    string S_InsertUnitDetail = "";
                                    try
                                    {
                                        //////////////////////////////////////  mesUnitDetail /////////////////////////////////////////////////////
                                        mesUnitDetail mesUnitDetail = mesUnitDetailSVC.GetUnitDetail(v_mesUnit.ID);

                                        mesUnitDetail v_mesUnitDetail = new mesUnitDetail();
                                        v_mesUnitDetail.ID = mesUnitDetail.ID;
                                        v_mesUnitDetail.UnitID = Convert.ToInt32(v_mesUnit.ID);
                                        v_mesUnitDetail.reserved_01 = S_SN;
                                        v_mesUnitDetail.reserved_02 = S_BatchSN;
                                        v_mesUnitDetail.reserved_03 = "";
                                        v_mesUnitDetail.reserved_04 = "";
                                        v_mesUnitDetail.reserved_05 = "";

                                        ///// webservice
                                        S_InsertUnitDetail = mesUnitDetailSVC.Update(v_mesUnitDetail);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
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
                                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
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
                                            catch (Exception ex)
                                            {
                                                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                            }
                                        }
                                    }
                                    /////////////////////////////////////////////////////////////////////////
                                    /////////////////////////////////////////////////////////////////////////
                                    if (S_luUnitStateID == "1")
                                    {
                                        base.SetOverStiaonQTY(true);
                                    }
                                    else if (S_luUnitStateID == "2")
                                    {
                                        base.SetOverStiaonQTY(false);
                                    }

                                    TimeSpan ts = DateTime.Now - dateStart;
                                    double mill = Math.Round(ts.TotalMilliseconds, 0);
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "10007", "OK", List_Login.Language, new string[] { ScanSN, S_BatchSN });

                                    Edt_SN.Text = "";
                                    Edt_SN.Focus();
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
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_RouteCheck);
                            Edt_SN.Focus();
                            Edt_SN.Text = "";
                        }
                    }
                    else
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language,new string[] { S_SN });
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
                            LogNet.LogDug(this.Name, "Station:" + List_Login.StationType + ",BatchNo:" + Edt_Batch.Text.ToString() + ",SN:" +
                                                    ScanSN + ",Time：" + mill.ToString() + "ms");
                        }
                        else
                        {
                            LogNet.LogEor(this.Name, "Station:" + List_Login.StationType + ",BatchNo:" + Edt_Batch.Text.ToString() + ",SN:" +
                                                    ScanSN + ",Time：" + mill.ToString() + "ms");
                        }
                    }
                }
            }
        }
    }
}