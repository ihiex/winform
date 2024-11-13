using App.DBServerDAL;
using App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL
{
    public class sysTest1
    {
        //private readonly IApp_Test1 dal = App.DALFactory.DataAccess.CreatApp_Test1();

        sql_Test1 dal = new sql_Test1(); 

        public string Insert(App_Test1 model)
        {
            return dal.Insert(model);
        }

        public string Update(App_Test1 model)
        {
            return dal.Update(model);
        }
        public string Delete(App_Test1 model)
        {
            return dal.Delete(model);

        }
        public DataSet GetTest1()
        {
            return dal.GetTest1();
        }
        public DataSet GetTest2(string strSql)
        {
            return dal.GetTest2(strSql);
        }
    }
}
