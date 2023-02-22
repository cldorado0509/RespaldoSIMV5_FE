var IdEmpresa = -1;

$(document).ready(function () {
    var IdRegistro = -1;
    var IdProducto = -1;
    $("#GidListado").dxDataGrid({
        dataSource: EmpresaDataSource,
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
            { dataField: 'S_NIT', width: '10%', caption: 'Nit de la Empresa', dataType: 'string' },
            { dataField: 'S_RAZON_SOCIAL', width: '30%', caption: 'Razón Social', dataType: 'string' },
            { dataField: 'CATEGORIA', width: '20%', caption: 'Categoría', dataType: 'string' },
            { dataField: 'MUNICIPIO', width: '20%', caption: 'Municipio', dataType: 'string' },
            {
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: '../Content/Images/edit.png',
                        height: 20,
                        hint: 'Editar la información de la Empresa',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "DesarrolloEconomico/api/EmpresaApi/ObtenerEmpresa";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.ID
                                }).done(function (data) {
                                    if (data !== null) {
                                        IdRegistro = parseInt(data.id);
                                        IdEmpresa = parseInt(data.id);
                                        txtNit.option("value", data.nit);
                                        txtRazonSocial.option("value", data.razonSocial);
                                        txtDescripcion.option("value", data.descripcion);
                                        txtUrlWeb.option("value", data.web);
                                        txtUrlInstagram.option("value", data.instagram);
                                        txtUrlFacebook.option("value", data.facebook);
                                        txtDireccion.option("value", data.direccion);
                                        cboMunicipio.option("text", data.municipio);
                                        txtTelefono.option("value", data.telefono1);
                                        txtOtroTelefono.option("value", data.telefono2);
                                        txtEMail.option("value", data.email);
                                        txtLogo.option("value", data.logo);
                                        cboCategoria.option("text", data.categoria);
                                        var OptEstado = data.estado;
                                        chkEstado.option("value", OptEstado);
                                        popup.show();
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
                        icon: '../Content/Images/Delete.png',
                        height: 20,
                        hint: 'Eliminar la Empresa',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar la Empresa : ' + options.data.S_RAZON_SOCIAL + '?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "DesarrolloEconomico/api/EmpresaApi/EliminarEmpresa";
                                    $.getJSON(_Ruta,
                                        {
                                            objData: options.data.ID
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar la Empresa');
                                            else {
                                                $('#GidListado').dxDataGrid({ dataSource: EmpresaDataSource });
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
                IdEmpresa = data.ID;
                $('#GidListadoProductos').dxDataGrid({ dataSource: ProductosDataSource });
               
            }
        }
    });

    $("#GidListadoProductos").dxDataGrid({
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
            { dataField: 'N_VALOR_UNIDAD', width: '30%', caption: 'Valor Unitario', dataType: 'string' },
            {
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: '../Content/Images/edit.png',
                        height: 20,
                        hint: 'Editar la información del Producto',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "DesarrolloEconomico/api/EmpresaApi/ObtenerProducto";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.ID
                                }).done(function (data) {
                                    if (data !== null) {
                                        IdProducto = parseInt(data.id);
                                        txtProducto.option("value", data.nombreProducto);
                                        cboUnidadMed.option("text", data.unidadMed);
                                        txtValorUnitario.option("value", data.valorUnitario);
                                        txtUrlImagen.option("value", data.urlImagen);
                                        txtDescripcionProducto.option("value", data.descripcionProducto);
                                        var OptEstado = data.activo;
                                        chkEstadoProducto.option("value", OptEstado);
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
                        icon: '../Content/Images/Delete.png',
                        height: 20,
                        hint: 'Eliminar el Producto',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar el Producto : ' + options.data.S_NOMBRE + '?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "DesarrolloEconomico/api/EmpresaApi/EliminarProducto";
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
        ]
    });

    var cboUnidadMed = $("#cboUnidadMed").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "DesarrolloEconomico/api/EmpresaApi/GetUnidadesMedida");
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
                    return $.getJSON($("#SIM").data("url") + "DesarrolloEconomico/api/EmpresaApi/GetCategorias");
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

    var cboMunicipio = $("#cboMunicipio").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "DesarrolloEconomico/api/EmpresaApi/GetMunicipios");
                }
            })
        }),
        displayExpr: "Nombre",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El Municipio al que pertenece la empresa es obligatorio!"
        }]
    }).dxSelectBox("instance");

    var txtNit = $("#txtNit").dxTextBox({
        placeholder: "Ingrese el Nit (Sin dígito de verificación)",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El Nit es requerido!"
        }]
    }).dxTextBox("instance");

    var txtRazonSocial = $("#txtRazonSocial").dxTextBox({
        placeholder: "Ingrese la Razón Social...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "La Razón Social es obligatoria!"
        }]
    }).dxTextBox("instance");

    var txtDescripcion = $("#txtDescripcion").dxTextArea({
        placeholder: "Ingrese la Descripción de la enpresa, sus productos y/o servicios...",
        value: "",
    }).dxTextArea("instance");

    var txtUrlWeb = $("#txtUrlWeb").dxTextBox({
        placeholder: "Ingrese la dirección del sitio web ...",
        value: "",
     }).dxTextBox("instance");

    var txtUrlInstagram = $("#txtUrlInstagram").dxTextBox({
        placeholder: "Ingrese la dirección de Instagram ...",
        value: "",
    }).dxTextBox("instance");

    var txtUrlFacebook = $("#txtUrlFacebook").dxTextBox({
        placeholder: "Ingrese la dirección de Facebook ...",
        value: "",
    }).dxTextBox("instance");

    var txtDireccion = $("#txtDireccion").dxTextBox({
        placeholder: "Ingrese la dirección de la empresa ...",
        value: "",
    }).dxTextBox("instance");

    var txtTelefono = $("#txtTelefono").dxTextBox({
        placeholder: "Ingrese el teléfono ...",
        value: "",
    }).dxTextBox("instance");

    var txtOtroTelefono = $("#txtOtroTelefono").dxTextBox({
        value: "",
    }).dxTextBox("instance");

    var txtEMail = $("#txtEMail").dxTextBox({
        placeholder: "Ingrese la dirección de correo electrónico ...",
        value: "",
    }).dxTextBox("instance");

    var txtLogo = $("#txtLogo").dxTextBox({
        placeholder: "Seleccione el Logotipo de la empresa...",
        value: "",
    }).dxTextBox("instance");

    var txtFoto = $("#txtFoto").dxTextBox({
        value: "",
    }).dxTextBox("instance");

    var chkEstado = $("#chkEstado").dxCheckBox({
        value: false,
        width: 80,
        text: "Si",

    }).dxCheckBox("instance");

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

   
    //var txtValorUnitario = $("#txtValorUnitario").dxTextBox({
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

    var txtUrlImagen = $("#txtUrlImagen").dxTextBox({
        value: "",
    }).dxTextBox("instance");

    var txtDescripcionProducto = $("#txtDescripcionProducto").dxTextArea({
        height: 150,
        placeholder: "Ingrese la descripción del producto...",
        value: "",
    }).dxTextArea("instance");

    var chkEstadoProducto = $("#chkEstadoProducto").dxCheckBox({
        value: false,
        width: 80,
        text: "Si",

    }).dxCheckBox("instance");


    $("#btnGuarda").dxButton({
        text: "Guardar",
        type: "default",
        height: 30,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var id = IdRegistro;
            var idTercero = 1;
            var razonSocial = txtRazonSocial.option("value");
            var descripcion = txtDescripcion.option("value");
            var direccion = txtDireccion.option("value");
            var email = txtEMail.option("value");
            var nit = txtNit.option("value"); 
            var municipio = cboMunicipio.option("value").Id;
            var categoria = cboCategoria.option("value").Id;
            var instagram = txtUrlInstagram.option("value");
            var facebook = txtUrlFacebook.option("value");
            var web = txtUrlWeb.option("value");
            var foto = txtFoto.option("value");
            var logo = txtLogo.option("value");
            var telefono1 = txtTelefono.option("value");
            var telefono2 = txtOtroTelefono.option("value");
            var estado = chkEstado.option("value");
            var params = { id: id, idTercero: idTercero, razonSocial: razonSocial, descripcion: descripcion, direccion: direccion, email: email, nit: nit, municipio: municipio, categoria: categoria, instagram: instagram, facebook: facebook, web: web, foto: foto, logo: logo, telefono1: telefono1, telefono2: telefono2, activo: estado };
            var _Ruta = $('#SIM').data('url') + "DesarrolloEconomico/api/EmpresaApi/GuardarEmpresa";
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
                        $('#GidListado').dxDataGrid({ dataSource: EmpresaDataSource });
                        $("#PopupNuevaEmpresa").dxPopup("instance").hide();
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
        type: "default",
        height: 30,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var id = IdProducto;
            var nombreProducto = txtProducto.option("value");
            var descripcionProducto = txtDescripcionProducto.option("value");
            var urlImagen = txtUrlImagen.option("value");
            var valorUnitario = txtValorUnitario.option("value");
            var unidadMed = cboUnidadMed.option("value").Id;
            var activo = chkEstadoProducto.option("value");

            var params = { id: id, terceroId: IdEmpresa, nombreProducto: nombreProducto, descripcionProducto: descripcionProducto, urlImagen: urlImagen, valorUnitario: valorUnitario, unidadMed: unidadMed, activo: activo };
            var _Ruta = $('#SIM').data('url') + "DesarrolloEconomico/api/EmpresaApi/GuardarProducto";
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

    var popup = $("#PopupNuevaEmpresa").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Empresa"
    }).dxPopup("instance");

    var popupP = $("#PopupNuevoProducto").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Producto"
    }).dxPopup("instance");

    $("#btnNuevo").dxButton({
        stylingMode: "contained",
        text: "Nuevo",
        type: "success",
        width: 200,
        height: 30,
        icon: '../Content/Images/new.png',
        onClick: function () {
            IdRegistro = -1;
            txtRazonSocial.reset();
            txtDescripcion.reset();
            txtDireccion.reset();
            txtEMail.reset();
            txtNit.reset();
            txtUrlInstagram.reset();
            txtUrlFacebook.reset();
            txtUrlWeb.reset();
            txtFoto.reset();
            txtLogo.reset();
            txtTelefono.reset();
            txtOtroTelefono.reset();
            chkEstado.option("value", false);
            popup.show();
        }
    });

    $("#btnNuevoProducto").dxButton({
        stylingMode: "contained",
        text: "Nuevo",
        type: "success",
        width: 200,
        height: 30,
        icon: '../Content/Images/new.png',
        onClick: function () {
            if (IdEmpresa == -1) {
                DevExpress.ui.dialog.alert('Debe seleccionar una Empresa en el Grid superior para poder crear un Producto', 'Nuevo Producto');
            } else {
                IdProducto = -1;
                txtProducto.reset();
                txtDescripcionProducto.reset();
                txtUrlImagen.reset();
                txtValorUnitario.reset();
                chkEstadoProducto.option("value", false);
                popupP.show();
            }
        }
    });

});

var EmpresaDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"S_RAZON_SOCIAL","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'DesarrolloEconomico/api/EmpresaApi/GetEmpresas', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: false,
            CodFuncionario: CodigoFuncionario
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var ProductosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"S_NOMBRE","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'DesarrolloEconomico/api/EmpresaApi/GetProductos', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: false,
            Id: IdEmpresa
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});