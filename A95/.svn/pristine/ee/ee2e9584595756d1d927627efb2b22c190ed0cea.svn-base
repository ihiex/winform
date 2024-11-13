using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   luUnitStatusBLL
	{
		public int Insert(luUnitStatus model)
		{
			return new luUnitStatusDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new luUnitStatusDAL().Delete(id);
		}

		public bool Update(luUnitStatus model)
		{
			return new luUnitStatusDAL().Update(model);
		}

		public luUnitStatus Get(int id)
		{
			return new luUnitStatusDAL().Get(id);
		}

		public IEnumerable<luUnitStatus> ListAll(string S_Where)
		{
			return new luUnitStatusDAL().ListAll(S_Where);
		}
	}
}
