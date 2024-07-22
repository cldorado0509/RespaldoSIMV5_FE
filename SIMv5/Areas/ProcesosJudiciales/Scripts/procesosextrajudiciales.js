let posTab = 0;
let idProcesoActual = -1;
let _departamentoId = '0';
let idTerSel = 0;
let nombreTerSel = "";
let nitTerSel = "";
let _idCorporacionJuzgado = "00"
let etapaProcesal = false;

var grdConvocantesDataSource = new DevExpress.data.ArrayStore({ store: [] });
var grdConvocadosDataSource = new DevExpress.data.ArrayStore({ store: [] });

jQuery(function () {

    var loadPanel = $("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        hideOnOutsideClick: true,
        shading: true,
        shadingColor: "rgba(0,0,0,0.4)",
    }).dxLoadPanel('instance');

    $('#asistente').accordion({
        collapsible: true,
        animationDuration: 500,
        multiple: true
    });

    $('#asistente2').accordion({
        collapsible: true,
        animationDuration: 500,
        multiple: true
    });

    $('#asistente3').accordion({
        collapsible: true,
        animationDuration: 500,
        multiple: true
    });


    const tabsData = [
        {
            id: 0,
            text: 'EXTRAJUDICIAL',
        }
    ];


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
        hoverStateEnabled: true,
        remoteOperations: true,
        columns: [
            {
                dataField: "procesoId",
                dataType: 'number',
                visible: false
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
                dataField: 'demandadoDemandante',
                caption: 'PARTE',
                dataType: 'string',
            }, {
                dataField: 'fechaRegistro',
                caption: 'F. Registro',
                dataType: 'date',
                sortOrder : 'desc',
            }, {
                caption: '',
                width: '6%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'edit',
                            type: 'success',
                            visible: canEdit || canRead,
                            hint: 'Editar Proceso Judicial',
                            onClick: function (params) {
                                $('#loadPanel').dxLoadPanel('instance').show();
                                $('#detalleProcesos').show();

                                idProcesoActual = cellInfo.data.procesoId;
                                var _Ruta = $('#app').data('url') + "ProcesosJudiciales/api/ProcesosJudicialesApi/ObtenerProcesoExtraJudicial"
                                $.getJSON(_Ruta,
                                    {
                                        Id: idProcesoActual
                                    }).done(function (data) {
                                        if (data !== null) {
                                            grdConvocantesDataSource = new DevExpress.data.ArrayStore({ store: [] });
                                            grdConvocadosDataSource = new DevExpress.data.ArrayStore({ store: [] });
                                     
                                            $("#grdConvocantes").dxDataGrid({ dataSource: grdConvocantesDataSource });
                                            $("#grdConvocados").dxDataGrid({ dataSource: grdConvocadosDataSource });

                                
                                            var procuraduriaId = data.procuraduriasId;
                                            procuraduria.option("value", procuraduriaId);

                                            var idApoderado = data.terceroId;
                                            apoderado.option("value", idApoderado);
                                         
                                            var medioControlId = data.procesoCodigoId;
                                            medioControl.option("value", medioControlId);

                                            radicado.option("value", data.radicado);
                                            fechaRadicado.option("value", data.fechaRadicado);
                                            hechos.option("value", data.hechos);
                                            fechaNotificacion.option("value", data.fechaNotificacion);
                                            recomencionAbogado.option("value", data.recomendacionesAbogado);
                                            fundamentoJuridicoConvocante.option("value", data.fundamentoJuridicoConvocante);
                                            fundamentoDefensa.option("value", data.fundamentoDefensa);
                                            fechaComiteConciliacion.option("value", data.fechaComiteConciliacion);
                                            fechaAudiencia.option("value", data.fechaAudienciaPrejudicial);
                                            decisionComite.option("value", data.decisionComite);
                                            pretensiones.option("value", data.pretenciones);
                                            asunto.option("value", data.asunto);

                                            var llamaEnGarantia = data.llamaEnGarantia;
                                            var _llamaEnGarantia = 2;
                                            if (llamaEnGarantia === "1") _llamaEnGarantia = 1;
                                            optLlamaGerantia.option("value", _llamaEnGarantia);


                                            var cadicidad = data.caducidad;
                                            var _caducidad = 2;
                                            if (cadicidad === "1") _caducidad = 1;
                                            caducidad.option("value", _caducidad);

                                            if (data.cuantiaId && data.cuantiaId.length > 0) { 
                                                cboCuantia.option("value", data.cuantiaId.toString());
                                            }

                                         
                                            var hayAcuerdo = data.hayAcuerdo;
                                            var _hayAcuerdo = 2;
                                            if (hayAcuerdo === "1") _hayAcuerdo = 1;
                                            huboAcuerdoConciliatorio.option("value", _hayAcuerdo);
                                            
                                            var decisionAudienciav = data.decisionAudiencia;
                                            var _decisionAudiencia = 2;
                                            if (decisionAudienciav === "1") _decisionAudiencia = 1;
                                            decisionAudiencia.option("value", _decisionAudiencia);


                                            var decisionComitev = data.decisionComite;
                                            var _decisionComite = 2;
                                            if (decisionComitev === "1") _decisionComite = 1;
                                            decisionComite.option("value", _decisionComite);

                                            cuantiaPretenciones.option("value", data.valorCuantia);
                                            riesgo.option("value", data.riesgoProcesal);
                                            politicasAplicables.option("value", data.politicaInstitucional);

                                            var _demandantes = data.demandantes;
                                            for (i = 0; i < _demandantes.length; i++) {
                                                const demandante = _demandantes[i];
                                                if (demandante.esConvocante === '1') {
                                                    const idt = demandante.demandanteId;
                                                    const identificacion = demandante.identificacion;
                                                    const nombre = demandante.nombre;
                                                    const data = {
                                                        demantanteId: idt,
                                                        identificacion: identificacion,
                                                        nombre: nombre.toUpperCase(),
                                                        isNew: 0
                                                    };
                                                    grdConvocantesDataSource.insert(data);
                                                }
                                            }

                                            $("#grdConvocantes").dxDataGrid({ dataSource: grdConvocantesDataSource });
                                        
                                            var _demandados = data.demandados;

                                            for (i = 0; i < _demandados.length; i++) {
                                                const demandado = _demandados[i];
                                                if (demandado.esConvocado === '1') {
                                                    const idt = demandado.demandadoId;
                                                    const identificacion = demandado.identificacion;
                                                    const nombre = demandado.nombre;
                                                    const data = {
                                                        demandadoId: idt,
                                                        identificacion: identificacion,
                                                        nombre: nombre.toUpperCase(),
                                                        isNew: 0
                                                    };
                                                    grdConvocadosDataSource.insert(data);
                                                }
                                                
                                            }
                                            $("#grdConvocados").dxDataGrid({ dataSource: grdConvocadosDataSource });
                                
                                            if (data.existeDocumentoSolicitud) {
                                                $('#verSolicitud').dxButton("instance").option('icon', 'exportpdf');
                                                $('#verSolicitud').dxButton("instance").option('hint', 'Ver la solicitud');
                                            }
                                            else {
                                                $('#verSolicitud').dxButton("instance").option('icon', 'export'); 
                                                $('#verSolicitud').dxButton("instance").option('hint', 'Subir la solicitud');
                                            }

                                            if (data.existeDocumentoNotificacion) {
                                                $('#verNotificacion').dxButton("instance").option('icon', 'exportpdf');
                                                $('#verNotificacion').dxButton("instance").option('hint', 'Ver la notificación o citación a la audiencia');
                                            }
                                            else {
                                                $('#verNotificacion').dxButton("instance").option('icon', 'export');
                                                $('#verNotificacion').dxButton("instance").option('hint', 'Subir la notificación o citación a la audiencia');
                                            }

                                            if (data.existeActaComite) {
                                                $('#verComiteConciliacion').dxButton("instance").option('icon', 'exportpdf');
                                                $('#verComiteConciliacion').dxButton("instance").option('hint', 'Ver el acta del comité');
                                            }
                                            else {
                                                $('#verComiteConciliacion').dxButton("instance").option('icon', 'export');
                                                $('#verComiteConciliacion').dxButton("instance").option('hint', 'Subir el acta del comité');
                                            }

                                            if (data.existeActaAudiencia) {
                                                $('#verActaAudiencia').dxButton("instance").option('icon', 'exportpdf');
                                                $('#verActaAudiencia').dxButton("instance").option('hint', 'Ver el acta de la audiencia');
                                            }
                                            else {
                                                $('#verActaAudiencia').dxButton("instance").option('icon', 'export');
                                                $('#verActaAudiencia').dxButton("instance").option('hint', 'Subir el acta de la audiencia');
                                            }


                                          

                                        }
                                    }).fail(function (jqxhr, textStatus, error) {
                                        DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Evento no esperado!');
                                    });

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
   
  
    
    var medioControl = $('#medioControl').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "procesoCodigoId",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetMediosControl');
                }
            })
        }),
        placeholder: '[Medio de Control]',
        value: null,
        disabled: !canEdit,
        displayExpr: "nombre",
        valueExpr: "procesoCodigoId",
        searchEnabled: true
    }).dxSelectBox("instance"); 
    
    var radicado = $("#radicado").dxTextBox({
        value: '00000000000000000000', fileUploaderSolicitud,
        disabled: !canEdit,
    }).dxTextBox("instance");
        
    $("#fileUploaderSolicitud").dxFileUploader({
        allowedFileExtensions: [".pdf"],
        multiple: false,
        disabled: !canEdit,
        selectButtonText: 'Seleccionar Archivo ...',
        labelText: 'o arrastre un archivo aquí',
        uploadMode: "instantly",
        uploadUrl: $('#app').data('url') + 'ProcesosJudiciales/ProcesosJudiciales/CargarArchivoTemp?Tra=1',
        inputAttr: { 'aria-label': 'Select a file' },
        onUploadAborted: (e) => removeFile(e.file.name),
        onUploaded: function (e) {
        },
        onUploadStarted: function (e) {
        },
        onUploadError: function (e) {
            DevExpress.ui.dialog.alert('Error Subiendo Archivo: ' + e.request.responseText, 'Documentos temporales');
        },
        onValueChanged: function (e) {
         
        }
        
    });

    $("#fileUploaderNotificacion").dxFileUploader({
        allowedFileExtensions: [".pdf"],
        multiple: false,
        disabled: !canEdit,
        selectButtonText: 'Seleccionar Archivo ...',
        labelText: 'o arrastre un archivo aquí',
        uploadMode: "instantly",
        uploadUrl: $('#app').data('url') + 'ProcesosJudiciales/ProcesosJudiciales/CargarArchivoTemp?Tra=2',
        inputAttr: { 'aria-label': 'Select a file' },
        onUploadAborted: (e) => removeFile(e.file.name),
        onUploaded: function (e) {
        },
        onUploadStarted: function (e) {
        },
        onUploadError: function (e) {
            DevExpress.ui.dialog.alert('Error Subiendo Archivo: ' + e.request.responseText, 'Documentos temporales');
        },
        onValueChanged: function (e) {

        }

    });

    $("#fileUploaderComiteConciliacion").dxFileUploader({
        allowedFileExtensions: [".pdf"],
        multiple: false,
        disabled: !canEdit,
        selectButtonText: 'Seleccionar Archivo ...',
        labelText: 'o arrastre un archivo aquí',
        uploadMode: "instantly",
        uploadUrl: $('#app').data('url') + 'ProcesosJudiciales/ProcesosJudiciales/CargarArchivoTemp?Tra=3',
        inputAttr: { 'aria-label': 'Select a file' },
        onUploadAborted: (e) => removeFile(e.file.name),
        onUploaded: function (e) {
        },
        onUploadStarted: function (e) {
        },
        onUploadError: function (e) {
            DevExpress.ui.dialog.alert('Error Subiendo Archivo: ' + e.request.responseText, 'Documentos temporales');
        },
        onValueChanged: function (e) {

        }

    });

    $("#fileUploaderActaAudiencia").dxFileUploader({
        allowedFileExtensions: [".pdf"],
        multiple: false,
        disabled: !canEdit,
        selectButtonText: 'Seleccionar Archivo ...',
        labelText: 'o arrastre un archivo aquí',
        uploadMode: "instantly",
        uploadUrl: $('#app').data('url') + 'ProcesosJudiciales/ProcesosJudiciales/CargarArchivoTemp?Tra=4',
        inputAttr: { 'aria-label': 'Select a file' },
        onUploadAborted: (e) => removeFile(e.file.name),
        onUploaded: function (e) {
        },
        onUploadStarted: function (e) {
        },
        onUploadError: function (e) {
            DevExpress.ui.dialog.alert('Error Subiendo Archivo: ' + e.request.responseText, 'Documentos temporales');
        },
        onValueChanged: function (e) {

        }

    });
    
    var cuantiaPretenciones = $("#cuantiaPretenciones").dxNumberBox({
        placeholder: '[Valor Cuantía]',
        min: 0,
        disabled: !canEdit,
        format: '$ #,##0',
        showClearButton: true,
        value: null,
        showSpinButtons: false,
    }).dxNumberBox("instance");
    
   
    var fechaRadicado = $('#fechaRadicado').dxDateBox({
        placeholder: '[Fecha Radicado]',
        disabled: !canEdit,
        with: '180pt',
        value: null
    }).dxDateBox("instance");

    var fechaAdmision = $('#fechaAdmision').dxDateBox({
        placeholder: '[Fecha Admisión]',
        disabled: !canEdit,
        value: null
    }).dxDateBox("instance");

      

    var fechaNotificacion = $('#fechaNotificacion').dxDateBox({
        placeholder: '[F.Notificación]',
        disabled: !canEdit,
        value: null
    }).dxDateBox("instance");


    var fechaCaducidadHechos = $('#fechaCaducidadHechos').dxDateBox({
        placeholder: '[F.Caducidad]',
        disabled: !canEdit,
        value: null
    }).dxDateBox("instance");

       
    var fechaAudiencia = $('#fechaAudiencia').dxDateBox({
        placeholder: '[F.Audiencia]',
        disabled: !canEdit,
        value: null
    }).dxDateBox("instance");

    var procuraduria = $('#procuraduria').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "procuraduriaId",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetProcuradurias');
                }
            })
        }),
        placeholder: '[Procuraduría]',
        value: null,
        disabled: !canEdit,
        displayExpr: "nombre",
        valueExpr: "procuraduriaId",
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
        disabled: !canEdit,
        displayExpr: "valor",
        valueExpr: "id",
        searchEnabled: true
    });
    
    
    var asunto = $("#asunto").dxTextArea({
        value: "",
        readOnly: false,
        disabled: !canEdit,
        height: 100,
        onValueChanged(e) {
            var value = e.component.option("value");
            if (value) {
                e.component.option("value", value.toUpperCase());
            }
        }
    }).dxTextArea("instance");

      
    var sesionNro = $('#sesionNro').dxNumberBox({
        placeholder: '[Número de la Sesión]',
        format: "####",
        min: 1,
        max: 9999,
        disabled: !canEdit,
        showSpinButtons: true,
        value: 1
    }).dxNumberBox("instance");

    var hechos = $("#hechos").dxTextArea({
        value: "",
        readOnly: false,
        disabled: !canEdit,
        height: 100,
        onValueChanged(e) {
            var value = e.component.option("value");
            if (value) {
                e.component.option("value", value.toUpperCase());
            }
        } 
    }).dxTextArea("instance");

    var riesgo = $("#riesgo").dxTextArea({
        value: "",
        readOnly: false,
        disabled: !canEdit,
        height: 90,
        onValueChanged(e) {
            var value = e.component.option("value");
            if (value) {
                e.component.option("value", value.toUpperCase());
            }
        }
    }).dxTextArea("instance");

    var politicasAplicables = $("#politicasAplicables").dxTextArea({
        value: "",
        readOnly: false,
        disabled: !canEdit,
        height: 90,
        onValueChanged(e) {
            var value = e.component.option("value");
            if (value) {
                e.component.option("value", value.toUpperCase());
            }
        }
    }).dxTextArea("instance");
       

    var recomencionAbogado = $("#recomencionAbogado").dxTextArea({
        value: "",
        readOnly: false,
        disabled: !canEdit,
        height: 280,
        onValueChanged(e) {
            var value = e.component.option("value");
            if (value) {
                e.component.option("value", value.toUpperCase());
            }
        } 
    }).dxTextArea("instance");

    var pretensiones = $("#pretensiones").dxTextArea({
        value: "",
        readOnly: false,
        height: 90,
        disabled: !canEdit,
        onValueChanged(e) {
        var value = e.component.option("value");

        if (value) {
           e.component.option("value", value.toUpperCase());
        }
    } 
    }).dxTextArea("instance");

    var fundamentoJuridicoConvocante = $("#fundamentoJuridicoConvocante").dxTextArea({
        value: "",
        readOnly: false,
        height: 100,
        disabled: !canEdit,
        onValueChanged(e) {
            var value = e.component.option("value");
            if (value) {
                e.component.option("value", value.toUpperCase());
            }
        } 
    }).dxTextArea("instance");

    var fundamentoDefensa = $("#fundamentoDefensa").dxTextArea({
        value: "",
        readOnly: false,
        disabled: !canEdit,
        height: 100,
        onValueChanged(e) {
            var value = e.component.option("value");
            if (value) {
                e.component.option("value", value.toUpperCase());
            }
        } 
    }).dxTextArea("instance");

    var fechaComiteConciliacion = $('#fechaComiteConciliacion').dxDateBox({
        placeholder: '[F.Comité]',
        disabled: !canEdit,
        value: null
    }).dxDateBox("instance");

    var fechaAudiencia = $('#fechaAudiencia').dxDateBox({
        placeholder: '[F.Audiencia]',
        disabled: !canEdit,
        value: null
    }).dxDateBox("instance");

    var decisionComite = $("#decisionComite").dxRadioGroup({
        dataSource: [{ text: "Conciliar", valor: 1 }, { text: "No conciliar", valor: 2 }],
        displayExpr: "text",
        disabled: !canEdit,
        valueExpr: "valor",
        value: 1
    }).dxRadioGroup("instance");

    var decisionAudiencia = $("#decisionAudiencia").dxRadioGroup({
        dataSource: [{ text: "Aprueba", valor: 1 }, { text: "Desaprueba", valor: 2 }],
        displayExpr: "text",
        disabled: !canEdit,
        valueExpr: "valor",
        value: 1
    }).dxRadioGroup("instance");

    var huboAcuerdoConciliatorio = $("#huboAcuerdoConciliatorio").dxRadioGroup({
        dataSource: [{ text: "Si", valor: 1 }, { text: "No", valor: 2 }],
        displayExpr: "text",
        disabled: !canEdit,
        valueExpr: "valor",
        value: 1
    }).dxRadioGroup("instance");

    var optLlamaGerantia = $("#optLlamaGerantia").dxRadioGroup({
        dataSource: [{ text: "SI", valor: 1 }, { text: "NO", valor: 2 }],
        displayExpr: "text",
        disabled: !canEdit,
        valueExpr: "valor",
        value: 1
    }).dxRadioGroup("instance");
    
  

    var caducidad = $("#caducidad").dxRadioGroup({
        dataSource: [{ text: "Si", valor: 1 }, { text: "No", valor: 2 }],
        displayExpr: "text",
        valueExpr: "valor",
        disabled: !canEdit,
        value: 1
    }).dxRadioGroup("instance");
    
    var popupTercero = $("#popupTercero").dxPopup({
        width: 1400,
        height: 800,
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Terceros"
    }).dxPopup("instance");

    var popActuacion = $("#popActuacion").dxPopup({
        width: 850,
        height: "600",
        resizeEnabled: true,
        hoverStateEnabled: true,
        title: "Actuación"
    }).dxPopup("instance");

    $("#cmdEditarNitConvocante").dxButton({
        hint : "Administrar Terceros",
        text: "",
        icon: 'startswith',
        type: 'danger',
        width: '340px',
        onClick: function () {
             popupTercero.show();
            $('#buscarTercero').attr('src', $('#app').data('url') + 'General/Tercero');
        }
    }).dxButton("instance");

   
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
                var readOnly = "true";
                const _asunto = asunto.option("value");
                const _despacho = procuraduria.option("text");
                const _radicado = radicado.option("value");
                const _instancia = 'EXTRAJUDICIAL';
                const _fechaAudiencia = fechaAudiencia.option("value");
                const _cuantia = cuantiaPretenciones.option("value");
                const _politicaInstitucional = politicasAplicables.option("value");
                var _llamaGarantia = optLlamaGerantia.option("value");
                const _apoderado = apoderado.option("text").replace('&', ' ');
                const _medioControl = medioControl.option("text");
                const _riesgoProcesal = riesgo.option("text").replace('&', ' ');;
                const _pretensiones = pretensiones.option("text").replace('&', ' ');;
                var _caducidad = caducidad.option("value");

                const _fechaComite = new Date(fechaComiteConciliacion.option("value"));
                const _fechaDada = new Date(Date.now());

                var _fechaRadicadov = fechaRadicado.option("value");
                var _hechos = hechos.option("value");
                var _recomendacionesAbogado = recomencionAbogado.option("value").replace('&', ' ');;
                var _fundamentoJuridicoConvocante = fundamentoJuridicoConvocante.option("value").replace('&', ' ');;
                var _fundamentoDefensa = fundamentoDefensa.option("value").replace('&', ' ');;
                var _fechaNotificacion = fechaNotificacion.option("value");
                var _decisionComite = decisionComite.option("value");
                var _hayAcuerdo = huboAcuerdoConciliatorio.option("value");
                var _decisionAudiencia = decisionAudiencia.option("value");


                if (_fechaRadicadov === null) {
                    DevExpress.ui.notify("Debe indicar la fecha del radicado de la solicitud!", "warning", 2500);
                    return;
                }

                if (_fechaNotificacion === null) {
                    DevExpress.ui.notify("Debe indicar la fecha de notificación a la citación de la audiencia!", "warning", 2500);
                    return;
                }

                if (_fechaAudiencia === null) {
                    DevExpress.ui.notify("Debe indicar la fecha de la audicencia!", "warning", 2500);
                    return;
                }

                var arraydata = grdConvocantesDataSource._array;
                var demandantesArray = "";

                if (_llamaGarantia === 1) {
                    _llamaGarantia = "SI"
                } else {
                    _llamaGarantia = "NO"
                }

                if (_caducidad === 1) {
                    _caducidad = "SI"
                } else {
                    _caducidad = "NO"
                }

                for (i = 0; i < arraydata.length; i++) {
                    demandantesArray = demandantesArray + arraydata[i].nombre + ","
                }

                var arraydatad = grdConvocadosDataSource._array;
                var demandadosArray = "";

                for (i = 0; i < arraydatad.length; i++) {
                    demandadosArray = demandadosArray + arraydatad[i].nombre + ","
                }

                if (demandantesArray.length > 0) {
                    demandantesArray = demandantesArray.trim().substring(0, demandantesArray.length - 2);
                }

                if (demandadosArray.length > 0) {
                    demandadosArray = demandadosArray.trim().substring(0, demandadosArray.length - 2);
                }

                if (_fechaComite >= _fechaDada) {
                    readOnly = "false";
                } else {
                    readOnly = "true";
                }

                var token = $('#app').data('token');
                var rutaTemplates = $('#app').data('rutaplantilla');

           
                var json = '{ "bytes":null, "name":"Ficha Técnica", "idPlantilla":21,"radicado":"' + _radicado + '","idProceso":' + idProcesoActual + ',"etiquetas":';

                var etiquetas = '[{"label":"[Cuantía]","value":"' + _cuantia + '"},{"label":"[Asunto]","value":"' + _asunto + '"},{"label":"[Caducidad]","value":"' + _caducidad + '"},{"label":"[Radicado]","value":"' + _radicado + ' - ' + _fechaRadicadov.toLocaleString() + '"},{"label":"[MedioControl]","value":"' + _medioControl + '"},{"label":"[Instancia]","value":"' + _instancia + '"},{"label":"[Convocante]","value":"' + demandantesArray + '"},{"label":"[Convocado]","value":"' + demandadosArray + '"},{"label":"[Apoderado]","value":"' + _apoderado + '"},{"label":"[Hechos]","value":"' + _hechos + '"},{"label":"[FundamentoJuricoConvocante]","value":"' + _fundamentoJuridicoConvocante + '"},{"label":"[FundamentoDefensa]","value":"' + _fundamentoDefensa + '"},{"label":"[RecomendacionAbogado]","value":"' + _recomendacionesAbogado + '"},{"label":"[Despacho]","value":"' + _despacho + '"},{"label":"[FechaAudiencia]","value":"' + _fechaAudiencia.toLocaleString() + '"},{"label":"[LlamaEnGarantia]","value":"' + _llamaGarantia + '"},{"label":"[PoliticaInstitucional]","value":"' + _politicaInstitucional + '"},{"label":"[Pretensiones]","value":"' + _pretensiones + '"},{"label":"[RiesgoProcesal]","value":"' + _riesgoProcesal + '"},{"label":"[FechaNotificacion]","value":"' + _fechaNotificacion.toLocaleString() + '"}]}';
                var url = rutaTemplates + token + "&readOnly=" + readOnly + "&documentoJS=" + json + etiquetas;
          
                window.open(url);
                
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

                idProcesoActual = 0;
                asunto.reset();
                procuraduria.reset();
                radicado.reset();
                fechaRadicado.reset();
                hechos.reset();
                fechaNotificacion.reset();
                fechaAudiencia.reset();
                pretensiones.reset();
                recomencionAbogado.reset();
                fundamentoJuridicoConvocante.reset();
                fundamentoDefensa.reset();
                fechaComiteConciliacion.reset();
                decisionComite.reset();
                optLlamaGerantia.option("value", 2);
                caducidad.option("value", 2);
                huboAcuerdoConciliatorio.option("value",2);
                decisionAudiencia.option("value", 2);
                decisionComite.option("value", 2);
                apoderado.reset();
                medioControl.reset();
                cuantiaPretenciones.reset();
                riesgo.reset();
                politicasAplicables.reset();
      
                grdConvocantesDataSource = new DevExpress.data.ArrayStore({ store: [] });
                grdConvocadosDataSource = new DevExpress.data.ArrayStore({ store: [] });
           
                $("#grdConvocantes").dxDataGrid({ dataSource: grdConvocantesDataSource });
                $("#grdConvocados").dxDataGrid({ dataSource: grdConvocadosDataSource });
            
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

                
                $('#verSolicitud').dxButton("instance").option('icon', 'export');
                $('#verNotificacion').dxButton("instance").option('icon', 'export');
                $('#verComiteConciliacion').dxButton("instance").option('icon', 'export');
                $('#verActaAudiencia').dxButton("instance").option('icon', 'export');
                $('#verSolicitud').dxButton("instance").option('hint', 'Subir la solicitud');
                $('#verNotificacion').dxButton("instance").option('hint', 'Subir la notificación o citación a la audiencia');
                $('#verComiteConciliacion').dxButton("instance").option('hint', 'Subir el acta del comité');
                $('#verActaAudiencia').dxButton("instance").option('hint', 'Subir el acta de la audiencia');
         
            }
        });

    $('#agregarConvocante').dxButton(
        {
            icon: 'plus',
            text: '',
            width: '30x',
            hint: 'Adicionar convocante',
            type: 'success',
            disabled: !canEdit,
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
            disabled: !canEdit,
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
                var readOnly = "true";
                var arraydata = grdConvocantesDataSource._array;
                var demandantesArray = "";

                for (i = 0; i < arraydata.length; i++) {
                    demandantesArray = demandantesArray + arraydata[i].nombre + ","
                }

                var arraydatad = grdConvocadosDataSource._array;
                var demandadosArray = "";


                for (i = 0; i < arraydatad.length; i++) {
                    demandadosArray = demandadosArray + arraydatad[i].nombre + ","
                }



                if (demandantesArray.length > 0) {
                    demandantesArray = demandantesArray.trim().substring(0, demandantesArray.length - 2);
                }

                if (demandadosArray.length > 0) {
                    demandadosArray = demandadosArray.trim().substring(0, demandadosArray.length - 2);
                }

                const _fechaAudiencia = new Date(fechaAudiencia.option("value"));
                const _fechaComite = new Date(fechaComiteConciliacion.option("value"));
                const _fechaDada = new Date(Date.now());
                const _despacho = procuraduria.option("text");
                const _etapa = "ExtraJudicial";
                const _radicado = radicado.option("value");
                const _convocante = demandantesArray;
                const _convocado = demandadosArray;
                const _diaDada = _fechaDada.getDate();
                const _mesDada = _fechaDada.getMonth();
                const _anioDada = _fechaDada.getFullYear();
                const _funcionario = $('#app').data('funcionario');
                const _cargo = "SECRETARIO TÉCNICO COMITÉ DE CONCILIACIÓN";
                const _nroSesion = sesionNro.option("value");
                const _diaSesion = _fechaAudiencia.getDate();
                const _mesSesion = _fechaAudiencia.getMonth();
                const _anioSesion = _fechaAudiencia.getFullYear();
                const _recomendacion = recomencionAbogado.option("value");

                var json = '{ "bytes":null, "name":"Certificado", "idPlantilla":22,"radicado":"' + _radicado + '","idProceso":' + idProcesoActual + ',"etiquetas":';

                var etiquetas = '[{"label":"[Despacho]","value":"' + _despacho + '"},{"label":"[Etapa]","value":"' + _etapa + '"},{"label":"[Radicado]","value":"' + _radicado + '"},{"label":"[Convocante]","value":"' + _convocante + '"},{"label":"[Convocado]","value":"' + _convocado + '"},{"label":"[DiaDada]","value":"' + _diaDada + '"},{"label":"[MesDada]","value":"' + _mesDada + '"},{"label":"[AñoDada]","value":"' + _anioDada + '"},{"label":"[Funcionario]","value":"' + _funcionario + '"},{"label":"[Cargo]","value":"' + _cargo + '"},{"label":"[NroSesion]","value":"' + _nroSesion + '"},{"label":"[DiaSesion]","value":"' + _diaSesion + '"},{"label":"[MesSesion]","value":"' + _mesSesion + '"},{"label":"[AñoSesion]","value":"' + _anioSesion + '"},{"label":"[RecomendacionAbogado]","value":"' + _recomendacion + '"}]}';

                if (_fechaComite >= _fechaDada) {
                    readOnly = "false";
                } else {
                    readOnly = "true";
                }


                var token = $('#app').data('token');
                var rutaTemplates = $('#app').data('rutaplantilla');


                var url = rutaTemplates + token + "&readOnly=" + readOnly + "&documentoJS=" + json + etiquetas;

           
                window.open(url);
            }
        });
    
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
                visible : canDelete,
                caption: '',
                width: '70px',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'clear',
                            type: 'danger',
                            hint: 'Eliminar convocante',
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
                visible : canDelete,
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'clear',
                            type: 'danger',
                            hint: 'Eliminar convocado',
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
            type: 'success',
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
  
    $('#verSolicitud').dxButton(
        {
            icon: 'exportpdf',
            text: '',
            width: '40px',
            hint: 'Ver la solicitud',
            type: 'normal',
            onClick: function (params) {
                var _Ruta = $('#app').data('url') + 'ProcesosJudiciales/api/ProcesosJudicialesApi/ObtenerDocumentoAnexo?id=' + idProcesoActual + '&tipo=1';
                 $.getJSON(_Ruta).done(function (data)
                {
                     if (data) {

                         var docWindow = window.open("");
                         docWindow.document.write("<iframe width='100%' height='100%' src='data:application/pdf;base64, " + data + "'></iframe>")
                    }       
                }).fail(function (jqxhr, textStatus, error) {
                        loadIndicator.option("visible", false);
                        DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                });
            }
        });

    $('#verNotificacion').dxButton(
        {
            icon: 'exportpdf',
            text: '',
            width: '40px',
            disabled: !canEdit,
            hint: 'Ver la solicitud',
            type: 'normal',
            onClick: function (params) {
                var _Ruta = $('#app').data('url') + 'ProcesosJudiciales/api/ProcesosJudicialesApi/ObtenerDocumentoAnexo?id=' + idProcesoActual + '&tipo=2';
                $.getJSON(_Ruta).done(function (data) {
                    if (data) {

                        var docWindow = window.open("");
                        docWindow.document.write("<iframe width='100%' height='100%' src='data:application/pdf;base64, " + data + "'></iframe>")
                    }
                }).fail(function (jqxhr, textStatus, error) {
                    loadIndicator.option("visible", false);
                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                });
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
        width: '40px',
        hint: 'Ver la solicitud',
        type: 'normal',
        onClick: function (params) {
            var _Ruta = $('#app').data('url') + 'ProcesosJudiciales/api/ProcesosJudicialesApi/ObtenerDocumentoAnexo?id=' + idProcesoActual + '&tipo=3';
            $.getJSON(_Ruta).done(function (data) {
                if (data) {
                    var docWindow = window.open("");
                    docWindow.document.write("<iframe width='100%' height='100%' src='data:application/pdf;base64, " + data + "'></iframe>")
                }
            }).fail(function (jqxhr, textStatus, error) {
                loadIndicator.option("visible", false);
                DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
            });
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
            width: '40px',
            hint: 'Ver la solicitud',
            type: 'normal',
            onClick: function (params) {
                var _Ruta = $('#app').data('url') + 'ProcesosJudiciales/api/ProcesosJudicialesApi/ObtenerDocumentoAnexo?id=' + idProcesoActual + '&tipo=4';
                $.getJSON(_Ruta).done(function (data) {
                    if (data) {

                        var docWindow = window.open("");
                        docWindow.document.write("<iframe width='100%' height='100%' src='data:application/pdf;base64, " + data + "'></iframe>")
                    }
                }).fail(function (jqxhr, textStatus, error) {
                    loadIndicator.option("visible", false);
                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                });
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

                loadPanel.show();

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
                var _procesoJuzgadoId = 71;
                var _procuraduriasId = procuraduria.option("value");
                var _contrato = "";
                var _radicado = radicado.option("value");
                var _fechaRadicado = fechaRadicado.option("value");
                var _cuantia = "";
                var _hechos = hechos.option("value");
                var _asunto = asunto.option("value");
                var _fechaNotificacion = fechaNotificacion.option("value");
                var _instanciaId = 0;
                var _eliminado = "0";
                var _recomendacionesAbogado = recomencionAbogado.option("value");
                var _sincronizado = "0";
                var _terminado = "0";
                var _resumen = "";
                var _mensajeSincronizacion = "";
                var _fundamentoJuridicoConvocante = fundamentoJuridicoConvocante.option("value");
                var _fundamentoDefensa = fundamentoDefensa.option("value");
                var _fechaComiteConciliacion = fechaComiteConciliacion.option("value");
                var _decisionComite = decisionComite.option("value");
                var _hayAcuerdo = huboAcuerdoConciliatorio.option("value");
                var _decisionAudiencia = decisionAudiencia.option("value");
                var _apoderado = apoderado.option("value");
                var _medioControlId = medioControl.option("value");
                var _pretenciones = pretensiones.option("value");
                var _caducidad = caducidad.option("value");
                var _llamaEnGarantia = optLlamaGerantia.option("value");
                var _riesgoProcesal = riesgo.option("value");
                var _valorCuantia = cuantiaPretenciones.option("value");
                var _politicaInstitucional = politicasAplicables.option("value");
                var _fechaAudienciaPrejudicial = fechaAudiencia.option("value");
              
                var tabs = $('#tabOpciones').dxTabs("instance");
                let tabId = tabs.option("selectedIndex");
                

                if (tabId === 0) {
                    if (_radicado === null || _radicado.length === 0) {
                        DevExpress.ui.notify("Debe ingresar el número del radicado de la solicitud!", "warning", 2500);
                        loadPanel.hide();
                        return;
                    }

                    if (_procuraduriasId === null) {
                        DevExpress.ui.notify("Debe seleccionar la procuraduría!", "warning", 2500);
                        loadPanel.hide();
                        return;
                    }

                    if (_medioControlId === null) {
                        DevExpress.ui.notify("Debe seleccionar el medio de control o acción constitucional!", "warning", 2500);
                        loadPanel.hide();
                        return;
                    }

                    if (_fechaRadicado === null) {
                        loadPanel.hide();
                        DevExpress.ui.notify("Debe indicar la fecha del radicado de la solicitud!", "warning", 2500);
                        return;
                    }

                    if (_fechaNotificacion === null) {
                        DevExpress.ui.notify("Debe indicar la fecha de notificación a la citación de la audiencia!", "warning", 2500);
                        loadPanel.hide();
                        return;
                    }

                    if (_asunto === null || _asunto.length === 0) {
                        DevExpress.ui.notify("Ingrese la información relacionada con el asunto!", "warning", 2500);
                        loadPanel.hide();
                        return;
                    }

                    if (_apoderado === null) {
                        DevExpress.ui.notify("Seleccione el apoderado!", "warning", 2500);
                        loadPanel.hide();
                        return;
                    }

                    if (arraydata.length === 0) {
                        DevExpress.ui.notify("Debe seleccionar los convocantes!", "warning", 2500);
                        loadPanel.hide();
                        return;
                    }

                    if (arraydatad.length === 0) {
                        DevExpress.ui.notify("Debe seleccionar los convocados!", "warning", 2500);
                        loadPanel.hide();
                        return;
                    }

                    if (_hechos === null || _hechos.length === 0) {
                        DevExpress.ui.notify("Ingrese el resumen de los hechos!", "warning", 2500);
                        loadPanel.hide();
                        return;
                    }

                    if (_fundamentoJuridicoConvocante === null || _fundamentoJuridicoConvocante.length === 0) {
                        DevExpress.ui.notify("Ingrese el fundamento jurídico del convocante!", "warning", 2500);
                        loadPanel.hide();
                        return;
                    }

                    if (_fundamentoDefensa === null || _fundamentoDefensa.length === 0) {
                        DevExpress.ui.notify("Ingrese los fundamentos de la defensa y excepciones!", "warning", 2500);
                        loadPanel.hide();
                        return;
                    }

                    if (_pretenciones === null || _pretenciones.length === 0) {
                        DevExpress.ui.notify("Ingrese el resumen de las pretensiones!", "warning", 2500);
                        loadPanel.hide();
                        return;
                    }

                    if (_valorCuantia === null || _valorCuantia.length === 0) {
                        DevExpress.ui.notify("Ingrese el valor de la cuantía de las pretensiones!", "warning", 2500);
                        loadPanel.hide();
                        return;
                    }

                    if (_riesgoProcesal === null || _riesgoProcesal.length === 0) {
                        DevExpress.ui.notify("Describa el riesgo procesal!", "warning", 2500);
                        loadPanel.hide();
                        return;
                    }

                    if (_politicaInstitucional === null || _politicaInstitucional.length === 0) {
                        DevExpress.ui.notify("Describa la política institucional!", "warning", 2500);
                        loadPanel.hide();
                        return;
                    }

                    if (_recomendacionesAbogado === null || _recomendacionesAbogado.length === 0) {
                        DevExpress.ui.notify("Ingrese las recomendaciones del abogado!", "warning", 2500);
                        loadPanel.hide();
                        return;
                    }

                    if (_fechaAudienciaPrejudicial === null) {
                        DevExpress.ui.notify("Debe indicar la fecha de la audicencia!", "warning", 2500);
                        loadPanel.hide();
                        return;
                    }
                }
               
                var params = {
                    procesoId: id, procesoJuzgadoId: _procesoJuzgadoId, procuraduriasId: _procuraduriasId, contrato: _contrato, terceroId: _apoderado, radicado: _radicado,
                    fechaRadicado: _fechaRadicado, cuantia: _cuantia, hechos: _hechos, fechaNotificacion: _fechaNotificacion, instanciaId: _instanciaId, politicaInstitucional: _politicaInstitucional,
                    eliminado: _eliminado, recomendacionesAbogado: _recomendacionesAbogado, sincronizado: _sincronizado, terminado: _terminado, valorCuantia: _valorCuantia,
                    mensajeSincronizacion: _mensajeSincronizacion, fundamentoJuridicoConvocante: _fundamentoJuridicoConvocante, fundamentoDefensa: _fundamentoDefensa, riesgoProcesal: _riesgoProcesal,
                    fechaAudienciaPrejudicial: _fechaAudienciaPrejudicial, fechaComiteConciliacion: _fechaComiteConciliacion, decisionComite: _decisionComite, hayAcuerdo: _hayAcuerdo,
                    decisionAudiencia: _decisionAudiencia, caducidad: _caducidad, pretenciones: _pretenciones, llamaEnGarantia: _llamaEnGarantia, procesoCodigoId: _medioControlId, resumen: _resumen,
                    asunto: _asunto, demandantes: demandantesArray, demandados: demandadosArray
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
                        loadPanel.hide();
                        if (data.IsSuccess === false) DevExpress.ui.dialog.alert('Ocurrió un error ' + data.Message, 'Guardar Datos');
                        else {

                            grdConvocantesDataSource = new DevExpress.data.ArrayStore({ store: [] });
                            grdConvocadosDataSource = new DevExpress.data.ArrayStore({ store: [] });
                            DevExpress.ui.dialog.alert('Proceso Judicial Creado/Actualizado correctamente', 'Guardar Datos');
                            $('#grdProcesosJudiciales').dxDataGrid({ dataSource: grdProcesosJudicialesDataSource });
                        }
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                    }
                });

                $('#listaProcesos').show();
                $('#detalleProcesos').hide();
              
            }
        });

    var apoderado = $('#cboApoderado').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "apoderadoId",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetApoderados');
                }
            })
        }),
        placeholder: '[Apoderado]',
        disabled: !canEdit,
        displayExpr: "nombre",
        valueExpr: "apoderadoId",
    }).dxSelectBox("instance");

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
        $.getJSON($('#app').data('url') + 'ProcesosJudiciales/api/ProcesosJudicialesApi/ConsultaProcesosExtraJudiciales', params)
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


