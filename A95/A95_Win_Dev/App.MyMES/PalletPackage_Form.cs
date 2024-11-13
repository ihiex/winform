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
using App.MyMES.PartSelectService;
using App.MyMES.mesUnitService;
using App.MyMES.mesSerialNumberService;
using App.MyMES.mesHistoryService;
using App.MyMES.mesProductionOrderService;
using App.MyMES.mesProductStructureService;
using App.MyMES.mesUnitComponentService;
using App.MyMES.mesUnitDefectService;
using System.Text.RegularExpressions;
using App.MyMES.mesPackageService;

namespace App.MyMES
{
    public partial class PalletPackage_Form : Form
    {
        Public_ public_ = new Public_();
        DataTable DT_ProductionOrder = new DataTable();
        DataTable DT_PackageData = null;
        string PalletSN;
        bool isPacking = false;
        bool IsGeneratePalletSN = false;
        LabelManager2.Application LabSN;
        string PalletSNFormatName;
        string PalletLabelTemplatePath;
        string PalletSN_Pattern;
        string xmlProdOrder;
        string xmlPart;
        string xmlExtraData;
        string xmlStation;

        int Allnumber;
        LoginList List_Login = new LoginList();
        PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();

        int I_RouteSequence = -9;  //当前工序顺序 
        int I_POID = 0;

        public PalletPackage_Form()
        {
            InitializeComponent();
        }

        private void Btn_Refresh_Click(object sender, EventArgs e)
        {            
            List_Login = this.Tag as LoginList;
            public_.AddLine(Com_Line,Grid_Line);

            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_LineID = List_Login.LineID.ToString();
            public_.AddStation(Com_Station,S_LineID,Grid_Station);

            Com_Line.Text = S_LineID;
            Com_Station.Text = List_Login.StationID.ToString();

            public_.AddPartFamilyType(Com_PartFamilyType,Grid_PartFamilyType);

            string S_PartID = Com_Part.EditValue.ToString(); 
            public_.AddPOAll(Com_PO, S_PartID, List_Login.LineID.ToString(), Grid_PO);


            Edt_SN.Focus();
            Edt_SN.SelectAll(); 

            //Edt_MSG.Text = ""; 
            Edt_SN.Text = "";
            Edt_SN.Enabled = true;

            Edt_ChildSN.Text = "";
            Edt_ChildSN.Enabled = false;

            PartEdtStatus(true);

        }

        /// <summary>
        /// Part  编辑框状态
        /// </summary>
        /// <param name="B_Status"></param>
        private void PartEdtStatus(Boolean B_Status)
        {
            Com_PartFamilyType.Enabled = B_Status;
            Com_PartFamily.Enabled = B_Status;
            Com_Part.Enabled = B_Status;
            Com_Part.Enabled = B_Status;
            Com_PO.Enabled = B_Status;
            Btn_ConfirmPO.Enabled = B_Status;
            btnClosePallet.Enabled = false;

            if (B_Status == true)
            {
                Com_PartFamilyType.BackColor = Color.White;
                Com_PartFamily.BackColor = Color.White;
                Com_Part.BackColor = Color.White;
                Com_PO.BackColor = Color.White;
                txtUpc.Text = "";
                txtMpn.Text = "";
                txtNowNumber.Text = "";
                txtNumber.Text = "";

                Edt_SN.Enabled = false;
                Edt_SN.Text = "";
                Edt_ChildSN.Text = "";
                Edt_ChildSN.Enabled = false;
            }
            else
            {
                Com_PartFamilyType.BackColor = Color.Yellow; 
                Com_PartFamily.BackColor = Color.Yellow; 
                Com_Part.BackColor = Color.Yellow; 
                Com_PO.BackColor = Color.Yellow;

                Edt_SN.Enabled = true;
            }

            Edt_SN.Focus();
            Edt_SN.SelectAll();
        }
        /// <summary>
        /// 第一站 扫描顺序号
        /// </summary>
        /// <param name="PartID"></param>
        /// <param name="StationTypeID"></param>
        /// <returns></returns>
        private int FirstScanSequence(string PartID, int StationTypeID)
        {
            int I_Result = 1;
            if (public_.IsOneStationPrint(PartID, StationTypeID, List_Login.LineID.ToString()))
            {
                I_Result = 2;
            }
            return I_Result;
        }


        private void BoxPackage_Form_Load(object sender, EventArgs e)
        {
            Btn_Refresh_Click(sender, e);

            Com_PO.Enabled = true;
         
        }

