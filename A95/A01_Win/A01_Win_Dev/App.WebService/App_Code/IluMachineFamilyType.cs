using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IluMachineFamilyType”。
[ServiceContract]
public interface IluMachineFamilyTypeSVC
{
    [OperationContract]
    int Insert(luMachineFamilyType model);
    [OperationContract]
    bool Delete(int id);
    [OperationContract]
    bool Update(luMachineFamilyType model);
    [OperationContract]
    luMachineFamilyType Get(int id);
    [OperationContract]
    IEnumerable<luMachineFamilyType> ListAll(string S_Where);
}
