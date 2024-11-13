using App.Model;
using App.MyMES.SimensService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.MyMES.Frm_WH_Old
{
    public partial class Import_Form : Form
    {
        public Import_Form()
        {
            InitializeComponent();
        }

        public delegate void De_UpdateMain();
        De_UpdateMain F_De_UpdateMain;
        SiemensSVCClient F_SiemensSVC;
        LoginList F_List_Login;
        //string F_StationTypeID;

        public void Show_Import_Form(Import_Form v_Import_Form,
            SiemensSVCClient SiemensSVC, LoginList List_Login,De_UpdateMain v_UpdateMain)
        {
            //F_StationTypeID = S_StationTypeID;
            F_SiemensSVC = SiemensSVC;
            F_List_Login = List_Login;
            F_De_UpdateMain = v_UpdateMain;
            v_Import_Form.ShowDialog();
        }

        private void Import_Form_Load(object sender, EventArgs e)
        {
            //List_Login = this.Tag as LoginList;
        }

        private void Btn_Excel2003_Click(object sender, EventArgs e)
        {
            if (Open_2003.ShowDialog() == DialogResult.OK)
            {
                string S_FileName = Open_2003.FileName;
                DataTable DT = GetExcelDatatable(S_FileName, "Sheet1", "2003");

                OpenExcel(DT);
            }
        }

        private void Btn_Excel_Click(object sender, EventArgs e)
        {
            if (Open_Excel.ShowDialog() == DialogResult.OK)
            {
                string S_FileName = Open_Excel.FileName;
                DataTable DT = GetExcelDatatable(S_FileName, "Sheet1", "");

                OpenExcel(DT);
            }

        }

        private void OpenExcel(DataTable DT)
        {
            string S_Sql = "Drop Table tmpExcelShipment ";
            F_SiemensSVC.ExecSql(S_Sql, F_List_Login.StationTypeID.ToString());
            Thread.Sleep(1000); 

            S_Sql =
            @"
            CREATE TABLE [dbo].[tmpExcelShipment](
	            [Ship date] [datetime] NULL,
	            [project] [nvarchar](255) NULL,
	            [HAWB#] [nvarchar](255) NULL,
	            [HUB CODE] [nvarchar](255) NULL,
	            [COUNTRY] [nvarchar](255) NULL,
	            [REGION] [nvarchar](255) NULL,
	            [KPO#] [nvarchar](255) NULL,
	            [MPN] [nvarchar](255) NULL,
	            [Q'ty] [nvarchar](255) NULL,
	            [Carton] [nvarchar](255) NULL,
	            [Pallet] [nvarchar](255) NULL,
	            [Line] [nvarchar](255) NULL,
	            [CarNO] [nvarchar](255) NULL,
	            [备注] [nvarchar](255) NULL,
	            [memo] [varchar](1000) NULL,
	            [import] [bit] NULL,
	            [NO.] [int] IDENTITY(1,1) NOT NULL
            ) ON [PRIMARY]
            ";
            F_SiemensSVC.ExecSql(S_Sql, F_List_Login.StationTypeID.ToString());

            Grid_Main.DataSource = DT;
            string S_Result = "";

            if (DT != null && DT.Rows.Count > 0)
            {
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    try
                    {
                        S_Sql = "insert into tmpExcelShipment([Ship date],[project],[HAWB#],[HUB CODE] ," +
                            "[COUNTRY],[REGION],[KPO#] ,[MPN],[Q'ty],[Carton],[Pallet],[Line],[CarNO],[备注]) " +
                            " Values(" +
                             "'" + DT.Rows[i]["Ship date"].ToString() + "'," +
                             "'" + DT.Rows[i]["project"].ToString() + "'," +
                             "'" + DT.Rows[i]["HAWB#"].ToString() + "'," +
                             "'" + DT.Rows[i]["HUB CODE"].ToString() + "'," +
                             "'" + DT.Rows[i]["COUNTRY"].ToString() + "'," +
                             "'" + DT.Rows[i]["REGION"].ToString() + "'," +
                             "'" + DT.Rows[i]["KPO#"].ToString() + "'," +
                             "'" + DT.Rows[i]["MPN"].ToString() + "'," +
                             "'" + DT.Rows[i]["Q'ty"].ToString() + "'," +
                             "'" + DT.Rows[i]["Carton"].ToString() + "'," +
                             "'" + DT.Rows[i]["Pallet"].ToString() + "'," +
                             "'" + DT.Rows[i]["Line"].ToString() + "'," +
                             "'" + DT.Rows[i]["CarNO"].ToString() + "'," +
                             "'" + DT.Rows[i]["备注"].ToString() +
                             "')";
                        S_Result = F_SiemensSVC.ExecSql(S_Sql, F_List_Login.StationTypeID.ToString());
                        if (S_Result != "OK")
                        {
                            MessageBox.Show(S_Result, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        S_Result = "NG";
                        MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void Btn_Check_Click(object sender, EventArgs e)
        {
            try
            {
                string S_Sql = "update tmpExcelShipment set import=1,memo=''";
                string S_Result = F_SiemensSVC.ExecSql(S_Sql, F_List_Login.StationTypeID.ToString());
                if (S_Result != "OK")
                {
                    MessageBox.Show(S_Result, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                S_Sql = "update A set memo=isnull(memo,'')+'1.该HAWB已存在',import=0 from tmpExcelShipment A " +
                             " where exists(select FInterID from CO_WH_Shipment where A.[HAWB#]=HAWB)";
                 S_Result = F_SiemensSVC.ExecSql(S_Sql, F_List_Login.StationTypeID.ToString());
                if (S_Result != "OK")
                {
                    MessageBox.Show(S_Result, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                S_Sql = "update A set memo=isnull(memo,'')+'1.非同一个项目',import=0 from tmpExcelShipment A " +
                       " where exists(select count(distinct Project) from tmpExcelShipment group by Project having count(distinct Project)>1)";
                S_Result = F_SiemensSVC.ExecSql(S_Sql, F_List_Login.StationTypeID.ToString());
                if (S_Result != "OK")
                {
                    MessageBox.Show(S_Result, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                S_Sql = "select * from tmpExcelShipment";
                DataSet DS = F_SiemensSVC.Data_Set(S_Sql, F_List_Login.StationTypeID.ToString());
                Grid_Main.DataSource = DS.Tables[0]; 

                MessageBox.Show("检查完毕！", "MSG", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_Import_Click(object sender, EventArgs e)
        {
            try
            {
                string S_Sql = "exec PRO_WH_ImportShipment '0','0','0','0'";
                string S_Result = F_SiemensSVC.ExecSql(S_Sql, F_List_Login.StationTypeID.ToString());
                if (S_Result == "OK")
                {
                    MessageBox.Show("导入成功！", "MSG", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    F_De_UpdateMain();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(S_Result, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public DataTable GetExcelDatatable(string fileUrl, string table,string Type)
        {
            //office2007之前 仅支持.xls
            //const string cmdText = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;IMEX=1';";
            //支持.xls和.xlsx，即包括office2010等版本的   HDR=Yes代表第一行是标题，不是数据；
            string cmdText = "Provider=Microsoft.Ace.OleDb.12.0;Data Source={0};Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";

            if (Type == "2003")
            {
                cmdText = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;IMEX=1';";
            }

            DataTable dt = null;
            //建立连接
            OleDbConnection conn = new OleDbConnection(string.Format(cmdText, fileUrl));
            try
            {
                //打开连接
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                System.Data.DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                //获取Excel的第一个Sheet名称
                string sheetName = schemaTable.Rows[0]["TABLE_NAME"].ToString().Trim();

                //查询sheet中的数据
                string strSql = "select * from [" + sheetName + "]";
                OleDbDataAdapter da = new OleDbDataAdapter(strSql, conn);
                DataSet ds = new DataSet();
                da.Fill(ds, table);
                dt = ds.Tables[0];

                return dt;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString() , "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

    }
}
