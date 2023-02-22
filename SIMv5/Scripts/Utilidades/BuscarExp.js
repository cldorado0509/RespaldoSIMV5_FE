var SelTipo = -1;
var txtCodigo = "";
var txtFiltro = "";
var txtFulltext = "";
var UnidadDoc = -1;

$(document).ready(function () {
    $("#rdbTipoBusca").dxRadioGroup({
        dataSource: [{ value: 0, text: "Por código" }, { value: 1, text: "Por índices" }, { value: 2, text: "Full Text" }],
        displayExpr: "text",
        valueExpr: "value", 
        value: 0,
        layout: "horizontal",
        onContentReady: function (e) {
            $("#PanelCodigo").show();
            $("#PanelIndices").hide();
            $("#PanelFulltext").hide();
            SelTipo = 0;
        },
        onValueChanged: function (e) {
            switch (e.value) {
                case 0:
                    $("#PanelCodigo").show();
                    $("#PanelIndices").hide();
                    $("#PanelFulltext").hide();
                    break;
                case 1:
                    $("#PanelCodigo").hide();
                    $("#PanelIndices").show();
                    $("#PanelFulltext").hide();
                    break;
                case 2:
                    $("#PanelCodigo").hide();
                    $("#PanelIndices").hide();
                    $("#PanelFulltext").show();
                    break;
            }
            SelTipo = e.value;
            txtCodigo = "";
            txtFiltro = "";
            txtFulltext = "";
            UnidadDoc = -1;
            $("#gridExpsEnc").dxDataGrid("instance").refresh();
        }
    });

    $("#txtCodigo").dxTextBox({
        placeholder: "Ingrese el código del expediente",
        value: ""
    });

    $("#txtDato").dxTextBox({
        placeholder: "Ingrese el dato a buscar",
        value: ""
    });
    

    $("#cmbUnidadDoc").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "CODSERIE",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Utilidades/GetListaUnidadesExp");
                }
            })
        }),
        displayExpr: "NOMBRE",
        valueExpr: "CODSERIE",
        searchEnabled: true,
        noDataText: "No hay datos para mostrar",
        placeholder: "Seleccione",
        onValueChanged: function (data) {
            UnidadDoc = data.value;
            $.getJSON($('#SIM').data('url') + 'Utilidades/GetFields?UniDoc=' + UnidadDoc
            ).done(function (data) {
                var Filtro = $("#FilterBuscar").dxFilterBuilder(OpcionesFiltro).dxFilterBuilder("instance");
                Filtro.option("fields", data);
            }).fail(function (jqxhr, textStatus, error) {
                alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
                });
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
                var _codigo = $("#txtCodigo").dxTextBox("instance").option("value");
                if (_codigo != "") {
                    txtFiltro = "";
                    txtFulltext = "";
                    txtCodigo = _codigo;
                } else {
                    txtCodigo = "";
                    DevExpress.ui.dialog.alert('No se ha ingresado un código de expediente para buscar', 'Buscar expedientes');
                }
            } else if (SelTipo == 1) {
                var _Filto = formatValue($("#FilterBuscar").dxFilterBuilder("instance").option("value"));
                if (_Filto != "") {
                    txtCodigo = "";
                    txtFulltext = "";
                    txtFiltro = _Filto;
                } else {
                    txtFiltro = "";
                    DevExpress.ui.dialog.alert('El filtro no se ha establecido o esta mal', 'Buscar expedientes');
                }
            } else if (SelTipo == 2) {
                var _Dato = $("#txtDato").dxTextBox("instance").option("value");
                if (_Dato != "") {
                    txtCodigo = "";
                    txtFiltro = "";
                    txtFulltext = _Dato;
                } else {
                    txtFulltext = "";
                    DevExpress.ui.dialog.alert('No se ha ingresado un dato para buscar', 'Buscar expedientes');
                }
            }
            $("#gridExpsEnc").dxDataGrid("instance").refresh();
        }
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

    function getLineBreak(spaces) {
        return "\r\n" + new Array(spaces + 1).join(" ");
    }

    $("#gridExpsEnc").dxDataGrid({
        dataSource: ExpedientesDataSource,
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
        hoverStateEnabled: true,
        remoteOperations: true,
        columns: [
            { dataField: "ID_EXPEDIENTE", visible: false },
            { dataField: "EXPEDIENTE", width: '5%', caption: 'Expediente', dataType: 'string' },
            { dataField: "CODIGO", width: '15%', caption: 'Código', dataType: 'string' },
            { dataField: "NOMBRE", width: '20%', caption: 'Unidad Documental', dataType: 'string' },
            { dataField: 'INDICES', width: '30%', caption: 'Indices', dataType: 'string'},
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'export',
                        hint: 'Seleccionar expediente',
                        onClick: function (e) {
                            parent.SeleccionaExp(options.data.ID_EXPEDIENTE);
                        }
                    }).appendTo(container);
                }
            }
        ]
    });

});

var ExpedientesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'api/UtilidadesApi/BuscarExp', {
            skip: skip,
            take: take,
            IdUnidadDoc: UnidadDoc,
            Buscar: txtCodigo.length > 0 ? 'C;' + txtCodigo : txtFiltro.length > 0 ? 'F;' + txtFiltro : txtFulltext.length > 0 ? 'B;' + txtFulltext : ''
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});


