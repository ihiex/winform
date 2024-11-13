using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;
using App.DBUtility;

namespace App.DBServerDAL
{
    public class luDefectDAL
    {
        public string Insert(luDefect model)
        {
            string S_Result = "";
            try
            {
                //string S_Sql = "select Count(ID)+1 MaxID from mesUnitDefect";
                //DataTable DT = SqlServerHelper.ExecuteDataTable(S_Sql);
                //int I_MaxID = Convert.ToInt32(DT.Rows[0][0].ToString());
                //model.ID = I_MaxID;

                object obj = SqlServerHelper.ExecuteScalar(
                    "INSERT INTO luDefect(DefectTypeID,DefectCode,Description,LocaltionID,Status)" +
                    "VALUES (@DefectTypeID,@DefectCode,@Description,@LocaltionID,@Status)" +
                    ";SELECT @@identity"

                    , new SqlParameter("@DefectTypeID", model.DefectTypeID)
                    , new SqlParameter("@DefectCode", model.DefectCode)
                    , new SqlParameter("@Description", model.Description)
                    , new SqlParameter("@LocaltionID", model.LocaltionID)
                    , new SqlParameter("@Status", model.Status)
                );

                S_Result = obj.ToString();
                if (S_Result == "") { S_Result = "0"; }
            }
            catch (Exception ex)
            {
                S_Result = "E:" + ex.Message;
            }
            return S_Result;
        }

        public bool Delete(int id)
        {
            int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM luDefect WHERE ID = @id", new SqlParameter("@id", id));
            return rows > 0;
        }

        public string Update(luDefect model)
        {
            string S_Result = "";
            try
            {
                string sql = "UPDATE luDefect SET " +
                    "DefectTypeID=@DefectTypeID," +
                    "DefectCode=@DefectCode," +
                    "Description=@Description," +
                    "LocaltionID=@LocaltionID" +
                    "Status=@Status" +

                    "WHERE ID=@ID";
                int rows = SqlServerHelper.ExecuteNonQuery(sql
                    , new SqlParameter("@ID", model.ID)
                    , new SqlParameter("@DefectTypeID", model.DefectTypeID)
                    , new SqlParameter("@DefectCode", model.DefectCode)
                    , new SqlParameter("@Description", model.Description)
                    , new SqlParameter("@LocaltionID", model.LocaltionID)
                    , new SqlParameter("@Status", model.Status)
                );
                S_Result = "OK";
            }
            catch (Exception ex)
            {
                S_Result = "E:" + ex.Message;
            }
            return S_Result;
        }

        public luDefect Get(int id)
        {
            DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM luDefect WHERE ID=@ID", new SqlParameter("@ID", id));
            if (dt.Rows.Count > 1)
            {
                throw new Exception("more than 1 row was found");
            }
            if (dt.Rows.Count <= 0)
            {
                return null;
            }
            DataRow row = dt.Rows[0];
            luDefect model = ToModel(row);
            return model;
        }

        private static luDefect ToModel(DataRow row)
        {
            luDefect model = new luDefect();
            model.ID = (int)row["ID"];
            model.DefectTypeID = (int)row["DefectTypeID"];
            model.DefectCode = (string)row["DefectCode"];
            model.Description = (string)row["Description"];
            model.LocaltionID = (int)row["LocaltionID"];
            model.Status = (int)row["Status"];

            return model;
        }

        public IEnumerable<luDefect> ListAll(string S_Where)
        {
            List<luDefect> list = new List<luDefect>();
            DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM luDefect  " + S_Where);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(ToModel(row));
            }
            return list;
        }
    }
}
