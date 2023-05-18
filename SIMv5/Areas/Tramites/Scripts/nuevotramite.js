let siNoOpciones = [
    { ID: 'S', Nombre: 'SI' },
    { ID: 'N', Nombre: 'NO' },
    { ID: null, Nombre: '-' },
];

let idProceso = -1;
let responsables = null;
let copias = null;

$(document).ready(function () {
    cargando = true;

    $("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
        shadingColor: "rgba(0,0,0,0.4)",
    });

    AjustarTamano();
    $('#asistente').accordion();

    $('#proceso').dxSelectBox({
        items: null,
        width: '100%',
        placeholder: 'Seleccionar Proceso',
        showClearButton: false,
        readOnly: ($('#app').data('p') != '0')
    });

    $.getJSON($('#app').data('url') + 'Tramites/api/TramitesApi/ObtenerProcesos').done(function (data) {
        $('#proceso').dxSelectBox({
            items: data,
            displayExpr: 'NOMBRE',
            valueExpr: 'CODPROCESO',
            placeholder: 'Seleccionar Proceso',
            showClearButton: false,
            onValueChanged: function (data) {
                idProceso = data.value;
                CargarTareas(idProceso);
                CargarIndices();
                idTareaPadre = -1;
                idTarea = -1;
            }
        });

        if ($('#app').data('p') != null && $('#app').data('p') != '') {
            $('#proceso').dxSelectBox('instance').option('value', $('#app').data('p'));
        }
    }).fail(function (jqxhr, textStatus, error) {
        alert('error cargando datos: ' + textStatus + ", " + error);
    });
    
    /*$('#tareaPadre').dxSelectBox({
        dataSource: null,
        width: '100%',
        placeholder: ' -- SIN TAREA PADRE -- ',
        displayExpr: "NOMBRE",
        valueExpr: "CODTAREA",
        showClearButton: false,
        readOnly: ($('#app').data('tp') != '0'),
    });*/

    $('#tarea').dxSelectBox({
        dataSource: null,
        width: '100%',
        placeholder: ' -- SELECCIONAR TAREA -- ',
        displayExpr: "NOMBRE",
        valueExpr: "CODTAREA",
        showClearButton: false,
        readOnly: ($('#app').data('t') != '0'),
    });

    $('#comentario').dxTextBox({
        value: '',
        width: '100%',
        readOnly: false
    });

    $('#cboFuncionario').dxLookup({
        dataSource: funcionariosDataSource,
        placeholder: '[Seleccionar Funcionario]',
        title: 'Funcionario',
        displayExpr: 'NOMBRE',
        valueExpr: 'CODFUNCIONARIO',
        cancelButtonText: 'Cancelar',
        pageLoadingText: 'Cargando...',
        refreshingText: 'Refrescando...',
        searchPlaceholder: 'Buscar',
        noDataText: 'Sin Datos',
    });

    $('#cboFuncionarioC').dxLookup({
        dataSource: funcionariosDataSource,
        placeholder: '[Seleccionar Funcionario]',
        title: 'Funcionario',
        displayExpr: 'NOMBRE',
        valueExpr: 'CODFUNCIONARIO',
        cancelButtonText: 'Cancelar',
        pageLoadingText: 'Cargando...',
        refreshingText: 'Refrescando...',
        searchPlaceholder: 'Buscar',
        noDataText: 'Sin Datos',
    });

    $('#agregarFuncionario').dxButton({
        icon: 'plus',
        text: '',
        width: '30x',
        type: 'success',
        elementAttr: {
            style: "float: left;"
        },
        onClick: function (params) {
            var item = $('#cboFuncionario').dxLookup('instance').option('selectedItem');

            if (item != null) {
                if (responsablesDataSource.length == 0) {

                    responsablesDataSource.push({ CODFUNCIONARIO: item.CODFUNCIONARIO, NOMBRE: item.NOMBRE });
                    $("#grdResponsables").dxDataGrid({
                        dataSource: responsablesDataSource
                    });
                } else {
                    MostrarNotificacion('alert', null, 'Solamente se puede seleccionar un funcionario responsable.');
                }
            }
        }
    });

    $('#agregarFuncionarioC').dxButton({
        icon: 'plus',
        text: '',
        width: '30x',
        type: 'success',
        elementAttr: {
            style: "float: left;"
        },
        onClick: function (params) {
            var item = $('#cboFuncionarioC').dxLookup('instance').option('selectedItem');

            if (item != null) {
                if (copiasDataSource.findIndex(f => f.CODFUNCIONARIO == item.CODFUNCIONARIO) == -1) {

                    copiasDataSource.push({ CODFUNCIONARIO: item.CODFUNCIONARIO, NOMBRE: item.NOMBRE });
                    $("#grdCopias").dxDataGrid({
                        dataSource: copiasDataSource
                    });
                } else {
                    MostrarNotificacion('alert', null, 'El funcionario ya se encuentra registrado.');
                }
            }
        }
    });

    $("#grdResponsables").dxDataGrid({
        dataSource: null,
        allowColumnResizing: true,
        height: '75%',
        width: '100%',
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            enabled: false
        },
        filterRow: {
            visible: false,
        },
        groupPanel: {
            visible: false,
            allowColumnDragging: false,
        },
        editing: {
            mode: 'cell',
            allowUpdating: ($('#app').data('ro') == 'N'),
            allowDeleting: false,
            allowAdding: false,
            useIcons: false
        },
        selection: {
            mode: 'single',
        },
        columns: [
            {
                dataField: "CODFUNCIONARIO",
                caption: 'CODIGO',
                width: '20%',
                dataType: 'number',
                visible: false,
            }, {
                dataField: "NOMBRE",
                caption: 'NOMBRE',
                dataType: 'string',
                allowEditing: false,
                visible: true,
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_ACTIVO == 'N') {
                        cellElement.css('color', 'red');
                    }

                    cellElement.html(cellInfo.data.NOMBRE);
                },
            }, {
                alignment: 'center',
                width: '5%',
                visible: true,
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_ACTIVO == 'S') {
                        var div = document.createElement("div");
                        cellElement.get(0).appendChild(div);

                        $(div).dxButton({
                            icon: 'trash',
                            width: '100%',
                            onClick: function () {
                                var result = DevExpress.ui.dialog.confirm("Está Seguro(a) de Eliminar el Registro ?", "Confirmación");
                                result.done(function (dialogResult) {
                                    if (dialogResult) {
                                        responsablesDataSource.splice(0, 1);

                                        $("#grdResponsables").dxDataGrid({
                                            dataSource: responsablesDataSource
                                        });
                                    }
                                });
                            }
                        });
                    }
                },
            }
        ],
    });

    $("#grdCopias").dxDataGrid({
        dataSource: null,
        allowColumnResizing: true,
        height: '75%',
        width: '100%',
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            enabled: false
        },
        filterRow: {
            visible: false,
        },
        groupPanel: {
            visible: false,
            allowColumnDragging: false,
        },
        editing: {
            mode: 'cell',
            allowUpdating: ($('#app').data('ro') == 'N'),
            allowDeleting: false,
            allowAdding: false,
            useIcons: false
        },
        selection: {
            mode: 'single',
        },
        columns: [
            {
                dataField: "CODFUNCIONARIO",
                caption: 'CODIGO',
                width: '20%',
                dataType: 'number',
                visible: false,
            }, {
                dataField: "NOMBRE",
                caption: 'NOMBRE',
                dataType: 'string',
                allowEditing: false,
                visible: true,
            }, {
                alignment: 'center',
                width: '5%',
                visible: true,
                cellTemplate: function (cellElement, cellInfo) {
                    var div = document.createElement("div");
                    cellElement.get(0).appendChild(div);

                    $(div).dxButton({
                        icon: 'trash',
                        width: '100%',
                        onClick: function () {
                            var result = DevExpress.ui.dialog.confirm("Está Seguro(a) de Eliminar el Registro ?", "Confirmación");
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    let indexF = copiasDataSource.findIndex(f => f.CODFUNCIONARIO == cellInfo.data.CODFUNCIONARIO);
                                    copiasDataSource.splice(indexF, 1);

                                    $("#grdCopias").dxDataGrid({
                                        dataSource: copiasDataSource
                                    });
                                }
                            });
                        }
                    });
                },
            }
        ],
    });

    $('#almacenar').dxButton(
        {
            icon: '',
            text: 'Crear Tr\u00E1mite',
            width: '200px',
            type: 'success',
            elementAttr: {
                style: "float: right;"
            },
            onClick: function (params) {
                if (ValidarDatosMinimos()) {
                    $("#loadPanel").dxLoadPanel('instance').show();

                    $.postJSON(
                        $('#app').data('url') + 'Tramites/api/TramitesApi/CrearTramite', {
                            CodProceso: $('#proceso').dxSelectBox('instance').option('value'),
                            //CodTareaPadre: $('#tareaPadre').dxSelectBox('instance').option('value'),
                            CodTarea: $('#tarea').dxSelectBox('instance').option('value'),
                            Comentario: $('#comentario').dxTextBox('instance').option('value'),
                            Indices: indicesProcesoStore._array,
                            Responsable: responsablesDataSource[0],
                            Copias: copiasDataSource
                        }
                    ).done(function (data) {
                        $("#loadPanel").dxLoadPanel('instance').hide();

                        var respuesta = data.split(':');

                        if (respuesta[0] == 'OK') {
                            var mensaje = 'Trámite Creado Satisfactoriamente.' + (respuesta[1] != '' ? '<br><br>Tr\u00E1mite: ' + respuesta[1] : '');
                            var result = MostrarNotificacion('alert', null, mensaje);
                            result.done(function () {
                                if (typeof parent.CerrarPopupNuevoTramite === "function") {
                                    parent.CerrarPopupNuevoTramite();
                                }
                            });
                        } else {
                            var result = MostrarNotificacion('alert', null, 'ERROR: ' + respuesta[1]);
                        }
                    }).fail(function (jqxhr, textStatus, error) {
                        $("#loadPanel").dxLoadPanel('instance').hide();
                        MostrarNotificacion('alert', null, 'ERROR DE INVOCACION: ' + textStatus + ", " + error);
                        MostrarNotificacion('alert', null, 'Se pudieron generar datos inconsistentes. Por favor cerrar esta ventana y revisar si el trámite fue registrado.');
                    });
                }
            }
        }
    );
});

