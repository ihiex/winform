using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.MyMES
{
    public partial class BoxPackage_VerifyNew_RePrint_Form : Form
    {
        public BoxPackage_VerifyNew_RePrint_Form()
        {
            InitializeComponent();
        }

        public delegate void De_RePrint(string S_BoxSN);
        private De_RePrint F_De_RePrint; 

        public void Show_BoxPackage_VerifyNew_RePrint_Form(BoxPackage_VerifyNew_RePrint_Form v_BoxPackage_VerifyNew_RePrint_Form
            , De_RePrint v_De_RePrint)
        {
            F_De_RePrint = v_De_RePrint;
            v_BoxPackage_VerifyNew_RePrint_Form.ShowDialog();
        }

        private void Edt_BoxSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                F_De_RePrint(Edt_BoxSN.Text.Trim());

                Edt_BoxSN.Focus();
                Edt_BoxSN.Text = "";
            }
        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            F_De_RePrint(Edt_BoxSN.Text.Trim());
            Edt_BoxSN.Focus();
            Edt_BoxSN.Text = "";
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
