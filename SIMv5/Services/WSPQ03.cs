﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Web.Services.WebServiceBindingAttribute(Name="WSPQ03Soap", Namespace="http://tempuri.org/")]
public partial class WSPQ03 : System.Web.Services.Protocols.SoapHttpClientProtocol {
    
    private System.Threading.SendOrPostCallback RecibirDocumentoOperationCompleted;
    
    private System.Threading.SendOrPostCallback RecibirDocumentoValidacionOperationCompleted;
    
    private System.Threading.SendOrPostCallback SolicitudXmlOperationCompleted;
    
    private System.Threading.SendOrPostCallback EstructuraFormularioXmlOperationCompleted;
    
    private System.Threading.SendOrPostCallback EjecutoriarActoOperationCompleted;
    
    private System.Threading.SendOrPostCallback EmitirDocumentoOperationCompleted;
    
    private System.Threading.SendOrPostCallback ObtenerDatosAdjuntosOperationCompleted;
    
    private System.Threading.SendOrPostCallback EnviarDatosRadicacionOperationCompleted;
    
    private System.Threading.SendOrPostCallback InsertarRelacionExpedientesOperationCompleted;
    
    private System.Threading.SendOrPostCallback EliminarRelacionExpedientesOperationCompleted;
    
    private System.Threading.SendOrPostCallback CrearDatoMisTramitesOperationCompleted;
    
    private System.Threading.SendOrPostCallback ModificarDatosExpedienteTramiteOperationCompleted;
    
    private System.Threading.SendOrPostCallback AsociarExpedienteNumeroSilpaOperationCompleted;
    
    private System.Threading.SendOrPostCallback AsociarInfoExpedienteNumeroSilpaOperationCompleted;
    
    private System.Threading.SendOrPostCallback ActivatExpedienteNumeroSilpaOperationCompleted;
    
    private System.Threading.SendOrPostCallback ObtenerDocumentosRadicacionOperationCompleted;
    
    private System.Threading.SendOrPostCallback ObtenerDocumentosNUROperationCompleted;
    
    private System.Threading.SendOrPostCallback ObtenerDocumentoRadicacionOperationCompleted;
    
    private System.Threading.SendOrPostCallback ObtenerPathRadicacionOperationCompleted;
    
    private System.Threading.SendOrPostCallback ObtenerPathCorrespondenciaOperationCompleted;
    
    private System.Threading.SendOrPostCallback ObtenerPathFusOperationCompleted;
    
    private System.Threading.SendOrPostCallback ObtenerPathDocumentosOperationCompleted;
    
    private System.Threading.SendOrPostCallback EnviarDocumentoRadicacionOperationCompleted;
    
    private System.Threading.SendOrPostCallback EnviarDocumentoRadicacionAlternoOperationCompleted;
    
    private System.Threading.SendOrPostCallback EnviarDocumentoCorrespondenciaOperationCompleted;
    
    private System.Threading.SendOrPostCallback RelacionarExpedientesPadreHijoOperationCompleted;
    
    private System.Threading.SendOrPostCallback CrearSolicitudManualOperationCompleted;
    
    private System.Threading.SendOrPostCallback ObtenerNumeroVITALPadreSolicitudOperationCompleted;
    
    private System.Threading.SendOrPostCallback ObtenerAutoridadesSolicitudOperationCompleted;
    
    private System.Threading.SendOrPostCallback ObtenerComunidadesSolicitudOperationCompleted;
    
    private System.Threading.SendOrPostCallback TestOperationCompleted;
    
    /// <remarks/>
    public WSPQ03() {
        string _url = SIM.Utilidades.Data.ObtenerValorParametro("UrlDocsVital").ToString();
        // this.Url = "http://vital.minambiente.gov.co:8182/wspq03.asmx";
        this.Url = _url;
    }
    
    /// <remarks/>
    public event RecibirDocumentoCompletedEventHandler RecibirDocumentoCompleted;
    
    /// <remarks/>
    public event RecibirDocumentoValidacionCompletedEventHandler RecibirDocumentoValidacionCompleted;
    
    /// <remarks/>
    public event SolicitudXmlCompletedEventHandler SolicitudXmlCompleted;
    
    /// <remarks/>
    public event EstructuraFormularioXmlCompletedEventHandler EstructuraFormularioXmlCompleted;
    
    /// <remarks/>
    public event EjecutoriarActoCompletedEventHandler EjecutoriarActoCompleted;
    
    /// <remarks/>
    public event EmitirDocumentoCompletedEventHandler EmitirDocumentoCompleted;
    
    /// <remarks/>
    public event ObtenerDatosAdjuntosCompletedEventHandler ObtenerDatosAdjuntosCompleted;
    
    /// <remarks/>
    public event EnviarDatosRadicacionCompletedEventHandler EnviarDatosRadicacionCompleted;
    
    /// <remarks/>
    public event InsertarRelacionExpedientesCompletedEventHandler InsertarRelacionExpedientesCompleted;
    
    /// <remarks/>
    public event EliminarRelacionExpedientesCompletedEventHandler EliminarRelacionExpedientesCompleted;
    
    /// <remarks/>
    public event CrearDatoMisTramitesCompletedEventHandler CrearDatoMisTramitesCompleted;
    
    /// <remarks/>
    public event ModificarDatosExpedienteTramiteCompletedEventHandler ModificarDatosExpedienteTramiteCompleted;
    
    /// <remarks/>
    public event AsociarExpedienteNumeroSilpaCompletedEventHandler AsociarExpedienteNumeroSilpaCompleted;
    
    /// <remarks/>
    public event AsociarInfoExpedienteNumeroSilpaCompletedEventHandler AsociarInfoExpedienteNumeroSilpaCompleted;
    
    /// <remarks/>
    public event ActivatExpedienteNumeroSilpaCompletedEventHandler ActivatExpedienteNumeroSilpaCompleted;
    
    /// <remarks/>
    public event ObtenerDocumentosRadicacionCompletedEventHandler ObtenerDocumentosRadicacionCompleted;
    
    /// <remarks/>
    public event ObtenerDocumentosNURCompletedEventHandler ObtenerDocumentosNURCompleted;
    
    /// <remarks/>
    public event ObtenerDocumentoRadicacionCompletedEventHandler ObtenerDocumentoRadicacionCompleted;
    
    /// <remarks/>
    public event ObtenerPathRadicacionCompletedEventHandler ObtenerPathRadicacionCompleted;
    
    /// <remarks/>
    public event ObtenerPathCorrespondenciaCompletedEventHandler ObtenerPathCorrespondenciaCompleted;
    
    /// <remarks/>
    public event ObtenerPathFusCompletedEventHandler ObtenerPathFusCompleted;
    
    /// <remarks/>
    public event ObtenerPathDocumentosCompletedEventHandler ObtenerPathDocumentosCompleted;
    
    /// <remarks/>
    public event EnviarDocumentoRadicacionCompletedEventHandler EnviarDocumentoRadicacionCompleted;
    
    /// <remarks/>
    public event EnviarDocumentoRadicacionAlternoCompletedEventHandler EnviarDocumentoRadicacionAlternoCompleted;
    
    /// <remarks/>
    public event EnviarDocumentoCorrespondenciaCompletedEventHandler EnviarDocumentoCorrespondenciaCompleted;
    
    /// <remarks/>
    public event RelacionarExpedientesPadreHijoCompletedEventHandler RelacionarExpedientesPadreHijoCompleted;
    
    /// <remarks/>
    public event CrearSolicitudManualCompletedEventHandler CrearSolicitudManualCompleted;
    
    /// <remarks/>
    public event ObtenerNumeroVITALPadreSolicitudCompletedEventHandler ObtenerNumeroVITALPadreSolicitudCompleted;
    
    /// <remarks/>
    public event ObtenerAutoridadesSolicitudCompletedEventHandler ObtenerAutoridadesSolicitudCompleted;
    
    /// <remarks/>
    public event ObtenerComunidadesSolicitudCompletedEventHandler ObtenerComunidadesSolicitudCompleted;
    
