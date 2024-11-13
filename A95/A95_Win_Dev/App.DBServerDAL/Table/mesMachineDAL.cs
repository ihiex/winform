using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class   mesMachineDAL
	{
		public int Insert(mesMachine model)
		{
			object obj = SqlServerHelper.ExecuteScalar(
				"INSERT INTO mesMachine(StationID,SN,RuningQuantity,MaxUseQuantity,StartRuningTime,LastRuningTime,MachineFamilyID,PartID,WarningStatus,Discription,Status,ValidFrom,ValidTo,PartNO,PartDesc,PartGroup,PartGroupDesc,LastMaintenance,NextMaintenance,Store,Attributes) VALUES (@StationID,@SN,@RuningQuantity,@MaxUseQuantity,@StartRuningTime,@LastRuningTime,@MachineFamilyID,@PartID,@WarningStatus,@Discription,@Status,@ValidFrom,@ValidTo,@PartNO,@PartDesc,@PartGroup,@PartGroupDesc,@LastMaintenance,@NextMaintenance,@Store,@Attributes);SELECT @@identity"
				,new SqlParameter("@StationID", model.StationID)
				,new SqlParameter("@SN", model.SN)
				,new SqlParameter("@RuningQuantity", model.RuningQuantity)
				,new SqlParameter("@MaxUseQuantity", model.MaxUseQuantity)
				,new SqlParameter("@StartRuningTime", model.StartRuningTime)
				,new SqlParameter("@LastRuningTime", model.LastRuningTime)
				,new SqlParameter("@MachineFamilyID", model.MachineFamilyID)
				,new SqlParameter("@PartID", model.PartID)
				,new SqlParameter("@WarningStatus", model.WarningStatus)
				,new SqlParameter("@Discription", model.Discription)
				,new SqlParameter("@Status", model.Status)
				,new SqlParameter("@ValidFrom", model.ValidFrom)
				,new SqlParameter("@ValidTo", model.ValidTo)
				,new SqlParameter("@PartNO", model.PartNO)
				,new SqlParameter("@PartDesc", model.PartDesc)
				,new SqlParameter("@PartGroup", model.PartGroup)
				,new SqlParameter("@PartGroupDesc", model.PartGroupDesc)
				,new SqlParameter("@LastMaintenance", model.LastMaintenance)
				,new SqlParameter("@NextMaintenance", model.NextMaintenance)
				,new SqlParameter("@Store", model.Store)
				,new SqlParameter("@Attributes", model.Attributes)
			);
			return Convert.ToInt32(obj);
		}

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM mesMachine WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public bool Update(mesMachine model)
		{
			string sql = "UPDATE mesMachine SET StationID=@StationID,SN=@SN,RuningQuantity=@RuningQuantity,MaxUseQuantity=@MaxUseQuantity,StartRuningTime=@StartRuningTime,LastRuningTime=@LastRuningTime,MachineFamilyID=@MachineFamilyID,PartID=@PartID,WarningStatus=@WarningStatus,Discription=@Discription,Status=@Status,ValidFrom=@ValidFrom,ValidTo=@ValidTo,PartNO=@PartNO,PartDesc=@PartDesc,PartGroup=@PartGroup,PartGroupDesc=@PartGroupDesc,LastMaintenance=@LastMaintenance,NextMaintenance=@NextMaintenance,Store=@Store,Attributes=@Attributes WHERE ID=@ID";
			int rows = SqlServerHelper.ExecuteNonQuery(sql
				, new SqlParameter("@ID", model.ID)
				, new SqlParameter("@StationID", model.StationID)
				, new SqlParameter("@SN", model.SN)
				, new SqlParameter("@RuningQuantity", model.RuningQuantity)
				, new SqlParameter("@MaxUseQuantity", model.MaxUseQuantity)
				, new SqlParameter("@StartRuningTime", model.StartRuningTime)
				, new SqlParameter("@LastRuningTime", model.LastRuningTime)
				, new SqlParameter("@MachineFamilyID", model.MachineFamilyID)
				, new SqlParameter("@PartID", model.PartID)
				, new SqlParameter("@WarningStatus", model.WarningStatus)
				, new SqlParameter("@Discription", model.Discription)
				, new SqlParameter("@Status", model.Status)
				, new SqlParameter("@ValidFrom", model.ValidFrom)
				, new SqlParameter("@ValidTo", model.ValidTo)
				, new SqlParameter("@PartNO", model.PartNO)
				, new SqlParameter("@PartDesc", model.PartDesc)
				, new SqlParameter("@PartGroup", model.PartGroup)
				, new SqlParameter("@PartGroupDesc", model.PartGroupDesc)
				, new SqlParameter("@LastMaintenance", model.LastMaintenance)
				, new SqlParameter("@NextMaintenance", model.NextMaintenance)
				, new SqlParameter("@Store", model.Store)
				, new SqlParameter("@Attributes", model.Attributes)
			);
			return rows > 0;
		}

		public mesMachine Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesMachine WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			mesMachine model = ToModel(row);
			return model;
		}

		private static mesMachine ToModel(DataRow row)
		{
			mesMachine model = new mesMachine();
			model.ID = (int)row["ID"];
			model.StationID = (int)row["StationID"];
			model.SN = (string)row["SN"];
			model.RuningQuantity = (int)row["RuningQuantity"];
			model.MaxUseQuantity = (int)row["MaxUseQuantity"];
			model.StartRuningTime = (DateTime)row["StartRuningTime"];
			model.LastRuningTime = (DateTime)row["LastRuningTime"];
			model.MachineFamilyID = (int)row["MachineFamilyID"];
			model.PartID = (int)row["PartID"];
			model.WarningStatus = (int)row["WarningStatus"];
			model.Discription = (string)row["Discription"];
			model.Status = (int)row["Status"];
			model.ValidFrom = (string)row["ValidFrom"];
			model.ValidTo = (string)row["ValidTo"];
			model.PartNO = (string)row["PartNO"];
			model.PartDesc = (string)row["PartDesc"];
			model.PartGroup = (string)row["PartGroup"];
			model.PartGroupDesc = (string)row["PartGroupDesc"];
			model.LastMaintenance = (DateTime)row["LastMaintenance"];
			model.NextMaintenance = (DateTime)row["NextMaintenance"];
			model.Store = (string)row["Store"];
			model.Attributes = (string)row["Attributes"];
			return model;
		}

		public IEnumerable<mesMachine> ListAll(string S_Where)
		{
			List<mesMachine> list = new List<mesMachine>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesMachine " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}

        public DataSet MesGetLineIDByMachineSN(string MachineSN)
        {
            string strSql = string.Format("select top 1 ID,LineID from mesStation where Description='{0}'", MachineSN);
            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }

        public DataSet MesGetStatusIDByList(int StationTypeID, int PartID, string MachineSN)
        {
            string strSql = string.Format(@"SELECT A.StatusID FROM mesMachine A where StationTypeID={0}
                                    AND EXISTS(SELECT 1 FROM mesRouteMachineMap B WHERE B.PartID ={1}
                                    AND A.MachineFamilyID = B.MachineFamilyID)  AND A.SN ='{2}'",
                                StationTypeID, PartID, MachineSN);
            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }

        public void MesModMachineBySN(string MachineSN)
        {
            string S_Sql = "Update mesMachine set StatusID=1,RuningQuantity=0 where SN='" + MachineSN + "'";
            SqlServerHelper.ExecSql(S_Sql);
        }
    }
}
