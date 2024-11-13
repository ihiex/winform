using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BarPrint.PartSelectService;
using Seagull.BarTender.Print;
using Seagull.BarTender.Print.Database;
using Seagull.BarTender.PrintServer;
using Seagull.BarTender.PrintServer.Tasks;

namespace BarPrint
{
    public partial class BarTender_Form : Form
    {
        string F_Value;
        public BarTender_Form(string S_Value)
        {
            if (S_Value == null || S_Value == "")
            {
//                S_Value =
//@"SN;CreateTime;PrintTime;TmpPath;PartID;
//TEST9009P001;2019-08-28 11:23:29;;D:\\Labels\\A95\\A95a_PTP.btw;186;
//TEST9009P002;2019-08-28 11:23:29;;;;
//TEST9009P003;2019-08-28 11:23:29;;;;
//;";
            }

            F_Value = S_Value;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (F_Value == null || F_Value == "")
            {
                Close();
            }

            DataTable DT_Value = new DataTable();
            string[] List_Value = F_Value.Split('\r');
            this.FormBorderStyle = FormBorderStyle.None;

            try
            {
                if (List_Value.Length > 0)
                {
                    string[] List_Row = List_Value[0].Replace("\n", "").Split(';');
                    for (int i = 0; i < List_Row.Length; i++)
                    {
                        if (List_Row[i] != "")
                        {
                            DT_Value.Columns.Add(List_Row[i]);
                        }
                    }

                    int I_ColCount = DT_Value.Columns.Count;
                    for (int i = 1; i < List_Value.Length; i++)
                    {
                        List_Value[i] = List_Value[i].Replace("\n", "").Trim();

                        if (List_Value[i].Length > 0)
                        {
                            List_Row = List_Value[i].Split(';');

                            if (List_Row[0].Trim().Length > 0)
                            {
                                if (List_Row.Length > 1)
                                {
                                    DataRow DR = DT_Value.NewRow();
                                    for (int j = 0; j < I_ColCount; j++)
                                    {
                                        DR[j] = List_Row[j];
                                    }
                                    DT_Value.Rows.Add(DR);
                                }
                            }
                        }
                    }

                    dataGridView1.DataSource = DT_Value;
                    for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
                    {
                        this.dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            try
            {
                string S_TmpPath = DT_Value.Rows[0]["TmpPath"].ToString();
                int I_PartID = Convert.ToInt32(DT_Value.Rows[0]["PartID"].ToString());

                Engine engine = new Engine(true);
                engine.Start();
                LabelFormatDocument LabFormat = engine.Documents.Open(S_TmpPath);
                LabFormat.PrintSetup.NumberOfSerializedLabels = 1;
                LabFormat.PrintSetup.IdenticalCopiesOfLabel = 1;

                //PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                //DataTable DT_SNParameter = PartSelectSVC.GetSNParameter(I_PartID, 2).Tables[0];

                ////获取配置第一条数据
                //string S_DBField = DT_SNParameter.Rows[0]["DBField"].ToString();
                //string S_TemplateField = DT_SNParameter.Rows[0]["TemplateField"].ToString();
                ////如果发现有 SN1;SN2;SN3  这类配置，值获取这一条配置，其他忽略
                //Boolean B_IsMany = false;
                //for (int j = 0; j < DT_SNParameter.Rows.Count; j++)
                //{
                //    string S_SNParameter = DT_SNParameter.Rows[0]["TemplateField"].ToString();
                //    if (S_SNParameter.IndexOf(";") > 0)
                //    {
                //        B_IsMany = true;
                //    }
                //}

                //int I_RowCount = DT_Value.Rows.Count;
                //if (B_IsMany == true)
                //{
                //    string[] List_TemplateField = S_TemplateField.Split(';');
                //    int I_LabRowQty = List_TemplateField.Length;
                //    decimal I_Group = I_RowCount / I_LabRowQty;
                //    int I_PrintRow = 0;

                //    for (int i = 0; i < I_Group; i++)
                //    {
                //        for (int k = 0; k < I_LabRowQty; k++)
                //        {
                //            LabFormat.SubStrings[List_TemplateField[k]].Value = DT_Value.Rows[I_PrintRow][S_DBField].ToString();
                //            DT_Value.Rows[I_PrintRow]["PrintTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                //            I_PrintRow += 1;
                //        }
                //        LabFormat.Print();
                //    }
                //}
                //else
                //{
                //    for (int i = 0; i < I_RowCount; i++)
                //    {
                //        LabFormat.SubStrings[S_TemplateField].Value = DT_Value.Rows[i][S_DBField].ToString();
                //        DT_Value.Rows[i]["PrintTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //        //LabFormat.Print("ZDesigner 110Xi4 600 dpi Network", out message);

                //        LabFormat.Print();
                //    }
                //}

                string S_Datecode1 = "";
                string S_Datecode2 = "";
                try
                {
                    S_Datecode1 = DT_Value.Rows[0]["Datecode1"].ToString();
                    S_Datecode2 = DT_Value.Rows[0]["Datecode2"].ToString();
                }
                catch
                {

                }

                int I_RowCount = DT_Value.Rows.Count;
                string S_DBPath = Path.GetDirectoryName(S_TmpPath) + "\\BarTender.txt";   //"D:\\Labels\\A95\\A95a_PTP.txt";
                File.Delete(S_DBPath);

                if (S_Datecode1 == "" && S_Datecode2=="")
                {
                    File.AppendAllText(S_DBPath, "\"" + "SN" + "\"" + "\r\n");
                    for (int i = 0; i < I_RowCount; i++)
                    {
                        DT_Value.Rows[i]["PrintTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        File.AppendAllText(S_DBPath, "\"" + DT_Value.Rows[i]["SN"].ToString() + "\"" + "\r\n");
                    }
                }
                else
                {
                    File.AppendAllText(S_DBPath, "\"" + "SN" + "\"" +","+
                                                 "\"" + "Datecode1" + "\"" +","+
                                                 "\"" + "Datecode2" + "\"" +
                                       "\r\n");

                    for (int i = 0; i < I_RowCount; i++)
                    {
                        DT_Value.Rows[i]["PrintTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        File.AppendAllText(S_DBPath, "\"" + DT_Value.Rows[i]["SN"].ToString() + "\"" +","+
                                                     "\"" + S_Datecode1 + "\"" + "," +
                                                     "\"" + S_Datecode2 + "\"" +
                                           "\r\n");
                    }
                }

                LabFormat.Print();
                engine.Stop();
                engine.Dispose();
                //PartSelectSVC.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
