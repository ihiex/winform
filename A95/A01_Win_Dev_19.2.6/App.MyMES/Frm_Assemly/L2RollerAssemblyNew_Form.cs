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
using App.MyMES.mesStationService;
using App.Model;
using System.Text.RegularExpressions;
using System.Reflection;

namespace App.MyMES
{
    public partial class L2RollerAssemblyNew_Form : FrmBase
    {
        bool COF = false;

        private ScanerHook listener = new ScanerHook();
        public ImesStationSVCClient mesStationSVC = ImesStationFactory.CreateServerClient();
        string batch = string.Empty;

        public L2RollerAssemblyNew_Form()
        {
            InitializeComponent();
            listener.ScanerEvent += Listener_ScanerEvent;
        }

        private void Listener_ScanerEvent(ScanerHook.ScanerCodes codes)
        {
            string Code = codes.Result;
            CheckInputBarcode(Code);
        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            MoveControlForPanel();
            radioGroupOperation_SelectedIndexChanged(null,null);
        }

        private void radioGroupOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtMachine.Text = string.Empty;
            if (radioGroupOperation.SelectedIndex==0)
            {
                txtMachine.Enabled = false;
                btnMachineChange.Enabled = false;
                listener.Start();
            }
            else
            {
                txtMachine.Enabled = true;
                btnMachineChange.Enabled = true;
                txtMachine.Focus();
                listener.Stop();
            }
        }

        private void btnMachineChange_Click(object sender, EventArgs e)
        {
            txtMachine.Text = string.Empty;
            txtMachine.Enabled = true;
            txtMachine.Focus();
            MoveControlForPanel();
        }

