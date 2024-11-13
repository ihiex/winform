using App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ImesProductStructure”。
[ServiceContract]
public interface ImesProductStructureSVC
{
    [OperationContract]
    int Insert(mesProductStructure model);
    [OperationContract]
    bool Delete(int id);
    [OperationContract]
    bool Update(mesProductStructure model);
    [OperationContract]
    mesProductStructure Get(int id);
    [OperationContract]
    IEnumerable<mesProductStructure> ListAll(string S_Where);
    [OperationContract]
    DataSet GetBOMStructure(string ParentPartID, string PartID, string StationTypeID);
}
