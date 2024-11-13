using App.DBUtility;
using App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DBServerDAL
{
    public class mesPackageDAL
    {
        public int Insert(mesPackage model)
        {
            string S_Sql = "select isnull(MAX(ID),0)+1 MaxID from mesPackage";
            DataTable DT = SqlServerHelper.ExecuteDataTable(S_Sql);
            string S_MaxID = DT.Rows[0][0].ToString();
            if (S_MaxID == "" || S_MaxID == null) { S_MaxID = "1"; }

            int I_MaxID = Convert.ToInt32(S_MaxID);
            model.ID = I_MaxID;

            object obj = SqlServerHelper.ExecuteScalar(
                "INSERT INTO mesPackage(ID,SerialNumber,StationID,EmployeeID,CreationTime,CurrentCount,StatusID,LastUpdate,ParentID,Stage,CurrProductionOrderID,CurrPartID) " +
                "VALUES (@ID,@SerialNumber,@StationID,@EmployeeID,@CreationTime,@CurrentCount,@StatusID,@LastUpdate,@ParentID,@Stage,@CurrProductionOrderID,@CurrPartID);SELECT @@identity"
                , new SqlParameter("@ID", I_MaxID)
                , new SqlParameter("@SerialNumber", model.SerialNumber)
                , new SqlParameter("@StationID", model.StationID)
                , new SqlParameter("@EmployeeID", model.EmployeeID)
                , new SqlParameter("@CreationTime", model.CreationTime)
                , new SqlParameter("@CurrentCount", model.CurrentCount)
                , new SqlParameter("@StatusID", model.StatusID)
                , new SqlParameter("@LastUpdate", model.LastUpdate)
                , new SqlParameter("@ParentID", model.ParentID)
                , new SqlParameter("@Stage", model.Stage)
                , new SqlParameter("@CurrProductionOrderID", model.CurrProductionOrderID)
                , new SqlParameter("@CurrPartID", model.CurrPartID)
            );
            return Convert.ToInt32(obj);
        }

        public bool Delete(int id)
        {
            int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM mesPackage WHERE ID = @id", new SqlParameter("@id", id));
            return rows > 0;
        }

        public bool Update(mesPackage model)
        {
            string sql = @"UPDATE mesPackage SET SerialNumber=@SerialNumber,StationID=@StationID,EmployeeID=@EmployeeID,CreationTime=@CreationTime
                            , CurrentCount = @CurrentCount, StatusID = @StatusID, LastUpdate = @LastUpdate, ParentID = @ParentID
                            , Stage = @Stage, CurrProductionOrderID = @CurrProductionOrderID, CurrPartID = @CurrPartID WHERE ID=@ID";
            int rows = SqlServerHelper.ExecuteNonQuery(sql
                , new SqlParameter("@ID", model.ID)
                , new SqlParameter("@SerialNumber", model.SerialNumber)
                , new SqlParameter("@StationID", model.StationID)
                , new SqlParameter("@EmployeeID", model.EmployeeID)
                , new SqlParameter("@CreationTime", model.CreationTime)
                , new SqlParameter("@CurrentCount", model.CurrentCount)
                , new SqlParameter("@StatusID", model.StatusID)
                , new SqlParameter("@LastUpdate", model.LastUpdate)
                , new SqlParameter("@ParentID", model.ParentID)
                , new SqlParameter("@Stage", model.Stage)
                , new SqlParameter("@CurrProductionOrderID", model.CurrProductionOrderID)
                , new SqlParameter("@CurrPartID", model.CurrPartID)
            );
            return rows > 0;
        }

        public mesPackage Get(int id)
        {
            DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesPackage WHERE ID=@ID", new SqlParameter("@ID", id));
            if (dt.Rows.Count > 1)
            {
                throw new Exception("more than 1 row was found");
            }
            if (dt.Rows.Count <= 0)
            {
                return null;
            }
            DataRow row = dt.Rows[0];
            mesPackage model = ToModel(row);
            return model;
        }

        private static mesPackage ToModel(DataRow row)
        {
            mesPackage model = new mesPackage();
            model.ID = Convert.ToInt32(row["ID"].ToString());
            model.SerialNumber = (string)row["SerialNumber"];
            model.StationID = Convert.ToInt32(row["StationID"].ToString());
            model.EmployeeID = Convert.ToInt32(row["EmployeeID"].ToString());
            model.CreationTime = (DateTime)row["CreationTime"];
            model.CurrentCount = (int)row["CurrentCount"];
            model.StatusID = Convert.ToInt32(row["StatusID"].ToString());
            model.LastUpdate = (DateTime)row["LastUpdate"];
            model.ParentID = Convert.ToInt32(row["ParentID"].ToString());
            model.Stage = Convert.ToInt32(row["Stage"].ToString());
            model.CurrProductionOrderID = (int)row["CurrProductionOrderID"];
            model.CurrPartID = (int)row["CurrPartID"];
            //if (!string.IsNullOrEmpty(row["CurrentCount"].ToString()))
            //{
            //    model.CurrentCount = Convert.ToInt32(row["CurrentCount"].ToString());
            //}
            //model.StatusID = Convert.ToInt32(row["StatusID"].ToString());
            //model.LastUpdate = (DateTime)row["LastUpdate"];
            //if (!string.IsNullOrEmpty(row["ParentID"].ToString()))
            //{
            //    model.ParentID = Convert.ToInt32(row["ParentID"].ToString());
            //}
            //model.Stage = Convert.ToInt32(row["Stage"].ToString());
            //if (!string.IsNullOrEmpty(row["CurrProductionOrderID"].ToString()))
            //{
            //    model.CurrProductionOrderID = (int)row["CurrProductionOrderID"];
            //}
            //if (!string.IsNullOrEmpty(row["CurrPartID"].ToString()))
            //{
            //    model.CurrPartID = (int)row["CurrPartID"];
            //}
            return model;
        }

        public IEnumerable<mesPackage> ListAll(string S_Where)
        {
            List<mesPackage> list = new List<mesPackage>();
            DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesPackage " + S_Where);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(ToModel(row));
            }
            return list;
        }
    }
}
