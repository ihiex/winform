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
using App.MyMES.mesUnitService;
using App.MyMES.mesSerialNumberService;
using App.MyMES.mesHistoryService;
using App.MyMES.mesUnitDefectService;
using App.MyMES.mesPartDetailService;
using System.Xml;

namespace App.MyMES
{
    public partial class frmPrintSnOffline : Form
    {
        string S_Path = Application.StartupPath;
        string S_TempletName;
        Public_ public_ = new Public_();
        LoginList List_Login = new LoginList();
        LabelManager2.Application LabSN = new LabelManager2.Application();
        DataTable dtPrintSn;
        int S_PartID;

        public frmPrintSnOffline()
        {
            InitializeComponent();
        }

        private void frmPrintSnOffline_Load(object sender, EventArgs e)
        {
            LoadComBox();
        }

        private void LoadComBox()
        {
            public_.AddPartFamilyType(Com_PartFamilyType);
            
            List_Login = this.Tag as LoginList;

            public_.AddStationType(Com_StationType);
            public_.AddLine(Com_Line);

            string S_StationTypeID = List_Login.StationTypeID.ToString();
            string S_LineID = List_Login.LineID.ToString();
            public_.AddStation(Com_Station, S_LineID);

            Com_StationType.Text = S_StationTypeID;
            Com_Line.Text = S_LineID;
            Com_Station.Text = List_Login.StationID.ToString();
        }

        private void Com_PartFamilyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView DRV = Com_PartFamilyType.SelectedItem as DataRowView;
            if (DRV != null)
            {
                string S_PartFamilyTypeID = DRV["ID"].ToString();
                public_.AddPartFamily(Com_PartFamily, S_PartFamilyTypeID);
            }
        }


        private void Com_PartFamily_SelectedIndexChanged(object sender, EventArgs e)
        {
            string S_PartFamily = Com_PartFamily.Text.Trim();

            DataRowView DRV = Com_PartFamily.SelectedItem as DataRowView;
            if (DRV != null)
            {
                string S_PartFamilyID = DRV["ID"].ToString();
                public_.AddPart(Com_Part, S_PartFamilyID);
            }
        }

