let posTab = 0;
let idProcesoActual = -1;
let _departamentoId = 0;

$(document).ready(function () {
    const tabsData = [
        {
            id: 0,
            text: 'PREJUDICALES',
        },
        {
            id: 1,
            text: 'DATOS GENERALES',
        },
        {
            id: 2,
            text: 'INFORMACIÓN BÁSICA DEL PROCESO',
        },
        {
            id: 3,
            text: 'ACTUACIONES',
        },
        {
            id: 4,
            text: 'PARTES',
        },
        {
            id: 5,
            text: 'PRETENSIONES',
        }
    ];

    $("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
        shadingColor: "rgba(0,0,0,0.4)",
    });

    $('#tabOpciones').dxTabs({
        width: 'auto',
        rtlEnabled: false,
        selectedIndex: 0,
        showNavButtons: false,
        dataSource: tabsData,
        orientation: 'horizontal',
        stylingMode: 'primary',
        iconPosition: 'top',
        onItemClick: (function (itemData) {
            switch (itemData.itemIndex) {
                case 0:
                    $('#tab01').css('display', 'block');
                    $('#tab02').css('display', 'none');
                    $('#tab03').css('display', 'none');
                    $('#tab04').css('display', 'none');
                    $('#tab05').css('display', 'none');
                    $('#tab06').css('display', 'none');
                    break;
                case 1:
                    $('#tab02').css('display', 'block');
                    $('#tab01').css('display', 'none');
                    $('#tab03').css('display', 'none');
                    $('#tab04').css('display', 'none');
                    $('#tab05').css('display', 'none');
                    $('#tab06').css('display', 'none');
                    break;
                case 2:
                    $('#tab03').css('display', 'block');
                    $('#tab01').css('display', 'none');
                    $('#tab02').css('display', 'none');
                    $('#tab04').css('display', 'none');
                    $('#tab05').css('display', 'none');
                    $('#tab06').css('display', 'none');
                    break;
                case 3:
                    $('#tab04').css('display', 'block');
                    $('#tab01').css('display', 'none');
                    $('#tab02').css('display', 'none');
                    $('#tab03').css('display', 'none');
                    $('#tab05').css('display', 'none');
                    $('#tab06').css('display', 'none');
                    break;
                case 4:
                    $('#tab05').css('display', 'block');
                    $('#tab01').css('display', 'none');
                    $('#tab02').css('display', 'none');
                    $('#tab03').css('display', 'none');
                    $('#tab04').css('display', 'none');
                    $('#tab06').css('display', 'none');
                    break;
                case 5:
                    $('#tab06').css('display', 'block');
                    $('#tab01').css('display', 'none');
                    $('#tab02').css('display', 'none');
                    $('#tab03').css('display', 'none');
                    $('#tab04').css('display', 'none');
                    $('#tab05').css('display', 'none');
                    break;
            }

            posTab = itemData.itemIndex;
        })
    });

    $("#grdProcesosJudiciales").dxDataGrid({
        dataSource: grdProcesosJudicialesDataSource,
        allowColumnResizing: true,
        height: '100%',
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            enabled: true,
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20, 50]
        },
        filterRow: {
            visible: true,
            applyFilter: 'auto'
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
                dataField: "procesoId",
                dataType: 'number',
                visible: false
            }, {
                dataField: 'medioControl',
                width: '20%',
                caption: 'MEDIO DE CONTROL',
                dataType: 'string',
            }, {
                dataField: 'juzgado',
                width: '20%',
                caption: 'JUZGADO Y/O TRIBUNAL',
                dataType: 'string',
            }, {
                dataField: 'juridiccion',
                width: '20%',
                caption: 'JURISDICCION',
                dataType: 'string',
            }, {
                dataField: 'radicado',
                width: '15%',
                caption: 'RADICADO',
                dataType: 'string',
            }, {
                dataField: 'demanda',
                caption: 'TIPO DEMANDA',
                dataType: 'string',
            }, {
                dataField: 'demandadoDemandante',
                caption: 'PARTE',
                dataType: 'string',
            }, {
                caption: '',
                width: '6%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'edit',
                            type: 'success',
                            hint: 'Editar Proceso Judicial',
                            onClick: function (params) {
                                $('#loadPanel').dxLoadPanel('instance').show();
                                $('#detalleProcesos').show();
                                LimpiarCamposDetalle();
                                $('#listaProcesos').hide();
                                CargarCamposDetalle(cellInfo.data.ID_PROCESO);
                                $('#loadPanel').dxLoadPanel('instance').hide();

                                $('#regresar').dxButton(
                                {
                                    icon: 'arrowleft',
                                    text: '',
                                    width: '30x',
                                    type: 'success',
                                    elementAttr: {
                                        style: "float: right;"
                                    },
                                    onClick: function (params) {
                                        $('#loadPanel').dxLoadPanel('instance').show();
                                        $('#listaProcesos').show();
                                        $('#detalleProcesos').hide();
                                        $('#loadPanel').dxLoadPanel('instance').hide();
                                    }
                                });
                            }
                        }
                    ).appendTo(cellElement);
                }
            }
        ]
    });

    $('#agregarProcesoJudicial').dxButton(
    {
        icon: 'plus',
        text: '',
        width: '30x',
        type: 'success',
        elementAttr: {
            style: "float: right;"
        },
        onClick: function (params) {
            $('#loadPanel').dxLoadPanel('instance').show();
            $('#detalleProcesos').show();
            LimpiarCamposDetalle();
            $('#listaProcesos').hide();
            CargarCamposDetalle(0);
            $('#loadPanel').dxLoadPanel('instance').hide();

            $('#regresar').dxButton(
                {
                    icon: 'arrowleft',
                    text: '',
                    width: '30x',
                    type: 'success',
                    elementAttr: {
                        style: "float: right;"
                    },
                    onClick: function (params) {
                        $('#loadPanel').dxLoadPanel('instance').show();
                        $('#listaProcesos').show();
                        $('#detalleProcesos').hide();
                        $('#loadPanel').dxLoadPanel('instance').hide();
                    }
                });
        }
    });

    $('#guardarProcesoJudicial').dxButton(
        {
            icon: 'save',
            text: '',
            width: '30x',
            type: 'success',
            elementAttr: {
                style: "float: right;"
            },
            onClick: function (params) {
                alert('Proceso almacanado satisfactoriamente!');
                $('#loadPanel').dxLoadPanel('instance').show();
                $('#listaProcesos').show();
                $('#detalleProcesos').hide();
                $('#loadPanel').dxLoadPanel('instance').hide();
            }
        });
   
});

