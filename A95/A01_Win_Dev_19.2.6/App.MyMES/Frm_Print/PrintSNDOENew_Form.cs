﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.Model;
using App.MyMES.PartSelectService;
using App.MyMES.mesUnitService;
using App.MyMES.mesSerialNumberService;
using App.MyMES.mesHistoryService;
using App.MyMES.mesProductionOrderService;
using App.MyMES.mesProductStructureService;
using App.MyMES.mesUnitComponentService;
using App.MyMES.mesUnitDefectService;
using App.MyMES.mesUnitDetailService;
using System.Text.RegularExpressions;
using App.MyMES.mesPartService;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using App.MyMES.mesRouteDetailService;
using App.MyMES.mesMachineService;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Threading;
using System.Diagnostics;

namespace App.MyMES
{
    public partial class PrintSNDOENew_Form : FrmBase2
    {
        public PrintSNDOENew_Form()
        {
            InitializeComponent();
        }
        LabelManager2.Application LabSN;
        DataTable DT_PrintSn;
        int I_PartID;

        [DllImport("User32.dll", EntryPoint = "SetParent")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
        private void PrintSNOffline_Form2_Load(object sender, EventArgs e)
        {
            try
            {
                LoadComBox();
                public_.OpenBartender(List_Login.StationID.ToString());
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            this.Com_LineGroup.Enabled = true;
            base.Btn_Refresh_Click(sender, e);
        }

        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            this.Com_LineGroup.Enabled = false;
            //获取参数1值
            DataSet dtSet = PartSelectSVC.GetProductionOrderDetailDef(Com_PO.EditValue.ToString());
            if (dtSet == null || dtSet.Tables.Count == 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20070", "NG", List_Login.Language);
                return;
            }
            DataTable dtDOE = dtSet.Tables[0];
            DataRow[] Parameter1 = dtDOE.Select("Description='DOE_Parameter1'");

            if (Parameter1.Count() == 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20240", "NG", List_Login.Language,new string[] { "DOE_Parameter1" });
                return;
            }

            string[] ListPara1 = Parameter1[0]["Content"].ToString().Split(',');
            foreach (string str in ListPara1)
            {
                this.comboxET.Properties.Items.AddRange(new object[] { str });
            }
            base.Btn_ConfirmPO_Click(sender, e);
        }

        private void LoadComBox()
        {
            List_Login = this.Tag as LoginList;
            ///////////////////////////////////////////////////////////////////////////////////////
            Get_TempletPath(true);
        }

        private void Get_TempletPath(Boolean IsMsg)
        {
            try
            {
                string S_StationTypeID = List_Login.StationTypeID.ToString();
                string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                string S_PartID = Com_Part.EditValue.ToString();
                string S_ProductionOrderID = Com_PO.EditValue.ToString(); 
                string S_LoginLineID = List_Login.LineID.ToString();

                string S_LabelPath = public_.GetLabelPath(PartSelectSVC, S_StationTypeID, S_PartFamilyID, S_PartID, S_ProductionOrderID, S_LoginLineID);
                if (S_LabelPath == "")
                {
                    if (IsMsg == true)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20097", "NG", List_Login.Language);
                    }
                }
                else
                {
                    Edt_Templet.Text = S_LabelPath;
                    if (S_LabelPath.Substring(0, 5) == "ERROR")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_LabelPath });
                        Edt_Templet.Text = "";
                    }
                }
            }
            catch
            { }
        }

        private void Btn_CreateSN_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(Edt_Templet.Text.Trim().Replace(";", "")))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20098", "NG", List_Login.Language);
                return;
            }

            if (!Public_.IsNumeric(Edt_Num.Text.Trim()))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20099", "NG", List_Login.Language);
                return;
            }

            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
            string S_PartID = Com_Part.EditValue.ToString();
            string S_ProductionOrderID = Com_PO.EditValue.ToString();
            string S_LoginLineID = List_Login.LineID.ToString();

            string S_LabelName = public_.GetLabelName(PartSelectSVC, S_StationTypeID, S_PartFamilyID, S_PartID, S_ProductionOrderID, S_LoginLineID);
            if (S_LabelName == "")
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20104", "NG", List_Login.Language);
                return;
            }

            int I_Num = Convert.ToInt32(Edt_Num.Text.Trim());
            if (I_Num > 200)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20109", "NG", List_Login.Language);
                return;
            }

            if (!Regex.IsMatch(txtNumbers.Text.Trim(), "^[0-9]*$"))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { "预计数量必须为正整数" }, "");
                return;
            }
            
            int surNumber = Convert.ToInt32(txtNumbers.Text.ToString());

            if(surNumber< I_Num)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { "预计数量不足以打印当前数量条码,请确认后重新设置!" }, "");
                return;
            }

            if (Com_LineGroup.Text.Trim() == "")
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20101", "NG", List_Login.Language);
                return;
            }

            if(txtSPCA.Text.Trim().Length!=4)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { "参数[CCCC]格式长度为4位" }, "");
                return;
            }

            if (comboxET.Text.Trim().Length != 2)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { "参数[ColoCode]格式长度为2位" }, "");
                return;
            }

            if (!Regex.IsMatch(txtBuild.Text.Trim(), "[0-9]"))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { "参数[Build]只能是0-9数字" }, "");
                return;
            }
            if (txtPP.Text.Trim().Length != 2)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { "参数[PP]格式长度为2位" }, "");
                return;
            }


            //工单数量检查
            string outString = string.Empty;
            PartSelectSVC.uspCallProcedure("uspPONumberCheck", Com_PO.EditValue.ToString(),
                    null, null, null, null, I_Num.ToString(), ref outString);
            if (outString != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Com_PO.EditValue.ToString(), ProMsg }, outString);
                return;
            }

            int num = Convert.ToInt32(Edt_Num.Text.Trim());
            string result = string.Empty;
            DataSet dsSN = new DataSet();

            mesUnit v_mesUnit = new mesUnit();
            v_mesUnit.UnitStateID = 1;
            v_mesUnit.StatusID = 1;
            v_mesUnit.StationID = List_Login.StationID;
            v_mesUnit.EmployeeID = List_Login.EmployeeID;
            v_mesUnit.CreationTime = DateTime.Now;
            v_mesUnit.LastUpdate = DateTime.Now;
            v_mesUnit.PanelID = 0;
            v_mesUnit.LineID = Convert.ToInt32(Com_LineGroup.EditValue.ToString());                                       //传入值为界面选择线别(注:FG条码生成不分线不用传值)
            v_mesUnit.ProductionOrderID = Convert.ToInt32(Com_PO.EditValue.ToString());
            v_mesUnit.RMAID = 0;
            v_mesUnit.PartID = I_PartID;
            v_mesUnit.LooperCount = 1;
            v_mesUnit.PartFamilyID = Convert.ToInt32(Com_PartFamily.EditValue);
            v_mesUnit.StatusID = 1;

            v_mesUnit.SerialNumberType = 5;  //FG SerialNumber

            string S_LineID = Grid_LineGroup.GetRowCellValue(Grid_LineGroup.GetSelectedRows()[0], "LineID").ToString();
            string S_PartFamilyTypeID = Com_PartFamilyType.EditValue.ToString();
            string S_Production0rder = "'<ProdOrder ProductionOrder=" + "\"" + Com_PO.EditValue.ToString() + "\"" + "> </ProdOrder>'";
            string S_Station = "'<Station StationID=" + "\"" + List_Login.StationID.ToString() + "\"" + "> </Station>'";
            string S_xmlPart = "'<Part PartID=" + "\"" + I_PartID + "\"" + "> </Part>'";
            string S_xmlExtraData = "'<ExtraData LineID=" + "\"" + S_LineID + "\"" +
                                             " PartFamilyTypeID=" + "\"" + S_PartFamilyTypeID + "\"" +
                                             " LineType=" + "\"" + "M" + "\"" +
                                             " ET=" + "\"" + comboxET.Text.Trim() + "\"" +
                                             " SPCA=" + "\"" + txtSPCA.Text.Trim().ToUpper() + "\"" +
                                             " PP=" + "\"" + txtPP.Text.Trim().ToUpper() + "\"" +
                                             " B=" + "\"" + txtBuild.Text.Trim() + "\"" + " > </ExtraData>'";

            result = PartSelectSVC.Get_CreateMesSN(SN_Pattern, List_Login,S_Production0rder, S_xmlPart, S_Station, S_xmlExtraData, v_mesUnit, num, ref dsSN);
            if (result == "1" && dsSN != null && dsSN.Tables.Count > 0)
            {
                DT_PrintSn = dsSN.Tables[0];
                DT_PrintSn.Columns.Add("CreateTime", typeof(string));
                DT_PrintSn.Columns.Add("PrintTime", typeof(string));
                foreach (DataRow dr in DT_PrintSn.Rows)
                {
                    dr["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                dataGridSN.DataSource = DT_PrintSn;
                for (int i = 0; i < this.dataGridSN.Columns.Count; i++)
                {
                    this.dataGridSN.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }


                string S_PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, S_LabelName, DT_PrintSn, I_PartID);
                if (S_PrintResult != "OK")
                {
                    string ProMsg = MessageInfo.GetMsgByCode(S_PrintResult, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_PrintResult);
                }
                else
                {
                    txtNumbers.Text = (Convert.ToInt32(txtNumbers.Text.Trim()) - num).ToString();
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10021", "OK", List_Login.Language);
                    Edt_Num.Text = "";

                    base.GetProductionCount();
                }
            }
            else
            {
                string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20106", "NG", List_Login.Language, new string[] { ProMsg }, result);
            }

        }

        private void Com_PO_EditValueChanged(object sender, EventArgs e)
        {
            Get_TempletPath(false);
        }

        private void Com_Part_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_Part = Com_Part.Text.Trim();

                if (S_Part != "")
                {
                    I_PartID = Convert.ToInt32(Com_Part.EditValue.ToString());

                    Get_TempletPath(false);
                }
            }
            catch
            {
            }
        }

        public override void Com_PartFamilyType_EditValueChanged(object sender, EventArgs e)
        {
            base.Com_PartFamilyType_EditValueChanged(sender, e);
            int I_PartFamilyID = Convert.ToInt32(Com_PartFamilyType.EditValue.ToString());
            public_.AddLineGroup(Com_LineGroup, Grid_LineGroup, "M", I_PartFamilyID);
            S_DefectTypeID = public_.GetDefectTypeID(Com_PartFamilyType.Text.Trim());
        }

        private void toggleSwitchLock_Toggled(object sender, EventArgs e)
        {
            if (toggleSwitchLock.EditValue.ToString() == "True")
            {
                comboxET.Enabled = false;
                txtBuild.Enabled = false;
                txtSPCA.Enabled = false;
                txtPP.Enabled = false;
                txtNumbers.Enabled = false;
            }
            else
            {
                comboxET.Enabled = true;
                txtBuild.Enabled = true;
                txtSPCA.Enabled = true;
                txtPP.Enabled = true;
                txtNumbers.Enabled = true;
            }
        }
    }
}