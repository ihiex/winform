using BarPrint.PartSelectService;
using Seagull.BarTender.Print;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarPrint
{
    public partial class Print2_Form : Form
    {
        public Print2_Form()
        {
            InitializeComponent();
        }

        static string S_Path_NG = Path.Combine(Application.StartupPath + "\\Sounds\\", "NG.wav");
        static string S_Path_OK = Path.Combine(Application.StartupPath + "\\Sounds\\", "OK.wav");
        SoundPlayer Sound_NG = new SoundPlayer(S_Path_NG);
        SoundPlayer Sound_OK = new SoundPlayer(S_Path_OK);
        
        string S_Path = Application.StartupPath;
        string S_Templet_Hp_Vero = "\\Lab_BarTender\\Hp_Vero.btw";
        private void button1_Click(object sender, EventArgs e)
        {
            Engine engine = new Engine(true);
            LabelFormatDocument format = engine.Documents.Open(S_Path + S_Templet_Hp_Vero);
            format.SubStrings["CT_SN"].Value = "AAAAAA";


            Seagull.BarTender.Print.Messages message = new Seagull.BarTender.Print.Messages();
            format.Print("Select printer", out message);
            engine.Dispose();
        }

        private void Print2_Form_Load(object sender, EventArgs e)
        {
            string S_Path = Application.StartupPath;
            MyINI myINI = new MyINI(S_Path + "\\MES_Set");

            string S_LoginDate = myINI.ReadValue("PrintSN2", "PrintSN2");
            if (S_LoginDate == "") { S_LoginDate = "2019-06-23 00:00:00"; }
            TimeSpan TS = DateTime.Now - Convert.ToDateTime(S_LoginDate);
                      
            if (TS.TotalSeconds>5)
            {
                //this.Close();
            }
            else
            {
                this.Text += myINI.ReadValue("PrintSN2", "Ver");
                Lab_Msg.Text= myINI.ReadValue("PrintSN2", "User");                
            }

        }

        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string S_Type = Com_Type.Text.Trim();
                string S_SN = Edt_SN.Text.Trim();   

                if (S_Type == "")
                {
                    //MessageBox.Show("请选择类别！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Add_Info_MSG(Edt_MSG, "请选择类别!", "NG");
                    return;
                }

                if (S_SN == "")
                {                    
                    Add_Info_MSG(Edt_MSG, "请扫描SN!", "NG");
                    Edt_SN.Focus();
                    return;
                }

                Engine engine = new Engine(true);
                PartSelectSVCClient PartSelectSVC = new PartSelectSVCClient();
                try
                {
                    LabelFormatDocument format = engine.Documents.Open(S_Path + "\\Lab_BarTender\\" + S_Type+".btw");
                    DataSet DS = PartSelectSVC.GetPartDetailDef(S_SN);

                    if (DS.Tables.Count == 0)
                    {
                        Add_Info_MSG(Edt_MSG, "SN不存在!", "NG");
                        Edt_SN.SelectAll();
                        return;
                    }
                    DataTable DT = DS.Tables[0];
                    if (DT.Rows.Count == 0)
                    {
                        Add_Info_MSG(Edt_MSG, "SN不存在!", "NG");
                        Edt_SN.SelectAll(); 
                        return;
                    }

                    if (DT.Rows.Count < 5)
                    {
                        Add_Info_MSG(Edt_MSG, "SN属性配置不完整!", "NG");
                        Edt_SN.SelectAll();
                        return;
                    }

                    if (S_Type == "Hp_Vero")
                    {
                        //HP_AV_PN
                        var query_HP_AV_PN = from c in DT.AsEnumerable()
                                             where c.Field<int>("PartDetailDefID") == 4
                                             select c;
                        if (query_HP_AV_PN != null)
                        {
                            DataTable DT_HP_AV_PN = query_HP_AV_PN.CopyToDataTable();
                            format.SubStrings["HP_AV_PN"].Value = DT_HP_AV_PN.Rows[0]["Content"].ToString();
                        }
                        // DateCode
                        format.SubStrings["Date"].Value = GetDateCode();
                        // SPS
                        var query_SPS = from c in DT.AsEnumerable()
                                        where c.Field<int>("PartDetailDefID") == 5
                                        select c;
                        if (query_SPS != null)
                        {
                            DataTable DT_query_SPS = query_SPS.CopyToDataTable();
                            format.SubStrings["SPS"].Value = DT_query_SPS.Rows[0]["Content"].ToString();
                        }
                        // Ver
                        var query_Ver = from c in DT.AsEnumerable()
                                        where c.Field<int>("PartDetailDefID") == 1
                                        select c;
                        if (query_Ver != null)
                        {
                            DataTable DT_query_Ver = query_Ver.CopyToDataTable();
                            format.SubStrings["Ver"].Value = DT_query_Ver.Rows[0]["Content"].ToString();
                        }
                        // CT_SN
                        format.SubStrings["CT_SN"].Value = S_SN;

                        // Rev
                        var query_Rev = from c in DT.AsEnumerable()
                                        where c.Field<int>("PartDetailDefID") == 9
                                        select c;
                        if (query_Rev != null)
                        {
                            DataTable DT_query_Rev = query_Rev.CopyToDataTable();
                            format.SubStrings["Rev"].Value = DT_query_Rev.Rows[0]["Content"].ToString();
                        }
                    }
                    else if (S_Type == "APJ" || S_Type == "ROW")
                    {
                        // SPS
                        var query_SPS = from c in DT.AsEnumerable()
                                        where c.Field<int>("PartDetailDefID") == 5
                                        select c;
                        if (query_SPS != null)
                        {
                            DataTable DT_query_SPS = query_SPS.CopyToDataTable();
                            format.SubStrings["SPS"].Value = DT_query_SPS.Rows[0]["Content"].ToString();
                        }
                        // CT_SN
                        format.SubStrings["CT_SN"].Value = S_SN;
                        //HP_AV_PN
                        var query_HP_AV_PN = from c in DT.AsEnumerable()
                                             where c.Field<int>("PartDetailDefID") == 4
                                             select c;
                        if (query_HP_AV_PN != null)
                        {
                            DataTable DT_HP_AV_PN = query_HP_AV_PN.CopyToDataTable();
                            format.SubStrings["HP_AV_PN"].Value = DT_HP_AV_PN.Rows[0]["Content"].ToString();
                        }
                    }

                    Seagull.BarTender.Print.Messages message = new Seagull.BarTender.Print.Messages();
                    format.Print("ZDesigner 110Xi4 600 dpi Network", out message);
                    engine.Dispose();
                    Add_Info_MSG(Edt_MSG, S_SN+" "+DateTime.Now.ToString("yyyy-Mm-dd HH:mm:ss")+" 打印成功！", "OK");
                    Edt_SN.Text = "";
                    Edt_SN.Focus();
                    PartSelectSVC.Close();
                }
                catch (Exception ex)
                {
                    engine.Dispose();
                    Add_Info_MSG(Edt_MSG, ex.ToString(), "NG");
                    PartSelectSVC.Close();
                }

            }
        }

        private string GetDateCode()
        {
            int I_Mod = DateTime.Now.Month;
            string S_Mod = "";
            if (I_Mod < 9)
            {
                I_Mod = I_Mod + 1;
                S_Mod = I_Mod.ToString();
            }
            else if (I_Mod==9)
            {
                S_Mod = "A";
            }
            else if (I_Mod == 10)
            {
                S_Mod = "B";
            }
            else if (I_Mod == 11)
            {
                S_Mod = "C";
            }
            return "7J"+DateTime.Now.ToString("yyyy").Substring(2,2) +S_Mod+"0";
        }

        private void Add_Info_MSG(RichTextBox Edt_MSG, string S_MSG, string S_Type)
        {
            Edt_MSG.Text = S_MSG;

            try
            {
                if (S_Type == "NG")
                {
                    Sound_NG.Play();
                }
                else if (S_Type == "OK")
                {
                    Sound_OK.Play();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
