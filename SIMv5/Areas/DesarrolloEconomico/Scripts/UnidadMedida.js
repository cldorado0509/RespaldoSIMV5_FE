$(document).ready(function () {
    var IdRegistro = -1;
  
    $("#GidListado").dxDataGrid({
        dataSource: UnidadMedidaDataSource,
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
            { dataField: 'S_NOMBRE', width: '30%', caption: 'Nombre de la Unidad de Medida', dataType: 'string' },
            { dataField: 'S_DESCRIPCION', width: '40%', caption: 'Descripción', dataType: 'string' },
            { dataField: 'B_ACTIVO', width: '7%', caption: 'Habilitado', dataType: 'string' },
            {
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                            icon: '../Content/Images/edit.png',
                            height: 20,
                            hint: 'Editar la Unidad de Medida',
                            onClick: function (e) {
                                var _Ruta = $('#SIM').data('url') + "DesarrolloEconomico/api/UnidadMedidaApi/ObtenerUnidad";
                                $.getJSON(_Ruta,
                                    {
                                        Id: options.data.ID
                                    }).done(function (data) {
                                        if (data !== null) {
                                            IdRegistro = parseInt(data.id);
                                            txtNombre.option("value", data.nombre);
                                            txtDescripcion.option("value", data.descripcion);
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
                            hint: 'Eliminar la Unidad',
                            onClick: function (e) {
                                var result = DevExpress.ui.dialog.confirm('Desea eliminar la Unidad de Medida : ' + options.data.S_NOMBRE + '?', 'Confirmación');
                                result.done(function (dialogResult) {
                                    if (dialogResult) {
                                        var _Ruta = $('#SIM').data('url') + "DesarrolloEconomico/api/UnidadMedidaApi/EliminarUnidad";
                                        $.getJSON(_Ruta,
                                            {
                                                objData: options.data.ID
                                            }).done(function (data) {
                                                if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar Unidad de Medida');
                                                else {
                                                    $('#GidListado').dxDataGrid({ dataSource: UnidadMedidaDataSource });
                                                }
                                            }).fail(function (jqxhr, textStatus, error) {
                                                DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar la Unidad de Medida');
                                            });
                                    }
                                });
                            }
                        }).appendTo(container);
                }
            }
        ]
    });

   
    var txtNombre = $("#txtNombre").dxTextBox({
        placeholder: "Ingrese el nombre de la Unidad de Medida...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El nombre de la Unidad de Medida es obligatorio!"
        }]
    }).dxTextBox("instance");

    var txtDescripcion = $("#txtDescripcion").dxTextArea({
        value: "",
        height: 90
    }).dxTextArea("instance");

    var chkEstado = $("#chkEstado").dxCheckBox({
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
            var nombre = txtNombre.option("value");
            var descripcion = txtDescripcion.option("value");
            var estado = chkEstado.option("value");
            var params = {id: id, nombre: nombre, descripcion: descripcion, estado: estado};
            var _Ruta = $('#SIM').data('url') + "DesarrolloEconomico/api/UnidadMedidaApi/GuardarUnidad";
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
                            $('#GidListado').dxDataGrid({ dataSource: UnidadMedidaDataSource });
                            $("#PopupNuevaUnidad").dxPopup("instance").hide();
                        }
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                    }
                });
         }
    });

    var popup = $("#PopupNuevaUnidad").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Unidad de Medida"
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
            txtNombre.reset();
            txtDescripcion.reset();
            chkEstado.option("value", false);
            popup.show();
        }
    });
});

var UnidadMedidaDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"S_NOMBRE","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'DesarrolloEconomico/api/UnidadMedidaApi/GetUnidadesMedida', {
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