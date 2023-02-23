var id;
var idTercero;
var idInstalacion;
var idPuntoControl = 0;
var _numeroVITAL = "";
var _radicadoVITAL = "";
var idExpedienteDoc;
var idEstadoPuntoControl = 0;
var idAnotacionPuntoControl = 0;
var NomExpediente = "";
var _nombreArchivo = "";
var IdIUnidadDoc = 0;
var IdTomo = 0;

$(document).ready(function () {

    $('#asistente').accordion({
        collapsible: true,
        animationDuration: 500,
    });
    $('#asistente').css('visibility', 'hidden');

    $("#GridListado").dxDataGrid({
        dataSource: SolicitudesVitalDataSource,
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
            { dataField: 'id', width: '5%', caption: 'Id'},
            { dataField: 'tipoTramite', width: '20%', caption: 'Tipo de Trámite'},
            { dataField: 'numeroVITAL', width: '10%', caption: 'Número VITAL'},
            { dataField: 'numeroVITALAsociado', width: '10%', caption: 'Número VITAL Asociado' },
            { dataField: 'radicacionId', width: '5%', caption: 'Radicado VITAL' },
            { dataField: 'identificador', width: '5%', caption: 'Identificador', visible: false },
            { dataField: 'codigoTramiteSIM', width: '5%', caption: 'Trámite SIM' },
            { dataField: 'fecha', width: '10%', caption: 'Fecha', dataType:'date' },
            { dataField: 'actoAdministrativo', width: '10%', caption: 'Acto Administrativo'},
            { dataField: 'numeroSILPA', width: '15%', caption: 'Número SILPA' },
            { dataField: 'radicacionId',visible:false},
            {
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
                                    Id: options.data.id
                                }).done(function (data) {
                                    if (data !== null) {
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
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            $('#asistente').css('visibility', 'visible');
            if (data) {
                id = data.id;
                _numeroVITAL = data.numeroVITAL;
                _radicadoVITAL = data.radicacionId;
                $('#GridListadoDocumentosRequeridos').dxDataGrid({ dataSource: DocumentosRequeridosDataSource });
                $('#GridListadoDocumentosAportados').dxDataGrid({ dataSource: DocumentosAportadosDataSource });
            }
        }
    });

    $("#GridListadoDocumentosRequeridos").dxDataGrid({
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        export: {
            enabled: false,
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
            visible: false,
            width: 240,
            placeholder: "Buscar..."
        },
        selection: {
            mode: 'single'
        },
        hoverStateEnabled: true,
        remoteOperations: true,
        columns: [
            { dataField: 'documentosRequeridosTramitesId', width: '5%', caption: 'Id', visible: false },
            { dataField: 'descripcion', width: '40%', caption: 'Descripción'},
            { dataField: 'formato', width: '10%', caption: 'Formato', visible: false },
            { dataField: 'orden', width: '10%', caption: 'Orden' , visible: false},
            { dataField: 'obligatorio', width: '5%', caption: 'Obligatorio', visible: false },
        ],
        onSelectionChanged: function (selectedItems) {
          
        }
    });

    $("#GridListadoDocumentosAportados").dxDataGrid({
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        export: {
            enabled: false,
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
            visible: false,
            width: 240,
            placeholder: "Buscar..."
        },
        selection: {
            mode: 'single'
        },
        hoverStateEnabled: true,
        remoteOperations: true,
        columns: [
            { dataField: 'nombre', width: '80%', caption: 'Descripción' },
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

                            var _Ruta = $('#SIM').data('url') + "ExpedienteAmbiental/api/VitalApi/GetDocumentoAsync";

                            $.getJSON(_Ruta,
                                {
                                    radicado: _radicadoVITAL,
                                    nombreDocumento: _nombreArchivo
                                }).done(function (data) {
                                    window.open(data, "Documento ", "width= 900,height=800,scrollbars = yes, location = no, toolbar = no, menubar = no, status = no");

                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Evento no esperado!');
                                });
                       }
                    }).appendTo(container);
                }
            },
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            _nombreArchivo = data.nombre;
        }
    });

    const loadIndicator = $('#spinLoadBusqueda').dxLoadIndicator({
        height: 40,
        width: 40,
        visible: false
    }).dxLoadIndicator("instance");

});

var SolicitudesVitalDataSource = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var d = $.Deferred();
            var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"tipoTramite","desc":false}]';
            var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

            var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
            var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
            $.getJSON($('#SIM').data('url') + 'ExpedienteAmbiental/api/VitalApi/GetSolicitudesVITALenSIMAsync', {
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

var DocumentosRequeridosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"orden","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'ExpedienteAmbiental/api/VitalApi/GetDocumentosRequeridosTramitesync', {
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
            CodFuncionario: CodigoFuncionario,
            numeroVITAL: _numeroVITAL
        }).done(function (data) {
            if (data.datos === null) {
                alert('No existen documentos establecidos!!!');
            }
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var DocumentosAportadosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"nombre","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'ExpedienteAmbiental/api/VitalApi/GetDocumentosAportadosync', {
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
            CodFuncionario: CodigoFuncionario,
            radicadoVITAL: _radicadoVITAL
        }).done(function (data) {
            if (data.datos === null) {
                alert('No existen documentos establecidos!!!');
            }
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});



