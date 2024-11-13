using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   mesUnitStateBLL
	{
		public int Insert(mesUnitState model)
		{
			return new mesUnitStateDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new mesUnitStateDAL().Delete(id);
		}

		public bool Update(mesUnitState model)
		{
			return new mesUnitStateDAL().Update(model);
		}

		public mesUnitState Get(int id)
		{
			return new mesUnitStateDAL().Get(id);
		}

		public IEnumerable<mesUnitState> ListAll(string S_Where)
		{
			return new mesUnitStateDAL().ListAll(S_Where);
		}
	}
}
