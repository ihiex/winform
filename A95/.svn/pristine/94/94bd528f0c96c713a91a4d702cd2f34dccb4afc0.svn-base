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

namespace MESUpdate
{
    public partial class Update_Form : Form
    {
        public Update_Form()
        {
            InitializeComponent();
        }

        string S_SysPath = Application.StartupPath;

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                Public_ public_ = new Public_(); 

                KillProcess("COSMOMES");
                Thread.Sleep(500);

                //File.Delete(S_SysPath + "\\COSMOMES.exe");
                //File.Delete(S_SysPath + "\\App.Model.dll");

                //File.Copy("\\\\192.168.50.243\\SFC_PubilcFolder\\Tony_Zhang\\2_A95_Win\\COSMOMES.exe",
                //           S_SysPath + "\\COSMOMES.exe", true);

                //File.Copy("\\\\192.168.50.243\\SFC_PubilcFolder\\Tony_Zhang\\2_A95_Win\\App.Model.dll",
                //           S_SysPath + "\\App.Model.dll", true);

                string[] List_Files = public_.GetFiles("\\\\192.168.50.243\\SFC_PubilcFolder\\Tony_Zhang\\3_A01_Win\\",
                                                       null,false,false );

                foreach (var item in List_Files)
                {                    
                    if (Path.GetExtension(item) == ".dll" || Path.GetExtension(item) == ".exe")
                    {
                        string S_FileName = Path.GetFileName(item);
                        if (S_FileName != "MESUpdate.exe" && S_FileName!="dotNetFx45.exe" && S_FileName!= "COSMOMES.exe.Config")
                        {
                            File.Copy(item, S_SysPath + "\\" + S_FileName, true);
                        }


                    }
                }

                MessageBox.Show("更新成功！", "MSG", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
    }
}
