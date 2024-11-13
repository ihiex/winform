using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using App.Model;
using System.Text.RegularExpressions;

namespace App.MyMES
{
    public partial class RepairKitQCV2_Form : FrmBase
    {
        string S_UnitID = "";
        bool COF = false;

        public RepairKitQCV2_Form()
        {
            InitializeComponent();
        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            Edt_SN.Enabled = false;
            Edt_ChildSN.Enabled = false;
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
                string PO = Com_PO.EditValue.ToString();
                DataSet dsCOF = PartSelectSVC.GetluPODetailDef(Convert.ToInt32(PO), "COF");
                if (dsCOF != null && dsCOF.Tables.Count > 0 && dsCOF.Tables[0].Rows.Count > 0)
                {
                    COF = dsCOF.Tables[0].Rows[0]["Content"].ToString() == "1";
                }
                //维修只能选择PASS或SCRAP
                base.Com_luUnitStatus.Properties.DataSource = null;
                DataTable DT = PartSelectSVC.GetluUnitStatus().Tables[0];
                DataTable dtNewStatus = DT.Clone();
                foreach (DataRow dr in DT.Rows)
                {
                    if (dr["ID"].ToString() == "1"  || dr["ID"].ToString() == "3")
                    {
                        DataRow drStatus = dtNewStatus.NewRow();
                        drStatus["ID"] = dr["ID"].ToString();
                        drStatus["Description"] = dr["Description"].ToString();
                        dtNewStatus.Rows.Add(drStatus);
                    }
                }
                base.Com_luUnitStatus.Properties.DataSource = dtNewStatus;
            }
            catch { }
            SetQC();
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
            if (public_.IsOneStationPrint(PartID, PartFamilyID,StationTypeID, 
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
                List<mesUnit> List_mesUnit = new List<mesUnit>();
                List<mesHistory> List_mesHistory = new List<mesHistory>();
                List<mesUnitDefect> List_mesUnitDefect = new List<mesUnitDefect>();

                DateTime dateStart = DateTime.Now;

                string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
                string PartFamilyID = Com_PartFamily.EditValue.ToString();

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

                if (!Regex.IsMatch(S_SN, SN_Pattern))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20027", "NG", List_Login.Language);
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

                string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID, 
                    List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());

                if (string.IsNullOrEmpty(S_UnitStateID))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
                    return;
                }

                //////////////////////////////////////////////////////
                /// //二次检查是否有指定线路  20231214
                var tmpSecondRet = PartSelectSVC.GetmesUnitStateSecond(S_PartID, PartFamilyID, "", List_Login.LineID.ToString(), List_Login.StationTypeID,
                     Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString(), S_SN);

                if (tmpSecondRet.StartsWith("Error", StringComparison.OrdinalIgnoreCase))
                {
                    //MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + mainSN });
                    MessageInfo.Add_Info_MSG(Edt_MSG, "NG", tmpSecondRet);
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }

                S_UnitStateID = tmpSecondRet;
                if (string.IsNullOrEmpty(S_UnitStateID))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                    Edt_SN.Focus();
                    Edt_SN.Text = "";
                    return;
                }
                /////////////////////////////////////////////////////

                int I_FirstScanSequence = FirstScanSequence(S_POPartID, List_Login.StationTypeID);

