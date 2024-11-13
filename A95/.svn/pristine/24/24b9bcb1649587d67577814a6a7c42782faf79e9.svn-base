using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   luEmployeeGroupBLL
	{
		public int Insert(luEmployeeGroup model)
		{
			return new luEmployeeGroupDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new luEmployeeGroupDAL().Delete(id);
		}

		public bool Update(luEmployeeGroup model)
		{
			return new luEmployeeGroupDAL().Update(model);
		}

		public luEmployeeGroup Get(int id)
		{
			return new luEmployeeGroupDAL().Get(id);
		}

		public IEnumerable<luEmployeeGroup> ListAll(string S_Where)
		{
			return new luEmployeeGroupDAL().ListAll(S_Where);
		}
	}
}
