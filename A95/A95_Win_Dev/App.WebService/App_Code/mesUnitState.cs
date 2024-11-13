using App.BLL;
using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“mesUnitState”。
public class mesUnitStateSVC : ImesUnitStateSVC
{
    mesUnitStateBLL F_BLL = new mesUnitStateBLL();
    public int Insert(mesUnitState model)
    {
        return F_BLL.Insert(model);
    }

    public bool Delete(int id)
    {
        return F_BLL.Delete(id);
    }

    public bool Update(mesUnitState model)
    {
        return F_BLL.Update(model);
    }

    public mesUnitState Get(int id)
    {
        return F_BLL.Get(id);
    }

    public IEnumerable<mesUnitState> ListAll(string S_Where)
    {
        return F_BLL.ListAll(S_Where);
    }
}
