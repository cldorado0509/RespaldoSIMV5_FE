var FacturasStore = null;
var filtros = "";

$(document).ready(function () {

    var grdFacturas = $("#gridFacturas").dxDataGrid({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "FACTURA",
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
   /*     visible: false,*/
        columns: [
            { dataField: 'FACTURA', width: '10%', caption: 'Factura', dataType: 'string' },
            { dataField: 'FECHAFACT', width: '10%', caption: 'Fecha', dataType: 'date', format: 'MM/dd/yyyy' },
            { dataField: 'DOCUMENTO', width: '10%', caption: 'Documento', dataType: 'string' },
            { dataField: 'TERCERO', width: '20%', caption: 'Nombre tercero', dataType: 'string' },
            { dataField: 'MUNICIPIO', width: '10%', caption: 'Ciudad', dataType: 'string' },
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
            filtros = "";
            if (Documento.option("value") >= 1) {
                filtros += ";D:" + Documento.option("value");
            }
            if (Tercero.option("selectedItem") != null) {
                filtros += ";T:" + Tercero.option("text");
            }
            if (Facturas.option("value") != "") {
                filtros += ";B:" + Facturas.option("value");
            }
            if (FecDesde.option("value") != "")
            {
                var _Desde = FecDesde.option("value");
                var _Hasta = FecHasta.option("value");
                if (_Desde != "" && _Hasta != "") {
                    if (_Hasta >= _Desde) {
                        filtros += ";F:" + _Desde + "," + _Hasta;
                    } else {
                        DevExpress.ui.dialog.alert('El rango de fechas esta mal establecido', 'Buscar facturas');
                        return;
                    }
                }
                else filtros += ";F:" + _Desde;
            }

            if (filtros == "")
            {
                DevExpress.ui.dialog.alert('No se ha ingresado un dato para buscar', 'Buscar facturas');
                return;
            }
            filtros = filtros.substring(1);
            grdFacturas.refresh();
        }
    });

    $("#btnLimpiar").dxButton({
        icon: "clearsquare",
        text: 'Limpiar filtros',
        onClick: function () {
            _docu = "";
            Documento.option("value", null);
            Tercero.option("value", null);
            BienesStore = null;
            grdBienes.option("dataSource", BienesStore);
            btnImprimir.option("visible", false);
            grdBienes.option("visible", false);
        }
    });

    var Documento = $("#txtDocumento").dxTextBox({
        placeholder: "Ingrese el documento",
        value: "",
        onValueChanged: function (e) {
            var TerceroDs = Tercero.getDataSource();
            TerceroDs.reload();
            Tercero.option("value", e.value);
        }
    }).dxTextBox("instance");

    var Tercero = $("#cboTercero").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "TERCERO",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#Dynamics").data("url") + "Dynamics/api/FacturasApi/Terceros");
                }
            })
        }),
        displayExpr: "NOMBRE",
        valueExpr: "TERCERO",
        searchEnabled: true,
        noDataText: "No hay datos para mostrar",
        placeholder: "Seleccione",
        showClearButton: true,
        onValueChanged: function (data) {
            if (data.value != null) {
                Documento.option("value", data.value);
            }
        }
    }).dxSelectBox("instance");

    var Facturas = $("#txtFacturas").dxTextBox({
        placeholder: "Ingrese la(s) factura(s) - separadas por ,",
        value: ""
    }).dxTextBox("instance");

    var FecDesde = $("#dpFecFactDesde").dxDateBox({
        type: 'date',
        value: '',
        displayFormat: 'dd/MM/yyyy',
        dateSerializationFormat: 'yyyy-MM-dd'
    }).dxDateBox("instance");

    var FecHasta = $("#dpFecFactHasta").dxDateBox({
        type: 'date',
        value: '',
        displayFormat: 'dd/MM/yyyy',
        dateSerializationFormat: 'yyyy-MM-dd'
    }).dxDateBox("instance");
});