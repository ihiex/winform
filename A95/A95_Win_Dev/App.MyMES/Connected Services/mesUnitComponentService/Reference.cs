﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace App.MyMES.mesUnitComponentService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="mesUnitComponentService.ImesUnitComponentSVC")]
    public interface ImesUnitComponentSVC {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitComponentSVC/Insert", ReplyAction="http://tempuri.org/ImesUnitComponentSVC/InsertResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.mesUnitComponent[]))]
        string Insert(App.Model.mesUnitComponent model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitComponentSVC/Insert", ReplyAction="http://tempuri.org/ImesUnitComponentSVC/InsertResponse")]
        System.Threading.Tasks.Task<string> InsertAsync(App.Model.mesUnitComponent model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitComponentSVC/Delete", ReplyAction="http://tempuri.org/ImesUnitComponentSVC/DeleteResponse")]
        bool Delete(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitComponentSVC/Delete", ReplyAction="http://tempuri.org/ImesUnitComponentSVC/DeleteResponse")]
        System.Threading.Tasks.Task<bool> DeleteAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitComponentSVC/Update", ReplyAction="http://tempuri.org/ImesUnitComponentSVC/UpdateResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.mesUnitComponent[]))]
        bool Update(App.Model.mesUnitComponent model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitComponentSVC/Update", ReplyAction="http://tempuri.org/ImesUnitComponentSVC/UpdateResponse")]
        System.Threading.Tasks.Task<bool> UpdateAsync(App.Model.mesUnitComponent model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitComponentSVC/Get", ReplyAction="http://tempuri.org/ImesUnitComponentSVC/GetResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.mesUnitComponent[]))]
        App.Model.mesUnitComponent Get(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitComponentSVC/Get", ReplyAction="http://tempuri.org/ImesUnitComponentSVC/GetResponse")]
        System.Threading.Tasks.Task<App.Model.mesUnitComponent> GetAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitComponentSVC/ListAll", ReplyAction="http://tempuri.org/ImesUnitComponentSVC/ListAllResponse")]
        App.Model.mesUnitComponent[] ListAll(string S_Where);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitComponentSVC/ListAll", ReplyAction="http://tempuri.org/ImesUnitComponentSVC/ListAllResponse")]
        System.Threading.Tasks.Task<App.Model.mesUnitComponent[]> ListAllAsync(string S_Where);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ImesUnitComponentSVCChannel : App.MyMES.mesUnitComponentService.ImesUnitComponentSVC, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ImesUnitComponentSVCClient : System.ServiceModel.ClientBase<App.MyMES.mesUnitComponentService.ImesUnitComponentSVC>, App.MyMES.mesUnitComponentService.ImesUnitComponentSVC {
        
        public ImesUnitComponentSVCClient() {
        }
        
        public ImesUnitComponentSVCClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ImesUnitComponentSVCClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ImesUnitComponentSVCClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ImesUnitComponentSVCClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string Insert(App.Model.mesUnitComponent model) {
            return base.Channel.Insert(model);
        }
        
        public System.Threading.Tasks.Task<string> InsertAsync(App.Model.mesUnitComponent model) {
            return base.Channel.InsertAsync(model);
        }
        
        public bool Delete(int id) {
            return base.Channel.Delete(id);
        }
        
        public System.Threading.Tasks.Task<bool> DeleteAsync(int id) {
            return base.Channel.DeleteAsync(id);
        }
        
        public bool Update(App.Model.mesUnitComponent model) {
            return base.Channel.Update(model);
        }
        
        public System.Threading.Tasks.Task<bool> UpdateAsync(App.Model.mesUnitComponent model) {
            return base.Channel.UpdateAsync(model);
        }
        
        public App.Model.mesUnitComponent Get(int id) {
            return base.Channel.Get(id);
        }
        
        public System.Threading.Tasks.Task<App.Model.mesUnitComponent> GetAsync(int id) {
            return base.Channel.GetAsync(id);
        }
        
        public App.Model.mesUnitComponent[] ListAll(string S_Where) {
            return base.Channel.ListAll(S_Where);
        }
        
        public System.Threading.Tasks.Task<App.Model.mesUnitComponent[]> ListAllAsync(string S_Where) {
            return base.Channel.ListAllAsync(S_Where);
        }
    }
}
