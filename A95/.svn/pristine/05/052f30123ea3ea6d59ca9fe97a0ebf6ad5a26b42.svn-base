using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ImesRouteMachineMap”。
[ServiceContract]
public interface ImesRouteMachineMapSVC
{
    [OperationContract]
    int Insert(mesRouteMachineMap model);
    [OperationContract]
    bool Delete(int id);
    [OperationContract]
    bool Update(mesRouteMachineMap model);
    [OperationContract]
    mesRouteMachineMap Get(int id);
    [OperationContract]
    IEnumerable<mesRouteMachineMap> ListAll(string S_Where);
}
