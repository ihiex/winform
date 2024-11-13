using App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ITest1”。
[ServiceContract]
public interface ITest1
{
    [OperationContract]
    void DoWork();


    [OperationContract]
    DataSet GetTest1();
    [OperationContract]
    DataSet GetTest2(string strSql);

    [OperationContract]
    string Insert(App_Test1 model);

    [OperationContract]
    string Update(App_Test1 model);
    [OperationContract]
    string Delete(App_Test1 model);
}
