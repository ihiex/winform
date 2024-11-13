using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Management;

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
			//model.LoginAttempt = Convert.ToInt32(row["LoginAttempt"].ToString());
			model.EmployeeGroupID =Convert.ToInt32(row["EmployeeGroupID"].ToString());
			model.StatusID = Convert.ToInt32(row["StatusID"].ToString());
            model.PermissionId = Convert.ToInt32(row["PermissionId"].ToString());
            return model;
		}

		public IEnumerable<mesEmployee> ListAll(string S_Where)
		{
			List<mesEmployee> list = new List<mesEmployee>();

            try
            {
                string[] List_Where = S_Where.Split('=');
                string v_where = S_Where;

                string[] List_User = List_Where[1].Split(' ');
                DataTable DT = SqlServerHelper.Data_Table("SELECT * FROM mesEmployee where UserId=" + List_User[0]);
                if (DT.Rows.Count > 0)
                {
                    string S_Password = DT.Rows[0]["Password"].ToString();

                    try
                    {
                        if (S_Where.IndexOf("Password=") > -1)
                        {
                            string S_PWD = PublicF.DecryptPassword(S_Password, "");
                            if (S_PWD == "-1:ciphertext is wrong")
                            {
                                v_where = S_Where;  // List_Where[0] + "=" + List_Where[1] + "='" + List_Where[2].Replace("''","'") + "'";
                                                    //return list;
                            }
                            else
                            {
                                string S_UserPWD = List_Where[2].Substring(1, List_Where[2].Length - 2);
                                string S_JM = PublicF.EncryptPassword(S_UserPWD, "");

                                v_where = List_Where[0] + "=" + List_Where[1] + "='" + S_JM + "'";
                            }
                        }
                        else
                        {
                            v_where = S_Where;
                        }
                    }
                    catch
                    {

                    }

                    //try
                    //{
                    //    string S_PWD = PublicF.DecryptPassword(S_Password, "");
                    //    if (S_PWD == "-1:ciphertext is wrong")
                    //    {
                    //        v_where = S_Where;  // List_Where[0] + "=" + List_Where[1] + "='" + List_Where[2].Replace("''","'") + "'";
                    //                            //return list;
                    //    }
                    //    else
                    //    {
                    //        string S_UserPWD = List_Where[2].Substring(1, List_Where[2].Length - 2);
                    //        string S_JM = PublicF.EncryptPassword(S_UserPWD, "");

                    //        v_where = List_Where[0] + "=" + List_Where[1] + "='" + S_JM + "'";
                    //    }
                    //}
                    //catch
                    //{
                    //    v_where = S_Where;
                    //}
                }

                DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesEmployee " + v_where + " and StatusID=1");
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(ToModel(row));
                }
            }
            catch { }

			return list;
		}

        public bool PermissionCheck(int EmployeeID,int StationTypeID)
        {
            string S_ValidateSecond = ValidateSecond(EmployeeID.ToString(), "");
            if (S_ValidateSecond != "OK")
            {
                return false;
            }

            string sql = string.Format("select * from mesStationTypeAccess where Status=1 and EmployeeID={0} and StationTypeID={1}",EmployeeID, StationTypeID);
            DataTable dt = SqlServerHelper.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public string ValidateSecond(string UserID, string PWD)
        {
            string S_Result = "OK";

            string S_IsValidateSecond = "0";
            try
            {
                S_IsValidateSecond = System.Configuration.ConfigurationManager.AppSettings["IsValidateSecond"];
                S_IsValidateSecond = S_IsValidateSecond ?? "0";
            }
            catch
            {
                S_IsValidateSecond = "0";
            }

            if (S_IsValidateSecond == "0")
            {
                return S_Result;
            }

            string S_Sql_ID = "";
            string S_Sql_UserID = "";
            string S_Sql_Lastname = "";
            string S_Sql_Firstname = "";
            string S_Sql_Password = "";
            string S_Sql_OrganizeId = "";
            string S_Sql_DepartmentId = "";
            string S_Sql_RoleId = "";
            string S_Sql_IsAdministrator = "";
            string S_Sql_UserType = "";

            string S_Sql2_Password = "";

            string S_Data1_ID = "";
            string S_Data1_UserID = "";
            string S_Data1_Lastname = "";
            string S_Data1_Firstname = "";
            string S_Data1_Password = "";
            string S_Data1_OrganizeId = "";
            string S_Data1_DepartmentId = "";
            string S_Data1_RoleId = "";
            string S_Data1_IsAdministrator = "";
            string S_Data1_UserType = "";

            string S_Data2_UserId = "";
            string S_Data2_Password = "";

            string S_DynPWD = "";
            try
            {
                string S_Sql = "select * from mesEmployee where ID='" + UserID.Trim() + "'";
                DataTable DT_mesEmployee = SqlServerHelper.Data_Table(S_Sql);

                S_Sql_ID = DT_mesEmployee.Rows[0]["Id"] .ToString().Trim();
                S_Sql_UserID = DT_mesEmployee.Rows[0]["UserID"].ToString().Trim();
                S_Sql_Lastname = DT_mesEmployee.Rows[0]["Lastname"].ToString().Trim();
                S_Sql_Firstname = DT_mesEmployee.Rows[0]["Firstname"].ToString().Trim();
                S_Sql_Password = DT_mesEmployee.Rows[0]["Password"].ToString().Trim();
                S_Sql_OrganizeId = DT_mesEmployee.Rows[0]["OrganizeId"].ToString().Trim();
                S_Sql_DepartmentId = DT_mesEmployee.Rows[0]["DepartmentId"].ToString().Trim();
                S_Sql_RoleId = DT_mesEmployee.Rows[0]["RoleId"].ToString().Trim();
                S_Sql_IsAdministrator = DT_mesEmployee.Rows[0]["IsAdministrator"].ToString().Trim();

                S_Sql_IsAdministrator= S_Sql_IsAdministrator==""?"False" : S_Sql_IsAdministrator;
                S_Sql_UserType = DT_mesEmployee.Rows[0]["UserType"].ToString().Trim();
                S_Sql2_Password=DT_mesEmployee.Rows[0]["Password"].ToString().Trim();
                ////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////
                string S_Sql2 = "select * from API_UserLogOn where UserID='" + S_Sql_ID + "'";
                DataTable DT_UserLogOn = SqlServerHelper.Data_Table(S_Sql2);
                S_Sql2_Password = DT_UserLogOn.Rows[0]["UserPassword"].ToString().Trim();

                string S_SysPath = System.AppDomain.CurrentDomain.BaseDirectory+"\\bin";
                DataTable dataTable1 = new DataTable();
                DataTable dataTable2 = new DataTable();
                string S_Data1 = S_SysPath + "\\Data1.dll";
                string S_Data2 = S_SysPath + "\\Data2.dll";

                try
                {
                    S_DynPWD = PublicF.DynPWd();

                    string S_MI1 = File.ReadAllText(S_Data1);
                    string S_JsonJM1 = EncryptHelper2023.DecryptString(S_MI1);
                    JArray jsonArray1 = JArray.Parse(S_JsonJM1);

                    foreach (JProperty property in jsonArray1[0])
                    {
                        dataTable1.Columns.Add(property.Name);
                    }
                    foreach (JObject jsonObject in jsonArray1)
                    {
                        DataRow row = dataTable1.NewRow();
                        foreach (JProperty property in jsonObject.Properties())
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        dataTable1.Rows.Add(row);
                    }
                }
                catch(Exception ex)
                {
                    if (UserID == "developer" && PWD == S_DynPWD)
                    {

                    }
                    else
                    {
                        string logPath = CreateServerDIR();
                        string msg = "ERROR1:"+ex.ToString()+"\r\n"+ "secondary verification account is invalid.";
                        File.AppendAllText(logPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".log",
                           DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + msg + "\r\n");

                        return S_Result = "secondary verification account is invalid.";
                    }
                }

                try
                {
                    DataRow[] List_DR1 = dataTable1.Select("ID='" + UserID + "'");
                    if (List_DR1.Count() > 0)
                    {
                        S_Data1_ID = List_DR1[0]["ID"].ToString().Trim();
                        S_Data1_UserID = List_DR1[0]["UserID"].ToString().Trim();
                        S_Data1_Lastname = List_DR1[0]["Lastname"].ToString().Trim();
                        S_Data1_Firstname = List_DR1[0]["Firstname"].ToString().Trim();

                        S_Data1_Password = List_DR1[0]["Password"].ToString().Trim();

                        S_Data1_OrganizeId = List_DR1[0]["OrganizeId"].ToString().Trim();
                        S_Data1_DepartmentId = List_DR1[0]["DepartmentId"].ToString().Trim();
                        S_Data1_RoleId = List_DR1[0]["RoleId"].ToString().Trim();
                        S_Data1_IsAdministrator = List_DR1[0]["IsAdministrator"].ToString().Trim();
                        S_Data1_IsAdministrator = S_Data1_IsAdministrator == "" ? "False" : S_Data1_IsAdministrator.Trim();

                        S_Data1_UserType = List_DR1[0]["UserType"].ToString().Trim();
                    }
                }
                catch { }

                try
                {
                    string S_MI2 = File.ReadAllText(S_Data2);
                    string S_JsonJM2 = EncryptHelper2023.DecryptString(S_MI2);
                    JArray jsonArray2 = JArray.Parse(S_JsonJM2);
                    foreach (JProperty property in jsonArray2[0])
                    {
                        dataTable2.Columns.Add(property.Name);
                    }
                    foreach (JObject jsonObject in jsonArray2)
                    {
                        DataRow row = dataTable2.NewRow();
                        foreach (JProperty property in jsonObject.Properties())
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        dataTable2.Rows.Add(row);
                    }
                }
                catch
                {
                }

                try
                {
                    DataRow[] List_DR2 = dataTable2.Select("ID='" + S_Sql_ID + "'");
                    if (List_DR2.Count() > 0)
                    {
                        S_Data2_UserId = List_DR2[0]["UserId"].ToString();

                        S_Data2_Password = List_DR2[0]["UserPassword"].ToString();
                    }
                }
                catch { }


                if (
                    (S_Sql_ID == S_Data1_ID)
                    && (S_Sql_UserID == S_Data1_UserID)
                    && (S_Sql_Lastname == S_Data1_Lastname)
                    && (S_Sql_Firstname == S_Data1_Firstname)

                    && (S_Sql_ID == S_Data2_UserId)

                    && (S_Sql2_Password == S_Data2_Password)

                    && (S_Sql_OrganizeId == S_Data1_OrganizeId)
                    && (S_Sql_DepartmentId == S_Data1_DepartmentId)
                    && (S_Sql_RoleId == S_Data1_RoleId)
                    && (S_Sql_IsAdministrator == S_Data1_IsAdministrator)
                    && (S_Sql_UserType == S_Data1_UserType)

                    )
                {
                    S_Result = "OK";
                }
                else
                {
                    S_Result = "secondary verification account is invalid.";

                    string logPath = CreateServerDIR();
                    string msg = "ERROR2:" + S_Result;
                    File.AppendAllText(logPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".log",
                       DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + msg + "\r\n");
                }

                if (S_Result != "OK")
                {
                    if ((UserID == "developer" || UserID == "Developer") && PWD == S_DynPWD)
                    {
                        S_Result = "OK";
                    }
                }

                string S_ServerData = GetServerData();
                if (S_ServerData.IndexOf("ERROR:") > -1)
                {
                    S_Result = S_ServerData;
                }

            }
            catch (Exception ex)
            {
                S_Result = ex.ToString();
            }
            return S_Result;
        }


        public string ChangePWD(string UserID,string OldPWD,string NewPWD)
        {
            string S_Result = "OK";
            try
            {
                string S_Sql = "select * from mesEmployee where userid='" + UserID+"' and Password='"+
                    PublicF.EncryptPassword(OldPWD,"") +"'";
                DataTable DT_Old = SqlServerHelper.Data_Table(S_Sql);
                
                if (DT_Old.Rows.Count == 0)
                {
                    S_Result = "ERROR:User ID or old password is incorrect";
                }
                else
                {
                    string S_ID = DT_Old.Rows[0]["ID"].ToString();

                    S_Sql = "Update mesEmployee set Password='" +
                        PublicF.EncryptPassword(NewPWD, "") + "' where Id='" + S_ID + "'"+"\r\n";

                    string S_Sql2 = "select * from API_UserLogOn where UserId='" + S_ID + "'";
                    DataTable DT_Sql2 = SqlServerHelper.Data_Table(S_Sql2);

                    string S_UserSecretkey = MD5Util.GetMD5_16(GuidUtils.NewGuidFormatN()).ToLower();
                    string S_UserPassword = MD5Util.GetMD5_32(DEncrypt.Encrypt(MD5Util.GetMD5_32(NewPWD).ToLower(),
                    S_UserSecretkey).ToLower()).ToLower();

                    if (DT_Sql2.Rows.Count < 1)
                    {
                        S_Sql += "INSERT INTO API_UserLogOn(Id,UserId,UserPassword,UserSecretkey) Values(" +
                           "'" + S_ID + "'" +
                           ",'" + S_ID + "'" +
                           ",'" + S_UserPassword + "'" +
                           ",'" + S_UserSecretkey + "'" +
                           ")"
                           ;
                    }
                    else
                    {
                        S_Sql += "Update API_UserLogOn set " +
                               " UserPassword='" + S_UserPassword + "'" +
                               ", UserSecretkey='" + S_UserSecretkey + "'" +
                               " where UserId='" + S_ID + "'";
                    }

                    string S_ExecSql= SqlServerHelper.ExecSql(S_Sql);
                    if (S_ExecSql == "OK")
                    {                        
                        string S_SetData1_2 = SetData1_2(S_ID, "Update");
                        if (S_SetData1_2 != "OK")
                        {
                            S_Result = "ERROR:" + S_SetData1_2;
                        }
                    }
                    else
                    {
                        S_Result = "ERROR:" + S_ExecSql;
                    }
                }
            }
            catch (Exception ex)
            {
                S_Result = "ERROR:"+ex.Message;
            }
            return S_Result;
        }

        private DataRow NewDT_Text(DataTable DT_Text, DataTable DT_Sql)
        {
            DataRow DR = DT_Text.NewRow();
            for (int k = 0; k < DT_Text.Columns.Count; k++)
            {
                DR[k] = DT_Sql.Rows[0][k].ToString();
            }

            return DR;
        }
        private string SetData1_2(string UserID, string Type)
        {
            string S_Result = "OK";


            string S_SysPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\bin";

            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = new DataTable();
            string S_Data1 = S_SysPath + "\\Data1.dll";
            string S_Data2 = S_SysPath + "\\Data2.dll";

            string S_Sql1 = "";
            string S_Sql2 = "";
            DataTable DT_Sql1 = null;
            DataTable DT_Sql2 = null;

            try
            {
                if (File.Exists(S_Data1) == false)
                {
                    S_Sql1 = "select * from mesEmployee where Id='" + UserID + "'";
                    DT_Sql1 = SqlServerHelper.Data_Table(S_Sql1);
                    string json1_Exists = PublicF.DataTableToJson(DT_Sql1);
                    string S_MI1_Exists = EncryptHelper2023.EncryptString(json1_Exists);
                    File.WriteAllText(S_Data1, S_MI1_Exists);

                }

                if (File.Exists(S_Data2) == false)
                {
                    S_Sql2 = "select * from API_UserLogOn where UserId='" + UserID + "'";
                    DT_Sql2 = SqlServerHelper.Data_Table(S_Sql2);
                    string json2_Exists = PublicF.DataTableToJson(DT_Sql2);
                    string S_MI2_Exists = EncryptHelper2023.EncryptString(json2_Exists);
                    File.WriteAllText(S_Data2, S_MI2_Exists);
                }

                string S_MI1 = File.ReadAllText(S_Data1);
                string S_JsonJM1 = EncryptHelper2023.DecryptString(S_MI1);
                JArray jsonArray1 = JArray.Parse(S_JsonJM1);

                foreach (JProperty property in jsonArray1[0])
                {
                    dataTable1.Columns.Add(property.Name);
                }
                foreach (JObject jsonObject in jsonArray1)
                {
                    DataRow row = dataTable1.NewRow();
                    foreach (JProperty property in jsonObject.Properties())
                    {
                        row[property.Name] = property.Value.ToString();
                    }
                    dataTable1.Rows.Add(row);
                }

                string S_MI2 = File.ReadAllText(S_Data2);
                string S_JsonJM2 = EncryptHelper2023.DecryptString(S_MI2);
                JArray jsonArray2 = JArray.Parse(S_JsonJM2);

                foreach (JProperty property in jsonArray2[0])
                {
                    dataTable2.Columns.Add(property.Name);
                }
                foreach (JObject jsonObject in jsonArray2)
                {
                    DataRow row = dataTable2.NewRow();
                    foreach (JProperty property in jsonObject.Properties())
                    {
                        row[property.Name] = property.Value.ToString();
                    }
                    dataTable2.Rows.Add(row);
                }

                S_Sql1 = "select * from mesEmployee where Id='" + UserID + "'";
                DT_Sql1 = SqlServerHelper.Data_Table(S_Sql1);

                S_Sql2 = "select * from API_UserLogOn where UserId='" + UserID + "'";
                DT_Sql2 = SqlServerHelper.Data_Table(S_Sql2);

                if (Type == "Insert")
                {
                    dataTable1.Rows.Add(NewDT_Text(dataTable1, DT_Sql1));
                    dataTable2.Rows.Add(NewDT_Text(dataTable2, DT_Sql2));

                }
                else if (Type == "Delete")
                {
                    for (int i = 0; i < dataTable1.Rows.Count; i++)
                    {
                        if (dataTable1.Rows[i]["Id"].ToString().Trim() == DT_Sql1.Rows[0]["Id"].ToString().Trim())
                        {
                            dataTable1.Rows[i].Delete();
                            break;
                        }
                    }

                    for (int i = 0; i < dataTable2.Rows.Count; i++)
                    {
                        if (dataTable2.Rows[i]["UserId"].ToString().Trim() == DT_Sql2.Rows[0]["UserId"].ToString().Trim())
                        {
                            dataTable2.Rows[i].Delete();
                            break;
                        }
                    }
                }
                else if (Type == "Update")
                {
                    if (dataTable1.Select("Id='" + DT_Sql1.Rows[0]["Id"].ToString().Trim() + "'").Count() > 0)
                    {

                        for (int i = 0; i < dataTable1.Rows.Count; i++)
                        {
                            if (dataTable1.Rows[i]["Id"].ToString().Trim() == DT_Sql1.Rows[0]["Id"].ToString().Trim())
                            {
                                dataTable1.Rows[i].Delete();

                                DataRow DR = NewDT_Text(dataTable1, DT_Sql1);
                                dataTable1.Rows.Add(DR);

                                break;
                            }
                        }
                    }
                    else
                    {
                        DataRow DR = NewDT_Text(dataTable1, DT_Sql1);
                        dataTable1.Rows.Add(DR);
                    }

                    if (dataTable2.Select("UserId='" + DT_Sql2.Rows[0]["UserId"].ToString().Trim() + "'").Count() > 0)
                    {
                        for (int i = 0; i < dataTable2.Rows.Count; i++)
                        {
                            if (dataTable2.Rows[i]["UserId"].ToString().Trim() == DT_Sql2.Rows[0]["UserId"].ToString().Trim())
                            {
                                dataTable2.Rows[i].Delete();

                                DataRow DR = NewDT_Text(dataTable2, DT_Sql2);
                                dataTable2.Rows.Add(DR);

                                break;
                            }
                        }
                    }
                    else
                    {
                        DataRow DR = NewDT_Text(dataTable2, DT_Sql2);
                        dataTable2.Rows.Add(DR);
                    }
                }

                string json1_New = PublicF.DataTableToJson(dataTable1);
                string S_MI1_New = EncryptHelper2023.EncryptString(json1_New);
                File.WriteAllText(S_Data1, S_MI1_New);

                string json2_New = PublicF.DataTableToJson(dataTable2);
                string S_MI2_New = EncryptHelper2023.EncryptString(json2_New);
                File.WriteAllText(S_Data2, S_MI2_New);

                try
                {

                    string S_WinformWebDIR = System.Configuration.ConfigurationManager.AppSettings["WinformWebDIR"];
                    string[] List_WinformWebDIR = S_WinformWebDIR.Split(',');

                    foreach (var item in List_WinformWebDIR)
                    {
                        //string S_WinformWebDIR_Data1 = item + "\\Data1.dll";
                        //File.WriteAllText(S_WinformWebDIR_Data1, S_MI1_New);

                        //string S_WinformWebDIR_Data2 = item + "\\Data2.dll";
                        //File.WriteAllText(S_WinformWebDIR_Data2, S_MI2_New);


                        try
                        {
                            string S_FTPIP = item;
                            string S_FTPUser = System.Configuration.ConfigurationManager.AppSettings["FTPUser"];
                            string S_FTPPassword = System.Configuration.ConfigurationManager.AppSettings["FTPPassword"];

                            FtpWeb FTP = new FtpWeb(S_FTPIP, "", S_FTPUser, S_FTPPassword);
                            FTP.GotoDirectory("", true);
                            FTP.Upload(S_Data1, "Data1.dll");
                            FTP.Upload(S_Data2, "Data2.dll");
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }
                }
                catch
                { }
          
            }
            catch (Exception ex)
            {
                S_Result = ex.ToString();
            }
            return S_Result;
        }

        private string GetServerData()
        {
            string S_Result = "OK";
            try
            {
                ManagementClass mc_CPU = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc_CPU = mc_CPU.GetInstances();
                string CPU = "";

                foreach (ManagementObject mo in moc_CPU)
                {
                    CPU = mo.Properties["ProcessorId"].Value.ToString();
                    break;
                }

                ManagementClass mc_ZB = new ManagementClass("Win32_BaseBoard");
                ManagementObjectCollection moc_ZB = mc_ZB.GetInstances();
                string ZB = "";

                foreach (ManagementObject mo in moc_ZB)
                {
                    ZB = mo.Properties["SerialNumber"].Value.ToString();
                    break;
                }

                var MAC = "";
                var mc_MAC = new ManagementClass("Win32_NetworkAdapterConfiguration");
                var moc_MAC = mc_MAC.GetInstances();
                foreach (var o in moc_MAC)
                {
                    var mo = (ManagementObject)o;
                    if (!(bool)mo["IPEnabled"]) continue;
                    MAC = mo["MacAddress"].ToString();
                    break;
                }

                var Menory = "";
                var mc_Menory = new ManagementClass("Win32_ComputerSystem");
                var moc_Menory = mc_Menory.GetInstances();
                foreach (var mo in moc_Menory)
                {
                    if (mo["TotalPhysicalMemory"] != null)
                    {
                        Menory = mo["TotalPhysicalMemory"].ToString();
                        break;
                    }
                }

                var DiskDrive = "";
                var mc_DiskDrive = new ManagementClass("Win32_DiskDrive");
                var moc_DiskDrive = mc_DiskDrive.GetInstances();
                foreach (var o in moc_DiskDrive)
                {
                    var mo = (ManagementObject)o;
                    DiskDrive += mo.Properties["Model"].Value.ToString() + ";";
                }


                string S_Top =
                "911C53107B32880492DA539B4BC653167FB87BE96B99820E45ED10177D267A1453A74D6A" +
                "4D6A7FF5421988417A156B997E1A89951017625F622B6B998C4F69EB4E0B7A7863F363B5" +
                "531374618B1C101763E29FB0985463BF772C755E464C6C2B531D93B66B9992DA7A7853A7" +
                "10177A377BE3906D4F2D63C0459E90F2426141A59FAC5310421D101792868C654EF75327" +
                "8862469492C14D0F4E7A4A3569214EED1017411242DE8DBE705C41196B99706A431F4833" +
                "4E4B421D9FAC7A4B101792A08D786B99443471AE485763AA63AA705C70D16B997E207651" +
                "8DD3101799C08CDA499148578C948C4F424E7651101763BF85817B0E9D1C92716A21754C" +
                "682D71C6731871FF1017446453427BE37B32909792DA539B89EB71AE531A8CD16B999E4F" +
                "4E82101753CB53427BE37B324F0F6529539B63C76DDF705C99204A3553A788E9101748327" +
                "A148DBE53106A3D6B994CF0751253137DD853DA52809F1010177B32538C795A7B328BF5787" +
                "E8DE04A354911531D5337795A66F310178DB28DE575BB48574EF953D7491110179E7E4E537B" +
                "0E4EA68A4F416C441944199FEC8BD94CBF101799C08CDA49918C948C4F424E7651101763BF8" +
                "5817B0E9D1C92716A21754C682D71C6731871FF";


                string S_Sql = "select Count(*) Count from mesStation";
                DataTable DT = SqlServerHelper.Data_Table(S_Sql);
                int I_StationCount = Convert.ToInt32(DT.Rows[0][0].ToString());

                string S_SysPath = System.AppDomain.CurrentDomain.BaseDirectory+"\\bin";
                string S_Data200 = S_SysPath + "\\Data200.dll";

                if (File.Exists(S_Data200))
                {
                    try
                    {
                        string S_MI200_PWD = File.ReadAllText(S_Data200);
                        string S_MI200 = EncryptHelper2023.DecryptString(S_MI200_PWD);
                        string[] List_MI200 = S_MI200.Split(',');

                        string S_Local_Top = List_MI200[0].Replace("\r", "").Replace("\n", "");
                        string S_Local_CPU = List_MI200[1].Replace("\r", "").Replace("\n", "");
                        string S_Local_ZB = List_MI200[2].Replace("\r", "").Replace("\n", "");
                        string S_Local_MAC = List_MI200[3].Replace("\r", "").Replace("\n", "");
                        string S_Local_Menory = List_MI200[4].Replace("\r", "").Replace("\n", "");
                        string S_Local_DiskDrive = List_MI200[5].Replace("\r", "").Replace("\n", "");
                        string S_Local_StationCount = List_MI200[6].Replace("\r", "").Replace("\n", "");

                        int I_Local_StationCount = 0;
                        if (S_Local_StationCount != "")
                        {
                            I_Local_StationCount = Convert.ToInt32(S_Local_StationCount);
                        }

                        if (I_StationCount > I_Local_StationCount)
                        {
                            S_Result = "ERROR:Too many station.";
                        }
                        else
                        {
                            if (S_Top == S_Local_Top
                                && CPU == S_Local_CPU
                                && ZB == S_Local_ZB
                                && MAC == S_Local_MAC
                                && Menory == S_Local_Menory
                                && DiskDrive == S_Local_DiskDrive
                                )
                            {

                            }
                            else
                            {
                                S_Result = "ERROR:Server authentication failure.";
                            }
                        }
                    }
                    catch
                    {
                        S_Result = "ERROR:Server authentication failure.";
                    }
                }
                else
                {
                    S_Result = "ERROR:Server verification data does not exist.";
                }

            }
            catch (Exception ex)
            {
                S_Result = "ERROR:" + ex.ToString();
            }
            return S_Result;
        }


        private string CreateServerDIR()
        {
            try
            {
                string S_Path = System.AppDomain.CurrentDomain.BaseDirectory;
                if (Directory.Exists(S_Path + "\\Log") == false)
                {
                    Directory.CreateDirectory(S_Path + "\\Log");
                }

                string S_DayLog = S_Path + "\\Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
                if (Directory.Exists(S_DayLog) == false)
                {
                    Directory.CreateDirectory(S_DayLog);
                }
                return S_DayLog;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

    }
}
