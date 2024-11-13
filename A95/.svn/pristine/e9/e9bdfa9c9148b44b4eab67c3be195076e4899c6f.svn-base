using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using App.BLL;
using App.Model;
using System.Data;


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

        public string mesGetSNFormatIDByList(object PartID, object PartFamilyID, object LineID)
        {
            return F_BLL.mesGetSNFormatIDByList(PartID, PartFamilyID, LineID);
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

        public DataSet GetmesStationTypeByStationID(string StationID)
        {
            return F_BLL.GetmesStationTypeByStationID(StationID);
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

        public DataSet GetmesUnitState(string S_PartID, string S_RouteSequence,string LineID,int StationTypeID)
        {
            return F_BLL.GetmesUnitState(S_PartID, S_RouteSequence, LineID, StationTypeID);
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

        public DataSet GetRoute(string PartID, string S_RouteSequence,string LineID,int StationTypeID)
        {
            return F_BLL.GetRoute(PartID, S_RouteSequence, LineID, StationTypeID);
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

        public string GetRouteCheck(int Scan_StationTypeID, int Scan_StationID,string LineID, DataTable DT_Unit, string S_Str)
        {
            return F_BLL.GetRouteCheck(Scan_StationTypeID, Scan_StationID, LineID, DT_Unit, S_Str);
        }

        public DataSet Get_UnitID(string S_SN)
        {
            return F_BLL.Get_UnitID(S_SN);
        }

        public string Get_CreateMesSN(string strSNFormat, string xmlProdOrder, string xmlPart, string xmlStation, string xmlExtraData, mesUnit v_mesUnit, int number, ref DataSet dsSN)
        {
            return F_BLL.Get_CreateMesSN(strSNFormat, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, v_mesUnit, number, ref dsSN);
        }

        public DataSet BoxSnToBatch(string S_BoxSN, out string S_Result)
        {
            return F_BLL.BoxSnToBatch(S_BoxSN,out S_Result);
        }

        public int GetComponentCount(string ParentPartID, string StationTypeID)
        {
            return F_BLL.GetComponentCount(ParentPartID, StationTypeID);
        }

        public DataSet GetmesUnitComponent(string UnitID, string ChildUnitID)
        {
            return F_BLL.GetmesUnitComponent(UnitID, ChildUnitID);
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
            F_BLL.ModMachine(S_SN, StatusID,IsUpUnitDetail);
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

    public DataSet GetLabelName(string StationTypeID, string PartFamilyID, string PartID, string ProductionOrderID, string LineID)
    {
        return F_BLL.GetLabelName(StationTypeID, PartFamilyID, PartID, ProductionOrderID, LineID);
    }

    public string BuckToFGSN(string S_BuckSN)
    {
        return F_BLL.BuckToFGSN(S_BuckSN);
    }

    public DataSet GetPLCSeting(string S_SetName, string S_StationID)
    {
        return F_BLL.GetPLCSeting(S_SetName, S_StationID);
    }

    public string TimeCheck(string StationTypeID, string S_SN)
    {
        return F_BLL.TimeCheck(StationTypeID, S_SN);
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
        return F_BLL.uspKitBoxPackaging(PartID, ProductionOrderID ,S_UPCSN, S_CartonSN, LoginList, BoxQty);
    }

    public string uspPalletCheck(string S_FormatName, string xmlProdOrder, string xmlPart, string xmlStation, string xmlExtraData, string strSNbuf)
    {
        return F_BLL.uspPalletCheck(S_FormatName, xmlProdOrder, xmlPart, xmlStation, xmlExtraData, strSNbuf);
    }

    public string uspPalletPackaging(string PartID, string ProductionOrderID, string S_CartonSN, string S_PalletSN, LoginList loginList, int PalletQty = 0)
    {
        return F_BLL.uspPalletPackaging(PartID, ProductionOrderID, S_CartonSN, S_PalletSN, loginList, PalletQty);
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

    public string MESAssembleCheckMianSN(string ProductionOrderID, int LineID, int StationID, int StationTypeID, string SN)
    {
        return F_BLL.MESAssembleCheckMianSN(ProductionOrderID, LineID, StationID, StationTypeID, SN);
    }

    public string MESAssembleCheckOtherSN(string SN, string PartID)
    {
        return F_BLL.MESAssembleCheckOtherSN(SN, PartID);
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
}
