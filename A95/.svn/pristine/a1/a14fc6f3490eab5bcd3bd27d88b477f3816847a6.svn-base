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
using System.ServiceModel;
using System.Net;
using System.Web.Security;

namespace App.MyMES
{
    public partial class LoginForm : Form
    {
        Public_ public_ = new Public_();
        public LoginForm()
        {
            InitializeComponent();
        }

        private void SaveSeting()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("UserID");
            config.AppSettings.Settings.Add("UserID", Edt_UserID.Text.Trim());

            try
            {
                string S_StationTypeID = Com_Station.EditValue.ToString();

                config.AppSettings.Settings.Remove("StationTypeID");
                config.AppSettings.Settings.Add("StationTypeID", S_StationTypeID);
            }
            catch { }

            try
            {
                string S_LineID = Com_Line.EditValue.ToString();
                config.AppSettings.Settings.Remove("LineID");
                config.AppSettings.Settings.Add("LineID", S_LineID);
            }
            catch { }

            try
            {
                string S_StationID = Com_Station.EditValue.ToString();
                config.AppSettings.Settings.Remove("StationID");
                config.AppSettings.Settings.Add("StationID", S_StationID);
            }
            catch { }

            try
            {
                string S_ApplicationTypeID = Com_ApplicationType.EditValue.ToString();
                config.AppSettings.Settings.Remove("ApplicationTypeID");
                config.AppSettings.Settings.Add("ApplicationTypeID", S_ApplicationTypeID);
            }
            catch { }

