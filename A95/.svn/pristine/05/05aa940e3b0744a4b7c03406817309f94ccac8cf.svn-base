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
        PartSelectDAL PartSelect = new PartSelectDAL();
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


        public DataSet GetRouteDetail(string LineID,string PartID,string PartFamilyID,string ProductionOrderID)
        {            
            DataTable DT = PartSelect.GetRouteData(LineID, PartID, PartFamilyID, ProductionOrderID).Tables[0];   
            string S_RouteType = DT.Rows[0]["RouteType"].ToString();
            string S_RouteID= DT.Rows[0]["ID"].ToString();

            string strSql = String.Format(@"SELECT * FROM (
                                                select B.Description,ROW_NUMBER() OVER(ORDER BY A.Sequence) Sequence,a.ID,
                                                RouteID,StationTypeID,UnitStateID from mesRouteDetail A
                                                    inner join mesStationType B ON A.StationTypeID=B.ID
                                                    WHERE A.RouteID=DBO.ufnRTEGetRouteID({0},{1},{2},{3})) A order by Sequence",
                                                    LineID, PartID, PartFamilyID, ProductionOrderID);

            if (S_RouteType == "1")
            {
                strSql = @"	select B.Description StationType
	                           ,C.Description  CurrentStation 
	                           ,D.Description  OutputStation 
                                ,E.Description StateDef
                                ,A.StationTypeID
                                ,(case F.Content when 'TT' then 'TT' else 'SFC' end) StationTypeType
	                            from mesUnitOutputState A 
			                     left join mesStationType B on A.StationTypeID=B.ID	
			                    left join mesUnitState C on A.CurrStateID=C.ID
			                    left join mesUnitState D on A.OutputStateID=D.ID
                                join luUnitStatus E on E.ID=A.OutputStateDefID
                                left join mesStationTypeDetail F on A.StationTypeID=F.StationTypeID	
	                    where A.RouteID=" + S_RouteID + @" 		
	                    order by A.StationTypeID	";
            }

            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }


        public DataSet GetRouteDetail2(string LineID, string PartID, string PartFamilyID,string ProductionOrderID)
        {
            //string strSql = String.Format(@"select B.Description,A.* from mesRouteDetail A
            //                                inner join mesStationType B ON A.StationTypeID=B.ID
            //                                WHERE A.RouteID=DBO.ufnRTEGetRouteID({0},{1},{2})
            //                                ORDER BY A.Sequence", LineID, PartID, PartFamilyID);

            PartSelectDAL v_PartSelectDAL = new PartSelectDAL();

            string routeID = v_PartSelectDAL.GetRouteID(LineID, PartID, PartFamilyID, ProductionOrderID).ToString();

            string S_Sql = string.Format(@"select A.* from 
                            (select B.ID,B.Name,B.Description RouteName,
			                            A.StationTypeID,A.Sequence SequenceMod,A.Description RouteDetailName,
			                            A.UnitStateID,                                
			                            E.Description ApplicationType,
                                         D.Description StationType,
			                            cast(ROW_NUMBER() over(order by A.Sequence) as int) as Sequence
		                            from 
			                            mesRouteDetail A
			                            join mesRoute B on A.RouteID=B.ID  		  	                             
			                            join mesStationType D on D.ID=A.StationTypeID
			                            join luApplicationType E on E.ID=D.ApplicationTypeID
	                            where B.ID={0}  
                            )A where 1=1", routeID);

            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }

    }
}
