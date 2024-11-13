namespace App.MyMES
{
    partial class Interlock_Form
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Interlock_Form));
            this.lblScanIn = new DevExpress.XtraEditors.LabelControl();
            this.Edt_SN = new DevExpress.XtraEditors.TextEdit();
            this.List_SN = new DevExpress.XtraEditors.ListBoxControl();
            this.Btn_OnLine = new DevExpress.XtraEditors.SimpleButton();
            this.Btn_Start = new DevExpress.XtraEditors.SimpleButton();
            this.Btn_End = new DevExpress.XtraEditors.SimpleButton();
            this.Btn_Run = new DevExpress.XtraEditors.SimpleButton();
            this.timer_OnLine = new System.Windows.Forms.Timer(this.components);
            this.timer_Run = new System.Windows.Forms.Timer(this.components);
            this.timer_Filish = new System.Windows.Forms.Timer(this.components);
            this.SP_Main = new System.IO.Ports.SerialPort(this.components);
            this.gpControlStatus = new DevExpress.XtraEditors.GroupControl();
            this.Lab_COM = new System.Windows.Forms.Label();
            this.Com_ComProt = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.Grid_Com = new DevExpress.XtraGrid.Views.Grid.GridView();
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
            ((System.ComponentModel.ISupportInitialize)(this.Edt_SN.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.List_SN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gpControlStatus)).BeginInit();
            this.gpControlStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Com_ComProt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Com)).BeginInit();
            this.SuspendLayout();
            // 
            // GrpControlMSG
            // 
            this.GrpControlMSG.Size = new System.Drawing.Size(1073, 185);
            // 
            // GrpControlInputData
            // 
            this.GrpControlInputData.Controls.Add(this.Com_ComProt);
            this.GrpControlInputData.Controls.Add(this.Lab_COM);
            this.GrpControlInputData.Controls.Add(this.gpControlStatus);
            this.GrpControlInputData.Controls.Add(this.List_SN);
            this.GrpControlInputData.Controls.Add(this.Edt_SN);
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
            this.Edt_MSG.Size = new System.Drawing.Size(1069, 160);
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
            this.lblScanIn.Location = new System.Drawing.Point(12, 60);
            this.lblScanIn.Name = "lblScanIn";
            this.lblScanIn.Size = new System.Drawing.Size(128, 19);
            this.lblScanIn.TabIndex = 75;
            this.lblScanIn.Text = "Scan SN(扫描SN):";
            // 
            // Edt_SN
            // 
            this.Edt_SN.Location = new System.Drawing.Point(170, 61);
            this.Edt_SN.Name = "Edt_SN";
            this.Edt_SN.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Edt_SN.Properties.Appearance.Options.UseFont = true;
            this.Edt_SN.Size = new System.Drawing.Size(552, 26);
            this.Edt_SN.TabIndex = 76;
            this.Edt_SN.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Edt_SN_KeyDown);
            // 
            // List_SN
            // 
            this.List_SN.Location = new System.Drawing.Point(170, 108);
            this.List_SN.Name = "List_SN";
            this.List_SN.Size = new System.Drawing.Size(552, 128);
            this.List_SN.TabIndex = 77;
            // 
            // Btn_OnLine
            // 
            this.Btn_OnLine.Appearance.BackColor = System.Drawing.Color.Gray;
            this.Btn_OnLine.Appearance.Options.UseBackColor = true;
            this.Btn_OnLine.Location = new System.Drawing.Point(59, 33);
            this.Btn_OnLine.Name = "Btn_OnLine";
            this.Btn_OnLine.Size = new System.Drawing.Size(73, 57);
            this.Btn_OnLine.TabIndex = 90;
            this.Btn_OnLine.Text = "在线状态";
            // 
            // Btn_Start
            // 
            this.Btn_Start.Appearance.BackColor = System.Drawing.Color.Gray;
            this.Btn_Start.Appearance.Options.UseBackColor = true;
            this.Btn_Start.Location = new System.Drawing.Point(185, 33);
            this.Btn_Start.Name = "Btn_Start";
            this.Btn_Start.Size = new System.Drawing.Size(73, 57);
            this.Btn_Start.TabIndex = 91;
            this.Btn_Start.Text = "机器启动";
            // 
            // Btn_End
            // 
            this.Btn_End.Appearance.BackColor = System.Drawing.Color.Gray;
            this.Btn_End.Appearance.Options.UseBackColor = true;
            this.Btn_End.Location = new System.Drawing.Point(185, 108);
            this.Btn_End.Name = "Btn_End";
            this.Btn_End.Size = new System.Drawing.Size(73, 57);
            this.Btn_End.TabIndex = 93;
            this.Btn_End.Text = "运行结束";
            // 
            // Btn_Run
            // 
            this.Btn_Run.Appearance.BackColor = System.Drawing.Color.Gray;
            this.Btn_Run.Appearance.Options.UseBackColor = true;
            this.Btn_Run.Location = new System.Drawing.Point(59, 108);
            this.Btn_Run.Name = "Btn_Run";
            this.Btn_Run.Size = new System.Drawing.Size(73, 57);
            this.Btn_Run.TabIndex = 92;
            this.Btn_Run.Text = "正在运行";
            // 
            // timer_OnLine
            // 
            this.timer_OnLine.Interval = 1000;
            // 
            // timer_Run
            // 
            this.timer_Run.Interval = 200;
            // 
            // timer_Filish
            // 
            this.timer_Filish.Interval = 200;
            // 
            // SP_Main
            // 
            this.SP_Main.DtrEnable = true;
            // 
            // gpControlStatus
            // 
            this.gpControlStatus.Controls.Add(this.Btn_OnLine);
            this.gpControlStatus.Controls.Add(this.Btn_End);
            this.gpControlStatus.Controls.Add(this.Btn_Start);
            this.gpControlStatus.Controls.Add(this.Btn_Run);
            this.gpControlStatus.Location = new System.Drawing.Point(755, 60);
            this.gpControlStatus.Name = "gpControlStatus";
            this.gpControlStatus.Size = new System.Drawing.Size(295, 176);
            this.gpControlStatus.TabIndex = 94;
            this.gpControlStatus.Text = "PLC Status(PLC 状态)";
            this.gpControlStatus.Visible = false;
            // 
            // Lab_COM
            // 
            this.Lab_COM.AutoSize = true;
            this.Lab_COM.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lab_COM.ForeColor = System.Drawing.Color.Blue;
            this.Lab_COM.Location = new System.Drawing.Point(719, 33);
            this.Lab_COM.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lab_COM.Name = "Lab_COM";
            this.Lab_COM.Size = new System.Drawing.Size(35, 14);
            this.Lab_COM.TabIndex = 95;
            this.Lab_COM.Text = "COM1";
            this.Lab_COM.Visible = false;
            // 
            // Com_ComProt
            // 
            this.Com_ComProt.EditValue = "";
            this.Com_ComProt.Location = new System.Drawing.Point(337, 31);
            this.Com_ComProt.Name = "Com_ComProt";
            this.Com_ComProt.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Com_ComProt.Properties.Appearance.Options.UseFont = true;
            this.Com_ComProt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Com_ComProt.Properties.PopupView = this.Grid_Com;
            this.Com_ComProt.Size = new System.Drawing.Size(377, 24);
            this.Com_ComProt.TabIndex = 96;
            this.Com_ComProt.Visible = false;
            // 
            // Grid_Com
            // 
            this.Grid_Com.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.Grid_Com.Name = "Grid_Com";
            this.Grid_Com.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.Grid_Com.OptionsView.ShowGroupPanel = false;
            // 
            // Interlock_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1380, 686);
            this.Name = "Interlock_Form";
            this.Text = "Interlock";
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
            ((System.ComponentModel.ISupportInitialize)(this.Edt_SN.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.List_SN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gpControlStatus)).EndInit();
            this.gpControlStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Com_ComProt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Com)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.ListBoxControl List_SN;
        private DevExpress.XtraEditors.TextEdit Edt_SN;
        private DevExpress.XtraEditors.LabelControl lblScanIn;
        private DevExpress.XtraEditors.SimpleButton Btn_Start;
        private DevExpress.XtraEditors.SimpleButton Btn_OnLine;
        private DevExpress.XtraEditors.SimpleButton Btn_End;
        private DevExpress.XtraEditors.SimpleButton Btn_Run;
        private System.Windows.Forms.Timer timer_OnLine;
        private System.Windows.Forms.Timer timer_Run;
        private System.Windows.Forms.Timer timer_Filish;
        private System.IO.Ports.SerialPort SP_Main;
        private DevExpress.XtraEditors.GroupControl gpControlStatus;
        private System.Windows.Forms.Label Lab_COM;
        private DevExpress.XtraEditors.SearchLookUpEdit Com_ComProt;
        private DevExpress.XtraGrid.Views.Grid.GridView Grid_Com;
    }
}