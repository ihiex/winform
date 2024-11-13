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
using System.Text.RegularExpressions;
using App.Model;

namespace App.MyMES
{
    public partial class PalletPackage_Form : FrmBase
    {
        DataTable DT_PackageData = null;
        string PalletSN;
        bool isPacking = false;
        bool IsGeneratePalletSN = false;
        LabelManager2.Application LabSN;
        string PalletSNFormatName;
        string PalletLabelTemplatePath;
        string PalletSN_Pattern;
        string xmlProdOrder;
        string xmlPart;
        string xmlExtraData;
        string xmlStation;
        int Allnumber;
        DataTable DT_PrintSn;

        public PalletPackage_Form()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 刷新重新
        /// </summary>
        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            if (DT_PackageData != null && DT_PackageData.Rows.Count > 0 && !isPacking)
            {
                string ProMsg = MessageInfo.GetMsgByCode("10034", List_Login.Language);
                if (MessageBox.Show(ProMsg, "prompt",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            if (DT_PackageData != null && DT_PackageData.Rows.Count > 0)
            {
                DT_PackageData.Clear();
            }
            base.Btn_Refresh_Click(sender, e);
        }

        /// <summary>
        /// 重写确认菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet dtSet = base.PartSelectSVC.GetProductionOrderDetailDef(Com_PO.EditValue.ToString());
                if (dtSet == null || dtSet.Tables.Count == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20070", "NG", List_Login.Language);
                    return;
                }
                DataTable dtPara = dtSet.Tables[0];
                DataRow[] drUpc = dtPara.Select("Description='UPC'");

                if (drUpc.Count() == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20071", "NG", List_Login.Language);
                    return;
                }

                DataRow[] drMpn = dtPara.Select("Description='MPN'");

                if (drMpn.Count() == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20072", "NG", List_Login.Language);
                    return;
                }

                DataRow[] drboxFullNumber = dtPara.Select("Description='PalletQty'");
                if (drboxFullNumber.Count() == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20091", "NG", List_Login.Language);
                    return;
                }

                DataRow[] drPalletSN_Pattern = dtPara.Select("Description='PalletSN_Pattern'");
                if (drPalletSN_Pattern.Count() == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20092", "NG", List_Login.Language);
                    return;
                }
                txtMpn.Text = drMpn[0]["Content"].ToString();
                txtUpc.Text = drUpc[0]["Content"].ToString();
                PalletSN_Pattern = drPalletSN_Pattern[0]["Content"].ToString().Replace("\\\\", "\\");
                Allnumber = Convert.ToInt32(drboxFullNumber[0]["Content"].ToString());
                txtNumber.Text = drboxFullNumber[0]["Content"].ToString();
                txtNowNumber.Text = "0";
                base.Btn_ConfirmPO_Click(sender, e);

                DataRow[] drGeneratePalletSN = dtPara.Select("Description='IsGeneratePalletSN'");
                xmlProdOrder = ("<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>");
                xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                xmlExtraData = "<ExtraData EmployeeId=\"" + List_Login.EmployeeID.ToString() + "\"> </ExtraData>";
                xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";

                //初始化参数
                DT_PackageData = null;
                PalletSN = string.Empty;
                Edt_MSG.Text = string.Empty;

                if (drGeneratePalletSN.Count() == 0 || drGeneratePalletSN[0]["Content"].ToString() == "0")
                {
                    Edt_SN.Text = string.Empty;
                    Edt_SN.Enabled = true;
                    Edt_SN.Focus();
                    Edt_ChildSN.Text = string.Empty;
                    Edt_ChildSN.Enabled = false;
                    IsGeneratePalletSN = false;
                }
                else
                {
                    // 查询模板
                    string S_StationTypeID = List_Login.StationTypeID.ToString();
                    string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                    string S_PartID = Com_Part.EditValue.ToString();
                    string S_ProductionOrderID = Com_PO.EditValue.ToString();
                    string S_LoginLineID = List_Login.LineID.ToString();

                    PalletSNFormatName = PartSelectSVC.mesGetSNFormatIDByList(S_PartID, S_PartFamilyID, S_LoginLineID,
                         Com_PO.EditValue.ToString(), List_Login.StationTypeID.ToString());
                    if (string.IsNullOrEmpty(PalletSNFormatName))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20093", "NG", List_Login.Language);
                        return;
                    }

                    PalletLabelTemplatePath = public_.GetLabelName(PartSelectSVC, S_StationTypeID, S_PartFamilyID,
                                                                S_PartID, S_ProductionOrderID, S_LoginLineID);

                    if (string.IsNullOrEmpty(PalletLabelTemplatePath))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20076", "NG", List_Login.Language);
                        return;
                    }
                    else
                    {
                        string pathList = string.Empty;
                        string[] ListTemplate = PalletLabelTemplatePath.Split(';');
                        foreach (string str in ListTemplate)
                        {
                            string[] listStr = str.Split(',');
                            pathList = (string.IsNullOrEmpty(pathList) ? "" : pathList + ";") + listStr[1].ToString();
                        }
                        txtTemplate.Text = pathList.Replace(@"\\", @"\");
                    }

                    if (DT_PackageData == null)
                    {
                        DT_PackageData = new DataTable();
                        DT_PackageData.Columns.Add("SEQNO", typeof(int));
                        DT_PackageData.Columns.Add("KITSN", typeof(string));
                        DT_PackageData.Columns.Add("TIME", typeof(string));
                        gdControlData.DataSource = DT_PackageData;
                    }

                    Edt_SN.Text = string.Empty;
                    Edt_SN.Enabled = false;
                    Edt_ChildSN.Text = string.Empty;
                    Edt_ChildSN.Enabled = true;
                    Edt_ChildSN.Focus();
                    IsGeneratePalletSN = true;
                }
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void LoadDataFromPalletSN()
        {
            if (!Regex.IsMatch(Edt_SN.Text.ToUpper().Trim(), PalletSN_Pattern))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20027", "NG", List_Login.Language, new string[] { Edt_SN.Text.ToUpper().Trim() });
                return;
            }
            string result = PartSelectSVC.uspPalletCheck(Edt_SN.Text.ToUpper().Trim(), xmlProdOrder, xmlPart, xmlStation, xmlExtraData, "PALLET");

            if (result != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Edt_SN.Text.ToUpper().Trim(), ProMsg }, result);
                Edt_SN.SelectAll();
                return;
            }
            else
            {
                isPacking = false;
                PalletSN = Edt_SN.Text.ToUpper().Trim();
                DataSet ds = PartSelectSVC.Get_PalletData(Edt_SN.Text.ToUpper().Trim());
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    DT_PackageData = new DataTable();
                    DT_PackageData.Columns.Add("SEQNO", typeof(int));
                    DT_PackageData.Columns.Add("KITSN", typeof(string));
                    DT_PackageData.Columns.Add("TIME", typeof(string));
                    txtNowNumber.Text = "0";
                }
                else
                {
                    DT_PackageData = ds.Tables[0];
                    txtNowNumber.Text = ds.Tables[0].Rows.Count.ToString();

                    //判断是否已经包装完成
                    DataTable DT_Package = PartSelectSVC.GetMesPackageStatusID(PalletSN).Tables[0];

                    if (DT_Package != null && DT_Package.Rows.Count > 0 && Convert.ToInt32(DT_Package.Rows[0]["StatusID"]) == 1)
                    {
                        isPacking = true;
                    }
                }
                if (isPacking)
                {
                    Edt_SN.SelectAll();
                    Edt_ChildSN.Enabled = false;
                    btnClosePallet.Enabled = false;
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10017", "OK", List_Login.Language, new string[] { PalletSN });
                }
                else
                {
                    Edt_SN.Enabled = false;
                    Edt_ChildSN.Enabled = true;
                    Edt_ChildSN.Focus();
                    if (DT_PackageData.Rows.Count > 0)
                    {
                        btnClosePallet.Enabled = true;
                    }
                }
                gdControlData.DataSource = DT_PackageData;
            }
        }


        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(Edt_SN.Text.Trim()))
            {
                LoadDataFromPalletSN();
            }
        }

        private void Edt_ChildSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(Edt_ChildSN.Text.Trim()))
            {
                comitDataV2();
            }
        }
        private void comitDataV2()
        {
            int NowNumber = string.IsNullOrEmpty(txtNowNumber.Text.Trim()) ? 0 : Convert.ToInt32(txtNowNumber.Text.Trim());
            if (Allnumber <= NowNumber)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20086", "NG", List_Login.Language);
                return;
            }

            string result = PartSelectSVC.uspPalletCheck(Edt_ChildSN.Text.ToUpper().Trim(), xmlProdOrder, xmlPart, xmlStation, xmlExtraData, "BOX");
            if (result != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Edt_ChildSN.Text.ToUpper().Trim(), ProMsg }, result);
                Edt_ChildSN.SelectAll();
                return;
            }
            else
            {
                //包装校验
                PartSelectSVC.uspCallProcedure("uspPackageRouteCheck", Edt_ChildSN.Text.ToUpper().Trim(), xmlProdOrder, xmlPart,
                                                            xmlStation, xmlExtraData, "", ref result);

                if (result != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Edt_ChildSN.Text.ToUpper().ToString(), ProMsg }, result);
                    Edt_ChildSN.SelectAll();
                    return;
                }

                //生成栈板SN
                if (IsGeneratePalletSN && string.IsNullOrEmpty(PalletSN))
                {
                    mesUnit mesUnit = new mesUnit();
                    mesUnit.StationID = List_Login.StationID;
                    mesUnit.EmployeeID = List_Login.EmployeeID;
                    mesUnit.ProductionOrderID = Convert.ToInt32(Com_PO.EditValue);
                    mesUnit.PartID = Convert.ToInt32(Com_Part.EditValue);
                    result = PartSelectSVC.Get_CreatePackageSN(PalletSNFormatName, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, mesUnit, ref PalletSN, 2);
                    if (result != "1")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Edt_ChildSN.Text.Trim(), ProMsg }, result);
                        Edt_ChildSN.SelectAll();
                        return;
                    }
                    Edt_SN.Text = PalletSN;
                }
                int boxCount = 0;
                result = PartSelectSVC.uspPalletPackagingV2(Com_Part.EditValue.ToString(),
                            Com_PO.EditValue.ToString(), Edt_ChildSN.Text.ToUpper().Trim(), PalletSN, List_Login, Allnumber,ref boxCount);
                if (result == "1")
                {
                    Edt_ChildSN.SelectAll();
                    btnClosePallet.Enabled = true;
                    txtNowNumber.Text = boxCount.ToString();
                    DataRow dr = DT_PackageData.NewRow();
                    dr["SEQNO"] = boxCount.ToString();
                    dr["KITSN"] = Edt_ChildSN.Text.ToUpper().Trim();
                    dr["TIME"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    DT_PackageData.Rows.Add(dr);
                    Edt_MSG.Text = string.Empty;

                    if (boxCount == Allnumber)
                    {
                        isPacking = true;
                        if (IsGeneratePalletSN)
                        {
                            if (DT_PrintSn != null)
                            {
                                DT_PrintSn.Columns.Clear();
                                DT_PrintSn.Rows.Clear();
                            }
                            else
                            {
                                DT_PrintSn = new DataTable();
                            }
                            DT_PrintSn.Columns.Add("SN");
                            DT_PrintSn.Columns.Add("CreateTime");
                            DT_PrintSn.Columns.Add("PrintTime");
                            DataRow DR = DT_PrintSn.NewRow();
                            DR["SN"] = PalletSN;
                            DR["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            DT_PrintSn.Rows.Add(DR);

                            string PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, PalletLabelTemplatePath,
                                DT_PrintSn, Convert.ToInt32(Com_Part.EditValue.ToString()));
                            if (PrintResult != "OK")
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20095", "NG", List_Login.Language, new string[] { PalletSN, PrintResult });

                            }
                            else
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "10018", "OK", List_Login.Language, new string[] { PalletSN });
                            }

                            Edt_SN.Text = string.Empty;
                            Edt_ChildSN.Text = string.Empty;
                            Edt_SN.Enabled = false;
                            Edt_ChildSN.Enabled = true;
                            btnClosePallet.Enabled = false;
                            Edt_ChildSN.Focus();
                        }
                        else
                        {
                            Edt_SN.Text = string.Empty;
                            Edt_ChildSN.Text = string.Empty;
                            Edt_SN.Enabled = true;
                            Edt_ChildSN.Enabled = false;
                            btnClosePallet.Enabled = false;
                            Edt_SN.Focus();
                            MessageInfo.Add_Info_MSG(Edt_MSG, "10019", "OK", List_Login.Language, new string[] { PalletSN });
                        }
                        //参数初始化
                        isPacking = true;
                        txtNowNumber.Text = string.Empty;
                        PalletSN = string.Empty;
                        DT_PackageData.Clear();
                    }
                    else
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "10020", "OK", List_Login.Language, new string[] { Edt_ChildSN.Text.Trim(), PalletSN });
                        Edt_ChildSN.Text = string.Empty;
                        Edt_ChildSN.Focus();
                    }
                }
                else
                {
                    string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, result);
                    Edt_ChildSN.SelectAll();
                    return;
                }
            }
        }

        private void commitDataV1()
        {
            int NowNumber = string.IsNullOrEmpty(txtNowNumber.Text.Trim()) ? 0 : Convert.ToInt32(txtNowNumber.Text.Trim());
            if (Allnumber <= NowNumber)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20086", "NG", List_Login.Language);
                return;
            }

            string result = PartSelectSVC.uspPalletCheck(Edt_ChildSN.Text.Trim(), xmlProdOrder, xmlPart, xmlStation, xmlExtraData, "BOX");
            if (result != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Edt_ChildSN.Text.Trim(), ProMsg }, result);
                Edt_ChildSN.SelectAll();
                return;
            }
            else
            {
                //包装校验
                PartSelectSVC.uspCallProcedure("uspPackageRouteCheck", Edt_ChildSN.Text.Trim(), xmlProdOrder, xmlPart,
                                                            xmlStation, xmlExtraData, "", ref result);

                if (result != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Edt_ChildSN.Text.ToString(), ProMsg }, result);
                    Edt_ChildSN.SelectAll();
                    return;
                }

                //生成栈板SN
                if (IsGeneratePalletSN && string.IsNullOrEmpty(PalletSN))
                {
                    mesUnit mesUnit = new mesUnit();
                    mesUnit.StationID = List_Login.StationID;
                    mesUnit.EmployeeID = List_Login.EmployeeID;
                    mesUnit.ProductionOrderID = Convert.ToInt32(Com_PO.EditValue);
                    mesUnit.PartID = Convert.ToInt32(Com_Part.EditValue);
                    result = PartSelectSVC.Get_CreatePackageSN(PalletSNFormatName, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, mesUnit, ref PalletSN, 2);
                    if (result != "1")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Edt_ChildSN.Text.Trim(), ProMsg }, result);
                        Edt_ChildSN.SelectAll();
                        return;
                    }
                    Edt_SN.Text = PalletSN;
                }

                result = PartSelectSVC.uspPalletPackaging(Com_Part.EditValue.ToString(),
                            Com_PO.EditValue.ToString(), Edt_ChildSN.Text.Trim(), PalletSN, List_Login, 0);
                if (result == "1")
                {
                    //int Reuse = (DT_PackageData == null ? 0 : DT_PackageData.Rows.Count) + 1;
                    //当前数量自动去数据库获取而不是自增保证OOBA功能实时性
                    string Reuse = "0";
                    PartSelectSVC.uspCallProcedure("GetPalletNumber", Edt_SN.Text.Trim(), "", "", "", "", "", ref Reuse);

                    Edt_ChildSN.SelectAll();
                    btnClosePallet.Enabled = true;
                    txtNowNumber.Text = Reuse.ToString();
                    DataRow dr = DT_PackageData.NewRow();
                    dr["SEQNO"] = Reuse.ToString();
                    dr["KITSN"] = Edt_ChildSN.Text.Trim();
                    dr["TIME"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    DT_PackageData.Rows.Add(dr);
                    Edt_MSG.Text = string.Empty;

                    if (Convert.ToInt32(Reuse) == Allnumber)
                    {
                        result = PartSelectSVC.uspPalletPackaging(Com_Part.EditValue.ToString(),
                                    Com_PO.EditValue.ToString(), Edt_ChildSN.Text.Trim(), PalletSN, List_Login, Convert.ToInt32(Reuse));
                        if (result == "1")
                        {
                            isPacking = true;
                            if (IsGeneratePalletSN)
                            {
                                if (DT_PrintSn != null)
                                {
                                    DT_PrintSn.Columns.Clear();
                                    DT_PrintSn.Rows.Clear();
                                }
                                else
                                {
                                    DT_PrintSn = new DataTable();
                                }
                                DT_PrintSn.Columns.Add("SN");
                                DT_PrintSn.Columns.Add("CreateTime");
                                DT_PrintSn.Columns.Add("PrintTime");
                                DataRow DR = DT_PrintSn.NewRow();
                                DR["SN"] = PalletSN;
                                DR["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                DT_PrintSn.Rows.Add(DR);

                                string PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, PalletLabelTemplatePath,
                                    DT_PrintSn, Convert.ToInt32(Com_Part.EditValue.ToString()));
                                if (PrintResult != "OK")
                                {
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20095", "NG", List_Login.Language, new string[] { PalletSN, PrintResult });

                                }
                                else
                                {
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "10018", "OK", List_Login.Language, new string[] { PalletSN });
                                }

                                Edt_SN.Text = string.Empty;
                                Edt_ChildSN.Text = string.Empty;
                                Edt_SN.Enabled = false;
                                Edt_ChildSN.Enabled = true;
                                btnClosePallet.Enabled = false;
                                Edt_ChildSN.Focus();
                            }
                            else
                            {
                                Edt_SN.Text = string.Empty;
                                Edt_ChildSN.Text = string.Empty;
                                Edt_SN.Enabled = true;
                                Edt_ChildSN.Enabled = false;
                                btnClosePallet.Enabled = false;
                                Edt_SN.Focus();
                                MessageInfo.Add_Info_MSG(Edt_MSG, "10019", "OK", List_Login.Language, new string[] { PalletSN });
                            }
                            //参数初始化
                            isPacking = true;
                            txtNowNumber.Text = string.Empty;
                            PalletSN = string.Empty;
                            DT_PackageData.Clear();
                        }
                        else
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20096", "NG", List_Login.Language, new string[] { PalletSN, ProMsg }, result);
                            return;
                        }
                    }
                    else
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "10020", "OK", List_Login.Language, new string[] { Edt_ChildSN.Text.Trim(), PalletSN });
                        Edt_ChildSN.Text = string.Empty;
                        Edt_ChildSN.Focus();
                    }
                }
                else
                {
                    string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, result);
                    Edt_ChildSN.SelectAll();
                    return;
                }
            }
        }

        private void btnClosePallet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Edt_SN.Text.Trim()) || DT_PackageData.Rows.Count == 0)
            {
                return;
            }
            string ProMsg = MessageInfo.GetMsgByCode("10035", List_Login.Language);
            if (MessageBox.Show(ProMsg, "prompt",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }
            string result = PartSelectSVC.uspPalletPackaging(Com_Part.EditValue.ToString(), Com_PO.EditValue.ToString(), Edt_ChildSN.Text.Trim(),
                                                                PalletSN, List_Login, DT_PackageData.Rows.Count);
            if (result == "1")
            {
                if (IsGeneratePalletSN)
                {
                    if (DT_PrintSn != null)
                    {
                        DT_PrintSn.Columns.Clear();
                        DT_PrintSn.Rows.Clear();
                    }
                    else
                    {
                        DT_PrintSn = new DataTable();
                    }
                    DT_PrintSn.Columns.Add("SN");
                    DT_PrintSn.Columns.Add("CreateTime");
                    DT_PrintSn.Columns.Add("PrintTime");
                    DataRow DR = DT_PrintSn.NewRow();
                    DR["SN"] = PalletSN;
                    DR["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    DT_PrintSn.Rows.Add(DR);

                    string PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, PalletLabelTemplatePath,
                                        DT_PrintSn, Convert.ToInt32(Com_Part.EditValue.ToString()));
                    if (PrintResult != "OK")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20095", "NG", List_Login.Language, new string[] { PalletSN, PrintResult });

                    }
                    else
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "10018", "OK", List_Login.Language, new string[] { PalletSN });
                    }
                }

                MessageInfo.Add_Info_MSG(Edt_MSG, "10019", "OK", List_Login.Language, new string[] { Edt_SN.Text.Trim() });
                Edt_SN.Text = "";
                Edt_ChildSN.Text = "";
                Edt_SN.Enabled = true;
                Edt_ChildSN.Enabled = false;
                btnClosePallet.Enabled = false;
                Edt_SN.Focus();

                //参数初始化
                isPacking = true;
                txtNowNumber.Text = string.Empty;
                PalletSN = string.Empty;
                DT_PackageData.Clear();
            }
            else
            {
                ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20096", "NG", List_Login.Language, new string[] { Edt_SN.Text.Trim(), ProMsg }, result);
                return;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            PackageRemove_Form shipMentRePrint = new PackageRemove_Form(List_Login, "2");
            if (shipMentRePrint.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(Edt_SN.Text.Trim()))
                {
                    LoadDataFromPalletSN();
                }
            }
        }

        private void PalletPackage_Form_Load(object sender, EventArgs e)
        {
            public_.OpenBartender(List_Login.StationID.ToString());
        }
    }
}