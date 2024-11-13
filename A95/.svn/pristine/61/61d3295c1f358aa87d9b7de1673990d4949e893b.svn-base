using App.DBServerDAL;
using App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Formatting.Json;
using Newtonsoft.Json;

namespace App.BLL
{
    public class SFCAPIBLL
    {

        public string ChildSN_Check(string S_SN, string S_ChildSN, int ChildPartID, LoginList List_Login,
            Tuple<BomInfo[], string> BomInfos, int SelectedPartID, int SelectedPOID)
        {
            //return new SFCAPIDAL().ChildSN_Check(S_SN, S_ChildSN, ChildPartID, List_Login, BomInfos, SelectedPartID, SelectedPOID);
            string result = "";
            DateTime startDateTime = DateTime.Now;
            try
            {
                result = new SFCAPIDAL().ChildSN_Check(S_SN, S_ChildSN, ChildPartID, List_Login, BomInfos, SelectedPartID, SelectedPOID);
                DateTime currentTime = DateTime.Now;
                LoggerHelper.WriteDebugLog($"input : {S_SN},{S_ChildSN},{ChildPartID},{JsonConvert.SerializeObject(List_Login)},{BomInfos},{SelectedPartID},{SelectedPOID}; result :{result}; {startDateTime} - {currentTime}, cost {(currentTime - startDateTime).TotalMilliseconds} ms");
            }
            catch (Exception e)
            {
                LoggerHelper.WriteErrorLog($"input: { S_SN},{ S_ChildSN},{ ChildPartID},{ JsonConvert.SerializeObject(List_Login)},{ BomInfos},{ SelectedPartID},{ SelectedPOID}; {e.Message} \r\n cost {(DateTime.Now - startDateTime).TotalMilliseconds} ms");
            }
            return result;
        }
        public string MainSN_Check(string S_SN, LoginList List_Login)
        {
            //return new SFCAPIDAL().MainSN_Check(S_SN, List_Login);
            string result = "";
            DateTime startDateTime = DateTime.Now;
            try
            {
                result = new SFCAPIDAL().MainSN_Check(S_SN, List_Login);
                DateTime currentTime = DateTime.Now;
                LoggerHelper.WriteDebugLog($"input : {S_SN},{JsonConvert.SerializeObject(List_Login)}; result :{result}; {startDateTime} - {currentTime}, cost {(currentTime - startDateTime).TotalMilliseconds} ms");
            }
            catch (Exception e)
            {
                result = e.Message;
                LoggerHelper.WriteErrorLog( $"input : {S_SN},{JsonConvert.SerializeObject(List_Login)}; {e.Message}\r\n cost {(DateTime.Now - startDateTime).TotalMilliseconds} ms");
            }
            return result;
        }

        public string MainSN_GetAssySN(string S_SN, LoginList List_Login)
        {
            //return new SFCAPIDAL().MainSN_GetAssySN(S_SN, List_Login);
            string result = "";
            DateTime startDateTime = DateTime.Now;
            try
            {
                result = new SFCAPIDAL().MainSN_GetAssySN(S_SN, List_Login);
                DateTime currentTime = DateTime.Now;
                LoggerHelper.WriteDebugLog($"input : {S_SN},{JsonConvert.SerializeObject(List_Login)}; result :{result}; {startDateTime} - {currentTime}, cost {(currentTime - startDateTime).TotalMilliseconds} ms");
            }
            catch (Exception e)
            {
                result = e.Message;
                LoggerHelper.WriteErrorLog($" {S_SN},{JsonConvert.SerializeObject(List_Login)}; result :{result}; {e.Message}\r\n cost {(DateTime.Now - startDateTime).TotalMilliseconds} ms");
            }
            return result;
        }
        public string MainSN_OutFGSN(string S_SN, LoginList List_Login)
        {
            //return new SFCAPIDAL().MainSN_OutFGSN(S_SN, List_Login);
            string result = "";
            DateTime startDateTime = DateTime.Now;
            try
            {
                result = new SFCAPIDAL().MainSN_OutFGSN(S_SN, List_Login);
                DateTime currentTime = DateTime.Now;
                LoggerHelper.WriteDebugLog($"input : {S_SN},{JsonConvert.SerializeObject(List_Login)}; result :{result}; {startDateTime} - {currentTime}, cost {(currentTime - startDateTime).TotalMilliseconds} ms");
            }
            catch (Exception e)
            {
                result = e.Message;
                LoggerHelper.WriteErrorLog(e.Message + $"\r\n cost {(DateTime.Now - startDateTime).TotalMilliseconds} ms");
            }
            return result;
        }

