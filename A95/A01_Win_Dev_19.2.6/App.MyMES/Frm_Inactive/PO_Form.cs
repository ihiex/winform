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
using App.MyMES.mesProductionOrderService;

namespace App.MyMES
{
    public partial class PO_Form : Form
    {
        Public_ public_ = new Public_();
        LoginList List_Login = new LoginList();
        string S_EditType = "";
        //int F_EmployeeID = 0;

        //public void Show_PO_Form(PO_Form v_IQCForm,int I_EmployeeID)
        //{
        //    F_EmployeeID = I_EmployeeID;
        //    v_IQCForm.Show(); 
        //}


        public PO_Form()
        {
            InitializeComponent();
        }

        private void PO_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void BtnEnabled(Boolean B_Btn)
        {
            Btn_Save.Enabled = B_Btn;
            Btn_Delete.Enabled = B_Btn;

            Edt_PONumber.Enabled = B_Btn;
            Edt_OrderQuantity.Enabled = B_Btn;   
        }


        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            public_.AddPartFamilyType(Com_PartFamilyType);
            BtnEnabled(false);
        }

        private void Com_PartFamilyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView DRV = Com_PartFamilyType.SelectedItem as DataRowView;

            if (DRV != null)
            {
                string S_PartFamilyTypeID = DRV["ID"].ToString();
                public_.AddPartFamily(Com_PartFamily, S_PartFamilyTypeID);
            }
        }

        private void Com_PartFamily_SelectedIndexChanged(object sender, EventArgs e)
        {
            string S_PartFamily = Com_PartFamily.Text.Trim();

            DataRowView DRV = Com_PartFamily.SelectedItem as DataRowView;
            if (DRV != null)
            {
                string S_PartFamilyID = DRV["ID"].ToString();
                public_.AddPart(Com_Part, S_PartFamilyID);
            }
        }

        private void PO_Form_Load(object sender, EventArgs e)
        {
            public_.AddPartFamilyType(Com_PartFamilyType);
            //public_.AddStstus(Com_Status);

            /////////////////////////////////////////////////////////////            
            List_Login = this.Tag as LoginList;

            public_.AddStationType(Com_StationType);
            public_.AddLine(Com_Line);

            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_LineID = List_Login.LineID.ToString();
            public_.AddStation(Com_Station, S_LineID);

            Com_StationType.Text = S_StationTypeID;
            Com_Line.Text = S_LineID;
            Com_Station.Text = List_Login.StationID.ToString();
            //AddData();
        }

        private void Btn_Insert_Click(object sender, EventArgs e)
        {
            S_EditType = "Insert";
            Btn_Save.Enabled = true;

            Edt_PONumber.Text = "";
            Edt_OrderQuantity.Text = "";
            BtnEnabled(true);
        }

        private void Btn_Delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？！", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                S_EditType = "Delete";
                BtnEnabled(false);

                ImesProductionOrderSVCClient v_ImesProductionOrderSVCClient = new ImesProductionOrderSVCClient();
                string S_ID = Grid_PO.CurrentRow.Cells["ID"].Value.ToString();
                v_ImesProductionOrderSVCClient.Delete(Convert.ToInt32(S_ID));

                Edt_PONumber.Text = "";
                Edt_OrderQuantity.Text = "";   

                AddData();
            }
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            if (Edt_PONumber.Text.Trim() == "")
            {
                MessageBox.Show("工单号不能为空！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Edt_OrderQuantity.Text.Trim() == "")
            {
                MessageBox.Show("数量不能为空！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            DataRowView DRV_PartFamilyType=Com_PartFamilyType.SelectedItem as DataRowView;
            DataRowView DRV_PartFamily = Com_PartFamily.SelectedItem as DataRowView;
            DataRowView DRV_Part = Com_Part.SelectedItem as DataRowView;
            //DataRowView DRV_Status = Com_Status.SelectedItem as DataRowView;

            string S_PartFamilyTypeID = DRV_PartFamilyType["ID"].ToString();
            string S_PartFamilyID= DRV_PartFamily["ID"].ToString();
            string S_PartID= DRV_Part["ID"].ToString();
            //string S_StatusID= DRV_Status["ID"].ToString();

            ImesProductionOrderSVCClient v_ImesProductionOrderSVCClient = new ImesProductionOrderSVCClient(); 
            mesProductionOrder v_mesProductionOrder = new mesProductionOrder();

            try
            {
                v_mesProductionOrder.ProductionOrderNumber = Edt_PONumber.Text.Trim();
                v_mesProductionOrder.Description = "";
                v_mesProductionOrder.PartID = Convert.ToInt32(S_PartID);
                v_mesProductionOrder.OrderQuantity = Convert.ToInt32(Edt_OrderQuantity.Text.Trim());
                v_mesProductionOrder.EmployeeID = List_Login.EmployeeID;

                v_mesProductionOrder.CreationTime = DateTime.Now;
                v_mesProductionOrder.LastUpdate = DateTime.Now;

                v_mesProductionOrder.StatusID = 1;
                v_mesProductionOrder.IsLotAuditCompleted = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (S_EditType == "Insert")
            {
                string S_InsertResult= v_ImesProductionOrderSVCClient.Insert(v_mesProductionOrder);

                string S_Sql = "select * from mesProductionOrder where ProductionOrderNumber='" + v_mesProductionOrder.ProductionOrderNumber + "'";
                DataTable DT_POID= public_.P_DataSet(S_Sql).Tables[0];
                int I_POID = Convert.ToInt32(DT_POID.Rows[0]["ID"].ToString()); 

                ImesUnitSVCClient mesUnitSVC = ImesUnitFactory.CreateServerClient();
                ImesSerialNumberSVCClient mesSerialNumberSVC = ImesSerialNumberFactory.CreateServerClient();
                ImesHistorySVCClient mesHistorySVC = ImesHistoryFactory.CreateServerClient();
                mesUnit v_mesUnit = new mesUnit();

                v_mesUnit.UnitStateID = 1;
                v_mesUnit.StatusID = 1;
                v_mesUnit.StationID = List_Login.StationID;
                v_mesUnit.EmployeeID = List_Login.EmployeeID;
                v_mesUnit.CreationTime = DateTime.Now;
                v_mesUnit.LastUpdate = DateTime.Now;
                v_mesUnit.PanelID = 0;
                v_mesUnit.LineID = List_Login.LineID;
                v_mesUnit.ProductionOrderID = I_POID;
                v_mesUnit.RMAID = 0;
                v_mesUnit.PartID = Convert.ToInt32(S_PartID);
                v_mesUnit.LooperCount = 1;

                string S_InsertmesUnit = mesUnitSVC.Insert(v_mesUnit);

                if (S_InsertResult != "OK")
                {
                    MessageBox.Show(S_InsertResult, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    BtnEnabled(false);

                    AddData();
                    S_EditType = "";
                }            
            }
            else if (S_EditType == "Update")
            {
                string S_ID = Grid_PO.CurrentRow.Cells["ID"].Value.ToString();
                v_mesProductionOrder.ID = Convert.ToInt32(S_ID);

                string S_UpdateResult= v_ImesProductionOrderSVCClient.Update(v_mesProductionOrder);
                if (S_UpdateResult != "OK")
                {
                    MessageBox.Show(S_UpdateResult, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    BtnEnabled(false);

                    AddData();
                    S_EditType = "";
                }
            }
        }

        private void Grid_PO_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            S_EditType = "Update";
            
            string S_ID = Grid_PO.CurrentRow.Cells["ID"].Value.ToString() ;
            string S_Sql = "SELECT  * FROM mesProductionOrder where ID='" + S_ID + "'";

            DataTable DT_PO = public_.P_DataSet(S_Sql).Tables[0];
            string S_PartID = DT_PO.Rows[0]["PartID"].ToString();
            string S_StatusID=DT_PO.Rows[0]["StatusID"].ToString();

            string S_ProductionOrderNumber=DT_PO.Rows[0]["ProductionOrderNumber"].ToString();
            string S_OrderQuantity=DT_PO.Rows[0]["OrderQuantity"].ToString();

            S_Sql = "select *  from  mesPart where ID='"+ S_PartID + "'";
            DataTable DT_Part= public_.P_DataSet(S_Sql).Tables[0];
            string S_PartFamilyID = DT_Part.Rows[0]["PartFamilyID"].ToString();
            
            S_Sql = "select *  from  luPartFamily where ID='" + S_PartFamilyID + "'";
            DataTable DT_PartFamily = public_.P_DataSet(S_Sql).Tables[0];
            string S_PartFamilyTypeID = DT_PartFamily.Rows[0]["PartFamilyTypeID"].ToString();

            Com_PartFamilyType.Text = S_PartFamilyTypeID;
            Com_PartFamily.Text = S_PartFamilyID;
            Com_Part.Text = S_PartID;

            Edt_PONumber.Text = S_ProductionOrderNumber;
            Edt_OrderQuantity.Text = S_OrderQuantity;
            //Com_Status.Text = S_StatusID;

            if (S_StatusID == "1")
            {
                BtnEnabled(true);
            }
            else
            {
                BtnEnabled(false);
            }
        }

        private void AddData()
        {
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();

            try
            {
                DataRowView DRV = Com_Part.SelectedItem as DataRowView;
                string S_PartID = DRV["ID"].ToString();

                DataTable DT = PartSelectSVC.GetPO(S_PartID, "").Tables[0];
                Grid_PO.DataSource = DT;
            }
            catch
            {
                Grid_PO.DataSource = null;
            }

            PartSelectSVC.Close();
        }

        private void Com_Part_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddData();
        }

        private void Com_Status_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddData();
        }
    }
}
