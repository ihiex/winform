using App.DBUtility;
using App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DBServerDAL
{
    public class DataCommitDAL
    {
        public string SubmitDataUH(List<mesUnit> List_mesUnit, List<mesHistory> List_mesHistory)
        {
            string S_Result = "";
            string S_Sql = "";

            try
            {
                if (List_mesUnit != null)
                {
                    for (int i = 0; i < List_mesUnit.Count; i++)
                    {
                        mesUnit v_mesUnit = new mesUnit();
                        v_mesUnit = List_mesUnit[i];

                        S_Sql += " update mesUnit set UnitStateID='" + v_mesUnit.UnitStateID + "'\r\n" +
                                ",StationID='" + v_mesUnit.StationID + "'\r\n" +
                                ",StatusID='" + v_mesUnit.StatusID + "'\r\n" +
                                ",EmployeeID='" + v_mesUnit.EmployeeID + "'\r\n" +
                                ",ProductionOrderID='" + v_mesUnit.ProductionOrderID + "'\r\n" +
                                ",LastUpdate=getdate() \r\n" +
                                " where ID='" + v_mesUnit.ID + "' \r\n";
                    }
                }


                if (List_mesHistory != null)
                {
                    for (int i = 0; i < List_mesHistory.Count; i++)
                    {
                        mesHistory v_mesHistory = new mesHistory();
                        v_mesHistory = List_mesHistory[i];

                        S_Sql += "insert into mesHistory(UnitID, UnitStateID, EmployeeID, StationID, " + "\r\n" +
                            "EnterTime, ExitTime, ProductionOrderID, PartID, LooperCount, StatusID) Values( " + "\r\n" +

                            "'" + v_mesHistory.UnitID + "'," + "\r\n" +
                            "'" + v_mesHistory.UnitStateID + "'," + "\r\n" +
                            "'" + v_mesHistory.EmployeeID + "'," + "\r\n" +
                            "'" + v_mesHistory.StationID + "'," + "\r\n" +
                            "getdate()," + "\r\n" +
                            "getdate()," + "\r\n" +
                            "'" + v_mesHistory.ProductionOrderID + "'," + "\r\n" +
                            "'" + v_mesHistory.PartID + "'," + "\r\n" +
                            "'1'," + "\r\n" +
                            "'" + v_mesHistory.StatusID + "'" + "\r\n" +
                            ")" + "\r\n";

                    }
                }

                S_Result = SqlServerHelper.ExecSql(S_Sql);
            }

            catch (Exception ex)
            {
                S_Result = "ERROR:" + ex.ToString();
            }
            return S_Result;
        }

        public string SubmitDataUHD(List<mesUnit> List_mesUnit, List<mesHistory> List_mesHistory, List<mesUnitDefect> List_mesUnitDefect)
        {
            string S_Result = "";
            string S_Sql = "";

            try
            {
                if (List_mesUnit != null)
                {
                    for (int i = 0; i < List_mesUnit.Count; i++)
                    {
                        mesUnit v_mesUnit = new mesUnit();
                        v_mesUnit = List_mesUnit[i];

                        S_Sql += " update mesUnit set UnitStateID='" + v_mesUnit.UnitStateID + "'\r\n" +
                                ",StationID='" + v_mesUnit.StationID + "'\r\n" +
                                ",StatusID='" + v_mesUnit.StatusID + "'\r\n" +
                                ",EmployeeID='" + v_mesUnit.EmployeeID + "'\r\n" +
                                ",ProductionOrderID='" + v_mesUnit.ProductionOrderID + "'\r\n" +
                                ",LastUpdate=getdate() \r\n" +
                                " where ID='" + v_mesUnit.ID + "' \r\n";
                    }
                }

                if (List_mesHistory != null)
                {
                    for (int i = 0; i < List_mesHistory.Count; i++)
                    {
                        mesHistory v_mesHistory = new mesHistory();
                        v_mesHistory = List_mesHistory[i];

                        S_Sql += "insert into mesHistory(UnitID, UnitStateID, EmployeeID, StationID, " + "\r\n" +
                            "EnterTime, ExitTime, ProductionOrderID, PartID, LooperCount, StatusID) Values( " + "\r\n" +

                            "'" + v_mesHistory.UnitID + "'," + "\r\n" +
                            "'" + v_mesHistory.UnitStateID + "'," + "\r\n" +
                            "'" + v_mesHistory.EmployeeID + "'," + "\r\n" +
                            "'" + v_mesHistory.StationID + "'," + "\r\n" +
                            "getdate()," + "\r\n" +
                            "getdate()," + "\r\n" +
                            "'" + v_mesHistory.ProductionOrderID + "'," + "\r\n" +
                            "'" + v_mesHistory.PartID + "'," + "\r\n" +
                            "'1'," + "\r\n" +
                            "'" + v_mesHistory.StatusID + "'" + "\r\n" +
                            ")" + "\r\n";
                    }
                }

                if (List_mesUnitDefect != null)
                {
                    if (List_mesUnitDefect.Count > 0)
                    {
                        S_Sql += "declare @MaxDefID int" + "\r\n";
                    }
                    int I_Qyt = 1;


                    for (int i = 0; i < List_mesUnitDefect.Count; i++)
                    {
                        mesUnitDefect v_mesUnitDefect = new mesUnitDefect();
                        v_mesUnitDefect = List_mesUnitDefect[i];
                        I_Qyt = i + 1;

                        S_Sql +=
                                "select @MaxDefID=ISNULL(Max(ID),0)+" + I_Qyt + " from mesUnitDefect " + "\r\n" +
                                "INSERT INTO mesUnitDefect(ID, UnitID, DefectID, StationID, EmployeeID) Values(" + "\r\n" +
                                "@MaxDefID," + "\r\n" +
                                "'" + v_mesUnitDefect.UnitID + "'," + "\r\n" +
                                "'" + v_mesUnitDefect.DefectID + "'," + "\r\n" +
                                "'" + v_mesUnitDefect.StationID + "'," + "\r\n" +
                                "'" + v_mesUnitDefect.EmployeeID + "'" + "\r\n" +
                                ")" + "\r\n";
                    }
                }
                S_Result = SqlServerHelper.ExecSql(S_Sql);
            }

            catch (Exception ex)
            {
                S_Result = "ERROR:" + ex.ToString();
            }
            return S_Result;
        }

        public string SubmitDataUHCD(List<mesUnit> List_mesUnit, List<mesHistory> List_mesHistory,
            List<mesUnitComponent> List_mesUnitComponent, List<mesUnitDefect> List_mesUnitDefect)
        {
            string S_Result = "";
            string S_Sql = "";

            try
            {
                if (List_mesUnit != null)
                {
                    for (int i = 0; i < List_mesUnit.Count; i++)
                    {
                        mesUnit v_mesUnit = new mesUnit();
                        v_mesUnit = List_mesUnit[i];

                        S_Sql += " update mesUnit set UnitStateID='" + v_mesUnit.UnitStateID + "'\r\n" +
                                ",StationID='" + v_mesUnit.StationID + "'\r\n" +
                                ",StatusID='" + v_mesUnit.StatusID + "'\r\n" +
                                ",EmployeeID='" + v_mesUnit.EmployeeID + "'\r\n" +
                                ",ProductionOrderID='" + v_mesUnit.ProductionOrderID + "'\r\n" +
                                ",LastUpdate=getdate() \r\n" +
                                " where ID='" + v_mesUnit.ID + "' \r\n";
                    }
                }

                if (List_mesHistory != null)
                {
                    for (int i = 0; i < List_mesHistory.Count; i++)
                    {
                        mesHistory v_mesHistory = new mesHistory();
                        v_mesHistory = List_mesHistory[i];

                        S_Sql += "insert into mesHistory(UnitID, UnitStateID, EmployeeID, StationID, " + "\r\n" +
                            "EnterTime, ExitTime, ProductionOrderID, PartID, LooperCount, StatusID) Values( " + "\r\n" +

                            "'" + v_mesHistory.UnitID + "'," + "\r\n" +
                            "'" + v_mesHistory.UnitStateID + "'," + "\r\n" +
                            "'" + v_mesHistory.EmployeeID + "'," + "\r\n" +
                            "'" + v_mesHistory.StationID + "'," + "\r\n" +
                            "getdate()," + "\r\n" +
                            "getdate()," + "\r\n" +
                            "'" + v_mesHistory.ProductionOrderID + "'," + "\r\n" +
                            "'" + v_mesHistory.PartID + "'," + "\r\n" +
                            "'1'," + "\r\n" +
                            "'" + v_mesHistory.StatusID + "'" + "\r\n" +
                            ")" + "\r\n";
                    }
                }


                if (List_mesUnitComponent != null)
                {
                    for (int i = 0; i < List_mesUnitComponent.Count; i++)
                    {
                        mesUnitComponent v_mesUnitComponent = new mesUnitComponent();
                        v_mesUnitComponent = List_mesUnitComponent[i];

                        S_Sql += "insert into mesUnitComponent(UnitID, UnitComponentTypeID, ChildUnitID, ChildSerialNumber, ChildLotNumber," + "\r\n" +
                                "ChildPartID, ChildPartFamilyID,Position, InsertedEmployeeID, InsertedStationID, InsertedTime, StatusID, LastUpdate)" + "\r\n" +
                                "Values(" + "'" + v_mesUnitComponent.UnitID + "'," + "\r\n" +
                                "'1'," + "\r\n" +
                                "'" + v_mesUnitComponent.ChildUnitID + "'," + "\r\n" +
                                "'" + v_mesUnitComponent.ChildSerialNumber + "'," + "\r\n" +
                                "'" + v_mesUnitComponent.ChildLotNumber + "'," + "\r\n" +
                                "'" + v_mesUnitComponent.ChildPartID + "'," + "\r\n" +
                                "'" + v_mesUnitComponent.ChildPartFamilyID + "'," + "\r\n" +
                                "''," + "\r\n" +
                                "'" + v_mesUnitComponent.InsertedEmployeeID + "'," + "\r\n" +
                                "'" + v_mesUnitComponent.InsertedStationID + "'," + "\r\n" +
                                "GETDATE()," + "\r\n" +
                                "1," + "\r\n" +
                                "GETDATE()" + "\r\n" +
                                ")" + "\r\n";

                    }
                }

                if (List_mesUnitDefect != null)
                {
                    if (List_mesUnitDefect.Count > 0)
                    {
                        S_Sql += "declare @MaxDefID int" + "\r\n";
                    }
                    int I_Qyt = 1;


                    for (int i = 0; i < List_mesUnitDefect.Count; i++)
                    {
                        mesUnitDefect v_mesUnitDefect = new mesUnitDefect();
                        v_mesUnitDefect = List_mesUnitDefect[i];
                        I_Qyt = i + 1;

                        S_Sql +=
                                "select @MaxDefID=ISNULL(Max(ID),0)+" + I_Qyt + " from mesUnitDefect " + "\r\n" +
                                "INSERT INTO mesUnitDefect(ID, UnitID, DefectID, StationID, EmployeeID) Values(" + "\r\n" +
                                "@MaxDefID," + "\r\n" +
                                "'" + v_mesUnitDefect.UnitID + "'," + "\r\n" +
                                "'" + v_mesUnitDefect.DefectID + "'," + "\r\n" +
                                "'" + v_mesUnitDefect.StationID + "'," + "\r\n" +
                                "'" + v_mesUnitDefect.EmployeeID + "'" + "\r\n" +
                                ")" + "\r\n";
                    }
                }

                S_Result = SqlServerHelper.ExecSql(S_Sql);
            }

            catch (Exception ex)
            {
                S_Result = "ERROR:" + ex.ToString();
            }
            return S_Result;
        }


        public string SubmitDataUHC(List<mesUnit> List_mesUnit, List<mesHistory> List_mesHistory,
            List<mesUnitComponent> List_mesUnitComponent, List<mesMaterialConsumeInfo> List_mesMaterialConsumeInfo,
            List<mesMachine> List_mesMachine, LoginList F_LoginList
            )
        {
            string S_Result = "";
            string S_Sql = "";

            try
            {
                if (List_mesUnit != null)
                {
                    for (int i = 0; i < List_mesUnit.Count; i++)
                    {
                        mesUnit v_mesUnit = new mesUnit();
                        v_mesUnit = List_mesUnit[i];

                        S_Sql += " update mesUnit set UnitStateID='" + v_mesUnit.UnitStateID + "'\r\n" +
                                ",StationID='" + v_mesUnit.StationID + "'\r\n" +
                                ",StatusID='" + v_mesUnit.StatusID + "'\r\n" +
                                ",EmployeeID='" + v_mesUnit.EmployeeID + "'\r\n" +
                                ",ProductionOrderID='" + v_mesUnit.ProductionOrderID + "'\r\n" +
                                ",LastUpdate=getdate() \r\n" +
                                " where ID='" + v_mesUnit.ID + "' \r\n";
                    }
                }


                if (List_mesHistory != null)
                {
                    for (int i = 0; i < List_mesHistory.Count; i++)
                    {
                        mesHistory v_mesHistory = new mesHistory();
                        v_mesHistory = List_mesHistory[i];

                        S_Sql += "insert into mesHistory(UnitID, UnitStateID, EmployeeID, StationID, " + "\r\n" +
                            "EnterTime, ExitTime, ProductionOrderID, PartID, LooperCount, StatusID) Values( " + "\r\n" +

                            "'" + v_mesHistory.UnitID + "'," + "\r\n" +
                            "'" + v_mesHistory.UnitStateID + "'," + "\r\n" +
                            "'" + v_mesHistory.EmployeeID + "'," + "\r\n" +
                            "'" + v_mesHistory.StationID + "'," + "\r\n" +
                            "getdate()," + "\r\n" +
                            "getdate()," + "\r\n" +
                            "'" + v_mesHistory.ProductionOrderID + "'," + "\r\n" +
                            "'" + v_mesHistory.PartID + "'," + "\r\n" +
                            "'1'," + "\r\n" +
                            "'" + v_mesHistory.StatusID + "'" + "\r\n" +
                            ")" + "\r\n";

                    }
                }

                if (List_mesUnitComponent != null)
                {
                    for (int i = 0; i < List_mesUnitComponent.Count; i++)
                    {
                        mesUnitComponent v_mesUnitComponent = new mesUnitComponent();
                        v_mesUnitComponent = List_mesUnitComponent[i];

                        S_Sql += "insert into mesUnitComponent(UnitID, UnitComponentTypeID, ChildUnitID, ChildSerialNumber, ChildLotNumber," + "\r\n" +
                                "ChildPartID, ChildPartFamilyID,Position, InsertedEmployeeID, InsertedStationID, InsertedTime, StatusID, LastUpdate)" + "\r\n" +
                                "Values(" + "'" + v_mesUnitComponent.UnitID + "'," + "\r\n" +
                                "'1'," + "\r\n" +
                                "'" + v_mesUnitComponent.ChildUnitID + "'," + "\r\n" +
                                "'" + v_mesUnitComponent.ChildSerialNumber + "'," + "\r\n" +
                                "'" + v_mesUnitComponent.ChildLotNumber + "'," + "\r\n" +
                                "'" + v_mesUnitComponent.ChildPartID + "'," + "\r\n" +
                                "'" + v_mesUnitComponent.ChildPartFamilyID + "'," + "\r\n" +
                                "''," + "\r\n" +
                                "'" + v_mesUnitComponent.InsertedEmployeeID + "'," + "\r\n" +
                                "'" + v_mesUnitComponent.InsertedStationID + "'," + "\r\n" +
                                "GETDATE()," + "\r\n" +
                                "1," + "\r\n" +
                                "GETDATE()" + "\r\n" +
                                ")" + "\r\n";

                    }
                }

                if (List_mesMaterialConsumeInfo != null)
                {

                    for (int i = 0; i < List_mesMaterialConsumeInfo.Count; i++)
                    {
                        mesMaterialConsumeInfo v_mesMaterialConsumeInfo = new mesMaterialConsumeInfo();
                        v_mesMaterialConsumeInfo = List_mesMaterialConsumeInfo[i];
                        string S_FGSN = "";

                        if (v_mesMaterialConsumeInfo.ScanType == 2)
                        {
                            string S_MachineSN_Sql = "SELECT reserved_02 FROM mesUnitDetail WHERE ID = " +
                                "(SELECT MAX(ID) FROM mesUnitDetail WHERE reserved_01 = '" + v_mesMaterialConsumeInfo.MachineSN + "' AND reserved_03 = 1)";
                            DataTable DT_UnitDetail = SqlServerHelper.Data_Table(S_MachineSN_Sql);

                            if (DT_UnitDetail == null || DT_UnitDetail.Rows.Count == 0)
                            {
                                S_Result = "ERROR:The Tooling is not bound to FG ";
                            }
                            else
                            {
                                S_FGSN = DT_UnitDetail.Rows[0]["reserved_02"].ToString();
                            }
                        }
                        else
                        {
                            S_FGSN = v_mesMaterialConsumeInfo.SN;
                        }

                        string S_Sql_MCI = "SELECT 1 FROM mesMaterialConsumeInfo where PartID='" + v_mesMaterialConsumeInfo.PartID + "'" +
                                            " and ProductionOrderID='" + v_mesMaterialConsumeInfo.ProductionOrderID + "' " +
                                            " and LineID = '" + F_LoginList.LineID + "'" +
                                            " and StationID ='" + F_LoginList.StationID + "'" +
                                            " and SN = '" + S_FGSN + "'";
                        DataTable DT_MCI = SqlServerHelper.Data_Table(S_Sql_MCI);
                        if (DT_MCI != null && DT_MCI.Rows.Count > 0)
                        {
                            S_Sql +=
                            "UPDATE mesMaterialConsumeInfo SET ConsumeQTY=isnull(ConsumeQTY,0)+1 where PartID=" + "\r\n" +
                                    "'" + v_mesMaterialConsumeInfo.PartID + "' " + "\r\n" +
                                    " and  ProductionOrderID='" + v_mesMaterialConsumeInfo.ProductionOrderID + "'" + "\r\n" +
                                    " and  LineID='" + F_LoginList.LineID + "'" + "\r\n" +
                                    " and  StationID='" + F_LoginList.StationID + "'" + "\r\n" +
                                    " and  SN='" + S_FGSN + "'" + "\r\n";
                        }
                        else
                        {
                            var tmpConsumeInfos = List_mesMaterialConsumeInfo.Take(i);
                            var beforeConsumeInfos = tmpConsumeInfos.Where(x =>
                                x.SN == S_FGSN && x.PartID == v_mesMaterialConsumeInfo.PartID &&
                                x.ProductionOrderID == v_mesMaterialConsumeInfo.ProductionOrderID &&
                                (x.LineID == F_LoginList.LineID || x.LineID == 0) && (x.StationID == F_LoginList.StationID || x.StationID == 0));
                            if (beforeConsumeInfos?.Count() <= 0)
                            {
                                S_Sql +=
                                    "INSERT INTO mesMaterialConsumeInfo(SN,MaterialTypeID,PartID,ProductionOrderID,LineID,StationID,ConsumeQTY)" + "\r\n" +
                                    " Values(" + "'" + S_FGSN + "'" + "\r\n" +
                                    ",'1'" + "\r\n" +
                                    ",'" + v_mesMaterialConsumeInfo.PartID + "'" + "\r\n" +
                                    ",'" + v_mesMaterialConsumeInfo.ProductionOrderID + "'" + "\r\n" +
                                    ",'" + F_LoginList.LineID + "'" + "\r\n" +
                                    ",'" + F_LoginList.StationID + "'" + "\r\n" +
                                    ",1" + ")" + "\r\n";
                            }
                            else
                            {
                                S_Sql +=
                                    "UPDATE mesMaterialConsumeInfo SET ConsumeQTY=isnull(ConsumeQTY,0)+1 where PartID=" + "\r\n" +
                                    "'" + v_mesMaterialConsumeInfo.PartID + "' " + "\r\n" +
                                    " and  ProductionOrderID='" + v_mesMaterialConsumeInfo.ProductionOrderID + "'" + "\r\n" +
                                    " and  LineID='" + F_LoginList.LineID + "'" + "\r\n" +
                                    " and  StationID='" + F_LoginList.StationID + "'" + "\r\n" +
                                    " and  SN='" + S_FGSN + "'" + "\r\n";
                            }
                        }
                    }
                }


                if (List_mesMachine != null)
                {
                    for (int i = 0; i < List_mesMachine.Count; i++)
                    {
                        mesMachine v_mesMachine = List_mesMachine[i];
                        string S_MachineReult = GetModMachineBySNStationTypeID_Sql(v_mesMachine.SN, F_LoginList.StationTypeID);

                        if (S_MachineReult != "")
                        {
                            if (S_MachineReult.Substring(0, 5) == "ERROR")
                            {
                                return S_Result = S_MachineReult;
                            }
                            else
                            {
                                S_Sql += S_MachineReult;
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
            return S_Result;
        }

        public string InsertUDS(mesUnit v_mesUnit, mesUnitDetail v_mesUnitDetail, mesSerialNumber v_mesSerialNumber,
            string S_ObjSN)
        {
            string S_Result = "";
            string S_Sql = "";

            try
            {
                S_Sql = " declare @UnitID int select @UnitID=max(ID)+1 from mesUnit" + "\r\n" +

                       " INSERT INTO mesUnit(ID,UnitStateID,StatusID,StationID,EmployeeID," +
                       "CreationTime,LastUpdate,PanelID,LineID," + "\r\n" +
                       "ProductionOrderID,RMAID,PartID,LooperCount) " + "\r\n" +

                      "VALUES (@UnitID" + "\r\n" +
                             ",'" + v_mesUnit.UnitStateID + "'" + "\r\n" +
                             ",1" + "\r\n" +
                             ",'" + v_mesUnit.StationID + "'" + "\r\n" +
                             ",'" + v_mesUnit.EmployeeID + "'" + "\r\n" +
                             ",GETDATE()" + "\r\n" +
                             ",GETDATE()" + "\r\n" +
                             ",0" + "\r\n" +
                             ",'" + v_mesUnit.LineID + "'" + "\r\n" +
                             ",'" + v_mesUnit.ProductionOrderID + "'" + "\r\n" +
                             ",0" + "\r\n" +
                             ",'" + v_mesUnit.PartID + "'" + "\r\n" +
                             ",1)" + "\r\n" +

                      " Insert into mesUnitDetail(UnitID,reserved_01,reserved_02,reserved_03,reserved_04,reserved_05) Values(" + "\r\n" +
                      " @UnitID,'" + S_ObjSN + "','','1','','')" + "\r\n" +

                      " INSERT INTO mesSerialNumber(UnitID,SerialNumberTypeID,Value) VALUES (@UnitID,5,'" + v_mesSerialNumber.Value + "') " + "\r\n" +
                      " select @UnitID as SqlResult";
                S_Result = SqlServerHelper.ExecSqlDataRead(S_Sql);
            }
            catch (Exception ex)
            {
                S_Result = "ERROR:" + ex.ToString();
            }
            return S_Result;
        }

        public string UpdatemesUnit(mesUnit v_mesUnit)
        {
            string S_Result = "";
            string S_Sql = "";

            try
            {
                S_Sql = " update mesUnit set UnitStateID='" + v_mesUnit.UnitStateID + "'\r\n" +
                        ",StationID='" + v_mesUnit.StationID + "'\r\n" +
                        ",StatusID='" + v_mesUnit.StatusID + "'\r\n" +
                        ",EmployeeID='" + v_mesUnit.EmployeeID + "'\r\n" +
                        ",ProductionOrderID='" + v_mesUnit.ProductionOrderID + "'\r\n" +
                        ",LastUpdate=getdate() \r\n" +
                        " where ID='" + v_mesUnit.ID + "' \r\n";

                S_Result = SqlServerHelper.ExecSql(S_Sql);
            }
            catch (Exception ex)
            {
                S_Result = "ERROR:" + ex.ToString();
            }
            return S_Result;
        }

        public string InsertALL(List<mesUnit> List_mesUnit,
                                List<mesUnitDetail> List_mesUnitDetail,
                                List<mesHistory> List_mesHistory,
                                List<mesSerialNumber> List_mesSerialNumber,
                                List<mesUnitComponent> List_mesUnitComponent,
                                List<mesUnitDefect> List_mesUnitDefect,
                                List<mesMachine> List_mesMachine,
                                LoginList F_LoginList,
                                string[] L_TLinkT
                                )
        {
            string S_Result = "";
            string S_Sql = "";

            try
            {
                if (List_mesUnit != null)
                {
                    for (int i = 0; i < List_mesUnit.Count; i++)
                    {
                        mesUnit v_mesUnit = new mesUnit();
                        v_mesUnit = List_mesUnit[i];

                        S_Sql +=
                        " declare @UnitID int select @UnitID=max(ID)+1 from mesUnit" + "\r\n" +

                        " INSERT INTO mesUnit(ID,UnitStateID,StatusID,StationID,EmployeeID," +
                        "CreationTime,LastUpdate,PanelID,LineID," + "\r\n" +
                        "ProductionOrderID,RMAID,PartID,LooperCount) " + "\r\n" +

                        "VALUES (@UnitID" + "\r\n" +
                                ",'" + v_mesUnit.UnitStateID + "'" + "\r\n" +
                                ",'" + v_mesUnit.StatusID + "'" + "\r\n" +
                                ",'" + v_mesUnit.StationID + "'" + "\r\n" +
                                ",'" + v_mesUnit.EmployeeID + "'" + "\r\n" +
                                ",GETDATE()" + "\r\n" +
                                ",GETDATE()" + "\r\n" +
                                ",0" + "\r\n" +
                                ",'" + v_mesUnit.LineID + "'" + "\r\n" +
                                ",'" + v_mesUnit.ProductionOrderID + "'" + "\r\n" +
                                ",0" + "\r\n" +
                                ",'" + v_mesUnit.PartID + "'" + "\r\n" +
                                ",1)" + "\r\n";
                    }
                }


                if (List_mesUnitDetail != null)
                {
                    for (int i = 0; i < List_mesUnitDetail.Count; i++)
                    {
                        mesUnitDetail v_mesUnitDetail = new mesUnitDetail();
                        v_mesUnitDetail = List_mesUnitDetail[i];

                        S_Sql +=
                             "INSERT INTO mesUnitDetail(UnitID,reserved_01,reserved_02,reserved_03,reserved_04,reserved_05) VALUES " + "\r\n" +
                             "(@UnitID" + "\r\n" +
                             ",'" + (string)v_mesUnitDetail.reserved_01 + "'" + "\r\n" +
                             ",'" + (string)v_mesUnitDetail.reserved_02 + "'" + "\r\n" +
                             ",'" + (string)v_mesUnitDetail.reserved_03 + "'" + "\r\n" +
                             ",'" + (string)v_mesUnitDetail.reserved_04 + "'" + "\r\n" +
                             ",'" + (string)v_mesUnitDetail.reserved_05 + "'" + "\r\n" +
                             ")" + "\r\n";
                    }
                }


                if (List_mesHistory != null)
                {
                    for (int i = 0; i < List_mesHistory.Count; i++)
                    {
                        mesHistory v_mesHistory = new mesHistory();
                        v_mesHistory = List_mesHistory[i];

                        S_Sql += "insert into mesHistory(UnitID, UnitStateID, EmployeeID, StationID, " + "\r\n" +
                            "EnterTime, ExitTime, ProductionOrderID, PartID, LooperCount, StatusID) Values( " + "\r\n" +

                            "@UnitID," + "\r\n" +
                            "'" + v_mesHistory.UnitStateID + "'," + "\r\n" +
                            "'" + v_mesHistory.EmployeeID + "'," + "\r\n" +
                            "'" + v_mesHistory.StationID + "'," + "\r\n" +
                            "getdate()," + "\r\n" +
                            "getdate()," + "\r\n" +
                            "'" + v_mesHistory.ProductionOrderID + "'," + "\r\n" +
                            "'" + v_mesHistory.PartID + "'," + "\r\n" +
                            "'1'," + "\r\n" +
                            "'" + v_mesHistory.StatusID + "'" + "\r\n" +
                            ")" + "\r\n";
                    }
                }


                if (List_mesSerialNumber != null)
                {
                    for (int i = 0; i < List_mesSerialNumber.Count; i++)
                    {
                        mesSerialNumber v_mesSerialNumber = new mesSerialNumber();
                        v_mesSerialNumber = List_mesSerialNumber[i];

                        S_Sql +=
                        " INSERT INTO mesSerialNumber(UnitID,SerialNumberTypeID,Value) VALUES " + "\r\n" +
                        "(@UnitID," + "\r\n" +
                        "'" + v_mesSerialNumber.SerialNumberTypeID + "'," + "\r\n" +
                        "'" + v_mesSerialNumber.Value + "'" + "\r\n" +
                        ") " + "\r\n";
                    }
                }


                if (List_mesUnitComponent != null)
                {
                    for (int i = 0; i < List_mesUnitComponent.Count; i++)
                    {
                        mesUnitComponent v_mesUnitComponent = new mesUnitComponent();
                        v_mesUnitComponent = List_mesUnitComponent[i];

                        S_Sql +=
                        " insert into mesUnitComponent(UnitID, UnitComponentTypeID, ChildUnitID, ChildSerialNumber, ChildLotNumber, " + "\r\n" +
                        "ChildPartID, ChildPartFamilyID,Position, InsertedEmployeeID, InsertedStationID, InsertedTime, StatusID, LastUpdate) " + "\r\n" +
                        "values(@UnitID,'1','0'" + "\r\n" +
                        ",'" + v_mesUnitComponent.ChildSerialNumber + "'" + "\r\n" +
                        ",'',0,0,''" + "\r\n" +
                        ",'" + v_mesUnitComponent.InsertedEmployeeID + "'" + "\r\n" +
                        ",'" + v_mesUnitComponent.InsertedStationID + "'" + "\r\n" +
                        ",GETDATE(),1,GETDATE()) " + "\r\n"; ;

                    }
                }


                if (List_mesUnitDefect != null)
                {
                    if (List_mesUnitDefect.Count > 0)
                    {
                        S_Sql += "declare @MaxDefID int" + "\r\n";
                    }
                    int I_Qyt = 1;

                    for (int i = 0; i < List_mesUnitDefect.Count; i++)
                    {
                        mesUnitDefect v_mesUnitDefect = new mesUnitDefect();
                        v_mesUnitDefect = List_mesUnitDefect[i];
                        I_Qyt = i + 1;

                        S_Sql +=
                                "select @MaxDefID=ISNULL(Max(ID),0)+" + I_Qyt + " from mesUnitDefect " + "\r\n" +
                                "INSERT INTO mesUnitDefect(ID, UnitID, DefectID, StationID, EmployeeID) Values(" + "\r\n" +
                                "@MaxDefID," + "\r\n" +
                                "@UnitID," + "\r\n" +
                                "'" + v_mesUnitDefect.DefectID + "'," + "\r\n" +
                                "'" + v_mesUnitDefect.StationID + "'," + "\r\n" +
                                "'" + v_mesUnitDefect.EmployeeID + "'" + "\r\n" +
                                ")" + "\r\n";
                    }
                }

                if (List_mesMachine != null)
                {
                    for (int i = 0; i < List_mesMachine.Count; i++)
                    {
                        mesMachine v_mesMachine = List_mesMachine[i];
                        string S_MachineReult = GetModMachineBySNStationTypeID_Sql(v_mesMachine.SN, F_LoginList.StationTypeID);

                        if (S_MachineReult != "")
                        {
                            if (S_MachineReult.Substring(0, 5) == "ERROR")
                            {
                                return S_Result = S_MachineReult;
                            }
                            else
                            {
                                S_Sql += S_MachineReult;
                            }
                        }
                    }
                }

                if (L_TLinkT != null)
                {
                    if (L_TLinkT.Length >= 3)
                    {
                        string S_ToolingLinkTooling = SetToolingLinkTooling_Sql(L_TLinkT[0], L_TLinkT[1], L_TLinkT[2], F_LoginList);

                        if (S_ToolingLinkTooling.Substring(0, 5) == "ERROR")
                        {
                            return S_Result = S_ToolingLinkTooling;
                        }
                        else
                        {
                            S_Sql += S_ToolingLinkTooling;
                        }
                    }
                }


                S_Result = SqlServerHelper.ExecSql(S_Sql);
            }

            catch (Exception ex)
            {
                S_Result = "ERROR:" + ex.ToString();
            }
            return S_Result;
        }


        private string GetModMachineBySNStationTypeID_Sql(string MachineSN, int StationTypeID)
        {
            string S_Result = "";

            string S_Sql = string.Empty;

            try
            {
                S_Sql = string.Format(@"select WarningStatus,ValidFrom,ValidTo,RuningStationTypeID,
                                    RuningCapacityQuantity,ValidDistribution from mesMachine where SN='{0}'", MachineSN);
                DataTable dts = SqlServerHelper.ExecuteDataTable(S_Sql);

                if (dts != null || dts.Rows.Count > 0)
                {
                    int WarningStatus = Convert.ToInt32(dts.Rows[0]["WarningStatus"].ToString());
                    int RuningCapacityQuantity = 0;
                    if (dts.Rows[0]["RuningCapacityQuantity"].ToString() != "")
                    {
                        RuningCapacityQuantity = Convert.ToInt32(dts.Rows[0]["RuningCapacityQuantity"].ToString());
                    }
                    string[] StationFromList = dts.Rows[0]["ValidFrom"].ToString().Split(';');
                    string[] StationToList = dts.Rows[0]["ValidTo"].ToString().Split(';');
                    string[] ValidDistributionList = dts.Rows[0]["ValidDistribution"].ToString().Split(';');
                    string RuningStationTypeID = dts.Rows[0]["RuningStationTypeID"].ToString();

                    if (WarningStatus == 1 || WarningStatus == 2 || WarningStatus == 3)
                    {
                        if (RuningStationTypeID != StationTypeID.ToString())
                        {
                            S_Sql = string.Format(@"Update mesMachine set RuningStationTypeID={1},RuningCapacityQuantity=1 where SN = '{0}'", MachineSN, StationTypeID);
                            RuningCapacityQuantity = 1;
                            RuningStationTypeID = StationTypeID.ToString();
                        }
                        else
                        {
                            S_Sql = string.Format(@"Update mesMachine set RuningCapacityQuantity=isnull(RuningCapacityQuantity,0)+1 where SN = '{0}'", MachineSN);
                            RuningCapacityQuantity = RuningCapacityQuantity + 1;
                        }
                        S_Result += "\r\n" + S_Sql;
                    }

                    if (WarningStatus == 2 || WarningStatus == 3)
                    {
                        if (StationFromList.Contains(StationTypeID.ToString()))
                        {
                            S_Sql = string.Format(@"Update mesMachine set RuningQuantity=isnull(RuningQuantity,0)+1 where SN = '{0}'", MachineSN);
                            S_Result += "\r\n" + S_Sql;
                        }
                    }

                    if (StationFromList.Contains(StationTypeID.ToString()))
                    {
                        S_Sql = string.Format("Update mesMachine set StartRuningTime=GETDATE(),StatusID='2' where SN = '{0}'", MachineSN);
                        S_Result += "\r\n" + S_Sql;
                    }
                    if (StationToList.Contains(StationTypeID.ToString()))
                    {
                        if (WarningStatus == 1 || WarningStatus == 3)
                        {
                            int qty = 0;
                            foreach (string str in ValidDistributionList)
                            {
                                string[] strList = str.Split(',');
                                if (strList[0].ToString() == StationTypeID.ToString())
                                {
                                    qty = Convert.ToInt32(strList[1].ToString());
                                    break;
                                }
                            }

                            if (RuningCapacityQuantity >= qty && RuningStationTypeID == StationTypeID.ToString())
                            {
                                S_Sql = string.Format("Update mesMachine set LastRuningTime=GETDATE(),StatusID=1 where SN = '{0}'", MachineSN);
                                S_Result += "\r\n" + S_Sql;
                                S_Sql = string.Format(" UPDATE mesUnitDetail SET reserved_03 = '2' WHERE reserved_03 = '1' AND reserved_01 = '{0}'", MachineSN);
                                S_Result += "\r\n" + S_Sql;
                            }
                        }
                        else
                        {
                            S_Sql = string.Format("Update mesMachine set LastRuningTime=GETDATE(),StatusID='1' where SN = '{0}'", MachineSN);
                            S_Result += "\r\n" + S_Sql;
                            S_Sql = string.Format(" UPDATE mesUnitDetail SET reserved_03 = '2' WHERE reserved_03 = '1' AND reserved_01 = '{0}'", MachineSN);
                            S_Result += "\r\n" + S_Sql;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                S_Result = "ERROR:" + ex.ToString();
            }

            return S_Result;
        }

        private string SetToolingLinkTooling_Sql(string FromTooling, string ToTooling, string FromUintID, LoginList loginList)
        {
            string S_Result = "";
            string Str_Sql = string.Format("SELECT MAX(UnitID) UnitID FROM mesUnitDetail WHERE reserved_01='{0}'", ToTooling);

            string ToUnitID = string.Empty;
            DataTable dts = SqlServerHelper.ExecuteDataTable(Str_Sql);
            if (dts == null || dts.Rows.Count == 0)
            {
                S_Result = "ERROR:" + ToTooling + " not exists";
                return S_Result;
            }

            ToUnitID = dts.Rows[0]["UnitID"].ToString();
            Str_Sql = string.Format(@"insert into mesUnitDetail(UnitID,ProductionOrderID,RMAID,LooperCount,KitSerialNumber,InmostPackageID,OutmostPackageID
                                          , reserved_01, reserved_02, reserved_03, reserved_04, reserved_05, reserved_06, reserved_07, reserved_08, reserved_09, reserved_10
                                          , reserved_11, reserved_12, reserved_13, reserved_14, reserved_15, reserved_16, reserved_17, reserved_18, reserved_19, reserved_20)
                                    select {0},ProductionOrderID,RMAID,LooperCount,KitSerialNumber,InmostPackageID,OutmostPackageID
                                          ,reserved_01,reserved_02,reserved_03,reserved_04,reserved_05,reserved_06,reserved_07,reserved_08,reserved_09,reserved_10
                                          ,reserved_11,reserved_12,reserved_13,reserved_14,reserved_15,reserved_16,reserved_17,reserved_18,reserved_19,reserved_20
                                    from mesUnitDetail where UnitID ={1} ", FromUintID, ToUnitID);
            //SqlServerHelper.ExecuteNonQuery(Str_Sql);
            S_Result += Str_Sql + "\r\n";

            Str_Sql = string.Format(@"INSERT INTO mesUnitComponent(UnitID,UnitComponentTypeID,ChildUnitID,ChildSerialNumber,ChildLotNumber
	                                      ,ChildPartID,ChildPartFamilyID,Position,InsertedEmployeeID,InsertedStationID,InsertedTime
	                                      ,RemovedEmployeeID,RemovedStationID,RemovedTime,StatusID,LastUpdate,PreviousLink)
                                      SELECT {0},UnitComponentTypeID,ChildUnitID,ChildSerialNumber,ChildLotNumber
	                                      ,ChildPartID,ChildPartFamilyID,Position,'{1}',{2},GETDATE()
	                                      ,RemovedEmployeeID,RemovedStationID,RemovedTime,StatusID,GETDATE(),PreviousLink
	                                      FROM mesUnitComponent WHERE UnitID={3} AND StatusID=1", FromUintID, loginList.EmployeeID, loginList.StationID, ToUnitID);
            //SqlServerHelper.ExecuteNonQuery(Str_Sql);
            S_Result += Str_Sql + "\r\n";

            return S_Result;
        }



        public string SubmitDataUH_UDetail(List<mesUnit> List_mesUnit, List<mesHistory> List_mesHistory,
            List<mesUnitDetail> List_mesUnitDetail)
        {
            string S_Result = "";
            string S_Sql = "";

            try
            {
                if (List_mesUnit != null)
                {
                    for (int i = 0; i < List_mesUnit.Count; i++)
                    {
                        mesUnit v_mesUnit = new mesUnit();
                        v_mesUnit = List_mesUnit[i];

                        S_Sql += " update mesUnit set UnitStateID='" + v_mesUnit.UnitStateID + "'\r\n" +
                                ",StationID='" + v_mesUnit.StationID + "'\r\n" +
                                ",StatusID='" + v_mesUnit.StatusID + "'\r\n" +
                                ",EmployeeID='" + v_mesUnit.EmployeeID + "'\r\n" +
                                ",ProductionOrderID='" + v_mesUnit.ProductionOrderID + "'\r\n" +
                                ",LastUpdate=getdate() \r\n" +
                                " where ID='" + v_mesUnit.ID + "' \r\n";
                    }
                }

                if (List_mesHistory != null)
                {
                    for (int i = 0; i < List_mesHistory.Count; i++)
                    {
                        mesHistory v_mesHistory = new mesHistory();
                        v_mesHistory = List_mesHistory[i];

                        S_Sql += "insert into mesHistory(UnitID, UnitStateID, EmployeeID, StationID, " + "\r\n" +
                            "EnterTime, ExitTime, ProductionOrderID, PartID, LooperCount, StatusID) Values( " + "\r\n" +

                            "'" + v_mesHistory.UnitID + "'," + "\r\n" +
                            "'" + v_mesHistory.UnitStateID + "'," + "\r\n" +
                            "'" + v_mesHistory.EmployeeID + "'," + "\r\n" +
                            "'" + v_mesHistory.StationID + "'," + "\r\n" +
                            "getdate()," + "\r\n" +
                            "getdate()," + "\r\n" +
                            "'" + v_mesHistory.ProductionOrderID + "'," + "\r\n" +
                            "'" + v_mesHistory.PartID + "'," + "\r\n" +
                            "'1'," + "\r\n" +
                            "'" + v_mesHistory.StatusID + "'" + "\r\n" +
                            ")" + "\r\n";
                    }
                }


                if (List_mesUnitDetail != null)
                {
                    for (int i = 0; i < List_mesUnitDetail.Count; i++)
                    {
                        mesUnitDetail v_mesUnitDetail = new mesUnitDetail();
                        v_mesUnitDetail = List_mesUnitDetail[i];

                        S_Sql += "Update mesUnitDetail set " + (string.IsNullOrEmpty(v_mesUnitDetail.KitSerialNumber) ?"": " KitSerialNumber='" + (string)v_mesUnitDetail.KitSerialNumber + "'\r\n,") +
                            "reserved_01='" + (string)v_mesUnitDetail.reserved_01 + "'\r\n" +
                            ",reserved_02='" + (string)v_mesUnitDetail.reserved_02 + "'\r\n" +
                            ",reserved_03='" + (string)v_mesUnitDetail.reserved_03 + "'\r\n" +
                            ",reserved_04='" + (string)v_mesUnitDetail.reserved_04 + "'\r\n" +
                            ",reserved_05='" + (string)v_mesUnitDetail.reserved_05 + "'\r\n" +
                            ",reserved_06='" + (string)v_mesUnitDetail.reserved_06 + "'\r\n" +
                            ",reserved_07='" + (string)v_mesUnitDetail.reserved_07 + "'\r\n" +
                            ",reserved_08='" + (string)v_mesUnitDetail.reserved_08 + "'\r\n" +
                            ",reserved_09='" + (string)v_mesUnitDetail.reserved_09 + "'\r\n" +
                            ",reserved_10='" + (string)v_mesUnitDetail.reserved_10 + "'\r\n" +
                            ",reserved_11='" + (string)v_mesUnitDetail.reserved_11 + "'\r\n" +
                            ",reserved_12='" + (string)v_mesUnitDetail.reserved_12 + "'\r\n" +

                            " where UnitID='" + v_mesUnitDetail.UnitID + "'" + "\r\n";

                    }
                }

                S_Result = SqlServerHelper.ExecSql(S_Sql);
            }

            catch (Exception ex)
            {
                S_Result = "ERROR:" + ex.ToString();
            }
            return S_Result;
        }



        Shipment v_Shipment = new Shipment();
        public string uspPalletPackaging(string PartID, string ProductionOrderID, string S_CartonSN,
            string S_PalletSN, LoginList loginList, string S_BillNO, int PalletQty = 0)
        {
            return v_Shipment.uspPalletPackaging(PartID, ProductionOrderID, S_CartonSN, S_PalletSN, loginList, S_BillNO, PalletQty);
        }

        public string MoveShipment(string S_BillNo, string S_MultipackSN, string S_ProdOrderID, string S_PartID, string S_StationId, string S_EmployeeId)
        {
            return v_Shipment.MoveShipment(S_BillNo, S_MultipackSN, S_ProdOrderID, S_PartID, S_StationId, S_EmployeeId);
        }

        public DataSet SetShipmentMultipack(string S_BillNo, string S_MultipackSN, string S_MultipackPalletSN)
        {
            return v_Shipment.SetShipmentMultipack(S_BillNo, S_MultipackSN, S_MultipackPalletSN);
        }

        public string SetMesPackageShipment(string ShipmentDetailID, string SerialNumber, int Type)
        {
            return v_Shipment.SetMesPackageShipment(ShipmentDetailID, SerialNumber, Type);
        }

        public string SetMesPackageShipmentRoll(string S_BillNo, string S_MultipackPalletSN, string S_MultipackSN, string S_ShipmentDetailID)
        {
            return v_Shipment.SetMesPackageShipmentRoll(S_BillNo, S_MultipackPalletSN,S_MultipackSN, S_ShipmentDetailID);
        }

        public string GetShipmentPalletSN(string S_BillNo)
        {
            return v_Shipment.GetShipmentPalletSN(S_BillNo);
        }

        public string GetIsOutCountComplete(string S_BillNo, string S_MultipackPalletSN, string S_ShipmentDetailID, Boolean B_ScanOver)
        {
            return v_Shipment.GetIsOutCountComplete(S_BillNo, S_MultipackPalletSN, S_ShipmentDetailID, B_ScanOver);
        }


        //PrintOne//PrintOne//PrintOne//PrintOne//PrintOne//PrintOne//PrintOne//PrintOne
        ShipmentPrintOne v_ShipmentPrintOne = new ShipmentPrintOne();
        public string uspPalletPackagingPrintOne(string PartID, string ProductionOrderID, string S_CartonSN,
            string S_PalletSN, LoginList loginList, int PalletQty = 0)
        {
            return v_ShipmentPrintOne.uspPalletPackaging(PartID, ProductionOrderID, S_CartonSN, S_PalletSN, loginList, PalletQty);
        }

        public string MoveShipmentPrintOne(string S_BillNo, string S_MultipackSN, string S_ProdOrderID, string S_PartID, string S_StationId, string S_EmployeeId)
        {
            return v_ShipmentPrintOne.MoveShipment(S_BillNo, S_MultipackSN, S_ProdOrderID, S_PartID, S_StationId, S_EmployeeId);
        }

        public DataSet SetShipmentMultipackPrintOne(string S_BillNo, string S_MultipackSN)
        {
            return v_ShipmentPrintOne.SetShipmentMultipack(S_BillNo, S_MultipackSN);
        }

        public string SetMesPackageShipmentPrintOne(string ShipmentDetailID, string SerialNumber, int Type)
        {
            return v_ShipmentPrintOne.SetMesPackageShipment(ShipmentDetailID, SerialNumber, Type);
        }

        public string SetMesPackageShipmentRollPrintOne(string S_BillNo, string S_MultipackSN)
        {
            return v_ShipmentPrintOne.SetMesPackageShipmentRoll(S_BillNo, S_MultipackSN);
        }


        ////////////// WH WH WH////WH WH WH////WH WH WH////WH WH WH////WH WH WH////WH WH WH////WH WH WH////
        WH v_WH = new WH(); 
        public string SetCancelInWH(string S_BoxSN, string S_ProdID, string S_PartID, string S_StationID, string S_EmployeeID)
        {
            return v_WH.SetCancelInWH(S_BoxSN, S_ProdID, S_PartID, S_StationID, S_EmployeeID);
        }
        public string SetCancelInWHEntry(string S_BoxSN, string S_ProdID, string S_PartID, string S_StationID, string S_EmployeeID, string S_ReturnToStationTypeID, string S_ReturnStatus)
        {
            return v_WH.SetCancelInWHEntry(S_BoxSN, S_ProdID, S_PartID, S_StationID, S_EmployeeID, S_ReturnToStationTypeID, S_ReturnStatus);
        }
    }
}
