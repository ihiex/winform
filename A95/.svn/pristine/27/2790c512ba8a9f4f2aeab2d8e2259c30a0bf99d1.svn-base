using App.Model;
using App.MyMES.PartSelectService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.MyMES
{
    public partial class ORT_Form : DevExpress.XtraEditors.XtraForm
    {
        public LoginList List_Login = new LoginList();
        public Public_ public_ = new Public_();
        public PartSelectSVCClient PartSelectSVC;
        string MianResultID = string.Empty;
        int FouceIndex = 0;

        public ORT_Form()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取一年周次
        /// </summary>
        /// <param name="year"></param>
        private void BindingWeeks(int year)
        {
            comboxEditWeek.Properties.Items.Clear();
            DateTime the_Date = new DateTime(year, 1, 1);
            TimeSpan tt = the_Date.AddYears(1) - the_Date;
            int totalweeks = tt.Days / 7 + 1;
            for (int i = 1; i < totalweeks; i++)
            {
                comboxEditWeek.Properties.Items.Add(i);
            }
        }

        /// <summary>
        /// 获取当前周次
        /// </summary>
        /// <returns></returns>
        private int GetCurrentWeeks()
        {
            GregorianCalendar gc = new GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            return weekOfYear;
        }

        /// <summary>
        /// 获取设置的取样数量
        /// </summary>
        private void GetORTNumber()
        {
            try
            {
                if (Com_PartFamilyType.EditValue == null)
                    return;

                DataTable dtNumber = null;
                if (radioBtnDay.Checked)
                {
                    DataSet dsDay = PartSelectSVC.GetORTluPartFamilyType("ORTDayNumber", Com_PartFamilyType.EditValue.ToString());
                    if (dsDay != null && dsDay.Tables.Count > 0 && dsDay.Tables[0].Rows.Count > 0)
                        dtNumber = dsDay.Tables[0];
                }
                else if (radioBtnWeek.Checked)
                {
                    DataSet dsWeek = PartSelectSVC.GetORTluPartFamilyType("ORTWeekNumber", Com_PartFamilyType.EditValue.ToString());
                    if (dsWeek != null && dsWeek.Tables.Count > 0 && dsWeek.Tables[0].Rows.Count > 0)
                        dtNumber = dsWeek.Tables[0];
                }


                if (dtNumber != null && dtNumber.Rows.Count > 0)
                {
                    txtReQTY.Text = dtNumber.Rows[0]["Content"].ToString();
                }
                else
                {
                    txtReQTY.Text = string.Empty;
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                txtReQTY.Text = string.Empty;
            }
        }

        /// <summary>
        /// 获取ORT编号
        /// </summary>
        private void GetORTCode()
        {
            try
            {
                if (Com_PartFamilyType.EditValue == null)
                    return;

                string PartFamilyTypeID = Com_PartFamilyType.EditValue.ToString();
                string YearID = dateEditYear.DateTime.ToString("yyyy");
                string WeekID = comboxEditWeek.Text.ToString();

                if (!string.IsNullOrEmpty(PartFamilyTypeID) &&
                    !string.IsNullOrEmpty(YearID) && Convert.ToInt32(YearID) > DateTime.Now.Year - 1 &&
                    !string.IsNullOrEmpty(WeekID) && WeekID != "0")
                {
                    string dsOrtCOde = PartSelectSVC.GetORTCode(PartFamilyTypeID, YearID, WeekID);
                    if (!string.IsNullOrEmpty(dsOrtCOde))
                    {
                        txtORTCode.ReadOnly = true;
                        btnAddORTCode.Visible = false;
                        txtORTCode.Text = dsOrtCOde;

                    }
                    else
                    {
                        txtIniQTY.Text = "0";
                        txtORTCode.ReadOnly = false;
                        btnAddORTCode.Visible = true;
                        txtORTCode.Text = string.Empty;
                    }
                }
                else
                {
                    txtORTCode.ReadOnly = true;
                    btnAddORTCode.Visible = false;
                    txtORTCode.Text = string.Empty;
                }
            }
            catch(Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                txtORTCode.Text = string.Empty;
                txtORTCode.ReadOnly = true;
                btnAddORTCode.Visible = false;
            }
        }

        /// <summary>
        /// 获取批次号
        /// </summary>
        private void GetORTMaxBatch()
        {
            try
            {
                comboxEditORTBatch.Properties.Items.Clear();
                string ORTNO = txtORTCode.Text.Trim();
                comboxEditORTBatch.Properties.Items.Add("0");

                if (!string.IsNullOrEmpty(ORTNO))
                {
                    if (!string.IsNullOrEmpty(comboxEditORTBatch.Text.Trim()))
                        GetORTTestResult();

                    if (!radioBtnNull.Checked && !string.IsNullOrEmpty(ORTNO))
                    {
                        string TestTypeID = radioBtnDay.Checked ? "0" : "1";
                        DataSet ds = PartSelectSVC.GetORTMaxBatch(ORTNO, TestTypeID);
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {

                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                comboxEditORTBatch.Properties.Items.Add(dr["OrderNo"].ToString());
                            }
                        }
                    }
                }
                else
                {
                    gridCtrlORTMian.DataSource = null;
                    btnResult.Enabled = false;
                }
                comboxEditORTBatch.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                gridCtrlORTMian.DataSource = null;
                btnResult.Enabled = false;
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void ResetRegist()
        {
            txtSN.Text = string.Empty;
            txtSN.ReadOnly = true;
            txtStationType.Text = string.Empty;
            txtStatus.Text = string.Empty;
        }

        private void GetORTTestResult()
        {
            string ORTCode = txtORTCode.Text.Trim();
            string TestTypeID = string.Empty;
            if (!radioBtnNull.Checked)
            {
                TestTypeID = radioBtnDay.Checked ? "0" : "1";
            }

            if (string.IsNullOrEmpty(ORTCode))
            {
                return;
            }

            string result = string.Empty;
            string xmlExtraData = "<ExtraData ORTCode=\"" + ORTCode + "\" " +
                                              "OrderNo =\"" + comboxEditORTBatch.Text.ToString() + "\" " +
                                              "TestTypeID =\"" + TestTypeID + "\"> </ExtraData>";
            DataSet dsORT = PartSelectSVC.uspCallProcedure("uspORTGetData", "", "", "",
                                                                "", xmlExtraData, "1", ref result);
            if (result != "1" || dsORT == null || dsORT.Tables.Count == 0)
            {
                btnResult.Enabled = false;
                string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, result);
                return;
            }
            gridCtrlORTMian.DataSource = dsORT.Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Com_PartFamilyType"></param>
        /// <param name="v_gridView"></param>
        /// <param name="DT"></param>
        public void AddPartFamilyType(DevExpress.XtraEditors.SearchLookUpEdit Com_PartFamilyType,
                                      DevExpress.XtraGrid.Views.Grid.GridView v_gridView,DataTable DT)
        {
            try
            {
                Com_PartFamilyType.Properties.DataSource = DT;
                Com_PartFamilyType.Properties.DisplayMember = "Name";
                Com_PartFamilyType.Properties.ValueMember = "ID";

                Size v_Size = new Size(450, 350);
                Com_PartFamilyType.Properties.PopupFormSize = v_Size;
                v_gridView.RowHeight = 30;
                if (v_gridView.Columns.Count == 0)
                {
                    DevExpress.XtraGrid.Columns.GridColumn Column_ID = public_.DevColumnP("ID", "ID", 80, true);
                    DevExpress.XtraGrid.Columns.GridColumn Column_Name = public_.DevColumnP("Name", "Name", 300, true);
                    v_gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[]
                        {
                        Column_ID,
                        Column_Name
                        }
                    );
                }

                if (DT.Rows.Count > 0)
                {
                    int ID = Convert.ToInt32(DT.Rows[0]["ID"].ToString());
                    Com_PartFamilyType.EditValue = ID;
                }
            }
            catch (Exception ex)
            {
                Com_PartFamilyType.Properties.DataSource = null;
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void ORT_Form_Load(object sender, EventArgs e)
        {
            try
            {
                PartSelectSVC = PartSelectFactory.CreateServerClient();

                BindingWeeks(DateTime.Now.Year);
                comboxEditWeek.Text = GetCurrentWeeks().ToString();
                dateEditYear.DateTime = DateTime.Now;

                DataSet dsOrt = PartSelectSVC.GetORTluPartFamilyType("ORTTestResult", "");
                if (dsOrt == null || dsOrt.Tables.Count == 0 || dsOrt.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                DataTable dtBingding = dsOrt.Tables[0];
                for (int i = dtBingding.Rows.Count - 1; i >= 0; i--)
                {
                    if (dtBingding.Rows[i]["Content"].ToString() != "1")
                        dtBingding.Rows.RemoveAt(i);
                }
                AddPartFamilyType(Com_PartFamilyType, Grid_PartFamilyType, dtBingding);
                List_Login = this.Tag as LoginList;
            }
            catch(Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }


        private void Com_PartFamilyType_EditValueChanged(object sender, EventArgs e)
        {
            GetORTNumber();
            GetORTCode();
            ResetRegist();
        }

        private void radioBtnDay_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnDay.Checked)
            {
                btmAddBatch.Enabled = true;
                GetORTNumber();
                GetORTMaxBatch();
            }
        }
        private void radioBtnWeek_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnWeek.Checked)
            {

                btmAddBatch.Enabled = true;
                GetORTNumber();
                GetORTMaxBatch();
            }
        }
        private void radioBtnNull_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnNull.Checked)
            {
                btnRegist.Enabled = false;
                btmAddBatch.Enabled = false;
                ResetRegist();
                GetORTMaxBatch();
            }
        }


        private void GetORTCode_EditValueChanged(object sender, EventArgs e)
        {
            GetORTCode();
            ResetRegist();
        }

        private void txtORTCode_EditValueChanged(object sender, EventArgs e)
        {
            if (!txtORTCode.ReadOnly && !string.IsNullOrEmpty(txtORTCode.Text.Trim()))
                return;
            if (string.IsNullOrEmpty(txtORTCode.Text.Trim()))
            {
                btnAlterORTNO.Enabled = false;
            }
            else
            {
                btnAlterORTNO.Enabled = true;
            }

            GetORTMaxBatch();
        }

        private void btnAddORTCode_Click(object sender, EventArgs e)
        {
            try
            {
                string ORTCode = txtORTCode.Text.Trim();
                string PartFamilyTypeID = Com_PartFamilyType.EditValue.ToString();
                string YearID = dateEditYear.DateTime.ToString("yyyy");
                string WeekID = comboxEditWeek.Text.ToString();
                if (string.IsNullOrEmpty(PartFamilyTypeID))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20209", "NG", List_Login.Language, new string[] { lblPartFamilyTypeID.Text.ToString() });
                    return;
                }
                if (string.IsNullOrEmpty(ORTCode))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20209", "NG", List_Login.Language, new string[] { lblORTCode.Text.ToString() });
                    return;
                }
                if (string.IsNullOrEmpty(YearID))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20209", "NG", List_Login.Language, new string[] { lblYear.Text.ToString() });
                    return;
                }
                if (string.IsNullOrEmpty(WeekID))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20209", "NG", List_Login.Language, new string[] { lblWeek.Text.ToString() });
                    return;
                }

                ORTCode = Com_PartFamilyType.Text.ToString() + "-" + ORTCode;
                DataSet dataSet = PartSelectSVC.GetORTForCode(ORTCode);
                if(dataSet!=null && dataSet.Tables[0].Rows.Count>0)
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20210", "NG", List_Login.Language, new string[] { ORTCode });
                    return;
                }

                string ActiveORT = PartSelectSVC.GetORTCode(PartFamilyTypeID, YearID, WeekID);
                if(!string.IsNullOrEmpty(ActiveORT))
                {
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20211", "NG", List_Login.Language, new string[] { ActiveORT });
                    return;
                }

                PartSelectSVC.InsertORTCodeData(ORTCode, PartFamilyTypeID, YearID, WeekID);
                btnAddORTCode.Visible = false;
                txtORTCode.ReadOnly = true;
                txtORTCode.Text = ORTCode;

                MessageInfo.Add_Info_MSG(Edt_MSG, "10036", "OK", List_Login.Language, new string[] { ORTCode });
            }
            catch(Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void btmAddBatch_Click(object sender, EventArgs e)
        {
            string ORTCode = txtORTCode.Text.Trim();
            string TestTypeID = radioBtnDay.Checked ? "0" : "1";
            string RequestedQty = txtReQTY.Text.ToString();

            if (string.IsNullOrEmpty(ORTCode))
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20209", "NG", List_Login.Language, new string[] { lblORTCode.Text.ToString() });
                return;
            }
            if (string.IsNullOrEmpty(RequestedQty) || Convert.ToInt32(RequestedQty) < 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20213", "NG", List_Login.Language, new string[] { lblReQTY.Text.ToString() });
                return;
            }

            string result = string.Empty;
            string xmlExtraData = "<ExtraData ORTCode=\"" + ORTCode + "\" " +
                                             "TestTypeID =\"" + TestTypeID + "\" " +
                                             "RequestedQty =\"" + RequestedQty + "\" " +
                                             "EmployeeId =\"" + List_Login.EmployeeID +  "\"> </ExtraData>";
            DataSet dsORT = PartSelectSVC.uspCallProcedure("uspORTCreateCode", "", "", "",
                                                                "", xmlExtraData, "", ref result);
            if(result!="1" || dsORT == null || dsORT.Tables.Count == 0)
            {
                string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, result);
                return;
            }

            string OrderNo = dsORT.Tables[0].Rows[0]["OrderNo"].ToString();
            MessageInfo.Add_Info_MSG(Edt_MSG, "10024", "OK", List_Login.Language, new string[] { OrderNo });
            comboxEditORTBatch.Properties.Items.Add(OrderNo);
            comboxEditORTBatch.Text = OrderNo;
        }

        private void comboxEditORTBatch_EditValueChanged(object sender, EventArgs e)
        {
            GetORTTestResult();
            ResetRegist();
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            if (gridViewORTMian.DataSource == null)
            {
                return;
            }

            int index = gridViewORTMian.GetFocusedDataSourceRowIndex();
            if (index < 0)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20215", "NG", List_Login.Language);
                return;
            }

            string ORTState =gridViewORTMian.GetRowCellValue(index, "ORTState").ToString();
            if (ORTState != "Open")
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20216", "NG", List_Login.Language,new string[] { ORTState });
                return;
            }

            int RequestedQty = (int)gridViewORTMian.GetRowCellValue(index, "RequestedQty");
            int InitedQty = (int)gridViewORTMian.GetRowCellValue(index, "InitedQty");
            int OrderNo = (int)gridViewORTMian.GetRowCellValue(index, "OrderNo");
            string TestTypeID = gridViewORTMian.GetRowCellValue(index, "TestTypeID").ToString();
            txtReQTY.Text = RequestedQty.ToString();
            comboxEditORTBatch.Text = OrderNo.ToString();
            if(TestTypeID== "Daily")
            {
                radioBtnDay.Checked = true;
            }
            else
            {
                radioBtnWeek.Checked = true;
            }
            txtSN.Text = string.Empty;
            txtSN.ReadOnly = false;
            btnResult.Enabled = false;
            txtSN.Focus();
        }

        private void gridViewORTMian_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            int index = gridViewORTMian.GetFocusedDataSourceRowIndex();

            FouceIndex = index;
            if (gridViewORTMian.DataSource == null || index<0)
            {
                gridCtrlDetail.DataSource = null;
                return;
            }

            string ORTState = gridViewORTMian.GetRowCellValue(index, "ORTState").ToString();
            string ResultID = gridViewORTMian.GetRowCellValue(index, "ResultID").ToString();
            string result = string.Empty;
            DataSet dsResult = PartSelectSVC.uspCallProcedure("uspORTGetData", ResultID, "", "",
                                                            "", "", "2", ref result);
            if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
            {
                gridCtrlDetail.DataSource = dsResult.Tables[0];
            }
            else
            {
                txtIniQTY.Text = "0";
                gridCtrlDetail.DataSource = null;
            }

            if (ORTState != "Open" || radioBtnNull.Checked)
            {
                btnRegist.Enabled = false;
                ResetRegist();
            }
            else
            {
                btnRegist.Enabled = true;
            }

            if (ORTState == "Verify")
            {
                btnResult.Enabled = true;
            }
            else
            {
                btnResult.Enabled = false;
            }
        }

        private void txtSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    string SN = txtSN.Text.Trim();
                    if (string.IsNullOrEmpty(SN))
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20007", "NG", List_Login.Language);
                        return;
                    }

                    int index = gridViewORTMian.GetFocusedDataSourceRowIndex();
                    if (index < 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20215", "NG", List_Login.Language);
                        return;
                    }
                    string ResultID = gridViewORTMian.GetRowCellValue(index, "ResultID").ToString();
                    string xmlStation = "<Station StationId=\"" + List_Login.StationID.ToString() + "\"> </Station>";
                    string xmlExtraData = "<ExtraData EmployeeId =\"" + List_Login.EmployeeID + "\"> </ExtraData>";
                    string result = string.Empty;
                    DataSet dsSNResult = PartSelectSVC.uspCallProcedure("uspORTInsertDetail", SN, "", "",
                                                                    xmlStation, xmlExtraData, ResultID, ref result);


                    if (dsSNResult != null && dsSNResult.Tables.Count>0 && dsSNResult.Tables[0].Rows.Count > 0)
                    {
                        txtStationType.Text = dsSNResult.Tables[0].Rows[0]["Station"].ToString();
                        txtStatus.Text = dsSNResult.Tables[0].Rows[0]["Status"].ToString();
                    }

                    if (result != "1")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, result);
                        txtSN.Text = string.Empty;
                        return;
                    }

                    txtSN.Text = string.Empty;

                    //重新加载数据
                    DataSet dsResult = PartSelectSVC.uspCallProcedure("uspORTGetData", ResultID, "", "",
                                                                    "", "", "3", ref result);
                    gridCtrlORTMian.DataSource = dsResult.Tables[0];

                    MessageInfo.Add_Info_MSG(Edt_MSG, "10037", "OK", List_Login.Language, new string[] { SN });
                }
                catch (Exception ex)
                {
                    txtSN.Text = string.Empty;
                    MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                }
            }
        }

        private void gridViewORTMian_DataSourceChanged(object sender, EventArgs e)
        {
            btnResult.Text = MessageInfo.GetMsgByCode("10039", List_Login.Language);
            btnResult.Tag = "0";
            this.gridColumnResult.Visible = false;

            int index = gridViewORTMian.GetFocusedDataSourceRowIndex();
            if (gridViewORTMian.DataSource == null || index < 0)
            {
                return;
            }

            string ORTState = gridViewORTMian.GetRowCellValue(index, "ORTState").ToString();
            if (ORTState != "Verify")
            {
                btnResult.Enabled = false;
            }
            else
            {
                btnResult.Enabled = true;
            }

            if (FouceIndex == 0)
            {
                gridViewORTMian_FocusedRowChanged(null, null);
            }
        }

        private void gridViewDetail_DataSourceChanged(object sender, EventArgs e)
        {
            if(gridCtrlDetail.DataSource!=null)
            {
                int number = ((DataTable)gridCtrlDetail.DataSource).Rows.Count;
                txtIniQTY.Text = number.ToString();
            }
            else
            {
                txtIniQTY.Text = "0";
            }
        }

        private void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnResult.Tag.ToString() == "0")
                {
                    if (gridCtrlORTMian.DataSource == null || gridCtrlDetail.DataSource == null)
                    {
                        return;
                    }

                    int index = gridViewORTMian.GetFocusedDataSourceRowIndex();
                    if (gridViewORTMian.DataSource == null || index < 0)
                    {
                        return;
                    }

                    string ORTState = gridViewORTMian.GetRowCellValue(index, "ORTState").ToString();
                    MianResultID = gridViewORTMian.GetRowCellValue(index, "ResultID").ToString();
                    if (ORTState != "Verify")
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20229", "NG", List_Login.Language, new string[] { ORTState });
                        return;
                    }
                    this.gridColumnResult.Visible = true;
                    this.gridColumnResult.VisibleIndex = 0;
                    btnResult.Text = MessageInfo.GetMsgByCode("10040", List_Login.Language);
                    btnResult.Tag = "1";
                }
                else
                {
                    //循环获取数据
                    DataTable dtDetail = (DataTable)gridCtrlDetail.DataSource;
                    if (dtDetail == null && dtDetail.Rows.Count == 0)
                    {
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20230", "NG", List_Login.Language);
                        return;
                    }
                    string strSNList = string.Empty;
                    foreach (DataRow dr in dtDetail.Rows)
                    {
                        strSNList = dr["DetailID"].ToString() + "," + dr["Status"].ToString() + ";" + strSNList;
                    }

                    string result = string.Empty;
                    PartSelectSVC.uspCallProcedure("uspORTSetResult", strSNList, "", "",
                                                        "", "", List_Login.EmployeeID.ToString(), ref result);
                    if (result != "1")
                    {
                        string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                        MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, result);
                        txtSN.Text = string.Empty;
                        return;
                    }

                    //重新加载数据
                    DataSet dsResult = PartSelectSVC.uspCallProcedure("uspORTGetData", MianResultID, "", "",
                                                                    "", "", "3", ref result);
                    gridCtrlORTMian.DataSource = dsResult.Tables[0];

                    btnResult.Text = MessageInfo.GetMsgByCode("10039", List_Login.Language);
                    btnResult.Tag = "0";
                    this.gridColumnResult.Visible = false;
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10038", "OK", List_Login.Language);
                }
            }
            catch(Exception ex)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
            }
        }

        private void btnDeleteSN_Click(object sender, EventArgs e)
        {
            int index = gridViewDetail.GetFocusedDataSourceRowIndex();
            if(gridCtrlDetail.DataSource == null || index < 0)
            {
                return;
            }

            string ORTState = gridViewDetail.GetRowCellValue(index, "ORTState").ToString();
            string DetailID = gridViewDetail.GetRowCellValue(index, "DetailID").ToString();
            string SN = gridViewDetail.GetRowCellValue(index, "Serialnumber").ToString();
            if (ORTState!="Open" && ORTState!= "Verify")
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20231", "NG", List_Login.Language);
                return;
            }

            string result = string.Empty;
            PartSelectSVC.uspCallProcedure("uspORTDelDetail", DetailID, "", "",
                                                            "", "", List_Login.EmployeeID.ToString(), ref result);
            if (result != "1")
            {
                string ProMsg = MessageInfo.GetMsgByCode(result, List_Login.Language);
                MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ProMsg }, result);
                txtSN.Text = string.Empty;
                return;
            }
            index = gridViewORTMian.GetFocusedDataSourceRowIndex();
            if (gridViewORTMian.DataSource != null && index >= 0)
            {
                string ResultID = gridViewORTMian.GetRowCellValue(index, "ResultID").ToString();

                DataSet dsResult = PartSelectSVC.uspCallProcedure("uspORTGetData", ResultID, "", "",
                                                                        "", "", "3", ref result);
                gridCtrlORTMian.DataSource = dsResult.Tables[0];
            }

            MessageInfo.Add_Info_MSG(Edt_MSG, "10041", "OK", List_Login.Language,new string[] { SN });
        }

        private void btnAlterORTNO_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtORTCode.Text.Trim()))
            {
                return;
            }

            ORTReplaceCode_Form OrtFrm = new ORTReplaceCode_Form(List_Login, txtORTCode.Text.Trim(),PartSelectSVC);
            if(OrtFrm.ShowDialog() == DialogResult.OK)
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "10044", "OK", List_Login.Language,new string[] { OrtFrm.NewOrtCode });
                txtORTCode.Text = OrtFrm.NewOrtCode;
            }
            else
            {
                MessageInfo.Add_Info_MSG(Edt_MSG, "20234", "NG", List_Login.Language);
            }
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            ORTReplaceSN OrtFrm = new ORTReplaceSN(List_Login, PartSelectSVC);
            OrtFrm.ShowDialog();
        }
    }
}
