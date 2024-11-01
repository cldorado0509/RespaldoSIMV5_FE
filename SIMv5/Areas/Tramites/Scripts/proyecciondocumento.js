﻿var cargoFirma = null;

var tipoFirma = [{ ID: 0, Nombre: "Firma Propia" }, { ID: 1, Nombre: "Firmaré con funciones de encargo" }, { ID: 2, Nombre: "Ad Hoc" }];

$(document).ready(function () {
    var historicoCargado = false;
    var posTab = 0;
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
        fullScreen: true,
    });

    $("#popIndicesTramites").dxPopup({
        title: "Índices Trámites",
        fullScreen: false,
    });

    $("#popDocumento").dxPopup({
        title: "Proyección Documento",
        fullScreen: true,
        onHiding: function (e) {
            $('#frmDocumento').attr('src', '');
            switch (posTab) {
                case 0:
                    $('#grdElaboracion').dxDataGrid('instance').option('dataSource', elaboracionDataSource);
                    break;
            }
        }
    });

    $("#passwordFirma").dxTextBox({
        mode: "password",
        placeholder: "Contraseña Firma",
        showClearButton: true,
        value: "",
        inputAttr: {
            autocomplete: 'off'
        }    
    });

    $("#btnAceptaFirma").dxButton({
        icon: 'edit',
        type: 'success',
        hint: 'Aceptar',
        onClick: function () {
            var Data = $("#grdFirmas").dxDataGrid("instance").getSelectedRowsData();
            var pss = $("#passwordFirma").dxTextBox("instance").option("value");
            if (pss != "") {
                if (Data[0].ID_PROYECCION_DOC > 0) {
                    $("#loadPanel").dxLoadPanel('instance').show();

                    $.postJSON($('#app').data('url') + 'Tramites/api/ProyeccionDocumentoApi/AvanzarDocumento', {
                        Id: Data[0].ID_PROYECCION_DOC,
                        Siguiente: 1,
                        Comentario: 'Pss;' + pss,
                        Cargo: cargoFirma
                    }).done(function (data) {
                        var respuesta = data.Respuesta.split(':');

                        if (respuesta[0] == 'OK') {
                            $("#passwordFirma").dxTextBox("instance").reset();
                            $("#popFirmaDigital").dxPopup("hide");
                            if (data.Avanzar == '1') {
                                $("#loadPanel").dxLoadPanel('instance').hide();
                                var result = MostrarNotificacion('alert', null, 'Documento Generado y Radicado Satisfactoriamente.\r\n' + respuesta[1] + '. RADICADO GENERADO: ' + data.Radicado);

                                result.done(function (dialogResult) {
                                    var avanzarInstance = $("#popAvanzaTareaTramite").dxPopup("instance");
                                    avanzarInstance.show();

                                    var date = new Date;
                                    $('#frmAvanzaTareaTramite').attr('src', $('#app').data('url') + 'Tramites/Tramites/AvanzaTareaTramite?codTramites=' + data.Tramites + '&tipo=0&origen=AVANZAR&multiTramites=1&c=' + date.getMilliseconds());
                                });
                            } else {
                                $("#passwordFirma").dxTextBox("instance").reset();
                                $("#popFirmaDigital").dxPopup("hide");
                                $("#loadPanel").dxLoadPanel('instance').hide();
                                var result = MostrarNotificacion('alert', null, 'Documento Avanzado Satisfactoriamente.\r\n' + respuesta[1]);
                            }
                        } else {
                            $("#passwordFirma").dxTextBox("instance").reset();
                            $("#popFirmaDigital").dxPopup("hide");
                            $("#loadPanel").dxLoadPanel('instance').hide();
                            var result = MostrarNotificacion('alert', null, 'ERROR: ' + respuesta[1]);
                        }
                        $('#grdFirmas').dxDataGrid('instance').option('dataSource', firmasDataSource);
                    }).fail(function (jqxhr, textStatus, error) {
                        $("#loadPanel").dxLoadPanel('instance').hide();
                        $("#passwordFirma").dxTextBox("instance").reset();
                        $("#popFirmaDigital").dxPopup("hide");
                        alert('falla: ' + textStatus + ", " + error);
                        $('#grdFirmas').dxDataGrid('instance').option('dataSource', firmasDataSource);
                    });
                } else {
                    $("#passwordFirma").dxTextBox("instance").reset();
                    $("#popFirmaDigital").dxPopup("hide");
                    var result = MostrarNotificacion('alert', null, 'ERROR: No se ha seleccionado un documento para firmar');
                }
            } else {
                $("#passwordFirma").dxTextBox("instance").reset();
                $("#popFirmaDigital").dxPopup("hide");
                var result = MostrarNotificacion('alert', null, 'ERROR: No se ha ingresado la clave');
            }
        }
    });

    $("#popFirmaDigital").dxPopup({
        title: "Contraseña de la Firma Digital",
        width: 300,
        height: 300
    });

    $("#popDocumentoPreview").dxPopup({
        title: "Proyección Documento - Vista Previa",
        fullScreen: true,
        onHiding: function (e) {
            switch (posTab) {
                case 0:
                    $('#frmDocumentoPreview').attr('src', null);
                    break;
            }
        }
    });

    $("#popComentario").dxPopup({
        title: "Comentario Devolución",
        fullScreen: false,
        onHidden: function (e) {
            if (aceptarComentario) {
                aceptarComentario = false;

                $("#loadPanel").dxLoadPanel('instance').show();

                $.postJSON($('#app').data('url') + 'Tramites/api/ProyeccionDocumentoApi/AvanzarDocumento', {
                    Id: idActual,
                    Siguiente: 0,
                    Comentario: $('#comentario').dxTextArea('instance').option('value')
                }).done(function (data) {
                    $("#loadPanel").dxLoadPanel('instance').hide();

                    var respuesta = data.Respuesta.split(':');

                    if (respuesta[0] == 'OK') {
                        var result = MostrarNotificacion('alert', null, 'Devolución Realizada Satisfactoriamente.\r\n' + respuesta[1]);
                    } else {
                        var result = MostrarNotificacion('alert', null, 'ERROR: ' + respuesta[1]);
                    }

                    $('#grdFirmas').dxDataGrid('instance').option('dataSource', firmasDataSource);
                }).fail(function (jqxhr, textStatus, error) {
                    $("#loadPanel").dxLoadPanel('instance').hide();
                    alert('falla: ' + textStatus + ", " + error);
                    $('#grdFirmas').dxDataGrid('instance').option('dataSource', firmasDataSource);
                });
            }

            $('#comentario').dxTextArea('instance').option('value', '');
        }
    });

    var tabsData = [
        { text: 'EN ELABORACIÓN', pos: 0 },
        { text: 'PENDIENTE FIRMAS', pos: 1 },
        { text: 'HISTÓRICO', pos: 2 }
    ];

    $("#tabOpciones").dxTabs({
        dataSource: tabsData,
        onItemClick: (function (itemData) {
            switch (itemData.itemIndex) {
                case 0:
                    $('#tab01').css('display', 'block');
                    $('#tab02').css('display', 'none');
                    $('#tab03').css('display', 'none');

                    $('#grdElaboracion').dxDataGrid('instance').option('dataSource', elaboracionDataSource);

                    $(window).trigger('resize');
                    break;
                case 1:
                    $('#tab02').css('display', 'block');
                    $('#tab01').css('display', 'none');
                    $('#tab03').css('display', 'none');

                    $('#grdFirmas').dxDataGrid('instance').option('dataSource', firmasDataSource);

                    $(window).trigger('resize');

                    break;
                case 2:
                    $('#tab03').css('display', 'block');
                    $('#tab01').css('display', 'none');
                    $('#tab02').css('display', 'none');

                    $('#grdDocumentosGenerados').dxDataGrid('instance').option('dataSource', generadosDataSource);

                    $(window).trigger('resize');
                    break;
            }

            posTab = itemData.itemIndex;
        }),
        selectedIndex: 0,
    });

    $("#grdElaboracion").dxDataGrid({
        dataSource: elaboracionDataSource,
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
            mode: 'single'
        },
        columns: [
            {
                dataField: "ID_PROYECCION_DOC",
                width: '8%',
                caption: 'CODIGO',
                dataType: 'number',
                visible: ($('#app').data('codigo') == '1'),
            }, {
                dataField: 'S_DESCRIPCION',
                width: '30%',
                caption: 'DOCUMENTO',
                dataType: 'string',
                visible: true,
            }, {
                dataField: 'S_SERIE',
                width: '15%',
                caption: 'SERIE DOCUMENTAL',
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
                dataField: 'D_FECHA_TRAMITE',
                width: '10%',
                caption: 'FECHA',
                dataType: 'date',
                format: "dd/MM/yyyy HH:mm",
                visible: true
            }, {
                dataField: 'S_PROCESOS',
                width: '18%',
                caption: 'PROCESO',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'S_FUNCIONARIO',
                width: '17%',
                caption: 'FUNCIONARIO',
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
                            hint: 'Editar Documento',
                            onClick: function (params) {
                                if (cellInfo.data.ID_PROYECCION_DOC != null && cellInfo.data.ID_PROYECCION_DOC > 0) {
                                    var documentoDetalle = $("#popDocumento").dxPopup("instance");
                                    documentoDetalle.show();

                                    $('#frmDocumento').attr('src', $('#app').data('url') + 'Tramites/ProyeccionDocumento/Documento?id=' + cellInfo.data.ID_PROYECCION_DOC + '&c=@DateTime.Now.ToString("HHmmss")');
                                }
                            }
                        }
                    ).appendTo(cellElement);
                }
            }, {
                caption: '',
                width: '6%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'search',
                            type: 'success',
                            hint: 'Vista Preliminar',
                            onClick: function (params) {
                                if (cellInfo.data.ID_PROYECCION_DOC != null && cellInfo.data.ID_PROYECCION_DOC > 0) {
                                    var documentoPreview = $("#popDocumentoPreview").dxPopup("instance");
                                    documentoPreview.show();

                                    $('#frmDocumentoPreview').attr('src', $('#app').data('url') + 'Utilidades/Documento?url=' + $('#app').data('url') + 'Tramites/ProyeccionDocumento/DocumentoPreview?id=' + cellInfo.data.ID_PROYECCION_DOC + '&c=@DateTime.Now.ToString("HHmmss")');
                                }
                            }
                        }
                    ).appendTo(cellElement);
                }
            }, {
                caption: '',
                width: '6%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'arrowright',
                            type: 'success',
                            hint: 'Avanzar para Firmas',
                            onClick: function (params) {
                                if (cellInfo.data.ID_PROYECCION_DOC != null && cellInfo.data.ID_PROYECCION_DOC > 0) {
                                    var result = DevExpress.ui.dialog.confirm("Está Seguro de Avanzar el Documento ?", "Confirmación");

                                    result.done(function (dialogResult) {
                                        if (dialogResult) {
                                            $("#loadPanel").dxLoadPanel('instance').show();

                                            $.postJSON($('#app').data('url') + 'Tramites/api/ProyeccionDocumentoApi/AvanzarDocumento', {
                                                Id: cellInfo.data.ID_PROYECCION_DOC,
                                                Siguiente: 1
                                            }).done(function (data) {
                                                $("#loadPanel").dxLoadPanel('instance').hide();

                                                var respuesta = data.Respuesta.split(':');

                                                if (respuesta[0] == 'OK') {
                                                    var result = MostrarNotificacion('alert', null, 'Documento Avanzado Satisfactoriamente.\r\n' + respuesta[1]);

                                                    result.done(function (dialogResult) {
                                                        if (data.TramitesIndices != null && data.TramitesIndices != '') {
                                                            var indicesTramites = $("#popIndicesTramites").dxPopup("instance");
                                                            indicesTramites.show();

                                                            $('#frmIndicesTramites').attr('src', $('#app').data('url') + 'Tramites/ProyeccionDocumento/IndicesTramites?t=' + data.TramitesIndices);
                                                        }
                                                    });
                                                } else {
                                                    var result = MostrarNotificacion('alert', null, 'ERROR: ' + respuesta[1]);
                                                }

                                                $('#grdElaboracion').dxDataGrid('instance').option('dataSource', elaboracionDataSource);
                                            }).fail(function (jqxhr, textStatus, error) {
                                                $("#loadPanel").dxLoadPanel('instance').hide();
                                                alert('falla: ' + textStatus + ", " + error, ", " + jqxhr);
                                                $('#grdElaboracion').dxDataGrid('instance').option('dataSource', elaboracionDataSource);
                                            });
                                        }
                                    });
                                }
                            }
                        }
                    ).appendTo(cellElement);
                }
            }, {
                dataField: 'S_DEVUELTO',
                caption: '',
                width: '6%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_DEVUELTO == 'S') {
                        $('<div />').dxButton(
                            {
                                icon: 'comment',
                                type: 'success',
                                hint: 'Comentarios',
                                onClick: function (params) {
                                    if (cellInfo.data.ID_PROYECCION_DOC != null && cellInfo.data.ID_PROYECCION_DOC > 0) {
                                        aceptarComentario = false;

                                        $.getJSON($('#app').data('url') + 'Tramites/api/ProyeccionDocumentoApi/ComentariosDocumento', {
                                            id: cellInfo.data.ID_PROYECCION_DOC
                                        }).done(function (data) {
                                            var comentarioPopup = $("#popComentario").dxPopup("instance");
                                            comentarioPopup.show();

                                            $('#comentario').dxTextArea({
                                                width: '100%',
                                                height: '90%',
                                                readOnly: true,
                                                value: data
                                            });

                                            $('#aceptarComentario').dxButton(
                                                {
                                                    type: 'success',
                                                    text: 'Aceptar',
                                                    width: '100%',
                                                    height: '100%',
                                                    onClick: function (params) {
                                                        comentarioPopup.hide();
                                                    }
                                                }
                                            );
                                        }).fail(function (jqxhr, textStatus, error) {
                                            alert('falla: ' + textStatus + ", " + error);
                                        });
                                    }
                                }
                            }
                        ).appendTo(cellElement);
                    }
                }
            }, {
                caption: '',
                width: '6%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'close',
                            type: 'danger',
                            hint: 'Eliminar',
                            onClick: function (params) {
                                if (cellInfo.data.ID_PROYECCION_DOC != null && cellInfo.data.ID_PROYECCION_DOC > 0) {
                                    var result = DevExpress.ui.dialog.confirm("Está Seguro de Eliminar el Documento ?", "Confirmación");

                                    result.done(function (dialogResult) {
                                        if (dialogResult) {
                                            $("#loadPanel").dxLoadPanel('instance').show();

                                            $.getJSON($('#app').data('url') + 'Tramites/api/ProyeccionDocumentoApi/EliminarDocumento', {
                                                id: cellInfo.data.ID_PROYECCION_DOC
                                            }).done(function (data) {
                                                $("#loadPanel").dxLoadPanel('instance').hide();

                                                var respuesta = data.split(':');

                                                if (respuesta[0] == 'OK') {
                                                        MostrarNotificacion('alert', null, 'Documento Eliminado Satisfactoriamente.');
                                                } else {
                                                    var result = MostrarNotificacion('alert', null, 'ERROR: ' + respuesta[1]);
                                                }

                                                $('#grdElaboracion').dxDataGrid('instance').option('dataSource', elaboracionDataSource);
                                            }).fail(function (jqxhr, textStatus, error) {
                                                alert('falla: ' + textStatus + ", " + error);
                                            });
                                        }
                                    });
                                }
                            }
                        }
                    ).appendTo(cellElement);
                }
            }
        ]
    });

    $("#grdFirmas").dxDataGrid({
        dataSource: null,
        allowColumnResizing: true,
        height: '100%',
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            enabled: false
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
                dataField: "ID_PROYECCION_DOC",
                width: '5%',
                caption: 'CODIGO',
                dataType: 'number',
                visible: ($('#app').data('codigo') == '1'),
            }, {
                dataField: 'S_DESCRIPCION',
                width: '17%',
                caption: 'DOCUMENTO',
                dataType: 'string',
                visible: true,
            }, {
                dataField: 'S_SERIE',
                width: '13%',
                caption: 'SERIE DOCUMENTAL',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'S_TRAMITES',
                width: '10%',
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
                dataField: 'D_FECHA_TRAMITE',
                width: '10%',
                caption: 'FECHA',
                dataType: 'date',
                format: "dd/MM/yyyy HH:mm",
                visible: true
            }, {
                dataField: 'S_PROCESOS',
                width: '15%',
                caption: 'PROCESO',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'S_FUNCIONARIO',
                width: '15%',
                caption: 'FUNC. DOCUMENTO',
                dataType: 'string',
            }, {
                dataField: 'S_FUNCIONARIO_ACTUAL',
                width: '15%',
                caption: 'FUNC. FIRMA',
                dataType: 'string',
            }, {
                caption: '',
                width: '5%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'edit',
                            type: 'success',
                            hint: 'Ver Detalle',
                            onClick: function (params) {
                                if (cellInfo.data.ID_PROYECCION_DOC != null && cellInfo.data.ID_PROYECCION_DOC > 0) {
                                    var documentoDetalle = $("#popDocumento").dxPopup("instance");
                                    documentoDetalle.show();

                                    $('#frmDocumento').attr('src', $('#app').data('url') + 'Tramites/ProyeccionDocumento/Documento?id=' + cellInfo.data.ID_PROYECCION_DOC + '&c=@DateTime.Now.ToString("HHmmss")');
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
                    $('<div />').dxButton(
                        {
                            icon: 'search',
                            type: 'success',
                            hint: 'Vista Preliminar',
                            onClick: function (params) {
                                if (cellInfo.data.ID_PROYECCION_DOC != null && cellInfo.data.ID_PROYECCION_DOC > 0) {
                                    var documentoPreview = $("#popDocumentoPreview").dxPopup("instance");
                                    documentoPreview.show();

                                    $('#frmDocumentoPreview').attr('src', $('#app').data('url') + 'Tramites/ProyeccionDocumento/DocumentoPreview?id=' + cellInfo.data.ID_PROYECCION_DOC + '&c=@DateTime.Now.ToString("HHmmss")');
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
                                hint: 'Aceptar Firma y Avanzar',
                                onClick: function (params) {
                                    cargo = null;
                                    if (cellInfo.data.ID_PROYECCION_DOC != null && cellInfo.data.ID_PROYECCION_DOC > 0) {
                                        //var result = DevExpress.ui.dialog.confirm("Está Seguro de Aprobar la Firma del Documento ?", "Confirmación");

                                        var customDialog = DevExpress.ui.dialog.custom({
                                            title: "Confirmación",
                                            message: "Está Seguro de Aprobar la Firma del Documento ?&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</br></br><div id=\"tipoFirma\"></div></br></br><div id=\"cboCargo\"></div>",
                                            buttons: [
                                                {
                                                    //text: "Sí", type: 'normal', onClick: function () { return 'S|' + (encargoCheckbox.option('value') ? 'E|' + (cargo.option('value') ?? '-1'): 'N') ; }
                                                    text: "Sí", type: 'normal', onClick: function () { return 'S|' + (tipoFirmaOption.option('value') > 0 ? (tipoFirmaOption.option('value') == 1 ? 'E' : 'A') + '|' + (cargo.option('value') ?? '-1') : 'N'); }
                                                },
                                                {
                                                    text: "No", type: 'normal', onClick: function () { return 'N'; }
                                                },
                                            ]
                                        });

                                        customDialog.show().done(function (dialogResult) {
                                            var respuestaConfirmacion = dialogResult.split('|');

                                            if (respuestaConfirmacion[0] == 'S')
                                            {
                                                if (respuestaConfirmacion[1] == 'E' && respuestaConfirmacion[2] != '-1')
                                                    cargoFirma = respuestaConfirmacion[2];

                                                if (respuestaConfirmacion[1] == 'A' && respuestaConfirmacion[2] != '-1')
                                                    cargoFirma = respuestaConfirmacion[2];

                                                $("#loadPanel").dxLoadPanel('instance').show();

                                                $.postJSON($('#app').data('url') + 'Tramites/api/ProyeccionDocumentoApi/AvanzarDocumento', {
                                                    Id: cellInfo.data.ID_PROYECCION_DOC,
                                                    Siguiente: 1,
                                                    Cargo: cargoFirma,
                                                    TipoFirma: respuestaConfirmacion[1]
                                                }).done(function (data) {
                                                    var respuesta = data.Respuesta.split(':');

                                                    if (respuesta[0] == 'OK') {
                                                        if (data.Avanzar == '1') {
                                                            $("#loadPanel").dxLoadPanel('instance').hide();
                                                            var result = MostrarNotificacion('alert', null, 'Documento Generado y Radicado Satisfactoriamente.\r\n' + respuesta[1] + '. RADICADO GENERADO: ' + data.Radicado);

                                                            result.done(function (dialogResult) {
                                                                var avanzarInstance = $("#popAvanzaTareaTramite").dxPopup("instance");
                                                                avanzarInstance.show();

                                                                var date = new Date;
                                                                $('#frmAvanzaTareaTramite').attr('src', $('#app').data('url') + 'Tramites/Tramites/AvanzaTareaTramite?codTramites=' + data.Tramites + '&tipo=0&origen=AVANZAR&multiTramites=1&c=' + date.getMilliseconds());
                                                            });
                                                        } else {
                                                            $("#loadPanel").dxLoadPanel('instance').hide();
                                                            var result = MostrarNotificacion('alert', null, 'Documento Avanzado Satisfactoriamente.\r\n' + respuesta[1]);
                                                        }
                                                    } else {
                                                        if (respuesta[1] == "Firma Digital") {
                                                            $("#loadPanel").dxLoadPanel('instance').hide();
                                                            $("#popFirmaDigital").dxPopup("show");
                                                        } else {
                                                            $("#loadPanel").dxLoadPanel('instance').hide();
                                                            var result = MostrarNotificacion('alert', null, 'ERROR: ' + respuesta[1]);
                                                        }
                                                    }

                                                    $('#grdFirmas').dxDataGrid('instance').option('dataSource', firmasDataSource);
                                                }).fail(function (jqxhr, textStatus, error) {
                                                    $("#loadPanel").dxLoadPanel('instance').hide();
                                                    alert('falla: ' + textStatus + ", " + error);
                                                    $('#grdFirmas').dxDataGrid('instance').option('dataSource', firmasDataSource);
                                                });
                                            }
                                        });

                                        /*var encargoCheckbox = $('#chkEncargo').dxCheckBox({
                                            text: 'Firmaré con Funciones de Encargo',
                                            value: false,
                                            onValueChanged: function (e) {
                                                cargo.option('visible', encargoCheckbox.option('value') || adhocCheckbox.option('value'));
                                            }
                                        }).dxCheckBox("instance");

                                        var adhocCheckbox = $('#chkAdhoc').dxCheckBox({
                                            text: 'Firmaré Adhoc',
                                            value: false,
                                            onValueChanged: function (e) {
                                                cargo.option('visible', encargoCheckbox.option('value') || adhocCheckbox.option('value'));
                                            }
                                        }).dxCheckBox("instance");*/

                                        var tipoFirmaOption = $("#tipoFirma").dxRadioGroup({
                                            items: tipoFirma,
                                            valueExpr: 'ID',
                                            displayExpr: 'Nombre',
                                            layout: "horizontal",
                                            value: 0,
                                            onOptionChanged: function (e) {
                                                cargo.option('visible', e.value > 0);
                                            }
                                        }).dxRadioGroup('instance');

                                        var cargo = $('#cboCargo').dxLookup({
                                            dataSource: cargosDataSource,
                                            placeholder: '[Seleccionar Cargo]',
                                            title: 'Cargo',
                                            displayExpr: 'NOMBRE',
                                            valueExpr: 'CODCARGO',
                                            cancelButtonText: 'Cancelar',
                                            pageLoadingText: 'Cargando...',
                                            refreshingText: 'Refrescando...',
                                            searchPlaceholder: 'Buscar',
                                            noDataText: 'Sin Datos',
                                            visible: false,
                                        }).dxLookup("instance");


                                        /*result.done(function (dialogResult) {
                                            if (dialogResult) {
                                                $("#loadPanel").dxLoadPanel('instance').show();

                                                $.postJSON($('#app').data('url') + 'Tramites/api/ProyeccionDocumentoApi/AvanzarDocumento', {
                                                    Id: cellInfo.data.ID_PROYECCION_DOC,
                                                    Siguiente: 1
                                                }).done(function (data) {
                                                    var respuesta = data.Respuesta.split(':');

                                                    if (respuesta[0] == 'OK') {
                                                        if (data.Avanzar == '1') {
                                                            $("#loadPanel").dxLoadPanel('instance').hide();
                                                            var result = MostrarNotificacion('alert', null, 'Documento Generado y Radicado Satisfactoriamente.\r\n' + respuesta[1] + '. RADICADO GENERADO: ' + data.Radicado);

                                                            result.done(function (dialogResult) {
                                                                var avanzarInstance = $("#popAvanzaTareaTramite").dxPopup("instance");
                                                                avanzarInstance.show();

                                                                var date = new Date;
                                                                $('#frmAvanzaTareaTramite').attr('src', $('#app').data('url') + 'Tramites/Tramites/AvanzaTareaTramite?codTramites=' + data.Tramites + '&tipo=0&origen=AVANZAR&multiTramites=1&c=' + date.getMilliseconds());
                                                            });
                                                        } else {
                                                            $("#loadPanel").dxLoadPanel('instance').hide();
                                                            var result = MostrarNotificacion('alert', null, 'Documento Avanzado Satisfactoriamente.\r\n' + respuesta[1]);
                                                        }
                                                    } else {
                                                        if (respuesta[1] == "Firma Digital") {
                                                            $("#loadPanel").dxLoadPanel('instance').hide();
                                                            $("#popFirmaDigital").dxPopup("show");
                                                        } else {
                                                            $("#loadPanel").dxLoadPanel('instance').hide();
                                                            var result = MostrarNotificacion('alert', null, 'ERROR: ' + respuesta[1]);
                                                        }
                                                    }

                                                    $('#grdFirmas').dxDataGrid('instance').option('dataSource', firmasDataSource);
                                                }).fail(function (jqxhr, textStatus, error) {
                                                    $("#loadPanel").dxLoadPanel('instance').hide();
                                                    alert('falla: ' + textStatus + ", " + error);
                                                    $('#grdFirmas').dxDataGrid('instance').option('dataSource', firmasDataSource);
                                                });
                                            }
                                        });*/
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
                                icon: 'arrowleft',
                                type: 'danger',
                                hint: 'Devolver Documento',
                                onClick: function (params) {
                                    if (cellInfo.data.ID_PROYECCION_DOC != null && cellInfo.data.ID_PROYECCION_DOC > 0) {
                                        var result = DevExpress.ui.dialog.confirm("Está Seguro de Devolver el Documento ?", "Confirmación");
                                        result.done(function (dialogResult) {
                                            if (dialogResult) {
                                                idActual = cellInfo.data.ID_PROYECCION_DOC;
                                                aceptarComentario = false;

                                                var comentarioPopup = $("#popComentario").dxPopup("instance");
                                                comentarioPopup.show();

                                                $('#comentario').dxTextArea({
                                                    width: '100%',
                                                    height: '90%',
                                                    value: '',
                                                    maxLength: 980,
                                                    readOnly: false
                                                });

                                                $('#aceptarComentario').dxButton(
                                                    {
                                                        type: 'success',
                                                        text: 'Aceptar',
                                                        width: '100%',
                                                        height: '100%',
                                                        onClick: function (params) {
                                                            aceptarComentario = true;
                                                            comentarioPopup.hide();
                                                        }
                                                    }
                                                );

                                                /*$('#cancelarComentario').dxButton(
                                                    {
                                                        type: 'danger',
                                                        text: 'Cancelar',
                                                        width: '100%',
                                                        height: '100%',
                                                        onClick: function (params) {
                                                            aceptarComentario = false;
                                                            comentarioPopup.hide();
                                                        }
                                                    }
                                                );*/
                                            }
                                        });
                                    }
                                }
                            }
                        ).appendTo(cellElement);
                    }
                }
            }, {
                dataField: 'S_DEVUELTO',
                caption: '',
                width: '5%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_DEVUELTO == 'S') {
                        $('<div />').dxButton(
                            {
                                icon: 'comment',
                                type: 'success',
                                hint: 'Comentarios',
                                onClick: function (params) {
                                    if (cellInfo.data.ID_PROYECCION_DOC != null && cellInfo.data.ID_PROYECCION_DOC > 0) {
                                        aceptarComentario = false;

                                        $.getJSON($('#app').data('url') + 'Tramites/api/ProyeccionDocumentoApi/ComentariosDocumento', {
                                            id: cellInfo.data.ID_PROYECCION_DOC
                                        }).done(function (data) {
                                            var comentarioPopup = $("#popComentario").dxPopup("instance");
                                            comentarioPopup.show();

                                            $('#comentario').dxTextArea({
                                                width: '100%',
                                                height: '90%',
                                                readOnly: true,
                                                value: data
                                            });

                                            $('#aceptarComentario').dxButton(
                                                {
                                                    type: 'success',
                                                    text: 'Aceptar',
                                                    width: '100%',
                                                    height: '100%',
                                                    onClick: function (params) {
                                                        comentarioPopup.hide();
                                                    }
                                                }
                                            );
                                        }).fail(function (jqxhr, textStatus, error) {
                                            alert('falla: ' + textStatus + ", " + error);
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

    $("#grdDocumentosGenerados").dxDataGrid({
        dataSource: null,
        allowColumnResizing: true,
        height: '100%',
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            enabled: true,
            pageSize: 20,
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
        export: {
            enabled: true,
            fileName: "Reporte",
            allowExportSelectedData: false
        },
        remoteOperations: true,
        onRowPrepared: function (info) {
            if (info.rowType == "data") {
                if (info.data.S_FINALIZADO == 'N') {
                    info.rowElement.css('background', 'LightGreen');
                } else if (info.data.S_ESTADODOC == 'A') {
                    info.rowElement.css('background', '#FAAFBA');
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
                width: '4%',
                caption: 'A',
                dataType: 'number',
                filterOperations: [],
                visible: true,
            }, {
                dataField: "ID_PROYECCION_DOC",
                width: '8%',
                caption: 'CÓDIGO',
                dataType: 'number',
                visible: ($('#app').data('codigo') == '1'),
            }, {
                dataField: 'S_DESCRIPCION',
                caption: 'DOCUMENTO',
                dataType: 'string',
                visible: true,
            }, {
                dataField: 'S_SERIE',
                width: '15%',
                caption: 'SERIE DOCUMENTAL',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'S_RADICADO',
                width: '7%',
                caption: 'RADICADO',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'S_TRAMITES',
                width: '10%',
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
                width: '10%',
                caption: 'PROCESOS',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'S_ESTADO_FLUJO',
                width: '10%',
                caption: 'ESTADO',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'S_FUNCIONARIO',
                width: '15%',
                caption: 'RESPONSABLE',
                dataType: 'string',
            }, {
                dataField: 'PF',
                width: '3%',
                caption: 'P/F',
                dataType: 'string',
            }, {
                dataField: 'S_ESTADODOC',
                width: '2%',
                caption: '',
                dataType: 'string',
            }, {
                caption: '',
                width: '5%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'search',
                            type: 'success',
                            onClick: function (params) {
                                if (cellInfo.data.ID_PROYECCION_DOC != null && cellInfo.data.ID_PROYECCION_DOC > 0) {
                                    if (cellInfo.data.S_FINALIZADO == 'S') {
                                        var documentoDetalle = $("#popDocumento").dxPopup("instance");
                                        documentoDetalle.show();

                                        $('#frmDocumento').attr('src', $('#app').data('url') + 'Tramites/ProyeccionDocumento/DocumentoFinal?id=' + cellInfo.data.ID_PROYECCION_DOC + '&c=@DateTime.Now.ToString("HHmmss")');
                                    } else {
                                        var documentoPreview = $("#popDocumentoPreview").dxPopup("instance");
                                        documentoPreview.show();

                                        $('#frmDocumentoPreview').attr('src', $('#app').data('url') + 'Tramites/ProyeccionDocumento/DocumentoPreview?id=' + cellInfo.data.ID_PROYECCION_DOC + '&c=@DateTime.Now.ToString("HHmmss")');
                                    }
                                }
                            }
                        }
                    ).appendTo(cellElement);
                }
            }
        ]
    });

    $('#agregarDocumento').dxButton(
        {
            icon: 'plus',
            text: '',
            width: '30x',
            type: 'success',
            elementAttr: {
                style: "float: right;"
            },
            onClick: function (params) {
                var aprobacionInstance = $("#popDocumento").dxPopup("instance");
                aprobacionInstance.show();

                $('#frmDocumento').attr('src', $('#app').data('url') + 'Tramites/ProyeccionDocumento/Documento?c=@DateTime.Now.ToString("HHmmss")');
            }
        });

});

var elaboracionDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        $.getJSON($('#app').data('url') + 'Tramites/api/ProyeccionDocumentoApi/ConsultaDocumentos?tipo=1').done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        });
        return d.promise();
    }
});

var firmasDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        $.getJSON($('#app').data('url') + 'Tramites/api/ProyeccionDocumentoApi/ConsultaDocumentos?tipo=2').done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        });
        return d.promise();
    }
});

