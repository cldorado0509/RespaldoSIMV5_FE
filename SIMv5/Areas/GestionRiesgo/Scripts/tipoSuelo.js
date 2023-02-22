var IdRegistro = -1;

$(document).ready(function () {

 $("#GidListado").dxDataGrid({
        dataSource: TipoVisitaDataSource,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        export: {
            enabled: true,
            allowExportSelectedData: true,
        },
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
            { dataField: 'idSuelo', width: '5%', caption: 'Id', alignment: 'center' },
            { dataField: 'nombre', width: '80%', caption: 'Nombre', alignment: 'center' },
            {
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        caption: "Editar",
                        height: 20,
                        hint: 'Editar la información relacionada con el registro seleccionado',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "GestionRiesgo/api/TipoSueloApi/ObtenerTipoSueloAsync";
                            $.getJSON(_Ruta,
                                {
                                    id: options.data.idSuelo
                                }).done(function (data) {
                                    if (data !== null) {
                                        txtNombre.option("value", data.nombre);
                                        popup.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Evento no esperado!');
                                });
                        }
                    }).appendTo(container);
                }
            },
            {
                width: '5%',
                caption: "Eliminar",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'remove',
                        height: 20,
                        hint: 'Eliminar el Tipo de Visita',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar el registro seleccionado?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "GestionRiesgo/api/TipoSueloApi/EliminarTipoSueloAsync";
                                    var params = {
                                        idSuelo: options.data.idSuelo, nombre: "."
                                    };
                                    $.ajax({
                                        type: 'POST',
                                        url: _Ruta,
                                        contentType: "application/json",
                                        dataType: 'json',
                                        data: JSON.stringify(params),
                                        success: function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar registro seleccionado');
                                            else {
                                                $('#GidListado').dxDataGrid({ dataSource: TipoVisitaDataSource });
                                                DevExpress.ui.dialog.alert('Registro eliminado correctamente!');
                                            }
                                        },
                                        error: function (xhr, textStatus, errorThrown) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar registro seleccionado');
                                      }
                                    });
                                }
                            });
                        }
                    }).appendTo(container);
                }
            },
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                IdRegistro = data.idTipoSuelo;
            }
        }
    });

var txtNombre = $("#txtNombre").dxTextBox({
    value: "",
    readOnly: false
}).dxValidator({
    validationGroup: "ProcesoGroup",
    validationRules: [{
        type: "required",
        message: "Debe ingresar el nombre del tipo de suelo!"
    }]
}).dxTextBox("instance");

$("#btnNuevo").dxButton({
    stylingMode: "contained",
    text: "Nuevo",
    type: "success",
    width: 200,
    height: 30,
    icon: 'add',
    onClick: function () {
        IdRegistro = 0;
        txtNombre.reset();
        popup.show();
    }
});

$("#btnGuarda").dxButton({
    text: "Guardar",
    type: "default",
    height: 30,
    onClick: function () {
        DevExpress.validationEngine.validateGroup("ProcesoGroup");
        var id = IdRegistro;
        var nombre = txtNombre.option("value");
        if (nombre === "") return;
        var params = {
            idSuelo: id, nombre: nombre
        };

        var _Ruta = $('#SIM').data('url') + "GestionRiesgo/api/TipoSueloApi/GuardarTipoSueloAsync";
        $.ajax({
            type: "POST",
            dataType: 'json',
            url: _Ruta,
            data: JSON.stringify(params),
            contentType: "application/json",
            crossDomain: true,
            headers: { 'Access-Control-Allow-Origin': '*' },
            success: function (data) {
                if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Guardar Datos');
                else {
                    DevExpress.ui.dialog.alert('Datos Guardados correctamente', 'Guardar Datos');
                    $('#GidListado').dxDataGrid({ dataSource: TipoVisitaDataSource });
                    $("#PopupNuevo").dxPopup("instance").hide();
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
            }
        });
    }
});

var popup = $("#PopupNuevo").dxPopup({
        width: 900,
        height: "auto",
        dragEnabled: true,
        resizeEnabled: true,
        hoverStateEnabled: true,
        title: "Tipo de Evento"
    }).dxPopup("instance");

});

var TipoVisitaDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"nombre","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'GestionRiesgo/api/TipoSueloApi/GetTiposSuelosAsync', {
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

    