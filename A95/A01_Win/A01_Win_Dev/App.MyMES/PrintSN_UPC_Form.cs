using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.Model;
using App.MyMES.PartSelectService;
using App.MyMES.mesUnitService;
using App.MyMES.mesSerialNumberService;
using App.MyMES.mesHistoryService;
using App.MyMES.mesUnitDefectService;
using App.MyMES.mesPartDetailService;
using System.Xml;
using System.Threading;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Diagnostics;

namespace App.MyMES
{
    public partial class PrintSN_UPC_Form : Form
    {
        public PrintSN_UPC_Form()
        {
            InitializeComponent();
        }

        PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
        Public_ public_ = new Public_();
        LoginList List_Login = new LoginList();
        LabelManager2.Application LabSN;
        DataTable DT_PrintSn;
        //DataTable DT_PartDetail;

        int I_PartID;
        int I_POID;
        string S_Print_TemplateType = "1";    //1 :CodeSoft  2：BarTender
        //string S_WindowsVer = "64";

        [DllImport("User32.dll", EntryPoint = "SetParent")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);


        private void PrintSN_UPC_Form_Load(object sender, EventArgs e)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                S_Print_TemplateType = config.AppSettings.Settings["Print_TemplateType"].Value.Trim();
            }
            catch
            {

            }
            //try
            //{
            //    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //    S_WindowsVer = config.AppSettings.Settings["WindowsVer"].Value.Trim();
            //}
            //catch
            //{

            //}

