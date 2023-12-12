var IdDocumento = -1;
var txtTramite = "";
var txtFiltro = "";
var txtFulltext = "";
var txtFechas = "";
var UnidadDoc = -1;

const fechas = { weekday: 'long', year: 'numeric', month: 'short', day: 'numeric', hour: 'numeric', minute: 'numeric', second: 'numeric' };
DevExpress.localization.locale(navigator.language);
$(document).ready(function () {
    $("#rdbTipoBusca").dxRadioGroup({
        dataSource: [{ value: 0, text: "Por trámite" }, { value: 1, text: "Por Unidad Documental" }, { value: 2, text: "Full Text" }, { value: 3, text: "Por Fecha Digitalización" }],
        displayExpr: "text",
        valueExpr: "value",
        value: 0,
        layout: "horizontal",
        onContentReady: function (e) {
            $("#PanelTramite").show();
            $("#PanelIndices").hide();
            $("#PanelFulltext").hide();
            $("#PanelFechas").hide();
            SelTipo = 0;
        },
        onValueChanged: function (e) {
            switch (e.value) {
                case 0:
                    $("#PanelTramite").show();
                    $("#PanelIndices").hide();
                    $("#PanelFulltext").hide();
                    $("#PanelFechas").hide();
                    break;
                case 1:
                    $("#PanelTramite").hide();
                    $("#PanelIndices").show();
                    $("#PanelFulltext").hide();
                    $("#PanelFechas").hide();
                    break;
                case 2:
                    $("#PanelTramite").hide();
                    $("#PanelIndices").hide();
                    $("#PanelFulltext").show();
                    $("#PanelFechas").hide();
                    break;
                case 3:
                    $("#PanelTramite").hide();
                    $("#PanelIndices").hide();
                    $("#PanelFulltext").hide();
                    $("#PanelFechas").show();
                    break;
            }
            SelTipo = e.value;
            txtTramite = "";
            txtFiltro = "";
            txtFulltext = "";
            UnidadDoc = -1;
            $("#grdDocs").dxDataGrid("instance").refresh();
            $("#IndicesDoc").html("");
            $("#grdDocs").dxDataGrid("instance").option("visible", false);
        }
    });

    $("#txtTramite").dxTextBox({
        placeholder: "Ingrese el código del trámite",
        value: ""
    });

    $("#txtDato").dxTextBox({
        placeholder: "Ingrese el dato a buscar",
        value: ""
    });

    $('#scrollView').dxScrollView({
        scrollByContent: true,
        scrollByThumb: true,
        showScrollbar: 'always'
    });

    //$("#btnBuscarDoc").dxButton({
    //    text: "Documentos",
    //    icon: "search",
    //    type: "default",
    //    width: "190",
    //    onClick: function () {
    //        var _popup = $("#popupBuscaDoc").dxPopup("instance");
    //        _popup.show();
    //        $('#BuscarDoc').attr('src', $('#SIM').data('url') + 'Utilidades/BuscarDocumento?popup=true');
    //    }
    //});

    $("#cmbUnidadDoc").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "CODSERIE",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Utilidades/GetListaUnidades");
                }
            })
        }),
        displayExpr: "NOMBRE",
        valueExpr: "CODSERIE",
        searchEnabled: true,
        noDataText: "No hay datos para mostrar",
        placeholder: "Seleccione",
        onValueChanged: function (data) {
            UnidadDoc = data.value;
            $("#grdDocs").dxDataGrid("instance").refresh();
            $("#IndicesDoc").html("");
            $("#grdDocs").dxDataGrid("instance").option("visible", false);
            $.getJSON($('#SIM').data('url') + 'Utilidades/GetFields?UniDoc=' + UnidadDoc
            ).done(function (data) {
                var Filtro = $("#FilterBuscar").dxFilterBuilder(OpcionesFiltro).dxFilterBuilder("instance");
                Filtro.option("fields", data);
            }).fail(function (jqxhr, textStatus, error) {
                alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
            });
        }
    });

    var OpcionesFiltro = {
        fields: [],
        value: [],
        filterOperationDescriptions: {
            between: "Entre",
            contains: "Contiene",
            endsWith: "Finaliza en",
            equal: "Igual",
            greaterThan: "Mayor que",
            greaterThanOrEqual: "Mayor o igual a",
            isBlank: "Es blanco",
            isNotBlank: "No es blanco",
            lessThan: "Menor que",
            lessThanOrEqual: "Menor o igual a",
            notContains: "No contiene",
            notEqual: "Diferente",
            startsWith: "Inicia con"
        },
        groupOperationDescriptions: {
            and: "Y",
            or: "O",
            notOr: "",
            notAnd: ""
        }
    };

    $("#cmbUnidadDocFec").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "CODSERIE",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Utilidades/GetListaUnidades");
                }
            })
        }),
        displayExpr: "NOMBRE",
        valueExpr: "CODSERIE",
        searchEnabled: true,
        noDataText: "No hay datos para mostrar",
        placeholder: "Seleccione",
        onValueChanged: function (data) {
            UnidadDoc = data.value;
            $("#grdDocs").dxDataGrid("instance").refresh();
            $("#IndicesDoc").html("");
            $("#grdDocs").dxDataGrid("instance").option("visible", false);

        }
    });

    $("#dpFechaDesde").dxDateBox({
        type: 'date',
        value: '',
        displayFormat: 'dd/MM/yyyy',
        dateSerializationFormat: 'yyyy-MM-dd'
    });

    $("#dpFechaHasta").dxDateBox({
        type: 'date',
        value: '',
        displayFormat: 'dd/MM/yyyy',
        dateSerializationFormat: 'yyyy-MM-dd'
    });

    $("#btnBuscar").dxButton({
        text: "Buscar",
        type: "default",
        onClick: function () {
            switch (SelTipo) {
                case 0 :
                    var _tramite = $("#txtTramite").dxTextBox("instance").option("value");
                    if (_tramite != "") {
                        txtFiltro = "";
                        txtFulltext = "";
                        txtFechas = "";
                        txtTramite = _tramite;
                    } else {
                        txtTramite = "";
                        DevExpress.ui.dialog.alert('No se ha ingresado un código de trámite para buscar', 'Buscar documentos');
                    }
                    break;
                case 1:
                    var _Filto = formatValue($("#FilterBuscar").dxFilterBuilder("instance").option("value"));
                    if (_Filto != "") {
                        txtTramite = "";
                        txtFulltext = "";
                        txtFechas = "";
                        txtFiltro = _Filto;
                    } else {
                        txtFiltro = "";
                        DevExpress.ui.dialog.alert('El filtro no se ha establecido o esta mal', 'Buscar documentos');
                    }
                    break;
                case 2:
                    var _Dato = $("#txtDato").dxTextBox("instance").option("value");
                    if (_Dato != "") {
                        txtTramite = "";
                        txtFiltro = "";
                        txtFechas = "";
                        txtFulltext = _Dato;
                    } else {
                        txtFulltext = "";
                        DevExpress.ui.dialog.alert('No se ha ingresado un dato para buscar', 'Buscar documentos');
                    }
                    break;
                case 3:
                    var _Desde = $("#dpFechaDesde").dxDateBox("instance").option("value");
                    var _Hasta = $("#dpFechaHasta").dxDateBox("instance").option("value");
                    if (_Desde != "" && _Hasta != "") {
                        if (_Hasta >= _Desde) {
                            txtTramite = "";
                            txtFiltro = "";
                            txtFulltext = "";
                            txtFechas = _Desde + "," + _Hasta;
                        } else {
                            txtFulltext = "";
                            DevExpress.ui.dialog.alert('El rango de fechas esta mal establecido', 'Buscar documentos');
                        }
                    } else {
                        txtFulltext = "";
                        DevExpress.ui.dialog.alert('No se ha ingresado un dato para buscar', 'Buscar documentos');
                    }
                    break;
            }
            $("#grdDocs").dxDataGrid("instance").option("visible", true);
            $("#grdDocs").dxDataGrid("instance").refresh();
            $("#IndicesDoc").html("");
        }
    });

    function formatValue(value, spaces) {
        if (value && Array.isArray(value[0])) {
            var TAB_SIZE = 4;
            spaces = spaces || TAB_SIZE;
            return "[" + getLineBreak(spaces) + value.map(function (item) {
                return Array.isArray(item[0]) ? formatValue(item, spaces + TAB_SIZE) : JSON.stringify(item);
            }).join("," + getLineBreak(spaces)) + getLineBreak(spaces - TAB_SIZE) + "]";
        }
        return JSON.stringify(value);
    }

    function getLineBreak(spaces) {
        return "\r\n" + new Array(spaces + 1).join(" ");
    }

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
                    var Buscar = txtTramite.length > 0 ? 'T;' + txtTramite : txtFiltro.length > 0 ? 'F;' + txtFiltro : txtFulltext.length > 0 ? 'B;' + txtFulltext : txtFechas.length > 0 ? 'D;' + txtFechas : '';
                    return $.getJSON($("#SIM").data("url") + 'GestionDocumental/api/DocumentosApi/BuscarDoc?IdUnidadDoc=' + UnidadDoc + '&Buscar=' + Buscar);
                }
            })
        }),
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        paging: {
            pageSize: 10,
            enabled: true
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 20, 50]
        },
        selection: {
            mode: 'single'
        },
        visible: false,
        remoteOperations: false,
        hoverStateEnabled: true,
        columns: [
            { dataField: "ID_DOCUMENTO", visible: false },
            { dataField: "CODTRAMITE", width: '10%', caption: 'Trámite', dataType: 'number' },
            { dataField: "CODDOCUMENTO", width: '7%', caption: 'Documento', dataType: 'number', visible: false  },
            { dataField: 'FECHACREACION', width: '15%', caption: 'Fecha Digitaliza', dataType: 'date', format: 'MMM dd yyyy HH: mm' },
            { dataField: "NOMBRE", width: '30%', caption: 'Unidad Documental', dataType: 'string' },
           /* { dataField: 'INDICES', width: '25%', caption: 'Indices', dataType: 'string' },*/
            {
                width: '10%',
                caption: 'Anulado',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    var _RutaDoc = $("#SIM").data("url") + "GestionDocumental/api/DocumentosApi/ObtieneDocumento?IdDocumento=" + options.data.ID_DOCUMENTO;
                    $.getJSON(_RutaDoc, function (data, status) {
                        if (status === "success") {                           
                            if (data.ESTADO == "Anulado") {
                                $('<div/>').dxButton({
                                    text: 'Si',
                                    hint: 'Documento Anulado',
                                    onClick: function (e) {
                                        var _Ruta = $('#SIM').data('url') + "Utilidades/MotivoDevolucion?IdDocumento=" + data.ID_DOCUMENTO;
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
                            } else if (data.ESTADO == "En proceso") {
                                $('<div/>').dxButton({
                                    text: 'Proceso',
                                    hint: 'En proceso de Anulación',
                                    onClick: function (e) {
                                        var _Ruta = $('#SIM').data('url') + "Utilidades/MotivoDevolucion?IdDocumento=" + data.ID_DOCUMENTO;
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
                                $('<div/>').append(data.ESTADO).appendTo(container);
                            }
                        }
                    });
                }
            },
            {
                width: '10%',
                caption: 'Adjunto',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    var _RutaDoc = $("#SIM").data("url") + "GestionDocumental/api/DocumentosApi/ObtieneDocumento?IdDocumento=" + options.data.ID_DOCUMENTO;
                    $.getJSON(_RutaDoc, function (data, status) {
                        if (status === "success") {
                            if (data.ADJUNTO == "Si") {
                                $('<div/>').dxButton({
                                    text: 'Si',
                                    hint: 'Ver Adjuntos',
                                    onClick: function (e) {
                                        var Documento = data.ID_DOCUMENTO;
                                        var _Ruta = $('#SIM').data('url') + "Utilidades/FuncionarioPoseePermiso?IdDocumento=" + Documento;
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
                    });
                }
            },
            //{
            //    width: '12%',
            //    alignment: 'center',
            //    caption: 'Indices Doc',
            //    cellTemplate: function (container, options) {
            //        $('<div/>').dxButton({
            //            icon: 'fields',
            //            hint: 'Indices del documento',
            //            onClick: function (e) {
            //                var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/IndicesDocumento";
            //                $.getJSON(_Ruta, { IdDocumento: options.data.ID_DOCUMENTO })
            //                    .done(function (data) {
            //                        if (data != null) {
            //                            IdDocumento = options.data.ID_DOCUMENTO;
            //                            showIndices(data);
            //                        }
            //                    }).fail(function (jqxhr, textStatus, error) {
            //                        DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Indices del documento');
            //                    });
            //            }
            //        }).appendTo(container);
            //    }
            //},
            {
                width: '10%',
                alignment: 'center',
                caption: 'Ver Doc',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'doc',
                        hint: 'Ver el documento',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "Utilidades/FuncionarioPoseePermiso?IdDocumento=" + options.data.ID_DOCUMENTO;
                            $.getJSON(_Ruta, function (result, status) {
                                if (status === "success") {
                                    if (result.returnvalue) {
                                        var pdfWindow = window.open("");
                                        pdfWindow.document.write("'<html><head><title>Tramite: " + options.data.CODTRAMITE + " Documento: " + options.data.CODDOCUMENTO + "</title></head><body height='100%' width='100%'><iframe width='100%' height='100%' src='" + $('#SIM').data('url') + "Utilidades/LeeDoc?IdDocumento=" + options.data.ID_DOCUMENTO + "'></iframe></body></html>")

                                       // window.open($('#SIM').data('url') + "Utilidades/LeeDoc?IdDocumento=" + options.data.ID_DOCUMENTO, "Documento " + options.data.ID_DOCUMENTO, "width= 100%,height=100%,scrollbars = yes, location = no, toolbar = no, menubar = no, status = no");
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
                width: '10%',
                alignment: 'center',
                caption: 'Editar Indices',
                cellTemplate: function (container, options) {
                    var _Ruta = $("#SIM").data("url") + 'Utilidades/PuedeEditarIndicesDoc?IdDoc=' + options.data.ID_DOCUMENTO;
                    $.getJSON(_Ruta)
                        .done(function (data) {
                            if (data.returnvalue) {
                                $('<div/>').dxButton({
                                    icon: 'comment',
                                    hint: 'Editar indices documento',
                                    onClick: function (e) {
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
                                    }
                                }).appendTo(container);
                            }
                        });

                    //$('<div/>').dxButton({
                    //    icon: 'comment',
                    //    hint: 'Editar indices documento',
                    //    onClick: function (e) {
                            //var _Ruta = $("#SIM").data("url") + 'Utilidades/PuedeEditarIndicesDoc?IdDoc=' + options.data.ID_DOCUMENTO;
                            //$.getJSON(_Ruta)
                            //    .done(function (data) {
                            //        if (data.returnvalue) {

                                //    } else {
                                //        DevExpress.ui.dialog.alert('Usted no posee permisos para modificar índices del documento!', 'Detalle del trámite');
                                //    }
                                //}).fail(function (jqxhr, textStatus, error) {
                                //    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Detalle del trámite');
                                //});
                    //    }
                    //}).appendTo(container);

                }
            },
            {
                width: '15%',
                alignment: 'center',
                caption: 'Detalle Trámite',
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
                caption: 'Reemplazar Doc',
                visible: EditarDoc == "Y" ? true : false,
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
        ],
        onSelectionChanged(selectedItems) {
            const data = selectedItems.selectedRowsData[0];
            if (data) {
                var strDocumento = data.ID_DOCUMENTO + " (Trámite: " + data.CODTRAMITE + " Documento: " + data.CODDOCUMENTO + ")";
                var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/IndicesDocumento";
                $.getJSON(_Ruta, { IdDocumento: data.ID_DOCUMENTO })
                    .done(function (data) {
                        if (data != null) {                        
                            var Content = "<table class='table table-sm' style='font-size: 10px;'><thead><tr><th scope='col'>NOMBRE DEL ÍNDICE</th><th scope='col'>VALOR</th></tr></thead><tbody>";
                            data.forEach(function (indice, index) {
                                Content += "<tr><th scope='row'>" + indice.INDICE + "</th><td>" + indice.VALOR + "</td></tr>";
                            });
                            $("#IndicesDoc").html("<p><b>Índices del documento " + strDocumento + "</b></p><br />" + Content);
                        }

                    }).fail(function (jqxhr, textStatus, error) {
                        DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Indices del documento');
                    });
            }
        }
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
                        var strDocumento = Sel.ID_DOCUMENTO + " (Trámite: " + Sel.CODTRAMITE + " Documento: " + Sel.CODDOCUMENTO + ")";
                        var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/IndicesDocumento";
                        $.getJSON(_Ruta, { IdDocumento: Sel.ID_DOCUMENTO })
                            .done(function (data) {
                                if (data != null) {
                                    var Content = "<table class='table table-sm' style='font-size: 10px;'><thead><tr><th scope='col'>NOMBRE DEL ÍNDICE</th><th scope='col'>VALOR</th></tr></thead><tbody>";
                                    data.forEach(function (indice, index) {
                                        Content += "<tr><th scope='row'>" + indice.INDICE + "</th><td>" + indice.VALOR + "</td></tr>";
                                    });
                                    $("#IndicesDoc").html("<p><b>Índices del documento " + strDocumento + "</b></p><br />" + Content);
                                }

                            }).fail(function (jqxhr, textStatus, error) {
                                DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Indices del documento');
                            });
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
            var Content = "<table class='table table-striped'><thead><tr><th scope='col'>NOMBRE DEL ÍNDICE</th><th scope='col'>VALOR</th></tr></thead><tbody>";
            $.each(Indices, function (key, value) {
                Content += "<tr><th scope='row'>" + value.INDICE + "</th><td>" + value.VALOR + "</td></tr>";
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

//function SeleccionaDocumento(IdDocumento) {
//    var _popup = $("#popupBuscaDoc").dxPopup("instance");
//    _popup.hide();
//    if (IdDocumento != "") {
//        IdDocumento = IdDocumento;
//        $('#grdDocs').dxDataGrid({
//            dataSource: new DevExpress.data.DataSource({
//                store: new DevExpress.data.CustomStore({
//                    key: "ID_DOCUMENTO",
//                    loadMode: "raw",
//                    load: function () {
//                        return $.getJSON($("#SIM").data("url") + "GestionDocumental/api/DocumentosApi/ObtieneDocumento?IdDocumento=" + IdDocumento);
//                    }
//                })
//            })
//        });
//        var GridDocumento = $("#grdDocs").dxDataGrid("instance");
//        GridDocumento.refresh();
//    } else alert("No se ha ingresado el codifo del expediente");
//}