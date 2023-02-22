var idAsunto = 0;
var IdRegistro = -1;
var idTramite;
var idDocumento;

$(document).ready(function () {

    $("#GidListado").dxDataGrid({
        dataSource: DerechosPeticionDataSource,
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
            visible: false,
            width: 240,
            placeholder: "Buscar..."
        },
        selection: {
            mode: 'single'
        },
        hoverStateEnabled: true,
        remoteOperations: false,
        columns: [
            { dataField: 'ID', width: '5%', caption: 'Id', alignment: 'center' },
            { dataField: 'CODTRAMITE', width: '5%', caption: 'Trámite', alignment: 'center' },
            { dataField: 'CODDOCUMENTO', width: '5%', caption: 'Documento', alignment: 'center' },
            { dataField: 'RADICADO', width: '5%', caption: 'Radicado', alignment: 'center' },
            { dataField: 'ANIO', width: '5%', caption: 'Año', alignment: 'center' },
            { dataField: 'CM', width: '5%', caption: 'CM', dataType: 'string' },
            { dataField: 'PROYECTO', width: '10%', caption: 'Proyecto', dataType: 'string' },
            { dataField: 'SOLICITUD', width: '10%', caption: 'Solicitud', dataType: 'string' },
            { dataField: 'SOLICITANTE', width: '10%', caption: 'Solicitante', dataType: 'string' },
            { dataField: 'TECNICO', width: '10%', caption: 'Técnico', dataType: 'string' },
            { dataField: 'APOYO', width: '15%', caption: 'Apoyo', dataType: 'string' },
            { dataField: 'RADICADO_SALIDA', width: '5%', caption: 'Radicado salida', dataType: 'string' },
            {
                visible: canEdit,
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        caption: "Editar",
                        height: 20,
                        hint: 'Editar la información relacionada con el derecho de petición',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/DerechoPeticionApi/ObtenerDerechoPeticion";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.ID
                                }).done(function (data) {
                                    if (data !== null) {
                                        IdRegistro = parseInt(data.id);
                                        txtCM.option("value", data.cm);
                                        txtProyecto.option("value", data.proyecto);
                                        txtNombreProyectoA.option("value", data.nombreProyecto);
                                        txtSolicitante.option("value", data.solicitante);
                                        txtAsunto.option("value", data.asunto);
                                        txtTecnico.option("value", data.tecnico);
                                        txtApoyo.option("value", data.apoyo);
                                        txtAnio.option("value", data.anio);
                                        txtRadicado.option("value", data.radicado);
                                        idTramite = data.codigoTramite;
                                        idDocumento = data.codigoDocumento;
                                        popup.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                                });
                        }
                    }).appendTo(container);
                }
            },
            {
                visible: canDelete,
                width: '5%',
                caption: "Eliminar",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'remove',
                        height: 20,
                        hint: 'Eliminar el Derecho de Petición',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar el registro seleccionado?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/DerechoPeticionApi/EliminarDerechoPeticion";
                                    var params = {
                                        id: options.data.ID
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
                                                $('#GidListado').dxDataGrid({ dataSource: DerechosPeticionDataSource });
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
                IdRegistro = data.ID;
            }
        }
    });


var txtCM = $("#txtCM").dxTextBox({
    readOnly: true,
    value: "",
}).dxTextBox("instance");

var txtAsunto = $("#txtAsunto").dxTextArea({
    value: "",
    readOnly: true,
    height: 60
}).dxTextArea("instance");

var txtRadicado = $("#txtRadicado").dxTextBox({
    value: "",
    readOnly: false
}).dxTextBox("instance");

var txtAnio = $("#txtAnio").dxNumberBox({
    value: 2022,
    readOnly: false,
    min: 2016,
    max: 2025,
    showSpinButtons: true,
}).dxNumberBox("instance");

var txtProyecto = $("#txtProyecto").dxTextArea({
    value: "",
    readOnly: true,
    height: 40
}).dxTextArea("instance");


     
var txtNombreProyectoA = $("#txtNombreProyectoA").dxTextArea({
        value: "",
        readOnly: false,
        height: 40
    }).dxTextArea("instance");

var txtTecnico = $("#txtTecnico").dxSelectBox({
    dataSource: new DevExpress.data.DataSource({
        store: new DevExpress.data.CustomStore({
            key: "Nombre",
            loadMode: "raw",
            load: function () {
                return $.getJSON($("#SIM").data("url") + "ControlVigilancia/api/ReposicionesApi/GetTecnicos");
            }
        })
    }),
    displayExpr: "Nombre",
    searchEnabled: true
}).dxValidator({
    validationGroup: "ProcesoGroup",
    validationRules: [{
        type: "required",
        message: "El Técnico es Obligatorio!"
    }]
}).dxSelectBox("instance");



 var txtSolicitante = $("#txtSolicitante").dxTextBox({
        value: "",
 }).dxTextBox("instance");


 var txtApoyo = $("#txtApoyo").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Nombre",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "ControlVigilancia/api/ReposicionesApi/GetTecnicos");
                }
            })
        }),
        displayExpr: "Nombre",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El Técnico es Obligatorio!"
        }]
 }).dxSelectBox("instance");

 var txtRadicadoSalida = $("#txtRadicadoSalida").dxTextBox({
        value: "",
        height: 40
    }).dxTextBox("instance");

