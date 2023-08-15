var CodRecursoAmb = -1;
var TipoPqrsd = -1;
var IdDepto = -1;
var IdPqrsd = "0";
var ArchivosSubidos = new DevExpress.data.ArrayStore({ store: [] });
var TramiteVerificado = false;
var CodTramite = "";
var popupOpciones = "";
var Orden = 1;
var indicesSerieDocumentalStore = null;
var indicesTramiteStore = null;
var ExpedientesDataSource = new DevExpress.data.ArrayStore({ store: [] });
var CodProceso = -1;
var CodTarea = -1;
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

function SelTramite(CodTramite) {
    var _popup = $("#popupBuscaTra").dxPopup("instance");
    _popup.hide();
    if (CodTramite != "") {
        CodTramite = CodTramite;
        $("#txtTramite").dxTextBox("instance").option("value", CodTramite);
    } else alert("No se ha ingresado el codifo del expediente");
}

$(document).ready(function () {

    $("#acordeon").accordion({
        collapsible: true,
        animationDuration: 500,
        height: 800,
        create: function (event, ui) {
            $.getJSON($("#SIM").data("url") + 'Tramites/api/ProyeccionDocumentoApi/ObtenerIndicesSerieDocumental', {
                codSerie: 10,
            }).done(function (data) {
                AsignarIndices(data);
            });
        },
        activate: function (event, ui) {
            tabsTramite.repaint();
        }
    });

    var TipoSol = $("#sbTipoSolicitud").dxSelectBox(
        {
            items: [{ Id: 0, Nombre: 'PQRSDF' }, { Id: 1, Nombre: 'Gestión Catastral' }, { Id: 2, Nombre: 'Notificación Judicial' }, { Id: 3, Nombre: 'COR' }],
            valueExpr: 'Id',
            displayExpr: 'Nombre',
            placeholder: 'Seleccione tipo de solicitud',
            showClearButton: true,
            onValueChanged: function (data) {
                TipoSolicitud = data.value;
                switch (TipoSolicitud) {
                    case 0:
                        TipoPqrsd.option("visible", true);
                        TipoOtros.option("visible", false);
                        $("#lblTipoSol").attr("for", "sbTipoSolicitud");
                        $("#lblTipoSol").text("(*) Tipo de Solicitud Pqrsd : ");
                        $("#lblTipoSol").css("visibility", "visible");
                        $("#lblchkUEA").css("visibility", "visible");
                        chkUEA.option("visible", true);
                        TipoPersona.option("disabled", false);
                        MedioResp.option("disabled", false);
                        TiposDoc.option("disabled", false);
                        txtDocumento.option("disabled", false);
                        txtContenido.option("disabled", false);
                        break;
                    case 1:
                    case 2:
                        TipoPqrsd.option("visible", false);
                        $("#lblTipoSol").css("visibility", "hidden");
                        TipoOtros.option("visible", false);
                        $("#lblTipoSol").css("visibility", "hidden");
                        $("#lblchkUEA").css("visibility", "hidden");
                        chkUEA.option("visible", false);
                        TipoPersona.option("disabled", true);
                        MedioResp.option("disabled", true);
                        TiposDoc.option("disabled", true);
                        txtDocumento.option("disabled", true);
                        txtContenido.option("disabled", true);
                        break;
                    case 3:
                        $("#lblTipoSol").attr("for", "sbTipoOtros");
                        $("#lblTipoSol").text("(*) Tipo Solicitud COR : ");
                        TipoOtros.option("visible", true);
                        TipoPqrsd.option("visible", false);
                        $("#lblTipoSol").css("visibility", "visible");
                        $("#lblchkUEA").css("visibility", "hidden");
                        chkUEA.option("visible", false);
                        TipoPersona.option("disabled", true);
                        MedioResp.option("disabled", true);
                        TiposDoc.option("disabled", true);
                        txtDocumento.option("disabled", true);
                        txtContenido.option("disabled", true);
                        break;
                }
            }
        }
    ).dxValidator({
        validationGroup: "PqrsdGroup",
        validationRules: [{
            type: "required",
            message: "Aún no ha seleccionado un tipo de solicitud!!"
        }]
    }).dxSelectBox("instance");

    var TipoPqrsd = $("#sbTipoPqrsd").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Solicitudes/api/ListadosApi/ObtenerTipoPqrsd");
                }
            })
        }),
        valueExpr: 'id',
        displayExpr: 'valor',
        placeholder: 'Seleccione tipo de solicitud',
        showClearButton: true,
        visible: false,
        onValueChanged: function (data) {
            TipoPqrsd = data.value;
            if (TipoPqrsd == 8) {
                $("#lblRecurso").css("visibility", "visible");
                Recurso.option("visible", true);
                $("#lblAfectacionAmb").css("visibility", "visible");
                AfectacionAmb.option("visible", true);
                chkUEA.option("disabled", false);
            } else {
                $("#lblRecurso").css("visibility", "hidden");
                $("#lblAfectacionAmb").css("visibility", "hidden");
                Recurso.option("visible", false);
                AfectacionAmb.option("visible", false);
                chkUEA.option("value", false);
                chkUEA.option("disabled", true);
            }
            if (TipoPqrsd == 6) {
                chkUEA.option("disabled", false);
            }
        }
    }).dxSelectBox("instance");

    var TipoOtros = $("#sbTipoOtros").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Solicitudes/api/ListadosApi/ObtenerOtros");
                }
            })
        }),
        valueExpr: 'id',
        displayExpr: 'valor',
        placeholder: 'Seleccione tipo de solicitud',
        showClearButton: true,
        visible: false
    }).dxSelectBox("instance");

    var Recurso = $("#sbRecurso").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Solicitudes/api/ListadosApi/ObtenerRecursoAmb");
                }
            })
        }),
        valueExpr: 'id',
        displayExpr: 'valor',
        visible: false,
        placeholder: 'Seleccione recurso ambiental',
        showClearButton: true,
        onValueChanged: function (data) {
            if (data.value != null) {
                var cboAfectacionAmbDs = AfectacionAmb.getDataSource();
                CodRecursoAmb = data.value;
                cboAfectacionAmbDs.reload();
                AfectacionAmb.option("value", null);
            }
        }
    }).dxSelectBox("instance");

    var AfectacionAmb = $("#sbAfectacionAmb").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Solicitudes/api/ListadosApi/ObtenerAfectacionAmb", { IdRecurso: CodRecursoAmb });
                }
            })
        }),
        valueExpr: 'id',
        displayExpr: 'valor',
        visible: false,
        placeholder: 'Seleccione afectación ambiental',
        showClearButton: true,
        itemTemplate(data) {
            return `<b>${data.valor} :</b><div class='custom-item'>${data.descripcion}</div>`;
        }
    }).dxSelectBox("instance");

    var TipoPersona = $("#sbTipoPersona").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Solicitudes/api/ListadosApi/ObtenerTipoPersona");
                }
            })
        }),
        valueExpr: 'id',
        displayExpr: 'valor',
        placeholder: 'Seleccione tipo de persona',
        showClearButton: true
    }).dxValidator({
        validationGroup: "PqrsdGroup",
        validationRules: [{
            type: "required",
            message: "Aún no ha seleccionado un tipo de solicitante!!"
        }]
    }).dxSelectBox("instance");

    var FormaRecibe = $("#sbFormaRecepcion").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Solicitudes/api/ListadosApi/ObtenerFormaRecepcion");
                }
            })
        }),
        valueExpr: 'id',
        displayExpr: 'valor',
        placeholder: 'Seleccione forma de recepción de la solicitud',
        showClearButton: true
    }).dxSelectBox("instance");

    var MedioResp = $("#sbMedioRespuesta").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Solicitudes/api/ListadosApi/ObtenerMedioRpta");
                }
            })
        }),
        valueExpr: 'id',
        displayExpr: 'valor',
        placeholder: 'Seleccione medio de respuesta de la PQRSD',
        showClearButton: true,
        onValueChanged: function (data) {
            if (data.text != null) {
                if (data.value == 3) {
                    if (txtTelefono.option("value") == "") {
                        DevExpress.ui.dialog.alert("Si el medio de respuesta es telefónicamente debe aportar el número telefónico!", 'Pqrsd');
                    }
                }
                if (data.value == 2) {
                    if (txtDireccion.option("value") == "") {
                        DevExpress.ui.dialog.alert("Si el medio de respuesta es correo certificado debe aportar una dirección para su envío!", 'Pqrsd');
                    }
                }
                var cboAfectacionAmbDs = AfectacionAmb.getDataSource();
                CodRecursoAmb = data.value;
                cboAfectacionAmbDs.reload();
                AfectacionAmb.option("value", null);
            }
        }
    }).dxValidator({
        validationGroup: "PqrsdGroup",
        validationRules: [{
            type: "required",
            message: "Aún no ha seleccionado un medio de respuesta para su solicitud!!"
        }]
    }).dxSelectBox("instance");

    var txtNombre = $("#txtNombre").dxTextBox({
        placeholder: "Ingrese el nombre de la persona o empresa",
        value: ""
    }).dxTextBox("instance");

    var TiposDoc = $("#sbTipoDoc").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Solicitudes/api/ListadosApi/ObtenerTiposDoc");
                }
            })
        }),
        valueExpr: 'id',
        displayExpr: 'valor',
        placeholder: 'Seleccione tipo de documento',
        showClearButton: true,
    }).dxSelectBox("instance");

    var txtDocumento = $("#txtDocumento").dxTextBox({
        placeholder: "Ingrese el numero del documento",
        value: ""
    }).dxTextBox("instance");

    var txtPais = $("#txtPais").dxTextBox({
        placeholder: "Ingrese el pais de origen de la Petición",
        value: "Colombia"
    }).dxTextBox("instance");

    var Departamentos = $("#sbDepto").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Solicitudes/api/ListadosApi/ObtenerDeptos");
                }
            })
        }),
        valueExpr: 'id',
        displayExpr: 'valor',
        placeholder: 'Seleccione el departamento',
        showClearButton: true,
        onValueChanged: function (data) {
            if (data.value != null) {
                var CiudadesDs = Ciudades.getDataSource();
                IdDepto = data.value;
                CiudadesDs.reload();
                Ciudades.option("value", null);
            }
        }
    }).dxSelectBox("instance");

    var Ciudades = $("#sbCiudad").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Solicitudes/api/ListadosApi/ObtenerCiudad", { IdDepto: IdDepto });
                }
            })
        }),
        valueExpr: 'id',
        displayExpr: 'valor',
        placeholder: 'Seleccione la ciudad',
        showClearButton: true
    }).dxSelectBox("instance");

    var txtBarrio = $("#txtBarrio").dxTextBox({
        placeholder: "Ingrese el barrio o vereda",
        value: ""
    }).dxTextBox("instance");

    var txtCorreo = $("#txtCorreo").dxTextBox({
        placeholder: "Ingrese el correo electrónico",
        value: ""
    }).dxValidator({
        validationGroup: "PqrsdGroup",
        validationRules: [{
            type: "required",
            message: "El correo electrónico es obligatorio"
        },
        {
            type: 'email',
            message: 'El formato del correo electrónico no es válido'
        }]
    }).dxTextBox("instance");

    var txtDireccion = $("#txtDireccion").dxTextBox({
        placeholder: "Ingrese la dirección",
        value: ""
    }).dxTextBox("instance");

    var txtTelefono = $("#txtTelefono").dxTextBox({
        placeholder: "Ingrese el número de teléfono fijo o celular",
        value: ""
    }).dxTextBox("instance");

    var txtAsunto = $("#txtAsunto").dxTextBox({
        placeholder: "Ingrese el asunto para la solicitud",
        value: ""
    }).dxValidator({
        validationGroup: "PqrsdGroup",
        validationRules: [{
            type: "required",
            message: "Aún no ha ingresado un asunto descriptivo para su solicitud!!"
        }]
    }).dxTextBox("instance");

    var txtContenido = $("#txtContenido").dxTextArea({
        placeholder: "Ingrese la descripción de su solicitud",
        value: "",
        height: 90
    }).dxValidator({
        validationGroup: "PqrsdGroup",
        validationRules: [{
            type: "required",
            message: "Aún no ha ingresado una descripción para su solicitud!!"
        }]
    }).dxTextArea("instance");

    $("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
        shadingColor: "rgba(0,0,0,0.4)",
    });

    var ufAnexos = $("#ufAnexos").dxFileUploader({
        allowedFileExtensions: [".doc", ".docx", ".pdf", ".jpg", ".jpeg", ".png"],
        multiple: true,
        selectButtonText: 'Seleccionar Archivo',
        invalidFileExtensionMessage: "El tipo de archivo no esta permitido",
        invalidMaxFileSizeMessage: "Tamaño del archivo demasiado grande",
        labelText: 'o arrastre un archivo aquí',
        uploadMode: "instantly",
        showFileList: false,
        uploadUrl: $('#SIM').data('url') + 'Solicitudes/api/SolicitudesdApi/RecibeArch',
        onUploadAborted: (e) => removeFile(e.file.name),
        onUploaded: function (e) {
            $("#loadPanel").dxLoadPanel('instance').hide();
            const obj = JSON.parse(e.request.responseText);
            if (obj.SubidaExitosa) {
                IdPqrsd = obj.IdPQRSD;
                DevExpress.ui.dialog.alert(obj.MensajeExito, 'Anexos Pqrsd');
                ArchivosSubidos.insert({ Documento: e.file.name, IdArchivo: obj.IdArchivo });
                gridAnexos.option("dataSource", ArchivosSubidos);
            } else {
                DevExpress.ui.dialog.alert(obj.MensajeError, 'Anexos Pqrsd');
                e.component.abortUpload();
            }
        },
        onUploadStarted: function (e) {
            $("#loadPanel").dxLoadPanel('instance').show();
        },
        onUploadError: function (e) {
            $("#loadPanel").dxLoadPanel('instance').hide();
            DevExpress.ui.dialog.alert('Error Subiendo Archivo: ' + e.request.responseText, 'Anexos Pqrsd');
        },
        onValueChanged: function (e) {
            var url = e.component.option('uploadUrl');
            url = updateQueryStringParameter(url, 'IdPqrsd', IdPqrsd);
            e.component.option('uploadUrl', url);
        }
    }).dxFileUploader("instance");

    var gridAnexos = $("#TablaAnexos").dxDataGrid({
        dataSource: ArchivosSubidos,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Aún no se han subido anexos al servidor",
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
            { dataField: 'Documento', width: '90%', caption: 'Documento', dataType: 'string' },
            {
                caption: 'Eliminar',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'remove',
                        hint: 'Eliminar el documento anexo',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar el documento ' + options.data.Documento + '?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "Solicitudes/api/SolicitudesApi/EliminaAnexo";
                                    $.getJSON(_Ruta, { IdAnexo: options.data.IdArchivo, IdPqrsd: IdPqrsd })
                                        .done(function (data) {
                                            if (data != null) {
                                                if (data.resp == "Error") {
                                                    DevExpress.ui.dialog.alert("Ocurrió el error " + data.mensaje, "Eliminar Anexo");
                                                } else {
                                                    DevExpress.ui.dialog.alert(data.mensaje, "Eliminar Anexo");
                                                    ArchivosSubidos.remove(options.data);
                                                    gridAnexos.option("dataSource", ArchivosSubidos);
                                                }
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Eliminar Anexo');
                                        });
                                }
                            });
                        }
                    }).appendTo(container);
                }
            }
        ]
    }).dxDataGrid("instance");

    var chkUEA = $("#chkUEA").dxCheckBox({
        value: false,
        hint: 'Unidad de Emergemcias Ambientales',
        visible: false
    }).dxCheckBox("instance");

    var txtTramite = $("#txtTramite").dxTextBox({
        placeholder: "Trámite asociado",
        value: ""
    }).dxTextBox("instance");

    $("#btnVerificaTramite").dxButton({
        icon: "check",
        hint: 'Validar el trámite',
        onClick: function () {
            CodTramite = txtTramite.option("value");
            if (CodTramite != "") {
                var _Ruta = $('#SIM').data('url') + "Solicitudes/api/SolicitudesApi/ValidaTramite";
                $.getJSON(_Ruta, { Codtramite: CodTramite })
                    .done(function (data) {
                        if (data != null) {
                            TramiteVerificado = data.TramiteVerificado;
                            lnkTramite.option("visible", true);
                            lnkTramite.option("text", data.Mensaje);
                            chTramiteNuevo.option("disabled", true);
                            Procesos.option("disabled", true);
                            CodProceso = -1;
                            $("#lblTitPregunta").text("");
                            gridTareasPosibles.refresh();
                            gridTareasPosibles.option("visible", false);
                            popupOpciones = {
                                height: 600,
                                width: 1100,
                                title: 'Detalle del trámite',
                                visible: false,
                                contentTemplate: function (container) {
                                    $("<iframe>").attr("src", $('#SIM').data('url') + 'Utilidades/DetalleTramite?popup=true&CodTramite=' + CodTramite + "&Orden=" + Orden).attr("width", "100%").attr("height", "100%").attr("frameborder", "0").attr("scrolling", "0").appendTo(container);
                                }
                            }
                        } else chTramiteNuevo.option("disabled", false);
                    }).fail(function (jqxhr, textStatus, error) {
                        DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Validar Trámite');
                    });
            } else {
                chTramiteNuevo.option("disabled", false);
                DevExpress.ui.dialog.alert('Aún no ha ingresado un código de trámite para verificar', 'Validar Trámite');
            }
        }

    });

    $("#btnBuscaTramite").dxButton({
        icon: "find",
        hint: 'Buscar el trámite',
        onClick: function () {
            var _popup = $("#popupBuscaTra").dxPopup("instance");
            _popup.show();
            $('#BuscarTra').attr('src', $('#SIM').data('url') + 'Utilidades/BuscarTramite?popup=true');
        }
    });

    $("#popupBuscaTra").dxPopup({
        width: 900,
        height: 800,
        showTitle: true,
        title: "Buscar Trámites del SIM"
    });

    var lnkTramite = $("#lnkVerificaTramite").dxButton({
        stylingMode: 'outlined',
        text: '',
        visible: false,
        onClick: function () {
            if (TramiteVerificado) {
                popup = $("#popDetalleTramite").dxPopup(popupOpciones).dxPopup("instance");
                $('#popDetalleTramite').css({ 'visibility': 'visible' });
                $("#popDetalleTramite").fadeTo("slow", 1);
                popup.show();
            }
        }
    }).dxButton("instance");

    //Indices del Documento

    function CargarGridIndices() {
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
            sorting: {
                mode: 'none',
            },
            wordWrapEnabled: true,
            columns: [
                {
                    dataField: "CODINDICE",
                    dataType: 'number',
                    visible: false,
                }, {
                    dataField: "INDICE",
                    caption: 'INDICE',
                    dataType: 'string',
                    width: '40%',
                    visible: true,
                    allowEditing: false
                }, {
                    dataField: 'VALOR',
                    caption: 'VALOR',
                    dataType: 'string',
                    allowEditing: true,
                    cellTemplate: function (cellElement, cellInfo) {
                        cellElement.css('text-align', 'center');

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
                                    //cellElement.html(cellInfo.data.VALOR.getDate() + '/' + (cellInfo.data.VALOR.getMonth() + 1) + '/' + cellInfo.data.VALOR.getFullYear());
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
                            /*var div = document.createElement("div");
                            cellElement.get(0).appendChild(div);
     
                            $(div).dxTextBox({
                                value: cellInfo.data.VALOR,
                                onValueChanged: function (e) {
                                    cellInfo.setValue(e.value);
                                },
                            });
                            break;*/
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
                                        displayExpr: 'NOMBRE',
                                        valueExpr: 'ID',
                                        placeholder: "[SELECCIONAR OPCION]",
                                        value: (cellInfo.data.VALOR == null ? null : itemsLista[itemsLista.findIndex(ls => ls.NOMBRE == cellInfo.data.VALOR)].ID),
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
                    dataField: "ID_VALOR",
                    caption: 'ID_VALOR',
                    dataType: 'int',
                    visible: false,
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
    function AsignarIndices(indices) {
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
            CargarGridIndices();
        } else {
            opcionesLista.forEach(function (valor, indice, array) {
                $.getJSON($("#SIM").data("url") + 'Tramites/api/ProyeccionDocumentoApi/ObtenerIndiceValoresLista?id=' + valor.idLista).done(function (data) {
                    var index = opcionesLista.findIndex(ol => ol.idLista == valor.idLista);
                    opcionesLista[index].datos = data;
                    opcionesLista[index].cargado = true;

                    var finalizado = true;

                    opcionesLista.forEach(function (v, i, a) {
                        finalizado = finalizado && v.cargado;
                    });

                    if (finalizado) {
                        CargarGridIndices();
                    }
                });
            });
        }

        cargando = false;
    }


    //Fin indice del documento



    $("#btnGuardar").dxButton({
        type: "default",
        icon: "save",
        text: 'Registrar la Solicitud',
        onClick: function () {
            DevExpress.validationEngine.validateGroup("PqrsdGroup");
            $("#loadPanel").dxLoadPanel('instance').show();
            var _Ruta = $('#SIM').data('url') + "Solicitudes/api/SolicitudesApi/IngresaSolicitud";
            var params = {
                idPQRSD: IdPqrsd,

                tipoSolicitante: TipoPersona.option("value"),
                tipoPqrsd: TipoPqrsd.option("value"),
                medioRespuesta: MedioResp.option("value"),
                correoElectronico: txtCorreo.option("value"),
                direccion: txtDireccion.option("value"),
                nombre: txtNombre.option("value"),
                barrioComuna: txtBarrio.option("value"),
                telefono: txtTelefono.option("value"),
                pais: txtPais.option("value"),
                departamento: Departamentos.option("text"),
                ciudad: Ciudades.option("text"),
                tipoDocumento: TiposDoc.option("text"),
                documento: txtDocumento.option("value"),
                asunto: txtAsunto.option("value"),
                textoContenido: txtContenido.option("value"),
                recurso: Recurso.option("text"),
                afectacion: AfectacionAmb.option("text"),
                proyecto: 'AMVA',
                codTramite: txtTramite.option("value"),
                emergenciaAmbiental: chkUEA.option("value")
            };
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                beforeSend: function () { },
                success: function (data) {
                    if (data.resp == "Error") {
                        $("#loadPanel").dxLoadPanel('instance').hide();
                        DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Guardar Datos');
                    }
                    else {
                        DevExpress.ui.dialog.alert('Datos Guardados correctamente', 'Guardar Datos');
                        $('#grdListaExp').dxDataGrid({ dataSource: ExpedientesDataSource });
                        $("#PopupNuevoExp").dxPopup("instance").hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });


        }
    });

    $("#popupBuscaExp").dxPopup({
        width: 900,
        height: 800,
        showTitle: true,
        title: "Buscar Expediente"
    });

    $("#btnBuscarExp").dxButton({
        text: "Buscar Expediente",
        icon: "search",
        type: "default",
        width: "190",
        onClick: function () {
            var _popup = $("#popupBuscaExp").dxPopup("instance");
            _popup.show();
            $('#BuscarExp').attr('src', $('#SIM').data('url') + 'Utilidades/BuscarExpediente?popup=true');
        }
    });

    $("#gridExpedientes").dxDataGrid({
        dataSource: ExpedientesDataSource,
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
        remoteOperations: false,
        columns: [
            { dataField: 'ID_EXPEDIENTE', width: '5%', caption: 'Identificador', alignment: 'center' },
            { dataField: 'TIPO', width: '20%', caption: 'Tipo de Expediente', dataType: 'string' },
            { dataField: 'NOMBRE', width: '25%', caption: 'Nombre del Expediente', dataType: 'string' },
            { dataField: 'CODIGO', width: '10%', caption: 'Código Asignado', dataType: 'string' },
            { dataField: 'FECHACREA', width: '10%', caption: 'Fecha', dataType: 'date', format: 'MMM dd yyyy HH:mm' },
            { dataField: 'ANULADO', width: '5%', caption: 'Anulado', dataType: 'string' },
            { dataField: 'ESTADO', width: '5%', caption: 'Último Estado', dataType: 'string' },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'fields',
                        hint: 'Ver detalles del expediente',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/DetalleExpediente";
                            $.getJSON(_Ruta, { IdExpediente: options.data.ID_EXPEDIENTE })
                                .done(function (data) {
                                    if (data != null) {
                                        showExpediente(data);
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Eliminar proceso');
                                });
                        }
                    }).appendTo(container);
                }
            }
        ]
    });

    var popupDet = null;
    var showExpediente = function (data) {
        Expediente = data;
        if (popupDet) {
            popupDet.option("contentTemplate", popupOptions.contentTemplate.bind(this));
        } else {
            popupDet = $("#PopupDetalleExp").dxPopup(popupOptions).dxPopup("instance");
        }
        popupDet.show();
    };

    var popupOptions = {
        width: 700,
        height: "auto",
        hoverStateEnabled: true,
        title: "Detalle del Expediente",
        visible: false,
        closeOnOutsideClick: true,
        contentTemplate: function () {
            var TablaIndi = "<table class='table table-sm' style='font-size: 11px;'><thead><tr><th scope='col'>NOMBRE DEL ÍNDICE</th><th scope='col'>VALOR</th></tr></thead><tbody>";
            Expediente.Indices.forEach(function (indice, index) {
                TablaIndi += "<tr><th scope='row'>" + indice.INDICE + "</th><td>" + indice.VALOR + "</td></tr>";
            });
            return $("<div />").append(
                $("<p>Unidad Documental : <span><b>" + Expediente.UnidadDoc + "</b></span></p>"),
                $("<p>Nombre del expediente : <span><b>" + Expediente.Nombre + "</b></span></p>"),
                $("<p>Código asignado : <span><b>" + Expediente.Codigo + "</b></span></p>"),
                $("<p>Descripción : <span><b>" + Expediente.Descripcion + "</b></span></p>"),
                $("<p>Funcionario que lo creó : <span><b>" + Expediente.Responsable + "</b></span></p>"),
                $("<p>Fecha de creación : <span><b>" + Expediente.FechaCrea + "</b></span></p>"),
                $("<p>Expediente anulado? : <span><b>" + Expediente.Anulado + "</b></span></p>"),
                $("<p>Último estado expediente : <span><b>" + Expediente.UltEstado + "</b></span></p>"),
                $("<br />"),
                $("<p>Cantidad de carpetas : <span><b>" + Expediente.Tomos + "</b></span></p>"),
                $("<p>Cantidad documentos : <span><b>" + Expediente.Documentos + "</b></span></p><br />"),
                $("<div id='tbalaInd'>" + TablaIndi + "</div>")
            );
        }
    };

    var GridFuncTarea = $("#grdFunciTarea").dxDataGrid({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: 'CodFuncionario',
                loadMode: 'raw',
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Solicitudes/api/SolicitudesApi/ObtenerRespTarea", { CodTarea: CodTarea });
                }                                            
            })
        }),
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        visible: false,
        paging: {
            pageSize: 10
        },
        selection: {
            mode: 'single'
        },
        hoverStateEnabled: true,
        remoteOperations: false,
        columns: [
            { dataField: 'CodFuncionario', width: '20%', caption: 'Código', alignment: 'center' },
            { dataField: 'Nombre', width: '80%', caption: 'Proceso', dataType: 'string' }
        ]
    }).dxDataGrid("instance");

    var gridTareasPosibles = $("#gridPosiblesTareas").dxDataGrid({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "CODTAREA",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Solicitudes/api/SolicitudesApi/ObtenerTareasProceso", { IdProceso: CodProceso });
                }
            })
        }),
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        visible: false,
        paging: {
            pageSize: 10
        },
        selection: {
            mode: 'single'
        },
        hoverStateEnabled: true,
        remoteOperations: false,
        columns: [
            { dataField: 'CODTAREA', width: '5%', caption: 'Código', alignment: 'center' },
            { dataField: 'PROCESO', width: '20%', caption: 'Proceso', dataType: 'string' },
            { dataField: 'TAREA', width: '20%', caption: 'Tarea (Actividad)', dataType: 'string' },
            { dataField: 'RESPUESTA', width: '25%', caption: 'Respuesta a la pregunta', dataType: 'string' }
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                CodTarea = data.CODTAREA;
                GridFuncTarea.option("visible", true);
                GridFuncTarea.refresh();
            }
        }
    }).dxDataGrid("instance");


    var Procesos = $("#cmbProcesos").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Solicitudes/api/ListadosApi/ObtenerProcesos");
                }
            })
        }),
        valueExpr: 'id',
        displayExpr: 'valor',
        placeholder: 'Seleccione el tipo de proceso',
        showClearButton: true,
        disabled: true,
        onValueChanged: function (data) {
            if (data.value > 0) {
                CodProceso = data.value;
                var _Ruta = $('#SIM').data('url') + "Solicitudes/api/SolicitudesApi/ObtenerPreguntaTareaInicial";
                $.getJSON(_Ruta, { IdProceso: CodProceso })
                    .done(function (data) {
                        $("#lblTitPregunta").text(data);
                        $.getJSON($("#SIM").data("url") + 'Solicitudes/api/ListadosApi/ObtenerIndicesProceso', {
                            CodProceso: CodProceso,
                        }).done(function (data) {
                            $("#carTramite").show();
                            AsignarIndicesTra(data);
                        });
                    })
                    .fail(function (jqxhr, textStatus, error) {
                        DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Nuevo Trámite');
                    });
                $("#cardTareas").show();
                gridTareasPosibles.option("visible", true);
                gridTareasPosibles.refresh();
            }
        }
    }).dxSelectBox("instance");

    var chTramiteNuevo = $("#chkTramiteNuevo").dxCheckBox({
        value: false,
        hint: 'Se debe crear un nuevo trámite',
        onValueChanged: function (data) {
            if (data.value) {
                Procesos.option("disabled", false);
                CodProceso = -1;
                $("#lblTitPregunta").text("");
                gridTareasPosibles.refresh();
            } else {
                Procesos.option("disabled", true);
                CodProceso = -1;
                $("#lblTitPregunta").text("");
                gridTareasPosibles.refresh();
                gridTareasPosibles.option("visible", false);
            }
        }
    }).dxCheckBox("instance");

    var tabsTramite = $("#TabPanelTramites").dxTabs({
        dataSource: [{ id: 0, text: 'Nuevo trámite' }, { id: 1, text: 'Funcionarios con copia' }],
        selectedIndex: 0,
        showNavButtons: false,
        width: '100%',
        onContentReady: function (e) {
            $("#PanelTramite").show();
            $("#PanelCopias").hide();
            SelTipo = 0;
        },
        onItemClick(e) {
            switch (e.itemData.id) {
                case 0:
                    $("#PanelTramite").show();
                    $("#PanelCopias").hide();
                    break;
                case 1:
                    $("#PanelTramite").hide();
                    $("#PanelCopias").show();
                    break;
            }
        }
    }).dxTabs('instance');

    var GridFuncinarios = $("#grdFunciCopiaTarea").dxDataGrid({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: 'CodFuncionario',
                loadMode: 'raw',
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Solicitudes/api/SolicitudesApi/ObtenerFuncionariosCopia");
                }
            })
        }),
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        filterRow: {
            visible: true,
            emptyPanelText: 'Arrastre una columna para agrupar'
        },
        paging: {
            pageSize: 10
        },
        selection: {
            mode: 'multiple',
            allowSelectAll: false,
            showCheckBoxesMode: 'always'
        },
        hoverStateEnabled: true,
        remoteOperations: false,
        columns: [
            { dataField: 'CodFuncionario', width: '20%', caption: 'Código', alignment: 'center' },
            { dataField: 'Nombre', width: '80%', caption: 'Proceso', dataType: 'string' }
        ]
    }).dxDataGrid("instance");


    //Editar indices Tramite

    function AsignarIndicesTra(indices) {
        opcionesLista = [];

        indicesTramiteStore = new DevExpress.data.LocalStore({
            key: 'CODINDICE',
            data: indices,
            name: 'indicesTramiteStore'
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
                $.getJSON($("#SIM").data("url") + 'Tramites/api/ProyeccionDocumentoApi/ObtenerIndiceValoresLista?id=' + valor.idLista).done(function (data) {
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

        cargando = false;
    }
    function CargarGridIndicesTra() {
        $("#grdIndicesTramite").dxDataGrid({
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
                                        $("#grdEditIndicesTra").dxDataGrid("saveEditData");
                                    },
                                });
                                break;
                            case 5: // Lista
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                let itemsLista = opcionesListaTra[opcionesListaTra.findIndex(ol => ol.idLista == cellInfo.data.ID_LISTA)].datos;

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
                                            $("#grdEditIndicesTra").dxDataGrid("saveEditData");
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

function SeleccionaExp(IdExpediente)
{
    var _popup = $("#popupBuscaExp").dxPopup("instance");
    _popup.hide();
    var _Ruta = $("#SIM").data("url") + "Solicitudes/api/SolicitudesApi/ObtenerExpediente";
    $.getJSON(_Ruta, { IdExp: IdExpediente })
        .done(function (data) {
            ExpedientesDataSource.insert({
                ID_EXPEDIENTE: data.ID_EXPEDIENTE,
                TIPO: data.TIPO,
                NOMBRE: data.NOMBRE,
                CODIGO: data.CODIGO,
                FECHACREA: data.FECHACREA,
                ANULADO: data.ANULADO,
                ESTADO: data.ESTADO
            });
            $('#gridExpedientes').dxDataGrid({ dataSource: ExpedientesDataSource });
        }).fail(function (jqxhr, textStatus, error) {
            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Buscar Expediente');
    });
}