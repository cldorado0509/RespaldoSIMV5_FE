var _docu = "";
var BienesStore = null;
var GridCargado = null;
var BotonCargado = null;

$(document).ready(function () {

    var btnImprimir = $("#btnImprimePazySalvo").dxButton({
        text: "Imprimir Paz y Salvo para ",
        type: "default",
        visible: false,
        onClick: function () {
            if (_docu != "") {
                window.open($('#Dynamics').data('url') + "Dynamics/PazsalvoBienes/GeneraPazySalvo?Tercero=" + _docu, "Paz y Salvo Bienes", "width= 900,height=800,scrollbars = yes, location = no, toolbar = no, menubar = no, status = no");
            }
        }
    }).dxButton("instance");

    var grdBienes = $("#gridBienes").dxDataGrid({
        dataSource: BienesStore,
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
            { dataField: 'CODIGO', width: '10%', caption: 'Código', dataType: 'string' },
            { dataField: 'FECHAINV', width: '10%', caption: 'Último Inventario (M/D/YYYY)', dataType: 'date', format: 'MM/dd/yyyy' },
            { dataField: 'NOMBREBIEN', width: '20%', caption: 'Nombre del bien', dataType: 'string' },
            { dataField: 'ESTADOBIEN', width: '12%', caption: 'Estado', dataType: 'string' },
            { dataField: 'PERSONABIEN', width: '20%', caption: 'Responsable', dataType: 'string' },
            { dataField: 'UBICACION', width: '18%', caption: 'Ubicación', dataType: 'string' },
        ]
    }).dxDataGrid("instance");


    $("#btnInforme").dxButton({
        icon: "filter",
        text: 'Buscar',
        onClick: function () {
            btnImprimir.option("visible", false);
            grdBienes.option("visible", false);
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
        value: ""
    }).dxTextBox("instance");

    var Tercero = $("#cboTercero").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "TERCERO",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#Dynamics").data("url") + "Dynamics/api/pazsalvoBienesApi/Terceros");
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
});