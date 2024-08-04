function isNotEmpty(value) {
    return value !== undefined && value !== null && value !== "";
}

var IdCertificado = -1;
var ConActividades = "";
var NombreTercero = "";
$(document).ready(function () {

    $("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
        shadingColor: "rgba(0,0,0,0.4)",
    });

    $("#btnAceptaFirma").dxButton({
        icon: 'check',
        type: 'default',
        text: 'Firmar Certificado',
        onClick: function () {
            $("#loadPanel").dxLoadPanel("instance").show();
            var _Ruta = $('#SIM').data('url') + "Contractual/Api/FirmarCertificadoApi/FirmarCertificado";
            $.getJSON(_Ruta, { IdCert: IdCertificado, IdUsuario: IdUsuario }).done(function (data) {
                if (data.IsSuccess) {
                    $("#grdListaCertificados").dxDataGrid("instance").refresh();
                    DevExpress.ui.dialog.alert('El certificado fue firmado correctamente y se envió al correo ' + data.Correo);
                    popFirmar.hide();
                    var pdfWindow = window.open("");
                    pdfWindow.document.write("'<html><head><title>Certificado para " + data.Tercero + "</title></head><body height='100%' width='100%'><iframe width='100%' height='100%' src='data:application/pdf;base64," + data.Archivo + "'></iframe></body></html>");
                } else {
                    DevExpress.ui.dialog.alert('Ocurrió un problema firmando el certificado para ' + data.Tercero + ': ' + data.Mensaje);
                }
            });
            $("#loadPanel").dxLoadPanel("instance").hide();
        }
    });

    $("#btnRechazaFirma").dxButton({
        icon: 'check',
        type: 'default',
        text: 'NO Firmar Certificado',
        onClick: function () {
            $("#lblFirmar").text("");
            popFirmar.hide();
        }
    });

    var popFirmar = $("#popupFirmar").dxPopup({
        width: 600,
        height: 200,
        showTitle: true,
        title: "Aprobar/Rechazar firma del certificado",
        onShown: function () {
            var _Activ = ConActividades == "Si" ? "con actividades " : " ";
            var _text = "Acepta firmar el certificado contractual " + _Activ + "de " + NombreTercero;
            $('#lblFirmar').text(_text);
        }
    }).dxPopup("instance");

    $("#grdListaCertificados").dxDataGrid({
        dataSource: DsListaCertificados,
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
            { dataField: 'FuncionarioFirma', dataType: 'number', visible: false },
            { dataField: 'IdCertificado', width: '5%', caption: 'Identificador', alignment: 'center', dataType: 'number' },
            { dataField: 'Documento', width: '10%', caption: 'Documento', dataType: 'string' },
            { dataField: 'Tercero', width: '30%', caption: 'Tercero', dataType: 'string' },
            { dataField: 'FechaCertificado', width: '10%', caption: 'Fecha Certificado', dataType: 'date', format: 'dd MMM yyyy' },
            { dataField: 'GeneradoPor', width: '30%', caption: 'Generado por', dataType: 'string' },
            { dataField: 'ConActividades', width: '10%', caption: 'Actividades', dataType: 'string' },
            {
                width: '5%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'trash',
                        hint: 'Eliminar el certificado',
                        type: 'success',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar el certificado de contratos de ' + options.data.Tercero + '?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    $("#loadPanel").dxLoadPanel("instance").show();
                                    var _Ruta = $('#SIM').data('url') + "Contractual/Api/FirmarCertificadoApi/EliminarCertificado";
                                    $.getJSON(_Ruta, { IdCert: options.data.IdCertificado }).done(function (data) {
                                        if (data) {
                                            $("#loadPanel").dxLoadPanel("instance").hide();
                                            DevExpress.ui.dialog.alert('El certificado se eliminó correctamente');
                                            $("#grdListaCertificados").dxDataGrid("instance").refresh();
                                        } else {
                                            $("#loadPanel").dxLoadPanel("instance").hide();
                                            DevExpress.ui.dialog.alert('Ocurrió un problema eliminando el certificado de ' + options.data.Tercero);
                                        }
                                    });
                                    $("#loadPanel").dxLoadPanel("instance").hide();
                                }
                            });
                        }
                    }).appendTo(container);
                }

            },
            {
                width: '5%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'doc',
                        hint: 'Ver el documento',
                        type: 'success',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "Contractual/Api/FirmarCertificadoApi/VerCertificado";
                            $.getJSON(_Ruta, { IdCert: options.data.IdCertificado }).done(function (data) {
                                if (data.IsSuccess) {
                                    var pdfWindow = window.open("");
                                    pdfWindow.document.write("'<html><head><title>Certificado para " + options.data.Tercero + "</title></head><body height='100%' width='100%'><iframe width='100%' height='100%' src='data:application/pdf;base64," + data.Result + "'></iframe></body></html>");
                                } else {
                                    DevExpress.ui.dialog.alert('Ocurrió un problema leyendo el certificado para ' + options.data.Tercero);
                                }
                            });
                        }
                    }).appendTo(container);
                }
            },
            {
                width: '5%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.data.FuncionarioFirma == IdUsuario) {
                        $('<div/>').dxButton({
                            icon: 'check',
                            type: 'success',
                            hint: 'Firmar el certficado',
                            onClick: function (e) {
                                IdCertificado = options.data.IdCertificado;
                                ConActividades = options.data.ConActividades;
                                NombreTercero = options.data.Tercero;
                                popFirmar.show();
                            }
                        }).appendTo(container);
                    }
                }
            }
        ]
    });
});

var DsListaCertificados = new DevExpress.data.CustomStore({
    key: "IdCertificado",
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
        $.getJSON($('#SIM').data('url') + 'Contractual/Api/FirmarCertificadoApi/GetCertificados', params)
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

