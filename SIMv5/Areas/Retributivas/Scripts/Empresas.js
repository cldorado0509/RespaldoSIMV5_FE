var IdTercero = 310993;

var urrl = "api/EmpresasApi";
var _option



$(document).ready(function () {



    $("#tabs").tabs;

    $("#tabsProductos").tabs;

    $("#generalidades").submit(function () {
    });


    //$('#start_time').datetimepicker({
    //    format: 'HH:mm:ss'
    //});

    //$('#end_time').datetimepicker({
    //    format: 'HH:mm:ss'
    //});

    // *************************************** TAB  Raw Material *************************************
    // *************************************** TAB  Raw Material *************************************
    // *************************************** TAB  Raw Material *************************************



    var txtMaterialDescribe = $("#txtMaterialDescribe").dxTextBox({
        placeholder: "Ingrese aqui la descripcion de olos materiales o insumos...",
        value: ""
    }).dxTextBox("instance");

    var txtMaterialName = $("#txtMaterialName").dxTextBox({
        placeholder: "Ingrese aqui el nombre del Material o Insumo utilizado...",
        value: ""
    }).dxTextBox("instance");

    $("#btnRawMaterial").dxButton({
        stylingMode: "contained",
        text: "Guardar",
        type: "default",
        width: 120,
        onClick: function () {
            DevExpress.ui.notify("The Contained button was clicked");
            var id = MaterialId;
            var describe = txtMaterialDescribe.option("value");
            var name = txtMaterialName.option("value");
            var params = {
                ID: id,
                NOMBRE: name,
                DESCRIPCION: describe,
            };
            var _Ruta = $('#SIM').data('url') + "Retributivas/api/EmpresasApi/InsertMaterial";
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
                        $('#grdRawMaterials').dxDataGrid({ dataSource: myMaterials });
                        popupRawMaterial.hide();
                        //$("#PopupNuevaEmpresa").dxPopup("instance").hide();

                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    var btnAddRawMaterial = $("#btnAddRawMaterial").dxButton({
        icon: 'plus',
        hint: 'ingresar Regsitro de las materias primasa e insumos',
        onClick: function (e) {
            MaterialId = null;
            txtMaterialName.option("value", null);
            txtMaterialDescribe.option("value", null);
            option = true;
            //ProductId.option("visible", false);
            popupRawMaterial.show();
        }
    });

    $("#grdRawMaterials").dxDataGrid({
        dataSource: myMaterials,
        repaintChangesOnly: true,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            pageSize: 10
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

        columns: [
            { dataField: "ID", width: '10%', caption: 'ID', dataType: 'Number', visible: true },
            { dataField: 'NOMBRE', width: '20%', caption: 'Material o Insumo' },
            { dataField: 'DESCRIPCION', width: '50%', caption: 'Descripción' },

            {
                alignment: 'center', caption: 'Funciones',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        hint: 'Editar Regsitro de Producción',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "Retributivas/api/EmpresasApi/loadMaterial";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.ID
                                }).done(function (data) {
                                    if (data !== null) {
                                        MaterialId = parseInt(data.ID);
                                        txtMaterialName.option("value", data.NOMBRE);
                                        txtMaterialDescribe.option("value", data.DESCRIPCION);
                                        option = false;
                                        popupRawMaterial.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');

                                });
                        }
                    }).appendTo(container);

                    $('<div/>').dxButton({
                        icon: 'remove',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar éste registro?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "Retributivas/api/EmpresasApi/RemoveMaterial";
                                    $.getJSON(_Ruta,
                                        {
                                            Id: options.data.ID
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar la Produccion');
                                            else {
                                                $('#grdRawMaterials').dxDataGrid({ dataSource: myMaterials });
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

    var popupRawMaterial = $("#popupRawMaterial").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Materia prima e Insumos"
    }).dxPopup("instance");

    // *************************************** TAB  Product *************************************
    // *************************************** TAB  Product *************************************
    // *************************************** TAB  Product *************************************


    var txtProductDescribe = $("#txtProductDescribe").dxTextBox({
        placeholder: "Ingrese aqui la descripcion del producto...",
        value: ""
    }).dxTextBox("instance");

    var txtProductName = $("#txtProductName").dxTextBox({
        placeholder: "Ingrese aqui el nombre del producto...",
        value: ""
    }).dxTextBox("instance");

    $("#btnProducts").dxButton({
        stylingMode: "contained",
        text: "Guardar",
        type: "default",
        width: 120,
        onClick: function () {
            DevExpress.ui.notify("The Contained button was clicked");
            var id = ProductId;
            var describe = txtProductDescribe.option("value");
            var name= txtProductName.option("value");
            var params = {
                ID: id,
                NOMBRE: name,
                DESCRIPCION: describe,
            };
            var _Ruta = $('#SIM').data('url') + "Retributivas/api/EmpresasApi/InsertProduct";
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
                        $('#grdProducts').dxDataGrid({ dataSource: myProducts });
                        popupProducts.hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    var btnAddProducts = $("#btnAddProducts").dxButton({
        icon: 'plus',
        hint: 'ingresar Regsitro de Producción',
        onClick: function (e) {
            ProductId = null;
            txtProductName.option("value", null);
            txtProductDescribe.option("value", null);
            option = true;
            popupProducts.show();
        }
    });

    $("#grdProducts").dxDataGrid({
        dataSource: myProducts,
        repaintChangesOnly: true,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            pageSize: 10
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

        columns: [
            { dataField: "ID", width: '10%', caption: 'ID', dataType: 'Number', visible: true },
            { dataField: 'Nombre', width: '20%', caption: 'Producto'},
            { dataField: 'Descripcion', width: '50%', caption: 'Descripción' },
    
            {
                alignment: 'center', caption: 'Funciones',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        hint: 'Editar Regsitro de Producción',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "Retributivas/api/EmpresasApi/loadProduct";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.ID
                                }).done(function (data) {
                                    if (data !== null) {
                                        ProductId = parseInt(data.ID);
                                        txtProductName.option("value", data.Nombre);
                                        txtProductDescribe.option("value", data.Descripcion);
                                        option = false;
                                        popupProducts.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');

                                });
                        }
                    }).appendTo(container);

                    $('<div/>').dxButton({
                        icon: 'remove',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar éste registro?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "Retributivas/api/EmpresasApi/RemoveProduct";
                                    $.getJSON(_Ruta,
                                        {
                                            Id: options.data.ID
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar la Produccion');
                                            else {
                                                $('#grdProducts').dxDataGrid({ dataSource: myProducts });
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

    var popupProducts = $("#popupProducts").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Productos"
    }).dxPopup("instance");

    // *************************************** TAB  Production *************************************
    // *************************************** TAB  Production *************************************
    // *************************************** TAB  Production *************************************



    var txtMonthly = $("#txtMount").dxTextBox({
        value: ""
    }).dxTextBox("instance");

    var txtDaily = $("#txtDaily").dxTextBox({
        value: ""
    }).dxTextBox("instance");

    var cmbUnits = $("#cmbUnits").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "IdUnits",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Retributivas/api/EmpresasApi/UnitsOfMeasurement");
                }
            })
        }),
        displayExpr: "Nombre",
        searchEnabled: true
    }).dxSelectBox("instance");

    var cmbProducts = $("#cmbProducts").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "IdProducts",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Retributivas/api/EmpresasApi/Products");
                }
            })
        }),
        displayExpr: "Nombre",
        searchEnabled: true
    }).dxSelectBox("instance");

    $("#btnProduction").dxButton({
        stylingMode: "contained",
        text: "Guardar",
        type: "default",
        width: 120,
        onClick: function () {
            DevExpress.ui.notify("The Contained button was clicked");
            var id = IdRegistro;
            var idTercero = IdTerceroPro;
            var monthly = txtMonthly.option("value");
            var daily = txtDaily.option("value");

            if (option) {
                var products = cmbProducts.option("value").IdProducts;
                var units = cmbUnits.option("value").IdUnits;
                option = null;
            } else {
                var products = cmbProducts.option("value");
                var units = cmbUnits.option("value");
                option = null;
            }

            var params = {
                ID: id,
                ID_TERCERO: idTercero,
                TSIMTASA_PRODUCTOS_ID: products,
                TSIMTASA_UNIDADES_ID: units,
                DIARIO: daily,
                MENSUAL: monthly
            };
            var _Ruta = $('#SIM').data('url') + "Retributivas/api/EmpresasApi/InsertProduction";
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
                        $('#grdProduction').dxDataGrid({ dataSource: myProduction });
                        popupProduction.hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    var btnAddProduction = $("#btnAddProduction").dxButton({
        icon: 'plus',
        hint: 'ingresar Regsitro de Producción',
        onClick: function (e) {
                IdRegistro = null;
            IdTerceroPro = IdTercero;
                cmbProducts.option("value", 0);
                cmbUnits.option("value", 0);
                txtMonthly.option("value", "");
                txtDaily.option("value", "");
                option = true;
                popupProduction.show();
             }
    });


    $("#grdProduction").dxDataGrid({
        dataSource: myProduction,
        repaintChangesOnly: true,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            pageSize: 10
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

        columns: [
            { dataField: "ID", width: '5%', caption: 'ID', dataType: 'Number', visible: true },
            { dataField: 'NameProducts', width: '20%', caption: 'Producto'},
            { dataField: 'NameUnits', width: '15%', caption: 'Unidades' },
            { dataField: 'DIARIO', width: '15%', caption: 'Cantidad Diaria', dataType: 'Number' },
            { dataField: 'MENSUAL', width: '15%', caption: 'Cantidad Mensual', dataType: 'Number' },
            { dataField: 'ID_TERCERO', width: '15%', caption: 'Empresa', dataType: 'Number' },
            {
                alignment: 'center', caption: 'Funciones',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        hint: 'Editar Regsitro de Producción',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "Retributivas/api/EmpresasApi/loadProduction";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.ID
                                }).done(function (data) {
                                    if (data !== null) {
                                        IdRegistro = parseInt(data.Id);
                                        IdTerceroPro = parseInt(data.IdTercero);
                                        cmbProducts.option("value", data.ProductosId);
                                        cmbUnits.option("value", data.UnidadesId);
                                        txtMonthly.option("value", data.Mensual);
                                        txtDaily.option("value", data.Diario);
                                        option = false;
                                        popupProduction.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');

                                });
                        }
                    }).appendTo(container);

                    $('<div/>').dxButton({
                            icon: 'remove',
                            onClick: function (e) {
                                var result = DevExpress.ui.dialog.confirm('Desea eliminar éste registro?', 'Confirmación');
                                result.done(function (dialogResult) {
                                    if (dialogResult) {
                                        var _Ruta = $('#SIM').data('url') + "Retributivas/api/EmpresasApi/RemoveProduction";
                                        $.getJSON(_Ruta,
                                            {
                                                Id: options.data.ID
                                            }).done(function (data) {
                                                if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar la Produccion');
                                                else {
                                                    $('#grdProduction').dxDataGrid({ dataSource: myProduction });
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


    var popupProduction = $("#popupProduccion").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Empresa"
    }).dxPopup("instance");

    $("#MisUnidades").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Retributivas/api/EmpresasApi/UnitsOfMeasurement");
                }
            })
        }),
        displayExpr: "Nombre",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "formGroup",
        validationRules: [{
            type: "required",
            message: "Las unidades del producto son requeridas"
        }]
    }).dxSelectBox("instance");

    $("#txtProducto").dxTextBox({
        placeholder: "Enter full name here...",
        value: ""
    });

    $("#numProduccion").dxNumberBox({

    });

    $("#areaDescripcion").dxTextArea({
        maxLength: 250,
    })

    $("#btnProductos").dxButton({
        stylingMode: "contained",
        text: "Enviar",
        type: "default",
        width: 120,
        onClick: function () {
            DevExpress.ui.notify("The Contained button was clicked");
        }
    });

    // *************************************** TAB CONSUMOS *************************************
    // *************************************** TAB CONSUMOS *************************************
    // *************************************** TAB CONSUMOS *************************************
    // *************************************** TAB CONSUMOS *************************************

    var cmbRawMaterials = $("#cmbRawMaterials").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "IdMaterials",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Retributivas/api/EmpresasApi/Materials");
                }
            })
        }),
        displayExpr: "Nombre",
        searchEnabled: true
    }).dxSelectBox("instance");

    var cmbUnitsMaterial = $("#cmbUnitsMaterial").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "IdUnits",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Retributivas/api/EmpresasApi/UnitsOfMeasurement");
                }
            })
        }),
        displayExpr: "Nombre",
        searchEnabled: true
    }).dxSelectBox("instance");

    var txtMountMaterials  = $("#txtMountMaterials ").dxTextBox({
        value: ""
    }).dxTextBox("instance");

    var txtDailyMaterials = $("#txtDailyMaterials").dxTextBox({
        value: ""
    }).dxTextBox("instance");

    $("#btnMaterials").dxButton({
        stylingMode: "contained",
        text: "Guardar",
        type: "default",
        width: 120,
        onClick: function () {
            DevExpress.ui.notify("The Contained button was clicked");
            var id = IdRegistroMaterials;
            var idTercero = IdTerceroPro;
            var monthly = txtMountMaterials.option("value");
            var daily = txtDailyMaterials.option("value");

            if (_option) {
                var materials = cmbRawMaterials.option("value").IdMaterials;
                var units = cmbUnitsMaterial.option("value").IdUnits;
            } else {
                var materials = cmbRawMaterials.option("value").IdMaterials;
                var units = cmbUnitsMaterial.option("value");
            }

            var params = {
                ID: id,
                ID_TERCERO: idTercero,
                TSIMTASA_MATERIAS_PRIMA_ID: materials,
                TSIMTASA_UNIDADES_ID: units,
                DIARIO: daily,
                MENSUAL: monthly
            };
            var _Ruta = $('#SIM').data('url') + "Retributivas/api/EmpresasApi/InsertConsumptions";
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
                        $('#grdMaterials').dxDataGrid({ dataSource: misMateriales });
                        popupMaterials.hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    var btnAddMaterials = $("#btnAddMaterials").dxButton({
        icon: 'plus',
        hint: 'ingresar Regsitro de Producción',
        onClick: function (e) {
            IdRegistroMaterials = null;
            IdTerceroPro = IdTercero;
            cmbRawMaterials.option("value", 0);
            cmbUnitsMaterial.option("value", 0);
            txtMountMaterials.option("value", "");
            txtDailyMaterials.option("value", "");
            _option = true;
            popupMaterials.show();
        }
    });


    $("#grdMaterials").dxDataGrid({

        dataSource: misMateriales,
        repaintChangesOnly: true,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            pageSize: 10
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
        columns: [
            { dataField: "ID", width: '5%', caption: 'ID', dataType: 'number' },
            { dataField: 'MaterialsName', width: '25%', caption: 'Materia Prima', dataType: 'number' },
            { dataField: 'MENSUAL', width: '15%', caption: 'Cantidad Mensual', dataType: 'string' },
            { dataField: 'DIARIO', width: '15%', caption: 'Cantidad Diaria', dataType: 'string' },
            { dataField: 'UnitsName', width: '20%', caption: 'Unidades', dataType: 'string' },
            {
                alignment: 'center', caption: 'Funciones',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        hint: 'Editar Regsitro de Producción',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "Retributivas/api/EmpresasApi/loadConsumption";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.ID
                                }).done(function (data) {
                                    if (data !== null) {
                                        IdRegistroMaterials = parseInt(data.Id);
                                        IdTerceroPro = parseInt(data.IdTercero);
                                        cmbRawMaterials.option("value", data.MaterialsId);
                                        cmbUnitsMaterial.option("value", data.UnitsId);
                                        txtMountMaterials.option("value", data.Mount);
                                        txtDailyMaterials.option("value", data.Daily);
                                        _option = false;
                                        popupMaterials.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');

                                });
                        }
                    }).appendTo(container);

                    $('<div/>').dxButton({
                        icon: 'remove',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar éste registro?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "Retributivas/api/EmpresasApi/RemoveConsumption";
                                    $.getJSON(_Ruta,
                                        {
                                            Id: options.data.ID
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar la Produccion');
                                            else {
                                                $('#grdMaterials').dxDataGrid({ dataSource: misMateriales });
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar la Empresa');
                                        });
                                }
                            });
                        }
                    }).appendTo(container);

                }

            }        ]
    });

    var popupMaterials = $("#popupMaterials").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Empresa"
    }).dxPopup("instance");

    $("#unidMateriales").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Id",
                /*           loadMode: "raw",*/
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Retributivas/api/EmpresasApi/UnitsOfMeasurement");
                }
            })
        }),
        displayExpr: "Nombre",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "formGroup",
        validationRules: [{
            type: "required",
            message: "Las unidades del producto son requeridas"
        }]
    }).dxSelectBox("instance");

    $("#txtMateriales").dxTextBox({
        placeholder: "Enter full name here...",
        value: ""
    });

    $("#numConsumo").dxNumberBox({

    });

    $("#descMateriales").dxTextArea({
        maxLength: 250,
    })

    $("#btnMateriales").dxButton({
        stylingMode: "contained",
        text: "Enviar",
        type: "default",
        width: 120,
        onClick: function () {
            DevExpress.ui.notify("The Contained button was clicked");
        }
    });

    /****************************************************************/
    
    //  ||||||||||||||  |||         |||   ||||||||||   |||||      |||  
    //        |||       |||         |||   |||     |||  ||| ||     |||     
    //        |||       |||         |||   |||     |||  |||  ||    |||    
    //        |||       |||         |||   |||     |||  |||   ||   |||  
    //        |||       |||         |||   ||||||||||   |||    ||  |||  
    //        |||       |||         |||   ||||||       |||     || |||       
    //        |||       |||         |||   ||| ||||     |||      |||||  
    //        |||       |||         |||   |||  ||||    |||       ||||  
    //        |||        |||||||||||||    |||   ||||   |||        |||    

    /****************************************************************/

    var txtTurnName = $("#txtTurnName").dxTextBox({
        placeholder: "Nombre o denominacion del turno...",
        value: ""
    }).dxTextBox("instance");

    var txtTurnDescription = $("#txtTurnDescription").dxTextArea({
        placeholder: "Realice una breve descripcion del turno laboral...",
        maxLength: 250,
    }).dxTextArea("instance");

    var intAmountWorker = $("#intAmountWorker").dxNumberBox({
        placeholder: "Numero de trabajadores",
        value: ""
    }).dxNumberBox("instance");

    var DateStartTurn = $("#DateStartTurn").dxDateBox({
        type: "time",
        showClearButton: true,
        value: new Date(2015, 11, 1, 6)
    }).dxDateBox("instance");

    var dateEndTurn = $("#dateEndTurn").dxDateBox({
        type: "time",
        showClearButton: true,
        value: new Date(2015, 11, 1, 6)
    }).dxDateBox("instance");

    $("#btnTurn").dxButton({
        stylingMode: "contained",
        text: "Enviar",
        type: "default",
        width: 120,
        onClick: function () {
            DevExpress.ui.notify("The Contained button was clicked");
            var _IdTurn = IdTurn;
            var _idTerceroTurn = IdTercero;
            var _txtTurnName = txtTurnName.option("value");;
            var _txtTurnDescription = txtTurnDescription.option("value");;
            var _intAmountWorker = intAmountWorker.option("value");;
            var _DateStartTurn = DateStartTurn.option("value");;
            var _dateEndTurn = dateEndTurn.option("value");;

            var params = {
                ID: _IdTurn,
                NOMBRE: _txtTurnName,
                DESCRIPCION: _txtTurnDescription,
                INICIA: _DateStartTurn,
                TERMINA: _dateEndTurn,
                NO_OPERARIOS: _intAmountWorker,
                ID_TERCERO:_idTerceroTurn
            };
            var _Ruta = $('#SIM').data('url') + "Retributivas/api/EmpresasApi/InsertTurn";
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
                        $('#grdTurns').dxDataGrid({ dataSource: myTurns });
                        popupTurns.hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    var btnAddTurn = $("#btnAddTurn").dxButton({
        icon: 'plus',
        hint: 'ingresar Turnos de Producción',
        onClick: function (e) {
            IdTurn = null;
            IdTerceroTurn = IdTercero;
            txtTurnName.option("value", "");
            txtTurnDescription.option("value", "");
            intAmountWorker.option("value", 0);
            DateStartTurn.option("value", "2021-09-16T02:00:00");
            dateEndTurn.option("value", "2021-09-16T02:00:00");
            _option = true;
            popupTurns.show();
        }
    });

    $("#grdTurns").dxDataGrid({
        dataSource: myTurns,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            pageSize: 10
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

        columns: [
            { dataField: 'ID_TERCERO', width: '0%', caption: 'Colaboradores', dataType: 'number', visible: false },
            { dataField: 'ID', width: '5%', caption: 'No.', dataType: 'number' },
            { dataField: 'NOMBRE', width: '15%', caption: 'Nombre', dataType: 'number' },
            { dataField: 'DESCRIPCION', width: '30%', caption: 'Descripcion', dataType: 'string' },
            { dataField: 'NO_OPERARIOS', width: '10%', caption: 'Colaboradores', dataType: 'number' },
            {
                dataField: 'INICIA', width: '15%', caption: 'Hora Inicio', dataType: 'datetime', format: "HH:mm",
                editorOptions: { // set DateBox options here
                    displayFormat: "HH:mm",
                    useMaskBehavior: true,
                    type: "time"
                }
            },
            {
                dataField: 'TERMINA', alignment: "right", width: '15%', caption: 'Hora Fin', dataType: 'datetime', format: "HH:mm",
                editorOptions: { // set DateBox options here
                    displayFormat: "HH:mm",
                    useMaskBehavior: true,
                    type: "time"
                }
            },
            {
                alignment: 'center', caption: 'Funciones',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        hint: 'Editar Regsitro de Producción',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "Retributivas/api/EmpresasApi/loadTurn";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.ID
                                }).done(function (data) {
                                    if (data !== null) {
                                        IdTurn = parseInt(data.ID);
                                        IdTerceroTurn = parseInt(data.ID_TERCERO);
                                        txtTurnName.option("value", data.NOMBRE);
                                        txtTurnDescription.option("value", data.DESCRIPCION);
                                        intAmountWorker.option("value", data.NO_OPERARIOS);
                                        DateStartTurn.option("value", data.INICIA);
                                        dateEndTurn.option("value", data.TERMINA);
                                        _option = false;
                                        popupTurns.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');

                                });
                        }
                    }).appendTo(container);

                    $('<div/>').dxButton({
                        icon: 'remove',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar éste registro?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "Retributivas/api/EmpresasApi/RemoveTurn";
                                    $.getJSON(_Ruta,
                                        {
                                            Id: options.data.ID
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar la Produccion');
                                            else {
                                                $('#grdTurns').dxDataGrid({ dataSource: myTurns });
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

    var popupTurns = $("#popupTurns").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Turnos de Trabajo"
    }).dxPopup("instance");

    // *************************************** TAB GENERALIDADES *************************************
    // *************************************** TAB GENERALIDADES *************************************
    // *************************************** TAB GENERALIDADES *************************************


 $("#formGeneralidades").dxForm({
     formData: myfactory,
     colCount: 2,
     items: [{
         itemType: "group",
         caption: "Informacion de la Empresa",
         items: ["NIT", "FirstName", "LastName", "HireDate", "Position", "OfficeNo"]
     }, {
         itemType: "group",
         caption: "Informacion de Contacto",
         items: ["BirthDate", {
             itemType: "group",
             caption: "Home Address",
             items: ["Address", "City", "State", "Zipcode"]
         }]
     }]
    });


    var txtNameEmpresa = $("#txtNameEmpresa").dxTextBox({
        displayExpr: "Nombre",

    }).dxTextBox("instance");

    $("#numberNit").dxTextBox({
        value: "890.890.890-8",
        disabled: false
    });

    $("#ciiu").dxTextBox({

    });

    var txtTipoEmpresa = $("#txtTipoEmpresa").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "IdType",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Retributivas/api/EmpresasApi/TypeFactory");
                }
            })
        }),
        displayExpr: "Nombre",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "formGroup",
        validationRules: [{
            type: "required",
            message: "El tipo de empresa es requerido"
        }]
    }).dxSelectBox("instance");

    $("#booleanESP").dxCheckBox({
        value: undefined
    }).dxCheckBox("instance");

    $("#txtRL").dxTextBox({
        value: "890.890.890-8",
        disabled: false
    });

    $("#txtCorreo").dxTextBox({
        value: "890.890.890-8",
        disabled: false
    });

    $("#txtDireccion").dxTextBox({
        value: "890.890.890-8",
        disabled: false
    });

    $("#txtTelefóno").dxTextBox({
        value: "890.890.890-8",
        disabled: false
    });

    $("#btnGeneralidades").dxButton({
        stylingMode: "contained",
        text: "Guardar",
        type: "default",
        width: 120,
        onClick: function () {
            DevExpress.ui.notify("Informacion de la empresa actualizada");
        }
    });



});

