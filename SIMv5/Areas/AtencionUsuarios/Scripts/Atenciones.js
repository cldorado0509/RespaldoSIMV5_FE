// varibles
const now = new Date();
IdAtencion = -1;//identifica si es para crear o actualizar
IdSolicitudXAtencion = -1;// guarda el id de la atencion deleccionado en el listado de atenciones para traer sus solicitudes
IdTercero = -1;// este es el id y no el documento
IdTipoAtencion = -1;// para sacar la clase de atencion
IdClaseAtencion = -1;// para sacar la lista de chequeo
DocumentoBuscado = -1;
var Hoy = new Date().toDateString("dd/MM/yyyy hh:mm:ss");
var FechaAnterior = new Date().toDateString("dd/MM/yyyy hh:mm:ss");

var FechaIR = "";
var FechaFR = "";

var IdUsuarioFuncionario = -1;



$(document).ready(function () {
    /*document.body.style.overflow = 'hidden';*/ // esto desabilita el scroll de la pagina
    //RadioButton
    var Genero = $('#radioGenero').dxRadioGroup({
        disabled: true,
        items: ['M', 'F'],
        value: "",
        layout: 'horizontal',
    }).dxRadioGroup("instance");

    //txt
    var DocumentoB = $('#txtDocumentoB').dxNumberBox({
        min: 1,
        disabled: false,
        showClearButton: true,
        placeholder: 'Ingresar documento',
        value: "",
        width: 250
    }).dxValidator({
        validationGroup: "",
        validationRules: [{
            type: "required",
            message: "Debe ingresar el Documento!"
        }]
    }).dxNumberBox("instance");

    var PrimerApellido = $('#txtApellido1').dxTextBox({
        disabled: true,
        showClearButton: true,
        placeholder: 'Ingresar primer apellido',
        value: "",
        width: 250
    }).dxValidator({
        validationGroup: "ValPersona",
        validationRules: [{
            type: "required",
            message: "Debe ingresar el apellido!"
        },
        {
            type: 'pattern',
            pattern: /^[^0-9]+$/,
            message: 'No uses digitos en el apellido',
        }, {
            type: 'stringLength',
            min: 2,
            message: 'El apellido debe tener al menos 2 caracteres',
        }
        ]
    }).dxTextBox("instance");

    var SegundoApellido = $('#txtApellido2').dxTextBox({
        disabled: true,
        showClearButton: true,
        placeholder: 'Ingresar segundo apellido',
        value: "",
        width: 250,

    }).dxValidator({
        validationGroup: "ValPersona",
        validationRules: [
            {
                type: 'pattern',
                pattern: /^[^0-9]+$/,
                message: 'No uses digitos en el nombre',
            }
            
        ]
    }).dxTextBox("instance");

    var PrimerNombre = $('#txtNombre1').dxTextBox({
        disabled: true,
        showClearButton: true,
        placeholder: 'Ingresar primer nombre',
        value: "",
        width: 250
    }).dxValidator({
        validationGroup: "ValPersona",
        validationRules: [{
            type: "required",
            message: "Debe ingresar el Nombre!"
        },
        {
            type: 'pattern',
            pattern: /^[^0-9]+$/,
            message: 'No uses digitos en el nombre',
        }, {
            type: 'stringLength',
            min: 2,
            message: 'El nombre debe tener al menos 2 caracteres',
        }
        ]
    }).dxTextBox("instance");

    var SegundoNombre = $('#txtNombre2').dxTextBox({
        disabled: true,
        showClearButton: true,
        placeholder: 'Ingresar segundo nombre',
        value: "",
        width: 250,

    }).dxValidator({
        validationGroup: "ValPersona",
        validationRules: [
            {
                type: 'pattern',
                pattern: /^[^0-9]+$/,
                message: 'No uses digitos en el nombre',
            },
        ]
    }).dxTextBox("instance");

    var Telefono = $('#txtTelefonoB').dxNumberBox({
        disabled: true,
        showClearButton: true,
        placeholder: 'Ingresar telefono',
        value: "",
        width: 250,

    }).dxValidator({
        validationGroup: "ValPersona",
        validationRules: [{
            type: 'pattern',
            pattern: /^([0-9]){7,10}$/,
            message: 'Ingrese un telefono con un formato correcto',
        }],
    }).dxNumberBox("instance");

    var Email = $('#txtEmail').dxTextBox({
        disabled: true,
        showClearButton: true,
        placeholder: 'Ingresar email',
        value: "",
        width: 250,
    }).dxValidator({
        validationGroup: "ValPersona",
        validationRules: [{
            type: 'email',
            message: 'Ingrese un correo valido',
        }],
    }).dxTextBox("instance");
    
    var Direccion = $('#txtDireccionB').dxTextBox({
        disabled: true,
        showClearButton: true,
        placeholder: 'Ingresar Direccion',
        value: "",
        width: 250,

    }).dxTextBox("instance");
    
    var txtDocumento = $("#txtDocumento").dxNumberBox({
        min: 1,
        placeholder: 'Ingresar documento',
        value: "",
        width: 250,
        readOnly: false,
        onValueChanged: function () {
            var _Ruta = ($('#SIM').data('url') + "AtencionUsuarios/api/AtencionesApi/Persona");

            $.getJSON(_Ruta,
                {
                    id: txtDocumento.option("value")
                }).done(function (data) {

                    if (data.idTercero > 0) {
                        IdTercero = data.idTercero
                        txtNombre.option("value", data.nombreCompleto);
                        txtDireccion.option("value", data.direccion);
                        txtTelefono.option("value", data.telefono);
                        GidListadoAtenciones.dxDataGrid("instance").option("dataSource", AtencionesDataSource);
                        btnNuevaAtencion.option("visible", true);

                    } else {
                        IdTercero = -1;

                        txtNombre.reset();
                        txtDireccion.reset();
                        txtTelefono.reset();
                        btnNuevaAtencion.option("visible", false);

                        DevExpress.ui.dialog.alert('Documento no registrado!');
                    }


                }).fail(function (jqxhr, textStatus, error) {

                });
        }
    }).dxNumberBox("instance");

    var txtNombre = $("#txtNombre").dxTextBox({
        readOnly: true,
        value: "",
        width: 300,
    }).dxTextBox("instance");

    var txtDireccion = $("#txtDireccion").dxTextBox({
        readOnly: true,
        value: "",
        width: 250,
    }).dxTextBox("instance");

    var txtTelefono = $("#txtTelefono").dxNumberBox({
        value: "",
        width: 250,
        readOnly: true
    }).dxNumberBox("instance");

    var txtIdTerceroRepre = $("#txtIdTerceroRepre").dxNumberBox({
        min: 1,
        value: "",
        width: 130,
    }).dxNumberBox("instance");

    var txtNombreTerceroRepre = $("#txtNombreTerceroRepre").dxTextBox({
        readOnly: true,
        value: "",
        width: 277,
    }).dxTextBox("instance");



    // funcion limpiar

    function Limpiar() {
        CodSolicitud = -1;
        CodProyecto = -1;
        IdSolicitudXAtencion = -1;
        IdTercero = -1;
        txtDocumento.reset();
        txtNombre.reset();
        txtDireccion.reset();
        txtTelefono.reset();
        GidListadoAtenciones.dxDataGrid("instance").option("dataSource", AtencionesDataSource);
        GidListadoAsuntos.dxDataGrid("instance").option("dataSource", ListadoSolicitudesDataSource);
        GidListadoQuejas.dxDataGrid("instance").option("dataSource", ListadoQuejasDataSource);
        GidGuardarQuejas.dxDataGrid("instance").option("dataSource", BuscarQuejasDataSource);
        GidGuardarTramites.dxDataGrid("instance").option("dataSource", BuscarTramitesDataSource);
        GidListadoTramites.dxDataGrid("instance").option("dataSource", ListadotramitesDataSource);
        btnNuevaAtencion.option("visible", false);
    }


    //botones
    $("#btnBuscarPersona").dxButton({
        hint: "Buscar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'find',
        onClick: function () {
            var _Ruta = ($('#SIM').data('url') + "AtencionUsuarios/api/AtencionesApi/PersonaNatural");

            $.getJSON(_Ruta,
                {
                    id: DocumentoB.option("value")
                }).done(function (data) {

                    if (data.idTercero > 0) {
                        DocumentoBuscado = data.documentoN;
                        IdTercero = data.idTercero;
                        PrimerApellido.option("value", data.primerApellido);
                        SegundoApellido.option("value", data.segundoApellido);
                        PrimerNombre.option("value", data.primerNombre);
                        SegundoNombre.option("value", data.segundoNombre);
                        Telefono.option("value", data.telefono);
                        Email.option("value", data.correo);
                        Genero.option("value", data.genero);
                        Direccion.option("value", data.direccion);
                        btnAceptar.option("visible", true);
                        BtnEditarPersona.option("visible", true);
                        btnNuevaAtencion.option("visible", true);
                        DocumentoB.option("disabled", true);

                    } else {
                        IdTercero = -1;
                        DevExpress.ui.dialog.alert('Documento no registrado!');
                        btnNuevaAtencion.option("visible", false);
                    }


                }).fail(function (jqxhr, textStatus, error) {

                });

        }
    }).dxButton("instance");

  var btnAceptar =   $("#btnAceptar").dxButton({
        hint: "",
        stylingMode: "contained",
        text: "Aceptar",
        type: "success",
        width: 70,
        height: 36,
        icon: 'todo',
        visible: false,
        onClick: function () {
            txtDocumento.option("value",DocumentoBuscado);
            popupBuscarPersona.hide();
            DocumentoB.reset();
            DocumentoB.option("disabled", false);
            PrimerApellido.reset();
            SegundoApellido.reset();
            PrimerNombre.reset();
            SegundoNombre.reset();
            Telefono.reset();
            Email.reset();
            Direccion.reset();
            Genero.reset();
            btnAceptar.option("visible", false);
            BtnEditarPersona.option("visible", false);
            $('#DocumentoB').dxValidator('instance').reset();
        }
    }).dxButton("instance");

    $("#btnBuscar").dxButton({
        hint: "Buscar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'find',
        onClick: function () {
            popupBuscarPersona.show();
            //$('#DocumentoB').dxValidator('instance').reset();
        }
    }).dxButton("instance");

   var Btnlimpiar = $("#btnLimpiar").dxButton({
        hint: "Limpiar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'clear',
        onClick: function () {
           
            Limpiar();
        }

   }).dxButton("instance");

    var BtnlimpiarP = $("#btnLimpiarPersona").dxButton({
        hint: "Limpiar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'clear',
        onClick: function () {
            DocumentoBuscado = -1;
            IdTercero = -1;
            DocumentoB.reset();
            PrimerApellido.reset();
            SegundoApellido.reset();
            PrimerNombre.reset();
            SegundoNombre.reset();
            Telefono.reset();
            Email.reset();
            Direccion.reset();
            Genero.reset();
            PrimerApellido.option("disabled", true);
            SegundoApellido.option("disabled", true);
            PrimerNombre.option("disabled", true);
            SegundoNombre.option("disabled", true);
            Telefono.option("disabled", true);
            Email.option("disabled", true);
            Genero.option("disabled", true);
            btnAceptar.option("visible", false);
            BtnEditarPersona.option("visible", false);
            btnActualizarPersona.option("visible", false);
            DocumentoB.option("disabled", false);
            btnNuevaAtencion.option("visible", false);
            
            //$('#DocumentoB').dxValidator('instance').reset();
        }

    }).dxButton("instance");

    var BtnEditarPersona = $("#btnEditarPersona").dxButton({
        hint: "Editar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'edit',
        visible: false,
        onClick: function () {
            DocumentoB.option("disabled", true);
            PrimerApellido.option("disabled", false);
            SegundoApellido.option("disabled", false);
            PrimerNombre.option("disabled", false);
            SegundoNombre.option("disabled", false);
            Telefono.option("disabled", false);
            Email.option("disabled", false);
            Genero.option("disabled", false);
            btnActualizarPersona.option("visible", true);
            BtnEditarPersona.option("visible", false);
            btnAceptar.option("visible", false);
        }
    }).dxButton("instance");

    var btnActualizarPersona =  $("#btnActualizarPersona").dxButton({
        hint: "Actualizar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'save',
        visible: false,
        validationGroup: "ValPersona",

        onClick: function (e) {
            var result = e.validationGroup.validate();
            if (result.isValid) {


            var id = IdTercero;
            var documento = DocumentoB.option("value");
            var primerApellido = PrimerApellido.option("value");
            var segundoApellido = SegundoApellido.option("value");
            var primerNombre = PrimerNombre.option("value");
            var segundoNombre = SegundoNombre.option("value");
            var telefono = Telefono.option("value");
            var email = Email.option("value");
            var genero = Genero.option("value");

            params = {
                idTercero: id, documentoN: documento, primerNombre: primerNombre, segundoNombre: segundoNombre, primerApellido: primerApellido, segundoApellido: segundoApellido, genero: genero, telefono: telefono, correo: email, direccion: "", nombreCompleto: "",
            };


            var _Ruta = $('#SIM').data('url') + "AtencionUsuarios/api/AtencionesApi/ActualizarTerceroNatural";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                crossDomain: true,
                headers: { 'Access-Control-Allow-Origin': '*' },
                success: function (data) {
                    if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error al Guardar Datos');
                    else {
                        DocumentoB.option("disabled", false);
                        PrimerApellido.option("disabled", true);
                        SegundoApellido.option("disabled", true);
                        PrimerNombre.option("disabled", true);
                        SegundoNombre.option("disabled", true);
                        Telefono.option("disabled", true);
                        Email.option("disabled", true);
                        Genero.option("disabled", true);
                        btnActualizarPersona.option("visible", false);
                        BtnEditarPersona.option("visible", true);
                        btnAceptar.option("visible", true);
                        DevExpress.ui.dialog.alert('Datos Guardados correctamente');

                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema al Guardar Datos');
                }


            });
        } else DevExpress.ui.dialog.alert('Debe llenar correctamente los datos!');
    }
}).dxButton("instance");

    $("#btnVincularAsunto").dxButton({
        hint: "Vincular Asunto",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,

        onClick: function () {
            popupAsuntos.show();
            GidGuardarAsunto.dxDataGrid("instance").option("dataSource", BuscarSolicitudDataSource);
        }
    }).dxButton("instance");

    $("#btnVincularQueja").dxButton({
        hint: "Adicionar y relacionar una queja",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,

        onClick: function () {
            popupQuejas.show();
        }
    }).dxButton("instance");

    $("#btnVincularDocumento").dxButton({
        hint: "Vincular un documento",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,

        onClick: function () {
            var _popup = $("#popupBuscaDoc").dxPopup("instance");
            _popup.show();
            $('#BuscarDoc').attr('src', $('#SIM').data('url') + 'Utilidades/BuscarDocumento?popup=true&Parametro=' + IdSolicitudXAtencion);
        }
    }).dxButton("instance");

    $("#btnVincularNotificacion").dxButton({
        hint: "Vincular Notificación Personal",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,

        onClick: function () {
            popupNotificaciones.show();
        }
    }).dxButton("instance");

    $("#btnVincularTramite").dxButton({
        hint: "Vincular Tramite",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        onClick: function () {
            popupTramites.show();
        }
    }).dxButton("instance");

    $("#btnCrearTramite").dxButton({
        hint: "Crear Tramite",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'add',
        onClick: function () {

        }
    }).dxButton("instance");

    $("#btnCancelarAsuntos").dxButton({
        hint: "Cancelar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'clear',
        onClick: function () {

            popupAsuntos.hide();

        }
    }).dxButton("instance");

    $("#btnCancelarQuejas").dxButton({
        hint: "Cancelar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'clear',
        onClick: function () {

            popupQuejas.hide();


        }
    }).dxButton("instance");

    $("#btnCancelarNotificaciones").dxButton({
        hint: "Cancelar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'clear',
        onClick: function () {

            popupNotificaciones.hide();


        }
    }).dxButton("instance");

    $("#btnCancelarTramites").dxButton({
        hint: "Cancelar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'clear',
        onClick: function () {
            popupTramites.hide();


        }
    }).dxButton("instance");

    var btnNuevaAtencion = $("#btnNuevaAtencion").dxButton({
        hint: "Nuevo Registro",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        visible: false,
        onClick: function () {
            popupAgregarAtencion.show();
            popupAgregarAtencion.focus();
            ClearAgregarAtencion();
            IdAtencion = -1;

        }
    }).dxButton("instance");

    $("#btnBuscarTerceroRepre").dxButton({
        hint: "Buscar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 25,
        height: 25,
        icon: 'find',
        onClick: function () {

        }
    }).dxButton("instance");

    $("#btnLimpiarTerceroRepre").dxButton({
        hint: "Limpiar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 25,
        height: 25,
        icon: 'clear',
        onClick: function () {

        }

    }).dxButton("instance");

    $("#btnGuardarAtencion").dxButton({
        hint: "Guardar Atención",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'save',
        validationGroup: "ValAtencion",

        onClick: function (e) {
            var result = e.validationGroup.validate();
            if (result.isValid) {

                var atencion = IdAtencion;
                var tercero = IdTercero;
                var fechaA = Hoy;
                var claseA = claseAtencion.option("value");
                var formaA = FormaAtencion.option("value");
                var detalle = Detalle.option("value");
                var compromiso = Compromiso.option("value");
                var fechaC = FechaCompromiso.option("value");
                var estadoC = EstadoAtencion.option("value");
               // var InstaC = InstalacionC.option("value");
                var idUsuarioFuncionario = IdUsuarioFuncionario;

                if (EstadoAtencion.option("value") == true) {
                    var estadoC = 1;
                }

                if (IdAtencion > 0) {

                    var params = {
                        idAtencion: atencion, idTercero: tercero, idClaseAtencion: claseA, idFormaAtencion: formaA, idTramite: "", detalle: detalle, fechaAtencion: FechaAnterior, idUsuarioFuncionario: idUsuarioFuncionario , fechaCompromiso: fechaC, Estado: estadoC, codigoUsuario: "", codigoCompromiso: compromiso, codigoComponente: "", radicado: "", nroFicha: "", salvoConducto: "", idTerceroRepresentado: "", idInstalacionAtencion: "" //poner todos los campos del dto asi esten vacios

                    };

                } else {

                    var params = {
                         idTercero: tercero, idClaseAtencion: claseA, idFormaAtencion: formaA, idTramite: "", detalle: detalle, fechaAtencion: fechaA, idUsuarioFuncionario: "", fechaCompromiso: fechaC, Estado: estadoC, codigoUsuario: "", codigoCompromiso: compromiso, codigoComponente: "", radicado: "", nroFicha: "", salvoConducto: "", idTerceroRepresentado: "", idInstalacionAtencion: ""

                    };
                }


                var _Ruta = $('#SIM').data('url') + "AtencionUsuarios/api/AtencionesApi/GuardarAtencionTAsync";
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    url: _Ruta,
                    data: JSON.stringify(params),
                    contentType: "application/json",
                    crossDomain: true,
                    headers: { 'Access-Control-Allow-Origin': '*' },
                    success: function (data) {
                        if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Guardar Datos');
                        else {
                            DevExpress.ui.dialog.alert('Datos Guardados correctamente', 'Guardar Datos');
                            popupAgregarAtencion.hide();
                            IdAtencion = -1;
                            GidListadoAtenciones.dxDataGrid("instance").option("dataSource", AtencionesDataSource);
                        }
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                    }
                });
            } else {
                DevExpress.ui.dialog.alert('Debe ingresar correctamente los datos!');
            }
        }
    }).dxButton("instance");

    $("#btnCancelarAtencion").dxButton({
        hint: "Cancelar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'remove',
        onClick: function () {
            
            popupAgregarAtencion.hide();
        }
    }).dxButton("instance");

    var btnNuevoReporte = $("#btnNuevoReporte").dxButton({
        hint: "Reporte",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'export',
        onClick: function () {
            popupReporte.show();
        }
    }).dxButton("instance");

    $("#btnCancelarReporte").dxButton({
        hint: "Cancelar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'clear',
        onClick: function () {
            popupReporte.hide();


        }
    }).dxButton("instance");

    $("#btnFiltrarReporte").dxButton({
        hint: "Buscar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'find',
        onClick: function () {

            FechaFR = FechaFReporte.option("text");
            FechaIR = FechaIReporte.option("text");
            GidListadoAtencionReporte.dxDataGrid("instance").option("dataSource", AtencionReporteDataSource);
    
        }
    }).dxButton("instance");


   //funcion limpiar campos del popupAgregarAtencion

    function ClearAgregarAtencion() {
        IdTipoAtencion = -1;
        IdClaseAtencion = -1;
        claseAtencion.reset();
        FormaAtencion.reset();
        TipoAtencion.reset();
        Detalle.reset();
        Compromiso.reset();
        InstalacionC.reset();
        EstadoAtencion.reset();
        FechaCompromiso.option("value", now);
        txtIdTerceroRepre.reset();
        txtNombreTerceroRepre.reset();
       
    }

    //grids

    var GidListadoAtenciones = $("#GidListadoAtenciones").dxDataGrid(
        {
            dataSource: AtencionesDataSource,
            allowColumnResizing: true,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
            noDataText: "Sin datos para mostrar",
            showBorders: true,
            wordWrapEnabled: true, // ajustar el texto(doble linea)

            export: {
                enabled: true
            },

            paging: {
                pageSize: 5
            },
            headerFilter: {
                visible: false,
                allowSearch: true,
            },
            pager: {
                showPageSizeSelector: false,
                allowedPageSizes: [5, 10, 20],
                showNavigationButtons: true,
            },
            filterRow: {
                visible: true,
                emptyPanelText: 'Arrastre una columna para agrupar'
            },
            selection: {
                mode: 'single'
            },
            hoverStateEnabled: true,
            remoteOperations: true,
            
            columns: [
                
                { dataField: 'idAtencion', width: '5%', caption: 'Codigo', alignment: 'left' },
                { dataField: 'fechaAtencion', width: '10%', caption: 'Fecha', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy' },
                { dataField: 'idUsuario', width: '10%', caption: 'Identificacion Usuario', alignment: 'left' },
                { dataField: 'rSocial', width: '10%', caption: 'Usuario', alignment: 'center' },
                { dataField: 'tipoAtencion', width: '10%', caption: 'Tipo de Atencion', alignment: 'left' },
                { dataField: 'claseAtencion', width: '10%', caption: 'Clase de Atencion', alignment: 'center' },
                { dataField: 'formaAtencion', width: '10%', caption: 'Forma de Atencion', alignment: 'center' },
                { dataField: 'atendidoPor', width: '10%', caption: 'Atendido Por', alignment: 'center' },
                { dataField: 'detalle', width: '10%', caption: 'Detalle', alignment: 'center' },
                { dataField: 'fechaCompromiso', width: '5%', caption: 'F. Compromiso', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy' },
                {
                    dataField: 'estado', width: '5%', caption: 'Estado', alignment: 'center', customizeText: function (cellInfo)
                    {
                        return cellInfo.value == "1" ? 'Terminada' : 'No terminada';
                    }
                },
               


                {
                    width: '5%',
                    caption: "Editar",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        $('<div/>').dxButton({
                            icon: 'edit',


                            hint: 'Editar registro',
                            onClick: function () {

                                var _Ruta = $('#SIM').data('url') + "AtencionUsuarios/api/AtencionesApi/ObtenerAtencion";
                                $.getJSON(_Ruta,
                                    {
                                        id: options.data.idAtencion
                                    }).done(function (data) {
                                        if (data !== null) {
                                            popupAgregarAtencion.show();
                                            IdAtencion = data.idAtencion;
                                            IdUsuarioFuncionario = data.idUsuarioFuncionario;
                                            TipoAtencion.option("value", data.idTipoAtencion);
                                            claseAtencion.option("value", data.idClaseAtencion);
                                            FormaAtencion.option("value", data.idFormaAtencion);
                                            Detalle.option("value", data.detalle);
                                            Compromiso.option("value", data.idTipoCompromiso);
                                            FechaCompromiso.option("value", data.fechaCompromiso);
                                            EstadoAtencion.option("value", data.estado);
                                            //txtIdTerceroRepre.option("value", data.idTerceroRepresentado);
                                            //txtNombreTerceroRepre.option("value", data.nombreTerceroRepresentado);
                                            
                                        }
                                    }).fail(function (jqxhr, textStatus, error) {
                                        DevExpress.ui.dialog.alert('Ocurrió un error');
                                    });


                            }
                        }).appendTo(container);
                    }

                },

                //{
                //    width: '5%',
                //    caption: "Lista chequeo",
                //    alignment: 'center',
                //    cellTemplate: function (container, options) {
                //        $('<div/>').dxButton({
                //            icon: 'print',


                //            hint: 'Imprimir',
                //            onClick: function () {


                //            }
                //        }).appendTo(container);
                //    }

                //},

            ],
            onSelectionChanged: function (selectedItems) {
                

                var data = selectedItems.selectedRowsData[0];
                if (data) {
                    FechaAnterior = data.fechaAtencion;
                    IdSolicitudXAtencion = data.idAtencion;
                    GidListadoAsuntos.dxDataGrid("instance").option("dataSource", ListadoSolicitudesDataSource);
                    GidListadoQuejas.dxDataGrid("instance").option("dataSource", ListadoQuejasDataSource);
                    GidListadoDocumentos.dxDataGrid("instance").option("dataSource", ListadoDocumentosDataSource);
                    GidListadoTramites.dxDataGrid("instance").option("dataSource", ListadotramitesDataSource);
                }

            }
        });

    var GidListadoAsuntos = $("#GidListadoAsuntos").dxDataGrid(
        {
            visible: true,
            dataSource: ListadoSolicitudesDataSource,
            allowColumnResizing: true,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
            noDataText: "Sin datos para mostrar",
            showBorders: true,
            wordWrapEnabled: false,

            paging: {
                pageSize: 5
            },
            headerFilter: {
                visible: false,
                allowSearch: true,
            },
            pager: {
                showPageSizeSelector: false,
                allowedPageSizes: [5, 10, 20],
                showNavigationButtons: true,
            },
            filterRow: {
                visible: true,
                emptyPanelText: 'Arrastre una columna para agrupar'
            },
            selection: {
                mode: 'single'
            },
            hoverStateEnabled: false,
            remoteOperations: true,
            columns: [
                { dataField: 'codigoSolicitud', width: '10%', caption: 'Codigo', alignment: 'center' },
                { dataField: 'CM', width: '10%', caption: 'CM', alignment: 'center' },
                { dataField: 'CodigoTramite', width: '10%', caption: 'Tramite', alignment: 'center' },
                { dataField: 'nombreTipoSolicitud', width: '20%', caption: 'Tipo de Solicitud', alignment: 'center' },
                { dataField: 'conexo', width: '20%', caption: 'Conexo', alignment: 'center' },
                { dataField: 'descripcion', width: '20%', caption: 'Descripción', alignment: 'center' },



                //{
                //    width: '7%',
                //    caption: "Ver Expediente",
                //    alignment: 'center',
                //    cellTemplate: function (container, options) {
                //        $('<div/>').dxButton({
                //            icon: 'link',


                //            hint: 'Ver Expediente',
                //            onClick: function () {


                //            }
                //        }).appendTo(container);
                //    }

                //},

                {
                    width: '10%',
                    caption: "Desvincular",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        $('<div/>').dxButton({
                            icon: 'remove',


                            hint: 'Desvincular Asunto',
                            onClick: function (e) {
                                var result = DevExpress.ui.dialog.confirm('Desea desvincular la solicitud seleccionada?', 'Confirmación');
                                result.done(function (dialogResult) {
                                    if (dialogResult) {

                                        var IdAtPro = options.data.idAtencionProyecto;
                                        var CodProyecto = options.data.codigoProyecto;
                                        var CodSolicitud = options.data.codigoSolicitud;

                                        var _Ruta = $('#SIM').data('url') + "AtencionUsuarios/api/AtencionesApi/DesvincularSolicitudTAsync";
                                        var params = {
                                            idAtencionProyecto: IdAtPro, idAtencion: IdSolicitudXAtencion, codigoProyecto: CodProyecto, codigoSolicitud: CodSolicitud
                                        };
                                        $.ajax({
                                            type: 'POST',
                                            url: _Ruta,
                                            contentType: "application/json",
                                            dataType: 'json',
                                            data: JSON.stringify(params),
                                            success: function (data) {
                                                if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error al desvincular registro seleccionado');
                                                else {
                                                    DevExpress.ui.dialog.alert('Registro desvinculado correctamente!');
                                                    GidListadoAsuntos.dxDataGrid("instance").option("dataSource", ListadoSolicitudesDataSource);
                                                }
                                            },
                                            error: function (xhr, textStatus, errorThrown) {
                                                DevExpress.ui.dialog.alert('Ocurrió un error al desvincular el registro seleccionado');
                                            }
                                        });
                                    }
                                });

                            }
                        }).appendTo(container);
                    }

                },

            ],
            onSelectionChanged: function (selectedItems) {


            }
        });

    var GidGuardarAsunto = $("#GidGuardarAsunto").dxDataGrid( 
        {
            visible: true,
            dataSource: BuscarSolicitudDataSource,
            allowColumnResizing: true,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
            noDataText: "Sin datos para mostrar",
            showBorders: true,
            wordWrapEnabled: true,
            
            paging: {
                pageSize: 5
            },
            headerFilter: {
                visible: false,
                allowSearch: true,
            },
            pager: {
                showPageSizeSelector: false,
                allowedPageSizes: [5, 10, 20],
                showNavigationButtons: true,
            },
            filterRow: {
                visible: true,
                emptyPanelText: 'Arrastre una columna para agrupar'
            },
            selection: {
                mode: 'single'
            },
            hoverStateEnabled: true,
            remoteOperations: true,
            columns: [

                { dataField: 'CM', width: '10%', caption: 'CM', alignment: 'center' },
                { dataField: 'CodigoTramite', width: '10%', caption: 'Tramite', alignment: 'center' },
                { dataField: 'nombreTipoSolicitud', width: '20%', caption: 'Tipo de Solicitud', alignment: 'left', allowSearch: false},
                { dataField: 'conexo', width: '20%', caption: 'Conexo', alignment: 'center',allowSearch: false },
                { dataField: 'descripcion', width: '20%', caption: 'Descripción', alignment: 'left', allowSearch: false },

                {
                    width: '20%',
                    caption: "Vincular",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        $('<div/>').dxButton({
                            icon: 'save',
                            hint: 'Vincular Solicitud',
                            onClick: function (e) {
                                
                                var idAt = IdSolicitudXAtencion;
                                var codP = options.data.codigoProyecto;
                                var codS = options.data.codigoSolicitud;
                               

                                params = {
                                    idAtencion: idAt, codigoProyecto: codP, codigoSolicitud: codS
                                };
                            

                                var _Ruta = $('#SIM').data('url') + "AtencionUsuarios/api/AtencionesApi/VincularSolicitudTAsync";
                                $.ajax({
                                    type: "POST",
                                    dataType: 'json',
                                    url: _Ruta,
                                    data: JSON.stringify(params),
                                    contentType: "application/json",
                                    crossDomain: true,
                                    headers: { 'Access-Control-Allow-Origin': '*' },
                                    success: function (data) {
                                        if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error');
                                        else {
                                            DevExpress.ui.dialog.alert('Registro vinculado correctamente!');
                                            GidListadoAsuntos.dxDataGrid("instance").option("dataSource", ListadoSolicitudesDataSource);
                                            popupAsuntos.hide();
                                        }
                                },
                                error: function (xhr, textStatus, errorThrown) {
                                    DevExpress.ui.dialog.alert('Ocurrió un problema Guardar Datos');
                                }
                            });
                            }

                        }).appendTo(container);
                    }

                },

            ],
            onSelectionChanged: function (selectedItems) {


            }
        });

    var GidListadoQuejas = $("#GidListadoQuejas").dxDataGrid(
        {
            visible: true,
            dataSource: ListadoQuejasDataSource,
            allowColumnResizing: true,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
            noDataText: "Sin datos para mostrar",
            showBorders: true,
            wordWrapEnabled: true,

            paging: {
                pageSize: 5
            },
            headerFilter: {
                visible: false,
                allowSearch: false,
            },
            pager: {
                showPageSizeSelector: false,
                allowedPageSizes: [5, 10, 20],
                showNavigationButtons: true,
            },
            filterRow: {
                visible: false,
                emptyPanelText: 'Arrastre una columna para agrupar'
            },
            selection: {
                mode: 'single'
            },
            hoverStateEnabled: true,
            remoteOperations: true,
            columns: [

                { dataField: 'idQuejaAtencion', width: '5%', caption: 'Codigo', alignment: 'center', allowSearch: false },
                { dataField: 'fechaRecepcion', width: '10%', caption: 'Fecha Recepción', alignment: 'left', dataType: 'date', format: 'dd/MM/yyyy', allowSearch: false },
                { dataField: 'Queja', width: '10%', caption: 'Queja', alignment: 'center', allowSearch: false },
                { dataField: 'ano', width: '10%', caption: 'Año', alignment: 'left', allowSearch: false },
                { dataField: 'nombreMunicipio', width: '10%', caption: 'Municipio', alignment: 'left', allowSearch: false },
                { dataField: 'nombreRecurso', width: '10%', caption: 'Recurso', alignment: 'left', allowSearch: false },
                { dataField: 'nombreAfectacion', width: '10%', caption: 'Afectacion', alignment: 'left', allowSearch: false },
                { dataField: 'asunto', width: '20%', caption: 'Asunto', alignment: 'left', allowSearch: false },
                { dataField: 'infractor', width: '10%', caption: 'Infractor', alignment: 'left', allowSearch: false },
               
                {
                    width: '5%',
                    caption: "Desvincular",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        $('<div/>').dxButton({
                            icon: 'remove',


                            hint: 'Desvincular Queja',
                            onClick: function (e) {
                                var result = DevExpress.ui.dialog.confirm('Desea desvincular la queja seleccionada?', 'Confirmación');
                                result.done(function (dialogResult) {
                                    if (dialogResult) {

                                      
                                        var idQa = options.data.idQuejaAtencion;
                                        var idQ = options.data.CodigoQueja;

                                        var _Ruta = $('#SIM').data('url') + "AtencionUsuarios/api/AtencionesApi/DesvincularQueja";
                                        var params = {
                                            idQuejaAtencion: idQa, idAtencion: IdSolicitudXAtencion, idQueja: idQ
                                        };
                                        $.ajax({
                                            type: 'POST',
                                            url: _Ruta,
                                            contentType: "application/json",
                                            dataType: 'json',
                                            data: JSON.stringify(params),
                                            success: function (data) {
                                                if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error al desvincular registro seleccionado');
                                                else {
                                                    DevExpress.ui.dialog.alert('Registro desvinculado correctamente!');
                                                    GidListadoQuejas.dxDataGrid("instance").option("dataSource", ListadoQuejasDataSource);
                                                }
                                            },
                                            error: function (xhr, textStatus, errorThrown) {
                                                DevExpress.ui.dialog.alert('Ocurrió un error al desvincular el registro seleccionado');
                                            }
                                        });
                                    }
                                });

                            }
                        }).appendTo(container);
                    }

                },

            ],
            onSelectionChanged: function (selectedItems) {


            }
        });

    var GidGuardarQuejas = $("#GidGuardarQuejas").dxDataGrid(
        {
            visible: true,
            dataSource: BuscarQuejasDataSource,
            allowColumnResizing: true,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
            noDataText: "Sin datos para mostrar",
            showBorders: true,
            wordWrapEnabled: true,

            paging: {
                pageSize: 5
            },
            headerFilter: {
                visible: false,
                
            },
            pager: {
                showPageSizeSelector: false,
                allowedPageSizes: [5, 10, 20],
                showNavigationButtons: true,
            },
            filterRow: {
                visible: true,
                emptyPanelText: 'Arrastre una columna para agrupar'
            },
            selection: {
                mode: 'single'
            },
            hoverStateEnabled: true,
            remoteOperations: true,
            columns: [

                { dataField: 'Queja', width: '5%', caption: 'Queja', alignment: 'center', allowSearch: true },
                { dataField: 'CodigoQueja', width: '5%', caption: 'Codigo', alignment: 'center', allowSearch: false },
                { dataField: 'fechaRecepcion', width: '10%', caption: 'Fecha Recepción', alignment: 'left', allowSearch: false, dataType: 'date', format: 'dd/MM/yyyy' },
                { dataField: 'ano', width: '10%', caption: 'Año', alignment: 'left', allowSearch: false },
                { dataField: 'nombreMunicipio', width: '10%', caption: 'Municipio', alignment: 'left', allowSearch: false },
                { dataField: 'nombreRecurso', width: '10%', caption: 'Recurso', alignment: 'left', allowSearch: false },
                { dataField: 'nombreAfectacion', width: '10%', caption: 'Afectacion', alignment: 'left', allowSearch: false },
                { dataField: 'asunto', width: '20%', caption: 'Asunto', alignment: 'left', allowSearch: false },
                { dataField: 'infractor', width: '15%', caption: 'Infractor', alignment: 'left', allowSearch: false },

                {
                    width: '5%',
                    caption: "Vincular",
                    alignment: 'center',
                    allowSearch: false,
                    cellTemplate: function (container, options) {
                        $('<div/>').dxButton({
                            icon: 'save',


                            hint: 'Vincular Queja',
                            onClick: function (e) {

                                var idAt = IdSolicitudXAtencion;
                                var idQ = options.data.CodigoQueja;
                                


                                params = {
                                    idAtencion: idAt, idQueja: idQ
                                };


                                var _Ruta = $('#SIM').data('url') + "AtencionUsuarios/api/AtencionesApi/VincularQueja";
                                $.ajax({
                                    type: "POST",
                                    dataType: 'json',
                                    url: _Ruta,
                                    data: JSON.stringify(params),
                                    contentType: "application/json",
                                    crossDomain: true,
                                    headers: { 'Access-Control-Allow-Origin': '*' },
                                    success: function (data) {
                                        if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error');
                                        else {
                                            DevExpress.ui.dialog.alert('Registro vinculado correctamente!');
                                            GidListadoQuejas.dxDataGrid("instance").option("dataSource", ListadoQuejasDataSource);
                                            popupQuejas.hide();
                                        }
                                    },
                                    error: function (xhr, textStatus, errorThrown) {
                                        DevExpress.ui.dialog.alert('Ocurrió un problema Guardar Datos');
                                    }
                                });
                            }
                        }).appendTo(container);
                    }

                },

            ],
            onSelectionChanged: function (selectedItems) {


            }
        });

    var GidListadoDocumentos = $("#GidListadoDocumentos").dxDataGrid(
        {
            visible: true,
            dataSource: ListadoDocumentosDataSource,
            allowColumnResizing: true,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
            noDataText: "Sin datos para mostrar",
            showBorders: true,
            wordWrapEnabled: true,

            paging: {
                pageSize: 5
            },
            headerFilter: {
                visible: false,
                allowSearch: true,
            },
            pager: {
                showPageSizeSelector: false,
                allowedPageSizes: [5, 10, 20],
                showNavigationButtons: true,
            },
            filterRow: {
                visible: true,
                emptyPanelText: 'Arrastre una columna para agrupar'
            },
            selection: {
                mode: 'single'
            },
            hoverStateEnabled: true,
            remoteOperations: true,
            columns: [

                { dataField: 'idDocumento', width: '20%', caption: 'Id Documento', alignment: 'center' },
                { dataField: 'fechaCreacion', width: '20%', caption: 'Fecha Creacion', alignment: 'left', dataType: 'date', format: 'dd/MM/yyyy' },
                { dataField: 'nombreSerie', width: '20%', caption: 'Serie', alignment: 'center' },
                
                {
                    width: '20%',
                    caption: "Ver Documento",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        $('<div/>').dxButton({
                            icon: 'doc',
                            hint: 'Ver Documento',
                            onClick: function () {
                                var _popup = $("#popDocumento").dxPopup("instance");

                                _popup.show();
                                $("#Documento").attr("src", $('#SIM').data('url') + "Utilidades/LeeDoc?IdDocumento=" + options.data.idDocumento);

                            }
                        }).appendTo(container);
                    }

                },

                {
                    width: '20%',
                    caption: "Desvincular",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        $('<div/>').dxButton({
                            icon: 'remove',
                            hint: 'Desvincular Documento',
                            onClick: function () {
                                var result = DevExpress.ui.dialog.confirm('Desea desvincular la queja seleccionada?', 'Confirmación');
                                result.done(function (dialogResult) {
                                    if (dialogResult) {


                                        var idAd = options.data.idAtencionDocumento;
                                        var idD = options.data.idDocumento;

                                        var _Ruta = $('#SIM').data('url') + "AtencionUsuarios/api/AtencionesApi/DesvincularDocumento";
                                        var params = {
                                            idAtencionDocumento: idAd, idAtencion: IdSolicitudXAtencion, idDocumento: idD
                                        };
                                        $.ajax({
                                            type: 'POST',
                                            url: _Ruta,
                                            contentType: "application/json",
                                            dataType: 'json',
                                            data: JSON.stringify(params),
                                            success: function (data) {
                                                if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error al desvincular registro seleccionado');
                                                else {
                                                    DevExpress.ui.dialog.alert('Registro desvinculado correctamente!');
                                                    GidListadoDocumentos.dxDataGrid("instance").option("dataSource", ListadoDocumentosDataSource);
                                                }
                                            },
                                            error: function (xhr, textStatus, errorThrown) {
                                                DevExpress.ui.dialog.alert('Ocurrió un error al desvincular el registro seleccionado');
                                            }
                                        });
                                    }
                                });

                            }
                        }).appendTo(container);
                    }

                },

            ],
            onSelectionChanged: function (selectedItems) {


            }
        });

    //var GidListadoNotificaciones = $("#GidListadoNotificaciones").dxDataGrid(
    //    {
    //        visible: false,
    //        dataSource: "",
    //        allowColumnResizing: true,
    //        loadPanel: { enabled: true, text: 'Cargando Datos...' },
    //        noDataText: "Sin datos para mostrar",
    //        showBorders: true,
    //        wordWrapEnabled: true,

    //        paging: {
    //            pageSize: 5
    //        },
    //        headerFilter: {
    //            visible: false,
    //            allowSearch: true,
    //        },
    //        pager: {
    //            showPageSizeSelector: false,
    //            allowedPageSizes: [5, 10, 20],
    //            showNavigationButtons: true,
    //        },
    //        filterRow: {
    //            visible: true,
    //            emptyPanelText: 'Arrastre una columna para agrupar'
    //        },
    //        selection: {
    //            mode: 'single'
    //        },
    //        hoverStateEnabled: true,
    //        remoteOperations: true,
    //        columns: [

    //            { dataField: 'FechaIngreso', width: '8.2%', caption: 'Codigo', alignment: 'center' },
    //            { dataField: 'Documento', width: '6%', caption: 'F. Notificación', alignment: 'left' },
    //            { dataField: 'NombreCompleto', width: '17%', caption: 'Elaborado Por', alignment: 'center' },
    //            { dataField: 'Carne', width: '4.1%', caption: 'Persona Natural', alignment: 'left' },
    //            { dataField: 'Carne', width: '4.1%', caption: 'Persona Juridica', alignment: 'left' },
    //            { dataField: 'Carne', width: '4.1%', caption: 'Tercero', alignment: 'left' },
    //            { dataField: 'Carne', width: '4.1%', caption: 'Forma', alignment: 'left' },
    //            { dataField: 'Carne', width: '4.1%', caption: 'F. Fijacion', alignment: 'left' },
    //            { dataField: 'Carne', width: '4.1%', caption: 'Notificado Presente', alignment: 'left' },
    //            { dataField: 'Carne', width: '4.1%', caption: 'F. Notificado Presente', alignment: 'left' },
    //            { dataField: 'Carne', width: '4.1%', caption: 'F. Vencimiento', alignment: 'left' },
    //            { dataField: 'Carne', width: '4.1%', caption: 'Realizada', alignment: 'left' },
    //            { dataField: 'Carne', width: '4.1%', caption: 'Generada', alignment: 'left' },




    //            {
    //                width: '5%',
    //                caption: "Impresión",
    //                alignment: 'center',
    //                cellTemplate: function (container, options) {
    //                    $('<div/>').dxButton({
    //                        icon: 'remove',


    //                        hint: 'Ver Notificación',
    //                        onClick: function () {


    //                        }
    //                    }).appendTo(container);
    //                }

    //            },

    //            {
    //                width: '5%',
    //                caption: "Notificar",
    //                alignment: 'center',
    //                cellTemplate: function (container, options) {
    //                    $('<div/>').dxButton({
    //                        icon: 'remove',


    //                        hint: 'Notificar',
    //                        onClick: function () {


    //                        }
    //                    }).appendTo(container);
    //                }

    //            },

    //            {
    //                width: '5%',
    //                caption: "Vista Previa",
    //                alignment: 'center',
    //                cellTemplate: function (container, options) {
    //                    $('<div/>').dxButton({
    //                        icon: 'remove',


    //                        hint: 'Vista Previa de la Notificación',
    //                        onClick: function () {


    //                        }
    //                    }).appendTo(container);
    //                }

    //            },

    //        ],
    //        onSelectionChanged: function (selectedItems) {


    //        }
    //    });

    //var GidGuardarNotificaciones = $("#GidGuardarNotificaciones").dxDataGrid(
    //    {
    //        visible: true,
    //        dataSource: "",
    //        allowColumnResizing: true,
    //        loadPanel: { enabled: true, text: 'Cargando Datos...' },
    //        noDataText: "Sin datos para mostrar",
    //        showBorders: true,
    //        wordWrapEnabled: true,

    //        paging: {
    //            pageSize: 5
    //        },
    //        headerFilter: {
    //            visible: false,
    //            allowSearch: true,
    //        },
    //        pager: {
    //            showPageSizeSelector: false,
    //            allowedPageSizes: [5, 10, 20],
    //            showNavigationButtons: true,
    //        },
    //        filterRow: {
    //            visible: true,
    //            emptyPanelText: 'Arrastre una columna para agrupar'
    //        },
    //        selection: {
    //            mode: 'single'
    //        },
    //        hoverStateEnabled: true,
    //        remoteOperations: true,
    //        columns: [

    //            { dataField: 'FechaIngreso', width: '8.2%', caption: 'Codigo', alignment: 'center' },
    //            { dataField: 'Documento', width: '6%', caption: 'Fecha Elaboracion', alignment: 'left' },
    //            { dataField: 'NombreCompleto', width: '17%', caption: 'Identificación', alignment: 'center' },
    //            { dataField: 'Carne', width: '4.1%', caption: 'Persona Natural', alignment: 'left' },
    //            { dataField: 'Carne', width: '4.1%', caption: 'Nit', alignment: 'left' },
    //            { dataField: 'Carne', width: '4.1%', caption: 'Persona Juridica', alignment: 'left' },
    //            { dataField: 'Carne', width: '4.1%', caption: 'Tercero', alignment: 'left' },
    //            { dataField: 'Carne', width: '4.1%', caption: 'Expediente', alignment: 'left' },
    //            { dataField: 'Carne', width: '4.1%', caption: 'Acto', alignment: 'left' },




    //            {
    //                width: '5%',
    //                caption: "Seleccionar",
    //                alignment: 'center',
    //                cellTemplate: function (container, options) {
    //                    $('<div/>').dxButton({
    //                        icon: 'save',


    //                        hint: 'Vincular Notificacion',
    //                        onClick: function () {


    //                        }
    //                    }).appendTo(container);
    //                }

    //            },

    //        ],
    //        onSelectionChanged: function (selectedItems) {


    //        }
    //    });

    var GidListadoTramites = $("#GidListadoTramites").dxDataGrid(
        {
            visible: true,
            dataSource: ListadotramitesDataSource,
            allowColumnResizing: true,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
            noDataText: "Sin datos para mostrar",
            showBorders: true,
            wordWrapEnabled: true,

            paging: {
                pageSize: 5
            },
            headerFilter: {
                visible: false,
                allowSearch: true,
            },
            pager: {
                showPageSizeSelector: false,
                allowedPageSizes: [5, 10, 20],
                showNavigationButtons: true,
            },
            filterRow: {
                visible: true,
                emptyPanelText: 'Arrastre una columna para agrupar'
            },
            selection: {
                mode: 'single'
            },
            hoverStateEnabled: true,
            remoteOperations: true,
            columns: [

                { dataField: 'nombreProceso', width: '20%', caption: 'Proceso', alignment: 'center' },
                { dataField: 'CodTramite', width: '20%', caption: 'Tramite', alignment: 'left' },
                { dataField: 'fechaInicio', width: '20%', caption: 'Fecha Inicio', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy' },
                {
                    dataField: 'estado', width: '20%', caption: 'Estado', alignment: 'center', customizeText: function (cellInfo) {
                        return cellInfo.value == "1" ? 'Cerrado' : 'Abierto';
                    }
                },

                {
                    width: '10%',
                    caption: "Ver Tramite",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        $('<div/>').dxButton({
                            icon: 'add',


                            hint: 'Ver Tramite',
                            onClick: function () {
                                var _popup = $("#popDocumento").dxPopup("instance");

                                _popup.show();
                                $("#Documento").attr("src", $('#SIM').data('url') + "Utilidades/DetalleTramite?popup=true&CodTramite=" + options.data.CodTramite);

                            }
                        }).appendTo(container);
                    }

                },

                {
                    width: '10%',
                    caption: "Desvincular",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        $('<div/>').dxButton({
                            icon: 'remove',


                            hint: 'Desvincular Tramite',
                            onClick: function (e) {
                                var result = DevExpress.ui.dialog.confirm('Desea desvincular la solicitud seleccionada?', 'Confirmación');
                                result.done(function (dialogResult) {
                                    if (dialogResult) {

                                        var IdAtT = options.data.idAtencionTramite;
                                        var CodT = options.data.CodTramite;
                                      

                                        var _Ruta = $('#SIM').data('url') + "AtencionUsuarios/api/AtencionesApi/DesvincularTramite";
                                        var params = {
                                            idAtencionTramite: IdAtT, idAtencion: IdSolicitudXAtencion, codigoTramite: CodT
                                        };
                                        $.ajax({
                                            type: 'POST',
                                            url: _Ruta,
                                            contentType: "application/json",
                                            dataType: 'json',
                                            data: JSON.stringify(params),
                                            success: function (data) {
                                                if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error al desvincular registro seleccionado');
                                                else {
                                                    DevExpress.ui.dialog.alert('Registro desvinculado correctamente!');
                                                    GidListadoTramites.dxDataGrid("instance").option("dataSource", ListadotramitesDataSource);
                                                }
                                            },
                                            error: function (xhr, textStatus, errorThrown) {
                                                DevExpress.ui.dialog.alert('Ocurrió un error al desvincular el registro seleccionado');
                                            }
                                        });
                                    }
                                });

                            }
                        }).appendTo(container);
                    }

                },

            ],
            onSelectionChanged: function (selectedItems) {


            }
        });

    var GidGuardarTramites = $("#GidGuardarTramites").dxDataGrid(
        {
            visible: true,
            dataSource: BuscarTramitesDataSource,
            allowColumnResizing: true,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
            noDataText: "Sin datos para mostrar",
            showBorders: true,
            wordWrapEnabled: true,

            export: {
                enabled: true
            },
            paging: {
                pageSize: 5
            },
            headerFilter: {
                visible: false,
                allowSearch: true,
            },
            pager: {
                showPageSizeSelector: false,
                allowedPageSizes: [5, 10, 20],
                showNavigationButtons: true,
            },
            filterRow: {
                visible: true,
                emptyPanelText: 'Arrastre una columna para agrupar'
            },
            selection: {
                mode: 'single'
            },
            hoverStateEnabled: true,
            remoteOperations: true,
            columns: [

                { dataField: 'CodTramite', width: '25%', caption: 'Codigo', alignment: 'center' },
                { dataField: 'fechaInicio', width: '25%', caption: 'Fecha Inicial', alignment: 'left', dataType: 'date', format: 'dd/MM/yyyy' },
                { dataField: 'fechaFin', width: '25%', caption: 'Fecha Final', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy' },




                {
                    width: '25%',
                    caption: "Vincular",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        $('<div/>').dxButton({
                            icon: 'save',


                            hint: 'Vincular Tramite',
                            onClick: function (e) {

                                var idAt = IdSolicitudXAtencion;
                                var idT = options.data.CodTramite;



                                params = {
                                    idAtencion: idAt, codigoTramite: idT
                                };


                                var _Ruta = $('#SIM').data('url') + "AtencionUsuarios/api/AtencionesApi/VincularTramite";
                                $.ajax({
                                    type: "POST",
                                    dataType: 'json',
                                    url: _Ruta,
                                    data: JSON.stringify(params),
                                    contentType: "application/json",
                                    crossDomain: true,
                                    headers: { 'Access-Control-Allow-Origin': '*' },
                                    success: function (data) {
                                        if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error');
                                        else {
                                            DevExpress.ui.dialog.alert('Registro vinculado correctamente!');
                                            GidListadoTramites.dxDataGrid("instance").option("dataSource", ListadotramitesDataSource);
                                            popupTramites.hide();
                                        }
                                    },
                                    error: function (xhr, textStatus, errorThrown) {
                                        DevExpress.ui.dialog.alert('Ocurrió un problema Guardar Datos');
                                    }
                                });
                            }
                        }).appendTo(container);
                    }

                },

            ],
            onSelectionChanged: function (selectedItems) {


            }
        });

    var GidListadoAtencionReporte = $("#GidListadoAtencionReporte").dxDataGrid(
        {
            dataSource: AtencionReporteDataSource,
            allowColumnResizing: true,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
            noDataText: "Sin datos para mostrar",
            showBorders: true,
            wordWrapEnabled: true, // ajustar el texto(doble linea)

            export: {
                enabled: true
            },

            paging: {
                pageSize: 4
            },
            headerFilter: {
                visible: false,
                allowSearch: true,
            },
            pager: {
                showPageSizeSelector: false,
                allowedPageSizes: [5, 10, 20],
                showNavigationButtons: true,
            },
            filterRow: {
                visible: true,
                emptyPanelText: 'Arrastre una columna para agrupar'
            },
            selection: {
                mode: 'single'
            },
            hoverStateEnabled: true,
            remoteOperations: true,

            columns: [

                { dataField: 'idAtencion', width: '5%', caption: 'Codigo', alignment: 'left' },
                { dataField: 'fechaAtencion', width: '10%', caption: 'Fecha', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy' },
                { dataField: 'idUsuario', width: '10%', caption: 'Identificacion Usuario', alignment: 'left' },
                { dataField: 'rSocial', width: '10%', caption: 'Usuario', alignment: 'center' },
                { dataField: 'tipoAtencion', width: '10%', caption: 'Tipo de Atencion', alignment: 'left' },
                { dataField: 'claseAtencion', width: '10%', caption: 'Clase de Atencion', alignment: 'center' },
                { dataField: 'formaAtencion', width: '10%', caption: 'Forma de Atencion', alignment: 'center' },
                { dataField: 'atendidoPor', width: '10%', caption: 'Atendido Por', alignment: 'center' },
                { dataField: 'detalle', width: '10%', caption: 'Detalle', alignment: 'center' },
                { dataField: 'fechaCompromiso', width: '5%', caption: 'F. Compromiso', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy' },
                {
                    dataField: 'estado', width: '5%', caption: 'Estado', alignment: 'center', customizeText: function (cellInfo) {
                        return cellInfo.value == "1" ? 'Terminada' : 'No terminada';
                    }
                }

         

            ]
            
        });

    //TextArea

    var Detalle = $('#txtareaDetalle').dxTextArea({
        width: 410,
        value: "",
        height: 102,
    }).dxValidator({
        validationGroup: "ValAtencion",
        validationRules: [{
            type: "required",
            message: "Debe ingresar el detalle de la atencion!"
        },
            {
                type: 'stringLength',
                min: 2,
                message: 'El nombre debe tener al menos 2 caracteres',
            },
            {
                type: 'stringLength',
                max: 4000,
                message: 'El nombre debe tener al maximo 4000 caracteres',
            }

        ]
    }).dxTextArea('instance');

    //data grid como texto

    var lista = []

    function cargarListaChequeo() {
        var res = $.getJSON($("#SIM").data("url") + "AtencionUsuarios/api/AtencionesApi/GetListaChequeoAsync?id=" + IdClaseAtencion);
        res.then(data => {

            lista = data
            listaChequeo.option("dataSource", lista)
        })
    }


    var listaChequeo = $('#ListaChequeo').dxDataGrid({
       

        columns: [

            { dataField: 'chequeo', caption: 'Lista de Chequeo', alignment: 'center', allowSorting: false},
            
        ],
      
        height: 200,
        
    }).dxDataGrid('instance');

    
    //menu

    var tabsInstance = $("#tabs").dxTabs({
        dataSource: tabs,
        
        onItemClick(e) {

            if (e.itemData.id == 0) {
                if (IdSolicitudXAtencion >= 1) {
                    $("#Asuntos").show();
                }
            }
            else
                $("#Asuntos").hide();

            if (e.itemData.id == 1) {
                if (IdSolicitudXAtencion >= 1) {
                    $("#Documentos").show();
                }
            }
            else
                $("#Documentos").hide();

            if (e.itemData.id == 2) {
                if (IdSolicitudXAtencion >= 1) {
                    $("#Quejas").show();
                }
            }
            else
                $("#Quejas").hide();

            //if (e.itemData.id == 3) {
            //    if (IdSolicitudXAtencion >= 1) {
            //        $("#Notificaciones").show();
            //    }
            //}
            //else
            //    $("#Notificaciones").hide();

            if (e.itemData.id == 3) {
                if (IdSolicitudXAtencion >= 1) {
                    $("#Tramites").show();
                }
            }
            else
                $("#Tramites").hide();
        },
    }).dxTabs('instance');

    //selectbox

    var TipoAtencion = $('#SelectBoxTipoAtencion').dxSelectBox({
        width: 410,
        
        dataSource: new DevExpress.data.DataSource(
            {
                store: new DevExpress.data.CustomStore(
                    {
                        key: "idTipoAtencion",
                        loadMode: "raw",
                        load: function () {
                            return $.getJSON($("#SIM").data("url") + "AtencionUsuarios/api/AtencionesApi/GetTipoAtencionAsync")
                        }
                    })
            }),

        displayExpr: "nombre",
        valueExpr: "idTipoAtencion",
        searchEnabled: true,

        onValueChanged: function (data) {

            IdTipoAtencion = data.value
            cargarClaseAtenciones()
            claseAtencion.option("dataSource", claseAtenciones)
            claseAtencion.reset();

        }

    }).dxValidator({
        validationGroup: "ValAtencion",
        validationRules: [{
            type: "required",
            message: "Debe ingresar el tipo de atencion!"
        }]
    }).dxSelectBox("instance");


    var claseAtenciones = []

    function cargarClaseAtenciones() {
        var res = $.getJSON($("#SIM").data("url") + "AtencionUsuarios/api/AtencionesApi/GetClasesAtencionAsync?id=" + IdTipoAtencion);
        res.then(data => {

            claseAtenciones = data
            claseAtencion.option("dataSource", claseAtenciones)
        })
    }

   var claseAtencion = $('#SelectBoxClaseAtencion').dxSelectBox({
        width: 410,

        displayExpr: "nombre",
        valueExpr: "idClaseAtencion",
       searchEnabled: true,

       onValueChanged: function (data) {

           IdClaseAtencion = data.value
           cargarListaChequeo()
           listaChequeo.option("dataSource", lista)
           
       }
        

   }).dxValidator({
       validationGroup: "ValAtencion",
       validationRules: [{
           type: "required",
           message: "Debe ingresar la clase de atencion!"
       }]
   }).dxSelectBox("instance");

    var FormaAtencion = $('#SelectBoxFormaAtencion').dxSelectBox({
        width: 410,
        dataSource: new DevExpress.data.DataSource(
            {
                store: new DevExpress.data.CustomStore(
                    {
                        key: "idFormaAtencion",
                        loadMode: "raw",
                        load: function () {
                            return $.getJSON($("#SIM").data("url") + "AtencionUsuarios/api/AtencionesApi/GetFormaAtencionAsync")
                        }
                    })
            }),

        displayExpr: "nombre",
        valueExpr: "idFormaAtencion",
        searchEnabled: true,

       
    }).dxValidator({
        validationGroup: "ValAtencion",
        validationRules: [{
            type: "required",
            message: "Debe ingresar la forma de atencion!"
        }]
    }).dxSelectBox("instance");

   var Compromiso = $('#SelectBoxTipoCompromiso').dxSelectBox({
        width: 255,

        dataSource: new DevExpress.data.DataSource(
            {
                store: new DevExpress.data.CustomStore(
                    {
                        key: "codigoCompromiso",
                        loadMode: "raw",
                        load: function () {
                            return $.getJSON($("#SIM").data("url") + "AtencionUsuarios/api/AtencionesApi/GetCompromisosAsync")
                        }
                    })
            }),

        displayExpr: "nombre",
        valueExpr: "codigoCompromiso",
        searchEnabled: true,

   }).dxSelectBox("instance");

    var InstalacionC = $('#SelectBoxInstalacionAtencion').dxSelectBox({
        width: 510,

        placeholder: '',

    }).dxSelectBox("instance");

    //checkbox

    var EstadoAtencion = $('#EstadoAtencion').dxCheckBox({
        value: false,
    }).dxCheckBox('instance');

    //datebox

   var FechaCompromiso = $('#dateFechaCompromiso').dxDateBox({
        type: 'date',
        value: now,
   }).dxDateBox('instance');

    var FechaIReporte = $('#FechaIReporte').dxDateBox({
        displayFormat: 'dd/MM/yyyy',
        type: 'date',
    }).dxDateBox('instance');

    var FechaFReporte = $('#FechaFReporte').dxDateBox({
        displayFormat: 'dd/MM/yyyy',
        type: 'date',
    }).dxDateBox('instance'); 

    //popups

    var popupAsuntos = $("#PopupAsuntos").dxPopup({
        width: 1800,
        height: "auto",
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Vincular Solicitud",
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },
        
    }).dxPopup("instance");

    var popupQuejas = $("#PopupQuejas").dxPopup({
        width: "auto",
        height: "auto",
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Queja Ambiental",
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },
    }).dxPopup("instance");

    var popupNotificaciones = $("#PopupNotificaciones").dxPopup({
        width: "auto",
        height: "auto",
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Notificacion Personal",
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },
    }).dxPopup("instance");

    var popupTramites = $("#PopupTramites").dxPopup({
        width: "auto",
        height: "auto",
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Buscar Tramites",
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },
    }).dxPopup("instance");

    var popupAgregarAtencion = $("#popupAgregarAtencion").dxPopup({
        width: 1300,
        height: 760,
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Agregar Atención",
        onShowing: function () {                  // esto es para bloquear el scroll del body mientras el popup esta abierto
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },
    }).dxPopup("instance");

    var popupBuscarPersona = $("#popupBuscarPersona").dxPopup({
        width: 1000,
        height: "auto",
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Busqueda Persona",
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },
    }).dxPopup("instance");

    $("#popupBuscaDoc").dxPopup({
        width: 900,
        height: 800,
        showTitle: true,
        title: "Buscar Documento",
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },
    });

    $("#popDocumento").dxPopup({
        width: 900,
        height: 800,
        showTitle: true,
        title: "Visualizar Documento",
        dragEnabled: false,
        closeOnOutsideClick: true,
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },
    });

    var popupReporte = $("#popupReporte").dxPopup({
        width: "auto",
        height: 750,
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Reporte",
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },
    }).dxPopup("instance");

});//end document

