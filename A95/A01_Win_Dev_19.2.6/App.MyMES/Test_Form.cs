using App.Model;
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
    public partial class Test_Form : Form
    {
        public Test_Form()
        {
            InitializeComponent();
        }

        LoginList F_LoginList;

        public void show2(LoginList v_LoginList, Test_Form v_Test_Form)
        {
            F_LoginList = v_LoginList;
            v_Test_Form.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mesUnit v_mesUnit = new mesUnit();
            v_mesUnit.UnitStateID = 102;
            v_mesUnit.StatusID = 1;
            v_mesUnit.StationID = 2;
            v_mesUnit.EmployeeID =13;
            v_mesUnit.CreationTime = DateTime.Now;
            v_mesUnit.LastUpdate = DateTime.Now;
            v_mesUnit.PanelID = 0;
            v_mesUnit.LineID = 5;
            v_mesUnit.ProductionOrderID = 79;
            v_mesUnit.RMAID = 0;
            v_mesUnit.PartID = 109;
            v_mesUnit.LooperCount = 1;
            v_mesUnit.StatusID = 1;

            string ProductionOrderID = "79";
            string ss1 = "2";
            DataSet dsSN = new DataSet();
            //App.Model.LoginList List_Login = new App.Model.LoginList();
            PartSelectSVCClient PartSelectSVC= PartSelectFactory.CreateServerClient(); 

            string S_Production0rder = "'<ProdOrder ProductionOrder=" + "\"" + ProductionOrderID + "\"" + "> </ProdOrder>'";
            string S_Station = "'<Station StationID=" + "\"" + ss1 + "\"" + "> </Station>'";
            string S_xmlPart = "'<Part PartID=" + "\"" + 109 + "\"" + "> </Part>'";
            string S_xmlExtraData = "'<ExtraData LineID=" + "\"" + 5 + "\"" +
                                             " PartFamilyTypeID=" + "\"" + 18 + "\"" +
                                             " LineType=" + "\"" + "M" + "\"" + " > </ExtraData>'";
           string  result = PartSelectSVC.Get_CreateMesSN(null, F_LoginList, S_Production0rder,
                S_xmlPart, S_Station, S_xmlExtraData, v_mesUnit, 1, ref dsSN);

            

        }
    }
}
