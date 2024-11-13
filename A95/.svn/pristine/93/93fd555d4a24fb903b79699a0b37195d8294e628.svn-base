using App.BLL;
using App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“mesRouteDetail”。
public class mesRouteDetailSVC : ImesRouteDetailSVC
{
    mesRouteDetailBLL F_BLL = new mesRouteDetailBLL();
    public int Insert(mesRouteDetail model)
    {
        return F_BLL.Insert(model);
    }

    public bool Delete(int id)
    {
        return F_BLL.Delete(id);
    }

    public bool Update(mesRouteDetail model)
    {
        return F_BLL.Update(model);
    }

    public mesRouteDetail Get(int id)
    {
        return F_BLL.Get(id);
    }

    public IEnumerable<mesRouteDetail> ListAll(string S_Where)
    {
        return F_BLL.ListAll(S_Where);
    }

    public DataSet GetRouteDetail(string LineID, string PartID, string PartFamilyID,string ProductionOrderID)
    {
        return F_BLL.GetRouteDetail(LineID, PartID, PartFamilyID, ProductionOrderID);
    }
    public DataSet GetRouteDetail2(string LineID, string PartID, string PartFamilyID,string ProductionOrderID)
    {
        return F_BLL.GetRouteDetail2(LineID, PartID, PartFamilyID, ProductionOrderID);
    }
}