function LimpiarCamposDetalle() {

    $('#jurisdiccion').dxSelectBox({
        dataSource: null,
        placeholder: '[Calidad de la Entidad]',
        value: null
    });

    $('#medioControl').dxSelectBox({
        dataSource: null,
        placeholder: '[Calidad de la Entidad]',
        value: null
    });

    $('#juzgado').dxSelectBox({
        dataSource: null,
        placeholder: '[Calidad de la Entidad]',
        value: null
    });
   
    $('#calidadEntidad').dxSelectBox({
        dataSource: null,
        placeholder: '[Calidad de la Entidad]',
        value: null
    });

    $('#riesgoProcesal').dxSelectBox({
        dataSource: null,
        placeholder: '[Riesgo Procesal]',
        value: null
    });

    $('#radicado').dxTextBox({
        placeholder: '#####################',
        format: '#####################',
        value: '000000000000000000000' 
    });
    $('#demandante').dxTextBox({
        placeholder: '[Demandante]',
        value: null
    });
    $('#demandado').dxTextBox({
        placeholder: '[Demandado]',
        value: null
    });

    $('#fechaRadicado').dxDateBox({
        placeholder: '[Fecha Radicado]',
        value: null
    });

    $('#fechaAdmision').dxDateBox({
        placeholder: '[Fecha Admisión]',
        value: null
    });

    $('#fechaNotificacion').dxDateBox({
        placeholder: '[Fecha Notificación]',
        value: null
    });

    $('#apoderado').dxTextBox({
        placeholder: '[Apoderado CC y TP]',
        readOnly: true,
        value: null
    });

    $('#codigoContable').dxTextBox({
        placeholder: '[Código Contable]',
        readOnly: true,
        value: null
    });

    $('#etapaActuacion').dxTextBox({
        placeholder: '[Etapa / Actuación]',
        readOnly: true,
        value: null
    });
    
  
    $('#generaErogacion').dxSelectBox({
        dataSource: null,
        placeholder: '[Genera Erogación Económica]',
        value: null
    });

    $('#valorEconomico').dxTextBox({
        placeholder: '[Valor Económico del Proceso]',
        value: null
    });

    $('#tipoPretension').dxSelectBox({
        dataSource: null,
        placeholder: '[Tipo de Pretensión]',
        value: null
    });

    $('#unidadMonetaria').dxSelectBox({
        dataSource: null,
        placeholder: '[Unidad Monetaria]',
        value: null
    });

    $('#unidadMonetariaJE').dxSelectBox({
        dataSource: null,
        placeholder: '[Unidad Monetaria]',
        value: null
    });

    $('#valorJE').dxTextBox({
        placeholder: '[Valor]',
        value: null
    });

    $('#fechaHechos').dxDateBox({
        placeholder: '[Fecha Hechos]',
        value: null
    });

    $('#departamentoHechos').dxSelectBox({
        dataSource: null,
        placeholder: '[Departamento]',
        value: null
    });

    $('#municipioHechos').dxSelectBox({
        dataSource: null,
        placeholder: '[Municipio]',
        value: null
    });

    $('#descripcionHechos').dxTextArea({
        placeholder: '[Descripción de los Hechos]',
        height: 380,
        value: null
    });
    
    $('#caducidadHechos').dxTextBox({
        placeholder: '[Caducidad o Prescripción]',
        value: null
    });

    $('#llamamientoHechos').dxTextBox({
        placeholder: '[Llamamiento en Garantía]',
        value: null
    });

    $('#pretensionesDeclarativasHechos').dxTextArea({
        placeholder: '[Pretensiones Declarativas]',
        height: 380,
        value: null
    });

    $('#cbovalorEconomico').dxSelectBox({
        dataSource: null,
        placeholder: '[Valor económico del proceso]',
        value: null
    });
    

}

