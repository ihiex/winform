using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.Model;
using App.MyMES.PartSelectService;

namespace App.MyMES
{
    public partial class ShipMentRePrint : DevExpress.XtraEditors.XtraForm
    {
        public LoginList List_Login = new LoginList();
        public PartSelectSVCClient PartSelectSVC;
        string MupltipackPalletLabelPath = string.Empty;
        public string GS1Name = string.Empty;                   //情况3打印模板名称
        public string GS2Name = string.Empty;                   //情况4打印模板名称
        DataTable DT_PrintSn;
        Public_ public_ = new Public_(); 

        public ShipMentRePrint()
        {
            InitializeComponent();
        }

        public ShipMentRePrint(LoginList loginList,string LabelPath,string GS1,string GS2)
        {
            InitializeComponent();
            this.List_Login = loginList;
            this.MupltipackPalletLabelPath = LabelPath;
            this.GS1Name = GS1;
            this.GS2Name = GS2;
        }

        private void btnRePrint_Click(object sender, EventArgs e)
        {
            string MultipackSN = txtSN.Text.Trim();
            if(string.IsNullOrEmpty(MultipackSN))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                txtSN.Focus();
                return;
            }
            if(string.IsNullOrEmpty(MupltipackPalletLabelPath))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20097", "NG", List_Login.Language);
                txtSN.Text = string.Empty;
                txtSN.Focus();
                return;
            }

            string Result = string.Empty;
            DataSet dsRePrintData = PartSelectSVC.uspCallProcedure("uspGetShipMentRePrint", MultipackSN,
                                                    null, null, null, null, null, ref Result);
            if(Result!="1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { MultipackSN, ProMsg }, Result);
                txtSN.Text = string.Empty;
                txtSN.Focus();
                return;
            }

            if(dsRePrintData==null || dsRePrintData.Tables.Count==0 || dsRePrintData.Tables[0].Rows.Count==0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20197", "NG", List_Login.Language);
                txtSN.Text = string.Empty;
                txtSN.Focus();
                return; 
            }

