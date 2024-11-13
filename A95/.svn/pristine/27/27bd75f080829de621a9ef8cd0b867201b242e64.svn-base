using App.BLL;
using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// mesPackage 的摘要说明
/// </summary>
public class mesPackageSVC : ImesPackageSVC
{
    mesPackageBLL F_BLL = new mesPackageBLL();
    public int Insert(mesPackage model)
    {
        return F_BLL.Insert(model);
    }

    public bool Delete(int id)
    {
        return F_BLL.Delete(id);
    }

    public bool Update(mesPackage model)
    {
        return F_BLL.Update(model);
    }

    public mesPackage Get(int id)
    {
        return F_BLL.Get(id);
    }

    public IEnumerable<mesPackage> ListAll(string S_Where)
    {
        return F_BLL.ListAll(S_Where);
    }
}