using App.Model;
using App.MyMES.PartSelectService;
using DevExpress.XtraEditors;
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
    public partial class ORTReplaceCode_Form : DevExpress.XtraEditors.XtraForm
    {
        public PartSelectSVCClient PartSelectSVC;
        LoginList List_Login = new LoginList();
        string OldORTCode = string.Empty;
        string Project = string.Empty;
        public string NewOrtCode = string.Empty;

        public ORTReplaceCode_Form()
        {
            InitializeComponent();
        }

        public ORTReplaceCode_Form(LoginList Lg,string OldCode, PartSelectSVCClient Ps)
        {
            this.PartSelectSVC = Ps;
            this.List_Login = Lg;
            this.OldORTCode = OldCode;
            InitializeComponent();
        }

        private void ORTAddWeek_Form_Load(object sender, EventArgs e)
        {
            try
            {
                lblOldORTCode.Text = MessageInfo.GetMsgByCode("10042", List_Login.Language);
                lblNewORTCode.Text = MessageInfo.GetMsgByCode("10043", List_Login.Language);
                btnConfim.Text = MessageInfo.GetMsgByCode("10040", List_Login.Language);

                if(!string.IsNullOrEmpty(OldORTCode))
                {
                    int index = OldORTCode.IndexOf('-');
                    txtOldORTCode.Text = OldORTCode.Substring(index + 1, OldORTCode.Length- index - 1);
                    Project= OldORTCode.Substring(0, index);
                }
            }
            catch(Exception ex) { }
        }

        private void btnConfim_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtNewORTCode.Text.Trim()))
            {
                return;
            }

            string message = string.Empty;
            string OrtCode = Project + "-" + txtNewORTCode.Text.Trim();
            string result = string.Empty;
            DataSet dsSNResult = PartSelectSVC.uspCallProcedure("uspORTReplaceCode", OrtCode, "", "",
                                                            "", "", OldORTCode, ref result);
            if (result != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                XtraMessageBox.Show(string.Format(ProMsg, OrtCode),"Hints",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NewOrtCode = OrtCode;
            this.DialogResult = DialogResult.OK;
        }
    }
}