        private Boolean IsFirstSequence()
        {
            string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
            I_RouteSequence = public_.RouteSequence(S_PartID, List_Login.StationTypeID, List_Login.LineID.ToString());
            int I_FirstScanSequence = FirstScanSequence(S_PartID, List_Login.StationTypeID);

            Boolean B_Result = true;
            if (I_RouteSequence > I_FirstScanSequence)
            {
                B_Result = false; 
            }

            return B_Result;
        }

        private void Get_DTPO(string S_POID)
        {
            I_POID = Convert.ToInt32(S_POID);
            DT_ProductionOrder = PartSelectSVC.GetProductionOrder(S_POID).Tables[0];
            Edt_OrderQuantity.Text = DT_ProductionOrder.Rows[0]["OrderQuantity"].ToString();
            //GetScanQuantity();
        }



        //private void GetScanQuantity()
        //{
        //    string S_PartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();
        //    string S_Sql = "select COUNT(ID) CountUnit  from  mesUnit  where PartID=" + S_PartID +
        //                    "  and StationID=" + List_Login.StationID +
        //                    "  and ProductionOrderID=" + I_POID;
        //    DataTable DT_CountUnit = public_.P_DataSet(S_Sql).Tables[0];

        //    Edt_ScanQuantity.Text = DT_CountUnit.Rows[0]["CountUnit"].ToString();  
        //}

