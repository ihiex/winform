using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class   mesUnitComponentDAL
	{
		public string Insert(mesUnitComponent model)
		{
            string S_Result = "";
            try
            {
                object obj = SqlServerHelper.ExecuteScalar(
				"INSERT INTO mesUnitComponent(UnitID,UnitComponentTypeID,ChildUnitID,ChildSerialNumber,ChildLotNumber,ChildPartID,ChildPartFamilyID,Position," +
                "InsertedEmployeeID,InsertedStationID," +
                "InsertedTime," +
                //"RemovedEmployeeID,RemovedStationID,RemovedTime," +
                "StatusID,LastUpdate" +
                //"PreviousLink" +
                ") VALUES " +
                "(@UnitID,@UnitComponentTypeID,@ChildUnitID,@ChildSerialNumber,@ChildLotNumber,@ChildPartID,@ChildPartFamilyID,@Position," +
                "@InsertedEmployeeID,@InsertedStationID," +
                "GETDATE()," +
                //"@RemovedEmployeeID,@RemovedStationID,@RemovedTime," +
                "@StatusID,GETDATE()" +
                //"@PreviousLink" +
                ");SELECT @@identity"
				,new SqlParameter("@UnitID", model.UnitID)
				,new SqlParameter("@UnitComponentTypeID", model.UnitComponentTypeID)
				,new SqlParameter("@ChildUnitID", model.ChildUnitID)
				,new SqlParameter("@ChildSerialNumber", model.ChildSerialNumber)
				,new SqlParameter("@ChildLotNumber", model.ChildLotNumber)
				,new SqlParameter("@ChildPartID", model.ChildPartID)
				,new SqlParameter("@ChildPartFamilyID", model.ChildPartFamilyID)
				,new SqlParameter("@Position", model.Position)
                , new SqlParameter("@InsertedEmployeeID", model.InsertedEmployeeID)
                , new SqlParameter("@InsertedStationID", model.InsertedStationID)
                //,new SqlParameter("@InsertedTime", model.InsertedTime)
                //, new SqlParameter("@RemovedEmployeeID", model.RemovedEmployeeID)
                //, new SqlParameter("@RemovedStationID", model.RemovedStationID)
                //,new SqlParameter("@RemovedTime", model.RemovedTime)
                , new SqlParameter("@StatusID", model.StatusID)
				//,new SqlParameter("@LastUpdate", model.LastUpdate)
				//,new SqlParameter("@PreviousLink", model.PreviousLink)
                );
                S_Result = "OK";
            }
            catch (Exception ex)
            {
                S_Result = "E:" + ex.Message;
            }
            return S_Result;
        }

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM mesUnitComponent WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public bool Update(mesUnitComponent model)
		{
			string sql = "UPDATE mesUnitComponent SET UnitID=@UnitID,UnitComponentTypeID=@UnitComponentTypeID," +
                "ChildUnitID=@ChildUnitID,ChildSerialNumber=@ChildSerialNumber,ChildLotNumber=@ChildLotNumber," +
                "ChildPartID=@ChildPartID,ChildPartFamilyID=@ChildPartFamilyID,Position=@Position," +
                "InsertedEmployeeID=@InsertedEmployeeID,InsertedStationID=@InsertedStationID,InsertedTime=GETDATE()," +
                "RemovedEmployeeID=@RemovedEmployeeID,RemovedStationID=@RemovedStationID,RemovedTime=GETDATE()," +
                "StatusID=@StatusID,LastUpdate=GETDATE(),PreviousLink=@PreviousLink WHERE ID=@ID";
			int rows = SqlServerHelper.ExecuteNonQuery(sql
				, new SqlParameter("@ID", model.ID)
				, new SqlParameter("@UnitID", model.UnitID)
				, new SqlParameter("@UnitComponentTypeID", model.UnitComponentTypeID)
				, new SqlParameter("@ChildUnitID", model.ChildUnitID)
				, new SqlParameter("@ChildSerialNumber", model.ChildSerialNumber)
				, new SqlParameter("@ChildLotNumber", model.ChildLotNumber)
				, new SqlParameter("@ChildPartID", model.ChildPartID)
				, new SqlParameter("@ChildPartFamilyID", model.ChildPartFamilyID)
				, new SqlParameter("@Position", model.Position)
				, new SqlParameter("@InsertedEmployeeID", model.InsertedEmployeeID)
				, new SqlParameter("@InsertedStationID", model.InsertedStationID)
				//, new SqlParameter("@InsertedTime", model.InsertedTime)
				, new SqlParameter("@RemovedEmployeeID", model.RemovedEmployeeID)
				, new SqlParameter("@RemovedStationID", model.RemovedStationID)
				//, new SqlParameter("@RemovedTime", model.RemovedTime)
				, new SqlParameter("@StatusID", model.StatusID)
				//, new SqlParameter("@LastUpdate", model.LastUpdate)
				, new SqlParameter("@PreviousLink", model.PreviousLink)
			);
			return rows > 0;
		}

		public mesUnitComponent Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesUnitComponent WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			mesUnitComponent model = ToModel(row);
			return model;
		}

		private static mesUnitComponent ToModel(DataRow row)
		{
			mesUnitComponent model = new mesUnitComponent();
			model.ID = (int)row["ID"];
			model.UnitID = (int)row["UnitID"];
			model.UnitComponentTypeID = (int)row["UnitComponentTypeID"];
			model.ChildUnitID = (int)row["ChildUnitID"];
			model.ChildSerialNumber = (string)row["ChildSerialNumber"];
			model.ChildLotNumber = (string)row["ChildLotNumber"];
			model.ChildPartID = (int)row["ChildPartID"];
			model.ChildPartFamilyID = (int)row["ChildPartFamilyID"];
			model.Position = (string)row["Position"];
			model.InsertedEmployeeID = (int)row["InsertedEmployeeID"];
			model.InsertedStationID = (int)row["InsertedStationID"];
			model.InsertedTime = (DateTime)row["InsertedTime"];
			model.RemovedEmployeeID = (int)row["RemovedEmployeeID"];
			model.RemovedStationID = (int)row["RemovedStationID"];
			model.RemovedTime = (DateTime)row["RemovedTime"];
			model.StatusID = (object)row["StatusID"];
			model.LastUpdate = (DateTime)row["LastUpdate"];
			model.PreviousLink = (int)row["PreviousLink"];
			return model;
		}

		public IEnumerable<mesUnitComponent> ListAll(string S_Where)
		{
			List<mesUnitComponent> list = new List<mesUnitComponent>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesUnitComponent " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}

        public bool MESCheckChildSerialNumber(string ChildSerialNumber)
        {
            string sql = string.Format("SELECT 1 FROM mesUnitComponent WHERE ChildSerialNumber='{0}' and StatusID=1", ChildSerialNumber);
            DataTable dts = SqlServerHelper.ExecuteDataTable(sql);
            if(dts!=null&& dts.Rows.Count>0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
	}
}
