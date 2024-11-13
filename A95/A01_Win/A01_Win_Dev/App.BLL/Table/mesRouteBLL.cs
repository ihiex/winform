using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   mesRouteBLL
	{
		public int Insert(mesRoute model)
		{
			return new mesRouteDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new mesRouteDAL().Delete(id);
		}

		public bool Update(mesRoute model)
		{
			return new mesRouteDAL().Update(model);
		}

		public mesRoute Get(int id)
		{
			return new mesRouteDAL().Get(id);
		}

		public IEnumerable<mesRoute> ListAll(string S_Where)
		{
			return new mesRouteDAL().ListAll(S_Where);
		}
	}
}
