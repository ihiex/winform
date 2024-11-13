using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using App.BLL;
using App.Model;
using System.Data;
using System.Configuration;


// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“PartSelectSVC”。
public class PartSelectSVC : IPartSelectSVC
{
    PartSelectBLL F_BLL = new PartSelectBLL();

    public double MyTest()
    {
        return F_BLL.MyTest();
    }

    public DataSet P_DataSet(string S_Sql)
    {
        return F_BLL.P_DataSet(S_Sql);
    }

    public string ExecSql(string S_Sql)
    {
        return F_BLL.ExecSql(S_Sql);
    }

    public DataSet GetluPartFamilyType()
    {
        return F_BLL.GetluPartFamilyType();
    }

    public string GetServerIP()
    {
        return F_BLL.GetServerIP();
    }
    public string GetDBName()
    {
        return F_BLL.GetDBName();
    }

    public string GetConn()
    {
        return F_BLL.GetConn();
    }
    public DataSet GetluPartFamily(string PartFamilyTypeID)
    {
        return F_BLL.GetluPartFamily(PartFamilyTypeID);
    }

    public DataSet GetmesPart(string PartFamilyID)
    {
        return F_BLL.GetmesPart(PartFamilyID);
    }

    public DataSet GetmesPartDetail(int PartID, string PartDetailDefName)
    {
        return F_BLL.GetmesPartDetail(PartID, PartDetailDefName);
    }

    public DataSet GetluPODetailDef(int ProductionOrderID, string PODetailDef)
    {
        return F_BLL.GetluPODetailDef(ProductionOrderID, PODetailDef);
    }

    public DataSet GetmesPartPrint()
    {
        return F_BLL.GetmesPartPrint();
    }

    public string mesGetSNFormatIDByList(string PartID, string PartFamilyID,
        string LineID, string ProductionOrderID, string StationTypeID)
    {
        return F_BLL.mesGetSNFormatIDByList(PartID, PartFamilyID, LineID, ProductionOrderID, StationTypeID);
    }

    public DataSet GetmesLine()
    {
        return F_BLL.GetmesLine();
    }

    public DataSet mesLineGroup(string LineType, int PartFamilyTypeID)
    {
        return F_BLL.mesLineGroup(LineType, PartFamilyTypeID);
    }

    public DataSet GetsysStatus()
    {
        return F_BLL.GetsysStatus();
    }

    public DataSet GetmesStationType()
    {
        return F_BLL.GetmesStationType();
    }

    public DataSet GetmesStation(string LineID)
    {
        return F_BLL.GetmesStation(LineID);
    }

    public DataSet GetmesStation2(string StationTypeID, string LineID)
    {
        return F_BLL.GetmesStation2(StationTypeID, LineID);
    }

    public DataSet GetmesStationTypeByStationID(string StationID)
    {
        return F_BLL.GetmesStationTypeByStationID(StationID);
    }

    public int GetRouteID(string LineID, string PartID, string PartFamilyID, string ProductionOrderID)
    {
        return F_BLL.GetRouteID(LineID, PartID, PartFamilyID, ProductionOrderID);
    }

    public DataSet GetmesRoute()
    {
        return F_BLL.GetmesRoute();
    }

    public DataSet GetRouteSequence(string RouteID, string StationTypeID)
    {
        return F_BLL.GetRouteSequence(RouteID, StationTypeID);
    }

    public DataSet GetluSerialNumberType()
    {
        return F_BLL.GetluSerialNumberType();
    }

    public DataSet GetUnit(string PartID)
    {
        return F_BLL.GetUnit(PartID);
    }

    public DataSet GetUnit2(string PartID, string StationID, string POID)
    {
        return F_BLL.GetUnit2(PartID, StationID, POID);
    }

    public DataSet GetUnit_Search(string S_DateStart, string S_DateEnd, string S_Where)
    {
        return F_BLL.GetUnit_Search(S_DateStart, S_DateEnd, S_Where);
    }

