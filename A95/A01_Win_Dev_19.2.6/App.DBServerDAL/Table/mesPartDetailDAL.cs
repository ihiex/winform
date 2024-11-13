using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class   mesPartDetailDAL
	{
		public int Insert(mesPartDetail model)
		{
			object obj = SqlServerHelper.ExecuteScalar(
				"INSERT INTO mesPartDetail(PartID,PartDetailDefID,Content) VALUES (@PartID,@PartDetailDefID,@Content);SELECT @@identity"
				,new SqlParameter("@PartID", model.PartID)
				,new SqlParameter("@PartDetailDefID", model.PartDetailDefID)
				,new SqlParameter("@Content", model.Content)
			);
			return Convert.ToInt32(obj);
		}

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM mesPartDetail WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public bool Update(mesPartDetail model)
		{
			string sql = "UPDATE mesPartDetail SET PartID=@PartID,PartDetailDefID=@PartDetailDefID,Content=@Content WHERE ID=@ID";
			int rows = SqlServerHelper.ExecuteNonQuery(sql
				, new SqlParameter("@ID", model.ID)
				, new SqlParameter("@PartID", model.PartID)
				, new SqlParameter("@PartDetailDefID", model.PartDetailDefID)
				, new SqlParameter("@Content", model.Content)
			);
			return rows > 0;
		}

		public mesPartDetail Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesPartDetail WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			mesPartDetail model = ToModel(row);
			return model;
		}

		private static mesPartDetail ToModel(DataRow row)
		{
			mesPartDetail model = new mesPartDetail();
			model.ID = (int)row["ID"];
			model.PartID = (int)row["PartID"];
			model.PartDetailDefID = (int)row["PartDetailDefID"];
			model.Content = (string)row["Content"];
			return model;
		}

		public IEnumerable<mesPartDetail> ListAll(string S_Where)
		{
			List<mesPartDetail> list = new List<mesPartDetail>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesPartDetail " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}

    }
}
