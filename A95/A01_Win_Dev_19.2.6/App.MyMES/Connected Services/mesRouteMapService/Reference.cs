﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace App.MyMES.mesRouteMapService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="mesRouteMapService.ImesRouteMapSVC")]
    public interface ImesRouteMapSVC {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesRouteMapSVC/Insert", ReplyAction="http://tempuri.org/ImesRouteMapSVC/InsertResponse")]
        int Insert(App.Model.mesRouteMap model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesRouteMapSVC/Insert", ReplyAction="http://tempuri.org/ImesRouteMapSVC/InsertResponse")]
        System.Threading.Tasks.Task<int> InsertAsync(App.Model.mesRouteMap model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesRouteMapSVC/Delete", ReplyAction="http://tempuri.org/ImesRouteMapSVC/DeleteResponse")]
        bool Delete(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesRouteMapSVC/Delete", ReplyAction="http://tempuri.org/ImesRouteMapSVC/DeleteResponse")]
        System.Threading.Tasks.Task<bool> DeleteAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesRouteMapSVC/Update", ReplyAction="http://tempuri.org/ImesRouteMapSVC/UpdateResponse")]
        bool Update(App.Model.mesRouteMap model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesRouteMapSVC/Update", ReplyAction="http://tempuri.org/ImesRouteMapSVC/UpdateResponse")]
        System.Threading.Tasks.Task<bool> UpdateAsync(App.Model.mesRouteMap model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesRouteMapSVC/MesGetPartIDByMachineSN", ReplyAction="http://tempuri.org/ImesRouteMapSVC/MesGetPartIDByMachineSNResponse")]
        System.Data.DataSet MesGetPartIDByMachineSN(int stationTypeID, string MachineSN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesRouteMapSVC/MesGetPartIDByMachineSN", ReplyAction="http://tempuri.org/ImesRouteMapSVC/MesGetPartIDByMachineSNResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> MesGetPartIDByMachineSNAsync(int stationTypeID, string MachineSN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesRouteMapSVC/Get", ReplyAction="http://tempuri.org/ImesRouteMapSVC/GetResponse")]
        App.Model.mesRouteMap Get(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesRouteMapSVC/Get", ReplyAction="http://tempuri.org/ImesRouteMapSVC/GetResponse")]
        System.Threading.Tasks.Task<App.Model.mesRouteMap> GetAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesRouteMapSVC/ListAll", ReplyAction="http://tempuri.org/ImesRouteMapSVC/ListAllResponse")]
        App.Model.mesRouteMap[] ListAll(string S_Where);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesRouteMapSVC/ListAll", ReplyAction="http://tempuri.org/ImesRouteMapSVC/ListAllResponse")]
        System.Threading.Tasks.Task<App.Model.mesRouteMap[]> ListAllAsync(string S_Where);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ImesRouteMapSVCChannel : App.MyMES.mesRouteMapService.ImesRouteMapSVC, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ImesRouteMapSVCClient : System.ServiceModel.ClientBase<App.MyMES.mesRouteMapService.ImesRouteMapSVC>, App.MyMES.mesRouteMapService.ImesRouteMapSVC {
        
        public ImesRouteMapSVCClient() {
        }
        
        public ImesRouteMapSVCClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ImesRouteMapSVCClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ImesRouteMapSVCClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ImesRouteMapSVCClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int Insert(App.Model.mesRouteMap model) {
            return base.Channel.Insert(model);
        }
        
        public System.Threading.Tasks.Task<int> InsertAsync(App.Model.mesRouteMap model) {
            return base.Channel.InsertAsync(model);
        }
        
        public bool Delete(int id) {
            return base.Channel.Delete(id);
        }
        
        public System.Threading.Tasks.Task<bool> DeleteAsync(int id) {
            return base.Channel.DeleteAsync(id);
        }
        
        public bool Update(App.Model.mesRouteMap model) {
            return base.Channel.Update(model);
        }
        
        public System.Threading.Tasks.Task<bool> UpdateAsync(App.Model.mesRouteMap model) {
            return base.Channel.UpdateAsync(model);
        }
        
        public System.Data.DataSet MesGetPartIDByMachineSN(int stationTypeID, string MachineSN) {
            return base.Channel.MesGetPartIDByMachineSN(stationTypeID, MachineSN);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> MesGetPartIDByMachineSNAsync(int stationTypeID, string MachineSN) {
            return base.Channel.MesGetPartIDByMachineSNAsync(stationTypeID, MachineSN);
        }
        
        public App.Model.mesRouteMap Get(int id) {
            return base.Channel.Get(id);
        }
        
        public System.Threading.Tasks.Task<App.Model.mesRouteMap> GetAsync(int id) {
            return base.Channel.GetAsync(id);
        }
        
        public App.Model.mesRouteMap[] ListAll(string S_Where) {
            return base.Channel.ListAll(S_Where);
        }
        
        public System.Threading.Tasks.Task<App.Model.mesRouteMap[]> ListAllAsync(string S_Where) {
            return base.Channel.ListAllAsync(S_Where);
        }
    }
}
