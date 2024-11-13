namespace App.MyMES
{
    partial class PartSelectForm
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
            this.Com_PartFamilyType = new System.Windows.Forms.ComboBox();
            this.Btn_Refurbish = new System.Windows.Forms.Button();
            this.Com_PartFamily = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Com_Part = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Com_PartFamilyType
            // 
            this.Com_PartFamilyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Com_PartFamilyType.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Com_PartFamilyType.FormattingEnabled = true;
            this.Com_PartFamilyType.Location = new System.Drawing.Point(344, 21);
            this.Com_PartFamilyType.Name = "Com_PartFamilyType";
            this.Com_PartFamilyType.Size = new System.Drawing.Size(198, 26);
            this.Com_PartFamilyType.TabIndex = 0;
            this.Com_PartFamilyType.SelectedIndexChanged += new System.EventHandler(this.Com_PartFamilyType_SelectedIndexChanged);
            // 
            // Btn_Refurbish
            // 
            this.Btn_Refurbish.Location = new System.Drawing.Point(40, 17);
            this.Btn_Refurbish.Name = "Btn_Refurbish";
            this.Btn_Refurbish.Size = new System.Drawing.Size(103, 33);
            this.Btn_Refurbish.TabIndex = 1;
            this.Btn_Refurbish.Text = "Refurbish";
            this.Btn_Refurbish.UseVisualStyleBackColor = true;
            this.Btn_Refurbish.Click += new System.EventHandler(this.Btn_Refurbish_Click);
            // 
            // Com_PartFamily
            // 
            this.Com_PartFamily.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Com_PartFamily.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Com_PartFamily.FormattingEnabled = true;
            this.Com_PartFamily.Location = new System.Drawing.Point(344, 70);
            this.Com_PartFamily.Name = "Com_PartFamily";
            this.Com_PartFamily.Size = new System.Drawing.Size(198, 26);
            this.Com_PartFamily.TabIndex = 2;
            this.Com_PartFamily.SelectedIndexChanged += new System.EventHandler(this.Com_PartFamily_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(191, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "PartFamilyType：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(223, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "PartFamily：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(271, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Part：";
            // 
            // Com_Part
            // 
            this.Com_Part.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Com_Part.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Com_Part.FormattingEnabled = true;
            this.Com_Part.Location = new System.Drawing.Point(344, 119);
            this.Com_Part.Name = "Com_Part";
            this.Com_Part.Size = new System.Drawing.Size(198, 26);
            this.Com_Part.TabIndex = 6;
            // 
            // PartSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1097, 669);
            this.Controls.Add(this.Com_Part);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Com_PartFamily);
            this.Controls.Add(this.Btn_Refurbish);
            this.Controls.Add(this.Com_PartFamilyType);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "PartSelectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PartSelectForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox Com_PartFamilyType;
        private System.Windows.Forms.Button Btn_Refurbish;
        private System.Windows.Forms.ComboBox Com_PartFamily;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox Com_Part;
    }
}