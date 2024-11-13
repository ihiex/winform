using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class   mesUnitDAL
	{
		public string  Insert(mesUnit model)
		{
            string S_Result = "";
            try
            {
                string S_Sql = "select MAX(ID)+1 MaxID from mesUnit";
                DataTable DT = SqlServerHelper.ExecuteDataTable(S_Sql);
                string S_MaxID = DT.Rows[0][0].ToString();
                if (S_MaxID == "" || S_MaxID == null) { S_MaxID = "1"; }

                int I_MaxID = Convert.ToInt32(S_MaxID);
                model.ID = I_MaxID;

                object obj = SqlServerHelper.ExecuteScalar(
                    "INSERT INTO mesUnit(ID,UnitStateID,StatusID,StationID,EmployeeID,CreationTime,LastUpdate,PanelID,LineID," +
                    "ProductionOrderID,RMAID,PartID,LooperCount) " +

                    "VALUES (@ID,@UnitStateID,@StatusID,@StationID,@EmployeeID,GETDATE(),GETDATE(),@PanelID,@LineID" +
                    ",@ProductionOrderID,@RMAID,@PartID,@LooperCount);SELECT @@identity"

                    , new SqlParameter("@ID", model.ID)
                    , new SqlParameter("@UnitStateID", model.UnitStateID)
                    , new SqlParameter("@StatusID", model.StatusID)
                    , new SqlParameter("@StationID", model.StationID)
                    , new SqlParameter("@EmployeeID", model.EmployeeID)
                    , new SqlParameter("@PanelID", model.PanelID)
                    , new SqlParameter("@LineID", model.LineID)
                    , new SqlParameter("@ProductionOrderID", model.ProductionOrderID)
                    , new SqlParameter("@RMAID", model.RMAID)
                    , new SqlParameter("@PartID", model.PartID)                    
                    , new SqlParameter("@LooperCount", model.LooperCount)
                );
                S_Result = I_MaxID.ToString(); 
            }
            catch (Exception ex)
            {
                S_Result ="E:"+ ex.Message; 
            }
            return S_Result;
        }

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM mesUnit WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public string Update(mesUnit model)
		{
            string S_Result = "";
            try
            {
                string sql = "UPDATE mesUnit SET " +
                    "UnitStateID=@UnitStateID," +
                    "StatusID=@StatusID," +
                    "StationID=@StationID," +
                    //"EmployeeID=@EmployeeID,CreationTime=@CreationTime," +
                    "LastUpdate=GETDATE()," +
                    //"PanelID=@PanelID,LineID=@LineID," +
                    "ProductionOrderID=@ProductionOrderID  " +
                    //"RMAID=@RMAID,PartID=@PartID,LooperCount=@LooperCount " +
                    "WHERE ID=@ID";
                int rows = SqlServerHelper.ExecuteNonQuery(sql
                    , new SqlParameter("@ID", model.ID)
                    , new SqlParameter("@UnitStateID", model.UnitStateID)
                    , new SqlParameter("@StatusID", model.StatusID)
                    , new SqlParameter("@StationID", model.StationID)
                    //, new SqlParameter("@EmployeeID", model.EmployeeID)
                    //, new SqlParameter("@CreationTime", model.CreationTime)
                    //, new SqlParameter("@LastUpdate", model.LastUpdate)
                    //, new SqlParameter("@PanelID", model.PanelID)
                    //, new SqlParameter("@LineID", model.LineID)
                    , new SqlParameter("@ProductionOrderID", model.ProductionOrderID)
                //, new SqlParameter("@RMAID", model.RMAID)
                //, new SqlParameter("@PartID", model.PartID)
                //, new SqlParameter("@LooperCount", model.LooperCount)
                );
                S_Result = "OK";
            }
            catch (Exception ex)
            {
                S_Result = "E:" + ex.Message;
            }
            return S_Result;
        }

		public mesUnit Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesUnit WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			mesUnit model = ToModel(row);
			return model;
		}

		private static mesUnit ToModel(DataRow row)
		{
			mesUnit model = new mesUnit();
			model.ID = Convert.ToInt32(row["ID"].ToString());
			model.UnitStateID = Convert.ToInt32(row["UnitStateID"].ToString());
            model.StatusID = Convert.ToInt32(row["StatusID"].ToString());
            model.StationID = Convert.ToInt32(row["StationID"].ToString());
            model.EmployeeID = Convert.ToInt32(row["EmployeeID"].ToString());
            model.CreationTime = Convert.ToDateTime(row["CreationTime"].ToString());
            model.LastUpdate = Convert.ToDateTime(row["LastUpdate"].ToString());
            model.PanelID = Convert.ToInt32(row["PanelID"].ToString());
            model.LineID = Convert.ToInt32(row["LineID"].ToString());
            model.ProductionOrderID = Convert.ToInt32(row["ProductionOrderID"].ToString());
            model.RMAID = Convert.ToInt32(row["RMAID"].ToString());
            model.PartID = Convert.ToInt32(row["PartID"].ToString());
            model.LooperCount = Convert.ToInt32(row["LooperCount"].ToString());
            return model;
		}

		public IEnumerable<mesUnit> ListAll(string S_Where)
		{
			List<mesUnit> list = new List<mesUnit>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesUnit  " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}

        public string UpdatePlasma(string SN,int UnitStateID,string LastUpdate,int LineID)
        {
            string S_Result = "";
            try
            {
                string S_Sql = string.Format(@"UPDATE A SET A.UnitStateID={0},LastUpdate='{1}',LineID={2} FROM mesUnit A 
                    INNER JOIN mesSerialNumber B ON A.ID = B.UnitID WHERE B.Value = '{3}'", UnitStateID, LastUpdate,LineID,SN);
                SqlServerHelper.ExecSql(S_Sql);
                S_Result = "OK";
            }
            catch (Exception ex)
            {
                S_Result = "E:" + ex.Message;
            }
            return S_Result;
        }
    }
}
