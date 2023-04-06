$(document).ready(function () {
    $("#grdListaSoprtesPago").dxDataGrid({
        dataSource: SoportesPagoDataSource,
        allowColumnResizing: true,
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
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Buscar..."
        },
        selection: {
            mode: 'single'
        },
        hoverStateEnabled: true,
        remoteOperations: true,
        columns: [
            { dataField: 'ID_CALCULO', width: '5%', caption: 'Identificador', alignment: 'center' },
            { dataField: 'TIPO', width: '25%', caption: 'Tipo de Trámite', dataType: 'string' },
            { dataField: 'NIT', width: '10%', caption: 'Documento tercero', dataType: 'string' },
            { dataField: 'TERCERO', width: '35%', caption: 'Tercero', dataType: 'string' },
            { dataField: 'FECHA', width: '12%', caption: 'Fecha', dataType: 'date', format: 'MMM dd yyyy HH:mm' },
            { dataField: 'CONSECUTIVO', width: '8%', caption: 'Soporte Pago', dataType: 'string' },
            {
                width: '5%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'fields',
                        hint: 'Ver parámetros del cálculo',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "AtencionUsuarios/api/ValorTramiteApi/ParametrosCalculo";
                            $.getJSON(_Ruta, { IdCalculo: options.data.ID_CALCULO })
                                .done(function (data) {
                                    if (data != null) {
                                        if (data.IdCalculo == options.data.ID_CALCULO) {
                                            showParametros(data);
                                        } else {
                                            DevExpress.ui.dialog.alert('Ocurrió un error No se encontraron datos para el cálculo del valor del trámite', 'Parámetros Cálculo');
                                        }
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Parámetros Cálculo');
                                });
                        }
                    }).appendTo(container);
                }
            },
            {
                width: '5%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'doc',
                        hint: 'Ver soporte de pago',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "AtencionUsuarios/api/ValorTramiteApi/ExisteSoporte";
                            $.getJSON(_Ruta, { IdCalculo: options.data.ID_CALCULO })
                                .done(function (data) {
                                    if (data != null) {
                                        if (data.IdCalculo == options.data.ID_CALCULO) {
                                            showParametros(data);
                                        } else {
                                            DevExpress.ui.dialog.alert('Ocurrió un error No se encontraron datos para el cálculo del valor del trámite', 'Parámetros Cálculo');
                                        }
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Parámetros Cálculo');
                                });
                        }
                    }).appendTo(container);
                }
            }
        ]
    });

    var popupParam = null;

    var showParametros = function (data) {
        Parametro = data;
        if (popupParam) {
            popupParam.option("contentTemplate", popupOptions.contentTemplate.bind(this));
        } else {
            popupParam = $("#PopupParametros").dxPopup(popupOptions).dxPopup("instance");
        }
        popupParam.show();
    };

    var popupOptions = {
        width: 700,
        height: "auto",
        hoverStateEnabled: true,
        title: "Parámetros del cálculo del valor del trámite",
        visible: false,
        closeOnOutsideClick: true,
        contentTemplate: function () {
            const formatter = new Intl.NumberFormat('sp-CO', {
                style: 'currency', currency: 'COP',
                minimumFractionDigits: 0 });
            return $("<div />").append(
                $("<div class='row col-md-12'><p style='text-align: center'><b>CÁLCULO DEL VALOR DE LA EVALUACIÓN</b></p></div><br />"),
                $("<div class='row'><div class='col-md-8'><p><b>ITEM</b></p></div><div class='col-md-4'><p><b>VALOR</b></p></div></div>"),
                $("<div class='row'><div class='col-md-8'><p>GASTOS POR SUELDOS Y HONORARIOS (A):</p></div><div class='col-md-4'><p>" + formatter.format(Parametro.Sueldos) + "</p></div>"),
                $("<div class='row'><div class='col-md-8'><p>GASTOS DE VIAJE (B):</p></div><div class='col-md-4'><p>" + formatter.format(Parametro.Viajes) + "</p></div>"),
                $("<div class='row'><div class='col-md-8'><p>GASTOS ANÁLISIS DE LABORATORIO Y OTROS TRABAJOS TÉCNICOS (C):</p></div><div class='col-md-4'><p>" + formatter.format(Parametro.Otros) + "</p></div>"),
                $("<div class='row'><div class='col-md-8'><p>GASTOS DE ADMINISTRACIÓN 25% (D):</p></div><div class='col-md-4'><p>" + formatter.format(Parametro.Admin) + "</p></div><br />"),
                $("<div class='row'><div class='col-md-8'><b><p>COSTO TOTAL DE LA TARIFA :</p></div><div class='col-md-4'><p>" + formatter.format(Parametro.Costo) + "</p></b></div>"),
                $("<div class='row'><div class='col-md-8'><p>DETERMINACIÓN DE LOS TOPES DE LAS TARIFAS (To):</p></div><div class='col-md-4'><p>" + formatter.format(Parametro.Topes) + "</p></div>"),
                $("<div class='row'><div class='col-md-8'><p><b>VALOR A CANCELAR POR TRÁMITE:</p></div><div class='col-md-4'><p>" + formatter.format(Parametro.Valor) + "</p></b></div>"),
                $("<div class='row'><div class='col-md-8'><p><b>VALOR A CANCELAR POR PUBLICACIÓN:</p></div><div class='col-md-4'><p>" + formatter.format(Parametro.Publicacion) + "</p></b></div>")
            );
        }
    };
});

var SoportesPagoDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"ID_CALCULO","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'AtencionUsuarios/api/ValorTramiteApi/ObtienSoportes', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: false,
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});
