namespace App.MyMES
{
    partial class QCNG_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QCNG_Form));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Lab_Status = new System.Windows.Forms.Label();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.GrpControlMSG = new DevExpress.XtraEditors.GroupControl();
            this.Edt_MSG = new System.Windows.Forms.RichTextBox();
            this.Btn_Close = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlMSG)).BeginInit();
            this.GrpControlMSG.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::App.MyMES.Properties.Resources.xxx;
            this.pictureBox1.Location = new System.Drawing.Point(2, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(239, 236);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Lab_Status
            // 
            this.Lab_Status.AutoSize = true;
            this.Lab_Status.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Lab_Status.Font = new System.Drawing.Font("Calibri", 150F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lab_Status.ForeColor = System.Drawing.Color.Red;
            this.Lab_Status.Location = new System.Drawing.Point(2, 2);
            this.Lab_Status.Name = "Lab_Status";
            this.Lab_Status.Size = new System.Drawing.Size(362, 244);
            this.Lab_Status.TabIndex = 1;
            this.Lab_Status.Text = "NG";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.Btn_Close);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 444);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(878, 43);
            this.panelControl1.TabIndex = 86;
            // 
            // GrpControlMSG
            // 
            this.GrpControlMSG.Controls.Add(this.Edt_MSG);
            this.GrpControlMSG.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.GrpControlMSG.Location = new System.Drawing.Point(0, 240);
            this.GrpControlMSG.Name = "GrpControlMSG";
            this.GrpControlMSG.Size = new System.Drawing.Size(878, 204);
            this.GrpControlMSG.TabIndex = 87;
            this.GrpControlMSG.Text = "Message";
            // 
            // Edt_MSG
            // 
            this.Edt_MSG.BackColor = System.Drawing.Color.White;
            this.Edt_MSG.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Edt_MSG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Edt_MSG.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Edt_MSG.ForeColor = System.Drawing.Color.Red;
            this.Edt_MSG.Location = new System.Drawing.Point(2, 23);
            this.Edt_MSG.Name = "Edt_MSG";
            this.Edt_MSG.ReadOnly = true;
            this.Edt_MSG.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.Edt_MSG.Size = new System.Drawing.Size(874, 179);
            this.Edt_MSG.TabIndex = 49;
            this.Edt_MSG.Text = "";
            // 
            // Btn_Close
            // 
            this.Btn_Close.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Close.Appearance.Options.UseFont = true;
            this.Btn_Close.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("Btn_Cancel.ImageOptions.SvgImage")));
            this.Btn_Close.Location = new System.Drawing.Point(391, 0);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new System.Drawing.Size(119, 43);
            this.Btn_Close.TabIndex = 81;
            this.Btn_Close.Text = "Close";
            this.Btn_Close.Click += new System.EventHandler(this.Btn_Close_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.pictureBox1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(243, 240);
            this.panelControl2.TabIndex = 88;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.Lab_Status);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(243, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(635, 240);
            this.panelControl3.TabIndex = 89;
            // 
            // QCNG_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 487);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.GrpControlMSG);
            this.Controls.Add(this.panelControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QCNG_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QCNG  Message";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlMSG)).EndInit();
            this.GrpControlMSG.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        public DevExpress.XtraEditors.GroupControl GrpControlMSG;
        public System.Windows.Forms.RichTextBox Edt_MSG;
        private DevExpress.XtraEditors.SimpleButton Btn_Close;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        public System.Windows.Forms.Label Lab_Status;
    }
}