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
using System.Text.RegularExpressions;
using System.Threading;
using System.Configuration;
using System.Reflection;

namespace App.DBServerDAL
{
    public class PartSelectDAL
    {
        public double MyTest()
        {
            DateTime dateStartC = DateTime.Now;

            var v=  SqlServerHelper.GetConnection();

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
            string[] list_IP = null;
            try
            {
                string S_Conn = SqlServerHelper.default_connection_str;
                string[] List_Conn = S_Conn.Split('=');
                list_IP = List_Conn[1].Split(';');
            }
            catch
            {
                list_IP[0] = "";
            }

            return list_IP[0];
        }

        public string GetDBName()
        {
            string S_Conn = SqlServerHelper.default_connection_str;
            string[] List_Conn = S_Conn.Split('=');

            string[] list_IP = List_Conn[2].Split(';');

            return list_IP[0];
        }

        public string GetConn()
        {
            return SqlServerHelper.default_connection_str;
        }
        public DataSet GetluPartFamilyType()
        {
            //string strSql = "select * from luPartFamilyType where Status=1 order by Name ";
            string strSql = @"SELECT DISTINCT(A.id) ID,A.Name, A.[Description], A.[Status]
                      from luPartFamilyType A
                        JOIN luPartFamily B ON A.ID = B.PartFamilyTypeID
                        JOIN mesPart C ON B.ID = C.PartFamilyID
                      where A.Status = 1
                    order by A.NAME";
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
            DataSet ds = SqlServerHelper.Data_Set(strSql+ "  ORDER BY C.Name");
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

            DataSet ds = SqlServerHelper.Data_Set(strSql+ "  ORDER BY PartNumber");
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

        public string mesGetSNFormatIDByList(string PartID, string PartFamilyID, string LineID, string ProductionOrderID, string StationTypeID)
        {
            string SNFormatID = string.Empty;
            PartID = string.IsNullOrEmpty(PartID) ? "null" : PartID;
            PartFamilyID = string.IsNullOrEmpty(PartFamilyID) ? "null" : PartFamilyID;
            LineID = string.IsNullOrEmpty(LineID) ? "null" : LineID;
            ProductionOrderID = string.IsNullOrEmpty(ProductionOrderID) ? "null" : ProductionOrderID;
            StationTypeID = string.IsNullOrEmpty(StationTypeID) ? "null" : StationTypeID;
            string strSql = string.Format(@"select dbo.ufnRTEGetSNFormatID({0},{1},{2},{3},{4}) as SNFormatID", LineID, PartID, PartFamilyID, ProductionOrderID, StationTypeID);
            DataTable dataTable = SqlServerHelper.ExecuteDataTable(strSql);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                SNFormatID = dataTable.Rows[0]["SNFormatID"].ToString();
            }
            return SNFormatID;
        }


        public DataSet GetmesLine()
        {
            DataSet ds = null;
            try
            {
                string strSql = "SELECT ID,Description  FROM mesLine where StatusID=1 or StatusID is null";
                ds = SqlServerHelper.Data_Set(strSql);
            }
            catch
            {
                string strSql = "SELECT ID,Description  FROM mesLine";
                ds = SqlServerHelper.Data_Set(strSql);
            }

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

        public DataSet GetmesStation2(string StationTypeID, string LineID)
        {
            string strSql = "SELECT *  FROM mesStation where  ";
            string S1 = "  StationTypeID='" + StationTypeID + "'";
            string S2 = "  LineID='" + LineID + "'";

            if (StationTypeID != "" && LineID != "")
            {
                strSql += S1 + " and " + S2;
            }
            else
            {
                if (StationTypeID != "")
                {
                    strSql += S1;
                }

                if (LineID != "")
                {
                    strSql += S2;
                }
            }
            strSql += " and Status=1";

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

        public DataSet GetmesUnitState(string S_PartID, string PartFamilyID, string S_RouteSequence,
                                        string LineID, int StationTypeID, string ProductionOrderID, string StatusID)
        {
            DataSet DsRoute = GetRouteData(LineID, S_PartID, PartFamilyID, ProductionOrderID);
            //string RouteType = DsRoute.Tables[0].Rows[0]["RouteType"].ToString();
            if (DsRoute == null || DsRoute.Tables.Count <= 0)
            {
                return null;
            }
            string S_RouteID = DsRoute.Tables[0].Rows[0]["ID"].ToString();
            string strSql = string.Format("SELECT dbo.ufnGetUnitStateID({0},{1},{2}) AS UnitStateID", 
                S_RouteID, StationTypeID, StatusID);

            //if (RouteType == "1")
            //{
            //    strSql = string.Format(@"select top 1 OutputStateID as UnitStateID from mesUnitOutputState 
            //                                where RouteID = {0} and  StationTypeID = {1} and OutputStateDefID={2}",
            //        S_RouteID, StationTypeID.ToString(), StatusID);

            //}
            //else
            //{
            //    strSql = string.Format("select UnitStateID from mesRouteDetail where RouteID={0} and StationTypeID={1}",
            //        S_RouteID, StationTypeID.ToString());
            //}

            DataTable dtRoute = SqlServerHelper.Data_Table(strSql);
            if (dtRoute != null && dtRoute.Rows.Count > 0)
            {
                string S_UnitStateID = dtRoute.Rows[0]["UnitStateID"].ToString();
                if (string.IsNullOrEmpty(S_UnitStateID))
                    return null;

                strSql = "SELECT *  FROM mesUnitState where ID =" + S_UnitStateID + " order by ID desc";
                DataSet ds = SqlServerHelper.Data_Set(strSql);
                return ds;
            }
            else
            {
                return null;
            }
        }

        public string GetmesUnitStateSecond(string S_PartID, string PartFamilyID, string S_RouteSequence,
                                        string LineID, int StationTypeID, string ProductionOrderID, string StatusID, string S_SN)
        {
            string result = "Error : ";
            DataSet DsRoute = GetRouteData(LineID, S_PartID, PartFamilyID, ProductionOrderID);

            if (DsRoute == null || DsRoute.Tables.Count <= 0)
            {
                return result + "can't get route info.";
            }



            string S_RouteID = DsRoute.Tables[0].Rows[0]["ID"].ToString();
            string strSql = string.Format("SELECT dbo.ufnGetUnitStateID({0},{1},{2}) AS UnitStateID",
                S_RouteID, StationTypeID, StatusID);

            DataTable dtRoute = SqlServerHelper.Data_Table(strSql);
            if (dtRoute != null && dtRoute.Rows.Count > 0)
            {
                string S_UnitStateID = dtRoute.Rows[0]["UnitStateID"].ToString();
                if (string.IsNullOrEmpty(S_UnitStateID))
                    return result + "can't get next unit state ID.";

                string tmpSql = $@"SELECT *
                            FROM dbo.mesSerialNumber a
                            JOIN dbo.mesUnit b ON b.ID = a.UnitID
                            WHERE a.Value = '{S_SN}'";
                var unitDateTab = SqlServerHelper.ExecuteDataTable(tmpSql);
                if (unitDateTab == null || unitDateTab.Rows.Count <= 0)
                    return S_UnitStateID;

                string unitCurrentState = unitDateTab.Rows[0]["UnitStateID"].ToString();

                var looperCount = GetmesUnitStateLooperAsync(int.Parse(S_RouteID), StationTypeID, unitCurrentState, S_SN);
                if (looperCount != "1")
                    return result + looperCount;

                strSql = $"SELECT * FROM dbo.mesUnitOutputState WHERE StationTypeID = {StationTypeID} AND RouteID = {S_RouteID} AND InputStateID = {unitCurrentState}";

                var internalLine = SqlServerHelper.ExecuteDataTable(strSql);
                if (internalLine != null && internalLine.Rows.Count > 0)
                {
                    var vl = from il in internalLine.AsEnumerable()
                        where il.Field<int>("OutputStateDefID") == int.Parse(StatusID)
                        select il;
                    if (vl == null || vl.Count() <= 0)
                    {
                        return result + "Please select the correct unit state.";
                    }

                    return vl.ToList()[0]["OutputStateID"].ToString();
                }
                strSql = "SELECT *  FROM mesUnitState where ID =" + S_UnitStateID + " order by ID desc";
                DataTable dt = SqlServerHelper.Data_Table(strSql);
                if (dt == null || dt.Rows.Count <= 0)
                    return result + "Please check current unit state is error.";

                return S_UnitStateID;
            }
            else
            {
                return null;
            }
        }

        public string GetmesUnitStateLooperAsync(int RouteId, int StationTypeID, string currentUnitState, string SN)
        {
            string sql = $@"
            DECLARE @looperCountCC INT,@looperCount INT, @historyCount INT, @sn VARCHAR(200) = '{SN}',  @strOutput VARCHAR(500)='1' 
            declare @routeId int = {RouteId},@stationTypeId int = {StationTypeID}, @currentStateId INT = {currentUnitState}
            IF EXISTS (
            SELECT * 
            FROM dbo.mesUnitInputState WHERE RouteID = @routeId AND StationTypeID = @stationTypeId AND CurrStateID = @currentStateId AND ISNULL(LooperCount, '')  <> '')
            BEGIN
                select  @looperCountCC =count(*)
                from (
                SELECT RouteID,StationTypeID,CurrStateID,LooperCount
                FROM dbo.mesUnitInputState WHERE RouteID = @routeId AND StationTypeID = @stationTypeId AND CurrStateID = @currentStateId AND ISNULL(LooperCount, '')  <> ''
                GROUP BY RouteID,StationTypeID,CurrStateID,LooperCount) ab
                IF @looperCountCC > 1
                BEGIN
                    SET @strOutput = '{"please check route setting, one currect state allow set up one line."}'
                    SELECT @strOutput strOutput
                    RETURN 
                END
                SELECT TOP 1 @looperCount = LooperCount
                FROM dbo.mesUnitInputState WHERE RouteID = @routeId AND StationTypeID = @stationTypeId AND CurrStateID = @currentStateId AND ISNULL(LooperCount, '')  <> ''

                SELECT @historyCount = COUNT(*)
                FROM dbo.mesSerialNumber a
                JOIN dbo.mesHistory b ON b.UnitID = a.UnitID AND b.UnitStateID = @currentStateId
                WHERE a.Value = @sn

                IF @historyCount >= @looperCount
                BEGIN
                    SET @strOutput = '{"Retest times exceeding the limit."}'
                    SELECT @strOutput strOutput
                    RETURN 
                END
            END
            SELECT @strOutput strOutput";
            var resObj = SqlServerHelper.ExecuteScalar(sql);
            return resObj?.ToString();
        }
        public DataSet GetmesUnitState_Diagram(string S_PartID, string PartFamilyID,
                                                string LineID, int StationTypeID, string ProductionOrderID)
        {
            int I_RouteID = GetRouteID(LineID, S_PartID, PartFamilyID, ProductionOrderID);
            string strSql = string.Format("SELECT * FROM  mesUnitOutputState WHERE RouteID ={0}  AND StationTypeID = {1}",
                I_RouteID, StationTypeID);
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
                            and a.Status = 1
                            order by DefectCode,ID";

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
            Boolean B_OK = false;
            DataSet ds = null;
             int I_Count = 0;
            while (B_OK == false)
            {
                xmlProdOrder = string.IsNullOrEmpty(xmlProdOrder) ? "null" : xmlProdOrder;
                xmlPart = string.IsNullOrEmpty(xmlPart) ? "null" : xmlPart;
                xmlStation = string.IsNullOrEmpty(xmlStation) ? "null" : xmlStation;
                xmlExtraData = string.IsNullOrEmpty(xmlExtraData) ? "null" : xmlExtraData;
                strNextSN = string.IsNullOrEmpty(strNextSN) ? "null" : strNextSN;

                string S_Sql = "exec [dbo].[uspSNRGetNext] '" + strSNFormat + "'," + ReuseSNByStation +
                    "," + xmlProdOrder + "," + xmlPart + "," + xmlStation + "," + xmlExtraData + "," + strNextSN;
                ds = SqlServerHelper.Data_Set(S_Sql);
                DataTable DT = ds.Tables[1];
                string S_SN = DT.Rows[0][0].ToString();

                DataSet DS_SN = GetmesSerialNumber(S_SN);
                if (DS_SN.Tables[0].Rows.Count == 0)
                {
                    B_OK = true;
                }
                else
                {
                    if (strSNFormat.IndexOf("FG") == -1 && strSNFormat.IndexOf("KIT") == -1 && strSNFormat.IndexOf("UPC") == -1)
                    {
                        B_OK = true;
                    }
                    else
                    {
                        B_OK = false;
                    }



                    //else
                    //{
                    //    I_Count += 1;
                    //    if (I_Count > 10)
                    //    {
                    //        B_OK = true;
                    //        I_Count = 0;
                    //    }
                    //}
                }
            }
            return ds;
        }

        public void InsertBulkData(SnModel[] lsSnModels)
        {
            var tmpTable = ToDataTable<SnModel>(lsSnModels);
            using (var con = new SqlConnection(SqlServerHelper.default_connection_str))
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                using (var sqlBulk = new SqlBulkCopy((SqlConnection)con))
                {
                    sqlBulk.DestinationTableName = "tmpBinSNList";
                    for (int i = 0; i < tmpTable.Columns.Count; i++)
                    {
                        sqlBulk.ColumnMappings.Add(tmpTable.Columns[i].ColumnName, tmpTable.Columns[i].ColumnName);
                    }
                    sqlBulk.WriteToServer(tmpTable);
                }
            }
        }
        /// <summary>
        /// 把列表转换为DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        private DataTable ToDataTable<T>(IEnumerable<T> list)
        {
            var type = typeof(T);
            var properties = type.GetProperties().ToList();
            var newDt = new DataTable(type.Name);
            properties.ForEach(propertie =>
            {
                Type columnType;
                if (propertie.PropertyType.IsGenericType && propertie.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    columnType = propertie.PropertyType.GetGenericArguments()[0];
                }
                else
                {
                    columnType = propertie.PropertyType;
                }

                newDt.Columns.Add(propertie.Name, columnType);
            });

            foreach (var item in list)
            {
                var newRow = newDt.NewRow();
                properties.ForEach(propertie =>
                {
                    newRow[propertie.Name] = propertie.GetValue(item, null) ?? DBNull.Value;
                });
                newDt.Rows.Add(newRow);
            }

            return newDt;
        }

        public  List<T> DataTableToList<T>(DataTable dt) where T : new()
        {
            var list = new List<T>();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (DataRow row in dt.Rows)
            {
                var item = new T();

                foreach (var prop in props)
                {
                    // 假设DataTable的列名和Model的属性名相同  
                    if (dt.Columns.Contains(prop.Name))
                    {
                        // 尝试将DataRow中的值转换为属性类型  
                        try
                        {
                            // 使用Convert.ChangeType来处理类型转换  
                            object value = row[prop.Name];
                            if (value != DBNull.Value)
                            {
                                value = Convert.ChangeType(value, prop.PropertyType);
                                prop.SetValue(item, value, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            // 处理或记录类型转换失败的情况  
                            Console.WriteLine($"无法将值转换为 {prop.PropertyType.Name} 类型: {ex.Message}");
                            LoggerHelper.WriteErrorLog($"无法将值转换为 {prop.PropertyType.Name} 类型: {ex.Message}");
                        }
                    }
                }

                list.Add(item);
            }

            return list;
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




        public DataSet GetRoute(string S_RouteSequence, int I_RouteID)
        {
            DataSet dsRoute = null;
            string routeID = I_RouteID.ToString();

            string S_Sql = string.Format(@"select A.* from 
                            (select B.ID,B.Name,B.Description RouteName,B.RouteType,
			                            A.StationTypeID,A.Sequence SequenceMod,A.Description RouteDetailName,
			                            A.UnitStateID,                                
			                            E.Description ApplicationType,
			                            cast(ROW_NUMBER() over(order by A.Sequence) as int) as Sequence
		                            from 
			                            mesRouteDetail A
			                            join mesRoute B on A.RouteID=B.ID  		  	                             
			                            join mesStationType D on D.ID=A.StationTypeID
			                            join luApplicationType E on E.ID=D.ApplicationTypeID
	                            where B.ID={0}  
                            )A where 1=1", I_RouteID);

            if (!string.IsNullOrEmpty(S_RouteSequence))
            {
                S_Sql = S_Sql + " AND A.Sequence=" + S_RouteSequence;
            }
            dsRoute = SqlServerHelper.Data_Set(S_Sql);

            return dsRoute;
        }

        public DataSet GetRouteType(int I_RouteID)
        {
            DataSet dsRoute = null;
            string routeID = I_RouteID.ToString();

            string S_Sql = string.Format(@"select * from mesRoute where ID={0}", I_RouteID);

            dsRoute = SqlServerHelper.Data_Set(S_Sql);

            return dsRoute;
        }


        //public string GetRouteCheck(int Scan_StationTypeID, int Scan_StationID, string LineID, DataTable DT_Unit, string S_Str)
        //{
        //    string S_Result = "1";
        //    string OpenLogFile = System.Configuration.ConfigurationManager.AppSettings["OpenLogFile"];
        //    DateTime dateStart = DateTime.Now;

        //    try
        //    {
        //        string S_PartID = DT_Unit.Rows[0]["PartID"].ToString();
        //        //最后扫描工序 UnitStateID
        //        string S_UnitStateID = DT_Unit.Rows[0]["UnitStateID"].ToString();
        //        string S_UnitStationID = DT_Unit.Rows[0]["StationID"].ToString();
        //        string PartPamilyID = DT_Unit.Rows[0]["PartFamilyID"].ToString();
        //        //获取此料工序路径
        //        DataTable DT_Route = GetRoute(S_PartID, "", LineID, Scan_StationTypeID, PartPamilyID).Tables[0];
        //        //当前扫描信息
        //        string S_Sql = "select *  from mesStation where ID='" + Scan_StationID + "'";
        //        DataTable DT_Now_Scan_Station = P_DataSet(S_Sql).Tables[0];

        //        //改为按  StationTypeID  来识别
        //        int I_StationTypeID_Scan = Convert.ToInt32(DT_Now_Scan_Station.Rows[0]["StationTypeID"].ToString());
        //        //当前工站类别  工序配置信息
        //        var v_Route_Sacn = from c in DT_Route.AsEnumerable()
        //                           where c.Field<int>("StationTypeID") == Scan_StationTypeID
        //                           select c;

        //        if (v_Route_Sacn.ToList().Count() > 0)
        //        {
        //            int I_Sequence_Scan = v_Route_Sacn.ToList()[0].Field<int>("Sequence");
        //            if (I_Sequence_Scan > 1)
        //            {
        //                //最后扫描信息 (上一站扫描)
        //                S_Sql = "select *  from mesStation where ID='" + S_UnitStationID + "'";
        //                DataTable DT_Station = P_DataSet(S_Sql).Tables[0];
        //                int I_StationTypeID = Convert.ToInt32(DT_Station.Rows[0]["StationTypeID"].ToString());

        //                try
        //                {
        //                    //最后 扫描路径信息
        //                    var v_Route = from c in DT_Route.AsEnumerable()
        //                                  where c.Field<int>("StationTypeID") == I_StationTypeID
        //                                  select c;
        //                    //最后 扫描顺序
        //                    int I_Sequence = v_Route.ToList()[0].Field<int>("Sequence");
        //                    //最后扫描顺序  比 当前扫描顺序
        //                    if (I_Sequence >= I_Sequence_Scan)
        //                    {
        //                        S_Result = "20016";
        //                    }
        //                    else
        //                    {
        //                        //判断上一站是否扫描
        //                        if (I_Sequence_Scan - 1 != I_Sequence)
        //                        {
        //                            S_Result = "20017";
        //                        }
        //                    }
        //                }
        //                catch
        //                {
        //                    S_Result = "20017";
        //                }
        //            }
        //        }
        //        else
        //        {
        //            //没有配置 此工位工序
        //            S_Result = "20018";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        S_Result = ex.ToString();
        //    }
        //    finally
        //    {
        //        try
        //        {
        //            if (OpenLogFile == "Y")
        //            {
        //                TimeSpan ts = DateTime.Now - dateStart;
        //                double mill = ts.TotalMilliseconds;
        //                string logPath = CreateServerDIR();
        //                string msg = "In check(GetRouteCheck) StationID:" + Scan_StationTypeID + ",SN:"
        //                                + S_Str + ",Time：" + mill.ToString() + "ms";
        //                File.AppendAllText(logPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".log",
        //                 DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + msg + "\r\n");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            S_Result = ex.ToString();
        //        }
        //    }

        //    return S_Result;
        //}


        public int GetRouteID(string LineID, string PartID, string PartFamilyID, string ProductionOrderID)
        {
            LineID = string.IsNullOrEmpty(LineID) ? "''" : LineID;
            PartID = string.IsNullOrEmpty(PartID) ? "''" : PartID;
            PartFamilyID = string.IsNullOrEmpty(PartFamilyID) ? "''" : PartFamilyID;
            ProductionOrderID = string.IsNullOrEmpty(ProductionOrderID) ? "''" : ProductionOrderID;
            string S_Sql = string.Format("select dbo.ufnRTEGetRouteID({0},{1},{2},{3}) as RouteName",
                LineID, PartID, PartFamilyID, ProductionOrderID);
            DataTable dtRoute = SqlServerHelper.ExecuteDataTable(S_Sql);
            if (dtRoute != null && dtRoute.Rows.Count > 0 && !string.IsNullOrEmpty(dtRoute.Rows[0][0].ToString()))
            {
                return Convert.ToInt32(dtRoute.Rows[0][0].ToString());
            }
            else
            {
                return -1;
            }

        }

        public string GetRouteCheck(int Scan_StationTypeID, int Scan_StationID, string LineID,
            DataTable DT_Unit, string S_Str)
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
                string PartPamilyID = DT_Unit.Rows[0]["PartFamilyID"].ToString();
                string ProductionOrderID = DT_Unit.Rows[0]["ProductionOrderID"].ToString();
                string S_StatusID=DT_Unit.Rows[0]["StatusID"].ToString();

                //获取此料工序路径
                int I_RouteID = GetRouteID(LineID, S_PartID, PartPamilyID, ProductionOrderID);
                if (I_RouteID == -1)
                {
                    return "20195";
                }
                DataTable DT_Route = GetRouteType(I_RouteID).Tables[0];
                string S_RouteType = DT_Route.Rows[0]["RouteType"].ToString();

                if (S_RouteType == "1")
                {
                    S_Result = GetRouteDiagramCheck(Scan_StationTypeID, LineID, DT_Unit, S_Str);
                }
                else
                {
                    //当前工站类别  工序配置信息
                    DT_Route = GetRoute("", I_RouteID).Tables[0];
                    var v_Route_Sacn = from c in DT_Route.AsEnumerable()
                                       where c.Field<int>("StationTypeID") == Scan_StationTypeID
                                       select c;

                    if (v_Route_Sacn.ToList().Count() > 0)
                    {
                        if (S_StatusID == "1")
                        {

                            int I_Sequence_Scan = v_Route_Sacn.ToList()[0].Field<int>("Sequence");
                            if (I_Sequence_Scan > 1)
                            {
                                //最后扫描信息 (上一站扫描)
                                //string S_Sql = "select *  from mesStation where ID='" + S_UnitStationID + "'";
                                //DataTable DT_Station = P_DataSet(S_Sql).Tables[0];
                                ////int I_StationTypeID = Convert.ToInt32(DT_Station.Rows[0]["StationTypeID"].ToString());
                                //int I_UnitStateID = Convert.ToInt32(DT_Station.Rows[0]["UnitStateID"].ToString());

                                int I_UnitStateID = Convert.ToInt32(DT_Unit.Rows[0]["UnitStateID"].ToString());

                                try
                                {
                                    //最后 扫描路径信息
                                    var v_Route = from c in DT_Route.AsEnumerable()
                                                  where c.Field<int>("UnitStateID") == I_UnitStateID
                                                  select c;
                                    //最后 扫描顺序
                                    int I_Sequence = v_Route.ToList()[0].Field<int>("Sequence");
                                    //最后扫描顺序  比 当前扫描顺序
                                    if (I_Sequence >= I_Sequence_Scan)
                                    {
                                        S_Result = "20016";
                                    }
                                    else
                                    {
                                        //判断上一站是否扫描
                                        if (I_Sequence_Scan - 1 != I_Sequence)
                                        {
                                            S_Result = "20017";
                                        }
                                    }
                                }
                                catch
                                {
                                    S_Result = "20017";
                                }
                            }
                        }
                        else
                        {
                            S_Result = "20036";  //此条码已NG.
                        }
                    }
                    else
                    {
                        //没有配置 此工位工序
                        S_Result = "20018";
                    }
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
                        string msg = "In check(GetRouteCheck) StationID:" + Scan_StationTypeID + ",SN:"
                                        + S_Str + ",Time：" + mill.ToString() + "ms";
                        File.AppendAllText(logPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".log",
                         DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + msg + "\r\n");
                    }
                }
                catch (Exception ex)
                {
                    S_Result = ex.ToString();
                }
            }
            return S_Result;
        }


        private string GetRouteDiagramCheck(int Scan_StationTypeID, string LineID,
            DataTable DT_Unit, string S_Str)
        {
            string S_Result = "1";
            string OpenLogFile = System.Configuration.ConfigurationManager.AppSettings["OpenLogFile"];
            DateTime dateStart = DateTime.Now;

            try
            {
                string S_UnitStationID = DT_Unit.Rows[0]["StationID"].ToString();
                string PartPamilyID = DT_Unit.Rows[0]["PartFamilyID"].ToString();
                string S_PartID = DT_Unit.Rows[0]["PartID"].ToString();
                string UnitID = DT_Unit.Rows[0]["ID"].ToString();
                string ProductionOrderID = DT_Unit.Rows[0]["ProductionOrderID"].ToString();
                int I_RouteID = GetRouteID(LineID, S_PartID, PartPamilyID, ProductionOrderID);
                string xmlExtraData = "<ExtraData StationTypeID=\"" + Scan_StationTypeID + "\"> </ExtraData>";
                string strOutput = string.Empty;
                uspCallProcedure("uspRouteDiagram", UnitID, "", "",
                                                                "", xmlExtraData, I_RouteID.ToString(), ref strOutput);

                return strOutput;
                //string S_PartID = DT_Unit.Rows[0]["PartID"].ToString();
                ////最后扫描工序 UnitStateID
                //string S_UnitStateID = DT_Unit.Rows[0]["UnitStateID"].ToString();
                //int I_UnitStateID = Convert.ToInt32(S_UnitStateID);

                //string S_StatusID = DT_Unit.Rows[0]["StatusID"].ToString();
                //int I_StatusID = Convert.ToInt32(S_StatusID);

                //string S_UnitStationID = DT_Unit.Rows[0]["StationID"].ToString();
                //string PartPamilyID = DT_Unit.Rows[0]["PartFamilyID"].ToString();

                ////获取此料工序路径
                //int I_RouteID = GetRouteID(LineID, S_PartID, PartPamilyID);
                //if (I_RouteID == -1)
                //{
                //    return "20195";
                //}


                ////string S_Sql = "select *  from mesStation where ID= " + S_UnitStationID;
                ////DataTable DT_Station= SqlServerHelper.Data_Table(S_Sql);
                //string S_StationTypeID = Scan_StationTypeID.ToString();

                //string S_Sql = "select * from mesUnitOutputState where RouteID=" + I_RouteID.ToString() +
                //              " and StationTypeID=" + S_StationTypeID;
                //DataTable DT_Route = SqlServerHelper.Data_Table(S_Sql);
                //if (DT_Route.Rows.Count == 0)
                //{
                //    S_Result = "20203";
                //}
                //else
                //{
                //    string S_CurrStateID = DT_Route.Rows[0]["CurrStateID"].ToString();
                //    if (S_CurrStateID == "0")
                //    {
                //        I_UnitStateID = 0;
                //    }

                //    if (I_UnitStateID == 1)
                //    {
                //        I_UnitStateID = Convert.ToInt32(S_CurrStateID);
                //    }

                //    //DataTable DT_Route = GetRoute("", I_RouteID).Tables[0];

                //    //当前工站类别  工序配置信息
                //    var v_Route_Sacn = from c in DT_Route.AsEnumerable()
                //                       where c.Field<int>("StationTypeID") == Scan_StationTypeID &&
                //                             c.Field<int>("CurrStateID") == I_UnitStateID &&
                //                             c.Field<int>("OutputStateDefID") == I_StatusID
                //                       select c;

                //    if (v_Route_Sacn != null)
                //    {
                //        if (v_Route_Sacn.ToList().Count() > 0)
                //        {
                //            //var v_Query_OutputStateID = (from c in v_Route_Sacn select c).FirstOrDefault();
                //            //S_OutputStateID = v_Query_OutputStateID.Field<int>("OutputStateID").ToString();
                //        }
                //        else
                //        {
                //            //"路径检查失败，请检查：是否NG / 是否前段工序未扫描 / 是否过站 / 是否没有没配本站";
                //            S_Result = "20240";
                //        }
                //    }
                //    else
                //    {
                //        S_Result = "20240";
                //    }
                //}


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
                        string msg = "In check(GetRouteDiagramCheck) StationID:" + Scan_StationTypeID + ",SN:"
                                        + S_Str + ",Time：" + mill.ToString() + "ms";
                        File.AppendAllText(logPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".log",
                         DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + msg + "\r\n");
                    }
                }
                catch (Exception ex)
                {
                    S_Result = ex.ToString();
                }
            }
            return S_Result;
        }

