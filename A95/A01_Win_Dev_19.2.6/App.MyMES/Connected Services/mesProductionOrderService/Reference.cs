﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace App.MyMES.mesProductionOrderService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="mesProductionOrderService.ImesProductionOrderSVC")]
    public interface ImesProductionOrderSVC {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesProductionOrderSVC/Insert", ReplyAction="http://tempuri.org/ImesProductionOrderSVC/InsertResponse")]
        string Insert(App.Model.mesProductionOrder model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesProductionOrderSVC/Insert", ReplyAction="http://tempuri.org/ImesProductionOrderSVC/InsertResponse")]
        System.Threading.Tasks.Task<string> InsertAsync(App.Model.mesProductionOrder model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesProductionOrderSVC/Delete", ReplyAction="http://tempuri.org/ImesProductionOrderSVC/DeleteResponse")]
        bool Delete(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesProductionOrderSVC/Delete", ReplyAction="http://tempuri.org/ImesProductionOrderSVC/DeleteResponse")]
        System.Threading.Tasks.Task<bool> DeleteAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesProductionOrderSVC/Update", ReplyAction="http://tempuri.org/ImesProductionOrderSVC/UpdateResponse")]
        string Update(App.Model.mesProductionOrder model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesProductionOrderSVC/Update", ReplyAction="http://tempuri.org/ImesProductionOrderSVC/UpdateResponse")]
        System.Threading.Tasks.Task<string> UpdateAsync(App.Model.mesProductionOrder model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesProductionOrderSVC/Get", ReplyAction="http://tempuri.org/ImesProductionOrderSVC/GetResponse")]
        App.Model.mesProductionOrder Get(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesProductionOrderSVC/Get", ReplyAction="http://tempuri.org/ImesProductionOrderSVC/GetResponse")]
        System.Threading.Tasks.Task<App.Model.mesProductionOrder> GetAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesProductionOrderSVC/ListAll", ReplyAction="http://tempuri.org/ImesProductionOrderSVC/ListAllResponse")]
        App.Model.mesProductionOrder[] ListAll(string S_Where);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesProductionOrderSVC/ListAll", ReplyAction="http://tempuri.org/ImesProductionOrderSVC/ListAllResponse")]
        System.Threading.Tasks.Task<App.Model.mesProductionOrder[]> ListAllAsync(string S_Where);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ImesProductionOrderSVCChannel : App.MyMES.mesProductionOrderService.ImesProductionOrderSVC, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ImesProductionOrderSVCClient : System.ServiceModel.ClientBase<App.MyMES.mesProductionOrderService.ImesProductionOrderSVC>, App.MyMES.mesProductionOrderService.ImesProductionOrderSVC {
        
        public ImesProductionOrderSVCClient() {
        }
        
        public ImesProductionOrderSVCClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ImesProductionOrderSVCClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ImesProductionOrderSVCClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ImesProductionOrderSVCClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string Insert(App.Model.mesProductionOrder model) {
            return base.Channel.Insert(model);
        }
        
        public System.Threading.Tasks.Task<string> InsertAsync(App.Model.mesProductionOrder model) {
            return base.Channel.InsertAsync(model);
        }
        
        public bool Delete(int id) {
            return base.Channel.Delete(id);
        }
        
        public System.Threading.Tasks.Task<bool> DeleteAsync(int id) {
            return base.Channel.DeleteAsync(id);
        }
        
        public string Update(App.Model.mesProductionOrder model) {
            return base.Channel.Update(model);
        }
        
        public System.Threading.Tasks.Task<string> UpdateAsync(App.Model.mesProductionOrder model) {
            return base.Channel.UpdateAsync(model);
        }
        
        public App.Model.mesProductionOrder Get(int id) {
            return base.Channel.Get(id);
        }
        
        public System.Threading.Tasks.Task<App.Model.mesProductionOrder> GetAsync(int id) {
            return base.Channel.GetAsync(id);
        }
        
        public App.Model.mesProductionOrder[] ListAll(string S_Where) {
            return base.Channel.ListAll(S_Where);
        }
        
        public System.Threading.Tasks.Task<App.Model.mesProductionOrder[]> ListAllAsync(string S_Where) {
            return base.Channel.ListAllAsync(S_Where);
        }
    }
}
