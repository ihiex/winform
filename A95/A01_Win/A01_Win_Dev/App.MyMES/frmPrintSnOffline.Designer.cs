namespace App.MyMES
{
    partial class frmPrintSnOffline
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtSN = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.Com_Station = new App.MyMES.MultiColumnComboBoxEx();
            this.panel8 = new System.Windows.Forms.Panel();
            this.Com_Line = new App.MyMES.MultiColumnComboBoxEx();
            this.panel7 = new System.Windows.Forms.Panel();
            this.Com_StationType = new App.MyMES.MultiColumnComboBoxEx();
            this.panel11 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.multiColumnComboBoxEx3 = new App.MyMES.MultiColumnComboBoxEx();
            this.panel5 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Btn_CreateSN = new System.Windows.Forms.Button();
            this.Com_Templet = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Com_PartFamilyType = new App.MyMES.MultiColumnComboBoxEx();
            this.Com_PartFamily = new App.MyMES.MultiColumnComboBoxEx();
            this.Com_Part = new App.MyMES.MultiColumnComboBoxEx();
            this.txtNum = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridSN = new System.Windows.Forms.DataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.Edt_MSG = new System.Windows.Forms.RichTextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSN)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1118, 236);
            this.panel1.TabIndex = 35;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtSN);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.btnTest);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(627, 125);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(491, 111);
            this.groupBox4.TabIndex = 41;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "补打条码(Reprint SN)";
            // 
            // txtSN
            // 
            this.txtSN.Font = new System.Drawing.Font("宋体", 11F);
            this.txtSN.Location = new System.Drawing.Point(100, 52);
            this.txtSN.Name = "txtSN";
            this.txtSN.Size = new System.Drawing.Size(248, 24);
            this.txtSN.TabIndex = 61;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F);
            this.label9.Location = new System.Drawing.Point(29, 58);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 60;
            this.label9.Text = "条码(SN)：";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(354, 51);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(129, 25);
            this.btnTest.TabIndex = 59;
            this.btnTest.Text = "补打(Reprint)";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.panel6);
            this.groupBox3.Controls.Add(this.panel11);
            this.groupBox3.Controls.Add(this.multiColumnComboBoxEx3);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(627, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(491, 125);
            this.groupBox3.TabIndex = 40;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Station Data(工站数据)";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.panel9);
            this.panel6.Controls.Add(this.panel8);
            this.panel6.Controls.Add(this.panel7);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(187, 17);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(301, 105);
            this.panel6.TabIndex = 39;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.Com_Station);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(0, 60);
            this.panel9.Name = "panel9";
            this.panel9.Padding = new System.Windows.Forms.Padding(5);
            this.panel9.Size = new System.Drawing.Size(301, 30);
            this.panel9.TabIndex = 39;
            // 
            // Com_Station
            // 
            this.Com_Station.BackColor = System.Drawing.Color.Yellow;
            this.Com_Station.ComboBoxHeight = 16;
            this.Com_Station.DisplayColumnNames = "ID,Description";
            this.Com_Station.DisplayMultiColumnsInBox = true;
            this.Com_Station.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Com_Station.DropDownHeight = 258;
            this.Com_Station.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Com_Station.DropDownWidth = 117;
            this.Com_Station.Enabled = false;
            this.Com_Station.Font = new System.Drawing.Font("宋体", 10F);
            this.Com_Station.FormattingEnabled = true;
            this.Com_Station.ItemDropDownHeight = 32;
            this.Com_Station.Location = new System.Drawing.Point(5, 5);
            this.Com_Station.Margin = new System.Windows.Forms.Padding(2);
            this.Com_Station.Name = "Com_Station";
            this.Com_Station.Size = new System.Drawing.Size(291, 22);
            this.Com_Station.TabIndex = 27;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.Com_Line);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 30);
            this.panel8.Name = "panel8";
            this.panel8.Padding = new System.Windows.Forms.Padding(5);
            this.panel8.Size = new System.Drawing.Size(301, 30);
            this.panel8.TabIndex = 38;
            // 
            // Com_Line
            // 
            this.Com_Line.BackColor = System.Drawing.Color.Yellow;
            this.Com_Line.ComboBoxHeight = 16;
            this.Com_Line.DisplayColumnNames = "ID,Description";
            this.Com_Line.DisplayMultiColumnsInBox = true;
            this.Com_Line.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Com_Line.DropDownHeight = 258;
            this.Com_Line.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Com_Line.DropDownWidth = 117;
            this.Com_Line.Enabled = false;
            this.Com_Line.Font = new System.Drawing.Font("宋体", 10F);
            this.Com_Line.FormattingEnabled = true;
            this.Com_Line.ItemDropDownHeight = 32;
            this.Com_Line.Location = new System.Drawing.Point(5, 5);
            this.Com_Line.Margin = new System.Windows.Forms.Padding(2);
            this.Com_Line.Name = "Com_Line";
            this.Com_Line.Size = new System.Drawing.Size(291, 22);
            this.Com_Line.TabIndex = 29;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.Com_StationType);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Padding = new System.Windows.Forms.Padding(5);
            this.panel7.Size = new System.Drawing.Size(301, 30);
            this.panel7.TabIndex = 37;
            // 
            // Com_StationType
            // 
            this.Com_StationType.BackColor = System.Drawing.Color.Yellow;
            this.Com_StationType.ComboBoxHeight = 16;
            this.Com_StationType.DisplayColumnNames = "ID,Description";
            this.Com_StationType.DisplayMultiColumnsInBox = true;
            this.Com_StationType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Com_StationType.DropDownHeight = 258;
            this.Com_StationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Com_StationType.DropDownWidth = 117;
            this.Com_StationType.Enabled = false;
            this.Com_StationType.Font = new System.Drawing.Font("宋体", 10F);
            this.Com_StationType.FormattingEnabled = true;
            this.Com_StationType.ItemDropDownHeight = 32;
            this.Com_StationType.Location = new System.Drawing.Point(5, 5);
            this.Com_StationType.Margin = new System.Windows.Forms.Padding(2);
            this.Com_StationType.Name = "Com_StationType";
            this.Com_StationType.Size = new System.Drawing.Size(291, 22);
            this.Com_StationType.TabIndex = 25;
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.label7);
            this.panel11.Controls.Add(this.label8);
            this.panel11.Controls.Add(this.label6);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel11.Location = new System.Drawing.Point(3, 17);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(184, 105);
            this.panel11.TabIndex = 38;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F);
            this.label7.Location = new System.Drawing.Point(80, 68);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 12);
            this.label7.TabIndex = 28;
            this.label7.Text = "Station(工站)：";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F);
            this.label8.Location = new System.Drawing.Point(26, 10);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(149, 12);
            this.label8.TabIndex = 26;
            this.label8.Text = "工站类别(Station Type)：";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F);
            this.label6.Location = new System.Drawing.Point(98, 37);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 30;
            this.label6.Text = "Line(线别)：";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // multiColumnComboBoxEx3
            // 
            this.multiColumnComboBoxEx3.BackColor = System.Drawing.Color.Yellow;
            this.multiColumnComboBoxEx3.ComboBoxHeight = 16;
            this.multiColumnComboBoxEx3.DisplayColumnNames = "ID,Description";
            this.multiColumnComboBoxEx3.DisplayMultiColumnsInBox = true;
            this.multiColumnComboBoxEx3.DropDownHeight = 258;
            this.multiColumnComboBoxEx3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.multiColumnComboBoxEx3.DropDownWidth = 117;
            this.multiColumnComboBoxEx3.Enabled = false;
            this.multiColumnComboBoxEx3.FormattingEnabled = true;
            this.multiColumnComboBoxEx3.ItemDropDownHeight = 32;
            this.multiColumnComboBoxEx3.Location = new System.Drawing.Point(484, 51);
            this.multiColumnComboBoxEx3.Margin = new System.Windows.Forms.Padding(2);
            this.multiColumnComboBoxEx3.Name = "multiColumnComboBoxEx3";
            this.multiColumnComboBoxEx3.Size = new System.Drawing.Size(65, 22);
            this.multiColumnComboBoxEx3.TabIndex = 25;
            this.multiColumnComboBoxEx3.Visible = false;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.groupBox2);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(627, 236);
            this.panel5.TabIndex = 39;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Btn_CreateSN);
            this.groupBox2.Controls.Add(this.Com_Templet);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.Com_PartFamilyType);
            this.groupBox2.Controls.Add(this.Com_PartFamily);
            this.groupBox2.Controls.Add(this.Com_Part);
            this.groupBox2.Controls.Add(this.txtNum);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(627, 236);
            this.groupBox2.TabIndex = 37;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "生成条码(Create SN)";
            // 
            // Btn_CreateSN
            // 
            this.Btn_CreateSN.Location = new System.Drawing.Point(416, 188);
            this.Btn_CreateSN.Name = "Btn_CreateSN";
            this.Btn_CreateSN.Size = new System.Drawing.Size(185, 25);
            this.Btn_CreateSN.TabIndex = 54;
            this.Btn_CreateSN.Text = "生成并打印(Create And Print)";
            this.Btn_CreateSN.UseVisualStyleBackColor = true;
            this.Btn_CreateSN.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // Com_Templet
            // 
            this.Com_Templet.Font = new System.Drawing.Font("宋体", 11F);
            this.Com_Templet.Location = new System.Drawing.Point(207, 145);
            this.Com_Templet.Name = "Com_Templet";
            this.Com_Templet.ReadOnly = true;
            this.Com_Templet.Size = new System.Drawing.Size(394, 24);
            this.Com_Templet.TabIndex = 53;
            this.Com_Templet.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F);
            this.label5.Location = new System.Drawing.Point(43, 152);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(149, 12);
            this.label5.TabIndex = 50;
            this.label5.Text = "模板名称(Templet Name)：";
            // 
            // Com_PartFamilyType
            // 
            this.Com_PartFamilyType.ComboBoxHeight = 16;
            this.Com_PartFamilyType.DisplayColumnNames = "ID,Name";
            this.Com_PartFamilyType.DisplayMultiColumnsInBox = true;
            this.Com_PartFamilyType.DropDownHeight = 258;
            this.Com_PartFamilyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Com_PartFamilyType.DropDownWidth = 117;
            this.Com_PartFamilyType.Font = new System.Drawing.Font("宋体", 10F);
            this.Com_PartFamilyType.FormattingEnabled = true;
            this.Com_PartFamilyType.ItemDropDownHeight = 32;
            this.Com_PartFamilyType.Location = new System.Drawing.Point(207, 22);
            this.Com_PartFamilyType.Margin = new System.Windows.Forms.Padding(2);
            this.Com_PartFamilyType.Name = "Com_PartFamilyType";
            this.Com_PartFamilyType.Size = new System.Drawing.Size(394, 22);
            this.Com_PartFamilyType.TabIndex = 18;
            this.Com_PartFamilyType.SelectedIndexChanged += new System.EventHandler(this.Com_PartFamilyType_SelectedIndexChanged);
            // 
            // Com_PartFamily
            // 
            this.Com_PartFamily.ComboBoxHeight = 16;
            this.Com_PartFamily.DisplayColumnNames = "ID,Name";
            this.Com_PartFamily.DisplayMultiColumnsInBox = true;
            this.Com_PartFamily.DropDownHeight = 258;
            this.Com_PartFamily.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Com_PartFamily.DropDownWidth = 117;
            this.Com_PartFamily.Font = new System.Drawing.Font("宋体", 10F);
            this.Com_PartFamily.FormattingEnabled = true;
            this.Com_PartFamily.ItemDropDownHeight = 32;
            this.Com_PartFamily.Location = new System.Drawing.Point(207, 63);
            this.Com_PartFamily.Margin = new System.Windows.Forms.Padding(2);
            this.Com_PartFamily.Name = "Com_PartFamily";
            this.Com_PartFamily.Size = new System.Drawing.Size(394, 22);
            this.Com_PartFamily.TabIndex = 9;
            this.Com_PartFamily.SelectedIndexChanged += new System.EventHandler(this.Com_PartFamily_SelectedIndexChanged);
            // 
            // Com_Part
            // 
            this.Com_Part.ComboBoxHeight = 16;
            this.Com_Part.DisplayColumnNames = "ID,PartNumber,Description";
            this.Com_Part.DisplayMultiColumnsInBox = true;
            this.Com_Part.DropDownHeight = 258;
            this.Com_Part.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Com_Part.DropDownWidth = 117;
            this.Com_Part.Font = new System.Drawing.Font("宋体", 10F);
            this.Com_Part.FormattingEnabled = true;
            this.Com_Part.ItemDropDownHeight = 32;
            this.Com_Part.Location = new System.Drawing.Point(207, 103);
            this.Com_Part.Margin = new System.Windows.Forms.Padding(2);
            this.Com_Part.Name = "Com_Part";
            this.Com_Part.Size = new System.Drawing.Size(394, 22);
            this.Com_Part.TabIndex = 17;
            this.Com_Part.SelectedIndexChanged += new System.EventHandler(this.Com_Part_SelectedIndexChanged);
            // 
            // txtNum
            // 
            this.txtNum.Font = new System.Drawing.Font("宋体", 11F);
            this.txtNum.Location = new System.Drawing.Point(207, 189);
            this.txtNum.Name = "txtNum";
            this.txtNum.Size = new System.Drawing.Size(203, 24);
            this.txtNum.TabIndex = 57;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F);
            this.label1.Location = new System.Drawing.Point(37, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "料号群(PartFamilyType）：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F);
            this.label4.Location = new System.Drawing.Point(61, 195);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 12);
            this.label4.TabIndex = 56;
            this.label4.Text = "打印数量(Print QTY)：";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F);
            this.label2.Location = new System.Drawing.Point(67, 69);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "料号组(PartFamily)：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F);
            this.label3.Location = new System.Drawing.Point(115, 109);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "料号(Part)：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 236);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1118, 394);
            this.panel2.TabIndex = 36;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridSN);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1118, 339);
            this.groupBox1.TabIndex = 36;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据表格(Grid Data)";
            // 
            // dataGridSN
            // 
            this.dataGridSN.AllowUserToAddRows = false;
            this.dataGridSN.AllowUserToDeleteRows = false;
            this.dataGridSN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridSN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridSN.Location = new System.Drawing.Point(3, 17);
            this.dataGridSN.Name = "dataGridSN";
            this.dataGridSN.RowTemplate.Height = 23;
            this.dataGridSN.Size = new System.Drawing.Size(1112, 319);
            this.dataGridSN.TabIndex = 20;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.Edt_MSG);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1118, 55);
            this.panel3.TabIndex = 35;
            // 
            // Edt_MSG
            // 
            this.Edt_MSG.BackColor = System.Drawing.Color.White;
            this.Edt_MSG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Edt_MSG.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Edt_MSG.ForeColor = System.Drawing.Color.Black;
            this.Edt_MSG.Location = new System.Drawing.Point(185, 0);
            this.Edt_MSG.Name = "Edt_MSG";
            this.Edt_MSG.ReadOnly = true;
            this.Edt_MSG.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.Edt_MSG.Size = new System.Drawing.Size(933, 55);
            this.Edt_MSG.TabIndex = 51;
            this.Edt_MSG.Text = "";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label10);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(185, 55);
            this.panel4.TabIndex = 50;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(32, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(152, 16);
            this.label10.TabIndex = 42;
            this.label10.Text = "Message(提示信息):";
            // 
            // frmPrintSnOffline
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1118, 630);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frmPrintSnOffline";
            this.Text = "条码打印";
            this.Load += new System.EventHandler(this.frmPrintSnOffline_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSN)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridSN;
        private System.Windows.Forms.RichTextBox Edt_MSG;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private MultiColumnComboBoxEx Com_Station;
        private MultiColumnComboBoxEx Com_Line;
        private MultiColumnComboBoxEx Com_StationType;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button Btn_CreateSN;
        private System.Windows.Forms.TextBox Com_Templet;
        private System.Windows.Forms.Label label5;
        private MultiColumnComboBoxEx Com_PartFamilyType;
        private MultiColumnComboBoxEx Com_PartFamily;
        private MultiColumnComboBoxEx Com_Part;
        private System.Windows.Forms.TextBox txtNum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtSN;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel11;
        private MultiColumnComboBoxEx multiColumnComboBoxEx3;
    }
}