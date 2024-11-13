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
using System.Configuration;
using System.Threading;

namespace App.MyMES
{
    public partial class BoxPackage_Form : FrmBase
    {

        LabelManager2.Application LabSN;
        DataTable DT_PackageData = null;
        bool IsGenerateBoxSN = false;                       //是否需要自动生成箱号条码
        bool isPacking = false;                             //是否包装完成
        bool IsScanOnlyFGSN = false;                        //只扫描FGSN包装
        string BoxSNFormatName;
        string BoxLabelTemplate;
        string BoxSN_Pattern;
        string BoxSN;
        string xmlProdOrder;
        string xmlPart;
        string xmlExtraData;
        string xmlStation;
        int Allnumber;
        DataTable DT_PrintSn;

        public BoxPackage_Form()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 刷新重写
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
            IsScanOnlyFGSN = false;
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
                DataSet dtSet = PartSelectSVC.GetProductionOrderDetailDef(Com_PO.EditValue.ToString());
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

                DataRow[] drboxFullNumber = dtPara.Select("Description='BoxQty'");
                if (drboxFullNumber.Count() == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20073", "NG", List_Login.Language);
                    return;
                }

                DataRow[] drBoxSN_Pattern = dtPara.Select("Description='BoxSN_Pattern'");
                if (drBoxSN_Pattern.Count() == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20074", "NG", List_Login.Language);
                    return;
                }

                DataRow[] dtScanFGSN = dtPara.Select("Description='MultipackScanOnlyFGSN'");
                if (dtScanFGSN.Count() > 0)
                {
                    IsScanOnlyFGSN = dtScanFGSN[0]["Content"].ToString() == "1" ? true : false;
                    this.UPCSN.Visible = !IsScanOnlyFGSN;
                }

                txtMpn.Text = drMpn[0]["Content"].ToString();
                txtUpc.Text = drUpc[0]["Content"].ToString();
                BoxSN_Pattern = drBoxSN_Pattern[0]["Content"].ToString().Replace("\\\\", "\\");
                Allnumber = Convert.ToInt32(drboxFullNumber[0]["Content"].ToString());
                txtNumber.Text = drboxFullNumber[0]["Content"].ToString();
                txtNowNumber.Text = "0";
                base.Btn_ConfirmPO_Click(sender, e);

                DataRow[] drGenerateBoxSN = dtPara.Select("Description='IsGenerateBoxSN'");
                xmlProdOrder = ("<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>");
                xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                xmlExtraData = "<ExtraData EmployeeId=\"" + List_Login.EmployeeID.ToString() + "\"> </ExtraData>";
                xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";

                //初始化参数
                DT_PackageData = null;
                BoxSN = string.Empty;
                Edt_MSG.Text = string.Empty;
                btnReprint.Enabled = false;

                if (drGenerateBoxSN.Count() == 0 || drGenerateBoxSN[0]["Content"].ToString() == "0")
                {
                    Edt_SN.Text = string.Empty;
                    Edt_SN.Enabled = true;
                    Edt_ChildSN.Text = string.Empty;
                    Edt_ChildSN.Enabled = false;
                    IsGenerateBoxSN = false;
                    Edt_SN.Focus();
                }
                else
                {
                    // 查询模板
                    string S_StationTypeID = List_Login.StationTypeID.ToString();
                    string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                    string S_PartID = Com_Part.EditValue.ToString();
                    string S_ProductionOrderID = Com_PO.EditValue.ToString();
                    string S_LoginLineID = List_Login.LineID.ToString();


                    BoxSNFormatName = PartSelectSVC.mesGetSNFormatIDByList(S_PartID, S_PartFamilyID, S_LoginLineID,
                         Com_PO.EditValue.ToString(), List_Login.StationTypeID.ToString());
                    if (string.IsNullOrEmpty(BoxSNFormatName))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20075", "NG", List_Login.Language);
                        return;
                    }

                    BoxLabelTemplate = public_.GetLabelName(PartSelectSVC, S_StationTypeID, S_PartFamilyID,
                        S_PartID, S_ProductionOrderID, S_LoginLineID);

