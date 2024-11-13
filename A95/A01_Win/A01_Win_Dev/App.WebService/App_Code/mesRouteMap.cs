using App.BLL;
using App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“mesRouteMap”。
public class mesRouteMapSVC : ImesRouteMapSVC
{
    mesRouteMapBLL F_BLL = new mesRouteMapBLL();
    public int Insert(mesRouteMap model)
    {
        return F_BLL.Insert(model);
    }

    public bool Delete(int id)
    {
        return F_BLL.Delete(id);
    }

    public bool Update(mesRouteMap model)
    {
        return F_BLL.Update(model);
    }

    public DataSet MesGetPartIDByMachineSN(int stationTypeID, string MachineSN)
    {
        return F_BLL.MesGetPartIDByMachineSN(stationTypeID, MachineSN);
    }

    public mesRouteMap Get(int id)
    {
        return F_BLL.Get(id);
    }

    public IEnumerable<mesRouteMap> ListAll(string S_Where)
    {
        return F_BLL.ListAll(S_Where);
    }
}