            LoadComBox();
        }

        private void LoadComBox()
        {
            public_.AddPartFamilyType(Com_PartFamilyType, Grid_PartFamilyType);

            List_Login = this.Tag as LoginList;

            public_.AddStationType(Com_StationType, Grid_StationType);
            public_.AddLine(Com_Line, Grid_Line);

            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_LineID = List_Login.LineID.ToString();
            public_.AddStation(Com_Station, S_LineID, Grid_Station);

            Com_StationType.EditValue = S_StationTypeID;
            Com_Line.EditValue = S_LineID;
            Com_Station.EditValue = List_Login.StationID.ToString();

            string S_PartID = Com_Part.EditValue.ToString();  
            public_.AddPOAll(Com_PO, S_PartID, List_Login.LineID.ToString(), Grid_PO);

            int I_PartFamilyID = Convert.ToInt32(Com_PartFamilyType.EditValue.ToString());
            public_.AddLineGroup(Com_LineGroup, Grid_LineGroup, "K", I_PartFamilyID);

            //////////////////////////////////////////////////////////////////////////////////////////////
            Get_TempletPath(true);
        }

        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            LoadComBox();
        }

        private void Com_PartFamilyType_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartFamilyTypeID = Com_PartFamilyType.EditValue.ToString();
                public_.AddPartFamily(Com_PartFamily, S_PartFamilyTypeID, Grid_PartFamily);

                int I_PartFamilyID = Convert.ToInt32(Com_PartFamilyType.EditValue.ToString());
                public_.AddLineGroup(Com_LineGroup, Grid_LineGroup, "K", I_PartFamilyID);
            }
            catch
            { }
        }

        private void Com_PartFamily_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                public_.AddPart(Com_Part, S_PartFamilyID, Grid_Part);
            }
            catch
            { }
        }

        private void Get_TempletPath(Boolean IsMsg)
        {
            try
            {
                string S_StationTypeID = List_Login.StationTypeID.ToString();
                string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                string S_PartID = Com_Part.EditValue.ToString();
                string S_ProductionOrderID = Com_PO.EditValue.ToString();
                string S_LoginLineID = List_Login.LineID.ToString();

                string S_LabelPath = public_.GetLabelPath(PartSelectSVC, S_StationTypeID, S_PartFamilyID, S_PartID, S_ProductionOrderID, S_LoginLineID);
                if (S_LabelPath == "")
                {
                    if (IsMsg == true)
                    {
                        Public_.Add_Info_MSG(Edt_MSG, "未配置打印模板!", "NG");
                    }
                }
                else
                {
                    Edt_Templet.Text = S_LabelPath;
                    if (S_LabelPath.Substring(0, 5) == "ERROR")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, S_LabelPath, "NG");
                        Edt_Templet.Text = "";
                    }
                }
            }
            catch
            { }
        }


        private void Com_Part_EditValueChanged(object sender, EventArgs e)
        {
            string S_Part = Com_Part.Text.Trim();
            try
            {               
                if (S_Part != "")
                {
                    Get_TempletPath(false);

                    I_PartID = Convert.ToInt32(Com_Part.EditValue.ToString());                    
                    public_.AddPOAll(Com_PO, I_PartID.ToString() , List_Login.LineID.ToString(), Grid_PO);
                    Com_PO_EditValueChanged(sender, e);
                }
            }
            catch
            { }
        }

        private void Btn_CreateSN_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Edt_Templet.Text.Trim()))
            {
                Public_.Add_Info_MSG(Edt_MSG, "未配置打印文件路径！", "NG");
                return;
            }

            if (!Public_.IsNumeric(txtNum.Text.Trim()))
            {
                Public_.Add_Info_MSG(Edt_MSG, "请在'数量'栏位输入数字类型的值！", "NG");
                return;
            }

            if (Convert.ToInt32(txtNum.Text.Trim()) > 200)
            {
                Public_.Add_Info_MSG(Edt_MSG, "打印数量不能大于200！", "NG");
                return;
            }

            if (Com_LineGroup.Text.Trim() == "")
            {
                Public_.Add_Info_MSG(Edt_MSG, "线号不能为空！", "NG");
                return;
            }

            string S_TempletPath = Edt_Templet.Text.Trim();
            string[] List_TempletPath = S_TempletPath.Split(';');

            string S_TmpType = List_TempletPath[0].Substring(List_TempletPath[0].Length - 3, 3);
            if (S_Print_TemplateType == "1" && S_TmpType != "lab")
            {
                Public_.Add_Info_MSG(Edt_MSG, "CodeSoft 模板配置错误！", "NG");
                return;
            }

            if (S_Print_TemplateType == "2" && S_TmpType != "btw")
            {
                Public_.Add_Info_MSG(Edt_MSG, "BarTender 模板配置错误！", "NG");
                return;
            }

            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
            string S_PartID = Com_Part.EditValue.ToString();
            string S_ProductionOrderID = Com_PO.EditValue.ToString();
            string S_LoginLineID = List_Login.LineID.ToString();

            string S_LabelName = public_.GetLabelName(PartSelectSVC, S_StationTypeID, S_PartFamilyID, S_PartID, S_ProductionOrderID, S_LoginLineID);
            if (S_LabelName == "")
            {
                Public_.Add_Info_MSG(Edt_MSG, "此工序没有配置打印标签！", "NG");
                return;
            }

            int num = Convert.ToInt32(txtNum.Text.Trim());
            string result = string.Empty;
            //PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            try
            {
                DataSet dsSN = new DataSet();

                mesUnit v_mesUnit = new mesUnit();
                v_mesUnit.UnitStateID = 1;
                v_mesUnit.StatusID = 1;
                v_mesUnit.StationID = List_Login.StationID;
                v_mesUnit.EmployeeID = List_Login.EmployeeID;
                v_mesUnit.CreationTime = DateTime.Now;
                v_mesUnit.LastUpdate = DateTime.Now;
                v_mesUnit.PanelID = 0;
                v_mesUnit.LineID = Convert.ToInt32(Com_LineGroup.EditValue);                   //传入值为界面选择线别(注:FG条码生成不分线不用传值)
                v_mesUnit.ProductionOrderID =Convert.ToInt32(Com_PO.EditValue.ToString());
                v_mesUnit.RMAID = 0;
                v_mesUnit.PartID = I_PartID;
                v_mesUnit.LooperCount = 1;
                v_mesUnit.PartFamilyID = null;

                v_mesUnit.SNFamilyID = 10;        //UPC
                v_mesUnit.SerialNumberType = 6;   //UPC SerialNumber

                string S_LineID = Grid_LineGroup.GetRowCellValue(Grid_LineGroup.GetSelectedRows()[0], "LineID").ToString();
                string S_PartFamilyTypeID = Com_PartFamilyType.EditValue.ToString();

                string S_xmlPart = "'<Part PartID=" + "\"" + I_PartID + "\"" + "> </Part>'";
                string S_xmlExtraData = "'<ExtraData LineID=" + "\"" + S_LineID + "\"" +
                                             " PartFamilyTypeID=" + "\"" + S_PartFamilyTypeID + "\"" +
                                             " LineType=" + "\"" + "M" + "\"" + " > </ExtraData>'";
                //string S_xmlExtraData = "'<ExtraData LineID=" + "\"" + Com_LineGroup.Text.Trim() + "\"" + " LineType=" + "\"" + "M" + "\"" + " > </ExtraData>'";
                result = PartSelectSVC.Get_CreateMesSN(null, null, S_xmlPart, null, S_xmlExtraData, v_mesUnit, num, ref dsSN);
                //result = PartSelectSVC.Get_CreateMesSN(null, null, null, null, null, v_mesUnit, num, ref dsSN);
                if (result == "1" && dsSN != null && dsSN.Tables.Count > 0)
                {
                    DT_PrintSn = dsSN.Tables[0];
                    DT_PrintSn.Columns.Add("CreateTime", typeof(string));
                    DT_PrintSn.Columns.Add("PrintTime", typeof(string));
                    foreach (DataRow dr in DT_PrintSn.Rows)
                    {
                        dr["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    dataGridSN.DataSource = DT_PrintSn;
                    for (int i = 0; i < this.dataGridSN.Columns.Count; i++)
                    {
                        this.dataGridSN.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    }


                    string S_PrintResult = PrintCodeSoftSN(S_LabelName);
                    if (S_PrintResult != "OK")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, "条码打印失败," + S_PrintResult, "NG");
                    }
                    else
                    {
                        Public_.Add_Info_MSG(Edt_MSG, "条码生成成功并发送至打印机！", "OK");
                        txtNum.Text = "";

                    }
                }
                else
                {
                    Public_.Add_Info_MSG(Edt_MSG, string.Format("条码生成失败;{0}！", result), "NG");
                }
            }
            catch (Exception ex)
            {
                Public_.Add_Info_MSG(Edt_MSG, string.Format("条码生成失败;{0}！", ex.ToString()), "NG");
            }

            //PartSelectSVC.Close();
        }

        private void txtSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Btn_Reprint_Click(sender, e);
            }
        }

        private string PrintCodeSoftSN(string S_LabelName)
        {
            string S_Result = "OK";
            if (DT_PrintSn == null || DT_PrintSn.Rows.Count == 0)
            {
                S_Result = "没有条码可以打印";
                return S_Result;
            }

            int I_PartID = Convert.ToInt32(Com_Part.EditValue.ToString());
            //PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            //DataTable DT_SNParameter = PartSelectSVC.GetSNParameter(I_PartID, 1).Tables[0];

            if (S_Print_TemplateType == "1")
            {
                try
                {
                    if (LabSN == null)
                    {
                        LabSN = new LabelManager2.Application();
                    }

                    string[] List_LabelName = S_LabelName.Split(';');
                    foreach (var item in List_LabelName)
                    {
                        if (item != "")
                        {
                            MESLabel.MESLabel.PrintLabel("", PartSelectSVC, LabSN, DT_PrintSn, Panel_Defact,
                                                        item, false, false, "", "", false, "");
                        }
                    }
                }
                catch (Exception ex)
                {
                    S_Result = ex.Message;
                }
            }
            else if (S_Print_TemplateType == "2")
            {
                DT_PrintSn.Columns.Add("TmpPath");
                DT_PrintSn.Columns.Add("PartID");

                int I_ColCount = DT_PrintSn.Columns.Count;                
                DT_PrintSn.Rows[0]["PartID"] = I_PartID;

                string[] List_LabelName = S_LabelName.Split(';');
                string[] List_Templet = Edt_Templet.Text.Trim().Split(';');

                for (int i=0;i< List_LabelName.Length;i++ )
                {
                    if (List_LabelName[i] != "")
                    {
                        DT_PrintSn.Rows[0]["TmpPath"] = List_Templet[i];
                        MESLabel.MESLabel.PrintLabel("", PartSelectSVC, LabSN, DT_PrintSn, Panel_Defact,
                                                List_LabelName[i], false, false, "", "", false, "");
                        Thread.Sleep(1000); 
                    }
                }
            }
            else
            {
                S_Result = "ERROR:打印软件配置错误！";
            }

            return S_Result;
        }


        //private string PrintCodeSoftSN()
        //{
        //    string S_Result = "OK";
        //    if (DT_PrintSn == null || DT_PrintSn.Rows.Count == 0)
        //    {
        //        S_Result = "没有条码可以打印";
        //        return S_Result;
        //    }

        //    if (S_Print_TemplateType == "1")
        //    {
        //        //LabSN = null;
        //        LabelManager2.Document doc = null;
        //        try
        //        {
        //            if (LabSN == null)
        //            {
        //                LabSN = new LabelManager2.Application();
        //            }
        //            string S_Path_Lab = Edt_Templet.Text;

        //            LabSN.Documents.Open(S_Path_Lab, false);
        //            doc = LabSN.ActiveDocument;
        //        }
        //        catch (Exception ex)
        //        {
        //            S_Result = ex.Message;
        //        }

        //        try
        //        {
        //            List<string> ListCode = ListVar(I_POID);
        //            if (ListCode.Count == 0)
        //            {
        //                Public_.Add_Info_MSG(Edt_MSG, string.Format("条码打印失败;{0}！", "Datecode1/2  错误"), "NG");
        //            }
        //            else
        //            {
        //                string S_Datecode1 = ListCode[0];
        //                string S_Datecode2 = ListCode[1];

        //                foreach (DataRow dr in DT_PrintSn.Rows)
        //                {
        //                    //doc.Variables.Item("UPCFGSN").Value = dr["SN"].ToString();

        //                    //doc.Variables.Item("Datecode1").Value = S_Datecode1;
        //                    //doc.Variables.Item("Datecode2").Value = S_Datecode2;

        //                    doc.Variables.Item("SN").Value = dr["SN"].ToString();

        //                    doc.PrintLabel(1);
        //                    doc.FormFeed();
        //                    dr["PrintTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //                    Thread.Sleep(100);
        //                }
        //                doc.Close();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            doc.Close();
        //            S_Result = ex.ToString();
        //        }
        //        finally
        //        {
        //            //LabSN.Documents.CloseAll();
        //            //LabSN.Quit();//退出
        //            //LabSN = null;
        //            //doc = null;
        //            GC.Collect(0);                    
        //        }
        //    }
        //    else if (S_Print_TemplateType == "2")
        //    {
        //        DT_PrintSn.Columns.Add("TmpPath");
        //        DT_PrintSn.Columns.Add("PartID");
        //        DT_PrintSn.Columns.Add("Datecode1");
        //        DT_PrintSn.Columns.Add("Datecode2");

        //        int I_ColCount = DT_PrintSn.Columns.Count;
        //        DT_PrintSn.Rows[0]["TmpPath"] = Edt_Templet.Text.Trim();
        //        DT_PrintSn.Rows[0]["PartID"] = I_PartID;

        //        List<string> ListCode = ListVar(I_POID);
        //        string S_Datecode1 = ListCode[0];
        //        string S_Datecode2 = ListCode[1];
        //        DT_PrintSn.Rows[0]["Datecode1"] = S_Datecode1;
        //        DT_PrintSn.Rows[0]["Datecode2"] = S_Datecode2;

        //        string S_Value = "";
        //        for (int j = 0; j < I_ColCount; j++)
        //        {
        //            S_Value += DT_PrintSn.Columns[j].ColumnName + ";";
        //        }
        //        S_Value += "\r\n";

        //        for (int i = 0; i < DT_PrintSn.Rows.Count; i++)
        //        {
        //            string SS = "";
        //            for (int j = 0; j < I_ColCount; j++)
        //            {
        //                SS += DT_PrintSn.Rows[i][j].ToString() + ";";
        //            }
        //            S_Value += SS + "\r\n";
        //        }

        //        try
        //        {
        //            string S_BarPrint = "";
        //            if (S_WindowsVer == "64")
        //            {
        //                S_BarPrint = "BarPrint.exe";
        //            }
        //            else if (S_WindowsVer == "32")
        //            {
        //                S_BarPrint = "BarPrint_X86.exe";
        //            }
        //            Process p = Process.Start(S_BarPrint, S_Value);
        //            //p.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;

        //            while (p.MainWindowHandle.ToInt32() == 0)
        //            {
        //                System.Threading.Thread.Sleep(200);
        //            }
        //            SetParent(p.MainWindowHandle, Panel_Defact.Handle);
        //            ShowWindow(p.MainWindowHandle, (int)ProcessWindowStyle.Maximized);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    else
        //    {
        //        S_Result = "ERROR:打印软件配置错误！";
        //    }

        //    return S_Result;
        //}

        private void Btn_Reprint_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSN.Text.Trim()))
            {
                Public_.Add_Info_MSG(Edt_MSG, "补打条码不能为空！", "NG");
                return;
            }

            if (string.IsNullOrEmpty(Edt_Templet.Text.Trim()))
            {
                Public_.Add_Info_MSG(Edt_MSG, "未配置打印文件路径！", "NG");
                return;
            }

            if (LabSN == null)
            {
                LabSN = new LabelManager2.Application();
            }

            string S_Path_Lab = Edt_Templet.Text.Trim();
            LabSN.Documents.Open(S_Path_Lab, false);
            LabelManager2.Document doc = LabSN.ActiveDocument;

            List<string> ListCode= ListVar(I_POID);
            string S_Datecode1 = ListCode[0];
            string S_Datecode2 = ListCode[1];

            doc.Variables.Item("UPCFGSN").Value = txtSN.Text.Trim();
            doc.Variables.Item("Datecode1").Value = S_Datecode1;
            doc.Variables.Item("Datecode2").Value = S_Datecode2;

            try
            {
                doc.PrintDocument(1);
                doc.FormFeed();
                doc.Close();
                Public_.Add_Info_MSG(Edt_MSG, string.Format("补打条码{0}成功！", txtSN.Text.Trim()), "OK");
                txtSN.Text = "";
            }
            catch (Exception ex)
            {
                Public_.Add_Info_MSG(Edt_MSG, string.Format("补打条码{0}失败！", ex.ToString()), "NG");
            }
            finally
            {
                //LabSN.Documents.CloseAll();
                //LabSN.Quit();//退出
                //LabSN = null;
                //doc = null;
                GC.Collect(0);
            }
        }

        private List<string> ListVar(int v_POID)
        {
            List<string> list = new List<string>();

            try
            {
                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                DataSet DS_Region = PartSelectSVC.GetluPODetailDef(v_POID, "Region");
                PartSelectSVC.Close();

                string S_Sql = "";
                if (DS_Region != null)
                {
                    string S_Region = DS_Region.Tables[0].Rows[0]["Content"].ToString();
                    if (S_Region == "FE")
                    {
                        S_Sql = "select RIGHT(CONVERT(VARCHAR(10), GETDATE(), 105), 7)";
                    }
                    else
                    {
                        S_Sql = "select RIGHT(CONVERT(VARCHAR(10), GETDATE(), 103), 7)";
                    }
                }
                else
                {
                    S_Sql = "select RIGHT(CONVERT(VARCHAR(10), GETDATE(), 103), 7)";
                }
                DataTable DT_Datecode1 = public_.P_DataSet(S_Sql).Tables[0];
                string S_Datecode1 = DT_Datecode1.Rows[0][0].ToString();
                list.Add(S_Datecode1);

                S_Sql = "select CONVERT(VARCHAR(10), DATEADD(year,543,getdate()), 105)";
                DataTable DT_Datecode2 = public_.P_DataSet(S_Sql).Tables[0];
                string S_Datecode2 = DT_Datecode2.Rows[0][0].ToString();
                list.Add(S_Datecode2);
            }
            catch (Exception ex)
            {
                list.Clear();
                Public_.Add_Info_MSG(Edt_MSG, string.Format("{0}", ex.ToString()), "NG");
            }

            return list;
        }



        private void PrintSN_UPC_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                PartSelectSVC.Close();
                LabSN.Quit();
            }
            catch
            {

            }
        }

        private void Com_PO_EditValueChanged(object sender, EventArgs e)
        {
            Get_TempletPath(false);


            //    try
            //    {
            //        string S_PO = Com_PO.Text.Trim();
            //        if (S_PO != "")
            //        {

            //            I_POID = Convert.ToInt32(Com_PO.EditValue.ToString());

            //            //PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            //            DataSet dataSet = null;

            //            if (S_Print_TemplateType == "1")
            //            {
            //                dataSet = PartSelectSVC.GetluPODetailDef(I_POID, "UPC_LabelTemplatePath");
            //            }
            //            else if (S_Print_TemplateType == "2")
            //            {
            //                dataSet = PartSelectSVC.GetluPODetailDef(I_POID, "UPC_BartenderPath");
            //            }

            //            //PartSelectSVC.Close();
            //            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            //            {
            //                Edt_Templet.Text = "";
            //                Public_.Add_Info_MSG(Edt_MSG, "未配置打印模板!", "NG");
            //            }
            //            else
            //            {
            //                Edt_Templet.Text = dataSet.Tables[0].Rows[0]["Content"].ToString();
            //                //Public_.Add_Info_MSG(Edt_MSG, "", "OK");
            //            }
            //        }
            //        else
            //        {
            //            Edt_Templet.Text = "";
            //        }
            //    }
            //    catch
            //    {

            //    }

        }



    }
}
