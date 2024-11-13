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
using System.Text.RegularExpressions;
using App.Model;

namespace App.MyMES
{
    public partial class AssemblyNoPo_Form : FrmBase
    {
        bool COF = false;
        string S_ProductionOrderID = "";
        string S_PartID = "";
        string S_PartFamilyID = "";
        string S_PartFamilyType = "";
        DataSet ds;

        public AssemblyNoPo_Form()
        {
            InitializeComponent();
        }

        public override void FrmBase_Load(object sender, EventArgs e)
        {
            base.FrmBase_Load(sender, e);
            base.Com_luUnitStatus.Enabled = false;

            //DataTable dt = new DataTable();
            //dt.Columns.Add("PartID", Type.GetType("System.String"));
            //dt.Columns.Add("PartNumber", Type.GetType("System.String"));
            //dt.Columns.Add("ScanType", Type.GetType("System.String"));
            //dt.Columns.Add("Pattern", Type.GetType("System.String"));
            //dt.Columns.Add("FieldName", Type.GetType("System.String"));
            //dt.Rows.Add();
            //DataRow dr = dt.NewRow();
            //dr["PartID"] = "短信";
            //dr["PartNumber"] = "SN";
            //dr["ScanType"] = "1";
            //dr["Pattern"] = "[\\s\\S]*";
            ////dr["FieldName"] = "短信";

            //dt.Rows.Add(dr);
            //CreateControl(dt, 1);
            base.GrpControlInputData.Enabled = true;
            //判断是否胶注册
            DataSet dst = PartSelectSVC.MESGetStationTypeParameter(List_Login.StationTypeID);
            DataRow[] drScanGlue = dst.Tables[0].Select("Description='IsScanAdhesive'");
            if (drScanGlue.Length > 0 && drScanGlue[0]["Content"].ToString() == "1")
            {
                PanelBatchConfirn.Visible = true;
            }
            else
            {
                PanelBatchConfirn.Visible = false;
            }

            int lblstartX = 35;
            int lblstartY = 35;
            int txtstartX = 280;
            int txtBtnstartY = 26;

            LabelControl labels = new LabelControl();
            labels.AutoSize = true;
            labels.Text = "SN";
            labels.Name = "lbl1";
            labels.Location = new System.Drawing.Point(lblstartX, lblstartY);

            TextEdit txts = new TextEdit();
            txts.Name = "txt1";

            txts.Location = new System.Drawing.Point(txtstartX, txtBtnstartY);
            txts.KeyDown += Txts_KeyDown;
            txts.Enabled = true;


            labels.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            txts.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            txts.Size = new System.Drawing.Size(548, 30);


            panelSpool.Controls.Add(labels);
            panelSpool.Controls.Add(txts);
        }

        /// <summary>
        /// 重写确认菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            base.Btn_ConfirmPO_Click(sender, e);
            if (string.IsNullOrEmpty(Com_PO.Text.ToString()))
                return;

            string PO = Com_PO.EditValue.ToString();
            DataSet dsCOF = PartSelectSVC.GetluPODetailDef(Convert.ToInt32(PO), "COF");
            if (dsCOF != null && dsCOF.Tables.Count > 0 && dsCOF.Tables[0].Rows.Count > 0)
            {
                COF = dsCOF.Tables[0].Rows[0]["Content"].ToString() == "1";
            }