function ValidarDatosMinimos() {
    if ($('#proceso').dxSelectBox('instance').option('value') == null) {
        MostrarNotificacion('alert', null, 'Proceso Requerido.');
        return false;
    }

    if ($('#tarea').dxSelectBox('instance').option('value') == null) {
        MostrarNotificacion('alert', null, 'Tarea Requerida.');
        return false;
    }

    if ($('#comentario').dxTextBox('instance').option('value') == null || $('#comentario').dxTextBox('instance').option('value').trim() == '') {
        MostrarNotificacion('alert', null, 'Comentario Requerido.');
        return false;
    }

    if (responsablesDataSource.length == 0) {
        MostrarNotificacion('alert', null, 'Debe seleccionar por lo menos un responsable.');
        return false;
    }

    return true;
}

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

function CargarTareas() {
    $.getJSON($('#app').data('url') + 'Tramites/api/TramitesApi/ObtenerTareasProceso', {
        codProceso: idProceso,
    }).done(function (data) {
        /*$("#tareaPadre").dxSelectBox({
            dataSource: data
        });*/

        $("#tarea").dxSelectBox({
            dataSource: data
        });

        if (cargando && $('#app').data('t') != null && $('#app').data('t') != '')
        {
            $('#tarea').dxSelectBox('instance').option('value', $('#app').data('t'));
        }

        cargando = false;
    });
}


