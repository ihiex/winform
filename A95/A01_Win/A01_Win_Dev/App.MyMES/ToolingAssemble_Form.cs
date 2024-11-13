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
using App.MyMES.mesMachineService;
using App.MyMES.mesPartService;
using App.Model;
using System.Text.RegularExpressions;

namespace App.MyMES
{
    public partial class ToolingAssemble_Form : Form
    {
        int I_POID = 0;
        string S_DefectTypeID = "";
        int OverStationNumber = 0;
        string MainBarCode = string.Empty;
        DataTable DT_ProductionOrder = new DataTable();

        Public_ public_ = new Public_();
        LoginList List_Login = new LoginList();

        PartSelectSVCClient PartSelectSVC;
        ImesUnitSVCClient mesUnitSVC;
        ImesHistorySVCClient mesHistorySVC;
        ImesUnitDefectSVCClient mesUnitDefectSVC;
        ImesUnitComponentSVCClient mesUnitComponentSVC;
        ImesProductionOrderSVCClient mesProductionOrderSVC;
        ImesMachineSVCClient mesMachineSVC;
        ImesPartSVCClient mesPartSVC;
        ImesSerialNumberSVCClient mesSerialNumberSVC;

        public ToolingAssemble_Form()
        {
            InitializeComponent();
        }

        private void Assemble_Form_Load(object sender, EventArgs e)
        {
            LoadControls();
            Com_PO.Enabled = true;
        }

        private void LoadControls()
        {
            try
            {
                PartSelectSVC = PartSelectFactory.CreateServerClient();
                mesUnitSVC = ImesUnitFactory.CreateServerClient();
                mesHistorySVC = ImesHistoryFactory.CreateServerClient();
                mesUnitDefectSVC = ImesUnitDefectFactory.CreateServerClient();
                mesUnitComponentSVC = ImesUnitComponentFactory.CreateServerClient();
                mesProductionOrderSVC = ImesProductionOrderFactory.CreateServerClient();
                mesMachineSVC = ImesMachineFactory.CreateServerClient();
                mesPartSVC = ImesPartFactory.CreateServerClient();
                mesSerialNumberSVC = ImesSerialNumberFactory.CreateServerClient();

                List_Login = this.Tag as LoginList;
                public_.AddLine(Com_Line, Grid_Line);

                string S_StationTypeID = List_Login.StationTypeID.ToString();
                string S_LineID = List_Login.LineID.ToString();
                public_.AddStation(Com_Station, S_LineID, Grid_Station);

                Com_Line.Text = S_LineID;
                Com_Station.Text = List_Login.StationID.ToString();

                public_.AddPartFamilyType(Com_PartFamilyType, Grid_PartFamilyType);
                string S_PartID = Com_Part.EditValue.ToString();
                public_.AddPOAll(Com_PO, S_PartID, List_Login.LineID.ToString(), Grid_PO);
            }
            catch
            { }

        }

        private void Com_PartFamilyType_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartFamilyTypeID = Com_PartFamilyType.EditValue.ToString();
                public_.AddPartFamily(Com_PartFamily, S_PartFamilyTypeID, Grid_PartFamily);

