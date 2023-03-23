var IdTRD = -1;

$(document).ready(function () {
    var IdRegistro = -1;

    $('#asistente').accordion();

    //Tablas de Retención Documental
    $("#GidListadoTRD").dxDataGrid({
        dataSource: TablasRetencionDocDataSource,
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
            mode: 'single'
        },
        hoverStateEnabled: true,
        remoteOperations: true,
        columns: [
            { dataField: 'COD_VIGENCIA_TRD', width: '10%', caption: 'Código', alignment: 'center' },
            { dataField: 'S_NOMBRE', width: '40%', caption: 'Nombre de la Tabla de Retención', dataType: 'string' },
            { dataField: 'S_DESCRIPCION', width: '30%', caption: 'Descripción', dataType: 'string' },
            { dataField: 'D_INICIOVIGENCIA', width: '10%', caption: 'Vigente desde', dataType: 'date' },
            {
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: '../Content/Images/edit.png',
                        height: 20,
                        hint: 'Editar la información de la Tabla de Retención Documental',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "AdministracionDocumental/api/AdminDocumentalAPI/ObtenerTablaRetencionDocumental";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.COD_VIGENCIA_TRD
                                }).done(function (data) {
                                    if (data !== null) {
                                        IdRegistro = parseInt(data.id);
                                        IdTRD = parseInt(data.id); 
                                        txtNombre.option("value", data.nombre);
                                        txtDescripcion.option("value", data.descripcion);
                                        var FecInicia = data.vigenteDesde != null ? new Date(data.vigenteDesde) : "";
                                        dpVigenteDesde.option("value", FecInicia);
                                        popupTRD.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
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
                        hint: 'Eliminar la Tabla de Retención Documental',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar la Tabla de Retención Documental : ' + options.data.NOMBRE + '?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "AdministracionDocumental/api/AdminDocumentalAPI/EliminarTablaRetencionDocumental";
                                    $.getJSON(_Ruta,
                                        {
                                            objData: options.data.COD_VIGENCIA_TRD
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar la Tabla de Retención Documental');
                                            else {
                                                $('#GidListado').dxDataGrid({ dataSource: SerieDocumentalDataSource });
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar la Tabla de Retención Documental');
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
                IdTRD = data.COD_VIGENCIA_TRD;
                IdRegistro = data.COD_VIGENCIA_TRD;
                $('#GidListadoUnidadesTRD').dxDataGrid({ dataSource: UnidadesTablaRetDataSource });
                $('#lblTRD').text(data.S_NOMBRE);
            }
        }
    });

    var txtNombre = $("#txtNombre").dxTextBox({
        placeholder: "Ingrese el nombre de la Tabla de Retención Documental...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El nombre de la Serie Documental es requerido!"
        }]
    }).dxTextBox("instance");

    var txtDescripcion = $("#txtDescripcion").dxTextArea({
        value: "",
        height: 90
    }).dxTextArea("instance");


    var dpVigenteDesde = $("#dpVigenteDesde").dxDateBox({
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
            message: "La fecha de inicio de la vigencia de la TRD es obligatoria"
        }]
    }).dxDateBox("instance");

    $("#btnGuardarTRD").dxButton({
        text: "Guardar",
        type: "default",
        height: 30,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var id = IdTRD;
            var nombre = txtNombre.option("value");
            var descripcion = txtDescripcion.option("value");
            var vigenteDesde = dpVigenteDesde.option("value");
            var params = { id: id, nombre: nombre, descripcion: descripcion, vigenteDesde: vigenteDesde };
            var _Ruta = $('#SIM').data('url') + "AdministracionDocumental/api/AdminDocumentalAPI/GuardarTablaRetencion";
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
                        $('#GidListadoTRD').dxDataGrid({ dataSource: TablasRetencionDocDataSource });
                        $("#PopupNuevaTablaRet").dxPopup("instance").hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    var popupTRD = $("#PopupNuevaTablaRet").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Tabla de Retención Documental"
    }).dxPopup("instance");

    $("#btnNuevaTRD").dxButton({
        stylingMode: "contained",
        text: "Nuevo",
        type: "success",
        width: 200,
        height: 30,
        icon: '../Content/Images/new.png',
        onClick: function () {
            IdRegistro = -1;
            txtNombre.reset();
            txtDescripcion.reset();
            dpVigenteDesde.option("value", new Date());
            popupTRD.show();
        }
    });


    //Unidades por Tabla de Retención Documental
    $("#GidListadoUnidadesTRD").dxDataGrid({
        dataSource: UnidadesTablaRetDataSource,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            pageSize: 50
        },
        pager: {
            showPageSizeSelector: false,
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
            { dataField: 'unidadId', width: '10%', caption: 'Código', alignment: 'center' },
            { dataField: 'unidadDocumental', width: '40%', caption: 'Unidad Documental', dataType: 'string' },
            {
                dataField: 'asignada', width: '10%', alignment: 'center', caption: 'Asignada', dataType: 'string', customizeText: function (cellInfo) {
                    if (cellInfo.value == '1') {
                        return 'Si';
                    }
                    else {
                        return 'No';
                    }
                }},
            {
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: '../Content/Images/edit.png',
                        height: 20,
                        hint: 'Editar la información de la Unidad Documental',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "AdministracionDocumental/api/AdminDocumentalAPI/CambiarEstado";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.unidadId,
                                    IdTR: IdTRD
                                }).done(function (data) {
                                    if (data !== null) {
                                        $('#GidListadoUnidadesTRD').dxDataGrid({ dataSource: UnidadesTablaRetDataSource });
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'al Editar el Metadato');
                                });
                        }
                    }).appendTo(container);
                }
            },
           
        ]
    });


});


var TablasRetencionDocDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"S_NOMBRE","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'AdministracionDocumental/api/AdminDocumentalAPI/GetTablasRetencionDocumental', {
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

var UnidadesTablaRetDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"UnidadDocumental","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'AdministracionDocumental/api/AdminDocumentalAPI/GetUnidadesTablasRetencionDocumental', {
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
            Id: IdTRD
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});