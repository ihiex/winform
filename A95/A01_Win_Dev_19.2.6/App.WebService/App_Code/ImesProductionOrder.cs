using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ImesProductionOrder”。
[ServiceContract]
public interface ImesProductionOrderSVC
{
    [OperationContract]
    string Insert(mesProductionOrder model);
    [OperationContract]
    bool Delete(int id);
    [OperationContract]
    string Update(mesProductionOrder model);
    [OperationContract]
    mesProductionOrder Get(int id);
    [OperationContract]
    IEnumerable<mesProductionOrder> ListAll(string S_Where);
}
