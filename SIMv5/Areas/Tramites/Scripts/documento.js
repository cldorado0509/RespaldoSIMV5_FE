var siNoOpciones = [
    { ID: 'S', Nombre: 'SI' },
    { ID: 'N', Nombre: 'NO' },
    { ID: null, Nombre: '-' },
];

var TipoArchivoOpciones = [
    //{ ID: 1, Nombre: 'Documento Principal (*.docx)' },
    { ID: 1, Nombre: 'Documento Principal (*.pdf)' },
    //{ ID: 2, Nombre: 'Adjunto (*.pdf, *.docx)' },
    { ID: 2, Nombre: 'Adjunto (*.pdf)' },
];

var tipoTramites = [{ ID: 1, Nombre: "Seleccionar Trámites" }, { ID: 2, Nombre: "Nuevo Trámite" }];

var idSerieDocumental = -1;
var idGrupo = -1;
var indicesSerieDocumentalStore = null;
var firmasDocumento = null;
var archivosDocumento = null;
var tramitesSeleccionados = null;
var idArchivo = 0;
var archivoActualizar = null;
var opcionesLista = [];
var cargando = false;

var listoMostrar = false;

$(document).ready(function () {
    $("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
        shadingColor: "rgba(0,0,0,0.4)",
    });

    $('#tipoArchivo').dxSelectBox({
        dataSource: TipoArchivoOpciones,
        width: '100%',
        displayExpr: "Nombre",
        valueExpr: "ID",
        value: 1,
        placeholder: "[Tipo de Archivo]",
        readOnly: true
    });

    $('#serieDocumental').dxSelectBox({
        items: null,
        width: '100%',
        placeholder: 'Seleccionar Serie Documental',
        showClearButton: false,
        readOnly: ($('#app').data('ro') == 'S')
    });

    $.getJSON($('#app').data('url') + 'Tramites/api/ProyeccionDocumentoApi/ObtenerSeriesDocumentales').done(function (data) {
        $('#serieDocumental').dxSelectBox({
            items: data,
            displayExpr: 'NOMBRE',
            valueExpr: 'CODSERIE',
            placeholder: 'Seleccionar Serie Documental',
            showClearButton: false,
            onValueChanged: function (data) {
                idSerieDocumental = data.value;
                CargarIndices();
                idGrupo = -1;
                $('#grupo').dxSelectBox('instance').option('value', idGrupo);
            },
            onSelectionChanged: function (data) {
                if (data.selectedItem.GRUPO == 'S') {
                    $('#pnlGrupo').show();
                } else {
                    $('#pnlGrupo').hide();
                }
            }
        });

        $.getJSON($('#app').data('url') + 'Tramites/api/ProyeccionDocumentoApi/ObtenerDatosProyeccionDocumento', {
            id: $('#app').data('id'),
        }).done(function (data) {
            $('.my-cloak').removeClass('my-cloak');
            AjustarTamano();
            $('#asistente').accordion();

            $('#centroCostos').dxTextBox({
                value: data.CentroCostos,
                width: '100%',
                readOnly: true
            });

            $("#tipoTramites").dxRadioGroup({
                items: tipoTramites,
                value: data.TipoSeleccionTramites,
                valueExpr: 'ID',
                displayExpr: 'Nombre',
                layout: "horizontal",
                readOnly: ($('#app').data('ro') == 'S'),
                onOptionChanged: function (e) {
                    if (e.value == 1) {
                        $('#panelSeleccionTareas').show();
                    } else {
                        $('#panelSeleccionTareas').hide();
                    }
                }
            });

            $("#noAvanzaTramites").dxCheckBox({
                value: data.NoAvanzaTramites ?? false,
                readOnly: ($('#app').data('ro') == 'S')
            });

            if (data.TipoSeleccionTramites == 2) {
                $('#panelSeleccionTareas').hide();
            }

            tramitesSeleccionados = data.TramitesSeleccionados;

            tramitesSeleccionados.forEach(function (valor, indice, array) {
                tramitesSeleccionadosDataSource.store().insert({ CODTRAMITE: valor.CODTRAMITE, CODTAREA: valor.CODTAREA, S_ASUNTO: valor.S_ASUNTO });
            });
            tramitesSeleccionadosDataSource.load();

            $("#grdTramitesSeleccionados").dxDataGrid({
                dataSource: tramitesSeleccionadosDataSource
            });

            archivosDocumento = data.Archivos;
            $("#grdArchivos").dxDataGrid({
                dataSource: archivosDocumento
            });

            cargando = true;
            $('#serieDocumental').dxSelectBox({
                value: data.SerieDocumental
            });

            $('#serieDocumental').dxSelectBox('instance').option('value', data.SerieDocumental);

            $('#grupo').dxSelectBox({
                value: data.Grupo
            });


            selectedItem = $('#serieDocumental').dxSelectBox('instance').option('selectedItem');

            if (selectedItem && selectedItem.GRUPO == 'S') {
                $('#pnlGrupo').show();
            } else {
                $('#pnlGrupo').hide();
            }

            $('#descripcion').dxTextBox({
                value: data.Descripcion,
                width: '100%',
                maxLength: 240,
                readOnly: ($('#app').data('ro') == 'S')
            });

            archivosDocumento = data.Archivos;
            $("#grdArchivos").dxDataGrid({
                dataSource: archivosDocumento
            });

            if ($('#app').data('ro') == 'N') {
                if (archivosDocumento.findIndex(a => a.N_TIPO == 1) == -1)
                    $('#tipoArchivo').dxSelectBox('instance').option('value', 1);
                else
                    $('#tipoArchivo').dxSelectBox('instance').option('value', 2);
            }

            firmasDocumento = data.Firmas;

            $("#grdFirmas").dxDataGrid({
                dataSource: firmasDocumento
            });

            AsignarIndices(data.Indices);

            listoMostrar = true;
            VisualizarPagina();

            //loadPanel.hide();
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + error);
        });
    }).fail(function (jqxhr, textStatus, error) {
        alert('error cargando datos: ' + textStatus + ", " + error);
    });

    $('#grupo').dxSelectBox({
        items: null,
        width: '100%',
        placeholder: ' -- NINGUNO -- ',
        showClearButton: false,
        readOnly: ($('#app').data('ro') == 'S')
    });

    $.getJSON($('#app').data('url') + 'Tramites/api/ProyeccionDocumentoApi/ObtenerGrupos').done(function (data) {
        $('#grupo').dxSelectBox({
            items: data,
            displayExpr: 'S_NOMBRE',
            valueExpr: 'ID_GUPOMEMO',
            placeholder: 'Seleccionar Grupo',
            showClearButton: false,
            onValueChanged: function (data) {
                idGrupo = data.value;
            }
        });
    }).fail(function (jqxhr, textStatus, error) {
        alert('error cargando datos: ' + textStatus + ", " + error);
    });
    
    if ($('#app').data('ro') == 'N') {
        $("#grdTramites").dxDataGrid({
            dataSource: grdTramitesDataSource,
            allowColumnResizing: true,
            height: '100%',
            width: '100%',
            loadPanel: { text: 'Cargando Datos...' },
            paging: {
                enabled: false,
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
                    dataField: "CODTRAMITE",
                    width: '100px',
                    caption: 'TRAMITE',
                    dataType: 'number',
                    visible: true,
                }, {
                    dataField: 'S_ASUNTO',
                    caption: 'ASUNTO',
                    dataType: 'string',
                }
            ],
            onRowClick: function (e) {
                var component = e.component;

                function initialClick() {
                    component.clickCount = 1;
                    component.clickKey = e.key;
                    component.clickDate = new Date();
                }

                function doubleClick() {
                    component.clickCount = 0;
                    component.clickKey = 0;
                    component.clickDate = null;

                    if (BuscarTramiteSeleccionado(e.data.CODTRAMITE)) {
                        tramitesSeleccionadosDataSource.store().insert({ CODTRAMITE: e.data.CODTRAMITE, CODTAREA: e.data.CODTAREA, S_ASUNTO: e.data.S_ASUNTO });
                        tramitesSeleccionadosDataSource.load();
                    } else {
                        MostrarNotificacion('notify', 'error', 'El trámite ya se encuentra seleccionado.')
                    }

                    $('#grdTramitesSeleccionados').dxDataGrid({
                        dataSource: tramitesSeleccionadosDataSource
                    });
                }

                if ((!component.clickCount) || (component.clickCount != 1) || (component.clickKey != e.key)) {
                    initialClick();
                }
                else if (component.clickKey == e.key) {
                    if (((new Date()) - component.clickDate) <= 500)
                        doubleClick();
                    else
                        initialClick();
                }
            }
        });
    }

    $("#grdTramitesSeleccionados").dxDataGrid({
        dataSource: null,
        //keyExpr: 'CODTRAMITE',
        allowColumnResizing: true,
        height: '100%',
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
            allowUpdating: false,
            allowDeleting: false,
            allowAdding: false

        },
        selection: {
            mode: 'single',
        },
        columns: [
            {
                dataField: "CODTRAMITE",
                width: '100px',
                caption: 'TRAMITE',
                dataType: 'number',
                visible: true,
            }, {
                dataField: "CODTAREA",
                caption: 'TAREA',
                dataType: 'number',
                visible: false,
            }, {
                dataField: 'S_ASUNTO',
                caption: 'ASUNTO',
                dataType: 'string',
            }
        ],
        onRowClick: function (e) {
            if ($('#app').data('ro') == 'N') {
                var component = e.component;

                function initialClick() {
                    component.clickCount = 1;
                    component.clickKey = e.key;
                    component.clickDate = new Date();
                }

                function doubleClick() {
                    component.clickCount = 0;
                    component.clickKey = 0;
                    component.clickDate = null;

                    tramitesSeleccionadosDataSource.store().remove(e.key);
                    tramitesSeleccionadosDataSource.load();

                    $('#grdTramitesSeleccionados').dxDataGrid({
                        dataSource: tramitesSeleccionadosDataSource
                    });
                }

                if ((!component.clickCount) || (component.clickCount != 1) || (component.clickKey != e.key)) {
                    initialClick();
                }
                else if (component.clickKey == e.key) {
                    if (((new Date()) - component.clickDate) <= 500)
                        doubleClick();
                    else
                        initialClick();
                }
            }
        }
    });

    if ($('#app').data('ro') == 'N') {
        $('#descripcionArchivo').dxTextBox({
            width: '100%',
            maxLength: 120,
        });

        $('#agregarArchivo').dxButton({
            icon: 'upload',
            text: 'Cargar Archivo',
            width: '100%',
            onClick: function () {
                if ($('#descripcionArchivo').dxTextBox('instance').option('value') == null || $('#descripcionArchivo').dxTextBox('instance').option('value').trim() == '') {
                    MostrarNotificacion('alert', null, 'Descripción de Archivo Requerida');
                } else {
                    var fileUploader = $('#fileDocumento').dxFileUploader('instance');
                    fileUploader._isCustomClickEvent = true;
                    fileUploader._$fileInput.click();
                }
            }
        });

        $('#fileDocumento').dxFileUploader({
            name: 'file',
            uploadUrl: $('#app').data('url') + 'Tramites/ProyeccionDocumento/CargarArchivo',
            //selectButtonText: 'Seleccionar Archivo (*.docx, *.pdf)',
            selectButtonText: 'Seleccionar Archivo (*.pdf)',
            labelText: 'o arrastre un archivo aquí',
            //allowedFileExtensions: [".docx", ".pdf"],
            allowedFileExtensions: [".pdf"],
            showFileList: false,
            visible: false,
            onValueChanged: function (e) {
                var url = e.component.option('uploadUrl');

                if (archivoActualizar != null) {
                    idArchivo = archivoActualizar.ID_PROYECCION_DOC_ARCHIVOS;
                    t = archivoActualizar.N_TIPO;

                    url = updateQueryStringParameter(url, 'ida', idArchivo);
                    url = updateQueryStringParameter(url, 't', t);
                } else {
                    idArchivo = 0;

                    archivosDocumento.forEach(archivosDocumento => {
                        if (archivosDocumento.ID_PROYECCION_DOC_ARCHIVOS > idArchivo) {
                            idArchivo = archivosDocumento.ID_PROYECCION_DOC_ARCHIVOS;
                        }
                    });

                    idArchivo++;

                    t = $('#tipoArchivo').dxSelectBox('instance').option('value');

                    url = updateQueryStringParameter(url, 'ida', idArchivo);
                    url = updateQueryStringParameter(url, 't', t);
                }

                e.component.option('uploadUrl', url);
            },
            onUploadStarted: function (e) {
                $("#loadPanel").dxLoadPanel('instance').show();
            },
            onUploadAborted: function (e) {
                $("#loadPanel").dxLoadPanel('instance').hide();
            },
            onUploaded: function (e) {
                var orden = 0;

                var descripcion = $('#descripcionArchivo').dxTextBox('instance').option('value');
                var tipoArchivo = $('#tipoArchivo').dxSelectBox('instance').option('value');

                $('#descripcionArchivo').dxTextBox('instance').option('value', '');

                if (tipoArchivo == 2) {
                    archivosDocumento.forEach(archivosDocumento => {
                        if (archivosDocumento.N_ORDEN > orden) {
                            orden = archivosDocumento.N_ORDEN;
                        }
                    });

                    orden++;
                }

                if (archivoActualizar == null)
                    archivosDocumento.push({ ID_PROYECCION_DOC_ARCHIVOS: idArchivo, S_DESCRIPCION: descripcion, N_TIPO: tipoArchivo, S_ACTIVO: 'S', N_ORDEN: orden, ID_DOCUMENTO: 0, MODIFICADO: 'N', ARCHIVONUEVO: 'S', ARCHIVOACTUALIZADO: 'N', S_NOMBRE_ARCHIVO: e.file.name });
                else {
                    archivosDocumento.forEach(archivosDocumento => {
                        if (archivosDocumento.ID_PROYECCION_DOC_ARCHIVOS == idArchivo) {
                            archivosDocumento.MODIFICADO = 'S';
                            archivosDocumento.ARCHIVOACTUALIZADO = 'S';
                            archivosDocumento.S_NOMBRE_ARCHIVO = e.file.name;
                        }
                    });
                }

                archivoActualizar = null;

                $("#grdArchivos").dxDataGrid({
                    dataSource: archivosDocumento
                });

                $('#tipoArchivo').dxSelectBox('instance').option('value', 2);

                $("#loadPanel").dxLoadPanel('instance').hide();
            },
            onUploadError: function (e) {
                $("#loadPanel").dxLoadPanel('instance').hide();
                archivoActualizar = null;

                MostrarNotificacion('alert', null, 'Error Subiendo Archivo: ' + e.request.responseText);
            },
        });
    }

    $("#grdArchivos").dxDataGrid({
        dataSource: null,
        allowColumnResizing: true,
        //keyExpr: 'ID_PROYECCION_DOC_ARCHIVOS',
        height: '100%',
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
            useIcons: true
        },
        columns: [
            {
                dataField: "ID_PROYECCION_DOC_ARCHIVOS",
                dataType: 'number',
                visible: false,
            }, {
                dataField: "S_DESCRIPCION",
                caption: 'DESCRIPCION',
                width: '60%',
                dataType: 'string',
                allowEditing: false,
                visible: true,
                cellTemplate: function (cellElement, cellInfo) {
                    switch (cellInfo.data.N_TIPO) {
                        case 1: // Principal
                            cellElement.css('font-weight', 'bold');
                            break;
                        case 2: // Adjunto
                        default:
                            cellElement.css('font-weight', 'normal');
                            break;
                    }

                    if (cellInfo.data.MODIFICADO == 'S') {
                        cellElement.css('font-style', 'italic');
                    }

                    if (cellInfo.data.S_ACTIVO == 'N') {
                        cellElement.css('color', 'red');
                    }

                    cellElement.html(cellInfo.data.S_DESCRIPCION);
                },
            }, {
                dataField: "N_TIPO",
                caption: 'TIPO',
                width: '15%',
                dataType: 'string',
                allowEditing: false,
                visible: true,
                cellTemplate: function (cellElement, cellInfo) {
                    switch (cellInfo.data.N_TIPO) {
                        case 1: // Principal
                            cellElement.css('font-weight', 'bold');
                            break;
                        case 2: // Adjunto
                        default:
                            cellElement.css('font-weight', 'normal');
                            break;
                    }

                    if (cellInfo.data.MODIFICADO == 'S') {
                        cellElement.css('font-style', 'italic');
                    }

                    if (cellInfo.data.S_ACTIVO == 'N') {
                        cellElement.css('color', 'red');
                    }

                    cellElement.html(cellInfo.data.N_TIPO == 1 ? 'PRINCIPAL' : 'ADJUNTO');
                },
            }, {
                dataField: 'N_ORDEN',
                caption: 'ORDEN',
                alignment: 'center',
                width: '10%',
                dataType: 'number',
                visible: true,
                cellTemplate: function (cellElement, cellInfo) {
                    cellElement.css('text-align', 'center');

                    switch (cellInfo.data.N_TIPO) {
                        case 1: // Principal
                            cellElement.css('font-weight', 'bold');
                            break;
                        case 2: // Adjunto
                        default:
                            cellElement.css('font-weight', 'normal');
                            break;
                    }

                    if (cellInfo.data.MODIFICADO == 'S') {
                        cellElement.css('font-style', 'italic');
                    }

                    if (cellInfo.data.S_ACTIVO == 'N') {
                        cellElement.css('color', 'red');
                    }

                    cellElement.html(cellInfo.data.N_ORDEN);
                },
            }, {
                alignment: 'center',
                width: '5%',
                visible: ($('#app').data('ro') == 'N'),
                allowEditing: false,
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_ACTIVO == 'S') {
                        var div = document.createElement("div");
                        cellElement.get(0).appendChild(div);

                        $(div).dxButton({
                            icon: 'upload',
                            width: '100%',
                            onClick: function () {
                                archivoActualizar = { ID_PROYECCION_DOC_ARCHIVOS: cellInfo.data.ID_PROYECCION_DOC_ARCHIVOS, N_TIPO: cellInfo.data.N_TIPO };

                                var fileUploader = $('#fileDocumento').dxFileUploader('instance');
                                fileUploader._isCustomClickEvent = true;
                                fileUploader._$fileInput.click(); 
                            }
                        });
                    }
                },
            }, {
                alignment: 'center',
                width: '5%',
                visible: true,
                allowEditing: false,
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_ACTIVO == 'S') {
                        var div = document.createElement("div");
                        cellElement.get(0).appendChild(div);

                        $(div).dxButton({
                            icon: 'download',
                            width: '100%',
                            onClick: function () {
                                window.open($('#app').data('url') + 'Tramites/ProyeccionDocumento/DescargarDocumento?id=' + cellInfo.data.ID_PROYECCION_DOC_ARCHIVOS + '&n=' + cellInfo.data.ARCHIVONUEVO + '&a=' + cellInfo.data.ARCHIVOACTUALIZADO + '&f=' + utf8_to_b64(cellInfo.data.S_NOMBRE_ARCHIVO));
                            }
                        });
                    }
                },
            }, {
                alignment: 'center',
                width: '5%',
                visible: ($('#app').data('ro') == 'N'),
                allowEditing: false,
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_ACTIVO == 'S' && cellInfo.data.N_TIPO == 2) {
                        var div = document.createElement("div");
                        cellElement.get(0).appendChild(div);

                        $(div).dxButton({
                            icon: 'trash',
                            width: '100%',
                            onClick: function () {
                                var result = DevExpress.ui.dialog.confirm("Está Seguro de Eliminar el Archivo ?", "Confirmación");
                                result.done(function (dialogResult) {
                                    if (dialogResult) {
                                        cellInfo.data.S_ACTIVO = 'N';
                                        cellInfo.data.MODIFICADO = 'S';

                                        $("#grdArchivos").dxDataGrid({
                                            dataSource: archivosDocumento
                                        });
                                    }
                                });
                            }
                        });
                    }
                },
            }
        ],
        onEditorPreparing: function (e) {
            if (e.dataField === "N_ORDEN" && e.parentType === "dataRow") {
                e.editorOptions.readOnly = (e.value == 0 || e.row.data.S_ACTIVO == 'N');
            }
        },
        onRowUpdated: function (e) {
            e.data.MODIFICADO = 'S';
        },
    });

    if ($('#app').data('ro') == 'N') {
        $('#cboFuncionario').dxLookup({
            dataSource: funcionariosDataSource,
            placeholder: '[Seleccionar Funcionario]',
            title: 'Funcionario',
            displayExpr: 'FUNCIONARIO',
            valueExpr: 'CODFUNCIONARIO',
            cancelButtonText: 'Cancelar',
            pageLoadingText: 'Cargando...',
            refreshingText: 'Refrescando...',
            searchPlaceholder: 'Buscar',
            noDataText: 'Sin Datos',
        });

        $('#agregarFuncionario').dxButton(
            {
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
                        if (firmasDocumento.findIndex(f => f.CODFUNCIONARIO == item.CODFUNCIONARIO) == -1) {
                            var orden = 0;
                            var TipoFirma = "Firma";

                            if (item.FIRMADIG == "1") {
                                firmasDocumento.forEach(A => {
                                    if (A.S_TIPO == "Digital") {
                                        A.S_TIPO = "Firma";
                                    }
                                });
                                TipoFirma = "Digital";
                            }

                            firmasDocumento.forEach(fd => {
                                if (fd.ORDEN > orden) {
                                    orden = fd.ORDEN;
                                }
                            });

                            orden++;

                            firmasDocumento.push({ CODFUNCIONARIO: item.CODFUNCIONARIO, FUNCIONARIO: item.FUNCIONARIO, ORDEN: orden, D_FECHA_FIRMA: null, S_ESTADO: 'N', S_ACTIVO: 'S', S_TIPO: TipoFirma });
                            $("#grdFirmas").dxDataGrid({
                                dataSource: firmasDocumento
                            });
                        } else {
                            MostrarNotificacion('alert', null, 'El funcionario ya se encuentra registrado.');
                        }
                    }
                }
            });
    }

    $("#grdFirmas").dxDataGrid({
        dataSource: null,
        //keyExpr: 'CODFUNCIONARIO',
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
                dataType: 'number',
                visible: false,
            }, {
                dataField: "FUNCIONARIO",
                caption: 'NOMBRE',
                width: '60%',
                dataType: 'string',
                allowEditing: false,
                visible: true,
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_ACTIVO == 'N') {
                        cellElement.css('color', 'red');
                    }

                    cellElement.html(cellInfo.data.FUNCIONARIO);
                },
            }, {
                dataField: 'ORDEN',
                caption: 'ORDEN',
                alignment: 'center',
                width: '10%',
                dataType: 'number',
                visible: true,
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_ACTIVO == 'N') {
                        cellElement.css('color', 'red');
                    }

                    cellElement.html(cellInfo.data.ORDEN);
                },
            }, {
                dataField: 'S_REVISA',
                caption: 'REVISA',
                alignment: 'center',
                width: '10%',
                dataType: 'string',
                allowEditing: true,
                visible: true,
                cellTemplate: function (cellElement, cellInfo) {
                    var div = document.createElement("div");
                    cellElement.get(0).appendChild(div);

                    $(div).dxCheckBox({
                        value: (cellInfo.data.S_REVISA == 'S'),
                        disabled: true,
                    });
                },
                editCellTemplate: function (cellElement, cellInfo) {
                    var div = document.createElement("div");
                    div.setAttribute('style', 'width: 100%; text-align: center !important');
                    cellElement.get(0).appendChild(div);

                    $(div).dxCheckBox({
                        value: (cellInfo.data.S_REVISA == 'S'),
                        onValueChanged: function (e) {
                            cellInfo.setValue(e.value ? 'S' : 'N');

                            //if (e.value)
                            //    cellInfo.data.S_APRUEBA = 'N';
                        },
                    });
                }
            }, {
                dataField: 'S_APRUEBA',
                caption: 'APRUEBA',
                alignment: 'center',
                width: '10%',
                dataType: 'string',
                allowEditing: true,
                visible: true,
                cellTemplate: function (cellElement, cellInfo) {
                    var div = document.createElement("div");
                    cellElement.get(0).appendChild(div);

                    $(div).dxCheckBox({
                        value: (cellInfo.data.S_APRUEBA == 'S'),
                        disabled: true,
                    });
                },
                editCellTemplate: function (cellElement, cellInfo) {
                    var div = document.createElement("div");
                    div.setAttribute('style', 'width: 100%; text-align: center !important');
                    cellElement.get(0).appendChild(div);

                    $(div).dxCheckBox({
                        value: (cellInfo.data.S_APRUEBA == 'S'),
                        onValueChanged: function (e) {
                            cellInfo.setValue(e.value ? 'S' : 'N');
                            //if (e.value)
                            //    cellInfo.data.S_REVISA = 'N';
                        },
                    });
                }
            }, {
                dataField: 'S_TIPO',
                caption: 'Tipo',
                alignment: 'center',
                width: '10%',
                dataType: 'string',
                allowEditing: false,
                visible: true,
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_ACTIVO == 'N') {
                        cellElement.css('color', 'red');
                    }

                    cellElement.html(cellInfo.data.S_TIPO);
                }
                //    ,
                //    lookup: {
                //        dataSource: ["Firma", "Principal"]
                //    }
            }, {
                dataField: 'S_ESTADO',
                caption: 'FIRMADO',
                alignment: 'center',
                width: '10%',
                dataType: 'string',
                allowEditing: false,
                visible: true,
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_ACTIVO == 'N') {
                        cellElement.css('color', 'red');
                    }

                    cellElement.html(cellInfo.data.S_ESTADO);
                },
            }, {
                alignment: 'center',
                width: '5%',
                visible: ($('#app').data('ro') == 'N'),
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_ACTIVO == 'S') {
                        var div = document.createElement("div");
                        cellElement.get(0).appendChild(div);

                        $(div).dxButton({
                            icon: 'trash',
                            width: '100%',
                            onClick: function () {
                                var result = DevExpress.ui.dialog.confirm("Está Seguro(a) de Eliminar la Firma ?", "Confirmación");
                                result.done(function (dialogResult) {
                                    if (dialogResult) {
                                        cellInfo.data.S_ACTIVO = 'N';
                                    }
                                });
                            }
                        });
                    }
                },
            }
        ],
    });

    if ($('#app').data('ro') == 'N') {
        $('#almacenar').dxButton(
            {
                icon: '',
                text: 'Almacenar',
                width: '200px',
                type: 'success',
                elementAttr: {
                    style: "float: right;"
                },
                onClick: function (params) {
                    if (ValidarDatosMinimos()) {
                        $("#loadPanel").dxLoadPanel('instance').show();

                        $.postJSON(
                            $('#app').data('url') + 'Tramites/api/ProyeccionDocumentoApi/AlmacenarDatosProyeccionDocumento', {
                                id: $('#app').data('id'),
                                TipoSeleccionTramites: $('#tipoTramites').dxRadioGroup('instance').option('value'),
                                CentroCostos: $('#centroCostos').dxTextBox('instance').option('value'),
                                Descripcion: $('#descripcion').dxTextBox('instance').option('value'),
                                SerieDocumental: $('#serieDocumental').dxSelectBox('instance').option('value'),
                                Grupo: $('#grupo').dxSelectBox('instance').option('value'),
                                NoAvanzaTramites: $('#noAvanzaTramites').dxCheckBox('instance').option('value'),
                                TramitesSeleccionados: tramitesSeleccionadosDataSource.items(),
                                Indices: indicesSerieDocumentalStore._array,
                                Archivos: archivosDocumento,
                                Firmas: firmasDocumento
                        }
                        ).done(function (data) {
                            $("#loadPanel").dxLoadPanel('instance').hide();

                            var respuesta = data.split(':');

                            if (respuesta[0] == 'OK') {
                                var mensaje = 'Documento Almacenado Satisfactoriamente.' + (respuesta[2] != '' ? '<br><br>ATENCION: ' + respuesta[2] : '');
                                var result = MostrarNotificacion('alert', null, mensaje);
                                result.done(function () {
                                    //window.location.href = $('#app').data('url') + 'Tramites/ProyeccionDocumento/Documento?id=' + respuesta[1] + '&c=@DateTime.Now.ToString("HHmmss")';
                                    parent.CerrarPopupDocumento();
                                });
                            } else {
                                var result = MostrarNotificacion('alert', null, 'ERROR: ' + respuesta[2]);

                                result.done(function () {
                                    /*if (respuesta[1] != '0') {
                                        window.location.href = $('#app').data('url') + 'Tramites/ProyeccionDocumento/Documento?id=' + respuesta[1] + '&c=@DateTime.Now.ToString("HHmmss")';
                                    }*/
                                    parent.CerrarPopupDocumento();
                                });
                            }
                        }).fail(function (jqxhr, textStatus, error) {
                            $("#loadPanel").dxLoadPanel('instance').hide();
                            MostrarNotificacion('alert', null, 'ERROR DE INVOCACION: ' + textStatus + ", " + error);
                            MostrarNotificacion('alert', null, 'Se pudieron generar datos inconsistentes. Por favor cerrar esta ventana y revisar si el documento fue registrado.');
                        });
                    }
                }
            }
        );
    }
});

