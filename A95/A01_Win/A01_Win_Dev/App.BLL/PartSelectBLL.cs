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

        public string GetServerIP()
        {
            return new PartSelectDAL().GetServerIP();
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
            return new PartSelectDAL().GetmesPartDetail( PartID, PartDetailDefName);
        }

        public DataSet GetluPODetailDef(int ProductionOrderID, string PODetailDef)
        {
            return new PartSelectDAL().GetluPODetailDef(ProductionOrderID, PODetailDef);
        }

        public DataSet GetmesPartPrint()
        {
            return new PartSelectDAL().GetmesPartPrint();
        }

        public string mesGetSNFormatIDByList(object PartID, object PartFamilyID, object LineID)
        {
            return new PartSelectDAL().mesGetSNFormatIDByList(PartID, PartFamilyID, LineID);
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
            return new PartSelectDAL().GetRouteSequence(RouteID,StationTypeID);
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
            return new PartSelectDAL().GetUnit_Search( S_DateStart,  S_DateEnd,  S_Where);
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
            return new PartSelectDAL().GetMachineHistory( S_UnitID);
        }

        public DataSet GetmesUnitState(string S_PartID, string S_RouteSequence,string LineID,int StationTypeID)
        {
            return new PartSelectDAL().GetmesUnitState(S_PartID, S_RouteSequence, LineID, StationTypeID);
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
            return new PartSelectDAL().GetPOAll( S_PartID,  S_LineID);
        }

        public DataSet GetRoute(string PartID, string S_RouteSequence, string LineID,int StationTypeID)
        {
            return new PartSelectDAL().GetRoute(PartID,S_RouteSequence, LineID, StationTypeID);
        }

        public DataSet GetApplicationType(string StationTypeID)
        {
            return new PartSelectDAL().GetApplicationType(StationTypeID);
        }
        public DataSet GetluDefect(int DefectTypeID)
        {
            return new PartSelectDAL().GetluDefect(DefectTypeID);
        }

        public DataSet  GetluUnitStatus()
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

        public DataSet  GetPartDetailDef(string SN)
        {
            return new PartSelectDAL().GetPartDetailDef(SN);
        }

        public string GetRouteCheck(int Scan_StationTypeID, int Scan_StationID,string LineID, DataTable DT_Unit, string S_Str)
        {
            return new PartSelectDAL().GetRouteCheck(Scan_StationTypeID, Scan_StationID, LineID,DT_Unit, S_Str);
        }

        public DataSet Get_UnitID(string S_SN)
        {
            return new PartSelectDAL().Get_UnitID(S_SN);
        }

        public string Get_CreateMesSN(string strSNFormat, string xmlProdOrder, string xmlPart, string xmlStation, string xmlExtraData, mesUnit v_mesUnit, int number, ref DataSet dsSN)
        {
            return new PartSelectDAL().Get_CreateMesSN(strSNFormat, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, v_mesUnit, number, ref dsSN);
        }

        public DataSet BoxSnToBatch(string S_BoxSN, out string S_Result)
        {
            S_Result = "";
            return new PartSelectDAL().BoxSnToBatch(S_BoxSN,out S_Result);
        }

        public int GetComponentCount(string ParentPartID, string StationTypeID)
        {
            return new PartSelectDAL().GetComponentCount(ParentPartID, StationTypeID);
        }

        public DataSet GetmesUnitComponent(string UnitID, string ChildUnitID)
        {
            return new PartSelectDAL().GetmesUnitComponent(UnitID, ChildUnitID);
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
            new PartSelectDAL().ModMachine(S_SN, StatusID,IsUpUnitDetail);
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

        public DataSet GetLabelName(string StationTypeID, string PartFamilyID, string PartID, string ProductionOrderID, string LineID)
        {
            return new PartSelectDAL().GetLabelName( StationTypeID,  PartFamilyID,  PartID,  ProductionOrderID,  LineID);
        }

        public string BuckToFGSN(string S_BuckSN)
        {
            return new PartSelectDAL().BuckToFGSN(S_BuckSN);
        }

        public DataSet GetPLCSeting(string S_SetName, string S_StationID)
        {
            return new PartSelectDAL().GetPLCSeting(S_SetName, S_StationID);
        }

        public string TimeCheck(string StationTypeID, string S_SN)
        {
            return new PartSelectDAL().TimeCheck(StationTypeID, S_SN);
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

        public string uspKitBoxPackaging(string PartID, string ProductionOrderID ,string S_UPCSN, string S_CartonSN, LoginList LoginList, int BoxQty = 0)
        {
            return new PartSelectDAL().uspKitBoxPackaging(PartID, ProductionOrderID ,S_UPCSN, S_CartonSN, LoginList, BoxQty);
        }

        public string uspPalletCheck(string S_FormatName, string xmlProdOrder, string xmlPart, string xmlStation, string xmlExtraData, string strSNbuf)
        {
            return new PartSelectDAL().uspPalletCheck(S_FormatName, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, strSNbuf);
        }

        public string uspPalletPackaging(string PartID, string ProductionOrderID ,string S_CartonSN, string S_PalletSN, LoginList loginList, int PalletQty = 0)
        {
            return new PartSelectDAL().uspPalletPackaging(PartID, ProductionOrderID ,S_CartonSN, S_PalletSN, loginList, PalletQty);
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

        public DataSet MESGetBomPartInfo(int ParentPartID,int StationTypeID)
        {
            return new PartSelectDAL().MESGetBomPartInfo(ParentPartID, StationTypeID);
        }

        public string MESAssembleCheckMianSN(string ProductionOrderID, int LineID, int StationID, int StationTypeID, string SN)
        {
            return new PartSelectDAL().MESAssembleCheckMianSN(ProductionOrderID, LineID, StationID, StationTypeID, SN);
        }

        public string MESAssembleCheckOtherSN(string SN, string PartID)
        {
            return new PartSelectDAL().MESAssembleCheckOtherSN(SN, PartID);
        }

        public string MESAssembleCheckVirtualSN(string SN, string PartID , string Status)
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


    }
}
