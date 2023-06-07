$(document).ready(function () {
    $("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
    });
    //$('#app').data('codtramites')

    $('#btnGenerarDocumento').dxButton(
        {
            icon: '',
            text: 'Generar Documento Evaluación',
            width: '300px',
            type: 'success',
            onClick: function (params) {
                $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/ValidarGeneracionDocumentoEvaluacion', {
                    eet: $('#app').data('id'),
                }).done(function (data) {
                    switch (data) {
                        case 0:
                            window.open($('#app').data('url') + 'EncuestaExterna/PMESEvaluacion/GenerarDocumentoEvaluacion?eet=' + $('#app').data('id') + '&v=' + $('#app').data('ve') + '&t=' + $('#app').data('t'), '_self');
                            break;
                        case 1:
                            MostrarNotificacion('alert', '', 'El Documento fue previamente Generado.');
                            break;
                        case 2:
                            if ($('#app').data('t') == '2')
                                MostrarNotificacion('alert', '', 'Datos Inválidos. No se puede generar el Documento de Evaluación de Seguimiento. Verificar que la evaluación esté generada y que esté seleccionada la Sede Principal.');
                            else
                                MostrarNotificacion('alert', '', 'Datos Inválidos. No se puede generar el Documento de Evaluación. Verificar que todas las evaluaciones estén generadas y que esté seleccionada la Sede Principal.');
                            break;
                    }
                });
            }
        }
    );

    $('#btnEvaluacion').dxButton(
        {
            icon: '',
            text: 'Evaluación',
            width: '150px',
            type: 'success',
            onClick: function (params) {
                window.open($('#app').data('url') + 'EncuestaExterna/PMESEvaluacion/EvaluacionEncuesta?ideet=' + $('#app').data('id') + '&v=' + $('#app').data('ve') + '&t=' + $('#app').data('t'));
            }
        }
    );

    $('#btnRefrescarDatos').dxButton(
        {
            icon: '',
            text: 'Refrescar Información',
            width: '200px',
            type: 'success',
            onClick: function (params) {
                var grid = $('#grdPMESTercero').dxDataGrid('instance');
                grid.refresh();
            }
        }
    );

    if ($('#app').data('ve') == '1') {
        $("#grdPMESTercero").dxDataGrid({
            dataSource: grdPMESTerceroDataSource,
            allowColumnResizing: true,
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
                allowColumnDragging: false,
            },
            editing: {
                editEnabled: false,
                removeEnabled: false,
                insertEnabled: false

            },
            selection: {
                mode: 'none'
            },
            columns: [
                {
                    dataField: "ID_EVALUACION_ENCUESTA",
                    dataType: 'number',
                    visible: false,
                }, {
                    dataField: "ID_EVALUACION_TERCERO",
                    dataType: 'number',
                    visible: false,
                }, {
                    dataField: "ID_TERCERO",
                    dataType: 'number',
                    visible: false,
                }, {
                    dataField: "ID_INSTALACION",
                    dataType: 'number',
                    visible: false,
                }, {
                    dataField: 'INSTALACION',
                    caption: 'SEDE',
                    dataType: 'string',
                    cellTemplate: function (cellElement, cellInfo) {
                        if (cellInfo.data.ID_EVALUACION_ENCUESTA == -1) {
                            cellElement.css('font-size', '20px');
                            cellElement.css('font-weight', 'bold');
                            cellElement.css('text-align', 'right');
                        }

                        cellElement.html(cellInfo.data.INSTALACION);
                    }
                }, {
                    dataField: 'ENC_SITIO',
                    width: '11%',
                    caption: 'SITIO',
                    dataType: 'string',
                    cellTemplate: function (cellElement, cellInfo) {
                        if (cellInfo.data.ID_EVALUACION_ENCUESTA == -1) {
                            cellElement.html('');
                        } else {
                            if (cellInfo.data.S_EXCLUIR == 'S') {
                                cellElement.html('N/A');
                                cellElement.css('background-color', 'white');
                                cellElement.css('color', 'black');
                            } else {
                                cellElement.html(cellInfo.data.ENC_SITIO);
                                switch (cellInfo.data.ENC_SITIO) {
                                    case 'OK':
                                        cellElement.css('background-color', 'limegreen');
                                        cellElement.css('color', 'white');
                                        break;
                                    case 'NO EXISTE':
                                        cellElement.css('background-color', 'red');
                                        cellElement.css('color', 'white');
                                        break;
                                    case 'SIN ENVIAR':
                                        cellElement.css('background-color', 'dodgerblue');
                                        cellElement.css('color', 'white');
                                        break;
                                }
                            }
                            cellElement.css('text-align', 'center');
                        }
                    }
                }, {
                    dataField: 'ENC_TRABAJADORES',
                    width: '11%',
                    caption: 'TRABAJADORES',
                    dataType: 'string',
                    cellTemplate: function (cellElement, cellInfo) {
                        if (cellInfo.data.ID_EVALUACION_ENCUESTA == -1) {
                            cellElement.html('');
                        } else {
                            if (cellInfo.data.S_EXCLUIR == 'S') {
                                cellElement.html('N/A');
                                cellElement.css('background-color', 'white');
                                cellElement.css('color', 'black');
                            } else {
                                cellElement.html(cellInfo.data.ENC_TRABAJADORES);
                                switch (cellInfo.data.ENC_TRABAJADORES) {
                                    case 'OK':
                                        cellElement.css('background-color', 'limegreen');
                                        cellElement.css('color', 'white');
                                        break;
                                    case 'NO EXISTE':
                                        cellElement.css('background-color', 'red');
                                        cellElement.css('color', 'white');
                                        break;
                                    case 'SIN ENVIAR':
                                        cellElement.css('background-color', 'dodgerblue');
                                        cellElement.css('color', 'white');
                                        break;
                                }
                            }
                            cellElement.css('text-align', 'center');
                        }
                    }
                }, {
                    dataField: 'RESULTADO',
                    width: '15%',
                    caption: 'RESULTADO',
                    dataType: 'string',
                    //visible: ($('#app').data('t') == '1'),
                    cellTemplate: function (cellElement, cellInfo) {
                        if (cellInfo.data.ID_EVALUACION_ENCUESTA == -1) {
                            cellElement.html(cellInfo.data.RESULTADO);
                            switch (cellInfo.data.RESULTADO) {
                                case 'PENDIENTE':
                                    cellElement.css('background-color', 'gray');
                                    cellElement.css('color', 'white');
                                    break;
                                case 'CUMPLE':
                                    cellElement.css('background-color', 'limegreen');
                                    cellElement.css('color', 'white');
                                    break;
                                case 'NO CUMPLE':
                                    cellElement.css('background-color', 'red');
                                    cellElement.css('color', 'white');
                                    break;
                            }

                            cellElement.css('font-size', '20px');
                            cellElement.css('font-weight', 'bold');
                            cellElement.css('color', 'white');
                        } else {
                            if ($('#app').data('t') == '1') {
                                if (cellInfo.data.S_EXCLUIR == 'S') {
                                    cellElement.html('N/A');
                                    cellElement.css('background-color', 'white');
                                    cellElement.css('color', 'black');
                                } else {
                                    cellElement.html(cellInfo.data.RESULTADO);
                                    switch (cellInfo.data.RESULTADO) {
                                        case 'PENDIENTE':
                                            cellElement.css('background-color', 'gray');
                                            cellElement.css('color', 'white');
                                            break;
                                        case 'CUMPLE':
                                            cellElement.css('background-color', 'limegreen');
                                            cellElement.css('color', 'white');
                                            break;
                                        case 'NO CUMPLE':
                                            cellElement.css('background-color', 'red');
                                            cellElement.css('color', 'white');
                                            break;
                                    }
                                }
                            } else {
                                cellElement.html('');
                            }
                        }
                        cellElement.css('text-align', 'center');
                    }
                },
                {
                    dataField: 'S_PRINCIPAL',
                    caption: 'P',
                    width: '3%',
                    alignment: 'center',
                    dataTye: 'boolean',
                    visible: ($('#app').data('ro') == 'N'),
                    cellTemplate: function (cellElement, cellInfo) {
                        if (cellInfo.data.ID_EVALUACION_ENCUESTA == -1) {
                            cellElement.html('');
                        } else {
                            if (cellInfo.data.S_EXCLUIR != 'S') {
                                $('<div />').dxCheckBox(
                                    {
                                        value: cellInfo.data.S_PRINCIPAL == 'S',
                                        onValueChanged: function (params) {
                                            $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/InstalacionPrincipal', {
                                                idee: cellInfo.data.ID_EVALUACION_ENCUESTA,
                                                ideet: cellInfo.data.ID_EVALUACION_TERCERO,
                                                i: cellInfo.data.ID_INSTALACION,
                                                valor: params.value
                                            }).done(function (data) {
                                                var grid = $('#grdPMESTercero').dxDataGrid('instance');
                                                grid.refresh();
                                            });
                                        }
                                    }
                                ).appendTo(cellElement);
                            }
                        }
                    }
                },
                {
                    caption: '',
                    width: '12%',
                    alignment: 'center',
                    visible: ($('#app').data('t') == '1'),
                    cellTemplate: function (cellElement, cellInfo) {
                        if (cellInfo.data.ID_EVALUACION_ENCUESTA == -1) {
                            cellElement.html('');
                        } else {
                            if (cellInfo.data.S_EXCLUIR == 'N' && cellInfo.data.ENC_SITIO == 'OK' && cellInfo.data.ENC_TRABAJADORES == 'OK') {
                                $('<div />').dxButton(
                                    {
                                        icon: '',
                                        text: 'Evaluación',
                                        type: 'success',
                                        onClick: function (params) {
                                            if (cellInfo.data.ID_EVALUACION_ENCUESTA != null)
                                                window.open($('#app').data('url') + 'EncuestaExterna/PMESEvaluacion/EvaluacionEncuesta?idee=' + cellInfo.data.ID_EVALUACION_ENCUESTA + '&v=' + $('#app').data('ve') + '&t=' + $('#app').data('t'));
                                            else if (cellInfo.data.ID_EVALUACION_TERCERO != null && cellInfo.data.ID_INSTALACION != null)
                                                window.open($('#app').data('url') + 'EncuestaExterna/PMESEvaluacion/EvaluacionEncuesta?ideet=' + cellInfo.data.ID_EVALUACION_TERCERO + '&i=' + cellInfo.data.ID_INSTALACION + '&v=' + $('#app').data('ve') + '&t=' + $('#app').data('t'));
                                            else
                                                MostrarNotificacion('alert', '', 'Datos Inválidos. No se puede cargar la Evaluación.');
                                        }
                                    }
                                ).appendTo(cellElement);
                            }
                        }
                    }
                },
                {
                    dataField: 'S_EXCLUIR',
                    caption: 'EXCLUIR',
                    width: '7%',
                    alignment: 'center',
                    dataTye: 'boolean',
                    visible: ($('#app').data('ro') == 'N'),
                    cellTemplate: function (cellElement, cellInfo) {
                        if (cellInfo.data.ID_EVALUACION_ENCUESTA == -1) {
                            cellElement.html('');
                        } else {
                            $('<div />').dxCheckBox(
                                {
                                    value: cellInfo.data.S_EXCLUIR == 'S',
                                    onValueChanged: function (params) {
                                        $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/ExcluirInstalacion', {
                                            idee: cellInfo.data.ID_EVALUACION_ENCUESTA,
                                            ideet: cellInfo.data.ID_EVALUACION_TERCERO,
                                            i: cellInfo.data.ID_INSTALACION,
                                            valor: params.value
                                        }).done(function (data) {
                                            var grid = $('#grdPMESTercero').dxDataGrid('instance');
                                            grid.refresh();
                                        });
                                    }
                                }
                            ).appendTo(cellElement);
                        }
                    }
                }
            ]
        });
    } else {
        $("#grdPMESTercero").dxDataGrid({
            dataSource: grdPMESTerceroDataSource,
            allowColumnResizing: true,
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
                allowColumnDragging: false,
            },
            editing: {
                editEnabled: false,
                removeEnabled: false,
                insertEnabled: false

            },
            selection: {
                mode: 'none'
            },
            columns: [
                {
                    dataField: "ID_EVALUACION_ENCUESTA",
                    dataType: 'number',
                    visible: false,
                }, {
                    dataField: "ID_EVALUACION_TERCERO",
                    dataType: 'number',
                    visible: false,
                }, {
                    dataField: "ID_TERCERO",
                    dataType: 'number',
                    visible: false,
                }, {
                    dataField: "ID_INSTALACION",
                    dataType: 'number',
                    visible: false,
                }, {
                    dataField: 'INSTALACION',
                    caption: 'SEDE',
                    dataType: 'string',
                    cellTemplate: function (cellElement, cellInfo) {
                        if (cellInfo.data.ID_EVALUACION_ENCUESTA == -1) {
                            cellElement.css('font-size', '20px');
                            cellElement.css('font-weight', 'bold');
                            cellElement.css('text-align', 'right');
                        }

                        cellElement.html(cellInfo.data.INSTALACION);
                    }
                }, {
                    dataField: 'ENC_SITIO',
                    width: '11%',
                    caption: 'SITIO',
                    dataType: 'string',
                    cellTemplate: function (cellElement, cellInfo) {
                        if (cellInfo.data.ID_EVALUACION_ENCUESTA == -1) {
                            cellElement.html('');
                        } else {
                            if (cellInfo.data.S_EXCLUIR == 'S') {
                                cellElement.html('N/A');
                                cellElement.css('background-color', 'white');
                                cellElement.css('color', 'black');
                            } else {
                                cellElement.html(cellInfo.data.ENC_SITIO);
                                switch (cellInfo.data.ENC_SITIO) {
                                    case 'OK':
                                        cellElement.css('background-color', 'limegreen');
                                        cellElement.css('color', 'white');
                                        break;
                                    case 'NO EXISTE':
                                        cellElement.css('background-color', 'red');
                                        cellElement.css('color', 'white');
                                        break;
                                    case 'SIN ENVIAR':
                                        cellElement.css('background-color', 'dodgerblue');
                                        cellElement.css('color', 'white');
                                        break;
                                }
                            }
                            cellElement.css('text-align', 'center');
                        }
                    }
                }, {
                    dataField: 'ENC_TRABAJADORES',
                    width: '11%',
                    caption: 'TRABAJADORES',
                    dataType: 'string',
                    cellTemplate: function (cellElement, cellInfo) {
                        if (cellInfo.data.ID_EVALUACION_ENCUESTA == -1) {
                            cellElement.html('');
                        } else {
                            if (cellInfo.data.S_EXCLUIR == 'S') {
                                cellElement.html('N/A');
                                cellElement.css('background-color', 'white');
                                cellElement.css('color', 'black');
                            } else {
                                cellElement.html(cellInfo.data.ENC_TRABAJADORES);
                                switch (cellInfo.data.ENC_TRABAJADORES) {
                                    case 'OK':
                                        cellElement.css('background-color', 'limegreen');
                                        cellElement.css('color', 'white');
                                        break;
                                    case 'NO EXISTE':
                                        cellElement.css('background-color', 'red');
                                        cellElement.css('color', 'white');
                                        break;
                                    case 'SIN ENVIAR':
                                        cellElement.css('background-color', 'dodgerblue');
                                        cellElement.css('color', 'white');
                                        break;
                                }
                            }
                            cellElement.css('text-align', 'center');
                        }
                    }
                }, {
                    dataField: 'RESULTADO',
                    width: '15%',
                    caption: 'RESULTADO',
                    dataType: 'string',
                    cellTemplate: function (cellElement, cellInfo) {
                        if (cellInfo.data.ID_EVALUACION_ENCUESTA == -1) {
                            cellElement.html(cellInfo.data.RESULTADO);
                            switch (cellInfo.data.RESULTADO) {
                                case 'PENDIENTE':
                                    cellElement.css('background-color', 'gray');
                                    cellElement.css('color', 'white');
                                    break;
                                case 'CUMPLE':
                                    cellElement.css('background-color', 'limegreen');
                                    cellElement.css('color', 'white');
                                    break;
                                case 'NO CUMPLE':
                                    cellElement.css('background-color', 'red');
                                    cellElement.css('color', 'white');
                                    break;
                            }

                            cellElement.css('font-size', '20px');
                            cellElement.css('font-weight', 'bold');
                            cellElement.css('color', 'white');
                        } else {
                            if (cellInfo.data.S_EXCLUIR == 'S') {
                                cellElement.html('N/A');
                                cellElement.css('background-color', 'white');
                                cellElement.css('color', 'black');
                            } else {
                                cellElement.html(cellInfo.data.RESULTADO);
                                switch (cellInfo.data.RESULTADO) {
                                    case 'PENDIENTE':
                                        cellElement.css('background-color', 'gray');
                                        cellElement.css('color', 'white');
                                        break;
                                    case 'CUMPLE':
                                        cellElement.css('background-color', 'limegreen');
                                        cellElement.css('color', 'white');
                                        break;
                                    case 'NO CUMPLE':
                                        cellElement.css('background-color', 'red');
                                        cellElement.css('color', 'white');
                                        break;
                                }
                            }
                        }
                        cellElement.css('text-align', 'center');
                    }
                },
                {
                    dataField: 'S_PRINCIPAL',
                    caption: 'P',
                    width: '3%',
                    alignment: 'center',
                    dataTye: 'boolean',
                    visible: ($('#app').data('ro') == 'N'),
                    cellTemplate: function (cellElement, cellInfo) {
                        if (cellInfo.data.ID_EVALUACION_ENCUESTA == -1) {
                            cellElement.html('');
                        } else {
                            if (cellInfo.data.S_EXCLUIR != 'S') {
                                $('<div />').dxCheckBox(
                                    {
                                        value: cellInfo.data.S_PRINCIPAL == 'S',
                                        onValueChanged: function (params) {
                                            $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/InstalacionPrincipal', {
                                                idee: cellInfo.data.ID_EVALUACION_ENCUESTA,
                                                ideet: cellInfo.data.ID_EVALUACION_TERCERO,
                                                i: cellInfo.data.ID_INSTALACION,
                                                valor: params.value
                                            }).done(function (data) {
                                                var grid = $('#grdPMESTercero').dxDataGrid('instance');
                                                grid.refresh();
                                            });
                                        }
                                    }
                                ).appendTo(cellElement);
                            }
                        }
                    }
                },
                {
                    dataField: 'S_EXCLUIR',
                    caption: 'EXCLUIR',
                    width: '7%',
                    alignment: 'center',
                    dataTye: 'boolean',
                    visible: ($('#app').data('ro') == 'N'),
                    cellTemplate: function (cellElement, cellInfo) {
                        if (cellInfo.data.ID_EVALUACION_ENCUESTA == -1) {
                            cellElement.html('');
                        } else {
                            $('<div />').dxCheckBox(
                                {
                                    value: cellInfo.data.S_EXCLUIR == 'S',
                                    onValueChanged: function (params) {
                                        $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/ExcluirInstalacion', {
                                            idee: cellInfo.data.ID_EVALUACION_ENCUESTA,
                                            ideet: cellInfo.data.ID_EVALUACION_TERCERO,
                                            i: cellInfo.data.ID_INSTALACION,
                                            valor: params.value
                                        }).done(function (data) {
                                            var grid = $('#grdPMESTercero').dxDataGrid('instance');
                                            grid.refresh();
                                        });
                                    }
                                }
                            ).appendTo(cellElement);
                        }
                    }
                }
            ]
        });
    }
});

var grdPMESTerceroDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"INSTALACION","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (loadOptions.skip ? loadOptions.skip : 0);
        var take = (loadOptions.take ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/EncuestasInstalacionesTercero', {
            idTercero: $('#app').data('idt'),
            valorVigencia: $('#app').data('vv'),
            v: $('#app').data('ve'),
            t: $('#app').data('t')
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        });
        return d.promise();
    }
});


function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        DevExpress.ui.dialog.alert(msg, 'Evaluación PMES');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}