    public DataSet GetUnitComponent(string S_UnitID)
    {
        return F_BLL.GetUnitComponent(S_UnitID);
    }

    public DataSet GetmesUnitDetail(string S_UnitID)
    {
        return F_BLL.GetmesUnitDetail(S_UnitID);
    }


    public DataSet GetHistory(string UnitID)
    {
        return F_BLL.GetHistory(UnitID);
    }

    public DataSet GetMachineHistory(string S_UnitID)
    {
        return F_BLL.GetMachineHistory(S_UnitID);
    }

    public DataSet GetmesUnitState(string S_PartID, string PartFamilyID, string S_RouteSequence,
        string LineID, int StationTypeID, string ProductionOrderID, string StatusID)
    {
        return F_BLL.GetmesUnitState(S_PartID, PartFamilyID, S_RouteSequence, LineID,
            StationTypeID, ProductionOrderID, StatusID);
    }

    public DataSet GetmesUnitState_Diagram(string S_PartID, string PartFamilyID,
        string LineID, int StationTypeID, string ProductionOrderID)
    {
        return F_BLL.GetmesUnitState_Diagram(S_PartID, PartFamilyID, LineID, StationTypeID, ProductionOrderID);
    }

    public DataSet GetmesSerialNumberOfLine(string SNCategory, string PrintCount)
    {
        return F_BLL.GetmesSerialNumberOfLine(SNCategory, PrintCount);
    }

    public DataSet GetPO(string S_PartID, string S_StatusID)
    {
        return F_BLL.GetPO(S_PartID, S_StatusID);
    }

    public DataSet GetPOAll(string S_PartID, string S_LineID)
    {
        return F_BLL.GetPOAll(S_PartID, S_LineID);
    }

    public DataSet GetRoute(string S_RouteSequence, int I_RouteID)
    {
        return F_BLL.GetRoute(S_RouteSequence, I_RouteID);
    }

    public DataSet GetApplicationType(string StationTypeID)
    {
        return F_BLL.GetApplicationType(StationTypeID);
    }

    public DataSet GetluDefect(int DefectTypeID)
    {
        return F_BLL.GetluDefect(DefectTypeID);
    }

    public DataSet GetluUnitStatus()
    {
        return F_BLL.GetluUnitStatus();
    }


    public DataSet GetmesUnitDefect(string S_UnitID)
    {
        return F_BLL.GetmesUnitDefect(S_UnitID);
    }

    public DataSet uspSNRGetNext(string strSNFormat, int ReuseSNByStation, string xmlProdOrder,
                                      string xmlPart, string xmlStation, string xmlExtraData, string strNextSN)
    {
        return F_BLL.uspSNRGetNext(strSNFormat, ReuseSNByStation, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, strNextSN);
    }

    public DataSet GetPartDetailDef(string SN)
    {
        return F_BLL.GetPartDetailDef(SN);
    }

    public string Get_MachineRouteMap(string S_ToolSN, string S_ProductPartID, string S_RouteID, string S_StationTypeID)
    {
        return F_BLL.Get_MachineRouteMap(S_ToolSN, S_ProductPartID, S_RouteID, S_StationTypeID);
    }
    public string GetRouteCheck(int Scan_StationTypeID, int Scan_StationID, string LineID, DataTable DT_Unit, string S_Str)
    {
        return F_BLL.GetRouteCheck(Scan_StationTypeID, Scan_StationID, LineID, DT_Unit, S_Str);
    }

    public DataSet Get_UnitID(string S_SN)
    {
        return F_BLL.Get_UnitID(S_SN);
    }

    public string Get_CreateMesSN(string strSNFormat, LoginList loginList, string xmlProdOrder, string xmlPart, string xmlStation,
                                string xmlExtraData, mesUnit v_mesUnit, int number, ref DataSet dsSN)
    {
        return F_BLL.Get_CreateMesSN(strSNFormat, loginList, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, v_mesUnit, number, ref dsSN);
    }

