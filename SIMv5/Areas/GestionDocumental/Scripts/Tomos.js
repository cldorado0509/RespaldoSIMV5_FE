var IdTomo = -1;
var Carpeta = "";
var NroDocumento = "";
var IdDocumento = -1;
var TomoCerrado = false;

$(document).ready(function () {

    $("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
        shadingColor: "rgba(0,0,0,0.4)",
    });


    $("#grdListaTomos").dxDataGrid({
        dataSource: TomosDataSource,
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
            { dataField: 'ID_TOMO', width: '5%', visible: false },
            { dataField: 'N_TOMO', width: '5%', caption: 'Carpeta', dataType: 'string' },
            { dataField: 'UBICACION', width: '20%', caption: 'Ubicación', dataType: 'string' },
            { dataField: 'FECHA', width: '15%', caption: 'Fecha Creación', dataType: 'date', format: 'MMM dd yyyy HH:mm' },
            { dataField: 'FUNCCREA', width: '20%', caption: 'Funcionario Crea', dataType: 'string' },
            { dataField: 'ABIERTO', width: '5%', caption: 'Abierto', dataType: 'string' },
            { dataField: 'DOCUMENTOS', width: '10%', caption: 'Cantidad documentos', dataType: 'number' },
            { dataField: 'FOLIOS', width: '10%', caption: 'Cantidad folios', dataType: 'number' },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        hint: 'Editar datos del tomo',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/EditarTomo";
                            $.getJSON(_Ruta,
                                {
                                    IdTomo: options.data.ID_TOMO
                                }).done(function (data) {
                                    if (data != null) {
                                        editar = true;
                                        txtTomo.option("value", data.NumeroTomo);
                                        txtUbicacion.option("value", data.Ubicacion);
                                        chkAbierto.option("value", data.Abierto == "1" ? true : false);
                                        chkAbierto.option("disabled", data.Abierto == "1" ? true : false);
                                        txtFolios.option("value", data.CantFolios);
                                        popup.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Eliminar proceso');
                                });
                        }
                    }).appendTo(container);
                }
            },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'trash',
                        hint: 'Eliminar tomo',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar la carpeta ' + options.data.N_TOMO + ' del expediente ' + Expediente + '?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/EliminaTomo";
                                    $.getJSON(_Ruta,
                                        {
                                            IdTomo: options.data.ID_TOMO
                                        }).done(function (data) {
                                            if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar tomo');
                                            else {
                                                $('#grdListaTomos').dxDataGrid({ dataSource: TomosDataSource });
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Eliminar tomo');
                                        });
                                }
                            });
                        }
                    }).appendTo(container);
                }
            },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'selectall',
                        hint: 'Asociar Documentos',
                        onClick: function (e) {
                            if (options.data.ABIERTO == "Si") {
                                var _popup = $("#popupBuscaDoc").dxPopup("instance");
                                _popup.show();
                                $('#BuscarDoc').attr('src', $('#SIM').data('url') + 'Utilidades/BuscarDocumento?popup=true');
                            } else {
                                DevExpress.ui.dialog.alert('Ocurrió un error No se pueden asociar documentos a carpetas cerradas', 'Asociar Documentos');
                            }
                        }
                    }).appendTo(container);
                }
            },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'box',
                        hint: 'Cerrar Carpeta',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Esta seguro de cerrar la carpeta ' + options.data.N_TOMO + ' del expediente ' + Expediente + '?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/CerrarTomo";
                                    $.getJSON(_Ruta,
                                        {
                                            IdTomo: options.data.ID_TOMO
                                        }).done(function (data) {
                                            if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Cerrar carpeta');
                                            else {
                                                DevExpress.ui.dialog.alert(data.mensaje, 'Cerrar carpeta');
                                                $('#grdListaTomos').dxDataGrid({ dataSource: TomosDataSource });
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Eliminar tomo');
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
                TomoCerrado = data.ABIERTO == "Si" ? false : true;
                IdTomo = data.ID_TOMO;
                $('#gridDocumentos').dxDataGrid({ dataSource: DocumentosDataSource });
                Carpeta = "Carpeta " + data.N_TOMO;
            }
        }
    });

    var txtTomo = $("#txtTomo").dxTextBox({
        placeholder: "Ingrese el número de la carpeta",
        value: "",
    }).dxValidator({
        validationGroup: "TomoGroup",
        validationRules: [{
            type: "required",
            message: "El número de la carpeta es obligatorio"
        }]
    }).dxTextBox("instance");

    var txtUbicacion = $("#txtUbicacion").dxTextBox({
        placeholder: "Ingrese la ubicación para la carpeta...",
        value: "",
    }).dxTextBox("instance");

    var chkAbierto = $("#chkAbierto").dxCheckBox({
        value: true,
        width: 80,
    }).dxCheckBox("instance");

    var txtFolios = $("#txtFolios").dxTextBox({
        placeholder: "Ingrese el número de folios para la carpeta",
        value: "",
    }).dxValidator({
        validationGroup: "TomoGroup",
        validationRules: [{
            type: "required",
            message: "El número de folios para la carpeta es obligatorio"
        }]
    }).dxTextBox("instance");

    $("#btnGuarda").dxButton({
        text: "Guardar",
        type: "default",
        onClick: function () {
            DevExpress.validationEngine.validateGroup("TomoGroup");
            var Tomo = txtTomo.option("value");
            var Ubicacion = txtUbicacion.option("value");
            var opcAbierto = chkAbierto.option("value");
            var Abierto = opcAbierto ? "1" : "0";
            var Folios = txtFolios.option("value");
            var params = { IdExpediente: IdExpediente, IdTomo: IdTomo, NumeroTomo: Tomo, Ubicacion: Ubicacion, Abierto: Abierto, CantFolios: Folios };
            var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/GuardaTomo";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                beforeSend: function () { },
                success: function (data) {
                    if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Guardar Datos');
                    else {
                        DevExpress.ui.dialog.alert('Datos Guardados correctamente', 'Guardar Datos');
                        $('#grdListaTomos').dxDataGrid({ dataSource: TomosDataSource });
                        $("#PopupNuevoTomo").dxPopup("instance").hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    var popup = $("#PopupNuevoTomo").dxPopup({
        width: 700,
        height: "auto",
        hoverStateEnabled: true,
        title: "Carpetas del expediente " + Expediente
    }).dxPopup("instance");


    $("#btnNuevo").dxButton({
        stylingMode: "contained",
        text: "Nueva Carpeta",
        type: "success",
        width: 200,
        height: 30,
        icon: 'plus',
        onClick: function () {
            IdTomo = -1;
            editar = false;
            txtTomo.option("value", "");
            txtUbicacion.option("value", "");
            chkAbierto.option("value", true);
            txtFolios.option("value", "");
            txtTomo.reset();
            txtFolios.reset();
            popup.show();
        }
    });

    $("#gridDocumentos").dxDataGrid({
        dataSource: DocumentosDataSource,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Carpeta sin documentos asociados para mostrar",
        showBorders: true,
        paging: {
            pageSize: 7
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20, 50]
        },
        filterRow: {
            visible: true,
            emptyPanelText: 'Arrastre una columna para agrupar'
        },
        selection: {
            mode: 'single'
        },
        hoverStateEnabled: true,
        remoteOperations: true,
        columns: [
            { dataField: 'ID_TOMO', visible: false },
            { dataField: 'ID_DOCUMENTO', visible: false },
            { dataField: 'ORDEN', width: '5%', caption: 'Orden', dataType: 'number' },
            { dataField: 'FECASOCIA', width: '15%', caption: 'Fecha Asociación', dataType: 'date', format: 'MMM dd yyyy HH:mm' },
            { dataField: 'FECDIGITA', width: '15%', caption: 'Fecha Digitaliza', dataType: 'date', format: 'MMM dd yyyy HH:mm' },
            { dataField: 'FUNCASOCIA', width: '20%', caption: 'Funcionario asocia', dataType: 'string' },
            { dataField: 'TIPODOC', width: '20%', caption: 'Tipo de Documento', dataType: 'string' },
            { dataField: 'IMAGENES', width: '5%', caption: 'Imágenes', dataType: 'string' },
            { dataField: 'FOLIOS', width: '5%', caption: 'Folios', dataType: 'string' },
            {
                width: '5%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'fields',
                        hint: 'Indices del documento',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/IndicesDocumento";
                            $.getJSON(_Ruta, { IdDocumento: options.data.ID_DOCUMENTO })
                                .done(function (data) {
                                    if (data != null) {
                                        NroDocumento = options.data.ORDEN;
                                        showIndices(data);
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Indices del documento');
                                });
                        }
                    }).appendTo(container);
                }
            },
            {
                width: '5%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'doc',
                        hint: 'Ver el documento',
                        onClick: function (e) {
                            MostrarDocumento(options.data.ID_DOCUMENTO);
                        }
                    }).appendTo(container);
                }
            },
            {
                width: '5%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (!TomoCerrado) {
                        $('<div/>').dxButton({
                            icon: 'sorted',
                            hint: 'Modificar orden del documento',
                            onClick: function (e) {
                                var result = DevExpress.ui.dialog.confirm('Si cambia el orden del documento se reordenará la carpeta, Esta seguro de cambiar el orden del documento?', 'Confirmación');
                                result.done(function (dialogResult) {
                                    if (dialogResult) {
                                        IdTomo = IdTomo;
                                        IdDocumento = options.data.ID_DOCUMENTO;
                                        NroDocumento = options.data.ORDEN;
                                        var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/ObtieneOrdenDoc";
                                        $.getJSON(_Ruta,
                                            {
                                                IdTomo: IdTomo,
                                                IdDocumento: IdDocumento
                                            }).done(function (data) {
                                                if (data != null) {
                                                    txtOrdenDoc.option("value", data.OrdenDoc);
                                                    txtOrdenDoc.option("max", data.MaxOrden);
                                                    popupReor.show();
                                                }
                                            }).fail(function (jqxhr, textStatus, error) {
                                                DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Modificar orden del documento');
                                            });
                                    }
                                });
                            }
                        }).appendTo(container);
                    }
                }
            },
            {
                width: '5%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (!TomoCerrado) {
                        $('<div/>').dxButton({
                            icon: 'remove',
                            hint: 'Desasociar documento',
                            onClick: function (e) {
                                var result = DevExpress.ui.dialog.confirm('Si desasocia el documento se reordenará la carpeta, Esta seguro de desasociar el documento?', 'Confirmación');
                                result.done(function (dialogResult) {
                                    if (dialogResult) {
                                        IdTomo = IdTomo;
                                        IdDocumento = options.data.ID_DOCUMENTO;
                                        var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/DesasociaDoc";
                                        $.getJSON(_Ruta,
                                            {
                                                IdTomo: IdTomo,
                                                IdDocumento: IdDocumento
                                            }).done(function (data) {
                                                if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Desasocia Documento');
                                                else {
                                                    DevExpress.ui.dialog.alert(data.resp, 'Desasocia Documento');
                                                    $('#gridDocumentos').dxDataGrid({ dataSource: DocumentosDataSource });
                                                }
                                            }).fail(function (jqxhr, textStatus, error) {
                                                DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Desasocia Documento');
                                            });
                                    }
                                });
                            }
                        }).appendTo(container);
                    }
                }
            }
            //,
            //{
            //    width: 30,
            //    alignment: 'center',
            //    cellTemplate: function (container, options) {
            //        $('<div/>').dxButton({
            //            icon: 'contains',
            //            hint: 'Foliar el documento',
            //            onClick: function (e) {
            //                var result = DevExpress.ui.dialog.confirm('Si cambia el orden del documento se reordenará todo el foliado de la carpeta, Esta seguro de foliar el documento?', 'Confirmación');
            //                result.done(function (dialogResult) {
            //                    if (dialogResult) {
            //                        IdTomo = IdTomo;
            //                        IdDocumento = options.data.ID_DOCUMENTO;
            //                        var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/ObtieneFolioDoc";
            //                        $.getJSON(_Ruta,
            //                            {
            //                                IdTomo: options.data.ID_TOMO,
            //                                IdDocumento: IdDocumento
            //                            }).done(function (data) {
            //                                if (data != null) {
            //                                    txtOrden.option("value", data.OrdenDoc);
            //                                    LblImg.option("value", data.Imagenes);
            //                                    txtFolioIni.option("value", data.FolioIni);
            //                                    txtFolioFin.option("value", data.FolioFin);
            //                                    popupFol.show();
            //                                }
            //                            }).fail(function (jqxhr, textStatus, error) {
            //                                DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Foliar Documento');
            //                            });
            //                    }
            //                });
            //            }
            //        }).appendTo(container);
            //    }
            //}
        ],
        onContentReady: function (e) {
            var visibleRowsCount = e.component.totalCount();
            if (visibleRowsCount > 0) {
                $("#TituloDocumentos").text("Documentos de la " + Carpeta + " del expediente " + Expediente);
            } else {
                if (Carpeta != "") $("#TituloDocumentos").text("La " + Carpeta + " del expediente " + Expediente + " NO POSEE DOCUMENTOS");
                else $("#TituloDocumentos").text("");
            }
        }
    });

    $("#btnOrdenar").dxButton({
        text: "Verificar folios documentos",
        type: "default",
        onClick: function () {
            if (Carpeta == "") DevExpress.ui.dialog.alert('Aún no ha seleccionado una carpeta para verificar su foliado', 'Verificar folios documentos');
            else {
                var result = DevExpress.ui.dialog.confirm('Se verificará y actualizará el foliado de la carpeta, Esta seguro de esta acción?', 'Verificar folios documentos');
                result.done(function (dialogResult) {
                    if (dialogResult) {
                        $("#loadPanel").dxLoadPanel('instance').show();
                        var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/FoliarCarpeta?IdTomo=" + IdTomo;
                        $.ajax({
                            type: "POST",
                            dataType: 'json',
                            url: _Ruta,
                            contentType: "application/json",
                            beforeSend: function () { },
                            success: function (data) {
                                if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Verificar folios documentos');
                                else {
                                    DevExpress.ui.dialog.alert('Datos Guardados correctamente', 'Verificar folios documentos');
                                    $('#gridDocumentos').dxDataGrid({ dataSource: DocumentosDataSource });
                                }
                            },
                            error: function (xhr, textStatus, errorThrown) {
                                DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Verificar folios documentos');
                            }
                        });
                        $("#loadPanel").dxLoadPanel('instance').hide();
                    }
                });
            }
        }
    });

    var txtOrden = $("#txtOrden").dxNumberBox({
        placeholder: "Ingrese el orden para el documento",
        value: ""
    }).dxValidator({
        validationGroup: "DocumentoGroup",
        validationRules: [{
            type: "required",
            message: "El orden del documento es obligatorio"
        }]
    }).dxNumberBox("instance");

    var txtFolioIni = $("#txtFolioIni").dxNumberBox({
        placeholder: "Ingrese el folio inicial",
        value: ""
    }).dxValidator({
        validationGroup: "DocumentoGroup",
        validationRules: [{
            type: "required",
            message: "El folio inicial del documento es obligatorio"
        }]
    }).dxNumberBox("instance");

    var txtFolioFin = $("#txtFolioFin").dxNumberBox({
        placeholder: "Ingrese el folio final",
        value: ""
    }).dxValidator({
        validationGroup: "DocumentoGroup",
        validationRules: [{
            type: "required",
            message: "El folio final del documento es obligatorio"
        }]
    }).dxNumberBox("instance");

    var txtOrdenDoc = $("#txtOrdenDoc").dxNumberBox({
        placeholder: "Ingrese el orden para el documento",
        value: "",
        min: 1,
        showSpinButtons: true
    }).dxValidator({
        validationGroup: "DocumentoGroup",
        validationRules: [{
            type: "required",
            message: "El orden del documento es obligatorio"
        }]
    }).dxNumberBox("instance");

    var LblImg = $("#lblImagenes").dxNumberBox({
        value: "",
        disabled: true
    }).dxNumberBox("instance");

    $("#btnGuardaFolio").dxButton({
        text: "Guardar foliado y reorganizar documentos",
        type: "default",
        onClick: function () {
            DevExpress.validationEngine.validateGroup("DocumentoGroup");
            var Orden = txtOrden.option("value");
            var FolioIni = txtFolioIni.option("value");
            var FolioFin = txtFolioFin.option("value");
            var CantImg = LblImg.option("value");
            var params = { IdTomo: IdTomo, IdDocumento: IdDocumento, Orden: Orden, FolioIni: FolioIni, FolioFin: FolioFin, Imagenes: CantImg };
            var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/FoliarDocumento";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                beforeSend: function () { },
                success: function (data) {
                    if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Foliar documento');
                    else {
                        DevExpress.ui.dialog.alert('Datos Guardados correctamente', 'Foliar documento');
                        $('#grdListaTomos').dxDataGrid({ dataSource: TomosDataSource });
                        $("#PopupNuevoTomo").dxPopup("instance").hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Foliar documento');
                }
            });
        }
    });

    $("#btnGuardaOrden").dxButton({
        text: "Guardar reordenar documentos",
        type: "default",
        onClick: function () {
            DevExpress.validationEngine.validateGroup("DocumentoGroup");
            var Orden = txtOrdenDoc.option("value");
            var params = { IdTomo: IdTomo, IdDocumento: IdDocumento, NuevoOrden: Orden };
            var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/ModificaOrdenDoc";
            if (Orden != NroDocumento) {
                $.getJSON(_Ruta,
                    params).done(function (data) {
                        if (data != null) {
                            if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Reordenar documento');
                            else {
                                DevExpress.ui.dialog.alert(data.resp, 'Reordenar Documento');
                                $('#gridDocumentos').dxDataGrid({ dataSource: DocumentosDataSource });
                                $("#PopupReordenar").dxPopup("instance").hide();
                            }
                        }
                    }).fail(function (jqxhr, textStatus, error) {
                        DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Reordenar documento');
                    });

                //    $.ajax({
                //        type: "POST",
                //        dataType: 'json',
                //        url: _Ruta,
                //        data: JSON.stringify(params),
                //        contentType: "application/json",
                //        beforeSend: function () { },
                //        success: function (data) {
                //            if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Reordenar documento');
                //            else {
                //                DevExpress.ui.dialog.alert(data.resp, 'Reordenar Documento');
                //                $('#gridDocumentos').dxDataGrid({ dataSource: DocumentosDataSource });
                //                $("#PopupReordenar").dxPopup("instance").hide();
                //            }
                //        },
                //        error: function (xhr, textStatus, errorThrown) {
                //            DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Reordenar documento');
                //        }
                //    });
            } else DevExpress.ui.dialog.alert('Ocurrió un problema : Debe cambiar el orden del docuemnto', 'Reordenar documento');
        }
    });

    var popupFol = $("#PopupFoliado").dxPopup({
        width: 700,
        height: "auto",
        hoverStateEnabled: true,
        title: "Foliado de documentos"
    }).dxPopup("instance");

    var popupReor = $("#PopupReordenar").dxPopup({
        width: 400,
        height: "auto",
        hoverStateEnabled: true,
        title: "Reordenar documento"
    }).dxPopup("instance");

    $("#popDocumento").dxPopup({
        width: 900,
        height: 800,
        showTitle: true,
        title: "Visualizar Documento",
        dragEnabled: false,
        closeOnOutsideClick: true,
    });

    var MostrarDocumento = function (IdDocumento) {
        var _popup = $("#popDocumento").dxPopup("instance");
        _popup.show();
        $("#Documento").attr("src", $('#SIM').data('url') + "Utilidades/LeeDoc?IdDocumento=" + IdDocumento);
    };

    var popupInd = null;

    var showIndices = function (data) {
        Indices = data;
        if (popupInd) {
            popupInd.option("contentTemplate", popupOptInd.contentTemplate.bind(this));
        } else {
            popupInd = $("#PopupIndicesDoc").dxPopup(popupOptInd).dxPopup("instance");
        }
        popupInd.show();
    };

    var popupOptInd = {
        width: 700,
        height: 900,
        hoverStateEnabled: true,
        title: "Indices del documento",
        visible: false,
        closeOnOutsideClick: true,
        contentTemplate: function (container) {
            var scrollView = $("<div id='scrollView'></div>");
            var divIni = $("<div></div>");
            scrollView.append(divIni);
            scrollView.dxScrollView({
                height: '100%',
                width: '100%'
            });
            var Content = "<table class='table table-sm' style='font-size: 10px;'><thead><tr><th scope='col'>NOMBRE DEL ÍNDICE</th><th scope='col'>VALOR</th></tr></thead><tbody>";
            Indices.forEach(function (indice, index) {
                Content += "<tr><th scope='row'>" + indice.INDICE + "</th><td>" + indice.VALOR + "</td></tr>";
            });
            divIni.html("<p><b>Indices del documento " + NroDocumento + " de la carpeta " + Carpeta + " del expediente " + Expediente + "</b></p><br />" + Content);
            container.append(scrollView);

            return container;
        }
    };

    $("#popupBuscaDoc").dxPopup({
        width: 900,
        height: 850,
        showTitle: true,
        title: "Buscar Documento"
    });
});


var TomosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"N_TOMO","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'GestionDocumental/api/ExpedientesApi/ObtieneTomos', {
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
            IdExpediente: IdExpediente
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var DocumentosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"ORDEN","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'GestionDocumental/api/ExpedientesApi/ObtieneDocsTomo', {
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
            IdTomo: IdTomo
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

function SeleccionaDocumento(Documentos) {
    var _popup = $("#popupBuscaDoc").dxPopup("instance");
    _popup.hide();
    var Sel = $("#grdListaTomos").dxDataGrid("instance").getSelectedRowsData()[0];
    if (Documentos.length > 0 != "" && Sel.ID_TOMO != "") {
        var ListaDocs = JSON.stringify(Documentos);
        var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/AsociaDocumento";
        $.getJSON(_Ruta, { ListaIdDocumentos: ListaDocs, IdTomo: Sel.ID_TOMO })
            .done(function (data) {
                if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Asociar documento');
                else {
                    $('#gridDocumentos').dxDataGrid({ dataSource: DocumentosDataSource });
                    $("#TituloDocumentos").text("");
                    DevExpress.ui.dialog.alert('El documento ' + Documento + ' se asoció correctamente al expediente', 'Asociar documento');
                }
            }).fail(function (jqxhr, textStatus, error) {
                DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Asociar documento');
            });
    }
}