                S_DefectTypeID = public_.GetDefectTypeID(Com_PartFamilyType.Text.Trim());
            }
            catch
            { }
        }

        private void Com_PartFamily_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                public_.AddPart(Com_Part, S_PartFamilyID, Grid_Part);
            }
            catch
            { }
        }

        private void Com_Part_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_PartID = Com_Part.EditValue.ToString();
                public_.AddPOAll(Com_PO, S_PartID, List_Login.LineID.ToString(), Grid_PO);
            }
            catch
            { }
        }

        private void Com_PO_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_POID = Com_PO.EditValue.ToString();

                Get_DTPO(S_POID);
            }
            catch
            { }
        }

        private void Get_DTPO(string S_POID)
        {
            I_POID = Convert.ToInt32(S_POID);
            DT_ProductionOrder = PartSelectSVC.GetProductionOrder(S_POID).Tables[0];

            Edt_OrderQuantity.Text = DT_ProductionOrder.Rows[0]["OrderQuantity"].ToString();
        }

        private void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {

            if (Btn_ConfirmPO.Tag.ToString() == "0")
            {
                if (Com_PartFamilyType.EditValue == null || string.IsNullOrEmpty(Com_PartFamilyType.Text.ToString()))
                {
                    Public_.Add_Info_MSG(Edt_MSG, "未选择料号类别，请确认！", "NG");
                    return;
                }
                if (Com_PartFamily.EditValue == null || string.IsNullOrEmpty(Com_PartFamily.Text.ToString()))
                {
                    Public_.Add_Info_MSG(Edt_MSG, "未选择料号群，请确认！", "NG");
                    return;
                }
                if (Com_Part.EditValue == null || string.IsNullOrEmpty(Com_Part.Text.ToString()))
                {
                    Public_.Add_Info_MSG(Edt_MSG, "未选择料号，请确认！", "NG");
                    return;
                }
                if (Com_PO.EditValue == null || string.IsNullOrEmpty(Com_PO.Text.ToString()))
                {
                    Public_.Add_Info_MSG(Edt_MSG, "未选择工单，请确认！", "NG");
                    return;
                }

                DataSet ds = PartSelectSVC.MESGetBomPartInfo(Convert.ToInt32(Com_Part.EditValue.ToString()),
                                List_Login.StationTypeID);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count < 2)
                {
                    Public_.Add_Info_MSG(Edt_MSG, string.Format("料号:{0}未配置BOM信息,无法进行组装！",
                        Com_Part.Text.ToString()), "NG");
                    return;
                }
                CreateControl(ds.Tables[0]);
                PartEdtStatus(false);
                Btn_ConfirmPO.Text = "Unlock(解锁)";
                Btn_ConfirmPO.Tag = 1;
            }
            else
            {
                Btn_ConfirmPO.Text = "Confirm(确定)";
                Btn_ConfirmPO.Tag = 0;
                MoveControlForPanel();
                PartEdtStatus(true);
            }
        }

        /// <summary>
        /// 移除生成控件
        /// </summary>
        private void MoveControlForPanel()
        {
            for (int i = panelSpool.Controls.Count; i > 0; i--)
            {
                panelSpool.Controls.Remove(panelSpool.Controls[i - 1]);
            }
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

            if (B_Status == true)
            {
                Com_PartFamilyType.BackColor = Color.White;
                Com_PartFamily.BackColor = Color.White;
                Com_Part.BackColor = Color.White;
                Com_PO.BackColor = Color.White;
            }
            else
            {
                Com_PartFamilyType.BackColor = Color.Yellow;
                Com_PartFamily.BackColor = Color.Yellow;
                Com_Part.BackColor = Color.Yellow;
                Com_PO.BackColor = Color.Yellow;
            }
        }

        /// <summary>
        /// 根据查询数据自动加载控件 
        /// </summary>
        private void CreateControl(DataTable dt)
        {
            int lblstartX = 35;
            int lblstartY = 35;
            int txtstartX = 280;
            int txtBtnstartY = 26;
            int btnStartX = 845;
            int interval = 65;
            Label lblBatch = null;
            TextBox txtBatch = null;

            if (dt.Rows.Count > 3)
                interval = 40;

            for (int j = 1; j <= dt.Rows.Count; j++)
            {

                string ScanType = dt.Rows[j - 1]["ScanType"].ToString();
                if (string.IsNullOrEmpty(ScanType))
                {
                    Public_.Add_Info_MSG(Edt_MSG, string.Format("料号:{0}未配置扫描类型！",
                        dt.Rows[j - 1]["PartNumber"].ToString()), "NG");
                    return;
                }

                string Pattern = dt.Rows[j - 1]["Pattern"].ToString();
                if (string.IsNullOrEmpty(Pattern))
                {
                    Public_.Add_Info_MSG(Edt_MSG, string.Format("料号:{0}未配置校验批次正则表达式！",
                        dt.Rows[j - 1]["PartNumber"].ToString()), "NG");
                    return;
                }

                string scanType = dt.Rows[j - 1]["ScanType"].ToString();
                if (scanType != "1" && scanType != "2" && scanType != "3" && scanType != "4" && scanType != "5")
                {
                    Public_.Add_Info_MSG(Edt_MSG, string.Format("料号:{0}配置未能识别的扫描类型:{1}！",
                       dt.Rows[j - 1]["PartNumber"].ToString(), scanType), "NG");
                    return;
                }

                Label labels = new Label();
                labels.AutoSize = true;
                labels.Text = dt.Rows[j - 1]["PartNumber"].ToString();
                labels.Name = "lbl" + j.ToString();
                labels.Location = new System.Drawing.Point(lblstartX, lblstartY + (j - 1) * interval);

                TextBox txts = new TextBox();
                txts.Name = "txt" + j.ToString();
                string[] TagPro = new string[]
                    {
                        dt.Rows[j - 1]["PartID"].ToString(),             //料号
                        scanType,                                        //扫描类型
                        dt.Rows[j - 1]["Pattern"].ToString(),            //校验正则
                        dt.Rows[j - 1]["FieldName"].ToString(),          //存储字段
                        "0"                                              //是否锁定
                    };
                txts.Tag = TagPro;

                //托盘、箱子（生成虚拟SN类型,显示绑定的批次号）
                if (ScanType == "3")
                {
                    //增加批次号
                    lblBatch = new Label();
                    lblBatch.AutoSize = true;
                    lblBatch.Text = "Batch(批次号):";
                    lblBatch.Name = "lbc" + j.ToString();
                    lblBatch.Location = new System.Drawing.Point(560, lblstartY + (j - 1) * interval);

                    txtBatch = new TextBox();
                    txtBatch.Name = "tbc" + j.ToString();
                    txtBatch.Size = new System.Drawing.Size(175, 26);
                    txtBatch.Location = new System.Drawing.Point(658, txtBtnstartY + (j - 1) * interval);
                    txtBatch.Enabled = false;
                    if (j == 1)
                    {
                        txtBatch.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
                    }
                    else
                    {
                        txtBatch.Font = new System.Drawing.Font("宋体", 13F);
                    }
                }


                txts.Location = new System.Drawing.Point(txtstartX, txtBtnstartY + (j - 1) * interval);
                txts.KeyDown += Txts_KeyDown;
                txts.Enabled = true;

                Button btns = new Button();
                btns.Text = "Lock(锁定)";
                btns.Name = "btn" + j.ToString();
                btns.Tag = 1;
                btns.Size = new System.Drawing.Size(97, 30);
                btns.Location = new System.Drawing.Point(btnStartX, txtBtnstartY + (j - 1) * interval);
                btns.Click += Btns_Click;

                if (j == 1)
                {
                    labels.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
                    txts.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);

                    if (ScanType == "3")
                        txts.Size = new System.Drawing.Size(270, 30);
                    else
                        txts.Size = new System.Drawing.Size(548, 30);
                }
                else
                {
                    labels.Font = new System.Drawing.Font("宋体", 10F);
                    txts.Font = new System.Drawing.Font("宋体", 13F);
                    if (ScanType == "3")
                        txts.Size = new System.Drawing.Size(270, 26);
                    else
                        txts.Size = new System.Drawing.Size(548, 28);
                }

                panelSpool.Controls.Add(labels);
                panelSpool.Controls.Add(txts);
                if(scanType=="3")
                {
                    panelSpool.Controls.Add(lblBatch);
                    panelSpool.Controls.Add(txtBatch);
                }
                panelSpool.Controls.Add(btns);

                //防止控件叠加
                int lblWidth = labels.Width + lblstartX;
                if (lblWidth > txtstartX)
                {
                    txts.Location = new System.Drawing.Point(lblWidth + 3, txtBtnstartY + (j - 1) * interval);
                    txts.Width = txts.Width - (lblWidth - txtstartX) - 3;
                }
                if (j == 1)
                {
                    txts.Focus();
                }
            }
        }

        /// <summary>
        /// 查找批次号
        /// </summary>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        private bool SearchBatch(string ControlName)
        {
            string batch = string.Empty;
            string ControlNumber = ControlName.Replace("txt", "");
            foreach (Control ctr in panelSpool.Controls)
            {
                if (ctr.Name == ControlName)
                {
                    PartSelectSVC.BoxSnToBatch(ctr.Text.Trim(), out batch);
                    if (string.IsNullOrEmpty(batch) || batch.Substring(0,5)=="ERROR")
                    {
                        return false;
                    }
                }
                else if (ctr.Name == "tbc" + ControlNumber)
                {
                    ctr.Text = batch;
                }
            }
            return true;
        }

        /// <summary>
        /// 手动回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Txts_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox txtPub = (TextBox)sender;

            if (e.KeyCode == Keys.Enter && txtPub.Enabled)
            {
                string btnName = txtPub.Name.Substring(3, txtPub.Name.Length - 3);
                bool IsChooseCotrol = false;
                bool Check = CheckInputBarcode(txtPub);

                if (Check && txtPub.Enabled )
                {
                    txtPub.Enabled = false;
                    IsChooseCotrol = true;
                    if (CheckControlInput())
                    {
                        if (!SubmitData())
                        {
                            txtPub.Text = "";
                            txtPub.Enabled = true;
                            return;
                        }
                    }
                }
                else
                {
                    txtPub.Text = "";
                    txtPub.Enabled = true;
                }

                foreach (Control ctr in panelSpool.Controls)
                {
                    //光标定位
                    if (IsChooseCotrol && ctr.Name == "txt" + (Convert.ToInt32(btnName) + 1).ToString())
                    {
                        ctr.Focus();
                    }
                }
            }
        }

        /// <summary>
        /// 锁定/解除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btns_Click(object sender, EventArgs e)
        {
            Button btnPub = sender as Button;
            bool IsChooseCotrol = false;

            string btnName = btnPub.Name.Substring(3, btnPub.Name.Length - 3);
            foreach (Control ctr in panelSpool.Controls)
            {
                if (ctr.Name == "txt" + btnName)
                {
                    if (btnPub.Tag.ToString() == "1")
                    {
                        if (!ctr.Enabled || CheckInputBarcode(ctr as TextBox))
                        {
                            ctr.Enabled = false;
                            btnPub.Text = "Unlock(解除)";
                            btnPub.Tag = 0;
                            IsChooseCotrol = true;
                            (ctr.Tag as string[])[4] = "1";
                            if (CheckControlInput())
                                SubmitData();
                        }
                        else
                        {
                            ctr.Text = "";
                            ctr.Focus();
                        }
                    }
                    else
                    {
                        ctr.Enabled = true;
                        ctr.Text = "";
                        ctr.Focus();
                        btnPub.Text = "Lock(锁定)";
                        btnPub.Tag = 1;
                        (ctr.Tag as string[])[4] = "0";
                    }
                }
            }

            foreach (Control ctr in panelSpool.Controls)
            {
                //光标定位
                if (IsChooseCotrol && ctr.Name == "txt" + (Convert.ToInt32(btnName) + 1).ToString())
                {
                    ctr.Focus();
                }
            }
        }

        /// <summary>
        /// TextBox输入框验证
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="pre">正则验证</param>
        /// <returns></returns>
        private bool CheckInputBarcode(TextBox txtBox)
        {
            try
            {
                string barcode = txtBox.Text.Trim();
                string[] strList = txtBox.Tag as string[];
                string Pattern = strList[2].ToString();
                string ScanType = strList[1].ToString();
                string PartID = strList[0].ToString();
                string Result = "1";

                //空值检查
                if (string.IsNullOrEmpty(barcode))
                {
                    return false;
                }
                    //正则校验
                if (!Regex.IsMatch(barcode, Pattern.Replace("\\\\", "\\")))
                {
                    Public_.Add_Info_MSG(Edt_MSG, string.Format("SN:{0} 正则校验未通过，请确认！", barcode), "NG");
                    txtBox.SelectAll();
                    return false;
                }

                if (txtBox.Name.Replace("txt", "") == "1")
                {
                    //Tooling校验
                    string xmlPartStr = "<Part PartID=\"" + PartID + "\"> </Part>";
                    PartSelectSVC.uspCallProcedure("uspMachineToolingCheck", barcode,
                                                                            null, xmlPartStr, null, null, List_Login.ApplicationTypeID.ToString(), ref Result);
                    if (Result != "1")
                    {
                        Public_.Add_Info_MSG(Edt_MSG, Result, "NG");
                        txtBox.Text = "";
                        return false;
                    }

                    //Tooling 转换BarCode
                    MainBarCode = PartSelectSVC.BuckToFGSN(barcode);
                    Result = PartSelectSVC.MESAssembleCheckMianSN(Com_PO.EditValue.ToString(), List_Login.LineID,
                                        List_Login.StationID, List_Login.StationTypeID, MainBarCode);
                }
                else
                {


                    switch (ScanType)
                    {
                        case "1":
                            Result = PartSelectSVC.MESAssembleCheckOtherSN(barcode, PartID);
                            break;
                        case "2":
                            break;
                        case "3":
                        case "4":
                            //Machine校验
                            string StationTypeID = List_Login.StationTypeID.ToString();
                            string xmlPartStr = "<Part PartID=\"" + PartID + "\"> </Part>";

                            PartSelectSVC.uspCallProcedure("uspMachineToolingCheck", barcode,
                                                                                    null, xmlPartStr, null, null, StationTypeID, ref Result);
                            if (ScanType == "3" && Result == "1")
                            {
                                if (!SearchBatch(txtBox.Name))
                                {
                                    Public_.Add_Info_MSG(Edt_MSG, "SN:{0}未找到关联的批次号.", "NG");
                                    txtBox.Text = "";
                                    return false;
                                }

                                //虚拟绑定批次的直接锁定  --删除此处需要手动锁定
                                foreach (Control ctl in panelSpool.Controls)
                                {
                                    if (ctl.Name == "btn" + txtBox.Name.Replace("txt", ""))
                                    {
                                        ctl.Text = "Unlock(解除)";
                                        ctl.Tag = 0;
                                        (txtBox.Tag as string[])[4] = "1";
                                    }
                                }
                            }
                            break;
                        case "5":
                            //校验是否存在重复条码
                            bool check = mesUnitComponentSVC.MESCheckChildSerialNumber(barcode);
                            if (!check)
                            {
                                Public_.Add_Info_MSG(Edt_MSG, string.Format("SN:{0}存在重复关联数据.", barcode), "NG");
                                txtBox.Text = "";
                                return false;
                            }
                            break;
                        default:
                            break;
                    }
                }

                if (Result != "1")
                {
                    Public_.Add_Info_MSG(Edt_MSG, Result, "NG");
                    txtBox.Text = "";
                    return false;
                }

                //调用通用过程
                string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + Com_PO.EditValue + "\"> </ProdOrder>";
                string xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                string outString = string.Empty;
                PartSelectSVC.uspCallProcedure("uspAssembleCheck", barcode,
                                                                        xmlProdOrder, xmlPart, xmlStation, null, PartID, ref outString);
                if (outString != "1")
                {
                    Public_.Add_Info_MSG(Edt_MSG, outString.ToString(), "NG");
                    txtBox.Text = "";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Public_.Add_Info_MSG(Edt_MSG, string.Format("程序异常:{0}", ex.Message.ToString()), "NG");
                return false;
            }
        }


        public bool CheckControlInput()
        {
            //校验是否全部验证完成
            foreach (Control ctr in panelSpool.Controls)
            {
                if (ctr.Name.Substring(0, 3) == "txt")
                {
                    if (ctr.Enabled)
                    {
                        ctr.Focus();
                        return false;
                    }
                }
            }

            //Machine类型必须重新校验(不放上面防止重复校验)
            foreach (Control ctr in panelSpool.Controls)
            {
                if (ctr.Name.Substring(0, 3) == "txt" && (ctr.Tag as string[])[4].ToString() == "1")
                {
                    if (!CheckInputBarcode((ctr as TextBox)))
                    {
                        ctr.Text = "";
                        ctr.Enabled = true;
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 数据提交
        /// </summary>
        public bool SubmitData()
        {
            DateTime dateStart = DateTime.Now;
            bool IsFouce = false;

            mesUnit v_mesUnit = new mesUnit();
            string msg = string.Empty;
            string S_POID = Com_PO.EditValue.ToString();
            string S_PartID = Com_Part.EditValue.ToString();
            string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
            string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, List_Login.StationTypeID, List_Login.LineID.ToString());


            foreach (Control ctr in panelSpool.Controls)
            {
                if (ctr.Name.Substring(0, 3) == "txt")
                {
                    TextBox txt = ctr as TextBox;
                    string[] tagList = ctr.Tag as string[];
                    string Batch = "";
                    string S_SN = txt.Text.Trim();

                    #region 循环控件数据提交
                    if (ctr.Name.Replace("txt", "") == "1")
                    {
                        DataTable DT_UPUnitID = PartSelectSVC.Get_UnitID(MainBarCode).Tables[0];
                        v_mesUnit.ID = Convert.ToInt32(DT_UPUnitID.Rows[0]["UnitID"].ToString());
                        v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                        v_mesUnit.StatusID = 1;
                        v_mesUnit.StationID = List_Login.StationID;
                        v_mesUnit.EmployeeID = List_Login.EmployeeID;
                        v_mesUnit.LastUpdate = DateTime.Now;
                        v_mesUnit.ProductionOrderID = Convert.ToInt32(S_POID);
                        //修改 Unit
                        string S_UpdateUnit = mesUnitSVC.Update(v_mesUnit);
                        if (S_UpdateUnit.Substring(0, 1) == "E")
                        {
                            Public_.Add_Info_MSG(Edt_MSG, S_UpdateUnit, "NG");
                            return false;
                        }

                        mesHistory v_mesHistory = new mesHistory();
                        string S_POPartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();

                        v_mesHistory.UnitID = Convert.ToInt32(v_mesUnit.ID);
                        v_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                        v_mesHistory.EmployeeID = List_Login.EmployeeID;
                        v_mesHistory.StationID = List_Login.StationID;
                        v_mesHistory.ProductionOrderID = Convert.ToInt32(S_POID);
                        v_mesHistory.PartID = Convert.ToInt32(S_POPartID);
                        v_mesHistory.LooperCount = 1;
                        v_mesHistory.EnterTime = DateTime.Now;
                        int I_InsertHistory = mesHistorySVC.Insert(v_mesHistory);
                        msg = "主条码:" + txt.Text.ToString();
                    }
                    else
                    {
                        mesUnitComponent v_mesUnitComponent = new mesUnitComponent();
                        
                        if (tagList[1].ToString() == "2" || tagList[1].ToString()=="5")
                        {
                            v_mesUnitComponent.UnitID = v_mesUnit.ID;
                            v_mesUnitComponent.UnitComponentTypeID = 1;
                            v_mesUnitComponent.ChildUnitID = 0;
                            if (tagList[1].ToString() == "2")
                            {
                                v_mesUnitComponent.ChildSerialNumber = "";
                                v_mesUnitComponent.ChildLotNumber = txt.Text.Trim();
                            }
                            else
                            {
                                v_mesUnitComponent.ChildSerialNumber = txt.Text.Trim();
                                v_mesUnitComponent.ChildLotNumber = "";
                            }
                            int partID = Convert.ToInt32(tagList[0].ToString());
                            mesPart part = mesPartSVC.Get(partID);
                            v_mesUnitComponent.ChildPartID = partID;
                            v_mesUnitComponent.ChildPartFamilyID = part.PartFamilyID;
                            v_mesUnitComponent.Position = "";
                            v_mesUnitComponent.InsertedEmployeeID = List_Login.EmployeeID;
                            v_mesUnitComponent.InsertedStationID = List_Login.StationID;
                            v_mesUnitComponent.StatusID = 1;
                        }
                        else
                        {
                            int ChildUnitID;

                            if (tagList[1].ToString() == "3")
                            {
                                //找到虚拟条码UnitID
                                DataSet dataSetMachine = PartSelectSVC.BoxSnToBatch(S_SN, out Batch);
                                if (dataSetMachine == null || dataSetMachine.Tables.Count == 0 || dataSetMachine.Tables[0].Rows.Count==0)
                                {
                                    Public_.Add_Info_MSG(Edt_MSG, "SN:{0}未找到该条码对应的虚拟条码", "NG");
                                    return false;
                                }
                                ChildUnitID =Convert.ToInt32(dataSetMachine.Tables[0].Rows[0]["UnitID"].ToString());
                            }
                            else if (tagList[1].ToString() == "4")
                            {
                                string Result = VirtualBarCode(S_SN, tagList[0].ToString(), null, S_UnitStateID, S_POID);
                                if (Result == "NG" || string.IsNullOrEmpty(Result))
                                {
                                    return false;
                                }
                                ChildUnitID = Convert.ToInt32(Result);
                            }
                            else
                            {
                                DataSet dataSetUnit = PartSelectSVC.GetmesSerialNumber(txt.Text.Trim());
                                if (dataSetUnit == null || dataSetUnit.Tables.Count == 0 || dataSetUnit.Tables[0].Rows.Count == 0)
                                {
                                    Public_.Add_Info_MSG(Edt_MSG, "SN:{0}未找到该条码对应UnitID", "NG");
                                    return false;
                                }
                                ChildUnitID = Convert.ToInt32(dataSetUnit.Tables[0].Rows[0]["UnitID"].ToString());
                            }

                            DataSet dsComponent = PartSelectSVC.GetComponent(ChildUnitID);
                            if (dsComponent == null || dsComponent.Tables.Count == 0 || dsComponent.Tables[0].Rows.Count == 0)
                            {
                                Public_.Add_Info_MSG(Edt_MSG, "SN:{0} 提交错误-GetComponent", "NG");
                                return false;
                            }

                            string S_ChildSerialNumber = dsComponent.Tables[0].Rows[0]["Value"].ToString();
                            int ChildPartFamilyID = Convert.ToInt32(dsComponent.Tables[0].Rows[0]["PartFamilyID"].ToString());
                            
                            v_mesUnitComponent.UnitID = v_mesUnit.ID;
                            v_mesUnitComponent.UnitComponentTypeID = 1;
                            v_mesUnitComponent.ChildUnitID = Convert.ToInt32(ChildUnitID);
                            v_mesUnitComponent.ChildSerialNumber = S_ChildSerialNumber;
                            v_mesUnitComponent.ChildLotNumber = Batch;
                            v_mesUnitComponent.ChildPartID = Convert.ToInt32(tagList[0].ToString());
                            v_mesUnitComponent.ChildPartFamilyID = ChildPartFamilyID;
                            v_mesUnitComponent.Position = "";
                            v_mesUnitComponent.InsertedEmployeeID = List_Login.EmployeeID;
                            v_mesUnitComponent.InsertedStationID = List_Login.StationID;
                            v_mesUnitComponent.StatusID = 1;
                        }

                        string S_UC_Result = mesUnitComponentSVC.Insert(v_mesUnitComponent);
                        if (S_UC_Result != "OK")
                        {
                            Public_.Add_Info_MSG(Edt_MSG, S_UC_Result, "NG");
                            return false;
                        }
                        msg = msg + " 子条码:" + txt.Text.ToString();
                    }

                    if (tagList[1].ToString() == "3" || tagList[1].ToString() == "4")
                    {
                        mesMachineSVC.MesModMachineBySNStationTypeID(S_SN, List_Login.StationTypeID);
                    }
                    #endregion

                    //不锁定的清空
                    if (tagList[4].ToString()!="1")
                    {
                        txt.Enabled = true;
                        txt.Text = string.Empty;
                        if(!IsFouce)
                        {
                            txt.Focus();
                            IsFouce = true;
                        }
                    }
                }
            }
            TimeSpan ts = DateTime.Now - dateStart;
            double mill = Math.Round(ts.TotalMilliseconds, 0);
            Public_.Add_Info_MSG(Edt_MSG, msg + "组装完成！"+ mill.ToString()+"毫秒", "OK");
            OverStationNumber++;
            Edt_ScanOKQuantity.Text = OverStationNumber.ToString();
            return true;
        }

        private string VirtualBarCode(string S_SN,string S_PartID,string S_PartFamilyID, string S_UnitStateID,string S_POID)
        {
            try
            {
                string S_InsertUnit = string.Empty;
                string S_xmlPart = "'<BoxSN SN=" + "\"" + S_SN + "\"" + "> </BoxSN>'";
                string S_FormatSN = PartSelectSVC.mesGetSNFormatIDByList(S_PartID, S_PartFamilyID, null);
                if (string.IsNullOrEmpty(S_FormatSN))
                {
                    Public_.Add_Info_MSG(Edt_MSG, string.Format("料号:{0}未配置打印格式", S_PartID), "NG");
                    return "NG";
                }
                DataSet DS = PartSelectSVC.uspSNRGetNext(S_FormatSN, 0, null, S_xmlPart, null, null, null);
                string New_SN = DS.Tables[2].Rows[0][0].ToString();
                mesUnit v_mesUnit = new mesUnit();
                v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                v_mesUnit.StatusID = 1;
                v_mesUnit.StationID = List_Login.StationID;
                v_mesUnit.EmployeeID = List_Login.EmployeeID;
                v_mesUnit.CreationTime = DateTime.Now;
                v_mesUnit.LastUpdate = DateTime.Now;
                v_mesUnit.PanelID = 0;
                v_mesUnit.LineID = List_Login.LineID;
                v_mesUnit.ProductionOrderID = Convert.ToInt32(S_POID);
                v_mesUnit.RMAID = 0;
                v_mesUnit.PartID = Convert.ToInt32(S_PartID);
                v_mesUnit.LooperCount = 1;
                S_InsertUnit = mesUnitSVC.Insert(v_mesUnit);

                mesSerialNumber v_mesSerialNumber = new mesSerialNumber();
                v_mesSerialNumber.UnitID = Convert.ToInt32(S_InsertUnit);
                v_mesSerialNumber.SerialNumberTypeID = 8;
                v_mesSerialNumber.Value = New_SN;
                mesSerialNumberSVC.Insert(v_mesSerialNumber);
                return S_InsertUnit;
            }
            catch(Exception ex)
            {
                Public_.Add_Info_MSG(Edt_MSG, string.Format("程序异常:{0}", ex.Message.ToString()), "NG");
                return "NG";
            }
        }

        private void Assemble_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                PartSelectSVC.Close();
                mesUnitSVC.Close();
                mesHistorySVC.Close();
                mesUnitDefectSVC.Close();
                mesUnitComponentSVC.Close();
                mesProductionOrderSVC.Close();
                mesMachineSVC.Close();
                mesPartSVC.Close();
                mesSerialNumberSVC.Close();
            }
            catch
            { }
        }

        private void btnRest_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确认要重置当前数量吗?", "提示",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }
            Edt_ScanOKQuantity.Text = "0";
            OverStationNumber = 0;
        }
    }
}
