using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace App.MyMES.Frm_Other
{
    public partial class ReleaseMachineSN_Form : App.MyMES.FrmBase2
    {
        public ReleaseMachineSN_Form()
        {
            InitializeComponent();
        }
        
        private void txtSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Enter)
                return;
            DateTime dateStart = DateTime.Now;
            string tmpMachineSN = txtSN.Text.Trim();
            string Result = string.Empty;
            string ExtraData = string.Empty;
            string S_Sql1 = $"SELECT 1 FROM dbo.mesMachine WHERE SN = '{tmpMachineSN}' AND StatusID = 2";
            DataSet tmpRes = PartSelectSVC.P_DataSet(S_Sql1);
            if (tmpRes == null || tmpRes.Tables.Count !=1 || tmpRes.Tables[0].Rows.Count != 1 || tmpRes.Tables[0].Rows[0][0].ToString() != "1")
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { $"请确认条码是否正确，或者条码已经解锁"  });

                txtSN.Text = string.Empty;
                txtSN.Focus();
                return;
            }


            mesMachineSVC.MesModMachineBySN(tmpMachineSN);
            string S_Sql = "insert into mesMachineHistory(UnitID,MachineID,EnterTime) Values(" +
                    "-1," +
                    $"(SELECT ID FROM dbo.mesMachine WHERE SN = '{tmpMachineSN}')," +
                    "GETDATE()" +
                    ")";
            public_.ExecSql(S_Sql,PartSelectSVC);


            TimeSpan ts = DateTime.Now - dateStart;
            double mill = Math.Round(ts.TotalMilliseconds, 0);
            MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { tmpMachineSN, mill.ToString() });
            txtSN.Text = string.Empty;
            txtSN.Focus();
        }

        private void ReleaseMachineSN_Load(object sender, EventArgs e)
        {
            base.FrmBase_Load(sender, e);
            base.btnHide_Click(sender, e);
            this.GrpControlPart.Visible = false;
            this.GrpControlInputData.Enabled = true;
            txtSN.Focus();
        }
    }
}
