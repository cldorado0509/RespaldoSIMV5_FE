$(document).ready(function () {
    $("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
    });

    switch ($('#app').data('r')) {
        case 'C':
            $('#cumplimientoCell').css('background-color', 'limegreen');
            $('#cumplimiento').text('CUMPLE');
            break;
        case 'N':
            $('#cumplimientoCell').css('background-color', 'red');
            $('#cumplimiento').text('NO CUMPLE');
            break;
        case 'P':
            $('#cumplimientoCell').css('background-color', 'gray');
            $('#cumplimiento').text('PENDIENTE');
            break;
    }

    $("#txtTextoComplementarioPregunta").dxTextArea({
        width: '100%',
        height: '80px',
        maxLength: 4000,
        readOnly: ($('#app').data('ro') == 'S'),
    });

    $("#popDetallePregunta").dxPopup({
        fullScreen: false,
    });

    var tabsData = [
            { text: $('#app').data('t'), pos: 0 },
            { text: 'Datos Generales', pos: 1 },
    ];

    $("#tabOpciones").dxTabs({
        dataSource: tabsData,
        onItemClick: (function (itemData) {
            if (itemData.itemIndex == 0) {
                $('#tab01').css('display', 'block');
                $('#tab02').css('display', 'none');
            }
            else {
                $('#tab02').css('display', 'block');
                $('#tab01').css('display', 'none');
            }
        }),
        selectedIndex: 0,
    });

    $("#txtObservaciones").dxTextArea({
        width: ($('#app').data('ro') == 'N' ? '85%' : '100%'),
        height: '100%',
        maxLength: 4000,
        readOnly: ($('#app').data('ro') == 'S'),
        placeHolder: '[Observaciones]',
    });

    $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/ObservacionesEvaluacion', {
        idee: $('#app').data('ee'),
    }).done(function (data) {
        $('#txtObservaciones').dxTextArea('instance').option('value', data);
    });

    $("#btnGuardarObservaciones").dxButton({
        width: '100%',
        height: '100%',
        text: 'ALMACENAR',
        type: 'success',
        onClick: function (params) {
            $.postJSON(
                $('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/ObservacionesEvaluacionActualizar',
                { idEncuestaEvaluacion: $('#app').data('ee'), observaciones: $('#txtObservaciones').dxTextArea('option', 'text') }
            ).done(function (data) {
                MostrarNotificacion('alert', '', 'Observaciones Almacenadas Satisfactoriamente');
            }).fail(function () {
                alert('Error Actualizando Observaciones');
            });
        },
    });

    $("#btnCalcularCumplimiento").dxButton({
        width: '50%',
        height: '50%',
        icon: 'arrowright',
        type: 'success',
        onClick: function (params) {
            $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/CumplimientoContenido', {
                idee: $('#app').data('ee'),
                v: 1,
            }).done(function (data) {
                switch (data) {
                    case 'C':
                        $('#cumplimientoCell').css('background-color', 'limegreen');
                        $('#cumplimiento').text('CUMPLE');
                        break;
                    case 'N':
                        $('#cumplimientoCell').css('background-color', 'red');
                        $('#cumplimiento').text('NO CUMPLE');
                        break;
                    case 'P':
                        $('#cumplimientoCell').css('background-color', 'gray');
                        $('#cumplimiento').text('PENDIENTE');
                        break;
                }
            });
        },
    });

    $("#btnActualizarDatos").dxButton({
        icon: 'refresh',
        onClick: function (params) {
            $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/ActualizarDatosGenerales', {
                idee: $('#app').data('ee'),
            }).done(function (data) {
                if (data != null)
                {
                    $('#medioEntrega').text(data.MedioEntrega);
                    $('#radicado').text(data.Radicado);
                    $('#fechaEntrega').text(data.FechaEntrega);
                    $('#coordenada').text(data.Coordenada);
                    $('#direccion').text(data.Direccion);
                    $('#TonEmitido').text(data.TonEmitido);
                    $('#kgEmitido').text(data.KgEmitido);
                    $('#pm25Emitido').text(data.PM25PEmitido);
                    $('#pm25IEmitido').text(data.PM25IEmitido);
                }
            });
        },
    });

    $("#grdPreguntasEvaluacion").dxDataGrid({
        dataSource: grdPreguntasEvaluacionDataSource,
        allowColumnResizing: true,
        allowSorting: false,
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
        grouping: {
            allowCollapsing: false,
            autoExpandAll: true,
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
        wordWrapEnabled: true,
        columns: [
            {
                dataField: "ID",
                dataType: 'number',
                visible: false,
            }, {
                dataField: "ID_PREGUNTA",
                dataType: 'number',
                visible: false,
            }, {
                dataField: "S_GRUPO",
                caption: 'GRUPO',
                groupIndex: 0,
                dataType: 'string',
                visible: true,
                allowEditing: false,
                groupCellTemplate: function (cellElement, cellInfo) {
                    cellElement.html("<span style='font-weight: bold;'>" + cellInfo.data.key + "</span>");
                }
            }, {
                dataField: "S_PREGUNTA",
                caption: 'PREGUNTA',
                width: '80%',
                dataType: 'string',
                visible: true,
                allowEditing: false,
                cellTemplate: function (cellElement, cellInfo) {
                    cellElement.html(cellInfo.data.S_PREGUNTA);

                    if (cellInfo.data.ID_GRUPO_COMPLEMENTO != null) {
                        if (cellInfo.data.N_VALOR_COMPLEMENTO == cellInfo.data.N_RESPUESTA) {
                            var div = document.createElement("div");
                            div.style.cssText = 'float:right; margin: -4px;';
                            cellElement.get(0).appendChild(div);

                            $(div).dxButton({
                                width: '50px',
                                height: '25px',
                                text: '...',
                                type: 'normal',
                                onClick: function (params) {
                                    $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/ObtenerDetalleRespuesta', {
                                        idRespuesta: cellInfo.data.ID,
                                    }).done(function (data) {
                                        var ventanaDetallePregunta = $("#popDetallePregunta").dxPopup("instance");
                                        ventanaDetallePregunta.option('title', cellInfo.data.S_PREGUNTA);
                                        ventanaDetallePregunta.show();

                                        $('#txtTextoComplementarioPregunta').dxTextArea({
                                            width: '100%',
                                            height: '80px',
                                            maxLength: 4000,
                                            value: (data.texto == null ? '' : data.texto),
                                            readOnly: ($('#app').data('ro') == 'S'),
                                        });

                                        var lista = $("#lstDetallePregunta").dxList({
                                            dataSource: data.opciones,
                                            disabled: ($('#app').data('ro') == 'S'),
                                            allowItemDeleting: false,
                                            showSelectionControls: true,
                                            selectionMode: 'multiple',
                                            scrollingEnabled: false,
                                            itemTemplate: function (item) {
                                                return item.S_DESCRIPCION;
                                            },
                                            displayExpr: function (item) {
                                                return item.S_DESCRIPCION;
                                            },
                                            keyExpr: 'ID',
                                        }).dxList('instance');

                                        lista.option('selectedItems', []);

                                        var itemsSeleccionados = [];
                                        for (i = 0; i < data.opciones.length; i++) {
                                            if (data.opciones[i].SELECCIONADO == 1) {
                                                lista.selectItem(i);
                                            }
                                        }

                                        $("#btnAceptarOpciones").dxButton({
                                            width: '150px',
                                            height: '40px',
                                            text: 'Aceptar',
                                            type: 'success',
                                            onClick: function (params) {
                                                var opcionesSeleccionadas = lista.option('selectedItems');
                                                var listaOpciones = '';

                                                for (i = 0; i < opcionesSeleccionadas.length; i++) {
                                                    if (listaOpciones == '')
                                                        listaOpciones = opcionesSeleccionadas[i].ID;
                                                    else
                                                        listaOpciones += ',' + opcionesSeleccionadas[i].ID;
                                                }

                                                $.postJSON(
                                                    $('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/PreguntasDetalleEvaluacionActualizar', { Id: cellInfo.data.ID, Opciones: listaOpciones, Texto: $('#txtTextoComplementarioPregunta').dxTextArea('instance').option('text') }
                                                ).done(function (data) {
                                                    var ventanaDetallePregunta = $("#popDetallePregunta").dxPopup("instance");
                                                    ventanaDetallePregunta.hide();
                                                });
                                            },
                                        });

                                        $("#btnCancelarOpciones").dxButton({
                                            width: '150px',
                                            height: '40px',
                                            text: 'Cancelar',
                                            type: 'danger',
                                            onClick: function (params) {
                                                var ventanaDetallePregunta = $("#popDetallePregunta").dxPopup("instance");
                                                ventanaDetallePregunta.hide();
                                            },
                                        });
                                    });
                                }
                            });
                        }
                    }
                },
            }, {
                dataField: 'RESPUESTA',
                caption: 'RESPUESTA',
                width: '20%',
                dataType: 'string',
                allowEditing: ($('#app').data('ro') == 'N'),
                cellTemplate: function (cellElement, cellInfo) {
                    cellElement.css('text-align', 'center');

                    if (cellInfo.data.N_RESPUESTA == null && cellInfo.data.S_RESPUESTA == null) {
                        cellElement.html('-');
                    } else {
                        switch (cellInfo.data.N_TIPO_RESPUESTA) {
                            case 1: // SI/NO
                                if (cellInfo.data.N_RESPUESTA != null) {
                                    cellElement.html(cellInfo.data.N_RESPUESTA == 1 ? 'SI' : 'NO');
                                }
                                break;
                            case 10: // PERIODO
                                if (cellInfo.data.N_RESPUESTA != null)
                                    cellElement.html(cellInfo.data.N_RESPUESTA == 1 ? 'PRIMERO' : 'SEGUNDO');
                                break;
                            case 2: // CUMPLE/NO CUMPLE
                                if (cellInfo.data.N_RESPUESTA != null) {
                                    cellElement.html(cellInfo.data.N_RESPUESTA == 1 ? 'CUMPLE' : 'NO CUMPLE');

                                    if (cellInfo.data.N_RESPUESTA == 1) {
                                        cellElement.css('background-color', 'limegreen');
                                        cellElement.css('color', 'white');
                                    } else {
                                        cellElement.css('background-color', 'red');
                                        cellElement.css('color', 'white');
                                    }
                                }
                                break;
                            case 3: // NUMERO
                                if (cellInfo.data.N_RESPUESTA != null)
                                    cellElement.html(cellInfo.data.N_RESPUESTA);
                                break;
                            case 4: // %
                                if (cellInfo.data.N_RESPUESTA != null)
                                    cellElement.html(Math.round(cellInfo.data.N_RESPUESTA * 100 * 100)/100 + '%');
                                break;
                            case 5: // TEXTO
                                if (cellInfo.data.S_RESPUESTA != null)
                                    cellElement.html(cellInfo.data.S_RESPUESTA);
                                break;
                        }
                    }
                },
                editCellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_CALCULADO == 'N' || cellInfo.data.S_CALCULADO == 'E') {
                        switch (cellInfo.data.N_TIPO_RESPUESTA) {
                            case 1: // SI/NO
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                $(div).dxSelectBox({
                                    dataSource: siNoOpciones,
                                    displayExpr: "Nombre",
                                    valueExpr: "ID",
                                    placeholder: "[SI/NO]",
                                    value: cellInfo.data.N_RESPUESTA,
                                    onValueChanged: function (e) {
                                        cellInfo.setValue(e.value);
                                        $("#grdPreguntasEvaluacion").dxDataGrid("saveEditData");
                                    },
                                });
                                break;
                            case 10: // PERIODO
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                $(div).dxSelectBox({
                                    dataSource: periodoOpciones,
                                    displayExpr: "Nombre",
                                    valueExpr: "ID",
                                    placeholder: "[PERIODO]",
                                    value: cellInfo.data.N_RESPUESTA,
                                    onValueChanged: function (e) {
                                        cellInfo.setValue(e.value);
                                        $("#grdPreguntasEvaluacionT").dxDataGrid("saveEditData");
                                    },
                                });
                                break;
                            case 2: // CUMPLE/NO CUMPLE
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                $(div).dxSelectBox({
                                    dataSource: cumpleNoCumpleOpciones,
                                    displayExpr: "Nombre",
                                    valueExpr: "ID",
                                    placeholder: "[CUMPLE/NO CUMPLE]",
                                    value: cellInfo.data.N_RESPUESTA,
                                    onValueChanged: function (e) {
                                        cellInfo.setValue(e.value);
                                    },
                                });
                                break;
                            case 3: // NUMERO
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                $(div).dxNumberBox({
                                    value: cellInfo.data.N_RESPUESTA,
                                    showSpinButtons: false,
                                    onValueChanged: function (e) {
                                        cellInfo.setValue(e.value);
                                    },
                                });
                                break;
                            case 4: // %
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                $(div).dxNumberBox({
                                    value: cellInfo.data.N_RESPUESTA,
                                    showSpinButtons: false,
                                    onValueChanged: function (e) {
                                        cellInfo.setValue(e.value);
                                    },
                                });
                                break;
                            case 5: // TEXTO
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                $(div).dxTextBox({
                                    value: cellInfo.data.S_RESPUESTA,
                                    onValueChanged: function (e) {
                                        cellInfo.setValue(e.value);
                                    },
                                });
                                break;
                        }
                    } else {
                        switch (cellInfo.data.N_TIPO_RESPUESTA) {
                            case 1: // SI/NO
                                if (cellInfo.data.N_RESPUESTA != null)
                                    cellElement.html(cellInfo.data.N_RESPUESTA == 1 ? 'SI' : 'NO');
                                break;
                            case 10: // PERIODO
                                if (cellInfo.data.N_RESPUESTA != null)
                                    cellElement.html(cellInfo.data.N_RESPUESTA == 1 ? 'PRIMERO' : 'SEGUNDO');
                                break;
                            case 2: // CUMPLE/NO CUMPLE
                                if (cellInfo.data.N_RESPUESTA != null)
                                    cellElement.html(cellInfo.data.N_RESPUESTA == 1 ? 'CUMPLE' : 'NO CUMPLE');
                                break;
                            case 3: // NUMERO
                                if (cellInfo.data.N_RESPUESTA != null)
                                    cellElement.html(cellInfo.data.N_RESPUESTA);
                                    //cellInfo.setValue(cellInfo.data.N_RESPUESTA);
                                break;
                            case 4: // %
                                if (cellInfo.data.N_RESPUESTA != null)
                                    cellElement.html((cellInfo.data.N_RESPUESTA * 100) + '%');
                                break;
                            case 5: // TEXTO
                                if (cellInfo.data.N_RESPUESTA != null)
                                    cellElement.html(cellInfo.data.S_RESPUESTA);
                                break;
                        }
                    }
                }
            }
        ]
    });
});