        //public DataSet GetRoute2(string LineID, string PartID, string PartFamilyID)
        //{
        //    string S_Sql = "select *  from mesRouteDetail WHERE RouteID=dbo.ufnRTEGetRouteID("+
        //                    LineID + ","+ PartID + ","+ PartFamilyID + ")";
        //    DataSet ds = SqlServerHelper.Data_Set(S_Sql);
        //    return ds;
        //}

        //public string GetRouteCheck2(LoginList List_Login, DataTable DT_Unit, string S_Str)
        //{
        //    string S_Result = "1";
        //    string OpenLogFile = System.Configuration.ConfigurationManager.AppSettings["OpenLogFile"];
        //    DateTime dateStart = DateTime.Now; 

        //    try
        //    {
        //        string S_PartID = DT_Unit.Rows[0]["PartID"].ToString();
        //        //最后扫描工序 UnitStateID
        //        //string S_UnitStateID = DT_Unit.Rows[0]["UnitStateID"].ToString();
        //        string S_UnitStationID = DT_Unit.Rows[0]["StationID"].ToString();

        //        //获取此料工序路径
        //        DataTable DT_Route = GetRoute2(List_Login.LineID.ToString(), S_PartID, null).Tables[0];

        //        //当前工站类别  工序配置信息
        //        var v_Route_Sacn = from c in DT_Route.AsEnumerable()
        //                           where c.Field<int>("StationTypeID") == List_Login.StationTypeID
        //                           select c;

        //        if (v_Route_Sacn.ToList().Count() > 0)
        //        {
        //            int I_Sequence_Scan = v_Route_Sacn.ToList()[0].Field<int>("Sequence");
        //            if (I_Sequence_Scan > 1)
        //            {
        //                //最后扫描信息 (上一站扫描)
        //                string S_Sql = "select *  from mesStation where ID='" + S_UnitStationID + "'";
        //                DataTable DT_Station = P_DataSet(S_Sql).Tables[0];
        //                //int I_StationTypeID = Convert.ToInt32(DT_Station.Rows[0]["StationTypeID"].ToString());
        //                int I_UnitStateID=Convert.ToInt32(DT_Station.Rows[0]["UnitStateID"].ToString());

        //                try
        //                {
        //                    //最后 扫描路径信息
        //                    var v_Route = from c in DT_Route.AsEnumerable()
        //                                  where c.Field<int>("UnitStateID") == I_UnitStateID
        //                                  select c;
        //                    //最后 扫描顺序
        //                    int I_Sequence = v_Route.ToList()[0].Field<int>("Sequence");
        //                    //最后扫描顺序  比 当前扫描顺序
        //                    if (I_Sequence >= I_Sequence_Scan)
        //                    {
        //                        S_Result = "20016";
        //                    }
        //                    else
        //                    {
        //                        //判断上一站是否扫描
        //                        if (I_Sequence_Scan - 1 != I_Sequence)
        //                        {
        //                            S_Result = "20017";
        //                        }
        //                    }
        //                }
        //                catch
        //                {
        //                    S_Result = "20017";
        //                }
        //            }
        //        }
        //        else
        //        {
        //            //没有配置 此工位工序
        //            S_Result = "20018";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        S_Result = ex.ToString();
        //    }
        //    finally
        //    {
        //        try
        //        {
        //            if (OpenLogFile == "Y")
        //            {
        //                TimeSpan ts = DateTime.Now - dateStart;
        //                double mill = ts.TotalMilliseconds;
        //                string logPath = CreateServerDIR();
        //                string msg = "In check(GetRouteCheck) StationID:" + List_Login.StationTypeID + ",SN:"
        //                                + S_Str + ",Time：" + mill.ToString() + "ms";
        //                File.AppendAllText(logPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".log",
        //                 DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + msg + "\r\n");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            S_Result = ex.ToString();
        //        }
        //    }

        //    return S_Result;
        //}

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

        //public DataSet GetMesUnitStateID(int routeID, int stationId)
        //{
        //    string strSql = String.Format(@"SELECT A.UnitStateID FROM mesRouteDetail A
        //                        INNER JOIN mesStation B ON A.StationTypeID=B.StationTypeID 
        //                        WHERE A.RouteID={0} AND B.ID={1}", routeID, stationId);
        //    DataSet ds = SqlServerHelper.Data_Set(strSql);
        //    return ds;
        //}

        public string Get_CreateMesSN_New(string strSNFormat, LoginList loginList, string xmlProdOrder, string xmlPart, string xmlStation,
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
            mesUnitDetailDAL mesUnitDetailDll = new mesUnitDetailDAL();

            string S_FormatName = string.Empty;

            S_FormatName = mesGetSNFormatIDByList(v_mesUnit.PartID.ToString(), v_mesUnit.PartFamilyID.ToString(),
                v_mesUnit.LineID.ToString(), v_mesUnit.ProductionOrderID.ToString(), loginList.StationTypeID.ToString());
            if (string.IsNullOrEmpty(S_FormatName))
            {
                return result = "20132";
            }

            int routeID = GetRouteID(loginList.LineID.ToString(), v_mesUnit.PartID.ToString(),
                                        v_mesUnit.PartFamilyID.ToString(), v_mesUnit.ProductionOrderID.ToString());
            if (routeID == -1)
            {
                return result = "20195";
            }
            DataSet dtSetState = GetmesUnitState(v_mesUnit.PartID.ToString(), v_mesUnit.PartFamilyID.ToString(), "",
                loginList.LineID.ToString(), loginList.StationTypeID, v_mesUnit.ProductionOrderID.ToString(), v_mesUnit.StatusID.ToString());
            if (dtSetState == null || dtSetState.Tables.Count == 0)
            {
                return result = "20131";
            }
            DataTable dtState = dtSetState.Tables[0];
            v_mesUnit.UnitStateID = Convert.ToInt32(dtState.Rows[0][0].ToString());

            string SN_Pattern = string.Empty;
            //查询SN正则表达式规则
            DataSet dataSet = GetmesPartDetail(Convert.ToInt32(v_mesUnit.PartID), "SN_Pattern");
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                SN_Pattern = dataSet.Tables[0].Rows[0]["Content"].ToString();
            }

            try
            {

                DataTable dt = new DataTable();
                dt.Columns.Add("SN", typeof(string));

                for (; number > 0; number--)
                {

                    ////生成条码
                    //if (strSNFormat == "" || strSNFormat == null)
                    //{
                    //    strSNFormat = S_FormatName;
                    //}
                    DataTable DT = uspSNRGetNext(S_FormatName, 0, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, null).Tables[1];
                    string S_SN = DT.Rows[0][0].ToString();

                    if (!string.IsNullOrEmpty(SN_Pattern))
                    {
                        if (!Regex.IsMatch(S_SN, SN_Pattern))
                        {
                            return "20027";
                        }
                    }

                    if (string.IsNullOrEmpty(S_SN))
                    {
                        return result = "20087";
                    }

                    string S_Sql = "select 1 from mesSerialNumber where Value='" + S_SN + "'";
                    DataTable DT_SN = SqlServerHelper.Data_Table(S_Sql);
                    if (DT_SN.Rows.Count > 0)
                    {
                        continue;
                    }


                    //写入UNIT表
                    //string unitId = string.Empty;
                    //v_mesUnit.CreationTime = DateTime.Now;
                    //v_mesUnit.LastUpdate = DateTime.Now;
                    //unitId = mesUnitDll.Insert(v_mesUnit);
                    //if (string.IsNullOrEmpty(unitId) || unitId.Substring(0, 1) == "E")
                    //{
                    //    return result = "20130";
                    //}

                    ////生成条码
                    //if (strSNFormat == "" || strSNFormat == null)
                    //{
                    //    strSNFormat = S_FormatName;
                    //}

                    //DataTable DT = uspSNRGetNext(strSNFormat, 0, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, null).Tables[1];
                    //string S_SN = DT.Rows[0][0].ToString();
                    //if (string.IsNullOrEmpty(S_SN))
                    //{
                    //    return result = "20087";
                    //}

                    //写入mesSerialNumber
                    //mesSerialNumber v_mesSerialNumber = new mesSerialNumber();
                    //v_mesSerialNumber.UnitID = Convert.ToInt32(unitId);
                    //v_mesSerialNumber.SerialNumberTypeID = v_mesUnit.SerialNumberType;
                    //v_mesSerialNumber.Value = S_SN;
                    //mesSerialNumberDll.Insert(v_mesSerialNumber);

                    ////写入UnitDetail表
                    //mesUnitDetail msDetail = new mesUnitDetail();
                    //msDetail.UnitID = Convert.ToInt32(unitId);
                    //msDetail.reserved_01 = "";
                    //msDetail.reserved_02 = "";
                    //msDetail.reserved_03 = "";
                    //msDetail.reserved_04 = "";
                    //msDetail.reserved_05 = "";
                    //string S_mesUnitDetailResult = mesUnitDetailDll.Insert(msDetail);

                    ////写过站履历
                    //mesHistory v_mesHistory = new mesHistory();
                    //v_mesHistory.UnitID = Convert.ToInt32(unitId);
                    //v_mesHistory.UnitStateID = v_mesUnit.UnitStateID;
                    //v_mesHistory.EmployeeID = v_mesUnit.EmployeeID;
                    //v_mesHistory.StationID = v_mesUnit.StationID;
                    //v_mesHistory.EnterTime = DateTime.Now;
                    //v_mesHistory.ExitTime = DateTime.Now;
                    //v_mesHistory.ProductionOrderID = v_mesUnit.ProductionOrderID;
                    //v_mesHistory.PartID = Convert.ToInt32(v_mesUnit.PartID);
                    //v_mesHistory.LooperCount = 1;
                    //int I_mesHistoryReult = mesHistoryDll.Insert(v_mesHistory);

                    //if (S_mesUnitDetailResult.Substring(0, 1) != "E" && I_mesHistoryReult > 0)
                    //{
                        DataRow dr = dt.NewRow();
                        dr["SN"] = S_SN;
                        dt.Rows.Add(dr);
                    //}
                }
                dsSN.Tables.Add(dt);
            }
            catch (Exception ex)
            {
                return result = ex.Message.ToString();
            }
            return result;
        }


        public string Get_CreateMesSN(string strSNFormat, LoginList loginList, string xmlProdOrder, string xmlPart, string xmlStation,
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
            mesUnitDetailDAL mesUnitDetailDll = new mesUnitDetailDAL();

            string S_FormatName = string.Empty;

            S_FormatName = mesGetSNFormatIDByList(v_mesUnit.PartID.ToString(), v_mesUnit.PartFamilyID.ToString(),
                v_mesUnit.LineID.ToString(), v_mesUnit.ProductionOrderID.ToString(), loginList.StationTypeID.ToString());
            if (string.IsNullOrEmpty(S_FormatName))
            {
                return result = "20132";
            }

            int routeID = GetRouteID(loginList.LineID.ToString(), v_mesUnit.PartID.ToString(),
                                        v_mesUnit.PartFamilyID.ToString(), v_mesUnit.ProductionOrderID.ToString());
            if (routeID == -1)
            {
                return result = "20195";
            }
            DataSet dtSetState = GetmesUnitState(v_mesUnit.PartID.ToString(), v_mesUnit.PartFamilyID.ToString(), "",
                loginList.LineID.ToString(), loginList.StationTypeID, v_mesUnit.ProductionOrderID.ToString(), v_mesUnit.StatusID.ToString());
            if (dtSetState == null || dtSetState.Tables.Count == 0)
            {
                return result = "20131";
            }
            DataTable dtState = dtSetState.Tables[0];
            v_mesUnit.UnitStateID = Convert.ToInt32(dtState.Rows[0][0].ToString());

            string SN_Pattern = string.Empty;
            //查询SN正则表达式规则
            DataSet dataSet = GetmesPartDetail(Convert.ToInt32(v_mesUnit.PartID), "SN_Pattern");
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                SN_Pattern = dataSet.Tables[0].Rows[0]["Content"].ToString();
            }

