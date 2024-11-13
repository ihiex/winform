using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IluSerialNumberType”。
[ServiceContract]
public interface IluSerialNumberTypeSVC
{
    [OperationContract]
    int Insert(luSerialNumberType model);
    [OperationContract]
    bool Delete(int id);
    [OperationContract]
    bool Update(luSerialNumberType model);
    [OperationContract]
    luSerialNumberType Get(int id);
    [OperationContract]
    IEnumerable<luSerialNumberType> ListAll(string S_Where);
}
