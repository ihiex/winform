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
using System.Configuration;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace App.MyMES
{
    public partial class PrintSNOffline_Form : Form
    {
        public PrintSNOffline_Form()
        {
            InitializeComponent();
        }

        PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
        //string S_Path = Application.StartupPath;
        //string S_TempletName="";
        Public_ public_ = new Public_();
        LoginList List_Login = new LoginList();
        LabelManager2.Application LabSN;
        DataTable DT_PrintSn;
        int I_PartID;

        string S_Print_TemplateType = "1";    //1 :CodeSoft  2：BarTender
        //string S_WindowsVer = "64";           
        //string S_PrintFG_Type = "1";           //2019-08-27  不在使用

        [DllImport("User32.dll", EntryPoint = "SetParent")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        private void PrintSNOffline_Form_Load(object sender, EventArgs e)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                S_Print_TemplateType = config.AppSettings.Settings["Print_TemplateType"].Value.Trim();
                //S_PrintFG_Type = config.AppSettings.Settings["PrintFG_Type"].Value.Trim();
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
            public_.AddPartFamilyType(Com_PartFamilyType,Grid_PartFamilyType);

            List_Login = this.Tag as LoginList;

            public_.AddStationType(Com_StationType,Grid_StationType);
            public_.AddLine(Com_Line,Grid_Line);

            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_LineID = List_Login.LineID.ToString();
            public_.AddStation(Com_Station, S_LineID,Grid_Station);

            Com_StationType.EditValue = S_StationTypeID;
            Com_Line.EditValue = S_LineID;
            Com_Station.EditValue = List_Login.StationID.ToString();

            int I_PartFamilyID = Convert.ToInt32(Com_PartFamilyType.EditValue.ToString());  
            public_.AddLineGroup(Com_LineGroup, Grid_LineGroup, "M", I_PartFamilyID);

            ///////////////////////////////////////////////////////////////////////////////////////
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
                public_.AddLineGroup(Com_LineGroup, Grid_LineGroup, "M", I_PartFamilyID);
            }
            catch
            {
            }
        }

        private void Com_PartFamily_EditValueChanged(object sender, EventArgs e)
        {
            try
            { 
                string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                public_.AddPart(Com_Part, S_PartFamilyID, Grid_Part);
            }
            catch
            {
            }
        }

        private void Get_TempletPath(Boolean IsMsg)
        {
            try
            {
                string S_StationTypeID = List_Login.StationTypeID.ToString();
                string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                string S_PartID = Com_Part.EditValue.ToString();
                string S_ProductionOrderID = "";
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
            try
            { 
                string S_Part = Com_Part.Text.Trim();
          
                if (S_Part!="")
                {
                    I_PartID =Convert.ToInt32(Com_Part.EditValue.ToString());

                    Get_TempletPath(false);


                    //PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();

                    //DataSet dataSet = null;
                    //if (S_Print_TemplateType == "1")
                    //{
                    //    dataSet=PartSelectSVC.GetmesPartDetail(I_PartID, "LabelTemplatePath");
                    //}
                    // else if (S_Print_TemplateType == "2")
                    //{
                    //    dataSet = PartSelectSVC.GetmesPartDetail(I_PartID, "BartenderTemplatePath");
                    //}

                    //if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                    //{
                    //    Edt_Templet.Text = "";
                    //    Public_.Add_Info_MSG(Edt_MSG, "未配置打印模板!", "NG");
                    //}
                    //else
                    //{
                    //    Edt_Templet.Text = dataSet.Tables[0].Rows[0]["Content"].ToString();
                    //    Public_.Add_Info_MSG(Edt_MSG, "", "OK");
                    //}

                    //PartSelectSVC.Close();
                }
            }
            catch
            {
            }           
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();

            if (string.IsNullOrEmpty(Edt_Templet.Text.Trim().Replace(";","") ))
            {
                Public_.Add_Info_MSG(Edt_MSG, "未配置打印文件路径！", "NG");
                return;
            }

            if (!Public_.IsNumeric(txtNum.Text.Trim()))
            {
                Public_.Add_Info_MSG(Edt_MSG, "请在'数量'栏位输入数字类型的值！", "NG");
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
            string S_ProductionOrderID = "";
            string S_LoginLineID = List_Login.LineID.ToString();
                                  
            string S_LabelName = public_.GetLabelName(PartSelectSVC, S_StationTypeID, S_PartFamilyID, S_PartID, S_ProductionOrderID, S_LoginLineID);
            if (S_LabelName == "")
            {
                Public_.Add_Info_MSG(Edt_MSG, "此工序没有配置打印标签！", "NG");
                return;
            }

            int I_Num = Convert.ToInt32(txtNum.Text.Trim());
            if (I_Num > 210)
            {
                Public_.Add_Info_MSG(Edt_MSG, "打印数量不能大于210！", "NG");
                return;
            }

            if (Com_LineGroup.Text.Trim() == "")
            {
                Public_.Add_Info_MSG(Edt_MSG, "线号不能为空！", "NG");
                return;
            }

            //if (S_PrintFG_Type == "2")
            //{
            //    int I_Mod = I_Num % 3;

            //    if (I_Mod > 0)
            //    {
            //        Public_.Add_Info_MSG(Edt_MSG, "数量不是 3 的倍数 ！", "NG");
            //        return;
            //    }
            //}

            int num = Convert.ToInt32(txtNum.Text.Trim());
            string result = string.Empty;            
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
            v_mesUnit.ProductionOrderID = 0;
            v_mesUnit.RMAID = 0;
            v_mesUnit.PartID = I_PartID;
            v_mesUnit.LooperCount = 1;
            v_mesUnit.PartFamilyID = null;//Convert.ToInt32(Com_PartFamily.EditValue);

            v_mesUnit.SerialNumberType = 5;  //FG SerialNumber

            string S_LineID = Grid_LineGroup.GetRowCellValue(Grid_LineGroup.GetSelectedRows()[0], "LineID").ToString(); 
            string S_PartFamilyTypeID = Com_PartFamilyType.EditValue.ToString(); 
            
            string S_xmlPart = "'<Part PartID=" + "\"" + I_PartID + "\"" + "> </Part>'";
            string S_xmlExtraData= "'<ExtraData LineID=" + "\"" + S_LineID + "\"" +
                                             " PartFamilyTypeID=" + "\"" + S_PartFamilyTypeID + "\"" + 
                                             " LineType=" + "\"" + "M" + "\"" + " > </ExtraData>'";

            result = PartSelectSVC.Get_CreateMesSN(null,null, S_xmlPart, null, S_xmlExtraData, v_mesUnit, num, ref dsSN);
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
                if (S_PrintResult!="OK")
                {
                    Public_.Add_Info_MSG(Edt_MSG, "条码打印失败,"+ S_PrintResult, "NG");
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
                DT_PrintSn.Rows[0]["TmpPath"] = Edt_Templet.Text.Trim();
                DT_PrintSn.Rows[0]["PartID"] = I_PartID;

                string[] List_LabelName = S_LabelName.Split(';');
                string[] List_Templet = Edt_Templet.Text.Trim().Split(';');

                for (int i = 0; i < List_LabelName.Length; i++)
                {
                    if (List_LabelName[i] != "")
                    {
                        DT_PrintSn.Rows[0]["TmpPath"] = List_Templet[i];
                        MESLabel.MESLabel.PrintLabel("", PartSelectSVC, LabSN, DT_PrintSn, Panel_Defact,
                                                List_LabelName[i], false, false, "", "", false, "");
                        Thread.Sleep(500);
                    }
                }
            }
            else
            {
                S_Result = "ERROR:打印软件配置错误！";
            }

            return S_Result;
        }



        //     private string PrintCodeSoftSN()
        //     {
        //         string S_Result = "OK";
        //         if (DT_PrintSn == null || DT_PrintSn.Rows.Count == 0)
        //         {
        //             S_Result = "没有条码可以打印";
        //             return S_Result;
        //         }

        //         int I_PartID = Convert.ToInt32(Com_Part.EditValue.ToString());
        //         PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
        ////DataTable DT_SNParameter = PartSelectSVC.GetSNParameter(I_PartID, 1).Tables[0];

        //         if (S_Print_TemplateType == "1")
        //         {
        //             //LabSN = null;
        //             LabelManager2.Document doc = null;
        //             try
        //             {
        //                 if (LabSN == null)
        //                 {
        //                     LabSN = new LabelManager2.Application();
        //                 }
        //                 string S_Path_Lab = Edt_Templet.Text;

        //                 LabSN.Documents.Open(S_Path_Lab, false);
        //                 doc = LabSN.ActiveDocument;
        //             }
        //             catch (Exception ex)
        //             {
        //                 S_Result = ex.Message;
        //             }

        //             try
        //             {
        //                 foreach (DataRow dr in DT_PrintSn.Rows)
        //                 {
        // //for (int i = 0; i < DT_SNParameter.Rows.Count; i++)
        // //{
        // //    string S_DBField = DT_SNParameter.Rows[i]["DBField"].ToString();
        // //    string S_TemplateField = DT_SNParameter.Rows[i]["TemplateField"].ToString();

        // //    doc.Variables.Item(S_TemplateField).Value = dr[S_DBField].ToString();
        // //}

        //                     doc.Variables.Item("SN").Value = dr["SN"].ToString();
        //                     doc.PrintLabel(1);
        //                     dr["PrintTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //                 }
        //                 doc.FormFeed();

        //                 doc.Close();
        //             }
        //             catch (Exception ex)
        //             {
        //                 doc.Close();
        //                 S_Result = ex.ToString();
        //             }
        //             finally
        //             {
        //                 //LabSN.Documents.CloseAll();
        //                 //LabSN.Quit();//退出
        //                 //LabSN = null;
        //                 //doc = null;
        //                 GC.Collect(0);
        //                 PartSelectSVC.Close();
        //             }
        //         }
        //         else if (S_Print_TemplateType == "2")
        //         {
        //             DT_PrintSn.Columns.Add("TmpPath");
        //             DT_PrintSn.Columns.Add("PartID");

        //             int I_ColCount = DT_PrintSn.Columns.Count;
        //             DT_PrintSn.Rows[0]["TmpPath"] = Edt_Templet.Text.Trim();
        //             DT_PrintSn.Rows[0]["PartID"] = I_PartID;

        //             string S_Value = "";
        //             for (int j = 0; j < I_ColCount; j++)
        //             {
        //                 S_Value += DT_PrintSn.Columns[j].ColumnName + ";";
        //             }
        //             S_Value += "\r\n";

        //             for (int i = 0; i < DT_PrintSn.Rows.Count; i++)
        //             {
        //                 string SS = "";
        //                 for (int j = 0; j < I_ColCount; j++)
        //                 {
        //                     SS += DT_PrintSn.Rows[i][j].ToString() + ";";
        //                 }
        //                 S_Value += SS + "\r\n";
        //             }

        //             string S_BarPrint = "";
        //             if (S_WindowsVer == "64")
        //             {
        //                 S_BarPrint = "BarPrint.exe";
        //             }
        //             else if (S_WindowsVer == "32")
        //             {
        //                 S_BarPrint = "BarPrint_X86.exe";
        //             }
        //             Process p = Process.Start(S_BarPrint, S_Value);
        //             //p.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;

        //             while (p.MainWindowHandle.ToInt32() == 0)
        //             {
        //                 System.Threading.Thread.Sleep(200);
        //             }
        //             SetParent(p.MainWindowHandle, Panel_Defact.Handle);
        //             ShowWindow(p.MainWindowHandle, (int)ProcessWindowStyle.Maximized);
        //         }
        //         else
        //         {
        //             S_Result = "ERROR:打印软件配置错误！";
        //         }

        //         return S_Result;
        //     }


        //private string  PrintCodeSoftSN()
        //{
        //    string S_Result = "OK";
        //    if (dtPrintSn == null || dtPrintSn.Rows.Count == 0)
        //    {
        //        S_Result = "没有条码可以打印";
        //        return S_Result;
        //    }

        //    //LabSN = null;
        //    LabelManager2.Document doc = null;
        //    try
        //    {
        //        if (LabSN == null)
        //        {
        //            LabSN = new LabelManager2.Application();
        //        }
        //        string S_Path_Lab = Edt_Templet.Text;

        //        LabSN.Documents.Open(S_Path_Lab, false);
        //        doc = LabSN.ActiveDocument;
        //    }
        //    catch (Exception ex)
        //    {
        //        S_Result = ex.Message;
        //        //Public_.Add_Info_MSG(Edt_MSG, ex.Message, "NG");
        //    }

        //    try
        //    {               
        //        if (S_PrintFG_Type == "2")
        //        {
        //            int I_RowCount = dtPrintSn.Rows.Count;
        //            decimal I_Group = I_RowCount / 3;
        //            int I_Row = 0;

        //            for (int i = 0; i < I_Group; i++)
        //            {
        //                doc.Variables.Item("SN1").Value = dtPrintSn.Rows[I_Row]["SN"].ToString();
        //                dtPrintSn.Rows[I_Row]["PrintTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //                I_Row += 1;

        //                doc.Variables.Item("SN2").Value = dtPrintSn.Rows[I_Row]["SN"].ToString();
        //                dtPrintSn.Rows[I_Row]["PrintTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //                I_Row += 1;

        //                doc.Variables.Item("SN3").Value = dtPrintSn.Rows[I_Row]["SN"].ToString();
        //                dtPrintSn.Rows[I_Row]["PrintTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //                I_Row += 1;

        //                doc.PrintLabel(1);
        //                doc.FormFeed();
        //                Thread.Sleep(100);
        //            }
        //        }
        //        else
        //        {
        //            foreach (DataRow dr in dtPrintSn.Rows)
        //            {
        //                doc.Variables.Item("SN").Value = dr["SN"].ToString();
        //                doc.PrintLabel(1);                       
        //                dr["PrintTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //                //Thread.Sleep(100);
        //            }
        //            doc.FormFeed();
        //        }

        //        doc.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        //Public_.Add_Info_MSG(Edt_MSG, string.Format("条码打印失败;{0}！", ex.ToString()), "NG");
        //          doc.Close();
        //        S_Result = ex.ToString();
        //    }
        //    finally
        //    {
        //        //LabSN.Documents.CloseAll();
        //        //LabSN.Quit();//退出
        //        //LabSN = null;
        //        //doc = null;
        //        GC.Collect(0);
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

            int I_PartID = Convert.ToInt32(Com_Part.EditValue.ToString());
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
 //DataTable DT_SNParameter = PartSelectSVC.GetSNParameter(I_PartID, 1).Tables[0];

            if (S_Print_TemplateType == "1")
            {
                if (LabSN == null)
                {
                    LabSN = new LabelManager2.Application();
                }

                string S_Path_Lab = Edt_Templet.Text.Trim();
                LabSN.Documents.Open(S_Path_Lab, false);
                LabelManager2.Document doc = LabSN.ActiveDocument;
                //doc.Variables.Item("SN").Value = txtSN.Text.Trim();

                //for (int i = 0; i < DT_SNParameter.Rows.Count; i++)
                //{
                //    //string S_DBField = DT_SNParameter.Rows[i]["DBField"].ToString();
                //    string S_TemplateField = DT_SNParameter.Rows[i]["TemplateField"].ToString();

                //    doc.Variables.Item(S_TemplateField).Value = txtSN.Text.Trim();
                //}


                doc.Variables.Item("SN").Value = txtSN.Text.Trim();
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
                    Public_.Add_Info_MSG(Edt_MSG, string.Format("补打条码{0}失败！",ex.ToString()), "NG");
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

            else if (S_Print_TemplateType == "2")
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
                DataRow DR = DT_PrintSn.NewRow();
                DT_PrintSn.Rows.Add(DR);

                DT_PrintSn.Columns.Add("SN");
                DT_PrintSn.Columns.Add("CreateTime");
                DT_PrintSn.Columns.Add("PrintTime");
                DT_PrintSn.Columns.Add("TmpPath");
                DT_PrintSn.Columns.Add("PartID");

                int I_ColCount = DT_PrintSn.Columns.Count;
                DT_PrintSn.Rows[0][0] = txtSN.Text.Trim();
                DT_PrintSn.Rows[0]["TmpPath"] = Edt_Templet.Text.Trim();
                DT_PrintSn.Rows[0]["PartID"] = I_PartID;

                string S_Value = "";
                for (int j = 0; j < I_ColCount; j++)
                {
                    S_Value += DT_PrintSn.Columns[j].ColumnName + ";";
                }
                S_Value += "\r\n";

                string SS = "";
                for (int j = 0; j < I_ColCount; j++)
                {
                    SS += DT_PrintSn.Rows[0][j].ToString() + ";";
                }
                S_Value += SS + "\r\n";

                Process p = Process.Start("BarPrint.exe", S_Value);
                //p.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;

                while (p.MainWindowHandle.ToInt32() == 0)
                {
                    System.Threading.Thread.Sleep(200);
                }
                SetParent(p.MainWindowHandle, Panel_Defact.Handle);
                ShowWindow(p.MainWindowHandle, (int)ProcessWindowStyle.Maximized);
            }
            else
            {
                Public_.Add_Info_MSG(Edt_MSG, "ERROR:打印软件配置错误！", "NG");               
            }

        }

        private void PrintSNOffline_Form_FormClosing(object sender, FormClosingEventArgs e)
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

        private void txtSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Btn_Reprint_Click(sender, e);
            }
        }

    }
}
