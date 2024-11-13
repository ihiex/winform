using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class   luMachineFamilyTypeDAL
	{
		public int Insert(luMachineFamilyType model)
		{
			object obj = SqlServerHelper.ExecuteScalar(
				"INSERT INTO luMachineFamilyType(Name,Description,Status) VALUES (@Name,@Description,@Status);SELECT @@identity"
				,new SqlParameter("@Name", model.Name)
				,new SqlParameter("@Description", model.Description)
				,new SqlParameter("@Status", model.Status)
			);
			return Convert.ToInt32(obj);
		}

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM luMachineFamilyType WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public bool Update(luMachineFamilyType model)
		{
			string sql = "UPDATE luMachineFamilyType SET Name=@Name,Description=@Description,Status=@Status WHERE ID=@ID";
			int rows = SqlServerHelper.ExecuteNonQuery(sql
				, new SqlParameter("@ID", model.ID)
				, new SqlParameter("@Name", model.Name)
				, new SqlParameter("@Description", model.Description)
				, new SqlParameter("@Status", model.Status)
			);
			return rows > 0;
		}

		public luMachineFamilyType Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM luMachineFamilyType WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			luMachineFamilyType model = ToModel(row);
			return model;
		}

		private static luMachineFamilyType ToModel(DataRow row)
		{
			luMachineFamilyType model = new luMachineFamilyType();
			model.ID = (int)row["ID"];
			model.Name = (string)row["Name"];
			model.Description = (string)row["Description"];
			model.Status = (object)row["Status"];
			return model;
		}

		public IEnumerable<luMachineFamilyType> ListAll(string S_Where)
		{
			List<luMachineFamilyType> list = new List<luMachineFamilyType>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM luMachineFamilyType " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}
	}
}
