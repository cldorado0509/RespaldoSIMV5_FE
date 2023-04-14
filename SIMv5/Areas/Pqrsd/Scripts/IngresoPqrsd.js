var CodRecursoAmb = -1;
var TipoPqrsd = -1;
var IdDepto = -1;
var IdPqrsd = "0";
var ArchivosSubidos = new DevExpress.data.ArrayStore({ store: [] });
var TramiteVerificado = false;
var CodTramite = "";
var popupOpciones = "";

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

$(document).ready(function () {
    var TipoTra = $("#sbTipoSolicitud").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Pqrsd/api/PqrsdApi/ObtenerTipoPqrsd");
                }
            })
        }),
        valueExpr: 'id',
        displayExpr: 'valor',
        placeholder: 'Seleccione tipo de solicitud',
        showClearButton: true,
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
                chkUEA.option("disabled", true);
            }
            if (TipoPqrsd == 6) {
                chkUEA.option("disabled", false);
            }
        }
    }).dxSelectBox("instance");

    var Recurso = $("#sbRecurso").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Pqrsd/api/PqrsdApi/ObtenerRecursoAmb");
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
                    return $.getJSON($("#SIM").data("url") + "Pqrsd/api/PqrsdApi/ObtenerAfectacionAmb", { IdRecurso: CodRecursoAmb });
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
                    return $.getJSON($("#SIM").data("url") + "Pqrsd/api/PqrsdApi/ObtenerTipoPersona");
                }
            })
        }),
        valueExpr: 'id',
        displayExpr: 'valor',
        placeholder: 'Seleccione tipo de persona',
        showClearButton: true
    }).dxSelectBox("instance");

    var FromaRecibe = $("#sbFormaRecepcion").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Pqrsd/api/PqrsdApi/ObtenerFormaRecepcion");
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
                    return $.getJSON($("#SIM").data("url") + "Pqrsd/api/PqrsdApi/ObtenerMedioRpta");
                }
            })
        }),
        valueExpr: 'id',
        displayExpr: 'valor',
        placeholder: 'Seleccione medio de respuesta de la PQRSD',
        showClearButton: true
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
                    return $.getJSON($("#SIM").data("url") + "Pqrsd/api/PqrsdApi/ObtenerTiposDoc");
                }
            })
        }),
        valueExpr: 'id',
        displayExpr: 'valor',
        placeholder: 'Seleccione tipo de documento',
        showClearButton: true
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
                    return $.getJSON($("#SIM").data("url") + "Pqrsd/api/PqrsdApi/ObtenerDeptos");
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
                    return $.getJSON($("#SIM").data("url") + "Pqrsd/api/PqrsdApi/ObtenerCiudad", { IdDepto: IdDepto });
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
    }).dxTextBox("instance");

    var txtContenido = $("#txtContenido").dxTextArea({
        placeholder: "Ingrese la descripción de su solicitud",
        value: "",
        height: 90
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
        uploadUrl: $('#SIM').data('url') + 'Pqrsd/api/PqrsdApi/RecibeArch',
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
                e.component.reset();
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
                                    var _Ruta = $('#SIM').data('url') + "Pqrsd/api/PqrsdApi/EliminaAnexo";
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
        hint: 'Unidad de Emergemcias Ambientales'
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
                var _Ruta = $('#SIM').data('url') + "Pqrsd/api/PqrsdApi/ValidaTramite";
                $.getJSON(_Ruta, { Codtramite: CodTramite })
                    .done(function (data) {
                        if (data != null) {
                            TramiteVerificado = data.TramiteVerificado;
                            lnkTramite.option("visible", true);
                            lnkTramite.option("text", data.Mensaje);
                            popupOpciones = {
                                height: 600,
                                width: 1100,
                                title: 'Detalle del trámite',
                                visible: false,
                                contentTemplate: function (container) {
                                    $("<iframe>").attr("src", $('#SIM').data('url') + 'Utilidades/DetalleTramite?popup=true&CodTramite=' + CodTramite + "&Orden=" + Orden).attr("width", "100%").attr("height", "100%").attr("frameborder", "0").attr("scrolling", "0").appendTo(container);
                                }
                            }
                        }
                    }).fail(function (jqxhr, textStatus, error) {
                        DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Validar Trámite');
                    });
            } else {
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

    $("#btnGuardar").dxButton({
        type: "default",
        icon: "save",
        text: 'Registrar la Pqrsd',
        onClick: function () {

        }
    });
});