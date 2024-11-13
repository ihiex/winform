using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Model;
using App.DBUtility;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using System.Configuration;

namespace App.DBServerDAL
{
    public class sql_Test1
    {
        public string Insert(App_Test1 model)
        {
            string strSql = "insert into Test1(AAA,BBB) Values(" +"\r\n"+
                            "'" + model.AAA + "'," + "\r\n" +
                            "'" + model.BBB + "'" + "\r\n" +
                            ")";
            
            return SqlServerHelper.ExecSql(strSql);
        }

        public string Delete(App_Test1 model)
        {
            string strSql = "delete Test1 where Id='" + model.ID + "'";
      
            return SqlServerHelper.ExecSql(strSql);
        }

        public string Update(App_Test1 model)
        {
            string strSql = "update Test1 set " + "\r\n" +
                            "AAA='" + model.AAA + "'," + "\r\n" +
                            "BBB='" + model.BBB + "'" + "\r\n" +
                            "where Id='" + model.ID + "'";
                                       
            return SqlServerHelper.ExecSql(strSql);
        }

        public DataSet GetTest1()
        {
            string strSql = "select * from Test1";
            DataSet ds = SqlServerHelper.Data_Set(strSql);
            return ds;
        }
        public DataSet GetTest2(string strSql)
        {            
            DataSet ds = SqlServerHelper.Data_Set(strSql);

            return ds;
        }

    }
}
