﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace App.MyMES.mesRouteService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="mesRouteService.ImesRouteSVC")]
    public interface ImesRouteSVC {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesRouteSVC/Insert", ReplyAction="http://tempuri.org/ImesRouteSVC/InsertResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.mesRoute[]))]
        int Insert(App.Model.mesRoute model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesRouteSVC/Insert", ReplyAction="http://tempuri.org/ImesRouteSVC/InsertResponse")]
        System.Threading.Tasks.Task<int> InsertAsync(App.Model.mesRoute model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesRouteSVC/Delete", ReplyAction="http://tempuri.org/ImesRouteSVC/DeleteResponse")]
        bool Delete(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesRouteSVC/Delete", ReplyAction="http://tempuri.org/ImesRouteSVC/DeleteResponse")]
        System.Threading.Tasks.Task<bool> DeleteAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesRouteSVC/Update", ReplyAction="http://tempuri.org/ImesRouteSVC/UpdateResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.mesRoute[]))]
        bool Update(App.Model.mesRoute model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesRouteSVC/Update", ReplyAction="http://tempuri.org/ImesRouteSVC/UpdateResponse")]
        System.Threading.Tasks.Task<bool> UpdateAsync(App.Model.mesRoute model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesRouteSVC/Get", ReplyAction="http://tempuri.org/ImesRouteSVC/GetResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(App.Model.mesRoute[]))]
        App.Model.mesRoute Get(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesRouteSVC/Get", ReplyAction="http://tempuri.org/ImesRouteSVC/GetResponse")]
        System.Threading.Tasks.Task<App.Model.mesRoute> GetAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesRouteSVC/ListAll", ReplyAction="http://tempuri.org/ImesRouteSVC/ListAllResponse")]
        App.Model.mesRoute[] ListAll(string S_Where);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesRouteSVC/ListAll", ReplyAction="http://tempuri.org/ImesRouteSVC/ListAllResponse")]
        System.Threading.Tasks.Task<App.Model.mesRoute[]> ListAllAsync(string S_Where);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ImesRouteSVCChannel : App.MyMES.mesRouteService.ImesRouteSVC, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ImesRouteSVCClient : System.ServiceModel.ClientBase<App.MyMES.mesRouteService.ImesRouteSVC>, App.MyMES.mesRouteService.ImesRouteSVC {
        
        public ImesRouteSVCClient() {
        }
        
        public ImesRouteSVCClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ImesRouteSVCClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ImesRouteSVCClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ImesRouteSVCClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int Insert(App.Model.mesRoute model) {
            return base.Channel.Insert(model);
        }
        
        public System.Threading.Tasks.Task<int> InsertAsync(App.Model.mesRoute model) {
            return base.Channel.InsertAsync(model);
        }
        
        public bool Delete(int id) {
            return base.Channel.Delete(id);
        }
        
        public System.Threading.Tasks.Task<bool> DeleteAsync(int id) {
            return base.Channel.DeleteAsync(id);
        }
        
        public bool Update(App.Model.mesRoute model) {
            return base.Channel.Update(model);
        }
        
        public System.Threading.Tasks.Task<bool> UpdateAsync(App.Model.mesRoute model) {
            return base.Channel.UpdateAsync(model);
        }
        
        public App.Model.mesRoute Get(int id) {
            return base.Channel.Get(id);
        }
        
        public System.Threading.Tasks.Task<App.Model.mesRoute> GetAsync(int id) {
            return base.Channel.GetAsync(id);
        }
        
        public App.Model.mesRoute[] ListAll(string S_Where) {
            return base.Channel.ListAll(S_Where);
        }
        
        public System.Threading.Tasks.Task<App.Model.mesRoute[]> ListAllAsync(string S_Where) {
            return base.Channel.ListAllAsync(S_Where);
        }
    }
}
