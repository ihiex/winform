using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using App.Model;
using System.IO.Ports;

namespace App.MyMES
{
    public delegate void FGSNCheckEventHandler1(string Result, string FGSN);
    public delegate void UPCSNSubmitEventHandler1(string UPCSN, string FGSN);

    public partial class SNlinkUPC_Auto_Form : FrmBase
    {
        public event FGSNCheckEventHandler1 FGSNCompletedCheck;
        public event UPCSNSubmitEventHandler1 UPCSNCompletedSubmit;
          
        private string strPort = "COM1";

        bool IsScanUPC = false;
        bool IsScanJAN = false;
        bool IsScanFGSN = false;

        string IsAuto = "0";
        string PassCmd = "PASS";
        string FailCmd = "FAIL";
        //string ReadCmd = "READ";

        string xmlProdOrder;
        string xmlPart;
        string xmlExtraData;
        string xmlStation;

        public SNlinkUPC_Auto_Form()
        {
            InitializeComponent();
            FGSNCompletedCheck += this.FGSNCheckCompleted;
            UPCSNCompletedSubmit += this.UPCSNSubmitCompleted;
        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            IsScanUPC = false;
            IsScanJAN = false;

            if (port1 != null)
            {
                if (port1.IsOpen) port1.Close();
                port1.Dispose();
            }
        }

        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet dsStationDetail = PartSelectSVC.GetPLCSeting("", List_Login.StationID.ToString());

                DataRow[] drAutoStation = dsStationDetail.Tables[0].Select("Name='AutoStation'");
                if (drAutoStation.Length > 0) IsAuto = drAutoStation[0]["Value"].ToString(); else IsAuto = "0";

                DataRow[] drPASS = dsStationDetail.Tables[0].Select("Name='PASS'");
                if (drPASS.Length > 0) PassCmd = drPASS[0]["Value"].ToString(); else PassCmd = "PASS";

                DataRow[] drFAIL = dsStationDetail.Tables[0].Select("Name='FAIL'");
                if (drFAIL.Length > 0) FailCmd = drFAIL[0]["Value"].ToString(); else FailCmd = "FAIL";

                DataRow[] drPort = dsStationDetail.Tables[0].Select("Name='Port'");
                if (drPort.Length > 0) strPort = drPort[0]["Value"].ToString(); else strPort = "COM1";

                xmlProdOrder = ("<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>");
                xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                xmlExtraData = "<ExtraData EmployeeId=\"" + List_Login.EmployeeID.ToString() + "\"> </ExtraData>";
                xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";


                DataSet dtSet = PartSelectSVC.GetProductionOrderDetailDef(Com_PO.EditValue.ToString());
                if (dtSet == null || dtSet.Tables.Count == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20070", "NG", List_Login.Language);
                    return;
                }

                DataTable dtPara = dtSet.Tables[0];

                DataRow[] DR_IsScanFGSN = dtPara.Select("Description='IsScanFGSN'");
                if (DR_IsScanFGSN.Count() > 0)
                {
                    IsScanFGSN = DR_IsScanFGSN[0]["Content"].ToString() == "1";
                }
                if (IsScanFGSN)
                {
                    Lab_CheckFG.Visible = true;
                    Edt_CheckFGSN.Visible = true;
                    tableLayoutPanel1.RowStyles[5].Height = 10;
                }
                else
                {
                    Lab_CheckFG.Visible = false;
                    Edt_CheckFGSN.Visible = false;
                    tableLayoutPanel1.RowStyles[5].Height = 0;
                }



                DataRow[] drUpc = dtPara.Select("Description='IsScanUPC'");
                if (drUpc.Count() > 0)
                {
                    IsScanUPC = drUpc[0]["Content"].ToString() == "1";
                }

