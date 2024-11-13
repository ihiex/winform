using System;
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
using App.MyMES.mesUnitDefectService;
using App.MyMES.luDefectService;

namespace App.MyMES
{
    //localhost:55368
    // 
    public partial class IQCForm : Form
    {
        Public_ public_ = new Public_();
        LoginList List_Login = new LoginList();
        public IQCForm()
        {
            InitializeComponent();
        }

        string S_PartID;
        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            GirdDataClear();
            public_.AddPartFamilyType(Com_PartFamilyType);
            //public_.AddmesUnitState(Com_UnitState);
            /////////////////////////////////////////////////////////////////////           
            List_Login=this.Tag as LoginList;

            public_.AddStationType(Com_StationType);
            public_.AddLine(Com_Line);
            //public_.AddDefect(Com_Defect);

            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_LineID = List_Login.LineID.ToString();  
            public_.AddStation(Com_Station,S_LineID);

            Com_StationType.Text = S_StationTypeID;
            Com_Line.Text = S_LineID;
            Com_Station.Text = List_Login.StationID.ToString();

            timer_GridColor.Enabled = true;
        }

        private void GirdDataClear()
        {
            Grid_mesUnit.DataSource = null;
            Grid_mesHistory.DataSource = null;
            Edt_ScanSN.Enabled = false;
            //Edt_MSG.Text = ""; 
        }

        private void Grid_mesUnit_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string S_ID = Grid_mesUnit.CurrentRow.Cells["ID"].Value.ToString();
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            DataTable DT = PartSelectSVC.GetHistory(S_ID).Tables[0];

            Grid_mesHistory.DataSource = DT;            
            public_.Grid_NGColor(Grid_mesHistory);

            try
            {
                DataTable DT_Defect = PartSelectSVC.GetmesUnitDefect(S_ID).Tables[0];
                Grid_Defect.DataSource = DT_Defect;
            }
            catch
            {
                Grid_Defect.DataSource = null;
            }

