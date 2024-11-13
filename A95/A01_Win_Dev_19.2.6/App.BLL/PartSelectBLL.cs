using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Model;
using App.DBServerDAL;
using System.Data;

namespace App.BLL
{
    public class PartSelectBLL
    {
        public double MyTest()
        {
            return new PartSelectDAL().MyTest();
        }
        public DataSet P_DataSet(string S_Sql)
        {
            return new PartSelectDAL().P_DataSet(S_Sql);
        }

        public string ExecSql(string S_Sql)
        {
            return new PartSelectDAL().ExecSql(S_Sql);
        }

        public string GetConn()
        {
            return new PartSelectDAL().GetConn();
        }
        public string GetServerIP()
        {
            return new PartSelectDAL().GetServerIP();
        }

        public string GetDBName()
        {
            return new PartSelectDAL().GetDBName();
        }

        public DataSet GetluPartFamilyType()
        {
            return new PartSelectDAL().GetluPartFamilyType();
        }

        public DataSet GetluPartFamily(string PartFamilyTypeID)
        {
            return new PartSelectDAL().GetluPartFamily(PartFamilyTypeID);
        }

        public DataSet GetmesPart(string PartFamilyID)
        {
            return new PartSelectDAL().GetmesPart(PartFamilyID);
        }

        public DataSet GetmesPartDetail(int PartID, string PartDetailDefName)
        {
            return new PartSelectDAL().GetmesPartDetail(PartID, PartDetailDefName);
        }

        public DataSet GetluPODetailDef(int ProductionOrderID, string PODetailDef)
        {
            return new PartSelectDAL().GetluPODetailDef(ProductionOrderID, PODetailDef);
        }

        public DataSet GetmesPartPrint()
        {
            return new PartSelectDAL().GetmesPartPrint();
        }

        public string mesGetSNFormatIDByList(string PartID, string PartFamilyID, string LineID,
                                                string ProductionOrderID, string StationTypeID)
        {
            return new PartSelectDAL().mesGetSNFormatIDByList(PartID, PartFamilyID, LineID, ProductionOrderID, StationTypeID);
        }

        public DataSet GetmesLine()
        {
            return new PartSelectDAL().GetmesLine();
        }

        public DataSet mesLineGroup(string LineType, int PartFamilyTypeID)
        {
            return new PartSelectDAL().mesLineGroup(LineType, PartFamilyTypeID);
        }

        public DataSet GetsysStatus()
        {
            return new PartSelectDAL().GetsysStatus();
        }

        public DataSet GetmesStationType()
        {
            return new PartSelectDAL().GetmesStationType();
        }

        public DataSet GetmesStation(string LineID)
        {
            return new PartSelectDAL().GetmesStation(LineID);
        }

        public DataSet GetmesStation2(string StationTypeID, string LineID)
        {
            return new PartSelectDAL().GetmesStation2(StationTypeID, LineID);
        }


        public DataSet GetmesStationTypeByStationID(string StationID)
        {
            return new PartSelectDAL().GetmesStationTypeByStationID(StationID);
        }

        public DataSet GetmesRoute()
        {
            return new PartSelectDAL().GetmesRoute();
        }

        public DataSet GetRouteSequence(string RouteID, string StationTypeID)
        {
            return new PartSelectDAL().GetRouteSequence(RouteID, StationTypeID);
        }

        public DataSet GetluSerialNumberType()
        {
            return new PartSelectDAL().GetluSerialNumberType();
        }

        public DataSet GetUnit(string PartID)   //暂时停用
        {
            return new PartSelectDAL().GetUnit(PartID);
        }

        public DataSet GetUnit2(string PartID, string StationID, string POID)  //暂时停用
        {
            return new PartSelectDAL().GetUnit2(PartID, StationID, POID);
        }

        public DataSet GetUnit_Search(string S_DateStart, string S_DateEnd, string S_Where)
        {
            return new PartSelectDAL().GetUnit_Search(S_DateStart, S_DateEnd, S_Where);
        }