        private void Com_Part_SelectedIndexChanged(object sender, EventArgs e)
        {
            string S_Part = Com_Part.Text.Trim();

            DataRowView DRV = Com_Part.SelectedItem as DataRowView;
            if (DRV != null)
            {
                S_PartID = Convert.ToInt32(DRV["ID"].ToString());
                ImesPartDetailSVCClient client = new ImesPartDetailSVCClient();
                DataSet dataSet = client.GetmesPartDetail(S_PartID, "LabelTemplatePath");
                if(dataSet==null || dataSet.Tables.Count==0|| dataSet.Tables[0].Rows.Count==0)
                {
                    Com_Templet.Text = "";
                    public_.Add_Info_MSG(Edt_MSG,  "未配置打印模板!", "NG");
                }
                else
                {
                    Com_Templet.Text = dataSet.Tables[0].Rows[0]["Content"].ToString();
                    public_.Add_Info_MSG(Edt_MSG, "", "OK");
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(Com_Templet.Text.Trim()))
            {
                public_.Add_Info_MSG(Edt_MSG, "未配置打印文件路径！", "NG");
                return;
            }

            if (!Public_.IsNumeric(txtNum.Text.Trim()))
            {
                public_.Add_Info_MSG(Edt_MSG, "请在'数量'栏位输入数字类型的值！", "NG");
                return;
            }

            if (Convert.ToInt32(txtNum.Text.Trim())>200)
            {
                public_.Add_Info_MSG(Edt_MSG, "打印数量不能大于200！", "NG");
                return;
            }

            int num = Convert.ToInt32(txtNum.Text.Trim());
            string result = string.Empty;
            PartSelectSVCClient partClint = new PartSelectSVCClient();

            DataSet dsSN=new DataSet();

            mesUnit v_mesUnit = new mesUnit();
            v_mesUnit.UnitStateID = 1;
            v_mesUnit.StatusID = 1;
            v_mesUnit.StationID = List_Login.StationID;
            v_mesUnit.EmployeeID = List_Login.EmployeeID;
            v_mesUnit.CreationTime = DateTime.Now;
            v_mesUnit.LastUpdate = DateTime.Now;
            v_mesUnit.PanelID = 0;
            v_mesUnit.LineID = List_Login.LineID;
            v_mesUnit.ProductionOrderID = 0;
            v_mesUnit.RMAID = 0;
            v_mesUnit.PartID = Convert.ToInt32(S_PartID);
            v_mesUnit.LooperCount = 1;

            result = partClint.Get_CreateMesSN(null, null, null, null, v_mesUnit, num ,ref dsSN);
            if (result == "1" && dsSN != null && dsSN.Tables.Count >0)
            {
                dtPrintSn = dsSN.Tables[0];
                dtPrintSn.Columns.Add("CreateTime", typeof(string));
                dtPrintSn.Columns.Add("PrintTime", typeof(string));
                foreach (DataRow dr in dtPrintSn.Rows)
                {
                    dr["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                dataGridSN.DataSource = dtPrintSn;

                if (!PrintCodeSoftSN())
                {
                    public_.Add_Info_MSG(Edt_MSG, "条码打印失败！", "NG");
                }
                else
                {
                    public_.Add_Info_MSG(Edt_MSG, "条码生成成功并发送至打印机！", "OK");
                    txtNum.Text = "";

                }
            }
            else
            {
                public_.Add_Info_MSG(Edt_MSG, string.Format("条码生成失败;{0}！", result), "NG");
            }
            
        }

        private bool PrintCodeSoftSN()
        {
            if(dtPrintSn==null || dtPrintSn.Rows.Count==0)
            {
                return false;
            }

            LabSN = null;
            LabelManager2.Document doc = null;

            try
            {
                if (LabSN == null)
                {
                    LabSN = new LabelManager2.Application();
                }
                string S_Path_Lab = S_Path + S_TempletName;

                LabSN.Documents.Open(S_Path_Lab, false);
                doc = LabSN.ActiveDocument;
            }
            catch(Exception ex)
            {
                public_.Add_Info_MSG(Edt_MSG, ex.Message, "NG");
            }

            try
            {
                foreach(DataRow dr in dtPrintSn.Rows)
                {
                    doc.Variables.Item("SN").Value = dr["SN"].ToString();
                    doc.PrintLabel(1);
                    dr["PrintTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }

                doc.FormFeed();
                doc.Close();
                return true;
            }
            catch (Exception ex)
            {
                doc.Close();
                return false;
            }
            finally
            {
                LabSN.Documents.CloseAll();
                LabSN.Quit();//退出
                LabSN = null;
                doc = null;
                GC.Collect(0);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtSN.Text.Trim()))
            {
                public_.Add_Info_MSG(Edt_MSG, "补打条码不能为空！", "NG");
                return;
            }

            if (string.IsNullOrEmpty(Com_Templet.Text.Trim()))
            {
                public_.Add_Info_MSG(Edt_MSG, "未配置打印文件路径！", "NG");
                return;
            }

            if (LabSN == null)
            {
                LabSN = new LabelManager2.Application();
            }

            string S_Path_Lab = S_Path + S_TempletName;
            LabSN.Documents.Open(S_Path_Lab, false);
            LabelManager2.Document doc = LabSN.ActiveDocument;
            doc.Variables.Item("SN").Value = txtSN.Text.Trim();

            try
            {
                doc.PrintDocument(1);
                doc.Close();
                public_.Add_Info_MSG(Edt_MSG, string.Format("补打条码{0}成功！", txtSN.Text.Trim()), "OK");
                txtSN.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                LabSN.Documents.CloseAll();
                LabSN.Quit();//退出
                LabSN = null;
                doc = null;
                GC.Collect(0);
            }
        }
    }
}
