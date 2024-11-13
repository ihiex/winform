using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   mesSerialNumberBLL
	{
		public int Insert(mesSerialNumber model)
		{
			return new mesSerialNumberDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new mesSerialNumberDAL().Delete(id);
		}

		public bool Update(mesSerialNumber model)
		{
			return new mesSerialNumberDAL().Update(model);
		}

		public mesSerialNumber Get(int id)
		{
			return new mesSerialNumberDAL().Get(id);
		}

		public IEnumerable<mesSerialNumber> ListAll(string S_Where)
		{
			return new mesSerialNumberDAL().ListAll(S_Where);
		}
	}
}
