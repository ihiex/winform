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
    public class ShipmentPrintOne
    {
        PartSelectDAL v_PartSelect = new PartSelectDAL();

        /// <summary>
        /// 栈板包装
        /// </summary>
        /// <param name="PartID"></param>
        /// <param name="ProductionOrderID"></param>
        /// <param name="S_CartonSN"></param>
        /// <param name="S_PalletSN"></param>
        /// <param name="loginList"></param>
        /// <param name="PalletQty"></param>
        /// <returns></returns>
        public string uspPalletPackaging(string PartID, string ProductionOrderID, string S_CartonSN,
            string S_PalletSN, LoginList loginList, int PalletQty = 0)
        {
            string S_Result = "";
            try
            {
                try
                {
                    string S_Sql = "";
                    string strSql = @"SELECT ID FROM mesPackage WHERE SerialNumber=@S_PalletSN AND StatusID=0";

                    DataTable dtMp = SqlServerHelper.ExecuteDataTable(strSql, new SqlParameter("@S_PalletSN", S_PalletSN));
                    if (dtMp == null || dtMp.Rows.Count == 0)
                    {
                        return "20119";
                    }
                    string PalletID = dtMp.Rows[0]["ID"].ToString();

                    if (PalletQty > 0)
                    {
                        S_Sql += "INSERT INTO mesPackageHistory(PackageID, PackageStatusID, StationID, EmployeeID, Time) Values(" + "\r\n" +
                               "'" + PalletID + "'" + "\r\n" +
                               ",'2'" + "\r\n" +
                               ",'" + loginList.StationID + "'" + "\r\n" +
                               ",'" + loginList.EmployeeID + "'" + "\r\n" +
                               ", GETDATE()" + "\r\n" +
                               ")" + "\r\n" +

                               "UPDATE mesPackage SET CurrentCount='" + PalletQty + "'" + "\r\n" +
                               ",StationID='" + loginList.StationID + "',EmployeeID='" + loginList.EmployeeID + "',StatusID=1" + "\r\n" +
                               ",LastUpdate=GETDATE() where ID='" + PalletID + "'" + "\r\n";
                    }
                    else
                    {
                        S_Sql += "DECLARE @Stage int " + "\r\n" +
                                " SELECT @Stage=Stage FROM mesPackage WHERE ID='" + PalletID + "'" + "\r\n" +
                                " if @Stage<>3 " + "\r\n" +   //关联包装信息
                                " Begin " + "\r\n" +
                                "    UPDATE A SET A.ParentID = '" + PalletID + "'  FROM mesPackage A WHERE A.SerialNumber = '" +
                                       S_CartonSN + "'" + "\r\n" +
                                " End else  Begin" + "\r\n" +   //关联包装信息
                                "     UPDATE A SET A.ShipmentParentID='" + PalletID + "'  FROM mesPackage A WHERE A.SerialNumber='" +
                                       S_CartonSN + "'" + "\r\n" +

                               "      DECLARE @BoxID int " + "\r\n" + //记录Shipment履历
                               "      SELECT top 1 @BoxID=ID FROM mesPackage WHERE SerialNumber='" + S_CartonSN + "'" + "\r\n" +
                               "       " + "\r\n" +
                               "     INSERT INTO mesPackageHistory(PackageID, PackageStatusID, StationID, EmployeeID, Time)" + "\r\n" +
                               "     VALUES(@BoxID, 8, '" + loginList.StationID + "', '" + loginList.EmployeeID + "', GETDATE())" + "\r\n" +

                               "   End" + "\r\n"
                                ;

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
                            if (strResult.Substring(0, 5) == "ERROR")
                            {
                                string ss = strResult.Substring(5, strResult.Length - 1);
                                return ss;
                            }
                            else
                            {
                                S_Sql += strResult;
                            }


                            if (!string.IsNullOrEmpty(UPCSN) && UPCSN != FGSN)
                            {
                                strResult = SetmesHistory(UPCSN, loginList, ProductionOrderID);
                                if (strResult.Substring(0, 5) == "ERROR")
                                {
                                    string ss = strResult.Substring(5, strResult.Length - 1);
                                    return ss;
                                }
                                else
                                {
                                    S_Sql += strResult;
                                }
                            }
                        }
                    }

                    S_Result = SqlServerHelper.ExecSql(S_Sql);
                }
                catch (Exception ex)
                {
                    S_Result = "ERROR:" + ex.ToString();
                }
            }
            catch (Exception ex)
            {
                S_Result = "ERROR:" + ex.ToString();
            }

            return S_Result;
        }

        /// <summary>
        /// 包装过站记录
        /// </summary>
        /// <param name="S_SN"></param>
        /// <param name="loginList"></param>
        /// <returns></returns>
        public string SetmesHistory(string S_SN, LoginList loginList, string ProductionOrderID)
        {
            string S_Sql = "";
            try
            {
                string strSql = string.Empty;

                strSql = @"SELECT B.ID,B.PartID,B.ProductionOrderID FROM mesSerialNumber A 
                            INNER JOIN mesUnit B ON A.UnitID=B.ID WHERE A.Value=@S_SN";
                DataTable dtUnit = SqlServerHelper.ExecuteDataTable(strSql, new SqlParameter("@S_SN", S_SN));
                if (dtUnit == null || dtUnit.Rows.Count == 0)
                {
                    return "ERROR20127";
                }
                int UnitID = Convert.ToInt32(dtUnit.Rows[0]["ID"].ToString());
                int PartID = Convert.ToInt32(dtUnit.Rows[0]["PartID"].ToString());

                PartSelectDAL PartSelect = new PartSelectDAL();
                mesHistory v_mesHistory = new mesHistory();
                mesPartDAL v_mesPartDAL = new mesPartDAL();
                mesPart mesPartUnit = v_mesPartDAL.Get(Convert.ToInt32(PartID));
                DataSet dsUinitState = PartSelect.GetmesUnitState(PartID.ToString(), mesPartUnit.PartFamilyID.ToString(), "",
                    loginList.LineID.ToString(), loginList.StationTypeID, ProductionOrderID.ToString(), "1");
                int UnitStateID = Convert.ToInt32(dsUinitState.Tables[0].Rows[0]["ID"].ToString());


                S_Sql += "UPDATE mesUnit SET UnitStateID = '" + UnitStateID + "',StatusID = 1,EmployeeID = '" + loginList.EmployeeID +
                                "',LineID = '" + loginList.LineID + "'," + "\r\n" +
                        "   StationID = '" + loginList.StationID + "',LastUpdate = GETDATE() WHERE ID = '" + UnitID + "'" + "\r\n" +

                        "Insert into mesHistory(UnitID,UnitStateID,EmployeeID,StationID,EnterTime,ExitTime,ProductionOrderID,PartID,LooperCount ) " + "\r\n" +
                        "Values('" + UnitID + "'" + "\r\n" +
                        ",'" + UnitStateID + "'" + "\r\n" +

                        ",'" + loginList.EmployeeID + "'" + "\r\n" +
                        ",'" + loginList.StationID + "'" + "\r\n" +
                        ",GetDate()" + "\r\n" +
                        ",GetDate()" + "\r\n" +
                        ",'" + ProductionOrderID + "'" + "\r\n" +
                        ",'" + PartID + "'" + "\r\n" +
                        ",'1'" + "\r\n" +
                        ")" + "\r\n";

                return S_Sql;
            }
            catch (Exception ex)
            {
                return "ERROR" + ex.Message.ToString();
            }
        }


        public string MoveShipment(string S_BillNo, string S_MultipackSN, string S_ProdOrderID, string S_PartID, string S_StationId, string S_EmployeeId)
        {
            string S_Result = "";
            try
            {
                string xmlProdOrder = ("<ProdOrder ProdOrderID=\"" + "" + "\"> </ProdOrder>");
                string xmlPart = "<Part PartID=\"" + "" + "\"> </Part>";
                string xmlExtraData = "<ExtraData EmployeeId=\"" + S_EmployeeId + "\"> </ExtraData>";
                string xmlStation = "<Station StationId=\"" + S_StationId + "\"> </Station>";

                //包装移除
                v_PartSelect.uspCallProcedure("uspMoveShipmentMultipack", S_MultipackSN, xmlProdOrder, xmlPart,
                                                            xmlStation, xmlExtraData, S_BillNo, ref S_Result);
            }
            catch (Exception ex)
            {
                S_Result = "ERROR:" + ex.ToString();
            }
            return S_Result;
        }

        public DataSet SetShipmentMultipack(string S_BillNo, string S_MultipackSN)
        {
            string S_Sql =
            @"
		    DECLARE @CurrProductionOrderID	int,
				    @ShipmentDetailID		int,
				    @Stage					INT,
				
				    @FBillNO                NVARCHAR(200),  
				    @SerialNumber           NVARCHAR(200),
				    @strOutput              NVARCHAR(200)";

            S_Sql += "Set @FBillNO='" + S_BillNo + "'" + "\r\n" +
                    "Set @SerialNumber='" + S_MultipackSN + "'" + "\r\n";

            S_Sql += @"
            SET @strOutput = 1

		    IF EXISTS(SELECT 1 FROM mesPackage WHERE SerialNumber = @SerialNumber  AND ISNULL(ShipmentDetailID,-1)<>-1)
		    BEGIN
			    SET @strOutput='20183'
			    SELECT @ShipmentDetailID AS FDetailID,@strOutput AS OutResult
			    RETURN
		    END

		    SELECT @CurrProductionOrderID=CurrProductionOrderID,@Stage=Stage FROM mesPackage WHERE SerialNumber = @SerialNumber 
		    IF ISNULL(@CurrProductionOrderID,-1)=-1
		    BEGIN
			    SET @strOutput='20007'
			    SELECT @ShipmentDetailID AS FDetailID,@strOutput AS OutResult
			    RETURN
		    END

		    IF @Stage<>1
		    BEGIN
			    SET @strOutput='20176'
			    SELECT @ShipmentDetailID AS FDetailID,@strOutput AS OutResult
			    RETURN
		    END

		    SELECT @ShipmentDetailID=A.FDetailID FROM CO_WH_ShipmentEntryNew A
			    INNER JOIN CO_WH_ShipmentNew B ON A.FInterID=B.FInterID WHERE B.FBillNO=@FBillNO AND EXISTS(
			    SELECT * FROM (SELECT A.Content FROM mesProductionOrderDetail A
			    INNER JOIN luProductionOrderDetailDef B ON A.ProductionOrderDetailDefID=B.ID
			    WHERE B.Description='MPN' AND A.ProductionOrderID=@CurrProductionOrderID) WS WHERE WS.Content=A.FMPNNO AND A.FStatus in (0,1))
		    IF ISNULL(@ShipmentDetailID,-1)=-1
		    BEGIN
			    SET @strOutput='20080'
			    SELECT @ShipmentDetailID AS FDetailID,@strOutput AS OutResult
			    RETURN
		    END

		    IF EXISTS(SELECT 1 FROM CO_WH_ShipmentEntryNew WHERE FDetailID=@ShipmentDetailID AND FStatus not in (0,1))
		    BEGIN
			    SET @strOutput='20045'
			    SELECT @ShipmentDetailID AS FDetailID,@strOutput AS OutResult
			    RETURN
		    END

		    IF EXISTS(SELECT 1 FROM CO_WH_ShipmentEntryNew WHERE FDetailID=@ShipmentDetailID AND FOutSN>=FCTN)
		    BEGIN
			    SET @strOutput='20086'
			    SELECT @ShipmentDetailID AS FDetailID,@strOutput AS OutResult
			    RETURN
		    END

		    IF @strOutput='1'
		    BEGIN
			    UPDATE A SET A.ShipmentDetailID =@ShipmentDetailID,A.ShipmentTime=GETDATE(),
				    ShipmentInterID=(SELECT TOP 1 FInterID FROM CO_WH_ShipmentEntryNew WHERE FDetailID=@ShipmentDetailID) 
			    FROM mesPackage A WHERE SerialNumber = @SerialNumber
			    UPDATE CO_WH_ShipmentEntryNew SET FOutSN=ISNULL(FOutSN,0)+1 WHERE FDetailID=@ShipmentDetailID and FStatus in (0,1)
			    IF  EXISTS(SELECT 1 FROM CO_WH_ShipmentEntryNew WHERE FDetailID=@ShipmentDetailID AND FOutSN = FCTN and FStatus in (0,1))
			    BEGIN
				    UPDATE CO_WH_ShipmentEntryNew SET FStatus=2 WHERE FDetailID=@ShipmentDetailID

				    IF NOT EXISTS( SELECT 1 FROM CO_WH_ShipmentEntryNew A
					    INNER JOIN CO_WH_ShipmentNew B ON A.FInterID=B.FInterID
					    WHERE B.FBillNO=@FBillNO AND (A.FOutSN<>a.FCTN))
				    BEGIN
					    UPDATE CO_WH_ShipmentNew SET FStatus=2 WHERE FBillNO=@FBillNO
					    SET @strOutput=0
				    END
			    END
		    END
		   
		    SELECT @ShipmentDetailID AS FDetailID,@strOutput AS OutResult
            ";

            DataSet DS = SqlServerHelper.Data_Set_Tran(S_Sql);
            return DS;
        }

        public string SetMesPackageShipment(string ShipmentDetailID, string SerialNumber, int Type)
        {
            string S_Sql = string.Empty;
            string ShipmentInterID = "";
            S_Sql = string.Format(@"select FInterID  from CO_WH_ShipmentEntryNew where FDetailID={0}", ShipmentDetailID);
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
            string S_Result = SqlServerHelper.ExecSql(S_Sql);
            return S_Result;
        }

        public string SetMesPackageShipmentRoll(string S_BillNo, string S_MultipackSN)
        {
            string S_Sql = "";
            S_Sql =
            @"DECLARE @FInterID INT
            SELECT @FInterID=FInterID FROM CO_WH_ShipmentNew WHERE FBillNO='" + S_BillNo + @"'
            UPDATE CO_WH_ShipmentNew SET FStatus = 1 WHERE FBillNO='" + S_BillNo + @"'
            UPDATE CO_WH_ShipmentEntryNew SET FStatus = 1,FOutSN = FOutSN-1 WHERE FInterID=@FInterID
            UPDATE mesPackage SET ShipmentInterID = NULL,ShipmentDetailID = NULL,ShipmentTime = NULL WHERE SerialNumber=" +
            @"'" + S_MultipackSN + "'";

            string S_Result = SqlServerHelper.ExecSql(S_Sql);
            return S_Result;
        }
    }
}



