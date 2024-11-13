namespace App.MyMES
{
    partial class LoginForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Lab_Ver = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.Com_Line = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.Grid_Line = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Com_Station = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.Grid_Station = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Grid_PO = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Com_PO = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.Com_StationType = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.Grid_StationType = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Com_ApplicationType = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.Grid_ApplicationType = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.label7 = new System.Windows.Forms.Label();
            this.Lab_IP = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Edt_UserID = new DevExpress.XtraEditors.TextEdit();
            this.Edt_Password = new DevExpress.XtraEditors.TextEdit();
            this.Btn_Seting = new DevExpress.XtraEditors.SimpleButton();
            this.Btn_Update = new DevExpress.XtraEditors.SimpleButton();
            this.Btn_Cancel = new DevExpress.XtraEditors.SimpleButton();
            this.Btn_OK = new DevExpress.XtraEditors.SimpleButton();
            this.defaultLook_Main = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.label9 = new System.Windows.Forms.Label();
            this.comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_Line.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Line)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_Station.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Station)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_PO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_PO.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_StationType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_StationType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_ApplicationType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_ApplicationType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Edt_UserID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Edt_Password.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(177, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "User ID：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(169, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(700, 88);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 16);
            this.label3.TabIndex = 16;
            this.label3.Text = "Station Type：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(177, 189);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 16);
            this.label4.TabIndex = 18;
            this.label4.Text = "Station：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(121, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(270, 24);
            this.label5.TabIndex = 19;
            this.label5.Text = "COSMO A01 MES System";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(201, 154);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 16);
            this.label6.TabIndex = 21;
            this.label6.Text = "Line：";
            // 
            // Lab_Ver
            // 
            this.Lab_Ver.AutoSize = true;
            this.Lab_Ver.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lab_Ver.ForeColor = System.Drawing.Color.Red;
            this.Lab_Ver.Location = new System.Drawing.Point(26, 250);
            this.Lab_Ver.Name = "Lab_Ver";
            this.Lab_Ver.Size = new System.Drawing.Size(188, 16);
            this.Lab_Ver.TabIndex = 24;
            this.Lab_Ver.Text = "Ver:2020-03-25 10:00";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(688, 51);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 12);
            this.label8.TabIndex = 25;
            this.label8.Text = "ProductionOrder：";
            // 
            // Com_Line
            // 
            this.Com_Line.EditValue = "";
            this.Com_Line.Enabled = false;
            this.Com_Line.Location = new System.Drawing.Point(251, 151);
            this.Com_Line.Name = "Com_Line";
            this.Com_Line.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.Com_Line.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Com_Line.Properties.Appearance.Options.UseBackColor = true;
            this.Com_Line.Properties.Appearance.Options.UseFont = true;
            this.Com_Line.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Com_Line.Properties.PopupView = this.Grid_Line;
            this.Com_Line.Size = new System.Drawing.Size(331, 26);
            this.Com_Line.TabIndex = 68;
            this.Com_Line.EditValueChanged += new System.EventHandler(this.Com_Line_EditValueChanged);
            // 
            // Grid_Line
            // 
            this.Grid_Line.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.Grid_Line.Name = "Grid_Line";
            this.Grid_Line.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.Grid_Line.OptionsView.ShowGroupPanel = false;
            // 
            // Com_Station
            // 
            this.Com_Station.EditValue = "";
            this.Com_Station.Enabled = false;
            this.Com_Station.Location = new System.Drawing.Point(251, 186);
            this.Com_Station.Name = "Com_Station";
            this.Com_Station.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.Com_Station.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Com_Station.Properties.Appearance.Options.UseBackColor = true;
            this.Com_Station.Properties.Appearance.Options.UseFont = true;
            this.Com_Station.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Com_Station.Properties.PopupView = this.Grid_Station;
            this.Com_Station.Size = new System.Drawing.Size(331, 26);
            this.Com_Station.TabIndex = 69;
            this.Com_Station.EditValueChanged += new System.EventHandler(this.Com_Station_EditValueChanged);
            // 
            // Grid_Station
            // 
            this.Grid_Station.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.Grid_Station.Name = "Grid_Station";
            this.Grid_Station.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.Grid_Station.OptionsView.ShowGroupPanel = false;
            // 
            // Grid_PO
            // 
            this.Grid_PO.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.Grid_PO.Name = "Grid_PO";
            this.Grid_PO.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.Grid_PO.OptionsView.ShowGroupPanel = false;
            // 
            // Com_PO
            // 
            this.Com_PO.EditValue = "";
            this.Com_PO.Location = new System.Drawing.Point(813, 43);
            this.Com_PO.Name = "Com_PO";
            this.Com_PO.Properties.Appearance.BackColor = System.Drawing.Color.Yellow;
            this.Com_PO.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Com_PO.Properties.Appearance.Options.UseBackColor = true;
            this.Com_PO.Properties.Appearance.Options.UseFont = true;
            this.Com_PO.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Com_PO.Properties.PopupView = this.Grid_PO;
            this.Com_PO.Size = new System.Drawing.Size(190, 24);
            this.Com_PO.TabIndex = 70;
            // 
            // Com_StationType
            // 
            this.Com_StationType.EditValue = "";
            this.Com_StationType.Location = new System.Drawing.Point(813, 85);
            this.Com_StationType.Name = "Com_StationType";
            this.Com_StationType.Properties.Appearance.BackColor = System.Drawing.Color.Yellow;
            this.Com_StationType.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Com_StationType.Properties.Appearance.Options.UseBackColor = true;
            this.Com_StationType.Properties.Appearance.Options.UseFont = true;
            this.Com_StationType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Com_StationType.Properties.PopupView = this.Grid_StationType;
            this.Com_StationType.Size = new System.Drawing.Size(190, 24);
            this.Com_StationType.TabIndex = 71;
            this.Com_StationType.EditValueChanged += new System.EventHandler(this.Com_StationType_EditValueChanged);
            // 
            // Grid_StationType
            // 
            this.Grid_StationType.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.Grid_StationType.Name = "Grid_StationType";
            this.Grid_StationType.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.Grid_StationType.OptionsView.ShowGroupPanel = false;
            // 
            // Com_ApplicationType
            // 
            this.Com_ApplicationType.EditValue = "";
            this.Com_ApplicationType.Location = new System.Drawing.Point(828, 146);
            this.Com_ApplicationType.Name = "Com_ApplicationType";
            this.Com_ApplicationType.Properties.Appearance.BackColor = System.Drawing.Color.Yellow;
            this.Com_ApplicationType.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Com_ApplicationType.Properties.Appearance.Options.UseBackColor = true;
            this.Com_ApplicationType.Properties.Appearance.Options.UseFont = true;
            this.Com_ApplicationType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Com_ApplicationType.Properties.PopupView = this.Grid_ApplicationType;
            this.Com_ApplicationType.Size = new System.Drawing.Size(190, 24);
            this.Com_ApplicationType.TabIndex = 72;
            // 
            // Grid_ApplicationType
            // 
            this.Grid_ApplicationType.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.Grid_ApplicationType.Name = "Grid_ApplicationType";
            this.Grid_ApplicationType.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.Grid_ApplicationType.OptionsView.ShowGroupPanel = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(688, 149);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(144, 16);
            this.label7.TabIndex = 73;
            this.label7.Text = "ApplicationType：";
            // 
            // Lab_IP
            // 
            this.Lab_IP.AutoSize = true;
            this.Lab_IP.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lab_IP.ForeColor = System.Drawing.Color.Red;
            this.Lab_IP.Location = new System.Drawing.Point(26, 281);
            this.Lab_IP.Name = "Lab_IP";
            this.Lab_IP.Size = new System.Drawing.Size(71, 16);
            this.Lab_IP.TabIndex = 74;
            this.Lab_IP.Text = "Server:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::App.MyMES.Properties.Resources._270;
            this.pictureBox1.Location = new System.Drawing.Point(29, 84);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Edt_UserID
            // 
            this.Edt_UserID.Location = new System.Drawing.Point(251, 42);
            this.Edt_UserID.Name = "Edt_UserID";
            this.Edt_UserID.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Edt_UserID.Properties.Appearance.Options.UseFont = true;
            this.Edt_UserID.Size = new System.Drawing.Size(331, 26);
            this.Edt_UserID.TabIndex = 75;
            this.Edt_UserID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Edt_UserID_KeyDown);
            // 
            // Edt_Password
            // 
            this.Edt_Password.Location = new System.Drawing.Point(251, 79);
            this.Edt_Password.Name = "Edt_Password";
            this.Edt_Password.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Edt_Password.Properties.Appearance.Options.UseFont = true;
            this.Edt_Password.Properties.PasswordChar = '*';
            this.Edt_Password.Size = new System.Drawing.Size(331, 26);
            this.Edt_Password.TabIndex = 76;
            this.Edt_Password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Edt_Password_KeyDown);
            // 
            // Btn_Seting
            // 
            this.Btn_Seting.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Seting.Appearance.Options.UseFont = true;
            this.Btn_Seting.Location = new System.Drawing.Point(251, 115);
            this.Btn_Seting.Name = "Btn_Seting";
            this.Btn_Seting.Size = new System.Drawing.Size(152, 27);
            this.Btn_Seting.TabIndex = 77;
            this.Btn_Seting.Text = "Seting";
            this.Btn_Seting.Click += new System.EventHandler(this.Btn_Seting_Click);
            // 
            // Btn_Update
            // 
            this.Btn_Update.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Update.Appearance.Options.UseFont = true;
            this.Btn_Update.Location = new System.Drawing.Point(434, 115);
            this.Btn_Update.Name = "Btn_Update";
            this.Btn_Update.Size = new System.Drawing.Size(148, 27);
            this.Btn_Update.TabIndex = 78;
            this.Btn_Update.Text = "Update";
            this.Btn_Update.Click += new System.EventHandler(this.Btn_Update_Click);
            // 
            // Btn_Cancel
            // 
            this.Btn_Cancel.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Cancel.Appearance.Options.UseFont = true;
            this.Btn_Cancel.Location = new System.Drawing.Point(434, 258);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(148, 27);
            this.Btn_Cancel.TabIndex = 80;
            this.Btn_Cancel.Text = "Cancel";
            this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);
            // 
            // Btn_OK
            // 
            this.Btn_OK.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_OK.Appearance.Options.UseFont = true;
            this.Btn_OK.Location = new System.Drawing.Point(251, 258);
            this.Btn_OK.Name = "Btn_OK";
            this.Btn_OK.Size = new System.Drawing.Size(152, 27);
            this.Btn_OK.TabIndex = 79;
            this.Btn_OK.Text = "OK";
            this.Btn_OK.Click += new System.EventHandler(this.Btn_OK_Click);
            // 
            // defaultLook_Main
            // 
            this.defaultLook_Main.LookAndFeel.SkinName = "Office 2010 Blue";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(169, 222);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 16);
            this.label9.TabIndex = 81;
            this.label9.Text = "Language：";
            // 
            // comboBoxEdit1
            // 
            this.comboBoxEdit1.Location = new System.Drawing.Point(251, 219);
            this.comboBoxEdit1.Name = "comboBoxEdit1";
            this.comboBoxEdit1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.comboBoxEdit1.Properties.Appearance.Options.UseFont = true;
            this.comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit1.Properties.Items.AddRange(new object[] {
            "简体中文",
            "English"});
            this.comboBoxEdit1.Size = new System.Drawing.Size(331, 26);
            this.comboBoxEdit1.TabIndex = 82;
            // 
            // LoginForm
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 304);
            this.Controls.Add(this.comboBoxEdit1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.Btn_Cancel);
            this.Controls.Add(this.Btn_OK);
            this.Controls.Add(this.Btn_Update);
            this.Controls.Add(this.Btn_Seting);
            this.Controls.Add(this.Edt_Password);
            this.Controls.Add(this.Edt_UserID);
            this.Controls.Add(this.Lab_IP);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.Com_ApplicationType);
            this.Controls.Add(this.Com_StationType);
            this.Controls.Add(this.Com_PO);
            this.Controls.Add(this.Com_Station);
            this.Controls.Add(this.Com_Line);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.Lab_Ver);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "COSMO MES Login";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Com_Line.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Line)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_Station.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Station)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_PO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_PO.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_StationType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_StationType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_ApplicationType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_ApplicationType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Edt_UserID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Edt_Password.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label Lab_Ver;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.SearchLookUpEdit Com_Line;
        private DevExpress.XtraGrid.Views.Grid.GridView Grid_Line;
        private DevExpress.XtraEditors.SearchLookUpEdit Com_Station;
        private DevExpress.XtraGrid.Views.Grid.GridView Grid_Station;
        private DevExpress.XtraGrid.Views.Grid.GridView Grid_PO;
        private DevExpress.XtraEditors.SearchLookUpEdit Com_PO;
        private DevExpress.XtraEditors.SearchLookUpEdit Com_StationType;
        private DevExpress.XtraGrid.Views.Grid.GridView Grid_StationType;
        private DevExpress.XtraEditors.SearchLookUpEdit Com_ApplicationType;
        private DevExpress.XtraGrid.Views.Grid.GridView Grid_ApplicationType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label Lab_IP;
        private DevExpress.XtraEditors.TextEdit Edt_UserID;
        private DevExpress.XtraEditors.TextEdit Edt_Password;
        private DevExpress.XtraEditors.SimpleButton Btn_Seting;
        private DevExpress.XtraEditors.SimpleButton Btn_Update;
        private DevExpress.XtraEditors.SimpleButton Btn_Cancel;
        private DevExpress.XtraEditors.SimpleButton Btn_OK;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLook_Main;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit1;
    }
}