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
    class WH
    {
        PartSelectDAL v_PartSelect = new PartSelectDAL();


        public string SetCancelInWH(string S_BoxSN,string S_ProdID,string S_PartID,string S_StationID,string S_EmployeeID)
        {
            string S_Result = "";
            try
            {
               string S_Sql =
                    @"
	            declare	
			            @prodID				int,
			            @partID				int,		
			            @stationID			int,
                        @Set_StationID      int,  
			            @stationTypeID		int,						
			
			            @PackageID			INT,
			            @EmployeeId         INT,
			
			            @AllowCancelStationTypeID INT,
			            @CancelToUnitStateID      INT,
			            @CancelToStationID        INT,
			
                        @CancelUnitStateID        int, 

			            @Result    NVARCHAR(100)

                select  @prodID='" + S_ProdID + @"'			
                select  @partID='" + S_PartID + @"'	
                select  @Set_StationID='" + S_StationID + @"'	
                select  @EmployeeId='" + S_EmployeeID + @"'	

	            SELECT @Result='OK'													
	            SELECT @PackageID=ID,@stationID=StationID FROM mesPackage WHERE SerialNumber='" + S_BoxSN + @"' AND Stage='1'	
	            SELECT @stationTypeID=StationTypeID FROM mesStation WHERE ID=@stationID
	
	            IF not EXISTS (SELECT * FROM mesStationConfigSetting WHERE StationID=@stationID AND NAME='CancelToUnitStateID')
	            BEGIN
		            set @Result='ERROR:station parameter ''CancelToUnitStateID'' does not exist '
	            END
	
	            IF @Result='OK' 
	            BEGIN
		            IF not EXISTS (SELECT * FROM mesStationConfigSetting WHERE StationID=@stationID AND NAME='CancelToStationID')
		            BEGIN
			            set  @Result='ERROR:station parameter ''CancelToStationID'' does not exist '
		            END		
	            END
	            IF @Result='OK' 
	            BEGIN
		            IF not EXISTS (SELECT * FROM mesStationConfigSetting WHERE StationID=@stationID AND NAME='AllowCancelStationTypeID')
		            BEGIN
			            set @Result='ERROR:station parameter ''AllowCancelStationTypeID'' does not exist '
		            END			
	            END
	            IF @Result='OK' 
	            BEGIN
		            IF not EXISTS (SELECT * FROM mesStationConfigSetting WHERE StationID=@stationID AND NAME='CancelUnitStateID')
		            BEGIN
			            set @Result='ERROR:station parameter ''CancelUnitStateID'' does not exist '
		            END			
	            END

	            IF @Result='OK' 
	            begin				
		            SELECT @CancelToUnitStateID=[Value] FROM mesStationConfigSetting WHERE StationID=@stationID AND NAME='CancelToUnitStateID'
		            SELECT @CancelToStationID=[Value] FROM mesStationConfigSetting WHERE StationID=@stationID AND NAME='CancelToStationID'
		            SELECT @AllowCancelStationTypeID=[Value] FROM mesStationConfigSetting WHERE StationID=@stationID AND NAME='AllowCancelStationTypeID'
                    SELECT @CancelUnitStateID=[Value] FROM mesStationConfigSetting WHERE StationID=@stationID AND NAME='CancelUnitStateID'
                END
	
	            IF @AllowCancelStationTypeID=@stationTypeID
	            BEGIN
			            UPDATE mesPackage SET StationID=@CancelToStationID,LastUpdate=GETDATE(),EmployeeID=@EmployeeId WHERE ID=@PackageID

			            INSERT INTO mesPackageHistory(PackageID,PackageStatusID,StationID,EmployeeID,Time)
			            VALUES (@PackageID,1,@StationID,@EmployeeId,GETDATE())	
		
			            --修改mesUnit
			            UPDATE D SET D.StationID=@CancelToStationID,UnitStateID=@CancelToUnitStateID,EmployeeID=@EmployeeId FROM mesUnit D
			            INNER JOIN mesUnitDetail C ON C.UnitID=D.ID  
			            INNER JOIN mesPackage A ON ISNULL(C.InmostPackageID,'')=A.ID
			            WHERE A.ID=@PackageID

			            --mesHistory记录
			            INSERT INTO mesHistory(UnitID,UnitStateID,EmployeeID,StationID,EnterTime,ExitTime,ProductionOrderID,PartID,LooperCount,StatusID)				
			            SELECT D.ID,@CancelUnitStateID,@EmployeeId,@StationID,GETDATE(),GETDATE(),@prodID,@partID,1,1 FROM mesPackage A  
			            INNER JOIN mesUnitDetail C ON ISNULL(C.InmostPackageID,'')=A.ID
			            INNER JOIN mesUnit D ON C.UnitID=D.ID WHERE A.ID=@PackageID		
	            END
	            ELSE
	            BEGIN
		             set @Result='ERROR:Current status is not allowed to cancel'	
	            END
	            SELECT @Result as SqlResult
                ";
                S_Result = SqlServerHelper.ExecSqlDataRead(S_Sql);
            }
            catch (Exception ex)
            {
                S_Result = "ERROR:" + ex.ToString();
            }

            return S_Result;

        }
        public string SetCancelInWHEntry(string S_BoxSN, string S_ProdID, string S_PartID, string S_StationID, string S_EmployeeID,string S_ReturnToStationTypeID, string S_ReturnStatus)
        {
            string S_Result = "";
            try
            {
                string S_Sql =
                @"
                DECLARE	 @IBoxSN VARCHAR(100) = '" + S_BoxSN + @"',
		        @IProdID			INT='" + S_ProdID + @"',
		        @partID				INT = '" + S_PartID + @"',		
		        @IStationID			INT = '" + S_StationID + @"',
		        @IEmployeeId         INT = '" + S_EmployeeID + @"',
                @IReturnToStationTypeID VARCHAR(10) = '" + S_ReturnToStationTypeID +@"',
                @IStatusId VARCHAR(10) = '"+S_ReturnStatus+ @"',

		        @stationTypeID		int,
		        @mStationId INT,						
		        @PackageID			INT,
		        @CancelToStationID  INT,

		        @Result    NVARCHAR(100)


                
                declare @CurrStateID int,@ProductionOrderID int,@unitId int,@StationID_Pre INT,@LineID INT,@routeID INT,@UnitStateID INT

                --SELECT TOP 10 * FROM dbo.mesHistory WHERE StationID = 319
                SELECT @Result='OK'													
                SELECT @PackageID=ID,@mStationId=StationID FROM mesPackage WHERE SerialNumber=@IBoxSN AND Stage='1'	
                SELECT @stationTypeID=StationTypeID FROM mesStation WHERE ID=@mStationId

                if ISNULL(@IReturnToStationTypeID,'') <> ''
                begin
	                SET @Result='ERROR:no setup current station to return.'
					SELECT @Result as SqlResult
	                RETURN
                end                

                CREATE TABLE #TempRoute
                (	
	                ID				int,
	                UnitStateID		int
                )
                --20220524 howard
                select u.UnitStateID,u.StationID, ROW_NUMBER() OVER(ORDER BY u.UnitStateID,u.StationID) AS rowIndex into #tmpStatesTable
                from mesUnitDetail ud
                join mesUnit u on ud.UnitID=u.ID
                join mesPackage p on ud.InmostPackageID=p.id 
				WHERE  p.SerialNumber = @IBoxSN AND p.Stage=1
				GROUP BY u.UnitStateID,u.StationID

				IF (SELECT COUNT(1) FROM #tmpStatesTable) > 1
				BEGIN
				    SET @Result='ERROR: FG status more than 1.'
					SELECT @Result as SqlResult
	                RETURN
				END


                select  top 1 @unitId=ud.UnitID,@CurrStateID=UnitStateID,@PartID=u.PartID,@LineID=u.LineID,@ProductionOrderID=u.ProductionOrderID,@StationID_Pre=u.StationID 
                from mesUnitDetail ud
                join mesUnit u on ud.UnitID=u.ID
                join mesPackage p on ud.InmostPackageID=p.id where  p.SerialNumber = @IBoxSN AND p.Stage=1

                IF ISNULL(@unitId, 0) = 0 OR ISNULL(@LineID, 0) = 0 OR ISNULL(@ProductionOrderID,0) = 0 OR @StationID_Pre <> @IStationID
                BEGIN
                    SET @Result='ERROR:current station no match.'
					SELECT @Result as SqlResult
	                RETURN
                END
		
                SET @routeID= DBO.ufnRTEGetRouteID(@lineID,@partID,'',@ProductionOrderID)
                IF ISNULL(@routeID,'')=''
                BEGIN
	                SET @Result='ERROR:can t find route ID.'
					SELECT @Result as SqlResult
	                RETURN
                END

                --根据route类型获取上一站UnitstateID
                IF EXISTS(SELECT 1 FROM mesRoute WHERE ID=@routeID AND RouteType=1)
                BEGIN
	                IF(SELECT COUNT(1) FROM mesUnitInputState A 
	                INNER JOIN mesStation B ON A.StationTypeID=B.StationTypeID AND B.ID=@StationID_Pre
	                WHERE RouteID=@routeID AND NOT EXISTS(SELECT 1 FROM dbo.V_StationTypeInfo 
		                WHERE StationTypeID = b.StationTypeID AND DetailDef = 'StationTypeType' AND ISNULL(Content,'') = 'TT')
		                )>1
	                BEGIN
		                SET @Result='ERROR:The current station type is not configured in the configured process flow. Please check the process flow.'
						SELECT @Result as SqlResult
		                RETURN			    
	                END

	                SELECT @UnitStateID = CurrStateID FROM mesUnitInputState A 
		                INNER JOIN mesStation B ON A.StationTypeID=B.StationTypeID AND B.ID=@StationID_Pre
	                WHERE RouteID=@routeID
                END
                ELSE
                BEGIN
	                INSERT INTO #TempRoute(ID,UnitStateID)
	                SELECT ROW_NUMBER() OVER(ORDER BY Sequence) AS ID,UnitStateID
	                FROM mesRouteDetail where RouteID=@routeID

	                SELECT @UnitStateID=A.UnitStateID FROM #TempRoute A WHERE EXISTS 
		                (SELECT 1 FROM #TempRoute B WHERE B.UnitStateID=@CurrStateID AND A.ID=B.ID-1)
                END

                IF ISNULL(@UnitStateID,'')=''
                BEGIN
	                SET @Result='ERROR:The part number is not configured with the unit state of the process flow.'
					SELECT @Result as SqlResult
	                RETURN 
                END

                IF not EXISTS(SELECT 1 FROM dbo.mesHistory WHERE UnitID = @unitId AND UnitStateID = @UnitStateID)
                BEGIN
                    SET @Result = 'ERROR:no exist to return station id.'
					SELECT @Result as SqlResult
	                RETURN
                END
                SELECT TOP 1 @CancelToStationID = StationID FROM dbo.mesHistory 
                WHERE UnitID = @unitId AND UnitStateID = @UnitStateID
                ORDER BY ID DESC

                BEGIN TRY
	                BEGIN TRAN
	                UPDATE mesPackage SET StationID=@CancelToStationID,LastUpdate=GETDATE(),EmployeeID=@IEmployeeId WHERE ID=@PackageID

	                INSERT INTO mesPackageHistory(PackageID,PackageStatusID,StationID,EmployeeID,Time)
	                VALUES (@PackageID,9,@IStationID,@IEmployeeId,GETDATE())	
		
	                --修改mesUnit
	                UPDATE D SET D.StationID=@CancelToStationID,UnitStateID=@UnitStateID,EmployeeID=@IEmployeeId,
	                D.StatusID = CASE WHEN(ISNULL(@IStatusId,'') = '') THEN 1 ELSE CAST(@IStatusId AS INT) END FROM mesUnit D
	                INNER JOIN mesUnitDetail C ON C.UnitID=D.ID  
	                INNER JOIN mesPackage A ON ISNULL(C.InmostPackageID,'')=A.ID
	                WHERE A.ID=@PackageID

	                --mesHistory记录
	                INSERT INTO mesHistory(UnitID,UnitStateID,EmployeeID,StationID,EnterTime,ExitTime,ProductionOrderID,PartID,LooperCount,StatusID)				
	                SELECT D.ID,@UnitStateID,@IEmployeeId,@IStationID,GETDATE(),GETDATE(),@IProdID,@partID,1,CASE WHEN(ISNULL(@IStatusId,'') = '') THEN 1 ELSE CAST(@IStatusId AS INT) END FROM mesPackage A  
	                INNER JOIN mesUnitDetail C ON ISNULL(C.InmostPackageID,'')=A.ID
	                INNER JOIN mesUnit D ON C.UnitID=D.ID WHERE A.ID=@PackageID	
	                COMMIT TRAN
					set @Result = 'OK'
                END TRY
                BEGIN CATCH
	                IF @@ROWCOUNT > 0
	                BEGIN
	                    ROLLBACK TRAN
	                END
	                SET @Result = ERROR_MESSAGE()
                END CATCH	

                SELECT @Result as SqlResult
                ";
                S_Result = SqlServerHelper.ExecSqlDataRead(S_Sql);
            }
            catch (Exception ex)
            {
                S_Result = "ERROR:" + ex.ToString();
            }

            return S_Result;

        }
    }
}
