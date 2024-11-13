using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Model;
using System.Windows.Forms;
using App.MyMES.mesEmployeeService;
using App.MyMES.PartSelectService;
using System.Diagnostics;
using System.Reflection;

namespace App.MyMES
{
    public partial class MES_Form : Form
    {
        public MES_Form()
        {
            InitializeComponent();
        }
        string S_SystemName = "COSMO MES->";
        LoginList F_LoginList;

        public void Show_MES_Form(MES_Form v_IQCForm, LoginList LoginList)
        {
            F_LoginList = LoginList;
            v_IQCForm.Show();
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
                        Lab_Title.Text = S_SystemName + frm.Text;
                        //Lab_Name.Text = frm.Text;
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
                _Form.Show();
                Lab_Title.Text = S_SystemName + _Form.Text;
                //Lab_Name.Text = _Form.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MES_Form_Resize(object sender, EventArgs e)
        {
            foreach (Form frm in Panel_Main.Controls)
            {
                frm.Width = Panel_Main.Width;
                frm.Height = Panel_Main.Height;
            }
        }

        private void MES_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process[] p = Process.GetProcesses();
            foreach (Process p1 in p)
            {
                try
                {
                    string processName = p1.ProcessName.Trim();
                    //判断是否包含阻碍更新的进程
                    if (processName == "BartenderPrint" || processName=="BartenderPrint_X86")
                   {
                        p1.Kill();
                    }
                }
                catch { }
            }
            Application.Exit();
        }

        private void MES_Form_Load(object sender, EventArgs e)
        {
            ImesEmployeeSVCClient v_mesEmployeeSVCC = ImesEmployeeFactory.CreateServerClient();
            var v_mesEmployee = v_mesEmployeeSVCC.ListAll(" where ID='" + F_LoginList.EmployeeID.ToString() + "'").ToList();
            Lab_Msg.Text = "User:" + v_mesEmployee[0].Lastname + v_mesEmployee[0].Firstname;

            Public_ public_ = new Public_();
            this.Text = "COSMO MES System " + F_LoginList.Ver + "  " + public_.GetServerIP();

            //Panel_Menu.Visible = false;  
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            DataSet DS = PartSelectSVC.GetApplicationType(F_LoginList.StationTypeID.ToString());
            string S_ApplicationType = DS.Tables[0].Rows[0]["Description"].ToString();

            if (F_LoginList.StationType == "BoxLinkBatch")
            {
                ShowForm<BoxLinkBatch_Form>();
            }
            else if (F_LoginList.StationType == "Lamination压合备胶")
            {
                ShowForm<Lamination_Form>();
                //ShowForm<Assemble_Form>(); 
            }
            else if (F_LoginList.StationType == "PrintSN")
            {
                ShowForm<PrintSNForm>();
            }
            else if (F_LoginList.StationType == "UC_OfflineLabelPrint")
            {
                ShowForm<PrintSNOffline_Form>();
            }
            else if (F_LoginList.StationType == "UPC_LablePrint")
            {
                ShowForm<PrintSN_UPC_Form>();
            }
            else if (F_LoginList.ApplicationType == "QC"
                     //|| F_LoginList.ApplicationType == "Assembly"
                     )
            {
                ShowForm<OverStation_Form>();
                //ShowForm<PO_Form>();
            }
            else if (F_LoginList.ApplicationType == "LinkBatch")
            {
                ShowForm<LinkBatch_Form>();
            }
            //else if (F_LoginList.ApplicationType == "Package")
            //{
            //    //ShowForm<OverStation_Form>();
            //}
            else if (F_LoginList.ApplicationType == "LinkUPC")
            {
                ShowForm<LinkUPC_Form>();
            }

            else if (F_LoginList.ApplicationType == "CartonBox")
            {
                ShowForm<BoxPackage_Form>();
            }

            else if (F_LoginList.ApplicationType == "Pallet")
            {
                ShowForm<PalletPackage_Form>();
            }

            v_mesEmployeeSVCC.Close();
            PartSelectSVC.Close();
        }

        private void Btn_Search_Click(object sender, EventArgs e)
        {
            Search_Form v_Search_Form = new Search_Form();
            v_Search_Form.Show_Search_Form(v_Search_Form, F_LoginList);
            //v_Search_Form.Show();
        }

        private void Btn_Close_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        private void Btn_InforCenter_Click(object sender, EventArgs e)
        {
            Select_Form v_Select_Form = new Select_Form();
            v_Select_Form.Show();
        }
    }
}
