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
using App.Model;
using App.MyMES.mesEmployeeService;
using App.MyMES.PartSelectService;

namespace App.MyMES
{
    public partial class MES_Dev2_Form : DevExpress.XtraEditors.XtraForm
    {
        public MES_Dev2_Form()
        {
            InitializeComponent();
        }

        string S_SystemName = "COSMO MES->";
        LoginList F_LoginList;

        public void Show_MES_Form(MES_Dev2_Form v_MainForm, LoginList LoginList)
        {
            F_LoginList = LoginList;
            v_MainForm.Show();
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

        private void MES_Dev2_Form_Load(object sender, EventArgs e)
        {
            ImesEmployeeSVCClient v_mesEmployeeSVCC = new ImesEmployeeSVCClient();
            var v_mesEmployee = v_mesEmployeeSVCC.ListAll(" where ID='" + F_LoginList.EmployeeID.ToString() + "'").ToList();
            Lab_MSG. Text = "User:" + v_mesEmployee[0].Lastname + v_mesEmployee[0].Firstname;

            this.Text = "COSMO MES System " + F_LoginList.Ver;

            //Panel_Menu.Visible = false;  
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            DataSet DS = PartSelectSVC.GetApplicationType(F_LoginList.StationTypeID.ToString());
            string S_ApplicationType = DS.Tables[0].Rows[0]["Description"].ToString();

            if (F_LoginList.StationType == "PO")
            {
                ShowForm<PO_Form>();
            }
            else if (F_LoginList.StationType == "PrintSN")
            {
                ShowForm<PrintSNForm>();
            }
            else if (F_LoginList.StationType == "UC_OfflineLabelPrint")
            {
                //ShowForm<frmPrintSnOffline>();
            }
            else if (F_LoginList.ApplicationType == "QC"
                     || F_LoginList.ApplicationType == "Assembly"
                     )
            {
                ShowForm<ToolingOverStation_Form>();

                //ShowForm<PO_Form>();
            }
            else if (F_LoginList.ApplicationType == "LinkBatch")
            {
                ShowForm<LinkBatch_Form>();

                //ShowForm<Search_Form>();
            }

            PartSelectSVC.Close();
        }

        private void Btn_Search_Click(object sender, EventArgs e)
        {
            Search_Form v_Search_Form = new Search_Form();
            v_Search_Form.Show();
        }

        private void MES_Dev2_Form_Resize(object sender, EventArgs e)
        {
            foreach (Form frm in Panel_Main.Controls)
            {
                frm.Width = Panel_Main.Width;
                frm.Height = Panel_Main.Height;
            }
        }

        private void MES_Dev2_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}