var myfactory = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        $.getJSON($("#SIM").data("url") + "Retributivas/api/EmpresasApi/factoryName", {
            Id: IdTercero
        }).done(function (data) {
            d.resolve(data.datos);
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var myProduction = new DevExpress.data.CustomStore({
    //key: "ID",

    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : "";
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'Retributivas/api/EmpresasApi/Production', {
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
            IdTercero: IdTercero
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    },
});

var myProducts = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : "";
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($("#SIM").data("url") + "Retributivas/api/EmpresasApi/Product", {
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
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    },

});

var myMaterials = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : "";
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($("#SIM").data("url") + "Retributivas/api/EmpresasApi/Material", {
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
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    },

});

var misMateriales = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : "";
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'Retributivas/api/EmpresasApi/MisMateriales', {
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
            IdTercero: IdTercero
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var myTurns = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : "";
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'Retributivas/api/EmpresasApi/Turns', {
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
            IdTercero: IdTercero
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var misUnidades = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        //var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        //var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : "";
        //var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        //var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        //var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'Retributivas/api/EmpresasApi/UnitsOfMeasurement', {
            //filter: loadOptions.filter ?
            //    JSON.stringify(filterOptions) : '',
            //sort: sortOptions,
            //group: JSON.stringify(groupOptions),
            //skip: skip,
            //take: take,
            //searchValue: '',
            //searchExpr: '',
            //comparation: '',
            //tipoData: 'f',
            //noFilterNoRecords: false,
            //IdTercero: IdTercero
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

function isNotEmpty(value) {
    return value !== undefined && value !== null && value !== "";
}


function updateEmpresa(tipo_Empresa, ciuu) {
    document.getElementById("btn_Empresa").disabled = true;

    $.ajax({
        cache: false,
        url: '<%= Url.Action("Welcome", "Home") %>',
        data: {
            tipo_Empresa: tipo_Empresa,
            CIUD: ciuu
        },
        success: function (msg) {
            $("#result").html(msg).show("slow");
        },
        error: function (msg) {
            $("#result").html("Bad parameters!").show("slow");
        }
    });

    //setTimeout(function () {
    //    document.getElementById("btn_Empresa").disabled = false;
    //    $("#result").hide("slow");
    //}, 3000);
}