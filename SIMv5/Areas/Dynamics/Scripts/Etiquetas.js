var filtros = "";

$(document).ready(function () {
    var Codigo = $("#txtCodigo").dxTextBox({
        placeholder: "Ingrese el código del bien",
        value: ""
    }).dxTextBox("instance");

    var Responsable = $("#cboResponsable").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "ORDEN",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#Etiquetas").data("url") + "Dynamics/api/EtiquetaApi/Responsables");
                }
            })
        }),
        displayExpr: "RESPONSABLE",
        valueExpr: "ORDEN",
        searchEnabled: true,
        noDataText: "No hay datos para mostrar",
        placeholder: "Seleccione",
        showClearButton: true
    }).dxSelectBox("instance");

    $("#btnInforme").dxButton({
        icon: "filter",
        text: 'Buscar',
        onClick: function () {
            if (Responsable.option("value") >= 1) {
                filtros = "R:" + Responsable.option("text");
            } else if (Codigo.option("value") != "") {
                filtros = "C:" + Codigo.option("value");
            } else if (Prefijo.option("value") >= 1 && Minimo.option("value") != "" && Maximo.option("value") != "") {
                var min = Minimo.option("value");
                var max = Maximo.option("value")
                if (min > max) {
                    DevExpress.ui.dialog.alert('Rango de etiquetas mal extablecido', 'Buscar bienes');
                }else filtros = "P:" + Prefijo.option("text") + ";" + Minimo.option("value") + ";" + Maximo.option("value");
            } else DevExpress.ui.dialog.alert('No se ha ingresado un dato para buscar', 'Buscar bienes');
            $("#gridEtiquetas").dxDataGrid("instance").refresh();
        }
    });


    $("#btnLimpiar").dxButton({
        icon: "clearsquare",
        text: 'Limpiar filtros',
        onClick: function () {
            filtros = "";
            Responsable.option("value", null);
            Prefijo.option("value", null);
            Minimo.option("value", "");
            Maximo.option("value", "");
            Codigo.option("value", "");
            $("#gridEtiquetas").dxDataGrid("instance").refresh();
        }
    });

    var Prefijo = $("#cboPrefijo").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "ORDEN",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#Etiquetas").data("url") + "Dynamics/api/EtiquetaApi/Prefijos");
                }
            })
        }),
        displayExpr: "PREFIJO",
        valueExpr: "ORDEN",
        searchEnabled: true,
        noDataText: "No hay datos para mostrar",
        placeholder: "Seleccione",
        showClearButton: true
    }).dxSelectBox("instance");

    var Minimo = $("#numMin").dxNumberBox({
        showSpinButtons: true,
        value: ""
    }).dxNumberBox("instance");

    var Maximo = $("#numMax").dxNumberBox({
        showSpinButtons: true,
        value: ""
    }).dxNumberBox("instance");

    $("#gridEtiquetas").dxDataGrid({
        dataSource: grdEtiquetas,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            pageSize: 5, pageIndex: 1
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20, 50]
        },
        selection: {
            mode: 'multiple',
            allowSelectAll: true,
            showCheckBoxesMode: 'always'
        },
        remoteOperations: true,
        hoverStateEnabled: true,
        columns: [
            { dataField: 'CODIGO', width: '15%', caption: 'Código', dataType: 'string' },
            { dataField: 'NOMBREBIEN', width: '25%', caption: 'Nombre del bien', dataType: 'string' },
            { dataField: 'ESTADOBIEN', width: '15%', caption: 'Estado', dataType: 'string' },
            { dataField: 'PERSONABIEN', width: '30%', caption: 'Responsable', dataType: 'string' },
            {
                caption: 'Etiquetas',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'print',
                        hint: 'Imprimir etiqueta ' + options.data.CODIGO,
                        onClick: function (e) {
                            window.open($('#Etiquetas').data('url') + "Dynamics/Etiqueta/ImprimirEti?Bien=" + options.data.CODIGO, "Etiqueta " + options.data.CODIGO, "width= 900,height=800,scrollbars = yes, location = no, toolbar = no, menubar = no, status = no");
                        }
                    }).appendTo(container);
                }
            }
        ]
    });

    $("#btnImprimeSel").dxButton({
        icon: 'print',
        hint: 'Imprimir etiqueta bienes seleccionados',
        onClick: function (e) {
            var grid = $('#gridEtiquetas').dxDataGrid('instance');
            var DatosGridEti = grid.getSelectedRowsData();
            var Etiquetas = [];
            for (i = 0; i < DatosGridEti.length; i++) {
                Etiquetas.push(DatosGridEti[i].CODIGO);
            }
            var ArrlistOfEti = JSON.stringify(Etiquetas);
            window.open($('#Etiquetas').data('url') + "Dynamics/Etiqueta/ImprimirEtiSel?ListaEti=" + ArrlistOfEti, "Etiquetas Seleccionadas", "width= 900,height=800,scrollbars = yes, location = no, toolbar = no, menubar = no, status = no");
        }

    });
});

var grdEtiquetas = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"CODIGO","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 10);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 10);
        $.getJSON($('#Etiquetas').data('url') + 'Dynamics/api/EtiquetaApi/ConsultaBienes', {
            filter: filterOptions,
            sort: sortOptions,
            group: groupOptions,
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            customFilters: filtros
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});