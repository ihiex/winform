using App.DBServerDAL;
using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL
{
    public class mesPackageBLL
    {
        public int Insert(mesPackage model)
        {
            return new mesPackageDAL().Insert(model);
        }

        public bool Delete(int id)
        {
            return new mesPackageDAL().Delete(id);
        }

        public bool Update(mesPackage model)
        {
            return new mesPackageDAL().Update(model);
        }

        public mesPackage Get(int id)
        {
            return new mesPackageDAL().Get(id);
        }

        public IEnumerable<mesPackage> ListAll(string S_Where)
        {
            return new mesPackageDAL().ListAll(S_Where);
        }
    }
}