            try
            {

                DataTable dt = new DataTable();
                dt.Columns.Add("SN", typeof(string));               

                for (; number > 0; number--)
                {

                    ////生成条码
                    //if (strSNFormat == "" || strSNFormat == null)
                    //{
                    //    strSNFormat = S_FormatName;
                    //}
                    DataTable DT = uspSNRGetNext(S_FormatName, 0, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, null).Tables[1];
                    string S_SN = DT.Rows[0][0].ToString();

                    if (!string.IsNullOrEmpty(SN_Pattern))
                    {
                        if (!Regex.IsMatch(S_SN, SN_Pattern))
                        {
                            return "20027";
                        }
                    }

                    if (string.IsNullOrEmpty(S_SN))
                    {
                        return result = "20087";
                    }

                    string S_Sql = "select 1 from mesSerialNumber where Value='"+ S_SN + "'";
                    DataTable DT_SN = SqlServerHelper.Data_Table(S_Sql);
                    if (DT_SN.Rows.Count > 0)
                    {
                        continue;   
                    }


                    //写入UNIT表
                        string unitId = string.Empty;
                    v_mesUnit.CreationTime = DateTime.Now;
                    v_mesUnit.LastUpdate = DateTime.Now;
                    unitId = mesUnitDll.Insert(v_mesUnit);
                    if (string.IsNullOrEmpty(unitId) || unitId.Substring(0, 1) == "E")
                    {
                        return result = "20130";
                    }

                    ////生成条码
                    //if (strSNFormat == "" || strSNFormat == null)
                    //{
                    //    strSNFormat = S_FormatName;
                    //}

                    //DataTable DT = uspSNRGetNext(strSNFormat, 0, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, null).Tables[1];
                    //string S_SN = DT.Rows[0][0].ToString();
                    //if (string.IsNullOrEmpty(S_SN))
                    //{
                    //    return result = "20087";
                    //}

                    //写入mesSerialNumber
                    mesSerialNumber v_mesSerialNumber = new mesSerialNumber();
                    v_mesSerialNumber.UnitID = Convert.ToInt32(unitId);
                    v_mesSerialNumber.SerialNumberTypeID = v_mesUnit.SerialNumberType;
                    v_mesSerialNumber.Value = S_SN;
                    mesSerialNumberDll.Insert(v_mesSerialNumber);

                    //写入UnitDetail表
                    mesUnitDetail msDetail = new mesUnitDetail();
                    msDetail.UnitID = Convert.ToInt32(unitId);
                    msDetail.reserved_01 = "";
                    msDetail.reserved_02 = "";
                    msDetail.reserved_03 = "";
                    msDetail.reserved_04 = "";
                    msDetail.reserved_05 = "";
                    string S_mesUnitDetailResult= mesUnitDetailDll.Insert(msDetail);

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
                    int I_mesHistoryReult= mesHistoryDll.Insert(v_mesHistory);

                    if (S_mesUnitDetailResult.Substring(0, 1) != "E" && I_mesHistoryReult>0)
                    {
                        DataRow dr = dt.NewRow();
                        dr["SN"] = S_SN;
                        dt.Rows.Add(dr);
                    }                    
                }                
                dsSN.Tables.Add(dt);
            }
            catch (Exception ex)
            {
                return result = ex.Message.ToString();
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
                S_Result = "ERROR20053";
            }
            else
            {
                string S_MachineStatusID = DT1.Rows[0]["StatusID"].ToString();
                if (S_MachineStatusID == "2")
                {
                    S_Sql = @"select B.partID,UnitID,reserved_02 HB_Batch from mesUnitDetail A inner join mesUnit B ON A.UnitID=B.ID
	                            where A.ID=(select max(ID)  from mesUnitDetail where reserved_01='" + S_BoxSN + @"' and reserved_03=1)";
                    DataTable DT2 = SqlServerHelper.Data_Table(S_Sql);

                    if (DT2.Rows.Count > 0)
                    {
                        S_Result = DT2.Rows[0]["HB_Batch"].ToString();
                        DS_Result.Tables.Add(DT2);
                    }
                    else
                    {
                        S_Result = "ERROR20054";
                    }
                }
                else
                {
                    S_Result = "ERROR20055";
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

        public DataSet GetmesUnitComponent2(string UnitID)
        {
            string S_Sql = "select * from mesUnitComponent where UnitID='" + UnitID + "' and StatusID=1";
            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }

        public DataSet GetmesProductStructure(string ParentPartID, string PartID, string StationTypeID)
        {
            string S_Sql = "select * from mesProductStructure where ParentPartID='" + ParentPartID + "' and PartID='" + PartID + "'" +
             " and StationTypeID = '" + StationTypeID + "' and Status=1";
            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }

        public DataSet GetmesProductStructure1(string ParentPartID)
        {
            string S_Sql = "select * from mesProductStructure where ParentPartID='" + ParentPartID + "' and Status=1";
            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }


        public DataSet GetmesProductStructure2(string ParentPartID, string StationTypeID)
        {
            string S_Sql = "select * from mesProductStructure where ParentPartID='" + ParentPartID +
                "' and StationTypeID='" + StationTypeID + "' and Status=1";
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
                S_Sql = "Update mesMachine set RuningCapacityQuantity=0 ,StatusID=" + StatusID + " where SN='" + S_SN + "'";
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
            string S_Sql = String.Format(@"SELECT A.*,B.PartFamilyID FROM mesUnit A 
                            INNER JOIN mesPart B ON A.PartID = B.ID
                            WHERE A.ID ={0} ", UnitID);
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
            string S_Sql = @"SELECT ID,StatusID FROM mesPackage WHERE SerialNumber='" + PalletSN + "'";
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

        public DataSet GetLabels(string PartID, string PartFamilyID, string LineID, string ProductionOrderID, string StationTypeID)
        {
            string LabelID = string.Empty;
            PartID = string.IsNullOrEmpty(PartID) ? "null" : PartID;
            PartFamilyID = string.IsNullOrEmpty(PartFamilyID) ? "null" : PartFamilyID;
            LineID = string.IsNullOrEmpty(LineID) ? "null" : LineID;
            ProductionOrderID = string.IsNullOrEmpty(ProductionOrderID) ? "null" : ProductionOrderID;
            StationTypeID = string.IsNullOrEmpty(StationTypeID) ? "null" : StationTypeID;
            string strSql = string.Format(@"select * from dbo.ufnRTEGetLabelID({0},{1},{2},{3},{4})", LineID, PartID, PartFamilyID, ProductionOrderID, StationTypeID);
            DataSet dtsLables = SqlServerHelper.Data_Set(strSql);
            return dtsLables;
        }

        public DataSet GetLabelName(string StationTypeID, string PartFamilyID, string PartID, string ProductionOrderID, string LineID)
        {
            string LabelID = string.Empty;
            DataSet Lables = GetLabels(PartID, PartFamilyID, LineID, ProductionOrderID, StationTypeID);
            if (Lables == null || Lables.Tables.Count == 0 || Lables.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                foreach (DataRow dr in Lables.Tables[0].Rows)
                {
                    LabelID = (string.IsNullOrEmpty(LabelID) ? "" : LabelID + ",") + dr["LabelMapID"].ToString();
                }
                string S_Sql = string.Format(@"select A.ID, A.StationTypeID,B.Description StationType,
		                            A.LabelID,C.Name LabelName,C.SourcePath LabelPath,
		                            A.PartFamilyID,E.Name PartFamily,
		                            A.PartID,D.PartNumber,		
		                            A.ProductionOrderID,F.ProductionOrderNumber,
		                            A.LineID,G.Description Line,
                                    C.OutputType,C.TargetPath,C.PageCapacity
                            from mesStationTypeLabelMap A
		                                left join mesStationType B on A.StationTypeID=B.ID
		                                left join mesLabel C on A.LabelID=C.ID
		                                left join luPartFamily E on A.PartFamilyID=E.ID
		                                left join mesPart D on A.PartID=D.ID           
                                        left join mesProductionOrder F on A.ProductionOrderID=F.ID
                                        left join mesLine G on A.LineID=G.ID  
                            where 1=1 and A.ID in ({0})", LabelID);

                return SqlServerHelper.Data_Set(S_Sql);
            }
        }


        public DataSet GetLabelCMD(string StationTypeID, string PartFamilyID, string PartID, string ProductionOrderID, string LineID)
        {
            DataSet DS = GetLabelName(StationTypeID, PartFamilyID, PartID, ProductionOrderID, LineID);
            if (DS == null || DS.Tables.Count == 0 || DS.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            string S_LabelID = DS.Tables[0].Rows[0]["LabelID"].ToString();
            string S_Sql = "select * from mesLabelCMD where LabelID=" + S_LabelID;
            return SqlServerHelper.Data_Set(S_Sql);
        }

        /// <summary>
        /// 获取标签ID
        /// </summary>
        /// <param name="StationTypeID"></param>
        /// <param name="PartFamilyID"></param>
        /// <param name="PartID"></param>
        /// <param name="ProductionOrderID"></param>
        /// <param name="LineID"></param>
        /// <returns></returns>
        public string GetLabelID(string StationTypeID, string PartFamilyID, string PartID, string ProductionOrderID, string LineID)
        {
            DataSet DS = GetLabelName(StationTypeID, PartFamilyID, PartID, ProductionOrderID, LineID);
            if (DS == null || DS.Tables.Count == 0 || DS.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            string S_LabelID = DS.Tables[0].Rows[0]["LabelID"].ToString();
            return S_LabelID;
        }

        public DataSet LabelNameToLabelCMD(string S_LabelName)
        {
            string S_Sql = "SELECT *   FROM mesLabel WHERE NAME='" + S_LabelName + "'";
            DataTable DT = SqlServerHelper.Data_Table(S_Sql);
            string S_LabelID =DT.Rows[0]["ID"].ToString();

            S_Sql = "select * from mesLabelCMD where LabelID=" + S_LabelID;

            return SqlServerHelper.Data_Set(S_Sql);
        }


        public string BuckToFGSN(string S_BuckSN)
        {
            string S_Sql = String.Format(@"SELECT TOP 1 Value AS FG_SN FROM mesSerialNumber A
                                            INNER JOIN mesUnitDetail B ON A.UnitID = B.UnitID
                                            WHERE B.reserved_03 = '1' AND B.reserved_01 = '{0}'
                                            ORDER BY B.ID DESC", S_BuckSN);
            DataTable DT = SqlServerHelper.Data_Table(S_Sql);
            if (DT == null || DT.Rows.Count == 0)
                return "";
            else
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

        public string TimeCheck(string StationID, string S_SN)
        {
            string S_Result = "1";

            //try
            //{
            //    string S_Sql = "select * from mesStationTypeDetail where StationTypeID=" + StationTypeID;
            //    DataTable DT = SqlServerHelper.Data_Table(S_Sql);

            //    if (DT.Rows.Count >= 3)
            //    {
            //        string S_IsTimeCheck = (from c in DT.AsEnumerable()
            //                                where c.Field<int>("StationTypeDetailDefID") == 6
            //                                select c.Field<String>("Content")
            //                                ).FirstOrDefault().ToString();

            //        string S_TimeCheckStartStationType = (from c in DT.AsEnumerable()
            //                                              where c.Field<int>("StationTypeDetailDefID") == 7
            //                                              select c.Field<String>("Content")
            //                                ).FirstOrDefault().ToString();

            //        string S_TimeCheckDuration = (from c in DT.AsEnumerable()
            //                                      where c.Field<int>("StationTypeDetailDefID") == 8
            //                                      select c.Field<String>("Content")
            //                                ).FirstOrDefault().ToString();

            //        if (S_IsTimeCheck == "1")
            //        {
            //            S_Sql = @"select A.*,C.EnterTime,getdate() NowTime,DATEDIFF(minute,C.EnterTime,getdate()) TimeDuration,    
            //                    C.StationID,D.Description Station, D.StationTypeID, E.Description StationType
            //                from mesSerialNumber A
            //                    join mesUnit B on A.UnitID = B.ID
            //                    join mesHistory C on C.UnitID = A.UnitID
            //                    join mesStation D on D.ID = C.StationID
            //                    join mesStationType E on E.ID = D.StationTypeID
            //                where A.Value = '" + S_SN + "' and D.StationTypeID=" + S_TimeCheckStartStationType;

            //            DT = SqlServerHelper.Data_Table(S_Sql);
            //            int I_TimeDuration = 0;
            //            if (DT.Rows.Count > 0)
            //            {
            //                I_TimeDuration = Convert.ToInt32(DT.Rows[0]["TimeDuration"].ToString());
            //            }

            //            if (I_TimeDuration <= Convert.ToInt32(S_TimeCheckDuration))
            //            {
            //                S_Result = "1";
            //            }
            //            else
            //            {
            //                S_Result ="20058";
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    S_Result = ex.ToString();
            //}
            string S_Sql = "exec [uspTimeCheck] '" + StationID + "','" + S_SN + "'";
            DataTable DT = SqlServerHelper.Data_Table(S_Sql);
            S_Result = DT.Rows[0][0].ToString();
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
                    result = "20124";
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
                return ex.Message.ToString();
            }
        }

        /// <summary>
        /// 中箱包装
        /// </summary>
        /// <param name="PartID"></param>
        /// <param name="S_UPCSN"></param>
        /// <param name="S_CartonSN"> 箱号SN</param>
        /// <param name="LoginList"></param>
        /// <param name="BoxQty">大于0 整箱包装完成数量
        ///                      0  扫描UPC包装
        ///                      -1 扫描FG包装
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
                    return "20119";
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
                        return "20120";
                    }

                    strSql = @"UPDATE mesPackage SET CurrentCount=@packCount,StationID=@stationId,EmployeeID=@employeeId,
				                StatusID=1,LastUpdate=GETDATE() where ID=@PackageId";
                    execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@PackageId", boxID),
                                                                         new SqlParameter("@stationId", loginList.StationID),
                                                                         new SqlParameter("@employeeId", loginList.EmployeeID),
                                                                         new SqlParameter("@packCount", BoxQty));
                    if (execNumber < 0)
                    {
                        return "20121";
                    }
                    //尾箱数量增加记录
                    strSql = @"DECLARE @PackageDetailDefDesc VARCHAR(200) = 'LastBoxCount',@PackageID int = @tmpPackageID, @LastBoxCount VARCHAR(50) = @tmpLastBoxCount
                        IF NOT EXISTS(SELECT 1 FROM dbo.luPackageDetailDef WHERE Description = @PackageDetailDefDesc)
                        BEGIN
	                        INSERT INTO dbo.luPackageDetailDef(ID, Description)
	                        VALUES( (SELECT MAX(ID)+1 FROM dbo.luPackageDetailDef), -- ID - int
	                        @PackageDetailDefDesc -- Description - nvarchar(200)
	                            )
                        END

                        INSERT INTO
                        dbo.mesPackageDetail(PackageID, PackageDetailDefID, Content)
                        SELECT @PackageID, ID,@LastBoxCount
                        from dbo.luPackageDetailDef
                        WHERE Description = @PackageDetailDefDesc ";

                    execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@tmpPackageID", boxID), new SqlParameter("@tmpLastBoxCount", BoxQty.ToString()));
                }
                else
                {
                    string FGSN = string.Empty;
                    //关联包装信息
                    if (BoxQty == 0)
                    {
                        strSql = @"UPDATE A SET A.InmostPackageID = @PackageId FROM mesUnitDetail A WHERE A.KitSerialNumber = @S_UPCSN";
                        mesUnitDetailDAL mesUnitDetail = new mesUnitDetailDAL();
                        FGSN = mesUnitDetail.MesGetFGSNByUPCSN(S_UPCSN);
                    }
                    else if (BoxQty == -1)
                    {
                        strSql = @"UPDATE A SET A.InmostPackageID = @PackageId FROM mesUnitDetail A 
                                        INNER JOIN mesSerialNumber B ON A.UnitID=B.UnitID WHERE B.Value = @S_UPCSN";
                        FGSN = S_UPCSN;
                    }
                    execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@S_UPCSN", S_UPCSN),
                                                                         new SqlParameter("@PackageId", boxID));
                    if (execNumber < 0)
                    {
                        return "20129";
                    }

                    string strResult = SetmesHistory(FGSN, loginList, ProductionOrderID);
                    if (strResult != "1")
                    {
                        return strResult;
                    }
                    //扫描UPC包装需要FG与UPC都过站
                    if (!string.IsNullOrEmpty(S_UPCSN) && S_UPCSN != FGSN)
                    {
                        strResult = SetmesHistory(S_UPCSN, loginList, ProductionOrderID);
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
                return ex.Message.ToString();
            }
        }

        public string uspKitBoxPackagingNew(string PartID, string ProductionOrderID, string S_UPCSN, string S_CartonSN, LoginList loginList, int Allnumber,int CurrentQty, int BoxQty = 0)
        {
            try
            {
                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, new TimeSpan(0, 0, 30)))
                {
                    //int execNumber = 0;

                    string strSql = @"SELECT ID FROM mesPackage WHERE SerialNumber=@S_CartonSN AND StatusID=0 AND Stage=1";

                    DataTable dtMp = SqlServerHelper.ExecuteDataTable(strSql, new SqlParameter("@S_CartonSN", S_CartonSN));
                    if (dtMp == null || dtMp.Rows.Count == 0)
                    {
                        return "20119";
                    }
                    string boxID = dtMp.Rows[0]["ID"].ToString();

                    string sqlValue = "";

                    if (BoxQty <= 0)
                    {

                        string FGSN = string.Empty;
                        //关联包装信息
                        if (BoxQty == 0)
                        {
                            sqlValue = @"UPDATE A SET A.InmostPackageID = " + boxID + " FROM mesUnitDetail A WHERE A.KitSerialNumber = '" + S_UPCSN + "' ";
                            mesUnitDetailDAL mesUnitDetail = new mesUnitDetailDAL();
                            FGSN = mesUnitDetail.MesGetFGSNByUPCSN(S_UPCSN);
                        }
                        else if (BoxQty == -1)
                        {
                            sqlValue = @"UPDATE A SET A.InmostPackageID = " + boxID + " FROM mesUnitDetail A INNER JOIN mesSerialNumber B ON A.UnitID=B.UnitID WHERE B.Value = '" + S_UPCSN + "' ";
                            FGSN = S_UPCSN;
                        }
                        if (CurrentQty == Allnumber)
                        {
                            sqlValue = sqlValue + @"INSERT INTO mesPackageHistory(PackageID, PackageStatusID, StationID, EmployeeID, Time)
                                VALUES(" + boxID + ", 1, " + loginList.StationID + ", " + loginList.EmployeeID + ", GETDATE()) ";


                            sqlValue = sqlValue + @"UPDATE mesPackage SET CurrentCount=" + CurrentQty + ",StationID=" + loginList.StationID +
                                ",EmployeeID=" + loginList.EmployeeID + ",StatusID=1,LastUpdate=GETDATE() where ID=" + boxID + " ";
                        }
                        string ReturnValue = SqlServerHelper.ExecSql(sqlValue);
                        if (ReturnValue != "OK")
                        {
                            return "20129";
                        }

                        string strResult = SetmesHistory(FGSN, loginList, ProductionOrderID);
                        if (strResult != "1")
                        {
                            return strResult;
                        }
                        //扫描UPC包装需要FG与UPC都过站
                        if (!string.IsNullOrEmpty(S_UPCSN) && S_UPCSN != FGSN)
                        {
                            strResult = SetmesHistory(S_UPCSN, loginList, ProductionOrderID);
                            if (strResult != "1")
                            {
                                return strResult;
                            }
                        }

                    }
                    ts.Complete();
                    return "1";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }


        /// <summary>
        /// 包装过站记录
        /// </summary>
        /// <param name="S_SN"></param>
        /// <param name="loginList"></param>
        /// <returns></returns>
        public string SetmesHistory(string S_SN, LoginList loginList,string ProductionOrderID)
        {
            try
            {
                string strSql = string.Empty;
                int execNumber = 0;

                strSql = @"SELECT B.ID,B.PartID,B.ProductionOrderID FROM mesSerialNumber A 
                            INNER JOIN mesUnit B ON A.UnitID=B.ID WHERE A.Value=@S_SN";
                DataTable dtUnit = SqlServerHelper.ExecuteDataTable(strSql, new SqlParameter("@S_SN", S_SN));
                if (dtUnit == null || dtUnit.Rows.Count == 0)
                {
                    return "20127";
                }
                int UnitID = Convert.ToInt32(dtUnit.Rows[0]["ID"].ToString());
                int PartID = Convert.ToInt32(dtUnit.Rows[0]["PartID"].ToString());

                mesHistoryDAL mesHistoryDll = new mesHistoryDAL();
                mesHistory v_mesHistory = new mesHistory();
                mesPartDAL mesPartDAL = new mesPartDAL();
                mesPart mesPartUnit = mesPartDAL.Get(Convert.ToInt32(PartID));
                DataSet dsUinitState = GetmesUnitState(PartID.ToString(), mesPartUnit.PartFamilyID.ToString(), "",
                    loginList.LineID.ToString(), loginList.StationTypeID, ProductionOrderID.ToString(), "1");
                int UnitStateID = Convert.ToInt32(dsUinitState.Tables[0].Rows[0]["ID"].ToString());

                //修改Unit属性
                strSql = @"UPDATE mesUnit SET UnitStateID=@UnitStateID,ProductionOrderID=@ProductionOrderID,StatusID=1,EmployeeID=@EmployeeID,LineID=@LineID,
                           StationID=@StationID,LastUpdate=GETDATE() WHERE ID=@UnitID";
                execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@EmployeeID", loginList.EmployeeID),
                                                                     new SqlParameter("@UnitID", UnitID),
                                                                     new SqlParameter("@UnitStateID", UnitStateID),
                                                                     new SqlParameter("@StationID", loginList.StationID),
                                                                     new SqlParameter("@LineID", loginList.LineID),
                                                                     new SqlParameter("@ProductionOrderID", ProductionOrderID)
                                                                     );
                if (execNumber < 0)
                {
                    return "20126";
                }

                //生成过站记录
                v_mesHistory.UnitID = UnitID;
                v_mesHistory.UnitStateID = UnitStateID;
                v_mesHistory.EmployeeID = loginList.EmployeeID;
                v_mesHistory.StationID = loginList.StationID;
                v_mesHistory.EnterTime = DateTime.Now;
                v_mesHistory.ExitTime = DateTime.Now;
                v_mesHistory.ProductionOrderID = Convert.ToInt32(ProductionOrderID);
                v_mesHistory.PartID = Convert.ToInt32(PartID);
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
        /// 包装过站记录
        /// </summary>
        /// <param name="S_SN"></param>
        /// <param name="loginList"></param>
        /// <returns></returns>
        public string SetmesPackBoxHistory(string S_SN, string boxWeight,LoginList loginList)
        {
            string result = "0";
            try
            {
                string strSql = string.Empty;

                strSql = @"INSERT INTO dbo.mesPackageDetail
                    (
                        PackageID,
                        PackageDetailDefID,
                        Content
                    )
                    SELECT a.ID AS PackageID,b.ID AS PackageDetailDefID,@boxWeight AS Content
					FROM dbo.mesPackage a,dbo.luPackageDetailDef b
					WHERE a.SerialNumber = @BoxSN AND 
					b.Description = 'PackBoxWeight'";
                result = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter[2] {
                        new SqlParameter("boxWeight",boxWeight),
                        new SqlParameter("BoxSN",S_SN)}).ToString();
                if (result != "1")
                {
                    return "0";
                }
                strSql = @"INSERT INTO	
                        dbo.mesPackageHistory
                        (
                            PackageID,
                            PackageStatusID,
                            StationID,
                            EmployeeID,
                            Time
                        )
                        SELECT a.ID,b.ID,@StationID AS StationID, @EmployeeID AS EmployeeID, GETDATE() AS Time
                        FROM dbo.mesPackage a,
	                        luPackageStatus b 
	                        WHERE a.SerialNumber = @BoxSN AND
	                        b.Description = 'ScalageWeight'";
                return SqlServerHelper.ExecuteNonQuery(strSql,
                        new SqlParameter[3]
                        {
                            new SqlParameter("BoxSN",S_SN),
                            new SqlParameter("StationID",loginList.StationID),
                            new SqlParameter("EmployeeID",loginList.EmployeeID)
                        }).ToString();
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
                    result = "20124";
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
                return ex.Message.ToString();
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
                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, new TimeSpan(0, 0, 30)))
                {

                    int execNumber = 0;
                    string strSql = @"SELECT ID FROM mesPackage WHERE SerialNumber=@S_PalletSN AND StatusID=0";

                    DataTable dtMp = SqlServerHelper.ExecuteDataTable(strSql, new SqlParameter("@S_PalletSN", S_PalletSN));
                    if (dtMp == null || dtMp.Rows.Count == 0)
                    {
                        return "20119";
                    }
                    string PalletID = dtMp.Rows[0]["ID"].ToString();

                    if (PalletQty > 0)
                    {
                        strSql = @"INSERT INTO mesPackageHistory(PackageID, PackageStatusID, StationID, EmployeeID, Time)
                                VALUES(@PackageId, 2, @stationId, @employeeId, GETDATE())";
                        execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@PackageId", PalletID),
                                                                             new SqlParameter("@stationId", loginList.StationID),
                                                                             new SqlParameter("@employeeId", loginList.EmployeeID));
                        if (execNumber < 0)
                        {
                            return "20120";
                        }

                        strSql = @"UPDATE mesPackage SET CurrentCount=@packCount,StationID=@stationId,EmployeeID=@employeeId,
				                StatusID=1,LastUpdate=GETDATE() where ID=@PackageId";
                        execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@PackageId", PalletID),
                                                                             new SqlParameter("@stationId", loginList.StationID),
                                                                             new SqlParameter("@employeeId", loginList.EmployeeID),
                                                                             new SqlParameter("@packCount", PalletQty));
                        if (execNumber < 0)
                        {
                            return "20121";
                        }
                    }
                    else
                    {
                        strSql = "SELECT Stage FROM mesPackage WHERE ID=@ParentID";
                        DataTable dtStage = SqlServerHelper.ExecuteDataTable(strSql, new SqlParameter("@ParentID", PalletID));

                        if (dtStage.Rows[0]["Stage"].ToString() != "3")
                        {
                            //关联包装信息
                            strSql = @"UPDATE A SET A.ParentID=@ParentID  FROM mesPackage A WHERE A.SerialNumber=@SerialNumber";
                            execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@SerialNumber", S_CartonSN),
                                                                                 new SqlParameter("@ParentID", PalletID));
                            if (execNumber < 0)
                            {
                                return "20122";
                            }
                        }
                        else
                        {
                            //关联包装信息
                            strSql = @"UPDATE A SET A.ShipmentParentID=@ParentID  FROM mesPackage A WHERE A.SerialNumber=@SerialNumber";
                            execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@SerialNumber", S_CartonSN),
                                                                                 new SqlParameter("@ParentID", PalletID));
                            if (execNumber < 0)
                            {
                                return "20122";
                            }

                            //记录Shipment履历
                            strSql = @"SELECT top 1 ID FROM mesPackage WHERE SerialNumber=@S_CartonSN";
                            DataTable dtBoxMp = SqlServerHelper.ExecuteDataTable(strSql, new SqlParameter("@S_CartonSN", S_CartonSN));
                            string BoxID = dtBoxMp.Rows[0]["ID"].ToString();

                            strSql = @"INSERT INTO mesPackageHistory(PackageID, PackageStatusID, StationID, EmployeeID, Time)
                                VALUES(@PackageId, 8, @stationId, @employeeId, GETDATE())";
                            execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@PackageId", BoxID),
                                                                                 new SqlParameter("@stationId", loginList.StationID),
                                                                                 new SqlParameter("@employeeId", loginList.EmployeeID));
                            if (execNumber < 0)
                            {
                                return "20120";
                            }
                        }

                        //记录包装数据所有UPC/FG履历信息
                        strSql = @"SELECT C.Value SerialNumber,B.KitSerialNumber from mesPackage A
                                INNER JOIN mesUnitDetail B ON A.ID=B.InmostPackageID
                                INNER JOIN mesSerialNumber C ON B.UnitID=C.UnitID
                                WHERE A.SerialNumber=@S_CartonSN";
                        DataTable dtUPCUnit = SqlServerHelper.ExecuteDataTable(strSql, new SqlParameter("@S_CartonSN", S_CartonSN));
                        if (dtUPCUnit == null || dtUPCUnit.Rows.Count == 0)
                        {
                            return "20123";
                        }
                        string strResult = string.Empty;
                        foreach (DataRow dr in dtUPCUnit.Rows)
                        {
                            string FGSN = dr["SerialNumber"].ToString();
                            string UPCSN = dr["KitSerialNumber"].ToString();
                            strResult = SetmesHistory(FGSN, loginList, ProductionOrderID);
                            if (strResult != "1")
                            {
                                return strResult;
                            }
                            if (!string.IsNullOrEmpty(UPCSN) && UPCSN != FGSN)
                            {
                                strResult = SetmesHistory(UPCSN, loginList, ProductionOrderID);
                                if (strResult != "1")
                                {
                                    return strResult;
                                }
                            }
                        }
                    }
                    ts.Complete();
                    return "1";
                }
                
            }
            catch (Exception ex)
            {
                
                return ex.Message.ToString();
            }
        }

        /// <summary>
        /// 栈板包装
        /// </summary>
        /// <param name="PartID"></param>
        /// <param name="ProductionOrderID"></param>
        /// <param name="S_CartonSN"></param>
        /// <param name="S_PalletSN"></param>
        /// <param name="loginList"></param>
        /// <param name="PalletQty">栈板需要绑定的箱数</param>
        /// <param name="boxCount">实际栈板绑定的箱数</param>
        /// <returns></returns>
        public string uspPalletPackagingV2(string PartID, string ProductionOrderID, string S_CartonSN, string S_PalletSN, LoginList loginList, int PalletQty,ref int boxCount)
        {
            try
            {
                int execNumber = 0;
                boxCount = -1;
                string strSql = @"SELECT ID FROM mesPackage WHERE SerialNumber=@S_PalletSN AND StatusID=0";

                DataTable dtMp = SqlServerHelper.ExecuteDataTable(strSql, new SqlParameter("@S_PalletSN", S_PalletSN));
                if (dtMp == null || dtMp.Rows.Count == 0)
                {
                    return "20119";
                }
                string PalletID = dtMp.Rows[0]["ID"].ToString();

                strSql = $"SELECT ID FROM dbo.mesPackage WHERE SerialNumber = '{S_CartonSN}'";

                object CartonID  = SqlServerHelper.ExecuteScalar(strSql, new SqlParameter[] { new SqlParameter("@S_CartonSN", S_CartonSN) });
                if (CartonID == null)
                {
                    return "20204";
                }

                mesPartDAL mesPartDAL = new mesPartDAL();
                mesPart mesPartUnit = mesPartDAL.Get(Convert.ToInt32(PartID));
                DataSet dsUinitState = GetmesUnitState(PartID.ToString(), mesPartUnit.PartFamilyID.ToString(), "", loginList.LineID.ToString(), loginList.StationTypeID, ProductionOrderID.ToString(), "1");
                int UnitStateID = Convert.ToInt32(dsUinitState.Tables[0].Rows[0]["ID"].ToString());

                using (SqlConnection sqlConnection = SqlServerHelper.GetConnection())
                {
                    sqlConnection.Open();
                    SqlTransaction transaction = sqlConnection.BeginTransaction(IsolationLevel.ReadUncommitted);
                    string sql = string.Empty;
                    SqlCommand cmd = new SqlCommand(sql, sqlConnection, transaction);
                    try
                    {
                        #region 绑定箱码和栈板条码
                        sql = @"UPDATE A SET A.ParentID=@ParentID  FROM mesPackage A WHERE A.SerialNumber=@SerialNumber";
                        cmd.CommandText = sql;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddRange(new SqlParameter[2]{ new SqlParameter("@SerialNumber", S_CartonSN),
                                                                      new SqlParameter("@ParentID", PalletID)});
                        execNumber = cmd.ExecuteNonQuery();
                        if (execNumber <= 0)
                        {
                            throw new Exception( "20122");
                        }
                        #endregion
                        #region 更新箱码绑定的FG/UPC状态，插入历史记录
                        sql = @"UPDATE d SET d.UnitStateID=@UnitStateID,StatusID=1,d.StationID=@StationID ,d.LineID=@LineID, d.EmployeeID=@EmployeeID,d.LastUpdate = GETDATE()
                            FROM mesPackage A
                            INNER JOIN mesUnitDetail B ON A.ID=B.InmostPackageID
                            INNER JOIN mesSerialNumber C ON B.UnitID=C.UnitID
                            INNER JOIN dbo.mesUnit d ON d.ID = B.UnitID
                            WHERE A.SerialNumber=@S_CartonSN";
                        cmd.CommandText = sql;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddRange(new SqlParameter[5]{ new SqlParameter("@S_CartonSN", S_CartonSN),
                                                                    new SqlParameter("@UnitStateID", UnitStateID),
                                                                    new SqlParameter("@StationID", loginList.StationID),
                                                                    new SqlParameter("@LineID", loginList.LineID),
                                                                    new SqlParameter("@EmployeeID", loginList.EmployeeID)});
                        cmd.CommandText = sql;
                        execNumber = cmd.ExecuteNonQuery();
                        if (execNumber <= 0)
                        {
                            throw new Exception("20126");
                        }

                        sql = @"UPDATE d SET d.UnitStateID=@UnitStateID,StatusID=1,d.StationID=@StationID ,d.LineID=@LineID, d.EmployeeID=@EmployeeID,d.LastUpdate = GETDATE()
                            FROM mesPackage A
                            INNER JOIN mesUnitDetail B ON A.ID = B.InmostPackageID
                            INNER JOIN mesSerialNumber C ON c.Value = b.KitSerialNumber
                            INNER JOIN dbo.mesUnit d ON d.ID = c.UnitID
                            WHERE A.SerialNumber = @S_CartonSN";
                        cmd.CommandText = sql;
                        execNumber = cmd.ExecuteNonQuery();
                        if (execNumber < 0)
                        {
                            throw new Exception("20126");
                        }

                        sql = @"INSERT INTO dbo.mesHistory(UnitID, UnitStateID, EmployeeID, StationID, EnterTime, ExitTime, ProductionOrderID, PartID, LooperCount, StatusID)
                            SELECT b.UnitID,@UnitStateID,@EmployeeID,@StationID,GETDATE(),GETDATE(),@ProductionOrderID,@PartID,1,1
                            FROM mesPackage A
                            INNER JOIN mesUnitDetail B ON A.ID=B.InmostPackageID
                            INNER JOIN mesSerialNumber C ON B.UnitID=C.UnitID
                            INNER JOIN dbo.mesUnit d ON d.ID = B.UnitID
                            WHERE A.SerialNumber=@S_CartonSN";
                        cmd.CommandText = sql;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddRange(new SqlParameter[7]{ new SqlParameter("@S_CartonSN", S_CartonSN),
                                                                    new SqlParameter("@UnitStateID", UnitStateID),
                                                                    new SqlParameter("@StationID", loginList.StationID),
                                                                    new SqlParameter("@LineID", loginList.LineID),
                                                                    new SqlParameter("@EmployeeID", loginList.EmployeeID),
                                                                    new SqlParameter("@ProductionOrderID",ProductionOrderID),
                                                                    new SqlParameter("@PartID",PartID)});
                        execNumber = cmd.ExecuteNonQuery();
                        if (execNumber <= 0)
                        {
                            throw new Exception("20126");
                        }

                        sql = @"INSERT INTO dbo.mesHistory(UnitID, UnitStateID, EmployeeID, StationID, EnterTime, ExitTime, ProductionOrderID, PartID, LooperCount, StatusID)
                            SELECT C.UnitID,@UnitStateID,@EmployeeID,@StationID,GETDATE(),GETDATE(),@ProductionOrderID,@PartID,1,1
                            FROM mesPackage A
                            INNER JOIN mesUnitDetail B ON A.ID=B.InmostPackageID
                            INNER JOIN mesSerialNumber C ON c.Value = b.KitSerialNumber
                            INNER JOIN dbo.mesUnit d ON d.ID = c.UnitID
                            WHERE A.SerialNumber=@S_CartonSN";
                        cmd.CommandText = sql;
                        execNumber = cmd.ExecuteNonQuery();
                        if (execNumber < 0)
                        {
                            throw new Exception("20126");
                        }
                        #endregion
                        #region 查询已绑定数量
                        sql = @"SELECT COUNT(1) FROM dbo.mesPackage WHERE ParentID = @ParentID";
                        cmd.CommandText = sql;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddRange(new SqlParameter[1]{  new SqlParameter("@ParentID", PalletID)});
                        object o = cmd.ExecuteScalar();
                        if (o == null)
                        {
                            throw new Exception("20122");
                        }
                        boxCount = int.Parse(o.ToString());
                        #endregion

                        #region 箱码插入历史记录
                        sql = @"INSERT INTO mesPackageHistory(PackageID, PackageStatusID, StationID, EmployeeID, Time)
                                VALUES(@PackageId, 1, @stationId, @employeeId, GETDATE())";
                        cmd.CommandText = sql;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddRange(new SqlParameter[3]{new SqlParameter("@PackageId", CartonID.ToString()),
                                                                             new SqlParameter("@stationId", loginList.StationID),
                                                                             new SqlParameter("@employeeId", loginList.EmployeeID)});
                        execNumber = cmd.ExecuteNonQuery();
                        if (execNumber <= 0)
                        {
                            throw new Exception("20120");
                        }
                        #endregion

                        #region 栈板装满后，更新状态
                        if (boxCount == PalletQty)
                        {
                            sql = @"INSERT INTO mesPackageHistory(PackageID, PackageStatusID, StationID, EmployeeID, Time)
                                VALUES(@PackageId, 2, @stationId, @employeeId, GETDATE())";
                            cmd.CommandText = sql;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddRange(new SqlParameter[3]{new SqlParameter("@PackageId", PalletID),
                                                                             new SqlParameter("@stationId", loginList.StationID),
                                                                             new SqlParameter("@employeeId", loginList.EmployeeID)});
                            execNumber = cmd.ExecuteNonQuery();
                            if (execNumber <= 0)
                            {
                                throw new Exception("20120");
                            }

                            sql = @"UPDATE mesPackage SET CurrentCount=@packCount,StationID=@stationId,EmployeeID=@employeeId,
				                StatusID=1,LastUpdate=GETDATE() where ID=@PackageId";
                            cmd.CommandText = sql;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddRange(new SqlParameter[4]{new SqlParameter("@PackageId", PalletID),
                                                                             new SqlParameter("@stationId", loginList.StationID),
                                                                             new SqlParameter("@employeeId", loginList.EmployeeID),
                                                                             new SqlParameter("@packCount", PalletQty)});
                            execNumber = cmd.ExecuteNonQuery();
                            if (execNumber <= 0)
                            {
                                throw new Exception("20121");
                            }
                        }
                        #endregion

                        transaction.Commit();
                        return "1";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {

                return ex.Message.ToString();
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
        public string uspPalletPackaging_Siemens(string PartID, string ProductionOrderID, string S_CartonSN, string S_PalletSN, LoginList loginList, int PalletQty = 0)
        {
            try
            {
                int execNumber = 0;
                string strSql = @"SELECT ID FROM mesPackage WHERE SerialNumber=@S_PalletSN AND StatusID=0";

                DataTable dtMp = SqlServerHelper.ExecuteDataTable(strSql, new SqlParameter("@S_PalletSN", S_PalletSN));
                if (dtMp == null || dtMp.Rows.Count == 0)
                {
                    return "20119";
                }
                string PalletID = dtMp.Rows[0]["ID"].ToString();

                if (PalletQty > 0)
                {
                    strSql = @"INSERT INTO mesPackageHistory(PackageID, PackageStatusID, StationID, EmployeeID, Time)
                                VALUES(@PackageId, 2, @stationId, @employeeId, GETDATE())";
                    execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@PackageId", PalletID),
                                                                         new SqlParameter("@stationId", loginList.StationID),
                                                                         new SqlParameter("@employeeId", loginList.EmployeeID));
                    if (execNumber < 0)
                    {
                        return "20120";
                    }

                    strSql = @"UPDATE mesPackage SET CurrentCount=@packCount,StationID=@stationId,EmployeeID=@employeeId,
				                StatusID=1,LastUpdate=GETDATE() where ID=@PackageId";
                    execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@PackageId", PalletID),
                                                                         new SqlParameter("@stationId", loginList.StationID),
                                                                         new SqlParameter("@employeeId", loginList.EmployeeID),
                                                                         new SqlParameter("@packCount", PalletQty));
                    if (execNumber < 0)
                    {
                        return "20121";
                    }
                }
                else
                {
                    strSql = "SELECT Stage FROM mesPackage WHERE ID=@ParentID";
                    DataTable dtStage = SqlServerHelper.ExecuteDataTable(strSql, new SqlParameter("@ParentID", PalletID));

                    if (dtStage.Rows[0]["Stage"].ToString() != "3")
                    {
                        //关联包装信息
                        strSql = @"UPDATE A SET A.ParentID=@ParentID  FROM mesPackage A WHERE A.SerialNumber=@SerialNumber";
                        execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@SerialNumber", S_CartonSN),
                                                                             new SqlParameter("@ParentID", PalletID));
                        if (execNumber < 0)
                        {
                            return "20122";
                        }
                    }
                    else
                    {
                        //关联包装信息
                        strSql = @"UPDATE A SET A.ShipmentParentID=@ParentID  FROM mesPackage A WHERE A.SerialNumber=@SerialNumber";
                        execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@SerialNumber", S_CartonSN),
                                                                             new SqlParameter("@ParentID", PalletID));
                        if (execNumber < 0)
                        {
                            return "20122";
                        }

                        //记录Shipment履历
                        strSql = @"SELECT top 1 ID FROM mesPackage WHERE SerialNumber=@S_CartonSN";
                        DataTable dtBoxMp = SqlServerHelper.ExecuteDataTable(strSql, new SqlParameter("@S_CartonSN", S_CartonSN));
                        string BoxID = dtBoxMp.Rows[0]["ID"].ToString();

                        strSql = @"INSERT INTO mesPackageHistory(PackageID, PackageStatusID, StationID, EmployeeID, Time)
                                VALUES(@PackageId, 8, @stationId, @employeeId, GETDATE())";
                        execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@PackageId", BoxID),
                                                                             new SqlParameter("@stationId", loginList.StationID),
                                                                             new SqlParameter("@employeeId", loginList.EmployeeID));
                        if (execNumber < 0)
                        {
                            return "20120";
                        }
                    }

                    //记录包装数据所有UPC/FG履历信息
                    //strSql = @"SELECT C.Value SerialNumber,B.KitSerialNumber from mesPackage A
                    //            INNER JOIN mesUnitDetail B ON A.ID=B.InmostPackageID
                    //            INNER JOIN mesSerialNumber C ON B.UnitID=C.UnitID
                    //            WHERE A.SerialNumber=@S_CartonSN";
                    //DataTable dtUPCUnit = SqlServerHelper.ExecuteDataTable(strSql, new SqlParameter("@S_CartonSN", S_CartonSN));
                    //if (dtUPCUnit == null || dtUPCUnit.Rows.Count == 0)
                    //{
                    //    return "20123";
                    //}
                    //string strResult = string.Empty;
                    //foreach (DataRow dr in dtUPCUnit.Rows)
                    //{
                    //    string FGSN = dr["SerialNumber"].ToString();
                    //    string UPCSN = dr["KitSerialNumber"].ToString();
                    //    strResult = SetmesHistory(FGSN, loginList, ProductionOrderID);
                    //    if (strResult != "1")
                    //    {
                    //        return strResult;
                    //    }
                    //    if (!string.IsNullOrEmpty(UPCSN) && UPCSN != FGSN)
                    //    {
                    //        strResult = SetmesHistory(UPCSN, loginList, ProductionOrderID);
                    //        if (strResult != "1")
                    //        {
                    //            return strResult;
                    //        }
                    //    }
                    //}
                }
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
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
        /// <param name="type">1:包装SN  2：栈板SN 3:MultipackPallet</param>
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
                    return result = "20087";
                }

                //生成条码插入包装表
                string strSql = @"INSERT INTO mesPackage(ID,SerialNumber,StationID,EmployeeID,CreationTime,StatusID,LastUpdate,Stage,CurrProductionOrderID,CurrPartID)
				                VALUES (ISNULL((SELECT MAX(ID) FROM mesPackage),0)+1,@BoxSN,@stationId,@employeeId,GETDATE(),0,GETDATE(),@type,@prodId,@partId)";
                int execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@BoxSN", S_SN),
                                                                         new SqlParameter("@stationId", v_mesUnit.StationID),
                                                                         new SqlParameter("@employeeId", v_mesUnit.EmployeeID),
                                                                         new SqlParameter("@type", type),
                                                                         new SqlParameter("@prodId", v_mesUnit.ProductionOrderID),
                                                                         new SqlParameter("@partId", v_mesUnit.PartID));
                if (execNumber < 0)
                {
                    return "20088";
                }
                strSN = S_SN;
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        /// <summary>
        /// 生成SN并注册包装表     ****停用****
        /// </summary>
        /// <param name="S_FormatName"></param>
        /// <param name="xmlProdOrder"></param>
        /// <param name="xmlPart"></param>
        /// <param name="xmlStation"></param>
        /// <param name="xmlExtraData"></param>
        /// <param name="v_mesUnit"></param>
        /// <param name="strSN"></param>
        /// <param name="type">1:包装SN  2：栈板SN 3:MultipackPallet  这里类别无效，操作西门子库, </param>
        /// <returns></returns>
        public string Get_CreatePackageSN_Siemens(string S_FormatName, string xmlProdOrder, string xmlPart, string xmlStation,
                                        string xmlExtraData, string MultipackSN, ref string strSN, int type)
        {
            try
            {
                string result = "1";

                //生成条码
                DataTable DT = uspSNRGetNext(S_FormatName, 0, "'" + xmlProdOrder + "'", "'" + xmlPart + "'", "'" + xmlStation + "'", "'" + xmlExtraData + "'", "").Tables[1];
                string S_SN = DT.Rows[0][0].ToString();
                if (string.IsNullOrEmpty(S_SN))
                {
                    return result = "20087!";
                }

                //生成条码插入包装表
                //string strSql = @"INSERT INTO mesPackage(ID,SerialNumber,StationID,EmployeeID,CreationTime,StatusID,LastUpdate,Stage,CurrProductionOrderID,CurrPartID)
                //    VALUES (ISNULL((SELECT MAX(ID) FROM mesPackage),0)+1,@BoxSN,@stationId,@employeeId,GETDATE(),0,GETDATE(),@type,@prodId,@partId)";
                //int execNumber = SqlServerHelper.ExecuteNonQuery(strSql, new SqlParameter("@BoxSN", S_SN),
                //                                                         new SqlParameter("@stationId", v_mesUnit.StationID),
                //                                                         new SqlParameter("@employeeId", v_mesUnit.EmployeeID),
                //                                                         new SqlParameter("@type", type),
                //                                                         new SqlParameter("@prodId", v_mesUnit.ProductionOrderID),
                //                                                         new SqlParameter("@partId", v_mesUnit.PartID));

                string Result = "";
                DataSet DS = uspCallProcedure("uspUpdateCOSMO_RA_Receipt", MultipackSN,
                                                    null, null, null, null, S_SN, ref Result);


                if (Result=="")
                {
                    return "20088";
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
            string S_Sql = @"SELECT ROW_NUMBER() OVER(ORDER BY A.ID) SEQNO, KitSerialNumber UPCSN,D.Value SN,C.LastUpdate TIME 
                                FROM mesPackage A INNER JOIN mesUnitDetail B ON A.ID=B.InmostPackageID 
                                inner join mesUnit C on B.UnitID=C.ID
                                inner join mesSerialNumber D on C.ID=D.UnitID
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


        public DataSet Get_SN_Shell(string FGSN, ref string strOutput)
        {
            strOutput = "1";
            DataSet ds = new DataSet();
            try
            {
                string S_Sql = @"select b.ChildSerialNumber from mesSerialNumber a inner join mesUnitComponent b on a.UnitID=b.UnitID
                                 where a.Value = '" + FGSN + "' and(ChildSerialNumber like 'TGP%' or ChildSerialNumber like 'FPR%') ";
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
                    strOutput = "20040";
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
                        new SqlParameter("@strOutput", SqlDbType.VarChar,1024)
                };

                commandParameters[6].Direction = ParameterDirection.Output;
                dataSet = SqlServerHelper.ExecuteNonQueryPro(Pro_Name, commandParameters);
                strOutput = commandParameters[6].Value.ToString();
                return dataSet;
            }
            catch (Exception ex)
            {
                strOutput = ex.Message.ToString();
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
        public DataSet MESGetBomPartInfo(int ParentPartID, int StationTypeID)
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
        public string MESAssembleCheckMianSN(string ProductionOrderID, int LineID, int StationID,
                                                int StationTypeID, string SN, bool COF)
        {
            DataTable DT_SN = GetmesSerialNumber(SN).Tables[0];
            if (DT_SN == null || DT_SN.Rows.Count == 0)
            {
                return "20012";
            }

            //string S_SerialNumberType = DT_SN.Rows[0]["SerialNumberTypeID"].ToString();
            //if (S_SerialNumberType != "5" && S_SerialNumberType != "0")
            //{
            //    return "20035";
            //}

            string S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
            DataTable DT_Unit = GetmesUnit(S_UnitID).Tables[0];
            if (!COF)
            {
                if (DT_Unit.Rows[0]["StatusID"].ToString() != "1")
                {
                    return "20036";
                }
            }

            string UnitProductionOrderID = DT_Unit.Rows[0]["ProductionOrderID"].ToString();
            if (UnitProductionOrderID != ProductionOrderID)
            {
                return "20133";
            }

            string S_RouteCheck = GetRouteCheck(StationTypeID, StationID, LineID.ToString(), DT_Unit, SN);
            return S_RouteCheck;
        }

        /// <summary>
        /// 校验其他类型条码(存在系统中条码)
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public string MESAssembleCheckOtherSN(string SN, string PartID, bool COF)
        {
            DataTable DT_SN = GetmesSerialNumber(SN).Tables[0];
            string S_UnitID = string.Empty;

            if (DT_SN == null || DT_SN.Rows.Count == 0)
            {
                return "20012";
            }

            //string S_SerialNumberType = DT_SN.Rows[0]["SerialNumberTypeID"].ToString();
            //if (S_SerialNumberType != "5" && S_SerialNumberType != "0")
            //{
            //    return "20035";
            //}

            S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
            DataTable DT_Unit = GetmesUnit(S_UnitID).Tables[0];
            if (!COF)
            {
                if (DT_Unit.Rows[0]["StatusID"].ToString() != "1")
                {
                    return "20036";
                }
            }

            string UnitPart = DT_Unit.Rows[0]["PartID"].ToString();
            if (UnitPart != PartID)
            {
                //替代料逻辑预留
                return "20037";
            }

            mesRouteDAL mesRouteDAL = new mesRouteDAL();

            //修改最后一站校验逻辑 20201112
            //if (!mesRouteDAL.MESCheckLastStation(Convert.ToInt32(S_UnitID)))
            //{
            //    return "20038";
            //}
            string strOutput = string.Empty;
            uspCallProcedure("uspRouteLastSationCheck", SN, "", "", "", "", "", ref strOutput);
            if (strOutput != "1")
            {
                return strOutput;
            }

            string sql = string.Format(@"select 1 from mesUnitComponent A INNER JOIN mesSerialNumber B
                            ON A.UnitID = B.UnitID AND A.ChildSerialNumber ='{0}' and A.StatusID=1", SN);
            DataTable dt = SqlServerHelper.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                return "20039";
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
                return "20012";
            }

            //状态检查
            string S_StatusID = DT_Machine.Rows[0]["StatusID"].ToString();
            if (S_StatusID != Status)
            {
                return "20045";
            }

            string S_MachineFamilyID = DT_Machine.Rows[0]["MachineFamilyID"].ToString();
            string S_MachinePartID = DT_Machine.Rows[0]["PartID"].ToString();
            string Str_Sql = string.Format(@"select top 1 PartID from  mesRouteMachineMap where (MachineFamilyID = CASE WHEN ISNULL({0},'')='' THEN MachineFamilyID ELSE {0}  END
	                                            OR MachineFamilyID = CASE WHEN ISNULL({1},'')='' THEN MachineFamilyID ELSE {1} END) 
                                                    AND (ISNULL({0},'')<>'' OR ISNULL({1},'')<>'')", S_MachineFamilyID, S_MachinePartID);
            DataTable dtMap = SqlServerHelper.ExecuteDataTable(Str_Sql);

            if (dtMap == null || dtMap.Rows.Count == 0)
            {
                return "20043";
            }

            if (!string.IsNullOrEmpty(PartID))
            {
                string S_MapPartID = dtMap.Rows[0]["PartID"].ToString();

                if (PartID != S_MapPartID)
                {
                    //替代料逻辑预留
                    return "20043";
                }
            }

            return "1";
        }

        /// <summary>
        /// 非系统数据保存
        /// </summary>
        /// <param name="SN">子料SN</param>
        /// <param name="PartID">料号</param>
        /// <returns></returns>
        public void MESModifyUnitDetail(int UnitID, string FileName, string Value)
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
                return ex.Message.ToString();
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

        public DataSet GetLanguage(string FormName, string Type)
        {
            string S_Sql = string.Format(@"SELECT * FROM sysLanguage where ('{0}'='' OR FormName='{0}') 
                                            and ('{1}'='' OR Type='{1}')", FormName, Type);
            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }

        public string GetVer()
        {
            string S_Result = "";
            string S_Sql = "select VersionNumber from sysVersion";
            DataTable DT = SqlServerHelper.Data_Table(S_Sql);

            if (DT.Rows.Count > 0)
            {
                S_Result = DT.Rows[0][0].ToString();
            }
            return S_Result;
        }

        public string GetMSG(string S_Lang, string S_Code)
        {
            string S_Result = "";
            string S_Sql = "select Description from sysLanguage  where Language='" + S_Lang + "' and Code='" + S_Code + "' ";
            DataTable DT = SqlServerHelper.Data_Table(S_Sql);

            if (DT.Rows.Count > 0)
            {
                S_Result = DT.Rows[0][0].ToString();
            }
            return S_Result;
        }

        public DataSet GetVendor(string PartID)
        {
            string S_Sql = "select * from luVendor where PartID=" + PartID;
            DataSet dsVendor = SqlServerHelper.Data_Set(S_Sql);
            return dsVendor;
        }

        public bool SetToolingLinkTooling(string FromTooling, string ToTooling, int FromUintID, LoginList loginList)
        {
            string Str_Sql = string.Format("SELECT MAX(UnitID) UnitID FROM mesUnitDetail WHERE reserved_01='{0}'", ToTooling);

            string ToUnitID = string.Empty;
            DataTable dts = SqlServerHelper.ExecuteDataTable(Str_Sql);
            if (dts == null || dts.Rows.Count == 0)
            {
                return false;
            }

            ToUnitID = dts.Rows[0]["UnitID"].ToString();
            Str_Sql = string.Format(@"insert into mesUnitDetail(UnitID,ProductionOrderID,RMAID,LooperCount,KitSerialNumber,InmostPackageID,OutmostPackageID
                                          , reserved_01, reserved_02, reserved_03, reserved_04, reserved_05, reserved_06, reserved_07, reserved_08, reserved_09, reserved_10
                                          , reserved_11, reserved_12, reserved_13, reserved_14, reserved_15, reserved_16, reserved_17, reserved_18, reserved_19, reserved_20)
                                    select {0},ProductionOrderID,RMAID,LooperCount,KitSerialNumber,InmostPackageID,OutmostPackageID
                                          ,reserved_01,reserved_02,reserved_03,reserved_04,reserved_05,reserved_06,reserved_07,reserved_08,reserved_09,reserved_10
                                          ,reserved_11,reserved_12,reserved_13,reserved_14,reserved_15,reserved_16,reserved_17,reserved_18,reserved_19,reserved_20
                                    from mesUnitDetail where UnitID ={1} ", FromUintID, ToUnitID);
            SqlServerHelper.ExecuteNonQuery(Str_Sql);

            Str_Sql = string.Format(@"INSERT INTO mesUnitComponent(UnitID,UnitComponentTypeID,ChildUnitID,ChildSerialNumber,ChildLotNumber
	                                      ,ChildPartID,ChildPartFamilyID,Position,InsertedEmployeeID,InsertedStationID,InsertedTime
	                                      ,RemovedEmployeeID,RemovedStationID,RemovedTime,StatusID,LastUpdate,PreviousLink)
                                      SELECT {0},UnitComponentTypeID,ChildUnitID,ChildSerialNumber,ChildLotNumber
	                                      ,ChildPartID,ChildPartFamilyID,Position,'{1}',{2},GETDATE()
	                                      ,RemovedEmployeeID,RemovedStationID,RemovedTime,StatusID,GETDATE(),PreviousLink
	                                      FROM mesUnitComponent WHERE UnitID={3} AND StatusID=1", FromUintID, loginList.EmployeeID, loginList.StationID, ToUnitID);
            SqlServerHelper.ExecuteNonQuery(Str_Sql);
            return true;
        }

        public string Get_CreateMaterail(string strSNFormat, string xmlProdOrder, string xmlPart, string xmlStation,
                                      string xmlExtraData, LoginList loginList, mesUnit v_mesUnit, MesMaterialUnit v_mesMaterialUnit, int number, ref DataSet dsSN)
        {
            string result = "1";

            if (dsSN == null)
            {
                dsSN = new DataSet();
            }

            mesUnitDAL mesUnitDll = new mesUnitDAL();
            mesSerialNumberDAL mesSerialNumberDll = new mesSerialNumberDAL();
            mesHistoryDAL mesHistoryDll = new mesHistoryDAL();
            mesUnitDetailDAL mesUnitDetailDll = new mesUnitDetailDAL();
            MesMaterialUnitDAL mesMaterialUnitDll = new MesMaterialUnitDAL();

            string S_FormatName = string.Empty;
            S_FormatName = mesGetSNFormatIDByList(v_mesUnit.PartID.ToString(), v_mesUnit.PartFamilyID.ToString(),
                    v_mesUnit.LineID.ToString(), v_mesUnit.ProductionOrderID.ToString(), loginList.StationTypeID.ToString());
            if (string.IsNullOrEmpty(S_FormatName))
            {
                return result = "20132";
            }

            int routeID = GetRouteID(loginList.LineID.ToString(), v_mesUnit.PartID.ToString(),
                                        v_mesUnit.PartFamilyID.ToString(), v_mesUnit.ProductionOrderID.ToString());
            if (routeID == -1)
            {
                return result = "20195";
            }
            DataTable dtState = GetmesUnitState(v_mesUnit.PartID.ToString(), v_mesUnit.PartFamilyID.ToString(), "",
                v_mesUnit.LineID.ToString(), loginList.StationTypeID, v_mesUnit.ProductionOrderID.ToString(), v_mesUnit.StatusID.ToString()).Tables[0];

            if (dtState == null || dtState.Rows.Count == 0)
            {
                return result = "20131";
            }
            v_mesUnit.UnitStateID = Convert.ToInt32(dtState.Rows[0][0].ToString());

            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("SN", typeof(string));
                for (; number > 0; number--)
                {
                    try
                    {
                        //写入UNIT表
                        string unitId = string.Empty;
                        v_mesUnit.CreationTime = DateTime.Now;
                        v_mesUnit.LastUpdate = DateTime.Now;
                        unitId = mesUnitDll.Insert(v_mesUnit);
                        if (string.IsNullOrEmpty(unitId) || unitId.Substring(0, 1) == "E")
                        {
                            return result = "20130";
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
                            return result = "20087";
                        }

                        //写入mesSerialNumber
                        mesSerialNumber v_mesSerialNumber = new mesSerialNumber();
                        v_mesSerialNumber.UnitID = Convert.ToInt32(unitId);
                        v_mesSerialNumber.SerialNumberTypeID = v_mesUnit.SerialNumberType;
                        v_mesSerialNumber.Value = S_SN;
                        mesSerialNumberDll.Insert(v_mesSerialNumber);

                        //写入UnitDetail表
                        mesUnitDetail msDetail = new mesUnitDetail();
                        msDetail.UnitID = Convert.ToInt32(unitId);
                        msDetail.reserved_01 = "";
                        msDetail.reserved_02 = "";
                        msDetail.reserved_03 = "";
                        msDetail.reserved_04 = "";
                        msDetail.reserved_05 = "";
                        mesUnitDetailDll.Insert(msDetail);

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


                        //写入MaterialUnit表
                        v_mesMaterialUnit.SerialNumber = S_SN;
                        mesMaterialUnitDll.InserForParent(v_mesMaterialUnit);

                        DataRow dr = dt.NewRow();
                        dr["SN"] = S_SN;
                        dt.Rows.Add(dr);
                    }
                    catch { }
                    
                }
                dsSN.Tables.Add(dt);
            }
            catch (Exception ex)
            {
                return result = ex.Message.ToString();
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginList"></param>
        /// <param name="ScanType">1:夹具绑批次校验/扫描批次组装校验 2:扫描夹具组装校验</param>
        /// <param name="SN"></param>
        /// <param name="MachineSN"></param>
        /// <param name="PartID"></param>
        /// <param name="ProductionOrderID"></param>
        /// <returns></returns>
        public string ModMesMaterialConsumeInfo(LoginList loginList, int ScanType, string SN,
                                                    string MachineSN, int PartID, int ProductionOrderID)
        {
            string Str_sql = string.Empty;
            int ConsumeQTY = 1;
            if (ScanType == 2)
            {
                Str_sql = string.Format(@"SELECT reserved_02 FROM mesUnitDetail WHERE ID = (SELECT MAX(ID) 
				                FROM mesUnitDetail WHERE reserved_01='{0}' AND reserved_03=1)", MachineSN);
                DataTable dtCq = SqlServerHelper.ExecuteDataTable(Str_sql);
                if (dtCq == null || dtCq.Rows.Count == 0)
                {
                    return "20054";
                }
                else
                {
                    SN = dtCq.Rows[0]["reserved_02"].ToString();
                }
            }

            Str_sql = string.Format(@"SELECT 1 FROM mesMaterialConsumeInfo where PartID={0} and ProductionOrderID={1} and LineID = {2} and 
                                        StationID = {3} AND SN = '{4}'", PartID, ProductionOrderID, loginList.LineID, loginList.StationID, SN);
            DataTable dtRs = SqlServerHelper.ExecuteDataTable(Str_sql);
            if (dtRs != null && dtRs.Rows.Count > 0)
            {
                Str_sql = string.Format(@"UPDATE mesMaterialConsumeInfo SET ConsumeQTY=isnull(ConsumeQTY,0)+{0} where PartID={1} and 
				                            ProductionOrderID={2} and LineID={3} and StationID={4} AND SN='{5}'",
                                            ConsumeQTY, PartID, ProductionOrderID, loginList.LineID, loginList.StationID, SN);
                SqlServerHelper.ExecuteNonQuery(Str_sql);
            }
            else
            {
                Str_sql = string.Format(@"INSERT INTO mesMaterialConsumeInfo(SN,MaterialTypeID,PartID,ProductionOrderID,LineID,StationID,ConsumeQTY)
			                                VALUES ('{0}',1,{1},{2},{3},{4},{5})",
                                            SN, PartID, ProductionOrderID, loginList.LineID, loginList.StationID, ConsumeQTY);
                SqlServerHelper.ExecuteNonQuery(Str_sql);
            }
            return "1";
        }


        public DataSet GetmesStationConfig(string Name, string StationID)
        {
            string S_Sql = string.Format(@"SELECT * FROM mesStationConfigSetting where ('{0}'='' OR Name='{0}') 
                                            and ('{1}'='' OR StationID='{1}')", Name, StationID);
            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }

        public DataSet GetShipmentInterID(string ShipmentDetailID)
        {
            string S_Sql = string.Format(@"select * from CO_WH_ShipmentEntryNew where FDetailID={0}", ShipmentDetailID);
            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }

        public void SetMesPackageShipmennt(string ShipmentDetailID, string SerialNumber, int Type)
        {
            string S_Sql = string.Empty;
            string ShipmentInterID = "";
            S_Sql = string.Format(@"select FInterID from CO_WH_ShipmentEntryNew where FDetailID={0}", ShipmentDetailID);
            DataTable dt = SqlServerHelper.ExecuteDataTable(S_Sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                ShipmentInterID = dt.Rows[0]["FInterID"].ToString();
            }

            if (Type == 1)
            {
                S_Sql = string.Format(@"UPDATE mesPackage SET ShipmentInterID={0},ShipmentTime=GETDATE() WHERE SerialNumber='{1}'",
                    ShipmentInterID, SerialNumber);
            }
            else if (Type == 2)
            {
                S_Sql = string.Format(@"UPDATE mesPackage SET ShipmentInterID={0},ShipmentTime=GETDATE(),ShipmentDetailID={2} WHERE SerialNumber='{1}'",
                    ShipmentInterID, SerialNumber, ShipmentDetailID);
            }
            SqlServerHelper.ExecSql(S_Sql);
        }

        public DataSet GetMesLabelData(string LabelName)
        {
            string S_Sql = string.Format(@"SELECT NAME+','+SourcePath+','+ CAST(OutputType AS VARCHAR)+','+TargetPath+','+CAST(PageCapacity AS varchar)  AS LablePath FROM mesLabel WHERE NAME ='{0}'", LabelName);
            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }

        public int InsertMesPackageHistory(mesPackageHistory model)
        {
            object obj = SqlServerHelper.ExecuteScalar(
                @"INSERT INTO mesPackageHistory(PackageID, PackageStatusID, StationID, EmployeeID, Time)
                                VALUES(@PackageId, @PackageStatusID, @StationID, @EmployeeID, GETDATE());SELECT @@identity"
                , new SqlParameter("@PackageId", model.PackageID)
                , new SqlParameter("@PackageStatusID", model.PackageStatusID)
                , new SqlParameter("@StationID", model.StationID)
                , new SqlParameter("@EmployeeID", model.EmployeeID)
            );
            string S_Result = obj.ToString();
            if (S_Result == "") { S_Result = "0"; }

            return Convert.ToInt32(S_Result);
        }


        public DataSet GetShipmentNew(string S_Start, string S_End, string FStatus)
        {
            string S_Sql = @"select *,
　　　　                    (case FStatus
　　　　	                    when 0     then    '暂未确认NoConfirm'
　　　　	                    when 1     then    '已确认栈板纸ConfirmedOK'
　　　　	                    when 2     then    '已扫描出库ShipScanOK'
　　　		                    when 3     then    '已确认出货Shipped'
			                    when 4     then    '已生成EDI'
			                    when 5     then    '已发送SAP_Recieved'
			                    when 6     then    '已关闭Closed'
						                    else '未定义' end
		                    ) MyStatus
                    FROM CO_WH_ShipmentNew
                  where FDate>='" + S_Start + "' and FDate<='" + S_End + "'";
            if (FStatus != "999")
            {
                S_Sql += " and FStatus=" + FStatus;
            }

            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }

        public DataSet GetShipmentEntryNew(string S_FInterID)
        {
            string S_Sql = "select FDetailID,FMPNNO,FKPONO,FCTN from CO_WH_ShipmentEntryNew where FInterID='" + S_FInterID + "'";
            DataTable DT = SqlServerHelper.Data_Table(S_Sql);
            int I_Count = 12 - DT.Rows.Count;

            S_Sql = "select FDetailID,FMPNNO,FKPONO,FCTN,FProjectNO,FEntryID,FLineItem from CO_WH_ShipmentEntryNew where FInterID='" + S_FInterID + "'" + "\r\n" +
                     "union select top " + I_Count.ToString() +
                     " 100000000 + column_id FDetailID,'' FMPNNO,'' FKPONO,0 FCTN,'' FProjectNO,'' FEntryID,'' FLineItem from sys.columns";
            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }

        public DataSet GetShipmentReport(string S_Start, string S_End, string FStatus)
        {
            string S_Sql = @"select A.*,B.FInterID, B.FEntryID, B.FDetailID, B.FKPONO, B.FMPNNO, B.FCTN FCTN2,B.FProjectNO FProjectNO2,
                                   B.FStatus, B.FOutSN, B.FLineItem, B.FPartNumberDesc, B.FQTY, B.FCrossWeight,
                                   B.FNetWeight, B.FCarrierNo, B.FCommercialInvoice, B.FProjectNO,
                                    (SELECT value FROM  [dbo].[F_Split](A.FShipNO,'#') WHERE id=1 )  FShipNO_L1  
                              from CO_WH_ShipmentNew A join CO_WH_ShipmentEntryNew B
                              on A.FInterID = B.FInterID 
                where A.FDate>='" + S_Start + "' and A.FDate<='" + S_End + "'";

            if (FStatus != "999")
            {
                S_Sql += " and A.FStatus=" + FStatus;
            }

            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }

        public string Edit_CO_WH_ShipmentNew
            (
                string S_FInterID,

                string S_HAWB,
                string S_PalletCount,
                string S_GrossWeight,
                string S_Project,

                string S_ShipDate,
                string S_PalletSeq,
                string S_EmptyCarton,

                string S_PalletSN,
                string S_ShipNO,
                string S_ShipID,

                string S_Regin,
                string S_ReferenceNO,
                string S_Country,
                string S_Carrier,
                string S_HubCode,
                string S_TruckNo,
                string S_ReturnAddress,
                string S_DeliveryStreetAddress,
                string S_DeliveryRegion,
                string S_DeliveryToName,
                string S_DeliveryCityName,
                string S_DeliveryCountry,
                string S_AdditionalDeliveryToName,
                string S_DeliveryPostalCode,
                string S_TelNo,

                string S_OceanContainerNumber,
                string S_Origin,
                string S_TotalVolume,
                string S_POE_COC,
                string S_TransportMethod,
                string S_POE,

                string S_Type
            )
        {
            string S_strOutput = "1";
            string S_Sql = "exec Edit_CO_WH_ShipmentNew " +
                        "'" + S_FInterID + "'," +
                        "'" + S_HAWB + "'," +
                        "'" + S_PalletCount + "'," +
                        "'" + S_GrossWeight + "'," +
                        "'" + S_Project + "'," +

                        "'" + S_ShipDate + "'," +
                        "'" + S_PalletSeq + "'," +
                        "'" + S_EmptyCarton + "'," +

                        "'" + S_PalletSN + "'," +
                        "'" + S_ShipNO + "'," +
                        "'" + S_ShipID + "'," +

                        "'" + S_Regin + "'," +
                        "'" + S_ReferenceNO + "'," +
                        "'" + S_Country + "'," +
                        "'" + S_Carrier + "'," +
                        "'" + S_HubCode + "'," +
                        "'" + S_TruckNo + "'," +
                        "'" + S_ReturnAddress + "'," +
                        "'" + S_DeliveryStreetAddress + "'," +
                        "'" + S_DeliveryRegion + "'," +
                        "'" + S_DeliveryToName + "'," +
                        "'" + S_DeliveryCityName + "'," +
                        "'" + S_DeliveryCountry + "'," +
                        "'" + S_AdditionalDeliveryToName + "'," +
                        "'" + S_DeliveryPostalCode + "'," +
                        "'" + S_TelNo + "'," +

                        "'" + S_OceanContainerNumber + "'," +
                        "'" + S_Origin + "'," +
                        "'" + S_TotalVolume + "'," +
                        "'" + S_POE_COC + "'," +
                        "'" + S_TransportMethod + "'," +
                        "'" + S_POE + "'," +

                        "'" + S_Type + "'";
            DataTable DT = SqlServerHelper.Data_Table(S_Sql);
            S_strOutput = DT.Rows[0][0].ToString();
            return S_strOutput;
        }

        public string DeleteShipmentNew(string FInterID)
        {
            string S_Result = "1";
            try
            {
                string S_Sql = "select FStatus from CO_WH_ShipmentNew where FInterID='" + FInterID + "'";
                DataTable DT = SqlServerHelper.Data_Table(S_Sql);
                if (DT.Rows[0][0].ToString() != "0")
                {
                    S_Result = "It has been confirmed that deletion is not allowed";
                }
                else
                {
                    S_Sql = "delete CO_WH_ShipmentNew  where FInterID='" + FInterID + "'";
                    SqlServerHelper.ExecSql(S_Sql);

                    S_Sql = "delete CO_WH_ShipmentEntryNew  where FInterID='" + FInterID + "'";
                    SqlServerHelper.ExecSql(S_Sql);
                }
            }
            catch (Exception ex)
            {
                S_Result = ex.ToString();
            }
            return S_Result;
        }

        public string DeleteShipmentEntryNew(string FDetailID)
        {
            string S_Result = "1";
            try
            {
                string S_Sql = "delete CO_WH_ShipmentEntryNew where FDetailID='" + FDetailID + "'";
                SqlServerHelper.ExecSql(S_Sql);
            }
            catch (Exception ex)
            {
                S_Result = ex.ToString();
            }
            return S_Result;
        }

        public string UpdateShipmentNew_FStatus(string FInterID_List, string Status)
        {
            string S_Result = "1";
            try
            {
                string S_Sql = "Update  CO_WH_ShipmentNew set FStatus='" + Status + "' where FInterID in(" + FInterID_List + ")";
                SqlServerHelper.ExecSql(S_Sql);
            }
            catch (Exception ex)
            {
                S_Result = ex.ToString();
            }
            return S_Result;
        }

        public DataSet GetShipmentNew_One(string FInterID)
        {
            string S_Sql = "select * from CO_WH_ShipmentNew where FInterID='" + FInterID + "'";
            DataSet DS = SqlServerHelper.Data_Set(S_Sql);
            return DS;
        }

        public DataSet GetShipmentEntryNew_One(string FDetailID)
        {
            string S_Sql = "select * from CO_WH_ShipmentEntryNew where FDetailID='" + FDetailID + "'";
            DataSet DS = SqlServerHelper.Data_Set(S_Sql);
            return DS;
        }

        public string Edit_CO_WH_ShipmentEntryNew
            (
                string S_FDetailID,
                string S_FInterID,
                string S_FEntryID,
                string S_FCarrierNo,
                string S_FCommercialInvoice,
                string S_FCrossWeight,
                string S_FCTN,
                string S_FKPONO,
                string S_FLineItem,
                string S_FMPNNO,
                string S_FNetWeight,
                string S_FOutSN,
                string S_FPartNumberDesc,
                string S_FQTY,
                string S_FStatus,
                string S_FProjectNO,

                string S_Type
            )
        {
            string S_strOutput = "1";
            string S_Sql = "exec Edit_CO_WH_ShipmentEntryNew " +
                            "'" + S_FDetailID + "'," +
                            "'" + S_FInterID + "'," +
                            "'" + S_FEntryID + "'," +
                            "'" + S_FCarrierNo + "'," +
                            "'" + S_FCommercialInvoice + "'," +
                            "'" + S_FCrossWeight + "'," +
                            "'" + S_FCTN + "'," +
                            "'" + S_FKPONO + "'," +
                            "'" + S_FLineItem + "'," +
                            "'" + S_FMPNNO + "'," +
                            "'" + S_FNetWeight + "'," +
                            "'" + S_FOutSN + "'," +
                            "'" + S_FPartNumberDesc + "'," +
                            "'" + S_FQTY + "'," +
                            "'" + S_FStatus + "'," +
                            "'" + S_FProjectNO + "'," +
                            "'" + S_Type + "'";

            DataTable DT = SqlServerHelper.Data_Table(S_Sql);
            S_strOutput = DT.Rows[0][0].ToString();
            return S_strOutput;
        }


        public DataSet GetORTluPartFamilyType(string DetailDefStr, string PartFamilyTypeID)
        {
            PartFamilyTypeID = string.IsNullOrEmpty(PartFamilyTypeID) ? "null" : PartFamilyTypeID;
            string strSql = String.Format(@"SELECT A.*,B.Content FROM luPartFamilyType A
                            LEFT JOIN mesPartFamilyTypeDetail B ON A.ID = B.PartFamilyTypeID
                            LEFT JOIN luPartFamilyTypeDetailDef C ON B.PartFamilyTypeDetailDefID = C.ID
                            WHERE C.Description = '{0}' and (A.ID={1} or {1} is null)", DetailDefStr, PartFamilyTypeID);
            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }

        public string GetORTCode(string PartFamilyTypeID, string YearID, string WeekID)
        {
            string ORTCode = string.Empty;
            string strSql = String.Format(@"SELECT ORTCode FROM mesORTTestWeek where 
                            PartFamilyTypeID='{0}' and YearID={1} and WeekID={2}", PartFamilyTypeID, YearID, WeekID);
            DataSet ds = SqlServerHelper.Data_Set(strSql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ORTCode = ds.Tables[0].Rows[0]["ORTCode"].ToString();
            }
            return ORTCode;
        }

        public void InsertORTCodeData(string ORTCode, string PartFamilyTypeID, string YearID, string WeekID)
        {
            string strSql = String.Format(@"insert into mesORTTestWeek(ORTCode,PartFamilyTypeID,YearID,WeekID)
                                            values ('{0}',{1},{2},{3})", ORTCode, PartFamilyTypeID, YearID, WeekID);
            SqlServerHelper.ExecuteNonQuery(strSql);
        }

        public void UpdateORTCodeData(string NewORTCode, string OldORTCode)
        {
            string strSql = String.Format(@"update mesORTTestWeek set ORTCode='{0}' where ORTCode='{1}'",
                                            NewORTCode, OldORTCode);
            SqlServerHelper.ExecuteNonQuery(strSql);
        }

        public DataSet GetORTMaxBatch(string OldORTCode, string TestTypeID)
        {
            string strSql = String.Format(@"select OrderNo from mesORTTestResult 
                                where ORTCode='{0}' AND TestTypeID={1}", OldORTCode, TestTypeID);
            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }

        public DataSet GetORTForCode(string ORTCode)
        {
            string strSql = String.Format(@"select * from mesORTTestWeek where ORTCode='{0}'", ORTCode);
            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }

        public string GetRouteCheck_Diagram(int Scan_StationTypeID, string LineID,
            DataTable DT_Unit, out string S_OutputStateID, string S_Str)
        {
            S_OutputStateID = "0";
            string S_Result = "1";
            string OpenLogFile = System.Configuration.ConfigurationManager.AppSettings["OpenLogFile"];
            DateTime dateStart = DateTime.Now;

            try
            {
                string S_UnitStationID = DT_Unit.Rows[0]["StationID"].ToString();
                string PartPamilyID = DT_Unit.Rows[0]["PartFamilyID"].ToString();
                string S_PartID = DT_Unit.Rows[0]["PartID"].ToString();
                string UnitID = DT_Unit.Rows[0]["ID"].ToString();
                string ProductionOrderID = DT_Unit.Rows[0]["ProductionOrderID"].ToString();
                int I_RouteID = GetRouteID(LineID, S_PartID, PartPamilyID, ProductionOrderID);
                string xmlExtraData = "<ExtraData StationTypeID=\"" + Scan_StationTypeID + "\"> </ExtraData>";
                string strOutput = string.Empty;
                uspCallProcedure("uspRouteDiagram", UnitID, "", "",
                                                                "", xmlExtraData, I_RouteID.ToString(), ref strOutput);
                return strOutput;

                //string S_PartID = DT_Unit.Rows[0]["PartID"].ToString();
                ////最后扫描工序 UnitStateID
                //string S_UnitStateID = DT_Unit.Rows[0]["UnitStateID"].ToString();
                //int I_UnitStateID = Convert.ToInt32(S_UnitStateID);  

                //string S_StatusID= DT_Unit.Rows[0]["StatusID"].ToString();
                //int I_StatusID = Convert.ToInt32(S_StatusID);

                //string S_UnitStationID = DT_Unit.Rows[0]["StationID"].ToString();
                //string PartPamilyID = DT_Unit.Rows[0]["PartFamilyID"].ToString();

                ////获取此料工序路径
                //int I_RouteID = GetRouteID(LineID, S_PartID, PartPamilyID);
                //if (I_RouteID == -1)
                //{
                //    return "20195";
                //}


                ////string S_Sql = "select *  from mesStation where ID= " + S_UnitStationID;
                ////DataTable DT_Station= SqlServerHelper.Data_Table(S_Sql);
                //string S_StationTypeID = Scan_StationTypeID.ToString();

                //string S_Sql = "select * from mesUnitOutputState where RouteID="+ I_RouteID.ToString()+
                //              " and StationTypeID="+S_StationTypeID;
                //DataTable DT_Route = SqlServerHelper.Data_Table(S_Sql);
                //if (DT_Route.Rows.Count == 0)
                //{
                //    S_Result = "20203";
                //}
                //else
                //{
                //    string S_CurrStateID = DT_Route.Rows[0]["CurrStateID"].ToString();
                //    if (S_CurrStateID == "0")
                //    {
                //        I_UnitStateID = 0;
                //    }

                //    if (I_UnitStateID == 1)
                //    {
                //        I_UnitStateID =Convert.ToInt32(S_CurrStateID);
                //    }

                //    //DataTable DT_Route = GetRoute("", I_RouteID).Tables[0];

                //    //当前工站类别  工序配置信息
                //    var v_Route_Sacn = from c in DT_Route.AsEnumerable()
                //                       where c.Field<int>("StationTypeID") == Scan_StationTypeID &&
                //                             c.Field<int>("CurrStateID") == I_UnitStateID &&
                //                             c.Field<int>("OutputStateDefID") == I_StatusID
                //                       select c;

                //    if (v_Route_Sacn != null)
                //    {
                //        if (v_Route_Sacn.ToList().Count() > 0)
                //        {
                //            var v_Query_OutputStateID = (from c in v_Route_Sacn select c).FirstOrDefault();
                //            S_OutputStateID = v_Query_OutputStateID.Field<int>("OutputStateID").ToString();
                //        }
                //        else
                //        {
                //            //"路径检查失败，请检查：是否NG / 是否前段工序未扫描 / 是否过站 / 是否没有没配本站";
                //            S_Result = "20240";
                //        }
                //    }
                //    else
                //    {
                //        S_Result = "20240";
                //    }
                //}
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
                        string msg = "In check(GetRouteCheck) StationID:" + Scan_StationTypeID + ",SN:"
                                        + S_Str + ",Time：" + mill.ToString() + "ms";
                        File.AppendAllText(logPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".log",
                         DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + msg + "\r\n");
                    }
                }
                catch (Exception ex)
                {
                    S_Result = ex.ToString();
                }
            }
            return S_Result;
        }


        public string AddmesReprintHistory(string PrintType, string SN, string StationID)
        {
            string S_Sql = "insert into mesReprintHistory(PrintType,SN,StationID) Values(" +
                          "'" + PrintType + "'," +
                          "'" + SN + "'," +
                          "'" + StationID + "'" +
                          ")";
            return SqlServerHelper.ExecSql(S_Sql);
        }

        public DataSet Get_WHExcel(string S_Start, string S_End, string FStatus)
        {
            string S_Status = "";
            if (FStatus != "999")
            {
                S_Status = " and FStatus=" + FStatus;
            }
            string S_Date = " FDate>='" + S_Start + "' and FDate<='" + S_End + "' ";

            string strSql = @"select A.FInterID,A.FBillNO, CONVERT(varchar(100),A.FDate,23) FDate ,A.HAWB,A.FPalletSeq,A.FPalletCount,A.FGrossweight,A.FEmptyCarton,
　　　　                    (case A.FStatus
　　　　	                    when 0     then    '暂未确认NoConfirm'
　　　　	                    when 1     then    '已确认栈板纸ConfirmedOK'
　　　　	                    when 2     then    '已扫描出库ShipScanOK'
　　　		                    when 3     then    '已确认出货Shipped'
			                    when 4     then    '已生成EDI'
			                    when 5     then    '已发送SAP_Recieved'
			                    when 6     then    '已关闭Closed'
						                    else '未定义' end
		                    ) FStatus,
		                    A.FShipNO,
		                    A.FCTN,
		                    A.FProjectNO,
		
		                    B.FDetailID,B.FMPNNO,B.FKPONO,B.FCTN FCTN_Detail,B.FProjectNO FProjectNO_Detail,B.FEntryID,B.FLineItem
                    from		
                    (select * from CO_WH_ShipmentNew where " + S_Date + S_Status + @") A 
                    join CO_WH_ShipmentEntryNew B on A.FInterID=B.FInterID";
            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }


        public DataSet GetRouteData(string LineID, string PartID, string PartFamilyID, string ProductionOrderID)
        {
            LineID = string.IsNullOrEmpty(LineID) ? "''" : LineID;
            PartID = string.IsNullOrEmpty(PartID) ? "''" : PartID;
            PartFamilyID = string.IsNullOrEmpty(PartFamilyID) ? "''" : PartFamilyID;
            ProductionOrderID = string.IsNullOrEmpty(ProductionOrderID) ? "''" : ProductionOrderID; 
            string S_Sql = string.Format("select ID,Name,Description,RouteType from mesRoute where ID= dbo.ufnRTEGetRouteID({0},{1},{2},{3})",
                LineID, PartID, PartFamilyID, ProductionOrderID);
            //DataTable dtRoute = SqlServerHelper.ExecuteDataTable(S_Sql);
            DataSet DS = SqlServerHelper.Data_Set(S_Sql);

            return DS;

        }

        public DataSet GetIpad_BB()
        {
            DataSet DS = new DataSet();
            string S_Sql =
                @"
                    select left(Value,2) as Name,RIGHT(Value,LEN(Value)-3) as BBDesc
                        from mesIpadBB
                    where AttributeName='BBCode'
                ";
            DataTable DT = SqlServerHelper.Data_Table(S_Sql);
            DT.TableName = "DT_BB";
            DS.Tables.Add(DT);
            return DS;
        }

        public string GetFirstStationType(string S_MachineSN)
        {
            string S_Sql = "SELECT * FROM mesMachine WHERE SN='P11-PC1-RO-00000048'";
            DataTable DT = SqlServerHelper.Data_Table(S_Sql);
            string S_Result = "";

            if (DT.Rows.Count > 0)
            {
                string S_ValidDistribution = DT.Rows[0]["ValidDistribution"].ToString();

                string[] List_ValidDistribution = S_ValidDistribution.Split(';');
                if (List_ValidDistribution.Length > 0)
                {
                    string[] List_StationType = List_ValidDistribution[0].Split(',');
                    S_Result = List_StationType[0];
                }
            }
            return S_Result;
        }

        public DataSet GetSNToUnit(string S_SN)
        {
            string S_Sql = "SELECT * FROM mesUnit  A JOIN mesSerialNumber B ON A.id=B.UnitID  WHERE B.[Value]='"+S_SN+"'";
            DataSet DS = SqlServerHelper.Data_Set(S_Sql);
            return DS;
        }

        public string uspUpdateBox(string PartID, string ProductionOrderID, string S_CartonSN, LoginList loginList, string boxWeight)
        {
            string outputStr = "0";
            DataSet ds = Get_PackageData(S_CartonSN);
            if (ds?.Tables[0]?.Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                try
                {
                    using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, new TimeSpan(0, 0, 30)))
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string tmpSN = dt.Rows[i]["SN"]?.ToString();
                            string tmpUPCSN = dt.Rows[i]["UPCSN"]?.ToString();
                            if (string.IsNullOrEmpty(tmpSN) || string.IsNullOrEmpty(tmpUPCSN))
                            {
                                return "20007";
                            }
                            outputStr = SetmesHistory(tmpSN, loginList, ProductionOrderID);
                            if (outputStr != "1")
                            {
                                return outputStr;
                            }
                            outputStr = SetmesHistory(tmpUPCSN, loginList, ProductionOrderID);
                            if (outputStr != "1")
                            {
                                return outputStr;
                            }
                        }
                        outputStr = SetmesPackBoxHistory(S_CartonSN, boxWeight, loginList);
                        if (outputStr != "1")
                        {
                            return "70023";
                        }
                        ts.Complete();
                    }
                }
                catch (System.Transactions.TransactionAbortedException ex)
                {
                    outputStr = "70006";
                }
            }
            return outputStr;
        }
        public string uspUpdateBoxV2(string PartID, string ProductionOrderID,  string S_CartonSN, LoginList loginList, string boxWeight)
        {
            int execNumber = 0;
            mesPartDAL mesPartDAL = new mesPartDAL();
            mesPart mesPartUnit = mesPartDAL.Get(Convert.ToInt32(PartID));
            DataSet dsUinitState = GetmesUnitState(PartID.ToString(), mesPartUnit.PartFamilyID.ToString(), "", loginList.LineID.ToString(), loginList.StationTypeID, ProductionOrderID.ToString(), "1");
            int UnitStateID = Convert.ToInt32(dsUinitState.Tables[0].Rows[0]["ID"].ToString());
            using (SqlConnection sqlConnection = SqlServerHelper.GetConnection())
            {
                sqlConnection.Open();
                SqlTransaction transaction = sqlConnection.BeginTransaction( IsolationLevel.ReadUncommitted);
                string sql = string.Empty;
                SqlCommand cmd = new SqlCommand(sql, sqlConnection, transaction);
                try
                {

                    #region 更新箱码绑定的FG/UPC状态，插入历史记录
                    sql = @"UPDATE d SET d.UnitStateID=@UnitStateID,StatusID=1,d.StationID=@StationID ,d.LineID=@LineID, d.EmployeeID=@EmployeeID,d.LastUpdate = GETDATE()
                            FROM mesPackage A
                            INNER JOIN mesUnitDetail B ON A.ID=B.InmostPackageID
                            INNER JOIN mesSerialNumber C ON B.UnitID=C.UnitID
                            INNER JOIN dbo.mesUnit d ON d.ID = B.UnitID
                            WHERE A.SerialNumber=@S_CartonSN";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(new SqlParameter[5]{ new SqlParameter("@S_CartonSN", S_CartonSN),
                                                                    new SqlParameter("@UnitStateID", UnitStateID),
                                                                    new SqlParameter("@StationID", loginList.StationID),
                                                                    new SqlParameter("@LineID", loginList.LineID),
                                                                    new SqlParameter("@EmployeeID", loginList.EmployeeID)});
                    cmd.CommandText = sql;
                    execNumber = cmd.ExecuteNonQuery();
                    if (execNumber <= 0)
                    {
                        throw new Exception("20126");
                    }

                    sql = @"UPDATE d SET d.UnitStateID=@UnitStateID,StatusID=1,d.StationID=@StationID ,d.LineID=@LineID, d.EmployeeID=@EmployeeID,d.LastUpdate = GETDATE()
                            FROM mesPackage A
                            INNER JOIN mesUnitDetail B ON A.ID = B.InmostPackageID
                            INNER JOIN mesSerialNumber C ON c.Value = b.KitSerialNumber
                            INNER JOIN dbo.mesUnit d ON d.ID = c.UnitID
                            WHERE A.SerialNumber = @S_CartonSN";
                    cmd.CommandText = sql;
                    execNumber = cmd.ExecuteNonQuery();
                    if (execNumber <= 0)
                    {
                        throw new Exception("20126");
                    }

                    sql = @"INSERT INTO dbo.mesHistory(UnitID, UnitStateID, EmployeeID, StationID, EnterTime, ExitTime, ProductionOrderID, PartID, LooperCount, StatusID)
                            SELECT b.UnitID,@UnitStateID,@EmployeeID,@StationID,GETDATE(),GETDATE(),@ProductionOrderID,@PartID,1,1
                            FROM mesPackage A
                            INNER JOIN mesUnitDetail B ON A.ID=B.InmostPackageID
                            INNER JOIN mesSerialNumber C ON B.UnitID=C.UnitID
                            INNER JOIN dbo.mesUnit d ON d.ID = B.UnitID
                            WHERE A.SerialNumber=@S_CartonSN";
                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(new SqlParameter[7]{ new SqlParameter("@S_CartonSN", S_CartonSN),
                                                                    new SqlParameter("@UnitStateID", UnitStateID),
                                                                    new SqlParameter("@StationID", loginList.StationID),
                                                                    new SqlParameter("@LineID", loginList.LineID),
                                                                    new SqlParameter("@EmployeeID", loginList.EmployeeID),
                                                                    new SqlParameter("@ProductionOrderID",ProductionOrderID),
                                                                    new SqlParameter("@PartID",PartID)});
                    execNumber = cmd.ExecuteNonQuery();
                    if (execNumber <= 0)
                    {
                        throw new Exception("20126");
                    }

                    sql = @"INSERT INTO dbo.mesHistory(UnitID, UnitStateID, EmployeeID, StationID, EnterTime, ExitTime, ProductionOrderID, PartID, LooperCount, StatusID)
                            SELECT C.UnitID,@UnitStateID,@EmployeeID,@StationID,GETDATE(),GETDATE(),@ProductionOrderID,@PartID,1,1
                            FROM mesPackage A
                            INNER JOIN mesUnitDetail B ON A.ID=B.InmostPackageID
                            INNER JOIN mesSerialNumber C ON c.Value = b.KitSerialNumber
                            INNER JOIN dbo.mesUnit d ON d.ID = c.UnitID
                            WHERE A.SerialNumber=@S_CartonSN";
                    cmd.CommandText = sql;
                    execNumber = cmd.ExecuteNonQuery();
                    if (execNumber <= 0)
                    {
                        throw new Exception("20126");
                    }
                    #endregion


                    #region 箱码插入历史记录
                    sql = @"INSERT INTO dbo.mesPackageDetail
                        (
                            PackageID,
                            PackageDetailDefID,
                            Content
                        )
                        SELECT a.ID AS PackageID,b.ID AS PackageDetailDefID,@boxWeight AS Content
					    FROM dbo.mesPackage a,dbo.luPackageDetailDef b
					    WHERE a.SerialNumber = @BoxSN AND 
					    b.Description = 'PackBoxWeight'";
                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(new SqlParameter[2]{
                        new SqlParameter("boxWeight",boxWeight),
                        new SqlParameter("BoxSN",S_CartonSN)});
                    execNumber = cmd.ExecuteNonQuery();
                    if (execNumber <= 0)
                    {
                        throw new Exception("20120");
                    }

                    sql = @"INSERT INTO	
                        dbo.mesPackageHistory
                        (
                            PackageID,
                            PackageStatusID,
                            StationID,
                            EmployeeID,
                            Time
                        )
                        SELECT a.ID,b.ID,@StationID AS StationID, @EmployeeID AS EmployeeID, GETDATE() AS Time
                        FROM dbo.mesPackage a,
	                        luPackageStatus b 
	                        WHERE a.SerialNumber = @BoxSN AND
	                        b.Description = 'ScalageWeight'";
                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(new SqlParameter[3]{
                            new SqlParameter("BoxSN",S_CartonSN),
                            new SqlParameter("StationID",loginList.StationID),
                            new SqlParameter("EmployeeID",loginList.EmployeeID)});
                    execNumber = cmd.ExecuteNonQuery();
                    if (execNumber <= 0)
                    {
                        throw new Exception("20120");
                    }
                    #endregion
                    transaction.Commit();
                    return "1";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ex.Message;
                }
            }
        }
        public string ShipMentScalesCommint(string PalletSn, LoginList loginList, string PalletWeight, string POID)
        {
            int execNumber = 0;
            SetupPackageScalesDesc();
            string tmpRes = CheckShipmentPalletState(PalletSn, POID);
            if (tmpRes != "1")
                return  string.IsNullOrEmpty(tmpRes)? "20244" : tmpRes;

            using (SqlConnection sqlConnection = SqlServerHelper.GetConnection())
            {
                sqlConnection.Open();
                SqlTransaction transaction = sqlConnection.BeginTransaction(IsolationLevel.ReadUncommitted);
                string sql = string.Empty;
                SqlCommand cmd = new SqlCommand(sql, sqlConnection, transaction);
                try
                {
                    #region 更新BilNo状态
                    sql = $@"UPDATE b SET b.FStatus = 3
                            FROM dbo.mesPackage a 
                            JOIN dbo.CO_WH_ShipmentNew b ON b.FInterID = a.ShipmentInterID
                            WHERE a.Stage = 3 
                            AND a.SerialNumber = @PalletSn 
                            AND b.FStatus = 2";
                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(new SqlParameter[1]{
                        new SqlParameter("PalletSn",PalletSn)});
                    execNumber = cmd.ExecuteNonQuery();
                    if (execNumber <= 0)
                    {
                        throw new Exception("20120");
                    }
                    #endregion

                    #region 箱码插入历史记录
                    sql = @"INSERT INTO dbo.mesPackageDetail
                        (
                            PackageID,
                            PackageDetailDefID,
                            Content
                        )
                        SELECT a.ID AS PackageID,b.ID AS PackageDetailDefID,@PalletWeight AS Content
					    FROM dbo.mesPackage a,dbo.luPackageDetailDef b
					    WHERE a.SerialNumber = @PalletSn AND 
					    b.Description = 'PackPalletWeight'";
                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(new SqlParameter[2]{
                        new SqlParameter("PalletWeight",PalletWeight),
                        new SqlParameter("PalletSn",PalletSn)});
                    execNumber = cmd.ExecuteNonQuery();
                    if (execNumber <= 0)
                    {
                        throw new Exception("20120");
                    }

                    sql = @"INSERT INTO	
                        dbo.mesPackageHistory
                        (
                            PackageID,
                            PackageStatusID,
                            StationID,
                            EmployeeID,
                            Time
                        )
                        SELECT a.ID,b.ID,@StationID AS StationID, @EmployeeID AS EmployeeID, GETDATE() AS Time
                        FROM dbo.mesPackage a,
	                        luPackageStatus b 
	                        WHERE a.SerialNumber = @PalletSn  AND a.Stage = 3 AND
	                        b.Description = 'WeighingCompleted'";
                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(new SqlParameter[3]{
                            new SqlParameter("PalletSn",PalletSn),
                            new SqlParameter("StationID",loginList.StationID),
                            new SqlParameter("EmployeeID",loginList.EmployeeID)});
                    execNumber = cmd.ExecuteNonQuery();
                    if (execNumber <= 0)
                    {
                        throw new Exception("20120");
                    }
                    #endregion
                    transaction.Commit();
                    return "1";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ex.Message;
                }
            }
        }

        public string CheckShipmentPalletState(string ShipPallet,string POID)
        {
            string sql = $@"DECLARE @strOutput        nvarchar(200) = '1', @shipmentRegion NVARCHAR(200),@PORegion NVARCHAR(200)
                        IF NOT EXISTS(SELECT 1 
                                                 FROM dbo.mesPackage a
                                                 JOIN dbo.CO_WH_ShipmentNew b ON b.FInterID = a.ShipmentInterID
                                                 WHERE
                                                 a.Stage = 3
                                                 AND a.SerialNumber = '{ShipPallet}'
                                                 AND b.FStatus = 2)
                        BEGIN
                            SET @strOutput = '20045'
	                        SELECT @strOutput
	                        RETURN
                        END

                        select @shipmentRegion=isnull(Region,'') from CO_WH_ShipmentNew s
                        JOIN dbo.mesPackage p ON p.ShipmentInterID = s.FInterID
                         where p.SerialNumber = '{ShipPallet}' AND p.Stage = 3
                        select @PORegion=isnull(content,'') from mesProductionOrderDetail pod where pod.ProductionOrderID='{POID}' and pod.ProductionOrderDetailDefID=(select id from luProductionOrderDetailDef where Description='ShipmentRegion')
                        IF @shipmentRegion <> @PORegion
                        BEGIN
                            SET @strOutput = '20050'
	                        SELECT @strOutput
	                        RETURN
                        END
                        SELECT @strOutput";


            var o = SqlServerHelper.ExecuteScalar(sql);



            return o?.ToString();
        }

        public void SetupPackageScalesDesc()
        {
            string sql = @"IF NOT EXISTS(SELECT * FROM dbo.luPackageDetailDef WHERE Description = 'PackPalletWeight')
                            BEGIN
                                INSERT INTO dbo.luPackageDetailDef(ID, Description)
                            VALUES((SELECT MAX(ID) + 1 FROM dbo.luPackageDetailDef), 
                            N'PackPalletWeight'
                                )
                            END
							IF NOT EXISTS(SELECT * FROM luPackageStatus WHERE Description = 'WeighingCompleted')
							BEGIN
							    INSERT INTO dbo.luPackageStatus(ID, Description)
							    VALUES((SELECT MAX(ID)+1 FROM luPackageStatus),
							    N'WeighingCompleted'
							        )
							END";

            SqlServerHelper.ExecuteNonQuery(sql);
        }

        public DataSet GetMesPartAndPartFamilyDetail(int PartId, string PartDetailDefName, string PartFamilyDetailDefName, out string result)
        {
            result = "0";
            try
            {
                if (string.IsNullOrEmpty(PartDetailDefName) && string.IsNullOrEmpty(PartFamilyDetailDefName))
                {
                    result = "70022";
                    return null;
                }

                PartFamilyDetailDefName = string.IsNullOrEmpty(PartFamilyDetailDefName) ? PartDetailDefName : PartFamilyDetailDefName;
                PartDetailDefName = string.IsNullOrEmpty(PartDetailDefName) ? PartFamilyDetailDefName : PartDetailDefName;
                
                DataTable configDataTable = SqlServerHelper.ExecuteDataTable(@"SELECT 'PartFamily'  item,  a.Content value
                                            FROM dbo.mesPartFamilyDetail a
                                            JOIN dbo.luPartFamilyDetailDef f ON f.ID = a.PartFamilyDetailDefID
                                            JOIN dbo.luPartFamily b ON a.PartFamilyID = b.ID
                                            JOIN dbo.mesPart c ON c.PartFamilyID = b.ID
                                            WHERE c.ID = @PartId AND
                                            f.Description = @PartFamilyDetailDefName
                                            UNION ALL
                                            SELECT 'Part' item, d.Content value
                                            FROM dbo.mesPart c
                                            FULL JOIN dbo.mesPartDetail d ON d.PartID = c.ID
                                            FULL JOIN dbo.luPartDetailDef e ON e.ID = d.PartDetailDefID
                                            WHERE
                                                e.Description = @PartDetailDefName AND
                                                c.ID = @PartId",
                                                    new SqlParameter[3]{new SqlParameter("PartId", PartId)
                                                ,new SqlParameter("PartDetailDefName",PartDetailDefName)
                                                ,new SqlParameter("PartFamilyDetailDefName",PartFamilyDetailDefName)
                                                    });

                DataRow[] tmpContext = configDataTable.Select("item = 'Part'");
                string context = string.Empty;
                if (tmpContext?.Length <= 0)
                {
                    tmpContext = configDataTable.Select("item = 'PartFamily'");
                    if (tmpContext?.Length <= 0)
                    {
                        result = "70021";
                        return null;
                    }
                }
                context = tmpContext[0]["value"].ToString();
                DataTable ParamDataTable = SqlServerHelper.ExecuteDataTable("SELECT * FROM dbo.Split(@context,@Delimiter)",
                    new SqlParameter("context", context), new SqlParameter("Delimiter", ";"));

                if (ParamDataTable?.Rows.Count < 1)
                {
                    result = "70022";
                    return null;
                }
                result = "1";
                DataSet dataSet = new DataSet();
                dataSet.Tables.Add(ParamDataTable);
                return dataSet;
            }
            catch (Exception ex)
            {
                result = "70022";
            }
            return null;
        }
        public string GetMesStationAndStationTypeDetail(int StationId, string StationDetailDefName, string StationTypeDetailDefName, out string result)
        {
            result = "0";
            try
            {
                if (string.IsNullOrEmpty(StationDetailDefName) && string.IsNullOrEmpty(StationTypeDetailDefName))
                {
                    result = "20199";
                    return null;
                }

                StationDetailDefName = string.IsNullOrEmpty(StationDetailDefName) ? StationTypeDetailDefName : StationDetailDefName;
                StationTypeDetailDefName = string.IsNullOrEmpty(StationTypeDetailDefName) ? StationDetailDefName : StationTypeDetailDefName;
                DataTable configDataTable = SqlServerHelper.ExecuteDataTable(@"SELECT 'StationType' AS item,a.Content AS value   FROM dbo.mesStationTypeDetail a
                    JOIN dbo.mesStationType b ON b.ID = a.StationTypeID
                    JOIN dbo.luStationTypeDetailDef c ON c.ID = a.StationTypeDetailDefID
                    JOIN dbo.mesStation d ON d.StationTypeID = b.ID
                    WHERE d.ID = @StationId AND c.Description = @StationTypeDetailDefName
                    UNION ALL
                    SELECT 'Station' AS item, a.Value AS value FROM dbo.mesStationConfigSetting a
                    JOIN dbo.mesStation b ON b.ID = a.StationID
                    WHERE b.ID = @StationId AND a.Name = @StationDetailDefName",
                                                    new SqlParameter[3]{new SqlParameter("StationId", StationId)
                                                ,new SqlParameter("StationTypeDetailDefName",StationTypeDetailDefName)
                                                ,new SqlParameter("StationDetailDefName",StationDetailDefName)
                                                    });
                if (configDataTable?.Rows.Count <= 0)
                {
                    result = "20199";
                    return null;
                }
                string context = configDataTable.Rows[configDataTable.Rows.Count-1]["value"].ToString();
                result = "1";
                return context;
            }
            catch (Exception ex)
            {
                result = "70022";
            }
            return null;
        }
        public string GetServerDateTime() => SqlServerHelper.ExecuteScalar("select getdate()")?.ToString();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="type">true  为站点类型，false 为站点</param>
        /// <param name="stationId">站点id或者站点类型ID</param>
        /// <returns></returns>
        public string GetSampleCount(string startTime,string endTime, bool type, int stationId)
        {
            try
            {
                string firstDay = DateTime.Parse(startTime).ToString("yyyy-MM-dd");
                string secendDay = DateTime.Parse(endTime).ToString("yyyy-MM-dd");
                string sql = string.Empty;
                if (type)
                {
                    sql = string.Format(@"SELECT COUNT(DISTINCT b.UnitID)
                            FROM dbo.mesUnitDetail a
                            JOIN dbo.mesHistory b ON a.UnitID = b.UnitID
                            JOIN dbo.mesStation c ON c.ID = b.StationID
                            WHERE a.reserved_04 IN ({3}) AND
                            b.EnterTime BETWEEN CAST('{0}' AS DATETIME)  AND CAST('{1}' AS DATETIME) AND 
                            c.StationTypeID in (SELECT StationTypeID FROM dbo.mesStation WHERE id = '{2}') AND
                            c.LineID IN (SELECT LineID FROM dbo.mesStation WHERE id= '{2}')", startTime, endTime,stationId, firstDay == secendDay ? $"'{firstDay}'" : $"'{firstDay}','{secendDay}'");
                }
                else
                {
                    sql = string.Format(@"SELECT COUNT(DISTINCT b.UnitID)
                            FROM dbo.mesUnitDetail a
                            JOIN dbo.mesHistory b ON a.UnitID = b.UnitID
                            JOIN dbo.mesStation c ON c.ID = b.StationID
                            WHERE a.reserved_04 IN ({3}) AND
                            b.EnterTime BETWEEN CAST('{0}' AS DATETIME)  AND CAST('{1}' AS DATETIME) AND
                            {2}", startTime, endTime, $"b.StationID = {stationId}", firstDay == secendDay ? $"'{firstDay}'" : $"'{firstDay}','{secendDay}'");

                }

                return SqlServerHelper.ExecuteScalar(sql)?.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="type">1:产品条码  2:箱码/栈板条码</param>
        /// <returns></returns>
        public string GetProductOrder(string barcode, int type)
        {
            string sql = string.Empty;
            if (type == 1)
            {
                sql = @"SELECT a.PartID FROM dbo.mesUnit a
                        JOIN dbo.mesSerialNumber b ON b.UnitID = a.ID
                        WHERE b.Value = '"+barcode+"'";
            }
            else if (type == 2)
            {
                sql = "SELECT CurrProductionOrderID FROM dbo.mesPackage WHERE SerialNumber = '"+barcode+"'";
            }
            else
            {
                return "";
            }
            return SqlServerHelper.ExecuteScalar(sql)?.ToString();
        }
        public DataSet GetProductDataInfo(string barcode ,int type)
        {
            DataSet dataSet = null;
            try
            {
                string sql = string.Empty;
                int tmptype = !new int[3] { 0, 1, 2 }.Contains(type)? GetBarcodeType(barcode):type;

                if (tmptype == 0)
                {
                    //查询FG或者治具条码 
                    sql = @"DECLARE @mFGBarcode VARCHAR(50),@IBarcode VARCHAR(50) = '"+ barcode + @"'
                    IF EXISTS(SELECT 1 FROM dbo.mesMachine WHERE SN = @IBarcode)
                    BEGIN
                        IF not EXISTS(SELECT 1 FROM dbo.mesUnitDetail WHERE reserved_01 = @IBarcode AND reserved_03 = '1')
                         BEGIN
                             PRINT '扫入的是治具条码，但是已经是解绑状态'
                          RETURN
                     END
                     SELECT @mFGBarcode = b.Value FROM dbo.mesUnitDetail a
                     JOIN dbo.mesSerialNumber b ON b.UnitID = a.UnitID
                     WHERE a.reserved_01 = @IBarcode AND a.reserved_03 = '1'
                    END
                    IF ISNULL(@mFGBarcode, '') = ''
                    BEGIN
                        SET @mFGBarcode = @IBarcode
                    END
                    SELECT a.*,b.Value FROM dbo.mesUnit a
                    JOIN dbo.mesSerialNumber b ON b.UnitID = a.ID
                    WHERE b.Value = @mFGBarcode
                    ";
                }
                else if(tmptype == 1)
                {
                    //查询箱码
                    sql = @"SELECT a.SerialNumber,c.UnitStateID,c.PartID,c.ProductionOrderID FROM dbo.mesPackage a
                            JOIN dbo.mesUnitDetail b ON a.ID = b.InmostPackageID
                            JOIN dbo.mesUnit c ON c.ID = b.UnitID
                            WHERE a.SerialNumber = '" + barcode + @"' AND a.Stage = 1
                            GROUP BY a.SerialNumber,c.UnitStateID,c.PartID,c.ProductionOrderID";
                }
                else if(tmptype == 2)
                {
                    //查询栈板条码
                    sql = @"SELECT  d.SerialNumber,a.UnitStateID, a.PartID,a.ProductionOrderID FROM dbo.mesUnit a
                            JOIN dbo.mesUnitDetail b ON b.UnitID = a.ID
                            JOIN dbo.mesPackage c ON c.ID = b.InmostPackageID
                            JOIN dbo.mesPackage d ON c.ParentID = d.ID
                            WHERE d.SerialNumber = '"+barcode+@"' AND d.Stage = '2'
                            GROUP BY d.SerialNumber,a.UnitStateID, a.PartID,a.ProductionOrderID";
                }
                else
                {
                    return dataSet;
                }
                dataSet = SqlServerHelper.Data_Set(sql);
            }
            catch (Exception)
            {   
                return null;
            }

            return dataSet;
        }
        /// <summary>
        /// 0  FG barcode
        /// 1  Carton barcode
        /// 2  Pallet barcode
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetBarcodeType(string barcode)
        {
            int tmpType = -1;
            try
            {
                string sql = string.Empty;
                sql = @"IF	EXISTS(SELECT 1 FROM dbo.mesUnit a
			                            JOIN dbo.mesSerialNumber b ON b.UnitID = a.ID
			                            WHERE b.Value = '" + barcode + @"' )
                            BEGIN
	                            SELECT 0 AS mType		    
	                            RETURN
                            END
                            IF	EXISTS(SELECT 1 FROM dbo.mesPackage WHERE SerialNumber = '" + barcode + @"' AND Stage = 1)
                            BEGIN
                                SELECT 1 AS mType
	                            RETURN
                            END
                            IF	EXISTS(SELECT 1 FROM dbo.mesPackage WHERE SerialNumber = '" + barcode + @"' AND Stage = 2)
                            BEGIN
                                SELECT 2 AS mType
	                            RETURN
                            END
                            SELECT -1 AS mType";
                object tmpRes = SqlServerHelper.ExecuteScalar(sql);
                if (!new string[3] { "0", "1", "2" }.Contains(tmpRes?.ToString()))
                    return tmpType;
                tmpType = int.Parse(tmpRes.ToString());
            }
            catch (Exception)
            {
                return tmpType;
            }
            return tmpType;
        }
        public DataSet GetAndCheckUnitInfo(string barcode, string POID,string PartID)
        {
            try
            {
                string sql = @"SELECT b.*,d.PartFamilyID
                            FROM dbo.mesSerialNumber a
                            JOIN dbo.mesUnit b ON b.ID = a.UnitID
                            JOIN dbo.mesProductionOrder c ON b.ProductionOrderID = c.ID AND b.PartID = c.PartID
                            JOIN dbo.mesPart d ON d.ID = b.PartID
                            WHERE a.Value = '" + barcode+"' AND b.PartID = '"+PartID+"' AND b.ProductionOrderID = '"+POID+"'";
                return SqlServerHelper.Data_Set(sql);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<string> SnToPOID(string SN)
        {
            List<string> List_Result = new List<string>();
            try
            {
                string S_Sql = @"select A.Value SN,B.ProductionOrderID,B.PartID
                                from mesSerialNumber A JOIN mesUnit B on A.UnitID=B.ID
                            where A.Value='" + SN + "'";
                DataTable DT = P_DataSet(S_Sql).Tables[0];
                if (DT.Rows.Count > 0)
                {
                    List_Result.Add(DT.Rows[0]["ProductionOrderID"].ToString());
                    List_Result.Add(DT.Rows[0]["PartID"].ToString());
                }
                else
                {
                    List_Result.Add("ERROR20012");
                }
            }
            catch (Exception ex)
            {
                List_Result.Clear();

                List_Result.Add("ERROR20013" + ex.Message);
                List_Result.Add("");
            }
            return List_Result;
        }


        public DataSet GetmesUnitTTBox(string S_SN)
        {
            DataSet DS_Result = new DataSet(); 
            try
            {
                string S_Sql = @"DECLARE @BoxUnitID INT
                DECLARE @ChildCount INT
                DECLARE @PartID INT
                DECLARE @FullNumber INT=-1

                select @BoxUnitID=UnitID FROM mesSerialNumber    WHERE [Value]='" + S_SN + @"' 


                SELECT @ChildCount=COUNT(*) FROM mesUnit WHERE PanelID=@BoxUnitID
                SELECT @PartID=PartID FROM mesUnit WHERE ID=@BoxUnitID

                select @FullNumber=a.[Content] from mesPartDetail a  WHERE  a.PartID=@PartID 
                and exists (select 1 from luPartDetailDef b where a.PartDetailDefID = b.ID and b.Description ='FullNumber')

                select A.*,B.Value SN, @ChildCount ChildCount,@FullNumber FullNumber,C.reserved_04 BoxSNStatus
                  FROM mesUnit A 
                    JOIN mesSerialNumber B ON A.ID=B.UnitID   
                    JOIN mesUnitDetail C ON A.ID=C.UnitID     
                WHERE B.[Value]='" + S_SN + @"' and C.reserved_02='" + S_SN + "' Order by A.ID";

                DS_Result = SqlServerHelper.Data_Set(S_Sql);
                if (DS_Result == null || DS_Result.Tables.Count == 0 || DS_Result.Tables[0].Rows.Count == 0)
                {
                    S_Sql = "select -1 ID, 0 ChildCount,10 FullNumber,1 BoxSNStatus";
                    DS_Result = SqlServerHelper.Data_Set(S_Sql);
                }

            }
            catch (Exception ex)
            {
                DataTable DT = new DataTable();
                DT.Columns.Add("ERROR");
                DataRow DR = DT.NewRow();
                DR[0] = ex.Message;

                DT.Rows.Add(DR);
            }

            return DS_Result;
        }


        public string GetAppSet(string S_SetName)
        {
            string S_Result = "";
            try
            {
                S_Result = ConfigurationManager.AppSettings[S_SetName];               
            }
            catch
            {
            }
            return S_Result;
        }

        public int InsertmesScreenshot(mesScreenshot model)
        {
            mesScreenshotDAL v_mesScreenshotDAL = new mesScreenshotDAL();
            int I_Result = v_mesScreenshotDAL.Insert(model);
            return I_Result;
        }

        public mesScreenshot GetmesScreenshot(int id)
        {
            mesScreenshotDAL v_mesScreenshotDAL = new mesScreenshotDAL();
            mesScreenshot v_mesScreenshot = v_mesScreenshotDAL.Get(id);
            return v_mesScreenshot;
        }

        public Boolean UpdateScreenshot(mesScreenshot model)
        {
            mesScreenshotDAL v_mesScreenshotDAL = new mesScreenshotDAL();
            Boolean B_Result  = v_mesScreenshotDAL.Update(model);
            return B_Result;
        }

        public DataSet ListmesScreenshot(string S_Where)
        {
            mesScreenshotDAL v_mesScreenshotDAL = new mesScreenshotDAL();
            DataTable v_ListmesScreenshot = v_mesScreenshotDAL.ListAll(S_Where);
            DataSet DS = new DataSet();
            DS.Tables.Add(v_ListmesScreenshot);
            return DS;
        }

        public string FGLinkRMACommit(string PartID, string ProductionOrderID, string NewFGUnitID, string OldFGUnitID, string NewToUnitStateID, string NewToStateID, string NewFromUnitStateID, string NewFromStateID, LoginList loginList,string[] defects = null)
        {
            int execNumber = 0;
            mesPartDAL mesPartDAL = new mesPartDAL();

            using (SqlConnection sqlConnection = SqlServerHelper.GetConnection())
            {
                sqlConnection.Open();
                SqlTransaction transaction = sqlConnection.BeginTransaction(IsolationLevel.ReadUncommitted);
                string sql = string.Empty;
                SqlCommand cmd = new SqlCommand(sql, sqlConnection, transaction);
                try
                {
                    #region //1/绑定新旧FGID 在reserved_07 
                    sql = @"UPDATE c SET c.reserved_07 = @OldFGUnitID
                            FROM dbo.mesSerialNumber a
                            JOIN dbo.mesUnit b ON b.ID = a.UnitID
                            JOIN dbo.mesUnitDetail c ON c.UnitID = b.ID
                            WHERE a.UnitID = @NewFGUnitID AND b.UnitStateID = @NewFromUnitStateID AND b.StatusID = @NewFromStateID ";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(new SqlParameter[7]{
                        new SqlParameter("@NewFGUnitID", NewFGUnitID),
                        new SqlParameter("@NewFromUnitStateID", NewFromUnitStateID),
                        new SqlParameter("@NewFromStateID", NewFromStateID),
                        new SqlParameter("@StationID", loginList.StationID),
                        new SqlParameter("@LineID", loginList.LineID),
                        new SqlParameter("@EmployeeID", loginList.EmployeeID),
                        new SqlParameter("@OldFGUnitID", OldFGUnitID)
                    });
                    cmd.CommandText = sql;
                    execNumber = cmd.ExecuteNonQuery();
                    if (execNumber <= 0)
                    {
                        throw new Exception("20126");
                    }


                    #endregion


                    #region 更新FG状态
                    //2/  
                    sql = @"UPDATE b SET b.UnitStateID = @NewToUnitStateID, b.StationID = @NewToStateID, b.LastUpdate = GETDATE(),b.StationID = @StationID,b.EmployeeID = @EmployeeID
                            FROM dbo.mesSerialNumber a
                            JOIN dbo.mesUnit b ON b.ID = a.UnitID
                            JOIN dbo.mesUnitDetail c ON c.UnitID = b.ID
                            WHERE a.UnitID = @NewFGUnitID AND b.UnitStateID = @NewFromUnitStateID AND b.StatusID = @NewFromStateID ";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(new SqlParameter[8]{
                        new SqlParameter("@NewFGUnitID", NewFGUnitID),
                        new SqlParameter("@NewFromUnitStateID", NewFromUnitStateID),
                        new SqlParameter("@NewFromStateID", NewFromStateID),
                        new SqlParameter("@StationID", loginList.StationID),
                        new SqlParameter("@LineID", loginList.LineID),
                        new SqlParameter("@EmployeeID", loginList.EmployeeID),
                        new SqlParameter("@NewToUnitStateID", NewToUnitStateID),
                        new SqlParameter("@NewToStateID", NewToStateID)
                    });
                    cmd.CommandText = sql;
                    execNumber = cmd.ExecuteNonQuery();
                    if (execNumber <= 0)
                    {
                        throw new Exception("20126");
                    }

                    #endregion



                    sql = @"INSERT INTO dbo.mesHistory(UnitID, UnitStateID, EmployeeID, StationID, EnterTime, ExitTime, ProductionOrderID, PartID, LooperCount, StatusID)
                        VALUES(@NewFGUnitID, -- UnitID - int
                        @NewToUnitStateID   , -- UnitStateID - int
                        @EmployeeID   , -- EmployeeID - int
                        @StationID   , -- StationID - int
                        GETDATE(), -- EnterTime - datetime
                        GETDATE(), -- ExitTime - datetime
                        @ProductionOrderID   , -- ProductionOrderID - int
                        @PartID   , -- PartID - int
                        1   , -- LooperCount - int
                        @NewToStateID   -- StatusID - int
                            )";
                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(new SqlParameter[10]{
                        new SqlParameter("@NewFGUnitID", NewFGUnitID),
                        new SqlParameter("@NewFromUnitStateID", NewFromUnitStateID),
                        new SqlParameter("@NewFromStateID", NewFromStateID),
                        new SqlParameter("@StationID", loginList.StationID),
                        new SqlParameter("@LineID", loginList.LineID),
                        new SqlParameter("@EmployeeID", loginList.EmployeeID),
                        new SqlParameter("@NewToUnitStateID", NewToUnitStateID),
                        new SqlParameter("@PartID", PartID),
                        new SqlParameter("@ProductionOrderID", ProductionOrderID),
                        new SqlParameter("@NewToStateID", NewToStateID)
                    });
                    execNumber = cmd.ExecuteNonQuery();
                    if (execNumber <= 0)
                    {
                        throw new Exception("20126");
                    }

                    if (defects != null && defects.Length > 0)
                    {
                        for (int i = 0; i < defects.Length; i++)
                        {
                            sql = @"INSERT INTO dbo.mesUnitDefect(ID, UnitID, DefectID, StationID, EmployeeID, CreationTime)
	                            VALUES((SELECT MAX(ID)+1 FROM dbo.mesUnitDefect), -- ID - int
	                            @NewFGUnitID   , -- UnitID - int
	                            @DefectID   , -- DefectID - int
	                            @StationID   , -- StationID - int
	                            @EmployeeID   , -- EmployeeID - int
	                            GETDATE() -- CreationTime - datetime
	                                )";
                            cmd.CommandText = sql;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddRange(new SqlParameter[11]{
                                new SqlParameter("@NewFGUnitID", NewFGUnitID),
                                new SqlParameter("@NewFromUnitStateID", NewFromUnitStateID),
                                new SqlParameter("@NewFromStateID", NewFromStateID),
                                new SqlParameter("@StationID", loginList.StationID),
                                new SqlParameter("@LineID", loginList.LineID),
                                new SqlParameter("@EmployeeID", loginList.EmployeeID),
                                new SqlParameter("@NewToUnitStateID", NewToUnitStateID),
                                new SqlParameter("@DefectID", defects[i]),
                                new SqlParameter("@PartID", PartID),
                                new SqlParameter("@ProductionOrderID", ProductionOrderID),
                                new SqlParameter("@NewToStateID", NewToStateID)
                            });
                            execNumber = cmd.ExecuteNonQuery();
                            if (execNumber <= 0)
                            {
                                throw new Exception("20126");
                            }
                        }
                    }


                    transaction.Commit();
                    return "1";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ex.Message;
                }
            }
        }
        public string[] GetPartIdByShippingPallet(string ShippingPalletSN)
        {
            try
            {
                string sql = $@" SELECT TOP 1 e.PartID,e.ProductionOrderID FROM dbo.mesUnit e 
						JOIN dbo.mesUnitDetail f ON f.UnitID = e.ID
						JOIN dbo.mesPackage g ON g.ID = f.InmostPackageID AND g.Stage = 1
						JOIN dbo.mesPackage p ON p.ID = g.ShipmentParentID AND p.Stage = 3
						WHERE p.SerialNumber = '{ShippingPalletSN}'";

                var table = SqlServerHelper.Data_Table(sql);
                if (table == null || table.Rows.Count <= 0)
                    return null;
                return new []{ table.Rows[0]["PartID"].ToString(), table.Rows[0]["ProductionOrderID"].ToString() };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string SaveTTOutputStationType(string SN, int StationTypeID)
        {
            string S_Result = "OK";
            string S_Sql = $@"UPDATE b SET b.reserved_09 = '{StationTypeID}'
                            FROM dbo.mesSerialNumber a
                            JOIN dbo.mesUnitDetail b ON b.UnitID = a.UnitID
                            WHERE a.Value = '{SN}'";

            S_Result = SqlServerHelper.ExecSql(S_Sql);
            return S_Result;
        }
        public StationTypeShow[] GetTTOutputStationTypeList(int routeID, int stationTypeID)
        {
            string S_Result = "OK";
            string S_Sql = $@"SELECT b.ID,b.Description StationTypeName FROM dbo.mesUnitOutputState  a
            JOIN dbo.mesStationType b ON b.ID = a.CurrStateID
            WHERE a.RouteID = {routeID} AND a.StationTypeID = {stationTypeID}";
            var stationTypeTab = SqlServerHelper.ExecuteDataTable(S_Sql);
            var stationTypeList = DataTableToList<StationTypeShow>(stationTypeTab);
            return stationTypeList.ToArray();
        }
    }
}
