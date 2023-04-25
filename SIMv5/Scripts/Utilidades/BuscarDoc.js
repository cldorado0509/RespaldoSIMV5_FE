var SelTipo = -1;
var txtTramite = "";
var txtFiltro = "";
var txtFulltext = "";
var UnidadDoc = -1;

$(document).ready(function () {
    $("#rdbTipoBusca").dxRadioGroup({
        dataSource: [{ value: 0, text: "Por trámite" }, { value: 1, text: "Por índices" }, { value: 2, text: "Full Text" }],
        displayExpr: "text",
        valueExpr: "value", 
        value: 0,
        layout: "horizontal",
        onContentReady: function (e) {
            $("#PanelTramite").show();
            $("#PanelIndices").hide();
            $("#PanelFulltext").hide();
            SelTipo = 0;
        },
        onValueChanged: function (e) {
            switch (e.value) {
                case 0:
                    $("#PanelTramite").show();
                    $("#PanelIndices").hide();
                    $("#PanelFulltext").hide();
                    break;
                case 1:
                    $("#PanelTramite").hide();
                    $("#PanelIndices").show();
                    $("#PanelFulltext").hide();
                    break;
                case 2:
                    $("#PanelTramite").hide();
                    $("#PanelIndices").hide();
                    $("#PanelFulltext").show();
                    break;
            }
            SelTipo = e.value;
            txtTramite = "";
            txtFiltro = "";
            txtFulltext = "";
            UnidadDoc = -1;
            $("#gridDocsEnc").dxDataGrid("instance").refresh();
        }
    });

    $("#txtTramite").dxTextBox({
        placeholder: "Ingrese el código del trámite",
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
                    return $.getJSON($("#SIM").data("url") + "Utilidades/GetListaUnidades");
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
                var _tramite = $("#txtTramite").dxTextBox("instance").option("value");
                if (_tramite != "") {
                    txtFiltro = "";
                    txtFulltext = "";
                    txtTramite = _tramite;
                } else {
                    txtTramite = "";
                    DevExpress.ui.dialog.alert('No se ha ingresado un código de trámite para buscar', 'Buscar documentos');
                }
            } else if (SelTipo == 1) {
                var _Filto = formatValue($("#FilterBuscar").dxFilterBuilder("instance").option("value"));
                if (_Filto != "") {
                    txtTramite = "";
                    txtFulltext = "";
                    txtFiltro = _Filto;
                } else {
                    txtFiltro = "";
                    DevExpress.ui.dialog.alert('El filtro no se ha establecido o esta mal', 'Buscar documentos');
                }
            } else if (SelTipo == 2) {
                var _Dato = $("#txtDato").dxTextBox("instance").option("value");
                if (_Dato != "") {
                    txtTramite = "";
                    txtFiltro = "";
                    txtFulltext = _Dato;
                } else {
                    txtFulltext = "";
                    DevExpress.ui.dialog.alert('No se ha ingresado un dato para buscar', 'Buscar documentos');
                }
            }
            $("#gridDocsEnc").dxDataGrid("instance").refresh();
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

    $("#gridDocsEnc").dxDataGrid({
        dataSource: DocumentosDataSource,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            pageSize: 7,
            enabled: true
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [7, 10, 20, 50]
        },
        selection: {
            mode: 'single'
        },
        hoverStateEnabled: true,
        remoteOperations: true,
        columns: [
            { dataField: "ID_DOCUMENTO", visible: false},
            { dataField: "CODTRAMITE", width: '10%', caption: 'Trámite', dataType: 'number' },
            { dataField: "CODDOCUMENTO", width: '5%', caption: 'Documento', dataType: 'number' },
            { dataField: 'FECHACREACION', width: '10%', caption: 'Fecha Digitaliza', dataType: 'date', format: 'MMM dd yyyy' },
            { dataField: "NOMBRE", width: '20%', caption: 'Unidad Documental', dataType: 'string' },
            { dataField: 'INDICES', width: '25%', caption: 'Indices', dataType: 'string' },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'doc',
                        hint: 'Ver el documento',
                        onClick: function (e) {
                            window.open($('#SIM').data('url') + "Utilidades/LeeDoc?IdDocumento=" + options.data.ID_DOCUMENTO, "Documento " + options.data.ID_DOCUMENTO , "width= 900,height=800,scrollbars = yes, location = no, toolbar = no, menubar = no, status = no");
                        }
                    }).appendTo(container);
                }
            },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'export',
                        hint: 'Seleccionar documento',
                        onClick: function (e) {
                            parent.SeleccionaDocumento(options.data.ID_DOCUMENTO);
                        }
                    }).appendTo(container);
                }
            }
        ]
    });

});

var DocumentosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'api/UtilidadesApi/BuscarDoc', {
            skip: skip,
            take: take,
            IdUnidadDoc: UnidadDoc,
            Buscar: txtTramite.length > 0 ? 'T;' + txtTramite : txtFiltro.length > 0 ? 'F;' + txtFiltro : txtFulltext.length > 0 ? 'B;' + txtFulltext : ''
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});


