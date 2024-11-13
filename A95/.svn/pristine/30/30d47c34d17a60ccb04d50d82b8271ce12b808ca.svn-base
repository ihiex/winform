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
    public partial class FGSNQueryShell_Form : FrmBase2
    {
        public FGSNQueryShell_Form()
        {
            InitializeComponent();
        }

       

        private void txtUnitSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (txtUnitSN.Text == "")
                {
                    return;
                }
                if ( com_ShellSupplier.EditValue==null || com_ShellSupplier.EditValue.ToString() == "")
                {
                    Public_.Add_Info_MSG(Edt_MSG, "Shell供应商不能为空", "NG");
                    com_ShellSupplier.Select();
                    return;
                }

                string result = string.Empty;
                //DataSet ds = PartSelectSVC.Get_SN_Shell(txtUnitSN.Text.Trim(), ref strOutput);

                DataSet ds = PartSelectSVC.uspCallProcedure("uspGetFGSN_Shell", txtUnitSN.Text.Trim(), "", "",
                                                        "", "", "", ref result);
                if (result != "")
                {
                    MessageBox.Show(result, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string ShellSN = ds.Tables[0].Rows[0]["ChildSerialNumber"].ToString().Substring(0, 3);
                    if (ShellSN == "FPR")
                    {
                        Edt_MSGValue.Text = "XK";

                    }
                    else if (ShellSN == "TGP")
                    {
                        Edt_MSGValue.Text = "TGP";
                    }
                    else
                    {
                        Edt_MSGValue.Text = ShellSN;
                    }

                }
                else
                {
                    Public_.Add_Info_MSG(Edt_MSG, txtUnitSN.Text + "找不到Shell条码", "NG");
                    txtUnitSN.SelectAll();
                    return;
                }

                if (Edt_MSGValue.Text != com_ShellSupplier.EditValue.ToString())
                {
                    //Public_.Add_Info_MSG(Edt_MSG, txtUnitSN.Text + "的Shell供应商为[" + Edt_MSGValue.Text + "]和选择的不一致", "NG");
                    //MessageInfo.Add_Info_MSG(Edt_MSG, "20114", "NG", List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20250", "NG", List_Login.Language, new string[] { txtUnitSN.Text, Edt_MSGValue.Text });
                    txtUnitSN.SelectAll();
                    Edt_MSGValue.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20251", "OK", List_Login.Language, new string[] { txtUnitSN.Text, Edt_MSGValue.Text });
                    txtUnitSN.SelectAll();
                    txtUnitSN.Text = "";
                    Edt_MSGValue.ForeColor = Color.Green;
                }
                
            }
        }

        private void FGSNQueryShell_Form_Load(object sender, EventArgs e)
        {
            base.FrmBase_Load(sender, e);
            base.GrpControlInputData.Enabled = true;
            DataTable dts = new DataTable();
            DataColumn dc1 = new DataColumn("ShellSupplier", typeof(string));
            dts.Columns.Add(dc1);

            DataRow newRow = dts.NewRow();
            newRow["ShellSupplier"] = "TGP";
            dts.Rows.Add(newRow);

            newRow = dts.NewRow();
            newRow["ShellSupplier"] = "XK";
            dts.Rows.Add(newRow);


            com_ShellSupplier.Properties.DataSource = dts;
            com_ShellSupplier.Properties.DisplayMember = "ShellSupplier";
            com_ShellSupplier.Properties.ValueMember = "ShellSupplier";

            Size v_Size = new Size(450, 350);
            com_ShellSupplier.Properties.PopupFormSize = v_Size;

        }
    }
}
