var SIMApp = angular.module('SIM', ['dx']);

SIMApp.controller("ReporteDinamicoController", function ($scope, $location, $http) {
    $('.my-cloak').removeClass('my-cloak');

    $scope.filtroId = -1;
    $scope.tituloAgrupar = '';

    $scope.parametros = {};
    $scope.parametros.ID_REPORTE = 0;

    $scope.filtros = [];
    $scope.filtrosConsulta = null;
    $scope.agruparIndexConsulta = -1;
    $scope.itemsSeleccionados = [];

    $scope.agrupar = false;
    $scope.agruparIndex = -1;

    $scope.selectedTab = -1;
    $scope.cargandoVisible = false;

    $scope.consultaGenerada = false;

    $scope.tabsData = [
            { text: 'Filtros', pos: 0 },
            { text: 'Resultado Consulta', pos: 1 },
    ];

    $scope.dxTabsOptions = {
        dataSource: $scope.tabsData,
        onItemClick: (function (itemData) {
            $scope.selectedTab = itemData.itemIndex;
        }),
        selectedIndex: 0,
        bindingOptions: { visible: 'parametros.ID_REPORTE > 0' }
    };

    $scope.isActive = function (tabIndex) {
        if ($scope.selectedTab === tabIndex)
            return true;
        else
            return false;
    };

    $scope.filtroChanged = function (e) {
        var filtrosDependientes = $scope.ObtenerFiltrosDependientes(e.element.data('idfiltro'));

        if (filtrosDependientes != null) {
            for (j = 0; j < filtrosDependientes.length; j++) {
                var filtro = search(filtrosDependientes[j], $scope.filtros);

                if (filtro.tipo == 3) {
                    $('#filtrolup' + filtrosDependientes[j]).dxLookup('instance').option('value', null);

                    switch (filtro.id) {
                        case 1:
                            $('#filtrolup' + filtrosDependientes[j]).dxLookup('instance').option('dataSource', $scope.filtroDataSource1);
                            break;
                        case 2:
                            $('#filtrolup' + filtrosDependientes[j]).dxLookup('instance').option('dataSource', $scope.filtroDataSource2);
                            break;
                        case 3:
                            $('#filtrolup' + filtrosDependientes[j]).dxLookup('instance').option('dataSource', $scope.filtroDataSource3);
                            break;
                        case 4:
                            $('#filtrolup' + filtrosDependientes[j]).dxLookup('instance').option('dataSource', $scope.filtroDataSource4);
                            break;
                        case 5:
                            $('#filtrolup' + filtrosDependientes[j]).dxLookup('instance').option('dataSource', $scope.filtroDataSource5);
                            break;
                        case 6:
                            $('#filtrolup' + filtrosDependientes[j]).dxLookup('instance').option('dataSource', $scope.filtroDataSource6);
                            break;
                        case 7:
                            $('#filtrolup' + filtrosDependientes[j]).dxLookup('instance').option('dataSource', $scope.filtroDataSource7);
                            break;
                        case 8:
                            $('#filtrolup' + filtrosDependientes[j]).dxLookup('instance').option('dataSource', $scope.filtroDataSource8);
                            break;
                        case 9:
                            $('#filtrolup' + filtrosDependientes[j]).dxLookup('instance').option('dataSource', $scope.filtroDataSource9);
                            break;
                    }
                }

                if (filtro.tipo == 4) {
                    if (e.component.option('value') == null)
                        $('#tvwFiltro').dxTreeView('instance').option('value', null);
                    else
                        $('#tvwFiltro').dxTreeView('instance').option('dataSource', $scope.filtroTreeDataSource);
                }
            }
        }
    }

    function EsHoja(data) {
        return !data.items.length;
    }

    function ProcesarNodo(nodo) {
        var itemIndex = -1;

        $.each($scope.itemsSeleccionados, function (index, item) {
            if (item.key === nodo.key) {
                itemIndex = index;
                return false;
            }
        });

        if (nodo.selected && itemIndex === -1) {
            $scope.itemsSeleccionados.push(nodo);
        } else if (!nodo.selected) {
            $scope.itemsSeleccionados.splice(itemIndex, 1);
        }
    }

    $scope.TreeViewSeleccion= function(e) {
        var item = e.node;
    
        if (EsHoja(item)) {
            ProcesarNodo(item);
        } else {
            $.each(item.items, function(index, nodo) {
                ProcesarNodo(nodo);
            });
        }
        //alert($scope.ObtenerItemsSeleccionados());
    }

    $scope.ObtenerItemsSeleccionados = function () {
        var items = '';

        for (k=0; k < $scope.itemsSeleccionados.length; k++)
        {
            if (items == '')
                items = $scope.itemsSeleccionados[k].key;
            else
                items += ',' + $scope.itemsSeleccionados[k].key;
        }

        return items;
    }

    $scope.reporteSelectBoxSettings = {
        dataSource: cboReporteDataSource,
        valueExpr: 'ID',
        displayExpr: 'NOMBRE',
        placeholder: '[Seleccionar Reporte]',
        bindingOptions: { value: 'parametros.ID_REPORTE' },
        onValueChanged: function (data) {
            $scope.cargandoVisible = true;

            $scope.idReporte = data.value;

            $scope.CargarConfiguracionReporte();
        },
    };

    $scope.CargarConfiguracionReporte = function () {
        $.getJSON($('#app').data('url') + 'Reporte/api/DinamicoApi/FiltrosReporte', { idReporte: $scope.idReporte }).done(function (resultado) {
            $scope.tituloAgrupar = resultado.titulo;
            $scope.filtros = resultado.filtros;

            $scope.agrupar = (resultado.agrupar != null);
            $scope.agruparIndex = -1;
            $scope.itemsSeleccionados = [];

            if ($scope.agrupar) {
                $('#cboAgrupar').dxSelectBox('instance').option('dataSource', resultado.agrupar);
            }

            $scope.cargandoVisible = false;

            var tab = $('#tabOpciones').dxTabs('instance');
            tab.option('selectedIndex', 0);
            $scope.selectedTab = 0;
        }).fail(function (jqxhr, textStatus, error) {
            $scope.cargandoVisible = false;
            alert('error cargando filtros del reporte: ' + textStatus + ", " + error);
        });
    };

    $scope.gridOptions = {
        dataSource: null,
        showBorders: true,
        selection: {
            mode: "multiple"
        },
        "export": {
            enabled: true,
            fileName: "Consulta",
            allowExportSelectedData: true
        },
        groupPanel: {
            visible: false
        },
        columns: [
            {
                dataField: "Prefix",
                caption: "Title",
                width: 60
            }, "FirstName",
            "LastName",
            "City",
            "State", {
                dataField: "Position",
                width: 130
            }, {
                dataField: "BirthDate",
                dataType: "date",
                width: 100
            }, {
                dataField: "HireDate",
                dataType: "date",
                width: 100
            }
        ]
    };

    $scope.limpiarSettings = {
        text: 'Limpiar Filtros',
        type: 'success',
        width: '100%',
        bindingOptions: { visible: 'filtros.length > 0' },
        onClick: function (params) {
            for (i = 0; i < $scope.filtros.length; i++) {
                $scope.filtros[i].valor = null;

                if ($scope.filtros[i].tipo == 4)
                {
                    $('#tvwFiltro').dxTreeView('instance').unselectAll();
                    $('#tvwFiltro').dxTreeView('instance').option('dataSource', null);
                    $scope.itemsSeleccionados = [];
                    $scope.$apply();
                }
            }
        }
    };

    $scope.consultarSettings = {
        text: 'Consultar',
        type: 'success',
        width: '100%',
        onClick: function (params) {
            //alert(JSON.stringify($scope.filtros));
            $scope.cargandoVisible = true;

            var filtrosSeleccionados = '';
            for (i = 0; i < $scope.filtros.length; i++) {
                var valor = $scope.filtros[i].valor;

                if ($scope.filtros[i].tipo == 4)
                    valor = $scope.ObtenerItemsSeleccionados();

                if (filtrosSeleccionados == '')
                    filtrosSeleccionados += $scope.filtros[i].id + '|' + $scope.filtros[i].tipo + '|' + $scope.filtros[i].campo + '|' + (valor == null ? '' : valor);
                else
                    filtrosSeleccionados += '&&' + $scope.filtros[i].id + '|' + $scope.filtros[i].tipo + '|' + $scope.filtros[i].campo + '|' + (valor == null ? '' : valor);
            }

            $scope.filtrosConsulta = filtrosSeleccionados;
            $scope.agruparIndexConsulta = $scope.agruparIndex;

            $.getJSON($('#app').data('url') + 'Reporte/api/DinamicoApi/ConsultaConfig', { idReporte: $scope.idReporte, agrupar: $scope.agruparIndexConsulta }).done(function (resultado) {
                $('#grdReporte').dxDataGrid({
                    dataSource: $scope.consultaDataSource,
                    allowColumnResizing: true,
                    showBorders: true,
                    loadPanel: { text: 'Cargando Datos...' },
                    paging: {
                        pageSize: 10
                    },
                    pager: {
                        showPageSizeSelector: true,
                        allowedPageSizes: [5, 10, 20, 50]
                    },
                    "export": {
                        enabled: true,
                        fileName: "Reporte",
                        allowExportSelectedData: false
                    },
                    filterRow: {
                        visible: false,
                    },
                    groupPanel: {
                        visible: false
                    },
                    selection: {
                        mode: 'single'
                    },
                    sorting: {
                        mode: 'none'
                    },
                    onCellPrepared: function (e) {
                        if (e.rowType === "header")
                            e.cellElement.css("text-align", "left");
                    },
                    columns: resultado.datos
                });
            }).fail(function (jqxhr, textStatus, error) {
                $scope.cargandoVisible = false;
                alert('error cargando configuración del reporte: ' + textStatus + ", " + error);
            });

            $scope.consultaGenerada = true;

            /*var filtrosSeleccionados = '';
            for (i = 0; i < $scope.filtros.length; i++)
            {
                if (filtrosSeleccionados == '')
                    filtrosSeleccionados += $scope.filtros[i].id + '|' + $scope.filtros[i].tipo + '|' + $scope.filtros[i].campo + '|' + $scope.filtros[i].valor;
                else
                    filtrosSeleccionados += '&&' + $scope.filtros[i].id + '|' + $scope.filtros[i].tipo + '|' + $scope.filtros[i].campo + '|' + $scope.filtros[i].valor;
            }

            alert(filtrosSeleccionados);

            $.getJSON($('#app').data('url') + 'Reporte/api/DinamicoApi/Consulta', { idReporte: $scope.idReporte, valoresFiltros: filtrosSeleccionados }).done(function (resultado) {
                alert('ok');
            }).fail(function (jqxhr, textStatus, error) {
                alert('error cargando filtros del reporte: ' + textStatus + ", " + error);
            });*/
        }
    };

    $scope.consultaDataSource = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var d = $.Deferred();

            var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '';
            var skip = (loadOptions.skip == null ? 0 : loadOptions.skip);
            var take = (loadOptions.take == null ? 0 : loadOptions.take);

            $.getJSON($('#app').data('url') + 'Reporte/api/DinamicoApi/Consulta', {
                idReporte: $scope.idReporte,
                idFiltro: 1,
                sort: sortOptions,
                skip: skip,
                take: take,
                valoresFiltros: $scope.filtrosConsulta,
                agrupar: $scope.agruparIndexConsulta
            }).done(function (data) {
                d.resolve(data.datos, { totalCount: data.numRegistros });

                var tab = $('#tabOpciones').dxTabs('instance');
                tab.option('selectedIndex', 1);
                $scope.selectedTab = 1;

                $scope.cargandoVisible = false;

                $scope.$apply();
            }).fail(function (jqxhr, textStatus, error) {
                $scope.cargandoVisible = false;
                alert('error FiltroConsulta: ' + textStatus + ", " + error);
            });
            return d.promise();
        },
        byKey: function (key, extra) {
            //alert('b');
            return key.toString();
        },
    });

    $scope.filtroDataSource1 = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var d = $.Deferred();

            var searchValueOptions = loadOptions.searchValue;
            var searchExprOptions = loadOptions.searchExpr;

            var skip = (loadOptions.skip == null ? 0 : loadOptions.skip);
            var take = (loadOptions.take == null ? 0 : loadOptions.take);
            $.getJSON($('#app').data('url') + 'Reporte/api/DinamicoApi/ConsultaFiltro', {
                idReporte: $scope.idReporte,
                idFiltro: 1,
                skip: skip,
                take: take,
                searchValue: (searchValueOptions === undefined || searchValueOptions === null ? '' : searchValueOptions),
                valoresFiltros: $scope.ObtenerFiltrosRestriccion(1)
            }).done(function (data) {
                d.resolve(data.datos, { totalCount: data.numRegistros });
            }).fail(function (jqxhr, textStatus, error) {
                alert('error FiltroConsulta: ' + textStatus + ", " + error);
            });
            return d.promise();
        },
        byKey: function (key, extra) {
            //alert('b');
            return key.toString();
        },
    });

    $scope.filtroDataSource2 = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var d = $.Deferred();

            var searchValueOptions = loadOptions.searchValue;
            var searchExprOptions = loadOptions.searchExpr;

            var skip = (loadOptions.skip == null ? 0 : loadOptions.skip);
            var take = (loadOptions.take == null ? 0 : loadOptions.take);

            ////alert('b');
            $.getJSON($('#app').data('url') + 'Reporte/api/DinamicoApi/ConsultaFiltro', {
                idReporte: $scope.idReporte,
                idFiltro: 2,
                skip: skip,
                take: take,
                searchValue: (searchValueOptions === undefined || searchValueOptions === null ? '' : searchValueOptions),
                valoresFiltros: $scope.ObtenerFiltrosRestriccion(2)
            }).done(function (data) {
                d.resolve(data.datos, { totalCount: data.numRegistros });
            }).fail(function (jqxhr, textStatus, error) {
                alert('error FiltroConsulta: ' + textStatus + ", " + error);
            });
            return d.promise();
        },
        byKey: function (key, extra) {
            //alert('b');
            return key.toString();
        },
    });

    $scope.filtroDataSource3 = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var d = $.Deferred();

            var searchValueOptions = loadOptions.searchValue;
            var searchExprOptions = loadOptions.searchExpr;

            var skip = (loadOptions.skip == null ? 0 : loadOptions.skip);
            var take = (loadOptions.take == null ? 0 : loadOptions.take);

            $.getJSON($('#app').data('url') + 'Reporte/api/DinamicoApi/ConsultaFiltro', {
                idReporte: $scope.idReporte,
                idFiltro: 3,
                skip: skip,
                take: take,
                searchValue: (searchValueOptions === undefined || searchValueOptions === null ? '' : searchValueOptions),
                valoresFiltros: $scope.ObtenerFiltrosRestriccion(3)
            }).done(function (data) {
                d.resolve(data.datos, { totalCount: data.numRegistros });
            }).fail(function (jqxhr, textStatus, error) {
                alert('error FiltroConsulta: ' + textStatus + ", " + error);
            });
            return d.promise();
        },
        byKey: function (key, extra) {
            //alert('b');
            return key.toString();
        },
    });

    $scope.filtroDataSource4 = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var d = $.Deferred();

            var searchValueOptions = loadOptions.searchValue;
            var searchExprOptions = loadOptions.searchExpr;

            var skip = (loadOptions.skip == null ? 0 : loadOptions.skip);
            var take = (loadOptions.take == null ? 0 : loadOptions.take);
            $.getJSON($('#app').data('url') + 'Reporte/api/DinamicoApi/ConsultaFiltro', {
                idReporte: $scope.idReporte,
                idFiltro: 4,
                skip: skip,
                take: take,
                searchValue: (searchValueOptions === undefined || searchValueOptions === null ? '' : searchValueOptions),
                valoresFiltros: $scope.ObtenerFiltrosRestriccion(4)
            }).done(function (data) {
                d.resolve(data.datos, { totalCount: data.numRegistros });
            }).fail(function (jqxhr, textStatus, error) {
                alert('error FiltroConsulta: ' + textStatus + ", " + error);
            });
            return d.promise();
        },
        byKey: function (key, extra) {
            //alert('b');
            return key.toString();
        },
    });

    $scope.filtroDataSource5 = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var d = $.Deferred();

            var searchValueOptions = loadOptions.searchValue;
            var searchExprOptions = loadOptions.searchExpr;

            var skip = (loadOptions.skip == null ? 0 : loadOptions.skip);
            var take = (loadOptions.take == null ? 0 : loadOptions.take);
            $.getJSON($('#app').data('url') + 'Reporte/api/DinamicoApi/ConsultaFiltro', {
                idReporte: $scope.idReporte,
                idFiltro: 5,
                skip: skip,
                take: take,
                searchValue: (searchValueOptions === undefined || searchValueOptions === null ? '' : searchValueOptions),
                valoresFiltros: $scope.ObtenerFiltrosRestriccion(5)
            }).done(function (data) {
                d.resolve(data.datos, { totalCount: data.numRegistros });
            }).fail(function (jqxhr, textStatus, error) {
                alert('error FiltroConsulta: ' + textStatus + ", " + error);
            });
            return d.promise();
        },
        byKey: function (key, extra) {
            //alert('b');
            return key.toString();
        },
    });

    $scope.filtroDataSource6 = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var d = $.Deferred();

            var searchValueOptions = loadOptions.searchValue;
            var searchExprOptions = loadOptions.searchExpr;

            var skip = (loadOptions.skip == null ? 0 : loadOptions.skip);
            var take = (loadOptions.take == null ? 0 : loadOptions.take);
            $.getJSON($('#app').data('url') + 'Reporte/api/DinamicoApi/ConsultaFiltro', {
                idReporte: $scope.idReporte,
                idFiltro: 6,
                skip: skip,
                take: take,
                searchValue: (searchValueOptions === undefined || searchValueOptions === null ? '' : searchValueOptions),
                valoresFiltros: $scope.ObtenerFiltrosRestriccion(6)
            }).done(function (data) {
                d.resolve(data.datos, { totalCount: data.numRegistros });
            }).fail(function (jqxhr, textStatus, error) {
                alert('error FiltroConsulta: ' + textStatus + ", " + error);
            });
            return d.promise();
        },
        byKey: function (key, extra) {
            //alert('b');
            return key.toString();
        },
    });

    $scope.filtroDataSource7 = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var d = $.Deferred();

            var searchValueOptions = loadOptions.searchValue;
            var searchExprOptions = loadOptions.searchExpr;

            var skip = (loadOptions.skip == null ? 0 : loadOptions.skip);
            var take = (loadOptions.take == null ? 0 : loadOptions.take);
            $.getJSON($('#app').data('url') + 'Reporte/api/DinamicoApi/ConsultaFiltro', {
                idReporte: $scope.idReporte,
                idFiltro: 7,
                skip: skip,
                take: take,
                searchValue: (searchValueOptions === undefined || searchValueOptions === null ? '' : searchValueOptions),
                valoresFiltros: $scope.ObtenerFiltrosRestriccion(7)
            }).done(function (data) {
                d.resolve(data.datos, { totalCount: data.numRegistros });
            }).fail(function (jqxhr, textStatus, error) {
                alert('error FiltroConsulta: ' + textStatus + ", " + error);
            });
            return d.promise();
        },
        byKey: function (key, extra) {
            //alert('b');
            return key.toString();
        },
    });

    $scope.filtroDataSource8 = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var d = $.Deferred();

            var searchValueOptions = loadOptions.searchValue;
            var searchExprOptions = loadOptions.searchExpr;

            var skip = (loadOptions.skip == null ? 0 : loadOptions.skip);
            var take = (loadOptions.take == null ? 0 : loadOptions.take);
            $.getJSON($('#app').data('url') + 'Reporte/api/DinamicoApi/ConsultaFiltro', {
                idReporte: $scope.idReporte,
                idFiltro: 8,
                skip: skip,
                take: take,
                searchValue: (searchValueOptions === undefined || searchValueOptions === null ? '' : searchValueOptions),
                valoresFiltros: $scope.ObtenerFiltrosRestriccion(8)
            }).done(function (data) {
                d.resolve(data.datos, { totalCount: data.numRegistros });
            }).fail(function (jqxhr, textStatus, error) {
                alert('error FiltroConsulta: ' + textStatus + ", " + error);
            });
            return d.promise();
        },
        byKey: function (key, extra) {
            //alert('b');
            return key.toString();
        },
    });

    $scope.filtroDataSource9 = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var d = $.Deferred();

            var searchValueOptions = loadOptions.searchValue;
            var searchExprOptions = loadOptions.searchExpr;

            var skip = (loadOptions.skip == null ? 0 : loadOptions.skip);
            var take = (loadOptions.take == null ? 0 : loadOptions.take);
            $.getJSON($('#app').data('url') + 'Reporte/api/DinamicoApi/ConsultaFiltro', {
                idReporte: $scope.idReporte,
                idFiltro: 9,
                skip: skip,
                take: take,
                searchValue: (searchValueOptions === undefined || searchValueOptions === null ? '' : searchValueOptions),
                valoresFiltros: $scope.ObtenerFiltrosRestriccion(9)
            }).done(function (data) {
                d.resolve(data.datos, { totalCount: data.numRegistros });
            }).fail(function (jqxhr, textStatus, error) {
                alert('error FiltroConsulta: ' + textStatus + ", " + error);
            });
            return d.promise();
        },
        byKey: function (key, extra) {
            //alert('b');
            return key.toString();
        },
    });

    $scope.filtroCboDataSource = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var d = $.Deferred();

            $.getJSON($('#app').data('url') + 'Reporte/api/DinamicoApi/ConsultaFiltro?').done(function (data) {
                d.resolve(data.datos);
            }).fail(function (jqxhr, textStatus, error) {
                alert('error cargando datos: ' + textStatus + ", " + error);
            });
            return d.promise();
        },
        byKey: function (key, extra) {
            return key.toString();
        },
    });

    $scope.filtroTreeDataSource = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var d = $.Deferred();

            var idFiltro = 5; //$('#tvwFiltro').data('idfiltro');

            $.getJSON($('#app').data('url') + 'Reporte/api/DinamicoApi/ConsultaFiltroTodos', {
                idReporte: $scope.idReporte,
                idFiltro: idFiltro,
                valoresFiltros: $scope.ObtenerFiltrosRestriccion(idFiltro)
            }).done(function (data) {
                d.resolve(data.datos);
            }).fail(function (jqxhr, textStatus, error) {
                alert('error cargando datos: ' + textStatus + ", " + error);
            });
            return d.promise();
        },
        byKey: function (key, extra) {
            return key.toString();
        },
    });

    $scope.ObtenerFiltrosRestriccion = function (id) {
        var filtrosRestriccion = [];
        var filtroActual = search(id, $scope.filtros);

        if (filtroActual != null) {
            var listaFiltrosRestriccion = filtroActual.idFiltrosRestriccion.split(',');

            for (i = 0; i < listaFiltrosRestriccion.length; i++) {
                if (listaFiltrosRestriccion[i] != null && listaFiltrosRestriccion[i].trim() != '') {
                    var filtroRestriccion = search(listaFiltrosRestriccion[i].split('|')[0], $scope.filtros);

                    if (filtroRestriccion != null) {
                        switch (filtroRestriccion.tipo)
                        {
                            case 3:
                                //var lookup = $('#filtrolup' + filtroRestriccion.id).dxLookup('instance');
                                var valor = (filtroRestriccion.valor == null ? '' : (filtroRestriccion.tipoId.trim().toUpperCase() == 'INT' ? filtroRestriccion.valor : '\'' + filtroRestriccion.valor + '\''));
                                filtrosRestriccion.push(filtroRestriccion.id + '|' + listaFiltrosRestriccion[i].split('|')[1] + '|' + valor);
                                break;
                        }
                    }
                }
            }

            if (filtrosRestriccion.length > 0)
                return filtrosRestriccion.join(',');
            else
                return '';
        }

        return '';
    }

    $scope.ObtenerFiltrosDependientes = function (id) {
        var filtrosDependientes = [];
        var filtroActual = search(id, $scope.filtros);

        if (filtroActual != null) {
            if (filtroActual.idFiltrosDependientes.trim() != '') {
                filtrosDependientes = filtroActual.idFiltrosDependientes.split(',');

                return filtrosDependientes;
            } else
                return null;
        }

        return null;
    }

    if ($('#app').data('idreporte') != null && $('#app').data('idreporte') != undefined) {
        $scope.idReporte = parseInt($('#app').data('idreporte'));
        $scope.parametros.ID_REPORTE = $scope.idReporte;
        $scope.CargarConfiguracionReporte();
    } else {
        $scope.idReporte = -1;
    }
});

var cboReporteDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        $.getJSON($('#app').data('url') + 'Reporte/api/DinamicoApi/Reportes').done(function (data) {
            d.resolve(data.datos);
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + error);
        });
        return d.promise();
    },
    byKey: function (key, extra) {
        return key.toString();
    },
});

function search(id, arrayList) {
    for (var i = 0; i < arrayList.length; i++) {
        if (arrayList[i].id.toString() === id.toString()) {
            return arrayList[i];
        }
    }

    return null;
}

function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        DevExpress.ui.dialog.alert(msg, 'Reporte');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}
