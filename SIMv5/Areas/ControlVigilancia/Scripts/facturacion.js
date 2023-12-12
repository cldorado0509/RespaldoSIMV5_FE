'use strict'

let aceptarFactura = false;
let tramitesSel = null;

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
        title: "Documento Radicado",
        fullScreen: true
    });

    var tabsData = [
        { text: 'PENDIENTES FACTURACION', pos: 0 },
        //{ text: 'HISTÓRICO', pos: 1 }
    ];

    $("#popRegistrarFactura").dxPopup({
        title: "Registrar Factura",
        fullScreen: false,
        onHidden: function (e) {
            if (aceptarFactura) {
                aceptarFactura = false;

                $("#loadPanel").dxLoadPanel('instance').show();

                $.getJSON($('#app').data('url') + 'ControlVigilancia/api/FacturacionApi/RegistrarFactura', {
                    Tramites: tramitesSel,
                    Factura: $('#factura').dxTextBox('instance').option('value')
                }).done(function (data) {
                    $("#loadPanel").dxLoadPanel('instance').hide();

                    $('#grdPendientesFacturacion').dxDataGrid('instance').option('dataSource', pendientesFacturacionDataSource);
                }).fail(function (jqxhr, textStatus, error) {
                    $("#loadPanel").dxLoadPanel('instance').hide();
                    alert('falla: ' + textStatus + ", " + error);

                    $('#grdPendientesFacturacion').dxDataGrid('instance').option('dataSource', pendientesFacturacionDataSource);
                });
            }

            $('#factura').dxTextBox('instance').option('value', '');
        }
    });

    $("#tabOpciones").dxTabs({
        dataSource: tabsData,
        onItemClick: (function (itemData) {
            switch (itemData.itemIndex) {
                case 0:
                    $('#tab01').css('display', 'block');
                    $('#tab02').css('display', 'none');

                    $('#grdPendientesFacturacion').dxDataGrid('instance').option('dataSource', pendientesFacturacionDataSource);

                    $(window).trigger('resize');
                    break;
                case 1:
                    $('#tab02').css('display', 'block');
                    $('#tab01').css('display', 'none');

                    $('#grdHistorico').dxDataGrid('instance').option('dataSource', historicoDataSource);

                    $(window).trigger('resize');

                    break;
            }

            posTab = itemData.itemIndex;
        }),
        selectedIndex: 0,
    });

    $("#grdPendientesFacturacion").dxDataGrid({
        dataSource: pendientesFacturacionDataSource,
        allowColumnResizing: true,
        height: '100%',
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            enabled: false,
        },
        filterRow: {
            visible: true,
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
                dataField: "TRAMITES",
                dataType: 'string',
                visible: false
            }, {
                dataField: "CODTRAMITE",
                width: '8%',
                caption: 'TRAMITE',
                dataType: 'number',
            }, {
                dataField: "CODDOCUMENTO",
                width: '5%',
                caption: 'DOC.',
                dataType: 'number',
            }, {
                dataField: 'ASUNTO',
                caption: 'ASUNTO',
                dataType: 'string',
            }, {
                dataField: 'NIT_SOLICITANTE',
                width: '10%',
                caption: 'NIT',
                dataType: 'string',
                visible: true
            }, {
                dataField: 'DIRECCION',
                width: '10%',
                caption: 'DIRECCION',
                dataType: 'string',
            }, {
                dataField: 'TELEFONO',
                width: '8%',
                caption: 'TELEFONO',
                dataType: 'string',
            }, {
                dataField: 'CM',
                width: '8%',
                caption: 'CM',
                dataType: 'string',
            }, {
                dataField: 'FECHARADICADO',
                width: '10%',
                caption: 'FECHA RADICADO',
                dataType: 'date',
                format: 'dd/MM/yyyy'
            }, {
                dataField: 'RADICADO',
                width: '8%',
                caption: 'RADICADO',
                dataType: 'string',
            }, {
                dataField: 'TIPO_APROVECHAMIENTO_FORESTAL',
                width: '15%',
                caption: 'TIPO APROV. FORESTAL',
                dataType: 'string',
            }, {
                caption: '',
                width: '4%',
                alignment: 'center',
                cellTemplate: function (cellElement, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: 'doc',
                            type: 'success',
                            hint: 'Ver Documento Radicado',
                            onClick: function (params) {
                                let documento = $("#popDocumento").dxPopup("instance");
                                documento.show();

                                $('#frmDocumento').attr('src', $('#app').data('url') + 'Tramites/Documento/ConsultarInformeTecnicoRadicado?idTramite=' + cellInfo.data.CODTRAMITE + '&idDocumento=' + cellInfo.data.CODDOCUMENTO + '&descargar=0');
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
                            icon: 'check',
                            type: 'success',
                            hint: 'Marcar Como Facturado',
                            onClick: function (params) {
                                aceptarFactura = false;

                                tramitesSel = cellInfo.data.TRAMITES;

                                var registrarFacuturaPopup = $("#popRegistrarFactura").dxPopup("instance");
                                registrarFacuturaPopup.show();

                                $('#factura').dxTextBox ({
                                    width: '100%',
                                    readOnly: false,
                                    value: ''
                                });

                                $('#aceptarRegistrarFactura').dxButton({
                                    type: 'success',
                                    text: 'Aceptar',
                                    width: '100%',
                                    height: '100%',
                                    onClick: function (params) {
                                        aceptarFactura = true;
                                        registrarFacuturaPopup.hide();
                                    }
                                });
                            }
                        }
                    ).appendTo(cellElement);
                }
            }
        ]
    });
});

var pendientesFacturacionDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        $.getJSON($('#app').data('url') + 'ControlVigilancia/api/FacturacionApi/ConsultaPendientesFacturacion').done(function (data) {
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
