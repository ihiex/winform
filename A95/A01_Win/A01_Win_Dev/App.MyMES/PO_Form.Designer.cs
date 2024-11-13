namespace App.MyMES
{
    partial class PO_Form
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
            this.Com_PartFamilyType = new App.MyMES.MultiColumnComboBoxEx();
            this.label1 = new System.Windows.Forms.Label();
            this.Com_PartFamily = new App.MyMES.MultiColumnComboBoxEx();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Com_Part = new App.MyMES.MultiColumnComboBoxEx();
            this.Btn_Refresh = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.Edt_PONumber = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Edt_OrderQuantity = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Com_StationType = new App.MyMES.MultiColumnComboBoxEx();
            this.Com_Station = new App.MyMES.MultiColumnComboBoxEx();
            this.label7 = new System.Windows.Forms.Label();
            this.Com_Line = new App.MyMES.MultiColumnComboBoxEx();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.Btn_Save = new System.Windows.Forms.Button();
            this.Btn_Delete = new System.Windows.Forms.Button();
            this.Btn_Insert = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Grid_PO = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_PO)).BeginInit();
            this.SuspendLayout();
            // 
            // Com_PartFamilyType
            // 
            this.Com_PartFamilyType.ComboBoxHeight = 16;
            this.Com_PartFamilyType.DisplayColumnNames = "ID,Name";
            this.Com_PartFamilyType.DisplayMultiColumnsInBox = true;
            this.Com_PartFamilyType.DropDownHeight = 258;
            this.Com_PartFamilyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Com_PartFamilyType.DropDownWidth = 117;
            this.Com_PartFamilyType.FormattingEnabled = true;
            this.Com_PartFamilyType.ItemDropDownHeight = 32;
            this.Com_PartFamilyType.Location = new System.Drawing.Point(115, 19);
            this.Com_PartFamilyType.Margin = new System.Windows.Forms.Padding(2);
            this.Com_PartFamilyType.Name = "Com_PartFamilyType";
            this.Com_PartFamilyType.Size = new System.Drawing.Size(192, 22);
            this.Com_PartFamilyType.TabIndex = 0;
            this.Com_PartFamilyType.SelectedIndexChanged += new System.EventHandler(this.Com_PartFamilyType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 20;
            this.label1.Text = "PartFamilyType：";
            // 
            // Com_PartFamily
            // 
            this.Com_PartFamily.ComboBoxHeight = 16;
            this.Com_PartFamily.DisplayColumnNames = "ID,Name";
            this.Com_PartFamily.DisplayMultiColumnsInBox = true;
            this.Com_PartFamily.DropDownHeight = 258;
            this.Com_PartFamily.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Com_PartFamily.DropDownWidth = 117;
            this.Com_PartFamily.FormattingEnabled = true;
            this.Com_PartFamily.ItemDropDownHeight = 32;
            this.Com_PartFamily.Location = new System.Drawing.Point(115, 50);
            this.Com_PartFamily.Margin = new System.Windows.Forms.Padding(2);
            this.Com_PartFamily.Name = "Com_PartFamily";
            this.Com_PartFamily.Size = new System.Drawing.Size(192, 22);
            this.Com_PartFamily.TabIndex = 1;
            this.Com_PartFamily.SelectedIndexChanged += new System.EventHandler(this.Com_PartFamily_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 54);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 21;
            this.label2.Text = "PartFamily：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(71, 85);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "Part：";
            // 
            // Com_Part
            // 
            this.Com_Part.ComboBoxHeight = 16;
            this.Com_Part.DisplayColumnNames = "ID,PartNumber,Description";
            this.Com_Part.DisplayMultiColumnsInBox = true;
            this.Com_Part.DropDownHeight = 258;
            this.Com_Part.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Com_Part.DropDownWidth = 117;
            this.Com_Part.FormattingEnabled = true;
            this.Com_Part.ItemDropDownHeight = 32;
            this.Com_Part.Location = new System.Drawing.Point(115, 81);
            this.Com_Part.Margin = new System.Windows.Forms.Padding(2);
            this.Com_Part.Name = "Com_Part";
            this.Com_Part.Size = new System.Drawing.Size(489, 22);
            this.Com_Part.TabIndex = 2;
            this.Com_Part.SelectedIndexChanged += new System.EventHandler(this.Com_Part_SelectedIndexChanged);
            // 
            // Btn_Refresh
            // 
            this.Btn_Refresh.Location = new System.Drawing.Point(112, 110);
            this.Btn_Refresh.Margin = new System.Windows.Forms.Padding(2);
            this.Btn_Refresh.Name = "Btn_Refresh";
            this.Btn_Refresh.Size = new System.Drawing.Size(60, 25);
            this.Btn_Refresh.TabIndex = 25;
            this.Btn_Refresh.Text = "Refresh";
            this.Btn_Refresh.UseVisualStyleBackColor = true;
            this.Btn_Refresh.Click += new System.EventHandler(this.Btn_Refresh_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(351, 22);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 26;
            this.label4.Text = "PONumber：";
            // 
            // Edt_PONumber
            // 
            this.Edt_PONumber.Enabled = false;
            this.Edt_PONumber.Location = new System.Drawing.Point(419, 18);
            this.Edt_PONumber.Name = "Edt_PONumber";
            this.Edt_PONumber.Size = new System.Drawing.Size(185, 21);
            this.Edt_PONumber.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(326, 54);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 12);
            this.label5.TabIndex = 28;
            this.label5.Text = "OrderQuantity：";
            // 
            // Edt_OrderQuantity
            // 
            this.Edt_OrderQuantity.Enabled = false;
            this.Edt_OrderQuantity.Location = new System.Drawing.Point(420, 49);
            this.Edt_OrderQuantity.Name = "Edt_OrderQuantity";
            this.Edt_OrderQuantity.Size = new System.Drawing.Size(185, 21);
            this.Edt_OrderQuantity.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(926, 153);
            this.panel1.TabIndex = 32;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Com_StationType);
            this.groupBox1.Controls.Add(this.Com_Station);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.Com_Line);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.Btn_Save);
            this.groupBox1.Controls.Add(this.Btn_Delete);
            this.groupBox1.Controls.Add(this.Btn_Insert);
            this.groupBox1.Controls.Add(this.Edt_PONumber);
            this.groupBox1.Controls.Add(this.Btn_Refresh);
            this.groupBox1.Controls.Add(this.Com_Part);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.Edt_OrderQuantity);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.Com_PartFamily);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.Com_PartFamilyType);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(916, 143);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data edition";
            // 
            // Com_StationType
            // 
            this.Com_StationType.BackColor = System.Drawing.Color.Yellow;
            this.Com_StationType.ComboBoxHeight = 16;
            this.Com_StationType.DisplayColumnNames = "ID,Description";
            this.Com_StationType.DisplayMultiColumnsInBox = true;
            this.Com_StationType.DropDownHeight = 258;
            this.Com_StationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Com_StationType.DropDownWidth = 117;
            this.Com_StationType.Enabled = false;
            this.Com_StationType.FormattingEnabled = true;
            this.Com_StationType.ItemDropDownHeight = 32;
            this.Com_StationType.Location = new System.Drawing.Point(711, 19);
            this.Com_StationType.Margin = new System.Windows.Forms.Padding(2);
            this.Com_StationType.Name = "Com_StationType";
            this.Com_StationType.Size = new System.Drawing.Size(190, 22);
            this.Com_StationType.TabIndex = 35;
            // 
            // Com_Station
            // 
            this.Com_Station.BackColor = System.Drawing.Color.Yellow;
            this.Com_Station.ComboBoxHeight = 16;
            this.Com_Station.DisplayColumnNames = "ID,Description";
            this.Com_Station.DisplayMultiColumnsInBox = true;
            this.Com_Station.DropDownHeight = 258;
            this.Com_Station.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Com_Station.DropDownWidth = 117;
            this.Com_Station.Enabled = false;
            this.Com_Station.FormattingEnabled = true;
            this.Com_Station.ItemDropDownHeight = 32;
            this.Com_Station.Location = new System.Drawing.Point(711, 81);
            this.Com_Station.Margin = new System.Windows.Forms.Padding(2);
            this.Com_Station.Name = "Com_Station";
            this.Com_Station.Size = new System.Drawing.Size(190, 22);
            this.Com_Station.TabIndex = 37;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(650, 84);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 38;
            this.label7.Text = "Station：";
            // 
            // Com_Line
            // 
            this.Com_Line.BackColor = System.Drawing.Color.Yellow;
            this.Com_Line.ComboBoxHeight = 16;
            this.Com_Line.DisplayColumnNames = "ID,Description";
            this.Com_Line.DisplayMultiColumnsInBox = true;
            this.Com_Line.DropDownHeight = 258;
            this.Com_Line.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Com_Line.DropDownWidth = 117;
            this.Com_Line.Enabled = false;
            this.Com_Line.FormattingEnabled = true;
            this.Com_Line.ItemDropDownHeight = 32;
            this.Com_Line.Location = new System.Drawing.Point(711, 50);
            this.Com_Line.Margin = new System.Windows.Forms.Padding(2);
            this.Com_Line.Name = "Com_Line";
            this.Com_Line.Size = new System.Drawing.Size(190, 22);
            this.Com_Line.TabIndex = 39;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(666, 53);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 40;
            this.label8.Text = "Line：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(620, 22);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 12);
            this.label9.TabIndex = 36;
            this.label9.Text = "Station Type：";
            // 
            // Btn_Save
            // 
            this.Btn_Save.Enabled = false;
            this.Btn_Save.Location = new System.Drawing.Point(328, 110);
            this.Btn_Save.Margin = new System.Windows.Forms.Padding(2);
            this.Btn_Save.Name = "Btn_Save";
            this.Btn_Save.Size = new System.Drawing.Size(60, 25);
            this.Btn_Save.TabIndex = 34;
            this.Btn_Save.Text = "Save";
            this.Btn_Save.UseVisualStyleBackColor = true;
            this.Btn_Save.Click += new System.EventHandler(this.Btn_Save_Click);
            // 
            // Btn_Delete
            // 
            this.Btn_Delete.Enabled = false;
            this.Btn_Delete.Location = new System.Drawing.Point(256, 110);
            this.Btn_Delete.Margin = new System.Windows.Forms.Padding(2);
            this.Btn_Delete.Name = "Btn_Delete";
            this.Btn_Delete.Size = new System.Drawing.Size(60, 25);
            this.Btn_Delete.TabIndex = 33;
            this.Btn_Delete.Text = "Delete";
            this.Btn_Delete.UseVisualStyleBackColor = true;
            this.Btn_Delete.Click += new System.EventHandler(this.Btn_Delete_Click);
            // 
            // Btn_Insert
            // 
            this.Btn_Insert.Location = new System.Drawing.Point(184, 110);
            this.Btn_Insert.Margin = new System.Windows.Forms.Padding(2);
            this.Btn_Insert.Name = "Btn_Insert";
            this.Btn_Insert.Size = new System.Drawing.Size(60, 25);
            this.Btn_Insert.TabIndex = 32;
            this.Btn_Insert.Text = "Insert";
            this.Btn_Insert.UseVisualStyleBackColor = true;
            this.Btn_Insert.Click += new System.EventHandler(this.Btn_Insert_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 153);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5);
            this.panel2.Size = new System.Drawing.Size(926, 472);
            this.panel2.TabIndex = 33;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Grid_PO);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(5, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(916, 462);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Data Grid";
            // 
            // Grid_PO
            // 
            this.Grid_PO.AllowUserToAddRows = false;
            this.Grid_PO.AllowUserToDeleteRows = false;
            this.Grid_PO.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.Grid_PO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid_PO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Grid_PO.Location = new System.Drawing.Point(3, 17);
            this.Grid_PO.Name = "Grid_PO";
            this.Grid_PO.RowTemplate.Height = 23;
            this.Grid_PO.Size = new System.Drawing.Size(910, 442);
            this.Grid_PO.TabIndex = 0;
            this.Grid_PO.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_PO_CellClick);
            // 
            // PO_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(926, 625);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "PO_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProductionOrder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PO_Form_FormClosing);
            this.Load += new System.EventHandler(this.PO_Form_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Grid_PO)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MultiColumnComboBoxEx Com_PartFamilyType;
        private System.Windows.Forms.Label label1;
        private MultiColumnComboBoxEx Com_PartFamily;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private MultiColumnComboBoxEx Com_Part;
        private System.Windows.Forms.Button Btn_Refresh;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Edt_PONumber;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Edt_OrderQuantity;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView Grid_PO;
        private System.Windows.Forms.Button Btn_Delete;
        private System.Windows.Forms.Button Btn_Insert;
        private System.Windows.Forms.Button Btn_Save;
        private MultiColumnComboBoxEx Com_StationType;
        private MultiColumnComboBoxEx Com_Station;
        private System.Windows.Forms.Label label7;
        private MultiColumnComboBoxEx Com_Line;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
    }
}