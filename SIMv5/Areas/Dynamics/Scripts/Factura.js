var FacturasStore = null;

$(document).ready(function () {

    var grdFacturas = $("#gridBienes").dxDataGrid({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "DOCUMENTO",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#Dynamics").data("url") + "Dynamics/api/FacturasApi/ObtenerFacturas", { customFilters: filtros });
                }
            })
        }),
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
        selection: {
            mode: 'multiple',
            allowSelectAll: false,
            showCheckBoxesMode: 'always'
        },
        remoteOperations: true,
        hoverStateEnabled: true,
        visible: false,
        columns: [
            { dataField: 'FACTURA', width: '10%', caption: 'Factura', dataType: 'string' },
            { dataField: 'FECHA', width: '10%', caption: 'Fecha', dataType: 'date', format: 'MM/dd/yyyy' },
            { dataField: 'IDENTIFICACION', width: '10%', caption: 'Documento', dataType: 'string' },
            { dataField: 'NOMBRE', width: '20%', caption: 'Nombre tercero', dataType: 'string' },
            { dataField: 'EMAIL', width: '12%', caption: 'Correo Elect.', dataType: 'string' },
            {
              caption: 'Imprimir',
              alignment: 'center',
              cellTemplate: function (container, options) {
                $('<div/>').dxButton({
                    icon: 'print',
                    hint: 'Imprimir factura',
                    onClick: function (e) {
                        window.open($('#Dynamics').data('url') + "Dynamics/Factura/ImprimirFactura?IdFact=" + options.data.FACTURA, "Factura " + options.data.FACTURA, "width= 900,height=800,scrollbars = yes, location = no, toolbar = no, menubar = no, status = no");
                    }
                }).appendTo(container);
            } },
            {  },
        ]
    }).dxDataGrid("instance");

    $("#btnInforme").dxButton({
        icon: "filter",
        text: 'Buscar',
        onClick: function () {
            btnImprimir.option("visible", false);
            grdFacturas.option("visible", false);
            if (Tercero.option("value") >= 1) {
                _docu = Tercero.option("value");
                Documento.option("value", "");
            } else if (Documento.option("value") != "") {
                _docu = Documento.option("value");
            } else {
                DevExpress.ui.dialog.alert('No se ha ingresado un dato para buscar', 'Paz y Salvo bienes');
                return;
            }
            $.getJSON($("#Dynamics").data("url") + "Dynamics/api/pazsalvoBienesApi/ExisteTercero", { Documento: _docu })
                .done(function (data) {
                    if (data.resp == "Error") {
                        DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Paz y Salvo bienes');
                    }
                    else {
                        $.getJSON($("#Dynamics").data("url") + "Dynamics/api/pazsalvoBienesApi/ConsultaBienes", { Tercero: _docu })
                            .done(function (data) {
                                if (data != null && data.length > 0) {
                                    grdBienes.option("visible", true);
                                    BienesStore = new DevExpress.data.LocalStore({
                                        key: 'CODIGO',
                                        data: data,
                                        name: 'BienesStore'
                                    });
                                    grdBienes.option("dataSource", BienesStore);
                                } else {
                                    btnImprimir.option("visible", true);
                                    var Persona = Documento.option("value").length > 0 ? Documento.option("value") : Tercero.option("text");
                                    btnImprimir.option("text", "Imprimir Paz y Salvo para " + Persona);
                                }
                            });
                    }
                });
        }
    });

    var Documento = $("#txtDocumento").dxTextBox({
        placeholder: "Ingrese el documento",
        value: ""
    }).dxTextBox("instance");
});