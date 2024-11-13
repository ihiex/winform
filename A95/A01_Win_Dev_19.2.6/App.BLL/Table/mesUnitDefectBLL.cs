using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Model;
using App.DBServerDAL;


namespace App.BLL
{
    public class mesUnitDefectBLL
    {
        public string Insert(mesUnitDefect model)
        {
            return new mesUnitDefectDAL().Insert(model);
        }

        public bool Delete(int id)
        {
            return new mesUnitDefectDAL().Delete(id);
        }

        public string Update(mesUnitDefect model)
        {
            return new mesUnitDefectDAL().Update(model);
        }

        public mesUnitDefect Get(int id)
        {
            return new mesUnitDefectDAL().Get(id);
        }

        public IEnumerable<mesUnitDefect> ListAll(string S_Where)
        {
            return new mesUnitDefectDAL().ListAll(S_Where);
        }
    }
}
