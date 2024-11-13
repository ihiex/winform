namespace BarPrint
{
    partial class BarTender_Form
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
            this.Btn_Print = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // Btn_Print
            // 
            this.Btn_Print.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Btn_Print.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_Print.ForeColor = System.Drawing.Color.Black;
            this.Btn_Print.Location = new System.Drawing.Point(0, 0);
            this.Btn_Print.Margin = new System.Windows.Forms.Padding(1);
            this.Btn_Print.Name = "Btn_Print";
            this.Btn_Print.Size = new System.Drawing.Size(235, 133);
            this.Btn_Print.TabIndex = 5;
            this.Btn_Print.Text = "BartenderPrint";
            this.Btn_Print.UseVisualStyleBackColor = false;
            this.Btn_Print.Click += new System.EventHandler(this.Btn_Print_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(204, 88);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(1);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(161, 104);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.Visible = false;
            // 
            // BarTender_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(2F, 3F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(235, 133);
            this.Controls.Add(this.Btn_Print);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("宋体", 2.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(500, 300);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BarTender_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BartenderPrint 2020-07-16";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BarTender_Form_FormClosed);
            this.Load += new System.EventHandler(this.BarTender_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Btn_Print;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}