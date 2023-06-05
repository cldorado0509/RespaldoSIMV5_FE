var _OpcEstado = 1;
var _OpcTipo = 1;
var CodigoFuncionario = -1;
$(document).ready(function () {
    $("#sbFuncionario").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "CODFUNCIONARIO",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Utilidades/GetListaFuncionarios");
                }
            })
        }),
        displayExpr: "NOMBRE",
        valueExpr: "CODFUNCIONARIO",
        searchEnabled: true,
        noDataText: "No hay datos para mostrar",
        placeholder: "Seleccione",
        onValueChanged: function (data) {
            $("#ldPanel").dxLoadPanel('instance').show();
            CodigoFuncionario = data.value;
            $("#PanelTareas").css("visibility", "visible");
            $.getJSON($('#SIM').data('url') + 'Tramites/TareasFuncionario/ResumenTareasFunc?CodFuncionario=' + CodigoFuncionario).done(function (data) {
                if (data) {
                    $("#NroTareasPendientes").text("Pendientes: " + data.NroTareasPendientes);
                    $("#NroTareasTerminadas").text("Terminadas: " + data.NroTareasTerminadas);
                    $("#NroTareasNoAbiertas").text("No abiertas: " + data.NroTareasNoAbiertas);
                    $("#NroTareasCopiaPendientes").text("Pendientes: " + data.NroTareasCopiaPendientes);
                    $("#NroTareasCopiaTerminadas").text("Terminadas: " + data.NroTareasCopiaTerminadas);
                    $("#NroTareasCopiaNoAbiertas").text("No abiertas: " + data.NroTareasCopiaNoAbiertas);
                    $("#MisTarGrid").dxDataGrid("instance").refresh();

                }
            });
            $("#ldPanel").dxLoadPanel('instance').hide();
        }
    });

    $("#MisTarGrid").dxDataGrid({
        dataSource: misTareasDataSource,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        onContentReady: function (e) {
            var toolbar = e.element.find('.dx-datagrid-header-panel .dx-toolbar').dxToolbar('instance');
            addOpcEstado(toolbar);
            addOpcTipo(toolbar);
        },
        paging: {
            pageSize: 10
        },
        filterRow: {
            visible: true,
            emptyPanelText: 'Arrastre una columna para agrupar'
        },
        'export': {
            enabled: true,
            fileName: 'MisTareas'
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Buscar..."
        },
        selection: {
            mode: 'single'
        },
        remoteOperations: true,
        hoverStateEnabled: true,
        columns: [
            {
                caption: 'Rec.', width: 50, alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.row.data.DEVOLUCION == '1') {
                        if (options.row.data.RECIBIDA == '1') {
                            $("<div>")
                                .append($("<img>", { "src": $("#SIM").data("url") + '/Content/Images/leido-devuelto.png', "width": '16px', "height": '16px' }))
                                .appendTo(container);
                        } else {
                            $("<div>")
                                .append($("<img>", { "src": $("#SIM").data("url") + '/Content/Images/sin-leer-devuelto.png', "width": '16px', "height": '16px' }))
                                .appendTo(container);
                        }
                    }
                    else if (options.row.data.RECIBIDA == '1') {
                        $("<div>")
                            .append($("<img>", { "src": $("#SIM").data("url") + '/Content/Images/leido.png', "width": '16px', "height": '16px' }))
                            .appendTo(container);
                    } else {
                        $("<div>")
                            .append($("<img>", { "src": $("#SIM").data("url") + '/Content/Images/sin-leer.png', "width": '16px', "height": '16px' }))
                            .appendTo(container);
                    }
                }
            },
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
            { dataField: 'COPIA', dataType: 'string', visible: false, allowSearch: false },
            { dataField: 'RECIBIDA', dataType: 'string', visible: false, allowSearch: false },
            { dataField: 'DEVOLUCION', dataType: 'string', visible: false, allowSearch: false },
            { dataField: 'PRIORITARIO', dataType: 'string', visible: false, allowSearch: false },
            { dataField: 'MARCAR', dataType: 'string', visible: false, allowSearch: false },
            { dataField: 'COLOR', dataType: 'string', visible: false, allowSearch: false },
            { dataField: 'CODTAREA', dataType: 'number', visible: false, allowSearch: false },
            { dataField: 'ORDEN', dataType: 'number', visible: false, allowSearch: false },
            {
                width: '10%',
                alignment: 'center',
                caption: 'Detalle Trámite',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'rowfield',
                        text: options.data.CODTRAMITE,
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
        },
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                if (data.CODTRAMITE.length > 0) {
                    CodTramite = Number(data.CODTRAMITE);
                    CodTarea = Number(data.CODTAREA);
                    Orden = Number(data.ORDEN);
                    Copia = Number(data.COPIA);
                    var _Ruta = $('#SIM').data('url') + "Tramites/api/MisTareasApi/EsTramiteBloqueado?CodTramite=" + CodTramite;
                    $.getJSON(_Ruta, function (result, status) {
                        if (status === "success") {
                            Bloqueada = result;
                            if (Bloqueada) {
                                BotonAvanza.dxButton("instance").option("text", "Desbloquear Trámite");
                            } else {
                                BotonAvanza.dxButton("instance").option("text", "Avanzar la actividad");
                            }
                            popupOpciones = {
                                height: 600,
                                width: 1100,
                                title: 'Detalle del trámite',
                                visible: false,
                                contentTemplate: function (container) {
                                    $("<iframe>").attr("src", $('#SIM').data('url') + 'Utilidades/DetalleTramite?popup=true&CodTramite=' + CodTramite + "&Orden=" + Orden).attr("width", "100%").attr("height", "100%").attr("frameborder", "0").attr("scrolling", "0").appendTo(container);
                                }
                            };
                        } else {
                            popupOpciones = {
                                height: 600,
                                width: 1100,
                                title: 'Detalle del trámite',
                                visible: false,
                                contentTemplate: function (container) {
                                    $("<iframe>").attr("src", $('#SIM').data('url') + 'Utilidades/DetalleTramite?popup=true&CodTramite=' + CodTramite + "&Orden=" + Orden).attr("width", "100%").attr("height", "100%").attr("frameborder", "0").attr("scrolling", "0").appendTo(container);
                                }
                            };
                        }
                    });
                }
            }
        }
    });

    function addOpcEstado(toolbar) {
        var items = toolbar.option('items');
        var myItem = DevExpress.data.query(items).filter(function (item) {
            return item.name == 'opcEstado';
        }).toArray();
        if (!myItem.length) {
            items.push({
                location: 'before',
                widget: 'dxRadioGroup',
                name: 'opcEstado',
                options: {
                    items: [{ text: "Pendientes", value: 1 }, { text: "Terminadas", value: 2 }, { text: "Ambas", value: 3 }],
                    layout: "horizontal",
                    valueExpr: "value",
                    value: 1,
                    onValueChanged: function (e) {
                        $("#ldPanel").dxLoadPanel('instance').show();
                        _OpcEstado = e.value;
                        $("#MisTarGrid").dxDataGrid("instance").refresh();
                        $("#ldPanel").dxLoadPanel('instance').hide();
                    }
                }
            });
            toolbar.option('items', items);
        }
    }

    function addOpcTipo(toolbar) {
        var items = toolbar.option('items');
        var myItem = DevExpress.data.query(items).filter(function (item) {
            return item.name == 'opcTipo';
        }).toArray();
        if (!myItem.length) {
            items.push({
                location: 'before',
                widget: 'dxRadioGroup',
                name: 'opcTipo',
                options: {
                    items: [{ text: "Soy responsable", value: 1 }, { text: "De mi conocimiento", value: 2 }],
                    layout: "horizontal",
                    valueExpr: "value",
                    value: 1,
                    onValueChanged: function (e) {
                        $("#ldPanel").dxLoadPanel('instance').show();
                        _OpcTipo = e.value;
                        $("#MisTarGrid").dxDataGrid("instance").refresh();
                        $("#ldPanel").dxLoadPanel('instance').hide();
                    }
                }
            });
            toolbar.option('items', items);
        }
    }

    $("#ldPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
        shadingColor: "rgba(0,0,0,0.4)",
    });
});

var misTareasDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"INICIOTAREA","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'Tramites/api/MisTareasApi/MisTareas', {
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
            estado: _OpcEstado,
            tipo: _OpcTipo
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});