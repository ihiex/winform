using App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Web;

/// <summary>
/// mesPackage 的摘要说明
/// </summary>
[ServiceContract]
public interface ImesMaterialUnitSVC
{
    [OperationContract]
    int Insert(MesMaterialUnit model);
    [OperationContract]
    int InserDetail(MesMaterialUnitDetail model);
    [OperationContract]
    int InserForParent(MesMaterialUnit model);
    [OperationContract]
    bool Delete(int id);
    [OperationContract]
    bool Update(MesMaterialUnit model);
    [OperationContract]
    MesMaterialUnit Get(int id);
    [OperationContract]
    IEnumerable<MesMaterialUnit> ListAll(string S_Where);
    [OperationContract]
    DataSet GetMesMaterialUnitByLotCode(string PartID,string LotCode);
    [OperationContract]
    int GetMesMaterialUseQTY(string MaterialUnitID);
    [OperationContract]
    DataSet GetMesMaterialUnitData(string SerialNumber);
}