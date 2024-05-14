var idRecord;

var nombre = "";

$(document).ready(function () {

    $("#GridListado").dxDataGrid({
        dataSource: gridDataSource,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        export: {
            enabled: true,
            allowExportSelectedData: true,
        },
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
            { dataField: 'tipoAdquisicionId', width: '5%', caption: 'Id', alignment: 'center' },
            { dataField: 'nombre', width: '35%', caption: 'Nombre' },
            { dataField: 'descripcion', width: '40%', caption: 'Descripción' },
            { dataField: 'eliminado', width: '5%', caption: 'Eliminado', dataType: 'boolean', alignment: 'center' },
            { dataField: 'activo', width: '5%', caption: 'Activo', dataType: 'boolean',alignment: 'center' },
            {
                visible: canEdit,
                width: 60,
                caption: "Editar",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        caption: "Editar",
                        height: 30,
                        width: 30,
                        hint: 'Editar la información relacionada con el registro seleccionado',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "CAV/api/CAVApi/GetTipoAdqusicionsync";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.tipoAdquisicionId
                                }).done(function (data) {
                                    if (data !== null) {
                                        idRecord = data.tipoAdquisicionId;
                                        txtNombre.option("value", data.nombre);
                                        txtDescripcion.option("value", data.descripcion);
                                        var OptEliminado = false;
                                        if (data.eliminado === 1) {
                                            OptEliminado = true;
                                        }
                                        chkEliminado.option("value", OptEliminado);
                                        var OptActivo = false;
                                        if (data.activo === 1) {
                                            OptActivo = true;
                                        }
                                        chkActivo.option("value", OptActivo);
                                        popupNewRecord.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Evento no esperado!');
                                });
                        }
                    }).appendTo(container);
                }
            },
            {
                visible: canDelete,
                width: 60,
                caption: "Eliminar",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'remove',
                        height: 30,
                        width : 30,
                        hint: 'Eliminar el Tipo de Adquisición',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar el registro seleccionado?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "CAV/api/CAVApi/EliminarTipoAdquisicionAsync?Id=" + options.data.tipoAdquisicionId;
                                    $.ajax({
                                        type: 'DELETE',
                                        url: _Ruta,
                                        contentType: "application/json",
                                        dataType: 'text',
                                        success: function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar registro seleccionado');
                                            else {
                                                $('#GidListado').dxDataGrid({ dataSource: gridDataSource });
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
                idRecord = data.TipoAdquisicionId;
            }
        }
    });


    var txtNombre = $("#txtNombre").dxTextBox({
        readOnly: false,
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El nombre es requerido!"
        }]
    }).dxTextBox("instance");


    var txtDescripcion = $("#txtDescripcion").dxTextArea({
        value: "",
        readOnly: false,
        height: 70
    }).dxTextArea("instance");
    
    var chkEliminado = $("#chkEliminado").dxCheckBox({
        value: false,
        width: 80,
        text: "Si",

    }).dxCheckBox("instance");

    var chkActivo = $("#chkActivo").dxCheckBox({
        value: false,
        width: 80,
        text: "Si",

    }).dxCheckBox("instance");
        
    
    $("#btnSaveRecord").dxButton({
        text: "Guardar",
        type: "default",
        height: 30,
        onClick: function () {

            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var id = idRecord;

            var nombre = txtNombre.option("value");
            if (nombre === "") return;

            var descripcion = txtDescripcion.option("value");
            var beliminado = chkEliminado.option("value");
            var eliminado = 0;
            if (beliminado) {
                eliminado = 1;
            }
            var bactivo = chkActivo.option("value");
            var activo = 0;
            if (bactivo) {
                activo = 1;
            }

            var params = {
                TipoAdquisicionId: id, Nombre: nombre, Descripcion: descripcion,Eliminado: eliminado, Activo: activo
            };

            var _Ruta = $('#SIM').data('url') + "CAV/api/CAVApi/GuardarTipoAdquisicionAsync";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                crossDomain: true,
                headers: { 'Access-Control-Allow-Origin': '*' },
                success: function (data) {
                    if (data.IsSuccess === false) DevExpress.ui.dialog.alert('Ocurrió un error ' + data.Message, 'Guardar Datos');
                    else {
                        DevExpress.ui.dialog.alert('Tipo de Adquisición Creado/Actualizado correctamente' , 'Guardar Datos');
                        $('#GidListado').dxDataGrid({ dataSource: gridDataSource });
                        $('#popupNewRecord').dxPopup("instance").hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    }).dxButton("instance");

    $("#btnNuevo").dxButton({
        text: "Nuevo",
        type: "success",
        height: 30,
        width: 100,
        icon: 'add',
        onClick: function () {
            idRecord = 0;
            txtNombre.reset();
            txtDescripcion.reset();
            chkEliminado.option("value", false);
            chkActivo.option("value", false);
            popupNewRecord.show();
        }
    }).dxButton("instance");

    var popupNewRecord = $("#popupNewRecord").dxPopup({
        width: 900,
        height: "auto",
        dragEnabled: true,
        resizeEnabled: true,
        hoverStateEnabled: true,
        title: "Creación/Edición un Tipo de Adquisición"
    }).dxPopup("instance");


});

function isNotEmpty(value) {
    return value !== undefined && value !== null && value !== "";
}

var gridDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var params = {};
        [
            "filter",
            "group",
            "groupSummary",
            "parentIds",
            "requireGroupCount",
            "requireTotalCount",
            "searchExpr",
            "searchOperation",
            "searchValue",
            "select",
            "sort",
            "skip",
            "take",
            "totalSummary",
            "userData"
        ].forEach(function (i) {
            if (i in loadOptions && isNotEmpty(loadOptions[i])) {
                params[i] = JSON.stringify(loadOptions[i]);
            }
        });

        $.getJSON($('#SIM').data('url') + 'CAV/api/CAVApi/GetTiposAdquisicionAsync', params).done(function (data) {
            if (data === null) {
                alert('La consulta no retornó ningún dato!');
            }
            d.resolve(data.data, { totalCount: data.totalCount });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
      
    }
});




