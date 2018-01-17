﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConnectFourClient.ConnectFourService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ConnectFourService.IConnectFourService")]
    public interface IConnectFourService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IConnectFourService/register", ReplyAction="http://tempuri.org/IConnectFourService/registerResponse")]
        void register(string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IConnectFourService/register", ReplyAction="http://tempuri.org/IConnectFourService/registerResponse")]
        System.Threading.Tasks.Task registerAsync(string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IConnectFourService/login", ReplyAction="http://tempuri.org/IConnectFourService/loginResponse")]
        bool login(string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IConnectFourService/login", ReplyAction="http://tempuri.org/IConnectFourService/loginResponse")]
        System.Threading.Tasks.Task<bool> loginAsync(string username, string password);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IConnectFourServiceChannel : ConnectFourClient.ConnectFourService.IConnectFourService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ConnectFourServiceClient : System.ServiceModel.ClientBase<ConnectFourClient.ConnectFourService.IConnectFourService>, ConnectFourClient.ConnectFourService.IConnectFourService {
        
        public ConnectFourServiceClient() {
        }
        
        public ConnectFourServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ConnectFourServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ConnectFourServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ConnectFourServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void register(string username, string password) {
            base.Channel.register(username, password);
        }
        
        public System.Threading.Tasks.Task registerAsync(string username, string password) {
            return base.Channel.registerAsync(username, password);
        }
        
        public bool login(string username, string password) {
            return base.Channel.login(username, password);
        }
        
        public System.Threading.Tasks.Task<bool> loginAsync(string username, string password) {
            return base.Channel.loginAsync(username, password);
        }
    }
}