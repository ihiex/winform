using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ImesStationType”。
[ServiceContract]
public interface ImesStationTypeSVC
{
    [OperationContract]
    int Insert(mesStationType model);
    [OperationContract]
    bool Delete(int id);
    [OperationContract]
    bool Update(mesStationType model);
    [OperationContract]
    mesStationType Get(int id);
    [OperationContract]
    IEnumerable<mesStationType> ListAll(string S_Where);
}
