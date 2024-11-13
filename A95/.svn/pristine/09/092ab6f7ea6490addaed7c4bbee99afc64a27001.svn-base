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
    public partial class Rework_Form : FrmBase3
    {
        DataTable RwDt;
        string UnitState = string.Empty;
        string barcode = string.Empty;

        public Rework_Form()
        {
            InitializeComponent();
        }

        public override void FrmBase_Load(object sender, EventArgs e)
        {
            try
            {
                base.FrmBase_Load(sender, e);
                DataSet dsStationConfig = PartSelectSVC.GetmesStationConfig("", List_Login.StationID.ToString());
                if (dsStationConfig == null || dsStationConfig.Tables.Count == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20184", "NG", List_Login.Language);
                    return;
                }

                DataRow[] drConfig = dsStationConfig.Tables[0].Select("Name='OperationType'");
                if (drConfig == null || drConfig.Count() == 0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20184", "NG", List_Login.Language);
                    return;
                }
                string OperationType = drConfig[0]["Value"].ToString();
                if (OperationType == "SN" || OperationType == "UPC" || OperationType == "Multipack")
                {
                    drConfig = dsStationConfig.Tables[0].Select("Name='ReworkUnitStateID'");
                    if (drConfig == null || drConfig.Count() == 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20185", "NG", List_Login.Language);
                        return;
                    }
                    UnitState = drConfig[0]["Value"].ToString();
                }
                //查找类型
                foreach (DevExpress.XtraEditors.Controls.RadioGroupItem radio in radioGtOperationType.Properties.Items)
                {
                    if (radio.Tag.ToString() == OperationType)
                    {
                        radioGtOperationType.EditValue = radio.Value;
                        break;
                    }
                }
                GrpControlInputData.Enabled = true;
                this.txtSN.Text = string.Empty;
                txtSN.Focus();
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                return;
            }
        }

        private void txtSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                barcode = txtSN.Text.Trim();
                string type = radioGtOperationType.EditValue.ToString();
                string Result = string.Empty;
                string xmlExtraData = "<ExtraData UnitStateID=\"" + UnitState + "\"> </ExtraData>";
                DataSet dts = PartSelectSVC.uspCallProcedure("uspGetReworkData", barcode, null, null, null, xmlExtraData, type, ref Result);
                if (Result == "1")
                {
                    if (RwDt != null)
                    {
                        RwDt.Columns.Clear();
                        RwDt.Clear();
                    }
                    RwDt = dts.Tables[0];
                    RwDt.Columns.Add("Choose", typeof(bool));
                    foreach (DataRow dr in RwDt.Rows)
                    {
                        dr["Choose"] = true;
                    }
                    gridViewRework.Columns.Clear();
                    gridControlViewDetail.DataSource = RwDt;
                }
                else
                {
                    string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { barcode, ProMsg }, Result);
                    txtSN.SelectAll();
                    barcode = string.Empty;
                }
            }
        }

        private void btnRework_Click(object sender, EventArgs e)
        {
            if (RwDt == null || RwDt.Rows.Count == 0 || string.IsNullOrEmpty(barcode))
            {
                return;
            }

            string type = radioGtOperationType.EditValue.ToString();
            string Result = string.Empty;
            string xmlExtraData = "<ExtraData UnitStateID=\"" + UnitState + "\"" +
                                             " EmployeeID=" + "\"" + List_Login.EmployeeID + "\"" + "> </ExtraData>";
            string xmlStation = "<Station StationID=" + "\"" + List_Login.StationID.ToString() + "\"" + "> </Station>";
            PartSelectSVC.uspCallProcedure("uspSetReworkData", barcode, null, null, xmlStation, xmlExtraData, type, ref Result);
            if (Result != "1")
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, Result, "NG", List_Login.Language);
                txtSN.SelectAll();
            }
            else
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "10033", "OK", List_Login.Language, new string[] { barcode });
                txtSN.Text = "";
            }
        }
    }
}
