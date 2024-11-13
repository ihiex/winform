using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.DBServerDAL;

namespace App.BLL
{
    public class SiemensBLL
    {
        public string WHIn(string S_MPN,string S_BoxSN, string S_Type, string S_StationTypeID)
        {
            return new SiemensDAL().WHIn(S_MPN ,S_BoxSN, S_Type, S_StationTypeID);
        }

        public DataSet WHIn_DT(string S_StationTypeID, string S_BoxSN)
        {
            return new SiemensDAL().WHIn_DT(S_StationTypeID, S_BoxSN);
        }

        public string WHOut(string S_MPN, string S_BillNo, string S_BoxSN, string S_Type, string S_StationTypeID)
        {
            return new SiemensDAL().WHOut(S_MPN, S_BillNo, S_BoxSN, S_Type, S_StationTypeID);
        }

        public DataSet WHOut_DT(string S_StationTypeID, string S_BoxSN)
        {
            return new SiemensDAL().WHOut_DT(S_StationTypeID, S_BoxSN);
        }

        public DataSet GetIpad_BB()
        {
            return new SiemensDAL().GetIpad_BB();
        }

        public DataSet CheckBillNo(string S_BillNo, string S_StationTypeID, out string S_Result)
        {
            S_Result = "1";
            return new SiemensDAL().CheckBillNo(S_BillNo, S_StationTypeID,out S_Result);
        }

        public DataSet GetShipment(string S_Start, string S_End, string FStatus, string S_StationTypeID)
        {
            return new SiemensDAL().GetShipment(S_Start, S_End, FStatus, S_StationTypeID);
        }

        public DataSet GetShipmentEntry(string S_FInterID, string S_StationTypeID)
        {
            return new SiemensDAL().GetShipmentEntry( S_FInterID,  S_StationTypeID);
        }

        public DataSet GetShipmentReport(string S_Start, string S_End, string FStatus, string S_StationTypeID)
        {
            return new SiemensDAL().GetShipmentReport( S_Start,  S_End,  FStatus,  S_StationTypeID);
        }

        public string Edit_CO_WH_Shipment
            (
                string S_FInterID,
                string S_ShipDate,
                string S_HAWB,
                string S_PalletSeq,
                string S_PalletCount,
                string S_GrossWeight,
                string S_EmptyCarton,
                string S_ShipNO,
                string S_Project,

                string S_Type,
                string S_StationTypeID
            )
        {
            return new SiemensDAL().Edit_CO_WH_Shipment
                (
                 S_FInterID,
                 S_ShipDate,
                 S_HAWB,
                 S_PalletSeq,
                 S_PalletCount,
                 S_GrossWeight,
                 S_EmptyCarton,
                 S_ShipNO,
                 S_Project,

                 S_Type,
                 S_StationTypeID
                );
        }

        public string DeleteShipment(string FInterID, string S_StationTypeID)
        {
            return new SiemensDAL().DeleteShipment( FInterID,  S_StationTypeID);
        }

        public string DeleteShipmentEntry(string FDetailID, string S_StationTypeID)
        {
            return new SiemensDAL().DeleteShipmentEntry( FDetailID,  S_StationTypeID);
        }

        public string DeleteMultiSelectShipment(string FInterID_List, string S_StationTypeID)
        {
            return new SiemensDAL().DeleteMultiSelectShipment(FInterID_List, S_StationTypeID);
        }

        public string UpdateShipment_FStatus(string FInterID_List, string Status, string S_StationTypeID)
        {
            return new SiemensDAL().UpdateShipment_FStatus( FInterID_List,  Status,  S_StationTypeID);
        }


        public DataSet GetShipment_One(string FInterID, string S_StationTypeID)
        {
            return new SiemensDAL().GetShipment_One( FInterID,  S_StationTypeID);
        }

        public DataSet GetShipmentEntry_One(string FDetailID, string S_StationTypeID)
        {
            return new SiemensDAL().GetShipmentEntry_One( FDetailID,  S_StationTypeID);
        }

        public string Edit_CO_WH_ShipmentEntry
            (
                string S_FInterID,
                string S_FEntryID,
                string S_FDetailID,
                string S_FKPONO,
                string S_FMPNNO,
                string S_FCTN,
                string S_FStatus,

                string S_StationTypeID,
                string S_Type
            )
        {
            return new SiemensDAL().Edit_CO_WH_ShipmentEntry
                (
                 S_FInterID,
                 S_FEntryID,
                 S_FDetailID,
                 S_FKPONO,
                 S_FMPNNO,
                 S_FCTN,
                 S_FStatus,

                 S_StationTypeID,
                 S_Type
                );
        }


        public string ExecSql(string S_Sql, string S_StationTypeID)
        {
            return new SiemensDAL().ExecSql(S_Sql, S_StationTypeID);
        }

        public DataSet Data_Set(string S_Sql, string S_StationTypeID)
        {
            return new SiemensDAL().Data_Set(S_Sql, S_StationTypeID);
        }
    }
}