        public DataSet GetUnitComponent(string S_UnitID)
        {
            return new PartSelectDAL().GetUnitComponent(S_UnitID);
        }

        public DataSet GetmesUnitDetail(string S_UnitID)
        {
            return new PartSelectDAL().GetmesUnitDetail(S_UnitID);
        }

        public DataSet GetHistory(string UnitID)
        {
            return new PartSelectDAL().GetHistory(UnitID);
        }

        public DataSet GetMachineHistory(string S_UnitID)
        {
            return new PartSelectDAL().GetMachineHistory(S_UnitID);
        }

        public DataSet GetmesUnitState(string S_PartID, string PartFamilyID, string S_RouteSequence, 
                                        string LineID, int StationTypeID,string ProductionOrderID, string StatusID)
        {
            return new PartSelectDAL().GetmesUnitState(S_PartID, PartFamilyID, S_RouteSequence,
                                                        LineID, StationTypeID, ProductionOrderID, StatusID);
        }

        public string GetmesUnitStateSecond(string S_PartID, string PartFamilyID, string S_RouteSequence,
            string LineID, int StationTypeID, string ProductionOrderID, string StatusID, string S_SN)
        {
            return new PartSelectDAL().GetmesUnitStateSecond(S_PartID, PartFamilyID, S_RouteSequence,
                LineID, StationTypeID, ProductionOrderID, StatusID, S_SN);
        }
        public DataSet GetmesUnitState_Diagram(string S_PartID, string PartFamilyID, 
                                                string LineID, int StationTypeID, string ProductionOrderID)
        {
            return new PartSelectDAL().GetmesUnitState_Diagram(S_PartID, PartFamilyID, LineID, StationTypeID, ProductionOrderID);
        }

        public DataSet GetmesSerialNumberOfLine(string SNCategory, string PrintCount)
        {
            return new PartSelectDAL().GetmesSerialNumberOfLine(SNCategory, PrintCount);
        }

        public DataSet GetPO(string S_PartID, string S_StatusID)
        {
            return new PartSelectDAL().GetPO(S_PartID, S_StatusID);
        }
        public DataSet GetPOAll(string S_PartID, string S_LineID)
        {
            return new PartSelectDAL().GetPOAll(S_PartID, S_LineID);
        }

        public int GetRouteID(string LineID, string PartID, string PartFamilyID,string ProductionOrderID)
        {
            return new PartSelectDAL().GetRouteID(LineID, PartID, PartFamilyID, ProductionOrderID);
        }

        public DataSet GetRoute(string S_RouteSequence, int I_RouteID)
        {
            return new PartSelectDAL().GetRoute(S_RouteSequence, I_RouteID);
        }

        public DataSet GetApplicationType(string StationTypeID)
        {
            return new PartSelectDAL().GetApplicationType(StationTypeID);
        }
        public DataSet GetluDefect(int DefectTypeID)
        {
            return new PartSelectDAL().GetluDefect(DefectTypeID);
        }

        public DataSet GetluUnitStatus()
        {
            return new PartSelectDAL().GetluUnitStatus();
        }

        public DataSet GetmesUnitDefect(string S_UnitID)
        {
            return new PartSelectDAL().GetmesUnitDefect(S_UnitID);
        }

        public string Get_MachineRouteMap(string S_ToolSN, string S_ProductPartID, string S_RouteID, string S_StationTypeID)
        {
            return new PartSelectDAL().Get_MachineRouteMap(S_ToolSN, S_ProductPartID, S_RouteID, S_StationTypeID);
        }

        public DataSet uspSNRGetNext(string strSNFormat, int ReuseSNByStation, string xmlProdOrder,
                                      string xmlPart, string xmlStation, string xmlExtraData, string strNextSN)
        {
            return new PartSelectDAL().uspSNRGetNext(strSNFormat, ReuseSNByStation, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, strNextSN);
        }

