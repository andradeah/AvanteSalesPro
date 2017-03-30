﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace AvanteSales.SyncManager.XmlProvider.XMLService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="XMLServiceSoap", Namespace="http://tempuri.org/")]
    public partial class XMLService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback CargaOperationCompleted;
        
        private System.Threading.SendOrPostCallback CargaParcialOperationCompleted;
        
        private System.Threading.SendOrPostCallback DescargaOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetCabFilesOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public XMLService() {
            this.Url = "http://127.0.0.1/Master.CompactFramework.Sync.XMLProviderServer/XMLService.asmx";
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event CargaCompletedEventHandler CargaCompleted;
        
        /// <remarks/>
        public event CargaParcialCompletedEventHandler CargaParcialCompleted;
        
        /// <remarks/>
        public event DescargaCompletedEventHandler DescargaCompleted;
        
        /// <remarks/>
        public event GetCabFilesCompletedEventHandler GetCabFilesCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Carga", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string Carga(string[] dataParams) {
            object[] results = this.Invoke("Carga", new object[] {
                        dataParams});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void CargaAsync(string[] dataParams) {
            this.CargaAsync(dataParams, null);
        }
        
        /// <remarks/>
        public void CargaAsync(string[] dataParams, object userState) {
            if ((this.CargaOperationCompleted == null)) {
                this.CargaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCargaOperationCompleted);
            }
            this.InvokeAsync("Carga", new object[] {
                        dataParams}, this.CargaOperationCompleted, userState);
        }
        
        private void OnCargaOperationCompleted(object arg) {
            if ((this.CargaCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CargaCompleted(this, new CargaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/CargaParcial", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string CargaParcial(string[] dataParams) {
            object[] results = this.Invoke("CargaParcial", new object[] {
                        dataParams});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void CargaParcialAsync(string[] dataParams) {
            this.CargaParcialAsync(dataParams, null);
        }
        
        /// <remarks/>
        public void CargaParcialAsync(string[] dataParams, object userState) {
            if ((this.CargaParcialOperationCompleted == null)) {
                this.CargaParcialOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCargaParcialOperationCompleted);
            }
            this.InvokeAsync("CargaParcial", new object[] {
                        dataParams}, this.CargaParcialOperationCompleted, userState);
        }
        
        private void OnCargaParcialOperationCompleted(object arg) {
            if ((this.CargaParcialCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CargaParcialCompleted(this, new CargaParcialCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Descarga", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void Descarga(string[] dataParams) {
            this.Invoke("Descarga", new object[] {
                        dataParams});
        }
        
        /// <remarks/>
        public void DescargaAsync(string[] dataParams) {
            this.DescargaAsync(dataParams, null);
        }
        
        /// <remarks/>
        public void DescargaAsync(string[] dataParams, object userState) {
            if ((this.DescargaOperationCompleted == null)) {
                this.DescargaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDescargaOperationCompleted);
            }
            this.InvokeAsync("Descarga", new object[] {
                        dataParams}, this.DescargaOperationCompleted, userState);
        }
        
        private void OnDescargaOperationCompleted(object arg) {
            if ((this.DescargaCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DescargaCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetCabFiles", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetCabFiles() {
            object[] results = this.Invoke("GetCabFiles", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetCabFilesAsync() {
            this.GetCabFilesAsync(null);
        }
        
        /// <remarks/>
        public void GetCabFilesAsync(object userState) {
            if ((this.GetCabFilesOperationCompleted == null)) {
                this.GetCabFilesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetCabFilesOperationCompleted);
            }
            this.InvokeAsync("GetCabFiles", new object[0], this.GetCabFilesOperationCompleted, userState);
        }
        
        private void OnGetCabFilesOperationCompleted(object arg) {
            if ((this.GetCabFilesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetCabFilesCompleted(this, new GetCabFilesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void CargaCompletedEventHandler(object sender, CargaCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CargaCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CargaCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void CargaParcialCompletedEventHandler(object sender, CargaParcialCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CargaParcialCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CargaParcialCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void DescargaCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void GetCabFilesCompletedEventHandler(object sender, GetCabFilesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetCabFilesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetCabFilesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591