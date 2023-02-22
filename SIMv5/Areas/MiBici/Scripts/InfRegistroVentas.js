
$(document).ready(function () {
    var IdRegistro = -1;
    var IdVenta = -1;

    //Información de la Organización
    $("#GidListado").dxDataGrid({
        dataSource: TerceroInstalacionDataSource,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            pageSize: 5
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20, 50]
        },
        filterRow: {
            visible: true,
            emptyPanelText: 'Arrastre una columna para agrupar'
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Buscar..."
        },
        selection: {
            mode: 'single',
            selectRowsByIndexes: [0],
        },
        hoverStateEnabled: true,
        remoteOperations: true,
        columns: [
            { dataField: 'ID_INSTALACION', width: '10%', caption: 'Id', alignment: 'center' },
            { dataField: 'INSTALACION', width: '30%', caption: 'Instalación', alignment: 'center' },
            { dataField: 'MUNICIPIO', width: '10%', caption: 'Municipio', dataType: 'string' },
            { dataField: 'DIRECCION', width: '30%', caption: 'Dirección', dataType: 'string' },
            {
                dataField: 'ACTUAL', width: '10%', caption: 'Actual', dataType: 'string', customizeText: function (cellInfo) {
                    if (cellInfo.value == '1') {
                        return 'Si';
                    }
                    else {
                        return 'No';
                    }
                }
            },
            {
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        type:'success',
                        height: 30,
                        width:30,
                        hint: 'Editar la información de la Organización',
                        onClick: function (e) {
                            IdInstalacionSel = options.data.ID_INSTALACION;
                            var _Ruta = $('#SIM').data('url') + "MiBici/api/InfOrganizacionAPI/ObtenerInformacionOrganizacion";
                            $.getJSON(_Ruta,
                                {
                                    IdInstalacion: options.data.ID_INSTALACION, IdTercero: IdTercero
                                }).done(function (data) {
                                    if (data !== null) {
                                        IdRegistro = parseInt(data.id);
                                        IdInstalacionSel = parseInt(data.idInstalacion);
                                        txtDescripcion.option("value", data.descripcion);
                                        txtWeb.option("value", data.sitioWeb);
                                        txtInstagram.option("value", data.instagram);
                                        txtFacebook.option("value", data.facebook);
                                        txtWhatsApp.option("value", data.whatsApp)
                                        txtEmailVentas.option("value", data.eMail);
                                        txtDireccion.option("value", data.direccion);
                                        txtTelefonos.option("value", data.telefono);
                                        popup.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                                });
                        }
                    }).appendTo(container);
                }
            },
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                IdInstalacionSel = data.ID_INSTALACION;
                $('#GridListadoVentas').dxDataGrid({ dataSource: VentasDataSource });
            }
        },
        onInitialized: function (i) {
            $("#GidListado").dxDataGrid("selectRowsByIndexes", [0]);
        }
    });

    $("#GridListadoVentas").dxDataGrid({
        dataSource: VentasDataSource,
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
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Buscar..."
        },
        selection: {
            mode: 'single'
        },
        hoverStateEnabled: true,
        remoteOperations: true,
        columns: [
            { dataField: 'ID', width: '10%', caption: 'Código', alignment: 'center' },
            { dataField: 'D_VENTA', width: '10%', caption: 'Fecha venta', dataType: 'string' },
            { dataField: 'S_DOCUMENTO_CLIENTE', width: '15%', caption: 'Identificación Cliente', dataType: 'string' },
            { dataField: 'S_MARCA', width: '20%', caption: 'Marca', dataType: 'string' },
            { dataField: 'S_REFERENCIA', width: '20%', caption: 'Referencia', dataType: 'string' },
            { dataField: 'S_SERIAL', width: '20%', caption: 'Serial', dataType: 'string' },
            {
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        height: 20,
                        hint: 'Editar la información del Registro',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "MiBici/api/InfOrganizacionAPI/ObtenerVenta";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.ID
                                }).done(function (data) {
                                    if (data !== null) {
                                        IdVenta = parseInt(data.id);
                                        var FechaV = data.fechaVenta != null ? new Date(data.fechaVenta) : "";
                                        txtFechaVenta.option("value", FechaV);
                                        txtIdentificacionCliente.option("value", data.identificacionCliente);
                                        txtMarca.option("value", data.marca);
                                        txtSerial.option("value", data.serial);
                                        txtReferencia.option("value", data.referencia);
                                        popupP.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Editar registro de venta');
                                });
                        }
                    }).appendTo(container);
                }
            },
            {
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'clear',
                        //height: 30,
                        hint: 'Eliminar el registro de venta',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar el registro de la venta : ' + options.data.S_REFERENCIA + '?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "MiBici/api/InfOrganizacionAPI/EliminarVenta";
                                    $.getJSON(_Ruta,
                                        {
                                            objData: options.data.ID
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar el registro');
                                            else {
                                                $('#GridListadoVentas').dxDataGrid({ dataSource: VentasDataSource });
                                            }
                                            $('#GridListadoVentas').dxDataGrid({ dataSource: VentasDataSource });
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar el registro');
                                        });
                                }
                            });
                        }
                    }).appendTo(container);
                }
            }
        ]
    });
 

    var txtDescripcion = $("#txtDescripcion").dxTextArea({
        placeholder: "Ingrese la descripción de la empresa...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "La descripción de la empresa es requerida!"
        }]
    }).dxTextArea("instance");

    var txtWeb = $("#txtWeb").dxTextBox({
        placeholder: "Ingrese la dirección del sitio web de la empresa...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "La dirección del sitio web la empresa es requerida!"
        }]
    }).dxTextBox("instance");

   
    var txtInstagram = $("#txtInstagram").dxTextBox({
        value: "",

    }).dxTextBox("instance");

    var txtFacebook = $("#txtFacebook").dxTextBox({
        value: "",

    }).dxTextBox("instance");

    var txtWhatsApp = $("#txtWhatsApp").dxTextBox({
        value: "",

    }).dxTextBox("instance"); 

    var txtEmailVentas = $("#txtEmailVentas").dxTextBox({
        value: "",

    }).dxTextBox("instance");

    var txtDireccion = $("#txtDireccion").dxTextBox({
        placeholder: "Ingrese la dirección de la instalación...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "La dirección de la instalación es requerida!"
        }]
    }).dxTextBox("instance");

    var txtTelefonos = $("#txtTelefonos").dxTextBox({
        placeholder: "Ingrese los teléfonos de la instalación...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Los teléfonos de la instalación son requeridos!"
        }]
    }).dxTextBox("instance");

   
    var txtFechaVenta = $("#txtFechaVenta").dxDateBox({
        type: "datetime",
        value: new Date(),
        showAnalogClock: false,
        onOpened: function (args) {
            let position = args.component._popup.option("position");
            position.my = "center";
            position.at = "left";
            args.component._popup.option("position", position);
        }
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "La fecha del registro de la venta es obligatoria"
        }]
    }).dxDateBox("instance");
  
   
    var txtIdentificacionCliente = $("#txtIdentificacionCliente").dxTextBox({
        placeholder: "Ingrese el número del documento del cliente...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El número del documento del cliente es requerido"
        }]
    }).dxTextBox("instance");


    var txtMarca = $("#txtMarca").dxTextBox({
        value: "",

    }).dxTextBox("instance");


    var txtSerial = $("#txtSerial").dxTextBox({
        value: "",

    }).dxTextBox("instance");


    var txtReferencia = $("#txtReferencia").dxTextBox({
        placeholder: "Ingrese la referencia del producto...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "La referencia de producto es requerida"
        }]
    }).dxTextBox("instance");

    $("#btnGuardar").dxButton({
        text: "Guardar",
        type: "default",
        icon:"save",
        height: 30,
        width:60,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var id = IdRegistro;
            var idCategoria = 1;
            var descripcion = txtDescripcion.option("value");
            var sitioweb = txtWeb.option("value");
            var instagram = txtInstagram.option("value");
            var facebook = txtFacebook.option("value");
            var whatsApp = txtWhatsApp.option("value");
            var email = txtEmailVentas.option("value");
            var direccion = txtDireccion.option("value");
            var telefonos = txtTelefonos.option("value");
           
            var params = { id: id, idTercero: IdTercero, idInstalacion: IdInstalacionSel, idCategoria: idCategoria, sitioweb: sitioweb, descripcion: descripcion, instagram: instagram, facebook: facebook, email: email, direccion: direccion, telefono: telefonos, whatsApp: whatsApp};
            var _Ruta = $('#SIM').data('url') + "MiBici/api/InfOrganizacionAPI/GuardarInformacion";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                beforeSend: function () { },
                success: function (data) {
                    if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Guardar Datos');
                    else {
                        DevExpress.ui.dialog.alert('Datos Guardados correctamente', 'Guardar Datos');
                        $('#GidListado').dxDataGrid({ dataSource: SerieDocumentalDataSource });
                        $("#PopupNuevaSerie").dxPopup("instance").hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });


    $("#btnGuardaRegistro").dxButton({
        text: "Guardar",
        type: "success",
        icon: "floppy",
        height: 30,
        width: 60,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var id = IdVenta;
            var fechaVenta = txtFechaVenta.option("value");
            var identificacionCliente = txtIdentificacionCliente.option("value");
            var marca = txtMarca.option("value");
            var serial = txtSerial.option("value");
            var referencia = txtReferencia.option("value");
          

            var params = {id : id, instalacionId: IdInstalacionSel, terceroId: IdTercero, fechaVenta: fechaVenta, identificacionCliente: identificacionCliente, marca: marca, serial: serial, referencia: referencia};
            var _Ruta = $('#SIM').data('url') + "MiBici/api/InfOrganizacionAPI/GuardarVenta";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                icon: 'edit',
                data: JSON.stringify(params),
                contentType: "application/json",
                beforeSend: function () { },
                success: function (data) {
                    if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Guardar Datos');
                    else {
                        DevExpress.ui.dialog.alert('Datos Guardados correctamente', 'Guardar Datos');
                        $('#GridListadoVentas').dxDataGrid({ dataSource: VentasDataSource });
                        $("#PopupNuevoRegistro").dxPopup("instance").hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    var popup = $("#PopupInformacion").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Información de la Organización"
    }).dxPopup("instance");

    var popupP = $("#PopupNuevoRegistro").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Registro de venta"
    }).dxPopup("instance");

    $("#btnNuevoRegistro").dxButton({
        stylingMode: "contained",
        text: "Nuevo",
        type: "success",
        width: 200,
        height: 30,
        icon: 'new',
        onClick: function () {
            if (IdInstalacionSel == -1) {
                DevExpress.ui.dialog.alert('Debe seleccionar una Instalación en el Grid superior para poder registrar una venta', 'Nueva Venta');
            } else {
                IdVenta = -1;
                txtIdentificacionCliente.reset();
                txtMarca.reset();
                txtSerial.reset();
                txtReferencia.reset();
                
                popupP.show();
            }
        }
    });
});


var TerceroInstalacionDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        $.getJSON($('#SIM').data('url') + 'MiBici/api/InfOrganizacionAPI/GetIntalacionesTercero', {
            TerceroId: IdTercero
        }).done(function (data) {
            if (data.Status === false) {
                alert(data.MessageError);
            }
            d.resolve(data.datos, { totalCount: data.numRegistros });
            IdInstalacionSel = data.datos[0].ID_INSTALACION;
        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var VentasDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        $.getJSON($('#SIM').data('url') + 'MiBici/api/InfOrganizacionAPI/GetVentas', {
            IdInstalacion: IdInstalacionSel,
            IdTercero: IdTercero
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
      
        return d.promise();
    }
});