/*var generadosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"S_RSOCIAL","desc":false}]';

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);

        $.getJSON($('#app').data('url') + 'Tramites/api/ProyeccionDocumentoApi/ConsultaDocumentos?tipo=3').done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        });
        return d.promise();
    }
});*/

var generadosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[]';

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'Tramites/api/ProyeccionDocumentoApi/ConsultaDocumentosGenerados', {
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


function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        return DevExpress.ui.dialog.alert(msg, 'Proyección Documento');
    } else {
        return DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}

function abrirDetalleTramite(id) {
    var tramiteInstance = $("#popDetalleTramite").dxPopup("instance");
    tramiteInstance.option('title', 'Detalle Trámite - ' + id);
    tramiteInstance.show();

    $('#frmDetalleTramite').attr('src', null);
    //$('#frmDetalleTramite').attr('src', 'https://webservices.metropol.gov.co/SIM/Tramites/WebForms/DetalleT.aspx?idt=' + id);
    $('#frmDetalleTramite').attr('src', $('#app').data('url') + 'Utilidades/DetalleTramite?popup=true&CodTramite=' + id);
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

function CerrarPopup(popUp, estado, origen, codTramites, codTarea, codTareaSiguiente, codFuncionario, copias) {
    var avanzaTareaInstance = $("#popAvanzaTareaTramite").dxPopup("instance");
    avanzaTareaInstance.hide();

    if (estado == 'OK') {
        mensajeAlmacenamiento("Los Trámites fueron avanzados Satisfactoriamente.");
        $('#grdFirmas').dxDataGrid('instance').option('dataSource', firmasDataSource);
    }
    else if (estado == 'ERROR') {
        mensajeAlmacenamiento("Por lo menos uno de los Trámites NO fue avanzado Satisfactoriamente.");
    }
}

function CerrarPopupDocumento() {
    var documentoInstance = $("#popDocumento").dxPopup("instance");
    documentoInstance.hide();
}

var cargosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var searchValueOptions = loadOptions.searchValue;
        var searchExprOptions = loadOptions.searchExpr;

        var skip = (loadOptions.skip ? loadOptions.skip : 0);
        var take = (loadOptions.take ? loadOptions.take : 0);

        if (take != 0) {
            $.getJSON($('#app').data('url') + 'Tramites/api/ProyeccionDocumentoApi/Cargos', {
                filter: '',
                sort: '[{"selector":"NOMBRE","desc":false}]',
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
