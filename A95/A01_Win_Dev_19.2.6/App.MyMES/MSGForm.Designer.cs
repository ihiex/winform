namespace App.MyMES
{
    partial class MSGForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MSGForm));
            this.Btn_OK = new DevExpress.XtraEditors.SimpleButton();
            this.defaultLook_Main = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.behaviorManager1 = new DevExpress.Utils.Behaviors.BehaviorManager(this.components);
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem3 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem7 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem8 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem9 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem10 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem11 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem12 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem13 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem14 = new DevExpress.XtraBars.BarStaticItem();
            this.Memo_MSG = new DevExpress.XtraEditors.MemoEdit();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Memo_MSG.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // Btn_OK
            // 
            this.Btn_OK.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_OK.Appearance.Options.UseFont = true;
            this.Btn_OK.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("Btn_OK.ImageOptions.SvgImage")));
            this.Btn_OK.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.Btn_OK.Location = new System.Drawing.Point(302, 442);
            this.Btn_OK.Name = "Btn_OK";
            this.Btn_OK.Size = new System.Drawing.Size(110, 33);
            this.Btn_OK.TabIndex = 79;
            this.Btn_OK.Text = "Close";
            this.Btn_OK.Click += new System.EventHandler(this.Btn_OK_Click);
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 483);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.ribbonControl1;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(754, 24);
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.AllowContentChangeAnimation = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.AllowKeyTips = false;
            this.ribbonControl1.AllowMdiChildButtons = false;
            this.ribbonControl1.AllowMinimizeRibbon = false;
            this.ribbonControl1.AllowTrimPageText = false;
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.ribbonControl1.SearchEditItem});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 9;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowDisplayOptionsMenuButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowMoreCommandsButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowPageHeadersInFormCaption = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            this.ribbonControl1.ShowQatLocationSelector = false;
            this.ribbonControl1.ShowToolbarCustomizeItem = false;
            this.ribbonControl1.Size = new System.Drawing.Size(754, 32);
            this.ribbonControl1.StatusBar = this.ribbonStatusBar1;
            this.ribbonControl1.Toolbar.ShowCustomizeItem = false;
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barStaticItem1.Caption = "版本：ver_2020-03-26";
            this.barStaticItem1.Id = 9;
            this.barStaticItem1.Name = "barStaticItem1";
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.Caption = "  系统名称:COSMO MES   ";
            this.barStaticItem2.Id = 10;
            this.barStaticItem2.Name = "barStaticItem2";
            // 
            // barStaticItem3
            // 
            this.barStaticItem3.Caption = "  开发者：COSMO IT MES 开发团队";
            this.barStaticItem3.Id = 11;
            this.barStaticItem3.Name = "barStaticItem3";
            // 
            // barStaticItem7
            // 
            this.barStaticItem7.Caption = " Version:";
            this.barStaticItem7.Id = 1;
            this.barStaticItem7.Name = "barStaticItem7";
            // 
            // barStaticItem8
            // 
            this.barStaticItem8.Caption = "2020-10-29 11:30";
            this.barStaticItem8.Id = 3;
            this.barStaticItem8.Name = "barStaticItem8";
            // 
            // barStaticItem9
            // 
            this.barStaticItem9.Caption = "DBServer:";
            this.barStaticItem9.Id = 4;
            this.barStaticItem9.Name = "barStaticItem9";
            // 
            // barStaticItem10
            // 
            this.barStaticItem10.Id = 2;
            this.barStaticItem10.Name = "barStaticItem10";
            // 
            // barStaticItem11
            // 
            this.barStaticItem11.Caption = "DBName:";
            this.barStaticItem11.Id = 5;
            this.barStaticItem11.Name = "barStaticItem11";
            // 
            // barStaticItem12
            // 
            this.barStaticItem12.Id = 6;
            this.barStaticItem12.Name = "barStaticItem12";
            // 
            // barStaticItem13
            // 
            this.barStaticItem13.Caption = "WepIP:";
            this.barStaticItem13.Id = 7;
            this.barStaticItem13.Name = "barStaticItem13";
            // 
            // barStaticItem14
            // 
            this.barStaticItem14.Id = 8;
            this.barStaticItem14.Name = "barStaticItem14";
            // 
            // Memo_MSG
            // 
            this.Memo_MSG.Location = new System.Drawing.Point(25, 45);
            this.Memo_MSG.MenuManager = this.ribbonControl1;
            this.Memo_MSG.Name = "Memo_MSG";
            this.Memo_MSG.Properties.ReadOnly = true;
            this.Memo_MSG.Size = new System.Drawing.Size(704, 388);
            this.Memo_MSG.TabIndex = 95;
            // 
            // timer1
            // 
            this.timer1.Interval = 120000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MSGForm
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 507);
            this.Controls.Add(this.Memo_MSG);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Controls.Add(this.Btn_OK);
            this.Controls.Add(this.ribbonControl1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("MSGForm.IconOptions.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MSGForm";
            this.Ribbon = this.ribbonControl1;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StatusBar = this.ribbonStatusBar1;
            this.Text = "Issue Feedback";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MSGForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Memo_MSG.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton Btn_OK;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLook_Main;
        private DevExpress.Utils.Behaviors.BehaviorManager behaviorManager1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
        private DevExpress.XtraBars.BarStaticItem barStaticItem3;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem7;
        private DevExpress.XtraBars.BarStaticItem barStaticItem8;
        private DevExpress.XtraBars.BarStaticItem barStaticItem9;
        private DevExpress.XtraBars.BarStaticItem barStaticItem10;
        private DevExpress.XtraBars.BarStaticItem barStaticItem11;
        private DevExpress.XtraBars.BarStaticItem barStaticItem12;
        private DevExpress.XtraBars.BarStaticItem barStaticItem13;
        private DevExpress.XtraBars.BarStaticItem barStaticItem14;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraEditors.MemoEdit Memo_MSG;
        public System.Windows.Forms.Timer timer1;
    }
}