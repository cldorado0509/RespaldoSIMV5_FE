var SIMApp = angular.module('SIM', ['dx']);

var documentosSeleccionado = null;
var fechaInicialSel = null;
var fechaFinalSel = null;
var horaInicialSel = null;
var horaFinalSel = null;
var estacionesSel = null;
var idDocumentosAlmacenadosSel = null;
var listaDocumentosSel = null;

SIMApp.controller("ConsultaPrestamoController", function ($scope, $location, $http) {
    $('.my-cloak').removeClass('my-cloak');

    $scope.parametros = {};
    $scope.parametros.IDDATOSCONSULTA = null;
    $scope.parametros.fechaInicial = null;
    $scope.parametros.fechaFinal = null;
    $scope.parametros.horaInicial = null;
    $scope.parametros.horaFinal = null;
    $scope.parametros.tipoReporte = 0;
    $scope.parametros.tipoReporteSel = null;
    $scope.documentos = {};
    $scope.documentos.tipoDocumentos = 0;
    $scope.documentos.nombreDatosAlmacenados = null;
    $scope.documentos.listaDocumentos = null;
    $scope.parametros.estacionesSeleccionadas = "[Sin Selección]";
    $scope.parametros.estacionesIdSeleccionadas = null;

    $scope.selectedTab = 0;
    $scope.cargandoVisible = false;
    $scope.almacenar = false;

    ObtenerDocumentosAlmacenados();

    $scope.tabsData = [
            { text: 'Filtros', pos: 0 },
            { text: 'Resultado Consulta', pos: 1 },
    ];

    $scope.dxTabsOptions = {
        dataSource: $scope.tabsData,
        onItemClick: (function (itemData) {
            $scope.selectedTab = itemData.itemIndex;
        }),
        selectedIndex: 0
    };

    $scope.isActive = function (tabIndex) {
        if ($scope.selectedTab === tabIndex)
            return true;
        else
            return false;
    };

    $scope.tipoDocumentos = function (tipoDoc) {
        if ($scope.documentos.tipoDocumentos == tipoDoc)
            return true;
        else
            return false;
    };

    $scope.documentosAlmacenadosSelectBoxSettings = {
        dataSource: cboDocumentosAlmacenadosDataSource,
        valueExpr: 'IDDATOSCONSULTA',
        displayExpr: 'S_NOMBRE',
        placeholder: '[Seleccionar Documentos Almacenados]',
        bindingOptions: { value: 'parametros.IDDATOSCONSULTA' },
    };

    $scope.documentosSettings = {
        height: 120,
        bindingOptions: {
            value: "documentos.listaDocumentos",
        }
    };

    $scope.archivoDocumentosSettings = {
        buttonText: 'Seleccionar Archivo',
        labelText: 'Arrastrar Archivo Aquí',
        multiple: false,
        showFileList: false,
        accept: '*',
        //uploadUrl: $('#app').data('url') + 'EnCicla/api/ConsultaApi/ArchivoDocumentos',
        onValueChanged: function (e) {
            var data = new FormData();
            var files = $('.dx-fileuploader-input').get(0).files;

            data.append('nombreDocumentos', $scope.documentos.nombreDatosAlmacenados);

            for (i = 0; i < files.length; i++) {
                data.append("file" + i, files[i]);
            }

            $scope.cargandoVisible = true;

            $http.post($('#app').data('url') + 'EnCicla/api/ConsultaApi/ArchivoDocumentos', data, { transformRequest: angular.identity, headers: { 'Content-Type': undefined } }).success(function (data, status, headers, config) {
                $scope.documentos.nombreDatosAlmacenados = null;
                $scope.cargandoVisible = false;
            }).error(function (data, status, headers, config) {
                $scope.cargandoVisible = false;
                MostrarNotificacion('alert', (data.resp === 'OK' ? 'success' : 'error'), data.mensaje);
            });
        }
    };

    $scope.nombreDatosAlmacenadosSettings = {
        width: '85%',
        bindingOptions: { value: 'documentos.nombreDatosAlmacenados' },
        maxLength: 150,
        placeholder: '[Nombre de Documentos del Archivo a Subir]'
    };

    $scope.almacenarArchivoSettings = {
        text: 'Almacenar',
        type: 'success',
        width: '15%',
        onClick: function (params) {
            var data = new FormData();

            if ($scope.documentos.nombreDatosAlmacenados) {
                data.append('nombreDocumentos', $scope.documentos.nombreDatosAlmacenados);

                var filesUpload = $('#archivoDocumentos').get(0).files;

                if (filesUpload.length > 0) {
                    for (i = 0; i < filesUpload.length; i++) {
                        data.append("file" + i, filesUpload[i]);
                    }

                    $scope.cargandoVisible = true;

                    $http.post($('#app').data('url') + 'EnCicla/api/ConsultaApi/ArchivoDocumentos', data, { transformRequest: angular.identity, headers: { 'Content-Type': undefined } }).success(function (data, status, headers, config) {
                        $scope.documentos.nombreDatosAlmacenados = null;
                        $("#archivoDocumentos").replaceWith($("#archivoDocumentos").val('').clone(true));

                        documentosSeleccionado = data.id;
                        ObtenerDocumentosAlmacenados();

                        $scope.cargandoVisible = false;
                    }).error(function (data, status, headers, config) {
                        alert(data);
                        alert(status);
                        alert(headers);
                        alert(config);
                        $scope.cargandoVisible = false;
                        MostrarNotificacion('alert', (data.resp === 'OK' ? 'success' : 'error'), data.mensaje);
                    });
                } else {
                    MostrarNotificacion('alert', 'error', 'Archivo Requerido');
                }
            } else {
                MostrarNotificacion('alert', 'error', '\'Nombre de Documentos\' Requerido');
            }
        }
    };

    $scope.estacionesGridSettings = {
        dataSource: estacionesDataSource,
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 500
        },
        pager: {
            showPageSizeSelector: false
        },
        height: 280,
        /*scrolling: {
            mode: 'virtual'
        },*/
        filterRow: {
            visible: false,
        },
        groupPanel: {
            visible: false,
        },
        editing: {
            editEnabled: false,
            removeEnabled: false,
            insertEnabled: false
        },
        selection: {
            mode: 'multiple'
        },
        columns: [
            {
                dataField: 'ID_ESTACION',
                width: '2%',
                caption: '',
                visible: false,
                dataType: 'number',
            },
            {
                dataField: 'ESTACION',
                width: '100%',
                caption: 'ESTACION DESTINO',
                visible: true,
                dataType: 'string',
            },
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData;

            if (data.length > 0) {
                $scope.parametros.estacionesSeleccionadas = $.map(data, function (value) {
                    return value.ESTACION;
                }).join(", ");

                $scope.parametros.estacionesIdSeleccionadas = $.map(data, function (value) {
                    return value.ID_ESTACION;
                }).join(", ");
            } else {
                $scope.parametros.estacionesSeleccionadas = "[Sin Selección]";
                $scope.parametros.estacionesIdSeleccionadas = null;
            }
        }
    };

    $scope.reporteResumidoGridSettings = {
        dataSource: reporteResumidoDataSource,
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 20
        },
        pager: {
            showPageSizeSelector: false
        },
        filterRow: {
            visible: true,
        },
        groupPanel: {
            visible: false,
        },
        "export": {
            enabled: true,
            fileName: "EnCiclaReport",
            allowExportSelectedData: false
        },
        editing: {
            editEnabled: false,
            removeEnabled: false,
            insertEnabled: false
        },
        selection: {
            mode: 'single'
        },
        columns: [
            /*{
                dataField: 'FECHA',
                width: '15%',
                caption: 'FECHA',
                visible: true,
                dataType: 'date'
            },*/
            {
                dataField: 'ESTACION_ORIGEN',
                width: '33%',
                caption: 'ESTACION ORIGEN',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'ESTACION_DESTINO',
                width: '33%',
                caption: 'ESTACION DESTINO',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'DOCUMENTO',
                width: '24%',
                caption: 'DOCUMENTO',
                visible: true,
                dataType: 'string'
            },
            /*{
                dataField: 'USUARIO',
                width: '25%',
                caption: 'USUARIO',
                visible: true,
                dataType: 'string'
            },*/
            {
                dataField: 'CANTIDAD',
                width: '10%',
                caption: 'CANT VIAJES',
                visible: true,
                dataType: 'number'
            }
        ],
        onContentReady: function()
        {
            $scope.cargandoVisible = false;
        }
    };

    $scope.reporteDetalladoGridSettings = {
        dataSource: reporteDetalladoDataSource,
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 20
        },
        pager: {
            showPageSizeSelector: false
        },
        filterRow: {
            visible: true,
        },
        groupPanel: {
            visible: false,
        },
        "export": {
            enabled: true,
            fileName: "EnCiclaReport",
            allowExportSelectedData: false
        },
        editing: {
            editEnabled: false,
            removeEnabled: false,
            insertEnabled: false
        },
        selection: {
            mode: 'single'
        },
        columns: [
            {
                dataField: 'FECHA_PRESTAMO',
                width: '15%',
                caption: 'FECHA PRES',
                visible: true,
                dataType: 'date',
                format: 'yyyy-MM-dd'
            },
            {
                dataField: 'FECHA_DEVOLUCION',
                width: '15%',
                caption: 'FECHA DEV',
                visible: true,
                dataType: 'date',
                format: 'shortDateShortTime'
            },
            {
                dataField: 'ESTACION_ORIGEN',
                width: '27%',
                caption: 'ESTACION ORIGEN',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'ESTACION_DESTINO',
                width: '27%',
                caption: 'ESTACION DESTINO',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'DOCUMENTO',
                width: '15%',
                caption: 'DOCUMENTO',
                visible: true,
                dataType: 'string'
            },
            /*{
                dataField: 'USUARIO',
                width: '30%',
                caption: 'USUARIO',
                visible: true,
                dataType: 'string'
            }*/
        ],
        onContentReady: function () {
            $scope.cargandoVisible = false;
        }
    };

    $scope.consultarSettings = {
        text: 'Consultar',
        type: 'success',
        onClick: function (params) {
            $scope.cargandoVisible = true;

            idDocumentosAlmacenadosSel = 0;
            listaDocumentosSel = null;

            if ($scope.documentos.tipoDocumentos == 0) {
                idDocumentosAlmacenadosSel = $scope.parametros.IDDATOSCONSULTA;
                if (ValidarParametros())
                    RealizarConsulta();
            } else {
                if (ValidarParametros()) {
                    $http.post($('#app').data('url') + 'EnCicla/api/ConsultaApi/ListaDocumentos', { listaDocumentos: $scope.documentos.listaDocumentos }).success(function (data, status, headers, config) {
                        idDocumentosAlmacenadosSel = data;
                        RealizarConsulta();
                    }).error(function (data, status, headers, config) {
                        $scope.tipoReporteSel = null;
                        $scope.cargandoVisible = false;
                        MostrarNotificacion('alert', 'error', 'Error Procesando Documentos');
                    });
                }
            }
        }
    };

    function ValidarParametros()
    {
        fechaInicialSel = $('#datFechaInicial').dxDateBox('instance').option('text');
        fechaFinalSel = $('#datFechaFinal').dxDateBox('instance').option('text');
        horaInicialSel = $('#datHoraInicial').dxDateBox('instance').option('text');
        horaFinalSel = $('#datHoraFinal').dxDateBox('instance').option('text');
        estacionesSel = $scope.parametros.estacionesIdSeleccionadas;
        $scope.parametros.tipoReporteSel = $scope.parametros.tipoReporte;

        if (fechaInicialSel == null) {
            $scope.cargandoVisible = false;
            MostrarNotificacion('alert', 'error', 'Fecha Inicial Requerida');
            return false;
        }

        if (fechaFinalSel == null) {
            $scope.cargandoVisible = false;
            MostrarNotificacion('alert', 'error', 'Fecha Final Requerida');
            return false;
        }

        if (horaInicialSel == null) {
            $scope.cargandoVisible = false;
            MostrarNotificacion('alert', 'error', 'Hora Inicial Requerida');
            return false;
        }

        if (horaFinalSel == null) {
            $scope.cargandoVisible = false;
            MostrarNotificacion('alert', 'error', 'Hora Final Requerida');
            return false;
        }

        if (estacionesSel == null) {
            $scope.cargandoVisible = false;
            MostrarNotificacion('alert', 'error', 'Debe Seleccionar por lo menos una Estación');
            return false;
        }

        return true;
    }

    function RealizarConsulta() {
        if ($scope.parametros.tipoReporte == 0) {
            $('#grdReporteResumido').dxDataGrid({
                dataSource: reporteResumidoDataSource
            });
        } else {
            $('#grdReporteDetallado').dxDataGrid({
                dataSource: reporteDetalladoDataSource
            });
        }

        var tab = $('#tabOpciones').dxTabs('instance');
        tab.option('selectedIndex', 1);
        $scope.selectedTab = 1;
    }

    function ObtenerDocumentosAlmacenados() {
        $scope.cargandoVisible = true;
        $.get($('#app').data('url') + 'EnCicla/api/ConsultaApi/DocumentosAlmacenados', function (data) {
            cboDocumentosAlmacenadosDataSource.store().clear();
            for (var i = 0; i < data.datos.length; i++) {
                cboDocumentosAlmacenadosDataSource.store().insert(data.datos[i]);
            }
            cboDocumentosAlmacenadosDataSource.load();
            $scope.cargandoVisible = false;

            if (documentosSeleccionado != null)
                $scope.parametros.IDDATOSCONSULTA = documentosSeleccionado;

            $scope.$apply();
        }, "json");
    }
});

