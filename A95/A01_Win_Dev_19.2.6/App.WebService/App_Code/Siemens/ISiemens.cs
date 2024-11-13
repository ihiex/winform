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
public interface ISiemensSVC
{
    [OperationContract]
    string WHIn(string S_MPN,string S_BoxSN, string S_Type,string S_StationTypeID);

    [OperationContract]
    DataSet WHIn_DT(string S_StationTypeID, string S_BoxSN);
    [OperationContract]
    string WHOut(string S_MPN, string S_BillNo, string S_BoxSN, string S_Type, string S_StationTypeID);
    [OperationContract]
    DataSet WHOut_DT(string S_StationTypeID, string S_BoxSN);


    [OperationContract]
    DataSet GetIpad_BB();
    [OperationContract]
    DataSet CheckBillNo(string S_BillNo, string S_StationTypeID, out string S_Result);

    [OperationContract]
    DataSet GetShipment(string S_Start, string S_End, string FStatus, string S_StationTypeID);

    [OperationContract]
    DataSet GetShipmentEntry(string S_FInterID, string S_StationTypeID);

    [OperationContract]
    DataSet GetShipmentReport(string S_Start, string S_End, string FStatus, string S_StationTypeID);
    [OperationContract]
    string Edit_CO_WH_Shipment
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
                );

    [OperationContract]
    string DeleteShipment(string FInterID, string S_StationTypeID);
    [OperationContract]
    string DeleteShipmentEntry(string FDetailID, string S_StationTypeID);
    [OperationContract]
    string DeleteMultiSelectShipment(string FInterID_List, string S_StationTypeID);


    [OperationContract]
    string UpdateShipment_FStatus(string FInterID_List, string Status, string S_StationTypeID);
    [OperationContract]
    DataSet GetShipment_One(string FInterID, string S_StationTypeID);
    [OperationContract]
    DataSet GetShipmentEntry_One(string FDetailID, string S_StationTypeID);
    [OperationContract]
    string Edit_CO_WH_ShipmentEntry
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
                );

    [OperationContract]
    string ExecSql(string S_Sql, string S_StationTypeID);
    [OperationContract]
    DataSet Data_Set(string S_Sql, string S_StationTypeID);

}
