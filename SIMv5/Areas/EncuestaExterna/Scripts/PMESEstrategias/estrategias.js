'use strict'

let estrategias;
let tiposEvidencia;
let periodicidades;
let datosEstrategiaAlmacenar;
let mergelements = {};

$(document).ready(function () {
    $("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
    });

    $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEstrategiasApi/Estrategias').done(function (data) {
        estrategias = data.estrategias;
        tiposEvidencia = data.tiposEvidencia;
        periodicidades = data.periodicidades;
    });

    $('[name^="btnNuevo_1"]').dxButton({
        icon: 'add',
        onClick: function (params) {
            CargarEstrategia(null, 'P', params.element.attr('idGrupo'), params.element.attr('titulo'));
        },
    });

    $('[name^="btnNuevo_2"]').dxButton({
        icon: 'add',
        onClick: function (params) {
            CargarEstrategia(null, 'F', params.element.attr('idGrupo'), params.element.attr('titulo'));
        },
    });

    $("#popEstrategia").dxPopup({
        fullScreen: false,
    });

    $("#popActividad").dxPopup({
        fullScreen: false,
    });

    $('[name^="grdEstrategiasGrupo_1"]').each(function () {
        let titulo = $(this).attr('titulo');

        let grdEstrategiasGrupoDataSource = EstrategiasGrupoDataSource($(this).attr('idGrupo'));

        $(this).dxDataGrid({
            dataSource: grdEstrategiasGrupoDataSource,
            allowColumnResizing: true,
            allowSorting: false,
            height: '100%',
            noDataText: 'Adicione las estrategias de movilidad sostenible formuladas por la organización',
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
                    dataField: "S_ESTRATEGIA",
                    caption: 'ESTRATEGIA',
                    width: '15%',
                    dataType: 'string',
                    visible: true,
                    allowEditing: false,
                }, {
                    dataField: "S_ACTIVIDAD",
                    caption: 'ACTIVIDAD',
                    width: '20%',
                    dataType: 'string',
                    visible: true,
                    allowEditing: false,
                }, {
                    dataField: "S_PERIODICIDAD",
                    caption: 'PERIODICIDAD',
                    width: '10%',
                    dataType: 'string',
                    visible: true,
                    allowEditing: false,
                }, {
                    dataField: "S_PUBLICO_OBJETIVO",
                    caption: 'PUBLICO OBJETIVO',
                    width: '20%',
                    dataType: 'string',
                    visible: true,
                    allowEditing: false,
                }, {
                    dataField: "S_EVIDENCIAS",
                    caption: 'EVIDENCIAS',
                    dataType: 'string',
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
                                CargarEstrategia(cellInfo.data.ID, 'P', cellInfo.data.ID_GRUPO, cellInfo.data.S_TITULO);
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
                                            $('#app').data('url') + 'EncuestaExterna/api/PMESEstrategiasApi/EliminarEstrategia?t=P&id=' + cellInfo.data.ID
                                        ).done(function (data) {
                                            let grdEstrategiasGrupo = $("#grdEstrategiasGrupo_" + cellInfo.data.ID_GRUPO).dxDataGrid("instance");
                                            let grdEstrategiasGrupoDataSource = EstrategiasGrupoDataSource(cellInfo.data.ID_GRUPO);

                                            grdEstrategiasGrupo.option("dataSource", grdEstrategiasGrupoDataSource);
                                        });
                                    }
                                });
                            },
                        });
                    }
                }
            ],
            onCellPrepared: function (cellInfo) {
                if (cellInfo.rowType == "data") {
                    cellInfo.cellElement.addClass("mergecells");
                    //color cell on selection  
                    cellInfo.cellElement.click(function () {
                        $(".mergecellselected").removeClass("mergecellselected")
                        $(this).addClass("mergecellselected");
                    });

                    if (cellInfo.rowIndex > 0 && cellInfo.column.command != "edit" && cellInfo.column.dataField != 'S_ACTIVIDAD' && cellInfo.column.dataField != 'S_PERIODICIDAD') {
                        if (cellInfo.column.dataField == '') {
                            if (cellInfo.component.cellValue(cellInfo.rowIndex - 1, 'S_ESTRATEGIA') == cellInfo.component.cellValue(cellInfo.rowIndex, 'S_ESTRATEGIA')) {
                                cellInfo.cellElement.css("display", "none");
                            }
                        } else {
                            if (cellInfo.component.cellValue(cellInfo.rowIndex - 1, cellInfo.column.dataField) == cellInfo.value) {
                                if (cellInfo.component.cellValue(cellInfo.rowIndex - 1, cellInfo.column.dataField)) {
                                    var prev = mergelements[cellInfo.rowIndex - 1][cellInfo.column.dataField]
                                    if (!mergelements[cellInfo.rowIndex]) mergelements[cellInfo.rowIndex] = {};
                                    mergelements[cellInfo.rowIndex][cellInfo.column.dataField] = prev;
                                    if (prev) {
                                        cellInfo.cellElement.css("display", "none");
                                        var span = prev.attr('rowspan');
                                        if (span)
                                            prev.attr('rowspan', Number(span) + 1);
                                        else
                                            prev.attr('rowspan', 2);
                                    }
                                }
                                else {
                                    if (!mergelements[cellInfo.rowIndex]) mergelements[cellInfo.rowIndex] = {};
                                    mergelements[cellInfo.rowIndex][cellInfo.column.dataField] = cellInfo.cellElement;
                                }

                            }
                            else {
                                if (!mergelements[cellInfo.rowIndex]) mergelements[cellInfo.rowIndex] = {};
                                mergelements[cellInfo.rowIndex][cellInfo.column.dataField] = cellInfo.cellElement;

                            }
                        }
                    }
                    else {
                        if (!mergelements[cellInfo.rowIndex]) mergelements[cellInfo.rowIndex] = {};
                        mergelements[cellInfo.rowIndex][cellInfo.column.dataField] = cellInfo.cellElement;
                    }
                }
            }
        });
    });

    $('[name^="grdEstrategiasGrupo_2"]').each(function () {
        let titulo = $(this).attr('titulo');

        $(this).dxDataGrid({
            dataSource: null,
            allowColumnResizing: true,
            allowSorting: false,
            height: '100%',
            noDataText: 'Adicione las estrategias de movilidad sostenible formuladas por la organización',
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
                    dataField: "S_ESTRATEGIA",
                    caption: 'ESTRATEGIA',
                    width: '15%',
                    dataType: 'string',
                    visible: true,
                    allowEditing: false,
                }, {
                    dataField: "S_OBJETIVO",
                    caption: 'OBJETIVO',
                    width: '20%',
                    dataType: 'string',
                    visible: true,
                    allowEditing: false,
                }, {
                    dataField: "S_PUBLICO_OBJETIVO",
                    caption: 'PUBLICO OBJETIVO',
                    width: '10%',
                    dataType: 'string',
                    visible: true,
                    allowEditing: false,
                }, {
                    dataField: "S_COMUNICACION_INTERNA",
                    caption: 'COMUNICACION INTERNA',
                    width: '20%',
                    dataType: 'string',
                    visible: true,
                    allowEditing: false,
                }, {
                    dataField: "S_COMUNICACION_EXTERNA",
                    caption: 'COMUNICACION EXTERNA',
                    dataType: 'string',
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
                                CargarEstrategia(cellInfo.data.ID, 'F', cellInfo.data.ID_GRUPO, cellInfo.data.S_TITULO);
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
                                            $('#app').data('url') + 'EncuestaExterna/api/PMESEstrategiasApi/EliminarEstrategia?t=F&id=' + cellInfo.data.ID
                                        ).done(function (data) {
                                            let grdEstrategiasGrupo = $("#grdEstrategiasGrupo_" + cellInfo.data.ID_GRUPO).dxDataGrid("instance");
                                            let grdEstrategiasGrupoDataSource = EstrategiasGrupoDataSource(cellInfo.data.ID_GRUPO);

                                            grdEstrategiasGrupo.option("dataSource", grdEstrategiasGrupoDataSource);
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

    $('[name^="grdMetasGrupo"]').each(function () {
        let dataSourceMetasGrupo = MetasGrupoDataSource($(this).attr('idGrupo'));

        $(this).dxDataGrid({
            dataSource: dataSourceMetasGrupo,
            allowColumnResizing: true,
            allowSorting: false,
            height: '100%',
            noDataText: 'Establezca el valor de la meta a cumplir semestralmente',
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
                    dataField: "S_META",
                    caption: 'META',
                    width: '40%',
                    dataType: 'string',
                    visible: true,
                    allowEditing: false,
                }, {
                    dataField: "S_MEDICION",
                    caption: 'MEDICION',
                    width: '40%',
                    dataType: 'string',
                    visible: true,
                    allowEditing: false,
                }, {
                    dataField: "N_VALOR",
                    caption: 'VALOR',
                    width: '150px',
                    dataType: 'number',
                    visible: true,
                }
            ]
        });
    });
});

function CargarEstrategia(id, tipo, idGrupo, grupo) {
    let datosEstrategia = null;
    let nuevo = false;
    if (id == null)
        nuevo = true;

    var estrategia = $('#popEstrategia').dxPopup('instance');
    estrategia.option('title', grupo);
    estrategia.option('showTitle', true);
    estrategia.option('width', '700px');
    estrategia.option('height', '550px');
    estrategia.show();

    $('#aceptarEstrategia').dxButton({
        type: 'success',
        text: 'Aceptar',
        width: '100%',
        height: '30px',
        onClick: function (params) {
            var formInstance = $('#frmDatosEstrategia').dxForm('instance');

            var result = formInstance.validate();
            if (result.isValid) {
                if (tipo == 'P') {
                    let tiposEvidenciaSeleccionadas = '';
                    tiposEvidencia.forEach((tipoEvidencia) => {
                        if ($('#tiposEvidencia' + tipoEvidencia.id).dxCheckBox('instance').option('value')) {
                            if (tiposEvidenciaSeleccionadas == '')
                                tiposEvidenciaSeleccionadas = '' + tipoEvidencia.id;
                            else
                                tiposEvidenciaSeleccionadas += ',' + tipoEvidencia.id;
                        }
                    });

                    datosEstrategiaAlmacenar = {
                        TIPO: tipo,
                        ID: datosEstrategia.ID,
                        ID_ESTRATEGIA_TERCERO: $('#app').data('et'),
                        ID_ESTRATEGIA: formInstance.getEditor('ID_ESTRATEGIA').option('value'),
                        S_OTRO: formInstance.getEditor('S_OTRO').option('value'),
                        S_PUBLICO_OBJETIVO: formInstance.getEditor('S_PUBLICO_OBJETIVO').option('value'),
                        S_TIPOEVIDENCIA: tiposEvidenciaSeleccionadas,
                        ID_ESTRATEGIA_ACTIVIDAD1: formInstance.option('formData').ID_ESTRATEGIA_ACTIVIDAD1,
                        S_ACTIVIDAD1: formInstance.option('formData').S_ACTIVIDAD1,
                        ID_PERIODICIDAD1: formInstance.option('formData').ID_PERIODICIDAD1,
                        ID_ESTRATEGIA_ACTIVIDAD2: formInstance.option('formData').ID_ESTRATEGIA_ACTIVIDAD2,
                        S_ACTIVIDAD2: formInstance.option('formData').S_ACTIVIDAD2,
                        ID_PERIODICIDAD2: formInstance.option('formData').ID_PERIODICIDAD2,
                        ID_ESTRATEGIA_ACTIVIDAD3: formInstance.option('formData').ID_ESTRATEGIA_ACTIVIDAD3,
                        S_ACTIVIDAD3: formInstance.option('formData').S_ACTIVIDAD3,
                        ID_PERIODICIDAD3: formInstance.option('formData').ID_PERIODICIDAD3,
                        ID_ESTRATEGIA_ACTIVIDAD4: formInstance.option('formData').ID_ESTRATEGIA_ACTIVIDAD4,
                        S_ACTIVIDAD4: formInstance.option('formData').S_ACTIVIDAD4,
                        ID_PERIODICIDAD4: formInstance.option('formData').ID_PERIODICIDAD4,
                        ID_ESTRATEGIA_ACTIVIDAD5: formInstance.option('formData').ID_ESTRATEGIA_ACTIVIDAD5,
                        S_ACTIVIDAD5: formInstance.option('formData').S_ACTIVIDAD5,
                        ID_PERIODICIDAD5: formInstance.option('formData').ID_PERIODICIDAD5,
                    }
                } else {
                    datosEstrategiaAlmacenar = {
                        TIPO: tipo,
                        ID: datosEstrategia.ID,
                        ID_ESTRATEGIA_TERCERO: $('#app').data('et'),
                        ID_ESTRATEGIA: formInstance.getEditor('ID_ESTRATEGIA').option('value'),
                        S_OTRO: formInstance.getEditor('S_OTRO').option('value'),
                        S_OBJETIVO: formInstance.getEditor('S_OBJETIVO').option('value'),
                        S_PUBLICO_OBJETIVO: formInstance.getEditor('S_PUBLICO_OBJETIVO').option('value'),
                        S_COMUNICACION_INTERNA: formInstance.getEditor('S_COMUNICACION_INTERNA').option('value'),
                        S_COMUNICACION_EXTERNA: formInstance.getEditor('S_COMUNICACION_EXTERNA').option('value'),
                    }
                }

                if (nuevo) {
                    $.postJSON(
                        $('#app').data('url') + 'EncuestaExterna/api/PMESEstrategiasApi/InsertarEstrategia', datosEstrategiaAlmacenar
                    ).done(function (data) {
                        let estrategia = $('#popEstrategia').dxPopup('instance');
                        estrategia.hide();

                        let grdEstrategiasGrupo = $("#grdEstrategiasGrupo_" + idGrupo).dxDataGrid("instance");
                        let grdEstrategiasGrupoDataSource = EstrategiasGrupoDataSource(idGrupo);
                        grdEstrategiasGrupo.option("dataSource", grdEstrategiasGrupoDataSource);
                    });
                } else {
                    $.postJSON(
                        $('#app').data('url') + 'EncuestaExterna/api/PMESEstrategiasApi/ActualizarEstrategia', datosEstrategiaAlmacenar
                    ).done(function (data) {
                        var estrategia = $('#popEstrategia').dxPopup('instance');
                        estrategia.hide();

                        let grdEstrategiasGrupo = $("#grdEstrategiasGrupo_" + idGrupo).dxDataGrid("instance");
                        let grdEstrategiasGrupoDataSource = EstrategiasGrupoDataSource(idGrupo);
                        grdEstrategiasGrupo.option("dataSource", grdEstrategiasGrupoDataSource);
                    });
                }
            }
        }
    });

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

    switch (tipo) {
        case 'P':
            {
                if (id == null) {
                    datosEstrategia = {
                        ID: null,
                        ID_GRUPO: null,
                        ID_ESTRATEGIA: null,
                        S_OTRO: null,
                        S_PUBLICO_OBJETIVO: null,
                        S_TIPOEVIDENCIA: null,
                        ID_ESTRATEGIA_ACTIVIDAD1: null,
                        S_ACTIVIDAD1: null,
                        ID_PERIODICIDAD1: null,
                        ID_ESTRATEGIA_ACTIVIDAD2: null,
                        S_ACTIVIDAD2: null,
                        ID_PERIODICIDAD2: null,
                        ID_ESTRATEGIA_ACTIVIDAD3: null,
                        S_ACTIVIDAD3: null,
                        ID_PERIODICIDAD3: null,
                        ID_ESTRATEGIA_ACTIVIDAD4: null,
                        S_ACTIVIDAD4: null,
                        ID_PERIODICIDAD4: null,
                        ID_ESTRATEGIA_ACTIVIDAD5: null,
                        S_ACTIVIDAD5: null,
                        ID_PERIODICIDAD5: null,
                    };

                    CargarDatosFormularioTP(datosEstrategia, idGrupo);
                } else {
                    $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEstrategiasApi/DatosEstrategiaID', {
                        tipo: tipo,
                        id: id
                    }).done(function (data) {
                        datosEstrategia = data;

                        CargarDatosFormularioTP(datosEstrategia, idGrupo);
                    });
                }
            }
            break;
        case 'F':
            {
                if (id == null) {
                    datosEstrategia = {
                        ID: null,
                        ID_GRUPO: null,
                        ID_ESTRATEGIA: null,
                        S_OTRO: null,
                        S_OBJETIVO: null,
                        S_PUBLICO_OBJETIVO: null,
                        S_COMUNICACION_INTERNA: null,
                        S_COMUNICACION_EXTERNA: null
                    };

                    CargarDatosFormularioTF(datosEstrategia, idGrupo);
                } else {
                    $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEstrategiasApi/DatosEstrategiaID', {
                        tipo: tipo,
                        id: id
                    }).done(function (data) {
                        datosEstrategia = data;

                        CargarDatosFormularioTF(datosEstrategia, idGrupo);
                    });
                }

                
            }
            break;
    }
}

