using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ImesUnit”。
[ServiceContract]
public interface ImesUnitSVC
{
    [OperationContract]
    string Insert(mesUnit model);
    [OperationContract]
    bool Delete(int id);
    [OperationContract]
    string Update(mesUnit model);
    [OperationContract]
    mesUnit Get(int id);
    [OperationContract]
    IEnumerable<mesUnit> ListAll(string S_Where);
    [OperationContract]
    string UpdatePlasma(string SN, int StationID, string LastUpdate, int LineID);
}
