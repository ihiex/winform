using App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IluDefect”。
[ServiceContract]
public interface IDataCommitSVC
{
    [OperationContract]
    string SubmitDataUH(List<mesUnit> List_mesUnit, List<mesHistory> List_mesHistory);
    [OperationContract]
    string SubmitDataUHD(List<mesUnit> List_mesUnit, List<mesHistory> List_mesHistory, List<mesUnitDefect> List_mesUnitDefect);
    [OperationContract]
    string SubmitDataUHCD(List<mesUnit> List_mesUnit, List<mesHistory> List_mesHistory,
               List<mesUnitComponent> List_mesUnitComponent, List<mesUnitDefect> List_mesUnitDefect);

    [OperationContract]
    string SubmitDataUHC(List<mesUnit> List_mesUnit, List<mesHistory> List_mesHistory,
            List<mesUnitComponent> List_mesUnitComponent, List<mesMaterialConsumeInfo> List_mesMaterialConsumeInfo,
            List<mesMachine> List_mesMachine, LoginList F_LoginList
            );

    [OperationContract]
    string InsertUDS(mesUnit v_mesUnit, mesUnitDetail v_mesUnitDetail, mesSerialNumber v_mesSerialNumber,
            string S_ObjSN);
    [OperationContract]
    string UpdatemesUnit(mesUnit v_mesUnit);

    [OperationContract]
    string InsertALL(List<mesUnit> List_mesUnit,
                                List<mesUnitDetail> List_mesUnitDetail,
                                List<mesHistory> List_mesHistory,
                                List<mesSerialNumber> List_mesSerialNumber,
                                List<mesUnitComponent> List_mesUnitComponent,
                                List<mesUnitDefect> List_mesUnitDefect,
                                List<mesMachine> List_mesMachine,
                                LoginList F_LoginList,
                                string[] L_TLinkT
                                );
    [OperationContract]
    string SubmitDataUH_UDetail(List<mesUnit> List_mesUnit, List<mesHistory> List_mesHistory, List<mesUnitDetail> List_mesUnitDetail);




    [OperationContract]
    string uspPalletPackaging(string PartID, string ProductionOrderID, string S_CartonSN,
            string S_PalletSN, LoginList loginList, string S_BillNO, int PalletQty = 0);

    [OperationContract]
    string MoveShipment(string S_BillNo, string S_MultipackSN, string S_ProdOrderID, string S_PartID, string S_StationId, string S_EmployeeId);

    [OperationContract]
    DataSet SetShipmentMultipack(string S_BillNo, string S_MultipackSN, string S_MultipackPalletSN);
    [OperationContract]
    string SetMesPackageShipment(string ShipmentDetailID, string SerialNumber, int Type);

    [OperationContract]
    string SetMesPackageShipmentRoll(string S_BillNo, string S_MultipackPalletSN, string S_MultipackSN, string S_ShipmentDetailID);

    [OperationContract]
    string GetShipmentPalletSN(string S_BillNo);

    [OperationContract]
    string GetIsOutCountComplete(string S_BillNo, string S_MultipackPalletSN, string S_ShipmentDetailID, Boolean B_ScanOver);


    //PrintOne//PrintOne//PrintOne//PrintOne//PrintOne/PrintOne

    [OperationContract]
    string uspPalletPackagingPrintOne(string PartID, string ProductionOrderID, string S_CartonSN,
            string S_PalletSN, LoginList loginList, int PalletQty = 0);

    [OperationContract]
    string MoveShipmentPrintOne(string S_BillNo, string S_MultipackSN, string S_ProdOrderID, string S_PartID, string S_StationId, string S_EmployeeId);

    [OperationContract]
    DataSet SetShipmentMultipackPrintOne(string S_BillNo, string S_MultipackSN);
    [OperationContract]
    string SetMesPackageShipmentPrintOne(string ShipmentDetailID, string SerialNumber, int Type);

    [OperationContract]
    string SetMesPackageShipmentRollPrintOne(string S_BillNo, string S_MultipackSN);

    [OperationContract]
    string SetCancelInWH(string S_BoxSN, string S_ProdID, string S_PartID, string S_StationID, string S_EmployeeID);
    [OperationContract]
    string SetCancelInWHEntry(string S_BoxSN, string S_ProdID, string S_PartID, string S_StationID, string S_EmployeeID, string S_ReturnToStationTypeID, string S_ReturnStatus);

}


