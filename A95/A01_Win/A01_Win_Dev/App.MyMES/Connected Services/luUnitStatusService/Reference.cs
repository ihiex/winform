﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace App.MyMES.luUnitStatusService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="luUnitStatusService.IluUnitStatusSVC")]
    public interface IluUnitStatusSVC {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluUnitStatusSVC/Insert", ReplyAction="http://tempuri.org/IluUnitStatusSVC/InsertResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.luUnitStatus[]))]
        int Insert(App.Model.luUnitStatus model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluUnitStatusSVC/Insert", ReplyAction="http://tempuri.org/IluUnitStatusSVC/InsertResponse")]
        System.Threading.Tasks.Task<int> InsertAsync(App.Model.luUnitStatus model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluUnitStatusSVC/Delete", ReplyAction="http://tempuri.org/IluUnitStatusSVC/DeleteResponse")]
        bool Delete(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluUnitStatusSVC/Delete", ReplyAction="http://tempuri.org/IluUnitStatusSVC/DeleteResponse")]
        System.Threading.Tasks.Task<bool> DeleteAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluUnitStatusSVC/Update", ReplyAction="http://tempuri.org/IluUnitStatusSVC/UpdateResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.luUnitStatus[]))]
        bool Update(App.Model.luUnitStatus model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluUnitStatusSVC/Update", ReplyAction="http://tempuri.org/IluUnitStatusSVC/UpdateResponse")]
        System.Threading.Tasks.Task<bool> UpdateAsync(App.Model.luUnitStatus model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluUnitStatusSVC/Get", ReplyAction="http://tempuri.org/IluUnitStatusSVC/GetResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.luUnitStatus[]))]
        App.Model.luUnitStatus Get(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluUnitStatusSVC/Get", ReplyAction="http://tempuri.org/IluUnitStatusSVC/GetResponse")]
        System.Threading.Tasks.Task<App.Model.luUnitStatus> GetAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluUnitStatusSVC/ListAll", ReplyAction="http://tempuri.org/IluUnitStatusSVC/ListAllResponse")]
        App.Model.luUnitStatus[] ListAll(string S_Where);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluUnitStatusSVC/ListAll", ReplyAction="http://tempuri.org/IluUnitStatusSVC/ListAllResponse")]
        System.Threading.Tasks.Task<App.Model.luUnitStatus[]> ListAllAsync(string S_Where);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IluUnitStatusSVCChannel : App.MyMES.luUnitStatusService.IluUnitStatusSVC, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class IluUnitStatusSVCClient : System.ServiceModel.ClientBase<App.MyMES.luUnitStatusService.IluUnitStatusSVC>, App.MyMES.luUnitStatusService.IluUnitStatusSVC {
        
        public IluUnitStatusSVCClient() {
        }
        
        public IluUnitStatusSVCClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public IluUnitStatusSVCClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IluUnitStatusSVCClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IluUnitStatusSVCClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int Insert(App.Model.luUnitStatus model) {
            return base.Channel.Insert(model);
        }
        
        public System.Threading.Tasks.Task<int> InsertAsync(App.Model.luUnitStatus model) {
            return base.Channel.InsertAsync(model);
        }
        
        public bool Delete(int id) {
            return base.Channel.Delete(id);
        }
        
        public System.Threading.Tasks.Task<bool> DeleteAsync(int id) {
            return base.Channel.DeleteAsync(id);
        }
        
        public bool Update(App.Model.luUnitStatus model) {
            return base.Channel.Update(model);
        }
        
        public System.Threading.Tasks.Task<bool> UpdateAsync(App.Model.luUnitStatus model) {
            return base.Channel.UpdateAsync(model);
        }
        
        public App.Model.luUnitStatus Get(int id) {
            return base.Channel.Get(id);
        }
        
        public System.Threading.Tasks.Task<App.Model.luUnitStatus> GetAsync(int id) {
            return base.Channel.GetAsync(id);
        }
        
        public App.Model.luUnitStatus[] ListAll(string S_Where) {
            return base.Channel.ListAll(S_Where);
        }
        
        public System.Threading.Tasks.Task<App.Model.luUnitStatus[]> ListAllAsync(string S_Where) {
            return base.Channel.ListAllAsync(S_Where);
        }
    }
}
