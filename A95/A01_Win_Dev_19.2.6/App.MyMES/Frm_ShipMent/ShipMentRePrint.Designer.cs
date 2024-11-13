namespace App.MyMES
{
    partial class ShipMentRePrint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShipMentRePrint));
            this.btnRePrint = new DevExpress.XtraEditors.SimpleButton();
            this.txtSN = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.GrpControlMSG = new DevExpress.XtraEditors.GroupControl();
            this.Edt_MSG = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtSN.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlMSG)).BeginInit();
            this.GrpControlMSG.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRePrint
            // 
            this.btnRePrint.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnRePrint.ImageOptions.SvgImage")));
            this.btnRePrint.Location = new System.Drawing.Point(530, 32);
            this.btnRePrint.Name = "btnRePrint";
            this.btnRePrint.Size = new System.Drawing.Size(91, 33);
            this.btnRePrint.TabIndex = 0;
            this.btnRePrint.Text = "RePrint";
            this.btnRePrint.Click += new System.EventHandler(this.btnRePrint_Click);
            // 
            // txtSN
            // 
            this.txtSN.Location = new System.Drawing.Point(93, 34);
            this.txtSN.Name = "txtSN";
            this.txtSN.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.txtSN.Properties.Appearance.Options.UseFont = true;
            this.txtSN.Size = new System.Drawing.Size(428, 30);
            this.txtSN.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(14, 37);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(70, 24);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "Box SN:";
            // 
            // GrpControlMSG
            // 
            this.GrpControlMSG.Controls.Add(this.Edt_MSG);
            this.GrpControlMSG.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.GrpControlMSG.Location = new System.Drawing.Point(0, 99);
            this.GrpControlMSG.Name = "GrpControlMSG";
            this.GrpControlMSG.Size = new System.Drawing.Size(643, 97);
            this.GrpControlMSG.TabIndex = 86;
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
            // ShipMentRePrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 196);
            this.Controls.Add(this.GrpControlMSG);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtSN);
            this.Controls.Add(this.btnRePrint);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShipMentRePrint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RePrint";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShipMentRePrint_FormClosing);
            this.Load += new System.EventHandler(this.ShipMentRePrint_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtSN.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlMSG)).EndInit();
            this.GrpControlMSG.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnRePrint;
        private DevExpress.XtraEditors.TextEdit txtSN;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        public DevExpress.XtraEditors.GroupControl GrpControlMSG;
        public System.Windows.Forms.RichTextBox Edt_MSG;
    }
}