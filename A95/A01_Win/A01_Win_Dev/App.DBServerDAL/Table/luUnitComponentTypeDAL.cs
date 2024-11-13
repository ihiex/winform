using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class   luUnitComponentTypeDAL
	{
		public int Insert(luUnitComponentType model)
		{
			object obj = SqlServerHelper.ExecuteScalar(
				"INSERT INTO luUnitComponentType(Description) VALUES (@Description);SELECT @@identity"
				,new SqlParameter("@Description", model.Description)
			);
			return Convert.ToInt32(obj);
		}

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM luUnitComponentType WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public bool Update(luUnitComponentType model)
		{
			string sql = "UPDATE luUnitComponentType SET Description=@Description WHERE ID=@ID";
			int rows = SqlServerHelper.ExecuteNonQuery(sql
				, new SqlParameter("@ID", model.ID)
				, new SqlParameter("@Description", model.Description)
			);
			return rows > 0;
		}

		public luUnitComponentType Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM luUnitComponentType WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			luUnitComponentType model = ToModel(row);
			return model;
		}

		private static luUnitComponentType ToModel(DataRow row)
		{
			luUnitComponentType model = new luUnitComponentType();
			model.ID = (int)row["ID"];
			model.Description = (string)row["Description"];
			return model;
		}

		public IEnumerable<luUnitComponentType> ListAll(string S_Where)
		{
			List<luUnitComponentType> list = new List<luUnitComponentType>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM luUnitComponentType " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}
	}
}
