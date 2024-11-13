using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class   mesHistoryDAL
	{
		public int Insert(mesHistory model)
		{
            //string S_Sql = "select Max(ID)+1 MaxID from mesHistory";
            //DataTable DT = SqlServerHelper.ExecuteDataTable(S_Sql);
            //int I_MaxID = Convert.ToInt32(DT.Rows[0][0].ToString());
            //model.ID = I_MaxID;

            object obj = SqlServerHelper.ExecuteScalar(
				"INSERT INTO mesHistory(UnitID,UnitStateID,EmployeeID,StationID,EnterTime,ExitTime,ProductionOrderID,PartID,LooperCount)" +
                " VALUES (@UnitID,@UnitStateID,@EmployeeID,@StationID,GETDATE(),GETDATE(),@ProductionOrderID,@PartID,@LooperCount);SELECT @@identity"
                , new SqlParameter("@UnitID", model.UnitID)
				,new SqlParameter("@UnitStateID", model.UnitStateID)
				,new SqlParameter("@EmployeeID", model.EmployeeID)
				,new SqlParameter("@StationID", model.StationID)
				,new SqlParameter("@ProductionOrderID", model.ProductionOrderID)
				,new SqlParameter("@PartID", model.PartID)
				,new SqlParameter("@LooperCount", model.LooperCount)
			);
            string S_Result = obj.ToString();
            if (S_Result == "") { S_Result = "0"; }

            return Convert.ToInt32(S_Result);
        }

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM mesHistory WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public bool Update(mesHistory model)
		{
			string sql = "UPDATE mesHistory SET " +
                //"UnitID=@UnitID,UnitStateID=@UnitStateID,EmployeeID=@EmployeeID," +
                "StationID=@StationID," +
                //"EnterTime=@EnterTime," +
                "ExitTime=GETDATE()," +
                "ProductionOrderID=@ProductionOrderID," +
                //"PartID=@PartID,LooperCount=@LooperCount" +
                " WHERE ID=@ID";
			int rows = SqlServerHelper.ExecuteNonQuery(sql
				, new SqlParameter("@ID", model.ID)
				//, new SqlParameter("@UnitID", model.UnitID)
				//, new SqlParameter("@UnitStateID", model.UnitStateID)
				//, new SqlParameter("@EmployeeID", model.EmployeeID)
				, new SqlParameter("@StationID", model.StationID)
				//, new SqlParameter("@EnterTime", model.EnterTime)
				//, new SqlParameter("@ExitTime", model.ExitTime)
				, new SqlParameter("@ProductionOrderID", model.ProductionOrderID)
				//, new SqlParameter("@PartID", model.PartID)
				//, new SqlParameter("@LooperCount", model.LooperCount)
			);
			return rows > 0;
		}

		public mesHistory Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesHistory WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			mesHistory model = ToModel(row);
			return model;
		}

		private static mesHistory ToModel(DataRow row)
		{
			mesHistory model = new mesHistory();
			model.ID = (int)row["ID"];
			model.UnitID = (int)row["UnitID"];
			model.UnitStateID = (int)row["UnitStateID"];
			model.EmployeeID = (int)row["EmployeeID"];
			model.StationID = (int)row["StationID"];
			model.EnterTime = (DateTime)row["EnterTime"];
			model.ExitTime = (DateTime)row["ExitTime"];
			model.ProductionOrderID = (int)row["ProductionOrderID"];
			model.PartID = (int)row["PartID"];
			model.LooperCount = (int)row["LooperCount"];
			return model;
		}

		public IEnumerable<mesHistory> ListAll(string S_Where)
		{
			List<mesHistory> list = new List<mesHistory>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesHistory " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}
	}
}
