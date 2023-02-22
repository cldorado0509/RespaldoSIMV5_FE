var SIMApp = angular.module('SIM', ['dx']);

var documentoSeleccionado = null;
var documentoSeleccionadoBusqueda = null;
var formularioSeleccionado = null;
var itemSeleccionado = null;
var tipoaActoSeleccionado = null;
var registroActual = null;

SIMApp.controller("ActuacionJuridicaController", function ($scope, $location, $http) {
    $('.my-cloak').removeClass('my-cloak');

    $scope.selectedTab = 0;
    $scope.cargandoVisible = true;
    $scope.almacenandoVisible = false;

    $scope.actuacion = {};
    $scope.actuacion.tercero = null;
    $scope.actuacion.instalacion = null;
    $scope.actuacion.cm = null;
    $scope.actuacion.instalacioncm = null;
    $scope.actuacion.documentoBusqueda = null;
    $scope.actuacion.documento = null;
    $scope.actuacion.terceroIndice = null;
    $scope.actuacion.instalacionIndice = null;
    $scope.actuacion.cmIndice = null;
    $scope.actuacion.tipoIndice = null;
    $scope.actuacion.formulario = null;
    $scope.actuacion.item = null;
    $scope.actuacion.nuevoItem = null;
    $scope.actuacion.asuntoActuacion = null;
    $scope.actuacion.tipoActo = null;
    $scope.actuacion.serie = null;
    $scope.actuacion.serieBusqueda = null;

    $scope.actuacion.documentoNombreBusqueda = null;
    $scope.documentoBusquedaSeleccionado = false;

    $scope.itemSeleccionadoInstalacion = false;
    $scope.itemSeleccionadoDocumento = false;
    $scope.itemSeleccionadoDocumentoBusqueda = false;
    $scope.formularioSeleccionado = false;
    $scope.tipoActoSeleccionado = false;

    $scope.actuacion.actuacionSeleccionada = false;
    $scope.almacenar = false;

    $scope.estado = null;
    $scope.idActuacion = null;

    $scope.tipoPrecargado = false;

    $scope.tipoConsulta = 1;

    $.get($('#app').data('url') + 'Tramites/api/ActuacionJuridicaApi/Formulario', function (data) {
        for (var i = 0; i < data.datos.length; i++) {
            cboFormularioDataSource.store().insert(data.datos[i]);
        }
        cboFormularioDataSource.load();
        $scope.cargandoVisible = false;
        $scope.$apply();
    }, "json");

    $scope.tabsData = [
            { text: 'Actuación Jurídica', pos: 0 },
            { text: 'Características', pos: 1 },
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

    $scope.terceroIndiceSettings = {
        readOnly: true,
        bindingOptions: { value: 'actuacion.terceroIndice' },
    }

    $scope.instalacionIndiceSettings = {
        readOnly: true,
        bindingOptions: { value: 'actuacion.instalacionIndice' },
    }

    $scope.cmIndiceSettings = {
        readOnly: true,
        bindingOptions: { value: 'actuacion.cmIndice' },
    }

    $scope.tipoIndiceSettings = {
        readOnly: true,
        bindingOptions: { value: 'actuacion.tipoIndice' },
    }

    $scope.popInstalacionSettings = {
        showTitle: true,
        deferRendering: false,
        title: 'Seleccionar Instalación - CM',
        onHidden: function () {
            if ($scope.itemSeleccionadoInstalacion) {
                $scope.actuacion.actuacionSeleccionada = false;

                cboInstalacionDataSource.store().clear();
                cboInstalacionDataSource.store().insert({ ID_POPUP: $scope.ID_POPUP, NOMBRE_POPUP: $scope.NOMBRE_POPUP });
                cboInstalacionDataSource.load();

                var datosInstalacion = $scope.ID_POPUP.split(',');
                $scope.actuacion.instalacioncm = $scope.ID_POPUP;
                $scope.actuacion.instalacion = datosInstalacion[0];
                $scope.actuacion.cm = (datosInstalacion[1] == '' ? null : datosInstalacion[1]);

                if ($scope.actuacion.formulario != null) {
                    $scope.cargandoVisible = true;
                    $.get($('#app').data('url') + 'Tramites/api/ActuacionJuridicaApi/Items?idFormulario=' + $scope.actuacion.formulario + '&idTercero=' + $scope.actuacion.tercero + '&idInstalacion=' + $scope.actuacion.instalacion, function (data) {
                        $scope.actuacion.item = null;
                        itemSeleccionado = null;

                        cboItemDataSource.store().clear();
                        if (data.numRegistros > 0) {
                            for (var i = 0; i < data.datos.length; i++) {
                                cboItemDataSource.store().insert(data.datos[i]);
                            }
                        }

                        cboItemDataSource.load();
                        $scope.cargandoVisible = false;
                        $scope.$apply();
                    }, "json");
                }
            }

            $scope.itemSeleccionadoInstalacion = false;
        },
    }

    $scope.popDocumentoBusquedaSettings = {
        showTitle: true,
        deferRendering: false,
        title: 'Seleccionar Documento',
        onHidden: function () {
            if ($scope.itemSeleccionadoDocumentoBusqueda) {
                cboDocumentoBusquedaDataSource.store().clear();
                cboDocumentoBusquedaDataSource.store().insert({ ID_POPUP: $scope.ID_POPUP, NOMBRE_POPUP: $scope.NOMBRE_POPUP });
                cboDocumentoBusquedaDataSource.load();

                $scope.actuacion.documentoBusqueda = $scope.ID_POPUP;
                $scope.actuacion.documentoNombreBusqueda = $scope.NOMBRE_POPUP;
                $scope.documentoBusquedaSeleccionado = true;
                documentoBusquedaSeleccionado = $scope.actuacion.documentoBusqueda;
            }
            $scope.itemSeleccionadoDocumentoBusqueda = false;
        },
    };

    $scope.popDocumentoSettings = {
        showTitle: true,
        deferRendering: false,
        title: 'Seleccionar Documento',
        onHidden: function () {
            if ($scope.itemSeleccionadoDocumento) {
                cboDocumentoDataSource.store().clear();
                cboDocumentoDataSource.store().insert({ ID_POPUP: $scope.ID_POPUP, NOMBRE_POPUP: $scope.NOMBRE_POPUP });
                cboDocumentoDataSource.load();

                $scope.actuacion.documento = $scope.ID_POPUP;
                documentoSeleccionado = $scope.actuacion.documento;
            }
            $scope.itemSeleccionadoDocumento = false;
        },
    };

    $scope.instalacionSelectBoxSettings = {
        dataSource: cboInstalacionDataSource,
        valueExpr: 'ID_POPUP',
        displayExpr: 'NOMBRE_POPUP',
        placeholder: '[Seleccionar Instalación - CM]',
        bindingOptions: { value: 'actuacion.instalacioncm' },
        onOpened: function () {
            $('#cboInstalacion').dxSelectBox('instance').close();
            var popup = $('#popInstalacion').dxPopup('instance');
            popup.show();
        }
    };

    $scope.documentoBusquedaSelectBoxSettings = {
        dataSource: cboDocumentoBusquedaDataSource,
        valueExpr: 'ID_POPUP',
        displayExpr: 'NOMBRE_POPUP',
        placeholder: '[Seleccionar Documento]',
        width: '85%',
        bindingOptions: { value: 'actuacion.documentoBusqueda' },
        onOpened: function () {
            $('#cboDocumentoBusqueda').dxSelectBox('instance').close();
            var popup = $('#popDocumentoBusqueda').dxPopup('instance');
            popup.show();
        },
    };

    $scope.documentoSelectBoxSettings = {
        dataSource: cboDocumentoDataSource,
        valueExpr: 'ID_POPUP',
        displayExpr: 'NOMBRE_POPUP',
        placeholder: '[Seleccionar Documento]',
        width: '85%',
        bindingOptions: { value: 'actuacion.documento', disabled: '!almacenar' },
        onOpened: function () {
            $('#cboDocumento').dxSelectBox('instance').close();
            var popup = $('#popDocumento').dxPopup('instance');
            popup.show();
        },
        onValueChanged: function () {
            if (!$scope.tipoPrecargado) {
                if ($scope.actuacion.serie) {
                    $scope.cargandoVisible = true;

                    $.get($('#app').data('url') + 'Tramites/api/ActuacionJuridicaApi/TiposActoFormulario?idSerie=' + $scope.actuacion.serie + '&idFormulario=' + $scope.actuacion.formulario, function (data) {
                        cboTipoActoDataSource.store().clear();
                        if (data.numRegistros > 0) {
                            for (var i = 0; i < data.datos.length; i++) {
                                cboTipoActoDataSource.store().insert(data.datos[i]);
                            }
                        }

                        cboTipoActoDataSource.load();
                        $scope.cargandoVisible = false;

                        $scope.$apply();

                    }, "json");
                } else {
                    cboTipoActoDataSource.store().clear();
                    cboTipoActoDataSource.load();

                    $scope.actuacion.tipoActo = null;
                    $scope.$apply();
                }
            }

            $scope.tipoPrecargado = false;
        },
    };

    $scope.instalacionGridSettings = {
        dataSource: instalacionDataSource,
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
                width: '5%',
                caption: 'ID_POPUP',
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
            var data = selecteditems.selectedRowsData[0];
            $scope.itemSeleccionadoInstalacion = true;
            $scope.ID_POPUP = data.ID_POPUP;
            $scope.NOMBRE_POPUP = data.NOMBRE_POPUP;
            $scope.actuacion.tercero = data.ID_TERCERO;

            var popup = $('#popInstalacion').dxPopup('instance');
            popup.hide();
        }
    };

    $scope.documentoBusquedaGridSettings = {
        dataSource: documentoBusquedaDataSource,
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
                width: '2%',
                caption: '',
                visible: false,
                dataType: 'string'
            },
            {
                dataField: 'TIPO_DOCUMENTO',
                width: '20%',
                caption: 'TIPO',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'ANO',
                width: '15%',
                caption: 'AÑO',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'NUMERO',
                width: '15%',
                caption: 'NUMERO',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'ASUNTO',
                width: '40%',
                caption: 'ASUNTO',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'EMPRESA',
                width: '30%',
                caption: 'EMPRESA',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'NOMBRE_POPUP',
                width: '2%',
                caption: 'DESCRIPCION',
                visible: false,
                dataType: 'string'
            },
            {
                dataField: 'TIPO',
                width: '2%',
                caption: 'TIPO',
                visible: false,
                dataType: 'string'
            },
        ],
        onSelectionChanged: function (selecteditems) {
            var data = selecteditems.selectedRowsData[0];
            $scope.itemSeleccionadoDocumentoBusqueda = true;
            $scope.ID_POPUP = data.ID_POPUP;
            $scope.NOMBRE_POPUP = data.NOMBRE_POPUP;
            $scope.actuacion.serieBusqueda = data.CODSERIE;

            var popup = $('#popDocumentoBusqueda').dxPopup('instance');
            popup.hide();
            //alert(JSON.stringify(data));
            $scope.actuacion.terceroIndice = data.EMPRESA;
            $scope.actuacion.cmIndice = data.CM;
            $scope.actuacion.tipoIndice = data.TIPO;
        }
    };

    $scope.documentoGridSettings = {
        dataSource: documentoDataSource,
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
                width: '2%',
                caption: '',
                visible: false,
                dataType: 'string'
            },
            {
                dataField: 'TIPO_DOCUMENTO',
                width: '20%',
                caption: 'TIPO',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'ANO',
                width: '15%',
                caption: 'AÑO',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'NUMERO',
                width: '15%',
                caption: 'NUMERO',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'ASUNTO',
                width: '40%',
                caption: 'ASUNTO',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'EMPRESA',
                width: '30%',
                caption: 'EMPRESA',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'NOMBRE_POPUP',
                width: '2%',
                caption: 'DESCRIPCION',
                visible: false,
                dataType: 'string'
            },
            {
                dataField: 'TIPO',
                width: '2%',
                caption: 'TIPO',
                visible: false,
                dataType: 'string'
            },
        ],
        onSelectionChanged: function (selecteditems) {
            var data = selecteditems.selectedRowsData[0];
            $scope.itemSeleccionadoDocumento = true;
            $scope.ID_POPUP = data.ID_POPUP;
            $scope.NOMBRE_POPUP = data.NOMBRE_POPUP;
            $scope.actuacion.serie = data.CODSERIE;

            var popup = $('#popDocumento').dxPopup('instance');
            popup.hide();

            //var datosDocumento = $scope.ID_POPUP.split(',');

            //$scope.actuacion.terceroIndice = data.EMPRESA;
            //$scope.actuacion.instalacionIndice = data.instalacionIndice;
            //$scope.actuacion.cmIndice = data.CM;
            //$scope.actuacion.tipoIndice = data.TIPO;
        }
    };

    $scope.actuacionesItemGridSettings = {
        dataSource: actuacionesItemDataSource,
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
                dataField: 'ID_ESTADO',
                width: '2%',
                caption: '',
                visible: false,
                dataType: 'number',
                cssClass: 'actuacionCell',
            },
            {
                dataField: 'ID_VISITA',
                width: '7%',
                caption: 'VISITA',
                visible: true,
                dataType: 'number',
                cssClass: 'actuacionCell',
            },
            {
                dataField: 'ASUNTO',
                width: '43%',
                caption: 'ASUNTO',
                visible: true,
                dataType: 'string',
                cssClass: 'actuacionCell',
            },
            {
                dataField: 'TIPO_VISITA',
                width: '20%',
                caption: 'TIPO ACTUACION',
                visible: true,
                dataType: 'string',
                cssClass: 'actuacionCell',
            },
            {
                dataField: 'ID_ESTADOVISITA',
                width: '2%',
                caption: '',
                visible: false,
                dataType: 'number',
                cssClass: 'actuacionCell',
            },
            {
                dataField: 'ESTADO',
                width: '15%',
                caption: 'ESTADO',
                visible: true,
                dataType: 'string',
                cssClass: 'actuacionCell',
            },
            {
                caption: '',
                width: '20%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div />').dxButton(
                        {
                            icon: '',
                            text: 'Características',
                            type: 'success',
                            onClick: function (params) {
                                registroActual = options.data;
                                CargarCaracteristicas(registroActual);
                            }
                        }
                        ).appendTo(container);
                }
            },
        ],
    };

    $scope.formularioSelectBoxSettings = {
        dataSource: cboFormularioDataSource,
        valueExpr: 'ID_FORMULARIO',
        displayExpr: 'S_NOMBRE',
        placeholder: '[Seleccionar Tipo]',
        bindingOptions: { value: 'actuacion.formulario' },
        onValueChanged: function () {
            $scope.actuacion.actuacionSeleccionada = false;
            $scope.actuacion.item = null;
            itemSeleccionado = null;
            formularioSeleccionado = $scope.actuacion.formulario;
            $scope.formularioSeleccionado = true;
            $scope.cargandoVisible = true;
            $.get($('#app').data('url') + 'Tramites/api/ActuacionJuridicaApi/Items?idFormulario=' + $scope.actuacion.formulario + '&idTercero=' + $scope.actuacion.tercero + '&idInstalacion=' + $scope.actuacion.instalacion, function (data) {
                cboItemDataSource.store().clear();
                if (data.numRegistros > 0) {
                    for (var i = 0; i < data.datos.length; i++) {
                        cboItemDataSource.store().insert(data.datos[i]);
                    }
                }

                cboItemDataSource.load();
                $scope.cargandoVisible = false;
                $scope.$apply();
            }, "json");
        },
    };

    $scope.tipoActoSelectBoxSettings = {
        dataSource: cboTipoActoDataSource,
        valueExpr: 'ID_TIPOACTO',
        displayExpr: 'S_DESCRIPCION',
        placeholder: '[Tipo Acto]',
        width: '85%',
        bindingOptions: { value: 'actuacion.tipoActo', disabled: '!almacenar' },
    };

    $scope.itemSelectBoxSettings = {
        dataSource: cboItemDataSource,
        placeholder: "[Seleccionar Item]",
        displayExpr: "NOMBRE",
        valueExpr: "ID",
        searchEnabled: false,
        searchExpr: "S_NOMBRE",
        searchTimeout: 0,
        width: '100%',
        bindingOptions: { value: 'actuacion.item' },
        onValueChanged: function (e) {
            //if ($scope.actuacion.item != null)
            //    $scope.actuacion.asuntoActuacion = $('#cboItemSelectBox').dxSelectBox('instance').text;
            $scope.actuacion.actuacionSeleccionada = false;
            itemSeleccionado = $scope.actuacion.item;

            $scope.actuacion.asuntoActuacion = $scope.actuacion.tipoIndice;

            //actuacionesItemDataSource.load();
            $('#grdActuacionesItem').dxDataGrid({
                dataSource: actuacionesItemDataSource
            });
        },
    };

    $scope.nuevoItemSettings = {
        width: '85%',
        bindingOptions: { value: 'actuacion.nuevoItem' },
    };

    $scope.asuntoActuacionSettings = {
        width: '78%',
        bindingOptions: { value: 'actuacion.asuntoActuacion' },
    };

    $scope.crearItemSettings = {
        text: 'Crear Item',
        type: 'success',
        width: '15%',
        onClick: function (params) {
            if ($scope.actuacion.nuevoItem != null) {
                $scope.cargandoVisible = true;
                $.get($('#app').data('url') + 'Tramites/api/ActuacionJuridicaApi/CrearItem?idTercero=' + $scope.actuacion.tercero + '&idInstalacion=' + $scope.actuacion.instalacion + '&idFormulario=' + $scope.actuacion.formulario + '&nombre=' + $scope.actuacion.nuevoItem, function (data) {
                    if (data.resp == 'Error') {
                        $scope.actuacion.item = null;
                        $scope.cargandoVisible = false;

                        MostrarNotificacion('alert', 'error', 'No se pudo crear el Item. ' + data.mensaje);
                    } else {
                        var itemSeleccionadoNuevo = data.id;
                        $.get($('#app').data('url') + 'Tramites/api/ActuacionJuridicaApi/Items?idFormulario=' + $scope.actuacion.formulario + '&idTercero=' + $scope.actuacion.tercero + '&idInstalacion=' + $scope.actuacion.instalacion, function (data) {
                            cboItemDataSource.store().clear();
                            if (data.numRegistros > 0) {
                                for (var i = 0; i < data.datos.length; i++) {
                                    cboItemDataSource.store().insert(data.datos[i]);
                                }
                            }

                            cboItemDataSource.load();
                            $scope.cargandoVisible = false;

                            $scope.actuacion.item = itemSeleccionadoNuevo;
                            itemSeleccionado = $scope.actuacion.item;
                            $scope.actuacion.nuevoItem = null;

                            actuacionesItemDataSource.load();
                            $scope.$apply();
                        }, "json");
                    }
                    cboItemDataSource.store().clear();
                    if (data.numRegistros > 0) {
                        for (var i = 0; i < data.datos.length; i++) {
                            cboItemDataSource.store().insert(data.datos[i]);
                        }
                    }

                    cboItemDataSource.load();
                    $scope.cargandoVisible = false;
                    $scope.$apply();
                }, "json");
            } else {
                MostrarNotificacion('alert', 'error', 'Descripción de Item Requerida.');
            }
        }
    };

    $scope.cargarDocumentoSettings = {
        text: 'Asignar',
        type: 'success',
        width: '15%',
        bindingOptions: { disabled: '!almacenar' },
        onClick: function (params) {
            if ($scope.documentoBusquedaSeleccionado) {
                cboDocumentoDataSource.store().clear();
                cboDocumentoDataSource.store().insert({ ID_POPUP: $scope.actuacion.documentoBusqueda, NOMBRE_POPUP: $scope.actuacion.documentoNombreBusqueda });
                cboDocumentoDataSource.load();

                $scope.actuacion.documento = $scope.actuacion.documentoBusqueda;
                documentoSeleccionado = $scope.actuacion.documento;
                $scope.actuacion.serie = $scope.actuacion.serieBusqueda;
            }
        }
    };

    $scope.verDocumentoBusquedaSettings = {
        text: 'Descargar',
        type: 'success',
        width: '15%',
        onClick: function (params) {
            if ($scope.documentoBusquedaSeleccionado) {
                VerDocumento($scope.actuacion.documentoBusqueda.split(',')[0], $scope.actuacion.documentoBusqueda.split(',')[1]);
            }
        }
    };

    $scope.verDocumentoSettings = {
        text: 'Descargar',
        type: 'success',
        width: '15%',
        onClick: function (params) {
            if ($scope.actuacion.documento) {
                VerDocumento($scope.actuacion.documento.split(',')[0], $scope.actuacion.documento.split(',')[1]);
            }
        }
    };

    $scope.nuevaActuacionSettings = {
        text: 'Crear Actuación',
        type: 'success',
        width: '15%',
        onClick: function (params) {
            if ($scope.actuacion.asuntoActuacion != null && $scope.actuacion.asuntoActuacion.trim() != '') {
                $scope.cargandoVisible = true;
                $.get($('#app').data('url') + 'Tramites/api/ActuacionJuridicaApi/CrearActuacion?idTercero=' + $scope.actuacion.tercero + '&idInstalacion=' + $scope.actuacion.instalacion + '&idFormulario=' + $scope.actuacion.formulario + '&idItem=' + $scope.actuacion.item + '&asunto=' + $scope.actuacion.asuntoActuacion, function (data) {
                    if (data.resp == 'Error') {
                        $scope.cargandoVisible = false;
                        MostrarNotificacion('alert', 'error', 'No se pudo crear la Actuación. ' + data.mensaje);
                    } else {
                        $scope.actuacion.asuntoActuacion = null;
                        $('#grdActuacionesItem').dxDataGrid({
                            dataSource: actuacionesItemDataSource
                        });
                        $scope.cargandoVisible = false;
                        $scope.$apply();
                    }
                }, "json");
            } else {
                MostrarNotificacion('alert', 'error', 'Asunto Requerido para Crear la Actuación.');
            }
        }
    };

    $scope.tipoSeleccionActiva = function (tipo) {
        if (tipo == 1 && $scope.actuacion.tercero != null)
            return true;
        else if (tipo == 2 && $scope.actuacion.tercero != null && $scope.actuacion.formulario != null)
            return true;
        else if (tipo == 3 && $scope.actuacion.tercero != null && $scope.actuacion.formulario != null && $scope.actuacion.item != null)
            return true;
        else
            return false;
    };

    $scope.almacenarCaracteristicasSettings = {
        text: 'Almacenar',
        type: 'success',
        width: '50%',
        onClick: function (params) {
            AlmacenarCaracteristicas(false);
        }
    };

    $scope.finalizarCaracteristicasSettings = {
        text: 'Almacenar y Finalizar',
        type: 'success',
        width: '50%',
        onClick: function (params) {
            AlmacenarCaracteristicas(true);
        }
    };

    function CargarCaracteristicas(registro) {
        $scope.cargandoVisible = true;

        $scope.almacenar = (registro.ID_TIPOVISITA == 6 && registro.ID_ESTADOVISITA != 5);

        $scope.estado = registro.ID_ESTADO;
        $scope.idActuacion = registro.ID_VISITA;

        $.get($('#app').data('url') + 'Tramites/api/ActuacionJuridicaApi/ConsultarDatosFormulario?idEstado=' + registro.ID_ESTADO + '&idFormulario=' + $scope.actuacion.formulario + '&idActuacion=' + $scope.idActuacion + '&tipoConsulta=' + $scope.tipoConsulta, function (data) {
            if (data) {
                if (!data.error) {
                    $scope.actuacion.actuacionSeleccionada = true;
                    jsonDetalle = eval('(' + data.jsonDatos + ')');
                    var html = consultarDetalle(jsonDetalle, 0, 0, "acordionDetallePrincipal");
                    $("#acordionDetallePrincipal").remove();
                    $("#acordionDetalleGeneral").append(html);

                    if (data.idTramite != 0) {
                        $scope.tipoPrecargado = true;

                        cboDocumentoDataSource.store().clear();
                        cboDocumentoDataSource.store().insert({ ID_POPUP: data.idTramite + ',' + data.idDocumento, NOMBRE_POPUP: data.documento });
                        cboDocumentoDataSource.load();

                        $scope.actuacion.serie = data.idSerie;
                        $scope.actuacion.documento = data.idTramite + ',' + data.idDocumento;
                        documentoSeleccionado = $scope.actuacion.documento;
                        tipoaActoSeleccionado = data.idTipoActo;

                        $scope.cargandoVisible = true;

                        $.get($('#app').data('url') + 'Tramites/api/ActuacionJuridicaApi/TiposActoFormulario?idSerie=' + $scope.actuacion.serie + '&idFormulario=' + $scope.actuacion.formulario, function (data) {
                            cboTipoActoDataSource.store().clear();
                            if (data.numRegistros > 0) {
                                for (var i = 0; i < data.datos.length; i++) {
                                    cboTipoActoDataSource.store().insert(data.datos[i]);
                                }
                            }

                            cboTipoActoDataSource.load();
                            $scope.cargandoVisible = false;
                            $scope.actuacion.tipoActo = tipoaActoSeleccionado;
                            $scope.tipoPrecargado = false;

                            tipoaActoSeleccionado = null;

                            $scope.$apply();

                        }, "json");
                    } else {
                        cboDocumentoDataSource.store().clear();
                        cboDocumentoDataSource.load();

                        $scope.actuacion.documento = null;
                        documentoSeleccionado = null;
                        $scope.actuacion.serie = null;
                        $scope.actuacion.tipoActo = null;
                    }

                    $scope.cargandoVisible = false;

                    var tab = $('#tabOpciones').dxTabs('instance');
                    tab.option('selectedIndex', 1);
                    $scope.selectedTab = 1;

                    $scope.$apply();
                } else {
                    $scope.cargandoVisible = false;
                    $scope.$apply();
                    MostrarNotificacion('alert', 'error', data.error);
                }
            } else {
                $scope.cargandoVisible = false;
                $scope.$apply();
                MostrarNotificacion('alert', 'error', 'No se pudo cargar la consulta de características.');
            }
        }, "json");
    }

    function AlmacenarCaracteristicas(finalizarActuacion) {
        if ($scope.actuacion.documento && $scope.actuacion.tipoActo) {
            $scope.cargandoVisible = true;

            var jsoE = guardarDetalle("acordionDetallePrincipal", 0);
            var jsonOficial = JSON.stringify(jsoE)

            $('#btnAlmacenarCaracteristicas').dxButton({
                disabled: true
            });

            $('#btnFinalizarCaracteristicas').dxButton({
                disabled: true
            });

            var datosDocumento = $scope.actuacion.documento.split(',');

            $http.post($('#app').data('url') + 'Tramites/api/ActuacionJuridicaApi/AlmacenarDatosFormulario', { idEstado: $scope.estado, idFormulario: $scope.actuacion.formulario, idActuacion: $scope.idActuacion, idTercero: $scope.actuacion.tercero, idTipoActo: $scope.actuacion.tipoActo, idTramite: datosDocumento[0], idDocumento: datosDocumento[1], jsonDatos: jsonOficial, finalizar: finalizarActuacion }).success(function (data, status, headers, config) {
                $scope.cargandoVisible = false;
                MostrarNotificacion('alert', (data.resp === 'OK' ? 'success' : 'error'), data.mensaje);

                CargarCaracteristicas(registroActual);

                if (finalizarActuacion) {
                    $('#grdActuacionesItem').dxDataGrid({
                        dataSource: actuacionesItemDataSource
                    });

                    $scope.almacenar = false;
                }
                $('#btnAlmacenarCaracteristicas').dxButton({
                    disabled: false
                });

                $('#btnFinalizarCaracteristicas').dxButton({
                    disabled: false
                });
            }).error(function (data, status, headers, config) {
                $scope.cargandoVisible = false;
                MostrarNotificacion('alert', 'error', 'Error Almacenando Características ' + data.message);
                $('#btnAlmacenarCaracteristicas').dxButton({
                    disabled: false
                });

                $('#btnFinalizarCaracteristicas').dxButton({
                    disabled: false
                });
            });
        } else {
            MostrarNotificacion('alert', 'error', 'Campos [Documento] y [Tipo Acto] Requeridos');
        }
    }
});

