using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;
using System.Data;

namespace App.BLL
{
	public class   mesMachineBLL
	{
		public int Insert(mesMachine model)
		{
			return new mesMachineDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new mesMachineDAL().Delete(id);
		}

		public bool Update(mesMachine model)
		{
			return new mesMachineDAL().Update(model);
		}

		public mesMachine Get(int id)
		{
			return new mesMachineDAL().Get(id);
		}

		public IEnumerable<mesMachine> ListAll(string S_Where)
		{
			return new mesMachineDAL().ListAll(S_Where);
		}

        public DataSet MesGetLineIDByMachineSN(string MachineSN)
        {
            return new mesMachineDAL().MesGetLineIDByMachineSN(MachineSN);
        }

        public DataSet MesGetStatusIDByList(int StationTypeID, int PartID, string MachineSN)
        {
            return new mesMachineDAL().MesGetStatusIDByList(StationTypeID, PartID, MachineSN);
        }

        public void MesModMachineBySN(string MachineSN)
        {
            new mesMachineDAL().MesModMachineBySN(MachineSN);
        }
    }
}
