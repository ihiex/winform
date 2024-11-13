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
using System.IO.Ports;

namespace App.MyMES
{
    public partial class BoxPackage_Auto_Verify_Form : FrmBase
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

        bool IsScanUPC = false;
        bool IsScanJAN = false;
        bool IsScanFGSN = false;

        private string strPort = "COM1";

        string IsAuto = "0";
        string PassCmd = "PASS";
        string FailCmd = "FAIL";
        int Delay = 500;

        public BoxPackage_Auto_Verify_Form()
        {
            InitializeComponent();
        }

        public override void FrmBase_Load(object sender, EventArgs e)
        {
            base.FrmBase_Load(sender, e);
            tablePanel1.Rows[5].Visible = false;
            tablePanel1.Rows[6].Visible = false;
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
            IsScanUPC = false;
            IsScanJAN = false;
            IsScanFGSN = false;
            IsScanOnlyFGSN = false;

            if (port1 != null)
            {
                if (port1.IsOpen) port1.Close();
                port1.Dispose();
            }

        }

        private void VerifyUPCJAN()
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
                
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                return;
            }
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
                DataSet dsStationDetail = PartSelectSVC.GetPLCSeting("", List_Login.StationID.ToString());

                DataRow[] drAutoStation = dsStationDetail.Tables[0].Select("Name='AutoStation'");
                if (drAutoStation.Length > 0) IsAuto = drAutoStation[0]["Value"].ToString(); else IsAuto = "0";

                DataRow[] drPASS = dsStationDetail.Tables[0].Select("Name='PASS'");
                if (drPASS.Length > 0) PassCmd = drPASS[0]["Value"].ToString(); else PassCmd = "PASS";

                DataRow[] drFAIL = dsStationDetail.Tables[0].Select("Name='FAIL'");
                if (drFAIL.Length > 0) FailCmd = drFAIL[0]["Value"].ToString(); else FailCmd = "FAIL";

                DataRow[] drPort = dsStationDetail.Tables[0].Select("Name='Port'");
                if (drPort.Length > 0) strPort = drPort[0]["Value"].ToString(); else strPort = "COM1";

                DataRow[] drDelay = dsStationDetail.Tables[0].Select("Name='Delay'");
                if (drDelay.Length > 0) Delay = Convert.ToInt32(drDelay[0]["Value"].ToString()); else Delay = 500;

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

                DataRow[] DR_IsScanFGSN = dtPara.Select("Description='IsScanFGSN'");
                if (DR_IsScanFGSN.Count() > 0)
                {
                    IsScanFGSN= DR_IsScanFGSN[0]["Content"].ToString() == "1";
                }
                if (IsScanFGSN)
                {
                    Lab_CheckFG.Visible = true;
                    Edt_CheckFGSN.Visible = true;
                    tablePanel1.Rows[9].Visible = true;
                }
                else
                {
                    Lab_CheckFG.Visible = false;
                    Edt_CheckFGSN.Visible = false;
                    tablePanel1.Rows[9].Visible = false;
                }

                DataRow[] IsScandrUpc = dtPara.Select("Description='IsScanUPC'");
                if (IsScandrUpc.Count() > 0)
                {
                    IsScanUPC = IsScandrUpc[0]["Content"].ToString() == "1";
                }

                if (IsScanUPC)
                {
                    lblUPCCode.Text = drUpc[0]["Content"].ToString();
                    lblUPCCode.Visible = true;
                    txtUPCCode.Visible = true;
                    lblUPCtxt.Visible = true;
                    tablePanel1.Rows[5].Visible = true;
                }
                else
                {
                    lblUPCCode.Visible = false;
                    txtUPCCode.Visible = false;
                    lblUPCtxt.Visible = false;
                    tablePanel1.Rows[5].Visible = false;
                }

                DataRow[] drJan = dtPara.Select("Description='IsScanJAN'");
                if (drJan.Count() > 0)
                {
                    IsScanJAN = drJan[0]["Content"].ToString() == "1";
                }

                if (IsScanJAN)
                {
                    DataRow[] drJanCode = dtPara.Select("Description='Jan'");
                    if (drJanCode.Count() == 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20198", "NG", List_Login.Language, new string[] { "Jan" });
                        return;
                    }
                    lblJANCode.Text = drJanCode[0]["Content"].ToString();
                    lblJANtxt.Visible = true;
                    txtJANCode.Visible = true;
                    lblJANCode.Visible = true;
                    tablePanel1.Rows[6].Visible = true;
                }
                else
                {
                    lblJANtxt.Visible = false;
                    txtJANCode.Visible = false;
                    lblJANCode.Visible = false;
                    tablePanel1.Rows[6].Visible = false;
                }
                txtUPCCode.Text = string.Empty;
                txtUPCCode.Enabled = false;
                txtJANCode.Text = string.Empty;
                txtJANCode.Enabled = false;

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

                    Edt_CheckFGSN.Text= string.Empty;
                    Edt_CheckFGSN.Enabled = false;  

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
                    IsGenerateBoxSN = true;

                    ResetVerify();
                }

                if (IsAuto == "1")
                {
                    port1 = new SerialPort(strPort);
                    port1.BaudRate = 9600;
                    port1.DataBits = 8;
                    port1.Parity = Parity.Odd;
                    port1.StopBits = StopBits.One;

                    try
                    {
                        if (!port1.IsOpen)
                        {
                            port1.Open();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "60104", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                        return;
                    }

                    try
                    {
                        string value1 = "B";
                        if (IsScanJAN && IsScanUPC)
                        {
                            value1 = "D";
                        }
                        else if (!IsScanJAN && !IsScanUPC)
                        {
                            value1 = "B";
                        }
                        else
                        {
                            value1 = "C";
                        }
                        port1.WriteLine(value1);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "60103", "OK", List_Login.Language, new string[] { value1 });
                    }
                    catch (Exception ex)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "60107", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                        return;
                    }
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
                    Edt_SN.Text = "";
                    Edt_SN.SelectAll();

                    if (IsAuto == "1")
                    {
                        try
                        {
                            port1.WriteLine(FailCmd);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
                        }
                        catch (Exception ex)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                            return;
                        }
                    }

                    return;
                }

                string result = string.Empty;
                DataSet dsBoxSN = PartSelectSVC.uspCallProcedure("uspKitBoxCheck", Edt_SN.Text.Trim(), xmlProdOrder, xmlPart,
                                                            xmlStation, xmlExtraData, "BOX", ref result);

                if (result != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Edt_SN.Text.ToString(), ProMsg }, result);
                    Edt_SN.Text = "";
                    Edt_SN.SelectAll();

                    if (IsAuto == "1")
                    {
                        try
                        {
                            port1.WriteLine(FailCmd);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
                        }
                        catch (Exception ex)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                            return;
                        }
                    }
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
                        btnReprint.Enabled = false;
                        if (DT_PackageData.Rows.Count > 0)
                        {
                            btnLastBox.Enabled = true;
                        }
                    }
                    gdControlData.DataSource = DT_PackageData;

                    ResetVerify();

                    if (IsAuto == "1")
                    {
                        try
                        {
                            port1.WriteLine("READY");
                            Thread.Sleep(Delay);
                            port1.WriteLine(PassCmd);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60101", "OK", List_Login.Language);
                        }
                        catch (Exception ex)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60105", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                return;
            }
        }

        private void ResetVerify()
        {
            txtUPCCode.Text = string.Empty;
            Edt_ChildSN.Text = string.Empty;
            txtJANCode.Text = string.Empty;

            if (IsScanUPC)
            {
                txtUPCCode.Enabled = true;
                Edt_ChildSN.Enabled = false;
                txtJANCode.Enabled = false;
                txtUPCCode.Focus();
            }
            else if (IsScanJAN)
            {
                txtJANCode.Enabled = true;
                Edt_ChildSN.Enabled = false;
                txtUPCCode.Enabled = false;
                txtJANCode.Focus();
            }
            else
            {
                Edt_ChildSN.Enabled = true;
                txtJANCode.Enabled = false;
                txtUPCCode.Enabled = false;
                Edt_ChildSN.Focus();
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
                string S_ChildSN = Edt_ChildSN.Text.Trim();

                if (IsScanFGSN)
                {
                    if (ChildSN_Check_KeyDown(sender, e) == "1")
                    {
                        Edt_ChildSN.Enabled = false;

                        Edt_CheckFGSN.Enabled = true;
                        Edt_CheckFGSN.Focus();
                        Edt_CheckFGSN.SelectAll();

                        try
                        {
                            port1.WriteLine(PassCmd);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60101", "OK", List_Login.Language);
                        }
                        catch (Exception ex)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60105", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                            return;
                        }
                    }
                    else
                    {
                        try
                        {
                            port1.WriteLine(FailCmd);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
                        }
                        catch (Exception ex)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                            return;
                        }
                    }
                }
                else
                {
                    ChildSN_KeyDown(sender, e);                   
                }
            }
        }

        private void Edt_CheckFGSN_KeyDown(object sender, KeyEventArgs e)
        {
            string S_UPCSN = Edt_ChildSN.Text.Trim();
            string S_CheckFGSN = Edt_CheckFGSN.Text.Trim();
                        
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(S_UPCSN) && !string.IsNullOrEmpty(S_CheckFGSN))
            {
                string S_Result = string.Empty;
                DataSet ds = PartSelectSVC.uspCallProcedure("uspKitBoxFGSNCheck", S_UPCSN, xmlProdOrder, xmlPart,
                                                                xmlStation, xmlExtraData, S_CheckFGSN, ref S_Result);
                if (S_Result == "1")
                {                    
                    Edt_CheckFGSN.Text = "";
                    Edt_CheckFGSN.Enabled = false;
                    txtUPCCode.Enabled = true;
                    txtUPCCode.Focus();

                    ChildSN_KeyDown(sender, e);
                }
                else
                {
                    string ProMsg = MessageInfo.GetMsgByCode(S_Result, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, 
                        new string[] { S_UPCSN+"  "+S_CheckFGSN, ProMsg }, S_Result);

                    Edt_ChildSN.Text = "";
                    Edt_ChildSN.Enabled = true;
                    Edt_ChildSN.Focus();

                    Edt_CheckFGSN.Text = "";
                    //Edt_CheckFGSN.Focus();

                    if (IsAuto == "1")
                    {
                        try
                        {
                            port1.WriteLine(FailCmd);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
                        }
                        catch (Exception ex)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() });                            
                            return;
                        }
                    }
                }
            }
        }
        private string ChildSN_Check_KeyDown(object sender, KeyEventArgs e)
        {
            string S_Result = "0";
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(Edt_ChildSN.Text.Trim()))
            {
                S_Result = "1";
                int NowNumber = string.IsNullOrEmpty(txtNowNumber.Text.Trim()) ? 0 : Convert.ToInt32(txtNowNumber.Text.Trim());
                if (Allnumber <= NowNumber)
                {
                    try
                    {
                        port1.WriteLine(FailCmd);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
                    }
                    catch (Exception ex)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "60105", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                        /*return*/;
                    }

                    MessageInfo.Add_Info_MSG(Edt_MSG, "20086", "NG", List_Login.Language);
                    return S_Result="0";
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
                        return S_Result = "0";
                    }
                }
                else if (result != "1")
                {
                    try
                    {
                        port1.WriteLine(FailCmd);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
                    }
                    catch (Exception ex)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "60105", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                        //return;
                    }

                    string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Edt_ChildSN.Text.ToString(), ProMsg }, result);
                    Edt_ChildSN.Text = "";
                    return S_Result = "0";
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
                        try
                        {
                            port1.WriteLine(FailCmd);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
                        }
                        catch (Exception ex)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60105", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                            /*return*/;
                        }

                        string ProMsg = MessageInfo.GetMsgByCode(S_RouteCheck, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_RouteCheck);
                        Edt_ChildSN.SelectAll();
                        return S_Result = "0";
                    }
                }
            }
            return S_Result;
        }

        private void  ChildSN_KeyDown(object sender, KeyEventArgs e)
        {
            string result = "";
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(Edt_ChildSN.Text.Trim()))
            {
                if (ChildSN_Check_KeyDown(sender, e) == "1")
                {
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
                            try
                            {
                                port1.WriteLine(FailCmd);
                                MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
                            }
                            catch (Exception ex)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "60105", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                /*return*/
                                ;
                            }

                            string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, result);
                            Edt_ChildSN.SelectAll();
                            return;
                        }

                        if (!Regex.IsMatch(BoxSN, BoxSN_Pattern))
                        {
                            try
                            {
                                port1.WriteLine(FailCmd);
                                MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
                            }
                            catch (Exception ex)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "60105", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                /*return*/
                                ;
                            }

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

                        //Edt_MSG.Text = string.Empty;

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
                                txtUPCCode.Text = string.Empty;
                                txtJANCode.Text = string.Empty;

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
                            ResetVerify();
                        }

                        try
                        {
                            port1.WriteLine(PassCmd);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60101", "OK", List_Login.Language);
                        }
                        catch (Exception ex)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60105", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                            return;
                        }
                    }
                    else
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, result);
                        Edt_ChildSN.SelectAll();

                        try
                        {
                            port1.WriteLine(FailCmd);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
                        }
                        catch (Exception ex)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                            return;
                        }

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

        private void txtUPCCode_KeyDown(object sender, KeyEventArgs e)
        {
            Boolean  B_AutoStatus = true;
            if (e.KeyCode == Keys.Enter)
            {
                string EnterCode = txtUPCCode.Text.Trim();
                if (EnterCode == lblUPCCode.Text.ToString())
                {
                    txtUPCCode.Enabled = false;
                    if (IsScanJAN && string.IsNullOrEmpty(txtJANCode.Text.Trim()))
                    {
                        txtJANCode.Enabled = true;
                        txtJANCode.Text = string.Empty;
                        txtJANCode.Focus();
                    }
                    else
                    {
                        Edt_ChildSN.Enabled = true;
                        Edt_ChildSN.Text = string.Empty;
                        Edt_ChildSN.Focus();
                    }
                    B_AutoStatus = true;
                }
                else if (string.IsNullOrEmpty(txtJANCode.Text.Trim()) && IsScanJAN && EnterCode == lblJANCode.Text.ToString())
                {
                    txtJANCode.Text = EnterCode;
                    txtJANCode.Enabled = false;
                    txtUPCCode.Text = string.Empty;
                    txtUPCCode.Focus();
                    B_AutoStatus = false;
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20050", "NG", List_Login.Language);
                    txtUPCCode.Focus();
                    txtUPCCode.Text = "";
                    B_AutoStatus = false;
                }


                if (IsAuto == "1")
                {
                    if (B_AutoStatus)
                    {
                        try
                        {
                            port1.WriteLine(PassCmd);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60101", "OK", List_Login.Language);
                        }
                        catch (Exception ex)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60105", "NG", List_Login.Language, new string[] { ex.Message.ToString() });                           
                            return;
                        }
                    }
                    else
                    {
                        try
                        {
                            port1.WriteLine(FailCmd);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
                        }
                        catch (Exception ex)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                            Edt_SN.SelectAll();
                            return;
                        }
                    }
                }

            }
        }

        private void txtJANCode_KeyDown(object sender, KeyEventArgs e)
        {
            Boolean B_AutoStatus = true;
            if (e.KeyCode == Keys.Enter)
            {
                string EnterCode = txtJANCode.Text.Trim();
                if (EnterCode == lblJANCode.Text.ToString())
                {
                    txtJANCode.Enabled = false;
                    if (IsScanUPC && string.IsNullOrEmpty(txtUPCCode.Text.Trim()))
                    {
                        txtUPCCode.Enabled = true;
                        txtUPCCode.Text = string.Empty;
                        txtUPCCode.Focus();
                        B_AutoStatus = false;
                    }
                    else
                    {
                        Edt_ChildSN.Enabled = true;
                        Edt_ChildSN.Text = string.Empty;
                        Edt_ChildSN.Focus();
                        B_AutoStatus = true;
                    }                    
                }
                else if (string.IsNullOrEmpty(txtUPCCode.Text.Trim()) && IsScanUPC && EnterCode == txtUPCCode.Text.ToString())
                {
                    txtUPCCode.Text = EnterCode;
                    txtUPCCode.Enabled = false;
                    txtJANCode.Text = string.Empty;
                    txtJANCode.Focus();
                    B_AutoStatus = false;
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20050", "NG", List_Login.Language);
                    txtJANCode.Focus();
                    txtJANCode.Text = "";
                    B_AutoStatus = false;
                }

                if (IsAuto == "1")
                {
                    if (B_AutoStatus)
                    {
                        try
                        {
                            port1.WriteLine(PassCmd);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60101", "OK", List_Login.Language);
                        }
                        catch (Exception ex)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60105", "NG", List_Login.Language, new string[] { ex.Message.ToString() });                            
                            return;
                        }
                    }
                    else
                    {
                        try
                        {
                            port1.WriteLine(FailCmd);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
                        }
                        catch (Exception ex)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() });                            
                            return;
                        }
                    }
                }

            }
        }
    }
}