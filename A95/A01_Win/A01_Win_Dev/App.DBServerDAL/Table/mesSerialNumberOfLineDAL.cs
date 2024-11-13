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
    public class mesSerialNumberOfLineDAL
    {
        public int Insert(mesSerialNumberOfLine model)
        {
            object obj = SqlServerHelper.ExecuteScalar(
                "INSERT INTO mesSerialNumberOfLine(SerialNumber,SNCategory,PrintCount,FirstPrintTime,LastPrintTime) VALUES " +
                "(@SerialNumber,@SNCategory,@PrintCount,@FirstPrintTime,@LastPrintTime);SELECT @@identity"
                , new SqlParameter("@SerialNumber", model.SerialNumber)
                , new SqlParameter("@SNCategory", model.SNCategory)
                , new SqlParameter("@PrintCount", model.PrintCount)
                , new SqlParameter("@FirstPrintTime", model.FirstPrintTime)
                , new SqlParameter("@LastPrintTime", model.LastPrintTime)
            );

            string S_Result = obj.ToString();
            if (S_Result == "") { S_Result = "0"; }

            return Convert.ToInt32(S_Result);
        }

        public bool Delete(int id)
        {
            int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM mesSerialNumberOfLine WHERE ID = @id", new SqlParameter("@id", id));
            return rows > 0;
        }

        public bool Update(mesSerialNumberOfLine model)
        {
            string sql = "UPDATE mesSerialNumberOfLine SET SerialNumber=@SerialNumber,SNCategory=@SNCategory,PrintCount=@PrintCount," +
                "FirstPrintTime=@FirstPrintTime,LastPrintTime=@LastPrintTime WHERE ID=@ID";
            int rows = SqlServerHelper.ExecuteNonQuery(sql
                , new SqlParameter("@ID", model.ID)
                , new SqlParameter("@SerialNumber", model.SerialNumber)
                , new SqlParameter("@SNCategory", model.SNCategory)
                , new SqlParameter("@PrintCount", model.PrintCount)
                , new SqlParameter("@FirstPrintTime", model.FirstPrintTime)
                , new SqlParameter("@LastPrintTime", model.LastPrintTime)
            );
            return rows > 0;
        }

        public mesSerialNumberOfLine Get(int id)
        {
            DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesSerialNumberOfLine WHERE ID=@ID", new SqlParameter("@ID", id));
            if (dt.Rows.Count > 1)
            {
                throw new Exception("more than 1 row was found");
            }
            if (dt.Rows.Count <= 0)
            {
                return null;
            }
            DataRow row = dt.Rows[0];
            mesSerialNumberOfLine model = ToModel(row);
            return model;
        }

        private static mesSerialNumberOfLine ToModel(DataRow row)
        {
            mesSerialNumberOfLine model = new mesSerialNumberOfLine();
            model.ID = (int)row["ID"];
            model.SerialNumber = (string)row["SerialNumber"];
            model.SNCategory = (string)row["SNCategory"];
            model.PrintCount = (int)row["PrintCount"];
            model.FirstPrintTime = (DateTime)row["FirstPrintTime"];
            model.LastPrintTime = (DateTime)row["LastPrintTime"];
            return model;
        }

        public IEnumerable<mesSerialNumberOfLine> ListAll(string S_Where)
        {
            List<mesSerialNumberOfLine> list = new List<mesSerialNumberOfLine>();
            DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesSerialNumberOfLine " + S_Where);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(ToModel(row));
            }
            return list;
        }
    }
}
