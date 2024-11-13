using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   mesStationTypeBLL
	{
		public int Insert(mesStationType model)
		{
			return new mesStationTypeDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new mesStationTypeDAL().Delete(id);
		}

		public bool Update(mesStationType model)
		{
			return new mesStationTypeDAL().Update(model);
		}

		public mesStationType Get(int id)
		{
			return new mesStationTypeDAL().Get(id);
		}

		public IEnumerable<mesStationType> ListAll(string S_Where)
		{
			return new mesStationTypeDAL().ListAll(S_Where);
		}
	}
}
