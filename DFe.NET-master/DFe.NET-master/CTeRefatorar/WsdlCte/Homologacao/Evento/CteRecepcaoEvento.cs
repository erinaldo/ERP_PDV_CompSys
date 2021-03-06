//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FusionCore.DFe.WsdlCte.Homologacao.Evento
{ // 
// This source code was auto-generated by wsdl, Version=4.6.1055.0.
// 


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="CteRecepcaoEventoSoap12", Namespace="http://www.portalfiscal.inf.br/cte/wsdl/CteRecepcaoEvento")]
    public partial class CteRecepcaoEvento : System.Web.Services.Protocols.SoapHttpClientProtocol {
    
        private cteCabecMsg cteCabecMsgValueField;
    
        private System.Threading.SendOrPostCallback cteRecepcaoEventoOperationCompleted;
    
        /// <remarks/>
        public CteRecepcaoEvento(string url) {
            this.SoapVersion = System.Web.Services.Protocols.SoapProtocolVersion.Soap12;
            this.Url = url;
        }
    
        public cteCabecMsg cteCabecMsgValue {
            get {
                return this.cteCabecMsgValueField;
            }
            set {
                this.cteCabecMsgValueField = value;
            }
        }
    
        /// <remarks/>
        public event cteRecepcaoEventoCompletedEventHandler cteRecepcaoEventoCompleted;
    
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("cteCabecMsgValue", Direction=System.Web.Services.Protocols.SoapHeaderDirection.InOut)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.portalfiscal.inf.br/cte/wsdl/CteRecepcaoEvento/cteRecepcaoEvento", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute(Namespace="http://www.portalfiscal.inf.br/cte/wsdl/CteRecepcaoEvento")]
        public System.Xml.XmlNode cteRecepcaoEvento([System.Xml.Serialization.XmlElementAttribute(Namespace="http://www.portalfiscal.inf.br/cte/wsdl/CteRecepcaoEvento")] System.Xml.XmlNode cteDadosMsg) {
            object[] results = this.Invoke("cteRecepcaoEvento", new object[] {
                cteDadosMsg});
            return ((System.Xml.XmlNode)(results[0]));
        }
    
        /// <remarks/>
        public System.IAsyncResult BegincteRecepcaoEvento(System.Xml.XmlNode cteDadosMsg, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("cteRecepcaoEvento", new object[] {
                cteDadosMsg}, callback, asyncState);
        }
    
        /// <remarks/>
        public System.Xml.XmlNode EndcteRecepcaoEvento(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((System.Xml.XmlNode)(results[0]));
        }
    
        /// <remarks/>
        public void cteRecepcaoEventoAsync(System.Xml.XmlNode cteDadosMsg) {
            this.cteRecepcaoEventoAsync(cteDadosMsg, null);
        }
    
        /// <remarks/>
        public void cteRecepcaoEventoAsync(System.Xml.XmlNode cteDadosMsg, object userState) {
            if ((this.cteRecepcaoEventoOperationCompleted == null)) {
                this.cteRecepcaoEventoOperationCompleted = new System.Threading.SendOrPostCallback(this.OncteRecepcaoEventoOperationCompleted);
            }
            this.InvokeAsync("cteRecepcaoEvento", new object[] {
                cteDadosMsg}, this.cteRecepcaoEventoOperationCompleted, userState);
        }
    
        private void OncteRecepcaoEventoOperationCompleted(object arg) {
            if ((this.cteRecepcaoEventoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.cteRecepcaoEventoCompleted(this, new cteRecepcaoEventoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
    
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.portalfiscal.inf.br/cte/wsdl/CteRecepcaoEvento")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.portalfiscal.inf.br/cte/wsdl/CteRecepcaoEvento", IsNullable=false)]
    public partial class cteCabecMsg : System.Web.Services.Protocols.SoapHeader {
    
        private string cUFField;
    
        private string versaoDadosField;
    
        private System.Xml.XmlAttribute[] anyAttrField;
    
        /// <remarks/>
        public string cUF {
            get {
                return this.cUFField;
            }
            set {
                this.cUFField = value;
            }
        }
    
        /// <remarks/>
        public string versaoDados {
            get {
                return this.versaoDadosField;
            }
            set {
                this.versaoDadosField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr {
            get {
                return this.anyAttrField;
            }
            set {
                this.anyAttrField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.1055.0")]
    public delegate void cteRecepcaoEventoCompletedEventHandler(object sender, cteRecepcaoEventoCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class cteRecepcaoEventoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
        private object[] results;
    
        internal cteRecepcaoEventoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
            this.results = results;
            }
    
        /// <remarks/>
        public System.Xml.XmlNode Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[0]));
            }
        }
    }
}