var txtNombreProyecto = $("#txtNombreProyecto").dxTextBox({
    value: "",
    readOnly: true
}).dxTextBox("instance");

$("#btnNuevo").dxButton({
    stylingMode: "contained",
    text: "Nuevo",
    type: "success",
    width: 200,
    height: 30,
    icon: 'add',
    onClick: function () {

        txtCM.reset();
        txtAsunto.reset();
        txtRadicado.reset();
        txtAnio.reset();
        txtProyecto.reset();
        txtTecnico.reset();
        txtSolicitante.reset();
        txtApoyo.reset();
        txtRadicadoSalida.reset();
        txtNombreProyecto.reset();
        txtNombreProyectoA.reset();
        IdRegistro = -1;
        popup.show();
    }
});

const dataini = [{
    id: 0,
    nombre: '...'
    }]

const dt = new DevExpress.data.ArrayStore({
    data: dataini,
    key: "id"
});


$("#btnAsignar").dxButton({
        text: "Asignar",
        type: "default",
        height: 30,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            txtProyecto.option("value", txtNombreProyecto.option("value"));
            txtAsunto.option("value", cboAsuntos._options.text);
            idAsunto = cboAsuntos._options.value;
            $("#PupupCM").dxPopup("instance").hide();
        }
});

var cboAsuntos = $("#cboAsuntos").dxSelectBox({
    dataSource: dt,
    displayExpr: "descripcion",
    valueExpr: 'id',
    searchEnabled: true
}).dxValidator({
    validationGroup: "ProcesoGroup",
    validationRules: [{
        type: "required",
        message: "Debe seleccionar el Asunto!"
    }]
}).dxSelectBox("instance");

$("#cmdCM").dxButton({
    text: "Consultar",
    stylingMode: 'outlined',
    type: "danger",
    height: 35,
    onClick: function () {
        var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/DerechoPeticionApi/ObtenerDocumentos";
        $.getJSON(_Ruta,
            {
                radicado: txtRadicado.option('value'),
                anio : txtAnio.option('value')
            }).done(function (data) {
                if (data.Id > 0) {
                    txtProyecto.option("value", data.Proyecto);
                    txtSolicitante.option("value", data.Solicitante);
                    txtAsunto.option("value", data.Asunto);
                    txtCM.option("value", data.Cm);
                    idTramite = data.TramiteId;
                    idDocumento = data.DocumentoId;
                 }
                else {
                    alert("Documento no encontrado!");
                }

            }).fail(function (jqxhr, textStatus, error) {
                DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
            });
    }
});

$("#btnGuarda").dxButton({
    text: "Guardar",
    type: "default",
    height: 30,
    onClick: function () {
        DevExpress.validationEngine.validateGroup("ProcesoGroup");
        var id = IdRegistro;
        var cm = txtCM.option("value");
        var asunto = txtAsunto.option("value");
        var proyecto = txtProyecto.option("value");
        var nombreProyecto = txtNombreProyectoA.option("value");
        var tecnico = txtTecnico.option("value").Nombre;
        var radicado = txtRadicado.option("value");
        var anio = txtAnio.option("value");
        var apoyo = txtApoyo.option("value").Nombre;
        var radicadoSalida = txtRadicadoSalida.option("value");
        var codigoSolicitud = idAsunto;
        var solicitante = txtSolicitante.option("value");
        var codigoActoAdministrativo = cboAsuntos._options.value;
        var params = {
            id: id, codigoTramite: idTramite, radicado: radicado, anio: anio, codigoDocumento: idDocumento, cm: cm, asunto: asunto, proyecto: proyecto, tecnico: tecnico,
            solicitante: solicitante, radicadoSalida: radicadoSalida, apoyo: apoyo, codigoSolicitud: codigoSolicitud, codigoActoAdministrativo: codigoActoAdministrativo,
            nombreProyecto: nombreProyecto
        };
        var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/DerechoPeticionApi/GuardarDerechoPeticion";
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
                    $('#GidListado').dxDataGrid({ dataSource: DerechosPeticionDataSource });
                    $("#PopupNuevoDP").dxPopup("instance").hide();
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
            }
        });
    }
});

var popup = $("#PopupNuevoDP").dxPopup({
        width: 900,
        height: "auto",
        dragEnabled: true,
        resizeEnabled: true,
        hoverStateEnabled: true,
        title: "Derecho de Petición"
    }).dxPopup("instance");

var pupupCM = $("#PupupCM").dxPopup({
    width: 1300,
    height: "auto",
    hoverStateEnabled: true,
    title: "Buscar documento"
}).dxPopup("instance");


});


var DerechosPeticionDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"CM","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'ControlVigilancia/api/DerechoPeticionApi/GetDerechosPeticion', {
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
    