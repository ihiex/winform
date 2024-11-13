using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   luApplicationTypeBLL
	{
		public int Insert(luApplicationType model)
		{
			return new luApplicationTypeDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new luApplicationTypeDAL().Delete(id);
		}

		public bool Update(luApplicationType model)
		{
			return new luApplicationTypeDAL().Update(model);
		}

		public luApplicationType Get(int id)
		{
			return new luApplicationTypeDAL().Get(id);
		}

		public IEnumerable<luApplicationType> ListAll(string S_Where)
		{
			return new luApplicationTypeDAL().ListAll(S_Where);
		}
	}
}
