using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   mesUnitComponentBLL
	{
		public string Insert(mesUnitComponent model)
		{
			return new mesUnitComponentDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new mesUnitComponentDAL().Delete(id);
		}

		public bool Update(mesUnitComponent model)
		{
			return new mesUnitComponentDAL().Update(model);
		}

		public mesUnitComponent Get(int id)
		{
			return new mesUnitComponentDAL().Get(id);
		}

		public IEnumerable<mesUnitComponent> ListAll(string S_Where)
		{
			return new mesUnitComponentDAL().ListAll(S_Where);
		}
	}
}
