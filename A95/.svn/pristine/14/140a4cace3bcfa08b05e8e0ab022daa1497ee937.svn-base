using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;
using System.Data;

namespace App.BLL
{
	public class   mesProductStructureBLL
	{
		public int Insert(mesProductStructure model)
		{
			return new mesProductStructureDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new mesProductStructureDAL().Delete(id);
		}

		public bool Update(mesProductStructure model)
		{
			return new mesProductStructureDAL().Update(model);
		}

		public mesProductStructure Get(int id)
		{
			return new mesProductStructureDAL().Get(id);
		}

		public IEnumerable<mesProductStructure> ListAll(string S_Where)
		{
			return new mesProductStructureDAL().ListAll(S_Where);
		}

        public DataSet GetBOMStructure(string ParentPartID, string PartID, string StationTypeID)
        {
            return new mesProductStructureDAL().GetBOMStructure(ParentPartID, PartID, StationTypeID);
        }
    }
}
