﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace App.MyMES.mesMaterialUnitService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="mesMaterialUnitService.ImesMaterialUnitSVC")]
    public interface ImesMaterialUnitSVC {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMaterialUnitSVC/Insert", ReplyAction="http://tempuri.org/ImesMaterialUnitSVC/InsertResponse")]
        int Insert(App.Model.MesMaterialUnit model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMaterialUnitSVC/Insert", ReplyAction="http://tempuri.org/ImesMaterialUnitSVC/InsertResponse")]
        System.Threading.Tasks.Task<int> InsertAsync(App.Model.MesMaterialUnit model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMaterialUnitSVC/InserDetail", ReplyAction="http://tempuri.org/ImesMaterialUnitSVC/InserDetailResponse")]
        int InserDetail(App.Model.MesMaterialUnitDetail model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMaterialUnitSVC/InserDetail", ReplyAction="http://tempuri.org/ImesMaterialUnitSVC/InserDetailResponse")]
        System.Threading.Tasks.Task<int> InserDetailAsync(App.Model.MesMaterialUnitDetail model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMaterialUnitSVC/InserForParent", ReplyAction="http://tempuri.org/ImesMaterialUnitSVC/InserForParentResponse")]
        int InserForParent(App.Model.MesMaterialUnit model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMaterialUnitSVC/InserForParent", ReplyAction="http://tempuri.org/ImesMaterialUnitSVC/InserForParentResponse")]
        System.Threading.Tasks.Task<int> InserForParentAsync(App.Model.MesMaterialUnit model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMaterialUnitSVC/Delete", ReplyAction="http://tempuri.org/ImesMaterialUnitSVC/DeleteResponse")]
        bool Delete(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMaterialUnitSVC/Delete", ReplyAction="http://tempuri.org/ImesMaterialUnitSVC/DeleteResponse")]
        System.Threading.Tasks.Task<bool> DeleteAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMaterialUnitSVC/Update", ReplyAction="http://tempuri.org/ImesMaterialUnitSVC/UpdateResponse")]
        bool Update(App.Model.MesMaterialUnit model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMaterialUnitSVC/Update", ReplyAction="http://tempuri.org/ImesMaterialUnitSVC/UpdateResponse")]
        System.Threading.Tasks.Task<bool> UpdateAsync(App.Model.MesMaterialUnit model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMaterialUnitSVC/Get", ReplyAction="http://tempuri.org/ImesMaterialUnitSVC/GetResponse")]
        App.Model.MesMaterialUnit Get(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMaterialUnitSVC/Get", ReplyAction="http://tempuri.org/ImesMaterialUnitSVC/GetResponse")]
        System.Threading.Tasks.Task<App.Model.MesMaterialUnit> GetAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMaterialUnitSVC/ListAll", ReplyAction="http://tempuri.org/ImesMaterialUnitSVC/ListAllResponse")]
        App.Model.MesMaterialUnit[] ListAll(string S_Where);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMaterialUnitSVC/ListAll", ReplyAction="http://tempuri.org/ImesMaterialUnitSVC/ListAllResponse")]
        System.Threading.Tasks.Task<App.Model.MesMaterialUnit[]> ListAllAsync(string S_Where);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMaterialUnitSVC/GetMesMaterialUnitByLotCode", ReplyAction="http://tempuri.org/ImesMaterialUnitSVC/GetMesMaterialUnitByLotCodeResponse")]
        System.Data.DataSet GetMesMaterialUnitByLotCode(string PartID, string LotCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMaterialUnitSVC/GetMesMaterialUnitByLotCode", ReplyAction="http://tempuri.org/ImesMaterialUnitSVC/GetMesMaterialUnitByLotCodeResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetMesMaterialUnitByLotCodeAsync(string PartID, string LotCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMaterialUnitSVC/GetMesMaterialUseQTY", ReplyAction="http://tempuri.org/ImesMaterialUnitSVC/GetMesMaterialUseQTYResponse")]
        int GetMesMaterialUseQTY(string MaterialUnitID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMaterialUnitSVC/GetMesMaterialUseQTY", ReplyAction="http://tempuri.org/ImesMaterialUnitSVC/GetMesMaterialUseQTYResponse")]
        System.Threading.Tasks.Task<int> GetMesMaterialUseQTYAsync(string MaterialUnitID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMaterialUnitSVC/GetMesMaterialUnitData", ReplyAction="http://tempuri.org/ImesMaterialUnitSVC/GetMesMaterialUnitDataResponse")]
        System.Data.DataSet GetMesMaterialUnitData(string SerialNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ImesMaterialUnitSVC/GetMesMaterialUnitData", ReplyAction="http://tempuri.org/ImesMaterialUnitSVC/GetMesMaterialUnitDataResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetMesMaterialUnitDataAsync(string SerialNumber);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ImesMaterialUnitSVCChannel : App.MyMES.mesMaterialUnitService.ImesMaterialUnitSVC, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ImesMaterialUnitSVCClient : System.ServiceModel.ClientBase<App.MyMES.mesMaterialUnitService.ImesMaterialUnitSVC>, App.MyMES.mesMaterialUnitService.ImesMaterialUnitSVC {
        
        public ImesMaterialUnitSVCClient() {
        }
        
        public ImesMaterialUnitSVCClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ImesMaterialUnitSVCClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ImesMaterialUnitSVCClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ImesMaterialUnitSVCClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int Insert(App.Model.MesMaterialUnit model) {
            return base.Channel.Insert(model);
        }
        
        public System.Threading.Tasks.Task<int> InsertAsync(App.Model.MesMaterialUnit model) {
            return base.Channel.InsertAsync(model);
        }
        
        public int InserDetail(App.Model.MesMaterialUnitDetail model) {
            return base.Channel.InserDetail(model);
        }
        
        public System.Threading.Tasks.Task<int> InserDetailAsync(App.Model.MesMaterialUnitDetail model) {
            return base.Channel.InserDetailAsync(model);
        }
        
        public int InserForParent(App.Model.MesMaterialUnit model) {
            return base.Channel.InserForParent(model);
        }
        
        public System.Threading.Tasks.Task<int> InserForParentAsync(App.Model.MesMaterialUnit model) {
            return base.Channel.InserForParentAsync(model);
        }
        
        public bool Delete(int id) {
            return base.Channel.Delete(id);
        }
        
        public System.Threading.Tasks.Task<bool> DeleteAsync(int id) {
            return base.Channel.DeleteAsync(id);
        }
        
        public bool Update(App.Model.MesMaterialUnit model) {
            return base.Channel.Update(model);
        }
        
        public System.Threading.Tasks.Task<bool> UpdateAsync(App.Model.MesMaterialUnit model) {
            return base.Channel.UpdateAsync(model);
        }
        
        public App.Model.MesMaterialUnit Get(int id) {
            return base.Channel.Get(id);
        }
        
        public System.Threading.Tasks.Task<App.Model.MesMaterialUnit> GetAsync(int id) {
            return base.Channel.GetAsync(id);
        }
        
        public App.Model.MesMaterialUnit[] ListAll(string S_Where) {
            return base.Channel.ListAll(S_Where);
        }
        
        public System.Threading.Tasks.Task<App.Model.MesMaterialUnit[]> ListAllAsync(string S_Where) {
            return base.Channel.ListAllAsync(S_Where);
        }
        
        public System.Data.DataSet GetMesMaterialUnitByLotCode(string PartID, string LotCode) {
            return base.Channel.GetMesMaterialUnitByLotCode(PartID, LotCode);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetMesMaterialUnitByLotCodeAsync(string PartID, string LotCode) {
            return base.Channel.GetMesMaterialUnitByLotCodeAsync(PartID, LotCode);
        }
        
        public int GetMesMaterialUseQTY(string MaterialUnitID) {
            return base.Channel.GetMesMaterialUseQTY(MaterialUnitID);
        }
        
        public System.Threading.Tasks.Task<int> GetMesMaterialUseQTYAsync(string MaterialUnitID) {
            return base.Channel.GetMesMaterialUseQTYAsync(MaterialUnitID);
        }
        
        public System.Data.DataSet GetMesMaterialUnitData(string SerialNumber) {
            return base.Channel.GetMesMaterialUnitData(SerialNumber);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetMesMaterialUnitDataAsync(string SerialNumber) {
            return base.Channel.GetMesMaterialUnitDataAsync(SerialNumber);
        }
    }
}
