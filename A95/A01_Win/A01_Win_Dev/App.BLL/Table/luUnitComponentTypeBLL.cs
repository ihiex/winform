using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   luUnitComponentTypeBLL
	{
		public int Insert(luUnitComponentType model)
		{
			return new luUnitComponentTypeDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new luUnitComponentTypeDAL().Delete(id);
		}

		public bool Update(luUnitComponentType model)
		{
			return new luUnitComponentTypeDAL().Update(model);
		}

		public luUnitComponentType Get(int id)
		{
			return new luUnitComponentTypeDAL().Get(id);
		}

		public IEnumerable<luUnitComponentType> ListAll(string S_Where)
		{
			return new luUnitComponentTypeDAL().ListAll(S_Where);
		}
	}
}
