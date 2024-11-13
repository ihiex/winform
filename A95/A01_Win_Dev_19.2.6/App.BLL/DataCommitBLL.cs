using App.DBServerDAL;
using App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL
{
    public class DataCommitBLL
    {
        public string SubmitDataUH(List<mesUnit> List_mesUnit, List<mesHistory> List_mesHistory)
        {
            return new DataCommitDAL().SubmitDataUH(List_mesUnit, List_mesHistory);
        }

        public string SubmitDataUHD(List<mesUnit> List_mesUnit, List<mesHistory> List_mesHistory, List<mesUnitDefect> List_mesUnitDefect)
        {
            return new DataCommitDAL().SubmitDataUHD(List_mesUnit, List_mesHistory, List_mesUnitDefect);
        }

        public string SubmitDataUHCD(List<mesUnit> List_mesUnit, List<mesHistory> List_mesHistory,
            List<mesUnitComponent> List_mesUnitComponent, List<mesUnitDefect> List_mesUnitDefect)
        {
            return new DataCommitDAL().SubmitDataUHCD(List_mesUnit, List_mesHistory, List_mesUnitComponent, List_mesUnitDefect);
        }

        public string SubmitDataUHC(List<mesUnit> List_mesUnit, List<mesHistory> List_mesHistory,
            List<mesUnitComponent> List_mesUnitComponent, List<mesMaterialConsumeInfo> List_mesMaterialConsumeInfo,
            List<mesMachine> List_mesMachine, LoginList F_LoginList
            )
        {
            return new DataCommitDAL().SubmitDataUHC(List_mesUnit, List_mesHistory,
                List_mesUnitComponent, List_mesMaterialConsumeInfo, List_mesMachine, F_LoginList);
        }

        public string InsertUDS(mesUnit v_mesUnit, mesUnitDetail v_mesUnitDetail, mesSerialNumber v_mesSerialNumber,
            string S_ObjSN)
        {
            return new DataCommitDAL().InsertUDS(v_mesUnit, v_mesUnitDetail, v_mesSerialNumber, S_ObjSN);
        }

        public string UpdatemesUnit(mesUnit v_mesUnit)
        {
            return new DataCommitDAL().UpdatemesUnit(v_mesUnit);
        }

        public string InsertALL(List<mesUnit> List_mesUnit,
                                List<mesUnitDetail> List_mesUnitDetail,
                                List<mesHistory> List_mesHistory,
                                List<mesSerialNumber> List_mesSerialNumber,
                                List<mesUnitComponent> List_mesUnitComponent,
                                List<mesUnitDefect> List_mesUnitDefect,
                                List<mesMachine> List_mesMachine,
                                LoginList F_LoginList,
                                string[] L_TLinkT
                                )
        {
            return new DataCommitDAL().InsertALL(List_mesUnit,
                                                 List_mesUnitDetail,
                                                 List_mesHistory,
                                                 List_mesSerialNumber,
                                                 List_mesUnitComponent,
                                                 List_mesUnitDefect,
                                                 List_mesMachine,
                                                 F_LoginList,
                                                 L_TLinkT
                                                 );
        }

        public string SubmitDataUH_UDetail(List<mesUnit> List_mesUnit, List<mesHistory> List_mesHistory, List<mesUnitDetail> List_mesUnitDetail)
        {
            return new DataCommitDAL().SubmitDataUH_UDetail(List_mesUnit, List_mesHistory, List_mesUnitDetail);
        }






        public string uspPalletPackaging(string PartID, string ProductionOrderID, string S_CartonSN,
            string S_PalletSN, LoginList loginList, string S_BillNO, int PalletQty = 0)
        {
            return new DataCommitDAL().uspPalletPackaging(PartID, ProductionOrderID, S_CartonSN, S_PalletSN, loginList, S_BillNO, PalletQty);
        }

        public string MoveShipment(string S_BillNo, string S_MultipackSN, string S_ProdOrderID, string S_PartID, string S_StationId, string S_EmployeeId)
        {
            return new DataCommitDAL().MoveShipment(S_BillNo, S_MultipackSN, S_ProdOrderID, S_PartID, S_StationId, S_EmployeeId);
        }

        public DataSet SetShipmentMultipack(string S_BillNo, string S_MultipackSN, string S_MultipackPalletSN)
        {
            return new DataCommitDAL().SetShipmentMultipack(S_BillNo, S_MultipackSN, S_MultipackPalletSN);
        }

        public string SetMesPackageShipment(string ShipmentDetailID, string SerialNumber, int Type)
        {
            return new DataCommitDAL().SetMesPackageShipment(ShipmentDetailID, SerialNumber, Type);
        }

        public string SetMesPackageShipmentRoll(string S_BillNo, string S_MultipackPalletSN, string S_MultipackSN, string S_ShipmentDetailID)
        {
            return new DataCommitDAL().SetMesPackageShipmentRoll(S_BillNo, S_MultipackPalletSN, S_MultipackSN, S_ShipmentDetailID);
        }

        public string GetShipmentPalletSN(string S_BillNo)
        {
            return new DataCommitDAL().GetShipmentPalletSN(S_BillNo);
        }

        public string GetIsOutCountComplete(string S_BillNo, string S_MultipackPalletSN, string S_ShipmentDetailID, Boolean B_ScanOver)
        {
            return new DataCommitDAL().GetIsOutCountComplete(S_BillNo, S_MultipackPalletSN, S_ShipmentDetailID,  B_ScanOver);
        }

        //PrintOne/////////////PrintOne//////PrintOne//////////PrintOne/////////PrintOne/////////PrintOne//


        public string uspPalletPackagingPrintOne(string PartID, string ProductionOrderID, string S_CartonSN,
            string S_PalletSN, LoginList loginList, int PalletQty = 0)
        {
            return new DataCommitDAL().uspPalletPackagingPrintOne(PartID, ProductionOrderID, S_CartonSN, S_PalletSN, loginList, PalletQty);
        }

        public string MoveShipmentPrintOne(string S_BillNo, string S_MultipackSN, string S_ProdOrderID, string S_PartID, string S_StationId, string S_EmployeeId)
        {
            return new DataCommitDAL().MoveShipmentPrintOne(S_BillNo, S_MultipackSN, S_ProdOrderID, S_PartID, S_StationId, S_EmployeeId);
        }

        public DataSet SetShipmentMultipackPrintOne(string S_BillNo, string S_MultipackSN)
        {
            return new DataCommitDAL().SetShipmentMultipackPrintOne(S_BillNo, S_MultipackSN);
        }

        public string SetMesPackageShipmentPrintOne(string ShipmentDetailID, string SerialNumber, int Type)
        {
            return new DataCommitDAL().SetMesPackageShipmentPrintOne(ShipmentDetailID, SerialNumber, Type);
        }

        public string SetMesPackageShipmentRollPrintOne(string S_BillNo, string S_MultipackSN)
        {
            return new DataCommitDAL().SetMesPackageShipmentRollPrintOne(S_BillNo, S_MultipackSN);
        }


        public string SetCancelInWH(string S_BoxSN, string S_ProdID, string S_PartID, string S_StationID, string S_EmployeeID)
        {
            return new DataCommitDAL().SetCancelInWH(S_BoxSN, S_ProdID, S_PartID, S_StationID, S_EmployeeID);
        }
        public string SetCancelInWHEntry(string S_BoxSN, string S_ProdID, string S_PartID, string S_StationID, string S_EmployeeID, string S_ReturnToStationTypeID, string S_ReturnStatus)
        {
            return new DataCommitDAL().SetCancelInWHEntry(S_BoxSN, S_ProdID, S_PartID, S_StationID, S_EmployeeID,  S_ReturnToStationTypeID, S_ReturnStatus);
        }
    }
}