            config.Save();
        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            if (Com_Line.Text.Trim() == "")
            {
                MessageBox.Show(" Line 不能为空！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (Com_Station.Text.Trim() == "")
            {
                MessageBox.Show(" Station 不能为空！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //EndpointAddress endadd = new EndpointAddress("net.tcp://localhost:55368/mesEmployee.svc");

            //ImesEmployeeSVCClient cilent = new ImesEmployeeSVCClient(WebP.GetNetTcpBinding(true), endadd);

            //NetworkCredential credential = cilent.ChannelFactory.Credentials.Windows.ClientCredential;
            //credential.UserName = "用户名";
            //credential.Password = "密码";

            //mesEmployee[] List_mesEmployee222 = cilent.ListAll("");


            //ImesEmployeeSVCChannel aaa = WcfInvokeFactory.CreateServiceByUrl<ImesEmployeeSVCChannel>("http://localhost:55368/mesEmployee.svc");            
            //mesEmployee[] iiii = aaa.ListAll("");
            //aaa.Close();

            //Panel_Menu.Visible = false;  


            //ImesEmployeeSVCClient mesEmployeeSVC111 = ImesEmployeeFactory.CreateServerClient();
            //var vvv = mesEmployeeSVC111.ListAll("");

            ImesEmployeeSVCClient mesEmployeeSVC = ImesEmployeeFactory.CreateServerClient(); //new ImesEmployeeSVCClient();
            string S_Sql = " where UserID='" + Edt_UserID.Text.Trim() + "' and Password='" + Edt_Password.Text.Trim() + "'";
            IEnumerable<mesEmployee> v_mesEmployee = mesEmployeeSVC.ListAll(S_Sql);
            List<mesEmployee> List_mesEmployee = v_mesEmployee.ToList();

            if (List_mesEmployee.Count() > 0)
            {

                SaveSeting();
                this.Hide();

                int I_EmployeeID = List_mesEmployee[0].ID;
                LoginList List_Login = new LoginList();

                //////////////////////////////////////////////////////////////////////////////////////
                List_Login.LineID = Convert.ToInt32(Com_Line.EditValue);
                List_Login.StationID = Convert.ToInt32(Com_Station.EditValue);
                //////////////////////////////////////////////////////////////////////////////////////
                List_Login.StationType = Com_StationType.Text;
                //////////////////////////////////////////////////////////////////////////////////////
                string S_StationTypeID = Com_StationType.EditValue.ToString();
                List_Login.StationTypeID = Convert.ToInt32(S_StationTypeID);

                //////////////////////////////////////////////////////////////////////////////////////
                List_Login.EmployeeID = I_EmployeeID;
                //////////////////////////////////////////////////////////////////////////////////////
                List_Login.ApplicationTypeID = Convert.ToInt32(Com_ApplicationType.EditValue);
                List_Login.ApplicationType = Com_ApplicationType.Text;
                //////////////////////////////////////////////////////////////////////////////////////

                string S_Path = Application.StartupPath;
                MyINI myINI = new MyINI(S_Path + "\\MES_Set");
                myINI.WriteValue("PrintSN2", "User", List_mesEmployee[0].Firstname);

                List_Login.Ver = Lab_Ver.Text;

                // MES_Dev_Form v_MES_Dev_Form = new MES_Dev_Form(); 
                //v_MES_Dev_Form.Show_MES_Dev_Form(v_MES_Dev_Form, List_Login);   

                MES_Form v_MES_Form = new MES_Form();
                v_MES_Form.Show_MES_Form(v_MES_Form, List_Login);

                //MES_Dev2_Form v_MES_Dev2_Form = new MES_Dev2_Form();
                //v_MES_Dev2_Form.Show_MES_Form(v_MES_Dev2_Form, List_Login);
            }
            else
            {
                MessageBox.Show("用户ID或密码错误！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            string S_LineID = "";

            public_.AddStationType(Com_StationType, Grid_StationType);
            public_.AddLine(Com_Line, Grid_Line);
            Lab_IP.Text = public_.GetServerIP();

            try
            {
                S_LineID = Com_Line.EditValue.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            try
            {
                string S_UserID = config.AppSettings.Settings["UserID"].Value.Trim();
                if (S_UserID != "") { Edt_UserID.Text = S_UserID; }
            }
            catch { }

            try
            {
                //Com_Line
                S_LineID = config.AppSettings.Settings["LineID"].Value.Trim();
                if (S_LineID != "")
                {
                    Com_Line.EditValue = S_LineID;
                }
            }
            catch { }

            try
            {
                //Com_Station
                string S_StationID = config.AppSettings.Settings["StationID"].Value.Trim();
                if (S_StationID != "")
                {
                    Com_Station.EditValue = S_StationID;
                }
            }
            catch { }

        }

        private void Btn_Seting_Click(object sender, EventArgs e)
        {
            ImesEmployeeSVCClient mesEmployeeSVC = ImesEmployeeFactory.CreateServerClient();
            string S_Sql = " where UserID='" + Edt_UserID.Text.Trim() + "' and Password='" + Edt_Password.Text.Trim() + "'";
            IEnumerable<mesEmployee> v_mesEmployee = mesEmployeeSVC.ListAll(S_Sql);
            List<mesEmployee> List_mesEmployee = v_mesEmployee.ToList();
            mesEmployeeSVC.Close();

            if (List_mesEmployee.Count() > 0)
            {
                int I_PermissionId = List_mesEmployee[0].PermissionId;

                if (I_PermissionId == 1)
                {
                    Com_StationType.Enabled = true;
                    Com_Line.Enabled = true;
                    Com_Station.Enabled = true;
                    //Com_PO.Enabled = true;

                    Com_StationType.BackColor = SystemColors.Window;
                    Com_Line.BackColor = SystemColors.Window;
                    Com_Station.BackColor = SystemColors.Window;
                    //Com_PO.BackColor = SystemColors.Window;

                    SaveSeting();
                }
                else
                {
                    Com_StationType.Enabled = false;
                    Com_Line.Enabled = false;
                    Com_Station.Enabled = false;
                    //Com_PO.Enabled = false;

                    Com_StationType.BackColor = Color.Yellow;
                    Com_Line.BackColor = Color.Yellow;
                    Com_Station.BackColor = Color.Yellow;
                    //Com_PO.BackColor = Color.Yellow;

                    MessageBox.Show("请联系SFC人员设置！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("用户ID或密码错误！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Edt_UserID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Edt_Password.Focus();
            }
        }

        private void Edt_Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Btn_OK_Click(sender, e);
            }
        }

        private void Btn_Update_Click(object sender, EventArgs e)
        {
            Process process = new Process();
            try
            {
                string S_SysPath = Application.StartupPath;

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.FileName = S_SysPath + "\\MESUpdate.exe";
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                //string S_LineID = Com_Line.EditValue.ToString();
                //DataTable DT = PartSelectSVC.GetmesStation(S_LineID).Tables[0];
                DataTable DT = PartSelectSVC.GetmesStationTypeByStationID(S_StationID).Tables[0];
                PartSelectSVC.Close();

                string S_StationTypeID = DT.Rows[0]["StationTypeID"].ToString();
                Com_StationType.EditValue = S_StationTypeID;
            }
            catch
            {
            }
        }

        private void Com_StationType_EditValueChanged(object sender, EventArgs e)
        {
            string StationTypeID = Com_StationType.EditValue.ToString();
            public_.AddApplicationType(Com_ApplicationType, StationTypeID, Grid_ApplicationType);
        }
    }
}
