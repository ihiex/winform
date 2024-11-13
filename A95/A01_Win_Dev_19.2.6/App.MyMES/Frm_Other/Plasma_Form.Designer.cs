namespace App.MyMES
{
    partial class Plasma_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Plasma_Form));
            this.lblScanIn = new DevExpress.XtraEditors.LabelControl();
            this.lblScanOut = new DevExpress.XtraEditors.LabelControl();
            this.txtScanIn = new DevExpress.XtraEditors.TextEdit();
            this.txtScanOut = new DevExpress.XtraEditors.TextEdit();
            this.btnScanIn = new DevExpress.XtraEditors.SimpleButton();
            this.BtnMoveScanIn = new DevExpress.XtraEditors.SimpleButton();
            this.btnScanOut = new DevExpress.XtraEditors.SimpleButton();
            this.BtnMoveScanOut = new DevExpress.XtraEditors.SimpleButton();
            this.DataGridViewScan = new DevExpress.XtraGrid.GridControl();
            this.gdViewData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SerialNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ScanInTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ScanOutTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GroupID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Status = new DevExpress.XtraGrid.Columns.GridColumn();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtScanIn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtScanOut.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewScan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdViewData)).BeginInit();
            this.SuspendLayout();
            // 
            // GrpControlMSG
            // 
            this.GrpControlMSG.Size = new System.Drawing.Size(1073, 153);
            // 
            // GrpControlInputData
            // 
            this.GrpControlInputData.Controls.Add(this.DataGridViewScan);
            this.GrpControlInputData.Controls.Add(this.BtnMoveScanOut);
            this.GrpControlInputData.Controls.Add(this.btnScanOut);
            this.GrpControlInputData.Controls.Add(this.BtnMoveScanIn);
            this.GrpControlInputData.Controls.Add(this.btnScanIn);
            this.GrpControlInputData.Controls.Add(this.txtScanOut);
            this.GrpControlInputData.Controls.Add(this.txtScanIn);
            this.GrpControlInputData.Controls.Add(this.lblScanOut);
            this.GrpControlInputData.Controls.Add(this.lblScanIn);
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
            // Edt_MSG
            // 
            this.Edt_MSG.Size = new System.Drawing.Size(1069, 128);
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
            // 
            // lblScanIn
            // 
            this.lblScanIn.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lblScanIn.Appearance.Options.UseFont = true;
            this.lblScanIn.Location = new System.Drawing.Point(17, 88);
            this.lblScanIn.Name = "lblScanIn";
            this.lblScanIn.Size = new System.Drawing.Size(129, 19);
            this.lblScanIn.TabIndex = 0;
            this.lblScanIn.Text = "Scan In(扫进条码)";
            // 
            // lblScanOut
            // 
            this.lblScanOut.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lblScanOut.Appearance.Options.UseFont = true;
            this.lblScanOut.Location = new System.Drawing.Point(7, 188);
            this.lblScanOut.Name = "lblScanOut";
            this.lblScanOut.Size = new System.Drawing.Size(140, 19);
            this.lblScanOut.TabIndex = 1;
            this.lblScanOut.Text = "Scan Out(扫出条码)";
            // 
            // txtScanIn
            // 
            this.txtScanIn.Location = new System.Drawing.Point(155, 88);
            this.txtScanIn.Name = "txtScanIn";
            this.txtScanIn.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtScanIn.Properties.Appearance.Options.UseFont = true;
            this.txtScanIn.Size = new System.Drawing.Size(431, 26);
            this.txtScanIn.TabIndex = 2;
            this.txtScanIn.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtScanIn_KeyDown);
            // 
            // txtScanOut
            // 
            this.txtScanOut.Location = new System.Drawing.Point(155, 188);
            this.txtScanOut.Name = "txtScanOut";
            this.txtScanOut.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtScanOut.Properties.Appearance.Options.UseFont = true;
            this.txtScanOut.Properties.ReadOnly = true;
            this.txtScanOut.Size = new System.Drawing.Size(431, 26);
            this.txtScanOut.TabIndex = 3;
            this.txtScanOut.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtScanOut_KeyDown);
            // 
            // btnScanIn
            // 
            this.btnScanIn.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnScanIn.ImageOptions.SvgImage")));
            this.btnScanIn.Location = new System.Drawing.Point(155, 41);
            this.btnScanIn.Name = "btnScanIn";
            this.btnScanIn.Size = new System.Drawing.Size(135, 32);
            this.btnScanIn.TabIndex = 4;
            this.btnScanIn.Text = "我要扫进";
            this.btnScanIn.Click += new System.EventHandler(this.btnScanIn_Click);
            // 
            // BtnMoveScanIn
            // 
            this.BtnMoveScanIn.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnMoveScanIn.ImageOptions.SvgImage")));
            this.BtnMoveScanIn.Location = new System.Drawing.Point(390, 41);
            this.BtnMoveScanIn.Name = "BtnMoveScanIn";
            this.BtnMoveScanIn.Size = new System.Drawing.Size(155, 32);
            this.BtnMoveScanIn.TabIndex = 5;
            this.BtnMoveScanIn.Text = "移除当前扫进";
            this.BtnMoveScanIn.Click += new System.EventHandler(this.BtnMoveScanIn_Click);
            // 
            // btnScanOut
            // 
            this.btnScanOut.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnScanOut.ImageOptions.SvgImage")));
            this.btnScanOut.Location = new System.Drawing.Point(155, 142);
            this.btnScanOut.Name = "btnScanOut";
            this.btnScanOut.Size = new System.Drawing.Size(135, 32);
            this.btnScanOut.TabIndex = 6;
            this.btnScanOut.Text = "我要扫出";
            this.btnScanOut.Click += new System.EventHandler(this.btnScanOut_Click);
            // 
            // BtnMoveScanOut
            // 
            this.BtnMoveScanOut.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnMoveScanOut.ImageOptions.SvgImage")));
            this.BtnMoveScanOut.Location = new System.Drawing.Point(390, 142);
            this.BtnMoveScanOut.Name = "BtnMoveScanOut";
            this.BtnMoveScanOut.Size = new System.Drawing.Size(155, 32);
            this.BtnMoveScanOut.TabIndex = 7;
            this.BtnMoveScanOut.Text = "移除当前扫出";
            // 
            // DataGridViewScan
            // 
            this.DataGridViewScan.Dock = System.Windows.Forms.DockStyle.Right;
            this.DataGridViewScan.Location = new System.Drawing.Point(627, 23);
            this.DataGridViewScan.MainView = this.gdViewData;
            this.DataGridViewScan.Name = "DataGridViewScan";
            this.DataGridViewScan.Size = new System.Drawing.Size(444, 105);
            this.DataGridViewScan.TabIndex = 8;
            this.DataGridViewScan.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gdViewData});
            // 
            // gdViewData
            // 
            this.gdViewData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.NO,
            this.SerialNumber,
            this.ScanInTime,
            this.ScanOutTime,
            this.GroupID,
            this.Status});
            this.gdViewData.GridControl = this.DataGridViewScan;
            this.gdViewData.Name = "gdViewData";
            this.gdViewData.OptionsBehavior.Editable = false;
            this.gdViewData.OptionsView.ShowGroupPanel = false;
            // 
            // NO
            // 
            this.NO.Caption = "序号";
            this.NO.FieldName = "NO";
            this.NO.Name = "NO";
            this.NO.Visible = true;
            this.NO.VisibleIndex = 0;
            this.NO.Width = 40;
            // 
            // SerialNumber
            // 
            this.SerialNumber.Caption = "条码编号";
            this.SerialNumber.FieldName = "SerialNumber";
            this.SerialNumber.Name = "SerialNumber";
            this.SerialNumber.Visible = true;
            this.SerialNumber.VisibleIndex = 1;
            this.SerialNumber.Width = 138;
            // 
            // ScanInTime
            // 
            this.ScanInTime.Caption = "扫进时间";
            this.ScanInTime.FieldName = "ScanInTime";
            this.ScanInTime.Name = "ScanInTime";
            this.ScanInTime.Visible = true;
            this.ScanInTime.VisibleIndex = 2;
            this.ScanInTime.Width = 69;
            // 
            // ScanOutTime
            // 
            this.ScanOutTime.Caption = "扫出时间";
            this.ScanOutTime.FieldName = "ScanOutTime";
            this.ScanOutTime.Name = "ScanOutTime";
            this.ScanOutTime.Visible = true;
            this.ScanOutTime.VisibleIndex = 3;
            this.ScanOutTime.Width = 69;
            // 
            // GroupID
            // 
            this.GroupID.Caption = "组别";
            this.GroupID.FieldName = "GroupID";
            this.GroupID.Name = "GroupID";
            this.GroupID.Visible = true;
            this.GroupID.VisibleIndex = 4;
            this.GroupID.Width = 50;
            // 
            // Status
            // 
            this.Status.Caption = "状态";
            this.Status.FieldName = "Status";
            this.Status.Name = "Status";
            this.Status.Visible = true;
            this.Status.VisibleIndex = 5;
            this.Status.Width = 55;
            // 
            // Plasma_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1381, 654);
            this.Name = "Plasma_Form";
            this.Text = "Plasma_Form";
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
            ((System.ComponentModel.ISupportInitialize)(this.txtScanIn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtScanOut.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewScan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdViewData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblScanOut;
        private DevExpress.XtraEditors.LabelControl lblScanIn;
        private DevExpress.XtraEditors.TextEdit txtScanOut;
        private DevExpress.XtraEditors.TextEdit txtScanIn;
        private DevExpress.XtraEditors.SimpleButton btnScanIn;
        private DevExpress.XtraEditors.SimpleButton BtnMoveScanOut;
        private DevExpress.XtraEditors.SimpleButton btnScanOut;
        private DevExpress.XtraEditors.SimpleButton BtnMoveScanIn;
        private DevExpress.XtraGrid.GridControl DataGridViewScan;
        private DevExpress.XtraGrid.Views.Grid.GridView gdViewData;
        private DevExpress.XtraGrid.Columns.GridColumn NO;
        private DevExpress.XtraGrid.Columns.GridColumn SerialNumber;
        private DevExpress.XtraGrid.Columns.GridColumn ScanInTime;
        private DevExpress.XtraGrid.Columns.GridColumn ScanOutTime;
        private DevExpress.XtraGrid.Columns.GridColumn GroupID;
        private DevExpress.XtraGrid.Columns.GridColumn Status;
    }
}