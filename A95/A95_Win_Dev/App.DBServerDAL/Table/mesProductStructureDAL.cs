using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class   mesProductStructureDAL
	{
		public int Insert(mesProductStructure model)
		{
			object obj = SqlServerHelper.ExecuteScalar(
				"INSERT INTO mesProductStructure(ParentPartID,PartID,PartPosition,IsCritical,Status,StationTypeID) VALUES (@ParentPartID,@PartID,@PartPosition,@IsCritical,@Status,@StationTypeID);SELECT @@identity"
				,new SqlParameter("@ParentPartID", model.ParentPartID)
				,new SqlParameter("@PartID", model.PartID)
				,new SqlParameter("@PartPosition", model.PartPosition)
				,new SqlParameter("@IsCritical", model.IsCritical)
				,new SqlParameter("@Status", model.Status)
				,new SqlParameter("@StationTypeID", model.StationTypeID)
			);
			return Convert.ToInt32(obj);
		}

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM mesProductStructure WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public bool Update(mesProductStructure model)
		{
			string sql = "UPDATE mesProductStructure SET ParentPartID=@ParentPartID,PartID=@PartID,PartPosition=@PartPosition,IsCritical=@IsCritical,Status=@Status,StationTypeID=@StationTypeID WHERE ID=@ID";
			int rows = SqlServerHelper.ExecuteNonQuery(sql
				, new SqlParameter("@ID", model.ID)
				, new SqlParameter("@ParentPartID", model.ParentPartID)
				, new SqlParameter("@PartID", model.PartID)
				, new SqlParameter("@PartPosition", model.PartPosition)
				, new SqlParameter("@IsCritical", model.IsCritical)
				, new SqlParameter("@Status", model.Status)
				, new SqlParameter("@StationTypeID", model.StationTypeID)
			);
			return rows > 0;
		}

		public mesProductStructure Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesProductStructure WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			mesProductStructure model = ToModel(row);
			return model;
		}

		private static mesProductStructure ToModel(DataRow row)
		{
			mesProductStructure model = new mesProductStructure();
			model.ID = (int)row["ID"];
			model.ParentPartID = (int)row["ParentPartID"];
			model.PartID = (int)row["PartID"];
			model.PartPosition = (string)row["PartPosition"];
			model.IsCritical = (bool)row["IsCritical"];
			model.Status = (object)row["Status"];
			model.StationTypeID = (int)row["StationTypeID"];
			return model;
		}

		public IEnumerable<mesProductStructure> ListAll(string S_Where)
		{
			List<mesProductStructure> list = new List<mesProductStructure>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesProductStructure " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}
	}
}