            DataSet ds = PartSelectSVC.MESGetBomPartInfo(Convert.ToInt32(Com_Part.EditValue.ToString()),
                                List_Login.StationTypeID);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count < 2)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20023", "NG", List_Login.Language, new string[] { Com_Part.Text.ToString() });
                return;
            }

            //判断是否胶注册
            DataSet dst = PartSelectSVC.MESGetStationTypeParameter(List_Login.StationTypeID);
            DataRow[] drScanGlue = dst.Tables[0].Select("Description='IsScanAdhesive'");
            if (drScanGlue.Length > 0 && drScanGlue[0]["Content"].ToString() == "1")
            {
                PanelBatchConfirn.Visible = true;
            }
            else
            {
                PanelBatchConfirn.Visible = false;
            }

            CreateControl(ds.Tables[0],"");
        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            MoveControlForPanel();
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
        /// 根据查询数据自动加载控件 
        /// </summary>
        private void CreateControl(DataTable dt,string S_ProductionOrderID)
        {
            int lblstartX = 35;
            int lblstartY = 35;
            int txtstartX = 280;
            int txtBtnstartY = 26;
            int btnStartX = 845;
            int interval = 65;
            LabelControl lblBatch = null;
            TextEdit txtBatch = null;

            if (dt.Rows.Count > 3)
                interval = 40;

            for (int j = 2; j <= dt.Rows.Count; j++)
            {

                string ScanType = dt.Rows[j - 1]["ScanType"].ToString();
                if (string.IsNullOrEmpty(ScanType))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20024", "NG", List_Login.Language, new string[] { dt.Rows[j - 1]["PartNumber"].ToString() });
                    return;
                }

                string Pattern = dt.Rows[j - 1]["Pattern"].ToString();
                if (string.IsNullOrEmpty(Pattern))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20025", "NG", List_Login.Language, new string[] { dt.Rows[j - 1]["PartNumber"].ToString() });
                    return;
                }

                string scanType = dt.Rows[j - 1]["ScanType"].ToString();
                if (!(new string[] { "1", "2", "3", "4", "5", "6" ,"7" }).Contains(scanType))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20026", "NG", List_Login.Language, new string[] { dt.Rows[j - 1]["PartNumber"].ToString(), scanType });
                    return;
                }

                LabelControl labels = new LabelControl();
                labels.AutoSize = true;
                labels.Text = dt.Rows[j - 1]["PartNumber"].ToString();
                labels.Name = "lbl" + j.ToString();
                labels.Location = new System.Drawing.Point(lblstartX, lblstartY + (j - 1) * interval);

                TextEdit txts = new TextEdit();
                txts.Name = "txt" + j.ToString();
                string[] TagPro = new string[]
                    {
                        dt.Rows[j - 1]["PartID"].ToString(),             //料号
                        scanType,                                        //扫描类型
                        dt.Rows[j - 1]["Pattern"].ToString(),            //校验正则
                        dt.Rows[j - 1]["FieldName"].ToString(),          //存储字段
                        "0" ,                                             //是否锁定
                        S_ProductionOrderID
                    };
                txts.Tag = TagPro;

                //托盘、箱子（生成虚拟SN类型,显示绑定的批次号）
                if (ScanType == "3")
                {
                    //增加批次号
                    lblBatch =  new LabelControl();
                    lblBatch.AutoSize = true;
                    lblBatch.Text = "Batch(批次号):";
                    lblBatch.Name = "lbc" + j.ToString();
                    lblBatch.Location = new System.Drawing.Point(560, lblstartY + (j - 1) * interval);

                    txtBatch = new TextEdit();
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

                SimpleButton btns = null;
                if (ScanType != "1")
                {
                    btns = new SimpleButton();
                    btns.Text = "Lock(锁定)";
                    btns.Name = "btn" + j.ToString();
                    btns.Tag = 1;
                    btns.Size = new System.Drawing.Size(97, 30);
                    btns.Location = new System.Drawing.Point(btnStartX, txtBtnstartY + (j - 1) * interval);
                    btns.Click += Btns_Click;
                }

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
                if (scanType == "3" || scanType == "6")
                {
                    //新增释放治具按钮
                    SimpleButton btnRelease = new SimpleButton();
                    btnRelease.Text = "Release(释放)";
                    btnRelease.Name = "Rel" + j.ToString();
                    btnRelease.Size = new System.Drawing.Size(97, 30);
                    btnRelease.Location = new System.Drawing.Point(btnStartX + btns.Width + 5, txtBtnstartY + (j - 1) * interval);
                    btnRelease.Click += BtnRelease_Click;

                    panelSpool.Controls.Add(lblBatch);
                    panelSpool.Controls.Add(txtBatch);
                    panelSpool.Controls.Add(btnRelease);
                }
                if (btns != null)
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
        /// 释放治具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRelease_Click(object sender, EventArgs e)
        {
            SimpleButton btnPub = sender as SimpleButton;
            string btnName = btnPub.Name.Substring(3, btnPub.Name.Length - 3);
            foreach (Control ctr in panelSpool.Controls)
            {
                if (ctr.Name == "txt" + btnName)
                {
                    if (ctr.Enabled || string.IsNullOrEmpty(ctr.Text.Trim()))
                        return;
                    string scanType = (ctr.Tag as string[])[1].ToString();
                    if (scanType == "3"|| scanType == "6")
                    {
                        if (MessageInfo.Add_Info_MessageBox("10022", List_Login.Language))
                        {
                            string result=mesMachineSVC.MesToolingReleaseCheck(ctr.Text.Trim(), List_Login.StationTypeID.ToString());
                            if (result != "1")
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, result, "NG", List_Login.Language);
                                return;
                            }
                            PartSelectSVC.ModMachine(ctr.Text.Trim(), "1", true);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "10023", "OK", List_Login.Language);
                            ctr.Text = string.Empty;
                            ctr.Enabled = true;
                            ctr.Focus();
                        }
                    }
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
                    //if (string.IsNullOrEmpty(batch) || (batch.Length>=5 && batch.Substring(0, 5) == "ERROR"))
                    if (batch.Length >= 5 && batch.Substring(0, 5) == "ERROR")
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
            TextEdit txtPub = (TextEdit)sender;
            //LabelControl lblPub = (LabelControl)sender;

            if (e.KeyCode == Keys.Enter && txtPub.Enabled)
            {
                if (txtPub.Name == "txt1")
                {
                    string outString1 = string.Empty;
                    string mainSN = txtPub.Text.Trim();
                    if (string.IsNullOrEmpty(mainSN))
                    {
                        return;
                    }
                    DataSet dsMainSN;
                    dsMainSN = PartSelectSVC.uspCallProcedure("uspGetBaseData", mainSN,
                           null, null, null, null, null, ref outString1);
                    if (outString1 != "1" || dsMainSN == null || dsMainSN.Tables[0].Rows.Count <= 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20119", "NG", List_Login.Language);
                        txtPub.Text = string.Empty;
                        return;
                    }

                    DataTable dt = dsMainSN.Tables[0];
                    S_ProductionOrderID = dt.Rows[0]["ProductionOrderID"].ToString();
                    S_PartID = dt.Rows[0]["PartID"].ToString();
                    S_PartFamilyID = dt.Rows[0]["PartFamilyID"].ToString();
                    S_PartFamilyType = dt.Rows[0]["PartFamilyTypeID"].ToString();

                    //string PO = Com_PO.EditValue.ToString();
                    DataSet dsCOF = PartSelectSVC.GetluPODetailDef(Convert.ToInt32(S_ProductionOrderID), "COF");
                    if (dsCOF != null && dsCOF.Tables.Count > 0 && dsCOF.Tables[0].Rows.Count > 0)
                    {
                        COF = dsCOF.Tables[0].Rows[0]["Content"].ToString() == "1";
                    }

                     ds = PartSelectSVC.MESGetBomPartInfo(Convert.ToInt32(S_PartID),
                                   List_Login.StationTypeID);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count < 2)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20023", "NG", List_Login.Language, new string[] { Com_Part.Text.ToString() });
                        return;
                    }

                

                    string[] TagPro = new string[]
                   {
                        ds.Tables[0].Rows[0]["PartID"].ToString(),             //料号
                        ds.Tables[0].Rows[0]["ScanType"].ToString(),                                        //扫描类型
                        ds.Tables[0].Rows[0]["Pattern"].ToString(),            //校验正则
                        ds.Tables[0].Rows[0]["FieldName"].ToString(),          //存储字段
                        "0" ,//是否锁定
                        S_ProductionOrderID
                   };
                    txtPub.Tag = TagPro;
                }

                string btnName = txtPub.Name.Substring(3, txtPub.Name.Length - 3);
                bool IsChooseCotrol = false;
                bool Check = CheckInputBarcode(txtPub);

                if (Check && txtPub.Enabled)
                {
                    if (txtPub.Name == "txt1")
                    {
                        CreateControl(ds.Tables[0], S_ProductionOrderID);
                    }

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
                        else
                        {
                            panelSpool.Controls.Clear();
                            //foreach (Control c in this.panelSpool.Controls)
                            //{
                            //    c.Dispose();
                            //}
                            int lblstartX = 35;
                            int lblstartY = 35;
                            int txtstartX = 280;
                            int txtBtnstartY = 26;

                            LabelControl labels = new LabelControl();
                            labels.AutoSize = true;
                            labels.Text = "SN";
                            labels.Name = "lbl1";
                            labels.Location = new System.Drawing.Point(lblstartX, lblstartY);

                            TextEdit txts = new TextEdit();
                            txts.Name = "txt1";

                            txts.Location = new System.Drawing.Point(txtstartX, txtBtnstartY);
                            txts.KeyDown += Txts_KeyDown;
                            txts.Enabled = true;


                            labels.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
                            txts.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
                            txts.Size = new System.Drawing.Size(548, 30);


                            panelSpool.Controls.Add(labels);
                            panelSpool.Controls.Add(txts);

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
                    if (IsChooseCotrol && ctr is TextEdit && ctr.Enabled)
                    {
                        ctr.Focus();
                        return;
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
            SimpleButton btnPub = sender as SimpleButton;
            bool IsChooseCotrol = false;

            string btnName = btnPub.Name.Substring(3, btnPub.Name.Length - 3);
            foreach (Control ctr in panelSpool.Controls)
            {
                if (ctr.Name == "txt" + btnName)
                {
                    if (btnPub.Tag.ToString() == "1")
                    {
                        if (!ctr.Enabled || CheckInputBarcode(ctr as TextEdit))
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
                if (IsChooseCotrol && ctr is TextEdit && ctr.Enabled)
                {
                    ctr.Focus();
                    return;
                }
            }
        }

        /// <summary>
        /// TextEdit输入框验证
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="pre">正则验证</param>
        /// <returns></returns>
        private bool CheckInputBarcode(TextEdit txtBox)
        {
            try
            {
                string barcode = txtBox.Text.Trim();
                string[] strList = txtBox.Tag as string[];
                string Pattern = strList[2].ToString();
                string ScanType = strList[1].ToString();
                string PartID = strList[0].ToString();
                string ProductionOrderID= strList[5].ToString();
                string Result = "1";
                string xmlPartStr = "<Part PartID=\"" + PartID + "\"> </Part>";

                //空值检查
                if (string.IsNullOrEmpty(barcode))
                {
                    return false;
                }
                if (!new string[] { "3", "4", "6" }.Contains(ScanType))
                {
                    //正则校验
                    if (!Regex.IsMatch(barcode, Pattern.Replace("\\\\", "\\")))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20027", "NG", List_Login.Language, new string[] { barcode });
                        txtBox.SelectAll();
                        return false;
                    }
                }

                string NowBarCode = barcode;
                switch (ScanType)
                {
                    case "1":
                        if (txtBox.Name.Replace("txt", "") == "1")
                        {
                            Result = PartSelectSVC.MESAssembleCheckMianSN(ProductionOrderID, List_Login.LineID,
                                        List_Login.StationID, List_Login.StationTypeID, barcode, COF);
                        }
                        else
                        {
                            Result = PartSelectSVC.MESAssembleCheckOtherSN(barcode, PartID, COF);
                        }
                        break;
                    case "2":
                    case "7":
                        //批次验证
                        PartSelectSVC.uspCallProcedure("uspBatchDataCheck", barcode,
                                                                                null, xmlPartStr, null, null, "1", ref Result);
                        if (Result != "1")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { barcode, ProMsg }, Result);
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
                        break;
                    case "3":
                    case "4":
                    case "6":
                        //Machine校验
                        string StationTypeID = List_Login.StationTypeID.ToString();
                        string MainCode = string.Empty;
                        foreach (Control ctr in panelSpool.Controls)
                        {
                            if (ctr.Name == "txt1")
                            {
                                MainCode = ctr.Text.ToString();
                                break;
                            }
                        }
                        string xmlExtraData = "<ExtraData MainCode=\"" + MainCode + "\"> </ExtraData>";
                        PartSelectSVC.uspCallProcedure("uspMachineToolingCheck", barcode,
                                                                                null, xmlPartStr, null, xmlExtraData, StationTypeID, ref Result);

                        if (txtBox.Name.Replace("txt", "") == "1" && Result == "1")
                        {
                            //Tooling 转换BarCode
                            NowBarCode = PartSelectSVC.BuckToFGSN(barcode);
                            Result = PartSelectSVC.MESAssembleCheckMianSN(ProductionOrderID, List_Login.LineID,
                                                List_Login.StationID, List_Login.StationTypeID, NowBarCode, COF);
                        }

                        if (ScanType == "3" && Result == "1")
                        {
                            PartSelectSVC.uspCallProcedure("uspBatchDataCheck", barcode,
                                                        null, xmlPartStr, null, null, "2", ref Result);
                            if (Result != "1")
                            {
                                string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { barcode, ProMsg }, Result);
                                txtBox.Text = "";
                                return false;
                            }

                            if (!SearchBatch(txtBox.Name))
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20028", "NG", List_Login.Language, new string[] { barcode });
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
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20029", "NG", List_Login.Language, new string[] { barcode });
                            txtBox.Text = "";
                            return false;
                        }
                        break;
                    default:
                        break;
                }

                if (Result != "1")
                {
                    if (Result == "20243")
                    {
                        string nowUnitStateID = PartSelectSVC.GetSerialNumber2(NowBarCode).Tables[0].Rows[0]["UnitStateID"].ToString();
                        mesUnitState mesunitSateModel = mesUnitStateSVC.Get(Convert.ToInt32(nowUnitStateID));
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20243", "NG", List_Login.Language, new string[] { NowBarCode, mesunitSateModel.Description });
                    }
                    else
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { barcode, ProMsg }, Result);
                    }

                    txtBox.Text = "";
                    return false;
                }

                //调用通用过程
                string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + ProductionOrderID+ "\"> </ProdOrder>";
                string xmlPart = "<Part PartID=\"" + PartID + "\"> </Part>";
                string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                string outString = string.Empty;
                PartSelectSVC.uspCallProcedure("uspAssembleCheck", barcode,
                                                                        xmlProdOrder, xmlPart, xmlStation, null, PartID, ref outString);
                if (outString != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { barcode, ProMsg }, outString);
                    txtBox.Text = "";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
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
                //新增ScanType为1的重新校验  防止shell多次绑定
                if (ctr.Name.Substring(0, 3) == "txt" && ((ctr.Tag as string[])[4].ToString() == "1"
                    || (ctr.Tag as string[])[1].ToString() == "1"))
                {
                    if (!CheckInputBarcode((ctr as TextEdit)))
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
            //string S_POID = Com_PO.EditValue.ToString();
            //string S_PartID = Com_Part.EditValue.ToString();
            //string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
            string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, S_PartFamilyID,List_Login.StationTypeID, 
                List_Login.LineID.ToString(), S_ProductionOrderID, Com_luUnitStatus.EditValue.ToString());
            if (string.IsNullOrEmpty(S_UnitStateID))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
                return false;
            }
            int MainUnitID = 0;

            foreach (Control ctr in panelSpool.Controls)
            {
                if (ctr.Name.Substring(0, 3) == "txt")
                {
                    TextEdit txt = ctr as TextEdit;
                    string[] tagList = ctr.Tag as string[];
                    string Batch = "";
                    string S_SN = txt.Text.Trim();

                    string ScanType = tagList[1].ToString();
                    if (ScanType == "6")
                    {
                        DataSet dtsMac = mesUnitDetailSVC.MesGetBatchIDByBarcodeSN(ctr.Text.Trim());
                        if (dtsMac == null || dtsMac.Tables[0].Rows.Count == 0)
                        {
                            ScanType = "4";
                        }
                        else
                        {
                            ScanType = "3";
                        }
                    }

                    #region 循环控件数据提交
                    if (ctr.Name.Replace("txt", "") == "1")
                    {
                        if (ScanType == "4")
                        {
                            string Result = VirtualBarCode(S_SN, S_PartID, S_PartFamilyID, S_UnitStateID, S_ProductionOrderID);
                            if (Result == "NG" || string.IsNullOrEmpty(Result))
                            {
                                return false;
                            }
                            MainUnitID = Convert.ToInt32(Result);
                        }
                        else
                        {
                            if (ScanType == "3")
                            {
                                //找到虚拟条码UnitID
                                DataSet dataSetMachine = PartSelectSVC.BoxSnToBatch(txt.Text.Trim(), out Batch);
                                if (dataSetMachine == null || dataSetMachine.Tables.Count == 0 || dataSetMachine.Tables[0].Rows.Count == 0)
                                {

                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20030", "NG", List_Login.Language, new string[] { S_SN });
                                    return false;
                                }
                                MainUnitID = Convert.ToInt32(dataSetMachine.Tables[0].Rows[0]["UnitID"].ToString());
                            }
                            else
                            {
                                DataTable DT_UPUnitID = PartSelectSVC.Get_UnitID(S_SN).Tables[0];
                                MainUnitID = Convert.ToInt32(DT_UPUnitID.Rows[0]["UnitID"].ToString());
                            }
                            v_mesUnit.ID = MainUnitID;
                            v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                            v_mesUnit.StatusID = 1;
                            v_mesUnit.StationID = List_Login.StationID;
                            v_mesUnit.EmployeeID = List_Login.EmployeeID;
                            v_mesUnit.ProductionOrderID = Convert.ToInt32(S_ProductionOrderID);
                            //修改 Unit
                            string S_UpdateUnit = mesUnitSVC.Update(v_mesUnit);

                            if (S_UpdateUnit.Substring(0, 1) == "E")
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_UpdateUnit });
                                return false;
                            }
                        }

                        mesHistory v_mesHistory = new mesHistory();
                        string S_POPartID = S_PartID;

                        v_mesHistory.UnitID = MainUnitID;
                        v_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                        v_mesHistory.EmployeeID = List_Login.EmployeeID;
                        v_mesHistory.StationID = List_Login.StationID;
                        v_mesHistory.ProductionOrderID = Convert.ToInt32(S_ProductionOrderID);
                        v_mesHistory.PartID = Convert.ToInt32(S_POPartID);
                        v_mesHistory.LooperCount = 1;
                        v_mesHistory.EnterTime = DateTime.Now;
                        int I_InsertHistory = mesHistorySVC.Insert(v_mesHistory);
                        msg = "Mian Code:" + txt.Text.ToString();
                    }
                    else
                    {
                        mesUnitComponent v_mesUnitComponent = new mesUnitComponent();

                        if (ScanType == "2" || ScanType == "5" || ScanType == "7")
                        {
                            v_mesUnitComponent.UnitID = MainUnitID;
                            v_mesUnitComponent.UnitComponentTypeID = 1;
                            v_mesUnitComponent.ChildUnitID = 0;
                            if (ScanType == "7")
                            {
                                DataSet Pss = PartSelectSVC.GetmesSerialNumber(txt.Text.Trim());
                                if(Pss!=null && Pss.Tables.Count>0)
                                {
                                    v_mesUnitComponent.ChildUnitID = Convert.ToInt32(Pss.Tables[0].Rows[0]["UnitID"].ToString());
                                }
                            }

                            if (ScanType == "2")
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

                            if (ScanType == "3")
                            {
                                //找到虚拟条码UnitID
                                DataSet dataSetMachine = PartSelectSVC.BoxSnToBatch(S_SN, out Batch);
                                if (dataSetMachine == null || dataSetMachine.Tables.Count == 0 || dataSetMachine.Tables[0].Rows.Count == 0)
                                {
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20030", "NG", List_Login.Language, new string[] { S_SN });
                                    return false;
                                }
                                ChildUnitID = Convert.ToInt32(dataSetMachine.Tables[0].Rows[0]["UnitID"].ToString());
                            }
                            else if (ScanType == "4")
                            {
                                string Result = VirtualBarCode(S_SN, tagList[0].ToString(), null, S_UnitStateID, S_ProductionOrderID);
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
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20031", "NG", List_Login.Language, new string[] { S_SN });
                                    return false;
                                }
                                ChildUnitID = Convert.ToInt32(dataSetUnit.Tables[0].Rows[0]["UnitID"].ToString());
                            }

                            DataSet dsComponent = PartSelectSVC.GetComponent(ChildUnitID);
                            if (dsComponent == null || dsComponent.Tables.Count == 0 || dsComponent.Tables[0].Rows.Count == 0)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20032", "NG", List_Login.Language, new string[] { S_SN });
                                return false;
                            }

                            string S_ChildSerialNumber = dsComponent.Tables[0].Rows[0]["Value"].ToString();
                            int ChildPartFamilyID = Convert.ToInt32(dsComponent.Tables[0].Rows[0]["PartFamilyID"].ToString());

                            v_mesUnitComponent.UnitID = MainUnitID;
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
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_UC_Result });
                            return false;
                        }
                        msg = msg + " Child barcode:" + txt.Text.ToString();
                    }

                    if (tagList[1].ToString() == "3" || tagList[1].ToString() == "4" || tagList[1].ToString() == "6")
                    {
                        mesMachineSVC.MesModMachineBySNStationTypeID(S_SN, List_Login.StationTypeID);
                    }

                    if (tagList[1].ToString() == "2" || tagList[1].ToString() == "7" || tagList[1].ToString() == "3")
                    {
                        string SN = string.Empty;
                        string MachineSN = string.Empty;
                        int BatchType = 1;
                        if(tagList[1].ToString() == "3")
                        {
                            MachineSN = txt.Text.Trim();
                            BatchType = 2;
                        }
                        else
                        {
                            SN = txt.Text.Trim();

                        }
                        string ResultStr = PartSelectSVC.ModMesMaterialConsumeInfo(List_Login, BatchType, SN, MachineSN,
                                        Convert.ToInt32(S_PartID), Convert.ToInt32(S_ProductionOrderID));
                        if (ResultStr != "1")
                        {
                            string ProMsg = MessageInfo.GetMsgByCode(ResultStr, List_Login.Language);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { txt.Text.Trim(), ProMsg }, ResultStr);
                        }
                    }
                    #endregion

                    //不锁定的清空
                    if (tagList[4].ToString() != "1")
                    {
                        txt.Enabled = true;
                        txt.Text = string.Empty;
                        if (!IsFouce)
                        {
                            txt.Focus();
                            IsFouce = true;
                        }
                    }
                }
            }
            TimeSpan ts = DateTime.Now - dateStart;
            double mill = Math.Round(ts.TotalMilliseconds, 0);
            MessageInfo.Add_Info_MSG(Edt_MSG, "10005", "OK", List_Login.Language, new string[] { msg, mill.ToString()});
            base.SetOverStiaonQTY(true);
            return true;
        }

        private string VirtualBarCode(string S_SN, string S_PartID, string S_PartFamilyID, string S_UnitStateID, string S_POID)
        {
            try
            {
                string S_InsertUnit = string.Empty;
                //判断是否已经生成了虚拟条码
                string S_FG_SN = PartSelectSVC.BuckToFGSN(S_SN);
                if (string.IsNullOrEmpty(S_FG_SN))
                {
                    string S_FormatSN = PartSelectSVC.mesGetSNFormatIDByList(S_PartID, S_PartFamilyID,
                        List_Login.LineID.ToString(), S_ProductionOrderID, List_Login.StationTypeID.ToString());
                    if (string.IsNullOrEmpty(S_FormatSN))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20034", "NG", List_Login.Language, new string[] { S_PartID });
                        return "NG";
                    }

                    string xmlProdOrder = "'<ProdOrder ProductionOrder=" + "\"" + S_ProductionOrderID + "\"" + "> </ProdOrder>'";
                    string xmlStation = "'<Station StationID=" + "\"" + List_Login.StationID.ToString() + "\"" + "> </Station>'";
                    string xmlPart = "'<Part PartID=" + "\"" + S_PartID + "\"" + "> </Part>'";
                    string xmlExtraData = "'<ExtraData BoxSN=" + "\"" + S_SN + "\"" +
                                                   " LineID = " + "\"" + List_Login.LineID.ToString() + "\"" +
                                                   " PartFamilyTypeID=" + "\"" + S_PartFamilyType + "\"" +
                                                   " LineType=" + "\"" + "M" + "\"" + "> </ExtraData>'";
                    DataSet DS = PartSelectSVC.uspSNRGetNext(S_FormatSN, 0, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, null);
                    string New_SN = DS.Tables[1].Rows[0][0].ToString();
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

                    mesUnitDetail v_mesUnitDetail = new mesUnitDetail();
                    v_mesUnitDetail.UnitID = Convert.ToInt32(S_InsertUnit);
                    v_mesUnitDetail.reserved_01 = S_SN;
                    v_mesUnitDetail.reserved_02 = "";
                    v_mesUnitDetail.reserved_03 = "1";
                    v_mesUnitDetail.reserved_04 = "";
                    v_mesUnitDetail.reserved_05 = "";
                    mesUnitDetailSVC.Insert(v_mesUnitDetail);

                    mesSerialNumber v_mesSerialNumber = new mesSerialNumber();
                    v_mesSerialNumber.UnitID = Convert.ToInt32(S_InsertUnit);
                    v_mesSerialNumber.SerialNumberTypeID = 8;
                    v_mesSerialNumber.Value = New_SN;
                    mesSerialNumberSVC.Insert(v_mesSerialNumber);
                }
                else
                {
                    DataSet ds = PartSelectSVC.GetmesSerialNumber(S_FG_SN);
                    S_InsertUnit = ds.Tables[0].Rows[0]["UnitID"].ToString();
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
                    v_mesUnit.ID = Convert.ToInt32(S_InsertUnit);
                    mesUnitSVC.Update(v_mesUnit);
                }
                return S_InsertUnit;
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                return "NG";
            }
        }

        private void BtnBatchConfirn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtGlueBatch.Text.Trim()))
            {
                InitializationGule();
            }
        }

        private void txtGlueBatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(txtGlueBatch.Text.Trim()))
            {
                InitializationGule();
            }
        }

        private void InitializationGule()
        {
            string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + S_ProductionOrderID + "\"> </ProdOrder>";
            string xmlPart = "<Part PartID=\"" + S_PartID + "\"> </Part>";
            string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
            string xmlExtraData = "<ExtraData EmployeeID=\"" + List_Login.EmployeeID + "\"> </ExtraData>";
            string outString = string.Empty;
            PartSelectSVC.uspCallProcedure("uspSetGlueBatch", txtGlueBatch.Text.Trim(),
                                                                    xmlProdOrder, xmlPart, xmlStation, xmlExtraData, null, ref outString);
            if (outString != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { txtGlueBatch.Text.Trim(), ProMsg }, outString);
                txtGlueBatch.Text = string.Empty;
            }
            else
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "10024", "OK", List_Login.Language, new string[] { txtGlueBatch.Text.Trim() });
                txtGlueBatch.Text = string.Empty;
            }
        }

        private void btnLock_Click(object sender, EventArgs e)
        {
            if (txtGlueBatch.Enabled)
            {
                txtGlueBatch.Enabled = false;
                btnLock.Text = "解锁";
            }
            else
            {
                txtGlueBatch.Enabled = true;
                btnLock.Text = "锁定";
            }
        }
    }
}