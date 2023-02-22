var SIMApp = angular.module('SIM', ['dx']);

SIMApp.controller("PrestamoController", function ($scope, $location, $http) {
    $scope.datosTercero = {};
    $scope.datosTercero.NATURAL = {};
    $scope.terceroOK = false;
    $scope.nuevoTercero = false;
    $scope.nombres = {};
    $scope.nombres['NOMBRES'] = '';
    $scope.nombres['APELLIDOS'] = '';
    $scope.datosPrestamo = {};
    $scope.datosPrestamo.COD_DOCUMENTO = '';
    $scope.datosPrestamo.S_OBSERVACION_DOCUMENTO = '';
    $scope.datosPrestamo.S_OBSERVACIONES = '';
    $scope.datosDevolucion = {};
    $scope.datosDevolucion.COD_DOCUMENTO = '';
    $scope.datosDevolucion.S_OBSERVACION_DOCUMENTO = '';
    $scope.selectedTab = 0;
    $scope.cargandoVisible = false;
    $scope.busqueda = {};
    $scope.busqueda.Texto = '';

    $('.my-cloak').removeClass('my-cloak');

    $scope.btnBusquedaSettings = {
        text: 'Buscar Documentos',
        type: 'success',
        onClick: function (params) {
            var popup = $('#popBusquedaDocumentos').dxPopup('instance');
            popup.show();
        }
    }

    $scope.btnBuscarSettings = {
        text: 'Buscar',
        width: '90%',
        type: 'success',
        onClick: function (params) {
            $('#grdResultadoBusqueda').dxDataGrid({
                dataSource: grdBusquedaDocumentosDataSource
            });
        }
    };

    $scope.popBusquedaDocumentosSettings = {
        buttons: [{
            toolbar: 'bottom', location: 'after', widget: 'button', options: {
                text: 'Cerrar', onClick: (function (itemData) {
                    var popup = $('#popBusquedaDocumentos').dxPopup('instance');
                    popup.hide();
                })
            }
        }],
        title: 'Búsqueda de Documentos',
        fullScreen: true,
        
    };

    $scope.grdResultadoBusquedaSettings = {
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
        sorting: {
            mode: 'none',
        },
        editing: {
            editEnabled: false,
            removeEnabled: false,
            insertEnabled: false,
        },
        selection: {
            mode: 'single'
        },
        columns: [
            {
                dataField: "S_UNIDADDOCUMENTAL",
                caption: 'UNIDAD DOCUMENTAL',
                dataType: 'string',
                width: '15%'
            }, {
                dataField: "S_TIPO",
                caption: 'TIPO',
                dataType: 'string',
                width: '5%'
            }, {
                dataField: "S_TIPOANEXO",
                caption: 'TIPO ANEXO',
                dataType: 'string',
                width: '8%'
            }, {
                dataField: 'S_TEXTO',
                caption: 'TEXTO',
                dataType: 'string',
                width: '42%'
            }, {
                dataField: 'S_UBICACION',
                caption: 'UBICACION',
                dataType: 'string',
                width: '10%'
            }, {
                dataField: 'S_ESTADO',
                caption: 'ESTADO',
                dataType: 'string',
                width: '8%'
            }, {
                dataField: 'S_PRESTADOPOR',
                caption: 'PRESTADO POR',
                dataType: 'string',
                width: '10%'
            }
        ],
    };

    $scope.tabsOptions = {
        dataSource: tabsData,
        onItemClick: (function (itemData) {
            $scope.setTab(itemData.itemIndex);
        }),
        selectedIndex: 0
    };

    $scope.setTab = function (tabIndex) {
        $scope.selectedTab = tabIndex;
    };

    $scope.isActive = function (tabIndex) {
        if ($scope.selectedTab === tabIndex)
            return true;
        else
            return false;
    };

    $scope.AdicionarDocumento = function (e) {
        if (e.jQueryEvent.keyCode == 13) {
            if ($scope.datosPrestamo.COD_DOCUMENTO != null && $scope.datosPrestamo.COD_DOCUMENTO.trim() != '') {
                // Busca si el código ya está registrado en el grid

                //var grid = $('#grdDocumentosSeleccionados').dxDataGrid('instance');
                var itemsGrid = documentosSeleccionadosDataSource.items();

                //for (i = 0; i < grid.totalCount() ; i++) {
                for (i = 0; i < itemsGrid.length ; i++) {
                    //if (grid.getCellElement(i, 'ID')[0].textContent == $scope.datosPrestamo.COD_DOCUMENTO.trim()) {
                    if (itemsGrid[i]['ID'] == $scope.datosPrestamo.COD_DOCUMENTO.trim()) {
                        //MostrarNotificacion('notify', 'error', 'El documento ya se encuentra registrado.')

                        var result = DevExpress.ui.dialog.confirm('El documento ya se encuentra registrado. Desea eliminarlo de la selección ?', 'Confirmación');
                        result.done(function (dialogResult) {
                            if (dialogResult) {
                                documentosSeleccionadosDataSource.store().remove(itemsGrid[i]);
                                documentosSeleccionadosDataSource.load();

                                var grid = $('#grdDocumentosSeleccionados').dxDataGrid('instance');
                                grid.refresh();
                            }

                            $scope.datosPrestamo.COD_DOCUMENTO = '';
                            $scope.datosPrestamo.S_OBSERVACION_DOCUMENTO = '';
                            $('#codigoDocumento').dxTextBox('instance').focus();
                        });

                        return;
                    }
                }
                $scope.cargandoVisible = true;
                $.getJSON($('#app').data('url') + 'GestionDocumental/api/PrestamoApi/ConsultaRadicado', {
                    radicado: $scope.datosPrestamo.COD_DOCUMENTO.trim()
                }).done(function (data) {
                    if (data.tipoRespuesta == 'OK') {
                        documentosSeleccionadosDataSource.store().insert({ ID_RADICADO: data.radicado.ID_RADICADO, ID: data.radicado.S_ETIQUETA.trim(), DOCUMENTO: '', OBSERVACIONES: $scope.datosPrestamo.S_OBSERVACION_DOCUMENTO, TIPO: data.radicado.S_TIPO });
                        documentosSeleccionadosDataSource.load();

                        var grid = $('#grdDocumentosSeleccionados').dxDataGrid('instance');
                        grid.refresh();

                        $scope.cargandoVisible = false;
                        var popupLoading = $('#popCargando').dxLoadPanel('instance');
                        popupLoading.hide();
                    } else {
                        $scope.cargandoVisible = false;
                        var popupLoading = $('#popCargando').dxLoadPanel('instance');
                        popupLoading.hide();
                        MostrarNotificacion('notify', 'Error', data.detalleRespuesta);
                    }

                    $scope.datosPrestamo.COD_DOCUMENTO = '';
                    $scope.datosPrestamo.S_OBSERVACION_DOCUMENTO = '';

                    $('#codigoDocumento').dxTextBox('instance').focus();
                }).fail(function (jqxhr, textStatus, error) {
                    var popupLoading = $('#popCargando').dxLoadPanel('instance');
                    popupLoading.hide();
                    $scope.cargandoVisible = false;
                    alert(error);
                });
            }
        }
    }

    $scope.CodigoDocumentoSettings = {
        placeholder: "[Código de Barras del Documento]",
        bindingOptions: { value: 'datosPrestamo.COD_DOCUMENTO', readOnly: '!terceroOK' },
        onKeyUp: $scope.AdicionarDocumento,
    };

    $scope.ObservacionesDocumentoSettings = {
        placeholder: "[Observaciones del Documento]",
        bindingOptions: { value: 'datosPrestamo.S_OBSERVACION_DOCUMENTO', readOnly: '!terceroOK' },
        onKeyUp: $scope.AdicionarDocumento,
    };

    $.get($('#app').data('url') + 'General/api/TerceroApi/TiposIdentificacionNatural', function (data) {
        for (var i = 0; i < data.length; i++) {
            tiposIdentificacionDataSource.store().insert(data[i]);
        }
        tiposIdentificacionDataSource.load();
    }, "json");

    $scope.TipoDocumentoSettings = {
        dataSource: tiposIdentificacionDataSource,
        placeholder: "",
        displayExpr: "S_ABREVIATURA",
        valueExpr: "ID_TIPODOCUMENTO",
        bindingOptions: { value: 'datosTercero.ID_TIPODOCUMENTO' }
    };

    $scope.IdentificacionSettings = {
        onFocusOut: function (params) {
            if ($scope.nuevoTercero) {
                $scope.cargandoVisible = true;
                $.getJSON($('#app').data('url') + 'General/api/TerceroApi/TerceroBasicoIdentificacion', {
                    tipoTercero: $scope.datosTercero.ID_TIPODOCUMENTO,
                    identificacion: $scope.datosTercero.N_DOCUMENTON
                }).done(function (data) {
                    if (data != null) {
                        $scope.datosTercero = data;

                        if ($scope.datosTercero.NATURAL.D_NACIMIENTO != null)
                            $scope.datosTercero.NATURAL.D_NACIMIENTO = new Date($scope.datosTercero.NATURAL.D_NACIMIENTO.substring(0, 4), parseInt($scope.datosTercero.NATURAL.D_NACIMIENTO.substring(5, 7)) - 1, $scope.datosTercero.NATURAL.D_NACIMIENTO.substring(8, 10));
                        $scope.nombres['NOMBRES'] = $scope.datosTercero.NATURAL.S_NOMBRE1 + ($scope.datosTercero.NATURAL.S_NOMBRE2 === null ? '' : ' ' + $scope.datosTercero.NATURAL.S_NOMBRE2);
                        $scope.nombres['APELLIDOS'] = $scope.datosTercero.NATURAL.S_APELLIDO1 + ($scope.datosTercero.NATURAL.S_APELLIDO2 === null ? '' : ' ' + $scope.datosTercero.NATURAL.S_APELLIDO2);

                        $scope.terceroOK = true;
                        $scope.nuevoTercero = false;

                        $scope.$apply();
                    }
                    $scope.cargandoVisible = false;
                    var popupLoading = $('#popCargando').dxLoadPanel('instance');
                    popupLoading.hide();
                }).fail(function (jqxhr, textStatus, error) {
                    var popupLoading = $('#popCargando').dxLoadPanel('instance');
                    popupLoading.hide();
                    $scope.cargandoVisible = false;
                    alert(error);
                });
            }
        },
        onChange: function (params) {
            $scope.terceroOK = false;
            $scope.nuevoTercero = true;

            $scope.datosTercero.NATURAL = {};
            $scope.nombres = {};
            $scope.nombres['NOMBRES'] = '';
            $scope.nombres['APELLIDOS'] = '';
            $scope.CalcularDV();
        },
        bindingOptions: { value: 'datosTercero.N_DOCUMENTON', min: 0, max: 9 }
    };

    $scope.GeneroSettings = {
        dataSource: generosDataSource,
        placeholder: "[Genero]",
        displayExpr: "NOMBRE",
        valueExpr: "ID",
        readOnly: true,
        bindingOptions: { value: 'datosTercero.NATURAL.S_GENERO' }
    };

    $scope.btnBuscarDocumentosSettings = {
        text: 'Buscar Documentos',
        type: 'success',
        onClick: function (params) {
            var popup = $('#popBusquedaDocumentos').dxPopup('instance');
            popup.show();
        }
    };

    $scope.btnPrestarDocumentosSettings = {
        text: 'Prestar Documentos',
        type: 'success',
        onClick: function (params) {
            var grid = $('#grdDocumentosSeleccionados').dxDataGrid('instance');
            var radicadosSeleccionados = '';

            if (documentosSeleccionadosDataSource.items().length == 0) {
                MostrarNotificacion('notify', 'error', 'No se han registrado documentos para prestar.');
                return;
            }

            $('#btnPrestarDocumentos').dxButton({
                disabled: true
            });

            for (i = 0; i < documentosSeleccionadosDataSource.items().length ; i++) {
                if (radicadosSeleccionados == '') {
                    radicadosSeleccionados = documentosSeleccionadosDataSource.items()[i].ID_RADICADO + '|' + documentosSeleccionadosDataSource.items()[i].OBSERVACIONES;
                } else {
                    radicadosSeleccionados += '^' + documentosSeleccionadosDataSource.items()[i].ID_RADICADO + '|' + documentosSeleccionadosDataSource.items()[i].OBSERVACIONES;
                }
            }

            $scope.cargandoVisible = true;
            $http.post($('#app').data('url') + 'GestionDocumental/api/PrestamoApi/PrestamoDocumentos', { idTercero: $scope.datosTercero.ID_TERCERO, documentosIDs: radicadosSeleccionados, observacion: $scope.datosPrestamo.S_OBSERVACIONES }).success(function (data, status, headers, config) {
                var popupLoading = $('#popCargando').dxLoadPanel('instance');
                popupLoading.hide();

                $('#btnPrestarDocumentos').dxButton({
                    disabled: false
                });

                $scope.cargandoVisible = false;
                MostrarNotificacion('alert', (data.tipoRespuesta === 'OK' ? 'success' : 'error'), data.detalleRespuesta);

                $scope.datosPrestamo.COD_DOCUMENTO = '';
                $scope.datosPrestamo.S_OBSERVACION_DOCUMENTO = '';
                $scope.datosPrestamo.S_OBSERVACIONES = '';

                //var arr = viewModel.DataSource.items(); for (i = 0; i < arr.length ; i++) { viewModel.DataSource.store().remove(arr[i]); }
                documentosSeleccionadosDataSource = new DevExpress.data.DataSource([]);
                $('#grdDocumentosSeleccionados').dxDataGrid({
                    dataSource: documentosSeleccionadosDataSource
                });
            });
        }
    };

    $scope.CalcularDV = function () {
        ObtenerDV($scope.datosTercero);
    };

    $scope.grdDocumentosSeleccionadosSettings = {
        dataSource: documentosSeleccionadosDataSource,
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: false,
            allowedPageSizes: [5, 10, 20, 50]
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
        height: 200,
        columns: [
            {
                dataField: 'ID',
                width: '20%',
                caption: 'ID',
                dataType: 'string',
            }, {
                dataField: 'DOCUMENTO',
                width: '20%',
                caption: 'Documento',
                dataType: 'string',
                visible: false,
            }, {
                dataField: 'OBSERVACIONES',
                width: '40%',
                caption: 'Observaciones',
                dataType: 'string',
            }, {
                dataField: 'TIPO',
                width: '20%',
                caption: 'Tipo',
                dataType: 'string',
            }
        ],
    };

    $scope.AdicionarDocumentoDev = function (e) {
        if (e.jQueryEvent.keyCode == 13) {
            //alert($scope.datosDevolucion.COD_DOCUMENTO);
            if ($scope.datosDevolucion.COD_DOCUMENTO != null && $scope.datosDevolucion.COD_DOCUMENTO.trim() != '') {
                // Busca si el código ya está registrado en el grid

                //var grid = $('#grdDocumentosSeleccionadosDev').dxDataGrid('instance');
                var itemsGrid = documentosSeleccionadosDevDataSource.items();

                //for (i = 0; i < grid.totalCount() ; i++) {
                for (i = 0; i < itemsGrid.length ; i++) {
                    //if (grid.getCellElement(i, 'ID')[0].textContent == $scope.datosDevolucion.COD_DOCUMENTO.trim()) {
                    if (itemsGrid[i]['ID'] == $scope.datosDevolucion.COD_DOCUMENTO.trim()) {
                        //MostrarNotificacion('notify', 'error', 'El documento ya se encuentra registrado.')

                        var result = DevExpress.ui.dialog.confirm('El documento ya se encuentra registrado. Desea eliminarlo de la selección ?', 'Confirmación');
                        result.done(function (dialogResult) {
                            if (dialogResult) {
                                documentosSeleccionadosDevDataSource.store().remove(itemsGrid[i]);
                                documentosSeleccionadosDevDataSource.load();

                                var grid = $('#grdDocumentosSeleccionadosDev').dxDataGrid('instance');
                                grid.refresh();
                            }

                            $scope.datosDevolucion.COD_DOCUMENTO = '';
                            $scope.datosDevolucion.S_OBSERVACION_DOCUMENTO = '';
                            $('#codigoDocumentoDev').dxTextBox('instance').focus();
                        });

                        return;
                    }
                }

                $scope.cargandoVisible = true;
                $.getJSON($('#app').data('url') + 'GestionDocumental/api/PrestamoApi/ConsultaRadicadoPrestado', {
                    radicado: $scope.datosDevolucion.COD_DOCUMENTO.trim()
                }).done(function (data) {
                    //alert(JSON.stringify(data));
                    if (data.tipoRespuesta == 'OK') {
                        documentosSeleccionadosDevDataSource.store().insert({
                            ID_RADICADO: data.radicado.ID_RADICADO,
                            ID_PRESTAMODETALLE: data.radicado.ID_PRESTAMODETALLE,
                            ID: data.radicado.S_ETIQUETA.trim(),
                            DOCUMENTO: '',
                            OBSERVACIONES_PRESTAMO: data.radicado.OBSERVACIONES_PRESTAMO,
                            OBSERVACIONES_DEVOLUCION: $scope.datosDevolucion.S_OBSERVACION_DOCUMENTO,
                            D_PRESTAMO: data.radicado.D_PRESTAMO,
                            S_TERCERO: data.radicado.S_TERCERO,
                            TIPO: data.radicado.S_TIPO
                        });
                        documentosSeleccionadosDevDataSource.load();

                        var grid = $('#grdDocumentosSeleccionadosDev').dxDataGrid('instance');
                        grid.refresh();

                        $scope.cargandoVisible = false;
                        var popupLoading = $('#popCargando').dxLoadPanel('instance');
                        popupLoading.hide();
                    } else {
                        $scope.cargandoVisible = false;
                        var popupLoading = $('#popCargando').dxLoadPanel('instance');
                        popupLoading.hide();
                        MostrarNotificacion('notify', 'Error', data.detalleRespuesta);
                    }

                    $scope.datosDevolucion.COD_DOCUMENTO = '';
                    $scope.datosDevolucion.S_OBSERVACION_DOCUMENTO = '';

                    $('#codigoDocumentoDev').dxTextBox('instance').focus();
                }).fail(function (jqxhr, textStatus, error) {
                    $scope.cargandoVisible = false;
                    var popupLoading = $('#popCargando').dxLoadPanel('instance');
                    popupLoading.hide();
                    alert(error);
                });
            }
        }
    }

    $scope.CodigoDocumentoDevSettings = {
        placeholder: "[Código de Barras del Documento]",
        bindingOptions: { value: 'datosDevolucion.COD_DOCUMENTO' },
        onKeyUp: $scope.AdicionarDocumentoDev,
    };

    $scope.ObservacionesDocumentoDevSettings = {
        placeholder: "[Observaciones del Documento]",
        bindingOptions: { value: 'datosDevolucion.S_OBSERVACION_DOCUMENTO' },
        onKeyUp: $scope.AdicionarDocumentoDev,
    };

    $scope.grdDocumentosSeleccionadosDevSettings = {
        dataSource: documentosSeleccionadosDevDataSource,
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: false,
            allowedPageSizes: [5, 10, 20, 50]
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
        height: 200,
        columns: [
            {
                dataField: 'ID',
                width: '20%',
                caption: 'ID',
                dataType: 'string',
            }, {
                dataField: 'DOCUMENTO',
                width: '20%',
                caption: 'Documento',
                dataType: 'string',
                visible: false,
            }, {
                dataField: 'OBSERVACIONES_PRESTAMO',
                width: '20%',
                caption: 'Observaciones Préstamo',
                dataType: 'string',
            }, {
                dataField: 'OBSERVACIONES_DEVOLUCION',
                width: '20%',
                caption: 'Observaciones Devolución',
                dataType: 'string',
            }, {
                dataField: 'D_PRESTAMO',
                width: '10%',
                caption: 'Fecha',
                dataType: 'date',
                format: 'yyyy/MM/dd',
            }, {
                dataField: 'S_TERCERO',
                width: '15%',
                caption: 'Prestado Por',
                dataType: 'string',
            }, {
                dataField: 'TIPO',
                width: '15%',
                caption: 'Tipo',
                dataType: 'string',
            }
        ],
    };

    $scope.btnDevolverDocumentosSettings = {
        text: 'Devolver Documentos',
        type: 'success',
        onClick: function (params) {
            var grid = $('#grdDocumentosSeleccionadosDev').dxDataGrid('instance');
            var radicadosSeleccionados = '';

            if (documentosSeleccionadosDevDataSource.items().length == 0) {
                MostrarNotificacion('notify', 'error', 'No se han registrado documentos para devolver.');
                return;
            }

            $('#btnDevolverDocumentos').dxButton({
                disabled: true
            });

            for (i = 0; i < documentosSeleccionadosDevDataSource.items().length ; i++) {
                if (radicadosSeleccionados == '') {
                    radicadosSeleccionados = documentosSeleccionadosDevDataSource.items()[i].ID_PRESTAMODETALLE + '|' +
                        documentosSeleccionadosDevDataSource.items()[i].OBSERVACIONES_DEVOLUCION;
                } else {
                    radicadosSeleccionados += '^' + documentosSeleccionadosDevDataSource.items()[i].ID_PRESTAMODETALLE + '|' +
                        documentosSeleccionadosDevDataSource.items()[i].OBSERVACIONES_DEVOLUCION;
                }
            }

            $scope.cargandoVisible = true;
            $http.post($('#app').data('url') + 'GestionDocumental/api/PrestamoApi/DevolucionDocumentos', { idTercero: $scope.datosTercero.ID_TERCERO, documentosIDs: radicadosSeleccionados }).success(function (data, status, headers, config) {
                $scope.cargandoVisible = false;
                var popupLoading = $('#popCargando').dxLoadPanel('instance');
                popupLoading.hide();

                $('#btnDevolverDocumentos').dxButton({
                    disabled: false
                });

                MostrarNotificacion('alert', (data.tipoRespuesta === 'OK' ? 'success' : 'error'), data.detalleRespuesta);

                if (data.tipoRespuesta == 'OK') {
                    documentosSeleccionadosDevDataSource = new DevExpress.data.DataSource([]);
                    $('#grdDocumentosSeleccionadosDev').dxDataGrid({
                        dataSource: documentosSeleccionadosDevDataSource
                    });
                }
            });
        }
    };

    var grdBusquedaDocumentosDataSource = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var d = $.Deferred();

            var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
            var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"S_UNIDADDOCUMENTAL","desc":false}]';
            var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

            var skip = loadOptions.skip;
            var take = loadOptions.take;
            $.getJSON($('#app').data('url') + 'GestionDocumental/api/PrestamoApi/BusquedaDocumentos', {
                filter: $scope.busqueda.Texto,
                sort: sortOptions,
                group: groupOptions,
                skip: skip,
                take: take,
                searchValue: '',
                searchExpr: '',
                comparation: '',
                noFilterNoRecords: true
            }).done(function (data) {
                d.resolve(data.datos, { totalCount: data.numRegistros });
            });
            return d.promise();
        }
    });

    $scope.BuscarUbicacionDocumento = function (e) {
        if (e.jQueryEvent.keyCode == 13) {
            if ($scope.datosUbicacion.COD_DOCUMENTO != null && $scope.datosUbicacion.COD_DOCUMENTO.trim() != '') {

                $scope.cargandoVisible = true;
                $.getJSON($('#app').data('url') + 'GestionDocumental/api/PrestamoApi/ConsultaUbicacionDocumento', {
                    CB: $scope.datosUbicacion.COD_DOCUMENTO.trim()
                }).done(function (data) {
                    if (data.tipoRespuesta == 'OK') {
                        $scope.cargandoVisible = false;
                        $scope.datosUbicacion.S_UBICACION = data.radicado.S_UBICACION;
                        var popupLoading = $('#popCargando').dxLoadPanel('instance');
                        popupLoading.hide();
                    } else {
                        $scope.cargandoVisible = false;
                        var popupLoading = $('#popCargando').dxLoadPanel('instance');
                        popupLoading.hide();
                        MostrarNotificacion('notify', 'Error', data.detalleRespuesta);
                    }

                    $('#txtCodigoDocumento input').select();
                }).fail(function (jqxhr, textStatus, error) {
                    $scope.cargandoVisible = false;
                    var popupLoading = $('#popCargando').dxLoadPanel('instance');
                    popupLoading.hide();
                    $('#txtCodigoDocumento input').select();
                    alert(error);
                });
            }
        }
    }

    $scope.CodigoDocumentoCUSettings = {
        placeholder: "[Código de Barras del Documento]",
        bindingOptions: { value: 'datosUbicacion.COD_DOCUMENTO' },
        onKeyUp: $scope.BuscarUbicacionDocumento,
    };

    $scope.UbicacionCUSettings = {
        placeholder: "[Ubicación del Documento]",
        bindingOptions: { value: 'datosUbicacion.S_UBICACION' },
    };

    $scope.btnReubicarCUSettings = {
        text: 'Reubicar Documento',
        type: 'success',
        onClick: function (params) {
            $scope.cargandoVisible = true;
            $.getJSON($('#app').data('url') + 'GestionDocumental/api/PrestamoApi/ReUbicacionDocumento', {
                CB: $scope.datosUbicacion.COD_DOCUMENTO.trim(),
                ubicacion: $scope.datosUbicacion.S_UBICACION
            }).done(function (data) {
                if (data.tipoRespuesta == 'OK') {
                    $scope.cargandoVisible = false;
                    var popupLoading = $('#popCargando').dxLoadPanel('instance');
                    popupLoading.hide();
                    MostrarNotificacion('alert', (data.tipoRespuesta === 'OK' ? 'success' : 'error'), data.detalleRespuesta);
                    $('#txtCodigoDocumento input').select();
                } else {
                    $scope.cargandoVisible = false;
                    var popupLoading = $('#popCargando').dxLoadPanel('instance');
                    popupLoading.hide();
                    MostrarNotificacion('notify', 'Error', data.detalleRespuesta);
                    $('#txtCodigoDocumento input').select();
                }
            }).fail(function (jqxhr, textStatus, error) {
                $scope.cargandoVisible = false;
                var popupLoading = $('#popCargando').dxLoadPanel('instance');
                popupLoading.hide();
                $('#txtCodigoDocumento input').select();
                alert(error);
            });
        }
    };
});

