var SIMApp = angular.module('SIM', ['dx']);

var codTareaSel = null;
var codFuncionarioSel = null;
var codTramiteTareaSel = null;
var multiTramites = false;
var tramitesTareasAsignadas = '';

SIMApp.controller("AvanzaTareaTramiteController", function ($scope, $location, $http) {
    $('.my-cloak').removeClass('my-cloak');

    multiTramites = ($('#app').data('multitramite') == '1');

    $scope.selectedTab = 0;
    $scope.itemSeleccionado = null;
    $scope.comentarios = '';
    $scope.TipoTarea = 'Flujo';

    $scope.tabsData = [
        { text: 'Tareas', pos: 0 },
        { text: 'Copias', pos: 1 },
        { text: 'Comentarios', pos: 2 },
    ];

    $scope.dxTabsOptions = {
        dataSource: $scope.tabsData,
        onItemClick: (function (itemData) {
            $scope.setTab(itemData.itemIndex);
            $(window).trigger('resize');
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

    $scope.txtComentarios = {
        height: '100%',
        width: '100%',
        maxLength: 1000,
        bindingOptions: { value: 'comentarios' }
    }

    $scope.btnAceptarAvanzarTareaSettings = {
        text: 'Avanzar Tarea',
        type: 'success',
        width: '20%',
        height: '35px',
        onClick: function (params) {
            var itemsCopias = copiasDataSource.items();
            var copiasSel = '';

            for (i = 0; i < itemsCopias.length ; i++) {
                if (copiasSel == '')
                    copiasSel += itemsCopias[i]['CODFUNCIONARIO'];
                else
                    copiasSel += ',' + itemsCopias[i]['CODFUNCIONARIO'];
            }

            if ($scope.TipoTarea != 'Final') {
                if (codFuncionarioSel == null && $('#app').data('restresp') == '1') {
                    MostrarNotificacion('notify', 'error', 'Responsable Requerido');
                    return;
                }
            }

            tramitesTareasAsignadas = '';

            if (multiTramites == 1) {
                var grid = $("#grdTramites").dxDataGrid("instance");

                for (i = 0; i < grid.totalCount(); i++) {
                    if (grid.cellValue(i, "CODTAREASIGUIENTE") == null) {
                        MostrarNotificacion('notify', 'error', 'Debe Seleccionar la Tarea Siguiente para Todos los Trámites.')
                        return;
                    }

                    if (tramitesTareasAsignadas == '') {
                        tramitesTareasAsignadas = grid.cellValue(i, "CODTRAMITE") + '|' + grid.cellValue(i, "CODTAREASIGUIENTE");
                    } else {
                        tramitesTareasAsignadas += ',' + grid.cellValue(i, "CODTRAMITE") + '|' + grid.cellValue(i, "CODTAREASIGUIENTE");
                    }
                }

                codTareaSel = 0;
            } else {
                if (codTareaSel == null) {
                    MostrarNotificacion('notify', 'error', 'Tarea Siguiente Requerida');
                    return;
                }
            }

            $http.post($('#app').data('url') + 'Tramites/api/TramitesApi/AvanzaTareaTramite',
                {
                    tipo: $('#app').data('tipo'),
                    codTramites: (multiTramites == 1 ? tramitesTareasAsignadas : $('#app').data('codtramites')),
                    codTarea: $('#app').data('codtarea'),
                    codTareaSiguiente: codTareaSel,
                    codFuncionario: codFuncionarioSel,
                    copias: copiasSel,
                    grupo: $('#app').data('codtramites'),
                    comentario: $scope.comentarios,
                }
            ).success(function (data, status, headers, config) {
                if (data.tipoRespuesta == 'OK') {
                    parent.CerrarPopup('AT', 'OK', $('#app').data('origen'), $('#app').data('codtramites'), $('#app').data('codtarea'), codTareaSel, codFuncionarioSel, copiasSel);
                } else {
                    parent.CerrarPopup('AT', 'ERROR', $('#app').data('origen'), '', '', '', '', '');
                }
            }).error(function (data, status, headers, config) {
                parent.CerrarPopup('AT', 'ERROR', $('#app').data('origen'), '', '', '', '', '');
            });
        }
    }

    $scope.btnCancelarAvanzarTareaSettings = {
        text: 'Cancelar',
        type: 'danger',
        width: '20%',
        height: '35px',
        onClick: function (params) {
            parent.CerrarPopup('AT', 'CANCEL', $('#app').data('origen'), '', '', '', '', '');
        }
    }

    $scope.popFuncionariosSettings = {
        showTitle: true,
        title: 'Seleccionar Funcionario',
        fullScreen: true,
        onShowing: function () {
            var gridInstance = $('#grdFuncionarios').dxDataGrid('instance');
            gridInstance.clearFilter();
            gridInstance.clearSelection();
        },
        onHidden: function () {
            if ($scope.itemSeleccionado != null) {
                var itemsCopias = copiasDataSource.items();

                for (i = 0; i < itemsCopias.length ; i++) {
                    if (itemsCopias[i]['CODFUNCIONARIO'] == $scope.itemSeleccionado.CODFUNCIONARIO) {
                        MostrarNotificacion('notify', 'error', 'El funcionario ya se encuentra seleccionado.')

                        return;
                    }
                }

                copiasDataSource.store().insert({ CODFUNCIONARIO: $scope.itemSeleccionado.CODFUNCIONARIO, NOMBRE: $scope.itemSeleccionado.NOMBRE });
                copiasDataSource.load();

                $('#grdCopias').dxDataGrid({
                    dataSource: copiasDataSource
                });
            }
        },
    }

    $scope.btnAdicionarFuncionarioSettings = {
        icon: "plus",
        onClick: function (e) {
            $scope.itemSeleccionado = null;
            var popup = $('#popFuncionarios').dxPopup('instance');
            popup.show();
        }
    };


    $scope.grdTramitesSettings = {
        dataSource: tramitesDataSource,
        height: '100%',
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 0,
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
                dataField: 'CODTRAMITE',
                width: '10%',
                caption: 'CODIGO',
                visible: true,
                dataType: 'number'
            },
            {
                dataField: 'ASUNTO',
                width: '80%',
                caption: 'ASUNTO',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'CODTAREA',
                visible: false,
                dataType: 'number'
            },
            {
                dataField: 'CODTAREASIGUIENTE',
                width: '10%',
                caption: 'TAREA',
                visible: true,
                dataType: 'number'
            },
        ],
        onSelectionChanged: function (selecteditems) {
            var data = selecteditems.selectedRowsData[0];

            codTramiteTareaSel = data.CODTAREA;

            $('#grdTareas').dxDataGrid({
                dataSource: tareasDataSource
            });
        }
    };

    $scope.grdTareasSettings = {
        dataSource: tareasDataSource,
        height: '100%',
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 0,
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
                dataField: 'CODTAREA',
                width: '15%',
                caption: 'CODIGO TAREA',
                visible: true,
                dataType: 'number'
            },
            {
                dataField: 'NOMBRE',
                width: '85%',
                caption: 'TAREA',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'TIPOTAREA',
                caption: 'TAREA',
                visible: false,
                dataType: 'string'
            }
        ],
        onSelectionChanged: function (selecteditems) {
            var data = selecteditems.selectedRowsData[0];

            codTareaSel = data.CODTAREA;
            codFuncionarioSel = null;
            if (data.TIPOTAREA == 'Final') $scope.TipoTarea = data.TIPOTAREA;
            $('#grdResponsables').dxDataGrid({
                dataSource: responsablesDataSource
            });
        },
        onRowClick: function (e) {
            if (multiTramites == 1) {
                var component = e.component;

                function initialClick() {
                    component.clickCount = 1;
                    component.clickKey = e.key;
                    component.clickDate = new Date();
                }

                function doubleClick() {
                    component.clickCount = 0;
                    component.clickKey = 0;
                    component.clickDate = null;

                    var grid = $("#grdTramites").dxDataGrid("instance");
                    var key = grid.getSelectedRowKeys()[0];
                    var index = grid.getRowIndexByKey(key);
                    grid.cellValue(index, "CODTAREASIGUIENTE", e.data.CODTAREA);
                    grid.saveEditData();
                }

                if ((!component.clickCount) || (component.clickCount != 1) || (component.clickKey != e.key)) {
                    initialClick();
                }
                else if (component.clickKey == e.key) {
                    if (((new Date()) - component.clickDate) <= 500)
                        doubleClick();
                    else
                        initialClick();
                }
            }
        }
    };

    $scope.grdResponsablesSettings = {
        dataSource: null,
        height: '100%',
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
                dataField: 'CODFUNCIONARIO',
                width: '20%',
                caption: 'FUNCIONARIO',
                visible: true,
                dataType: 'number',
                allowFiltering: false,
                allowHeaderFiltering: false,
            },
            {
                dataField: 'NOMBRE',
                width: '80%',
                caption: 'NOMBRE',
                visible: true,
                dataType: 'string',
                allowFiltering: true,
                allowHeaderFiltering: true,
            },
        ],
        onSelectionChanged: function (selecteditems) {
            var data = selecteditems.selectedRowsData[0];

            codFuncionarioSel = data.CODFUNCIONARIO;
        }
    };

    $scope.grdCopiasSettings = {
        dataSource: copiasDataSource,
        height: '100%',
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
        sorting: {
            mode: "none"
        },
        columns: [
            {
                dataField: 'CODFUNCIONARIO',
                width: '20%',
                caption: 'FUNCIONARIO',
                visible: true,
                dataType: 'number',
                allowFiltering: false,
            },
            {
                dataField: 'NOMBRE',
                width: '80%',
                caption: 'NOMBRE',
                visible: true,
                dataType: 'string',
                allowFiltering: true,
            },
        ],
        onRowClick: function (e) {
            var component = e.component;

            function initialClick() {
                component.clickCount = 1;
                component.clickKey = e.key;
                component.clickDate = new Date();
            }

            function doubleClick() {
                component.clickCount = 0;
                component.clickKey = 0;
                component.clickDate = null;

                var result = DevExpress.ui.dialog.confirm('Desea eliminar el funcionario de la selección ?', 'Confirmación');
                result.done(function (dialogResult) {
                    if (dialogResult) {
                        copiasDataSource.store().remove(e.key);
                        copiasDataSource.load();

                        var grid = $('#grdCopias').dxDataGrid('instance');
                        grid.refresh();
                    }
                });
            }

            if ((!component.clickCount) || (component.clickCount != 1) || (component.clickKey != e.key)) {
                initialClick();
            }
            else if (component.clickKey == e.key) {
                if (((new Date()) - component.clickDate) <= 500)
                    doubleClick();
                else
                    initialClick();
            }
        },
    };

    $scope.grdFuncionariosSettings = {
        dataSource: funcionariosDataSource,
        height: '100%',
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20, 50],
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
                dataField: 'CODFUNCIONARIO',
                width: '20%',
                caption: 'FUNCIONARIO',
                visible: true,
                dataType: 'number'
            },
            {
                dataField: 'NOMBRE',
                width: '80%',
                caption: 'NOMBRE',
                visible: true,
                dataType: 'string'
            },
        ],
        onSelectionChanged: function (selecteditems) {
            if (selecteditems.selectedRowsData.length > 0) {
                var data = selecteditems.selectedRowsData[0];
                $scope.itemSeleccionado = data;

                var popup = $('#popFuncionarios').dxPopup('instance');
                popup.hide();
            }
        }
    };
});

var copiasDataSource = new DevExpress.data.DataSource({ store: [], key: 'CODFUNCIONARIO' });

tramitesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        $.getJSON($('#app').data('url') + 'Tramites/api/TramitesApi/Tramites', {
            tramites: $('#app').data('codtramites')
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

tareasDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        $.getJSON($('#app').data('url') + 'Tramites/api/TramitesApi/TareasFlujoSiguiente', {
            codTarea: (codTramiteTareaSel ?? $('#app').data('codtarea')),
            tipo: $('#app').data('tipo'),
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

responsablesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'Tramites/api/TramitesApi/ResponsablesTarea', {
            filter: filterOptions,
            skip: skip,
            take: take,
            codTareaActual: $('#app').data('codtarea'),
            codTarea: codTareaSel,
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

funcionariosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"NOMBRE","desc":false}]';
        var groupOptions = "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'Seguridad/api/UsuarioApi/Funcionarios', {
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
        });
        return d.promise();
    },
    byKey: function (key, extra) {
        return key.toString();
    },
});

function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        DevExpress.ui.dialog.alert(msg, 'Mis Trámites');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}
