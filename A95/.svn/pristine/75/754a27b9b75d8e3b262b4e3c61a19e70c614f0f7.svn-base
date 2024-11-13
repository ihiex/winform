using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class   mesRouteMapDAL
	{
		public int Insert(mesRouteMap model)
		{
			object obj = SqlServerHelper.ExecuteScalar(
				"INSERT INTO mesRouteMap(PartFamilyID,PartID,LineID,RouteID) VALUES (@PartFamilyID,@PartID,@LineID,@RouteID);SELECT @@identity"
				,new SqlParameter("@PartFamilyID", model.PartFamilyID)
				,new SqlParameter("@PartID", model.PartID)
				,new SqlParameter("@LineID", model.LineID)
				,new SqlParameter("@RouteID", model.RouteID)
			);
			return Convert.ToInt32(obj);
		}

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM mesRouteMap WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public bool Update(mesRouteMap model)
		{
			string sql = "UPDATE mesRouteMap SET PartFamilyID=@PartFamilyID,PartID=@PartID,LineID=@LineID,RouteID=@RouteID WHERE ID=@ID";
			int rows = SqlServerHelper.ExecuteNonQuery(sql
				, new SqlParameter("@ID", model.ID)
				, new SqlParameter("@PartFamilyID", model.PartFamilyID)
				, new SqlParameter("@PartID", model.PartID)
				, new SqlParameter("@LineID", model.LineID)
				, new SqlParameter("@RouteID", model.RouteID)
			);
			return rows > 0;
		}

        public DataSet MesGetPartIDByMachineSN(int stationTypeID,string MachineSN)
        {
            string strSql = string.Format(@"SELECT PartID FROM mesRouteMachineMap A WHERE A.StationTypeID={0}
                                                AND EXISTS(SELECT 1 FROM mesMachine B WHERE A.MachineFamilyID=B.MachineFamilyID AND SN='{1}')",
                                                stationTypeID, MachineSN);
            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }

        public mesRouteMap Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesRouteMap WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			mesRouteMap model = ToModel(row);
			return model;
		}

		private static mesRouteMap ToModel(DataRow row)
		{
			mesRouteMap model = new mesRouteMap();
			model.ID = (int)row["ID"];
			model.PartFamilyID = (int)row["PartFamilyID"];
			model.PartID = (int)row["PartID"];
			model.LineID = (int)row["LineID"];
			model.RouteID = (int)row["RouteID"];
			return model;
		}

		public IEnumerable<mesRouteMap> ListAll(string S_Where)
		{
			List<mesRouteMap> list = new List<mesRouteMap>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesRouteMap " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}
	}
}
