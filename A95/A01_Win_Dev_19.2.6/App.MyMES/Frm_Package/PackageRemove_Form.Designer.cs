namespace App.MyMES
{
    partial class PackageRemove_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageRemove_Form));
            this.GrpControlMSG = new DevExpress.XtraEditors.GroupControl();
            this.Edt_MSG = new System.Windows.Forms.RichTextBox();
            this.lblSN = new DevExpress.XtraEditors.LabelControl();
            this.txtSN = new DevExpress.XtraEditors.TextEdit();
            this.btnRemove = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlMSG)).BeginInit();
            this.GrpControlMSG.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSN.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // GrpControlMSG
            // 
            this.GrpControlMSG.Controls.Add(this.Edt_MSG);
            this.GrpControlMSG.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.GrpControlMSG.Location = new System.Drawing.Point(0, 99);
            this.GrpControlMSG.Name = "GrpControlMSG";
            this.GrpControlMSG.Size = new System.Drawing.Size(643, 97);
            this.GrpControlMSG.TabIndex = 87;
            this.GrpControlMSG.Text = "Message";
            // 
            // Edt_MSG
            // 
            this.Edt_MSG.BackColor = System.Drawing.Color.White;
            this.Edt_MSG.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Edt_MSG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Edt_MSG.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Edt_MSG.ForeColor = System.Drawing.Color.Black;
            this.Edt_MSG.Location = new System.Drawing.Point(2, 23);
            this.Edt_MSG.Name = "Edt_MSG";
            this.Edt_MSG.ReadOnly = true;
            this.Edt_MSG.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.Edt_MSG.Size = new System.Drawing.Size(639, 72);
            this.Edt_MSG.TabIndex = 49;
            this.Edt_MSG.Text = "";
            // 
            // lblSN
            // 
            this.lblSN.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.lblSN.Appearance.Options.UseFont = true;
            this.lblSN.Location = new System.Drawing.Point(14, 39);
            this.lblSN.Name = "lblSN";
            this.lblSN.Size = new System.Drawing.Size(79, 24);
            this.lblSN.TabIndex = 90;
            this.lblSN.Text = "Scan SN:";
            // 
            // txtSN
            // 
            this.txtSN.Location = new System.Drawing.Point(97, 36);
            this.txtSN.Name = "txtSN";
            this.txtSN.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.txtSN.Properties.Appearance.Options.UseFont = true;
            this.txtSN.Size = new System.Drawing.Size(428, 30);
            this.txtSN.TabIndex = 89;
            this.txtSN.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSN_KeyDown);
            // 
            // btnRemove
            // 
            this.btnRemove.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnRemove.ImageOptions.SvgImage")));
            this.btnRemove.Location = new System.Drawing.Point(530, 34);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(91, 33);
            this.btnRemove.TabIndex = 88;
            this.btnRemove.Text = "Remove";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // PackageRemove_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 196);
            this.Controls.Add(this.lblSN);
            this.Controls.Add(this.txtSN);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.GrpControlMSG);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PackageRemove_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Devanning(拆箱)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PackageRemove_Form_FormClosing);
            this.Load += new System.EventHandler(this.PackageRemove_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlMSG)).EndInit();
            this.GrpControlMSG.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtSN.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public DevExpress.XtraEditors.GroupControl GrpControlMSG;
        public System.Windows.Forms.RichTextBox Edt_MSG;
        private DevExpress.XtraEditors.LabelControl lblSN;
        private DevExpress.XtraEditors.TextEdit txtSN;
        private DevExpress.XtraEditors.SimpleButton btnRemove;
    }
}