var cboDocumentosAlmacenadosDataSource = new DevExpress.data.DataSource([]);

var estacionesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var skip = 0;
        var take = 1000;
        $.getJSON($('#app').data('url') + 'EnCicla/api/ConsultaApi/Estaciones').done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + error);
        });
        return d.promise();
    },
    byKey: function (key, extra) {
        return key.toString();
    },
});

var documentosAlmacenadosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var searchValueOptions = loadOptions.searchValue;
        var searchExprOptions = loadOptions.searchExpr;

        var skip = loadOptions.skip;
        var take = loadOptions.take;

        $.getJSON($('#app').data('url') + 'EnCicla/api/ConsultaApi/DocumentosAlmacenados', {
            filter: '',
            sort: '[{"selector":"S_NOMBRE_LOOKUP","desc":false}]',
            group: '',
            skip: skip,
            take: take,
            searchValue: (searchValueOptions === undefined || searchValueOptions === null ? '' : searchValueOptions),
            searchExpr: (searchExprOptions === undefined || searchExprOptions === null ? '' : searchExprOptions),
            comparation: 'contains',
            tipoData: 'l',
            noFilterNoRecords: true
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('falla2a: ' + textStatus + ", " + error);
        });
        return d.promise();
    },
    byKey: function (key, extra) {
        /*return key.toString();*/
        var d = $.Deferred();

        $.getJSON($('#app').data('url') + 'EnCicla/api/ConsultaApi/DocumentosAlmacenados', {
            filter: '',
            sort: '[{"selector":"S_NOMBRE_LOOKUP","desc":false}]',
            group: '',
            skip: 0,
            take: 1,
            searchValue: key,
            searchExpr: 'ID_LOOKUP',
            comparation: '=',
            tipoData: 'l',
            noFilterNoRecords: true
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('falla1: ' + textStatus + ", " + error);
        });
        return d.promise();
    },
});

reporteResumidoDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        if (idDocumentosAlmacenadosSel != null) {
            var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
            var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"ESTACION_DESTINO","desc":false}]';
            var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

            var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
            var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);

            $.getJSON($('#app').data('url') + 'EnCicla/api/ConsultaApi/ConsultaPrestamos', {
                tipoReporte: 0,
                fechaInicial: fechaInicialSel,
                fechaFinal: fechaFinalSel,
                horaInicial: horaInicialSel,
                horaFinal: horaFinalSel,
                estaciones: estacionesSel,
                idDocumentosAlmacenados: idDocumentosAlmacenadosSel,
                //listaDocumentos: listaDocumentosSel,
                filter: filterOptions,
                sort: sortOptions,
                group: groupOptions,
                skip: skip,
                take: take,
                searchValue: '',
                searchExpr: '',
                comparation: '',
                tipoData: 'f',
                noFilterNoRecords: false
            }).done(function (data) {
                d.resolve(data.datos, { totalCount: data.numRegistros });
            }).fail(function (jqxhr, textStatus, error) {
                alert('error cargando datos: ' + textStatus + ", " + error);
            });
            return d.promise();
        } else {
            d.resolve(null, { totalCount: 0 });
            return d.promise();
        }
    },
    byKey: function (key, extra) {
        return key.toString();
    },
});

reporteDetalladoDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        if (idDocumentosAlmacenadosSel != null) {
            var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
            var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"FECHA_DEVOLUCION","desc":false}]';
            var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

            var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
            var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);

            $.getJSON($('#app').data('url') + 'EnCicla/api/ConsultaApi/ConsultaPrestamos', {
                tipoReporte: 1,
                fechaInicial: fechaInicialSel,
                fechaFinal: fechaFinalSel,
                horaInicial: horaInicialSel,
                horaFinal: horaFinalSel,
                estaciones: estacionesSel,
                idDocumentosAlmacenados: idDocumentosAlmacenadosSel,
                //listaDocumentos: listaDocumentosSel,
                filter: filterOptions,
                sort: sortOptions,
                group: groupOptions,
                skip: skip,
                take: take,
                searchValue: '',
                searchExpr: '',
                comparation: '',
                tipoData: 'f',
                noFilterNoRecords: false
            }).done(function (data) {
                d.resolve(data.datos, { totalCount: data.numRegistros });
            }).fail(function (jqxhr, textStatus, error) {
                alert('error cargando datos: ' + textStatus + ", " + error);
            });
            return d.promise();
        } else {
            d.resolve(null, { totalCount: 0 });
            return d.promise();
        }
    },
    byKey: function (key, extra) {
        return key.toString();
    },
});

function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        DevExpress.ui.dialog.alert(msg, 'EnCicla Consulta');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}