var cboInstalacionDataSource = new DevExpress.data.DataSource([]);
var cboDocumentoDataSource = new DevExpress.data.DataSource([]);
var cboDocumentoBusquedaDataSource = new DevExpress.data.DataSource([]);
var cboFormularioDataSource = new DevExpress.data.DataSource([]);
var cboItemDataSource = new DevExpress.data.DataSource([]);
var cboTipoActoDataSource = new DevExpress.data.DataSource([]);

function VerDocumento(idTramite, idDocumento)
{
    window.open($('#app').data('url') + 'Tramites/Documento/ConsultarInformeTecnicoRadicado?idTramite=' + idTramite + '&idDocumento=' + idDocumento);
}

instalacionDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"S_RSOCIAL","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'Tramites/api/ActuacionJuridicaApi/ConsultaInstalacion', {
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

documentoBusquedaDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"ANO","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'Tramites/api/ActuacionJuridicaApi/ConsultaDocumentos', {
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

documentoDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"ANO","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'Tramites/api/ActuacionJuridicaApi/ConsultaDocumentos', {
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

actuacionesItemDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var skip = 0;
        var take = 1000;
        $.getJSON($('#app').data('url') + 'Tramites/api/ActuacionJuridicaApi/ActuacionesItem', {
            idFormulario: formularioSeleccionado == null ? -1 : formularioSeleccionado,
            idItem: itemSeleccionado == null ? -1 : itemSeleccionado
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


function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        DevExpress.ui.dialog.alert(msg, 'Actuaciones Jurídicas');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}
