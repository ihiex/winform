using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;
using System.Data;

namespace App.BLL
{
	public class   mesRouteDetailBLL
	{
		public int Insert(mesRouteDetail model)
		{
			return new mesRouteDetailDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new mesRouteDetailDAL().Delete(id);
		}

		public bool Update(mesRouteDetail model)
		{
			return new mesRouteDetailDAL().Update(model);
		}

		public mesRouteDetail Get(int id)
		{
			return new mesRouteDetailDAL().Get(id);
		}

		public IEnumerable<mesRouteDetail> ListAll(string S_Where)
		{
			return new mesRouteDetailDAL().ListAll(S_Where);
		}

        public DataSet GetRouteDetail(string LineID, string PartID, string PartFamilyID, string ProductionOrderID)
        {
            return new mesRouteDetailDAL().GetRouteDetail(LineID, PartID, PartFamilyID, ProductionOrderID);
        }

        public DataSet GetRouteDetail2(string LineID, string PartID, string PartFamilyID, string ProductionOrderID)
        {
            return new mesRouteDetailDAL().GetRouteDetail2(LineID, PartID, PartFamilyID, ProductionOrderID);
        }

    }
}
