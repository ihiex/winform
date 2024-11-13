using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Model;
using App.DBUtility;
using App.MyMES;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace App.DBServerDAL
{
    public class SiemensDB
    {
        string S_Conn = "";
        string S_StationTypeID = "";
        public string ProjectType="";
        public SiemensDB(string StationTypeID)
        {
            S_StationTypeID = StationTypeID;

            //if (S_StationTypeID == "Ipad")
            //{
            //    S_Conn = "Data Source=192.168.100.110;Initial Catalog =execution_v10;Persist Security Info=True;User ID=SFC;Password=Cosmo@123";
            //}
            //else
            //{
            //    S_Conn = Conn();
            //}


            S_Conn = SqlServerHelper.default_connection_str;

            string S_Sql =
            @"
                    select *  from mesStationTypeDetail A 
                         join luStationTypeDetailDef B on A.StationTypeDetailDefID=B.ID
                    where A.StationTypeID=" + S_StationTypeID + @" and B.Description='SiemensDBConn'  
                ";
            DataTable DT_SiemensDBConn = SqlServerHelper.Data_Table(S_Sql);
            if (DT_SiemensDBConn.Rows.Count > 0)
            {
                S_Conn = DT_SiemensDBConn.Rows[0]["Content"].ToString();
            }
            ProjectType = GetProjectType();
        }

        private string GetProjectType()
        {
            string S_Reult = "";

            if (S_StationTypeID == "")
            {
                S_Reult = "";
            }
            else
            {               
                string S_Sql =
                @"
                    select *  from mesStationTypeDetail A 
                         join luStationTypeDetailDef B on A.StationTypeDetailDefID=B.ID
                    where A.StationTypeID=" + S_StationTypeID + @" and B.Description='SiemensProjectType'  
                ";
                DataTable DT_SiemensDBName = SqlServerHelper.Data_Table(S_Sql);
                if (DT_SiemensDBName.Rows.Count == 0)
                {
                    S_Reult = "";
                }
                else
                {
                    S_Reult = DT_SiemensDBName.Rows[0]["Content"].ToString();                    
                }
            }
            return S_Reult;
        }

        //private string Conn()
        //{
        //    string S_Reult = "";
        //    string S_MES_Conn = SqlServerHelper.default_connection_str;
        //    string[] List_MES_Conn = S_MES_Conn.Split(';');
        //    //string[] List_Conn = List_MES_Conn[1].Split('=');

        //    //string S_Path = this.S_StationTypeID; //Application.StartupPath;
        //    //MyINI myINI = new MyINI(S_Path + "\\MES_Main.ini");
        //    //string S_StationTypeID = myINI.ReadValue("StationTypeID", "Value");

        //    if (S_StationTypeID == "")
        //    {
        //        S_Reult = "";
        //    }
        //    else
        //    {                
        //        string DBName = "";
        //        string S_Sql =
        //        @"
        //        select *  from mesStationTypeDetail A 
        //                join luStationTypeDetailDef B on A.StationTypeDetailDefID=B.ID
        //        where A.StationTypeID=" + S_StationTypeID + @" and B.Description='SiemensDBName'  
        //    ";
        //        DataTable DT_SiemensDBName = SqlServerHelper.Data_Table(S_Sql);
        //        if (DT_SiemensDBName.Rows.Count == 0)
        //        {
        //            S_Reult = "";
        //        }
        //        else
        //        {
        //            DBName = DT_SiemensDBName.Rows[0]["Content"].ToString();
        //            S_Reult = List_MES_Conn[0] + ";Initial Catalog =" + DBName + ";" + List_MES_Conn[2] + ";" + List_MES_Conn[3] + ";" + List_MES_Conn[4];
        //        }                
        //    }
        //    return S_Reult;
        //}


        public  DataSet Data_Set(string S_Sql)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection Conn = new SqlConnection(S_Conn);
                Conn.Open();
                SqlCommand cmd = new SqlCommand(S_Sql, Conn);
                SqlDataAdapter Sql_DA = new SqlDataAdapter(cmd);
                Sql_DA.Fill(ds);
                Conn.Close();
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return ds;
        }

        public  DataTable Data_Table(string S_Sql)
        {
            DataTable DT = new DataTable();
            try
            {
                SqlConnection Conn = new SqlConnection(S_Conn);
                Conn.Open();
                SqlCommand cmd = new SqlCommand(S_Sql, Conn);
                SqlDataAdapter Sql_DA = new SqlDataAdapter(cmd);
                Sql_DA.Fill(DT);
                Sql_DA.Dispose();
                Conn.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return DT;
        }

        public string ExecSql(string S_Sql)
        {
            string S_Result = "OK";
            try
            {
                SqlConnection Conn = new SqlConnection(S_Conn);
                Conn.Open();
                SqlCommand cmd = new SqlCommand(S_Sql, Conn);
                cmd.ExecuteNonQuery();
                Conn.Close();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                S_Result = ex.Message;
            }
            return S_Result;
        }

        public static DataSet ExecuteNonQueryPro(string S_Conn, string ProName, params SqlParameter[] commandParameters)
        {
            DataSet daset = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(S_Conn))
                {
                    con.Open();
                    SqlCommand com = new SqlCommand(ProName, con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddRange(commandParameters);
                    SqlDataAdapter sqlData = new SqlDataAdapter();
                    sqlData.SelectCommand = com;
                    sqlData.Fill(daset);
                    return daset;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
