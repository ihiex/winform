using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   luPartFamilyBLL
	{
		public int Insert(luPartFamily model)
		{
			return new luPartFamilyDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new luPartFamilyDAL().Delete(id);
		}

		public bool Update(luPartFamily model)
		{
			return new luPartFamilyDAL().Update(model);
		}

		public luPartFamily Get(int id)
		{
			return new luPartFamilyDAL().Get(id);
		}

		public IEnumerable<luPartFamily> ListAll(string S_Where)
		{
			return new luPartFamilyDAL().ListAll(S_Where);
		}
	}
}
