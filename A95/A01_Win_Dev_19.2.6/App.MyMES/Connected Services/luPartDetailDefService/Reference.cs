﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace App.MyMES.luPartDetailDefService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="luPartDetailDefService.IluPartDetailDefSVC")]
    public interface IluPartDetailDefSVC {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluPartDetailDefSVC/Insert", ReplyAction="http://tempuri.org/IluPartDetailDefSVC/InsertResponse")]
        int Insert(App.Model.luPartDetailDef model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluPartDetailDefSVC/Insert", ReplyAction="http://tempuri.org/IluPartDetailDefSVC/InsertResponse")]
        System.Threading.Tasks.Task<int> InsertAsync(App.Model.luPartDetailDef model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluPartDetailDefSVC/Delete", ReplyAction="http://tempuri.org/IluPartDetailDefSVC/DeleteResponse")]
        bool Delete(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluPartDetailDefSVC/Delete", ReplyAction="http://tempuri.org/IluPartDetailDefSVC/DeleteResponse")]
        System.Threading.Tasks.Task<bool> DeleteAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluPartDetailDefSVC/Update", ReplyAction="http://tempuri.org/IluPartDetailDefSVC/UpdateResponse")]
        bool Update(App.Model.luPartDetailDef model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluPartDetailDefSVC/Update", ReplyAction="http://tempuri.org/IluPartDetailDefSVC/UpdateResponse")]
        System.Threading.Tasks.Task<bool> UpdateAsync(App.Model.luPartDetailDef model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluPartDetailDefSVC/Get", ReplyAction="http://tempuri.org/IluPartDetailDefSVC/GetResponse")]
        App.Model.luPartDetailDef Get(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluPartDetailDefSVC/Get", ReplyAction="http://tempuri.org/IluPartDetailDefSVC/GetResponse")]
        System.Threading.Tasks.Task<App.Model.luPartDetailDef> GetAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluPartDetailDefSVC/ListAll", ReplyAction="http://tempuri.org/IluPartDetailDefSVC/ListAllResponse")]
        App.Model.luPartDetailDef[] ListAll(string S_Where);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluPartDetailDefSVC/ListAll", ReplyAction="http://tempuri.org/IluPartDetailDefSVC/ListAllResponse")]
        System.Threading.Tasks.Task<App.Model.luPartDetailDef[]> ListAllAsync(string S_Where);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IluPartDetailDefSVCChannel : App.MyMES.luPartDetailDefService.IluPartDetailDefSVC, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class IluPartDetailDefSVCClient : System.ServiceModel.ClientBase<App.MyMES.luPartDetailDefService.IluPartDetailDefSVC>, App.MyMES.luPartDetailDefService.IluPartDetailDefSVC {
        
        public IluPartDetailDefSVCClient() {
        }
        
        public IluPartDetailDefSVCClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public IluPartDetailDefSVCClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IluPartDetailDefSVCClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IluPartDetailDefSVCClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int Insert(App.Model.luPartDetailDef model) {
            return base.Channel.Insert(model);
        }
        
        public System.Threading.Tasks.Task<int> InsertAsync(App.Model.luPartDetailDef model) {
            return base.Channel.InsertAsync(model);
        }
        
        public bool Delete(int id) {
            return base.Channel.Delete(id);
        }
        
        public System.Threading.Tasks.Task<bool> DeleteAsync(int id) {
            return base.Channel.DeleteAsync(id);
        }
        
        public bool Update(App.Model.luPartDetailDef model) {
            return base.Channel.Update(model);
        }
        
        public System.Threading.Tasks.Task<bool> UpdateAsync(App.Model.luPartDetailDef model) {
            return base.Channel.UpdateAsync(model);
        }
        
        public App.Model.luPartDetailDef Get(int id) {
            return base.Channel.Get(id);
        }
        
        public System.Threading.Tasks.Task<App.Model.luPartDetailDef> GetAsync(int id) {
            return base.Channel.GetAsync(id);
        }
        
        public App.Model.luPartDetailDef[] ListAll(string S_Where) {
            return base.Channel.ListAll(S_Where);
        }
        
        public System.Threading.Tasks.Task<App.Model.luPartDetailDef[]> ListAllAsync(string S_Where) {
            return base.Channel.ListAllAsync(S_Where);
        }
    }
}
