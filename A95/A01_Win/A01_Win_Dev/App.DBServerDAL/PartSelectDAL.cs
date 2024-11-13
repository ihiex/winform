using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Model;
using App.DBUtility;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace App.DBServerDAL
{
    public class PartSelectDAL
    {
        public double MyTest()
        {
            DateTime dateStartC = DateTime.Now;

            string S_Sql = "exec GetRouteCheck 21,19,11,186,102,19,'CHINA  GG2ZN05WP3MT',''";
            DataSet ds = SqlServerHelper.Data_Set(S_Sql);

            TimeSpan tsC = DateTime.Now - dateStartC;
            double millC = Math.Round(tsC.TotalMilliseconds, 0);

            return millC;
        }


        public DataSet P_DataSet(string S_Sql)
        {
            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }

        public string ExecSql(string S_Sql)
        {
            string S_Result = SqlServerHelper.ExecSql(S_Sql);
            return S_Result;
        }

        public string GetServerIP()
        {
            string[] List_Conn = SqlServerHelper.default_connection_str.Split('=');
            string[] list_IP = List_Conn[1].Split(';');

            return list_IP[0];
        }


        public DataSet GetluPartFamilyType()
        {
            string strSql = "select * from luPartFamilyType where Status=1";
            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }
        public DataSet GetluPartFamily(string PartFamilyTypeID)
        {
            string strSql = @"select A.ID, A.Name, A.Description, A.PartFamilyTypeID,C.Description PartFamilyTypeValue, 
                             A.Status,B.Description  StatusValue
                            from 
                            (select * from luPartFamily WHERE Status=1) AS A 
                                    join (select * from sysStatus ) AS B on A.Status=B.ID
                                join (select * from  luPartFamilyType) AS C on A.PartFamilyTypeID=C.ID";

            if (PartFamilyTypeID != "")
            {
                strSql = strSql + " where PartFamilyTypeID = '" + PartFamilyTypeID + "'";
            }
            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }

        public DataSet GetmesPart(string PartFamilyID)
        {
            string strSql = "SELECT * FROM mesPart where Status=1";


            if (PartFamilyID != "")
            {
                strSql = @"SELECT * FROM mesPart   
                            where PartFamilyID='" + PartFamilyID + "' and Status=1";
            }

            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }

        //public DataSet GetmesPartDetail(string PartID)
        //{
        //    string strSql = @"SELECT A.ID, A.PartID,B.PartNumber, A.PartDetailDefID,C.Description, A.Content
        //                      FROM mesPartDetail A 
        //                       join mesPart B on A.PartID=B.ID
        //                       join luPartDetailDef C on A.PartDetailDefID=C.ID
        //                      Where A.PartID="+ PartID;

        //    DataSet ds = SqlServerHelper.Data_Set(strSql);
        //    return ds;
        //}


        public DataSet GetmesPartDetail(int PartID, string PartDetailDefName)
        {
            string sql = @"select * from mesPartDetail a  WHERE  a.PartID=" + PartID +
                " and exists (select 1 from luPartDetailDef b where a.PartDetailDefID = b.ID and b.Description = '" + PartDetailDefName + "')";
            DataSet ds = SqlServerHelper.Data_Set(sql);
            return ds;

        }


        public DataSet GetluPODetailDef(int ProductionOrderID, string PODetailDef)
        {
            string strSql = @"select A.ID,
                                       A.ProductionOrderID,B.ProductionOrderNumber, 
	                                   A.ProductionOrderDetailDefID,C.Description PODetailDef,
	                                   A.Content 
                                from mesProductionOrderDetail A 
	                                join mesProductionOrder B on A.ProductionOrderID=B.ID
	                                join luProductionOrderDetailDef C on C.ID=A.ProductionOrderDetailDefID 
                              Where A.ProductionOrderID=" + ProductionOrderID +
                              " and  C.Description='" + PODetailDef + "'";

            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }


        public DataSet GetmesPartPrint()
        {
            string strSql = @"select B.ID,B.PartNumber,B.Description,C.ID SNFormatID,A.LineID,
                              C.SNFamilyID,C.Name SNFormatName from 
                                (select * from mesSNFormatMap)A join
                                (select* from mesPart where Status=1) B on A.PartID = B.ID join
                                (select * from mesSNFormat) C on A.SNFormatID = C.ID ";

            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }

        public string mesGetSNFormatIDByList(object PartID, object PartFamilyID, object LineID)
        {
            string SNFormatID = string.Empty;
            string strSql = @"SELECT top 1 B.Name FROM mesSNFormatMap A INNER JOIN mesSNFormat B
                                ON A.SNFormatID=B.ID
	                            WHERE (A.PartID=@PartID OR ISNULL(@PartID,'')='')
	                            AND (A.PartFamilyID=@PartFamilyID OR ISNULL(@PartFamilyID,'')='')
	                            AND (A.LineID=@LineID OR ISNULL(@LineID,'')='')";

            if (PartID == null) PartID = DBNull.Value;
            if (PartFamilyID == null) PartFamilyID = DBNull.Value;
            if (LineID == null) LineID = DBNull.Value;
            SqlParameter[] sqlPar = new SqlParameter[] { new SqlParameter("@PartID",PartID),
                                                         new SqlParameter("@PartFamilyID",PartFamilyID),
                                                         new SqlParameter("@LineID",LineID)};
            DataTable dataTable = SqlServerHelper.ExecuteDataTable(strSql, sqlPar);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                SNFormatID = dataTable.Rows[0]["Name"].ToString();
            }
            return SNFormatID;
        }


        public DataSet GetmesLine()
        {
            string strSql = "SELECT ID,Description  FROM mesLine ";
            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }


        public DataSet mesLineGroup(string LineType, int PartFamilyTypeID)
        {
            string strSql = @"select A.*,B.Description LineName  
                                from  mesLineGroup A join mesLine B on A.LineID=B.ID" +
                            " where LineType='" + LineType + "' and PartFamilyTypeID='" + PartFamilyTypeID + "'";
            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }


        public DataSet GetsysStatus()
        {
            string strSql = "SELECT *  FROM sysStatus ";
            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }

        public DataSet GetmesStationType()
        {
            string strSql = "SELECT *  FROM mesStationType ";
            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }

        public DataSet GetmesStation(string LineID)
        {
            string strSql = "SELECT *  FROM mesStation where  LineID='" + LineID + "' and Status=1 ";

            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }

        public DataSet GetmesStationTypeByStationID(string StationID)
        {
            string strSql = "SELECT *  FROM mesStation where  ID='" + StationID + "' and Status=1 ";

            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }


        public DataSet GetmesRoute()
        {
            string strSql = "SELECT *  FROM mesRoute ";
            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }

        public DataSet GetRouteSequence(string RouteID, string StationTypeID)
        {
            string strSql = @"SELECT A.ID, A.Name, A.[Description] AS RouteName,C.[Description] AS StationType,
                                B.[Description] AS StationTypeName,B.Sequence       
                            FROM 
                                mesRoute A 
                                join mesRouteDetail B on A.ID=B.RouteID
                                join mesStationType C on B.StationTypeID=C.ID 
                            WHERE 
                                B.RouteID=" + RouteID + @" and B.StationTypeID=" + StationTypeID;

            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }

        public DataSet GetluSerialNumberType()
        {
            string strSql = "select '0' ID ,'ALL' Description UNION SELECT *  FROM luSerialNumberType    ";
            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }


        public DataSet GetUnit(string PartID)  //暂时停用
        {
            //E.[Description] AS STATUS,	SN_TYPE.[Description] AS SNType, 
            string strSql = @"SELECT TOP 1000 A.ID,
                               SN.[Value] AS SN,                               
                               I.PartNumber,
                               F.[Description] AS Station,
                               J.ProductionOrderNumber,
                               D.[Description] AS UnitState,
                               		                   
                               G.Lastname+G.Firstname AS Employee,
			                   A.CreationTime,
                               A.LastUpdate,
                               H.[Description] AS Line, 
                               
			                   A.LooperCount
		                FROM 
			                (SELECT * FROM mesUnit) AS A 
			                JOIN (SELECT * FROM mesSerialNumber) AS B ON A.ID=B.UnitID 
			                JOIN (SELECT * FROM luSerialNumberType) AS C ON  B.SerialNumberTypeID=C.ID 
			                JOIN (SELECT * FROM mesUnitState) AS D ON A.UnitStateID=D.ID 
			                JOIN (SELECT * FROM sysStatus) AS E ON A.StatusID=E.ID 
			                JOIN (SELECT * FROM mesStation) AS F ON A.StationID=F.ID 
			                JOIN (SELECT * FROM mesEmployee ) AS G ON A.EmployeeID=G.ID  
			                JOIN (SELECT * FROM mesLine ) AS H ON A.LineID=H.ID  
			                JOIN (SELECT * FROM mesPart ) AS I ON A.PartID=I.ID 
			                LEFT JOIN (SELECT * FROM mesProductionOrder) AS J ON A.ProductionOrderID=J.ID 
			                JOIN (SELECT * FROM mesSerialNumber) AS SN ON A.ID=SN.UnitID
			                JOIN (SELECT * FROM luSerialNumberType) AS SN_TYPE ON SN.SerialNumberTypeID=SN_TYPE.ID
                       WHERE A.PartID='" + PartID + "' " +
                       " ORDER BY A.ID DESC";

            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }

        public DataSet GetUnit2(string PartID, string StationID, string POID)  //暂时停用
        {
            //E.[Description] AS STATUS,	SN_TYPE.[Description] AS SNType, 
            string strSql = @"SELECT TOP 1000 A.ID,
                               SN.[Value] AS SN,                               
                               I.PartNumber,
                               F.[Description] AS Station,
                               J.ID AS POID, 
                               J.ProductionOrderNumber,
                               D.[Description] AS UnitState,
                               		                   
                               G.Lastname+G.Firstname AS Employee,
			                   A.CreationTime,
                               A.LastUpdate,
                               H.[Description] AS Line, 
                               
			                   A.LooperCount
		                FROM 
			                (SELECT * FROM mesUnit) AS A 
			                JOIN (SELECT * FROM mesSerialNumber) AS B ON A.ID=B.UnitID 
			                JOIN (SELECT * FROM luSerialNumberType) AS C ON  B.SerialNumberTypeID=C.ID 
			                JOIN (SELECT * FROM mesUnitState) AS D ON A.UnitStateID=D.ID 
			                JOIN (SELECT * FROM sysStatus) AS E ON A.StatusID=E.ID 
			                JOIN (SELECT * FROM mesStation) AS F ON A.StationID=F.ID 
			                JOIN (SELECT * FROM mesEmployee ) AS G ON A.EmployeeID=G.ID  
			                JOIN (SELECT * FROM mesLine ) AS H ON A.LineID=H.ID  
			                JOIN (SELECT * FROM mesPart ) AS I ON A.PartID=I.ID 
			                LEFT JOIN (SELECT * FROM mesProductionOrder) AS J ON A.ProductionOrderID=J.ID 
			                JOIN (SELECT * FROM mesSerialNumber) AS SN ON A.ID=SN.UnitID
			                JOIN (SELECT * FROM luSerialNumberType) AS SN_TYPE ON SN.SerialNumberTypeID=SN_TYPE.ID
                       WHERE A.PartID='" + PartID + "'  and A.StationID='" + StationID + "' and J.ID='" + POID + "'" +
                       " ORDER BY A.ID DESC";

            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }

        public DataSet GetUnit_Search(string S_DateStart, string S_DateEnd, string S_Where)
        {
            //H.[Description] AS Line
            string S_Sql = @"SELECT  A.ID,
                               SN.[Value] AS SN,   
                               A_Detail.reserved_02 AS BatchSN,
                               C.Description AS SerialNumberType,
                               I.PartNumber,
                               F.[Description] AS Station,
                               J.ProductionOrderNumber,
                               D.[Description] AS UnitState,
                               E.[Description] AS Status,
                               		                   
                               G.Lastname+G.Firstname AS Employee,
			                   A.CreationTime,
                               A.LastUpdate                                                              			                   
		                FROM 
			                (SELECT * FROM mesUnit) AS A 
			                JOIN (SELECT * FROM mesSerialNumber) AS B ON A.ID=B.UnitID 
			                JOIN (SELECT * FROM luSerialNumberType) AS C ON  B.SerialNumberTypeID=C.ID 
			                LEFT JOIN (SELECT * FROM mesUnitState) AS D ON A.UnitStateID=D.ID 
			                LEFT JOIN (SELECT * FROM luUnitStatus) AS E ON A.StatusID=E.ID 
			                JOIN (SELECT * FROM mesStation) AS F ON A.StationID=F.ID 
			                JOIN (SELECT * FROM mesEmployee ) AS G ON A.EmployeeID=G.ID  
			                JOIN (SELECT * FROM mesLine ) AS H ON A.LineID=H.ID  
			                JOIN (SELECT * FROM mesPart ) AS I ON A.PartID=I.ID 
			                LEFT JOIN (SELECT * FROM mesProductionOrder) AS J ON A.ProductionOrderID=J.ID 
			                JOIN (SELECT * FROM mesSerialNumber) AS SN ON A.ID=SN.UnitID
			                JOIN (SELECT * FROM luSerialNumberType) AS SN_TYPE ON SN.SerialNumberTypeID=SN_TYPE.ID
                            LEFT JOIN (select UnitID,reserved_02 from mesUnitDetail ) AS A_Detail ON A_Detail.UnitID=A.ID

                       WHERE A.CreationTime>= '" + S_DateStart + "' and A.CreationTime<='" + S_DateEnd + "' " + S_Where +
                    " ORDER BY A.ID DESC";

            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }


        public DataSet GetUnitComponent(string S_UnitID)
        {
            string S_Sql = @"select  uc.ID, uc.UnitID,uc.ChildUnitID,uc.ChildSerialNumber,uc.ChildLotNumber,uc.LastUpdate  
                           from mesUnitComponent uc where uc.UnitID='" + S_UnitID + "'";

            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }

        public DataSet GetmesUnitDetail(string S_UnitID)
        {
            string S_Sql = @"select ID, UnitID,KitSerialNumber, reserved_01 SN, reserved_02 
                                BatchSN from mesUnitDetail  where UnitID='" + S_UnitID + "'";

            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }

        public DataSet GetHistory(string UnitID)
        {
            string strSql = @"SELECT TOP 1000 A.ID, 
                               A.UnitID,
                               B.[Description] AS UnitState,
                               C.Lastname+C.Firstname AS Employee,
                               D.[Description] AS Station,
                               A.EnterTime, 
                               A.ExitTime,
                               E.PartNumber,
                               F.ProductionOrderNumber,
                               A.LooperCount
                        FROM 
                            (SELECT * FROM mesHistory) A 
                            JOIN (SELECT * FROM mesUnitState) AS B ON A.UnitStateID=B.ID 
                            JOIN (SELECT * FROM mesEmployee ) AS C ON A.EmployeeID=C.ID 
                            JOIN (SELECT * FROM mesStation) AS D ON A.StationID=D.ID  
                            JOIN (SELECT * FROM mesPart ) AS E ON A.PartID=E.ID 
                            LEFT JOIN (SELECT * FROM mesProductionOrder) AS F ON A.ProductionOrderID=F.ID 
                        WHERE A.UnitID ='" + UnitID + "'";

            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }

        public DataSet GetMachineHistory(string S_UnitID)
        {
            string S_Sql = @"select A.*,B.SN from
                                (select *  from mesMachineHistory)A
	                            JOIN (select *  from mesMachine)B ON A.MachineID=B.ID
                            where A.UnitID='" + S_UnitID + "'";

            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }

        public DataSet GetmesUnitState(string S_PartID, string S_RouteSequence, string LineID, int StationTypeID)
        {
            DataTable DT_Route = GetRoute(S_PartID, S_RouteSequence, LineID, StationTypeID).Tables[0];
            string S_UnitStateID = DT_Route.Rows[0]["UnitStateID"].ToString();

            //string strSql = "SELECT *  FROM mesUnitState where ID in(2," + S_UnitStateID+") order by ID desc";
            string strSql = "SELECT *  FROM mesUnitState where ID =" + S_UnitStateID + " order by ID desc";
            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }

        public DataSet GetmesSerialNumberOfLine(string SNCategory, string PrintCount)
        {
            string strSql = "select * from mesSerialNumberOfLine where SNCategory='" + SNCategory + "'  and PrintCount=0 Order By ID";
            if (PrintCount != "0")
            {
                strSql = "select * from mesSerialNumberOfLine where SNCategory='" + SNCategory + "'  and PrintCount>0  Order By ID";
            }

            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }

        public DataSet GetPO(string S_PartID, string S_StatusID)
        {
            string strSql = @"SELECT A.ID, A.ProductionOrderNumber,C.PartNumber, A.Description, A.OrderQuantity,
		                        B.Lastname+B.Firstname AS Employee, A.CreationTime, A.LastUpdate,D.Description AS Status 
		                        FROM 
		                        (SELECT  * FROM mesProductionOrder where StatusID=1)AS A  
		                        JOIN (SELECT * from mesEmployee ) B ON A.EmployeeID=B.ID
		                        JOIN (SELECT * from mesPart)AS C ON A.PartID=C.ID 
		                        JOIN (select * from sysStatus ) AS D ON A.StatusID=D.ID
                            WHERE A.PartID ='" + S_PartID + "'";

            if (S_StatusID != "")
            {
                strSql += " AND A.StatusID='" + S_StatusID + "'";
            }

            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }


        public DataSet GetPOAll(string S_PartID, string S_LineID)
        {
            string strSql = @"SELECT A.ID, A.ProductionOrderNumber,C.PartNumber, A.Description, A.OrderQuantity,
		                        B.Lastname+B.Firstname AS Employee, A.CreationTime, A.LastUpdate,D.Description AS Status 
		                        FROM 
		                        (SELECT  * FROM mesProductionOrder where StatusID=1)AS A  
		                        JOIN (SELECT * from mesEmployee ) B ON A.EmployeeID=B.ID
		                        JOIN (SELECT * from mesPart)AS C ON A.PartID=C.ID 
		                        JOIN (select * from sysStatus ) AS D ON A.StatusID=D.ID";
            //--不需要这句 JOIN (select *  from  mesRouteMap) AS E ON A.PartID=E.PartID 
            if (S_LineID != "")
            {
                strSql = strSql + " JOIN(select * from mesLineOrder ) AS F ON A.ID = F.ProductionOrderID";
            }
            strSql = strSql + " Where 1=1";
            if (S_PartID != "" && S_LineID == "")
            {
                strSql += "AND C.ID=" + S_PartID;
            }
            if (S_LineID != "" && S_PartID == "")
            {
                strSql += "AND F.LineID=" + S_LineID;
            }

            if (S_PartID != "" && S_LineID != "")
            {
                strSql += "AND C.ID=" + S_PartID + "  and F.LineID=" + S_LineID;
            }

            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }

        public DataSet GetApplicationType(string StationTypeID)
        {
            string S_Sql = @"select A.*,B.Description ApplicationType  from  mesStationType A 
                                    Join luApplicationType B ON A.ApplicationTypeID=B.ID 
                             where A.ID='" + StationTypeID + "'";

            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }


        public DataSet GetluDefect(int DefectTypeID)
        {
            // A.DefectCode 不良代码,
            string S_Sql = @"select                                
                                    A.ID,  
	                                A.DefectCode 不良代码,                     
	                                A.Description 不良名称,
	                                C.Description 位置	                             
                            from 
	                            (select *  from  luDefect)A 
	                            left join luDefectType B on A.DefectTypeID=B.ID
	                            left join luLocaltion C on A.LocaltionID=C.ID
	                            left join sysStatus D on A.Status=D.ID                            
                            where A.DefectTypeID=" + DefectTypeID + @"
                            order by ID";

            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }

        public DataSet GetluUnitStatus()
        {
            string S_Sql = @"select * from luUnitStatus ";

            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }
        public DataSet GetmesUnitDefect(string S_UnitID)
        {
            string S_Sql = @"
                            select A.ID,A.UnitID,B.DefectCode,B.Description,B.Localtion,
	                               C.Description Station, D.Lastname + D.Firstname Employee,
	                               A.CreationTime
                            from
                            (
                                select * from mesUnitDefect  where UnitID = " + S_UnitID + @"
                            )A
                            join
                            (
                                select
                                        A.ID,
                                        B.Description DefectType,
                                        A.DefectCode,
                                        A.Description,
                                        C.Description Localtion
                                from
                                    (select * from luDefect)A
                                    left join luDefectType B on A.DefectTypeID = B.ID
                                    left join luLocaltion C on A.LocaltionID = C.ID
                                    left join sysStatus D on A.Status = D.ID
                            )B on A.DefectID = B.ID
                            join mesStation C on A.StationID = C.ID
                            join mesEmployee D on A.EmployeeID = D.ID
                            order by ID";

            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }

        public string Get_MachineRouteMap(string S_ToolSN, string S_ProductPartID, string S_RouteID, string S_StationTypeID)
        {
            string S_Result = "N";
            try
            {
                string S_Sql = "select *  from mesMachine where SN='" + S_ToolSN + "'";
                DataTable DT_Machine = SqlServerHelper.Data_Set(S_Sql).Tables[0];

                string S_MachineID = DT_Machine.Rows[0]["ID"].ToString();
                string S_MachineFamilyID = DT_Machine.Rows[0]["MachineFamilyID"].ToString();
                string S_MachinePartID = DT_Machine.Rows[0]["PartID"].ToString(); ;

                //检查设备 关联是否是有SN
                S_Sql = "select *  from mesRouteMachineMap WHERE RouteID='" + S_RouteID
                                                          + "' and StationTypeID='" + S_StationTypeID
                                                          + "' and PartID='" + S_ProductPartID
                                                          + "' and MachineID='" + S_MachineID + "'";
                DataTable DT_MachineSN = SqlServerHelper.Data_Set(S_Sql).Tables[0];
                if (DT_MachineSN.Rows.Count == 0)
                {
                    //检查设备 关联是否有设备料号（当设备SN为空时）
                    S_Sql = "select *  from mesRouteMachineMap WHERE RouteID='" + S_RouteID
                                                              + "' and StationTypeID='" + S_StationTypeID
                                                              + "' and PartID='" + S_ProductPartID
                                                              + "' and MachineID is null"
                                                              + " and MachinePartID='" + S_MachinePartID + "'";
                    DataTable DT_MachinePart = SqlServerHelper.Data_Set(S_Sql).Tables[0];
                    if (DT_MachinePart.Rows.Count == 0)
                    {
                        //检查设备 关联是否有设备群（当设备SN，设备料号 为空时）
                        S_Sql = "select *  from mesRouteMachineMap WHERE RouteID='" + S_RouteID
                                                                  + "' and StationTypeID='" + S_StationTypeID
                                                                  + "' and PartID='" + S_ProductPartID
                                                                  + "' and MachineID is null and MachinePartID is null"
                                                                  + " and MachineFamilyID='" + S_MachineFamilyID + "'";
                        DataTable DT_MachineFamily = SqlServerHelper.Data_Set(S_Sql).Tables[0];
                        if (DT_MachineFamily.Rows.Count > 0)
                        {
                            S_Result = "Y";
                        }
                    }
                    else
                    {
                        S_Result = "Y";
                    }
                }
                else
                {
                    S_Result = "Y";
                }
            }
            catch (Exception ex)
            {
                S_Result = "Error:" + ex.ToString();
            }
            return S_Result;
        }


        public DataSet uspSNRGetNext(string strSNFormat, int ReuseSNByStation, string xmlProdOrder,
                                      string xmlPart, string xmlStation, string xmlExtraData, string strNextSN)
        {
            xmlProdOrder = string.IsNullOrEmpty(xmlProdOrder) ? "null" : xmlProdOrder;
            xmlPart = string.IsNullOrEmpty(xmlPart) ? "null" : xmlPart;
            xmlStation = string.IsNullOrEmpty(xmlStation) ? "null" : xmlStation;
            xmlExtraData = string.IsNullOrEmpty(xmlExtraData) ? "null" : xmlExtraData;
            strNextSN = string.IsNullOrEmpty(strNextSN) ? "null" : strNextSN;

            string S_Sql = "exec [dbo].[uspSNRGetNext] '" + strSNFormat + "'," + ReuseSNByStation +
                "," + xmlProdOrder + "," + xmlPart + "," + xmlStation + "," + xmlExtraData + "," + strNextSN;
            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }


        public DataSet GetPartDetailDef(string SN)
        {
            string S_Sql = @"select B.Value as CTSN,C.PartDetailDefID, C.Content  from mesUnit A 
                                join mesSerialNumber B on A.ID = B.UnitID
                                join mesPartDetail C on A.PartID = C.PartID
                            where B.Value = '" + SN + @"'";
            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }


        public DataSet GetRoute(string PartID, string S_RouteSequence, string LineID, int StationTypeID)
        {
            string S_Sql_Line = @"select B.ID,B.Name,B.Description RouteName,
                                A.StationTypeID,A.Sequence,A.Description RouteDetailName,
                                A.UnitStateID,
	                            C.PartFamilyID,C.PartID,C.LineID,
	                            E.Description ApplicationType
                            from 
                                mesRouteDetail A
	                            join mesRoute B on A.RouteID=B.ID  		  	
	                            join mesRouteMap C on B.ID=C.RouteID
	                            join mesStationType D on D.ID=A.StationTypeID
	                            join luApplicationType E on E.ID=D.ApplicationTypeID
                            where C.PartID=" + PartID + " and A.StationTypeID=" + StationTypeID;
            DataTable DT_Line = SqlServerHelper.Data_Table(S_Sql_Line);
            string S_LineID = "0";
            if (DT_Line.Rows.Count > 0)
            {
                S_LineID = DT_Line.Rows[0]["LineID"].ToString();
            }

            string S_Sql = @"select B.ID,B.Name,B.Description RouteName,
                                A.StationTypeID,A.Sequence,A.Description RouteDetailName,
                                A.UnitStateID,
	                            C.PartFamilyID,C.PartID,C.LineID,
	                            E.Description ApplicationType
                            from 
                                mesRouteDetail A
	                            join mesRoute B on A.RouteID=B.ID  		  	
	                            join mesRouteMap C on B.ID=C.RouteID
	                            join mesStationType D on D.ID=A.StationTypeID
	                            join luApplicationType E on E.ID=D.ApplicationTypeID
                            where C.PartID=" + PartID;
            if (S_RouteSequence != "")
            {
                S_Sql += " and A.Sequence=" + S_RouteSequence;
            }

            if (S_LineID != "0")
            {
                S_Sql += " and C.LineID = " + LineID;
            }
            else
            {
                S_Sql += " and C.LineID = 0";
            }

            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }


        public string GetRouteCheck(int Scan_StationTypeID, int Scan_StationID, string LineID, DataTable DT_Unit, string S_Str)
        {
            string S_Result = "1";
            string OpenLogFile = System.Configuration.ConfigurationManager.AppSettings["OpenLogFile"];
            DateTime dateStart = DateTime.Now;

            try
            {
                string S_PartID = DT_Unit.Rows[0]["PartID"].ToString();
                //最后扫描工序 UnitStateID
                string S_UnitStateID = DT_Unit.Rows[0]["UnitStateID"].ToString();
                string S_UnitStationID = DT_Unit.Rows[0]["StationID"].ToString();

                //获取此料工序路径
                DataTable DT_Route = GetRoute(S_PartID, "", LineID, Scan_StationTypeID).Tables[0];
                //当前扫描信息
                string S_Sql = "select *  from mesStation where ID='" + Scan_StationID + "'";
                DataTable DT_Now_Scan_Station = P_DataSet(S_Sql).Tables[0];

                //改为按  StationTypeID  来识别
                int I_StationTypeID_Scan = Convert.ToInt32(DT_Now_Scan_Station.Rows[0]["StationTypeID"].ToString());
                //当前工站类别  工序配置信息
                var v_Route_Sacn = from c in DT_Route.AsEnumerable()
                                   where c.Field<int>("StationTypeID") == Scan_StationTypeID
                                   select c;

                if (v_Route_Sacn.ToList().Count() > 0)
                {
                    int I_Sequence_Scan = v_Route_Sacn.ToList()[0].Field<int>("Sequence");
                    if (I_Sequence_Scan > 1)
                    {
                        //最后扫描信息 (上一站扫描)
                        S_Sql = "select *  from mesStation where ID='" + S_UnitStationID + "'";
                        DataTable DT_Station = P_DataSet(S_Sql).Tables[0];
                        int I_StationTypeID = Convert.ToInt32(DT_Station.Rows[0]["StationTypeID"].ToString());

                        try
                        {
                            //最后 扫描路径信息
                            var v_Route = from c in DT_Route.AsEnumerable()
                                          where c.Field<int>("StationTypeID") == I_StationTypeID
                                          select c;
                            //最后 扫描顺序
                            int I_Sequence = v_Route.ToList()[0].Field<int>("Sequence");
                            //最后扫描顺序  比 当前扫描顺序
                            if (I_Sequence >= I_Sequence_Scan)
                            {
                                S_Result = S_Str + " 此条码已过站！";
                            }
                            else
                            {
                                //判断上一站是否扫描
                                if (I_Sequence_Scan - 1 != I_Sequence)
                                {
                                    S_Result = S_Str + " 上一站未扫描！";
                                }
                            }
                        }
                        catch
                        {
                            S_Result = S_Str + " 上一站未扫描！";
                        }
                    }
                }
                else
                {
                    //没有配置 此工位工序
                    S_Result = S_Str + " 无此工序！(工站类别ID：" + Scan_StationTypeID + ")";
                }
            }
            catch (Exception ex)
            {
                S_Result = ex.ToString();
            }
            finally
            {
                try
                {
                    if (OpenLogFile == "Y")
                    {
                        TimeSpan ts = DateTime.Now - dateStart;
                        double mill = ts.TotalMilliseconds;
                        string logPath = CreateServerDIR();
                        string msg = "过站校验(GetRouteCheck) 工位ID:" + Scan_StationTypeID + ",SN:"
                                        + S_Str + ",用时：" + mill.ToString() + "毫秒";
                        File.AppendAllText(logPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".log",
                         DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + msg + "\r\n");
                    }
                }
                catch (Exception ex)
                { }
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

        public DataSet Get_UnitID(string S_SN)
        {
            string S_Sql = @"select A.*,B.PartID,B.StationID,B.UnitStateID,B.StatusID  from 
	                            (select * from mesSerialNumber) A 
	                            JOIN (select u.ID,u.PartID,u.StationID,u.UnitStateID,u.StatusID  from mesUnit u)B on A.UnitID=B.ID
                            where Value = '" + S_SN + "'";
            DataSet DS = SqlServerHelper.Data_Set(S_Sql);
            return DS;
        }

        public DataSet GetMesUnitStateID(int? partId, int stationId)
        {
            string strSql = @"SELECT B.UnitStateID FROM mesRouteMap a,mesRouteDetail
                                   b,mesStationType c,mesStation D WHERE a.RouteID = b.RouteID and
                                   b.StationTypeID = c.ID AND C.ID = D.StationTypeID and a.PartID = '"
                            + partId + "'and D.ID = '" + stationId + "'";
            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }

        public string Get_CreateMesSN(string strSNFormat, string xmlProdOrder, string xmlPart, string xmlStation,
                                      string xmlExtraData, mesUnit v_mesUnit, int number, ref DataSet dsSN)
        {
            string result = "1";

            if (dsSN == null)
            {
                dsSN = new DataSet();
            }

            mesUnitDAL mesUnitDll = new mesUnitDAL();
            mesSerialNumberDAL mesSerialNumberDll = new mesSerialNumberDAL();
            mesHistoryDAL mesHistoryDll = new mesHistoryDAL();

            string S_FormatName = string.Empty;
            S_FormatName = mesGetSNFormatIDByList(v_mesUnit.PartID, v_mesUnit.PartFamilyID, v_mesUnit.LineID);
            if (string.IsNullOrEmpty(S_FormatName))
            {
                return result = "该物料编号未配置条码打印格式!";
            }


            DataTable dtState = GetMesUnitStateID(v_mesUnit.PartID, v_mesUnit.StationID).Tables[0];
            if (dtState == null || dtState.Rows.Count == 0)
            {
                return result = "该物料编号未配置工艺流程的单元状态!";
            }
            v_mesUnit.UnitStateID = Convert.ToInt32(dtState.Rows[0][0].ToString());

            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("SN", typeof(string));
                for (; number > 0; number--)
                {
                    //写入UNIT表
                    string unitId = string.Empty;
                    v_mesUnit.CreationTime = DateTime.Now;
                    v_mesUnit.LastUpdate = DateTime.Now;
                    unitId = mesUnitDll.Insert(v_mesUnit);
                    if (string.IsNullOrEmpty(unitId) || unitId.Substring(0, 1) == "E")
                    {
                        return result = "写入mesunit数据失败!";
                    }

                    //生成条码
                    if (strSNFormat == "" || strSNFormat == null)
                    {
                        strSNFormat = S_FormatName;
                    }

                    DataTable DT = uspSNRGetNext(strSNFormat, 0, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, null).Tables[1];
                    string S_SN = DT.Rows[0][0].ToString();
                    if (string.IsNullOrEmpty(S_SN))
                    {
                        return result = "条码生成失败!";
                    }

                    //写入mesSerialNumber
                    mesSerialNumber v_mesSerialNumber = new mesSerialNumber();
                    v_mesSerialNumber.UnitID = Convert.ToInt32(unitId);
                    v_mesSerialNumber.SerialNumberTypeID = v_mesUnit.SerialNumberType;
                    v_mesSerialNumber.Value = S_SN;
                    mesSerialNumberDll.Insert(v_mesSerialNumber);

                    //写过站履历
                    mesHistory v_mesHistory = new mesHistory();
                    v_mesHistory.UnitID = Convert.ToInt32(unitId);
                    v_mesHistory.UnitStateID = v_mesUnit.UnitStateID;
                    v_mesHistory.EmployeeID = v_mesUnit.EmployeeID;
                    v_mesHistory.StationID = v_mesUnit.StationID;
                    v_mesHistory.EnterTime = DateTime.Now;
                    v_mesHistory.ExitTime = DateTime.Now;
                    v_mesHistory.ProductionOrderID = v_mesUnit.ProductionOrderID;
                    v_mesHistory.PartID = Convert.ToInt32(v_mesUnit.PartID);
                    v_mesHistory.LooperCount = 1;
                    mesHistoryDll.Insert(v_mesHistory);
                    DataRow dr = dt.NewRow();
                    dr["SN"] = S_SN;
                    dt.Rows.Add(dr);
                }
                dsSN.Tables.Add(dt);
            }
            catch (Exception ex)
            {
                return result = "程序异常" + ex.Message.ToString();
            }
            return result;
        }

        public string GetluDefectType(string Description)
        {
            string S_Result = "";
            string S_Sql = "select * from luDefectType where Description='" + Description + "'";
            DataSet ds = SqlServerHelper.Data_Set(S_Sql);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                S_Result = ds.Tables[0].Rows[0]["ID"].ToString();
            }

            return S_Result;
        }

        public DataSet BoxSnToBatch(string S_BoxSN, out string S_Result)
        {
            S_Result = "";
            DataSet DS_Result = new DataSet();
            string S_Sql = "select * from mesMachine WHERE  SN='" + S_BoxSN + "'";
            DataTable DT1 = SqlServerHelper.Data_Table(S_Sql);

            if (DT1.Rows.Count == 0)
            {
                S_Result = "ERROR:" + S_BoxSN + " 盒子条码不存在！";
            }
            else
            {
                string S_MachineStatusID = DT1.Rows[0]["StatusID"].ToString();
                if (S_MachineStatusID == "2")
                {
                    S_Sql = @"select UnitID,reserved_02 HB_Batch from mesUnitDetail 
	                            where reserved_01='" + S_BoxSN + @"' and reserved_03=1 and 
	                            UnitID=(select max(unitid)  from mesUnitDetail where reserved_01='" + S_BoxSN + @"' and reserved_03=1)";
                    DataTable DT2 = SqlServerHelper.Data_Table(S_Sql);

                    if (DT2.Rows.Count > 0)
                    {
                        S_Result = DT2.Rows[0]["HB_Batch"].ToString();
                        DS_Result.Tables.Add(DT2);
                    }
                    else
                    {
                        S_Result = "ERROR:" + S_BoxSN + "盒子条码不存在或没有配置！";
                    }
                }
                else
                {
                    S_Result = "ERROR:" + S_BoxSN + "盒子条码需要前段工序扫描以后方可再次扫描！";
                }
            }
            return DS_Result;
        }

        //子料数量
        public int GetComponentCount(string ParentPartID, string StationTypeID)
        {
            string S_Sql = "select * from mesProductStructure where ParentPartID='"
                            + ParentPartID + "'" + " and StationTypeID='" + StationTypeID + "'";
            DataTable DT_ComponentCount = SqlServerHelper.Data_Table(S_Sql);
            return DT_ComponentCount.Rows.Count;
        }

        public DataSet GetmesUnitComponent(string UnitID, string ChildUnitID)
        {
            string S_Sql = "select * from mesUnitComponent where UnitID='" + UnitID + "' and ChildUnitID='" + ChildUnitID + "'";
            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }

        public DataSet GetmesProductStructure(string ParentPartID, string PartID, string StationTypeID)
        {
            string S_Sql = "select * from mesProductStructure where ParentPartID='" + ParentPartID + "' and PartID='" + PartID + "'" +
             " and StationTypeID = '" + StationTypeID + "'";
            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }

        public DataSet GetmesProductStructure1(string ParentPartID)
        {
            string S_Sql = "select * from mesProductStructure where ParentPartID='" + ParentPartID + "'";
            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }


        public DataSet GetmesProductStructure2(string ParentPartID, string StationTypeID)
        {
            string S_Sql = "select * from mesProductStructure where ParentPartID='" + ParentPartID +
                "' and StationTypeID='" + StationTypeID + "'";
            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }

        public DataSet GetChildScanLast(string S_SN)
        {
            //判断子料是否有流程扫描过， 如果是判断资料是否扫描完毕                    
            string S_Sql = @"select top 1 * from 
                    (
	                    select A.*,B.StationID,
			                    B.PartID,B.ProductionOrderID,
			                    C.Description Station,C.StationTypeID,
			                    E.StationTypeID StationTypeID_RouteDetail,
			                    E.Sequence           
	                    from 
		                    (select *  from mesSerialNumber where Value  ='" + S_SN + @"')A   
		                    join (select ID,StationID,PartID,ProductionOrderID from  mesUnit) B on A.UnitID=B.ID
		                    join (select ID,Description,StationTypeID from mesStation) C on B.StationID=C.ID
		                    join mesRouteMap D on D.PartID=B.PartID 
		                    join mesRouteDetail E on E.RouteID=D.RouteID
	    
                    )AA order by AA.Sequence desc";
            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }

        //不用修改 目前StatusID为2是不可用工单
        public void ModPO(string S_POID)
        {
            //string S_Sql = "Update mesProductionOrder set StatusID=2,LastUpdate=GEDATE() where ID='" + S_POID + "'";
            //SqlServerHelper.ExecSql(S_Sql);
        }

        public void ModMachine(string S_SN, string StatusID, Boolean IsUpUnitDetail)
        {
            string S_Sql = string.Empty;
            if (StatusID == "1")
            {
                S_Sql = "Update mesMachine set RuningQuantity=0 ,StatusID=" + StatusID + " where SN='" + S_SN + "'";
            }
            else
            {
                S_Sql = "Update mesMachine set StatusID=" + StatusID + " where SN='" + S_SN + "'";
            }

            SqlServerHelper.ExecSql(S_Sql);

            if (IsUpUnitDetail == true)
            {
                S_Sql = @"Update  mesUnitDetail set reserved_03=2
	                    where reserved_01='" + S_SN + @"' and reserved_03=1 and 
	                    UnitID=(select max(unitid)  from mesUnitDetail where reserved_01='" + S_SN + @"' and reserved_03=1)";
                SqlServerHelper.ExecSql(S_Sql);
            }
        }

        public void ModMachine2(string ID, string StatusID)
        {
            string S_Sql = "Update mesMachine set StatusID=" + StatusID + " where ID='" + ID + "'";
            SqlServerHelper.ExecSql(S_Sql);
        }
        public DataSet GetmesSerialNumber(string S_SN)
        {
            string S_Sql = "select * from mesSerialNumber where Value =N'" + S_SN + "'";
            return SqlServerHelper.Data_Set(S_Sql);
        }

        public DataSet GetmesSerialNumberByUnitID(string UnitID)
        {
            string S_Sql = "select * from mesSerialNumber where UnitID =" + UnitID;
            return SqlServerHelper.Data_Set(S_Sql);
        }

        public DataSet GetmesUnit(string UnitID)
        {
            string S_Sql = "select * from mesUnit where ID='" + UnitID + "'";
            return SqlServerHelper.Data_Set(S_Sql);
        }

        public DataSet GetComponent(int I_ChildUnitID)
        {
            string S_Sql = @"select A.*,B.PartFamilyID,C.Value  from 
                                    mesUnit A join mesPart B on A.PartID=B.ID
                                    join mesSerialNumber C on A.ID=C.UnitID
                            where A.ID= " + I_ChildUnitID;
            return SqlServerHelper.Data_Set(S_Sql);
        }

        public DataSet GetmesMachine(string S_SN)
        {
            string S_Sql = "select * from mesMachine where SN='" + S_SN + "' ";
            return SqlServerHelper.Data_Set(S_Sql);
        }

        public DataSet GetmesRouteMachineMap(string MachineID, string MachineFamilyID)
        {
            string S_Sql = "";
            if (MachineID != "")
            {
                S_Sql = "select *  from mesRouteMachineMap where MachineID=" + MachineID;
            }
            else if (MachineFamilyID != "")
            {
                S_Sql = "select *  from mesRouteMachineMap where MachineFamilyID=" + MachineFamilyID;
            }
            return SqlServerHelper.Data_Set(S_Sql);
        }

        public DataSet GetProductionOrder(string ID)
        {
            string S_Sql = "select *  from mesProductionOrder where ID='" + ID + "'";
            return SqlServerHelper.Data_Set(S_Sql);
        }

        public DataSet GetMesPackageStatusID(string PalletSN)
        {
            string S_Sql = @"SELECT StatusID FROM mesPackage WHERE SerialNumber='" + PalletSN + "'";
            return SqlServerHelper.Data_Set(S_Sql);
        }


        public DataSet GetSNParameter(int PartID, int TemplateType)
        {
            DataSet DS = new DataSet();
            string S_Sql = @"select A.*,B.PartID FROM mesSNParameter A 
                                 join mesSNFormatMap B on A.SNFormatID=B.SNFormatID
                            where B.PartID= " + PartID + " and A.StatusID=1 and A.TemplateType=" + TemplateType;
            DataTable DT_SNParameter = SqlServerHelper.Data_Table(S_Sql);

            DS.Tables.Add(DT_SNParameter);
            return DS;
        }

        public DataSet GetLBLGenLabel(string S_SN, string S_LabelName)
        {
            DataSet DS = new DataSet();
            string S_Sql = "exec uspLBLGenLabel '" + S_SN + "','" + S_LabelName + "'";
            return SqlServerHelper.Data_Set(S_Sql);
        }

        public DataSet GetLabelName(string StationTypeID,string PartFamilyID,string PartID,string ProductionOrderID,string LineID)
        {
            DataSet DS = new DataSet();
            string S_Sql = @"select A.ID, A.StationTypeID,B.Description StationType,
		                            A.LabelID,C.Name  LabelName,C.SourcePath LabelPath,
		                            A.PartFamilyID,E.Name PartFamily,
		                            A.PartID,D.PartNumber,		
		                            A.ProductionOrderID,F.ProductionOrderNumber,
		                            A.LineID,G.Description Line 
                            from mesStationTypeLabelMap A
		                                left join mesStationType B on A.StationTypeID=B.ID
		                                left join mesLabel C on A.LabelID=C.ID
		                                left join luPartFamily E on A.PartFamilyID=E.ID
		                                left join mesPart D on A.PartID=D.ID           
                                        left join mesProductionOrder F on A.ProductionOrderID=F.ID
                                        left join mesLine G on A.LineID=G.ID  
                            where A.StationTypeID = " + StationTypeID;

            DataTable DT = SqlServerHelper.Data_Table(S_Sql);
            Boolean B_IsParameter = false; 
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                string S_LineID = DT.Rows[i]["LineID"].ToString();
                string S_ProductionOrderID = DT.Rows[i]["ProductionOrderID"].ToString();
                string S_PartID = DT.Rows[i]["PartID"].ToString();
                string S_PartFamilyID = DT.Rows[i]["PartFamilyID"].ToString();

                if (S_LineID != "")
                {
                    if (S_LineID == LineID)
                    {
                        S_Sql += " and A.LineID=" + LineID;
                        B_IsParameter = true;
                    }
                }

                if (S_ProductionOrderID != "")
                {
                    if (S_ProductionOrderID == ProductionOrderID)
                    {
                        S_Sql += " and A.ProductionOrderID=" + S_ProductionOrderID;
                        B_IsParameter = true;
                    }
                }

                if (S_PartID != "")
                {
                    if (S_PartID == PartID)
                    {
                        S_Sql += " and A.PartID=" + S_PartID;
                        B_IsParameter = true;
                    }
                }

                if (S_PartFamilyID != "")
                {
                    if (S_PartFamilyID == PartFamilyID)
                    {
                        S_Sql += " and A.PartFamilyID=" + PartFamilyID;
                        B_IsParameter = true;
                    }
                }
            }

            if (B_IsParameter == false)
            {
                S_Sql = "select '' LabelName,'' LabelPath";
            }
            return SqlServerHelper.Data_Set(S_Sql);
        }

        public string BuckToFGSN(string S_BuckSN)
        {
            string S_Sql =
                @"select reserved_04 FG_SN  from  mesUnitDetail 
                    WHERE reserved_01='" + S_BuckSN + @"'
                    and ID=(select isnull(max(ID),0) ID from  mesUnitDetail where reserved_01='" + S_BuckSN + "')";
            DataTable DT = SqlServerHelper.Data_Table(S_Sql);

            return DT.Rows[0]["FG_SN"].ToString();
        }


        public DataSet GetPLCSeting(string S_SetName, string S_StationID)
        {
            string S_Sql = "select Name,Value from mesStationConfigSetting where StationID='" + S_StationID + "'";

            if (S_SetName != "")
            {
                S_Sql = "select Value from mesStationConfigSetting where Name='" +
                   S_SetName + "' and StationID='" + S_StationID + "'";
            }

            DataSet DS = SqlServerHelper.Data_Set(S_Sql);

            return DS;
        }

        public string TimeCheck(string StationTypeID, string S_SN)
        {
            string S_Result = "1";

            try
            {
                string S_Sql = "select * from mesStationTypeDetail where StationTypeID=" + StationTypeID;
                DataTable DT = SqlServerHelper.Data_Table(S_Sql);

                if (DT.Rows.Count >= 3)
                {
                    string S_IsTimeCheck = (from c in DT.AsEnumerable()
                                            where c.Field<int>("StationTypeDetailDefID") == 6
                                            select c.Field<String>("Content")
                                            ).FirstOrDefault().ToString();

                    string S_TimeCheckStartStationType = (from c in DT.AsEnumerable()
                                                          where c.Field<int>("StationTypeDetailDefID") == 7
                                                          select c.Field<String>("Content")
                                            ).FirstOrDefault().ToString();

                    string S_TimeCheckDuration = (from c in DT.AsEnumerable()
                                                  where c.Field<int>("StationTypeDetailDefID") == 8
                                                  select c.Field<String>("Content")
                                            ).FirstOrDefault().ToString();

                    if (S_IsTimeCheck == "1")
                    {
                        S_Sql = @"select A.*,C.EnterTime,getdate() NowTime,DATEDIFF(minute,C.EnterTime,getdate()) TimeDuration,    
                                C.StationID,D.Description Station, D.StationTypeID, E.Description StationType
                            from mesSerialNumber A
                                join mesUnit B on A.UnitID = B.ID
                                join mesHistory C on C.UnitID = A.UnitID
                                join mesStation D on D.ID = C.StationID
                                join mesStationType E on E.ID = D.StationTypeID
                            where A.Value = '" + S_SN + "' and D.StationTypeID=" + S_TimeCheckStartStationType;

                        DT = SqlServerHelper.Data_Table(S_Sql);
                        int I_TimeDuration = Convert.ToInt32(DT.Rows[0]["TimeDuration"].ToString());

                        if (I_TimeDuration <= Convert.ToInt32(S_TimeCheckDuration))
                        {
                            S_Result = "1";
                        }
                        else
                        {
                            S_Result = S_SN + " 已超时，时间检查失败！";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                S_Result = ex.ToString();
            }

            return S_Result;
        }

        //**********************************************************************************************************************// 
        //**********************************************************************************************************************// 
        //**********************************************************************************************************************// 

        public DataSet GetPartParameter(string PartID)
        {
            string strSql = @"SELECT B.Content,C.Description FROM mesPart A 
                                INNER JOIN mesPartDetail B ON A.ID = B.PartID
                                LEFT JOIN luPartDetailDef C ON B.PartDetailDefID = C.ID WHERE A.ID =" + PartID;

            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }

        public string uspKitBoxCheck(string S_FormatName, string xmlProdOrder, string xmlPart, string xmlStation, string xmlExtraData, string strSNbuf)
        {
            string result;
            try
            {
                if (string.IsNullOrEmpty(S_FormatName))
                {
                    result = "参数S_FormatName不能为空。";
                }
                else
                {
                    xmlProdOrder = string.IsNullOrEmpty(xmlProdOrder) ? "null" : xmlProdOrder;
                    xmlPart = string.IsNullOrEmpty(xmlPart) ? "null" : xmlPart;
                    xmlStation = string.IsNullOrEmpty(xmlStation) ? "null" : xmlStation;
                    xmlExtraData = string.IsNullOrEmpty(xmlExtraData) ? "null" : xmlExtraData;

                    SqlParameter[] commandParameters = new SqlParameter[]
                    {
                        new SqlParameter("@strSNFormat", S_FormatName),
                        new SqlParameter("@xmlProdOrder", xmlProdOrder),
                        new SqlParameter("@xmlPart", xmlPart),
                        new SqlParameter("@xmlStation", xmlStation),
                        new SqlParameter("@xmlExtraData", xmlExtraData),
                        new SqlParameter("@strSNbuf", strSNbuf),
                        new SqlParameter("@strOutput", "")
                    };

                    result = SqlServerHelper.ExecuteScalar(CommandType.StoredProcedure, "uspKitBoxCheck", commandParameters).ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                return "程序异常:" + ex.Message.ToString();
            }
        }

        /// <summary>
        /// 中箱包装
        /// </summary>
        /// <param name="PartID"></param>
        /// <param name="S_UPCSN"></param>
        /// <param name="S_CartonSN"> 箱号SN</param>
        /// <param name="LoginList"></param>
        /// <param name="BoxQty">整箱数量</param>
        /// <returns></returns>
        public string uspKitBoxPackaging(string PartID, string ProductionOrderID, string S_UPCSN, string S_CartonSN, LoginList loginList, int BoxQty = 0)
        {
            try
            {
                int execNumber = 0;
                string strSql = @"SELECT ID FROM mesPackage WHERE SerialNumber=@S_CartonSN AND StatusID=0 AND Stage=1";

                DataTable dtMp = SqlServerHelper.ExecuteDataTable(strSql, new SqlParameter("@S_CartonSN", S_CartonSN));
                if (dtMp == null || dtMp.Rows.Count == 0)
                {
                    return "箱号:" + S_CartonSN + ",不存在或者状态不符";
                }
                string boxID = dtMp.Rows[0]["ID"].ToString();

                if (BoxQty > 0)
                {
                    strSql = @"INSERT INTO mesPackageHistory(PackageID, PackageStatusID, StationID, EmployeeID, Time)
                                VALUES(@PackageId, 1, @stationId, @employeeId, GETDATE())";
                    execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@PackageId", boxID),
                                                                         new SqlParameter("@stationId", loginList.StationID),
                                                                         new SqlParameter("@employeeId", loginList.EmployeeID));
                    if (execNumber < 0)
                    {
                        return "箱号过站履历记录失败!";
                    }

                    strSql = @"UPDATE mesPackage SET CurrentCount=@packCount,StationID=@stationId,EmployeeID=@employeeId,
				                StatusID=1,LastUpdate=GETDATE() where ID=@PackageId";
                    execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@PackageId", boxID),
                                                                         new SqlParameter("@stationId", loginList.StationID),
                                                                         new SqlParameter("@employeeId", loginList.EmployeeID),
                                                                         new SqlParameter("@packCount", BoxQty));
                    if (execNumber < 0)
                    {
                        return "箱号信息更新失败!";
                    }
                }
                else
                {
                    //关联包装信息
                    strSql = @"UPDATE A SET A.InmostPackageID = @PackageId FROM mesUnitDetail A WHERE A.KitSerialNumber = @S_UPCSN";
                    execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@S_UPCSN", S_UPCSN),
                                                                         new SqlParameter("@PackageId", boxID));
                    if (execNumber < 0)
                    {
                        return "UPC SN关联箱号数据失败!";
                    }

                    string strResult = SetmesHistory(PartID, ProductionOrderID, S_UPCSN, S_CartonSN, loginList);
                    if (strResult != "1")
                    {
                        return strResult;
                    }
                }
                return "1";
            }
            catch (Exception ex)
            {
                return "程序异常:" + ex.Message.ToString();
            }
        }

        /// <summary>
        /// 包装过站记录
        /// </summary>
        /// <param name="PartID"></param>
        /// <param name="ProductionOrderID"></param>
        /// <param name="S_UPCSN"></param>
        /// <param name="S_CartonSN"></param>
        /// <param name="loginList"></param>
        /// <returns></returns>
        public string SetmesHistory(string PartID, string ProductionOrderID, string S_UPCSN, string S_CartonSN, LoginList loginList)
        {
            try
            {
                string strSql = string.Empty;
                int execNumber = 0;

                strSql = @"SELECT UnitID,B.ProductionOrderID FROM mesSerialNumber A INNER JOIN mesUnit B ON A.UnitID=B.ID Where Value=@S_UPCSN";
                DataTable dtUPCUnit = SqlServerHelper.ExecuteDataTable(strSql, new SqlParameter("@S_UPCSN", S_UPCSN));
                if (dtUPCUnit == null || dtUPCUnit.Rows.Count == 0)
                {
                    return "UPC SN获取UnitID失败!";
                }
                int UPCUnitID = Convert.ToInt32(dtUPCUnit.Rows[0]["UnitID"].ToString());
                int UPCProductionOrderID = Convert.ToInt32(dtUPCUnit.Rows[0]["ProductionOrderID"].ToString());

                //UPC SN更新mesUnit
                DataSet dsUinitStateUPC = GetMesUnitStateID(Convert.ToInt32(PartID), loginList.StationID);
                int UnitStateIDUPC = Convert.ToInt32(dsUinitStateUPC.Tables[0].Rows[0]["UnitStateID"].ToString());

                strSql = @"UPDATE mesUnit SET UnitStateID=@UnitStateID,StatusID=1,EmployeeID=@EmployeeID,PartID=@PartID,LineID=@LineID,
                           ProductionOrderID=@ProductionOrderID,StationID=@StationID,LastUpdate=GETDATE() WHERE ID=@UnitID";
                execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@EmployeeID", loginList.EmployeeID),
                                                                     new SqlParameter("@UnitID", UPCUnitID),
                                                                     new SqlParameter("@UnitStateID", UnitStateIDUPC),
                                                                     new SqlParameter("@StationID", loginList.StationID),
                                                                     new SqlParameter("@PartID", PartID),
                                                                     new SqlParameter("@ProductionOrderID", ProductionOrderID),
                                                                     new SqlParameter("@LineID", loginList.LineID)
                                                                     );
                if (execNumber < 0)
                {
                    return "更新UPC SN的Unit数据表失败!";
                }

                //UPC SN生成过站记录
                mesHistoryDAL mesHistoryDll = new mesHistoryDAL();
                mesHistory v_mesHistory = new mesHistory();
                v_mesHistory.UnitID = UPCUnitID;
                v_mesHistory.UnitStateID = UnitStateIDUPC;
                v_mesHistory.EmployeeID = loginList.EmployeeID;
                v_mesHistory.StationID = loginList.StationID;
                v_mesHistory.EnterTime = DateTime.Now;
                v_mesHistory.ExitTime = DateTime.Now;
                v_mesHistory.ProductionOrderID = UPCProductionOrderID;
                v_mesHistory.PartID = Convert.ToInt32(PartID);
                v_mesHistory.LooperCount = 1;
                mesHistoryDll.Insert(v_mesHistory);

                //FG SN更新mesUnit
                strSql = @"SELECT A.ID,A.PartID,A.ProductionOrderID FROM mesUnit A inner join mesUnitDetail B on A.ID=B.UnitID 
	                            WHERE B.KitSerialNumber=@KitSerialNumber";
                DataTable dtFGUnit = SqlServerHelper.ExecuteDataTable(strSql, new SqlParameter("@KitSerialNumber", S_UPCSN));
                if (dtFGUnit == null || dtFGUnit.Rows.Count == 0)
                {
                    return "UPC SN没有关联FG SN数据，FG SN履历记录增加失败!";
                }
                int FGUintID = Convert.ToInt32(dtFGUnit.Rows[0]["ID"].ToString());
                int FGPartID = Convert.ToInt32(dtFGUnit.Rows[0]["PartID"].ToString());
                int FGProductionOrderID = Convert.ToInt32(dtFGUnit.Rows[0]["ProductionOrderID"].ToString());

                DataSet dsUinitStateFG = GetMesUnitStateID(FGPartID, loginList.StationID);
                int UnitStateIDFG = Convert.ToInt32(dsUinitStateFG.Tables[0].Rows[0]["UnitStateID"].ToString());

                strSql = @"UPDATE mesUnit SET UnitStateID=@UnitStateID,StatusID=1,EmployeeID=@EmployeeID,LastUpdate=GETDATE(),LineID=@LineID,
                            StationID=@StationID,PartID=@PartID, ProductionOrderID=@ProductionOrderID WHERE ID=@UnitID";
                execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@EmployeeID", loginList.EmployeeID),
                                                                     new SqlParameter("@UnitID", FGUintID),
                                                                     new SqlParameter("@UnitStateID", UnitStateIDFG),
                                                                     new SqlParameter("@StationID", loginList.StationID),
                                                                     new SqlParameter("@PartID", PartID),
                                                                     new SqlParameter("@ProductionOrderID", ProductionOrderID),
                                                                     new SqlParameter("@LineID", loginList.LineID)
                                                                     );
                if (execNumber < 0)
                {
                    return "更新UPC SN的Unit数据表失败!";
                }

                //UPC SN生成过站记录
                v_mesHistory.UnitID = FGUintID;
                v_mesHistory.UnitStateID = UnitStateIDFG;
                v_mesHistory.EmployeeID = loginList.EmployeeID;
                v_mesHistory.StationID = loginList.StationID;
                v_mesHistory.EnterTime = DateTime.Now;
                v_mesHistory.ExitTime = DateTime.Now;
                v_mesHistory.ProductionOrderID = FGProductionOrderID;
                v_mesHistory.PartID = FGPartID;
                v_mesHistory.LooperCount = 1;
                mesHistoryDll.Insert(v_mesHistory);

                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        /// <summary>
        /// 栈板校验
        /// </summary>
        /// <param name="S_FormatName"></param>
        /// <param name="xmlProdOrder"></param>
        /// <param name="xmlPart"></param>
        /// <param name="xmlStation"></param>
        /// <param name="xmlExtraData"></param>
        /// <param name="strSNbuf"></param>
        /// <returns></returns>
        public string uspPalletCheck(string S_FormatName, string xmlProdOrder, string xmlPart, string xmlStation, string xmlExtraData, string strSNbuf)
        {
            string result;
            try
            {
                if (string.IsNullOrEmpty(S_FormatName))
                {
                    result = "参数S_FormatName不能为空。";
                }
                else
                {
                    xmlProdOrder = string.IsNullOrEmpty(xmlProdOrder) ? "null" : xmlProdOrder;
                    xmlPart = string.IsNullOrEmpty(xmlPart) ? "null" : xmlPart;
                    xmlStation = string.IsNullOrEmpty(xmlStation) ? "null" : xmlStation;
                    xmlExtraData = string.IsNullOrEmpty(xmlExtraData) ? "null" : xmlExtraData;

                    SqlParameter[] commandParameters = new SqlParameter[]
                    {
                        new SqlParameter("@strSNFormat", S_FormatName),
                        new SqlParameter("@xmlProdOrder", xmlProdOrder),
                        new SqlParameter("@xmlPart", xmlPart),
                        new SqlParameter("@xmlStation", xmlStation),
                        new SqlParameter("@xmlExtraData", xmlExtraData),
                        new SqlParameter("@strSNbuf", strSNbuf),
                        new SqlParameter("@strOutput", "")
                    };

                    result = SqlServerHelper.ExecuteScalar(CommandType.StoredProcedure, "uspPalletCheck", commandParameters).ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                return "程序异常:" + ex.Message.ToString();
            }
        }

        /// <summary>
        /// 栈板包装
        /// </summary>
        /// <param name="PartID"></param>
        /// <param name="S_CartonSN"></param>
        /// <param name="S_PalletSN"></param>
        /// <param name="loginList"></param>
        /// <param name="BoxQty"></param>
        /// <returns></returns>
        public string uspPalletPackaging(string PartID, string ProductionOrderID, string S_CartonSN, string S_PalletSN, LoginList loginList, int PalletQty = 0)
        {
            try
            {
                int execNumber = 0;
                string strSql = @"SELECT ID FROM mesPackage WHERE SerialNumber=@S_PalletSN AND StatusID=0 AND Stage=2";

                DataTable dtMp = SqlServerHelper.ExecuteDataTable(strSql, new SqlParameter("@S_PalletSN", S_PalletSN));
                if (dtMp == null || dtMp.Rows.Count == 0)
                {
                    return "栈板:" + S_PalletSN + ",不存在或者状态不符";
                }
                string PalletID = dtMp.Rows[0]["ID"].ToString();

                if (PalletQty > 0)
                {
                    strSql = @"INSERT INTO mesPackageHistory(PackageID, PackageStatusID, StationID, EmployeeID, Time)
                                VALUES(@PackageId, 1, @stationId, @employeeId, GETDATE())";
                    execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@PackageId", PalletID),
                                                                         new SqlParameter("@stationId", loginList.StationID),
                                                                         new SqlParameter("@employeeId", loginList.EmployeeID));
                    if (execNumber < 0)
                    {
                        return "栈板过站履历记录失败!";
                    }

                    strSql = @"UPDATE mesPackage SET CurrentCount=@packCount,StationID=@stationId,EmployeeID=@employeeId,
				                StatusID=1,LastUpdate=GETDATE() where ID=@PackageId";
                    execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@PackageId", PalletID),
                                                                         new SqlParameter("@stationId", loginList.StationID),
                                                                         new SqlParameter("@employeeId", loginList.EmployeeID),
                                                                         new SqlParameter("@packCount", PalletQty));
                    if (execNumber < 0)
                    {
                        return "栈板信息更新失败!";
                    }
                }
                else
                {
                    //关联包装信息
                    strSql = @"UPDATE A SET A.ParentID=@ParentID  FROM mesPackage A WHERE A.SerialNumber=@SerialNumber";
                    execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@SerialNumber", S_CartonSN),
                                                                         new SqlParameter("@ParentID", PalletID));
                    if (execNumber < 0)
                    {
                        return "箱号关联栈板数据失败!";
                    }

                    //记录包装数据所有UPC/FG履历信息
                    strSql = @"select A.KitSerialNumber from mesUnitDetail A inner join mesPackage B 
                                on a.InmostPackageID=b.ID where b.SerialNumber=@S_CartonSN";
                    DataTable dtUPCUnit = SqlServerHelper.ExecuteDataTable(strSql, new SqlParameter("@S_CartonSN", S_CartonSN));
                    if (dtUPCUnit == null || dtUPCUnit.Rows.Count == 0)
                    {
                        return "未找到改包装的UPC信息!";
                    }
                    string strResult = string.Empty;
                    foreach (DataRow dr in dtUPCUnit.Rows)
                    {
                        strResult = SetmesHistory(PartID, ProductionOrderID, dr["KitSerialNumber"].ToString(), S_CartonSN, loginList);
                        if (strResult != "1")
                        {
                            return strResult;
                        }
                    }
                }
                return "1";
            }
            catch (Exception ex)
            {
                return "程序异常:" + ex.Message.ToString();
            }
        }

        public DataSet GetProductionOrderDetailDef(string ProductionOrderNumber)
        {
            string S_Sql = @"SELECT C.Description,B.Content FROM mesProductionOrder A INNER JOIN mesProductionOrderDetail B
	                            ON A.ID=B.ProductionOrderID INNER JOIN luProductionOrderDetailDef C
	                            ON B.ProductionOrderDetailDefID=C.ID WHERE A.ID='" + ProductionOrderNumber + @"'";
            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }

        /// <summary>
        /// 生成SN并注册包装表
        /// </summary>
        /// <param name="S_FormatName"></param>
        /// <param name="xmlProdOrder"></param>
        /// <param name="xmlPart"></param>
        /// <param name="xmlStation"></param>
        /// <param name="xmlExtraData"></param>
        /// <param name="v_mesUnit"></param>
        /// <param name="strSN"></param>
        /// <param name="type">1:包装SN  2：栈板SN</param>
        /// <returns></returns>
        public string Get_CreatePackageSN(string S_FormatName, string xmlProdOrder, string xmlPart, string xmlStation,
                                        string xmlExtraData, mesUnit v_mesUnit, ref string strSN, int type)
        {
            try
            {
                string result = "1";

                //生成条码
                DataTable DT = uspSNRGetNext(S_FormatName, 0, "'" + xmlProdOrder + "'", "'" + xmlPart + "'", "'" + xmlStation + "'", "'" + xmlExtraData + "'", "").Tables[1];
                string S_SN = DT.Rows[0][0].ToString();
                if (string.IsNullOrEmpty(S_SN))
                {
                    return result = "条码生成失败!";
                }

                //生成条码插入包装表
                string strSql = @"INSERT INTO mesPackage(ID,SerialNumber,StationID,EmployeeID,CreationTime,StatusID,LastUpdate,Stage,CurrProductionOrderID,CurrPartID)
				                VALUES ((SELECT MAX(ID) FROM mesPackage)+1,@BoxSN,@stationId,@employeeId,GETDATE(),0,GETDATE(),@type,@prodId,@partId)";
                int execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@BoxSN", S_SN),
                                                                         new SqlParameter("@stationId", v_mesUnit.StationID),
                                                                         new SqlParameter("@employeeId", v_mesUnit.EmployeeID),
                                                                         new SqlParameter("@type", type),
                                                                         new SqlParameter("@prodId", v_mesUnit.ProductionOrderID),
                                                                         new SqlParameter("@partId", v_mesUnit.PartID));
                if (execNumber < 0)
                {
                    return "生成SN插入包装表失败!";
                }
                strSN = S_SN;
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public DataSet Get_PackageData(string S_SN)
        {
            string S_Sql = @"SELECT ROW_NUMBER() OVER(ORDER BY A.ID) SEQNO, KitSerialNumber UPCSN,C.LastUpdate TIME 
                                FROM mesPackage A INNER JOIN mesUnitDetail B ON A.ID=B.InmostPackageID 
                                inner join mesUnit C on B.UnitID=C.ID
                                where A.Stage=1 and A.SerialNumber= '" + S_SN + "'";
            DataSet DS = SqlServerHelper.Data_Set(S_Sql);
            return DS;
        }

        public DataSet Get_PalletData(string S_SN)
        {
            string S_Sql = @"SELECT ROW_NUMBER() OVER(ORDER BY A.ID) SEQNO, B.SerialNumber KITSN,B.LastUpdate TIME 
	                            FROM mesPackage A INNER JOIN mesPackage B ON A.ID=B.ParentID WHERE A.Stage=2 AND A.SerialNumber= '" + S_SN + "'";
            DataSet DS = SqlServerHelper.Data_Set(S_Sql);
            return DS;
        }

        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="S_FormatName"></param>
        /// <param name="xmlProdOrder"></param>
        /// <param name="xmlPart"></param>
        /// <param name="xmlStation"></param>
        /// <param name="xmlExtraData"></param>
        /// <param name="strSNbuf"></param>
        /// <returns></returns>
        public DataSet Get_SearchData(string S_FormatName, string xmlProdOrder, string xmlPart, string xmlStation, string xmlExtraData, string strSNbuf, ref string strOutput)
        {
            strOutput = "1";
            DataSet ds = new DataSet();
            try
            {
                S_FormatName = string.IsNullOrEmpty(S_FormatName) ? "null" : S_FormatName;
                xmlProdOrder = string.IsNullOrEmpty(xmlProdOrder) ? "null" : xmlProdOrder;
                xmlPart = string.IsNullOrEmpty(xmlPart) ? "null" : xmlPart;
                xmlStation = string.IsNullOrEmpty(xmlStation) ? "null" : xmlStation;
                xmlExtraData = string.IsNullOrEmpty(xmlExtraData) ? "null" : xmlExtraData;
                strOutput = string.IsNullOrEmpty(strOutput) ? "null" : strOutput;

                string S_Sql = "exec [dbo].[uspSearchData] '" + S_FormatName + "','" + xmlProdOrder + "','" + xmlPart
                    + "','" + xmlStation + "','" + xmlExtraData + "','" + strSNbuf + "',''";
                ds = SqlServerHelper.Data_Set(S_Sql);
            }
            catch (Exception ex)
            {
                strOutput = ex.Message.ToString();
            }
            return ds;
        }

        /// <summary>
        /// 过程调用
        /// </summary>
        /// <param name="S_FormatName"></param>
        /// <param name="xmlProdOrder"></param>
        /// <param name="xmlPart"></param>
        /// <param name="xmlStation"></param>
        /// <param name="xmlExtraData"></param>
        /// <param name="strSNbuf"></param>
        /// <returns></returns>
        public DataSet uspCallProcedure(string Pro_Name, string S_FormatName, string xmlProdOrder, string xmlPart,
                                        string xmlStation, string xmlExtraData, string strSNbuf, ref string strOutput)
        {
            strOutput = "1";
            DataSet dataSet = new DataSet();
            try
            {
                if (string.IsNullOrEmpty(Pro_Name))
                {
                    strOutput = "过程名称不能为空!";
                    return null;
                }
                S_FormatName = string.IsNullOrEmpty(S_FormatName) ? "null" : S_FormatName;
                xmlProdOrder = string.IsNullOrEmpty(xmlProdOrder) ? "null" : xmlProdOrder;
                xmlPart = string.IsNullOrEmpty(xmlPart) ? "null" : xmlPart;
                xmlStation = string.IsNullOrEmpty(xmlStation) ? "null" : xmlStation;
                xmlExtraData = string.IsNullOrEmpty(xmlExtraData) ? "null" : xmlExtraData;
                strSNbuf = string.IsNullOrEmpty(strSNbuf) ? "null" : strSNbuf;

                SqlParameter[] commandParameters = new SqlParameter[]
                {
                        new SqlParameter("@strSNFormat", S_FormatName),
                        new SqlParameter("@xmlProdOrder", xmlProdOrder),
                        new SqlParameter("@xmlPart", xmlPart),
                        new SqlParameter("@xmlStation", xmlStation),
                        new SqlParameter("@xmlExtraData", xmlExtraData),
                        new SqlParameter("@strSNbuf", strSNbuf),
                        new SqlParameter("@strOutput", SqlDbType.VarChar,64)
                };

                commandParameters[6].Direction = ParameterDirection.Output;
                dataSet = SqlServerHelper.ExecuteNonQueryPro(Pro_Name, commandParameters);
                strOutput = commandParameters[6].Value.ToString();
                return dataSet;
            }
            catch (Exception ex)
            {
                strOutput = "程序异常:" + ex.Message.ToString();
                return null;
            }
        }

        public DataSet Get_PartParameter(string PartID)
        {
            string S_Sql = string.Format(@"SELECT A.ID AS PartID,'' AS Barcode,          
			                    C.Content AS Prefix,A.PartNumber partDesc FROM mesPart A 
		                    INNER JOIN mesProductStructure D ON A.ID=D.PartID
		                    LEFT JOIN mesPartDetail C ON A.ID=C.PartID
		                    LEFT JOIN luPartDetailDef B ON B.ID=C.PartDetailDefID AND B.Description='Prefix_Pattern'
		                    WHERE D.ParentPartID={0}", PartID);
            DataSet DS = SqlServerHelper.Data_Set(S_Sql);
            return DS;
        }

        /// <summary>
        /// 获取BOM表关系
        /// </summary>
        /// <param name="ParentPartID"></param>
        /// <returns></returns>
        public DataSet MESGetBomPartInfo(int ParentPartID,int StationTypeID)
        {
            string S_Sql = string.Format("SELECT * FROM DBO.ufnGetBomPartInfoByStationType({0},{1})", ParentPartID, StationTypeID);
            DataSet DS = SqlServerHelper.Data_Set(S_Sql);
            return DS;
        }


        /// <summary>
        /// 校验主条码
        /// </summary>
        /// <param name="PartID"></param>
        /// <param name="LineID"></param>
        /// <param name="StationID"></param>
        /// <param name="StationTypeID"></param>
        /// <param name="SN"></param>
        /// <returns></returns>
        public string MESAssembleCheckMianSN(string ProductionOrderID,int LineID,int StationID,int StationTypeID,string SN)
        {
            DataTable DT_SN = GetmesSerialNumber(SN).Tables[0];
            if (DT_SN == null || DT_SN.Rows.Count == 0)
            {
                return string.Format("SN：{0} 条码不存在！", SN);
            }

            string S_SerialNumberType = DT_SN.Rows[0]["SerialNumberTypeID"].ToString();
            if (S_SerialNumberType != "5")
            {
                return string.Format("SN：{0} 此条码类别不匹配！", SN);
            }

            string S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
            DataTable DT_Unit = GetmesUnit(S_UnitID).Tables[0];
            if (DT_Unit.Rows[0]["StatusID"].ToString() != "1")
            {
                return string.Format("SN：{0} 此条码已NG！", SN);
            }

            string UnitProductionOrderID = DT_Unit.Rows[0]["ProductionOrderID"].ToString();
            if (UnitProductionOrderID != ProductionOrderID)
            {
                return string.Format("SN：{0} 条码和当前选择工单料号不一致！", SN);
            }

            string S_RouteCheck = GetRouteCheck(StationTypeID, StationID, LineID.ToString(), DT_Unit, SN);
            return S_RouteCheck;
        }

        /// <summary>
        /// 校验其他类型条码(存在系统中条码)
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public string MESAssembleCheckOtherSN(string SN, string PartID)
        {
            DataTable DT_SN = GetmesSerialNumber(SN).Tables[0];
            string S_UnitID = string.Empty;

            if (DT_SN == null || DT_SN.Rows.Count == 0)
            {
                return string.Format("SN：{0} 条码不存在!", SN);
            }

            string S_SerialNumberType = DT_SN.Rows[0]["SerialNumberTypeID"].ToString();
            if (S_SerialNumberType != "5")
            {
                return string.Format("SN：{0} 此条码类别不匹配！", SN);
            }

            S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
            DataTable DT_Unit = GetmesUnit(S_UnitID).Tables[0];
            if (DT_Unit.Rows[0]["StatusID"].ToString() != "1")
            {
                return string.Format("SN：{0} 此条码已NG！", SN);
            }

            string UnitPart = DT_Unit.Rows[0]["PartID"].ToString();
            if (UnitPart != PartID)
            {
                //替代料逻辑预留
                return string.Format("SN：{0} 子料和主料未绑定关系！", SN);
            }

            mesRouteDAL mesRouteDAL = new mesRouteDAL();

            if (!mesRouteDAL.MESCheckLastStation(Convert.ToInt32(S_UnitID)))
            {
                return string.Format("SN：{0} 子料对应的工艺路线未完成！", SN);
            }

            string sql = string.Format(@"select 1 from mesUnitComponent A INNER JOIN mesSerialNumber B
                            ON A.UnitID = B.UnitID AND A.ChildSerialNumber ='{0}'", SN);
            DataTable dt = SqlServerHelper.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                return string.Format("SN：{0} 子料已存在绑定关系,不能重复绑定！", SN);
            }
            return "1";
        }

        /// <summary>
        /// 校验虚拟条码
        /// </summary>
        /// <param name="SN">子料SN</param>
        /// <param name="PartID">料号</param>
        /// <returns></returns>
        public string MESAssembleCheckVirtualSN(string SN, string PartID, string Status)
        {
            DataTable DT_Machine = GetmesMachine(SN).Tables[0];
            if (DT_Machine == null || DT_Machine.Rows.Count == 0)
            {
                return string.Format("SN：{0} 条码不存在!", SN);
            }

            //状态检查
            string S_StatusID = DT_Machine.Rows[0]["StatusID"].ToString();
            if (S_StatusID != Status)
            {
                return string.Format("MachineSN：{0} 状态不符，请确认！", SN);
            }

            string S_MachineFamilyID = DT_Machine.Rows[0]["MachineFamilyID"].ToString();
            string S_MachinePartID = DT_Machine.Rows[0]["PartID"].ToString();
            string Str_Sql = string.Format(@"select top 1 PartID from  mesRouteMachineMap where (MachineFamilyID = CASE WHEN ISNULL({0},'')='' THEN MachineFamilyID ELSE {0}  END
	                                            OR MachineFamilyID = CASE WHEN ISNULL({1},'')='' THEN MachineFamilyID ELSE {1} END) 
                                                    AND (ISNULL({0},'')<>'' OR ISNULL({1},'')<>'')", S_MachineFamilyID, S_MachinePartID);
            DataTable dtMap = SqlServerHelper.ExecuteDataTable(Str_Sql);

            if (dtMap==null || dtMap.Rows.Count == 0)
            {
                return string.Format("MachineSN：{0} 未分配任何料号！", SN);
            }

            if (!string.IsNullOrEmpty(PartID))
            {
                string S_MapPartID = dtMap.Rows[0]["PartID"].ToString();

                if (PartID != S_MapPartID)
                {
                    //替代料逻辑预留

                    return string.Format("MachineSN：{0} 分配料号与当前料号不匹配！", SN);
                }
            }

            //Str_Sql = string.Format(@"SELECT MAX(B.ID) ID FROM mesUnitDetail A INNER JOIN mesUnit B ON A.UnitID=B.ID
            //            WHERE A.reserved_01='{0}' AND reserved_03=1",SN);
            //DataTable dtUnt = SqlServerHelper.ExecuteDataTable(Str_Sql);
            //if (dtUnt == null || dtUnt.Rows.Count == 0)
            //{
            //    return string.Format("SN：{0} 未找到已绑定的数据！", SN);
            //}
            //string VirtualUnitID= dtUnt.Rows[0]["ID"].ToString();
            //mesRouteDAL mesRouteDAL = new mesRouteDAL();

            //if (!mesRouteDAL.MESCheckLastStation(Convert.ToInt32(VirtualUnitID)))
            //{
            //    return string.Format("SN：{0} 子料对应的工艺路线未完成！", SN);
            //}
            return "1";
        }

        /// <summary>
        /// 非系统数据保存
        /// </summary>
        /// <param name="SN">子料SN</param>
        /// <param name="PartID">料号</param>
        /// <returns></returns>
        public void MESModifyUnitDetail(int UnitID, string FileName,string Value)
        {
            string S_Sql = string.Format("Update mesUnitDetail set {0}='{1}' WHERE UnitID ={2}", FileName, Value, UnitID);
            SqlServerHelper.ExecSql(S_Sql);
        }

        /// <summary>
        /// 获取SN对应的单元状态
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public string MESGetUnitUnitStateID(string SN)
        {
            string UnitStateID = string.Empty;
            try
            {
                string S_Sql = string.Format(@"select CASE WHEN D.UnitStateID=B.UnitStateID THEN 1 ELSE 0 END UnitStateID from mesSerialNumber A  
                                                inner join mesUnit B on a.UnitID = b.ID
                                                inner join mesStation C on b.StationID = C.ID
                                                inner join mesRouteDetail D ON C.StationTypeID = D.StationTypeID
                                                where A.Value = '{0}' AND D.RouteID = dbo.ufnRTEGetRouteID(null, b.PartID, null)", SN);
                DataTable dts = SqlServerHelper.ExecuteDataTable(S_Sql);
                if (dts != null && dts.Rows.Count > 0)
                {
                    UnitStateID = dts.Rows[0]["UnitStateID"].ToString();
                }
                return UnitStateID;
            }
            catch (Exception ex)
            {
                return "程序异常:" + ex.Message.ToString();
            }
        }

        /// <summary>
        /// 获取工站类型参数
        /// </summary>
        /// <param name="stationTypeID"></param>
        /// <returns></returns>
        public DataSet MESGetStationTypeParameter(int stationTypeID)
        {
            string strSql = String.Format(@" SELECT A.Content,B.Description FROM mesStationTypeDetail A
	                                LEFT JOIN luStationTypeDetailDef B ON A.StationTypeDetailDefID = B.ID 
                                 WHERE A.StationTypeID ={0}", stationTypeID);

            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }


        public DataSet GetSerialNumber2(string S_SN)
        {
            string S_Sql = "select * from mesSerialNumber A join mesUnit B on A.UnitID=B.ID where A.Value='" + S_SN + "'";
            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }

    }
}