    public string Get_CreateMesSN_New(string strSNFormat, LoginList loginList, string xmlProdOrder, string xmlPart, string xmlStation,
                            string xmlExtraData, mesUnit v_mesUnit, int number, ref DataSet dsSN)
    {
        return F_BLL.Get_CreateMesSN_New(strSNFormat, loginList, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, v_mesUnit, number, ref dsSN);
    }

    public DataSet BoxSnToBatch(string S_BoxSN, out string S_Result)
    {
        return F_BLL.BoxSnToBatch(S_BoxSN, out S_Result);
    }

    public int GetComponentCount(string ParentPartID, string StationTypeID)
    {
        return F_BLL.GetComponentCount(ParentPartID, StationTypeID);
    }

    public DataSet GetmesUnitComponent(string UnitID, string ChildUnitID)
    {
        return F_BLL.GetmesUnitComponent(UnitID, ChildUnitID);
    }
    public DataSet GetmesUnitComponent2(string UnitID)
    {
        return F_BLL.GetmesUnitComponent2(UnitID);
    }
    public DataSet GetmesProductStructure(string ParentPartID, string PartID, string StationTypeID)
    {
        return F_BLL.GetmesProductStructure(ParentPartID, PartID, StationTypeID);
    }
    public DataSet GetmesProductStructure1(string ParentPartID)
    {
        return F_BLL.GetmesProductStructure1(ParentPartID);
    }
    public DataSet GetmesProductStructure2(string ParentPartID, string StationTypeID)
    {
        return F_BLL.GetmesProductStructure2(ParentPartID, StationTypeID);
    }

    public DataSet GetChildScanLast(string S_SN)
    {
        return F_BLL.GetChildScanLast(S_SN);
    }

    public void ModPO(string S_POID)
    {
        F_BLL.ModPO(S_POID);
    }

    public void ModMachine(string S_SN, string StatusID, Boolean IsUpUnitDetail)
    {
        F_BLL.ModMachine(S_SN, StatusID, IsUpUnitDetail);
    }

    public void ModMachine2(string ID, string StatusID)
    {
        F_BLL.ModMachine2(ID, StatusID);
    }

    public DataSet GetmesSerialNumber(string S_SN)
    {
        return F_BLL.GetmesSerialNumber(S_SN);
    }

    public DataSet GetmesSerialNumberByUnitID(string UnitID)
    {
        return F_BLL.GetmesSerialNumberByUnitID(UnitID);
    }

    public DataSet GetmesUnit(string UnitID)
    {
        return F_BLL.GetmesUnit(UnitID);
    }

    public DataSet GetComponent(int I_ChildUnitID)
    {
        return F_BLL.GetComponent(I_ChildUnitID);
    }

    public DataSet GetmesMachine(string S_SN)
    {
        return F_BLL.GetmesMachine(S_SN);
    }

    public DataSet GetmesRouteMachineMap(string MachineID, string MachineFamilyID)
    {
        return F_BLL.GetmesRouteMachineMap(MachineID, MachineFamilyID);
    }

    public DataSet GetProductionOrder(string ID)
    {
        return F_BLL.GetProductionOrder(ID);
    }

    public DataSet GetMesPackageStatusID(string PalletSN)
    {
        return F_BLL.GetMesPackageStatusID(PalletSN);
    }

    public DataSet GetSNParameter(int PartID, int TemplateType)
    {
        return F_BLL.GetSNParameter(PartID, TemplateType);
    }

    public DataSet GetLBLGenLabel(string S_SN, string S_LabelName)
    {
        return F_BLL.GetLBLGenLabel(S_SN, S_LabelName);
    }

    public DataSet GetLabels(string PartID, string PartFamilyID, string LineID, string ProductionOrderID, string StationTypeID)
    {
        return F_BLL.GetLabels(PartID, PartFamilyID, LineID, ProductionOrderID, StationTypeID);
    }

