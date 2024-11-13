using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class   luSerialNumberTypeDAL
	{
		public int Insert(luSerialNumberType model)
		{
			object obj = SqlServerHelper.ExecuteScalar(
				"INSERT INTO luSerialNumberType(Description) VALUES (@Description);SELECT @@identity"
				,new SqlParameter("@Description", model.Description)
			);
            string S_Result = obj.ToString();
            if (S_Result == "") { S_Result = "0"; }

            return Convert.ToInt32(S_Result);
        }

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM luSerialNumberType WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public bool Update(luSerialNumberType model)
		{
			string sql = "UPDATE luSerialNumberType SET Description=@Description WHERE ID=@ID";
			int rows = SqlServerHelper.ExecuteNonQuery(sql
				, new SqlParameter("@ID", model.ID)
				, new SqlParameter("@Description", model.Description)
			);
			return rows > 0;
		}

		public luSerialNumberType Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM luSerialNumberType WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			luSerialNumberType model = ToModel(row);
			return model;
		}

		private static luSerialNumberType ToModel(DataRow row)
		{
			luSerialNumberType model = new luSerialNumberType();
			model.ID = (object)row["ID"];
			model.Description = (string)row["Description"];
			return model;
		}

		public IEnumerable<luSerialNumberType> ListAll(string S_Where)
		{
			List<luSerialNumberType> list = new List<luSerialNumberType>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM luSerialNumberType " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}
	}
}
