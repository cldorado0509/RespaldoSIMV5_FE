var myApp = angular.module('SIM', ['dx']);
myApp.controller("FacturPendController", function ($scope, $location, $http) {

    $scope.tabpanelContainer = {
        dataSource: [{
            "ID": 1,
            "title": "Informes Técnicos",
            "template": "tab1"
        }, {
            "ID": 2,
            "title": "Resoluciones",
            "template": "tab2"
            }],

    }

    $scope.grdInfPendSetting = {
        dataSource: $scope.grdInfDataSource,
        allowColumnResizing: true,
        caption: 'Informes Ténicos pendientes de facturación',
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20, 50]
        },
        filterRow: {
            visible: true,
            emptyPanelText: 'Arrastre una columna para agrupar'
        },
        editing: {
            mode: "cell",
            allowUpdating: true
        },
        'export': {
            enabled: true,
            fileName: 'Informes_Facturas'
        },
        columns: [
            {
                dataField: "CODTRAMITE",
                width: '10%',
                caption: 'Codigo del Trámite',
                dataType: 'number',
            }, {
                dataField: 'CODDOCUMENTO',
                width: '5%',
                caption: 'Documento',
                dataType: 'number',
            }, {
                dataField: 'RADICADO',
                width: '10%',
                caption: 'Radicado',
                dataType: 'string',
            }, {
                dataField: 'FECHA RADICADO',
                width: '10%',
                caption: 'Fecha del Radicado',
                alignment: 'right',
                dataType: 'date'
            }, {
                dataField: 'ASUNTO',
                width: '15%',
                caption: 'Asunto del documento',
                alignment: 'right',
                dataType: 'string',
            }, {
                dataField: 'FACTURA ASIGNADA',
                width: '15%',
                caption: 'Factura asignada',
                dataType: 'string',
            }]
    }
});

var grdInfDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"RADICADO","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON('@Url.Content("~")Facturacion/api/FacturApi/InformesPend', {
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
    }
});
