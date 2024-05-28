var IdSolicitud = "-1";
var indicesSerieDocumentalStore = null;
var columnasExcel;
var opcionesLista = [];
var ArrIndices = [];
var ArrFirmas = [];
var TramiteValido = false;
var CantFirmas = 0;
var firmasDocumento = [];
var Tema = "";
var EnEdicion = false;
var FuncElabora = false;
$(document).ready(function () {

    $("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
        shadingColor: "rgba(0,0,0,0.4)",
    });

    var txtRechazo = $("#txtComentario").dxTextArea({
        placeholder: "Ingrese el comentario de rechazo de la firma",
        value: "",
        height: 90,
    }).dxTextArea("instance");

    $("#btnAceptaFirma").dxButton({
        icon: 'key',
        type: "default",
        text: "Firmar Plantilla COD",
        onClick: function () {
            var Propia = FirmaPropia.option("value");
            var Enc = Encargo.option("value");
            var Ado = Adhoc.option("value");
            var TipoFirma = Propia ? "N" : Enc ? "E" : Ado ? "A" : "";
            var Cargo = -1;
            if (TipoFirma != "" && TipoFirma != "N") Cargo = cargos.option("value");
            var params = { CodFuncionario: CodFunc, IdSolicitud: IdSolicitud, Comentario: "", Firmado: true, TipoFirma: TipoFirma, Cargo : Cargo };
            var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/MasivosApi/GuardaFirma";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                beforeSend: function () { },
                success: function (data) {
                    if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Firmar documento');
                    else {
                        DevExpress.ui.dialog.alert(data.mensaje, 'Firmar documento');
                        $('#grdListaMasivos').dxDataGrid("instance").refresh();
                        popFirmar.hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Firmar documento');
                }
            });
        }
    });

    $("#btnRechazaFirma").dxButton({
        icon: 'clear',
        type: "default",
        text: "No Firmar COD",
        onClick: function () {
            var txtRechaza = txtRechazo.option("value");
            if (txtRechaza.length > 0) {
                var params = { CodFuncionario: CodFunc, IdSolicitud: IdSolicitud, Comentario: txtRechaza, Firmado: false };
                var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/MasivosApi/RechazaFirma";
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    url: _Ruta,
                    data: JSON.stringify(params),
                    contentType: "application/json",
                    beforeSend: function () { },
                    success: function (data) {
                        if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Firmar documento');
                        else {
                            DevExpress.ui.dialog.alert(data.mensaje, 'Firmar documento');
                            $('#grdListaMasivos').dxDataGrid("instance").refresh();
                            popFirmar.hide();
                        }
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Firmar documento');
                    }
                });
            } else {
                DevExpress.ui.dialog.alert('Si rechaza la firma debe indicar el motivo', 'Firmar documento');
            }
        }
    });

    var gridExcel = $("#grdExcel").dxDataGrid({
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            pageSize: 15,
            enabled: true
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [15, 20, 50]
        },
        columns: []
    }).dxDataGrid("instance");

    $("#grdAsociaIndices").dxDataGrid({
        dataSource: indicesSerieDocumentalStore,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        editing: {
            mode: "cell",
            allowUpdating: true,
            allowAdding: false,
            allowDeleting: false
        },
        columns: [
            { dataField: "CODINDICE", dataType: 'number', visible: false },
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
                                    $("#grdAsociaIndices").dxDataGrid("saveEditData");
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
                                        $("#grdAsociaIndices").dxDataGrid("saveEditData");
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

            },
            {
                dataField: 'VALORDEFECTO', caption: 'COLUMNA EXCEL', dataType: 'string', allowEditing: true,
                cellTemplate: function (cellElement, cellInfo) {
                    cellElement.css('text-align', 'center');
                    if (cellInfo.data.VALORDEFECTO != null) cellElement.html("");
                },
                editCellTemplate: function (cellElement, cellInfo) {
                    var div = document.createElement("div");
                    cellElement.get(0).appendChild(div);
                    $(div).dxSelectBox({
                        items: columnasExcel,
                        placeholder: "[SELECCIONAR COLUMNA]",
                        value: cellInfo.data.VALORDEFECTO,
                        onValueChanged: function (e) {
                            cellInfo.VALORDEFECTO = e.value;
                            cellInfo.setValue(e.value);
                            $("#grdAsociaIndices").dxDataGrid("saveEditData");
                        },
                    });
                }
            }
        ]
    }).dxDataGrid("instance");

    $("#btnBuscaTra").dxButton({
        text: "Buscar Trámites",
        icon: "search",
        type: "default",
        width: "190",
        onClick: function () {
            var _popup = $("#popupBuscaTra").dxPopup("instance");
            _popup.show();
            $('#BuscarTra').attr('src', $('#SIM').data('url') + 'Utilidades/BuscarTramite?popup=true');
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
            $("#grdAsociaIndices").dxDataGrid("instance").refresh();
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
                        $("#grdAsociaIndices").dxDataGrid("instance").refresh();
                    }
                });
            });
        }
    }

    var btnAsociaInd = $("#btnAsociaIndices").dxButton({
        text: "Asociar Indices",
        type: "default",
        disabled: true,
        onClick: function () {
            indicesSerieDocumentalStore = null;
            var Indices = [];
            if (!EnEdicion) {
                $.getJSON($('#SIM').data('url') + 'GestionDocumental/api/MasivosApi/ObtenerIndicesSerieDocumental', { codSerie: 12 })
                    .done(function (data) {
                        AsignarIndicesDoc(data);
                        ArrIndices = data;
                    });
            } else {
                $.getJSON($('#SIM').data('url') + 'GestionDocumental/api/MasivosApi/EditarIndicesMasivo', { IdSolicitud: IdSolicitud })
                .done(function (data) {
                    if (data.length > 0) {
                        AsignarIndicesDoc(data);
                        ArrIndices = data;
                    }
                });
            }
            columnasExcel = gridExcel.option("columns");
            const index = columnasExcel.indexOf("ID");
            if (index > -1) {
                columnasExcel.splice(index, 1);
            }
            var popupInd = $("#popupIndices").dxPopup("instance");
            popupInd.show();
        }
    }).dxButton("instance");

    var btnFirmas = $("#btnFirmas").dxButton({
        text: "Firmas",
        type: "default",
        disabled: true,
        onClick: function () {
            $("#grdFirmas").dxDataGrid({ dataSource: firmasDocumento });
            $.getJSON($('#SIM').data('url') + 'GestionDocumental/api/MasivosApi/ObtenerCantidadFirmas', { IdSolicitud: IdSolicitud })
                .done(function (data) {
                    if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Firmas COD');
                    else {
                        CantFirmas = data.Cantidad;
                        $("#CantFirmas").text(CantFirmas);
                        grdFirmas.repaint();
                        var popupFirm = $("#popupFirmas").dxPopup("instance");
                        popupFirm.show();
                    }
                });
        }
    }).dxButton("instance");

    $("#popupBuscaTra").dxPopup({
        width: 900,
        height: 800,
        showTitle: true,
        title: "Buscar Trámites del SIM"
    });

    var txtTramite = $("#txtTramite").dxTextBox({
        placeholder: "Ingrese el código del trámite...",
        value: "",
    }).dxTextBox("instance");

    var ufPlantilla = $("#fuPlantilla").dxFileUploader({
        allowedFileExtensions: [".pdf"],
        multiple: false,
        selectButtonText: 'Seleccionar Archivo',
        invalidFileExtensionMessage: "El tipo de archivo no esta permitido",
        invalidMaxFileSizeMessage: "Tamaño del archivo demasiado grande",
        labelText: 'o arrastre un archivo aquí',
        uploadMode: "instantly",
        showFileList: true,
        disabled: true,
        uploadUrl: $('#SIM').data('url') + 'GestionDocumental/api/MasivosApi/RecibePlantilla',
        onUploadAborted: (e) => removeFile(e.file.name),
        onUploaded: function (e) {
            $("#loadPanel").dxLoadPanel('instance').hide();
            const obj = JSON.parse(e.request.responseText);
            if (obj.SubidaExitosa) {
                IdSolicitud = obj.IdSolicitud;
                DevExpress.ui.dialog.alert(obj.MensajeExito, 'Plantilla COD');
                btnPreview.option("disabled", false);
                btnFirmas.option("disabled", false);
            } else {
                DevExpress.ui.dialog.alert(obj.MensajeError, 'Plantilla COD');
                ufPlantilla.reset();
            }
        },
        onUploadStarted: function (e) {
            $("#loadPanel").dxLoadPanel('instance').show();
        },
        onUploadError: function (e) {
            $("#loadPanel").dxLoadPanel('instance').hide();
            DevExpress.ui.dialog.alert('Error Subiendo Archivo: ' + e.request.responseText, 'Subir plantilla COD');
        },
        onValueChanged: function (e) {
            var url = e.component.option('uploadUrl');
            url = updateQueryStringParameter(url, 'IdSolicitud', IdSolicitud);
            e.component.option('uploadUrl', url);
        }
    }).dxFileUploader("instance");


    var ufExcel = $("#fuListaExcel").dxFileUploader({
        allowedFileExtensions: [".xls", ".xlsx"],
        multiple: false,
        selectButtonText: 'Seleccionar Archivo',
        invalidFileExtensionMessage: "El tipo de archivo no esta permitido",
        invalidMaxFileSizeMessage: "Tamaño del archivo demasiado grande",
        labelText: 'o arrastre un archivo aquí',
        uploadMode: "instantly",
        showFileList: true,
        uploadUrl: $('#SIM').data('url') + 'GestionDocumental/api/MasivosApi/RecibeExcel',
        onUploadAborted: (e) => removeFile(e.file.name),
        onUploaded: function (e) {
            $("#loadPanel").dxLoadPanel('instance').hide();
            const obj = JSON.parse(e.request.responseText);
            if (obj.SubidaExitosa) {
                DatosExcelStore = null;
                gridExcel.option("dataSource", DatosExcelStore);
                gridExcel.option("columns", []);
                IdSolicitud = obj.IdSolicitud;
                DevExpress.ui.notify(obj.MensajeExito);
                ufPlantilla.option("disabled", false);
                chkEmail.option("disabled", false);
                btnAsociaInd.option("disabled", false);
                var _Ruta = $("#SIM").data("url") + "GestionDocumental/api/MasivosApi/CargaExcel";
                $.getJSON(_Ruta, { IdSolicitud: IdSolicitud }).done(function (data) {
                    if (data.length > 0) {
                        var columnsIn = data[0];
                        for (var key in columnsIn) {
                            var columns = gridExcel.option("columns");
                            columns.push(key);
                        }
                        gridExcel.option("columns", columns);
                        DatosExcelStore = new DevExpress.data.LocalStore({
                            key: columns[0],
                            data: data,
                            name: 'DatosExcelStore'
                        });
                        gridExcel.option("dataSource", DatosExcelStore);
                    }
                });
            } else {
                DevExpress.ui.dialog.alert(obj.MensajeError, 'Listado Excel');
                ufExcel.reset();
            }
        },
        onUploadStarted: function (e) {
            $("#loadPanel").dxLoadPanel('instance').show();
        },
        onUploadError: function (e) {
            $("#loadPanel").dxLoadPanel('instance').hide();
            DevExpress.ui.dialog.alert('Error Subiendo Archivo: ' + e.request.responseText, 'Subir plantilla COD');
        },
        onValueChanged: function (e) {
            var url = e.component.option('uploadUrl');
            url = updateQueryStringParameter(url, 'IdSolicitud', IdSolicitud);
            e.component.option('uploadUrl', url);
        }
    }).dxFileUploader("instance");

    var chkEmail = $("#chkEmail").dxCheckBox({
        value: false,
        disabled: true,
        onValueChanged(e) {
            if (e.event) {
                if (!e.component.option('disable')) {
                    var columns = gridExcel.option("columns");
                    if (!columns.some(item => item.toLowerCase() == 'email'.toLowerCase())) {
                        DevExpress.ui.notify("Para poder enviar la comunicación por correo electrónico el archivo de Excel debe contener la columna con el dato correo electrónico!");
                        chkEmail.option("value", false);
                    }
                }
            }
        }
    }).dxCheckBox("instance");

    $("#btnGuardaIndicesDoc").dxButton({
        text: "Asociar Indices",
        type: "default",
        onClick: function () {
            var Indices = indicesSerieDocumentalStore._array;
            for (const indice of Indices) {

                if ((indice.VALOR !== null && indice.VALOR !== "") && (indice.VALORDEFECTO !== "" && indice.VALORDEFECTO !== null)) {
                    indice.VALOR = "";
                }
            }
            ArrIndices = Indices;
            var popupInd = $("#popupIndices").dxPopup("instance");
            popupInd.hide();
        }
    });

    $("#btnGuardaFirmas").dxButton({
        text: "Guardar Firmas Documento",
        type: "default",
        onClick: function () {
            if (firmasDocumento.length <= CantFirmas) {
                var popupFirmas = $("#popupFirmas").dxPopup("instance");
                popupFirmas.hide();
            } else DevExpress.ui.dialog.alert('La cantidad de firmas es superior a las etiquetas de firmas de la plantilla!');
        }
    });

    $('#cboFuncionario').dxLookup({
        dataSource: funcionariosDataSource,
        placeholder: '[Seleccionar Funcionario]',
        title: 'Funcionario',
        displayExpr: 'FUNCIONARIO',
        valueExpr: 'CODFUNCIONARIO',
        cancelButtonText: 'Cancelar',
        pageLoadingText: 'Cargando...',
        refreshingText: 'Refrescando...',
        searchPlaceholder: 'Buscar',
        noDataText: 'Sin Datos',
    });


    $("#btnCancelIndDoc").dxButton({
        text: "Cancelar",
        type: "default",
        onClick: function () {
            ArrIndices = null;
            var popupInd = $("#popupIndices").dxPopup("instance");
            popupInd.hide();
        }
    });

    $('#agregarFuncionario').dxButton({
        icon: 'plus',
        text: '',
        width: '30x',
        type: 'success',
        elementAttr: { style: "float: left;" },
        onClick: function (params) {
            var item = $('#cboFuncionario').dxLookup('instance').option('selectedItem');
            if (item != null) {
                if (firmasDocumento.findIndex(f => f.CODFUNCIONARIO == item.CODFUNCIONARIO) == -1) {
                    var orden = 0;
                    firmasDocumento.forEach(fd => {
                        if (fd.ORDEN > orden) {
                            orden = fd.ORDEN;
                        }
                    });
                    orden++;
                    firmasDocumento.push({ CODFUNCIONARIO: item.CODFUNCIONARIO, FUNCIONARIO: item.FUNCIONARIO, ORDEN: orden });
                    $("#grdFirmas").dxDataGrid({ dataSource: firmasDocumento });
                } else DevExpress.ui.dialog.alert('El funcionario ya se encuentra registrado.');
            }
        }
    });

    var grdFirmas = $("#grdFirmas").dxDataGrid({
        dataSource: firmasDocumento,
        allowColumnResizing: true,
        height: '75%',
        width: '100%',
        loadPanel: { text: 'Cargando Datos...' },
        selection: {
            mode: 'single',
        },
        columns: [
            { dataField: "CODFUNCIONARIO", caption: 'CODIGO', dataType: 'number', visible: false, },
            { dataField: "FUNCIONARIO", caption: 'NOMBRE', width: '60%', dataType: 'string', visible: true },
            { dataField: 'ORDEN', caption: 'ORDEN', alignment: 'center', width: '25%', dataType: 'number', visible: true },
            {
                alignment: 'center',
                width: '5%',
                cellTemplate: function (cellElement, cellInfo) {
                    var div = document.createElement("div");
                    cellElement.get(0).appendChild(div);
                    $(div).dxButton({
                        icon: 'trash',
                        width: '100%',
                        onClick: function () {
                            var result = DevExpress.ui.dialog.confirm("Está Seguro(a) de Eliminar la Firma ?", "Confirmación");
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var index = firmasDocumento.indexOf(i => i.ORDEN == cellInfo.data.ORDEN) - 1;
                                    firmasDocumento.splice(index, 1);
                                    firmasDocumento.forEach((element, index) => {
                                        element.ORDEN = index + 1;
                                    });
                                    $("#grdFirmas").dxDataGrid({ dataSource: firmasDocumento });
                                }
                            });
                        }
                    });

                },
            }
        ],
    }).dxDataGrid("instance");


    var btnRadicar = $("#btnRadicar").dxButton({
        text: "Radicar COD",
        type: "default",
        onClick: function () {
            $("#loadPanel").dxLoadPanel('instance').show();
            var _Tramite = txtTramite.option("value");
            if (_Tramite == "") {
                var columns = gridExcel.option("columns");
                if (!columns.some(item => item.toLowerCase() === 'codtramite')) {
                    $("#loadPanel").dxLoadPanel('instance').hide();
                    DevExpress.ui.notify("Para poder asociar el documento a un trámite debe proporcionar un único código general o por documento!");
                    return;
                }
            } else {
                if (!TramiteValido) {
                    $("#loadPanel").dxLoadPanel('instance').hide();
                    DevExpress.ui.notify("Si el trámite es general para todos los documentos debe ser validado para verificar que exista en el SIM!");
                    return;
                }
            }
            var parametros = { IdSolicitud: IdSolicitud };
            var _Ruta = $("#SIM").data("url") + "GestionDocumental/api/MasivosApi/RadicarMasivo?IdSolicitud=" + IdSolicitud;
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(parametros),
                contentType: "application/json",
                success: function (data) {
                    if (!data.isSuccess) {
                        $("#loadPanel").dxLoadPanel('instance').hide();
                        DevExpress.ui.dialog.alert('Ocurrió un error ' + data.message, 'Radicación Masivos');
                    } else {
                        $("#loadPanel").dxLoadPanel('instance').hide();
                        DetalleMasivo.hide();
                        DevExpress.ui.dialog.alert(data.message, 'Radicación Masivos');
                        var fileURL = 'data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8;base64,' + data.responseFile;
                        var link = document.createElement('a');
                        link.href = fileURL;
                        link.download = "resultado.xlsx";
                        link.click();
                    }
                }
            });
        }
    }).dxButton("instance"); 

    var btnPreview = $("#btnPreview").dxButton({
        text: "Previsualizar muestra",
        type: "default",
        disabled: true,
        onClick: function () {
            $("#loadPanel").dxLoadPanel('instance').show();
            var _Tramite = txtTramite.option("value");
            var _EnviarEmail = chkEmail.option("value");
            var parametros = { IdSolicitud: IdSolicitud, CodTramite: _Tramite, EnviarEmail: _EnviarEmail, Indices: ArrIndices };
            var _Ruta = $("#SIM").data("url") + "GestionDocumental/api/MasivosApi/PrevisualizaMasivo";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(parametros),
                contentType: "application/json",
                success: function (data) {
                    if (!data.isSuccess) {
                        $("#loadPanel").dxLoadPanel('instance').hide();
                        DevExpress.ui.dialog.alert('Ocurrió un error ' + data.message, 'Previsualización Radicación Masivos');
                    } else {
                        $("#loadPanel").dxLoadPanel('instance').hide();
                        var pdfWindow = window.open("");
                        pdfWindow.document.write("<iframe width='100%' height='100%' src='data:application/pdf;base64, " + data.responseFile + "'></iframe>")
                    }
                }
            });
            $("#loadPanel").dxLoadPanel('instance').hide();
        }
    }).dxButton("instance");

    var btnGuardar = $("#btnGuardar").dxButton({
        text: "Guardar datos",
        type: "default",
        onClick: function () {
            var _Tramite = txtTramite.option("value");
            var _EnviarEmail = chkEmail.option("value");
            var params = {};
            var result = DevExpress.ui.dialog.confirm('El proceso de radicación masiva esta completo?', 'Confirmación');
            result.done(function (dialogResult) {
                if (dialogResult) {
                    if (firmasDocumento.length < CantFirmas) {
                        DevExpress.ui.dialog.alert('La cantidad de firmas es inferior a las etiquetas de firmas de la plantilla!');
                        return;
                    }
                    if (_Tramite == "" || _Tramite == null) {
                        var columns = gridExcel.option("columns");
                        if (!columns.some(item => item.toLowerCase() == 'codtramite'.toLowerCase())) {
                            DevExpress.ui.dialog.alert("El proceso de envío masivo de correspondencia requiere de un código de trámite para asociar los documentos, ya se como columna del archivo de Excel o un código de trámite para todo el proceso!");
                            return;
                        }
                    }
                    params = { TemaMasivo: Tema, CodFuncionario: CodFunc, IdSolicitud: IdSolicitud, Completo: true, Indices: ArrIndices, CodTramite: _Tramite, EnviarEmail: _EnviarEmail, Firmas: firmasDocumento };
                } else params = { TemaMasivo: Tema, CodFuncionario: CodFunc, IdSolicitud: IdSolicitud, Completo: false, Indices: ArrIndices, CodTramite: _Tramite, EnviarEmail: _EnviarEmail, Firmas: firmasDocumento };
                var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/MasivosApi/GuardaMasivo";
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
                            $('#grdListaMasivos').dxDataGrid("instance").refresh();
                            DetalleMasivo.hide();
                            EnEdicion = false;
                        }
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                    }
                });
            });
        }
    }).dxButton("instance");

    $("#popupIndices").dxPopup({
        width: 900,
        height: 800,
        showTitle: true,
        title: "Asociar indices del documeno",
        onShown: function () {
            $("#grdAsociaIndices").dxDataGrid("instance").option("dataSource", indicesSerieDocumentalStore);
        }
    });

    var FirmaPropia = $("#chkFirmaPropia").dxCheckBox({
        text: "Firma propia",
        value: true,
        onValueChanged: function (e) {
            if (e.value) {
                Encargo.option("value", !e.value);
                Adhoc.option("value", !e.value);
                cargos.option("visible", false);
            }
        }
    }).dxCheckBox("instance");

    var Encargo = $("#chkFirmaEncargo").dxCheckBox({
        text: "Firmaré con funciones de encargo",
        value: false,
        onValueChanged: function (e) {
            if (e.value) {
                Adhoc.option("value", !e.value);
                FirmaPropia.option("value", !e.value);
                cargos.option("visible", true);
            }
        }
    }).dxCheckBox("instance");

    var Adhoc = $("#chkFirmaAdhoc").dxCheckBox({
        text: "Ad Hoc",
        value: false,
        onValueChanged: function (e) {
            if (e.value) {
                FirmaPropia.option("value", !e.value);
                Encargo.option("value", !e.value);
                cargos.option("visible", true);
            }
        }
    }).dxCheckBox("instance");

    var cargos = $('#cboCargos').dxLookup({
        dataSource: cargosDataSource,
        placeholder: '[Seleccionar Cargo]',
        title: 'Cargo',
        displayExpr: 'NOMBRE',
        valueExpr: 'CODCARGO',
        cancelButtonText: 'Cancelar',
        pageLoadingText: 'Cargando...',
        refreshingText: 'Refrescando...',
        searchPlaceholder: 'Buscar',
        noDataText: 'Sin Datos',
        visible: false,
    }).dxLookup("instance");

    var popFirmar = $("#popupFirmar").dxPopup({
        width: 600,
        height: 350,
        showTitle: true,
        title: "Aprobar/Rechazar firmas de la plantilla COD"
    }).dxPopup("instance");

    $("#popupFirmas").dxPopup({
        width: 700,
        height: 600,
        showTitle: true,
        title: "Firmas del documento COD (Plantilla)",
        onShown: function () {
            grdFirmas.refresh();
            $("#CantFirmas").text(CantFirmas);
        }
    });

    var DetalleMasivo =$("#popDetalleMasivo").dxPopup({
        fullScreen: true,
        showTitle: true,
        title: "Generar / Modificar proceso de radicación masiva de COD",
        onHiding: function (e) {
            $('#grdListaMasivos').dxDataGrid("instance").refresh();
            gridExcel.option("dataSource", null);
            gridExcel.repaint();
            gridExcel.refresh();
        }
    }).dxPopup("instance");

    var txtTema = $("#txtTemaMasivo").dxTextBox({
        placeholder: "Ingrese el tema para la radicación masiva",
        value: ""
    }).dxTextBox("instance");

    $("#btnNuevoMasivo").dxButton({
        icon: 'doc',
        type: "default",
        text: "Nuevo proceso de radicacón",
        onClick: function () {
            Tema = $("#txtTemaMasivo").dxTextBox("instance").option("value");
            if (Tema != "") {
                indicesSerieDocumentalStore = null;
                ufPlantilla.option("disabled", true);
               // chkEmail.option("disabled", false);
                btnAsociaInd.option("disabled", true);
                btnFirmas.option("disabled", true);
                gridExcel.option("dataSource", null);
                DetalleMasivo.option("title", "Generar / Modificar proceso de radicación masiva de COD " + Tema);
                DetalleMasivo.show();
            } else {
                DevExpress.ui.dialog.alert("Para crear un proceso de radicación masiva debe proporcionar un tema pra identificarlo!", 'Radicación Masiva COD');
            }
        }
    });

    $("#grdListaMasivos").dxDataGrid({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "ID",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "GestionDocumental/api/MasivosApi/ListadoMasivos?CodFunc=" + CodFunc);
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
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20, 50]
        },
        selection: {
            mode: 'single'
        },
        hoverStateEnabled: true,
        remoteOperations: true,
        columns: [
            { dataField: 'ID', width: '5%', caption: 'Identificador', alignment: 'center' },
            { dataField: 'TEMA', width: '20%', caption: 'Tema del proceso de Radicación', dataType: 'string' },
            { dataField: 'D_FECHA', width: '15%', caption: 'Fecha del Proceso', dataType: 'date', format: 'MMM dd yyyy HH:mm' },
            { dataField: 'CANTIDAD_FILAS', width: '20%', caption: 'Documentos a generar', dataType: 'string' },
            { dataField: 'ESTADO', width: '10%', caption: 'Estado', dataType: 'string' },
            { dataField: 'MENSAJE', dataType: 'string', visible: false },
            { dataField: 'IDSOLICITUD', dataType: 'string', visible: false },
            { dataField: 'CODTRAMITE', dataType: 'string', visible: false },
            { dataField: 'ENVIACORREO', dataType: 'string', visible: false },
            { dataField: 'FUNCIONARIOFIRMA', dataType: 'number', visible: false },
            { dataField: 'FUNCIONARIOELABORA', dataType: 'number', visible: false },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if ((options.data.FUNCIONARIOELABORA == CodFunc && options.data.ESTADO == "ELABORACION") || PuedeRadicar == "1") {
                        $('<div/>').dxButton({
                            icon: 'edit',
                            type: 'success',
                            hint: 'Editar proceso masivo COD',
                            onClick: function (e) {
                                EnEdicion = true;
                                FuncElabora = options.data.FUNCIONARIOELABORA === CodFunc ? true : false;
                                IdSolicitud = options.data.IDSOLICITUD;
                                Tema = options.data.TEMA;
                                ufPlantilla.option("disabled", false);
                                chkEmail.option("disabled", false);
                                btnAsociaInd.option("disabled", false);
                                gridExcel.option("dataSource", null);
                                gridExcel.repaint();
                                gridExcel.option("visible", false);
                                btnGuardar.option("visible", FuncElabora);
                                var _Ruta = $("#SIM").data("url") + "GestionDocumental/api/MasivosApi/CargaExcel";
                                $.getJSON(_Ruta, { IdSolicitud: IdSolicitud }).done(function (data) {
                                    if (data.length > 0) {
                                        var columnsIn = data[0];
                                        var columns = [];
                                        for (var key in columnsIn) {
                                            columns.push(key);
                                        }
                                        gridExcel.option("columns", columns);
                                        DatosExcelStore = new DevExpress.data.LocalStore({
                                            key: columns[0],
                                            data: data,
                                            name: 'DatosExcelStore'
                                        });
                                        btnFirmas.option("disabled", false);
                                        gridExcel.option("dataSource", DatosExcelStore);
                                        gridExcel.option("visible", true);
                                        btnPreview.option("disabled", false);
                                    }
                                });
                                if (options.data.CODTRAMITE != "") {
                                    txtTramite.option("value", options.data.CODTRAMITE);
                                    TramiteValido = true;
                                }
                                if (options.data.ENVIACORREO == "1") {
                                    chkEmail.option("diabled", false);
                                    chkEmail.option("value", true);
                                }
                                _Ruta = $("#SIM").data("url") + "GestionDocumental/api/MasivosApi/ObtenerFirmas";
                                firmasDocumento = [];
                                $.getJSON(_Ruta, { IdSolicitud: IdSolicitud }).done(function (data) {
                                    if (data.length > 0) {
                                        data.forEach(fd => {
                                            firmasDocumento.push({ CODFUNCIONARIO: fd.CODFUNCIONARIO, FUNCIONARIO: fd.FUNCIONARIO, ORDEN: fd.ORDEN });
                                        });
                                        //$("#grdFirmas").dxDataGrid({ dataSource: firmasDocumento });
                                        grdFirmas.option("dataSource", firmasDocumento);
                                    }
                                });

                                DetalleMasivo.option("title", "Generar / Modificar proceso de radicación masiva de COD " + Tema);
                                DetalleMasivo.show();
                            }
                        }).appendTo(container);
                    }
                }
            },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.data.FUNCIONARIOFIRMA == CodFunc) {
                        $('<div/>').dxButton({
                            icon: 'check',
                            type: 'success',
                            hint: 'Firmar la plantilla',
                            onClick: function (e) {
                                IdSolicitud = options.data.IDSOLICITUD;
                                txtRechazo.option("value", "");
                                popFirmar.show();
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
                        type: 'success',
                        hint: 'Previsualizar muestra del documento',
                        onClick: function (e) {
                            $("#loadPanel").dxLoadPanel('instance').show();
                            var _Tramite = txtTramite.option("value");
                            var _EnviarEmail = chkEmail.option("value");
                            var parametros = { IdSolicitud: options.data.IDSOLICITUD, CodTramite: _Tramite, EnviarEmail: _EnviarEmail, Indices: ArrIndices };
                            var _Ruta = $("#SIM").data("url") + "GestionDocumental/api/MasivosApi/PrevisualizaMasivo";
                            $.ajax({
                                type: "POST",
                                dataType: 'json',
                                url: _Ruta,
                                data: JSON.stringify(parametros),
                                contentType: "application/json",
                                success: function (data) {
                                    if (!data.isSuccess) {
                                        $("#loadPanel").dxLoadPanel('instance').hide();
                                        DevExpress.ui.dialog.alert('Ocurrió un error ' + data.message, 'Previsualización Radicación Masivos');
                                    } else {
                                        $("#loadPanel").dxLoadPanel('instance').hide();
                                        var pdfWindow = window.open("");
                                        pdfWindow.document.write("<iframe width='100%' height='100%' src='data:application/pdf;base64, " + data.responseFile + "'></iframe>")
                                    }
                                }
                            });
                            $("#loadPanel").dxLoadPanel('instance').hide();
                        }
                    }).appendTo(container);
                }
            },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'fields',
                        type: 'success',
                        hint: 'Ver la plantilla del proceso',
                        onClick: function (e) {
                            var parametros = { IdSolicitud: options.data.IDSOLICITUD };
                            var _Ruta = $("#SIM").data("url") + "GestionDocumental/api/MasivosApi/LeePlantilla?IdSolicitud=" + options.data.IDSOLICITUD;
                            $.ajax({
                                type: "POST",
                                dataType: 'json',
                                url: _Ruta,
                                data: JSON.stringify(parametros),
                                contentType: "application/json",
                                success: function (data) {
                                    var pdfWindow = window.open("");
                                    pdfWindow.document.write("<iframe width='100%' height='100%' src='data:application/pdf;base64, " + data + "'></iframe>")
                                }
                            });

                        }
                    }).appendTo(container);
                }
            },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.data.FUNCIONARIOELABORA == CodFunc && options.data.MENSAJE != "" && options.data.MENSAJE != null) {
                        $('<div/>').dxButton({
                            icon: 'tips',
                            type: 'success',
                            hint: 'Ver motivo de rechazo firma',
                            onClick: function (e) {
                                MostrarMensage(options.data);
                            }
                        }).appendTo(container);
                    }
                }
            }
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                FuncElabora = data.FUNCIONARIOELABORA === CodFunc ? true : false;
            }
        }
    });

    var popupMess = null;

    var MostrarMensage = function (data) {
        dt = data
        if (popupMess) {
            popupMess.option("contentTemplate", popupOptMess.contentTemplate.bind(this));
        } else {
            popupMess = $("#PopupMensaje").dxPopup(popupOptMess).dxPopup("instance");
        }
        popupMess.show();
    }

    var popupOptMess = {
        width: 600,
        height: 200,
        hoverStateEnabled: true,
        title: "Motivo del rechazo de la firma (Plantilla)",
        closeOnOutsideClick: true,
        contentTemplate: function (container) {
            var divIni = $("<div>").append($("<p><span><b>" + dt.MENSAJE + "</b></span></p>"))
            container.append(divIni);
            return container;
        }
    };

});

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
        $("#txtTramite").dxTextBox("instance").option("value", CodTramite);
        TramiteValido = true;
    } else alert("No se ha ingresado el codigo del trámite");
}

var funcionariosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {

        var d = $.Deferred();

        var searchValueOptions = loadOptions.searchValue;
        var searchExprOptions = loadOptions.searchExpr;

        var skip = (loadOptions.skip ? loadOptions.skip : 0);
        var take = (loadOptions.take ? loadOptions.take : 0);

        if (take != 0) {
            $.getJSON($('#SIM').data('url') + 'Tramites/api/ProyeccionDocumentoApi/Funcionarios', {
                filter: '',
                sort: '[{"selector":"FUNCIONARIO","desc":false}]',
                group: '',
                skip: skip,
                take: take,
                searchValue: (searchValueOptions === undefined || searchValueOptions === null ? '' : searchValueOptions),
                searchExpr: (searchExprOptions === undefined || searchExprOptions === null ? '' : searchExprOptions),
                comparation: 'contains',
                tipoData: 'f',
                noFilterNoRecords: true
            }).done(function (data) {
                d.resolve(data.datos, { totalCount: data.numRegistros });
            }).fail(function (jqxhr, textStatus, error) {
                alert('falla2a: ' + textStatus + ", " + error);
            });
            return d.promise();
        }
    },
    byKey: function (key, extra) {
        return key.toString();
    },
});

var cargosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var searchValueOptions = loadOptions.searchValue;
        var searchExprOptions = loadOptions.searchExpr;

        var skip = (loadOptions.skip ? loadOptions.skip : 0);
        var take = (loadOptions.take ? loadOptions.take : 0);

        if (take != 0) {
            $.getJSON($('#SIM').data('url') + 'Tramites/api/ProyeccionDocumentoApi/Cargos', {
                filter: '',
                sort: '[{"selector":"NOMBRE","desc":false}]',
                group: '',
                skip: skip,
                take: take,
                searchValue: (searchValueOptions === undefined || searchValueOptions === null ? '' : searchValueOptions),
                searchExpr: (searchExprOptions === undefined || searchExprOptions === null ? '' : searchExprOptions),
                comparation: 'contains',
                tipoData: 'f',
                noFilterNoRecords: true
            }).done(function (data) {
                d.resolve(data.datos, { totalCount: data.numRegistros });
            }).fail(function (jqxhr, textStatus, error) {
                alert('falla2a: ' + textStatus + ", " + error);
            });
            return d.promise();
        }
    },
    byKey: function (key, extra) {
        return key.toString();
    },
});