    public DataSet GetLabelName(string StationTypeID, string PartFamilyID, string PartID, string ProductionOrderID, string LineID)
    {
        return F_BLL.GetLabelName(StationTypeID, PartFamilyID, PartID, ProductionOrderID, LineID);
    }

    public DataSet GetLabelCMD(string StationTypeID, string PartFamilyID, string PartID, string ProductionOrderID, string LineID)
    {
        return F_BLL.GetLabelCMD(StationTypeID, PartFamilyID, PartID, ProductionOrderID, LineID);
    }


    public string GetLabelID(string StationTypeID, string PartFamilyID, string PartID, string ProductionOrderID, string LineID)
    {
        return F_BLL.GetLabelID(StationTypeID, PartFamilyID, PartID, ProductionOrderID, LineID);
    }

    public DataSet LabelNameToLabelCMD(string S_LabelName)
    {
        return F_BLL.LabelNameToLabelCMD(S_LabelName);
    }


    public string BuckToFGSN(string S_BuckSN)
    {
        return F_BLL.BuckToFGSN(S_BuckSN);
    }

    public DataSet GetPLCSeting(string S_SetName, string S_StationID)
    {
        return F_BLL.GetPLCSeting(S_SetName, S_StationID);
    }

    public string TimeCheck(string StationID, string S_SN)
    {
        return F_BLL.TimeCheck(StationID, S_SN);
    }



    public DataSet GetPartParameter(string PartID)
    {
        return F_BLL.GetPartParameter(PartID);
    }

    public string GetluDefectType(string Description)
    {
        return F_BLL.GetluDefectType(Description);
    }

    public string uspKitBoxCheck(string S_FormatName, string xmlProdOrder, string xmlPart, string xmlStation, string xmlExtraData, string strSNbuf)
    {
        return F_BLL.uspKitBoxCheck(S_FormatName, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, strSNbuf);
    }

    public string uspKitBoxPackaging(string PartID, string ProductionOrderID, string S_UPCSN, string S_CartonSN, LoginList LoginList, int BoxQty = 0)
    {
        return F_BLL.uspKitBoxPackaging(PartID, ProductionOrderID, S_UPCSN, S_CartonSN, LoginList, BoxQty);
    }

    public string uspKitBoxPackagingNew(string PartID, string ProductionOrderID, string S_UPCSN, string S_CartonSN, LoginList LoginList, int Allnumber, int CurrentQty, int BoxQty = 0)
    {
        return F_BLL.uspKitBoxPackagingNew(PartID, ProductionOrderID, S_UPCSN, S_CartonSN, LoginList, Allnumber, CurrentQty, BoxQty);
    }

    public string uspPalletCheck(string S_FormatName, string xmlProdOrder, string xmlPart, string xmlStation, string xmlExtraData, string strSNbuf)
    {
        return F_BLL.uspPalletCheck(S_FormatName, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, strSNbuf);
    }

    public string uspPalletPackaging(string PartID, string ProductionOrderID, string S_CartonSN, string S_PalletSN, LoginList loginList, int PalletQty = 0)
    {
        return F_BLL.uspPalletPackaging(PartID, ProductionOrderID, S_CartonSN, S_PalletSN, loginList, PalletQty);
    }
    public string uspPalletPackaging_Siemens(string PartID, string ProductionOrderID, string S_CartonSN, string S_PalletSN, LoginList loginList, int PalletQty = 0)
    {
        return F_BLL.uspPalletPackaging_Siemens(PartID, ProductionOrderID, S_CartonSN, S_PalletSN, loginList, PalletQty);
    }
    public DataSet GetProductionOrderDetailDef(string ProductionOrderNumber)
    {
        return F_BLL.GetProductionOrderDetailDef(ProductionOrderNumber);
    }