        public DataSet GetPartDetailDef(string SN)
        {
            return new PartSelectDAL().GetPartDetailDef(SN);
        }
        public string GetRouteCheck(int Scan_StationTypeID, int Scan_StationID, string LineID, DataTable DT_Unit, string S_Str)
        {
            return new PartSelectDAL().GetRouteCheck(Scan_StationTypeID, Scan_StationID, LineID, DT_Unit, S_Str);
        }

        public DataSet Get_UnitID(string S_SN)
        {
            return new PartSelectDAL().Get_UnitID(S_SN);
        }

        public string Get_CreateMesSN(string strSNFormat, LoginList loginList, string xmlProdOrder, string xmlPart, string xmlStation,
                                string xmlExtraData, mesUnit v_mesUnit, int number, ref DataSet dsSN)
        {
            return new PartSelectDAL().Get_CreateMesSN(strSNFormat, loginList, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, v_mesUnit, number, ref dsSN);
        }

        public string Get_CreateMesSN_New(string strSNFormat, LoginList loginList, string xmlProdOrder, string xmlPart, string xmlStation,
                           string xmlExtraData, mesUnit v_mesUnit, int number, ref DataSet dsSN)
        {
            return new PartSelectDAL().Get_CreateMesSN_New(strSNFormat, loginList, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, v_mesUnit, number, ref dsSN);
        }

        public DataSet BoxSnToBatch(string S_BoxSN, out string S_Result)
        {
            S_Result = "";
            return new PartSelectDAL().BoxSnToBatch(S_BoxSN, out S_Result);
        }

        public int GetComponentCount(string ParentPartID, string StationTypeID)
        {
            return new PartSelectDAL().GetComponentCount(ParentPartID, StationTypeID);
        }

        public DataSet GetmesUnitComponent(string UnitID, string ChildUnitID)
        {
            return new PartSelectDAL().GetmesUnitComponent(UnitID, ChildUnitID);
        }

        public DataSet GetmesUnitComponent2(string UnitID)
        {
            return new PartSelectDAL().GetmesUnitComponent2(UnitID);
        }

        public DataSet GetmesProductStructure(string ParentPartID, string PartID, string StationTypeID)
        {
            return new PartSelectDAL().GetmesProductStructure(ParentPartID, PartID, StationTypeID);
        }

        public DataSet GetmesProductStructure1(string ParentPartID)
        {
            return new PartSelectDAL().GetmesProductStructure1(ParentPartID);
        }

        public DataSet GetmesProductStructure2(string ParentPartID, string StationTypeID)
        {
            return new PartSelectDAL().GetmesProductStructure2(ParentPartID, StationTypeID);
        }

        public DataSet GetChildScanLast(string S_SN)
        {
            return new PartSelectDAL().GetChildScanLast(S_SN);
        }

        public void ModPO(string S_POID)
        {
            new PartSelectDAL().ModPO(S_POID);
        }

        public void ModMachine(string S_SN, string StatusID, Boolean IsUpUnitDetail)
        {
            new PartSelectDAL().ModMachine(S_SN, StatusID, IsUpUnitDetail);
        }

        public void ModMachine2(string ID, string StatusID)
        {
            new PartSelectDAL().ModMachine2(ID, StatusID);
        }

        public DataSet GetmesSerialNumber(string S_SN)
        {
            return new PartSelectDAL().GetmesSerialNumber(S_SN);
        }

        public DataSet GetmesSerialNumberByUnitID(string UnitID)
        {
            return new PartSelectDAL().GetmesSerialNumberByUnitID(UnitID);
        }

        public DataSet GetmesUnit(string UnitID)
        {
            return new PartSelectDAL().GetmesUnit(UnitID);
        }

        public DataSet GetComponent(int I_ChildUnitID)
        {
            return new PartSelectDAL().GetComponent(I_ChildUnitID);
        }

        public DataSet GetmesMachine(string S_SN)
        {
            return new PartSelectDAL().GetmesMachine(S_SN);
        }

        public DataSet GetmesRouteMachineMap(string MachineID, string MachineFamilyID)
        {
            return new PartSelectDAL().GetmesRouteMachineMap(MachineID, MachineFamilyID);
        }

