$(document).ready(function () {
    var historicoCargado = false;
    var posTab = 0;

    $("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
    });

    $("#popAvanzaTareaTramite").dxPopup({
        title: "Avanza Tarea",
        fullScreen: true,
    });

    $("#popDetalleTramite").dxPopup({
        title: "Detalle Trámite",
        fullScreen: false,
    });
    
    var tabsData = [
            { text: 'EMPRESAS PENDIENTES POR EVALUACIÓN', pos: 0 },
            { text: 'EVALUACIONES PENDIENTES POR AVANZAR', pos: 1 },
            { text: 'HISTORICO EVALUACIONES', pos: 2 }
    ];

    $("#tabOpciones").dxTabs({
        dataSource: tabsData,
        onItemClick: (function (itemData) {
            switch (itemData.itemIndex)
            {
                case 0:
                    $('#tab01').css('display', 'block');
                    $('#tab02').css('display', 'none');
                    $('#tab03').css('display', 'none');
                    break;
                case 1:
                    $('#tab02').css('display', 'block');
                    $('#tab01').css('display', 'none');
                    $('#tab03').css('display', 'none');

                    $('#grdPMESPlanesAvanzar').dxDataGrid('instance').option('dataSource', grdPendientesAvanzarDataSource);

                    $(window).trigger('resize');

                    break;
                case 2:
                    $('#tab03').css('display', 'block');
                    $('#tab01').css('display', 'none');
                    $('#tab02').css('display', 'none');

                    if (!historicoCargado) {
                        $('#grdPMESPlanesHistorico').dxDataGrid('instance').option('dataSource', grdPlanesHistoricoDataSource);
                    }

                    $(window).trigger('resize');
                    break;
            }

            posTab = itemData.itemIndex;
        }),
        selectedIndex: 0,
    });

    $("#grdPMESEmpresas").dxDataGrid({
        dataSource: grdPMESEmpresasDataSource,
        allowColumnResizing: true,
        height: '100%',
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 20,
        },
        pager: {
            showPageSizeSelector: true,
        },
        filterRow: {
            visible: true,
        },
        groupPanel: {
            visible: false,
            allowColumnDragging: false,
        },
        editing: {
            allowUpdating: false,
            allowDeleting: false,
            allowAdding: false

        },
        selection: {
            mode: 'single'
        },
        columns: [
            {
                dataField: "ID_EET",
                dataType: 'number',
                visible: false,
            }, {
                dataField: "ID_TERCERO",
                dataType: 'number',
                visible: false,
            }, {
                dataField: 'CM',
                width: '5%',
                caption: 'CM',
                dataType: 'string',
                visible: false,
            }, {
                dataField: 'CODTRAMITE',
                width: '7%',
                caption: 'TRAMITE',
                dataType: 'number',
            }, {
                dataField: 'N_DOCUMENTO',
                width: '9%',
                caption: 'NIT',
                dataType: 'string',
            }, {
                dataField: 'S_TERCERO',
                width: '24%',
                caption: 'EMPRESA',
                dataType: 'string',
            }, {
                dataField: 'S_VALOR_VIGENCIA',
                width: '7%',
                caption: 'VIGENCIA',
                dataType: 'string',
            }, {
                dataField: 'S_ASUNTO',
                width: '40%',
                caption: 'ASUNTO',
                dataType: 'string',
            }, {
                caption: '',
                width: '13%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div />').dxButton(
                        {
                            icon: '',
                            text: 'Evaluación',
                            type: 'success',
                            onClick: function (params) {
                                if (options.data.S_VALOR_VIGENCIA == '-') {
                                    MostrarNotificacion('alert', 0, 'Vigencia Inválida');
                                } else {
                                    if (options.data.ID_EET == null) {
                                        window.open($('#app').data('url') + 'EncuestaExterna/PMESEvaluacion/EvaluacionTercero?t=' + $('#app').data('t') + '&ve=' + $('#app').data('ve') + '&ter=' + options.data.ID_TERCERO + '&v=' + options.data.S_VALOR_VIGENCIA + '&tra=' + options.data.CODTRAMITE + '&c=' + $('#app').data('c'));
                                    } else {
                                        window.open($('#app').data('url') + 'EncuestaExterna/PMESEvaluacion/EvaluacionTercero?t=' + $('#app').data('t') + '&ve=' + $('#app').data('ve') + '&id=' + options.data.ID_EET);
                                    }
                                }
                            }
                        }
                        ).appendTo(container);
                }
            }
        ]
    });

    $("#grdPMESPlanesAvanzar").dxDataGrid({
        dataSource: null,
        allowColumnResizing: true,
        height: '100%',
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 20,
        },
        pager: {
            showPageSizeSelector: true,
        },
        filterRow: {
            visible: false,
        },
        groupPanel: {
            visible: false,
            allowColumnDragging: false,
        },
        editing: {
            allowUpdating: false,
            allowDeleting: false,
            allowAdding: false

        },
        selection: {
            mode: 'single'
        },
        columns: [
            {
                dataField: "ID_EET",
                dataType: 'number',
                visible: false,
            }, {
                dataField: "ID_TERCERO",
                dataType: 'number',
                visible: false,
            }, {
                dataField: 'CM',
                width: '5%',
                caption: 'CM',
                dataType: 'string',
                visible: false,
            }, {
                dataField: 'CODTRAMITE',
                width: '7%',
                caption: 'TRAMITE',
                dataType: 'number',
            }, {
                dataField: 'N_DOCUMENTO',
                width: '9%',
                caption: 'NIT',
                dataType: 'string',
            }, {
                dataField: 'S_TERCERO',
                width: '20%',
                caption: 'EMPRESA',
                dataType: 'string',
            }, {
                dataField: 'S_VALOR_VIGENCIA',
                width: '7%',
                caption: 'VIGENCIA',
                dataType: 'string',
            }, {
                dataField: 'S_ASUNTO',
                width: '20%',
                caption: 'ASUNTO',
                dataType: 'string',
            }, {
                dataField: 'S_RESULTADO',
                width: '10%',
                caption: 'RESULTADO',
                dataType: 'string',
            }, {
                dataField: 'D_FECHA_GENERACION',
                width: '10%',
                caption: 'FECHA',
                dataType: 'string',
            }, {
                caption: '',
                width: '10%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div />').dxButton(
                        {
                            icon: '',
                            text: 'Trámite',
                            type: 'normal',
                            onClick: function (params) {
                                abrirDetalleTramite(options.data.CODTRAMITE);
                            }
                        }
                        ).appendTo(container);
                }
            }, {
                caption: '',
                width: '10%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div />').dxButton(
                        {
                            icon: '',
                            text: 'Devolver',
                            type: 'default',
                            onClick: function (params) {
                                $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/DevolverEvaluacion', {
                                    ideet: options.data.ID_EET,
                                }).done(function (data) {
                                    var grid = $('#grdPMESPlanesAvanzar').dxDataGrid('instance');
                                    grid.refresh();

                                    if (data == "OK") {
                                        MostrarNotificacion('alert', 0, 'Evaluación quedó pendiente por Evaluar.');
                                    } else {
                                        MostrarNotificacion('alert', 0, 'ERROR Devolviendo la Evaluación.');
                                    }
                                });
                            }
                        }
                        ).appendTo(container);
                }
            }, {
                caption: '',
                width: '10%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div />').dxButton(
                        {
                            icon: '',
                            text: 'Avanzar',
                            type: 'success',
                            onClick: function (params) {
                                var aprobacionInstance = $("#popAvanzaTareaTramite").dxPopup("instance");
                                aprobacionInstance.show();

                                $('#frmAvanzaTareaTramite').attr('src', $('#app').data('url') + 'Tramites/Tramites/AvanzaTareaTramite?codTramites=' + options.data.CODTRAMITE + '&tipo=0&origen=AVANZAR&c=@DateTime.Now.ToString("HHmmss")');
                            }
                        }
                        ).appendTo(container);
                }
            }
        ]
    });

    $("#grdPMESPlanesHistorico").dxDataGrid({
        dataSource: null,
        allowColumnResizing: true,
        height: '100%',
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 20,
        },
        pager: {
            showPageSizeSelector: true,
        },
        filterRow: {
            visible: true,
        },
        groupPanel: {
            visible: false,
            allowColumnDragging: false,
        },
        editing: {
            allowUpdating: false,
            allowDeleting: false,
            allowAdding: false

        },
        selection: {
            mode: 'single'
        },
        columns: [
            {
                dataField: "ID_EET",
                dataType: 'number',
                visible: false,
            }, {
                dataField: "ID_TERCERO",
                dataType: 'number',
                visible: false,
            }, {
                dataField: 'CM',
                width: '5%',
                caption: 'CM',
                dataType: 'string',
                visible: false,
            }, {
                dataField: 'CODTRAMITE',
                width: '7%',
                caption: 'TRAMITE',
                dataType: 'number',
            }, {
                dataField: 'N_DOCUMENTO',
                width: '10%',
                caption: 'NIT',
                dataType: 'string',
            }, {
                dataField: 'S_TERCERO',
                width: '18%',
                caption: 'EMPRESA',
                dataType: 'string',
            }, {
                dataField: 'S_VALOR_VIGENCIA',
                width: '7%',
                caption: 'VIGENCIA',
                dataType: 'string',
            }, {
                dataField: 'S_ASUNTO',
                width: '24%',
                caption: 'ASUNTO',
                dataType: 'string',
            }, {
                dataField: 'S_RESULTADO',
                width: '8%',
                caption: 'RESULTADO',
                dataType: 'string',
            }, {
                dataField: 'N_DIA',
                width: '5%',
                caption: 'DIA',
                dataType: 'number',
            }, {
                dataField: 'N_MES',
                width: '5%',
                caption: 'MES',
                dataType: 'number',
            }, {
                dataField: 'N_ANO',
                width: '7%',
                caption: 'AÑO',
                dataType: 'number',
            }, {
                caption: '',
                width: '10%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div />').dxButton(
                        {
                            icon: '',
                            text: 'Trámite',
                            type: 'normal',
                            onClick: function (params) {
                                abrirDetalleTramite(options.data.CODTRAMITE);
                            }
                        }
                        ).appendTo(container);
                }
            }, {
                caption: '',
                width: '12%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div />').dxButton(
                        {
                            icon: '',
                            text: 'Evaluación',
                            type: 'success',
                            onClick: function (params) {
                                window.open($('#app').data('url') + 'EncuestaExterna/PMESEvaluacion/EvaluacionTercero?id=' + options.data.ID_EET);
                            }
                        }
                        ).appendTo(container);
                }
            }
        ]
    });
});

var grdPMESEmpresasDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"S_TERCERO","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (loadOptions.skip ? loadOptions.skip : 0);
        var take = (loadOptions.take ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/EvaluacionTerceros', {
            filter: filterOptions,
            sort: sortOptions,
            group: groupOptions,
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: false,
            version: $('#app').data('ve'),
            tipo: $('#app').data('t'),
            copia: $('#app').data('c')
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        });
        return d.promise();
    }
});

var grdPendientesAvanzarDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"S_TERCERO","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (loadOptions.skip ? loadOptions.skip : 0);
        var take = (loadOptions.take ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/PendientesAvanzar', {
            filter: filterOptions,
            sort: sortOptions,
            group: groupOptions,
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: false
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        });
        return d.promise();
    }
});

var grdPlanesHistoricoDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"S_TERCERO","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (loadOptions.skip ? loadOptions.skip : 0);
        var take = (loadOptions.take ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEvaluacionApi/PlanesHistoricoEvaluacion', {
            filter: filterOptions,
            sort: sortOptions,
            group: groupOptions,
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: false
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

function abrirDetalleTramite(id) {
    /*$("#pantallaDetalleTramites").dialog(
           {
               width: 650,
               height: 550,
               modal: true
           });
    $("#DetalleTramites").attr("src", "http://webservices.metropol.gov.co/SIM/Tramites/WebForms/DetalleT.aspx?idt=" + id);*/

    var tramiteInstance = $("#popDetalleTramite").dxPopup("instance");
    tramiteInstance.option('title', 'Detalle Trámite - ' + id);
    tramiteInstance.option('fullScreen', true);

    tramiteInstance.show();

    $('#frmDetalleTramite').attr('src', null);
    //$('#frmDetalleTramite').attr('src', 'http://webservices.metropol.gov.co/SIM/Tramites/WebForms/DetalleT.aspx?idt=' + id);
    $('#frmDetalleTramite').attr('src', $('#app').data('url') + 'Utilidades/DetalleTramite?popup=true&CodTramite=' + id);
}

function CerrarPopup(popUp, estado, origen, codTramites, codTarea, codTareaSiguiente, codFuncionario, copias) {
    var aprobacionInstance = $("#popAvanzaTareaTramite").dxPopup("instance");
    aprobacionInstance.hide();

    if (estado == 'OK') {
        mensajeAlmacenamiento("Los Trámites fueron avanzados Satisfactoriamente.");
        $('#grdPMESPlanesAvanzar').dxDataGrid('instance').option('dataSource', grdPendientesAvanzarDataSource);
    }
    else if (estado == 'ERROR') {
        mensajeAlmacenamiento("Por lo menos uno de los Trámites NO fue avanzado Satisfactoriamente.");
    }
}