function CargarIndices() {
    $.getJSON($('#app').data('url') + 'Tramites/api/TramitesApi/ObtenerIndicesProceso', {
        codProceso: idProceso,
    }).done(function (data) {
        AsignarIndices(data);
    });
}

function AsignarIndices(indices) {
    opcionesLista = [];

    indicesProcesoStore = new DevExpress.data.LocalStore({
        key: 'CODINDICE',
        data: indices,
        name: 'indicesProceso'
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
            $.getJSON($('#app').data('url') + 'Tramites/api/TramitesApi/ObtenerIndiceValoresLista?id=' + valor.idLista).done(function (data) {
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
}

function CargarGridIndices() {
    $("#grdIndices").dxDataGrid({
        dataSource: indicesProcesoStore,
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

var funcionariosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {

        var d = $.Deferred();

        var searchValueOptions = loadOptions.searchValue;
        var searchExprOptions = loadOptions.searchExpr;

        var skip = (loadOptions.skip ? loadOptions.skip : 0);
        var take = (loadOptions.take ? loadOptions.take : 0);

        if (take != 0) {
            $.getJSON($('#app').data('url') + 'Tramites/api/TramitesApi/Funcionarios', {
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

var responsablesDataSource = [];
var copiasDataSource = [];

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
        return DevExpress.ui.dialog.alert(msg, 'Nuevo Tr\u00E1mite');
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