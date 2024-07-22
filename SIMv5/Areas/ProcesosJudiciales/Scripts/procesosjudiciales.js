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
var grdDemandantesDataSource = new DevExpress.data.ArrayStore({ store: [] });
var grdDemandadosDataSource = new DevExpress.data.ArrayStore({ store: [] });

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
        }
    ];

    const tabsData2 = [
        {
            id: 0,
            text: 'JUDICIAL',
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
                caption: 'RADICADO PROCURADURIA',
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
                dataField: 'radicado21',
                caption: 'RADICADO PROCESO',
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
                                var _Ruta = $('#app').data('url') + "ProcesosJudiciales/api/ProcesosJudicialesApi/ObtenerProcesoJudicial"
                                $.getJSON(_Ruta,
                                    {
                                        Id: idProcesoActual
                                    }).done(function (data) {
                                        if (data !== null) {

                                            grdDemandadosDataSource = new DevExpress.data.ArrayStore({ store: [] });
                                            grdDemandantesDataSource = new DevExpress.data.ArrayStore({ store: [] });
                                            grdConvocantesDataSource = new DevExpress.data.ArrayStore({ store: [] });
                                            grdConvocadosDataSource = new DevExpress.data.ArrayStore({ store: [] });

                                            $("#grdDemandantes").dxDataGrid({ dataSource: grdDemandantesDataSource });
                                            $("#grdDemandados").dxDataGrid({ dataSource: grdDemandadosDataSource });
                                            $("#grdConvocantes").dxDataGrid({ dataSource: grdConvocantesDataSource });
                                            $("#grdConvocados").dxDataGrid({ dataSource: grdConvocadosDataSource });
                                        
                                            var procuraduriaId = data.procuraduriasId;
                                            procuraduria.option("value", procuraduriaId);

                                            var idApoderado = data.terceroId;
                                            apoderado.option("value", idApoderado);
                                            const apo = apoderado.option("text");

                                            apoderadotxt.option("value", apo);

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
                                            calidadEntidad.option("value", data.tipoDemanda);
                                            jurisdiccion.option("value", data.jurisdiccionId);
                                            fechaHechosP.option("value", data.fechaHechos);

                                            if (data.derechoAccionTutelaId) cboDerechosTutela.option("value", data.derechoAccionTutelaId.toString());
                                            if (data.derechoAccionPopularId) cboDerechosAccionPupular.option("value", data.derechoAccionPopularId.toString());

                                            fechaAdmision.option("value", data.fechaAdmisionProceso);
                                            fechaNotificacionJ.option("value", data.fechaNotificacionProceso);

                                            if (data.hechosProceso && data.hechosProceso.length > 0) {
                                                descripcionHechos.option("value", data.hechosProceso);
                                            }
                                            else {
                                                descripcionHechos.option("value", data.hechos);
                                            }

                                            if (data.pretensionesEtapaProcesal !== null && data.pretensionesEtapaProcesal.length > 0) {
                                                pretensionesDeclarativasHechos.option("value", data.pretensionesEtapaProcesal);
                                            }
                                            else {
                                                pretensionesDeclarativasHechos.option("value", data.pretenciones);
                                            }

                                            var llamaEnGarantia = data.llamaEnGarantia;
                                            var _llamaEnGarantia = 2;
                                            if (llamaEnGarantia === "1") _llamaEnGarantia = 1;
                                            optLlamaGerantia.option("value", _llamaEnGarantia);


                                            var cadicidad = data.caducidad;
                                            var _caducidad = 2;
                                            if (cadicidad === "1") _caducidad = 1;
                                            caducidad.option("value", _caducidad);

                                            fechaCaducidadHechos.option("value", data.fechaCaducidad);

                                            var departamentoh = "0";
                                            var municipioh = "0";

                                            if (data.municipioHechosId && data.municipioHechosId.length > 4) {
                                                departamentoh = data.municipioHechosId.substring(0, 2);
                                                departamentoHechosP.option("value", departamentoh);
                                                municipioh = data.municipioHechosId.substring(0, 5);

                                                var cboMunicipioHDs = municipioHechosP.getDataSource();
                                                _departamentoIdHP = departamentoh;
                                                cboMunicipioHDs.reload();
                                                municipioHechosP.option("value", municipioh);
                                            }


                                            if (data.cuantiaId && data.cuantiaId.length > 0) { 
                                                cboCuantia.option("value", data.cuantiaId.toString());
                                            }

                                            if (data.juramentoEstimatorioId && data.juramentoEstimatorioId.length > 0) {
                                                cboJuramenteoEstimatorio.option("value", data.juramentoEstimatorioId.toString());
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



                                            var rad23 = data.radicado21;
                                            var departamento = "0";
                                            var municipio = "0";
                                            var codigoJuzgado = "0";
                                            var codigoEspecialidad = "0";
                                            var consecutivoDespacho = 0;
                                            var consecutivoRadv = "00000"
                                            var anio = 2000;
                                            var consecutivoRecRadv = "00";

                                            if (rad23 !== null) {
                                                etapaProcesal = true;
                                                departamento = rad23.substring(0, 2);
                                                cboDepartamento.option("value", departamento);
                                                municipio = rad23.substring(0, 5);

                                                var cboCiudadDs = cboMunicipios.getDataSource();
                                                _departamentoId = departamento;
                                                cboCiudadDs.reload();
                                                cboMunicipios.option("value", municipio);

                                                codigoJuzgado = rad23.substring(5, 7);
                                                cboCategoriaJuzgado.option("value", codigoJuzgado);

                                                codigoEspecialidad = rad23.substring(7, 9);
                                                cboEspecialidadJuzgado.option("value", codigoEspecialidad);

                                                consecutivoDespacho = rad23.substring(9, 12);
                                                numeroJuzgado.option("value", consecutivoDespacho);

                                                anio = rad23.substring(12, 16);
                                                anioRad.option("value", anio);

                                                consecutivoRadv = rad23.substring(16, 21);
                                                consecutivoRad.option("value", consecutivoRadv);

                                                consecutivoRecRadv = rad23.substring(21, 23);
                                                consecutivoRecRad.option("value", consecutivoRecRadv);
                                            }


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
                                                if (demandante.esDemandante === '1') {
                                                    const idt = demandante.demandanteId;
                                                    const identificacion = demandante.identificacion;
                                                    const nombre = demandante.nombre;
                                                    const data = {
                                                        demantanteId: idt,
                                                        identificacion: identificacion,
                                                        nombre: nombre.toUpperCase(),
                                                        isNew: 0
                                                    };
                                                    grdDemandantesDataSource.insert(data);
                                                }

                                            }

                                            $("#grdConvocantes").dxDataGrid({ dataSource: grdConvocantesDataSource });
                                            $("#grdDemandantes").dxDataGrid({ dataSource: grdDemandantesDataSource });

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
                                                if (demandado.esDemandado === '1') {
                                                    const idt = demandado.demandadoId;
                                                    const identificacion = demandado.identificacion;
                                                    const nombre = demandado.nombre;
                                                    const data = {
                                                        demandadoId: idt,
                                                        identificacion: identificacion,
                                                        nombre: nombre.toUpperCase(),
                                                        isNew: 0
                                                    };
                                                    grdDemandadosDataSource.insert(data);
                                                }
                                            }
                                            $("#grdConvocados").dxDataGrid({ dataSource: grdConvocadosDataSource });
                                            $("#grdDemandados").dxDataGrid({ dataSource: grdDemandadosDataSource });

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

                                            if (data.radicado === null) {
                                                $('#tabOpciones').dxTabs('instance').option('dataSource', tabsData2);
                                                $('#tabOpciones').dxTabs('instance').repaint();
                                                $("#grdConvocantes").dxDataGrid('instance').option("visible", false);
                                                $("#grdConvocados").dxDataGrid('instance').option("visible", false);
                                                radicado.option("visible", false);
                                                procuraduria.option("visible", false);
                                                medioControl.option("visible", false);
                                                $('#divLabelsRowInfoBasica1').hide();
                                                $('#divLabelsRowInfoBasica2').hide();
                                                $('#divRowInfoBasica2').hide();
                                                $('#divLabelsRowInfoBasica3').hide();
                                                $('#divRowInfoBasica3').hide();
                                                $('#divbandInfoBasica').height(300);
                                                $('#divbandHechos').height(300);
                                                $('#divLabelRowTabTerceros').hide();
                                                $('#labelSujetos').hide();
                                                $('#divtabTerceros').hide();
                                                labelHechos.innerText = "Fundamentos";
                                                $('#divLabelHechos').hide();
                                                $('#divTextHechos').hide();
                                                $('#divtabAudienciaExtra').hide();
                                                $('#labelAudicencia').hide();
                                            } else
                                            {
                                                $('#tabOpciones').dxTabs('instance').option('dataSource', tabsData);
                                                $('#tabOpciones').dxTabs('instance').repaint();
                                                $('#tabOpciones').dxTabs('instance').re
                                                $("#grdConvocantes").dxDataGrid('instance').option("visible", true);
                                                $("#grdConvocados").dxDataGrid('instance').option("visible", true);
                                                radicado.option("visible", true);
                                                procuraduria.option("visible", true);
                                                medioControl.option("visible", true);
                                                $('#divLabelsRowInfoBasica1').show();
                                                $('#divLabelsRowInfoBasica2').show();
                                                $('#divRowInfoBasica2').show();
                                                $('#divLabelsRowInfoBasica3').show();
                                                $('#divRowInfoBasica3').show();
                                                $('#divbandInfoBasica').height(600);
                                                $('#divbandHechos').height(500);
                                                $('#divLabelRowTabTerceros').show();
                                                $('#labelSujetos').show();
                                                $('#divtabTerceros').show();
                                                labelHechos.innerText = "Hechos y Fundamentos";
                                                $('#divLabelHechos').show();
                                                $('#divTextHechos').show();
                                                $('#divtabAudienciaExtra').show();
                                                $('#labelAudicencia').show();
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
   
    $("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
        shadingColor: "rgba(0,0,0,0.4)",
    });


    var apoderadotxt = $("#apoderadotxt").dxTextBox({
        disabled: true,
        value: ''

    }).dxTextBox("instance");


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
        disabled: !canEdit,
        displayExpr: "valor",
        valueExpr: "id",
    }).dxSelectBox("instance");
        
    var jurisdiccion = $('#jurisdiccion').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "procesoCodigoId",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetJurisdicciones');
                }
            })
        }),
        placeholder: '[Jurisdicción]',
        value: null,
        disabled: !canEdit,
        displayExpr: "nombre",
        valueExpr: "procesoCodigoId",
        searchEnabled: true
    }).dxSelectBox("instance");

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
        disabled: !canEdit,
        displayExpr: "valor",
        valueExpr: "id",
        searchEnabled: true,
        onOpened: function (e) {
            e.component._popup.option('width', 500);
        }
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

      
    var codDaneDepRad = $("#codDaneDepRad").dxTextBox({
        value: '00000',
        disabled: !canEdit,

    }).dxTextBox("instance");

    var codJuzgadoRad = $("#codJuzgadoRad").dxTextBox({
        value: '00',
        disabled: !canEdit,
    }).dxTextBox("instance");

    var codEspecialidadRad = $("#codEspecialidadRad").dxTextBox({
        value: '00',
        disabled: !canEdit,
    }).dxTextBox("instance");
    
    var codDespachoRad = $("#codDespachoRad").dxTextBox({
        value: '000',
        disabled: !canEdit,
    }).dxTextBox("instance");
    
    var anioRad = $("#anioRad").dxNumberBox({
        placeholder: '[Año]',
        min: 2000,
        max: 2050,
        disabled: !canEdit,
        format: "####",
        value: 2024,
        showSpinButtons: true,
    }).dxNumberBox("instance");
    
    var cuantiaPretenciones = $("#cuantiaPretenciones").dxNumberBox({
        placeholder: '[Valor Cuantía]',
        min: 0,
        format: '$ #,##0',
        disabled: !canEdit,
        showClearButton: true,
        value: null,
        showSpinButtons: false,
    }).dxNumberBox("instance");


    var consecutivoRad = $("#consecutivoRad").dxTextBox({
        value: '00000',
        disabled: !canEdit,
    }).dxTextBox("instance");
    
    var consecutivoRecRad = $("#consecutivoRecRad").dxTextBox({
        value: '00',
        disabled: !canEdit,
    }).dxTextBox("instance");

  
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

    var fechaHechosP =  $('#fechaHechosP').dxDateBox({
        placeholder: '[Fecha Hechos]',
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

    var fechaNotificacionJ = $('#fechaNotificacionJ').dxDateBox({
        placeholder: '[F.Notificación]',
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
        disabled: !canEdit,
        displayExpr: "valor",
        valueExpr: "id",
        searchEnabled: true
    });

    var riesgoProcesal = $('#riesgoProcesal').dxSelectBox({
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
        disabled: !canEdit,
        displayExpr: "valor",
        valueExpr: "id",
        searchEnabled: true
    }).dxSelectBox("instance");

    var cboDepartamento = $('#cboDepartamento').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "codigoDane",
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
        disabled: !canEdit,
        displayExpr: "nombre",
        valueExpr: "codigoDane",
        searchEnabled: true
    }).dxSelectBox("instance");

    var cboMunicipios = $('#cboMunicipios').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "codigoDane",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetMunicipios', { departamentoId: _departamentoId });
                }
            })
        }),
        onValueChanged: function (data) {
            if (data.value != null) {
                codDaneDepRad.option("value", data.value);
            }
        },
        placeholder: '[Municipio]',
        value: null,
        disabled: !canEdit,
        displayExpr: "nombre",
        valueExpr: "codigoDane",
        searchEnabled: true
    }).dxSelectBox("instance");

    var departamentoHechosP = $('#departamentoHechosP').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "codigoDane",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetDepartamentos');
                }
            })
        }),
        onValueChanged: function (data) {
            if (data.value != null) {
                var cboCiudadDsP = municipioHechosP.getDataSource();
                _departamentoIdHP = data.value;
                cboCiudadDsP.reload();
                municipioHechosP.option("value", null);
            }
        },
        placeholder: '[Departamento]',
        value: null,
        disabled: !canEdit,
        displayExpr: "nombre",
        valueExpr: "codigoDane",
        searchEnabled: true
    }).dxSelectBox("instance");

    var municipioHechosP = $('#municipioHechosP').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "codigoDane",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetMunicipios', { departamentoId: _departamentoId });
                }
            })
        }),
        onValueChanged: function (data) {
            if (data.value != null) {
             
            }
        },
        placeholder: '[Municipio]',
        value: null,
        disabled: !canEdit,
        displayExpr: "nombre",
        valueExpr: "codigoDane",
        searchEnabled: true
    }).dxSelectBox("instance");
          
    var cboDerechosTutela = $('#cboDerechosTutela').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetDerechosTutela');
                }
            })
        }),
        placeholder: '[Derechos invocados]',
        value: null,
        disabled: !canEdit,
        displayExpr: "valor",
        valueExpr: "id",
        searchEnabled: true
    }).dxSelectBox("instance");
    
    var cboDerechosAccionPupular = $('#cboDerechosAccionPupular').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetDerechosAccionPupular');
                }
            })
        }),
        placeholder: '[Derechos invocados]',
        value: null,
        disabled: !canEdit,
        displayExpr: "valor",
        valueExpr: "id",
        searchEnabled: true
    }).dxSelectBox("instance");

    var cboCategoriaJuzgado = $('#cboCategoriaJuzgado').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetCorporacionesJuzgados');
                }
            })
        }),
        onValueChanged: function (data) {
            if (data.value != null) {
                var cboEspecialidadJuzgadoDs = cboEspecialidadJuzgado.getDataSource();
                _idCorporacionJuzgado = data.value;
                cboEspecialidadJuzgadoDs.reload();
                cboEspecialidadJuzgado.option("value", null);
                codJuzgadoRad.option("value", data.value);
            }
        },
        placeholder: '[Corporación, juzgado o entidad]',
        value: null,
        disabled: !canEdit,
        displayExpr: "valor",
        valueExpr: "id",
        searchEnabled: true
    }).dxSelectBox("instance");

    var cboTipoActuacion = $('#cboTipoActuacion').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetTiposActuacion');
                }
            })
        }),
        onValueChanged: function (data) {
           
        },
        placeholder: '[Tipo de actuación]',
        value: null,
        disabled: !canEdit,
        displayExpr: "valor",
        valueExpr: "id",
        searchEnabled: true
    }).dxSelectBox("instance");

    var cboJuzgadoActuacion = $('#cboJuzgadoActuacion').dxSelectBox({
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
        disabled: !canEdit,
        displayExpr: "valor",
        valueExpr: "id",
        searchEnabled: true,
        onOpened: function (e) {
            e.component._popup.option('width', 500);
        }
    }).dxSelectBox("instance");

    var fechaJuzgadoActuacion = $('#fechaJuzgadoActuacion').dxDateBox({
        placeholder: '[F.Juzgado]',
        disabled: !canEdit,
        value: null
    }).dxDateBox("instance");

    var codigoLitigio = $("#codigoLitigio").dxTextBox({
        value: '',
        disabled: true

    }).dxTextBox("instance");

    var fechaActuacion = $('#fechaActuacion').dxDateBox({
        placeholder: '[F.Actuación]',
        disabled: !canEdit,
        value: null
    }).dxDateBox("instance");

    var resumenActuacion = $("#resumenActuacion").dxTextArea({
        value: "",
        readOnly: false,
        disabled: !canEdit,
        height: 80,
        onValueChanged(e) {
            var value = e.component.option("value");
            if (value) {
                e.component.option("value", value.toUpperCase());
            }
        }
    }).dxTextArea("instance");


    var observacionActuacion = $("#observacionActuacion").dxTextArea({
        value: "",
        readOnly: false,
        disabled: !canEdit,
        height: 80,
        onValueChanged(e) {
            var value = e.component.option("value");
            if (value) {
                e.component.option("value", value.toUpperCase());
            }
        }
    }).dxTextArea("instance");

     
    $('#btnSaveActuacion').dxButton(
        {
            icon: 'save',
            text: '',
            hint: 'Guardar actuación',
            width: '40px',
            type: 'success',
            disabled: !canEdit,
            onClick: function (params) {
                //const data = {
                //    demandadoId: idTerSel,
                //    identificacion: nitTerSel.toUpperCase(),
                //    nombre: nombreTerSel.toUpperCase(),
                //    isNew: 1
                //};
                //grdConvocadosDataSource.insert(data);

                //$("#grdConvocados").dxDataGrid({ dataSource: grdConvocadosDataSource });

                popActuacion.hide();
            }
        });
    
    var cboEspecialidadJuzgado = $('#cboEspecialidadJuzgado').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetEspecialidadJuzgados?codigoCorporacion=' + _idCorporacionJuzgado);
                }
            })
        }),
        onValueChanged: function (data) {
            if (data.value != null) {
                codEspecialidadRad.option("value", data.value);
            }
        },
        placeholder: '[Sala o Especialidad]',
        value: null,
        disabled: !canEdit,
        displayExpr: "valor",
        valueExpr: "id",
        searchEnabled: true
    }).dxSelectBox("instance");

    var cboCuantia = $('#cboCuantia').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetCuantias');
                }
            })
        }),
        placeholder: '[Cuantía]',
        value: null,
        disabled: !canEdit,
        displayExpr: "valor",
        valueExpr: "id",
        searchEnabled: true
    }).dxSelectBox("instance");

    var cboJuramenteoEstimatorio = $('#cboJuramenteoEstimatorio').dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#app").data("url") + 'ProcesosJudiciales/api/ProcesosJudicialesApi/GetJuramentosEstimatorio');
                }
            })
        }),
        placeholder: '[Juramento estimatorio]',
        value: null,
        disabled: !canEdit,
        displayExpr: "valor",
        valueExpr: "id",
        searchEnabled: true
    }).dxSelectBox("instance");

    var valorEconomico = $('#valorEconomico').dxNumberBox({
        placeholder: '[Valor Económico del Proceso]',
        format: "$ #,##0.##",
        disabled: !canEdit,
        value: 0
    }).dxNumberBox("instance");

    var numeroJuzgado = $('#numeroJuzgado').dxNumberBox({
        placeholder: '[Número]',
        min: 1,
        max: 999,
        format: "##",
        value: 1,
        disabled: !canEdit,
        showSpinButtons: true,
        onValueChanged: function (data) {
            if (data.value != null) {
                let numero = data.value;
                let snumero = "000" + numero;
                snumero = snumero.slice(-3);
                codDespachoRad.option("value", snumero);
            }
        },
    }).dxNumberBox("instance");

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

    var descripcionHechos = $("#descripcionHechos").dxTextArea({
        value: "",
        readOnly: false,
        disabled: !canEdit,
        height: 160,
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
        disabled: !canEdit,
        max: 9999,
        showSpinButtons: true,
        value: 1
    }).dxNumberBox("instance");

    var hechos = $("#hechos").dxTextArea({
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

    var pretensionesDeclarativasHechos = $("#pretensionesDeclarativasHechos").dxTextArea({
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
        disabled: !canEdit,
        height: 90,
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
        disabled: !canEdit,
        height: 100,
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
    
    var caducidadHechos = $("#caducidadHechos").dxRadioGroup({
        dataSource: [{ text: "Si", valor: 1 }, { text: "No", valor: 2 }],
        displayExpr: "text",
        valueExpr: "valor",
        disabled: !canEdit,
        value: 1
    }).dxRadioGroup("instance");

    var caducidad = $("#caducidad").dxRadioGroup({
        dataSource: [{ text: "Si", valor: 1 }, { text: "No", valor: 2 }],
        displayExpr: "text",
        disabled: !canEdit,
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
        disabled: !canEdit,
        displayExpr: "valor",
        valueExpr: "id",
    }).dxSelectBox("instance");

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

    $("#cmdEditarNitDemandante").dxButton({
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
                const _asunto = asunto.option("value");
                const _despacho = procuraduria.option("text");
                const _radicado = radicado.option("value");
                const _instancia = 'EXTRAJUDICIAL';
                var _fechaAudiencia = fechaAudiencia.option("value");
                const _cuantia = cuantiaPretenciones.option("value");
                const _politicaInstitucional = politicasAplicables.option("value");
                var _llamaGarantia = optLlamaGerantia.option("value");
                const _apoderado = apoderado.option("text").replace('&', ' ');
                const _medioControl = medioControl.option("text");
                const _riesgoProcesal = riesgo.option("text").replace('&', ' ');;
                const _pretensiones = pretensiones.option("text").replace('&', ' ');;
                var _caducidad = caducidad.option("value");

                var _fechaRadicadov = fechaRadicado.option("value");
                var _hechos = hechos.option("value");
                var _recomendacionesAbogado = recomencionAbogado.option("value").replace('&', ' ');;
                var _fundamentoJuridicoConvocante = fundamentoJuridicoConvocante.option("value").replace('&', ' ');;
                var _fundamentoDefensa = fundamentoDefensa.option("value").replace('&', ' ');;
                var _fechaComiteConciliacion = fechaComiteConciliacion.option("value");
                var _fechaNotificacion = fechaNotificacion.option("value");
                var _decisionComite = decisionComite.option("value");
                var _hayAcuerdo = huboAcuerdoConciliatorio.option("value");
                var _decisionAudiencia = decisionAudiencia.option("value");
                var readOnly = "true";

                if (_fechaRadicadov === null) {
                    _fechaRadicadov = "";
                    
                }
                else {
                    _fechaRadicadov = _fechaRadicadov.toLocaleString();
                }

                if (_fechaNotificacion === null) {
                    _fechaNotificacion = "";
                }
                else {
                    _fechaNotificacion = _fechaNotificacion.toLocaleString();
                }

                _fechaAudiencia = "";

       
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


                var token = $('#app').data('token');
                var rutaTemplates = $('#app').data('rutaplantilla');

           
                var json = '{ "bytes":null, "name":"Ficha Técnica", "idPlantilla":21,"radicado":"' + _radicado + '","idProceso":' + idProcesoActual + ',"etiquetas":';

                var etiquetas = '[{"label":"[Cuantía]","value":"' + _cuantia + '"},{"label":"[Asunto]","value":"' + _asunto + '"},{"label":"[Caducidad]","value":"' + _caducidad + '"},{"label":"[Radicado]","value":"' + _radicado + ' - ' + _fechaRadicadov + '"},{"label":"[MedioControl]","value":"' + _medioControl + '"},{"label":"[Instancia]","value":"' + _instancia + '"},{"label":"[Convocante]","value":"' + demandantesArray + '"},{"label":"[Convocado]","value":"' + demandadosArray + '"},{"label":"[Apoderado]","value":"' + _apoderado + '"},{"label":"[Hechos]","value":"' + _hechos + '"},{"label":"[FundamentoJuricoConvocante]","value":"' + _fundamentoJuridicoConvocante + '"},{"label":"[FundamentoDefensa]","value":"' + _fundamentoDefensa + '"},{"label":"[RecomendacionAbogado]","value":"' + _recomendacionesAbogado + '"},{"label":"[Despacho]","value":"' + _despacho + '"},{"label":"[FechaAudiencia]","value":"' + _fechaAudiencia + '"},{"label":"[LlamaEnGarantia]","value":"' + _llamaGarantia + '"},{"label":"[PoliticaInstitucional]","value":"' + _politicaInstitucional + '"},{"label":"[Pretensiones]","value":"' + _pretensiones + '"},{"label":"[RiesgoProcesal]","value":"' + _riesgoProcesal + '"},{"label":"[FechaNotificacion]","value":"' + _fechaNotificacion + '"}]}';
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
                calidadEntidad.reset();
                jurisdiccion.reset();
                cboDerechosTutela.reset();
                cboDerechosAccionPupular.reset();
                fechaAdmision.reset();
                fechaNotificacionJ.reset();
                descripcionHechos.reset();

                grdConvocantesDataSource = new DevExpress.data.ArrayStore({ store: [] });
                grdConvocadosDataSource = new DevExpress.data.ArrayStore({ store: [] });
                grdDemandantesDataSource = new DevExpress.data.ArrayStore({ store: [] });
                grdDemandadosDataSource = new DevExpress.data.ArrayStore({ store: [] });

                $("#grdConvocantes").dxDataGrid({ dataSource: grdConvocantesDataSource });
                $("#grdConvocados").dxDataGrid({ dataSource: grdConvocadosDataSource });
                $("#grdDemandantes").dxDataGrid({ dataSource: grdDemandantesDataSource });
                $("#grdDemandados").dxDataGrid({ dataSource: grdDemandadosDataSource });

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

                $('#tabOpciones').dxTabs('instance').option('dataSource', tabsData2);
                $('#tabOpciones').dxTabs('instance').repaint();
                $("#grdConvocantes").dxDataGrid('instance').option("visible", false);
                $("#grdConvocados").dxDataGrid('instance').option("visible", false);
                radicado.option("visible", false);
                procuraduria.option("visible", false);
                medioControl.option("visible", false);
                $('#divLabelsRowInfoBasica1').hide();
                $('#divLabelsRowInfoBasica2').hide();
                $('#divRowInfoBasica2').hide();
                $('#divLabelsRowInfoBasica3').hide();
                $('#divRowInfoBasica3').hide();
                $('#divbandInfoBasica').height(300);
                $('#divbandHechos').height(300);
                $('#divLabelRowTabTerceros').hide();
                $('#labelSujetos').hide();
                $('#divtabTerceros').hide();
                labelHechos.innerText = "Fundamentos";
                $('#divLabelHechos').hide();
                $('#divTextHechos').hide();
                $('#divtabAudienciaExtra').hide();
                $('#labelAudicencia').hide();
            }
        });


    var grdTercerosP = $("#grdTercerosP").dxDataGrid({
        dataSource: grdTercerosDataSourceP,
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
    }).dxDataGrid('instance');

    var btnGuardarDemandante = $('#btnGuardarDemandante').dxButton(
        {
            icon: 'save',
            text: '',
            hint: 'Asignar Demandante',
            width: '340px',
            type: 'success',
            onClick: function (params) {

                const tipo = document.getElementById('popupDemandantes').getAttribute('data-tipo');

                if (tipo === "1") {
                    const data = {
                        demantanteId: idTerSel,
                        identificacion: nitTerSel.toUpperCase(),
                        nombre: nombreTerSel.toUpperCase(),
                        isNew: 1
                    };
                    grdDemandantesDataSource.insert(data);

                    $("#grdDemandantes").dxDataGrid({ dataSource: grdDemandantesDataSource });
                }
                else {
                    const data = {
                        demandadoId: idTerSel,
                        identificacion: nitTerSel.toUpperCase(),
                        nombre: nombreTerSel.toUpperCase(),
                        isNew: 1
                    };
                    grdDemandadosDataSource.insert(data);

                    $("#grdDemandados").dxDataGrid({ dataSource: grdDemandadosDataSource });
                }


                popupDemandantes.hide();
            }
       }).dxButton('instance');   

    var popupDemandantes = $("#popupDemandantes").dxPopup({
        width: 700,
        height: 'auto',
        hoverStateEnabled: true,
        title: "Terceros"
    }).dxPopup("instance");

    $('#agregarDemandante').dxButton(
        {
            icon: 'plus',
            text: '',
            width: '30x',
            hint: 'Adicionar demandante',
            type: 'success',
            elementAttr: {
                style: "float: right;"
            },
            onClick: function (params) {
                let btntipo = document.getElementById('popupDemandantes');
                btntipo.setAttribute('data-tipo', '1');
                popupDemandantes.show();
            }
        });

    $('#agregarDemandado').dxButton(
        {
            icon: 'plus',
            text: '',
            width: '30x',
            hint: 'Adicionar demandado',
            type: 'success',
            elementAttr: {
                style: "float: right;"
            },
            onClick: function (params) {
                let btntipo = document.getElementById('popupDemandantes');
                btntipo.setAttribute('data-tipo', '2');
                popupDemandantes.show();
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


                var token = $('#app').data('token');
                var rutaTemplates = $('#app').data('rutaplantilla');


                var url = rutaTemplates + token + "&readOnly=" + readOnly + "&documentoJS=" + json + etiquetas;


                window.open(url);
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
                caption: '',
                width: '70px',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'clear',
                            type: 'danger',
                            hint: 'Eliminar demandante',
                            onClick: function (params) {
                                grdDemandantesDataSource.remove(cellInfo.data);
                                $("#grdDemandantes").dxDataGrid({ dataSource: grdDemandantesDataSource });
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
                                grdDemandadosDataSource.remove(cellInfo.data);
                                $("#grdDemandados").dxDataGrid({ dataSource: grdDemandadosDataSource });
                            }
                        }
                    ).appendTo(cellElement);
                }
            }
        ]
    }).dxDataGrid("instance");
    
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
            }
        ]
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
                dataField: 'tipoActuacion',
                width: '20%',
                caption: 'Tipo de actuación',
                dataType: 'string',
            }, {
                dataField: 'actuacion',
                width: '40%',
                caption: 'Resumen de la actuación',
                dataType: 'string',
            }
            , {
                dataField: 'noficacionJudicialId',
                width: '10%',
                caption: 'Código Litigio Virtual',
                dataType: 'string',
            }, {
                dataField: 'finalizado',
                width: '10%',
                caption: 'Finalizado',
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
   

    $('#agregarActuacion').dxButton(
        {
            icon: 'plus',
            text: '',
            width: '30x',
            hint: 'Adicionar actuación',
            type: 'success',

            onClick: function (params) {
                popActuacion.show();
            }
        });

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
                var arraydata = grdDemandantesDataSource._array;
                var demandantesArray = [];

                for (i = 0; i < arraydata.length; i++) {
                    demandantesArray.push({ demantanteId: arraydata[i].demantanteId, identificacion: arraydata[i].identificacion, nombre: arraydata[i].nombre, isNew: arraydata[i].isNew });
                } 

                var arraydatad = grdDemandadosDataSource._array;
                var demandadosArray = [];

                for (i = 0; i < arraydatad.length; i++) {
                    demandadosArray.push({ demandadoId: arraydatad[i].demandadoId, identificacion: arraydatad[i].identificacion, nombre: arraydatad[i].nombre, isNew: arraydatad[i].isNew });
                } 

                var id = idProcesoActual;
                var _procesoJuzgadoId = 71;
                var _procuraduriasId = procuraduria.option("value");
                var _contrato = "";
                var _radicado = radicado.option("value");
                var _radicado21 = codDaneDepRad.option("value") + codJuzgadoRad.option("value") + codEspecialidadRad.option("value") + codDespachoRad.option("value") + anioRad.option("value") + consecutivoRad.option("value") + consecutivoRecRad.option("value");;
                var _fechaRadicado = fechaRadicado.option("value");
                var _cuantia = "";
                var _hechos = hechos.option("value");
                var _asunto = asunto.option("value");
                var _fechaNotificacion = fechaNotificacion.option("value");
                var _instanciaId = 0;
                var _eliminado = "0";
                var _recomendacionesAbogado = recomencionAbogado.option("value");
                var _tipoDemanda = calidadEntidad.option("value");
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
                var _jurisdiccion = jurisdiccion.option("value");
                var _derechoAccionTutelaId = cboDerechosTutela.option("value");
                var _derechoAccionPopularId = cboDerechosAccionPupular.option("value");
                var _hechosProceso = descripcionHechos.option("value");
                var _fechaAdmisionProceso = fechaAdmision.option("value");
                var _fechaNotificacionProceso = fechaNotificacionJ.option("value");
                var _fechaCaducidad = fechaCaducidadHechos.option("value");
                var _municipioHechosId = municipioHechosP.option("value");
                var _pretensionesEtapaProcesal = pretensionesDeclarativasHechos.option("value");
                var _cuantiaId = cboCuantia.option("value");
                var _juramentoEstimatorioId = cboJuramenteoEstimatorio.option("value");
                var _fechaHechos = fechaHechosP.option("value");

                var tabs = $('#tabOpciones').dxTabs("instance");
                let tabId = tabs.option("selectedIndex");
                

                if (tabId <= 0) {
                
                    if (_asunto === null || _asunto.length === 0) {
                        loadPanel.hide();
                        DevExpress.ui.notify("Ingrese la información relacionada con el asunto!", "warning", 2500);
                        return;
                    }

                    if (_apoderado === null) {
                        loadPanel.hide();
                        DevExpress.ui.notify("Seleccione el apoderado!", "warning", 2500);
                        return;
                    }
    
                    if (_fundamentoJuridicoConvocante === null || _fundamentoJuridicoConvocante.length === 0) {
                        loadPanel.hide();
                        DevExpress.ui.notify("Ingrese el fundamento jurídico del convocante!", "warning", 2500);
                        return;
                    }

                    if (_fundamentoDefensa === null || _fundamentoDefensa.length === 0) {
                        loadPanel.hide();
                        DevExpress.ui.notify("Ingrese los fundamentos de la defensa y excepciones!", "warning", 2500);
                        return;
                    }

                    if (_pretenciones === null || _pretenciones.length === 0) {
                        loadPanel.hide();
                        DevExpress.ui.notify("Ingrese el resumen de las pretensiones!", "warning", 2500);
                        return;
                    }

                    if (_valorCuantia === null || _valorCuantia.length === 0) {
                        loadPanel.hide();
                        DevExpress.ui.notify("Ingrese el valor de la cuantía de las pretensiones!", "warning", 2500);
                        return;
                    }

                    if (_riesgoProcesal === null || _riesgoProcesal.length === 0) {
                        loadPanel.hide();
                        DevExpress.ui.notify("Describa el riesgo procesal!", "warning", 2500);
                        return;
                    }

                    if (_politicaInstitucional === null || _politicaInstitucional.length === 0) {
                        loadPanel.hide();
                        DevExpress.ui.notify("Describa la política institucional!", "warning", 2500);
                        return;
                    }

                    if (_recomendacionesAbogado === null || _recomendacionesAbogado.length === 0) {
                        loadPanel.hide();
                        DevExpress.ui.notify("Ingrese las recomendaciones del abogado!", "warning", 2500);
                        return;
                    }

                 
                }
                if (tabId === 1) {
                    if (id <= 0) {
                        loadPanel.hide();
                        DevExpress.ui.notify("Se debe registrar primero la información de la etapa Extrajudicial!", "error", 2500);
                        return;
                    }
                }
                if (tabId === 2) {
                    if (id <= 0) {
                        loadPanel.hide();
                        DevExpress.ui.notify("Se debe registrar primero la información de la etapa Extrajudicial!", "error", 2500);
                        return;
                    }
                    if (etapaProcesal === false) {
                        loadPanel.hide();
                        DevExpress.ui.notify("Se debe registrar primero la información de la etapa procesal!", "error", 2500);
                        return;
                    }
                }
            
                var params = {
                    procesoId: id, procesoJuzgadoId: _procesoJuzgadoId, procuraduriasId: _procuraduriasId, contrato: _contrato, terceroId: _apoderado, radicado: _radicado, radicado21: _radicado21,
                    fechaRadicado: _fechaRadicado, cuantia: _cuantia, hechos: _hechos, fechaNotificacion: _fechaNotificacion, instanciaId: _instanciaId, politicaInstitucional: _politicaInstitucional,
                    eliminado: _eliminado, recomendacionesAbogado: _recomendacionesAbogado, tipoDemanda: _tipoDemanda, sincronizado: _sincronizado, terminado: _terminado, valorCuantia: _valorCuantia,
                    mensajeSincronizacion: _mensajeSincronizacion, fundamentoJuridicoConvocante: _fundamentoJuridicoConvocante, fundamentoDefensa: _fundamentoDefensa, riesgoProcesal: _riesgoProcesal, juramentoEstimatorioId: _juramentoEstimatorioId,
                    fechaAudienciaPrejudicial: _fechaAudienciaPrejudicial, fechaComiteConciliacion: _fechaComiteConciliacion, decisionComite: _decisionComite, hayAcuerdo: _hayAcuerdo, jurisdiccionId: _jurisdiccion, fechaHechos: _fechaHechos,
                    decisionAudiencia: _decisionAudiencia, caducidad: _caducidad, pretenciones: _pretenciones, llamaEnGarantia: _llamaEnGarantia, procesoCodigoId: _medioControlId, resumen: _resumen, cuantiaId: _cuantiaId,
                    hechosProceso: _hechosProceso, fechaAdmisionProceso: _fechaAdmisionProceso, fechaNotificacionProceso: _fechaNotificacionProceso, fechaCaducidad: _fechaCaducidad, municipioHechosId: _municipioHechosId,
                    pretensionesEtapaProcesal: _pretensionesEtapaProcesal,derechoAccionTutelaId: _derechoAccionTutelaId, derechoAccionPopularId: _derechoAccionPopularId,asunto: _asunto, demandantes: demandantesArray, demandados: demandadosArray
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

                            grdConvocantesDataSource = new DevExpress.data.ArrayStore({ store: [] });
                            grdConvocadosDataSource = new DevExpress.data.ArrayStore({ store: [] });
                            grdDemandantesDataSource = new DevExpress.data.ArrayStore({ store: [] });
                            grdDemandadosDataSource = new DevExpress.data.ArrayStore({ store: [] });
                            loadPanel.hide();
                            DevExpress.ui.dialog.alert('Proceso Judicial Creado/Actualizado correctamente', 'Guardar Datos');
                            $('#grdProcesosJudiciales').dxDataGrid({ dataSource: grdProcesosJudicialesDataSource });
                        }
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        loadPanel.hide();
                        DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                    }
                });

                $('#loadPanel').dxLoadPanel('instance').show();
                $('#listaProcesos').show();
                $('#detalleProcesos').hide();
                $('#loadPanel').dxLoadPanel('instance').hide();
                loadPanel.hide();
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
        onValueChanged: function (data) {
            if (data.value != null) {
                apoderadotxt.option("value", apoderado.option("text"));
            }
        },
        placeholder: '[Apoderado]',
        disabled: false,
        displayExpr: "nombre",
        valueExpr: "apoderadoId",
    }).dxSelectBox("instance");

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
        $.getJSON($('#app').data('url') + 'ProcesosJudiciales/api/ProcesosJudicialesApi/ConsultaActuaciones?procesoJudicialId=' + idProcesoActual , params)
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

var grdTercerosDataSourceP = new DevExpress.data.CustomStore({
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


