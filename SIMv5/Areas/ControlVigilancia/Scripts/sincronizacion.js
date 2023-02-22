var IdRegistro = -1;

$(document).ready(function () {
    $("#GidListado").dxDataGrid({
        dataSource: SincronizacionDataSource,
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
            showPageSizeSelector: false,
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
            { dataField: 'ID', width: '10%', caption: 'Id', alignment: 'center' },
            { dataField: 'FECHA', width: '15%', caption: 'Fecha', alignment: 'center' },
            { dataField: 'OBSERVACIONES', width: '40%', caption: 'Observaciones', alignment: 'center' },
            { dataField: 'SERVICIO', width: '30%', caption: 'Servicio', alignment: 'center' },
            {
                width: '5%',
                caption: "Log Sincronizados",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'event',
                        height: 20,
                        hint: 'Registros Sincronizados',
                        onClick: function (e) {
                            IdReposicion = options.data.ID;
                            $("#GidListadoSincronizados").dxDataGrid({
                                dataSource: SincronizadosDataSource,
                                allowColumnResizing: true,
                                loadPanel: { enabled: true, text: 'Cargando Datos...' },
                                noDataText: "Sin datos para mostrar",
                                showBorders: true,
                                height: 450,
                                paging: {
                                    enabled: false,
                                    pageIndex: 0,
                                    pageSize: 20
                                },
                                pager: {
                                    showPageSizeSelector: false,
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
                                    { dataField: 'CODIGO_REGISTRO', width: '15%', caption: 'Código', alignment: 'center' },
                                    { dataField: 'INSTRUCCION', width: '85%', caption: 'Instrucción', dataType: 'string' }
                                ]
                            });
                            var popupS = $("#PupupSincronizados").dxPopup({
                                width: 1200,
                                height: 530,
                                hideOnParentScroll: false,
                                hoverStateEnabled: true,
                                dragEnabled: true,
                                resizeEnabled: true,
                                title: "Registros Sincronizados"
                            }).dxPopup("instance");
                            popupS.show();
                        }
                    }).appendTo(container);
                }
            },
            {
                width: '5%',
                caption: "Log no Sincronizados",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'event',
                        height: 20,
                        hint: 'Registros no Sincronizados',
                        onClick: function (e) {
                            IdReposicion = options.data.ID;
                            $("#GidListadoNoSincronizados").dxDataGrid({
                                dataSource: NoSincronizadosDataSource,
                                allowColumnResizing: true,
                                loadPanel: { enabled: true, text: 'Cargando Datos...' },
                                noDataText: "Sin datos para mostrar",
                                showBorders: true,
                                height: 450,
                                paging: {
                                    enabled: false,
                                    pageIndex: 0,
                                    pageSize: 20
                                },
                                pager: {
                                    showPageSizeSelector: false,
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
                                    { dataField: 'CODIGO_REGISTRO', width: '15%', caption: 'Código', alignment: 'center' },
                                    { dataField: 'INSTRUCCION', width: '40%', caption: 'Instrucción', dataType: 'string' },
                                    { dataField: 'MENSAJE_ERROR', width: '40%', caption: 'Mensaje de Error', dataType: 'string' }
                                ]
                            });
                            var popupNS = $("#PupupNoSincronizados").dxPopup({
                                width: 1200,
                                height: 530,
                                hoverStateEnabled: true,
                                hideOnParentScroll: false,
                                dragEnabled: true,
                                resizeEnabled: true,
                                title: "Registros no Sincronizados"
                            }).dxPopup("instance");
                            popupNS.show();
                        }
                    }).appendTo(container);
                }
            },
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                IdRegistro = data.ID;
            }
        }
    });
    
    $("#btnSincronizar").dxButton({
        text: "Sincronizar ....",
        height: 30,
        template(data, container) {
            $(`<div class='button-indicator'></div><span class='dx-button-text'>${data.text}</span>`).appendTo(container);
            buttonIndicator = container.find('.button-indicator').dxLoadIndicator({
                visible: false,
            }).dxLoadIndicator('instance');
        },
        onClick: function (data) {
            data.component.option('text', 'Sincronizando ...');
            buttonIndicator.option('visible', true);

            var button = $("#btnSincronizar").dxButton("instance");
            button.option('disabled', true);

            var _Ruta = "http://localhost/SIMAPI/api/SAU/ActualizarDBAsync";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                contentType: "application/json",
                beforeSend: function () { },
                success: function (data) {
                    data.component.option('text', 'Sincronizar ...');
                    buttonIndicator.option('visible', false);
                    var button = $("#btnSincronizar").dxButton("instance");
                    button.option('disabled', false);
                    alert("Proceso ejcutado! : " + data.Message);
                },
                error: function (xhr, textStatus, errorThrown) {
                    data.component.option('text', 'Sincronizar ...');
                    buttonIndicator.option('visible', false);
                    var button = $("#btnSincronizar").dxButton("instance");
                    button.option('disabled', false);
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                }
            });
        }
    });
    var popup = $("#PopupNuevaSincro").dxPopup({
        width: 900,
        height: "auto",
        dragEnabled: true,
        resizeEnabled: true,
        hoverStateEnabled: true,
        title: "Iniciar Proceso de Sincronización"
    }).dxPopup("instance");

  
    $("#btnNuevo").dxButton({
        stylingMode: "contained",
        text: "Sincronizar ...",
        type: "success",
        width: 200,
        height: 30,
        icon: 'add',
        onClick: function () {
            popup.show();
        }
    });
});

var SincronizacionDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"ID","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'ControlVigilancia/api/SincronizacionSAUApi/GetSincronizaciones', {
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

var SincronizadosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        $.getJSON($('#SIM').data('url') + 'ControlVigilancia/api/SincronizacionSAUApi/GetSincronizados', {
            Id: IdRegistro
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var NoSincronizadosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        $.getJSON($('#SIM').data('url') + 'ControlVigilancia/api/SincronizacionSAUApi/GetNoSincronizados', {
            Id: IdRegistro
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});
