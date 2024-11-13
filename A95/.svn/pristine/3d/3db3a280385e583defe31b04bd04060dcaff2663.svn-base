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
    public partial class Disassembly_Form : FrmBase3
    {
        DataTable RwDt;
        string barcode = string.Empty;
        string ShellSN = string.Empty;
        int ShellpartID;
        bool IsPrint = false;
        string S_LabelName = string.Empty;

        public Disassembly_Form()
        {
            InitializeComponent();
        }

        public override void FrmBase_Load(object sender, EventArgs e)
        {
            try
            {
                base.FrmBase_Load(sender, e);
                GrpControlInputData.Enabled = true;
                this.txtSN.Text = string.Empty;
                txtSN.Focus();
                public_.OpenBartender(List_Login.StationID.ToString());
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                return;
            }
        }

        private void txtSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                barcode = txtSN.Text.Trim();
                string Result = string.Empty;
                DataSet dts = PartSelectSVC.uspCallProcedure("uspGetDisassembly", barcode, null, null, null, null, null, ref Result);
                if (Result == "1")
                {
                    if (RwDt != null)
                    {
                        RwDt.Columns.Clear();
                        RwDt.Clear();
                    }
                    RwDt = dts.Tables[0];
                    RwDt.Columns.Add("Choose", typeof(bool));
                    foreach (DataRow dr in RwDt.Rows)
                    {
                        dr["Choose"] = true;
                    }
                    gridViewRework.Columns.Clear();
                    gridControlViewDetail.DataSource = RwDt;

                    if (dts != null && dts.Tables.Count > 1 && dts.Tables[1].Rows.Count > 0)
                    {
                        ShellSN = dts.Tables[1].Rows[0]["ChildSerialNumber"].ToString();
                        ShellpartID = Convert.ToInt32(dts.Tables[1].Rows[0]["ChildPartID"].ToString());
                    }
                }
                else
                {
                    string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { barcode, ProMsg }, Result);
                    txtSN.SelectAll();
                    barcode = string.Empty;
                    RwDt = null;
                    gridControlViewDetail.DataSource = RwDt;
                }
            }
        }

        private void btnRework_Click(object sender, EventArgs e)
        {
            if (RwDt == null || RwDt.Rows.Count == 0 || string.IsNullOrEmpty(barcode))
            {
                return;
            }

            if (!IsPrint)
            {
                string ProMsg = MessageInfo.GetMsgByCode("10047", List_Login.Language);
                if (MessageBox.Show(ProMsg, "prompt",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            string Result = string.Empty;
            string xmlExtraData = "<ExtraData EmployeeID=" + "\"" + List_Login.EmployeeID + "\"" + "> </ExtraData>";
            string xmlStation = "<Station StationID=" + "\"" + List_Login.StationID.ToString() + "\"" + "> </Station>";
            PartSelectSVC.uspCallProcedure("uspSetDisassembly", barcode, null, null, xmlStation, xmlExtraData, "", ref Result);
            if (Result != "1")
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, Result, "NG", List_Login.Language);
                txtSN.SelectAll();
            }
            else
            {
                ShellSN = string.Empty;
                RwDt = null;
                gridControlViewDetail.DataSource = RwDt;
                MessageInfo.Add_Info_MSG(Edt_MSG, "10033", "OK", List_Login.Language, new string[] { barcode });
                txtSN.Text = "";
                IsPrint = false;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ShellSN))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20107", "NG", List_Login.Language);
                return;
            }

            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_LoginLineID = List_Login.LineID.ToString();
            S_LabelName = public_.GetLabelName(PartSelectSVC, S_StationTypeID, null, ShellpartID.ToString(), null, S_LoginLineID);
            if (string.IsNullOrEmpty(S_LabelName))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20097", "NG", List_Login.Language);
                return;
            }

            DataTable DT_PrintSn = new DataTable();
            DT_PrintSn.Columns.Add("SN", typeof(string));
            DT_PrintSn.Columns.Add("CreateTime", typeof(string));
            DT_PrintSn.Columns.Add("PrintTime", typeof(string));

            DataRow dr = DT_PrintSn.NewRow();
            dr["SN"] = ShellSN;
            dr["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DT_PrintSn.Rows.Add(dr);

            //打印Shell
            string S_PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, S_LabelName, DT_PrintSn, ShellpartID);
            if (S_PrintResult != "OK")
            {
                string ProMsg = MessageInfo.GetMsgByCode(S_PrintResult, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_PrintResult);
                IsPrint = false;
            }
            else
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "10021", "OK", List_Login.Language);
                IsPrint = true;
            }
        }
    }
}
