using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class   luEmployeeGroupDAL
	{
		public int Insert(luEmployeeGroup model)
		{
			object obj = SqlServerHelper.ExecuteScalar(
				"INSERT INTO luEmployeeGroup(Description) VALUES (@Description);SELECT @@identity"
				,new SqlParameter("@Description", model.Description)
			);
			return Convert.ToInt32(obj);
		}

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM luEmployeeGroup WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public bool Update(luEmployeeGroup model)
		{
			string sql = "UPDATE luEmployeeGroup SET Description=@Description WHERE ID=@ID";
			int rows = SqlServerHelper.ExecuteNonQuery(sql
				, new SqlParameter("@ID", model.ID)
				, new SqlParameter("@Description", model.Description)
			);
			return rows > 0;
		}

		public luEmployeeGroup Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM luEmployeeGroup WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			luEmployeeGroup model = ToModel(row);
			return model;
		}

		private static luEmployeeGroup ToModel(DataRow row)
		{
			luEmployeeGroup model = new luEmployeeGroup();
			model.ID = (int)row["ID"];
			model.Description = (string)row["Description"];
			return model;
		}

		public IEnumerable<luEmployeeGroup> ListAll(string S_Where)
		{
			List<luEmployeeGroup> list = new List<luEmployeeGroup>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM luEmployeeGroup  " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}
	}
}
