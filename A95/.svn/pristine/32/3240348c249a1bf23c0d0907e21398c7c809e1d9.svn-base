using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class   mesStationDAL
	{
		public int Insert(mesStation model)
		{
			object obj = SqlServerHelper.ExecuteScalar(
				"INSERT INTO mesStation(Description,StationTypeID,LineID,Status) VALUES (@Description,@StationTypeID,@LineID,@Status);SELECT @@identity"
				,new SqlParameter("@Description", model.Description)
				,new SqlParameter("@StationTypeID", model.StationTypeID)
				,new SqlParameter("@LineID", model.LineID)
				,new SqlParameter("@Status", model.Status)
			);
			return Convert.ToInt32(obj);
		}

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM mesStation WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public bool Update(mesStation model)
		{
			string sql = "UPDATE mesStation SET Description=@Description,StationTypeID=@StationTypeID,LineID=@LineID,Status=@Status WHERE ID=@ID";
			int rows = SqlServerHelper.ExecuteNonQuery(sql
				, new SqlParameter("@ID", model.ID)
				, new SqlParameter("@Description", model.Description)
				, new SqlParameter("@StationTypeID", model.StationTypeID)
				, new SqlParameter("@LineID", model.LineID)
				, new SqlParameter("@Status", model.Status)
			);
			return rows > 0;
		}

		public mesStation Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesStation WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			mesStation model = ToModel(row);
			return model;
		}

		private static mesStation ToModel(DataRow row)
		{
			mesStation model = new mesStation();
			model.ID = (int)row["ID"];
			model.Description = (string)row["Description"];
			model.StationTypeID = (int)row["StationTypeID"];
			model.LineID = (int)row["LineID"];
			model.Status = (object)row["Status"];
			return model;
		}

		public IEnumerable<mesStation> ListAll(string S_Where)
		{
			List<mesStation> list = new List<mesStation>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesStation " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}
	}
}
