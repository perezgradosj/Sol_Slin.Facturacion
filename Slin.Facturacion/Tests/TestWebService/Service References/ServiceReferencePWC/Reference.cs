﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestWebService.ServiceReferencePWC {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.slin.com.pe/", ConfigurationName="ServiceReferencePWC.WServiceGetDocumentSoap")]
    public interface WServiceGetDocumentSoap {
        
        // CODEGEN: Generating message contract since element name NUM_CPE from namespace http://www.slin.com.pe/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://www.slin.com.pe/GetDocumentoPDF", ReplyAction="*")]
        TestWebService.ServiceReferencePWC.GetDocumentoPDFResponse GetDocumentoPDF(TestWebService.ServiceReferencePWC.GetDocumentoPDFRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.slin.com.pe/GetDocumentoPDF", ReplyAction="*")]
        System.Threading.Tasks.Task<TestWebService.ServiceReferencePWC.GetDocumentoPDFResponse> GetDocumentoPDFAsync(TestWebService.ServiceReferencePWC.GetDocumentoPDFRequest request);
        
        // CODEGEN: Generating message contract since element name NUM_CPE from namespace http://www.slin.com.pe/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://www.slin.com.pe/GetDocumentoXML", ReplyAction="*")]
        TestWebService.ServiceReferencePWC.GetDocumentoXMLResponse GetDocumentoXML(TestWebService.ServiceReferencePWC.GetDocumentoXMLRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.slin.com.pe/GetDocumentoXML", ReplyAction="*")]
        System.Threading.Tasks.Task<TestWebService.ServiceReferencePWC.GetDocumentoXMLResponse> GetDocumentoXMLAsync(TestWebService.ServiceReferencePWC.GetDocumentoXMLRequest request);
        
        // CODEGEN: Generating message contract since element name NUM_CE from namespace http://www.slin.com.pe/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://www.slin.com.pe/GetStatusDocument", ReplyAction="*")]
        TestWebService.ServiceReferencePWC.GetStatusDocumentResponse GetStatusDocument(TestWebService.ServiceReferencePWC.GetStatusDocumentRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.slin.com.pe/GetStatusDocument", ReplyAction="*")]
        System.Threading.Tasks.Task<TestWebService.ServiceReferencePWC.GetStatusDocumentResponse> GetStatusDocumentAsync(TestWebService.ServiceReferencePWC.GetStatusDocumentRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetDocumentoPDFRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetDocumentoPDF", Namespace="http://www.slin.com.pe/", Order=0)]
        public TestWebService.ServiceReferencePWC.GetDocumentoPDFRequestBody Body;
        
        public GetDocumentoPDFRequest() {
        }
        
        public GetDocumentoPDFRequest(TestWebService.ServiceReferencePWC.GetDocumentoPDFRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.slin.com.pe/")]
    public partial class GetDocumentoPDFRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string NUM_CPE;
        
        public GetDocumentoPDFRequestBody() {
        }
        
        public GetDocumentoPDFRequestBody(string NUM_CPE) {
            this.NUM_CPE = NUM_CPE;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetDocumentoPDFResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetDocumentoPDFResponse", Namespace="http://www.slin.com.pe/", Order=0)]
        public TestWebService.ServiceReferencePWC.GetDocumentoPDFResponseBody Body;
        
        public GetDocumentoPDFResponse() {
        }
        
        public GetDocumentoPDFResponse(TestWebService.ServiceReferencePWC.GetDocumentoPDFResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.slin.com.pe/")]
    public partial class GetDocumentoPDFResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetDocumentoPDFResult;
        
        public GetDocumentoPDFResponseBody() {
        }
        
        public GetDocumentoPDFResponseBody(string GetDocumentoPDFResult) {
            this.GetDocumentoPDFResult = GetDocumentoPDFResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetDocumentoXMLRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetDocumentoXML", Namespace="http://www.slin.com.pe/", Order=0)]
        public TestWebService.ServiceReferencePWC.GetDocumentoXMLRequestBody Body;
        
        public GetDocumentoXMLRequest() {
        }
        
        public GetDocumentoXMLRequest(TestWebService.ServiceReferencePWC.GetDocumentoXMLRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.slin.com.pe/")]
    public partial class GetDocumentoXMLRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string NUM_CPE;
        
        public GetDocumentoXMLRequestBody() {
        }
        
        public GetDocumentoXMLRequestBody(string NUM_CPE) {
            this.NUM_CPE = NUM_CPE;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetDocumentoXMLResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetDocumentoXMLResponse", Namespace="http://www.slin.com.pe/", Order=0)]
        public TestWebService.ServiceReferencePWC.GetDocumentoXMLResponseBody Body;
        
        public GetDocumentoXMLResponse() {
        }
        
        public GetDocumentoXMLResponse(TestWebService.ServiceReferencePWC.GetDocumentoXMLResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.slin.com.pe/")]
    public partial class GetDocumentoXMLResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetDocumentoXMLResult;
        
        public GetDocumentoXMLResponseBody() {
        }
        
        public GetDocumentoXMLResponseBody(string GetDocumentoXMLResult) {
            this.GetDocumentoXMLResult = GetDocumentoXMLResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetStatusDocumentRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetStatusDocument", Namespace="http://www.slin.com.pe/", Order=0)]
        public TestWebService.ServiceReferencePWC.GetStatusDocumentRequestBody Body;
        
        public GetStatusDocumentRequest() {
        }
        
        public GetStatusDocumentRequest(TestWebService.ServiceReferencePWC.GetStatusDocumentRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.slin.com.pe/")]
    public partial class GetStatusDocumentRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string NUM_CE;
        
        public GetStatusDocumentRequestBody() {
        }
        
        public GetStatusDocumentRequestBody(string NUM_CE) {
            this.NUM_CE = NUM_CE;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetStatusDocumentResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetStatusDocumentResponse", Namespace="http://www.slin.com.pe/", Order=0)]
        public TestWebService.ServiceReferencePWC.GetStatusDocumentResponseBody Body;
        
        public GetStatusDocumentResponse() {
        }
        
        public GetStatusDocumentResponse(TestWebService.ServiceReferencePWC.GetStatusDocumentResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.slin.com.pe/")]
    public partial class GetStatusDocumentResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetStatusDocumentResult;
        
        public GetStatusDocumentResponseBody() {
        }
        
        public GetStatusDocumentResponseBody(string GetStatusDocumentResult) {
            this.GetStatusDocumentResult = GetStatusDocumentResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface WServiceGetDocumentSoapChannel : TestWebService.ServiceReferencePWC.WServiceGetDocumentSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WServiceGetDocumentSoapClient : System.ServiceModel.ClientBase<TestWebService.ServiceReferencePWC.WServiceGetDocumentSoap>, TestWebService.ServiceReferencePWC.WServiceGetDocumentSoap {
        
        public WServiceGetDocumentSoapClient() {
        }
        
        public WServiceGetDocumentSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WServiceGetDocumentSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WServiceGetDocumentSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WServiceGetDocumentSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        TestWebService.ServiceReferencePWC.GetDocumentoPDFResponse TestWebService.ServiceReferencePWC.WServiceGetDocumentSoap.GetDocumentoPDF(TestWebService.ServiceReferencePWC.GetDocumentoPDFRequest request) {
            return base.Channel.GetDocumentoPDF(request);
        }
        
        public string GetDocumentoPDF(string NUM_CPE) {
            TestWebService.ServiceReferencePWC.GetDocumentoPDFRequest inValue = new TestWebService.ServiceReferencePWC.GetDocumentoPDFRequest();
            inValue.Body = new TestWebService.ServiceReferencePWC.GetDocumentoPDFRequestBody();
            inValue.Body.NUM_CPE = NUM_CPE;
            TestWebService.ServiceReferencePWC.GetDocumentoPDFResponse retVal = ((TestWebService.ServiceReferencePWC.WServiceGetDocumentSoap)(this)).GetDocumentoPDF(inValue);
            return retVal.Body.GetDocumentoPDFResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<TestWebService.ServiceReferencePWC.GetDocumentoPDFResponse> TestWebService.ServiceReferencePWC.WServiceGetDocumentSoap.GetDocumentoPDFAsync(TestWebService.ServiceReferencePWC.GetDocumentoPDFRequest request) {
            return base.Channel.GetDocumentoPDFAsync(request);
        }
        
        public System.Threading.Tasks.Task<TestWebService.ServiceReferencePWC.GetDocumentoPDFResponse> GetDocumentoPDFAsync(string NUM_CPE) {
            TestWebService.ServiceReferencePWC.GetDocumentoPDFRequest inValue = new TestWebService.ServiceReferencePWC.GetDocumentoPDFRequest();
            inValue.Body = new TestWebService.ServiceReferencePWC.GetDocumentoPDFRequestBody();
            inValue.Body.NUM_CPE = NUM_CPE;
            return ((TestWebService.ServiceReferencePWC.WServiceGetDocumentSoap)(this)).GetDocumentoPDFAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        TestWebService.ServiceReferencePWC.GetDocumentoXMLResponse TestWebService.ServiceReferencePWC.WServiceGetDocumentSoap.GetDocumentoXML(TestWebService.ServiceReferencePWC.GetDocumentoXMLRequest request) {
            return base.Channel.GetDocumentoXML(request);
        }
        
        public string GetDocumentoXML(string NUM_CPE) {
            TestWebService.ServiceReferencePWC.GetDocumentoXMLRequest inValue = new TestWebService.ServiceReferencePWC.GetDocumentoXMLRequest();
            inValue.Body = new TestWebService.ServiceReferencePWC.GetDocumentoXMLRequestBody();
            inValue.Body.NUM_CPE = NUM_CPE;
            TestWebService.ServiceReferencePWC.GetDocumentoXMLResponse retVal = ((TestWebService.ServiceReferencePWC.WServiceGetDocumentSoap)(this)).GetDocumentoXML(inValue);
            return retVal.Body.GetDocumentoXMLResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<TestWebService.ServiceReferencePWC.GetDocumentoXMLResponse> TestWebService.ServiceReferencePWC.WServiceGetDocumentSoap.GetDocumentoXMLAsync(TestWebService.ServiceReferencePWC.GetDocumentoXMLRequest request) {
            return base.Channel.GetDocumentoXMLAsync(request);
        }
        
        public System.Threading.Tasks.Task<TestWebService.ServiceReferencePWC.GetDocumentoXMLResponse> GetDocumentoXMLAsync(string NUM_CPE) {
            TestWebService.ServiceReferencePWC.GetDocumentoXMLRequest inValue = new TestWebService.ServiceReferencePWC.GetDocumentoXMLRequest();
            inValue.Body = new TestWebService.ServiceReferencePWC.GetDocumentoXMLRequestBody();
            inValue.Body.NUM_CPE = NUM_CPE;
            return ((TestWebService.ServiceReferencePWC.WServiceGetDocumentSoap)(this)).GetDocumentoXMLAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        TestWebService.ServiceReferencePWC.GetStatusDocumentResponse TestWebService.ServiceReferencePWC.WServiceGetDocumentSoap.GetStatusDocument(TestWebService.ServiceReferencePWC.GetStatusDocumentRequest request) {
            return base.Channel.GetStatusDocument(request);
        }
        
        public string GetStatusDocument(string NUM_CE) {
            TestWebService.ServiceReferencePWC.GetStatusDocumentRequest inValue = new TestWebService.ServiceReferencePWC.GetStatusDocumentRequest();
            inValue.Body = new TestWebService.ServiceReferencePWC.GetStatusDocumentRequestBody();
            inValue.Body.NUM_CE = NUM_CE;
            TestWebService.ServiceReferencePWC.GetStatusDocumentResponse retVal = ((TestWebService.ServiceReferencePWC.WServiceGetDocumentSoap)(this)).GetStatusDocument(inValue);
            return retVal.Body.GetStatusDocumentResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<TestWebService.ServiceReferencePWC.GetStatusDocumentResponse> TestWebService.ServiceReferencePWC.WServiceGetDocumentSoap.GetStatusDocumentAsync(TestWebService.ServiceReferencePWC.GetStatusDocumentRequest request) {
            return base.Channel.GetStatusDocumentAsync(request);
        }
        
        public System.Threading.Tasks.Task<TestWebService.ServiceReferencePWC.GetStatusDocumentResponse> GetStatusDocumentAsync(string NUM_CE) {
            TestWebService.ServiceReferencePWC.GetStatusDocumentRequest inValue = new TestWebService.ServiceReferencePWC.GetStatusDocumentRequest();
            inValue.Body = new TestWebService.ServiceReferencePWC.GetStatusDocumentRequestBody();
            inValue.Body.NUM_CE = NUM_CE;
            return ((TestWebService.ServiceReferencePWC.WServiceGetDocumentSoap)(this)).GetStatusDocumentAsync(inValue);
        }
    }
}
