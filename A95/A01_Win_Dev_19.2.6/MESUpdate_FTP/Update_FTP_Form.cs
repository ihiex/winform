using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MESUpdate_FTP
{
    public partial class Update_FTP_Form : Form
    {
        public Update_FTP_Form()
        {
            InitializeComponent();
        }

  
        string S_SysPath = Application.StartupPath;
        MyINI INI;//= new MyINI(S_SysPath+"\\Update.ini");
        FtpWeb FTP;

        string S_UpdateURL; //= "\\\\192.168.50.243\\SFC_PubilcFolder\\Tony_Zhang\\6_S01_Win\\";
        private void Btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                Public_ public_ = new Public_(); 

                KillProcess("COSMOMES");
                Thread.Sleep(500);

                string[] List_Files = FTP.GetFileList("");

                foreach (var item in List_Files)
                {                    
                    if (Path.GetExtension(item) == ".dll" || Path.GetExtension(item) == ".exe" ||  Path.GetExtension(item)== ".config")
                    {
                        string S_FileName = Path.GetFileName(item);
                        if (S_FileName != "MESUpdate.exe" && S_FileName!="dotNetFx45.exe")
                        {
                            if (S_FileName.Substring(0, 3) == "Dev")
                            {
                                if (Check_Dev.Checked == true)
                                {
                                    //File.Copy(item, S_SysPath + "\\" + S_FileName, true);
                                    FTP.Download(S_SysPath, S_FileName);
                                }
                            }
                            else
                            {
                                //File.Copy(item, S_SysPath + "\\" + S_FileName, true);
                                FTP.Download(S_SysPath, S_FileName);
                            }                            
                        }
                        Thread.Sleep(100);
                    }
                }

                MessageBox.Show("Update complete ！", "MSG", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Process process = new Process();
                try
                {
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.FileName = S_SysPath + "\\COSMOMES.exe";
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        ///// <summary>
        ///// 关闭进程
        ///// </summary>
        ///// <param name="processName">进程名</param>
        //private void KillProcess(string processName)
        //{
        //    Process[] myproc = Process.GetProcesses();
        //    foreach (Process item in myproc)
        //    {
        //        try
        //        {
        //            if (item.ProcessName == processName)
        //            {
        //                item.Kill();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //}

        public void KillProcess(string strProcessesByName)//关闭线程
        {
            foreach (Process p in Process.GetProcesses())//GetProcessesByName(strProcessesByName))
            {
                if (p.ProcessName.ToUpper().Contains(strProcessesByName))
                {
                    try
                    {
                        p.Kill();
                        p.WaitForExit(); // possibly with a timeout
                    }
                    catch (Win32Exception ex)
                    {
                        MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);   // process was terminating or can't be terminated - deal with it
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); // process has already exited - might be able to let this one go
                    }
                }
            }
        }




        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Update_Form_Load(object sender, EventArgs e)
        {
            INI = new MyINI(S_SysPath + "\\Update.ini");
            S_UpdateURL = INI.ReadValue("FTP", "URL");
            FTP = new FtpWeb(S_UpdateURL, "", "Anonymous", "");


            Lab_URL.Text ="Update Server:"+ S_UpdateURL; 
        }
    }
}