        public string MainSN_Check_OutCCCode(string S_SN, LoginList List_Login, out string S_CCCode)
        {
            //return new SFCAPIDAL().MainSN_Check_OutCCCode(S_SN, List_Login,out S_CCCode);
            S_CCCode = "";
            string result = "";
            DateTime startDateTime = DateTime.Now;
            try
            {
                result = new SFCAPIDAL().MainSN_Check_OutCCCode(S_SN, List_Login, out S_CCCode);
                DateTime currentTime = DateTime.Now;
                LoggerHelper.WriteDebugLog($"input : {S_SN},{JsonConvert.SerializeObject(List_Login)}, out cccCode :{S_CCCode}; result :{result}; {startDateTime} - {currentTime}, cost {(currentTime - startDateTime).TotalMilliseconds} ms");
            }
            catch (Exception e)
            {
                result = e.Message;
                LoggerHelper.WriteErrorLog(e.Message + $"\r\n cost {(DateTime.Now - startDateTime).TotalMilliseconds} ms");
            }
            return result;
        }

        public string MainSN_Check_OutCCCodeV2(string S_SN, LoginList List_Login)
        {
            //return new SFCAPIDAL().MainSN_Check_OutCCCodeV2(S_SN, List_Login);
            string result = "";
            DateTime startDateTime = DateTime.Now;
            try
            {
                result = new SFCAPIDAL().MainSN_Check_OutCCCodeV2(S_SN, List_Login);
                DateTime currentTime = DateTime.Now;
                LoggerHelper.WriteDebugLog($"input : {S_SN},{JsonConvert.SerializeObject(List_Login)}; result :{result}; {startDateTime} - {currentTime}, cost {(currentTime - startDateTime).TotalMilliseconds} ms");
            }
            catch (Exception e)
            {
                result = e.Message;
                LoggerHelper.WriteErrorLog(e.Message + $"\r\n cost {(DateTime.Now - startDateTime).TotalMilliseconds} ms");
            }
            return result;
        }

        public string Update_SFC(string S_SN, int I_StatusID, LoginList List_Login, string S_DefectCode)
        {
            //return new SFCAPIDAL().Update_SFC(S_SN,I_StatusID, List_Login, S_DefectCode);
            string result = "";
            DateTime startDateTime = DateTime.Now;
            try
            {
                result = new SFCAPIDAL().Update_SFC(S_SN, I_StatusID, List_Login, S_DefectCode);
                DateTime currentTime = DateTime.Now;
                LoggerHelper.WriteDebugLog($"input : {S_SN},{I_StatusID},{JsonConvert.SerializeObject(List_Login)},{S_DefectCode}; result :{result}; {startDateTime} - {currentTime}, cost {(currentTime - startDateTime).TotalMilliseconds} ms");
            }
            catch (Exception e)
            {
                result = e.Message;
                LoggerHelper.WriteErrorLog($"input : {S_SN},{I_StatusID},{JsonConvert.SerializeObject(List_Login)},{S_DefectCode}; {e.Message}\r\n cost {(DateTime.Now - startDateTime).TotalMilliseconds} ms");
            }
            return result;
        }

