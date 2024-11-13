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
    public partial class ScanCheck_Form : FrmBase2
    {
        string OldOKSN = string.Empty;
        string OldNGSN = string.Empty;

        public ScanCheck_Form()
        {
            InitializeComponent();
        }

        public override void FrmBase_Load(object sender, EventArgs e)
        {
            base.FrmBase_Load(sender, e);
            this.GrpControlInputData.Enabled = true;
        }

        private void txtSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DateTime dateStart = DateTime.Now;

                string S_SN = txtSN.Text.Trim();
                if(string.IsNullOrEmpty(S_SN))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                    txtSN.Focus();
                }

                DataSet dsUnit = PartSelectSVC.GetmesSerialNumber(S_SN);

                if(dsUnit==null || dsUnit.Tables.Count==0 || dsUnit.Tables[0].Rows.Count==0)
                {
                    if (S_SN != OldNGSN)
                    {
                        base.SetOverStiaonQTY(false);
                    }
                    OldNGSN = S_SN;
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language,new string[] { S_SN });
                    txtSN.Text = string.Empty;
                    txtSN.Focus();
                }
                else
                {
                    TimeSpan ts = DateTime.Now - dateStart;
                    double mill = Math.Round(ts.TotalMilliseconds, 0);
                    if (S_SN != OldOKSN)
                    {
                        base.SetOverStiaonQTY(true);
                    }
                    OldOKSN = S_SN;
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language,new string[] { S_SN, mill.ToString() });
                    txtSN.Text = string.Empty;
                    txtSN.Focus();
                }

            }
        }
    }
}
