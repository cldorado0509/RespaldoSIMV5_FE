var SIMApp = angular.module('SIM', ['dx']);
var windowHeight = $(window).height();

var identificacion = null;
var prestadoPor = null;
var estadoPrestamo = 0;
var diasPrestado = null;
var fechaInicial = null;
var fechaFinal = null;
var texto = null;

SIMApp.controller("ConsultaPrestamosController", function ($scope, $location, $http) {
    $('.my-cloak').removeClass('my-cloak');

    $scope.consultandoVisible = false;
    $scope.busqueda = {};
    $scope.busqueda.Identificacion = null;
    $scope.busqueda.PrestadoPor = null;
    $scope.busqueda.EstadoPrestamo = 0;
    $scope.busqueda.DiasPrestado = null;
    $scope.busqueda.FechaInicial = null;
    $scope.busqueda.FechaFinal = null;
    $scope.busqueda.Texto = null;

    $scope.txtIdentificacionSettings = {
        bindingOptions: { value: 'busqueda.Identificacion' }
    };

    $scope.txtPrestadoPorSettings = {
        bindingOptions: { value: 'busqueda.PrestadoPor' }
    };

    $scope.cboEstadoPrestamoSettings = {
        dataSource: estadoPrestamoDataSource,
        placeholder: "[Estado Préstamo]",
        displayExpr: "NOMBRE",
        valueExpr: "ID",
        bindingOptions: { value: 'busqueda.EstadoPrestamo' }
    };

    $scope.txtDiasPrestadoSettings = {
        width: '80px',
        bindingOptions: { value: 'busqueda.DiasPrestado' }
    };

    $scope.datFechaInicialSettings = {
        displayFormat: 'yyyy/MM/dd',
        //showClearButton: true,
        useCalendar: true,
        width:'100%',
        bindingOptions: { value: 'busqueda.FechaInicial' }
    };

    $scope.datFechaFinalSettings = {
        displayFormat: 'yyyy/MM/dd',
        //showClearButton: true,
        useCalendar: true,
        width: '100%',
        bindingOptions: { value: 'busqueda.FechaFinal' }
    };

    $scope.txtTextoBusquedaSettings = {
        bindingOptions: { value: 'busqueda.Texto' }
    };

    $scope.btnConsultarSettings = {
        text: 'Consultar',
        type: 'success',
        width: '100%',
        onClick: function (params) {
            identificacion = $scope.busqueda.Identificacion;
            prestadoPor = $scope.busqueda.PrestadoPor;
            estadoPrestamo = $scope.busqueda.EstadoPrestamo;
            diasPrestado = $scope.busqueda.DiasPrestado;
            fechaInicial = $('#datFechaInicial').dxDateBox('instance').option('text');
            fechaFinal = $('#datFechaFinal').dxDateBox('instance').option('text');
            texto = $scope.busqueda.Texto;

            if (ValidarParametros()) {
                $('#grdResultadoConsulta').dxDataGrid({
                    dataSource: consultaPrestamoDataSource
                });
            }
        }
    }

    function ValidarParametros() {
        if ($scope.busqueda.FechaInicial != null && $scope.busqueda.FechaFinal != null && $scope.busqueda.FechaInicial > $scope.busqueda.FechaFinal) {
            $scope.cargandoVisible = false;
            MostrarNotificacion('alert', 'error', 'Fecha Inicial NO puede ser mayor a la Fecha Final');
            return false;
        }

        return true;
    }

    $scope.grdResultadoConsultaSettings = {
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true
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
            insertEnabled: false,
        },
        selection: {
            mode: 'single'
        },
        "export": {
            enabled: true,
            fileName: "Prestamos",
            allowExportSelectedData: false
        },
        columns: [
            {
                //dataField: 'S_UNIDADDOCUMENTAL',
                dataField: 'S_TIPOEXPEDIENTE',
                //caption: 'UNIDAD DOCUMENTAL',
                caption: 'TIPO EXPEDIENTE',
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
                width: '25%'
            }, {
                dataField: 'S_ESTADO',
                caption: 'ESTADO',
                dataType: 'string',
                width: '8%'
            }, {
                dataField: 'S_PRESTADOPOR',
                caption: 'PRESTADO POR',
                dataType: 'string',
                width: '14%'
            }, {
                dataField: 'D_PRESTAMO',
                caption: 'FECHA PRESTAMO',
                alignment: 'right',
                dataType: 'date',
                format: 'yyyy/MM/dd',
                width: '10%'
            }, {
                dataField: 'D_DEVOLUCION',
                caption: 'FECHA DEVOLUCION',
                alignment: 'right',
                dataType: 'date',
                format: 'yyyy/MM/dd',
                width: '10%'
            }, {
                dataField: 'N_DIAS',
                caption: 'DIAS',
                dataType: 'number',
                width: '5%'
            }
        ],
    }
});

var estadoPrestamoDataSource = [
    { ID: 0, NOMBRE: '(Todos)' },
    { ID: 1, NOMBRE: 'Prestado' },
    { ID: 2, NOMBRE: 'Devuelto' },
];

consultaPrestamoDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"D_PRESTAMO","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);

        $.getJSON($('#app').data('url') + 'GestionDocumental/api/PrestamoApi/ConsultaPrestamo', {
            filter: filterOptions,
            sort: sortOptions,
            group: groupOptions,
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            noFilterNoRecords: false,
            identificacion: identificacion,
            prestadoPor: prestadoPor,
            estadoPrestamo: estadoPrestamo,
            diasPrestado: diasPrestado,
            fechaInicial: fechaInicial,
            fechaFinal: fechaFinal,
            texto: texto,
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
        DevExpress.ui.dialog.alert(msg, 'Programación de Actuaciones');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}
