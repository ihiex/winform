using App.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace App.MyMES
{
    public partial class PrintSN_UPC_Form : FrmBase2
    {
        public PrintSN_UPC_Form()
        {
            InitializeComponent();
        }

        DataTable DT_PrintSn;

        int I_PartID;
        int I_POID;


        [DllImport("User32.dll", EntryPoint = "SetParent")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        private void GrpControlPart_Paint(object sender, PaintEventArgs e)
        {

        }

        public override void Com_PartFamilyType_EditValueChanged(object sender, EventArgs e)
        {
            base.Com_PartFamilyType_EditValueChanged(sender, e);
            int I_PartFamilyID = Convert.ToInt32(Com_PartFamilyType.EditValue.ToString());
            public_.AddLineGroup(Com_LineGroup, Grid_LineGroup, "K", I_PartFamilyID);
            S_DefectTypeID = public_.GetDefectTypeID(Com_PartFamilyType.Text.Trim());
        }

        public override void Com_Part_EditValueChanged(object sender, EventArgs e)
        {
            base.Com_Part_EditValueChanged(sender, e);
            string S_Part = Com_Part.Text.Trim();
            try
            {
                if (S_Part != "")
                {
                    Get_TempletPath(false);

                    I_PartID = Convert.ToInt32(Com_Part.EditValue.ToString());
                    //public_.AddPOAll(Com_PO, I_PartID.ToString(), List_Login.LineID.ToString(), Grid_PO);
                    //Com_PO_EditValueChanged(sender, e);
                }
            }
            catch(Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void Com_PO_EditValueChanged(object sender, EventArgs e)
        {
            Get_TempletPath(false);
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
            if (string.IsNullOrEmpty(Edt_Templet.Text.Trim()))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20098", "NG", List_Login.Language);
                return;
            }

            if (!Public_.IsNumeric(Edt_Num.Text.Trim()))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20099", "NG", List_Login.Language);
                return;
            }

            if (Convert.ToInt32(Edt_Num.Text.Trim()) > 200)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20100", "NG", List_Login.Language);
                return;
            }

            if (Com_LineGroup.Text.Trim() == "")
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20101", "NG", List_Login.Language);
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

            int num = Convert.ToInt32(Edt_Num.Text.Trim());

            //工单数量检查
            string outString = string.Empty;
            PartSelectSVC.uspCallProcedure("uspPONumberCheck", Com_PO.EditValue.ToString(),
                    null, null, null, null, num.ToString(), ref outString);
            if (outString != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(outString, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { Com_PO.EditValue.ToString(), ProMsg }, outString);
                return;
            }

            string result = string.Empty;
            try
            {
                DataSet dsSN = new DataSet();

                mesUnit v_mesUnit = new mesUnit();
                v_mesUnit.UnitStateID = 1;
                v_mesUnit.StatusID = 1;
                v_mesUnit.StationID = List_Login.StationID;
                v_mesUnit.EmployeeID = List_Login.EmployeeID;
                v_mesUnit.CreationTime = DateTime.Now;
                v_mesUnit.LastUpdate = DateTime.Now;
                v_mesUnit.PanelID = 0;
                v_mesUnit.LineID = Convert.ToInt32(Grid_LineGroup.GetRowCellValue(Grid_LineGroup.GetSelectedRows()[0], "LineID")); //Convert.ToInt32(Com_LineGroup.EditValue.ToString());                 //传入值为界面选择线别(注:FG条码生成不分线不用传值)
                v_mesUnit.ProductionOrderID = Convert.ToInt32(Com_PO.EditValue.ToString());
                v_mesUnit.RMAID = 0;
                v_mesUnit.PartID = I_PartID;
                v_mesUnit.LooperCount = 1;
                v_mesUnit.PartFamilyID = Convert.ToInt32(Com_PartFamily.EditValue);
                v_mesUnit.StatusID = 1;

                v_mesUnit.SNFamilyID = 10;        //UPC
                v_mesUnit.SerialNumberType = 6;   //UPC SerialNumber

                string S_LineID = Grid_LineGroup.GetRowCellValue(Grid_LineGroup.GetSelectedRows()[0], "LineID").ToString();
                string S_PartFamilyTypeID = Com_PartFamilyType.EditValue.ToString();

                string S_xmlPart = "'<Part PartID=" + "\"" + I_PartID + "\"" + "> </Part>'";
                string S_Production0rder = "'<ProdOrder ProductionOrder=" + "\"" + Com_PO.EditValue.ToString() + "\"" + "> </ProdOrder>'";
                string S_Station = "'<Station StationID=" + "\"" + List_Login.StationID.ToString() + "\"" + "> </Station>'";
                string S_xmlExtraData = "'<ExtraData LineID=" + "\"" + S_LineID + "\"" +
                                             " PartFamilyTypeID=" + "\"" + S_PartFamilyTypeID + "\"" +
                                             " LineType=" + "\"" + "K" + "\"" + " > </ExtraData>'";
                result = PartSelectSVC.Get_CreateMesSN(SN_Pattern, List_Login, S_Production0rder, S_xmlPart, S_Station, S_xmlExtraData, v_mesUnit, num, ref dsSN);
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
                        MessageInfo.Add_Info_MSG(Edt_MSG, "10021", "OK", List_Login.Language);
                        Edt_Num.Text = "";

                    }
                }
                else
                {
                    string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20106", "NG", List_Login.Language, new string[] { ProMsg }, result);
                }
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void PrintSN_UPC_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                PartSelectSVC.Close();
            }
            catch
            {

            }
        }

        private void PrintSN_UPC_Form_Load(object sender, EventArgs e)
        {
            try
            {
                LoadComBox();
                public_.OpenBartender(List_Login.StationID.ToString());
            }
            catch(Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                return;
            }
            
        }

        private void LoadComBox()
        {
            List_Login = this.Tag as LoginList;
            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_LineID = List_Login.LineID.ToString();

            string S_PartID = Com_Part.EditValue.ToString();
            public_.AddPOAll(Com_PO, S_PartID, List_Login.LineID.ToString(), Grid_PO);

            //////////////////////////////////////////////////////////////////////////////////////////////
            Get_TempletPath(true);
        }

    }
}
