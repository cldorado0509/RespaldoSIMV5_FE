var siNoOpciones = [
    { ID: 'S', Nombre: 'SI' },
    { ID: 'N', Nombre: 'NO' },
    { ID: null, Nombre: '-' },
];

var indicesTramiteStore = [];
var opcionesLista = [];

var listoMostrar = false;

$(document).ready(function () {
    $("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
        shadingColor: "rgba(0,0,0,0.4)",
    });

    var tramites = ('' + $('#app').data('t')).split(',');

    var tabsData = [];
    var i = 0;
    tramites.forEach(function (tramite) {
        tabsData.push({ text: tramite, pos: i });

        if (i == 0)
            $('#tab_' + tramite).css('display', 'block');

        indicesTramiteStore.push({ tramite: tramite, datos: null, cargado: false, cargadoListas: false });
        CargarIndices(tramite);

        $.getJSON($('#app').data('url') + 'Tramites/api/TramitesApi/TramiteAsunto?tramite=' + tramite).done(function (data) {
            $('#txtDescripcion_' + tramite).dxTextBox({
                height: '10%',
                readOnly: true,
                value: data
            });
        });

        i++;
    });

    $("#tabOpciones").dxTabs({
        dataSource: tabsData,
        onItemClick: (function (itemData) {
            $('[id^=tab_]').css('display', 'none');
            $('#tab_' + itemData.itemData.text).css('display', 'block');

            posTab = itemData.itemIndex;
        }),
        selectedIndex: 0,
    });

    setTimeout(function () { verificarCargaIndices() }, 5000);
});

function verificarCargaIndices() {
    var cargaFinalizada = true;

    indicesTramiteStore.forEach(function (v, i, a) {
        cargaFinalizada = cargaFinalizada && v.cargado;
    });

    if (cargaFinalizada) {
        CargarListas();
        verificarCargaListas();
    } else {
        setTimeout(function () { verificarCargaIndices() }, 5000);
    }
}

function verificarCargaListas() {
    var cargarGrids = false;
    if (opcionesLista.length > 0) {
        var finalizado = true;

        opcionesLista.forEach(function (v, i, a) {
            finalizado = finalizado && v.cargado;
        });

        if (finalizado) {
            cargarGrids = true;
        } else {
            setTimeout(function () { verificarCargaListas() }, 2000);
        }
    } else {
        cargarGrids = true;
    }

    if (cargarGrids) {
        var tramites = ('' + $('#app').data('t')).split(',');

        tramites.forEach(function (tramite) {
            CargarGridIndices(tramite);
        });

        $('.my-cloak').removeClass('my-cloak');

        listoMostrar = true;
        VisualizarPagina();

        $('#tabOpciones').dxTabs('instance').repaint();
    }
}

function CargarListas() {
    opcionesLista.forEach(function (valor, indice, array) {
        $.getJSON($('#app').data('url') + 'Tramites/api/ProyeccionDocumentoApi/ObtenerIndiceValoresLista?id=' + valor.idLista).done(function (data) {
            var index = opcionesLista.findIndex(ol => ol.idLista == valor.idLista);
            opcionesLista[index].datos = data;
            opcionesLista[index].cargado = true;
        });
    });
}

function CargarIndices(tramite) {
    $.getJSON($('#app').data('url') + 'Tramites/api/ProyeccionDocumentoApi/ObtenerIndicesTramite', {
        t: tramite,
    }).done(function (data) {
        var indexTramite = indicesTramiteStore.findIndex(ol => ol.tramite == tramite);

        indicesTramiteStore[indexTramite].datos = new DevExpress.data.LocalStore({
            key: 'CODINDICE',
            data: data,
            name: 'indicesTramite' + tramite
        });

        indicesTramiteStore[indexTramite].cargado = true;

        data.forEach(function (valor, indice, array) {
            if (valor.TIPO == 5 && valor.ID_LISTA != null) {

                if (opcionesLista.findIndex(ol => ol.idLista == valor.ID_LISTA) == -1) {
                    opcionesLista.push({ idLista: valor.ID_LISTA, datos: null, cargado: false });
                }
            }
        });
    });
}

