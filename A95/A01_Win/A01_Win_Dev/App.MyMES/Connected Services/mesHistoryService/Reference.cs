﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace App.MyMES.mesHistoryService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="mesHistoryService.ImesHistorySVC")]
    public interface ImesHistorySVC {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesHistorySVC/Insert", ReplyAction="http://tempuri.org/ImesHistorySVC/InsertResponse")]
        int Insert(App.Model.mesHistory model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesHistorySVC/Insert", ReplyAction="http://tempuri.org/ImesHistorySVC/InsertResponse")]
        System.Threading.Tasks.Task<int> InsertAsync(App.Model.mesHistory model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesHistorySVC/Delete", ReplyAction="http://tempuri.org/ImesHistorySVC/DeleteResponse")]
        bool Delete(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesHistorySVC/Delete", ReplyAction="http://tempuri.org/ImesHistorySVC/DeleteResponse")]
        System.Threading.Tasks.Task<bool> DeleteAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesHistorySVC/Update", ReplyAction="http://tempuri.org/ImesHistorySVC/UpdateResponse")]
        bool Update(App.Model.mesHistory model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesHistorySVC/Update", ReplyAction="http://tempuri.org/ImesHistorySVC/UpdateResponse")]
        System.Threading.Tasks.Task<bool> UpdateAsync(App.Model.mesHistory model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesHistorySVC/Get", ReplyAction="http://tempuri.org/ImesHistorySVC/GetResponse")]
        App.Model.mesHistory Get(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesHistorySVC/Get", ReplyAction="http://tempuri.org/ImesHistorySVC/GetResponse")]
        System.Threading.Tasks.Task<App.Model.mesHistory> GetAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesHistorySVC/ListAll", ReplyAction="http://tempuri.org/ImesHistorySVC/ListAllResponse")]
        App.Model.mesHistory[] ListAll(string S_Where);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesHistorySVC/ListAll", ReplyAction="http://tempuri.org/ImesHistorySVC/ListAllResponse")]
        System.Threading.Tasks.Task<App.Model.mesHistory[]> ListAllAsync(string S_Where);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ImesHistorySVCChannel : App.MyMES.mesHistoryService.ImesHistorySVC, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ImesHistorySVCClient : System.ServiceModel.ClientBase<App.MyMES.mesHistoryService.ImesHistorySVC>, App.MyMES.mesHistoryService.ImesHistorySVC {
        
        public ImesHistorySVCClient() {
        }
        
        public ImesHistorySVCClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ImesHistorySVCClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ImesHistorySVCClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ImesHistorySVCClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int Insert(App.Model.mesHistory model) {
            return base.Channel.Insert(model);
        }
        
        public System.Threading.Tasks.Task<int> InsertAsync(App.Model.mesHistory model) {
            return base.Channel.InsertAsync(model);
        }
        
        public bool Delete(int id) {
            return base.Channel.Delete(id);
        }
        
        public System.Threading.Tasks.Task<bool> DeleteAsync(int id) {
            return base.Channel.DeleteAsync(id);
        }
        
        public bool Update(App.Model.mesHistory model) {
            return base.Channel.Update(model);
        }
        
        public System.Threading.Tasks.Task<bool> UpdateAsync(App.Model.mesHistory model) {
            return base.Channel.UpdateAsync(model);
        }
        
        public App.Model.mesHistory Get(int id) {
            return base.Channel.Get(id);
        }
        
        public System.Threading.Tasks.Task<App.Model.mesHistory> GetAsync(int id) {
            return base.Channel.GetAsync(id);
        }
        
        public App.Model.mesHistory[] ListAll(string S_Where) {
            return base.Channel.ListAll(S_Where);
        }
        
        public System.Threading.Tasks.Task<App.Model.mesHistory[]> ListAllAsync(string S_Where) {
            return base.Channel.ListAllAsync(S_Where);
        }
    }
}