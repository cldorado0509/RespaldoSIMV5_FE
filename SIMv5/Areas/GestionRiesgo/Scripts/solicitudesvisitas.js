var IdRegistro;
var idTramite;
var idDocumento;


$(document).ready(function () {

    $("#GidListado").dxDataGrid({
        dataSource: SolicitudesDataSource,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        export: {
            enabled: true,
            allowExportSelectedData: true,
        },
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20, 50]
        },
        filterRow: {
            visible: true,
            emptyPanelText: 'Arrastre una columna para agrupar'
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Buscar..."
        },
        selection: {
            mode: 'single'
        },
        with:2000,
        hoverStateEnabled: true,
        remoteOperations: true,
        columns: [
            { dataField: 'idSolicitudVisita', width: '5%', caption: 'Id', alignment: 'center' },
            { dataField: 'solicitante', width: '10%', caption: 'Solicitante', alignment: 'center' },
            { dataField: 'numeroContacto', width: '5%', caption: 'Nro de Contacto', alignment: 'center' },
            { dataField: 'codigoTramite', width: '5%', caption: 'Código Trámite', alignment: 'center' },
            { dataField: 'radicadoSolicitud', width: '5%', caption: 'Radicado Solicitud', alignment: 'center' },
            { dataField: 'fechaRadicadoSolicitud', width: '10%', caption: 'Fecha del Radicado', alignment: 'center' },
            { dataField: 'barrioVereda', width: '10%', caption: 'Barrio o Vereda', alignment: 'center' },
            { dataField: 'direccion', width: '10%', caption: 'Otra Dirección', alignment: 'center', visible: false },
            { dataField: 'viaPrincipal', width: '´2%', caption: 'Dirección', alignment: 'center' },
            { dataField: 'numViaPrincipal', width: '2%', caption: '', alignment: 'center' },
            { dataField: 'letraViaPrincipal', width: '2%', caption: '', alignment: 'center' },
            { dataField: 'sentidoViaPrincipal', width: '2%', caption: '', alignment: 'center' },
            { dataField: 'viaSecundaria', width: '2%', caption: '', alignment: 'center' },
            { dataField: 'numViaSecundaria', width: '2%', caption: '', alignment: 'center' },
            { dataField: 'letraViaSecundaria', width: '2%', caption: '', alignment: 'center' },
            { dataField: 'sentidoViaSecundaria', width: '2%', caption: '', alignment: 'center' },
            { dataField: 'placa', width: '2%', caption: '', alignment: 'center' },
            { dataField: 'interior', width: '2%', caption: '', alignment: 'center' },
            { dataField: 'radicadoSalida', width: '5%', caption: 'Radicado Salida', alignment: 'center' },
            { dataField: 'fechaRadicadoSalida', width: '10%', caption: 'F. Radicado Sal', alignment: 'center' },
            { dataField: 'latitud', width: '10%', caption: 'Latitud', alignment: 'center', visible: false },
            { dataField: 'longitud', width: '10%', caption: 'Longitud', alignment: 'center', visible: false },
            { dataField: 'fechaVisita', width: '10%', caption: 'Fecha Visita', alignment: 'center', visible: false },
            { dataField: 'mes', width: '5%', caption: 'Mes', alignment: 'center', visible: false },
            { dataField: 'numeroPersonasImpactadas', width: '10%', caption: 'Nro Personas Impactadas', alignment: 'center', visible: false },
            { dataField: 'destinatarios', width: '10%', caption: 'Destinatarios', alignment: 'center', visible: false },
            { dataField: 'quebradas', width: '10%', caption: 'Quebradas', alignment: 'center', visible: false },
            { dataField: 'calificacionRiesgo', width: '10%', caption: 'Calificación del Riesgo', alignment: 'center', visible: false },
            { dataField: 'municipio', width: '10%', caption: 'Municipio', alignment: 'center', visible: false },
            { dataField: 'tipoVisita', width: '10%', caption: 'Tipo de Visita', alignment: 'center', visible: false },
            { dataField: 'origen', width: '10%', caption: 'Origen', alignment: 'center', visible: false },
            { dataField: 'suelo', width: '10%', caption: 'Suelo', alignment: 'center', visible: false },
            { dataField: 'evento', width: '10%', caption: 'Evento', alignment: 'center', visible: false },
            { dataField: 'nivelRiesgo', width: '10%', caption: 'Nivel de Riesgo', alignment: 'center', visible: false },
            {
                width: 80,
                alignment: 'center',
                caption: "Editar",
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        caption: "Editar",
                        height: 20,
                        hint: 'Editar la información relacionada con el registro seleccionado',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "GestionRiesgo/api/SolicitudVisitaApi/ObtenerSolicitudVisitaAsync";
                            $.getJSON(_Ruta,
                                {
                                    id: options.data.idSolicitudVisita
                                }).done(function (data) {
                                    if (data !== null) {
                                        txtSolicitante.option("value",data.solicitante);
                                        txtNroContacto.option("value", data.numeroContacto);
                                        txtCodTramite.option("value", data.codigoTramite);
                                        txtAnio.option("value",data.fechaRadicadoSolicitud);
                                        txtRadicadoSolicitud.option("value", data.radicadoSolicitud);
                                        cboMunicipio.option("value", data.municipioId);
                                        txtBarrioVereda.option("value", data.barrioVereda);
                                        cboViaPpal.option("value", data.viaPrincipal);
                                        cboViaSec.option("value", data.viaSecundaria);
                                        cboLetraPpal.option("value", data.letraViaPrincipal);
                                        cboLetraSec.option("value", data.letraViaSecundaria);
                                        cboSentidoViaPpal.option("value", data.sentidoViaPrincipal);
                                        cboSentidoViaSec.option("value", data.sentidoViaSecundaria);
                                        txtNroPpal.option("value", data.numViaPrincipal);
                                        txtNroSec.option("value", data.numViaSecundaria);
                                        txtPlaca.option("value", data.placa);
                                        txtInterior.option("value", data.interior);
                                        txtDireccion.option("value", data.direccion);
                                        cboFuncionario.option("value", data.funcionarioId);
                                        txtLatitud.option("value", data.latitud);
                                        txtLongitud.option("value", data.longitud);
                                        cboTipoVisita.option("value", data.tipoVisitaId);
                                        cboTipoOrigen.option("value", data.origenId);
                                        cboTipoSuelo.option("value", data.sueloId);
                                        cboTipoEvento.option("value", data.eventoId);
                                        cboNivelRiesgo.option("value", data.nivelRiesgoId);
                                        txtCalificacionRiesgo.option("value", data.calificacionRiesgo);
                                        txtAnioSalida.option("value", data.fechaRadicadoSalida);
                                        txtRadicadoSalida.option("value", data.radicadoSalida);
                                        txtNroPersonas.option("value", data.numeroPersonasImpactadas);
                                        cboMes.option("value",data.mes);
                                        txtDestinatarios.option("value", data.destinatarios);
                                        txtQuebradas.option("value", data.quebradas);
                                        popup.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                                });
                        }
                    }).appendTo(container);
                }
            },
            {
                width: 80,
                caption: "Eliminar",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'remove',
                        height: 20,
                        hint: 'Eliminar la solicitud de visita',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar el registro seleccionado?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "GestionRiesgo/api/SolicitudVisitaApi/EliminarSolicitudVisitaAsync";
                                  var params = {
                                      idSolicitudVisita: options.data.idSolicitudVisita
                                    };
                                    $.ajax({
                                        type: 'POST',
                                        url: _Ruta,
                                        contentType: "application/json",
                                        dataType: 'json',
                                        data: JSON.stringify(params),
                                        success: function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar registro seleccionado');
                                            else {
                                                $('#GidListado').dxDataGrid({ dataSource: SolicitudesDataSource });
                                                DevExpress.ui.dialog.alert('Registro eliminado correctamente!');
                                            }
                                        },
                                        error: function (xhr, textStatus, errorThrown) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar registro seleccionado');
                                        }
                                    });
                                }
                            });
                        }
                    }).appendTo(container);
                }
            },
            {

                width: '5%',
                caption: "Ver Informe",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'search',
                        height: 20,
                        hint: 'Ver Documento',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/DerechoPeticionApi/ObtenerDocumentosCOD";
                            $.getJSON(_Ruta,
                                {
                                    radicado: options.data.radicadoSalida,
                                    anio: options.data.fechaVisita
                                }).done(function (data) {
                                    if (data.id > 0) {
                                        idTramite = data.tramiteId;
                                        idDocumento = data.documentoId;
                                        var _popup = $("#popDocumento").dxPopup("instance");
                                        _popup.show();

                                        var ruta = $('#SIM').data('url') + 'ControlVigilancia/Reposiciones/LeeDoc?CodTramite=' + idTramite + '&CodDocumento=' + idDocumento;
                                        $("#DocumentoAdjunto").attr("src", ruta);
                                    }
                                    else {
                                        alert("Documento no encontrado!");
                                    }

                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                                });
                        }
                    }).appendTo(container);
                }
            }
        ],
        onExporting: function (e) {
            e.component.beginUpdate();
            e.component.columnOption('latitud', 'visible', true);
            e.component.columnOption('longitud', 'visible', true);
            e.component.columnOption('direccion', 'visible', true);
            e.component.columnOption('fechaVisita', 'visible', true);
            e.component.columnOption('numeroPersonasImpactadas', 'visible', true);
            e.component.columnOption('destinatarios', 'visible', true);
            e.component.columnOption('quebradas', 'visible', true);
            e.component.columnOption('calificacionRiesgo', 'visible', true);
            e.component.columnOption('tipoVisita', 'visible', true);
            e.component.columnOption('origen', 'visible', true);
            e.component.columnOption('suelo', 'visible', true);
            e.component.columnOption('evento', 'visible', true);
            e.component.columnOption('nivelRiesgo', 'visible', true);
        },
        onExported: function (e) {
            e.component.columnOption('latitud', 'visible', false);
            e.component.columnOption('longitud', 'visible', false);
            e.component.columnOption('direccion', 'visible', false);
            e.component.columnOption('fechaVisita', 'visible', false);
            e.component.columnOption('numeroPersonasImpactadas', 'visible', false);
            e.component.columnOption('destinatarios', 'visible', false);
            e.component.columnOption('quebradas', 'visible', false);
            e.component.columnOption('calificacionRiesgo', 'visible', false);
            e.component.columnOption('tipoVisita', 'visible', false);
            e.component.columnOption('origen', 'visible', false);
            e.component.columnOption('suelo', 'visible', false);
            e.component.columnOption('evento', 'visible', false);
            e.component.columnOption('nivelRiesgo', 'visible', false);
            e.component.endUpdate();
        },  
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                IdRegistro = data.idSolicitudVisita;
            }
        }

    });

    $("#popDocumento").dxPopup({
        width: 900,
        height: 800,
        resizeEnabled: true,
        showTitle: true,
        title: "Visualizar Documento",
        dragEnabled: true,
        closeOnOutsideClick: true,

    });

    var txtSolicitante = $("#txtSolicitante").dxTextBox({
        value: "",
        readOnly: false
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe ingresar el nombre del solicitante!"
        }]
    }).dxTextBox("instance");

    var txtNroContacto = $("#txtNroContacto").dxTextBox({
        value: "",
        readOnly: false
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Ingrese el número de contacto del solicitante!"
        }]
    }).dxTextBox("instance");

    var txtCodTramite = $("#txtCodTramite").dxTextBox({
        value: "",
        readOnly: false
    }).dxTextBox("instance");

    var txtRadicadoSolicitud = $("#txtRadicadoSolicitud").dxTextBox({
        value: "",
        readOnly: false
    }).dxTextBox("instance");

    var txtAnio = $("#txtAnio").dxNumberBox({
        value: 2022,
        readOnly: false,
        min: 2016,
        max: 2027,
        showSpinButtons: true,
    }).dxNumberBox("instance");

    var txtAnioSalida = $("#txtAnioSalida").dxNumberBox({
        value: 2022,
        readOnly: false,
        min: 2016,
        max: 2027,
        showSpinButtons: true,
    }).dxNumberBox("instance");
       
    var txtBarrioVereda = $("#txtBarrioVereda").dxTextBox({
        value: "",
        readOnly: false
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Ingrese el nombre del barrio o vereda asociado al evento de la solicitud!"
        }]
    }).dxTextBox("instance");


    var cboViaPpal = $("#cboViaPpal").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Value",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "ControlVigilancia/api/SolicitudVisitaApi/GetTiposViasAsync");
                }
            })
        }),
        displayExpr: "Text",
        searchEnabled: true
    }).dxSelectBox("instance");

    var cboViaSec = $("#cboViaSec").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Value",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "ControlVigilancia/api/SolicitudVisitaApi/GetTiposViasAsync");
                }
            })
        }),
        displayExpr: "Text",
        searchEnabled: true
    }).dxSelectBox("instance");

    var cboLetraPpal = $("#cboLetraPpal").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Value",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "ControlVigilancia/api/SolicitudVisitaApi/GetLetrasViasAsync");
                }
            })
        }),
        displayExpr: "Text",
        searchEnabled: true
    }).dxSelectBox("instance");

    var cboLetraSec = $("#cboLetraSec").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Value",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "ControlVigilancia/api/SolicitudVisitaApi/GetLetrasViasAsync");
                }
            })
        }),
        displayExpr: "Text",
        searchEnabled: true
    }).dxSelectBox("instance");

    var cboSentidoViaPpal = $("#cboSentidoViaPpal").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Value",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "ControlVigilancia/api/SolicitudVisitaApi/GetSentidosViasAsync");
                }
            })
        }),
        displayExpr: "Text",
        searchEnabled: true
    }).dxSelectBox("instance");

    var cboSentidoViaSec = $("#cboSentidoViaSec").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Value",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "ControlVigilancia/api/SolicitudVisitaApi/GetSentidosViasAsync");
                }
            })
        }),
        displayExpr: "Text",
        searchEnabled: true
    }).dxSelectBox("instance");

    var txtNroPpal = $("#txtNroPpal").dxNumberBox({
        value: "",
    }).dxNumberBox("instance");

    var txtNroSec = $("#txtNroSec").dxNumberBox({
        value: "",
    }).dxNumberBox("instance");

    var txtPlaca = $("#txtPlaca").dxTextBox({
        value: "",
        readOnly: false
    }).dxTextBox("instance");

    var txtInterior = $("#txtInterior").dxTextBox({
        value: "",
        readOnly: false
    }).dxTextBox("instance");

    var txtDireccion = $("#txtDireccion").dxTextBox({
        value: "",
        readOnly: false
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Ingrese la dirección relacionada con el evento de la solicitud!"
        }]
    }).dxTextBox("instance");

    var cboFuncionario = $("#cboFuncionario").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "ControlVigilancia/api/ReposicionesApi/GetTecnicos");
                }
            })
        }),
        displayExpr: "Nombre",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Seleccione el Funcionario responsable!"
        }]
    }).dxSelectBox("instance");
  

    var txtLatitud = $("#txtLatitud").dxNumberBox({
        value: "",
        format: "#.#######0",
    }).dxNumberBox("instance");

    var txtLongitud = $("#txtLongitud").dxNumberBox({
        value: "",
        format: "#.######0",
    }).dxNumberBox("instance");


    $("#cmdValTramite").dxButton({
        text: "Verificar",
        type: "success",
        height: 35,
        onClick: function () {
            var _Ruta = $('#SIM').data('url') + "GestionRiesgo/api/SolicitudVisitaApi/ValidarTramiteAsync";
            $.getJSON(_Ruta,
                {
                    idTramite: txtCodTramite.option('value')
                }).done(function (data) {
                    if (data.codTramite === 0) {
                        alert("El trámite no existe!");
                        txtCodTramite.option("value", "0");
                    }
                    else {
                        alert("El trámite existe! : " + data.mensaje + " - " + data.comentarios);
                    }
                  
                   
                }).fail(function (jqxhr, textStatus, error) {
                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                });

        }
    });

    $("#cmdCOR").dxButton({
        text: "Consultar",
        stylingMode: 'outlined',
        type: "danger",
        height: 35,
        onClick: function () {
            var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/DerechoPeticionApi/ObtenerDocumentos";
            $.getJSON(_Ruta,
                {
                    radicado: txtRadicadoSolicitud.option('value'),
                    anio: txtAnio.option('value')
                }).done(function (data) {
                    if (data.id > 0) {
                        idTramite = data.tramiteId;
                        idDocumento = data.documentoId;
                        alert('Documento válido!');
                    }
                    else {
                        alert("Documento no encontrado!");
                    }

                }).fail(function (jqxhr, textStatus, error) {
                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                });
        }
    });

    $("#cmdCOD").dxButton({
        text: "Consultar",
        stylingMode: 'outlined',
        type: "danger",
        height: 35,
        onClick: function () {
            var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/DerechoPeticionApi/ObtenerDocumentosCOD";
            $.getJSON(_Ruta,
                {
                    radicado: txtRadicadoSalida.option('value'),
                    anio: txtAnioSalida.option('value')
                }).done(function (data) {
                    if (data.id > 0) {
                        idTramite = data.tramiteId;
                        idDocumento = data.documentoId;
                        alert('Documento válido!');
                    }
                    else {
                        alert("Documento no encontrado!");
                    }

                }).fail(function (jqxhr, textStatus, error) {
                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                });
        }
    });


    var cboTipoVisita = $("#cboTipoVisita").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "idTipoVisita",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "ControlVigilancia/api/TiposVisitaApi/GetTiposVisitasAsync");
                }
            })
        }),
        displayExpr: "nombre",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Seleccione el Tipo de Visita!"
        }]
    }).dxSelectBox("instance");


    var cboMunicipio = $("#cboMunicipio").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "DesarrolloEconomico/api/EmpresaApi/GetMunicipios");
                }
            })
        }),
        displayExpr: "Nombre",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El Seleccione el Municipio!"
        }]
    }).dxSelectBox("instance");

    var cboTipoOrigen = $("#cboTipoOrigen").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "idOrigen",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "ControlVigilancia/api/TipoOrigenApi/GetListTiposOrigenesAsync");
                }
            })
        }),
        displayExpr: "nombre",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Seleccione el Tipo de Orígen!"
        }]
    }).dxSelectBox("instance");

    var cboTipoSuelo = $("#cboTipoSuelo").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "idSuelo",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "ControlVigilancia/api/TipoSueloApi/GetListTiposSuelosAsync");
                }
            })
        }),
        displayExpr: "nombre",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Seleccione el Tipo de Suelo!"
        }]
    }).dxSelectBox("instance");

    var cboTipoEvento = $("#cboTipoEvento").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "idEvento",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "ControlVigilancia/api/EventosApi/GetListEventosAsync");
                }
            })
        }),
        displayExpr: "nombre",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Seleccione el Tipo de Evento!"
        }]
    }).dxSelectBox("instance");

    var cboNivelRiesgo = $("#cboNivelRiesgo").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "idNivelRiesgo",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "ControlVigilancia/api/NivelesRiesgoApi/GetListNivelesRiesgoAsync");
                }
            })
        }),
        displayExpr: "nombre",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Seleccione el Tipo de Nivel de Riesgo!"
        }]
    }).dxSelectBox("instance");


    var txtCalificacionRiesgo = $("#txtCalificacionRiesgo").dxNumberBox({
        value: "",
        format: "#.######0",
    }).dxNumberBox("instance");


    var txtRadicadoSalida = $("#txtRadicadoSalida").dxTextBox({
        value: "",
        readOnly: false
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Ingrese el número del radicado de salida!"
        }]
    }).dxTextBox("instance");

    var dRadicadoSalida = $("#dRadicadoSalida").dxDateBox({
        type: "date",
        value: new Date(),
        showAnalogClock: false,
        onOpened: function (args) {
            let position = args.component._popup.option("position");
            position.my = "center";
            position.at = "left";
            args.component._popup.option("position", position);
        }
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "La fecha del Radicado de salida"
        }]
    }).dxDateBox("instance");

    var txtNroPersonas = $("#txtNroPersonas").dxNumberBox({
        value: "",
        format: "##0",
    }).dxNumberBox("instance");

    var cboMes = $("#cboMes").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "GestionRiesgo/api/SolicitudVisitaApi/GetMeses");
                }
            })
        }),
        displayExpr: "name",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Seleccione el Mes!"
        }]
    }).dxSelectBox("instance");


    var txtDestinatarios = $("#txtDestinatarios").dxTextBox({
        value: "",
        readOnly: false
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Ingrese los destinatarios!"
        }]
    }).dxTextBox("instance");

    var txtQuebradas = $("#txtQuebradas").dxTextBox({
        value: "",
        readOnly: false
    }).dxTextBox("instance");


    $("#btnNuevo").dxButton({
        stylingMode: "contained",
        text: "Nuevo",
        type: "success",
        width: 200,
        height: 30,
        icon: 'add',
        onClick: function () {
            IdRegistro = 0;
            txtSolicitante.option("value", "");
            txtNroContacto.option("value","");
            txtCodTramite.option("value","");
            txtAnio.option("value", "");
            txtRadicadoSolicitud.option("value", "");
            cboMunicipio.option("value",0);
            txtBarrioVereda.option("value", "");
            cboViaPpal.option("value", 0);
            cboViaSec.option("value", 0);
            cboLetraPpal.option("value", 0);
            cboLetraSec.option("value", 0);
            cboSentidoViaPpal.option("value", 0);
            cboSentidoViaSec.option("value", 0);
            txtNroPpal.option("value", "");
            txtNroSec.option("value", "");
            txtPlaca.option("value", "");
            txtInterior.option("value", "");
            txtDireccion.option("value", "");
            cboFuncionario.option("value", 0);
            txtLatitud.option("value", 6.2518400);
            txtLongitud.option("value", -75.5635900);
            cboTipoVisita.option("value",0);
            cboTipoOrigen.option("value",0);
            cboTipoSuelo.option("value",0);
            cboTipoEvento.option("value", 0);
            cboNivelRiesgo.option("value",0);
            txtCalificacionRiesgo.reset();
            txtAnioSalida.option("value", "");
            txtRadicadoSalida.option("value", "");
            txtNroPersonas.reset();
            cboMes.option("value", 1);
            txtDestinatarios.option("value", "");
            txtQuebradas.option("value", "");
            popup.show();
        }
    });


    $("#btnGuarda").dxButton({
        text: "Guardar",
        type: "default",
        height: 30,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
              var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/DerechoPeticionApi/ObtenerDocumentosCOD";
              $.getJSON(_Ruta,
                            {
                               radicado: txtRadicadoSalida.option('value'),
                               anio: txtAnioSalida.option('value')
                            }).done(function (data) {
                               if (data.id <= 0) {
                                    alert("Comunicación Oficial Despachada no encontrada!");
                                }
                               else {
                                    var id = IdRegistro;
                                    var solicitante = txtSolicitante.option("value");
                                    var numeroContacto = txtNroContacto.option("value");
                                    var codigoTramite = txtCodTramite.option("value");
                                    var fechaRadicadoSolicitud = txtAnio.option("value");;
                                    var radicadoSolicitud = txtRadicadoSolicitud.option("value");
                                    var municipioId = 0;
                                    if (cboMunicipio.option("value").Id === undefined) {
                                        municipioId = cboMunicipio.option("value");
                                    } else {
                                        municipioId = cboMunicipio.option("value").Id;
                                    }
                                    var barrioVereda = txtBarrioVereda.option("value");
                                    var direccion = txtDireccion.option("value");
                                    var funcionarioId = 0;
                                    if (cboFuncionario.option("value").Id === undefined) {
                                        funcionarioId = cboFuncionario.option("value");
                                    }
                                    else {
                                        funcionarioId = cboFuncionario.option("value").Id;
                                    }
                                    var latitud = txtLatitud.option("value");
                                    var longitud = txtLongitud.option("value");
                                    var tipoVisitaId = 0;
                                    if (cboTipoVisita.option("value").idTipoVisita === undefined) {
                                        tipoVisitaId = cboTipoVisita.option("value");
                                    } else {
                                        tipoVisitaId = cboTipoVisita.option("value").idTipoVisita;
                                    }
                                    var origenId = 0;
                                    if (cboTipoOrigen.option("value").idOrigen === undefined) {
                                        origenId = cboTipoOrigen.option("value");
                                    } else {
                                        origenId = cboTipoOrigen.option("value").idOrigen;
                                    }
                                    var sueloId = 0;
                                    if (cboTipoSuelo.option("value").idSuelo === undefined) {
                                        sueloId = cboTipoSuelo.option("value");
                                    } else {
                                        sueloId = cboTipoSuelo.option("value").idSuelo;
                                    }
                                    var eventoId = 0;
                                    if (cboTipoEvento.option("value").idEvento === undefined) {
                                        eventoId = cboTipoEvento.option("value");
                                    } else {
                                        eventoId = cboTipoEvento.option("value").idEvento;
                                    }
                                    var nivelRiesgoId = 0;
                                    if (cboNivelRiesgo.option("value").idNivelRiesgo === undefined) {
                                        nivelRiesgoId = cboNivelRiesgo.option("value");
                                    } else {
                                        nivelRiesgoId = cboNivelRiesgo.option("value").idNivelRiesgo;
                                    }
                                    var calificacionRiesgo = txtCalificacionRiesgo.option("value");
                                    var fechaVisita = '2022-03-11T16:49:09.883Z';
                                    var esMonitoreo = true;
                                    var fechaRadicadoSalida = txtAnioSalida.option("value");
                                    var radicadoSalida = txtRadicadoSalida.option("value");
                                    var numeroPersonasImpactadas = txtNroPersonas.option("value");
                                    var mes = 0;
                                    if (cboMes.option("value").id === undefined) {
                                        mes = cboMes.option("value");
                                    } else {
                                        mes = cboMes.option("value").id;
                                    }
                                    var destinatarios = txtDestinatarios.option("value");
                                    var quebradas = txtQuebradas.option("value");

                                    var viaPrincipal = "";
                                    if (cboViaPpal.option("value").Value === undefined) {
                                        viaPrincipal = cboViaPpal.option("value");
                                    } else {
                                        viaPrincipal = cboViaPpal.option("value").Value;
                                    }

                                    var numViaPrincipal = txtNroPpal.option("value");

                                    var letraViaPrincipal = "";
                                    if (cboLetraPpal.option("value").Value === undefined) {
                                        letraViaPrincipal = cboLetraPpal.option("value");
                                    } else {
                                        letraViaPrincipal = cboLetraPpal.option("value").Value;
                                    }

                                    var sentidoViaPrincipal = "";
                                    if (cboSentidoViaPpal.option("value").Value === undefined) {
                                        sentidoViaPrincipal = cboSentidoViaPpal.option("value");
                                    } else {
                                        sentidoViaPrincipal = cboSentidoViaPpal.option("value").Value;
                                    }


                                    var viaSecundaria = "";
                                    if (cboViaSec.option("value").Value === undefined) {
                                        viaSecundaria = cboViaSec.option("value");
                                    } else {
                                        viaSecundaria = cboViaSec.option("value").Value;
                                    }

                                    var numViaSecundaria = txtNroSec.option("value");

                                    var letraViaSecundaria = "";
                                    if (cboLetraSec.option("value").Value === undefined) {
                                        letraViaSecundaria = cboLetraSec.option("value");
                                    } else {
                                        letraViaSecundaria = cboLetraSec.option("value").Value;
                                    }

                                    var sentidoViaSecundaria = "";
                                    if (cboSentidoViaSec.option("value").Value === undefined) {
                                        sentidoViaSecundaria = cboSentidoViaSec.option("value");
                                    } else {
                                        sentidoViaSecundaria = cboSentidoViaSec.option("value").Value;
                                    }

                                    var placa = txtPlaca.option("value");
                                    var interior = txtInterior.option("value");

                                    var params = {
                                        idSolicitudVisita: id, solicitante: solicitante, numeroContacto: numeroContacto,
                                        codigoTramite: codigoTramite, fechaRadicadoSolicitud: fechaRadicadoSolicitud,
                                        radicadoSolicitud: radicadoSolicitud, municipioId: municipioId, barrioVereda: barrioVereda,
                                        viaPrincipal: viaPrincipal, numViaPrincipal: numViaPrincipal, letraViaPrincipal: letraViaPrincipal, sentidoViaPrincipal: sentidoViaPrincipal,
                                        viaSecundaria: viaSecundaria, numViaSecundaria: numViaSecundaria, letraViaSecundaria: letraViaSecundaria, sentidoViaSecundaria: sentidoViaSecundaria,
                                        placa: placa, interior: interior,
                                        direccion: direccion, funcionarioId: funcionarioId, latitud: latitud, longitud: longitud,
                                        tipoVisitaId: tipoVisitaId, origenId: origenId, sueloId: sueloId, eventoId: eventoId,
                                        nivelRiesgoId: nivelRiesgoId, calificacionRiesgo: calificacionRiesgo, fechaVisita: fechaVisita,
                                        esMonitoreo: esMonitoreo, fechaRadicadoSalida: fechaRadicadoSalida, radicadoSalida: radicadoSalida,
                                        numeroPersonasImpactadas: numeroPersonasImpactadas, mes: mes, destinatarios: destinatarios, quebradas: quebradas
                                    };

                                    var _Ruta = $('#SIM').data('url') + "GestionRiesgo/api/SolicitudVisitaApi/GuardarSolicitudVisitaAsync";
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
                                                $('#GidListado').dxDataGrid({ dataSource: SolicitudesDataSource });
                                                $("#PopupNuevo").dxPopup("instance").hide();
                                            }
                                        },
                                        error: function (xhr, textStatus, errorThrown) {
                                            DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                                        }
                                    });
                                }
                        }).fail(function (jqxhr, textStatus, error) {
                                DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
               });
        }
    });

    var popup = $("#PopupNuevo").dxPopup({
        width: 900,
        height: "auto",
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Solicitud de Visita"
    }).dxPopup("instance");


});



var SolicitudesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"codigoTramite","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'GestionRiesgo/api/SolicitudVisitaApi/GetSolicitudesAsync', {
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
            CodFuncionario: CodigoFuncionario
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});



