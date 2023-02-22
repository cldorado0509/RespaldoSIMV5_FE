var SIMApp = angular.module('SIM', ['dx']);
var cmSeleccionada = null;
var asuntoSeleccionado = null;
var quejaSeleccionada = null;
var tramiteSeleccionado = null;
var documentoSeleccionado = null;
var tipoSeleccion = null;
var idProgramacion = null;
var windowHeight = $(window).height();
var heightType = 1;
var cargandoDatos = false;

SIMApp.controller("ProgramadorActuacionesController", function ($scope, $location, $http) {
    $('.my-cloak').removeClass('my-cloak');

    if (windowHeight < 700) {
        heightType = 1;
    } else if (windowHeight < 1400) {
        heightType = 2;
    } else {
        heightType = 3;
    }

    $scope.selectedTab = 0;

    $scope.programacion = {};
    $scope.programacion.basadaEn = 0;
    tipoSeleccion = $scope.programacion.basadaEn;
    $scope.cargandoVisible = false;
    $scope.almacenandoVisible = false;
    $scope.itemSeleccionadoQueja = false;
    $scope.itemSeleccionadoCM = false;
    $scope.itemSeleccionadoAsunto = false;
    $scope.programacion.id = 0;
    $scope.programacion.queja = null;
    $scope.programacion.cm = null;
    $scope.programacion.asunto = null;
    $scope.programacion.tramite = null;
    $scope.programacion.documento = null;
    $scope.programacion.tiempo = 0;
    $scope.programacion.tipoTiempo = 3;
    $scope.programacion.tipoFrecuencia = 3;
    $scope.programacion.frecuencia = 1;
    $scope.programacion.repeticiones = null;
    $scope.programacion.tipoActuacion = 0;
    $scope.programacion.tipoPeriodicidad = 0;
    $scope.programacion.fechaNotificacion = null;
    $scope.programacion.observaciones = null;
    $scope.programacion.instalacion = null;
    $scope.programacion.tercero = null;
    $scope.programacion.idAsignacion = null;

    $scope.CargarProgramacion = function () {
        cargandoDatos = true;
        $scope.programacion = {};
        $scope.programacion.basadaEn = 0;
        tipoSeleccion = $scope.programacion.basadaEn;

        $scope.itemSeleccionadoQueja = false;
        $scope.itemSeleccionadoCM = false;
        $scope.itemSeleccionadoAsunto = false;
        $scope.programacion.queja = null;
        $scope.programacion.cm = null;
        $scope.programacion.asunto = null;
        $scope.programacion.tramite = null;
        $scope.programacion.documento = null;
        $scope.programacion.tiempo = 0;
        $scope.programacion.tipoTiempo = 3;
        $scope.programacion.tipoFrecuencia = 3;
        $scope.programacion.frecuencia = 1;
        $scope.programacion.repeticiones = null;
        $scope.programacion.tipoActuacion = 0;
        $scope.programacion.tipoPeriodicidad = 0;
        $scope.programacion.fechaNotificacion = null;
        $scope.programacion.observaciones = null;
        $scope.programacion.instalacion = null;
        $scope.programacion.tercero = null;
        $scope.programacion.idAsignacion = null;

        $scope.cargandoVisible = true;

        $scope.programacion.id = idProgramacion;

        $.getJSON($('#app').data('url') + 'Tramites/api/ProgramadorActuacionesApi/ConsultaProgramacion?id=' + idProgramacion).done(function (data) {
            var gridInstance;

            $scope.programacion.basadaEn = data.programacion.TIPO;
            tipoSeleccion = $scope.programacion.basadaEn;

            $scope.itemSeleccionadoQueja = false;
            $scope.itemSeleccionadoCM = false;
            $scope.itemSeleccionadoAsunto = false;

            if (tipoSeleccion == 1) // CM
            {
                gridInstance = $('#grdCM').dxDataGrid('instance');
                gridInstance.clearFilter();
                gridInstance.clearSelection();

                $('#grdCM').dxDataGrid({
                    dataSource: cmDataSource
                });

                cboCMDataSource.store().clear();
                cboCMDataSource.store().insert({ ID_POPUP: data.programacion.CODIGO_PROYECTO, NOMBRE_POPUP: data.cm });
                cboCMDataSource.load();

                cmSeleccionada = data.programacion.CODIGO_PROYECTO;

                gridInstance = $('#grdAsunto').dxDataGrid('instance');
                gridInstance.clearFilter();
                gridInstance.clearSelection();

                $('#grdAsunto').dxDataGrid({
                    dataSource: asuntoDataSource
                });

                cboAsuntoDataSource.store().clear();
                cboAsuntoDataSource.store().insert({ ID_POPUP: data.programacion.CODIGO_SOLICITUD, NOMBRE_POPUP: data.asunto });
                cboAsuntoDataSource.load();

                asuntoSeleccionado = data.programacion.CODIGO_SOLICITUD;
            }
            else // Queja
            {
                gridInstance = $('#grdQueja').dxDataGrid('instance');
                gridInstance.clearFilter();
                gridInstance.clearSelection();

                $('#grdQueja').dxDataGrid({
                    dataSource: quejaDataSource
                });

                cboQuejaDataSource.store().clear();
                cboQuejaDataSource.store().insert({ ID_POPUP: data.programacion.CODIGO_QUEJA, NOMBRE_POPUP: data.queja });
                cboQuejaDataSource.load();

                quejaSeleccionada = data.programacion.CODIGO_QUEJA;
            }

            $scope.programacion.queja = data.programacion.CODIGO_QUEJA;
            $scope.programacion.cm = data.programacion.CODIGO_PROYECTO;
            $scope.programacion.asunto = data.programacion.CODIGO_SOLICITUD;
            $scope.programacion.tiempo = data.programacion.TIEMPO;
            $scope.programacion.tipoTiempo = data.programacion.TIPO_TIEMPO;
            $scope.programacion.tipoFrecuencia = data.programacion.TIPO_FRECUENCIA;
            $scope.programacion.frecuencia = data.programacion.FRECUENCIA;
            $scope.programacion.repeticiones = data.programacion.REPETICIONES;
            $scope.programacion.tipoActuacion = data.programacion.CODIGO_TIPO_ACTUACION;
            $scope.programacion.tipoPeriodicidad = data.programacion.PERIODICIDAD;
            $scope.programacion.fechaNotificacion = data.programacion.FECHA_NOTIFICACION;
            $scope.programacion.observaciones = data.programacion.OBSERVACIONES;
            $scope.programacion.instalacion = data.programacion.ID_INSTALACION;
            $scope.programacion.tercero = data.programacion.ID_TERCERO;
            $scope.programacion.idAsignacion = data.programacion.ID_ASIGNACION;

            gridInstance = $('#grdTramite').dxDataGrid('instance');
            gridInstance.clearFilter();
            gridInstance.clearSelection();

            $('#grdTramite').dxDataGrid({
                dataSource: tramiteDataSource
            });

            cboTramiteDataSource.store().clear();
            cboTramiteDataSource.store().insert({ ID_POPUP: data.programacion.CODIGO_TRAMITE, NOMBRE_POPUP: data.tramite });
            cboTramiteDataSource.load();

            tramiteSeleccionado = data.programacion.CODIGO_TRAMITE;

            gridInstance = $('#grdDocumento').dxDataGrid('instance');
            gridInstance.clearFilter();
            gridInstance.clearSelection();

            $('#grdDocumento').dxDataGrid({
                dataSource: documentoDataSource
            });

            cboDocumentoDataSource.store().clear();
            cboDocumentoDataSource.store().insert({ ID_POPUP: data.programacion.CODIGO_DOCUMENTO, NOMBRE_POPUP: data.documento });
            cboDocumentoDataSource.load();

            $scope.programacion.tramite = data.programacion.CODIGO_TRAMITE;
            $scope.programacion.documento = data.programacion.CODIGO_DOCUMENTO;

            var tab = $('#tabOpciones').dxTabs('instance');
            tab.option('selectedIndex', 1);
            $scope.selectedTab = 1;

            $scope.cargandoVisible = false;
            $scope.$apply();
            cargandoDatos = false;
        }).fail(function (jqxhr, textStatus, error) {
            $scope.cargandoVisible = false;
            $scope.$apply();
            cargandoDatos = false;
            alert('error cargando datos: ' + textStatus + ", " + error);
        });
    };

    $scope.tabsData = [
            { text: 'Ejecución de Programaciones', pos: 0 },
            { text: 'Programación de Actuaciones', pos: 1 },
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

    $scope.optTipoProgramacionSettings = {
        displayExpr: 'text',
        valueExpr: 'value',
        //dataSource: [{ text: 'CM', value: 1 }, { text: 'Queja', value: 2 }, { text: 'Solicitud', value: 3 }],
        dataSource: [{ text: 'CM', value: 1 }, { text: 'Queja', value: 2 }],
        layout: 'horizontal',
        bindingOptions: { value: 'programacion.basadaEn' },
        onValueChanged: function () {
            /*if ($scope.programacion.basadaEn == 1) {
                $scope.programacion.queja = null;
            } else {
                $scope.programacion.cm = null;
                $scope.programacion.asunto = null;
            }*/

            if (!cargandoDatos) {
                $scope.programacion.queja = null;
                $scope.programacion.cm = null;
                $scope.programacion.asunto = null;

                $scope.programacion.tramite = null;
                $scope.programacion.documento = null;
            }

            tipoSeleccion = $scope.programacion.basadaEn;

            var gridInstance = $('#grdTramite').dxDataGrid('instance');
            gridInstance.clearFilter();

            if ($scope.programacion.basadaEn == 3) {
                $('#grdTramite').dxDataGrid({
                    dataSource: tramiteDataSource
                });
            }
        }
    }

    $scope.optTipoActuacionSettings = {
        displayExpr: 'text',
        valueExpr: 'value',
        dataSource: [{ text: 'Jurídica', value: 1 }, { text: 'Técnica', value: 2 }],
        layout: 'horizontal',
        bindingOptions: { value: 'programacion.tipoActuacion' },
    }

    $scope.optTipoPeriodicidadSettings = {
        displayExpr: 'text',
        valueExpr: 'value',
        dataSource: [{ text: 'Única', value: 1 }, { text: 'Recurrente', value: 2 }],
        layout: 'horizontal',
        bindingOptions: { value: 'programacion.tipoPeriodicidad' }
    }

    $scope.tiempoSettings = {
        width: '100%',
        bindingOptions: { value: 'programacion.tiempo' },
    }

    $scope.repeticionesSettings = {
        width: '100%',
        placeholder: '(0 Sin Finalización)',
        bindingOptions: { value: 'programacion.repeticiones' },
    }

    $scope.TipoTiempoSettings = {
        displayExpr: 'text',
        valueExpr: 'value',
        width: '100%',
        dataSource: [{ text: 'Días', value: 1 }, { text: 'Semanas', value: 2 }, { text: 'Meses', value: 3 }, { text: 'Años', value: 4 }],
        layout: 'horizontal',
        bindingOptions: { value: 'programacion.tipoTiempo' }
    }

    $scope.frecuenciaSettings = {
        width: '100%',
        bindingOptions: { value: 'programacion.frecuencia' },
    }

    $scope.cboTipoFrecuenciaSettings = {
        displayExpr: 'text',
        valueExpr: 'value',
        width: '100%',
        dataSource: [ { text: 'Meses', value: 3 }, { text: 'Años', value: 4 }],
        layout: 'horizontal',
        bindingOptions: { value: 'programacion.tipoFrecuencia' }
    }

    $scope.fechaNotificacionSettings = {
        formatstring: 'yyyy/MM/dd',
        format: 'date',
        useCalendar: true,
        bindingOptions: { value: 'programacion.fechaNotificacion' }
    }

    $scope.observacionesSettings = { 
        bindingOptions: { value: 'programacion.observaciones' },
        height: '80px'
    }

    $scope.seleccionarDocumentoSettings = {
        text: 'Seleccionar Documento',
        type: 'success',
        disabled: true,
        onClick: function (params) {
            if (documentoSeleccionado != null) {
                $scope.itemSeleccionadoDocumento = true;
                $('#grdIndicesDocumento').dxDataGrid({
                    dataSource: indicesDocumentoDataSource
                });
            }

            var popup = $('#popDocumento').dxPopup('instance');
            popup.hide();
        }
    }

    $scope.popQuejaSettings = {
        showTitle: true,
        deferRendering: false,
        title: 'Seleccionar Queja',
        onHidden: function () {
            if ($scope.itemSeleccionadoQueja) {
                cboQuejaDataSource.store().clear();
                cboQuejaDataSource.store().insert({ ID_POPUP: $scope.ID_POPUP, NOMBRE_POPUP: $scope.NOMBRE_POPUP });
                cboQuejaDataSource.load();

                $scope.programacion.queja = $scope.ID_POPUP;
                $scope.programacion.idAsignacion = -1;
                quejaSeleccionada = $scope.programacion.queja;

                $scope.programacion.tramite = null;
                $scope.programacion.documento = null;

                tipoSeleccion = $scope.programacion.basadaEn;
                $('#grdTramite').dxDataGrid({
                    dataSource: tramiteDataSource
                });
            }
            $scope.itemSeleccionadoQueja = false;
        },
    }

    $scope.popCMSettings = {
        showTitle: true,
        deferRendering: false,
        title: 'Seleccionar CM',
        onHidden: function () {
            if ($scope.itemSeleccionadoCM) {
                cboCMDataSource.store().clear();
                cboCMDataSource.store().insert({ ID_POPUP: $scope.ID_POPUP, NOMBRE_POPUP: $scope.NOMBRE_POPUP });
                cboCMDataSource.load();

                $scope.programacion.cm = $scope.ID_POPUP;
                cmSeleccionada = $scope.programacion.cm;
                $scope.programacion.asunto = null;
                $scope.programacion.tramite = null;
                $scope.programacion.documento = null;

                $scope.programacion.instalacion = $scope.instalacionSel;
                $scope.programacion.tercero = $scope.terceroSel;

                $('#grdAsunto').dxDataGrid({
                    dataSource: asuntoDataSource
                });
            }

            $scope.itemSeleccionadoCM = false;
        },
    }

    $scope.popAsuntoSettings = {
        showTitle: true,
        deferRendering: false,
        title: 'Seleccionar Asunto',
        onHidden: function () {
            if ($scope.itemSeleccionadoAsunto) {
                cboAsuntoDataSource.store().clear();
                cboAsuntoDataSource.store().insert({ ID_POPUP: $scope.ID_POPUP, NOMBRE_POPUP: $scope.NOMBRE_POPUP });
                cboAsuntoDataSource.load();

                $scope.programacion.asunto = $scope.ID_POPUP;
                $scope.programacion.idAsignacion = $scope.CODIGO_TIPO_SOLICITUD;
                asuntoSeleccionado = $scope.programacion.asunto;

                $scope.programacion.tramite = null;
                $scope.programacion.documento = null;

                tipoSeleccion = $scope.programacion.basadaEn;
                $('#grdTramite').dxDataGrid({
                    dataSource: tramiteDataSource
                });
            }
            $scope.itemSeleccionadoAsunto = false;
            $scope.itemSeleccionadoCM = false;
        },
    }

    $scope.popTramiteSettings = {
        showTitle: true,
        deferRendering: false,
        title: 'Seleccionar Trámite',
        onHidden: function () {
            if ($scope.itemSeleccionadoTramite) {
                cboTramiteDataSource.store().clear();
                cboTramiteDataSource.store().insert({ ID_POPUP: $scope.ID_POPUP, NOMBRE_POPUP: $scope.NOMBRE_POPUP });
                cboTramiteDataSource.load();

                $scope.programacion.tramite = $scope.ID_POPUP;
                tramiteSeleccionado = $scope.programacion.tramite;

                $('#grdDocumento').dxDataGrid({
                    dataSource: documentoDataSource
                });
            }
            $scope.itemSeleccionadoTramite = false;
        },
    }

    $scope.popDocumentoSettings = {
        showTitle: true,
        fullScreen: false,
        deferRendering: false,
        title: 'Seleccionar Documento',
        onHidden: function () {
            if ($scope.itemSeleccionadoDocumento) {
                cboDocumentoDataSource.store().clear();
                cboDocumentoDataSource.store().insert({ ID_POPUP: $scope.ID_POPUP, NOMBRE_POPUP: $scope.NOMBRE_POPUP });
                cboDocumentoDataSource.load();

                $scope.programacion.documento = $scope.ID_POPUP;
            }
            $scope.itemSeleccionadoDocumento = false;
        },
    }

    $scope.quejaSelectBoxSettings = {
        dataSource: cboQuejaDataSource,
        valueExpr: 'ID_POPUP',
        displayExpr: 'NOMBRE_POPUP',
        placeholder: '[Seleccionar Queja]',
        bindingOptions: { value: 'programacion.queja' },
        onOpened: function () {
            var gridInstance = $('#grdQueja').dxDataGrid('instance');

            //gridInstance.clearFilter();
            gridInstance.clearSelection();

            $('#cboQueja').dxSelectBox('instance').close();
            var popup = $('#popQueja').dxPopup('instance');
            popup.show();
        }
    };

    $scope.cmSelectBoxSettings = {
        dataSource: cboCMDataSource,
        valueExpr: 'ID_POPUP',
        displayExpr: 'NOMBRE_POPUP',
        placeholder: '[Seleccionar CM]',
        bindingOptions: { value: 'programacion.cm' },
        onOpened: function () {
            var gridInstance = $('#grdCM').dxDataGrid('instance');

            //gridInstance.clearFilter();
            gridInstance.clearSelection();

            $('#cboCM').dxSelectBox('instance').close();
            var popup = $('#popCM').dxPopup('instance');
            popup.show();
        }
    };

    $scope.asuntoSelectBoxSettings = {
        dataSource: cboAsuntoDataSource,
        valueExpr: 'ID_POPUP',
        displayExpr: 'NOMBRE_POPUP',
        placeholder: '[Seleccionar Asunto]',
        bindingOptions: { value: 'programacion.asunto' },
        onOpened: function () {
            var gridInstance = $('#grdAsunto').dxDataGrid('instance');

            //gridInstance.clearFilter();
            gridInstance.clearSelection();

            $('#cboAsunto').dxSelectBox('instance').close();
            var popup = $('#popAsunto').dxPopup('instance');
            popup.show();
        }
    };

    $scope.tramiteSelectBoxSettings = {
        dataSource: cboTramiteDataSource,
        valueExpr: 'ID_POPUP',
        displayExpr: 'NOMBRE_POPUP',
        placeholder: '[Seleccionar Tarea]',
        bindingOptions: { value: 'programacion.tramite' },
        onOpened: function () {
            var gridInstance = $('#grdTramite').dxDataGrid('instance');

            //gridInstance.clearFilter();
            gridInstance.clearSelection();

            $('#cboTramite').dxSelectBox('instance').close();
            var popup = $('#popTramite').dxPopup('instance');
            popup.show();
        }
    };

    $scope.documentoSelectBoxSettings = {
        dataSource: cboDocumentoDataSource,
        valueExpr: 'ID_POPUP',
        displayExpr: 'NOMBRE_POPUP',
        placeholder: '[Seleccionar Documento]',
        bindingOptions: { value: 'programacion.documento' },
        onOpened: function () {
            var gridInstance = $('#grdDocumento').dxDataGrid('instance');

            //gridInstance.clearFilter();
            gridInstance.clearSelection();

            documentoSeleccionado = null;
            
            $('#cboDocumento').dxSelectBox('instance').close();
            var popup = $('#popDocumento').dxPopup('instance');
            popup.show();
        }
    };

    $scope.quejaGridSettings = {
        dataSource: quejaDataSource,
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 10
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
                dataField: 'ID_POPUP',
                width: '15%',
                caption: 'CODIGO_QUEJA',
                visible: false,
                dataType: 'number'
            },
            {
                dataField: 'ANO',
                width: '15%',
                caption: 'AÑO',
                visible: true,
                dataType: 'number'
            },
            {
                dataField: 'QUEJA',
                width: '15%',
                caption: 'QUEJA',
                visible: true,
                dataType: 'number'
            },
            {
                dataField: 'NOMBRE_POPUP',
                width: '55%',
                caption: 'ASUNTO',
                visible: true,
                dataType: 'string'
            },
        ],
        onSelectionChanged: function (selecteditems) {
            if (selecteditems != null && selecteditems.selectedRowsData.length > 0) {
                var data = selecteditems.selectedRowsData[0];
                $scope.itemSeleccionadoQueja = true;
                $scope.ID_POPUP = data.ID_POPUP;
                $scope.NOMBRE_POPUP = data.NOMBRE_POPUP;

                var popup = $('#popQueja').dxPopup('instance');
                popup.hide();
            }
        }
    };

    $scope.cmGridSettings = {
        dataSource: cmDataSource,
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 10
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
                dataField: 'N_DOCUMENTO',
                width: '20%',
                caption: 'DOCUMENTO',
                visible: true,
                dataType: 'number'
            },
            {
                dataField: 'S_RSOCIAL',
                width: '30%',
                caption: 'RAZON SOCIAL',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'INSTALACION',
                width: '30%',
                caption: 'INSTALACION',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'CM',
                width: '20%',
                caption: 'CM',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'ID_POPUP',
                width: '2%',
                caption: 'ID_POPUP',
                visible: false,
                dataType: 'number'
            },
            {
                dataField: 'NOMBRE_POPUP',
                width: '2%',
                caption: 'NOMBRE_POPUP',
                visible: false,
                dataType: 'string'
            },
        ],
        onSelectionChanged: function (selecteditems) {
            if (selecteditems != null && selecteditems.selectedRowsData.length > 0) {
                var data = selecteditems.selectedRowsData[0];
                $scope.itemSeleccionadoCM = true;
                $scope.ID_POPUP = data.ID_POPUP;
                $scope.NOMBRE_POPUP = data.NOMBRE_POPUP;
                $scope.instalacionSel = data.ID_INSTALACION;
                $scope.terceroSel = data.ID_TERCERO;

                var popup = $('#popCM').dxPopup('instance');
                popup.hide();
            }
        }
    };

    $scope.asuntoGridSettings = {
        dataSource: null,
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: false
        },
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
            mode: 'single'
        },
        columns: [
            {
                dataField: 'TIPOSOLICITUD',
                width: '20%',
                caption: 'TIPO SOLICITUD',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'NUMERO',
                width: '15%',
                caption: 'SOLICITUD',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'MUNICIPIO',
                width: '30%',
                caption: 'MUNICIPIO',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'CONEXO',
                width: '20%',
                caption: 'CONEXO',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'TRAMO',
                width: '15%',
                caption: 'TRAMO',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'ID_POPUP',
                width: '5%',
                caption: 'CM',
                visible: false,
                dataType: 'string'
            },
            {
                dataField: 'NOMBRE_POPUP',
                width: '5%',
                caption: 'NOMBRE_POPUP',
                visible: false,
                dataType: 'string'
            },
        ],
        onSelectionChanged: function (selecteditems) {
            if (selecteditems != null && selecteditems.selectedRowsData.length > 0) {
                var data = selecteditems.selectedRowsData[0];
                $scope.itemSeleccionadoAsunto = true;
                $scope.ID_POPUP = data.ID_POPUP;
                $scope.NOMBRE_POPUP = data.NOMBRE_POPUP;
                $scope.CODIGO_TIPO_SOLICITUD = data.CODIGO_TIPO_SOLICITUD;

                var popup = $('#popAsunto').dxPopup('instance');
                popup.hide();
            }
        }
    };

    $scope.tramiteGridSettings = {
        dataSource: null,
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 10
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
                dataField: 'ID_POPUP',
                width: '10%',
                caption: 'CODIGO',
                visible: true,
                dataType: 'number'
            },
            {
                dataField: 'TIPOTRAMITE',
                width: '20%',
                caption: 'TIPO TRAMITE',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'NOMBRE_POPUP',
                width: '40%',
                caption: 'COMENTARIOS',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'FECHAINI',
                width: '15%',
                caption: 'FECHA INICIAL',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'FECHAFIN',
                width: '15%',
                caption: 'FECHA FINAL',
                visible: true,
                dataType: 'string'
            },
        ],
        onSelectionChanged: function (selecteditems) {
            if (selecteditems != null && selecteditems.selectedRowsData.length > 0) {
                var data = selecteditems.selectedRowsData[0];
                $scope.itemSeleccionadoTramite = true;
                $scope.ID_POPUP = data.ID_POPUP;
                $scope.NOMBRE_POPUP = data.ID_POPUP + ' - ' + data.NOMBRE_POPUP;

                var popup = $('#popTramite').dxPopup('instance');
                popup.hide();
            }
        }
    };

    $scope.documentoGridSettings = {
        dataSource: null,
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: (heightType == 1 ? 4 : (heightType == 2 ? 7 : 10)),
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
            editEnabled: false,
            removeEnabled: false,
            insertEnabled: false
        },
        selection: {
            mode: 'single'
        },
        columns: [
            {
                dataField: 'ID_POPUP',
                width: '10%',
                caption: 'CODIGO',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'NOMBRE_POPUP',
                width: '90%',
                caption: 'TIPO DOCUMENTAL',
                visible: true,
                dataType: 'string'
            },
        ],
        onSelectionChanged: function (selecteditems) {
            if (selecteditems != null && selecteditems.selectedRowsData.length > 0) {
                var data = selecteditems.selectedRowsData[0];
                //$scope.itemSeleccionadoDocumento = true;
                $scope.ID_POPUP = data.ID_POPUP;
                $scope.NOMBRE_POPUP = data.NOMBRE_POPUP;

                documentoSeleccionado = $scope.ID_POPUP;

                $('#grdIndicesDocumento').dxDataGrid({
                    dataSource: indicesDocumentoDataSource
                });

                $('#btnSeleccionarDocumento').dxButton({
                    disabled: false
                });

                /*var popup = $('#popDocumento').dxPopup('instance');
                popup.hide();*/
            }
        }
    };

    $scope.indicesDocumentoGridSettings = {
        dataSource: null,
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: (heightType == 1 ? 3 : (heightType == 2 ? 5 : 7)),
        },
        pager: {
            showPageSizeSelector: false
        },
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
            mode: 'single'
        },
        columns: [
            {
                dataField: 'ORDEN',
                width: '5%',
                caption: 'ORDEN',
                visible: false,
                dataType: 'number'
            },
            {
                dataField: 'INDICE',
                width: '30%',
                caption: 'INDICE',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'VALOR',
                width: '70%',
                caption: 'VALOR',
                visible: true,
                dataType: 'string'
            },
        ],
    };

    $scope.ejecucionGridSettings = {
        dataSource: programacionesDataSource,
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 20, 50]
        },
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
            mode: 'single'
        },
        columns: [
            {
                dataField: 'TIPO',
                width: '20%',
                caption: 'BASADO EN',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'TIPO_ACTUACION',
                width: '30%',
                caption: 'TIPO ACTUACION',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'FECHA_NOTIFICACION',
                width: '20%',
                caption: 'FECHA NOTIFICACION',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'FECHA_PROXIMA_EJECUCION',
                width: '20%',
                caption: 'FECHA PROX EJECUCION',
                visible: true,
                dataType: 'string'
            },
            {
                caption: '',
                width: '10%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div />').dxButton(
                        {
                            icon: '',
                            text: 'Editar',
                            type: 'normal',
                            onClick: function (params) {
                                idProgramacion = options.data.ID_PROGRAMACION;
                                $scope.CargarProgramacion();
                            }
                        }
                        ).appendTo(container);
                }
            },
        ],
    };

    $scope.btnAlmacenarSettings = {
        text: 'Almacenar',
        type: 'success',
        width: '30%',
        //bindingOptions: { disabled: '!DatosValidos()' },
        onClick: function (params) {

            /*$('#btnAlmacenar').dxButton({
                disabled: true
            });*/

            if (!$scope.DatosValidos())
            {
                MostrarNotificacion('alert', 'error', 'Datos Inválidos o Incompletos.');
                return;
            }

            $scope.almacenandoVisible = true;
            $http.post($('#app').data('url') + 'Tramites/api/ProgramadorActuacionesApi/ProgramacionActuacion', JSON.stringify($scope.programacion)).success(function (data, status, headers, config) {
                $scope.almacenandoVisible = false;
                MostrarNotificacion('alert', (data.resp === 'OK' ? 'success' : 'error'), data.mensaje);

                if (data.resp === 'OK') {
                    $scope.programacion = {};
                    $scope.programacion.basadaEn = 0;
                    tipoSeleccion = $scope.programacion.basadaEn;

                    $scope.itemSeleccionadoQueja = false;
                    $scope.itemSeleccionadoCM = false;
                    $scope.itemSeleccionadoAsunto = false;
                    $scope.programacion.id = 0;
                    $scope.programacion.queja = null;
                    $scope.programacion.cm = null;
                    $scope.programacion.asunto = null;
                    $scope.programacion.tramite = null;
                    $scope.programacion.documento = null;
                    $scope.programacion.tiempo = 0;
                    $scope.programacion.tipoTiempo = 3;
                    $scope.programacion.tipoFrecuencia = 3;
                    $scope.programacion.frecuencia = 1;
                    $scope.programacion.repeticiones = null;
                    $scope.programacion.tipoActuacion = 0;
                    $scope.programacion.tipoPeriodicidad = 0;
                    $scope.programacion.fechaNotificacion = null;
                    $scope.programacion.observaciones = null;
                    $scope.programacion.instalacion = null;
                    $scope.programacion.tercero = null;

                    $('#grdEjecucion').dxDataGrid({
                        dataSource: programacionesDataSource
                    });
                }
                /*$('#btnAlmacenar').dxButton({
                    disabled: false
                });*/
            }).error(function (data, status, headers, config) {
                $scope.almacenandoVisible = false;
                MostrarNotificacion('alert', (data.resp === 'OK' ? 'success' : 'error'), data.mensaje);
                /*$('#btnAlmacenar').dxButton({
                    disabled: false
                });*/
            });
        }
    };

    $scope.btnCancelarSettings = {
        text: 'Cancelar',
        type: 'danger',
        width: '30%',
        onClick: function (params) {
            $scope.programacion = {};
            $scope.programacion.basadaEn = 0;
            tipoSeleccion = $scope.programacion.basadaEn;

            $scope.itemSeleccionadoQueja = false;
            $scope.itemSeleccionadoCM = false;
            $scope.itemSeleccionadoAsunto = false;
            $scope.programacion.id = 0;
            $scope.programacion.queja = null;
            $scope.programacion.cm = null;
            $scope.programacion.asunto = null;
            $scope.programacion.tramite = null;
            $scope.programacion.documento = null;
            $scope.programacion.tiempo = 0;
            $scope.programacion.tipoTiempo = 3;
            $scope.programacion.tipoFrecuencia = 3;
            $scope.programacion.frecuencia = 1;
            $scope.programacion.repeticiones = null;
            $scope.programacion.tipoActuacion = 0;
            $scope.programacion.tipoPeriodicidad = 0;
            $scope.programacion.fechaNotificacion = null;
            $scope.programacion.observaciones = null;
            $scope.programacion.instalacion = null;
            $scope.programacion.tercero = null;
        }
    };

    $scope.DatosValidos = function () {
        return (
            (
                ($scope.programacion.basadaEn == 1 && $scope.programacion.cm != null && $scope.programacion.asunto != null) ||
                ($scope.programacion.basadaEn == 2 && $scope.programacion.queja != null) ||
                ($scope.programacion.basadaEn == 3)
            ) &&
            $scope.programacion.tramite != null &&
            $scope.programacion.documento != null &&
            $scope.programacion.tiempo > 0 &&
            $scope.programacion.tipoTiempo != 0 &&
            $scope.programacion.tipoActuacion != 0 &&
            $scope.programacion.fechaNotificacion != null
        );
    }
});

quejaDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"ANO","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'Tramites/api/ProgramadorActuacionesApi/ConsultaQuejas', {
            filter: filterOptions,
            sort: sortOptions,
            group: groupOptions,
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
            alert('error cargando datos: ' + textStatus + ", " + error);
        });
        return d.promise();
    },
    byKey: function (key, extra) {
        return key.toString();
    },
});

var cboQuejaDataSource = new DevExpress.data.DataSource([]);

cmDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"S_RSOCIAL","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'Tramites/api/ProgramadorActuacionesApi/ConsultaCM', {
            filter: filterOptions,
            sort: sortOptions,
            group: groupOptions,
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
            alert('error cargando datos: ' + textStatus + ", " + error);
        });
        return d.promise();
    },
    byKey: function (key, extra) {
        return key.toString();
    },
});

var cboCMDataSource = new DevExpress.data.DataSource([]);

asuntoDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"NUMERO","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'Tramites/api/ProgramadorActuacionesApi/ConsultaAsuntos', {
            cm: cmSeleccionada,
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
    },
    byKey: function (key, extra) {
        return key.toString();
    },
});

var cboAsuntoDataSource = new DevExpress.data.DataSource([]);

tramiteDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var tipo = (tipoSeleccion == 1 ? cmSeleccionada : (tipoSeleccion == 2 ? quejaSeleccionada : 0));

        if (tipo != null) {
            var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
            var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"ID_POPUP","desc":false}]';
            var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

            var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
            var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
            $.getJSON($('#app').data('url') + 'Tramites/api/ProgramadorActuacionesApi/ConsultaTramites', {
                tipo: tipoSeleccion,
                idTipo: (tipoSeleccion == 1 ? cmSeleccionada : (tipoSeleccion == 2 ? quejaSeleccionada : 0)),
                filter: filterOptions,
                sort: sortOptions,
                group: groupOptions,
                skip: skip,
                take: take,
                searchValue: '',
                searchExpr: '',
                comparation: '',
                tipoData: 'f',
                noFilterNoRecords: tipoSeleccion == 3 ? true : false
            }).done(function (data) {
                d.resolve(data.datos, { totalCount: data.numRegistros });
            }).fail(function (jqxhr, textStatus, error) {
                alert('error cargando datos: ' + textStatus + ", " + error);
            });
            return d.promise();
        } else {
            return null;
        }
    },
    byKey: function (key, extra) {
        return key.toString();
    },
});

var cboTramiteDataSource = new DevExpress.data.DataSource([]);

documentoDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"ID_POPUP","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'Tramites/api/ProgramadorActuacionesApi/ConsultaDocumentos', {
            tramite: tramiteSeleccionado,
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
    },
    byKey: function (key, extra) {
        return key.toString();
    },
});

var cboDocumentoDataSource = new DevExpress.data.DataSource([]);

indicesDocumentoDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"ORDEN","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'Tramites/api/ProgramadorActuacionesApi/ConsultaIndicesDocumentos', {
            tramite: tramiteSeleccionado,
            documento: documentoSeleccionado,
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
    },
    byKey: function (key, extra) {
        return key.toString();
    },
});

programacionesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"ID_PROGRAMACION","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'Tramites/api/ProgramadorActuacionesApi/ConsultaProgramaciones', {
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
    },
    byKey: function (key, extra) {
        return key.toString();
    },
});

var cboDocumentoDataSource = new DevExpress.data.DataSource([]);

function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        DevExpress.ui.dialog.alert(msg, 'Programación de Actuaciones');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}
