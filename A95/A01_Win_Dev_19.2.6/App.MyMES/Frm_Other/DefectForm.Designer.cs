namespace App.MyMES
{
    partial class DefectForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.Btn_OK = new System.Windows.Forms.Button();
            this.Grid_Main = new DevExpress.XtraGrid.GridControl();
            this.GridView_Defact = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Col_Check = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Rep_CheckEdit_Select = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.Col_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Col_不良代码 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Col_不良名称 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Col_位置 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView_Defact)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rep_CheckEdit_Select)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Btn_OK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 562);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(877, 30);
            this.panel1.TabIndex = 22;
            // 
            // Btn_OK
            // 
            this.Btn_OK.BackColor = System.Drawing.Color.Yellow;
            this.Btn_OK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Btn_OK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Btn_OK.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_OK.ForeColor = System.Drawing.Color.Red;
            this.Btn_OK.Location = new System.Drawing.Point(0, 0);
            this.Btn_OK.Name = "Btn_OK";
            this.Btn_OK.Size = new System.Drawing.Size(877, 30);
            this.Btn_OK.TabIndex = 0;
            this.Btn_OK.Text = "确认不良";
            this.Btn_OK.UseVisualStyleBackColor = false;
            this.Btn_OK.Click += new System.EventHandler(this.Btn_OK_Click);
            // 
            // Grid_Main
            // 
            this.Grid_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Grid_Main.Location = new System.Drawing.Point(0, 0);
            this.Grid_Main.MainView = this.GridView_Defact;
            this.Grid_Main.Name = "Grid_Main";
            this.Grid_Main.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.Rep_CheckEdit_Select});
            this.Grid_Main.Size = new System.Drawing.Size(877, 562);
            this.Grid_Main.TabIndex = 23;
            this.Grid_Main.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridView_Defact});
            this.Grid_Main.Click += new System.EventHandler(this.Grid_Main_Click);
            // 
            // GridView_Defact
            // 
            this.GridView_Defact.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.GridView_Defact.Appearance.FocusedRow.Options.UseBackColor = true;
            this.GridView_Defact.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.GridView_Defact.Appearance.SelectedRow.Options.UseBackColor = true;
            this.GridView_Defact.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Col_Check,
            this.Col_ID,
            this.Col_不良代码,
            this.Col_不良名称,
            this.Col_位置});
            this.GridView_Defact.GridControl = this.Grid_Main;
            this.GridView_Defact.Name = "GridView_Defact";
            this.GridView_Defact.OptionsFind.AlwaysVisible = true;
            this.GridView_Defact.OptionsFind.SearchInPreview = true;
            this.GridView_Defact.OptionsView.ShowGroupPanel = false;
            this.GridView_Defact.RowHeight = 30;
            this.GridView_Defact.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.GridView_Defact_RowClick);
            this.GridView_Defact.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.GridView_Defact_RowCellClick);
            // 
            // Col_Check
            // 
            this.Col_Check.Caption = "Check";
            this.Col_Check.ColumnEdit = this.Rep_CheckEdit_Select;
            this.Col_Check.FieldName = "Check";
            this.Col_Check.Name = "Col_Check";
            this.Col_Check.Visible = true;
            this.Col_Check.VisibleIndex = 0;
            this.Col_Check.Width = 58;
            // 
            // Rep_CheckEdit_Select
            // 
            this.Rep_CheckEdit_Select.AutoHeight = false;
            this.Rep_CheckEdit_Select.Name = "Rep_CheckEdit_Select";
            // 
            // Col_ID
            // 
            this.Col_ID.Caption = "ID";
            this.Col_ID.FieldName = "ID";
            this.Col_ID.Name = "Col_ID";
            this.Col_ID.Width = 50;
            // 
            // Col_不良代码
            // 
            this.Col_不良代码.Caption = "不良代码";
            this.Col_不良代码.FieldName = "不良代码";
            this.Col_不良代码.Name = "Col_不良代码";
            this.Col_不良代码.Visible = true;
            this.Col_不良代码.VisibleIndex = 1;
            this.Col_不良代码.Width = 70;
            // 
            // Col_不良名称
            // 
            this.Col_不良名称.Caption = "不良名称";
            this.Col_不良名称.FieldName = "不良名称";
            this.Col_不良名称.Name = "Col_不良名称";
            this.Col_不良名称.Visible = true;
            this.Col_不良名称.VisibleIndex = 2;
            this.Col_不良名称.Width = 300;
            // 
            // Col_位置
            // 
            this.Col_位置.Caption = "位置";
            this.Col_位置.FieldName = "位置";
            this.Col_位置.Name = "Col_位置";
            this.Col_位置.Visible = true;
            this.Col_位置.VisibleIndex = 3;
            this.Col_位置.Width = 200;
            // 
            // DefectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 592);
            this.Controls.Add(this.Grid_Main);
            this.Controls.Add(this.panel1);
            this.Name = "DefectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Defect";
            this.Load += new System.EventHandler(this.DefectForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView_Defact)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rep_CheckEdit_Select)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button Btn_OK;
        private DevExpress.XtraGrid.GridControl Grid_Main;
        private DevExpress.XtraGrid.Views.Grid.GridView GridView_Defact;
        private DevExpress.XtraGrid.Columns.GridColumn Col_Check;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit Rep_CheckEdit_Select;
        private DevExpress.XtraGrid.Columns.GridColumn Col_ID;
        private DevExpress.XtraGrid.Columns.GridColumn Col_不良代码;
        private DevExpress.XtraGrid.Columns.GridColumn Col_不良名称;
        private DevExpress.XtraGrid.Columns.GridColumn Col_位置;
    }
}