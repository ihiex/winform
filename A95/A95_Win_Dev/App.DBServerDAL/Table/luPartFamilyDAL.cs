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
	public class   luPartFamilyDAL
	{
		public int Insert(luPartFamily model)
		{
			object obj = SqlServerHelper.ExecuteScalar(
				"INSERT INTO luPartFamily(Name,Description,PartFamilyTypeID,Status) VALUES (@Name,@Description,@PartFamilyTypeID,@Status);SELECT @@identity"
				,new SqlParameter("@Name", model.Name)
				,new SqlParameter("@Description", model.Description)
				,new SqlParameter("@PartFamilyTypeID", model.PartFamilyTypeID)
				,new SqlParameter("@Status", model.Status)
			);
			return Convert.ToInt32(obj);
		}

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM luPartFamily WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public bool Update(luPartFamily model)
		{
			string sql = "UPDATE luPartFamily SET Name=@Name,Description=@Description,PartFamilyTypeID=@PartFamilyTypeID,Status=@Status WHERE ID=@ID";
			int rows = SqlServerHelper.ExecuteNonQuery(sql
				, new SqlParameter("@ID", model.ID)
				, new SqlParameter("@Name", model.Name)
				, new SqlParameter("@Description", model.Description)
				, new SqlParameter("@PartFamilyTypeID", model.PartFamilyTypeID)
				, new SqlParameter("@Status", model.Status)
			);
			return rows > 0;
		}

		public luPartFamily Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM luPartFamily WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			luPartFamily model = ToModel(row);
			return model;
		}

		private static luPartFamily ToModel(DataRow row)
		{
			luPartFamily model = new luPartFamily();
			model.ID = (int)row["ID"];
			model.Name = (string)row["Name"];
			model.Description = (string)row["Description"];
			model.PartFamilyTypeID = (int)row["PartFamilyTypeID"];
			model.Status = (object)row["Status"];
			return model;
		}

		public IEnumerable<luPartFamily> ListAll(string S_Where)
		{
			List<luPartFamily> list = new List<luPartFamily>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM luPartFamily " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}
	}
}
