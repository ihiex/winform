namespace App.MyMES
{
    partial class CarrierLinkMaterialBatch_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CarrierLinkMaterialBatch_Form));
            this.lblBox = new DevExpress.XtraEditors.LabelControl();
            this.lblSNBatch = new DevExpress.XtraEditors.LabelControl();
            this.Edt_Box = new DevExpress.XtraEditors.TextEdit();
            this.Edt_BactchSN = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.DT_ProductionOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DT_ProductStructure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtDefect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlMSG)).BeginInit();
            this.GrpControlMSG.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlInputData)).BeginInit();
            this.GrpControlInputData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Com_luUnitStatus.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_PO.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_Part.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_PartFamily.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_PartFamilyType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Edt_Box.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Edt_BactchSN.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // GrpControlInputData
            // 
            this.GrpControlInputData.Controls.Add(this.lblSNBatch);
            this.GrpControlInputData.Controls.Add(this.Edt_BactchSN);
            this.GrpControlInputData.Controls.Add(this.lblBox);
            this.GrpControlInputData.Controls.Add(this.Edt_Box);
            // 
            // Com_luUnitStatus
            // 
            this.Com_luUnitStatus.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Com_luUnitStatus.Properties.Appearance.Options.UseFont = true;
            // 
            // Com_PO
            // 
            this.Com_PO.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Com_PO.Properties.Appearance.Options.UseFont = true;
            // 
            // Com_Part
            // 
            this.Com_Part.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Com_Part.Properties.Appearance.Options.UseFont = true;
            // 
            // Com_PartFamily
            // 
            this.Com_PartFamily.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Com_PartFamily.Properties.Appearance.Options.UseFont = true;
            // 
            // Com_PartFamilyType
            // 
            this.Com_PartFamilyType.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Com_PartFamilyType.Properties.Appearance.Options.UseFont = true;
            // 
            // Btn_ConfirmPO
            // 
            this.Btn_ConfirmPO.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_ConfirmPO.Appearance.Options.UseFont = true;
            this.Btn_ConfirmPO.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("Btn_ConfirmPO.ImageOptions.SvgImage")));
            // 
            // Btn_Defect
            // 
            this.Btn_Defect.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Defect.Appearance.Options.UseFont = true;
            this.Btn_Defect.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("Btn_Defect.ImageOptions.SvgImage")));
            // 
            // Btn_Refresh
            // 
            this.Btn_Refresh.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Refresh.Appearance.Options.UseFont = true;
            this.Btn_Refresh.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("Btn_Refresh.ImageOptions.SvgImage")));
            this.Btn_Refresh.Click += new System.EventHandler(this.Btn_Refresh_Click_1);
            // 
            // lblBox
            // 
            this.lblBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblBox.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lblBox.Appearance.Options.UseFont = true;
            this.lblBox.Location = new System.Drawing.Point(71, 130);
            this.lblBox.Name = "lblBox";
            this.lblBox.Size = new System.Drawing.Size(127, 19);
            this.lblBox.TabIndex = 0;
            this.lblBox.Text = "Tray SN(托盘SN):";
            // 
            // lblSNBatch
            // 
            this.lblSNBatch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblSNBatch.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lblSNBatch.Appearance.Options.UseFont = true;
            this.lblSNBatch.Location = new System.Drawing.Point(65, 169);
            this.lblSNBatch.Name = "lblSNBatch";
            this.lblSNBatch.Size = new System.Drawing.Size(133, 19);
            this.lblSNBatch.TabIndex = 1;
            this.lblSNBatch.Text = "Batch SN(批次SN):";
            // 
            // Edt_Box
            // 
            this.Edt_Box.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Edt_Box.EditValue = "";
            this.Edt_Box.Location = new System.Drawing.Point(231, 127);
            this.Edt_Box.Name = "Edt_Box";
            this.Edt_Box.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Edt_Box.Properties.Appearance.Options.UseFont = true;
            this.Edt_Box.Size = new System.Drawing.Size(682, 26);
            this.Edt_Box.TabIndex = 41;
            this.Edt_Box.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Edt_Box_KeyDown);
            // 
            // Edt_BactchSN
            // 
            this.Edt_BactchSN.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Edt_BactchSN.EditValue = "";
            this.Edt_BactchSN.Enabled = false;
            this.Edt_BactchSN.Location = new System.Drawing.Point(231, 166);
            this.Edt_BactchSN.Name = "Edt_BactchSN";
            this.Edt_BactchSN.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Edt_BactchSN.Properties.Appearance.Options.UseFont = true;
            this.Edt_BactchSN.Size = new System.Drawing.Size(682, 26);
            this.Edt_BactchSN.TabIndex = 42;
            this.Edt_BactchSN.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Edt_BactchSN_KeyDown);
            // 
            // CarrierLinkMaterialBatch_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1381, 590);
            this.Name = "CarrierLinkMaterialBatch_Form";
            this.Text = "CarrierLinkMaterialBatch";
            ((System.ComponentModel.ISupportInitialize)(this.DT_ProductionOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DT_ProductStructure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtDefect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlMSG)).EndInit();
            this.GrpControlMSG.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlInputData)).EndInit();
            this.GrpControlInputData.ResumeLayout(false);
            this.GrpControlInputData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Com_luUnitStatus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_PO.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_Part.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_PartFamily.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_PartFamilyType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Edt_Box.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Edt_BactchSN.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.TextEdit Edt_BactchSN;
        private DevExpress.XtraEditors.TextEdit Edt_Box;
        private DevExpress.XtraEditors.LabelControl lblSNBatch;
        private DevExpress.XtraEditors.LabelControl lblBox;
    }
}