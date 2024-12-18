﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Model;
using App.DBServerDAL;
using System.Data;

namespace App.BLL
{
    public class mesUnitDetailBLL
    {
        public string Insert(mesUnitDetail model)
        {
            return new mesUnitDetailDAL().Insert(model);
        }

        public bool Delete(int id)
        {
            return new mesUnitDetailDAL().Delete(id);
        }

        public string Update(mesUnitDetail model)
        {
            return new mesUnitDetailDAL().Update(model);
        }

        public mesUnitDetail Get(int id)
        {
            return new mesUnitDetailDAL().Get(id);
        }

        public mesUnitDetail GetUnitDetail(int unitid)
        {
            return new mesUnitDetailDAL().GetUnitDetail(unitid);
        }

        public IEnumerable<mesUnitDetail> ListAll(string S_Where)
        {
            return new mesUnitDetailDAL().ListAll(S_Where);
        }

        public DataSet MesGetBatchIDByBarcodeSN(string BarcodeSN)
        {
            return new mesUnitDetailDAL().MesGetBatchIDByBarcodeSN(BarcodeSN);
        }

        public string MesGetFGSNByUPCSN(string UPCSN)
        {
            return new mesUnitDetailDAL().MesGetFGSNByUPCSN(UPCSN);
        }

        public void UpdateKitSerialnumber(int UnitID, string KitSerialnumber)
        {
            new mesUnitDetailDAL().UpdateKitSerialnumber(UnitID, KitSerialnumber);
        }
    }
}
