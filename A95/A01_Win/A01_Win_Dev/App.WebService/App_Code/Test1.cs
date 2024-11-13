using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using App.BLL;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Test1”。
public class Test1 : ITest1
{
    public void DoWork()
    {
    }

    public DataSet GetTest1()
    {
        sysTest1 F_Test = new sysTest1();
        return F_Test.GetTest1();
    }

    public DataSet GetTest2(string strSql)
    {
        sysTest1 F_Test = new sysTest1();
        return F_Test.GetTest2(strSql);
    }

    public string Insert(App.Model.App_Test1 model)
    {
        sysTest1 F_Test = new sysTest1();
        return F_Test.Insert(model);
    }

    public string Update(App.Model.App_Test1 model)
    {
        sysTest1 F_Test = new sysTest1();
        return F_Test.Update(model);
    }

    public string Delete(App.Model.App_Test1 model)
    {
        sysTest1 F_Test = new sysTest1();
        return F_Test.Delete(model);
    }
}