function CargarCamposDetalle(id) {
    $('#calidadEntidad').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetCalidadEntidad');
                }
            })
        }),
        placeholder: '[Calidad de la Entidad]',
        value: null,
        disabled: false,
        displayExpr: "valor",
        valueExpr: "id",
    });
    $('#jurisdiccion').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetJurisdicciones');
                }
            })
        }),
        placeholder: '[Jurisdicción]',
        value: null,
        disabled: false,
        displayExpr: "valor",
        valueExpr: "id",
        searchEnabled: true
    });
    $('#medioControl').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetMediosControl');
                }
            })
        }),
        placeholder: '[Medio de Control]',
        value: null,
        disabled: false,
        displayExpr: "valor",
        valueExpr: "id",
        searchEnabled: true
    });
    $('#juzgado').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetJuzgados');
                }
            })
        }),
        placeholder: '[Juzgado]',
        value: null,
        disabled: false,
        displayExpr: "valor",
        valueExpr: "id",
        searchEnabled: true,
        onOpened: function (e) {
            e.component._popup.option('width', 500);
        }
    });
    $('#unidadMonetaria').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetTiposCuantia');
                }
            })
        }),
        placeholder: '[Unidad Monetaria]',
        value: null,
        disabled: false,
        displayExpr: "valor",
        valueExpr: "id",
        searchEnabled: true
    });
    $('#tipoPretension').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetTiposPretencion');
                }
            })
        }),
        placeholder: '[Unidad Monetaria]',
        value: null,
        disabled: false,
        displayExpr: "valor",
        valueExpr: "id",
        searchEnabled: true
    });
    $('#riesgoProcesal').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetRiesgosProcesales');
                }
            })
        }),
        placeholder: '[Riesgo Procesal]',
        value: null,
        disabled: false,
        displayExpr: "valor",
        valueExpr: "id",
        searchEnabled: true
    });
    var cboDepartamento = $('#departamentoHechos').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetDepartamentos');
                }
            })
        }),
        onValueChanged: function (data) {
            if (data.value != null) {
                var cboCiudadDs = cboMunicipios.getDataSource();
                _departamentoId = data.value;
                cboCiudadDs.reload();
                cboMunicipios.option("value", null);
            }
        },
        placeholder: '[Departamento]',
        value: null,
        disabled: false,
        displayExpr: "valor",
        valueExpr: "id",
        searchEnabled: true
   }).dxSelectBox("instance");
    var cboMunicipios = $('#municipioHechos').dxSelectBox({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.CustomStore({
                    key: "id",
                    loadMode: "raw",
                    load: function (loadOptions) {
                        return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetMunicipios', { departamentoId: _departamentoId });
                    }
                })
            }),
            placeholder: '[Municipio]',
            value: null,
            disabled: false,
            displayExpr: "valor",
            valueExpr: "id",
            searchEnabled: true
    }).dxSelectBox("instance");
    var valorEconomico = $('#valorEconomico').dxNumberBox({
        placeholder: '[Valor Económico del Proceso]',
        format: "$ #,##0.##",
        value: 0
    }).dxNumberBox("instance");
    var cbovalorEconomico = $('#cbovalorEconomico').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetTipoValorEconomico');
                }
            }),
            onValueChanged: function (data) {
                if (data.value != null) {
                    if (data.value == '1') {
                        valorEconomico.option('disabled', false);
                    }
                    else {
                         valorEconomico.option('disabled', true);
                    }
                }
            },
        }),
        placeholder: '[Jurisdicción]',
        value: null,
        disabled: false,
        displayExpr: "valor",
        valueExpr: "id",
    }).dxSelectBox("instance");
    $('#generaErogacion').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetErogacion');
                }
            })
        }),
        onValueChanged: function (data) {
            if (data.value != null) {
                if (data.value == '1') {
                    cbovalorEconomico.option('disabled', false);
                    valorEconomico.option('disabled', false);
                }
                else {
                    cbovalorEconomico.option('disabled', true);
                    valorEconomico.option('disabled', true);
                }
            }
        },
        placeholder: '[Jurisdicción]',
        value: null,
        disabled: false,
        displayExpr: "valor",
        valueExpr: "id",
    });
}

var grdHechosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"ID_PROCESO","desc":true}]';

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'ProcesosJudiciales/api/ProcesosJudicialesApi/ConsultaHechos?id=' + idProcesoActual, {
            estadoProceso: '-1',
            filter: filterOptions,
            sort: sortOptions,
            group: '',
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            noFilterNoRecords: false
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('falla: ' + textStatus + ", " + error);
        });
        return d.promise();
    },
    byKey: function (key, extra) {
        return key.toString();
    },
});

var grdProcesosJudicialesDataSource = new DevExpress.data.CustomStore({
    key: "procesoId",
    load: function (loadOptions) {
        var d = $.Deferred();
        var params = {};
        [
            "filter",
            "group",
            "groupSummary",
            "parentIds",
            "requireGroupCount",
            "requireTotalCount",
            "searchExpr",
            "searchOperation",
            "searchValue",
            "select",
            "sort",
            "skip",
            "take",
            "totalSummary",
            "userData"
        ].forEach(function (i) {
            if (i in loadOptions && isNotEmpty(loadOptions[i])) {
                params[i] = JSON.stringify(loadOptions[i]);
            }
        });
        $.getJSON($('#app').data('url') + 'ProcesosJudiciales/api/ProcesosJudicialesApi/ConsultaProcesosJudiciales', params)
            .done(function (response) {
                d.resolve(response.data, {
                    totalCount: response.totalCount
                });
            }).fail(function (jqxhr, textStatus, error) {
                alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
            });
        return d.promise();
    }
});

function isNotEmpty(value) {
    return value !== undefined && value !== null && value !== "";
}