    public string Get_CreatePackageSN(string S_FormatName, string xmlProdOrder, string xmlPart, string xmlStation,
                                    string xmlExtraData, mesUnit v_mesUnit, ref string strSN, int type)
    {
        return F_BLL.Get_CreatePackageSN(S_FormatName, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, v_mesUnit, ref strSN, type);
    }

    public string Get_CreatePackageSN_Siemens(string S_FormatName, string xmlProdOrder, string xmlPart, string xmlStation,
                                    string xmlExtraData, string MultipackSN, ref string strSN, int type)
    {
        return F_BLL.Get_CreatePackageSN_Siemens(S_FormatName, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, MultipackSN, ref strSN, type);
    }

    public DataSet Get_PackageData(string S_SN)
    {
        return F_BLL.Get_PackageData(S_SN);
    }

    public DataSet Get_PalletData(string S_SN)
    {
        return F_BLL.Get_PalletData(S_SN);
    }

    public DataSet Get_SearchData(string S_FormatName, string xmlProdOrder, string xmlPart,
                                    string xmlStation, string xmlExtraData, string strSNbuf, ref string strOutput)
    {
        return F_BLL.Get_SearchData(S_FormatName, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, strSNbuf, ref strOutput);
    }

    public DataSet uspCallProcedure(string Pro_Name, string S_FormatName, string xmlProdOrder, string xmlPart,
                                 string xmlStation, string xmlExtraData, string strSNbuf, ref string strOutput)
    {
        return F_BLL.uspCallProcedure(Pro_Name, S_FormatName, xmlProdOrder, xmlPart,
                              xmlStation, xmlExtraData, strSNbuf, ref strOutput);
    }

    public DataSet Get_PartParameter(string PartID)
    {
        return F_BLL.Get_PartParameter(PartID);
    }

    public DataSet MESGetBomPartInfo(int ParentPartID, int StationTypeID)
    {
        return F_BLL.MESGetBomPartInfo(ParentPartID, StationTypeID);
    }

    public string MESAssembleCheckMianSN(string ProductionOrderID, int LineID, int StationID, int StationTypeID, string SN, bool COF)
    {
        return F_BLL.MESAssembleCheckMianSN(ProductionOrderID, LineID, StationID, StationTypeID, SN, COF);
    }

    public string MESAssembleCheckOtherSN(string SN, string PartID, bool COF)
    {
        return F_BLL.MESAssembleCheckOtherSN(SN, PartID, COF);
    }

    public string MESAssembleCheckVirtualSN(string SN, string PartID, string Status)
    {
        return F_BLL.MESAssembleCheckVirtualSN(SN, PartID, Status);
    }

    public void MESModifyUnitDetail(int UnitID, string FileName, string Value)
    {
        F_BLL.MESModifyUnitDetail(UnitID, FileName, Value);
    }

    public string MESGetUnitUnitStateID(string SN)
    {
        return F_BLL.MESGetUnitUnitStateID(SN);
    }

    public DataSet MESGetStationTypeParameter(int stationTypeID)
    {
        return F_BLL.MESGetStationTypeParameter(stationTypeID);
    }
    public DataSet GetSerialNumber2(string S_SN)
    {
        return F_BLL.GetSerialNumber2(S_SN);
    }

    public DataSet GetLanguage(string FormName, string Type)
    {
        return F_BLL.GetLanguage(FormName, Type);
    }

    public string GetVer()
    {
        return F_BLL.GetVer();
    }

    public string GetMSG(string S_Lang, string S_Code)
    {
        return F_BLL.GetMSG(S_Lang, S_Code);
    }

    public DataSet GetVendor(string PartID)
    {
        return F_BLL.GetVendor(PartID);
    }

    //public DataSet GetRoute2(string LineID, string PartID, string PartFamilyID)
    //{
    //    return F_BLL.GetRoute2( LineID,  PartID,  PartFamilyID);
    //}

    //public string GetRouteCheck2(LoginList List_Login, DataTable DT_Unit, string S_Str)
    //{
    //    return F_BLL.GetRouteCheck2( List_Login,  DT_Unit,  S_Str);
    //}

