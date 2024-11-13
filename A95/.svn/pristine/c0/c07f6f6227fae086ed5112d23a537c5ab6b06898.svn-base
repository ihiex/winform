using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
    public class mesSerialNumberOfLineBLL
    {
        public int Insert(mesSerialNumberOfLine model)
        {
            return new mesSerialNumberOfLineDAL().Insert(model);
        }

        public bool Delete(int id)
        {
            return new mesSerialNumberOfLineDAL().Delete(id);
        }

        public bool Update(mesSerialNumberOfLine model)
        {
            return new mesSerialNumberOfLineDAL().Update(model);
        }

        public mesSerialNumberOfLine Get(int id)
        {
            return new mesSerialNumberOfLineDAL().Get(id);
        }

        public IEnumerable<mesSerialNumberOfLine> ListAll(string S_Where)
        {
            return new mesSerialNumberOfLineDAL().ListAll(S_Where);
        }
    }
}
