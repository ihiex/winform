namespace App.MyMES
{
    partial class SetSkin_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetSkin_Form));
            this.Btn_Cancel = new DevExpress.XtraEditors.SimpleButton();
            this.Btn_OK = new DevExpress.XtraEditors.SimpleButton();
            this.Com_Skin = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.Grid_Skin = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Com_Skin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Skin)).BeginInit();
            this.SuspendLayout();
            // 
            // Btn_Cancel
            // 
            this.Btn_Cancel.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Cancel.Appearance.Options.UseFont = true;
            this.Btn_Cancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("Btn_Cancel.ImageOptions.Image")));
            this.Btn_Cancel.Location = new System.Drawing.Point(273, 157);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(110, 33);
            this.Btn_Cancel.TabIndex = 88;
            this.Btn_Cancel.Text = "Cancel";
            this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);
            // 
            // Btn_OK
            // 
            this.Btn_OK.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_OK.Appearance.Options.UseFont = true;
            this.Btn_OK.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("Btn_OK.ImageOptions.Image")));
            this.Btn_OK.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.Btn_OK.Location = new System.Drawing.Point(130, 157);
            this.Btn_OK.Name = "Btn_OK";
            this.Btn_OK.Size = new System.Drawing.Size(110, 33);
            this.Btn_OK.TabIndex = 87;
            this.Btn_OK.Text = "Confirm";
            this.Btn_OK.Click += new System.EventHandler(this.Btn_OK_Click);
            // 
            // Com_Skin
            // 
            this.Com_Skin.EditValue = "";
            this.Com_Skin.Location = new System.Drawing.Point(119, 85);
            this.Com_Skin.Name = "Com_Skin";
            this.Com_Skin.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.Com_Skin.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Com_Skin.Properties.Appearance.Options.UseBackColor = true;
            this.Com_Skin.Properties.Appearance.Options.UseFont = true;
            this.Com_Skin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Com_Skin.Properties.PopupView = this.Grid_Skin;
            this.Com_Skin.Size = new System.Drawing.Size(294, 26);
            this.Com_Skin.TabIndex = 85;
            this.Com_Skin.EditValueChanged += new System.EventHandler(this.Com_Skin_EditValueChanged);
            // 
            // Grid_Skin
            // 
            this.Grid_Skin.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.Grid_Skin.Name = "Grid_Skin";
            this.Grid_Skin.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.Grid_Skin.OptionsView.ShowGroupPanel = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(70, 90);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 16);
            this.label6.TabIndex = 84;
            this.label6.Text = "Skin：";
            // 
            // SetSkin_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 242);
            this.Controls.Add(this.Btn_Cancel);
            this.Controls.Add(this.Btn_OK);
            this.Controls.Add(this.Com_Skin);
            this.Controls.Add(this.label6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetSkin_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Set Language/Skin";
            this.Load += new System.EventHandler(this.SetLangSkin_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Com_Skin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Skin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton Btn_Cancel;
        private DevExpress.XtraEditors.SimpleButton Btn_OK;
        private DevExpress.XtraEditors.SearchLookUpEdit Com_Skin;
        private DevExpress.XtraGrid.Views.Grid.GridView Grid_Skin;
        private System.Windows.Forms.Label label6;
    }
}