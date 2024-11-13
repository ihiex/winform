using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;
using System.Data;

namespace App.BLL
{
	public class   mesRouteMapBLL
	{
		public int Insert(mesRouteMap model)
		{
			return new mesRouteMapDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new mesRouteMapDAL().Delete(id);
		}

		public bool Update(mesRouteMap model)
		{
			return new mesRouteMapDAL().Update(model);
		}

        public DataSet MesGetPartIDByMachineSN(int stationTypeID, string MachineSN)
        {
            return new mesRouteMapDAL().MesGetPartIDByMachineSN(stationTypeID, MachineSN);
        }

        public mesRouteMap Get(int id)
		{
			return new mesRouteMapDAL().Get(id);
		}

		public IEnumerable<mesRouteMap> ListAll(string S_Where)
		{
			return new mesRouteMapDAL().ListAll(S_Where);
		}
	}
}
