using App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ImesRouteMap”。
[ServiceContract]
public interface ImesRouteMapSVC
{
    [OperationContract]
    int Insert(mesRouteMap model);
    [OperationContract]
    bool Delete(int id);
    [OperationContract]
    bool Update(mesRouteMap model);
    [OperationContract]
    DataSet MesGetPartIDByMachineSN(int stationTypeID, string MachineSN);
    [OperationContract]
    mesRouteMap Get(int id);
    [OperationContract]
    IEnumerable<mesRouteMap> ListAll(string S_Where);
}
