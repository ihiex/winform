using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.MyMES
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //string str = "0008074605+1+0+1000+IJ+3008083+870-14761-01+870-14761-01++++2022-04-30 08:24+8146+3836+R";
            //Console.WriteLine(Regex.IsMatch(str, @"^.{88}$"));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
            //Application.Run(new AAAForm());

            //Application.Run(new MES_Dev_Form());
        }
    }
}
