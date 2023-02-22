$(document).ready(function () {
    var IdRegistro = -1;
    var IdProducto = -1;
    var productSel = 0;

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
                IdInstalacion = data.ID_INSTALACION;
                $('#GidListadoProductos').dxDataGrid({ dataSource: ProductosDataSource });
                var _Ruta = $('#SIM').data('url') + "MiBici/api/InfOrganizacionAPI/GetFotoInstalacion?IdTercero=" + IdTercero + "&IdInstalacion=" + IdInstalacionSel;
                $.ajax({
                    type: "GET",
                    dataType: 'json',
                    url: _Ruta,
                    contentType: "application/json",
                    beforeSend: function () { },
                    success: function (data) {
                        $('#imgLogo').attr('src', 'data:image/png;base64,' + data.photo_file);
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                    }
                });
            }
        },
        onInitialized: function (i) {
            $("#GidListado").dxDataGrid("selectRowsByIndexes", [0]);
        }
    });

    $("#GridListadoProductos").dxDataGrid({
        dataSource: ProductosDataSource,
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
            { dataField: 'S_NOMBRE', width: '40%', caption: 'Producto', dataType: 'string' },
            {
                dataField: 'N_VALOR_UNIDAD', width: '30%', caption: 'Valor Unitario', dataType: 'number', format: "currency",
                editorOptions: {
                    format: "currency",
                    showClearButton: true
                }  },
            {
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        type: 'success',
                        height: 30,
                        width: 30,
                        hint: 'Editar la información del Producto',
                        onClick: function (e) {
                            
                            var _Ruta = $('#SIM').data('url') + "MiBici/api/InfOrganizacionAPI/ObtenerProducto";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.ID
                                }).done(function (data) {
                                    if (data !== null) {
                                        IdProducto = parseInt(data.id);
                                        txtProducto.option("value", data.nombreProducto);
                                        cboUnidadMed.option("text", data.unidadMed);
                                        txtValorUnitario.option("value", data.valorUnitario);
                                        txtDescripcionProducto.option("value", data.descripcionProducto);
                                        var OptEstado = data.activo;
                                        popupP.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');
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
                        icon: 'photo',
                        height: 20,
                        hint: 'Agregar Foto',
                        onClick: function (e) {
                            productSel = options.data.ID;
                            var _Ruta = $('#SIM').data('url') + "MiBici/api/InfOrganizacionAPI/ObtenerProductoF";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.ID
                                }).done(function (data) {
                                    if (data !== null) {
                                        IdProducto = parseInt(data.id);
                                        txtProductoF.option("value", data.nombreProducto);
                                        txtDescripcionProductoF.option("value", data.descripcionProducto);
                                        popupF.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');
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
                        hint: 'Eliminar el Producto',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar el Producto : ' + options.data.S_NOMBRE + '?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "MiBici/api/InfOrganizacionAPI/EliminarProducto";
                                    $.getJSON(_Ruta,
                                        {
                                            objData: options.data.ID
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar la Empresa');
                                            else {
                                                $('#GidListadoProductos').dxDataGrid({ dataSource: ProductosDataSource });
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar la Empresa');
                                        });
                                }
                            });
                        }
                    }).appendTo(container);
                }
            }

        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                productSel = data.ID;
            }
        },
        onInitialized: function (i) {
            $("#GidListadoProductos").dxDataGrid("selectRowsByIndexes", [0]);
        }
    });
       
    var cboUnidadMed = $("#cboUnidadMed").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "MiBici/api/InfOrganizacionAPI/GetUnidadesMedida");
                }
            })
        }),
        displayExpr: "Nombre",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "La Unidad de Medida es obligatoria!"
        }]
    }).dxSelectBox("instance");

    var cboCategoria = $("#cboCategoria").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "MiBici/api/InfOrganizacionAPI/GetCategorias");
                }
            })
        }),
        displayExpr: "Nombre",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "La Categoría a la que pertenece la empresa es obligatoria!"
        }]
    }).dxSelectBox("instance");

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


 

    var galleryproductsl = [
        "../../Areas/MiBici/images/1.jpg",
        "../../Areas/MiBici/images/2.jpg",
        "../../Areas/MiBici/images/3.jpg",
        "../../Areas/MiBici/images/4.jpg",
        "../../Areas/MiBici/images/5.jpg"
    ];

  
    var galleryproduct = $("#galleryproduct").dxGallery({
        dataSource: galleryproductsl,
        height: 300,
        loop: true,
        slideshowDelay: 2000,
        showNavButtons: true,
        showIndicator: true
    }).dxGallery("instance");


    var fileUploader = $("#file-uploader").dxFileUploader({
        multiple: false,
        accept: "image/*",
        chunkSize: 200000,
        value: [],
        uploadMode: "instantly",
        uploadUrl: $('#SIM').data('url') + "MiBici/api/InfOrganizacionAPI/SubirFoto?id=Foto" + IdInstalacionSel,
        onValueChanged: function (e) {
            var files = e.value;
            if (files.length > 0) {
                $("#selected-files .selected-item").remove();
                $.each(files, function (i, file) {
                    var $selectedItem = $("<div />").addClass("selected-item");
                    $selectedItem.append(
                        $("<span />").html("Name: " + file.name + "<br/>"),
                        $("<span />").html("Size " + file.size + " bytes" + "<br/>"),
                        $("<span />").html("Type " + file.type + "<br/>")
                    );
                    $selectedItem.appendTo($("#selected-files"));
                });
                $("#selected-files").show();
            }
            else
                $("#selected-files").hide();
        },
        onUploadError: function (e) {
            var xhttp = e.request;
            if (xhttp.readyState == 4 && xhttp.status == 0) {
                alert("Connection refused.");
            }
        },
    }).dxFileUploader("instance");

    var fileUploaderF = $("#file-uploaderf").dxFileUploader({
        multiple: false,
        accept: "*",
        value: [],
        uploadMode: "instantly",
        uploadUrl: $('#SIM').data('url') + "MiBici/api/InfOrganizacionAPI/SubirFotoP?id=Foto" + productSel,
        onValueChanged: function (e) {
            var files = e.value;
            if (files.length > 0) {
                $("#selected-filesf .selected-item").remove();
                $.each(files, function (i, file) {
                    var $selectedItem = $("<div />").addClass("selected-item");
                    $selectedItem.append(
                        $("<span />").html("Name: " + file.name + "<br/>"),
                        $("<span />").html("Size " + file.size + " bytes" + "<br/>"),
                        $("<span />").html("Type " + file.type + "<br/>")
                    );
                    $selectedItem.appendTo($("#selected-filesf"));
                });
                $("#selected-filesf").show();
            }
            else
                $("#selected-filesf").hide();
        }
    }).dxFileUploader("instance");

    var txtProducto = $("#txtProducto").dxTextBox({
        placeholder: "Ingrese el nombre del producto ...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El nombre del producto es obligatorio!"
        }]
    }).dxTextBox("instance");

    var txtProductoF = $("#txtProductoF").dxTextBox({
        value: "",
        enabled:false,
    }).dxTextBox("instance");

  
    var txtValorUnitario = $("#txtValorUnitario").dxNumberBox({
        placeholder: "Ingrese el valor unitario ...",
        //value: "",
        format: "$ #,##0.##",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El valor unitario es obligatorio!"
        }]
    }).dxNumberBox("instance");

  
    var txtDescripcionProducto = $("#txtDescripcionProducto").dxTextArea({
        height: 150,
        placeholder: "Ingrese la descripción del producto...",
        value: "",
    }).dxTextArea("instance");

    var txtDescripcionProductoF = $("#txtDescripcionProductoF").dxTextArea({
        height: 150,
        value: "",
    }).dxTextArea("instance");

    var chkEstadoProducto = $("#chkEstadoProducto").dxCheckBox({
        value: false,
        width: 80,
        text: "Si",

    }).dxCheckBox("instance");


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
                        $('#GidListado').dxDataGrid({ dataSource: TerceroInstalacionDataSource });
                        $("#PopupInformacion").dxPopup("instance").hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    $("#btnGuardaProducto").dxButton({
        text: "Guardar",
        type: "success",
        icon: "floppy",
        height: 30,
        width: 60,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var id = IdProducto;
            var nombreProducto = txtProducto.option("value");
            var descripcionProducto = txtDescripcionProducto.option("value");
            var valorUnitario = txtValorUnitario.option("value");
            var unidadMed = cboUnidadMed.option("value").Id;
          
            var params = {id:id, instalacionId: IdInstalacionSel, terceroId: IdTercero, nombreProducto: nombreProducto, descripcionProducto: descripcionProducto, valorUnitario: valorUnitario, unidadMed: unidadMed };
            var _Ruta = $('#SIM').data('url') + "MiBici/api/InfOrganizacionAPI/GuardarProducto";
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
                        IdInstalacionSel = data.idinstalacion;
                        IdTercero = data.idtercero;
                        DevExpress.ui.dialog.alert('Datos Guardados correctamente', 'Guardar Datos');

                        
                        $('#GidListadoProductos').dxDataGrid({ dataSource: ProductosDataSource });
                        $("#PopupNuevoProducto").dxPopup("instance").hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    $("#btnGuardaFoto").dxButton({
        text: "Guardar",
        type: "success",
        icon: "floppy",
        height: 30,
        width: 60,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var id = productSel;
            var params = { id: id };
            var _Ruta = $('#SIM').data('url') + "MiBici/api/InfOrganizacionAPI/GuardarFotoProducto";
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
                        $('#GidListadoProductos').dxDataGrid({ dataSource: ProductosDataSource });
                        $("#PopupFotos").dxPopup("instance").hide();
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

    var popupP = $("#PopupNuevoProducto").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Producto"
    }).dxPopup("instance");

    var popupF = $("#PopupFotos").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Fotos del Producto"
    }).dxPopup("instance");

    $("#btnNuevoProducto").dxButton({
        stylingMode: "contained",
        text: "Nuevo",
        type: "success",
        width: 200,
        height: 30,
        icon: 'new',
        onClick: function () {
            if (IdInstalacionSel == -1) {
                DevExpress.ui.dialog.alert('Debe seleccionar una Instalación en el Grid superior para poder crear un Producto', 'Nuevo Producto');
            } else {
                IdProducto = -1;
                txtProducto.reset();
                txtDescripcionProducto.reset();
                txtValorUnitario.reset();
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

var ProductosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        $.getJSON($('#SIM').data('url') + 'MiBici/api/InfOrganizacionAPI/GetProductos', {
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