        public DataSet GetProductionOrder(string ID)
        {
            return new PartSelectDAL().GetProductionOrder(ID);
        }

        public DataSet GetMesPackageStatusID(string PalletSN)
        {
            return new PartSelectDAL().GetMesPackageStatusID(PalletSN);
        }

        public DataSet GetSNParameter(int PartID, int TemplateType)
        {
            return new PartSelectDAL().GetSNParameter(PartID, TemplateType);
        }

        public DataSet GetLBLGenLabel(string S_SN, string S_LabelName)
        {
            return new PartSelectDAL().GetLBLGenLabel(S_SN, S_LabelName);
        }

        public DataSet GetLabels(string PartID, string PartFamilyID, string LineID, string ProductionOrderID, string StationTypeID)
        {
            return new PartSelectDAL().GetLabels(PartID, PartFamilyID, LineID, ProductionOrderID, StationTypeID);
        }

        public DataSet GetLabelName(string StationTypeID, string PartFamilyID, string PartID, string ProductionOrderID, string LineID)
        {
            return new PartSelectDAL().GetLabelName(StationTypeID, PartFamilyID, PartID, ProductionOrderID, LineID);
        }

        public DataSet GetLabelCMD(string StationTypeID, string PartFamilyID, string PartID, string ProductionOrderID, string LineID)
        {
            return new PartSelectDAL().GetLabelCMD(StationTypeID, PartFamilyID, PartID, ProductionOrderID, LineID);
        }

        public string GetLabelID(string StationTypeID, string PartFamilyID, string PartID, string ProductionOrderID, string LineID)
        {
            return new PartSelectDAL().GetLabelID(StationTypeID, PartFamilyID, PartID, ProductionOrderID, LineID);
        }

        public DataSet LabelNameToLabelCMD(string S_LabelName)
        {
            return new PartSelectDAL().LabelNameToLabelCMD(S_LabelName);
        }

        public string BuckToFGSN(string S_BuckSN)
        {
            return new PartSelectDAL().BuckToFGSN(S_BuckSN);
        }

        public DataSet GetPLCSeting(string S_SetName, string S_StationID)
        {
            return new PartSelectDAL().GetPLCSeting(S_SetName, S_StationID);
        }

        public string TimeCheck(string StationID, string S_SN)
        {
            return new PartSelectDAL().TimeCheck(StationID, S_SN);
        }

        //**********************************************************************************************************************// 
        //**********************************************************************************************************************// 
        //**********************************************************************************************************************// 


        public DataSet GetPartParameter(string PartID)
        {
            return new PartSelectDAL().GetPartParameter(PartID);
        }

        public string GetluDefectType(string Description)
        {
            return new PartSelectDAL().GetluDefectType(Description);
        }


        public string uspKitBoxCheck(string S_FormatName, string xmlProdOrder, string xmlPart, string xmlStation, string xmlExtraData, string strSNbuf)
        {
            return new PartSelectDAL().uspKitBoxCheck(S_FormatName, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, strSNbuf);
        }

        public string uspKitBoxPackaging(string PartID, string ProductionOrderID, string S_UPCSN, string S_CartonSN, LoginList LoginList, int BoxQty = 0)
        {
            return new PartSelectDAL().uspKitBoxPackaging(PartID, ProductionOrderID, S_UPCSN, S_CartonSN, LoginList, BoxQty);
        }

        public string uspKitBoxPackagingNew(string PartID, string ProductionOrderID, string S_UPCSN, string S_CartonSN, LoginList LoginList,int Allnumber,int CurrentQty, int BoxQty = 0)
        {
            return new PartSelectDAL().uspKitBoxPackagingNew(PartID, ProductionOrderID, S_UPCSN, S_CartonSN, LoginList,Allnumber, CurrentQty, BoxQty);
        }

