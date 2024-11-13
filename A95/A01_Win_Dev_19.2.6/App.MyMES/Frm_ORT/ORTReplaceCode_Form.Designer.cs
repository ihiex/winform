namespace App.MyMES
{
    partial class ORTReplaceCode_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ORTReplaceCode_Form));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnConfim = new DevExpress.XtraEditors.SimpleButton();
            this.txtNewORTCode = new DevExpress.XtraEditors.TextEdit();
            this.lblNewORTCode = new DevExpress.XtraEditors.LabelControl();
            this.txtOldORTCode = new DevExpress.XtraEditors.TextEdit();
            this.lblOldORTCode = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNewORTCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOldORTCode.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnConfim);
            this.panelControl1.Controls.Add(this.txtNewORTCode);
            this.panelControl1.Controls.Add(this.lblNewORTCode);
            this.panelControl1.Controls.Add(this.txtOldORTCode);
            this.panelControl1.Controls.Add(this.lblOldORTCode);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(415, 83);
            this.panelControl1.TabIndex = 0;
            // 
            // btnConfim
            // 
            this.btnConfim.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnConfim.ImageOptions.SvgImage")));
            this.btnConfim.Location = new System.Drawing.Point(308, 10);
            this.btnConfim.Name = "btnConfim";
            this.btnConfim.Size = new System.Drawing.Size(82, 59);
            this.btnConfim.TabIndex = 5;
            this.btnConfim.Text = "确定";
            this.btnConfim.Click += new System.EventHandler(this.btnConfim_Click);
            // 
            // txtNewORTCode
            // 
            this.txtNewORTCode.Location = new System.Drawing.Point(107, 43);
            this.txtNewORTCode.Name = "txtNewORTCode";
            this.txtNewORTCode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtNewORTCode.Properties.Appearance.Options.UseFont = true;
            this.txtNewORTCode.Size = new System.Drawing.Size(178, 26);
            this.txtNewORTCode.TabIndex = 4;
            // 
            // lblNewORTCode
            // 
            this.lblNewORTCode.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lblNewORTCode.Appearance.Options.UseFont = true;
            this.lblNewORTCode.Location = new System.Drawing.Point(12, 48);
            this.lblNewORTCode.Name = "lblNewORTCode";
            this.lblNewORTCode.Size = new System.Drawing.Size(48, 19);
            this.lblNewORTCode.TabIndex = 3;
            this.lblNewORTCode.Text = "新编号";
            // 
            // txtOldORTCode
            // 
            this.txtOldORTCode.Location = new System.Drawing.Point(107, 11);
            this.txtOldORTCode.Name = "txtOldORTCode";
            this.txtOldORTCode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtOldORTCode.Properties.Appearance.Options.UseFont = true;
            this.txtOldORTCode.Properties.ReadOnly = true;
            this.txtOldORTCode.Size = new System.Drawing.Size(178, 26);
            this.txtOldORTCode.TabIndex = 2;
            // 
            // lblOldORTCode
            // 
            this.lblOldORTCode.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lblOldORTCode.Appearance.Options.UseFont = true;
            this.lblOldORTCode.Location = new System.Drawing.Point(12, 14);
            this.lblOldORTCode.Name = "lblOldORTCode";
            this.lblOldORTCode.Size = new System.Drawing.Size(48, 19);
            this.lblOldORTCode.TabIndex = 1;
            this.lblOldORTCode.Text = "旧编号";
            // 
            // ORTReplaceCode_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 83);
            this.Controls.Add(this.panelControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ORTReplaceCode_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.ORTAddWeek_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNewORTCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOldORTCode.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnConfim;
        private DevExpress.XtraEditors.TextEdit txtNewORTCode;
        private DevExpress.XtraEditors.LabelControl lblNewORTCode;
        private DevExpress.XtraEditors.TextEdit txtOldORTCode;
        private DevExpress.XtraEditors.LabelControl lblOldORTCode;
    }
}