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

        public void MesModMachineBySNStationTypeID(string MachineSN, int StationTypeID)
        {
            new mesMachineDAL().MesModMachineBySNStationTypeID(MachineSN, StationTypeID);
        }

        public string MesModMachineBySNStationTypeID_Sql(string MachineSN, int StationTypeID)
        {
            string S_Result= new mesMachineDAL().MesModMachineBySNStationTypeID_Sql(MachineSN, StationTypeID);
            return S_Result;
        }

        public void MesModMachineBySN(string MachineSN)
        {
            new mesMachineDAL().MesModMachineBySN(MachineSN);
        }

        public void SetMachineRuningQuantity(string MachineSN)
        {
            new mesMachineDAL().SetMachineRuningQuantity(MachineSN);
        }

        public string MesToolingReleaseCheck(string MachineSN, string StationTypeID)
        {
            return new mesMachineDAL().MesToolingReleaseCheck(MachineSN, StationTypeID);
        }
    }
}
