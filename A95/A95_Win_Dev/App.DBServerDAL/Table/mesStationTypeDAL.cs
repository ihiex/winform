using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class   mesStationTypeDAL
	{
		public int Insert(mesStationType model)
		{
			object obj = SqlServerHelper.ExecuteScalar(
				"INSERT INTO mesStationType(Description,ApplicationTypeID) VALUES (@Description,@ApplicationTypeID);SELECT @@identity"
				,new SqlParameter("@Description", model.Description)
				,new SqlParameter("@ApplicationTypeID", model.ApplicationTypeID)
			);
			return Convert.ToInt32(obj);
		}

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM mesStationType WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public bool Update(mesStationType model)
		{
			string sql = "UPDATE mesStationType SET Description=@Description,ApplicationTypeID=@ApplicationTypeID WHERE ID=@ID";
			int rows = SqlServerHelper.ExecuteNonQuery(sql
				, new SqlParameter("@ID", model.ID)
				, new SqlParameter("@Description", model.Description)
				, new SqlParameter("@ApplicationTypeID", model.ApplicationTypeID)
			);
			return rows > 0;
		}

		public mesStationType Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesStationType WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			mesStationType model = ToModel(row);
			return model;
		}

		private static mesStationType ToModel(DataRow row)
		{
			mesStationType model = new mesStationType();
			model.ID = (int)row["ID"];
			model.Description = (string)row["Description"];
			model.ApplicationTypeID = (int)row["ApplicationTypeID"];
			return model;
		}

		public IEnumerable<mesStationType> ListAll(string S_Where)
		{
			List<mesStationType> list = new List<mesStationType>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesStationType " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}
	}
}
