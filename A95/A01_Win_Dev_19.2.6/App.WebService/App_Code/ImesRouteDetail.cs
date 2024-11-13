﻿using App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ImesRouteDetail”。
[ServiceContract]
public interface ImesRouteDetailSVC
{
    [OperationContract]
    int Insert(mesRouteDetail model);
    [OperationContract]
    bool Delete(int id);
    [OperationContract]
    bool Update(mesRouteDetail model);
    [OperationContract]
    mesRouteDetail Get(int id);
    [OperationContract]
    IEnumerable<mesRouteDetail> ListAll(string S_Where);
    [OperationContract]
    DataSet GetRouteDetail(string LineID, string PartID, string PartFamilyID, string ProductionOrderID);

    [OperationContract]
    DataSet GetRouteDetail2(string LineID, string PartID, string PartFamilyID, string ProductionOrderID);
}
