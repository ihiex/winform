﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace App.MyMES.DataCommitService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="DataCommitService.IDataCommitSVC")]
    public interface IDataCommitSVC {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SubmitDataUH", ReplyAction="http://tempuri.org/IDataCommitSVC/SubmitDataUHResponse")]
        string SubmitDataUH(App.Model.mesUnit[] List_mesUnit, App.Model.mesHistory[] List_mesHistory);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SubmitDataUH", ReplyAction="http://tempuri.org/IDataCommitSVC/SubmitDataUHResponse")]
        System.Threading.Tasks.Task<string> SubmitDataUHAsync(App.Model.mesUnit[] List_mesUnit, App.Model.mesHistory[] List_mesHistory);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SubmitDataUHD", ReplyAction="http://tempuri.org/IDataCommitSVC/SubmitDataUHDResponse")]
        string SubmitDataUHD(App.Model.mesUnit[] List_mesUnit, App.Model.mesHistory[] List_mesHistory, App.Model.mesUnitDefect[] List_mesUnitDefect);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SubmitDataUHD", ReplyAction="http://tempuri.org/IDataCommitSVC/SubmitDataUHDResponse")]
        System.Threading.Tasks.Task<string> SubmitDataUHDAsync(App.Model.mesUnit[] List_mesUnit, App.Model.mesHistory[] List_mesHistory, App.Model.mesUnitDefect[] List_mesUnitDefect);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SubmitDataUHCD", ReplyAction="http://tempuri.org/IDataCommitSVC/SubmitDataUHCDResponse")]
        string SubmitDataUHCD(App.Model.mesUnit[] List_mesUnit, App.Model.mesHistory[] List_mesHistory, App.Model.mesUnitComponent[] List_mesUnitComponent, App.Model.mesUnitDefect[] List_mesUnitDefect);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SubmitDataUHCD", ReplyAction="http://tempuri.org/IDataCommitSVC/SubmitDataUHCDResponse")]
        System.Threading.Tasks.Task<string> SubmitDataUHCDAsync(App.Model.mesUnit[] List_mesUnit, App.Model.mesHistory[] List_mesHistory, App.Model.mesUnitComponent[] List_mesUnitComponent, App.Model.mesUnitDefect[] List_mesUnitDefect);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SubmitDataUHC", ReplyAction="http://tempuri.org/IDataCommitSVC/SubmitDataUHCResponse")]
        string SubmitDataUHC(App.Model.mesUnit[] List_mesUnit, App.Model.mesHistory[] List_mesHistory, App.Model.mesUnitComponent[] List_mesUnitComponent, App.Model.mesMaterialConsumeInfo[] List_mesMaterialConsumeInfo, App.Model.mesMachine[] List_mesMachine, App.Model.LoginList F_LoginList);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SubmitDataUHC", ReplyAction="http://tempuri.org/IDataCommitSVC/SubmitDataUHCResponse")]
        System.Threading.Tasks.Task<string> SubmitDataUHCAsync(App.Model.mesUnit[] List_mesUnit, App.Model.mesHistory[] List_mesHistory, App.Model.mesUnitComponent[] List_mesUnitComponent, App.Model.mesMaterialConsumeInfo[] List_mesMaterialConsumeInfo, App.Model.mesMachine[] List_mesMachine, App.Model.LoginList F_LoginList);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/InsertUDS", ReplyAction="http://tempuri.org/IDataCommitSVC/InsertUDSResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(string[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.mesUnit[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.mesUnit))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.mesHistory[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.mesHistory))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.mesUnitDefect[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.mesUnitDefect))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.mesUnitComponent[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.mesUnitComponent))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.mesMaterialConsumeInfo[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.mesMaterialConsumeInfo))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.mesMachine[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.mesMachine))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.LoginList))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.mesUnitDetail))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.mesUnitDetail[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.mesSerialNumber[]))]
        string InsertUDS(App.Model.mesUnit v_mesUnit, App.Model.mesUnitDetail v_mesUnitDetail, App.Model.mesSerialNumber v_mesSerialNumber, string S_ObjSN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/InsertUDS", ReplyAction="http://tempuri.org/IDataCommitSVC/InsertUDSResponse")]
        System.Threading.Tasks.Task<string> InsertUDSAsync(App.Model.mesUnit v_mesUnit, App.Model.mesUnitDetail v_mesUnitDetail, App.Model.mesSerialNumber v_mesSerialNumber, string S_ObjSN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/UpdatemesUnit", ReplyAction="http://tempuri.org/IDataCommitSVC/UpdatemesUnitResponse")]
        string UpdatemesUnit(App.Model.mesUnit v_mesUnit);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/UpdatemesUnit", ReplyAction="http://tempuri.org/IDataCommitSVC/UpdatemesUnitResponse")]
        System.Threading.Tasks.Task<string> UpdatemesUnitAsync(App.Model.mesUnit v_mesUnit);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/InsertALL", ReplyAction="http://tempuri.org/IDataCommitSVC/InsertALLResponse")]
        string InsertALL(App.Model.mesUnit[] List_mesUnit, App.Model.mesUnitDetail[] List_mesUnitDetail, App.Model.mesHistory[] List_mesHistory, App.Model.mesSerialNumber[] List_mesSerialNumber, App.Model.mesUnitComponent[] List_mesUnitComponent, App.Model.mesUnitDefect[] List_mesUnitDefect, App.Model.mesMachine[] List_mesMachine, App.Model.LoginList F_LoginList, string[] L_TLinkT);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/InsertALL", ReplyAction="http://tempuri.org/IDataCommitSVC/InsertALLResponse")]
        System.Threading.Tasks.Task<string> InsertALLAsync(App.Model.mesUnit[] List_mesUnit, App.Model.mesUnitDetail[] List_mesUnitDetail, App.Model.mesHistory[] List_mesHistory, App.Model.mesSerialNumber[] List_mesSerialNumber, App.Model.mesUnitComponent[] List_mesUnitComponent, App.Model.mesUnitDefect[] List_mesUnitDefect, App.Model.mesMachine[] List_mesMachine, App.Model.LoginList F_LoginList, string[] L_TLinkT);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SubmitDataUH_UDetail", ReplyAction="http://tempuri.org/IDataCommitSVC/SubmitDataUH_UDetailResponse")]
        string SubmitDataUH_UDetail(App.Model.mesUnit[] List_mesUnit, App.Model.mesHistory[] List_mesHistory, App.Model.mesUnitDetail[] List_mesUnitDetail);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SubmitDataUH_UDetail", ReplyAction="http://tempuri.org/IDataCommitSVC/SubmitDataUH_UDetailResponse")]
        System.Threading.Tasks.Task<string> SubmitDataUH_UDetailAsync(App.Model.mesUnit[] List_mesUnit, App.Model.mesHistory[] List_mesHistory, App.Model.mesUnitDetail[] List_mesUnitDetail);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/uspPalletPackaging", ReplyAction="http://tempuri.org/IDataCommitSVC/uspPalletPackagingResponse")]
        string uspPalletPackaging(string PartID, string ProductionOrderID, string S_CartonSN, string S_PalletSN, App.Model.LoginList loginList, string S_BillNO, int PalletQty);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/uspPalletPackaging", ReplyAction="http://tempuri.org/IDataCommitSVC/uspPalletPackagingResponse")]
        System.Threading.Tasks.Task<string> uspPalletPackagingAsync(string PartID, string ProductionOrderID, string S_CartonSN, string S_PalletSN, App.Model.LoginList loginList, string S_BillNO, int PalletQty);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/MoveShipment", ReplyAction="http://tempuri.org/IDataCommitSVC/MoveShipmentResponse")]
        string MoveShipment(string S_BillNo, string S_MultipackSN, string S_ProdOrderID, string S_PartID, string S_StationId, string S_EmployeeId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/MoveShipment", ReplyAction="http://tempuri.org/IDataCommitSVC/MoveShipmentResponse")]
        System.Threading.Tasks.Task<string> MoveShipmentAsync(string S_BillNo, string S_MultipackSN, string S_ProdOrderID, string S_PartID, string S_StationId, string S_EmployeeId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SetShipmentMultipack", ReplyAction="http://tempuri.org/IDataCommitSVC/SetShipmentMultipackResponse")]
        System.Data.DataSet SetShipmentMultipack(string S_BillNo, string S_MultipackSN, string S_MultipackPalletSN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SetShipmentMultipack", ReplyAction="http://tempuri.org/IDataCommitSVC/SetShipmentMultipackResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> SetShipmentMultipackAsync(string S_BillNo, string S_MultipackSN, string S_MultipackPalletSN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SetMesPackageShipment", ReplyAction="http://tempuri.org/IDataCommitSVC/SetMesPackageShipmentResponse")]
        string SetMesPackageShipment(string ShipmentDetailID, string SerialNumber, int Type);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SetMesPackageShipment", ReplyAction="http://tempuri.org/IDataCommitSVC/SetMesPackageShipmentResponse")]
        System.Threading.Tasks.Task<string> SetMesPackageShipmentAsync(string ShipmentDetailID, string SerialNumber, int Type);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SetMesPackageShipmentRoll", ReplyAction="http://tempuri.org/IDataCommitSVC/SetMesPackageShipmentRollResponse")]
        string SetMesPackageShipmentRoll(string S_BillNo, string S_MultipackPalletSN, string S_MultipackSN, string S_ShipmentDetailID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SetMesPackageShipmentRoll", ReplyAction="http://tempuri.org/IDataCommitSVC/SetMesPackageShipmentRollResponse")]
        System.Threading.Tasks.Task<string> SetMesPackageShipmentRollAsync(string S_BillNo, string S_MultipackPalletSN, string S_MultipackSN, string S_ShipmentDetailID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/GetShipmentPalletSN", ReplyAction="http://tempuri.org/IDataCommitSVC/GetShipmentPalletSNResponse")]
        string GetShipmentPalletSN(string S_BillNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/GetShipmentPalletSN", ReplyAction="http://tempuri.org/IDataCommitSVC/GetShipmentPalletSNResponse")]
        System.Threading.Tasks.Task<string> GetShipmentPalletSNAsync(string S_BillNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/GetIsOutCountComplete", ReplyAction="http://tempuri.org/IDataCommitSVC/GetIsOutCountCompleteResponse")]
        string GetIsOutCountComplete(string S_BillNo, string S_MultipackPalletSN, string S_ShipmentDetailID, bool B_ScanOver);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/GetIsOutCountComplete", ReplyAction="http://tempuri.org/IDataCommitSVC/GetIsOutCountCompleteResponse")]
        System.Threading.Tasks.Task<string> GetIsOutCountCompleteAsync(string S_BillNo, string S_MultipackPalletSN, string S_ShipmentDetailID, bool B_ScanOver);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/uspPalletPackagingPrintOne", ReplyAction="http://tempuri.org/IDataCommitSVC/uspPalletPackagingPrintOneResponse")]
        string uspPalletPackagingPrintOne(string PartID, string ProductionOrderID, string S_CartonSN, string S_PalletSN, App.Model.LoginList loginList, int PalletQty);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/uspPalletPackagingPrintOne", ReplyAction="http://tempuri.org/IDataCommitSVC/uspPalletPackagingPrintOneResponse")]
        System.Threading.Tasks.Task<string> uspPalletPackagingPrintOneAsync(string PartID, string ProductionOrderID, string S_CartonSN, string S_PalletSN, App.Model.LoginList loginList, int PalletQty);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/MoveShipmentPrintOne", ReplyAction="http://tempuri.org/IDataCommitSVC/MoveShipmentPrintOneResponse")]
        string MoveShipmentPrintOne(string S_BillNo, string S_MultipackSN, string S_ProdOrderID, string S_PartID, string S_StationId, string S_EmployeeId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/MoveShipmentPrintOne", ReplyAction="http://tempuri.org/IDataCommitSVC/MoveShipmentPrintOneResponse")]
        System.Threading.Tasks.Task<string> MoveShipmentPrintOneAsync(string S_BillNo, string S_MultipackSN, string S_ProdOrderID, string S_PartID, string S_StationId, string S_EmployeeId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SetShipmentMultipackPrintOne", ReplyAction="http://tempuri.org/IDataCommitSVC/SetShipmentMultipackPrintOneResponse")]
        System.Data.DataSet SetShipmentMultipackPrintOne(string S_BillNo, string S_MultipackSN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SetShipmentMultipackPrintOne", ReplyAction="http://tempuri.org/IDataCommitSVC/SetShipmentMultipackPrintOneResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> SetShipmentMultipackPrintOneAsync(string S_BillNo, string S_MultipackSN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SetMesPackageShipmentPrintOne", ReplyAction="http://tempuri.org/IDataCommitSVC/SetMesPackageShipmentPrintOneResponse")]
        string SetMesPackageShipmentPrintOne(string ShipmentDetailID, string SerialNumber, int Type);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SetMesPackageShipmentPrintOne", ReplyAction="http://tempuri.org/IDataCommitSVC/SetMesPackageShipmentPrintOneResponse")]
        System.Threading.Tasks.Task<string> SetMesPackageShipmentPrintOneAsync(string ShipmentDetailID, string SerialNumber, int Type);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SetMesPackageShipmentRollPrintOne", ReplyAction="http://tempuri.org/IDataCommitSVC/SetMesPackageShipmentRollPrintOneResponse")]
        string SetMesPackageShipmentRollPrintOne(string S_BillNo, string S_MultipackSN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SetMesPackageShipmentRollPrintOne", ReplyAction="http://tempuri.org/IDataCommitSVC/SetMesPackageShipmentRollPrintOneResponse")]
        System.Threading.Tasks.Task<string> SetMesPackageShipmentRollPrintOneAsync(string S_BillNo, string S_MultipackSN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SetCancelInWH", ReplyAction="http://tempuri.org/IDataCommitSVC/SetCancelInWHResponse")]
        string SetCancelInWH(string S_BoxSN, string S_ProdID, string S_PartID, string S_StationID, string S_EmployeeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SetCancelInWH", ReplyAction="http://tempuri.org/IDataCommitSVC/SetCancelInWHResponse")]
        System.Threading.Tasks.Task<string> SetCancelInWHAsync(string S_BoxSN, string S_ProdID, string S_PartID, string S_StationID, string S_EmployeeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SetCancelInWHEntry", ReplyAction="http://tempuri.org/IDataCommitSVC/SetCancelInWHEntryResponse")]
        string SetCancelInWHEntry(string S_BoxSN, string S_ProdID, string S_PartID, string S_StationID, string S_EmployeeID, string S_ReturnToStationTypeID, string S_ReturnStatus);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataCommitSVC/SetCancelInWHEntry", ReplyAction="http://tempuri.org/IDataCommitSVC/SetCancelInWHEntryResponse")]
        System.Threading.Tasks.Task<string> SetCancelInWHEntryAsync(string S_BoxSN, string S_ProdID, string S_PartID, string S_StationID, string S_EmployeeID, string S_ReturnToStationTypeID, string S_ReturnStatus);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IDataCommitSVCChannel : App.MyMES.DataCommitService.IDataCommitSVC, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DataCommitSVCClient : System.ServiceModel.ClientBase<App.MyMES.DataCommitService.IDataCommitSVC>, App.MyMES.DataCommitService.IDataCommitSVC {
        
        public DataCommitSVCClient() {
        }
        
        public DataCommitSVCClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public DataCommitSVCClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DataCommitSVCClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DataCommitSVCClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string SubmitDataUH(App.Model.mesUnit[] List_mesUnit, App.Model.mesHistory[] List_mesHistory) {
            return base.Channel.SubmitDataUH(List_mesUnit, List_mesHistory);
        }
        
        public System.Threading.Tasks.Task<string> SubmitDataUHAsync(App.Model.mesUnit[] List_mesUnit, App.Model.mesHistory[] List_mesHistory) {
            return base.Channel.SubmitDataUHAsync(List_mesUnit, List_mesHistory);
        }
        
        public string SubmitDataUHD(App.Model.mesUnit[] List_mesUnit, App.Model.mesHistory[] List_mesHistory, App.Model.mesUnitDefect[] List_mesUnitDefect) {
            return base.Channel.SubmitDataUHD(List_mesUnit, List_mesHistory, List_mesUnitDefect);
        }
        
        public System.Threading.Tasks.Task<string> SubmitDataUHDAsync(App.Model.mesUnit[] List_mesUnit, App.Model.mesHistory[] List_mesHistory, App.Model.mesUnitDefect[] List_mesUnitDefect) {
            return base.Channel.SubmitDataUHDAsync(List_mesUnit, List_mesHistory, List_mesUnitDefect);
        }
        
        public string SubmitDataUHCD(App.Model.mesUnit[] List_mesUnit, App.Model.mesHistory[] List_mesHistory, App.Model.mesUnitComponent[] List_mesUnitComponent, App.Model.mesUnitDefect[] List_mesUnitDefect) {
            return base.Channel.SubmitDataUHCD(List_mesUnit, List_mesHistory, List_mesUnitComponent, List_mesUnitDefect);
        }
        
        public System.Threading.Tasks.Task<string> SubmitDataUHCDAsync(App.Model.mesUnit[] List_mesUnit, App.Model.mesHistory[] List_mesHistory, App.Model.mesUnitComponent[] List_mesUnitComponent, App.Model.mesUnitDefect[] List_mesUnitDefect) {
            return base.Channel.SubmitDataUHCDAsync(List_mesUnit, List_mesHistory, List_mesUnitComponent, List_mesUnitDefect);
        }
        
        public string SubmitDataUHC(App.Model.mesUnit[] List_mesUnit, App.Model.mesHistory[] List_mesHistory, App.Model.mesUnitComponent[] List_mesUnitComponent, App.Model.mesMaterialConsumeInfo[] List_mesMaterialConsumeInfo, App.Model.mesMachine[] List_mesMachine, App.Model.LoginList F_LoginList) {
            return base.Channel.SubmitDataUHC(List_mesUnit, List_mesHistory, List_mesUnitComponent, List_mesMaterialConsumeInfo, List_mesMachine, F_LoginList);
        }
        
        public System.Threading.Tasks.Task<string> SubmitDataUHCAsync(App.Model.mesUnit[] List_mesUnit, App.Model.mesHistory[] List_mesHistory, App.Model.mesUnitComponent[] List_mesUnitComponent, App.Model.mesMaterialConsumeInfo[] List_mesMaterialConsumeInfo, App.Model.mesMachine[] List_mesMachine, App.Model.LoginList F_LoginList) {
            return base.Channel.SubmitDataUHCAsync(List_mesUnit, List_mesHistory, List_mesUnitComponent, List_mesMaterialConsumeInfo, List_mesMachine, F_LoginList);
        }
        
        public string InsertUDS(App.Model.mesUnit v_mesUnit, App.Model.mesUnitDetail v_mesUnitDetail, App.Model.mesSerialNumber v_mesSerialNumber, string S_ObjSN) {
            return base.Channel.InsertUDS(v_mesUnit, v_mesUnitDetail, v_mesSerialNumber, S_ObjSN);
        }
        
        public System.Threading.Tasks.Task<string> InsertUDSAsync(App.Model.mesUnit v_mesUnit, App.Model.mesUnitDetail v_mesUnitDetail, App.Model.mesSerialNumber v_mesSerialNumber, string S_ObjSN) {
            return base.Channel.InsertUDSAsync(v_mesUnit, v_mesUnitDetail, v_mesSerialNumber, S_ObjSN);
        }
        
        public string UpdatemesUnit(App.Model.mesUnit v_mesUnit) {
            return base.Channel.UpdatemesUnit(v_mesUnit);
        }
        
        public System.Threading.Tasks.Task<string> UpdatemesUnitAsync(App.Model.mesUnit v_mesUnit) {
            return base.Channel.UpdatemesUnitAsync(v_mesUnit);
        }
        
        public string InsertALL(App.Model.mesUnit[] List_mesUnit, App.Model.mesUnitDetail[] List_mesUnitDetail, App.Model.mesHistory[] List_mesHistory, App.Model.mesSerialNumber[] List_mesSerialNumber, App.Model.mesUnitComponent[] List_mesUnitComponent, App.Model.mesUnitDefect[] List_mesUnitDefect, App.Model.mesMachine[] List_mesMachine, App.Model.LoginList F_LoginList, string[] L_TLinkT) {
            return base.Channel.InsertALL(List_mesUnit, List_mesUnitDetail, List_mesHistory, List_mesSerialNumber, List_mesUnitComponent, List_mesUnitDefect, List_mesMachine, F_LoginList, L_TLinkT);
        }
        
        public System.Threading.Tasks.Task<string> InsertALLAsync(App.Model.mesUnit[] List_mesUnit, App.Model.mesUnitDetail[] List_mesUnitDetail, App.Model.mesHistory[] List_mesHistory, App.Model.mesSerialNumber[] List_mesSerialNumber, App.Model.mesUnitComponent[] List_mesUnitComponent, App.Model.mesUnitDefect[] List_mesUnitDefect, App.Model.mesMachine[] List_mesMachine, App.Model.LoginList F_LoginList, string[] L_TLinkT) {
            return base.Channel.InsertALLAsync(List_mesUnit, List_mesUnitDetail, List_mesHistory, List_mesSerialNumber, List_mesUnitComponent, List_mesUnitDefect, List_mesMachine, F_LoginList, L_TLinkT);
        }
        
        public string SubmitDataUH_UDetail(App.Model.mesUnit[] List_mesUnit, App.Model.mesHistory[] List_mesHistory, App.Model.mesUnitDetail[] List_mesUnitDetail) {
            return base.Channel.SubmitDataUH_UDetail(List_mesUnit, List_mesHistory, List_mesUnitDetail);
        }
        
        public System.Threading.Tasks.Task<string> SubmitDataUH_UDetailAsync(App.Model.mesUnit[] List_mesUnit, App.Model.mesHistory[] List_mesHistory, App.Model.mesUnitDetail[] List_mesUnitDetail) {
            return base.Channel.SubmitDataUH_UDetailAsync(List_mesUnit, List_mesHistory, List_mesUnitDetail);
        }
        
        public string uspPalletPackaging(string PartID, string ProductionOrderID, string S_CartonSN, string S_PalletSN, App.Model.LoginList loginList, string S_BillNO, int PalletQty) {
            return base.Channel.uspPalletPackaging(PartID, ProductionOrderID, S_CartonSN, S_PalletSN, loginList, S_BillNO, PalletQty);
        }
        
        public System.Threading.Tasks.Task<string> uspPalletPackagingAsync(string PartID, string ProductionOrderID, string S_CartonSN, string S_PalletSN, App.Model.LoginList loginList, string S_BillNO, int PalletQty) {
            return base.Channel.uspPalletPackagingAsync(PartID, ProductionOrderID, S_CartonSN, S_PalletSN, loginList, S_BillNO, PalletQty);
        }
        
        public string MoveShipment(string S_BillNo, string S_MultipackSN, string S_ProdOrderID, string S_PartID, string S_StationId, string S_EmployeeId) {
            return base.Channel.MoveShipment(S_BillNo, S_MultipackSN, S_ProdOrderID, S_PartID, S_StationId, S_EmployeeId);
        }
        
        public System.Threading.Tasks.Task<string> MoveShipmentAsync(string S_BillNo, string S_MultipackSN, string S_ProdOrderID, string S_PartID, string S_StationId, string S_EmployeeId) {
            return base.Channel.MoveShipmentAsync(S_BillNo, S_MultipackSN, S_ProdOrderID, S_PartID, S_StationId, S_EmployeeId);
        }
        
        public System.Data.DataSet SetShipmentMultipack(string S_BillNo, string S_MultipackSN, string S_MultipackPalletSN) {
            return base.Channel.SetShipmentMultipack(S_BillNo, S_MultipackSN, S_MultipackPalletSN);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> SetShipmentMultipackAsync(string S_BillNo, string S_MultipackSN, string S_MultipackPalletSN) {
            return base.Channel.SetShipmentMultipackAsync(S_BillNo, S_MultipackSN, S_MultipackPalletSN);
        }
        
        public string SetMesPackageShipment(string ShipmentDetailID, string SerialNumber, int Type) {
            return base.Channel.SetMesPackageShipment(ShipmentDetailID, SerialNumber, Type);
        }
        
        public System.Threading.Tasks.Task<string> SetMesPackageShipmentAsync(string ShipmentDetailID, string SerialNumber, int Type) {
            return base.Channel.SetMesPackageShipmentAsync(ShipmentDetailID, SerialNumber, Type);
        }
        
        public string SetMesPackageShipmentRoll(string S_BillNo, string S_MultipackPalletSN, string S_MultipackSN, string S_ShipmentDetailID) {
            return base.Channel.SetMesPackageShipmentRoll(S_BillNo, S_MultipackPalletSN, S_MultipackSN, S_ShipmentDetailID);
        }
        
        public System.Threading.Tasks.Task<string> SetMesPackageShipmentRollAsync(string S_BillNo, string S_MultipackPalletSN, string S_MultipackSN, string S_ShipmentDetailID) {
            return base.Channel.SetMesPackageShipmentRollAsync(S_BillNo, S_MultipackPalletSN, S_MultipackSN, S_ShipmentDetailID);
        }
        
        public string GetShipmentPalletSN(string S_BillNo) {
            return base.Channel.GetShipmentPalletSN(S_BillNo);
        }
        
        public System.Threading.Tasks.Task<string> GetShipmentPalletSNAsync(string S_BillNo) {
            return base.Channel.GetShipmentPalletSNAsync(S_BillNo);
        }
        
        public string GetIsOutCountComplete(string S_BillNo, string S_MultipackPalletSN, string S_ShipmentDetailID, bool B_ScanOver) {
            return base.Channel.GetIsOutCountComplete(S_BillNo, S_MultipackPalletSN, S_ShipmentDetailID, B_ScanOver);
        }
        
        public System.Threading.Tasks.Task<string> GetIsOutCountCompleteAsync(string S_BillNo, string S_MultipackPalletSN, string S_ShipmentDetailID, bool B_ScanOver) {
            return base.Channel.GetIsOutCountCompleteAsync(S_BillNo, S_MultipackPalletSN, S_ShipmentDetailID, B_ScanOver);
        }
        
        public string uspPalletPackagingPrintOne(string PartID, string ProductionOrderID, string S_CartonSN, string S_PalletSN, App.Model.LoginList loginList, int PalletQty) {
            return base.Channel.uspPalletPackagingPrintOne(PartID, ProductionOrderID, S_CartonSN, S_PalletSN, loginList, PalletQty);
        }
        
        public System.Threading.Tasks.Task<string> uspPalletPackagingPrintOneAsync(string PartID, string ProductionOrderID, string S_CartonSN, string S_PalletSN, App.Model.LoginList loginList, int PalletQty) {
            return base.Channel.uspPalletPackagingPrintOneAsync(PartID, ProductionOrderID, S_CartonSN, S_PalletSN, loginList, PalletQty);
        }
        
        public string MoveShipmentPrintOne(string S_BillNo, string S_MultipackSN, string S_ProdOrderID, string S_PartID, string S_StationId, string S_EmployeeId) {
            return base.Channel.MoveShipmentPrintOne(S_BillNo, S_MultipackSN, S_ProdOrderID, S_PartID, S_StationId, S_EmployeeId);
        }
        
        public System.Threading.Tasks.Task<string> MoveShipmentPrintOneAsync(string S_BillNo, string S_MultipackSN, string S_ProdOrderID, string S_PartID, string S_StationId, string S_EmployeeId) {
            return base.Channel.MoveShipmentPrintOneAsync(S_BillNo, S_MultipackSN, S_ProdOrderID, S_PartID, S_StationId, S_EmployeeId);
        }
        
        public System.Data.DataSet SetShipmentMultipackPrintOne(string S_BillNo, string S_MultipackSN) {
            return base.Channel.SetShipmentMultipackPrintOne(S_BillNo, S_MultipackSN);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> SetShipmentMultipackPrintOneAsync(string S_BillNo, string S_MultipackSN) {
            return base.Channel.SetShipmentMultipackPrintOneAsync(S_BillNo, S_MultipackSN);
        }
        
        public string SetMesPackageShipmentPrintOne(string ShipmentDetailID, string SerialNumber, int Type) {
            return base.Channel.SetMesPackageShipmentPrintOne(ShipmentDetailID, SerialNumber, Type);
        }
        
        public System.Threading.Tasks.Task<string> SetMesPackageShipmentPrintOneAsync(string ShipmentDetailID, string SerialNumber, int Type) {
            return base.Channel.SetMesPackageShipmentPrintOneAsync(ShipmentDetailID, SerialNumber, Type);
        }
        
        public string SetMesPackageShipmentRollPrintOne(string S_BillNo, string S_MultipackSN) {
            return base.Channel.SetMesPackageShipmentRollPrintOne(S_BillNo, S_MultipackSN);
        }
        
        public System.Threading.Tasks.Task<string> SetMesPackageShipmentRollPrintOneAsync(string S_BillNo, string S_MultipackSN) {
            return base.Channel.SetMesPackageShipmentRollPrintOneAsync(S_BillNo, S_MultipackSN);
        }
        
        public string SetCancelInWH(string S_BoxSN, string S_ProdID, string S_PartID, string S_StationID, string S_EmployeeID) {
            return base.Channel.SetCancelInWH(S_BoxSN, S_ProdID, S_PartID, S_StationID, S_EmployeeID);
        }
        
        public System.Threading.Tasks.Task<string> SetCancelInWHAsync(string S_BoxSN, string S_ProdID, string S_PartID, string S_StationID, string S_EmployeeID) {
            return base.Channel.SetCancelInWHAsync(S_BoxSN, S_ProdID, S_PartID, S_StationID, S_EmployeeID);
        }
        
        public string SetCancelInWHEntry(string S_BoxSN, string S_ProdID, string S_PartID, string S_StationID, string S_EmployeeID, string S_ReturnToStationTypeID, string S_ReturnStatus) {
            return base.Channel.SetCancelInWHEntry(S_BoxSN, S_ProdID, S_PartID, S_StationID, S_EmployeeID, S_ReturnToStationTypeID, S_ReturnStatus);
        }
        
        public System.Threading.Tasks.Task<string> SetCancelInWHEntryAsync(string S_BoxSN, string S_ProdID, string S_PartID, string S_StationID, string S_EmployeeID, string S_ReturnToStationTypeID, string S_ReturnStatus) {
            return base.Channel.SetCancelInWHEntryAsync(S_BoxSN, S_ProdID, S_PartID, S_StationID, S_EmployeeID, S_ReturnToStationTypeID, S_ReturnStatus);
        }
    }
}