using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using App.MyMES.SimensService;
using App.Model;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;

namespace App.MyMES
{
    public partial class WH_In_Old_Form : DevExpress.XtraEditors.XtraForm
    {
        public WH_In_Old_Form()
        {
            InitializeComponent();
        }

        SiemensSVCClient SiemensSVC;
        LoginList List_Login = new LoginList();
        private void Btn_UnLock_Click(object sender, EventArgs e)
        {
            Edt_MPN.Enabled = true;
            Btn_UnLock.Enabled = false;  
        }

        private void Edt_MPN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Edt_MPN.Text.Trim() != "")
                {
                    Edt_MPN.Enabled = false;
                    Btn_UnLock.Enabled = true;
                }
            }
        }

        private void Edt_SN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DateTime dateStart = DateTime.Now;
                string S_MPN = Edt_MPN.Text.Trim();   
                string S_BoxSN = Edt_SN.Text.Trim();

                string S_Type = "0";
                if (Check_NotIn.Checked == true) { S_Type = "1"; }
                string S_WHin = SiemensSVC.WHIn(S_MPN, S_BoxSN, S_Type, List_Login.StationTypeID.ToString());

                if (S_WHin == "OK")
                {
                    Edt_SN.Text = "";
                    Edt_SN.Focus();
                    TimeSpan ts = DateTime.Now - dateStart;
                    double mill = Math.Round(ts.TotalMilliseconds, 0);
                    MessageInfo.Add_Info_MSG(Edt_MSG, "10010", "OK", List_Login.Language, new string[] { S_BoxSN, mill.ToString() });

                    txtOverStationQTY.Text = (Convert.ToInt32(txtOverStationQTY.Text) + 1).ToString();   
                }
                else
                {
                    Edt_SN.Focus();
                    Edt_SN.SelectAll();
                    string ProMsg = MessageInfo.GetMsgByCode(S_WHin, List_Login.Language);
                    string S_GetMsgByFormName=MessageInfo.GetMsgByFormName(S_WHin, List_Login.Language);

                    MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { S_BoxSN, ProMsg, S_GetMsgByFormName }, S_WHin);
                }
               
                DataSet DS= SiemensSVC.WHIn_DT(List_Login.StationTypeID.ToString(), S_BoxSN);
                if (DS.Tables.Count == 3)
                {
                    Grid_Main.DataSource = DS.Tables[0];
                    string S_LCount = DS.Tables[1].Rows[0]["LCount"].ToString();  
                    string S_Reciepted=DS.Tables[2].Rows[0]["LCountS"].ToString();  

                    Lab_TotalBox.Text = "Total Box:"+ S_LCount; 
                    Lab_Reciepted.Text = "Reciepted:" + S_Reciepted;
                    Lab_UnReciepted.Text = "UnReciepted:"+(Convert.ToInt32(S_LCount) - Convert.ToInt32(S_Reciepted)).ToString(); 
                }
            }
        }

        private void WH_In_Form_Load(object sender, EventArgs e)
        {
            SiemensSVC = ISiemensFactory.CreateServerClient();
            List_Login = this.Tag as LoginList;

            var repositoryItemImageComboBoxStatus = new RepositoryItemImageComboBox();
            repositoryItemImageComboBoxStatus.Items.AddRange(new[]
            {
                    new ImageComboBoxItem("",0,0),
                    new ImageComboBoxItem("",1,1)
                });

            repositoryItemImageComboBoxStatus.Name = "repositoryItemImageComboBoxStatus";
            repositoryItemImageComboBoxStatus.SmallImages = imageList1;
            gridColumn3.ColumnEdit = repositoryItemImageComboBoxStatus;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtOverStationQTY.Text = "0";
        }
    }
}