using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class   mesRouteMachineMapDAL
	{
		public int Insert(mesRouteMachineMap model)
		{
			object obj = SqlServerHelper.ExecuteScalar(
				"INSERT INTO mesRouteMachineMap(RouteID,RouteDetailID,MachineID) VALUES (@RouteID,@RouteDetailID,@MachineID);SELECT @@identity"
				,new SqlParameter("@RouteID", model.RouteID)
				,new SqlParameter("@RouteDetailID", model.RouteDetailID)
				,new SqlParameter("@MachineID", model.MachineID)
			);
			return Convert.ToInt32(obj);
		}

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM mesRouteMachineMap WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public bool Update(mesRouteMachineMap model)
		{
			string sql = "UPDATE mesRouteMachineMap SET RouteID=@RouteID,RouteDetailID=@RouteDetailID,MachineID=@MachineID WHERE ID=@ID";
			int rows = SqlServerHelper.ExecuteNonQuery(sql
				, new SqlParameter("@ID", model.ID)
				, new SqlParameter("@RouteID", model.RouteID)
				, new SqlParameter("@RouteDetailID", model.RouteDetailID)
				, new SqlParameter("@MachineID", model.MachineID)
			);
			return rows > 0;
		}

		public mesRouteMachineMap Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesRouteMachineMap WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			mesRouteMachineMap model = ToModel(row);
			return model;
		}

		private static mesRouteMachineMap ToModel(DataRow row)
		{
			mesRouteMachineMap model = new mesRouteMachineMap();
			model.ID = (int)row["ID"];
			model.RouteID = (string)row["RouteID"];
			model.RouteDetailID = (int)row["RouteDetailID"];
			model.MachineID = (int)row["MachineID"];
			return model;
		}

		public IEnumerable<mesRouteMachineMap> ListAll(string S_Where)
		{
			List<mesRouteMachineMap> list = new List<mesRouteMachineMap>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesRouteMachineMap " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}
	}
}
