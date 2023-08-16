var filtros = "";

$(document).ready(function () {
    var Agnos = generarArrayDeAnos();

    function generarArrayDeAnos() {
        var max = new Date().getFullYear() - 1
        var min = max - 4
        var years = []

        for (var i = max; i >= min; i--) {
            years.push(i)
        }
        return years
    }

    $("#btnInforme").dxButton({
        icon: "filter",
        text: 'Buscar',
        onClick: function () {
            if (Tercero.option("value") >= 1) {
                filtros = "F:" + Tercero.option("value") + ";" + cmbAno.option("value");
            } else if (Documento.option("value") != "") {
                filtros = "C:" + Documento.option("value") + ";" + cmbAno.option("value");
            } else if (cmbAno.option("value") >= 2020) {
                filtros = "A:" + cmbAno.option("value") + ";";
            } else DevExpress.ui.dialog.alert('No se ha ingresado un dato para buscar', 'Buscar bienes');
            $("#gridCertificados").dxDataGrid("instance").refresh();
        }
    });

    $("#btnLimpiar").dxButton({
        icon: "clearsquare",
        text: 'Limpiar filtros',
        onClick: function () {
            filtros = "";
            Documento.option("value", null);
            Tercero.option("value", null);
            cmbAno.option("value", 0);
            $("#gridCertificados").dxDataGrid("instance").refresh();
        }
    });

    var Tercero = $("#cboTercero").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "TERCERO",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#Dynamics").data("url") + "Dynamics/api/CertificadoIngresosApi/Terceros");
                }
            })
        }),
        displayExpr: "NOMBRE",
        valueExpr: "TERCERO",
        searchEnabled: true,
        noDataText: "No hay datos para mostrar",
        placeholder: "Seleccione",
        showClearButton: true
    }).dxSelectBox("instance");

    var Documento = $("#txtDocumento").dxTextBox({
        placeholder: "Ingrese el documento",
        value: ""
    }).dxTextBox("instance");

    var cmbAno = $("#cboAgno").dxSelectBox({
        items: Agnos,
        value: Agnos[0]
    }).dxSelectBox("instance");

    $("#gridCertificados").dxDataGrid({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "DOCUMENTO",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#Dynamics").data("url") + "Dynamics/api/CertificadoIngresosApi/ConsultaCertificados", { customFilters: filtros });
                }
            })
        }),
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            pageSize: 20, pageIndex: 1
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20, 50]
        },
        remoteOperations: true,
        hoverStateEnabled: true,
        columns: [
            { dataField: 'DOCUMENTO', width: '20%', caption: 'Documento', dataType: 'string' },
            { dataField: 'TERCERO', width: '60%', caption: 'Nombre', dataType: 'string' },
            { dataField: 'AGNO', width: '10%', caption: 'Año certificado', dataType: 'string' },
            {
                caption: 'Imprimir',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'print',
                        hint: 'Imprimir certificado para  ' + options.data.DOCUMENTO,
                        onClick: function (e) {
                            window.open($('#Dynamics').data('url') + "Dynamics/CertificadoIngresos/ImprimirCertificado?IdTer=" + options.data.DOCUMENTO + "&Agno=" + options.data.AGNO, "Certificado de Ingresos y Retennciones " + options.data.TERCERO, "width= 900,height=800,scrollbars = yes, location = no, toolbar = no, menubar = no, status = no");
                        }
                    }).appendTo(container);
                }
            }
        ]
    });
});

var grdCertificados = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"CODIGO","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 10);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 10);
        $.getJSON($('#Dynamics').data('url') + 'Dynamics/api/CertificadoIngresosApi/ConsultaCertificados', {
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