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
        string mesGetSNFormatIDByList(object PartID, object PartFamilyID, object LineID);

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
        DataSet GetmesUnitState(string S_PartID, string S_RouteSequence,string LineID,int StationTypeID);

        [OperationContract]
        DataSet GetmesSerialNumberOfLine(string SNCategory, string PrintCount);

        [OperationContract]
        DataSet GetmesSerialNumberByUnitID(string UnitID);

        [OperationContract]
        DataSet GetPO(string S_PartID, string S_StatusID);
        [OperationContract]
        DataSet GetPOAll(string S_PartID, string S_LineID);

        [OperationContract]
        DataSet GetRoute(string PartID, string S_RouteSequence, string LineID,int StationTypeID);

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
        string Get_CreateMesSN(string strSNFormat, string xmlProdOrder, string xmlPart, string xmlStation, string xmlExtraData, mesUnit v_mesUnit, int number, ref DataSet dsSN);
        [OperationContract]
        DataSet BoxSnToBatch(string S_BoxSN, out string S_Result);

        [OperationContract]
        int GetComponentCount(string ParentPartID, string StationTypeID);
        [OperationContract]
        DataSet GetmesUnitComponent(string UnitID, string ChildUnitID);
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
    DataSet GetLBLGenLabel(string S_SN, string S_LabelName);
    [OperationContract]
    DataSet GetLabelName(string StationTypeID, string PartFamilyID, string PartID, string ProductionOrderID, string LineID);

    [OperationContract]
    string BuckToFGSN(string S_BuckSN);
    [OperationContract]
    DataSet GetPLCSeting(string S_SetName, string S_StationID);

    [OperationContract]
    string TimeCheck(string StationTypeID, string S_SN);


    [OperationContract]
    DataSet GetPartParameter(string PartID);

    [OperationContract]
    string GetluDefectType(string Description);

    [OperationContract]
    string uspKitBoxCheck(string S_FormatName, string xmlProdOrder, string xmlPart, string xmlStation, string xmlExtraData, string strSNbuf);

    [OperationContract]
    string uspKitBoxPackaging(string PartID, string ProductionOrderID, string S_UPCSN, string S_CartonSN, LoginList LoginList, int BoxQty = 0);

    [OperationContract]
    string uspPalletCheck(string S_FormatName, string xmlProdOrder, string xmlPart, string xmlStation, string xmlExtraData, string strSNbuf);

    [OperationContract]
    string uspPalletPackaging(string PartID, string ProductionOrderID, string S_CartonSN, string S_PalletSN, LoginList loginList, int PalletQty = 0);

    [OperationContract]
    DataSet GetProductionOrderDetailDef(string ProductionOrderNumber);

    [OperationContract]
    string Get_CreatePackageSN(string S_FormatName, string xmlProdOrder, string xmlPart, string xmlStation,
                                    string xmlExtraData, mesUnit v_mesUnit, ref string strSN, int type);

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
    string MESAssembleCheckMianSN(string ProductionOrderID, int LineID, int StationID, int StationTypeID, string SN);

    [OperationContract]
    string MESAssembleCheckOtherSN(string SN, string PartID);

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

}
