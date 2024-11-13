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
    public partial class MSGForm : DevExpress.XtraBars.Ribbon.RibbonForm 
    {
        Public_ public_ = new Public_();
        public MSGForm()
        {
            InitializeComponent();
        }

        PartSelectSVCClient F_PartSelectSVC;
        mesScreenshot F_mesScreenshot;
        public void Show_MSGForm(MSGForm v_MSGForm, string S_MSG, PartSelectSVCClient PartSelectSVC, mesScreenshot v_mesScreenshot)
        {
            timer1.Interval = 60000;
            try
            {
                int I_FeedbackPopupCloseTimer = Convert.ToInt32(public_.GetAppSet(PartSelectSVC, "FeedbackPopupCloseTimer"));
                timer1.Interval = I_FeedbackPopupCloseTimer;
            }
            catch { }

            timer1.Enabled = true;

            F_PartSelectSVC = PartSelectSVC;
            F_mesScreenshot = v_mesScreenshot;

            v_MSGForm.Memo_MSG.Text = S_MSG;
            v_MSGForm.ShowDialog();            
        }

        private void GetClose()
        {
            F_mesScreenshot.IsFeedback = 2;
            F_PartSelectSVC.UpdateScreenshot(F_mesScreenshot);

            timer1.Enabled = false;
            this.Close();
        }


        private void Btn_OK_Click(object sender, EventArgs e)
        {
            GetClose();
        }

        private void MSGForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            GetClose();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GetClose();
        }
    }
}
