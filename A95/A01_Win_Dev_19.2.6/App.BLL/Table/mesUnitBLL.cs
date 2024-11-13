using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;

namespace App.BLL
{
	public class   mesUnitBLL
	{
		public string Insert(mesUnit model)
		{
			return new mesUnitDAL().Insert(model);
		}

		public bool Delete(int id)
		{
			return new mesUnitDAL().Delete(id);
		}

		public string Update(mesUnit model)
		{
			return new mesUnitDAL().Update(model);
		}

        public string UpdatePart(mesUnit model)
        {
            return new mesUnitDAL().UpdatePart(model);
        }


        public mesUnit Get(int id)
		{
			return new mesUnitDAL().Get(id);
		}

        public mesUnit GetUnit(string SN)
        {
            return new mesUnitDAL().GetUnit(SN);
        }


        public IEnumerable<mesUnit> ListAll(string S_Where)
		{
			return new mesUnitDAL().ListAll(S_Where);
		}

        public string UpdatePlasma(string SN, int StationID, string LastUpdate, int LineID)
        {
            return new mesUnitDAL().UpdatePlasma(SN, StationID, LastUpdate, LineID);
        }

        public string UpdateUnitStateID(mesUnit v_mesUnit)
        {
            return new mesUnitDAL().UpdateUnitStateID(v_mesUnit);
        }

        public string UpdateTTUnitStateID(mesUnit v_mesUnit, string PanelID)
        {
            return new mesUnitDAL().UpdateTTUnitStateID(v_mesUnit, PanelID);
        }
    }
}