                if (IsScanUPC)
                {
                    DataRow[] drUpcCode = dtPara.Select("Description='UPC'");
                    if (drUpcCode.Count() == 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20071", "NG", List_Login.Language);
                        return;
                    }
                    lblUPCCode.Text = drUpcCode[0]["Content"].ToString();
                    lblUPC.Visible = true;
                    txtUPC.Visible = true;
                    lblUPCCode.Visible = true;
                }
                else
                {
                    lblUPC.Visible = false;
                    txtUPC.Visible = false;
                    lblUPCCode.Visible = false;
                }

                DataRow[] drJan = dtPara.Select("Description='IsScanJAN'");
                if (drJan.Count() > 0)
                {
                    IsScanJAN = drJan[0]["Content"].ToString() == "1";
                }

                if (IsScanJAN)
                {
                    DataRow[] drJanCode = dtPara.Select("Description='Jan'");
                    if (drJanCode.Count() == 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20198", "NG", List_Login.Language, new string[] { "Jan" });
                        return;
                    }
                    lblJANCode.Text = drJanCode[0]["Content"].ToString();
                    lblJAN.Visible = true;
                    txtJan.Visible = true;
                    lblJANCode.Visible = true;
                }
                else
                {
                    lblJAN.Visible = false;
                    txtJan.Visible = false;
                    lblJANCode.Visible = false;
                }
                if (IsAuto == "1")
                {
                    if (port1 == null)
                    {
                        port1 = new SerialPort(strPort);
                        port1.BaudRate = 9600;
                        port1.DataBits = 8;
                        port1.Parity = Parity.Odd;
                        port1.StopBits = StopBits.One;
                    }

                    try
                    {
                        if (port1.IsOpen)
                        {
                            port1.Close();
                        }
                        port1.Open();
                    }
                    catch (Exception ex)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "60104", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                        return;
                    }