const tabs = [
    {
        id: 0,
        text: 'Asuntos (Solicitudes) Relacionadas',
        icon: '',
       
    },
    {
        id: 1,
        text: 'Documentos Relacionados',
        icon: '',
       
    },
    {
        id: 2,
        text: 'Quejas Relacionadas',
        icon: '',
        
    },
    //{
    //    id: 3,
    //    text: 'Notificaciones Relacionadas',
    //    icon: '',
        
    //},
    {
        id: 3,
        text: 'Tramites',
        icon: '',
       
    },
];

var AtencionesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"IdAtencion","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'AtencionUsuarios/api/AtencionesApi/Atenciones', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: false,
            id: IdTercero
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var ListadoSolicitudesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"codigoSolicitud","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'AtencionUsuarios/api/AtencionesApi/ListadoSolicitudes', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: false,
            id: IdSolicitudXAtencion
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var BuscarSolicitudDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"cm","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'AtencionUsuarios/api/AtencionesApi/BuscarSolicitud', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: true            
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var ListadoQuejasDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"idQuejaAtencion","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'AtencionUsuarios/api/AtencionesApi/ListadoQuejas', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: false,
            id: IdSolicitudXAtencion
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var BuscarQuejasDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"CodigoQueja","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'AtencionUsuarios/api/AtencionesApi/BuscarQuejas', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: true
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var ListadoDocumentosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"idDocumento","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'AtencionUsuarios/api/AtencionesApi/ListadoDocumentos', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: false,
            id: IdSolicitudXAtencion
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var ListadotramitesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"codTramite","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'AtencionUsuarios/api/AtencionesApi/ListadoTramites', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: false,
            id: IdSolicitudXAtencion
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var BuscarTramitesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"CodTramite","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'AtencionUsuarios/api/AtencionesApi/BuscarTramites', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: true
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var AtencionReporteDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"FechaAtencion","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'AtencionUsuarios/api/AtencionesApi/ReporteAtenciones', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: false,
            fechaI: FechaIR,
            fechaF: FechaFR
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

function AsociaDocumento(Documento, Parametro) {
    var _popup = $("#popupBuscaDoc").dxPopup("instance");
    _popup.hide();
  //  alert("Documento :" + Documento + " Atencion :" + Parametro);
    if (Documento != "") {

        params = {
            idDocumento: Documento, idAtencion: Parametro
        };
        var _Ruta = $('#SIM').data('url') + "AtencionUsuarios/api/AtencionesApi/VincularDocumento";

        $.ajax({
            type: "POST",
            dataType: 'json',
            url: _Ruta,
            data: JSON.stringify(params),
            contentType: "application/json",
            crossDomain: true,
            headers: { 'Access-Control-Allow-Origin': '*' },
            success: function (data) {
                if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error'); 

           
                else {
                    DevExpress.ui.dialog.alert('Registro vinculado correctamente!');

                    $('#GidListadoDocumentos').dxDataGrid({ dataSource: ListadoDocumentosDataSource });
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                DevExpress.ui.dialog.alert('Ocurrió un problema Guardar Datos');
            }
        });
       
    }
}
/*$("#direc").show();*/