var siNoOpciones = [
                { ID: 1, Nombre: 'SI' },
                { ID: 2, Nombre: 'NO' },
                { ID: null, Nombre: '-' },
];

var cumpleNoCumpleOpciones = [
                { ID: 1, Nombre: 'CUMPLE' },
                { ID: 2, Nombre: 'NO CUMPLE' },
                { ID: null, Nombre: '-' },
];

var periodoOpciones = [
    { ID: 1, Nombre: 'PRIMERO' },
    { ID: 2, Nombre: 'SEGUNDO' },
    { ID: null, Nombre: '-' },
];


var grdPreguntasEvaluacionDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/PreguntasEvaluacion', {
            idEncuestaEvaluacion: $('#app').data('ee'),
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        });
        return d.promise();
    },
    byKey: function(key) {
        return { id: key };
    },
    update: function (key, values) {
        $.postJSON(
            $('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/PreguntasEvaluacionActualizar', { idEncuestaEvaluacion: $('#app').data('ee'), id: key.ID, idPregunta: key.ID_PREGUNTA, tipoRespuesta: key.N_TIPO_RESPUESTA, valor: values.RESPUESTA }
        ).done(function (data) {
            $('#cumplimientoCell').css('background-color', 'gray');
            $('#cumplimiento').text('PENDIENTE');

            var grid = $('#grdPreguntasEvaluacion').dxDataGrid('instance');
            grid.refresh();
        });
    }
});

$.postJSON = function (url, data) {
    var o = {
        url: url,
        type: "POST",
        dataType: "json",
        contentType: 'application/json; charset=utf-8'
    };
    if (data !== undefined) {
        o.data = JSON.stringify(data);
    }
    return $.ajax(o);
};

function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        DevExpress.ui.dialog.alert(msg, 'Evaluación PMES');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}
