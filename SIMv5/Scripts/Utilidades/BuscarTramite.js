var SelTipo = -1;
var txtTramite = "";
var txtVital = "";
var txtFiltro = "";
var codFuncionario = -1;
var TipoProceso = -1;

$(document).ready(function () {
    $("#rdbTipoBusca").dxRadioGroup({
        dataSource: [{ value: 0, text: "Por trámite" }, { value: 1, text: "Por índices" }, { value: 2, text: "Por funcionario" }],
        displayExpr: "text",
        valueExpr: "value",
        value: 0,
        layout: "horizontal",
        onContentReady: function (e) {
            $("#PanelTramite").show();
            $("#PanelIndices").hide();
            $("#PanelFuncionario").hide();
            SelTipo = 0;
        },
        onValueChanged: function (e) {
            switch (e.value) {
                case 0:
                    $("#PanelTramite").show();
                    $("#PanelIndices").hide();
                    $("#PanelFuncionario").hide();
                    break;
                case 1:
                    $("#PanelTramite").hide();
                    $("#PanelIndices").show();
                    $("#PanelFuncionario").hide();
                    break;
                case 2:
                    $("#PanelTramite").hide();
                    $("#PanelIndices").hide();
                    $("#PanelFuncionario").show();
                    break;
            }
            SelTipo = e.value;
            txtTramite = "";
            txtVital = "";
            txtFiltro = "";
            txtFulltext = "";
            codFuncionario = -1;
            TipoProceso = -1;
            $("#gridTramitesEnc").dxDataGrid("instance").refresh();
        }
    });

    $("#txtTramite").dxTextBox({
        placeholder: "Ingrese el código del trámite",
        value: ""
    });

    $("#txtVital").dxTextBox({
        placeholder: "Ingrese el número VITAL",
        value: ""
    });

    $("#cmbTipoProceso").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "CODPROCESO",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Utilidades/GetListaProcesos");
                }
            })
        }),
        displayExpr: "NOMBRE",
        valueExpr: "CODPROCESO",
        searchEnabled: true,
        noDataText: "No hay datos para mostrar",
        placeholder: "Seleccione",
        onValueChanged: function (data) {
            TipoProceso = data.value;
            $.getJSON($('#SIM').data('url') + 'Utilidades/GetIndicesProc?CodProceso=' + TipoProceso
            ).done(function (data) {
                var Filtro = $("#FilterBuscar").dxFilterBuilder(OpcionesFiltro).dxFilterBuilder("instance");
                Filtro.option("fields", data);
            }).fail(function (jqxhr, textStatus, error) {
                alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
            });
        }
    });

    $("#cmbFuncionario").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "CODFUNCIONARIO",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Utilidades/GetListaFuncionarios");
                }
            })
        }),
        displayExpr: "NOMBRE",
        valueExpr: "CODFUNCIONARIO",
        searchEnabled: true,
        noDataText: "No hay datos para mostrar",
        placeholder: "Seleccione",
        onValueChanged: function (data) {
            codFuncionario = data.value;
        }
    });

    var OpcionesFiltro = {
        fields: [],
        value: [],
        filterOperationDescriptions: {
            between: "Entre",
            contains: "Contiene",
            endsWith: "Finaliza en",
            equal: "Igual",
            greaterThan: "Mayor que",
            greaterThanOrEqual: "Mayor o igual a",
            isBlank: "Es blanco",
            isNotBlank: "No es blanco",
            lessThan: "Menor que",
            lessThanOrEqual: "Menor o igual a",
            notContains: "No contiene",
            notEqual: "Diferente",
            startsWith: "Inicia con"
        },
        groupOperationDescriptions: {
            and: "Y (and)",
            or: "O (or)",
            notOr: "",
            notAnd: ""
        }
    };

    $("#btnBuscar").dxButton({
        text: "Buscar",
        type: "default",
        onClick: function () {
            if (SelTipo == 0) {
                var _tramite = $("#txtTramite").dxTextBox("instance").option("value");
                var _vital = $("#txtVital").dxTextBox("instance").option("value");
                if (_tramite != "" || _vital != "") {
                    txtFiltro = "";
                    codFuncionario = -1;
                    txtTramite = _tramite;
                    txtVital = _vital;
                } else {
                    txtTramite = "";
                    txtVital = "";
                    DevExpress.ui.dialog.alert('No se ha ingresado un código de trámite o número VITAL para buscar', 'Buscar documentos');
                }
            } else if (SelTipo == 1) {
                var _Filto = formatValue($("#FilterBuscar").dxFilterBuilder("instance").option("value"));
                if (_Filto != "") {
                    txtTramite = "";
                    txtVital = "";
                    codFuncionario = -1;
                    txtFiltro = _Filto;
                } else {
                    txtFiltro = "";
                    DevExpress.ui.dialog.alert('El filtro no se ha establecido o esta mal', 'Buscar documentos');
                }
            } else if (SelTipo == 2) {
                var _funcionario = $("#cmbFuncionario").dxSelectBox("instance").option("value");
                if (_funcionario != "") {
                    txtTramite = "";
                    txtFiltro = "";
                    codFuncionario = _funcionario;
                } else {
                    codFuncionario = -1;
                    DevExpress.ui.dialog.alert('No se ha ingresado un dato para buscar', 'Buscar documentos');
                }
            }
            $("#gridTramitesEnc").dxDataGrid("instance").refresh();
        }
    });

    $("#gridTramitesEnc").dxDataGrid({
        dataSource: TramitesDataSource,
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
        selection: {
            mode: 'single'
        },
        'export': {
            enabled: true,
            fileName: 'Tramites'
        },
        hoverStateEnabled: true,
        remoteOperations: true,
        columns: [
            { dataField: 'CODTRAMITE', width: '10%', caption: 'Trámite', dataType: 'number' },
            { dataField: 'TIPOPROCESO', width: '50%', caption: 'Tipo Proceso', dataType: 'string' },
            { dataField: 'FECHAINI', width: '20%', caption: 'Fecha Inicia trámite', dataType: 'date', format: 'MMM dd yyyy' },
            { dataField: 'ESTADO', width: '12%', caption: 'Estado del trámite', dataType: 'string' },
            {
                with: '8%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'export',
                        hint: 'Seleccionar trámite',
                        onClick: function (e) {
                            parent.SelTramite(options.data.CODTRAMITE);
                        }
                    }).appendTo(container);
                }
            }
        ]
    });

    function formatValue(value, spaces) {
        if (value && Array.isArray(value[0])) {
            var TAB_SIZE = 4;
            spaces = spaces || TAB_SIZE;
            return "[" + getLineBreak(spaces) + value.map(function (item) {
                return Array.isArray(item[0]) ? formatValue(item, spaces + TAB_SIZE) : JSON.stringify(item);
            }).join("," + getLineBreak(spaces)) + getLineBreak(spaces - TAB_SIZE) + "]";
        }
        return JSON.stringify(value);
    }

});

var TramitesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'api/UtilidadesApi/BuscarTramites', {
            skip: skip,
            take: take,
            IdTipoProceso: TipoProceso,
            Buscar: txtTramite.length > 0 || txtVital.length > 0 ? 'T;' + txtTramite + ' ' + txtVital : txtFiltro.length > 0 ? 'F;' + txtFiltro : codFuncionario > 0 ? 'U;' + codFuncionario : ''
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});