var IdReposicion = -1;
var IdDetReposicion = 0;
var idDocActo = 0;
var idAsunto = 0;
var idVisita = 0;
var tipomedida = 0;
var idControl = 0;
var idDocumento = 0;
var IdRegistro = -1;


$(document).ready(function () {

    $("#GidListado").dxDataGrid({
        dataSource: ReposicionesDataSource,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        export: {
            enabled: true,
            allowExportSelectedData: true,
        },
        paging: {
            pageSize: 5
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20, 50]
        },
        filterRow: {
            visible: true,
            emptyPanelText: 'Arrastre una columna para agrupar'
        },
        searchPanel: {
            visible: false,
            width: 240,
            placeholder: "Buscar..."
        },
        selection: {
            mode: 'single'
        },
        hoverStateEnabled: true,
        remoteOperations: false,
        columns: [
            { dataField: 'ID', width: '5%', caption: 'Id', alignment: 'center' },
            { dataField: 'ANIO_ACTO', width: '10%', caption: 'Año Acto', visible: false },
            { dataField: 'CM', width: '5%', caption: 'CM', alignment: 'center' },
            { dataField: 'PROYECTO', width: '20%', caption: 'Nombre del Proyecto - (Instalación)', alignment: 'center' },
            { dataField: 'ASUNTO', width: '20%', caption: 'Asunto - (Permiso)', alignment: 'center' },
            { dataField: 'OBSERVACIONES', width: '35%', caption: 'Observaciones', dataType: 'string' },
            { dataField: 'AUTORIZADO', width: '10%', caption: 'Autorizado', visible: false },
            { dataField: 'CANTIDAD_DESTOCONADO', width: '10%', caption: 'Cantidad Destoconado', visible: false },
            { dataField: 'CANTIDAD_LEVANTAMIENTO_PISO', width: '10%', caption: 'Cantidad Levantamiento Piso', visible: false },
            { dataField: 'CANTIDAD_MANTENIMIENTO', width: '10%', caption: 'Cantidad Mantenimiento', visible: false },
            { dataField: 'CODIGO_ACTOADMINISTRATIVO', width: '10%', caption: 'Código Acto Administrativo', visible: false },
            { dataField: 'CODIGO_SOLICITUD', width: '10%', caption: 'Código Solicitud', visible: false },
            { dataField: 'CONSERVACION_AUTORIZADO', width: '10%', caption: 'Conservación Autorizada', visible: false },
            { dataField: 'CONSERVACION_EJECUTADA', width: '10%', caption: 'Canservación Ejecutada', visible: false },
            { dataField: 'CONSERVACION_SOLICITADO', width: '10%', caption: 'Conservación Solicitada', visible: false },
            { dataField: 'COORDENADAX', width: '10%', caption: 'Coordenada X', visible: false },
            { dataField: 'COORDENADAY', width: '10%', caption: 'Coordenada Y', visible: false },
            { dataField: 'DAP_MEN_10_AUTORIZADO', width: '10%', caption: 'Dap Men 10 Autorizado', visible: false },
            { dataField: 'DAP_MEN_10_EJECUTADA', width: '10%', caption: 'Dap Men 10 Ejecutado', visible: false },
            { dataField: 'DAP_MEN_10_SOLICITADO', width: '10%', caption: 'Dap Menor 10 Solicitado', visible: false },
            { dataField: 'ID_USUARIOVISITA', width: '10%', caption: 'Id usuario Visita', visible: false },
            { dataField: 'INVERSION_MEDIDA_ADICIONAL_DESTOCONADO', width: '10%', caption: 'Inversión Medida Adicional Destoconado', visible: false },
            { dataField: 'INVERSION_MEDIDA_ADICIONAL_LEVANTAMIENTO_PISO', width: '10%', caption: 'Inversión Medida Adicional Levantamiento Piso', visible: false },
            { dataField: 'INVERSION_MEDIDA_ADICIONAL_MANTENIMIENTO', width: '10%', caption: 'Inversión Medida Adicional Mantenimiento', visible: false },
            { dataField: 'INVERSION_MEDIDA_ADICIONAL_SIEMBRA', width: '10%', caption: 'Inversión Medida Adicional Siembra', visible: false },
            { dataField: 'INVERSION_MEDIDAS_ADICIONALES', width: '10%', caption: 'Inversión Medidas Adicionales', visible: false },
            { dataField: 'INVERSION_REPOSICION_MINIMA', width: '10%', caption: 'Inversión Reposición Mínima', visible: false },
            { dataField: 'MEDIDA_ADICIONAL_ASIGNADA', width: '10%', caption: 'Medida Adicional Asignada', visible: false },
            { dataField: 'MEDIDA_ADICIONAL_EJECUTADA', width: '10%', caption: 'Medida Adicional Ejecutada', visible: false },
            { dataField: 'NOMBREPROYECTO', width: '10%', caption: 'Nombre Proyecto', visible: false },
            { dataField: 'NRO_LENIOS_AUTORIZADOS', width: '10%', caption: 'Nro Leños Autorizados', visible: false },
            { dataField: 'NRO_LENIOS_SOLICITADOS', width: '10%', caption: 'Nro Leños Solicitados', visible: false },
            { dataField: 'NUMERO_ACTO', width: '10%', caption: 'Número Acto Administrativo', visible: false },
            { dataField: 'OBSERVACIONES', width: '10%', caption: 'Observaciones', visible: false },
            { dataField: 'OBSERVACIONVISITA', width: '10%', caption: 'Observaciones Visita', visible: false },
            { dataField: 'PAGO_FONDO_VERDE_METROPOLITANO', width: '10%', caption: 'Pago Fondo Verde Metropolitano', visible: false },
            { dataField: 'PODA_AUTORIZADO', width: '10%', caption: 'Poda Autorizada', visible: false },
            { dataField: 'PODA_EJECUTADA', width: '10%', caption: 'Poda Ejecutada', visible: false },
            { dataField: 'PODA_SOLICITADO', width: '10%', caption: 'Poda Solicitada', visible: false },
            { dataField: 'RADICADOVISITA', width: '10%', caption: 'Radicado Visita', visible: false },
            { dataField: 'REPOSICION_AUTORIZADO', width: '10%', caption: 'Reposición Autorizada ', visible: false },
            { dataField: 'REPOSICION_EJECUTADA', width: '10%', caption: 'Reposición Ejecutada', visible: false },
            { dataField: 'REPOSICION_MINIMA_OBLIGATORIA', width: '10%', caption: 'Reposición Mínima Obligatoria', visible: false },
            { dataField: 'REPOSICION_PROPUESTA', width: '10%', caption: 'Reposición Propuesta', visible: false },
            { dataField: 'TALA_AUTORIZADO', width: '10%', caption: 'Tala Autorizada', visible: false },
            { dataField: 'TALA_EJECUTADA', width: '10%', caption: 'Tala Ejecutada', visible: false },
            { dataField: 'TALA_SOLICITADA', width: '10%', caption: 'Tala Solicitada', visible: false },
            { dataField: 'TIPO_DOCUMENTO', width: '10%', caption: 'Tipo Documento', visible: false },
            { dataField: 'TIPOMEDIDAADICIONAL', width: '10%', caption: 'Tipo de Medida Adicional', visible: false },
            { dataField: 'TRASPLANTE_AUTORIZADO', width: '10%', caption: 'Transplante Autorizado', visible: false },
            { dataField: 'TRASPLANTE_EJECUTADO', width: '10%', caption: 'Transplante Ejecutado', visible: false },
            { dataField: 'TRASPLANTE_SOLICITADO', width: '10%', caption: 'Transplante Solicitado', visible: false },
            { dataField: 'VALORACION_INVENTARIO_FORESTAL', width: '10%', caption: 'Valoración Inventario Forestal', visible: false },
            { dataField: 'VALORACION_TALA', width: '10%', caption: 'Valoración Tala', visible: false },
            { dataField: 'VOLUMEN_AUTORIZADO', width: '10%', caption: 'Volumen Autorizado', visible: false },
            { dataField: 'VOLUMEN_EJECUTADO', width: '10%', caption: 'Volumen Ejecutado', visible: false },
            { dataField: 'DIRECCION', width: '10%', caption: 'Dirección', visible: false },
            { dataField: 'ENTIDAD_PUBLICA', width: '10%', caption: 'Es Entidad Pública', visible: false },
            {
                visible: canEdit,
                width: 40,
                caption: "Editar",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        caption: "Editar",
                        height: 20,
                        hint: 'Editar la información del registro',
                        onClick: function (e) {

                            var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/TramitesNuevosApi/ObtenerReposicion";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.ID
                                }).done(function (data) {
                                    if (data !== null) {
                                        IdRegistro = parseInt(data.id);
                                        IdReposicion = parseInt(data.id);
                                        idAsunto = parseInt(data.codigoSolicitud);
                                        txtCM.option("value", data.cm);
                                        txtProyecto.option("value", data.proyecto);
                                        txtNombreProyectoA.option("value", data.nombreProyecto);
                                        tipomedida = data.tipoMedidaId;
                                        txtAsunto.option("value", data.asunto);
                                        txtTalaAut.option("value", data.talaAutorizada);
                                        txtdAPMen10Autorizada.option("value", data.dAPMen10Autorizada);
                                        txtvolumenAutorizado.option("value", data.volumenAutorizado);
                                        txttransplanteAutorizado.option("value", data.transplanteAutorizado);
                                        txtpodaAutorizada.option("value", data.podaAutorizada);
                                        txtconservacionAutorizada.option("value", data.conservacionAutorizada);
                                        txtObservaciones.option("value", data.observaciones);
                                        txtCoordenadaX.option("value", data.coordenadaX);
                                        txtCoordenadaY.option("value", data.coordenadaY);
                                        txtreposicionPropuesta.option("value", data.reposicionPropuesta);
                                        txtreposicionMinimaObligatoria.option("value", data.reposicionMinimaObligatoria);
                                        txtTalaSol.option("value", data.talaSolicitada);
                                        txtdAPMen10Solicitada.option("value", data.dAPMen10Solicitada)
                                        txtNroLeniososAut.option("value", data.nroLeniosAutorizados);
                                        txtNroLeniososSol.option("value", data.nroLeniosSolicitados);
                                        txtValoracionInventarioForestal.option("value", data.valoracionInventarioForestal);
                                        txtValoracionTala.option("value", data.valoracionTala);
                                        txtInversionReposicionMinima.option("value", data.inversionReposicionMinima);
                                        txtInversionMedidasAdicionales.option("value", data.inversionMedidasAdicionales);
                                        txtCantidadSiembraAdicional.option("value", data.cantidadSiembraAdicional);
                                        txtInversionMedidaAdicionalSiembra.option("value", data.inversionMedidaAdicionalSiembra);
                                        txtCantidadMantenimiento.option("value", data.cantidadMantenimiento);
                                        txtInversionMedidaAdicionalMantenimiento.option("value", data.inversionMedidaAdicionalMantenimiento);
                                        txtCantidadDestoconado.option("value", data.cantidadDestoconado);
                                        txtInversionMedidaAdicionalDestoconado.option("value", data.inversionMedidaAdicionalDestoconado);
                                        txtCantidadLevantamientoPiso.option("value", data.cantidadLevantamientoPiso);
                                        txtInversionMedidaAdicionalLevantamientoPiso.option("value", data.inversionMedidaAdicionalLevantamientoPiso);
                                        txtPagoFondoVerde.option("value", data.pagoFondoVerdeMetropolitano);
                                        popup.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                                });
                        }
                    }).appendTo(container);
                }
            },
            {
                visible: canDelete,
                width: '5%',
                caption: "Eliminar",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'remove',
                        height: 20,
                        hint: 'Eliminar la Reposición',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar el registro seleccionado?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/TramitesNuevosApi/EliminarReposicion";
                                    $.getJSON(_Ruta,
                                        {
                                            id: options.data.ID
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar registro seleccionado');
                                            else {
                                                $('#GidListado').dxDataGrid({ dataSource: ReposicionesDataSource });
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar registro seleccionado');
                                        });
                                }
                            });
                        }
                    }).appendTo(container);
                }
            },
            {
                width: '5%',
                caption: "Seguimientos de control",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'event',
                        height: 20,
                        hint: 'Seguimientos de control',
                        onClick: function (e) {
                            IdReposicion = options.data.ID;
                            IdDetReposicion = 0;
                            $("#divLblCM").dxTextBox({
                                value: "CM :" + options.data.CM,
                                readOnly: true,
                            }).dxTextBox("instance");
                            $("#GridListadoControl").dxDataGrid({
                                dataSource: ControlDataSource,
                                with: "95%",
                                height: 'auto',
                                allowColumnResizing: true,
                                loadPanel: { enabled: true, text: 'Cargando Datos...' },
                                noDataText: "Sin datos para mostrar",
                                showBorders: true,
                                paging: {
                                    pageSize: 10
                                },
                                pager: {
                                    showPageSizeSelector: true,
                                    allowedPageSizes: [5, 10, 20, 50]
                                },
                                filterRow: {
                                    visible: false,
                                    emptyPanelText: 'Arrastre una columna para agrupar'
                                },
                                searchPanel: {
                                    visible: false,
                                    width: 240,
                                    placeholder: "Buscar..."
                                },
                                selection: {
                                    mode: 'single'
                                },
                                hoverStateEnabled: true,
                                remoteOperations: false,
                                columns: [
                                    { dataField: 'id', width: '10%', caption: 'Id', alignment: 'center' },
                                    { dataField: 'fechaControl', width: '15%', caption: 'Fecha Registro', alignment: 'center', dataType: 'date' },
                                    { dataField: 'tramiteId', width: '15%', caption: 'Trámite (Tarea)', alignment: 'center' },
                                    { dataField: 'radicado', width: '10%', caption: 'Radicado' },
                                    { dataField: 'anioRadicado', width: '15%', caption: 'Año Radicado' },
                                    { dataField: 'descripcionEstado', width: '15%', caption: 'Estado' },
                                    { dataField: 'tecnico', width: '20%', caption: 'Técnico' },
                                    { dataField: 'documentoId', width: '2%', caption: 'Técnico', visible: false },
                                    {
                                        width: 40,
                                        alignment: 'center',
                                        cellTemplate: function (container, options) {
                                            $('<div/>').dxButton({
                                                icon: 'edit',
                                                height: 20,
                                                caption: 'Editar',
                                                hint: 'Editar la información del control',
                                                onClick: function (e) {
                                                    var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/TramitesNuevosApi/ObtenerRegistroControl";
                                                    idControl = options.data.id;
                                                    $.getJSON(_Ruta,
                                                        {
                                                            Id: options.data.id
                                                        }).done(function (data) {
                                                            if (data !== null) {
                                                                txtRadicadoSTN.option("value", data.radicado);
                                                                /*  txtTalaEje.option("value", data.talaEjecutada);*/

                                                                popupNewControl.show();
                                                            }
                                                        }).fail(function (jqxhr, textStatus, error) {
                                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                                                        });
                                                }
                                            }).appendTo(container);
                                        }
                                    },
                                    {
                                        width: '5%',
                                        caption: "Eliminar",
                                        alignment: 'center',
                                        cellTemplate: function (container, options) {
                                            $('<div/>').dxButton({
                                                icon: 'remove',
                                                height: 20,
                                                hint: 'Eliminar la Reposición',
                                                onClick: function (e) {
                                                    var result = DevExpress.ui.dialog.confirm('Desea eliminar el registro seleccionado?', 'Confirmación');
                                                    result.done(function (dialogResult) {
                                                        if (dialogResult) {
                                                            var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/TramitesNuevosApi/EliminarSeguimiento";
                                                            $.getJSON(_Ruta,
                                                                {
                                                                    id: options.data.id
                                                                }).done(function (data) {
                                                                    if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar registro seleccionado');
                                                                    else {
                                                                        $('#GridListadoControl').dxDataGrid({ dataSource: ControlDataSource });
                                                                    }
                                                                }).fail(function (jqxhr, textStatus, error) {
                                                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar registro seleccionado');
                                                                });
                                                        }
                                                    });
                                                }
                                            }).appendTo(container);
                                        }
                                    },
                                    {
                                        width: '100px',
                                        caption: "Ver",
                                        alignment: 'center',
                                        cellTemplate: function (container, options) {
                                            $('<div/>').dxButton({
                                                icon: 'event',
                                                height: 20,
                                                hint: 'Ver documento',
                                                onClick: function (e) {
                                                    var CodTramite = options.row.data.tramiteId;
                                                    var CodDocumento = options.row.data.documentoId;
                                                    var _popup = $("#popDocumento").dxPopup("instance");
                                                    _popup.show();
                                                    var ruta = $('#SIM').data('url') + 'ControlVigilancia/Reposiciones/LeeDoc?CodTramite=' + CodTramite + '&CodDocumento=' + CodDocumento;
                                                    $("#DocumentoAdjunto").attr("src", ruta);
                                                }
                                            }).appendTo(container);
                                        }
                                    },
                                ],
                                onSelectionChanged: function (selectedItems) {
                                    var data = selectedItems.selectedRowsData[0];
                                    if (data) {
                                        IdDetReposicion = data.id;
                                    }
                                }
                            });

                            $("#PopupNuevoControl").css('visibility', 'visible');

                            var popupNewControl = $("#PopupNuevoControl").dxPopup({
                                width: 900,
                                height: "auto",
                                dragEnabled: true,
                                resizeEnabled: true,
                                hoverStateEnabled: true,
                                title: "Registro de Segimiento"
                            }).dxPopup("instance");

                            $("#btnnewControl").dxButton({
                                text: "Nuevo  Seguimiento",
                                type: "success",
                                height: 30,
                                width: 200,
                                onClick: function () {
                                    IdRegistro = -1;
                                    idControl = 0;

                                    txtAnioRadicado.reset();
                                    cboTecnicoSTN.reset();
                                    cboEstadoSTN.reset();
                                    txtRadicadoSTN.reset();
                                    txtTramiteSTN.reset();

                                    popupNewControl.show();
                                }
                            });

                            var popupA = $("#PupupActuaciones").dxPopup({
                                width: 1200,
                                height: 500,
                                dragEnabled: true,
                                position: "top",
                                resizeEnabled: true,
                                hoverStateEnabled: false,
                                title: "Seguimientos de control relacionados con el trámite"
                            }).dxPopup("instance");

                            $('#PupupActuaciones').css('visibility', 'visible');

                            popupA.show();

                        }
                    }).appendTo(container);
                }
            },
            {
                width: '5%',
                caption: "Documentos relacionados",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'event',
                        height: 20,
                        hint: 'Documentos',
                        onClick: function (e) {
                            IdReposicion = options.data.ID;
                            $("#GidListadoActos").dxDataGrid({
                                dataSource: ActosDataSource,
                                allowColumnResizing: true,
                                loadPanel: { enabled: true, text: 'Cargando Datos...' },
                                noDataText: "Sin datos para mostrar",
                                showBorders: true,
                                filterRow: {
                                    visible: true,
                                    emptyPanelText: 'Arrastre una columna para agrupar'
                                },
                                searchPanel: {
                                    visible: false,
                                    width: 240,
                                    placeholder: "Buscar..."
                                },
                                selection: {
                                    mode: 'single'
                                },
                                paging: {
                                    enabled: false,
                                    pageIndex: 0,
                                    pageSize: 20
                                },
                                hoverStateEnabled: true,
                                remoteOperations: false,
                                columns: [
                                    { dataField: 'Id', width: '5%', caption: 'Código', alignment: 'center' },
                                    { dataField: 'TramiteId', width: '5%', caption: 'Trámite SIM', dataType: 'string' },
                                    { dataField: 'TipoSolicitud', width: '25%', caption: 'Tipo', dataType: 'string' },
                                    { dataField: 'Radicado', width: '5%', caption: 'Radicado', dataType: 'string' },
                                    { dataField: 'Anio', width: '10%', caption: 'Fecha radicado', dataType: 'string' },
                                    { dataField: 'SubTipoSolicitud', width: '35%', caption: 'Descripción', dataType: 'string' },
                                    {
                                        width: '100px',
                                        caption: "Ver",
                                        alignment: 'center',
                                        cellTemplate: function (container, options) {
                                            $('<div/>').dxButton({
                                                icon: 'event',
                                                height: 20,
                                                hint: 'Ver documento',
                                                onClick: function (e) {
                                                    var CodTramite = options.row.data.TramiteId;
                                                    var CodDocumento = options.row.data.Id;
                                                    var _popup = $("#popDocumento").dxPopup("instance");
                                                    _popup.show();
                                                    var ruta = $('#SIM').data('url') + 'ControlVigilancia/Reposiciones/LeeDoc?CodTramite=' + CodTramite + '&CodDocumento=' + CodDocumento;
                                                    $("#DocumentoAdjunto").attr("src", ruta);
                                                }
                                            }).appendTo(container);
                                        }
                                    },
                                ],
                            });
                            var popupD = $("#PopupDocumentos").dxPopup({
                                width: 1500,
                                height: 900,
                                hoverStateEnabled: true,
                                dragEnabled: true,
                                resizeEnabled: true,
                                title: "Documentos asociados"
                            }).dxPopup("instance");

                            popupD.show();
                        }
                    }).appendTo(container);
                }
            }
        ],
        onExporting: function (e) {
            e.component.beginUpdate();
            e.component.columnOption('ANIO_ACTO', 'visible', true);
            e.component.columnOption('AUTORIZADO', 'visible', true);
            e.component.columnOption('CANTIDAD_DESTOCONADO', 'visible', true);
            e.component.columnOption('CANTIDAD_LEVANTAMIENTO_PISO', 'visible', true);
            e.component.columnOption('CANTIDAD_MANTENIMIENTO', 'visible', true);
            e.component.columnOption('CODIGO_ACTOADMINISTRATIVO', 'visible', true);
            e.component.columnOption('CODIGO_SOLICITUD', 'visible', true);
            e.component.columnOption('CONSERVACION_AUTORIZADO', 'visible', true);
            e.component.columnOption('CONSERVACION_EJECUTADA', 'visible', true);
            e.component.columnOption('CONSERVACION_SOLICITADO', 'visible', true);
            e.component.columnOption('COORDENADAX', 'visible', true);
            e.component.columnOption('COORDENADAY', 'visible', true);
            e.component.columnOption('DAP_MEN_10_AUTORIZADO', 'visible', true);
            e.component.columnOption('DAP_MEN_10_EJECUTADA', 'visible', true);
            e.component.columnOption('DAP_MEN_10_SOLICITADO', 'visible', true);
            e.component.columnOption('ID_USUARIOVISITA', 'visible', true);
            e.component.columnOption('INVERSION_MEDIDA_ADICIONAL_DESTOCONADO', 'visible', true);
            e.component.columnOption('INVERSION_MEDIDA_ADICIONAL_LEVANTAMIENTO_PISO', 'visible', true);
            e.component.columnOption('INVERSION_MEDIDA_ADICIONAL_MANTENIMIENTO', 'visible', true);
            e.component.columnOption('INVERSION_MEDIDA_ADICIONAL_SIEMBRA', 'visible', true);
            e.component.columnOption('INVERSION_MEDIDAS_ADICIONALES', 'visible', true);
            e.component.columnOption('INVERSION_REPOSICION_MINIMA', 'visible', true);
            e.component.columnOption('MEDIDA_ADICIONAL_ASIGNADA', 'visible', true);
            e.component.columnOption('MEDIDA_ADICIONAL_EJECUTADA', 'visible', true);
            e.component.columnOption('NOMBREPROYECTO', 'visible', true);
            e.component.columnOption('NRO_LENIOS_AUTORIZADOS', 'visible', true);
            e.component.columnOption('NRO_LENIOS_SOLICITADOS', 'visible', true);
            e.component.columnOption('NUMERO_ACTO', 'visible', true);
            e.component.columnOption('OBSERVACIONES', 'visible', true);
            e.component.columnOption('OBSERVACIONVISITA', 'visible', true);
            e.component.columnOption('PAGO_FONDO_VERDE_METROPOLITANO', 'visible', true);
            e.component.columnOption('PODA_AUTORIZADO', 'visible', true);
            e.component.columnOption('PODA_EJECUTADA', 'visible', true);
            e.component.columnOption('PODA_SOLICITADO', 'visible', true);
            e.component.columnOption('RADICADOVISITA', 'visible', true);
            e.component.columnOption('REPOSICION_AUTORIZADO', 'visible', true);
            e.component.columnOption('REPOSICION_EJECUTADA', 'visible', true);
            e.component.columnOption('REPOSICION_MINIMA_OBLIGATORIA', 'visible', true);
            e.component.columnOption('REPOSICION_PROPUESTA', 'visible', true);
            e.component.columnOption('TALA_AUTORIZADO', 'visible', true);
            e.component.columnOption('TALA_EJECUTADA', 'visible', true);
            e.component.columnOption('TALA_SOLICITADA', 'visible', true);
            e.component.columnOption('TIPO_DOCUMENTO', 'visible', true);
            e.component.columnOption('TIPOMEDIDAADICIONAL', 'visible', true);
            e.component.columnOption('TRASPLANTE_AUTORIZADO', 'visible', true);
            e.component.columnOption('TRASPLANTE_EJECUTADO', 'visible', true);
            e.component.columnOption('TRASPLANTE_SOLICITADO', 'visible', true);
            e.component.columnOption('VALORACION_INVENTARIO_FORESTAL', 'visible', true);
            e.component.columnOption('VALORACION_TALA', 'visible', true);
            e.component.columnOption('VOLUMEN_AUTORIZADO', 'visible', true);
            e.component.columnOption('VOLUMEN_EJECUTADO', 'visible', true);
            e.component.columnOption('DIRECCION', 'visible', true);
            e.component.columnOption('ENTIDAD_PUBLICA', 'visible', true);
        },
        onExported: function (e) {
            e.component.columnOption('ANIO_ACTO', 'visible', false);
            e.component.columnOption('AUTORIZADO', 'visible', false);
            e.component.columnOption('CANTIDAD_DESTOCONADO', 'visible', false);
            e.component.columnOption('CANTIDAD_LEVANTAMIENTO_PISO', 'visible', false);
            e.component.columnOption('CANTIDAD_MANTENIMIENTO', 'visible', false);
            e.component.columnOption('CODIGO_ACTOADMINISTRATIVO', 'visible', false);
            e.component.columnOption('CODIGO_SOLICITUD', 'visible', false);
            e.component.columnOption('CONSERVACION_AUTORIZADO', 'visible', false);
            e.component.columnOption('CONSERVACION_EJECUTADA', 'visible', false);
            e.component.columnOption('CONSERVACION_SOLICITADO', 'visible', false);
            e.component.columnOption('COORDENADAX', 'visible', false);
            e.component.columnOption('COORDENADAY', 'visible', false);
            e.component.columnOption('DAP_MEN_10_AUTORIZADO', 'visible', false);
            e.component.columnOption('DAP_MEN_10_EJECUTADA', 'visible', false);
            e.component.columnOption('DAP_MEN_10_SOLICITADO', 'visible', false);
            e.component.columnOption('ID_USUARIOVISITA', 'visible', false);
            e.component.columnOption('INVERSION_MEDIDA_ADICIONAL_DESTOCONADO', 'visible', false);
            e.component.columnOption('INVERSION_MEDIDA_ADICIONAL_LEVANTAMIENTO_PISO', 'visible', false);
            e.component.columnOption('INVERSION_MEDIDA_ADICIONAL_MANTENIMIENTO', 'visible', false);
            e.component.columnOption('INVERSION_MEDIDA_ADICIONAL_SIEMBRA', 'visible', false);
            e.component.columnOption('INVERSION_MEDIDAS_ADICIONALES', 'visible', false);
            e.component.columnOption('INVERSION_REPOSICION_MINIMA', 'visible', false);
            e.component.columnOption('MEDIDA_ADICIONAL_ASIGNADA', 'visible', false);
            e.component.columnOption('MEDIDA_ADICIONAL_EJECUTADA', 'visible', false);
            e.component.columnOption('NOMBREPROYECTO', 'visible', false);
            e.component.columnOption('NRO_LENIOS_AUTORIZADOS', 'visible', false);
            e.component.columnOption('NRO_LENIOS_SOLICITADOS', 'visible', false);
            e.component.columnOption('NUMERO_ACTO', 'visible', false);
            e.component.columnOption('OBSERVACIONES', 'visible', false);
            e.component.columnOption('OBSERVACIONVISITA', 'visible', false);
            e.component.columnOption('PAGO_FONDO_VERDE_METROPOLITANO', 'visible', false);
            e.component.columnOption('PODA_AUTORIZADO', 'visible', false);
            e.component.columnOption('PODA_EJECUTADA', 'visible', false);
            e.component.columnOption('PODA_SOLICITADO', 'visible', false);
            e.component.columnOption('RADICADOVISITA', 'visible', false);
            e.component.columnOption('REPOSICION_AUTORIZADO', 'visible', false);
            e.component.columnOption('REPOSICION_EJECUTADA', 'visible', false);
            e.component.columnOption('REPOSICION_MINIMA_OBLIGATORIA', 'visible', false);
            e.component.columnOption('REPOSICION_PROPUESTA', 'visible', false);
            e.component.columnOption('TALA_AUTORIZADO', 'visible', false);
            e.component.columnOption('TALA_EJECUTADA', 'visible', false);
            e.component.columnOption('TALA_SOLICITADA', 'visible', false);
            e.component.columnOption('TIPO_DOCUMENTO', 'visible', false);
            e.component.columnOption('TIPOMEDIDAADICIONAL', 'visible', false);
            e.component.columnOption('TRASPLANTE_AUTORIZADO', 'visible', false);
            e.component.columnOption('TRASPLANTE_EJECUTADO', 'visible', false);
            e.component.columnOption('TRASPLANTE_SOLICITADO', 'visible', false);
            e.component.columnOption('VALORACION_INVENTARIO_FORESTAL', 'visible', false);
            e.component.columnOption('VALORACION_TALA', 'visible', false);
            e.component.columnOption('VOLUMEN_AUTORIZADO', 'visible', false);
            e.component.columnOption('VOLUMEN_EJECUTADO', 'visible', false);
            e.component.columnOption('DIRECCION', 'visible', false);
            e.component.columnOption('ENTIDAD_PUBLICA', 'visible', false);
            e.component.endUpdate();
        },
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                IdReposicion = data.ID;
            }
        }
    });

    $("#popDocumento").dxPopup({
        width: 900,
        height: 800,
        resizeEnabled: true,
        showTitle: true,
        title: "Visualizar Documento",
        dragEnabled: true,
        closeOnOutsideClick: true,

    });

    var txtCM = $("#txtCM").dxTextBox({
        placeholder: "Ingrese el CM...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El CM es requerido!"
        }]
    }).dxTextBox("instance");

    var txtTramiteSTN = $("#txtTramiteSTN").dxTextBox({
        placeholder: "Ingrese el número del trámite SIM...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El  número del trámite SIM es requerido!"
        }]
    }).dxTextBox("instance");

    var txtAsunto = $("#txtAsunto").dxTextArea({
        value: "",
        readOnly: true,
        height: 30
    }).dxTextArea("instance");

    var txtProyecto = $("#txtProyecto").dxTextArea({
        value: "",
        readOnly: true,
        height: 30
    }).dxTextArea("instance");

    var txtNombreProyectoA = $("#txtNombreProyectoA").dxTextArea({
        value: "",
        readOnly: false,
        height: 40
    }).dxTextArea("instance");


    var popExp = $("#popExportar").dxPopup({
        height: 600,
        width: 1100,
        resizeEnabled: true,
        title: 'Tablero  de Control',
        visible: false,
        contentTemplate: function (container) {
            $("<iframe>").attr("src", "https://app.powerbi.com/view?r=eyJrIjoiNmU4YTc0MTAtOWNiOC00NTg5LTgyNGItNTRmZDI5ZGVhYmRjIiwidCI6IjRkZWI0ZjAwLTNhOTgtNDcwMi04Nzk2LTIxNmRiMDljMzA3YyIsImMiOjR9").attr("width", "100%").attr("height", "100%").attr("frameborder", "0").attr("scrolling", "0").appendTo(container);
        }
    }).dxPopup("instance");




    var cboTecnico = $("#cboTecnico").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "ControlVigilancia/api/TramitesNuevosApi/GetTecnicos");
                }
            })
        }),
        displayExpr: "Nombre",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El Técnico es Obligatorio!"
        }]
    }).dxSelectBox("instance");

    var cboTecnicoSTN = $("#cboTecnicoSTN").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "ControlVigilancia/api/TramitesNuevosApi/GetTecnicos");
                }
            })
        }),
        displayExpr: "Nombre",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El Técnico es Obligatorio!"
        }]
    }).dxSelectBox("instance");

    cboEstadoSTN = $("#cboEstadoSTN").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "ControlVigilancia/api/TramitesNuevosApi/GetEstados");
                }
            })
        }),
        displayExpr: "estado",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El Técnico es Obligatorio!"
        }]
    }).dxSelectBox("instance");

    var txtTalaSol = $("#txtTalaSol").dxNumberBox({
        value: "0",
        format: "#,##0",
    }).dxNumberBox("instance");

    var txtdAPMen10Solicitada = $("#txtdAPMen10Solicitada").dxNumberBox({
        value: "0",
        format: "#,##0",
    }).dxNumberBox("instance");

    var txtNroLeniososAut = $("#txtNroLeniososAut").dxNumberBox({
        value: "0",
        format: "#,##0",

    }).dxNumberBox("instance");

    var txttransplanteSolicitado = $("#txttransplanteSolicitado").dxNumberBox({
        value: "0",
        format: "#,##0",
    }).dxNumberBox("instance");

    var txtpodaSolicitada = $("#txtpodaSolicitada").dxNumberBox({
        value: "0",
        format: "#,##0",
    }).dxNumberBox("instance");

    var txtconservacionSolicitada = $("#txtconservacionSolicitada").dxNumberBox({
        value: "0",
        format: "#,##0",
    }).dxNumberBox("instance");

    var txtreposicionPropuesta = $("#txtreposicionPropuesta").dxNumberBox({
        value: "0",
        format: "#,##0",
    }).dxNumberBox("instance");

    var txtreposicionMinimaObligatoria = $("#txtreposicionMinimaObligatoria").dxNumberBox({
        value: "0",
        format: "#,##0",
    }).dxNumberBox("instance");

    var txtTalaAut = $("#txtTalaAut").dxNumberBox({
        value: "0",
        format: "#,##0",

    }).dxNumberBox("instance");

    var txtTalaEje = $("#txtTalaEje").dxNumberBox({
        value: "0",
        format: "#,##0",
    }).dxNumberBox("instance");

    var txtdAPMen10Ejecutada = $("#txtdAPMen10Ejecutada").dxNumberBox({
        value: "0",
        format: "#,##0",
    }).dxNumberBox("instance");
    var txttransplanteEjecutado = $("#txttransplanteEjecutado").dxNumberBox({
        value: "0",
        format: "#,##0",
    }).dxNumberBox("instance");
    var txtpodaEjecutada = $("#txtpodaEjecutada").dxNumberBox({
        value: "0",
        format: "#,##0",
    }).dxNumberBox("instance");
    var txtconservacionEjecutada = $("#txtconservacionEjecutada").dxNumberBox({
        value: "0",
        format: "#,##0",
    }).dxNumberBox("instance");
    var txtMedidaAdicionalEjecutado = $("#txtMedidaAdicionalEjecutado").dxNumberBox({
        value: "0",
        format: "#,##0",
    }).dxNumberBox("instance");
    var txtvolumenEjecutado = $("#txtvolumenEjecutado").dxNumberBox({
        value: "0",
        format: "#.#######0",
    }).dxNumberBox("instance");
    var txtFechaControl = $("#txtFechaControl").dxDateBox({
        type: "date",
        value: new Date(),
        showAnalogClock: false,
        onOpened: function (args) {
            let position = args.component._popup.option("position");
            position.my = "center";
            position.at = "left";
            args.component._popup.option("position", position);
        }
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "La fecha del control es obligatoria"
        }]
    }).dxDateBox("instance");
    var txtRadicadoSTN = $("#txtRadicadoSTN").dxTextBox({
        placeholder: "Ingrese el número del Radicado",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        readOnly: true,
        validationRules: [{
            type: "required",
            message: "El número del radicado es requerido!"
        }]
    }).dxTextBox("instance");

    var txtAnioRadicado = $("#txtAnioRadicado").dxNumberBox({
        readOnly: false,
        value: "0",
        format: "###0",
    }).dxNumberBox("instance");


    var txtRadicadoDocu = $("#txtRadicadoDocu").dxNumberBox({
        value: "",
        readOnly: false
    }).dxNumberBox("instance");

    var txtAnioDocu = $("#txtAnioDocu").dxNumberBox({
        value: "",
        readOnly: false
    }).dxNumberBox("instance");



    var txtdAPMen10Autorizada = $("#txtdAPMen10Autorizada").dxNumberBox({
        value: "0",
        format: "#,##0",

    }).dxNumberBox("instance");
    var txtvolumenAutorizado = $("#txtvolumenAutorizado").dxNumberBox({
        value: "0",
        format: "#.#######0",
    }).dxNumberBox("instance");
    var txttransplanteAutorizado = $("#txttransplanteAutorizado").dxNumberBox({
        value: "0",
        format: "#,##0",
    }).dxNumberBox("instance");
    var txtpodaAutorizada = $("#txtpodaAutorizada").dxNumberBox({
        value: "0",
        format: "#,##0",
    }).dxNumberBox("instance");
    var txtconservacionAutorizada = $("#txtconservacionAutorizada").dxNumberBox({
        value: "0",
        format: "#,##0",
    }).dxNumberBox("instance");
    var txtreposicionAutorizada = $("#txtreposicionAutorizada").dxNumberBox({
        value: "0",
        format: "#,##0",
    }).dxNumberBox("instance");
    var txtAutorizado = $("#txtAutorizado").dxNumberBox({
        value: "0",
        format: "#,##0",
    }).dxNumberBox("instance");
    var txtObservaciones = $("#txtObservaciones").dxTextArea({
        value: "",
        height: 60
    }).dxTextArea("instance");
    var txtCMv = $("#txtCMv").dxTextBox({
        value: "",
        readOnly: true
    }).dxTextBox("instance");
    var txtNombreProyecto = $("#txtNombreProyecto").dxTextBox({
        value: "",
        readOnly: true
    }).dxTextBox("instance");
    var txtDireccionProyecto = $("#txtDireccionProyecto").dxTextBox({
        value: "",
        readOnly: true
    }).dxTextBox("instance");
    var txtNroLeniososSol = $("#txtNroLeniososSol").dxNumberBox({
        value: "0",
        format: "#,##0",

    }).dxNumberBox("instance");
    var txtValoracionInventarioForestal = $("#txtValoracionInventarioForestal").dxNumberBox({
        value: "0",
        format: "#,##0",

    }).dxNumberBox("instance");
    var txtValoracionTala = $("#txtValoracionTala").dxNumberBox({
        value: "0",
        format: "#,##0",

    }).dxNumberBox("instance");
    var txtInversionReposicionMinima = $("#txtInversionReposicionMinima").dxNumberBox({
        value: "0",
        format: "#,##0",

    }).dxNumberBox("instance");
    var txtInversionMedidasAdicionales = $("#txtInversionMedidasAdicionales").dxNumberBox({
        value: "0",
        format: "#,##0",

    }).dxNumberBox("instance");
    var txtCantidadSiembraAdicional = $("#txtCantidadSiembraAdicional").dxNumberBox({
        value: "0",
        format: "#,##0",

    }).dxNumberBox("instance");
    var txtInversionMedidaAdicionalSiembra = $("#txtInversionMedidaAdicionalSiembra").dxNumberBox({
        value: "0",
        format: "#,##0",

    }).dxNumberBox("instance");
    var txtCantidadMantenimiento = $("#txtCantidadMantenimiento").dxNumberBox({
        value: "0",
        format: "#,##0",

    }).dxNumberBox("instance");
    var txtInversionMedidaAdicionalMantenimiento = $("#txtInversionMedidaAdicionalMantenimiento").dxNumberBox({
        value: "0",
        format: "#,##0",

    }).dxNumberBox("instance");
    var txtCantidadDestoconado = $("#txtCantidadDestoconado").dxNumberBox({
        value: "0",
        format: "#,##0",

    }).dxNumberBox("instance");
    var txtInversionMedidaAdicionalDestoconado = $("#txtInversionMedidaAdicionalDestoconado").dxNumberBox({
        value: "0",
        format: "#,##0",

    }).dxNumberBox("instance");
    var txtCantidadLevantamientoPiso = $("#txtCantidadLevantamientoPiso").dxNumberBox({
        value: "0",
        format: "#,##0",

    }).dxNumberBox("instance");
    var txtInversionMedidaAdicionalLevantamientoPiso = $("#txtInversionMedidaAdicionalLevantamientoPiso").dxNumberBox({
        value: "0",
        format: "#,##0",

    }).dxNumberBox("instance");
    var txtPagoFondoVerde = $("#txtPagoFondoVerde").dxNumberBox({
        value: "0",
        format: "#,##0",

    }).dxNumberBox("instance");
    var txtCoordenadaX = $("#txtCoordenadaX").dxNumberBox({
        value: "",
    }).dxNumberBox("instance");
    var txtCoordenadaY = $("#txtCoordenadaY").dxNumberBox({
        value: "",
    }).dxNumberBox("instance");
    const dataini = [{
        id: 0,
        nombre: '...'
    }]
    const dt = new DevExpress.data.ArrayStore({
        data: dataini,
        key: "id"
    });
    var cboAsuntos = $("#cboAsuntos").dxSelectBox({
        dataSource: dt,
        displayExpr: "Descripcion",
        valueExpr: 'Id',
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe seleccionar el Asunto!"
        }]
    }).dxSelectBox("instance");


    var cboAsuntosTN = $("#cboAsuntosTN").dxSelectBox({
        dataSource: dt,
        displayExpr: "Descripcion",
        valueExpr: 'Id',
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe seleccionar el Asunto!"
        }]
    }).dxSelectBox("instance");

    const loadIndicator = $('#PopupLoadNuevoControl').dxLoadIndicator({
        height: 40,
        width: 40,
        visible: false
    }).dxLoadIndicator("instance");


    $("#cmdCM").dxButton({
        text: "Verificar",
        type: "danger",
        height: 35,
        onClick: function () {
            var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/TramitesNuevosApi/ObtenerCM";
            $.getJSON(_Ruta,
                {
                    cm: txtCM.option('value')
                }).done(function (data) {
                    txtCMv.option("value", txtCM.option('value'));
                    txtNombreProyecto.option("value", data.nombre);
                    txtProyecto.option("value", data.nombre + ' ' + data.direccion);
                    $("#btnBuscarActosCMs").dxButton({
                        text: "Buscar ...",
                        type: "default",
                        height: 30,
                        onClick: function () {
                            var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/TramitesNuevosApi/ObtenerActuacionesCM";
                            $.getJSON(_Ruta,
                                {
                                    radicado: txtRadicadoDocu.option('value'), anio: txtAnioDocu.option('value'), cm: txtCM.option('value')
                                }).done(function (data) {
                                    txtCMv.option("value", txtCM.option('value'));
                                    txtNombreProyecto.option("value", data.nombre);
                                    txtProyecto.option("value", data.nombre + ' ' + data.direccion);
                                    $("#cboAsuntos").dxSelectBox({
                                        dataSource: new DevExpress.data.ArrayStore({
                                            data: data.asuntos,
                                            key: "id"
                                        }),
                                        displayExpr: "Descripcion",
                                        valueExpr: 'Id',
                                        searchEnabled: true
                                    }).dxValidator({
                                        validationGroup: "ProcesoGroup",
                                        validationRules: [{
                                            type: "required",
                                            message: "Debe seleccionar el Asunto!"
                                        }]
                                    });
                                    pupupCM.show();
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                                });
                        }
                    });
                    $("#cboAsuntos").dxSelectBox({
                        dataSource: new DevExpress.data.ArrayStore({
                            data: data.asuntos,
                            key: "Id"
                        }),
                        displayExpr: "Descripcion",
                        valueExpr: 'Id',
                        searchEnabled: true
                    }).dxValidator({
                        validationGroup: "ProcesoGroup",
                        validationRules: [{
                            type: "required",
                            message: "Debe seleccionar el Asunto!"
                        }]
                    });
                    pupupCM.show();
                }).fail(function (jqxhr, textStatus, error) {
                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                });


            $.getJSON(_Ruta,
                {
                    cm: txtCM.option('value')
                }).done(function (data) {
                    txtCMv.option("value", txtCM.option('value'));
                    txtNombreProyecto.option("value", data.nombre);
                    txtDireccionProyecto.option("value", data.direccion);
                    $("#btnBuscarActosCMs").dxButton({
                        text: "Buscar ...",
                        type: "default",
                        height: 30,
                        onClick: function () {
                            $("#btnBuscarActosCMs").dxButton("instance").option("icon", "clock");
                            $("#btnBuscarActosCMs").dxButton("instance").option("disabled", true);
                            loadIndicator.option("visible", true);
                            var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/TramitesNuevosApi/ObtenerActuacionesCM";
                            $.getJSON(_Ruta,
                                {
                                    radicado: txtRadicadoDocu.option('value'), anio: txtAnioDocu.option('value'), cm: txtCM.option('value')
                                }).done(function (data) {
                                    $("#btnBuscarActosCMs").dxButton("instance").option("disabled", false);
                                    $("#btnBuscarActosCMs").dxButton("instance").option("icon", "");
                                    loadIndicator.option("visible", false);
                                    txtCMv.option("value", txtCM.option('value'));
                                    txtNombreProyecto.option("value", data.nombre);
                                    txtProyecto.option("value", data.nombre + ' ' + data.direccion);
                                    $("#cboAsuntos").dxSelectBox({
                                        dataSource: new DevExpress.data.ArrayStore({
                                            data: data.asuntos,
                                            key: "Id"
                                        }),
                                        displayExpr: "Descripcion",
                                        valueExpr: 'Id',
                                        searchEnabled: true
                                    }).dxValidator({
                                        validationGroup: "ProcesoGroup",
                                        validationRules: [{
                                            type: "required",
                                            message: "Debe seleccionar el Asunto!"
                                        }]
                                    });
                                    pupupCM.show();
                                }).fail(function (jqxhr, textStatus, error) {

                                    $("#btnBuscarActosCMs").dxButton("instance").option("disabled", false);
                                    $("#btnBuscarActosCMs").dxButton("instance").option("icon", "");
                                    loadIndicator.option("visible", false);
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                                });
                        }
                    })
                    $("#cboAsuntos").dxSelectBox({
                        dataSource: new DevExpress.data.ArrayStore({
                            data: data.asuntos,
                            key: "Id"
                        }),
                        displayExpr: "Descripcion",
                        valueExpr: 'Id',
                        searchEnabled: true
                    }).dxValidator({
                        validationGroup: "ProcesoGroup",
                        validationRules: [{
                            type: "required",
                            message: "Debe seleccionar el Asunto!"
                        }]
                    });
                    pupupCM.show();
                }).fail(function (jqxhr, textStatus, error) {
                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                });

        }
    });

    $("#cmdBuscarInfTec").dxButton({
        text: "...",
        type: "success",
        height: 35,
        onClick: function () {
            var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/TramitesNuevosApi/ObtenerInformeTecnico";
            $.getJSON(_Ruta,
                {
                    tramite: txtTramiteSTN.option('value')
                }).done(function (data) {
                    $("#cboAsuntosTN").dxSelectBox({
                        dataSource: new DevExpress.data.ArrayStore({
                            data: data,
                            key: "Id"
                        }),
                        displayExpr: "Descripcion",
                        valueExpr: 'Id',
                        searchEnabled: true
                    }).dxValidator({
                        validationGroup: "ProcesoGroup",
                        validationRules: [{
                            type: "required",
                            message: "Debe seleccionar el Asunto!"
                        }]
                    });
                    PupupDocumentosTramite.show();
                }).fail(function (jqxhr, textStatus, error) {
                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                });

            $.getJSON(_Ruta,
                {
                    tramite: txtTramiteSTN.option('value')
                }).done(function (data) {
                    $("#cboAsuntosTN").dxSelectBox({
                        dataSource: new DevExpress.data.ArrayStore({
                            data: data,
                            key: "Id"
                        }),
                        displayExpr: "Descripcion",
                        valueExpr: 'Id',
                        searchEnabled: true
                    }).dxValidator({
                        validationGroup: "ProcesoGroup",
                        validationRules: [{
                            type: "required",
                            message: "Debe seleccionar el Asunto!"
                        }]
                    });
                    PupupDocumentosTramite.show();
                }).fail(function (jqxhr, textStatus, error) {
                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                });


        }
    });

    $("#btnGuarda").dxButton({
        text: "Guardar",
        type: "default",
        height: 30,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var id = IdRegistro;
            var cm = txtCM.option("value");
            var asunto = txtAsunto.option("value");
            var proyecto = txtProyecto.option("value");
            var nombreProyecto = txtNombreProyectoA.option("value");
            var codigoSolicitud = idAsunto;
            var codigoActoAdministrativo = 0;
            var talaAutorizada = txtTalaAut.option("value");
            var dAPMen10Autorizada = txtdAPMen10Autorizada.option("value");
            var volumenAutorizado = txtvolumenAutorizado.option("value");
            var transplanteAutorizado = txttransplanteAutorizado.option("value");
            var podaAutorizada = txtpodaAutorizada.option("value");
            var conservacionAutorizada = txtconservacionAutorizada.option("value");
            var observaciones = txtObservaciones.option("value");

            var nroLeniosSolicitados = txtNroLeniososSol.option("value");
            var valoracionInventarioForestal = txtValoracionInventarioForestal.option("value");
            var valoracionTala = txtValoracionTala.option("value");
            var inversionReposicionMinima = txtInversionReposicionMinima.option("value");
            var inversionMedidasAdicionales = txtInversionMedidasAdicionales.option("value");
            var cantidadSiembraAdicional = txtCantidadSiembraAdicional.option("value");
            var inversionMedidaAdicionalSiembra = txtInversionMedidaAdicionalSiembra.option("value");
            var cantidadMantenimiento = txtCantidadMantenimiento.option("value");
            var inversionMedidaAdicionalMantenimiento = txtInversionMedidaAdicionalMantenimiento.option("value");
            var cantidadDestoconado = txtCantidadDestoconado.option("value");
            var inversionMedidaAdicionalDestoconado = txtInversionMedidaAdicionalDestoconado.option("value");
            var cantidadLevantamientoPiso = txtCantidadLevantamientoPiso.option("value");
            var inversionMedidaAdicionalLevantamientoPiso = txtInversionMedidaAdicionalLevantamientoPiso.option("value");
            var pagoFondoVerdeMetropolitano = txtPagoFondoVerde.option("value");


            var talaSolicitada = txtTalaSol.option("value");
            var dAPMen10Solicitada = txtdAPMen10Solicitada.option("value");
            var nroLeniosAutorizados = txtNroLeniososAut.option("value");
            var transplanteSolicitado = txttransplanteSolicitado.option("value");
            var conservacionSolicitada = txtconservacionSolicitada.option("value");
            var reposicionPropuesta = txtreposicionPropuesta.option("value");
            var reposicionMinimaObligatoria = txtreposicionMinimaObligatoria.option("value");


            var coordenadaX = txtCoordenadaX.option("value");
            var coordenadaY = txtCoordenadaY.option("value");

            var params = {
                id: id, cm: cm, asunto: asunto, proyecto: proyecto, codigoSolicitud: codigoSolicitud, codigoActoAdministrativo: codigoActoAdministrativo,
                talaAutorizada: talaAutorizada, talaSolicitada: talaSolicitada, dAPMen10Solicitada: dAPMen10Solicitada, dAPMen10Autorizada: dAPMen10Autorizada,
                volumenAutorizado: volumenAutorizado, transplanteSolicitado: transplanteSolicitado, transplanteAutorizado: transplanteAutorizado,
                podaAutorizada: podaAutorizada, conservacionAutorizada: conservacionAutorizada,
                reposicionPropuesta: reposicionPropuesta, reposicionMinimaObligatoria: reposicionMinimaObligatoria,
                observaciones: observaciones, coordenadaX: coordenadaX, coordenadaY: coordenadaY,
                nroLeniosSolicitados: nroLeniosSolicitados, nroLeniosAutorizados: nroLeniosAutorizados, valoracionInventarioForestal: valoracionInventarioForestal,
                valoracionTala: valoracionTala, inversionReposicionMinima: inversionReposicionMinima, conservacionSolicitada: conservacionSolicitada,
                inversionMedidasAdicionales: inversionMedidasAdicionales, cantidadSiembraAdicional: cantidadSiembraAdicional,
                inversionMedidaAdicionalSiembra: inversionMedidaAdicionalSiembra, cantidadMantenimiento: cantidadMantenimiento,
                inversionMedidaAdicionalMantenimiento: inversionMedidaAdicionalMantenimiento, cantidadDestoconado: cantidadDestoconado,
                inversionMedidaAdicionalDestoconado: inversionMedidaAdicionalDestoconado, cantidadLevantamientoPiso: cantidadLevantamientoPiso,
                inversionMedidaAdicionalLevantamientoPiso: inversionMedidaAdicionalLevantamientoPiso, pagoFondoVerdeMetropolitano: pagoFondoVerdeMetropolitano,
                nombreProyecto: nombreProyecto
            };

            var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/TramitesNuevosApi/GuardarReposicion";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                beforeSend: function () { },
                success: function (data) {
                    if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Guardar Datos');
                    else {
                        DevExpress.ui.dialog.alert('Datos Guardados correctamente', 'Guardar Datos');
                        $('#GidListado').dxDataGrid({ dataSource: ReposicionesDataSource });
                        $("#PopupNuevaRepo").dxPopup("instance").hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    $("#btnGuardarNewControl").dxButton({
        text: "Guardar",
        type: "default",
        height: 30,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var reposicionId = IdReposicion;
            var tipoDocumentoId = 1;
            var anioRadicado = txtAnioRadicado.option("value");
            var tecnico = cboTecnicoSTN.option("value").Nombre;
            var estado = cboEstadoSTN.option("value").id;
            var radicado = txtRadicadoSTN.option("value");
            var tramiteId = txtTramiteSTN.option("value");
            var params = {
                id: idControl, reposicionId: reposicionId, anioRadicado: anioRadicado, tecnico: tecnico, estado: estado, radicado: radicado, tramiteId: tramiteId
            };
            var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/TramitesNuevosApi/GuardarControl";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                beforeSend: function () { },
                success: function (data) {
                    if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Guardar Datos');
                    else {
                        DevExpress.ui.dialog.alert('Datos Guardados correctamente', 'Guardar Datos');
                        $('#GridListadoControl').dxDataGrid({ dataSource: ControlDataSource });
                        $("#PopupNuevoControl").dxPopup("instance").hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    $("#btnVerActoPpal").dxButton({
        icon: 'exportpdf',
        hint: 'Ver documento ...',
        type: "default",
        height: 35,
        width: 60,
        onClick: function () {

            var asunto = txtAsunto.option("value");
            var _Ruta = $('#SIM').data('url') + "ControlVigilancia/api/TramitesNuevosApi/ObtenerTramiteDocumento";
            $.getJSON(_Ruta,
                {
                    id: asunto
                }).done(function (data) {
                    if (data != null && data.idTramite !== 0 && data.idDocumento !== 0) {
                        var CodTramite = data.idTramite;
                        var CodDocumento = data.idDocumento;
                        var _popup = $("#popDocumento").dxPopup("instance");
                        _popup.show();

                        var ruta = $('#SIM').data('url') + 'ControlVigilancia/Reposiciones/LeeDoc?CodTramite=' + CodTramite + '&CodDocumento=' + CodDocumento;
                        $("#DocumentoAdjunto").attr("src", ruta);
                    } else {
                        alert('No se encuentra seleccionado el documento!');
                    }
                }).fail(function (jqxhr, textStatus, error) {
                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                });
        }
    });

    var popup = $("#PopupNuevaRepo").dxPopup({
        width: 1100,
        height: "auto",
        dragEnabled: true,
        resizeEnabled: true,
        hoverStateEnabled: true,
        title: "Permiso y CM asociado al proceso de Control y Vigilancia"
    }).dxPopup("instance");

    $("#btnNuevo").dxButton({
        stylingMode: "contained",
        text: "Nuevo",
        type: "success",
        width: 200,
        height: 30,
        icon: 'add',
        onClick: function () {
            IdRegistro = -1;
            txtCM.reset();
            txtAsunto.reset();
            txtTalaAut.reset();
            txtdAPMen10Autorizada.reset();
            txtvolumenAutorizado.reset();
            txttransplanteAutorizado.reset();
            txtpodaAutorizada.reset();
            txtconservacionAutorizada.reset();
            txtTalaSol.reset();
            txtdAPMen10Solicitada.reset();
            txtNroLeniososAut.reset();
            txttransplanteSolicitado.reset();
            txtpodaSolicitada.reset();
            txtconservacionSolicitada.reset();
            txtreposicionPropuesta.reset();
            txtreposicionMinimaObligatoria.reset();
            txtObservaciones.reset();
            txtNroLeniososSol.reset();
            txtValoracionInventarioForestal.reset();
            txtValoracionTala.reset();
            txtInversionReposicionMinima.reset();
            txtInversionMedidasAdicionales.reset();
            txtCantidadSiembraAdicional.reset();
            txtInversionMedidaAdicionalSiembra.reset();
            txtCantidadMantenimiento.reset();
            txtInversionMedidaAdicionalMantenimiento.reset();
            txtCantidadDestoconado.reset();
            txtInversionMedidaAdicionalDestoconado.reset();
            txtCantidadLevantamientoPiso.reset();
            txtInversionMedidaAdicionalLevantamientoPiso.reset();
            txtPagoFondoVerde.reset();
            txtCoordenadaX.reset();
            txtCoordenadaY.reset();
            txtNombreProyectoA.reset();
            popup.show();
        }
    });

    $("#btnAsignar").dxButton({
        text: "Asignar",
        type: "default",
        height: 30,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            txtProyecto.option("value", txtNombreProyecto.option("value"));
            txtAsunto.option("value", cboAsuntos._options.text);
            idAsunto = cboAsuntos._options.value;
            $("#PupupCM").dxPopup("instance").hide();
        }
    });

    $("#btnAsignarTN").dxButton({
        text: "Asignar",
        type: "default",
        height: 30,
        onClick: function () {
            const docsel = cboAsuntosTN._options.text;
            let arr = docsel.split('-');
            const radicadoDoc = arr[0].split(':')[1] + '-' + arr[1];
            const anioDoc = arr[4];

            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            txtRadicadoSTN.option("value", radicadoDoc);
            txtAnioRadicado.option("value", anioDoc);
            idAsunto = cboAsuntos._options.value;

            $("#PupupDocumentosTramite").dxPopup("instance").hide();
        }
    });

    cboAsuntos._options.value

    var pupupCM = $("#PupupCM").dxPopup({
        width: 1300,
        height: "auto",
        hoverStateEnabled: true,
        title: "Información del CM"
    }).dxPopup("instance");


    $("#btnReporte").dxButton({
        text: "Reporte",
        stylingMode: "contained",
        type: "default",
        height: 30,
        onClick: function () {
            popupReporte.show();
            let url = $('#SIM').data('url') + 'ControlVigilancia/TramiteSeguimiento/GetReporte';
            $('#iframeReporte').attr('src', url);
        }
    });

    var popupReporte = $("#popupReporte").dxPopup({
        width: 1200,
        height: 600,
        hoverStateEnabled: true,
        title: "Reporte"
    }).dxPopup("instance");

    var PupupDocumentosTramite = $("#PupupDocumentosTramite").dxPopup({
        width: 1000,
        height: "auto",
        hoverStateEnabled: true,
        title: "Documentos asociados al trámite"
    }).dxPopup("instance");

});

var ReposicionesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"CODIGO_SOLICITUD","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'ControlVigilancia/api/TramitesNuevosApi/GetReposiciones', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: false,
            CodFuncionario: CodigoFuncionario
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var ActosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        $.getJSON($('#SIM').data('url') + 'ControlVigilancia/api/TramitesNuevosApi/ObtenerActos', {
            Id: IdReposicion
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

var ControlDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        $.getJSON($('#SIM').data('url') + 'ControlVigilancia/api/TramitesNuevosApi/ObtenerControles', {
            Id: IdReposicion
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});
