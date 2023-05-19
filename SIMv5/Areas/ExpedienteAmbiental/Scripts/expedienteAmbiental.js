var idExpediente;
var codExpediente = 0;
var idExpedienteAmb = 0;
var idTercero;
var idInstalacion;
var idPuntoControl = 0;
var codigoSolicitudId = 0;
var idExpedienteDoc;
var idEstadoPuntoControl = 0;
var idAnotacionPuntoControl = 0;
var NomExpediente = "";
var IdIUnidadDoc = 0;
var IdTomo = 0;
var editar = false;
var cm = "";
var direccion = "";
var municipio = "";
var nombrePunto = "";

$(document).ready(function () {

    $('#asistente').accordion({
        collapsible: true,
        animationDuration: 500,
    });
    $('#divEncabezado').css('visibility', 'hidden');
    $('#asistente').css('visibility', 'hidden');

    $("#GridListado").dxDataGrid({
        dataSource: ExpedientesDataSource,
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
        hoverStateEnabled: true,
        remoteOperations: true,
        columns: [
            { dataField: 'idExpediente', width: '5%', caption: 'Id', alignment: 'center' },
            { dataField: 'proyectoId', width: '5%', caption: 'Id', alignment: 'center', visible:false },
            { dataField: 'nombre', width: '20%', caption: 'Nombre', alignment: 'center' },
            { dataField: 'cm', width: '5%', caption: 'CM', alignment: 'center' },
            { dataField: 'descripcion', width: '25%', caption: 'Descripción', alignment: 'center' },
            { dataField: 'municipio', width: '5%', caption: 'Municipio', alignment: 'center' },
            { dataField: 'direccion', width: '15%', caption: 'Dirección', alignment: 'center' },
            { dataField: 'clasificacionExpediente', width: '15%', caption: 'Clasificación', alignment: 'center' },
            {
                visible: canEdit,
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        caption: "Editar",
                        height: 20,
                        hint: 'Editar la información relacionada con el Expediente Ambiental seleccionado',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "ExpedienteAmbiental/api/ExpedientesAmbApi/ObtenerExpedienteAsync";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.idExpediente
                                }).done(function (data) {
                                    if (data !== null) {
                                        idExpedienteAmb = data.idExpediente;
                                        txtCM.option("value", data.cm);
                                        txtNombreExpediente.option("value", data.nombre);
                                        txtDescripcionExpediente.option("value", data.descripcion);
                                        txtDireccionExpediente.option("value", data.direccion);
                                        cboClasificacion.option("value", data.clasificacionExpedienteId);
                                        cboMunicipio.option("value", data.municipioId);
                                        popupNuevoExpediente.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Evento no esperado!');
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
                        hint: 'Eliminar el Expediente Ambiental',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar el registro seleccionado?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "ExpedienteAmbiental/api/ExpedientesAmbApi/EliminarExpedienteAsync?Id=" + options.data.idExpediente; 
                                    $.ajax({
                                        type: 'POST',
                                        url: _Ruta,
                                        contentType: "application/json",
                                        dataType: 'text',
                                        success: function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar registro seleccionado');
                                            else {
                                                $('#GidListado').dxDataGrid({ dataSource: ExpedientesDataSource });
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
                visible: canRead,
                width: '5%',
                caption: "Trámites asociados",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'columnfield',
                        height: 20,
                        hint: 'Trámites asociados al Expediente',
                        onClick: function (e) {
                            txtlblCM2.option("value", options.data.cm);
                            codExpediente = options.data.proyectoId;

                            var _Ruta = $('#SIM').data('url') + "ExpedienteAmbiental/api/ExpedientesAmbApi/ObtenerDatosTerceroExpedienteAsync";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.idExpediente
                                }).done(function (datat) {
                                    if (datat !== null) {
                                        $("#GridListadoTramitesExpediente").dxDataGrid({
                                            dataSource: new DevExpress.data.DataSource({
                                                store: new DevExpress.data.CustomStore({
                                                    key: "CODTRAMITE",
                                                    loadMode: "raw",
                                                    load: function () {
                                                        return $.getJSON($("#SIM").data("url") + "ExpedienteAmbiental/api/ExpedientesAmbApi/GetTramitesExpedienteAsync", { codExpediente: codExpediente });
                                                    }
                                                })
                                            }),
                                            allowColumnResizing: true,
                                            loadPanel: { enabled: true, text: 'Cargando Datos...' },
                                            noDataText: "Sin datos para mostrar",
                                            showBorders: true,
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
                                            paging: {
                                                pageSize: 5
                                            },
                                            pager: {
                                                showPageSizeSelector: true,
                                                allowedPageSizes: [5, 10, 20, 50]
                                            },
                                            hoverStateEnabled: true,
                                            remoteOperations: true,
                                            columns: [
                                                { dataField: 'CODTRAMITE', width: '5%', caption: 'Código Trámite', alignment: 'center' },
                                                { dataField: 'PROYECTO', width: '15%', caption: 'Proyecto', alignment: 'center' },
                                                { dataField: 'FECHAINI', width: '5%', caption: 'Fecha Inicial', alignment: 'center', dataType:'date' },
                                                { dataField: 'FECHAFIN', width: '5%', caption: 'Fecha Final', alignment: 'center', dataType: 'date' },
                                                { dataField: 'COMENTARIOS', width: '15%', caption: 'Comentarios', dataType: 'string' },
                                                { dataField: 'MENSAJE', width: '25%', caption: 'Mensaje', dataType: 'string' },
                                                { dataField: 'ESTADO', width: '10%', caption: 'Estado', dataType: 'string' },
                                                {
                                                    width: '12%',
                                                    alignment: 'center',
                                                    cellTemplate: function (container, options) {
                                                        $('<div/>').dxButton({
                                                            icon: 'fields',
                                                            text: options.data.CODTRAMITE,
                                                            hint: 'Detalle trámite',
                                                            onClick: function (e) {
                                                                if (options.data.CODTRAMITE > 0) {
                                                                    var popupOpciones = {
                                                                        height: 600,
                                                                        width: 1100,
                                                                        title: 'Detalle del trámite',
                                                                        visible: false,
                                                                        contentTemplate: function (container) {
                                                                            $("<iframe>").attr("src", $('#SIM').data('url') + 'Utilidades/DetalleTramite?popup=true&CodTramite=' + options.data.CODTRAMITE).attr("width", "100%").attr("height", "100%").attr("frameborder", "0").attr("scrolling", "0").appendTo(container);
                                                                        }
                                                                    }
                                                                    var popupTra = $("#popDetalleTramite").dxPopup(popupOpciones).dxPopup("instance");
                                                                    $("#popDetalleTramite").css({ 'visibility': 'visible' });
                                                                    $("#popDetalleTramite").fadeTo("slow", 1);
                                                                    popupTra.show();
                                                                }
                                                            }
                                                        }).appendTo(container);
                                                    }
                                                },
                                            ],
                                            onSelectionChanged: function (selectedItems) {

                                            }
                                        });
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Evento no esperado!');
                                });

                       

                            popupTramitesExpediente.show();
                        }
                    }).appendTo(container);
                }
            },
            {
                width: '5%',
                caption: "Puntos de Control",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'event',
                        height: 20,
                        hint: 'Puntos de Control',
                        onClick: function (e) {
                            $('#divEncabezado').css('visibility', 'visible');
                            $('#asistente').css('visibility', 'visible');
                            idExpediente = options.data.idExpediente;
                            txtlblNombreExpediente.option("value", options.data.nombre);
                            txtlblCM.option("value", options.data.cm);
                                              

                            $("#GidListadoPuntosControl").dxDataGrid({
                                dataSource: PuntosControlDataSource,
                                allowColumnResizing: true,
                                loadPanel: { enabled: true, text: 'Cargando Datos...' },
                                noDataText: "Sin datos para mostrar",
                                showBorders: true,
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
                                paging: {
                                    pageSize: 5
                                },
                                pager: {
                                    showPageSizeSelector: true,
                                    allowedPageSizes: [5, 10, 20, 50]
                                },
                                hoverStateEnabled: true,
                                remoteOperations: true,
                                columns: [
                                    { dataField: 'idPuntoControl', width: '5%', caption: 'Código', alignment: 'center' },
                                    { dataField: 'codigoSolicitudId', width: '5%', caption: 'CódigoV4', alignment: 'center', visible:false },
                                    { dataField: 'nombre', width: '25%', caption: 'Nombre', dataType: 'string' },
                                    { dataField: 'conexo', width: '9%', caption: 'Conexo', dataType: 'string' },
                                    { dataField: 'tipoSolicitudAmbiental', width: '20%', caption: 'Tipo de Solicitud Ambiental', dataType: 'string' },
                                    { dataField: 'observacion', width: '35%', caption: 'Observación', dataType: 'string' },
                                    { dataField: 'expedienteDocumentalLabel', width: '35%', caption: 'Expediente Documental', dataType: 'string' },
                                    { dataField: 'expedienteDocumentalId', width: '5%', caption: 'IdEdoc', dataType: 'string', visible:false },
                                    {
                                        visible: canEdit,
                                        width: 40,
                                        alignment: 'center',
                                        cellTemplate: function (container, options) {
                                            $('<div/>').dxButton({
                                                icon: 'edit',
                                                caption: "Editar",
                                                height: 20,
                                                hint: 'Editar la información relacionada con el Punto de Control del Expediente Ambiental seleccionado',
                                                onClick: function (e) {
                                                    idPuntoControl = options.data.idPuntoControl;
                                                    codigoSolicitudId = options.data.codigoSolicitudId;
                                                    var _Ruta = $('#SIM').data('url') + "ExpedienteAmbiental/api/ExpedientesAmbApi/ObtenerPuntoConttrolAsync";
                                                    $.getJSON(_Ruta,
                                                        {
                                                            Id: options.data.idPuntoControl
                                                        }).done(function (data) {
                                                            if (data !== null) {
                                                                editar = true;
                                                                txtNombrePuntoControl.option("value", data.nombre);
                                                                txtObservacionPuntoControl.option("value", data.observacion);
                                                                cboTipoSolicitudAmbiental.option("value", data.tipoSolicitudAmbientalId);
                                                                cboTipoSolicitudAmbiental.option("disabled", true);
                                                                txtConexoPunto.option("value", data.conexo);
                                                                txtFechaOrigen.option("value", data.fechaOrigen);

                                                                popupNuevoPuntoControl.show();
                                                            }
                                                        }).fail(function (jqxhr, textStatus, error) {
                                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Evento no esperado!');
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
                                                hint: 'Eliminar el Punto de Control',
                                                onClick: function (e) {
                                                    var result = DevExpress.ui.dialog.confirm('Desea eliminar el registro seleccionado?', 'Confirmación');
                                                    result.done(function (dialogResult) {
                                                        if (dialogResult) {
                                                            var _Ruta = $('#SIM').data('url') + "ExpedienteAmbiental/api/ExpedientesAmbApi/EliminarPuntoControlAsync?Id=" + options.data.idPuntoControl;
                                                            $.ajax({
                                                                type: 'POST',
                                                                url: _Ruta,
                                                                contentType: "application/json",
                                                                dataType: 'text',
                                                                success: function (data) {
                                                                    if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar registro seleccionado');
                                                                    else {
                                                                        $('#GidListadoPuntosControl').dxDataGrid({ dataSource: PuntosControlDataSource });
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
                                        visible: canEdit,
                                        width: '5%',
                                        caption: "Vincular Expediente Documental",
                                        alignment: 'center',
                                        cellTemplate: function (container, options) {
                                            $('<div/>').dxButton({
                                                icon: 'event',
                                                height: 20,
                                                hint: 'Vincular Expediente Documental',
                                                onClick: function (e) {
                                                    popupVicularExpedienteDocumental.show();
                                                    $('#BuscarExp').attr('src', $('#SIM').data('url') + 'Utilidades/BuscarExpediente?popup=true');
                                                }
                                            }).appendTo(container);
                                        }
                                    },
                                    {
                                        visible: canRead,
                                        width: '5%',
                                        caption: "Trámites asociados",
                                        alignment: 'center',
                                        cellTemplate: function (container, options) {
                                            $('<div/>').dxButton({
                                                icon: 'columnfield',
                                                height: 20,
                                                hint: 'Trámites asociados al Punto de Control',
                                                onClick: function (e) {

                                                    popupTramitesPuntoControl.show();
                                                    codigoSolicitudId = options.data.codigoSolicitudId;
                                                      $("#GridListadoTramitesPuntoControl").dxDataGrid({
                                                                    dataSource: new DevExpress.data.DataSource({
                                                                        store: new DevExpress.data.CustomStore({
                                                                            key: "CODTRAMITE",
                                                                            loadMode: "raw",
                                                                            load: function () {
                                                                                return $.getJSON($("#SIM").data("url") + "ExpedienteAmbiental/api/ExpedientesAmbApi/GetTramitesPuntoAsync", { codigoSolicitudId: codigoSolicitudId });
                                                                            }
                                                                        })
                                                                    }),
                                                                    allowColumnResizing: true,
                                                                    loadPanel: { enabled: true, text: 'Cargando Datos...' },
                                                                    noDataText: "Sin datos para mostrar",
                                                                    showBorders: true,
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
                                                                    paging: {
                                                                        pageSize: 5
                                                                    },
                                                                    pager: {
                                                                        showPageSizeSelector: true,
                                                                        allowedPageSizes: [5, 10, 20, 50]
                                                                    },
                                                                    hoverStateEnabled: true,
                                                                    remoteOperations: true,
                                                                    columns: [
                                                                        { dataField: 'CODTRAMITE', width: '5%', caption: 'Código Trámite', alignment: 'center' },
                                                                        { dataField: 'PROYECTO', width: '15%', caption: 'Proyecto', alignment: 'center' },
                                                                        { dataField: 'FECHAINI', width: '5%', caption: 'Fecha Inicial', alignment: 'center', dataType: 'date' },
                                                                        { dataField: 'FECHAFIN', width: '5%', caption: 'Fecha Final', alignment: 'center', dataType: 'date' },
                                                                        { dataField: 'COMENTARIOS', width: '15%', caption: 'Comentarios', dataType: 'string' },
                                                                        { dataField: 'MENSAJE', width: '25%', caption: 'Mensaje', dataType: 'string' },
                                                                        { dataField: 'ESTADO', width: '10%', caption: 'Estado', dataType: 'string' },
                                                                        {
                                                                            width: '12%',
                                                                            alignment: 'center',
                                                                            cellTemplate: function (container, options) {
                                                                                $('<div/>').dxButton({
                                                                                    icon: 'fields',
                                                                                    text: options.data.CODTRAMITE,
                                                                                    hint: 'Detalle trámite',
                                                                                    onClick: function (e) {
                                                                                        if (options.data.CODTRAMITE > 0) {
                                                                                            var popupOpciones = {
                                                                                                height: 600,
                                                                                                width: 1100,
                                                                                                title: 'Detalle del trámite',
                                                                                                visible: false,
                                                                                                contentTemplate: function (container) {
                                                                                                    $("<iframe>").attr("src", $('#SIM').data('url') + 'Utilidades/DetalleTramite?popup=true&CodTramite=' + options.data.CODTRAMITE).attr("width", "100%").attr("height", "100%").attr("frameborder", "0").attr("scrolling", "0").appendTo(container);
                                                                                                }
                                                                                            }
                                                                                            var popupTra = $("#popDetalleTramite").dxPopup(popupOpciones).dxPopup("instance");
                                                                                            $("#popDetalleTramite").css({ 'visibility': 'visible' });
                                                                                            $("#popDetalleTramite").fadeTo("slow", 1);
                                                                                            popupTra.show();
                                                                                        }
                                                                                    }
                                                                                }).appendTo(container);
                                                                            }
                                                                        },
                                                                    ],
                                                                    onSelectionChanged: function (selectedItems) {

                                                                    }
                                                                });
                                                   
                                                }
                                            }).appendTo(container);
                                        }
                                    },
                                ],
                                onSelectionChanged: function (selectedItems) {
                                    var data = selectedItems.selectedRowsData[0];
                                    if (data) {
                                        idPuntoControl = data.idPuntoControl;
                                        codigoSolicitudId = data.codigoSolicitudId;
                                        txtlblNombrePuntoControl.option("value", data.nombre);
                                        txtlblNombrePuntoControlEstado.option("value", data.nombre);
                                        txtlblNombrePuntoControlNotas.option("value", data.nombre);
                                        txtlblNombrePuntoControlTramite.option("value", data.nombre);
                                        if (data.expedienteDocumentalId) {
                                                idExpedienteDoc = data.expedienteDocumentalId;
                                        } else
                                        {
                                           idExpedienteDoc = 0;
                                        }
                                        $("#GidListadoEstadosPuntosControl").dxDataGrid({
                                            dataSource: EstadosPuntoControlDataSource,
                                            allowColumnResizing: true,
                                            loadPanel: { enabled: true, text: 'Cargando Datos...' },
                                            noDataText: "Sin datos para mostrar",
                                            showBorders: true,
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
                                            paging: {
                                                pageSize: 5
                                            },
                                            pager: {
                                                showPageSizeSelector: true,
                                                allowedPageSizes: [5, 10, 20, 50]
                                            },
                                            hoverStateEnabled: true,
                                            remoteOperations: true,
                                            columns: [
                                                { dataField: 'idEstadoPuntoControl', width: '5%', caption: 'Código', alignment: 'center' },
                                                { dataField: 'tipoEstadoPuntoControl', width: '25%', caption: 'Nombre', dataType: 'string' },
                                                { dataField: 'observacion', width: '55%', caption: 'Observación', dataType: 'string' },
                                                {
                                                    visible: canEdit,
                                                    width: 40,
                                                    caption: "Editar",
                                                    alignment: 'center',
                                                    cellTemplate: function (container, options) {
                                                        $('<div/>').dxButton({
                                                            icon: 'edit',
                                                            caption: "Editar",
                                                            height: 20,
                                                            hint: 'Editar la información relacionada con el Estado del Punto de Control del Expediente Ambiental seleccionado',
                                                            onClick: function (e) {
                                                                var _Ruta = $('#SIM').data('url') + "ExpedienteAmbiental/api/ExpedientesAmbApi/ObtenerEstadoPuntoControlAsync";
                                                                idEstadoPuntoControl = options.data.idEstadoPuntoControl;
                                                                $.getJSON(_Ruta,
                                                                    {
                                                                        Id: options.data.idEstadoPuntoControl
                                                                    }).done(function (data) {
                                                                        if (data !== null) {

                                                                            txtObservacionEstado.option("value", data.observacion);
                                                                            cboTipoEstado.option("value", data.tipoEstadoPuntoControlId);
                                                                            popupEstadoPuntoControl.show();
                                                                      
                                                                        }
                                                                    }).fail(function (jqxhr, textStatus, error) {
                                                                        DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Evento no esperado!');
                                                                    });
                                                            }
                                                        }).appendTo(container);
                                                    }
                                                },
                                                {
                                                    visible: canDelete,
                                                    width: 40,
                                                    caption: "Eliminar",
                                                    alignment: 'center',
                                                    cellTemplate: function (container, options) {
                                                        $('<div/>').dxButton({
                                                            icon: 'remove',
                                                            height: 20,
                                                            hint: 'Eliminar el Estado del Punto de Control',
                                                            onClick: function (e) {
                                                                var result = DevExpress.ui.dialog.confirm('Desea eliminar el registro seleccionado?', 'Confirmación');
                                                                result.done(function (dialogResult) {
                                                                    if (dialogResult) {
                                                                        var _Ruta = $('#SIM').data('url') + "ExpedienteAmbiental/api/ExpedientesAmbApi/EliminarEstadoPuntoControlAsync?Id=" + options.data.idEstadoPuntoControl;
                                                                        $.ajax({
                                                                            type: 'POST',
                                                                            url: _Ruta,
                                                                            contentType: "application/json",
                                                                            dataType: 'text',
                                                                            success: function (data) {
                                                                                if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar registro seleccionado');
                                                                                else {
                                                                                    $('#GidListadoEstadosPuntosControl').dxDataGrid({ dataSource: EstadosPuntoControlDataSource });
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
                                            ],
                                            onSelectionChanged: function (selectedItems) {
                                                var data = selectedItems.selectedRowsData[0];
                                                if (data) {
                                                    idEstadoPuntoControl = data.idEstadoPuntoControl;
                                                }
                                            }
                                         });
                                        $("#GidListadoAnotacionesPuntosControl").dxDataGrid({
                                            dataSource: AnotacionesPuntoControlDataSource,
                                            allowColumnResizing: true,
                                            loadPanel: { enabled: true, text: 'Cargando Datos...' },
                                            noDataText: "Sin datos para mostrar",
                                            showBorders: true,
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
                                            paging: {
                                                pageSize: 5
                                            },
                                            pager: {
                                                showPageSizeSelector: true,
                                                allowedPageSizes: [5, 10, 20, 50]
                                            },
                                            hoverStateEnabled: true,
                                            remoteOperations: true,
                                            columns: [
                                                { dataField: 'idAnotacionPuntoControl', width: '5%', caption: 'Código', alignment: 'center' },
                                                { dataField: 'anotacion', width: '55%', caption: 'Anotación', dataType: 'string' },
                                                { dataField: 'fechaRegistro', width: '10%', caption: 'Fecha Registro', dataType: 'date' },
                                                { dataField: 'funcionario', width: '15%', caption: 'Funcionario', dataType: 'string' },
                                                {
                                                    visible: canEdit,
                                                    width: 60,
                                                    caption: "Editar",
                                                    alignment: 'center',
                                                    cellTemplate: function (container, options) {
                                                        $('<div/>').dxButton({
                                                            icon: 'edit',
                                                            caption: "Editar",
                                                            height: 20,
                                                            hint: 'Editar la información relacionada con el Estado del Punto de Control del Expediente Ambiental seleccionado',
                                                            onClick: function (e) {
                                                                idAnotacionPuntoControl = options.data.idAnotacionPuntoControl;
                                                                var _Ruta = $('#SIM').data('url') + "ExpedienteAmbiental/api/ExpedientesAmbApi/ObtenerAnotacionPuntoControlAsync";
                                                                $.getJSON(_Ruta,
                                                                    {
                                                                        Id: options.data.idAnotacionPuntoControl
                                                                    }).done(function (data) {
                                                                        if (data !== null) {

                                                                            txtObservacionAnotacion.option("value", data.anotacion);
                                                                            popupAnotacionPuntoControl.show();

                                                                        }
                                                                    }).fail(function (jqxhr, textStatus, error) {
                                                                        DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Evento no esperado!');
                                                                    });
                                                            }
                                                        }).appendTo(container);
                                                    }
                                                },
                                                {
                                                    visible: canDelete,
                                                    width: 60,
                                                    caption: "Eliminar",
                                                    alignment: 'center',
                                                    cellTemplate: function (container, options) {
                                                        $('<div/>').dxButton({
                                                            icon: 'remove',
                                                            height: 20,
                                                            hint: 'Eliminar el Estado del Punto de Control',
                                                            onClick: function (e) {
                                                                var result = DevExpress.ui.dialog.confirm('Desea eliminar el registro seleccionado?', 'Confirmación');
                                                                result.done(function (dialogResult) {
                                                                    if (dialogResult) {
                                                                        var _Ruta = $('#SIM').data('url') + "ExpedienteAmbiental/api/ExpedientesAmbApi/EliminarAnotacionPuntoControlAsync?Id=" + options.data.idAnotacionPuntoControl;
                                                                        $.ajax({
                                                                            type: 'POST',
                                                                            url: _Ruta,
                                                                            contentType: "application/json",
                                                                            dataType: 'text',
                                                                            success: function (data) {
                                                                                if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar registro seleccionado');
                                                                                else {
                                                                                    $('#GidListadoAnotacionesPuntosControl').dxDataGrid({ dataSource: AnotacionesPuntoControlDataSource });
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
                                            ],
                                            onSelectionChanged: function (selectedItems) {
                                                var data = selectedItems.selectedRowsData[0];
                                                if (data) {
                                                    idAnotacionPuntoControl = data.idAnotacionPuntoControl;
                                                }
                                            }
                                        });
                                        SeleccionaExpPunto(idExpedienteDoc);
                                        $('#GidListadoEstadosPuntosControl').dxDataGrid({ dataSource: EstadosPuntoControlDataSource });
                                        $('#GidListadoAnotacionesPuntosControl').dxDataGrid({ dataSource: AnotacionesPuntoControlDataSource });
                                   }
                                }
                            });

                            var popupPuntoControl = $("#PopupPuntosControl").dxPopup({
                                fullScreen: true,
                                hoverStateEnabled: true,
                                dragEnabled: true,
                                resizeEnabled: true,
                                title: "Puntos de Control asociados al Expediente Ambiental",
                                onShown: function (e) {
                                    $('#divEncabezado').css('visibility', 'visible');
                                    $('#asistente').css('visibility', 'visible');
                                }
                            }).dxPopup("instance");

                            popupPuntoControl.show();
                        }
                    }).appendTo(container);
                }
            }
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            $('#divEncabezado').css('visibility', 'hidden');
            $('#asistente').css('visibility', 'hidden');
            if (data) {
                idExpediente = data.idExpediente;
                idExpedienteAmb = data.idExpediente; 
            }
        }
    });

    var popupTercero = $("#popupTercero").dxPopup({
        width: 1400,
        height: 800,
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Terceros"
    }).dxPopup("instance");

    var txtCM = $("#txtCM").dxTextBox({
        readOnly: true,
        value: "0000",
    }).dxTextBox("instance");

    var txtNit = $("#txtNit").dxTextBox({
        readOnly: false,
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "La cédula o Nit es es requrida(o)!"
        }]
    }).dxTextBox("instance");

    var txtRazonSocial = $("#txtRazonSocial").dxTextBox({
        readOnly: true,
        value: "",
    }).dxTextBox("instance");

    var txtlblNombreExpediente = $("#txtlblNombreExpediente").dxTextBox({
        readOnly: true
    }).dxTextBox("instance");
       
    var txtlblNombrePuntoControl = $("#txtlblNombrePuntoControl").dxTextBox({
        readOnly: true
    }).dxTextBox("instance");

    var txtlblNombrePuntoControlTramite = $("#txtlblNombrePuntoControlTramite").dxTextBox({
        readOnly: true
    }).dxTextBox("instance");
  
    var txtlblNombrePuntoControlEstado = $("#txtlblNombrePuntoControlEstado").dxTextBox({
        readOnly: true
    }).dxTextBox("instance");

    var txtlblNombrePuntoControlNotas = $("#txtlblNombrePuntoControlNotas").dxTextBox({
        readOnly: true
    }).dxTextBox("instance");

    var txtlblCM = $("#txtlblCM").dxTextBox({
        readOnly: true
    }).dxTextBox("instance");

    txtlblCM2 = $("#txtlblCM2").dxTextBox({
        readOnly: true
    }).dxTextBox("instance");

    var txtlblCedulaNit = $("#txtlblCedulaNit").dxTextBox({
        readOnly: true
    }).dxTextBox("instance");

    
    var txtlblNombreRazonSocial = $("#txtlblNombreRazonSocial").dxTextBox({
        readOnly: true
    }).dxTextBox("instance");
        
    var cboIntalaciones = $('#cboInstalaciones').dxDropDownBox({
        valueExpr: 'idInstalacion',
        deferRendering: false,
        placeholder: 'Seleccione una Instalación',
        displayExpr(item) {
            return item && `${item.nombre} <${item.direccion}>`;
        },
        showClearButton: true,
        contentTemplate(e) {
            const value = e.component.option('value');
            const $dataGrid = $('<div>').dxDataGrid({
                columns: [
                    { dataField: 'nombre', width: '45%', caption: 'Nombre Instalación', alignment: 'center' },
                    { dataField: 'direccion', width: '35%', caption: 'Dirección Instalación', alignment: 'center' },
                    { dataField: 'telefono', width: '10%', caption: 'Teléfono', alignment: 'center' },
                ],
                hoverStateEnabled: true,
                paging: { enabled: true, pageSize: 10 },
                filterRow: { visible: true },
                scrolling: { mode: 'virtual' },
                selection: { mode: 'single' },
                selectedRowKeys: [value],
                height: '100%',
            });

            dataGrid = $dataGrid.dxDataGrid('instance');

            e.component.on('valueChanged', (args) => {
                dataGrid.selectRows(args.value, false);
                e.component.close();
            });

            return $dataGrid;
        },
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe seleccionar la Instalación del Tercero que se desaea asociar al nuevo Expediente Ambiental!"
        }]
    }).dxDropDownBox("instance");

    var cboTipoSolicitudAmbiental = $("#cboTipoSolicitudAmbiental").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "idTipoSolicitudAmbiental",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "ExpedienteAmbiental/api/ExpedientesAmbApi/ObtenerTiposSolicitudAmbientalAsync");
                }
            })
        }),
        displayExpr: "nombre",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe seleccionar el Tipo de Solicitud Ambiental!"
        }]
    }).dxSelectBox("instance");

    const loadIndicator = $('#spinLoadBusqueda').dxLoadIndicator({
        height: 40,
        width: 40,
        visible: false
    }).dxLoadIndicator("instance");

    $("#cmdBuscarNit").dxButton({
        text: "Buscar ...",
        type: 'success',
        height: 35,
        onClick: function () {

            var instalacionesArray = [
                { idInstalacion: 0, nombre: '', direccion: '', telefono: '' },
            ];

            var dataSourceInsta = new DevExpress.data.DataSource({
                store: {
                    type: "array",
                    key: "idInstalacion",
                    data: instalacionesArray,
                
                },
            });

            $('#cboInstalaciones').dxDropDownBox({
                valueExpr: 'idInstalacion',
                deferRendering: false,
                placeholder: 'Select a value...',
                displayExpr(item) {
                    return '';
                },
                showClearButton: true,
                dataSource: dataSourceInsta,
                contentTemplate(e) {
                    const value = e.component.option('value');
                    const $dataGrid = $('<div>').dxDataGrid({
                        dataSource: dataSourceInsta,
                        columns: [
                            { dataField: 'nombre', width: '45%', caption: 'Nombre Instalación', alignment: 'center' },
                            { dataField: 'direccion', width: '35%', caption: 'Dirección Instalación', alignment: 'center' },
                            { dataField: 'telefono', width: '10%', caption: 'Teléfono', alignment: 'center' },
                        ],
                        hoverStateEnabled: true,
                        paging: { enabled: true, pageSize: 10 },
                        filterRow: { visible: true },
                        scrolling: { mode: 'virtual' },
                        selection: { mode: 'single' },
                        selectedRowKeys: [value],
                        height: '100%',
                    });

                    dataGrid = $dataGrid.dxDataGrid('instance');

                    e.component.on('valueChanged', (args) => {
                        dataGrid.selectRows(args.value, false);
                        e.component.close();
                    });

                    return $dataGrid;
                },
            });
           
            var _Ruta = $('#SIM').data('url') + "ExpedienteAmbiental/api/ExpedientesAmbApi/ObtenerTerceroAsync";
            loadIndicator.option("visible", true);
            $.getJSON(_Ruta,
                {
                    id: txtNit.option('value')
                }).done(function (data) {
                    if (data.idTercero === 0) {

                        txtRazonSocial.option("value", "");
                        loadIndicator.option("visible", false);
                        popupTercero.show();
                        $('#buscarTercero').attr('src', $('#SIM').data('url') + 'General/Tercero');
                    }
                    else {
                        txtRazonSocial.option("value", data.rSocial);
                        idTercero = data.idTercero;
                        var _Ruta = $('#SIM').data('url') + "ExpedienteAmbiental/api/ExpedientesAmbApi/GetInstalacionesTerceroAsync";
                        loadIndicator.option("visible", true);
                        $.getJSON(_Ruta,
                            {
                                idTercero: data.idTercero
                            }).done(function (datal) {
                                if (datal.numRegistros === 0) {
                                    loadIndicator.option("visible", false);
                                    txtRazonSocial.option("value", "");
                                 
                                    alert('No existen instalaciones disponibles (sin Expediente Ambiental asociado) para el Tercero!')
                                    popupTercero.show();
                                    $('#buscarTercero').attr('src', $('#SIM').data('url') + 'General/Tercero/Index?popup=true');
                                }
                                else {
                                    $('#cboInstalaciones').dxDropDownBox({
                                        valueExpr: 'idInstalacion',
                                        deferRendering: false,
                                        placeholder: 'Select a value...',
                                        displayExpr(item) {
                                            return item && `${item.nombre} <${item.direccion}>`;
                                        },
                                        showClearButton: true,
                                        dataSource: new DevExpress.data.ArrayStore({
                                            data: datal.datos,
                                            key: "idInstalacion"
                                        }), 
                                        contentTemplate(e) {
                                            const value = e.component.option('value');
                                            const $dataGrid = $('<div>').dxDataGrid({
                                                dataSource: e.component.getDataSource(),
                                                columns: [
                                                    { dataField: 'nombre', width: '45%', caption: 'Nombre Instalación', alignment: 'center' },
                                                    { dataField: 'direccion', width: '35%', caption: 'Dirección Instalación', alignment: 'center' },
                                                    { dataField: 'telefono', width: '10%', caption: 'Teléfono', alignment: 'center' },
                                                ],
                                                hoverStateEnabled: true,
                                                paging: { enabled: true, pageSize: 10 },
                                                filterRow: { visible: true },
                                                scrolling: { mode: 'virtual' },
                                                selection: { mode: 'single' },
                                                selectedRowKeys: [value],
                                                height: '100%',
                                                onSelectionChanged(selectedItems) {
                                                    const keys = selectedItems.selectedRowKeys;
                                                    const hasSelection = keys.length;
                                                    idInstalacion = keys[0] ? keys[0] : null;
                                                    txtDireccionExpediente.option("value", selectedItems.selectedRowsData[0] ? selectedItems.selectedRowsData[0].direccion  :null);
                                                    e.component.option('value', hasSelection ? keys[0] : null);
                                                },
                                            });

                                            dataGrid = $dataGrid.dxDataGrid('instance');

                                            e.component.on('valueChanged', (args) => {
                                                dataGrid.selectRows(args.value, false);
                                                e.component.close();
                                            });

                                            return $dataGrid;
                                        },
                                    });

                                    loadIndicator.option("visible", false);
                                }
                            }).fail(function (jqxhr, textStatus, error) {
                                loadIndicator.option("visible", false);
                                DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                            });
               
                        loadIndicator.option("visible", false);
                    }
                }).fail(function (jqxhr, textStatus, error) {
                    loadIndicator.option("visible", false);
                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                });
        }
    }).dxButton("instance");


    $("#cmdEditarNit").dxButton({
        text: "Editar ...",
        type: 'success',
        height: 35,
        onClick: function () {
            txtRazonSocial.option("value", "");
            loadIndicator.option("visible", false);
            popupTercero.show();
            $('#buscarTercero').attr('src', $('#SIM').data('url') + 'General/Tercero');
        }
    }).dxButton("instance");

    var txtDescripcionExpediente = $("#txtDescripcionExpediente").dxTextArea({
        value: "",
        readOnly: false,
        height: 60
    }).dxTextArea("instance");
    
    var txtNombreExpediente = $("#txtNombreExpediente").dxTextArea({
        value: "",
        readOnly: false,
        height: 50
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe ingresar un nombre/descripción asociado al Expediente Ambiental!"
        }]
    }).dxTextArea("instance");

    var txtDireccionExpediente = $("#txtDireccionExpediente").dxTextBox({
        readOnly: false,
        value: "",
    }).dxTextBox("instance");

    var cboMunicipio = $("#cboMunicipio").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "ExpedienteAmbiental/api/ExpedientesAmbApi/ObtenerMunicipios");
                }
            })
        }),
        displayExpr: "Nombre",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe seleccionar el Municipio asociado con el Expediente Ambiental!"
        }]
    }).dxSelectBox("instance");
     
       
    var cboTipoEstado = $("#cboTipoEstado").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "idTipoEstadoPuntoControl",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "ExpedienteAmbiental/api/ExpedientesAmbApi/ObtenerTiposEstadoPuntosControlAsync");
                }
            })
        }),
        valueExpr: "idTipoEstadoPuntoControl",
        displayExpr: "nombre",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe seleccionar el Tipo de Estado del Punto de Control!"
        }]
    }).dxSelectBox("instance");

    var txtObservacionEstado = $("#txtObservacionEstado").dxTextArea({
        value: "",
        readOnly: false,
        height: 70
    }).dxTextArea("instance");

    var txtObservacionAnotacion = $("#txtObservacionAnotacion").dxTextArea({
        value: "",
        readOnly: false,
        height: 70
    }).dxTextArea("instance");

    $("#btnGuardarEstadoPuntoControl").dxButton({
        text: "Guardar Estado",
        type: "default",
        height: 30,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var idPunto = idPuntoControl;
            var observacionEstado = txtObservacionEstado.option("value");
            var item = cboTipoEstado.option("selectedItem");
            var tipoEstadoId = item.idTipoEstadoPuntoControl;
            var idEstadoPunto = idEstadoPuntoControl;
            var params = {
                puntoControlId: idPunto, tipoEstadoPuntoControlId: tipoEstadoId, observacion: observacionEstado, terceroId: idTercero, idEstadoPuntoControl: idEstadoPunto
            };

            var _Ruta = $('#SIM').data('url') + "ExpedienteAmbiental/api/ExpedientesAmbApi/GuardarEstadoPuntoControllAsync";
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
                        DevExpress.ui.dialog.alert('Estado del Punto de Control Creado/Actualizado correctamente con el Id:' + data.Result.IdGenerated, 'Guardar Datos');
                        $('#GidListadoEstadosPuntosControl').dxDataGrid({ dataSource: EstadosPuntoControlDataSource });
                        popupEstadoPuntoControl.hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });


        }
    });
          
    $("#btnGuardarAnotacionPuntoControl").dxButton({
        text: "Guardar Anotación",
        type: "default",
        height: 30,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var idPunto = idPuntoControl;
            var anotacion = txtObservacionAnotacion.option("value");
            var idAnotacionPunto = idAnotacionPuntoControl;
            var params = {
                puntoControlId: idPunto, anotacion: anotacion, idAnotacionPuntoControl: idAnotacionPunto
            };

            var _Ruta = $('#SIM').data('url') + "ExpedienteAmbiental/api/ExpedientesAmbApi/GuardarAnotacionPuntoControllAsync";
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
                        DevExpress.ui.dialog.alert('Anotación del Punto de Control Creado/Actualizado correctamente con el Id:' + data.Result.IdGenerated, 'Guardar Datos');
                        $('#GidListadoAnotacionesPuntosControl').dxDataGrid({ dataSource: AnotacionesPuntoControlDataSource });
                        popupAnotacionPuntoControl.hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });


        }
    });

       
    var cboClasificacion = $("#cboClasificacion").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "idClasificacionExpediente",
                loadMode: "raw",
                load: function () {
                    var datos = $.getJSON($("#SIM").data("url") + "ExpedienteAmbiental/api/ExpedientesAmbApi/ObtenerClasificacionExpedientesAsync");
                    return datos;
                }
            })
        }),
        displayExpr: "nombre"
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe seleccionar la clasificación asociada al Expediente Ambiental!"
        }]
    }).dxSelectBox("instance");

    
    $("#btnGuardarExpediente").dxButton({
        text: "Guardar",
        type: "default",
        height: 30,
        onClick: function () {

            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var id = idExpedienteAmb;

            var nombre = txtNombreExpediente.option("value");
            if (nombre === "") return;

            var descripcion = txtDescripcionExpediente.option("value");
            var direccion = txtDireccionExpediente.option("value");
            var municipioId = cboMunicipio.option("value").Id;
            var clasificacionId = cboClasificacion.option("value").idClasificacionExpediente;
            var cm = txtCM.option("value");
            var params = {
                idExpediente: id, nombre: nombre, cm: cm, descripcion: descripcion, clasificacionExpedienteId: clasificacionId, municipioId: municipioId, direccion: direccion, terceroId: idTercero, instalacionId : idInstalacion 
            };

            var _Ruta = $('#SIM').data('url') + "ExpedienteAmbiental/api/ExpedientesAmbApi/GuardarExpedienteAmbientalAsync";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                crossDomain: true,
                headers: { 'Access-Control-Allow-Origin': '*' },
                success: function (data) {
                    if (data.Result.Response === false) DevExpress.ui.dialog.alert('Ocurrió un error ' + data.Message, 'Guardar Datos');
                    else {
                        DevExpress.ui.dialog.alert('Expediente Ambiental Creado correctamente con el CM:' + data.Result.IdGenerated, 'Guardar Datos');
                        $('#GidListado').dxDataGrid({ dataSource: ExpedientesDataSource });
                        $('#popupNuevoExpediente').dxPopup("instance").hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    }).dxButton("instance");

    $("#btnNuevoExpediente").dxButton({
        text: "Nuevo",
        type: "success",
        height: 30,
        width: 100,
        icon: 'add',
        onClick: function () {
            idExpediente = 0;
            txtCM.reset();
            txtNombreExpediente.reset();
            txtDescripcionExpediente.reset();
            txtDireccionExpediente.reset();
            cboClasificacion.reset();
            cboMunicipio.reset();
            popupNuevoExpediente.show();
        }
    }).dxButton("instance");

    var popupNuevoExpediente = $("#popupNuevoExpediente").dxPopup({
        width: 900,
        height: "auto",
        dragEnabled: true,
        resizeEnabled: true,
        hoverStateEnabled: true,
        title: "Creación/Edición de Expediente Ambiental"
    }).dxPopup("instance");

    var popupTramitesExpediente = $("#popupTramitesExpediente").dxPopup({
        width: 1200,
        height: 700,
        dragEnabled: true,
        resizeEnabled: true,
        hoverStateEnabled: true,
        title: "Trámites asociados al Expediente"
    }).dxPopup("instance");

    var popupTramitesPuntoControl = $("#popupTramitesPuntoControl").dxPopup({
        width: 1200,
        height: 700,
        dragEnabled: true,
        resizeEnabled: true,
        hoverStateEnabled: true,
        title: "Trámites asociados al Punto de Control"
    }).dxPopup("instance");
       

    var popupEstadoPuntoControl = $("#popupEstadoPuntoControl").dxPopup({
        width: 800,
        height: "auto",
        dragEnabled: true,
        resizeEnabled: true,
        hoverStateEnabled: true,
        title: "Creación/Edición de Estado de Punto de Control"
    }).dxPopup("instance"); 

    var popupAnotacionPuntoControl = $("#popupAnotacionPuntoControl").dxPopup({
        width: 800,
        height: "auto",
        dragEnabled: true,
        resizeEnabled: true,
        hoverStateEnabled: true,
        title: "Creación/Edición de Anotación asociada al Punto de Control"
    }).dxPopup("instance"); 

    $("#btnNuevoPuntoControl").dxButton({
        text: "Nuevo",
        type: "success",
        height: 30,
        width: 100,
        icon: 'add',
        onClick: function () {
            editar = false;
            idPuntoControl = 0;
            txtNombrePuntoControl.reset();
            txtObservacionPuntoControl.reset();
            cboTipoSolicitudAmbiental.option("disabled", false);
            cboTipoSolicitudAmbiental.reset();
            txtConexoPunto.reset();
            txtFechaOrigen.option("value", new Date());
            popupNuevoPuntoControl.show();
           
        }
    }).dxButton("instance");

    $("#btnNuevoEstadoPuntoControl").dxButton({
        text: "Nuevo",
        type: "success",
        height: 30,
        width: 100,
        icon: 'add',
        onClick: function () {
            idEstadoPuntoControl = 0;
            cboTipoEstado.reset();
            txtObservacionEstado.reset();
            popupEstadoPuntoControl.show();

        }
    }).dxButton("instance");

    $("#btnNuevoAnotacionPuntoControl").dxButton({
        text: "Nuevo",
        type: "success",
        height: 30,
        width: 100,
        icon: 'add',
        onClick: function () {
            idAnotacionPuntoControl = 0;
            txtObservacionAnotacion.reset();
            popupAnotacionPuntoControl.show();

        }
    }).dxButton("instance");
            
    var txtNombrePuntoControl = $("#txtNombrePuntoControl").dxTextArea({
        value: "",
        readOnly: false,
        height: 50
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe ingresar un nombre/descripción asociado al Punto de Control!"
        }]
    }).dxTextArea("instance");

    var txtObservacionPuntoControl = $("#txtObservacionPuntoControl").dxTextArea({
        value: "",
        readOnly: false,
        height: 70
    }).dxTextArea("instance");
          
    var txtFechaOrigen = $("#txtFechaOrigen").dxDateBox({
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
            message: "Debe ingresar la fecha de orígen de la solicitud"
        }]
    }).dxDateBox("instance");

    var txtConexoPunto = $("#txtConexoPunto").dxTextBox({
        readOnly: true,
        value: "",
    }).dxTextBox("instance");

    $("#btnGuardarPuntoControl").dxButton({
        text: "Guardar",
        type: "default",
        height: 30,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var id = idPuntoControl;
            var idExpedienteA = idExpediente;
            var nombreG = txtNombrePuntoControl.option("value");
            var conexoG = txtConexoPunto.option("value");
            var observacionG = txtObservacionPuntoControl.option("value");
            var fechaOrigenG = txtFechaOrigen.option("value");
            if (nombreG === "") return;
            var item = cboTipoSolicitudAmbiental.option("selectedItem");
            var tipoSolicitudAmbientalId = item.idTipoSolicitudAmbiental;
            var Indices = indicesSerieDocumentalStore._array;
            var params = {
                idPuntoControl: id, expedienteAmbientalId: idExpedienteA, TipoSolicitudAmbientalId: tipoSolicitudAmbientalId, nombre: nombreG, conexo: conexoG, observacion: observacionG, fechaOrigen: fechaOrigenG, Indices: Indices
            };

            var _Ruta = $('#SIM').data('url') + "ExpedienteAmbiental/api/ExpedientesAmbApi/GuardarPuntoControllAsync";
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
                        DevExpress.ui.dialog.alert('Punto de Control Creado/Actualizado correctamente!', 'Guardar Datos');
                        $('#GidListadoPuntosControl').dxDataGrid({ dataSource: PuntosControlDataSource });
                        popupNuevoPuntoControl.hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    }).dxButton("instance");
         
    var popupNuevoPuntoControl = $("#popupNuevoPuntoControl").dxPopup({
        width: 800,
        height: "auto",
        dragEnabled: true,
        resizeEnabled: true,
        hoverStateEnabled: true,
        title: "Creación/Edición de un Punto de Control",
        onShown: function () {
            if (!editar) {
                indicesSerieDocumentalStore = null;
                CargarIndices();
            } else {
                CargarGridIndices();
            }
        }  
    }).dxPopup("instance");

    var popupVicularExpedienteDocumental = $("#popupVicularExpedienteDocumental").dxPopup({
        width: 900,
        height: 800,
        dragEnabled: true,
        resizeEnabled: true,
        hoverStateEnabled: true,
        title: "Vincular Expediente Documental"
    }).dxPopup("instance");
       
    $("#btnIndices").dxButton({
        text: "Indices Expediente",
        icon: "fields",
        hint: 'Indices del documento',
        visible: false,
        onClick: function () {
            var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/IndicesExpediente";
            $.getJSON(_Ruta, { IdExp: idExpedienteDoc })
                .done(function (data) {
                    if (data != null) {
                        showIndices(data);
                    }
                }).fail(function (jqxhr, textStatus, error) {
                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Indices del documento');
                });
        }
    });

    $("#popupBuscaExp").dxPopup({
        width: 900,
        height: 800,
        showTitle: true,
        title: "Buscar Expediente"
    });

    $("#cmdShowExpedienteFlip").dxButton({
        text: "Ver Expediente ...",
        icon: "fields",
        hint: 'Ver el Expediente Documental',
        visible: false,
        onClick: function () {
            verExpediente($('#SIM').data('url') + 'ExpedienteAmbiental/Expedientes/FlipExpediente?id=' + idExpedienteDoc);
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

    function CargarGridIndices() {


        var _Ruta = $('#SIM').data('url') + "ExpedienteAmbiental/api/ExpedientesAmbApi/ObtenerExpedienteAsync";
        $.getJSON(_Ruta,
            {
                Id: idExpediente
            }).done(function (data) {
                if (data !== null) {
                    cm = data.cm;
                    direccion = data.direccion;
                    municipio = data.municipio;
                }
            }).fail(function (jqxhr, textStatus, error) {
                DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Evento no esperado!');
            });



        $("#GridIndices").dxDataGrid({
            dataSource: indicesSerieDocumentalStore,
            allowColumnResizing: true,
            allowSorting: false,
            showRowLines: true,
            rowAlternationEnabled: true,
            showBorders: true,
            height: '100%',
            loadPanel: { text: 'Cargando Datos...' },
            paging: {
                pageSize: 0,
            },
            pager: {
                showPageSizeSelector: false,
            },
            filterRow: {
                visible: false,
            },
            groupPanel: {
                visible: false,
            },
            editing: {
                mode: "cell",
                allowUpdating: true,
                allowAdding: false,
                allowDeleting: false

            },
            selection: {
                mode: 'single'
            },
            scrolling: {
                showScrollbar: 'always',
            },
            wordWrapEnabled: true,
            columns: [
                {
                    dataField: "CODINDICE",
                    dataType: 'number',
                    visible: false,
                }, {
                    dataField: "INDICE",
                    caption: 'INDICE',
                    dataType: 'string',
                    width: '40%',
                    visible: true,
                    allowEditing: false
                }, {
                    dataField: 'VALOR',
                    caption: 'VALOR',
                    dataType: 'string',
                    allowEditing: true,
                    cellTemplate: function (cellElement, cellInfo) {
                        switch (cellInfo.data.TIPO) {
                            case 0: // TEXTO
                                if (cellInfo.data.VALOR != null) {
                                    cellElement.html(cellInfo.data.VALOR);
                                } else {
                                    if (cellInfo.data.INDICE == "CM") {
                                        cellElement.html(cm);
                                    }
                                    if (cellInfo.data.INDICE == "DIRECCIÓN") {
                                        cellElement.html(direccion);
                                    }
                                    if (cellInfo.data.INDICE == "OFICINA PRODUCTORA Y / O CENTRO DE COSTOS") {
                                        cellElement.html('10602');
                                    }
                                }
                                break;
                            case 1: // NUMERO
                                if (cellInfo.data.VALOR != null) {
                                    cellElement.html(cellInfo.data.VALOR);
                                } else {
                                    if (cellInfo.data.INDICE == "OFICINA PRODUCTORA Y / O CENTRO DE COSTOS") {
                                        cellElement.html('10602');
                                    }
                                }
                                break;
                            case 3: // HORA
                            case 5: //LISTA
                                if (cellInfo.data.VALOR != null) {
                                    cellElement.html(cellInfo.data.VALOR);
                                } else {
                                    if (cellInfo.data.INDICE == "MUNICIPIO") {
                                        cellElement.html(municipio);
                                    }
                                }
                                break;
                            case 8: // DIRECCION
                                if (cellInfo.data.VALOR != null) {
                                    cellElement.html(cellInfo.data.VALOR);
                                }
                                break;
                            case 2: // FECHA
                                if (cellInfo.data.VALOR != null) {
                                    //cellElement.html(cellInfo.data.VALOR.getDate() + '/' + (cellInfo.data.VALOR.getMonth() + 1) + '/' + cellInfo.data.VALOR.getFullYear());
                                    cellElement.html(cellInfo.data.VALOR);
                                }
                                break;
                            case 4: // BOOLEAN
                                if (cellInfo.data.VALOR != null)
                                    cellElement.html(cellInfo.data.VALOR == 'S' ? 'SI' : 'NO');
                                break;
                            default: // OTRO
                                if (cellInfo.data.VALOR != null)
                                    cellElement.html(cellInfo.data.VALOR);
                                break;
                        }
                    },
                    editCellTemplate: function (cellElement, cellInfo) {
                        switch (cellInfo.data.TIPO) {
                            case 1: // NUMERO
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);
                                if (cellInfo.data.MINIMO.length && cellInfo.data.MAXIMO.length > 0) {
                                    $(div).dxNumberBox({
                                        value: cellInfo.data.VALOR,
                                        width: '100%',
                                        min: cellInfo.data.MINIMO,
                                        max: cellInfo.data.MAXIMO,
                                        showSpinButtons: false,
                                        onValueChanged: function (e) {
                                            cellInfo.setValue(e.value);
                                        },
                                    });
                                } else {
                                    $(div).dxNumberBox({
                                        value: cellInfo.data.VALOR,
                                        width: '100%',
                                        showSpinButtons: false,
                                        onValueChanged: function (e) {
                                            cellInfo.setValue(e.value);
                                        },
                                    });
                                }
                                break;
                            case 0: // TEXTO
                            case 3: // HORA
                            case 8: // DIRECCION
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                $(div).dxTextBox({
                                    value: cellInfo.data.VALOR,
                                    width: '100%',
                                    onValueChanged: function (e) {
                                        cellInfo.setValue(e.value);
                                    },
                                });
                                break;
                            case 2: // FECHA
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                var fecha = null;
                                if (cellInfo.data.VALOR != null && cellInfo.data.VALOR.trim() != '') {
                                    var partesFecha = cellInfo.data.VALOR.split('/');
                                    fecha = new Date(parseInt(partesFecha[2]), parseInt(partesFecha[1]) - 1, parseInt(partesFecha[0]));

                                    $(div).dxDateBox({
                                        type: 'date',
                                        width: '100%',
                                        displayFormat: 'dd/MM/yyyy',
                                        value: fecha,
                                        onValueChanged: function (e) {
                                            //cellInfo.setValue(e.value);
                                            cellInfo.setValue(e.value.getDate() + '/' + (e.value.getMonth() + 1) + '/' + e.value.getFullYear());
                                        },
                                    });
                                } else {
                                    $(div).dxDateBox({
                                        type: 'date',
                                        width: '100%',
                                        displayFormat: 'dd/MM/yyyy',
                                        onValueChanged: function (e) {
                                            //cellInfo.setValue(e.value);
                                            cellInfo.setValue(e.value.getDate() + '/' + (e.value.getMonth() + 1) + '/' + e.value.getFullYear());
                                        },
                                    });
                                }

                                break;
                            case 4: // SI/NO
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                $(div).dxSelectBox({
                                    dataSource: siNoOpciones,
                                    width: '100%',
                                    displayExpr: "Nombre",
                                    valueExpr: "ID",
                                    placeholder: "[SI/NO]",
                                    value: cellInfo.data.VALOR,
                                    onValueChanged: function (e) {
                                        cellInfo.setValue(e.value);
                                        $("#grdIndices").dxDataGrid("saveEditData");
                                    },
                                });
                                break;
                            case 5: // Lista
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                let itemsLista = opcionesLista[opcionesLista.findIndex(ol => ol.idLista == cellInfo.data.ID_LISTA)].datos;

                                if (cellInfo.data.ID_LISTA != null) {
                                    $(div).dxSelectBox({
                                        items: itemsLista,
                                        width: '100%',
                                        placeholder: "[SELECCIONAR OPCION]",
                                        value: (cellInfo.data.VALOR == null ? null : itemsLista[itemsLista.findIndex(ls => ls.NOMBRE == cellInfo.data.VALOR)].ID),
                                        displayExpr: 'NOMBRE',
                                        valueExpr: 'ID',
                                        searchEnabled: true,
                                        onValueChanged: function (e) {
                                            cellInfo.data.ID_VALOR = e.value;
                                            cellInfo.setValue(itemsLista[itemsLista.findIndex(ls => ls.ID == e.value)].NOMBRE);
                                            $("#grdIndices").dxDataGrid("saveEditData");
                                        },
                                    });
                                } else {
                                    $(div).dxTextBox({
                                        value: cellInfo.data.VALOR,
                                        width: '100%',
                                        onValueChanged: function (e) {
                                            cellInfo.setValue(e.value);
                                        },
                                    });
                                }
                                break;
                        }
                    }
                }, {
                    dataField: "OBLIGA",
                    caption: 'R',
                    width: '40px',
                    dataType: 'string',
                    visible: true,
                    allowEditing: false,
                    cellTemplate: function (cellElement, cellInfo) {
                        cellElement.css('text-align', 'center');
                        cellElement.css('color', 'red');
                        if (cellInfo.data.OBLIGA == 1)
                            cellElement.html('*');

                    }
                }
            ]
        });
    }

    function AsignarIndices(indices) {
        opcionesLista = [];

        indicesSerieDocumentalStore = new DevExpress.data.LocalStore({
            key: 'CODINDICE',
            data: indices,
            name: 'indicesSerieDocumental'
        });

        indices.forEach(function (valor, indice, array) {
            if (valor.TIPO == 5 && valor.ID_LISTA != null) {

                if (opcionesLista.findIndex(ol => ol.idLista == valor.ID_LISTA) == -1) {
                    opcionesLista.push({ idLista: valor.ID_LISTA, datos: null, cargado: false });
                }
            }
        });

        if (opcionesLista.length == 0) {
            CargarGridIndices();
        } else {
            opcionesLista.forEach(function (valor, indice, array) {
                $.getJSON($('#SIM').data('url') + 'Tramites/api/ProyeccionDocumentoApi/ObtenerIndiceValoresLista?id=' + valor.idLista).done(function (data) {
                    var index = opcionesLista.findIndex(ol => ol.idLista == valor.idLista);
                    opcionesLista[index].datos = data;
                    opcionesLista[index].cargado = true;

                    var finalizado = true;

                    opcionesLista.forEach(function (v, i, a) {
                        finalizado = finalizado && v.cargado;
                    });

                    if (finalizado) {
                        CargarGridIndices();
                    }
                });
            });
        }
    }

    function CargarIndices() {
        var URL = $('#SIM').data('url') + 'GestionDocumental/api/ExpedientesApi/ObtenerIndicesSerieDocumental';
        $.getJSON(URL, {
            codSerie: CodigoUnidadDocumental,
        }).done(function (data) {
            AsignarIndices(data);
        });
    }

   });

