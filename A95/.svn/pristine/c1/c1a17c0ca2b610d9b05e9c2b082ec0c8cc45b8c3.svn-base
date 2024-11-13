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
using App.MyMES.mesPackageService;
using System.Text.RegularExpressions;

namespace App.MyMES
{
    public partial class BoxPackage_Form : Form
    {
        Public_ public_ = new Public_();
        DataTable DT_ProductionOrder = new DataTable();
        DataTable DT_PackageData = null;
        bool IsGenerateBoxSN = false;                       //是否需要自动生成箱号条码
        bool isPacking = false;                             //是否包装完成
        string BoxSNFormatName;
        string BoxLabelTemplatePath;
        string BoxSN_Pattern;
        string BoxSN;
        string xmlProdOrder;
        string xmlPart;
        string xmlExtraData;
        string xmlStation;

        int Allnumber;
        LoginList List_Login = new LoginList();
        PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient(); 
        LabelManager2.Application LabSN;

        int I_RouteSequence = -9;  //当前工序顺序 
        int I_POID = 0;

        public BoxPackage_Form()
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
            btnLastBox.Enabled = false;

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

            //if (IsFirstSequence()==false)
            //{                
            //    Com_PO.Enabled = false;
            //    Com_PO.BackColor = Color.Yellow;

            //    Edt_SN.Enabled = true;
            //    Btn_Refresh.Enabled = false;
            //    Btn_ConfirmPO.Enabled = false;

