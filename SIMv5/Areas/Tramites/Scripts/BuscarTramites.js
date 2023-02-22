$(document).ready(function () {
    var CodTramite = -1;

    $("#btnBuscarTra").dxButton({
        text: "Buscar Trámites",
        icon: "search",
        type: "default",
        width: "190",
        onClick: function () {
            var _popup = $("#popupBuscaTra").dxPopup("instance");
            _popup.show();
            $('#BuscarTra').attr('src', $('#SIM').data('url') + 'Utilidades/BuscarTramite?popup=true');
        }
    });

    $("#popupBuscaTra").dxPopup({
        width: 900,
        height: 800,
        showTitle: true,
        title: "Buscar Trámites del SIM"
    });

    $("#grdTarmite").dxDataGrid({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "CODTRAMITE",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Tramites/api/TramitesApi/ObtieneTramite?CodTramite=" + CodTramite);
                }
            })
        }),
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            pageSize: 10
        },
        selection: {
            mode: 'single'
        },
        remoteOperations: true,
        hoverStateEnabled: true,
        columns: [
            { dataField: "CODTRAMITE", width: '5%', caption: 'Codigo del Trámite', dataType: 'string' },
            { dataField: 'VITAL', width: '5%', caption: 'VITAL', dataType: 'string' },
            { dataField: 'EXPEDIENTE', width: '5%', caption: 'Expediente', dataType: 'string' },
            { dataField: 'PROCESO', width: '15%', caption: 'Tipo de Trámite', dataType: 'string' },
            { dataField: 'TAREA', width: '15%', caption: 'Tarea', dataType: 'string' },
            { dataField: 'ASUNTO', width: '15%', caption: 'Asunto', dataType: 'string' },
            { dataField: 'INICIOTRAMITE', width: '10%', caption: 'Inicio Trámite', dataType: 'date', allowSearch: false, format: 'dd/MM/yyyy HH:mm' },
            { dataField: 'INICIOTAREA', width: '10%', caption: 'Inicio Tarea', dataType: 'date', allowSearch: false, format: 'dd/MM/yyyy HH:mm' },
            { dataField: 'FINTAREA', width: '10%', caption: 'Final Tarea', dataType: 'date', allowSearch: false, format: 'dd/MM/yyyy HH:mm', visible: false },
            { dataField: "TIPO", width: '10%', caption: "Tipo", dataType: 'string' },
            { dataField: 'MARCAR', dataType: 'string', visible: false, allowSearch: false },
            { dataField: 'COLOR', dataType: 'string', visible: false, allowSearch: false },
            {
                with: '8%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'fields',
                        hint: 'Detalle trámite',
                        onClick: function (e) {
                            if (options.data.CODTRAMITE > 0) {
                                var popupOpciones = {
                                    height: 600,
                                    width: 1100,
                                    title: 'Detalle del trámite',
                                    visible: false,
                                    contentTemplate: function (container) {
                                        $("<iframe>").attr("src", $('#SIM').data('url') + 'Utilidades/DetalleTramite?popup=true&CodTramite=' + options.data.CODTRAMITE).attr("width", "100%").attr("height", "100%").attr("frameborder", "0").attr("scrolling", "0").appendTo(container);
                                    }
                                }
                                var popupTra = $("#popDetalleTramite").dxPopup(popupOpciones).dxPopup("instance");
                                $("#popDetalleTramite").css({ 'visibility': 'visible' });
                                $("#popDetalleTramite").fadeTo("slow", 1);
                                popupTra.show();
                            }
                        }
                    }).appendTo(container);
                }
            }
        ],
        onRowPrepared: function (e) {
            if (e.rowType !== "data")
                return
            if (e.data.MARCAR == "1") {
                e.rowElement.css("background-color", e.data.COLOR);
                e.rowElement.css("color", "white");
            }
        }
    });
});

function SelTramite(CodTramite) {
    var _popup = $("#popupBuscaTra").dxPopup("instance");
    _popup.hide();
    if (CodTramite != "") {
        CodTramite = CodTramite;
        $('#grdTarmite').dxDataGrid({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.CustomStore({
                    key: "CODTRAMITE",
                    loadMode: "raw",
                    load: function () {
                        return $.getJSON($("#SIM").data("url") + "Tramites/api/TramitesApi/ObtieneTramite?CodTramite=" + CodTramite);
                    }
                })
            }) });
        var GridTramite = $("#grdTarmite").dxDataGrid("instance");
        GridTramite.refresh();
    } else alert("No se ha ingresado el codifo del expediente");
}