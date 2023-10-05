var IdSolicitud = "-1";
var indicesSerieDocumentalStore = null;
var columnasExcel;
var opcionesLista = [];
var ArrIndices =  [];
var TramiteValido = false;
$(document).ready(function () {

    $("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
        shadingColor: "rgba(0,0,0,0.4)",
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
            allowedPageSizes: [15, 20 , 50]
        },
        columns: []
    });

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
            CargarIndicesDoc();
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
                        CargarIndicesDoc();
                    }
                });
            });
        }
    }

    function CargarIndicesDoc() {

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

                },
                {
                    dataField: 'VALORDEFECTO', caption: 'COLUMNA EXCEL', dataType: 'string', allowEditing: true,
                    cellTemplate: function (cellElement, cellInfo) {
                        cellElement.css('text-align', 'center');
                        if (cellInfo.data.VALORDEFECTO != null) cellElement.html(cellInfo.data.VALORDEFECTO);
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
                                //var sb = $("#grdAsociaIndices").dxSelectBox('instance');
                                //sb.getDataSource().store().remove(e.value);
                                //sb.getDataSource().reload();
                            },
                        });
                    }
                }
            ]
        }).dxDataGrid("instance");
    }

    var btnAsociaInd = $("#btnAsociaIndices").dxButton({
        text: "Asociar Indices COD",
        type: "default",
        disabled: true,
        onClick: function () {
            $.getJSON($('#SIM').data('url') + 'GestionDocumental/api/MasivosApi/ObtenerIndicesSerieDocumental', { codSerie: 12 })
                .done(function (data) {
                    AsignarIndicesDoc(data);
            });

            //$.getJSON($('#SIM').data('url') + 'Tramites/api/ProyeccionDocumentoApi/ObtenerIndicesSerieDocumental', { codSerie: 12 });


            columnasExcel = $("#grdExcel").dxDataGrid("instance").option("columns");
            const index = columnasExcel.indexOf("ID");
            if (index > -1) { 
                columnasExcel.splice(index, 1); 
            }
            var popupInd = $("#popupIndices").dxPopup("instance");
            popupInd.show();
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
                btnRadicar.option("disabled", false);
                btnPreview.option("disabled", false);
            } else {
                DevExpress.ui.dialog.alert(obj.MensajeError, 'Plantilla COD');
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
        multiple: true,
        selectButtonText: 'Seleccionar Archivo',
        invalidFileExtensionMessage: "El tipo de archivo no esta permitido",
        invalidMaxFileSizeMessage: "Tamaño del archivo demasiado grande",
        labelText: 'o arrastre un archivo aquí',
        uploadMode: "instantly",
        showFileList: false,
        uploadUrl: $('#SIM').data('url') + 'GestionDocumental/api/MasivosApi/RecibeExcel',
        onUploadAborted: (e) => removeFile(e.file.name),
        onUploaded: function (e) {
            $("#loadPanel").dxLoadPanel('instance').hide();
            const obj = JSON.parse(e.request.responseText);
            if (obj.SubidaExitosa) {
                DatosExcelStore = null;
                $("#grdExcel").dxDataGrid("instance").option("dataSource", DatosExcelStore);
                $("#grdExcel").dxDataGrid("instance").option("columns", []);
                IdSolicitud = obj.IdSolicitud;
                DevExpress.ui.notify(obj.MensajeExito);
                ufPlantilla.option("disabled", false);
                //chkEmail.option("disabled", false);
                btnAsociaInd.option("disabled", false);
                var _Ruta = $("#SIM").data("url") + "GestionDocumental/api/MasivosApi/CargaExcel";
                $.getJSON(_Ruta, { IdSolicitud: IdSolicitud }).done(function (data) {
                    if (data.length > 0) {
                        var columnsIn = data[0];
                        for (var key in columnsIn) {
                            var columns = $("#grdExcel").dxDataGrid("instance").option("columns");
                            columns.push(key);
                        }
                        $("#grdExcel").dxDataGrid("instance").option("columns", columns);
                        DatosExcelStore = new DevExpress.data.LocalStore({
                            key: columns[0],
                            data: data,
                            name: 'DatosExcelStore'
                        });
                        $("#grdExcel").dxDataGrid("instance").option("dataSource", DatosExcelStore);
                    }
                });
            } else {
                DevExpress.ui.dialog.alert(obj.MensajeError, 'Anexos Pqrsd');
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
        onValueChanged(data) {
            var columns = $("#grdExcel").dxDataGrid("instance").option("columns");
            if (!columns.some(item => item.toLowerCase() == 'email'.toLowerCase())) {
                DevExpress.ui.notify("Para poder enviar la comunicación por correo electrónico el archivo de Excel debe contener la columna con el dato!");
                chkEmail.option("value", false);

            }
        }
    }).dxCheckBox("instance");

    $("#btnGuardaIndicesDoc").dxButton({
        text: "Asociar Indices COD",
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

    $("#btnCancelIndDoc").dxButton({
        text: "Cancelar",
        type: "default",
        onClick: function () {
            ArrIndices = null;
            var popupInd = $("#popupIndices").dxPopup("instance");
            popupInd.hide();
        }
    });

    var btnRadicar = $("#btnRadicar").dxButton({
        text: "Radicar COD",
        type: "default",
        disabled: true,
        onClick: function () {
            $("#loadPanel").dxLoadPanel('instance').show();
            var _Tramite = txtTramite.option("value");
            if (_Tramite.length == 0) {
                var columns = $("#grdExcel").dxDataGrid("instance").option("columns");
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
            if (ArrIndices.length === 0) {
                $("#loadPanel").dxLoadPanel('instance').hide();
                DevExpress.ui.notify("Para poder radicar los documentos se deben proporcionar las asociaciones de índices!");
                return;
            }
            var _EnviarEmail = chkEmail.option("value");
            var parametros = { IdSolicitud: IdSolicitud, CodTramite: _Tramite, EnviarEmail: _EnviarEmail, Indices: ArrIndices };
            var _Ruta = $("#SIM").data("url") + "GestionDocumental/api/MasivosApi/RadicarMasivo";
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

    $("#popupIndices").dxPopup({
        width: 900,
        height: 800,
        showTitle: true,
        title: "Asociar indices del documeno"
    });
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