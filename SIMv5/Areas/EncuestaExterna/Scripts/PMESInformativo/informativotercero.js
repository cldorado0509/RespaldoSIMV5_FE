let intAno = 0;
let idTercero = null;
let nombreTercero = '';
let itemSeleccionadoTercero = false;

$(document).ready(function () {
    idTercero = $('#app').data('tercero');

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
        }
    });

    $(window).trigger('resize');

    $("#vigencia").dxSelectBox({
        dataSource: null,
        placeholder: "[Vigencia]",
        displayExpr: "VIGENCIA",
        valueExpr: "VIGENCIA",
        searchEnabled: false,
        searchExpr: "VIGENCIA",
        searchTimeout: 0,
        onSelectionChanged: function (e) {
            intAno = e.selectedItem.VIGENCIA;
            $('#grdDocumentosPublicados').dxDataGrid('instance').option('dataSource', generadosDataSource);
        }
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
            }
        ]
    });

    $('#popTercero').dxPopup({
        showTitle: true,
        deferRendering: false,
        title: 'Seleccionar Tercero',
        onHidden: function () {
            if (itemSeleccionadoTercero) {
                itemSeleccionadoTercero = false;

                cboTercerosDataSource.store().clear();
                cboTercerosDataSource.store().insert({ ID_TERCERO: idTercero, S_RSOCIAL: nombreTercero });
                cboTercerosDataSource.load();

                window.location = $('#app').data('url') + 'EncuestaExterna/EncuestaExterna/GestionarEncUsExterno?idestado=0&t=' + idTercero;
            }
        },
    });

    $('#grdTercero').dxDataGrid({
        dataSource: terceroDataSource,
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: false
        },
        filterRow: {
            visible: true,
        },
        groupPanel: {
            visible: false,
        },
        editing: {
            editEnabled: false,
            removeEnabled: false,
            insertEnabled: false
        },
        selection: {
            mode: 'single'
        },
        columns: [
            {
                dataField: 'ID_TERCERO',
                caption: 'ID_TERCERO',
                visible: false,
                dataType: 'number'
            },
            {
                dataField: 'N_DOCUMENTON',
                width: '25%',
                caption: 'DOCUMENTO',
                visible: true,
                dataType: 'number'
            },
            {
                dataField: 'S_RSOCIAL',
                width: '75%',
                caption: 'RAZON SOCIAL',
                visible: true,
                dataType: 'string'
            }
        ],
        onSelectionChanged: function (selecteditems) {
            var data = selecteditems.selectedRowsData[0];
            itemSeleccionadoTercero = true;
            idTercero = data.ID_TERCERO;
            nombreTercero = data.S_RSOCIAL;

            var popup = $('#popTercero').dxPopup('instance');
            popup.hide();
        }
    });

    cboTercerosDataSource.store().clear();
    cboTercerosDataSource.store().insert({ ID_TERCERO: $('#app').data('tercero'), S_RSOCIAL: $('#app').data('nombretercero') });
    cboTercerosDataSource.load();

    $('#cboTercero').dxSelectBox({
        dataSource: cboTercerosDataSource,
        valueExpr: 'ID_TERCERO',
        displayExpr: 'S_RSOCIAL',
        placeholder: '[Seleccionar Tercero]',
        value: idTercero,
        onOpened: function () {
            $('#cboTercero').dxSelectBox('instance').close();
            var popup = $('#popTercero').dxPopup('instance');
            popup.show();

            $('#grdTercero').dxDataGrid('instance').option('dataSource', terceroDataSource);
        }
    });

    $.get($('#app').data('url') + 'EncuestaExterna/api/PMESInformativoApi/Vigencias?t=' + idTercero, function (data) {
        for (var i = 0; i < data.datos.length; i++) {
            vigenciaDataSource.store().insert(data.datos[i]);
        }
        vigenciaDataSource.load();
        $('#vigencia').dxSelectBox('instance').option('dataSource', vigenciaDataSource);

        if (data.datos.length > 0) {
            $('#vigencia').dxSelectBox('instance').option('value', data.datos[0].VIGENCIA);
            intAno = data.datos[0].VIGENCIA;
        }
        
    }, "json");
});

var vigenciaDataSource = new DevExpress.data.DataSource([]);

var cboTercerosDataSource = new DevExpress.data.DataSource([]);

var terceroDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"S_RSOCIAL","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'General/api/TerceroApi/Terceros', {
            filter: filterOptions,
            sort: sortOptions,
            group: groupOptions,
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'l',
            noFilterNoRecords: true
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + error);
        });
        return d.promise();
    },
    byKey: function (key, extra) {
        return key.toString();
    },
});

var generadosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESInformativoApi/ConsultaDocumentosGeneradosTercero?t=' + idTercero + '&ano=' + intAno).done(function (data) {
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