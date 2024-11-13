namespace App.MyMES
{
    partial class T_Form
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
            this.Main_Title = new System.Windows.Forms.Panel();
            this.Main_Panel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // Main_Title
            // 
            this.Main_Title.BackColor = System.Drawing.Color.LightSteelBlue;
            this.Main_Title.Dock = System.Windows.Forms.DockStyle.Top;
            this.Main_Title.Location = new System.Drawing.Point(0, 0);
            this.Main_Title.Name = "Main_Title";
            this.Main_Title.Size = new System.Drawing.Size(642, 34);
            this.Main_Title.TabIndex = 1;
            this.Main_Title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Main_Title_MouseDown);
            // 
            // Main_Panel
            // 
            this.Main_Panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Main_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Main_Panel.Location = new System.Drawing.Point(0, 34);
            this.Main_Panel.Name = "Main_Panel";
            this.Main_Panel.Padding = new System.Windows.Forms.Padding(7);
            this.Main_Panel.Size = new System.Drawing.Size(642, 470);
            this.Main_Panel.TabIndex = 2;
            // 
            // T_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 504);
            this.Controls.Add(this.Main_Panel);
            this.Controls.Add(this.Main_Title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "T_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "T_Form";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Main_Title;
        private System.Windows.Forms.Panel Main_Panel;
    }
}