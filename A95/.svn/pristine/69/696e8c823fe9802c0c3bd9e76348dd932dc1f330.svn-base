using App.Model;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.MyMES
{
    public partial class AssemblyPanle_Form : FrmBase
    {

        bool COF = false;
        int ScanNumber = 1;
        int ScanCurrentNumber = 1;
        bool IsCheckVendor = false;
        string Vendor = string.Empty;
        DataTable dtScanData;

        public AssemblyPanle_Form()
        {
            InitializeComponent();
        }

        public override void FrmBase_Load(object sender, EventArgs e)
        {
            base.FrmBase_Load(sender, e);
            base.Com_luUnitStatus.Enabled = false;
        }

        /// <summary>
        /// 重写确认菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            string PO = Com_PO.EditValue.ToString();
            DataSet dsCOF = PartSelectSVC.GetluPODetailDef(Convert.ToInt32(PO), "COF");
            if (dsCOF != null && dsCOF.Tables.Count > 0 && dsCOF.Tables[0].Rows.Count > 0)
            {
                COF = dsCOF.Tables[0].Rows[0]["Content"].ToString() == "1";
            }

            //判断是否DOE打印
            DataSet dst = PartSelectSVC.MESGetStationTypeParameter(List_Login.StationTypeID);
            DataRow[] drPOE = dst.Tables[0].Select("Description='IsDOEPrint'");
            if (drPOE.Length > 0 && drPOE[0]["Content"].ToString()=="1")
            {
                lblPara.Visible = true;
                panelPara.Visible = true;
                toggleSwitchLock.Visible = true;

                //获取参数1值
                DataSet dtSet = PartSelectSVC.GetProductionOrderDetailDef(Com_PO.EditValue.ToString());
                if (dtSet == null || dtSet.Tables.Count == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20240", "NG", List_Login.Language, new string[] { "DOE_Parameter1" });
                    return;
                }
                else
                {
                    DataTable dtDOE = dtSet.Tables[0];
                    DataRow[] Parameter1 = dtDOE.Select("Description='DOE_Parameter1'");

                    if (Parameter1.Count() == 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20240", "NG", List_Login.Language, new string[] { "DOE_Parameter1" });
                        return;
                    }
                    else
                    {
                        string[] ListPara1 = Parameter1[0]["Content"].ToString().Split(',');
                        foreach (string str in ListPara1)
                        {
                            this.comboxET.Properties.Items.AddRange(new object[] { str });
                        }
                    }
                }
            }
            else
            {
                lblPara.Visible = false;
                panelPara.Visible = false;
                toggleSwitchLock.Visible = false;
            }

            DataRow[] drCheckVendor = dst.Tables[0].Select("Description='IsCheckVendor'");
            if(drCheckVendor!=null&& drCheckVendor.Length>0)
            {
                IsCheckVendor = drCheckVendor[0]["Content"].ToString() == "1" ? true : false;
            }

            DataSet ds = PartSelectSVC.MESGetBomPartInfo(Convert.ToInt32(Com_Part.EditValue.ToString()),
                List_Login.StationTypeID);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count < 2)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20023", "NG", List_Login.Language, new string[] { Com_Part.Text.ToString() });
                return;
            }


            dtScanData = ds.Tables[0];
            dtScanData.Columns.Add("SN", typeof(string));

            //工站类型配置扫描类型替换掉
            DataRow[] drScanType = dst.Tables[0].Select("Description='SNScanType'");
            if (drScanType.Length > 0)
            {
                dtScanData.Rows[0]["ScanType"] = drScanType[0]["Content"];
            }

            ScanNumber = dtScanData.Rows.Count-1;
            InitialContrl();
            base.Btn_ConfirmPO_Click(sender, e);

        }

        private bool VerifyMianCode()
        {
            //工单数量检查
            string outString = string.Empty;
            PartSelectSVC.uspCallProcedure("uspPONumberCheck", Com_PO.EditValue.ToString(),
                    null, null, null, null, "1", ref outString);
            if (outString != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Com_PO.EditValue.ToString(), ProMsg }, outString);
                return false;
            }

            string mainSN = txtMainSN.Text.Trim();
            if (string.IsNullOrEmpty(mainSN))
            {
                return false;
            }
            string mainPattern = dtScanData.Rows[0]["Pattern"].ToString();
            string maincanType = dtScanData.Rows[0]["ScanType"].ToString();
            string mainPartID = dtScanData.Rows[0]["PartID"].ToString();
            if (!new string[] { "3", "4", "6" }.Contains(maincanType))
            {
                //正则校验
                if (!Regex.IsMatch(mainSN, mainPattern.Replace("\\\\", "\\")))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20027", "NG", List_Login.Language, new string[] { mainSN });
                    txtMainSN.SelectAll();
                    txtMainSN.Text = "";
                    return false;
                }
            }
            string msg = string.Empty;
            if (CheckInputBarcode(1, mainSN, mainPattern, maincanType, mainPartID, txtMainSN, ref msg))
            {
                dtScanData.Rows[0]["SN"] = mainSN;
                txtMainSN.Enabled = false;
                txtChildSN.Enabled = true;
                txtChildSN.Text = string.Empty;
                txtChildSN.Focus();
                return true;
            }
            else
            {
                return false;
            }
            
        }

        private void txtMainSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                VerifyMianCode();
            }
        }

        private void txtChildSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string msg = string.Empty;
                string childSN = txtChildSN.Text.Trim();
                if (string.IsNullOrEmpty(childSN))
                {
                    return;
                }

                //判断是否扫描重复
                foreach(DataRow dr in dtScanData.Rows)
                {
                    if(dr["SN"].ToString()== childSN)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20006", "NG", List_Login.Language);
                        txtChildSN.Text = "";
                        txtChildSN.Focus();
                        return;
                    }
                }
                //判断是否需要校验供应商
                if(IsCheckVendor && !string.IsNullOrEmpty(Vendor))
                {
                    if(childSN.Substring(0,2) != Vendor)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { childSN, "供应商与之前扫描不一致,请检查." });
                        txtChildSN.Text = "";
                        txtChildSN.Focus();
                        return;
                    }
                }

                for (int i = 1; i < dtScanData.Rows.Count; i++)
                {
                    string childScanType = dtScanData.Rows[i]["ScanType"].ToString();
                    string childPartID = dtScanData.Rows[i]["PartID"].ToString();
                    string childPattern = dtScanData.Rows[i]["Pattern"].ToString();

                    if (childScanType == "1")
                    {
                       DataTable dtSN= PartSelectSVC.GetmesSerialNumber(childSN).Tables[0];
                        if (dtSN.Rows.Count == 0)
                        {
                            continue;
                        }

                    }

                    if (!string.IsNullOrEmpty(dtScanData.Rows[i]["SN"].ToString()))
                    {
                        continue;
                    }
                    if (!new string[] { "3", "4", "6" }.Contains(childScanType))
                    {
                        //正则校验
                        if (!Regex.IsMatch(childSN, childPattern.Replace("\\\\", "\\")))
                        {
                            continue;
                        }
                    }

                    if (CheckInputBarcode(0, childSN, childPattern, childScanType, childPartID, txtChildSN, ref msg))
                    {
                        if (string.IsNullOrEmpty(Vendor))
                        {
                            Vendor = childSN.Substring(0, 2);
                        }

                        dtScanData.Rows[i]["SN"] = childSN;
                        ScanCurrentNumber++;
                        GrpControlInputData.CustomHeaderButtons[0].Properties.Caption = ScanCurrentNumber.ToString() + "/" + ScanNumber.ToString();
                        if (ScanCurrentNumber == ScanNumber + 1)
                        {
                            if (SubmitData())
                            {
                                InitialContrl();
                            }
                        }
                        else
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "10004", "OK", List_Login.Language, new string[] { childSN });
                            txtChildSN.Text = "";
                            txtChildSN.Focus();
                        }
                        return;
                    }
                    int j = i + 1;
                    if (j >= dtScanData.Rows.Count - 1)
                    {
                        j = dtScanData.Rows.Count - 1;
                    }
                    if (dtScanData.Rows[j]["ScanType"].ToString() != "1")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { msg });
                        return;
                    }
                    //else
                    //{
                    //    if (childScanType == "1")
                    //    {
                    //        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { msg });
                    //        return;
                    //    }
                    //}
                }


                txtChildSN.Text = string.Empty;
                if (!string.IsNullOrEmpty(msg))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { msg });
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20035", "NG", List_Login.Language);
                }
            }
        }

        private void InitialContrl()
        {
            Vendor = string.Empty;
            ScanCurrentNumber = 1;
            txtChildSN.Text = string.Empty;
            txtChildSN.Enabled = false;
            txtMainSN.Text = string.Empty;
            txtMainSN.Enabled = true;
            txtMainSN.Focus();
            GrpControlInputData.CustomHeaderButtons[0].Properties.Caption = ScanCurrentNumber.ToString() + "/" + ScanNumber.ToString();

            for (int j = 0; j < dtScanData.Rows.Count; j++)
            {
                dtScanData.Rows[j]["SN"] = string.Empty;
            }
            toggleSwitchLock_Toggled(null,null);
        }

        /// <summary>
        /// TextEdit输入框验证
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="pre">正则验证</param>
        /// <returns></returns>
        private bool CheckInputBarcode(int type,string barcode,string Pattern, string ScanType, string PartID, TextEdit txtBox, ref string msg)
        {
            try
            {
                string Result = "1";
                string xmlPartStr = "<Part PartID=\"" + PartID + "\"> </Part>";
                string NowBarCode = barcode;
                switch (ScanType)
                {
                    case "1":
                        if (type == 1)
                        {
                            Result = PartSelectSVC.MESAssembleCheckMianSN(Com_PO.EditValue.ToString(), List_Login.LineID,
                                        List_Login.StationID, List_Login.StationTypeID, barcode, COF);
                        }
                        else
                        {
                            Result = PartSelectSVC.MESAssembleCheckOtherSN(barcode, PartID, COF);
                        }
                        break;
                    case "2":
                    case "7":
                        //批次验证
                        PartSelectSVC.uspCallProcedure("uspBatchDataCheck", barcode,
                                                                                null, xmlPartStr, null, null, "1", ref Result);
                        if (Result != "1")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                            if (type == 1)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { barcode, ProMsg }, Result);
                                txtBox.Text = "";
                            }
                            else
                            {
                                msg = ProMsg;
                            }
                            return false;
                        }

                        break;
                    case "3":
                    case "4":
                    case "6":
                        string StationTypeID = List_Login.StationTypeID.ToString();
                        string MainCode = txtMainSN.Text.Trim();
                        string xmlExtraData = "<ExtraData MainCode=\"" + MainCode + "\"> </ExtraData>";
                        PartSelectSVC.uspCallProcedure("uspMachineToolingCheck", barcode,
                                                                                null, xmlPartStr, null, xmlExtraData, StationTypeID, ref Result);

                        if (type == 1 && Result == "1")
                        {
                            //是否夹具初始化工位
                            DataSet dsMachine = PartSelectSVC.GetmesMachine(barcode);
                            string validFrom = dsMachine.Tables[0].Rows[0]["ValidFrom"].ToString();
                            if (validFrom != List_Login.StationTypeID.ToString())
                            {
                                //Tooling 转换BarCode
                                NowBarCode = PartSelectSVC.BuckToFGSN(barcode);
                                Result = PartSelectSVC.MESAssembleCheckMianSN(Com_PO.EditValue.ToString(), List_Login.LineID,
                                                    List_Login.StationID, List_Login.StationTypeID, NowBarCode, COF);
                            }
                        }

                        if (ScanType == "3" && Result == "1")
                        {
                            PartSelectSVC.uspCallProcedure("uspBatchDataCheck", barcode,
                                                        null, xmlPartStr, null, null, "2", ref Result);
                            if (Result != "1")
                            {
                                string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                                if (type == 1)
                                {
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { barcode, ProMsg }, Result);
                                    txtBox.Text = "";
                                }
                                else
                                {
                                    msg = ProMsg;
                                }
                                return false;
                            }
                        }

                        break;
                    case "5":
                        //校验是否存在重复条码
                        bool check = mesUnitComponentSVC.MESCheckChildSerialNumber(barcode);
                        if (!check)
                        {
                            if (type == 1)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20029", "NG", List_Login.Language, new string[] { barcode });
                                txtBox.Text = "";
                            }
                            else
                            {
                                msg = MessageInfo.GetMsgByCode("20029", List_Login.Language);
                            }
                            return false;
                        }
                        break;
                    default:
                        break;
                }

                if (Result != "1")
                {
                    if (type == 1)
                    {
                        if (Result == "20243")
                        {
                            string nowUnitStateID = PartSelectSVC.GetSerialNumber2(NowBarCode).Tables[0].Rows[0]["UnitStateID"].ToString();
                            mesUnitState mesunitSateModel = mesUnitStateSVC.Get(Convert.ToInt32(nowUnitStateID));
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20243", "NG", List_Login.Language, new string[] { NowBarCode, mesunitSateModel.Description });
                        }
                        else
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { barcode, ProMsg }, Result);
                        }

                        txtBox.Text = "";
                    }
                    else
                    {
                        msg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                    }
                    return false;
                }

                //调用通用过程
                string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>";
                string xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                string outString = string.Empty;
                PartSelectSVC.uspCallProcedure("uspAssembleCheck", barcode,
                                                                        xmlProdOrder, xmlPart, xmlStation, null, PartID, ref outString);
                if (outString != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                    if (type == 1)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { barcode, ProMsg }, outString);
                        txtBox.Text = "";
                    }
                    else
                    {
                        msg = ProMsg;
                    }
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                if (type == 1)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                }
                else
                {
                    msg = ex.Message.ToString();
                }
                return false;
            }
        }

        public bool SubmitData()
        {
            DateTime dateStart = DateTime.Now;

            //再次验证主条码
            if(!VerifyMianCode())
            {
                return false;
            }

            mesUnit v_mesUnit = new mesUnit();
            string msg = string.Empty;
            string S_POID = Com_PO.EditValue.ToString();
            string S_PartID = Com_Part.EditValue.ToString();
            string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
            string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, S_PartFamilyID, List_Login.StationTypeID,
                List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());
            if (string.IsNullOrEmpty(S_UnitStateID))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
                return false;
            }
            int MainUnitID = 0;


            for (int i = 0; i < dtScanData.Rows.Count; i++)
            {
                string S_SN = dtScanData.Rows[i]["SN"].ToString();
                string ScanType= dtScanData.Rows[i]["ScanType"].ToString();
                string PartID = dtScanData.Rows[i]["PartID"].ToString();
                string Batch = "";

                if (ScanType == "6")
                {
                    DataSet dtsMac = mesUnitDetailSVC.MesGetBatchIDByBarcodeSN(S_SN);
                    if (dtsMac == null || dtsMac.Tables[0].Rows.Count == 0)
                    {
                        ScanType = "4";
                    }
                    else
                    {
                        ScanType = "3";
                    }
                }

                #region 循环控件数据提交
                if (i == 0)
                {
                    if (ScanType == "4")
                    {
                        string Result = VirtualBarCode(S_SN, S_PartID, S_PartFamilyID, S_UnitStateID, S_POID);
                        if (Result == "NG" || string.IsNullOrEmpty(Result))
                        {
                            return false;
                        }
                        MainUnitID = Convert.ToInt32(Result);
                    }
                    else
                    {
                        if (ScanType == "3")
                        {
                            //找到虚拟条码UnitID
                            DataSet dataSetMachine = PartSelectSVC.BoxSnToBatch(S_SN, out Batch);
                            if (dataSetMachine == null || dataSetMachine.Tables.Count == 0 || dataSetMachine.Tables[0].Rows.Count == 0)
                            {

                                MessageInfo.Add_Info_MSG(Edt_MSG, "20030", "NG", List_Login.Language, new string[] { S_SN });
                                return false;
                            }
                            MainUnitID = Convert.ToInt32(dataSetMachine.Tables[0].Rows[0]["UnitID"].ToString());
                        }
                        else
                        {
                            DataTable DT_UPUnitID = PartSelectSVC.Get_UnitID(S_SN).Tables[0];
                            MainUnitID = Convert.ToInt32(DT_UPUnitID.Rows[0]["UnitID"].ToString());
                        }
                        v_mesUnit.ID = MainUnitID;
                        v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                        v_mesUnit.StatusID = 1;
                        v_mesUnit.StationID = List_Login.StationID;
                        v_mesUnit.EmployeeID = List_Login.EmployeeID;
                        v_mesUnit.ProductionOrderID = Convert.ToInt32(S_POID);
                        //修改 Unit
                        string S_UpdateUnit = mesUnitSVC.Update(v_mesUnit);

                        if (S_UpdateUnit.Substring(0, 1) == "E")
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_UpdateUnit });
                            return false;
                        }
                    }

                    mesHistory v_mesHistory = new mesHistory();
                    string S_POPartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();

                    v_mesHistory.UnitID = MainUnitID;
                    v_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                    v_mesHistory.EmployeeID = List_Login.EmployeeID;
                    v_mesHistory.StationID = List_Login.StationID;
                    v_mesHistory.ProductionOrderID = Convert.ToInt32(S_POID);
                    v_mesHistory.PartID = Convert.ToInt32(S_POPartID);
                    v_mesHistory.LooperCount = 1;
                    v_mesHistory.EnterTime = DateTime.Now;
                    int I_InsertHistory = mesHistorySVC.Insert(v_mesHistory);
                    msg = "Mian Code:" + S_SN;
                }
                else
                {
                    mesUnitComponent v_mesUnitComponent = new mesUnitComponent();

                    if (ScanType == "2" || ScanType == "5" || ScanType == "7")
                    {
                        v_mesUnitComponent.UnitID = MainUnitID;
                        v_mesUnitComponent.UnitComponentTypeID = 1;
                        v_mesUnitComponent.ChildUnitID = 0;
                        if (ScanType == "7")
                        {
                            DataSet Pss = PartSelectSVC.GetmesSerialNumber(S_SN);
                            if (Pss != null && Pss.Tables.Count > 0)
                            {
                                v_mesUnitComponent.ChildUnitID = Convert.ToInt32(Pss.Tables[0].Rows[0]["UnitID"].ToString());
                            }
                        }

                        if (ScanType == "2")
                        {
                            v_mesUnitComponent.ChildSerialNumber = "";
                            v_mesUnitComponent.ChildLotNumber = S_SN;
                        }
                        else
                        {
                            v_mesUnitComponent.ChildSerialNumber = S_SN;
                            v_mesUnitComponent.ChildLotNumber = "";
                        }
                        int partID = Convert.ToInt32(PartID);
                        mesPart part = mesPartSVC.Get(partID);
                        v_mesUnitComponent.ChildPartID = partID;
                        v_mesUnitComponent.ChildPartFamilyID = part.PartFamilyID;
                        v_mesUnitComponent.Position = "";
                        v_mesUnitComponent.InsertedEmployeeID = List_Login.EmployeeID;
                        v_mesUnitComponent.InsertedStationID = List_Login.StationID;
                        v_mesUnitComponent.StatusID = 1;
                    }
                    else
                    {
                        int ChildUnitID;

                        if (ScanType == "3")
                        {
                            //找到虚拟条码UnitID
                            DataSet dataSetMachine = PartSelectSVC.BoxSnToBatch(S_SN, out Batch);
                            if (dataSetMachine == null || dataSetMachine.Tables.Count == 0 || dataSetMachine.Tables[0].Rows.Count == 0)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20030", "NG", List_Login.Language, new string[] { S_SN });
                                return false;
                            }
                            ChildUnitID = Convert.ToInt32(dataSetMachine.Tables[0].Rows[0]["UnitID"].ToString());
                        }
                        else if (ScanType == "4")
                        {
                            string Result = VirtualBarCode(S_SN, PartID, null, S_UnitStateID, S_POID);
                            if (Result == "NG" || string.IsNullOrEmpty(Result))
                            {
                                return false;
                            }
                            ChildUnitID = Convert.ToInt32(Result);
                        }
                        else
                        {
                            DataSet dataSetUnit = PartSelectSVC.GetmesSerialNumber(S_SN);
                            if (dataSetUnit == null || dataSetUnit.Tables.Count == 0 || dataSetUnit.Tables[0].Rows.Count == 0)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20031", "NG", List_Login.Language, new string[] { S_SN });
                                return false;
                            }
                            ChildUnitID = Convert.ToInt32(dataSetUnit.Tables[0].Rows[0]["UnitID"].ToString());
                        }

                        DataSet dsComponent = PartSelectSVC.GetComponent(ChildUnitID);
                        if (dsComponent == null || dsComponent.Tables.Count == 0 || dsComponent.Tables[0].Rows.Count == 0)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20032", "NG", List_Login.Language, new string[] { S_SN });
                            return false;
                        }

                        string S_ChildSerialNumber = dsComponent.Tables[0].Rows[0]["Value"].ToString();
                        int ChildPartFamilyID = Convert.ToInt32(dsComponent.Tables[0].Rows[0]["PartFamilyID"].ToString());

                        v_mesUnitComponent.UnitID = MainUnitID;
                        v_mesUnitComponent.UnitComponentTypeID = 1;
                        v_mesUnitComponent.ChildUnitID = Convert.ToInt32(ChildUnitID);
                        v_mesUnitComponent.ChildSerialNumber = S_ChildSerialNumber;
                        v_mesUnitComponent.ChildLotNumber = Batch;
                        v_mesUnitComponent.ChildPartID = Convert.ToInt32(PartID);
                        v_mesUnitComponent.ChildPartFamilyID = ChildPartFamilyID;
                        v_mesUnitComponent.Position = "";
                        v_mesUnitComponent.InsertedEmployeeID = List_Login.EmployeeID;
                        v_mesUnitComponent.InsertedStationID = List_Login.StationID;
                        v_mesUnitComponent.StatusID = 1;
                    }

                    string S_UC_Result = mesUnitComponentSVC.Insert(v_mesUnitComponent);
                    if (S_UC_Result != "OK")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_UC_Result });
                        return false;
                    }
                    msg = msg + " Child barcode:" + S_SN;
                }

                if (ScanType == "3" || ScanType == "4" || ScanType == "6")
                {
                    mesMachineSVC.MesModMachineBySNStationTypeID(S_SN, List_Login.StationTypeID);
                }

                if (ScanType == "2" || ScanType == "7" || ScanType == "3")
                {
                    string SN = string.Empty;
                    string MachineSN = string.Empty;
                    int BatchType = 1;
                    if (ScanType == "3")
                    {
                        MachineSN = S_SN;
                        BatchType = 2;
                    }
                    else
                    {
                        SN = S_SN;

                    }
                    string ResultStr = PartSelectSVC.ModMesMaterialConsumeInfo(List_Login, BatchType, SN, MachineSN,
                                    Convert.ToInt32(Com_Part.EditValue.ToString()), Convert.ToInt32(Com_PO.EditValue.ToString()));
                    if (ResultStr != "1")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(ResultStr, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, ResultStr);
                    }
                }
                #endregion
            }
            TimeSpan ts = DateTime.Now - dateStart;
            double mill = Math.Round(ts.TotalMilliseconds, 0);
            MessageInfo.Add_Info_MSG(Edt_MSG, "10005", "OK", List_Login.Language, new string[] { msg, mill.ToString() });
            base.SetOverStiaonQTY(true);
            Vendor = string.Empty;
            return true;
        }

        private string VirtualBarCode(string S_SN, string S_PartID, string S_PartFamilyID, string S_UnitStateID, string S_POID)
        {
            try
            {
                string S_InsertUnit = string.Empty;
                //判断是否已经生成了虚拟条码
                string S_FG_SN = PartSelectSVC.BuckToFGSN(S_SN);
                if (string.IsNullOrEmpty(S_FG_SN))
                {
                    //2021-4-16 Max.Xie 产生FGSN 时增加正则校验，如果不符合的话报错，不产生条码
                    string SN_Pattern = string.Empty;
                    //查询SN正则表达式规则
                    DataSet dataSet = PartSelectSVC.GetmesPartDetail(Convert.ToInt32(S_PartID), "SN_Pattern");
                    if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        SN_Pattern = dataSet.Tables[0].Rows[0]["Content"].ToString();
                    }

                    string S_FormatSN = PartSelectSVC.mesGetSNFormatIDByList(S_PartID, S_PartFamilyID,
                        List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), List_Login.StationTypeID.ToString());
                    if (string.IsNullOrEmpty(S_FormatSN))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20034", "NG", List_Login.Language, new string[] { S_PartID });
                        return "NG";
                    }

                    string xmlProdOrder = "'<ProdOrder ProductionOrder=" + "\"" + Com_PO.EditValue.ToString() + "\"" + "> </ProdOrder>'";
                    string xmlStation = "'<Station StationID=" + "\"" + List_Login.StationID.ToString() + "\"" + "> </Station>'";
                    string xmlPart = "'<Part PartID=" + "\"" + S_PartID + "\"" + "> </Part>'";
                    string xmlExtraData = "'<ExtraData BoxSN=" + "\"" + S_SN + "\"" +
                                                   " LineID = " + "\"" + List_Login.LineID.ToString() + "\"" +
                                                   " PartFamilyTypeID=" + "\"" + Com_PartFamilyType.EditValue.ToString() + "\"" +
                                                   " LineType=" + "\"" + "M" + "\""+
                                                   " BB=" + "\"" + comboxET.Text.Trim() + "\"" +
                                                   " CCCC=" + "\"" + txtSPCA.Text.Trim().ToUpper() + "\"" +
                                                   " PP=" + "\"" + txtPP.Text.Trim().ToUpper() + "\"" +
                                                   " B=" + "\"" + txtBuild.Text.Trim() + "\"" + " > </ExtraData>'";
                    DataSet DS = PartSelectSVC.uspSNRGetNext(S_FormatSN, 0, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, null);
                    string New_SN = DS.Tables[1].Rows[0][0].ToString();

                    if (!string.IsNullOrEmpty(SN_Pattern))
                    {
                        if (!Regex.IsMatch(New_SN, SN_Pattern))
                        {
                            //return "20027";
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20027", "NG", List_Login.Language);
                            return "NG";
                        }
                    }

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
                    S_InsertUnit = mesUnitSVC.Insert(v_mesUnit);

                    mesUnitDetail v_mesUnitDetail = new mesUnitDetail();
                    v_mesUnitDetail.UnitID = Convert.ToInt32(S_InsertUnit);
                    v_mesUnitDetail.reserved_01 = S_SN;
                    v_mesUnitDetail.reserved_02 = "";
                    v_mesUnitDetail.reserved_03 = "1";
                    v_mesUnitDetail.reserved_04 = "";
                    v_mesUnitDetail.reserved_05 = "";
                    mesUnitDetailSVC.Insert(v_mesUnitDetail);

                    mesSerialNumber v_mesSerialNumber = new mesSerialNumber();
                    v_mesSerialNumber.UnitID = Convert.ToInt32(S_InsertUnit);
                    v_mesSerialNumber.SerialNumberTypeID = 5;  
                    v_mesSerialNumber.Value = New_SN;
                    mesSerialNumberSVC.Insert(v_mesSerialNumber);
                }
                else
                {
                    DataSet ds = PartSelectSVC.GetmesSerialNumber(S_FG_SN);
                    S_InsertUnit = ds.Tables[0].Rows[0]["UnitID"].ToString();
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
                    v_mesUnit.ID = Convert.ToInt32(S_InsertUnit);
                    mesUnitSVC.Update(v_mesUnit);
                }
                return S_InsertUnit;
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                return "NG";
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            InitialContrl();
        }

        private void toggleSwitchLock_Toggled(object sender, EventArgs e)
        {
            if (toggleSwitchLock.EditValue.ToString() == "True")
            {
                comboxET.Enabled = false;
                txtBuild.Enabled = false;
                txtSPCA.Enabled = false;
                txtPP.Enabled = false;
            }
            else
            {
                comboxET.Enabled = true;
                txtBuild.Enabled = true;
                txtSPCA.Enabled = true;
                txtPP.Enabled = true;
            }
        }
    }
}
