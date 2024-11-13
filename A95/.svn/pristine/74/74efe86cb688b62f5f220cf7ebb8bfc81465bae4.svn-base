using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;

using App.Model;
using App.MyMES.mesEmployeeService;
using App.MyMES.PartSelectService;
using System.Diagnostics;
using DevExpress.DXperience.Demos;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraBars.Ribbon;
using System.Net;
using System.Reflection;
using App.MyMES.Frm_Other;

namespace App.MyMES
{
    public partial class MES_Dev_Form : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public MES_Dev_Form()
        {
            InitializeComponent();
        }

        string S_SystemName = "";
        LoginList F_LoginList;
        MyINI myINI = null;
        string FrmName = "lblFrmName";
        string User = string.Empty;
        Form CurrForm = null;
        FtpWeb FTP;
        string S_ErrorID = "";

        Public_ public_;

        public void Show_MES_Dev_Form(MES_Dev_Form v_MES_Dev_Form, LoginList LoginList)
        {
            F_LoginList = LoginList;
            v_MES_Dev_Form.Show();
            string S_Path = Application.StartupPath;
            myINI = new MyINI(S_Path + "\\MES_Main.ini");
        }

        private void MES_Dev_Form_Resize(object sender, EventArgs e)
        {
            foreach (Form frm in Panel_Main.Controls)
            {
                frm.Width = Panel_Main.Width;
                frm.Height = Panel_Main.Height;
            }
        }