var generosDataSource = [
    { ID: 'M', NOMBRE: 'Masculino' },
    { ID: 'F', NOMBRE: 'Femenino' },
];

var tabsData = [
        { text: 'Préstamo de Documentos' },
        { text: 'Devolución de Documentos' },
        { text: 'Consulta de Ubicaciones' },
];

var tiposIdentificacionDataSource = new DevExpress.data.DataSource([]);
var documentosSeleccionadosDataSource = new DevExpress.data.DataSource({ store: [], key: 'ID' });
var documentosSeleccionadosDevDataSource = new DevExpress.data.DataSource({ store: [], key: 'ID' });

function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        DevExpress.ui.dialog.alert(msg, 'Préstamo');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}

function ObtenerDV(tercero) {
    nume = parseInt(tercero.N_DOCUMENTON);
    if (nume >= 1)
        ceros = "00000000000000";
    if (nume >= 10)
        ceros = "0000000000000";
    if (nume >= 100)
        ceros = "000000000000";
    if (nume >= 1000)
        ceros = "00000000000";
    if (nume >= 10000)
        ceros = "0000000000";
    if (nume >= 100000)
        ceros = "000000000";
    if (nume >= 1000000)
        ceros = "00000000";
    if (nume >= 10000000)
        ceros = "0000000";
    if (nume >= 100000000)
        ceros = "000000";
    if (nume >= 1000000000)
        ceros = "00000";
    if (nume >= 10000000000)
        ceros = "0000";
    if (nume >= 100000000000)
        ceros = "000";
    if (nume >= 1000000000000)
        ceros = "00";
    if (nume >= 10000000000000)
        ceros = "0";
    if (nume >= 100000000000000)
        ceros = "";

    li_peso = new Array();
    li_peso[0] = 71;
    li_peso[1] = 67;
    li_peso[2] = 59;
    li_peso[3] = 53;
    li_peso[4] = 47;
    li_peso[5] = 43;
    li_peso[6] = 41;
    li_peso[7] = 37; //8
    li_peso[8] = 29; //3
    li_peso[9] = 23; //0
    li_peso[10] = 19; //1
    li_peso[11] = 17; //2
    li_peso[12] = 13; //0
    li_peso[13] = 7; //9
    li_peso[14] = 3; //9

    ls_str_nit = ceros + tercero.N_DOCUMENTON;
    li_suma = 0;
    for (i = 0; i < 15; i++) {
        li_suma += ls_str_nit.substring(i, i + 1) * li_peso[i];
    }
    digito_chequeo = li_suma % 11;
    if (digito_chequeo >= 2)
        digito_chequeo = 11 - digito_chequeo;

    tercero.N_DIGITOVER = (digito_chequeo);
}