using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   sysStatusBLL
	{
		public int Insert(sysStatus model)
		{
			return new sysStatusDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new sysStatusDAL().Delete(id);
		}

		public bool Update(sysStatus model)
		{
			return new sysStatusDAL().Update(model);
		}

		public sysStatus Get(int id)
		{
			return new sysStatusDAL().Get(id);
		}

		public IEnumerable<sysStatus> ListAll(string S_Where)
		{
			return new sysStatusDAL().ListAll(S_Where);
		}
	}
}
