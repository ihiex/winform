namespace App.MyMES
{
    partial class AssemblyPanleNoPoNew_Form
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
            DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions buttonImageOptions1 = new DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssemblyPanleNoPo_Form));
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtChildSN = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnReset = new DevExpress.XtraEditors.SimpleButton();
            this.txtMainSN = new DevExpress.XtraEditors.TextEdit();
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
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlRouteDetail)).BeginInit();
            this.panel15.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlPart)).BeginInit();
            this.GrpControlPart.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtChildSN.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMainSN.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // GrpControlMSG
            // 
            this.GrpControlMSG.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.GrpControlMSG.Size = new System.Drawing.Size(1139, 66);
            // 
            // GrpControlInputData
            // 
            this.GrpControlInputData.Controls.Add(this.tableLayoutPanel2);
            this.GrpControlInputData.CustomHeaderButtons.AddRange(new DevExpress.XtraEditors.ButtonPanel.IBaseButton[] {
            new DevExpress.XtraEditors.ButtonsPanelControl.GroupBoxButton("1/1", true, buttonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, true, null, true, false, true, null, -1)});
            this.GrpControlInputData.CustomHeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText;
            this.GrpControlInputData.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.GrpControlInputData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // Com_luUnitStatus
            // 
            this.Com_luUnitStatus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Com_luUnitStatus.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Com_luUnitStatus.Properties.Appearance.Options.UseFont = true;
            // 
            // Com_PO
            // 
            this.Com_PO.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Com_PO.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Com_PO.Properties.Appearance.Options.UseFont = true;
            this.Com_PO.Visible = false;
            // 
            // Com_Part
            // 
            this.Com_Part.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Com_Part.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Com_Part.Properties.Appearance.Options.UseFont = true;
            this.Com_Part.Visible = false;
            // 
            // Com_PartFamily
            // 
            this.Com_PartFamily.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Com_PartFamily.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Com_PartFamily.Properties.Appearance.Options.UseFont = true;
            this.Com_PartFamily.Visible = false;
            // 
            // Com_PartFamilyType
            // 
            this.Com_PartFamilyType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Com_PartFamilyType.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Com_PartFamilyType.Properties.Appearance.Options.UseFont = true;
            this.Com_PartFamilyType.Visible = false;
            // 
            // Edt_MSG
            // 
            this.Edt_MSG.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Edt_MSG.Size = new System.Drawing.Size(1135, 36);
            // 
            // Btn_ConfirmPO
            // 
            this.Btn_ConfirmPO.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_ConfirmPO.Appearance.Options.UseFont = true;
            this.Btn_ConfirmPO.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("Btn_ConfirmPO.ImageOptions.SvgImage")));
            this.Btn_ConfirmPO.Visible = false;
            // 
            // Panel_Defact
            // 
            this.Panel_Defact.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.Btn_Refresh.Visible = false;
            // 
            // GrpControlRouteDetail
            // 
            this.GrpControlRouteDetail.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.GrpControlRouteDetail.Size = new System.Drawing.Size(239, 341);
            // 
            // lblUnitState
            // 
            this.lblUnitState.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblUnitState.Appearance.Options.UseFont = true;
            // 
            // lblProductionOrder
            // 
            this.lblProductionOrder.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblProductionOrder.Appearance.Options.UseFont = true;
            this.lblProductionOrder.Visible = false;
            // 
            // lblPart
            // 
            this.lblPart.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblPart.Appearance.Options.UseFont = true;
            this.lblPart.Visible = false;
            // 
            // lblPartFamily
            // 
            this.lblPartFamily.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblPartFamily.Appearance.Options.UseFont = true;
            this.lblPartFamily.Visible = false;
            // 
            // lblPartFamilyType
            // 
            this.lblPartFamilyType.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblPartFamilyType.Appearance.Options.UseFont = true;
            this.lblPartFamilyType.Visible = false;
            // 
            // GrpControlPart
            // 
            this.GrpControlPart.Visible = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.43395F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.46806F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.09799F));
            this.tableLayoutPanel2.Controls.Add(this.labelControl2, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtChildSN, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.labelControl1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnReset, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtMainSN, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(2, 28);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1135, 205);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Appearance.Options.UseTextOptions = true;
            this.labelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.Location = new System.Drawing.Point(3, 106);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(226, 26);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "AssemblySN(组装SN):";
            // 
            // txtChildSN
            // 
            this.txtChildSN.Location = new System.Drawing.Point(268, 106);
            this.txtChildSN.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtChildSN.Name = "txtChildSN";
            this.txtChildSN.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtChildSN.Properties.Appearance.Options.UseFont = true;
            this.txtChildSN.Size = new System.Drawing.Size(594, 30);
            this.txtChildSN.TabIndex = 3;
            this.txtChildSN.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtChildSN_KeyDown);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseTextOptions = true;
            this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl1.Location = new System.Drawing.Point(3, 55);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(226, 26);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "MianSN(主条码):";
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(954, 106);
            this.btnReset.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(107, 26);
            this.btnReset.TabIndex = 4;
            this.btnReset.Text = "重置";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // txtMainSN
            // 
            this.txtMainSN.Location = new System.Drawing.Point(268, 55);
            this.txtMainSN.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMainSN.Name = "txtMainSN";
            this.txtMainSN.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtMainSN.Properties.Appearance.Options.UseFont = true;
            this.txtMainSN.Size = new System.Drawing.Size(594, 30);
            this.txtMainSN.TabIndex = 2;
            this.txtMainSN.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMainSN_KeyDown);
            // 
            // AssemblyPanleNoPo_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1394, 778);
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "AssemblyPanleNoPoNew_Form";
            this.Text = "AssemblyPanle_Form";
            ((System.ComponentModel.ISupportInitialize)(this.DT_ProductionOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DT_ProductStructure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtDefect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlMSG)).EndInit();
            this.GrpControlMSG.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlInputData)).EndInit();
            this.GrpControlInputData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Com_luUnitStatus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_PO.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_Part.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_PartFamily.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_PartFamilyType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlRouteDetail)).EndInit();
            this.panel15.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel14.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlPart)).EndInit();
            this.GrpControlPart.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtChildSN.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMainSN.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtMainSN;
        private DevExpress.XtraEditors.TextEdit txtChildSN;
        private DevExpress.XtraEditors.SimpleButton btnReset;
    }
}