using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   luSerialNumberTypeBLL
	{
		public int Insert(luSerialNumberType model)
		{
			return new luSerialNumberTypeDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new luSerialNumberTypeDAL().Delete(id);
		}

		public bool Update(luSerialNumberType model)
		{
			return new luSerialNumberTypeDAL().Update(model);
		}

		public luSerialNumberType Get(int id)
		{
			return new luSerialNumberTypeDAL().Get(id);
		}

		public IEnumerable<luSerialNumberType> ListAll(string S_Where)
		{
			return new luSerialNumberTypeDAL().ListAll(S_Where);
		}
	}
}
