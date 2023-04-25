var IdDocumento = -1;


$(document).ready(function () {
    $("#btnBuscarDoc").dxButton({
        text: "Documentos",
        icon: "search",
        type: "default",
        width: "190",
        onClick: function () {
            var _popup = $("#popupBuscaDoc").dxPopup("instance");
            _popup.show();
            $('#BuscarDoc').attr('src', $('#SIM').data('url') + 'Utilidades/BuscarDocumento?popup=true');
        }
    });

    $("#popupBuscaDoc").dxPopup({
        width: 900,
        height: 800,
        showTitle: true,
        title: "Buscar Documentos del SIM"
    });

    $("#popAdjuntosDocumento").dxPopup({
        title: "Adjunto del documento",
        fullScreen: false,
    });

    $("#grdDocs").dxDataGrid({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "ID_DOCUMENTO",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + 'GestionDocumental/api/DocumentosApi/ObtieneDocumento?IdDocumento=' + IdDocumento);
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
            { dataField: "CODDOC", width: '5%', caption: 'Orden', dataType: 'number' },
            { dataField: 'SERIE', width: '25%', caption: 'Unidad Documental', dataType: 'string' },
            { dataField: 'FECHA', width: '12%', caption: 'Fecha Digitalización', dataType: 'date', format: 'MMM dd yyyy HH:mm' },
            {
                dataField: 'ESTADO', width: '12%', caption: 'Anulado', dataType: 'string',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.data.ESTADO == "Anulado") {
                        $('<div/>').dxButton({
                            text: 'Si',
                            hint: 'Documento Anulado',
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
                    } else if (options.data.ESTADO == "En proceso") {
                        $('<div/>').dxButton({
                            text: 'Proceso',
                            hint: 'En proceso de Anulación',
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
                    }
                    else {
                        $('<div/>').append(options.data.ESTADO).appendTo(container);
                    }
                }
            },
            {
                dataField: 'ADJUNTO', width: '12%', caption: 'Adjunto', dataType: 'string',
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
                width: '12%',
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
                                        IdDocumento = options.data.ID_DOCUMENTO;
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
                width: '12%',
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
            },
            {
                width: '12%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.data.EDIT_INDICES) {
                        $('<div/>').dxButton({
                            icon: 'comment',
                            hint: 'Editar indices documento',
                            onClick: function (e) {
                                var _Ruta = $("#SIM").data("url") + 'Utilidades/PuedeEditarIndicesDoc?IdDoc=' + options.data.ID_DOCUMENTO;
                                $.getJSON(_Ruta)
                                    .done(function (data) {
                                        if (data.returnvalue) {
                                            var _Ruta = $('#SIM').data('url') + "api/UtilidadesApi/EditarIndicesDocumento";
                                            $.getJSON(_Ruta, { IdDocumento: options.data.ID_DOCUMENTO })
                                                .done(function (data) {
                                                    if (data != null) {
                                                        if (data.length > 0) {
                                                            popupEditInd = $("#popUpEditIndicesDoc").dxPopup("instance");
                                                            popupEditInd.show();
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
                                    });                            }
                        }).appendTo(container);
                    }
                }
            },
            {
                width: '12%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'rowfield',
                        text: options.data.CODTRAMITE,
                        hint: 'Detalle trámite',
                        onClick: function (e) {
                            if (options.data.CODTRAMITE > 0) {
                                var popupOpciones = {
                                    height: 600,
                                    width: 1100,
                                    title: 'Detalle del trámite',
                                    visible: false,
                                    contentTemplate: function (container) {
                                        $("<iframe>").attr("src", $('#SIM').data('url') + 'Utilidades/DetalleTramite?popup=true&CodTramite=' + options.data.CODTRAMITE).attr("width", "100%").attr("height", "100%").attr("frameborder", "0").attr("scrolling", "0").appendTo(container);
                                    }
                                }
                                var popupTra = $("#popDetalleTramite").dxPopup(popupOpciones).dxPopup("instance");
                                $("#popDetalleTramite").css({ 'visibility': 'visible' });
                                $("#popDetalleTramite").fadeTo("slow", 1);
                                popupTra.show();
                            }
                        }
                    }).appendTo(container);
                }
            },
            {
                width: '12%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (EditarDoc == 'Y') {
                        $('<div/>').dxButton({
                            icon: 'unselectall',
                            hint: 'Reemplazar documento del SIM',
                            onClick: function (e) {
                                var result = DevExpress.ui.dialog.confirm('Realmente desea reemplazar el documento? esta acción es irreversible!! tiene copia del documento anterior?', 'Confirmación');
                                result.done(function (dialogResult) {
                                    if (dialogResult) {
                                        $("#popUpReempDoc").dxPopup("instance").show();
                                    }
                                });
                            }
                        }).appendTo(container);
                    }
                }
            }
        ]
    });

    $("#ufDocumento").dxFileUploader({
        allowedFileExtensions: [".pdf"],
        multiple: false,
        selectButtonText: 'Seleccionar Archivo',
        invalidFileExtensionMessage: "El tipo de archivo no esta permitido",
        labelText: 'o arrastre un archivo aquí',
        uploadMode: "instantly",
        showFileList: false,
        uploadUrl: $('#SIM').data('url') + 'GestionDocumental/Api/DocumentosApi/RecibeArch',
        onUploaded: function (e) {
            $("#loadPanel").dxLoadPanel('instance').hide();
            var obj = JSON.parse(e.request.responseText);
            const resp = obj.split(';');
            if (resp[0] == "Ok") {
                $("#lblArchivo").text(resp[1]);
            } else {
                DevExpress.ui.dialog.alert(resp[1], 'Reemplazar documento');
                $("#lblArchivo").text(resp[1]);
            }
        },
        onUploadStarted: function (e) {
            $("#loadPanel").dxLoadPanel('instance').show();
        },
        onUploadError: function (e) {
            $("#loadPanel").dxLoadPanel('instance').hide();
            DevExpress.ui.dialog.alert('Error Subiendo Archivo: ' + e.request.responseText, 'Reemplazar documento');
        }
    });

    $("#btnGuardaDoc").dxButton({
        stylingMode: "contained",
        text: "Guarda documento temporal",
        icon: 'save',
        onClick: function () {
            var Documento = $("#lblArchivo").text();
            var Sel = $("#grdDocs").dxDataGrid("instance").getSelectedRowsData()[0];
            //var params = { IdDocumento: Sel.ID_DOCUMENTO, Doc: Documento };
            var _Ruta = $('#SIM').data('url') + 'GestionDocumental/Api/DocumentosApi/ReemplazaDoc?IdDocumento=' + Sel.ID_DOCUMENTO + '&Doc=' + Documento;
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                //data: JSON.stringify(params),
                contentType: "application/json",
                beforeSend: function () { },
                success: function (data) {
                    if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Reemplazar documento');
                    else {
                        DevExpress.ui.dialog.alert('Documento reemplazado correctamente', 'Reemplazar documento');
                        $('#grdDocs').dxDataGrid("instance").refresh();
                        $("#popUpReempDoc").dxPopup("instance").hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Reemplazar documento');
                }
            });
        }
    });

    $("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
        shadingColor: "rgba(0,0,0,0.4)",
    });

    $("#popUpReempDoc").dxPopup({
        width: 700,
        height: 500,
        showTitle: true,
        title: "Reemplazar documento del SIM"
    });

    $("#btnGuardaIndicesDoc").dxButton({
        icon: 'save',
        hint: 'Guardar indices del documento',
        onClick: function (e) {
            var Indices = indicesSerieDocumentalStore._array;
            var Sel = $("#grdDocs").dxDataGrid("instance").getSelectedRowsData()[0];
            var params = { IdDocumento: Sel.ID_DOCUMENTO, Indices: Indices };
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
                        popupEditInd = $("#popUpEditIndicesDoc").dxPopup("instance");
                        popupEditInd.hide();
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
            popupEditInd = $("#popUpEditIndicesDoc").dxPopup("instance");
            popupEditInd.hide();
        }
    });

    var popupInd = null;

    var showIndices = function (data) {
        Indices = data;
        if (popupInd) {
            popupInd.option("contentTemplate", popupOptInd.contentTemplate.bind(this));
        } else {
            popupInd = $("#PopupVerIndicesDoc").dxPopup(popupOptInd).dxPopup("instance");
        }
        popupInd.show();
    };

    var popupOptInd = {
        width: 700,
        height: "auto",
        hoverStateEnabled: true,
        title: "Indices del documento",
        visible: false,
        closeOnOutsideClick: true,
        contentTemplate: function () {
            var Content = "";
            $.each(Indices, function (key, value) {
                Content += "<p>" + value.INDICE + " : <span><b>" + value.VALOR + "</b></span></p>";
            });
            return $("<div />").append(
                $("<p><b>Indices del documento " + IdDocumento + "</b></p>"),
                $("<br />"),
                Content
            );
        }
    };

    $("#popUpEditIndicesDoc").dxPopup({
        width: 900,
        height: 800,
        showTitle: true,
        title: "Editar indices del documeno"
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
            CargarGridIndicesTra();
            //$("#GridIndices").dxDataGrid("instance").option("dataSource", indicesSerieDocumentalStore); 
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
                        //$("#GridIndices").dxDataGrid("instance").option("dataSource", indicesSerieDocumentalStore);
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
                pageSize: 0
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
                columnRenderingMode: "standard",
                mode: "standard",
                preloadEnabled: false,
                renderAsync: undefined,
                rowRenderingMode: "standard",
                scrollByContent: true,
                scrollByThumb: false,
                showScrollbar: "always",
                useNative: "auto"
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
});

function SeleccionaDocumento(IdDocumento) {
    var _popup = $("#popupBuscaDoc").dxPopup("instance");
    _popup.hide();
    if (IdDocumento != "") {
        IdDocumento = IdDocumento;
        $('#grdDocs').dxDataGrid({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.CustomStore({
                    key: "ID_DOCUMENTO",
                    loadMode: "raw",
                    load: function () {
                        return $.getJSON($("#SIM").data("url") + "GestionDocumental/api/DocumentosApi/ObtieneDocumento?IdDocumento=" + IdDocumento);
                    }
                })
            })
        });
        var GridDocumento = $("#grdDocs").dxDataGrid("instance");
        GridDocumento.refresh();
    } else alert("No se ha ingresado el codifo del expediente");
}