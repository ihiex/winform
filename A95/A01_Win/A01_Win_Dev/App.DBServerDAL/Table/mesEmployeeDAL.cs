using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class   mesEmployeeDAL
	{
		public int Insert(mesEmployee model)
		{
			object obj = SqlServerHelper.ExecuteScalar(
				"INSERT INTO mesEmployee(UserID,Password,Lastname,Firstname,Description,EmailAddress,LoginAttempt,EmployeeGroupID,StatusID) VALUES (@UserID,@Password,@Lastname,@Firstname,@Description,@EmailAddress,@LoginAttempt,@EmployeeGroupID,@StatusID);SELECT @@identity"
				,new SqlParameter("@UserID", model.UserID)
				,new SqlParameter("@Password", model.Password)
				,new SqlParameter("@Lastname", model.Lastname)
				,new SqlParameter("@Firstname", model.Firstname)
				,new SqlParameter("@Description", model.Description)
				,new SqlParameter("@EmailAddress", model.EmailAddress)
				,new SqlParameter("@LoginAttempt", model.LoginAttempt)
				,new SqlParameter("@EmployeeGroupID", model.EmployeeGroupID)
				,new SqlParameter("@StatusID", model.StatusID)
                ,new SqlParameter("@PermissionId", model.PermissionId)
			);
			return Convert.ToInt32(obj);
		}

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM mesEmployee WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public bool Update(mesEmployee model)
		{
			string sql = "UPDATE mesEmployee SET UserID=@UserID,Password=@Password,Lastname=@Lastname,Firstname=@Firstname,Description=@Description,EmailAddress=@EmailAddress,LoginAttempt=@LoginAttempt,EmployeeGroupID=@EmployeeGroupID,StatusID=@StatusID WHERE ID=@ID";
			int rows = SqlServerHelper.ExecuteNonQuery(sql
				, new SqlParameter("@ID", model.ID)
				, new SqlParameter("@UserID", model.UserID)
				, new SqlParameter("@Password", model.Password)
				, new SqlParameter("@Lastname", model.Lastname)
				, new SqlParameter("@Firstname", model.Firstname)
				, new SqlParameter("@Description", model.Description)
				, new SqlParameter("@EmailAddress", model.EmailAddress)
				, new SqlParameter("@LoginAttempt", model.LoginAttempt)
				, new SqlParameter("@EmployeeGroupID", model.EmployeeGroupID)
				, new SqlParameter("@StatusID", model.StatusID)
                , new SqlParameter("@PermissionId", model.PermissionId)
            );
			return rows > 0;
		}

		public mesEmployee Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesEmployee WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			mesEmployee model = ToModel(row);
			return model;
		}

		private static mesEmployee ToModel(DataRow row)
		{
			mesEmployee model = new mesEmployee();
			model.ID =Convert.ToInt32(row["ID"].ToString());
			model.UserID = row["UserID"].ToString();
			model.Password = row["Password"].ToString();
			model.Lastname = row["Lastname"].ToString();
			model.Firstname = row["Firstname"].ToString();
			model.Description = row["Description"].ToString();
			model.EmailAddress = row["EmailAddress"].ToString();
			model.LoginAttempt = Convert.ToInt32(row["LoginAttempt"].ToString());
			model.EmployeeGroupID =Convert.ToInt32(row["EmployeeGroupID"].ToString());
			model.StatusID = Convert.ToInt32(row["StatusID"].ToString());
            model.PermissionId = Convert.ToInt32(row["PermissionId"].ToString());
            return model;
		}

		public IEnumerable<mesEmployee> ListAll(string S_Where)
		{
			List<mesEmployee> list = new List<mesEmployee>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesEmployee " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}


        
	}
}
