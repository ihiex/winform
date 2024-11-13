namespace App.MyMES
{
    partial class PrintSNForm
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Check_RegisterSN = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Com_Qty = new System.Windows.Forms.ComboBox();
            this.Btn_CreateSN = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.Com_Part = new App.MyMES.MultiColumnComboBoxEx();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Com_Templet = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Btn_PrintTest = new System.Windows.Forms.Button();
            this.Btn_Print = new System.Windows.Forms.Button();
            this.Edt_SN = new System.Windows.Forms.TextBox();
            this.RBtn_AlreadyPrinted = new System.Windows.Forms.RadioButton();
            this.Btn_Refresh = new System.Windows.Forms.Button();
            this.Rbtn_NotPrinted = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.Com_SNCategory = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Grid_SN = new System.Windows.Forms.DataGridView();
            this.panel3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_SN)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(5);
            this.panel3.Size = new System.Drawing.Size(966, 86);
            this.panel3.TabIndex = 25;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Check_RegisterSN);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.Com_Qty);
            this.groupBox3.Controls.Add(this.Btn_CreateSN);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.Com_Part);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(5, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(956, 76);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "生成数据(Create Data)";
            // 
            // Check_RegisterSN
            // 
            this.Check_RegisterSN.AutoSize = true;
            this.Check_RegisterSN.Checked = true;
            this.Check_RegisterSN.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Check_RegisterSN.Location = new System.Drawing.Point(1018, 32);
            this.Check_RegisterSN.Name = "Check_RegisterSN";
            this.Check_RegisterSN.Size = new System.Drawing.Size(132, 16);
            this.Check_RegisterSN.TabIndex = 40;
            this.Check_RegisterSN.Text = "是否注册(Register)";
            this.Check_RegisterSN.UseVisualStyleBackColor = true;
            this.Check_RegisterSN.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1017, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 12);
            this.label2.TabIndex = 39;
            this.label2.Text = "注册SN(Register SN)：";
            this.label2.Visible = false;
            // 
            // Com_Qty
            // 
            this.Com_Qty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Com_Qty.FormattingEnabled = true;
            this.Com_Qty.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50"});
            this.Com_Qty.Location = new System.Drawing.Point(117, 47);
            this.Com_Qty.Name = "Com_Qty";
            this.Com_Qty.Size = new System.Drawing.Size(278, 20);
            this.Com_Qty.TabIndex = 38;
            // 
            // Btn_CreateSN
            // 
            this.Btn_CreateSN.Location = new System.Drawing.Point(403, 45);
            this.Btn_CreateSN.Name = "Btn_CreateSN";
            this.Btn_CreateSN.Size = new System.Drawing.Size(378, 23);
            this.Btn_CreateSN.TabIndex = 37;
            this.Btn_CreateSN.Text = "生成SN(Create  SN)";
            this.Btn_CreateSN.UseVisualStyleBackColor = true;
            this.Btn_CreateSN.Click += new System.EventHandler(this.Btn_CreateSN_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 25);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 35;
            this.label5.Text = "料号(Part)：";
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
            this.Com_Part.Location = new System.Drawing.Point(117, 17);
            this.Com_Part.Margin = new System.Windows.Forms.Padding(2);
            this.Com_Part.Name = "Com_Part";
            this.Com_Part.Size = new System.Drawing.Size(664, 22);
            this.Com_Part.TabIndex = 36;
            this.Com_Part.SelectedIndexChanged += new System.EventHandler(this.Com_Part_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 33;
            this.label4.Text = "数量(Quantity)：";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 86);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(966, 99);
            this.panel1.TabIndex = 26;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Com_Templet);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.Btn_PrintTest);
            this.groupBox1.Controls.Add(this.Btn_Print);
            this.groupBox1.Controls.Add(this.Edt_SN);
            this.groupBox1.Controls.Add(this.RBtn_AlreadyPrinted);
            this.groupBox1.Controls.Add(this.Btn_Refresh);
            this.groupBox1.Controls.Add(this.Rbtn_NotPrinted);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.Com_SNCategory);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(956, 89);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "打印条码(Print SN)";
            // 
            // Com_Templet
            // 
            this.Com_Templet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Com_Templet.FormattingEnabled = true;
            this.Com_Templet.Items.AddRange(new object[] {
            "2D",
            "1D"});
            this.Com_Templet.Location = new System.Drawing.Point(653, 24);
            this.Com_Templet.Name = "Com_Templet";
            this.Com_Templet.Size = new System.Drawing.Size(123, 20);
            this.Com_Templet.TabIndex = 24;
            this.Com_Templet.SelectedIndexChanged += new System.EventHandler(this.Com_Templet_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(508, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "模板名称(Templet Name)：";
            // 
            // Btn_PrintTest
            // 
            this.Btn_PrintTest.Location = new System.Drawing.Point(365, 24);
            this.Btn_PrintTest.Name = "Btn_PrintTest";
            this.Btn_PrintTest.Size = new System.Drawing.Size(138, 23);
            this.Btn_PrintTest.TabIndex = 1;
            this.Btn_PrintTest.Text = "打印测试(Print Test)";
            this.Btn_PrintTest.UseVisualStyleBackColor = true;
            this.Btn_PrintTest.Click += new System.EventHandler(this.Btn_PrintTest_Click);
            // 
            // Btn_Print
            // 
            this.Btn_Print.Location = new System.Drawing.Point(461, 63);
            this.Btn_Print.Name = "Btn_Print";
            this.Btn_Print.Size = new System.Drawing.Size(315, 23);
            this.Btn_Print.TabIndex = 22;
            this.Btn_Print.Text = "打印(Print)";
            this.Btn_Print.UseVisualStyleBackColor = true;
            this.Btn_Print.Click += new System.EventHandler(this.Btn_Print_Click);
            // 
            // Edt_SN
            // 
            this.Edt_SN.Location = new System.Drawing.Point(127, 24);
            this.Edt_SN.Name = "Edt_SN";
            this.Edt_SN.Size = new System.Drawing.Size(232, 21);
            this.Edt_SN.TabIndex = 0;
            // 
            // RBtn_AlreadyPrinted
            // 
            this.RBtn_AlreadyPrinted.AutoSize = true;
            this.RBtn_AlreadyPrinted.Location = new System.Drawing.Point(282, 66);
            this.RBtn_AlreadyPrinted.Name = "RBtn_AlreadyPrinted";
            this.RBtn_AlreadyPrinted.Size = new System.Drawing.Size(173, 16);
            this.RBtn_AlreadyPrinted.TabIndex = 21;
            this.RBtn_AlreadyPrinted.TabStop = true;
            this.RBtn_AlreadyPrinted.Text = "已经打印(Already printed)";
            this.RBtn_AlreadyPrinted.UseVisualStyleBackColor = true;
            this.RBtn_AlreadyPrinted.Click += new System.EventHandler(this.RBtn_AlreadyPrinted_CheckedChanged);
            // 
            // Btn_Refresh
            // 
            this.Btn_Refresh.Location = new System.Drawing.Point(18, 22);
            this.Btn_Refresh.Name = "Btn_Refresh";
            this.Btn_Refresh.Size = new System.Drawing.Size(103, 60);
            this.Btn_Refresh.TabIndex = 2;
            this.Btn_Refresh.Text = "刷新(Refresh)";
            this.Btn_Refresh.UseVisualStyleBackColor = true;
            this.Btn_Refresh.Click += new System.EventHandler(this.Btn_Refresh_Click);
            // 
            // Rbtn_NotPrinted
            // 
            this.Rbtn_NotPrinted.AutoSize = true;
            this.Rbtn_NotPrinted.Location = new System.Drawing.Point(127, 66);
            this.Rbtn_NotPrinted.Name = "Rbtn_NotPrinted";
            this.Rbtn_NotPrinted.Size = new System.Drawing.Size(149, 16);
            this.Rbtn_NotPrinted.TabIndex = 20;
            this.Rbtn_NotPrinted.TabStop = true;
            this.Rbtn_NotPrinted.Text = "没有打印(Not Printed)";
            this.Rbtn_NotPrinted.UseVisualStyleBackColor = true;
            this.Rbtn_NotPrinted.Click += new System.EventHandler(this.Rbtn_NotPrinted_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(889, 24);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "SN种类(SNCategory)：";
            this.label3.Visible = false;
            // 
            // Com_SNCategory
            // 
            this.Com_SNCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Com_SNCategory.FormattingEnabled = true;
            this.Com_SNCategory.Location = new System.Drawing.Point(904, 39);
            this.Com_SNCategory.Name = "Com_SNCategory";
            this.Com_SNCategory.Size = new System.Drawing.Size(86, 20);
            this.Com_SNCategory.TabIndex = 18;
            this.Com_SNCategory.Visible = false;
            this.Com_SNCategory.SelectedIndexChanged += new System.EventHandler(this.Com_SNCategory_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 185);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5);
            this.panel2.Size = new System.Drawing.Size(966, 414);
            this.panel2.TabIndex = 27;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Grid_SN);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(5, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(956, 404);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "数据表格(Grid Data)";
            // 
            // Grid_SN
            // 
            this.Grid_SN.AllowUserToAddRows = false;
            this.Grid_SN.AllowUserToDeleteRows = false;
            this.Grid_SN.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.Grid_SN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid_SN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Grid_SN.Location = new System.Drawing.Point(3, 17);
            this.Grid_SN.Name = "Grid_SN";
            this.Grid_SN.RowTemplate.Height = 23;
            this.Grid_SN.Size = new System.Drawing.Size(950, 384);
            this.Grid_SN.TabIndex = 19;
            // 
            // PrintSNForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(966, 599);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "PrintSNForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Print SN";
            this.Load += new System.EventHandler(this.PrintSNForm_Load);
            this.panel3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Grid_SN)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private MultiColumnComboBoxEx Com_Part;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox Com_Templet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Btn_PrintTest;
        private System.Windows.Forms.Button Btn_Print;
        private System.Windows.Forms.TextBox Edt_SN;
        private System.Windows.Forms.RadioButton RBtn_AlreadyPrinted;
        private System.Windows.Forms.Button Btn_Refresh;
        private System.Windows.Forms.RadioButton Rbtn_NotPrinted;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox Com_SNCategory;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView Grid_SN;
        private System.Windows.Forms.Button Btn_CreateSN;
        private System.Windows.Forms.ComboBox Com_Qty;
        private System.Windows.Forms.CheckBox Check_RegisterSN;
        private System.Windows.Forms.Label label2;
    }
}