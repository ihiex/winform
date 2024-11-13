using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

/// <summary>
/// mesPackage 的摘要说明
/// </summary>
[ServiceContract]
public interface ImesPackageSVC
{
    [OperationContract]
    int Insert(mesPackage model);
    [OperationContract]
    bool Delete(int id);
    [OperationContract]
    bool Update(mesPackage model);
    [OperationContract]
    mesPackage Get(int id);
    [OperationContract]
    IEnumerable<mesPackage> ListAll(string S_Where);
}