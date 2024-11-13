using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   mesPartBLL
	{
		public int Insert(mesPart model)
		{
			return new mesPartDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new mesPartDAL().Delete(id);
		}

		public bool Update(mesPart model)
		{
			return new mesPartDAL().Update(model);
		}

		public mesPart Get(int id)
		{
			return new mesPartDAL().Get(id);
		}

		public IEnumerable<mesPart> ListAll(string S_Where)
		{
			return new mesPartDAL().ListAll(S_Where);
		}
	}
}