    /// <remarks/>
    public event TestCompletedEventHandler TestCompleted;
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/RecibirActoAdministrativoEIA-01", RequestElementName="RecibirActoAdministrativoEIA-01", RequestNamespace="http://tempuri.org/", ResponseElementName="RecibirActoAdministrativoEIA-01Response", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    [return: System.Xml.Serialization.XmlElementAttribute("RecibirActoAdministrativoEIA-01Result")]
    public string RecibirDocumento(string documentoXML) {
        object[] results = this.Invoke("RecibirDocumento", new object[] {
                    documentoXML});
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginRecibirDocumento(string documentoXML, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("RecibirDocumento", new object[] {
                    documentoXML}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndRecibirDocumento(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void RecibirDocumentoAsync(string documentoXML) {
        this.RecibirDocumentoAsync(documentoXML, null);
    }
    
    /// <remarks/>
    public void RecibirDocumentoAsync(string documentoXML, object userState) {
        if ((this.RecibirDocumentoOperationCompleted == null)) {
            this.RecibirDocumentoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRecibirDocumentoOperationCompleted);
        }
        this.InvokeAsync("RecibirDocumento", new object[] {
                    documentoXML}, this.RecibirDocumentoOperationCompleted, userState);
    }
    
    private void OnRecibirDocumentoOperationCompleted(object arg) {
        if ((this.RecibirDocumentoCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.RecibirDocumentoCompleted(this, new RecibirDocumentoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ValidacionRecibirDocumento", RequestElementName="ValidacionRecibirDocumento", RequestNamespace="http://tempuri.org/", ResponseElementName="ValidacionRecibirDocumentoResponse", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    [return: System.Xml.Serialization.XmlElementAttribute("ValidacionRecibirDocumentoResult")]
    public string RecibirDocumentoValidacion(string documentoXML) {
        object[] results = this.Invoke("RecibirDocumentoValidacion", new object[] {
                    documentoXML});
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginRecibirDocumentoValidacion(string documentoXML, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("RecibirDocumentoValidacion", new object[] {
                    documentoXML}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndRecibirDocumentoValidacion(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void RecibirDocumentoValidacionAsync(string documentoXML) {
        this.RecibirDocumentoValidacionAsync(documentoXML, null);
    }
    
    /// <remarks/>
    public void RecibirDocumentoValidacionAsync(string documentoXML, object userState) {
        if ((this.RecibirDocumentoValidacionOperationCompleted == null)) {
            this.RecibirDocumentoValidacionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRecibirDocumentoValidacionOperationCompleted);
        }
        this.InvokeAsync("RecibirDocumentoValidacion", new object[] {
                    documentoXML}, this.RecibirDocumentoValidacionOperationCompleted, userState);
    }
    
    private void OnRecibirDocumentoValidacionOperationCompleted(object arg) {
        if ((this.RecibirDocumentoValidacionCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.RecibirDocumentoValidacionCompleted(this, new RecibirDocumentoValidacionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SolicitudXml", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string SolicitudXml(string numeroVital) {
        object[] results = this.Invoke("SolicitudXml", new object[] {
                    numeroVital});
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginSolicitudXml(string numeroVital, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("SolicitudXml", new object[] {
                    numeroVital}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndSolicitudXml(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void SolicitudXmlAsync(string numeroVital) {
        this.SolicitudXmlAsync(numeroVital, null);
    }
    
    /// <remarks/>
    public void SolicitudXmlAsync(string numeroVital, object userState) {
        if ((this.SolicitudXmlOperationCompleted == null)) {
            this.SolicitudXmlOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSolicitudXmlOperationCompleted);
        }
        this.InvokeAsync("SolicitudXml", new object[] {
                    numeroVital}, this.SolicitudXmlOperationCompleted, userState);
    }
    
    private void OnSolicitudXmlOperationCompleted(object arg) {
        if ((this.SolicitudXmlCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.SolicitudXmlCompleted(this, new SolicitudXmlCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/EstructuraFormularioXml", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string EstructuraFormularioXml(string numeroVital) {
        object[] results = this.Invoke("EstructuraFormularioXml", new object[] {
                    numeroVital});
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginEstructuraFormularioXml(string numeroVital, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("EstructuraFormularioXml", new object[] {
                    numeroVital}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndEstructuraFormularioXml(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void EstructuraFormularioXmlAsync(string numeroVital) {
        this.EstructuraFormularioXmlAsync(numeroVital, null);
    }
    
    /// <remarks/>
    public void EstructuraFormularioXmlAsync(string numeroVital, object userState) {
        if ((this.EstructuraFormularioXmlOperationCompleted == null)) {
            this.EstructuraFormularioXmlOperationCompleted = new System.Threading.SendOrPostCallback(this.OnEstructuraFormularioXmlOperationCompleted);
        }
        this.InvokeAsync("EstructuraFormularioXml", new object[] {
                    numeroVital}, this.EstructuraFormularioXmlOperationCompleted, userState);
    }
    
    private void OnEstructuraFormularioXmlOperationCompleted(object arg) {
        if ((this.EstructuraFormularioXmlCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.EstructuraFormularioXmlCompleted(this, new EstructuraFormularioXmlCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/EjecutoriarActo", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string EjecutoriarActo(string documento, out bool respuesta) {
        object[] results = this.Invoke("EjecutoriarActo", new object[] {
                    documento});
        respuesta = ((bool)(results[1]));
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginEjecutoriarActo(string documento, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("EjecutoriarActo", new object[] {
                    documento}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndEjecutoriarActo(System.IAsyncResult asyncResult, out bool respuesta) {
        object[] results = this.EndInvoke(asyncResult);
        respuesta = ((bool)(results[1]));
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void EjecutoriarActoAsync(string documento) {
        this.EjecutoriarActoAsync(documento, null);
    }
    
    /// <remarks/>
    public void EjecutoriarActoAsync(string documento, object userState) {
        if ((this.EjecutoriarActoOperationCompleted == null)) {
            this.EjecutoriarActoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnEjecutoriarActoOperationCompleted);
        }
        this.InvokeAsync("EjecutoriarActo", new object[] {
                    documento}, this.EjecutoriarActoOperationCompleted, userState);
    }
    
    private void OnEjecutoriarActoOperationCompleted(object arg) {
        if ((this.EjecutoriarActoCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.EjecutoriarActoCompleted(this, new EjecutoriarActoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/EmitirDocumento", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string EmitirDocumento([System.Xml.Serialization.XmlElementAttribute(Namespace="http://tempuri.org/Notificacion.xsd")] NotificacionType Notificacion) {
        object[] results = this.Invoke("EmitirDocumento", new object[] {
                    Notificacion});
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginEmitirDocumento(NotificacionType Notificacion, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("EmitirDocumento", new object[] {
                    Notificacion}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndEmitirDocumento(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void EmitirDocumentoAsync(NotificacionType Notificacion) {
        this.EmitirDocumentoAsync(Notificacion, null);
    }
    
    /// <remarks/>
    public void EmitirDocumentoAsync(NotificacionType Notificacion, object userState) {
        if ((this.EmitirDocumentoOperationCompleted == null)) {
            this.EmitirDocumentoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnEmitirDocumentoOperationCompleted);
        }
        this.InvokeAsync("EmitirDocumento", new object[] {
                    Notificacion}, this.EmitirDocumentoOperationCompleted, userState);
    }
    
    private void OnEmitirDocumentoOperationCompleted(object arg) {
        if ((this.EmitirDocumentoCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.EmitirDocumentoCompleted(this, new EmitirDocumentoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ObtenerDatosAdjuntos", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string ObtenerDatosAdjuntos(string strXmlDatos) {
        object[] results = this.Invoke("ObtenerDatosAdjuntos", new object[] {
                    strXmlDatos});
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginObtenerDatosAdjuntos(string strXmlDatos, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("ObtenerDatosAdjuntos", new object[] {
                    strXmlDatos}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndObtenerDatosAdjuntos(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void ObtenerDatosAdjuntosAsync(string strXmlDatos) {
        this.ObtenerDatosAdjuntosAsync(strXmlDatos, null);
    }
    
    /// <remarks/>
    public void ObtenerDatosAdjuntosAsync(string strXmlDatos, object userState) {
        if ((this.ObtenerDatosAdjuntosOperationCompleted == null)) {
            this.ObtenerDatosAdjuntosOperationCompleted = new System.Threading.SendOrPostCallback(this.OnObtenerDatosAdjuntosOperationCompleted);
        }
        this.InvokeAsync("ObtenerDatosAdjuntos", new object[] {
                    strXmlDatos}, this.ObtenerDatosAdjuntosOperationCompleted, userState);
    }
    
    private void OnObtenerDatosAdjuntosOperationCompleted(object arg) {
        if ((this.ObtenerDatosAdjuntosCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.ObtenerDatosAdjuntosCompleted(this, new ObtenerDatosAdjuntosCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/EnviarDatosRadicacion", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string EnviarDatosRadicacion(string strXmlDatos) {
        object[] results = this.Invoke("EnviarDatosRadicacion", new object[] {
                    strXmlDatos});
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginEnviarDatosRadicacion(string strXmlDatos, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("EnviarDatosRadicacion", new object[] {
                    strXmlDatos}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndEnviarDatosRadicacion(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void EnviarDatosRadicacionAsync(string strXmlDatos) {
        this.EnviarDatosRadicacionAsync(strXmlDatos, null);
    }
    
    /// <remarks/>
    public void EnviarDatosRadicacionAsync(string strXmlDatos, object userState) {
        if ((this.EnviarDatosRadicacionOperationCompleted == null)) {
            this.EnviarDatosRadicacionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnEnviarDatosRadicacionOperationCompleted);
        }
        this.InvokeAsync("EnviarDatosRadicacion", new object[] {
                    strXmlDatos}, this.EnviarDatosRadicacionOperationCompleted, userState);
    }
    
    private void OnEnviarDatosRadicacionOperationCompleted(object arg) {
        if ((this.EnviarDatosRadicacionCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.EnviarDatosRadicacionCompleted(this, new EnviarDatosRadicacionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/InsertarRelacionExpedientes", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public bool InsertarRelacionExpedientes(string expedienteId, string expedienteIdRef, string numeroVital) {
        object[] results = this.Invoke("InsertarRelacionExpedientes", new object[] {
                    expedienteId,
                    expedienteIdRef,
                    numeroVital});
        return ((bool)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginInsertarRelacionExpedientes(string expedienteId, string expedienteIdRef, string numeroVital, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("InsertarRelacionExpedientes", new object[] {
                    expedienteId,
                    expedienteIdRef,
                    numeroVital}, callback, asyncState);
    }
    
    /// <remarks/>
    public bool EndInsertarRelacionExpedientes(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((bool)(results[0]));
    }
    
    /// <remarks/>
    public void InsertarRelacionExpedientesAsync(string expedienteId, string expedienteIdRef, string numeroVital) {
        this.InsertarRelacionExpedientesAsync(expedienteId, expedienteIdRef, numeroVital, null);
    }
    
    /// <remarks/>
    public void InsertarRelacionExpedientesAsync(string expedienteId, string expedienteIdRef, string numeroVital, object userState) {
        if ((this.InsertarRelacionExpedientesOperationCompleted == null)) {
            this.InsertarRelacionExpedientesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnInsertarRelacionExpedientesOperationCompleted);
        }
        this.InvokeAsync("InsertarRelacionExpedientes", new object[] {
                    expedienteId,
                    expedienteIdRef,
                    numeroVital}, this.InsertarRelacionExpedientesOperationCompleted, userState);
    }
    
    private void OnInsertarRelacionExpedientesOperationCompleted(object arg) {
        if ((this.InsertarRelacionExpedientesCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.InsertarRelacionExpedientesCompleted(this, new InsertarRelacionExpedientesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/EliminarRelacionExpedientes", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public bool EliminarRelacionExpedientes(string expedienteId, string expedienteIdRef) {
        object[] results = this.Invoke("EliminarRelacionExpedientes", new object[] {
                    expedienteId,
                    expedienteIdRef});
        return ((bool)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginEliminarRelacionExpedientes(string expedienteId, string expedienteIdRef, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("EliminarRelacionExpedientes", new object[] {
                    expedienteId,
                    expedienteIdRef}, callback, asyncState);
    }
    
    /// <remarks/>
    public bool EndEliminarRelacionExpedientes(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((bool)(results[0]));
    }
    
    /// <remarks/>
    public void EliminarRelacionExpedientesAsync(string expedienteId, string expedienteIdRef) {
        this.EliminarRelacionExpedientesAsync(expedienteId, expedienteIdRef, null);
    }
    
    /// <remarks/>
    public void EliminarRelacionExpedientesAsync(string expedienteId, string expedienteIdRef, object userState) {
        if ((this.EliminarRelacionExpedientesOperationCompleted == null)) {
            this.EliminarRelacionExpedientesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnEliminarRelacionExpedientesOperationCompleted);
        }
        this.InvokeAsync("EliminarRelacionExpedientes", new object[] {
                    expedienteId,
                    expedienteIdRef}, this.EliminarRelacionExpedientesOperationCompleted, userState);
    }
    
    private void OnEliminarRelacionExpedientesOperationCompleted(object arg) {
        if ((this.EliminarRelacionExpedientesCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.EliminarRelacionExpedientesCompleted(this, new EliminarRelacionExpedientesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/CrearDatoMisTramites", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public bool CrearDatoMisTramites(string strParametrosXml) {
        object[] results = this.Invoke("CrearDatoMisTramites", new object[] {
                    strParametrosXml});
        return ((bool)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginCrearDatoMisTramites(string strParametrosXml, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("CrearDatoMisTramites", new object[] {
                    strParametrosXml}, callback, asyncState);
    }
    
    /// <remarks/>
    public bool EndCrearDatoMisTramites(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((bool)(results[0]));
    }
    
    /// <remarks/>
    public void CrearDatoMisTramitesAsync(string strParametrosXml) {
        this.CrearDatoMisTramitesAsync(strParametrosXml, null);
    }
    
    /// <remarks/>
    public void CrearDatoMisTramitesAsync(string strParametrosXml, object userState) {
        if ((this.CrearDatoMisTramitesOperationCompleted == null)) {
            this.CrearDatoMisTramitesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCrearDatoMisTramitesOperationCompleted);
        }
        this.InvokeAsync("CrearDatoMisTramites", new object[] {
                    strParametrosXml}, this.CrearDatoMisTramitesOperationCompleted, userState);
    }
    
    private void OnCrearDatoMisTramitesOperationCompleted(object arg) {
        if ((this.CrearDatoMisTramitesCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.CrearDatoMisTramitesCompleted(this, new CrearDatoMisTramitesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ModificarDatosExpedienteTramite", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public bool ModificarDatosExpedienteTramite(string strTramiteAnterior, string strTramiteNuevo) {
        object[] results = this.Invoke("ModificarDatosExpedienteTramite", new object[] {
                    strTramiteAnterior,
                    strTramiteNuevo});
        return ((bool)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginModificarDatosExpedienteTramite(string strTramiteAnterior, string strTramiteNuevo, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("ModificarDatosExpedienteTramite", new object[] {
                    strTramiteAnterior,
                    strTramiteNuevo}, callback, asyncState);
    }
    
    /// <remarks/>
    public bool EndModificarDatosExpedienteTramite(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((bool)(results[0]));
    }
    
    /// <remarks/>
    public void ModificarDatosExpedienteTramiteAsync(string strTramiteAnterior, string strTramiteNuevo) {
        this.ModificarDatosExpedienteTramiteAsync(strTramiteAnterior, strTramiteNuevo, null);
    }
    
    /// <remarks/>
    public void ModificarDatosExpedienteTramiteAsync(string strTramiteAnterior, string strTramiteNuevo, object userState) {
        if ((this.ModificarDatosExpedienteTramiteOperationCompleted == null)) {
            this.ModificarDatosExpedienteTramiteOperationCompleted = new System.Threading.SendOrPostCallback(this.OnModificarDatosExpedienteTramiteOperationCompleted);
        }
        this.InvokeAsync("ModificarDatosExpedienteTramite", new object[] {
                    strTramiteAnterior,
                    strTramiteNuevo}, this.ModificarDatosExpedienteTramiteOperationCompleted, userState);
    }
    
    private void OnModificarDatosExpedienteTramiteOperationCompleted(object arg) {
        if ((this.ModificarDatosExpedienteTramiteCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.ModificarDatosExpedienteTramiteCompleted(this, new ModificarDatosExpedienteTramiteCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/AsociarExpedienteNumeroSilpa", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public void AsociarExpedienteNumeroSilpa(int autoridadId, string numeroExpediente, string tipoAsociacion, string[] numerosSilpa) {
        this.Invoke("AsociarExpedienteNumeroSilpa", new object[] {
                    autoridadId,
                    numeroExpediente,
                    tipoAsociacion,
                    numerosSilpa});
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginAsociarExpedienteNumeroSilpa(int autoridadId, string numeroExpediente, string tipoAsociacion, string[] numerosSilpa, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("AsociarExpedienteNumeroSilpa", new object[] {
                    autoridadId,
                    numeroExpediente,
                    tipoAsociacion,
                    numerosSilpa}, callback, asyncState);
    }
    
    /// <remarks/>
    public void EndAsociarExpedienteNumeroSilpa(System.IAsyncResult asyncResult) {
        this.EndInvoke(asyncResult);
    }
    
    /// <remarks/>
    public void AsociarExpedienteNumeroSilpaAsync(int autoridadId, string numeroExpediente, string tipoAsociacion, string[] numerosSilpa) {
        this.AsociarExpedienteNumeroSilpaAsync(autoridadId, numeroExpediente, tipoAsociacion, numerosSilpa, null);
    }
    
    /// <remarks/>
    public void AsociarExpedienteNumeroSilpaAsync(int autoridadId, string numeroExpediente, string tipoAsociacion, string[] numerosSilpa, object userState) {
        if ((this.AsociarExpedienteNumeroSilpaOperationCompleted == null)) {
            this.AsociarExpedienteNumeroSilpaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAsociarExpedienteNumeroSilpaOperationCompleted);
        }
        this.InvokeAsync("AsociarExpedienteNumeroSilpa", new object[] {
                    autoridadId,
                    numeroExpediente,
                    tipoAsociacion,
                    numerosSilpa}, this.AsociarExpedienteNumeroSilpaOperationCompleted, userState);
    }
    
    private void OnAsociarExpedienteNumeroSilpaOperationCompleted(object arg) {
        if ((this.AsociarExpedienteNumeroSilpaCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.AsociarExpedienteNumeroSilpaCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/AsociarInfoExpedienteNumeroSilpa", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public void AsociarInfoExpedienteNumeroSilpa(string codigoExpediente, string numeroVital, string nombreExpediente, string descripcionExpediente, string sectorId, string[] ubicacionExpediente, string[] localizacionExpediente) {
        this.Invoke("AsociarInfoExpedienteNumeroSilpa", new object[] {
                    codigoExpediente,
                    numeroVital,
                    nombreExpediente,
                    descripcionExpediente,
                    sectorId,
                    ubicacionExpediente,
                    localizacionExpediente});
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginAsociarInfoExpedienteNumeroSilpa(string codigoExpediente, string numeroVital, string nombreExpediente, string descripcionExpediente, string sectorId, string[] ubicacionExpediente, string[] localizacionExpediente, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("AsociarInfoExpedienteNumeroSilpa", new object[] {
                    codigoExpediente,
                    numeroVital,
                    nombreExpediente,
                    descripcionExpediente,
                    sectorId,
                    ubicacionExpediente,
                    localizacionExpediente}, callback, asyncState);
    }
    
    /// <remarks/>
    public void EndAsociarInfoExpedienteNumeroSilpa(System.IAsyncResult asyncResult) {
        this.EndInvoke(asyncResult);
    }
    
    /// <remarks/>
    public void AsociarInfoExpedienteNumeroSilpaAsync(string codigoExpediente, string numeroVital, string nombreExpediente, string descripcionExpediente, string sectorId, string[] ubicacionExpediente, string[] localizacionExpediente) {
        this.AsociarInfoExpedienteNumeroSilpaAsync(codigoExpediente, numeroVital, nombreExpediente, descripcionExpediente, sectorId, ubicacionExpediente, localizacionExpediente, null);
    }
    
    /// <remarks/>
    public void AsociarInfoExpedienteNumeroSilpaAsync(string codigoExpediente, string numeroVital, string nombreExpediente, string descripcionExpediente, string sectorId, string[] ubicacionExpediente, string[] localizacionExpediente, object userState) {
        if ((this.AsociarInfoExpedienteNumeroSilpaOperationCompleted == null)) {
            this.AsociarInfoExpedienteNumeroSilpaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAsociarInfoExpedienteNumeroSilpaOperationCompleted);
        }
        this.InvokeAsync("AsociarInfoExpedienteNumeroSilpa", new object[] {
                    codigoExpediente,
                    numeroVital,
                    nombreExpediente,
                    descripcionExpediente,
                    sectorId,
                    ubicacionExpediente,
                    localizacionExpediente}, this.AsociarInfoExpedienteNumeroSilpaOperationCompleted, userState);
    }
    
    private void OnAsociarInfoExpedienteNumeroSilpaOperationCompleted(object arg) {
        if ((this.AsociarInfoExpedienteNumeroSilpaCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.AsociarInfoExpedienteNumeroSilpaCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ActivatExpedienteNumeroSilpa", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public void ActivatExpedienteNumeroSilpa(int autoridadId, string numeroExpediente) {
        this.Invoke("ActivatExpedienteNumeroSilpa", new object[] {
                    autoridadId,
                    numeroExpediente});
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginActivatExpedienteNumeroSilpa(int autoridadId, string numeroExpediente, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("ActivatExpedienteNumeroSilpa", new object[] {
                    autoridadId,
                    numeroExpediente}, callback, asyncState);
    }
    
    /// <remarks/>
    public void EndActivatExpedienteNumeroSilpa(System.IAsyncResult asyncResult) {
        this.EndInvoke(asyncResult);
    }
    
    /// <remarks/>
    public void ActivatExpedienteNumeroSilpaAsync(int autoridadId, string numeroExpediente) {
        this.ActivatExpedienteNumeroSilpaAsync(autoridadId, numeroExpediente, null);
    }
    
    /// <remarks/>
    public void ActivatExpedienteNumeroSilpaAsync(int autoridadId, string numeroExpediente, object userState) {
        if ((this.ActivatExpedienteNumeroSilpaOperationCompleted == null)) {
            this.ActivatExpedienteNumeroSilpaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnActivatExpedienteNumeroSilpaOperationCompleted);
        }
        this.InvokeAsync("ActivatExpedienteNumeroSilpa", new object[] {
                    autoridadId,
                    numeroExpediente}, this.ActivatExpedienteNumeroSilpaOperationCompleted, userState);
    }
    
    private void OnActivatExpedienteNumeroSilpaOperationCompleted(object arg) {
        if ((this.ActivatExpedienteNumeroSilpaCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.ActivatExpedienteNumeroSilpaCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ObtenerDocumentosRadicacion", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string ObtenerDocumentosRadicacion(long idRadicacion) {
        object[] results = this.Invoke("ObtenerDocumentosRadicacion", new object[] {
                    idRadicacion});
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginObtenerDocumentosRadicacion(long idRadicacion, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("ObtenerDocumentosRadicacion", new object[] {
                    idRadicacion}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndObtenerDocumentosRadicacion(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void ObtenerDocumentosRadicacionAsync(long idRadicacion) {
        this.ObtenerDocumentosRadicacionAsync(idRadicacion, null);
    }
    
    /// <remarks/>
    public void ObtenerDocumentosRadicacionAsync(long idRadicacion, object userState) {
        if ((this.ObtenerDocumentosRadicacionOperationCompleted == null)) {
            this.ObtenerDocumentosRadicacionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnObtenerDocumentosRadicacionOperationCompleted);
        }
        this.InvokeAsync("ObtenerDocumentosRadicacion", new object[] {
                    idRadicacion}, this.ObtenerDocumentosRadicacionOperationCompleted, userState);
    }
    
    private void OnObtenerDocumentosRadicacionOperationCompleted(object arg) {
        if ((this.ObtenerDocumentosRadicacionCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.ObtenerDocumentosRadicacionCompleted(this, new ObtenerDocumentosRadicacionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ObtenerDocumentosNUR", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string ObtenerDocumentosNUR(string strNUR) {
        object[] results = this.Invoke("ObtenerDocumentosNUR", new object[] {
                    strNUR});
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginObtenerDocumentosNUR(string strNUR, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("ObtenerDocumentosNUR", new object[] {
                    strNUR}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndObtenerDocumentosNUR(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void ObtenerDocumentosNURAsync(string strNUR) {
        this.ObtenerDocumentosNURAsync(strNUR, null);
    }
    
    /// <remarks/>
    public void ObtenerDocumentosNURAsync(string strNUR, object userState) {
        if ((this.ObtenerDocumentosNUROperationCompleted == null)) {
            this.ObtenerDocumentosNUROperationCompleted = new System.Threading.SendOrPostCallback(this.OnObtenerDocumentosNUROperationCompleted);
        }
        this.InvokeAsync("ObtenerDocumentosNUR", new object[] {
                    strNUR}, this.ObtenerDocumentosNUROperationCompleted, userState);
    }
    
    private void OnObtenerDocumentosNUROperationCompleted(object arg) {
        if ((this.ObtenerDocumentosNURCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.ObtenerDocumentosNURCompleted(this, new ObtenerDocumentosNURCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ObtenerDocumentoRadicacion", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    [return: System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
    public byte[] ObtenerDocumentoRadicacion(long idRadicacion, string nombreArchivo) {
        object[] results = this.Invoke("ObtenerDocumentoRadicacion", new object[] {
                    idRadicacion,
                    nombreArchivo});
        return ((byte[])(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginObtenerDocumentoRadicacion(long idRadicacion, string nombreArchivo, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("ObtenerDocumentoRadicacion", new object[] {
                    idRadicacion,
                    nombreArchivo}, callback, asyncState);
    }
    
    /// <remarks/>
    public byte[] EndObtenerDocumentoRadicacion(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((byte[])(results[0]));
    }
    
    /// <remarks/>
    public void ObtenerDocumentoRadicacionAsync(long idRadicacion, string nombreArchivo) {
        this.ObtenerDocumentoRadicacionAsync(idRadicacion, nombreArchivo, null);
    }
    
    /// <remarks/>
    public void ObtenerDocumentoRadicacionAsync(long idRadicacion, string nombreArchivo, object userState) {
        if ((this.ObtenerDocumentoRadicacionOperationCompleted == null)) {
            this.ObtenerDocumentoRadicacionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnObtenerDocumentoRadicacionOperationCompleted);
        }
        this.InvokeAsync("ObtenerDocumentoRadicacion", new object[] {
                    idRadicacion,
                    nombreArchivo}, this.ObtenerDocumentoRadicacionOperationCompleted, userState);
    }
    
    private void OnObtenerDocumentoRadicacionOperationCompleted(object arg) {
        if ((this.ObtenerDocumentoRadicacionCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.ObtenerDocumentoRadicacionCompleted(this, new ObtenerDocumentoRadicacionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ObtenerPathRadicacion", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string ObtenerPathRadicacion(long idRadicacion, string nombreArchivo) {
        object[] results = this.Invoke("ObtenerPathRadicacion", new object[] {
                    idRadicacion,
                    nombreArchivo});
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginObtenerPathRadicacion(long idRadicacion, string nombreArchivo, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("ObtenerPathRadicacion", new object[] {
                    idRadicacion,
                    nombreArchivo}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndObtenerPathRadicacion(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void ObtenerPathRadicacionAsync(long idRadicacion, string nombreArchivo) {
        this.ObtenerPathRadicacionAsync(idRadicacion, nombreArchivo, null);
    }
    
    /// <remarks/>
    public void ObtenerPathRadicacionAsync(long idRadicacion, string nombreArchivo, object userState) {
        if ((this.ObtenerPathRadicacionOperationCompleted == null)) {
            this.ObtenerPathRadicacionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnObtenerPathRadicacionOperationCompleted);
        }
        this.InvokeAsync("ObtenerPathRadicacion", new object[] {
                    idRadicacion,
                    nombreArchivo}, this.ObtenerPathRadicacionOperationCompleted, userState);
    }
    
    private void OnObtenerPathRadicacionOperationCompleted(object arg) {
        if ((this.ObtenerPathRadicacionCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.ObtenerPathRadicacionCompleted(this, new ObtenerPathRadicacionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ObtenerPathCorrespondencia", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string ObtenerPathCorrespondencia(string NUR, string nombreArchivo) {
        object[] results = this.Invoke("ObtenerPathCorrespondencia", new object[] {
                    NUR,
                    nombreArchivo});
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginObtenerPathCorrespondencia(string NUR, string nombreArchivo, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("ObtenerPathCorrespondencia", new object[] {
                    NUR,
                    nombreArchivo}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndObtenerPathCorrespondencia(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void ObtenerPathCorrespondenciaAsync(string NUR, string nombreArchivo) {
        this.ObtenerPathCorrespondenciaAsync(NUR, nombreArchivo, null);
    }
    
    /// <remarks/>
    public void ObtenerPathCorrespondenciaAsync(string NUR, string nombreArchivo, object userState) {
        if ((this.ObtenerPathCorrespondenciaOperationCompleted == null)) {
            this.ObtenerPathCorrespondenciaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnObtenerPathCorrespondenciaOperationCompleted);
        }
        this.InvokeAsync("ObtenerPathCorrespondencia", new object[] {
                    NUR,
                    nombreArchivo}, this.ObtenerPathCorrespondenciaOperationCompleted, userState);
    }
    
    private void OnObtenerPathCorrespondenciaOperationCompleted(object arg) {
        if ((this.ObtenerPathCorrespondenciaCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.ObtenerPathCorrespondenciaCompleted(this, new ObtenerPathCorrespondenciaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ObtenerPathFus", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string ObtenerPathFus(long int_id_radicacion) {
        object[] results = this.Invoke("ObtenerPathFus", new object[] {
                    int_id_radicacion});
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginObtenerPathFus(long int_id_radicacion, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("ObtenerPathFus", new object[] {
                    int_id_radicacion}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndObtenerPathFus(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void ObtenerPathFusAsync(long int_id_radicacion) {
        this.ObtenerPathFusAsync(int_id_radicacion, null);
    }
    
    /// <remarks/>
    public void ObtenerPathFusAsync(long int_id_radicacion, object userState) {
        if ((this.ObtenerPathFusOperationCompleted == null)) {
            this.ObtenerPathFusOperationCompleted = new System.Threading.SendOrPostCallback(this.OnObtenerPathFusOperationCompleted);
        }
        this.InvokeAsync("ObtenerPathFus", new object[] {
                    int_id_radicacion}, this.ObtenerPathFusOperationCompleted, userState);
    }
    
    private void OnObtenerPathFusOperationCompleted(object arg) {
        if ((this.ObtenerPathFusCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.ObtenerPathFusCompleted(this, new ObtenerPathFusCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ObtenerPathDocumentos", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string ObtenerPathDocumentos(long int_id_radicacion) {
        object[] results = this.Invoke("ObtenerPathDocumentos", new object[] {
                    int_id_radicacion});
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginObtenerPathDocumentos(long int_id_radicacion, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("ObtenerPathDocumentos", new object[] {
                    int_id_radicacion}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndObtenerPathDocumentos(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void ObtenerPathDocumentosAsync(long int_id_radicacion) {
        this.ObtenerPathDocumentosAsync(int_id_radicacion, null);
    }
    
    /// <remarks/>
    public void ObtenerPathDocumentosAsync(long int_id_radicacion, object userState) {
        if ((this.ObtenerPathDocumentosOperationCompleted == null)) {
            this.ObtenerPathDocumentosOperationCompleted = new System.Threading.SendOrPostCallback(this.OnObtenerPathDocumentosOperationCompleted);
        }
        this.InvokeAsync("ObtenerPathDocumentos", new object[] {
                    int_id_radicacion}, this.ObtenerPathDocumentosOperationCompleted, userState);
    }
    
    private void OnObtenerPathDocumentosOperationCompleted(object arg) {
        if ((this.ObtenerPathDocumentosCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.ObtenerPathDocumentosCompleted(this, new ObtenerPathDocumentosCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/EnviarDocumentoRadicacion", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string EnviarDocumentoRadicacion(long idRadicacion, string nombreArchivo, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] bytesArchivo) {
        object[] results = this.Invoke("EnviarDocumentoRadicacion", new object[] {
                    idRadicacion,
                    nombreArchivo,
                    bytesArchivo});
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginEnviarDocumentoRadicacion(long idRadicacion, string nombreArchivo, byte[] bytesArchivo, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("EnviarDocumentoRadicacion", new object[] {
                    idRadicacion,
                    nombreArchivo,
                    bytesArchivo}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndEnviarDocumentoRadicacion(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void EnviarDocumentoRadicacionAsync(long idRadicacion, string nombreArchivo, byte[] bytesArchivo) {
        this.EnviarDocumentoRadicacionAsync(idRadicacion, nombreArchivo, bytesArchivo, null);
    }
    
    /// <remarks/>
    public void EnviarDocumentoRadicacionAsync(long idRadicacion, string nombreArchivo, byte[] bytesArchivo, object userState) {
        if ((this.EnviarDocumentoRadicacionOperationCompleted == null)) {
            this.EnviarDocumentoRadicacionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnEnviarDocumentoRadicacionOperationCompleted);
        }
        this.InvokeAsync("EnviarDocumentoRadicacion", new object[] {
                    idRadicacion,
                    nombreArchivo,
                    bytesArchivo}, this.EnviarDocumentoRadicacionOperationCompleted, userState);
    }
    
    private void OnEnviarDocumentoRadicacionOperationCompleted(object arg) {
        if ((this.EnviarDocumentoRadicacionCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.EnviarDocumentoRadicacionCompleted(this, new EnviarDocumentoRadicacionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/EnviarDocumentoRadicacionAlterno", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string EnviarDocumentoRadicacionAlterno(long idRadicacion, string nombreArchivo, out string rutaArchivos) {
        object[] results = this.Invoke("EnviarDocumentoRadicacionAlterno", new object[] {
                    idRadicacion,
                    nombreArchivo});
        rutaArchivos = ((string)(results[1]));
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginEnviarDocumentoRadicacionAlterno(long idRadicacion, string nombreArchivo, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("EnviarDocumentoRadicacionAlterno", new object[] {
                    idRadicacion,
                    nombreArchivo}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndEnviarDocumentoRadicacionAlterno(System.IAsyncResult asyncResult, out string rutaArchivos) {
        object[] results = this.EndInvoke(asyncResult);
        rutaArchivos = ((string)(results[1]));
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void EnviarDocumentoRadicacionAlternoAsync(long idRadicacion, string nombreArchivo) {
        this.EnviarDocumentoRadicacionAlternoAsync(idRadicacion, nombreArchivo, null);
    }
    
    /// <remarks/>
    public void EnviarDocumentoRadicacionAlternoAsync(long idRadicacion, string nombreArchivo, object userState) {
        if ((this.EnviarDocumentoRadicacionAlternoOperationCompleted == null)) {
            this.EnviarDocumentoRadicacionAlternoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnEnviarDocumentoRadicacionAlternoOperationCompleted);
        }
        this.InvokeAsync("EnviarDocumentoRadicacionAlterno", new object[] {
                    idRadicacion,
                    nombreArchivo}, this.EnviarDocumentoRadicacionAlternoOperationCompleted, userState);
    }
    
    private void OnEnviarDocumentoRadicacionAlternoOperationCompleted(object arg) {
        if ((this.EnviarDocumentoRadicacionAlternoCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.EnviarDocumentoRadicacionAlternoCompleted(this, new EnviarDocumentoRadicacionAlternoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/EnviarDocumentoCorrespondencia", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string EnviarDocumentoCorrespondencia(string NUR, string nombreArchivo, string strCarpetaNURs, out string rutaArchivos) {
        object[] results = this.Invoke("EnviarDocumentoCorrespondencia", new object[] {
                    NUR,
                    nombreArchivo,
                    strCarpetaNURs});
        rutaArchivos = ((string)(results[1]));
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginEnviarDocumentoCorrespondencia(string NUR, string nombreArchivo, string strCarpetaNURs, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("EnviarDocumentoCorrespondencia", new object[] {
                    NUR,
                    nombreArchivo,
                    strCarpetaNURs}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndEnviarDocumentoCorrespondencia(System.IAsyncResult asyncResult, out string rutaArchivos) {
        object[] results = this.EndInvoke(asyncResult);
        rutaArchivos = ((string)(results[1]));
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void EnviarDocumentoCorrespondenciaAsync(string NUR, string nombreArchivo, string strCarpetaNURs) {
        this.EnviarDocumentoCorrespondenciaAsync(NUR, nombreArchivo, strCarpetaNURs, null);
    }
    
    /// <remarks/>
    public void EnviarDocumentoCorrespondenciaAsync(string NUR, string nombreArchivo, string strCarpetaNURs, object userState) {
        if ((this.EnviarDocumentoCorrespondenciaOperationCompleted == null)) {
            this.EnviarDocumentoCorrespondenciaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnEnviarDocumentoCorrespondenciaOperationCompleted);
        }
        this.InvokeAsync("EnviarDocumentoCorrespondencia", new object[] {
                    NUR,
                    nombreArchivo,
                    strCarpetaNURs}, this.EnviarDocumentoCorrespondenciaOperationCompleted, userState);
    }
    
    private void OnEnviarDocumentoCorrespondenciaOperationCompleted(object arg) {
        if ((this.EnviarDocumentoCorrespondenciaCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.EnviarDocumentoCorrespondenciaCompleted(this, new EnviarDocumentoCorrespondenciaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/RelacionarExpedientesPadreHijo", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public void RelacionarExpedientesPadreHijo(string codigo_Expediente, string padre, string hijo, int tipoTramite) {
        this.Invoke("RelacionarExpedientesPadreHijo", new object[] {
                    codigo_Expediente,
                    padre,
                    hijo,
                    tipoTramite});
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginRelacionarExpedientesPadreHijo(string codigo_Expediente, string padre, string hijo, int tipoTramite, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("RelacionarExpedientesPadreHijo", new object[] {
                    codigo_Expediente,
                    padre,
                    hijo,
                    tipoTramite}, callback, asyncState);
    }
    
    /// <remarks/>
    public void EndRelacionarExpedientesPadreHijo(System.IAsyncResult asyncResult) {
        this.EndInvoke(asyncResult);
    }
    
    /// <remarks/>
    public void RelacionarExpedientesPadreHijoAsync(string codigo_Expediente, string padre, string hijo, int tipoTramite) {
        this.RelacionarExpedientesPadreHijoAsync(codigo_Expediente, padre, hijo, tipoTramite, null);
    }
    
    /// <remarks/>
    public void RelacionarExpedientesPadreHijoAsync(string codigo_Expediente, string padre, string hijo, int tipoTramite, object userState) {
        if ((this.RelacionarExpedientesPadreHijoOperationCompleted == null)) {
            this.RelacionarExpedientesPadreHijoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRelacionarExpedientesPadreHijoOperationCompleted);
        }
        this.InvokeAsync("RelacionarExpedientesPadreHijo", new object[] {
                    codigo_Expediente,
                    padre,
                    hijo,
                    tipoTramite}, this.RelacionarExpedientesPadreHijoOperationCompleted, userState);
    }
    
    private void OnRelacionarExpedientesPadreHijoOperationCompleted(object arg) {
        if ((this.RelacionarExpedientesPadreHijoCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.RelacionarExpedientesPadreHijoCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/CrearSolicitudManual", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public void CrearSolicitudManual(int AutoridadID, int ExpedienteID, string CodigoExpediente, int SectorID, int PersonaID, string NumeroVITAL, string NumeroRadicado, int TramiteID, string NombreProyecto) {
        this.Invoke("CrearSolicitudManual", new object[] {
                    AutoridadID,
                    ExpedienteID,
                    CodigoExpediente,
                    SectorID,
                    PersonaID,
                    NumeroVITAL,
                    NumeroRadicado,
                    TramiteID,
                    NombreProyecto});
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginCrearSolicitudManual(int AutoridadID, int ExpedienteID, string CodigoExpediente, int SectorID, int PersonaID, string NumeroVITAL, string NumeroRadicado, int TramiteID, string NombreProyecto, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("CrearSolicitudManual", new object[] {
                    AutoridadID,
                    ExpedienteID,
                    CodigoExpediente,
                    SectorID,
                    PersonaID,
                    NumeroVITAL,
                    NumeroRadicado,
                    TramiteID,
                    NombreProyecto}, callback, asyncState);
    }
    
    /// <remarks/>
    public void EndCrearSolicitudManual(System.IAsyncResult asyncResult) {
        this.EndInvoke(asyncResult);
    }
    
    /// <remarks/>
    public void CrearSolicitudManualAsync(int AutoridadID, int ExpedienteID, string CodigoExpediente, int SectorID, int PersonaID, string NumeroVITAL, string NumeroRadicado, int TramiteID, string NombreProyecto) {
        this.CrearSolicitudManualAsync(AutoridadID, ExpedienteID, CodigoExpediente, SectorID, PersonaID, NumeroVITAL, NumeroRadicado, TramiteID, NombreProyecto, null);
    }
    
    /// <remarks/>
    public void CrearSolicitudManualAsync(int AutoridadID, int ExpedienteID, string CodigoExpediente, int SectorID, int PersonaID, string NumeroVITAL, string NumeroRadicado, int TramiteID, string NombreProyecto, object userState) {
        if ((this.CrearSolicitudManualOperationCompleted == null)) {
            this.CrearSolicitudManualOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCrearSolicitudManualOperationCompleted);
        }
        this.InvokeAsync("CrearSolicitudManual", new object[] {
                    AutoridadID,
                    ExpedienteID,
                    CodigoExpediente,
                    SectorID,
                    PersonaID,
                    NumeroVITAL,
                    NumeroRadicado,
                    TramiteID,
                    NombreProyecto}, this.CrearSolicitudManualOperationCompleted, userState);
    }
    
    private void OnCrearSolicitudManualOperationCompleted(object arg) {
        if ((this.CrearSolicitudManualCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.CrearSolicitudManualCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ObtenerNumeroVITALPadreSolicitud", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string ObtenerNumeroVITALPadreSolicitud(string strNumeroVital) {
        object[] results = this.Invoke("ObtenerNumeroVITALPadreSolicitud", new object[] {
                    strNumeroVital});
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginObtenerNumeroVITALPadreSolicitud(string strNumeroVital, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("ObtenerNumeroVITALPadreSolicitud", new object[] {
                    strNumeroVital}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndObtenerNumeroVITALPadreSolicitud(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void ObtenerNumeroVITALPadreSolicitudAsync(string strNumeroVital) {
        this.ObtenerNumeroVITALPadreSolicitudAsync(strNumeroVital, null);
    }
    
    /// <remarks/>
    public void ObtenerNumeroVITALPadreSolicitudAsync(string strNumeroVital, object userState) {
        if ((this.ObtenerNumeroVITALPadreSolicitudOperationCompleted == null)) {
            this.ObtenerNumeroVITALPadreSolicitudOperationCompleted = new System.Threading.SendOrPostCallback(this.OnObtenerNumeroVITALPadreSolicitudOperationCompleted);
        }
        this.InvokeAsync("ObtenerNumeroVITALPadreSolicitud", new object[] {
                    strNumeroVital}, this.ObtenerNumeroVITALPadreSolicitudOperationCompleted, userState);
    }
    
    private void OnObtenerNumeroVITALPadreSolicitudOperationCompleted(object arg) {
        if ((this.ObtenerNumeroVITALPadreSolicitudCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.ObtenerNumeroVITALPadreSolicitudCompleted(this, new ObtenerNumeroVITALPadreSolicitudCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ObtenerAutoridadesSolicitud", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string ObtenerAutoridadesSolicitud(string numeroVital) {
        object[] results = this.Invoke("ObtenerAutoridadesSolicitud", new object[] {
                    numeroVital});
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginObtenerAutoridadesSolicitud(string numeroVital, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("ObtenerAutoridadesSolicitud", new object[] {
                    numeroVital}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndObtenerAutoridadesSolicitud(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void ObtenerAutoridadesSolicitudAsync(string numeroVital) {
        this.ObtenerAutoridadesSolicitudAsync(numeroVital, null);
    }
    
    /// <remarks/>
    public void ObtenerAutoridadesSolicitudAsync(string numeroVital, object userState) {
        if ((this.ObtenerAutoridadesSolicitudOperationCompleted == null)) {
            this.ObtenerAutoridadesSolicitudOperationCompleted = new System.Threading.SendOrPostCallback(this.OnObtenerAutoridadesSolicitudOperationCompleted);
        }
        this.InvokeAsync("ObtenerAutoridadesSolicitud", new object[] {
                    numeroVital}, this.ObtenerAutoridadesSolicitudOperationCompleted, userState);
    }
    
    private void OnObtenerAutoridadesSolicitudOperationCompleted(object arg) {
        if ((this.ObtenerAutoridadesSolicitudCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.ObtenerAutoridadesSolicitudCompleted(this, new ObtenerAutoridadesSolicitudCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ObtenerComunidadesSolicitud", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string ObtenerComunidadesSolicitud(string numeroVital) {
        object[] results = this.Invoke("ObtenerComunidadesSolicitud", new object[] {
                    numeroVital});
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginObtenerComunidadesSolicitud(string numeroVital, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("ObtenerComunidadesSolicitud", new object[] {
                    numeroVital}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndObtenerComunidadesSolicitud(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void ObtenerComunidadesSolicitudAsync(string numeroVital) {
        this.ObtenerComunidadesSolicitudAsync(numeroVital, null);
    }
    
    /// <remarks/>
    public void ObtenerComunidadesSolicitudAsync(string numeroVital, object userState) {
        if ((this.ObtenerComunidadesSolicitudOperationCompleted == null)) {
            this.ObtenerComunidadesSolicitudOperationCompleted = new System.Threading.SendOrPostCallback(this.OnObtenerComunidadesSolicitudOperationCompleted);
        }
        this.InvokeAsync("ObtenerComunidadesSolicitud", new object[] {
                    numeroVital}, this.ObtenerComunidadesSolicitudOperationCompleted, userState);
    }
    
    private void OnObtenerComunidadesSolicitudOperationCompleted(object arg) {
        if ((this.ObtenerComunidadesSolicitudCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.ObtenerComunidadesSolicitudCompleted(this, new ObtenerComunidadesSolicitudCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Test", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public void Test() {
        this.Invoke("Test", new object[0]);
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginTest(System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("Test", new object[0], callback, asyncState);
    }
    
    /// <remarks/>
    public void EndTest(System.IAsyncResult asyncResult) {
        this.EndInvoke(asyncResult);
    }
    
    /// <remarks/>
    public void TestAsync() {
        this.TestAsync(null);
    }
    
    /// <remarks/>
    public void TestAsync(object userState) {
        if ((this.TestOperationCompleted == null)) {
            this.TestOperationCompleted = new System.Threading.SendOrPostCallback(this.OnTestOperationCompleted);
        }
        this.InvokeAsync("Test", new object[0], this.TestOperationCompleted, userState);
    }
    
    private void OnTestOperationCompleted(object arg) {
        if ((this.TestCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.TestCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    public new void CancelAsync(object userState) {
        base.CancelAsync(userState);
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Notificacion.xsd")]
public partial class NotificacionType {
    
    private string codigoAutoridadAmbientalField;
    
    private string codigoExpedienteField;
    
    private string nombreArchivoField;
    
    private string numActoAdministrativoField;
    
    private System.DateTime fechaActoAdministrativoField;
    
    private string numProcesoAdministracionField;
    
    private string parteResolutivaField;
    
    private enumTipoIdentificacion tipoIdentificacionFuncionarioField;
    
    private string numeroIdentificacionFuncionarioField;
    
    private string numActoAdministrativoAsociadoField;
    
    private string numSILPAField;
    
    private PersonaType[] listaPersonasField;
    
    private PersonaType[] listaPersonaComunicarField;
    
    private bool requierePublicacionField;
    
    private string tipoActoAdministrativoField;
    
    private bool requiereNotificacionField;
    
    private bool esEjecutoriaField;
    
    private string entidadPublicaNotField;
    
    private string sistemaEntidadPublicaNotField;
    
    private string idDependenciaEntidadField;
    
    private string idPlantillaNotificacionField;
    
    private System.Nullable<bool> esSancionatorioField;
    
    private System.Nullable<bool> esNotificacionField;
    
    private System.Nullable<bool> esAudienciaField;
    
    private string correoAutAmbNotificacionField;
    
    private string tramiteField;
    
    private string solicitanteField;
    
    private System.Nullable<bool> aplicaRecursoField;
    
    private System.Nullable<bool> esComunicacionField;
    
    private System.Nullable<bool> esCumplaseField;
    
    private System.Nullable<bool> esNotificacionEdictoField;
    
    private System.Nullable<bool> esNotificacionEstradoField;
    
    private System.Nullable<bool> esCobroField;
    
    private System.Nullable<int> cobroIdentificadorField;
    
    private byte[] datosArchivoField;
    
    /// <remarks/>
    public string CodigoAutoridadAmbiental {
        get {
            return this.codigoAutoridadAmbientalField;
        }
        set {
            this.codigoAutoridadAmbientalField = value;
        }
    }
    
    /// <remarks/>
    public string CodigoExpediente {
        get {
            return this.codigoExpedienteField;
        }
        set {
            this.codigoExpedienteField = value;
        }
    }
    
    /// <remarks/>
    public string nombreArchivo {
        get {
            return this.nombreArchivoField;
        }
        set {
            this.nombreArchivoField = value;
        }
    }
    
    /// <remarks/>
    public string numActoAdministrativo {
        get {
            return this.numActoAdministrativoField;
        }
        set {
            this.numActoAdministrativoField = value;
        }
    }
    
    /// <remarks/>
    public System.DateTime fechaActoAdministrativo {
        get {
            return this.fechaActoAdministrativoField;
        }
        set {
            this.fechaActoAdministrativoField = value;
        }
    }
    
    /// <remarks/>
    public string numProcesoAdministracion {
        get {
            return this.numProcesoAdministracionField;
        }
        set {
            this.numProcesoAdministracionField = value;
        }
    }
    
    /// <remarks/>
    public string parteResolutiva {
        get {
            return this.parteResolutivaField;
        }
        set {
            this.parteResolutivaField = value;
        }
    }
    
    /// <remarks/>
    public enumTipoIdentificacion tipoIdentificacionFuncionario {
        get {
            return this.tipoIdentificacionFuncionarioField;
        }
        set {
            this.tipoIdentificacionFuncionarioField = value;
        }
    }
    
    /// <remarks/>
    public string numeroIdentificacionFuncionario {
        get {
            return this.numeroIdentificacionFuncionarioField;
        }
        set {
            this.numeroIdentificacionFuncionarioField = value;
        }
    }
    
    /// <remarks/>
    public string numActoAdministrativoAsociado {
        get {
            return this.numActoAdministrativoAsociadoField;
        }
        set {
            this.numActoAdministrativoAsociadoField = value;
        }
    }
    
    /// <remarks/>
    public string numSILPA {
        get {
            return this.numSILPAField;
        }
        set {
            this.numSILPAField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("listaPersonas")]
    public PersonaType[] listaPersonas {
        get {
            return this.listaPersonasField;
        }
        set {
            this.listaPersonasField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("listaPersonaComunicar")]
    public PersonaType[] listaPersonaComunicar {
        get {
            return this.listaPersonaComunicarField;
        }
        set {
            this.listaPersonaComunicarField = value;
        }
    }
    
    /// <remarks/>
    public bool requierePublicacion {
        get {
            return this.requierePublicacionField;
        }
        set {
            this.requierePublicacionField = value;
        }
    }
    
    /// <remarks/>
    public string tipoActoAdministrativo {
        get {
            return this.tipoActoAdministrativoField;
        }
        set {
            this.tipoActoAdministrativoField = value;
        }
    }
    
    /// <remarks/>
    public bool requiereNotificacion {
        get {
            return this.requiereNotificacionField;
        }
        set {
            this.requiereNotificacionField = value;
        }
    }
    
    /// <remarks/>
    public bool esEjecutoria {
        get {
            return this.esEjecutoriaField;
        }
        set {
            this.esEjecutoriaField = value;
        }
    }
    
    /// <remarks/>
    public string EntidadPublicaNot {
        get {
            return this.entidadPublicaNotField;
        }
        set {
            this.entidadPublicaNotField = value;
        }
    }
    
    /// <remarks/>
    public string SistemaEntidadPublicaNot {
        get {
            return this.sistemaEntidadPublicaNotField;
        }
        set {
            this.sistemaEntidadPublicaNotField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
    public string IdDependenciaEntidad {
        get {
            return this.idDependenciaEntidadField;
        }
        set {
            this.idDependenciaEntidadField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
    public string IdPlantillaNotificacion {
        get {
            return this.idPlantillaNotificacionField;
        }
        set {
            this.idPlantillaNotificacionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public System.Nullable<bool> esSancionatorio {
        get {
            return this.esSancionatorioField;
        }
        set {
            this.esSancionatorioField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public System.Nullable<bool> esNotificacion {
        get {
            return this.esNotificacionField;
        }
        set {
            this.esNotificacionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public System.Nullable<bool> esAudiencia {
        get {
            return this.esAudienciaField;
        }
        set {
            this.esAudienciaField = value;
        }
    }
    
    /// <remarks/>
    public string CorreoAutAmbNotificacion {
        get {
            return this.correoAutAmbNotificacionField;
        }
        set {
            this.correoAutAmbNotificacionField = value;
        }
    }
    
    /// <remarks/>
    public string Tramite {
        get {
            return this.tramiteField;
        }
        set {
            this.tramiteField = value;
        }
    }
    
    /// <remarks/>
    public string Solicitante {
        get {
            return this.solicitanteField;
        }
        set {
            this.solicitanteField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public System.Nullable<bool> aplicaRecurso {
        get {
            return this.aplicaRecursoField;
        }
        set {
            this.aplicaRecursoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public System.Nullable<bool> esComunicacion {
        get {
            return this.esComunicacionField;
        }
        set {
            this.esComunicacionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public System.Nullable<bool> esCumplase {
        get {
            return this.esCumplaseField;
        }
        set {
            this.esCumplaseField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public System.Nullable<bool> esNotificacionEdicto {
        get {
            return this.esNotificacionEdictoField;
        }
        set {
            this.esNotificacionEdictoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public System.Nullable<bool> esNotificacionEstrado {
        get {
            return this.esNotificacionEstradoField;
        }
        set {
            this.esNotificacionEstradoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public System.Nullable<bool> esCobro {
        get {
            return this.esCobroField;
        }
        set {
            this.esCobroField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public System.Nullable<int> CobroIdentificador {
        get {
            return this.cobroIdentificadorField;
        }
        set {
            this.cobroIdentificadorField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
    public byte[] datosArchivo {
        get {
            return this.datosArchivoField;
        }
        set {
            this.datosArchivoField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Notificacion.xsd")]
public enum enumTipoIdentificacion {
    
    /// <remarks/>
    RE,
    
    /// <remarks/>
    TI,
    
    /// <remarks/>
    CC,
    
    /// <remarks/>
    CE,
    
    /// <remarks/>
    AS,
    
    /// <remarks/>
    MS,
    
    /// <remarks/>
    RN,
    
    /// <remarks/>
    PA,
    
    /// <remarks/>
    CX,
    
    /// <remarks/>
    NI,
    
    /// <remarks/>
    NU,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Notificacion.xsd")]
public partial class PersonaType {
    
    private string numeroIdentificacionField;
    
    private enumTipoIdentificacion ipoIdentificacionField;
    
    private enumTipoPersona tipoPersonaField;
    
    private int numeroNITField;
    
    private int digitoVerificacionNITField;
    
    private string primerApellidoField;
    
    private string segundoApellidoField;
    
    private string primerNombreField;
    
    private string segundoNombreField;
    
    private string razonSocialField;
    
    private EstadoNotificadoType estadoNotificadoField;
    
    private System.Nullable<System.DateTime> fechaEstadoNotificadoField;
    
    private System.Nullable<System.DateTime> fechaEstadoField;
    
    private string numeroSilpaField;
    
    /// <remarks/>
    public string numeroIdentificacion {
        get {
            return this.numeroIdentificacionField;
        }
        set {
            this.numeroIdentificacionField = value;
        }
    }
    
    /// <remarks/>
    public enumTipoIdentificacion ipoIdentificacion {
        get {
            return this.ipoIdentificacionField;
        }
        set {
            this.ipoIdentificacionField = value;
        }
    }
    
    /// <remarks/>
    public enumTipoPersona tipoPersona {
        get {
            return this.tipoPersonaField;
        }
        set {
            this.tipoPersonaField = value;
        }
    }
    
    /// <remarks/>
    public int numeroNIT {
        get {
            return this.numeroNITField;
        }
        set {
            this.numeroNITField = value;
        }
    }
    
    /// <remarks/>
    public int digitoVerificacionNIT {
        get {
            return this.digitoVerificacionNITField;
        }
        set {
            this.digitoVerificacionNITField = value;
        }
    }
    
    /// <remarks/>
    public string primerApellido {
        get {
            return this.primerApellidoField;
        }
        set {
            this.primerApellidoField = value;
        }
    }
    
    /// <remarks/>
    public string segundoApellido {
        get {
            return this.segundoApellidoField;
        }
        set {
            this.segundoApellidoField = value;
        }
    }
    
    /// <remarks/>
    public string primerNombre {
        get {
            return this.primerNombreField;
        }
        set {
            this.primerNombreField = value;
        }
    }
    
    /// <remarks/>
    public string segundoNombre {
        get {
            return this.segundoNombreField;
        }
        set {
            this.segundoNombreField = value;
        }
    }
    
    /// <remarks/>
    public string razonSocial {
        get {
            return this.razonSocialField;
        }
        set {
            this.razonSocialField = value;
        }
    }
    
    /// <remarks/>
    public EstadoNotificadoType estadoNotificado {
        get {
            return this.estadoNotificadoField;
        }
        set {
            this.estadoNotificadoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public System.Nullable<System.DateTime> fechaEstadoNotificado {
        get {
            return this.fechaEstadoNotificadoField;
        }
        set {
            this.fechaEstadoNotificadoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public System.Nullable<System.DateTime> FechaEstado {
        get {
            return this.fechaEstadoField;
        }
        set {
            this.fechaEstadoField = value;
        }
    }
    
    /// <remarks/>
    public string numeroSilpa {
        get {
            return this.numeroSilpaField;
        }
        set {
            this.numeroSilpaField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Notificacion.xsd")]
public enum enumTipoPersona {
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("1")]
    Item1,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("2")]
    Item2,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Notificacion.xsd")]
public partial class EstadoNotificadoType {
    
    private enumCodigoEstadoNotificacion codigoEstadoField;
    
    private enumNombreEstadoNotificacion nombreEstadoField;
    
    /// <remarks/>
    public enumCodigoEstadoNotificacion codigoEstado {
        get {
            return this.codigoEstadoField;
        }
        set {
            this.codigoEstadoField = value;
        }
    }
    
    /// <remarks/>
    public enumNombreEstadoNotificacion nombreEstado {
        get {
            return this.nombreEstadoField;
        }
        set {
            this.nombreEstadoField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Notificacion.xsd")]
public enum enumCodigoEstadoNotificacion {
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("1")]
    Item1,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("2")]
    Item2,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("3")]
    Item3,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("4")]
    Item4,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("5")]
    Item5,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("6")]
    Item6,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("7")]
    Item7,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("8")]
    Item8,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("9")]
    Item9,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("-1")]
    Item11,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("17")]
    Item17,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("18")]
    Item18,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("19")]
    Item19,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("20")]
    Item20,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("21")]
    Item21,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("22")]
    Item22,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("23")]
    Item23,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("68")]
    Item68,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Notificacion.xsd")]
public enum enumNombreEstadoNotificacion {
    
    /// <remarks/>
    PENDIENTE_DE_ACUSE_DE_NOTIFICACIÓN,
    
    /// <remarks/>
    NOTIFICADA,
    
    /// <remarks/>
    EN_EDICTO,
    
    /// <remarks/>
    CON_RECURSO_INTERPUESTO,
    
    /// <remarks/>
    EJECUTORIADO,
    
    /// <remarks/>
    CON_RENUNCIA_A_TÉRMINOS,
    
    /// <remarks/>
    SUSPENDIDO,
    
    /// <remarks/>
    REVOCADO,
    
    /// <remarks/>
    NO_EXISTE,
    
    /// <remarks/>
    CON_ERROR,
    
    /// <remarks/>
    SIN_INICIAR,
    
    /// <remarks/>
    FIN_DE_PROCESO,
    
    /// <remarks/>
    NOTIFICACION_POR_AVISO,
    
    /// <remarks/>
    NOTIFICACION_ELECTRONICA,
    
    /// <remarks/>
    NOTIFICACION_EN_ESTRADO,
    
    /// <remarks/>
    NOTIFICACION_CONDUCTA_CONCLUYENTE,
    
    /// <remarks/>
    NOTIFICACION_CORREO_ELECTRONICO,
    
    /// <remarks/>
    NOTIFICACION_FIRME,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void RecibirDocumentoCompletedEventHandler(object sender, RecibirDocumentoCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class RecibirDocumentoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal RecibirDocumentoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void RecibirDocumentoValidacionCompletedEventHandler(object sender, RecibirDocumentoValidacionCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class RecibirDocumentoValidacionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal RecibirDocumentoValidacionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void SolicitudXmlCompletedEventHandler(object sender, SolicitudXmlCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class SolicitudXmlCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal SolicitudXmlCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void EstructuraFormularioXmlCompletedEventHandler(object sender, EstructuraFormularioXmlCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class EstructuraFormularioXmlCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal EstructuraFormularioXmlCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void EjecutoriarActoCompletedEventHandler(object sender, EjecutoriarActoCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class EjecutoriarActoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal EjecutoriarActoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    
    /// <remarks/>
    public bool respuesta {
        get {
            this.RaiseExceptionIfNecessary();
            return ((bool)(this.results[1]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void EmitirDocumentoCompletedEventHandler(object sender, EmitirDocumentoCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class EmitirDocumentoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal EmitirDocumentoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void ObtenerDatosAdjuntosCompletedEventHandler(object sender, ObtenerDatosAdjuntosCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class ObtenerDatosAdjuntosCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal ObtenerDatosAdjuntosCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void EnviarDatosRadicacionCompletedEventHandler(object sender, EnviarDatosRadicacionCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class EnviarDatosRadicacionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal EnviarDatosRadicacionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void InsertarRelacionExpedientesCompletedEventHandler(object sender, InsertarRelacionExpedientesCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class InsertarRelacionExpedientesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal InsertarRelacionExpedientesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public bool Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((bool)(this.results[0]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void EliminarRelacionExpedientesCompletedEventHandler(object sender, EliminarRelacionExpedientesCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class EliminarRelacionExpedientesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal EliminarRelacionExpedientesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public bool Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((bool)(this.results[0]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void CrearDatoMisTramitesCompletedEventHandler(object sender, CrearDatoMisTramitesCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class CrearDatoMisTramitesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal CrearDatoMisTramitesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public bool Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((bool)(this.results[0]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void ModificarDatosExpedienteTramiteCompletedEventHandler(object sender, ModificarDatosExpedienteTramiteCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class ModificarDatosExpedienteTramiteCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal ModificarDatosExpedienteTramiteCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public bool Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((bool)(this.results[0]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void AsociarExpedienteNumeroSilpaCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void AsociarInfoExpedienteNumeroSilpaCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void ActivatExpedienteNumeroSilpaCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void ObtenerDocumentosRadicacionCompletedEventHandler(object sender, ObtenerDocumentosRadicacionCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class ObtenerDocumentosRadicacionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal ObtenerDocumentosRadicacionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void ObtenerDocumentosNURCompletedEventHandler(object sender, ObtenerDocumentosNURCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class ObtenerDocumentosNURCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal ObtenerDocumentosNURCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void ObtenerDocumentoRadicacionCompletedEventHandler(object sender, ObtenerDocumentoRadicacionCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class ObtenerDocumentoRadicacionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal ObtenerDocumentoRadicacionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public byte[] Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((byte[])(this.results[0]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void ObtenerPathRadicacionCompletedEventHandler(object sender, ObtenerPathRadicacionCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class ObtenerPathRadicacionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal ObtenerPathRadicacionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void ObtenerPathCorrespondenciaCompletedEventHandler(object sender, ObtenerPathCorrespondenciaCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class ObtenerPathCorrespondenciaCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal ObtenerPathCorrespondenciaCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void ObtenerPathFusCompletedEventHandler(object sender, ObtenerPathFusCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class ObtenerPathFusCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal ObtenerPathFusCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void ObtenerPathDocumentosCompletedEventHandler(object sender, ObtenerPathDocumentosCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class ObtenerPathDocumentosCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal ObtenerPathDocumentosCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void EnviarDocumentoRadicacionCompletedEventHandler(object sender, EnviarDocumentoRadicacionCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class EnviarDocumentoRadicacionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal EnviarDocumentoRadicacionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void EnviarDocumentoRadicacionAlternoCompletedEventHandler(object sender, EnviarDocumentoRadicacionAlternoCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class EnviarDocumentoRadicacionAlternoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal EnviarDocumentoRadicacionAlternoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    
    /// <remarks/>
    public string rutaArchivos {
        get {
            this.RaiseExceptionIfNecessary();
            return ((string)(this.results[1]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void EnviarDocumentoCorrespondenciaCompletedEventHandler(object sender, EnviarDocumentoCorrespondenciaCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class EnviarDocumentoCorrespondenciaCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal EnviarDocumentoCorrespondenciaCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    
    /// <remarks/>
    public string rutaArchivos {
        get {
            this.RaiseExceptionIfNecessary();
            return ((string)(this.results[1]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void RelacionarExpedientesPadreHijoCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void CrearSolicitudManualCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void ObtenerNumeroVITALPadreSolicitudCompletedEventHandler(object sender, ObtenerNumeroVITALPadreSolicitudCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class ObtenerNumeroVITALPadreSolicitudCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal ObtenerNumeroVITALPadreSolicitudCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void ObtenerAutoridadesSolicitudCompletedEventHandler(object sender, ObtenerAutoridadesSolicitudCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class ObtenerAutoridadesSolicitudCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal ObtenerAutoridadesSolicitudCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void ObtenerComunidadesSolicitudCompletedEventHandler(object sender, ObtenerComunidadesSolicitudCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class ObtenerComunidadesSolicitudCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal ObtenerComunidadesSolicitudCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void TestCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);