using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class   mesSerialNumberDAL
	{
		public int Insert(mesSerialNumber model)
		{
            object obj = SqlServerHelper.ExecuteScalar(
				"INSERT INTO mesSerialNumber(UnitID,SerialNumberTypeID,Value) VALUES (@UnitID,@SerialNumberTypeID,@Value);SELECT @@identity"
				,new SqlParameter("@UnitID", model.UnitID)
				,new SqlParameter("@SerialNumberTypeID", model.SerialNumberTypeID)
				,new SqlParameter("@Value", model.Value)
			);
            string S_Result = obj.ToString();
            if (S_Result == "") { S_Result = "0"; }

            return Convert.ToInt32(S_Result);
        }

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM mesSerialNumber WHERE UnitID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public bool Update(mesSerialNumber model)
		{
			string sql = "UPDATE mesSerialNumber SET UnitID=@UnitID,SerialNumberTypeID=@SerialNumberTypeID,Value=@Value WHERE UnitID=@UnitID";
			int rows = SqlServerHelper.ExecuteNonQuery(sql
				, new SqlParameter("@UnitID", model.UnitID)
				, new SqlParameter("@SerialNumberTypeID", model.SerialNumberTypeID)
				, new SqlParameter("@Value", model.Value)
			);
			return rows > 0;
		}

		public mesSerialNumber Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesSerialNumber WHERE UnitID=@UnitID", new SqlParameter("@UnitID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			mesSerialNumber model = ToModel(row);
			return model;
		}

		private static mesSerialNumber ToModel(DataRow row)
		{
			mesSerialNumber model = new mesSerialNumber();
			model.UnitID = (int)row["UnitID"];
			model.SerialNumberTypeID = (object)row["SerialNumberTypeID"];
			model.Value = (string)row["Value"];
			return model;
		}

		public IEnumerable<mesSerialNumber> ListAll(string S_Where)
		{
			List<mesSerialNumber> list = new List<mesSerialNumber>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesSerialNumber " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}
	}
}