            //    Panel_Part.Visible = false;               
            //}           
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
                try
                {
                    if (!Regex.IsMatch(Edt_SN.Text.Trim(), BoxSN_Pattern))
                    {
                        Public_.Add_Info_MSG(Edt_MSG, "箱号SN不符合格式,请核对后再试。", "NG");
                        return;
                    }
                    string result = PartSelectSVC.uspKitBoxCheck(Edt_SN.Text.Trim(), xmlProdOrder, xmlPart, xmlStation, xmlExtraData, "BOX");

                    if (result != "1")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, result, "NG");
                        Edt_SN.SelectAll();
                        return;
                    }
                    else
                    {
                        isPacking = false;
                        BoxSN = Edt_SN.Text.Trim();
                        //BoxSN= BoxSN.Substring(BoxSN.IndexOf("SSCC"));
                        //BoxSN = "00" + BoxSN.Substring(4, BoxSN.IndexOf(',') - 4);

                        DataSet ds = PartSelectSVC.Get_PackageData(BoxSN);
                        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        {
                            DT_PackageData = new DataTable();
                            DT_PackageData.Columns.Add("SEQNO", typeof(int));
                            DT_PackageData.Columns.Add("UPCSN", typeof(string));
                            DT_PackageData.Columns.Add("TIME", typeof(string));
                            txtNowNumber.Text = "0";
                        }
                        else
                        {
                            DT_PackageData = ds.Tables[0];
                            txtNowNumber.Text = ds.Tables[0].Rows.Count.ToString();

                            //判断是否已经包装完成
                            DataTable DT_Package = PartSelectSVC.GetMesPackageStatusID(BoxSN).Tables[0];

                            if (DT_Package != null && DT_Package.Rows.Count > 0 && Convert.ToInt32(DT_Package.Rows[0]["StatusID"]) == 1)
                            {
                                isPacking = true;
                            }
                        }
                        if (isPacking)
                        {
                            Edt_SN.SelectAll();
                            Edt_ChildSN.Enabled = false;
                            btnLastBox.Enabled = false;
                            Public_.Add_Info_MSG(Edt_MSG, string.Format("箱号:{0}已经完成包装。", BoxSN), "OK");
                        }
                        else
                        {
                            Edt_SN.Enabled = false;
                            Edt_ChildSN.Enabled = true;
                            Edt_ChildSN.Focus();
                            if (DT_PackageData.Rows.Count > 0)
                            {
                                btnLastBox.Enabled = true;
                            }
                            Public_.Add_Info_MSG(Edt_MSG, "", "OK");
                        }
                        dataGridSN.DataSource = DT_PackageData;
                    }
                }catch(Exception ex)
                {
                    Public_.Add_Info_MSG(Edt_MSG, string.Format("ERROR:{0}。", ex.Message.ToString()), "NG");
                    return;
                }
            }
        }

        private void Edt_ChildSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(Edt_ChildSN.Text.Trim()))
            {
                int NowNumber = string.IsNullOrEmpty(txtNowNumber.Text.Trim()) ? 0 : Convert.ToInt32(txtNowNumber.Text.Trim());
                if (Allnumber <= NowNumber)
                {
                    Public_.Add_Info_MSG(Edt_MSG, "包装数量不能超出规格数量!", "NG");
                    return;
                }

                string result = PartSelectSVC.uspKitBoxCheck(Edt_ChildSN.Text.Trim(), xmlProdOrder, xmlPart, xmlStation, xmlExtraData, "SN");
                if (result != "1")
                {
                    Public_.Add_Info_MSG(Edt_MSG, result, "NG");
                    Edt_ChildSN.SelectAll();
                    return;
                }
                else
                {
                    //校验工艺路线
                    string S_SN = Edt_ChildSN.Text.Trim();
                    string S_Sql = @"select * from mesUnit A inner join mesUnitDetail b on a.ID=b.UnitID
	                                    where b.KitSerialNumber='" + S_SN + "'";
                    DataTable DT_Unit = public_.P_DataSet(S_Sql).Tables[0];

                    if (DT_Unit.Rows[0]["StatusID"].ToString() != "1")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, S_SN + " 此条码已NG！", "NG");
                        Edt_SN.Focus();
                        Edt_SN.Text = "";
                        return;
                    }

                    string S_RouteCheck = PartSelectSVC.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, S_SN);
                    if (S_RouteCheck != "1")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, S_RouteCheck, "NG");
                        Edt_ChildSN.SelectAll();
                        return;
                    }

                    //生成箱号SN
                    if (IsGenerateBoxSN && string.IsNullOrEmpty(BoxSN))
                    {
                        mesUnit mesUnit = new mesUnit();
                        mesUnit.StationID = List_Login.StationID;
                        mesUnit.EmployeeID = List_Login.EmployeeID;
                        mesUnit.ProductionOrderID = Convert.ToInt32(Com_PO.EditValue);
                        mesUnit.PartID = Convert.ToInt32(Com_Part.EditValue);
                        result = PartSelectSVC.Get_CreatePackageSN(BoxSNFormatName, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, mesUnit, ref BoxSN, 1);
                        if (result != "1")
                        {
                            Public_.Add_Info_MSG(Edt_MSG, result, "NG");
                            Edt_ChildSN.SelectAll();
                            return;
                        }
                    }

                    int Reuse = (DT_PackageData == null ? 0 : DT_PackageData.Rows.Count) + 1;
                    result = PartSelectSVC.uspKitBoxPackaging(Com_Part.EditValue.ToString(),Com_PO.EditValue.ToString(),
                                                                Edt_ChildSN.Text.Trim(), BoxSN,List_Login,0);
                    if (result == "1")
                    {
                        Edt_ChildSN.SelectAll();
                        btnLastBox.Enabled = true;
                        txtNowNumber.Text = Reuse.ToString();
                        DataRow dr = DT_PackageData.NewRow();
                        dr["SEQNO"] = Reuse.ToString();
                        dr["UPCSN"] = Edt_ChildSN.Text.Trim();
                        dr["TIME"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        DT_PackageData.Rows.Add(dr);
                        Edt_MSG.Text = string.Empty;

                        if (Reuse == Allnumber)
                        {
                            result = PartSelectSVC.uspKitBoxPackaging(Com_Part.EditValue.ToString(), Com_PO.EditValue.ToString(), 
                                                                        Edt_ChildSN.Text.Trim(), BoxSN, List_Login, Reuse);
                            if (result == "1")
                            {
                                if (IsGenerateBoxSN)
                                {
                                    string PrintResult = PrintCodeSoftSN();
                                    if(PrintResult!= "OK")
                                    {
                                        Public_.Add_Info_MSG(Edt_MSG, "箱号:" + BoxSN + "包装完成,打印失败." + PrintResult, "NG");
                                    }
                                    else
                                    {
                                        Public_.Add_Info_MSG(Edt_MSG, "箱号:" + BoxSN + "包装完成并发送至打印机!", "OK");
                                    }
                                    btnLastBox.Enabled = false;
                                    Edt_ChildSN.Text = string.Empty;
                                    Edt_ChildSN.Focus();
                                }
                                else
                                {
                                    Edt_SN.Text = string.Empty; 
                                    Edt_ChildSN.Text = string.Empty;
                                    Edt_SN.Enabled = true;
                                    Edt_ChildSN.Enabled = false;
                                    btnLastBox.Enabled = false;
                                    Edt_SN.Focus();
                                    Public_.Add_Info_MSG(Edt_MSG, "箱号:" + BoxSN + "包装完成!", "OK");
                                }

                                //参数初始化
                                isPacking = true;
                                txtNowNumber.Text = string.Empty;
                                BoxSN = string.Empty;
                                DT_PackageData.Clear();
                            }
                            else
                            {
                                Public_.Add_Info_MSG(Edt_MSG, "箱号:" + BoxSN + "包装失败。" + result, "NG");
                                return;
                            }
                        }
                        else
                        {
                            Public_.Add_Info_MSG(Edt_MSG,string.Format("UPC SN:{0}关联箱号:{1}成功", Edt_ChildSN.Text.Trim(), BoxSN), "OK");
                            Edt_ChildSN.Text = string.Empty;
                            Edt_ChildSN.Focus();
                        }
                    }
                    else
                    {
                        Public_.Add_Info_MSG(Edt_MSG, result, "NG");
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

                LabSN.Documents.Open(BoxLabelTemplatePath, false);
                doc = LabSN.ActiveDocument;
            }
            catch (Exception ex)
            {
                S_Result = ex.Message;
            }

            try
            {
                doc.Variables.Item("SN").Value = BoxSN;
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
                Public_.Add_Info_MSG(Edt_MSG, "工单不能为空!", "NG");
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
                        Public_.Add_Info_MSG(Edt_MSG, "工单未配置包装所需参数,请检查！", "NG");
                        return;
                    }
                    DataTable dtPara = dtSet.Tables[0];
                    DataRow[] drUpc = dtPara.Select("Description='UPC'");
                    
                    if (drUpc.Count()==0)
                    {
                        Public_.Add_Info_MSG(Edt_MSG, "工单未配置UPC参数,请检查！", "NG");
                        return;
                    }

                    DataRow[] drMpn = dtPara.Select("Description='MPN'");

                    if (drMpn.Count() == 0)
                    {
                        Public_.Add_Info_MSG(Edt_MSG, "工单未配置MPN参数,请检查！", "NG");
                        return;
                    }

                    DataRow[] drboxFullNumber = dtPara.Select("Description='BoxQty'");
                    if (drboxFullNumber.Count()==0)
                    {
                        Public_.Add_Info_MSG(Edt_MSG, "工单未配置整箱数量(BoxQty)参数,请检查！", "NG");
                        return;
                    }

                    DataRow[] drBoxSN_Pattern = dtPara.Select("Description='BoxSN_Pattern'");
                    if (drBoxSN_Pattern.Count() == 0)
                    {
                        Public_.Add_Info_MSG(Edt_MSG, "工单未配置箱号SN正则表达式(BoxSN_Pattern)参数,请检查！", "NG");
                        return;
                    }

                    txtMpn.Text = drMpn[0]["Content"].ToString();
                    txtUpc.Text = drUpc[0]["Content"].ToString();
                    BoxSN_Pattern = drBoxSN_Pattern[0]["Content"].ToString().Replace("\\\\", "\\");
                    Allnumber = Convert.ToInt32(drboxFullNumber[0]["Content"].ToString());
                    txtNumber.Text = drboxFullNumber[0]["Content"].ToString();
                    txtNowNumber.Text = "0";
                    PartEdtStatus(false);

                    DataRow[] drGenerateBoxSN = dtPara.Select("Description='IsGenerateBoxSN'");
                    xmlProdOrder = ("<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>");
                    xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                    xmlExtraData = "<ExtraData EmployeeId=\""+ List_Login.EmployeeID.ToString() + "\"> </ExtraData>";
                    xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";

                    //初始化参数
                    DT_PackageData = null;
                    BoxSN = string.Empty;
                    Edt_MSG.Text = string.Empty;

                    if (drGenerateBoxSN.Count() == 0 || drGenerateBoxSN[0]["Content"].ToString()=="0")
                    {
                        Edt_SN.Text = string.Empty;
                        Edt_SN.Enabled = true;
                        Edt_ChildSN.Text = string.Empty;
                        Edt_ChildSN.Enabled = false;
                        IsGenerateBoxSN = false;
                        Edt_SN.Focus();
                    }
                    else
                    {
                        DataRow[] drBoxSNFormatName = dtPara.Select("Description='BoxSNFormatName'");
                        if (drBoxSNFormatName.Count() == 0)
                        {
                            Public_.Add_Info_MSG(Edt_MSG, "工单未配置箱号条码打印格式(BoxSNFormatName)参数,请检查！", "NG");
                            return;
                        }
                        
                        DataRow[] drBoxLabelTemplatePath = dtPara.Select("Description='BoxLabelTemplatePath'");
                        if (drBoxLabelTemplatePath.Count() == 0)
                        {
                            Public_.Add_Info_MSG(Edt_MSG, "工单未配置箱号条码打印文件路径(BoxLabelTemplatePath)参数,请检查！", "NG");
                            return;
                        }

                        BoxSNFormatName = drBoxSNFormatName[0]["Content"].ToString();
                        BoxLabelTemplatePath = drBoxLabelTemplatePath[0]["Content"].ToString();


                        if (DT_PackageData == null)
                        {
                            DT_PackageData = new DataTable();
                            DT_PackageData.Columns.Add("SEQNO", typeof(int));
                            DT_PackageData.Columns.Add("UPCSN", typeof(string));
                            DT_PackageData.Columns.Add("TIME", typeof(string));
                            dataGridSN.DataSource = DT_PackageData;
                        }

                        Edt_SN.Text = string.Empty;
                        Edt_SN.Enabled = false;
                        Edt_ChildSN.Text = string.Empty;
                        Edt_ChildSN.Enabled = true;
                        Edt_ChildSN.Focus();
                        IsGenerateBoxSN = true;
                    }
                }
                catch (Exception ex)
                {
                    Public_.Add_Info_MSG(Edt_MSG, "参数加载异常,错误信息:" + ex.Message.ToString(), "NG");
                    return;
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
                if (MessageBox.Show("中箱包装还有未包装完成的数据，确定是否需要解锁?", "提示",
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

        private void btnLastBox_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Edt_SN.Text.Trim()) || DT_PackageData.Rows.Count == 0)
            {
                return;
            }
            if (MessageBox.Show("当前包装的数量可能未达到设定规格数量，确认是否继续?", "提示",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }
            string result = PartSelectSVC.uspKitBoxPackaging(Com_Part.EditValue.ToString(),Com_PO.EditValue.ToString(),
                                                                Edt_ChildSN.Text.Trim(),BoxSN, List_Login, DT_PackageData.Rows.Count);
            if (result == "1")
            {
                Public_.Add_Info_MSG(Edt_MSG, "箱号:" + BoxSN + "尾箱包装完成!", "OK");
                Edt_SN.Text = "";
                Edt_ChildSN.Text = "";
                Edt_SN.Enabled = true;
                Edt_ChildSN.Enabled = false;
                btnLastBox.Enabled = false;
                Edt_SN.Focus();
            }
            else
            {
                Public_.Add_Info_MSG(Edt_MSG, "箱号:" + BoxSN + "尾箱包装失败。" + result, "NG");
                return;
            }
        }
    }
}
