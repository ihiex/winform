using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class luMachineFamilyDAL
	{
		public int Insert(luMachineFamily model)
		{
			object obj = SqlServerHelper.ExecuteScalar(
				"INSERT INTO luMachineFamily(Name,Description,MachineFamilyTypeID,Status) VALUES (@Name,@Description,@MachineFamilyTypeID,@Status);SELECT @@identity"
				,new SqlParameter("@Name", model.Name)
				,new SqlParameter("@Description", model.Description)
				,new SqlParameter("@MachineFamilyTypeID", model.MachineFamilyTypeID)
				,new SqlParameter("@Status", model.Status)
			);
			return Convert.ToInt32(obj);
		}

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM luMachineFamily WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public bool Update(luMachineFamily model)
		{
			string sql = "UPDATE luMachineFamily SET Name=@Name,Description=@Description,MachineFamilyTypeID=@MachineFamilyTypeID,Status=@Status WHERE ID=@ID";
			int rows = SqlServerHelper.ExecuteNonQuery(sql
				, new SqlParameter("@ID", model.ID)
				, new SqlParameter("@Name", model.Name)
				, new SqlParameter("@Description", model.Description)
				, new SqlParameter("@MachineFamilyTypeID", model.MachineFamilyTypeID)
				, new SqlParameter("@Status", model.Status)
			);
			return rows > 0;
		}

		public luMachineFamily Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM luMachineFamily WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			luMachineFamily model = ToModel(row);
			return model;
		}

		private static luMachineFamily ToModel(DataRow row)
		{
			luMachineFamily model = new luMachineFamily();
			model.ID = (int)row["ID"];
			model.Name = (string)row["Name"];
			model.Description = (string)row["Description"];
			model.MachineFamilyTypeID = (int)row["MachineFamilyTypeID"];
			model.Status = (object)row["Status"];
			return model;
		}

		public IEnumerable<luMachineFamily> ListAll(string S_Where)
		{
			List<luMachineFamily> list = new List<luMachineFamily>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM luMachineFamily " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}
	}
}
