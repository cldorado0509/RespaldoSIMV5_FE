var SIMApp = angular.module('SIM', ['dx']);

SIMApp.controller("PMESDashboardController", function ($scope, $location, $http) {
    $('.my-cloak').removeClass('my-cloak');

    $scope.dashboard = {};
    $scope.dashboard.instalacion = -1;
    $scope.dashboard.vigencia = null;
    $scope.dashboard.encuesta = -1;
    $scope.selectedTab = 0;
    $scope.cargandoVisible = false;
    $scope.mapaCargado = false;
    $scope.itemSeleccionadoTercero = false;
    $scope.idTercero = $('#app').data('tercero');
    $scope.nombreTercero = '';

    $scope.tabsData = [
        { text: 'Visualización Anual', pos: 0 },
        { text: 'Geolocalización', pos: 1 },
        { text: 'Seguimiento', pos: 2 },
    ];

    $scope.dxTabsOptions = {
        dataSource: $scope.tabsData,
        onItemClick: (function (itemData) {
            $scope.selectedTab = itemData.itemIndex;
            setTimeout(AjustarTamano, 100);

            if (itemData.itemIndex == 1 && !$scope.mapaCargado) {
                if ($scope.dashboard.vigencia != null) {
                    $scope.mapaCargado = true;
                    setTimeout($scope.CargarMapa, 100);
                }
            }
        }),
        selectedIndex: 0
    };

    $scope.CargarMapa = function() {
        $('#frmMapa').attr('src', $('#app').data('url') + 'Dashboard/PMES/PMESMapaContainer?v=' + $scope.dashboard.vigencia + '&e=' + $scope.dashboard.encuesta + '&t=' + $('#app').data('tercero'));
    };

    $scope.isActive = function (tabIndex) {
        if ($scope.selectedTab === tabIndex)
            return true;
        else
            return false;
    };

    $scope.isActiveEncuesta = function (idEncuesta) {
        return $scope.dashboard.encuesta == idEncuesta;
    };

    $scope.VigenciasSettings = {
        dataSource: vigenciaDataSource,
        placeholder: "[Vigencia]",
        displayExpr: "VIGENCIA",
        valueExpr: "VIGENCIA",
        searchEnabled: false,
        searchExpr: "VIGENCIA",
        searchTimeout: 0,
        bindingOptions: { value: 'dashboard.vigencia' },
        onSelectionChanged: function (e) {
            $scope.dashboard.encuesta = e.selectedItem.ID_ENCUESTA;
            $scope.dashboard.vigencia = e.selectedItem.VIGENCIA;
            setTimeout(AjustarTamano, 100);
            $('#frmDashboard_' + $scope.dashboard.encuesta).attr('src', $('#app').data('url') + 'Dashboard/PMES/PMESDashboard?v=' + e.selectedItem.VIGENCIA + '&e=' + e.selectedItem.ID_ENCUESTA + '&t=' + $('#app').data('tercero'));
            $('#frmMapa').attr('src', $('#app').data('url') + 'Dashboard/PMES/PMESMapaContainer?v=' + e.selectedItem.VIGENCIA + '&e=' + e.selectedItem.ID_ENCUESTA + '&t=' + $('#app').data('tercero'));
            $('#frmDashboard_100').attr('src', $('#app').data('url') + 'Dashboard/PMES/PMESDashboardS?v=' + e.selectedItem.VIGENCIA + '&e=' + e.selectedItem.ID_ENCUESTA + '&t=' + $('#app').data('tercero'));
        }
    };

    $scope.popTerceroSettings = {
        showTitle: true,
        deferRendering: false,
        title: 'Seleccionar Tercero',
        onHidden: function () {
            if ($scope.itemSeleccionadoTercero) {
                $scope.terceroSeleccionado = false;

                cboTercerosDataSource.store().clear();
                cboTercerosDataSource.store().insert({ ID_TERCERO: $scope.idTercero, S_RSOCIAL: $scope.nombreTercero });
                cboTercerosDataSource.load();

                window.location = $('#app').data('url') + 'Dashboard/PMES/PMES?t=' + $scope.idTercero;
            }
        },
    }

    cboTercerosDataSource.store().clear();
    cboTercerosDataSource.store().insert({ ID_TERCERO: $('#app').data('tercero'), S_RSOCIAL: $('#app').data('nombretercero') });
    cboTercerosDataSource.load();

    $scope.terceroSelectBoxSettings = {
        dataSource: cboTercerosDataSource,
        valueExpr: 'ID_TERCERO',
        displayExpr: 'S_RSOCIAL',
        placeholder: '[Seleccionar Tercero]',
        bindingOptions: { value: 'idTercero' },
        onOpened: function () {
            $('#cboTercero').dxSelectBox('instance').close();
            var popup = $('#popTercero').dxPopup('instance');
            popup.show();
        }
    };

    $scope.terceroGridSettings = {
        dataSource: terceroDataSource,
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
                dataField: 'ID_TERCERO',
                caption: 'ID_TERCERO',
                visible: false,
                dataType: 'number'
            },
            {
                dataField: 'N_DOCUMENTON',
                width: '25%',
                caption: 'DOCUMENTO',
                visible: true,
                dataType: 'number'
            },
            {
                dataField: 'S_RSOCIAL',
                width: '75%',
                caption: 'RAZON SOCIAL',
                visible: true,
                dataType: 'string'
            }
        ],
        onSelectionChanged: function (selecteditems) {
            var data = selecteditems.selectedRowsData[0];
            $scope.itemSeleccionadoTercero = true;
            $scope.idTercero = data.ID_TERCERO;
            $scope.nombreTercero = data.S_RSOCIAL;

            var popup = $('#popTercero').dxPopup('instance');
            popup.hide();
        }
    };

    $.get($('#app').data('url') + 'Dashboard/api/PMESApi/Vigencias?t=' + $('#app').data('tercero'), function (data) {
        for (var i = 0; i < data.datos.length; i++) {
            vigenciaDataSource.store().insert(data.datos[i]);
        }
        vigenciaDataSource.load();
        $scope.$apply();
    }, "json");
});

var vigenciaDataSource = new DevExpress.data.DataSource([]);
var cboTercerosDataSource = new DevExpress.data.DataSource([]);

terceroDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"S_RSOCIAL","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'General/api/TerceroApi/Terceros', {
            filter: filterOptions,
            sort: sortOptions,
            group: groupOptions,
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
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


function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        DevExpress.ui.dialog.alert(msg, 'PMES');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}
