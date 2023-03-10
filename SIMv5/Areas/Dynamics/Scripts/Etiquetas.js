$(document).ready(function () {
    var Codigo = $("#txtCodigo").dxTextBox({
        placeholder: "Ingrese el código del bien",
        value: ""
    }).dxTextBox("instance");

    var Responsable = $("#cboResponsable").dxSelectBox({
        placeholder: 'Seleccione el responsable',
        showClearButton: true
    }).dxSelectBox("instance");

    $("#btnInforme").dxButton({
        icon: "filter",
        text: 'Buscar',
        onClick: function () {

        }
    });

    $("#btnLimpiar").dxButton({
        icon: "clearsquare",
        text: 'Limpiar filtros',
        onClick: function () {

        }
    });

    var Prefijo = $("#cboPrefijo").dxSelectBox({
        placeholder: 'Seleccione el prefijo',
        showClearButton: true
    }).dxSelectBox("instance");

    var Minimo = $("#numMin").dxTextBox({
        placeholder: "Ingrese el código del bien",
        value: ""
    }).dxTextBox("instance");

    var Maximo = $("#numMax").dxTextBox({
        placeholder: "Ingrese el código del bien",
        value: ""
    }).dxTextBox("instance");

    var gridEtiquetas = $("#gridEtiquetas").dxDataGrid({
        dataSource: grdEtiquetas,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            enabled: false
        },
        selection: {
            mode: 'none'
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
                        hint: 'Imprimir etiqueta ' + options.data.Codigo,
                        onClick: function (e) {

                        }
                    }).appendTo(container);
                }
            }
        ]
    }).dxDataGrid("instance");
});

var grdEtiquetas = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"CODIGO","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#Etiquetas').data('url') + 'Dynamics/api/EtiquetaApi/ConsultaBienes', {
            filter: filterOptions,
            sort: sortOptions,
            group: groupOptions,
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            noFilterNoRecords: true
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});