using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using App.BLL;
using App.Model;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“mesUnitDetail”。
public class mesUnitDetailSVC : ImesUnitDetailSVC
{
    mesUnitDetailBLL F_BLL = new mesUnitDetailBLL();
    public string Insert(mesUnitDetail model)
    {
        return F_BLL.Insert(model);
    }

    public bool Delete(int id)
    {
        return F_BLL.Delete(id);
    }

    public string Update(mesUnitDetail model)
    {
        return F_BLL.Update(model);
    }

    public mesUnitDetail Get(int id)
    {
        return F_BLL.Get(id);
    }
    public mesUnitDetail GetUnitDetail(int unitid)
    {
        return F_BLL.GetUnitDetail(unitid);
    }

    public IEnumerable<mesUnitDetail> ListAll(string S_Where)
    {
        return F_BLL.ListAll(S_Where);
    }

    public DataSet MesGetBatchIDByBarcodeSN(string BarcodeSN)
    {
        return F_BLL.MesGetBatchIDByBarcodeSN(BarcodeSN);
    }

    public string MesGetFGSNByUPCSN(string UPCSN)
    {
        return F_BLL.MesGetFGSNByUPCSN(UPCSN);
    }

    public void UpdateKitSerialnumber(int UnitID, string KitSerialnumber)
    {
        F_BLL.UpdateKitSerialnumber(UnitID, KitSerialnumber);
    }
}
