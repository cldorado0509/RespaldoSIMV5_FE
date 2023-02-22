var SIMApp = angular.module('SIM', ['dx']);

SIMApp.controller("RadicadorController", function ($scope, $http) {
    $scope.radicador = {};
    $scope.radicador.CB = null;
    $scope.radicador.serie = null;
    $scope.radicador.subSerie = null;
    $scope.radicador.unidadDocumental = null;
    $scope.radicador.tipoExpediente = null;
    $scope.radicador.tipoEtiqueta = null;
    $scope.radicador.tipoAnexo = null;
    $scope.radicador.ubicacion = null;
    $scope.radicador.consecutivo = null;
    $scope.radicador.documentoAsociado = null;
    $scope.radicador.documentoAsociadoTextos = [];
    $scope.recuperadorRadicado = {};
    $scope.recuperadorRadicado.serie = null;
    $scope.recuperadorRadicado.subSerie = null;
    $scope.recuperadorRadicado.unidadDocumental = null;
    $scope.recuperadorRadicado.tipoExpediente = null;
    $scope.recuperadorRadicado.documentoAsociado = null;
    $scope.recuperadorRadicado.documentoAsociadoTextos = [];
    $scope.recuperadorRadicado.radicado = null;
    $scope.anexo = {};
    $scope.anexo.tipo = null;
    $scope.anexo.CB = null;
    $scope.tomo = {};
    $scope.tomo.CB = null;
    $scope.documentoAsociadoTextos = {};
    $scope.documentoAsociadoLookup = false;
    $scope.documentoAsociadoLookupRR = false;
    $scope.documentoAsociadoPopupGrid = false;
    $scope.documentoAsociadoRRPopupGrid = false;
    $scope.documentoAsociadoText = false;
    $scope.documentoAsociadoTitulo = '';
    $scope.documentoAsociadoTituloRR = '';
    $scope.cargandoVisible = true;
    $scope.selectedTab = 0;
    $scope.itemSeleccionado = false;
    $scope.ID_POPUP = 0;
    $scope.NOMBRE_POPUP = 0;
    $scope.returnCallBack = false;
    $scope.showWait = true;

    $('.my-cloak').removeClass('my-cloak');

    $scope.popRadicadoSettings = {
        fullScreen: false,
        showTitle: true,
        title: 'Etiqueta'
    };

    if ($('#app').data('tipo') == '1') {
        var tabsData = [
                { text: 'Radicación', pos: 0 },
                { text: 'Anexos', pos: 1 },
                { text: 'Recuperación de Etiquetas', pos: 3 },
        ];
    } else {
        var tabsData = [
                { text: 'Etiquetado', pos: 0 },
                //{ text: 'Tomos', pos: 1 },
                //{ text: 'Anexos', pos: 2 },
                { text: 'Recuperación de Etiquetas', pos: 3 },
        ];
    }

    $scope.SoloNumeros = function (object) {
        var e = object.jQueryEvent;

        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            // Allow: Ctrl+A
            (e.keyCode == 65 && e.ctrlKey === true) ||
            // Allow: Ctrl+C
            (e.keyCode == 67 && e.ctrlKey === true) ||
            // Allow: Ctrl+X
            (e.keyCode == 88 && e.ctrlKey === true) ||
            // Allow: home, end, left, right
            (e.keyCode >= 35 && e.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    };

    $scope.popDocumentoAsociadoSettings = {
        /*fullScreen: true,*/
        showTitle: true,
        deferRendering: false,
        onHidden: function () {
            if ($scope.itemSeleccionado) {
                cboDocumentoAsociadoDataSource.store().clear();
                cboDocumentoAsociadoDataSource.store().insert({ ID_POPUP: $scope.ID_POPUP, NOMBRE_POPUP: $scope.NOMBRE_POPUP });
                cboDocumentoAsociadoDataSource.load();

                $scope.radicador.documentoAsociado = $scope.ID_POPUP;
            }
            $scope.itemSeleccionado = false;
        },
    }

    $scope.popDocumentoAsociadoRRSettings = {
        showTitle: true,
        deferRendering: false,
        onHidden: function () {
            if ($scope.itemSeleccionado) {
                cboDocumentoAsociadoDataSource.store().clear();
                cboDocumentoAsociadoDataSource.store().insert({ ID_POPUP: $scope.ID_POPUP, NOMBRE_POPUP: $scope.NOMBRE_POPUP });
                cboDocumentoAsociadoDataSource.load();

                $scope.recuperadorRadicado.documentoAsociado = $scope.ID_POPUP;
            }
            $scope.itemSeleccionado = false;
        },
    }

    $scope.documentoAsociadoPopupGridSettings = {
        valueExpr: 'ID_POPUP',
        displayExpr: 'NOMBRE_POPUP',
        bindingOptions: { value: 'radicador.documentoAsociado' },
        onOpened: function () {
            $('#cboDocumentoAsociado').dxSelectBox('instance').close();
            var popup = $('#popDocumentoAsociado').dxPopup('instance');
            popup.option('title', $scope.documentoAsociadoTitulo);
            popup.show();
        }
    };

    $scope.documentoAsociadoRRPopupGridSettings = {
        valueExpr: 'ID_POPUP',
        displayExpr: 'NOMBRE_POPUP',
        bindingOptions: { value: 'recuperadorRadicado.documentoAsociado' },
        onOpened: function () {
            $('#cboDocumentoAsociadoRR').dxSelectBox('instance').close();
            var popup = $('#popDocumentoAsociadoRR').dxPopup('instance');
            popup.option('title', $scope.documentoAsociadoTituloRR);
            popup.show();
        }
    };

    $scope.grdDocumentoAsociadoSettings = {
        dataSource: documentoAsociadoDataSource,
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20, 50]
        },
        filterRow: {
            visible: true,
        },
        groupPanel: {
            visible: true,
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
                dataField: "ID_TERCERO",
                width: '10%',
                caption: 'ID',
                dataType: 'number',
            }, {
                dataField: 'S_TIPO_DOCUMENTO',
                width: '7%',
                caption: 'Tipo',
                dataType: 'string',
            }
        ],
    };

    $scope.tabsOptions = {
        dataSource: tabsData,
        onItemClick: (function (itemData) {
            $scope.setTab(tabsData[itemData.itemIndex].pos);
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

    /*$.get($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/Serie?tipoRadicador=' + $('#app').data('tipo'), function (data) {
        for (var i = 0; i < data.datos.length; i++) {
            serieDataSource.store().insert(data.datos[i]);
        }
        serieDataSource.load();
        $scope.cargandoVisible = false;
        $scope.$apply();
    }, "json");*/

    $.get($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/TipoExpediente', function (data) {
        for (var i = 0; i < data.datos.length; i++) {
            tipoExpedienteDataSource.store().insert(data.datos[i]);
        }
        tipoExpedienteDataSource.load();
        $scope.cargandoVisible = false;
        $scope.$apply();
    }, "json");

    $scope.TipoAnexoRadicadorSettings = {
        dataSource: tipoAnexoDataSource,
        placeholder: "[Tipo de Anexo]",
        displayExpr: "S_NOMBRE",
        valueExpr: "ID_TIPO",
        searchEnabled: true,
        searchExpr: "S_NOMBRE",
        searchTimeout: 0,
        bindingOptions: { value: 'radicador.tipoAnexo' }
    };

    $scope.TipoAnexoSettings = {
        dataSource: tipoAnexoDataSource,
        placeholder: "[Tipo de Anexo]",
        displayExpr: "S_NOMBRE",
        valueExpr: "ID_TIPO",
        searchEnabled: true,
        searchExpr: "S_NOMBRE",
        searchTimeout: 0,
        bindingOptions: { value: 'radicador.tipoAnexo' }
    };

    $scope.ConsecutivoSettings = {
        placeholder: '[Consecutivo]',
        max: 999999999999999,
        min: 0,
        bindingOptions: { value: 'radicador.consecutivo' }
    };

    $scope.UbicacionSettings = {
        placeholder: '[Ubicación]',
        bindingOptions: { value: 'radicador.ubicacion' }
    };

    $scope.TipoExpedienteSettings = {
        dataSource: tipoExpedienteDataSource,
        placeholder: "[Tipo Expediente]",
        displayExpr: "S_NOMBRE",
        valueExpr: "ID_TIPOEXPEDIENTE",
        searchEnabled: true,
        searchExpr: "S_NOMBRE",
        searchTimeout: 0,
        onValueChanged: function (data) {
            $scope.CambiaTipoExpediente(data.value, false);
        },
        bindingOptions: { value: 'radicador.tipoExpediente' }
    };

    $scope.CambiaTipoExpediente = function (idTipoExpediente, recuperadoEtiquetas) {
        registrosRelacionadosDataSource.store().clear();
        registrosRelacionadosDataSource.load();
        var grdRegistrosRelacionados = $('#grdRegistrosRelacionados').dxDataGrid('instance');
        grdRegistrosRelacionados.refresh();

        if (idTipoExpediente != null) {
            if (recuperadoEtiquetas) {
                $scope.recuperadorRadicado.documentoAsociado = null;
                $scope.recuperadorRadicado.documentoAsociadoTextos = [];

                $('#lupDocumentoAsociadoRR').dxValidator({
                    validationRules: [],
                });

                $('#cboDocumentoAsociadoRR').dxValidator({
                    validationRules: [],
                });

                $scope.documentoAsociadoLookupRR = false;
                $scope.documentoAsociadoRRPopupGrid = false;
            } else {
                $scope.radicador.documentoAsociado = null;
                $scope.radicador.documentoAsociadoTextos = [];

                $('#lupDocumentoAsociado').dxValidator({
                    validationRules: [],
                });

                $('#cboDocumentoAsociado').dxValidator({
                    validationRules: [],
                });

                $scope.documentoAsociadoLookup = false;
                $scope.documentoAsociadoPopupGrid = false;
            }

            $scope.documentoAsociadoTextos = {};

            $scope.documentoAsociadoText = false;

            $scope.cargandoVisible = true;
            $.get($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/DocumentoAsociado?idTipoExpediente=' + idTipoExpediente, function (data) {
                if (data.tipoRespuesta == 'OK' && data.configuracion.DocumentoAsociado.Nombre != null) {
                    switch (data.configuracion.DocumentoAsociado.TipoConsulta) {
                        case 1:
                            documentoAsociadoDataSource = new DevExpress.data.CustomStore({
                                load: function (loadOptions) {
                                    var d = $.Deferred();

                                    var searchValueOptions = loadOptions.searchValue;
                                    var searchExprOptions = loadOptions.searchExpr;

                                    var skip = loadOptions.skip;
                                    var take = loadOptions.take;

                                    $.getJSON($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/' + data.configuracion.DocumentoAsociado.Funcion, {
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
                                        alert('error cargando datos: ' + textStatus + ", " + error);
                                    });
                                    return d.promise();
                                },
                                byKey: function (key, extra) {
                                    return key.toString();
                                },
                            });

                            $('#lupDocumentoAsociado' + (recuperadoEtiquetas ? 'RR' : '')).dxLookup({
                                dataSource: documentoAsociadoDataSource,
                                placeholder: data.configuracion.DocumentoAsociado.PlaceHolder,
                                title: data.configuracion.DocumentoAsociado.Titulo,
                                displayExpr: "S_NOMBRE_LOOKUP",
                                valueExpr: "ID_LOOKUP",
                                usePopover: false,
                                bindingOptions: { value: (recuperadoEtiquetas ? 'recuperadorRadicado' : 'radicador') + '.documentoAsociado' },
                            }).dxValidator({
                                validationRules: [{
                                    type: 'required',
                                    message: data.configuracion.DocumentoAsociado.Titulo + ' Requerido'
                                }],
                            });

                            if (recuperadoEtiquetas) {
                                $scope.documentoAsociadoLookupRR = true;
                                $scope.documentoAsociadoTituloRR = data.configuracion.DocumentoAsociado.Titulo;
                            } else {
                                $scope.documentoAsociadoLookup = true;
                                $scope.documentoAsociadoTitulo = data.configuracion.DocumentoAsociado.Titulo;
                            }

                            break;
                        case 2: // TODO: Pendiente por implementar. Se requiere un control tipo SelectBox que permita detectar cuando se despliegan los items para cargar un popup con un grid para realizar la selección. La idea por ahora es poner un select box y cuando se presione click sobre él se carga el grid, luego al seleccionar un registro se alimenta el dataSource del SelectBox y se toma este elemento como seleccionado.
                            var columnas = [];

                            for (i = 0; i < data.configuracion.DocumentoAsociado.ColumnasCombo.length; i++) {
                                var columna = {
                                    dataField: data.configuracion.DocumentoAsociado.ColumnasCombo[i].Nombre,
                                    width: data.configuracion.DocumentoAsociado.ColumnasCombo[i].Ancho + '%',
                                    caption: data.configuracion.DocumentoAsociado.ColumnasCombo[i].Titulo,
                                    visible: data.configuracion.DocumentoAsociado.ColumnasCombo[i].Visible,
                                    dataType: (data.configuracion.DocumentoAsociado.ColumnasCombo[i].TipoDato == 'S' ? 'string' : (data.configuracion.DocumentoAsociado.ColumnasCombo[i].TipoDato == 'N' ? 'number' : (data.configuracion.DocumentoAsociado.ColumnasCombo[i].TipoDato == 'D' ? 'date' : (data.configuracion.DocumentoAsociado.ColumnasCombo[i].TipoDato == 'B' ? 'boolean' : 'string'))))
                                };
                                columnas.push(columna);
                            }

                            documentoAsociadoDataSource = new DevExpress.data.CustomStore({
                                load: function (loadOptions) {
                                    var d = $.Deferred();

                                    var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
                                    var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"' + data.configuracion.DocumentoAsociado.OrdenadoPor + '","desc":false}]';
                                    var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

                                    var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
                                    var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
                                    $.getJSON($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/' + data.configuracion.DocumentoAsociado.Funcion, {
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

                            var gridName = '#grdDocumentoAsociado' + (recuperadoEtiquetas ? 'RR' : '');

                            var dataGrid = $(gridName).dxDataGrid({
                                dataSource: documentoAsociadoDataSource,
                                allowColumnResizing: true,
                                loadPanel: { text: 'Cargando Datos...' },
                                paging: {
                                    pageSize: 8
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
                                columns: columnas,
                                onSelectionChanged: function (selecteditems) {
                                    var data = selecteditems.selectedRowsData[0];
                                    $scope.itemSeleccionado = true;
                                    $scope.ID_POPUP = data.ID_POPUP;
                                    $scope.NOMBRE_POPUP = data.NOMBRE_POPUP;

                                    var popup = $('#popDocumentoAsociado' + (recuperadoEtiquetas ? 'RR' : '')).dxPopup('instance');
                                    popup.hide();

                                    if (recuperadoEtiquetas) {
                                        $scope.cargandoVisible = true;
                                        $.get($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/RegistrosRelacionadosExpediente?idTipoExpediente=' + $scope.recuperadorRadicado.tipoExpediente + '&idRelacionado=' + data.ID_POPUP, function (dataRegistros) {
                                            registrosRelacionadosDataSource.store().clear();
                                            for (var i = 0; i < dataRegistros.datos.length; i++) {
                                                registrosRelacionadosDataSource.store().insert(dataRegistros.datos[i]);
                                            }
                                            registrosRelacionadosDataSource.load();
                                            var grdRegistrosRelacionados = $('#grdRegistrosRelacionados').dxDataGrid('instance');
                                            grdRegistrosRelacionados.refresh();
                                            $scope.cargandoVisible = false;
                                        }, "json");
                                    }
                                }
                            });

                            var columns = $(gridName).dxDataGrid('instance').option("columns");
                            for (var i = 0; i < columns.length; i++) {
                                $(gridName).dxDataGrid('instance').columnOption(i, "filterValue", null); ////***** Have to put undefined in the last parameter instead of ""
                            }

                            $('#cboDocumentoAsociado' + (recuperadoEtiquetas ? 'RR' : '')).dxSelectBox({
                                dataSource: cboDocumentoAsociadoDataSource,
                                placeholder: data.configuracion.DocumentoAsociado.PlaceHolder,
                                displayExpr: 'NOMBRE_POPUP',
                                valueExpr: 'ID_POPUP',
                                usePopover: false,
                                bindingOptions: { value: (recuperadoEtiquetas ? 'recuperadorRadicado' : 'radicador') + '.documentoAsociado' },
                            }).dxValidator({
                                validationRules: [{
                                    type: 'required',
                                    message: data.configuracion.DocumentoAsociado.Titulo + ' Requerido'
                                }],
                            });

                            if (recuperadoEtiquetas) {
                                $scope.documentoAsociadoRRPopupGrid = true;
                                $scope.documentoAsociadoTituloRR = data.configuracion.DocumentoAsociado.Titulo;
                            } else {
                                $scope.documentoAsociadoPopupGrid = true;
                                $scope.documentoAsociadoTitulo = data.configuracion.DocumentoAsociado.Titulo;
                            }
                            break;
                        case 3:
                            $scope.documentoAsociadoTextos = data.configuracion.DocumentoAsociado.Campos;
                            $scope.$apply();

                            for (i = 0; i < data.configuracion.DocumentoAsociado.Campos.length; i++) { // Este ingreso de validadores NO FUNCIONA
                                $scope.radicador.documentoAsociadoTextos.push({ nombre: data.configuracion.DocumentoAsociado.Campos[i].Nombre, texto: '' });
                                $('#campo' + i).dxValidator({
                                    validationRules: [{
                                        type: 'required',
                                        message: data.configuracion.DocumentoAsociado.Campos[i].Titulo + ' Requerido'
                                    }],
                                });
                            }

                            $scope.documentoAsociadoText = true;
                            break;
                    }
                    $scope.cargandoVisible = false;
                } else {
                    $scope.cargandoVisible = false;
                    MostrarNotificacion('alert', '', 'Error - ' + data.detalleRespuesta);
                }
            }, "json");
        }
    }

    $scope.SerieSettings = {
        dataSource: serieDataSource,
        placeholder: "[Serie]",
        displayExpr: "S_NOMBRE",
        valueExpr: "ID_SERIE",
        searchEnabled: true,
        searchExpr: "S_NOMBRE",
        searchTimeout: 0,
        onValueChanged: function (data) {
            $scope.CambiaSerie(data.value);
        },
        bindingOptions: { value: 'radicador.serie' }
    };

    $scope.CambiaSerie = function (idSerie) {
        $scope.radicador.CB = null;

        $scope.radicador.subSerie = null;
        $scope.radicador.unidadDocumental = null;

        $scope.radicador.documentoAsociado = null;
        $scope.radicador.documentoAsociadoTextos = [];
        $scope.documentoAsociadoTextos = {};

        $scope.documentoAsociadoLookup = false;
        $scope.documentoAsociadoText = false;
        $scope.documentoAsociadoPopupGrid = false;

        if ($scope.showWait)
            $scope.cargandoVisible = true;
        $.get($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/SubSerie?idSerie=' + idSerie + '&tipoRadicador=' + $('#app').data('tipo'), function (dataSubSerie) {
            subSerieDataSource.store().clear();
            for (var i = 0; i < dataSubSerie.datos.length; i++) {
                subSerieDataSource.store().insert(dataSubSerie.datos[i]);
            }
            subSerieDataSource.load();
            if ($scope.showWait)
                $scope.cargandoVisible = false;
        }, "json");
    }

    $scope.SerieRRSettings = {
        dataSource: serieDataSource,
        placeholder: "[Serie]",
        displayExpr: "S_NOMBRE",
        valueExpr: "ID_SERIE",
        searchEnabled: true,
        searchExpr: "S_NOMBRE",
        searchTimeout: 0,
        onValueChanged: function (data) {
            $scope.recuperadorRadicado.subSerie = null;
            $scope.recuperadorRadicado.unidadDocumental = null;

            registrosRelacionadosDataSource.store().clear();
            registrosRelacionadosDataSource.load();

            var grdRegistrosRelacionados = $('#grdRegistrosRelacionados').dxDataGrid('instance');
            grdRegistrosRelacionados.refresh();

            $scope.cargandoVisible = true;
            $.get($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/SubSerie?idSerie=' + data.value + '&tipoRadicador=' + $('#app').data('tipo'), function (dataSubSerie) {
                subSerieRRDataSource.store().clear();
                for (var i = 0; i < dataSubSerie.datos.length; i++) {
                    subSerieRRDataSource.store().insert(dataSubSerie.datos[i]);
                }
                subSerieRRDataSource.load();
                $scope.cargandoVisible = false;
            }, "json");
        },
        bindingOptions: { value: 'recuperadorRadicado.serie' }
    };

    $scope.SubSerieSettings = {
        dataSource: subSerieDataSource,
        placeholder: "[Sub-Serie]",
        displayExpr: "S_NOMBRE",
        valueExpr: "ID_SUBSERIE",
        searchEnabled: true,
        searchExpr: "S_NOMBRE",
        searchTimeout: 0,
        onValueChanged: function (data) {
            $scope.CambiaSubSerie(data.value);
        },
        bindingOptions: { value: 'radicador.subSerie' }
    };

    $scope.CambiaSubSerie = function (idSubSerie) {
        $scope.radicador.unidadDocumental = null;

        $scope.radicador.documentoAsociado = null;
        $scope.radicador.documentoAsociadoTextos = [];
        $scope.documentoAsociadoTextos = {};
        alert('b');
        $scope.documentoAsociadoLookup = false;
        $scope.documentoAsociadoText = false;
        $scope.documentoAsociadoPopupGrid = false;

        $scope.cargandoVisible = true;
        $.get($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/UnidadDocumental?idSerie=&idSubSerie=' + idSubSerie + '&tipoRadicador=' + $('#app').data('tipo'), function (dataUnidadDocumental) {
            unidadDocumentalDataSource.store().clear();
            for (var i = 0; i < dataUnidadDocumental.datos.length; i++) {
                unidadDocumentalDataSource.store().insert(dataUnidadDocumental.datos[i]);
            }
            unidadDocumentalDataSource.load();
            $scope.cargandoVisible = false;
        }, "json");
    }

    $scope.SubSerieRRSettings = {
        dataSource: subSerieRRDataSource,
        placeholder: "[Sub-Serie]",
        displayExpr: "S_NOMBRE",
        valueExpr: "ID_SUBSERIE",
        searchEnabled: true,
        searchExpr: "S_NOMBRE",
        searchTimeout: 0,
        onValueChanged: function (data) {
            $scope.recuperadorRadicado.unidadDocumental = null;

            registrosRelacionadosDataSource.store().clear();
            registrosRelacionadosDataSource.load();

            var grdRegistrosRelacionados = $('#grdRegistrosRelacionados').dxDataGrid('instance');
            grdRegistrosRelacionados.refresh();

            $scope.cargandoVisible = true;
            $.get($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/UnidadDocumental?idSerie=&idSubSerie=' + data.value + '&tipoRadicador=' + $('#app').data('tipo'), function (dataUnidadDocumental) {
                unidadDocumentalRRDataSource.store().clear();
                for (var i = 0; i < dataUnidadDocumental.datos.length; i++) {
                    unidadDocumentalRRDataSource.store().insert(dataUnidadDocumental.datos[i]);
                }
                unidadDocumentalRRDataSource.load();
                $scope.cargandoVisible = false;
            }, "json");
        },
        bindingOptions: { value: 'recuperadorRadicado.subSerie' }
    };

    $scope.UnidadDocumentalSettings = {
        dataSource: unidadDocumentalDataSource,
        placeholder: "[Unidad Documental]",
        displayExpr: "S_NOMBRE",
        valueExpr: "ID_UNIDADDOCUMENTAL",
        searchEnabled: true,
        searchExpr: "S_NOMBRE",
        searchTimeout: 0,
        onValueChanged: function (data) {
            $scope.CambiaUnidadDocumental(data.value, false);
        },
        bindingOptions: { value: 'radicador.unidadDocumental' }
    };

    $scope.CambiaUnidadDocumental = function (idUnidadDocumental, recuperadoEtiquetas) {
        if (idUnidadDocumental != null) {
            if (recuperadoEtiquetas) {
                $scope.recuperadorRadicado.documentoAsociado = null;
                $scope.recuperadorRadicado.documentoAsociadoTextos = [];

                $('#lupDocumentoAsociadoRR').dxValidator({
                    validationRules: [],
                });

                $('#cboDocumentoAsociadoRR').dxValidator({
                    validationRules: [],
                });
            } else {
                $scope.radicador.documentoAsociado = null;
                $scope.radicador.documentoAsociadoTextos = [];

                $('#lupDocumentoAsociado').dxValidator({
                    validationRules: [],
                });

                $('#cboDocumentoAsociado').dxValidator({
                    validationRules: [],
                });
            }

            $scope.documentoAsociadoTextos = {};

            alert('c');

            $scope.documentoAsociadoLookup = false;
            $scope.documentoAsociadoText = false;
            $scope.documentoAsociadoPopupGrid = false;

            $scope.cargandoVisible = true;
            $.get($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/DocumentoAsociado?idUnidadDocumental=' + idUnidadDocumental, function (data) {
                if (data.tipoRespuesta == 'OK' && data.configuracion.DocumentoAsociado.Nombre != null) {
                    switch (data.configuracion.DocumentoAsociado.TipoConsulta) {
                        case 1:
                            documentoAsociadoDataSource = new DevExpress.data.CustomStore({
                                load: function (loadOptions) {
                                    var d = $.Deferred();

                                    var searchValueOptions = loadOptions.searchValue;
                                    var searchExprOptions = loadOptions.searchExpr;

                                    var skip = loadOptions.skip;
                                    var take = loadOptions.take;

                                    $.getJSON($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/' + data.configuracion.DocumentoAsociado.Funcion, {
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
                                        alert('error cargando datos: ' + textStatus + ", " + error);
                                    });
                                    return d.promise();
                                },
                                byKey: function (key, extra) {
                                    return key.toString();
                                },
                            });

                            $('#lupDocumentoAsociado' + (recuperadoEtiquetas ? 'RR' : '')).dxLookup({
                                dataSource: documentoAsociadoDataSource,
                                placeholder: data.configuracion.DocumentoAsociado.PlaceHolder,
                                title: data.configuracion.DocumentoAsociado.Titulo,
                                displayExpr: "S_NOMBRE_LOOKUP",
                                valueExpr: "ID_LOOKUP",
                                usePopover: false,
                                bindingOptions: { value: (recuperadoEtiquetas ? 'recuperadorRadicado' : 'radicador') + '.documentoAsociado' },
                            }).dxValidator({
                                validationRules: [{
                                    type: 'required',
                                    message: data.configuracion.DocumentoAsociado.Titulo + ' Requerido'
                                }],
                            });

                            if (recuperadoEtiquetas) {
                                $scope.documentoAsociadoLookupRR = true;
                                $scope.documentoAsociadoTituloRR = data.configuracion.DocumentoAsociado.Titulo;
                            } else {
                                $scope.documentoAsociadoLookup = true;
                                $scope.documentoAsociadoTitulo = data.configuracion.DocumentoAsociado.Titulo;
                            }

                            break;
                        case 2: // TODO: Pendiente por implementar. Se requiere un control tipo SelectBox que permita detectar cuando se despliegan los items para cargar un popup con un grid para realizar la selección. La idea por ahora es poner un select box y cuando se presione click sobre él se carga el grid, luego al seleccionar un registro se alimenta el dataSource del SelectBox y se toma este elemento como seleccionado.
                            var columnas = [];

                            for (i = 0; i < data.configuracion.DocumentoAsociado.ColumnasCombo.length; i++) {
                                var columna = {
                                    dataField: data.configuracion.DocumentoAsociado.ColumnasCombo[i].Nombre,
                                    width: data.configuracion.DocumentoAsociado.ColumnasCombo[i].Ancho + '%',
                                    caption: data.configuracion.DocumentoAsociado.ColumnasCombo[i].Titulo,
                                    visible: data.configuracion.DocumentoAsociado.ColumnasCombo[i].Visible,
                                    dataType: (data.configuracion.DocumentoAsociado.ColumnasCombo[i].TipoDato == 'S' ? 'string' : (data.configuracion.DocumentoAsociado.ColumnasCombo[i].TipoDato == 'N' ? 'number' : (data.configuracion.DocumentoAsociado.ColumnasCombo[i].TipoDato == 'D' ? 'date' : (data.configuracion.DocumentoAsociado.ColumnasCombo[i].TipoDato == 'B' ? 'boolean' : 'string'))))
                                };
                                columnas.push(columna);
                            }

                            documentoAsociadoDataSource = new DevExpress.data.CustomStore({
                                load: function (loadOptions) {
                                    var d = $.Deferred();

                                    var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
                                    var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"' + data.configuracion.DocumentoAsociado.OrdenadoPor + '","desc":false}]';
                                    var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

                                    var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
                                    var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
                                    $.getJSON($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/' + data.configuracion.DocumentoAsociado.Funcion, {
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

                            var dataGrid = $('#grdDocumentoAsociado' + (recuperadoEtiquetas ? 'RR' : '')).dxDataGrid({
                                dataSource: documentoAsociadoDataSource,
                                allowColumnResizing: true,
                                loadPanel: { text: 'Cargando Datos...' },
                                paging: {
                                    pageSize: 8
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
                                columns: columnas,
                                onSelectionChanged: function (selecteditems) {
                                    var data = selecteditems.selectedRowsData[0];
                                    $scope.itemSeleccionado = true;
                                    $scope.ID_POPUP = data.ID_POPUP;
                                    $scope.NOMBRE_POPUP = data.NOMBRE_POPUP;

                                    var popup = $('#popDocumentoAsociado' + (recuperadoEtiquetas ? 'RR' : '')).dxPopup('instance');
                                    popup.hide();

                                    if (recuperadoEtiquetas)
                                    {
                                        $scope.cargandoVisible = true;
                                        $.get($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/RegistrosRelacionadosExpediente?idUnidadDocumental=' + $scope.recuperadorRadicado.unidadDocumental + '&idRelacionado=' + data.ID_POPUP, function (dataRegistros) {
                                            registrosRelacionadosDataSource.store().clear();
                                            for (var i = 0; i < dataRegistros.datos.length; i++) {
                                                registrosRelacionadosDataSource.store().insert(dataRegistros.datos[i]);
                                            }
                                            registrosRelacionadosDataSource.load();
                                            var grdRegistrosRelacionados = $('#grdRegistrosRelacionados').dxDataGrid('instance');
                                            grdRegistrosRelacionados.refresh();
                                            $scope.cargandoVisible = false;
                                        }, "json");
                                    }
                                }
                            });

                            $('#cboDocumentoAsociado' + (recuperadoEtiquetas ? 'RR' : '')).dxSelectBox({
                                dataSource: cboDocumentoAsociadoDataSource,
                                placeholder: data.configuracion.DocumentoAsociado.PlaceHolder,
                                displayExpr: 'NOMBRE_POPUP',
                                valueExpr: 'ID_POPUP',
                                usePopover: false,
                                bindingOptions: { value: (recuperadoEtiquetas ? 'recuperadorRadicado' : 'radicador') + '.documentoAsociado' },
                            }).dxValidator({
                                validationRules: [{
                                    type: 'required',
                                    message: data.configuracion.DocumentoAsociado.Titulo + ' Requerido'
                                }],
                            });

                            if (recuperadoEtiquetas) {
                                $scope.documentoAsociadoRRPopupGrid = true;
                                $scope.documentoAsociadoTituloRR = data.configuracion.DocumentoAsociado.Titulo;
                            } else {
                                $scope.documentoAsociadoPopupGrid = true;
                                $scope.documentoAsociadoTitulo = data.configuracion.DocumentoAsociado.Titulo;
                            }

                            break;
                        case 3:
                            $scope.documentoAsociadoTextos = data.configuracion.DocumentoAsociado.Campos;
                            $scope.$apply();

                            for (i = 0; i < data.configuracion.DocumentoAsociado.Campos.length; i++) { // Este ingreso de validadores NO FUNCIONA
                                $scope.radicador.documentoAsociadoTextos.push({ nombre: data.configuracion.DocumentoAsociado.Campos[i].Nombre, texto: '' });
                                $('#campo' + i).dxValidator({
                                    validationRules: [{
                                        type: 'required',
                                        message: data.configuracion.DocumentoAsociado.Campos[i].Titulo + ' Requerido'
                                    }],
                                });
                            }

                            $scope.documentoAsociadoText = true;
                            break;
                    }
                    $scope.cargandoVisible = false;
                } else {
                    $scope.cargandoVisible = false;
                    MostrarNotificacion('alert', '', 'Error - ' + data.detalleRespuesta);
                }
                //{

                //}

            }, "json");
        }
    }

    $scope.UnidadDocumentalRRSettings = {
        dataSource: unidadDocumentalRRDataSource,
        placeholder: "[Unidad Documental]",
        displayExpr: "S_NOMBRE",
        valueExpr: "ID_UNIDADDOCUMENTAL",
        searchEnabled: true,
        searchExpr: "S_NOMBRE",
        searchTimeout: 0,
        bindingOptions: { value: 'recuperadorRadicado.unidadDocumental' },
        onValueChanged: function (data) {
            registrosRelacionadosDataSource.store().clear();
            registrosRelacionadosDataSource.load();

            var grdRegistrosRelacionados = $('#grdRegistrosRelacionados').dxDataGrid('instance');
            grdRegistrosRelacionados.refresh();

            $scope.CambiaUnidadDocumental(data.value, true);
        },
    };

    $scope.TipoExpedienteRRSettings = {
        dataSource: tipoExpedienteDataSource,
        placeholder: "[Tipo Expediente]",
        displayExpr: "S_NOMBRE",
        valueExpr: "ID_TIPOEXPEDIENTE",
        searchEnabled: true,
        searchExpr: "S_NOMBRE",
        searchTimeout: 0,
        onValueChanged: function (data) {
            $scope.CambiaTipoExpediente(data.value, true);
        },
        bindingOptions: { value: 'recuperadorRadicado.tipoExpediente' }
    };

    $scope.tipoEtiquetaSettings = {
        displayExpr: 'text',
        valueExpr: 'value',
        dataSource: [{ text: 'Tomo', value: 'T' }, { text: 'Anexo', value: 'A' }],
        layout: 'horizontal',
        bindingOptions: { value: 'radicador.tipoEtiqueta' },
    };

    $scope.ValidarTipoAnexoEtiqueta = function (options) {
        if ($scope.radicador.tipoEtiqueta == 'A' && options.value == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    $scope.btnGenerarRadicadoSettings = {
        text: $('#app').data('tipo') == '1' ? 'Generar Radicado y Etiqueta' : 'Generar Etiqueta',
        type: 'success',
        onClick: function (params) {
            if ($scope.radicador.CB == null || $scope.radicador.CB.trim() == '') {
                var result = params.validationGroup.validate();

                if (!result.isValid) return;
            } else {
                var resultUbicacion = $('#txtUbicacion').dxValidator('instance').validate();
                var resultTipoAnexo = $('#tipoAnexo').dxValidator('instance').validate();
                var resultTipoEtiqueta = $('#tipoEtiqueta').dxValidator('instance').validate();

                if ($scope.radicador.tipoEtiqueta == 'A' && $scope.radicador.tipoAnexo == null) {
                    var result = DevExpress.ui.dialog.alert('Tipo de Anexo Requerido.', "Etiquetas");
                    return;
                }

                if (!(resultUbicacion.isValid && resultTipoAnexo.isValid && resultTipoEtiqueta.isValid)) return;
            }

            $('#btnGenerarRadicado').dxButton({
                disabled: true
            });

            $scope.cargandoVisible = true;

            if ($scope.radicador.CB != null && $scope.radicador.CB != '') {
                $http.post($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/Radicar', { tipoRetorno: 'key', formatoRetorno: '', datosRadicado: $scope.radicador }).success(function (data, status, headers, config) {
                    $('#btnGenerarRadicado').dxButton({
                        disabled: false
                    });

                    $scope.cargandoVisible = false;

                    if (data.id != null && data.id != '') {
                        if (data.tipoRespuesta == 'Error') {
                            var result = DevExpress.ui.dialog.confirm(data.detalleRespuesta, "Radicación");
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    /*popRadicado.SetContentUrl($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/GenerarEtiqueta?idRadicado=' + data.id + '&formatoRetorno=pdf');
                                    //popRadicado.SetContentUrl('@Url.Content("~")GestionDocumental/api/RadicadorApi/GenerarEtiqueta?idRadicado=' + JSON.parse(data) + '&idUnidadDocumental=' + $scope.radicador.unidadDocumental + '&formatoRetorno=pdf');
                                    popRadicado.Show();*/

                                    var popRadicado = $("#popRadicado").dxPopup("instance");
                                    popRadicado.show();

                                    $('#frmRadicado').attr('src', null);
                                    $('#frmRadicado').attr('src', $('#app').data('url') + 'GestionDocumental/api/RadicadorApi/GenerarEtiqueta?idRadicado=' + data.id + '&formatoRetorno=pdf');

                                    $scope.radicador.serie = null;
                                    $scope.radicador.subSerie = null;
                                    $scope.radicador.unidadDocumental = null;
                                    $scope.radicador.tipoExpediente = null;
                                    $scope.radicador.documentoAsociado = null;

                                    $scope.radicador.ubicacion = null;

                                    //params.validationGroup.reset();
                                }
                            });
                        } else {
                            /*popRadicado.SetContentUrl($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/GenerarEtiqueta?idRadicado=' + data.id + '&formatoRetorno=pdf');
                            //popRadicado.SetContentUrl('@Url.Content("~")GestionDocumental/api/RadicadorApi/GenerarEtiqueta?idRadicado=' + data.id + '&idUnidadDocumental=' + $scope.radicador.unidadDocumental + '&formatoRetorno=pdf');
                            popRadicado.Show();*/

                            var popRadicado = $("#popRadicado").dxPopup("instance");
                            popRadicado.show();

                            $('#frmRadicado').attr('src', null);
                            $('#frmRadicado').attr('src', $('#app').data('url') + 'GestionDocumental/api/RadicadorApi/GenerarEtiqueta?idRadicado=' + data.id + '&formatoRetorno=pdf');

                            $scope.radicador.serie = null;
                            $scope.radicador.subSerie = null;
                            $scope.radicador.unidadDocumental = null;
                            $scope.radicador.tipoExpediente = null;
                            $scope.radicador.documentoAsociado = null;

                            $scope.radicador.ubicacion = null;

                            //params.validationGroup.reset();
                        }
                    } else {
                        MostrarNotificacion('alert', '', 'Error - ' + data.detalleRespuesta);
                    }
                }).error(function (data, status, headers, config) {
                    $scope.cargandoVisible = false;
                    MostrarNotificacion('alert', '', 'Error Generando Radicado - ' + data);
                    $('#btnGenerarRadicado').dxButton({
                        disabled: false
                    });
                });
            } else {
                //$http.get('@Url.Content("~")GestionDocumental/api/RadicadorApi/Radicar?idUnidadDocumental=' + $scope.radicador.unidadDocumental + '&tipoRetorno=id&formatoRetorno=').success(function (data, status, headers, config) {
                //$http.get('@Url.Content("~")GestionDocumental/api/RadicadorApi/Radicar?idUnidadDocumental=' + $scope.radicador.unidadDocumental).success(function (data, status, headers, config) {
                $http.post($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/Radicar', { tipoRetorno: 'key', formatoRetorno: '', datosRadicado: $scope.radicador }).success(function (data, status, headers, config) {
                    $('#btnGenerarRadicado').dxButton({
                        disabled: false
                    });

                    $scope.cargandoVisible = false;

                    if (data.id != null && data.id != '') {
                        if (data.tipoRespuesta == 'Error') {
                            var result = DevExpress.ui.dialog.confirm(data.detalleRespuesta, "Radicación");
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    /*popRadicado.SetContentUrl($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/GenerarEtiqueta?idRadicado=' + data.id + '&formatoRetorno=pdf');
                                    //popRadicado.SetContentUrl('@Url.Content("~")GestionDocumental/api/RadicadorApi/GenerarEtiqueta?idRadicado=' + JSON.parse(data) + '&idUnidadDocumental=' + $scope.radicador.unidadDocumental + '&formatoRetorno=pdf');
                                    popRadicado.Show();*/

                                    var popRadicado = $("#popRadicado").dxPopup("instance");
                                    popRadicado.show();

                                    $('#frmRadicado').attr('src', null);
                                    $('#frmRadicado').attr('src', $('#app').data('url') + 'GestionDocumental/api/RadicadorApi/GenerarEtiqueta?idRadicado=' + data.id + '&formatoRetorno=pdf');

                                    /*$scope.radicador.serie = null;
                                    $scope.radicador.subSerie = null;
                                    $scope.radicador.unidadDocumental = null;
                                    $scope.radicador.documentoAsociado = null;*/

                                    $scope.radicador.ubicacion = null;
                                    $scope.radicador.consecutivo = null;

                                    //params.validationGroup.reset();
                                }
                            });
                        } else {
                            /*popRadicado.SetContentUrl($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/GenerarEtiqueta?idRadicado=' + data.id + '&formatoRetorno=pdf');
                            //popRadicado.SetContentUrl('@Url.Content("~")GestionDocumental/api/RadicadorApi/GenerarEtiqueta?idRadicado=' + data.id + '&idUnidadDocumental=' + $scope.radicador.unidadDocumental + '&formatoRetorno=pdf');
                            popRadicado.Show();*/

                            var popRadicado = $("#popRadicado").dxPopup("instance");
                            popRadicado.show();

                            $('#frmRadicado').attr('src', null);
                            $('#frmRadicado').attr('src', $('#app').data('url') + 'GestionDocumental/api/RadicadorApi/GenerarEtiqueta?idRadicado=' + data.id + '&formatoRetorno=pdf');

                            /*$scope.radicador.serie = null;
                            $scope.radicador.subSerie = null;
                            $scope.radicador.unidadDocumental = null;
                            $scope.radicador.documentoAsociado = null;*/

                            $scope.radicador.ubicacion = null;
                            $scope.radicador.consecutivo = null;

                            //params.validationGroup.reset();
                        }
                    } else {
                        MostrarNotificacion('alert', '', 'Error - ' + data.detalleRespuesta);
                    }
                }).error(function (data, status, headers, config) {
                    $scope.cargandoVisible = false;
                    MostrarNotificacion('alert', '', 'Error Generando Radicado - ' + data);
                    $('#btnGenerarRadicado').dxButton({
                        disabled: false
                    });
                });
            }
        }
    };

    $scope.btnRecuperarRadicadoSettings = {
        text: 'Recuperar Etiqueta',
        type: 'success',
        onClick: function (params) {
            var grdRegistrosRelacionados = $('#grdRegistrosRelacionados').dxDataGrid('instance');
            var registroSeleccionado = grdRegistrosRelacionados.getSelectedRowsData();

            if (registroSeleccionado.length > 0) {
                //$('#btnRecuperarRadicado').dxButton({
                //    disabled: true
                //});

                //$scope.cargandoVisible = true;
                //$http.get('@Url.Content("~")GestionDocumental/api/RadicadorApi/RecuperarRadicado?idUnidadDocumental=' + $scope.recuperadorRadicado.unidadDocumental + '&tipoRetorno=key&radicado=' + ).success(function (data, status, headers, config) {
                //    $('#btnRecuperarRadicado').dxButton({
                //        disabled: false
                //    });

                //    $scope.cargandoVisible = false;

                //    if (data.id != null && data.id != '') {


                /*popRadicado.SetContentUrl($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/GenerarEtiqueta?idRadicado=' + registroSeleccionado[0]['ID_RADICADO'] + '&formatoRetorno=pdf');
                popRadicado.Show();*/

                var popRadicado = $("#popRadicado").dxPopup("instance");
                popRadicado.show();

                $('#frmRadicado').attr('src', null);
                $('#frmRadicado').attr('src', $('#app').data('url') + 'GestionDocumental/api/RadicadorApi/GenerarEtiqueta?idRadicado=' + registroSeleccionado[0]['ID_RADICADO'] + '&formatoRetorno=pdf');


                //    } else {
                //        MostrarNotificacion('alert', '', 'Error - ' + data.detalleRespuesta);
                //    }
                //}).error(function (data, status, headers, config) {
                //    $scope.cargandoVisible = false;
                //    MostrarNotificacion('alert', '', 'Error Recuperando Radicado - ' + data);
                //    $('#btnRecuperarRadicado').dxButton({
                //        disabled: false
                //    });
                //});

                grdRegistrosRelacionados.clearSelection();
            } else {
                MostrarNotificacion('alert', '', 'Debe Seleccionar un Registro para Recuperar la Etiqueta.');
            }
        }
    };

    $scope.btnGenerarAnexoSettings = {
        text: 'Generar Etiqueta Anexo',
        type: 'success',
        onClick: function (params) {
            var result = params.validationGroup.validate();
            if (!result.isValid) return;

            if ($scope.anexo.CB != null && $scope.anexo.CB.trim() != '') {
                $('#btnGenerarAnexo').dxButton({
                    disabled: true
                });

                $scope.cargandoVisible = true;
                $http.get($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/RadicarAnexo?codigoBarras=' + $scope.anexo.CB + '&tipoAnexo=' + $scope.anexo.tipo).success(function (data, status, headers, config) {
                    $('#btnGenerarAnexo').dxButton({
                        disabled: false
                    });

                    $scope.cargandoVisible = false;

                    if (data.id != null && data.id != '') {
                        /*popRadicado.SetContentUrl($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/GenerarEtiqueta?idRadicado=' + data.id + '&formatoRetorno=pdf');
                        //popRadicado.SetContentUrl('@Url.Content("~")GestionDocumental/api/RadicadorApi/GenerarEtiqueta?idRadicado=' + JSON.parse(data) + '&idUnidadDocumental=' + $scope.radicador.unidadDocumental + '&formatoRetorno=pdf');
                        popRadicado.Show();*/

                        var popRadicado = $("#popRadicado").dxPopup("instance");
                        popRadicado.show();

                        $('#frmRadicado').attr('src', null);
                        $('#frmRadicado').attr('src', $('#app').data('url') + 'GestionDocumental/api/RadicadorApi/GenerarEtiqueta?idRadicado=' + data.id + '&formatoRetorno=pdf');

                    } else {
                        MostrarNotificacion('alert', '', 'Error - ' + data.detalleRespuesta);
                    }
                }).error(function (data, status, headers, config) {
                    $scope.cargandoVisible = false;
                    MostrarNotificacion('alert', '', 'Error Generando Radicado - ' + data);
                    $('#btnGenerarAnexo').dxButton({
                        disabled: false
                    });
                });
            }
        }
    };

    $scope.btnGenerarTomoSettings = {
        text: 'Generar Etiqueta Tomo',
        type: 'success',
        onClick: function (params) {
            var result = params.validationGroup.validate();
            if (!result.isValid) return;

            if ($scope.tomo.CB != null && $scope.tomo.CB.trim() != '') {
                $('#btnGenerarTomoo').dxButton({
                    disabled: true
                });

                $scope.cargandoVisible = true;
                $http.get($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/RadicarTomo?codigoBarras=' + $scope.tomo.CB).success(function (data, status, headers, config) {
                    $('#btnGenerarTomo').dxButton({
                        disabled: false
                    });

                    $scope.cargandoVisible = false;

                    if (data.id != null && data.id != '') {
                        /*popRadicado.SetContentUrl($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/GenerarEtiqueta?idRadicado=' + data.id + '&formatoRetorno=pdf');
                        popRadicado.Show();*/

                        var popRadicado = $("#popRadicado").dxPopup("instance");
                        popRadicado.show();

                        $('#frmRadicado').attr('src', null);
                        $('#frmRadicado').attr('src', $('#app').data('url') + 'GestionDocumental/api/RadicadorApi/GenerarEtiqueta?idRadicado=' + data.id + '&formatoRetorno=pdf');
                    } else {
                        MostrarNotificacion('alert', '', 'Error - ' + data.detalleRespuesta);
                    }
                }).error(function (data, status, headers, config) {
                    $scope.cargandoVisible = false;
                    MostrarNotificacion('alert', '', 'Error Generando Radicado - ' + data);
                    $('#btnGenerarTomo').dxButton({
                        disabled: false
                    });
                });
            }
        }
    };

    $scope.grdRegistrosRelacionadosSettings = {
        dataSource: registrosRelacionadosDataSource,
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
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
                dataField: 'ID_RADICADO',
                width: '25%',
                caption: 'ID',
                dataType: 'string',
                visible: false,
            }, {
                dataField: 'S_ETIQUETA',
                width: '35%',
                caption: 'Código de Barras',
                dataType: 'string',
            }, {
                dataField: 'S_CONSECUTIVO',
                width: '15%',
                caption: 'Cons Anexo / Tomo',
                dataType: 'string',
            }, {
                dataField: 'S_TIPO',
                width: '25%',
                caption: 'Tipo',
                dataType: 'string',
            }, {
                dataField: 'S_TIPOANEXO',
                width: '25%',
                caption: 'Tipo Anexo',
                dataType: 'string',
            }
        ],
    };

    // La siguiente función no se utiliza debido a la complejidad de cargar los datos en los combos y sobre todo los datos dinámicos
    $scope.CargarDatosAsociadosCB = function (object) {
        var e = object.jQueryEvent;

        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            // Allow: Ctrl+A
            (e.keyCode == 65 && e.ctrlKey === true) ||
            // Allow: Ctrl+C
            (e.keyCode == 67 && e.ctrlKey === true) ||
            // Allow: Ctrl+X
            (e.keyCode == 88 && e.ctrlKey === true) ||
            // Allow: home, end, left, right
            (e.keyCode >= 35 && e.keyCode <= 39)) {
            // let it happen, don't do anything

            if (e.keyCode == 13) {
                $scope.cargandoVisible = true;
                $.get($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/DatosAsociadosCB?CB=' + $scope.radicador.CB, function (dataRegistros) {
                    $scope.showWait = false;
                    $scope.returnCallBack = false;
                   // $scope.radicador.serie = 1;
                    //$scope.radicador.subSerie = 1;
                    //$scope.radicador.unidadDocumental = 20;
                    $scope.cargandoVisible = false;
                    $scope.showWait = true;
                }, "json");
            }

            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    };

    $scope.CargarRegistrosRelacionados = function (object) {
        var result = DevExpress.validationEngine.validateGroup($("#recuperacionEtiquetasGroup").dxValidationGroup("instance"));

        if (!result.isValid) return;

        var e = object.jQueryEvent;

        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            // Allow: Ctrl+A
            (e.keyCode == 65 && e.ctrlKey === true) ||
            // Allow: Ctrl+C
            (e.keyCode == 67 && e.ctrlKey === true) ||
            // Allow: Ctrl+X
            (e.keyCode == 88 && e.ctrlKey === true) ||
            // Allow: home, end, left, right
            (e.keyCode >= 35 && e.keyCode <= 39)) {
            // let it happen, don't do anything

            if (e.keyCode == 13) {
                $scope.cargandoVisible = true;
                $.get($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/RegistrosRelacionadosRadicado?idUnidadDocumental=' + $scope.recuperadorRadicado.unidadDocumental + '&idRadicado=' + $scope.recuperadorRadicado.radicado, function (dataRegistros) {
                    registrosRelacionadosDataSource.store().clear();
                    for (var i = 0; i < dataRegistros.datos.length; i++) {
                        registrosRelacionadosDataSource.store().insert(dataRegistros.datos[i]);
                    }
                    registrosRelacionadosDataSource.load();
                    var grdRegistrosRelacionados = $('#grdRegistrosRelacionados').dxDataGrid('instance');
                    grdRegistrosRelacionados.refresh();
                    $scope.cargandoVisible = false;
                }, "json");
            }

            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    };
});

var serieDataSource = new DevExpress.data.DataSource([]);
var subSerieDataSource = new DevExpress.data.DataSource([]);
var unidadDocumentalDataSource = new DevExpress.data.DataSource([]);
var cboDocumentoAsociadoDataSource = new DevExpress.data.DataSource([]);
var documentoAsociadoDataSource = new DevExpress.data.CustomStore();
var subSerieRRDataSource = new DevExpress.data.DataSource([]);
var unidadDocumentalRRDataSource = new DevExpress.data.DataSource([]);
var registrosRelacionadosDataSource = new DevExpress.data.DataSource([]);
var tipoExpedienteDataSource = new DevExpress.data.DataSource([]);

function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        DevExpress.ui.dialog.alert(msg, 'Radicación');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }

}
