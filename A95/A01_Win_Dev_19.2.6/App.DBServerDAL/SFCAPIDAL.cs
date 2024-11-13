using App.DBUtility;
using App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace App.DBServerDAL
{
    public class SFCAPIDAL
    {
        PartSelectDAL v_PartSelect = new PartSelectDAL();
        mesPartDAL v_mesPartDAL = new mesPartDAL();
        mesUnitStateDAL v_mesUnitState = new mesUnitStateDAL();
        DataCommitDAL v_DataCommitDAL = new DataCommitDAL();
        List<ToolingInfo> toolingInfos = new List<ToolingInfo>();
        public string MainSN_Check(string S_SN, LoginList List_Login)
        {
            string S_Result = "1";

            DateTime Date_Start = DateTime.Now;
            string outString1 = string.Empty;
            string mainSN = S_SN.Trim();

            string S_PartID = "";
            string S_POPartID = "";
            string PartFamilyID = "";
            string S_ProductionOrderID = "";
            string S_IsCheckPO = "1";

            Boolean COF = false;
            string OpenLogFile = System.Configuration.ConfigurationManager.AppSettings["OpenLogFile"];
            DataTable DT_Unit = new DataTable();
            List_Login.IsCheckPO = true;

            try
            {
                if (mainSN == "")
                {
                    //"SN不能为空";
                    S_Result = MyMSG(List_Login.Language, "20119");
                    return S_Result;
                }

                string S_Sql = "SELECT StationTypeID FROM mesStation WHERE ID="+ List_Login.StationID;
                DataTable DT_StationType = SqlServerHelper.Data_Table(S_Sql);
                List_Login.StationTypeID =Convert.ToInt32(DT_StationType.Rows[0]["StationTypeID"].ToString());

                DataSet dsStationDetail = v_PartSelect.GetPLCSeting("", List_Login.StationID.ToString());
                DataSet DS_StationTypeDef = v_PartSelect.MESGetStationTypeParameter(List_Login.StationTypeID);

                DataRow[] DR_IsCheckPO = DS_StationTypeDef.Tables[0].Select("Description='IsCheckPO'");
                if (DR_IsCheckPO.Length > 0)
                {
                    S_IsCheckPO = DR_IsCheckPO[0]["Content"].ToString();

                    if (S_IsCheckPO == "1")
                    {
                        DataRow[] DR_Child_IsCheckPO = dsStationDetail.Tables[0].Select("Name='IsCheckPO'");
                        if (DR_Child_IsCheckPO.Length > 0) S_IsCheckPO = DR_Child_IsCheckPO[0]["Value"].ToString(); else S_IsCheckPO = "1";
                    }
                }
                else
                {
                    S_IsCheckPO = "1";
                }

                if (S_IsCheckPO != "1") { List_Login.IsCheckPO = false;  }

                //获取 SN
                S_Sql = "select * from mesSerialNumber where Value='"+ mainSN + "'";
                DataTable DT_GetSN = SqlServerHelper.Data_Table(S_Sql);
                if (DT_GetSN.Rows.Count == 0)
                {
                    S_Sql = @"SELECT  B.[Value] SN
                                FROM mesUnitDetail A JOIN mesSerialNumber B ON A.UnitID = B.UnitID
                             WHERE A.reserved_01 = '"+ mainSN + "' AND A.reserved_03 = 1";
                    DT_GetSN = SqlServerHelper.Data_Table(S_Sql);

                    mainSN = "";
                    if (DT_GetSN.Rows.Count > 0)
                    {
                        mainSN = DT_GetSN.Rows[0]["SN"].ToString();
                    }
                }


                DataSet dsMainSN;
                dsMainSN = v_PartSelect.uspCallProcedure("uspGetBaseData", mainSN,
                       null, null, null, null, null, ref outString1);
                if (outString1 != "1" || dsMainSN == null || dsMainSN.Tables[0].Rows.Count <= 0)
                {
                    //条码不存在或者状态不符.
                    S_Result = MyMSG(List_Login.Language, "20119");
                    return S_Result;
                }

                DataTable dt = dsMainSN.Tables[0];
                S_ProductionOrderID = dt.Rows[0]["ProductionOrderID"].ToString();
                S_PartID = dt.Rows[0]["PartID"].ToString();
                S_POPartID = dt.Rows[0]["PartID"].ToString();
                PartFamilyID = dt.Rows[0]["PartFamilyID"].ToString();

                if (List_Login.IsCheckPO == true)
                {
                    S_Sql = "select Value  from mesStationConfigSetting where StationID='"+ List_Login.StationID + "' and Name='POID'";
                    DataTable DT_COO_POID = SqlServerHelper.Data_Table(S_Sql);

                    if (DT_COO_POID.Rows.Count > 0)
                    {
                        string S_COO_POID = DT_COO_POID.Rows[0]["Value"].ToString();

                        if (S_ProductionOrderID != S_COO_POID)
                        {
                            //此条码和工单不匹配.
                            S_Result = MyMSG(List_Login.Language, "20050");
                            return S_Result;
                        }
                    }
                    else
                    {
                        //此条码和工单不匹配.
                        S_Result = MyMSG(List_Login.Language, "20050");
                        return S_Result;
                    }
                }


                DataSet dsCOF = v_PartSelect.GetluPODetailDef(Convert.ToInt32(S_ProductionOrderID), "COF");
                if (dsCOF != null && dsCOF.Tables.Count > 0 && dsCOF.Tables[0].Rows.Count > 0)
                {
                    COF = dsCOF.Tables[0].Rows[0]["Content"].ToString() == "1";
                }


                DataTable DT_SN = v_PartSelect.GetmesSerialNumber(mainSN).Tables[0];
                if (DT_SN.Rows.Count > 0)
                {
                    string S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                    DT_Unit = v_PartSelect.GetmesUnit(S_UnitID).Tables[0];
                    string S_StationID = List_Login.StationID.ToString();

                    string S_UnitStateID = "";

                    try
                    {
                        S_UnitStateID = GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID,
                            List_Login.LineID.ToString(), S_ProductionOrderID, "1");
                        if (S_UnitStateID == "")
                        {
                            S_UnitStateID = GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID,
                                List_Login.LineID.ToString(), S_ProductionOrderID, "2");
                        }
                        if (S_UnitStateID == "")
                        {
                            S_UnitStateID = GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID,
                                List_Login.LineID.ToString(), S_ProductionOrderID, "3");
                        }
                    }
                    catch
                    {
                    }


                    if (string.IsNullOrEmpty(S_UnitStateID))
                    {
                        //当前工站类型在配置的工艺流程中未配置,请检查工艺流程.
                        S_Result = MyMSG(List_Login.Language, "20203");
                        return S_Result;
                    }
                }
                else
                {
                    //条码:{0},不存在,请确认.
                    S_Result = MyMSG(List_Login.Language, "20242");
                    return S_Result;
                }

                if (!COF)
                {
                    //根据工艺路线判断状态
                    DataSet DsRout = v_PartSelect.GetRouteData(List_Login.LineID.ToString(), S_PartID,
                                                                "", S_ProductionOrderID);
                    if (DsRout == null || DsRout.Tables.Count == 0 || DsRout.Tables[0].Rows.Count == 0)
                    {
                        //料号未配置工艺流程路线.
                        S_Result = MyMSG(List_Login.Language, "20195");
                        return S_Result;
                    }

                    //2022-05-17 NG通过在路径检查中实现

                    //string RoutType = DsRout.Tables[0].Rows[0]["RouteType"].ToString();
                    //string StatusID = DT_Unit.Rows[0]["StatusID"].ToString();

                    ////无需检查NG 请设置 false
                    //if (List_Login.IsCheckNG == true)
                    //{
                    //    if (RoutType != "1" && StatusID != "1")
                    //    {
                    //        //此条码已NG. 
                    //        S_Result = MyMSG(List_Login.Language, "20036");
                    //        return S_Result;
                    //    }
                    //}
                }

                //2024-01-12  加入此逻辑
                //////////////////////////////////////////////////////
                /// //二次检查是否有指定线路  20231214
                
                string S_luUnitStateID = DT_Unit.Rows[0]["StatusID"].ToString();
                var tmpSecondRet = v_PartSelect.GetmesUnitStateSecond(S_PartID, PartFamilyID, "", List_Login.LineID.ToString(),
                    List_Login.StationTypeID, S_ProductionOrderID, S_luUnitStateID, mainSN);

                if (tmpSecondRet.StartsWith("Error", StringComparison.OrdinalIgnoreCase))
                {
                    S_Result = tmpSecondRet;
                    return S_Result;
                }

                string mS_UnitStateID = tmpSecondRet;
                if (string.IsNullOrEmpty(mS_UnitStateID))
                {
                    S_Result = MyMSG(List_Login.Language, "20203");
                    return S_Result;
                }
                /// //二次检查是否有指定线路  END

                string S_RouteCheck = v_PartSelect.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID,
                    List_Login.LineID.ToString(), DT_Unit, mainSN);
                if (S_RouteCheck != "1")
                {
                    if (S_RouteCheck == "20243")
                    {
                        string nowUnitStateID = v_PartSelect.GetSerialNumber2(mainSN).Tables[0].Rows[0]["UnitStateID"].ToString();
                        mesUnitState mesunitSateModel = v_mesUnitState.Get(Convert.ToInt32(nowUnitStateID));

                        S_Result = "Route Check Failure->"+ mainSN +":"+ mesunitSateModel.Description;
                        return S_Result;
                    }
                    else
                    {
                        S_Result = "Route Check Failure->" + MyMSG(List_Login.Language, S_RouteCheck);
                        return S_Result;
                    }
                }

                //调用通用过程
                string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + S_ProductionOrderID + "\"> </ProdOrder>";
                string xmlPart = "<Part PartID=\"" + S_PartID + "\"> </Part>";
                string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                string outString = string.Empty;
                v_PartSelect.uspCallProcedure("uspQCCheck", mainSN, xmlProdOrder, xmlPart, xmlStation, null, null, ref outString);
                if (outString != "1")
                {
                    S_Result = outString;
                    return S_Result;
                }

                return S_Result;
            }
            catch (Exception ex)
            {
                try
                {
                    if (OpenLogFile == "1")
                    {
                        TimeSpan ts = DateTime.Now - Date_Start;
                        double mill = ts.TotalMilliseconds;
                        string logPath = CreateServerDIR();
                        string msg = ex.ToString() + "Time：" + mill.ToString() + "ms";
                        File.AppendAllText(logPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".log",
                           DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + msg + "\r\n");
                    }
                }
                catch
                {

                }

                return S_Result = ex.ToString();
            }
        }


        public string ChildSN_Check(string S_SN, string S_ChildSN, int ChildPartID, LoginList List_Login, Tuple<BomInfo[], string> BomInfos, int SelectedPartID,int SelectedPOID)
        {
            string S_Result = "1";

            DateTime Date_Start = DateTime.Now;
            string outString1 = string.Empty;
            string mainSN = S_SN.Trim();

            string S_PartID = "";
            string S_POPartID = "";
            string PartFamilyID = "";
            string S_ProductionOrderID = "";
            string S_IsCheckPO = "1";

            Boolean COF = false;
            string OpenLogFile = System.Configuration.ConfigurationManager.AppSettings["OpenLogFile"];
            DataTable DT_Unit = new DataTable();
            List_Login.IsCheckPO = true;

            try
            {
                if (string.IsNullOrEmpty(S_ChildSN))
                {
                    S_Result = "子条码不能为空(child SN no allow null.)";
                    return S_Result;
                }

                if (BomInfos.Item1 == null || !BomInfos.Item1.Any() || !string.IsNullOrEmpty(BomInfos.Item2))
                {
                    S_Result = "请先设定组装工站信息(please set up assembly station information.)";
                    return S_Result;
                }

                if (string.IsNullOrEmpty(mainSN))
                {
                    //S_Result = "主条码不能为空(main SN no allow null.)";
                    //return S_Result;
                    S_ProductionOrderID = SelectedPOID.ToString();
                    S_PartID = SelectedPartID.ToString();
                }
                else
                {
                    DataSet dsMainSN = v_PartSelect.uspCallProcedure("uspGetBaseData", mainSN,
                        null, null, null, null, null, ref outString1);
                    if (outString1 != "1" || dsMainSN == null || dsMainSN.Tables[0].Rows.Count <= 0)
                    {
                        //条码不存在或者状态不符.
                        S_Result = MyMSG(List_Login.Language, "20119");
                        return S_Result;
                    }

                    DataTable dt = dsMainSN.Tables[0];
                    S_ProductionOrderID = dt.Rows[0]["ProductionOrderID"].ToString();
                    S_PartID = dt.Rows[0]["PartID"].ToString();
                    S_POPartID = dt.Rows[0]["PartID"].ToString();
                    PartFamilyID = dt.Rows[0]["PartFamilyID"].ToString();
                }

                var tmpBomInfo = BomInfos.Item1.Where(x => x.PartID == ChildPartID);
                if (!tmpBomInfo.Any())
                {
                    //20043
                    S_Result = MyMSG(List_Login.Language, "20043");
                    return S_Result;
                }
                var tmpRes = CheckInputBarcode(mainSN.Trim(), S_ChildSN, tmpBomInfo.ToList()[0], COF, List_Login, S_ProductionOrderID, S_PartID);
                if (tmpRes != "1")
                {
                    S_Result = tmpRes;
                    return S_Result;
                }
                return S_Result;
            }
            catch (Exception ex)
            {
                try
                {
                    if (OpenLogFile == "1")
                    {
                        TimeSpan ts = DateTime.Now - Date_Start;
                        double mill = ts.TotalMilliseconds;
                        string logPath = CreateServerDIR();
                        string msg = ex.ToString() + "Time：" + mill.ToString() + "ms";
                        File.AppendAllText(logPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".log",
                           DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + msg + "\r\n");
                    }
                }
                catch
                {

                }

                return S_Result = ex.ToString();
            }
        }

        public string MainSN_GetAssySN(string S_SN, LoginList List_Login)
        {
            string S_Result = "";

            DateTime Date_Start = DateTime.Now;
            string outString1 = string.Empty;
            string mainSN = S_SN.Trim();

            string S_PartID = "";
            string S_POPartID = "";
            string PartFamilyID = "";
            string S_ProductionOrderID = "";
            string S_IsCheckPO = "1";

            Boolean COF = false;
            string OpenLogFile = System.Configuration.ConfigurationManager.AppSettings["OpenLogFile"];
            DataTable DT_Unit = new DataTable();
            List_Login.IsCheckPO = true;

            try
            {
                if (mainSN == "")
                {
                    //"SN不能为空";
                    S_Result = MyMSG(List_Login.Language, "20119");
                    return S_Result;
                }

                DataSet dsMainSN;
                dsMainSN = v_PartSelect.uspCallProcedure("uspGetBaseData", mainSN,
                       null, null, null, null, null, ref outString1);
                if (outString1 != "1" || dsMainSN == null || dsMainSN.Tables[0].Rows.Count <= 0)
                {
                    //条码不存在或者状态不符.
                    S_Result = MyMSG(List_Login.Language, "20119");
                    return S_Result;
                }

                DataTable dt = dsMainSN.Tables[0];
                S_ProductionOrderID = dt.Rows[0]["ProductionOrderID"].ToString();
                S_PartID = dt.Rows[0]["PartID"].ToString();
                S_POPartID = dt.Rows[0]["PartID"].ToString();
                PartFamilyID = dt.Rows[0]["PartFamilyID"].ToString();



                //调用通用过程
                string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + S_ProductionOrderID + "\"> </ProdOrder>";
                string xmlPart = "<Part PartID=\"" + S_PartID + "\"> </Part>";
                string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                string outString = string.Empty;
                var dataSet = v_PartSelect.uspCallProcedure("uspGetAssembly", mainSN, xmlProdOrder, xmlPart, xmlStation, null, null, ref outString);
                if (outString != "1")
                {
                    S_Result = outString;
                    return S_Result;
                }

                if (dataSet == null || dataSet.Tables.Count <= 0 || dataSet.Tables[0].Rows.Count <= 0)
                {
                    S_Result = "Error : can't any child SN.";
                    return S_Result;
                }


                return S_Result = JsonConvert.SerializeObject(dataSet.Tables[0]);
            }
            catch (Exception ex)
            {
                try
                {
                    if (OpenLogFile == "1")
                    {
                        TimeSpan ts = DateTime.Now - Date_Start;
                        double mill = ts.TotalMilliseconds;
                        string logPath = CreateServerDIR();
                        string msg = ex.ToString() + "Time：" + mill.ToString() + "ms";
                        File.AppendAllText(logPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".log",
                           DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + msg + "\r\n");
                    }
                }
                catch
                {

                }

                return S_Result = ex.ToString();
            }
        }

        public string MainSN_OutFGSN(string S_SN, LoginList List_Login)
        {
            string mainSN = S_SN;
            DateTime Date_Start = DateTime.Now;
            string OpenLogFile = System.Configuration.ConfigurationManager.AppSettings["OpenLogFile"];

            try
            {
                //获取 SN
                string
                S_Sql = "select * from mesSerialNumber where Value='" + mainSN + "'";
                DataTable DT_GetSN = SqlServerHelper.Data_Table(S_Sql);
                if (DT_GetSN.Rows.Count == 0)
                {
                    S_Sql = @"SELECT  B.[Value] SN
                                FROM mesUnitDetail A JOIN mesSerialNumber B ON A.UnitID = B.UnitID
                             WHERE A.reserved_01 = '" + mainSN + "' AND A.reserved_03 = 1";
                    DT_GetSN = SqlServerHelper.Data_Table(S_Sql);

                    mainSN = "";
                    if (DT_GetSN.Rows.Count > 0)
                    {
                        mainSN = DT_GetSN.Rows[0]["SN"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (OpenLogFile == "1")
                    {
                        TimeSpan ts = DateTime.Now - Date_Start;
                        double mill = ts.TotalMilliseconds;
                        string logPath = CreateServerDIR();
                        string msg = ex.ToString() + "Time：" + mill.ToString() + "ms";
                        File.AppendAllText(logPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".log",
                           DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + msg + "\r\n");
                    }
                }
                catch
                {

                }
               
            }

            return mainSN;
        }

        public string MainSN_Check_OutCCCode(string S_SN, LoginList List_Login,out string S_CCCode)
        {
            string S_Result = "1";
            S_CCCode = "";

            DateTime Date_Start = DateTime.Now;
            string outString1 = string.Empty;
            string mainSN = S_SN.Trim();

            string S_PartID = "";
            string S_POPartID = "";
            string PartFamilyID = "";
            string S_ProductionOrderID = "";
            string S_IsCheckPO = "1";

            Boolean COF = false;
            string OpenLogFile = System.Configuration.ConfigurationManager.AppSettings["OpenLogFile"];
            DataTable DT_Unit = new DataTable();
            List_Login.IsCheckPO = true;

            try
            {
                if (mainSN == "")
                {
                    //"SN不能为空";
                    S_Result = MyMSG(List_Login.Language, "20119");
                    return S_Result;
                }

                string S_Sql = "SELECT StationTypeID FROM mesStation WHERE ID=" + List_Login.StationID;
                DataTable DT_StationType = SqlServerHelper.Data_Table(S_Sql);
                List_Login.StationTypeID = Convert.ToInt32(DT_StationType.Rows[0]["StationTypeID"].ToString());

                DataSet dsStationDetail = v_PartSelect.GetPLCSeting("", List_Login.StationID.ToString());
                DataSet DS_StationTypeDef = v_PartSelect.MESGetStationTypeParameter(List_Login.StationTypeID);

                DataRow[] DR_IsCheckPO = DS_StationTypeDef.Tables[0].Select("Description='IsCheckPO'");
                if (DR_IsCheckPO.Length > 0)
                {
                    S_IsCheckPO = DR_IsCheckPO[0]["Content"].ToString();

                    if (S_IsCheckPO == "1")
                    {
                        DataRow[] DR_Child_IsCheckPO = dsStationDetail.Tables[0].Select("Name='IsCheckPO'");
                        if (DR_Child_IsCheckPO.Length > 0) S_IsCheckPO = DR_Child_IsCheckPO[0]["Value"].ToString(); else S_IsCheckPO = "1";
                    }
                }
                else
                {
                    S_IsCheckPO = "1";
                }

                if (S_IsCheckPO != "1") { List_Login.IsCheckPO = false; }


                //获取 SN
                S_Sql = "select * from mesSerialNumber where Value='" + mainSN + "'";
                DataTable DT_GetSN = SqlServerHelper.Data_Table(S_Sql);
                if (DT_GetSN.Rows.Count == 0)
                {
                    S_Sql = @"SELECT  B.[Value] SN
                                FROM mesUnitDetail A JOIN mesSerialNumber B ON A.UnitID = B.UnitID
                             WHERE A.reserved_01 = '" + mainSN + "' AND A.reserved_03 = 1";
                    DT_GetSN = SqlServerHelper.Data_Table(S_Sql);

                    mainSN = "";
                    if (DT_GetSN.Rows.Count > 0)
                    {
                        mainSN = DT_GetSN.Rows[0]["SN"].ToString();
                    }
                }

                DataSet dsMainSN;
                dsMainSN = v_PartSelect.uspCallProcedure("uspGetBaseData", mainSN,
                       null, null, null, null, null, ref outString1);
                if (outString1 != "1" || dsMainSN == null || dsMainSN.Tables[0].Rows.Count <= 0)
                {
                    //条码不存在或者状态不符.
                    S_Result = MyMSG(List_Login.Language, "20119");
                    return S_Result;
                }

                DataTable dt = dsMainSN.Tables[0];
                S_ProductionOrderID = dt.Rows[0]["ProductionOrderID"].ToString();
                S_PartID = dt.Rows[0]["PartID"].ToString();
                S_POPartID = dt.Rows[0]["PartID"].ToString();
                PartFamilyID = dt.Rows[0]["PartFamilyID"].ToString();

                //获取CCCode
                S_Sql = @"SELECT  * FROM mesPartDetail A JOIN luPartDetailDef B ON A.PartDetailDefID=B.ID
                        WHERE B.[Description]='CCCC Code' AND A.PartID='" + S_PartID + "'";
                DataTable DT_Color = SqlServerHelper.Data_Table(S_Sql);
                if (DT_Color.Rows.Count > 0)
                {
                    S_CCCode = DT_Color.Rows[0]["Content"].ToString();
                }

                if (S_CCCode == "")
                {
                    S_Result = "CCCC Code Can't be empty";
                    return S_Result;
                }

                if (List_Login.IsCheckPO == true)
                {
                    S_Sql = "select Value  from mesStationConfigSetting where StationID='" + List_Login.StationID + "' and Name='POID'";
                    DataTable DT_COO_POID = SqlServerHelper.Data_Table(S_Sql);

                    if (DT_COO_POID.Rows.Count > 0)
                    {
                        string S_COO_POID = DT_COO_POID.Rows[0]["Value"].ToString();

                        if (S_ProductionOrderID != S_COO_POID)
                        {
                            //此条码和工单不匹配.
                            S_Result = MyMSG(List_Login.Language, "20050");
                            return S_Result;
                        }
                    }
                    else
                    {
                        //此条码和工单不匹配.
                        S_Result = MyMSG(List_Login.Language, "20050");
                        return S_Result;
                    }
                }


                DataSet dsCOF = v_PartSelect.GetluPODetailDef(Convert.ToInt32(S_ProductionOrderID), "COF");
                if (dsCOF != null && dsCOF.Tables.Count > 0 && dsCOF.Tables[0].Rows.Count > 0)
                {
                    COF = dsCOF.Tables[0].Rows[0]["Content"].ToString() == "1";
                }


                DataTable DT_SN = v_PartSelect.GetmesSerialNumber(mainSN).Tables[0];
                if (DT_SN.Rows.Count > 0)
                {
                    string S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                    DT_Unit = v_PartSelect.GetmesUnit(S_UnitID).Tables[0];
                    string S_StationID = List_Login.StationID.ToString();

                    string S_UnitStateID = "";

                    try
                    {
                        S_UnitStateID = GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID,
                            List_Login.LineID.ToString(), S_ProductionOrderID, "1");
                        if (S_UnitStateID == "")
                        {
                            S_UnitStateID = GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID,
                                List_Login.LineID.ToString(), S_ProductionOrderID, "2");
                        }
                        if (S_UnitStateID == "")
                        {
                            S_UnitStateID = GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID,
                                List_Login.LineID.ToString(), S_ProductionOrderID, "3");
                        }
                    }
                    catch
                    {
                    }


                    if (string.IsNullOrEmpty(S_UnitStateID))
                    {
                        //当前工站类型在配置的工艺流程中未配置,请检查工艺流程.
                        S_Result = MyMSG(List_Login.Language, "20203");
                        return S_Result;
                    }
                }
                else
                {
                    //条码:{0},不存在,请确认.
                    S_Result = MyMSG(List_Login.Language, "20242");
                    return S_Result;
                }

                if (!COF)
                {
                    //根据工艺路线判断状态
                    DataSet DsRout = v_PartSelect.GetRouteData(List_Login.LineID.ToString(), S_PartID,
                                                                "", S_ProductionOrderID);
                    if (DsRout == null || DsRout.Tables.Count == 0 || DsRout.Tables[0].Rows.Count == 0)
                    {
                        //料号未配置工艺流程路线.
                        S_Result = MyMSG(List_Login.Language, "20195");
                        return S_Result;
                    }

                    //2022-05-17 NG通过在路径检查中实现

                    //string RoutType = DsRout.Tables[0].Rows[0]["RouteType"].ToString();
                    //string StatusID = DT_Unit.Rows[0]["StatusID"].ToString();

                    ////无需检查NG 请设置 false
                    //if (List_Login.IsCheckNG == true)
                    //{
                    //    if (RoutType != "1" && StatusID != "1")
                    //    {
                    //        //此条码已NG. 
                    //        S_Result = MyMSG(List_Login.Language, "20036");
                    //        return S_Result;
                    //    }
                    //}
                }

                string S_RouteCheck = v_PartSelect.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID,
                    List_Login.LineID.ToString(), DT_Unit, mainSN);
                if (S_RouteCheck != "1")
                {
                    if (S_RouteCheck == "20243")
                    {
                        string nowUnitStateID = v_PartSelect.GetSerialNumber2(mainSN).Tables[0].Rows[0]["UnitStateID"].ToString();
                        mesUnitState mesunitSateModel = v_mesUnitState.Get(Convert.ToInt32(nowUnitStateID));

                        S_Result = "Route Check Failure->" + mainSN + ":" + mesunitSateModel.Description;
                        return S_Result;
                    }
                    else
                    {
                        S_Result = "Route Check Failure->" + MyMSG(List_Login.Language, S_RouteCheck);
                        return S_Result;
                    }
                }

                //调用通用过程
                string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + S_ProductionOrderID + "\"> </ProdOrder>";
                string xmlPart = "<Part PartID=\"" + S_PartID + "\"> </Part>";
                string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                string outString = string.Empty;
                v_PartSelect.uspCallProcedure("uspQCCheck", mainSN, xmlProdOrder, xmlPart, xmlStation, null, null, ref outString);
                if (outString != "1")
                {
                    S_Result = outString;
                    return S_Result;
                }
               
                return S_Result;
            }
            catch (Exception ex)
            {
                try
                {
                    if (OpenLogFile == "1")
                    {
                        TimeSpan ts = DateTime.Now - Date_Start;
                        double mill = ts.TotalMilliseconds;
                        string logPath = CreateServerDIR();
                        string msg = ex.ToString() + "Time：" + mill.ToString() + "ms";
                        File.AppendAllText(logPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".log",
                           DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + msg + "\r\n");
                    }
                }
                catch { }

                return S_Result = ex.ToString();
            }
        }

        public string MainSN_Check_OutCCCodeV2(string S_SN, LoginList List_Login)
        {
            string S_Result = "1";
            string S_CCCode = "";
            DateTime Date_Start = DateTime.Now;
            string outString = string.Empty;
            string mainSN = S_SN.Trim();
            string S_PartID = "";
            string S_ProductionOrderID = "";

            Boolean COF = false;
            string OpenLogFile = System.Configuration.ConfigurationManager.AppSettings["OpenLogFile"];
            DataTable DT_Unit = new DataTable();
            List_Login.IsCheckPO = true;

            string result;
            try
            {
                if (mainSN == "")
                {
                    S_Result = this.MyMSG(List_Login.Language, "20119");
                    result = S_Result + ";" + S_CCCode;
                }
                else
                {
                    DataTable DT_StationType = SqlServerHelper.Data_Table("SELECT StationTypeID FROM mesStation WHERE ID=" + List_Login.StationID);
                    List_Login.StationTypeID=Convert.ToInt32(DT_StationType.Rows[0]["StationTypeID"].ToString());
                    DataSet dsStationDetail = this.v_PartSelect.GetPLCSeting("", List_Login.StationID.ToString());
                    DataRow[] DR_IsCheckPO = this.v_PartSelect.MESGetStationTypeParameter(List_Login.StationTypeID).Tables[0].Select("Description='IsCheckPO'");
                    string S_IsCheckPO;
                    if (DR_IsCheckPO.Length != 0)
                    {
                        S_IsCheckPO = DR_IsCheckPO[0]["Content"].ToString();
                        if (S_IsCheckPO == "1")
                        {
                            DataRow[] DR_Child_IsCheckPO = dsStationDetail.Tables[0].Select("Name='IsCheckPO'");
                            if (DR_Child_IsCheckPO.Length != 0)
                            {
                                S_IsCheckPO = DR_Child_IsCheckPO[0]["Value"].ToString();
                            }
                            else
                            {
                                S_IsCheckPO = "1";
                            }
                        }
                    }
                    else
                    {
                        S_IsCheckPO = "1";
                    }
                    if (S_IsCheckPO != "1") { List_Login.IsCheckPO = false; }

                    DataTable DT_GetSN = SqlServerHelper.Data_Table("select * from mesSerialNumber where Value='" + mainSN + "'");
                    if (DT_GetSN.Rows.Count == 0)
                    {
                        DT_GetSN = SqlServerHelper.Data_Table("SELECT  B.[Value] SN FROM mesUnitDetail A JOIN mesSerialNumber B ON A.UnitID = B.UnitID" +
                            " WHERE A.reserved_01 = '" + mainSN + "' AND A.reserved_03 = 1");
                        mainSN = "";
                        if (DT_GetSN.Rows.Count > 0)
                        {
                            mainSN = DT_GetSN.Rows[0]["SN"].ToString();
                        }
                    }
                    DataSet dsMainSN = this.v_PartSelect.uspCallProcedure("uspGetBaseData", mainSN, null, null, null, null, null, ref outString);
                    if (outString != "1" || dsMainSN == null || dsMainSN.Tables[0].Rows.Count <= 0)
                    {
                        S_Result = this.MyMSG(List_Login.Language, "20119");
                        result = S_Result + ";" + S_CCCode;
                    }
                    else
                    {
                        DataTable expr_289 = dsMainSN.Tables[0];
                        S_ProductionOrderID = expr_289.Rows[0]["ProductionOrderID"].ToString();
                        S_PartID = expr_289.Rows[0]["PartID"].ToString();
                        expr_289.Rows[0]["PartID"].ToString();
                        string PartFamilyID = expr_289.Rows[0]["PartFamilyID"].ToString();
                        //DataTable DT_Color = SqlServerHelper.Data_Table("SELECT  * FROM mesPartDetail A JOIN luPartDetailDef B ON A.PartDetailDefID=B.ID" +
                        //    "                       WHERE B.[Description]='CCCC Code' AND A.PartID='" + S_PartID + "'");

                        string S_Sql = 
                       @"SELECT  B.[Content] FROM mesProductionOrder A
                            JOIN mesProductionOrderDetail B ON A.ID = B.ProductionOrderID    
                            JOIN luProductionOrderDetailDef C ON B.ProductionOrderDetailDefID = C.ID
                        WHERE A.PartID = '"+ S_PartID + @"' AND C.[Description]='7ECode'	 
                        ";

                        DataTable DT_Color = SqlServerHelper.Data_Table(S_Sql);
                        if (DT_Color.Rows.Count < 1)
                        {
                            S_Sql =
                            @"
                                SELECT A.[Content] FROM mesPartDetail A JOIN luPartDetailDef B ON A.PartDetailDefID=B.ID
                                    WHERE B.[Description]='7ECode' AND A.PartID='"+ S_PartID + @"'
                             ";
                            DT_Color = SqlServerHelper.Data_Table(S_Sql);
                        }


                        if (DT_Color.Rows.Count > 0)
                        {
                            S_CCCode = DT_Color.Rows[0]["Content"].ToString();
                        }
                        if (S_CCCode == "")
                        {
                            S_Result = "CCCC Code Can't be empty";
                            result = S_Result + ";" + S_CCCode;
                        }
                        else
                        {
                            if (List_Login.IsCheckPO)
                            {
                                DataTable DT_COO_POID = SqlServerHelper.Data_Table("select Value  from mesStationConfigSetting where StationID='" + List_Login.StationID + "' and Name='POID'");
                                if (DT_COO_POID.Rows.Count <= 0)
                                {
                                    S_Result = this.MyMSG(List_Login.Language, "20050");
                                    result = S_Result + ";" + S_CCCode;
                                    return result;
                                }
                                string S_COO_POID = DT_COO_POID.Rows[0]["Value"].ToString();
                                if (S_ProductionOrderID != S_COO_POID)
                                {
                                    S_Result = this.MyMSG(List_Login.Language, "20050");
                                    result = S_Result + ";" + S_CCCode;
                                    return result;
                                }
                            }
                            DataSet dsCOF = this.v_PartSelect.GetluPODetailDef(Convert.ToInt32(S_ProductionOrderID), "COF");
                            if (dsCOF != null && dsCOF.Tables.Count > 0 && dsCOF.Tables[0].Rows.Count > 0)
                            {
                                COF = (dsCOF.Tables[0].Rows[0]["Content"].ToString() == "1");
                            }
                            DataTable DT_SN = this.v_PartSelect.GetmesSerialNumber(mainSN).Tables[0];
                            if (DT_SN.Rows.Count > 0)
                            {
                                string S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                                DT_Unit = this.v_PartSelect.GetmesUnit(S_UnitID).Tables[0];
                                List_Login.StationID.ToString();
                                string S_UnitStateID = "";
                                try
                                {
                                    S_UnitStateID = this.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID, List_Login.LineID.ToString(), S_ProductionOrderID, "1");
                                    if (S_UnitStateID == "")
                                    {
                                        S_UnitStateID = this.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID, List_Login.LineID.ToString(), S_ProductionOrderID, "2");
                                    }
                                    if (S_UnitStateID == "")
                                    {
                                        S_UnitStateID = this.GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID, List_Login.LineID.ToString(), S_ProductionOrderID, "3");
                                    }
                                }
                                catch
                                {
                                }
                                if (string.IsNullOrEmpty(S_UnitStateID))
                                {
                                    S_Result = this.MyMSG(List_Login.Language, "20203");
                                    result = S_Result + ";" + S_CCCode;
                                }
                                else
                                {
                                    if (!COF)
                                    {
                                        DataSet DsRout = this.v_PartSelect.GetRouteData(List_Login.LineID.ToString(), S_PartID, "", S_ProductionOrderID);
                                        if (DsRout == null || DsRout.Tables.Count == 0 || DsRout.Tables[0].Rows.Count == 0)
                                        {
                                            S_Result = this.MyMSG(List_Login.Language, "20195");
                                            result = S_Result + ";" + S_CCCode;
                                            return result;
                                        }
                                    }

                                    //2024-01-12  加入此逻辑
                                    //////////////////////////////////////////////////////
                                    /// //二次检查是否有指定线路  20231214

                                    string S_luUnitStateID = DT_Unit.Rows[0]["StatusID"].ToString();
                                    var tmpSecondRet = v_PartSelect.GetmesUnitStateSecond(S_PartID, PartFamilyID, "", List_Login.LineID.ToString(),
                                        List_Login.StationTypeID, S_ProductionOrderID, S_luUnitStateID, mainSN);

                                    if (tmpSecondRet.StartsWith("Error", StringComparison.OrdinalIgnoreCase))
                                    {
                                        S_Result = tmpSecondRet;
                                        return S_Result;
                                    }

                                    string mS_UnitStateID = tmpSecondRet;
                                    if (string.IsNullOrEmpty(mS_UnitStateID))
                                    {
                                        S_Result = MyMSG(List_Login.Language, "20203");
                                        return S_Result;
                                    }
                                    /// //二次检查是否有指定线路  END

                                    string S_RouteCheck = this.v_PartSelect.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID, List_Login.LineID.ToString(), DT_Unit, mainSN);
                                    if (S_RouteCheck != "1")
                                    {
                                        if (S_RouteCheck == "20243")
                                        {
                                            string nowUnitStateID = this.v_PartSelect.GetSerialNumber2(mainSN).Tables[0].Rows[0]["UnitStateID"].ToString();
                                            mesUnitState mesunitSateModel = this.v_mesUnitState.Get(Convert.ToInt32(nowUnitStateID));
                                            S_Result = "Route Check Failure->" + mainSN + ":" + mesunitSateModel.Description;
                                            result = S_Result + ";" + S_CCCode;
                                        }
                                        else
                                        {
                                            S_Result = "Route Check Failure->" + this.MyMSG(List_Login.Language, S_RouteCheck);
                                            result = S_Result + ";" + S_CCCode;
                                        }
                                    }
                                    else
                                    {
                                        string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + S_ProductionOrderID + "\"> </ProdOrder>";
                                        string xmlPart = "<Part PartID=\"" + S_PartID + "\"> </Part>";
                                        string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                                        string outString2 = string.Empty;
                                        this.v_PartSelect.uspCallProcedure("uspQCCheck", mainSN, xmlProdOrder, xmlPart, xmlStation, null, null, ref outString2);
                                        if (outString2 != "1")
                                        {
                                            S_Result = outString2;
                                            result = S_Result + ";" + S_CCCode;
                                        }
                                        else
                                        {
                                            result = S_Result + ";" + S_CCCode;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                S_Result = this.MyMSG(List_Login.Language, "20242");
                                result = S_Result + ";" + S_CCCode;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (OpenLogFile == "1")
                    {
                        double mill = (DateTime.Now - Date_Start).TotalMilliseconds;
                        string arg_85F_0 = this.CreateServerDIR();
                        string msg = ex.ToString() + "Time：" + mill.ToString() + "ms";
                        File.AppendAllText(arg_85F_0 + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".log", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + msg + "\r\n");
                    }
                }
                catch
                { }

                S_Result = (result = ex.ToString() + ";" + S_CCCode);
            }
            return result;
        }


        public string Update_SFC(string S_SN,int I_StatusID, LoginList List_Login, string S_DefectCode)
        {
            string S_Result = "1";

            DateTime Date_Start = DateTime.Now;
            string outString1 = string.Empty;
            string mainSN = S_SN.Trim();

            string S_PartID = "";
            string S_POPartID = "";
            string PartFamilyID = "";
            string S_ProductionOrderID = "";
            string S_IsCheckPO = "1";

            string S_UnitStateID = "";
            //string S_luUnitStateID = "1";

            Boolean COF = false;
            string OpenLogFile = System.Configuration.ConfigurationManager.AppSettings["OpenLogFile"];
            DataTable DT_Unit = new DataTable();
            DataTable DT_Defect = new DataTable();
            string[] List_DefectCode=null;

            if (I_StatusID != 1)
            {
                if (S_DefectCode == "")
                {
                    S_Result = "NG状态，不良代码不为空(NG state, DefectCode cannot be empty)";
                    return S_Result;
                }
            }


            if (I_StatusID!=1 && S_DefectCode != "")
            {
                List_DefectCode = S_DefectCode.Split(',');
                //S_luUnitStateID = "2";
                for (int i = 0; i < List_DefectCode.Length; i++)
                {
                    string S_Dcode = List_DefectCode[i].Trim();
                    string S_Sql =
                    @"SELECT * FROM luDefect WHERE DefectTypeID=
                    (
	                    SELECT E.ID FROM mesUnit A 
		                    JOIN mesSerialNumber B ON A.ID=B.UnitID
		                    JOIN mesPart C ON C.ID=A.PartID
		                    JOIN luPartFamily D ON D.ID=C.PartFamilyID
		                    JOIN luPartFamilyType DD ON DD.ID=D.PartFamilyTypeID		                    
		                    JOIN luDefectType E ON RTRIM(LTRIM(E.[Description]))=RTRIM(LTRIM(DD.Name))     
	                    WHERE B.[Value]='" + S_SN + @"'  
                    ) AND DefectCode='" + S_Dcode + "'";

                    DT_Defect = SqlServerHelper.ExecuteDataTable(S_Sql);
                    if (DT_Defect.Rows.Count == 0)
                    {
                        S_Result = "SFC系统没有这个不良代码(SFC systems do not have this DefectCode):" + S_Dcode;
                        return S_Result;
                    }
                }
            }

            try
            {
                if (mainSN == "")
                {
                    //"SN不能为空";
                    S_Result = MyMSG(List_Login.Language, "20119");
                    return S_Result;
                }

                string S_Sql = "SELECT StationTypeID FROM mesStation WHERE ID=" + List_Login.StationID;
                DataTable DT_StationType = SqlServerHelper.Data_Table(S_Sql);
                List_Login.StationTypeID = Convert.ToInt32(DT_StationType.Rows[0]["StationTypeID"].ToString());

                DataSet dsStationDetail = v_PartSelect.GetPLCSeting("", List_Login.StationID.ToString());
                DataSet DS_StationTypeDef = v_PartSelect.MESGetStationTypeParameter(List_Login.StationTypeID);

                DataRow[] DR_IsCheckPO = DS_StationTypeDef.Tables[0].Select("Description='IsCheckPO'");
                if (DR_IsCheckPO.Length > 0)
                {
                    S_IsCheckPO = DR_IsCheckPO[0]["Content"].ToString();

                    if (S_IsCheckPO == "1")
                    {
                        DataRow[] DR_Child_IsCheckPO = dsStationDetail.Tables[0].Select("Name='IsCheckPO'");
                        if (DR_Child_IsCheckPO.Length > 0) S_IsCheckPO = DR_Child_IsCheckPO[0]["Value"].ToString(); else S_IsCheckPO = "1";
                    }
                }
                else
                {
                    S_IsCheckPO = "1";
                }

                if (S_IsCheckPO != "1") { List_Login.IsCheckPO = false; }

                //获取 SN
                S_Sql = "select * from mesSerialNumber where Value='" + mainSN + "'";
                DataTable DT_GetSN = SqlServerHelper.Data_Table(S_Sql);
                if (DT_GetSN.Rows.Count == 0)
                {
                    S_Sql = @"SELECT  B.[Value] SN
                                FROM mesUnitDetail A JOIN mesSerialNumber B ON A.UnitID = B.UnitID
                             WHERE A.reserved_01 = '" + mainSN + "' AND A.reserved_03 = 1";
                    DT_GetSN = SqlServerHelper.Data_Table(S_Sql);

                    mainSN = "";
                    if (DT_GetSN.Rows.Count > 0)
                    {
                        mainSN = DT_GetSN.Rows[0]["SN"].ToString();
                    }
                }


                DataSet dsMainSN;
                dsMainSN = v_PartSelect.uspCallProcedure("uspGetBaseData", mainSN,
                       null, null, null, null, null, ref outString1);
                if (outString1 != "1" || dsMainSN == null || dsMainSN.Tables[0].Rows.Count <= 0)
                {
                    //条码不存在或者状态不符.
                    S_Result = MyMSG(List_Login.Language, "20119");
                    return S_Result;
                }

                DataTable dt = dsMainSN.Tables[0];
                S_ProductionOrderID = dt.Rows[0]["ProductionOrderID"].ToString();
                S_PartID = dt.Rows[0]["PartID"].ToString();
                S_POPartID = dt.Rows[0]["PartID"].ToString();
                PartFamilyID = dt.Rows[0]["PartFamilyID"].ToString();

                if (List_Login.IsCheckPO == true)
                {
                    S_Sql = "select Value  from mesStationConfigSetting where StationID='" + List_Login.StationID + "' and Name='POID'";
                    DataTable DT_COO_POID = SqlServerHelper.Data_Table(S_Sql);
                   
                    if (DT_COO_POID.Rows.Count > 0)
                    {
                        string S_COO_POID = DT_COO_POID.Rows[0]["Value"].ToString();

                        if (S_ProductionOrderID != S_COO_POID)
                        {
                            //此条码和工单不匹配.
                            S_Result = MyMSG(List_Login.Language, "20050");
                            return S_Result;
                        }
                    }
                    else
                    {
                        //此条码和工单不匹配.
                        S_Result = MyMSG(List_Login.Language, "20050");
                        return S_Result;
                    }
                }


                DataSet dsCOF = v_PartSelect.GetluPODetailDef(Convert.ToInt32(S_ProductionOrderID), "COF");
                if (dsCOF != null && dsCOF.Tables.Count > 0 && dsCOF.Tables[0].Rows.Count > 0)
                {
                    COF = dsCOF.Tables[0].Rows[0]["Content"].ToString() == "1";
                }


                DataTable DT_SN = v_PartSelect.GetmesSerialNumber(mainSN).Tables[0];
                if (DT_SN.Rows.Count > 0)
                {
                    string S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                    DT_Unit = v_PartSelect.GetmesUnit(S_UnitID).Tables[0];
                    string S_StationID = List_Login.StationID.ToString();

                    try
                    {
                        S_UnitStateID = GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID,
                            List_Login.LineID.ToString(), S_ProductionOrderID, I_StatusID.ToString());
                        //if (S_UnitStateID == "")
                        //{
                        //    S_UnitStateID = GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID,
                        //        List_Login.LineID.ToString(), S_ProductionOrderID, "2");
                        //}
                        //if (S_UnitStateID == "")
                        //{
                        //    S_UnitStateID = GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID,
                        //        List_Login.LineID.ToString(), S_ProductionOrderID, "3");
                        //}
                    }
                    catch
                    {
                    }

                    if (string.IsNullOrEmpty(S_UnitStateID))
                    {
                        //当前工站类型在配置的工艺流程中未配置,请检查工艺流程.
                        S_Result = MyMSG(List_Login.Language, "20203");
                        return S_Result;
                    }
                }
                else
                {
                    //条码:{0},不存在,请确认.
                    S_Result = MyMSG(List_Login.Language, "20242");
                    return S_Result;
                }

                if (!COF)
                {
                    //根据工艺路线判断状态
                    DataSet DsRout = v_PartSelect.GetRouteData(List_Login.LineID.ToString(), S_PartID,
                                                                "", S_ProductionOrderID);
                    if (DsRout == null || DsRout.Tables.Count == 0 || DsRout.Tables[0].Rows.Count == 0)
                    {
                        //料号未配置工艺流程路线.
                        S_Result = MyMSG(List_Login.Language, "20195");
                        return S_Result;
                    }

                    string RoutType = DsRout.Tables[0].Rows[0]["RouteType"].ToString();
                    string StatusID = DT_Unit.Rows[0]["StatusID"].ToString();

                    //无需检查NG 请设置 false
                    if (List_Login.IsCheckNG == true)
                    {
                        if (RoutType != "1" && StatusID != "1")
                        {
                            //此条码已NG. 
                            S_Result = MyMSG(List_Login.Language, "20036");
                            return S_Result;
                        }
                    }
                }

                //2024-01-12  加入此逻辑
                //////////////////////////////////////////////////////
                /// //二次检查是否有指定线路  20231214

                string S_luUnitStateID = I_StatusID.ToString();
                var tmpSecondRet = v_PartSelect.GetmesUnitStateSecond(S_PartID, PartFamilyID, "", List_Login.LineID.ToString(),
                    List_Login.StationTypeID, S_ProductionOrderID, S_luUnitStateID, mainSN);

                if (tmpSecondRet.StartsWith("Error", StringComparison.OrdinalIgnoreCase))
                {
                    S_Result = tmpSecondRet;
                    return S_Result;
                }

                string mS_UnitStateID = tmpSecondRet;
                if (string.IsNullOrEmpty(mS_UnitStateID))
                {
                    S_Result = MyMSG(List_Login.Language, "20203");
                    return S_Result;
                }
                /// //二次检查是否有指定线路  END


                string S_RouteCheck = v_PartSelect.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID,
                    List_Login.LineID.ToString(), DT_Unit, mainSN);
                if (S_RouteCheck != "1")
                {
                    if (S_RouteCheck == "20243")
                    {
                        string nowUnitStateID = v_PartSelect.GetSerialNumber2(mainSN).Tables[0].Rows[0]["UnitStateID"].ToString();
                        mesUnitState mesunitSateModel = v_mesUnitState.Get(Convert.ToInt32(nowUnitStateID));

                        S_Result = "Route Check Failure->" + mainSN + ":" + mesunitSateModel.Description;
                        return S_Result;
                    }
                    else
                    {
                        S_Result = "Route Check Failure->" + MyMSG(List_Login.Language, S_RouteCheck);
                        return S_Result;
                    }
                }

                //调用通用过程
                string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + S_ProductionOrderID + "\"> </ProdOrder>";
                string xmlPart = "<Part PartID=\"" + S_PartID + "\"> </Part>";
                string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                string outString = string.Empty;
                v_PartSelect.uspCallProcedure("uspQCCheck", mainSN, xmlProdOrder, xmlPart, xmlStation, null, null, ref outString);
                if (outString != "1")
                {
                    S_Result = outString;
                    return S_Result;
                }

                List<mesUnit> List_mesUnit = new List<mesUnit>();
                List<mesHistory> List_mesHistory = new List<mesHistory>();
                List<mesUnitDefect> List_mesUnitDefect = new List<mesUnitDefect>();

                DataTable DT_UPUnitID = v_PartSelect.Get_UnitID(mainSN).Tables[0];

                mesUnit v_mesUnit = new mesUnit();
                v_mesUnit.ID = Convert.ToInt32(DT_UPUnitID.Rows[0]["UnitID"].ToString());
                //v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                //2024-01-12 2024版画图
                v_mesUnit.UnitStateID = Convert.ToInt32(mS_UnitStateID);
                v_mesUnit.StatusID = I_StatusID;
                v_mesUnit.StationID = List_Login.StationID;
                v_mesUnit.EmployeeID = List_Login.EmployeeID;
                v_mesUnit.LastUpdate = DateTime.Now;
                v_mesUnit.ProductionOrderID = Convert.ToInt32(S_ProductionOrderID);
                //修改 Unit
                List_mesUnit.Add(v_mesUnit);

                mesHistory v_mesHistory = new mesHistory();
                v_mesHistory.UnitID = Convert.ToInt32(v_mesUnit.ID);
                //v_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                //2024-01-12 2024版画图
                v_mesHistory.UnitStateID = Convert.ToInt32(mS_UnitStateID);
                v_mesHistory.EmployeeID = List_Login.EmployeeID;
                v_mesHistory.StationID = List_Login.StationID;
                v_mesHistory.EnterTime = DateTime.Now;
                v_mesHistory.ExitTime = DateTime.Now;
                v_mesHistory.ProductionOrderID = Convert.ToInt32(S_ProductionOrderID);
                v_mesHistory.PartID = Convert.ToInt32(S_POPartID);
                v_mesHistory.LooperCount = 1;
                v_mesHistory.StatusID = I_StatusID;

                List_mesHistory.Add(v_mesHistory);

                if (S_DefectCode != "1" && S_DefectCode != "" && I_StatusID !=1)
                {
                    string S_DefectTypeID = DT_Defect.Rows[0]["DefectTypeID"].ToString();
                    for (int i = 0; i < List_DefectCode.Length; i++)
                    {
                        string S_Dcode = List_DefectCode[i].Trim();
                        
                        S_Sql = "SELECT * FROM luDefect WHERE DefectTypeID ='"+ S_DefectTypeID + "' and DefectCode='"+ S_Dcode + "'";
                        DT_Defect = SqlServerHelper.Data_Table(S_Sql);

                        int I_DefectID = Convert.ToInt32(DT_Defect.Rows[0]["Id"].ToString());
                        mesUnitDefect v_mesUnitDefect = new mesUnitDefect();

                        v_mesUnitDefect.UnitID = v_mesUnit.ID;
                        v_mesUnitDefect.DefectID = I_DefectID;
                        v_mesUnitDefect.StationID = List_Login.StationID;
                        v_mesUnitDefect.EmployeeID = List_Login.EmployeeID;

                        List_mesUnitDefect.Add(v_mesUnitDefect);
                    }

                }

                string ReturnValue = v_DataCommitDAL.SubmitDataUHD(List_mesUnit, List_mesHistory, List_mesUnitDefect);
                if (ReturnValue != "OK")
                {
                    S_Result = ReturnValue;
                    return S_Result;
                }

                return S_Result;
            }
            catch (Exception ex)
            {
                try
                {
                    if (OpenLogFile == "1")
                    {
                        TimeSpan ts = DateTime.Now - Date_Start;
                        double mill = ts.TotalMilliseconds;
                        string logPath = CreateServerDIR();
                        string msg = ex.ToString() + "Time：" + mill.ToString() + "ms";
                        File.AppendAllText(logPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".log",
                           DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + msg + "\r\n");
                    }
                }
                catch
                { }

                return S_Result = ex.ToString();
            }
        }



        public string Update_Assy_SFC(string S_SN, int I_StatusID, LoginList List_Login, string S_DefectCode, string S_ChildCode, Tuple<BomInfo[], string> BomInfos)
        {
            string S_Result = "1";
            if (BomInfos.Item1 == null || !BomInfos.Item1.Any() || !string.IsNullOrEmpty(BomInfos.Item2))
            {
                S_Result = "请先设定组装工站信息(please set up assembly station information.)";
                return S_Result;
            }

            DateTime Date_Start = DateTime.Now;
            string outString1 = string.Empty;
            string mainSN = S_SN.Trim();

            string S_PartID = "";
            string S_POPartID = "";
            string PartFamilyID = "";
            string S_ProductionOrderID = "";
            string S_IsCheckPO = "1";

            string S_UnitStateID = "";
            //string S_luUnitStateID = "1";

            Boolean COF = false;
            string OpenLogFile = System.Configuration.ConfigurationManager.AppSettings["OpenLogFile"];
            DataTable DT_Unit = new DataTable();
            DataTable DT_Defect = new DataTable();
            string[] List_DefectCode = null;

            if (I_StatusID != 1)
            {
                if (S_DefectCode == "")
                {
                    S_Result = "NG状态，不良代码不为空(NG state, DefectCode cannot be empty)";
                    return S_Result;
                }
            }


            if (I_StatusID != 1 && S_DefectCode != "")
            {
                List_DefectCode = S_DefectCode.Split(',');
                //S_luUnitStateID = "2";
                for (int i = 0; i < List_DefectCode.Length; i++)
                {
                    string S_Dcode = List_DefectCode[i].Trim();
                    string S_Sql =
                    @"SELECT * FROM luDefect WHERE DefectTypeID=
                    (
	                    SELECT E.ID FROM mesUnit A 
		                    JOIN mesSerialNumber B ON A.ID=B.UnitID
		                    JOIN mesPart C ON C.ID=A.PartID
		                    JOIN luPartFamily D ON D.ID=C.PartFamilyID
		                    JOIN luPartFamilyType DD ON DD.ID=D.PartFamilyTypeID		                    
		                    JOIN luDefectType E ON RTRIM(LTRIM(E.[Description]))=RTRIM(LTRIM(DD.Name))     
	                    WHERE B.[Value]='" + S_SN + @"'  
                    ) AND DefectCode='" + S_Dcode + "'";

                    DT_Defect = SqlServerHelper.ExecuteDataTable(S_Sql);
                    if (DT_Defect.Rows.Count == 0)
                    {
                        S_Result = "SFC系统没有这个不良代码(SFC systems do not have this DefectCode):" + S_Dcode;
                        return S_Result;
                    }
                }
            }

            try
            {
                if (mainSN == "")
                {
                    //"SN不能为空";
                    S_Result = MyMSG(List_Login.Language, "20119");
                    return S_Result;
                }

                string S_Sql = "SELECT StationTypeID FROM mesStation WHERE ID=" + List_Login.StationID;
                DataTable DT_StationType = SqlServerHelper.Data_Table(S_Sql);
                List_Login.StationTypeID = Convert.ToInt32(DT_StationType.Rows[0]["StationTypeID"].ToString());

                DataSet dsStationDetail = v_PartSelect.GetPLCSeting("", List_Login.StationID.ToString());
                DataSet DS_StationTypeDef = v_PartSelect.MESGetStationTypeParameter(List_Login.StationTypeID);

                DataRow[] DR_IsCheckPO = DS_StationTypeDef.Tables[0].Select("Description='IsCheckPO'");
                if (DR_IsCheckPO.Length > 0)
                {
                    S_IsCheckPO = DR_IsCheckPO[0]["Content"].ToString();

                    if (S_IsCheckPO == "1")
                    {
                        DataRow[] DR_Child_IsCheckPO = dsStationDetail.Tables[0].Select("Name='IsCheckPO'");
                        if (DR_Child_IsCheckPO.Length > 0) S_IsCheckPO = DR_Child_IsCheckPO[0]["Value"].ToString(); else S_IsCheckPO = "1";
                    }
                }
                else
                {
                    S_IsCheckPO = "1";
                }

                if (S_IsCheckPO != "1") { List_Login.IsCheckPO = false; }

                //获取 SN
                S_Sql = "select * from mesSerialNumber where Value='" + mainSN + "'";
                DataTable DT_GetSN = SqlServerHelper.Data_Table(S_Sql);
                if (DT_GetSN.Rows.Count == 0)
                {
                    S_Sql = @"SELECT  B.[Value] SN
                                FROM mesUnitDetail A JOIN mesSerialNumber B ON A.UnitID = B.UnitID
                             WHERE A.reserved_01 = '" + mainSN + "' AND A.reserved_03 = 1";
                    DT_GetSN = SqlServerHelper.Data_Table(S_Sql);

                    mainSN = "";
                    if (DT_GetSN.Rows.Count > 0)
                    {
                        mainSN = DT_GetSN.Rows[0]["SN"].ToString();
                    }
                }


                DataSet dsMainSN;
                dsMainSN = v_PartSelect.uspCallProcedure("uspGetBaseData", mainSN,
                       null, null, null, null, null, ref outString1);
                if (outString1 != "1" || dsMainSN == null || dsMainSN.Tables[0].Rows.Count <= 0)
                {
                    //条码不存在或者状态不符.
                    S_Result = MyMSG(List_Login.Language, "20119");
                    return S_Result;
                }

                DataTable dt = dsMainSN.Tables[0];
                S_ProductionOrderID = dt.Rows[0]["ProductionOrderID"].ToString();
                S_PartID = dt.Rows[0]["PartID"].ToString();
                S_POPartID = dt.Rows[0]["PartID"].ToString();
                PartFamilyID = dt.Rows[0]["PartFamilyID"].ToString();

                if (List_Login.IsCheckPO == true)
                {
                    S_Sql = "select Value  from mesStationConfigSetting where StationID='" + List_Login.StationID + "' and Name='POID'";
                    DataTable DT_COO_POID = SqlServerHelper.Data_Table(S_Sql);

                    if (DT_COO_POID.Rows.Count > 0)
                    {
                        string S_COO_POID = DT_COO_POID.Rows[0]["Value"].ToString();

                        if (S_ProductionOrderID != S_COO_POID)
                        {
                            //此条码和工单不匹配.
                            S_Result = MyMSG(List_Login.Language, "20050");
                            return S_Result;
                        }
                    }
                    else
                    {
                        //此条码和工单不匹配.
                        S_Result = MyMSG(List_Login.Language, "20050");
                        return S_Result;
                    }
                }


                DataSet dsCOF = v_PartSelect.GetluPODetailDef(Convert.ToInt32(S_ProductionOrderID), "COF");
                if (dsCOF != null && dsCOF.Tables.Count > 0 && dsCOF.Tables[0].Rows.Count > 0)
                {
                    COF = dsCOF.Tables[0].Rows[0]["Content"].ToString() == "1";
                }


                DataTable DT_SN = v_PartSelect.GetmesSerialNumber(mainSN).Tables[0];
                if (DT_SN.Rows.Count > 0)
                {
                    string S_UnitID = DT_SN.Rows[0]["UnitID"].ToString();
                    DT_Unit = v_PartSelect.GetmesUnit(S_UnitID).Tables[0];
                    string S_StationID = List_Login.StationID.ToString();

                    try
                    {
                        S_UnitStateID = GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID,
                            List_Login.LineID.ToString(), S_ProductionOrderID, I_StatusID.ToString());
                        //if (S_UnitStateID == "")
                        //{
                        //    S_UnitStateID = GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID,
                        //        List_Login.LineID.ToString(), S_ProductionOrderID, "2");
                        //}
                        //if (S_UnitStateID == "")
                        //{
                        //    S_UnitStateID = GetluUnitStatusID(S_PartID, PartFamilyID, List_Login.StationTypeID,
                        //        List_Login.LineID.ToString(), S_ProductionOrderID, "3");
                        //}
                    }
                    catch
                    {
                    }

                    if (string.IsNullOrEmpty(S_UnitStateID))
                    {
                        //当前工站类型在配置的工艺流程中未配置,请检查工艺流程.
                        S_Result = MyMSG(List_Login.Language, "20203");
                        return S_Result;
                    }
                }
                else
                {
                    //条码:{0},不存在,请确认.
                    S_Result = MyMSG(List_Login.Language, "20242");
                    return S_Result;
                }

                if (!COF)
                {
                    //根据工艺路线判断状态
                    DataSet DsRout = v_PartSelect.GetRouteData(List_Login.LineID.ToString(), S_PartID,
                                                                "", S_ProductionOrderID);
                    if (DsRout == null || DsRout.Tables.Count == 0 || DsRout.Tables[0].Rows.Count == 0)
                    {
                        //料号未配置工艺流程路线.
                        S_Result = MyMSG(List_Login.Language, "20195");
                        return S_Result;
                    }

                    string RoutType = DsRout.Tables[0].Rows[0]["RouteType"].ToString();
                    string StatusID = DT_Unit.Rows[0]["StatusID"].ToString();

                    //无需检查NG 请设置 false
                    if (List_Login.IsCheckNG == true)
                    {
                        if (RoutType != "1" && StatusID != "1")
                        {
                            //此条码已NG. 
                            S_Result = MyMSG(List_Login.Language, "20036");
                            return S_Result;
                        }
                    }
                }

                //2024-01-12  加入此逻辑
                //////////////////////////////////////////////////////
                /// //二次检查是否有指定线路  20231214

                string S_luUnitStateID = I_StatusID.ToString();
                var tmpSecondRet = v_PartSelect.GetmesUnitStateSecond(S_PartID, PartFamilyID, "", List_Login.LineID.ToString(),
                    List_Login.StationTypeID, S_ProductionOrderID, S_luUnitStateID, mainSN);

                if (tmpSecondRet.StartsWith("Error", StringComparison.OrdinalIgnoreCase))
                {
                    S_Result = tmpSecondRet;
                    return S_Result;
                }

                string mS_UnitStateID = tmpSecondRet;
                if (string.IsNullOrEmpty(mS_UnitStateID))
                {
                    S_Result = MyMSG(List_Login.Language, "20203");
                    return S_Result;
                }
                /// //二次检查是否有指定线路  END
                string S_RouteCheck = v_PartSelect.GetRouteCheck(List_Login.StationTypeID, List_Login.StationID,
                    List_Login.LineID.ToString(), DT_Unit, mainSN);
                if (S_RouteCheck != "1")
                {
                    if (S_RouteCheck == "20243")
                    {
                        string nowUnitStateID = v_PartSelect.GetSerialNumber2(mainSN).Tables[0].Rows[0]["UnitStateID"].ToString();
                        mesUnitState mesunitSateModel = v_mesUnitState.Get(Convert.ToInt32(nowUnitStateID));

                        S_Result = "Route Check Failure->" + mainSN + ":" + mesunitSateModel.Description;
                        return S_Result;
                    }
                    else
                    {
                        S_Result = "Route Check Failure->" + MyMSG(List_Login.Language, S_RouteCheck);
                        return S_Result;
                    }
                }

                //调用通用过程
                string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + S_ProductionOrderID + "\"> </ProdOrder>";
                string xmlPart = "<Part PartID=\"" + S_PartID + "\"> </Part>";
                string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                string outString = string.Empty;
                v_PartSelect.uspCallProcedure("uspQCCheck", mainSN, xmlProdOrder, xmlPart, xmlStation, null, null, ref outString);
                if (outString != "1")
                {
                    S_Result = outString;
                    return S_Result;
                }

                int MainUnitID = 0;
                string BatchSN = string.Empty;
                //S_ProductionOrderID
                if (S_ChildCode != null && S_ChildCode.Trim() != "")
                {
                    toolingInfos.Clear();
                    var tmpPartFamilyType = new luPartFamilyDAL().Get(Convert.ToInt32(PartFamilyID));
                    var childCodes = S_ChildCode.Trim().Split(',');

                    //var boms = v_PartSelect.MESGetBomPartInfo(Convert.ToInt32(S_PartID), List_Login.StationTypeID);
                    var boms = BomInfos.Item1;
                    if (childCodes.Length != boms.Length - 1)
                    {
                        S_Result = List_Login.Language == "CH" ? $"子料条码数量({childCodes.Length})与Bom中数量不符({boms.Length-1})" :$"";
                        return S_Result;
                    }

                    for (int i = 0; i < boms.Length; i++)
                    {
                        string tmpRes = string.Empty;
                        var tmpBom = boms[i];
                        if (i > 0 && string.IsNullOrEmpty(childCodes[i-1].Trim()))
                        {
                            S_Result = $"Child SN index : {i}, " + MyMSG(List_Login.Language, "20007");
                            return S_Result;
                        }
                        tmpRes = CheckInputBarcode(mainSN.Trim(), i == 0 ? "" : childCodes[i - 1].Trim(), boms[i], COF, List_Login, S_ProductionOrderID, S_PartID);
                        if (tmpRes != "1")
                        {
                            S_Result = tmpRes;
                            return S_Result;
                        }
                    }

                    if (toolingInfos.Any())
                    {
                        foreach (var toolingInfo in toolingInfos)
                        {
                            if (!CheckParentControlInput(toolingInfo,BomInfos.Item1,mainSN + ","+S_ChildCode))
                            {
                                S_Result = $"parent-child relationship filed.（子母治具检查失败) {toolingInfo.ChildSN} {toolingInfo.ParentSN}";
                                return S_Result;
                            }
                        }
                    }


                    List<mesUnit> List_mesUnit = new List<mesUnit>();
                    List<mesHistory> List_mesHistory = new List<mesHistory>();
                    List<mesUnitComponent> List_mesUnitComponent = new List<mesUnitComponent>();

                    List<mesMaterialConsumeInfo> List_mesMaterialConsumeInfo = new List<mesMaterialConsumeInfo>();
                    List<mesMachine> List_mesMachine = new List<mesMachine>();

                    for (int i = 0; i < boms.Length; i++)
                    {
                        var tmpBom = boms[i];
                        var tmpScanType = boms[i].ScanType;
                        var tmpPartId = boms[i].PartID;
                        string currentSN = i == 0 ? mainSN.Trim() : childCodes[i - 1].Trim();
                        string tmpRes = string.Empty;
                        if (tmpScanType == 6)
                        {
                            var dtsMac = new mesUnitDetailDAL().MesGetBatchIDByBarcodeSN(currentSN);
                            tmpScanType = (dtsMac == null || dtsMac.Tables.Count <= 0 ||
                                           dtsMac.Tables[0].Rows.Count == 0)
                                ? 4
                                : 3;
                        }

                        if (i == 0)
                        {
                            if (tmpScanType == 4)
                            {
                                tmpRes = VirtualBarCode(mainSN, S_PartID, PartFamilyID, S_UnitStateID,
                                    S_ProductionOrderID, tmpPartFamilyType.ID.ToString(),List_Login);

                                if (tmpRes == "NG" || string.IsNullOrEmpty(tmpRes))
                                {
                                    S_Result = "Error: Inert virtual barcode failed.";
                                    return S_Result;
                                }

                                MainUnitID = Convert.ToInt32(tmpRes);
                            }
                            else
                            {
                                if (tmpScanType == 3)
                                {
                                    var dataSetMachine = v_PartSelect.BoxSnToBatch(currentSN, out BatchSN);
                                    if (dataSetMachine == null || dataSetMachine.Tables.Count <= 0 || dataSetMachine.Tables[0].Rows.Count <= 0)
                                    {
                                        S_Result = string.Format($"{MyMSG(List_Login.Language,"20030")}",currentSN);
                                        return S_Result;
                                    }

                                    MainUnitID = Convert.ToInt32(dataSetMachine.Tables[0].Rows[0]["UnitID".ToString()]);
                                }
                                else
                                {
                                    var DT_UPUnitID = v_PartSelect.Get_UnitID(S_SN).Tables[0];
                                    MainUnitID = Convert.ToInt32(DT_UPUnitID.Rows[0]["UnitID"].ToString());
                                }

                                mesUnit F_mesUnit = new mesUnit();
                                F_mesUnit.ID = MainUnitID;
                                F_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                                F_mesUnit.StatusID = 1;
                                F_mesUnit.StationID = List_Login.StationID;
                                F_mesUnit.EmployeeID = List_Login.EmployeeID;
                                F_mesUnit.ProductionOrderID = Convert.ToInt32(S_ProductionOrderID);
                                List_mesUnit.Add(F_mesUnit);
                            }
                            mesUnit mesUnit_Part = new mesUnitDAL().Get(MainUnitID);
                            string tmpPoPartID = mesUnit_Part.PartID.ToString();

                            mesHistory F_mesHistory = new mesHistory();
                            F_mesHistory.UnitID = MainUnitID;
                            F_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                            F_mesHistory.EmployeeID = List_Login.EmployeeID;
                            F_mesHistory.StationID = List_Login.StationID;
                            F_mesHistory.ProductionOrderID = Convert.ToInt32(S_ProductionOrderID);
                            F_mesHistory.PartID = Convert.ToInt32(tmpPoPartID);
                            F_mesHistory.LooperCount = 1;
                            F_mesHistory.StatusID = 1;
                            F_mesHistory.EnterTime = DateTime.Now;
                            List_mesHistory.Add(F_mesHistory);
                        }
                        else
                        {
                            mesUnitComponent v_mesUnitComponent = new mesUnitComponent();
                            if (tmpScanType == 2 || tmpScanType == 5 || tmpScanType == 7 || tmpScanType == 8)
                            {
                                v_mesUnitComponent.UnitID = MainUnitID;
                                v_mesUnitComponent.UnitComponentTypeID = 1;
                                v_mesUnitComponent.ChildUnitID = 0;
                                if (tmpScanType == 7 || tmpScanType == 8)
                                {
                                    DataSet Pss = v_PartSelect.GetmesSerialNumber(currentSN);
                                    if (Pss != null && Pss.Tables.Count > 0)
                                    {
                                        v_mesUnitComponent.ChildUnitID = Convert.ToInt32(Pss.Tables[0].Rows[0]["UnitID"].ToString());
                                    }
                                }

                                if (tmpScanType == 2)
                                {
                                    v_mesUnitComponent.ChildSerialNumber = "";
                                    v_mesUnitComponent.ChildLotNumber = currentSN;
                                }
                                else
                                {
                                    v_mesUnitComponent.ChildSerialNumber = currentSN;
                                    v_mesUnitComponent.ChildLotNumber = "";
                                }
                                int partID = Convert.ToInt32(tmpBom.PartID);
                                mesPart part = new mesPartDAL().Get(partID);
                                v_mesUnitComponent.ChildPartID = partID;
                                v_mesUnitComponent.ChildPartFamilyID = part.PartFamilyID;
                                v_mesUnitComponent.Position = "";
                                v_mesUnitComponent.InsertedEmployeeID = List_Login.EmployeeID;
                                v_mesUnitComponent.InsertedStationID = List_Login.StationID;
                                v_mesUnitComponent.StatusID = 1;
                            }
                            else
                            {
                                int ChildUnitID;
                                string tmpBatch = string.Empty;
                                if (tmpScanType == 3)
                                {
                                    //找到虚拟条码UnitID
                                    DataSet dataSetMachine = v_PartSelect.BoxSnToBatch(S_SN, out tmpBatch);
                                    if (dataSetMachine == null || dataSetMachine.Tables.Count == 0 || dataSetMachine.Tables[0].Rows.Count == 0)
                                    {
                                        S_Result = string.Format($"{MyMSG(List_Login.Language, "20030")}", "SN:" + currentSN);
                                        //MessageInfo.Add_Info_MSG(Edt_MSG, "20030", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                                        return S_Result;
                                    }
                                    ChildUnitID = Convert.ToInt32(dataSetMachine.Tables[0].Rows[0]["UnitID"].ToString());
                                }
                                else if (tmpScanType == 4)
                                {
                                    //子料是夹具，要找夹具的工单，如果夹具没有工单就默认0  2022-04-15 修改
                                    DataTable DT_ChildPO = v_PartSelect.GetPO(tmpBom.PartID.ToString(), "").Tables[0];
                                    string S_TollPOID = "0";

                                    if (DT_ChildPO != null && DT_ChildPO.Rows.Count > 0)
                                    {
                                        S_TollPOID = DT_ChildPO.Rows[0]["Id"].ToString();
                                    }
                                    else
                                    {
                                        S_TollPOID = "0";
                                    }

                                    string Result = VirtualBarCode(currentSN, tmpBom.PartID.ToString(), null, S_UnitStateID, S_ProductionOrderID, tmpPartFamilyType.ID.ToString(), List_Login,S_TollPOID );
                                    if (Result == "NG" || string.IsNullOrEmpty(Result))
                                    {
                                        S_Result = "Error 2: Inert virtual barcode failed.";
                                        return S_Result;
                                    }
                                    ChildUnitID = Convert.ToInt32(Result);
                                }
                                else
                                {
                                    DataSet dataSetUnit = v_PartSelect.GetmesSerialNumber(currentSN);
                                    if (dataSetUnit == null || dataSetUnit.Tables.Count == 0 || dataSetUnit.Tables[0].Rows.Count == 0)
                                    {
                                        S_Result = string.Format($"{MyMSG(List_Login.Language, "20031")}", "SN:" + currentSN);
                                        //MessageInfo.Add_Info_MSG(Edt_MSG, "20031", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                                        return S_Result;
                                    }
                                    ChildUnitID = Convert.ToInt32(dataSetUnit.Tables[0].Rows[0]["UnitID"].ToString());
                                }

                                DataSet dsComponent = v_PartSelect.GetComponent(ChildUnitID);
                                if (dsComponent == null || dsComponent.Tables.Count == 0 || dsComponent.Tables[0].Rows.Count == 0)
                                {
                                    S_Result = string.Format($"{MyMSG(List_Login.Language, "20032")}", "SN:" + currentSN);

                                    //MessageInfo.Add_Info_MSG(Edt_MSG, "20032", "NG", List_Login.Language, new string[] { "SN:" + S_SN });
                                    return S_Result;
                                }

                                string S_ChildSerialNumber = dsComponent.Tables[0].Rows[0]["Value"].ToString();
                                int ChildPartFamilyID = Convert.ToInt32(dsComponent.Tables[0].Rows[0]["PartFamilyID"].ToString());

                                v_mesUnitComponent.UnitID = MainUnitID;
                                v_mesUnitComponent.UnitComponentTypeID = 1;
                                v_mesUnitComponent.ChildUnitID = Convert.ToInt32(ChildUnitID);
                                v_mesUnitComponent.ChildSerialNumber = S_ChildSerialNumber;
                                v_mesUnitComponent.ChildLotNumber = tmpBatch;
                                v_mesUnitComponent.ChildPartID = Convert.ToInt32(tmpBom.PartID);
                                v_mesUnitComponent.ChildPartFamilyID = ChildPartFamilyID;
                                v_mesUnitComponent.Position = "";
                                v_mesUnitComponent.InsertedEmployeeID = List_Login.EmployeeID;
                                v_mesUnitComponent.InsertedStationID = List_Login.StationID;
                                v_mesUnitComponent.StatusID = 1;
                            }
                            List_mesUnitComponent.Add(v_mesUnitComponent);
                        }

                        if (tmpScanType== 3 || tmpScanType == 4 || tmpScanType == 6)
                        {
                            List_mesMachine.Add(new mesMachine() { SN = currentSN });
                        }

                        if (tmpScanType == 2 || tmpScanType == 7 || tmpScanType == 3 || tmpScanType == 8)
                        {
                            string SN = string.Empty;
                            string MachineSN = string.Empty;
                            int BatchType = 1;
                            if (tmpScanType == 3)
                            {
                                MachineSN = currentSN;
                                BatchType = 2;
                            }
                            else
                            {
                                SN = currentSN;

                            }

                            mesMaterialConsumeInfo v_mesMaterialConsumeInfo = new mesMaterialConsumeInfo();
                            v_mesMaterialConsumeInfo.ScanType = BatchType;
                            v_mesMaterialConsumeInfo.SN = SN;
                            v_mesMaterialConsumeInfo.MachineSN = MachineSN;
                            v_mesMaterialConsumeInfo.PartID = Convert.ToInt32(S_PartID);
                            v_mesMaterialConsumeInfo.ProductionOrderID = Convert.ToInt32(S_ProductionOrderID);

                            List_mesMaterialConsumeInfo.Add(v_mesMaterialConsumeInfo);
                        }
                    }
                    int I_RouteID = v_PartSelect.GetRouteID(List_Login.LineID.ToString(), S_PartID, PartFamilyID, S_ProductionOrderID.ToString());
                    bool isLastStation = IsDiagramSFCLastStation(List_Login.StationTypeID.ToString(), I_RouteID.ToString());
                    if (isLastStation)
                    {
                        List<mesUnitComponent> mucs = new List<mesUnitComponent>();
                        foreach (mesUnitComponent unitComponent in List_mesUnitComponent)
                        {
                            bool tmpExistComponent = IsExistBindChildPart(unitComponent.UnitID,
                                unitComponent.ChildUnitID, List_Login.StationTypeID.ToString(), unitComponent.ChildLotNumber, unitComponent.ChildPartID.ToString());
                            if (tmpExistComponent)
                                continue;

                            unitComponent.StatusID = tmpExistComponent ? 0 : 1;
                            mucs.Add(unitComponent);
                        }
                        List_mesUnitComponent.Clear();
                        List_mesUnitComponent = mucs;
                    }

                    //mesUnit[] L_mesUnit = List_mesUnit.ToArray();
                    //mesHistory[] L_mesHistory = List_mesHistory.ToArray();
                    //mesUnitComponent[] L_mesUnitComponent = List_mesUnitComponent.ToArray();
                    //mesMaterialConsumeInfo[] L_mesMaterialConsumeInfo = List_mesMaterialConsumeInfo.ToArray();
                    //mesMachine[] L_mesMachine = List_mesMachine.ToArray();
                    string ReturnValue = new DataCommitDAL().SubmitDataUHC(List_mesUnit, List_mesHistory, List_mesUnitComponent,
                        List_mesMaterialConsumeInfo, List_mesMachine, List_Login);
                    if (ReturnValue != "OK")
                    {
                        S_Result = ReturnValue;
                        //MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ReturnValue });
                        return S_Result;
                    }



                }
                else
                {   
                    List<mesUnit> List_mesUnit = new List<mesUnit>();
                    List<mesHistory> List_mesHistory = new List<mesHistory>();
                    List<mesUnitDefect> List_mesUnitDefect = new List<mesUnitDefect>();

                    DataTable DT_UPUnitID = v_PartSelect.Get_UnitID(mainSN).Tables[0];

                    mesUnit v_mesUnit = new mesUnit();
                    v_mesUnit.ID = Convert.ToInt32(DT_UPUnitID.Rows[0]["UnitID"].ToString());
                    //v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                    //2024-01-12 2024版画图
                    v_mesUnit.UnitStateID = Convert.ToInt32(mS_UnitStateID);
                    v_mesUnit.StatusID = I_StatusID;
                    v_mesUnit.StationID = List_Login.StationID;
                    v_mesUnit.EmployeeID = List_Login.EmployeeID;
                    v_mesUnit.LastUpdate = DateTime.Now;
                    v_mesUnit.ProductionOrderID = Convert.ToInt32(S_ProductionOrderID);
                    //修改 Unit
                    List_mesUnit.Add(v_mesUnit);

                    mesHistory v_mesHistory = new mesHistory();
                    v_mesHistory.UnitID = Convert.ToInt32(v_mesUnit.ID);
                    //v_mesHistory.UnitStateID = Convert.ToInt32(S_UnitStateID);
                    //2024-01-12 2024版画图
                    v_mesHistory.UnitStateID = Convert.ToInt32(mS_UnitStateID);
                    v_mesHistory.EmployeeID = List_Login.EmployeeID;
                    v_mesHistory.StationID = List_Login.StationID;
                    v_mesHistory.EnterTime = DateTime.Now;
                    v_mesHistory.ExitTime = DateTime.Now;
                    v_mesHistory.ProductionOrderID = Convert.ToInt32(S_ProductionOrderID);
                    v_mesHistory.PartID = Convert.ToInt32(S_POPartID);
                    v_mesHistory.LooperCount = 1;
                    v_mesHistory.StatusID = I_StatusID;

                    List_mesHistory.Add(v_mesHistory);

                    if (S_DefectCode != "1" && S_DefectCode != "" && I_StatusID != 1)
                    {
                        string S_DefectTypeID = DT_Defect.Rows[0]["DefectTypeID"].ToString();
                        for (int i = 0; i < List_DefectCode.Length; i++)
                        {
                            string S_Dcode = List_DefectCode[i].Trim();

                            S_Sql = "SELECT * FROM luDefect WHERE DefectTypeID ='" + S_DefectTypeID + "' and DefectCode='" + S_Dcode + "'";
                            DT_Defect = SqlServerHelper.Data_Table(S_Sql);

                            int I_DefectID = Convert.ToInt32(DT_Defect.Rows[0]["Id"].ToString());
                            mesUnitDefect v_mesUnitDefect = new mesUnitDefect();

                            v_mesUnitDefect.UnitID = v_mesUnit.ID;
                            v_mesUnitDefect.DefectID = I_DefectID;
                            v_mesUnitDefect.StationID = List_Login.StationID;
                            v_mesUnitDefect.EmployeeID = List_Login.EmployeeID;

                            List_mesUnitDefect.Add(v_mesUnitDefect);
                        }

                    }

                    string ReturnValue = v_DataCommitDAL.SubmitDataUHD(List_mesUnit, List_mesHistory, List_mesUnitDefect);
                    if (ReturnValue != "OK")
                    {
                        S_Result = ReturnValue;
                        return S_Result;
                    }
                }
                return S_Result;
            }
            catch (Exception ex)
            {
                try
                {
                    if (OpenLogFile == "1")
                    {
                        TimeSpan ts = DateTime.Now - Date_Start;
                        double mill = ts.TotalMilliseconds;
                        string logPath = CreateServerDIR();
                        string msg = ex.ToString() + "Time：" + mill.ToString() + "ms";
                        File.AppendAllText(logPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".log",
                           DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + msg + "\r\n");
                    }
                }
                catch
                { }

                return S_Result = ex.ToString();
            }
        }
        public bool CheckParentControlInput(ToolingInfo toolingInfo,BomInfo[] bomInfos, string SNs)
        {
            string tmpParentSN = string.Empty;
            string tmpSN = string.Empty;

            string[] SNList = SNs.Split(',');
            for (int i = 0; i < bomInfos.Length; i++)
            {
                if (toolingInfo.ParentPartID == bomInfos[i].PartID && toolingInfo.ParentSN == SNList[i].Trim())
                    tmpParentSN = toolingInfo.ParentSN;
                if (toolingInfo.ChildPartID == bomInfos[i].PartID && toolingInfo.ChildSN == SNList[i].Trim())
                    tmpSN = toolingInfo.ChildSN;
            }
            return !string.IsNullOrEmpty(tmpParentSN) && !string.IsNullOrEmpty(tmpSN);
        }
        private bool IsExistBindChildPart(int mainUnitID, int childUnitID, string stationTypeId, string BatchNumber, string childPartId)
        {
            bool isSFCLastStation = false;
            string S_Sql = $@"SELECT 1
            FROM dbo.mesUnitComponent a
            JOIN dbo.mesStation b ON b.ID = a.InsertedStationID
            WHERE b.StationTypeID = {stationTypeId} AND a.UnitID = {mainUnitID}  AND a.StatusID = 1  AND a.ChildPartID = {childPartId}";
            DataSet ds = v_PartSelect.P_DataSet(S_Sql);
            if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
            {
                string res = ds.Tables[0].Rows[0][0]?.ToString();
                isSFCLastStation = res == "1";
            }
            return isSFCLastStation;
        }
        private bool IsDiagramSFCLastStation(string stationTypeId, string routeId)
        {
            bool isSFCLastStation = false;
            string S_Sql = $@"SELECT 1 FROM mesUnitoUTputState A
						LEFT JOIN V_StationTypeInfo B ON A.StationTypeID=B.StationTypeID
						WHERE A.RouteID={routeId} and A.StationTypeID='{stationTypeId}' 
						AND ISNULL(B.Content,'')<>'TT' AND CurrStateID=0
						 AND A.OutputStateDefID=1";
            DataSet ds = v_PartSelect.P_DataSet(S_Sql);
            if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 1)
            {
                string res = ds.Tables[0].Rows[0][0]?.ToString();
                isSFCLastStation = res == "1";
            }
            return isSFCLastStation;
        }
        private string VirtualBarCode(string S_SN, string S_PartID, string S_PartFamilyID, string S_UnitStateID, string S_ProductionOrderID, string S_PartFamilyType, LoginList List_Login, string toolPOID = "")
        {
            try
            {
                string S_InsertUnit = string.Empty;
                if (string.IsNullOrEmpty(toolPOID))
                {
                    toolPOID = S_ProductionOrderID;
                }
                //判断是否已经生成了虚拟条码
                string S_FG_SN = v_PartSelect.BuckToFGSN(S_SN);
                if (string.IsNullOrEmpty(S_FG_SN))
                {
                    string S_FormatSN = v_PartSelect.mesGetSNFormatIDByList(S_PartID, S_PartFamilyID,
                        List_Login.LineID.ToString(), S_ProductionOrderID, List_Login.StationTypeID.ToString());
                    if (string.IsNullOrEmpty(S_FormatSN))
                    {
                        //MessageInfo.Add_Info_MSG(Edt_MSG, "20034", "NG", List_Login.Language, new string[] { S_PartID });
                        return "NG";
                    }

                    string xmlProdOrder = "'<ProdOrder ProductionOrder=" + "\"" + S_ProductionOrderID + "\"" + "> </ProdOrder>'";
                    string xmlStation = "'<Station StationID=" + "\"" + List_Login.StationID.ToString() + "\"" + "> </Station>'";
                    string xmlPart = "'<Part PartID=" + "\"" + S_PartID + "\"" + "> </Part>'";
                    string xmlExtraData = "'<ExtraData BoxSN=" + "\"" + S_SN + "\"" +
                                                   " LineID = " + "\"" + List_Login.LineID.ToString() + "\"" +
                                                   " PartFamilyTypeID=" + "\"" + S_PartFamilyType + "\"" +
                                                   " LineType=" + "\"" + "M" + "\"" + "> </ExtraData>'";
                    DataSet DS = v_PartSelect.uspSNRGetNext(S_FormatSN, 0, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, null);
                    string New_SN = DS.Tables[1].Rows[0][0].ToString();
                    mesUnit v_mesUnit = new mesUnit();
                    v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                    v_mesUnit.StatusID = 1;
                    v_mesUnit.StationID = List_Login.StationID;
                    v_mesUnit.EmployeeID = List_Login.EmployeeID;
                    v_mesUnit.CreationTime = DateTime.Now;
                    v_mesUnit.LastUpdate = DateTime.Now;
                    v_mesUnit.PanelID = 0;
                    v_mesUnit.LineID = List_Login.LineID;
                    v_mesUnit.ProductionOrderID = Convert.ToInt32(toolPOID);
                    v_mesUnit.RMAID = 0;
                    v_mesUnit.PartID = Convert.ToInt32(S_PartID);
                    v_mesUnit.LooperCount = 1;


                    mesUnitDetail v_mesUnitDetail = new mesUnitDetail();
                    v_mesUnitDetail.reserved_01 = S_SN;
                    v_mesUnitDetail.reserved_02 = "";
                    v_mesUnitDetail.reserved_03 = "1";
                    v_mesUnitDetail.reserved_04 = "";
                    v_mesUnitDetail.reserved_05 = "";


                    mesSerialNumber v_mesSerialNumber = new mesSerialNumber();
                    v_mesSerialNumber.SerialNumberTypeID = 8;
                    v_mesSerialNumber.Value = New_SN;

                    string ReturnValue = new DataCommitDAL().InsertUDS(v_mesUnit, v_mesUnitDetail, v_mesSerialNumber, S_SN);
                    S_InsertUnit = ReturnValue;

                    //if (ReturnValue.Substring(0,5) =="ERROR")
                    if (string.IsNullOrEmpty(ReturnValue) || (ReturnValue.Length >= 5 && ReturnValue.Substring(0, 5) == "ERROR"))
                    {
                        S_InsertUnit = "NG";
                        //string ProMsg = MessageInfo.GetMsgByCode(ReturnValue, List_Login.Language);
                        //MessageInfo.Add_Info_MSG(Edt_MSG, "20011", "NG", List_Login.Language, new string[] { New_SN, ProMsg }, "", "");
                    }
                    else
                    {
                        ReturnValue = "SN:" + New_SN;
                        //MessageInfo.Add_Info_MSG(Edt_MSG, "66000", "OK", List_Login.Language, new string[] { ReturnValue });
                    }
                }
                else
                {
                    DataSet ds = v_PartSelect.GetmesSerialNumber(S_FG_SN);
                    S_InsertUnit = ds.Tables[0].Rows[0]["UnitID"].ToString();
                    mesUnit v_mesUnit = new mesUnit();
                    v_mesUnit.UnitStateID = Convert.ToInt32(S_UnitStateID);
                    v_mesUnit.StatusID = 1;
                    v_mesUnit.StationID = List_Login.StationID;
                    v_mesUnit.EmployeeID = List_Login.EmployeeID;
                    //v_mesUnit.CreationTime = DateTime.Now;
                    v_mesUnit.LastUpdate = DateTime.Now;
                    v_mesUnit.PanelID = 0;
                    v_mesUnit.LineID = List_Login.LineID;
                    v_mesUnit.ProductionOrderID = Convert.ToInt32(toolPOID);
                    v_mesUnit.RMAID = 0;
                    v_mesUnit.PartID = Convert.ToInt32(S_PartID);
                    v_mesUnit.LooperCount = 1;
                    v_mesUnit.ID = Convert.ToInt32(S_InsertUnit);

                    string ReturnValue = new DataCommitDAL().UpdatemesUnit(v_mesUnit);
                    if (ReturnValue != "OK")
                    {
                        S_InsertUnit = "NG";
                        //MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ReturnValue });
                    }
                }
                return S_InsertUnit;
            }
            catch (Exception ex)
            {
                //MessageInfo.Add_Info_MSG(Edt_MSG, "20009", "NG", List_Login.Language, new string[] { ex.Message.ToString() });
                return "NG";
            }
        }
        private string CheckInputBarcode(string MainSN, string ChildSN, BomInfo bi, bool COF, LoginList List_Login, string POID, string PartID)
        {
            bool isMainSN = string.IsNullOrEmpty(ChildSN);
            string tmpSigResult = "1";
            var tmpChildSN = ChildSN;
            var tmpBom = bi;
            string NewSN = isMainSN ? MainSN : ChildSN;
            if (!new int[] {3, 4, 6}.Contains(tmpBom.ScanType))
            {
                //正则校验
                if (!Regex.IsMatch(NewSN, tmpBom.Pattern.Replace("\\\\", "\\")))
                {
                    tmpSigResult = List_Login.Language == "CH" ? $"条码({NewSN})正则表达式校验失败" : $"SN: {NewSN} Regular expression verification failed.";
                    return tmpSigResult;
                }
            }


            string xmlPartStr = "<Part PartID=\"" + tmpBom.PartID + "\"> </Part>";
            string xmlExtraData = "<ExtraData MainCode=\"" + MainSN + "\"> </ExtraData>";
            switch (tmpBom.ScanType)
            {
                case 1:
                    tmpSigResult = isMainSN? v_PartSelect.MESAssembleCheckMianSN(POID,List_Login.LineID,List_Login.StationID,List_Login.StationTypeID,MainSN,COF) : v_PartSelect.MESAssembleCheckOtherSN(tmpChildSN, tmpBom.PartID.ToString(), COF);
                    break;
                case 2:
                case 7:
                case 8:
                    v_PartSelect.uspCallProcedure("uspBatchDataCheck", tmpChildSN,
                        null, xmlPartStr, null, null, "1", ref tmpSigResult);
                    break;
                case 3:
                case 4:
                case 6:
                    v_PartSelect.uspCallProcedure("uspMachineToolingCheck", ChildSN,
                        null, xmlPartStr, null, xmlExtraData, List_Login.StationTypeID.ToString(), ref tmpSigResult);

                    if (isMainSN && tmpSigResult =="1")
                    {
                        NewSN = v_PartSelect.BuckToFGSN(MainSN);
                        tmpSigResult = v_PartSelect.MESAssembleCheckMianSN(POID, List_Login.LineID,
                            List_Login.StationID, List_Login.StationTypeID, MainSN, COF);
                    }

                    if (tmpBom.ScanType == 3 && tmpSigResult == "1")
                    {
                        v_PartSelect.uspCallProcedure("uspBatchDataCheck", isMainSN?MainSN:tmpChildSN,
                            null, xmlPartStr, null, null, "2", ref tmpSigResult);

                        //if (tmpSigResult != "1")
                        //    break;
                    }

                    if (tmpBom.ScanType == 4 && tmpSigResult == "1")
                    {
                        var tmpParentId = QueryParentTooling(NewSN);
                        if (!string.IsNullOrEmpty(tmpParentId.Item1) && tmpParentId.Item1 != "0")
                        {
                            if (string.IsNullOrEmpty(tmpParentId.Item2) || string.IsNullOrEmpty(tmpParentId.Item3))
                            {
                                tmpSigResult = $"can't found Parent Info, please check machine setting.(无法找到父治具信息，请检查治具设定) {tmpParentId.Item2} ";
                                return tmpSigResult;
                            }

                            ToolingInfo ti = new ToolingInfo()
                            {
                                ChildPartID = int.Parse(PartID),
                                ChildSN = NewSN,
                                ParentID = int.Parse(tmpParentId.Item1),
                                ParentPartID = int.Parse(tmpParentId.Item2),
                                ParentSN = tmpParentId.Item3
                            };

                            int parentIndex = toolingInfos.FindIndex(x =>
                                x.ParentPartID == ti.ParentPartID && x.ParentID == ti.ParentID);
                            if (parentIndex >= 0)
                            {
                                toolingInfos[parentIndex] = ti;
                            }
                            else
                            {
                                toolingInfos.Add(ti);
                            }
                        }
                    }
                    break;
                case 5:
                    mesUnitComponentDAL mesUnitComponent = new mesUnitComponentDAL();
                    bool isCheck = mesUnitComponent.MESCheckChildSerialNumber(ChildSN);
                    tmpSigResult = "20029";
                    break;
                default:
                    break;
            }

            if (tmpSigResult != "1")
            {
                if (tmpSigResult == "20243")
                {
                    var currentUnitStateID = v_PartSelect.GetSerialNumber2(NewSN).Tables[0].Rows[0]["UnitStateID"].ToString();
                    mesUnitState mus = new mesUnitStateDAL().Get(Convert.ToInt32(currentUnitStateID));
                    string tmpMsg = MyMSG(List_Login.Language, tmpSigResult);
                    tmpSigResult = string.Format($"{tmpMsg}", NewSN, mus.Description);
                }
                else
                {
                     tmpSigResult = MyMSG(List_Login.Language, tmpSigResult);
                }
            }
            else
            {
                string xmlExtraDataAss = "<ExtraData MainCode=\"" + MainSN + "\"> </ExtraData>";

                //调用通用过程
                string xmlProdOrder = "<ProdOrder ProdOrderID=\"" + POID + "\"> </ProdOrder>";
                string xmlPart = "<Part PartID=\"" + tmpBom.PartID + "\"> </Part>";
                string xmlStation = "<Station StationID=\"" + List_Login.StationID + "\"> </Station>";
                v_PartSelect.uspCallProcedure("uspAssembleCheck", NewSN,
                    xmlProdOrder, xmlPart, xmlStation, xmlExtraDataAss, PartID, ref tmpSigResult);
                if (tmpSigResult != "1")
                {
                    tmpSigResult = MyMSG(List_Login.Language, tmpSigResult);
                }
            }
            return tmpSigResult;
        }
        private Tuple<string, string, string> QueryParentTooling(string SN)
        {

            string S_Sql = $@"DECLARE  @SN VARCHAR(200) = '{SN}', @parentPartId INT,@parentId INT,@parentSN VARCHAR(200)
            SELECT @parentId = ISNULL(a.ParentID, 0)
            FROM dbo.mesMachine a
            WHERE a.SN = @SN

            IF @parentId IS NOT NULL AND @parentId <> 0
            BEGIN
                SELECT @parentPartId = ISNULL(PartID, 0), @parentSN = SN FROM dbo.mesMachine WHERE ID = @parentId
            END

            SELECT @parentId parentId, @parentPartId parentPartId, @parentSN parentSN
            ";
            DataSet ds = v_PartSelect.P_DataSet(S_Sql);

            if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
            {
                return new Tuple<string, string, string>(ds.Tables[0].Rows[0][0].ToString(), ds.Tables[0].Rows[0][1]?.ToString(), ds.Tables[0].Rows[0][2]?.ToString());
            }
            else
            {
                return new Tuple<string, string, string>("", "", "");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        ///
        /// 
        public DataSet GetEmployee(string S_UserName)
        {
            string S_Sql = "select * from mesEmployee  where Firstname='" + S_UserName + "' or Lastname='"+ S_UserName + "' ";                                
            DataSet ds = SqlServerHelper.Data_Set(S_Sql);
            return ds;
        }


        public int GetUser(string UserID, string Password)
        {
            int I_Result = 0;

            string S_Sql = "select *  from mesEmployee where UserID='" + UserID + "'  and Password='" + Password + "'";
            DataTable DT = SqlServerHelper.Data_Table(S_Sql);
            if (DT.Rows.Count == 0)
            {
                string S_JM = PublicF.EncryptPassword(Password, "");
                S_Sql = "select *  from mesEmployee where UserID='" + UserID + "'  and Password='" + S_JM + "'";
                DT = SqlServerHelper.Data_Table(S_Sql);
            }

            I_Result = DT.Rows.Count;
            return I_Result;
        }

        public DataSet GetmesLine()
        {
            DataSet ds = v_PartSelect.GetmesLine();
            return ds;
        }

        public DataSet GetluPartFamilyType()
        {
            DataSet ds = v_PartSelect.GetluPartFamilyType() ;
            return ds;
        }
        public DataSet GetluPartFamily(string PartFamilyTypeID)
        {
            DataSet ds = v_PartSelect.GetluPartFamily(PartFamilyTypeID);
            return ds;
        }

        public DataSet GetmesPart(string PartFamilyID)
        {
            DataSet ds = v_PartSelect.GetmesPart(PartFamilyID);
            return ds;
        }

        public DataSet GetmesPartOne(string ID)
        {
            string strSql = "SELECT * FROM mesPart where Status=1 and Id=" + ID;

            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }


        public DataSet GetmesStationType()
        {            
            DataSet ds = v_PartSelect.GetmesStationType();
            return ds;
        }

        public DataSet GetmesStation(string LineID)
        {
            DataSet ds = v_PartSelect.GetmesStation(LineID);
            return ds;
        }

        public DataSet GetPOAll(string S_PartID, string S_LineID)
        {
            DataSet ds = v_PartSelect.GetPOAll(S_PartID, S_LineID);
            return ds;
        }

        public string SaveStationSet(string StationID, string Value)
        {
            string S_Result = "OK";
            string S_Sql = "select * from mesStationConfigSetting where Name='POID' and StationID='" + StationID + "'";
            DataTable DT = SqlServerHelper.Data_Table(S_Sql);
            int I_Result = DT.Rows.Count;

            if (I_Result == 0)
            {

                S_Sql = "insert into mesStationConfigSetting(Name,Description,StationID,Value) Values(" +
                              " 'POID'," +
                              " 'POID'," +
                              "'" + StationID + "'," +
                              "'" + Value + "'" +
                              ")";
            }
            else
            {
                S_Sql = "Update mesStationConfigSetting set Value='" + Value +
                    "' where Name='POID' and StationID='" + StationID + "'";
            }
            S_Result = SqlServerHelper.ExecSql(S_Sql);
            return S_Result;
        }

        public Tuple<BomInfo[], string> GetBomInfos(int ParentID, int StationTypeID)
        {
            List<BomInfo>  lb = new List<BomInfo>();
            var bomDataSet = v_PartSelect.MESGetBomPartInfo(ParentID, StationTypeID);
            string errorCode = string.Empty;
            if (bomDataSet != null && bomDataSet.Tables.Count > 0 && bomDataSet.Tables[0].Rows.Count > 0)
            {
                var tmpTab = bomDataSet.Tables[0];
                for (int i = 0; i < tmpTab.Rows.Count; i++)
                {
                    var tmpRow = tmpTab.Rows[i];
                    string partId = tmpRow["PartID"].ToString();
                    if (string.IsNullOrEmpty(partId))
                    {
                        errorCode = "20023";
                        break;
                    }
                    string ScanType = tmpRow["ScanType"]?.ToString();
                    if (string.IsNullOrEmpty(ScanType))
                    {
                        errorCode = "20024";
                        break;
                    }

                    if (!(new string[] { "1", "2", "3", "4", "5", "6", "7", "8" }).Contains(ScanType))
                    {
                        errorCode = "20026";
                        break;
                    }

                    string Pattern = tmpRow["Pattern"]?.ToString();
                    if (string.IsNullOrEmpty(Pattern))
                    {
                        errorCode = "20025";
                        break;
                    }
                    lb.Add(new BomInfo(){ID = i,PartID = Convert.ToInt32(partId), ScanType = Convert.ToInt32(ScanType), Pattern = Pattern});
                }
            }
            else
            {
                errorCode = "20173";
            }
            return new Tuple<BomInfo[], string>(lb.ToArray(), errorCode);
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////

        private string MyMSG(string S_Language, string MSG)
        {
            string S_Result = MSG;
            string S_MSG = v_PartSelect.GetMSG(S_Language, MSG);
            if (S_MSG != "")
            {
                S_Result = S_MSG;
            }
            return S_Result;
        }


        private string GetluUnitStatusID(string S_PartID, string PartFamilyID, int StationTypeID,
            string LineID, string ProductionOrderID, string StatusID)
        {
            string S_UnitStateID = "";            
            try
            {
                DataSet ds = v_PartSelect.GetmesUnitState(S_PartID, PartFamilyID, "",
                    LineID, StationTypeID, ProductionOrderID, StatusID);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    DataTable DT = v_PartSelect.GetmesUnitState(S_PartID, PartFamilyID, "",
                        LineID, StationTypeID, ProductionOrderID, StatusID).Tables[0];
                    S_UnitStateID = DT.Rows[0]["ID"].ToString().Trim();
                }               
            }
            catch
            {                
            }
            return S_UnitStateID;
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


    }
    public class ToolingInfo
    {
        public string ParentSN { get; set; }
        public int ParentPartID { get; set; }
        public int ParentID { get; set; }
        public string ChildSN { get; set; }
        public int ChildPartID { get; set; }
        public int ChildID { get; set; }
    }
}