                    try
                    {
                        string value1 = "B";
                        if (IsScanJAN && IsScanUPC)
                        {
                            value1 = "D";
                        }
                        else if (!IsScanJAN && !IsScanUPC)
                        {
                            value1 = "B";
                        }
                        else
                        {
                            value1 = "C";
                        }
                        port1.WriteLine(value1);
                        //MessageInfo.Add_Info_MSG(Edt_MSG, "60103", "OK", List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "60103", "OK", List_Login.Language, new string[] { value1 });
                        //MessageInfo.Add_Info_MSG(Edt_MSG, "20198", "NG", List_Login.Language, new string[] { "Jan" });
                        //MessageInfo.Add_Info_MSG(Edt_MSG, "60103", "OK", List_Login.Language, new string[] { value1, "60103" }, "60103");
                    }
                    catch (Exception ex)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "60107", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                        return;
                    }
                }

                base.Btn_ConfirmPO_Click(sender, e);
                Edt_SN.Text = string.Empty;
                Edt_UPCSN.Text = string.Empty;
                Edt_SN.Enabled = true;
                Edt_UPCSN.Enabled = false;
                txtUPC.Text = string.Empty;
                txtUPC.Enabled = false;
                txtJan.Text = string.Empty;
                txtJan.Enabled = false;
                Edt_SN.Focus();


                //System.Threading.Thread.Sleep(1000);

            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                return;
            }
        }

        private void FGSNCheckEvent(string Result, string FGSN)
        {
            FGSNCompletedCheck?.Invoke(Result, FGSN);
        }

        private void FGSNCheckCompleted(string Result, string FGSN)
        {
            if (Result != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { FGSN, ProMsg }, Result);
                Edt_SN.Text = string.Empty;
                Edt_SN.Focus();
                return;
            }
            else
            {
                Edt_SN.Enabled = false;
                if (!IsScanUPC && !IsScanJAN)
                {
                    Edt_UPCSN.Text = "";
                    Edt_UPCSN.Enabled = true;
                    Edt_UPCSN.Focus();
                    Edt_UPCSN.SelectAll();
                }
                else if (IsScanUPC)
                {
                    txtUPC.Text = string.Empty;
                    txtUPC.Enabled = true;
                    txtUPC.Focus();
                }
                else
                {
                    txtJan.Text = string.Empty;
                    txtJan.Enabled = true;
                    txtJan.Focus();
                }
            }
        }

        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string FGSN = Edt_SN.Text.Trim();
                string xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";
                string xmlPart = "<Part PartID=\"" + Com_Part.EditValue.ToString() + "\"> </Part>";
                string result = string.Empty;
                string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + Com_PO.EditValue.ToString() + "\"> </ProdOrder>";
                PartSelectSVC.uspCallProcedure("uspFGLinkUPCFGSNCheck", FGSN, xmlProdOrder, xmlPart,
                                                            xmlStation, "", null, ref result);
                if (IsAuto == "1")
                {
                    if (result == "1")
                    {
                        try
                        {
                            port1.WriteLine(PassCmd);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60101", "OK", List_Login.Language);
                        }
                        catch (Exception ex)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60105", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                            Edt_SN.SelectAll();
                            return;
                        }
                    }
                    else
                    {
                        try
                        {
                            port1.WriteLine(FailCmd);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
                        }
                        catch (Exception ex)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                            Edt_SN.SelectAll();
                            return;
                        }
                    }
                }

                FGSNCheckEvent(result, FGSN);

               
            }
        }


        private void UPCSNSubmitEvent(string UPCSN, string FGSN)
        {
            UPCSNCompletedSubmit?.Invoke(UPCSN, FGSN);
        }

        private void UPCSNSubmitCompleted(string UPCSN, string FGSN)
        {
            MessageInfo.Add_Info_MSG(Edt_MSG, "10008", "OK", List_Login.Language, new string[] { FGSN, UPCSN });

            Edt_SN.Text = "";
            Edt_SN.Enabled = true;
            Edt_SN.Focus();
            txtUPC.Text = string.Empty;
            txtJan.Text = string.Empty;

            Edt_UPCSN.Text = "";
            Edt_UPCSN.Enabled = false;
        }

        //private void Edt_UPCSN_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        try
        //        {
        //            string UPCSN = Edt_UPCSN.Text.Trim();
        //            string S_SN = Edt_SN.Text.Trim();

        //            string xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";
        //            string xmlPart = "<Part PartID=\"" + Com_Part.EditValue.ToString() + "\"> </Part>";
        //            string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + Com_PO.EditValue.ToString() + "\"> </ProdOrder>";
        //            string result = string.Empty;
        //            PartSelectSVC.uspCallProcedure("uspFGLinkUPCUPCSNCheck", UPCSN, xmlProdOrder, xmlPart,
        //                                                        xmlStation, "", null, ref result);
        //            if (result != "1")
        //            {
        //                if (IsAuto == "1")
        //                {
        //                    try
        //                    {
        //                        port1.WriteLine(FailCmd);
        //                        MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
        //                    }
        //                }

        //                string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
        //                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { UPCSN, ProMsg }, result);
        //                Edt_UPCSN.Text = string.Empty;
        //                Edt_UPCSN.Focus();
        //                return;
        //            }
        //            else
        //            {
        //                string S_StationTypeID = List_Login.StationTypeID.ToString();
        //                string S_PartID = Com_Part.EditValue.ToString();
        //                string PartFamilyID = Com_PartFamily.EditValue.ToString();
        //                string S_POID = Com_PO.EditValue.ToString();
        //                string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID,
        //                    List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());
        //                if (string.IsNullOrEmpty(S_UnitStateID))
        //                {
        //                    MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
        //                    Edt_UPCSN.Text = string.Empty;
        //                    Edt_UPCSN.Focus();
        //                    return;
        //                }
        //                DataTable DT_FGUnitID = PartSelectSVC.Get_UnitID(S_SN).Tables[0];
        //                DataTable DT_UPCUnitID = PartSelectSVC.Get_UnitID(UPCSN).Tables[0];
        //                string FGUnitID = DT_FGUnitID.Rows[0]["UnitID"].ToString();
        //                string UPCUitID = DT_UPCUnitID.Rows[0]["UnitID"].ToString();

        //                mesUnit v_mesUnit = new mesUnit();
        //                v_mesUnit.ID = Convert.ToInt32(FGUnitID);
        //                v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
        //                v_mesUnit.StatusID = 1;
        //                v_mesUnit.StationID = List_Login.StationID;
        //                v_mesUnit.EmployeeID = List_Login.EmployeeID;
        //                v_mesUnit.LastUpdate = DateTime.Now;
        //                v_mesUnit.ProductionOrderID = Convert.ToInt32(S_POID);
        //                //修改 Unit
        //                mesUnitSVC.Update(v_mesUnit);
        //                v_mesUnit.ID = Convert.ToInt32(UPCUitID);
        //                mesUnitSVC.Update(v_mesUnit);

        //                mesHistory v_mesHistory = new mesHistory();
        //                v_mesHistory.UnitID = Convert.ToInt32(FGUnitID);
        //                v_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
        //                v_mesHistory.EmployeeID = List_Login.EmployeeID;
        //                v_mesHistory.StationID = List_Login.StationID;
        //                v_mesHistory.EnterTime = DateTime.Now;
        //                v_mesHistory.ExitTime = DateTime.Now;
        //                v_mesHistory.ProductionOrderID = Convert.ToInt32(S_POID);
        //                v_mesHistory.PartID = Convert.ToInt32(S_PartID);
        //                v_mesHistory.LooperCount = 1;
        //                mesHistorySVC.Insert(v_mesHistory);
        //                v_mesHistory.UnitID = Convert.ToInt32(UPCUitID);
        //                mesHistorySVC.Insert(v_mesHistory);

        //                PartSelectSVC.MESModifyUnitDetail(Convert.ToInt32(FGUnitID), "KitSerialNumber", UPCSN);
        //                UPCSNSubmitEvent(UPCSN, S_SN);
        //                if (IsAuto == "1")
        //                {
        //                    try
        //                    {
        //                        port1.WriteLine(PassCmd);
        //                        MessageInfo.Add_Info_MSG(Edt_MSG, "60101", "OK", List_Login.Language);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        MessageInfo.Add_Info_MSG(Edt_MSG, "60105", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
        //                    }
        //                }

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            if (IsAuto == "1")
        //            {
        //                port1.WriteLine(FailCmd);
        //                MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
        //            }

        //            MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
        //        }
        //    }
        //}
        private void Edt_UPCSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    string UPCSN = Edt_UPCSN.Text.Trim();
                    string S_SN = Edt_SN.Text.Trim();

                    string xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";
                    string xmlPart = "<Part PartID=\"" + Com_Part.EditValue.ToString() + "\"> </Part>";
                    string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + Com_PO.EditValue.ToString() + "\"> </ProdOrder>";
                    string result = string.Empty;
                    PartSelectSVC.uspCallProcedure("uspFGLinkUPCUPCSNCheck", UPCSN, xmlProdOrder, xmlPart,
                                                                xmlStation, "", null, ref result);
                    if (result != "1")
                    {
                        if (IsAuto == "1")
                        {
                            try
                            {
                                port1.WriteLine(FailCmd);
                                MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
                            }
                            catch (Exception ex)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                            }
                        }

                        string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { UPCSN, ProMsg }, result);

                        Edt_UPCSN.Text = string.Empty;
                        Edt_UPCSN.Focus();

                        return;
                    }
                    else
                    {
                        if (IsScanFGSN)
                        {
                            Edt_UPCSN.Enabled = false; 
                            Edt_CheckFGSN.Enabled = true;
                        }
                        else
                        {
                            SacnSubmit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (IsAuto == "1")
                    {
                        port1.WriteLine(FailCmd);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                    }

                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                }
            }
        }

        private void Edt_CheckFGSN_KeyDown(object sender, KeyEventArgs e)
        {
            string S_UPCSN = Edt_UPCSN.Text.Trim();
            string S_CheckFGSN = Edt_CheckFGSN.Text.Trim();

            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(S_UPCSN) && !string.IsNullOrEmpty(S_CheckFGSN))
            {
                string S_Result = string.Empty;
                DataSet ds = PartSelectSVC.uspCallProcedure("uspKitBoxFGSNCheck", S_UPCSN, xmlProdOrder, xmlPart,
                                                                xmlStation, xmlExtraData, S_CheckFGSN, ref S_Result);
                if (S_Result == "1")
                {
                    Edt_CheckFGSN.Text = "";
                    Edt_CheckFGSN.Enabled = false;
                    Edt_SN.Enabled = true;
                    Edt_SN.Focus();

                    SacnSubmit();
                }
                else
                {
                    string ProMsg = MessageInfo.GetMsgByCode(S_Result, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language,
                        new string[] { S_UPCSN + "  " + S_CheckFGSN, ProMsg }, S_Result);

                    Edt_UPCSN.Text = "";
                    Edt_UPCSN.Enabled = true;
                    Edt_UPCSN.Focus();

                    Edt_CheckFGSN.Text = "";

                    if (IsAuto == "1")
                    {
                        port1.WriteLine(FailCmd);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language);
                    }                    
                }
            }
        }

        private void SacnSubmit()
        {
            try
            {
                string UPCSN = Edt_UPCSN.Text.Trim();
                string S_SN = Edt_SN.Text.Trim();

                string S_StationTypeID = List_Login.StationTypeID.ToString();
                string S_PartID = Com_Part.EditValue.ToString();
                string PartFamilyID = Com_PartFamily.EditValue.ToString();
                string S_POID = Com_PO.EditValue.ToString();
                string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID,
                    List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());
                if (string.IsNullOrEmpty(S_UnitStateID))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
                    Edt_UPCSN.Text = string.Empty;
                    Edt_UPCSN.Focus();
                    return;
                }
                DataTable DT_FGUnitID = PartSelectSVC.Get_UnitID(S_SN).Tables[0];
                DataTable DT_UPCUnitID = PartSelectSVC.Get_UnitID(UPCSN).Tables[0];
                string FGUnitID = DT_FGUnitID.Rows[0]["UnitID"].ToString();
                string UPCUitID = DT_UPCUnitID.Rows[0]["UnitID"].ToString();

                mesUnit v_mesUnit = new mesUnit();
                v_mesUnit.ID = Convert.ToInt32(FGUnitID);
                v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                v_mesUnit.StatusID = 1;
                v_mesUnit.StationID = List_Login.StationID;
                v_mesUnit.EmployeeID = List_Login.EmployeeID;
                v_mesUnit.LastUpdate = DateTime.Now;
                v_mesUnit.ProductionOrderID = Convert.ToInt32(S_POID);
                //修改 Unit
                mesUnitSVC.Update(v_mesUnit);
                v_mesUnit.ID = Convert.ToInt32(UPCUitID);
                mesUnitSVC.Update(v_mesUnit);

                mesHistory v_mesHistory = new mesHistory();
                v_mesHistory.UnitID = Convert.ToInt32(FGUnitID);
                v_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                v_mesHistory.EmployeeID = List_Login.EmployeeID;
                v_mesHistory.StationID = List_Login.StationID;
                v_mesHistory.EnterTime = DateTime.Now;
                v_mesHistory.ExitTime = DateTime.Now;
                v_mesHistory.ProductionOrderID = Convert.ToInt32(S_POID);
                v_mesHistory.PartID = Convert.ToInt32(S_PartID);
                v_mesHistory.LooperCount = 1;
                mesHistorySVC.Insert(v_mesHistory);
                v_mesHistory.UnitID = Convert.ToInt32(UPCUitID);
                mesHistorySVC.Insert(v_mesHistory);

                PartSelectSVC.MESModifyUnitDetail(Convert.ToInt32(FGUnitID), "KitSerialNumber", UPCSN);
                UPCSNSubmitEvent(UPCSN, S_SN);
                if (IsAuto == "1")
                {
                    try
                    {
                        port1.WriteLine(PassCmd);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "60101", "OK", List_Login.Language);
                    }
                    catch (Exception ex)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "60105", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                if (IsAuto == "1")
                {
                    port1.WriteLine(FailCmd);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                }

                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }



        private void btnReset_Click(object sender, EventArgs e)
        {
            Edt_SN.Text = string.Empty;
            Edt_SN.Enabled = true;
            Edt_UPCSN.Text = string.Empty;
            Edt_UPCSN.Enabled = false;
            txtUPC.Enabled = false;
            txtJan.Enabled = false;
            txtUPC.Text = string.Empty;
            txtJan.Text = string.Empty;
            Edt_SN.Focus();
        }

        private void txtUPC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string EnterCode = txtUPC.Text.Trim();
                if (EnterCode == lblUPCCode.Text.ToString())
                {
                    if (IsAuto == "1")
                    {
                        try
                        {
                            port1.WriteLine(PassCmd);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60101", "OK", List_Login.Language);
                        }
                        catch (Exception ex)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60105", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                            txtUPC.SelectAll();
                            return;
                        }
                    }

                    txtUPC.Enabled = false;
                    if (IsScanJAN && string.IsNullOrEmpty(txtJan.Text.Trim()))
                    {
                        txtJan.Enabled = true;
                        txtJan.Text = string.Empty;
                        txtJan.Focus();
                    }
                    else
                    {
                        Edt_UPCSN.Enabled = true;
                        Edt_UPCSN.Text = string.Empty;
                        Edt_UPCSN.Focus();
                    }

             
                }
                else if (string.IsNullOrEmpty(txtJan.Text.Trim()) && IsScanJAN && EnterCode == lblJANCode.Text.ToString())
                {
                    txtJan.Text = EnterCode;
                    txtJan.Enabled = false;
                    txtUPC.Text = string.Empty;
                    txtUPC.Focus();
                }
                else
                {
                    if (IsAuto == "1")
                    {
                        try
                        {
                            port1.WriteLine(FailCmd);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
                        }
                        catch (Exception ex)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                        }
                    }

                    MessageInfo.Add_Info_MSG(Edt_MSG, "20050", "NG", List_Login.Language);
                    txtUPC.Focus();
                    txtUPC.Text = "";
                }

    
            }
        }

        private void txtJan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string EnterCode = txtJan.Text.Trim();
                if (EnterCode == lblJANCode.Text.ToString())
                {
                    if (IsAuto == "1")
                    {
                        try
                        {
                            port1.WriteLine(PassCmd);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60101", "OK", List_Login.Language);
                        }
                        catch (Exception ex)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60105", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                            txtJan.SelectAll();
                            return;
                        }
                    }

                    txtJan.Enabled = false;
                    if (IsScanUPC && string.IsNullOrEmpty(txtUPC.Text.Trim()))
                    {
                        txtUPC.Enabled = true;
                        txtUPC.Text = string.Empty;
                        txtUPC.Focus();
                    }
                    else
                    {
                        Edt_UPCSN.Enabled = true;
                        Edt_UPCSN.Text = string.Empty;
                        Edt_UPCSN.Focus();
                    }

                  
                }
                else if (string.IsNullOrEmpty(txtUPC.Text.Trim()) && IsScanUPC && EnterCode == txtUPC.Text.ToString())
                {
                    txtUPC.Text = EnterCode;
                    txtUPC.Enabled = false;
                    txtJan.Text = string.Empty;
                    txtJan.Focus();
                }
                else
                {
                    if (IsAuto == "1")
                    {
                        try
                        {
                            port1.WriteLine(FailCmd);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60102", "OK", List_Login.Language);
                        }
                        catch (Exception ex)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                        }
                    }
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20050", "NG", List_Login.Language);
                    txtJan.Focus();
                    txtJan.Text = "";
                }
            }
        }


    }
}