function CargarGridIndices(tramite) {
    var indexTramite = indicesTramiteStore.findIndex(ol => ol.tramite == tramite);

    $('#grdIndices_' + tramite).dxDataGrid({
        dataSource: indicesTramiteStore[indexTramite].datos,
        allowColumnResizing: true,
        allowSorting: false,
        showRowLines: true,
        rowAlternationEnabled: true,
        showBorders: true,
        height: '90%',
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
                                    $('#grdIndices_' + tramite).dxDataGrid("saveEditData");
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
                                    $('#grdIndices_' + tramite).dxDataGrid("saveEditData");
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
                                        $('#grdIndices_' + tramite).dxDataGrid("saveEditData");
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
                                        $('#grdIndices_' + tramite).dxDataGrid("saveEditData");
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
                                    $('#grdIndices_' + tramite).dxDataGrid("saveEditData");
                                },
                            });
                            break;
                        case 5: // Lista
                            var div = document.createElement("div");
                            cellElement.get(0).appendChild(div);

                            if (cellInfo.data.ID_LISTA != null) {
                                $(div).dxSelectBox({
                                    //dataSource: opcionesLista[cellInfo.data.CODINDICE],
                                    items: opcionesLista[opcionesLista.findIndex(ol => ol.idLista == cellInfo.data.ID_LISTA)].datos,
                                    width: '100%',
                                    //displayExpr: (cellInfo.TIPO_LISTA == 0 ? 'NOMBRE' : cellInfo.CAMPO_NOMBRE),
                                    //valueExpr: (cellInfo.TIPO_LISTA == 0 ? 'NOMBRE' : cellInfo.CAMPO_NOMBRE),
                                    placeholder: "[SELECCIONAR OPCION]",
                                    value: cellInfo.data.VALOR,
                                    searchEnabled: true,
                                    onValueChanged: function (e) {
                                        cellInfo.setValue(e.value);
                                        $('#grdIndices_' + tramite).dxDataGrid("saveEditData");
                                    },
                                });
                            } else {
                                $(div).dxTextBox({
                                    value: cellInfo.data.VALOR,
                                    width: '100%',
                                    onValueChanged: function (e) {
                                        cellInfo.setValue(e.value);
                                        $('#grdIndices_' + tramite).dxDataGrid("saveEditData");
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
                                    $('#grdIndices_' + tramite).dxDataGrid("saveEditData");
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
        ],
        onRowUpdating: function (e) {
            var d = $.Deferred();

            if ((e.oldData.OBLIGA == 1) && (e.newData.VALOR == null || e.newData.VALOR.trim() == '')) {
                d.reject('[' + e.oldData.INDICE + '] Requerido');
                e.cancel = true;
            } else {
                $.postJSON(
                    $('#app').data('url') + 'Tramites/api/TramitesApi/ActualizarIndiceTramite', {
                    CODTRAMITE: tramite,
                    CODINDICE: e.oldData.CODINDICE,
                    VALOR: e.newData.VALOR
                }
                ).done(function (data) {
                    var respuesta = data.split(':');

                    if (respuesta[0] == 'OK') {
                        d.resolve();
                    } else {
                        d.reject(respuesta[1]);
                    }
                }).fail(function (jqxhr, textStatus, error) {
                    return d.reject();
                });

                e.cancel = d.promise();
            }
        }
    });
}

$.postJSON = function (url, data) {
    var o = {
        url: url,
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    };
    if (data !== undefined) {
        o.data = JSON.stringify(data);
    }
    return $.ajax(o);
};


function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        return DevExpress.ui.dialog.alert(msg, 'Proyección Documento');
    } else {
        return DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}

function utf8_to_b64(str) {
    return window.btoa(unescape(encodeURIComponent(str)));
}

function b64_to_utf8(str) {
    return decodeURIComponent(escape(window.atob(str)));
}