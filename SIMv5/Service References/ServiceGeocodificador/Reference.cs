﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIM.ServiceGeocodificador {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://util.geo.municipio.com.co/", ConfigurationName="ServiceGeocodificador.GeoUbicacionWS")]
    public interface GeoUbicacionWS {
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="urn:getUbicaDirUrbana", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        SIM.ServiceGeocodificador.getUbicaDirUrbanaResponse getUbicaDirUrbana(SIM.ServiceGeocodificador.getUbicaDirUrbanaRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:getUbicaDirUrbana", ReplyAction="*")]
        System.Threading.Tasks.Task<SIM.ServiceGeocodificador.getUbicaDirUrbanaResponse> getUbicaDirUrbanaAsync(SIM.ServiceGeocodificador.getUbicaDirUrbanaRequest request);
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="urn:getComunaCorregimiento", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        SIM.ServiceGeocodificador.getComunaCorregimientoResponse getComunaCorregimiento(SIM.ServiceGeocodificador.getComunaCorregimientoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:getComunaCorregimiento", ReplyAction="*")]
        System.Threading.Tasks.Task<SIM.ServiceGeocodificador.getComunaCorregimientoResponse> getComunaCorregimientoAsync(SIM.ServiceGeocodificador.getComunaCorregimientoRequest request);
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="urn:getBarrioVereda", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        SIM.ServiceGeocodificador.getBarrioVeredaResponse getBarrioVereda(SIM.ServiceGeocodificador.getBarrioVeredaRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:getBarrioVereda", ReplyAction="*")]
        System.Threading.Tasks.Task<SIM.ServiceGeocodificador.getBarrioVeredaResponse> getBarrioVeredaAsync(SIM.ServiceGeocodificador.getBarrioVeredaRequest request);
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="urn:getUbicaDirUrbanaImagen", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        SIM.ServiceGeocodificador.getUbicaDirUrbanaImagenResponse getUbicaDirUrbanaImagen(SIM.ServiceGeocodificador.getUbicaDirUrbanaImagenRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:getUbicaDirUrbanaImagen", ReplyAction="*")]
        System.Threading.Tasks.Task<SIM.ServiceGeocodificador.getUbicaDirUrbanaImagenResponse> getUbicaDirUrbanaImagenAsync(SIM.ServiceGeocodificador.getUbicaDirUrbanaImagenRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://util.geo.municipio.com.co/")]
    public partial class beanDireccionSimple : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string codBarrioField;
        
        private string codCbmlField;
        
        private string codComunaField;
        
        private double valueCoordXField;
        
        private bool valueCoordXFieldSpecified;
        
        private double valueCoordYField;
        
        private bool valueCoordYFieldSpecified;
        
        private string valueDireccionNormalField;
        
        private string valueDireccionStandField;
        
        private string valueNomBarrioField;
        
        private string valueNomComunaField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string codBarrio {
            get {
                return this.codBarrioField;
            }
            set {
                this.codBarrioField = value;
                this.RaisePropertyChanged("codBarrio");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string codCbml {
            get {
                return this.codCbmlField;
            }
            set {
                this.codCbmlField = value;
                this.RaisePropertyChanged("codCbml");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string codComuna {
            get {
                return this.codComunaField;
            }
            set {
                this.codComunaField = value;
                this.RaisePropertyChanged("codComuna");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public double valueCoordX {
            get {
                return this.valueCoordXField;
            }
            set {
                this.valueCoordXField = value;
                this.RaisePropertyChanged("valueCoordX");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valueCoordXSpecified {
            get {
                return this.valueCoordXFieldSpecified;
            }
            set {
                this.valueCoordXFieldSpecified = value;
                this.RaisePropertyChanged("valueCoordXSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public double valueCoordY {
            get {
                return this.valueCoordYField;
            }
            set {
                this.valueCoordYField = value;
                this.RaisePropertyChanged("valueCoordY");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valueCoordYSpecified {
            get {
                return this.valueCoordYFieldSpecified;
            }
            set {
                this.valueCoordYFieldSpecified = value;
                this.RaisePropertyChanged("valueCoordYSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string valueDireccionNormal {
            get {
                return this.valueDireccionNormalField;
            }
            set {
                this.valueDireccionNormalField = value;
                this.RaisePropertyChanged("valueDireccionNormal");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string valueDireccionStand {
            get {
                return this.valueDireccionStandField;
            }
            set {
                this.valueDireccionStandField = value;
                this.RaisePropertyChanged("valueDireccionStand");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string valueNomBarrio {
            get {
                return this.valueNomBarrioField;
            }
            set {
                this.valueNomBarrioField = value;
                this.RaisePropertyChanged("valueNomBarrio");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
        public string valueNomComuna {
            get {
                return this.valueNomComunaField;
            }
            set {
                this.valueNomComunaField = value;
                this.RaisePropertyChanged("valueNomComuna");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://util.geo.municipio.com.co/")]
    public partial class beanDireccion : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int altoImagenField;
        
        private int anchoImagenField;
        
        private string codBarrioField;
        
        private string codCbmlField;
        
        private string codComunaField;
        
        private string tipoGeocodField;
        
        private string uRLImagenField;
        
        private double valueCoordXField;
        
        private bool valueCoordXFieldSpecified;
        
        private double valueCoordYField;
        
        private bool valueCoordYFieldSpecified;
        
        private string valueDireccionNormalField;
        
        private string valueDireccionStandField;
        
        private string valueNomBarrioField;
        
        private string valueNomComunaField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public int altoImagen {
            get {
                return this.altoImagenField;
            }
            set {
                this.altoImagenField = value;
                this.RaisePropertyChanged("altoImagen");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public int anchoImagen {
            get {
                return this.anchoImagenField;
            }
            set {
                this.anchoImagenField = value;
                this.RaisePropertyChanged("anchoImagen");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string codBarrio {
            get {
                return this.codBarrioField;
            }
            set {
                this.codBarrioField = value;
                this.RaisePropertyChanged("codBarrio");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string codCbml {
            get {
                return this.codCbmlField;
            }
            set {
                this.codCbmlField = value;
                this.RaisePropertyChanged("codCbml");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string codComuna {
            get {
                return this.codComunaField;
            }
            set {
                this.codComunaField = value;
                this.RaisePropertyChanged("codComuna");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string tipoGeocod {
            get {
                return this.tipoGeocodField;
            }
            set {
                this.tipoGeocodField = value;
                this.RaisePropertyChanged("tipoGeocod");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string URLImagen {
            get {
                return this.uRLImagenField;
            }
            set {
                this.uRLImagenField = value;
                this.RaisePropertyChanged("URLImagen");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public double valueCoordX {
            get {
                return this.valueCoordXField;
            }
            set {
                this.valueCoordXField = value;
                this.RaisePropertyChanged("valueCoordX");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valueCoordXSpecified {
            get {
                return this.valueCoordXFieldSpecified;
            }
            set {
                this.valueCoordXFieldSpecified = value;
                this.RaisePropertyChanged("valueCoordXSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
        public double valueCoordY {
            get {
                return this.valueCoordYField;
            }
            set {
                this.valueCoordYField = value;
                this.RaisePropertyChanged("valueCoordY");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valueCoordYSpecified {
            get {
                return this.valueCoordYFieldSpecified;
            }
            set {
                this.valueCoordYFieldSpecified = value;
                this.RaisePropertyChanged("valueCoordYSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=9)]
        public string valueDireccionNormal {
            get {
                return this.valueDireccionNormalField;
            }
            set {
                this.valueDireccionNormalField = value;
                this.RaisePropertyChanged("valueDireccionNormal");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=10)]
        public string valueDireccionStand {
            get {
                return this.valueDireccionStandField;
            }
            set {
                this.valueDireccionStandField = value;
                this.RaisePropertyChanged("valueDireccionStand");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=11)]
        public string valueNomBarrio {
            get {
                return this.valueNomBarrioField;
            }
            set {
                this.valueNomBarrioField = value;
                this.RaisePropertyChanged("valueNomBarrio");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=12)]
        public string valueNomComuna {
            get {
                return this.valueNomComunaField;
            }
            set {
                this.valueNomComunaField = value;
                this.RaisePropertyChanged("valueNomComuna");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://util.geo.municipio.com.co/")]
    public partial class beanBarrioVereda : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string codCodigoField;
        
        private string valueNombreField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string codCodigo {
            get {
                return this.codCodigoField;
            }
            set {
                this.codCodigoField = value;
                this.RaisePropertyChanged("codCodigo");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string valueNombre {
            get {
                return this.valueNombreField;
            }
            set {
                this.valueNombreField = value;
                this.RaisePropertyChanged("valueNombre");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://util.geo.municipio.com.co/")]
    public partial class beanComunaCorregimiento : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string codCodigoField;
        
        private string valueNombreField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string codCodigo {
            get {
                return this.codCodigoField;
            }
            set {
                this.codCodigoField = value;
                this.RaisePropertyChanged("codCodigo");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string valueNombre {
            get {
                return this.valueNombreField;
            }
            set {
                this.valueNombreField = value;
                this.RaisePropertyChanged("valueNombre");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getUbicaDirUrbana", WrapperNamespace="http://util.geo.municipio.com.co/", IsWrapped=true)]
    public partial class getUbicaDirUrbanaRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://util.geo.municipio.com.co/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Direccion;
        
        public getUbicaDirUrbanaRequest() {
        }
        
        public getUbicaDirUrbanaRequest(string Direccion) {
            this.Direccion = Direccion;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getUbicaDirUrbanaResponse", WrapperNamespace="http://util.geo.municipio.com.co/", IsWrapped=true)]
    public partial class getUbicaDirUrbanaResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://util.geo.municipio.com.co/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SIM.ServiceGeocodificador.beanDireccionSimple @return;
        
        public getUbicaDirUrbanaResponse() {
        }
        
        public getUbicaDirUrbanaResponse(SIM.ServiceGeocodificador.beanDireccionSimple @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getComunaCorregimiento", WrapperNamespace="http://util.geo.municipio.com.co/", IsWrapped=true)]
    public partial class getComunaCorregimientoRequest {
        
        public getComunaCorregimientoRequest() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getComunaCorregimientoResponse", WrapperNamespace="http://util.geo.municipio.com.co/", IsWrapped=true)]
    public partial class getComunaCorregimientoResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://util.geo.municipio.com.co/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SIM.ServiceGeocodificador.beanComunaCorregimiento[] @return;
        
        public getComunaCorregimientoResponse() {
        }
        
        public getComunaCorregimientoResponse(SIM.ServiceGeocodificador.beanComunaCorregimiento[] @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getBarrioVereda", WrapperNamespace="http://util.geo.municipio.com.co/", IsWrapped=true)]
    public partial class getBarrioVeredaRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://util.geo.municipio.com.co/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string CodigoComuna;
        
        public getBarrioVeredaRequest() {
        }
        
        public getBarrioVeredaRequest(string CodigoComuna) {
            this.CodigoComuna = CodigoComuna;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getBarrioVeredaResponse", WrapperNamespace="http://util.geo.municipio.com.co/", IsWrapped=true)]
    public partial class getBarrioVeredaResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://util.geo.municipio.com.co/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SIM.ServiceGeocodificador.beanBarrioVereda[] @return;
        
        public getBarrioVeredaResponse() {
        }
        
        public getBarrioVeredaResponse(SIM.ServiceGeocodificador.beanBarrioVereda[] @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getUbicaDirUrbanaImagen", WrapperNamespace="http://util.geo.municipio.com.co/", IsWrapped=true)]
    public partial class getUbicaDirUrbanaImagenRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://util.geo.municipio.com.co/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Direccion;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://util.geo.municipio.com.co/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int AnchoImagen;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://util.geo.municipio.com.co/", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int AltoImagen;
        
        public getUbicaDirUrbanaImagenRequest() {
        }
        
        public getUbicaDirUrbanaImagenRequest(string Direccion, int AnchoImagen, int AltoImagen) {
            this.Direccion = Direccion;
            this.AnchoImagen = AnchoImagen;
            this.AltoImagen = AltoImagen;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getUbicaDirUrbanaImagenResponse", WrapperNamespace="http://util.geo.municipio.com.co/", IsWrapped=true)]
    public partial class getUbicaDirUrbanaImagenResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://util.geo.municipio.com.co/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SIM.ServiceGeocodificador.beanDireccion @return;
        
        public getUbicaDirUrbanaImagenResponse() {
        }
        
        public getUbicaDirUrbanaImagenResponse(SIM.ServiceGeocodificador.beanDireccion @return) {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface GeoUbicacionWSChannel : SIM.ServiceGeocodificador.GeoUbicacionWS, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GeoUbicacionWSClient : System.ServiceModel.ClientBase<SIM.ServiceGeocodificador.GeoUbicacionWS>, SIM.ServiceGeocodificador.GeoUbicacionWS {
        
        public GeoUbicacionWSClient() {
        }
        
        public GeoUbicacionWSClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public GeoUbicacionWSClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GeoUbicacionWSClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GeoUbicacionWSClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SIM.ServiceGeocodificador.getUbicaDirUrbanaResponse SIM.ServiceGeocodificador.GeoUbicacionWS.getUbicaDirUrbana(SIM.ServiceGeocodificador.getUbicaDirUrbanaRequest request) {
            return base.Channel.getUbicaDirUrbana(request);
        }
        
        public SIM.ServiceGeocodificador.beanDireccionSimple getUbicaDirUrbana(string Direccion) {
            SIM.ServiceGeocodificador.getUbicaDirUrbanaRequest inValue = new SIM.ServiceGeocodificador.getUbicaDirUrbanaRequest();
            inValue.Direccion = Direccion;
            SIM.ServiceGeocodificador.getUbicaDirUrbanaResponse retVal = ((SIM.ServiceGeocodificador.GeoUbicacionWS)(this)).getUbicaDirUrbana(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SIM.ServiceGeocodificador.getUbicaDirUrbanaResponse> SIM.ServiceGeocodificador.GeoUbicacionWS.getUbicaDirUrbanaAsync(SIM.ServiceGeocodificador.getUbicaDirUrbanaRequest request) {
            return base.Channel.getUbicaDirUrbanaAsync(request);
        }
        
        public System.Threading.Tasks.Task<SIM.ServiceGeocodificador.getUbicaDirUrbanaResponse> getUbicaDirUrbanaAsync(string Direccion) {
            SIM.ServiceGeocodificador.getUbicaDirUrbanaRequest inValue = new SIM.ServiceGeocodificador.getUbicaDirUrbanaRequest();
            inValue.Direccion = Direccion;
            return ((SIM.ServiceGeocodificador.GeoUbicacionWS)(this)).getUbicaDirUrbanaAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SIM.ServiceGeocodificador.getComunaCorregimientoResponse SIM.ServiceGeocodificador.GeoUbicacionWS.getComunaCorregimiento(SIM.ServiceGeocodificador.getComunaCorregimientoRequest request) {
            return base.Channel.getComunaCorregimiento(request);
        }
        
        public SIM.ServiceGeocodificador.beanComunaCorregimiento[] getComunaCorregimiento() {
            SIM.ServiceGeocodificador.getComunaCorregimientoRequest inValue = new SIM.ServiceGeocodificador.getComunaCorregimientoRequest();
            SIM.ServiceGeocodificador.getComunaCorregimientoResponse retVal = ((SIM.ServiceGeocodificador.GeoUbicacionWS)(this)).getComunaCorregimiento(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SIM.ServiceGeocodificador.getComunaCorregimientoResponse> SIM.ServiceGeocodificador.GeoUbicacionWS.getComunaCorregimientoAsync(SIM.ServiceGeocodificador.getComunaCorregimientoRequest request) {
            return base.Channel.getComunaCorregimientoAsync(request);
        }
        
        public System.Threading.Tasks.Task<SIM.ServiceGeocodificador.getComunaCorregimientoResponse> getComunaCorregimientoAsync() {
            SIM.ServiceGeocodificador.getComunaCorregimientoRequest inValue = new SIM.ServiceGeocodificador.getComunaCorregimientoRequest();
            return ((SIM.ServiceGeocodificador.GeoUbicacionWS)(this)).getComunaCorregimientoAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SIM.ServiceGeocodificador.getBarrioVeredaResponse SIM.ServiceGeocodificador.GeoUbicacionWS.getBarrioVereda(SIM.ServiceGeocodificador.getBarrioVeredaRequest request) {
            return base.Channel.getBarrioVereda(request);
        }
        
        public SIM.ServiceGeocodificador.beanBarrioVereda[] getBarrioVereda(string CodigoComuna) {
            SIM.ServiceGeocodificador.getBarrioVeredaRequest inValue = new SIM.ServiceGeocodificador.getBarrioVeredaRequest();
            inValue.CodigoComuna = CodigoComuna;
            SIM.ServiceGeocodificador.getBarrioVeredaResponse retVal = ((SIM.ServiceGeocodificador.GeoUbicacionWS)(this)).getBarrioVereda(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SIM.ServiceGeocodificador.getBarrioVeredaResponse> SIM.ServiceGeocodificador.GeoUbicacionWS.getBarrioVeredaAsync(SIM.ServiceGeocodificador.getBarrioVeredaRequest request) {
            return base.Channel.getBarrioVeredaAsync(request);
        }
        
        public System.Threading.Tasks.Task<SIM.ServiceGeocodificador.getBarrioVeredaResponse> getBarrioVeredaAsync(string CodigoComuna) {
            SIM.ServiceGeocodificador.getBarrioVeredaRequest inValue = new SIM.ServiceGeocodificador.getBarrioVeredaRequest();
            inValue.CodigoComuna = CodigoComuna;
            return ((SIM.ServiceGeocodificador.GeoUbicacionWS)(this)).getBarrioVeredaAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SIM.ServiceGeocodificador.getUbicaDirUrbanaImagenResponse SIM.ServiceGeocodificador.GeoUbicacionWS.getUbicaDirUrbanaImagen(SIM.ServiceGeocodificador.getUbicaDirUrbanaImagenRequest request) {
            return base.Channel.getUbicaDirUrbanaImagen(request);
        }
        
        public SIM.ServiceGeocodificador.beanDireccion getUbicaDirUrbanaImagen(string Direccion, int AnchoImagen, int AltoImagen) {
            SIM.ServiceGeocodificador.getUbicaDirUrbanaImagenRequest inValue = new SIM.ServiceGeocodificador.getUbicaDirUrbanaImagenRequest();
            inValue.Direccion = Direccion;
            inValue.AnchoImagen = AnchoImagen;
            inValue.AltoImagen = AltoImagen;
            SIM.ServiceGeocodificador.getUbicaDirUrbanaImagenResponse retVal = ((SIM.ServiceGeocodificador.GeoUbicacionWS)(this)).getUbicaDirUrbanaImagen(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SIM.ServiceGeocodificador.getUbicaDirUrbanaImagenResponse> SIM.ServiceGeocodificador.GeoUbicacionWS.getUbicaDirUrbanaImagenAsync(SIM.ServiceGeocodificador.getUbicaDirUrbanaImagenRequest request) {
            return base.Channel.getUbicaDirUrbanaImagenAsync(request);
        }
        
        public System.Threading.Tasks.Task<SIM.ServiceGeocodificador.getUbicaDirUrbanaImagenResponse> getUbicaDirUrbanaImagenAsync(string Direccion, int AnchoImagen, int AltoImagen) {
            SIM.ServiceGeocodificador.getUbicaDirUrbanaImagenRequest inValue = new SIM.ServiceGeocodificador.getUbicaDirUrbanaImagenRequest();
            inValue.Direccion = Direccion;
            inValue.AnchoImagen = AnchoImagen;
            inValue.AltoImagen = AltoImagen;
            return ((SIM.ServiceGeocodificador.GeoUbicacionWS)(this)).getUbicaDirUrbanaImagenAsync(inValue);
        }
    }
}