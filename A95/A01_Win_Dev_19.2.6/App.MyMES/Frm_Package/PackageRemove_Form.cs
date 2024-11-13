using App.Model;
using App.MyMES.PartSelectService;
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
    public partial class PackageRemove_Form : DevExpress.XtraEditors.XtraForm
    {

        public LoginList List_Login = new LoginList();
        public PartSelectSVCClient PartSelectSVC;
        public string OpertionType;
        public string RemoveSN = string.Empty;

        public PackageRemove_Form(LoginList Lst, string Type)
        {
            InitializeComponent();
            List_Login = Lst;
            OpertionType = Type;
        }

        private void txtSN_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                btnRemove_Click(null, null);
            }
        }

        private void PackageRemove_Form_Load(object sender, EventArgs e)
        {
            PartSelectSVC = PartSelectFactory.CreateServerClient();
        }

        private void PackageRemove_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            PartSelectSVC.Close();
            this.DialogResult = DialogResult.OK;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            string S_SN = txtSN.Text.Trim();
            if (string.IsNullOrEmpty(S_SN))
            {
                return;
            }

            string xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";
            string xmlExtraData = "<ExtraData EmployeeId=\"" + List_Login.EmployeeID.ToString() + "\"> </ExtraData>";

            string result = string.Empty;
            PartSelectSVC.uspCallProcedure("uspPackageRemoveSingle", S_SN, null, null,
                                                        xmlStation, xmlExtraData, OpertionType, ref result);
            if (result != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, result);
                txtSN.SelectAll();
                return;
            }

            MessageInfo.Add_Info_MSG(Edt_MSG, "10033", "OK", List_Login.Language, new string[] { S_SN });
            txtSN.Text = String.Empty;
        }
    }
}
