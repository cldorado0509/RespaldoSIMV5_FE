let posTab = 0;
let idProcesoActual = -1;
let _departamentoId = 0;
let idTerSel = 0;
let nombreTerSel = "";
let nitTerSel = "";

var grdConvocantesDataSource = new DevExpress.data.ArrayStore({ store: [] });
var grdConvocadosDataSource = new DevExpress.data.ArrayStore({ store: [] });
var grdDemandadosDataSource = new DevExpress.data.ArrayStore({ store: [] });

$(document).ready(function () {

    $('#asistente').accordion({
        collapsible: true,
        animationDuration: 500,
        multiple: false
    });


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
            text: 'PRETENSIONES',
        }
    ];

   
    $("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
        shadingColor: "rgba(0,0,0,0.4)",
    });


    var calidadEntidad = $('#calidadEntidad').dxSelectBox({
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
    }).dxSelectBox("instance");
     

    var apoderado = $('#apoderado').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetApoderados');
                }
            })
        }),
        placeholder: '[Apoderado]',
        value: null,
        disabled: false,
        displayExpr: "valor",
        valueExpr: "id",
    }).dxSelectBox("instance");

 
    var jurisdiccion = $('#jurisdiccion').dxSelectBox({
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
    }).dxSelectBox("instance");

    var medioControl = $('#medioControl').dxSelectBox({
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
    }).dxSelectBox("instance");

    var juzgado = $('#juzgado').dxSelectBox({
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
    }).dxSelectBox("instance");

    var radicado21 = $("#radicado21").dxTextBox({
        value: '000000000000000000000',
    }).dxTextBox("instance");

    var fechaRadicado = $('#fechaRadicado').dxDateBox({
        placeholder: '[Fecha Radicado]',
        with: '180pt',
        value: null
    }).dxDateBox("instance");

    $('#fechaAdmision').dxDateBox({
        placeholder: '[Fecha Admisión]',
        value: null
    });

    var fechaNotificacion = $('#fechaNotificacion').dxDateBox({
        placeholder: '[F.Notificación]',
        value: null
    }).dxDateBox("instance");


    var fechaAudiencia = $('#fechaAudiencia').dxDateBox({
        placeholder: '[F.Audiencia]',
        value: null
    }).dxDateBox("instance");

    var procuraduria = $('#procuraduria').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetProcuradurias');
                }
            })
        }),
        placeholder: '[Procuraduría]',
        value: null,
        disabled: false,
        displayExpr: "valor",
        valueExpr: "id",
        searchEnabled: true
    }).dxSelectBox("instance");

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

    var hechos = $("#hechos").dxTextArea({
        value: "",
        readOnly: false,
        height: 160,
        onValueChanged(e) {
            var value = e.component.option("value");
            e.component.option("value", value.toUpperCase());
        } 
    }).dxTextArea("instance");

    var recomencionAbogado = $("#recomencionAbogado").dxTextArea({
        value: "",
        readOnly: false,
        height: 280,
        onValueChanged(e) {
            var value = e.component.option("value");
            e.component.option("value", value.toUpperCase());
        } 
    }).dxTextArea("instance");

    var pretensiones = $("#pretensiones").dxTextArea({
        value: "",
        readOnly: false,
        height: 160,
        onValueChanged(e) {
        var value = e.component.option("value");
        e.component.option("value", value.toUpperCase());
    } 
    }).dxTextArea("instance");

    var fundamentoJuridicoConvocante = $("#fundamentoJuridicoConvocante").dxTextArea({
        value: "",
        readOnly: false,
        height: 160,
        onValueChanged(e) {
            var value = e.component.option("value");
            e.component.option("value", value.toUpperCase());
        } 
    }).dxTextArea("instance");

    var fundamentoDefensa = $("#fundamentoDefensa").dxTextArea({
        value: "",
        readOnly: false,
        height: 160,
        onValueChanged(e) {
            var value = e.component.option("value");
            e.component.option("value", value.toUpperCase());
        } 
    }).dxTextArea("instance");

    var fechaComiteConciliacion = $('#fechaComiteConciliacion').dxDateBox({
        placeholder: '[F.Comité]',
        value: null
    }).dxDateBox("instance");

    var fechaAudiencia = $('#fechaAudiencia').dxDateBox({
        placeholder: '[F.Audiencia]',
        value: null
    }).dxDateBox("instance");

    var decisionComite = $("#decisionComite").dxRadioGroup({
        dataSource: [{ text: "Aprueba", valor: 1 }, { text: "Desaprueba", valor: 2 }],
        displayExpr: "text",
        valueExpr: "valor",
        value: 1
    }).dxRadioGroup("instance");

    var decisionAudiencia = $("#decisionAudiencia").dxRadioGroup({
        dataSource: [{ text: "Aprueba", valor: 1 }, { text: "Desaprueba", valor: 2 }],
        displayExpr: "text",
        valueExpr: "valor",
        value: 1
    }).dxRadioGroup("instance");

    var huboAcuerdoConciliatorio = $("#huboAcuerdoConciliatorio").dxRadioGroup({
        dataSource: [{ text: "Si", valor: 1 }, { text: "No", valor: 2 }],
        displayExpr: "text",
        valueExpr: "valor",
        value: 1
    }).dxRadioGroup("instance");

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

    $('#abrirFichaPejudicial').dxButton(
        {
            icon: 'startswith',
            text: '',
            width: '30x',
            hint: 'Ficha Comité de Conciliación',
            type: 'danger',
            elementAttr: {
                style: "float:left;"
            },
            onClick: function (params) {
                var datos = '';
                const _asunto = '';
                const _despacho = procuraduria.option("text");
                const _medioControl = '';
                const _radicado = radicado21.option("value");
                const _instancia = '';
                const _convocante = 'Pedro Páramo A';
                const _convocado = 'Ana María Castaño';
                const _fechaNotificacion = fechaNotificacion.option("value");
                const _fechaAudiencia = fechaAudiencia.option("value");
                const _riesgoProcesal = '';
                const _cuantia = '0';
                const _politicaInstitucional = '';
                const _llamaGarantia = '';
                const _apoderado = apoderado.option("text");

                var datos = _asunto + '|' + _despacho + ' |' + _medioControl + '|' + _radicado + '|' + _instancia + '|' + _convocante + '|' + _convocado + '|' + _fechaNotificacion + '|' + _fechaAudiencia + '|' + _riesgoProcesal + '|' + _cuantia + '|' + _politicaInstitucional + '|' + _llamaGarantia + '|' + _apoderado;
                window.open("https://sim.metropol.gov.co/editor/procesosjudiciales/fichacomite?procesoJudicialId=1&token=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiam9yZ2UuZXN0cmFkYSIsIm5iZiI6MTcxNzA3NTcyOSwiZXhwIjoxNzE3MDc4MTI5LCJpYXQiOjE3MTcwNzU3Mjl9.6z4DhaW5whNk5f-Pk15UzTs-8dAAeGKbtC-N23m_JKI&datos=" + datos);
                //popupFichaPre.show();
            }
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
                visible: true
            }, {
                dataField: 'radicado',
                width: '15%',
                caption: 'RADICADO',
                dataType: 'string',
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
                            
                                idProcesoActual = cellInfo.data.procesoId;

                                $('#listaProcesos').hide();
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
        ],
       onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                idProcesoActual = data.procesoId;
            }
        }
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
                $('#listaProcesos').hide();
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

    $('#agregarConvocante').dxButton(
        {
            icon: 'plus',
            text: '',
            width: '30x',
            hint: 'Adicionar convocante',
            type: 'success',
            elementAttr: {
                style: "float: right;"
            },
            onClick: function (params) {
                let btntipo = document.getElementById('popupConvocante');
                btntipo.setAttribute('data-tipo', '1');
                popupConvocante.show();
            }
        });
           
    $('#agregarConvocado').dxButton(
        {
            icon: 'plus',
            text: '',
            width: '30x',
            hint: 'Adicionar convocado',
            type: 'success',
            elementAttr: {
                style: "float: right;"
            },
            onClick: function (params) {
                let btntipo = document.getElementById('popupConvocante');
                btntipo.setAttribute('data-tipo', '2');
                popupConvocante.show();
            }
        });
    
    $('#abrirCertificado').dxButton(
        {
            icon: 'startswith',
            text: '',
            width: '30x',
            hint: 'Certificado del Comité',
            type: 'danger',
            elementAttr: {
                style: "float:left;"
            },
            onClick: function (params) {
                var datos = '';
                window.open("https://sim.metropol.gov.co/editor/procesosjudiciales/Certificado?procesoJudicialId=1&token=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiam9yZ2UuZXN0cmFkYSIsIm5iZiI6MTcxNzA3NTcyOSwiZXhwIjoxNzE3MDc4MTI5LCJpYXQiOjE3MTcwNzU3Mjl9.6z4DhaW5whNk5f-Pk15UzTs-8dAAeGKbtC-N23m_JKI");
                //popupFichaPre.show();
            }
        });
         
    $("#grdDemandantes").dxDataGrid({
        dataSource: grdConvocantesDataSource,
        allowColumnResizing: true,
        height: '100%',
        with: '150px',
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
                dataField: "demantanteId",
                dataType: 'number',
                visible: false
            }, {
                dataField: 'identificacion',
                width: '10%',
                caption: 'Identificación',
                dataType: 'string',
            }, {
                dataField: 'nombre',
                width: '70%',
                caption: 'Nombre/Razón social',
                dataType: 'string',
            },
            {
                caption: '',
                width: '6%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'delete',
                            type: 'danger',
                            hint: 'Eliminar demandante',
                            onClick: function (params) {
                             
                            }
                        }
                    ).appendTo(cellElement);
                }
            }
        ]
    });

    $("#grdDemandados").dxDataGrid({
        dataSource: grdDemandadosDataSource,
        allowColumnResizing: true,
        height: '100%',
        with: '150px',
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
                dataField: "demandadoId",
                dataType: 'number',
                visible: false
            }, {
                dataField: 'identificacion',
                width: '10%',
                caption: 'Identificación',
                dataType: 'string',
            }, {
                dataField: 'nombre',
                width: '70%',
                caption: 'Nombre/Razón social',
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

                            }
                        }
                    ).appendTo(cellElement);
                }
            },
            {
                caption: '',
                width: '6%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'delete',
                            type: 'danger',
                            hint: 'Eliminar demandado',
                            onClick: function (params) {

                            }
                        }
                    ).appendTo(cellElement);
                }
            }
        ]
    }).dxDataGrid("instance");
    
    $("#grdTerceros").dxDataGrid({
        dataSource: grdTercerosDataSource,
        allowColumnResizing: true,
        height: '100%',
        with: '150px',
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            enabled: true,
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10]
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
                dataField: "terceroId",
                dataType: 'number',
                visible: false
            }, {
                dataField: 'identificacion',
                width: '20%',
                caption: 'Identificación',
                dataType: 'string',
            }, {
                dataField: 'nombre',
                width: '40%',
                caption: 'Nombre/Razón social',
                dataType: 'string',
            }
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                idTerSel = data.terceroId;
                nombreTerSel = data.nombre;
                nitTerSel = data.identificacion;
            }
        }
    });
 
    $("#grdConvocantes").dxDataGrid({
        dataSource: grdConvocantesDataSource,
        allowColumnResizing: true,
        height: '100%',
        with: '150px',
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            enabled: true,
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10]
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
                dataField: "demantanteId",
                dataType: 'number',
                visible: false
            }, {
                dataField: 'identificacion',
                width: '20%',
                caption: 'Identificación',
                dataType: 'string',
            }, {
                dataField: 'nombre',
                width: '75%',
                caption: 'Nombre/Razón social',
                dataType: 'string',
            }, 
            {
                dataField: 'isNew',
                dataType: 'number',
                visible : false
            },
            {
                caption: '',
                width: '70px',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'clear',
                            type: 'danger',
                            hint: 'Eliminar demandado',
                            onClick: function (params) {
                                grdConvocantesDataSource.remove(cellInfo.data);
                                $("#grdConvocantes").dxDataGrid({ dataSource: grdConvocantesDataSource });
                            }
                        }
                    ).appendTo(cellElement);
                }
            }
        ]
    });
       
    $("#grdConvocados").dxDataGrid({
        dataSource: grdConvocadosDataSource,
        allowColumnResizing: true,
        height: '100%',
        with: '150px',
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
                dataField: "demandadoId",
                dataType: 'number',
                visible: false
            }, {
                dataField: 'identificacion',
                width: '20%',
                caption: 'Identificación',
                dataType: 'string',
            }, {
                dataField: 'nombre',
                width: '75%',
                caption: 'Nombre/Razón social',
                dataType: 'string',
            },
            {
                dataField: 'isNew',
                dataType: 'number',
                visible: false
            },
             {
                caption: '',
                width: '70px',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'clear',
                            type: 'danger',
                            hint: 'Eliminar demandado',
                            onClick: function (params) {
                                grdConvocadosDataSource.remove(cellInfo.data);
                                $("#grdConvocados").dxDataGrid({ dataSource: grdConvocadosDataSource });
                            }
                        }
                    ).appendTo(cellElement);
                }
            }
        ]
    });

    $('#btnGuardarConvocante').dxButton(
        {
            icon: 'save',
            text: '',
            hint: 'Asignar Convocante',
            width: '340px',
            type: 'danger',
            onClick: function (params) {

                const tipo = document.getElementById('popupConvocante').getAttribute('data-tipo');

                if (tipo === "1") {
                    const data = {
                        demantanteId: idTerSel,
                        identificacion: nitTerSel.toUpperCase(),
                        nombre: nombreTerSel.toUpperCase(),
                        isNew: 1
                    };
                    grdConvocantesDataSource.insert(data);

                    $("#grdConvocantes").dxDataGrid({ dataSource: grdConvocantesDataSource });
                }
                else {
                    const data = {
                        demandadoId: idTerSel,
                        identificacion: nitTerSel.toUpperCase(),
                        nombre: nombreTerSel.toUpperCase(),
                        isNew: 1
                    };
                    grdConvocadosDataSource.insert(data);

                    $("#grdConvocados").dxDataGrid({ dataSource: grdConvocadosDataSource });
                }


                popupConvocante.hide();
            }
        });

    $('#btnGuardarConvocado').dxButton(
        {
            icon: 'save',
            text: '',
            hint: 'Asignar Convocante',
            width: '340px',
            type: 'danger',
            onClick: function (params) {
                const data = {
                    demandadoId: idTerSel,
                    identificacion: nitTerSel.toUpperCase(),
                    nombre: nombreTerSel.toUpperCase(),
                    isNew: 1
                };
                grdConvocadosDataSource.insert(data);

                $("#grdConvocados").dxDataGrid({ dataSource: grdConvocadosDataSource });

                popupConvocado.hide();
            }
        });

    $("#grdActuacion").dxDataGrid({
        dataSource: grdActuacionesDataSource,
        allowColumnResizing: true,
        height: '100%',
        with: '150px',
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
                dataField: "actuacionId",
                dataType: 'number',
                visible: false
            }, {
                dataField: 'etapa',
                width: '10%',
                caption: 'Etapa',
                dataType: 'string',
            }, {
                dataField: 'conciliacion',
                width: '10%',
                caption: 'Conciliación',
                dataType: 'string',
            }
            , {
                dataField: 'valorConciliado',
                width: '10%',
                caption: 'Valor conciliado',
                dataType: 'string',
            }, {
                dataField: 'finalizado',
                width: '10%',
                caption: 'Finalizado',
                dataType: 'boolean',
            }, {
                dataField: 'fechaFinalizacion',
                width: '10%',
                caption: 'Fecha Finalización',
                dataType: 'datetime',
            }, {
                dataField: 'comiteVerificacion',
                width: '10%',
                caption: 'Comité Verificación',
                dataType: 'boolean',
            }, {
                dataField: 'desacato',
                width: '10%',
                caption: 'Desactato',
                dataType: 'boolean',
            }
            , {
                caption: '',
                width: '10%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'edit',
                            type: 'success',
                            hint: 'Editar Actuación',
                            onClick: function (params) {

                            }
                        }
                    ).appendTo(cellElement);
                }
            }
        ]
    });

    $('#verSolicitud').dxButton(
        {
            icon: 'exportpdf',
            text: '',
            width: '40px',
            hint: 'Ver la solicitud',
            type: 'normal',
            onClick: function (params) {
                window.open('../content/imagenes/certificado.pdf');
            }
        });

    $('#verNotificacion').dxButton(
        {
            icon: 'exportpdf',
            text: '',
            hint: 'Ver citación audiencia',
            width: '40px',
            type: 'normal',
            onClick: function (params) {
                window.open('../content/imagenes/certificado.pdf');
            }
        });

    $('#btnGuardarFichaPre').dxButton(
        {
            icon: 'save',
            text: 'Guadar',
            hint: 'Guadar Ficha',
            width: '240px',
            type: 'success',
            onClick: function (params) {
                popupFichaPre.hide();
            }
        });

    $('#btnImprimirFichaPre').dxButton(
        {
            icon: 'print',
            text: 'Imprimir/Exportar',
            hint: 'Imprimir / Exportar Ficha',
            width: '240px',
            type: 'danger',
            onClick: function (params) {
                popupFichaPre.hide();
            }
        });

       
    var popupFichaPre = $("#PopupFichaPrejudicial").dxPopup({
        width: 900,
        height: "600",
        resizeEnabled: true,
        hoverStateEnabled: true,
        title: "Ficha"
    }).dxPopup("instance");

   
    var popupConvocante = $("#popupConvocante").dxPopup({
        width: 700,
        height: "auto",
        hoverStateEnabled: true,
        title: "Convocante"
    }).dxPopup("instance");

   

 
    $('#verComiteConciliacion').dxButton(
        {
            icon: 'exportpdf',
            text: '',
            hint: 'Ver acta del comité de conciliación ...',
            width: '40px',
            type: 'normal',
            onClick: function (params) {
                window.open('../content/imagenes/certificado.pdf');
            }
        });

    $('#verSolicitudConsiliacion').dxButton(
        {
            icon: 'exportpdf',
            text: '',
            hint: 'Ver la solicitud ...',
            width: '40px',
            type: 'normal',
            onClick: function (params) {
                window.open('../content/imagenes/certificado.pdf');
            }
        });

    $('#verActaAudiencia').dxButton(
        {
            icon: 'exportpdf',
            text: '',
            hint: 'Ver el acta de la audiciencia',
            width: '40px',
            type: 'normal',
            onClick: function (params) {
                window.open('../content/imagenes/certificado.pdf');
            }
        });

    $('#verAdmision').dxButton(
        {
            icon: 'exportpdf',
            text: '',
            hint: 'Ver admisión',
            width: '40px',
            type: 'normal',
            onClick: function (params) {
                window.open('../content/imagenes/certificado.pdf');
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

                var arraydata = grdConvocantesDataSource._array;
                var demandantesArray = [];

                for (i = 0; i < arraydata.length; i++) {
                    demandantesArray.push({ demantanteId: arraydata[i].demantanteId, identificacion: arraydata[i].identificacion, nombre: arraydata[i].nombre, isNew: arraydata[i].isNew });
                } 

                var arraydatad = grdConvocadosDataSource._array;
                var demandadosArray = [];

                for (i = 0; i < arraydatad.length; i++) {
                    demandadosArray.push({ demandadoId: arraydatad[i].demandadoId, identificacion: arraydatad[i].identificacion, nombre: arraydatad[i].nombre, isNew: arraydatad[i].isNew });
                } 

                var id = idProcesoActual;
                var _procesoJuzgadoId = juzgado.option("value");
                var _procuraduriasId = procuraduria.option("value");
                var _contrato = "";
                var _terceroId = 0;
                var _radicado = radicado21.option("value");
                var _radicado21 =  radicado21.option("value");
                var _fechaRadicado = fechaRadicado.option("value");
                var _cuantia = "";
                var _hechos = hechos.option("value");
                var _fechaNotificacion = fechaNotificacion.option("value");
                var _procesoCodigoId = 0;
                var _instanciaId = 0;
                var _eliminado = "0";
                var _recomendacionesAbogado = recomencionAbogado.option("value");
                var _tipoDemanda = 0;
                var _sincronizado = "0";
                var _terminado = "0";
                var _mensajeSincronizacion = "";
                var _fundamentoJuridicoConvocante = fundamentoJuridicoConvocante.option("value");
                var _fundamentoDefensa = fundamentoDefensa.option("value");
                var _fechaComiteConciliacion = fechaComiteConciliacion.option("value");
                var _decisionComite = decisionComite.option("value");
                var _hayAcuerdo = huboAcuerdoConciliatorio.option("value");
                var _decisionAudiencia = decisionAudiencia.option("value");
       
                var params = {
                    id: id, procesoJuzgadoId: _procesoJuzgadoId, procuraduriasId: _procuraduriasId, contrato: _contrato, terceroId: _terceroId, radicado: _radicado, radicado21: _radicado21,
                    fechaRadicado: _fechaRadicado, cuantia: _cuantia, hechos: _hechos, fechaNotificacion: _fechaNotificacion, procesoCodigoId: _procesoCodigoId, instanciaId: _instanciaId,
                    eliminado: _eliminado, recomendacionesAbogado: _recomendacionesAbogado, tipoDemanda: _tipoDemanda, sincronizado: _sincronizado, terminado: _terminado,
                    mensajeSincronizacion: _mensajeSincronizacion, fundamentoJuridicoConvocante: _fundamentoJuridicoConvocante, fundamentoDefensa: _fundamentoDefensa,
                    fechaComiteConciliacion: _fechaComiteConciliacion, decisionComite: _decisionComite, hayAcuerdo: _hayAcuerdo, decisionAudiencia: _decisionAudiencia,
                    demandantes: demandantesArray, demandados: demandadosArray
                };

                var _Ruta = $('#app').data('url') + "ProcesosJudiciales/api/ProcesosJudicialesApi/GuardarProcesoJudicialAsync";
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    url: _Ruta,
                    data: JSON.stringify(params),
                    contentType: "application/json",
                    crossDomain: true,
                    headers: { 'Access-Control-Allow-Origin': '*' },
                    success: function (data) {
                        if (data.IsSuccess === false) DevExpress.ui.dialog.alert('Ocurrió un error ' + data.Message, 'Guardar Datos');
                        else {
                            DevExpress.ui.dialog.alert('Historian Creada/Actualizada correctamente', 'Guardar Datos');
                            $('#gridExamenes').dxDataGrid({ dataSource: gridExamenesDataSource });
                            $("#btnNuevoExamen").dxButton("instance").option("visible", true);
                        }
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                    }
                });

                $('#loadPanel').dxLoadPanel('instance').show();
                $('#listaProcesos').show();
                $('#detalleProcesos').hide();
                $('#loadPanel').dxLoadPanel('instance').hide();
            }
        });

});


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


var grdActuacionesDataSource = new DevExpress.data.CustomStore({
    key: "actuacionId",
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
        $.getJSON($('#app').data('url') + 'ProcesosJudiciales/api/ProcesosJudicialesApi/ConsultaActuaciones', params)
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

var grdTercerosDataSource = new DevExpress.data.CustomStore({
    key: "terceroId",
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
        $.getJSON($('#app').data('url') + 'ProcesosJudiciales/api/ProcesosJudicialesApi/ConsultaTerceros', params)
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


