$(document).ready(function () {
    /*$("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
    });*/

    unidadesMeta = [
        { id : 'UN', texto : 'UNIDADES' },
        { id : 'PORC', texto : 'PORCENTAJE' },
    ];
    grupos = null;
    estrategias = null;

    $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/Estrategias').done(function (data) {
        grupos = data.grupos;
        estrategias = data.estrategias;
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

    $("#popEstrategia").dxPopup({
        fullScreen: false,
    });

    $("#txtObservaciones").dxTextArea({
        width: ($('#app').data('ro') == 'N' ? '85%' : '100%'),
        height: '100%',
        maxLength: 4000,
        readOnly: ($('#app').data('ro') == 'S'),
        placeHolder: '[Observaciones]',
    });

    $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/ObservacionesEvaluacionT', {
        ideet: $('#app').data('eet'),
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
                $('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/ObservacionesEvaluacionActualizarT',
                { idEncuestaEvaluacion: $('#app').data('eet'), observaciones: $('#txtObservaciones').dxTextArea('option', 'text') }
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
            $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/CumplimientoContenidoSeguimiento', {
                ideet: $('#app').data('eet'),
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

                var grid = $('#grdPreguntasEvaluacionT').dxDataGrid('instance');
                grid.refresh();
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

    $("#grdPreguntasEvaluacionT").dxDataGrid({
        dataSource: grdPreguntasEvaluacionTDataSource,
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
                                        $("#grdPreguntasEvaluacionT").dxDataGrid("saveEditData");
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

    $("#btnNuevoMC").dxButton({
        icon: 'add',
        onClick: function (params) {
            CargarEstrategia(null, 'MC');
        },
    });

    $("#grdPreguntasEvaluacionMC").dxDataGrid({
        dataSource: grdEstrategiasEvaluacionMCDataSource,
        allowColumnResizing: true,
        allowSorting: false,
        height: '100%',
        noDataText: 'Adicione las estrategias de movilidad sostenible a implementar en el próximo periodo por la organización',
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
        wordWrapEnabled: true,
        columns: [
            {
                dataField: "ID",
                dataType: 'number',
                visible: false,
            }, {
                dataField: "ID_ESTRATEGIA",
                dataType: 'number',
                visible: false,
            }, {
                dataField: "S_GRUPO",
                caption: 'GRUPO',
                width: '230px',
                dataType: 'string',
                visible: true,
                allowEditing: false,
            }, {
                dataField: "S_ESTRATEGIA",
                caption: 'ESTRATEGIA',
                dataType: 'string',
                visible: true,
                allowEditing: false,
            }, {
                dataField: "S_INDICADOR_MEDICION",
                caption: 'INDICADOR',
                width: '150px',
                dataType: 'string',
                visible: true,
                allowEditing: false,
            }, {
                dataField: "S_UNIDADES_META",
                dataType: 'string',
                visible: false,
            }, {
                dataField: "S_UNIDADES_META_NOMBRE",
                caption: 'UNIDADES',
                width: '90px',
                dataType: 'string',
                visible: true,
                allowEditing: false,
            }, {
                dataField: "N_VALOR_META",
                caption: 'META ACTUAL',
                width: '100px',
                dataType: 'number',
                visible: true,
                allowEditing: false,
            }, {
                dataField: "N_VALOR_META_ALCANZAR",
                caption: 'META ALCANZAR',
                width: '100px',
                dataType: 'number',
                visible: true,
                allowEditing: false,
            }, {
                dataField: '',
                caption: '',
                width: '60px',
                dataType: 'string',
                allowEditing: false,
                cellTemplate: function (cellElement, cellInfo) {
                    cellElement.css('text-align', 'center');

                    var div = document.createElement("div");
                    cellElement.get(0).appendChild(div);
                    $(div).dxButton({
                        icon: 'edit',
                        onClick: function (params) {
                            CargarEstrategia(cellInfo.data, 'MC');
                        },
                    });
                }
            }, {
                dataField: '',
                caption: '',
                width: '60px',
                dataType: 'string',
                allowEditing: false,
                cellTemplate: function (cellElement, cellInfo) {
                    cellElement.css('text-align', 'center');

                    var div = document.createElement("div");
                    cellElement.get(0).appendChild(div);
                    $(div).dxButton({
                        icon: 'remove',
                        onClick: function (params) {
                            var result = DevExpress.ui.dialog.confirm("Está Seguro de Eliminar la Estrategia ?", "Confirmación");
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    $.getJSON(
                                        $('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/EliminarEstrategiasEvaluacion?idEstrategiaTercero=' + cellInfo.data.ID
                                    ).done(function (data) {
                                        var grdPreguntasEvaluacionMC = $("#grdPreguntasEvaluacionMC").dxDataGrid("instance");
                                        grdPreguntasEvaluacionMC.option("dataSource", grdEstrategiasEvaluacionMCDataSource);
                                    });
                                }
                            });
                        },
                    });
                }
            }
        ]
    });
});

function CargarEstrategia(datosEstrategia, tipo) {
    var nuevo = false;
    if (datosEstrategia == null)
        nuevo = true;

    var estrategia = $('#popEstrategia').dxPopup('instance');
    estrategia.option('title', '3. METAS A CUMPLIR EN EL PRÓXIMO PERIODO DE IMPLEMENTACIÓN DE ESTRATEGIAS DE MOVILIDAD SOSTENIBLE');
    estrategia.option('showTitle', false);
    estrategia.option('width', '700px');
    estrategia.option('height', '550px');
    estrategia.show();

    $('#aceptarEstrategia').dxButton(
        {
            type: 'success',
            text: 'Aceptar',
            width: '100%',
            height: '30px',
            onClick: function (params) {
                var formInstance = $('#frmDatosEstrategia').dxForm('instance');

                var result = formInstance.validate();
                if (result.isValid) {
                    datosEstrategiaAlmacenar = {
                        ID: datosEstrategia.ID,
                        ID_EVALUACION_TERCERO: $('#app').data('eet'),
                        ID_ESTRATEGIA: formInstance.getEditor('ID_ESTRATEGIA').option('value'),
                        S_OTRO: formInstance.getEditor('S_OTRO').option('value'),
                        S_INDICADOR_MEDICION: formInstance.getEditor('S_INDICADOR_MEDICION').option('value'),
                        S_UNIDADES_META: formInstance.getEditor('S_UNIDADES_META').option('value'),
                        N_VALOR_META: formInstance.getEditor('N_VALOR_META').option('value'),
                        //N_PRESUPUESTO: formInstance.getEditor('N_PRESUPUESTO').option('value'),
                        S_TIPO: tipo,
                        N_VALOR_META_ALCANZAR: formInstance.getEditor('N_VALOR_META_ALCANZAR').option('value'),
                    }

                    if (nuevo) {
                        $.postJSON(
                            $('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/InsertarEstrategiaEvaluacion', datosEstrategiaAlmacenar
                        ).done(function (data) {
                            var estrategia = $('#popEstrategia').dxPopup('instance');
                            estrategia.hide();

                            var grdPreguntasEvaluacionMC = $("#grdPreguntasEvaluacionMC").dxDataGrid("instance");
                            grdPreguntasEvaluacionMC.option("dataSource", grdEstrategiasEvaluacionMCDataSource);
                        });
                    } else {
                        $.postJSON(
                            $('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/ActualizarEstrategiaEvaluacion', datosEstrategiaAlmacenar
                        ).done(function (data) {
                            var estrategia = $('#popEstrategia').dxPopup('instance');
                            estrategia.hide();

                            var grdPreguntasEvaluacionMC = $("#grdPreguntasEvaluacionMC").dxDataGrid("instance");
                            grdPreguntasEvaluacionMC.option("dataSource", grdEstrategiasEvaluacionMCDataSource);
                        });
                    }
                }
            }
        }
    );

    $('#cancelarEstrategia').dxButton(
        {
            type: 'danger',
            text: 'Cancelar',
            width: '100%',
            height: '30px',
            onClick: function (params) {
                var estrategia = $('#popEstrategia').dxPopup('instance');
                estrategia.hide();
            }
        }
    );

    if (datosEstrategia == null) {
        datosEstrategia = {
            ID: null,
            ID_GRUPO: null,
            ID_ESTRATEGIA: null,
            S_OTRO: null,
            S_INDICADOR_MEDICION: null,
            S_UNIDADES_META: null,
            N_VALOR_META: null,
            //N_PRESUPUESTO: null,
            N_VALOR_META_ALCANZAR: null,
        };
    }

    var estrategiasIniciales = [];

    if (datosEstrategia.ID_GRUPO != null) {
        estrategiasIniciales = DevExpress.data.query(estrategias).filter(["idGrupo", "=", datosEstrategia.ID_GRUPO]).toArray();
    }

    $("#frmDatosEstrategia").dxForm({
        labelLocation: 'top',
        scrollingEnabled: true,
        colCount: 1,
        formData: datosEstrategia,
        validationGroup: "estrategiaData",
        items: [
            {
                label: { text: '1. GRUPO DE ESTRATEGIAS PARA LA MOVILIDAD SOSTENIBLE' },
                dataField: 'ID_GRUPO',
                editorType: 'dxSelectBox',
                editorOptions: {
                    items: grupos,
                    placeholder: '[Seleccionar Grupo]',
                    displayExpr: 'grupo',
                    valueExpr: 'id',
                    disabled: false,
                    onValueChanged: function (e) {
                        var form = $('#frmDatosEstrategia').dxForm('instance');
                        var estrategiaEditor = form.getEditor("ID_ESTRATEGIA");
                        var filteredEstrategias = [];

                        if (e.value != null) {
                            filteredEstrategias = DevExpress.data.query(estrategias).filter(["idGrupo", "=", e.value]).toArray();
                        }

                        estrategiaEditor.option("dataSource", filteredEstrategias);
                    }
                },
                validationRules: [{
                    type: "required",
                    message: "Grupo Requerido"
                }]
            }, {
                label: { text: '1.1. ESTRATEGIA DE MOVILIDAD SOSTENIBLE' },
                dataField: 'ID_ESTRATEGIA',
                editorType: 'dxSelectBox',
                editorOptions: {
                    items: estrategiasIniciales,
                    placeholder: '[Seleccionar Estrategia]',
                    displayExpr: 'estrategia',
                    valueExpr: 'id',
                    disabled: false
                },
                validationRules: [{
                    type: "required",
                    message: "Estrategia Requerida"
                }]
            }, {
                label: { text: 'OTRO (ESPECIFIQUE LA ESTRATEGIA)' },
                dataField: 'S_OTRO',
                editorOptions: {
                    disabled: false
                }
            }, {
                label: { text: "1.2. INDICADOR DE MEDICIÓN" },
                dataField: 'S_INDICADOR_MEDICION',
                editorOptions: {
                    disabled: false
                },
                /*validationRules: [{
                    type: "required",
                    message: "Indicador de Medición Requerido"
                }]*/
            }, {
                label: { text: '1.2.1.UNIDADES DE LA META' },
                dataField: 'S_UNIDADES_META',
                editorType: 'dxSelectBox',
                editorOptions: {
                    items: unidadesMeta,
                    placeholder: '[Seleccionar Estrategia]',
                    displayExpr: 'texto',
                    valueExpr: 'id',
                    disabled: false
                },
                /*validationRules: [{
                    type: "required",
                    message: "Unidad Requerida"
                }]*/
            }, {
                label: { text: '1.2.2.VALOR META ACTUAL' },
                dataField: 'N_VALOR_META',
                editorType: 'dxNumberBox',
                editorOptions: {
                    format: "#,##0",
                    disabled: false
                },
                /*validationRules: [{
                    type: "required",
                    message: "Valor Meta Requerido"
                }]*/
            }, {
                label: { text: '1.2.3.VALOR META A ALCANZAR' },
                dataField: 'N_VALOR_META_ALCANZAR',
                editorType: 'dxNumberBox',
                editorOptions: {
                    format: "#,##0",
                    disabled: false
                },
                /*validationRules: [{
                    type: "required",
                    message: "Valor Meta Requerido"
                }]*/
            }/*, {
                label: { text: '1.3. PRESUPUESTO' },
                dataField: 'N_PRESUPUESTO',
                editorType: 'dxNumberBox',
                dataType: 'number',
                editorOptions: {
                    format: "#,##0",
                    disabled: false
                },
                validationRules: [{
                    type: "required",
                    message: "Presupuesto Requerido"
                }]
            },*/
        ]
    });
}

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


var grdPreguntasEvaluacionTDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/PreguntasSeguimiento', {
            idEncuestaEvaluacionTercero: $('#app').data('eet'),
            t: 'T'
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
            $('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/PreguntasEvaluacionActualizarT', { idEncuestaEvaluacion: $('#app').data('eet'), id: key.ID, idPregunta: key.ID_PREGUNTA, tipoRespuesta: key.N_TIPO_RESPUESTA, valor: values.RESPUESTA }
        ).done(function (data) {
            $('#cumplimientoCell').css('background-color', 'gray');
            $('#cumplimiento').text('PENDIENTE');

            var grid = $('#grdPreguntasEvaluacionT').dxDataGrid('instance');
            grid.refresh();
        });
    }
});

var grdEstrategiasEvaluacionMCDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/PreguntasEstrategias', {
            idEncuestaEvaluacionTercero: $('#app').data('eet'),
            t: 'MC'
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        });
        return d.promise();
    },
    byKey: function (key) {
        return { id: key };
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
