using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class   luPartFamilyDetailDefDAL
	{
		public int Insert(luPartFamilyDetailDef model)
		{
			object obj = SqlServerHelper.ExecuteScalar(
				"INSERT INTO luPartFamilyDetailDef(Description) VALUES (@Description);SELECT @@identity"
				,new SqlParameter("@Description", model.Description)
			);
			return Convert.ToInt32(obj);
		}

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM luPartFamilyDetailDef WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public bool Update(luPartFamilyDetailDef model)
		{
			string sql = "UPDATE luPartFamilyDetailDef SET Description=@Description WHERE ID=@ID";
			int rows = SqlServerHelper.ExecuteNonQuery(sql
				, new SqlParameter("@ID", model.ID)
				, new SqlParameter("@Description", model.Description)
			);
			return rows > 0;
		}

		public luPartFamilyDetailDef Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM luPartFamilyDetailDef WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			luPartFamilyDetailDef model = ToModel(row);
			return model;
		}

		private static luPartFamilyDetailDef ToModel(DataRow row)
		{
			luPartFamilyDetailDef model = new luPartFamilyDetailDef();
			model.ID = (int)row["ID"];
			model.Description = (string)row["Description"];
			return model;
		}

		public IEnumerable<luPartFamilyDetailDef> ListAll(string S_Where)
		{
			List<luPartFamilyDetailDef> list = new List<luPartFamilyDetailDef>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM luPartFamilyDetailDef " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}
	}
}