function CargarDatosFormularioTP(datosEstrategia, idGrupo) {
    $("#frmDatosEstrategia").dxForm({
        labelLocation: 'top',
        scrollingEnabled: true,
        colCount: 1,
        formData: datosEstrategia,
        validationGroup: "estrategiaData",
        items: [{
            itemType: "tabbed",
            tabPanelOptions: {
                height: '100%'
            },
            tabs: [{
                title: "Estrategia",
                items: [
                    {
                        label: { text: 'ESTRATEGIA DE MOVILIDAD SOSTENIBLE' },
                        dataField: 'ID_ESTRATEGIA',
                        editorType: 'dxSelectBox',
                        editorOptions: {
                            items: DevExpress.data.query(estrategias).filter(["idGrupo", "=", idGrupo]).toArray(),
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
                        label: { text: "PUBLICO OBJETIVO" },
                        dataField: 'S_PUBLICO_OBJETIVO',
                        editorOptions: {
                            disabled: false
                        },
                    }, {
                        label: { text: 'TIPO DE EVIDENCIAS' },
                        dataField: 'S_TIPOEVIDENCIA',
                        template: function (data, itemElement) {
                            let divRow = $("<div class='form-group row'></div>");
                            let divCol1 = $("<div class='col-md-6'></div>");
                            let divCol2 = $("<div class='col-md-6'></div>");

                            let par = true;

                            let evidenciasSeleccionadas = (data.component.option('formData')[data.dataField] ?? '').split(',');

                            tiposEvidencia.forEach((tipoEvidencia) => {
                                let divEvidencia = (par ? divCol2 : divCol1);

                                divEvidencia.append($("<div id='tiposEvidencia" + tipoEvidencia.id + "'></div>")
                                    .dxCheckBox({
                                        text: tipoEvidencia.tipoEvidencia,
                                        value: evidenciasSeleccionadas.includes(tipoEvidencia.id.toString())
                                    })
                                ).append($("<br>"));

                                par = !par;
                            });

                            divRow.append(divCol2);
                            divRow.append(divCol1);
                            itemElement.append(divRow);
                        }
                    }
                ]
            }, {
                title: "Actividades",
                colCount: 2,
                items: [
                    {
                        label: { text: 'ID ACTIVIDAD 1' },
                        dataField: 'ID_ESTRATEGIA_ACTIVIDAD1',
                        visible: false
                    }, {
                        label: { text: 'ACTIVIDAD 1' },
                        dataField: 'S_ACTIVIDAD1',
                        editorOptions: {
                            disabled: false
                        }
                    }, {
                        label: { text: 'PERIODICIDAD 1' },
                        dataField: 'ID_PERIODICIDAD1',
                        editorType: 'dxSelectBox',
                        editorOptions: {
                            items: periodicidades,
                            placeholder: '[Seleccionar Periodicidad]',
                            displayExpr: 'periodicidad',
                            valueExpr: 'id',
                            disabled: false
                        }
                    }, {
                        label: { text: 'ACTIVIDAD 2' },
                        dataField: 'S_ACTIVIDAD2',
                        editorOptions: {
                            disabled: false
                        }
                    }, {
                        label: { text: 'PERIODICIDAD 2' },
                        dataField: 'ID_PERIODICIDAD2',
                        editorType: 'dxSelectBox',
                        editorOptions: {
                            items: periodicidades,
                            placeholder: '[Seleccionar Periodicidad]',
                            displayExpr: 'periodicidad',
                            valueExpr: 'id',
                            disabled: false
                        }
                    }, {
                        label: { text: 'ACTIVIDAD 3' },
                        dataField: 'S_ACTIVIDAD3',
                        editorOptions: {
                            disabled: false
                        }
                    }, {
                        label: { text: 'PERIODICIDAD 3' },
                        dataField: 'ID_PERIODICIDAD3',
                        editorType: 'dxSelectBox',
                        editorOptions: {
                            items: periodicidades,
                            placeholder: '[Seleccionar Periodicidad]',
                            displayExpr: 'periodicidad',
                            valueExpr: 'id',
                            disabled: false
                        }
                    }, {
                        label: { text: 'ACTIVIDAD 4' },
                        dataField: 'S_ACTIVIDAD4',
                        editorOptions: {
                            disabled: false
                        }
                    }, {
                        label: { text: 'PERIODICIDAD 4' },
                        dataField: 'ID_PERIODICIDAD4',
                        editorType: 'dxSelectBox',
                        editorOptions: {
                            items: periodicidades,
                            placeholder: '[Seleccionar Periodicidad]',
                            displayExpr: 'periodicidad',
                            valueExpr: 'id',
                            disabled: false
                        }
                    }, {
                        label: { text: 'ACTIVIDAD 5' },
                        dataField: 'S_ACTIVIDAD5',
                        editorOptions: {
                            disabled: false
                        }
                    }, {
                        label: { text: 'PERIODICIDAD 5' },
                        dataField: 'ID_PERIODICIDAD5',
                        editorType: 'dxSelectBox',
                        editorOptions: {
                            items: periodicidades,
                            placeholder: '[Seleccionar Periodicidad]',
                            displayExpr: 'periodicidad',
                            valueExpr: 'id',
                            disabled: false
                        }
                    },
                ]
            }]
        }]
    });
}

function CargarDatosFormularioTF(datosEstrategia, idGrupo) {
    $("#frmDatosEstrategia").dxForm({
        labelLocation: 'top',
        scrollingEnabled: true,
        colCount: 1,
        formData: datosEstrategia,
        validationGroup: "estrategiaData",
        items: [
            {
                label: { text: 'ESTRATEGIA DE MOVILIDAD SOSTENIBLE' },
                dataField: 'ID_ESTRATEGIA',
                editorType: 'dxSelectBox',
                editorOptions: {
                    items: DevExpress.data.query(estrategias).filter(["idGrupo", "=", idGrupo]).toArray(),
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
                label: { text: "OBJETIVO" },
                dataField: 'S_OBJETIVO',
                editorOptions: {
                    disabled: false
                },
            }, {
                label: { text: "PUBLICO OBJETIVO" },
                dataField: 'S_PUBLICO_OBJETIVO',
                editorOptions: {
                    disabled: false
                },
            }, {
                label: { text: "COMUNICACION INTERNA" },
                dataField: 'S_COMUNICACION_INTERNA',
                editorOptions: {
                    disabled: false
                },
            }, {
                label: { text: "COMUNICACION EXTERNA" },
                dataField: 'S_COMUNICACION_EXTERNA',
                editorOptions: {
                    disabled: false
                },
            }
        ]
    });
}

function EstrategiasGrupoDataSource(idGrupo) {
    let grdMetasGrupoDataSource = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var d = $.Deferred();

            $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEstrategiasApi/EstrategiasGrupo', {
                idEstrategiasTercero: $('#app').data('et'),
                idGrupo: idGrupo
            }).done(function (data) {
                d.resolve(data.datos, { totalCount: data.numRegistros });
            });
            return d.promise();
        },
        byKey: function (key) {
            return { id: key };
        }
    });

    return grdMetasGrupoDataSource;
}

