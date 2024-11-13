using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;
using App.Model;
using System.IO.Ports;
using System.Reflection;
using App.MyMES.mesRouteService;
using App.MyMES.PartSelectService;
using DevExpress.DataProcessing;
using MethodTimer;


namespace App.MyMES
{
    // 2021-11-12  测试成功  串口发送,是否检查PO，是否显示4件套并验证，均在配置端设置
    public partial class AssemblyV2_Form : FrmBase
    {
        bool COF = false;
        string IsAuto = "0";
        string IsNFC = "0";
        string IsBindAdhesive = "0";
        //string IsNoPOCheck = "0";
        //string IsNoPNCheck = "0";
        string S_IsCheckPO = "1";
        string S_IsCheckPN = "1";

        /// <summary>
        /// //为了性能，增加一个参数
        /// </summary>
        private string S_IsMainSNUPC = "0";

        /// <summary>
        /// 保存输入的UPC条码
        /// </summary>
        private string S_UPC_SN;
        string S_IsToolingLock = "1";


        private string strPort = "COM1";
        string ReadCmd = "READY";
        private string S_PASS_cmd { get; set; } = "PASS";
        private string S_FAIL_cmd { get; set; } = "FAIL";
        private string S_Is_AutoStation { get; set; } = "0";

        string TestSN = "TESTSHELL001";

        string S_ProductionOrderID = "";
        string S_PartID = "";
        string S_PartFamilyID = "";
        string S_PartFamilyType = "";
        DataSet dsBOM = null;
        DataSet DS_StationTypeDef;

        private string enableDetailLog = "0";

        List<ToolingInfo> toolingInfos = new List<ToolingInfo>();
        public AssemblyV2_Form()
        {
            InitializeComponent();
        }

        private DataSet GetStationData()
        {
            DataSet dsStationDetail = PartSelectSVC.GetPLCSeting("", List_Login.StationID.ToString());
            DS_StationTypeDef = PartSelectSVC.MESGetStationTypeParameter(List_Login.StationTypeID);

            DataRow[] drREAD = dsStationDetail.Tables[0].Select("Name='READY'");
            if (drREAD.Length > 0) ReadCmd = drREAD[0]["Value"].ToString(); else ReadCmd = "READY";

            DataRow[] drPort = dsStationDetail.Tables[0].Select("Name='Port'");
            if (drPort.Length > 0) strPort = drPort[0]["Value"].ToString(); else strPort = "COM1";

            DataRow[] drAutoStation = dsStationDetail.Tables[0].Select("Name='AutoStation'");
            if (drAutoStation.Length > 0) IsAuto = drAutoStation[0]["Value"].ToString(); else IsAuto = "0";

            DataRow[] drIsBindAdhesive = dsStationDetail.Tables[0].Select("Name='IsBindAdhesive'");
            if (drIsBindAdhesive.Length > 0) IsBindAdhesive = drIsBindAdhesive[0]["Value"].ToString(); else IsBindAdhesive = "0";

            DataRow[] drIsNFC = dsStationDetail.Tables[0].Select("Name='IsBindNFC'");
            if (drIsNFC.Length > 0) IsNFC = drIsNFC[0]["Value"].ToString(); else IsNFC = "0";

            DataRow[] drIsToolingLock = dsStationDetail.Tables[0].Select("Name='IsToolingLock'");
            if (drIsToolingLock.Length > 0) S_IsToolingLock = drIsToolingLock[0]["Value"].ToString(); else S_IsToolingLock = "1";

            DataRow[] drIsDetailLog = dsStationDetail.Tables[0].Select("Name='IsDetailLog'");
            if (drIsDetailLog.Length > 0) enableDetailLog = drIsDetailLog[0]["Value"].ToString(); else enableDetailLog = "0";
            //DataRow[] drIsNoPOCheck = dsStationDetail.Tables[0].Select("Name='IsNoPOCheck'");
            //if (drIsNoPOCheck.Length > 0) IsNoPOCheck = drIsNoPOCheck[0]["Value"].ToString(); else IsNoPOCheck = "0";

            //DataRow[] drIsNoPNCheck = dsStationDetail.Tables[0].Select("Name='IsNoPNCheck'");
            //if (drIsNoPNCheck.Length > 0) IsNoPNCheck = drIsNoPNCheck[0]["Value"].ToString(); else IsNoPNCheck = "0";

            {
                var drIsAutoStation = dsStationDetail.Tables[0].Select("Name='IsAutoStation'");

                if (drIsAutoStation != null && drIsAutoStation.Length > 0)
                {
                    S_Is_AutoStation = drIsAutoStation[0]["Value"].ToString();
                }
                else
                {
                    var drIsAutoStationType = DS_StationTypeDef.Tables[0].Select("Description='IsAutoStation'");
                    S_Is_AutoStation = drIsAutoStationType.Length > 0
                        ? drIsAutoStationType[0]["Content"].ToString()
                        : "0";
                }


                if (S_Is_AutoStation == "1")
                {
                    var drPass = dsStationDetail.Tables[0].Select("Name='PASS'");

                    if (drPass != null && drPass.Length > 0)
                    {
                        S_PASS_cmd = drPass[0]["Value"].ToString();
                    }
                    else
                    {
                        var drPassType = DS_StationTypeDef.Tables[0].Select("Description='PASS'");
                        S_PASS_cmd = drPassType.Length > 0
                            ? drPassType[0]["Content"].ToString()
                            : "PASS";
                    }

                    var drFail = dsStationDetail.Tables[0].Select("Name='FAIL'");

                    if (drFail != null && drFail.Length > 0)
                    {
                        S_FAIL_cmd = drFail[0]["Value"].ToString();
                    }
                    else
                    {
                        var drFailType = DS_StationTypeDef.Tables[0].Select("Description='FAIL'");
                        S_FAIL_cmd = drFailType.Length > 0
                            ? drFailType[0]["Content"].ToString()
                            : "FAIL";
                    }
                }
            }

            DataRow[] DR_IsCheckPO = DS_StationTypeDef.Tables[0].Select("Description='IsCheckPO'");
            if (DR_IsCheckPO.Length > 0)
            {
                S_IsCheckPO = DR_IsCheckPO[0]["Content"].ToString();

                if (S_IsCheckPO == "1")
                {
                    DataRow[] DR_Child_IsCheckPO = dsStationDetail.Tables[0].Select("Name='IsCheckPO'");
                    if (DR_Child_IsCheckPO.Length > 0) S_IsCheckPO = DR_Child_IsCheckPO[0]["Value"].ToString(); else S_IsCheckPO = "1";
                }
            }
            else
            {
                S_IsCheckPO = "1";
            }

            DataRow[] DR_IsCheckPN = DS_StationTypeDef.Tables[0].Select("Description='IsCheckPN'");
            if (DR_IsCheckPN.Length > 0)
            {
                S_IsCheckPN = DR_IsCheckPN[0]["Content"].ToString();

                if (S_IsCheckPN == "1")
                {
                    DataRow[] DR_Child_IsCheckPN = dsStationDetail.Tables[0].Select("Name='IsCheckPN'");
                    if (DR_Child_IsCheckPN.Length > 0) S_IsCheckPN = DR_Child_IsCheckPN[0]["Value"].ToString(); else S_IsCheckPN = "1";
                }
            }
            else
            {
                S_IsCheckPN = "1";
            }

            DataRow[] DR_IsToolingLock = DS_StationTypeDef.Tables[0].Select("Description='IsToolingLock'");
            if (DR_IsToolingLock.Length > 0)
            {
                S_IsToolingLock = DR_IsToolingLock[0]["Content"].ToString();

                if (S_IsToolingLock == "1")
                {
                    DataRow[] DR_Child_IsToolingLock = dsStationDetail.Tables[0].Select("Name='IsToolingLock'");
                    if (DR_Child_IsToolingLock.Length > 0) S_IsToolingLock = DR_Child_IsToolingLock[0]["Value"].ToString(); else S_IsToolingLock = "1";
                }
            }
            else
            {
                S_IsToolingLock = "1";
            }


            if (S_IsCheckPO == "1")
            {
                lblProductionOrder.Visible = true;
                lblUnitState.Visible = true;
                lblUnitState.Location = new Point(58, 135);

                panel12.Visible = true;
            }
            else
            {
                lblProductionOrder.Visible = false;
                lblUnitState.Visible = true;

                lblUnitState.Location = new Point(58, 102);
                panel12.Visible = false;
            }


            //判断是否胶注册
            DataRow[] drScanGlue = DS_StationTypeDef.Tables[0].Select("Description='IsScanAdhesive'");
            if (drScanGlue.Length > 0 && drScanGlue[0]["Content"].ToString() == "1")
            {
                PanelBatchConfirn.Visible = true;
            }
            else
            {
                PanelBatchConfirn.Visible = false;
            }

            DataRow[] drScanUPC = DS_StationTypeDef.Tables[0].Select("Description='IsMainSNUPC'");
            if (drScanUPC.Length > 0 && drScanUPC[0]["Content"].ToString() == "1")
            {
                S_IsMainSNUPC = "1";
            }
            return dsStationDetail;
        }

