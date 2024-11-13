namespace App.MyMES
{
    partial class MES_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MES_Form));
            this.Panel_Title = new System.Windows.Forms.Panel();
            this.Panel_Right = new System.Windows.Forms.Panel();
            this.ToolBar_Main = new System.Windows.Forms.ToolStrip();
            this.Lab_Msg = new System.Windows.Forms.ToolStripLabel();
            this.Btn_Search = new System.Windows.Forms.ToolStripButton();
            this.Btn_Close = new System.Windows.Forms.ToolStripButton();
            this.Btn_InforCenter = new System.Windows.Forms.ToolStripButton();
            this.Panel_Left = new System.Windows.Forms.Panel();
            this.TooBar_Left = new System.Windows.Forms.ToolStrip();
            this.Lab_Title = new System.Windows.Forms.ToolStripButton();
            this.Panel_Main = new System.Windows.Forms.Panel();
            this.Btn_LogOut = new System.Windows.Forms.ToolStripButton();
            this.Panel_Title.SuspendLayout();
            this.Panel_Right.SuspendLayout();
            this.ToolBar_Main.SuspendLayout();
            this.Panel_Left.SuspendLayout();
            this.TooBar_Left.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel_Title
            // 
            this.Panel_Title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Panel_Title.Controls.Add(this.Panel_Right);
            this.Panel_Title.Controls.Add(this.Panel_Left);
            this.Panel_Title.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel_Title.Location = new System.Drawing.Point(0, 0);
            this.Panel_Title.Name = "Panel_Title";
            this.Panel_Title.Size = new System.Drawing.Size(1121, 44);
            this.Panel_Title.TabIndex = 35;
            // 
            // Panel_Right
            // 
            this.Panel_Right.Controls.Add(this.ToolBar_Main);
            this.Panel_Right.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel_Right.Location = new System.Drawing.Point(518, 0);
            this.Panel_Right.Name = "Panel_Right";
            this.Panel_Right.Size = new System.Drawing.Size(603, 44);
            this.Panel_Right.TabIndex = 6;
            // 
            // ToolBar_Main
            // 
            this.ToolBar_Main.AutoSize = false;
            this.ToolBar_Main.BackColor = System.Drawing.Color.Transparent;
            this.ToolBar_Main.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ToolBar_Main.GripMargin = new System.Windows.Forms.Padding(0);
            this.ToolBar_Main.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolBar_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Lab_Msg,
            this.Btn_Search,
            this.Btn_InforCenter,
            this.Btn_Close,
            this.Btn_LogOut});
            this.ToolBar_Main.Location = new System.Drawing.Point(0, 0);
            this.ToolBar_Main.Name = "ToolBar_Main";
            this.ToolBar_Main.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ToolBar_Main.Size = new System.Drawing.Size(603, 44);
            this.ToolBar_Main.TabIndex = 5;
            this.ToolBar_Main.Text = "toolStrip1";
            // 
            // Lab_Msg
            // 
            this.Lab_Msg.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lab_Msg.ForeColor = System.Drawing.Color.White;
            this.Lab_Msg.Name = "Lab_Msg";
            this.Lab_Msg.Size = new System.Drawing.Size(81, 41);
            this.Lab_Msg.Text = "Lab_Msg";
            // 
            // Btn_Search
            // 
            this.Btn_Search.AutoSize = false;
            this.Btn_Search.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Btn_Search.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_Search.ForeColor = System.Drawing.Color.White;
            this.Btn_Search.Image = ((System.Drawing.Image)(resources.GetObject("Btn_Search.Image")));
            this.Btn_Search.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Btn_Search.Name = "Btn_Search";
            this.Btn_Search.Size = new System.Drawing.Size(120, 41);
            this.Btn_Search.Text = "Search(查询)";
            this.Btn_Search.Click += new System.EventHandler(this.Btn_Search_Click);
            // 
            // Btn_Close
            // 
            this.Btn_Close.AutoSize = false;
            this.Btn_Close.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Btn_Close.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_Close.ForeColor = System.Drawing.Color.White;
            this.Btn_Close.Image = ((System.Drawing.Image)(resources.GetObject("Btn_Close.Image")));
            this.Btn_Close.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new System.Drawing.Size(120, 41);
            this.Btn_Close.Text = "Close(关闭)";
            this.Btn_Close.Click += new System.EventHandler(this.Btn_Close_Click);
            // 
            // Btn_InforCenter
            // 
            this.Btn_InforCenter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Btn_InforCenter.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_InforCenter.ForeColor = System.Drawing.SystemColors.Window;
            this.Btn_InforCenter.Image = ((System.Drawing.Image)(resources.GetObject("Btn_InforCenter.Image")));
            this.Btn_InforCenter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Btn_InforCenter.Name = "Btn_InforCenter";
            this.Btn_InforCenter.Size = new System.Drawing.Size(111, 41);
            this.Btn_InforCenter.Text = "Infor Center";
            this.Btn_InforCenter.Click += new System.EventHandler(this.Btn_InforCenter_Click);
            // 
            // Panel_Left
            // 
            this.Panel_Left.Controls.Add(this.TooBar_Left);
            this.Panel_Left.Dock = System.Windows.Forms.DockStyle.Left;
            this.Panel_Left.Location = new System.Drawing.Point(0, 0);
            this.Panel_Left.Name = "Panel_Left";
            this.Panel_Left.Size = new System.Drawing.Size(518, 44);
            this.Panel_Left.TabIndex = 7;
            // 
            // TooBar_Left
            // 
            this.TooBar_Left.AutoSize = false;
            this.TooBar_Left.BackColor = System.Drawing.Color.Transparent;
            this.TooBar_Left.Dock = System.Windows.Forms.DockStyle.None;
            this.TooBar_Left.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TooBar_Left.GripMargin = new System.Windows.Forms.Padding(0);
            this.TooBar_Left.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.TooBar_Left.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Lab_Title});
            this.TooBar_Left.Location = new System.Drawing.Point(-6, 0);
            this.TooBar_Left.Name = "TooBar_Left";
            this.TooBar_Left.Size = new System.Drawing.Size(537, 44);
            this.TooBar_Left.TabIndex = 6;
            this.TooBar_Left.Text = "TooBar_Left";
            // 
            // Lab_Title
            // 
            this.Lab_Title.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Lab_Title.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lab_Title.ForeColor = System.Drawing.Color.White;
            this.Lab_Title.Image = ((System.Drawing.Image)(resources.GetObject("Lab_Title.Image")));
            this.Lab_Title.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Lab_Title.Name = "Lab_Title";
            this.Lab_Title.Size = new System.Drawing.Size(144, 41);
            this.Lab_Title.Text = "COSMO MES";
            // 
            // Panel_Main
            // 
            this.Panel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel_Main.Location = new System.Drawing.Point(0, 44);
            this.Panel_Main.Name = "Panel_Main";
            this.Panel_Main.Padding = new System.Windows.Forms.Padding(5);
            this.Panel_Main.Size = new System.Drawing.Size(1121, 646);
            this.Panel_Main.TabIndex = 36;
            // 
            // Btn_LogOut
            // 
            this.Btn_LogOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Btn_LogOut.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_LogOut.ForeColor = System.Drawing.Color.White;
            this.Btn_LogOut.Image = ((System.Drawing.Image)(resources.GetObject("Btn_LogOut.Image")));
            this.Btn_LogOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Btn_LogOut.Name = "Btn_LogOut";
            this.Btn_LogOut.Size = new System.Drawing.Size(119, 41);
            this.Btn_LogOut.Text = "(LogOut)注销";
            this.Btn_LogOut.Click += new System.EventHandler(this.Btn_LogOut_Click);
            // 
            // MES_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1121, 690);
            this.Controls.Add(this.Panel_Main);
            this.Controls.Add(this.Panel_Title);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MES_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "COSMO MES System ";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MES_Form_FormClosing);
            this.Load += new System.EventHandler(this.MES_Form_Load);
            this.Resize += new System.EventHandler(this.MES_Form_Resize);
            this.Panel_Title.ResumeLayout(false);
            this.Panel_Right.ResumeLayout(false);
            this.ToolBar_Main.ResumeLayout(false);
            this.ToolBar_Main.PerformLayout();
            this.Panel_Left.ResumeLayout(false);
            this.TooBar_Left.ResumeLayout(false);
            this.TooBar_Left.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel Panel_Title;
        private System.Windows.Forms.Panel Panel_Main;
        private System.Windows.Forms.ToolStrip ToolBar_Main;
        private System.Windows.Forms.ToolStripLabel Lab_Msg;
        private System.Windows.Forms.ToolStripButton Btn_Search;
        private System.Windows.Forms.ToolStripButton Btn_Close;
        private System.Windows.Forms.Panel Panel_Right;
        private System.Windows.Forms.Panel Panel_Left;
        private System.Windows.Forms.ToolStrip TooBar_Left;
        private System.Windows.Forms.ToolStripButton Lab_Title;
        private System.Windows.Forms.ToolStripButton Btn_InforCenter;
        private System.Windows.Forms.ToolStripButton Btn_LogOut;
    }
}