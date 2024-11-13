using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   mesProductionOrderBLL
	{
		public string Insert(mesProductionOrder model)
		{
			return new mesProductionOrderDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new mesProductionOrderDAL().Delete(id);
		}

		public string Update(mesProductionOrder model)
		{
			return new mesProductionOrderDAL().Update(model);
		}

		public mesProductionOrder Get(int id)
		{
			return new mesProductionOrderDAL().Get(id);
		}

		public IEnumerable<mesProductionOrder> ListAll(string S_Where)
		{
			return new mesProductionOrderDAL().ListAll(S_Where);
		}
	}
}