function MetasGrupoDataSource(idGrupo) {
    let grdMetasGrupoDataSource = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var d = $.Deferred();

            $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESEstrategiasApi/MetasGrupo', {
                idEstrategiasTercero: $('#app').data('et'),
                idGrupo: idGrupo
            }).done(function (data) {
                d.resolve(data.datos, { totalCount: data.numRegistros });
            });
            return d.promise();
        },
        byKey: function (key) {
            return { id: key };
        },
        update: function (key, values) {
            //$.postJSON(
            $.getJSON(
                //$('#app').data('url') + 'EncuestaExterna/api/PMESEstrategiasApi/MetasGrupoActualizar', { idEstrategiasTercero: $('#app').data('et'), idEstrategiasMeta: values.ID_META, id: key.ID, valor: values.N_VALOR }
                $('#app').data('url') + 'EncuestaExterna/api/PMESEstrategiasApi/MetasGrupoActualizar?idEstrategiasTercero=' + $('#app').data('et') + '&idEstrategiasMeta=' + key.ID_META + '&id=' + key.ID + '&valor=' + values.N_VALOR
            ).done(function (data) {
                var grid = $('#grdMetasGrupo_' + idGrupo).dxDataGrid('instance');
                grid.refresh();
            });
        }
    });

    return grdMetasGrupoDataSource;
}

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
