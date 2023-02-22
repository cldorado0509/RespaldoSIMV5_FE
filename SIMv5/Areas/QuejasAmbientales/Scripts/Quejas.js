
const today = new Date();

const year = today.getFullYear();

const month = `${today.getMonth() + 1}`.padStart(2, "0");

const day = `${today.getDate()}`.padStart(2, "0");

const FechaHoy = [day, month, year].join("/");

var Hora = today.getHours() + ':' + today.getMinutes() + ':' + today.getSeconds();


var UsuarioLogueado = "";

var ConsecutivoQueja = 0;

var CodQueja = -1;

var CodAfectacion = -1;

var CodRecurso = -1;

var CodMunicipio = -1;

var CodFun = -1;

var CodFormaQueja = -1;

var FechaGuardar = new Date();

var DiaGuardar = new Date().toDateString("yyyy/MM/dd");

var HoraGuardar = FechaGuardar.getHours() + ':' + FechaGuardar.getMinutes() + ':' + FechaGuardar.getSeconds();

var HoyGuardar = DiaGuardar + " " + HoraGuardar;

var IdExpedienteQueja = -1;

var DocumentoBuscado = -1;

var IdTercero = -1;

var IdInstalacion = -1;

var IdTipoTerceroQueja = -1;

var IdQuejaTercero = -1;

var CodRespuestaQueja = -1;

var IdTipoEstadoQueja = -1;

var CodEstadoFuncionarioQueja = -1;

var CodEstadoQueja = -1;

var FuncionarioNombre = "";

var TecnicoNombre = "";

var CodCitacionQueja = -1;

var CodComentarioQueja = -1;

var FunActualizarComentario = -1;// para que no se actualice el funcionairio en el comentario

var CodVisitaTecnicoQueja = -1;

var CodVisitaQueja = -1;

var IdExpDoc = -1;

var CodFunEstado = -1;

