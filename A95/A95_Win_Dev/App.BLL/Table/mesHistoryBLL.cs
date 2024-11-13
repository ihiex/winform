using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   mesHistoryBLL
	{
		public int Insert(mesHistory model)
		{
			return new mesHistoryDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new mesHistoryDAL().Delete(id);
		}

		public bool Update(mesHistory model)
		{
			return new mesHistoryDAL().Update(model);
		}

		public mesHistory Get(int id)
		{
			return new mesHistoryDAL().Get(id);
		}

		public IEnumerable<mesHistory> ListAll(string S_Where)
		{
			return new mesHistoryDAL().ListAll(S_Where);
		}
	}
}
