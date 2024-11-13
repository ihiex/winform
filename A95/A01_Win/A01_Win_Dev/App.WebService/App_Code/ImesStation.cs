using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ImesStation”。
[ServiceContract]
public interface ImesStationSVC
{
    [OperationContract]
    int Insert(mesStation model);
    [OperationContract]
    bool Delete(int id);
    [OperationContract]
    bool Update(mesStation model);
    [OperationContract]
    mesStation Get(int id);
    [OperationContract]
    IEnumerable<mesStation> ListAll(string S_Where);
}
