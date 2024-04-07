var FacturasStore = null;
var filtros = "";

$(document).ready(function () {

    var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;

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
        editing: {
            mode: "cell",
            allowUpdating: true,
            allowAdding: false,
            allowDeleting: false
        },
        remoteOperations: true,
        repaintChangesOnly: true,
        hoverStateEnabled: true,
        columns: [
            { dataField: 'FACTURA', width: '10%', caption: 'Factura', dataType: 'string', allowEditing: false },
            { dataField: 'FECHAFACTURA', width: '10%', caption: 'Fecha (d/m/y)', dataType: 'date', format: 'MM/dd/yyyy', allowEditing: false },
            { dataField: 'DOCUMENTO', width: '10%', caption: 'Documento', dataType: 'string', allowEditing: false },
            { dataField: 'TERCERO', width: '20%', caption: 'Nombre tercero', dataType: 'string', allowEditing: false },
            { dataField: 'MUNICIPIO', width: '10%', caption: 'Ciudad', dataType: 'string', allowEditing: false },
            {
                dataField: 'EMAIL', width: '25%', caption: 'Correo Electrónico', dataType: 'string',
                allowEditing: true,
                cellTemplate: function (cellElement, cellInfo) {
                    cellElement.html(cellInfo.data.EMAIL);
                },
                editCellTemplate: function (cellElement, cellInfo) {
                    var div = document.createElement("div");
                    cellElement.get(0).appendChild(div);
                    $(div).dxTextBox({
                        placeholder: "Ingrese el email",
                        value: cellInfo.data.EMAIL,
                        onValueChanged: function (e) {
                            if (e.value != null && e.value != "") {
                                if (emailPattern.test(e.value)) cellInfo.data.EMAIL = e.value;
                                else DevExpress.ui.dialog.alert('El correo electrónico ' + e.value + ' no posee un formato válido');
                            }
                        }
                    });
                }
            },
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
              }
            },
            {
                caption: 'Enviar',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'email',
                        hint: 'Enviar factura',
                        onClick: function (e) {
                            popMensaje.show();
                            $("#txtFactura").text(options.data.FACTURA);
                            $("#txtTercero").text(options.data.TERCERO);
                            Email.option("value", options.data.EMAIL);
                            Mensaje.reset();                           
                        }
                    }).appendTo(container);
                }
            },
        ],
        onSelectionChanged(selectedItems) {
            const data = selectedItems.selectedRowsData;
            if (data.length > 0) {
                $("#btnImprimeSel").dxButton("instance").option("visible", true);
                $("#btnEnviarSel").dxButton("instance").option("visible", true);
            } else {
                $("#btnImprimeSel").dxButton("instance").option("visible", false);
                $("#btnEnviarSel").dxButton("instance").option("visible", false);
            }
        }
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
                if (_Desde != "" && _Desde != null && _Hasta != "" && _Hasta != nul) {
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
            filtros = "";
            Documento.option("value", null);
            Tercero.option("value", null);
            grdFacturas.refresh();
            Facturas.option("value", "");
            FecDesde.reset();
            FecHasta.reset();
        }
    });

    var Email = $("#txtCorreoEle").dxTextBox({
        placeholder: "Ingrese el documento",
        value: "",
        onValueChanged: function (e) {
            if (e.value != null && e.value != "") {
                if (!emailPattern.test(e.value)) DevExpress.ui.dialog.alert('El correo electrónico ' + e.value + ' no posee un formato válido');
            }
        }
    }).dxTextBox("instance");

    var Mensaje = $("#txtMensaje").dxTextArea({
        placeholder: "Ingrese el mensaje para el correo electrónico",
        value: "",
        minHeight: 50,
        maxHeight: 150,
        autoResizeEnabled: true
    }).dxTextArea("instance");

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
            if (data.value != null && data.value != "" ) {
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

    var popMensaje = $("#popupEnviaCorreo").dxPopup({
        width: 900,
        height: 400,
        hoverStateEnabled: true,
        title: "Envio factura",
        dragEnabled: true,
        toolbarItems: [{
            widget: 'dxButton',
            toolbar: 'bottom',
            location: 'center',
            options: {
                icon: 'email',
                text: 'Enviar correo electrónico',
                elementAttr: { class: 'dx-popup-content-bottom' },
                type: 'default',
                onClick: function (e) {
                    if (Email.option("value") == null || Email.option("value") == "") {
                        DevExpress.ui.dialog.alert('El correo electrónico es requerido!!');
                        return;
                    } 
                    var URL = $("#Dynamics").data("url") + "Dynamics/api/FacturasApi/EnviarFactura";
                    var IdFact = $("#txtFactura").text();
                    var Tercero = $("#txtTercero").text();
                    var parametros = { IdFact: IdFact, Mail: Email.option("value"), Tercero: Tercero,  Mensaje: Mensaje.option("value") };
                    $.ajax({
                        type: "POST",
                        dataType: 'json',
                        url: URL,
                        data: JSON.stringify(parametros),
                        contentType: "application/json",
                        beforeSend: function () { },
                        success: function (data) {
                            if (data.result == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Enviar Facturas');
                            else {
                                DevExpress.ui.dialog.alert(data.mensaje, 'Enviar Facturas');
                                popMensaje.hide();
                            }
                        },
                        error: function (xhr, textStatus, errorThrown) {
                            DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Enviar Facturas');
                        }
                    });
                }
            }
        }]
    }).dxPopup("instance");

    $("#btnEnviarFact").dxButton({
        icon: 'email',
        text: 'Enviar factura',
        onClick: function (e) {
            Email.option("value", options.data.EMAIL);
            Mensaje.reset();
            popMensaje.show();
        }
    });

    var MensajeSel = $("#txtMensajeSel").dxTextArea({
        placeholder: "Ingrese el mensaje para el correo electrónico",
        value: "",
        minHeight: 50,
        maxHeight: 150,
        autoResizeEnabled: true
    }).dxTextArea("instance");

    $("#btnImprimeSel").dxButton({
        icon: 'print',
        text: 'Imprimir Facturas Seleccionadas',
        type: 'default',
        visible: false,
        onClick: function (e) {
            var DatosFacturasSel = grdFacturas.getSelectedRowsData();
            if (DatosFacturasSel.length > 0) {
                var Facturas = [];
                for (i = 0; i < DatosFacturasSel.length; i++) {
                    Facturas.push(DatosFacturasSel[i].FACTURA);
                }
                var ArrFacturas = JSON.stringify(Facturas);
                window.open($('#Dynamics').data('url') + "Dynamics/Factura/ImprimirFactSel?ListFacturas=" + ArrFacturas, "Facturas Seleccionadas", "width= 900,height=800,scrollbars = yes, location = no, toolbar = no, menubar = no, status = no");
            } else {
                DevExpress.ui.dialog.alert('No ha seleccionado facturas para imprimir!', 'Imprimir Facturas');
            }
        }
    });

    $("#btnEnviarSel").dxButton({
        icon: 'email',
        text: 'Enviar Facturas Seleccionadas',
        type: 'default',
        visible: false,
        onClick: function (e) {
            var _vacias = false;
            var DatosFacturasSel = grdFacturas.getSelectedRowsData();
            if (DatosFacturasSel.length > 0) {
                for (i = 0; i < DatosFacturasSel.length; i++) {
                    if (DatosFacturasSel[i].EMAIL == null || DatosFacturasSel[i].EMAIL == "") _vacias = true;
                }
                if (_vacias) {
                    var result = DevExpress.ui.dialog.confirm('Se encontraron registro sin una dirección de correo electrónico y estas serán omitidas, desea continuar?', 'Confirmación');
                    result.done(function (dialogResult) {
                        if (dialogResult) {
                            popMensajeSel.show();
                            MensajeSel.reset();
                        } else return;
                    });
                } else { 
                    popMensajeSel.show();
                    MensajeSel.reset();
                }
            } else {
                DevExpress.ui.dialog.alert('No ha seleccionado facturas para enviar por correo electrónico!', 'Imprimir Facturas');
            }
        }
    });

    var popMensajeSel = $("#popupEnviaCorreoSel").dxPopup({
        width: 900,
        height: 350,
        hoverStateEnabled: true,
        title: "Envio facturas Seleccionadas",
        dragEnabled: true,
        toolbarItems: [{
            widget: 'dxButton',
            toolbar: 'bottom',
            location: 'center',
            options: {
                icon: 'email',
                text: 'Enviar correos electrónicos',
                elementAttr: { class: 'dx-popup-content-bottom' },
                type: 'default',
                onClick: function (e) {
                    var URL = $("#Dynamics").data("url") + "Dynamics/api/FacturasApi/EnviarFacturasSel";
                    var DatosFacturasSel = grdFacturas.getSelectedRowsData();
                    if (DatosFacturasSel.length > 0) {
                        var Facturas = [];
                        for (i = 0; i < DatosFacturasSel.length; i++) {
                            Facturas.push({ IdFact: DatosFacturasSel[i].FACTURA, Mail: DatosFacturasSel[i].EMAIL, Tercero: DatosFacturasSel[i].TERCERO, Mensaje: '' });
                        }
                        if (MensajeSel.option("value") ) Facturas[0].Mensaje = MensajeSel.option("value");
                        $.ajax({
                            type: "POST",
                            dataType: 'json',
                            url: URL,
                            data: JSON.stringify(Facturas),
                            contentType: "application/json",
                            beforeSend: function () { },
                            success: function (data) {
                                if (data.result == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Enviar Facturas');
                                else {
                                    DevExpress.ui.dialog.alert(data.mensaje, 'Enviar Facturas');
                                    popMensaje.hide();
                                }
                            },
                            error: function (xhr, textStatus, errorThrown) {
                                DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Enviar Facturas');
                            }
                        });
                    }
                }
            }
        }]
    }).dxPopup("instance");

});