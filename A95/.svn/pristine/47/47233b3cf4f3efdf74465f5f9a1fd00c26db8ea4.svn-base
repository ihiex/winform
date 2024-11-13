using App.Model;
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
    public partial class OOBA_Form : FrmBase2
    {
        string LabelPath = string.Empty;
        string BoxID = string.Empty;
        string PartID = string.Empty;
        DataTable DtPrint = null;
        bool isPrint = true;

        public OOBA_Form()
        {
            InitializeComponent();
        }

        public override void FrmBase_Load(object sender, EventArgs e)
        {
            PartSelectSVC = PartSelectFactory.CreateServerClient();
            List_Login = this.Tag as LoginList;
            //判断是否DOE打印
            DataSet dst = PartSelectSVC.MESGetStationTypeParameter(List_Login.StationTypeID);
            DataRow[] drPOE = dst.Tables[0].Select("Description='IsNotPrint'");
            if (drPOE!=null && drPOE.Length > 0 && drPOE[0]["Content"].ToString() == "1")
            {
                isPrint = false;
            }
            else
            {
                isPrint = true;
            }
        }

        private void txtSN_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                string S_SN = txtSN.Text.Trim();
                if(string.IsNullOrEmpty(S_SN))
                {
                    return;
                }

                //初始化
                DtPrint = null;
                PartID = string.Empty;
                BoxID = string.Empty;
                btnRework.Enabled = false;
                btnPrint.Enabled = false;
                btnPrintRework.Enabled = false;

                string result = string.Empty;
                DataSet dsBoxSN = PartSelectSVC.uspCallProcedure("uspPackageCheckOOBA", S_SN, null, null,null, null, null, ref result);
                if (result != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_SN, ProMsg }, result);
                    txtSN.SelectAll();
                    return;
                }

                if (dsBoxSN == null || dsBoxSN.Tables.Count == 0 || dsBoxSN.Tables[0].Rows.Count == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20190", "NG", List_Login.Language);
                    txtSN.SelectAll();
                    return;
                }

                DataTable dtSN = dsBoxSN.Tables[0];
                DtPrint = dsBoxSN.Tables[1];
                txtMPN.Text = dtSN.Rows[0]["MPN"].ToString();
                txtUPC.Text = dtSN.Rows[0]["UPC"].ToString();
                txtSCC.Text = dtSN.Rows[0]["SCC"].ToString();
                txtCurrCount.Text = dtSN.Rows[0]["CurrentCount"].ToString();
                txtQty.Text = dtSN.Rows[0]["BoxQty"].ToString();
                txtPalletSN.Text = dtSN.Rows[0]["PalletSN"].ToString();
                BoxID= dtSN.Rows[0]["ID"].ToString();
                PartID = dtSN.Rows[0]["CurrPartID"].ToString();

                // 查询模板
                string S_StationTypeID = List_Login.StationTypeID.ToString();
                string S_LoginLineID = List_Login.LineID.ToString();
                string S_PartFamilyID = string.Empty;
                string S_PartID = PartID;
                string S_ProductionOrderID = dtSN.Rows[0]["CurrProductionOrderID"].ToString();

                LabelPath = public_.GetLabelName(PartSelectSVC, S_StationTypeID, S_PartFamilyID,
                                            S_PartID, S_ProductionOrderID, S_LoginLineID);

                if (string.IsNullOrEmpty(LabelPath))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20098", "NG", List_Login.Language);
                    return;
                }

                string pathList = string.Empty;
                string[] ListTemplate = LabelPath.Split(';');
                foreach (string str in ListTemplate)
                {
                    string[] listStr = str.Split(',');
                    pathList = (string.IsNullOrEmpty(pathList) ? "" : pathList + ";") + listStr[1].ToString();
                }
                txtLablePath.Text = pathList.Replace(@"\\", @"\");
                btnPrint.Enabled = true;
                btnPrintRework.Enabled = true;
                if(!isPrint)
                {
                    btnRework.Enabled = true;
                }
                txtSN.Enabled = false;
            }
        }


        /// <summary>
        /// print SN
        /// </summary>
        private bool PrintForMultipackSN()
        {
            if (DtPrint == null || DtPrint.Rows.Count == 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20123", "NG", List_Login.Language);
                txtSN.SelectAll();
                return false;
            }
            if (!DtPrint.Columns.Contains("CreateTime"))
            {
                DtPrint.Columns.Add("CreateTime", typeof(string));
                foreach (DataRow dr in DtPrint.Rows)
                {
                    dr["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            if (!DtPrint.Columns.Contains("PrintTime"))
            {
                DtPrint.Columns.Add("PrintTime", typeof(string));
            }


            string S_PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, LabelPath, DtPrint, Convert.ToInt32(PartID));
            if (S_PrintResult != "OK")
            {
                string ProMsg = MessageInfo.GetMsgByCode(S_PrintResult, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_PrintResult);
                return false;
            }
            else
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "10021", "OK", List_Login.Language);
                return true;

            }
        }

        /// <summary>
        /// Rework
        /// </summary>
        private void ReworkMultipackSN()
        {
            string xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";
            string xmlExtraData = "<ExtraData EmployeeId=\"" + List_Login.EmployeeID.ToString() + "\"> </ExtraData>";

            string result = string.Empty;
            PartSelectSVC.uspCallProcedure("uspPackageReworkOOBA", txtSN.Text.Trim(), null, null,
                                                        xmlStation, xmlExtraData, null, ref result);
            if (result != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { txtSN.Text.Trim(), ProMsg }, result);
                txtSN.SelectAll();
                return;
            }
            else
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "10033", "OK", List_Login.Language,new string[] { txtSN.Text.Trim() });
            }

            BtnReset_Click(null, null);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if(PrintForMultipackSN())
            {
                btnRework.Enabled = true;
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            txtSN.Text = string.Empty;
            txtMPN.Text = string.Empty;
            txtUPC.Text = string.Empty;
            txtSCC.Text = string.Empty;
            txtCurrCount.Text = string.Empty;
            txtQty.Text = string.Empty;
            txtPalletSN.Text = string.Empty;
            txtLablePath.Text = string.Empty;
            txtSN.Enabled = true;
            btnPrint.Enabled = false;
            btnRework.Enabled = false;
            btnPrintRework.Enabled = false;
            txtSN.Focus();
        }

        private void btnPrintRework_Click(object sender, EventArgs e)
        {
            if(PrintForMultipackSN())
            {
                ReworkMultipackSN();
            }
        }

        private void btnRework_Click(object sender, EventArgs e)
        {
            ReworkMultipackSN();
        }

        private void OOBA_Form_Load(object sender, EventArgs e)
        {
            //base.FrmBase_Load(sender, e);

            public_.OpenBartender(List_Login.StationID.ToString());
        }
    }
}
