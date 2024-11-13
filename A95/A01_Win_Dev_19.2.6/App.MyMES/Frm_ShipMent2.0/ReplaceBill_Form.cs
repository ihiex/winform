using App.Model;
using App.MyMES.PartSelectService;
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

namespace App.MyMES
{
    public partial class ReplaceBill_Form : DevExpress.XtraEditors.XtraForm
    {
        public ReplaceBill_Form()
        {
            InitializeComponent();
        }

        ReplaceBill_Form F_ReplaceBill_Form;
        PartSelectSVCClient F_PartSelectSVC;
        LoginList F_List_Login;

        string MupltipackPalletLabelPath = string.Empty;
        public string GS1Name = string.Empty;                   //情况3打印模板名称
        public string GS2Name = string.Empty;                   //情况4打印模板名称
        DataTable DT_PrintSn;
        Public_ public_ = new Public_();

        public void Show_ReplaceBill_Form(ReplaceBill_Form v_ReplaceBill_Form 
                                          ,LoginList v_List_Login
                                          ,PartSelectSVCClient PartSelectSVC
                                          , string S_LabelPath, string S_GS1, string S_GS2
                                        )
        {
            F_ReplaceBill_Form = v_ReplaceBill_Form;
            F_List_Login = v_List_Login;
            F_PartSelectSVC = PartSelectSVC;

            this.MupltipackPalletLabelPath = S_LabelPath;
            this.GS1Name = S_GS1;
            this.GS2Name = S_GS2;

            F_ReplaceBill_Form.ShowDialog();
        }

        private void ReplaceBill_Form_Resize(object sender, EventArgs e)
        {
            Panel_Left.Width = Convert.ToInt32((this.Width / 2) - 200);
        }

        private void Btn_Comfirm_Click(object sender, EventArgs e)
        {
            string S_Result = "1";
            string xmlProdOrder = null;
            string xmlPart = null;
            string xmlExtraData = "<ExtraData EmployeeId=\"" + F_List_Login.EmployeeID.ToString() + "\"> </ExtraData>";
            string xmlStation = "<Station StationId=\"" + F_List_Login.StationID.ToString() + "\"> </Station>";

            string S_BillNo_Old = Edt_Old.Text.Trim();
            string S_BillNo_New = Edt_New.Text.Trim();  

            DataSet dsDetail = F_PartSelectSVC.uspCallProcedure("uspReplaceShipmentPallet", S_BillNo_New,
                                            xmlProdOrder, xmlPart, xmlStation, xmlExtraData, S_BillNo_Old, ref S_Result);

            if (S_Result == "1")
            {
                if (dsDetail != null && dsDetail.Tables.Count > 0 && dsDetail.Tables[0].Rows.Count > 0)
                {
                    DataTable DT_SN = dsDetail.Tables[0];
                    string S_SN = DT_SN.Rows[0][0].ToString();

                    PrintPalletSN(S_SN, S_BillNo_Old, S_BillNo_New);
                }
            }
            else
            {
                string ProMsg = MessageInfo.GetMsgByCode(S_Result, F_List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", F_List_Login.Language, new string[] { ProMsg }, S_Result);
            }
        }

        private void PrintPalletSN(string S_BoxSN, string S_BillNo_Old, string S_BillNo_New)
        {
            string MultipackSN = S_BoxSN;
            if (string.IsNullOrEmpty(MultipackSN))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", F_List_Login.Language);
                return;
            }
            if (string.IsNullOrEmpty(MupltipackPalletLabelPath))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20097", "NG", F_List_Login.Language);
                return;
            }

            string Result = string.Empty;
            DataSet dsRePrintData = F_PartSelectSVC.uspCallProcedure("uspGetShipMentRePrint", MultipackSN,
                                                    null, null, null, null, null, ref Result);
            if (Result != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(Result, F_List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", F_List_Login.Language, new string[] { MultipackSN, ProMsg }, Result);
                return;
            }

            if (dsRePrintData == null || dsRePrintData.Tables.Count == 0 || dsRePrintData.Tables[0].Rows.Count == 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20197", "NG", F_List_Login.Language);
                return;
            }

            string PrintResult = string.Empty;
            DataTable dtRePrintData = dsRePrintData.Tables[0];
            string LabelSCType = dtRePrintData.Rows[0]["LabelSCType"].ToString();
            string PrintBarCode = dtRePrintData.Rows[0]["SerialNumber"].ToString();
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

                PrintResult = Public_.PrintCodeSoftSN(F_PartSelectSVC, MupltipackPalletLabelPath,
                                    DT_PrintSn, 0);

                Thread.Sleep(1000);
                if (PrintResult != "OK")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20095", "NG", F_List_Login.Language, new string[] { MultipackSN, PrintResult });

                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10018", "OK", F_List_Login.Language, new string[] { MultipackSN });
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

                PrintResult = Public_.PrintCodeSoftSN(F_PartSelectSVC, MupltipackPalletLabelPath,
                                    DT_PrintSn, 0);
                Thread.Sleep(1000);
                if (PrintResult != "OK")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20095", "NG", F_List_Login.Language, new string[] { MultipackSN, PrintResult });

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

                    DataSet GS1LabelDs = F_PartSelectSVC.GetMesLabelData(GS1Name);
                    if (GS1LabelDs == null || GS1LabelDs.Tables.Count == 0 || GS1LabelDs.Tables[0].Rows.Count == 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20104", "NG", F_List_Login.Language);
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

                    DataSet GS2LabelDs = F_PartSelectSVC.GetMesLabelData(GS2Name);
                    if (GS2LabelDs == null || GS2LabelDs.Tables.Count == 0 || GS2LabelDs.Tables[0].Rows.Count == 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20104", "NG", F_List_Login.Language);
                        return;
                    }
                    LabelPathData = GS2LabelDs.Tables[0].Rows[0]["LablePath"].ToString();
                }

                PrintResult = Public_.PrintCodeSoftSN(F_PartSelectSVC, LabelPathData,
                                            DT_PrintSn, 0);

                Thread.Sleep(1000);
                if (PrintResult != "OK")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20095", "NG", F_List_Login.Language, new string[] { MultipackSN, PrintResult });

                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10018", "OK", F_List_Login.Language, new string[] { MultipackSN });
                }
            }

            if (PrintResult == "OK")
            {
                mesPackageHistory mesPackageHistory = new mesPackageHistory();
                mesPackageHistory.PackageID = Convert.ToInt32(ShipPalletID);
                mesPackageHistory.PackageStatusID = 7;
                mesPackageHistory.StationID = F_List_Login.StationID;
                mesPackageHistory.EmployeeID = F_List_Login.EmployeeID;
                F_PartSelectSVC.InsertMesPackageHistory(mesPackageHistory); 
                
                MessageInfo.Add_Info_MSG(Edt_MSG, "10028", "OK", F_List_Login.Language, new string[] 
                  { "BillNo Old:" + S_BillNo_Old + "   BillNo New:" + S_BillNo_New });
            }
        }


        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            F_ReplaceBill_Form.Close();
        }

        private void ReplaceBill_Form_Load(object sender, EventArgs e)
        {
            public_.OpenBartender(F_List_Login.StationID.ToString());
        }
    }
}
