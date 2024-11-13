using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using App.Model;using App.DBUtility;

namespace App.DBServerDAL
{
	public class   mesProductionOrderDAL
	{
		public string Insert(mesProductionOrder model)
		{
            string S_Result = "";

            try
            {
                string S_Sql = "select * from mesProductionOrder where ProductionOrderNumber='" + model.ProductionOrderNumber + "'";
                DataTable DT = SqlServerHelper.Data_Table(S_Sql);

                if (DT.Rows.Count == 0)
                {
                    object obj = SqlServerHelper.ExecuteScalar(
                        "INSERT INTO mesProductionOrder(ProductionOrderNumber,Description,PartID,OrderQuantity,EmployeeID,CreationTime,LastUpdate,StatusID," +
                        //"RequestedStartTime,ActualStartTime,RequestedFinishTime,ActualFinishTime,ShippedTime,UOM,PriorityByERP," +
                        "IsLotAuditCompleted) " +

                        "VALUES (@ProductionOrderNumber,@Description,@PartID,@OrderQuantity,@EmployeeID,@CreationTime,@LastUpdate,@StatusID," +
                        //"@RequestedStartTime,@ActualStartTime,@RequestedFinishTime,@ActualFinishTime,@ShippedTime,@UOM,@PriorityByERP," +
                        "@IsLotAuditCompleted);SELECT @@identity"

                        , new SqlParameter("@ProductionOrderNumber", model.ProductionOrderNumber)
                        , new SqlParameter("@Description", model.Description)
                        , new SqlParameter("@PartID", model.PartID)
                        , new SqlParameter("@OrderQuantity", model.OrderQuantity)
                        , new SqlParameter("@EmployeeID", model.EmployeeID)
                        , new SqlParameter("@CreationTime", model.CreationTime)
                        , new SqlParameter("@LastUpdate", model.LastUpdate)
                        , new SqlParameter("@StatusID", model.StatusID)
                        //,new SqlParameter("@RequestedStartTime", model.RequestedStartTime)
                        //,new SqlParameter("@ActualStartTime", model.ActualStartTime)
                        //,new SqlParameter("@RequestedFinishTime", model.RequestedFinishTime)
                        //,new SqlParameter("@ActualFinishTime", model.ActualFinishTime)
                        //,new SqlParameter("@ShippedTime", model.ShippedTime)
                        //,new SqlParameter("@UOM", model.UOM)
                        //,new SqlParameter("@PriorityByERP", model.PriorityByERP)
                        , new SqlParameter("@IsLotAuditCompleted", model.IsLotAuditCompleted)
                    );

                    S_Result = obj.ToString();
                    if (S_Result == "") { S_Result = "NG"; } else { S_Result = "OK"; }
                }
                else
                {
                    S_Result = "资料已存在！";
                }
            }
            catch (Exception ex)
            {
                S_Result = ex.ToString(); 
            }

            return S_Result;
        }

		public bool Delete(int id)
		{
			int rows = SqlServerHelper.ExecuteNonQuery("DELETE FROM mesProductionOrder WHERE ID = @id", new SqlParameter("@id", id));
			return rows > 0;
		}