        private void Edt_ScanSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(Edt_SN.Text.Trim()))
            {
                if (!Regex.IsMatch(Edt_SN.Text.Trim(), PalletSN_Pattern))
                {
                    public_.Add_Info_MSG(Edt_MSG, "栈板SN["+ Edt_SN.Text.Trim() + "]不符合格式,请核对后再试。", "NG");
                    return;
                }
                string result = PartSelectSVC.uspPalletCheck(Edt_SN.Text.Trim(), xmlProdOrder, xmlPart, xmlStation, xmlExtraData, "PALLET");
                
                if (result != "1")
                {
                    public_.Add_Info_MSG(Edt_MSG, result, "NG");
                    Edt_SN.SelectAll();
                    return;
                }
                else
                {
                    isPacking = false;
                    PalletSN = Edt_SN.Text.Trim();
                    DataSet ds = PartSelectSVC.Get_PalletData(Edt_SN.Text.Trim());
                    if(ds==null || ds.Tables.Count==0 || ds.Tables[0].Rows.Count==0)
                    {
                        DT_PackageData = new DataTable();
                        DT_PackageData.Columns.Add("SEQNO", typeof(int));
                        DT_PackageData.Columns.Add("KITSN", typeof(string));
                        DT_PackageData.Columns.Add("TIME", typeof(string));
                        txtNowNumber.Text = "0";
                    }
                    else
                    {
                        DT_PackageData = ds.Tables[0];
                        txtNowNumber.Text = ds.Tables[0].Rows.Count.ToString();

                        //判断是否已经包装完成
                        DataTable DT_Package = PartSelectSVC.GetMesPackageStatusID(PalletSN).Tables[0];

                        if (DT_Package != null && DT_Package.Rows.Count > 0 && Convert.ToInt32(DT_Package.Rows[0]["StatusID"]) == 1)
                        {
                            isPacking = true;
                        }
                    }
                    if (isPacking)
                    {
                        Edt_SN.SelectAll();
                        Edt_ChildSN.Enabled = false;
                        btnClosePallet.Enabled = false;
                        public_.Add_Info_MSG(Edt_MSG, string.Format("栈板:{0}已经完成包装。", PalletSN), "OK");
                    }
                    else
                    {
                        Edt_SN.Enabled = false;
                        Edt_ChildSN.Enabled = true;
                        Edt_ChildSN.Focus();
                        if(DT_PackageData.Rows.Count>0)
                        {
                            btnClosePallet.Enabled = true;
                        }
                        public_.Add_Info_MSG(Edt_MSG, "", "OK");
                    }
                    dataGridSN.DataSource = DT_PackageData;
                }
            }
        }

        private void Edt_ChildSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(Edt_ChildSN.Text.Trim()))
            {
                int NowNumber = string.IsNullOrEmpty(txtNowNumber.Text.Trim()) ? 0 : Convert.ToInt32(txtNowNumber.Text.Trim());
                if(Allnumber<= NowNumber)
                {
                    public_.Add_Info_MSG(Edt_MSG, "包装数量不能超出规格数量!", "NG");
                    return;
                }

                string result = PartSelectSVC.uspPalletCheck(Edt_ChildSN.Text.Trim(), xmlProdOrder, xmlPart, xmlStation, xmlExtraData, "BOX");
                if (result != "1")
                {
                    public_.Add_Info_MSG(Edt_MSG, result, "NG");
                    Edt_ChildSN.SelectAll();
                    return;
                }
                else
                {
                    //生成栈板SN
                    if (IsGeneratePalletSN && string.IsNullOrEmpty(PalletSN))
                    {
                        mesUnit mesUnit = new mesUnit();
                        mesUnit.StationID = List_Login.StationID;
                        mesUnit.EmployeeID = List_Login.EmployeeID;
                        mesUnit.ProductionOrderID = Convert.ToInt32(Com_PO.EditValue);
                        mesUnit.PartID = Convert.ToInt32(Com_Part.EditValue);
                        result = PartSelectSVC.Get_CreatePackageSN(PalletSNFormatName, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, mesUnit, ref PalletSN, 2);
                        if (result != "1")
                        {
                            public_.Add_Info_MSG(Edt_MSG, result, "NG");
                            Edt_ChildSN.SelectAll();
                            return;
                        }
                    }

                    int Reuse = (DT_PackageData == null ? 0 : DT_PackageData.Rows.Count) + 1;
                    result = PartSelectSVC.uspPalletPackaging(Com_Part.EditValue.ToString(), 
                                Com_PO.EditValue.ToString(),Edt_ChildSN.Text.Trim(), PalletSN, List_Login, 0);
                    if (result == "1")
                    {
                        Edt_ChildSN.SelectAll();
                        btnClosePallet.Enabled = true;
                        txtNowNumber.Text = Reuse.ToString();
                        DataRow dr = DT_PackageData.NewRow();
                        dr["SEQNO"] = Reuse.ToString();
                        dr["KITSN"] = Edt_ChildSN.Text.Trim();
                        dr["TIME"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        DT_PackageData.Rows.Add(dr);
                        Edt_MSG.Text = string.Empty;

                        if (Reuse == Allnumber)
                        {
                            result = PartSelectSVC.uspPalletPackaging(Com_Part.EditValue.ToString(),
                                        Com_PO.EditValue.ToString(), Edt_ChildSN.Text.Trim(), PalletSN, List_Login, Reuse);
                            if (result == "1")
                            {
                                isPacking = true;
                                if (IsGeneratePalletSN)
                                {
                                    btnClosePallet.Enabled = false;
                                    Edt_ChildSN.Text = string.Empty;
                                    Edt_ChildSN.Focus();
                                    txtNowNumber.Text = string.Empty;
                                    string PrintResult = PrintCodeSoftSN();
                                    if (PrintResult != "OK")
                                    {
                                        public_.Add_Info_MSG(Edt_MSG, "栈板:" + PalletSN + ",包装完成,条码打印失败." + PrintResult, "NG");
                                        
                                    }
                                    else
                                    {
                                        public_.Add_Info_MSG(Edt_MSG, "栈板:" + PalletSN + "包装完成并发送至打印机!", "OK");
                                    }
                                }
                                else
                                {
                                    Edt_SN.Text = string.Empty;
                                    Edt_ChildSN.Text = string.Empty;
                                    Edt_SN.Enabled = true;
                                    Edt_ChildSN.Enabled = false;
                                    btnClosePallet.Enabled = false;
                                    Edt_SN.Focus();
                                    public_.Add_Info_MSG(Edt_MSG, "栈板:" + PalletSN + "包装完成!", "OK");
                                }

                                //参数初始化
                                isPacking = true;
                                txtNowNumber.Text = string.Empty;
                                PalletSN = string.Empty;
                                DT_PackageData.Clear();
                            }
                            else
                            {
                                public_.Add_Info_MSG(Edt_MSG, "栈板:" + PalletSN + "包装失败。" + result, "NG");
                                return;
                            }
                        }
                        else
                        {
                            public_.Add_Info_MSG(Edt_MSG, string.Format("箱号:{0}关联栈板:{1}成功", Edt_ChildSN.Text.Trim(), PalletSN), "OK");
                            Edt_ChildSN.Text = string.Empty;
                            Edt_ChildSN.Focus();
                        }
                    }
                    else
                    {
                        public_.Add_Info_MSG(Edt_MSG, result, "NG");
                        Edt_ChildSN.SelectAll();
                        return;
                    }
                }
            }
        }

        private string PrintCodeSoftSN()
        {
            string S_Result = "OK";

            LabelManager2.Document doc = null;
            try
            {
                if (LabSN == null)
                {
                    LabSN = new LabelManager2.Application();
                }

                LabSN.Documents.Open(PalletLabelTemplatePath, false);
                doc = LabSN.ActiveDocument;
            }
            catch (Exception ex)
            {
                S_Result = ex.Message;
            }

            try
            {
                doc.Variables.Item("SN").Value = PalletSN;
                doc.PrintLabel(1);
                doc.FormFeed();
                doc.Close();
            }
            catch (Exception ex)
            {
                if (doc != null)
                    doc.Close();
                S_Result = ex.ToString();
            }
            finally
            {
                GC.Collect(0);
            }

            return S_Result;
        }

        private void BoxPackage_Form_Resize(object sender, EventArgs e)
        {
            try
            {
                foreach (Form frm in Panel_Defact.Controls)
                {
                    frm.Width = Panel_Defact.Width;
                    frm.Height = Panel_Defact.Height;
                }
            }
            catch { }
        }

        private void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            if (Com_PO.Text.Trim() == "")
            {
                public_.Add_Info_MSG(Edt_MSG, "工单不能为空!", "NG");
                return;
            }
            else
            {
                PartSelectSVCClient partClint = PartSelectFactory.CreateServerClient();
                try
                {
                    DataSet dtSet = partClint.GetProductionOrderDetailDef(Com_PO.EditValue.ToString());
                    if (dtSet == null || dtSet.Tables.Count == 0)
                    {
                        public_.Add_Info_MSG(Edt_MSG, "工单未配置包装所需参数,请检查！", "NG");
                        return;
                    }
                    DataTable dtPara = dtSet.Tables[0];
                    DataRow[] drUpc = dtPara.Select("Description='UPC'");//[0]["Content"].ToString();
                    
                    if (drUpc.Count()==0)
                    {
                        public_.Add_Info_MSG(Edt_MSG, "工单未配置UPC参数,请检查！", "NG");
                        return;
                    }

                    DataRow[] drMpn = dtPara.Select("Description='MPN'");

                    if (drMpn.Count() == 0)
                    {
                        public_.Add_Info_MSG(Edt_MSG, "工单未配置MPN参数,请检查！", "NG");
                        return;
                    }

                    DataRow[] drboxFullNumber = dtPara.Select("Description='PalletQty'");
                    if (drboxFullNumber.Count()==0)
                    {
                        public_.Add_Info_MSG(Edt_MSG, "工单未配置栈板最大箱数(PalletQty)参数,请检查！", "NG");
                        return;
                    }

                    DataRow[] drPalletSN_Pattern = dtPara.Select("Description='PalletSN_Pattern'");
                    if (drPalletSN_Pattern.Count() == 0)
                    {
                        public_.Add_Info_MSG(Edt_MSG, "工单未配置栈板SN正则表达式(PalletSN_Pattern)参数,请检查！", "NG");
                        return;
                    }
                    txtMpn.Text = drMpn[0]["Content"].ToString();
                    txtUpc.Text = drUpc[0]["Content"].ToString();
                    PalletSN_Pattern = drPalletSN_Pattern[0]["Content"].ToString().Replace("\\\\", "\\");
                    Allnumber = Convert.ToInt32(drboxFullNumber[0]["Content"].ToString());
                    txtNumber.Text = drboxFullNumber[0]["Content"].ToString();
                    txtNowNumber.Text = "0";
                    PartEdtStatus(false);

                    DataRow[] drGeneratePalletSN = dtPara.Select("Description='IsGeneratePalletSN'");
                    xmlProdOrder = ("<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>");
                    xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                    xmlExtraData = "<ExtraData EmployeeId=\"" + List_Login.EmployeeID.ToString() + "\"> </ExtraData>";
                    xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";

                    //初始化参数
                    DT_PackageData = null;
                    PalletSN = string.Empty;
                    Edt_MSG.Text = string.Empty;

                    if (drGeneratePalletSN.Count() == 0 || drGeneratePalletSN[0]["Content"].ToString() == "0")
                    {
                        Edt_SN.Text = string.Empty;
                        Edt_SN.Enabled = true;
                        Edt_SN.Focus();
                        Edt_ChildSN.Text = string.Empty;
                        Edt_ChildSN.Enabled = false;
                        IsGeneratePalletSN = false;
                    }
                    else
                    {
                        DataRow[] drPalletSNFormatName = dtPara.Select("Description='PalletSNFormatName'");
                        if (drPalletSNFormatName.Count() == 0)
                        {
                            public_.Add_Info_MSG(Edt_MSG, "工单未配置栈板条码打印格式(PalletSNFormatName)参数,请检查！", "NG");
                            return;
                        }
                        
                        DataRow[] drPalletLabelTemplatePath = dtPara.Select("Description='PalletLabelTemplatePath'");
                        if (drPalletLabelTemplatePath.Count() == 0)
                        {
                            public_.Add_Info_MSG(Edt_MSG, "工单未配置栈板条码打印文件路径(PalletLabelTemplatePath)参数,请检查！", "NG");
                            return;
                        }
                        PalletSNFormatName = drPalletSNFormatName[0]["Content"].ToString();
                        PalletLabelTemplatePath = drPalletLabelTemplatePath[0]["Content"].ToString();

                        if (DT_PackageData == null)
                        {
                            DT_PackageData = new DataTable();
                            DT_PackageData.Columns.Add("SEQNO", typeof(int));
                            DT_PackageData.Columns.Add("KITSN", typeof(string));
                            DT_PackageData.Columns.Add("TIME", typeof(string));
                            dataGridSN.DataSource = DT_PackageData;
                        }

                        Edt_SN.Text = string.Empty;
                        Edt_SN.Enabled = false;
                        Edt_ChildSN.Text = string.Empty;
                        Edt_ChildSN.Enabled = true;
                        Edt_ChildSN.Focus();
                        IsGeneratePalletSN = true;
                    }
                }
                catch (Exception ex)
                {
                    public_.Add_Info_MSG(Edt_MSG, "参数加载异常,错误信息:" + ex.Message.ToString(), "NG");
                }
                finally
                {
                    partClint.Close();
                }
            }
        }

        private void Btn_Unlock_Click(object sender, EventArgs e)
        {
            if (DT_PackageData != null && DT_PackageData.Rows.Count > 0 && !isPacking)
            {
                if (MessageBox.Show("栈板包装还有未包装完成的数据，确定是否需要解锁?", "提示",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }
            PartEdtStatus(true);
        }

        private void Com_PartFamilyType_EditValueChanged(object sender, EventArgs e)
        {
            string S_PartFamilyTypeID = Com_PartFamilyType.EditValue.ToString();
            public_.AddPartFamily(Com_PartFamily, S_PartFamilyTypeID, Grid_PartFamily);
        }

        private void Com_PartFamily_EditValueChanged(object sender, EventArgs e)
        {
            string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
            public_.AddPart(Com_Part, S_PartFamilyID, Grid_Part);
        }

        private void Com_Part_EditValueChanged(object sender, EventArgs e)
        {
            string S_PartID = Com_Part.EditValue.ToString();
            public_.AddPOAll(Com_PO, S_PartID, List_Login.LineID.ToString(), Grid_PO);
        }

        private void Com_PO_EditValueChanged(object sender, EventArgs e)
        {
            string S_POID = Com_PO.EditValue.ToString();

            Get_DTPO(S_POID);
        }

        private void btnClosePallet_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(Edt_SN.Text.Trim()) || DT_PackageData.Rows.Count==0)
            {
                return;
            }
            if (MessageBox.Show("当前包装的数量可能未达到设定规格数量，确认是否继续?", "提示",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }
            string result = PartSelectSVC.uspPalletPackaging(Com_Part.EditValue.ToString(), Com_PO.EditValue.ToString(), Edt_ChildSN.Text.Trim(),
                                                                PalletSN, List_Login, DT_PackageData.Rows.Count);
            if (result == "1")
            {
                public_.Add_Info_MSG(Edt_MSG, "栈板号:" + Edt_SN.Text.Trim() + "包装完成!", "OK");
                Edt_SN.Text = "";
                Edt_ChildSN.Text = "";
                Edt_SN.Enabled = true;
                Edt_ChildSN.Enabled = false;
                btnClosePallet.Enabled = false;
                Edt_SN.Focus();
            }
            else
            {
                public_.Add_Info_MSG(Edt_MSG, "栈板号:" + Edt_SN.Text.Trim() + "包装失败。" + result, "NG");
                return;
            }
        }
    }
}
