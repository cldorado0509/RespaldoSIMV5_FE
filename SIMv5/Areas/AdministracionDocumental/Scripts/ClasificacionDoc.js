var IdSerie = -1;
var IdSubSerie = -1;
var IdUnidad = -1;
var IdMetadato = -1;

$(document).ready(function () {
    var IdRegistro = -1;

    $('#asistente').accordion();

    //Series Documentales
    $("#GidListado").dxDataGrid({
        dataSource: SerieDocumentalDataSource,
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
            { dataField: 'CODSERIE_DOCUMENTAL', width: '10%', caption: 'Id', alignment: 'center' },
            { dataField: 'COD_INTERNO', width: '10%', caption: 'Código Interno', alignment: 'center' },
            { dataField: 'NOMBRE', width: '30%', caption: 'Nombre de la Serie Documental', dataType: 'string' },
            { dataField: 'DESCRIPCION', width: '30%', caption: 'Descripción', dataType: 'string' },
            {
                dataField: 'ACTIVO', width: '10%', caption: 'Habilitada', dataType: 'string', customizeText: function (cellInfo) {
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
                        hint: 'Editar la información de la Serie Documental',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "AdministracionDocumental/api/AdminDocumentalAPI/ObtenerSerieDocumental";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.CODSERIE_DOCUMENTAL
                                }).done(function (data) {
                                    if (data !== null) {
                                        IdRegistro = parseInt(data.id);
                                        IdSerie = parseInt(data.id);
                                        txtNombre.option("value", data.nombre);
                                        txtCodInterno.option("value", data.codInterno);
                                        txtDescripcion.option("value", data.descripcion);
                                        var OptEstado = data.activo;
                                        chkActivo.option("value", OptEstado);
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
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: '../Content/Images/Delete.png',
                        height: 20,
                        hint: 'Eliminar la Serie Documental',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar la Serie Documental : ' + options.data.NOMBRE + '?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "AdministracionDocumental/api/AdminDocumentalAPI/EliminarSerieDocumental";
                                    $.getJSON(_Ruta,
                                        {
                                            objData: options.data.CODSERIE_DOCUMENTAL
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar la Serie Documental');
                                            else {
                                                $('#GidListado').dxDataGrid({ dataSource: SerieDocumentalDataSource });
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar la Serie Documental');
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
                IdSerie = data.CODSERIE_DOCUMENTAL;
                $('#GidListadoSubSeries').dxDataGrid({ dataSource: SubSeriesDataSource });
                $("#btnNuevaSubSerie").dxButton("instance").option("disabled", false);
                $('#lblserie').text(data.NOMBRE);
            }
        }
    });

    var txtNombre = $("#txtNombre").dxTextBox({
        placeholder: "Ingrese el nombre de la Serie Documental...",
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

    var txtCodInterno = $("#txtCodInterno").dxTextBox({
        value: "",
       
    }).dxTextBox("instance");

    var chkActivo = $("#chkActivo").dxCheckBox({
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
            var codInterno = txtCodInterno.option("value");
            var descripcion = txtDescripcion.option("value");
            var estado = chkActivo.option("value");
            var params = { id: id, nombre: nombre, descripcion: descripcion, habilitado: estado, codInterno: codInterno };
            var _Ruta = $('#SIM').data('url') + "AdministracionDocumental/api/AdminDocumentalAPI/GuardarSerieDocumental";
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

    var popup = $("#PopupNuevaSerie").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Serie Documental"
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
            chkActivo.option("value", true);
            popup.show();
        }
    });


    //SubSeries Documentales
    $("#GidListadoSubSeries").dxDataGrid({
        dataSource: SubSeriesDataSource,
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
            { dataField: 'CODSUBSERIE_DOCUMENTAL', width: '10%', caption: 'Código', alignment: 'center' },
            { dataField: 'NOMBRE', width: '30%', caption: 'Nombre SubSerie Documental', dataType: 'string' },
            { dataField: 'DESCRIPCION', width: '30%', caption: 'Descripción', dataType: 'string' },
            {
                dataField: 'ACTIVO', width: '10%', caption: 'Habilitada', dataType: 'string', customizeText: function (cellInfo) {
                    if (cellInfo.value == 'S') {
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
                        hint: 'Editar la información de la SubSerie Documental',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "AdministracionDocumental/api/AdminDocumentalAPI/ObtenerSubSerieDocumental";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.CODSUBSERIE_DOCUMENTAL
                                }).done(function (data) {
                                    if (data !== null) {
                                        IdSubSerie = parseInt(data.id);
                                        txtSubSerie.option("value", data.nombre);
                                        txtDescripcionSubSerie.option("value", data.descripcion);
                                        var OptEstado = data.habilitado;
                                        chkEstadoSubSerie.option("value", OptEstado);
                                        popupSubSerie.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'al Editar la SubSerie Documental');
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
                        hint: 'Eliminar la SubSerie Documental',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar la SubSerie Documental : ' + options.data.NOMBRE + '?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "AdministracionDocumental/api/AdminDocumentalAPI/EliminarSubSerieDocumental";
                                    $.getJSON(_Ruta,
                                        {
                                            objData: options.data.ID
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'al Eliminar la SubSerie Documental');
                                            else {
                                                $('#GidListadoSubSeries').dxDataGrid({ dataSource: SubSeriesDataSource });
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
                IdSubSerie = data.CODSUBSERIE_DOCUMENTAL;
                $('#lblsubserie').text(data.NOMBRE);
                $('#GidListadoUnidades').dxDataGrid({ dataSource: UnidadesDataSource });
                $("#btnNuevaUnidad").dxButton("instance").option("disabled", false);
            }
        }
    });

    var txtSubSerie = $("#txtSubSerie").dxTextBox({
        placeholder: "Ingrese el nombre de la SubSerie Documental ...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El nombre de la SubSerie Documental es obligatorio!"
        }]
    }).dxTextBox("instance");

    var txtDescripcionSubSerie = $("#txtDescripcionSubSerie").dxTextArea({
        height: 150,
        placeholder: "Ingrese la descripción de la SubSerie Documental...",
        value: "",
    }).dxTextArea("instance");

    var chkEstadoSubSerie = $("#chkEstadoSubSerie").dxCheckBox({
        value: false,
        width: 80,
        text: "Si",

    }).dxCheckBox("instance");

    $("#btnGuardarSubSerie").dxButton({
        text: "Guardar",
        type: "default",
        height: 30,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var id = IdSubSerie;
            var nombre = txtSubSerie.option("value");
            var descripcion = txtDescripcionSubSerie.option("value");
            var activo = chkEstadoSubSerie.option("value");

            var params = { id: id, serieId: IdSerie, nombre: nombre, descripcion: descripcion, habilitado: activo };
            var _Ruta = $('#SIM').data('url') + "AdministracionDocumental/api/AdminDocumentalAPI/GuardarSubSerieDocumental";
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
                        $('#GidListadoSubSeries').dxDataGrid({ dataSource: SubSeriesDataSource });
                        $("#PopupNuevaSubSerie").dxPopup("instance").hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un evento no esperado : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    var popupSubSerie = $("#PopupNuevaSubSerie").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "SubSerie Documental"
    }).dxPopup("instance");

    $("#btnNuevaSubSerie").dxButton({
        stylingMode: "contained",
        text: "Nuevo",
        type: "success",
        width: 200,
        height: 30,
        disabled: true,
        icon: '../Content/Images/new.png',
        onClick: function () {
            IdRegistro = -1;
            txtSubSerie.reset();
            txtDescripcionSubSerie.reset();
            chkEstadoSubSerie.option("value", true);
            popupSubSerie.show();
        }
    });

   

       //Unidades Documentales
    $("#GidListadoUnidades").dxDataGrid({
        dataSource: UnidadesDataSource,
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
            { dataField: 'CODSERIE', width: '10%', caption: 'Código', alignment: 'center' },
            { dataField: 'NOMBRE', width: '15%', caption: 'Nombre SubSerie Documental', dataType: 'string' },
            { dataField: 'DESCRIPCION', width: '20%', caption: 'Descripción', dataType: 'string' },
            { dataField: 'TIEMPO_GESTION', width: '10%', caption: 'T. Gestión', dataType: 'number' },
            { dataField: 'TIEMPO_CENTRAL', width: '10%', caption: 'T. Central', dataType: 'number' },
            { dataField: 'TIEMPO_HISTORICO', width: '10%', caption: 'T. Histórico', dataType: 'number' },
            { dataField: 'RUTA_DOCUMENTOS', width: '10%', caption: 'Ruta de los Documentos', dataType: 'string' },
            {
                dataField: 'RADICADO', width: '10%', caption: 'Radicado', dataType: 'string', customizeText: function (cellInfo) {
                    if (cellInfo.value == '1') {
                        return 'Si';
                    }
                    else {
                        return 'No';
                    }
                }},
            {
                dataField: 'ACTIVO', width: '10%', caption: 'Habilitada', dataType: 'string', customizeText: function (cellInfo) {
                    if (cellInfo.value == '1') {
                        return 'Si';
                    }
                    else {
                        return 'No';
                    }
                } },
            {
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: '../Content/Images/edit.png',
                        height: 20,
                        hint: 'Editar la información de la Unidad Documental',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "AdministracionDocumental/api/AdminDocumentalAPI/ObtenerUnidadDocumental";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.CODSERIE
                                }).done(function (data) {
                                    if (data !== null) {
                                        IdUnidad = parseInt(data.id);
                                        txtUnidad.option("value", data.nombre);
                                        txtDescripcionUnidad.option("value", data.descripcion);
                                        txtCentral.option("value", data.tiempoCentral);
                                        txtGestion.option("value", data.tiempoGestion);
                                        txtHistorico.option("value", data.tiempoHistorico);
                                        txtRutaDocumentos.option("value", data.rutaDocumentos);
                                        var OptEstado = data.habilitado;
                                        var OptRadicado = data.radicado;
                                        chkEstadoUnidad.option("value", OptEstado);
                                        chkRadicado.option("value", OptRadicado)
                                        popupUnidad.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'al Editar la Unidad Documental');
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
                        hint: 'Eliminar la Unidad Documental',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar la Unidad Documental : ' + options.data.NOMBRE + '?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "AdministracionDocumental/api/AdminDocumentalAPI/EliminarUnidadDocumental";
                                    $.getJSON(_Ruta,
                                        {
                                            objData: options.data.ID
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'al Eliminar la Unidad Documental');
                                            else {
                                                $('#GidListadoUnidades').dxDataGrid({ dataSource: UnidadesDataSource });
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar la Unidad Documental');
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
                IdUnidad = data.CODSERIE;
                $('#lblunidad').text(data.NOMBRE);
                $('#GidListadoMetadatos').dxDataGrid({ dataSource: MetadatosDataSource });
                $("#btnNuevoMetadato").dxButton("instance").option("disabled", false);
            }
        }
    });


    var txtUnidad = $("#txtUnidad").dxTextBox({
        placeholder: "Ingrese el nombre de la Unidad Documental ...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El nombre de la Unidad Documental es obligatorio!"
        }]
    }).dxTextBox("instance");

    var txtRutaDocumentos = $("#txtRutaDocumentos").dxTextBox({
        placeholder: "Ingrese la ruta de almacenamiento de la Unidad Documental ...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "La ruta de almacenamiento de la Unidad Documental es obligatorio!"
        }]
    }).dxTextBox("instance");

    var txtDescripcionUnidad = $("#txtDescripcionUnidad").dxTextArea({
        height: 150,
        placeholder: "Ingrese la descripción de la Unidad Documental...",
        value: "",
    }).dxTextArea("instance");


    var chkEstadoUnidad = $("#chkEstadoUnidad").dxCheckBox({
        value: false,
        width: 80,
        text: "Si",

    }).dxCheckBox("instance");

    var txtGestion = $("#txtGestion").dxNumberBox({
        placeholder: "Ingrese el tiempo en meses en el archivo de gestión...",
        format: "$ #,##0.##",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El tiempo en meses en el archivo de gestión es obligatorio!"
        }]
    }).dxNumberBox("instance");

    var txtCentral = $("#txtCentral").dxNumberBox({
        placeholder: "Ingrese el tiempo en meses en el archivo central...",
        format: "$ #,##0.##",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El tiempo en meses en el archivo central es obligatorio!"
        }]
    }).dxNumberBox("instance");

    var txtHistorico = $("#txtHistorico").dxNumberBox({
        placeholder: "Ingrese el tiempo en meses en el archivo histórico...",
        format: "$ #,##0.##",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El tiempo en meses en el archivo histórico es obligatorio!"
        }]
    }).dxNumberBox("instance");

    var chkRadicado = $("#chkRadicado").dxCheckBox({
        value: false,
        width: 80,
        text: "Si",

    }).dxCheckBox("instance");

    $("#btnGuardarUnidad").dxButton({
        text: "Guardar",
        type: "default",
        height: 30,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var id = IdUnidad;
            var nombre = txtUnidad.option("value");
            var descripcion = txtDescripcionUnidad.option("value");
            var rutaDocumentos = txtRutaDocumentos.option("value");
            var tgestion = txtGestion.option("value");
            var tcentral = txtCentral.option("value");
            var thistorico = txtHistorico.option("value");
            var radicado = chkRadicado.option("value");
            var activo = chkEstadoUnidad.option("value");

            var params = { id: id, subSerieId: IdSubSerie, nombre: nombre, descripcion: descripcion, tiempoGestion: tgestion, tiempoCentral: tcentral, tiempoHistorico: thistorico, habilitado: activo, radicado: radicado, rutaDocumentos: rutaDocumentos };
            var _Ruta = $('#SIM').data('url') + "AdministracionDocumental/api/AdminDocumentalAPI/GuardarUnidadDocumental";
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
                        $('#GidListadoUnidades').dxDataGrid({ dataSource: UnidadesDataSource });
                        $("#PopupNuevaUnidad").dxPopup("instance").hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un evento no esperado : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    var popupUnidad = $("#PopupNuevaUnidad").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Unidad Documental"
    }).dxPopup("instance");


    $("#btnNuevaUnidad").dxButton({
        stylingMode: "contained",
        text: "Nuevo",
        type: "success",
        width: 200,
        height: 30,
        disabled: true,
        icon: '../Content/Images/new.png',
        onClick: function () {
            IdRegistro = -1;
            txtUnidad.reset();
            txtDescripcionUnidad.reset();
            txtRutaDocumentos.reset();
            txtGestion.reset();
            txtCentral.reset();
            txtHistorico.reset();
            chkEstadoUnidad.option("value", true);
            chkRadicado.option("value", false);
            popupUnidad.show();
        }
    });

    //Metadatos Descriptivos
    $("#GidListadoMetadatos").dxDataGrid({
        dataSource: MetadatosDataSource,
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
            { dataField: 'CODINDICE', width: '10%', caption: 'Código', alignment: 'center' },
            { dataField: 'INDICE', width: '30%', caption: 'Nombre Metadato', dataType: 'string' },
            {
                dataField: 'TIPO', width: '20%', caption: 'Tipo', dataType: 'string', customizeText: function (cellInfo) {
                    if (cellInfo.value == '0') {
                        return 'Alfanumérico';
                    }
                    if (cellInfo.value == '1') {
                        return 'Numérico';
                    }
                    if (cellInfo.value == '2') {
                        return 'Fecha';
                    }
                    if (cellInfo.value == '3') {
                        return 'Hora';
                    }
                    if (cellInfo.value == '4') {
                        return 'Booleano';
                    }
                    if (cellInfo.value == '5') {
                        return 'Listado';
                    }
                    if (cellInfo.value == '8') {
                        return 'Dirección';
                    }
                    if (cellInfo.value == '9') {
                        return 'Expediente';
                    }
                }
            },
            { dataField: 'ORDEN', width: '10%', caption: 'Orden', dataType: 'string' },
            {
                dataField: 'OBLIGA', width: '10%', caption: 'Obligatorio', dataType: 'string', customizeText: function (cellInfo) {
                    if (cellInfo.value == '1') {
                        return 'Si';
                    }
                    else {
                        return 'No';
                    }
                } },
            {
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: '../Content/Images/edit.png',
                        height: 20,
                        hint: 'Editar la información del Metadato',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "AdministracionDocumental/api/AdminDocumentalAPI/ObtenerMetadato";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.CODINDICE
                                }).done(function (data) {
                                    if (data !== null) {
                                        IdMetadato = parseInt(data.id);
                                        txtMetadato.option("value", data.nombre);

                                        switch (data.tipo) {
                                            case "0":
                                                cboTipoDato.option("text", "Alfanumérico");
                                                break;
                                            case "1":
                                                cboTipoDato.option("text", "Numérico");
                                                break;
                                            case "2":
                                                cboTipoDato.option("text", "Fecha");
                                                break;
                                            case "3":
                                                cboTipoDato.option("text", "Hora");
                                                break;
                                            case "4":
                                                cboTipoDato.option("text", "Booleano");
                                                break;
                                            case "5":
                                                cboTipoDato.option("text", "Listado");
                                                break;
                                            case "8":
                                                cboTipoDato.option("text", "Dirección");
                                                break;
                                        }
                                        
                                        txtLongitud.option("value", data.longitud);
                                        var OptObligatorio = data.obligatorio;
                                        chkObligatorio.option("value", OptObligatorio);
                                        txtValorDefecto.option("value", data.valorDefecto);
                                        var OptMostrar = data.mostrar;
                                        chkMostrar.option("value", OptMostrar);
                                        var OptMostrarGrid = data.mostrarEnGrid;
                                        chkMostrarGrid.option("value", OptMostrarGrid);
                                        txtOrden.option("value", data.orden);
                                        cboListados.option("text", data.listadoId);
                                        popupMetadato.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'al Editar el Metadato');
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
                        hint: 'Eliminar la Unidad Documental',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar la Unidad Documental : ' + options.data.NOMBRE + '?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "AdministracionDocumental/api/AdminDocumentalAPI/EliminarUnidadDocumental";
                                    $.getJSON(_Ruta,
                                        {
                                            objData: options.data.ID
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'al Eliminar la Unidad Documental');
                                            else {
                                                $('#GidListadoUnidades').dxDataGrid({ dataSource: UnidadesDataSource });
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar la Unidad Documental');
                                        });
                                }
                            });
                        }
                    }).appendTo(container);
                }
            }
        ]
    });


    var txtMetadato = $("#txtMetadato").dxTextBox({
        placeholder: "Ingrese el nombre del Metadato (índice) ...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El nombre del Metadato es obligatorio!"
        }]
    }).dxTextBox("instance");

    var cboTipoDato = $("#cboTipoDato").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "AdministracionDocumental/api/AdminDocumentalAPI/GetTipoDatos");
                }
            })
        }),
        displayExpr: "Nombre",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El Tipo de dato del Metadato es requerido!"
        }]
    }).dxSelectBox("instance");

    var cboListados = $("#cboListados").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "AdministracionDocumental/api/AdminDocumentalAPI/GetListados");
                }
            })
        }),
        displayExpr: "Nombre",
        searchEnabled: true
    }).dxSelectBox("instance");


    var txtLongitud = $("#txtLongitud").dxTextBox({
        placeholder: "Ingrese la longitud del Metadato ...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "La longitud de metadato es obligatorio!"
        }]
    }).dxTextBox("instance");

    var txtOrden = $("#txtOrden").dxTextBox({
        placeholder: "Ingrese el orden de presentación del Metadato ...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El orden de presentación del metadato es obligatorio!"
        }]
    }).dxTextBox("instance");

    var txtValorDefecto = $("#txtValorDefecto").dxTextArea({
        height: 150,
        placeholder: "Ingrese el valor por defecto del Metadato...",
        value: "",
    }).dxTextArea("instance");

    var chkObligatorio = $("#chkObligatorio").dxCheckBox({
        value: false,
        width: 80,
        text: "Si",

    }).dxCheckBox("instance");

    var chkMostrar = $("#chkMostrar").dxCheckBox({
        value: false,
        width: 80,
        text: "Si",

    }).dxCheckBox("instance");

    var chkMostrarGrid = $("#chkMostrarGrid").dxCheckBox({
        value: false,
        width: 80,
        text: "Si",

    }).dxCheckBox("instance");


    $("#btnGuardarMetadato").dxButton({
        text: "Guardar",
        type: "default",
        height: 30,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var id = IdMetadato;
            var nombre = txtMetadato.option("value");
            var tipo = cboTipoDato.option("value").Id;
            var longitud = txtLongitud.option("value");
            var obligatorio = chkObligatorio.option("value");
            var valorDefecto = txtValorDefecto.option("value");
            var mostrar = chkMostrar.option("value");
            var mostrarEnGrid = chkMostrarGrid.option("value");
            var orden = txtOrden.option("value");
            var listadoId = cboListados.option("value").Id;

            var params = { id: id, unidadId: IdUnidad, nombre: nombre, tipo: tipo, longitud: longitud, obligatorio: obligatorio, valorDefecto: valorDefecto, mostrar: mostrar, mostrarEnGrid: mostrarEnGrid, orden: orden, listadoId: listadoId };
            var _Ruta = $('#SIM').data('url') + "AdministracionDocumental/api/AdminDocumentalAPI/GuardarMetadato";
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
                        $('#GidListadoMetadatos').dxDataGrid({ dataSource: MetadatosDataSource });
                        $("#PopupNuevoMetadato").dxPopup("instance").hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un evento no esperado : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    var popupMetadato = $("#PopupNuevoMetadato").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Metadato"
    }).dxPopup("instance");


    $("#btnNuevoMetadato").dxButton({
        stylingMode: "contained",
        text: "Nuevo",
        type: "success",
        width: 200,
        height: 30,
        disabled: true,
        icon: '../Content/Images/new.png',
        onClick: function () {
            IdRegistro = -1;
            txtMetadato.reset();
            txtLongitud.reset();
            txtValorDefecto.reset();
            txtOrden.reset();
            chkMostrar.option("value", true);
            chkMostrarGrid.option("value", false);
            popupMetadato.show();
        }
    });

   
});

var SerieDocumentalDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"NOMBRE","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'AdministracionDocumental/api/AdminDocumentalAPI/GetSeriesDocumentales', {
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

var SubSeriesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"NOMBRE","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'AdministracionDocumental/api/AdminDocumentalAPI/GetSubSeriesDocumentales', {
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
            Id: IdSerie
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var UnidadesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"NOMBRE","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'AdministracionDocumental/api/AdminDocumentalAPI/GetUnidadesDocumentales', {
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
            Id: IdSubSerie
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var MetadatosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"INDICE","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'AdministracionDocumental/api/AdminDocumentalAPI/GetMetadatosUnidadeDocumental', {
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
            Id: IdUnidad
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});



