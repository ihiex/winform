using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   luPartFamilyTypeBLL
	{
		public int Insert(luPartFamilyType model)
		{
			return new luPartFamilyTypeDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new luPartFamilyTypeDAL().Delete(id);
		}

		public bool Update(luPartFamilyType model)
		{
			return new luPartFamilyTypeDAL().Update(model);
		}

		public luPartFamilyType Get(int id)
		{
			return new luPartFamilyTypeDAL().Get(id);
		}

		public IEnumerable<luPartFamilyType> ListAll(string S_Where)
		{
			return new luPartFamilyTypeDAL().ListAll(S_Where);
		}
	}
}
