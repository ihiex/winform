using App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ISFCAPI”。
[ServiceContract]
public interface ISFCAPISVC
{
    [OperationContract]
    string MainSN_Check(string S_SN, LoginList List_Login);
    [OperationContract]
    string MainSN_OutFGSN(string S_SN, LoginList List_Login);

    [OperationContract]
    string MainSN_Check_OutCCCode(string S_SN, LoginList List_Login, out string S_CCCode);
    [OperationContract]
    string MainSN_Check_OutCCCodeV2(string S_SN, LoginList List_Login);


    [OperationContract]
    string Update_SFC(string S_SN, int I_StatusID, LoginList List_Login, string S_DefectCode);

    [OperationContract]
    DataSet GetEmployee(string S_UserName);


    [OperationContract]
    int GetUser(string UserID, string Password);
    [OperationContract]
    DataSet GetmesLine();
    [OperationContract]
    DataSet GetluPartFamilyType();
    [OperationContract]
    DataSet GetluPartFamily(string PartFamilyTypeID);
    [OperationContract]
    DataSet GetmesPart(string PartFamilyID);
    [OperationContract]
    DataSet GetmesPartOne(string ID);
    [OperationContract]
    DataSet GetmesStationType();
    [OperationContract]
    DataSet GetmesStation(string LineID);
    [OperationContract]
    DataSet GetPOAll(string S_PartID, string S_LineID);
    [OperationContract]
    string SaveStationSet(string StationID, string Value);
}