$(document).ready(function () {
    
    //txt
    var consecutivo = $("#txtConsecutivo").dxNumberBox({
        value: "",
        width: 150,
        disabled: true,
    }).dxNumberBox("instance");

    var annoQueja = $("#txtAnno").dxNumberBox({
        value: year,
        width: 100,
        disabled: true,
    }).dxNumberBox("instance");

    var fecha = $("#txtFecha").dxTextBox({
        value: FechaHoy,
        width: 100,
        disabled: true,
    }).dxTextBox("instance");

    var radicadoQueja = $("#txtRadicadoQueja").dxTextBox({
        
        width: 150,
    }).dxTextBox("instance");

    var userLog = $("#txtUserLog").dxTextBox({
        value: UsuarioLogueado,
        width: 150,
        disabled: true,
    }).dxTextBox("instance");

    var txtDocumentoTercero = $("#txtDocumentoTercero").dxNumberBox({
        min: 1,
        placeholder: 'Ingresar documento',
        value: "",
        width: 250,
        readOnly: false,
        onValueChanged: function () {
            var _Ruta = ($('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/Tercero");

            $.getJSON(_Ruta,
                {
                    id: txtDocumentoTercero.option("value")
                }).done(function (data) {

                    if (data.idTercero > 0) {
                        IdTercero = data.idTercero;
                        IdInstalacion = data.idInstalacion;
                        txtNombreTercero.option("value", data.rSocial);
                        txtDireccionTercero.option("value", data.direccion);
                        txtTelefonoTercero.option("value", data.telefono);
                       
               
                    } else {
                        IdTercero = -1;

                        txtNombreTercero.reset();
                        txtDireccionTercero.reset();
                        txtTelefonoTercero.reset();
                        

                        DevExpress.ui.dialog.alert('Documento no registrado!');
                    }


                }).fail(function (jqxhr, textStatus, error) {
                    DevExpress.ui.dialog.alert('Documento no registrado!');
                });
        }
    }).dxNumberBox("instance");

    var txtNombreTercero = $("#txtNombreTercero").dxTextBox({
        readOnly: true,
        value: "",
        width: 300,
    }).dxValidator({
        validationGroup: "TerceroGroup",
        validationRules: [{
            type: "required",
            message: "Debe asociarse un tercero!"
        }]
    }).dxTextBox("instance");

    var txtDireccionTercero = $("#txtDireccionTercero").dxTextBox({
        readOnly: true,
        value: "",
        width: 250,
    }).dxTextBox("instance");

    var txtTelefonoTercero = $("#txtTelefonoTercero").dxNumberBox({
        value: "",
        width: 250,
        readOnly: true
    }).dxNumberBox("instance");

    var txtDireccionDescriptiva = $("#txtDireccionDescriptiva").dxTextBox({
        placeholder: 'Ingresar direccion',
        readOnly: false,
        value: "",
        width: 250,
    }).dxTextBox("instance");

    var txtFechaAsociacion = $("#txtFechaAsociacion").dxTextBox({
        readOnly: true,
        value: FechaHoy,
        width: 250,
    }).dxTextBox("instance");

    var responsableQueja = $("#Responsable").dxTextBox({
        readOnly: true,
        
        width: 250,
    }).dxTextBox("instance");

    var userLogEstado = $("#txtUserLogEstado").dxTextBox({
        value: UsuarioLogueado,
        width: 150,
        disabled: true,
    }).dxTextBox("instance");

    //popup de terceros opcion natural

    var DocumentoNatural = $('#txtDocumentoNatural').dxNumberBox({
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
        validationGroup: "ValNatural",
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
        validationGroup: "ValNatural",
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
        validationGroup: "ValNatural",
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
        validationGroup: "ValNatural",
        validationRules: [
            {
                type: 'pattern',
                pattern: /^[^0-9]+$/,
                message: 'No uses digitos en el nombre',
            },
        ]
    }).dxTextBox("instance");

    var TelefonoNatural = $('#txtTelefonoNatural').dxNumberBox({
        disabled: true,
        showClearButton: true,
        placeholder: 'Ingresar telefono',
        value: "",
        width: 250,

    }).dxValidator({
        validationGroup: "ValNatural",
        validationRules: [{
            type: 'pattern',
            pattern: /^([0-9]){7,10}$/,
            message: 'Ingrese un telefono con un formato correcto',
        }],
    }).dxNumberBox("instance");

    var EmailNatural = $('#txtEmailNatural').dxTextBox({
        disabled: true,
        showClearButton: true,
        placeholder: 'Ingresar email',
        value: "",
        width: 250,
    }).dxValidator({
        validationGroup: "ValNatural",
        validationRules: [{
            type: 'email',
            message: 'Ingrese un correo valido',
        }],
    }).dxTextBox("instance");

    var DireccionNatural = $('#txtDireccionNatural').dxTextBox({
        disabled: true,
        showClearButton: true,
        placeholder: 'Ingresar Direccion',
        value: "",
        width: 250,

    }).dxTextBox("instance");

     //popup de terceros opcion juridica

    var DocumentoJuridico = $('#txtDocumentoJuridico').dxNumberBox({
        min: 1,
        disabled: false,
        showClearButton: true,
        placeholder: 'Ingresar documento',
        value: "",
        width: 250
    }).dxValidator({
        validationGroup: "ValJuridico",
        validationRules: [{
            type: "required",
            message: "Debe ingresar el Documento!"
        }]
    }).dxNumberBox("instance");

    var TelefonoJuridico = $('#txtTelefonoJuridico').dxNumberBox({
        disabled: true,
        showClearButton: true,
        placeholder: 'Ingresar telefono',
        value: "",
        width: 250,

    }).dxValidator({
        validationGroup: "ValJuridico",
        validationRules: [{
            type: 'pattern',
            pattern: /^([0-9]){7,10}$/,
            message: 'Ingrese un telefono con un formato correcto',
        }],
    }).dxNumberBox("instance");

    var EmailJuridico = $('#txtEmailJuridico').dxTextBox({
        disabled: true,
        showClearButton: true,
        placeholder: 'Ingresar email',
        value: "",
        width: 250,
    }).dxValidator({
        validationGroup: "ValJuridico",
        validationRules: [{
            type: 'email',
            message: 'Ingrese un correo valido',
        }],
    }).dxTextBox("instance");

    var DireccionJuridico = $('#txtDireccionJuridico').dxTextBox({
        disabled: true,
        showClearButton: true,
        placeholder: 'Ingresar Direccion',
        value: "",
        width: 250,

    }).dxTextBox("instance");

    var NombreJuridico = $('#txtNombreJuridico').dxTextBox({
        disabled: true,
        showClearButton: true,
        placeholder: '',
        value: "",
        width: 250
    }).dxValidator({
        validationGroup: "ValJuridico",
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

    //respuestas

    var txtRadicado = $("#txtRadicado").dxNumberBox({
        value: "",
        width: 150,
        disabled: false,
    }).dxValidator({
        validationGroup: "RespuestaGroup",
        validationRules: [{
            type: "required",
            message: "Debe ingresar el radicado!"
        }]
    }).dxNumberBox("instance");

    var userLogComentario = $("#txtUserLogComentario").dxTextBox({
        value: UsuarioLogueado,
        width: 150,
        disabled: true,
    }).dxTextBox("instance");


    //RadioButton
    var Genero = $('#radioGenero').dxRadioGroup({
        disabled: true,
        items: ['M', 'F'],
        value: "",
        layout: 'horizontal',
    }).dxRadioGroup("instance");

    //funcion limpiar

    function ClearAgregarQueja() {
       
        recurso.reset();
        fecha.option("value", FechaHoy);
        Afectacion.reset();
        municipio.reset();
        formaQueja.reset();
        asuntoQueja.reset();
        direccion.reset();
        comentariosQueja.reset();
        radicadoQueja.reset();

    }

    //botones

    var btnNuevaQueja = $("#btnNuevaQueja").dxButton({
        hint: "Crear una queja",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 80,
        height: 36,
        icon: 'add',
        onClick: function () {

            ClearAgregarQueja();
            CodQueja = -1;
            popupNuevaQueja.show();
            var _Ruta = ($('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/BuscarFuncionario");

            $.getJSON(_Ruta,
                {
                    id:-1
                }).done(function (data) {

                    var nombre = data[0].nombres;
                    //var apellido = data[0].apellidos;
                    //var nombreCompleto = nombre + " " + apellido;
                    userLog.option("value", nombre);

                }).fail(function (jqxhr, textStatus, error) {
                    
                });

            var _Ruta1 = ($('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/Consecutivo");

            $.getJSON(_Ruta1,).done(function (data) {

                ConsecutivoQueja = data + 1;
                consecutivo.option("value", ConsecutivoQueja);

                }).fail(function (jqxhr, textStatus, error) {

                });


            var _Ruta = ($('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/Responsablesasync");

            $.getJSON(_Ruta,
                {
                    id: -1
                }).done(function (data) {

                    var nombre = data[0].nombres;
                    var apellido = data[0].apellidos;
                    var nombreCompleto = nombre + " " + apellido;
                    responsableQueja.option("value", nombreCompleto);

                }).fail(function (jqxhr, textStatus, error) {

                });
        }
    

        
    }).dxButton("instance");

    var btnGuardarQueja = $("#btnGuardarQueja").dxButton({
        visible: true,
        text: "Guardar",
        type: "success",
        width: 80,
        height: 36,
        icon: 'save',
        validationGroup: "QuejaGroup",

        onClick: function (e) {

            var result = e.validationGroup.validate();
            if (result.isValid) {


                var codigoQueja = CodQueja;
                var codigoAfectacion = CodAfectacion;
                var codigoRecurso = CodRecurso;
                var codigoMunicipio = CodMunicipio;
                var asunto = asuntoQueja.option("value");
                var fechaRecepcion = HoyGuardar;
                var recibe = userLog.option("value");
                var idExpediente = "";
                var comentarios = comentariosQueja.option("value");
                var codigoFormaQueja = CodFormaQueja;
                var codigoAbogado = CodFun;
                var queja = consecutivo.option("value");
                var anno = annoQueja.option("value");
                var direc = direccion.option("value");
                var radicado = radicadoQueja.option("value");

                var params = {};

                if (CodQueja > 0) {
                    params = {
                        codigoQueja: codigoQueja, codigoAfectacion: codigoAfectacion, codigoRecurso: codigoRecurso, codigoUsuario: "", codigoComponente: "", codigoMunicipio: codigoMunicipio, asunto: asunto, fechaRecepcion: fechaRecepcion, recibe: recibe, remitidoA: "", radicado: radicado, fechaTecnico: "", comentarios: comentarios, fechaAbogado: "", direccion: direc, infractor: "", telefonoInfractor: "", direccionInfractor: "", fechaEstado: "", funcionarioEstado: "", codigoTipoEstado: "", codigoFormaQueja: codigoFormaQueja, codigoTecnico: "", codigoAbogado: "", codigoCategoria: "", queja: queja, anno: anno, radicadoVinculo: "", fechaRadicadoVinculo: "", idExpediente: idExpediente // este es para el actualizar la queja
                    };
                } else {
                    params = {
                        codigoAfectacion: codigoAfectacion, codigoRecurso: codigoRecurso, codigoUsuario: "", codigoComponente: "", codigoMunicipio: codigoMunicipio, asunto: asunto, fechaRecepcion: fechaRecepcion, recibe: recibe, remitidoA: "", radicado: radicado, fechaTecnico: "", comentarios: comentarios, fechaAbogado: "", direccion: direc, infractor: "", telefonoInfractor: "", direccionInfractor: "", fechaEstado: "", funcionarioEstado: "", codigoTipoEstado: "", codigoFormaQueja: codigoFormaQueja, codigoTecnico: "", codigoAbogado: "", codigoCategoria: "", queja: queja, anno: anno, radicadoVinculo: "", fechaRadicadoVinculo: "", idExpediente: idExpediente//este es el que crea la queja
                    };
                }

                var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/GuardarQueja";
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    url: _Ruta,
                    data: JSON.stringify(params),
                    contentType: "application/json",
                    crossDomain: true,
                    headers: { 'Access-Control-Allow-Origin': '*' },
                    success: function (data) {

                        if (data.resp === "Error")
                            DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Guardar Datos');

                        else {
                            DevExpress.ui.dialog.alert('Datos Guardados correctamente', 'Guardar Datos');
                            popupNuevaQueja.hide();
                            CodQueja= -1;
                            GidListadoQuejas.dxDataGrid("instance").option("dataSource", QuejasDataSource);
                        }

                    },
                    error: function (xhr, textStatus, errorThrown) {
                        DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                    }
                });

            } else DevExpress.ui.dialog.alert('Debes llenar correctamente los datos!');
        }
    }).dxButton("instance");

    $("#btnBuscarExpediente").dxButton({
        hint: "Vincular un documento",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,

        onClick: function () {
            var _popup = $("#popupBuscaExp").dxPopup("instance");
            _popup.show();
            $('#BuscarExp').attr('src', $('#SIM').data('url') + 'Utilidades/BuscarExpediente?popup=true');
        }
    }).dxButton("instance");

    var btnNuevoTercero = $("#btnNuevoTercero").dxButton({
        hint: "Nuevo Tercero",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,

        onClick: function () {
            popupTerceros.show();
            IdQuejaTercero = -1;


            //txtDocumentoTercero.reset();
            //slctboxTipoTercero.reset();
            txtNombreTercero.reset();
            txtDireccionTercero.reset();
            txtTelefonoTercero.reset();
            ObservacionTercero.reset();
            txtDireccionDescriptiva.reset();
            $('#txtNombreTercero').dxValidator('instance').reset();


          
            slctboxTipoTercero.dxSelectBox("instance").option("value", "null");
            //$('#txtDocumentoTercero').reset();
            //$('#txtDocumentoTercero').dxValidator('instance').reset();
          
           
        }
    }).dxButton("instance");

    var btnlimpiarTercero = $("#btnLimpiarTercero").dxButton({
        hint: "Limpiar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'clear',
        onClick: function () {

            
            txtNombreTercero.reset();
            txtDireccionTercero.reset();
            txtTelefonoTercero.reset();
            ObservacionTercero.reset();
            txtDireccionDescriptiva.reset();
            
            

           
        }

    }).dxButton("instance");

    $("#btnpopupBuscar").dxButton({
        hint: "Buscar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'find',
        onClick: function () {
            popupBuscarTercero.show();

        }
    }).dxButton("instance");//abre el buscardor detalldo de natural y juridico

    $("#btnVincularTercero").dxButton({
        hint: "Vincular Tercero",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'check',
        validationGroup: "TerceroGroup",
        onClick: function (e) {
            var result = e.validationGroup.validate();
            if (result.isValid) {

                var codQ = CodQueja;
                var idT = IdTercero;
                var idI = IdInstalacion;
                var idTtq = IdTipoTerceroQueja;
                var fechatq = today;
                var desc = ObservacionTercero.option("value");
                var esp = txtDireccionDescriptiva.option("value");
                var idQt = IdQuejaTercero;

                if (IdQuejaTercero > 0) {
                    params = {
                        idQuejaTercero: idQt, codigoQueja: codQ, idTercero: idT, idInstalacion: idI, idTipoTerceroQueja: idTtq, fechaTerceroQueja: fechatq, descripcion: desc, especial: esp

                    };
                } else {
                    params = {
                        codigoQueja: codQ, idTercero: idT, idInstalacion: idI, idTipoTerceroQueja: idTtq, fechaTerceroQueja: fechatq, descripcion: desc, especial: esp
                    };
                }




                var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/VincularQuejaTerceroTAsync";
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
                            GidListadoTerceros.dxDataGrid("instance").option("dataSource", TercerosQuejasDataSource);
                            popupTerceros.hide();
                            popupBuscarTercero.hide();
                        }
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        DevExpress.ui.dialog.alert('Ocurrió un problema Guardar Datos');
                    }
                });
            } else {
                DevExpress.ui.dialog.alert('Debe ingresar correctamente los datos!');
            }

        }
    }).dxButton("instance");

    $("#btnCancelarTercero").dxButton({
        hint: "Cancelar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'remove',
        onClick: function () {

            popupTerceros.hide();
        }
    }).dxButton("instance");

    $("#btnBuscarTerceroNatural").dxButton({
        hint: "Buscar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'find',
        onClick: function () {
            var _Ruta = ($('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/TerceroNatural");

            $.getJSON(_Ruta,
                {
                    id: DocumentoNatural.option("value")
                }).done(function (data) {

                    if (data.idTercero > 0) {
                        DocumentoBuscado = data.documentoN;
                        IdTercero = data.idTercero;
                        PrimerApellido.option("value", data.primerApellido);
                        SegundoApellido.option("value", data.segundoApellido);
                        PrimerNombre.option("value", data.primerNombre);
                        SegundoNombre.option("value", data.segundoNombre);
                        TelefonoNatural.option("value", data.telefono);
                        EmailNatural.option("value", data.correo);
                        Genero.option("value", data.genero);
                        DireccionNatural.option("value", data.direccion);
                        btnAceptarNatural.option("visible", true);
                        BtnEditarTerceroNatural.option("visible", true);

                        DocumentoNatural.option("disabled", true);

                    } else {
                        IdTercero = -1;
                        DevExpress.ui.dialog.alert('Documento no registrado!');

                    }


                }).fail(function (jqxhr, textStatus, error) {

                });

        }
    }).dxButton("instance");

    var btnlimpiarTerceroNatural = $("#btnLimpiarTerceroNatural").dxButton({
        hint: "Limpiar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'clear',
        onClick: function () {

            DocumentoNatural.reset();
            PrimerApellido.reset();
            SegundoApellido.reset();
            PrimerNombre.reset();
            SegundoNombre.reset();
            TelefonoNatural.reset();
            EmailNatural.reset();
            DireccionNatural.reset();
            Genero.reset();
            PrimerApellido.option("disabled", true);
            SegundoApellido.option("disabled", true);
            PrimerNombre.option("disabled", true);
            SegundoNombre.option("disabled", true);
            TelefonoNatural.option("disabled", true);
            EmailNatural.option("disabled", true);
            Genero.option("disabled", true);
            btnAceptarNatural.option("visible", false);
            BtnEditarTerceroNatural.option("visible", false);
            btnActualizarTerceroNatural.option("visible", false);
            DocumentoNatural.option("disabled", false);
          
        }

    }).dxButton("instance");

    var btnAceptarNatural = $("#btnAceptarNatural").dxButton({
        hint: "",
        stylingMode: "contained",
        text: "Aceptar",
        type: "success",
        width: 70,
        height: 36,
        icon: 'todo',
        visible: false,
        onClick: function () {
            txtDocumentoTercero.option("value", DocumentoBuscado);
            popupBuscarTercero.hide();
            DocumentoNatural.reset();
            DocumentoNatural.option("disabled", false);
            PrimerApellido.reset();
            SegundoApellido.reset();
            PrimerNombre.reset();
            SegundoNombre.reset();
            TelefonoNatural.reset();
            EmailNatural.reset();
            DireccionNatural.reset();
            Genero.reset();
            btnAceptarNatural.option("visible", false);
            BtnEditarTerceroNatural.option("visible", false);
            $('#txtDocumentoNatural').dxValidator('instance').reset();
        }
    }).dxButton("instance");

    var BtnEditarTerceroNatural = $("#btnEditarTerceroNatural").dxButton({
        hint: "Editar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'edit',
        visible: false,
        onClick: function () {
            DocumentoNatural.option("disabled", true);
            PrimerApellido.option("disabled", false);
            SegundoApellido.option("disabled", false);
            PrimerNombre.option("disabled", false);
            SegundoNombre.option("disabled", false);
            TelefonoNatural.option("disabled", false);
            EmailNatural.option("disabled", false);
            Genero.option("disabled", false);
            btnActualizarTerceroNatural.option("visible", true);
            BtnEditarTerceroNatural.option("visible", false);
            btnAceptarNatural.option("visible", false);
        }
    }).dxButton("instance");

    var btnActualizarTerceroNatural = $("#btnActualizarTerceroNatural").dxButton({
        hint: "Actualizar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'save',
        visible: false,
        validationGroup: "ValNatural",

        onClick: function (e) {
            var result = e.validationGroup.validate();
            if (result.isValid) {


                var id = IdTercero;
                var documento = DocumentoNatural.option("value");
                var primerApellido = PrimerApellido.option("value");
                var segundoApellido = SegundoApellido.option("value");
                var primerNombre = PrimerNombre.option("value");
                var segundoNombre = SegundoNombre.option("value");
                var telefono = TelefonoNatural.option("value");
                var email = EmailNatural.option("value");
                var genero = Genero.option("value");

                params = {
                    idTercero: id, documentoN: documento, primerNombre: primerNombre, segundoNombre: segundoNombre, primerApellido: primerApellido, segundoApellido: segundoApellido, genero: genero, telefono: telefono, correo: email, direccion: "", nombreCompleto: "",
                };


                var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/ActualizarTerceroNatural";
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
                            DocumentoNatural.option("disabled", false);
                            PrimerApellido.option("disabled", true);
                            SegundoApellido.option("disabled", true);
                            PrimerNombre.option("disabled", true);
                            SegundoNombre.option("disabled", true);
                            TelefonoNatural.option("disabled", true);
                            EmailNatural.option("disabled", true);
                            Genero.option("disabled", true);
                            btnActualizarTerceroNatural.option("visible", false);
                            BtnEditarTerceroNatural.option("visible", true);
                            btnAceptarNatural.option("visible", true);
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

    $("#btnBuscarTerceroJuridico").dxButton({
        hint: "Buscar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'find',
        onClick: function () {
            var _Ruta = ($('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/TerceroJuridico");

            $.getJSON(_Ruta,
                {
                    id: DocumentoJuridico.option("value")
                }).done(function (data) {

                    if (data.idTercero > 0) {
                        DocumentoBuscado = data.documentoN;
                        IdTercero = data.idTercero;
                        NombreJuridico.option("value", data.nombreCompleto);
                        TelefonoJuridico.option("value", data.telefono);
                        EmailJuridico.option("value", data.correo);
                        DireccionJuridico.option("value", data.direccion);
                        btnAceptarJuridico.option("visible", true);
                        BtnEditarTerceroJuridico.option("visible", true);
                        DocumentoJuridico.option("disabled", true);

                    } else {
                        IdTercero = -1;
                        DevExpress.ui.dialog.alert('Documento no registrado!');
                        
                    }


                }).fail(function (jqxhr, textStatus, error) {

                });

        }
    }).dxButton("instance");

    var BtnlimpiarTerceroJuridico = $("#btnLimpiarTerceroJuridico").dxButton({
        hint: "Limpiar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'clear',
        onClick: function () {

            DocumentoJuridico.reset();
            NombreJuridico.reset();
       
            TelefonoJuridico.reset();
            EmailJuridico.reset();
            DireccionJuridico.reset();
            TelefonoJuridico.option("disabled", true);
            EmailJuridico.option("disabled", true);
           
            btnAceptarJuridico.option("visible", false);
            BtnEditarTerceroJuridico.option("visible", false);
            btnActualizarTercerojuridico.option("visible", false);
            DocumentoJuridico.option("disabled", false);
           

          
        }

    }).dxButton("instance");

    var btnAceptarJuridico = $("#btnAceptarJuridico").dxButton({
        hint: "",
        stylingMode: "contained",
        text: "Aceptar",
        type: "success",
        width: 70,
        height: 36,
        icon: 'todo',
        visible: false,
        onClick: function () {
            txtDocumentoTercero.option("value", DocumentoBuscado);
            popupBuscarTercero.hide();
            DocumentoJuridico.reset();
            DocumentoJuridico.option("disabled", false);
            NombreJuridico.reset();
            
            TelefonoJuridico.reset();
            EmailJuridico.reset();
            DireccionJuridico.reset();
           
            btnAceptarJuridico.option("visible", false);
            BtnEditarTerceroJuridico.option("visible", false);
            $('#txtDocumentoJuridico').dxValidator('instance').reset();
        }
    }).dxButton("instance");

    var BtnEditarTerceroJuridico = $("#btnEditarTerceroJuridico").dxButton({
        hint: "Editar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'edit',
        visible: false,
        onClick: function () {
            DocumentoJuridico.option("disabled", true);
            NombreJuridico.option("disabled", false);
            TelefonoJuridico.option("disabled", false);
            EmailJuridico.option("disabled", false);
          
            btnActualizarTercerojuridico.option("visible", true);
            BtnEditarTerceroJuridico.option("visible", false);
            btnAceptarJuridico.option("visible", false);
        }
    }).dxButton("instance");

    var btnActualizarTercerojuridico = $("#btnActualizarTerceroJuridico").dxButton({
        hint: "Actualizar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'save',
        visible: false,
        validationGroup: "ValJuridico",

        onClick: function (e) {
            var result = e.validationGroup.validate();
            if (result.isValid) {


                var id = IdTercero;
                var documento = DocumentoJuridico.option("value");
                
                var Nombre = NombreJuridico.option("value");
                var telefono = TelefonoJuridico.option("value");
                var email = EmailJuridico.option("value");
               
                params = {
                    idTercero: id, documentoN: documento, nombreCompleto: Nombre, telefono: telefono, correo: email, direccion: ""
                };


                var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/ActualizarTerceroJuridico";
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
                            DocumentoJuridico.option("disabled", false);
                            NombreJuridico.option("disabled", true);
                            TelefonoJuridico.option("disabled", true);
                            EmailJuridico.option("disabled", true);
                            
                            btnActualizarTercerojuridico.option("visible", false);
                            BtnEditarTerceroJuridico.option("visible", true);
                            btnAceptarJuridico.option("visible", true);
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

    var btnNuevaRespuesta = $("#btnNuevaRespuesta").dxButton({
        hint: "Crear una queja",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 80,
        height: 36,
        icon: 'add',
        onClick: function () {

            popupNuevaRespuesta.show();
            
        }



    }).dxButton("instance");

    $("#btnVincularRespuesta").dxButton({
        hint: "Vincular Respuesta",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'check',
        validationGroup: "RespuestaGroup",
        onClick: function (e) {
            var result = e.validationGroup.validate();
            if (result.isValid) {
                var codQ = CodQueja;
                var radicado = txtRadicado.option("value");
                var fecha = dateFechaRespuesta.option("value");
                var asunto = asuntoRespuesta.option("value");
                var codRQ = CodRespuestaQueja;// en el editar respuesta se cambia este valor

                if (CodRespuestaQueja > 0) {
                    params = {
                        codContestaQueja: codRQ, codigoQueja: codQ, radicado: radicado, fecha: fecha, asunto: asunto, codigoTramite: "", codigoDocumento: ""

                    };
                } else {
                    params = {
                        codigoQueja: codQ, radicado: radicado, fecha: fecha, asunto: asunto, codigoTramite: "", codigoDocumento: ""
                    };
                }


                var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/VincularRespuestaQueja";
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
                            DevExpress.ui.dialog.alert('Registrado correctamente!');
                            GidListadoRespuestas.dxDataGrid("instance").option("dataSource", RespuestasDataSource);
                            popupNuevaRespuesta.hide();
                            CodRespuestaQueja = -1;


                        }
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        DevExpress.ui.dialog.alert('Ocurrió un problema Guardar Datos');
                    }
                });
            } else {
                DevExpress.ui.dialog.alert('Debe ingresar correctamente los datos!');
            }
        }
    }).dxButton("instance");

    var btnNuevoEstado = $("#btnNuevoEstado").dxButton({
        hint: "Crear un estado",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 80,
        height: 36,
        icon: 'add',
        onClick: function () {

            popupNuevoEstado.show();

            var _Ruta = ($('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/BuscarFuncionario");

            $.getJSON(_Ruta,
                {
                    id: -1
                }).done(function (data) {

                    var nombres = data[0].nombres;
                    CodFunEstado = data[0].codFuncionario;
                    
                    userLogEstado.option("value", nombres);

                }).fail(function (jqxhr, textStatus, error) {

                });

        }



    }).dxButton("instance");

    $("#btnVincularEstado").dxButton({
        hint: "Vincular Estado",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'check',
        validationGroup: "EstadoGroup",
        onClick: function (e) {
            var result = e.validationGroup.validate();
            if (result.isValid) {
            var codQ = CodQueja;
            var funcionario = userLogEstado.option("value");
            var fecha = dateFechaEstado.option("value");
            var codTE = selectBoxTipoEstado.option("value");
            var codF = CodFunEstado;
            var codEQ = CodEstadoQueja;// en el editar respuesta se cambia este valor

            if (CodEstadoQueja > 0) {
                params = {
                    codEstadoQueja: codEQ, fechaEstado: fecha, funcionario: funcionario, codQueja: codQ, codTipoEstadoQueja: codTE, codFuncionario: codF

                };
            } else {
                params = {
                    fechaEstado: fecha, funcionario: funcionario, codQueja: codQ, codTipoEstadoQueja: codTE, codFuncionario: codF
                };
            }


            var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/VincularEstadoQueja";
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
                        DevExpress.ui.dialog.alert('Registrado correctamente!');
                        GidListadoEstados.dxDataGrid("instance").option("dataSource", EstadosDataSource);
                        popupNuevoEstado.hide();
                        CodEstadoQueja = -1;


                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema Guardar Datos');
                }
            });
            } else {
                DevExpress.ui.dialog.alert('Debe ingresar correctamente los datos!');
            }
        }
    }).dxButton("instance");

    var btnNuevaCitacion = $("#btnNuevaCitacion").dxButton({
        hint: "Crear una citacion",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 80,
        height: 36,
        icon: 'add',
        onClick: function () {

            popupNuevaCitacion.show();

        }



    }).dxButton("instance");

    $("#btnVincularCitacion").dxButton({
        hint: "Vincular Citacion",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'check',
        validationGroup: "CitacionGroup",
        onClick: function (e) {
            var result = e.validationGroup.validate();
            if (result.isValid) {

                var codQ = CodQueja;
                var codObjeto = selectBoxObjetoCitacion.option("value");;
                var fecha = dateFechaCitacion.option("value");
                var observacion = ObservacionCitacion.option("value");
                var hora = Hora;
                var codCQ = CodCitacionQueja;// en el editar respuesta se cambia este valor

                if (CodCitacionQueja > 0) {
                    params = {
                        codCitacionQueja: codCQ, codigoQueja: codQ, codigoObjetoCitacion: codObjeto, fechaCitacion: fecha, observacion: observacion, hora: hora

                    };
                } else {
                    params = {
                        codigoQueja: codQ, codigoObjetoCitacion: codObjeto, fechaCitacion: fecha, observacion: observacion, hora: hora
                    };
                }


                var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/VincularCitacionQueja";
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
                            DevExpress.ui.dialog.alert('Registrado correctamente!');
                            GidListadoCitaciones.dxDataGrid("instance").option("dataSource", CitacionesDataSource);
                            popupNuevaCitacion.hide();
                            CodCitacionQueja = -1;


                        }
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        DevExpress.ui.dialog.alert('Ocurrió un problema Guardar Datos');
                    }
                });
            } else {
                DevExpress.ui.dialog.alert('Debe ingresar correctamente los datos!');
            }
        }
    }).dxButton("instance");

    var btnNuevoComentario = $("#btnNuevoComentario").dxButton({
        hint: "Crear un comentario",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 100,
        height: 36,
        icon: 'add',
        onClick: function () {

            popupNuevoComentario.show();
            var _Ruta = ($('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/BuscarFuncionario");

            $.getJSON(_Ruta,
                {
                    id: -1
                }).done(function (data) {
                   
                    var nombre = data[0].nombres;
                    //var apellido = data[0].apellidos;
                    //var nombreCompleto = nombre + " " + apellido;
                    userLogComentario.option("value", nombre);

                }).fail(function (jqxhr, textStatus, error) {

                });

        }



    }).dxButton("instance");

    $("#btnVincularComentario").dxButton({
        hint: "Vincular Comentario",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'check',
        validationGroup: "ComentarioGroup",
        onClick: function (e) {
            var result = e.validationGroup.validate();
            if (result.isValid) {
            var codQ = CodQueja;
            var comentario = ComentarioQueja.option("value");
            var fecha = dateFechaCitacion.option("value");
           
            var codCQ = CodComentarioQueja;// en el editar respuesta se cambia este valor

            if (CodComentarioQueja > 0) {
                params = {
                    idQuejaComentario: codCQ, comentario: comentario, idFuncionario: FunActualizarComentario, idTercero: "", fechaComentario: fecha, idQueja: codQ

                };
            } else {
                params = {
                    comentario: comentario, idFuncionario: "", idTercero: "", fechaComentario: fecha, idQueja: codQ
                };
            }


            var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/VincularQuejaComentario";
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
                        DevExpress.ui.dialog.alert('Registrado correctamente!');
                        GidListadoComentarios.dxDataGrid("instance").option("dataSource", ComentariosDataSource);
                        popupNuevoComentario.hide();
                        CodComentarioQueja = -1;


                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema Guardar Datos');
                }
            });
            } else {
                DevExpress.ui.dialog.alert('Debe ingresar correctamente los datos!');
            }
        }
    }).dxButton("instance");

    var btnNuevaVisita = $("#btnNuevaVisita").dxButton({
        hint: "Crear una Visita",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 80,
        height: 36,
        icon: 'add',
        onClick: function () {

            popupNuevaVisita.show();

        }



    }).dxButton("instance");

    $("#btnVincularVisita").dxButton({
        hint: "Vincular Visita",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'check',
        validationGroup: "VisitaGroup",
        onClick: function (e) {
            var result = e.validationGroup.validate();
            if (result.isValid) {
            var codQ = CodQueja;
            var tecnico = TecnicoNombre;
            var fecha = dateFechaVisita.option("value");
            var codOV = selectBoxObjetoVisita.option("value");
            var codT = CodVisitaTecnicoQueja;
            var observacion = ObservacionVisita.option("value");
            var codVQ = CodVisitaQueja;// en el editar respuesta se cambia este valor

            if (CodVisitaQueja > 0) {
                params = {
                    codVisitaQueja: codVQ, codigoQueja: codQ, codigoObjetoVisita: codOV, fechaVisita: fecha, tecnico: tecnico, observacion: observacion, codigoTecnico: codT

                };
            } else {
                params = {
                    codigoQueja: codQ, codigoObjetoVisita: codOV, fechaVisita: fecha, tecnico: tecnico, observacion: observacion, codigoTecnico: codT
                };
            }


            var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/VincularVisitaQueja";
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
                        DevExpress.ui.dialog.alert('Registrado correctamente!');
                        GidListadoVisitas.dxDataGrid("instance").option("dataSource", VisitasDataSource);
                        popupNuevaVisita.hide();
                        CodVisitaQueja = -1;


                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema Guardar Datos');
                }
            });
            } else {
                DevExpress.ui.dialog.alert('Debe ingresar correctamente los datos!');
            }
        }
    }).dxButton("instance");

    var btnNuevoInforme = $("#btnNuevoInforme").dxButton({
        hint: "Vincular Informe",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 80,
        height: 36,
        icon: 'add',
        onClick: function () {

            popupNuevoInforme.show();

        }



    }).dxButton("instance");

    var btnNuevoExpediente = $("#btnNuevoExpediente").dxButton({
        hint: "Vincular Expediente",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 80,
        height: 36,
        icon: 'add',
        onClick: function () {

            popupNuevoExpediente.show();

        }



    }).dxButton("instance");

    var btnNuevoTramite = $("#btnNuevoTramite").dxButton({
        hint: "Vincular Tramite",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 80,
        height: 36,
        icon: 'add',
        onClick: function () {

            popupNuevoTramite.show();

        }



    }).dxButton("instance");

    //ventana expedientes

    $("#btnIndices").dxButton({
        text: "Indices Expediente",
        icon: "fields",
        hint: 'Indices del documento',
        visible: false,
        onClick: function () {
            var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/IndicesExpediente";
            $.getJSON(_Ruta, { IdExp: IdExpDoc })
                .done(function (data) {
                    if (data != null) {
                        showIndices(data);
                    }
                }).fail(function (jqxhr, textStatus, error) {
                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Indices del documento');
                });
        }
    });

    var popupInd = null;

    var showIndices = function (data) {
        Indices = data;
        if (popupInd) {
            popupInd.option("contentTemplate", popupOptInd.contentTemplate.bind(this));
        } else {
            popupInd = $("#PopupIndicesExp").dxPopup(popupOptInd).dxPopup("instance");
        }
        popupInd.show();
    };

    var popupOptInd = {
        width: 700,
        height: "auto",
        hoverStateEnabled: true,
        title: "Indices del Expediente",
        visible: false,
        closeOnOutsideClick: true,
        contentTemplate: function () {
            var Content = "";
            $.each(Indices, function (key, value) {
                Content += "<p>" + value.INDICE + " : <span><b>" + value.VALOR + "</b></span></p>";
            });
            return $("<div />").append(
                $("<p><b>Indices expediente " + NomExpediente + "</b></p>"),
                $("<br />"),
                Content
            );
        }
    };

    //grids

    var GidListadoQuejas = $("#GidListadoQuejas").dxDataGrid(
        {
            dataSource: QuejasDataSource,
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
                visible: true,
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

                //{ dataField: 'CodigoQueja', width: '10%', caption: 'Codigo', alignment: 'left' },
                { dataField: 'NroQueja', width: '5%', caption: 'Nro Queja', alignment: 'center'},
                { dataField: 'Anno', width: '5%', caption: 'Año', alignment: 'left' },
                { dataField: 'FechaRecepcion', width: '9%', caption: 'Fecha recepcion', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy'},
                { dataField: 'Radicado', width: '8%', caption: ' Nro Radicado', alignment: 'left'},
                { dataField: 'Asunto', width: '22%', caption: 'Asunto', alignment: 'center' },
                //{ dataField: 'NombreTipoEstadoQueja', width: '9%', caption: 'Estado', alignment: 'center' },
                { dataField: 'NombreRecurso', width: '9%', caption: 'Recurso', alignment: 'center' },
                { dataField: 'NombreAfectacion', width: '9%', caption: 'Afectacion', alignment: 'center' },
                { dataField: 'Recibe', width: '9%', caption: 'Recibe', alignment: 'center'},

                {
                    width: '5%',
                    caption: "Editar",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        $('<div/>').dxButton({
                            icon: 'edit',


                            hint: 'Editar Queja',
                            onClick: function () {

                                var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/ObtenerQueja";
                                $.getJSON(_Ruta,
                                    {
                                        id: options.data.CodigoQueja
                                    }).done(function (data) {
                                        if (data !== null) {
                                            popupNuevaQueja.show();
                                           
                                            consecutivo.option("value", data.queja);
                                            annoQueja.option("value", data.anno);
                                            fecha.option("value", data.fechaRecepcion);
                                            recurso.option("value", data.codigoRecurso);
                                            Afectacion.option("value", data.codigoAfectacion);
                                            municipio.option("value", data.codigoMunicipio);
                                            responsableQueja.option("value", data.nombreAbogado);
                                            formaQueja.option("value", data.codigoFormaQueja);
                                            asuntoQueja.option("value", data.asunto);
                                            direccion.option("value", data.direccion);
                                            comentariosQueja.option("value", data.comentarios);
                                            userLog.option("value", data.recibe);
                                        }
                                    }).fail(function (jqxhr, textStatus, error) {
                                        DevExpress.ui.dialog.alert('Ocurrió un error');
                                    });


                            }
                        }).appendTo(container);
                    }

                },

                {
                    width: '5%',
                    caption: "Imprimir",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        $('<div/>').dxButton({
                            icon: 'print',


                            hint: 'Imprimir Queja',
                            onClick: function () {
                                var _popup = $("#popReporte").dxPopup("instance");
                                _popup.show();
                                $("#Reporte").attr("src", $('#SIM').data('url') + "QuejasAmbientales/Quejas/ReporteQueja?id=" + options.data.CodigoQueja)

                               

                            }
                        }).appendTo(container);
                    }

                },

                //{
                //    width: '5%',
                //    caption: "Expediente",
                //    alignment: 'center',
                //    cellTemplate: function (container, options) {
                //        if (options.data.IdExpediente > 0) {


                //            $('<div/>').dxButton({
                //                type: "success",
                //                width: 40,
                //                height: 36,
                //                icon: 'doc',
                //                hint: 'Ver Expediente',
                //                onClick: function () {
                //                    IdExpedienteQueja = options.data.IdExpediente;
                //                    var _popup = $("#popExpediente").dxPopup("instance");
                //                    _popup.show();
                //                }
                //            }).appendTo(container);

                //        } else {


                //            $('<div/>').dxButton({
                //                hint: "Vincular un Expediente",
                //                stylingMode: "contained",
                //                text: "",
                //                type: "success",
                //                width: 40,
                //                height: 36,
                //                icon: 'pin',
                //                onClick: function () {
                //                    CodQueja = options.data.CodigoQueja;
                //                    var _popup = $("#popupBuscaExp").dxPopup("instance");
                //                    _popup.show();
                //                    $('#BuscarExp').attr('src', $('#SIM').data('url') + 'Utilidades/BuscarExpediente?popup=true');
                //                }
                //            }).appendTo(container);

                //        }
                //    }

                //},
             
            ],
            onSelectionChanged: function (selectedItems) {


                var data = selectedItems.selectedRowsData[0];
                if (data) {
                    CodQueja = data.CodigoQueja;
                    GidListadoTerceros.dxDataGrid("instance").option("dataSource", TercerosQuejasDataSource);
                    GidListadoAutos.dxDataGrid("instance").option("dataSource", AutosDataSource);
                    GidListadoResoluciones.dxDataGrid("instance").option("dataSource", ResolucionesDataSource);
                    GidListadoRespuestas.dxDataGrid("instance").option("dataSource", RespuestasDataSource);
                    GidListadoEstados.dxDataGrid("instance").option("dataSource", EstadosDataSource);
                    GidListadoCitaciones.dxDataGrid("instance").option("dataSource", CitacionesDataSource);
                    GidListadoComentarios.dxDataGrid("instance").option("dataSource", ComentariosDataSource);
                    GidListadoVisitas.dxDataGrid("instance").option("dataSource", VisitasDataSource);
                    GidListadOficios.dxDataGrid("instance").option("dataSource", OficiosDataSource);
                    GidListadoAtenciones.dxDataGrid("instance").option("dataSource", AtencionesDataSource);
                    GidListadoInformes.dxDataGrid("instance").option("dataSource", ListadoInformesDataSource);
                    GidListadoExpedientes.dxDataGrid("instance").option("dataSource", ListadoSolicitudesQuejaDataSource);
                    GidListadoNotificaciones.dxDataGrid("instance").option("dataSource", NotificacionesDataSource);
                    GidListadoTramites.dxDataGrid("instance").option("dataSource", ListadoTramitesDataSource);

                    
                }

            }
        });

    var GidListadoTerceros = $("#GidListadoTerceros").dxDataGrid(
        {
            visible: true,
            dataSource: TercerosQuejasDataSource,
            allowColumnResizing: true,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
            noDataText: "",
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

                { dataField: 'idQuejaTercero', width: '10%', caption: 'Codigo', alignment: 'center' },
                { dataField: 'rSocial', width: '15%', caption: 'Tercero', alignment: 'left' },
                { dataField: 'nombreInstalacion', width: '15%', caption: 'Instalacion', alignment: 'center' },
                { dataField: 'descripcion', width: '20%', caption: 'Direccion descriptiva', alignment: 'left' },
                { dataField: 'nombreTipoTercero', width: '15%', caption: 'Tipo de tercero', alignment: 'left' },
                { dataField: 'observacion', width: '15%', caption: 'Observacion', alignment: 'left' },

                {
                    width: '5%',
                    caption: "Detalle",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        $('<div/>').dxButton({
                            icon: 'edit',


                            hint: 'Ver detalle',
                            onClick: function (e) {
                                var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/ObtenerTerceroQueja";
                                $.getJSON(_Ruta,
                                    {
                                        id: options.data.idQuejaTercero
                                    }).done(function (data) {
                                        if (data !== null) {
                                            IdQuejaTercero = options.data.idQuejaTercero;
                                            popupTerceros.show();
                                            txtDocumentoTercero.option("value", data.documentoN);
                                            slctboxTipoTerceroQueja.option("value", data.idTipoTerceroQueja);
                                            ObservacionTercero.option("value", data.descripcion);
                                            txtDireccionDescriptiva.option("value", data.especial);
                                            txtFechaAsociacion.option("value", data.fechaTerceroQueja);
                                            
                                        }
                                    }).fail(function (jqxhr, textStatus, error) {
                                        DevExpress.ui.dialog.alert('Ocurrió un error');
                                    });
                                

                            }
                        }).appendTo(container);
                    }

                },

                {
                    width: '5%',
                    caption: "Desvincular",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        $('<div/>').dxButton({
                            icon: 'remove',


                            hint: 'Desvincular Tercero',
                            onClick: function (e) {
                                var result = DevExpress.ui.dialog.confirm('Desea desvincular el tercero seleccionado?', 'Confirmación');
                                result.done(function (dialogResult) {
                                    if (dialogResult) {


                                        var idQt = options.data.idQuejaTercero;
                                        var codQ = CodQueja;
                                        var idT = options.data.idTercero;
                                        var idI = options.data.idInstalacion;
                                        var idTtq = options.data.idTipoTerceroQueja;

                                        var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/DesvincularTerceroQueja";
                                        var params = {
                                            idQuejaTercero: idQt, codigoQueja: codQ, idTercero: idT, idInstalacion: idI, idTipoTerceroQueja: idTtq, fechaTerceroQueja: "", descripcion: "", especial: ""
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
                                                    GidListadoTerceros.dxDataGrid("instance").option("dataSource", TercerosQuejasDataSource);
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

    var GidListadoAutos = $("#GidListadoAutos").dxDataGrid(
        {
            dataSource: AutosDataSource,
            allowColumnResizing: true,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
            noDataText: "Sin datos para mostrar",
            showBorders: true,
            wordWrapEnabled: true, // ajustar el texto(doble linea)

            export: {
                enabled: false
            },

            paging: {
                pageSize: 5
            },
            headerFilter: {
                visible: true,
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

                { dataField: 'idAuto', width: '10%', caption: 'Codigo', alignment: 'left' },
                { dataField: 'codAuto', width: '10%', caption: 'Consecutivo', alignment: 'center' },
                { dataField: 'anio', width: '10%', caption: 'Año', alignment: 'left' },
                { dataField: 'fechaElaboracion', width: '10%', caption: 'Fecha Elaboracion', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy' },
                { dataField: 'nombreFuncionario', width: '15%', caption: 'Funcionario', alignment: 'center'},
                { dataField: 'nombreTipoAuto', width: '15%', caption: 'Tipo Auto', alignment: 'center' },
                { dataField: 'descripcion', width: '20%', caption: 'Descripcion', alignment: 'center' },

                {
                    width: '10%',
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


            ],
            onSelectionChanged: function (selectedItems) {


            }
        });

    var GidListadoResoluciones = $("#GidListadoResoluciones").dxDataGrid(
        {
            dataSource: ResolucionesDataSource,
            allowColumnResizing: true,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
            noDataText: "Sin datos para mostrar",
            showBorders: true,
            wordWrapEnabled: true, // ajustar el texto(doble linea)

            export: {
                enabled: false
            },

            paging: {
                pageSize: 5
            },
            headerFilter: {
                visible: true,
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

                { dataField: 'idResolucion', width: '10%', caption: 'Codigo', alignment: 'left' },
                { dataField: 'codResolucion', width: '10%', caption: 'Consecutivo', alignment: 'center' },
                { dataField: 'anio', width: '10%', caption: 'Año', alignment: 'left' },
                { dataField: 'fechaElaboracion', width: '10%', caption: 'Fecha Elaboracion', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy' },
                { dataField: 'nombreFuncionario', width: '20%', caption: 'Funcionario', alignment: 'center' },
                { dataField: 'nombreTipoResolucion', width: '15%', caption: 'Tipo Resolucion', alignment: 'center' },
                { dataField: 'descripcion', width: '15%', caption: 'Descripcion', alignment: 'center' },

                {
                    width: '10%',
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

            ],
            onSelectionChanged: function (selectedItems) {


            }
        });

    var GidListadoRespuestas = $("#GidListadoRespuestas").dxDataGrid(
        {
            dataSource: RespuestasDataSource,
            allowColumnResizing: true,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
            noDataText: "Sin datos para mostrar",
            showBorders: true,
            wordWrapEnabled: true, // ajustar el texto(doble linea)

            export: {
                enabled: false
            },

            paging: {
                pageSize: 5
            },
            headerFilter: {
                visible: true,
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

                { dataField: 'codContestaQueja', width: '20%', caption: 'Codigo', alignment: 'left' },
                { dataField: 'fecha', width: '20%', caption: 'F. Actualizacion', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy' },
                { dataField: 'radicado', width: '20%', caption: 'Radicado', alignment: 'left' },
                { dataField: 'asunto', width: '20%', caption: 'Asunto', alignment: 'center' },
                
                //{
                //    width: '10%',
                //    caption: "Ver Documento",
                //    alignment: 'center',
                //    cellTemplate: function (container, options) {

                //        $('<div/>').dxButton({
                //            type: "success",
                //            width: 40,
                //            height: 36,
                //            icon: 'doc',
                //            hint: 'Ver Documento',
                //            onClick: function () {

                //            }
                //        }).appendTo(container);
                //    }

                //},
                {
                    width: '5%',
                    caption: "Ver Detalles",
                    alignment: 'center',
                    cellTemplate: function (container, options) {

                        $('<div/>').dxButton({
                            type: "success",
                            width: 40,
                            height: 36,
                            icon: 'edit',
                            hint: 'Ver Detalles',
                            onClick: function () {
                                var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/ObtenerRespuestaQueja";
                                $.getJSON(_Ruta,
                                    {
                                        id: options.data.codContestaQueja
                                    }).done(function (data) {
                                        if (data !== null) {
                                            popupNuevaRespuesta.show();
                                            CodRespuestaQueja = data.codContestaQueja;
                                            txtRadicado.option("value", data.radicado);
                                            dateFechaRespuesta.option("value", data.fecha);
                                            asuntoRespuesta.option("value", data.asunto);
                                        }
                                    }).fail(function (jqxhr, textStatus, error) {
                                        DevExpress.ui.dialog.alert('Ocurrió un error');
                                    });


                            }
                        }).appendTo(container);
                    }

                },

                {
                    width: '5%',
                    caption: "Eliminar",
                    alignment: 'center',
                    cellTemplate: function (container, options) {

                        $('<div/>').dxButton({
                            type: "success",
                            width: 40,
                            height: 36,
                            icon: 'close',
                            hint: 'Eliminar',
                            onClick: function (e) {
                                var result = DevExpress.ui.dialog.confirm('Desea desvincular la respuesta seleccionada?', 'Confirmación');
                                result.done(function (dialogResult) {
                                    if (dialogResult) {

                                        var cosCQ = options.data.codContestaQueja;
                                        var CodQ = CodQueja;
                                       

                                        var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/DesvincularRespuestaQueja";
                                        var params = {
                                            codContestaQueja: cosCQ, codigoQueja: CodQ, radicado: "", fecha: "", asunto: "", codigoTramite: "", codigoDocumento: ""
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
                                                    GidListadoRespuestas.dxDataGrid("instance").option("dataSource", RespuestasDataSource);
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

    var GidListadoEstados = $("#GidListadoEstados").dxDataGrid(
        {
            dataSource: EstadosDataSource,
            allowColumnResizing: true,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
            noDataText: "Sin datos para mostrar",
            showBorders: true,
            wordWrapEnabled: true, // ajustar el texto(doble linea)

            export: {
                enabled: false
            },

            paging: {
                pageSize: 5
            },
            headerFilter: {
                visible: true,
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

                { dataField: 'codEstadoQueja', width: '20%', caption: 'Codigo', alignment: 'left' },
                { dataField: 'fechaEstado', width: '20%', caption: 'F. Estado', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy' },
                { dataField: 'nombreTipoEstadoQueja', width: '20%', caption: 'Estado', alignment: 'left' },
                { dataField: 'funcionario', width: '20%', caption: 'Funcionario', alignment: 'center' },

                {
                    width: '10%',
                    caption: "Ver Detalles",
                    alignment: 'center',
                    cellTemplate: function (container, options) {

                        $('<div/>').dxButton({
                            type: "success",
                            width: 40,
                            height: 36,
                            icon: 'edit',
                            hint: 'Ver Detalles',
                            onClick: function () {
                                var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/ObtenerEstadoQueja";
                                $.getJSON(_Ruta,
                                    {
                                        id: options.data.codEstadoQueja
                                    }).done(function (data) {
                                        if (data !== null) {
                                            popupNuevoEstado.show();
                                            CodEstadoQueja = data.codEstadoQueja;
                                            selectBoxTipoEstado.option("value", data.codTipoEstadoQueja);
                                            dateFechaEstado.option("value", data.fechaEstado);
                                            Funcionario.option("value", data.codFuncionario);
                                        }
                                    }).fail(function (jqxhr, textStatus, error) {
                                        DevExpress.ui.dialog.alert('Ocurrió un error');
                                    });


                            }
                        }).appendTo(container);
                    }

                },
             

                {
                    width: '10%',
                    caption: "Eliminar",
                    alignment: 'center',
                    cellTemplate: function (container, options) {

                        $('<div/>').dxButton({
                            type: "success",
                            width: 40,
                            height: 36,
                            icon: 'close',
                            hint: 'Eliminar',
                            onClick: function (e) {
                                var result = DevExpress.ui.dialog.confirm('Desea desvincular el estado seleccionado?', 'Confirmación');
                                result.done(function (dialogResult) {
                                    if (dialogResult) {

                                        var codEq = options.data.codEstadoQueja;
                                        var codQ = CodQueja;


                                        var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/DesvincularEstadoQueja";
                                        var params = {
                                            codEstadoQueja: codEq, fechaEstado: "", funcionario: "", codQueja: codQ, codTipoEstadoQueja: "", codFuncionario: ""
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
                                                    GidListadoEstados.dxDataGrid("instance").option("dataSource", EstadosDataSource);
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

    var GidListadoCitaciones = $("#GidListadoCitaciones").dxDataGrid(
        {
            dataSource: CitacionesDataSource,
            allowColumnResizing: true,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
            noDataText: "Sin datos para mostrar",
            showBorders: true,
            wordWrapEnabled: true, // ajustar el texto(doble linea)

            export: {
                enabled: false
            },

            paging: {
                pageSize: 5
            },
            headerFilter: {
                visible: true,
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

                { dataField: 'codCitacionQueja', width: '15%', caption: 'Codigo', alignment: 'left' },
                { dataField: 'fechaCitacion', width: '15%', caption: 'F. Citacion', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy' },
                { dataField: 'hora', width: '15%', caption: 'Hora', alignment: 'left' },
                { dataField: 'nombreObjetoCitacion', width: '20%', caption: 'Objeto', alignment: 'center' },
                { dataField: 'observacion', width: '20%', caption: 'Observacion', alignment: 'center' },

                {
                    width: '15%',
                    caption: "Ver Detalles",
                    alignment: 'center',
                    cellTemplate: function (container, options) {

                        $('<div/>').dxButton({
                            type: "success",
                            width: 40,
                            height: 36,
                            icon: 'edit',
                            hint: 'Ver Detalles',
                            onClick: function () {
                                var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/ObtenerCitacionQueja";
                                $.getJSON(_Ruta,
                                    {
                                        id: options.data.codCitacionQueja
                                    }).done(function (data) {
                                        if (data !== null) {
                                            popupNuevaCitacion.show();
                                            CodCitacionQueja = data.codCitacionQueja;
                                            dateFechaCitacion.option("value", data.fechaCitacion);
                                            selectBoxObjetoCitacion.option("value", data.codigoObjetoCitacion);
                                            ObservacionCitacion.option("value", data.observacion);
                                        }
                                    }).fail(function (jqxhr, textStatus, error) {
                                        DevExpress.ui.dialog.alert('Ocurrió un error');
                                    });


                            }
                        }).appendTo(container);
                    }

                },


                {
                    width: '15%',
                    caption: "Eliminar",
                    alignment: 'center',
                    cellTemplate: function (container, options) {

                        $('<div/>').dxButton({
                            type: "success",
                            width: 40,
                            height: 36,
                            icon: 'close',
                            hint: 'Eliminar',
                            onClick: function (e) {
                                var result = DevExpress.ui.dialog.confirm('Desea desvincular la citacion seleccionada?', 'Confirmación');
                                result.done(function (dialogResult) {
                                    if (dialogResult) {

                                        var codCQ = options.data.codCitacionQueja;
                                        var codQ = CodQueja;


                                        var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/DesvincularCitacionQueja";
                                        var params = {
                                            codCitacionQueja: codCQ, codigoQueja: codQ, codigoObjetoCitacion: "", fechaCitacion: "", observacion: "", hora: ""
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
                                                    GidListadoCitaciones.dxDataGrid("instance").option("dataSource", CitacionesDataSource);
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

    var GidListadoComentarios = $("#GidListadoComentarios").dxDataGrid(
        {
            dataSource: ComentariosDataSource,
            allowColumnResizing: true,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
            noDataText: "Sin datos para mostrar",
            showBorders: true,
            wordWrapEnabled: true, // ajustar el texto(doble linea)

            export: {
                enabled: false
            },

            paging: {
                pageSize: 5
            },
            headerFilter: {
                visible: true,
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

                { dataField: 'idQuejaComentario', width: '20%', caption: 'Codigo', alignment: 'left' },
                { dataField: 'fechaComentario', width: '20%', caption: 'F. Comentario', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy' },
                { dataField: 'nombres', width: '20%', caption: 'Funcionario', alignment: 'left' },
                { dataField: 'comentario', width: '20%', caption: 'Comentarios', alignment: 'center' },
                

                {
                    width: '10%',
                    caption: "Ver Detalles",
                    alignment: 'center',
                    cellTemplate: function (container, options) {

                        $('<div/>').dxButton({
                            type: "success",
                            width: 40,
                            height: 36,
                            icon: 'edit',
                            hint: 'Ver Detalles',
                            onClick: function () {
                                var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/ObtenerQuejaComentario";
                                $.getJSON(_Ruta,
                                    {
                                        id: options.data.idQuejaComentario
                                    }).done(function (data) {
                                        if (data !== null) {
                                            FunActualizarComentario = data.idFuncionario;
                                            popupNuevoComentario.show();
                                            CodComentarioQueja = data.idQuejaComentario;
                                            ComentarioQueja.option("value", data.comentario);
                                            userLogComentario.option("value", options.data.nombres);
                                           
                                        }
                                    }).fail(function (jqxhr, textStatus, error) {
                                        DevExpress.ui.dialog.alert('Ocurrió un error');
                                    });


                            }
                        }).appendTo(container);
                    }

                },


                {
                    width: '10%',
                    caption: "Eliminar",
                    alignment: 'center',
                    cellTemplate: function (container, options) {

                        $('<div/>').dxButton({
                            type: "success",
                            width: 40,
                            height: 36,
                            icon: 'close',
                            hint: 'Eliminar',
                            onClick: function (e) {
                                var result = DevExpress.ui.dialog.confirm('Desea desvincular el comentario seleccionado?', 'Confirmación');
                                result.done(function (dialogResult) {
                                    if (dialogResult) {

                                        var codCQ = options.data.idQuejaComentario;
                                        var codQ = CodQueja;


                                        var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/DesvincularQuejaComentario";
                                        var params = {
                                            idQuejaComentario: codCQ, comentario: "", idFuncionario: "", idTercero: "", fechaComentario: "", idQueja: codQ
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
                                                    GidListadoComentarios.dxDataGrid("instance").option("dataSource", ComentariosDataSource);
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

    var GidListadoVisitas = $("#GidListadoVisitas").dxDataGrid(
        {
            dataSource: VisitasDataSource,
            allowColumnResizing: true,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
            noDataText: "Sin datos para mostrar",
            showBorders: true,
            wordWrapEnabled: true, // ajustar el texto(doble linea)

            export: {
                enabled: false
            },

            paging: {
                pageSize: 5
            },
            headerFilter: {
                visible: true,
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

                { dataField: 'codVisitaQueja', width: '10%', caption: 'Codigo', alignment: 'left' },
                { dataField: 'fechaVisita', width: '15%', caption: 'F. Actuacion', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy' },
                { dataField: 'nombreObjeto', width: '15%', caption: 'Objeto Visita', alignment: 'left' },
                { dataField: 'tecnico', width: '15%', caption: 'Tecnico', alignment: 'center' },
                { dataField: 'observacion', width: '15%', caption: 'Observacion', alignment: 'center' },

                {
                    width: '15%',
                    caption: "Ver Detalles",
                    alignment: 'center',
                    cellTemplate: function (container, options) {

                        $('<div/>').dxButton({
                            type: "success",
                            width: 40,
                            height: 36,
                            icon: 'edit',
                            hint: 'Ver Detalles',
                            onClick: function () {
                                var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/ObtenerVisitaQueja";
                                $.getJSON(_Ruta,
                                    {
                                        id: options.data.codVisitaQueja
                                    }).done(function (data) {
                                        if (data !== null) {
                                            popupNuevaVisita.show();
                                            CodVisitaQueja = data.codVisitaQueja;
                                            selectBoxObjetoVisita.option("value", data.codigoObjetoVisita);
                                            dateFechaVisita.option("value", data.fechaVisita);
                                            Tecnico.option("value", data.codigoTecnico);
                                        }
                                    }).fail(function (jqxhr, textStatus, error) {
                                        DevExpress.ui.dialog.alert('Ocurrió un error');
                                    });


                            }
                        }).appendTo(container);
                    }

                },


                {
                    width: '15%',
                    caption: "Eliminar",
                    alignment: 'center',
                    cellTemplate: function (container, options) {

                        $('<div/>').dxButton({
                            type: "success",
                            width: 40,
                            height: 36,
                            icon: 'close',
                            hint: 'Eliminar',
                            onClick: function (e) {
                                var result = DevExpress.ui.dialog.confirm('Desea desvincular el estado seleccionado?', 'Confirmación');
                                result.done(function (dialogResult) {
                                    if (dialogResult) {

                                        var codVq = options.data.codVisitaQueja;
                                        var codQ = CodQueja;


                                        var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/DesvincularVisitaQueja";
                                        var params = {
                                            codVisitaQueja: codVq, codigoQueja: codQ, codigoObjetoVisita: "", fechaVisita: "", tecnico: "", observacion: "", codigoTecnico: ""
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
                                                    GidListadoVisitas.dxDataGrid("instance").option("dataSource", VisitasDataSource);
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

    var GidListadOficios = $("#GidListadOficios").dxDataGrid(
        {
            dataSource: OficiosDataSource,
            allowColumnResizing: true,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
            noDataText: "Sin datos para mostrar",
            showBorders: true,
            wordWrapEnabled: true, // ajustar el texto(doble linea)

            export: {
                enabled: false
            },

            paging: {
                pageSize: 5
            },
            headerFilter: {
                visible: true,
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

                { dataField: 'idQuejaOficio', width: '25%', caption: 'Codigo', alignment: 'left' },
                { dataField: 'codTramite', width: '25%', caption: 'Tramite', alignment: 'center' },
                { dataField: 'fechaAsociacion', width: '25%', caption: 'F. Asociacion', alignment: 'left', dataType: 'date', format: 'dd/MM/yyyy'},
                
                {
                    width: '25%',
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
              

            ],
            onSelectionChanged: function (selectedItems) {


            }
        });

    var GidListadoAtenciones = $("#GidListadoAtenciones").dxDataGrid(
        {
            dataSource: AtencionesDataSource,
            allowColumnResizing: true,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
            noDataText: "Sin datos para mostrar",
            showBorders: true,
            wordWrapEnabled: true, // ajustar el texto(doble linea)

            export: {
                enabled: false
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
                { dataField: 'fechaCompromiso', width: '10%', caption: 'F. Compromiso', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy' },
                {
                    dataField: 'estado', width: '5%', caption: 'Estado', alignment: 'center', customizeText: function (cellInfo) {
                        return cellInfo.value == "1" ? 'Terminada' : 'No terminada';
                    }
                },
                

            ],
            onSelectionChanged: function (selectedItems) {



            }
        });

    var GidListadoInformes = $("#GidListadoInformes").dxDataGrid(
        {
            visible: true,
            dataSource: ListadoInformesDataSource,
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
                { dataField: 'codInformeQueja', width: '10%', caption: 'Codigo', alignment: 'center' },
                { dataField: 'consecutivo', width: '10%', caption: 'Consecutivo', alignment: 'center' },
                { dataField: 'anio', width: '10%', caption: 'Año', alignment: 'center' },
                { dataField: 'nroInforme', width: '10%', caption: 'Nro informe', alignment: 'center' },
                { dataField: 'fechaElaboracion', width: '10%', caption: 'F. Elaboracion', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy' },
                { dataField: 'tecnico', width: '10%', caption: 'Tecnico', alignment: 'center' },
                { dataField: 'observacion', width: '20%', caption: 'Observacion', alignment: 'center' },
               
                {
                    width: '10%',
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
                    width: '10%',
                    caption: "Desvincular",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        $('<div/>').dxButton({
                            icon: 'remove',


                            hint: 'Desvincular Informe',
                            onClick: function (e) {
                                var result = DevExpress.ui.dialog.confirm('Desea desvincular el informe seleccionado?', 'Confirmación');
                                result.done(function (dialogResult) {
                                    if (dialogResult) {

                                        var codIQ = options.data.codInformeQueja ;
                                        
                                        

                                        var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/DesvincularInformeQueja";
                                        var params = {
                                            codInformeQueja: codIQ ,codigoQueja: "", radicado: "", fecha: "", tecnico: "", observacion: "", fechaControl: "", fechaSeguimiento: "", responsable: "", codTramite: 0, codDocumento: 0, estado: "", componente: "", idInforme: ""
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
                                                    GidListadoInformes.dxDataGrid("instance").option("dataSource", ListadoInformesDataSource);
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

    var GidGuardarInforme = $("#GidGuardarInforme").dxDataGrid(
        {
            visible: true,
            dataSource: BuscarInformesDataSource,
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

                { dataField: 'IdInforme', width: '10%', caption: 'IdInforme', alignment: 'center' },
                { dataField: 'consecutivo', width: '10%', caption: 'Consecutivo', alignment: 'center' },
                { dataField: 'anio', width: '10%', caption: 'Año', alignment: 'center' },
                { dataField: 'nroInforme', width: '10%', caption: 'Nro informe', alignment: 'center' },
                { dataField: 'fechaElaboracion', width: '10%', caption: 'F. Elaboracion', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy' },
                { dataField: 'descripcion', width: '15%', caption: 'Descripcion', alignment: 'center' },
                { dataField: 'fechaControl', width: '10%', caption: 'F. Control', alignment: 'center' },
                { dataField: 'fechaSeguimiento', width: '10%', caption: 'F. Seguimiento', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy' },
                { dataField: 'responsable', width: '10%', caption: 'Responsable', alignment: 'center' },

                {
                    width: '5%',
                    caption: "Vincular",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        $('<div/>').dxButton({
                            icon: 'save',
                            hint: 'Vincular Informe',
                            onClick: function (e) {

                                var codQ = CodQueja;
                                var idInforme = options.data.IdInforme;
                                var codT = options.data.codTramite;
                                var codD = options.data.codDocumento;


                                params = {
                                    codInformeQueja: "", codigoQueja: codQ, radicado: "", fecha: "", tecnico: "", observacion: "", fechaControl: "", fechaSeguimiento: "", responsable: "", codTramite: codT, codDocumento: codD, estado: "", componente: "", idInforme: idInforme
                                };


                                var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/VincularInformeQueja";
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
                                            GidListadoInformes.dxDataGrid("instance").option("dataSource", ListadoInformesDataSource);
                                            popupNuevoInforme.hide();
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

    var GidListadoExpedientes = $("#GidListadoExpedientes").dxDataGrid(
        {
            visible: true,
            dataSource: ListadoSolicitudesQuejaDataSource,
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
                { dataField: 'codigoSolicitud', width: '15%', caption: 'Codigo', alignment: 'center' },
                { dataField: 'CM', width: '15%', caption: 'CM', alignment: 'center' },
                { dataField: 'nombreTipoSolicitud', width: '15%', caption: 'Tipo de Solicitud', alignment: 'center' },
                { dataField: 'conexo', width: '15%', caption: 'Conexo', alignment: 'center' },
                { dataField: 'descripcion', width: '15%', caption: 'Descripción', alignment: 'center' },

                {
                    width: '15%',
                    caption: "Ver Expediente",
                    alignment: 'center',
                    visible: false,
                    cellTemplate: function (container, options) {
                        $('<div/>').dxButton({
                            icon: 'doc',


                            hint: 'Ver Expediente',
                            onClick: function () {
                                IdExpDoc = options.data.idExpDoc;
                                var _popup = $("#popExpediente").dxPopup("instance");
                                _popup.show();
                                $("#PanelDer").addClass("hidden");
                                var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/NombreExpediente";
                                $.getJSON(_Ruta, { IdExp: IdExpDoc })
                                    .done(function (data) {
                                        if (data != "") {
                                            NomExpediente = data;
                                            $("#lblExpediente").text(NomExpediente);
                                            $("#btnIndices").dxButton("instance").option("visible", true);
                                            $("#dxTreeView").dxTreeView({
                                                dataSource: new DevExpress.data.DataSource({
                                                    store: new DevExpress.data.CustomStore({
                                                        key: "ID",
                                                        loadMode: "raw",
                                                        load: function () {
                                                            return $.getJSON($("#SIM").data("url") + "GestionDocumental/api/ExpedientesApi/ArbolExpediente", { IdExp: IdExpDoc });
                                                        }
                                                    })
                                                }),
                                                dataStructure: "plain",
                                                keyExpr: "ID",
                                                displayExpr: "NOMBRE",
                                                parentIdExpr: "PADRE",
                                                width: '100%',
                                                onItemClick: function (e) {
                                                    var item = e.itemData;
                                                    if (item.DOCS) {
                                                        var valores = item.ID.split(".");
                                                        IdTomo = valores[2];
                                                        IdIUnidadDoc = valores[3];
                                                        $("#ListaDocs").dxList({
                                                            dataSource: new DevExpress.data.DataSource({
                                                                store: new DevExpress.data.CustomStore({
                                                                    key: "Documento",
                                                                    loadMode: "raw",
                                                                    load: function () {
                                                                        return $.getJSON($("#SIM").data("url") + "GestionDocumental/api/ExpedientesApi/ObtieneDocs", { IdUniDoc: IdIUnidadDoc, IdTomo: IdTomo });
                                                                    }
                                                                })
                                                            }),
                                                            height: "100%",
                                                            width: "100%",
                                                            allowItemDeleting: false,
                                                            itemDeleteMode: "toggle",
                                                            showSelectionControls: true,
                                                            scrollingEnabled: true,
                                                            itemTemplate: function (data, index) {
                                                                var divP = $("<div>");
                                                                var div1 = $("<div>").addClass("image-container").appendTo(divP);
                                                                $("<img>").attr("src", $('#SIM').data('url') + "Content/imagenes/doc.png").appendTo(div1);
                                                                var div2 = $("<div>").addClass("info").appendTo(divP);
                                                                $("<div>").text("Documento: " + data.Documento).appendTo(div2);
                                                                $("<div>").text(data.Datos.substring(0, 30) + ' ...').appendTo(div2);
                                                                $("<div>").text("Fecha digitliza: " + data.Fecha).appendTo(div2);
                                                                return divP;
                                                            },
                                                            onContentReady: function (e) {
                                                                var listitems = e.element.find('.dx-item');
                                                                var tooltip = $('#tooltip').dxTooltip({
                                                                    width: 500,
                                                                    contentTemplate: function (contentElement) {
                                                                        contentElement.append(
                                                                            $("<p style='indices'/>").text(contentElement.text)
                                                                        )
                                                                    }
                                                                }).dxTooltip('instance');
                                                                listitems.on('dxhoverstart', function (args) {
                                                                    tooltip.content().text($(this).data().dxListItemData.Datos);
                                                                    tooltip.show(args.target);
                                                                });

                                                                listitems.on('dxhoverend', function () {
                                                                    tooltip.hide();
                                                                });
                                                            },
                                                            onItemClick: function (e) {
                                                                var item = e.itemData;
                                                                window.open($('#SIM').data('url') + "Utilidades/LeeDoc?IdDocumento=" + item.Documento, "Documento " + item.Documento, "width= 900,height=800,scrollbars = yes, location = no, toolbar = no, menubar = no, status = no");
                                                            }
                                                        });
                                                        $("#PanelDer").removeClass("hidden");
                                                    } else if (item.TOMO) {
                                                        var valores = item.ID.split(".");
                                                        IdTomo = valores[2];
                                                        $("#ListaDocs").dxList({
                                                            dataSource: new DevExpress.data.DataSource({
                                                                store: new DevExpress.data.CustomStore({
                                                                    key: "Documento",
                                                                    loadMode: "raw",
                                                                    load: function () {
                                                                        return $.getJSON($("#SIM").data("url") + "GestionDocumental/api/ExpedientesApi/ObtieneDocs", { IdTomo: IdTomo });
                                                                    }
                                                                })
                                                            }),
                                                            height: "100%",
                                                            width: "100%",
                                                            allowItemDeleting: false,
                                                            itemDeleteMode: "toggle",
                                                            showSelectionControls: true,
                                                            scrollingEnabled: true,
                                                            itemTemplate: function (data, index) {
                                                                var divP = $("<div>");
                                                                var div1 = $("<div>").addClass("image-container").appendTo(divP);
                                                                $("<img>").attr("src", $('#SIM').data('url') + "Content/imagenes/doc.png").appendTo(div1);
                                                                var div2 = $("<div>").addClass("info").appendTo(divP);

                                                                $("<div>").text("Documento: " + data.Documento).appendTo(div2);
                                                                $("<div>").html("<p style='indices'>" + data.Datos + "</p>").addClass("divInd").appendTo(div2);
                                                                $("<div>").text("Fecha digitliza: " + data.Fecha).appendTo(div2);
                                                                return divP;
                                                            },
                                                            onContentReady: function (e) {
                                                                var listitems = e.element.find('.dx-item');
                                                                var tooltip = $('#tooltip').dxTooltip({
                                                                    width: 500,
                                                                    contentTemplate: function (contentElement) {
                                                                        contentElement.append(
                                                                            $("<p style='indices'/>").text(contentElement.text)
                                                                        )
                                                                    }
                                                                }).dxTooltip('instance');
                                                                listitems.on('dxhoverstart', function (args) {
                                                                    tooltip.content().text($(this).data().dxListItemData.Datos);
                                                                    tooltip.show(args.target);
                                                                });

                                                                listitems.on('dxhoverend', function () {
                                                                    tooltip.hide();
                                                                });
                                                            },
                                                            onItemClick: function (e) {
                                                                var item = e.itemData;
                                                                window.open($('#SIM').data('url') + "Utilidades/LeeDoc?IdDocumento=" + item.Documento, "Documento " + item.Documento, "width= 900,height=800,scrollbars = yes, location = no, toolbar = no, menubar = no, status = no");
                                                            }

                                                        });
                                                        $("#PanelDer").removeClass("hidden");
                                                    } else {
                                                        $("#PanelDer").addClass("hidden");
                                                    }
                                                }
                                            });
                                        }
                                    }).fail(function (jqxhr, textStatus, error) {
                                        DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Asociar documento');
                                    }
                                    );


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


                            hint: 'Desvincular Asunto',
                            onClick: function (e) {
                                var result = DevExpress.ui.dialog.confirm('Desea desvincular el expediente seleccionado?', 'Confirmación');
                                result.done(function (dialogResult) {
                                    if (dialogResult) {

                                        var codQuejaProyecto = options.data.codQuejaProyecto;
                                        var CodProyecto = options.data.codigoProyecto;
                                        var CodQueja = options.data.codigoQueja;
                                       

                                        var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/DesvincularSolicitudTAsync";
                                        var params = {
                                            codQuejaProyecto: codQuejaProyecto, codProyecto: CodProyecto, codQueja: CodQueja, origenCm: ""
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
                                                    GidListadoExpedientes.dxDataGrid("instance").option("dataSource", ListadoSolicitudesQuejaDataSource);
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

    var GidGuardarExpedientes = $("#GidGuardarExpedientes").dxDataGrid(
        {
            visible: true,
            dataSource: BuscarSolicitudQuejaDataSource,
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

                { dataField: 'CM', width: '20%', caption: 'CM', alignment: 'center' },
                { dataField: 'nombreTipoSolicitud', width: '20%', caption: 'Tipo de Solicitud', alignment: 'left', allowSearch: false },
                { dataField: 'conexo', width: '20%', caption: 'Conexo', alignment: 'center', allowSearch: false },
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

                              
                                var codProyecto = options.data.codigoProyecto;
                                var codQueja = CodQueja;


                                params = {
                                    codProyecto: codProyecto, codQueja: codQueja, origenCm: ""
                                };


                                var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/VincularSolicitudTAsync";
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
                                            GidListadoExpedientes.dxDataGrid("instance").option("dataSource", ListadoSolicitudesQuejaDataSource);
                                            popupNuevoExpediente.hide();
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

    var GidListadoNotificaciones = $("#GidListadoNotificaciones").dxDataGrid(
        {
            dataSource: NotificacionesDataSource,
            allowColumnResizing: true,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
            noDataText: "Sin datos para mostrar",
            showBorders: true,
            wordWrapEnabled: true, // ajustar el texto(doble linea)

            export: {
                enabled: false
            },

            paging: {
                pageSize: 5
            },
            headerFilter: {
                visible: true,
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

                { dataField: 'idNotificacion', width: '10%', caption: 'Codigo', alignment: 'left' },
                { dataField: 'asociado', width: '10%', caption: 'Asociado a', alignment: 'center' },
                { dataField: 'fechaNotificacion', width: '10%', caption: 'F. Notificacion', alignment: 'left', dataType: 'date', format: 'dd/MM/yyyy' },
                { dataField: 'funcionarioElabora', width: '10%', caption: 'Funcionario Elabora', alignment: 'left' },
                { dataField: 'personaN', width: '10%', caption: 'Persona Natural', alignment: 'left' },
                { dataField: 'personaJ', width: '10%', caption: 'Persona Juridica', alignment: 'left' },
                { dataField: 'tercero', width: '10%', caption: 'Tercero', alignment: 'left' },
                {
                    dataField: 'idForma', width: '10%', caption: 'Forma', alignment: 'left', customizeText: function (cellInfo) {
                        return cellInfo.value == "1" ? 'Personal' : 'Edicto';
                    }
                },

                { dataField: 'fechaFijacion', width: '10%', caption: 'F. Fijacion', alignment: 'left', dataType: 'date', format: 'dd/MM/yyyy' },
                { dataField: 'fechaVencimiento', width: '10%', caption: 'F. Vencimiento', alignment: 'left', dataType: 'date', format: 'dd/MM/yyyy' },


 

            ],
            onSelectionChanged: function (selectedItems) {


            }
        });

    var GidListadoTramites = $("#GidListadoTramites").dxDataGrid(
        {
            visible: true,
            dataSource: ListadoTramitesDataSource ,
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
              /*  { dataField: 'codigoTramiteExpediente', width: '15%', caption: 'Codigo', alignment: 'center' },*/
                { dataField: 'CodTramite', width: '20%', caption: 'Tramite', alignment: 'center' },
                { dataField: 'fechaIni', width: '20%', caption: 'F. Inicio Tramite', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy' },
                { dataField: 'tipoTramite', width: '20%', caption: 'Tipo Tramite', alignment: 'center' },
                

                {
                    width: '20%',
                    caption: "Ver Tramite",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        $('<div/>').dxButton({
                            icon: 'doc',
                            hint: 'Ver Tramite',
                            onClick: function () {
                                var _popup = $("#popTramite").dxPopup("instance");

                                _popup.show();
                                $("#Tramite").attr("src", $('#SIM').data('url') + "Utilidades/DetalleTramite?popup=true&CodTramite=" + options.data.CodTramite);

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


                            hint: 'Desvincular Tramite',
                            onClick: function (e) {
                                var result = DevExpress.ui.dialog.confirm('Desea desvincular el tramite seleccionado?', 'Confirmación');
                                result.done(function (dialogResult) {
                                    if (dialogResult) {

                                        var codTe = options.data.codigoTramiteExpediente;



                                        var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/DesvincularTramiteQueja";
                                        var params = {
                                            idTramiteExpediente: codTe, codTramite: "", codQueja: ""
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
                                                    GidListadoTramites.dxDataGrid("instance").option("dataSource", ListadoTramitesDataSource);
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

    var GidGuardarTramite = $("#GidGuardarTramite").dxDataGrid(
        {
            visible: true,
            dataSource: BuscartramitesDataSource,
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

                { dataField: 'CodTramite', width: '15%', caption: 'Tramite', alignment: 'center' },
                { dataField: 'tipoTramite', width: '15%', caption: 'Tipo tramite', alignment: 'center' },
                { dataField: 'fechaIni', width: '15%', caption: 'F. Inicio', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy' },
                { dataField: 'comentarios', width: '20%', caption: 'Comentarios', alignment: 'center' },
                {
                    dataField: 'estado', width: '15%', caption: 'Estado', alignment: 'center', customizeText: function (cellInfo) {
                        return cellInfo.value == "0" ? 'En Ejecución' : 'Terminado';
                    }
                },
                

                {
                    width: '10%',
                    caption: "Vincular",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        $('<div/>').dxButton({
                            icon: 'save',
                            hint: 'Vincular tramite',
                            onClick: function (e) {

                                var codQ = CodQueja;
                                var codT = options.data.CodTramite;


                                params = {
                                    codTramite: codT, codQueja: codQ
                                };


                                var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/VincularTramiteQueja";
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
                                            GidListadoTramites.dxDataGrid("instance").option("dataSource", ListadoTramitesDataSource);
                                            popupNuevoTramite.hide();
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
                {
                    width: '10%',
                    caption: "Ver Tramite",
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        $('<div/>').dxButton({
                            icon: 'doc',
                            hint: 'Ver Tramite',
                            onClick: function () {
                                var _popup = $("#popTramite").dxPopup("instance");

                                _popup.show();
                                $("#Tramite").attr("src", $('#SIM').data('url') + "Utilidades/DetalleTramite?popup=true&CodTramite=" + options.data.CodTramite);

                            }
                        }).appendTo(container);
                    }

                },

            ],
            onSelectionChanged: function (selectedItems) {


            }
        });

    //TextArea

    var asuntoQueja = $('#txtareaAsunto').dxTextArea({
        width: 500,
        value: "",
        height: 70,
    }).dxValidator({
        validationGroup: "QuejaGroup",
        validationRules: [{
            type: "required",
            message: "Ingrese el asunto de la queja!"
        }]
    }).dxTextArea('instance');

    var direccion = $('#txtareaDireccion').dxTextArea({
        width: 500,
        value: "",
        height: 70,
    }).dxTextArea('instance');

    var comentariosQueja = $('#txtareaComentarios').dxTextArea({
        width: 500,
        value: "",
        height: 70,
    }).dxTextArea('instance');

    var ObservacionTercero = $('#txtareaObservacionTercero').dxTextArea({
        width: 500,
        value: "",
        height: 102,
    }).dxValidator({
        validationGroup: "",
        //validationRules: [{
        //    type: "required",
        //    message: "Debe ingresar el detalle de la atencion!"
        //},
        //{
        //    type: 'stringLength',
        //    min: 2,
        //    message: 'El nombre debe tener al menos 2 caracteres',
        //},
        //{
        //    type: 'stringLength',
        //    max: 4000,
        //    message: 'El nombre debe tener al maximo 4000 caracteres',
        //}

        //]
    }).dxTextArea('instance');

    var asuntoRespuesta = $('#txtareaAsuntoRespuesta').dxTextArea({
        width: 400,
        value: "",
        height: 70,
    }).dxTextArea('instance');

    var ObservacionCitacion = $('#txtareaObservacionCitacion').dxTextArea({
        width: 400,
        value: "",
        height: 70,
    }).dxTextArea('instance');

    var ComentarioQueja = $('#txtareaComentarioQueja').dxTextArea({
        width: 400,
        value: "",
        height: 70,
    }).dxValidator({
        validationGroup: "ComentarioGroup",
        validationRules: [{
            type: "required",
            message: "Ingrese un comentario!"
        }]
    }).dxTextArea('instance');

    var ObservacionVisita = $('#txtareaObservacionVisita').dxTextArea({
        width: 400,
        value: "",
        height: 70,
    }).dxTextArea('instance');

    //data grid como texto

    
    //menu
    var tabsInstance = $("#tabs").dxTabs({
        dataSource: tabs,

        onItemClick(e) {

            if (e.itemData.id == 0) {
                if (CodQueja >= 1) {
                    $("#Terceros").show();
                }
            }  
            else
                $("#Terceros").hide();

            if (e.itemData.id == 3) {
                if (CodQueja >= 1) {
                    $("#Autos").show();
                }
            }
            else
                $("#Autos").hide();

            if (e.itemData.id == 4) {
                if (CodQueja >= 1) {
                    $("#Resoluciones").show();
                }
            }
            else
                $("#Resoluciones").hide();

            if (e.itemData.id == 7) {
                if (CodQueja >= 1) {
                    $("#Estados").show();
                }
            }
            else
                $("#Estados").hide();

            if (e.itemData.id == 6) {
                if (CodQueja >= 1) {
                    $("#Respuestas").show();
                }
            }
            else
                $("#Respuestas").hide();

            if (e.itemData.id == 2) {
                 if (CodQueja >= 1) {
                $("#Citaciones").show();
                }
            }
            else
                $("#Citaciones").hide();

            if (e.itemData.id == 12) {
                 if (CodQueja >= 1) {
                $("#Comentarios").show();
                }
            }
            else
                $("#Comentarios").hide();

            if (e.itemData.id == 13) {
                 if (CodQueja >= 1) {
                $("#Visitas").show();
                }
            }
            else
                $("#Visitas").hide();

            if (e.itemData.id == 11) {
                 if (CodQueja >= 1) {
                $("#Oficios").show();
                }
            }
            else
                $("#Oficios").hide();

            if (e.itemData.id == 10) {
                 if (CodQueja >= 1) {
                $("#Atenciones").show();
                }
            }
            else
                $("#Atenciones").hide();

            if (e.itemData.id == 1) {
                 if (CodQueja >= 1) {
                $("#Informes").show();
                }
            }
            else
                $("#Informes").hide();

            if (e.itemData.id == 8) {
                 if (CodQueja >= 1) {
                $("#Expedientes").show();
                }
            }
            else
                $("#Expedientes").hide();

            if (e.itemData.id == 5) {
                 if (CodQueja >= 1) {
                $("#Notificaciones").show();
                }
            }
            else
                $("#Notificaciones").hide();

            if (e.itemData.id == 9) {
                 if (CodQueja >= 1) {
                $("#Tramites").show();
                }
            }
            else
                $("#Tramites").hide();

        },
    }).dxTabs('instance');
   
    //selectbox

    var recurso = $("#selectBoxRecurso").dxSelectBox({
        width: 500,
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "codigoRecurso",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "QuejasAmbientales/api/QuejasApi/Recursosasync");
                }
            })
        }),
        displayExpr: "nombre",
        valueExpr: "codigoRecurso",
        searchEnabled: true,

        onValueChanged: function (data) {
            CodRecurso = data.value
            cargarAfectaciones()
            Afectacion.option("dataSource", claseAfectaciones)
            Afectacion.reset();
        }
    }).dxValidator({
        validationGroup: "QuejaGroup",
        validationRules: [{
            type: "required",
            message: "Seleccione el Recurso!"
        }]
    }).dxSelectBox("instance");

    var claseAfectaciones = [];

    function cargarAfectaciones() {
        var res = $.getJSON($("#SIM").data("url") + "QuejasAmbientales/api/QuejasApi/Afectacionsasync?id=" + CodRecurso);
        res.then(data => {

            claseAfectaciones = data
            Afectacion.option("dataSource", claseAfectaciones)
        })
    }

    var Afectacion = $("#selectBoxAfectacion").dxSelectBox({
        width: 500,
        
        displayExpr: "nombre",
        valueExpr: "codigoAfectacion",
        searchEnabled: true,

        onValueChanged: function (data) {
            CodAfectacion = data.value
        }
    }).dxValidator({
        validationGroup: "QuejaGroup",
        validationRules: [{
            type: "required",
            message: "Seleccione la Afectacion!"
        }]
    }).dxSelectBox("instance");

    var municipio = $("#selectBoxMunicipio").dxSelectBox({
        width: 500,
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "codigoMunicipio",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "QuejasAmbientales/api/QuejasApi/Municipiosasync");
                }
            })
        }),
        displayExpr: "nombre",
        valueExpr: "codigoMunicipio",
        searchEnabled: true,

        onValueChanged: function (data) {
            CodMunicipio = data.value
        }
    }).dxValidator({
        validationGroup: "QuejaGroup",
        validationRules: [{
            type: "required",
            message: "Seleccione el Municipio!"
        }]
    }).dxSelectBox("instance");

    //var responsableQueja = $("#selectBoxResponsable").dxSelectBox({
    //    width: 500,
    //    dataSource: new DevExpress.data.DataSource({
    //        store: new DevExpress.data.CustomStore({
    //            key: "codFuncionario",
    //            loadMode: "raw",
    //            load: function () {
    //                return $.getJSON($("#SIM").data("url") + "QuejasAmbientales/api/QuejasApi/Responsablesasync");
    //            }
    //        })
    //    }),
    //    displayExpr: "nombres",
    //    valueExpr: "codFuncionario",
    //    searchEnabled: true,

    //    onValueChanged: function (data) {
    //        CodFun = data.value
    //    }
    //}).dxValidator({
    //    validationGroup: "QuejaGroup",
    //    validationRules: [{
    //        type: "required",
    //        message: "Seleccione el Responsable!"
    //    }]
    //}).dxSelectBox("instance");

    var formaQueja = $("#selectBoxFormaQueja").dxSelectBox({
        width: 500,
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "codigoFormaQueja",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "QuejasAmbientales/api/QuejasApi/FormasQuejassasync");
                }
            })
        }),
        displayExpr: "nombre",
        valueExpr: "codigoFormaQueja",
        searchEnabled: true,

        onValueChanged: function (data) {
            CodFormaQueja = data.value
        }
    }).dxValidator({
        validationGroup: "QuejaGroup",
        validationRules: [{
            type: "required",
            message: "Seleccione la forma de la queja!"
        }]
    }).dxSelectBox("instance");

    var slctboxTipoTerceroQueja = $("#selectBoxTipoTerceroQueja").dxSelectBox({
        width: 500,
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "idTipoTerceroQueja",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "QuejasAmbientales/api/QuejasApi/TipoTerceroQuejaasync");
                }
            })
        }),
        displayExpr: "nombre",
        valueExpr: "idTipoTerceroQueja",
        searchEnabled: true,

        onValueChanged: function (data) {
            IdTipoTerceroQueja = data.value
        }
    }).dxValidator({
        validationGroup: "TerceroGroup",
        validationRules: [{
            type: "required",
            message: "Seleccione el tipo de tercero!"
        }]
    }).dxSelectBox("instance");

    var slctboxTipoTercero = $("#selectBoxTipoTercero").dxSelectBox({
        dataSource: TiposTercero,
        searchEnabled: false,
        value:0,
        width: 250,
        displayExpr: "text",
        valueExpr: "id",
        onValueChanged: function (e) {

            if (e.value == 0) {
                $("#Natural").show();
                $("#Juridico").hide();

            } else if (e.value == 1) {
                $("#Natural").hide();
                $("#Juridico").show();
            }
            
        }
    });

    var selectBoxTipoEstado = $("#selectBoxTipoEstado").dxSelectBox({
        width: 350,
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "codTipoEstadoQueja",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "QuejasAmbientales/api/QuejasApi/TipoEstadoQueja");
                }
            })
        }),
        displayExpr: "nombre",
        valueExpr: "codTipoEstadoQueja",
        searchEnabled: true,

        onValueChanged: function (data) {
            IdTipoEstadoQueja = data.value
        }
    }).dxValidator({
        validationGroup: "EstadoGroup",
        validationRules: [{
            type: "required",
            message: "Seleccione el estado!"
        }]
    }).dxSelectBox("instance");

    ///Inicio
    var funcionarios = []
    
    function getFuncionario(value) {
        for (var i = 0; i < funcionarios.length; i++) {
            let element = funcionarios[i]

            if (element.codFuncionario == value)
                return element
        }
    }

    function cargarFuncionarios() {
        var res = $.getJSON($("#SIM").data("url") + "QuejasAmbientales/api/QuejasApi/GetFuncionarioAsync");
        res.then(data => {

            funcionarios = data
            Funcionario.option("dataSource", funcionarios)
        })
    }

    cargarFuncionarios()

    var Funcionario = $('#treeBoxFuncionario').dxSelectBox({
        width: 350,


        displayExpr: "nombres",
        valueExpr: "codFuncionario",
        searchEnabled: true,

        onValueChanged: function (data) {

            CodEstadoFuncionarioQueja = data.value
            let itemFunc = getFuncionario(data.value)
            FuncionarioNombre = itemFunc.nombres;
        }
    }).dxValidator({
        validationGroup: "EstadoGroup",
        validationRules: [{
            type: "required",
            message: "Seleccione el Funcionario!"
        }]
    }).dxSelectBox("instance");
    ///Fin

    var selectBoxObjetoCitacion = $("#selectBoxObjetoCitacion").dxSelectBox({
        width: 350,
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "codigoObjetoCitacion",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "QuejasAmbientales/api/QuejasApi/ObjetoCitacion");
                }
            })
        }),
        displayExpr: "nombre",
        valueExpr: "codigoObjetoCitacion",
        searchEnabled: true,

        onValueChanged: function (data) {
            
        }
    }).dxValidator({
        validationGroup: "CitacionGroup",
        validationRules: [{
            type: "required",
            message: "Seleccione el objeto!"
        }]
    }).dxSelectBox("instance");

    ///Inicio
    var tecnicos = []

    function getTecnico(value) {
        for (var i = 0; i < tecnicos.length; i++) {
            let element = tecnicos[i]

            if (element.codFuncionario == value)
                return element
        }
    }

    function cargarTecnicos() {
        var res = $.getJSON($("#SIM").data("url") + "QuejasAmbientales/api/QuejasApi/GetFuncionarioAsync");
        res.then(data => {

            tecnicos = data
            Tecnico.option("dataSource", tecnicos)
        })
    }

    cargarTecnicos()

    var Tecnico = $('#treeBoxTecnico').dxSelectBox({
        width: 350,


        displayExpr: "nombres",
        valueExpr: "codFuncionario",
        searchEnabled: true,

        onValueChanged: function (data) {

            CodVisitaTecnicoQueja = data.value
            let itemFunc = getTecnico(data.value)
            TecnicoNombre = itemFunc.nombres;
        }
    }).dxValidator({
        validationGroup: "VisitaGroup",
        validationRules: [{
            type: "required",
            message: "Seleccione el Tecnico!"
        }]
    }).dxSelectBox("instance");
    ///Fin

    var selectBoxObjetoVisita = $("#selectBoxObjetoVisita").dxSelectBox({
        width: 350,
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "codigoObjetoVisita",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "QuejasAmbientales/api/QuejasApi/ObjetoVisita");
                }
            })
        }),
        displayExpr: "nombre",
        valueExpr: "codigoObjetoVisita",
        searchEnabled: true,

        onValueChanged: function (data) {

        }
    }).dxValidator({
        validationGroup: "VisitaGroup",
        validationRules: [{
            type: "required",
            message: "Seleccione el objeto!"
        }]
    }).dxSelectBox("instance");

    //checkbox

   

    //datebox
    var dateFechaRespuesta = $('#dateFechaRespuesta').dxDateBox({
        type: 'date',
        value: today,
        displayFormat: 'd/MM/yyyy'
    }).dxDateBox('instance');

    var dateFechaEstado = $('#dateFechaEstado').dxDateBox({
        type: 'date',
        value: today,
        displayFormat: 'd/MM/yyyy'
    }).dxDateBox('instance');

    var dateFechaCitacion = $('#dateFechaCitacion').dxDateBox({
        type: 'date',
        value: today,
        displayFormat: 'd/MM/yyyy'
    }).dxDateBox('instance');

    var dateFechaVisita = $('#dateFechaVisita').dxDateBox({
        type: 'date',
        value: today,
        displayFormat: 'd/MM/yyyy'
    }).dxDateBox('instance');
 

    //popups

    var popupNuevaQueja = $("#PopupNuevaQueja").dxPopup({
        width: 800,
        height: 750,
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Crear Queja",
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },

    }).dxPopup("instance");

    $("#popupBuscaExp").dxPopup({
        width: 900,
        height: 800,
        showTitle: true,
        title: "Buscar Expediente",
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },
    });

    $("#popExpediente").dxPopup({
        width: 1500,
        height: 800,
        showTitle: true,
        title: "Visualizar Expediente",
        dragEnabled: false,
        resizeEnabled: false,
        closeOnOutsideClick: true,
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },
    });

    $("#PopupIndicesExp").dxPopup({
        width: 1500,
        height: 800,
        showTitle: true,
        title: "Visualizar indices",
        dragEnabled: false,
        resizeEnabled: false,
        closeOnOutsideClick: true,
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },
    });

    var popupTerceros = $("#popupTerceros").dxPopup({
        width: 1100,
        height: 'auto',
        showTitle: true,
        title: "Tercero",
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },
    }).dxPopup("instance");

    var popupBuscarTercero = $("#popupBuscarTercero").dxPopup({
        width: 1000,
        height: "auto",
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Busqueda Tercero",
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },
    }).dxPopup("instance");

    var popupNuevaRespuesta = $("#PopupNuevaRespuesta").dxPopup({
        width: 700,
        height: 'auto',
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Crear Respuesta",
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },

    }).dxPopup("instance");

    var popupNuevoEstado = $("#PopupNuevoEstado").dxPopup({
        width: 700,
        height: 'auto',
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Crear estado",
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },

    }).dxPopup("instance");

    var popupNuevaCitacion = $("#PopupNuevaCitacion").dxPopup({
        width: 700,
        height: 'auto',
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Crear Citacion",
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },

    }).dxPopup("instance");

    var popupNuevoComentario = $("#PopupNuevoComentario").dxPopup({
    width: 700,
    height: 'auto',
    dragEnabled: true,
    resizeEnabled: false,
    hoverStateEnabled: true,
    title: "Crear Comentario",
    onShowing: function () {
        $('body').css('overflow', 'hidden');
    },
    onHiding: function () {
        $('body').css('overflow', 'auto');
    },

    }).dxPopup("instance");

    var popupNuevaVisita = $("#PopupNuevaVisita").dxPopup({
        width: 700,
        height: 'auto',
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Crear Visita",
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },

    }).dxPopup("instance");

    var popupNuevoInforme = $("#PopupNuevoInforme").dxPopup({
    width: 'auto',
    height: 'auto',
    dragEnabled: true,
    resizeEnabled: false,
    hoverStateEnabled: true,
    title: "Vincular Informe",
    onShowing: function () {
        $('body').css('overflow', 'hidden');
    },
    onHiding: function () {
        $('body').css('overflow', 'auto');
    },

    }).dxPopup("instance");

    var popupNuevoExpediente = $("#PopupNuevoExpediente").dxPopup({
        width: 'auto',
        height: 'auto',
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Vincular expediente",
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },

    }).dxPopup("instance");

    $("#popDocumento").dxPopup({
        width: 1100,
        height: 700,
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

    $("#popTramite").dxPopup({
        width: 1100,
        height: 700,
        showTitle: true,
        title: "Visualizar Tramite",
        dragEnabled: false,
        closeOnOutsideClick: true,
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },
    });

    $("#popReporte").dxPopup({
        width: 900,
        height: 800,
        showTitle: true,
        title: "Visualizar reporte",
        dragEnabled: false,
        closeOnOutsideClick: true,
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },
    });

    var popupNuevoTramite = $("#PopupNuevoTramite").dxPopup({
        width: 'auto',
        height: 'auto',
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Vincular Tramite",
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },

    }).dxPopup("instance");

    


});//end document

var QuejasDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"CodigoQueja","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'QuejasAmbientales/api/QuejasApi/ListadoQuejas', {
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

var TercerosQuejasDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"idQuejaTercero","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'QuejasAmbientales/api/QuejasApi/ListadoQuejaTerceros', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: true,
            cod: CodQueja
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

const tabs = [
    {
        id: 0,
        text: 'Terceros',
        icon: '',

    },
    {
        id: 1,
        text: 'Informes',
        icon: '',

    },
    {
        id: 2,
        text: 'Citaciones',
        icon: '',

    },
    {
        id: 3,
        text: 'Autos',
        icon: '',

    },
    {
        id: 4,
        text: 'Resoluciones',
        icon: '',

    },
    {
        id: 5,
        text: 'Notificaciones',
        icon: '',

    },

    {
        id: 6,
        text: 'Respuestas',
        icon: '',

    },
    {
        id: 7,
        text: 'Estados',
        icon: '',

    },
    {
        id: 8,
        text: 'Expedientes',
        icon: '',

    },
    {
        id: 9,
        text: 'Tramites',
        icon: '',

    },
    
    {
        id: 10,
        text: 'Atenciones',
        icon: '',

    },
    {
        id: 11,
        text: 'Oficios',
        icon: '',

    },

    {
        id: 12,
        text: 'Comentarios',
        icon: '',

    },
    {
        id: 13,
        text: 'Visitas',
        icon: '',

    },

];

function SeleccionaExp(IdExpediente) {
    var _popup = $("#popupBuscaExp").dxPopup("instance");
    _popup.hide();
   
        params = {
            codigoQueja: CodQueja, idExpediente: IdExpediente
        };
    var _Ruta = $('#SIM').data('url') + "QuejasAmbientales/api/QuejasApi/VincularExpediente";

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

                    $('#GidListadoQuejas').dxDataGrid({ dataSource: QuejasDataSource });
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                DevExpress.ui.dialog.alert('Ocurrió un problema Guardar Datos');
            }
        });

    
}

const TiposTercero = [
    {
        id: 0,
        text: 'Persona Natural',
        

    },
    {
        id: 1,
        text: 'Persona Juridica',
       

    }
    
];

var AutosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"idAuto","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'QuejasAmbientales/api/QuejasApi/ListadoAutos', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: true,
            id: CodQueja
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var ResolucionesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"idResolucion","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'QuejasAmbientales/api/QuejasApi/ListadoResoluciones', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: true,
            id: CodQueja
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var RespuestasDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"codContestaQueja","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'QuejasAmbientales/api/QuejasApi/ListadoRespuestas', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: true,
            id: CodQueja
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var EstadosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"codEstadoQueja","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'QuejasAmbientales/api/QuejasApi/ListadoEstados', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: true,
            id: CodQueja
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var CitacionesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"codCitacionQueja","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'QuejasAmbientales/api/QuejasApi/ListadoCitaciones', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: true,
            id: CodQueja
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var ComentariosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"idQuejaComentario","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'QuejasAmbientales/api/QuejasApi/ListadoQuejaComentario', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: true,
            id: CodQueja
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var VisitasDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"codVisitaQueja","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'QuejasAmbientales/api/QuejasApi/ListadoVisitasQueja', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: true,
            id: CodQueja
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var OficiosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"idQuejaOficio","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'QuejasAmbientales/api/QuejasApi/ListadoOficioQueja', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: true,
            id: CodQueja
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var AtencionesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"IdAtencion","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'QuejasAmbientales/api/QuejasApi/QuejaAtenciones', {
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
            id: CodQueja
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var ListadoInformesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"codInformeQueja","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'QuejasAmbientales/api/QuejasApi/ListadoInformeQueja', {
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
            id: CodQueja
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var BuscarInformesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"IdInforme","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'QuejasAmbientales/api/QuejasApi/BuscarInforme', {
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

var ListadoSolicitudesQuejaDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"codigoSolicitud","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'QuejasAmbientales/api/QuejasApi/ListadoSolicitudes', {
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
            id: CodQueja
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var BuscarSolicitudQuejaDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"CM","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'QuejasAmbientales/api/QuejasApi/BuscarSolicitud', {
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

var NotificacionesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"idNotificacion","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'QuejasAmbientales/api/QuejasApi/ListadoNotificaciones', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: true,
            id: CodQueja
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var ListadoTramitesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"codigoTramiteExpediente","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'QuejasAmbientales/api/QuejasApi/ListadoTramiteQueja', {
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
            id: CodQueja
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var BuscartramitesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"codTramite","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'QuejasAmbientales/api/QuejasApi/BuscarTramite', {
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


//funcion expedientes



/*document.body.style.overflow = 'hidden';*/ // esto desabilita el scroll de la pagina

