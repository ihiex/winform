using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   luMachineFamilyBLL
	{
		public int Insert(luMachineFamily model)
		{
			return new luMachineFamilyDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new luMachineFamilyDAL().Delete(id);
		}

		public bool Update(luMachineFamily model)
		{
			return new luMachineFamilyDAL().Update(model);
		}

		public luMachineFamily Get(int id)
		{
			return new luMachineFamilyDAL().Get(id);
		}

		public IEnumerable<luMachineFamily> ListAll(string S_Where)
		{
			return new luMachineFamilyDAL().ListAll(S_Where);
		}
	}
}