        public string uspPalletCheck(string S_FormatName, string xmlProdOrder, string xmlPart, string xmlStation, string xmlExtraData, string strSNbuf)
        {
            return new PartSelectDAL().uspPalletCheck(S_FormatName, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, strSNbuf);
        }

        public string uspPalletPackaging(string PartID, string ProductionOrderID, string S_CartonSN, string S_PalletSN, LoginList loginList, int PalletQty = 0)
        {
            return new PartSelectDAL().uspPalletPackaging(PartID, ProductionOrderID, S_CartonSN, S_PalletSN, loginList, PalletQty);
        }

        public string uspPalletPackaging_Siemens(string PartID, string ProductionOrderID, string S_CartonSN, string S_PalletSN, LoginList loginList, int PalletQty = 0)
        {
            return new PartSelectDAL().uspPalletPackaging_Siemens(PartID, ProductionOrderID, S_CartonSN, S_PalletSN, loginList, PalletQty);
        }

        public DataSet GetProductionOrderDetailDef(string ProductionOrderNumber)
        {
            return new PartSelectDAL().GetProductionOrderDetailDef(ProductionOrderNumber);
        }

        public string Get_CreatePackageSN(string S_FormatName, string xmlProdOrder, string xmlPart, string xmlStation,
                                        string xmlExtraData, mesUnit v_mesUnit, ref string strSN, int type)
        {
            return new PartSelectDAL().Get_CreatePackageSN(S_FormatName, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, v_mesUnit, ref strSN, type);
        }

        public string Get_CreatePackageSN_Siemens(string S_FormatName, string xmlProdOrder, string xmlPart, string xmlStation,
                                        string xmlExtraData, string MultipackSN, ref string strSN, int type)
        {
            return new PartSelectDAL().Get_CreatePackageSN_Siemens(S_FormatName, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, MultipackSN, ref strSN, type);
        }


        public DataSet Get_PackageData(string S_SN)
        {
            return new PartSelectDAL().Get_PackageData(S_SN);
        }

        public DataSet Get_PalletData(string S_SN)
        {
            return new PartSelectDAL().Get_PalletData(S_SN);
        }

        public DataSet Get_SearchData(string S_FormatName, string xmlProdOrder, string xmlPart, string xmlStation,
                                        string xmlExtraData, string strSNbuf, ref string strOutput)
        {
            return new PartSelectDAL().Get_SearchData(S_FormatName, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, strSNbuf, ref strOutput);
        }


        public DataSet Get_SN_Shell(string FGSN, ref string strOutput)
        {
            return new PartSelectDAL().Get_SN_Shell(FGSN, ref strOutput);
        }

        public DataSet uspCallProcedure(string Pro_Name, string S_FormatName, string xmlProdOrder, string xmlPart,
                         string xmlStation, string xmlExtraData, string strSNbuf, ref string strOutput)
        {
            return new PartSelectDAL().uspCallProcedure(Pro_Name, S_FormatName, xmlProdOrder, xmlPart,
                                  xmlStation, xmlExtraData, strSNbuf, ref strOutput);
        }

        public DataSet Get_PartParameter(string PartID)
        {
            return new PartSelectDAL().Get_PartParameter(PartID);
        }

        public DataSet MESGetBomPartInfo(int ParentPartID, int StationTypeID)
        {
            return new PartSelectDAL().MESGetBomPartInfo(ParentPartID, StationTypeID);
        }

        public string MESAssembleCheckMianSN(string ProductionOrderID, int LineID, int StationID, int StationTypeID, string SN,bool COF)
        {
            return new PartSelectDAL().MESAssembleCheckMianSN(ProductionOrderID, LineID, StationID, StationTypeID, SN, COF);
        }

        public string MESAssembleCheckOtherSN(string SN, string PartID, bool COF)
        {
            return new PartSelectDAL().MESAssembleCheckOtherSN(SN, PartID, COF);
        }

        public string MESAssembleCheckVirtualSN(string SN, string PartID, string Status)
        {
            return new PartSelectDAL().MESAssembleCheckVirtualSN(SN, PartID, Status);
        }