        public string Update_Assy_SFC(string S_SN, int I_StatusID, LoginList List_Login, string S_DefectCode,
            string S_ChildCode, Tuple<BomInfo[], string> BomInfos)
        {
            //return new SFCAPIDAL().Update_Assy_SFC(S_SN, I_StatusID, List_Login, S_DefectCode, S_ChildCode,  BomInfos);
            string result = "";
            DateTime startDateTime = DateTime.Now;
            try
            {
                result = new SFCAPIDAL().Update_Assy_SFC(S_SN, I_StatusID, List_Login, S_DefectCode, S_ChildCode, BomInfos);
                DateTime currentTime = DateTime.Now;
                LoggerHelper.WriteDebugLog($"input : {S_SN},{I_StatusID},{JsonConvert.SerializeObject(List_Login)},{S_DefectCode},{S_ChildCode},{JsonConvert.SerializeObject(BomInfos)}; result :{result}; {startDateTime} - {currentTime}, cost {(currentTime - startDateTime).TotalMilliseconds} ms");
            }
            catch (Exception e)
            {
                result = e.Message;
                LoggerHelper.WriteErrorLog($"input : {S_SN},{I_StatusID},{JsonConvert.SerializeObject(List_Login)},{S_DefectCode},{S_ChildCode},{JsonConvert.SerializeObject(BomInfos)}; {e.Message}\r\n cost {(DateTime.Now - startDateTime).TotalMilliseconds} ms");
            }
            return result;
        }

        public Tuple<BomInfo[], string> GetBomInfos(int ParentID, int StationTypeID)
        {
            //return new SFCAPIDAL().GetBomInfos(ParentID, StationTypeID);
            Tuple<BomInfo[], string> result = null;
            DateTime startDateTime = DateTime.Now;
            try
            {
                result = new SFCAPIDAL().GetBomInfos(ParentID, StationTypeID);
                DateTime currentTime = DateTime.Now;
                LoggerHelper.WriteDebugLog($"input : {ParentID},{StationTypeID}; result :{JsonConvert.SerializeObject(result) }; {startDateTime} - {currentTime}, cost {(currentTime - startDateTime).TotalMilliseconds} ms");
            }
            catch (Exception e)
            {
                if (result == null)
                {
                    result = new Tuple<BomInfo[], string>(null, e.Message);
                }
                LoggerHelper.WriteErrorLog($"input : {ParentID},{StationTypeID}; result :{JsonConvert.SerializeObject(result) }; {e.Message}\r\n cost {(DateTime.Now - startDateTime).TotalMilliseconds} ms");
            }
            return result;
        }
        public DataSet GetEmployee(string S_UserName)
        {
            return new SFCAPIDAL().GetEmployee(S_UserName);
        }

        public int GetUser(string UserID, string Password)
        {
            return new SFCAPIDAL().GetUser(UserID, Password);
        }

        public DataSet GetmesLine()
        {
            return new SFCAPIDAL().GetmesLine();
        }

        public DataSet GetluPartFamilyType()
        {
            return new SFCAPIDAL().GetluPartFamilyType();
        }

        public DataSet GetluPartFamily(string PartFamilyTypeID)
        {
            return new SFCAPIDAL().GetluPartFamily(PartFamilyTypeID);
        }

        public DataSet GetmesPart(string PartFamilyID)
        {
            return new SFCAPIDAL().GetmesPart(PartFamilyID);
        }

        public DataSet GetmesPartOne(string ID)
        {
            return new SFCAPIDAL().GetmesPartOne(ID);
        }

        public DataSet GetmesStationType()
        {
            return new SFCAPIDAL().GetmesStationType();
        }

        public DataSet GetmesStation(string LineID)
        {
            return new SFCAPIDAL().GetmesStation(LineID);
        }

        public DataSet GetPOAll(string S_PartID, string S_LineID)
        {
            return new SFCAPIDAL().GetPOAll(S_PartID, S_LineID);
        }

        public string SaveStationSet(string StationID, string Value)
        {
            return new SFCAPIDAL().SaveStationSet(StationID, Value);
        }

    }
}
