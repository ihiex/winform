﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace App.MyMES.mesUnitStateService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="mesUnitStateService.ImesUnitStateSVC")]
    public interface ImesUnitStateSVC {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitStateSVC/Insert", ReplyAction="http://tempuri.org/ImesUnitStateSVC/InsertResponse")]
        int Insert(App.Model.mesUnitState model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitStateSVC/Insert", ReplyAction="http://tempuri.org/ImesUnitStateSVC/InsertResponse")]
        System.Threading.Tasks.Task<int> InsertAsync(App.Model.mesUnitState model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitStateSVC/Delete", ReplyAction="http://tempuri.org/ImesUnitStateSVC/DeleteResponse")]
        bool Delete(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitStateSVC/Delete", ReplyAction="http://tempuri.org/ImesUnitStateSVC/DeleteResponse")]
        System.Threading.Tasks.Task<bool> DeleteAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitStateSVC/Update", ReplyAction="http://tempuri.org/ImesUnitStateSVC/UpdateResponse")]
        bool Update(App.Model.mesUnitState model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitStateSVC/Update", ReplyAction="http://tempuri.org/ImesUnitStateSVC/UpdateResponse")]
        System.Threading.Tasks.Task<bool> UpdateAsync(App.Model.mesUnitState model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitStateSVC/Get", ReplyAction="http://tempuri.org/ImesUnitStateSVC/GetResponse")]
        App.Model.mesUnitState Get(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitStateSVC/Get", ReplyAction="http://tempuri.org/ImesUnitStateSVC/GetResponse")]
        System.Threading.Tasks.Task<App.Model.mesUnitState> GetAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitStateSVC/ListAll", ReplyAction="http://tempuri.org/ImesUnitStateSVC/ListAllResponse")]
        App.Model.mesUnitState[] ListAll(string S_Where);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitStateSVC/ListAll", ReplyAction="http://tempuri.org/ImesUnitStateSVC/ListAllResponse")]
        System.Threading.Tasks.Task<App.Model.mesUnitState[]> ListAllAsync(string S_Where);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ImesUnitStateSVCChannel : App.MyMES.mesUnitStateService.ImesUnitStateSVC, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ImesUnitStateSVCClient : System.ServiceModel.ClientBase<App.MyMES.mesUnitStateService.ImesUnitStateSVC>, App.MyMES.mesUnitStateService.ImesUnitStateSVC {
        
        public ImesUnitStateSVCClient() {
        }
        
        public ImesUnitStateSVCClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ImesUnitStateSVCClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ImesUnitStateSVCClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ImesUnitStateSVCClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int Insert(App.Model.mesUnitState model) {
            return base.Channel.Insert(model);
        }
        
        public System.Threading.Tasks.Task<int> InsertAsync(App.Model.mesUnitState model) {
            return base.Channel.InsertAsync(model);
        }
        
        public bool Delete(int id) {
            return base.Channel.Delete(id);
        }
        
        public System.Threading.Tasks.Task<bool> DeleteAsync(int id) {
            return base.Channel.DeleteAsync(id);
        }
        
        public bool Update(App.Model.mesUnitState model) {
            return base.Channel.Update(model);
        }
        
        public System.Threading.Tasks.Task<bool> UpdateAsync(App.Model.mesUnitState model) {
            return base.Channel.UpdateAsync(model);
        }
        
        public App.Model.mesUnitState Get(int id) {
            return base.Channel.Get(id);
        }
        
        public System.Threading.Tasks.Task<App.Model.mesUnitState> GetAsync(int id) {
            return base.Channel.GetAsync(id);
        }
        
        public App.Model.mesUnitState[] ListAll(string S_Where) {
            return base.Channel.ListAll(S_Where);
        }
        
        public System.Threading.Tasks.Task<App.Model.mesUnitState[]> ListAllAsync(string S_Where) {
            return base.Channel.ListAllAsync(S_Where);
        }
    }
}