		public string Update(mesProductionOrder model)
		{
            string S_Result = "";
            try
            {
                string S_Sql = "select * from mesProductionOrder where ProductionOrderNumber='" +
                                model.ProductionOrderNumber + "' and ID<>'"+ model.ID+ "'";
                DataTable DT = SqlServerHelper.Data_Table(S_Sql);

                if (DT.Rows.Count == 0)
                {
                    string sql = "UPDATE mesProductionOrder SET " +
                                "ProductionOrderNumber=@ProductionOrderNumber," +
                                "Description=@Description," +
                                "PartID=@PartID," +
                                "OrderQuantity=@OrderQuantity," +
                                "EmployeeID=@EmployeeID," +
                                //"CreationTime=@CreationTime," +
                                "LastUpdate=@LastUpdate," +
                                "StatusID=@StatusID," +
                                //"RequestedStartTime=@RequestedStartTime," +
                                //"ActualStartTime=@ActualStartTime," +
                                //"RequestedFinishTime=@RequestedFinishTime," +
                                //"ActualFinishTime=@ActualFinishTime," +
                                //"ShippedTime=@ShippedTime," +
                                //"UOM=@UOM," +
                                //"PriorityByERP=@PriorityByERP," +
                                "IsLotAuditCompleted=@IsLotAuditCompleted" +
                                " WHERE ID=@ID";

                    int rows = SqlServerHelper.ExecuteNonQuery(sql
                        , new SqlParameter("@ID", model.ID)
                        , new SqlParameter("@ProductionOrderNumber", model.ProductionOrderNumber)
                        , new SqlParameter("@Description", model.Description)
                        , new SqlParameter("@PartID", model.PartID)
                        , new SqlParameter("@OrderQuantity", model.OrderQuantity)
                        , new SqlParameter("@EmployeeID", model.EmployeeID)
                        //, new SqlParameter("@CreationTime", model.CreationTime)
                        , new SqlParameter("@LastUpdate", model.LastUpdate)
                        , new SqlParameter("@StatusID", model.StatusID)
                        //, new SqlParameter("@RequestedStartTime", model.RequestedStartTime)
                        //, new SqlParameter("@ActualStartTime", model.ActualStartTime)
                        //, new SqlParameter("@RequestedFinishTime", model.RequestedFinishTime)
                        //, new SqlParameter("@ActualFinishTime", model.ActualFinishTime)
                        //, new SqlParameter("@ShippedTime", model.ShippedTime)
                        //, new SqlParameter("@UOM", model.UOM)
                        //, new SqlParameter("@PriorityByERP", model.PriorityByERP)
                        , new SqlParameter("@IsLotAuditCompleted", model.IsLotAuditCompleted)
                    );

                    S_Result = "OK";
                }
                else
                {
                    S_Result = "资料已存在！";
                }
            }
            catch (Exception ex)
            {
                S_Result = ex.ToString();
            }
            return S_Result;

        }

		public mesProductionOrder Get(int id)
		{
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesProductionOrder WHERE ID=@ID", new SqlParameter("@ID", id));
			if (dt.Rows.Count > 1)
			{
				throw new Exception("more than 1 row was found");
			}
			if (dt.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dt.Rows[0];
			mesProductionOrder model = ToModel(row);
			return model;
		}

		private static mesProductionOrder ToModel(DataRow row)
		{
			mesProductionOrder model = new mesProductionOrder();
            try
            {
                model.ID = (int)row["ID"];
                model.ProductionOrderNumber = (string)row["ProductionOrderNumber"];
                model.Description = (string)row["Description"];
                model.PartID = (int)row["PartID"];
                model.OrderQuantity = (int)row["OrderQuantity"];
                model.EmployeeID = (int)row["EmployeeID"];
                model.CreationTime = (DateTime)row["CreationTime"];
                model.LastUpdate = (DateTime)row["LastUpdate"];
                model.StatusID = Convert.ToInt32(row["StatusID"].ToString()) ;

                if (row["RequestedStartTime"] != null) { model.RequestedStartTime = (DateTime)row["RequestedStartTime"]; }
                if (row["ActualStartTime"] != null) { model.ActualStartTime = (DateTime)row["ActualStartTime"]; }

                if (row["RequestedFinishTime"] != null) { model.RequestedFinishTime = (DateTime)row["RequestedFinishTime"]; }
                if (row["ActualFinishTime"] != null) { model.ActualFinishTime = (DateTime)row["ActualFinishTime"]; }
                if (row["ShippedTime"] != null) { model.ShippedTime = (DateTime)row["ShippedTime"]; }

                model.UOM = (string)row["UOM"];
                model.PriorityByERP = (int)row["PriorityByERP"];
                model.IsLotAuditCompleted = (bool)row["IsLotAuditCompleted"];
            }
            catch (Exception ex)
            {
                string ss = ex.ToString(); 
            }
			return model;
		}

		public IEnumerable<mesProductionOrder> ListAll(string S_Where)
		{
			List<mesProductionOrder> list = new List<mesProductionOrder>();
			DataTable dt = SqlServerHelper.ExecuteDataTable("SELECT * FROM mesProductionOrder " + S_Where);
			foreach (DataRow row in dt.Rows)
			{
				list.Add(ToModel(row));
			}
			return list;
		}
	}
}
