﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace App.MyMES.mesUnitDefectService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="mesUnitDefectService.ImesUnitDefectSVC")]
    public interface ImesUnitDefectSVC {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitDefectSVC/Insert", ReplyAction="http://tempuri.org/ImesUnitDefectSVC/InsertResponse")]
        string Insert(App.Model.mesUnitDefect model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitDefectSVC/Insert", ReplyAction="http://tempuri.org/ImesUnitDefectSVC/InsertResponse")]
        System.Threading.Tasks.Task<string> InsertAsync(App.Model.mesUnitDefect model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitDefectSVC/Delete", ReplyAction="http://tempuri.org/ImesUnitDefectSVC/DeleteResponse")]
        bool Delete(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitDefectSVC/Delete", ReplyAction="http://tempuri.org/ImesUnitDefectSVC/DeleteResponse")]
        System.Threading.Tasks.Task<bool> DeleteAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitDefectSVC/Update", ReplyAction="http://tempuri.org/ImesUnitDefectSVC/UpdateResponse")]
        string Update(App.Model.mesUnitDefect model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitDefectSVC/Update", ReplyAction="http://tempuri.org/ImesUnitDefectSVC/UpdateResponse")]
        System.Threading.Tasks.Task<string> UpdateAsync(App.Model.mesUnitDefect model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitDefectSVC/Get", ReplyAction="http://tempuri.org/ImesUnitDefectSVC/GetResponse")]
        App.Model.mesUnitDefect Get(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitDefectSVC/Get", ReplyAction="http://tempuri.org/ImesUnitDefectSVC/GetResponse")]
        System.Threading.Tasks.Task<App.Model.mesUnitDefect> GetAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitDefectSVC/ListAll", ReplyAction="http://tempuri.org/ImesUnitDefectSVC/ListAllResponse")]
        App.Model.mesUnitDefect[] ListAll(string S_Where);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesUnitDefectSVC/ListAll", ReplyAction="http://tempuri.org/ImesUnitDefectSVC/ListAllResponse")]
        System.Threading.Tasks.Task<App.Model.mesUnitDefect[]> ListAllAsync(string S_Where);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ImesUnitDefectSVCChannel : App.MyMES.mesUnitDefectService.ImesUnitDefectSVC, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ImesUnitDefectSVCClient : System.ServiceModel.ClientBase<App.MyMES.mesUnitDefectService.ImesUnitDefectSVC>, App.MyMES.mesUnitDefectService.ImesUnitDefectSVC {
        
        public ImesUnitDefectSVCClient() {
        }
        
        public ImesUnitDefectSVCClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ImesUnitDefectSVCClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ImesUnitDefectSVCClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ImesUnitDefectSVCClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string Insert(App.Model.mesUnitDefect model) {
            return base.Channel.Insert(model);
        }
        
        public System.Threading.Tasks.Task<string> InsertAsync(App.Model.mesUnitDefect model) {
            return base.Channel.InsertAsync(model);
        }
        
        public bool Delete(int id) {
            return base.Channel.Delete(id);
        }
        
        public System.Threading.Tasks.Task<bool> DeleteAsync(int id) {
            return base.Channel.DeleteAsync(id);
        }
        
        public string Update(App.Model.mesUnitDefect model) {
            return base.Channel.Update(model);
        }
        
        public System.Threading.Tasks.Task<string> UpdateAsync(App.Model.mesUnitDefect model) {
            return base.Channel.UpdateAsync(model);
        }
        
        public App.Model.mesUnitDefect Get(int id) {
            return base.Channel.Get(id);
        }
        
        public System.Threading.Tasks.Task<App.Model.mesUnitDefect> GetAsync(int id) {
            return base.Channel.GetAsync(id);
        }
        
        public App.Model.mesUnitDefect[] ListAll(string S_Where) {
            return base.Channel.ListAll(S_Where);
        }
        
        public System.Threading.Tasks.Task<App.Model.mesUnitDefect[]> ListAllAsync(string S_Where) {
            return base.Channel.ListAllAsync(S_Where);
        }
    }
}