                {
                    List<string> List_PO = public_.SnToPOID(S_SN);
                    string S_POID = List_PO[0];

                    if (S_POID.Length > 4)
                    {
                        if (S_POID.Substring(0, 5) == "ERROR")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(S_POID.Replace("ERROR", ""), List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_POID.Replace("ERROR", ""));
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

                //string S_Sql = "";
                DataTable DT_SN = PartSelectSVC.GetmesSerialNumber(S_SN).Tables[0]; 

                if (DT_SN.Rows.Count > 0)
                {
                    if(DT_SN.Rows[0]["SerialNumberTypeID"].ToString()!="6")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20035", "NG", List_Login.Language);
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }

                    S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                    DataTable DT_Unit = PartSelectSVC.GetmesUnit(S_UnitID).Tables[0];

                    string S_StationID = List_Login.StationID.ToString();

                    //string S_SerialNumberType = DT_SN.Rows[0]["SerialNumberTypeID"].ToString();
                    //if (S_SerialNumberType != "5" && S_SerialNumberType!="0")
                    //{
                    //    MessageInfo.Add_Info_MSG(Edt_MSG, "20035", "NG", List_Login.Language);
                    //    Edt_SN.Focus();
                    //    Edt_SN.Text = "";
                    //    return;
                    //}
                    if (!COF)
                    {
                        //20230525  提议报废NG可以流
                        //if (DT_Unit.Rows[0]["StatusID"].ToString() != "1")
                        //{
                        //    MessageInfo.Add_Info_MSG(Edt_MSG, "20036", "NG", List_Login.Language);
                        //    Edt_SN.Focus();
                        //    Edt_SN.Text = "";
                        //    return;
                        //}
                    }
                    string FGSN = mesUnitDetailSVC.MesGetFGSNByUPCSN(S_SN);
                    //过站检查的条码由UPC SN更改为FG SN  20230329
                    string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, FGSN);

                    if (S_RouteCheck == "1")
                    {
                        string S_SelectPartID = DT_Unit.Rows[0]["PartID"].ToString();
                        if (S_POPartID == S_SelectPartID)
                        {
                            string S_StationTypeID = List_Login.StationTypeID.ToString();
 
                            DT_ProductStructure = PartSelectSVC.GetmesProductStructure2(S_POPartID, S_StationTypeID).Tables[0];   
                            if (DT_ProductStructure.Rows.Count > 0)
                            {
                                Edt_SN.Enabled = false;
                                Edt_ChildSN.Enabled = true;
                                Edt_ChildSN.Focus();
                            }
                            else
                            {
                                try
                                {
                                    //调用通用过程
                                    string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>";
                                    string xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                                    string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                                    string outString = string.Empty;
                                    PartSelectSVC.uspCallProcedure("uspQCCheck", FGSN,
                                                                                            xmlProdOrder, xmlPart, xmlStation, null, null, ref outString);
                                    if (outString != "1")
                                    {
                                        string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { FGSN, ProMsg }, outString);
                                        Edt_SN.Focus();
                                        Edt_SN.Text = "";
                                        return;
                                    }

                                    string S_POID = DT_ProductionOrder.Rows[0]["ID"].ToString();
                                    DataTable DT_UPUnitID = PartSelectSVC.Get_UnitID(S_SN).Tables[0];
                                    int UPCUnitID = Convert.ToInt32(DT_UPUnitID.Rows[0]["UnitID"].ToString());

                                    mesUnit v_mesUnit = new mesUnit();
                                    v_mesUnit.ID = UPCUnitID;
                                    v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                                    v_mesUnit.StatusID = Convert.ToInt32(S_luUnitStateID);
                                    v_mesUnit.StationID = List_Login.StationID;
                                    v_mesUnit.EmployeeID = List_Login.EmployeeID;
                                    v_mesUnit.LastUpdate = DateTime.Now;
                                    v_mesUnit.ProductionOrderID = Convert.ToInt32(S_POID);
                                    //修改 UPCUnit                                    
                                    //string S_UpdateUnit = mesUnitSVC.Update(v_mesUnit);
                                    //if (S_UpdateUnit.Substring(0, 1) == "E")
                                    //{
                                    //    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_UpdateUnit });
                                    //    return;
                                    //}
                                    List_mesUnit.Add(v_mesUnit);
                                    //修改FG Unit(FG与UPC不一样才执行)
                                    
                                    int FGUnitID = 0;
                                    if(FGSN!= S_SN)
                                    {
                                        DataTable DT_FGUnitID = PartSelectSVC.Get_UnitID(FGSN).Tables[0];
                                        FGUnitID = Convert.ToInt32(DT_FGUnitID.Rows[0]["UnitID"].ToString());
                                        var FgUnit = DeepCopyByReflection(v_mesUnit);
                                        FgUnit.ID = FGUnitID;
                                        List_mesUnit.Add(FgUnit);

                                        //修改 UPCUnit                                       
                                        //S_UpdateUnit = mesUnitSVC.Update(v_mesUnit);
                                        //if (S_UpdateUnit.Substring(0, 1) == "E")
                                        //{
                                        //    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_UpdateUnit });
                                        //    return;
                                        //}
                                    }


                                    //if (S_UpdateUnit.Substring(0, 1) != "E")
                                    {
                                        mesHistory v_mesHistory = new mesHistory();

                                        v_mesHistory.UnitID = UPCUnitID;
                                        v_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                                        v_mesHistory.EmployeeID = List_Login.EmployeeID;
                                        v_mesHistory.StationID = List_Login.StationID;
                                        v_mesHistory.ProductionOrderID = Convert.ToInt32(S_POID);
                                        v_mesHistory.PartID = Convert.ToInt32(S_POPartID);
                                        v_mesHistory.LooperCount = 1;
                                        v_mesHistory.StatusID = Convert.ToInt32(S_luUnitStateID);
                                        //int I_InsertHistory = mesHistorySVC.Insert(v_mesHistory);
                                        List_mesHistory.Add(v_mesHistory);
                                        if (FGSN != S_SN)
                                        {
                                            var FgHistory = DeepCopyByReflection(v_mesHistory);
                                            FgHistory.UnitID = FGUnitID;
                                            List_mesHistory.Add(FgHistory);
                                            //mesHistorySVC.Insert(v_mesHistory);
                                        }
                                        


                                        //////////////////////////  Defect ///////////////////////////////////////////
                                        //////////////////////////////////////////////////////////////////////////////
                                        string[] Array_Defect = S_DefectID.Split(';');
                                        if (S_luUnitStateID != "1")
                                        {
                                            foreach (var item in Array_Defect)
                                            {
                                                //string S_mesUnitDefectInsert = "";
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
                                                        if (FGSN != S_SN)
                                                        {
                                                            var FgDefect = DeepCopyByReflection(v_mesUnitDefect);
                                                            FgDefect.UnitID = UPCUnitID;
                                                            List_mesUnitDefect.Add(FgDefect);
                                                        }
                                                        //S_mesUnitDefectInsert = mesUnitDefectSVC.Insert(v_mesUnitDefect);
                                                    }
                                                }
                                                catch(Exception ex)
                                                {
                                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                                }
                                            }
                                            //非Pass产品解绑关联的治具
                                            string result = string.Empty;
                                            string xmlStationStr = "<Part PartID=\"" + List_Login.StationID.ToString() + "\"> </Part>";
                                            string xmlPartStr = "<Part PartID=\"" + Com_Part.EditValue.ToString() + "\"> </Part>";
                                            PartSelectSVC.uspCallProcedure("uspQcReleaseMachine", Edt_SN.Text.ToString(),
                                                                                    null, xmlPartStr, xmlStationStr, null, null, ref result);
                                        }

                                        mesUnit[] L_mesUnit = List_mesUnit.ToArray();
                                        mesHistory[] L_mesHistory = List_mesHistory.ToArray();
                                        mesUnitDefect[] L_mesUnitDefect = List_mesUnitDefect.ToArray();

                                        string ReturnValue = DataCommitSVC.SubmitDataUHD(L_mesUnit, L_mesHistory, L_mesUnitDefect);
                                        if (ReturnValue != "OK")
                                        {
                                            ReturnValue = "SN:" + S_SN + "  " + ReturnValue;
                                            MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ReturnValue });
                                            return;
                                        }

                                        /////////////////////////////////////////////////////////////////////////
                                        /////////////////////////////////////////////////////////////////////////
                                        //if (S_luUnitStateID == "1")
                                        //{
                                        //    base.SetOverStiaonQTY(true);
                                        //}
                                        //else if(S_luUnitStateID == "2")
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
                                        MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { S_SN, mill.ToString() });
                                    }
                                }
                                catch(Exception ex)
                                {
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
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
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_RouteCheck);
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
        public static T DeepCopyByReflection<T>(T obj)
        {
            if (obj is string || obj.GetType().IsValueType)
                return obj;

            object retval = Activator.CreateInstance(obj.GetType());
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            foreach (var field in fields)
            {
                try
                {
                    field.SetValue(retval, DeepCopyByReflection(field.GetValue(obj)));
                }
                catch { }
            }

            return (T)retval;
        }
        private void Edt_ChildSN_KeyDown(object sender, KeyEventArgs e)
        {

        }

        public override void Com_luUnitStatus_EditValueChanged(object sender, EventArgs e)
        {
            base.Com_luUnitStatus_EditValueChanged(sender, e);
            string S_luUnitStateID = Com_luUnitStatus.EditValue.ToString();
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
                    Edt_SN.BackColor = Com_luUnitStatus.ForeColor;
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
        
    }
}