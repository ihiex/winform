//using Seagull.BarTender.Print;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.MyMES
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        string S_Path = Application.StartupPath;
        string S_TempletName = "\\Lab_BarTender\\Hp_Vero_20190612.btw";


        private void button1_Click(object sender, EventArgs e)
        {
            //Engine engine = new Engine(true);
            //LabelFormatDocument format = engine.Documents.Open(S_Path + S_TempletName);
            //format.SubStrings["1"].Value = "AAAAAA";


            //Seagull.BarTender.Print.Messages message = new Seagull.BarTender.Print.Messages();
            //format.Print("Select printer", out message);
            //engine.Dispose();


            //BarTenderPrint v_BarTenderPrint = new BarTenderPrint();
            //v_BarTenderPrint.PrintSN(S_TempletName);

            //BarTender.Application btApp;
            //BarTender.Format btFormat;
            //btApp = new BarTender.Application();
            //btFormat = btApp.Formats.Open(S_Path + S_TempletName,false,"");


            //BarTender.Messages message = new BarTender.Messages();
            //btFormat.Print("", true, 5, out message);           
            //btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);





        }
    }
}
