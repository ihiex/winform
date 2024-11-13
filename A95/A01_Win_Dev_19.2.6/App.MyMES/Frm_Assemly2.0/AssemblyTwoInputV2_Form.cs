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
    // 2021-11-16 测试通过
    public partial class AssemblyTwoInputV2_Form : FrmBase
    {

        bool COF = false;
        int ScanNumber = 1;
        int ScanCurrentNumber = 1;
        bool IsCheckVendor = false;
        string Vendor = string.Empty;
        DataTable dtScanData;

        string S_ScanTypeNew = string.Empty;
        string S_ProductionOrderID = string.Empty;
        string S_PartOld = string.Empty;
        string S_MainPartID = string.Empty;
        string S_PartFamilyID = string.Empty;
        string S_PartFamilyTypeID = string.Empty;

        //string IsNoPOCheck = "0";
        //string IsNoPNCheck = "0";
        string S_IsCheckPO = "1";
        string S_IsCheckPN = "1";

        public AssemblyTwoInputV2_Form()
        {
            InitializeComponent();
        }

        private DataSet GetStationData()
        {
            DataSet dsStationDetail = PartSelectSVC.GetPLCSeting("", List_Login.StationID.ToString());
            DataSet DS_StationTypeDef = PartSelectSVC.MESGetStationTypeParameter(List_Login.StationTypeID);

            //DataRow[] drIsNoPOCheck = dsStationDetail.Tables[0].Select("Name='IsNoPOCheck'");
            //if (drIsNoPOCheck.Length > 0) IsNoPOCheck = drIsNoPOCheck[0]["Value"].ToString(); else IsNoPOCheck = "0";

            //DataRow[] drIsNoPNCheck = dsStationDetail.Tables[0].Select("Name='IsNoPNCheck'");
            //if (drIsNoPNCheck.Length > 0) IsNoPNCheck = drIsNoPNCheck[0]["Value"].ToString(); else IsNoPNCheck = "0";

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
                Com_PO.Visible = true;
            }
            else
            {
                lblProductionOrder.Visible = false;
                Com_PO.Visible = false;
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


        public override void FrmBase_Load(object sender, EventArgs e)
        {
            base.FrmBase_Load(sender, e);
            base.Com_luUnitStatus.Enabled = false;

            GetStationData();

            if (S_IsCheckPN == "1")
            {
                GrpControlPart.Visible = true;
            }
            else
            {
                GrpControlPart.Visible = false;
            }

            if (S_IsCheckPO == "1" || S_IsCheckPN == "1")
            {
                                
            }

            DataSet dst = PartSelectSVC.MESGetStationTypeParameter(List_Login.StationTypeID);
            DataRow[] drCheckVendor = dst.Tables[0].Select("Description='IsCheckVendor'");
            if (drCheckVendor != null && drCheckVendor.Length > 0)
            {
                IsCheckVendor = drCheckVendor[0]["Content"].ToString() == "1" ? true : false;
            }
            //工站类型配置扫描类型替换掉
            DataRow[] drScanType = dst.Tables[0].Select("Description='SNScanType'");
            if (drScanType.Length > 0)
            {
                S_ScanTypeNew = drScanType[0]["Content"].ToString();
            }

            if (S_IsCheckPN == "0")
            {
                base.GrpControlInputData.Enabled = true;
                //base.Btn_ConfirmPO_Click(sender, e);
                InitialContrl();

                tableLayoutPanel2.RowStyles[0].Height = 1;
                tableLayoutPanel2.RowStyles[1].Height = 30;
                tableLayoutPanel2.RowStyles[2].Height = 30;

                tableLayoutPanel2.RowStyles[3].Height = 1;
                tableLayoutPanel2.RowStyles[4].Height = 1;
                tableLayoutPanel2.RowStyles[5].Height = 150;
                List_SN.Height = 150;
            }
            else
            {
                tableLayoutPanel2.RowStyles[0].Height = 1;
                tableLayoutPanel2.RowStyles[1].Height = 30;
                tableLayoutPanel2.RowStyles[2].Height = 30;

                tableLayoutPanel2.RowStyles[3].Height = 30;
                tableLayoutPanel2.RowStyles[4].Height = 1;
                tableLayoutPanel2.RowStyles[5].Height = 150;
                List_SN.Height = 150;
            }
        }

        /// <summary>
        /// 重写确认菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            //DataSet dsStationDetail = PartSelectSVC.GetPLCSeting("", List_Login.StationID.ToString());
            GetStationData();

            if (S_IsCheckPO == "1")
            {
                if (S_IsCheckPN == "1")
                {
                    string PO = Com_PO.EditValue.ToString();
                    S_ProductionOrderID = PO;

                    if (Com_PO.Properties.DataSource==null  || PO=="")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20114", "NG", List_Login.Language);
                        return;
                    }


                    DataSet dsCOF = PartSelectSVC.GetluPODetailDef(Convert.ToInt32(PO), "COF");
                    if (dsCOF != null && dsCOF.Tables.Count > 0 && dsCOF.Tables[0].Rows.Count > 0)
                    {
                        COF = dsCOF.Tables[0].Rows[0]["Content"].ToString() == "1";
                    }

                    //判断是否DOE打印
                    DataSet dst = PartSelectSVC.MESGetStationTypeParameter(List_Login.StationTypeID);
                    DataRow[] drPOE = dst.Tables[0].Select("Description='IsDOEPrint'");
                    if (drPOE.Length > 0 && drPOE[0]["Content"].ToString() == "1")
                    {
                        lblPara.Visible = true;
                        panelPara.Visible = true;
                        toggleSwitchLock.Visible = true;

                        //获取参数1值
                        DataSet dtSet = PartSelectSVC.GetProductionOrderDetailDef(S_ProductionOrderID);
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
                    if (drCheckVendor != null && drCheckVendor.Length > 0)
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

                    List_SN.Items.Clear(); 
                    dtScanData = ds.Tables[0];
                    dtScanData.Columns.Add("SN", typeof(string));

                    //工站类型配置扫描类型替换掉
                    DataRow[] drScanType = dst.Tables[0].Select("Description='SNScanType'");
                    if (drScanType.Length > 0)
                    {
                        dtScanData.Rows[0]["ScanType"] = drScanType[0]["Content"];
                    }
                    ScanNumber = dtScanData.Rows.Count - 1;
                    InitialContrl();
                }

                base.Btn_ConfirmPO_Click(sender, e);
                InitialContrl();
                if (S_IsCheckPN == "0")
                {
                    txtMainSN.Select();
                }
            }

            if (S_IsCheckPO == "1")
            {
                lblProductionOrder.Visible = true;
                Com_PO.Visible = true;
            }
            else
            {
                lblProductionOrder.Visible = false;
                Com_PO.Visible = false;

                base.GrpControlInputData.Enabled = true;
                base.Btn_ConfirmPO_Click(sender, e);
                InitialContrl();
            }
        }

        private bool VerifyMianCode()
        {
            string outString = string.Empty;
            string mainSN = txtMainSN.Text.Trim();
            S_ProductionOrderID = Com_PO.EditValue.ToString();
            S_MainPartID = Com_Part.EditValue.ToString();
            S_PartFamilyID = Com_PartFamily.EditValue.ToString();
            S_PartFamilyTypeID = Com_PartFamilyType.EditValue.ToString(); 

            if (string.IsNullOrEmpty(mainSN))
            {
                txtMainSN.Focus();
                txtMainSN.Text = string.Empty;
                return false;
            }

            //第一个工站不适合设置 ,因为第一个工站需要绑定夹具，然后生成SN，第一个工站如果设置  uspGetBaseData 无法校验
            //if (IsNoPOCheck == "1" || IsNoPNCheck == "1")
            if (S_IsCheckPO=="0" || S_IsCheckPN=="0")
            {
                DataSet dsMainSN;
                dsMainSN = PartSelectSVC.uspCallProcedure("uspGetBaseData", mainSN,
                       null, null, null, null, null, ref outString);
                if (outString != "1" || dsMainSN == null || dsMainSN.Tables[0].Rows.Count <= 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20119", "NG", List_Login.Language);
                    txtMainSN.Text = string.Empty;
                    return false;
                }

                DataTable dt = dsMainSN.Tables[0];
                S_ProductionOrderID = dt.Rows[0]["ProductionOrderID"].ToString();
                S_MainPartID = dt.Rows[0]["PartID"].ToString();
                S_PartFamilyID = dt.Rows[0]["PartFamilyID"].ToString();
                S_PartFamilyTypeID = dt.Rows[0]["PartFamilyTypeID"].ToString();
            }

            if (S_IsCheckPO == "0" && S_IsCheckPN == "1")
            {
                //max.xie 验证料号是否一致  2021-11-22 tony 修改判断
                if (S_MainPartID != Com_Part.EditValue.ToString())
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20043", "NG", List_Login.Language, new string[] { mainSN });
                    txtMainSN.Focus();
                    txtMainSN.Text = string.Empty;
                    return false;
                }
            }

            //工单数量检查
            PartSelectSVC.uspCallProcedure("uspPONumberCheck", S_ProductionOrderID,
                    null, null, null, null, "1", ref outString);
            if (outString != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_ProductionOrderID, ProMsg }, outString);
                txtMainSN.Text = string.Empty;
                return false;
            }
            //第一个工站不适合设置 ,因为第一个工站需要绑定夹具，然后生成SN
            if (S_IsCheckPO == "0" || S_IsCheckPN == "0")
            {
                if (S_MainPartID != S_PartOld || string.IsNullOrEmpty(S_PartOld))
                {

                    DataSet ds = PartSelectSVC.MESGetBomPartInfo(Convert.ToInt32(S_MainPartID),
                        List_Login.StationTypeID);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count < 2)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20023", "NG", List_Login.Language, new string[] { S_MainPartID });
                        return false;
                    }
                    //max.xie 20210520 扫子码时再次验证主码，会将dtScanData中子码SN清除掉
                    if (txtChildSN.Text == "")
                    {                        
                        dtScanData = ds.Tables[0];
                        ScanNumber = dtScanData.Rows.Count - 1;
                        dtScanData.Columns.Add("SN", typeof(string));

                        //工站类型配置扫描类型替换掉                        
                        if (S_ScanTypeNew!="" && S_ScanTypeNew!=null)
                        {
                            dtScanData.Rows[0]["ScanType"] = S_ScanTypeNew;
                        }
                    }
                }
                else
                {
                    S_PartOld = S_MainPartID;
                }

                GrpControlInputData.CustomHeaderButtons[0].Properties.Caption = ScanCurrentNumber.ToString() + "/" + ScanNumber.ToString();
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
                    return false;
                }
            }

            string msg = string.Empty;
            if (CheckInputBarcode(1, mainSN, mainPattern, maincanType, mainPartID, txtMainSN, ref msg))
            {
                if (dtScanData.Rows[0]["SN"].ToString() == "")
                {
                    dtScanData.Rows[0]["SN"] = mainSN;
                }
                txtMainSN.Enabled = false;
                txtChildSN.Enabled = true;
                txtChildSN.Text = string.Empty;
                txtChildSN.Focus();
                List_SN.Items.Clear();
                
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
                if (VerifyMianCode())
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10004", "OK", List_Login.Language, new string[] { txtMainSN.Text.Trim()});
                }
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
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20006", "NG", List_Login.Language, new string[] { "SN:" + childSN });
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
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { "SN:" + childSN, "供应商与之前扫描不一致,请检查." });
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
                    if (!new string[] { "3", "4", "6","7" }.Contains(childScanType))
                    {
                        //正则校验
                        if (!Regex.IsMatch(childSN, childPattern.Replace("\\\\", "\\")))
                        {
                            continue;
                        }
                    }
                    /////////////////////////////////////////////////////////////
                    //20221123  针对3,4,6扫描类型，再次进行判断实际绑定的料号是否匹配
                    if (new string[] { "3", "4", "6" }.Contains(childScanType))
                    {
                        DataSet mPartDataSet = public_.GetPartIdByMachineSN(childSN, PartSelectSVC);
                        if (mPartDataSet == null || mPartDataSet.Tables.Count != 1 || mPartDataSet.Tables[0].Rows.Count != 1 || mPartDataSet.Tables[0].Rows[0][0].ToString() != childPartID)
                        {
                            continue;
                        }
                    }

                    if (childScanType == "7")
                    {
                        DataSet partIdByMachineSn = public_.GetPartIdByMaterialUnitSN(childSN, PartSelectSVC);
                        if (partIdByMachineSn == null || partIdByMachineSn.Tables.Count != 1 || partIdByMachineSn.Tables[0].Rows.Count != 1 || partIdByMachineSn.Tables[0].Rows[0][0].ToString() != childPartID)
                        {
                            continue;
                        }
                    }

                    /////////////////////////////////////////////////////////////

                    if (CheckInputBarcode(0, childSN, childPattern, childScanType, childPartID, txtChildSN, ref msg))
                    {
                        if (string.IsNullOrEmpty(Vendor))
                        {
                            Vendor = childSN.Substring(0, 2);
                        }


                        dtScanData.Rows[i]["SN"] = childSN;
                        List_SN.Items.Add(childSN);

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
                            MessageInfo.Add_Info_MSG(Edt_MSG, "10004", "OK", List_Login.Language, new string[] {childSN });
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
                        //MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { msg });

                        string ProMsg = MessageInfo.GetMsgByCode(msg, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { childSN, ProMsg },"","");

                        txtChildSN.Text = "";
                        txtChildSN.Focus();
                        return;
                    }
                }


                txtChildSN.Text = string.Empty;
                if (!string.IsNullOrEmpty(msg))
                {
                    //MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { msg });   
                    string ProMsg = MessageInfo.GetMsgByCode(msg, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { childSN, ProMsg }, "", "");

                    txtChildSN.Focus();
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20035", "NG", List_Login.Language);
                    txtChildSN.Focus();
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

            if (dtScanData!=null)
            {
                for (int j = 0; j < dtScanData.Rows.Count; j++)
                {
                    dtScanData.Rows[j]["SN"] = string.Empty;
                }
            }
            List_SN.Items.Clear();
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
                            Result = PartSelectSVC.MESAssembleCheckMianSN(S_ProductionOrderID, List_Login.LineID,
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
                                Result = PartSelectSVC.MESAssembleCheckMianSN(S_ProductionOrderID, List_Login.LineID,
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
                string MainSN = txtMainSN.Text.Trim();
                string xmlExtraDataAss = "<ExtraData MainCode=\"" + MainSN + "\"> </ExtraData>";
                //调用通用过程
                string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + S_ProductionOrderID + "\"> </ProdOrder>";
                string xmlPart = "<Part PartID=\"" + S_MainPartID + "\"> </Part>";
                string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                string outString = string.Empty;
                PartSelectSVC.uspCallProcedure("uspAssembleCheck", barcode,
                                                                        xmlProdOrder, xmlPart, xmlStation, xmlExtraDataAss, PartID, ref outString);
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

            List<mesUnit> List_mesUnit = new List<mesUnit>();
            List<mesHistory> List_mesHistory = new List<mesHistory>();
            List<mesUnitComponent> List_mesUnitComponent = new List<mesUnitComponent>();

            List<mesMaterialConsumeInfo> List_mesMaterialConsumeInfo = new List<mesMaterialConsumeInfo>();
            List<mesMachine> List_mesMachine = new List<mesMachine>();

            //再次验证主条码
            if (!VerifyMianCode())
            {
                return false;
            }

            mesUnit v_mesUnit = new mesUnit();
            string msg = string.Empty;
            //string S_POID = Com_PO.EditValue.ToString();
            //string S_PartID = Com_Part.EditValue.ToString();
            //string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
            //if (IsNoPOCheck == "0")
            if (S_IsCheckPO=="1")
            {
                S_ProductionOrderID = Com_PO.EditValue.ToString();
                S_MainPartID = Com_Part.EditValue.ToString();
                S_PartFamilyID = Com_PartFamily.EditValue.ToString();
            }

            string S_UnitStateID = public_.GetluUnitStatusID(S_MainPartID, S_PartFamilyID, List_Login.StationTypeID,
                List_Login.LineID.ToString(), S_ProductionOrderID, Com_luUnitStatus.EditValue.ToString());
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
                        string Result = VirtualBarCode(S_SN, S_MainPartID, S_PartFamilyID, S_UnitStateID, S_ProductionOrderID);
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

                        mesUnit F_mesUnit = new mesUnit();
                        F_mesUnit.ID = MainUnitID;
                        F_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                        F_mesUnit.StatusID = 1;
                        F_mesUnit.StationID = List_Login.StationID;
                        F_mesUnit.EmployeeID = List_Login.EmployeeID;
                        F_mesUnit.ProductionOrderID = Convert.ToInt32(S_ProductionOrderID);
                        List_mesUnit.Add(F_mesUnit);
                    }

                    mesUnit mesUnit_Part = mesUnitSVC.Get(MainUnitID);
                    string S_POPartID = mesUnit_Part.PartID.ToString();
                    //string S_POPartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();

                    mesHistory F_mesHistory = new mesHistory();
                    F_mesHistory.UnitID = MainUnitID;
                    F_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                    F_mesHistory.EmployeeID = List_Login.EmployeeID;
                    F_mesHistory.StationID = List_Login.StationID;
                    F_mesHistory.ProductionOrderID = Convert.ToInt32(S_ProductionOrderID);
                    F_mesHistory.PartID = Convert.ToInt32(S_POPartID);
                    F_mesHistory.LooperCount = 1;
                    F_mesHistory.StatusID = 1;
                    F_mesHistory.EnterTime = DateTime.Now;

                    List_mesHistory.Add(F_mesHistory);
 
                    msg = msg = "Mian Code:" + S_SN;
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
                            //子料是夹具，要找夹具的工单，如果夹具没有工单就默认0  2022-04-15 修改
                            DataTable DT_ChildPO = PartSelectSVC.GetPO(PartID, "").Tables[0];
                            string S_TollPOID = "0";

                            if (DT_ChildPO != null && DT_ChildPO.Rows.Count > 0)
                            {
                                S_TollPOID = DT_ChildPO.Rows[0]["Id"].ToString();
                            }
                            else
                            {
                                S_TollPOID = "0";
                            }

                            string Result = VirtualBarCode(S_SN, PartID, null, S_UnitStateID, S_TollPOID);

                            //string Result = VirtualBarCode(S_SN, PartID, null, S_UnitStateID, S_ProductionOrderID);
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

                    List_mesUnitComponent.Add(v_mesUnitComponent);

                    msg = msg + " Child barcode:" + S_SN;
                }

                if (ScanType == "3" || ScanType == "4" || ScanType == "6")
                {
                    mesMachine v_mesMachine = new mesMachine();
                    v_mesMachine.SN = S_SN;
                    List_mesMachine.Add(v_mesMachine);

                    //mesMachineSVC.MesModMachineBySNStationTypeID(S_SN, List_Login.StationTypeID);
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

                    mesMaterialConsumeInfo v_mesMaterialConsumeInfo = new mesMaterialConsumeInfo();
                    v_mesMaterialConsumeInfo.ScanType = BatchType;
                    v_mesMaterialConsumeInfo.SN = SN;
                    v_mesMaterialConsumeInfo.MachineSN = MachineSN;
                    v_mesMaterialConsumeInfo.PartID = Convert.ToInt32(S_MainPartID);
                    v_mesMaterialConsumeInfo.ProductionOrderID = Convert.ToInt32(S_ProductionOrderID);

                    List_mesMaterialConsumeInfo.Add(v_mesMaterialConsumeInfo);

                    //string ResultStr = PartSelectSVC.ModMesMaterialConsumeInfo(List_Login, BatchType, SN, MachineSN,
                    //                Convert.ToInt32(S_MainPartID), Convert.ToInt32(S_ProductionOrderID));
                    //if (ResultStr != "1")
                    //{
                    //    string ProMsg = MessageInfo.GetMsgByCode(ResultStr, List_Login.Language);
                    //    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, ResultStr);
                    //}
                }
                #endregion
            }
            //string ReturnValue = PartSelectSVC.ExecSql(sqlValue);

            /////////////////////////////////////////////////
            //20230710  针对在工艺路线中的最后一站，从主线最后一站A站到TT的B站，再从TT的B站回到主线的最后一站A站时，
            //当A站是组装站时，unitcomponent表中会增加子料的绑定记录,遂在这里增加判断，当子料与主料已存在绑定则不插入
            string stationTypeId = List_Login.StationTypeID.ToString();

            //获取此料工序路径
            int I_RouteID = PartSelectSVC.GetRouteID(List_Login.LineID.ToString(), Com_Part.EditValue.ToString(), S_PartFamilyID.ToString(), S_ProductionOrderID.ToString());
            if (I_RouteID == -1)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20195", "NG", List_Login.Language);
                return false;
            }

            DataTable DT_Route = public_.GetRouteType(I_RouteID, PartSelectSVC).Tables[0];
            string S_RouteType = DT_Route.Rows[0]["RouteType"].ToString();
            bool isLastStation = false;
            if (S_RouteType == "1")
            {
                isLastStation = public_.IsDiagramSFCLastStation(stationTypeId, I_RouteID.ToString(), PartSelectSVC);
            }
            else
            {
                isLastStation =
                    public_.IsTableFCLastStation(stationTypeId, I_RouteID.ToString(), S_UnitStateID, PartSelectSVC);
            }

            if (isLastStation)
            {
                List<mesUnitComponent> mucs = new List<mesUnitComponent>();
                foreach (mesUnitComponent unitComponent in List_mesUnitComponent)
                {
                    bool tmpExistComponent = public_.IsExistBindChildPart(unitComponent.UnitID,
                        unitComponent.ChildUnitID, stationTypeId, unitComponent.ChildLotNumber, unitComponent.ChildPartID.ToString(), PartSelectSVC);
                    if (tmpExistComponent)
                        continue;

                    unitComponent.StatusID = tmpExistComponent ? 0 : 1;
                    mucs.Add(unitComponent);
                }
                List_mesUnitComponent.Clear();
                List_mesUnitComponent = mucs;
            }


            /////////////////////////////////////////////////
            
            mesUnit[] L_mesUnit = List_mesUnit.ToArray();
            mesHistory[] L_mesHistory = List_mesHistory.ToArray();
            mesUnitComponent[] L_mesUnitComponent = List_mesUnitComponent.ToArray();
            mesMaterialConsumeInfo[] L_mesMaterialConsumeInfo = List_mesMaterialConsumeInfo.ToArray();
            mesMachine[] L_mesMachine = List_mesMachine.ToArray();

            string ReturnValue = DataCommitSVC.SubmitDataUHC(L_mesUnit, L_mesHistory, L_mesUnitComponent, 
                L_mesMaterialConsumeInfo, L_mesMachine,List_Login);
            if (ReturnValue != "OK")
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ReturnValue });
                return false;
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
                        List_Login.LineID.ToString(), S_ProductionOrderID, List_Login.StationTypeID.ToString());
                    if (string.IsNullOrEmpty(S_FormatSN))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20034", "NG", List_Login.Language, new string[] { S_PartID });
                        return "NG";
                    }

                    string xmlProdOrder = "'<ProdOrder ProductionOrder=" + "\"" + S_ProductionOrderID + "\"" + "> </ProdOrder>'";
                    string xmlStation = "'<Station StationID=" + "\"" + List_Login.StationID.ToString() + "\"" + "> </Station>'";
                    string xmlPart = "'<Part PartID=" + "\"" + S_PartID + "\"" + "> </Part>'";
                    string xmlExtraData = "'<ExtraData BoxSN=" + "\"" + S_SN + "\"" +
                                                   " LineID = " + "\"" + List_Login.LineID.ToString() + "\"" +
                                                   " PartFamilyTypeID=" + "\"" + S_PartFamilyTypeID + "\"" +
                                                   " LineType=" + "\"" + "M" + "\""+
                                                   " BB=" + "\"" + comboxET.Text.Trim() + "\"" +
                                                   " CCCC=" + "\"" + txtSPCA.Text.Trim().ToUpper() + "\"" +
                                                   " PP=" + "\"" + txtPP.Text.Trim().ToUpper() + "\"" +
                                                   " B=" + "\"" + txtBuild.Text.Trim() + "\"" + 
                                                   " ET=" + "\"" + comboxET.Text.Trim() + "\"" +
                                                   " SPCA = " + "\"" + txtSPCA.Text.Trim().ToUpper() + "\"" +                                                     
                                                   "> </ExtraData>'";
                    DataSet DS = PartSelectSVC.uspSNRGetNext(S_FormatSN, 0, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, null);
                    string New_SN = DS.Tables[1].Rows[0][0].ToString();

                    //20221123 增加对3,4,6类型不进行正则判断
                    DataSet scanTypeSet = PartSelectSVC.GetmesPartDetail(Convert.ToInt32(S_PartID), "ScanType");
                    string tmpScanType = string.Empty;
                    if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        tmpScanType = scanTypeSet.Tables[0].Rows[0]["Content"].ToString();
                    }

                    if (!string.IsNullOrEmpty(SN_Pattern) && !new string[]{"3","4","6"}.Contains(tmpScanType))
                    {
                        if (string.IsNullOrEmpty(tmpScanType) || !new string[] { "3", "4", "6" }.Contains(tmpScanType))
                        {
                            if (!Regex.IsMatch(New_SN, SN_Pattern))
                            {
                                //return "20027";
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20027", "NG", List_Login.Language);
                                return "NG";
                            }
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
                    //S_InsertUnit = mesUnitSVC.Insert(v_mesUnit);

                    mesUnitDetail v_mesUnitDetail = new mesUnitDetail();
                    //v_mesUnitDetail.UnitID = Convert.ToInt32(S_InsertUnit);
                    v_mesUnitDetail.reserved_01 = S_SN;
                    v_mesUnitDetail.reserved_02 = "";
                    v_mesUnitDetail.reserved_03 = "1";
                    v_mesUnitDetail.reserved_04 = "";
                    v_mesUnitDetail.reserved_05 = "";
                    //mesUnitDetailSVC.Insert(v_mesUnitDetail);

                    mesSerialNumber v_mesSerialNumber = new mesSerialNumber();
                    //v_mesSerialNumber.UnitID = Convert.ToInt32(S_InsertUnit);
                    v_mesSerialNumber.SerialNumberTypeID = 5;
                    v_mesSerialNumber.Value = New_SN;
                    //mesSerialNumberSVC.Insert(v_mesSerialNumber);

                    string ReturnValue = DataCommitSVC.InsertUDS(v_mesUnit, v_mesUnitDetail, v_mesSerialNumber, S_SN);
                    S_InsertUnit = ReturnValue;

                    //if (ReturnValue.Substring(0, 5) == "ERROR")
                    if (string.IsNullOrEmpty(ReturnValue) || (ReturnValue.Length >= 5 && ReturnValue.Substring(0, 5) == "ERROR"))
                    {
                        S_InsertUnit = "NG";

                        string ProMsg = MessageInfo.GetMsgByCode(ReturnValue, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, "", "");

                    }
                    else
                    {
                        ReturnValue = "SN:" + New_SN;
                        MessageInfo.Add_Info_MSG(Edt_MSG, "66000", "OK", List_Login.Language, new string[] { ReturnValue });
                    }
                }
                else
                {
                    string S_FirstStationType = PartSelectSVC.GetFirstStationType(S_SN);
                    S_InsertUnit = "";
                    if (S_FirstStationType != List_Login.StationTypeID.ToString())
                    {
                        DataSet ds = PartSelectSVC.GetmesSerialNumber(S_FG_SN);
                        S_InsertUnit = ds.Tables[0].Rows[0]["UnitID"].ToString();
                        mesUnit v_mesUnit = new mesUnit();
                        v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                        v_mesUnit.StatusID = 1;
                        v_mesUnit.StationID = List_Login.StationID;
                        v_mesUnit.EmployeeID = List_Login.EmployeeID;
                        //v_mesUnit.CreationTime = DateTime.Now;
                        //v_mesUnit.LastUpdate = DateTime.Now;
                        v_mesUnit.PanelID = 0;
                        v_mesUnit.LineID = List_Login.LineID;
                        v_mesUnit.ProductionOrderID = Convert.ToInt32(S_POID);
                        v_mesUnit.RMAID = 0;
                        v_mesUnit.PartID = Convert.ToInt32(S_PartID);
                        v_mesUnit.LooperCount = 1;
                        v_mesUnit.ID = Convert.ToInt32(S_InsertUnit);
                        //mesUnitSVC.Update(v_mesUnit);

                        string ReturnValue = DataCommitSVC.UpdatemesUnit(v_mesUnit);
                        if (ReturnValue != "OK")
                        {
                            S_InsertUnit = "NG";
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ReturnValue });
                        }
                    }
                    else
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20154", "NG", List_Login.Language);
                        return "NG";
                    }
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

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            GetStationData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
             string ReturnValue = "SN:111";
            MessageInfo.Add_Info_MSG(Edt_MSG, "66000", "OK", List_Login.Language, new string[] { ReturnValue });
        }
    }
}
