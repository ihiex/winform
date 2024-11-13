using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BartenderPrint
{
    public partial class BarTenderNewFrm : Form
    {
        public BarTenderNewFrm()
        {
            InitializeComponent();
        }

        private void BarTenderNewFrm_Load(object sender, EventArgs e)
        {
            this.SizeChanged += BarTenderNewFrm_SizeChanged;
            SocketManager socketManager = new SocketManager();
            socketManager.StartSocket();
        }

        private void BarTenderNewFrm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIconBarTender.Visible = true;
            }
        }

        private void notifyIconBarTender_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.notifyIconBarTender.Visible = false;

            Application.Exit();
        }
    }
}
