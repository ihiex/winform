using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;
using App.DBUtility;

namespace App.DBServerDAL
{
    public class mesUnitDetailDAL
    {
        public string Insert(mesUnitDetail model)
        {
            string S_Result = "";

            try
            {
                object obj = SqlServerHelper.ExecuteScalar(
                "INSERT INTO mesUnitDetail(" +
                "UnitID," +
                //  "ProductionOrderID," +
                //  "RMAID," +
                //  "LooperCount," +
                //  "KitSerialNumber," +
                //  "InmostPackageID," +
                //  "OutmostPackageID," +
                "reserved_01," +
                "reserved_02," +
                "reserved_03," +
                "reserved_04," +
                "reserved_05" +
                //   "reserved_06," +
                //   "reserved_07," +
                //   "reserved_08," +
                //   "reserved_09," +
                //   "reserved_10," +
                //   "reserved_11," +
                //   "reserved_12," +
                //   "reserved_13," +
                //   "reserved_14," +
                //   "reserved_15," +
                //   "reserved_16," +
                //   "reserved_17," +
                //   "reserved_18," +
                //   "reserved_19," +
                //   "reserved_20" +
                ") VALUES " +

                "(" +
                "@UnitID," +
                //"@ProductionOrderID," +
                //"@RMAID," +
                //"@LooperCount," +
                //"@KitSerialNumber," +
                //"@InmostPackageID," +
                //"@OutmostPackageID," +
                "@reserved_01," +
                "@reserved_02," +
                "@reserved_03," +
                "@reserved_04," +
                "@reserved_05" +
                //"@reserved_06," +
                //"@reserved_07," +
                //"@reserved_08," +
                //"@reserved_09," +
                //"@reserved_10," +
                //"@reserved_11," +
                //"@reserved_12," +
                //"@reserved_13," +
                //"@reserved_14," +
                //"@reserved_15," +
                //"@reserved_16," +
                //"@reserved_17," +
                //"@reserved_18," +
                //"@reserved_19," +
                //"@reserved_20" +

                ");SELECT @@identity"
                , new SqlParameter("@UnitID", model.UnitID)
                //, new SqlParameter("@ProductionOrderID", model.ProductionOrderID)
                //, new SqlParameter("@RMAID", model.RMAID)
                //, new SqlParameter("@LooperCount", model.LooperCount)
                //, new SqlParameter("@KitSerialNumber", model.KitSerialNumber)
                //, new SqlParameter("@InmostPackageID", model.InmostPackageID)
                //, new SqlParameter("@OutmostPackageID", model.OutmostPackageID)
                , new SqlParameter("@reserved_01", model.reserved_01)
                , new SqlParameter("@reserved_02", model.reserved_02)
                , new SqlParameter("@reserved_03", model.reserved_03)
                , new SqlParameter("@reserved_04", model.reserved_04)
                , new SqlParameter("@reserved_05", model.reserved_05)
                //, new SqlParameter("@reserved_06", model.reserved_06)
                //, new SqlParameter("@reserved_07", model.reserved_07)
                //, new SqlParameter("@reserved_08", model.reserved_08)
                //, new SqlParameter("@reserved_09", model.reserved_09)
                //, new SqlParameter("@reserved_10", model.reserved_10)
                //, new SqlParameter("@reserved_11", model.reserved_11)
                //, new SqlParameter("@reserved_12", model.reserved_12)
                //, new SqlParameter("@reserved_13", model.reserved_13)
                //, new SqlParameter("@reserved_14", model.reserved_14)
                //, new SqlParameter("@reserved_15", model.reserved_15)
                //, new SqlParameter("@reserved_16", model.reserved_16)
                //, new SqlParameter("@reserved_17", model.reserved_17)
                //, new SqlParameter("@reserved_18", model.reserved_18)
                //, new SqlParameter("@reserved_19", model.reserved_19)
                //, new SqlParameter("@reserved_20", model.reserved_20)
                );

                S_Result = obj.ToString();
                if (S_Result == "") { S_Result = "0"; }
            }
            catch (Exception ex)
            {
                S_Result = "E:" + ex.Message;
            }
            return S_Result;
        }

        public bool Delete(int id)
        {
            int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM mesUnitDetail WHERE ID = @id", new SqlParameter("@id", id));
            return rows > 0;
        }

