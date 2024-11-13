using App.BLL;
using App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“mesMachine”。
public class mesMaterialUnitSVC : ImesMaterialUnitSVC
{

    MesMaterialUnitBLL F_BLL = new MesMaterialUnitBLL();
    public int Insert(MesMaterialUnit model)
    {
        return F_BLL.Insert(model);
    }

    public int InserDetail(MesMaterialUnitDetail model)
    {
        return F_BLL.InserDetail(model);
    }

    public int InserForParent(MesMaterialUnit model)
    {
        return F_BLL.InserForParent(model);
    }

    public bool Delete(int id)
    {
        return F_BLL.Delete(id);
    }

    public bool Update(MesMaterialUnit model)
    {
        return F_BLL.Update(model);
    }

    public MesMaterialUnit Get(int id)
    {
        return F_BLL.Get(id);
    }

    public IEnumerable<MesMaterialUnit> ListAll(string S_Where)
    {
        return F_BLL.ListAll(S_Where);
    }

    public DataSet GetMesMaterialUnitByLotCode(string PartID, string LotCode)
    {
        return F_BLL.GetMesMaterialUnitByLotCode(PartID,LotCode);
    }

    public int GetMesMaterialUseQTY(string MaterialUnitID)
    {
        return F_BLL.GetMesMaterialUseQTY(MaterialUnitID);
    }

    public DataSet GetMesMaterialUnitData(string SerialNumber)
    {
        return F_BLL.GetMesMaterialUnitData(SerialNumber);
    }
}