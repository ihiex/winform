﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace App.MyMES.mesMachineService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="mesMachineService.ImesMachineSVC")]
    public interface ImesMachineSVC {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/Insert", ReplyAction="http://tempuri.org/ImesMachineSVC/InsertResponse")]
        int Insert(App.Model.mesMachine model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/Insert", ReplyAction="http://tempuri.org/ImesMachineSVC/InsertResponse")]
        System.Threading.Tasks.Task<int> InsertAsync(App.Model.mesMachine model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/Delete", ReplyAction="http://tempuri.org/ImesMachineSVC/DeleteResponse")]
        bool Delete(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/Delete", ReplyAction="http://tempuri.org/ImesMachineSVC/DeleteResponse")]
        System.Threading.Tasks.Task<bool> DeleteAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/Update", ReplyAction="http://tempuri.org/ImesMachineSVC/UpdateResponse")]
        bool Update(App.Model.mesMachine model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/Update", ReplyAction="http://tempuri.org/ImesMachineSVC/UpdateResponse")]
        System.Threading.Tasks.Task<bool> UpdateAsync(App.Model.mesMachine model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/Get", ReplyAction="http://tempuri.org/ImesMachineSVC/GetResponse")]
        App.Model.mesMachine Get(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/Get", ReplyAction="http://tempuri.org/ImesMachineSVC/GetResponse")]
        System.Threading.Tasks.Task<App.Model.mesMachine> GetAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/ListAll", ReplyAction="http://tempuri.org/ImesMachineSVC/ListAllResponse")]
        App.Model.mesMachine[] ListAll(string S_Where);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/ListAll", ReplyAction="http://tempuri.org/ImesMachineSVC/ListAllResponse")]
        System.Threading.Tasks.Task<App.Model.mesMachine[]> ListAllAsync(string S_Where);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/MesGetLineIDByMachineSN", ReplyAction="http://tempuri.org/ImesMachineSVC/MesGetLineIDByMachineSNResponse")]
        System.Data.DataSet MesGetLineIDByMachineSN(string MachineSN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/MesGetLineIDByMachineSN", ReplyAction="http://tempuri.org/ImesMachineSVC/MesGetLineIDByMachineSNResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> MesGetLineIDByMachineSNAsync(string MachineSN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/MesGetStatusIDByList", ReplyAction="http://tempuri.org/ImesMachineSVC/MesGetStatusIDByListResponse")]
        System.Data.DataSet MesGetStatusIDByList(int StationTypeID, int PartID, string MachineSN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/MesGetStatusIDByList", ReplyAction="http://tempuri.org/ImesMachineSVC/MesGetStatusIDByListResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> MesGetStatusIDByListAsync(int StationTypeID, int PartID, string MachineSN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/MesModMachineBySNStationTypeID", ReplyAction="http://tempuri.org/ImesMachineSVC/MesModMachineBySNStationTypeIDResponse")]
        void MesModMachineBySNStationTypeID(string MachineSN, int StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/MesModMachineBySNStationTypeID", ReplyAction="http://tempuri.org/ImesMachineSVC/MesModMachineBySNStationTypeIDResponse")]
        System.Threading.Tasks.Task MesModMachineBySNStationTypeIDAsync(string MachineSN, int StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/MesModMachineBySNStationTypeID_Sql", ReplyAction="http://tempuri.org/ImesMachineSVC/MesModMachineBySNStationTypeID_SqlResponse")]
        string MesModMachineBySNStationTypeID_Sql(string MachineSN, int StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/MesModMachineBySNStationTypeID_Sql", ReplyAction="http://tempuri.org/ImesMachineSVC/MesModMachineBySNStationTypeID_SqlResponse")]
        System.Threading.Tasks.Task<string> MesModMachineBySNStationTypeID_SqlAsync(string MachineSN, int StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/MesModMachineBySN", ReplyAction="http://tempuri.org/ImesMachineSVC/MesModMachineBySNResponse")]
        void MesModMachineBySN(string MachineSN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/MesModMachineBySN", ReplyAction="http://tempuri.org/ImesMachineSVC/MesModMachineBySNResponse")]
        System.Threading.Tasks.Task MesModMachineBySNAsync(string MachineSN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/SetMachineRuningQuantity", ReplyAction="http://tempuri.org/ImesMachineSVC/SetMachineRuningQuantityResponse")]
        void SetMachineRuningQuantity(string MachineSN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/SetMachineRuningQuantity", ReplyAction="http://tempuri.org/ImesMachineSVC/SetMachineRuningQuantityResponse")]
        System.Threading.Tasks.Task SetMachineRuningQuantityAsync(string MachineSN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/MesToolingReleaseCheck", ReplyAction="http://tempuri.org/ImesMachineSVC/MesToolingReleaseCheckResponse")]
        string MesToolingReleaseCheck(string MachineSN, string StationTypeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMachineSVC/MesToolingReleaseCheck", ReplyAction="http://tempuri.org/ImesMachineSVC/MesToolingReleaseCheckResponse")]
        System.Threading.Tasks.Task<string> MesToolingReleaseCheckAsync(string MachineSN, string StationTypeID);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ImesMachineSVCChannel : App.MyMES.mesMachineService.ImesMachineSVC, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ImesMachineSVCClient : System.ServiceModel.ClientBase<App.MyMES.mesMachineService.ImesMachineSVC>, App.MyMES.mesMachineService.ImesMachineSVC {
        
        public ImesMachineSVCClient() {
        }
        
        public ImesMachineSVCClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ImesMachineSVCClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ImesMachineSVCClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ImesMachineSVCClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int Insert(App.Model.mesMachine model) {
            return base.Channel.Insert(model);
        }
        
        public System.Threading.Tasks.Task<int> InsertAsync(App.Model.mesMachine model) {
            return base.Channel.InsertAsync(model);
        }
        
        public bool Delete(int id) {
            return base.Channel.Delete(id);
        }
        
        public System.Threading.Tasks.Task<bool> DeleteAsync(int id) {
            return base.Channel.DeleteAsync(id);
        }
        
        public bool Update(App.Model.mesMachine model) {
            return base.Channel.Update(model);
        }
        
        public System.Threading.Tasks.Task<bool> UpdateAsync(App.Model.mesMachine model) {
            return base.Channel.UpdateAsync(model);
        }
        
        public App.Model.mesMachine Get(int id) {
            return base.Channel.Get(id);
        }
        
        public System.Threading.Tasks.Task<App.Model.mesMachine> GetAsync(int id) {
            return base.Channel.GetAsync(id);
        }
        
        public App.Model.mesMachine[] ListAll(string S_Where) {
            return base.Channel.ListAll(S_Where);
        }
        
        public System.Threading.Tasks.Task<App.Model.mesMachine[]> ListAllAsync(string S_Where) {
            return base.Channel.ListAllAsync(S_Where);
        }
        
        public System.Data.DataSet MesGetLineIDByMachineSN(string MachineSN) {
            return base.Channel.MesGetLineIDByMachineSN(MachineSN);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> MesGetLineIDByMachineSNAsync(string MachineSN) {
            return base.Channel.MesGetLineIDByMachineSNAsync(MachineSN);
        }
        
        public System.Data.DataSet MesGetStatusIDByList(int StationTypeID, int PartID, string MachineSN) {
            return base.Channel.MesGetStatusIDByList(StationTypeID, PartID, MachineSN);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> MesGetStatusIDByListAsync(int StationTypeID, int PartID, string MachineSN) {
            return base.Channel.MesGetStatusIDByListAsync(StationTypeID, PartID, MachineSN);
        }
        
        public void MesModMachineBySNStationTypeID(string MachineSN, int StationTypeID) {
            base.Channel.MesModMachineBySNStationTypeID(MachineSN, StationTypeID);
        }
        
        public System.Threading.Tasks.Task MesModMachineBySNStationTypeIDAsync(string MachineSN, int StationTypeID) {
            return base.Channel.MesModMachineBySNStationTypeIDAsync(MachineSN, StationTypeID);
        }
        
        public string MesModMachineBySNStationTypeID_Sql(string MachineSN, int StationTypeID) {
            return base.Channel.MesModMachineBySNStationTypeID_Sql(MachineSN, StationTypeID);
        }
        
        public System.Threading.Tasks.Task<string> MesModMachineBySNStationTypeID_SqlAsync(string MachineSN, int StationTypeID) {
            return base.Channel.MesModMachineBySNStationTypeID_SqlAsync(MachineSN, StationTypeID);
        }
        
        public void MesModMachineBySN(string MachineSN) {
            base.Channel.MesModMachineBySN(MachineSN);
        }
        
        public System.Threading.Tasks.Task MesModMachineBySNAsync(string MachineSN) {
            return base.Channel.MesModMachineBySNAsync(MachineSN);
        }
        
        public void SetMachineRuningQuantity(string MachineSN) {
            base.Channel.SetMachineRuningQuantity(MachineSN);
        }
        
        public System.Threading.Tasks.Task SetMachineRuningQuantityAsync(string MachineSN) {
            return base.Channel.SetMachineRuningQuantityAsync(MachineSN);
        }
        
        public string MesToolingReleaseCheck(string MachineSN, string StationTypeID) {
            return base.Channel.MesToolingReleaseCheck(MachineSN, StationTypeID);
        }
        
        public System.Threading.Tasks.Task<string> MesToolingReleaseCheckAsync(string MachineSN, string StationTypeID) {
            return base.Channel.MesToolingReleaseCheckAsync(MachineSN, StationTypeID);
        }
    }
}
