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
    public class mesUnitDefectDAL
    {
        public string Insert(mesUnitDefect model)
        {
            string S_Result = "";
            try
            {
                string S_Sql = "select Count(ID)+1 MaxID from mesUnitDefect";
                DataTable DT = SqlServerHelper.ExecuteDataTable(S_Sql);
                int I_MaxID = Convert.ToInt32(DT.Rows[0][0].ToString());
                model.ID = I_MaxID;

                object obj = SqlServerHelper.ExecuteScalar(
                    "INSERT INTO mesUnitDefect(ID,UnitID,DefectID,StationID,EmployeeID)" +
                    "VALUES (@ID,@UnitID,@DefectID,@StationID,@EmployeeID)" +
                    ";SELECT @@identity"

                    , new SqlParameter("@ID", model.ID)
                    , new SqlParameter("@UnitID", model.UnitID)
                    , new SqlParameter("@DefectID", model.DefectID)
                    , new SqlParameter("@StationID", model.StationID)
                    , new SqlParameter("@EmployeeID", model.EmployeeID)
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
            int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM mesUnitDefect WHERE ID = @id", new SqlParameter("@id", id));
            return rows > 0;
        }

        public string Update(mesUnitDefect model)
        {
            string S_Result = "";
            try
            {
                string sql = "UPDATE mesUnitDefect SET " +
                    "UnitID=@UnitID," +
                    "DefectID=@DefectID," +
                    "StationID=@StationID," +
                    "EmployeeID=@EmployeeID" +

                    "WHERE ID=@ID";
                int rows = SqlServerHelper.ExecuteNonQuery(sql
                    , new SqlParameter("@ID", model.ID)
                    , new SqlParameter("@UnitID", model.UnitID)
                    , new SqlParameter("@DefectID", model.DefectID)
                    , new SqlParameter("@StationID", model.StationID)
                    , new SqlParameter("@EmployeeID", model.EmployeeID)
                );
                S_Result = "OK";
            }
            catch (Exception ex)
            {
                S_Result = "E:" + ex.Message;
            }
            return S_Result;
        }

        public mesUnitDefect Get(int id)
        {
            DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesUnitDefect WHERE ID=@ID", new SqlParameter("@ID", id));
            if (dt.Rows.Count > 1)
            {
                throw new Exception("more than 1 row was found");
            }
            if (dt.Rows.Count <= 0)
            {
                return null;
            }
            DataRow row = dt.Rows[0];
            mesUnitDefect model = ToModel(row);
            return model;
        }

        private static mesUnitDefect ToModel(DataRow row)
        {
            mesUnitDefect model = new mesUnitDefect();
            model.ID = (int)row["ID"];
            model.UnitID = (int)row["UnitID"];
            model.DefectID = (int)row["DefectID"];
            model.StationID = (int)row["StationID"];
            model.EmployeeID = (int)row["EmployeeID"];
            model.CreationTime = (DateTime)row["CreationTime"];

            return model;
        }

        public IEnumerable<mesUnitDefect> ListAll(string S_Where)
        {
            List<mesUnitDefect> list = new List<mesUnitDefect>();
            DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesUnitDefect  " + S_Where);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(ToModel(row));
            }
            return list;
        }
    }
}