var SIMApp = angular.module('SIM', ['dx']);

SIMApp.controller("MisTramitesController", function ($scope, $location, $http) {
    $('.my-cloak').removeClass('my-cloak');

    $scope.misTramitesGridSettings = {
        dataSource: misTramitesDataSource,
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
    };
});

misTramitesDataSource = new DevExpress.data.CustomStore({
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

function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        DevExpress.ui.dialog.alert(msg, 'Mis Trámites');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}
