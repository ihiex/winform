﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace App.MyMES.mesSerialNumberOfLineService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="mesSerialNumberOfLineService.ImesSerialNumberOfLineSVC")]
    public interface ImesSerialNumberOfLineSVC {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesSerialNumberOfLineSVC/Insert", ReplyAction="http://tempuri.org/ImesSerialNumberOfLineSVC/InsertResponse")]
        int Insert(App.Model.mesSerialNumberOfLine model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesSerialNumberOfLineSVC/Insert", ReplyAction="http://tempuri.org/ImesSerialNumberOfLineSVC/InsertResponse")]
        System.Threading.Tasks.Task<int> InsertAsync(App.Model.mesSerialNumberOfLine model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesSerialNumberOfLineSVC/Delete", ReplyAction="http://tempuri.org/ImesSerialNumberOfLineSVC/DeleteResponse")]
        bool Delete(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesSerialNumberOfLineSVC/Delete", ReplyAction="http://tempuri.org/ImesSerialNumberOfLineSVC/DeleteResponse")]
        System.Threading.Tasks.Task<bool> DeleteAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesSerialNumberOfLineSVC/Update", ReplyAction="http://tempuri.org/ImesSerialNumberOfLineSVC/UpdateResponse")]
        bool Update(App.Model.mesSerialNumberOfLine model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesSerialNumberOfLineSVC/Update", ReplyAction="http://tempuri.org/ImesSerialNumberOfLineSVC/UpdateResponse")]
        System.Threading.Tasks.Task<bool> UpdateAsync(App.Model.mesSerialNumberOfLine model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesSerialNumberOfLineSVC/Get", ReplyAction="http://tempuri.org/ImesSerialNumberOfLineSVC/GetResponse")]
        App.Model.mesSerialNumberOfLine Get(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesSerialNumberOfLineSVC/Get", ReplyAction="http://tempuri.org/ImesSerialNumberOfLineSVC/GetResponse")]
        System.Threading.Tasks.Task<App.Model.mesSerialNumberOfLine> GetAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesSerialNumberOfLineSVC/ListAll", ReplyAction="http://tempuri.org/ImesSerialNumberOfLineSVC/ListAllResponse")]
        App.Model.mesSerialNumberOfLine[] ListAll(string S_Where);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesSerialNumberOfLineSVC/ListAll", ReplyAction="http://tempuri.org/ImesSerialNumberOfLineSVC/ListAllResponse")]
        System.Threading.Tasks.Task<App.Model.mesSerialNumberOfLine[]> ListAllAsync(string S_Where);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ImesSerialNumberOfLineSVCChannel : App.MyMES.mesSerialNumberOfLineService.ImesSerialNumberOfLineSVC, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ImesSerialNumberOfLineSVCClient : System.ServiceModel.ClientBase<App.MyMES.mesSerialNumberOfLineService.ImesSerialNumberOfLineSVC>, App.MyMES.mesSerialNumberOfLineService.ImesSerialNumberOfLineSVC {
        
        public ImesSerialNumberOfLineSVCClient() {
        }
        
        public ImesSerialNumberOfLineSVCClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ImesSerialNumberOfLineSVCClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ImesSerialNumberOfLineSVCClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ImesSerialNumberOfLineSVCClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int Insert(App.Model.mesSerialNumberOfLine model) {
            return base.Channel.Insert(model);
        }
        
        public System.Threading.Tasks.Task<int> InsertAsync(App.Model.mesSerialNumberOfLine model) {
            return base.Channel.InsertAsync(model);
        }
        
        public bool Delete(int id) {
            return base.Channel.Delete(id);
        }
        
        public System.Threading.Tasks.Task<bool> DeleteAsync(int id) {
            return base.Channel.DeleteAsync(id);
        }
        
        public bool Update(App.Model.mesSerialNumberOfLine model) {
            return base.Channel.Update(model);
        }
        
        public System.Threading.Tasks.Task<bool> UpdateAsync(App.Model.mesSerialNumberOfLine model) {
            return base.Channel.UpdateAsync(model);
        }
        
        public App.Model.mesSerialNumberOfLine Get(int id) {
            return base.Channel.Get(id);
        }
        
        public System.Threading.Tasks.Task<App.Model.mesSerialNumberOfLine> GetAsync(int id) {
            return base.Channel.GetAsync(id);
        }
        
        public App.Model.mesSerialNumberOfLine[] ListAll(string S_Where) {
            return base.Channel.ListAll(S_Where);
        }
        
        public System.Threading.Tasks.Task<App.Model.mesSerialNumberOfLine[]> ListAllAsync(string S_Where) {
            return base.Channel.ListAllAsync(S_Where);
        }
    }
}
