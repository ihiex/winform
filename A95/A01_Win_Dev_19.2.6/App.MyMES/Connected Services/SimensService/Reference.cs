﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace App.MyMES.SimensService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SimensService.ISiemensSVC")]
    public interface ISiemensSVC {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/WHIn", ReplyAction="http://tempuri.org/ISiemensSVC/WHInResponse")]
        string WHIn(string S_MPN, string S_BoxSN, string S_Type, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/WHIn", ReplyAction="http://tempuri.org/ISiemensSVC/WHInResponse")]
        System.Threading.Tasks.Task<string> WHInAsync(string S_MPN, string S_BoxSN, string S_Type, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/WHIn_DT", ReplyAction="http://tempuri.org/ISiemensSVC/WHIn_DTResponse")]
        System.Data.DataSet WHIn_DT(string S_StationTypeID, string S_BoxSN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/WHIn_DT", ReplyAction="http://tempuri.org/ISiemensSVC/WHIn_DTResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> WHIn_DTAsync(string S_StationTypeID, string S_BoxSN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/WHOut", ReplyAction="http://tempuri.org/ISiemensSVC/WHOutResponse")]
        string WHOut(string S_MPN, string S_BillNo, string S_BoxSN, string S_Type, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/WHOut", ReplyAction="http://tempuri.org/ISiemensSVC/WHOutResponse")]
        System.Threading.Tasks.Task<string> WHOutAsync(string S_MPN, string S_BillNo, string S_BoxSN, string S_Type, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/WHOut_DT", ReplyAction="http://tempuri.org/ISiemensSVC/WHOut_DTResponse")]
        System.Data.DataSet WHOut_DT(string S_StationTypeID, string S_BoxSN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/WHOut_DT", ReplyAction="http://tempuri.org/ISiemensSVC/WHOut_DTResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> WHOut_DTAsync(string S_StationTypeID, string S_BoxSN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/GetIpad_BB", ReplyAction="http://tempuri.org/ISiemensSVC/GetIpad_BBResponse")]
        System.Data.DataSet GetIpad_BB();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/GetIpad_BB", ReplyAction="http://tempuri.org/ISiemensSVC/GetIpad_BBResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetIpad_BBAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/CheckBillNo", ReplyAction="http://tempuri.org/ISiemensSVC/CheckBillNoResponse")]
        App.MyMES.SimensService.CheckBillNoResponse CheckBillNo(App.MyMES.SimensService.CheckBillNoRequest request);
        
        // CODEGEN: 正在生成消息协定，应为该操作具有多个返回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/CheckBillNo", ReplyAction="http://tempuri.org/ISiemensSVC/CheckBillNoResponse")]
        System.Threading.Tasks.Task<App.MyMES.SimensService.CheckBillNoResponse> CheckBillNoAsync(App.MyMES.SimensService.CheckBillNoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/GetShipment", ReplyAction="http://tempuri.org/ISiemensSVC/GetShipmentResponse")]
        System.Data.DataSet GetShipment(string S_Start, string S_End, string FStatus, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/GetShipment", ReplyAction="http://tempuri.org/ISiemensSVC/GetShipmentResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetShipmentAsync(string S_Start, string S_End, string FStatus, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/GetShipmentEntry", ReplyAction="http://tempuri.org/ISiemensSVC/GetShipmentEntryResponse")]
        System.Data.DataSet GetShipmentEntry(string S_FInterID, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/GetShipmentEntry", ReplyAction="http://tempuri.org/ISiemensSVC/GetShipmentEntryResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetShipmentEntryAsync(string S_FInterID, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/GetShipmentReport", ReplyAction="http://tempuri.org/ISiemensSVC/GetShipmentReportResponse")]
        System.Data.DataSet GetShipmentReport(string S_Start, string S_End, string FStatus, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/GetShipmentReport", ReplyAction="http://tempuri.org/ISiemensSVC/GetShipmentReportResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetShipmentReportAsync(string S_Start, string S_End, string FStatus, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/Edit_CO_WH_Shipment", ReplyAction="http://tempuri.org/ISiemensSVC/Edit_CO_WH_ShipmentResponse")]
        string Edit_CO_WH_Shipment(string S_FInterID, string S_ShipDate, string S_HAWB, string S_PalletSeq, string S_PalletCount, string S_GrossWeight, string S_EmptyCarton, string S_ShipNO, string S_Project, string S_Type, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/Edit_CO_WH_Shipment", ReplyAction="http://tempuri.org/ISiemensSVC/Edit_CO_WH_ShipmentResponse")]
        System.Threading.Tasks.Task<string> Edit_CO_WH_ShipmentAsync(string S_FInterID, string S_ShipDate, string S_HAWB, string S_PalletSeq, string S_PalletCount, string S_GrossWeight, string S_EmptyCarton, string S_ShipNO, string S_Project, string S_Type, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/DeleteShipment", ReplyAction="http://tempuri.org/ISiemensSVC/DeleteShipmentResponse")]
        string DeleteShipment(string FInterID, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/DeleteShipment", ReplyAction="http://tempuri.org/ISiemensSVC/DeleteShipmentResponse")]
        System.Threading.Tasks.Task<string> DeleteShipmentAsync(string FInterID, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/DeleteShipmentEntry", ReplyAction="http://tempuri.org/ISiemensSVC/DeleteShipmentEntryResponse")]
        string DeleteShipmentEntry(string FDetailID, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/DeleteShipmentEntry", ReplyAction="http://tempuri.org/ISiemensSVC/DeleteShipmentEntryResponse")]
        System.Threading.Tasks.Task<string> DeleteShipmentEntryAsync(string FDetailID, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/DeleteMultiSelectShipment", ReplyAction="http://tempuri.org/ISiemensSVC/DeleteMultiSelectShipmentResponse")]
        string DeleteMultiSelectShipment(string FInterID_List, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/DeleteMultiSelectShipment", ReplyAction="http://tempuri.org/ISiemensSVC/DeleteMultiSelectShipmentResponse")]
        System.Threading.Tasks.Task<string> DeleteMultiSelectShipmentAsync(string FInterID_List, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/UpdateShipment_FStatus", ReplyAction="http://tempuri.org/ISiemensSVC/UpdateShipment_FStatusResponse")]
        string UpdateShipment_FStatus(string FInterID_List, string Status, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/UpdateShipment_FStatus", ReplyAction="http://tempuri.org/ISiemensSVC/UpdateShipment_FStatusResponse")]
        System.Threading.Tasks.Task<string> UpdateShipment_FStatusAsync(string FInterID_List, string Status, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/GetShipment_One", ReplyAction="http://tempuri.org/ISiemensSVC/GetShipment_OneResponse")]
        System.Data.DataSet GetShipment_One(string FInterID, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/GetShipment_One", ReplyAction="http://tempuri.org/ISiemensSVC/GetShipment_OneResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetShipment_OneAsync(string FInterID, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/GetShipmentEntry_One", ReplyAction="http://tempuri.org/ISiemensSVC/GetShipmentEntry_OneResponse")]
        System.Data.DataSet GetShipmentEntry_One(string FDetailID, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/GetShipmentEntry_One", ReplyAction="http://tempuri.org/ISiemensSVC/GetShipmentEntry_OneResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetShipmentEntry_OneAsync(string FDetailID, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/Edit_CO_WH_ShipmentEntry", ReplyAction="http://tempuri.org/ISiemensSVC/Edit_CO_WH_ShipmentEntryResponse")]
        string Edit_CO_WH_ShipmentEntry(string S_FInterID, string S_FEntryID, string S_FDetailID, string S_FKPONO, string S_FMPNNO, string S_FCTN, string S_FStatus, string S_StationTypeID, string S_Type);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/Edit_CO_WH_ShipmentEntry", ReplyAction="http://tempuri.org/ISiemensSVC/Edit_CO_WH_ShipmentEntryResponse")]
        System.Threading.Tasks.Task<string> Edit_CO_WH_ShipmentEntryAsync(string S_FInterID, string S_FEntryID, string S_FDetailID, string S_FKPONO, string S_FMPNNO, string S_FCTN, string S_FStatus, string S_StationTypeID, string S_Type);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/ExecSql", ReplyAction="http://tempuri.org/ISiemensSVC/ExecSqlResponse")]
        string ExecSql(string S_Sql, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/ExecSql", ReplyAction="http://tempuri.org/ISiemensSVC/ExecSqlResponse")]
        System.Threading.Tasks.Task<string> ExecSqlAsync(string S_Sql, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/Data_Set", ReplyAction="http://tempuri.org/ISiemensSVC/Data_SetResponse")]
        System.Data.DataSet Data_Set(string S_Sql, string S_StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISiemensSVC/Data_Set", ReplyAction="http://tempuri.org/ISiemensSVC/Data_SetResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> Data_SetAsync(string S_Sql, string S_StationTypeID);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="CheckBillNo", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class CheckBillNoRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string S_BillNo;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public string S_StationTypeID;
        
        public CheckBillNoRequest() {
        }
        
        public CheckBillNoRequest(string S_BillNo, string S_StationTypeID) {
            this.S_BillNo = S_BillNo;
            this.S_StationTypeID = S_StationTypeID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="CheckBillNoResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class CheckBillNoResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public System.Data.DataSet CheckBillNoResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public string S_Result;
        
        public CheckBillNoResponse() {
        }
        
        public CheckBillNoResponse(System.Data.DataSet CheckBillNoResult, string S_Result) {
            this.CheckBillNoResult = CheckBillNoResult;
            this.S_Result = S_Result;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISiemensSVCChannel : App.MyMES.SimensService.ISiemensSVC, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SiemensSVCClient : System.ServiceModel.ClientBase<App.MyMES.SimensService.ISiemensSVC>, App.MyMES.SimensService.ISiemensSVC {
        
        public SiemensSVCClient() {
        }
        
        public SiemensSVCClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SiemensSVCClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SiemensSVCClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SiemensSVCClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string WHIn(string S_MPN, string S_BoxSN, string S_Type, string S_StationTypeID) {
            return base.Channel.WHIn(S_MPN, S_BoxSN, S_Type, S_StationTypeID);
        }
        
        public System.Threading.Tasks.Task<string> WHInAsync(string S_MPN, string S_BoxSN, string S_Type, string S_StationTypeID) {
            return base.Channel.WHInAsync(S_MPN, S_BoxSN, S_Type, S_StationTypeID);
        }
        
        public System.Data.DataSet WHIn_DT(string S_StationTypeID, string S_BoxSN) {
            return base.Channel.WHIn_DT(S_StationTypeID, S_BoxSN);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> WHIn_DTAsync(string S_StationTypeID, string S_BoxSN) {
            return base.Channel.WHIn_DTAsync(S_StationTypeID, S_BoxSN);
        }
        
        public string WHOut(string S_MPN, string S_BillNo, string S_BoxSN, string S_Type, string S_StationTypeID) {
            return base.Channel.WHOut(S_MPN, S_BillNo, S_BoxSN, S_Type, S_StationTypeID);
        }
        
        public System.Threading.Tasks.Task<string> WHOutAsync(string S_MPN, string S_BillNo, string S_BoxSN, string S_Type, string S_StationTypeID) {
            return base.Channel.WHOutAsync(S_MPN, S_BillNo, S_BoxSN, S_Type, S_StationTypeID);
        }
        
        public System.Data.DataSet WHOut_DT(string S_StationTypeID, string S_BoxSN) {
            return base.Channel.WHOut_DT(S_StationTypeID, S_BoxSN);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> WHOut_DTAsync(string S_StationTypeID, string S_BoxSN) {
            return base.Channel.WHOut_DTAsync(S_StationTypeID, S_BoxSN);
        }
        
        public System.Data.DataSet GetIpad_BB() {
            return base.Channel.GetIpad_BB();
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetIpad_BBAsync() {
            return base.Channel.GetIpad_BBAsync();
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        App.MyMES.SimensService.CheckBillNoResponse App.MyMES.SimensService.ISiemensSVC.CheckBillNo(App.MyMES.SimensService.CheckBillNoRequest request) {
            return base.Channel.CheckBillNo(request);
        }
        
        public System.Data.DataSet CheckBillNo(string S_BillNo, string S_StationTypeID, out string S_Result) {
            App.MyMES.SimensService.CheckBillNoRequest inValue = new App.MyMES.SimensService.CheckBillNoRequest();
            inValue.S_BillNo = S_BillNo;
            inValue.S_StationTypeID = S_StationTypeID;
            App.MyMES.SimensService.CheckBillNoResponse retVal = ((App.MyMES.SimensService.ISiemensSVC)(this)).CheckBillNo(inValue);
            S_Result = retVal.S_Result;
            return retVal.CheckBillNoResult;
        }
        
        public System.Threading.Tasks.Task<App.MyMES.SimensService.CheckBillNoResponse> CheckBillNoAsync(App.MyMES.SimensService.CheckBillNoRequest request) {
            return base.Channel.CheckBillNoAsync(request);
        }
        
        public System.Data.DataSet GetShipment(string S_Start, string S_End, string FStatus, string S_StationTypeID) {
            return base.Channel.GetShipment(S_Start, S_End, FStatus, S_StationTypeID);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetShipmentAsync(string S_Start, string S_End, string FStatus, string S_StationTypeID) {
            return base.Channel.GetShipmentAsync(S_Start, S_End, FStatus, S_StationTypeID);
        }
        
        public System.Data.DataSet GetShipmentEntry(string S_FInterID, string S_StationTypeID) {
            return base.Channel.GetShipmentEntry(S_FInterID, S_StationTypeID);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetShipmentEntryAsync(string S_FInterID, string S_StationTypeID) {
            return base.Channel.GetShipmentEntryAsync(S_FInterID, S_StationTypeID);
        }
        
        public System.Data.DataSet GetShipmentReport(string S_Start, string S_End, string FStatus, string S_StationTypeID) {
            return base.Channel.GetShipmentReport(S_Start, S_End, FStatus, S_StationTypeID);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetShipmentReportAsync(string S_Start, string S_End, string FStatus, string S_StationTypeID) {
            return base.Channel.GetShipmentReportAsync(S_Start, S_End, FStatus, S_StationTypeID);
        }
        
        public string Edit_CO_WH_Shipment(string S_FInterID, string S_ShipDate, string S_HAWB, string S_PalletSeq, string S_PalletCount, string S_GrossWeight, string S_EmptyCarton, string S_ShipNO, string S_Project, string S_Type, string S_StationTypeID) {
            return base.Channel.Edit_CO_WH_Shipment(S_FInterID, S_ShipDate, S_HAWB, S_PalletSeq, S_PalletCount, S_GrossWeight, S_EmptyCarton, S_ShipNO, S_Project, S_Type, S_StationTypeID);
        }
        
        public System.Threading.Tasks.Task<string> Edit_CO_WH_ShipmentAsync(string S_FInterID, string S_ShipDate, string S_HAWB, string S_PalletSeq, string S_PalletCount, string S_GrossWeight, string S_EmptyCarton, string S_ShipNO, string S_Project, string S_Type, string S_StationTypeID) {
            return base.Channel.Edit_CO_WH_ShipmentAsync(S_FInterID, S_ShipDate, S_HAWB, S_PalletSeq, S_PalletCount, S_GrossWeight, S_EmptyCarton, S_ShipNO, S_Project, S_Type, S_StationTypeID);
        }
        
        public string DeleteShipment(string FInterID, string S_StationTypeID) {
            return base.Channel.DeleteShipment(FInterID, S_StationTypeID);
        }
        
        public System.Threading.Tasks.Task<string> DeleteShipmentAsync(string FInterID, string S_StationTypeID) {
            return base.Channel.DeleteShipmentAsync(FInterID, S_StationTypeID);
        }
        
        public string DeleteShipmentEntry(string FDetailID, string S_StationTypeID) {
            return base.Channel.DeleteShipmentEntry(FDetailID, S_StationTypeID);
        }
        
        public System.Threading.Tasks.Task<string> DeleteShipmentEntryAsync(string FDetailID, string S_StationTypeID) {
            return base.Channel.DeleteShipmentEntryAsync(FDetailID, S_StationTypeID);
        }
        
        public string DeleteMultiSelectShipment(string FInterID_List, string S_StationTypeID) {
            return base.Channel.DeleteMultiSelectShipment(FInterID_List, S_StationTypeID);
        }
        
        public System.Threading.Tasks.Task<string> DeleteMultiSelectShipmentAsync(string FInterID_List, string S_StationTypeID) {
            return base.Channel.DeleteMultiSelectShipmentAsync(FInterID_List, S_StationTypeID);
        }
        
        public string UpdateShipment_FStatus(string FInterID_List, string Status, string S_StationTypeID) {
            return base.Channel.UpdateShipment_FStatus(FInterID_List, Status, S_StationTypeID);
        }
        
        public System.Threading.Tasks.Task<string> UpdateShipment_FStatusAsync(string FInterID_List, string Status, string S_StationTypeID) {
            return base.Channel.UpdateShipment_FStatusAsync(FInterID_List, Status, S_StationTypeID);
        }
        
        public System.Data.DataSet GetShipment_One(string FInterID, string S_StationTypeID) {
            return base.Channel.GetShipment_One(FInterID, S_StationTypeID);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetShipment_OneAsync(string FInterID, string S_StationTypeID) {
            return base.Channel.GetShipment_OneAsync(FInterID, S_StationTypeID);
        }
        
        public System.Data.DataSet GetShipmentEntry_One(string FDetailID, string S_StationTypeID) {
            return base.Channel.GetShipmentEntry_One(FDetailID, S_StationTypeID);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetShipmentEntry_OneAsync(string FDetailID, string S_StationTypeID) {
            return base.Channel.GetShipmentEntry_OneAsync(FDetailID, S_StationTypeID);
        }
        
        public string Edit_CO_WH_ShipmentEntry(string S_FInterID, string S_FEntryID, string S_FDetailID, string S_FKPONO, string S_FMPNNO, string S_FCTN, string S_FStatus, string S_StationTypeID, string S_Type) {
            return base.Channel.Edit_CO_WH_ShipmentEntry(S_FInterID, S_FEntryID, S_FDetailID, S_FKPONO, S_FMPNNO, S_FCTN, S_FStatus, S_StationTypeID, S_Type);
        }
        
        public System.Threading.Tasks.Task<string> Edit_CO_WH_ShipmentEntryAsync(string S_FInterID, string S_FEntryID, string S_FDetailID, string S_FKPONO, string S_FMPNNO, string S_FCTN, string S_FStatus, string S_StationTypeID, string S_Type) {
            return base.Channel.Edit_CO_WH_ShipmentEntryAsync(S_FInterID, S_FEntryID, S_FDetailID, S_FKPONO, S_FMPNNO, S_FCTN, S_FStatus, S_StationTypeID, S_Type);
        }
        
        public string ExecSql(string S_Sql, string S_StationTypeID) {
            return base.Channel.ExecSql(S_Sql, S_StationTypeID);
        }
        
        public System.Threading.Tasks.Task<string> ExecSqlAsync(string S_Sql, string S_StationTypeID) {
            return base.Channel.ExecSqlAsync(S_Sql, S_StationTypeID);
        }
        
        public System.Data.DataSet Data_Set(string S_Sql, string S_StationTypeID) {
            return base.Channel.Data_Set(S_Sql, S_StationTypeID);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> Data_SetAsync(string S_Sql, string S_StationTypeID) {
            return base.Channel.Data_SetAsync(S_Sql, S_StationTypeID);
        }
    }
}