        private void MES_Dev_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process[] p = Process.GetProcesses();
            foreach (Process p1 in p)
            {
                try
                {
                    string processName = p1.ProcessName.Trim();
                    //判断是否包含阻碍更新的进程
                    if (processName == "BartenderPrint" || processName == "BartenderPrint_X86")
                    {
                        p1.Kill();
                    }
                }
                catch { }
            }
            Application.Exit();
        }


        /// <summary>
        /// 控件语言切换
        /// </summary>
        /// <param name="objControl"></param>
        /// <param name="dt"></param>
        /// <param name="language"></param>
        private void LanguageForLableControl(object objControl, DataTable dt, string language)
        {
            string controlName = string.Empty;
            string captionName = string.Empty;
            DataRow[] drList = null;

            switch (objControl.GetType().Name)
            {
                case "LabelControl":
                    DevExpress.XtraEditors.LabelControl lblControl = objControl as DevExpress.XtraEditors.LabelControl;
                    if (lblControl.Name == "lblShowFrmName")
                    {
                        lblControl.Text = S_SystemName + FrmName;
                    }
                    else
                    {
                        lblControl.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        lblControl.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
                        controlName = lblControl.Name;
                        drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                        if (drList.Length > 0)
                        {
                            captionName = drList[0]["Description"].ToString();
                            (objControl as DevExpress.XtraEditors.LabelControl).Text = captionName;
                        }
                    }
                    break;
                case "Label":
                    controlName = (objControl as Label).Name;
                    drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                    if (drList.Length > 0)
                    {
                        captionName = drList[0]["Description"].ToString();
                        (objControl as Label).Text = captionName;
                    }
                    break;
                case "TextEdit":
                    controlName = (objControl as DevExpress.XtraEditors.TextEdit).Name;
                    drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                    if (drList.Length > 0)
                    {
                        captionName = drList[0]["Description"].ToString();
                        (objControl as DevExpress.XtraEditors.TextEdit).Text = captionName;
                    }
                    break;
                case "TextBox":
                    controlName = (objControl as TextBox).Name;
                    drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                    if (drList.Length > 0)
                    {
                        captionName = drList[0]["Description"].ToString();
                        (objControl as TextBox).Text = captionName;
                    }
                    break;
                case "SimpleButton":
                    controlName = (objControl as DevExpress.XtraEditors.SimpleButton).Name;
                    drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                    if (drList.Length > 0)
                    {
                        captionName = drList[0]["Description"].ToString();
                        (objControl as DevExpress.XtraEditors.SimpleButton).Text = captionName;
                    }
                    break;
                case "Button":
                    controlName = (objControl as Button).Name;
                    drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                    if (drList.Length > 0)
                    {
                        captionName = drList[0]["Description"].ToString();
                        (objControl as Button).Text = captionName;
                    }
                    break;
                case "BarButtonItem":
                    controlName = (objControl as BarButtonItem).Name;
                    drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                    if (drList.Length > 0)
                    {
                        captionName = drList[0]["Description"].ToString();
                        (objControl as BarButtonItem).Caption = captionName;
                    }
                    break;
                case "RadioButton":
                    controlName = (objControl as RadioButton).Name;
                    drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                    if (drList.Length > 0)
                    {
                        captionName = drList[0]["Description"].ToString();
                        (objControl as RadioButton).Text = captionName;
                    }
                    break;
                case "SearchLookUpEdit":
                    controlName = (objControl as DevExpress.XtraEditors.SearchLookUpEdit).Name;
                    drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                    if (drList.Length > 0)
                    {
                        captionName = drList[0]["Description"].ToString();
                        (objControl as DevExpress.XtraEditors.SearchLookUpEdit).Text = captionName;
                    }
                    break;
                case "SplitContainerControl":
                    DevExpress.XtraEditors.SplitContainerControl spcControl = objControl as DevExpress.XtraEditors.SplitContainerControl;
                    foreach (Control ctrl in spcControl.Panel1.Controls)
                    {
                        LanguageForLableControl(ctrl, dt, language);
                    }

                    foreach (Control ctrl in spcControl.Panel2.Controls)
                    {
                        LanguageForLableControl(ctrl, dt, language);
                    }
                    break;
                case "PanelControl":
                    DevExpress.XtraEditors.PanelControl plControl = objControl as DevExpress.XtraEditors.PanelControl;
                    foreach (Control ctrl in plControl.Controls)
                    {
                        LanguageForLableControl(ctrl, dt, language);
                    }
                    break;
                case "Panel":
                    Panel pl = objControl as Panel;
                    foreach (Control ctrl in pl.Controls)
                    {
                        LanguageForLableControl(ctrl, dt, language);
                    }
                    break;
                case "GroupControl":
                    DevExpress.XtraEditors.GroupControl gpControl = objControl as DevExpress.XtraEditors.GroupControl;
                    controlName = gpControl.Name;
                    drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                    if (drList.Length > 0)
                    {
                        captionName = drList[0]["Description"].ToString();
                        gpControl.Text = captionName;
                    }

                    if (gpControl.Name.ToString() == "GrpControlInputData")
                    {
                        string subFromName = gpControl.FindForm().Name;
                        PartSelectSVCClient PartSelectPanle = PartSelectFactory.CreateServerClient();
                        DataSet dsFrm = PartSelectPanle.GetLanguage(subFromName, "FRM");
                        PartSelectPanle.Close();
                        if (dsFrm != null && dsFrm.Tables[0].Rows.Count > 0)
                        {
                            DataTable dtFrm = dsFrm.Tables[0];
                            foreach (object objPanles in gpControl.Controls)
                                LanguageForLableControl(objPanles, dtFrm, language);
                        }
                    }
                    else
                    {
                        foreach (Control ctrl in gpControl.Controls)
                        {
                            LanguageForLableControl(ctrl, dt, language);
                        }
                    }
                    break;
                case "BarStaticItem":
                    controlName = (objControl as DevExpress.XtraBars.BarStaticItem).Name;
                    drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                    if (drList.Length > 0)
                    {
                        captionName = drList[0]["Description"].ToString();
                        (objControl as BarStaticItem).Caption = captionName;
                    }
                    break;
                case "TablePanel":
                    DevExpress.Utils.Layout.TablePanel tablePanel = objControl as DevExpress.Utils.Layout.TablePanel;
                    foreach (Control ctrl in tablePanel.Controls)
                    {
                        LanguageForLableControl(ctrl, dt, language);
                    }
                    break;
                case "GridControl":
                    foreach (DevExpress.XtraGrid.Views.Grid.GridView view in (objControl as DevExpress.XtraGrid.GridControl).Views)
                    {
                        if (view != null && view.Columns.Count > 0)
                        {
                            foreach (DevExpress.XtraGrid.Columns.GridColumn column in view.Columns)
                            {
                                controlName = column.Name;
                                drList = dt.Select("Code='" + controlName + "' AND Language='" + language + "'");
                                if (drList.Length > 0)
                                {
                                    captionName = drList[0]["Description"].ToString();
                                    column.Caption = captionName;
                                }
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 语言转换
        /// </summary>
        public void ConvertLanguage(string Language)
        {
            PartSelectSVCClient PartSelect = PartSelectFactory.CreateServerClient();
            string SwLan = string.Empty;
            string ControlName = string.Empty;

            DataSet dsMian = PartSelect.GetLanguage(this.Name, "FRM");
            if (dsMian != null && dsMian.Tables[0].Rows.Count > 0)
            {
                DataTable dtMian = dsMian.Tables[0];
                foreach (Control ctrl in this.Controls)
                {
                    //主界面"导航菜单/状态条"语言替换
                    if (ctrl is DevExpress.XtraBars.Ribbon.RibbonControl)
                    {
                        foreach (object obj in (ctrl as RibbonControl).Items)
                        {
                            LanguageForLableControl(obj, dtMian, Language);
                        }
                    }
                    // FormBase界面语言替换
                    else if (ctrl is DevExpress.XtraEditors.PanelControl)
                    {
                        foreach (object objPanle in ctrl.Controls)
                        {
                            if (objPanle is Form)
                            {
                                DataSet dsFrmBase = null;
                                if (typeof(FrmBase).IsAssignableFrom(objPanle.GetType()) ||
                                    typeof(FrmBase2).IsAssignableFrom(objPanle.GetType()) ||
                                    typeof(FrmBase3).IsAssignableFrom(objPanle.GetType()))
                                {
                                    dsFrmBase = PartSelect.GetLanguage("FrmBase", "FRM");
                                }
                                else
                                {
                                    dsFrmBase = PartSelect.GetLanguage((objPanle as Form).Name, "FRM");
                                }
                                if (dsFrmBase != null && dsFrmBase.Tables[0].Rows.Count > 0)
                                {
                                    DataTable dtFrmBase = dsFrmBase.Tables[0];
                                    foreach (Control ctrBase in (objPanle as Form).Controls)
                                    {
                                        ReplaceFrmName(objPanle as Form, Language, PartSelect);
                                        LanguageForLableControl(ctrBase, dtFrmBase, Language);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            PartSelect.Close();
        }

        /// <summary>
        /// 显示窗体名称
        /// </summary>
        public void ReplaceFrmName(Form frm, string Language, PartSelectSVCClient PartSelectSVC)
        {
            DataSet dsFrmBase = PartSelectSVC.GetLanguage(frm.Name, "FRM");
            if (dsFrmBase != null && dsFrmBase.Tables[0].Rows.Count > 0)
            {
                DataRow[] drListFrm = dsFrmBase.Tables[0].Select("Code='lblShowlFrmName' AND FormName='"
                    + frm.Name + "' AND Language='" + Language + "'");
                if (drListFrm.Length > 0)
                {
                    FrmName = drListFrm[0]["Description"].ToString();
                }
                else
                {
                    FrmName = this.Text;
                }
            }
        }

        public void ShowForm<T_Form>() where T_Form : Form, new()
        {
            try
            {
                foreach (Form frm in Panel_Main.Controls)
                {
                    if (frm is T_Form)
                    {
                        frm.Width = Panel_Main.Width;
                        frm.Height = Panel_Main.Height;
                        frm.BringToFront();
                        frm.Tag = F_LoginList;

                        CurrForm = frm;

                        return;
                    }
                }

                T_Form _Form = new T_Form();
                _Form.FormBorderStyle = FormBorderStyle.None;
                _Form.Width = Panel_Main.Width;
                _Form.Height = Panel_Main.Height;
                _Form.TopLevel = false;
                _Form.Parent = Panel_Main;
                _Form.BringToFront();
                _Form.Tag = F_LoginList;

                CurrForm = _Form;
                _Form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MES_Dev_Form_Load(object sender, EventArgs e)
        {
            /////////////////////////影藏不要的功能<有了就删除>///////////////////
            //Btn_IssueRequest.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            BarItem_TeamLbl.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            BarItem_Team.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            Btn_Upgrade.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            public_ = new Public_();
            ImesEmployeeSVCClient v_mesEmployeeSVCC = ImesEmployeeFactory.CreateServerClient();
            var v_mesEmployee = v_mesEmployeeSVCC.ListAll(" where ID='" + F_LoginList.EmployeeID.ToString() + "'").ToList();
            //Lab_Msg.Hint = "User:" + v_mesEmployee[0].Lastname + v_mesEmployee[0].Firstname;
            //Btn_Station.Hint = F_LoginList.Station;
            //Btn_StationType.Hint = F_LoginList.StationType;
            //Btn_Line.Hint = F_LoginList.Line;
            Lab_Msg.Caption = v_mesEmployee[0].Lastname + v_mesEmployee[0].Firstname;
            User = Lab_Msg.Caption;
            Btn_Station.Caption = F_LoginList.Station;
            Btn_StationType.Caption = F_LoginList.StationType;
            Btn_Line.Caption = F_LoginList.Line;

            BarItem_Ver.Caption = " " + F_LoginList.Ver + "  ";
            string hostName = Dns.GetHostName();
            BarItem_ComputerName.Caption = hostName;
            IPHostEntry localhost = Dns.GetHostByName(hostName);
            BarItem_ComputerIP.Caption = localhost.AddressList[0].ToString();
            BarItem_ServerIP.Caption = F_LoginList.ServerIP;
            BarItem_DBName.Caption = public_.GetDbName();
            BarItem_WebIP.Caption = public_.GetWebIP();

            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            DataSet DS = PartSelectSVC.GetApplicationType(F_LoginList.StationTypeID.ToString());
            string S_ApplicationType = DS.Tables[0].Rows[0]["Description"].ToString();
            this.BarItem_DateTime.Caption = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            //ShowForm<Interlock_Form>();

            switch (F_LoginList.ApplicationType)
            {
                case "PrintFG":
                    ShowForm<PrintSNOffline_Form>();
                    break;
                case "PrintUPC":
                    ShowForm<PrintSN_UPC_Form>();
                    break;
                case "QC":
                    ShowForm<QC_Form>();
                    break;
                case "QC_NoPo":
                    ShowForm<QCNoPo_Form>();
                    break;
                case "QCPart":
                    ShowForm<QCPart_Form>();
                    break;
                case "QCNew":
                    ShowForm<QCNew_Form>();
                    break;
                case "QC_NoPoNew":
                    ShowForm<QCNoPoNew_Form>();
                    break;
                case "QCPartNew":
                    ShowForm<QCPartNew_Form>();
                    break;
                case "ToolingQC":
                    ShowForm<ToolingOverStation_Form>();
                    break;
                case "ToolingQCNGPrint":
                    ShowForm<ToolingQCNGPrint>();
                    break;
                case "ToolingQCPrint":
                    ShowForm<ToolingQCPrint>();
                    break;
                case "Interlock":
                    ShowForm<Interlock_Form>();
                    break;
                case "SNLinkBatch":
                    ShowForm<SNLinkBatch_Form>();
                    break;
                case "UnitReplaceTool":
                    ShowForm<UnitReplaceTool_Form>();
                    break;
                case "LinkUPC":
                    ShowForm<SNlinkUPC_Form>();
                    break;
                case "CartonBox":
                    ShowForm<BoxPackage_Form>();
                    break;
                case "LinkUPC_Auto":
                    ShowForm<SNlinkUPC_Auto_Form>();
                    break;
                case "CartonBox_Auto":
                    ShowForm<BoxPackage_Auto_Form>();
                    break;
                case "Pallet":
                    ShowForm<PalletPackage_Form>();
                    break;
                case "Plasma":
                    ShowForm<Plasma_Form>();
                    break;
                case "Assembly":
                    ShowForm<Assembly_Form>();
                    break;
                case "AssemblyNew":
                    ShowForm<AssemblyNew_Form>();
                    break;
                case "AssemblyAuto":
                    ShowForm<AssemblyAuto_Form>();
                    break;
                case "AssemblyAutoNoPo":
                    ShowForm<AssemblyAutoNoPo_Form>();
                    break;
                case "AssemblyAutoPart":
                    ShowForm<AssemblyAutoPart_Form>();
                    break;
                case "AssemblyAutoNew":
                    ShowForm<AssemblyAutoNew_Form>();
                    break;
                case "AssemblyNoPo":
                    ShowForm<AssemblyNoPo_Form>();
                    break;
                case "AssemblyNoPoNew":
                    ShowForm<AssemblyNoPoNew_Form>();
                    break;
                case "AssemblyPart":
                    ShowForm<AssemblyPart_Form>();
                    break;
                case "AssemblyPartNew":
                    ShowForm<AssemblyPartNew_Form>();
                    break;
                case "AssemblyPanel":
                    ShowForm<AssemblyPanle_Form>();
                    break;
                case "AssemblyPanelNew":
                    ShowForm<AssemblyPanleNew_Form>();
                    break;
                case "AssemblyPanelNoPo":
                    ShowForm<AssemblyPanleNoPo_Form>();
                    break;
                case "AssemblyPanelNoPoNew":
                    ShowForm<AssemblyPanleNoPoNew_Form>();
                    break;
                case "AssemblyPanelPart":
                    ShowForm<AssemblyPanlePart_Form>();
                    break;
                case "AssemblyPanelPartNew":
                    ShowForm<AssemblyPanlePartNew_Form>();
                    break;
                case "ToolingAssembly":
                    ShowForm<ToolingAssembly_Form>();
                    break;
                case "ToolingLinkTooling":
                    ShowForm<ToolingLinkTooling_Form>();
                    break;
                case "ToolingAssemblyNew":
                    ShowForm<ToolingAssemblyNew_Form>();
                    break;
                case "MaterialInitialize":
                    ShowForm<MaterialInitial_Form>();
                    break;
                case "MaterialCropping-SN":
                    ShowForm<MaterialCropping_SN_Form>();
                    break;
                case "MaterialCropping-Batch":
                    ShowForm<MaterialCropping_Batch_Form>();
                    break;
                case "RollerAssembly":
                    ShowForm<L2RollerAssembly_Form>();
                    break;
                case "RollerAssemblyNew":
                    ShowForm<L2RollerAssemblyNew_Form>();
                    break;
                case "BoxLinkBatch":
                    ShowForm<CarrierLinkMaterialBatch_Form>();
                    break;
                case "OverStation":
                    ShowForm<OverStation_Form>();
                    break;
                case "OverStationNoPo":
                    ShowForm<OverStationNoPo_Form>();
                    break;
                case "OverStationPart":
                    ShowForm<OverStationPart_Form>();
                    break;
                case "OverStationNew":
                    ShowForm<OverStationNew_Form>();
                    break;
                case "OverStationNoPoNew":
                    ShowForm<OverStationNoPoNew_Form>();
                    break;
                case "OverStationPartNew":
                    ShowForm<OverStationPartNew_Form>();
                    break;
                case "UPCPrint":
                    ShowForm<PrintSN_UPC_Form>();
                    break;
                case "IQC":
                    ShowForm<IQC_Form>();
                    break;
                case "IQCNew":
                    ShowForm<IQCNew_Form>();
                    break;
                case "ModifyUnitState":
                    ShowForm<ModUnitState_Form>();
                    break;
                case "JumpStationQC":
                    ShowForm<JumpStationQC_Form>();
                    break;
                case "RepaiQC":
                    ShowForm<Repai_QC_Form>();
                    break;
                case "Rework":
                    ShowForm<ReworkV2_Form>();
                    break;
                case "ToolingPrint":
                    ShowForm<ScanToolingPrint>();
                    break;
                case "FGPrintUPC":
                    ShowForm<ScanFGSN_PrintUPC_Form>();
                    break;
                case "FGPrintUPCAuto":
                    ShowForm<ScanFGSN_PrintUPC_Auto_Form>();
                    break;
                case "Shipping":
                    ShowForm<ShipMent_Form>();
                    break;
                case "JumpStation":
                    ShowForm<JumpStation_Frm>();
                    break;
                case "RePrint":
                    ShowForm<RePrintUPC_Form>();
                    break;
                case "FG_OfflineLabelPrint_DOE":
                    ShowForm<PrintSNOfflineDOE_Form>();
                    break;
                case "FG_OfflineLabelPrint_NPI":
                    ShowForm<PrintSNOfflineNPI_Form>();
                    break;
                case "FG_Print_DOE_New":
                    ShowForm<PrintSNDOENew_Form>();
                    break;
                case "PackageOverStation":
                    ShowForm<PackageOverStation_Form>();
                    break;
                case "ORT":
                    ShowForm<ORT_Form>();
                    break;
                case "WH":
                    ShowForm<WH_Form>();
                    break;
                case "MaterialLineCropping-SN":
                    ShowForm<MaterialLineCropping_SN_Form>();
                    break;
                case "MaterialCropping-SN_DOE":
                    ShowForm<MaterialCroppingDOE_SN_Form>();
                    break;
                case "TT-OverStationV2":
                    ShowForm<TT_OverStation_V2_Form>();
                    break;
                case "TT-OverStation":
                    ShowForm<TT_OverStation_Form>();
                    break;
                case "TT_Registers":
                    ShowForm<TT_Registers_Form>();
                    break;
                case "TT_BindBox":
                    ShowForm<TT_BindBox_Form>();
                    break;
                case "OOBA":
                    ShowForm<OOBA_Form>();
                    break;
                case "RePrintFG":
                    ShowForm<ScanUPC_PrintFGSN_Form>();
                    break;
                case "UPCPrintFG":
                    ShowForm<ScanUPC_PrintFGSN_Form>();
                    break;

                case "KitQC":
                    ShowForm<KitQC_Form>();
                    break;
                case "CartonBox-Verify":
                    ShowForm<BoxPackage_Verify_Form>();
                    break;
                case "CartonBox-VerifyNew":
                    ShowForm<BoxPackage_VerifyNew_Form>();
                    break;
                case "CartonBox-Verify_Auto":
                    ShowForm<BoxPackage_Auto_Verify_Form>();
                    break;
                case "RePrintFGSN":
                    ShowForm<RePrintFGSN_Form>();
                    break;
                case "FGSNQueryShell":
                    ShowForm<FGSNQueryShell_Form>();
                    break;
                case "Disassembly":
                    ShowForm<Disassembly_Form>();
                    break;
                case "ScanCheck":
                    ShowForm<ScanCheck_Form>();
                    break;
                case "QueryInactive":
                    ShowForm<QueryInactiveCode_Form>();
                    break;
                case "PrintSNOfflinePrint_SOV":
                    ShowForm<PrintSNOfflineSOV_Form>();
                    break;
                case "WH_Siemens-In":
                    ShowForm<WH_In_Form>();
                    break;
                case "WH_Siemens-Out":
                    ShowForm<WH_ShipmentOut_Form>();
                    break;
                case "WH_Old-In":
                    ShowForm<WH_In_Old_Form>();
                    break;
                case "WH_Old-Out":
                    ShowForm<WH_ShipmentOut_Old_Form>();
                    break;
                case "WH_Old":
                    ShowForm<WH_Old_Form>();
                    break;
                case "ChangePart":
                    ShowForm<ChangePart_Form>();
                    break;

                case "AssemblyV2":
                    ShowForm<AssemblyV2_Form>();
                    break;
                case "AssemblyTwoInputV2":
                    ShowForm<AssemblyTwoInputV2_Form>();
                    break;
                case "L2RollerAssemblyV2":
                    ShowForm<L2RollerAssemblyV2_Form>();
                    break;
                case "ToolingAssemblyV2":
                    ShowForm<ToolingAssemblyV2_Form>();
                    break;
                case "ShipMentRemove":
                    ShowForm<ShipMentRemove_Form>();
                    break;
                case "IQCV2":
                    ShowForm<IQCV2_Form>();
                    break;
                //case "IQCV3":
                //    ShowForm<IQCV3_Form>();
                //    break;
                case "QCV2":
                    ShowForm<QCV2_Form>();
                    break;
                case "OverStationV2":
                    ShowForm<OverStationV2_Form>();
                    break;
                case "JumpStationQCV2":
                    ShowForm<JumpStationQCV2_Form>();
                    break;
                case "KitQCV2":
                    ShowForm<KitQCV2_Form>();
                    break;
                case "RepairKitQCV2":
                    ShowForm<RepairKitQCV2_Form>();
                    break;
                case "Repair_QCV2":
                    ShowForm<Repair_QCV2_Form>();
                    break;
                case "ToolingOverStationV2":
                    ShowForm<ToolingOverStationV2_Form>();
                    break;
                case "ShippingV2":
                    ShowForm<ShipMentV2_Form>();
                    break;
                case "BoxLinkBatchV2":
                    ShowForm<CarrierLinkMaterialBatchV2_Form>();
                    break;
                case "SNLinkBatchV2":
                    ShowForm<SNLinkBatchV2_Form>();
                    break;
                case "LinkUPCV2":
                    ShowForm<SNlinkUPCV2_Form>();
                    break;
                case "ToolingLinkToolingV2":
                    ShowForm<ToolingLinkToolingV2_Form>();
                    break;
                case "BoxPackage_Weight":
                    ShowForm<BoxPackage_Weight_Form>();
                    break;
                case "ReleaseMachineSN":
                    ShowForm<ReleaseMachineSN_Form>();
                    break;
                case "ShipMentScales":
                    ShowForm<ShipMentScales_Form>();
                    break;
                case "RMAChange":
                    ShowForm<RMAChange_Form>();
                    break;
                case "MaterialImport":
                    ShowForm<MaterialImport_Form>();
                    break;
                case "MaterialRePrint":
                    ShowForm<MaterialRePrint_Form>();
                    break;
            }
            //break;
            //}


            if (string.IsNullOrEmpty(F_LoginList.Language))
            {
                F_LoginList.Language = "EN";
            }
            ConvertLanguage(F_LoginList.Language);

            BarPic_Logo.EditValue = Properties.Resources.COSMO_Black;
            BarPic_Logo.Edit.BorderStyle = BorderStyles.NoBorder;

            try
            {
                string S_IP = localhost.AddressList[0].ToString();

                string S_Where = " where IP='" + S_IP + "' and IsFeedback=0";
                DataSet DS_ListmesScreenshot = PartSelectSVC.ListmesScreenshot(S_Where);


                if (DS_ListmesScreenshot.Tables.Count > 0)
                {
                    DataTable v_DT = DS_ListmesScreenshot.Tables[0];
                    if (v_DT.Rows.Count > 0)
                    {
                        S_ErrorID = v_DT.Rows[v_DT.Rows.Count - 1]["ID"].ToString();

                        Btn_IssueRequest.Caption = S_ErrorID;
                        Btn_IssueRequest.ItemAppearance.Disabled.ForeColor = Color.Red;
                        Btn_IssueRequest.Enabled = false;

                        timer_MSG.Enabled = true;
                    }
                }
            }
            catch { }

            v_mesEmployeeSVCC.Close();
            PartSelectSVC.Close();
        }

        private void Btn_LogOut_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Hide();

            foreach (Form frm in Panel_Main.Controls)
            {
                if (frm is Form)
                {
                    if (frm.Name.IndexOf("Auto") > 0)
                    {
                        frm.Close();
                        frm.Dispose();
                    }

                    if (frm.Name == "TT_BindBox_Form" || frm.Name == "TT_OverStation_Form" || frm.Name == "TT_Registers_Form")
                    {
                        frm.Close();
                        frm.Dispose();
                    }
                }
            }


            LoginForm loginFrom = new LoginForm();
            loginFrom.ShowDialog();
            this.Dispose();
        }

        private void Btn_Close_ItemClick(object sender, ItemClickEventArgs e)
        {
            //Btn_LogOut_ItemClick(sender, e);
            if (MessageBox.Show("Exit the current system?", "prompt",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }
            this.Close();
        }

        private void Btn_InforCenter_ItemClick(object sender, ItemClickEventArgs e)
        {
            Select_Form v_Select_Form = new Select_Form();
            v_Select_Form.Show();
        }

        private void Btn_Search_ItemClick(object sender, ItemClickEventArgs e)
        {
            Search_Form v_Search_Form = new Search_Form();
            v_Search_Form.Show_Search_Form(v_Search_Form, F_LoginList);
        }

        private void btnChina_ItemClick(object sender, ItemClickEventArgs e)
        {
            ConvertLanguage("CH");
            myINI.WriteValue("LanguageID", "Value", "CH");
            F_LoginList.Language = "CH";
        }

        private void btnEnglish_ItemClick(object sender, ItemClickEventArgs e)
        {
            ConvertLanguage("EN");
            myINI.WriteValue("LanguageID", "Value", "EN");
            F_LoginList.Language = "EN";
        }

        private void btnThailand_ItemClick(object sender, ItemClickEventArgs e)
        {
            ConvertLanguage("TH");
            myINI.WriteValue("LanguageID", "Value", "TH");
            F_LoginList.Language = "TH";
        }

        private void Btn_SetStation_ItemClick(object sender, ItemClickEventArgs e)
        {
            ImesEmployeeSVCClient mesEmployeeSVC = ImesEmployeeFactory.CreateServerClient();
            string S_Sql = " where ID='" + F_LoginList.EmployeeID + "'";
            IEnumerable<mesEmployee> v_mesEmployee = mesEmployeeSVC.ListAll(S_Sql);
            List<mesEmployee> List_mesEmployee = v_mesEmployee.ToList();
            mesEmployeeSVC.Close();

            if (List_mesEmployee.Count() > 0)
            {
                int I_PermissionId = List_mesEmployee[0].PermissionId;

                if (I_PermissionId == 1)
                {
                    SetStation_Form v_SetStation_Form = new SetStation_Form();
                    v_SetStation_Form.Show_SetStation_Form(this, v_SetStation_Form);
                }
                else
                {
                    string ProMsg = MessageInfo.GetMsgByCode("20142", F_LoginList.Language);
                    MessageBox.Show(ProMsg, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                string ProMsg = MessageInfo.GetMsgByCode("20142", F_LoginList.Language);
                MessageBox.Show(ProMsg, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //if (User!="Admin")
            //{
            //    string ProMsg = MessageInfo.GetMsgByCode("20142", F_LoginList.Language);
            //    MessageBox.Show(ProMsg, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            //SetStation_Form v_SetStation_Form = new SetStation_Form();
            //v_SetStation_Form.Show_SetStation_Form(this, v_SetStation_Form);
        }

        private void Btn_Skin_ItemClick(object sender, ItemClickEventArgs e)
        {
            SetSkin_Form v_SetLangSkin_Form = new SetSkin_Form();
            v_SetLangSkin_Form.Show_SetStation_Form(this, v_SetLangSkin_Form);
        }

        private void Btn_Upgrade_ItemClick(object sender, ItemClickEventArgs e)
        {
            //Process process = new Process();
            //try
            //{
            //    string S_SysPath = Application.StartupPath;

            //    process.StartInfo.UseShellExecute = false;
            //    process.StartInfo.FileName = S_SysPath + "\\MESUpdate.exe";
            //    process.StartInfo.CreateNoWindow = true;
            //    process.Start();

            //    this.Close();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.BarItem_DateTime.Caption = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }



        class ListmesScreenshot
        {
            public string MSG { get; set; }
            public int PartID { get; set; }
            public int ProductionOrderID { get; set; }
            public Boolean luUnitStatus { get; set; }
        }

        private ListmesScreenshot GetMSG()
        {
            ListmesScreenshot v_ListmesScreenshot = new ListmesScreenshot();

            string S_MSG = "";
            int PartID = 0;
            int ProductionOrderID = 0;
            Boolean luUnitStatus = false;

            v_ListmesScreenshot.MSG = S_MSG;
            v_ListmesScreenshot.PartID = PartID;
            v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
            v_ListmesScreenshot.luUnitStatus = luUnitStatus;

            {

                if (CurrForm is AssemblyTwoInputV2_Form)
                {
                    S_MSG = (CurrForm as AssemblyTwoInputV2_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as AssemblyTwoInputV2_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as AssemblyTwoInputV2_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as AssemblyTwoInputV2_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is AssemblyV2_Form)
                {
                    S_MSG = (CurrForm as AssemblyV2_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as AssemblyV2_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as AssemblyV2_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as AssemblyV2_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is L2RollerAssemblyV2_Form)
                {
                    S_MSG = (CurrForm as L2RollerAssemblyV2_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as L2RollerAssemblyV2_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as L2RollerAssemblyV2_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as L2RollerAssemblyV2_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is ToolingAssemblyV2_Form)
                {
                    S_MSG = (CurrForm as ToolingAssemblyV2_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as ToolingAssemblyV2_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as ToolingAssemblyV2_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as ToolingAssemblyV2_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is AssemblyAutoNew_Form)
                {
                    S_MSG = (CurrForm as AssemblyAutoNew_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as AssemblyAutoNew_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as AssemblyAutoNew_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as AssemblyAutoNew_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is AssemblyAutoNoPo_Form)
                {
                    S_MSG = (CurrForm as AssemblyAutoNoPo_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as AssemblyAutoNoPo_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as AssemblyAutoNoPo_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as AssemblyAutoNoPo_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is AssemblyAutoPart_Form)
                {
                    S_MSG = (CurrForm as AssemblyAutoPart_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as AssemblyAutoPart_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as AssemblyAutoPart_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as AssemblyAutoPart_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is AssemblyNoPoNew_Form)
                {
                    S_MSG = (CurrForm as AssemblyNoPoNew_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as AssemblyNoPoNew_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as AssemblyNoPoNew_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as AssemblyNoPoNew_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is AssemblyNoPo_Form)
                {
                    S_MSG = (CurrForm as AssemblyNoPo_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as AssemblyNoPo_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as AssemblyNoPo_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as AssemblyNoPo_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is AssemblyPanleNew_Form)
                {
                    S_MSG = (CurrForm as AssemblyPanleNew_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as AssemblyPanleNew_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as AssemblyPanleNew_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as AssemblyPanleNew_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is AssemblyPanleNoPoNew_Form)
                {
                    S_MSG = (CurrForm as AssemblyPanleNoPoNew_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as AssemblyPanleNoPoNew_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as AssemblyPanleNoPoNew_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as AssemblyPanleNoPoNew_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is AssemblyPanleNoPo_Form)
                {
                    S_MSG = (CurrForm as AssemblyPanleNoPo_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as AssemblyPanleNoPo_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as AssemblyPanleNoPo_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as AssemblyPanleNoPo_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is AssemblyPanlePartNew_Form)
                {
                    S_MSG = (CurrForm as AssemblyPanlePartNew_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as AssemblyPanlePartNew_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as AssemblyPanlePartNew_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as AssemblyPanlePartNew_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is AssemblyPanlePart_Form)
                {
                    S_MSG = (CurrForm as AssemblyPanlePart_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as AssemblyPanlePart_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as AssemblyPanlePart_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as AssemblyPanlePart_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is AssemblyPanle_Form)
                {
                    S_MSG = (CurrForm as AssemblyPanle_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as AssemblyPanle_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as AssemblyPanle_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as AssemblyPanle_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is AssemblyPartNew_Form)
                {
                    S_MSG = (CurrForm as AssemblyPartNew_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as AssemblyPartNew_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as AssemblyPartNew_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as AssemblyPartNew_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is AssemblyPart_Form)
                {
                    S_MSG = (CurrForm as AssemblyPart_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as AssemblyPart_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as AssemblyPart_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as AssemblyPart_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is AssemblyAuto_Form)
                {
                    S_MSG = (CurrForm as AssemblyAuto_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as AssemblyAuto_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as AssemblyAuto_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as AssemblyAuto_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is AssemblyNew_Form)
                {
                    S_MSG = (CurrForm as AssemblyNew_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as AssemblyNew_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as AssemblyNew_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as AssemblyNew_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is Assembly_Form)
                {
                    S_MSG = (CurrForm as Assembly_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as Assembly_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as Assembly_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as Assembly_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is L2RollerAssemblyNew_Form)
                {
                    S_MSG = (CurrForm as L2RollerAssemblyNew_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as L2RollerAssemblyNew_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as L2RollerAssemblyNew_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as L2RollerAssemblyNew_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is L2RollerAssembly_Form)
                {
                    S_MSG = (CurrForm as L2RollerAssembly_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as L2RollerAssembly_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as L2RollerAssembly_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as L2RollerAssembly_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is ToolingAssemblyNew_Form)
                {
                    S_MSG = (CurrForm as ToolingAssemblyNew_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as ToolingAssemblyNew_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as ToolingAssemblyNew_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as ToolingAssemblyNew_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is ToolingAssembly_Form)
                {
                    S_MSG = (CurrForm as ToolingAssembly_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as ToolingAssembly_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as ToolingAssembly_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as ToolingAssembly_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is CarrierLinkMaterialBatchV2_Form)
                {
                    S_MSG = (CurrForm as CarrierLinkMaterialBatchV2_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as CarrierLinkMaterialBatchV2_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as CarrierLinkMaterialBatchV2_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as CarrierLinkMaterialBatchV2_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is SNLinkBatchV2_Form)
                {
                    S_MSG = (CurrForm as SNLinkBatchV2_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as SNLinkBatchV2_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as SNLinkBatchV2_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as SNLinkBatchV2_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is SNlinkUPCV2_Form)
                {
                    S_MSG = (CurrForm as SNlinkUPCV2_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as SNlinkUPCV2_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as SNlinkUPCV2_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as SNlinkUPCV2_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is ToolingLinkToolingV2_Form)
                {
                    S_MSG = (CurrForm as ToolingLinkToolingV2_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as ToolingLinkToolingV2_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as ToolingLinkToolingV2_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as ToolingLinkToolingV2_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is CarrierLinkMaterialBatch_Form)
                {
                    S_MSG = (CurrForm as CarrierLinkMaterialBatch_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as CarrierLinkMaterialBatch_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as CarrierLinkMaterialBatch_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as CarrierLinkMaterialBatch_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is SNLinkBatch_Form)
                {
                    S_MSG = (CurrForm as SNLinkBatch_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as SNLinkBatch_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as SNLinkBatch_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as SNLinkBatch_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is SNlinkUPC_Auto_Form)
                {
                    S_MSG = (CurrForm as SNlinkUPC_Auto_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as SNlinkUPC_Auto_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as SNlinkUPC_Auto_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as SNlinkUPC_Auto_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is SNlinkUPC_Form)
                {
                    S_MSG = (CurrForm as SNlinkUPC_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as SNlinkUPC_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as SNlinkUPC_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as SNlinkUPC_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is ToolingLinkTooling_Form)
                {
                    S_MSG = (CurrForm as ToolingLinkTooling_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as ToolingLinkTooling_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as ToolingLinkTooling_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as ToolingLinkTooling_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is MaterialCroppingDOE_SN_Form)
                {
                    S_MSG = (CurrForm as MaterialCroppingDOE_SN_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as MaterialCroppingDOE_SN_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as MaterialCroppingDOE_SN_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as MaterialCroppingDOE_SN_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is MaterialCropping_Batch_Form)
                {
                    S_MSG = (CurrForm as MaterialCropping_Batch_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as MaterialCropping_Batch_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as MaterialCropping_Batch_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as MaterialCropping_Batch_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is MaterialCropping_SN_Form)
                {
                    S_MSG = (CurrForm as MaterialCropping_SN_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as MaterialCropping_SN_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as MaterialCropping_SN_Form).Com_PO.EditValue);
                        // luUnitStatus = (CurrForm as MaterialCropping_SN_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is MaterialInitial_Form)
                {
                    S_MSG = (CurrForm as MaterialInitial_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as MaterialInitial_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as MaterialInitial_Form).Com_PO.EditValue);
                        // luUnitStatus = (CurrForm as MaterialInitial_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is MaterialLineCropping_SN_Form)
                {
                    S_MSG = (CurrForm as MaterialLineCropping_SN_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as MaterialLineCropping_SN_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as MaterialLineCropping_SN_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as MaterialLineCropping_SN_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is ORTReplaceCode_Form)
                {
                    //S_MSG = (CurrForm as ORTReplaceCode_Form).Edt_MSG.Text;
                    //try
                    //{
                    //    PartID = Convert.ToInt32((CurrForm as ORTReplaceCode_Form).Com_Part.EditValue);
                    //    ProductionOrderID = Convert.ToInt32((CurrForm as ORTReplaceCode_Form).Com_PO.EditValue);
                    //    luUnitStatus = (CurrForm as ORTReplaceCode_Form).Com_luUnitStatus.Enabled;
                    //}
                    //catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is ChangePart_Form)
                {
                    S_MSG = (CurrForm as ChangePart_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as ChangePart_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as ChangePart_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as ChangePart_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is Disassembly_Form)
                {
                    S_MSG = (CurrForm as Disassembly_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as Disassembly_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as Disassembly_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as Disassembly_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is ReworkV2_Form)
                {
                    S_MSG = (CurrForm as ReworkV2_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as ReworkV2_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as ReworkV2_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as ReworkV2_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is ScanCheck_Form)
                {
                    S_MSG = (CurrForm as ScanCheck_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as ScanCheck_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as ScanCheck_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as ScanCheck_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is BoxPackage_Auto_Form)
                {
                    S_MSG = (CurrForm as BoxPackage_Auto_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as BoxPackage_Auto_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as BoxPackage_Auto_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as BoxPackage_Auto_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is BoxPackage_Auto_Verify_Form)
                {
                    S_MSG = (CurrForm as BoxPackage_Auto_Verify_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as BoxPackage_Auto_Verify_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as BoxPackage_Auto_Verify_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as BoxPackage_Auto_Verify_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is BoxPackage_VerifyMax_Form)
                {
                    S_MSG = (CurrForm as BoxPackage_VerifyMax_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as BoxPackage_VerifyMax_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as BoxPackage_VerifyMax_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as BoxPackage_VerifyMax_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is BoxPackage_VerifyNew_Form)
                {
                    S_MSG = (CurrForm as BoxPackage_VerifyNew_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as BoxPackage_VerifyNew_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as BoxPackage_VerifyNew_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as BoxPackage_VerifyNew_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is BoxPackage_VerifyNew_RePrint_Form)
                {
                    //S_MSG = (CurrForm as BoxPackage_VerifyNew_RePrint_Form).Edt_MSG.Text;
                    //try
                    //{
                    //    PartID = Convert.ToInt32((CurrForm as BoxPackage_VerifyNew_RePrint_Form).Com_Part.EditValue);
                    //    ProductionOrderID = Convert.ToInt32((CurrForm as BoxPackage_VerifyNew_RePrint_Form).Com_PO.EditValue);
                    //    luUnitStatus = (CurrForm as BoxPackage_VerifyNew_RePrint_Form).Com_luUnitStatus.Enabled;
                    //}
                    //catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is BoxPackage_Verify_Form)
                {
                    S_MSG = (CurrForm as BoxPackage_Verify_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as BoxPackage_Verify_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as BoxPackage_Verify_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as BoxPackage_Verify_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is BoxPackage_Weight_Form)
                {
                    S_MSG = (CurrForm as BoxPackage_Weight_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as BoxPackage_Weight_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as BoxPackage_Weight_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as BoxPackage_Weight_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is PrintSNDOENew_Form)
                {
                    S_MSG = (CurrForm as PrintSNDOENew_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as PrintSNDOENew_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as PrintSNDOENew_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as PrintSNDOENew_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is PrintSNOfflineNPI_Form)
                {
                    S_MSG = (CurrForm as PrintSNOfflineNPI_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as PrintSNOfflineNPI_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as PrintSNOfflineNPI_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as PrintSNOfflineNPI_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is PrintSNOfflineSOV_Form)
                {
                    S_MSG = (CurrForm as PrintSNOfflineSOV_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as PrintSNOfflineSOV_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as PrintSNOfflineSOV_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as PrintSNOfflineSOV_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is RePrintFGSN_Form)
                {
                    S_MSG = (CurrForm as RePrintFGSN_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as RePrintFGSN_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as RePrintFGSN_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as RePrintFGSN_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is ScanFGSN_PrintUPC_Auto_Form)
                {
                    S_MSG = (CurrForm as ScanFGSN_PrintUPC_Auto_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as ScanFGSN_PrintUPC_Auto_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as ScanFGSN_PrintUPC_Auto_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as ScanFGSN_PrintUPC_Auto_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is ScanUPC_PrintFGSN_Form)
                {
                    S_MSG = (CurrForm as ScanUPC_PrintFGSN_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as ScanUPC_PrintFGSN_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as ScanUPC_PrintFGSN_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as ScanUPC_PrintFGSN_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is IQCV2_Form)
                {
                    S_MSG = (CurrForm as IQCV2_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as IQCV2_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as IQCV2_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as IQCV2_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is JumpStationQCV2_Form)
                {
                    S_MSG = (CurrForm as JumpStationQCV2_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as JumpStationQCV2_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as JumpStationQCV2_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as JumpStationQCV2_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is KitQCV2_Form)
                {
                    S_MSG = (CurrForm as KitQCV2_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as KitQCV2_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as KitQCV2_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as KitQCV2_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is OverStationV2_Form)
                {
                    S_MSG = (CurrForm as OverStationV2_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as OverStationV2_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as OverStationV2_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as OverStationV2_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is QCV2_Form)
                {
                    S_MSG = (CurrForm as QCV2_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as QCV2_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as QCV2_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as QCV2_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is Repair_QCV2_Form)
                {
                    S_MSG = (CurrForm as Repair_QCV2_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as Repair_QCV2_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as Repair_QCV2_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as Repair_QCV2_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is ToolingOverStationV2_Form)
                {
                    S_MSG = (CurrForm as ToolingOverStationV2_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as ToolingOverStationV2_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as ToolingOverStationV2_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as ToolingOverStationV2_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is IQCNew_Form)
                {
                    S_MSG = (CurrForm as IQCNew_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as IQCNew_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as IQCNew_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as IQCNew_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is KitQC_Form)
                {
                    S_MSG = (CurrForm as KitQC_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as KitQC_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as KitQC_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as KitQC_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is OverStationNoPoNew_Form)
                {
                    S_MSG = (CurrForm as OverStationNoPoNew_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as OverStationNoPoNew_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as OverStationNoPoNew_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as OverStationNoPoNew_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is OverStationNoPo_Form)
                {
                    S_MSG = (CurrForm as OverStationNoPo_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as OverStationNoPo_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as OverStationNoPo_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as OverStationNoPo_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is OverStationPartNew_Form)
                {
                    S_MSG = (CurrForm as OverStationPartNew_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as OverStationPartNew_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as OverStationPartNew_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as OverStationPartNew_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is OverStationPart_Form)
                {
                    S_MSG = (CurrForm as OverStationPart_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as OverStationPart_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as OverStationPart_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as OverStationPart_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is OverStationNew_Form)
                {
                    S_MSG = (CurrForm as OverStationNew_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as OverStationNew_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as OverStationNew_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as OverStationNew_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is QCNoPoNew_Form)
                {
                    S_MSG = (CurrForm as QCNoPoNew_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as QCNoPoNew_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as QCNoPoNew_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as QCNoPoNew_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is QCNoPo_Form)
                {
                    S_MSG = (CurrForm as QCNoPo_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as QCNoPo_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as QCNoPo_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as QCNoPo_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is QCPartNew_Form)
                {
                    S_MSG = (CurrForm as QCPartNew_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as QCPartNew_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as QCPartNew_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as QCPartNew_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is QCPart_Form)
                {
                    S_MSG = (CurrForm as QCPart_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as QCPart_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as QCPart_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as QCPart_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is QCNew_Form)
                {
                    S_MSG = (CurrForm as QCNew_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as QCNew_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as QCNew_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as QCNew_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is FGSNQueryShell_Form)
                {
                    S_MSG = (CurrForm as FGSNQueryShell_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as FGSNQueryShell_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as FGSNQueryShell_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as FGSNQueryShell_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is QueryInactiveCode_Form)
                {
                    S_MSG = (CurrForm as QueryInactiveCode_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as QueryInactiveCode_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as QueryInactiveCode_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as QueryInactiveCode_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is ReplaceBill_Form)
                {
                    S_MSG = (CurrForm as ReplaceBill_Form).Edt_MSG.Text;
                    //try
                    //{
                    //    PartID = Convert.ToInt32((CurrForm as ReplaceBill_Form).Com_Part.EditValue);
                    //    ProductionOrderID = Convert.ToInt32((CurrForm as ReplaceBill_Form).Com_PO.EditValue);
                    //    luUnitStatus = (CurrForm as ReplaceBill_Form).Com_luUnitStatus.Enabled;
                    //}
                    //catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is ShipMentV2_Form)
                {
                    S_MSG = (CurrForm as ShipMentV2_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as ShipMentV2_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as ShipMentV2_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as ShipMentV2_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is ShipMentRemove_Form)
                {
                    S_MSG = (CurrForm as ShipMentRemove_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as ShipMentRemove_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as ShipMentRemove_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as ShipMentRemove_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is TT_OverStation_V2_Form)
                {
                    S_MSG = (CurrForm as TT_OverStation_V2_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as TT_OverStation_V2_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as TT_OverStation_V2_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as TT_OverStation_V2_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is TT_Registers_Form)
                {
                    S_MSG = (CurrForm as TT_Registers_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as TT_Registers_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as TT_Registers_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as TT_Registers_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is Interlock_Form)
                {
                    S_MSG = (CurrForm as Interlock_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as Interlock_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as Interlock_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as Interlock_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is JumpStation_Form)
                {
                    S_MSG = (CurrForm as JumpStation_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as JumpStation_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as JumpStation_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as JumpStation_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is Lamination_Form)
                {
                    S_MSG = (CurrForm as Lamination_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as Lamination_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as Lamination_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as Lamination_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is ModUnitState_Form)
                {
                    S_MSG = (CurrForm as ModUnitState_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as ModUnitState_Form).Com_Part.EditValue);
                        //ProductionOrderID = Convert.ToInt32((CurrForm as ModUnitState_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as ModUnitState_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is ORT_Form)
                {
                    S_MSG = (CurrForm as ORT_Form).Edt_MSG.Text;
                    //try
                    //{
                    //    PartID = Convert.ToInt32((CurrForm as ORT_Form).Com_Part.EditValue);
                    //    ProductionOrderID = Convert.ToInt32((CurrForm as ORT_Form).Com_PO.EditValue);
                    //    luUnitStatus = (CurrForm as ORT_Form).Com_luUnitStatus.Enabled;
                    //}
                    //catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is Plasma_Form)
                {
                    S_MSG = (CurrForm as Plasma_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as Plasma_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as Plasma_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as Plasma_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is Rework_Form)
                {
                    S_MSG = (CurrForm as Rework_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as Rework_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as Rework_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as Rework_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is SetSkin_Form)
                {
                    //S_MSG = (CurrForm as SetSkin_Form).Edt_MSG.Text;
                    //try
                    //{
                    //    PartID = Convert.ToInt32((CurrForm as SetSkin_Form).Com_Part.EditValue);
                    //    ProductionOrderID = Convert.ToInt32((CurrForm as SetSkin_Form).Com_PO.EditValue);
                    //    luUnitStatus = (CurrForm as SetSkin_Form).Com_luUnitStatus.Enabled;
                    //}
                    //catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is SetStation_Form)
                {
                    //S_MSG = (CurrForm as SetStation_Form).Edt_MSG.Text;
                    //try
                    //{
                    //    PartID = Convert.ToInt32((CurrForm as SetStation_Form).Com_Part.EditValue);
                    //    ProductionOrderID = Convert.ToInt32((CurrForm as SetStation_Form).Com_PO.EditValue);
                    //    luUnitStatus = (CurrForm as SetStation_Form).Com_luUnitStatus.Enabled;
                    //}
                    //catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is UnitReplaceTool_Form)
                {
                    S_MSG = (CurrForm as UnitReplaceTool_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as UnitReplaceTool_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as UnitReplaceTool_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as UnitReplaceTool_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is BoxPackage_Form)
                {
                    S_MSG = (CurrForm as BoxPackage_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as BoxPackage_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as BoxPackage_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as BoxPackage_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is OOBA_Form)
                {
                    S_MSG = (CurrForm as OOBA_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as OOBA_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as OOBA_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as OOBA_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is PackageOverStation_Form)
                {
                    S_MSG = (CurrForm as PackageOverStation_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as PackageOverStation_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as PackageOverStation_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as PackageOverStation_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is PackageRemove_Form)
                {
                    S_MSG = (CurrForm as PackageRemove_Form).Edt_MSG.Text;
                    //try
                    //{
                    //    //PartID = Convert.ToInt32((CurrForm as PackageRemove_Form).Com_Part.EditValue);
                    //    ProductionOrderID = Convert.ToInt32((CurrForm as PackageRemove_Form).Com_PO.EditValue);
                    //    //luUnitStatus = (CurrForm as PackageRemove_Form).Com_luUnitStatus.Enabled;
                    //}
                    //catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is PalletPackage_Form)
                {
                    S_MSG = (CurrForm as PalletPackage_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as PalletPackage_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as PalletPackage_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as PalletPackage_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is PrintSNOfflineDOE_Form)
                {
                    S_MSG = (CurrForm as PrintSNOfflineDOE_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as PrintSNOfflineDOE_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as PrintSNOfflineDOE_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as PrintSNOfflineDOE_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is PrintSNOffline_Form)
                {
                    S_MSG = (CurrForm as PrintSNOffline_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as PrintSNOffline_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as PrintSNOffline_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as PrintSNOffline_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is PrintSN_UPC_Form)
                {
                    S_MSG = (CurrForm as PrintSN_UPC_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as PrintSN_UPC_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as PrintSN_UPC_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as PrintSN_UPC_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is RePrintUPC_Form)
                {
                    S_MSG = (CurrForm as RePrintUPC_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as RePrintUPC_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as RePrintUPC_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as RePrintUPC_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is ScanFGSN_PrintUPC_Form)
                {
                    S_MSG = (CurrForm as ScanFGSN_PrintUPC_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as ScanFGSN_PrintUPC_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as ScanFGSN_PrintUPC_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as ScanFGSN_PrintUPC_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is IQC_Form)
                {
                    S_MSG = (CurrForm as IQC_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as IQC_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as IQC_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as IQC_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is OverStation_Form)
                {
                    S_MSG = (CurrForm as OverStation_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as OverStation_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as OverStation_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as OverStation_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is QC_Form)
                {
                    S_MSG = (CurrForm as QC_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as QC_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as QC_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as QC_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is JumpStationQC_Form)
                {
                    S_MSG = (CurrForm as JumpStationQC_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as JumpStationQC_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as JumpStationQC_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as JumpStationQC_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is Repai_QC_Form)
                {
                    S_MSG = (CurrForm as Repai_QC_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as Repai_QC_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as Repai_QC_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as Repai_QC_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is ToolingOverStation_Form)
                {
                    S_MSG = (CurrForm as ToolingOverStation_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as ToolingOverStation_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as ToolingOverStation_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as ToolingOverStation_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is ShipMent_Form)
                {
                    S_MSG = (CurrForm as ShipMent_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as ShipMent_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as ShipMent_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as ShipMent_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is TT_OverStation_Form)
                {
                    S_MSG = (CurrForm as TT_OverStation_Form).Edt_MSG.Text;
                    //try
                    //{
                    //    PartID = Convert.ToInt32((CurrForm as TT_OverStation_Form).Com_Part.EditValue);
                    //    ProductionOrderID = Convert.ToInt32((CurrForm as TT_OverStation_Form).Com_PO.EditValue);
                    //    luUnitStatus = (CurrForm as TT_OverStation_Form).Com_luUnitStatus.Enabled;
                    //}
                    //catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is TT_BindBox_Form)
                {
                    S_MSG = (CurrForm as TT_BindBox_Form).Edt_MSG.Text;
                    try
                    {
                        PartID = Convert.ToInt32((CurrForm as TT_BindBox_Form).Com_Part.EditValue);
                        ProductionOrderID = Convert.ToInt32((CurrForm as TT_BindBox_Form).Com_PO.EditValue);
                        luUnitStatus = (CurrForm as TT_BindBox_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is WH_ShipmentOut_Form)
                {
                    S_MSG = (CurrForm as WH_ShipmentOut_Form).Edt_MSG.Text;
                    //try
                    //{
                    //    PartID = Convert.ToInt32((CurrForm as WH_ShipmentOut_Form).Com_Part.EditValue);
                    //    ProductionOrderID = Convert.ToInt32((CurrForm as WH_ShipmentOut_Form).Com_PO.EditValue);
                    //    luUnitStatus = (CurrForm as WH_ShipmentOut_Form).Com_luUnitStatus.Enabled;
                    //}
                    //catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is WH_Edit_Detail_Form)
                {
                    //S_MSG = (CurrForm as WH_Edit_Detail_Form).Edt_MSG.Text;
                    //try
                    //{
                    //    PartID = Convert.ToInt32((CurrForm as WH_Edit_Detail_Form).Com_Part.EditValue);
                    //    ProductionOrderID = Convert.ToInt32((CurrForm as WH_Edit_Detail_Form).Com_PO.EditValue);
                    //    luUnitStatus = (CurrForm as WH_Edit_Detail_Form).Com_luUnitStatus.Enabled;
                    //}
                    //catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is WH_Edit_Form)
                {
                    //S_MSG = (CurrForm as WH_Edit_Form).Edt_MSG.Text;
                    //try
                    //{
                    //    PartID = Convert.ToInt32((CurrForm as WH_Edit_Form).Com_Part.EditValue);
                    //    ProductionOrderID = Convert.ToInt32((CurrForm as WH_Edit_Form).Com_PO.EditValue);
                    //    luUnitStatus = (CurrForm as WH_Edit_Form).Com_luUnitStatus.Enabled;
                    //}
                    //catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is WH_Form)
                {
                    //S_MSG = (CurrForm as WH_Form).Edt_MSG.Text;
                    //try
                    //{
                    //    PartID = Convert.ToInt32((CurrForm as WH_Form).Com_Part.EditValue);
                    //    ProductionOrderID = Convert.ToInt32((CurrForm as WH_Form).Com_PO.EditValue);
                    //    luUnitStatus = (CurrForm as WH_Form).Com_luUnitStatus.Enabled;
                    //}
                    //catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is WH_In_Form)
                {
                    S_MSG = (CurrForm as WH_In_Form).Edt_MSG.Text;
                    //try
                    //{
                    //    PartID = Convert.ToInt32((CurrForm as WH_In_Form).Com_Part.EditValue);
                    //    ProductionOrderID = Convert.ToInt32((CurrForm as WH_In_Form).Com_PO.EditValue);
                    //    luUnitStatus = (CurrForm as WH_In_Form).Com_luUnitStatus.Enabled;
                    //}
                    //catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is WH_Edit_Detail_Old_Form)
                {
                    //S_MSG = (CurrForm as WH_Edit_Detail_Old_Form).Edt_MSG.Text;
                    //try
                    //{
                    //    PartID = Convert.ToInt32((CurrForm as WH_Edit_Detail_Old_Form).Com_Part.EditValue);
                    //    ProductionOrderID = Convert.ToInt32((CurrForm as WH_Edit_Detail_Old_Form).Com_PO.EditValue);
                    //    luUnitStatus = (CurrForm as WH_Edit_Detail_Old_Form).Com_luUnitStatus.Enabled;
                    //}
                    //catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is WH_Edit_Old_Form)
                {
                    //S_MSG = (CurrForm as WH_Edit_Old_Form).Edt_MSG.Text;
                    //try
                    //{
                    //    PartID = Convert.ToInt32((CurrForm as WH_Edit_Old_Form).Com_Part.EditValue);
                    //    ProductionOrderID = Convert.ToInt32((CurrForm as WH_Edit_Old_Form).Com_PO.EditValue);
                    //    luUnitStatus = (CurrForm as WH_Edit_Old_Form).Com_luUnitStatus.Enabled;
                    //}
                    //catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is WH_In_Old_Form)
                {
                    S_MSG = (CurrForm as WH_In_Old_Form).Edt_MSG.Text;
                    try
                    {
                        //PartID = Convert.ToInt32((CurrForm as WH_In_Old_Form).Com_Part.EditValue);
                        //ProductionOrderID = Convert.ToInt32((CurrForm as WH_In_Old_Form).Com_PO.EditValue);
                        //luUnitStatus = (CurrForm as WH_In_Old_Form).Com_luUnitStatus.Enabled;
                    }
                    catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is WH_Old_Form)
                {
                    //S_MSG = (CurrForm as WH_Old_Form).Edt_MSG.Text;
                    //try
                    //{
                    //    PartID = Convert.ToInt32((CurrForm as WH_Old_Form).Com_Part.EditValue);
                    //    ProductionOrderID = Convert.ToInt32((CurrForm as WH_Old_Form).Com_PO.EditValue);
                    //    luUnitStatus = (CurrForm as WH_Old_Form).Com_luUnitStatus.Enabled;
                    //}
                    //catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is WH_ShipmentOut_Old_Form)
                {
                    S_MSG = (CurrForm as WH_ShipmentOut_Old_Form).Edt_MSG.Text;
                    //try
                    //{
                    //    PartID = Convert.ToInt32((CurrForm as WH_ShipmentOut_Old_Form).Com_Part.EditValue);
                    //    ProductionOrderID = Convert.ToInt32((CurrForm as WH_ShipmentOut_Old_Form).Com_PO.EditValue);
                    //    luUnitStatus = (CurrForm as WH_ShipmentOut_Old_Form).Com_luUnitStatus.Enabled;
                    //}
                    //catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }

                if (CurrForm is Tooling_Form)
                {
                    //S_MSG = (CurrForm as Tooling_Form).Edt_MSG.Text;
                    //try
                    //{
                    //    PartID = Convert.ToInt32((CurrForm as Tooling_Form).Com_Part.EditValue);
                    //    ProductionOrderID = Convert.ToInt32((CurrForm as Tooling_Form).Com_PO.EditValue);
                    //    luUnitStatus = (CurrForm as Tooling_Form).Com_luUnitStatus.Enabled;
                    //}
                    //catch { }

                    S_MSG = S_MSG ?? "";
                    v_ListmesScreenshot.MSG = S_MSG;
                    v_ListmesScreenshot.PartID = PartID;
                    v_ListmesScreenshot.ProductionOrderID = ProductionOrderID;
                    v_ListmesScreenshot.luUnitStatus = luUnitStatus;
                    return v_ListmesScreenshot;
                }



            }

            return v_ListmesScreenshot;
        }

        private void Btn_IssueRequest_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you confirm the screenshot?", "screenshot", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            string S_Path = Application.StartupPath + "\\Screenshot.jpg";
            string S_webName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            try
            {
                //设置显示屏幕为主屏（windows主屏）
                Screen scr = Screen.PrimaryScreen;
                //获取屏幕边界参数
                Rectangle rc = scr.Bounds;
                //获取屏幕分辨率宽度值
                int iWidth = rc.Width;
                //获取屏幕分辨率高度值
                int iHeight = rc.Height;
                //创建Bitmap位图类（尺寸与分辨率相同）            
                Image myImage = new Bitmap(iWidth, iHeight);
                //从一个继承自Image类的对象中创建Graphics对象            
                Graphics g = Graphics.FromImage(myImage);
                //截取屏幕并复制到（g）myimage里            
                g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(iWidth, iHeight));

                //将myImage保存为文件，此处的保存路径和文件名以及图片格式可自行修改。当前文件名：截屏+系统日期时间（年月日 时分秒）
                myImage.Save(S_Path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            //string ss = "";
            //foreach (var type in this.GetType().Assembly.GetTypes())
            //{
            //    if (typeof(Form).IsAssignableFrom(type))
            //    {
            //        try
            //        {
            //            ss += type.Name + "\r\n";

            //        }
            //        catch
            //        {

            //        }
            //    }
            //}


            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            if (FTP == null)
            {
                try
                {
                    string S_FTPIP = public_.GetAppSet(PartSelectSVC, "FTPIP");
                    string S_FTPUser = public_.GetAppSet(PartSelectSVC, "FTPUser");
                    string S_FTPPassword = public_.GetAppSet(PartSelectSVC, "FTPPassword");


                    FTP = new FtpWeb(S_FTPIP, "", S_FTPUser, S_FTPPassword);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "FTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            try
            {
                string S_DateTime = DateTime.Now.ToString("yyyy_MM_dd");
                FTP.GotoDirectory("", true);
                if (FTP.DirectoryExist(S_DateTime) == false)
                {
                    FTP.MakeDir(S_DateTime);
                }
                FTP.GotoDirectory(S_DateTime, true);
                string S_FTPURL = FTP.Upload(S_Path, S_webName);
                //webHTTP
                string S_WebHTTP = "/"+ S_DateTime +"/"+ S_webName;


                ListmesScreenshot v_ListmesScreenshot = GetMSG();
                string hostName = Dns.GetHostName();
                IPHostEntry localhost = Dns.GetHostByName(hostName);


                mesScreenshot v_mesScreenshot = new mesScreenshot();
                v_mesScreenshot.LineID = F_LoginList.LineID;
                v_mesScreenshot.StationID = F_LoginList.StationID;
                v_mesScreenshot.PartID = v_ListmesScreenshot.PartID;
                v_mesScreenshot.ProductionOrderID = v_ListmesScreenshot.ProductionOrderID;

                v_mesScreenshot.IP = localhost.AddressList[0].ToString();
                v_mesScreenshot.PCName = hostName;
                v_mesScreenshot.IMGURL = S_WebHTTP;  // S_FTPURL;
                v_mesScreenshot.MSG = v_ListmesScreenshot.MSG;
                v_mesScreenshot.Feedback = "";
                v_mesScreenshot.IsFeedback = 0;

                S_ErrorID = PartSelectSVC.InsertmesScreenshot(v_mesScreenshot).ToString();
                Btn_IssueRequest.Caption = S_ErrorID;
                Btn_IssueRequest.ItemAppearance.Disabled.ForeColor = Color.Red;
                Btn_IssueRequest.Enabled = false;
                //设置弹出时间
                timer_MSG.Enabled = true;
                timer_MSG.Interval = 30000;
                try
                {
                    int I_FeedbackPopupTimer = Convert.ToInt32(public_.GetAppSet(PartSelectSVC, "FeedbackPopupTimer"));
                    timer_MSG.Interval = I_FeedbackPopupTimer;
                }
                catch { }

                MessageBox.Show("Screenshot OK,ID: " + S_ErrorID, "FTP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        MSGForm v_MSGForm = new MSGForm();
        private void timer_MSG_Tick(object sender, EventArgs e)
        {
            try
            {
                if (S_ErrorID != "")
                {
                    PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();

                    int I_ErrorID = Convert.ToInt32(S_ErrorID);
                    mesScreenshot v_mesScreenshot = PartSelectSVC.GetmesScreenshot(I_ErrorID);
                    if (v_mesScreenshot.IsFeedback == 1)
                    {
                        timer_MSG.Enabled = false;
                        Btn_IssueRequest.Caption = "IssueRequest";
                        Btn_IssueRequest.ItemAppearance.Normal.ForeColor = Color.Black;
                        Btn_IssueRequest.Enabled = true;

                        //是否弹出接收信息
                        int I_IsFeedbackPopup = 0;
                        try
                        {
                            I_IsFeedbackPopup = Convert.ToInt32(public_.GetAppSet(PartSelectSVC, "IsFeedbackPopup"));
                        }
                        catch { }

                        if (I_IsFeedbackPopup == 1)
                        {
                            v_MSGForm.Show_MSGForm(v_MSGForm, v_mesScreenshot.Feedback, PartSelectSVC, v_mesScreenshot);
                        }
                    }
                }
            }
            catch
            {

            }
        }
    }
}