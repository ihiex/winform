using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class sysStatusDAL
	{
		public int Insert(sysStatus model)
		{
			object obj = SqlServerHelper.ExecuteScalar(
				"INSERT INTO sysStatus(Description) VALUES (@Description);SELECT @@identity"
				,new SqlParameter("@Description", model.Description)
			);
			return Convert.ToInt32(obj);
		}

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM sysStatus WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public bool Update(sysStatus model)
		{
			string sql = "UPDATE sysStatus SET Description=@Description WHERE ID=@ID";
			int rows = SqlServerHelper.ExecuteNonQuery(sql
				, new SqlParameter("@ID", model.ID)
				, new SqlParameter("@Description", model.Description)
			);
			return rows > 0;
		}

		public sysStatus Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM sysStatus WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			sysStatus model = ToModel(row);
			return model;
		}

		private static sysStatus ToModel(DataRow row)
		{
			sysStatus model = new sysStatus();
			model.ID = (object)row["ID"];
			model.Description = (string)row["Description"];
			return model;
		}

		public IEnumerable<sysStatus> ListAll(string S_Where)
		{
			List<sysStatus> list = new List<sysStatus>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM sysStatus  " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}
	}
}