                    if (string.IsNullOrEmpty(BoxLabelTemplate))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20076", "NG", List_Login.Language);
                        return;
                    }
                    else
                    {
                        string pathList = string.Empty;
                        string[] ListTemplate = BoxLabelTemplate.Split(';');
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
                        DT_PackageData.Columns.Add("UPCSN", typeof(string));
                        DT_PackageData.Columns.Add("SN", typeof(string));
                        DT_PackageData.Columns.Add("TIME", typeof(string));
                        gdControlData.DataSource = DT_PackageData;
                    }

                    Edt_SN.Text = string.Empty;
                    Edt_SN.Enabled = false;
                    Edt_ChildSN.Text = string.Empty;
                    Edt_ChildSN.Enabled = true;
                    Edt_ChildSN.Focus();
                    IsGenerateBoxSN = true;
                }
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                return;
            }
        }

        private void LoadDataFromBoxSN()
        {
            try
            {
                if (!Regex.IsMatch(Edt_SN.Text.Trim(), BoxSN_Pattern))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20027", "NG", List_Login.Language, new string[] { Edt_SN.Text.ToString() });
                    Edt_SN.SelectAll();
                    return;
                }

                string result = string.Empty;
                DataSet dsBoxSN = PartSelectSVC.uspCallProcedure("uspKitBoxCheck", Edt_SN.Text.Trim(), xmlProdOrder, xmlPart,
                                                            xmlStation, xmlExtraData, "BOX", ref result);

                if (result != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Edt_SN.Text.ToString(), ProMsg }, result);
                    Edt_SN.SelectAll();
                    return;
                }
                else
                {
                    isPacking = false;
                    BoxSN = dsBoxSN.Tables[0].Rows[0]["BoxSN"].ToString();
                    Edt_SN.Text = BoxSN;
                    DataSet ds = PartSelectSVC.Get_PackageData(BoxSN);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        DT_PackageData = new DataTable();
                        DT_PackageData.Columns.Add("SEQNO", typeof(int));
                        DT_PackageData.Columns.Add("UPCSN", typeof(string));
                        DT_PackageData.Columns.Add("SN", typeof(string));
                        DT_PackageData.Columns.Add("TIME", typeof(string));
                        txtNowNumber.Text = "0";
                    }
                    else
                    {
                        DT_PackageData = ds.Tables[0];
                        txtNowNumber.Text = ds.Tables[0].Rows.Count.ToString();

                        //判断是否已经包装完成
                        DataTable DT_Package = PartSelectSVC.GetMesPackageStatusID(BoxSN).Tables[0];

                        if (DT_Package != null && DT_Package.Rows.Count > 0 && Convert.ToInt32(DT_Package.Rows[0]["StatusID"]) == 1)
                        {
                            isPacking = true;
                        }
                    }
                    if (isPacking)
                    {
                        Edt_SN.SelectAll();
                        Edt_ChildSN.Enabled = false;
                        btnLastBox.Enabled = false;
                        if (IsGenerateBoxSN)
                        {
                            btnReprint.Enabled = true;
                        }
                        else
                        {
                            btnReprint.Enabled = false;
                        }
                        MessageInfo.Add_Info_MSG(Edt_MSG, "10014", "OK", List_Login.Language, new string[] { BoxSN });
                    }
                    else
                    {
                        Edt_SN.Enabled = false;
                        Edt_ChildSN.Enabled = true;
                        Edt_ChildSN.Focus();
                        btnReprint.Enabled = false;
                        if (DT_PackageData.Rows.Count > 0)
                        {
                            btnLastBox.Enabled = true;
                        }
                    }
                    gdControlData.DataSource = DT_PackageData;
                }
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                return;
            }
        }

        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(Edt_SN.Text.Trim()))
            {
                LoadDataFromBoxSN();
            }
        }

        private void Edt_ChildSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(Edt_ChildSN.Text.Trim()))
            {
                int NowNumber = string.IsNullOrEmpty(txtNowNumber.Text.Trim()) ? 0 : Convert.ToInt32(txtNowNumber.Text.Trim());
                if (Allnumber <= NowNumber)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20086", "NG", List_Login.Language);
                    return;
                }


                //条码验证
                string result = string.Empty;
                DataSet ds = PartSelectSVC.uspCallProcedure("uspKitBoxCheck", Edt_ChildSN.Text.Trim(), xmlProdOrder, xmlPart,
                                                                xmlStation, xmlExtraData, "SN", ref result);
                if (result == "0")
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string palletSN = ds.Tables[0].Rows[0][0].ToString();
                        Edt_SN.Text = palletSN;
                        Edt_SN_KeyDown(sender, e);
                        Edt_ChildSN.Text = string.Empty;
                        return;
                    }
                }
                else if (result != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Edt_ChildSN.Text.ToString(), ProMsg }, result);
                    Edt_ChildSN.Text = "";
                    return;
                }
                else
                {
                    //校验工艺路线
                    string S_SN = Edt_ChildSN.Text.Trim();
                    if (!IsScanOnlyFGSN)
                    {
                        string UPCSN = Edt_ChildSN.Text.Trim();
                        S_SN = mesUnitDetailSVC.MesGetFGSNByUPCSN(UPCSN);
                    }
                    DataTable DT_Unit = PartSelectSVC.GetmesSerialNumber(S_SN).Tables[0];
                    string SN_UnitID = DT_Unit.Rows[0]["UnitID"].ToString();
                    DataTable DT_UnitSN = PartSelectSVC.GetmesUnit(SN_UnitID).Tables[0];
                    if (IsScanOnlyFGSN)
                    {
                        DT_UnitSN.Rows[0]["ProductionOrderID"] = Com_PO.EditValue.ToString();
                    }

                    string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_UnitSN, S_SN);
                    if (S_RouteCheck != "1")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(S_RouteCheck, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_RouteCheck);
                        Edt_ChildSN.SelectAll();
                        return;
                    }

                    //生成箱号SN
                    if (IsGenerateBoxSN && string.IsNullOrEmpty(BoxSN))
                    {
                        mesUnit mesUnit = new mesUnit();
                        mesUnit.StationID = List_Login.StationID;
                        mesUnit.EmployeeID = List_Login.EmployeeID;
                        mesUnit.ProductionOrderID = Convert.ToInt32(Com_PO.EditValue);
                        mesUnit.PartID = Convert.ToInt32(Com_Part.EditValue);
                        result = PartSelectSVC.Get_CreatePackageSN(BoxSNFormatName, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, mesUnit, ref BoxSN, 1);
                        if (result != "1")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, result);
                            Edt_ChildSN.SelectAll();
                            return;
                        }

                        if (!Regex.IsMatch(BoxSN, BoxSN_Pattern))
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20027", "NG", List_Login.Language, new string[] { BoxSN });
                            return;
                        }
                        Edt_SN.Text = BoxSN;
                    }

                    int Reuse = (DT_PackageData == null ? 0 : DT_PackageData.Rows.Count) + 1;
                    result = PartSelectSVC.uspKitBoxPackagingNew(Com_Part.EditValue.ToString(), Com_PO.EditValue.ToString(),
                                                                Edt_ChildSN.Text.Trim(), BoxSN, List_Login, Allnumber, Reuse, IsScanOnlyFGSN ? -1 : 0);
                    if (result == "1")
                    {
                        Edt_ChildSN.SelectAll();
                        btnLastBox.Enabled = true;
                        //txtNowNumber.Text = Reuse.ToString();
                        //DataRow dr = DT_PackageData.NewRow();
                        //dr["SEQNO"] = Reuse.ToString();
                        //if (IsScanOnlyFGSN)
                        //{
                        //    dr["UPCSN"] = string.Empty;
                        //    dr["SN"] = Edt_ChildSN.Text.Trim();
                        //}
                        //else
                        //{
                        //    string FGSN = mesUnitDetailSVC.MesGetFGSNByUPCSN(Edt_ChildSN.Text.Trim());
                        //    dr["UPCSN"] = Edt_ChildSN.Text.Trim();
                        //    dr["SN"] = FGSN;
                        //}
                        //dr["TIME"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //DT_PackageData.Rows.Add(dr);
                        //max 2021-8-23
                        DataSet ds1 = PartSelectSVC.Get_PackageData(BoxSN);

                        DT_PackageData = ds1.Tables[0];
                        txtNowNumber.Text = ds1.Tables[0].Rows.Count.ToString();
                        gdControlData.DataSource = DT_PackageData;

                        Edt_MSG.Text = string.Empty;

                        if (Reuse == Allnumber)
                        {
                            //result = PartSelectSVC.uspKitBoxPackaging(Com_Part.EditValue.ToString(), Com_PO.EditValue.ToString(),
                            //                                            Edt_ChildSN.Text.Trim(), BoxSN, List_Login, Reuse);
                            //if (result == "1")
                            //{
                                if (IsGenerateBoxSN)
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
                                    DR["SN"] = BoxSN;
                                    DR["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    DT_PrintSn.Rows.Add(DR);

                                    string PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, BoxLabelTemplate,
                                        DT_PrintSn, Convert.ToInt32(Com_Part.EditValue.ToString()));
                                    if (PrintResult != "OK")
                                    {
                                        MessageInfo.Add_Info_MSG(Edt_MSG, "20089", "NG", List_Login.Language, new string[] { BoxSN, PrintResult });
                                    }
                                    else
                                    {
                                        MessageInfo.Add_Info_MSG(Edt_MSG, "10015", "OK", List_Login.Language, new string[] { BoxSN });
                                    }

                                    Edt_SN.Text = string.Empty;
                                    Edt_ChildSN.Text = string.Empty;
                                    Edt_SN.Enabled = false;
                                    Edt_ChildSN.Enabled = true;
                                    btnLastBox.Enabled = false;
                                    Edt_ChildSN.Focus();
                                }
                                else
                                {
                                    Edt_SN.Text = string.Empty;
                                    Edt_ChildSN.Text = string.Empty;
                                    Edt_SN.Enabled = true;
                                    Edt_ChildSN.Enabled = false;
                                    btnLastBox.Enabled = false;
                                    Edt_SN.Focus();
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "10014", "OK", List_Login.Language, new string[] { BoxSN });
                                }

                                //参数初始化
                                isPacking = true;
                                txtNowNumber.Text = string.Empty;
                                BoxSN = string.Empty;
                                DT_PackageData.Clear();
                            //}
                            //else
                            //{
                            //    string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                            //    MessageInfo.Add_Info_MSG(Edt_MSG, "20090", "NG", List_Login.Language, new string[] { BoxSN, ProMsg }, result);
                            //    return;
                            //}
                        }
                        else
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "10016", "OK", List_Login.Language, new string[] { Edt_ChildSN.Text.Trim(), BoxSN });
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
        }

        private void btnLastBox_Click(object sender, EventArgs e)
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
            string result = PartSelectSVC.uspKitBoxPackaging(Com_Part.EditValue.ToString(), Com_PO.EditValue.ToString(),
                                                                Edt_ChildSN.Text.Trim(), BoxSN, List_Login, DT_PackageData.Rows.Count);
            if (result == "1")
            {
                if (IsGenerateBoxSN && !string.IsNullOrEmpty(BoxSN))
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
                    DR["SN"] = BoxSN;
                    DR["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    DT_PrintSn.Rows.Add(DR);

                    string PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, BoxLabelTemplate,
                                       DT_PrintSn, Convert.ToInt32(Com_Part.EditValue.ToString()));
                    if (PrintResult != "OK")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20089", "NG", List_Login.Language, new string[] { BoxSN, PrintResult });
                    }
                    else
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "10015", "OK", List_Login.Language, new string[] { BoxSN });
                    }
                    Edt_SN.Text = string.Empty;
                    Edt_ChildSN.Text = string.Empty;
                    Edt_SN.Enabled = false;
                    Edt_ChildSN.Enabled = true;
                    btnLastBox.Enabled = false;
                    Edt_ChildSN.Focus();

                    //参数初始化
                    isPacking = true;
                    txtNowNumber.Text = string.Empty;
                    BoxSN = string.Empty;
                    DT_PackageData.Clear();

                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10014", "OK", List_Login.Language, new string[] { BoxSN });
                    Edt_SN.Text = "";
                    Edt_ChildSN.Text = "";
                    Edt_SN.Enabled = true;
                    Edt_ChildSN.Enabled = false;
                    btnLastBox.Enabled = false;
                    Edt_SN.Focus();
                    DT_PackageData.Clear();
                }
            }
            else
            {
                ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20090", "NG", List_Login.Language, new string[] { BoxSN, ProMsg }, result);
                return;
            }
        }

        private void btnReprint_Click(object sender, EventArgs e)
        {
            try
            {
                string packageSN = Edt_SN.Text.Trim();
                if (string.IsNullOrEmpty(packageSN))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                    Edt_ChildSN.Text = "";
                    return;
                }

                DataSet ds = PartSelectSVC.GetMesPackageStatusID(packageSN);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20127", "NG", List_Login.Language);
                    Edt_ChildSN.Text = "";
                    return;
                }

                string status = ds.Tables[0].Rows[0]["StatusID"].ToString();
                if (status != "1" || DT_PackageData.Rows.Count == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20190", "NG", List_Login.Language);
                    Edt_ChildSN.Text = "";
                    return;
                }

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
                DR["SN"] = packageSN;
                DR["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DT_PrintSn.Rows.Add(DR);

                string PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, BoxLabelTemplate,
                                   DT_PrintSn, Convert.ToInt32(Com_Part.EditValue.ToString()));
                if (PrintResult != "OK")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20089", "NG", List_Login.Language, new string[] { BoxSN, PrintResult });
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10015", "OK", List_Login.Language, new string[] { BoxSN });

                    DataSet dsPackage = PartSelectSVC.GetMesPackageStatusID(packageSN);
                    if (dsPackage != null && dsPackage.Tables.Count > 0 && dsPackage.Tables[0].Rows.Count > 0)
                    {
                        mesPackageHistory mesPackageHistory = new mesPackageHistory();
                        mesPackageHistory.PackageID = Convert.ToInt32(dsPackage.Tables[0].Rows[0]["ID"].ToString());
                        mesPackageHistory.PackageStatusID = 6;
                        mesPackageHistory.StationID = List_Login.StationID;
                        mesPackageHistory.EmployeeID = List_Login.EmployeeID;
                        PartSelectSVC.InsertMesPackageHistory(mesPackageHistory);
                    }
                }

                Edt_SN.Text = string.Empty;
                Edt_ChildSN.Text = string.Empty;
                Edt_SN.Enabled = false;
                Edt_ChildSN.Enabled = true;
                btnLastBox.Enabled = false;
                Edt_ChildSN.Focus();
                btnReprint.Enabled = false;

                //参数初始化
                isPacking = true;
                txtNowNumber.Text = string.Empty;
                BoxSN = string.Empty;
                DT_PackageData.Clear();
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                return;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            PackageRemove_Form shipMentRePrint = new PackageRemove_Form(List_Login, "1");
            if (shipMentRePrint.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(Edt_SN.Text.Trim()))
                {
                    LoadDataFromBoxSN();
                }
            }
        }

        private void BoxPackage_Form_Load(object sender, EventArgs e)
        {
            public_.OpenBartender(List_Login.StationID.ToString());

            #region 20220727  add by howard
            string result = string.Empty;
            string returnStr = PartSelectSVC.GetMesStationAndStationTypeDetail(List_Login.StationID, "IsLastBox", "IsLastBox", out result);

            if (result == "1" && returnStr == "1")
            {
                btnLastBox.Visible = true;
            }
            #endregion

        }
    }
}