        /// <summary>
        /// 重写确认菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            base.Btn_ConfirmPO_Click(sender, e);
            string PO = Com_PO.EditValue.ToString();
            DataSet dsCOF = PartSelectSVC.GetluPODetailDef(Convert.ToInt32(PO), "COF");
            if (dsCOF != null && dsCOF.Tables.Count > 0 && dsCOF.Tables[0].Rows.Count > 0)
            {
                COF = dsCOF.Tables[0].Rows[0]["Content"].ToString() == "1";
            }
        }

        /// <summary>
        /// 扫描枪扫描验证输入条码
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        private void CheckInputBarcode(string barcode)
        {
            try
            {
                if (barcode.Length >= 2 && barcode.Substring(0, 2) == "OK")
                {
                    btnConfirm_Click(null, null);
                    return;
                }

                else if (barcode.Length >= 3 && barcode.Substring(0, 3) == "L2-")
                {
                    if (CheckMachine(barcode))
                    {
                        txtMachine.Text = barcode;
                        txtMachine.ForeColor = Color.Green;
                        return;
                    }
                    else
                    {
                        return;
                    }
                }

                foreach (Control ctls in panelSpool.Controls)
                {
                    if (ctls is TextEdit && ctls.Tag.GetType().ToString() == "System.String[]")
                    {

                        string[] strList = (string[])ctls.Tag;
                        string pre = strList[0].ToString().Replace("\\\\", "\\");

                        if (barcode == strList[1].ToString())
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20170", "NG", List_Login.Language, new string[] { barcode });
                            return;
                        }

                        //正则校验
                        if (Regex.IsMatch(barcode, pre))
                        {
                            ctls.Text = barcode;
                            bool IsInput = CheckInputBarcode(ctls as TextEdit);
                            if (!IsInput)
                            {
                                ctls.Text = string.Empty;
                                return;
                            }

                            if (barcode == strList[1].ToString())
                            {
                                ctls.BackColor = System.Drawing.SystemColors.Window;
                            }
                            else
                            {
                                ctls.BackColor = Color.Blue;
                            }

                            MessageInfo.Add_Info_MSG(Edt_MSG, "10027", "OK", List_Login.Language, new string[] { barcode });
                            return;
                        }
                    }
                }
                MessageInfo.Add_Info_MSG(Edt_MSG, "20171", "NG", List_Login.Language, new string[] { barcode });
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
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
                string Result = "1";
                string xmlPartStr = "<Part PartID=\"" + PartID + "\"> </Part>";

                //空值检查
                if (string.IsNullOrEmpty(barcode))
                {
                    return false;
                }
                //正则校验
                if (!Regex.IsMatch(barcode, Pattern.Replace("\\\\", "\\")))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20027", "NG", List_Login.Language, new string[] { barcode });
                    txtBox.SelectAll();
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
                    string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { barcode, ProMsg }, outString);
                    txtBox.Text = "";
                    return false;
                }

                switch (ScanType)
                {
                    case "1":
                        if (txtBox.Name.Replace("txt", "") == "1")
                        {
                            Result = PartSelectSVC.MESAssembleCheckMianSN(Com_PO.EditValue.ToString(), List_Login.LineID,
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
                    string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { barcode, ProMsg }, Result);
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

        /// <summary>
        /// 查找批次号
        /// </summary>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        private bool SearchBatch(string ControlName)
        {
            string ControlNumber = ControlName.Replace("txt", "");
            foreach (Control ctr in panelSpool.Controls)
            {
                if (ctr.Name == ControlName)
                {
                    PartSelectSVC.BoxSnToBatch(ctr.Text.Trim(), out batch);
                    if (string.IsNullOrEmpty(batch) || (batch.Length >= 5 && batch.Substring(0, 5) == "ERROR"))
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
        /// 机器编号校验
        /// </summary>
        /// <returns></returns>
        private bool CheckMachine(string barcode)
        {
            try
            {
                //判断工单对应线别是否与扫描的机器对应线别一致
                string Machine = barcode;
                mesStation[] listStation = mesStationSVC.ListAll("where Description='" + Machine + "'");
                if (listStation==null|| listStation.Count()==0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20162", "NG", List_Login.Language, new string[] { Machine });
                    return false;
                }

                string LineID = listStation[0].LineID.ToString();
                string StationID = listStation[0].ID.ToString();

                if (LineID != List_Login.LineID.ToString())
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20163", "NG", List_Login.Language, new string[] { Machine });
                    return false;
                }

                //查询当前机器已存在的关联数据
                string xmlPart = "<Part PartID=\"" + Com_Part.EditValue + "\"> </Part>";
                string xmlStation = "<Station StationID=\"" + StationID + "\"> </Station>";
                string outString = string.Empty;
                DataSet resultSet = PartSelectSVC.uspCallProcedure("uspGetL2UpSpoolData", null,
                                                                        null, xmlPart, xmlStation, null, null, ref outString);
                if (outString != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Machine, ProMsg }, outString);
                    return false;
                }

                if (resultSet == null || resultSet.Tables.Count < 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20164", "NG", List_Login.Language, new string[] { Machine });
                    return false;
                }

                //生成控件
                MoveControlForPanel();
                CreateControl(resultSet.Tables[0]);
                MessageInfo.Add_Info_MSG(Edt_MSG, "10026", "OK", List_Login.Language, new string[] { Machine});
                List_Login.StationID = Convert.ToInt32(StationID);
                return true;
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                return false;
            }
        }

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
        private void CreateControl(DataTable dt)
        {
            int lblstartX = 35;
            int lblstartY = 35;
            int txtstartX = 220;
            int txtBtnstartY = 26;
            int btnStartX = 700;
            int interval = 65;
            LabelControl lblBatch = null;
            TextEdit txtBatch = null;

            if (dt.Rows.Count > 2)
                interval = 40;

            for (int j = 1; j <= dt.Rows.Count; j++)
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

                if (!(new string[] { "1", "2", "3", "4", "5", "6", "7" }).Contains(scanType))
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
                txts.ForeColor = Color.Black;
                txts.Name = "txt" + j.ToString();
                string[] TagPro = new string[]
                    {
                        dt.Rows[j - 1]["PartID"].ToString(),             //料号
                        scanType,                                        //扫描类型
                        dt.Rows[j - 1]["Pattern"].ToString(),            //校验正则
                        dt.Rows[j - 1]["FieldName"].ToString(),          //存储字段
                        "0",                                             //是否锁定
                        ""                                               //替换之前存值
                    };
                txts.Tag = TagPro;

                //托盘、箱子（生成虚拟SN类型,显示绑定的批次号）
                if (ScanType == "3")
                {
                    //增加批次号
                    lblBatch = new LabelControl();
                    lblBatch.AutoSize = true;
                    lblBatch.Text = "Batch(批次号):";
                    lblBatch.Name = "lbc" + j.ToString();
                    lblBatch.Location = new System.Drawing.Point(415, lblstartY + (j - 1) * interval);

                    txtBatch = new TextEdit();
                    txtBatch.ForeColor = Color.Black;
                    txtBatch.Name = "tbc" + j.ToString();
                    txtBatch.Text = dt.Rows[j - 1]["Batch"].ToString();
                    txtBatch.Size = new System.Drawing.Size(175, 26);
                    txtBatch.Location = new System.Drawing.Point(513, txtBtnstartY + (j - 1) * interval);
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

                SimpleButton btns = new SimpleButton();
                if (!string.IsNullOrEmpty(dt.Rows[j - 1]["Barcode"].ToString()))
                {
                    txts.Text = dt.Rows[j - 1]["Barcode"].ToString();
                    (txts.Tag as string[])[5] = txts.Text.Trim();
                    txts.Enabled = false;
                    btns.Text = "Unlock(解除)";
                    btns.Tag = 0;
                }
                else
                {
                    txts.Enabled = true;
                    btns.Text = "Lock(锁定)";
                    btns.Tag = 1;
                }

                btns.Name = "btn" + j.ToString();
                btns.Size = new System.Drawing.Size(97, 30);
                btns.Location = new System.Drawing.Point(btnStartX, txtBtnstartY + (j - 1) * interval);
                btns.Click += Btns_Click;

                if (j == 1)
                {
                    labels.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
                    txts.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);

                    if (ScanType == "3")
                        txts.Size = new System.Drawing.Size(185, 30);
                    else
                        txts.Size = new System.Drawing.Size(463, 30);
                }
                else
                {
                    labels.Font = new System.Drawing.Font("宋体", 10F);
                    txts.Font = new System.Drawing.Font("宋体", 13F);
                    if (ScanType == "3")
                        txts.Size = new System.Drawing.Size(185, 26);
                    else
                        txts.Size = new System.Drawing.Size(463, 28);
                }

                panelSpool.Controls.Add(labels);
                panelSpool.Controls.Add(txts);
                if (scanType == "3")
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
                    if (scanType == "3")
                    {
                        if (MessageInfo.Add_Info_MessageBox("10022", List_Login.Language))
                        {
                            string result = mesMachineSVC.MesToolingReleaseCheck(ctr.Text.Trim(), List_Login.StationTypeID.ToString());
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
        /// 手动回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Txts_KeyDown(object sender, KeyEventArgs e)
        {
            TextEdit txtPub = (TextEdit)sender;

            if (e.KeyCode == Keys.Enter && txtPub.Enabled)
            {
                string btnName = txtPub.Name.Substring(3, txtPub.Name.Length - 3);
                bool IsChooseCotrol = false;
                bool Check = CheckInputBarcode(txtPub);

                if (Check && txtPub.Enabled)
                {
                    txtPub.Enabled = false;
                    IsChooseCotrol = true;
                    string[] strList = (txtPub.Tag as string[]);
                    if(string.IsNullOrEmpty(strList[5].ToString())||
                        strList[5].ToString()!= txtPub.Text.Trim())
                    {
                        txtPub.ForeColor = Color.Blue;
                    }
                    else
                    {
                        txtPub.ForeColor = Color.Black;
                    }

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

                if (radioGroupOperation.SelectedIndex == 0)
                    return;

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

            if (radioGroupOperation.SelectedIndex == 0)
                return;

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

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (CheckControlInput())
            {
                SubmitData();
            }
            else
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20167", "NG", List_Login.Language); 
            }
        }

        public bool CheckControlInput()
        {
            bool IsReplace = false;

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

                    if(ctr.ForeColor==Color.Blue)
                    {
                        IsReplace = true;
                    }
                }
            }

            if(!IsReplace)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20168", "NG", List_Login.Language);
                return false;
            }

            //Machine类型必须重新校验(不放上面防止重复校验)
            foreach (Control ctr in panelSpool.Controls)
            {
                if (ctr.Name.Substring(0, 3) == "txt" && (ctr.Tag as string[])[4].ToString() == "1")
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
            string S_POID = Com_PO.EditValue.ToString();
            string S_PartID = Com_Part.EditValue.ToString();
            string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
            string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, S_PartFamilyID, List_Login.StationTypeID, 
                List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), Com_luUnitStatus.EditValue.ToString());
            if (string.IsNullOrEmpty(S_UnitStateID))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
                return false;
            }
            int MainUnitID = 0;

            string sqlValue = "";
            foreach (Control ctr in panelSpool.Controls)
            {
                if (ctr.Name.Substring(0, 3) == "txt")
                {
                    TextEdit txt = ctr as TextEdit;
                    string[] tagList = ctr.Tag as string[];
                    string Batch = "";
                    string S_SN = txt.Text.Trim();
                    string ScanType = string.Empty;

                    #region 循环控件数据提交
                    if (ctr.Name.Replace("txt", "") == "1")
                    {
                        ScanType = tagList[1].ToString();
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

                        if (ScanType == "4")
                        {
                            string Result = VirtualBarCode(S_SN, S_PartID, S_PartFamilyID, S_UnitStateID, S_POID);
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
                            sqlValue = sqlValue + " update mesUnit set UnitStateID='" + Convert.ToInt32(S_UnitStateID) +
                                       "',StatusID=1,StationID='" + List_Login.StationID + "',EmployeeID='" + List_Login.EmployeeID +
                                       "',ProductionOrderID='" + Convert.ToInt32(S_POID) + "',LastUpdate=getdate() where ID='" + MainUnitID + "'";
                        }

                        string S_POPartID = DT_ProductionOrder.Rows[0]["PartID"].ToString();

                        sqlValue = sqlValue + " insert into mesHistory(UnitID, UnitStateID, EmployeeID, StationID, EnterTime, ExitTime, ProductionOrderID, PartID, LooperCount, StatusID) " +
                                              "values('" + MainUnitID + "', '" + Convert.ToInt32(S_UnitStateID) + "', '" + List_Login.EmployeeID +
                                              "', '" + List_Login.StationID + "', getdate(), GETDATE(), '" + Convert.ToInt32(S_POID) + "', '" + Convert.ToInt32(S_POPartID) + "', '1', 1)";

                        msg = "Mian Code:" + txt.Text.ToString();
                    }
                    else if(ctr.ForeColor == Color.Blue)
                    {
                        //if(ScanType=="6")
                        //{

                        //}

                        mesUnitComponent v_mesUnitComponent = new mesUnitComponent();

                        if (tagList[1].ToString() == "2" || tagList[1].ToString() == "5" || tagList[1].ToString() == "7")
                        {
                            v_mesUnitComponent.UnitID = MainUnitID;
                            v_mesUnitComponent.UnitComponentTypeID = 1;
                            v_mesUnitComponent.ChildUnitID = 0;
                            if (tagList[1].ToString() == "7")
                            {
                                DataSet Pss = PartSelectSVC.GetmesSerialNumber(txt.Text.Trim());
                                if (Pss != null && Pss.Tables.Count > 0)
                                {
                                    v_mesUnitComponent.ChildUnitID = Convert.ToInt32(Pss.Tables[0].Rows[0]["UnitID"].ToString());
                                }
                            }

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
                                if (dataSetMachine == null || dataSetMachine.Tables.Count == 0 || dataSetMachine.Tables[0].Rows.Count == 0)
                                {
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20030", "NG", List_Login.Language, new string[] { S_SN });
                                    return false;
                                }
                                ChildUnitID = Convert.ToInt32(dataSetMachine.Tables[0].Rows[0]["UnitID"].ToString());
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

                        sqlValue = sqlValue + " insert into mesUnitComponent(UnitID, UnitComponentTypeID, ChildUnitID, ChildSerialNumber, ChildLotNumber, " +
                                              "ChildPartID, ChildPartFamilyID,Position, InsertedEmployeeID, InsertedStationID, InsertedTime, StatusID, LastUpdate) " +
                                              "values('" + v_mesUnitComponent.UnitID + "','1','" + v_mesUnitComponent.ChildUnitID + "','" + v_mesUnitComponent.ChildSerialNumber +
                                              "','" + v_mesUnitComponent.ChildLotNumber + "','" + v_mesUnitComponent.ChildPartID + "','" + v_mesUnitComponent.ChildPartFamilyID +
                                              "','','" + v_mesUnitComponent.InsertedEmployeeID + "','" + v_mesUnitComponent.InsertedStationID + "',GETDATE(),1,GETDATE())";

                        msg = msg + " Child barcode:" + txt.Text.ToString();
                    }
                    else
                    {
                        continue;
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
                        if (tagList[1].ToString() == "3")
                        {
                            MachineSN = txt.Text.Trim();
                            BatchType = 2;
                        }
                        else
                        {
                            SN = txt.Text.Trim();

                        }
                        string ResultStr = PartSelectSVC.ModMesMaterialConsumeInfo(List_Login, BatchType, SN, MachineSN,
                                        Convert.ToInt32(Com_Part.EditValue.ToString()), Convert.ToInt32(Com_PO.EditValue.ToString()));
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

            string ReturnValue = PartSelectSVC.ExecSql(sqlValue);
            if (ReturnValue != "OK")
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ReturnValue });
                return false;
            }

            TimeSpan ts = DateTime.Now - dateStart;
            double mill = Math.Round(ts.TotalMilliseconds, 0);
            MessageInfo.Add_Info_MSG(Edt_MSG, "10005", "OK", List_Login.Language, new string[] { msg, mill.ToString() });
            base.SetOverStiaonQTY(true);
            btnMachineChange_Click(null,null);
            return true;
        }

        private string VirtualBarCode(string S_SN, string S_PartID, string S_PartFamilyID, string S_UnitStateID, string S_POID)
        {
            try
            {
                string S_InsertUnit = string.Empty;
                string S_xmlPart = "'<BoxSN SN=" + "\"" + S_SN + "\"" + "> </BoxSN>'";
                string S_FormatSN = PartSelectSVC.mesGetSNFormatIDByList(S_PartID, S_PartFamilyID,
                    List_Login.LineID.ToString(), Com_PO.EditValue.ToString(), List_Login.StationTypeID.ToString());
                if (string.IsNullOrEmpty(S_FormatSN))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20034", "NG", List_Login.Language, new string[] { S_PartID });
                    return "NG";
                }
                DataSet DS = PartSelectSVC.uspSNRGetNext(S_FormatSN, 0, null, S_xmlPart, null, null, null);
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
                return S_InsertUnit;
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                return "NG";
            }
        }

        private void L2RollerAssembly_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            listener.Stop();
            mesStationSVC.Close();
        }

        private void txtMachine_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtMachine.Enabled)
                {
                    if (CheckMachine(txtMachine.Text.Trim()))
                    {
                        txtMachine.Enabled = false;
                        btnMachineChange.Text = "Change(更换)";
                    }
                    else
                    {
                        txtMachine.SelectAll();
                    }
                }
                else
                {
                    txtMachine.Enabled = true;
                    btnMachineChange.Text = "Confirm(确定)";
                    txtMachine.Focus();
                    txtMachine.SelectAll();
                }
            }
        }
    }
}