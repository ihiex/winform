using System.Collections.Generic;
using System.Text;
using App.Model;
using App.DBServerDAL;
using System.Data;

namespace App.BLL
{
    public class MesMaterialUnitBLL
    {
        public int Insert(MesMaterialUnit model)
        {
            return new MesMaterialUnitDAL().Insert(model);
        }

        public int InserDetail(MesMaterialUnitDetail model)
        {
            return new MesMaterialUnitDAL().InserDetail(model);
        }

        public int InserForParent(MesMaterialUnit model)
        {
            return new MesMaterialUnitDAL().InserForParent(model);
        }

        public bool Delete(int id)
        {
            return new MesMaterialUnitDAL().Delete(id);
        }

        public bool Update(MesMaterialUnit model)
        {
            return new MesMaterialUnitDAL().Update(model);
        }

        public MesMaterialUnit Get(int id)
        {
            return new MesMaterialUnitDAL().Get(id);
        }

        public IEnumerable<MesMaterialUnit> ListAll(string S_Where)
        {
            return new MesMaterialUnitDAL().ListAll(S_Where);
        }

        public DataSet GetMesMaterialUnitByLotCode(string PartID, string LotCode)
        {
            return new MesMaterialUnitDAL().GetMesMaterialUnitByLotCode(PartID,LotCode);
        }

        public int GetMesMaterialUseQTY(string MaterialUnitID)
        {
            return new MesMaterialUnitDAL().GetMesMaterialUseQTY(MaterialUnitID);
        }

        public DataSet GetMesMaterialUnitData(string SerialNumber)
        {
            return new MesMaterialUnitDAL().GetMesMaterialUnitData(SerialNumber);
        }
    }
}
