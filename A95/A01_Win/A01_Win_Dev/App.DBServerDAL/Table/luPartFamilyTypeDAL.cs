using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class   luPartFamilyTypeDAL
	{
		public int Insert(luPartFamilyType model)
		{
			object obj = SqlServerHelper.ExecuteScalar(
				"INSERT INTO luPartFamilyType(Name,Description,Status) VALUES (@Name,@Description,@Status);SELECT @@identity"
				,new SqlParameter("@Name", model.Name)
				,new SqlParameter("@Description", model.Description)
				,new SqlParameter("@Status", model.Status)
			);
			return Convert.ToInt32(obj);
		}

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM luPartFamilyType WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public bool Update(luPartFamilyType model)
		{
			string sql = "UPDATE luPartFamilyType SET Name=@Name,Description=@Description,Status=@Status WHERE ID=@ID";
			int rows = SqlServerHelper.ExecuteNonQuery(sql
				, new SqlParameter("@ID", model.ID)
				, new SqlParameter("@Name", model.Name)
				, new SqlParameter("@Description", model.Description)
				, new SqlParameter("@Status", model.Status)
			);
			return rows > 0;
		}

		public luPartFamilyType Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM luPartFamilyType WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			luPartFamilyType model = ToModel(row);
			return model;
		}

		private static luPartFamilyType ToModel(DataRow row)
		{
			luPartFamilyType model = new luPartFamilyType();
			model.ID = (int)row["ID"];
			model.Name = (string)row["Name"];
			model.Description = (string)row["Description"];
			model.Status = (object)row["Status"];
			return model;
		}

		public IEnumerable<luPartFamilyType> ListAll(string S_Where)
		{
			List<luPartFamilyType> list = new List<luPartFamilyType>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM luPartFamilyType " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}
	}
}
