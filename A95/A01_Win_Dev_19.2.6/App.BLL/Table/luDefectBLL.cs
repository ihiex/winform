using App.DBServerDAL;
using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL
{
    public class luDefectBLL
    {
        public string Insert(luDefect model)
        {
            return new luDefectDAL().Insert(model);
        }

        public bool Delete(int id)
        {
            return new luDefectDAL().Delete(id);
        }

        public string Update(luDefect model)
        {
            return new luDefectDAL().Update(model);
        }

        public luDefect Get(int id)
        {
            return new luDefectDAL().Get(id);
        }

        public IEnumerable<luDefect> ListAll(string S_Where)
        {
            return new luDefectDAL().ListAll(S_Where);
        }
    }
}
