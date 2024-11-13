namespace MESUpdate_FTP
{
    partial class Update_FTP_Form
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Update_FTP_Form));
            this.Btn_Cancel = new System.Windows.Forms.Button();
            this.Btn_OK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Check_Dev = new System.Windows.Forms.CheckBox();
            this.Lab_URL = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Btn_Cancel
            // 
            this.Btn_Cancel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_Cancel.Location = new System.Drawing.Point(213, 149);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(89, 32);
            this.Btn_Cancel.TabIndex = 25;
            this.Btn_Cancel.Text = "Cancel";
            this.Btn_Cancel.UseVisualStyleBackColor = true;
            this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);
            // 
            // Btn_OK
            // 
            this.Btn_OK.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_OK.Location = new System.Drawing.Point(111, 149);
            this.Btn_OK.Name = "Btn_OK";
            this.Btn_OK.Size = new System.Drawing.Size(89, 32);
            this.Btn_OK.TabIndex = 24;
            this.Btn_OK.Text = "OK";
            this.Btn_OK.UseVisualStyleBackColor = true;
            this.Btn_OK.Click += new System.EventHandler(this.Btn_OK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(82, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(273, 21);
            this.label1.TabIndex = 26;
            this.label1.Text = "Are you sure to update？";
            // 
            // Check_Dev
            // 
            this.Check_Dev.AutoSize = true;
            this.Check_Dev.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Check_Dev.Location = new System.Drawing.Point(130, 102);
            this.Check_Dev.Name = "Check_Dev";
            this.Check_Dev.Size = new System.Drawing.Size(139, 20);
            this.Check_Dev.TabIndex = 27;
            this.Check_Dev.Text = "Update DEV DLL";
            this.Check_Dev.UseVisualStyleBackColor = true;
            // 
            // Lab_URL
            // 
            this.Lab_URL.AutoSize = true;
            this.Lab_URL.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lab_URL.ForeColor = System.Drawing.Color.Red;
            this.Lab_URL.Location = new System.Drawing.Point(19, 199);
            this.Lab_URL.Name = "Lab_URL";
            this.Lab_URL.Size = new System.Drawing.Size(49, 20);
            this.Lab_URL.TabIndex = 28;
            this.Lab_URL.Text = "FTP:";
            // 
            // Update_FTP_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 233);
            this.Controls.Add(this.Lab_URL);
            this.Controls.Add(this.Check_Dev);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Btn_Cancel);
            this.Controls.Add(this.Btn_OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Update_FTP_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Update  FTP 2020-06-21";
            this.Load += new System.EventHandler(this.Update_Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Btn_Cancel;
        private System.Windows.Forms.Button Btn_OK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox Check_Dev;
        private System.Windows.Forms.Label Lab_URL;
    }
}

