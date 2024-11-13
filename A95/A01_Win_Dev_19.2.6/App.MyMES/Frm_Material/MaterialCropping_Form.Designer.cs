namespace App.MyMES
{
    partial class MaterialCropping_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MaterialCropping_Form));
            this.lblBatchNo = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.radioGroupType = new DevExpress.XtraEditors.RadioGroup();
            this.lblChildBatchQTY = new DevExpress.XtraEditors.LabelControl();
            this.lblSpecQTY = new DevExpress.XtraEditors.LabelControl();
            this.txtSpecQTY = new DevExpress.XtraEditors.SpinEdit();
            this.btnSplit = new DevExpress.XtraEditors.SimpleButton();
            this.txtRoolNo = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.ProBarControl = new DevExpress.XtraEditors.ProgressBarControl();
            this.txtParentBatchNo = new DevExpress.XtraEditors.LookUpEdit();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.Panel_Defact = new System.Windows.Forms.Panel();
            this.dataGridSN = new System.Windows.Forms.DataGridView();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.panel4 = new System.Windows.Forms.Panel();
            this.Com_PO = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.Grid_PO = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.DT_ProductionOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DT_ProductStructure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtDefect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlMSG)).BeginInit();
            this.GrpControlMSG.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlInputData)).BeginInit();
            this.GrpControlInputData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Com_Part.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_PartFamily.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_PartFamilyType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlPart)).BeginInit();
            this.GrpControlPart.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxRouteDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBalanceQTY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInitialQTY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBatchQTY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroupType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSpecQTY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRoolNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProBarControl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParentBatchNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            this.Panel_Defact.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSN)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Com_PO.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_PO)).BeginInit();
            this.SuspendLayout();
            // 
            // GrpControlMSG
            // 
            this.GrpControlMSG.Size = new System.Drawing.Size(1076, 167);
            // 
            // GrpControlInputData
            // 
            this.GrpControlInputData.Controls.Add(this.Panel_Defact);
            this.GrpControlInputData.Controls.Add(this.panelControl4);
            // 
            // Com_Part
            // 
            this.Com_Part.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Com_Part.Properties.Appearance.Options.UseFont = true;
            this.Com_Part.EditValueChanged += new System.EventHandler(this.Com_Part_EditValueChanged);
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
            this.Edt_MSG.Size = new System.Drawing.Size(1072, 142);
            // 
            // Btn_Refresh
            // 
            this.Btn_Refresh.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Refresh.Appearance.Options.UseFont = true;
            this.Btn_Refresh.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("Btn_Refresh.ImageOptions.SvgImage")));
            this.Btn_Refresh.Location = new System.Drawing.Point(18, 29);
            this.Btn_Refresh.Size = new System.Drawing.Size(111, 40);
            // 
            // GrpControlPart
            // 
            this.GrpControlPart.Controls.Add(this.panel4);
            this.GrpControlPart.Size = new System.Drawing.Size(1076, 155);
            this.GrpControlPart.Controls.SetChildIndex(this.panel1, 0);
            this.GrpControlPart.Controls.SetChildIndex(this.panel2, 0);
            this.GrpControlPart.Controls.SetChildIndex(this.panel14, 0);
            this.GrpControlPart.Controls.SetChildIndex(this.panel3, 0);
            this.GrpControlPart.Controls.SetChildIndex(this.panel11, 0);
            this.GrpControlPart.Controls.SetChildIndex(this.panel4, 0);
            // 
            // panel2
            // 
            this.panel2.Size = new System.Drawing.Size(161, 130);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelControl3);
            this.panel1.Size = new System.Drawing.Size(223, 130);
            this.panel1.Controls.SetChildIndex(this.lblPartFamilyType, 0);
            this.panel1.Controls.SetChildIndex(this.lblPartFamily, 0);
            this.panel1.Controls.SetChildIndex(this.lblPart, 0);
            this.panel1.Controls.SetChildIndex(this.labelControl3, 0);
            // 
            // lblPart
            // 
            this.lblPart.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblPart.Appearance.Options.UseFont = true;
            this.lblPart.Location = new System.Drawing.Point(141, 70);
            // 
            // lblPartFamily
            // 
            this.lblPartFamily.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblPartFamily.Appearance.Options.UseFont = true;
            this.lblPartFamily.Location = new System.Drawing.Point(89, 37);
            // 
            // lblPartFamilyType
            // 
            this.lblPartFamilyType.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblPartFamilyType.Appearance.Options.UseFont = true;
            // 
            // Btn_ConfirmPO
            // 
            this.Btn_ConfirmPO.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_ConfirmPO.Appearance.Options.UseFont = true;
            this.Btn_ConfirmPO.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("Btn_ConfirmPO.ImageOptions.SvgImage")));
            this.Btn_ConfirmPO.Location = new System.Drawing.Point(18, 80);
            this.Btn_ConfirmPO.Size = new System.Drawing.Size(111, 39);
            // 
            // listBoxRouteDetail
            // 
            this.listBoxRouteDetail.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.listBoxRouteDetail.Appearance.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBoxRouteDetail.Appearance.Options.UseBackColor = true;
            this.listBoxRouteDetail.Appearance.Options.UseFont = true;
            this.listBoxRouteDetail.Size = new System.Drawing.Size(287, 359);
            // 
            // txtBalanceQTY
            // 
            // 
            // txtInitialQTY
            // 
            // 
            // txtBatchQTY
            // 
            // 
            // lblBatchNo
            // 
            this.lblBatchNo.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lblBatchNo.Appearance.Options.UseFont = true;
            this.lblBatchNo.Location = new System.Drawing.Point(89, 46);
            this.lblBatchNo.Name = "lblBatchNo";
            this.lblBatchNo.Size = new System.Drawing.Size(124, 19);
            this.lblBatchNo.TabIndex = 0;
            this.lblBatchNo.Text = "BatchNo(批次号):";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(38, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(175, 17);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "Operation Type(操作类型)：";
            // 
            // radioGroupType
            // 
            this.radioGroupType.EditValue = "0";
            this.radioGroupType.Location = new System.Drawing.Point(226, 7);
            this.radioGroupType.Name = "radioGroupType";
            this.radioGroupType.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.radioGroupType.Properties.Appearance.Options.UseBackColor = true;
            this.radioGroupType.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.radioGroupType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("0", "大批次切小批次"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("1", "批次切物料条码")});
            this.radioGroupType.Size = new System.Drawing.Size(685, 26);
            this.radioGroupType.TabIndex = 50;
            // 
            // lblChildBatchQTY
            // 
            this.lblChildBatchQTY.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lblChildBatchQTY.Appearance.Options.UseFont = true;
            this.lblChildBatchQTY.Location = new System.Drawing.Point(113, 83);
            this.lblChildBatchQTY.Name = "lblChildBatchQTY";
            this.lblChildBatchQTY.Size = new System.Drawing.Size(100, 19);
            this.lblChildBatchQTY.TabIndex = 2;
            this.lblChildBatchQTY.Text = "ReelNo(卷号):";
            // 
            // lblSpecQTY
            // 
            this.lblSpecQTY.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lblSpecQTY.Appearance.Options.UseFont = true;
            this.lblSpecQTY.Location = new System.Drawing.Point(522, 83);
            this.lblSpecQTY.Name = "lblSpecQTY";
            this.lblSpecQTY.Size = new System.Drawing.Size(147, 19);
            this.lblSpecQTY.TabIndex = 4;
            this.lblSpecQTY.Text = "SpecQTY(规格数量):";
            // 
            // txtSpecQTY
            // 
            this.txtSpecQTY.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtSpecQTY.Location = new System.Drawing.Point(675, 79);
            this.txtSpecQTY.Name = "txtSpecQTY";
            this.txtSpecQTY.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtSpecQTY.Properties.Appearance.Options.UseFont = true;
            this.txtSpecQTY.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtSpecQTY.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
            this.txtSpecQTY.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.txtSpecQTY.Size = new System.Drawing.Size(236, 26);
            this.txtSpecQTY.TabIndex = 5;
            // 
            // btnSplit
            // 
            this.btnSplit.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSplit.Appearance.Options.UseFont = true;
            this.btnSplit.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnSplit.ImageOptions.SvgImage")));
            this.btnSplit.Location = new System.Drawing.Point(929, 98);
            this.btnSplit.Name = "btnSplit";
            this.btnSplit.Size = new System.Drawing.Size(111, 48);
            this.btnSplit.TabIndex = 84;
            this.btnSplit.Text = "Split";
            this.btnSplit.Click += new System.EventHandler(this.btnSplit_Click);
            // 
            // txtRoolNo
            // 
            this.txtRoolNo.EditValue = "";
            this.txtRoolNo.Location = new System.Drawing.Point(226, 79);
            this.txtRoolNo.Name = "txtRoolNo";
            this.txtRoolNo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtRoolNo.Properties.Appearance.Options.UseFont = true;
            this.txtRoolNo.Size = new System.Drawing.Size(263, 26);
            this.txtRoolNo.TabIndex = 3;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(58, 123);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(155, 19);
            this.labelControl2.TabIndex = 85;
            this.labelControl2.Text = "Progress Bar(进度条):";
            // 
            // ProBarControl
            // 
            this.ProBarControl.Location = new System.Drawing.Point(226, 119);
            this.ProBarControl.Name = "ProBarControl";
            this.ProBarControl.Properties.ShowTitle = true;
            this.ProBarControl.ShowProgressInTaskBar = true;
            this.ProBarControl.Size = new System.Drawing.Size(685, 27);
            this.ProBarControl.TabIndex = 86;
            // 
            // txtParentBatchNo
            // 
            this.txtParentBatchNo.Location = new System.Drawing.Point(226, 42);
            this.txtParentBatchNo.Name = "txtParentBatchNo";
            this.txtParentBatchNo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtParentBatchNo.Properties.Appearance.Options.UseFont = true;
            this.txtParentBatchNo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtParentBatchNo.Properties.NullText = "";
            this.txtParentBatchNo.Properties.PopupSizeable = false;
            this.txtParentBatchNo.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.txtParentBatchNo.Size = new System.Drawing.Size(685, 26);
            this.txtParentBatchNo.TabIndex = 1;
            this.txtParentBatchNo.EditValueChanged += new System.EventHandler(this.txtParentBatchNo_EditValueChanged);
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.labelControl1);
            this.panelControl4.Controls.Add(this.txtParentBatchNo);
            this.panelControl4.Controls.Add(this.radioGroupType);
            this.panelControl4.Controls.Add(this.txtRoolNo);
            this.panelControl4.Controls.Add(this.txtSpecQTY);
            this.panelControl4.Controls.Add(this.ProBarControl);
            this.panelControl4.Controls.Add(this.lblBatchNo);
            this.panelControl4.Controls.Add(this.labelControl2);
            this.panelControl4.Controls.Add(this.lblChildBatchQTY);
            this.panelControl4.Controls.Add(this.btnSplit);
            this.panelControl4.Controls.Add(this.lblSpecQTY);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl4.Location = new System.Drawing.Point(2, 23);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(1072, 156);
            this.panelControl4.TabIndex = 93;
            // 
            // Panel_Defact
            // 
            this.Panel_Defact.Controls.Add(this.dataGridSN);
            this.Panel_Defact.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel_Defact.Location = new System.Drawing.Point(2, 179);
            this.Panel_Defact.Name = "Panel_Defact";
            this.Panel_Defact.Size = new System.Drawing.Size(1072, 151);
            this.Panel_Defact.TabIndex = 94;
            // 
            // dataGridSN
            // 
            this.dataGridSN.AllowUserToAddRows = false;
            this.dataGridSN.AllowUserToDeleteRows = false;
            this.dataGridSN.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridSN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridSN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridSN.Location = new System.Drawing.Point(0, 0);
            this.dataGridSN.Name = "dataGridSN";
            this.dataGridSN.RowTemplate.Height = 23;
            this.dataGridSN.Size = new System.Drawing.Size(1072, 151);
            this.dataGridSN.TabIndex = 21;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(64, 102);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(154, 17);
            this.labelControl3.TabIndex = 45;
            this.labelControl3.Text = "ProductionOrder(工单)：";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.Com_PO);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(225, 119);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(3);
            this.panel4.Size = new System.Drawing.Size(688, 32);
            this.panel4.TabIndex = 52;
            // 
            // Com_PO
            // 
            this.Com_PO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Com_PO.EditValue = "";
            this.Com_PO.Location = new System.Drawing.Point(3, 3);
            this.Com_PO.Name = "Com_PO";
            this.Com_PO.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Com_PO.Properties.Appearance.Options.UseFont = true;
            this.Com_PO.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Com_PO.Properties.PopupView = this.Grid_PO;
            this.Com_PO.Size = new System.Drawing.Size(682, 26);
            this.Com_PO.TabIndex = 82;
            // 
            // Grid_PO
            // 
            this.Grid_PO.DetailHeight = 408;
            this.Grid_PO.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.Grid_PO.Name = "Grid_PO";
            this.Grid_PO.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.Grid_PO.OptionsView.ShowGroupPanel = false;
            // 
            // MaterialCropping_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1381, 706);
            this.Name = "MaterialCropping_Form";
            this.Text = "MaterialCropping_Form";
            ((System.ComponentModel.ISupportInitialize)(this.DT_ProductionOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DT_ProductStructure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtDefect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlMSG)).EndInit();
            this.GrpControlMSG.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlInputData)).EndInit();
            this.GrpControlInputData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Com_Part.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_PartFamily.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_PartFamilyType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlPart)).EndInit();
            this.GrpControlPart.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel14.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listBoxRouteDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBalanceQTY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInitialQTY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBatchQTY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroupType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSpecQTY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRoolNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProBarControl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParentBatchNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            this.panelControl4.PerformLayout();
            this.Panel_Defact.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSN)).EndInit();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Com_PO.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_PO)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.LabelControl lblSpecQTY;
        private DevExpress.XtraEditors.LabelControl lblChildBatchQTY;
        private DevExpress.XtraEditors.LabelControl lblBatchNo;
        private DevExpress.XtraEditors.SpinEdit txtSpecQTY;
        private DevExpress.XtraEditors.RadioGroup radioGroupType;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        public DevExpress.XtraEditors.SimpleButton btnSplit;
        private DevExpress.XtraEditors.ProgressBarControl ProBarControl;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtRoolNo;
        private DevExpress.XtraEditors.LookUpEdit txtParentBatchNo;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private System.Windows.Forms.Panel Panel_Defact;
        private System.Windows.Forms.DataGridView dataGridSN;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private System.Windows.Forms.Panel panel4;
        public DevExpress.XtraEditors.SearchLookUpEdit Com_PO;
        public DevExpress.XtraGrid.Views.Grid.GridView Grid_PO;
    }
}