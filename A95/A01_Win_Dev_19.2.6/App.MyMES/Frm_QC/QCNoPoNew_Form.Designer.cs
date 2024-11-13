﻿namespace App.MyMES
{
    partial class QCNoPoNew_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QCNoPoNew_Form));
            this.lblSN = new DevExpress.XtraEditors.LabelControl();
            this.lblChildSN = new DevExpress.XtraEditors.LabelControl();
            this.Edt_SN = new DevExpress.XtraEditors.TextEdit();
            this.Edt_ChildSN = new DevExpress.XtraEditors.TextEdit();
            this.List_ChildSN = new DevExpress.XtraEditors.ListBoxControl();
            this.lblDefectCode = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.DT_ProductionOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DT_ProductStructure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtDefect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlMSG)).BeginInit();
            this.GrpControlMSG.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlInputData)).BeginInit();
            this.GrpControlInputData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Com_luUnitStatus.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_PO.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_Part.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_PartFamily.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_PartFamilyType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlRouteDetail)).BeginInit();
            this.panel15.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlPart)).BeginInit();
            this.GrpControlPart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Edt_SN.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Edt_ChildSN.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.List_ChildSN)).BeginInit();
            this.SuspendLayout();
            // 
            // GrpControlMSG
            // 
            this.GrpControlMSG.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GrpControlMSG.Size = new System.Drawing.Size(997, 97);
            // 
            // GrpControlInputData
            // 
            this.GrpControlInputData.Controls.Add(this.lblDefectCode);
            this.GrpControlInputData.Controls.Add(this.List_ChildSN);
            this.GrpControlInputData.Controls.Add(this.Edt_ChildSN);
            this.GrpControlInputData.Controls.Add(this.Edt_SN);
            this.GrpControlInputData.Controls.Add(this.lblChildSN);
            this.GrpControlInputData.Controls.Add(this.lblSN);
            // 
            // Com_luUnitStatus
            // 
            this.Com_luUnitStatus.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Com_luUnitStatus.Properties.Appearance.Options.UseFont = true;
            this.Com_luUnitStatus.Size = new System.Drawing.Size(603, 26);
            // 
            // Com_PO
            // 
            this.Com_PO.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Com_PO.Properties.Appearance.Options.UseFont = true;
            this.Com_PO.Size = new System.Drawing.Size(603, 26);
            this.Com_PO.Visible = false;
            // 
            // Com_Part
            // 
            this.Com_Part.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Com_Part.Properties.Appearance.Options.UseFont = true;
            this.Com_Part.Size = new System.Drawing.Size(603, 26);
            this.Com_Part.Visible = false;
            // 
            // Com_PartFamily
            // 
            this.Com_PartFamily.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Com_PartFamily.Properties.Appearance.Options.UseFont = true;
            this.Com_PartFamily.Size = new System.Drawing.Size(603, 26);
            this.Com_PartFamily.Visible = false;
            // 
            // Com_PartFamilyType
            // 
            this.Com_PartFamilyType.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Com_PartFamilyType.Properties.Appearance.Options.UseFont = true;
            this.Com_PartFamilyType.Size = new System.Drawing.Size(603, 26);
            this.Com_PartFamilyType.Visible = false;
            // 
            // Edt_MSG
            // 
            this.Edt_MSG.Location = new System.Drawing.Point(2, 23);
            this.Edt_MSG.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            // 
            // Btn_ConfirmPO
            // 
            this.Btn_ConfirmPO.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_ConfirmPO.Appearance.Options.UseFont = true;
            this.Btn_ConfirmPO.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("Btn_ConfirmPO.ImageOptions.SvgImage")));
            this.Btn_ConfirmPO.Visible = false;
            // 
            // Panel_Defact
            // 
            this.Panel_Defact.Location = new System.Drawing.Point(2, 23);
            this.Panel_Defact.Size = new System.Drawing.Size(993, 103);
            // 
            // Btn_Defect
            // 
            this.Btn_Defect.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Defect.Appearance.Options.UseFont = true;
            this.Btn_Defect.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("Btn_Defect.ImageOptions.SvgImage")));
            // 
            // Btn_Refresh
            // 
            this.Btn_Refresh.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Refresh.Appearance.Options.UseFont = true;
            this.Btn_Refresh.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("Btn_Refresh.ImageOptions.SvgImage")));
            this.Btn_Refresh.Visible = false;
            // 
            // GrpControlRouteDetail
            // 
            this.GrpControlRouteDetail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GrpControlRouteDetail.Size = new System.Drawing.Size(370, 310);
            // 
            // lblUnitState
            // 
            this.lblUnitState.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblUnitState.Appearance.Options.UseFont = true;
            // 
            // lblProductionOrder
            // 
            this.lblProductionOrder.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblProductionOrder.Appearance.Options.UseFont = true;
            this.lblProductionOrder.Visible = false;
            // 
            // lblPart
            // 
            this.lblPart.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblPart.Appearance.Options.UseFont = true;
            this.lblPart.Visible = false;
            // 
            // lblPartFamily
            // 
            this.lblPartFamily.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblPartFamily.Appearance.Options.UseFont = true;
            this.lblPartFamily.Visible = false;
            // 
            // lblPartFamilyType
            // 
            this.lblPartFamilyType.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblPartFamilyType.Appearance.Options.UseFont = true;
            this.lblPartFamilyType.Visible = false;
            // 
            // lblSN
            // 
            this.lblSN.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lblSN.Appearance.Options.UseFont = true;
            this.lblSN.Location = new System.Drawing.Point(62, 117);
            this.lblSN.Name = "lblSN";
            this.lblSN.Size = new System.Drawing.Size(128, 19);
            this.lblSN.TabIndex = 0;
            this.lblSN.Text = "Scan SN(扫描SN):";
            // 
            // lblChildSN
            // 
            this.lblChildSN.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lblChildSN.Appearance.Options.UseFont = true;
            this.lblChildSN.Location = new System.Drawing.Point(5, 207);
            this.lblChildSN.Name = "lblChildSN";
            this.lblChildSN.Size = new System.Drawing.Size(185, 19);
            this.lblChildSN.TabIndex = 1;
            this.lblChildSN.Text = "Scan Child SN(扫描子SN):";
            this.lblChildSN.Visible = false;
            // 
            // Edt_SN
            // 
            this.Edt_SN.Enabled = false;
            this.Edt_SN.Location = new System.Drawing.Point(228, 113);
            this.Edt_SN.Name = "Edt_SN";
            this.Edt_SN.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Edt_SN.Properties.Appearance.Options.UseFont = true;
            this.Edt_SN.Size = new System.Drawing.Size(685, 26);
            this.Edt_SN.TabIndex = 2;
            this.Edt_SN.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Edt_SN_KeyDown);
            // 
            // Edt_ChildSN
            // 
            this.Edt_ChildSN.Enabled = false;
            this.Edt_ChildSN.Location = new System.Drawing.Point(228, 204);
            this.Edt_ChildSN.Name = "Edt_ChildSN";
            this.Edt_ChildSN.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Edt_ChildSN.Properties.Appearance.Options.UseFont = true;
            this.Edt_ChildSN.Size = new System.Drawing.Size(685, 26);
            this.Edt_ChildSN.TabIndex = 3;
            this.Edt_ChildSN.Visible = false;
            this.Edt_ChildSN.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Edt_ChildSN_KeyDown);
            // 
            // List_ChildSN
            // 
            this.List_ChildSN.Enabled = false;
            this.List_ChildSN.Location = new System.Drawing.Point(228, 246);
            this.List_ChildSN.Name = "List_ChildSN";
            this.List_ChildSN.Size = new System.Drawing.Size(685, 88);
            this.List_ChildSN.TabIndex = 4;
            this.List_ChildSN.Visible = false;
            // 
            // lblDefectCode
            // 
            this.lblDefectCode.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lblDefectCode.Appearance.Options.UseFont = true;
            this.lblDefectCode.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblDefectCode.Location = new System.Drawing.Point(228, 157);
            this.lblDefectCode.Name = "lblDefectCode";
            this.lblDefectCode.Size = new System.Drawing.Size(682, 19);
            this.lblDefectCode.TabIndex = 5;
            this.lblDefectCode.Text = "DefectCode:";
            this.lblDefectCode.Visible = false;
            // 
            // QCNoPoNew_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1381, 651);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "QCNoPoNew_Form";
            this.Text = "QCNoPoNew_Form";
            this.Load += new System.EventHandler(this.QCNoPo_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DT_ProductionOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DT_ProductStructure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtDefect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlMSG)).EndInit();
            this.GrpControlMSG.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlInputData)).EndInit();
            this.GrpControlInputData.ResumeLayout(false);
            this.GrpControlInputData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Com_luUnitStatus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_PO.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_Part.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_PartFamily.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Com_PartFamilyType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlRouteDetail)).EndInit();
            this.panel15.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel14.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GrpControlPart)).EndInit();
            this.GrpControlPart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Edt_SN.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Edt_ChildSN.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.List_ChildSN)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblSN;
        private DevExpress.XtraEditors.TextEdit Edt_ChildSN;
        private DevExpress.XtraEditors.TextEdit Edt_SN;
        private DevExpress.XtraEditors.LabelControl lblChildSN;
        private DevExpress.XtraEditors.ListBoxControl List_ChildSN;
        private DevExpress.XtraEditors.LabelControl lblDefectCode;
    }
}