            PartSelectSVC.Close();
        }

        private void Edt_ScanSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //Edt_MSG.Text = ""; 

                 ImesUnitSVCClient mesUnitSVC = ImesUnitFactory.CreateServerClient();
                ImesSerialNumberSVCClient mesSerialNumberSVC = ImesSerialNumberFactory.CreateServerClient();
                ImesHistorySVCClient mesHistorySVC = ImesHistoryFactory.CreateServerClient();

                string S_SN = Edt_ScanSN.Text.Trim();
                IEnumerable<mesSerialNumber> List_mesSerialNumber = mesSerialNumberSVC.ListAll(" Where Value='" + S_SN.Trim() + "'");

                string S_UnitState = Com_UnitState.Text.Trim();
                DataRowView DRV = Com_UnitState.SelectedItem as DataRowView;
                string S_UnitStateID = DRV["ID"].ToString().Trim();
                Boolean B_NG = true;

                string S_DefectID = Edt_DefectID.Text.Trim();  

                if (List_mesSerialNumber.ToList().Count > 0)
                {
                    Public_.Add_Info_MSG(Edt_MSG, "条码已存在!", "NG");                                                
                    Edt_ScanSN.SelectAll();
                }
                else
                {
                    if (S_SN.Trim().Length >= 5)
                    {
                        if (S_UnitStateID == "2")
                        {
                            if (S_DefectID == "")
                            {
                                Public_.Add_Info_MSG(Edt_MSG, " 确认此物料NG,请设置NG原因!", "NG");
                                Edt_ScanSN.SelectAll();
                                B_NG = false;
                            }

                            //if (MessageBox.Show("确认此物料是: NG ？", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                            //{
                            //    B_NG = false;
                            //    Edt_ScanSN.SelectAll();
                            //}
                        }

                        if (B_NG == true)
                        {
                            mesUnit v_mesUnit = new mesUnit();

                            v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                            v_mesUnit.StatusID = 1;
                            v_mesUnit.StationID = List_Login.StationID;
                            v_mesUnit.EmployeeID = List_Login.EmployeeID;
                            v_mesUnit.CreationTime = DateTime.Now;
                            v_mesUnit.LastUpdate = DateTime.Now;
                            v_mesUnit.PanelID = 0;
                            v_mesUnit.LineID = List_Login.LineID;
                            v_mesUnit.ProductionOrderID = 0;
                            v_mesUnit.RMAID = 0;
                            v_mesUnit.PartID = Convert.ToInt32(S_PartID);
                            v_mesUnit.LooperCount = 1;

                            string S_InsertUnit = mesUnitSVC.Insert(v_mesUnit);

                            if (S_InsertUnit.Substring(0, 1) != "E")
                            {
                                mesSerialNumber v_mesSerialNumber = new mesSerialNumber();

                                v_mesSerialNumber.UnitID = Convert.ToInt32(S_InsertUnit);
                                v_mesSerialNumber.SerialNumberTypeID = 1;
                                v_mesSerialNumber.Value = S_SN;
                                int I_InsertSN = mesSerialNumberSVC.Insert(v_mesSerialNumber);
                                ///////////////////
                                public_.Grid_NGColor(Grid_mesUnit);

                                mesHistory v_mesHistory = new mesHistory();

                                v_mesHistory.UnitID = Convert.ToInt32(S_InsertUnit);
                                v_mesHistory.UnitStateID = v_mesUnit.UnitStateID;
                                v_mesHistory.EmployeeID = List_Login.EmployeeID;
                                v_mesHistory.StationID = List_Login.StationID;
                                v_mesHistory.EnterTime = DateTime.Now;
                                v_mesHistory.ExitTime = DateTime.Now;
                                v_mesHistory.ProductionOrderID = 0;
                                v_mesHistory.PartID = Convert.ToInt32(S_PartID);
                                v_mesHistory.LooperCount = 1;
                                int I_InsertHistory = mesHistorySVC.Insert(v_mesHistory);


                                //////////////////////////  Defect ///////////////////////////////////////////
                                string[] Array_Defect = S_DefectID.Split(';');
                                ImesUnitDefectSVCClient mesUnitDefectSVC = ImesUnitDefectFactory.CreateServerClient();
                                if (S_UnitStateID == "2")
                                {
                                    foreach (var item in Array_Defect)
                                    {
                                        string S_mesUnitDefectInsert = "";
                                        try
                                        {
                                            if (item.Trim() != "")
                                            {
                                                int I_DefectID = Convert.ToInt32(item);
                                                mesUnitDefect v_mesUnitDefect = new mesUnitDefect();

                                                v_mesUnitDefect.UnitID = Convert.ToInt32(S_InsertUnit);
                                                v_mesUnitDefect.DefectID = I_DefectID;
                                                v_mesUnitDefect.StationID = List_Login.StationID;
                                                v_mesUnitDefect.EmployeeID = List_Login.EmployeeID;

                                                S_mesUnitDefectInsert = mesUnitDefectSVC.Insert(v_mesUnitDefect);
                                            }
                                        }
                                        catch 
                                        {
                                            Public_.Add_Info_MSG(Edt_MSG, S_mesUnitDefectInsert, "NG");
                                        }
                                    }
                                }
                                //关闭对象
                                mesSerialNumberSVC.Close();
                                mesUnitSVC.Close();
                                mesHistorySVC.Close();
                                mesUnitDefectSVC.Close();
                                ///////////////////////////////////
                                Com_Part_SelectedIndexChanged(sender, e);
                                Edt_ScanSN.Text = "";
                                Edt_DefectID.Text = "";
                                //////////////////////////////////
                                Public_.Add_Info_MSG(Edt_MSG, "OK！", "OK");
                            }
                            else
                            {
                                Public_.Add_Info_MSG(Edt_MSG, S_InsertUnit, "NG");
                            }
                        }                        
                    }
                    else
                    {
                        Public_.Add_Info_MSG(Edt_MSG, "条码格式错误!", "NG");                                                
                        Edt_ScanSN.SelectAll();
                    }
                }

            }
        }

        private void IQCForm_Load(object sender, EventArgs e)
        {
            Btn_Refresh_Click(sender, e);           
        }

        private void Com_PartFamilyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView DRV = Com_PartFamilyType.SelectedItem as DataRowView;
            GirdDataClear();

            if (DRV != null)
            {
                string S_PartFamilyTypeID = DRV["ID"].ToString();
                public_.AddPartFamily(Com_PartFamily, S_PartFamilyTypeID);
            }
        }

        private void Com_PartFamily_SelectedIndexChanged(object sender, EventArgs e)
        {
            string S_PartFamily = Com_PartFamily.Text.Trim();
            GirdDataClear();

            DataRowView DRV = Com_PartFamily.SelectedItem as DataRowView;
            if (DRV != null)
            {
                string S_PartFamilyID = DRV["ID"].ToString();
                public_.AddPart(Com_Part, S_PartFamilyID);
            }
        }

        private void Com_Part_SelectedIndexChanged(object sender, EventArgs e)
        {
            string S_Part = Com_Part.Text.Trim();

            DataRowView DRV = Com_Part.SelectedItem as DataRowView;
            if (DRV != null)
            {
                S_PartID = DRV["ID"].ToString();

                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                DataTable DT = PartSelectSVC.GetUnit(S_PartID).Tables[0];

                Grid_mesUnit.DataSource = DT;
                Edt_ScanSN.Enabled = true;

                PartSelectSVC.Close();
                public_.Grid_NGColor(Grid_mesUnit);
            }

            Grid_mesHistory.DataSource = null;
            Grid_Defect.DataSource = null;
        }

        private void timer_GridColor_Tick(object sender, EventArgs e)
        {
            public_.Grid_NGColor(Grid_mesUnit);
            timer_GridColor.Enabled = false;
        }

        private void Grid_mesUnit_Sorted(object sender, EventArgs e)
        {
            timer_GridColor.Enabled = true;
        }

        private void Com_UnitState_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView DRV = Com_UnitState.SelectedItem as DataRowView;
            string S_UnitStateID = DRV["ID"].ToString().Trim();

            if (S_UnitStateID == "2")
            {
                Btn_Defect.Enabled = true;
                DefectForm v_DefectForm = new DefectForm();
                v_DefectForm.Show_DefectForm(v_DefectForm, Edt_DefectID);
                Lab_NG.Visible = true;
            }
            else
            {
                Btn_Defect.Enabled = false;
                Lab_NG.Visible = false;
            }
        }

        private void Btn_Defect_Click(object sender, EventArgs e)
        {
            DefectForm v_DefectForm = new DefectForm();
            v_DefectForm.Show_DefectForm(v_DefectForm, Edt_DefectID);
        }
    }
}