        public override void FrmBase_Load(object sender, EventArgs e)
        {
            base.FrmBase_Load(sender, e);
            base.Com_luUnitStatus.Enabled = false;
            GetStationData();


            //if (IsNoPNCheck == "0")
            if (S_IsCheckPN == "1")
            {
                panelSpool.Controls.Add(PanelBatchConfirn);
                GrpControlPart.Visible = true;
            }
            else
            {
                //  max 原著，

                //int lblstartX = 35;
                //int lblstartY = 35;
                //int txtstartX = 280;
                //int txtBtnstartY = 26;

                //LabelControl labels = new LabelControl();
                //labels.AutoSize = true;
                //labels.Text = "SN";
                //labels.Name = "lbl1";
                //labels.Location = new System.Drawing.Point(lblstartX, lblstartY);

                //TextEdit txts = new TextEdit();
                //txts.Name = "txt1";

                //txts.Location = new System.Drawing.Point(txtstartX, txtBtnstartY);
                //txts.KeyDown += Txts_KeyDown;
                //txts.Enabled = true;


                //labels.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
                //txts.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
                //txts.Size = new System.Drawing.Size(548, 30);

                GrpControlPart.Visible = false;
                GrpControlInputData.Enabled = true;
                //panelSpool .Controls.Add(labels);
                //panelSpool .Controls.Add(txts);


                dsBOM = PartSelectSVC.MESGetBomPartInfo(Convert.ToInt32(Com_Part.EditValue.ToString()),
                                   List_Login.StationTypeID);

                //工站类型配置扫描类型替换掉
                DataRow[] drScanType = DS_StationTypeDef.Tables[0].Select("Description='SNScanType'");
                if (drScanType.Length > 0)
                {
                    dsBOM.Tables[0].Rows[0]["ScanType"] = drScanType[0]["Content"];
                }

                CreateControl(dsBOM.Tables[0], "");
            }
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

            if (!int.TryParse(Com_Part.EditValue?.ToString(),out int I_PartID))
                return;

            dsBOM = PartSelectSVC.MESGetBomPartInfo(Convert.ToInt32(Com_Part.EditValue.ToString()),
                               List_Login.StationTypeID);
            //工站类型配置扫描类型替换掉
            DataRow[] drScanType = DS_StationTypeDef.Tables[0].Select("Description='SNScanType'");
            if (drScanType.Length > 0)
            {
                dsBOM.Tables[0].Rows[0]["ScanType"] = drScanType[0]["Content"];
            }

            if (dsBOM == null || dsBOM.Tables.Count == 0 || dsBOM.Tables[0].Rows.Count < 2)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20023", "NG", List_Login.Language, new string[] { Com_Part.Text.ToString() });
                return;
            }

            DataSet dsStationDetail = GetStationData();

            DataRow[] drTestSN = dsStationDetail.Tables[0].Select("Name='TestSN'");
            if (drTestSN.Length > 0) TestSN = drTestSN[0]["Value"].ToString(); else TestSN = "TESTSHELL001";

            if (IsAuto == "1" || S_Is_AutoStation == "1")
            {
                port1 = new SerialPort(strPort);
                port1.BaudRate = 9600;
                port1.DataBits = 8;
                port1.Parity = Parity.None;
                port1.StopBits = StopBits.One;

                try
                {
                    if (!port1.IsOpen)
                    {
                        port1.Open();
                    }
                }
                catch (Exception ex)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "60104", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                    return;
                }
            }

            CreateControl(dsBOM.Tables[0], "");
        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            MoveControlForPanel();

            GetStationData();
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
        private void CreateControl(DataTable dt, string S_ProductionOrderID)
        {
            int lblstartX = 35;
            int lblstartY = 35;
            int txtstartX = 280;
            int txtBtnstartY = 26;
            int btnStartX = 845;
            int interval = 65;
            int lblBinStartX = 945;
            LabelControl lblBatch = null;
            TextEdit txtBatch = null;

            if (dt.Rows.Count > 3)
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
                if (!(new string[] { "1", "2", "3", "4", "5", "6", "7", "8" }).Contains(scanType))
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

                string IsShowBin = "0";
                //20240405 
                {
                    int I_PartID = Convert.ToInt32(dt.Rows[j - 1]["PartID"].ToString());
                    DataSet dataSet = PartSelectSVC.GetmesPartDetail(I_PartID, "IsShowBin");
                    if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        IsShowBin = dataSet.Tables[0].Rows[0]["Content"].ToString();
                    }
                }

