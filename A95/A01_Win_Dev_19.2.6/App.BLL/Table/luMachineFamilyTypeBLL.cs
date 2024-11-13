using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   luMachineFamilyTypeBLL
	{
		public int Insert(luMachineFamilyType model)
		{
			return new luMachineFamilyTypeDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new luMachineFamilyTypeDAL().Delete(id);
		}

		public bool Update(luMachineFamilyType model)
		{
			return new luMachineFamilyTypeDAL().Update(model);
		}

		public luMachineFamilyType Get(int id)
		{
			return new luMachineFamilyTypeDAL().Get(id);
		}

		public IEnumerable<luMachineFamilyType> ListAll(string S_Where)
		{
			return new luMachineFamilyTypeDAL().ListAll(S_Where);
		}
	}
}
