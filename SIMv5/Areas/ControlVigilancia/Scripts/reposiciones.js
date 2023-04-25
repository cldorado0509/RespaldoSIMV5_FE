var IdReposicion = -1;
var IdDetReposicion = 0;
var idDocActo = 0;
var idAsunto = 0;
var tipomedida = 0;
var idControl = 0;
var idDocumento = 0;
var IdRegistro = -1;
var idTramite = 0;


$(document).ready(function () {

    $("#GidListado").dxDataGrid({
        dataSource: ReposicionesDataSource,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        export: {
            enabled: true,
            allowExportSelectedData: true,
        },
        paging: {
            pageSize: 5
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
        hoverStateEnabled: true,
        remoteOperations: true,
        columns: [
            { dataField: 'ID', width: '5%', caption: 'Id' },
            { dataField: 'CM', width: '5%', caption: 'CM' },
            { dataField: 'PROYECTO', width: '20%', caption: 'Nombre del Proyecto - (Instalación)' },
            { dataField: 'COORDENADAX', width: '10%', caption: 'Latitud', visible: false },
            { dataField: 'COORDENADAY', width: '10%', caption: 'Longitud', visible: false },
            { dataField: 'NUMERO_ACTO', width: '10%', caption: 'Radicado Resolución', visible: false },
            { dataField: 'FECHA_ACTO', width: '10%', caption: 'Fecha Radicado', dataType:'date', visible: false },
            { dataField: 'ANIO_ACTO', width: '10%', caption: 'Año Radicado', visible: false },
            { dataField: 'ASUNTO', width: '20%', caption: 'Asunto - (Permiso)' },
            {dataField: 'ASUNTO', width: '20%', caption: 'Acto Administrativo', visible: true, customizeText: function (cellInfo) {

                let dato = cellInfo.value.split(':')[1];
                if (dato) {
                    return dato.split('-')[0];
                } else {
                    return '';
                }
            }
            },
            {
                dataField: 'ASUNTO', width: '20%', caption: 'F.Acto Administrativo', visible: true, customizeText: function (cellInfo) {
                    let dato = cellInfo.value.split(':')[2];
                    if (dato) {
                        return dato.split('-')[0] + '-' + dato.split('-')[1] + '-'+ dato.split('-')[2];
                    } else {
                        return '';
                    }
                }
            },
            {
                dataField: 'ASUNTO', width: '20%', caption: 'Año Act Admin', visible: true, customizeText: function (cellInfo) {
                    let dato = cellInfo.value.split(':')[2];
                    if (dato) {
                        return dato.split('-')[2];
                    } else {
                        return '';
                    }
                }
            },
            { dataField: 'CODIGO_TRAMITE', width: '10%', caption: 'Cod Tarea Informe Técnico' },
            { dataField: 'OBSERVACIONES', width: '35%', caption: 'Observaciones', dataType: 'string' },
            { dataField: 'ENTIDAD_PUBLICA', caption: 'Es Entidad Pública', visible: false },
            {
                visible: canEdit,
                caption: "Editar",
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        caption:"Editar",
                        height: 20,
                        hint: 'Editar la información de la reposición',
                        onClick: function (e) {

                            var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/ReposicionesApi/ObtenerReposicion";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.ID
                                }).done(function (data) {
                                    if (data !== null) {
                                        IdRegistro = parseInt(data.id);
                                        IdReposicion = parseInt(data.id);
                                        idAsunto = parseInt(data.codigoSolicitud);
                                        idTramite = data.codigoTramite;
                                        idDocumento = data.codigoDocumento;
                                        txtCM.option("value", data.cm);
                                        txtProyecto.option("value", data.proyecto);
                                        txtCodTramite.option("value", data.codigoTramite);
                                        txtNombreProyectoA.option("value", data.nombreProyecto);
                                        cboTipoMedidaAdicional.option("value", data.tipoMedidaId);
                                        tipomedida = data.tipoMedidaId;
                                        txtAsunto.option("value", data.asunto);
                                        txtTalaAut.option("value", data.talaAutorizada);
                                        txtdAPMen10Autorizada.option("value", data.dAPMen10Autorizada);
                                        txtvolumenAutorizado.option("value", data.volumenAutorizado);
                                        txttransplanteAutorizado.option("value", data.transplanteAutorizado);
                                        txtpodaAutorizada.option("value", data.podaAutorizada);
                                        txtconservacionAutorizada.option("value", data.conservacionAutorizada);
                                        txtreposicionAutorizada.option("value", data.reposicionAutorizada);
                                        txtAutorizado.option("value", data.autorizado);
                                        txtObservaciones.option("value", data.observaciones);
                                        txtCoordenadaX.option("value", data.coordenadaX);
                                        txtCoordenadaY.option("value", data.coordenadaY);
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
                visible: canDelete,
                width: '5%',
                caption: "Eliminar",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'remove',
                        height: 20,
                        hint: 'Eliminar la Reposición',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar el registro seleccionado?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {

                                    var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/ReposicionesApi/EliminarReposicion";
                                    $.getJSON(_Ruta,
                                        {
                                            id: options.data.ID
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar registro seleccionado');
                                            else {
                                                $('#GidListado').dxDataGrid({ dataSource: ReposicionesDataSource });
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar registro seleccionado');
                                        });
                                }
                            });
                        }
                    }).appendTo(container);
                }
            },
            {
                width: '5%',
                caption: "Seguimientos de control",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'event',
                        height: 20,
                        hint: 'Seguimientos de control',
                        onClick: function (e) {
                            IdReposicion = options.data.ID;
                            IdDetReposicion = 0;
                             $("#divLblCM").dxTextBox({
                                value: "CM :" + options.data.CM,
                                readOnly: true,
                            }).dxTextBox("instance");

                            $("#GridListadoControl").dxDataGrid({
                                dataSource: ControlDataSource,
                                with: "95%",
                                allowColumnResizing: true,
                                loadPanel: { enabled: true, text: 'Cargando Datos...' },
                                noDataText: "Sin datos para mostrar",
                                showBorders: true,
                                paging: {
                                    pageSize: 10
                                },
                                pager: {
                                    showPageSizeSelector: true,
                                    allowedPageSizes: [5, 10, 20, 50]
                                },
                                filterRow: {
                                    visible: false,
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
                                hoverStateEnabled: true,
                                remoteOperations: true,
                                columns: [
                                    {
                                        dataField: 'id',  caption: 'Id', alignment: 'center',
                                        headerCellTemplate: function (header, info) {
                                            $('<div>')
                                                .html(info.column.caption)
                                                .css('font-size', '11px')
                                                .appendTo(header);
                                        }
                                    },
                                    {
                                        dataField: 'fechaVisita',  caption: 'Fecha Visita', alignment: 'center',
                                        dataType: 'date',
                                        headerCellTemplate: function (header, info) {
                                            $('<div>')
                                                .html(info.column.caption)
                                                .css('font-size', '11px')
                                                .appendTo(header);
                                        }
                                    },
                                    {
                                        dataField: 'radicadoVisita', caption: 'Radicado Inf Visita', alignment: 'center',
                                        headerCellTemplate: function (header, info) {
                                            $('<div>')
                                                .html(info.column.caption)
                                                .css('font-size', '11px')
                                                .appendTo(header);
                                        }
                                    },
                                    {
                                        dataField: 'anioRadicadoVisita', caption: 'Año rad visita', visible: false, alignment: 'center',
                                        headerCellTemplate: function (header, info) {
                                            $('<div>')
                                                .html(info.column.caption)
                                                .css('font-size', '11px')
                                                .appendTo(header);
                                        }
                                    },
                                    {
                                        dataField: 'fechaRadicadoVisita', caption: 'Fecha Radicado Visita', alignment: 'center',
                                        dataType: 'date',
                                        headerCellTemplate: function (header, info) {
                                            $('<div>')
                                                .html(info.column.caption)
                                                .css('font-size', '11px')
                                                .appendTo(header);
                                        }
                                    },
                                    {
                                        dataField: 'talaEjecutada', caption: 'Tala ejecutada (Individuos)',
                                        headerCellTemplate: function (header, info) {
                                            $('<div>')
                                                .html(info.column.caption)
                                                .css('font-size', '11px')
                                                .appendTo(header);
                                        }
                                    },
                                    {
                                        dataField: 'dAPMen10Ejecutada', caption: 'DAP < 10 ejecutada (Individuos)',
                                        headerCellTemplate: function (header, info) {
                                            $('<div>')
                                                .html(info.column.caption)
                                                .css('font-size', '11px')
                                                .appendTo(header);
                                        }
                                    },
                                    {
                                        dataField: 'volumenEjecutado', caption: 'Volumen ejecutado  (M3)',
                                        headerCellTemplate: function (header, info) {
                                            $('<div>')
                                                .html(info.column.caption)
                                                .css('font-size', '11px')
                                                .appendTo(header);
                                        }
                                    },
                                    {
                                        dataField: 'transplanteEjecutado',caption: 'Trasplante ejecutado (Individuos)',
                                        headerCellTemplate: function (header, info) {
                                            $('<div>')
                                                .html(info.column.caption)
                                                .css('font-size', '11px')
                                                .appendTo(header);
                                        }
                                    },
                                    {
                                        dataField: 'podaEjecutada',  caption: 'Poda ejecutada (Individuos)',
                                        headerCellTemplate: function (header, info) {
                                            $('<div>')
                                                .html(info.column.caption)
                                                .css('font-size', '11px')
                                                .appendTo(header);
                                        }
                                    },
                                    {
                                        dataField: 'conservacionEjecutada', caption: 'Conservación ejecutado (Individuos)',
                                        headerCellTemplate: function (header, info) {
                                            $('<div>')
                                                .html(info.column.caption)
                                                .css('font-size', '11px')
                                                .appendTo(header);
                                        }
                                    },
                                    {
                                        dataField: 'reposicionEjecutada', caption: 'Reposición ejecutada (Individuos)',
                                        headerCellTemplate: function (header, info) {
                                            $('<div>')
                                                .html(info.column.caption)
                                                .css('font-size', '11px')
                                                .appendTo(header);
                                        }
                                    },
                                    {
                                        dataField: 'medidaAdicionalEjecutada', caption: 'Medida Adicional ejecutada (Unidades)',
                                        headerCellTemplate: function (header, info) {
                                            $('<div>')
                                                .html(info.column.caption)
                                                .css('font-size', '11px')
                                                .appendTo(header);
                                        }
                                    },
                                    {
                                        width: 80,
                                        alignment: 'center',
                                        caption: "Editar",
                                        headerCellTemplate: function (header, info) {
                                            $('<div>')
                                                .html(info.column.caption)
                                                .css('font-size', '11px')
                                                .appendTo(header);
                                        },
                                        cellTemplate: function (container, options) {
                                            $('<div/>').dxButton({
                                                visible: canEdit,
                                                icon: 'edit',
                                                height: 20,
                                                caption: 'Editar',
                                                hint: 'Editar la información del control',
                                                onClick: function (e) {
                                                    var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/ReposicionesApi/ObtenerRegistroControl";
                                                    idControl = options.data.id;
                                                    $.getJSON(_Ruta,
                                                        {
                                                            Id: options.data.id
                                                        }).done(function (data) {
                                                            if (data !== null) {
                                                                txtFechaControl.option("value", data.fechaControl);
                                                                txtTalaEje.option("value", data.talaEjecutada);
                                                                txtdAPMen10Ejecutada.option("value", data.dAPMen10Ejecutada);
                                                                txtvolumenEjecutado.option("value", data.volumenEjecutado);
                                                                txttransplanteEjecutado.option("value", data.transplanteEjecutado);
                                                                txtreposicionEjecutada.option("value", data.reposicionEjecutada);
                                                                txtpodaEjecutada.option("value", data.podaEjecutada);
                                                                txtconservacionEjecutada.option("value", data.conservacionEjecutada);
                                                                txtFechaControl.option("value", data.fechaControl);
                                                                txtMedidaAdicionalEjecutado.option("value", data.medidaAdicionalEjecutada);
                                                                cboTecnico.option("value", data.tecnicoId);
                                                                txtObservacionesVisita.option("value", data.observacionVisita);
                                                                txtFechaVisita.option("value", data.fechaVisita);
                                                                txtRadicadoVisita.option("value", data.radicadoVisita);
                                                                txtFechaRadicadoVisita.option("value", data.fechaRadicadoVisita);
                                                                popupNewControl.show();
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
                                        visible: canDelete,
                                        caption: "Eliminar",
                                        headerCellTemplate: function (header, info) {
                                            $('<div>')
                                                .html(info.column.caption)
                                                .css('font-size', '11px')
                                                .appendTo(header);
                                        },
                                        alignment: 'center',
                                        cellTemplate: function (container, options) {
                                            $('<div/>').dxButton({
                                                icon: 'remove',
                                                height: 20,
                                                hint: 'Eliminar el Control',
                                                onClick: function (e) {
                                                    var result = DevExpress.ui.dialog.confirm('Desea eliminar el registro seleccionado?', 'Confirmación');
                                                    result.done(function (dialogResult) {
                                                        if (dialogResult) {

                                                            var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/ReposicionesApi/EliminarControl";
                                                            $.getJSON(_Ruta,
                                                                {
                                                                    id: options.data.id
                                                                }).done(function (data) {
                                                                    if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar registro seleccionado');
                                                                    else {
                                                                        $('#GridListadoControl').dxDataGrid({ dataSource: ControlDataSource });
                                                                    }
                                                                }).fail(function (jqxhr, textStatus, error) {
                                                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar registro seleccionado');
                                                                });
                                                        }
                                                    });
                                                }
                                            }).appendTo(container);
                                        }
                                    },
                                    {
                                        width: '100px',
                                        caption: "Ver",
                                        alignment: 'center',
                                        cellTemplate: function (container, options) {
                                            $('<div/>').dxButton({
                                                icon: 'event',
                                                height: 20,
                                                hint: 'Ver documento',
                                                onClick: function (e) {

                                                    let radi = options.row.data.radicadoVisita;
                                                    let aniorad = options.row.data.anioRadicadoVisita;

                                                    var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/ReposicionesApi/ObtenerInformeTecnico";

                                                    $.getJSON(_Ruta,
                                                        {
                                                            radicado: radi,
                                                            anioradicado: aniorad
                                                        }).done(function (data) {
                                                            if (data != null && data.idTramite !== 0 && data.idDocumento !== 0) {
                                                                var CodTramite = data.idTramite;
                                                                var CodDocumento = data.idDocumento;
                                                                var _popup = $("#popDocumento").dxPopup("instance");
                                                                _popup.show();

                                                                var ruta = $('#SIM').data('url') + 'ControlVigilancia/Reposiciones/LeeDoc?CodTramite=' + CodTramite + '&CodDocumento=' + CodDocumento;
                                                                $("#DocumentoAdjunto").attr("src", ruta);
                                                            } else {
                                                                alert('No se encuentra seleccionado el documento!');
                                                            }
                                                        }).fail(function (jqxhr, textStatus, error) {
                                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Evento no esperado!');
                                                        });



                                                    //var CodTramite = options.row.data.TramiteId;
                                                    //var CodDocumento = options.row.data.Id;
                                                    //var _popup = $("#popDocumento").dxPopup("instance");
                                                    //_popup.show();
                                                    //var ruta = $('#SIM').data('url') + 'ControlVigilancia/Reposiciones/LeeDoc?CodTramite=' + CodTramite + '&CodDocumento=' + CodDocumento;
                                                    //$("#DocumentoAdjunto").attr("src", ruta);
                                                }
                                            }).appendTo(container);
                                        }
                                    },
                                ],
                                onSelectionChanged: function (selectedItems) {
                                    var data = selectedItems.selectedRowsData[0];
                                    if (data) {
                                        IdDetReposicion = data.id;
                                    }
                                }
                            });

                            $('#PupupActuaciones').css('visibility', 'visible');
                            var popupA = $("#PupupActuaciones").dxPopup({
                                width: 1700,
                                height: 600,
                                fullScreen: true,
                                hoverStateEnabled: false,
                                title: "Seguimientos de control relacionados con el asunto (CM)"
                            }).dxPopup("instance");

                            popupA.show();
                           
                        }
                    }).appendTo(container);
                }
            },
            {
                visible: canRead,
                width: '5%',
                caption: "Documentos relacionados",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'event',
                        height: 20,
                        hint: 'Documentos',
                        onClick: function (e) {
                            IdReposicion = options.data.ID;
                             $("#GidListadoActos").dxDataGrid({
                                dataSource: ActosDataSource,
                                allowColumnResizing: true,
                                loadPanel: { enabled: true, text: 'Cargando Datos...' },
                                noDataText: "Sin datos para mostrar",
                                showBorders: true,
                                filterRow: {
                                    visible: true,
                                    emptyPanelText: 'Arrastre una columna para agrupar'
                                },
                                searchPanel: {
                                    visible: false,
                                    width: 240,
                                    placeholder: "Buscar..."
                                },
                                selection: {
                                    mode: 'single'
                                 },
                                paging: false,
                                hoverStateEnabled: true,
                                remoteOperations: false,
                                columns: [
                                    { dataField: 'TramiteId', width: '5%', caption: 'Trámite SIM', dataType: 'string' },
                                    { dataField: 'TipoSolicitud', width: '25%', caption: 'Tipo', dataType: 'string' },
                                    { dataField: 'Radicado', width: '5%', caption: 'Radicado', dataType: 'string' },
                                    { dataField: 'Anio', width: '10%', caption: 'Fecha radicado', dataType: 'string' },
                                    { dataField: 'SubTipoSolicitud', width: '35%', caption: 'Descripción', dataType: 'string' },
                                    {
                                        width: '100px',
                                        caption: "Ver",
                                        alignment: 'center',
                                        cellTemplate: function (container, options) {
                                            $('<div/>').dxButton({
                                                icon: 'event',
                                                height: 20,
                                                hint: 'Ver documento',
                                                onClick: function (e) {
                                                    var CodTramite = options.row.data.TramiteId;
                                                    var CodDocumento = options.row.data.Id;
                                                    var _popup = $("#popDocumento").dxPopup("instance");
                                                    _popup.show();
                                                    var ruta = $('#SIM').data('url') + 'ControlVigilancia/Reposiciones/LeeDoc?CodTramite=' + CodTramite + '&CodDocumento=' + CodDocumento;
                                                    $("#DocumentoAdjunto").attr("src", ruta);
                                                }
                                            }).appendTo(container);
                                        }
                                    },
                                ],
                             });
                             var popupD = $("#PopupDocumentos").dxPopup({
                                 width: 1500,
                                 height: 900,
                                 hoverStateEnabled: true,
                                 dragEnabled: true,
                                 fullScreen: true,
                                 resizeEnabled: true,
                                 title: "Documentos asociados"
                            }).dxPopup("instance");

                            popupD.show();
                        }
                    }).appendTo(container);
                }
            }
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                IdReposicion = data.ID;
                idTramite = data.codigoTramite;
                idDocumento = data.codigoDocumento;
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
    
    var txtCM = $("#txtCM").dxTextBox({
        placeholder: "Ingrese el CM...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El CM es requerido!"
        }] 
    }).dxTextBox("instance");

    // Controles asociados al listado de Reposiciones
    var txtAsunto = $("#txtAsunto").dxTextArea({
        value: "",
        readOnly: true,
        height: 60
    }).dxTextArea("instance");
   
    var txtProyecto = $("#txtProyecto").dxTextArea({
        value: "",
        readOnly: true,
        height: 40
    }).dxTextArea("instance");

    var txtNombreProyectoA = $("#txtNombreProyectoA").dxTextArea({
        value: "",
        readOnly: false,
        height: 40
    }).dxTextArea("instance");

    var txtCodTramite = $("#txtCodTramite").dxTextBox({
        value: "",
        readOnly: false,
    }).dxTextBox("instance");
  
    var cboTipoMedidaAdicional = $("#cboTipoMedidaAdicional").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "ControlVigilancia/api/ReposicionesApi/GetTiposMedidaAdicional");
                }
            })
        }),
        displayExpr: "Nombre",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El Tipo de Medida Adicional es obligatoria!"
        }]
    }).dxSelectBox("instance");

    var cboTecnico = $("#cboTecnico").dxSelectBox({
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
            message: "El Técnico es Obligatorio!"
        }]
    }).dxSelectBox("instance");
  
    var txtTalaAut = $("#txtTalaAut").dxNumberBox({
        value: "",
        format: "#,##0",

    }).dxNumberBox("instance");
    var txtTalaEje = $("#txtTalaEje").dxNumberBox({
        value: "",
        format: "#,##0",
    }).dxNumberBox("instance");

    var txtdAPMen10Ejecutada = $("#txtdAPMen10Ejecutada").dxNumberBox({
        value: "",
        format: "#,##0",
    }).dxNumberBox("instance");
    var txttransplanteEjecutado = $("#txttransplanteEjecutado").dxNumberBox({
        value: "",
        format: "#,##0",
    }).dxNumberBox("instance");
    var txtpodaEjecutada = $("#txtpodaEjecutada").dxNumberBox({
        value: "",
        format: "#,##0",
    }).dxNumberBox("instance");
    var txtconservacionEjecutada = $("#txtconservacionEjecutada").dxNumberBox({
        value: "",
        format: "#,##0",
    }).dxNumberBox("instance");
    var txtreposicionEjecutada = $("#txtreposicionEjecutada").dxNumberBox({
        value: "",
        format: "#,##0",
    }).dxNumberBox("instance");
    var txtMedidaAdicionalEjecutado = $("#txtMedidaAdicionalEjecutado").dxNumberBox({
        value: "",
        format: "#,##0",
    }).dxNumberBox("instance");
    var txtvolumenEjecutado = $("#txtvolumenEjecutado").dxNumberBox({
        value: "",
        format: "#.#######0",
    }).dxNumberBox("instance");
    var txtFechaControl = $("#txtFechaControl").dxDateBox({
        type: "datetime",
        value: new Date(),
        readOnly: true,
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
            message: "La fecha del control es obligatoria"
        }]
    }).dxDateBox("instance");
    var txtdAPMen10Autorizada = $("#txtdAPMen10Autorizada").dxNumberBox({
        value: "",
        format: "#,##0",
   }).dxNumberBox("instance");
    var txtvolumenAutorizado = $("#txtvolumenAutorizado").dxNumberBox({
        value: "",
        format: "#.#######0",
    }).dxNumberBox("instance");
    var txttransplanteAutorizado = $("#txttransplanteAutorizado").dxNumberBox({
        value: "",
        format: "#,##0",
    }).dxNumberBox("instance");
    var txtpodaAutorizada = $("#txtpodaAutorizada").dxNumberBox({
        value: "",
        format: "#,##0",
    }).dxNumberBox("instance");
    var txtconservacionAutorizada = $("#txtconservacionAutorizada").dxNumberBox({
        value: "",
        format: "#,##0",
    }).dxNumberBox("instance");
    var txtreposicionAutorizada = $("#txtreposicionAutorizada").dxNumberBox({
        value: "",
        format: "#,##0",
    }).dxNumberBox("instance");
    var txtAutorizado = $("#txtAutorizado").dxNumberBox({
        value: "",
        format: "#,##0",
    }).dxNumberBox("instance");
    var txtObservaciones = $("#txtObservaciones").dxTextArea({
        value: "",
        height: 90
    }).dxTextArea("instance");
    var txtCMv = $("#txtCMv").dxTextBox({
        value: "",
        readOnly: true
    }).dxTextBox("instance");
    var txtNombreProyecto = $("#txtNombreProyecto").dxTextBox({
        value: "",
        readOnly: true
    }).dxTextBox("instance");

    var txtRadicadoDocu = $("#txtRadicadoDocu").dxTextBox({
        value: "",
        format: "######",
        readOnly: false
    }).dxTextBox("instance");

    var txtAnioDocu = $("#txtAnioDocu").dxNumberBox({
        value: "",
        readOnly: false
    }).dxNumberBox("instance");

    var txtDireccionProyecto = $("#txtDireccionProyecto").dxTextBox({
        value: "",
        readOnly: true
    }).dxTextBox("instance");

    var txtCoordenadaX = $("#txtCoordenadaX").dxNumberBox({
        format :'#,##0.###### Coordenada X',
        value: -75.000000,
        min: -76,
        max:-75
    }).dxNumberBox("instance");

    var txtCoordenadaY = $("#txtCoordenadaY").dxNumberBox({
        format: '#,##0.###### Coordenada Y',
        value: 6.000000,
        min: 6,
        max: 7,
    }).dxNumberBox("instance");

    const dataini = [{
        id: 0,
        nombre: '...'
    }]
    const dt = new DevExpress.data.ArrayStore({
        data: dataini,
        key: "id"
    });

    var cboAsuntos = $("#cboAsuntos").dxSelectBox({
        dataSource: dt,
        displayExpr: "Descripcion",
        valueExpr: 'Id',
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe seleccionar el Asunto!"
        }]
    }).dxSelectBox("instance");


    var txtObservacionesVisita = $("#txtObservacionesVisita").dxTextBox({
        value: "",
        readOnly: false
    }).dxTextBox("instance");

    var txtFechaVisita = $("#txtFechaVisita").dxDateBox({
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
            message: "La fecha de la visita es obligatorio"
        }]
    }).dxDateBox("instance");

    var txtRadicadoVisita = $("#txtRadicadoVisita").dxTextBox({
        placeholder: "Radicado del Informe Técnico",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El Radicado del Informe Técnico es requerido!"
        }]
    }).dxTextBox("instance");

  

    var txtFechaRadicadoVisita = $("#txtFechaRadicadoVisita").dxDateBox({
        type: "date",
        displayFormat: "MM/dd/yyyy",
        value: new Date()
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "La fecha del radicado del Informe Técnico es obligatorio!"
        }]
    }).dxDateBox("instance");



    $("#btnVerInformeTecnico").dxButton({
        icon: 'exportpdf',
        hint: 'Ver documento ...',
        type: "default",
        height: 35,
        width: 60,
        onClick: function () {

            let radi = txtRadicadoVisita.option("value");
            let fecharad = new Date(txtFechaRadicadoVisita.option("value")); 
            let aniorad = fecharad.getFullYear();

            var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/ReposicionesApi/ObtenerInformeTecnico";

            $.getJSON(_Ruta, 
                {
                    radicado: radi,
                    anioradicado: aniorad
                }).done(function (data) {
                    if (data != null && data.idTramite !== 0 && data.idDocumento !== 0) {
                        var CodTramite = data.idTramite;
                        var CodDocumento = data.idDocumento;
                        var _popup = $("#popDocumento").dxPopup("instance");
                        _popup.show();

                        var ruta = $('#SIM').data('url') + 'ControlVigilancia/Reposiciones/LeeDoc?CodTramite=' + CodTramite + '&CodDocumento=' + CodDocumento;
                        $("#DocumentoAdjunto").attr("src", ruta);
                    } else {
                        alert('No se encuentra seleccionado el documento!');
                    }
                }).fail(function (jqxhr, textStatus, error) {
                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Evento no esperado!');
                });
        }
    }).dxButton("instance");

    const loadIndicator = $('#PopupLoadNuevoControl').dxLoadIndicator({
        height: 40,
        width: 40,
        visible: false
    }).dxLoadIndicator("instance");
   
    $("#cmdCM").dxButton({
        text: "Verificar",
        type: "danger",
        height: 35,
        onClick: function () {
          
            var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/ReposicionesApi/ObtenerCM";
            $.getJSON(_Ruta,
            {
                cm: txtCM.option('value')
            }).done(function (data) {
                txtCMv.option("value", txtCM.option('value'));
                txtNombreProyecto.option("value", data.nombre);
                txtProyecto.option("value", data.nombre + ' ' + data.direccion);
                $.getJSON(_Ruta,
                    {
                        cm: txtCM.option('value')
                    }).done(function (data) {
                        txtCMv.option("value", txtCM.option('value'));
                        txtNombreProyecto.option("value", data.nombre);
                        txtDireccionProyecto.option("value", data.direccion);

                        $("#btnBuscarActosCMs").dxButton({
                            text: "Buscar ...",
                            type: "default",
                            height: 30,
                            onClick: function () {
                                $("#btnBuscarActosCMs").dxButton("instance").option("icon", "clock");
                                $("#btnBuscarActosCMs").dxButton("instance").option("disabled", true);
                                loadIndicator.option("visible", true);
                                var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/ReposicionesApi/ObtenerActuacionesCM";
                                $.getJSON(_Ruta,
                                    {
                                        radicado: txtRadicadoDocu.option('value'), anio: txtAnioDocu.option('value'), cm: txtCM.option('value')
                                    }).done(function (data) {
                                        $("#btnBuscarActosCMs").dxButton("instance").option("disabled", false);
                                        $("#btnBuscarActosCMs").dxButton("instance").option("icon", "");
                                        loadIndicator.option("visible", false);
                                        txtCMv.option("value", txtCM.option('value'));
                                        txtNombreProyecto.option("value", data.nombre);
                                        txtProyecto.option("value", data.nombre + ' ' + data.direccion);
                                        $("#cboAsuntos").dxSelectBox({
                                            dataSource: new DevExpress.data.ArrayStore({
                                                data: data.asuntos,
                                                key: "Id"
                                            }),
                                            displayExpr: "Descripcion",
                                            valueExpr: 'Id',
                                            searchEnabled: true
                                        }).dxValidator({
                                            validationGroup: "ProcesoGroup",
                                            validationRules: [{
                                                type: "required",
                                                message: "Debe seleccionar el Asunto!"
                                            }]
                                        }).dxSelectBox("instance");
                                        pupupCM.show();
                                    }).fail(function (jqxhr, textStatus, error) {

                                        $("#btnBuscarActosCMs").dxButton("instance").option("disabled", false);
                                        $("#btnBuscarActosCMs").dxButton("instance").option("icon", "");
                                        loadIndicator.option("visible", false);
                                        DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                                    });
                            }
                        });
                      
                        pupupCM.show();
                    }).fail(function (jqxhr, textStatus, error) {
                        DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                    });

                pupupCM.show();
            }).fail(function (jqxhr, textStatus, error) {
                DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
            });


            
        }
    });
   
    $("#btnGuarda").dxButton({
        text: "Guardar",
        type: "default",
        height: 30,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var id = IdRegistro;
            var cm = txtCM.option("value");
            var asunto = txtAsunto.option("value");
            var proyecto = txtProyecto.option("value");
            var nombreProyecto = txtNombreProyectoA.option("value");
            var codTramite = txtCodTramite.option("value");
            var codigoSolicitud = idAsunto;
            var codigoActoAdministrativo = 0;
            var talaAutorizada = txtTalaAut.option("value");
            var dAPMen10Autorizada = txtdAPMen10Autorizada.option("value");
            var volumenAutorizado = txtvolumenAutorizado.option("value");
            var transplanteAutorizado = txttransplanteAutorizado.option("value");
            var podaAutorizada = txtpodaAutorizada.option("value");
            var conservacionAutorizada = txtconservacionAutorizada.option("value");
            var reposicionAutorizada = txtreposicionAutorizada.option("value");
            var autorizado = txtAutorizado.option("value");
            var observaciones = txtObservaciones.option("value");
            var coordenadaX = txtCoordenadaX.option("value");
            var coordenadaY = txtCoordenadaY.option("value");

            var numx = parseFloat(coordenadaX);

            if (coordenadaX !== "" && isNaN(numx)) {
                alert("La coordenada X no tiene un formato de entrada válido!");
                return;
            }
            var numy = parseFloat(coordenadaY);
            if (coordenadaY !== "" && isNaN(numy)) {
                alert("La coordenada Y no tiene un formato de entrada válido!");
                return;
            }


            if (cboTipoMedidaAdicional.option("value").Id !== undefined) {
                tipomedida = cboTipoMedidaAdicional.option("value").Id;
            }
            var params = {
                id: id, cm: cm, asunto: asunto, proyecto: proyecto, codigoSolicitud: codigoSolicitud, codigoActoAdministrativo: codigoActoAdministrativo,
                talaAutorizada: talaAutorizada, dAPMen10Autorizada: dAPMen10Autorizada, tipoMedidaId: tipomedida,
                volumenAutorizado: volumenAutorizado, transplanteAutorizado: transplanteAutorizado, 
                podaAutorizada: podaAutorizada, conservacionAutorizada: conservacionAutorizada,
                reposicionAutorizada: reposicionAutorizada, autorizado: autorizado,
                observaciones: observaciones, coordenadaX: numx, coordenadaY: numy, nombreProyecto: nombreProyecto, codigoTramite: codTramite
            };
            var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/ReposicionesApi/GuardarReposicion";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                beforeSend: function () { },
                success: function (data) {
                    if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Guardar Datos');
                    else {
                        DevExpress.ui.dialog.alert('Datos Guardados correctamente', 'Guardar Datos');
                        $('#GidListado').dxDataGrid({ dataSource: ReposicionesDataSource });
                        $("#PopupNuevaRepo").dxPopup("instance").hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    var btnGuardarNewControl = $("#btnGuardarNewControl").dxButton({
        text: "Guardar",
        type: "default",
        height: 30,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var reposicionId = IdReposicion;
            var tipoDocumentoId = 1;
            var documentoId = idDocActo;
            var numeroActo = 1;
            var fechaActo = txtFechaControl.option("value");;
            var anioActo = txtFechaRadicadoVisita.option("value");
            var talaEjecutada = txtTalaEje.option("value");
            var dapMen10Ejecutada = txtdAPMen10Ejecutada.option("value");
            var volumenEjecutado = txtvolumenEjecutado.option("value");
            var trasplanteEjecutado = txttransplanteEjecutado.option("value");
            var reposicionEjecutada = txtreposicionEjecutada.option("value");
            var podaEjecutada = txtpodaEjecutada.option("value");
            var conservacionEjecutada = txtconservacionEjecutada.option("value");
            var fechaControl = txtFechaControl.option("value");
            var fechaRadicadoVisita = txtFechaRadicadoVisita.option("value");
            var medidaAdicionalEjecutada = txtMedidaAdicionalEjecutado.option("value");
            var observacionVisita = txtObservacionesVisita.option("value");
            var fechaVisita = txtFechaVisita.option("value");
            var tecnicoId = cboTecnico.option("value").Id;
            var radicadoVisita = txtRadicadoVisita.option("value");
            var params = {
                id: idControl, reposicionId: reposicionId, tipoDocumento: tipoDocumentoId, documentoId: documentoId,
                numeroActo: numeroActo, fechaActo: fechaActo, anioActo: anioActo, talaEjecutada: talaEjecutada, reposicionEjecutada: reposicionEjecutada,
                dapMen10Ejecutada: dapMen10Ejecutada, volumenEjecutado: volumenEjecutado, transplanteEjecutado: trasplanteEjecutado,
                podaEjecutada: podaEjecutada, conservacionEjecutada: conservacionEjecutada, medidaAdicionalEjecutada: medidaAdicionalEjecutada,
                fechaControl: fechaControl, observacionVisita: observacionVisita, fechaVisita: fechaVisita, tecnicoId: tecnicoId, radicadoVisita: radicadoVisita, fechaRadicadoVisita: fechaRadicadoVisita
            };
            var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/ReposicionesApi/GuardarControl";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                beforeSend: function () { },
                success: function (data) {
                    if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Guardar Datos');
                    else {
                        DevExpress.ui.dialog.alert('Datos Guardados correctamente', 'Guardar Datos');
                        $('#GridListadoControl').dxDataGrid({ dataSource: ControlDataSource });
                        popupNewControl.hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    }).dxButton("instance");
   
    var popupNewControl = $("#PopupNuevoControl").dxPopup({
        width: 1200,
        height: "auto",
        dragEnabled: true,
        resizeEnabled: true,
        hoverStateEnabled: true,
        title: "Registro de Control y Vigilancia"
    }).dxPopup("instance");

    $("#btnBuscarActosCMs").dxButton({
        text: "Buscar ...",
        type: "default",
        height: 30,
        onClick: function () {
            var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/ReposicionesApi/ObtenerActuacionesCM";
            $.getJSON(_Ruta,
                {
                    radicado: txtRadicadoDocu.option('value'), anio: txtAnioDocu.option('value'), cm: txtCM.option('value')
                }).done(function (data) {
                    txtCMv.option("value", txtCM.option('value'));
                    txtNombreProyecto.option("value", data.nombre);
                    txtProyecto.option("value", data.nombre + ' ' + data.direccion);
                    
                    pupupCM.show();
                }).fail(function (jqxhr, textStatus, error) {
                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                });
        }
    }).dxButton("instance");

    $("#btnVerActoPpal").dxButton({
        icon: 'exportpdf',
        hint: 'Ver documento ...',
        type: "default",
        height: 35,
        width: 60,
        onClick: function () {

            var asunto = txtAsunto.option("value");
            var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/ReposicionesApi/ObtenerTramiteDocumento";
            $.getJSON(_Ruta,
                {
                    id: asunto
                }).done(function (data) {
                    if (data != null && data.idTramite !== 0 && data.idDocumento !== 0) {
                        var CodTramite = data.idTramite;
                        var CodDocumento = data.idDocumento;
                        var _popup = $("#popDocumento").dxPopup("instance");
                        _popup.show();

                        var ruta = $('#SIM').data('url') + 'ControlVigilancia/Reposiciones/LeeDoc?CodTramite=' + CodTramite + '&CodDocumento=' + CodDocumento;
                        $("#DocumentoAdjunto").attr("src", ruta);
                    } else {
                        alert('No se encuentra seleccionado el documento!');
                    }
                }).fail(function (jqxhr, textStatus, error) {
                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                });
        }
    });

    var popup = $("#PopupNuevaRepo").dxPopup({
        width: 1300,
        height: "auto",
        dragEnabled: true,
        resizeEnabled: true,
        hoverStateEnabled: true,
        title: "Permiso y CM asociado al proceso de Control y Vigilancia"
    }).dxPopup("instance");

    $("#btnNuevo").dxButton({
        stylingMode: "contained",
        text: "Nuevo",
        type: "success",
        width: 200,
        height: 30,
        icon: 'add',
        onClick: function () {
            IdRegistro = -1;
            txtCM.reset();
            txtAsunto.reset();
            txtTalaAut.reset();
            txtdAPMen10Autorizada.reset();
            txtvolumenAutorizado.reset();
            txttransplanteAutorizado.reset();
            txtpodaAutorizada.reset();
            txtconservacionAutorizada.reset();
            txtreposicionAutorizada.reset();
            txtObservaciones.reset();
            txtAutorizado.reset();
            txtCoordenadaX.reset();
            txtCoordenadaY.reset();
            txtNombreProyectoA.reset();
            txtCodTramite.reset();
            popup.show();
        }
    });

    $("#btnAsignar").dxButton({
        text: "Asignar",
        type: "default",
        height: 30,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            txtProyecto.option("value", txtNombreProyecto.option("value"));
            txtAsunto.option("value", cboAsuntos._options.text);
            idAsunto = cboAsuntos._options.value;
            $("#PupupCM").dxPopup("instance").hide();
        }
    });
  
    var btnnewControl = $("#btnnewControl").dxButton({
        visible: canInsert,
        text: "Nuevo  Seguimiento",
        type: "success",
        height: 30,
        width: 200,
        onClick: function () {
            IdRegistro = -1;
            idControl = 0;
            txtTalaEje.reset();
            txtdAPMen10Ejecutada.reset();
            txttransplanteEjecutado.reset();
            txtpodaEjecutada.reset();
            txtconservacionEjecutada.reset();
            txtreposicionEjecutada.reset();
            txtMedidaAdicionalEjecutado.reset();
            txtvolumenEjecutado.reset();
            txtObservacionesVisita.reset();
            cboTecnico.reset();
            txtRadicadoVisita.reset();
            popupNewControl.show();
        }
    }).dxButton("instance");
   
    var pupupCM = $("#PupupCM").dxPopup({
        width: 1300,
        height: "auto",
        hoverStateEnabled: true,
        title: "Información del CM"
    }).dxPopup("instance");


    $("#btnReporte").dxButton({
        text: "Reporte",
        stylingMode: "contained",
        type: "default",
        height: 30,
        onClick: function () {
            popupReporte.show();
            let url = $('#SIM').data('url') + 'ControlVigilancia/Reposiciones/GetReporte';
            $('#iframeReporte').attr('src', url);
        }
    });


    var popupReporte = $("#popupReporte").dxPopup({
        width: 1200,
        height: 600,
        hoverStateEnabled: true,
        title: "Reporte"
    }).dxPopup("instance");

});


//Data Stores
var ReposicionesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"ID","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'ControlVigilancia/api/ReposicionesApi/GetReposiciones', {
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

var ActosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"Id","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);

        $.getJSON($('#SIM').data('url') + 'ControlVigilancia/api/ReposicionesApi/ObtenerActos', {
            Id: IdReposicion,
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
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var ControlDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        $.getJSON($('#SIM').data('url') + 'ControlVigilancia/api/ReposicionesApi/ObtenerControles', {
            Id: IdReposicion
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

