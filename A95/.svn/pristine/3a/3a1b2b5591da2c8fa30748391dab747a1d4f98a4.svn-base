using App.MyMES.PartSelectService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace App.MyMES
{
    public partial class PartSelectForm : Form
    {
        public PartSelectForm()
        {
            InitializeComponent();
        }

        private void Btn_Refurbish_Click(object sender, EventArgs e)
        {
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            DataTable DT= PartSelectSVC.GetluPartFamilyType().Tables[0];

            Com_PartFamilyType.Items.Clear();
            Com_PartFamily.Items.Clear();
            Com_Part.Items.Clear();


            for (int i = 0; i < DT.Rows.Count; i++)
            {
                Com_PartFamilyType.Items.Add(DT.Rows[i]["ID"].ToString() + ":" + DT.Rows[i]["Name"].ToString());
            }

            PartSelectSVC.Close();
        }

        private void Com_PartFamilyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string S_PartFamilyType = Com_PartFamilyType.Text.Trim();
            string[] Array_PartFamilyType = S_PartFamilyType.Split(':');
            string S_PartFamilyTypeID = Array_PartFamilyType[0].Trim();

            Com_PartFamily.Items.Clear();

            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            DataTable DT = PartSelectSVC.GetluPartFamily(S_PartFamilyTypeID).Tables[0];
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                Com_PartFamily.Items.Add(DT.Rows[i]["ID"].ToString() + ":" + DT.Rows[i]["Name"].ToString());
            }

            PartSelectSVC.Close();
        }

        private void Com_PartFamily_SelectedIndexChanged(object sender, EventArgs e)
        {
            string S_PartFamily = Com_PartFamily.Text.Trim();
            string[] Array_Part = S_PartFamily.Split(':');
            string S_PartFamilyID = Array_Part[0].Trim();

            Com_Part.Items.Clear();
            PartSelectSVCClient PartSelectSVC = PartSelectFactory.CreateServerClient();
            DataTable DT = PartSelectSVC.GetmesPart(S_PartFamilyID).Tables[0];
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                Com_Part.Items.Add(DT.Rows[i]["ID"].ToString() + ":" + DT.Rows[i]["PartNumber"].ToString());
            }

            PartSelectSVC.Close();
        }


    }
}