        public void MESModifyUnitDetail(int UnitID, string FileName, string Value)
        {
            new PartSelectDAL().MESModifyUnitDetail(UnitID, FileName, Value);
        }

        public string MESGetUnitUnitStateID(string SN)
        {
            return new PartSelectDAL().MESGetUnitUnitStateID(SN);
        }

        public DataSet MESGetStationTypeParameter(int stationTypeID)
        {
            return new PartSelectDAL().MESGetStationTypeParameter(stationTypeID);
        }

        public DataSet GetSerialNumber2(string S_SN)
        {
            return new PartSelectDAL().GetSerialNumber2(S_SN);
        }

        public DataSet GetLanguage(string FormName, string Type)
        {
            return new PartSelectDAL().GetLanguage(FormName, Type);
        }

        public string GetVer()
        {
            return new PartSelectDAL().GetVer();
        }

        public string GetMSG(string S_Lang, string S_Code)
        {
            return new PartSelectDAL().GetMSG(S_Lang, S_Code);
        }

        public DataSet GetVendor(string PartID)
        {
            return new PartSelectDAL().GetVendor(PartID);
        }

        //public DataSet GetRoute2(string LineID, string PartID, string PartFamilyID)
        //{
        //    return new PartSelectDAL().GetRoute2(LineID, PartID, PartFamilyID);
        //}

        //public string GetRouteCheck2(LoginList List_Login, DataTable DT_Unit, string S_Str)
        //{
        //    return new PartSelectDAL().GetRouteCheck2(List_Login, DT_Unit, S_Str);
        //}

        public bool SetToolingLinkTooling(string FromTooling, string ToTooling, int FromUintID, LoginList loginList)
        {
            return new PartSelectDAL().SetToolingLinkTooling(FromTooling, ToTooling, FromUintID, loginList);
        }

        public string Get_CreateMaterail(string strSNFormat, string xmlProdOrder, string xmlPart, string xmlStation,
                                      string xmlExtraData, LoginList loginList, mesUnit v_mesUnit, MesMaterialUnit v_mesMaterialUnit, int number, ref DataSet dsSN)
        {
            return new PartSelectDAL().Get_CreateMaterail(strSNFormat, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, loginList, v_mesUnit, v_mesMaterialUnit, number, ref dsSN);
        }

        public string ModMesMaterialConsumeInfo(LoginList loginList, int ScanType, string SN, string MachineSN, int PartID, int ProductionOrderID)
        {
            return new PartSelectDAL().ModMesMaterialConsumeInfo(loginList, ScanType, SN, MachineSN, PartID, ProductionOrderID);
        }

        public DataSet GetmesStationConfig(string Name, string StationID)
        {
            return new PartSelectDAL().GetmesStationConfig(Name, StationID);
        }

        public DataSet GetShipmentInterID(string ShipmentDetailID)
        {
            return new PartSelectDAL().GetShipmentInterID(ShipmentDetailID);
        }

        public void SetMesPackageShipmennt(string ShipmentDetailID, string SerialNumber, int Type)
        {
            new PartSelectDAL().SetMesPackageShipmennt(ShipmentDetailID, SerialNumber, Type);
        }

        public DataSet GetMesLabelData(string LabelName)
        {
            return new PartSelectDAL().GetMesLabelData(LabelName);
        }

        public int InsertMesPackageHistory(mesPackageHistory model)
        {
            return new PartSelectDAL().InsertMesPackageHistory(model);
        }

        public DataSet GetShipmentNew(string S_Start, string S_End, string FStatus)
        {
            return new PartSelectDAL().GetShipmentNew(S_Start, S_End, FStatus);
        }

        public DataSet GetShipmentEntryNew(string S_FInterID)
        {
            return new PartSelectDAL().GetShipmentEntryNew(S_FInterID);
        }

        public DataSet GetShipmentReport(string S_Start, string S_End, string FStatus)
        {
            return new PartSelectDAL().GetShipmentReport(S_Start, S_End, FStatus);
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
            return new PartSelectDAL().Edit_CO_WH_ShipmentNew
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
            return new PartSelectDAL().DeleteShipmentNew(FInterID);
        }

