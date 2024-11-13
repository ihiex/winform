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
using System.Configuration;
using App.MyMES.mesEmployeeService;
using System.Diagnostics;

namespace App.MyMES
{
    public partial class SetStation_Form : DevExpress.XtraEditors.XtraForm
    {
        Public_ public_ = new Public_();
        MES_Dev_Form F_Main_Form;

        public SetStation_Form()
        {
            InitializeComponent();
        }

        public void Show_SetStation_Form(MES_Dev_Form Main_Form , SetStation_Form F_SetStation_Form)
        {
            F_Main_Form = Main_Form;
            F_SetStation_Form.ShowDialog();
        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                string S_Path = Application.StartupPath;
                MyINI myINI = new MyINI(S_Path + "\\MES_Main.ini");

                string S_StationTypeID = Com_StationType.EditValue.ToString();
                string S_StationID = Com_Station.EditValue.ToString();
                string S_LineID = Com_Line.EditValue.ToString();

                myINI.WriteValue("StationTypeID", "Value", S_StationTypeID);
                myINI.WriteValue("StationID", "Value", S_StationID);
                myINI.WriteValue("LineID", "Value", S_LineID);

                F_Main_Form.Hide();
                this.Hide(); 
                LoginForm loginFrom = new LoginForm();
                loginFrom.ShowDialog();
                F_Main_Form.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void SetStation_Form_Load(object sender, EventArgs e)
        {
            try
            {
                public_.AddStationType(Com_StationType, Grid_StationType);
                public_.AddLine(Com_Line, Grid_Line);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }

            try
            {
                string S_Path = Application.StartupPath;
                MyINI myINI = new MyINI(S_Path + "\\MES_Main.ini");

                string S_StationTypeID = myINI.ReadValue("StationTypeID", "Value");
                string S_StationID = myINI.ReadValue("StationID", "Value");
                string S_LineID = myINI.ReadValue("LineID", "Value");

                //if (S_StationTypeID != "") { Com_StationType.EditValue = S_StationTypeID; }
                if (S_LineID != "") { Com_Line.EditValue = S_LineID; }
                if (S_StationID != "") { Com_Station.EditValue = S_StationID; }                            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Com_Line_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_LineID = Com_Line.EditValue.ToString();
                public_.AddStation(Com_Station, S_LineID, Grid_Station);
            }
            catch
            {
            }
        }

        private void Com_Station_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string S_StationID = Com_Station.EditValue.ToString();

                PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
                DataTable DT = PartSelectSVC.GetmesStationTypeByStationID(S_StationID).Tables[0];
                PartSelectSVC.Close();

                string S_StationTypeID = DT.Rows[0]["StationTypeID"].ToString();
                Com_StationType.EditValue = S_StationTypeID;
            }
            catch
            {
            }
        }
    }
}