function ValidarDatosMinimos()
{
    if (tramitesSeleccionadosDataSource.items().length == 0 && $('#tipoTramites').dxRadioGroup('instance').option('value') == 1) {
        MostrarNotificacion('alert', null, 'Debe seleccionarse por lo menos una Tarea.');
        return false;
    }

    if ($('#serieDocumental').dxSelectBox('instance').option('value') == null) {
        MostrarNotificacion('alert', null, 'Serie Documental Requerida.');
        return false;
    }

    if ($('#descripcion').dxTextBox('instance').option('value') == null || $('#descripcion').dxTextBox('instance').option('value').trim() == '') {
        MostrarNotificacion('alert', null, 'Descripción Requerida.');
        return false;
    }

    if (archivosDocumento.findIndex(ad => ad.N_TIPO == 1 && ad.S_ACTIVO == 'S') == -1) {
        MostrarNotificacion('alert', null, 'Debe cargar por lo menos el archivo principal.');
        return false;
    }

    if (firmasDocumento.length == 0) {
        MostrarNotificacion('alert', null, 'Debe seleccionar por lo menos una firma.');
        return false;
    }
    //else {
    //    var _index = firmasDocumento.findIndex(f => f.S_TIPO == 'Principal');
    //    if (_index > -1) {
    //        if (firmasDocumento.filter(f => f.S_TIPO == 'Principal').length > 1) {
    //            MostrarNotificacion('alert', null, 'Solo puede ingresar un tipo de firma Principal.');
    //            return false;
    //        } else {
    //            if (_index < (firmasDocumento.length - 1)) {
    //                MostrarNotificacion('alert', null, 'El tipo de firma Principal debe ser el ùltimo que firma.');
    //                return false;
    //            }
    //        }
    //    }
    //}

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

function CargarIndices() {
    if (!cargando) {
        $.getJSON($('#app').data('url') + 'Tramites/api/ProyeccionDocumentoApi/ObtenerIndicesSerieDocumental', {
            codSerie: idSerieDocumental,
        }).done(function (data) {
            AsignarIndices(data);
        });
    }

    cargando = false;
}

function AsignarIndices(indices)
{
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
        CargarGridIndices();
    } else {
        opcionesLista.forEach(function (valor, indice, array) {
            $.getJSON($('#app').data('url') + 'Tramites/api/ProyeccionDocumentoApi/ObtenerIndiceValoresLista?id=' + valor.idLista).done(function (data) {
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

    cargando = false;
}

function CargarGridIndices()
{
    $("#grdIndices").dxDataGrid({
        dataSource: indicesSerieDocumentalStore,
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
            allowUpdating: ($('#app').data('ro') == 'N'),
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
            $.getJSON($('#app').data('url') + 'Tramites/api/ProyeccionDocumentoApi/Funcionarios', {
                filter: '',
                sort: '[{"selector":"FUNCIONARIO","desc":false}]',
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

function BuscarTramiteSeleccionado(codTramite) {
    var tramitesSeleccionadosActual = tramitesSeleccionadosDataSource.items();

    for (i = 0; i < tramitesSeleccionadosActual.length; i++) {
        if (tramitesSeleccionadosActual[i]['CODTRAMITE'] == codTramite) {
            return false;
        }
    }

    return true;
}


var grdTramitesDataSource = new DevExpress.data.CustomStore({
    key: "CODTRAMITE",
    load: function (loadOptions) {
        var d = $.Deferred();

        $.getJSON($('#app').data('url') + 'Tramites/api/ProyeccionDocumentoApi/TareasProyeccion', {
            f: '20',
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        });
        return d.promise();
    }
});

var tramitesSeleccionadosDataSource = new DevExpress.data.DataSource({ store: [], key: 'CODTRAMITE' });

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