using App.MyMES.mesSerialNumberService;
using App.MyMES.mesSerialNumberOfLineService;
using App.MyMES.PartSelectService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.Model;
using App.MyMES.mesUnitService;
using App.MyMES.mesHistoryService;

namespace App.MyMES
{
    public partial class PrintSNForm : Form
    {

        string S_Path = Application.StartupPath;
        string S_TempletName= "\\Lab\\SN1.lab";
        LabelManager2.Application LabSN = new LabelManager2.Application();

        Public_ public_ = new Public_();
        LoginList List_Login = new LoginList(); 
        public PrintSNForm()
        {
            InitializeComponent();
        }

        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            try
            {
                List_Login = this.Tag as LoginList;

                LabSN.Quit();
                LabSN = new LabelManager2.Application();
                AddSNCategory();

                //AddSNFormat();
                public_.AddPartPrint(Com_Part);

                Grid_SN.DataSource = null;
            }
            catch
            {

            }
        }

        private void AddSNCategory()
        {
            string S_Sql = "select *  from luSerialNumberOfLine ";
            DataTable DT = public_.P_DataSet(S_Sql).Tables[0];  

            Com_SNCategory.Items.Clear();
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                Com_SNCategory.Items.Add(DT.Rows[i]["Description"].ToString());  
            }
        }


        //private void AddSNFormat()
        //{
        //    string S_Sql = "select *  from mesSNFormat ";
        //    DataTable DT = public_.P_DataSet(S_Sql).Tables[0];

        //    Com_Format___.Items.Clear();
        //    for (int i = 0; i < DT.Rows.Count; i++)
        //    {
        //        Com_Format___.Items.Add(DT.Rows[i]["Name"].ToString());
        //    }
        //}



        private void Rbtn_NotPrinted_CheckedChanged(object sender, EventArgs e)
        {
            AddData();
        }

        private void RBtn_AlreadyPrinted_CheckedChanged(object sender, EventArgs e)
        {
            AddData();
        }

        private void AddData()
        {
            //string S_SNCategory = Com_SNCategory.Text.Trim();
            DataRowView DRV_Part = Com_Part.SelectedItem as DataRowView;           
            string S_SNCategory= DRV_Part["PartNumber"].ToString();

            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            DataTable DT = new DataTable();

            if (RBtn_AlreadyPrinted.Checked)
            {
                DT = PartSelectSVC.GetmesSerialNumberOfLine(S_SNCategory, "1").Tables[0];
            }
            else
            {
                DT = PartSelectSVC.GetmesSerialNumberOfLine(S_SNCategory, "0").Tables[0];
            }
            Grid_SN.DataSource = DT;
            PartSelectSVC.Close();

            for (int i = 0; i < this.Grid_SN.Columns.Count; i++)
            {
                this.Grid_SN.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void PrintSNForm_Load(object sender, EventArgs e)
        {
            AddSNCategory();
            //AddSNFormat();
            public_.AddPartPrint(Com_Part);
            Rbtn_NotPrinted.Checked = true;

            //Com_Format___.SelectedIndex = 0;
            Com_Part.SelectedIndex = 0;
            Com_SNCategory.SelectedIndex = 0;              
            Com_Templet.SelectedIndex = 0;
            Com_Qty.SelectedItem = 0;

            List_Login = this.Tag as LoginList;
        }

        private void Com_SNCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            //AddData();
        }

        private void Btn_PrintTest_Click(object sender, EventArgs e)
        {
            if (LabSN == null)
            {
                LabSN = new LabelManager2.Application();
            }

            string S_Path_Lab = S_Path + S_TempletName;
            LabSN.Documents.Open(S_Path_Lab, false);
            LabelManager2.Document doc = LabSN.ActiveDocument;
            doc.Variables.Item("SN").Value = Edt_SN.Text.Trim();

            try
            {                                
                doc.PrintDocument(1);

                doc.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                LabSN.Documents.CloseAll();
                LabSN.Quit();//退出
                LabSN = null;
                doc = null;
                GC.Collect(0);
            }
        }

        private void Btn_Print_Click(object sender, EventArgs e)
        {
            LabSN = null;
            LabelManager2.Document doc = null;
            if (LabSN == null)
            {
                LabSN = new LabelManager2.Application();
            }
            string S_Path_Lab = S_Path + S_TempletName;

            LabSN.Documents.Open(S_Path_Lab, false);
            doc = LabSN.ActiveDocument;

            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            ImesSerialNumberOfLineSVCClient mesSerialNumberOfLineSVC = new ImesSerialNumberOfLineSVCClient();
            mesSerialNumberOfLine v_mesSerialNumberOfLine = new mesSerialNumberOfLine();

            try
            {
                List<SelectRowSEQ> List_ID = new List<SelectRowSEQ>();

                for (int i = 0; i < Grid_SN.SelectedRows.Count; i++)    //Grid_SN.SelectedRows.Count
                {
                    string S_ID = Grid_SN.Rows[i].Cells["ID"].Value.ToString();
                    string S_SerialNumber = Grid_SN.Rows[i].Cells["SerialNumber"].Value.ToString();

                    doc.Variables.Item("SN").Value = S_SerialNumber;
                    doc.PrintLabel(1);

                    int I_ID = Convert.ToInt32(S_ID);
                    v_mesSerialNumberOfLine = mesSerialNumberOfLineSVC.Get(I_ID);

                    string S_Sql = "Update mesSerialNumberOfLine set PrintCount=PrintCount+1,FirstPrintTime=getdate(),LastPrintTime=getdate() where ID='" + S_ID + "'";
                    if (v_mesSerialNumberOfLine.PrintCount != 0)
                    {
                        S_Sql = "Update mesSerialNumberOfLine set PrintCount=PrintCount+1,LastPrintTime=getdate() where ID='" + S_ID + "'";
                    }
                    string S_SaveResult = PartSelectSVC.ExecSql(S_Sql);

                    if (S_SaveResult != "OK")
                    {
                        MessageBox.Show(S_SaveResult, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                doc.FormFeed();

                AddData();
                doc.Close();
                MessageBox.Show("打印完成！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                doc.Close();
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                LabSN.Documents.CloseAll();
                LabSN.Quit();//退出
                LabSN = null;
                doc = null;
                GC.Collect(0);
            }
        }

        private void Com_Templet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Com_Templet.Text.Trim() == "1D")
            {
                S_TempletName = "\\Lab\\SN1.lab";
            }
            else
            {
                S_TempletName = "\\Lab\\SN2.lab";
            }
        }

        private void Btn_CreateSN_Click(object sender, EventArgs e)
        {
            try
            {
                //if (Check_RegisterSN.Checked == false)
                //{
                //    if (MessageBox.Show("确认创建SN后  但不注册SN！", "Exclamation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                //    {
                //        return;
                //    }
                //}
                //else 
                //{
                //    if (MessageBox.Show("确认创建SN后  并注册SN！", "Exclamation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                //    {
                //        return;
                //    }
                //}

                if (Com_Qty.Text == "")
                {
                    MessageBox.Show("数量不能为空！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    DataRowView DRV_Part =Com_Part.SelectedItem as DataRowView;
                    string S_FormatName = DRV_Part["SNFormatName"].ToString();
                    string v_PartID= DRV_Part["ID"].ToString();

                    string S_xmlPart = "null";
                    if (S_FormatName == "HP-FG")
                    {                       
                        S_xmlPart = "'<Part PartID=" + "\""+ v_PartID + "\"" + "> </Part>'";
                    }

                    int I_QTY = Convert.ToInt32(Com_Qty.Text.Trim());
                    PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();

                    for (int i = 1; i <= I_QTY; i++)
                    {
                        //  string ss= "exec [dbo].[uspSNRGetNext] '" + S_FormatName + "',0,null," + S_xmlPart + ",'',null,null  ";
                        // string S_Sql = "exec [dbo].[uspSNRGetNext] '" + S_FormatName + "',0,null,"+S_xmlPart+",'',null,null  ";
                        //DataSet DS = public_.P_DataSet(S_Sql);

                        DataTable DT = new DataTable(); //PartSelectSVC.uspSNRGetNext(S_FormatName, null, S_xmlPart, null, null).Tables[1];  //DS.Tables[1];
                        string S_SN = DT.Rows[0][0].ToString();
                        string S_Sql = "";

                        //解决  生成  SN条码 Bug  ******************************************************************************************
                        // 2019-07-16  存储过程已处理
                        //S_Sql = " select *  from mesPartDetail  WHERE  PartDetailDefID=8  and PartID=" + v_PartID;
                        //DataSet DS = public_.P_DataSet(S_Sql);

                        //if (DS.Tables.Count > 0 && S_FormatName == "HP-FG")
                        //{
                        //    DataTable DT_PartDetail = DS.Tables[0];
                        //    string S_HpAssemblyCode = DT_PartDetail.Rows[0]["Content"].ToString();

                        //    S_SN = S_SN.Substring(0, 1) + S_HpAssemblyCode + S_SN.Substring(5, S_SN.Length - 5);
                        //}
                        /////////////////////////// *******************************************************************************************

                        string S_PartID=DRV_Part["ID"].ToString();
                        string S_SNCategory = DRV_Part["PartNumber"].ToString();
                        ////////////////////////////////////
                        S_Sql = "insert into mesSerialNumberOfLine(SerialNumber,SNCategory,PrintCount) values(" +
                                      "'" + S_SN + "','" + S_SNCategory + "',0)";
                        public_.ExecSql(S_Sql);
                        ////////////////////////////////////////
                        S_Sql = " select *  from  luSerialNumberOfLine  where Description='" + S_SNCategory + "'";
                        DataTable DT_luSerialNumberOfLine= public_.P_DataSet(S_Sql).Tables[0];
                        if (DT_luSerialNumberOfLine.Rows.Count == 0)
                        {
                            S_Sql = "insert into luSerialNumberOfLine(Description) values(" +"'" + S_SNCategory + "')";
                            public_.ExecSql(S_Sql);
                        }

                        if (Check_RegisterSN.Checked == true)  //默认生成SN的时候，注册SN
                        {
                            ///////////////////////////////////////////////////////////
                            ImesUnitSVCClient mesUnitSVC = ImesUnitFactory.CreateServerClient();
                            ImesSerialNumberSVCClient mesSerialNumberSVC = ImesSerialNumberFactory.CreateServerClient();
                            ImesHistorySVCClient mesHistorySVC = ImesHistoryFactory.CreateServerClient();

                            mesUnit v_mesUnit = new mesUnit();

                            v_mesUnit.UnitStateID = 1;
                            v_mesUnit.StatusID = 1;
                            v_mesUnit.StationID = List_Login.StationID;
                            v_mesUnit.EmployeeID = List_Login.EmployeeID;
                            v_mesUnit.CreationTime = DateTime.Now;
                            v_mesUnit.LastUpdate = DateTime.Now;
                            v_mesUnit.PanelID = 0;
                            v_mesUnit.LineID = List_Login.LineID;
                            v_mesUnit.ProductionOrderID = 0;
                            v_mesUnit.RMAID = 0;
                            v_mesUnit.PartID = Convert.ToInt32(S_PartID);
                            v_mesUnit.LooperCount = 1;

                            string S_InsertUnit = mesUnitSVC.Insert(v_mesUnit);
                            if (S_InsertUnit.Substring(0, 1) != "E")
                            {
                                mesSerialNumber v_mesSerialNumber = new mesSerialNumber();

                                v_mesSerialNumber.UnitID = Convert.ToInt32(S_InsertUnit);
                                v_mesSerialNumber.SerialNumberTypeID = 1;
                                v_mesSerialNumber.Value = S_SN;
                                int I_InsertSN = mesSerialNumberSVC.Insert(v_mesSerialNumber);

                                //////////////////////////////////////////////////////////////////
                                mesHistory v_mesHistory = new mesHistory();

                                v_mesHistory.UnitID = Convert.ToInt32(S_InsertUnit);
                                v_mesHistory.UnitStateID = v_mesUnit.UnitStateID;
                                v_mesHistory.EmployeeID = List_Login.EmployeeID;
                                v_mesHistory.StationID = List_Login.StationID;
                                v_mesHistory.EnterTime = DateTime.Now;
                                v_mesHistory.ExitTime = DateTime.Now;
                                v_mesHistory.ProductionOrderID = 0;
                                v_mesHistory.PartID = Convert.ToInt32(S_PartID);
                                v_mesHistory.LooperCount = 1;
                                int I_InsertHistory = mesHistorySVC.Insert(v_mesHistory);
                            }
                        }

                    }
                    AddData();
                    PartSelectSVC.Close();
                    AddSNCategory();

                    MessageBox.Show("生成SN完成！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {                
                MessageBox.Show(ex.ToString() , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Com_Part_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddData();
        }
    }
}
