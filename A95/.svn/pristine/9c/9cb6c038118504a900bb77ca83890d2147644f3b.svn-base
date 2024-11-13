using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using App.BLL;
using App.Model;
using System.Data;



    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IPartSelectSVC”。
    [ServiceContract]
    public interface IPartSelectSVC
    {
        [OperationContract]
        double MyTest();

        [OperationContract]
        DataSet P_DataSet(string S_Sql);
        [OperationContract]
        string ExecSql(string S_Sql);

        [OperationContract]
        DataSet GetluPartFamilyType();

        [OperationContract]
        string GetServerIP();

        [OperationContract]
        string GetConn();
        [OperationContract]
        string GetDBName();

        [OperationContract]
        DataSet GetluPartFamily(string PartFamilyTypeID);

        [OperationContract]
        DataSet GetmesPart(string PartFamilyID);

        [OperationContract]
        DataSet GetmesPartDetail(int PartID, string PartDetailDefName);

        [OperationContract]
        DataSet GetluPODetailDef(int ProductionOrderID, string PODetailDef);


        [OperationContract]
        DataSet GetmesPartPrint();

        [OperationContract]
        string mesGetSNFormatIDByList(string PartID, string PartFamilyID, string LineID, string ProductionOrderID, string StationTypeID);

        [OperationContract]
        DataSet GetmesLine();

        [OperationContract]
        DataSet mesLineGroup(string LineType, int PartFamilyTypeID);

        [OperationContract]
        DataSet GetsysStatus();


        [OperationContract]
        DataSet GetmesStationType();
        [OperationContract]
        DataSet GetmesRoute();

        [OperationContract]
        DataSet GetRouteSequence(string RouteID, string StationTypeID);
        [OperationContract]
        DataSet GetmesStation(string LineID);
        [OperationContract]
        DataSet GetmesStation2(string StationTypeID, string LineID);

        [OperationContract]
        DataSet GetmesStationTypeByStationID(string StationID);

        [OperationContract]
        DataSet GetluSerialNumberType();

        [OperationContract]
        DataSet GetUnit(string PartID);
        [OperationContract]
        DataSet GetUnit2(string PartID, string StationID, string POID);
        [OperationContract]
        DataSet GetMachineHistory(string S_UnitID);



        [OperationContract]
        DataSet GetUnit_Search(string S_DateStart, string S_DateEnd, string S_Where);

        [OperationContract]
        DataSet GetUnitComponent(string S_UnitID);


        [OperationContract]
        DataSet GetmesUnitDetail(string S_UnitID);


        [OperationContract]
        DataSet GetHistory(string UnitID);

        [OperationContract]
        DataSet GetmesUnitState(string S_PartID, string PartFamilyID, string S_RouteSequence,string LineID,
            int StationTypeID,string ProductionOrderID, string StatusID);

        [OperationContract]
        DataSet GetmesUnitState_Diagram(string S_PartID, string PartFamilyID, string LineID, int StationTypeID,string ProductionOrderID);

        [OperationContract]
        DataSet GetmesSerialNumberOfLine(string SNCategory, string PrintCount);

        [OperationContract]
        DataSet GetmesSerialNumberByUnitID(string UnitID);

        [OperationContract]
        DataSet GetPO(string S_PartID, string S_StatusID);
        [OperationContract]
        DataSet GetPOAll(string S_PartID, string S_LineID);

        [OperationContract]
        int GetRouteID(string LineID, string PartID, string PartFamilyID,string ProductionOrderID);

        [OperationContract]
        DataSet GetRoute(string S_RouteSequence, int I_RouteID);

        [OperationContract]
        DataSet GetApplicationType(string StationTypeID);

        [OperationContract]
        DataSet GetluDefect(int DefectTypeID);

        [OperationContract]
        DataSet GetluUnitStatus();

        [OperationContract]
        DataSet GetmesUnitDefect(string S_UnitID);

        [OperationContract]
        DataSet uspSNRGetNext(string strSNFormat, int ReuseSNByStation, string xmlProdOrder,
                                            string xmlPart, string xmlStation, string xmlExtraData, string strNextSN);

        [OperationContract]
        DataSet GetPartDetailDef(string SN);
        [OperationContract]
        string Get_MachineRouteMap(string S_ToolSN, string S_ProductPartID, string S_RouteID, string S_StationTypeID);
        [OperationContract]
        string GetRouteCheck(int Scan_StationTypeID, int Scan_StationID,string LineID, DataTable DT_Unit, string S_Str);

        [OperationContract]
        DataSet Get_UnitID(string S_SN);

        [OperationContract]
        string Get_CreateMesSN(string strSNFormat, LoginList loginList, string xmlProdOrder, string xmlPart, string xmlStation,
                                string xmlExtraData, mesUnit v_mesUnit, int number, ref DataSet dsSN);
       [OperationContract]
       string Get_CreateMesSN_New(string strSNFormat, LoginList loginList, string xmlProdOrder, string xmlPart, string xmlStation,
                              string xmlExtraData, mesUnit v_mesUnit, int number, ref DataSet dsSN);

    [OperationContract]
        DataSet BoxSnToBatch(string S_BoxSN, out string S_Result);

        [OperationContract]
        int GetComponentCount(string ParentPartID, string StationTypeID);
        [OperationContract]
        DataSet GetmesUnitComponent(string UnitID, string ChildUnitID);
        [OperationContract]
        DataSet GetmesUnitComponent2(string UnitID);


        [OperationContract]
        DataSet GetmesProductStructure(string ParentPartID, string PartID, string StationTypeID);
        [OperationContract]
        DataSet GetmesProductStructure1(string ParentPartID);
        [OperationContract]
        DataSet GetmesProductStructure2(string ParentPartID, string StationTypeID);

        [OperationContract]
        DataSet GetChildScanLast(string S_SN);
        [OperationContract]
        void ModPO(string S_POID);
        [OperationContract]
        void ModMachine(string S_SN, string StatusID, Boolean IsUpUnitDetail);
        [OperationContract]
        void ModMachine2(string ID, string StatusID);
        [OperationContract]
        DataSet GetProductionOrder(string ID);

        [OperationContract]
        DataSet GetMesPackageStatusID(string PalletSN);
         
        [OperationContract]
        DataSet GetmesSerialNumber(string S_SN);
        [OperationContract]
        DataSet GetmesUnit(string UnitID);
        [OperationContract]
        DataSet GetComponent(int I_ChildUnitID);
        [OperationContract]
        DataSet GetmesMachine(string S_SN);
        [OperationContract]
        DataSet GetmesRouteMachineMap(string MachineID, string MachineFamilyID);

        [OperationContract]
        DataSet GetSNParameter(int PartID, int TemplateType);

        [OperationContract]
        DataSet GetLabels(string PartID, string PartFamilyID, string LineID, string ProductionOrderID, string StationTypeID);

        [OperationContract]
        DataSet GetLBLGenLabel(string S_SN, string S_LabelName);
        [OperationContract]
        DataSet GetLabelName(string StationTypeID, string PartFamilyID, string PartID, string ProductionOrderID, string LineID);
    [OperationContract]
    DataSet GetLabelCMD(string StationTypeID, string PartFamilyID, string PartID, string ProductionOrderID, string LineID);
    [OperationContract]
    string GetLabelID(string StationTypeID, string PartFamilyID, string PartID, string ProductionOrderID, string LineID);
    [OperationContract]
     DataSet LabelNameToLabelCMD(string S_LabelName);



    [OperationContract]
        string BuckToFGSN(string S_BuckSN);
        [OperationContract]
        DataSet GetPLCSeting(string S_SetName, string S_StationID);

        [OperationContract]
        string TimeCheck(string StationID, string S_SN);


        [OperationContract]
        DataSet GetPartParameter(string PartID);

        [OperationContract]
        string GetluDefectType(string Description);

        [OperationContract]
        string uspKitBoxCheck(string S_FormatName, string xmlProdOrder, string xmlPart, string xmlStation, string xmlExtraData, string strSNbuf);

        [OperationContract]
        string uspKitBoxPackaging(string PartID, string ProductionOrderID, string S_UPCSN, string S_CartonSN, LoginList LoginList, int BoxQty = 0);

    [OperationContract]
    string uspKitBoxPackagingNew(string PartID, string ProductionOrderID, string S_UPCSN, string S_CartonSN, LoginList LoginList,int Allnumber,int CurrentQty, int BoxQty = 0);

    [OperationContract]
        string uspPalletCheck(string S_FormatName, string xmlProdOrder, string xmlPart, string xmlStation, string xmlExtraData, string strSNbuf);

        [OperationContract]
        string uspPalletPackaging(string PartID, string ProductionOrderID, string S_CartonSN, string S_PalletSN, LoginList loginList, int PalletQty = 0);
    [OperationContract]
    string uspPalletPackaging_Siemens(string PartID, string ProductionOrderID, string S_CartonSN, string S_PalletSN, LoginList loginList, int PalletQty = 0);


    [OperationContract]
        DataSet GetProductionOrderDetailDef(string ProductionOrderNumber);

        [OperationContract]
        string Get_CreatePackageSN(string S_FormatName, string xmlProdOrder, string xmlPart, string xmlStation,
                                        string xmlExtraData, mesUnit v_mesUnit, ref string strSN, int type);
    [OperationContract]
    string Get_CreatePackageSN_Siemens(string S_FormatName, string xmlProdOrder, string xmlPart, string xmlStation,
                                    string xmlExtraData, string MultipackSN, ref string strSN, int type);

        [OperationContract]
        DataSet Get_PackageData(string S_SN);

        [OperationContract]
        DataSet Get_PalletData(string S_SN);

        [OperationContract]
        DataSet Get_SearchData(string S_FormatName, string xmlProdOrder, string xmlPart,
                                    string xmlStation, string xmlExtraData, string strSNbuf, ref string strOutput);

        [OperationContract]
        DataSet uspCallProcedure(string Pro_Name, string S_FormatName, string xmlProdOrder, string xmlPart,
                                     string xmlStation, string xmlExtraData, string strSNbuf, ref string strOutput);

        [OperationContract]
        DataSet Get_PartParameter(string PartID);

        [OperationContract]
        DataSet MESGetBomPartInfo(int ParentPartID, int StationTypeID);

        [OperationContract]
        string MESAssembleCheckMianSN(string ProductionOrderID, int LineID, int StationID, int StationTypeID, string SN, bool COF);

        [OperationContract]
        string MESAssembleCheckOtherSN(string SN, string PartID, bool COF);

        [OperationContract]
        string MESAssembleCheckVirtualSN(string SN, string PartID, string Status);

        [OperationContract]
        void MESModifyUnitDetail(int UnitID, string FileName, string Value);

        [OperationContract]
        string MESGetUnitUnitStateID(string SN);

        [OperationContract]
        DataSet MESGetStationTypeParameter(int stationTypeID);
        [OperationContract]
        DataSet GetSerialNumber2(string S_SN);
        [OperationContract]
        DataSet GetLanguage(string FormName, string Type);

        [OperationContract]
         string GetVer();
        [OperationContract]
        string GetMSG(string S_Lang, string S_Code);

        [OperationContract]
        DataSet GetVendor(string PartID);
        //[OperationContract]
        // DataSet GetRoute2(string LineID, string PartID, string PartFamilyID);
        //[OperationContract]
        // string GetRouteCheck2(LoginList List_Login, DataTable DT_Unit, string S_Str);
        [OperationContract]
        bool SetToolingLinkTooling(string FromTooling, string ToTooling, int FromUintID, LoginList loginList);
        [OperationContract]
        string Get_CreateMaterail(string strSNFormat, string xmlProdOrder, string xmlPart, string xmlStation,
                                          string xmlExtraData, LoginList loginList, mesUnit v_mesUnit, MesMaterialUnit v_mesMaterialUnit, int number, ref DataSet dsSN);
        [OperationContract]
        string ModMesMaterialConsumeInfo(LoginList loginList, int ScanType, string SN, string MachineSN, int PartID, int ProductionOrderID);
    [OperationContract]
    DataSet GetmesStationConfig(string Name, string StationID);

    [OperationContract]
    DataSet GetShipmentInterID(string ShipmentDetailID);

    [OperationContract]
    void SetMesPackageShipmennt(string ShipmentDetailID, string SerialNumber, int Type);

    [OperationContract]
    DataSet GetMesLabelData(string LabelName);

    [OperationContract]
    int InsertMesPackageHistory(mesPackageHistory model);
    [OperationContract]
    DataSet GetShipmentNew(string S_Start, string S_End, string FStatus);
    [OperationContract]
    DataSet GetShipmentEntryNew(string S_FInterID);
    [OperationContract]
    DataSet GetShipmentReport(string S_Start, string S_End, string FStatus);

    [OperationContract]
    string Edit_CO_WH_ShipmentNew
        (
            string S_FInterID,

            string S_HAWB,
            string S_PalletCount,
            string S_GrossWeight,
            string S_Project,

            string S_ShipDate,
            string S_PalletSeq,
            string S_EmptyCarton,

            string S_PalletSN,
            string S_ShipNO,
            string S_ShipID,

            string S_Regin,
            string S_ReferenceNO,
            string S_Country,
            string S_Carrier,
            string S_HubCode,
            string S_TruckNo,
            string S_ReturnAddress,
            string S_DeliveryStreetAddress,
            string S_DeliveryRegion,
            string S_DeliveryToName,
            string S_DeliveryCityName,
            string S_DeliveryCountry,
            string S_AdditionalDeliveryToName,
            string S_DeliveryPostalCode,
            string S_TelNo,

            string S_OceanContainerNumber,
            string S_Origin,
            string S_TotalVolume,
            string S_POE_COC,
            string S_TransportMethod,
            string S_POE,

            string S_Type            
        );

    [OperationContract]
    string DeleteShipmentNew(string FInterID);
    [OperationContract]
    string DeleteShipmentEntryNew(string FDetailID);
    [OperationContract]
    string UpdateShipmentNew_FStatus(string FInterID_List, string Status);
    [OperationContract]
    DataSet GetShipmentNew_One(string FInterID);
    [OperationContract]
    DataSet GetShipmentEntryNew_One(string FDetailID);
    [OperationContract]
     string Edit_CO_WH_ShipmentEntryNew
               (
                   string S_FDetailID,
                   string S_FInterID,
                   string S_FEntryID,
                   string S_FCarrierNo,
                   string S_FCommercialInvoice,
                   string S_FCrossWeight,
                   string S_FCTN,
                   string S_FKPONO,
                   string S_FLineItem,
                   string S_FMPNNO,
                   string S_FNetWeight,
                   string S_FOutSN,
                   string S_FPartNumberDesc,
                   string S_FQTY,
                   string S_FStatus,
                   string S_FProjectNO,
                   string S_Type
               );

    [OperationContract]
    DataSet GetORTluPartFamilyType(string DetailDefStr, string PartFamilyTypeID);
    [OperationContract]
    string GetORTCode(string PartFamilyTypeID, string YearID, string WeekID);
    [OperationContract]
    void InsertORTCodeData(string ORTCode, string PartFamilyTypeID, string YearID, string WeekID);
    [OperationContract]
    void UpdateORTCodeData(string NewORTCode, string OldORTCode);
    [OperationContract]
    DataSet GetORTMaxBatch(string OldORTCode, string TestTypeID);
    [OperationContract]
    DataSet GetORTForCode(string ORTCode);
    [OperationContract]
    string GetRouteCheck_Diagram(int Scan_StationTypeID, string LineID, DataTable DT_Unit, out string S_OutputStateID, string S_Str);

    [OperationContract]
    string AddmesReprintHistory(string PrintType, string SN, string StationID);
    [OperationContract]
    DataSet Get_WHExcel(string S_Start, string S_End, string FStatus);

    [OperationContract]
    DataSet GetRouteData(string LineID, string PartID, string PartFamilyID, string ProductionOrderID);

    [OperationContract]
    DataSet Get_SN_Shell(string FGSN, ref string strOutput);

    [OperationContract]
    DataSet GetIpad_BB();

    [OperationContract]
    string GetFirstStationType(string S_MachineSN);
    [OperationContract]
    DataSet GetSNToUnit(string S_SN);
	[OperationContract]
    string uspUpdateBox(string PartID, string ProductionOrderID, string S_CartonSN, LoginList loginList, string boxWeight);
    [OperationContract]
    DataSet GetMesPartAndPartFamilyDetail(int PartId, string PartDetailDefName, string PartFamilyDetailDefName, out string result);
    [OperationContract]
    string GetServerDateTime();
    [OperationContract]
    string GetSampleCount(string startTime, string endTime, bool type, int stationId);
    [OperationContract]
    string GetProductOrder(string barcode, int type);
    [OperationContract]
    DataSet GetProductDataInfo(string barcode, int type);
    [OperationContract]
    string GetMesStationAndStationTypeDetail(int StationId, string StationDetailDefName, string StationTypeDetailDefName, out string result);
    [OperationContract]
    int GetBarcodeType(string barcode);
    [OperationContract]
    DataSet GetAndCheckUnitInfo(string barcode, string POID, string PartID);
    [OperationContract]
    List<string> SnToPOID(string SN);
    [OperationContract]
    string uspUpdateBoxV2(string PartID, string ProductionOrderID, string S_CartonSN, LoginList loginList, string boxWeight);
    [OperationContract]
    string uspPalletPackagingV2(string PartID, string ProductionOrderID, string S_CartonSN, string S_PalletSN, LoginList loginList, int PalletQty, ref int boxCount);

    [OperationContract]
    DataSet GetmesUnitTTBox(string S_SN);

    [OperationContract]
    string GetAppSet(string S_SetName);
    [OperationContract]
    int InsertmesScreenshot(mesScreenshot model);

    [OperationContract]
    mesScreenshot GetmesScreenshot(int id);
    [OperationContract]
    Boolean UpdateScreenshot(mesScreenshot model);

    [OperationContract]
    DataSet ListmesScreenshot(string S_Where);

    [OperationContract]
    string ShipMentScalesCommint(string PalletSn, LoginList loginList, string PalletWeight, string POID);
    [OperationContract]
    string CheckShipmentPalletState(string ShipPallet, string POID);
    [OperationContract]
    void SetupPackageScalesDesc();
    [OperationContract]
    string[] GetPartIdByShippingPallet(string ShippingPalletSN);

    [OperationContract]
    string GetmesUnitStateSecond(string S_PartID, string PartFamilyID, string S_RouteSequence,
        string LineID, int StationTypeID, string ProductionOrderID, string StatusID, string S_SN);
    }