function SeleccionaExp(Expediente) {
    if (Expediente != "") {
        var IdExpedienteDoc = Expediente;

        var params = {
            idExpediente: IdExpedienteDoc, idPuntoControl: idPuntoControl
        };

        var _Ruta = $('#SIM').data('url') + "ExpedienteAmbiental/api/ExpedientesAmbApi/VincularExpedienteDocumentalAPuntoControllAsync";
        $.ajax({
            type: "POST",
            dataType: 'json',
            url: _Ruta,
            data: JSON.stringify(params),
            contentType: "application/json",
            crossDomain: true,
            headers: { 'Access-Control-Allow-Origin': '*' },
            success: function (data) {
                if (data.Result.Response === false) DevExpress.ui.dialog.alert('Advertencia:  ' + data.Result.Message, 'Vinculando Expediente Documental');
                else
                {
                    $('#GidListadoPuntosControl').dxDataGrid({ dataSource: PuntosControlDataSource });
                    DevExpress.ui.dialog.alert('Expediente Documental se vinculó con el Punto de Control seleccionado!');
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Vinculando Expediente Documental');
            }
        });
    }

    var popupVicularExpedienteDocumental = $("#popupVicularExpedienteDocumental").dxPopup("instance");
    popupVicularExpedienteDocumental.hide();

}

function SeleccionaExpPunto(ExpedienteDocId) {
    if (ExpedienteDocId != "") {
        idExpedienteDoc = ExpedienteDocId;
        $("#PanelDer").addClass("hidden");
        var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/NombreExpediente";
        $.getJSON(_Ruta, { IdExp: idExpedienteDoc })
            .done(function (data) {
                if (data != "") {
                    NomExpediente = data;
                    $("#lblExpediente").text(NomExpediente);
                    $("#btnIndices").dxButton("instance").option("visible", true);
                    $("#cmdShowExpedienteFlip").removeClass("hidden");
                    $("#cmdShowExpedienteFlip").dxButton("instance").option("visible", true);
                    $("#dxTreeView").dxTreeView({
                        dataSource: new DevExpress.data.DataSource({
                            store: new DevExpress.data.CustomStore({
                                key: "ID",
                                loadMode: "raw",
                                load: function () {
                                    return $.getJSON($("#SIM").data("url") + "GestionDocumental/api/ExpedientesApi/ArbolExpediente", { IdExp: idExpedienteDoc });
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
                    $("#dxTreeView").dxTreeView("instance").option("visible", true);
                    $("#btnIndices").dxButton("instance").option("visible", true);
                }
            }).fail(function (jqxhr, textStatus, error) {
                DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Asociar documento');
            }
            );
    }
    else {
        $("#dxTreeView").dxTreeView("instance").option("dataSource", null);
        $("#dxTreeView").dxTreeView("instance").option("visible", false);
        $("#btnIndices").dxButton("instance").option("visible", false);
        $("#lblExpediente").text("No se encuentra vinculado un Expediente Documental con el Punto de Control!");
        $("#cmdShowExpedienteFlip").dxButton("instance").option("visible", false);
        $("#cmdShowExpedienteFlip").addClass("hidden");
        alert("No se ha vinculado un Expediente Documental al Punto de Control seleccionado!");
        $("#PanelDer").addClass("hidden");

    }
}

function verExpediente(ruta) {
    var w = 1500;
    var h = 800;
    var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : window.screenX;
    var dualScreenTop = window.screenTop != undefined ? window.screenTop : window.screenY;
    var width = w;
    var height = h;
    var left = ((width / 2) - (w / 2)) + dualScreenLeft;
    var top = ((height / 2) - (h / 2)) + dualScreenTop;
    var title = "";

    var newWindow = window.open(ruta, title, 'scrollbars=yes,resizable=1,status=yes, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);

    if (window.focus) {
        newWindow.focus();
    }
}

var ExpedientesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"nombre","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') +'ExpedienteAmbiental/api/ExpedientesAmbApi/GetExpedientesAsync', {
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
            if (data.datos === null) {
                alert('No fué posible realizar conexión al microservicio de Expedientes Ambientales');
            }
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var PuntosControlDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        $.getJSON($('#SIM').data('url') + 'ExpedienteAmbiental/api/ExpedientesAmbApi/GetPuntosControlExpedienteAsync', {
            idExpediente: idExpediente
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var EstadosPuntoControlDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        $.getJSON($('#SIM').data('url') + 'ExpedienteAmbiental/api/ExpedientesAmbApi/GetEstadosPuntoControlExpedienteAsync', {
            idPuntoControl: idPuntoControl
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var AnotacionesPuntoControlDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        $.getJSON($('#SIM').data('url') + 'ExpedienteAmbiental/api/ExpedientesAmbApi/GetAnotacionesPuntoControlExpedienteAsync', {
            idPuntoControl: idPuntoControl
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