            string PrintResult = string.Empty;
            DataTable dtRePrintData = dsRePrintData.Tables[0];
            string LabelSCType = dtRePrintData.Rows[0]["LabelSCType"].ToString();
            string PrintBarCode= dtRePrintData.Rows[0]["SerialNumber"].ToString();
            string ShipPalletSN = dtRePrintData.Rows[0]["PalletSN"].ToString();
            string ShipPalletID = dtRePrintData.Rows[0]["PalletID"].ToString();
            if (string.IsNullOrEmpty(LabelSCType))
            {
                if (DT_PrintSn != null)
                {
                    DT_PrintSn.Columns.Clear();
                    DT_PrintSn.Rows.Clear();
                }
                else
                {
                    DT_PrintSn = new DataTable();
                }
                DT_PrintSn.Columns.Add("SN");
                DT_PrintSn.Columns.Add("CreateTime");
                DT_PrintSn.Columns.Add("PrintTime");
                DataRow DR = DT_PrintSn.NewRow();
                DR["SN"] = PrintBarCode;
                DR["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DT_PrintSn.Rows.Add(DR);

                PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, MupltipackPalletLabelPath,
                                    DT_PrintSn, 0);

    Thread.Sleep(2000);
                if (PrintResult != "OK")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20095", "NG", List_Login.Language, new string[] { MultipackSN, PrintResult });

                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10018", "OK", List_Login.Language, new string[] { MultipackSN });
                }
            }
            else
            {
                if (DT_PrintSn != null)
                {
                    DT_PrintSn.Columns.Clear();
                    DT_PrintSn.Rows.Clear();
                }
                else
                {
                    DT_PrintSn = new DataTable();
                }
                DT_PrintSn.Columns.Add("SN");
                DT_PrintSn.Columns.Add("CreateTime");
                DT_PrintSn.Columns.Add("PrintTime");

                foreach (DataRow drPrint in dtRePrintData.Rows)
                {
                    DataRow DR = DT_PrintSn.NewRow();
                    DR["SN"] = drPrint["SerialNumber"].ToString();
                    DR["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    DT_PrintSn.Rows.Add(DR);
                }

                PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, MupltipackPalletLabelPath,
                                    DT_PrintSn, 0);
     Thread.Sleep(2000);
                if (PrintResult != "OK")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20095", "NG", List_Login.Language, new string[] { MultipackSN, PrintResult });

                }

                string LabelPathData = string.Empty;
                if (LabelSCType == "1")
                {
                    if (DT_PrintSn != null)
                    {
                        DT_PrintSn.Columns.Clear();
                        DT_PrintSn.Rows.Clear();
                    }
                    else
                    {
                        DT_PrintSn = new DataTable();
                    }
                    DT_PrintSn.Columns.Add("SN");
                    DT_PrintSn.Columns.Add("CreateTime");
                    DT_PrintSn.Columns.Add("PrintTime");
                    DataRow DR = DT_PrintSn.NewRow();
                    DR["SN"] = PrintBarCode;
                    DR["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    DT_PrintSn.Rows.Add(DR);

                    DataSet GS1LabelDs = PartSelectSVC.GetMesLabelData(GS1Name);
                    if (GS1LabelDs == null || GS1LabelDs.Tables.Count == 0 || GS1LabelDs.Tables[0].Rows.Count == 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20104", "NG", List_Login.Language);
                        txtSN.Text = string.Empty;
                        txtSN.Focus();
                        return;
                    }
                    LabelPathData = GS1LabelDs.Tables[0].Rows[0]["LablePath"].ToString();
                }
                else
                {
                    if (DT_PrintSn != null)
                    {
                        DT_PrintSn.Columns.Clear();
                        DT_PrintSn.Rows.Clear();
                    }
                    else
                    {
                        DT_PrintSn = new DataTable();
                    }
                    DT_PrintSn.Columns.Add("SN");
                    DT_PrintSn.Columns.Add("CreateTime");
                    DT_PrintSn.Columns.Add("PrintTime");
                    DataRow DR = DT_PrintSn.NewRow();
                    DR["SN"] = ShipPalletSN;
                    DR["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    DT_PrintSn.Rows.Add(DR);

                    DataSet GS2LabelDs = PartSelectSVC.GetMesLabelData(GS2Name);
                    if (GS2LabelDs == null || GS2LabelDs.Tables.Count == 0 || GS2LabelDs.Tables[0].Rows.Count == 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20104", "NG", List_Login.Language);
                        txtSN.Text = string.Empty;
                        txtSN.Focus();
                        return;
                    }
                    LabelPathData = GS2LabelDs.Tables[0].Rows[0]["LablePath"].ToString();
                }

                PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, LabelPathData,
                                            DT_PrintSn, 0);

      Thread.Sleep(2000);
                if (PrintResult != "OK")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20095", "NG", List_Login.Language, new string[] { MultipackSN, PrintResult });

                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10018", "OK", List_Login.Language, new string[] { MultipackSN });
                }
            }

            if (PrintResult == "OK")
            {
                mesPackageHistory mesPackageHistory = new mesPackageHistory();
                mesPackageHistory.PackageID = Convert.ToInt32(ShipPalletID);
                mesPackageHistory.PackageStatusID = 7;
                mesPackageHistory.StationID = List_Login.StationID;
                mesPackageHistory.EmployeeID = List_Login.EmployeeID;
                PartSelectSVC.InsertMesPackageHistory(mesPackageHistory);
                this.txtSN.Text = string.Empty;
            }
            else
            {
                this.txtSN.SelectAll();
            }
        }

        private void ShipMentRePrint_Load(object sender, EventArgs e)
        {
            PartSelectSVC = PartSelectFactory.CreateServerClient();
            public_.OpenBartender(List_Login.StationID.ToString());
        }

        private void ShipMentRePrint_FormClosing(object sender, FormClosingEventArgs e)
        {
            PartSelectSVC.Close();
        }
    }
}
