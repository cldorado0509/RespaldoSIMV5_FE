var posTab = 0;

$(document).ready(function () {
    var idActual = 0;

    $("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
        shadingColor: "rgba(0,0,0,0.4)",
    });

    $("#popAvanzaTareaTramite").dxPopup({
        title: "Avanza Tarea",
        fullScreen: true,
    });

    $("#popDetalleTramite").dxPopup({
        title: "Detalle Trámite",
        fullScreen: false,
    });

    $("#popDocumento").dxPopup({
        title: "Anulación Documento",
        fullScreen: true,
        onHiding: function (e) {
            $('#frmDocumento').attr('src', '');
            switch (posTab) {
                case 0:
                    $('#grdSolicitud').dxDataGrid('instance').option('dataSource', solicitudDataSource);
                    break;
            }
        }
    });

    $("#popDocumentoPreview").dxPopup({
        title: "Anulación Documento - Vista Previa",
        fullScreen: true,
        onHiding: function (e) {
            switch (posTab) {
                case 0:
                    $('#frmDocumentoPreview').attr('src', null);
                    break;
            }
        }
    });

    var tabsData = [
        { text: 'BÚSQUEDA DOCUMENTO', pos: 0 },
        { text: 'MIS SOLICITUDES', pos: 1 },
        { text: 'JUSTIFICACIÓN', pos: 2 },
    ];

    if ($('#app').data('ua') == '1')
        tabsData.push({ text: 'AUTORIZACIÓN', pos: 3 });

    if ($('#app').data('uatu') == '1')
        tabsData.push({ text: 'APROBACIÓN ATU', pos: 4 });

    $("#tabOpciones").dxTabs({
        dataSource: tabsData,
        onItemClick: (function (itemData) {
            switch (itemData.itemData.pos) {
                case 0:
                    $('#tab00').css('display', 'block');
                    $('#tab01').css('display', 'none');
                    $('#tab02').css('display', 'none');
                    $('#tab03').css('display', 'none');
                    $('#tab04').css('display', 'none');

                    $(window).trigger('resize');
                    break;
                case 1:
                    $('#tab01').css('display', 'block');
                    $('#tab00').css('display', 'none');
                    $('#tab02').css('display', 'none');
                    $('#tab03').css('display', 'none');
                    $('#tab04').css('display', 'none');

                    $('#grdSolicitud').dxDataGrid('instance').option('dataSource', solicitudDataSource);

                    $(window).trigger('resize');
                    break;
                case 2:
                    $('#tab02').css('display', 'block');
                    $('#tab00').css('display', 'none');
                    $('#tab01').css('display', 'none');
                    $('#tab03').css('display', 'none');
                    $('#tab04').css('display', 'none');

                    $('#grdJustificacion').dxDataGrid('instance').option('dataSource', justificacionDataSource);

                    $(window).trigger('resize');

                    break;
                case 3:
                    $('#tab03').css('display', 'block');
                    $('#tab00').css('display', 'none');
                    $('#tab01').css('display', 'none');
                    $('#tab02').css('display', 'none');
                    $('#tab04').css('display', 'none');

                    $('#grdAutorizacion').dxDataGrid('instance').option('dataSource', autorizacionDataSource);

                    $(window).trigger('resize');
                    break;
                case 4:
                    $('#tab04').css('display', 'block');
                    $('#tab00').css('display', 'none');
                    $('#tab01').css('display', 'none');
                    $('#tab02').css('display', 'none');
                    $('#tab03').css('display', 'none');

                    $('#grdAprobacionATU').dxDataGrid('instance').option('dataSource', aprobacionATUDataSource);

                    $(window).trigger('resize');
                    break;
            }

            posTab = itemData.itemIndex;
        }),
        selectedIndex: 0,
    });

    $("#grdDocumentos").dxDataGrid({
        dataSource: documentosDataSource,
        allowColumnResizing: true,
        height: '100%',
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            enabled: true,
            pageSize: 10,
        },
        pager: {
            showPageSizeSelector: true,
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
        remoteOperations: true,
        onRowPrepared: function (info) {
            if (info.rowType == "data") {
                if (info.data.S_ESTADODOC == 'A') {
                    info.rowElement.css('background', 'LightPink');
                } else if (info.data.S_ESTADODOC == 'P') {
                    info.rowElement.css('background', 'Yellow');
                }
            }
        },
        columns: [
            {
                dataField: "DIA",
                width: '3%',
                caption: 'D',
                dataType: 'number',
                filterOperations: [],
                visible: true,
            }, {
                dataField: "MES",
                width: '3%',
                caption: 'M',
                dataType: 'number',
                filterOperations: [],
                visible: true,
            }, {
                dataField: "ANO",
                width: '5%',
                caption: 'A',
                dataType: 'number',
                filterOperations: [],
                visible: true,
            }, {
                dataField: "ID_PROYECCION_DOC",
                width: '8%',
                caption: 'CODIGO',
                dataType: 'number',
                visible: ($('#app').data('codigo') == '1'),
            }, {
                dataField: 'S_DESCRIPCION',
                caption: 'DOCUMENTO',
                dataType: 'string',
                visible: true,
            }, {
                dataField: 'S_SERIE',
                width: '20%',
                caption: 'SERIE DOCUMENTAL',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'S_RADICADO',
                width: '8%',
                caption: 'RADICADO',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'S_TRAMITES',
                width: '12%',
                caption: 'TRÁMITES',
                dataType: 'string',
                visible: true,
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_TRAMITES != null && cellInfo.data.S_TRAMITES != '') {
                        var tramites = cellInfo.data.S_TRAMITES.split(',');
                        var enlaces = '';

                        if (tramites.length > 10) {
                            cellElement.append("<div title='" + cellInfo.text + "'>" + cellInfo.text + "</div>")
                        } else {
                            for (index = 0; index < tramites.length; index++) {
                                if (index == 0) {
                                    enlaces = '<a href="#" onclick="abrirDetalleTramite(' + tramites[index] + ');">' + tramites[index] + '</a>'
                                } else {
                                    enlaces += '<br><a href="#" onclick="abrirDetalleTramite(' + tramites[index] + ');">' + tramites[index] + '</a>'
                                }
                            }

                            cellElement.html(enlaces);
                        }
                    }
                }
            }, {
                dataField: 'S_ESTADO',
                width: '8%',
                caption: 'ESTADO',
                dataType: 'string',
                visible: true
            }/*, {
                dataField: 'S_FUNCIONARIO',
                width: '15%',
                caption: 'FUNCIONARIO',
                dataType: 'string',
            }*/, {
                caption: '',
                width: '5%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'search',
                            type: 'success',
                            hint: 'Ver Documento',
                            onClick: function (params) {
                                if (cellInfo.data.ID_PROYECCION_DOC != null && cellInfo.data.ID_PROYECCION_DOC > 0) {
                                    var documentoDetalle = $("#popDocumento").dxPopup("instance");
                                    documentoDetalle.show();

                                    $('#frmDocumento').attr('src', $('#app').data('url') + 'Tramites/ProyeccionDocumento/DocumentoFinal?id=' + cellInfo.data.ID_PROYECCION_DOC + '&c=@DateTime.Now.ToString("HHmmss")');
                                }
                            }
                        }
                    ).appendTo(cellElement);
                }
            }, {
                caption: '',
                width: '5%',
                alignment: 'center',
                visible: true,
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_ESTADODOC == 'OK' && cellInfo.data.S_PERMITEANULAR == 'S') {
                        $('<div />').dxButton(
                            {
                                icon: 'clear',
                                type: 'danger',
                                hint: 'Anular Documento',
                                onClick: function (params) {
                                    if (cellInfo.data.ID_PROYECCION_DOC != null && cellInfo.data.ID_PROYECCION_DOC > 0) {
                                        var result = DevExpress.ui.dialog.confirm("Está Seguro de Solicitar la Anulación del Documento Seleccionado ?", "Confirmación");
                                        result.done(function (dialogResult) {
                                            if (dialogResult) {
                                                var aprobacionInstance = $("#popDocumento").dxPopup("instance");
                                                aprobacionInstance.show();

                                                $('#frmDocumento').attr('src', $('#app').data('url') + 'Tramites/AnulacionDocumento/Documento?id=-1&idP=' + cellInfo.data.ID_PROYECCION_DOC);
                                            }
                                        });
                                    } else {
                                        MostrarNotificacion('alert', null, 'Selección Inválida.');
                                    }
                                }
                            }
                        ).appendTo(cellElement);
                    }
                }
            }
        ]
    });

    $("#grdSolicitud").dxDataGrid({
        dataSource: null,
        allowColumnResizing: true,
        height: '100%',
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            enabled: false,
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
            mode: 'none'
        },
        onCellPrepared: function (info) {
            if (info.rowType == "data" && info.column.dataField == 'S_ESTADO_DESCRIPCION') {
                //alert(JSON.stringify(info.data));
                if (info.data.S_ESTADO == 'P') {
                    info.cellElement.css('background', 'Yellow');
                } else if (info.data.S_ESTADO == 'A') {
                    info.cellElement.css('background', 'LightGreen');
                } else if (info.data.S_ESTADO == 'R') {
                    info.cellElement.css('background', 'LightPink');
                }
            }
        },
        columns: [
            {
                dataField: "ID_ANULACION_DOC",
                width: '8%',
                caption: 'CODIGO',
                dataType: 'number',
                visible: ($('#app').data('codigo') == '1'),
            }, {
                dataField: 'D_FECHA_SOLICITUD',
                width: '8%',
                caption: 'FECHA SOL',
                dataType: 'date',
                format: "dd/MM/yyyy",
                visible: true
            }, {
                dataField: 'S_DESCRIPCION',
                caption: 'DOCUMENTO',
                dataType: 'string',
                visible: true,
            }, {
                dataField: 'S_SERIE',
                width: '12%',
                caption: 'SERIE DOCUMENTAL',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'S_RADICADO',
                width: '10%',
                caption: 'RADICADO',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'S_MOTIVO_ANULACION',
                width: '20%',
                caption: 'MOTIVO ANULACIÓN',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'D_FECHA_FINALIZACION',
                width: '8%',
                caption: 'FECHA FIN',
                dataType: 'date',
                format: "dd/MM/yyyy",
                visible: true
            }, {
                dataField: 'S_FUNCIONARIO_ACTUAL',
                width: '12%',
                caption: 'FUNCIONARIO ACTUAL',
                dataType: 'string',
            }, {
                dataField: 'S_ESTADO_DESCRIPCION',
                width: '10%',
                caption: 'ESTADO',
                dataType: 'string',
            }, {
                caption: '',
                width: '4%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'edit',
                            type: 'success',
                            hint: 'Ver Solicitud',
                            onClick: function (params) {
                                var documentoDetalle = $("#popDocumento").dxPopup("instance");
                                documentoDetalle.show();

                                $('#frmDocumento').attr('src', $('#app').data('url') + 'Tramites/AnulacionDocumento/Documento?id=' + cellInfo.data.ID_ANULACION_DOC + '&idP=0&c=@DateTime.Now.ToString("HHmmss")');
                            }
                        }
                    ).appendTo(cellElement);
                }
            }, {
                caption: '',
                width: '4%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'search',
                            type: 'success',
                            hint: 'Ver Documento',
                            onClick: function (params) {
                                if (cellInfo.data.ID_PROYECCION_DOC != null && cellInfo.data.ID_PROYECCION_DOC > 0) {
                                    var documentoDetalle = $("#popDocumento").dxPopup("instance");
                                    documentoDetalle.show();

                                    $('#frmDocumento').attr('src', $('#app').data('url') + 'Tramites/ProyeccionDocumento/DocumentoFinal?id=' + cellInfo.data.ID_PROYECCION_DOC + '&c=@DateTime.Now.ToString("HHmmss")');
                                }
                            }
                        }
                    ).appendTo(cellElement);
                }
            }
        ]
    });

    $("#grdJustificacion").dxDataGrid({
        dataSource: null,
        allowColumnResizing: true,
        height: '100%',
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            enabled: false,
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
            mode: 'none'
        },
        columns: [
            {
                dataField: "ID_ANULACION_DOC",
                width: '8%',
                caption: 'CODIGO',
                dataType: 'number',
                visible: ($('#app').data('codigo') == '1'),
            }, {
                dataField: 'D_FECHA_SOLICITUD',
                width: '8%',
                caption: 'FECHA SOL',
                dataType: 'date',
                format: "dd/MM/yyyy",
                visible: true
            }, {
                dataField: 'S_DESCRIPCION',
                caption: 'DOCUMENTO',
                dataType: 'string',
                visible: true,
            }, {
                dataField: 'S_RADICADO',
                width: '10%',
                caption: 'RADICADO',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'S_SERIE',
                width: '14%',
                caption: 'SERIE DOCUMENTAL',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'S_MOTIVO_ANULACION',
                width: '18%',
                caption: 'MOTIVO ANULACIÓN',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'S_TRAMITES',
                width: '7%',
                caption: 'TRÁMITES',
                dataType: 'string',
                visible: true,
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_TRAMITES != null && cellInfo.data.S_TRAMITES != '') {
                        var tramites = cellInfo.data.S_TRAMITES.split(',');
                        var enlaces = '';

                        if (tramites.length > 10) {
                            cellElement.append("<div title='" + cellInfo.text + "'>" + cellInfo.text + "</div>")
                        } else {
                            for (index = 0; index < tramites.length; index++) {
                                if (index == 0) {
                                    enlaces = '<a href="#" onclick="abrirDetalleTramite(' + tramites[index] + ');">' + tramites[index] + '</a>'
                                } else {
                                    enlaces += '<br><a href="#" onclick="abrirDetalleTramite(' + tramites[index] + ');">' + tramites[index] + '</a>'
                                }
                            }

                            cellElement.html(enlaces);
                        }
                    }
                }
            }, {
                dataField: 'S_PROCESOS',
                width: '13%',
                caption: 'PROCESO',
                dataType: 'string',
                visible: true
            }, {
                caption: '',
                width: '4%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'edit',
                            type: 'success',
                            hint: 'Ver Solicitud',
                            onClick: function (params) {
                                var documentoDetalle = $("#popDocumento").dxPopup("instance");
                                documentoDetalle.show();

                                $('#frmDocumento').attr('src', $('#app').data('url') + 'Tramites/AnulacionDocumento/Documento?id=' + cellInfo.data.ID_ANULACION_DOC + '&idP=0&c=@DateTime.Now.ToString("HHmmss")');
                            }
                        }
                    ).appendTo(cellElement);
                }
            }, {
                caption: '',
                width: '4%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'search',
                            type: 'success',
                            hint: 'Ver Documento',
                            onClick: function (params) {
                                if (cellInfo.data.ID_PROYECCION_DOC != null && cellInfo.data.ID_PROYECCION_DOC > 0) {
                                    var documentoDetalle = $("#popDocumento").dxPopup("instance");
                                    documentoDetalle.show();

                                    $('#frmDocumento').attr('src', $('#app').data('url') + 'Tramites/ProyeccionDocumento/DocumentoFinal?id=' + cellInfo.data.ID_PROYECCION_DOC + '&c=@DateTime.Now.ToString("HHmmss")');
                                }
                            }
                        }
                    ).appendTo(cellElement);
                }
            }, {
                caption: '',
                width: '5%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_ACTUAL == 'S') {
                        $('<div />').dxButton(
                            {
                                icon: 'check',
                                type: 'success',
                                hint: 'Aceptar Solicitud y Avanzar a Autorización',
                                onClick: function (params) {
                                    if (cellInfo.data.ID_ANULACION_DOC != null && cellInfo.data.ID_ANULACION_DOC > 0) {
                                        var result = DevExpress.ui.dialog.confirm("Está Seguro de Avanzar el Documento a Autorización ?", "Confirmación");
                                        result.done(function (dialogResult) {
                                            if (dialogResult) {
                                                $("#loadPanel").dxLoadPanel('instance').show();

                                                $.getJSON($('#app').data('url') + 'Tramites/api/AnulacionDocumentoApi/AvanzarDocumento', {
                                                    id: cellInfo.data.ID_ANULACION_DOC
                                                }).done(function (data) {
                                                    var respuesta = data.split(':');

                                                    if (respuesta[0] == 'OK') {
                                                        $("#loadPanel").dxLoadPanel('instance').hide();
                                                        var result = MostrarNotificacion('alert', null, 'Documento Avanzado Satisfactoriamente.');

                                                        result.done(function (dialogResult) {
                                                            $('#grdJustificacion').dxDataGrid('instance').option('dataSource', justificacionDataSource);
                                                        });
                                                    } else {
                                                        $("#loadPanel").dxLoadPanel('instance').hide();
                                                        var result = MostrarNotificacion('alert', null, 'ERROR: ' + respuesta[1]);
                                                    }
                                                }).fail(function (jqxhr, textStatus, error) {
                                                    $("#loadPanel").dxLoadPanel('instance').hide();
                                                    alert('falla: ' + textStatus + ", " + error);
                                                    $('#grdJustificacion').dxDataGrid('instance').option('dataSource', justificacionDataSource);
                                                });
                                            }
                                        });
                                    }
                                }
                            }
                        ).appendTo(cellElement);
                    }
                }
            }, {
                caption: '',
                width: '5%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_ACTUAL == 'S') {
                        $('<div />').dxButton(
                            {
                                icon: 'close',
                                type: 'danger',
                                hint: 'Rechazar Solicitud de Anulación',
                                onClick: function (params) {
                                    if (cellInfo.data.ID_ANULACION_DOC != null && cellInfo.data.ID_ANULACION_DOC > 0) {
                                        var result = DevExpress.ui.dialog.confirm("Está Seguro de Rechazar la Solicitud de Anulación ?", "Confirmación");
                                        result.done(function (dialogResult) {
                                            if (dialogResult) {
                                                $("#loadPanel").dxLoadPanel('instance').show();

                                                $.getJSON($('#app').data('url') + 'Tramites/api/AnulacionDocumentoApi/RechazarDocumento', {
                                                    id: cellInfo.data.ID_ANULACION_DOC
                                                }).done(function (data) {
                                                    var respuesta = data.split(':');

                                                    if (respuesta[0] == 'OK') {
                                                        $("#loadPanel").dxLoadPanel('instance').hide();
                                                        var result = MostrarNotificacion('alert', null, 'Solicitud de Anulación Rechazada Satisfactoriamente.');

                                                        result.done(function (dialogResult) {
                                                            $('#grdJustificacion').dxDataGrid('instance').option('dataSource', justificacionDataSource);
                                                        });
                                                    } else {
                                                        $("#loadPanel").dxLoadPanel('instance').hide();
                                                        var result = MostrarNotificacion('alert', null, 'ERROR: ' + respuesta[1]);
                                                    }
                                                }).fail(function (jqxhr, textStatus, error) {
                                                    $("#loadPanel").dxLoadPanel('instance').hide();
                                                    alert('falla: ' + textStatus + ", " + error);
                                                    $('#grdJustificacion').dxDataGrid('instance').option('dataSource', justificacionDataSource);
                                                });
                                            }
                                        });
                                    }
                                }
                            }
                        ).appendTo(cellElement);
                    }
                }
            }
        ]
    });

    $("#grdAutorizacion").dxDataGrid({
        dataSource: null,
        allowColumnResizing: true,
        height: '100%',
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            enabled: false,
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
            mode: 'none'
        },
        columns: [
            {
                dataField: "ID_ANULACION_DOC",
                width: '8%',
                caption: 'CODIGO',
                dataType: 'number',
                visible: ($('#app').data('codigo') == '1'),
            }, {
                dataField: 'D_FECHA_SOLICITUD',
                width: '8%',
                caption: 'FECHA SOL',
                dataType: 'date',
                format: "dd/MM/yyyy",
                visible: true
            }, {
                dataField: 'S_DESCRIPCION',
                caption: 'DOCUMENTO',
                dataType: 'string',
                visible: true,
            }, {
                dataField: 'S_SERIE',
                width: '14%',
                caption: 'SERIE DOCUMENTAL',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'S_RADICADO',
                width: '10%',
                caption: 'RADICADO',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'S_MOTIVO_ANULACION',
                width: '18%',
                caption: 'MOTIVO ANULACIÓN',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'S_TRAMITES',
                width: '7%',
                caption: 'TRÁMITES',
                dataType: 'string',
                visible: true,
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_TRAMITES != null && cellInfo.data.S_TRAMITES != '') {
                        var tramites = cellInfo.data.S_TRAMITES.split(',');
                        var enlaces = '';

                        if (tramites.length > 10) {
                            cellElement.append("<div title='" + cellInfo.text + "'>" + cellInfo.text + "</div>")
                        } else {
                            for (index = 0; index < tramites.length; index++) {
                                if (index == 0) {
                                    enlaces = '<a href="#" onclick="abrirDetalleTramite(' + tramites[index] + ');">' + tramites[index] + '</a>'
                                } else {
                                    enlaces += '<br><a href="#" onclick="abrirDetalleTramite(' + tramites[index] + ');">' + tramites[index] + '</a>'
                                }
                            }

                            cellElement.html(enlaces);
                        }
                    }
                }
            }, {
                dataField: 'S_PROCESOS',
                width: '13%',
                caption: 'PROCESO',
                dataType: 'string',
                visible: true
            }, {
                caption: '',
                width: '4%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'edit',
                            type: 'success',
                            hint: 'Ver Solicitud',
                            onClick: function (params) {
                                var documentoDetalle = $("#popDocumento").dxPopup("instance");
                                documentoDetalle.show();

                                $('#frmDocumento').attr('src', $('#app').data('url') + 'Tramites/AnulacionDocumento/Documento?id=' + cellInfo.data.ID_ANULACION_DOC + '&idP=0&c=@DateTime.Now.ToString("HHmmss")');
                            }
                        }
                    ).appendTo(cellElement);
                }
            }, {
                caption: '',
                width: '4%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'search',
                            type: 'success',
                            hint: 'Ver Documento',
                            onClick: function (params) {
                                if (cellInfo.data.ID_PROYECCION_DOC != null && cellInfo.data.ID_PROYECCION_DOC > 0) {
                                    var documentoDetalle = $("#popDocumento").dxPopup("instance");
                                    documentoDetalle.show();

                                    $('#frmDocumento').attr('src', $('#app').data('url') + 'Tramites/ProyeccionDocumento/DocumentoFinal?id=' + cellInfo.data.ID_PROYECCION_DOC + '&c=@DateTime.Now.ToString("HHmmss")');
                                }
                            }
                        }
                    ).appendTo(cellElement);
                }
            }, {
                caption: '',
                width: '5%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_ACTUAL == 'S') {
                        $('<div />').dxButton(
                            {
                                icon: 'check',
                                type: 'success',
                                hint: 'Aceptar Solicitud y Avanzar a Aprobación ATU',
                                onClick: function (params) {
                                    if (cellInfo.data.ID_ANULACION_DOC != null && cellInfo.data.ID_ANULACION_DOC > 0) {
                                        var result = DevExpress.ui.dialog.confirm("Está Seguro de Avanzar el Documento a Aprobación ATU?", "Confirmación");
                                        result.done(function (dialogResult) {
                                            if (dialogResult) {
                                                $("#loadPanel").dxLoadPanel('instance').show();

                                                $.getJSON($('#app').data('url') + 'Tramites/api/AnulacionDocumentoApi/AvanzarDocumento', {
                                                    id: cellInfo.data.ID_ANULACION_DOC
                                                }).done(function (data) {
                                                    var respuesta = data.split(':');

                                                    if (respuesta[0] == 'OK') {
                                                        $("#loadPanel").dxLoadPanel('instance').hide();
                                                        var result = MostrarNotificacion('alert', null, 'Documento Avanzado Satisfactoriamente.');

                                                        result.done(function (dialogResult) {
                                                            $('#grdAutorizacion').dxDataGrid('instance').option('dataSource', autorizacionDataSource);
                                                        });
                                                    } else {
                                                        $("#loadPanel").dxLoadPanel('instance').hide();
                                                        var result = MostrarNotificacion('alert', null, 'ERROR: ' + respuesta[1]);
                                                    }
                                                }).fail(function (jqxhr, textStatus, error) {
                                                    $("#loadPanel").dxLoadPanel('instance').hide();
                                                    alert('falla: ' + textStatus + ", " + error);
                                                    $('#grdAutorizacion').dxDataGrid('instance').option('dataSource', autorizacionDataSource);
                                                });
                                            }
                                        });
                                    }
                                }
                            }
                        ).appendTo(cellElement);
                    }
                }
            }, {
                caption: '',
                width: '5%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_ACTUAL == 'S') {
                        $('<div />').dxButton(
                            {
                                icon: 'close',
                                type: 'danger',
                                hint: 'Rechazar Solicitud de Anulación',
                                onClick: function (params) {
                                    if (cellInfo.data.ID_ANULACION_DOC != null && cellInfo.data.ID_ANULACION_DOC > 0) {
                                        var result = DevExpress.ui.dialog.confirm("Está Seguro de Rechazar la Solicitud de Anulación ?", "Confirmación");
                                        result.done(function (dialogResult) {
                                            if (dialogResult) {
                                                $("#loadPanel").dxLoadPanel('instance').show();

                                                $.getJSON($('#app').data('url') + 'Tramites/api/AnulacionDocumentoApi/RechazarDocumento', {
                                                    id: cellInfo.data.ID_ANULACION_DOC
                                                }).done(function (data) {
                                                    var respuesta = data.split(':');

                                                    if (respuesta[0] == 'OK') {
                                                        $("#loadPanel").dxLoadPanel('instance').hide();
                                                        var result = MostrarNotificacion('alert', null, 'Solicitud de Anulación Rechazada Satisfactoriamente.');

                                                        result.done(function (dialogResult) {
                                                            $('#grdAutorizacion').dxDataGrid('instance').option('dataSource', autorizacionDataSource);
                                                        });
                                                    } else {
                                                        $("#loadPanel").dxLoadPanel('instance').hide();
                                                        var result = MostrarNotificacion('alert', null, 'ERROR: ' + respuesta[1]);
                                                    }
                                                }).fail(function (jqxhr, textStatus, error) {
                                                    $("#loadPanel").dxLoadPanel('instance').hide();
                                                    alert('falla: ' + textStatus + ", " + error);
                                                    $('#grdAutorizacion').dxDataGrid('instance').option('dataSource', autorizacionDataSource);
                                                });
                                            }
                                        });
                                    }
                                }
                            }
                        ).appendTo(cellElement);
                    }
                }
            }
        ]
    });

    $("#grdAprobacionATU").dxDataGrid({
        dataSource: null,
        allowColumnResizing: true,
        height: '100%',
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            enabled: false,
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
            mode: 'none'
        },
        columns: [
            {
                dataField: "ID_ANULACION_DOC",
                width: '8%',
                caption: 'CODIGO',
                dataType: 'number',
                visible: ($('#app').data('codigo') == '1'),
            }, {
                dataField: 'D_FECHA_SOLICITUD',
                width: '8%',
                caption: 'FECHA SOL',
                dataType: 'date',
                format: "dd/MM/yyyy",
                visible: true
            }, {
                dataField: 'S_DESCRIPCION',
                caption: 'DOCUMENTO',
                dataType: 'string',
                visible: true,
            }, {
                dataField: 'S_SERIE',
                width: '14%',
                caption: 'SERIE DOCUMENTAL',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'S_RADICADO',
                width: '10%',
                caption: 'RADICADO',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'S_MOTIVO_ANULACION',
                width: '18%',
                caption: 'MOTIVO ANULACIÓN',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'S_TRAMITES',
                width: '7%',
                caption: 'TRÁMITES',
                dataType: 'string',
                visible: true,
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_TRAMITES != null && cellInfo.data.S_TRAMITES != '') {
                        var tramites = cellInfo.data.S_TRAMITES.split(',');
                        var enlaces = '';

                        if (tramites.length > 10) {
                            cellElement.append("<div title='" + cellInfo.text + "'>" + cellInfo.text + "</div>")
                        } else {
                            for (index = 0; index < tramites.length; index++) {
                                if (index == 0) {
                                    enlaces = '<a href="#" onclick="abrirDetalleTramite(' + tramites[index] + ');">' + tramites[index] + '</a>'
                                } else {
                                    enlaces += '<br><a href="#" onclick="abrirDetalleTramite(' + tramites[index] + ');">' + tramites[index] + '</a>'
                                }
                            }

                            cellElement.html(enlaces);
                        }
                    }
                }
            }, {
                dataField: 'S_PROCESOS',
                width: '13%',
                caption: 'PROCESO',
                dataType: 'string',
                visible: true
            }, {
                caption: '',
                width: '4%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'edit',
                            type: 'success',
                            hint: 'Ver Solicitud',
                            onClick: function (params) {
                                var documentoDetalle = $("#popDocumento").dxPopup("instance");
                                documentoDetalle.show();

                                $('#frmDocumento').attr('src', $('#app').data('url') + 'Tramites/AnulacionDocumento/Documento?id=' + cellInfo.data.ID_ANULACION_DOC + '&idP=0&c=@DateTime.Now.ToString("HHmmss")');
                            }
                        }
                    ).appendTo(cellElement);
                }
            }, {
                caption: '',
                width: '4%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'search',
                            type: 'success',
                            hint: 'Ver Documento',
                            onClick: function (params) {
                                if (cellInfo.data.ID_PROYECCION_DOC != null && cellInfo.data.ID_PROYECCION_DOC > 0) {
                                    var documentoDetalle = $("#popDocumento").dxPopup("instance");
                                    documentoDetalle.show();

                                    $('#frmDocumento').attr('src', $('#app').data('url') + 'Tramites/ProyeccionDocumento/DocumentoFinal?id=' + cellInfo.data.ID_PROYECCION_DOC + '&c=@DateTime.Now.ToString("HHmmss")');
                                }
                            }
                        }
                    ).appendTo(cellElement);
                }
            }, {
                caption: '',
                width: '5%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_ACTUAL == 'S') {
                        $('<div />').dxButton(
                            {
                                icon: 'check',
                                type: 'success',
                                hint: 'Aceptar Solicitud de Anulación',
                                onClick: function (params) {
                                    if (cellInfo.data.ID_ANULACION_DOC != null && cellInfo.data.ID_ANULACION_DOC > 0) {
                                        var result = DevExpress.ui.dialog.confirm("Está seguro de aceptar la solicitud y hacer efectiva la anulación ?", "Confirmación");
                                        result.done(function (dialogResult) {
                                            if (dialogResult) {
                                                $("#loadPanel").dxLoadPanel('instance').show();

                                                $.getJSON($('#app').data('url') + 'Tramites/api/AnulacionDocumentoApi/AvanzarDocumento', {
                                                    id: cellInfo.data.ID_ANULACION_DOC
                                                }).done(function (data) {
                                                    var respuesta = data.split(':');

                                                    if (respuesta[0] == 'OK') {
                                                        $("#loadPanel").dxLoadPanel('instance').hide();
                                                        var result = MostrarNotificacion('alert', null, 'Documento Anulado Satisfactoriamente.');

                                                        result.done(function (dialogResult) {
                                                            $('#grdAprobacionATU').dxDataGrid('instance').option('dataSource', aprobacionATUDataSource);
                                                        });
                                                    } else {
                                                        $("#loadPanel").dxLoadPanel('instance').hide();
                                                        var result = MostrarNotificacion('alert', null, 'ERROR: ' + respuesta[1]);
                                                    }
                                                }).fail(function (jqxhr, textStatus, error) {
                                                    $("#loadPanel").dxLoadPanel('instance').hide();
                                                    alert('falla: ' + textStatus + ", " + error);
                                                    $('#grdAprobacionATU').dxDataGrid('instance').option('dataSource', aprobacionATUDataSource);
                                                });
                                            }
                                        });
                                    }
                                }
                            }
                        ).appendTo(cellElement);
                    }
                }
            }, {
                caption: '',
                width: '5%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_ACTUAL == 'S') {
                        $('<div />').dxButton(
                            {
                                icon: 'close',
                                type: 'danger',
                                hint: 'Rechazar Solicitud de Anulación',
                                onClick: function (params) {
                                    if (cellInfo.data.ID_ANULACION_DOC != null && cellInfo.data.ID_ANULACION_DOC > 0) {
                                        var result = DevExpress.ui.dialog.confirm("Está Seguro de Rechazar la Solicitud de Anulación ?", "Confirmación");
                                        result.done(function (dialogResult) {
                                            if (dialogResult) {
                                                $("#loadPanel").dxLoadPanel('instance').show();

                                                $.getJSON($('#app').data('url') + 'Tramites/api/AnulacionDocumentoApi/RechazarDocumento', {
                                                    id: cellInfo.data.ID_ANULACION_DOC
                                                }).done(function (data) {
                                                    var respuesta = data.split(':');

                                                    if (respuesta[0] == 'OK') {
                                                        $("#loadPanel").dxLoadPanel('instance').hide();
                                                        var result = MostrarNotificacion('alert', null, 'Solicitud de Anulación Rechazada Satisfactoriamente.');

                                                        result.done(function (dialogResult) {
                                                            $('#grdAprobacionATU').dxDataGrid('instance').option('dataSource', aprobacionATUDataSource);
                                                        });
                                                    } else {
                                                        $("#loadPanel").dxLoadPanel('instance').hide();
                                                        var result = MostrarNotificacion('alert', null, 'ERROR: ' + respuesta[1]);
                                                    }
                                                }).fail(function (jqxhr, textStatus, error) {
                                                    $("#loadPanel").dxLoadPanel('instance').hide();
                                                    alert('falla: ' + textStatus + ", " + error);
                                                    $('#grdAprobacionATU').dxDataGrid('instance').option('dataSource', aprobacionATUDataSource);
                                                });
                                            }
                                        });
                                    }
                                }
                            }
                        ).appendTo(cellElement);
                    }
                }
            }
        ]
    });
});

var documentosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[]';

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'Tramites/api/AnulacionDocumentoApi/ConsultaDocumentosGenerados', {
            filter: filterOptions,
            sort: sortOptions,
            group: '',
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            noFilterNoRecords: true
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
            $(window).trigger('resize');
        }).fail(function (jqxhr, textStatus, error) {
            alert('falla: ' + textStatus + ", " + error);
        });
        return d.promise();
    },
    byKey: function (key, extra) {
        return key.toString();
    },
});


var solicitudDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        $.getJSON($('#app').data('url') + 'Tramites/api/AnulacionDocumentoApi/ConsultaSolicitudes').done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        });
        return d.promise();
    }
});

var justificacionDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        $.getJSON($('#app').data('url') + 'Tramites/api/AnulacionDocumentoApi/ConsultaDocumentos?tipo=1').done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        });
        return d.promise();
    }
});

var autorizacionDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        $.getJSON($('#app').data('url') + 'Tramites/api/AnulacionDocumentoApi/ConsultaDocumentos?tipo=2').done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        });
        return d.promise();
    }
});

var aprobacionATUDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        $.getJSON($('#app').data('url') + 'Tramites/api/AnulacionDocumentoApi/ConsultaDocumentos?tipo=3').done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        });
        return d.promise();
    }
});

function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        return DevExpress.ui.dialog.alert(msg, 'Anulación de Documento');
    } else {
        return DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}

function abrirDetalleTramite(id) {
    var tramiteInstance = $("#popDetalleTramite").dxPopup("instance");
    tramiteInstance.option('title', 'Detalle Trámite - ' + id);
    tramiteInstance.show();

    $('#frmDetalleTramite').attr('src', null);
    $('#frmDetalleTramite').attr('src', 'https://webservices.metropol.gov.co/SIM/Tramites/WebForms/DetalleT.aspx?idt=' + id);
}

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

function CerrarPopupDocumento() {
    var documentoInstance = $("#popDocumento").dxPopup("instance");
    documentoInstance.hide();

    if (posTab == 0) {
        $('#grdDocumentos').dxDataGrid('instance').option('dataSource', documentosDataSource);
    }
}