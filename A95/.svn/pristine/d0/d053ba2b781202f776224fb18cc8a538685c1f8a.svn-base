using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarPrint
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] List_Value)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string S_Value = ""; 
            for (int i = 0; i < List_Value.Length; i++)
            {
                S_Value += List_Value[i];
            }
            Application.Run(new BarTender_Form(S_Value));
        }
    }
}
