const tabs = [
    {
        id: 0,
        text: 'Mensajes',
        icon: 'comment'
    },
    {
        id: 1,
        text: 'Datos generales',
        icon: 'event'
    },
    {
        id: 2,
        text: 'Documentos',
        icon: 'doc'
    },
    {
        id: 3,
        text: 'Temporales',
        icon: 'folder'
    },
    {
        id: 4,
        text: 'Ruta',
        icon: 'box'
    },
    {
        id: 5,
        text: 'VITAL',
        icon: 'fieldchooser'
    }
];

const impprtancia = ['Normal', 'Media', 'Alta'];
const fechas = { weekday: 'long', year: 'numeric', month: 'short', day: 'numeric', hour: 'numeric', minute: 'numeric', second: 'numeric' };
function updateQueryStringParameter(uri, key, value) {
    var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
    var separator = uri.indexOf('?') !== -1 ? "&" : "?";
    if (uri.match(re)) {
        return uri.replace(re, '$1' + key + "=" + value + '$2');
    }
    else {
        return uri + separator + key + "=" + value;
    }
}

var version = 1;
var IdDocumento = -1;


$(document).ready(function () {
    var Vital = $("#NumVital").text() != "undefined" ? $("#NumVital").text() : "-1";
    $("#sebDetalleTramite").dxTabs({
        dataSource: tabs,
        selectedIndex: 0,
        onContentReady: function (e) {
            $("#PanelMensajes").show();
            $("#PanelDatos").hide();
            $("#PanelDocs").hide();
            $("#PanelTemp").hide();
            $("#PanelRuta").hide();
            $("#PanelVital").hide();
            SelTipo = 0;
        },
        onItemClick(e) {
            switch (e.itemData.id) {
                case 0:
                    $("#PanelMensajes").show();
                    $("#PanelDatos").hide();
                    $("#PanelDocs").hide();
                    $("#PanelTemp").hide();
                    $("#PanelRuta").hide();
                    $("#PanelVital").hide();
                    break;
                case 1:
                    $("#PanelMensajes").hide();
                    $("#PanelDatos").show();
                    $("#PanelDocs").hide();
                    $("#PanelTemp").hide();
                    $("#PanelRuta").hide();
                    $("#PanelVital").hide();
                    break;
                case 2:
                    $("#PanelMensajes").hide();
                    $("#PanelDatos").hide();
                    $("#PanelDocs").show();
                    $("#PanelTemp").hide();
                    $("#PanelRuta").hide();
                    $("#PanelVital").hide();
                    break;
                case 3:
                    $("#PanelMensajes").hide();
                    $("#PanelDatos").hide();
                    $("#PanelDocs").hide();
                    $("#PanelTemp").show();
                    $("#PanelRuta").hide();
                    $("#PanelVital").hide();
                    break;
                case 4:
                    $("#PanelMensajes").hide();
                    $("#PanelDatos").hide();
                    $("#PanelDocs").hide();
                    $("#PanelTemp").hide();
                    $("#PanelRuta").show();
                    $("#PanelVital").hide();
                    break;
                case 5:
                    $("#PanelMensajes").hide();
                    $("#PanelDatos").hide();
                    $("#PanelDocs").hide();
                    $("#PanelTemp").hide();
                    $("#PanelRuta").hide();
                    $("#PanelVital").show();
                    break;
            }
        }
    }).dxTabs('instance');

    $("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
        shadingColor: "rgba(0,0,0,0.4)",
    });

    $("#grdMensajes").dxDataGrid({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "FECHA",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + 'api/UtilidadesApi/MensajesTra?CodTramite=' + CodTramite);
                }
            })
        }),
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            enabled: false
        },
        selection: {
            mode: 'none'
        },
        remoteOperations: true,
        hoverStateEnabled: true,
        columns: [
            { dataField: "FECHA", width: '15%', caption: 'Fecha', dataType: 'date', format: 'dd/MM/yyyy HH:mm' },
            { dataField: 'ACTIVIDAD', width: '20%', caption: 'Actividad en la que fué generado el Comentario', dataType: 'string' },
            { dataField: 'FUNCIONARIO', width: '30%', caption: 'Funcionario que elaboró el Comentario', dataType: 'string' },
            { dataField: 'COMENTARIO', width: '30%', caption: 'Comentario', dataType: 'string' },
            { dataField: 'IMPORTANCIA', width: '5%', caption: 'Importancia', dataType: 'string' }
        ],
        onRowPrepared: function (e) {
                e.rowElement.css("font-size", "10px");
                e.rowElement.css("white-space", "normal");
        }
    });

    $("#grdIndicesTra").dxDataGrid({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "INDICE",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + 'api/UtilidadesApi/IndicesTra?CodTramite=' + CodTramite);
                }
            })
        }),
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
        selection: {
            mode: 'none'
        },
        remoteOperations: true,
        hoverStateEnabled: true,
        columns: [
            { dataField: "INDICE", width: '30%', caption: 'Indice', dataType: 'string' },
            { dataField: 'VALOR', width: '70%', caption: 'Valor', dataType: 'string' },
        ]
    });

    $("#btnEditIndicesTra").dxButton({
        icon: 'edit',
        hint: 'modificar indices del tramite',
        onClick: function (e) {
            var _Ruta = $("#SIM").data("url") + 'Utilidades/PuedeEditarIndicesTra';
            $.getJSON(_Ruta)
                .done(function (data) {
                    if (data.returnvalue) {
                        var _Ruta = $('#SIM').data('url') + "api/UtilidadesApi/EditarIndicesTramite";
                        $.getJSON(_Ruta, { CodTramite: CodTramite })
                            .done(function (data) {
                                if (data != null) {
                                    if (data.length > 0) {
                                        $("#PanelIndicesTra").hide();
                                        $("#PanelEditIndicesTra").show();
                                        AsignarIndicesTra(data);
                                    } else {
                                        DevExpress.ui.dialog.alert('El proceso no posee indices para el trámite!', 'Detalle del trámite');
                                    }
                                }
                            }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Eliminar proceso');
                            });
                    } else {
                        DevExpress.ui.dialog.alert('Usted no posee permisos para modificar índices de trámite!', 'Detalle del trámite');
                    }
                }).fail(function (jqxhr, textStatus, error) {
                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Detalle del trámite');
                });
        }
    });

    $("#btnGuardaIndicesTra").dxButton({
        icon: 'save',
        hint: 'Guardar indices del trámite',
        onClick: function (e) {
            var Indices = indicesTramiteStore._array;
            var params = { CodTramite: CodTramite, Indices: Indices };
            var _Ruta = $('#SIM').data('url') + "api/UtilidadesApi/GuardaindicesTramite";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                beforeSend: function () { },
                success: function (data) {
                    if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Detalle del trámite');
                    else {
                        DevExpress.ui.dialog.alert('Indices Guardados correctamente', 'Detalle del trámite');
                        $('#grdIndicesTra').dxDataGrid("instance").refresh();
                        $("#PanelIndicesTra").show();
                        $("#PanelEditIndicesTra").hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Detalle del trámite');
                }
            });           
        }
    });

    $("#btnCancelIndices").dxButton({
        icon: 'revert',
        hint: 'Cancelar modificar indices del trámite',
        onClick: function (e) {
            $("#PanelIndicesTra").show();
            $("#PanelEditIndicesTra").hide();
        }
    });

    $("#popAdjuntosDocumento").dxPopup({
        title: "Adjunto del documento",
        fullScreen: false,
    });

    $("#grdDocumentos").dxDataGrid({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "CODDOC",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + 'api/UtilidadesApi/DocsTra?CodTramite=' + CodTramite);
                }
            })
        }),
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            pageSize: 10
        },
        selection: {
            mode: 'single'
        },
        remoteOperations: true,
        hoverStateEnabled: true,
        columns: [
            { dataField: "ID_DOCUMENTO", visible: false },
            { dataField: "CODDOC", width: '0%', caption: 'Orden', dataType: 'number', visible: false },
            { dataField: 'SERIE', width: '40%', caption: 'Unidad Documental', dataType: 'string' },
            { dataField: 'FECHA', width: '25%', caption: 'Fecha Digitalización', dataType: 'date', format: 'MMM dd yyyy HH:mm' },
            {
                dataField: 'ESTADO', width: '15%', caption: 'Anulado', dataType: 'string',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    var _RutaDoc = $("#SIM").data("url") + "GestionDocumental/api/DocumentosApi/ObtieneDocumento?IdDocumento=" + options.data.ID_DOCUMENTO;
                    $.getJSON(_RutaDoc, function (data, status) {
                        if (status === "success") {
                            if (data.ESTADO == "Anulado" || data.ESTADO == "En proceso") {
                                var _Text = data.ESTADO == "Anulado" ? "Si" : data.ESTADO == "En proceso" ? "Proceso" : "";
                                var _hint = data.ESTADO == "Anulado" ? "Documento Anulado" : data.ESTADO == "En proceso" ? "En proceso de Anulación" : "";
                                $('<div/>').dxButton({
                                    text: _Text,
                                    hint: _hint,
                                    onClick: function (e) {
                                        var Documento = options.data.ID_DOCUMENTO;
                                        var _Ruta = $('#SIM').data('url') + "Utilidades/MotivoDevolucion?IdDocumento=" + options.data.ID_DOCUMENTO;
                                        $.getJSON(_Ruta, function (data, status) {
                                            if (status === "success") {
                                                popupOpcAnu = {
                                                    width: 500,
                                                    height: 300,
                                                    contentTemplate: function () {
                                                        return $("<div>").append(
                                                            $("<p>Motivo de la anulación:  <strong>" + data.Motivo + "</strong></p>"),
                                                            $("<br />"),
                                                            $("<p>Solicitud inicial:  " + data.Causa + "</p>"),
                                                            $("<br />"),
                                                            $("<p>Fecha Anulación:  <strong>" + ((data.Fecha == "N") ? '' : new Date(data.Fecha).toLocaleDateString('es-CO', fechas)) + "</strong></p>"),
                                                            $("<br />"),
                                                            $("<p>Trámite Anulación:  <strong>" + data.TraAnula + "</strong></p>")
                                                        );
                                                    },
                                                    showTitle: true,
                                                    title: "Motivo de la anulación",
                                                    visible: false,
                                                    dragEnabled: false,
                                                    closeOnOutsideClick: true
                                                };
                                                popup = $("#PopupAnula").dxPopup(popupOpcAnu).dxPopup("instance");
                                                $('#PopupAnula').css({ 'visibility': 'visible' });
                                                $("#PopupAnula").fadeTo("slow", 1);
                                                popup.show();
                                            }
                                        });
                                    }
                                }).appendTo(container);
                            } else {
                                $('<div/>').append(data.ESTADO).appendTo(container);
                            }
                        }
                    });
                }
            },
            {
                dataField: 'ADJUNTO', width: '10%', caption: 'Adjunto', dataType: 'string',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.data.ADJUNTO == "Si") {
                        $('<div/>').dxButton({
                            text: 'Si',
                            hint: 'Ver Adjuntos',
                            onClick: function (e) {
                                var Documento = options.data.ID_DOCUMENTO;
                                var _Ruta = $('#SIM').data('url') + "Utilidades/FuncionarioPoseePermiso?IdDocumento=" + options.data.ID_DOCUMENTO;
                                $.getJSON(_Ruta, function (result, status) {
                                    if (status === "success") {
                                        if (result.returnvalue) {
                                            var AdjuntoInstance = $("#popAdjuntosDocumento").dxPopup("instance");
                                            AdjuntoInstance.option('title', 'Adjunto del documento - ' + Documento);
                                            AdjuntoInstance.show();
                                            $('#frmAdjuntoDocumento').attr('src', null);
                                            $('#frmAdjuntoDocumento').attr('src', 'https://webservices.metropol.gov.co/FileManager/FileManager.aspx?id=.' + Documento);
                                        } else {
                                            DevExpress.ui.notify("No posee permiso para ver este tipo de documento");
                                        }
                                    }
                                });
                            }
                        }).appendTo(container);
                    }
                }
            },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'doc',
                        hint: 'Ver el documento',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "Utilidades/FuncionarioPoseePermiso?IdDocumento=" + options.data.ID_DOCUMENTO;
                            $.getJSON(_Ruta, function (result, status) {
                                if (status === "success") {
                                    if (result.returnvalue) {
                                        window.open($('#SIM').data('url') + "Utilidades/LeeDoc?IdDocumento=" + options.data.ID_DOCUMENTO, "Documento " + options.data.ID_DOCUMENTO, "width= 900,height=800,scrollbars = yes, location = no, toolbar = no, menubar = no, status = no");
                                    }
                                    else {
                                        DevExpress.ui.notify("No posee permiso para ver este tipo de documento");
                                    }
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
                IdDocumento = data.ID_DOCUMENTO;
                $('#grdIndicesDoc').dxDataGrid("instance").refresh();
            }
        }
    });

    $("#grdDocsTemp").dxDataGrid({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "IDDOCUMENTO",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + 'api/UtilidadesApi/DocsTemp?CodTramite=' + CodTramite + "&Orden=" + Orden);
                }
            })
        }),
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            pageSize: 10
        },
        selection: {
            mode: 'single'
        },
        remoteOperations: true,
        hoverStateEnabled: true,
        columns: [
            { dataField: "IDDOCUMENTO", visible: false },
            { dataField: "FECHAVER", width: '15%', caption: 'fecha', dataType: 'date', format: 'MMM dd yyyy HH:mm' },
            { dataField: 'DESCRIPCION', width: '30%', caption: 'Descripción', dataType: 'string' },
            { dataField: 'VERSION', width: '20%', caption: 'Versión', dataType: 'number' },
            { dataField: 'FUNCIONARIO', width: '25%', caption: 'Funcionario', dataType: 'string' },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.data.PUEDEVER == "SI") {
                        $('<div/>').dxButton({
                            icon: 'doc',
                            hint: 'Ver el documento',
                            onClick: function (e) {
                                window.open($('#SIM').data('url') + "Utilidades/LeeTemporal?IdDocumento=" + options.data.IDDOCUMENTO, "Documento Temporal" + options.data.DESCRIPCION, "width= 900,height=800,scrollbars = yes, location = no, toolbar = no, menubar = no, status = no");
                            }
                        }).appendTo(container);
                    }
                }
            },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.data.ESTTRA == "0") {
                        if (options.data.PUEDEELIMINAR == "SI") {
                            if (options.data.ESULTVER) {
                                $('<div/>').dxButton({
                                    icon: 'trash',
                                    hint: 'Eliminar el documento',
                                    onClick: function (e) {
                                        var result = DevExpress.ui.dialog.confirm('¿Está seguro de eliminar este documento temporal?', 'Confirmación');
                                        result.done(function (dialogResult) {
                                            if (dialogResult) {
                                                var _Ruta = $('#SIM').data('url') + "Utilidades/EliminaTemporal?IdDocumento=" + options.data.IDDOCUMENTO;
                                                $.getJSON(_Ruta, function (result, status) {
                                                    if (status === "success") {
                                                        if (result.returnvalue) {
                                                            DevExpress.ui.notify("Documento temporal eliminado");
                                                            $('#grdDocsTemp').dxDataGrid("instance").refresh();
                                                        } else {
                                                            DevExpress.ui.notify("No se pudo eliminar el documento temporal");
                                                        }
                                                    }
                                                });                                              
                                            }
                                        });
                                        $('#grdDocsTemp').dxDataGrid("instance").refresh();
                                    }
                                }).appendTo(container);
                            }
                        }
                    }
                }
            }
        ]
    });

    $("#grdDocsVital").dxDataGrid({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Documento",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + 'api/UtilidadesApi/DocsVital?Vital=' + Vital);
                }
            })
        }),
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            pageSize: 10
        },
        selection: {
            mode: 'single'
        },
        columns: [
            { dataField: "Documento", width: '75%', caption: 'Archivo', dataType: 'string' },
            { dataField: "ID_RADICACION", with: '15%', caption: 'Radicado VITAL', dataType: 'number' },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'doc',
                        hint: 'Ver el documento',
                        onClick: function (e) {
                            window.open($('#SIM').data('url') + "Utilidades/LeeDocVital?IdRadicadoVital=" + options.data.ID_RADICACION + "&NombreArchivo=" + options.data.Documento, "Documento " + options.data.ID_DOCUMENTO, "width= 900,height=800,scrollbars = yes, location = no, toolbar = no, menubar = no, status = no");
                        }
                    }).appendTo(container);
                }
            },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'upload',
                        hint: 'Agregar a Temporales',
                        onClick: function (e) {
                            var DocsTemp = $("#grdDocsTemp").dxDataGrid("instance").getDataSource().store().load().done(r => r.DESCRIPCION);
                            if (DocsTemp != null)
                            {
                                for (var i = 0; i < DocsTemp.length; i++) {
                                    if (DocsTemp[i].toUpperCase() == options.data.Documento.toUpperCase())
                                    {
                                        DevExpress.ui.dialog.alert("Error: Este documento ya existe en los temporales!", "Documentos temporales");
                                        return;
                                    }
                                }
                            }
                            var params = { CodTramite: CodTramite, Orden: Orden, DocumentoVital: options.data.Documento, Descripcion: options.data.Documento, RadicadoVital: options.data.ID_RADICACION };
                            var _Ruta = $('#SIM').data('url') + "api/UtilidadesApi/SubeDocVital";
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
                                        $('#grdDocsTemp').dxDataGrid("instance").refresh();
                                    }
                                },
                                error: function (xhr, textStatus, errorThrown) {
                                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                                }
                            });
                        }
                    }).appendTo(container);
                }
            }
        ]
    });

    $("#grdIndicesDoc").dxDataGrid({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "CODINDICE",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + 'api/UtilidadesApi/IndicesDoc?IdDocumento=' + IdDocumento);
                }
            })
        }),
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            enabled: false
        },
        selection: {
            mode: 'none'
        },
        columns: [
            { dataField: "CODINDICE", visible: false },
            { dataField: "INDICE", width: '30%', caption: 'Indice', dataType: 'string' },
            { dataField: 'VALOR', width: '70%', caption: 'Valor', dataType: 'string' },
        ]
    });

    //Inicio Editar indices documento

    $("#btnEditIndicesDoc").dxButton({
        icon: 'edit',
        hint: 'modificar indices del documento',
        onClick: function (e) {
            var _Ruta = $("#SIM").data("url") + 'Utilidades/PuedeEditarIndicesDoc?IdDoc=' + IdDocumento;
            $.getJSON(_Ruta)
                .done(function (data) {
                    if (data.returnvalue) {
                        var _Ruta = $('#SIM').data('url') + "api/UtilidadesApi/EditarIndicesDocumento";
                        $.getJSON(_Ruta, { IdDocumento: IdDocumento })
                            .done(function (data) {
                                if (data != null) {
                                    if (data.length > 0) {
                                        $("#PanelIndicesDoc").hide();
                                        $("#PanelEditIndicesDoc").show();
                                        AsignarIndicesDoc(data);
                                    } else {
                                        DevExpress.ui.dialog.alert('La unidad documnental no posee indices para el documento!', 'Detalle del trámite');
                                    }
                                }
                            }).fail(function (jqxhr, textStatus, error) {
                                DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Detalle del trámite');
                            });
                    } else {
                        DevExpress.ui.dialog.alert('Usted no posee permisos para modificar índices del documento!', 'Detalle del trámite');
                    }
                }).fail(function (jqxhr, textStatus, error) {
                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Detalle del trámite');
                });
        }
    });

    $("#btnGuardaIndicesDoc").dxButton({
        icon: 'save',
        hint: 'Guardar indices del documento',
        onClick: function (e) {
            var Indices = indicesSerieDocumentalStore._array;
            var params = { IdDocumento: IdDocumento, Indices: Indices };
            var _Ruta = $('#SIM').data('url') + "api/UtilidadesApi/GuardaindicesDocumento";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                beforeSend: function () { },
                success: function (data) {
                    if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Detalle del trámite');
                    else {
                        DevExpress.ui.dialog.alert('Indices Guardados correctamente', 'Detalle del trámite');
                        $('#grdIndicesDoc').dxDataGrid("instance").refresh();
                        $("#PanelIndicesDoc").show();
                        $("#PanelEditIndicesDoc").hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Detalle del trámite');
                }
            });
        }
    });

    $("#btnCancelIndDoc").dxButton({
        icon: 'revert',
        hint: 'Cancelar modificar indices del documento',
        onClick: function (e) {
            $("#PanelIndicesDoc").show();
            $("#PanelEditIndicesDoc").hide();
        }
    });

    function AsignarIndicesDoc(indices) {
        opcionesLista = [];

        indicesSerieDocumentalStore = new DevExpress.data.LocalStore({
            key: 'CODINDICE',
            data: indices,
            name: 'indicesSerieDocumental'
        });

        indices.forEach(function (valor, indice, array) {
            if (valor.TIPO == 5 && valor.ID_LISTA != null) {

                if (opcionesLista.findIndex(ol => ol.idLista == valor.ID_LISTA) == -1) {
                    opcionesLista.push({ idLista: valor.ID_LISTA, datos: null, cargado: false });
                }
            }
        });

        if (opcionesLista.length == 0) {
            CargarGridIndicesDoc();
        } else {
            opcionesLista.forEach(function (valor, indice, array) {
                $.getJSON($('#SIM').data('url') + 'Tramites/api/ProyeccionDocumentoApi/ObtenerIndiceValoresLista?id=' + valor.idLista).done(function (data) {
                    var index = opcionesLista.findIndex(ol => ol.idLista == valor.idLista);
                    opcionesLista[index].datos = data;
                    opcionesLista[index].cargado = true;

                    var finalizado = true;

                    opcionesLista.forEach(function (v, i, a) {
                        finalizado = finalizado && v.cargado;
                    });

                    if (finalizado) {
                        CargarGridIndicesDoc();
                    }
                });
            });
        }
    }

    function CargarGridIndicesDoc() {
        $("#grdEditIndicesDoc").dxDataGrid({
            dataSource: indicesSerieDocumentalStore,
            allowColumnResizing: true,
            allowSorting: false,
            showRowLines: true,
            rowAlternationEnabled: true,
            showBorders: true,
            height: '100%',
            loadPanel: { text: 'Cargando Datos...' },
            paging: {
                pageSize: 0,
            },
            pager: {
                showPageSizeSelector: false,
            },
            filterRow: {
                visible: false,
            },
            groupPanel: {
                visible: false,
            },
            editing: {
                mode: "cell",
                allowUpdating: true,
                allowAdding: false,
                allowDeleting: false

            },
            selection: {
                mode: 'single'
            },
            scrolling: {
                showScrollbar: 'always',
            },
            wordWrapEnabled: true,
            columns: [
                { dataField: "CODINDICE", dataType: 'number', visible: false, },
                { dataField: "INDICE", caption: 'INDICE', dataType: 'string', width: '40%', visible: true, allowEditing: false },
                {
                    dataField: 'VALOR', caption: 'VALOR', dataType: 'string', allowEditing: true,
                    cellTemplate: function (cellElement, cellInfo) {
                        switch (cellInfo.data.TIPO) {
                            case 0: // TEXTO
                            case 1: // NUMERO
                            case 3: // HORA
                            case 8: // DIRECCION
                                if (cellInfo.data.VALOR != null) {
                                    cellElement.html(cellInfo.data.VALOR);
                                }
                                break;
                            case 2: // FECHA
                                if (cellInfo.data.VALOR != null) {
                                    cellElement.html(cellInfo.data.VALOR);
                                }
                                break;
                            case 4: // BOOLEAN
                                if (cellInfo.data.VALOR != null)
                                    cellElement.html(cellInfo.data.VALOR == 'S' ? 'SI' : 'NO');
                                break;
                            default: // OTRO
                                if (cellInfo.data.VALOR != null)
                                    cellElement.html(cellInfo.data.VALOR);
                                break;
                        }
                    },
                    editCellTemplate: function (cellElement, cellInfo) {
                        switch (cellInfo.data.TIPO) {
                            case 1: // NUMERO
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                $(div).dxNumberBox({
                                    value: cellInfo.data.VALOR,
                                    width: '100%',
                                    showSpinButtons: false,
                                    onValueChanged: function (e) {
                                        cellInfo.setValue(e.value);
                                    },
                                });
                                break;
                            case 0: // TEXTO
                            case 3: // HORA
                            case 8: // DIRECCION
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                $(div).dxTextBox({
                                    value: cellInfo.data.VALOR,
                                    width: '100%',
                                    onValueChanged: function (e) {
                                        cellInfo.setValue(e.value);
                                    },
                                });
                                break;
                            case 2: // FECHA
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                var fecha = null;
                                if (cellInfo.data.VALOR != null && cellInfo.data.VALOR.trim() != '') {
                                    var partesFecha;
                                    if (cellInfo.data.VALOR.includes('/')) partesFecha = cellInfo.data.VALOR.split('/');
                                    else partesFecha = cellInfo.data.VALOR.split('-');
                                    fecha = new Date(parseInt(partesFecha[2]), parseInt(partesFecha[1]) - 1, parseInt(partesFecha[0]));
                                    $(div).dxDateBox({
                                        type: 'date',
                                        width: '100%',
                                        displayFormat: 'dd/MM/yyyy',
                                        value: fecha,
                                        onValueChanged: function (e) {
                                            //cellInfo.setValue(e.value);
                                            cellInfo.setValue(e.value.getDate() + '/' + (e.value.getMonth() + 1) + '/' + e.value.getFullYear());
                                        },
                                    });
                                } else {
                                    $(div).dxDateBox({
                                        type: 'date',
                                        width: '100%',
                                        displayFormat: 'dd/MM/yyyy',
                                        onValueChanged: function (e) {
                                            //cellInfo.setValue(e.value);
                                            cellInfo.setValue(e.value.getDate() + '/' + (e.value.getMonth() + 1) + '/' + e.value.getFullYear());
                                        },
                                    });
                                }

                                break;
                            case 4: // SI/NO
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                $(div).dxSelectBox({
                                    dataSource: siNoOpciones,
                                    width: '100%',
                                    displayExpr: "Nombre",
                                    valueExpr: "ID",
                                    placeholder: "[SI/NO]",
                                    value: cellInfo.data.VALOR,
                                    onValueChanged: function (e) {
                                        cellInfo.setValue(e.value);
                                        $("#grdIndices").dxDataGrid("saveEditData");
                                    },
                                });
                                break;
                            case 5: // Lista
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                let itemsLista = opcionesLista[opcionesLista.findIndex(ol => ol.idLista == cellInfo.data.ID_LISTA)].datos;

                                if (cellInfo.data.ID_LISTA != null) {
                                    $(div).dxSelectBox({
                                        items: itemsLista,
                                        width: '100%',
                                        placeholder: "[SELECCIONAR OPCION]",
                                        value: (cellInfo.data.VALOR == null ? null : itemsLista[itemsLista.findIndex(ls => ls.NOMBRE == cellInfo.data.VALOR)].ID),
                                        displayExpr: 'NOMBRE',
                                        valueExpr: 'ID',
                                        searchEnabled: true,
                                        onValueChanged: function (e) {
                                            cellInfo.data.ID_VALOR = e.value;
                                            cellInfo.setValue(itemsLista[itemsLista.findIndex(ls => ls.ID == e.value)].NOMBRE);
                                            $("#grdIndices").dxDataGrid("saveEditData");
                                        },
                                    });
                                } else {
                                    $(div).dxTextBox({
                                        value: cellInfo.data.VALOR,
                                        width: '100%',
                                        onValueChanged: function (e) {
                                            cellInfo.setValue(e.value);
                                        },
                                    });
                                }
                                break;
                            default: // Otro
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                $(div).dxTextBox({
                                    value: cellInfo.data.VALOR,
                                    width: '100%',
                                    onValueChanged: function (e) {
                                        cellInfo.setValue(e.value);
                                    },
                                });
                                break;
                        }
                    }
                }, {
                    dataField: "OBLIGA",
                    caption: 'R',
                    width: '40px',
                    dataType: 'string',
                    visible: true,
                    allowEditing: false,
                    cellTemplate: function (cellElement, cellInfo) {
                        cellElement.css('text-align', 'center');
                        cellElement.css('color', 'red');
                        if (cellInfo.data.OBLIGA == 1)
                            cellElement.html('*');

                    }
                }
            ]
        });
    }


    //Fin Editar indices documento


    $("#grdRutaTra").dxDataGrid({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "ORDEN",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + 'api/UtilidadesApi/RutaTra?CodTramite=' + CodTramite);
                }
            })
        }),
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            enabled: false
        },
        selection: {
            mode: 'none'
        },
        remoteOperations: true,
        hoverStateEnabled: true,
        columns: [
            { dataField: 'ORDEN', width: '5%', caption: 'Orden', dataType: 'number' },
            { dataField: 'ACTIVIDAD', width: '15%', caption: 'Actividad', dataType: 'string' },
            { dataField: 'ENVIADAA', width: '15%', caption: 'Enviada a', dataType: 'string' },
            { dataField: 'CONCOPIAA', width: '15%', caption: 'Con copia a', dataType: 'string' },
            { dataField: 'COMENTARIO', width: '20%', caption: 'Comentario', dataType: 'string' },
            { dataField: "FECHAINICIAL", width: '11%', caption: 'Fecha Inicial', dataType: 'date', format: 'dd/MM/yyyy HH:mm' },
            { dataField: "FECHAFINAL", width: '11%', caption: 'Fecha Final', dataType: 'date', format: 'dd/MM/yyyy HH:mm' },
            { dataField: 'ESTADO', width: '8%', caption: 'Estado', dataType: 'string' },
            { dataField: 'TIPO', width: '8%', caption: 'Tipo', dataType: 'string' }
        ],
        onRowPrepared: function (e) {
                if (e.rowType !== "data")
                    return
                e.rowElement.css("font-size", "10px");
                e.rowElement.css("white-space", "normal");
        }
    });

    $("#grdRutaVital").dxDataGrid({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "CODIGO",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + 'api/UtilidadesApi/RutaVital?CodTramite=' + CodTramite);
                }
            })
        }),
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            enabled: false
        },
        selection: {
            mode: 'none'
        },
        remoteOperations: true,
        hoverStateEnabled: true,
        columns: [
            { dataField: 'ACTIVIDAD', width: '35%', caption: 'Actividad', dataType: 'string' },
            { dataField: 'FECHA', width: '15%', caption: 'Fecha', dataType: 'date', format: 'dd/MM/yyyy HH:mm' },
            { dataField: 'COMENTARIO', width: '50%', caption: 'Comentario', dataType: 'string' }
        ]
    });

    var VersionTemp = $("#txtVersionTemp").dxTextBox({
        value: version,
        disabled: true
    }).dxTextBox("instance");

    var DescripcionTemp = $("#txtDescripTemp").dxTextBox({
        value: '',
    }).dxValidator({
        validationGroup: "TempGroup",
        validationRules: [{
            type: "required",
            message: "La descripción es obligatoria"
        }]
    }).dxTextBox("instance");

    $("#cmbDocumentoTemp").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "ID",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "api/UtilidadesApi/ListaTemp?CodTramite=" + CodTramite);
                }
            })
        }),
        displayExpr: "DESCRIPCION",
        noDataText: "Aún no se han adicionado documentos temporales a este trámite",
        placeholder: " --- Nuevo Documento --- ",
        onValueChanged: function (data) {
            if (data.value != null) {
                    var descrip = data.value.DESCRIPCION;
                    version = data.value.VERSION;
                    VersionTemp.option("value", version);
                    DescripcionTemp.option("value", descrip);
                    DescripcionTemp.option("disabled", true);
            } else {
                version = 1;
                VersionTemp.option("value", version);
                DescripcionTemp.option("disabled", false);
                DescripcionTemp.option("value", "");
            }
        }
    });

    $("#fuArchivoTemporal").dxFileUploader({
        allowedFileExtensions: [".doc", ".docx", ".pdf", ".xls", ".ppt", ".pptx" ],
        multiple: true,
        selectButtonText: 'Seleccionar Archivo Temporal',
        labelText: 'o arrastre un archivo aquí',
        uploadMode: "instantly",
        uploadUrl: $('#SIM').data('url') + 'Utilidades/CargarArchivoTemp?Tra=' + CodTramite,
        onUploadAborted: (e) => removeFile(e.file.name),
        onUploaded: function (e) {
            $("#loadPanel").dxLoadPanel('instance').hide();
        },
        onUploadStarted: function (e) {
            $("#loadPanel").dxLoadPanel('instance').show();
        },
        onUploadError: function (e) {
            $("#loadPanel").dxLoadPanel('instance').hide();
            DevExpress.ui.dialog.alert('Error Subiendo Archivo: ' + e.request.responseText, 'Documentos temporales');
        },
        onValueChanged: function (e) {
            var url = e.component.option('uploadUrl');
            url = updateQueryStringParameter(url, 'Version', VersionTemp.option("value"));
            e.component.option('uploadUrl', url);
        }
    });

    $("#btnGuardaTemp").dxButton({
        stylingMode: "contained",
        text: "Guarda documento temporal",
        icon: 'save',
        onClick: function () {
            DevExpress.validationEngine.validateGroup("TempGroup");
            var Version = VersionTemp.option("value");
            var Descripcion = DescripcionTemp.option("value");
            var params = { CodTramite: CodTramite, Orden: Orden, Version: Version, Descripcion: Descripcion };
            var _Ruta = $('#SIM').data('url') + "api/UtilidadesApi/GuardaTemporal";
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
                        $('#grdDocsTemp').dxDataGrid("instance").refresh();
                        popupTemp.hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    $("#btnNuevoTemporal").dxButton({
        stylingMode: "contained",
        text: "Nuevo documento temporal",
        icon: 'folder',
        onClick: function () {
            popupTemp.show();
        }
    });

    $("#btnTramitePadre").dxButton({
        text: TramitePadre,
        type: 'normal',
        onClick: function (e) {
            var left = (screen.width / 2) - 550;
            var top = (screen.height / 2) - 300;
            window.open($('#SIM').data('url') + "Utilidades/DetalleTramite?popup=true&CodTramite=" + TramitePadre + "&Orden=" + Orden, "Documento ", "width= 1100,height=600,scrollbars = yes, location = no, toolbar = no, menubar = no, status = no, top" + top + ",left=" + left);
        }
    });

    var popupTemp = $("#PopupTemporal").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Documento Temporal",
        dragEnabled: true,
    }).dxPopup("instance");

    var TxtMensaje = $('#txtMensaje').dxTextArea({
        value: '',
        height: 90
    }).dxTextArea("instance");

    var opcImpotancia = $('#opcImportMsg').dxRadioGroup({
        items: impprtancia,
        value: impprtancia[0],
        layout: 'horizontal',
    }).dxRadioGroup("instance");

    $("#btnGuardaMsg").dxButton({
        stylingMode: "contained",
        text: "Guarda Mensaje",
        icon: 'save',
        onClick: function () {
            var Mensaje = TxtMensaje.option("value");
            var Impotancia = opcImpotancia.option("value");
            var params = { CodTramite: CodTramite, Orden: Orden, Mensaje: Mensaje, Importancia: Impotancia };
            var _Ruta = $('#SIM').data('url') + "api/UtilidadesApi/GuardaMensaje";
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
                        $('#grdMensajes').dxDataGrid("instance").refresh();
                        popupMsg.hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    $("#btnNuevoMsg").dxButton({
        stylingMode: "contained",
        text: "Nuevo Mensaje",
        icon: 'tags',
        onClick: function () {
            popupMsg.show();
        }
    });

    var popupMsg = $("#PopupMensaje").dxPopup({
        width: 600,
        height: "auto",
        hoverStateEnabled: true,
        title: "Mensaje asociado al trámite",
        dragEnabled: true,
    }).dxPopup("instance");

    function AsignarIndicesTra(indices) {
        opcionesLista = [];

        indicesTramiteStore = new DevExpress.data.LocalStore({
            key: 'CODINDICE',
            data: indices,
            name: 'indicesTramite'
        });

        indices.forEach(function (valor, indice, array) {
            if (valor.TIPO == 5 && valor.ID_LISTA != null) {

                if (opcionesLista.findIndex(ol => ol.idLista == valor.ID_LISTA) == -1) {
                    opcionesLista.push({ idLista: valor.ID_LISTA, datos: null, cargado: false });
                }
            }
        });

        if (opcionesLista.length == 0) {
            CargarGridIndicesTra();
        } else {
            opcionesLista.forEach(function (valor, indice, array) {
                $.getJSON($('#SIM').data('url') + 'Tramites/api/ProyeccionDocumentoApi/ObtenerIndiceValoresLista?id=' + valor.idLista).done(function (data) {
                    var index = opcionesLista.findIndex(ol => ol.idLista == valor.idLista);
                    opcionesLista[index].datos = data;
                    opcionesLista[index].cargado = true;

                    var finalizado = true;

                    opcionesLista.forEach(function (v, i, a) {
                        finalizado = finalizado && v.cargado;
                    });

                    if (finalizado) {
                        CargarGridIndicesTra();
                    }
                });
            });
        }
    }

    function CargarGridIndicesTra() {
        $("#grdEditIndicesTra").dxDataGrid({
            dataSource: indicesTramiteStore,
            allowColumnResizing: true,
            allowSorting: false,
            showRowLines: true,
            rowAlternationEnabled: true,
            showBorders: true,
            height: '100%',
            loadPanel: { text: 'Cargando Datos...' },
            paging: {
                pageSize: 0,
            },
            pager: {
                showPageSizeSelector: false,
            },
            filterRow: {
                visible: false,
            },
            groupPanel: {
                visible: false,
            },
            editing: {
                mode: "cell",
                allowUpdating: true,
                allowAdding: false,
                allowDeleting: false

            },
            selection: {
                mode: 'single'
            },
            scrolling: {
                showScrollbar: 'always',
            },
            wordWrapEnabled: true,
            columns: [
                { dataField: "CODINDICE", dataType: 'number', visible: false, },
                { dataField: "INDICE", caption: 'INDICE', dataType: 'string', width: '40%', visible: true, allowEditing: false },
                { dataField: 'VALOR', caption: 'VALOR', dataType: 'string', allowEditing: true,
                    cellTemplate: function (cellElement, cellInfo) {
                        switch (cellInfo.data.TIPO) {
                            case 0: // TEXTO
                            case 1: // NUMERO
                            case 3: // HORA
                            case 8: // DIRECCION
                                if (cellInfo.data.VALOR != null) {
                                    cellElement.html(cellInfo.data.VALOR);
                                }
                                break;
                            case 2: // FECHA
                                if (cellInfo.data.VALOR != null) {
                                    cellElement.html(cellInfo.data.VALOR);
                                }
                                break;
                            case 4: // BOOLEAN
                                if (cellInfo.data.VALOR != null)
                                    cellElement.html(cellInfo.data.VALOR == 'S' ? 'SI' : 'NO');
                                break;
                            default: // OTRO
                                if (cellInfo.data.VALOR != null)
                                    cellElement.html(cellInfo.data.VALOR);
                                break;
                        }
                    },
                    editCellTemplate: function (cellElement, cellInfo) {
                        switch (cellInfo.data.TIPO) {
                            case 1: // NUMERO
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                $(div).dxNumberBox({
                                    value: cellInfo.data.VALOR,
                                    width: '100%',
                                    showSpinButtons: false,
                                    onValueChanged: function (e) {
                                        cellInfo.setValue(e.value);
                                    },
                                });
                                break;
                            case 0: // TEXTO
                            case 3: // HORA
                            case 8: // DIRECCION
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                $(div).dxTextBox({
                                    value: cellInfo.data.VALOR,
                                    width: '100%',
                                    onValueChanged: function (e) {
                                        cellInfo.setValue(e.value);
                                    },
                                });
                                break;
                            case 2: // FECHA
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                var fecha = null;
                                if (cellInfo.data.VALOR != null && cellInfo.data.VALOR.trim() != '') {
                                    var partesFecha = cellInfo.data.VALOR.split('/');
                                    fecha = new Date(parseInt(partesFecha[2]), parseInt(partesFecha[1]) - 1, parseInt(partesFecha[0]));

                                    $(div).dxDateBox({
                                        type: 'date',
                                        width: '100%',
                                        displayFormat: 'dd/MM/yyyy',
                                        value: fecha,
                                        onValueChanged: function (e) {
                                            //cellInfo.setValue(e.value);
                                            cellInfo.setValue(e.value.getDate() + '/' + (e.value.getMonth() + 1) + '/' + e.value.getFullYear());
                                        },
                                    });
                                } else {
                                    $(div).dxDateBox({
                                        type: 'date',
                                        width: '100%',
                                        displayFormat: 'dd/MM/yyyy',
                                        onValueChanged: function (e) {
                                            //cellInfo.setValue(e.value);
                                            cellInfo.setValue(e.value.getDate() + '/' + (e.value.getMonth() + 1) + '/' + e.value.getFullYear());
                                        },
                                    });
                                }

                                break;
                            case 4: // SI/NO
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                $(div).dxSelectBox({
                                    dataSource: siNoOpciones,
                                    width: '100%',
                                    displayExpr: "Nombre",
                                    valueExpr: "ID",
                                    placeholder: "[SI/NO]",
                                    value: cellInfo.data.VALOR,
                                    onValueChanged: function (e) {
                                        cellInfo.setValue(e.value);
                                        $("#grdIndices").dxDataGrid("saveEditData");
                                    },
                                });
                                break;
                            case 5: // Lista
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                let itemsLista = opcionesLista[opcionesLista.findIndex(ol => ol.idLista == cellInfo.data.ID_LISTA)].datos;

                                if (cellInfo.data.ID_LISTA != null) {
                                    $(div).dxSelectBox({
                                        items: itemsLista,
                                        width: '100%',
                                        placeholder: "[SELECCIONAR OPCION]",
                                        value: (cellInfo.data.VALOR == null ? null : itemsLista[itemsLista.findIndex(ls => ls.NOMBRE == cellInfo.data.VALOR)].ID),
                                        displayExpr: 'NOMBRE',
                                        valueExpr: 'ID',
                                        searchEnabled: true,
                                        onValueChanged: function (e) {
                                            cellInfo.data.ID_VALOR = e.value;
                                            cellInfo.setValue(itemsLista[itemsLista.findIndex(ls => ls.ID == e.value)].NOMBRE);
                                            $("#grdIndices").dxDataGrid("saveEditData");
                                        },
                                    });
                                } else {
                                    $(div).dxTextBox({
                                        value: cellInfo.data.VALOR,
                                        width: '100%',
                                        onValueChanged: function (e) {
                                            cellInfo.setValue(e.value);
                                        },
                                    });
                                }
                                break;
                            default: // Otro
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                $(div).dxTextBox({
                                    value: cellInfo.data.VALOR,
                                    width: '100%',
                                    onValueChanged: function (e) {
                                        cellInfo.setValue(e.value);
                                    },
                                });
                                break;
                        }
                    }
                }, {
                    dataField: "OBLIGA",
                    caption: 'R',
                    width: '40px',
                    dataType: 'string',
                    visible: true,
                    allowEditing: false,
                    cellTemplate: function (cellElement, cellInfo) {
                        cellElement.css('text-align', 'center');
                        cellElement.css('color', 'red');
                        if (cellInfo.data.OBLIGA == 1)
                            cellElement.html('*');

                    }
                }
            ]
        });
    }

});