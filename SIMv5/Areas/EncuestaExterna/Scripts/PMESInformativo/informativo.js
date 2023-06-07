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

    $("#popDocumento").dxPopup({
        title: "Documentos",
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

    $("#popDocumentoPreview").dxPopup({
        title: "Documento",
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
        { text: 'EN ELABORACIÓN', pos: 0 },
        { text: 'HISTÓRICO', pos: 1 }
    ];

    $("#tabOpciones").dxTabs({
        dataSource: tabsData,
        onItemClick: (function (itemData) {
            switch (itemData.itemIndex) {
                case 0:
                    $('#tab01').css('display', 'block');
                    $('#tab02').css('display', 'none');

                    $('#grdElaboracion').dxDataGrid('instance').option('dataSource', elaboracionDataSource);

                    $(window).trigger('resize');
                    break;
                case 1:
                    $('#tab02').css('display', 'block');
                    $('#tab01').css('display', 'none');

                    $('#grdDocumentosPublicados').dxDataGrid('instance').option('dataSource', generadosDataSource);

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
                dataField: "ID_INFORMATIVO_DOC",
                width: '8%',
                caption: 'CODIGO',
                dataType: 'number',
                visible: ($('#app').data('codigo') == '1'),
            }, {
                dataField: 'S_TIPOCOMUNICACION',
                width: '20%',
                caption: 'TIPO DE COMUNICACIÓN',
                dataType: 'string',
                visible: true,
            }, {
                dataField: 'S_ORGANIZACION',
                width: '35%',
                caption: 'ORGANIZACION',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'S_DESCRIPCION',
                width: '35%',
                caption: 'DESCRIPCIÓN',
                dataType: 'string',
                visible: true,
            }, {
                dataField: 'D_FECHA',
                width: '10%',
                caption: 'FECHA',
                dataType: 'date',
                format: "dd/MM/yyyy",
                visible: true
            }, {
                caption: '',
                width: '6%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'edit',
                            type: 'success',
                            hint: 'Editar Publicación',
                            onClick: function (params) {
                                if (cellInfo.data.ID_INFORMATIVO_DOC != null && cellInfo.data.ID_INFORMATIVO_DOC > 0) {
                                    var documentoDetalle = $("#popDocumento").dxPopup("instance");
                                    documentoDetalle.show();

                                    $('#frmDocumento').attr('src', $('#app').data('url') + 'EncuestaExterna/PMESInformativo/Documentos?id=' + cellInfo.data.ID_INFORMATIVO_DOC + '&c=@DateTime.Now.ToString("HHmmss")');
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
                            hint: 'Publicar Documentos',
                            onClick: function (params) {
                                if (cellInfo.data.ID_INFORMATIVO_DOC != null && cellInfo.data.ID_INFORMATIVO_DOC > 0) {
                                    var result = DevExpress.ui.dialog.confirm("Está Seguro de Publicar los Documentos ?", "Confirmación");

                                    result.done(function (dialogResult) {
                                        if (dialogResult) {
                                            $("#loadPanel").dxLoadPanel('instance').show();

                                            $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESInformativoApi/PublicarDocumento', {
                                                Id: cellInfo.data.ID_INFORMATIVO_DOC
                                            }).done(function (data) {
                                                $("#loadPanel").dxLoadPanel('instance').hide();

                                                var respuesta = data.split(':');

                                                if (respuesta[0] == 'OK') {
                                                    var result = MostrarNotificacion('alert', null, 'Documentos Publicados Satisfactoriamente.\r\n' + respuesta[1]);
                                                } else {
                                                    var result = MostrarNotificacion('alert', null, 'ERROR: ' + respuesta[1]);
                                                }

                                                $('#grdElaboracion').dxDataGrid('instance').option('dataSource', elaboracionDataSource);
                                            }).fail(function (jqxhr, textStatus, error) {
                                                $("#loadPanel").dxLoadPanel('instance').hide();
                                                alert('falla: ' + textStatus + ", " + error);
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
                                if (cellInfo.data.ID_INFORMATIVO_DOC != null && cellInfo.data.ID_INFORMATIVO_DOC > 0) {
                                    var result = DevExpress.ui.dialog.confirm("Está Seguro de Eliminar la Publicación ?", "Confirmación");

                                    result.done(function (dialogResult) {
                                        if (dialogResult) {
                                            $("#loadPanel").dxLoadPanel('instance').show();

                                            $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESInformativoApi/EliminarPublicacion', {
                                                id: cellInfo.data.ID_INFORMATIVO_DOC
                                            }).done(function (data) {
                                                $("#loadPanel").dxLoadPanel('instance').hide();

                                                var respuesta = data.split(':');

                                                if (respuesta[0] == 'OK') {
                                                        MostrarNotificacion('alert', null, 'Publicación Eliminada Satisfactoriamente.');
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

    $("#grdDocumentosPublicados").dxDataGrid({
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
        columns: [
            {
                dataField: "ID_INFORMATIVO_DOC",
                width: '8%',
                caption: 'CODIGO',
                dataType: 'number',
                visible: ($('#app').data('codigo') == '1'),
            }, {
                dataField: 'S_TIPOCOMUNICACION',
                width: '20%',
                caption: 'TIPO DE COMUNICACIÓN',
                dataType: 'string',
                visible: true,
            }, {
                dataField: 'S_ORGANIZACION',
                width: '35%',
                caption: 'ORGANIZACION',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'S_DESCRIPCION',
                width: '35%',
                caption: 'DESCRIPCIÓN',
                dataType: 'string',
                visible: true,
            }, {
                dataField: 'D_FECHA',
                width: '10%',
                caption: 'FECHA',
                dataType: 'date',
                format: "dd/MM/yyyy",
                visible: true
            }, {
                caption: '',
                width: '6%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'find',
                            type: 'success',
                            hint: 'Ver Publicación',
                            onClick: function (params) {
                                if (cellInfo.data.ID_INFORMATIVO_DOC != null && cellInfo.data.ID_INFORMATIVO_DOC > 0) {
                                    var documentoDetalle = $("#popDocumento").dxPopup("instance");
                                    documentoDetalle.show();

                                    $('#frmDocumento').attr('src', $('#app').data('url') + 'EncuestaExterna/PMESInformativo/Documentos?id=' + cellInfo.data.ID_INFORMATIVO_DOC + '&c=@DateTime.Now.ToString("HHmmss")');
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
                            icon: 'arrowleft',
                            type: 'danger',
                            hint: 'Devolver Publicación',
                            onClick: function (params) {
                                if (cellInfo.data.ID_INFORMATIVO_DOC != null && cellInfo.data.ID_INFORMATIVO_DOC > 0) {
                                    var result = DevExpress.ui.dialog.confirm("Está Seguro de Devolver la Publicación ?", "Confirmación");

                                    result.done(function (dialogResult) {
                                        if (dialogResult) {
                                            $("#loadPanel").dxLoadPanel('instance').show();

                                            $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESInformativoApi/DevolverPublicacion', {
                                                Id: cellInfo.data.ID_INFORMATIVO_DOC
                                            }).done(function (data) {
                                                $("#loadPanel").dxLoadPanel('instance').hide();

                                                var respuesta = data.split(':');

                                                if (respuesta[0] == 'OK') {
                                                    var result = MostrarNotificacion('alert', null, 'Publicación Devuelta Satisfactoriamente.\r\n' + respuesta[1]);
                                                } else {
                                                    var result = MostrarNotificacion('alert', null, 'ERROR: ' + respuesta[1]);
                                                }

                                                $('#grdDocumentosPublicados').dxDataGrid('instance').option('dataSource', generadosDataSource);
                                            }).fail(function (jqxhr, textStatus, error) {
                                                $("#loadPanel").dxLoadPanel('instance').hide();
                                                alert('falla: ' + textStatus + ", " + error);
                                                $('#grdDocumentosPublicados').dxDataGrid('instance').option('dataSource', generadosDataSource);
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

                $('#frmDocumento').attr('src', $('#app').data('url') + 'EncuestaExterna/PMESInformativo/Documentos?c=@DateTime.Now.ToString("HHmmss")');
            }
        });

});

var elaboracionDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESInformativoApi/ConsultaDocumentos?tipo=1').done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        });
        return d.promise();
    }
});

var generadosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESInformativoApi/ConsultaDocumentos?tipo=2').done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        });
        return d.promise();
    }
});


function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        return DevExpress.ui.dialog.alert(msg, 'Proyección Documento');
    } else {
        return DevExpress.ui.notify(msg, typeMsg, 3000);
    }
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
}