        public string DeleteShipmentEntryNew(string FDetailID)
        {
            return new PartSelectDAL().DeleteShipmentEntryNew(FDetailID);
        }

        public string UpdateShipmentNew_FStatus(string FInterID_List, string Status)
        {
            return new PartSelectDAL().UpdateShipmentNew_FStatus(FInterID_List, Status);
        }

        public DataSet GetShipmentNew_One(string FInterID)
        {
            return new PartSelectDAL().GetShipmentNew_One(FInterID);
        }

        public DataSet GetShipmentEntryNew_One(string FDetailID)
        {
            return new PartSelectDAL().GetShipmentEntryNew_One(FDetailID);
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
            return new PartSelectDAL().Edit_CO_WH_ShipmentEntryNew
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
            return new PartSelectDAL().GetORTluPartFamilyType(DetailDefStr, PartFamilyTypeID);
        }

        public string GetORTCode(string PartFamilyTypeID, string YearID, string WeekID)
        {
            return new PartSelectDAL().GetORTCode(PartFamilyTypeID, YearID, WeekID);
        }

        public void InsertORTCodeData(string ORTCode, string PartFamilyTypeID, string YearID, string WeekID)
        {
            new PartSelectDAL().InsertORTCodeData(ORTCode, PartFamilyTypeID, YearID, WeekID);
        }

        public void UpdateORTCodeData(string NewORTCode, string OldORTCode)
        {
            new PartSelectDAL().UpdateORTCodeData(NewORTCode, OldORTCode);
        }

        public DataSet GetORTMaxBatch(string OldORTCode, string TestTypeID)
        {
            return new PartSelectDAL().GetORTMaxBatch(OldORTCode, TestTypeID);
        }

        public DataSet GetORTForCode(string ORTCode)
        {
            return new PartSelectDAL().GetORTForCode(ORTCode);
        }

        public string GetRouteCheck_Diagram(int Scan_StationTypeID, string LineID, DataTable DT_Unit, out string S_OutputStateID, string S_Str)
        {
            return new PartSelectDAL().GetRouteCheck_Diagram(Scan_StationTypeID, LineID, DT_Unit, out S_OutputStateID, S_Str);
        }

        public string AddmesReprintHistory(string PrintType, string SN, string StationID)
        {
            return new PartSelectDAL().AddmesReprintHistory( PrintType,  SN,  StationID);
        }

        public DataSet Get_WHExcel(string S_Start, string S_End, string FStatus)
        {
            return new PartSelectDAL().Get_WHExcel(S_Start, S_End, FStatus);
        }

        public DataSet GetRouteData(string LineID, string PartID, string PartFamilyID,string ProductionOrderID)
        {
            return new PartSelectDAL().GetRouteData( LineID,  PartID,  PartFamilyID, ProductionOrderID);
        }

        public DataSet GetIpad_BB()
        {
            return new PartSelectDAL().GetIpad_BB();
        }

        public string GetFirstStationType(string S_MachineSN)
        {
            return new PartSelectDAL().GetFirstStationType(S_MachineSN);
        }

        public DataSet GetSNToUnit(string S_SN)
        {
            return new PartSelectDAL().GetSNToUnit(S_SN);
        }
        public string uspUpdateBox(string PartID, string ProductionOrderID,  string S_CartonSN, LoginList loginList, string boxWeight)
        {
            return new PartSelectDAL().uspUpdateBox(PartID, ProductionOrderID, S_CartonSN, loginList,boxWeight);
        }

