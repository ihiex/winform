using App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ImesMachine”。
[ServiceContract]
public interface ImesMachineSVC
{
    [OperationContract]
    int Insert(mesMachine model);
    [OperationContract]
    bool Delete(int id);
    [OperationContract]
    bool Update(mesMachine model);
    [OperationContract]
    mesMachine Get(int id);
    [OperationContract]
    IEnumerable<mesMachine> ListAll(string S_Where);
    [OperationContract]
    DataSet MesGetLineIDByMachineSN(string MachineSN);
    [OperationContract]
    DataSet MesGetStatusIDByList(int StationTypeID, int PartID, string MachineSN);

    [OperationContract]
    void MesModMachineBySNStationTypeID(string MachineSN, int StationTypeID);
    [OperationContract]
    string MesModMachineBySNStationTypeID_Sql(string MachineSN, int StationTypeID);


    [OperationContract]
    void MesModMachineBySN(string MachineSN);
    [OperationContract]
    void SetMachineRuningQuantity(string MachineSN);
    [OperationContract]
    string MesToolingReleaseCheck(string MachineSN, string StationTypeID);
}
