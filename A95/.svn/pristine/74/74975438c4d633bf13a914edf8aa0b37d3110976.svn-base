using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class   mesRouteDAL
	{
		public int Insert(mesRoute model)
		{
			object obj = SqlServerHelper.ExecuteScalar(
				"INSERT INTO mesRoute(Name,Description,XMLRoute) VALUES (@Name,@Description,@XMLRoute);SELECT @@identity"
				,new SqlParameter("@Name", model.Name)
				,new SqlParameter("@Description", model.Description)
				,new SqlParameter("@XMLRoute", model.XMLRoute)
			);
			return Convert.ToInt32(obj);
		}

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM mesRoute WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public bool Update(mesRoute model)
		{
			string sql = "UPDATE mesRoute SET Name=@Name,Description=@Description,XMLRoute=@XMLRoute WHERE ID=@ID";
			int rows = SqlServerHelper.ExecuteNonQuery(sql
				, new SqlParameter("@ID", model.ID)
				, new SqlParameter("@Name", model.Name)
				, new SqlParameter("@Description", model.Description)
				, new SqlParameter("@XMLRoute", model.XMLRoute)
			);
			return rows > 0;
		}

		public mesRoute Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesRoute WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			mesRoute model = ToModel(row);
			return model;
		}

		private static mesRoute ToModel(DataRow row)
		{
			mesRoute model = new mesRoute();
			model.ID = (int)row["ID"];
			model.Name = (string)row["Name"];
			model.Description = (string)row["Description"];
			model.XMLRoute = (object)row["XMLRoute"];
			return model;
		}

		public IEnumerable<mesRoute> ListAll(string S_Where)
		{
			List<mesRoute> list = new List<mesRoute>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesRoute " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}

        public int MESGetRounterID(int LineID,int PartID, int PartFamilyID)
        {
            int routeID = 0;
            string S_Sql = string.Format("SELECT DBO.ufnRTEGetRouteID({0},{1},{2})", LineID, PartID, PartFamilyID);
            DataTable dtRounterID = SqlServerHelper.ExecuteDataTable(S_Sql);
            if (dtRounterID != null && dtRounterID.Rows.Count > 0)
            {
                routeID = Convert.ToInt32(dtRounterID.Rows[0][0].ToString());
            }
            return routeID;
        }

        public int MESGetRouteSequence(int uintID,int routeID)
        {
            int Sequence = 0;
            string S_Sql = string.Format(@"	SELECT A.Sequence FROM mesRouteDetail A 
	                                        INNER JOIN mesUnit C  ON A.UnitStateID=C.UnitStateID 
	                                        WHERE C.ID={0} AND A.RouteID={1}", uintID, routeID);
            DataTable dtSequence = SqlServerHelper.ExecuteDataTable(S_Sql);
            if (dtSequence != null && dtSequence.Rows.Count > 0)
            {
                Sequence = Convert.ToInt32(dtSequence.Rows[0][0].ToString());
            }
            return Sequence;
        }

        public bool MESCheckLastStation(int UintID)
        {
            string Str_Sql= string.Format(@"SELECT A.LineID,A.PartID,B.PartFamilyID FROM mesUnit A 
	                                            INNER JOIN mesPart B ON A.PartID=B.ID WHERE A.ID={0}", UintID);
            DataTable dtUnt = SqlServerHelper.ExecuteDataTable(Str_Sql);

            int LineID = Convert.ToInt32(dtUnt.Rows[0]["LineID"]);
            int partID = Convert.ToInt32(dtUnt.Rows[0]["partID"]);
            int PartFamilyID = Convert.ToInt32(dtUnt.Rows[0]["PartFamilyID"]);
            int RouteID = MESGetRounterID(LineID, partID, PartFamilyID);

            int sequence = MESGetRouteSequence(UintID, RouteID);
            mesRouteDetailDAL mesRouteDetail = new mesRouteDetailDAL();
            int RouteNumber = mesRouteDetail.ListAll(" WHERE RouteID="+RouteID.ToString()).Count();
            if(sequence!= RouteNumber)
            {
                return false;
            }
            return true;
        }
    }
}
