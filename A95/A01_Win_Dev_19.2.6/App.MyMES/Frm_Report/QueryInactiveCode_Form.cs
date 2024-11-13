using App.Model;
using DevComponents.Editors;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGauges.Core.Drawing;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.MyMES
{
    public partial class QueryInactiveCode_Form : FrmBase3
    {
        public QueryInactiveCode_Form()
        {
            InitializeComponent();
        }

        public override void Btn_ConfirmPO_Click(object sender, EventArgs e)
        {
            if (Get_TempletPath())
            {
                base.Btn_ConfirmPO_Click(sender, e);
            }
        }

        public override void Btn_Refresh_Click(object sender, EventArgs e)
        {
            base.Btn_Refresh_Click(sender, e);
            gridCtlQuery.DataSource = null;
        }

        private bool Get_TempletPath()
        {
            try
            {
                string S_StationTypeID = List_Login.StationTypeID.ToString();
                string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
                string S_PartID = Com_Part.EditValue.ToString();
                string S_ProductionOrderID = "";
                string S_LoginLineID = List_Login.LineID.ToString();

                string S_LabelPath = public_.GetLabelPath(PartSelectSVC, S_StationTypeID, S_PartFamilyID, S_PartID, S_ProductionOrderID, S_LoginLineID);
                if (S_LabelPath == "")
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20097", "NG", List_Login.Language);
                    return false;
                }
                else
                {
                    if (S_LabelPath.Substring(0, 5) == "ERROR")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { S_LabelPath });
                        lblTemp.Text = "";
                        return false;
                    }
                    else
                    {
                        lblTemp.Text = S_LabelPath;
                        return true;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                return false;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + Com_PO.EditValue.ToString() + "\"> </ProdOrder>";
            string outStr = string.Empty;

            DataSet dsQuery = PartSelectSVC.uspCallProcedure("uspGetInactiveCode", null, xmlProdOrder, null, null, null, null, ref outStr);
            if (outStr != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(outStr, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { ProMsg });
                return;
            }
            else
            {
                if (dsQuery != null && dsQuery.Tables.Count > 0 && dsQuery.Tables[0].Rows.Count>0)
                {
                    gridCtlQuery.DataSource = dsQuery.Tables[0];
                    if(!gridView1.OptionsSelection.MultiSelect)
                    {
                        gridView1.OptionsSelection.MultiSelect = true;
                        gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
                    }
                }
                else
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20119", "NG", List_Login.Language);
                }
            }
            
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)gridCtlQuery.DataSource;
                if(dt==null || dt.Rows.Count==0)
                {
                    return;
                }
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "导出Excel";
                saveFileDialog.Filter = "Excel文件(*.xls)|*.xls";
                DialogResult dialogResult = saveFileDialog.ShowDialog(this);
                if (dialogResult == DialogResult.OK)
                {
                    DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                    gridCtlQuery.ExportToXls(saveFileDialog.FileName);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10028", "OK", List_Login.Language);
                }
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20169", "NG", List_Login.Language);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if(gridView1.DataSource==null || gridView1.GetSelectedRows().Length == 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20251", "NG", List_Login.Language);
                return;
            }

            Int32[] selectedRow = gridView1.GetSelectedRows();
            DataView dtView = (DataView)gridView1.DataSource;
            DataTable DT_PrintSn = new DataTable();
            DT_PrintSn.Columns.Add("SN", typeof(string));
            DT_PrintSn.Columns.Add("CreateTime", typeof(string));
            DT_PrintSn.Columns.Add("PrintTime", typeof(string));
            foreach (var q in selectedRow)
            {
                DataRow dr = DT_PrintSn.NewRow();
                dr["SN"] = dtView.Table.Rows[q]["SN"].ToString();
                dr["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DT_PrintSn.Rows.Add(dr);
            }

            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_PartFamilyID = Com_PartFamily.EditValue.ToString();
            string S_PartID = Com_Part.EditValue.ToString();
            string S_ProductionOrderID = "";
            string S_LoginLineID = List_Login.LineID.ToString();

            string S_LabelName = public_.GetLabelName(PartSelectSVC, S_StationTypeID, S_PartFamilyID, S_PartID, S_ProductionOrderID, S_LoginLineID);
            if (S_LabelName == "")
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20104", "NG", List_Login.Language);
                return;
            }

            string S_PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, S_LabelName, DT_PrintSn, Convert.ToInt32(S_PartID));
            if (S_PrintResult != "OK")
            {
                string ProMsg = MessageInfo.GetMsgByCode(S_PrintResult, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_PrintResult);
            }
            else
            {
                //记录日志
                foreach (DataRow dr in DT_PrintSn.Rows)
                {
                    DataSet dsSetPr = PartSelectSVC.GetmesSerialNumber(dr["SN"].ToString());
                    string unitID = dsSetPr.Tables[0].Rows[0]["UnitID"].ToString();
                    mesHistory v_mesHistory = new mesHistory();
                    v_mesHistory.UnitID = Convert.ToInt32(unitID);
                    v_mesHistory.UnitStateID = -200;
                    v_mesHistory.EmployeeID = List_Login.EmployeeID;
                    v_mesHistory.StationID = List_Login.StationID;
                    v_mesHistory.EnterTime = DateTime.Now;
                    v_mesHistory.ExitTime = DateTime.Now;
                    v_mesHistory.ProductionOrderID = Convert.ToInt32(Com_PO.EditValue.ToString());
                    v_mesHistory.PartID = Convert.ToInt32(Com_Part.EditValue.ToString());
                    v_mesHistory.LooperCount = 1;
                    mesHistorySVC.Insert(v_mesHistory);
                }
                MessageInfo.Add_Info_MSG(Edt_MSG, "10021", "OK", List_Login.Language);
                gridCtlQuery.DataSource = null;
            }
        }

        private void QueryInactiveCode_Form_Load(object sender, EventArgs e)
        {
            public_.OpenBartender(List_Login.StationID.ToString());
        }
    }
}