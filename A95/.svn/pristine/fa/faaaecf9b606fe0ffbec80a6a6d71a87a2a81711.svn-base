using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   mesRouteDetailBLL
	{
		public int Insert(mesRouteDetail model)
		{
			return new mesRouteDetailDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new mesRouteDetailDAL().Delete(id);
		}

		public bool Update(mesRouteDetail model)
		{
			return new mesRouteDetailDAL().Update(model);
		}

		public mesRouteDetail Get(int id)
		{
			return new mesRouteDetailDAL().Get(id);
		}

		public IEnumerable<mesRouteDetail> ListAll(string S_Where)
		{
			return new mesRouteDetailDAL().ListAll(S_Where);
		}
	}
}
