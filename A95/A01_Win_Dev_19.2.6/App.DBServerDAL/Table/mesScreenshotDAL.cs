using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
    public class mesScreenshotDAL
    {
        public int Insert(mesScreenshot model)
        {
            //if (model.MSG.Trim() != "")
            //{
            //    model.MSG = PublicF.EncryptPassword(model.MSG, "");
            //}

            object obj = SqlServerHelper.ExecuteScalar(
                "INSERT INTO mesScreenshot(LineID,StationID,PartID,ProductionOrderID,IP,PCName,IMGURL,MSG,Feedback,IsFeedback) VALUES" +
                " (@LineID,@StationID,@PartID,@ProductionOrderID,@IP,@PCName,@IMGURL,@MSG,@Feedback,@IsFeedback);SELECT @@identity"
                , new SqlParameter("@LineID", model.LineID)
                , new SqlParameter("@StationID", model.StationID)
                , new SqlParameter("@PartID", model.PartID)
                , new SqlParameter("@ProductionOrderID", model.ProductionOrderID)
                , new SqlParameter("@IP", model.IP)
                , new SqlParameter("@PCName", model.PCName)
                , new SqlParameter("@IMGURL", model.IMGURL)
                , new SqlParameter("@MSG", model.MSG)
                , new SqlParameter("@Feedback", model.Feedback)
                , new SqlParameter("@IsFeedback", model.IsFeedback)
            );
            return Convert.ToInt32(obj);
        }

        public bool Delete(int id)
        {
            int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM mesScreenshot WHERE ID = @id", new SqlParameter("@id", id));
            return rows > 0;
        }

        public bool Update(mesScreenshot model)
        {
            string sql = "UPDATE mesScreenshot SET IsFeedback=@IsFeedback WHERE ID=@ID";
            int rows = SqlServerHelper.ExecuteNonQuery(sql
                , new SqlParameter("@ID", model.ID)                
                , new SqlParameter("@IsFeedback", model.IsFeedback)

            );
            return rows > 0;
        }

        public mesScreenshot Get(int id)
        {
            DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesScreenshot WHERE ID=@ID", new SqlParameter("@ID", id));
            if (dt.Rows.Count > 1)
            {
                throw new Exception("more than 1 row was found");
            }
            if (dt.Rows.Count <= 0)
            {
                return null;
            }
            DataRow row = dt.Rows[0];
            mesScreenshot model = ToModel(row);
            return model;
        }

        private static mesScreenshot ToModel(DataRow row)
        {
            mesScreenshot model = new mesScreenshot();
            model.ID = (int)row["ID"];
            model.LineID = (int)row["LineID"];
            model.StationID = (int)row["StationID"];
            model.PartID = (int)row["PartID"];
            model.ProductionOrderID = (int)row["ProductionOrderID"];

            model.IP = row["IP"].ToString();
            model.PCName = row["PCName"].ToString();
            model.IMGURL = row["IMGURL"].ToString();
            model.MSG = row["MSG"].ToString();
            model.Feedback = row["Feedback"].ToString();
            model.IsFeedback =Convert.ToInt32(row["IsFeedback"].ToString());

            return model;
        }

        public DataTable ListAll(string S_Where)
        {            
            DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesScreenshot " + S_Where);
            return dt;
        }
    }
}

