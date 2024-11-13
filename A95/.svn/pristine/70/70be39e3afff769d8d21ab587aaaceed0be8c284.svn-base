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
public class mesMachineSVC : ImesMachineSVC
{
    mesMachineBLL F_BLL = new mesMachineBLL();
    public int Insert(mesMachine model)
    {
        return F_BLL.Insert(model);
    }

    public bool Delete(int id)
    {
        return F_BLL.Delete(id);
    }

    public bool Update(mesMachine model)
    {
        return F_BLL.Update(model);
    }

    public mesMachine Get(int id)
    {
        return F_BLL.Get(id);
    }

    public IEnumerable<mesMachine> ListAll(string S_Where)
    {
        return F_BLL.ListAll(S_Where);
    }

    public DataSet MesGetLineIDByMachineSN(string MachineSN)
    {
        return F_BLL.MesGetLineIDByMachineSN(MachineSN);
    }

    public DataSet MesGetStatusIDByList(int StationTypeID, int PartID, string MachineSN)
    {
        return F_BLL.MesGetStatusIDByList(StationTypeID, PartID, MachineSN);
    }

    public void MesModMachineBySNStationTypeID(string MachineSN, int StationTypeID)
    {
        F_BLL.MesModMachineBySNStationTypeID(MachineSN, StationTypeID);
    }

    public void MesModMachineBySN(string MachineSN)
    {
        F_BLL.MesModMachineBySN(MachineSN);
    }

    public void SetMachineRuningQuantity(string MachineSN)
    {
        F_BLL.SetMachineRuningQuantity(MachineSN);
    }
}
