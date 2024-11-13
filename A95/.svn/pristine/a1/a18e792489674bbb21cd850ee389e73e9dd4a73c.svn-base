using App.MyMES.mesEmployeeService;
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
    public partial class ChangePWDForm : DevExpress.XtraEditors.XtraForm
    {
        public ChangePWDForm()
        {
            InitializeComponent();
        }

        ImesEmployeeSVCClient mesEmployeeSVC = ImesEmployeeFactory.CreateServerClient();
        string F_UserID;
        public void Show_ChangePWDForm(ChangePWDForm v_ChangePWDForm,string UserID)
        {
            F_UserID = UserID;
            v_ChangePWDForm.ShowDialog();
        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            string S_OldPWD = Edt_OldPassword.Text.Trim();
            string S_NewPWD = Edt_NewPassword.Text.Trim();
            string S_ConfirmNewPWD = Edt_ConfirmNewPassword.Text.Trim();

            if (S_OldPWD == "")
            {
                MessageBox.Show("Old password cannot be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (S_NewPWD == "")
            {
                MessageBox.Show("New password cannot be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (S_ConfirmNewPWD == "")
            {
                MessageBox.Show("Confirm New password cannot be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (S_NewPWD != S_ConfirmNewPWD)
            {
                MessageBox.Show("New password is different from the confirmed new password.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (S_NewPWD.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (S_NewPWD=="12345678")
            {
                MessageBox.Show(" New password cannot be 12345678", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string S_Result = mesEmployeeSVC.ChangePWD(F_UserID, S_OldPWD, S_NewPWD);
            if (S_Result == "OK")
            {
                MessageBox.Show("Password is successfully changed", "MSG", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show(S_Result, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
