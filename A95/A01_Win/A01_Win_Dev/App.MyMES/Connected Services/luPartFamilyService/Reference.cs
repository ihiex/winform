﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace App.MyMES.luPartFamilyService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="luPartFamilyService.IluPartFamilySVC")]
    public interface IluPartFamilySVC {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluPartFamilySVC/Insert", ReplyAction="http://tempuri.org/IluPartFamilySVC/InsertResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.luPartFamily[]))]
        int Insert(App.Model.luPartFamily model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluPartFamilySVC/Insert", ReplyAction="http://tempuri.org/IluPartFamilySVC/InsertResponse")]
        System.Threading.Tasks.Task<int> InsertAsync(App.Model.luPartFamily model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluPartFamilySVC/Delete", ReplyAction="http://tempuri.org/IluPartFamilySVC/DeleteResponse")]
        bool Delete(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluPartFamilySVC/Delete", ReplyAction="http://tempuri.org/IluPartFamilySVC/DeleteResponse")]
        System.Threading.Tasks.Task<bool> DeleteAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluPartFamilySVC/Update", ReplyAction="http://tempuri.org/IluPartFamilySVC/UpdateResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.luPartFamily[]))]
        bool Update(App.Model.luPartFamily model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluPartFamilySVC/Update", ReplyAction="http://tempuri.org/IluPartFamilySVC/UpdateResponse")]
        System.Threading.Tasks.Task<bool> UpdateAsync(App.Model.luPartFamily model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluPartFamilySVC/Get", ReplyAction="http://tempuri.org/IluPartFamilySVC/GetResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.luPartFamily[]))]
        App.Model.luPartFamily Get(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluPartFamilySVC/Get", ReplyAction="http://tempuri.org/IluPartFamilySVC/GetResponse")]
        System.Threading.Tasks.Task<App.Model.luPartFamily> GetAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluPartFamilySVC/ListAll", ReplyAction="http://tempuri.org/IluPartFamilySVC/ListAllResponse")]
        App.Model.luPartFamily[] ListAll(string S_Where);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IluPartFamilySVC/ListAll", ReplyAction="http://tempuri.org/IluPartFamilySVC/ListAllResponse")]
        System.Threading.Tasks.Task<App.Model.luPartFamily[]> ListAllAsync(string S_Where);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IluPartFamilySVCChannel : App.MyMES.luPartFamilyService.IluPartFamilySVC, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class IluPartFamilySVCClient : System.ServiceModel.ClientBase<App.MyMES.luPartFamilyService.IluPartFamilySVC>, App.MyMES.luPartFamilyService.IluPartFamilySVC {
        
        public IluPartFamilySVCClient() {
        }
        
        public IluPartFamilySVCClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public IluPartFamilySVCClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IluPartFamilySVCClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IluPartFamilySVCClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int Insert(App.Model.luPartFamily model) {
            return base.Channel.Insert(model);
        }
        
        public System.Threading.Tasks.Task<int> InsertAsync(App.Model.luPartFamily model) {
            return base.Channel.InsertAsync(model);
        }
        
        public bool Delete(int id) {
            return base.Channel.Delete(id);
        }
        
        public System.Threading.Tasks.Task<bool> DeleteAsync(int id) {
            return base.Channel.DeleteAsync(id);
        }
        
        public bool Update(App.Model.luPartFamily model) {
            return base.Channel.Update(model);
        }
        
        public System.Threading.Tasks.Task<bool> UpdateAsync(App.Model.luPartFamily model) {
            return base.Channel.UpdateAsync(model);
        }
        
        public App.Model.luPartFamily Get(int id) {
            return base.Channel.Get(id);
        }
        
        public System.Threading.Tasks.Task<App.Model.luPartFamily> GetAsync(int id) {
            return base.Channel.GetAsync(id);
        }
        
        public App.Model.luPartFamily[] ListAll(string S_Where) {
            return base.Channel.ListAll(S_Where);
        }
        
        public System.Threading.Tasks.Task<App.Model.luPartFamily[]> ListAllAsync(string S_Where) {
            return base.Channel.ListAllAsync(S_Where);
        }
    }
}