    public bool SetToolingLinkTooling(string FromTooling, string ToTooling, int FromUintID, LoginList loginList)
    {
        return F_BLL.SetToolingLinkTooling(FromTooling, ToTooling, FromUintID, loginList);
    }

    public string Get_CreateMaterail(string strSNFormat, string xmlProdOrder, string xmlPart, string xmlStation,
                                      string xmlExtraData, LoginList loginList, mesUnit v_mesUnit, MesMaterialUnit v_mesMaterialUnit, int number, ref DataSet dsSN)
    {
        return F_BLL.Get_CreateMaterail(strSNFormat, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, loginList, v_mesUnit, v_mesMaterialUnit, number, ref dsSN);
    }

    public string ModMesMaterialConsumeInfo(LoginList loginList, int ScanType, string SN, string MachineSN, int PartID, int ProductionOrderID)
    {
        return F_BLL.ModMesMaterialConsumeInfo(loginList, ScanType, SN, MachineSN, PartID, ProductionOrderID);
    }

    public DataSet GetmesStationConfig(string Name, string StationID)
    {
        return F_BLL.GetmesStationConfig(Name, StationID);
    }

    public DataSet GetShipmentInterID(string ShipmentDetailID)
    {
        return F_BLL.GetShipmentInterID(ShipmentDetailID);
    }

    public void SetMesPackageShipmennt(string ShipmentDetailID, string SerialNumber, int Type)
    {
        F_BLL.SetMesPackageShipmennt(ShipmentDetailID, SerialNumber, Type);
    }

    public DataSet GetMesLabelData(string LabelName)
    {
        return F_BLL.GetMesLabelData(LabelName);
    }

    public int InsertMesPackageHistory(mesPackageHistory model)
    {
        return F_BLL.InsertMesPackageHistory(model);
    }

    public DataSet GetShipmentNew(string S_Start, string S_End, string FStatus)
    {
        return F_BLL.GetShipmentNew(S_Start, S_End, FStatus);
    }

    public DataSet GetShipmentEntryNew(string S_FInterID)
    {
        return F_BLL.GetShipmentEntryNew(S_FInterID);
    }
    public DataSet GetShipmentReport(string S_Start, string S_End, string FStatus)
    {
        return F_BLL.GetShipmentReport(S_Start, S_End, FStatus);
    }

