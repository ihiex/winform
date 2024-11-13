using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ImesSerialNumber”。
[ServiceContract]
public interface ImesSerialNumberSVC
{
    [OperationContract]
    int Insert(mesSerialNumber model);
    [OperationContract]
    bool Delete(int id);
    [OperationContract]
    bool Update(mesSerialNumber model);
    [OperationContract]
    mesSerialNumber Get(int id);
    [OperationContract]
    IEnumerable<mesSerialNumber> ListAll(string S_Where);
}