                string[] TagPro = new string[]
                    {
                        dt.Rows[j - 1]["PartID"].ToString(),             //料号
                        scanType,                                        //扫描类型
                        dt.Rows[j - 1]["Pattern"].ToString(),            //校验正则
                        dt.Rows[j - 1]["FieldName"].ToString(),          //存储字段
                        "0" ,                                             //是否锁定
                        S_ProductionOrderID,
                        IsShowBin                                        //是否需要显示条码对应的Bin信息
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
                if (S_IsToolingLock == "1")
                {
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
                }
                else
                {
                    if (ScanType != "1" && ScanType != "3" && ScanType != "4" && ScanType != "6")
                    {
                        btns = new SimpleButton();
                        btns.Text = "Lock(锁定)";
                        btns.Name = "btn" + j.ToString();
                        btns.Tag = 1;
                        btns.Size = new System.Drawing.Size(97, 30);
                        btns.Location = new System.Drawing.Point(btnStartX, txtBtnstartY + (j - 1) * interval);
                        btns.Click += Btns_Click;
                    }
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

                if (IsShowBin == "1")
                {
                    LabelControl lblBin = new LabelControl();
                    lblBin.AutoSize = true;
                    lblBin.Text = "";
                    lblBin.Name = "lbcB" + j.ToString();
                    lblBin.Font = new System.Drawing.Font("宋体", 14F, FontStyle.Bold);
                    lblBin.BackColor = Color.DarkBlue;
                    lblBin.ForeColor = Color.AliceBlue;
                    lblBin.Location = new System.Drawing.Point(lblBinStartX, lblstartY + (j - 1) * interval);
                    panelSpool.Controls.Add(lblBin);
                }

                //防止控件叠加
                int lblWidth = labels.Width + lblstartX;
                if (lblWidth > txtstartX)
                {
                    txts.Location = new System.Drawing.Point(lblWidth + 3, txtBtnstartY + (j - 1) * interval);
                    txts.Width = txts.Width - (lblWidth - txtstartX) - 3;
                }
                if (j == 1)
                {
                    //20230809  针对一个FG绑多个子码时，有需要对FG码进行重置，提高产线效率
                    //属特殊需求，只针对站点配置使用
                    string output = string.Empty;
                    string res = PartSelectSVC.GetMesStationAndStationTypeDetail(List_Login.StationID, "IsShowReleaseBtn", "", out output);

                    if (res == "1" && output == "1")
                    {
                        SimpleButton btnFG = new SimpleButton();
                        btnFG.Text = "Release(释放)";
                        btnFG.Name = "btnFG" + j.ToString();
                        btnFG.Tag = 2;
                        btnFG.Size = new System.Drawing.Size(97, 30);
                        btnFG.Location = new System.Drawing.Point(btnStartX, txtBtnstartY + (j - 1) * interval);
                        btnFG.Click += BtnFG_Click;
                        btnFG.TabIndex = int.MaxValue - 1;
                        panelSpool.Controls.Add(btnFG);
                    }
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
                    if (scanType == "3" || scanType == "6")
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
        [Time("Txt Input")]
        private void Txts_KeyDown(object sender, KeyEventArgs e)
        {
            TextEdit txtPub = (TextEdit)sender;

            if (e.KeyCode == Keys.Enter && txtPub.Enabled)
            {
                string tmpSN = txtPub.Text.Trim();
                DateTime startTime = DateTime.Now;
                Tmp_Txts_keyDown(txtPub);
                DateTime endTime = DateTime.Now;
                MessageInfo.Add_Info_MSG(Edt_MSG, "Time", $"[{startTime.ToString("yyyy-MM-dd HH:mm:ss fff")}] --> [{endTime.ToString("yyyy-MM-dd HH:mm:ss fff")}] {tmpSN}  elapsed {(endTime - startTime).Milliseconds} ms");
            }
        }

        private void TimeLog(string SN, string Type, ref DateTime startTime)
        {
            if (enableDetailLog == "1")
            {
                DateTime finishDateTime = DateTime.Now;
                LogNet.LogDug("Detail Time ", $"{SN} [{Type}] elapsed {(finishDateTime - startTime).Milliseconds} ms");
                startTime = DateTime.Now;
            }
        }

        private void SendAutoCmd(bool isPass = false)
        {
            if (S_Is_AutoStation != "1" || !port1.IsOpen)
                return;
            try
            {
                port1.Write(isPass ? S_PASS_cmd : S_FAIL_cmd);
                MessageInfo.Add_Info_MSG(Edt_MSG, isPass ? "60101" : "60102", "OK", List_Login.Language);
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, isPass ? "60105" : "60106", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void Tmp_Txts_keyDown(TextEdit txtPub)
        {
            string tmpSN = txtPub.Text.Trim();
            DateTime startDateTime = DateTime.Now;
            if (txtPub.Name == "txt1")
            {
                string outString1 = string.Empty;
                string mainSN = txtPub.Text.Trim();
                if (string.IsNullOrEmpty(mainSN))
                {
                    SendAutoCmd();
                    return;
                }

                
                if (S_IsMainSNUPC == "1")
                {
                    DataSet dts = PartSelectSVC.GetmesSerialNumber(mainSN);
                    if (dts == null || dts.Tables.Count == 0 || dts.Tables[0].Rows.Count == 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20242", "NG", List_Login.Language, new string[] { mainSN });
                        txtPub.Text = string.Empty;
                        txtPub.Focus();
                        return;
                    }
                    string SerialNumberType = dts.Tables[0].Rows[0]["SerialNumberTypeID"].ToString();

                    //校验是否满足unitstate状态
                    if (SerialNumberType == "6")
                    {
                        //转换FG条码检测
                        string FGSN = mesUnitDetailSVC.MesGetFGSNByUPCSN(mainSN);
                        if (string.IsNullOrEmpty(FGSN))
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20189", "NG", List_Login.Language);
                            txtPub.Text = string.Empty;
                            txtPub.Focus();
                            return;
                        }

                        S_UPC_SN = mainSN;
                        txtPub.Text = FGSN;
                        mainSN = FGSN;
                        tmpSN = FGSN;

                    }
                }

                DataSet dsMainSN;
                dsMainSN = PartSelectSVC.uspCallProcedure("uspGetBaseData", mainSN,
                       null, null, null, null, null, ref outString1);
                TimeLog(tmpSN, "BaseData", ref startDateTime);
                if (outString1 != "1" || dsMainSN == null || dsMainSN.Tables[0].Rows.Count <= 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20119", "NG", List_Login.Language, new string[] { "SN:" + mainSN });
                    txtPub.Text = string.Empty;
                    SendAutoCmd();
                    return;
                }

                DataTable dt = dsMainSN.Tables[0];
                S_ProductionOrderID = dt.Rows[0]["ProductionOrderID"].ToString();
                S_PartID = dt.Rows[0]["PartID"].ToString();
                S_PartFamilyID = dt.Rows[0]["PartFamilyID"].ToString();
                S_PartFamilyType = dt.Rows[0]["PartFamilyTypeID"].ToString();

                //max.xie 验证料号是否一致 2021-11-22 tony 修改判断
                //if (IsNoPOCheck == "0")
                if (S_IsCheckPO == "0" && S_IsCheckPN == "1")
                {
                    if (S_PartID != Com_Part.EditValue.ToString())
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20043", "NG", List_Login.Language, new string[] { "SN:" + mainSN });
                        txtPub.Focus();
                        txtPub.Text = string.Empty;
                        SendAutoCmd();
                        return;
                    }
                }

                //工单数量检查
                //string outString = string.Empty;
                //PartSelectSVC.uspCallProcedure("uspPONumberCheck", S_ProductionOrderID,
                //        null, null, null, null, "1", ref outString);
                //if (outString != "1")
                //{
                //    string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                //    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_ProductionOrderID, ProMsg }, outString);
                //    txtPub.Text = string.Empty;
                //    return;
                //}


                //string PO = Com_PO.EditValue.ToString();
                DataSet dsCOF = PartSelectSVC.GetluPODetailDef(Convert.ToInt32(S_ProductionOrderID), "COF");
                TimeLog(tmpSN, "PODetail", ref startDateTime);
                if (dsCOF != null && dsCOF.Tables.Count > 0 && dsCOF.Tables[0].Rows.Count > 0)
                {
                    COF = dsCOF.Tables[0].Rows[0]["Content"].ToString() == "1";
                }
                string IsShowBin = "0";
                //20240405 
                {
                    int I_PartID = Convert.ToInt32(dsBOM.Tables[0].Rows[0]["PartID"].ToString());
                    DataSet dataSet = PartSelectSVC.GetmesPartDetail(I_PartID, "IsShowBin");
                    TimeLog(tmpSN, "PartDetail", ref startDateTime);
                    if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        IsShowBin = dataSet.Tables[0].Rows[0]["Content"].ToString();
                    }
                }

                string[] TagPro = new string[]
               {
                        dsBOM.Tables[0].Rows[0]["PartID"].ToString(),             //料号
                        dsBOM.Tables[0].Rows[0]["ScanType"].ToString(),           //扫描类型
                        dsBOM.Tables[0].Rows[0]["Pattern"].ToString(),            //校验正则
                        dsBOM.Tables[0].Rows[0]["FieldName"].ToString(),          //存储字段
                        "0" ,//是否锁定
                        S_ProductionOrderID,
                        IsShowBin
               };
                txtPub.Tag = TagPro;



                if (txtPub.Text == TestSN)
                {
                    if (IsAuto == "1")
                    {
                        try
                        {
                            port1.Write(ReadCmd);
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60108", "OK", List_Login.Language);
                            txtPub.SelectAll();
                            return;
                            //System.Threading.Thread.Sleep(500);
                        }
                        catch (Exception ex)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "60109", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                            txtPub.SelectAll();
                            return;
                        }

                    }
                }
            }

            //////////
            /// 20240405
            string[] tmpArr = txtPub.Tag as string[];
            if (tmpArr.Any() && tmpArr.Length > 6 && tmpArr[6] == "1")
            {
                string tmpBin = public_.GetBatchBin(txtPub.EditValue.ToString(), PartSelectSVC);
                var lblBs = panelSpool.Controls.Find("lbcB" + txtPub.Name.Substring(3, txtPub.Name.Length - 3), true);
                if (lblBs.Any() && lblBs.Length == 1)
                {
                    lblBs[0].Text = tmpBin;
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "NG", "未找到Bin展示控件。。");
                    SendAutoCmd();
                    return;
                }
            }
            ////////////


            string btnName = txtPub.Name.Substring(3, txtPub.Name.Length - 3);
            bool IsChooseCotrol = false;
            bool Check = CheckInputBarcode(txtPub);

            if (Check && txtPub.Enabled)
            {
                //if (IsNoPNCheck == "1")
                if (S_IsCheckPN == "0")
                {
                    if (txtPub.Name == "txt1")
                    {
                        CreateControl(dsBOM.Tables[0], S_ProductionOrderID);
                    }
                }

                txtPub.Enabled = false;
                IsChooseCotrol = true;
                if (CheckControlInput())
                {
                    if (IsAuto == "1")
                    {
                        if (IsBindAdhesive == "1")
                        {
                            try
                            {
                                port1.Write(ReadCmd);
                                MessageInfo.Add_Info_MSG(Edt_MSG, "60108", "OK", List_Login.Language);
                                //System.Threading.Thread.Sleep(500);
                            }
                            catch (Exception ex)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "60109", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                return;
                            }
                        }


                        if (!SubmitData())
                        {
                            if (IsNFC == "1")
                            {
                                try
                                {
                                    port1.Write("FAIL");
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "FAIL command sent successfully", "OK", List_Login.Language);
                                    //System.Threading.Thread.Sleep(500);
                                }
                                catch (Exception ex)
                                {
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "Failed to send FAIL instruction", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                    return;
                                }
                            }
                            //tempValue = "";
                            txtPub.Text = "";
                            txtPub.Enabled = true;
                            SendAutoCmd();
                            return;
                        }
                        else
                        {
                            SendAutoCmd(true);
                            if (IsNFC == "1")
                            {
                                try
                                {
                                    port1.Write("PASS");
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "PASS command sent successfully", "OK", List_Login.Language);
                                    //System.Threading.Thread.Sleep(500);
                                }
                                catch (Exception ex)
                                {
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "Failed to send PASS instruction", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!SubmitData())
                        {
                            txtPub.Text = "";
                            txtPub.Enabled = true;
                            SendAutoCmd();
                            return;
                        }
                        SendAutoCmd(true);
                    }
                }
                else
                {
                    SendAutoCmd(true);
                }
            }
            else
            {
                SendAutoCmd();
                txtPub.Text = "";
                txtPub.Enabled = true;
                if (txtPub.Name == "txt1")
                {
                    if (IsAuto == "1" && IsNFC == "1")
                    {
                        try
                        {
                            port1.Write("FAIL");
                            MessageInfo.Add_Info_MSG(Edt_MSG, "FAIL command sent successfully", "OK", List_Login.Language);
                            //System.Threading.Thread.Sleep(500);
                        }
                        catch (Exception ex)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "Failed to send FAIL instruction", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                            return;
                        }
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

        private void BtnFG_Click(object sender, EventArgs e)
        {
            TextEdit tb = panelSpool.Controls.Find("txt1", true).FirstOrDefault() as TextEdit;
            if (tb is null || tb.Enabled || string.IsNullOrEmpty(tb.Text))
                return;

            int totalCount = 0;
            int scanFinish = 0;
            //如果没有需要输入的框，不允许点击
            foreach (Control ctr in panelSpool.Controls)
            {
                if (ctr is TextEdit)
                {
                    Console.WriteLine(ctr.Text);
                    Console.WriteLine(ctr.Enabled);
                    totalCount++;
                    if (!ctr.Enabled && !string.IsNullOrEmpty(ctr.Text))
                    {
                        scanFinish++;
                    }
                }
            }

            if (scanFinish == totalCount)
                return;

            string[] TagPro = tb.Tag as string[];
            tb.Text = string.Empty;
            TagPro[4] = "0";
            tb.Tag = TagPro;
            tb.Enabled = true;
            tb.Focus();
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
                DateTime dateStart = DateTime.Now;
                DateTime dateStart2 = DateTime.Now;
                string barcode = txtBox.Text.Trim();
                string[] strList = txtBox.Tag as string[];
                string Pattern = strList[2].ToString();
                string ScanType = strList[1].ToString();
                string PartID = strList[0].ToString();
                string Result = "1";
                string xmlPartStr = "<Part PartID=\"" + PartID + "\"> </Part>";

                string ProductionOrderID = Com_PO.EditValue.ToString();
                //if (IsNoPOCheck == "1")
                if (S_IsCheckPO == "0")
                {
                    ProductionOrderID = strList[5].ToString();
                }


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
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20027", "NG", List_Login.Language, new string[] { "SN:" + barcode });
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
                            TimeLog(barcode, "CheckMianSN", ref dateStart2);

                        }
                        else
                        {
                            Result = PartSelectSVC.MESAssembleCheckOtherSN(barcode, PartID, COF);
                            TimeLog(barcode, "CheckOther", ref dateStart2);
                        }
                        break;
                    case "2":
                    case "7":
                    case "8":
                        //批次验证
                        PartSelectSVC.uspCallProcedure("uspBatchDataCheck", barcode,
                                                                                null, xmlPartStr, null, null, "1", ref Result);
                        TimeLog(barcode, "BatchData", ref dateStart2);
                        if (Result != "1")
                        {
                            if (IsAuto == "1" && IsNFC == "1")
                            {
                                if (Result == "20177")
                                {
                                    try
                                    {
                                        System.Threading.Thread.Sleep(500);
                                        port1.Write("READ");
                                        MessageInfo.Add_Info_MSG(Edt_MSG, "READ command sent successfully", "OK", List_Login.Language);
                                        //System.Threading.Thread.Sleep(500);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageInfo.Add_Info_MSG(Edt_MSG, "Failed to send READ instruction", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                                    }
                                }

                            }

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
                        TimeLog(barcode, "MachineTooling", ref dateStart2);

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
                            TimeLog(barcode, "BatchData", ref dateStart2);

                            if (Result != "1")
                            {
                                string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { barcode, ProMsg }, Result);
                                txtBox.Text = "";
                                return false;
                            }

                            if (!SearchBatch(txtBox.Name))
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20028", "NG", List_Login.Language, new string[] { "SN:" + barcode });
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

                        if (ScanType == "4" && Result == "1")
                        {
                            //20240410  通过治具SN查找并添加
                            var tmpParentId = public_.QueryParentTooling(barcode, PartSelectSVC);
                            TimeLog(barcode, "ParentTooling", ref dateStart2);

                            if (!string.IsNullOrEmpty(tmpParentId.Item1) && tmpParentId.Item1 != "0")
                            {
                                if (string.IsNullOrEmpty(tmpParentId.Item2) || string.IsNullOrEmpty(tmpParentId.Item3))
                                {
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"can't found Parent Info, please check machine setting.(无法找到父治具信息，请检查治具设定) {tmpParentId.Item2} {tmpParentId.Item3}");
                                    txtBox.Text = "";
                                    return false;
                                }

                                ToolingInfo ti = new ToolingInfo()
                                {
                                    ChildPartID = int.Parse(PartID),
                                    ChlidSN = barcode,
                                    ParentID = int.Parse(tmpParentId.Item1),
                                    ParentPartID = int.Parse(tmpParentId.Item2),
                                    ParentSN = tmpParentId.Item3
                                };

                                int parentIndex = toolingInfos.FindIndex(x =>
                                    x.ParentPartID == ti.ParentPartID && x.ParentID == ti.ParentID);
                                if (parentIndex >= 0)
                                {
                                    toolingInfos[parentIndex] = ti;
                                }
                                else
                                {
                                    toolingInfos.Add(ti);
                                }
                            }
                        }



                        //if (ScanType == "4" && Result == "1")
                        //{
                        //    //2024-03-29 针对4类型增加检查，是否存在子母治具
                        //    var tmpParentPartId = public_.CheckBindParentPart(int.Parse(PartID), int.Parse(Com_Part.EditValue.ToString()),
                        //        List_Login.StationTypeID, PartSelectSVC);

                        //    if (!string.IsNullOrEmpty(tmpParentPartId) && tmpParentPartId != "0")
                        //    {
                        //        //当前条码对应料号为子料，且存在绑定主治具,检查是否已经扫描主码
                        //        var isScannedParent = CheckParentControlInput(tmpParentPartId);
                        //        if (!isScannedParent)
                        //        {
                        //            var parentPart = mesPartSVC.Get(int.Parse(tmpParentPartId));
                        //            MessageInfo.Add_Info_MSG(Edt_MSG,"NG",$"please scanned it first(请先扫描主料号) {parentPart.PartNumber}");
                        //            txtBox.Text = "";
                        //            return false;
                        //        }
                        //    }
                        //}
                        break;
                    case "5":
                        //校验是否存在重复条码
                        bool check = mesUnitComponentSVC.MESCheckChildSerialNumber(barcode);
                        TimeLog(barcode, "CheckChild", ref dateStart2);

                        if (!check)
                        {
                            MessageInfo.Add_Info_MSG(Edt_MSG, "20029", "NG", List_Login.Language, new string[] { "SN:" + barcode });
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

                string MainSN = string.Empty;
                foreach (Control ctr in panelSpool.Controls)
                {
                    if (ctr.Name == "txt1")
                    {
                        MainSN = ctr.Text.ToString();
                        break;
                    }
                }
                string xmlExtraDataAss = "<ExtraData MainCode=\"" + MainSN + "\"> </ExtraData>";

                //调用通用过程
                string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + ProductionOrderID + "\"> </ProdOrder>";
                string xmlPart = "<Part PartID=\"" + S_PartID + "\"> </Part>";
                string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                string outString = string.Empty;
                PartSelectSVC.uspCallProcedure("uspAssembleCheck", barcode,
                                                                        xmlProdOrder, xmlPart, xmlStation, xmlExtraDataAss, PartID, ref outString);
                TimeLog(barcode, "AssembleCheck", ref dateStart2);

                if (outString != "1")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { barcode, ProMsg }, outString);
                    txtBox.Text = "";
                    return false;
                }
                TimeSpan ts = DateTime.Now - dateStart;
                double mill = Math.Round(ts.TotalMilliseconds, 0);
                //MessageInfo.Add_Info_MSG(Edt_MSG, "OK", $"check input barcode time :{mill.ToString()} ");
                return true;
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { "check input : " + ex.Message.ToString() });
                return false;
            }
        }

        public bool CheckParentControlInput(string parentPartId)
        {
            bool result = false;
            foreach (Control ctr in panelSpool.Controls)
            {
                if (ctr.Name.Substring(0, 3) == "txt")
                {
                    var tmpArr = ctr.Tag as string[];
                    if (tmpArr != null && tmpArr.Length > 0)
                    {
                        if (tmpArr[0] == parentPartId && !string.IsNullOrEmpty(ctr.Text) && !ctr.Enabled)
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        public bool CheckParentControlInput(ToolingInfo toolingInfo)
        {
            string tmpParentSN = string.Empty;
            string tmpSN = string.Empty;
            foreach (Control ctr in panelSpool.Controls)
            {
                if (ctr.Name.Substring(0, 3) == "txt")
                {
                    var tmpArr = ctr.Tag as string[];
                    if (tmpArr != null && tmpArr.Length > 0)
                    {
                        if (tmpArr[0] == toolingInfo.ParentPartID.ToString() && ctr.Text == toolingInfo.ParentSN)
                        {
                            tmpParentSN = ctr.Text;
                        }

                        if (tmpArr[0] == toolingInfo.ChildPartID.ToString() && ctr.Text == toolingInfo.ChlidSN)
                        {
                            tmpSN = ctr.Text;
                        }
                    }
                }
            }

            return !string.IsNullOrEmpty(tmpParentSN) && !string.IsNullOrEmpty(tmpSN);
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
            DateTime dateStart2 = DateTime.Now;
            bool IsFouce = false;
            List<mesUnit> List_mesUnit = new List<mesUnit>();
            List<mesHistory> List_mesHistory = new List<mesHistory>();
            List<mesUnitComponent> List_mesUnitComponent = new List<mesUnitComponent>();

            List<mesMaterialConsumeInfo> List_mesMaterialConsumeInfo = new List<mesMaterialConsumeInfo>();
            List<mesMachine> List_mesMachine = new List<mesMachine>();


            mesUnit v_mesUnit = new mesUnit();
            string msg = string.Empty;
            //string S_POID = Com_PO.EditValue.ToString();
            //string S_PartID = Com_Part.EditValue.ToString();
            //string S_PartFamilyID = Com_PartFamily.EditValue.ToString();

            //if (IsNoPOCheck == "0")
            if (S_IsCheckPO == "1")
            {
                S_ProductionOrderID = Com_PO.EditValue.ToString();
                S_PartID = Com_Part.EditValue.ToString();
                S_PartFamilyID = Com_PartFamily.EditValue.ToString();
            }

            string S_UnitStateID = public_.GetluUnitStatusID(S_PartID, S_PartFamilyID, List_Login.StationTypeID,
                List_Login.LineID.ToString(), S_ProductionOrderID, Com_luUnitStatus.EditValue.ToString());
            if (string.IsNullOrEmpty(S_UnitStateID))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20203", "NG", List_Login.Language);
                return false;
            }
            int MainUnitID = 0;


            //20240410 检查父子关系
            if (toolingInfos.Any())
            {
                foreach (var toolingInfo in toolingInfos)
                {
                    if (!CheckParentControlInput(toolingInfo))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "NG", $"parent-child relationship filed.（子母治具检查失败) {toolingInfo.ChlidSN} {toolingInfo.ParentSN}");
                        //提交时再检查父子关系，无法定位输入框，则全部清空
                        foreach (Control ctr in panelSpool.Controls)
                        {
                            if (ctr.Name.Substring(0, 3) == "txt")
                            {
                                TextEdit txt = ctr as TextEdit;
                                string[] tagList = ctr.Tag as string[];
                                //string Batch = "";
                                //string S_SN = txt.Text.Trim();
                                if (tagList[4].ToString() != "1")
                                {
                                    txt.Enabled = true;
                                    txt.Text = string.Empty;
                                    if (!IsFouce)
                                    {
                                        txt.Focus();
                                        IsFouce = true;
                                    }
                                    //////////////////////////////////////////////////////////
                                    /// 20240405
                                    if (tagList.Length > 6 && tagList[6] == "1")
                                    {
                                        var lblBs = panelSpool.Controls.Find("lbcB" + txt.Name.Substring(3, txt.Name.Length - 3), true);
                                        if (lblBs.Any() && lblBs.Length == 1)
                                        {
                                            lblBs[0].Text = "";
                                        }
                                    }
                                    /////////////////////////////////////////////////
                                }
                            }

                        }
                        return false;
                    }
                }
            }

            TimeLog("submit", "unit status - parent", ref dateStart2);

            //增加发送命令            
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

                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20030", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                                    return false;
                                }
                                MainUnitID = Convert.ToInt32(dataSetMachine.Tables[0].Rows[0]["UnitID"].ToString());
                            }
                            else
                            {
                                DataTable DT_UPUnitID = PartSelectSVC.Get_UnitID(S_SN).Tables[0];
                                MainUnitID = Convert.ToInt32(DT_UPUnitID.Rows[0]["UnitID"].ToString());


                            }

                            mesUnit F_mesUnit = new mesUnit();
                            F_mesUnit.ID = MainUnitID;
                            F_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                            F_mesUnit.StatusID = 1;
                            F_mesUnit.StationID = List_Login.StationID;
                            F_mesUnit.EmployeeID = List_Login.EmployeeID;
                            F_mesUnit.ProductionOrderID = Convert.ToInt32(S_ProductionOrderID);
                            List_mesUnit.Add(F_mesUnit);
                        }

                        mesUnit mesUnit_Part = mesUnitSVC.Get(MainUnitID);
                        string S_POPartID = mesUnit_Part.PartID.ToString();
                        //DT_ProductionOrder.Rows[0]["PartID"].ToString();

                        mesHistory F_mesHistory = new mesHistory();
                        F_mesHistory.UnitID = MainUnitID;
                        F_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                        F_mesHistory.EmployeeID = List_Login.EmployeeID;
                        F_mesHistory.StationID = List_Login.StationID;
                        F_mesHistory.ProductionOrderID = Convert.ToInt32(S_ProductionOrderID);
                        F_mesHistory.PartID = Convert.ToInt32(S_POPartID);
                        F_mesHistory.LooperCount = 1;
                        F_mesHistory.StatusID = 1;
                        F_mesHistory.EnterTime = DateTime.Now;

                        List_mesHistory.Add(F_mesHistory);

                        if (S_IsMainSNUPC == "1" && !string.IsNullOrEmpty(S_UPC_SN))
                        {
                            DataTable DT_UPCUnitID = PartSelectSVC.Get_UnitID(S_UPC_SN).Tables[0];
                            int upcUnitID = Convert.ToInt32(DT_UPCUnitID.Rows[0]["UnitID"].ToString());

                            mesUnit upc_mesUnit = new mesUnit();
                            upc_mesUnit.ID = upcUnitID;
                            upc_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                            upc_mesUnit.StatusID = 1;
                            upc_mesUnit.StationID = List_Login.StationID;
                            upc_mesUnit.EmployeeID = List_Login.EmployeeID;
                            upc_mesUnit.ProductionOrderID = Convert.ToInt32(S_ProductionOrderID);
                            List_mesUnit.Add(upc_mesUnit);

                            mesHistory upc_mesHistory = new mesHistory();
                            upc_mesHistory.UnitID = upcUnitID;
                            upc_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                            upc_mesHistory.EmployeeID = List_Login.EmployeeID;
                            upc_mesHistory.StationID = List_Login.StationID;
                            upc_mesHistory.ProductionOrderID = Convert.ToInt32(S_ProductionOrderID);
                            upc_mesHistory.PartID = Convert.ToInt32(S_POPartID);
                            upc_mesHistory.LooperCount = 1;
                            upc_mesHistory.StatusID = 1;
                            upc_mesHistory.EnterTime = DateTime.Now;
                            List_mesHistory.Add(upc_mesHistory);
                        }

                        msg = "Mian Code:" + txt.Text.ToString();
                    }
                    else
                    {
                        mesUnitComponent v_mesUnitComponent = new mesUnitComponent();

                        if (ScanType == "2" || ScanType == "5" || ScanType == "7" || ScanType == "8")
                        {
                            v_mesUnitComponent.UnitID = MainUnitID;
                            v_mesUnitComponent.UnitComponentTypeID = 1;
                            v_mesUnitComponent.ChildUnitID = 0;
                            if (ScanType == "7" || ScanType == "8")
                            {
                                DataSet Pss = PartSelectSVC.GetmesSerialNumber(txt.Text.Trim());
                                if (Pss != null && Pss.Tables.Count > 0)
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
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20030", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                                    return false;
                                }
                                ChildUnitID = Convert.ToInt32(dataSetMachine.Tables[0].Rows[0]["UnitID"].ToString());
                            }
                            else if (ScanType == "4")
                            {
                                //子料是夹具，要找夹具的工单，如果夹具没有工单就默认0  2022-04-15 修改
                                DataTable DT_ChildPO = PartSelectSVC.GetPO(tagList[0].ToString(), "").Tables[0];
                                string S_TollPOID = "0";

                                if (DT_ChildPO != null && DT_ChildPO.Rows.Count > 0)
                                {
                                    S_TollPOID = DT_ChildPO.Rows[0]["Id"].ToString();
                                }
                                else
                                {
                                    S_TollPOID = "0";
                                }

                                string Result = VirtualBarCode(S_SN, tagList[0].ToString(), null, S_UnitStateID, S_TollPOID);
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
                                    MessageInfo.Add_Info_MSG(Edt_MSG, "20031", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                                    return false;
                                }
                                ChildUnitID = Convert.ToInt32(dataSetUnit.Tables[0].Rows[0]["UnitID"].ToString());
                            }

                            DataSet dsComponent = PartSelectSVC.GetComponent(ChildUnitID);
                            if (dsComponent == null || dsComponent.Tables.Count == 0 || dsComponent.Tables[0].Rows.Count == 0)
                            {
                                MessageInfo.Add_Info_MSG(Edt_MSG, "20032", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
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

                        List_mesUnitComponent.Add(v_mesUnitComponent);

                        msg = msg + " Child barcode:" + txt.Text.ToString();
                    }

                    if (tagList[1].ToString() == "3" || tagList[1].ToString() == "4" || tagList[1].ToString() == "6")
                    {
                        //mesMachineSVC.MesModMachineBySNStationTypeID(S_SN, List_Login.StationTypeID);
                        mesMachine v_mesMachine = new mesMachine();
                        v_mesMachine.SN = S_SN;
                        List_mesMachine.Add(v_mesMachine);
                    }

                    if (tagList[1].ToString() == "2" || tagList[1].ToString() == "7" || tagList[1].ToString() == "3" || tagList[1].ToString() == "8")
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

                        mesMaterialConsumeInfo v_mesMaterialConsumeInfo = new mesMaterialConsumeInfo();
                        v_mesMaterialConsumeInfo.ScanType = BatchType;
                        v_mesMaterialConsumeInfo.SN = SN;
                        v_mesMaterialConsumeInfo.MachineSN = MachineSN;
                        v_mesMaterialConsumeInfo.PartID = Convert.ToInt32(S_PartID);
                        v_mesMaterialConsumeInfo.ProductionOrderID = Convert.ToInt32(S_ProductionOrderID);

                        List_mesMaterialConsumeInfo.Add(v_mesMaterialConsumeInfo);

                        //string ResultStr = PartSelectSVC.ModMesMaterialConsumeInfo(List_Login, BatchType, SN, MachineSN,
                        //                Convert.ToInt32(S_PartID), Convert.ToInt32(S_ProductionOrderID));
                        //if (ResultStr != "1")
                        //{
                        //    string ProMsg = MessageInfo.GetMsgByCode(ResultStr, List_Login.Language);
                        //    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] {  txt.Text.Trim(), ProMsg }, ResultStr);
                        //}
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
                        //////////////////////////////////////////////////////////
                        /// 20240405
                        if (tagList.Length > 6 && tagList[6] == "1")
                        {
                            var lblBs = panelSpool.Controls.Find("lbcB" + txt.Name.Substring(3, txt.Name.Length - 3), true);
                            if (lblBs.Any() && lblBs.Length == 1)
                            {
                                lblBs[0].Text = "";
                            }
                        }
                        /////////////////////////////////////////////////
                    }
                }
            }
            TimeLog("submit", "loop data", ref dateStart2);

            /////////////////////////////////////////////////
            //20230710  针对在工艺路线中的最后一站，从主线最后一站A站到TT的B站，再从TT的B站回到主线的最后一站A站时，
            //当A站是组装站时，unitcomponent表中会增加子料的绑定记录,遂在这里增加判断，当子料与主料已存在绑定则则不插入
            string stationTypeId = List_Login.StationTypeID.ToString();

            //获取此料工序路径
            int I_RouteID = PartSelectSVC.GetRouteID(List_Login.LineID.ToString(), S_PartID, S_PartFamilyID.ToString(), S_ProductionOrderID.ToString());
            TimeLog("submit", "route id", ref dateStart2);

            if (I_RouteID == -1)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20195", "NG", List_Login.Language);
                return false;
            }

            DataTable DT_Route = public_.GetRouteType(I_RouteID, PartSelectSVC).Tables[0];
            TimeLog("submit", " route type", ref dateStart2);

            string S_RouteType = DT_Route.Rows[0]["RouteType"].ToString();
            bool isLastStation = false;
            if (S_RouteType == "1")
            {
                isLastStation = public_.IsDiagramSFCLastStation(stationTypeId, I_RouteID.ToString(), PartSelectSVC);
            }
            else
            {
                isLastStation =
                    public_.IsTableFCLastStation(stationTypeId, I_RouteID.ToString(), S_UnitStateID, PartSelectSVC);
            }
            TimeLog("submit", "last station", ref dateStart2);

            if (isLastStation)
            {
                List<mesUnitComponent> mucs = new List<mesUnitComponent>();
                foreach (mesUnitComponent unitComponent in List_mesUnitComponent)
                {
                    bool tmpExistComponent = public_.IsExistBindChildPart(unitComponent.UnitID,
                        unitComponent.ChildUnitID, stationTypeId, unitComponent.ChildLotNumber, unitComponent.ChildPartID.ToString(), PartSelectSVC);
                    if (tmpExistComponent)
                        continue;

                    unitComponent.StatusID = tmpExistComponent ? 0 : 1;
                    mucs.Add(unitComponent);
                }
                List_mesUnitComponent.Clear();
                List_mesUnitComponent = mucs;
                TimeLog("submit", "process exist", ref dateStart2);

            }



            mesUnit[] L_mesUnit = List_mesUnit.ToArray();
            mesHistory[] L_mesHistory = List_mesHistory.ToArray();
            mesUnitComponent[] L_mesUnitComponent = List_mesUnitComponent.ToArray();
            mesMaterialConsumeInfo[] L_mesMaterialConsumeInfo = List_mesMaterialConsumeInfo.ToArray();
            mesMachine[] L_mesMachine = List_mesMachine.ToArray();

            string ReturnValue = DataCommitSVC.SubmitDataUHC(L_mesUnit, L_mesHistory, L_mesUnitComponent,
                L_mesMaterialConsumeInfo, L_mesMachine, List_Login);
            TimeLog("submit", "submit data", ref dateStart2);

            if (ReturnValue != "OK")
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { "submit : " + ReturnValue });
                return false;
            }

            S_UPC_SN = string.Empty;
            toolingInfos.Clear();
            TimeSpan ts = DateTime.Now - dateStart;
            double mill = Math.Round(ts.TotalMilliseconds, 0);
            MessageInfo.Add_Info_MSG(Edt_MSG, "10005", "OK", List_Login.Language, new string[] { msg, mill.ToString() });
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


                    mesUnitDetail v_mesUnitDetail = new mesUnitDetail();
                    v_mesUnitDetail.reserved_01 = S_SN;
                    v_mesUnitDetail.reserved_02 = "";
                    v_mesUnitDetail.reserved_03 = "1";
                    v_mesUnitDetail.reserved_04 = "";
                    v_mesUnitDetail.reserved_05 = "";


                    mesSerialNumber v_mesSerialNumber = new mesSerialNumber();
                    v_mesSerialNumber.SerialNumberTypeID = 8;
                    v_mesSerialNumber.Value = New_SN;

                    string ReturnValue = DataCommitSVC.InsertUDS(v_mesUnit, v_mesUnitDetail, v_mesSerialNumber, S_SN);
                    S_InsertUnit = ReturnValue;

                    //if (ReturnValue.Substring(0,5) =="ERROR")
                    if (string.IsNullOrEmpty(ReturnValue) || (ReturnValue.Length >= 5 && ReturnValue.Substring(0, 5) == "ERROR"))
                    {
                        S_InsertUnit = "NG";
                        string ProMsg = MessageInfo.GetMsgByCode(ReturnValue, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { New_SN, ProMsg }, "", "");
                    }
                    else
                    {
                        ReturnValue = "SN:" + New_SN;
                        MessageInfo.Add_Info_MSG(Edt_MSG, "66000", "OK", List_Login.Language, new string[] { ReturnValue });
                    }
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
                    //v_mesUnit.CreationTime = DateTime.Now;
                    v_mesUnit.LastUpdate = DateTime.Now;
                    v_mesUnit.PanelID = 0;
                    v_mesUnit.LineID = List_Login.LineID;
                    v_mesUnit.ProductionOrderID = Convert.ToInt32(S_POID);
                    v_mesUnit.RMAID = 0;
                    v_mesUnit.PartID = Convert.ToInt32(S_PartID);
                    v_mesUnit.LooperCount = 1;
                    v_mesUnit.ID = Convert.ToInt32(S_InsertUnit);

                    string ReturnValue = DataCommitSVC.UpdatemesUnit(v_mesUnit);
                    if (ReturnValue != "OK")
                    {
                        S_InsertUnit = "NG";
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { "VirtualBarCode1 :" + ReturnValue });
                    }
                }
                return S_InsertUnit;
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { "VirtualBarCode2 :" + ex.Message.ToString() });
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

        private void Btn_Hide_Click(object sender, EventArgs e)
        {
            GrpControlPart.Visible = false;
            Btn_Hide.Visible = false;
            Btn_Show.Visible = true;
        }

        private void Btn_Show_Click(object sender, EventArgs e)
        {
            GrpControlPart.Visible = true;
            Btn_Hide.Visible = true;
            Btn_Show.Visible = false;
        }
    }

    public class ToolingInfo
    {
        public string ParentSN { get; set; }
        public int ParentPartID { get; set; }
        public int ParentID { get; set; }
        public string ChlidSN { get; set; }
        public int ChildPartID { get; set; }
        public int ChildID { get; set; }
    }
}