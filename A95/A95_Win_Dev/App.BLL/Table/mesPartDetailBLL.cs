using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;
using System.Data;

namespace App.BLL
{
	public class   mesPartDetailBLL
	{
		public int Insert(mesPartDetail model)
		{
			return new mesPartDetailDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new mesPartDetailDAL().Delete(id);
		}

		public bool Update(mesPartDetail model)
		{
			return new mesPartDetailDAL().Update(model);
		}

		public mesPartDetail Get(int id)
		{
			return new mesPartDetailDAL().Get(id);
		}

		public IEnumerable<mesPartDetail> ListAll(string S_Where)
		{
			return new mesPartDetailDAL().ListAll(S_Where);
		}

    }
}
