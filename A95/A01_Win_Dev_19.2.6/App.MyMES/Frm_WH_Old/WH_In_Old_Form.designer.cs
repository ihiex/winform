namespace App.MyMES
{
    partial class WH_In_Old_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WH_In_Old_Form));
            this.GrpControlPart = new DevExpress.XtraEditors.GroupControl();
            this.btnRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.txtOverStationQTY = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.Btn_UnLock = new DevExpress.XtraEditors.SimpleButton();
            this.Edt_MPN = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.Lab_UnReciepted = new DevExpress.XtraEditors.LabelControl();
            this.Lab_Reciepted = new DevExpress.XtraEditors.LabelControl();
            this.Lab_TotalBox = new DevExpress.XtraEditors.LabelControl();
            this.Edt_SN = new DevExpress.XtraEditors.TextEdit();
            this.Lab_SN = new DevExpress.XtraEditors.LabelControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grdColumnID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdColumnPONO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdColumnLineItem = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdColumnFCTN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdColumnFOutSN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnFMPNNO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.GrpControlMSG = new DevExpress.XtraEditors.GroupControl();
            this.Edt_MSG = new System.Windows.Forms.RichTextBox();
            this.GrpControlInputData = new DevExpress.XtraEditors.GroupControl();
            this.Grid_Main = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Check_NotIn = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlPart)).BeginInit();
            this.GrpControlPart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtOverStationQTY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Edt_MPN.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Edt_SN.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlMSG)).BeginInit();
            this.GrpControlMSG.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlInputData)).BeginInit();
            this.GrpControlInputData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // GrpControlPart
            // 
            this.GrpControlPart.Controls.Add(this.Check_NotIn);
            this.GrpControlPart.Controls.Add(this.btnRefresh);
            this.GrpControlPart.Controls.Add(this.txtOverStationQTY);
            this.GrpControlPart.Controls.Add(this.labelControl2);
            this.GrpControlPart.Controls.Add(this.Btn_UnLock);
            this.GrpControlPart.Controls.Add(this.Edt_MPN);
            this.GrpControlPart.Controls.Add(this.labelControl1);
            this.GrpControlPart.Controls.Add(this.Lab_UnReciepted);
            this.GrpControlPart.Controls.Add(this.Lab_Reciepted);
            this.GrpControlPart.Controls.Add(this.Lab_TotalBox);
            this.GrpControlPart.Controls.Add(this.Edt_SN);
            this.GrpControlPart.Controls.Add(this.Lab_SN);
            this.GrpControlPart.Dock = System.Windows.Forms.DockStyle.Top;
            this.GrpControlPart.Location = new System.Drawing.Point(0, 0);
            this.GrpControlPart.Name = "GrpControlPart";
            this.GrpControlPart.Size = new System.Drawing.Size(913, 153);
            this.GrpControlPart.TabIndex = 96;
            this.GrpControlPart.Text = "InputData";
            // 
            // btnRefresh
            // 
            this.btnRefresh.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.ImageOptions.Image")));
            this.btnRefresh.Location = new System.Drawing.Point(1132, 92);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(28, 21);
            this.btnRefresh.TabIndex = 98;
            this.btnRefresh.Visible = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // txtOverStationQTY
            // 
            this.txtOverStationQTY.EditValue = "0";
            this.txtOverStationQTY.Enabled = false;
            this.txtOverStationQTY.Location = new System.Drawing.Point(1034, 92);
            this.txtOverStationQTY.Name = "txtOverStationQTY";
            this.txtOverStationQTY.Size = new System.Drawing.Size(87, 20);
            this.txtOverStationQTY.TabIndex = 99;
            this.txtOverStationQTY.Visible = false;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.LineLocation = DevExpress.XtraEditors.LineLocation.Right;
            this.labelControl2.Location = new System.Drawing.Point(915, 94);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(113, 17);
            this.labelControl2.TabIndex = 97;
            this.labelControl2.Text = "OverStationQTY：";
            this.labelControl2.Visible = false;
            // 
            // Btn_UnLock
            // 
            this.Btn_UnLock.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_UnLock.Appearance.Options.UseFont = true;
            this.Btn_UnLock.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("Btn_UnLock.ImageOptions.Image")));
            this.Btn_UnLock.Location = new System.Drawing.Point(1157, 53);
            this.Btn_UnLock.Name = "Btn_UnLock";
            this.Btn_UnLock.Size = new System.Drawing.Size(77, 26);
            this.Btn_UnLock.TabIndex = 96;
            this.Btn_UnLock.Text = "UnLock";
            this.Btn_UnLock.Visible = false;
            this.Btn_UnLock.Click += new System.EventHandler(this.Btn_UnLock_Click);
            // 
            // Edt_MPN
            // 
            this.Edt_MPN.Location = new System.Drawing.Point(973, 52);
            this.Edt_MPN.Name = "Edt_MPN";
            this.Edt_MPN.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Edt_MPN.Properties.Appearance.Options.UseFont = true;
            this.Edt_MPN.Size = new System.Drawing.Size(170, 26);
            this.Edt_MPN.TabIndex = 0;
            this.Edt_MPN.Visible = false;
            this.Edt_MPN.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Edt_MPN_KeyDown);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.LineLocation = DevExpress.XtraEditors.LineLocation.Right;
            this.labelControl1.Location = new System.Drawing.Point(924, 56);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 19);
            this.labelControl1.TabIndex = 94;
            this.labelControl1.Text = "MPN：";
            this.labelControl1.Visible = false;
            // 
            // Lab_UnReciepted
            // 
            this.Lab_UnReciepted.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.Lab_UnReciepted.Appearance.Options.UseFont = true;
            this.Lab_UnReciepted.LineLocation = DevExpress.XtraEditors.LineLocation.Right;
            this.Lab_UnReciepted.Location = new System.Drawing.Point(281, 122);
            this.Lab_UnReciepted.Name = "Lab_UnReciepted";
            this.Lab_UnReciepted.Size = new System.Drawing.Size(91, 17);
            this.Lab_UnReciepted.TabIndex = 92;
            this.Lab_UnReciepted.Text = "UnReciepted：";
            // 
            // Lab_Reciepted
            // 
            this.Lab_Reciepted.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.Lab_Reciepted.Appearance.Options.UseFont = true;
            this.Lab_Reciepted.LineLocation = DevExpress.XtraEditors.LineLocation.Right;
            this.Lab_Reciepted.Location = new System.Drawing.Point(156, 122);
            this.Lab_Reciepted.Name = "Lab_Reciepted";
            this.Lab_Reciepted.Size = new System.Drawing.Size(74, 17);
            this.Lab_Reciepted.TabIndex = 91;
            this.Lab_Reciepted.Text = "Reciepted：";
            // 
            // Lab_TotalBox
            // 
            this.Lab_TotalBox.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.Lab_TotalBox.Appearance.Options.UseFont = true;
            this.Lab_TotalBox.LineLocation = DevExpress.XtraEditors.LineLocation.Right;
            this.Lab_TotalBox.Location = new System.Drawing.Point(49, 121);
            this.Lab_TotalBox.Name = "Lab_TotalBox";
            this.Lab_TotalBox.Size = new System.Drawing.Size(72, 17);
            this.Lab_TotalBox.TabIndex = 90;
            this.Lab_TotalBox.Text = "Total Box：";
            // 
            // Edt_SN
            // 
            this.Edt_SN.Location = new System.Drawing.Point(135, 75);
            this.Edt_SN.Name = "Edt_SN";
            this.Edt_SN.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Edt_SN.Properties.Appearance.Options.UseFont = true;
            this.Edt_SN.Size = new System.Drawing.Size(430, 26);
            this.Edt_SN.TabIndex = 1;
            this.Edt_SN.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Edt_SN_KeyDown);
            // 
            // Lab_SN
            // 
            this.Lab_SN.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lab_SN.Appearance.Options.UseFont = true;
            this.Lab_SN.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.Lab_SN.LineLocation = DevExpress.XtraEditors.LineLocation.Right;
            this.Lab_SN.Location = new System.Drawing.Point(34, 79);
            this.Lab_SN.Name = "Lab_SN";
            this.Lab_SN.Size = new System.Drawing.Size(89, 17);
            this.Lab_SN.TabIndex = 42;
            this.Lab_SN.Text = "Scan BoxSN：";
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.grdColumnID,
            this.grdColumnPONO,
            this.grdColumnLineItem,
            this.grdColumnFCTN,
            this.grdColumnFOutSN,
            this.gridColumnFMPNNO});
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.ReadOnly = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // grdColumnID
            // 
            this.grdColumnID.Caption = "ID";
            this.grdColumnID.FieldName = "FDetailID";
            this.grdColumnID.Name = "grdColumnID";
            // 
            // grdColumnPONO
            // 
            this.grdColumnPONO.Caption = "KPO#";
            this.grdColumnPONO.FieldName = "FKPONO";
            this.grdColumnPONO.Name = "grdColumnPONO";
            this.grdColumnPONO.Visible = true;
            this.grdColumnPONO.VisibleIndex = 1;
            // 
            // grdColumnLineItem
            // 
            this.grdColumnLineItem.Caption = "LineItem";
            this.grdColumnLineItem.FieldName = "FLineItem";
            this.grdColumnLineItem.Name = "grdColumnLineItem";
            this.grdColumnLineItem.Visible = true;
            this.grdColumnLineItem.VisibleIndex = 0;
            // 
            // grdColumnFCTN
            // 
            this.grdColumnFCTN.Caption = "CTN";
            this.grdColumnFCTN.FieldName = "FCTN";
            this.grdColumnFCTN.Name = "grdColumnFCTN";
            this.grdColumnFCTN.Visible = true;
            this.grdColumnFCTN.VisibleIndex = 3;
            // 
            // grdColumnFOutSN
            // 
            this.grdColumnFOutSN.Caption = "OutSN";
            this.grdColumnFOutSN.FieldName = "FOutSN";
            this.grdColumnFOutSN.Name = "grdColumnFOutSN";
            this.grdColumnFOutSN.Visible = true;
            this.grdColumnFOutSN.VisibleIndex = 4;
            // 
            // gridColumnFMPNNO
            // 
            this.gridColumnFMPNNO.Caption = "MPN";
            this.gridColumnFMPNNO.FieldName = "FMPNNO";
            this.gridColumnFMPNNO.Name = "gridColumnFMPNNO";
            this.gridColumnFMPNNO.Visible = true;
            this.gridColumnFMPNNO.VisibleIndex = 2;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "2.png");
            this.imageList1.Images.SetKeyName(1, "1.png");
            // 
            // GrpControlMSG
            // 
            this.GrpControlMSG.Controls.Add(this.Edt_MSG);
            this.GrpControlMSG.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.GrpControlMSG.Location = new System.Drawing.Point(0, 368);
            this.GrpControlMSG.Name = "GrpControlMSG";
            this.GrpControlMSG.Size = new System.Drawing.Size(913, 162);
            this.GrpControlMSG.TabIndex = 99;
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
            this.Edt_MSG.Size = new System.Drawing.Size(909, 137);
            this.Edt_MSG.TabIndex = 49;
            this.Edt_MSG.Text = "";
            // 
            // GrpControlInputData
            // 
            this.GrpControlInputData.Controls.Add(this.Grid_Main);
            this.GrpControlInputData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GrpControlInputData.Location = new System.Drawing.Point(0, 153);
            this.GrpControlInputData.Name = "GrpControlInputData";
            this.GrpControlInputData.Size = new System.Drawing.Size(913, 215);
            this.GrpControlInputData.TabIndex = 100;
            this.GrpControlInputData.Text = "Grid Data";
            // 
            // Grid_Main
            // 
            this.Grid_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Grid_Main.Location = new System.Drawing.Point(2, 23);
            this.Grid_Main.MainView = this.gridView2;
            this.Grid_Main.Name = "Grid_Main";
            this.Grid_Main.Size = new System.Drawing.Size(909, 190);
            this.Grid_Main.TabIndex = 97;
            this.Grid_Main.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4});
            this.gridView2.GridControl = this.Grid_Main;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.ReadOnly = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "PalletID";
            this.gridColumn1.FieldName = "PalletID";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.ReadOnly = true;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "SerialNumber";
            this.gridColumn2.FieldName = "SerialNumber";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.ReadOnly = true;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "isReceipt";
            this.gridColumn3.FieldName = "isReceipt";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "ReceiptDate";
            this.gridColumn4.DisplayFormat.FormatString = "yyyy-MM-dd hh:mm:ss";
            this.gridColumn4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn4.FieldName = "ReceiptDate";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.ReadOnly = true;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            // 
            // Check_NotIn
            // 
            this.Check_NotIn.AutoSize = true;
            this.Check_NotIn.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Check_NotIn.Location = new System.Drawing.Point(135, 37);
            this.Check_NotIn.Name = "Check_NotIn";
            this.Check_NotIn.Size = new System.Drawing.Size(69, 21);
            this.Check_NotIn.TabIndex = 101;
            this.Check_NotIn.Text = "反入库";
            this.Check_NotIn.UseVisualStyleBackColor = true;
            // 
            // WH_In_Old_Form
            // 
            this.ClientSize = new System.Drawing.Size(913, 530);
            this.Controls.Add(this.GrpControlInputData);
            this.Controls.Add(this.GrpControlMSG);
            this.Controls.Add(this.GrpControlPart);
            this.Name = "WH_In_Old_Form";
            this.Text = "WH In";
            this.Load += new System.EventHandler(this.WH_In_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlPart)).EndInit();
            this.GrpControlPart.ResumeLayout(false);
            this.GrpControlPart.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtOverStationQTY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Edt_MPN.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Edt_SN.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlMSG)).EndInit();
            this.GrpControlMSG.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlInputData)).EndInit();
            this.GrpControlInputData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl GrpControlPart;
        private DevExpress.XtraEditors.LabelControl Lab_SN;
        private DevExpress.XtraEditors.TextEdit Edt_SN;
        private DevExpress.XtraEditors.LabelControl Lab_UnReciepted;
        private DevExpress.XtraEditors.LabelControl Lab_Reciepted;
        private DevExpress.XtraEditors.LabelControl Lab_TotalBox;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn grdColumnID;
        private DevExpress.XtraGrid.Columns.GridColumn grdColumnPONO;
        private DevExpress.XtraGrid.Columns.GridColumn grdColumnLineItem;
        private DevExpress.XtraGrid.Columns.GridColumn grdColumnFCTN;
        private DevExpress.XtraGrid.Columns.GridColumn grdColumnFOutSN;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnFMPNNO;
        private DevExpress.XtraEditors.TextEdit Edt_MPN;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        public DevExpress.XtraEditors.SimpleButton Btn_UnLock;
        private System.Windows.Forms.ImageList imageList1;
        public DevExpress.XtraEditors.GroupControl GrpControlMSG;
        public System.Windows.Forms.RichTextBox Edt_MSG;
        public DevExpress.XtraEditors.GroupControl GrpControlInputData;
        private DevExpress.XtraGrid.GridControl Grid_Main;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btnRefresh;
        private DevExpress.XtraEditors.TextEdit txtOverStationQTY;
        private System.Windows.Forms.CheckBox Check_NotIn;
    }
}