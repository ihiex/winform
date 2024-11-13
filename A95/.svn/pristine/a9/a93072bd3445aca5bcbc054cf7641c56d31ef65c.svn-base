using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.Model;
using App.MyMES.PartSelectService;
using System.Configuration;
using App.MyMES.mesEmployeeService;
using System.Diagnostics;
using DevExpress.LookAndFeel;

namespace App.MyMES
{
    public partial class SetSkin_Form : DevExpress.XtraEditors.XtraForm
    {
        Public_ public_ = new Public_();
        MES_Dev_Form F_MES_Dev_Form;

        public SetSkin_Form()
        {
            InitializeComponent();
        }

        public void Show_SetStation_Form(MES_Dev_Form Main_Form, SetSkin_Form F_SetLangSkin_Form)
        {
            F_MES_Dev_Form = Main_Form;
            F_SetLangSkin_Form.ShowDialog();
        }

        private void SetLangSkin_Form_Load(object sender, EventArgs e)
        {
            public_.AddSkin(Com_Skin, Grid_Skin);

            try
            {
                string S_Path = Application.StartupPath;
                MyINI myINI = new MyINI(S_Path + "\\MES_Main.ini");
              
                string S_SkinID = myINI.ReadValue("SkinID", "Value");
                string S_LanguageID = myINI.ReadValue("LanguageID", "Value");


                if (S_SkinID != "") { Com_Skin.EditValue = S_SkinID; }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                string S_Path = Application.StartupPath;
                MyINI myINI = new MyINI(S_Path + "\\MES_Main.ini");

                string S_SkinID = Com_Skin.EditValue.ToString();

                myINI.WriteValue("SkinID", "Value", S_SkinID);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Com_Skin_EditValueChanged(object sender, EventArgs e)
        {
            string S_SkinName = Com_Skin.Text.Trim(); 
            F_MES_Dev_Form.defaultLook_Main.LookAndFeel.SkinName = S_SkinName;
        }
    }
}
