using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   mesUnitBLL
	{
		public string Insert(mesUnit model)
		{
			return new mesUnitDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new mesUnitDAL().Delete(id);
		}

		public string Update(mesUnit model)
		{
			return new mesUnitDAL().Update(model);
		}

		public mesUnit Get(int id)
		{
			return new mesUnitDAL().Get(id);
		}

		public IEnumerable<mesUnit> ListAll(string S_Where)
		{
			return new mesUnitDAL().ListAll(S_Where);
		}
	}
}