        public DataSet GetMesPartAndPartFamilyDetail(int PartId, string PartDetailDefName,  string PartFamilyDetailDefName, out string result)
        {
            return new PartSelectDAL().GetMesPartAndPartFamilyDetail(PartId, PartDetailDefName, PartFamilyDetailDefName, out result);
        }
        public string GetServerDateTime() => new PartSelectDAL().GetServerDateTime();
        public string GetSampleCount(string startTime, string endTime, bool type, int stationId)
        {
            return new PartSelectDAL().GetSampleCount(startTime, endTime, type, stationId);
        }
        public string GetProductOrder(string barcode, int type)
        {
            return new PartSelectDAL().GetProductOrder(barcode, type);
        }
        public DataSet GetProductDataInfo(string barcode, int type)
        {
            return new PartSelectDAL().GetProductDataInfo(barcode, type);
        }
        public string GetMesStationAndStationTypeDetail(int StationId, string StationDetailDefName, string StationTypeDetailDefName, out string result)
        {
            return new PartSelectDAL().GetMesStationAndStationTypeDetail(StationId, StationDetailDefName, StationTypeDetailDefName, out result);
        }
        public int GetBarcodeType(string barcode)
        {
            return new PartSelectDAL().GetBarcodeType(barcode);
        }
        public DataSet GetAndCheckUnitInfo(string barcode, string POID, string PartID)
        {
            return new PartSelectDAL().GetAndCheckUnitInfo(barcode, POID, PartID);
        }
        public List<string> SnToPOID(string SN)
        {
            return new PartSelectDAL().SnToPOID(SN);
        }
        public string uspUpdateBoxV2(string PartID, string ProductionOrderID, string S_CartonSN, LoginList loginList, string boxWeight)
        {
            return new PartSelectDAL().uspUpdateBoxV2(PartID, ProductionOrderID, S_CartonSN, loginList, boxWeight);
        }
        public string uspPalletPackagingV2(string PartID, string ProductionOrderID, string S_CartonSN, string S_PalletSN, LoginList loginList, int PalletQty, ref int boxCount)
        {
            return new PartSelectDAL().uspPalletPackagingV2(PartID, ProductionOrderID, S_CartonSN, S_PalletSN, loginList, PalletQty, ref boxCount);
        }

        public DataSet GetmesUnitTTBox(string S_SN)
        {
            return new PartSelectDAL().GetmesUnitTTBox(S_SN);
        }

        public string GetAppSet(string S_SetName)
        {
            return new PartSelectDAL().GetAppSet(S_SetName);
        }

        public int InsertmesScreenshot(mesScreenshot model)
        {
            return new PartSelectDAL().InsertmesScreenshot(model);
        }

        public mesScreenshot GetmesScreenshot(int id)
        {
            return new PartSelectDAL().GetmesScreenshot(id);
        }

        public Boolean UpdateScreenshot(mesScreenshot model)
        {
            return new PartSelectDAL().UpdateScreenshot(model);
        }

        public DataSet ListmesScreenshot(string S_Where)
        {
            return new PartSelectDAL().ListmesScreenshot(S_Where);
        }
        public string ShipMentScalesCommint(string PalletSn, LoginList loginList, string PalletWeight, string POID)
        {
            return new PartSelectDAL().ShipMentScalesCommint(PalletSn, loginList, PalletWeight,  POID);
        }
        public string CheckShipmentPalletState(string ShipPallet, string POID)
        {
            return new PartSelectDAL().CheckShipmentPalletState(ShipPallet,POID);
        }
        public void SetupPackageScalesDesc()
        {
             new PartSelectDAL().SetupPackageScalesDesc();
        }

        public string[] GetPartIdByShippingPallet(string ShippingPalletSN)
        {
            return  new PartSelectDAL().GetPartIdByShippingPallet(ShippingPalletSN);
        }

        public void InsertBulkData(SnModel[] lsSnModels)
        {
            new PartSelectDAL().InsertBulkData(lsSnModels);
        }

        public string SaveTTOutputStationType(string SN, int StationTypeID)
        {
            return new PartSelectDAL().SaveTTOutputStationType(SN, StationTypeID);
        }

        public StationTypeShow[] GetTTOutputStationTypeList(int routeID, int stationTypeID)
        {
            return new PartSelectDAL().GetTTOutputStationTypeList(routeID, stationTypeID);
        }
    }
}
