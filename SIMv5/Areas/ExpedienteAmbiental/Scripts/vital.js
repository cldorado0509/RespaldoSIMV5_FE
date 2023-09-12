var id;
var idTercero;
var idInstalacion;
var idPuntoControl = 0;
var _numeroVITAL = "";
var _numeroVITALAsociado = "";
var idSolicitudVITAL = 0;
var _radicadoVITAL = "";
var _procesoId = 0;
var _tareaId = 0;
var idExpedienteDoc;
var idEstadoPuntoControl = 0;
var idAnotacionPuntoControl = 0;
var NomExpediente = "";
var _nombreArchivo = "";
var IdIUnidadDoc = 0;
var IdTomo = 0;
var codFuncionarioSim = $('#SIM').data('codfuncionario')

$(document).ready(function () {

   
    $('#asistente').accordion({
        collapsible: true,
        animationDuration: 500,
        multiple : false
    });

    $('#asistente').css('visibility', 'hidden');

    $("#GridListado").dxDataGrid({
        dataSource: SolicitudesVitalDataSource,
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
            visible: true,
            width: 240,
            placeholder: "Buscar..."
        },
        selection: {
            mode: 'single'
        },
        hoverStateEnabled: true,
        remoteOperations: true,
        columns: [
            { dataField: 'id', width: '5%', caption: 'Id'},
            { dataField: 'tipoTramite', width: '20%', caption: 'Tipo de Trámite'},
            { dataField: 'numeroVITAL', width: '10%', caption: 'Número VITAL'},
            { dataField: 'numeroVITALAsociado', width: '10%', caption: 'Número VITAL Asociado' },
            { dataField: 'radicacionId', width: '5%', caption: 'Radicado VITAL' },
            { dataField: 'identificador', width: '5%', caption: 'Identificador', visible: false },
            { dataField: 'codigoTramiteSIM', width: '5%', caption: 'Trámite SIM' },
            { dataField: 'fecha', width: '10%', caption: 'Fecha', dataType:'date' },
            { dataField: 'actoAdministrativo', width: '10%', caption: 'Acto Administrativo'},
            { dataField: 'numeroSILPA', width: '15%', caption: 'Número SILPA' },
            { dataField: 'radicacionId',visible:false},
            {
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        caption: "Editar",
                        height: 20,
                        hint: 'Editar la información relacionada con el Expediente Ambiental seleccionado',
                        onClick: function (e) {
                           
                        }
                    }).appendTo(container);
                }
            },
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            $('#asistente').css('visibility', 'visible');
            if (data) {
                id = data.id;
                _numeroVITAL = data.numeroVITAL;
                if (data.codigoTramiteSIM == null) {
                    $("#btnIniciarTramite").dxButton("instance").option("disabled", false);
                    txtTramiteSIM.option("value", '');
                }
                else {
                    txtTramiteSIM.option("value", data.codigoTramiteSIM);
                    $("#btnIniciarTramite").dxButton("instance").option("disabled", true);
                }
                idSolicitudVITAL = data.id;
                _numeroVITALAsociado = data.numeroVITALAsociado;
                _radicadoVITAL = data.radicacionId;
                $('#GridListadoDocumentosRequeridos').dxDataGrid({ dataSource: DocumentosRequeridosDataSource });
                $('#GridListadoDocumentosAportados').dxDataGrid({ dataSource: DocumentosAportadosDataSource });
            }
        }
    });

    $("#GridListadoDocumentosRequeridos").dxDataGrid({
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        export: {
            enabled: false,
            allowExportSelectedData: true,
        },
        paging: {
            pageSize: 10
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
        remoteOperations: true,
        columns: [
            { dataField: 'documentosRequeridosTramitesId', width: '5%', caption: 'Id', visible: false },
            { dataField: 'descripcion', width: '40%', caption: 'Descripción'},
            { dataField: 'formato', width: '10%', caption: 'Formato', visible: false },
            { dataField: 'orden', width: '10%', caption: 'Orden' , visible: false},
            { dataField: 'obligatorio', width: '5%', caption: 'Obligatorio', visible: false },
        ],
        onSelectionChanged: function (selectedItems) {
          
        }
    });

    $("#GridListadoDocumentosAportados").dxDataGrid({
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        export: {
            enabled: false,
            allowExportSelectedData: true,
        },
        paging: {
            pageSize: 10
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
        remoteOperations: true,
        columns: [
            { dataField: 'nombre', width: '80%', caption: 'Descripción' },
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
                            window.open($('#SIM').data('url') + "Utilidades/LeeDocVital?IdRadicadoVital=" + _radicadoVITAL + "&NombreArchivo=" + _nombreArchivo, "Documento " + _nombreArchivo, "width= 900,height=800,scrollbars = yes, location = no, toolbar = no, menubar = no, status = no");
                       }
                    }).appendTo(container);
                }
            },
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            _nombreArchivo = data.nombre;
        }
    });

    var cboProceso = $("#cboProceso").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "codProceso",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#SIM").data("url") + "ExpedienteAmbiental/api/VitalApi/GetProcesos");;
                }
            })
        }),
        onValueChanged: function (data) {
            if (data.value != null) {
                var cboActividadDs = cboActividad.getDataSource();
                _procesoId = data.value;
                cboActividadDs.reload();
                cboActividad.option("value", null);
                _tareaId = 0;
                var cboResponsablesDs = cboResponsable.getDataSource();
                cboResponsablesDs.reload();
                cboResponsable.option("value", null);
            }
        },
        onInitialized(e) {
            e.component.getDataSource().load().done(function (res) {
                e.component.option("value", res[0].codProceso);
            })
        },  
        displayExpr: "nombre",
        valueExpr: "codProceso",
        width: 400,
        searchEnabled: true,
        
    }).dxSelectBox("instance");

    var cboActividad = $("#cboActividad").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "tareaId",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#SIM").data("url") + "ExpedienteAmbiental/api/VitalApi/GetActividades", { procesoId: _procesoId });
                }
            })
        }),
        onValueChanged: function (data) {
            if (data.value != null) {
                var cboResponsablesDs = cboResponsable.getDataSource();
                _tareaId = data.value;
                cboResponsablesDs.reload();
                cboResponsable.option("value", null);
            }
        },
        displayExpr: "nombre",
        valueExpr: "tareaId",
        width: 400,
        searchEnabled: true
    }).dxSelectBox("instance");

    var cboActividadUserE = $("#cboActividadUserE").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "tareaId",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#SIM").data("url") + "ExpedienteAmbiental/api/VitalApi/GetActividades", { procesoId: _procesoId });
                }
            })
        }),
        onValueChanged: function (data) {
            if (data.value != null) {
                var cboResponsablesDs = cboResponsable.getDataSource();
                _tareaId = data.value;
                cboResponsablesDs.reload();
                cboResponsable.option("value", null);
            }
        },
        disabled : true,
        displayExpr: "nombre",
        valueExpr: "tareaId",
        width: 400,
        searchEnabled: true
    }).dxSelectBox("instance");

    var cboResponsable = $("#cboResponsable").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "codFuncionario",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#SIM").data("url") + "ExpedienteAmbiental/api/VitalApi/GetResponsables", { TareaId: _tareaId });
                }
            })
        }),
        displayExpr: "funcionario",
        valueExpr: "codFuncionario",
        width: 400,
        searchEnabled: true
    }).dxSelectBox("instance");

    var cboResponsableUserE = $("#cboResponsableUserE").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "codFuncionario",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#SIM").data("url") + "ExpedienteAmbiental/api/VitalApi/GetResponsables", { TareaId: _tareaId });
                }
            })
        }),
        disabled: true,
        displayExpr: "funcionario",
        valueExpr: "codFuncionario",
        width: 400,
        searchEnabled: true
    }).dxSelectBox("instance");

    var cboCausaNoAtencion = $("#cboCausaNoAtencion").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "causaNoAtencionVITALId",
                loadMode: "raw",
                load: function (loadOptions) {
                    return $.getJSON($("#SIM").data("url") + "ExpedienteAmbiental/api/VitalApi/GetCausasNoAtencion");
                }
            })
        }),
        displayExpr: "nombre",
        valueExpr: "causaNoAtencionVITALId",
        width: 400,
        searchEnabled: true
    }).dxSelectBox("instance");

    const loadIndicator = $('#spinLoadBusqueda').dxLoadIndicator({
        height: 40,
        width: 40,
        visible: false
    }).dxLoadIndicator("instance");

    var txtComentario = $("#txtComentario").dxTextArea({
        value: "",
        readOnly: false,
        height: 380
    }).dxTextArea("instance");

    var txtTramiteSIM = $("#txtTramiteSIM").dxTextBox({
        value: "",
    }).dxTextBox("instance");


    $("#btnBuscarTramite").dxButton({
        text: "Buscar Trámite SIM",
        icon: 'check',
        type: 'success',
        height: 30,
        onClick: function () {
            var tramiteSIM = txtTramiteSIM.option("value");
            if (tramiteSIM === '') {
                DevExpress.ui.notify("Debe Indicar el código del Trámite SIM!");
                return
            }
            var _Ruta = $('#SIM').data('url') + "ExpedienteAmbiental/api/VitalApi/BuscarTramiteEnSIMAsync?codTramite=" + tramiteSIM;
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                contentType: "application/json",
                crossDomain: true,
                headers: { 'Access-Control-Allow-Origin': '*' },
                success: function (data) {
                    if (data.IsSuccess === false) DevExpress.ui.notify({ message: "Ocurrió un evento no esperado!:" + data.Message, width: 1000, shading: true }, "error", 2000);
                    else {
                        var popupOpciones = null;

                        popupOpciones = {
                            height: 600,
                            width: 1100,
                            title: 'Detalle del trámite',
                            visible: false,
                            contentTemplate: function (container) {
                                $("<iframe>").attr("src", $('#SIM').data('url') + 'Utilidades/DetalleTramite?popup=true&CodTramite=' + tramiteSIM).attr("width", "100%").attr("height", "100%").attr("frameborder", "0").attr("scrolling", "0").appendTo(container);
                            }
                        };

                        popup = $("#PopupDetalle").dxPopup(popupOpciones).dxPopup("instance");
                        $('#PopupDetalle').css({ 'visibility': 'visible' });
                        $("#PopupDetalle").fadeTo("slow", 1);
                        popup.show();

                        if (data.Message === 'usuario externo') {
                            cboActividadUserE.option("disabled", false);
                            cboResponsableUserE.option("disabled", false);
                        }
                        else {
                            cboActividadUserE.option("disabled", true);
                            cboActividadUserE.reset();
                            cboResponsableUserE.option("disabled", true);
                            cboResponsableUserE.reset();
                        }
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });


        }
    });

    $("#btnIniciarTramite").dxButton({
        text: "Crear Trámite SIM",
        icon: 'check',
        type: 'success',
        height: 30,
        onClick: function () {
          
            var idResponsable = cboResponsable.option("value");
            if (idResponsable <= 0) {
                DevExpress.ui.notify("Debe seleccionar el funcionario responsable!");
                return
            }

            var comentarios = txtComentario.option("value");
            var idTarea = cboActividad.option("value");
            var codProceso = cboProceso.option("value");

            var params = {
                codTramite: 0, codProceso: codProceso, codTarea: idTarea, codFuncionario: idResponsable, cliente: '', cedula: '', direccion: '', telefono: '', mail: '', comentarios: comentarios, numeroVital: _numeroVITAL, numeroVitalPadre: _numeroVITALAsociado, idSolicitudVITAL: idSolicitudVITAL, radicadoVITAL: _radicadoVITAL
            };

            var _Ruta = $('#SIM').data('url') + "ExpedienteAmbiental/api/VitalApi/AvanzarEnSIMAsync";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                crossDomain: true,
                headers: { 'Access-Control-Allow-Origin': '*' },
                success: function (data) {
                    if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.Message, 'Guardar Datos');
                    else {
                        DevExpress.ui.notify({ message: "Se crea el trámite en el SIM : " + data.Result + " para atender la solicitud de VITAL : ", width: 1000, shading: true }, "warning", 2000);
                        $('#GridListado').dxDataGrid({ dataSource: SolicitudesVitalDataSource });
                      
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });


        }
    });
        
    $("#btnDescartarTramite").dxButton({
        text: "No atender solicitud",
        type: "danger",
        height: 30,
        onClick: function () {
            var comentarios = txtComentario.option("value");
            if (comentarios.trim().length === 0) {
                DevExpress.ui.notify("Debe explicar el motivo de la no atención de la solicitud de VITAL!");
                return
            }
            var causaNoAtencion = cboCausaNoAtencion.option("value");
            if (causaNoAtencion <= 0) {
                DevExpress.ui.notify("Debe seleccionar la causa de la no atención de la solicitud de VITAL!");
                return
            }

            var params = {
                codTramite: 0, codProceso: 0, codTarea: 0, codFuncionario: 0, cliente: '', cedula: '', direccion: '', telefono: '', mail: '', comentarios: comentarios,
                numeroVital: _numeroVITAL, numeroVitalPadre: _numeroVITALAsociado, codCausaNoAtencion: causaNoAtencion, mensaje: comentarios,
                CodFuncionarioSIM: codFuncionarioSim, radicadoVITAL: _radicadoVITAL
            };

            var _Ruta = $('#SIM').data('url') + "ExpedienteAmbiental/api/VitalApi/DescartarEnSIM";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                crossDomain: true,
                headers: { 'Access-Control-Allow-Origin': '*' },
                success: function (data) {
                    if(data.IsSuccess === false) DevExpress.ui.notify({message: "Ocurrió un evento no esperado!:" + data.mensaje, width: 1000, shading: true }, "error", 2000);
                    else {
                        var cboActividadDs = cboActividad.getDataSource();

                        _procesoId = 0;
                        cboActividadDs.reload();
                        cboActividad.option("value", null);

                        _tareaId = 0;
                        var cboResponsablesDs = cboResponsable.getDataSource();
                        cboResponsablesDs.reload();
                        cboResponsable.option("value", null);

                        txtComentario.reset();
                        txtTramiteSIM.reset();
                        cboCausaNoAtencion.reset();

                        DevExpress.ui.notify({ message: "Se descarta la atención en el SIM de la solicitud de VITAL" , width: 1000, shading: true }, "warning", 2000);
                        $('#GridListado').dxDataGrid({ dataSource: SolicitudesVitalDataSource });

                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    $("#btnAsignarATramite").dxButton({
        text: "Asignar al Trámite SIM",
        type: "success",
        height: 30,
        onClick: function () {
            var tramiteSIM = txtTramiteSIM.option("value");
            if (tramiteSIM.length <= 0) {
                DevExpress.ui.notify("Debe Indicar el código del Trámite SIM!");
                return
            }

            var idResponsable = cboResponsableUserE.option("value");
            if (idResponsable <= 0 && cboResponsableUserE.option("disabled") == false) {
                DevExpress.ui.notify("Debe seleccionar el funcionario Responsable!");
                return
            }

            var comentarios = txtComentario.option("value");
            var idTarea = cboActividadUserE.option("value");
            var codProceso = cboProceso.option("value");
            
            var params = {
                codTramite: tramiteSIM, codProceso: codProceso, codTarea: idTarea, codFuncionario: idResponsable, cliente: '', cedula: '', direccion: '', telefono: '', mail: '', comentarios: comentarios, numeroVital: _numeroVITAL, numeroVitalPadre: _numeroVITALAsociado, idSolicitudVITAL: idSolicitudVITAL, codFuncionarioSIM: CodigoFuncionario, radicadoVITAL: _radicadoVITAL
            };

            var _Ruta = $('#SIM').data('url') + "ExpedienteAmbiental/api/VitalApi/AsignarTramiteSIMAsync";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                crossDomain: true,
                headers: { 'Access-Control-Allow-Origin': '*' },
                success: function (data) {
                    if (data.IsSuccess === false) DevExpress.ui.notify({ message: "Advertencia! : " + data.Message, width: 1000, shading: true }, "error", 2000);
                    else {
                        var cboActividadDs = cboActividad.getDataSource();

                        _procesoId = 0;
                        cboActividadDs.reload();
                        cboActividad.option("value", null);

                        //_tareaId = 0;
                        var cboResponsablesDs = cboResponsable.getDataSource();
                        cboResponsablesDs.reload();
                        cboResponsableUserE.option("value", null);

                        txtComentario.reset();
                        txtTramiteSIM.reset();
                        cboCausaNoAtencion.reset();

                        DevExpress.ui.notify({ message: "Se anexa la solicitud de VITAL al Trámite SIM: " + tramiteSIM, width: 1000, shading: true }, "warning", 2000);
                        $('#GridListado').dxDataGrid({ dataSource: SolicitudesVitalDataSource });
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });


        }
    });

});

var SolicitudesVitalDataSource = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var d = $.Deferred();
            var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
            var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"tipoTramite","desc":false}]';
            var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

            var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
            var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
            $.getJSON($('#SIM').data('url') + 'ExpedienteAmbiental/api/VitalApi/GetSolicitudesVITALenSIMAsync', {
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
                if (data.datos === null) {
                    DevExpress.ui.notify({ message: "No fué posible realizar conexión al microservicio de Expedientes Ambientales", width: 1000, shading: true }, "error", 2000);
                }
                d.resolve(data.datos, { totalCount: data.numRegistros });

            }).fail(function (jqxhr, textStatus, error) {
                alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
            });
            return d.promise();
        }
});

var DocumentosRequeridosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"orden","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'ExpedienteAmbiental/api/VitalApi/GetDocumentosRequeridosTramitesync', {
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
            CodFuncionario: CodigoFuncionario,
            numeroVITAL: _numeroVITAL
        }).done(function (data) {
            if (data.datos === null) {
                DevExpress.ui.notify({ message: "No existen documentos establecidos para este tipo de solicitud!!!", width: 1000, shading: true }, "warning", 2000);
            }
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            DevExpress.ui.notify({
                message: "Error cargando datos: " + textStatus + ", " + jqxhr.responseText, width: 1000, shading: true }, "error", 2000);
        });
        return d.promise();
    }
});

var DocumentosAportadosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"nombre","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'ExpedienteAmbiental/api/VitalApi/GetDocumentosAportadosync', {
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
            CodFuncionario: CodigoFuncionario,
            radicadoVITAL: _radicadoVITAL
        }).done(function (data) {
            if (data.datos === null) {
                DevExpress.ui.notify({ message: "No existen documentos establecidos para este tipo de solicitud!!!", width: 1000, shading: true }, "warning", 2000);
            }
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            DevExpress.ui.notify({
                message: "Error cargando datos: " + textStatus + ", " + jqxhr.responseText, width: 1000, shading: true
            }, "error", 2000);
        });
        return d.promise();
    }
});