    public string Edit_CO_WH_ShipmentNew
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
        )
    {
        return F_BLL.Edit_CO_WH_ShipmentNew
            (
                 S_FInterID,

                 S_HAWB,
                 S_PalletCount,
                 S_GrossWeight,
                 S_Project,

                 S_ShipDate,
                 S_PalletSeq,
                 S_EmptyCarton,

                 S_PalletSN,
                 S_ShipNO,
                 S_ShipID,

                 S_Regin,
                 S_ReferenceNO,
                 S_Country,
                 S_Carrier,
                 S_HubCode,
                 S_TruckNo,
                 S_ReturnAddress,
                 S_DeliveryStreetAddress,
                 S_DeliveryRegion,
                 S_DeliveryToName,
                 S_DeliveryCityName,
                 S_DeliveryCountry,
                 S_AdditionalDeliveryToName,
                 S_DeliveryPostalCode,
                 S_TelNo,

                 S_OceanContainerNumber,
                 S_Origin,
                 S_TotalVolume,
                 S_POE_COC,
                 S_TransportMethod,
                 S_POE,

                 S_Type
            );
    }


    public string DeleteShipmentNew(string FInterID)
    {
        return F_BLL.DeleteShipmentNew(FInterID);
    }

    public string DeleteShipmentEntryNew(string FDetailID)
    {
        return F_BLL.DeleteShipmentEntryNew(FDetailID);
    }

    public string UpdateShipmentNew_FStatus(string FInterID_List, string Status)
    {
        return F_BLL.UpdateShipmentNew_FStatus(FInterID_List, Status);
    }

    public DataSet GetShipmentNew_One(string FInterID)
    {
        return F_BLL.GetShipmentNew_One(FInterID);
    }

    public DataSet GetShipmentEntryNew_One(string FDetailID)
    {
        return F_BLL.GetShipmentEntryNew_One(FDetailID);
    }

    public string Edit_CO_WH_ShipmentEntryNew
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
        )
    {
        return F_BLL.Edit_CO_WH_ShipmentEntryNew
            (
             S_FDetailID,
             S_FInterID,
             S_FEntryID,
             S_FCarrierNo,
             S_FCommercialInvoice,
             S_FCrossWeight,
             S_FCTN,
             S_FKPONO,
             S_FLineItem,
             S_FMPNNO,
             S_FNetWeight,
             S_FOutSN,
             S_FPartNumberDesc,
             S_FQTY,
             S_FStatus,
             S_FProjectNO,
             S_Type
            );
    }

    public DataSet GetORTluPartFamilyType(string DetailDefStr, string PartFamilyTypeID)
    {
        return F_BLL.GetORTluPartFamilyType(DetailDefStr, PartFamilyTypeID);
    }

    public string GetORTCode(string PartFamilyTypeID, string YearID, string WeekID)
    {
        return F_BLL.GetORTCode(PartFamilyTypeID, YearID, WeekID);
    }

    public void InsertORTCodeData(string ORTCode, string PartFamilyTypeID, string YearID, string WeekID)
    {
        F_BLL.InsertORTCodeData(ORTCode, PartFamilyTypeID, YearID, WeekID);
    }

    public void UpdateORTCodeData(string NewORTCode, string OldORTCode)
    {
        F_BLL.UpdateORTCodeData(NewORTCode, OldORTCode);
    }

    public DataSet GetORTMaxBatch(string OldORTCode, string TestTypeID)
    {
        return F_BLL.GetORTMaxBatch(OldORTCode, TestTypeID);
    }

    public DataSet GetORTForCode(string ORTCode)
    {
        return F_BLL.GetORTForCode(ORTCode);
    }

    public string GetRouteCheck_Diagram(int Scan_StationTypeID, string LineID, DataTable DT_Unit, out string S_OutputStateID, string S_Str)
    {
        return F_BLL.GetRouteCheck_Diagram(Scan_StationTypeID, LineID, DT_Unit, out S_OutputStateID, S_Str);
    }

    public string AddmesReprintHistory(string PrintType, string SN, string StationID)
    {
        return F_BLL.AddmesReprintHistory(PrintType, SN, StationID);
    }

    public DataSet Get_WHExcel(string S_Start, string S_End, string FStatus)
    {
        return F_BLL.Get_WHExcel(S_Start, S_End, FStatus);
    }

    public DataSet GetRouteData(string LineID, string PartID, string PartFamilyID, string ProductionOrderID)
    {
        return F_BLL.GetRouteData(LineID, PartID, PartFamilyID, ProductionOrderID);
    }

    public DataSet Get_SN_Shell(string FGSN, ref string strOutput)
    {
        return F_BLL.Get_SN_Shell(FGSN, ref strOutput);
    }

    public DataSet GetIpad_BB()
    {
        return F_BLL.GetIpad_BB();
    }

    public string GetFirstStationType(string S_MachineSN)
    {
        return F_BLL.GetFirstStationType(S_MachineSN);
    }

    public DataSet GetSNToUnit(string S_SN)
    {
        return F_BLL.GetSNToUnit(S_SN);
    }
    public string uspUpdateBox(string PartID, string ProductionOrderID, string S_CartonSN, LoginList loginList, string boxWeight)
    {
        return F_BLL.uspUpdateBox(PartID, ProductionOrderID, S_CartonSN, loginList, boxWeight);
    }
    public DataSet GetMesPartAndPartFamilyDetail(int PartId, string PartDetailDefName, string PartFamilyDetailDefName, out string result)
    {
        return F_BLL.GetMesPartAndPartFamilyDetail(PartId, PartDetailDefName, PartFamilyDetailDefName, out result);
    }
    public string GetServerDateTime()
    {
        return F_BLL.GetServerDateTime();
    }
    public string GetSampleCount(string startTime, string endTime, bool type, int stationId)
    {
        return F_BLL.GetSampleCount(startTime, endTime, type, stationId);
    }
    public string GetProductOrder(string barcode, int type)
    {
        return F_BLL.GetProductOrder(barcode, type);
    }
    public DataSet GetProductDataInfo(string barcode, int type)
    {
        return F_BLL.GetProductDataInfo(barcode, type);
    }
    public string GetMesStationAndStationTypeDetail(int StationId, string StationDetailDefName, string StationTypeDetailDefName, out string result)
    {
        return F_BLL.GetMesStationAndStationTypeDetail(StationId, StationDetailDefName, StationTypeDetailDefName, out result);
    }
    public int GetBarcodeType(string barcode)
    {
        return F_BLL.GetBarcodeType(barcode);
    }
    public DataSet GetAndCheckUnitInfo(string barcode, string POID, string PartID)
    {
        return F_BLL.GetAndCheckUnitInfo(barcode, POID, PartID);
    }
    public List<string> SnToPOID(string SN)
    {
        return F_BLL.SnToPOID(SN);
    }
    public string uspUpdateBoxV2(string PartID, string ProductionOrderID, string S_CartonSN, LoginList loginList, string boxWeight)
    {
        return F_BLL.uspUpdateBoxV2(PartID, ProductionOrderID, S_CartonSN, loginList, boxWeight);
    }
    public string uspPalletPackagingV2(string PartID, string ProductionOrderID, string S_CartonSN, string S_PalletSN, LoginList loginList, int PalletQty, ref int boxCount)
    {
        return F_BLL.uspPalletPackagingV2(PartID, ProductionOrderID, S_CartonSN, S_PalletSN, loginList, PalletQty, ref boxCount);
    }


    public DataSet GetmesUnitTTBox(string S_SN)
    {
        return F_BLL.GetmesUnitTTBox(S_SN);
    }


    public string GetAppSet(string S_SetName)
    {
        return F_BLL.GetAppSet(S_SetName);
    }

    public int InsertmesScreenshot(mesScreenshot model)
    {
        return F_BLL.InsertmesScreenshot(model);
    }

    public mesScreenshot GetmesScreenshot(int id)
    {
        return F_BLL.GetmesScreenshot(id);
    }

    public Boolean UpdateScreenshot(mesScreenshot model)
    {
        return F_BLL.UpdateScreenshot(model);
    }

    public DataSet ListmesScreenshot(string S_Where)
    {
        return F_BLL.ListmesScreenshot(S_Where);
    }

    public string ShipMentScalesCommint(string PalletSn, LoginList loginList, string PalletWeight, string POID)
    {
        return F_BLL.ShipMentScalesCommint(PalletSn, loginList, PalletWeight,POID);
    }

    public string CheckShipmentPalletState(string ShipPallet, string POID)
    {
        return F_BLL.CheckShipmentPalletState(ShipPallet,POID);
    }

    public void SetupPackageScalesDesc()
    {
        F_BLL.SetupPackageScalesDesc();
    }

    public string[] GetPartIdByShippingPallet(string ShippingPalletSN)
    {
        return F_BLL.GetPartIdByShippingPallet(ShippingPalletSN);
    }
    public string GetmesUnitStateSecond(string S_PartID, string PartFamilyID, string S_RouteSequence,
        string LineID, int StationTypeID, string ProductionOrderID, string StatusID, string S_SN)
    {
        return F_BLL.GetmesUnitStateSecond(S_PartID, PartFamilyID, S_RouteSequence,
            LineID, StationTypeID, ProductionOrderID, StatusID, S_SN);
    }
}


