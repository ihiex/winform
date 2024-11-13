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
    public class MesMaterialUnitDAL
    {
        public int Insert(MesMaterialUnit model)
        {
            object obj = SqlServerHelper.ExecuteScalar(@"INSERT INTO dbo.mesMaterialUnit
                   (SerialNumber
                   , PartID
                   , StatusID
                   , MaterialTypeID
                   , StationID
                   , EmployeeID
                   , LotCode
                   , MaterialDateCode
                   , DateCode
                   , TraceCode
                   , RollCode
                   , MPN
                   , VendorID
                   , Quantity
                   , RemainQTY
                   , CreationTime
                   , LineID
                   , LastUpdate
                   , ExpiringTime)
                VALUES
                   (@SerialNumber
                   , @PartID
                   , @StatusID
                   , @MaterialTypeID
                   , @StationID
                   , @EmployeeID
                   , @LotCode
                   , @MaterialDateCode
                   , @DateCode
                   , @TraceCode
                   , @RollCode
                   , @MPN
                   , @VendorID
                   , @Quantity
                   , @RemainQTY
                   , getdate()
                   , @LineID
                   , getdate()
                   , @ExpiringTime
                    ); SELECT @@identity"
                , new SqlParameter("@SerialNumber", model.SerialNumber)
                , new SqlParameter("@PartID", model.PartID)
                , new SqlParameter("@StatusID", model.StatusID)
                , new SqlParameter("@MaterialTypeID", model.MaterialTypeID)
                , new SqlParameter("@StationID", model.StationID)
                , new SqlParameter("@EmployeeID", model.EmployeeID)
                , new SqlParameter("@LotCode", model.LotCode)
                , new SqlParameter("@MaterialDateCode", model.MaterialDateCode)
                , new SqlParameter("@DateCode", model.DateCode)
                , new SqlParameter("@TraceCode", model.TraceCode)
                , new SqlParameter("@RollCode", model.RollCode)
                , new SqlParameter("@MPN", model.MPN)
                , new SqlParameter("@VendorID", model.VendorID)
                , new SqlParameter("@Quantity", model.Quantity)
                , new SqlParameter("@RemainQTY", model.RemainQTY)
                , new SqlParameter("@LineID", model.LineID)
                , new SqlParameter("@ExpiringTime", model.ExpiringTime)
            );
            return Convert.ToInt32(obj);
        }

        public int InserDetail(MesMaterialUnitDetail model)
        {
            object obj = SqlServerHelper.ExecuteScalar(@"INSERT INTO dbo.mesMaterialUnitDetail
                   (MaterialUnitID
                   ,LooperCount
                   ,Reserved_01
                   ,Reserved_02
                   ,Reserved_03
                   ,Reserved_04
                   ,Reserved_05)
             VALUES
		        (@MaterialUnitID
		        ,@LooperCount
		        ,@Reserved_01
		        ,@Reserved_02
		        ,@Reserved_03
		        ,@Reserved_04
		        ,@Reserved_05);SELECT @@identity"
                , new SqlParameter("@MaterialUnitID", model.MaterialUnitID)
                , new SqlParameter("@LooperCount", model.LooperCount)
                , new SqlParameter("@Reserved_01", model.Reserved_01)
                , new SqlParameter("@Reserved_02", model.Reserved_02)
                , new SqlParameter("@Reserved_03", model.Reserved_03)
                , new SqlParameter("@Reserved_04", model.Reserved_04)
                , new SqlParameter("@Reserved_05", model.Reserved_05)
            );
            return Convert.ToInt32(obj);
        }

        public int InserForParent(MesMaterialUnit model)
        {
            object obj = SqlServerHelper.ExecuteScalar(@"INSERT INTO dbo.mesMaterialUnit
                   (SerialNumber
                   , PartID
                   , StatusID
                   , MaterialTypeID
                   , StationID
                   , EmployeeID
                   , LotCode
                   , DateCode
                   , TraceCode
                   , MPN
                   , VendorID
                   , Quantity
                   , RemainQTY
                   , CreationTime
                   , LineID
                   , LastUpdate
                   , ExpiringTime
				   , ParentID
				   , RollCode)
                SELECT	@SerialNumber
				   , @PartID
                   , StatusID
                   , @MaterialTypeID
                   , @StationID
                   , @EmployeeID
                   , LotCode
                   , DateCode
                   , TraceCode
                   , MPN
                   , VendorID
                   , @Quantity
                   , @Quantity
                   , GETDATE()
                   , @LineID
                   , GETDATE()
                   , ExpiringTime
				   , @ParentID
				   , @RollCode FROM mesMaterialUnit WHERE ID=@ID; SELECT @@identity"
                , new SqlParameter("@SerialNumber", model.SerialNumber)
                , new SqlParameter("@MaterialTypeID", model.MaterialTypeID)
                , new SqlParameter("@PartID", model.PartID)
                , new SqlParameter("@StationID", model.StationID)
                , new SqlParameter("@EmployeeID", model.EmployeeID)
                , new SqlParameter("@Quantity", model.Quantity)
                , new SqlParameter("@RemainQTY", model.RemainQTY)
                , new SqlParameter("@LineID", model.LineID)
                , new SqlParameter("@ParentID", model.ParentID)
                , new SqlParameter("@RollCode", model.RollCode)
                , new SqlParameter("@ID", model.ParentID)
            );
            return Convert.ToInt32(obj);
        }

        public bool Delete(int id)
        {
            int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM mesMaterialUnit WHERE ID = @id", new SqlParameter("@id", id));
            return rows > 0;
        }

        public bool Update(MesMaterialUnit model)
        {
            string sql = @"UPDATE dbo.mesMaterialUnit
                           SET SerialNumber = @MesMaterialUnit
                              ,PartID = @PartID
                              ,StatusID = @StatusID
                              ,MaterialTypeID = @MaterialTypeID
                              ,StationID = @StationID
                              ,EmployeeID = @EmployeeID
                              ,LotCode = @LotCode
                              ,MaterialDateCode=@MaterialDateCode
                              ,DateCode = @DateCode
                              ,TraceCode = @TraceCode
                              ,MPN = @MPN
                              ,VendorID = @VendorID
                              ,Quantity = @Quantity
                              ,BalanceQty = @BalanceQty
                              ,LooperCount = @LooperCount
                              ,CreationTime = @CreationTime
                              ,FinishTime = @FinishTime
                              ,LineID = @LineID
                              ,LastUpdate = @LastUpdate
                              ,ProcessNameID = @ProcessNameID
                              ,ExpiringTime = @ExpiringTime
                              ,ParentID = @ParentID
                         WHERE ID=@ID";
            int rows = SqlServerHelper.ExecuteNonQuery(sql
                , new SqlParameter("@ID", model.ID)
                , new SqlParameter("@SerialNumber", model.SerialNumber)
                , new SqlParameter("@PartID", model.PartID)
                , new SqlParameter("@StatusID", model.StatusID)
                , new SqlParameter("@MaterialTypeID", model.MaterialTypeID)
                , new SqlParameter("@StationID", model.StationID)
                , new SqlParameter("@EmployeeID", model.EmployeeID)
                , new SqlParameter("@LotCode", model.LotCode)
                , new SqlParameter("@LotCode", model.MaterialDateCode)
                , new SqlParameter("@DateCode", model.DateCode)
                , new SqlParameter("@TraceCode", model.TraceCode)
                , new SqlParameter("@MPN", model.MPN)
                , new SqlParameter("@VendorID", model.VendorID)
                , new SqlParameter("@Quantity", model.Quantity)
                , new SqlParameter("@BalanceQty", model.BalanceQty)
                , new SqlParameter("@LooperCount", model.LooperCount)
                , new SqlParameter("@CreationTime", model.CreationTime)
                , new SqlParameter("@FinishTime", model.FinishTime)
                , new SqlParameter("@LineID", model.LineID)
                , new SqlParameter("@LastUpdate", model.LastUpdate)
                , new SqlParameter("@ProcessNameID", model.ProcessNameID)
                , new SqlParameter("@ExpiringTime", model.ExpiringTime)
                , new SqlParameter("@ParentID", model.ParentID)
            );
            return rows > 0;
        }

        public MesMaterialUnit Get(int id)
        {
            DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesMaterialUnit WHERE ID=@ID", new SqlParameter("@ID", id));
            if (dt.Rows.Count > 1)
            {
                throw new Exception("more than 1 row was found");
            }
            if (dt.Rows.Count <= 0)
            {
                return null;
            }
            DataRow row = dt.Rows[0];
            MesMaterialUnit model = ToModel(row);
            return model;
        }

        private static MesMaterialUnit ToModel(DataRow row)
        {
            MesMaterialUnit model = new MesMaterialUnit();
            model.ID = Convert.ToInt32(row["ID"].ToString());
            model.SerialNumber = (string)row["SerialNumber"];
            model.PartID = Convert.ToInt32(row["PartID"].ToString());
            model.StatusID = Convert.ToInt32(row["StatusID"].ToString());
            model.MaterialTypeID = Convert.ToInt32(row["MaterialTypeID"]);
            model.StationID = (int)row["StationID"];
            model.EmployeeID = Convert.ToInt32(row["EmployeeID"].ToString());
            model.LotCode = row["LotCode"].ToString();
            model.DateCode = row["DateCode"].ToString();
            model.TraceCode = row["TraceCode"].ToString();
            model.MPN = row["MPN"].ToString();
            model.VendorID = (int)row["VendorID"];
            model.Quantity = Convert.ToInt32(row["Quantity"]);
            model.BalanceQty = (int)row["BalanceQty"];
            model.LooperCount = Convert.ToInt32(row["LooperCount"].ToString());
            model.CreationTime = (DateTime)row["CreationTime"];
            model.FinishTime = (DateTime)row["FinishTime"];
            model.LineID = (int)row["LineID"];
            model.LastUpdate = (DateTime)row["LastUpdate"];
            model.ProcessNameID = (int)row["ProcessNameID"];
            model.ExpiringTime = (DateTime)row["ExpiringTime"];
            model.ParentID = (int)row["ParentID"];
            return model;
        }

        public IEnumerable<MesMaterialUnit> ListAll(string S_Where)
        {
            List<MesMaterialUnit> list = new List<MesMaterialUnit>();
            DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesMaterialUnit " + S_Where);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(ToModel(row));
            }
            return list;
        }

        public DataSet GetMesMaterialUnitByLotCode(string PartID,string LotCode)
        {
            string sql = string.Format(@"select * from mesMaterialUnit where SerialNumber='{0}' and partid={1}", LotCode, PartID);
            DataSet ds = SqlServerHelper.Data_Set(sql);
            return ds;
        }

        public int GetMesMaterialUseQTY(string MaterialUnitID)
        {
            int UseQTY = 0;
            string sql = string.Format(@"SELECT isnull(SUM(isnull(B.Quantity,0))+SUM(isnull(B.BalanceQty,0)),0) UseQTY 
                    FROM mesMaterialUnit B WHERE B.ParentID={0}", MaterialUnitID);
            DataTable dt = SqlServerHelper.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                UseQTY = Convert.ToInt32(dt.Rows[0]["UseQTY"].ToString());
            }
            return UseQTY;
        }

        public DataSet GetMesMaterialUnitData(string SerialNumber)
        {
            string sql = string.Format(@"select * from mesMaterialUnit where SerialNumber='{0}'", SerialNumber);
            DataSet ds = SqlServerHelper.Data_Set(sql);
            return ds;
        }
    }
}
