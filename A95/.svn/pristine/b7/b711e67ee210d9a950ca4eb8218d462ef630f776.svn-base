using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class   mesRouteDetailDAL
	{
		public int Insert(mesRouteDetail model)
		{
			object obj = SqlServerHelper.ExecuteScalar(
				"INSERT INTO mesRouteDetail(RouteID,StationTypeID,Sequence,Description) VALUES (@RouteID,@StationTypeID,@Sequence,@Description);SELECT @@identity"
				,new SqlParameter("@RouteID", model.RouteID)
				,new SqlParameter("@StationTypeID", model.StationTypeID)
				,new SqlParameter("@Sequence", model.Sequence)
				,new SqlParameter("@Description", model.Description)
			);
			return Convert.ToInt32(obj);
		}

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM mesRouteDetail WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public bool Update(mesRouteDetail model)
		{
			string sql = "UPDATE mesRouteDetail SET RouteID=@RouteID,StationTypeID=@StationTypeID,Sequence=@Sequence,Description=@Description WHERE ID=@ID";
			int rows = SqlServerHelper.ExecuteNonQuery(sql
				, new SqlParameter("@ID", model.ID)
				, new SqlParameter("@RouteID", model.RouteID)
				, new SqlParameter("@StationTypeID", model.StationTypeID)
				, new SqlParameter("@Sequence", model.Sequence)
				, new SqlParameter("@Description", model.Description)
			);
			return rows > 0;
		}

		public mesRouteDetail Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesRouteDetail WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			mesRouteDetail model = ToModel(row);
			return model;
		}

		private static mesRouteDetail ToModel(DataRow row)
		{
			mesRouteDetail model = new mesRouteDetail();
			model.ID = (int)row["ID"];
			model.RouteID = (int)row["RouteID"];
			model.StationTypeID = (int)row["StationTypeID"];
			model.Sequence = (int)row["Sequence"];
			model.Description = (string)row["Description"];
			return model;
		}

		public IEnumerable<mesRouteDetail> ListAll(string S_Where)
		{
			List<mesRouteDetail> list = new List<mesRouteDetail>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesRouteDetail " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}
	}
}
