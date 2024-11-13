using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   mesStationBLL
	{
		public int Insert(mesStation model)
		{
			return new mesStationDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new mesStationDAL().Delete(id);
		}

		public bool Update(mesStation model)
		{
			return new mesStationDAL().Update(model);
		}

		public mesStation Get(int id)
		{
			return new mesStationDAL().Get(id);
		}

		public IEnumerable<mesStation> ListAll(string S_Where)
		{
			return new mesStationDAL().ListAll(S_Where);
		}
	}
}
