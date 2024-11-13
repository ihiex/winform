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
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;

namespace App.MyMES
{
    public partial class LoginForm : DevExpress.XtraBars.Ribbon.RibbonForm 
    {
        Public_ public_ = new Public_();
        public LoginForm()
        {
            InitializeComponent();
        }

        private void SaveSeting()
        {
            try
            {
                string S_Path = Application.StartupPath;
                MyINI myINI = new MyINI(S_Path + "\\MES_Main.ini");

                string S_UserID = Edt_UserID.EditValue.ToString();             
                string S_StationID = Com_Station.EditValue.ToString();
                string S_LineID = Com_Line.EditValue.ToString();

                myINI.WriteValue("UserID", "Value", S_UserID);
                myINI.WriteValue("StationID", "Value", S_StationID);
                myINI.WriteValue("LineID", "Value", S_LineID);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            string language = Com_Language.EditValue.ToString();
            if(string.IsNullOrEmpty(language))
            {
                language = "EN";
            }


            if (Com_Line.Text.Trim() == "")
            {
                string ProMsg = MessageInfo.GetMsgByCode("20137", language);
                MessageBox.Show(ProMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (Com_Station.Text.Trim() == "")
            {
                string ProMsg = MessageInfo.GetMsgByCode("20138", language);
                MessageBox.Show(ProMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ImesEmployeeSVCClient mesEmployeeSVC = ImesEmployeeFactory.CreateServerClient(); //new ImesEmployeeSVCClient();
            string S_Sql = " where UserID='" + Edt_UserID.Text.Trim() + "' and Password='" + Edt_Password.Text.Trim() + "'";
            IEnumerable<mesEmployee> v_mesEmployee = mesEmployeeSVC.ListAll(S_Sql);
            List<mesEmployee> List_mesEmployee = v_mesEmployee.ToList();

            if (List_mesEmployee.Count() > 0)
            {
                if (Edt_Password.Text.Trim() == "12345678")
                {
                    Btn_ChangePWD_Click(sender, e);
                    return;
                }

                int I_EmployeeID = List_mesEmployee[0].ID;
                //二次校验
                string S_ValidateSecond = mesEmployeeSVC.ValidateSecond(I_EmployeeID.ToString(), Edt_Password.Text.Trim());
                if (S_ValidateSecond != "OK")
                {
                    MessageBox.Show(S_ValidateSecond, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //权限校验
                bool Permission = mesEmployeeSVC.PermissionCheck(I_EmployeeID, Convert.ToInt32(Com_StationType.EditValue.ToString()));
                if(!Permission)
                {
                    string ProMsg = MessageInfo.GetMsgByCode("20139", language);
                    MessageBox.Show(ProMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SaveSeting();
                this.Hide();
                LoginList List_Login = new LoginList();

                //////////////////////////////////////////////////////////////////////////////////////
                List_Login.LineID = Convert.ToInt32(Com_Line.EditValue);
                List_Login.Line = Com_Line.Text.Trim();  

                List_Login.StationID = Convert.ToInt32(Com_Station.EditValue);
                List_Login.Station = Com_Station.Text.Trim();  
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
                List_Login.SkinID= Convert.ToInt32(Com_Skin.EditValue);
                List_Login.SkinName = Com_Skin.Text;
                List_Login.Language= Com_Language.EditValue.ToString();
                List_Login.ServerIP = Lab_IP.Caption;
                List_Login.Ver = Lab_Ver.Caption.Trim();

                string S_Path = Application.StartupPath;
                MyINI myINI = new MyINI(S_Path + "\\MES_Set");
                myINI.WriteValue("PrintSN2", "User", List_mesEmployee[0].Firstname);
               
                MES_Dev_Form v_MES_Dev_Form = new MES_Dev_Form(); 
                v_MES_Dev_Form.Show_MES_Dev_Form(v_MES_Dev_Form, List_Login); 

                //Test_Form v_Test = new Test_Form();
                //v_Test.show2(List_Login, v_Test);

                try
                {
                    IPartSelectSVC partSelect = PartSelectFactory.CreateServerClient();
                    DataSet DS = partSelect.GetPLCSeting("CodeSoft_Ver", List_Login.StationID.ToString());
                    if (DS != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                    {
                        DataTable DT = DS.Tables[0];

                        if (DT.Rows.Count > 0)
                        {
                            string S_Value = DT.Rows[0]["Value"].ToString().Trim();

                            if (S_Value != "")
                            {
                                File.Delete(S_Path + "\\Interop.LabelManager2.dll");
                                File.Copy(S_Path + "\\CodeSoft\\" + S_Value + "\\Interop.LabelManager2.dll", S_Path + "\\Interop.LabelManager2.dll");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                string ProMsg = MessageInfo.GetMsgByCode("20140", language);
                MessageBox.Show(ProMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                Process[] arrayProcess = Process.GetProcessesByName("bartend");
                foreach (Process pp in arrayProcess)
                {
                    pp.Kill();
                }
            }
            catch { }
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            //string S_ServerVer = public_.GetVer();
            string S_LocalVer = Lab_Ver.Caption.Trim();
            //string S_IsUpdate = public_.IsUpdate();

            //if (S_ServerVer != S_LocalVer)
            //{
            //if (S_IsUpdate == "1")
            //{
            //    Btn_Update_Click(sender, e);
            //    return;
            //}
            //else
            //{
            //    if (MessageBox.Show("The system has been updated. Do you need to update?", "MSG", MessageBoxButtons.YesNo,
            //        MessageBoxIcon.Exclamation) == DialogResult.Yes)
            //    {
            //        Btn_Update_Click(sender, e);
            //        return;
            //    }
            //}
            //}

            string S_LineID = "";
            public_.AddStationType(Com_StationType, Grid_StationType);
            public_.AddLine(Com_Line, Grid_Line);
            public_.AddLanguage(Com_Language, Grid_Language);
            public_.AddSkin(Com_Skin, Grid_Skin);

            Lab_IP.Caption = public_.GetServerIP();
            Lab_DBName.Caption = public_.GetDbName();
            Lab_WebIP.Caption = public_.GetWebIP();

            try
            {
                S_LineID = Com_Line.EditValue.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                string S_Path = Application.StartupPath;
                MyINI myINI = new MyINI(S_Path + "\\MES_Main.ini");

                string S_UserID = myINI.ReadValue("UserID", "Value");
                string S_StationID = myINI.ReadValue("StationID", "Value");
                S_LineID = myINI.ReadValue("LineID", "Value");
                string S_SkinNameID = myINI.ReadValue("SkinID", "Value");
                string S_LanguageID = myINI.ReadValue("LanguageID", "Value");

                if (S_UserID != "") { Edt_UserID.Text = S_UserID; }
                if (S_LineID != "") { Com_Line.EditValue = Convert.ToInt32(S_LineID); }
                if (S_StationID != "") { Com_Station.EditValue = Convert.ToInt32(S_StationID); }

                if (S_SkinNameID != "")
                {
                    Com_Skin.EditValue = S_SkinNameID;
                    defaultLook_Main.LookAndFeel.SkinName = Com_Skin.Text;
                }
                if (S_LanguageID != "")
                {
                    Com_Language.EditValue = S_LanguageID;
                }

                //调用通用过程
                PartSelectSVCClient partSelect = PartSelectFactory.CreateServerClient();               

                string outString = string.Empty;
                if (S_StationID == "") { S_StationID = null; }
                partSelect.uspCallProcedure("uspVersionCheck", S_StationID, null, null, null, null, S_LocalVer, ref outString);

                if (outString == "2")
                {
                    Btn_Update_Click(sender, e);
                    return;
                }
                else if (outString == "1")
                {
                    if (MessageBox.Show("The system has been updated. Do you need to update?", "MSG", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        Btn_Update_Click(sender, e);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_Seting_Click(object sender, EventArgs e)
        {
            ImesEmployeeSVCClient mesEmployeeSVC = ImesEmployeeFactory.CreateServerClient();
            string S_Sql = " where UserID='" + Edt_UserID.Text.Trim() + "' and Password='" + Edt_Password.Text.Trim() + "'";
            IEnumerable<mesEmployee> v_mesEmployee = mesEmployeeSVC.ListAll(S_Sql);
            List<mesEmployee> List_mesEmployee = v_mesEmployee.ToList();
            mesEmployeeSVC.Close();

            string language = Com_Language.EditValue.ToString();
            if (string.IsNullOrEmpty(language))
            {
                language = "EN";
            }

            if (List_mesEmployee.Count() > 0)
            {
                int I_PermissionId = List_mesEmployee[0].PermissionId;

                if (I_PermissionId == 1)
                {
                    Com_StationType.Enabled = true;
                    Com_Line.Enabled = true;
                    Com_Station.Enabled = true;

                    Com_StationType.BackColor = SystemColors.Window;
                    Com_Line.BackColor = SystemColors.Window;
                    Com_Station.BackColor = SystemColors.Window;

                    SaveSeting();
                }
                else
                {
                    Com_StationType.Enabled = false;
                    Com_Line.Enabled = false;
                    Com_Station.Enabled = false;

                    string ProMsg = MessageInfo.GetMsgByCode("20141", language);
                    MessageBox.Show(ProMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                string ProMsg = MessageInfo.GetMsgByCode("20140", language);
                MessageBox.Show(ProMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                process.StartInfo.FileName = S_SysPath + "\\MESUpdate.exe";  //FTP
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                Com_StationType.EditValue = Convert.ToInt32( S_StationTypeID);
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

        private void button1_Click(object sender, EventArgs e)
        {
            PartSelectSVCClient PartSelectSVC= PartSelectFactory.CreateServerClient();
            DataSet DS = PartSelectSVC.GetmesUnitComponent2("24402");


        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            //string  ss = PartSelectSVC.MESAssembleCheckMianSN("63", 5, 2, 7, "PUK-CUDGA-5.0-20421-01-04210519000001", false);

            string ss= PartSelectSVC.MESAssembleCheckOtherSN("PUK-CUDGA-5.0-20421-01-04210519000001", "46", false);
        }

        private void Btn_ChangePWD_Click(object sender, EventArgs e)
        {
            string S_UserID = Edt_UserID.Text.Trim();
            if (S_UserID == "")
            {
                MessageBox.Show("UserID cannot be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ChangePWDForm v_ChangePWDForm = new ChangePWDForm();
            v_ChangePWDForm.Show_ChangePWDForm(v_ChangePWDForm, S_UserID);
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    ImesEmployeeSVCClient mesEmployeeSVC = ImesEmployeeFactory.CreateServerClient();
        //    string S_Sql = " where Firstname='APIUser' and Password='APIUser'";
        //    IEnumerable<mesEmployee> v_mesEmployee = mesEmployeeSVC.ListAll(S_Sql);
        //    List<mesEmployee> List_mesEmployee = v_mesEmployee.ToList();
        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
        //    string ss= PartSelectSVC.TimeCheck("10", "TGP0202E033436091AA0W10202");
        //}
    }
}
