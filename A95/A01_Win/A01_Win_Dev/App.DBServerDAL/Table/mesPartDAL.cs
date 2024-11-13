using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class   mesPartDAL
	{
		public int Insert(mesPart model)
		{
			object obj = SqlServerHelper.ExecuteScalar(
				"INSERT INTO mesPart(PartNumber,Revision,Description,PartFamilyID,UOM,IsUnit,Status) VALUES (@PartNumber,@Revision,@Description,@PartFamilyID,@UOM,@IsUnit,@Status);SELECT @@identity"
				,new SqlParameter("@PartNumber", model.PartNumber)
				,new SqlParameter("@Revision", model.Revision)
				,new SqlParameter("@Description", model.Description)
				,new SqlParameter("@PartFamilyID", model.PartFamilyID)
				,new SqlParameter("@UOM", model.UOM)
				,new SqlParameter("@IsUnit", model.IsUnit)
				,new SqlParameter("@Status", model.Status)
			);
			return Convert.ToInt32(obj);
		}

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM mesPart WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public bool Update(mesPart model)
		{
			string sql = "UPDATE mesPart SET PartNumber=@PartNumber,Revision=@Revision,Description=@Description,PartFamilyID=@PartFamilyID,UOM=@UOM,IsUnit=@IsUnit,Status=@Status WHERE ID=@ID";
			int rows = SqlServerHelper.ExecuteNonQuery(sql
				, new SqlParameter("@ID", model.ID)
				, new SqlParameter("@PartNumber", model.PartNumber)
				, new SqlParameter("@Revision", model.Revision)
				, new SqlParameter("@Description", model.Description)
				, new SqlParameter("@PartFamilyID", model.PartFamilyID)
				, new SqlParameter("@UOM", model.UOM)
				, new SqlParameter("@IsUnit", model.IsUnit)
				, new SqlParameter("@Status", model.Status)
			);
			return rows > 0;
		}

		public mesPart Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesPart WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			mesPart model = ToModel(row);
			return model;
		}

		private static mesPart ToModel(DataRow row)
		{
			mesPart model = new mesPart();
			model.ID = (int)row["ID"];
			model.PartNumber = row["PartNumber"].ToString();
			model.Revision = row["Revision"].ToString();
			model.Description = row["Description"].ToString();
			model.PartFamilyID = (int)row["PartFamilyID"];
			model.UOM = row["UOM"].ToString();
			model.IsUnit = (bool)row["IsUnit"];
			model.Status = (object)row["Status"];
			return model;
		}

		public IEnumerable<mesPart> ListAll(string S_Where)
		{
			List<mesPart> list = new List<mesPart>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesPart " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}
	}
}
