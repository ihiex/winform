using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IluPartFamilyType”。
[ServiceContract]
public interface IluPartFamilyTypeSVC
{
    [OperationContract]
    int Insert(luPartFamilyType model);
    [OperationContract]
    bool Delete(int id);
    [OperationContract]
    bool Update(luPartFamilyType model);
    [OperationContract]
    luPartFamilyType Get(int id);
    [OperationContract]
    IEnumerable<luPartFamilyType> ListAll(string S_Where);
}
