using App.BLL;
using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“mesEmployee”。
public class mesEmployeeSVC : ImesEmployeeSVC
{
    mesEmployeeBLL F_BLL = new mesEmployeeBLL();
    public int Insert(mesEmployee model)
    {
        return F_BLL.Insert(model);
    }

    public bool Delete(int id)
    {
        return F_BLL.Delete(id);
    }

    public bool Update(mesEmployee model)
    {
        return F_BLL.Update(model);
    }

    public mesEmployee Get(int id)
    {
        return F_BLL.Get(id);
    }

    public IEnumerable<mesEmployee> ListAll(string S_Where)
    {
        return F_BLL.ListAll(S_Where);
    }

    public bool PermissionCheck(int EmployeeID, int StationTypeID)
    {
        return F_BLL.PermissionCheck(EmployeeID, StationTypeID);
    }

    public string ValidateSecond(string UserID, string PWD)
    {
        return F_BLL.ValidateSecond(UserID, PWD);
    }

    public string ChangePWD(string UserID, string OldPWD, string NewPWD)
    {
        return F_BLL.ChangePWD(UserID, OldPWD, NewPWD);
    }
}
