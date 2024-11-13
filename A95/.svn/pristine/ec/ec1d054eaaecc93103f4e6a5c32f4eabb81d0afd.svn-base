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
    public partial class ReworkV2_Form : FrmBase3
    {
        DataTable RwDt;
        string UnitState = string.Empty;
        string barcode = string.Empty;

        DataTable DT_SN=new DataTable();
        string S_SNPartID;
        string S_PartFamilyID;
        string S_ProductionOrderID;

        public ReworkV2_Form()
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

                DataSet DS_SN=new DataSet();
                string S_Sql = "";
                if (type == "1" || type=="2")
                {
                    //S_Sql="SELECT A.PartID FROM mesUnit A JOIN mesSerialNumber B ON A.Id=B.UnitID WHERE B.Value='" 
                    //    + barcode + "'";

                    S_Sql = @"SELECT B.PartID,B.ProductionOrderID,p.PartFamilyID
			                    FROM mesSerialNumber A 
			                    INNER JOIN mesUnit B ON A.UnitID=B.ID 
			                    inner join mesPart p on p.id=B.partID
			                WHERE Value='" + barcode + "'";

                }

                if (type == "3")
                {
                     S_Sql = @"SELECT TOP 1 C.PartID,C.ProductionOrderID,D.PartFamilyID
	                            FROM mesPackage A JOIN mesUnitDetail B ON A.ID = B.InmostPackageID
	                            JOIN mesUnit C ON B.UnitID = C.ID
	                            JOIN mesPart D ON C.PartID=D.ID
                            WHERE A.SerialNumber  = '" + barcode + "'";                    
                }

                if (type == "4")
                {
                     S_Sql = @" SELECT TOP 1 D.PartID,D.ProductionOrderID,E.PartFamilyID 
                            FROM mesPackage A JOIN mesPackage B ON A.ID=B.ParentID
                                JOIN mesUnitDetail C ON B.ID  =C.InmostPackageID
	                            JOIN mesUnit D ON C.UnitID=D.ID 
	                            JOIN mesPart E ON D.PartID=E.ID
                            WHERE A.SerialNumber='" + barcode + "'";
                    
                }

                DS_SN = public_.P_DataSet(S_Sql);
                if (DS_SN.Tables.Count > 0)
                {
                    if (DS_SN.Tables[0].Rows.Count > 0)
                    {
                        S_SNPartID = DS_SN.Tables[0].Rows[0]["PartID"].ToString();
                        S_PartFamilyID=DS_SN.Tables[0].Rows[0]["PartFamilyID"].ToString();
                        S_ProductionOrderID=DS_SN.Tables[0].Rows[0]["ProductionOrderID"].ToString();
                    }
                }


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

                    DT_SN = RwDt;

                    if (RwDt.Rows.Count < 1)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "No Data", "NG", List_Login.Language);
                        return;
                    }

                    btnPrint.Enabled = true;
                    btnRework.Enabled = true;
                }
                else
                {
                    string ProMsg = MessageInfo.GetMsgByCode(Result, List_Login.Language);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { barcode, ProMsg }, Result);
                    txtSN.SelectAll();
                    barcode = string.Empty;
                    gridControlViewDetail.DataSource = null;
                }
            }
        }

        private void btnRework_Click(object sender, EventArgs e)
        {
            if (RwDt == null || RwDt.Rows.Count == 0 || string.IsNullOrEmpty(barcode))
            {
                txtSN.Focus();
                txtSN.Text = "";
                string MSG = "ERROR:No data";
                string ProMsg = MessageInfo.GetMsgByCode(MSG, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, "");
               
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
                gridControlViewDetail.DataSource = null;
                txtSN.Text = "";

                btnPrint.Enabled = false;
                btnRework.Enabled = false;
            }
        }

        private void Btn_RefreshTarget_Click(object sender, EventArgs e)
        {
            FrmBase_Load(sender, e);
            gridControlViewDetail.DataSource = null;

            btnPrint.Enabled = false;
            btnRework.Enabled = false;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (DT_SN.Rows.Count<1)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20107", "NG", List_Login.Language);
                return;
            }

            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_LoginLineID = List_Login.LineID.ToString();
            string S_LabelName = public_.GetLabelName(PartSelectSVC, S_StationTypeID, S_PartFamilyID, S_SNPartID, S_ProductionOrderID, S_LoginLineID);
            if (string.IsNullOrEmpty(S_LabelName))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20097", "NG", List_Login.Language);
                return;
            }

            DataTable DT_PrintSn = new DataTable();
            DT_PrintSn.Columns.Add("SN", typeof(string));
            DT_PrintSn.Columns.Add("CreateTime", typeof(string));
            DT_PrintSn.Columns.Add("PrintTime", typeof(string));

            for (int i = 0; i < DT_SN.Rows.Count; i++)
            {
                DataRow dr = DT_PrintSn.NewRow();
                dr["SN"] =DT_SN.Rows[i]["SerialNumber"].ToString();
                dr["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                DT_PrintSn.Rows.Add(dr);
            }

            //打印 SN
            int I_SNPartID = 0;
            if (S_SNPartID != "")
            {
                I_SNPartID = Convert.ToInt32(S_SNPartID);
            }

            string S_PrintResult = Public_.PrintCodeSoftSN(PartSelectSVC, S_LabelName, DT_PrintSn, I_SNPartID);
            if (S_PrintResult != "OK")
            {
                string ProMsg = MessageInfo.GetMsgByCode(S_PrintResult, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, S_PrintResult);                
            }
            else
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "10021", "OK", List_Login.Language);

                btnPrint.Enabled = false;                
            }
        }
    }
}
