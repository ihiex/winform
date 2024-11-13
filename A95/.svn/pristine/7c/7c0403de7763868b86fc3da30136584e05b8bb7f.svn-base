using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   mesRouteMachineMapBLL
	{
		public int Insert(mesRouteMachineMap model)
		{
			return new mesRouteMachineMapDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new mesRouteMachineMapDAL().Delete(id);
		}

		public bool Update(mesRouteMachineMap model)
		{
			return new mesRouteMachineMapDAL().Update(model);
		}

		public mesRouteMachineMap Get(int id)
		{
			return new mesRouteMachineMapDAL().Get(id);
		}

		public IEnumerable<mesRouteMachineMap> ListAll(string S_Where)
		{
			return new mesRouteMachineMapDAL().ListAll(S_Where);
		}
	}
}