        public string Update(mesUnitDetail model)
        {
            string S_Result = "";
            try
            {
                string sql = "UPDATE mesUnitDetail SET " +
                "UnitID=@UnitID," +
                //"ProductionOrderID=@ProductionOrderID," +
                //"RMAID=@RMAID,LooperCount=@LooperCount," +
                //"KitSerialNumber=@KitSerialNumber," +
                //"InmostPackageID=@InmostPackageID," +
                //"OutmostPackageID=@OutmostPackageID," +
                "reserved_01=@reserved_01," +
                "reserved_02=@reserved_02," +
                "reserved_03=@reserved_03," +
                "reserved_04=@reserved_04," +
                "reserved_05=@reserved_05 " +
                //"reserved_06=@reserved_06," +
                //"reserved_07=@reserved_07," +
                //"reserved_08=@reserved_08," +
                //"reserved_09=@reserved_09," +
                //"reserved_10=@reserved_10," +
                //"reserved_11=@reserved_11," +
                //"reserved_12=@reserved_12," +
                //"reserved_13=@reserved_13," +
                //"reserved_14=@reserved_14," +
                //"reserved_15=@reserved_15," +
                //"reserved_16=@reserved_16," +
                //"reserved_17=@reserved_17," +
                //"reserved_18=@reserved_18," +
                //"reserved_19=@reserved_19," +
                //"reserved_20=@reserved_20 " +
                "WHERE ID=@ID";

                int rows = SqlServerHelper.ExecuteNonQuery(sql
                    , new SqlParameter("@ID", model.ID)
                    , new SqlParameter("@UnitID", model.UnitID)
                    //, new SqlParameter("@ProductionOrderID", model.ProductionOrderID)
                    //, new SqlParameter("@RMAID", model.RMAID)
                    //, new SqlParameter("@LooperCount", model.LooperCount)
                    //, new SqlParameter("@KitSerialNumber", model.KitSerialNumber)
                    //, new SqlParameter("@InmostPackageID", model.InmostPackageID)
                    //, new SqlParameter("@OutmostPackageID", model.OutmostPackageID)
                    , new SqlParameter("@reserved_01", model.reserved_01)
                    , new SqlParameter("@reserved_02", model.reserved_02)
                    , new SqlParameter("@reserved_03", model.reserved_03)
                    , new SqlParameter("@reserved_04", model.reserved_04)
                    , new SqlParameter("@reserved_05", model.reserved_05)
                    //, new SqlParameter("@reserved_06", model.reserved_06)
                    //, new SqlParameter("@reserved_07", model.reserved_07)
                    //, new SqlParameter("@reserved_08", model.reserved_08)
                    //, new SqlParameter("@reserved_09", model.reserved_09)
                    //, new SqlParameter("@reserved_10", model.reserved_10)
                    //, new SqlParameter("@reserved_11", model.reserved_11)
                    //, new SqlParameter("@reserved_12", model.reserved_12)
                    //, new SqlParameter("@reserved_13", model.reserved_13)
                    //, new SqlParameter("@reserved_14", model.reserved_14)
                    //, new SqlParameter("@reserved_15", model.reserved_15)
                    //, new SqlParameter("@reserved_16", model.reserved_16)
                    //, new SqlParameter("@reserved_17", model.reserved_17)
                    //, new SqlParameter("@reserved_18", model.reserved_18)
                    //, new SqlParameter("@reserved_19", model.reserved_19)
                    //, new SqlParameter("@reserved_20", model.reserved_20)           
                    );
                S_Result = "OK";
            }
            catch (Exception ex)
            {
                S_Result = "E:" + ex.Message;
            }
            return S_Result;
        }

        public mesUnitDetail Get(int id)
        {
            DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesUnitDetail WHERE ID=@ID", new SqlParameter("@ID", id));
            if (dt.Rows.Count > 1)
            {
                throw new Exception("more than 1 row was found");
            }
            if (dt.Rows.Count <= 0)
            {
                return null;
            }
            DataRow row = dt.Rows[0];
            mesUnitDetail model = ToModel(row);
            return model;
        }

        private static mesUnitDetail ToModel(DataRow row)
        {
            mesUnitDetail model = new mesUnitDetail();
            model.ID = (int)row["ID"];
            model.UnitID = (int)row["UnitID"];
            //model.ProductionOrderID = (int)row["ProductionOrderID"];
            //model.RMAID = (int)row["RMAID"];
            //model.LooperCount = (int)row["LooperCount"];
            //model.KitSerialNumber = (string)row["KitSerialNumber"];
            //model.InmostPackageID = (int)row["InmostPackageID"];
            //model.OutmostPackageID = (int)row["OutmostPackageID"];
            model.reserved_01 = (string)row["reserved_01"];
            model.reserved_02 = (string)row["reserved_02"];
            model.reserved_03 = (string)row["reserved_03"];
            model.reserved_04 = (string)row["reserved_04"];
            model.reserved_05 = (string)row["reserved_05"];
            //model.reserved_06 = (string)row["reserved_06"];
            //model.reserved_07 = (string)row["reserved_07"];
            //model.reserved_08 = (string)row["reserved_08"];
            //model.reserved_09 = (string)row["reserved_09"];
            //model.reserved_10 = (string)row["reserved_10"];
            //model.reserved_11 = (string)row["reserved_11"];
            //model.reserved_12 = (string)row["reserved_12"];
            //model.reserved_13 = (string)row["reserved_13"];
            //model.reserved_14 = (string)row["reserved_14"];
            //model.reserved_15 = (string)row["reserved_15"];
            //model.reserved_16 = (string)row["reserved_16"];
            //model.reserved_17 = (string)row["reserved_17"];
            //model.reserved_18 = (string)row["reserved_18"];
            //model.reserved_19 = (string)row["reserved_19"];
            //model.reserved_20 = (string)row["reserved_20"];
            return model;
        }

        public IEnumerable<mesUnitDetail> ListAll(string S_Where)
        {
            List<mesUnitDetail> list = new List<mesUnitDetail>();
            DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesUnitDetail  " + S_Where);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(ToModel(row));
            }
            return list;
        }

        public DataSet MesGetBatchIDByBarcodeSN(string BarcodeSN)
        {
            string strSql = string.Format("SELECT top 1 reserved_02 FROM mesUnitDetail WHERE reserved_01='{0}' and reserved_04=1", BarcodeSN);
            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }
    }
}
